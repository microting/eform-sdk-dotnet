using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestPublicSetting : DbTestFixture
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
            sut.SetPicturePath(path + @"\output\dataFolder\picture\");
            sut.SetPdfPath(path + @"\output\dataFolder\pdf\");
            sut.SetJasperPath(path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }


        #region Public Setting

        [Test]
        public void SQL_Setting_SettingCreateDefault_CreatesDetault()
        {
            // Arrance

            // Act
            var match = sut.SettingCreateDefaults();
            // Assert

            Assert.True(match);

        }


        [Test]
        public void SQL_Setting_SetingCreate_CreatesSetting()
        {
            // Arrance

            // Act
            var match1 = sut.SettingCreate(Settings.firstRunDone);
            var match2 = sut.SettingCreate(Settings.logLevel);
            var match3 = sut.SettingCreate(Settings.logLimit);
            var match4 = sut.SettingCreate(Settings.knownSitesDone);
            var match5 = sut.SettingCreate(Settings.fileLocationPicture);
            var match6 = sut.SettingCreate(Settings.fileLocationPdf);
            var match7 = sut.SettingCreate(Settings.fileLocationJasper);
            var match8 = sut.SettingCreate(Settings.token);
            var match9 = sut.SettingCreate(Settings.comAddressBasic);
            var match10 = sut.SettingCreate(Settings.comAddressApi);
            var match11 = sut.SettingCreate(Settings.comAddressPdfUpload);
            var match12 = sut.SettingCreate(Settings.comOrganizationId);
            var match13 = sut.SettingCreate(Settings.awsAccessKeyId);
            var match14 = sut.SettingCreate(Settings.awsSecretAccessKey);
            var match15 = sut.SettingCreate(Settings.awsEndPoint);
            var match16 = sut.SettingCreate(Settings.unitLicenseNumber);
            var match17 = sut.SettingCreate(Settings.httpServerAddress);

            // Assert
            Assert.True(match1);
            Assert.True(match2);
            Assert.True(match3);
            Assert.True(match4);
            Assert.True(match5);
            Assert.True(match6);
            Assert.True(match7);
            Assert.True(match8);
            Assert.True(match9);
            Assert.True(match10);
            Assert.True(match11);
            Assert.True(match12);
            Assert.True(match13);
            Assert.True(match14);
            Assert.True(match15);
            Assert.True(match16);
            Assert.True(match17);


        }


        [Test]
        public void SQL_Setting_SettingRead_ReadsSetting()
        {
            // Arrance

            // Act
            var match1 = sut.SettingRead(Settings.awsAccessKeyId);
            var match2 = sut.SettingRead(Settings.awsEndPoint);
            var match3 = sut.SettingRead(Settings.awsSecretAccessKey);
            var match4 = sut.SettingRead(Settings.comAddressApi);
            var match5 = sut.SettingRead(Settings.comAddressBasic);
            var match6 = sut.SettingRead(Settings.comAddressPdfUpload);
            var match7 = sut.SettingRead(Settings.comOrganizationId);
            var match8 = sut.SettingRead(Settings.fileLocationJasper);
            var match9 = sut.SettingRead(Settings.fileLocationPdf);
            var match10 = sut.SettingRead(Settings.fileLocationPicture);
            var match11 = sut.SettingRead(Settings.firstRunDone);
            var match12 = sut.SettingRead(Settings.httpServerAddress);
            var match13 = sut.SettingRead(Settings.knownSitesDone);
            var match14 = sut.SettingRead(Settings.logLevel);
            var match15 = sut.SettingRead(Settings.logLimit);
            var match16 = sut.SettingRead(Settings.token);
            var match17 = sut.SettingRead(Settings.unitLicenseNumber);


            // Assert
            Assert.AreEqual(match1, "XXX");
            Assert.AreEqual(match2, "XXX");
            Assert.AreEqual(match3, "XXX");
            Assert.AreEqual(match4, "https://xxxxxx.xxxxxx.com");
            Assert.AreEqual(match5, "https://basic.microting.com");
            Assert.AreEqual(match6, "https://xxxxxx.xxxxxx.com");
            Assert.AreEqual(match7, "0");
            Assert.AreEqual(match8, path + @"\output\dataFolder\reports\");
            Assert.AreEqual(match9, path + @"\output\dataFolder\pdf\");
            Assert.AreEqual(match10, path + @"\output\dataFolder\picture\");
            Assert.AreEqual(match11, "false");
            Assert.AreEqual(match12, "http://localhost:3000");
            Assert.AreEqual(match13, "false");
            Assert.AreEqual(match14, "4");
            Assert.AreEqual(match15, "25000");
            Assert.AreEqual(match16, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            Assert.AreEqual(match17, "0");


        }


        [Test]
        public void SQL_Setting_SettingUpdate_UpdatesSetting()
        {
            // Arrance

            // Act
            //List<settings> setting = DbContext.settings.AsNoTracking().ToList();
            sut.SettingUpdate(Settings.token, "player");
            var match = DbContext.settings.AsNoTracking().ToList();

            // Assert
            //Assert.NotNull(setting);
            Assert.AreEqual(match[7].value, "player");
        }


        [Test]
        public void SQL_Setting_SettingCheckAll_AllSettingsAreCreated()
        {
            // Arrance

            // Act
            List<settings> setting = DbContext.settings.AsNoTracking().ToList();
            sut.SettingCheckAll();
            var match = DbContext.settings.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(match);
            #region Name

            Assert.AreEqual(match[0].name, "firstRunDone");
            Assert.AreEqual(match[1].name, "logLevel");
            Assert.AreEqual(match[2].name, "logLimit");
            Assert.AreEqual(match[3].name, "knownSitesDone");
            Assert.AreEqual(match[4].name, "fileLocationPicture");
            Assert.AreEqual(match[5].name, "fileLocationPdf");
            Assert.AreEqual(match[6].name, "fileLocationJasper");
            Assert.AreEqual(match[7].name, "token");
            Assert.AreEqual(match[8].name, "comAddressBasic");
            Assert.AreEqual(match[9].name, "comAddressApi");
            Assert.AreEqual(match[10].name, "comAddressPdfUpload");
            Assert.AreEqual(match[11].name, "comOrganizationId");
            Assert.AreEqual(match[12].name, "awsAccessKeyId");
            Assert.AreEqual(match[13].name, "awsSecretAccessKey");
            Assert.AreEqual(match[14].name, "awsEndPoint");
            Assert.AreEqual(match[15].name, "unitLicenseNumber");
            Assert.AreEqual(match[16].name, "httpServerAddress");
            #endregion

            #region Value
            Assert.AreEqual(match[0].value, "false");
            Assert.AreEqual(match[1].value, "4");
            Assert.AreEqual(match[2].value, "25000");
            Assert.AreEqual(match[3].value, "false");
            Assert.AreEqual(match[4].value, path + @"\output\dataFolder\picture\");
            Assert.AreEqual(match[5].value, path + @"\output\dataFolder\pdf\");
            Assert.AreEqual(match[6].value, path + @"\output\dataFolder\reports\");
            Assert.AreEqual(match[7].value, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            Assert.AreEqual(match[8].value, "https://basic.microting.com");
            Assert.AreEqual(match[9].value, "https://xxxxxx.xxxxxx.com");
            Assert.AreEqual(match[10].value, "https://xxxxxx.xxxxxx.com");
            Assert.AreEqual(match[11].value, "0");
            Assert.AreEqual(match[12].value, "XXX");
            Assert.AreEqual(match[13].value, "XXX");
            Assert.AreEqual(match[14].value, "XXX");
            Assert.AreEqual(match[15].value, "0");
            Assert.AreEqual(match[16].value, "http://localhost:3000");
            #endregion

            #region ID's
            Assert.AreEqual(match[0].id, 1);
            Assert.AreEqual(match[1].id, 2);
            Assert.AreEqual(match[2].id, 3);
            Assert.AreEqual(match[3].id, 4);
            Assert.AreEqual(match[4].id, 5);
            Assert.AreEqual(match[5].id, 6);
            Assert.AreEqual(match[6].id, 7);
            Assert.AreEqual(match[7].id, 8);
            Assert.AreEqual(match[8].id, 9);
            Assert.AreEqual(match[9].id, 10);
            Assert.AreEqual(match[10].id, 11);
            Assert.AreEqual(match[11].id, 12);
            Assert.AreEqual(match[12].id, 13);
            Assert.AreEqual(match[13].id, 14);
            Assert.AreEqual(match[14].id, 15);
            Assert.AreEqual(match[15].id, 16);

            #endregion

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