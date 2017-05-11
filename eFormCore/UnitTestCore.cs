using eFormShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormCore
{
    public class UnitTestCore
    {
        #region var
        Core core;
        Tools t = new Tools();
        #endregion

        #region con
        public UnitTestCore(Core core)
        {
            this.core = core;
        }
        #endregion

        public void CaseComplet(string microtingUId, string checkUId)
        {
            core.UnitTest_CaseComplet(microtingUId, checkUId);
        }

        public void CaseDelete(string microtingUId)
        {
            core.UnitTest_CaseDelete(microtingUId);
        }

        public void EventLog(string text)
        {
            core.UnitTest_TriggerLog(text);
        }
    }
}
