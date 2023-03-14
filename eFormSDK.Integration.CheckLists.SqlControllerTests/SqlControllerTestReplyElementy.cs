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
using System.Linq;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using Case = Microting.eForm.Infrastructure.Data.Entities.Case;
using CheckListValue = Microting.eForm.Infrastructure.Data.Entities.CheckListValue;
using Field = Microting.eForm.Infrastructure.Data.Entities.Field;
using FieldValue = Microting.eForm.Infrastructure.Data.Entities.FieldValue;

namespace eFormSDK.Integration.CheckLists.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestReplyElementy : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        private Language language;

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
            language = DbContext.Languages.Single(x => x.Name == "Danish");
        }

        [Test]
        public async Task SQL_Check_CheckRead_ReturnsReplyElement()
        {
            // Arrance

            #region Arrance

            Random rnd = new Random();

            #region Template1

            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1

            CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region Fields

            #region field1

            Field f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "",
                "Comment field description",
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2

            Field f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);

            #endregion

            #region field3

            Field f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "",
                "Number Field Description",
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field5

            Field f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                85, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region Case1

            Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Check List Values

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Values1 =
                await testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Values2 =
                await testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Values3 =
                await testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Values4 =
                await testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Values5 =
                await testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion

            #endregion

            #endregion

            // Act

            ReplyElement match = await sut.CheckRead((int)aCase.MicrotingUid, (int)aCase.MicrotingCheckUid, language);

            // Assert

            #region Assert

            Assert.AreEqual(1, match.ElementList.Count());
            Microting.eForm.Infrastructure.Models.CheckListValue clv =
                (Microting.eForm.Infrastructure.Models.CheckListValue)match.ElementList[0];
            Assert.AreEqual(5, clv.DataItemList.Count);

            #region casts

            Microting.eForm.Infrastructure.Models.Field _f1 =
                (Microting.eForm.Infrastructure.Models.Field)clv.DataItemList[0];
            Microting.eForm.Infrastructure.Models.Field _f2 =
                (Microting.eForm.Infrastructure.Models.Field)clv.DataItemList[1];
            Microting.eForm.Infrastructure.Models.Field _f3 =
                (Microting.eForm.Infrastructure.Models.Field)clv.DataItemList[2];
            Microting.eForm.Infrastructure.Models.Field _f4 =
                (Microting.eForm.Infrastructure.Models.Field)clv.DataItemList[3];
            Microting.eForm.Infrastructure.Models.Field _f5 =
                (Microting.eForm.Infrastructure.Models.Field)clv.DataItemList[4];

            #endregion

            #region Barcode

            Assert.AreEqual(f1.BarcodeEnabled, 1);
            Assert.AreEqual(f2.BarcodeEnabled, 1);
            Assert.AreEqual(f3.BarcodeEnabled, 0);
            Assert.AreEqual(f4.BarcodeEnabled, 1);
            Assert.AreEqual(f5.BarcodeEnabled, 0);

            Assert.AreEqual(f1.BarcodeType, "barcode");
            Assert.AreEqual(f2.BarcodeType, "barcode");
            Assert.AreEqual(f3.BarcodeType, "barcode");
            Assert.AreEqual(f4.BarcodeType, "barcode");
            Assert.AreEqual(f5.BarcodeType, "barcode");

            #endregion

            #region chckl_id

            Assert.AreEqual(f1.CheckListId, cl2.Id);
            Assert.AreEqual(f2.CheckListId, cl2.Id);
            Assert.AreEqual(f3.CheckListId, cl2.Id);
            Assert.AreEqual(f4.CheckListId, cl2.Id);
            Assert.AreEqual(f5.CheckListId, cl2.Id);

            #endregion

            #region Color

            Assert.AreEqual(f1.Color, _f1.FieldValues[0].Color);
            Assert.AreEqual(f2.Color, _f2.FieldValues[0].Color);
            Assert.AreEqual(f3.Color, _f3.FieldValues[0].Color);
            Assert.AreEqual(f4.Color, _f4.FieldValues[0].Color);
            Assert.AreEqual(f5.Color, _f5.FieldValues[0].Color);

            #endregion

            #region custom

            //  Assert.AreEqual(f1.custom, _f1.FieldValues[0].Id);

            #endregion

            #region Decimal_Count

            Assert.AreEqual(f1.DecimalCount, null);
            Assert.AreEqual(f2.DecimalCount, null);
            Assert.AreEqual(f3.DecimalCount, 3);
            Assert.AreEqual(f4.DecimalCount, null);
            Assert.AreEqual(f5.DecimalCount, null);

            #endregion

            #region Default_value

            Assert.AreEqual(f1.DefaultValue, "");
            Assert.AreEqual(f2.DefaultValue, "");
            Assert.AreEqual(f3.DefaultValue, "");
            Assert.AreEqual(f4.DefaultValue, "");
            Assert.AreEqual(f5.DefaultValue, "");

            #endregion

            #region Description

            CDataValue f1desc = _f1.Description;
            CDataValue f2desc = _f2.Description;
            CDataValue f3desc = _f3.Description;
            CDataValue f4desc = _f4.Description;
            CDataValue f5desc = _f5.Description;

            Assert.AreEqual("Comment field description", f1desc.InderValue);
            Assert.AreEqual("showPDf Description", f2desc.InderValue);
            Assert.AreEqual("Number Field Description", f3desc.InderValue);
            Assert.AreEqual("date Description", f4desc.InderValue);
            Assert.AreEqual("picture Description", f5desc.InderValue);

            #endregion

            #region Displayindex

            Assert.AreEqual(f1.DisplayIndex, _f1.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f2.DisplayIndex, _f2.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f3.DisplayIndex, _f3.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f4.DisplayIndex, _f4.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f5.DisplayIndex, _f5.FieldValues[0].DisplayOrder);

            #endregion

            #region Dummy

            Assert.AreEqual(f1.Dummy, 1);
            Assert.AreEqual(f2.Dummy, 1);
            Assert.AreEqual(f3.Dummy, 0);
            Assert.AreEqual(f4.Dummy, 0);
            Assert.AreEqual(f5.Dummy, 0);

            #endregion

            #region geolocation

            #region enabled

            Assert.AreEqual(f1.GeolocationEnabled, 0);
            Assert.AreEqual(f2.GeolocationEnabled, 0);
            Assert.AreEqual(f3.GeolocationEnabled, 0);
            Assert.AreEqual(f4.GeolocationEnabled, 0);
            Assert.AreEqual(f5.GeolocationEnabled, 1);

            #endregion

            #region forced

            Assert.AreEqual(f1.GeolocationForced, 0);
            Assert.AreEqual(f2.GeolocationForced, 1);
            Assert.AreEqual(f3.GeolocationForced, 0);
            Assert.AreEqual(f4.GeolocationForced, 0);
            Assert.AreEqual(f5.GeolocationForced, 0);

            #endregion

            #region hidden

            Assert.AreEqual(f1.GeolocationHidden, 1);
            Assert.AreEqual(f2.GeolocationHidden, 0);
            Assert.AreEqual(f3.GeolocationHidden, 1);
            Assert.AreEqual(f4.GeolocationHidden, 1);
            Assert.AreEqual(f5.GeolocationHidden, 1);

            #endregion

            #endregion

            #region isNum

            Assert.AreEqual(f1.IsNum, 0);
            Assert.AreEqual(f2.IsNum, 0);
            Assert.AreEqual(f3.IsNum, 0);
            Assert.AreEqual(f4.IsNum, 0);
            Assert.AreEqual(f5.IsNum, 0);

            #endregion

            #region Label

            Assert.AreEqual("Comment field", _f1.Label);
            Assert.AreEqual("ShowPdf", _f2.Label);
            Assert.AreEqual("Numberfield", _f3.Label);
            Assert.AreEqual("Date", _f4.Label);
            Assert.AreEqual("Picture", _f5.Label);

            #endregion

            #region Mandatory

            Assert.AreEqual(f1.Mandatory, 1);
            Assert.AreEqual(f2.Mandatory, 0);
            Assert.AreEqual(f3.Mandatory, 1);
            Assert.AreEqual(f4.Mandatory, 1);
            Assert.AreEqual(f5.Mandatory, 1);

            #endregion

            #region maxLength

            Assert.AreEqual(f1.MaxLength, 55);
            Assert.AreEqual(f2.MaxLength, 5);
            Assert.AreEqual(f3.MaxLength, 8);
            Assert.AreEqual(f4.MaxLength, 666);
            Assert.AreEqual(f5.MaxLength, 69);

            #endregion

            #region min/max_Value

            #region max

            Assert.AreEqual(f1.MaxValue, "55");
            Assert.AreEqual(f2.MaxValue, "5");
            Assert.AreEqual(f3.MaxValue, "4865");
            Assert.AreEqual(f4.MaxValue, "41153");
            Assert.AreEqual(f5.MaxValue, "69");

            #endregion

            #region min

            Assert.AreEqual(f1.MinValue, "0");
            Assert.AreEqual(f2.MinValue, "0");
            Assert.AreEqual(f3.MinValue, "0");
            Assert.AreEqual(f4.MinValue, "0");
            Assert.AreEqual(f5.MinValue, "1");

            #endregion

            #endregion

            #region Multi

            Assert.AreEqual(f1.Multi, 0);
            Assert.AreEqual(f2.Multi, 0);
            Assert.AreEqual(f3.Multi, 0);
            Assert.AreEqual(f4.Multi, 0);
            Assert.AreEqual(f5.Multi, 0);

            #endregion

            #region Optional

            Assert.AreEqual(f1.Optional, 0);
            Assert.AreEqual(f2.Optional, 0);
            Assert.AreEqual(f3.Optional, 1);
            Assert.AreEqual(f4.Optional, 1);
            Assert.AreEqual(f5.Optional, 1);

            #endregion

            #region Query_Type

            Assert.AreEqual(f1.QueryType, null);
            Assert.AreEqual(f2.QueryType, null);
            Assert.AreEqual(f3.QueryType, null);
            Assert.AreEqual(f4.QueryType, null);
            Assert.AreEqual(f5.QueryType, null);

            #endregion

            #region Read_Only

            Assert.AreEqual(f1.ReadOnly, 1);
            Assert.AreEqual(f2.ReadOnly, 0);
            Assert.AreEqual(f3.ReadOnly, 1);
            Assert.AreEqual(f4.ReadOnly, 0);
            Assert.AreEqual(f5.ReadOnly, 0);

            #endregion

            #region Selected

            Assert.AreEqual(f1.Selected, 0);
            Assert.AreEqual(f2.Selected, 0);
            Assert.AreEqual(f3.Selected, 0);
            Assert.AreEqual(f4.Selected, 1);
            Assert.AreEqual(f5.Selected, 1);

            #endregion

            #region Split_Screen

            Assert.AreEqual(f1.Split, 0);
            Assert.AreEqual(f2.Split, 0);
            Assert.AreEqual(f3.Split, 0);
            Assert.AreEqual(f4.Split, 0);
            Assert.AreEqual(f5.Split, 0);

            #endregion

            #region Stop_On_Save

            Assert.AreEqual(f1.StopOnSave, 0);
            Assert.AreEqual(f2.StopOnSave, 0);
            Assert.AreEqual(f3.StopOnSave, 0);
            Assert.AreEqual(f4.StopOnSave, 0);
            Assert.AreEqual(f5.StopOnSave, 0);

            #endregion

            #region Unit_Name

            Assert.AreEqual(f1.UnitName, "");
            Assert.AreEqual(f2.UnitName, "");
            Assert.AreEqual(f3.UnitName, "");
            Assert.AreEqual(f4.UnitName, "");
            Assert.AreEqual(f5.UnitName, "");

            #endregion

            #region Values

            Assert.AreEqual(1, _f1.FieldValues.Count());
            Assert.AreEqual(1, _f2.FieldValues.Count());
            Assert.AreEqual(1, _f3.FieldValues.Count());
            Assert.AreEqual(1, _f4.FieldValues.Count());
            Assert.AreEqual(1, _f5.FieldValues.Count());

            Assert.AreEqual(field_Values1.Value, _f1.FieldValues[0].Value);
            Assert.AreEqual(field_Values2.Value, _f2.FieldValues[0].Value);
            Assert.AreEqual(field_Values3.Value, _f3.FieldValues[0].Value);
            Assert.AreEqual(field_Values4.Value, _f4.FieldValues[0].Value);
            Assert.AreEqual(field_Values5.Value, _f5.FieldValues[0].Value);

            #endregion

            #region Version

            Assert.AreEqual(f1.Version, 49);
            Assert.AreEqual(f2.Version, 9);
            Assert.AreEqual(f3.Version, 1);
            Assert.AreEqual(f4.Version, 1);
            Assert.AreEqual(f5.Version, 1);

            #endregion

            #endregion
        }


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