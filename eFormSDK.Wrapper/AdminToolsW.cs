using eFormCore;
using eFormSDK.Wrapper;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microting.eForm.Wrapper
{
    public static class AdminToolsW 
    {
        private static AdminTools program;
      
        [DllExport("AdminTools_Create")]
        public static int AdminTools_Create([MarshalAs(UnmanagedType.BStr)]String serverConnectionString)
        {
            int result = 0;
            try
            {
                program = new AdminTools(serverConnectionString);
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }

            return result;
        }

        [DllExport("AdminTools_DbSetup")]
        public static int AdminTools_DbSetup([MarshalAs(UnmanagedType.BStr)]String token,
            [MarshalAs(UnmanagedType.BStr)] ref string reply)
        {
            int result = 0;
            try
            {
                reply = program.DbSetup(token);
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }

            return result;
        }

        [DllExport("AdminTools_DbSetupCompleted")]
        public static int AdminTools_DbSetupCompleted([MarshalAs(UnmanagedType.BStr)] ref string reply)
        {
            int result = 0;
            try
            {
                List<string> checkResult = program.DbSetupCompleted();
                if (checkResult.Count == 1)
                {
                    if (checkResult[0] == "NO SETTINGS PRESENT, NEEDS PRIMING!")
                        reply = checkResult[0];
                    else
                        reply = "Settings table is incomplete, please fix the following settings: " + String.Join(",", checkResult);
                }
            }
            catch (Exception ex)
            {
                LastError.Value = ex.Message;
                result = 1;
            }

            return result;
        }

    }
}
