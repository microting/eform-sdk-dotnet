using eFormCore;
using eFormSqlController;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {

        protected MicrotingDbAnySql DbContext;
        protected string ConnectionString;

        private MicrotingDbAnySql GetContext(string connectionStr)
        {
            
            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();
         
            if (ConnectionString.ToLower().Contains("convert zero datetime"))
            {
                dbContextOptionsBuilder.UseMySql(connectionStr);
            }
            else
            {
                dbContextOptionsBuilder.UseSqlServer(connectionStr);          
            }
            dbContextOptionsBuilder.UseLazyLoadingProxies(true);
            return new MicrotingDbAnySql(dbContextOptionsBuilder.Options);            

        }

        [SetUp]
        public void Setup()
        {

//            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
//            {
//                ConnectionString = @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests;Integrated Security=True";
//            }
//            else
//            {
                ConnectionString = @"Server = localhost; port = 3306; Database = eformsdk-tests; user = root; Convert Zero Datetime = true;";
//            }

            DbContext = GetContext(ConnectionString);


            DbContext.Database.SetCommandTimeout(300);

            try
            {
                ClearDb();
            }
            catch
            {
            }
            try
            {
                Core core = new Core();
                core.StartSqlOnly(ConnectionString);
                core.Close();
            } catch
            {
                AdminTools adminTools = new AdminTools(ConnectionString);
                adminTools.DbSetup("abc1234567890abc1234567890abcdef");
            }

            DoSetup();
        }
      
        [TearDown]
        public void TearDown()
        {

            ClearDb();

            ClearFile();

            DbContext.Dispose();
        }

        public void ClearDb()
        {

            List<string> modelNames = new List<string>();
            modelNames.Add("case_versions");
            modelNames.Add("cases");
            modelNames.Add("field_value_versions");
            modelNames.Add("field_values");
            modelNames.Add("field_versions");
            modelNames.Add("fields");
            modelNames.Add("check_list_site_versions");
            modelNames.Add("check_list_sites");
            modelNames.Add("check_list_value_versions");
            modelNames.Add("check_list_values");
            modelNames.Add("taggings");
            modelNames.Add("tagging_versions");
            modelNames.Add("tags");
            modelNames.Add("tag_versions");
            modelNames.Add("check_list_versions");
            modelNames.Add("check_lists");
            modelNames.Add("entity_group_versions");
            modelNames.Add("entity_groups");
            modelNames.Add("entity_item_versions");
            modelNames.Add("entity_items");
            modelNames.Add("log_exceptions");
            modelNames.Add("logs");
            modelNames.Add("notifications");
            modelNames.Add("settings");
            modelNames.Add("unit_versions");
            modelNames.Add("units");
            modelNames.Add("site_worker_versions");
            modelNames.Add("site_workers");
            modelNames.Add("worker_versions");
            modelNames.Add("workers");
            modelNames.Add("site_versions");
            modelNames.Add("sites");
            modelNames.Add("uploaded_data");
            modelNames.Add("uploaded_data_versions");
            modelNames.Add("field_types");
            modelNames.Add("survey_configurations");
            modelNames.Add("survey_configuration_versions");
            modelNames.Add("site_survey_configurations");
            modelNames.Add("site_survey_configuration_versions");
            modelNames.Add("languages");
            modelNames.Add("language_versions");
            modelNames.Add("question_sets");
            modelNames.Add("question_set_versions");

            foreach (var modelName in modelNames)
            {
                try
                {
                    string sqlCmd = string.Empty;
                    if(DbContext.Database.IsMySql())
                    {
                        sqlCmd = string.Format("SET FOREIGN_KEY_CHECKS = 0;TRUNCATE `{0}`.`{1}`", "eformsdk-tests", modelName);
                    }
                    else
                    {
                        sqlCmd = string.Format("DELETE FROM [{0}]", modelName);
                    }
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                    DbContext.Database.ExecuteSqlCommand(sqlCmd);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private string path;

        public void ClearFile()
        {
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");

            string picturePath;


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                picturePath = path + @"\output\dataFolder\picture\Deleted";
            }
            else
            {
                picturePath = path + @"/output/dataFolder/picture/Deleted";
            }

            DirectoryInfo diPic = new DirectoryInfo(picturePath);

            try
            {
                foreach (FileInfo file in diPic.GetFiles())
                {
                    file.Delete();
                }
            }
            catch { }


        }
        public virtual void DoSetup() { }

    }
}
