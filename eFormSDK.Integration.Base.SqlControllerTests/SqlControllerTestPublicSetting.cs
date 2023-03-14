/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;

namespace eFormSDK.Integration.Base.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestPublicSetting : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");

            #endregion

            sut = new SqlController(dbContextHelper);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
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

            var matchb1 = await sut.SettingRead(Settings.awsAccessKeyId);
            var matchb2 = await sut.SettingRead(Settings.awsEndPoint);
            var matchb3 = await sut.SettingRead(Settings.awsSecretAccessKey);
            var matchb4 = await sut.SettingRead(Settings.comAddressApi);
            var matchb5 = await sut.SettingRead(Settings.comAddressBasic);
            var matchb6 = await sut.SettingRead(Settings.comAddressPdfUpload);
            var matchb7 = await sut.SettingRead(Settings.comOrganizationId);
            var matchb8 = await sut.SettingRead(Settings.fileLocationJasper);
            var matchb9 = await sut.SettingRead(Settings.fileLocationPdf);
            var matchb10 = await sut.SettingRead(Settings.fileLocationPicture);
            var matchb11 = await sut.SettingRead(Settings.firstRunDone);
            var matchb12 = await sut.SettingRead(Settings.httpServerAddress);
            var matchb13 = await sut.SettingRead(Settings.knownSitesDone);
            var matchb14 = await sut.SettingRead(Settings.logLevel);
            var matchb15 = await sut.SettingRead(Settings.logLimit);
            var matchb16 = await sut.SettingRead(Settings.token);
            var matchb17 = await sut.SettingRead(Settings.unitLicenseNumber);


            // Assert
            Assert.AreEqual(matchb1, "3T98EGIO4Y9H8W2");
            Assert.AreEqual(matchb2, "https://sqs.eu-central-1.amazonaws.com/564456879978/");
            Assert.AreEqual(matchb3, "098u34098uergijt3098w");
            Assert.AreEqual(matchb4, "http://srv05.microting.com");
            Assert.AreEqual(matchb5, "https://basic.microting.com");
            Assert.AreEqual(matchb6, "https://srv16.microting.com");
            Assert.AreEqual(matchb7, "64856189");
            Assert.AreEqual(matchb8, @"\output\dataFolder\reports\");
            Assert.AreEqual(matchb9, @"\output\dataFolder\pdf\");
            Assert.AreEqual(matchb10, @"\output\dataFolder\picture\");
            Assert.AreEqual(matchb11, "true");
            Assert.AreEqual(matchb12, "http://localhost:3000");
            Assert.AreEqual(matchb13, "true");
            Assert.AreEqual(matchb14, "4");
            Assert.AreEqual(matchb15, "25000");
            Assert.AreEqual(matchb16, "abc1234567890abc1234567890abcdef");
            Assert.AreEqual(matchb17, "55");
        }


        // [Test]
        // public async Task SQL_Setting_SettingRead_ReadsSetting()
        // {
        //     // Arrance
        //
        //     // Act
        //     var match1 = await sut.SettingRead(Settings.awsAccessKeyId);
        //     var match2 = await sut.SettingRead(Settings.awsEndPoint);
        //     var match3 = await sut.SettingRead(Settings.awsSecretAccessKey);
        //     var match4 = await sut.SettingRead(Settings.comAddressApi);
        //     var match5 = await sut.SettingRead(Settings.comAddressBasic);
        //     var match6 = await sut.SettingRead(Settings.comAddressPdfUpload);
        //     var match7 = await sut.SettingRead(Settings.comOrganizationId);
        //     var match8 = await sut.SettingRead(Settings.fileLocationJasper);
        //     var match9 = await sut.SettingRead(Settings.fileLocationPdf);
        //     var match10 = await sut.SettingRead(Settings.fileLocationPicture);
        //     var match11 = await sut.SettingRead(Settings.firstRunDone);
        //     var match12 = await sut.SettingRead(Settings.httpServerAddress);
        //     var match13 = await sut.SettingRead(Settings.knownSitesDone);
        //     var match14 = await sut.SettingRead(Settings.logLevel);
        //     var match15 = await sut.SettingRead(Settings.logLimit);
        //     var match16 = await sut.SettingRead(Settings.token);
        //     var match17 = await sut.SettingRead(Settings.unitLicenseNumber);
        //
        //
        //     // Assert
        //     Assert.AreEqual(match1, "XXX");
        //     Assert.AreEqual(match2, "XXX");
        //     Assert.AreEqual(match3, "098u34098uergijt3098w");
        //     Assert.AreEqual(match4, "https://xxxxxx.xxxxxx.com");
        //     Assert.AreEqual(match5, "https://basic.microting.com");
        //     Assert.AreEqual(match6, "https://srv16.microting.com");
        //     Assert.AreEqual(match7, "64856189");
        //     Assert.AreEqual(match8, @"\output\dataFolder\reports\");
        //     Assert.AreEqual(match9, @"\output\dataFolder\pdf\");
        //     Assert.AreEqual(match10, @"\output\dataFolder\picture\");
        //     Assert.AreEqual(match11, "true");
        //     Assert.AreEqual(match12, "http://localhost:3000");
        //     Assert.AreEqual(match13, "true");
        //     Assert.AreEqual(match14, "4");
        //     Assert.AreEqual(match15, "25000");
        //     Assert.AreEqual(match16, "abc1234567890abc1234567890abcdef");
        //     Assert.AreEqual(match17, "55");
        //
        //
        // }


        [Test]
        public async Task SQL_Setting_SettingUpdate_UpdatesSetting()
        {
            // Arrance

            // Act
            await sut.SettingUpdate(Settings.token, "player");
            var match = DbContext.Settings.AsNoTracking().ToList();

            // Assert
            Assert.AreEqual(match[7].Value, "player");
        }


        // [Test]
        // public async Task SQL_Setting_SettingCheckAll_AllSettingsAreCreated()
        // {
        //     // Arrance
        //
        //     // Act
        //     List<Setting> setting = DbContext.Settings.AsNoTracking().ToList();
        //     await sut.SettingCheckAll();
        //     var match = DbContext.Settings.AsNoTracking().ToList();
        //
        //     // Assert
        //     Assert.NotNull(match);
        //     #region Name
        //
        //     Assert.AreEqual(match[0].Name, "firstRunDone");
        //     Assert.AreEqual(match[1].Name, "logLevel");
        //     Assert.AreEqual(match[2].Name, "logLimit");
        //     Assert.AreEqual(match[3].Name, "knownSitesDone");
        //     Assert.AreEqual(match[4].Name, "fileLocationPicture");
        //     Assert.AreEqual(match[5].Name, "fileLocationPdf");
        //     Assert.AreEqual(match[6].Name, "fileLocationJasper");
        //     Assert.AreEqual(match[7].Name, "token");
        //     Assert.AreEqual(match[8].Name, "comAddressBasic");
        //     Assert.AreEqual(match[9].Name, "comAddressApi");
        //     Assert.AreEqual(match[10].Name, "comAddressPdfUpload");
        //     Assert.AreEqual(match[11].Name, "comOrganizationId");
        //     Assert.AreEqual(match[12].Name, "awsAccessKeyId");
        //     Assert.AreEqual(match[13].Name, "awsSecretAccessKey");
        //     Assert.AreEqual(match[14].Name, "awsEndPoint");
        //     Assert.AreEqual(match[15].Name, "unitLicenseNumber");
        //     Assert.AreEqual(match[16].Name, "httpServerAddress");
        //     #endregion
        //
        //     #region Value
        //     Assert.AreEqual(match[0].Value, "true");
        //     Assert.AreEqual(match[1].Value, "4");
        //     Assert.AreEqual(match[2].Value, "25000");
        //     Assert.AreEqual(match[3].Value, "true");
        //     Assert.AreEqual(match[4].Value, @"\output\dataFolder\picture\");
        //     Assert.AreEqual(match[5].Value, @"\output\dataFolder\pdf\");
        //     Assert.AreEqual(match[6].Value, @"\output\dataFolder\reports\");
        //     Assert.AreEqual(match[7].Value, "abc1234567890abc1234567890abcdef");
        //     Assert.AreEqual(match[8].Value, "https://basic.microting.com");
        //     Assert.AreEqual(match[9].Value, "https://xxxxxx.xxxxxx.com");
        //     Assert.AreEqual(match[10].Value, "https://xxxxxx.xxxxxx.com");
        //     Assert.AreEqual(match[11].Value, "64856189");
        //     Assert.AreEqual(match[12].Value, "3T98EGIO4Y9H8W2");
        //     Assert.AreEqual(match[13].Value, "098u34098uergijt3098w");
        //     Assert.AreEqual(match[14].Value, "https://sqs.eu-central-1.amazonaws.com/564456879978/");
        //     Assert.AreEqual(match[15].Value, "55");
        //     Assert.AreEqual(match[16].Value, "http://localhost:3000");
        //     #endregion
        //
        //     #region ID's
        //     Assert.AreEqual(match[0].Id, 1);
        //     Assert.AreEqual(match[1].Id, 2);
        //     Assert.AreEqual(match[2].Id, 3);
        //     Assert.AreEqual(match[3].Id, 4);
        //     Assert.AreEqual(match[4].Id, 5);
        //     Assert.AreEqual(match[5].Id, 6);
        //     Assert.AreEqual(match[6].Id, 7);
        //     Assert.AreEqual(match[7].Id, 8);
        //     Assert.AreEqual(match[8].Id, 9);
        //     Assert.AreEqual(match[9].Id, 10);
        //     Assert.AreEqual(match[10].Id, 11);
        //     Assert.AreEqual(match[11].Id, 12);
        //     Assert.AreEqual(match[12].Id, 13);
        //     Assert.AreEqual(match[13].Id, 14);
        //     Assert.AreEqual(match[14].Id, 15);
        //     Assert.AreEqual(match[15].Id, 16);
        //
        //     #endregion
        //
        // }

        #endregion


        #region eventhandlers

#pragma warning disable 1998
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
#pragma warning restore 1998

        #endregion
    }
}