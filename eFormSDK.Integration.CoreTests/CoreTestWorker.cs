using System;
using System.IO;
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using NUnit.Framework;

namespace eFormSDK.Integration.CoreTests
{
    [TestFixture]
    public class CoreTestWorker : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {
            if (sut == null)
            {
                sut = new Core();
                sut.HandleCaseCreated += EventCaseCreated;
                sut.HandleCaseRetrived += EventCaseRetrived;
                sut.HandleCaseCompleted += EventCaseCompleted;
                sut.HandleCaseDeleted += EventCaseDeleted;
                sut.HandleFileDownloaded += EventFileDownloaded;
                sut.HandleSiteActivated += EventSiteActivated;
                await sut.StartSqlOnly(ConnectionString);
            }
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers();
            //await sut.StartLog(new CoreBase());
        }


        // Test for Advanced_WorkerCreate

        // Test for Advanced_SiteWorkerCreate


        #region eventhandlers
        public void EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }
        #endregion
    }

}