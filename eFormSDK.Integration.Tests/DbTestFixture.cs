using eFormSqlController;
//using Microsoft.WindowsAzure;
using NUnit.Framework;
using System;
//using System.Security.Cryptography.X509Certificates;

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
            DbContext.Database.CreateIfNotExists();

            DbContext.Database.Initialize(false);
            DbContext.Dispose();

        }

        public virtual void DoSetup() { }

        //static SubscriptionCloudCredentials getCredentials()
        //{
        //    string SubscriptionId = "";
        //    string CertificateBase64String = "";
        //    return new CertificateCloudCredentials(SubscriptionId, new X509Certificate2(Convert.FromBase64String(CertificateBase64String))); 
        //}
    }
}
