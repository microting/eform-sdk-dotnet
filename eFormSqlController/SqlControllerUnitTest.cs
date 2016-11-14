using eFormRequest;
using Trools;

using System;
using System.Collections.Generic;
using System.Linq;

namespace eFormSqlController
{
    public class SqlControllerUnitTest
    {
        #region var
        object _lockQuery = new object();

        string connectionStr;
        int userId;

        Tools t = new Tools();
        #endregion

        #region con
        public SqlControllerUnitTest(string connectionString, int userId)
        {
            connectionStr = connectionString;
            this.userId = userId;
        }
        #endregion

        #region public
        public List<string> CheckCase()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<cases> lstCases = db.cases.Where(x => x.workflow_state == "created").ToList();
                    foreach (cases aCase in lstCases)
                    {
                        lstMUId.Add(aCase.microting_api_id);
                    }

                    List<check_list_sites> lstCLS = db.check_list_sites.Where(x => x.workflow_state == "created").ToList();
                    foreach (check_list_sites cLS in lstCLS)
                    {
                        lstMUId.Add(cLS.microting_check_list_uuid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckCase failed", ex);
            }
        }

        public List<string> CleanActiveCases()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<cases> lstCases = db.cases.Where(x => x.workflow_state == "created").ToList();
                    foreach (cases aCase in lstCases)
                    {
                        lstMUId.Add(aCase.microting_api_id);
                    }

                    List<check_list_sites> lstCLS = db.check_list_sites.Where(x => x.workflow_state == "created").ToList();
                    foreach (check_list_sites cLS in lstCLS)
                    {
                        lstMUId.Add(cLS.microting_check_list_uuid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CleanActiveCases failed", ex);
            }
        }

        public bool CleanUpDB()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[case_versions]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[cases]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[check_list_site_versions]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[check_list_sites]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[check_list_value_versions]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[check_list_values]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[check_list_versions]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[check_lists]");
                    //---
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[field_types]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[field_value_versions]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[field_values]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[field_versions]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[fields]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[notifications]");
                    //---
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[site_versions]");
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[sites]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[uploaded_data_versions]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[uploaded_data]");
                    //---
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[user_versions]");
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [microting].[users]");
                    //---

                    return true;
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.ToString();
                return false;
            }
        }
        #endregion
    }
}
