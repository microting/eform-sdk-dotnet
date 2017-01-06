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
        Tools t = new Tools();
        #endregion

        #region con
        public SqlControllerUnitTest(string connectionString)
        {
            connectionStr = connectionString;
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
                        lstMUId.Add(aCase.microting_uid);
                    }

                    List<check_list_sites> lstCLS = db.check_list_sites.Where(x => x.workflow_state == "created").ToList();
                    foreach (check_list_sites cLS in lstCLS)
                    {
                        lstMUId.Add(cLS.microting_uid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckCase failed", ex);
            }
        }

        public List<string> FindAllActiveCases()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<cases> lstCases = db.cases.Where(x => x.workflow_state == "created").ToList();
                    foreach (cases aCase in lstCases)
                    {
                        lstMUId.Add(aCase.microting_uid);
                    }

                    List<check_list_sites> lstCLS = db.check_list_sites.Where(x => x.workflow_state == "created").ToList();
                    foreach (check_list_sites cLS in lstCLS)
                    {
                        lstMUId.Add(cLS.microting_uid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FindAllActiveCases failed", ex);
            }
        }

        public bool CleanUpDB()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_cases]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[cases]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_check_list_sites]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[check_list_sites]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_check_list_values]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[check_list_values]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_check_lists]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[check_lists]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_entity_groups]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[entity_groups]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_entity_items]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[entity_items]");
                    //---
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[field_types]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_field_values]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[field_values]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_fields]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[fields]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[notifications]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[outlook]");
                    //---
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[site_versions]");
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[sites]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_data_uploaded]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[data_uploaded]");
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
