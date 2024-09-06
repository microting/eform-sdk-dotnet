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
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.CheckLists.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CheckListsUTest : DbTestFixture
    {
        [Test]
        public async Task Checklists_create_DoesCreate()
        {
            //Arrange

            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            bool randomBool = rnd.Next(0, 2) > 0;

            CheckList checklistParent = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklistParent.Create(DbContext).ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ParentId = checklistParent.Id
            };

            //Act

            await checklist.Create(DbContext).ConfigureAwait(false);

            List<CheckList> checkLists = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListVersion> checkListVersion = DbContext.CheckListVersions.AsNoTracking().ToList();

            //Assert

            Assert.That(checkLists, Is.Not.EqualTo(null));
            Assert.That(checkListVersion, Is.Not.EqualTo(null));

            Assert.That(checkLists.Count(), Is.EqualTo(2));
            Assert.That(checkListVersion.Count(), Is.EqualTo(2));

            Assert.That(checkLists[1].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkLists[1].Version, Is.EqualTo(checklist.Version));
            //            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkLists[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(checkLists[1].WorkflowState));
            Assert.That(checkLists[1].Id, Is.EqualTo(checklist.Id));
            Assert.That(checkLists[1].Color, Is.EqualTo(checklist.Color));
            Assert.That(checkLists[1].Custom, Is.EqualTo(checklist.Custom));
            Assert.That(checkLists[1].Description, Is.EqualTo(checklist.Description));
            Assert.That(checkLists[1].Field1, Is.EqualTo(checklist.Field1));
            Assert.That(checkLists[1].Field2, Is.EqualTo(checklist.Field2));
            Assert.That(checkLists[1].Field3, Is.EqualTo(checklist.Field3));
            Assert.That(checkLists[1].Field4, Is.EqualTo(checklist.Field4));
            Assert.That(checkLists[1].Field5, Is.EqualTo(checklist.Field5));
            Assert.That(checkLists[1].Field6, Is.EqualTo(checklist.Field6));
            Assert.That(checkLists[1].Field7, Is.EqualTo(checklist.Field7));
            Assert.That(checkLists[1].Field8, Is.EqualTo(checklist.Field8));
            Assert.That(checkLists[1].Field9, Is.EqualTo(checklist.Field9));
            Assert.That(checkLists[1].Field10, Is.EqualTo(checklist.Field10));
            Assert.That(checkLists[1].Label, Is.EqualTo(checklist.Label));
            Assert.That(checkLists[1].Repeated, Is.EqualTo(checklist.Repeated));
            Assert.That(checkLists[1].ApprovalEnabled, Is.EqualTo(checklist.ApprovalEnabled));
            Assert.That(checkLists[1].CaseType, Is.EqualTo(checklist.CaseType));
            Assert.That(checkLists[1].DisplayIndex, Is.EqualTo(checklist.DisplayIndex));
            Assert.That(checkLists[1].DownloadEntities, Is.EqualTo(checklist.DownloadEntities));
            Assert.That(checkLists[1].FastNavigation, Is.EqualTo(checklist.FastNavigation));
            Assert.That(checkLists[1].FolderName, Is.EqualTo(checklist.FolderName));
            Assert.That(checkLists[1].ManualSync, Is.EqualTo(checklist.ManualSync));
            Assert.That(checkLists[1].MultiApproval, Is.EqualTo(checklist.MultiApproval));
            Assert.That(checkLists[1].OriginalId, Is.EqualTo(checklist.OriginalId));
            Assert.That(checklistParent.Id, Is.EqualTo(checklist.ParentId));
            Assert.That(checkLists[1].ReviewEnabled, Is.EqualTo(checklist.ReviewEnabled));
            Assert.That(checkLists[1].DocxExportEnabled, Is.EqualTo(checklist.DocxExportEnabled));
            Assert.That(checkLists[1].DoneButtonEnabled, Is.EqualTo(checklist.DoneButtonEnabled));
            Assert.That(checkLists[1].ExtraFieldsEnabled, Is.EqualTo(checklist.ExtraFieldsEnabled));
            Assert.That(checkLists[1].JasperExportEnabled, Is.EqualTo(checklist.JasperExportEnabled));
            Assert.That(checkLists[1].QuickSyncEnabled, Is.EqualTo(checklist.QuickSyncEnabled));

            //Versions
            Assert.That(checkListVersion[1].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkListVersion[1].Version, Is.EqualTo(1));
            //            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkListVersion[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(checkListVersion[1].WorkflowState));
            Assert.That(checkListVersion[1].Id, Is.EqualTo(checklist.Id));
            Assert.That(checkListVersion[1].Color, Is.EqualTo(checklist.Color));
            Assert.That(checkListVersion[1].Custom, Is.EqualTo(checklist.Custom));
            Assert.That(checkListVersion[1].Description, Is.EqualTo(checklist.Description));
            Assert.That(checkListVersion[1].Field1, Is.EqualTo(checklist.Field1));
            Assert.That(checkListVersion[1].Field2, Is.EqualTo(checklist.Field2));
            Assert.That(checkListVersion[1].Field3, Is.EqualTo(checklist.Field3));
            Assert.That(checkListVersion[1].Field4, Is.EqualTo(checklist.Field4));
            Assert.That(checkListVersion[1].Field5, Is.EqualTo(checklist.Field5));
            Assert.That(checkListVersion[1].Field6, Is.EqualTo(checklist.Field6));
            Assert.That(checkListVersion[1].Field7, Is.EqualTo(checklist.Field7));
            Assert.That(checkListVersion[1].Field8, Is.EqualTo(checklist.Field8));
            Assert.That(checkListVersion[1].Field9, Is.EqualTo(checklist.Field9));
            Assert.That(checkListVersion[1].Field10, Is.EqualTo(checklist.Field10));
            Assert.That(checkListVersion[1].Label, Is.EqualTo(checklist.Label));
            Assert.That(checkListVersion[1].Repeated, Is.EqualTo(checklist.Repeated));
            Assert.That(checkListVersion[1].ApprovalEnabled, Is.EqualTo(checklist.ApprovalEnabled));
            Assert.That(checkListVersion[1].CaseType, Is.EqualTo(checklist.CaseType));
            Assert.That(checkListVersion[1].DisplayIndex, Is.EqualTo(checklist.DisplayIndex));
            Assert.That(checkListVersion[1].DownloadEntities, Is.EqualTo(checklist.DownloadEntities));
            Assert.That(checkListVersion[1].FastNavigation, Is.EqualTo(checklist.FastNavigation));
            Assert.That(checkListVersion[1].FolderName, Is.EqualTo(checklist.FolderName));
            Assert.That(checkListVersion[1].ManualSync, Is.EqualTo(checklist.ManualSync));
            Assert.That(checkListVersion[1].MultiApproval, Is.EqualTo(checklist.MultiApproval));
            Assert.That(checkListVersion[1].OriginalId, Is.EqualTo(checklist.OriginalId));
            Assert.That(checkListVersion[1].ParentId, Is.EqualTo(checklistParent.Id));
            Assert.That(checkListVersion[1].ReviewEnabled, Is.EqualTo(checklist.ReviewEnabled));
            Assert.That(checkListVersion[1].DocxExportEnabled, Is.EqualTo(checklist.DocxExportEnabled));
            Assert.That(checkListVersion[1].DoneButtonEnabled, Is.EqualTo(checklist.DoneButtonEnabled));
            Assert.That(checkListVersion[1].ExtraFieldsEnabled, Is.EqualTo(checklist.ExtraFieldsEnabled));
            Assert.That(checkListVersion[1].JasperExportEnabled, Is.EqualTo(checklist.JasperExportEnabled));
            Assert.That(checkListVersion[1].QuickSyncEnabled, Is.EqualTo(checklist.QuickSyncEnabled));
        }

        [Test]
        public async Task Checklists_Update_DoesUpdate()
        {
            //Arrange

            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            bool randomBool = rnd.Next(0, 2) > 0;

            CheckList checklistParent = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklistParent.Create(DbContext).ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ParentId = checklistParent.Id
            };
            await checklist.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = checklist.UpdatedAt;
            string oldColour = checklist.Color;
            string oldCustom = checklist.Custom;
            string oldDescription = checklist.Description;
            int? oldField1 = checklist.Field1;
            int? oldField2 = checklist.Field2;
            int? oldField3 = checklist.Field3;
            int? oldField4 = checklist.Field4;
            int? oldField5 = checklist.Field5;
            int? oldField6 = checklist.Field6;
            int? oldField7 = checklist.Field7;
            int? oldField8 = checklist.Field8;
            int? oldField9 = checklist.Field9;
            int? oldField10 = checklist.Field10;
            string oldLabel = checklist.Label;
            int? oldRepeated = checklist.Repeated;
            short? oldApprovalEnabled = checklist.ApprovalEnabled;
            string oldCaseType = checklist.CaseType;
            int? oldDisplayIndex = checklist.DisplayIndex;
            short? oldDownloadEntities = checklist.DownloadEntities;
            short? oldFastNavigation = checklist.FastNavigation;
            string oldFolderName = checklist.FolderName;
            short? oldManualSync = checklist.ManualSync;
            short? oldMultiApproval = checklist.MultiApproval;
            string oldOriginalId = checklist.OriginalId;
            short? oldReviewEnabled = checklist.ReviewEnabled;
            bool oldDocxExportEnabled = checklist.DocxExportEnabled;
            short? oldDoneButtonEnabled = checklist.DoneButtonEnabled;
            short? oldExtraFieldsEnabled = checklist.ExtraFieldsEnabled;
            bool oldJasperExportEnabled = checklist.JasperExportEnabled;
            short? oldQuickSyncEnabled = checklist.QuickSyncEnabled;

            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
            checklist.Field4 = rnd.Next(1, 255);
            checklist.Field5 = rnd.Next(1, 255);
            checklist.Field6 = rnd.Next(1, 255);
            checklist.Field7 = rnd.Next(1, 255);
            checklist.Field8 = rnd.Next(1, 255);
            checklist.Field9 = rnd.Next(1, 255);
            checklist.Field10 = rnd.Next(1, 255);
            checklist.Label = Guid.NewGuid().ToString();
            checklist.Repeated = rnd.Next(1, 255);
            checklist.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.CaseType = Guid.NewGuid().ToString();
            checklist.DisplayIndex = rnd.Next(1, 255);
            checklist.DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.FolderName = Guid.NewGuid().ToString();
            checklist.ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.OriginalId = Guid.NewGuid().ToString();
            checklist.ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);

            await checklist.Update(DbContext).ConfigureAwait(false);


            List<CheckList> checkLists = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListVersion> checkListVersion = DbContext.CheckListVersions.AsNoTracking().ToList();

            //Assert

            Assert.That(checkLists, Is.Not.EqualTo(null));
            Assert.That(checkListVersion, Is.Not.EqualTo(null));

            Assert.That(checkLists.Count(), Is.EqualTo(2));
            Assert.That(checkListVersion.Count(), Is.EqualTo(3));

            Assert.That(checkLists[1].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkLists[1].Version, Is.EqualTo(checklist.Version));
            //            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkLists[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(checkLists[1].WorkflowState));
            Assert.That(checkLists[1].Id, Is.EqualTo(checklist.Id));
            Assert.That(checkLists[1].Color, Is.EqualTo(checklist.Color));
            Assert.That(checkLists[1].Custom, Is.EqualTo(checklist.Custom));
            Assert.That(checkLists[1].Description, Is.EqualTo(checklist.Description));
            Assert.That(checkLists[1].Field1, Is.EqualTo(checklist.Field1));
            Assert.That(checkLists[1].Field2, Is.EqualTo(checklist.Field2));
            Assert.That(checkLists[1].Field3, Is.EqualTo(checklist.Field3));
            Assert.That(checkLists[1].Field4, Is.EqualTo(checklist.Field4));
            Assert.That(checkLists[1].Field5, Is.EqualTo(checklist.Field5));
            Assert.That(checkLists[1].Field6, Is.EqualTo(checklist.Field6));
            Assert.That(checkLists[1].Field7, Is.EqualTo(checklist.Field7));
            Assert.That(checkLists[1].Field8, Is.EqualTo(checklist.Field8));
            Assert.That(checkLists[1].Field9, Is.EqualTo(checklist.Field9));
            Assert.That(checkLists[1].Field10, Is.EqualTo(checklist.Field10));
            Assert.That(checkLists[1].Label, Is.EqualTo(checklist.Label));
            Assert.That(checkLists[1].Repeated, Is.EqualTo(checklist.Repeated));
            Assert.That(checkLists[1].ApprovalEnabled, Is.EqualTo(checklist.ApprovalEnabled));
            Assert.That(checkLists[1].CaseType, Is.EqualTo(checklist.CaseType));
            Assert.That(checkLists[1].DisplayIndex, Is.EqualTo(checklist.DisplayIndex));
            Assert.That(checkLists[1].DownloadEntities, Is.EqualTo(checklist.DownloadEntities));
            Assert.That(checkLists[1].FastNavigation, Is.EqualTo(checklist.FastNavigation));
            Assert.That(checkLists[1].FolderName, Is.EqualTo(checklist.FolderName));
            Assert.That(checkLists[1].ManualSync, Is.EqualTo(checklist.ManualSync));
            Assert.That(checkLists[1].MultiApproval, Is.EqualTo(checklist.MultiApproval));
            Assert.That(checkLists[1].OriginalId, Is.EqualTo(checklist.OriginalId));
            Assert.That(checklistParent.Id, Is.EqualTo(checklist.ParentId));
            Assert.That(checkLists[1].ReviewEnabled, Is.EqualTo(checklist.ReviewEnabled));
            Assert.That(checkLists[1].DocxExportEnabled, Is.EqualTo(checklist.DocxExportEnabled));
            Assert.That(checkLists[1].DoneButtonEnabled, Is.EqualTo(checklist.DoneButtonEnabled));
            Assert.That(checkLists[1].ExtraFieldsEnabled, Is.EqualTo(checklist.ExtraFieldsEnabled));
            Assert.That(checkLists[1].JasperExportEnabled, Is.EqualTo(checklist.JasperExportEnabled));
            Assert.That(checkLists[1].QuickSyncEnabled, Is.EqualTo(checklist.QuickSyncEnabled));

            //Old Version
            Assert.That(checkListVersion[1].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkListVersion[1].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), checkListVersion[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(checkListVersion[1].WorkflowState));
            Assert.That(checkListVersion[1].CheckListId, Is.EqualTo(checklist.Id));
            Assert.That(checkListVersion[1].Color, Is.EqualTo(oldColour));
            Assert.That(checkListVersion[1].Custom, Is.EqualTo(oldCustom));
            Assert.That(checkListVersion[1].Description, Is.EqualTo(oldDescription));
            Assert.That(checkListVersion[1].Field1, Is.EqualTo(oldField1));
            Assert.That(checkListVersion[1].Field2, Is.EqualTo(oldField2));
            Assert.That(checkListVersion[1].Field3, Is.EqualTo(oldField3));
            Assert.That(checkListVersion[1].Field4, Is.EqualTo(oldField4));
            Assert.That(checkListVersion[1].Field5, Is.EqualTo(oldField5));
            Assert.That(checkListVersion[1].Field6, Is.EqualTo(oldField6));
            Assert.That(checkListVersion[1].Field7, Is.EqualTo(oldField7));
            Assert.That(checkListVersion[1].Field8, Is.EqualTo(oldField8));
            Assert.That(checkListVersion[1].Field9, Is.EqualTo(oldField9));
            Assert.That(checkListVersion[1].Field10, Is.EqualTo(oldField10));
            Assert.That(checkListVersion[1].Label, Is.EqualTo(oldLabel));
            Assert.That(checkListVersion[1].Repeated, Is.EqualTo(oldRepeated));
            Assert.That(checkListVersion[1].ApprovalEnabled, Is.EqualTo(oldApprovalEnabled));
            Assert.That(checkListVersion[1].CaseType, Is.EqualTo(oldCaseType));
            Assert.That(checkListVersion[1].DisplayIndex, Is.EqualTo(oldDisplayIndex));
            Assert.That(checkListVersion[1].DownloadEntities, Is.EqualTo(oldDownloadEntities));
            Assert.That(checkListVersion[1].FastNavigation, Is.EqualTo(oldFastNavigation));
            Assert.That(checkListVersion[1].FolderName, Is.EqualTo(oldFolderName));
            Assert.That(checkListVersion[1].ManualSync, Is.EqualTo(oldManualSync));
            Assert.That(checkListVersion[1].MultiApproval, Is.EqualTo(oldMultiApproval));
            Assert.That(checkListVersion[1].OriginalId, Is.EqualTo(oldOriginalId));
            Assert.That(checkListVersion[1].ParentId, Is.EqualTo(checklistParent.Id));
            Assert.That(checkListVersion[1].ReviewEnabled, Is.EqualTo(oldReviewEnabled));
            Assert.That(checkListVersion[1].DocxExportEnabled, Is.EqualTo(oldDocxExportEnabled));
            Assert.That(checkListVersion[1].DoneButtonEnabled, Is.EqualTo(oldDoneButtonEnabled));
            Assert.That(checkListVersion[1].ExtraFieldsEnabled, Is.EqualTo(oldExtraFieldsEnabled));
            Assert.That(checkListVersion[1].JasperExportEnabled, Is.EqualTo(oldJasperExportEnabled));
            Assert.That(checkListVersion[1].QuickSyncEnabled, Is.EqualTo(oldQuickSyncEnabled));

            //New Version
            Assert.That(checkListVersion[2].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkListVersion[2].Version, Is.EqualTo(2));
            //            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkListVersion[2].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(checkListVersion[2].WorkflowState));
            Assert.That(checkListVersion[2].CheckListId, Is.EqualTo(checklist.Id));
            Assert.That(checkListVersion[2].Color, Is.EqualTo(checklist.Color));
            Assert.That(checkListVersion[2].Custom, Is.EqualTo(checklist.Custom));
            Assert.That(checkListVersion[2].Description, Is.EqualTo(checklist.Description));
            Assert.That(checkListVersion[2].Field1, Is.EqualTo(checklist.Field1));
            Assert.That(checkListVersion[2].Field2, Is.EqualTo(checklist.Field2));
            Assert.That(checkListVersion[2].Field3, Is.EqualTo(checklist.Field3));
            Assert.That(checkListVersion[2].Field4, Is.EqualTo(checklist.Field4));
            Assert.That(checkListVersion[2].Field5, Is.EqualTo(checklist.Field5));
            Assert.That(checkListVersion[2].Field6, Is.EqualTo(checklist.Field6));
            Assert.That(checkListVersion[2].Field7, Is.EqualTo(checklist.Field7));
            Assert.That(checkListVersion[2].Field8, Is.EqualTo(checklist.Field8));
            Assert.That(checkListVersion[2].Field9, Is.EqualTo(checklist.Field9));
            Assert.That(checkListVersion[2].Field10, Is.EqualTo(checklist.Field10));
            Assert.That(checkListVersion[2].Label, Is.EqualTo(checklist.Label));
            Assert.That(checkListVersion[2].Repeated, Is.EqualTo(checklist.Repeated));
            Assert.That(checkListVersion[2].ApprovalEnabled, Is.EqualTo(checklist.ApprovalEnabled));
            Assert.That(checkListVersion[2].CaseType, Is.EqualTo(checklist.CaseType));
            Assert.That(checkListVersion[2].DisplayIndex, Is.EqualTo(checklist.DisplayIndex));
            Assert.That(checkListVersion[2].DownloadEntities, Is.EqualTo(checklist.DownloadEntities));
            Assert.That(checkListVersion[2].FastNavigation, Is.EqualTo(checklist.FastNavigation));
            Assert.That(checkListVersion[2].FolderName, Is.EqualTo(checklist.FolderName));
            Assert.That(checkListVersion[2].ManualSync, Is.EqualTo(checklist.ManualSync));
            Assert.That(checkListVersion[2].MultiApproval, Is.EqualTo(checklist.MultiApproval));
            Assert.That(checkListVersion[2].OriginalId, Is.EqualTo(checklist.OriginalId));
            Assert.That(checkListVersion[2].ParentId, Is.EqualTo(checklistParent.Id));
            Assert.That(checkListVersion[2].ReviewEnabled, Is.EqualTo(checklist.ReviewEnabled));
            Assert.That(checkListVersion[2].DocxExportEnabled, Is.EqualTo(checklist.DocxExportEnabled));
            Assert.That(checkListVersion[2].DoneButtonEnabled, Is.EqualTo(checklist.DoneButtonEnabled));
            Assert.That(checkListVersion[2].ExtraFieldsEnabled, Is.EqualTo(checklist.ExtraFieldsEnabled));
            Assert.That(checkListVersion[2].JasperExportEnabled, Is.EqualTo(checklist.JasperExportEnabled));
            Assert.That(checkListVersion[2].QuickSyncEnabled, Is.EqualTo(checklist.QuickSyncEnabled));
        }

        [Test]
        public async Task CheckLists_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            bool randomBool = rnd.Next(0, 2) > 0;

            CheckList checklistParent = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklistParent.Create(DbContext).ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ParentId = checklistParent.Id
            };
            await checklist.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = checklist.UpdatedAt;


            await checklist.Delete(DbContext);


            List<CheckList> checkLists = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListVersion> checkListVersion = DbContext.CheckListVersions.AsNoTracking().ToList();

            //Assert

            Assert.That(checkLists, Is.Not.EqualTo(null));
            Assert.That(checkListVersion, Is.Not.EqualTo(null));

            Assert.That(checkLists.Count(), Is.EqualTo(2));
            Assert.That(checkListVersion.Count(), Is.EqualTo(3));

            Assert.That(checkLists[1].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkLists[1].Version, Is.EqualTo(checklist.Version));
            //            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkLists[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(checkLists[1].WorkflowState));
            Assert.That(checkLists[1].Id, Is.EqualTo(checklist.Id));
            Assert.That(checkLists[1].Color, Is.EqualTo(checklist.Color));
            Assert.That(checkLists[1].Custom, Is.EqualTo(checklist.Custom));
            Assert.That(checkLists[1].Description, Is.EqualTo(checklist.Description));
            Assert.That(checkLists[1].Field1, Is.EqualTo(checklist.Field1));
            Assert.That(checkLists[1].Field2, Is.EqualTo(checklist.Field2));
            Assert.That(checkLists[1].Field3, Is.EqualTo(checklist.Field3));
            Assert.That(checkLists[1].Field4, Is.EqualTo(checklist.Field4));
            Assert.That(checkLists[1].Field5, Is.EqualTo(checklist.Field5));
            Assert.That(checkLists[1].Field6, Is.EqualTo(checklist.Field6));
            Assert.That(checkLists[1].Field7, Is.EqualTo(checklist.Field7));
            Assert.That(checkLists[1].Field8, Is.EqualTo(checklist.Field8));
            Assert.That(checkLists[1].Field9, Is.EqualTo(checklist.Field9));
            Assert.That(checkLists[1].Field10, Is.EqualTo(checklist.Field10));
            Assert.That(checkLists[1].Label, Is.EqualTo(checklist.Label));
            Assert.That(checkLists[1].Repeated, Is.EqualTo(checklist.Repeated));
            Assert.That(checkLists[1].ApprovalEnabled, Is.EqualTo(checklist.ApprovalEnabled));
            Assert.That(checkLists[1].CaseType, Is.EqualTo(checklist.CaseType));
            Assert.That(checkLists[1].DisplayIndex, Is.EqualTo(checklist.DisplayIndex));
            Assert.That(checkLists[1].DownloadEntities, Is.EqualTo(checklist.DownloadEntities));
            Assert.That(checkLists[1].FastNavigation, Is.EqualTo(checklist.FastNavigation));
            Assert.That(checkLists[1].FolderName, Is.EqualTo(checklist.FolderName));
            Assert.That(checkLists[1].ManualSync, Is.EqualTo(checklist.ManualSync));
            Assert.That(checkLists[1].MultiApproval, Is.EqualTo(checklist.MultiApproval));
            Assert.That(checkLists[1].OriginalId, Is.EqualTo(checklist.OriginalId));
            Assert.That(checklistParent.Id, Is.EqualTo(checklist.ParentId));
            Assert.That(checkLists[1].ReviewEnabled, Is.EqualTo(checklist.ReviewEnabled));
            Assert.That(checkLists[1].DocxExportEnabled, Is.EqualTo(checklist.DocxExportEnabled));
            Assert.That(checkLists[1].DoneButtonEnabled, Is.EqualTo(checklist.DoneButtonEnabled));
            Assert.That(checkLists[1].ExtraFieldsEnabled, Is.EqualTo(checklist.ExtraFieldsEnabled));
            Assert.That(checkLists[1].JasperExportEnabled, Is.EqualTo(checklist.JasperExportEnabled));
            Assert.That(checkLists[1].QuickSyncEnabled, Is.EqualTo(checklist.QuickSyncEnabled));

            //Old Version
            Assert.That(checkListVersion[1].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkListVersion[1].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), checkListVersion[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(checkListVersion[1].WorkflowState));
            Assert.That(checkListVersion[1].CheckListId, Is.EqualTo(checklist.Id));
            Assert.That(checkListVersion[1].Color, Is.EqualTo(checklist.Color));
            Assert.That(checkListVersion[1].Custom, Is.EqualTo(checklist.Custom));
            Assert.That(checkListVersion[1].Description, Is.EqualTo(checklist.Description));
            Assert.That(checkListVersion[1].Field1, Is.EqualTo(checklist.Field1));
            Assert.That(checkListVersion[1].Field2, Is.EqualTo(checklist.Field2));
            Assert.That(checkListVersion[1].Field3, Is.EqualTo(checklist.Field3));
            Assert.That(checkListVersion[1].Field4, Is.EqualTo(checklist.Field4));
            Assert.That(checkListVersion[1].Field5, Is.EqualTo(checklist.Field5));
            Assert.That(checkListVersion[1].Field6, Is.EqualTo(checklist.Field6));
            Assert.That(checkListVersion[1].Field7, Is.EqualTo(checklist.Field7));
            Assert.That(checkListVersion[1].Field8, Is.EqualTo(checklist.Field8));
            Assert.That(checkListVersion[1].Field9, Is.EqualTo(checklist.Field9));
            Assert.That(checkListVersion[1].Field10, Is.EqualTo(checklist.Field10));
            Assert.That(checkListVersion[1].Label, Is.EqualTo(checklist.Label));
            Assert.That(checkListVersion[1].Repeated, Is.EqualTo(checklist.Repeated));
            Assert.That(checkListVersion[1].ApprovalEnabled, Is.EqualTo(checklist.ApprovalEnabled));
            Assert.That(checkListVersion[1].CaseType, Is.EqualTo(checklist.CaseType));
            Assert.That(checkListVersion[1].DisplayIndex, Is.EqualTo(checklist.DisplayIndex));
            Assert.That(checkListVersion[1].DownloadEntities, Is.EqualTo(checklist.DownloadEntities));
            Assert.That(checkListVersion[1].FastNavigation, Is.EqualTo(checklist.FastNavigation));
            Assert.That(checkListVersion[1].FolderName, Is.EqualTo(checklist.FolderName));
            Assert.That(checkListVersion[1].ManualSync, Is.EqualTo(checklist.ManualSync));
            Assert.That(checkListVersion[1].MultiApproval, Is.EqualTo(checklist.MultiApproval));
            Assert.That(checkListVersion[1].OriginalId, Is.EqualTo(checklist.OriginalId));
            Assert.That(checkListVersion[1].ParentId, Is.EqualTo(checklistParent.Id));
            Assert.That(checkListVersion[1].ReviewEnabled, Is.EqualTo(checklist.ReviewEnabled));
            Assert.That(checkListVersion[1].DocxExportEnabled, Is.EqualTo(checklist.DocxExportEnabled));
            Assert.That(checkListVersion[1].DoneButtonEnabled, Is.EqualTo(checklist.DoneButtonEnabled));
            Assert.That(checkListVersion[1].ExtraFieldsEnabled, Is.EqualTo(checklist.ExtraFieldsEnabled));
            Assert.That(checkListVersion[1].JasperExportEnabled, Is.EqualTo(checklist.JasperExportEnabled));
            Assert.That(checkListVersion[1].QuickSyncEnabled, Is.EqualTo(checklist.QuickSyncEnabled));

            //New Version
            Assert.That(checkListVersion[2].CreatedAt.ToString(), Is.EqualTo(checklist.CreatedAt.ToString()));
            Assert.That(checkListVersion[2].Version, Is.EqualTo(2));
            //            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkListVersion[2].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(checkListVersion[2].WorkflowState));
            Assert.That(checkListVersion[2].CheckListId, Is.EqualTo(checklist.Id));
            Assert.That(checkListVersion[2].Color, Is.EqualTo(checklist.Color));
            Assert.That(checkListVersion[2].Custom, Is.EqualTo(checklist.Custom));
            Assert.That(checkListVersion[2].Description, Is.EqualTo(checklist.Description));
            Assert.That(checkListVersion[2].Field1, Is.EqualTo(checklist.Field1));
            Assert.That(checkListVersion[2].Field2, Is.EqualTo(checklist.Field2));
            Assert.That(checkListVersion[2].Field3, Is.EqualTo(checklist.Field3));
            Assert.That(checkListVersion[2].Field4, Is.EqualTo(checklist.Field4));
            Assert.That(checkListVersion[2].Field5, Is.EqualTo(checklist.Field5));
            Assert.That(checkListVersion[2].Field6, Is.EqualTo(checklist.Field6));
            Assert.That(checkListVersion[2].Field7, Is.EqualTo(checklist.Field7));
            Assert.That(checkListVersion[2].Field8, Is.EqualTo(checklist.Field8));
            Assert.That(checkListVersion[2].Field9, Is.EqualTo(checklist.Field9));
            Assert.That(checkListVersion[2].Field10, Is.EqualTo(checklist.Field10));
            Assert.That(checkListVersion[2].Label, Is.EqualTo(checklist.Label));
            Assert.That(checkListVersion[2].Repeated, Is.EqualTo(checklist.Repeated));
            Assert.That(checkListVersion[2].ApprovalEnabled, Is.EqualTo(checklist.ApprovalEnabled));
            Assert.That(checkListVersion[2].CaseType, Is.EqualTo(checklist.CaseType));
            Assert.That(checkListVersion[2].DisplayIndex, Is.EqualTo(checklist.DisplayIndex));
            Assert.That(checkListVersion[2].DownloadEntities, Is.EqualTo(checklist.DownloadEntities));
            Assert.That(checkListVersion[2].FastNavigation, Is.EqualTo(checklist.FastNavigation));
            Assert.That(checkListVersion[2].FolderName, Is.EqualTo(checklist.FolderName));
            Assert.That(checkListVersion[2].ManualSync, Is.EqualTo(checklist.ManualSync));
            Assert.That(checkListVersion[2].MultiApproval, Is.EqualTo(checklist.MultiApproval));
            Assert.That(checkListVersion[2].OriginalId, Is.EqualTo(checklist.OriginalId));
            Assert.That(checkListVersion[2].ParentId, Is.EqualTo(checklistParent.Id));
            Assert.That(checkListVersion[2].ReviewEnabled, Is.EqualTo(checklist.ReviewEnabled));
            Assert.That(checkListVersion[2].DocxExportEnabled, Is.EqualTo(checklist.DocxExportEnabled));
            Assert.That(checkListVersion[2].DoneButtonEnabled, Is.EqualTo(checklist.DoneButtonEnabled));
            Assert.That(checkListVersion[2].ExtraFieldsEnabled, Is.EqualTo(checklist.ExtraFieldsEnabled));
            Assert.That(checkListVersion[2].JasperExportEnabled, Is.EqualTo(checklist.JasperExportEnabled));
            Assert.That(checkListVersion[2].QuickSyncEnabled, Is.EqualTo(checklist.QuickSyncEnabled));
        }
    }
}