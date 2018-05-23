using eFormCore;
using eFormSqlController;
//using Microsoft.Azure.Management.Fluent;
//using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.IO;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {
        protected MicrotingDbMs DbContext;
        protected string ConnectionString => @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests;Integrated Security=True";

        private static string userName = "__USER_NAME__";
        private static string password = "__PASSWORD__";
        private static string databaseName = "__DBNAME__";
        private static string databaseServerId = "__DB_SERVER_ID__";
        private static string directoryId = "__DIRECTORY_ID__";
        private static string applicationId = "__APPLICATION_ID__";


        [SetUp]
        public void Setup()
        {            
            DbContext = new MicrotingDbMs(ConnectionString);
            DbContext.Database.CommandTimeout = 300;

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
            DbContext.Database.CreateIfNotExists();

            DbContext.Database.Initialize(true);

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
            var metadata = ((IObjectContextAdapter)DbContext).ObjectContext.MetadataWorkspace.GetItems(DataSpace.SSpace);

            List<string> tables = new List<string>();
            foreach (var item in metadata)
            {
                if (item.ToString().Contains("CodeFirstDatabaseSchema"))
                {
                    tables.Add(item.ToString().Replace("CodeFirstDatabaseSchema.", ""));
                }
            }

            foreach (string tableName in tables)
            {
                try
                {
                    DbContext.Database.ExecuteSqlCommand("DELETE FROM [" + tableName + "]");
                }
                catch 
                { }

            }
        }
        private string path;
        
        public void ClearFile()
        {
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");

            string picturePath =path + @"\output\dataFolder\picture\Deleted";

        DirectoryInfo diPic = new DirectoryInfo(picturePath);

            foreach (FileInfo file in diPic.GetFiles())
            {
                file.Delete();
            }

        }
        public virtual void DoSetup() { }

    }
}
