using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTest : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private TestHelperReturnXML testHelperReturnXml;
        private string path;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            sut.StartSqlOnly(ConnectionString);
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:", "");
            sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers();
            testHelperReturnXml = new TestHelperReturnXML();
            //sut.StartLog(new CoreBase());
        }


        

        
        
        


        [Test]
        public void Core_CheckStatusByMicrotingUid_DoesCreateCaseAndFieldValues()
        {
            // Arrange
            string microtingUuid = testHelperReturnXml.CreateMultiPictureXMLResult(true);
            
            // Act
            sut.CheckStatusByMicrotingUid(int.Parse(microtingUuid));

            // Assert
            List<cases> caseMatches = DbContext.cases.AsNoTracking().ToList();
            List<uploaded_data> udMatches = DbContext.uploaded_data.AsNoTracking().ToList();
            List<field_values> fvMatches = DbContext.field_values.AsNoTracking().ToList();

            Assert.NotNull(caseMatches);
            Assert.NotNull(udMatches);
            Assert.NotNull(fvMatches);
            Assert.AreEqual(3, caseMatches.Count());
            Assert.AreEqual(5, udMatches.Count());
            Assert.AreEqual(5, fvMatches.Count());

        }

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
        // Arrange

        // Act

        // Assert
    }
}
