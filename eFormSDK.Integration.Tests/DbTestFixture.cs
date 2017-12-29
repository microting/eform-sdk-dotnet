using eFormSqlController;
using NUnit.Framework;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {
        protected MicrotingDbMs DbContext;
        protected string ConnectionString => @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests";

        [SetUp]
        public void Setup()
        {
            DbContext = new MicrotingDbMs(ConnectionString);
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
