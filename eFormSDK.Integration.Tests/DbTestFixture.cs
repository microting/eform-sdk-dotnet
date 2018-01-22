using eFormSqlController;
using NUnit.Framework;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {
        protected MicrotingDbMs DbContext;
        protected string ConnectionString => @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests";
        private static string authority = "https://login.microsoftonline.com/REPLACE_ME_GUID/oauth2/token";
        private static string azureSubscriptionId = "__AZURE_SUBSCRIPTION_ID__"; /* Azure Subscription ID */

        private static string location = "Data Center Location"; /* Location of server and location for database (eg. Australia East) */
        private static string edition = "Standard"; /* Databse Edition (eg. Standard)*/
        private static string requestedServiceObjectName = "Performance Level"; /* Name of Service Object (eg. S0) */

        private static string resourceGroupName = "Resource Group Name"; /* Name of Resource Group containing SQL Server */
        private static string serverName = "Server Name"; /* Name of SQL Server */
        private static string databaseName = "Database Name"; /* Name of Database */

        private static string domainName = "domain.name.com"; /* Tenant ID or AAD domain */

        /* Authentication variables from Azure Active Directory (AAD) */
        private static string clientId = "__CLIENT_ID__"; /* Active Directory Client ID */
        private static string clientSecret = "__CLIENT_SECRET__";
        private static string DirectoryId = "__DIRECTORY_ID__";
        private static string ApplicationId = "__APPLICATION_ID__";


        [SetUp]
        public void Setup()
        {
            DbContext = new MicrotingDbMs(ConnectionString);

            if (clientId != "__CLIENT_ID__")
            {

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
