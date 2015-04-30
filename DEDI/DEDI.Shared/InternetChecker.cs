﻿using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime;
using System.Runtime.InteropServices;

public class InternetChecker
{
    [System.Runtime.InteropServices.DllImport("wininet.dll")]
    private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

    //Creating a function that uses the API function...
    public static bool IsConnectedToInternet()
    {
        int Desc;
        return InternetGetConnectedState(out Desc, 0);
    }

}