using eFormSqlController;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using NUnit.Framework;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {
        protected MicrotingDbMs DbContext;
        protected string ConnectionString => @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests";

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


            if (!userName.Contains("USER_NAME"))
            {
                var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(applicationId, password, directoryId, AzureEnvironment.AzureGermanCloud);
                var azure = Azure
                    .Configure()
                    .Authenticate(credentials)
                    .WithDefaultSubscription();

                var sqlServer = azure.SqlServers.GetById(databaseServerId);
                sqlServer.Databases.Define(databaseName: databaseName).Create();
            }
            DbContext.Database.CreateIfNotExists();

            DbContext.Database.Initialize(false);

            DoSetup();
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Database.Delete();
            DbContext.Dispose();

        }

        public virtual void DoSetup() { }

    }
}
