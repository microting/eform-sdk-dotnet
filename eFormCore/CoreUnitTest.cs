using eFormShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eFormCore
{
    public class CoreUnitTest
    {
        #region var
        Core core;
        Tools t = new Tools();
        #endregion

        #region con
        public CoreUnitTest(Core core)
        {
            this.core = core;
            core.UnitTest_SetUnittest();
        }
        #endregion

        public void CaseComplet(string microtingUId, string checkUId, int workerUId, int unitUId)
        {
            core.UnitTest_CaseComplet(microtingUId, checkUId, workerUId, unitUId);
        }

        public void CaseDelete(string microtingUId)
        {
            core.UnitTest_CaseDelete(microtingUId);
        }

        public void Close()
        {
            Thread closeCore
                = new Thread(() => core.Close());
            closeCore.Start();
        }
    }
}