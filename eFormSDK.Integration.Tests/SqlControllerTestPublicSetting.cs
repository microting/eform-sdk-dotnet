using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestPublicSetting : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            await sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }


        #region Public Setting

        [Test]
        public async Task SQL_Setting_SettingCreateDefault_CreatesDetault()
        {
            // Arrance

            // Act
            var match = await sut.SettingCreateDefaults();
            // Assert

            Assert.True(match);

        }


        [Test]
        public async Task SQL_Setting_SetingCreate_CreatesSetting()
        {
            // Arrance

            // Act
            var match1 = await sut.SettingCreate(Settings.firstRunDone);
            var match2 = await sut.SettingCreate(Settings.logLevel);
            var match3 = await sut.SettingCreate(Settings.logLimit);
            var match4 = await sut.SettingCreate(Settings.knownSitesDone);
            var match5 = await sut.SettingCreate(Settings.fileLocationPicture);
            var match6 = await sut.SettingCreate(Settings.fileLocationPdf);
            var match7 = await sut.SettingCreate(Settings.fileLocationJasper);
            var match8 = await sut.SettingCreate(Settings.token);
            var match9 = await sut.SettingCreate(Settings.comAddressBasic);
            var match10 = await sut.SettingCreate(Settings.comAddressApi);
            var match11 = await sut.SettingCreate(Settings.comAddressPdfUpload);
            var match12 = await sut.SettingCreate(Settings.comOrganizationId);
            var match13 = await sut.SettingCreate(Settings.awsAccessKeyId);
            var match14 = await sut.SettingCreate(Settings.awsSecretAccessKey);
            var match15 = await sut.SettingCreate(Settings.awsEndPoint);
            var match16 = await sut.SettingCreate(Settings.unitLicenseNumber);
            var match17 = await sut.SettingCreate(Settings.httpServerAddress);

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
        public async Task SQL_Setting_SettingRead_ReadsSetting()
        {
            // Arrance

            // Act
            var match1 = await sut.SettingRead(Settings.awsAccessKeyId);
            var match2 = await sut.SettingRead(Settings.awsEndPoint);
            var match3 = await sut.SettingRead(Settings.awsSecretAccessKey);
            var match4 = await sut.SettingRead(Settings.comAddressApi);
            var match5 = await sut.SettingRead(Settings.comAddressBasic);
            var match6 = await sut.SettingRead(Settings.comAddressPdfUpload);
            var match7 = await sut.SettingRead(Settings.comOrganizationId);
            var match8 = await sut.SettingRead(Settings.fileLocationJasper);
            var match9 = await sut.SettingRead(Settings.fileLocationPdf);
            var match10 = await sut.SettingRead(Settings.fileLocationPicture);
            var match11 = await sut.SettingRead(Settings.firstRunDone);
            var match12 = await sut.SettingRead(Settings.httpServerAddress);
            var match13 = await sut.SettingRead(Settings.knownSitesDone);
            var match14 = await sut.SettingRead(Settings.logLevel);
            var match15 = await sut.SettingRead(Settings.logLimit);
            var match16 = await sut.SettingRead(Settings.token);
            var match17 = await sut.SettingRead(Settings.unitLicenseNumber);


            // Assert
            Assert.AreEqual(match1, "3T98EGIO4Y9H8W2");
            Assert.AreEqual(match2, "https://sqs.eu-central-1.amazonaws.com/564456879978/");
            Assert.AreEqual(match3, "098u34098uergijt3098w");
            Assert.AreEqual(match4, "http://srv05.microting.com");
            Assert.AreEqual(match5, "https://basic.microting.com");
            Assert.AreEqual(match6, "https://srv16.microting.com");
            Assert.AreEqual(match7, "64856189");
            Assert.AreEqual(match8, @"\output\dataFolder\reports\");
            Assert.AreEqual(match9, @"\output\dataFolder\pdf\");
            Assert.AreEqual(match10, @"\output\dataFolder\picture\");
            Assert.AreEqual(match11, "true");
            Assert.AreEqual(match12, "http://localhost:3000");
            Assert.AreEqual(match13, "true");
            Assert.AreEqual(match14, "4");
            Assert.AreEqual(match15, "25000");
            Assert.AreEqual(match16, "abc1234567890abc1234567890abcdef");
            Assert.AreEqual(match17, "55");


        }


        [Test]
        public async Task SQL_Setting_SettingUpdate_UpdatesSetting()
        {
            // Arrance

            // Act
            await sut.SettingUpdate(Settings.token, "player");
            var match = dbContext.settings.AsNoTracking().ToList();

            // Assert
            Assert.AreEqual(match[7].Value, "player");
        }


        [Test]
        public async Task SQL_Setting_SettingCheckAll_AllSettingsAreCreated()
        {
            // Arrance

            // Act
            List<settings> setting = dbContext.settings.AsNoTracking().ToList();
            await sut.SettingCheckAll();
            var match = dbContext.settings.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(match);
            #region Name

            Assert.AreEqual(match[0].Name, "firstRunDone");
            Assert.AreEqual(match[1].Name, "logLevel");
            Assert.AreEqual(match[2].Name, "logLimit");
            Assert.AreEqual(match[3].Name, "knownSitesDone");
            Assert.AreEqual(match[4].Name, "fileLocationPicture");
            Assert.AreEqual(match[5].Name, "fileLocationPdf");
            Assert.AreEqual(match[6].Name, "fileLocationJasper");
            Assert.AreEqual(match[7].Name, "token");
            Assert.AreEqual(match[8].Name, "comAddressBasic");
            Assert.AreEqual(match[9].Name, "comAddressApi");
            Assert.AreEqual(match[10].Name, "comAddressPdfUpload");
            Assert.AreEqual(match[11].Name, "comOrganizationId");
            Assert.AreEqual(match[12].Name, "awsAccessKeyId");
            Assert.AreEqual(match[13].Name, "awsSecretAccessKey");
            Assert.AreEqual(match[14].Name, "awsEndPoint");
            Assert.AreEqual(match[15].Name, "unitLicenseNumber");
            Assert.AreEqual(match[16].Name, "httpServerAddress");
            #endregion

            #region Value
            Assert.AreEqual(match[0].Value, "true");
            Assert.AreEqual(match[1].Value, "4");
            Assert.AreEqual(match[2].Value, "25000");
            Assert.AreEqual(match[3].Value, "true");
            Assert.AreEqual(match[4].Value, @"\output\dataFolder\picture\");
            Assert.AreEqual(match[5].Value, @"\output\dataFolder\pdf\");
            Assert.AreEqual(match[6].Value, @"\output\dataFolder\reports\");
            Assert.AreEqual(match[7].Value, "abc1234567890abc1234567890abcdef");
            Assert.AreEqual(match[8].Value, "https://basic.microting.com");
            Assert.AreEqual(match[9].Value, "http://srv05.microting.com");
            Assert.AreEqual(match[10].Value, "https://srv16.microting.com");
            Assert.AreEqual(match[11].Value, "64856189");
            Assert.AreEqual(match[12].Value, "3T98EGIO4Y9H8W2");
            Assert.AreEqual(match[13].Value, "098u34098uergijt3098w");
            Assert.AreEqual(match[14].Value, "https://sqs.eu-central-1.amazonaws.com/564456879978/");
            Assert.AreEqual(match[15].Value, "55");
            Assert.AreEqual(match[16].Value, "http://localhost:3000");
            #endregion

            #region ID's
            Assert.AreEqual(match[0].Id, 1);
            Assert.AreEqual(match[1].Id, 2);
            Assert.AreEqual(match[2].Id, 3);
            Assert.AreEqual(match[3].Id, 4);
            Assert.AreEqual(match[4].Id, 5);
            Assert.AreEqual(match[5].Id, 6);
            Assert.AreEqual(match[6].Id, 7);
            Assert.AreEqual(match[7].Id, 8);
            Assert.AreEqual(match[8].Id, 9);
            Assert.AreEqual(match[9].Id, 10);
            Assert.AreEqual(match[10].Id, 11);
            Assert.AreEqual(match[11].Id, 12);
            Assert.AreEqual(match[12].Id, 13);
            Assert.AreEqual(match[13].Id, 14);
            Assert.AreEqual(match[14].Id, 15);
            Assert.AreEqual(match[15].Id, 16);

            #endregion

        }
        #endregion

        #region eventhandlers
        public async Task EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }
        #endregion
    }

}