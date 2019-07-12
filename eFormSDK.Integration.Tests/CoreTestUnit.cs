using eFormCore;
using eFormShared;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTestUnit : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
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
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");
            sut.SetSdkSetting(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SetSdkSetting(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SetSdkSetting(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        #region unit
        [Test]
        public void Core_Advanced_UnitRequestOtp_SetsNewOtp()
        {
            // Arrange
            sites site = testHelpers.CreateSite("test site 1", 1313);
            units unit = testHelpers.CreateUnit(564646, 0, site, 0);

            // Act
            sut.Advanced_UnitRequestOtp((int)unit.MicrotingUid);

            // Assert
            List<units> matches = DbContext.units.AsNoTracking().ToList();

            Assert.NotNull(matches);
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(558877, matches[0].OtpCode);
        }
        #endregion

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