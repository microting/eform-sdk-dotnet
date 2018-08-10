using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");
            sut.SetPicturePath(path + @"\output\dataFolder\picture\");
            sut.SetPdfPath(path + @"\output\dataFolder\pdf\");
            sut.SetJasperPath(path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            testHelperReturnXml = new TestHelperReturnXML();
            //sut.StartLog(new CoreBase());
        }


        

        
        
        


        [Test]
        public void Core_CheckStatusByMicrotingUid_DoesCreateCaseAndFieldValues()
        {
            //Arrance
            string microtingUuid = testHelperReturnXml.CreateMultiPictureXMLResult(true);

            //Act
            sut.CheckStatusByMicrotingUid(microtingUuid);

            //Assert
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
        //Arrance

        //Act

        //Assert
    }
}
