using eFormRequest;
using eFormResponse;
using Trools;

using System;
using System.Collections.Generic;
using System.Linq;

namespace eFormSqlController
{
    public class SqlControllerExtended
    {
        #region var
        object _lockQuery = new object();

        string connectionStr;
        int userId;
        Tools t = new Tools();
        #endregion

        #region con
        public SqlControllerExtended(string connectionString, int userId)
        {
            connectionStr = connectionString;
            this.userId = userId;
        }
        #endregion

        #region public
        public int                  CaseCountResponses(string caseUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    int count = db.cases.Where(x => x.serialized_values == caseUId && x.done_by_user_id != null).ToList().Count();
                    return count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseCountResponses failed", ex);
            }
        }
        #endregion
    }
}
