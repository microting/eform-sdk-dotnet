using eFormCore;
using eFormSqlController;
//using Microsoft.Azure.Management.Fluent;
//using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microting.eForm;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {
        // set true for MS SQL Server Database
        // set false for MySQL Datbase
      //  const bool IsMSSQL = true;//SQL Type
        const string dbName = "eformsdk-tests";
        
        protected MicrotingDbAnySql DbContext;
       
        //string mySQLConnStringFormat = "Server = localhost; port = 3306; Database = {0}; user = eform; password = eform; Convert Zero Datetime = true;";
        //string msSQLConnStringFormat = @"data source=localhost;Initial catalog={0};Integrated Security=True";

        protected string ConnectionString => string.Format(DbConfig.ConnectionString, dbName);

        private static string userName = "__USER_NAME__";
        private static string password = "__PASSWORD__";
        private static string databaseName = "__DBNAME__";
        private static string databaseServerId = "__DB_SERVER_ID__";
        private static string directoryId = "__DIRECTORY_ID__";
        private static string applicationId = "__APPLICATION_ID__";

        private MicrotingDbAnySql GetContext(string connectionStr)
        {
            
            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();
         
            if (DbConfig.IsMSSQL)
            {
                dbContextOptionsBuilder.UseSqlServer(connectionStr);               
            }
            else
            {
                dbContextOptionsBuilder.UseMySql(connectionStr);              
            }
            dbContextOptionsBuilder.UseLazyLoadingProxies(true);
            return new MicrotingDbAnySql(dbContextOptionsBuilder.Options);            

        }

        [SetUp]
        public void Setup()
        {

            //DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();
            //dbContextOptionsBuilder.UseLazyLoadingProxies(true);
            //if (IsMSSQL)
            //{
            //    dbContextOptionsBuilder.UseSqlServer(ConnectionString);
            //}
            //else
            //{
            //    dbContextOptionsBuilder.UseMySql(ConnectionString);
            //}
            DbContext = GetContext(ConnectionString);

          //  DbContext = new MicrotingDbMs(dbContextOptionsBuilder.Options);

            //DbContext = new MicrotingDbMs(ConnectionString);
            DbContext.Database.SetCommandTimeout(300);

            try
            {
                ClearDb();
            }
            catch
            {
                Core core = new Core();
                core.StartSqlOnly(ConnectionString);
                core.Close();
            }

            //if (!userName.Contains("USER_NAME"))
            //{
            //    var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(applicationId, password, directoryId, AzureEnvironment.AzureGlobalCloud);
            //    var azure = Azure
            //        .Configure()
            //        .Authenticate(credentials)
            //        .WithDefaultSubscription();

            //    var sqlServer = azure.SqlServers.GetById(databaseServerId);
            //    sqlServer.Databases.Define(databaseName: databaseName).Create();
            //}
            //DbContext.Database.CreateIfNotExists();

            //DbContext.Database.Initialize(true);

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


            foreach (var modelName in modelNames)
            {
                //Console.WriteLine(modelName.Name);
                try
                {
                    string sqlCmd = string.Empty;
                    if(!DbConfig.IsMSSQL)
                    {
                        sqlCmd = string.Format("DELETE FROM `{0}`.`{1}`", dbName, modelName);                       
                    }
                    else
                    {
                        sqlCmd = string.Format("DELETE FROM [{0}]", modelName);
                    }
                    DbContext.Database.ExecuteSqlCommand(sqlCmd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //TODO! THIS part need to be redone in some form in EF Core!
            //var metadata = ((IObjectContextAdapter)DbContext).ObjectContext.MetadataWorkspace.GetItems(DataSpace.SSpace);

            //List<string> tables = new List<string>();
            //foreach (var item in metadata)
            //{
            //    if (item.ToString().Contains("CodeFirstDatabaseSchema"))
            //    {
            //        tables.Add(item.ToString().Replace("CodeFirstDatabaseSchema.", ""));
            //    }
            //}

            //foreach (string tableName in tables)
            //{
            //    try
            //    {
            //        DbContext.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]");
            //    }
            //    catch 
            //    { }

            //}
        }
        private string path;
        
        public void ClearFile()
        {
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");

            string picturePath =path + @"\output\dataFolder\picture\Deleted";

        DirectoryInfo diPic = new DirectoryInfo(picturePath);

            try {
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
