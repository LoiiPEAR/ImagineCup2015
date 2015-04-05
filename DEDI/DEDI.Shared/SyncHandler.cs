﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Windows.UI.Popups;

namespace DEDI
{
    public class SyncHandler : IMobileServiceSyncHandler
    {
        MobileServiceClient client;
        const string LOCAL_VERSION = "Use local version";
        const string SERVER_VERSION = "Use server version";

        public SyncHandler(MobileServiceClient client)
        {
            this.client = client;
        }

        public virtual Task OnPushCompleteAsync(MobileServicePushCompletionResult result)
        {
            return Task.FromResult(0);
        }

        public virtual async Task<JObject> ExecuteTableOperationAsync(IMobileServiceTableOperation operation)
        {
            MobileServiceInvalidOperationException error;
            Func<Task<JObject>> tryOperation = operation.ExecuteAsync;

            do
            {
                error = null;

                try
                {
                    JObject result = await operation.ExecuteAsync();
                    return result;
                }
                catch (MobileServiceConflictException ex)
                {
                    error = ex;
                }
                catch (MobileServicePreconditionFailedException ex)
                {
                    error = ex;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    throw e;
                }

                if (error != null)
                {
                    var localItem = operation.Item.ToObject<Health_Worker>();
                    var serverValue = error.Value;
                    if (serverValue == null) // 409 doesn't return the server item
                    {
                        serverValue = await operation.Table.LookupAsync(localItem.id) as JObject;
                    }
                    var serverItem = serverValue.ToObject<Health_Worker>();


                    if (serverItem.Complete == localItem.Complete && serverItem.fname == localItem.fname)
                    {
                        // items are same so we can ignore the conflict
                        return serverValue;
                    }

                    IUICommand command = await ShowConflictDialog(localItem, serverValue);
                    if (command.Label == LOCAL_VERSION)
                    {
                        // Overwrite the server version and try the operation again by continuing the loop
                        operation.Item[MobileServiceSystemColumns.Version] = serverValue[MobileServiceSystemColumns.Version];
                        if (error is MobileServiceConflictException) // change operation from Insert to Update
                        {
                            tryOperation = async () => await operation.Table.UpdateAsync(operation.Item) as JObject;
                        }
                        continue;
                    }
                    else if (command.Label == SERVER_VERSION)
                    {
                        return (JObject)serverValue;
                    }
                    else
                    {
                        operation.AbortPush();
                    }
                }
            } while (error != null);

            return null;
        }

        private async Task<IUICommand> ShowConflictDialog(Health_Worker localItem, JObject serverValue)
        {
            var dialog = new MessageDialog(
                "How do you want to resolve this conflict?\n\n" + "Local item: \n" + localItem +
                "\n\nServer item:\n" + serverValue.ToObject<Health_Worker>(),
                title: "Conflict between local and server versions");

            dialog.Commands.Add(new UICommand(LOCAL_VERSION));
            dialog.Commands.Add(new UICommand(SERVER_VERSION));

            // Windows Phone not supporting 3 command MessageDialog
            //dialog.Commands.Add(new UICommand("Cancel"));

            return await dialog.ShowAsync();
        }
    }
}