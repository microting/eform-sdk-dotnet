using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace eFormSDK.Wrapper
{
    public static class LastError
    {
        public static string Value
        {
            get;
            set;
        }

        [DllExport("GetLastError")]
        public static void GetLastError([MarshalAs(UnmanagedType.BStr)] ref string value)
        {
            value = Value;
        }
    }
}
