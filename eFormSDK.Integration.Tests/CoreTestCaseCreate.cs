using eFormCore;
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
using Microting.eForm.Infrastructure.Models;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTestCaseCreate : DbTestFixture
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
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:", "");
            sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        [Test]//needs http mock done
        public void Core_Case_CaseCreate_CreatesCase()
        {
            // Arrange
            #region Template1
            DateTime c1_Ca = DateTime.Now;
            DateTime c1_Ua = DateTime.Now;

            check_lists cl1 = testHelpers.CreateTemplate(c1_Ca, c1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = testHelpers.CreateSite("SiteName", 88);

            #endregion


            CoreElement CElement = new CoreElement();
            //CElement.ElementList = new List<Element>();

            MainElement main = new MainElement(1, "label1", 1, "FolderWithList",
                0, DateTime.Now, DateTime.Now.AddDays(2),
                "Swahili", false, false, false, false, 
                "Type1", "Push", "TextForBody", false,
                CElement.ElementList, "Blue");
            // Act
            var match = sut.CaseCreate(main, "", (int)site.MicrotingUid);
            // Assert
             Assert.NotNull(match);
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
    }

}