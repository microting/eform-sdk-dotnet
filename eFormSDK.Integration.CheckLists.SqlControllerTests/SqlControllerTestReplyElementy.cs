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
            language = DbContext.Languages.Single(x => x.Name == "Dansk");
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

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            Microting.eForm.Infrastructure.Models.CheckListValue clv =
                (Microting.eForm.Infrastructure.Models.CheckListValue)match.ElementList[0];
            Assert.That(clv.DataItemList.Count, Is.EqualTo(5));

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

            Assert.That(1, Is.EqualTo(f1.BarcodeEnabled));
            Assert.That(1, Is.EqualTo(f2.BarcodeEnabled));
            Assert.That(0, Is.EqualTo(f3.BarcodeEnabled));
            Assert.That(1, Is.EqualTo(f4.BarcodeEnabled));
            Assert.That(0, Is.EqualTo(f5.BarcodeEnabled));

            Assert.That("barcode", Is.EqualTo(f1.BarcodeType));
            Assert.That("barcode", Is.EqualTo(f2.BarcodeType));
            Assert.That("barcode", Is.EqualTo(f3.BarcodeType));
            Assert.That("barcode", Is.EqualTo(f4.BarcodeType));
            Assert.That("barcode", Is.EqualTo(f5.BarcodeType));

            #endregion

            #region chckl_id

            Assert.That(cl2.Id, Is.EqualTo(f1.CheckListId));
            Assert.That(cl2.Id, Is.EqualTo(f2.CheckListId));
            Assert.That(cl2.Id, Is.EqualTo(f3.CheckListId));
            Assert.That(cl2.Id, Is.EqualTo(f4.CheckListId));
            Assert.That(cl2.Id, Is.EqualTo(f5.CheckListId));

            #endregion

            #region Color

            Assert.That(_f1.FieldValues[0].Color, Is.EqualTo(f1.Color));
            Assert.That(_f2.FieldValues[0].Color, Is.EqualTo(f2.Color));
            Assert.That(_f3.FieldValues[0].Color, Is.EqualTo(f3.Color));
            Assert.That(_f4.FieldValues[0].Color, Is.EqualTo(f4.Color));
            Assert.That(_f5.FieldValues[0].Color, Is.EqualTo(f5.Color));

            #endregion

            #region custom

            //  Assert.AreEqual(f1.custom, _f1.FieldValues[0].Id);

            #endregion

            #region Decimal_Count

            Assert.That(f1.DecimalCount, Is.EqualTo(null));
            Assert.That(f2.DecimalCount, Is.EqualTo(null));
            Assert.That(f3.DecimalCount, Is.EqualTo(3));
            Assert.That(f4.DecimalCount, Is.EqualTo(null));
            Assert.That(f5.DecimalCount, Is.EqualTo(null));

            #endregion

            #region Default_value

            Assert.That(f1.DefaultValue, Is.EqualTo(""));
            Assert.That(f2.DefaultValue, Is.EqualTo(""));
            Assert.That(f3.DefaultValue, Is.EqualTo(""));
            Assert.That(f4.DefaultValue, Is.EqualTo(""));
            Assert.That(f5.DefaultValue, Is.EqualTo(""));

            #endregion

            #region Description

            CDataValue f1desc = _f1.Description;
            CDataValue f2desc = _f2.Description;
            CDataValue f3desc = _f3.Description;
            CDataValue f4desc = _f4.Description;
            CDataValue f5desc = _f5.Description;

            Assert.That(f1desc.InderValue, Is.EqualTo("Comment field description"));
            Assert.That(f2desc.InderValue, Is.EqualTo("showPDf Description"));
            Assert.That(f3desc.InderValue, Is.EqualTo("Number Field Description"));
            Assert.That(f4desc.InderValue, Is.EqualTo("date Description"));
            Assert.That(f5desc.InderValue, Is.EqualTo("picture Description"));

            #endregion

            #region Displayindex

            Assert.That(_f1.FieldValues[0].DisplayOrder, Is.EqualTo(f1.DisplayIndex));
            Assert.That(_f2.FieldValues[0].DisplayOrder, Is.EqualTo(f2.DisplayIndex));
            Assert.That(_f3.FieldValues[0].DisplayOrder, Is.EqualTo(f3.DisplayIndex));
            Assert.That(_f4.FieldValues[0].DisplayOrder, Is.EqualTo(f4.DisplayIndex));
            Assert.That(_f5.FieldValues[0].DisplayOrder, Is.EqualTo(f5.DisplayIndex));

            #endregion

            #region Dummy

            Assert.That(1, Is.EqualTo(f1.Dummy));
            Assert.That(1, Is.EqualTo(f2.Dummy));
            Assert.That(0, Is.EqualTo(f3.Dummy));
            Assert.That(0, Is.EqualTo(f4.Dummy));
            Assert.That(0, Is.EqualTo(f5.Dummy));

            #endregion

            #region geolocation

            #region enabled

            Assert.That(0, Is.EqualTo(f1.GeolocationEnabled));
            Assert.That(0, Is.EqualTo(f2.GeolocationEnabled));
            Assert.That(0, Is.EqualTo(f3.GeolocationEnabled));
            Assert.That(0, Is.EqualTo(f4.GeolocationEnabled));
            Assert.That(1, Is.EqualTo(f5.GeolocationEnabled));

            #endregion

            #region forced

            Assert.That(0, Is.EqualTo(f1.GeolocationForced));
            Assert.That(1, Is.EqualTo(f2.GeolocationForced));
            Assert.That(0, Is.EqualTo(f3.GeolocationForced));
            Assert.That(0, Is.EqualTo(f4.GeolocationForced));
            Assert.That(0, Is.EqualTo(f5.GeolocationForced));

            #endregion

            #region hidden

            Assert.That(1, Is.EqualTo(f1.GeolocationHidden));
            Assert.That(0, Is.EqualTo(f2.GeolocationHidden));
            Assert.That(1, Is.EqualTo(f3.GeolocationHidden));
            Assert.That(1, Is.EqualTo(f4.GeolocationHidden));
            Assert.That(1, Is.EqualTo(f5.GeolocationHidden));

            #endregion

            #endregion

            #region isNum

            Assert.That(0, Is.EqualTo(f1.IsNum));
            Assert.That(0, Is.EqualTo(f2.IsNum));
            Assert.That(0, Is.EqualTo(f3.IsNum));
            Assert.That(0, Is.EqualTo(f4.IsNum));
            Assert.That(0, Is.EqualTo(f5.IsNum));

            #endregion

            #region Label

            Assert.That(_f1.Label, Is.EqualTo("Comment field"));
            Assert.That(_f2.Label, Is.EqualTo("ShowPdf"));
            Assert.That(_f3.Label, Is.EqualTo("Numberfield"));
            Assert.That(_f4.Label, Is.EqualTo("Date"));
            Assert.That(_f5.Label, Is.EqualTo("Picture"));

            #endregion

            #region Mandatory

            Assert.That(1, Is.EqualTo(f1.Mandatory));
            Assert.That(0, Is.EqualTo(f2.Mandatory));
            Assert.That(1, Is.EqualTo(f3.Mandatory));
            Assert.That(1, Is.EqualTo(f4.Mandatory));
            Assert.That(1, Is.EqualTo(f5.Mandatory));

            #endregion

            #region maxLength

            Assert.That(55, Is.EqualTo(f1.MaxLength));
            Assert.That(5, Is.EqualTo(f2.MaxLength));
            Assert.That(8, Is.EqualTo(f3.MaxLength));
            Assert.That(666, Is.EqualTo(f4.MaxLength));
            Assert.That(69, Is.EqualTo(f5.MaxLength));

            #endregion

            #region min/max_Value

            #region max

            Assert.That("55", Is.EqualTo(f1.MaxValue));
            Assert.That("5", Is.EqualTo(f2.MaxValue));
            Assert.That("4865", Is.EqualTo(f3.MaxValue));
            Assert.That("41153", Is.EqualTo(f4.MaxValue));
            Assert.That("69", Is.EqualTo(f5.MaxValue));

            #endregion

            #region min

            Assert.That("0", Is.EqualTo(f1.MinValue));
            Assert.That("0", Is.EqualTo(f2.MinValue));
            Assert.That("0", Is.EqualTo(f3.MinValue));
            Assert.That("0", Is.EqualTo(f4.MinValue));
            Assert.That("1", Is.EqualTo(f5.MinValue));

            #endregion

            #endregion

            #region Multi

            Assert.That(0, Is.EqualTo(f1.Multi));
            Assert.That(0, Is.EqualTo(f2.Multi));
            Assert.That(0, Is.EqualTo(f3.Multi));
            Assert.That(0, Is.EqualTo(f4.Multi));
            Assert.That(0, Is.EqualTo(f5.Multi));

            #endregion

            #region Optional

            Assert.That(0, Is.EqualTo(f1.Optional));
            Assert.That(0, Is.EqualTo(f2.Optional));
            Assert.That(1, Is.EqualTo(f3.Optional));
            Assert.That(1, Is.EqualTo(f4.Optional));
            Assert.That(1, Is.EqualTo(f5.Optional));

            #endregion

            #region Query_Type

            Assert.That(f1.QueryType, Is.Null);
            Assert.That(f2.QueryType, Is.Null);
            Assert.That(f3.QueryType, Is.Null);
            Assert.That(f4.QueryType, Is.Null);
            Assert.That(f5.QueryType, Is.Null);

            #endregion

            #region Read_Only

            Assert.That(1, Is.EqualTo(f1.ReadOnly));
            Assert.That(0, Is.EqualTo(f2.ReadOnly));
            Assert.That(1, Is.EqualTo(f3.ReadOnly));
            Assert.That(0, Is.EqualTo(f4.ReadOnly));
            Assert.That(0, Is.EqualTo(f5.ReadOnly));

            #endregion

            #region Selected

            Assert.That(0, Is.EqualTo(f1.Selected));
            Assert.That(0, Is.EqualTo(f2.Selected));
            Assert.That(0, Is.EqualTo(f3.Selected));
            Assert.That(1, Is.EqualTo(f4.Selected));
            Assert.That(1, Is.EqualTo(f5.Selected));

            #endregion

            #region Split_Screen

            Assert.That(0, Is.EqualTo(f1.Split));
            Assert.That(0, Is.EqualTo(f2.Split));
            Assert.That(0, Is.EqualTo(f3.Split));
            Assert.That(0, Is.EqualTo(f4.Split));
            Assert.That(0, Is.EqualTo(f5.Split));

            #endregion

            #region Stop_On_Save

            Assert.That(0, Is.EqualTo(f1.StopOnSave));
            Assert.That(0, Is.EqualTo(f2.StopOnSave));
            Assert.That(0, Is.EqualTo(f3.StopOnSave));
            Assert.That(0, Is.EqualTo(f4.StopOnSave));
            Assert.That(0, Is.EqualTo(f5.StopOnSave));

            #endregion

            #region Unit_Name

            Assert.That(f1.UnitName, Is.EqualTo(""));
            Assert.That(f2.UnitName, Is.EqualTo(""));
            Assert.That(f3.UnitName, Is.EqualTo(""));
            Assert.That(f4.UnitName, Is.EqualTo(""));
            Assert.That(f5.UnitName, Is.EqualTo(""));

            #endregion

            #region Values

            Assert.That(_f1.FieldValues.Count(), Is.EqualTo(1));
            Assert.That(_f2.FieldValues.Count(), Is.EqualTo(1));
            Assert.That(_f3.FieldValues.Count(), Is.EqualTo(1));
            Assert.That(_f4.FieldValues.Count(), Is.EqualTo(1));
            Assert.That(_f5.FieldValues.Count(), Is.EqualTo(1));

            Assert.That(_f1.FieldValues[0].Value, Is.EqualTo(field_Values1.Value));
            Assert.That(_f2.FieldValues[0].Value, Is.EqualTo(field_Values2.Value));
            Assert.That(_f3.FieldValues[0].Value, Is.EqualTo(field_Values3.Value));
            Assert.That(_f4.FieldValues[0].Value, Is.EqualTo(field_Values4.Value));
            Assert.That(_f5.FieldValues[0].Value, Is.EqualTo(field_Values5.Value));

            #endregion

            #region Version

            Assert.That(49, Is.EqualTo(f1.Version));
            Assert.That(9, Is.EqualTo(f2.Version));
            Assert.That(1, Is.EqualTo(f3.Version));
            Assert.That(1, Is.EqualTo(f4.Version));
            Assert.That(1, Is.EqualTo(f5.Version));

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