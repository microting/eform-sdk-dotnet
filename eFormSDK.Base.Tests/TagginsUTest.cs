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

namespace eFormSDK.Base.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class TagginsUTest : DbTestFixture
    {
        [Test]
        public async Task Taggins_Create_DoesCreate()
        {
            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            bool randomBool = rnd.Next(0, 2) > 0;

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };
            await tag.Create(DbContext).ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field3 = rnd.Next(1, 255),
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
                ParentId = rnd.Next(1, 255),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklist.Create(DbContext).ConfigureAwait(false);

            Tagging tagging = new Tagging
            {
                Tag = tag,
                CheckList = checklist,
                TaggerId = rnd.Next(1, 255),
                TagId = rnd.Next(1, 255),
                CheckListId = checklist.Id
            };


            //Act

            await tagging.Create(DbContext).ConfigureAwait(false);

            List<Tagging> taggings = DbContext.Taggings.AsNoTracking().ToList();
            List<TaggingVersion> taggingVersions = DbContext.TaggingVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(taggings);
            Assert.NotNull(taggingVersions);

            Assert.AreEqual(1, taggings.Count());
            Assert.AreEqual(1, taggingVersions.Count());

            Assert.AreEqual(tagging.CreatedAt.ToString(), taggings[0].CreatedAt.ToString());
            Assert.AreEqual(tagging.Version, taggings[0].Version);
//            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggings[0].UpdatedAt.ToString());
            Assert.AreEqual(taggings[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tagging.Id, taggings[0].Id);
            Assert.AreEqual(tagging.TaggerId, taggings[0].TaggerId);
            Assert.AreEqual(tagging.TagId, tag.Id);
            Assert.AreEqual(tagging.CheckListId, checklist.Id);

            //Version 1
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggingVersions[0].CreatedAt.ToString());
            Assert.AreEqual(tagging.Version, taggingVersions[0].Version);
//            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggingVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(taggingVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tagging.Id, taggingVersions[0].Id);
            Assert.AreEqual(tagging.TaggerId, taggingVersions[0].TaggerId);
            Assert.AreEqual(tag.Id, taggingVersions[0].TagId);
            Assert.AreEqual(checklist.Id, taggingVersions[0].CheckListId);
        }

        [Test]
        public async Task Taggings_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            bool randomBool = rnd.Next(0, 2) > 0;

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };
            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field3 = rnd.Next(1, 255),
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
                ParentId = rnd.Next(1, 255),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklist.Create(DbContext).ConfigureAwait(false);

            Tagging tagging = new Tagging
            {
                Tag = tag,
                CheckList = checklist,
                TaggerId = rnd.Next(1, 255),
                TagId = rnd.Next(1, 255),
                CheckListId = checklist.Id
            };
            await tagging.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = tagging.UpdatedAt;

            await tagging.Delete(DbContext);

            List<Tagging> taggings = DbContext.Taggings.AsNoTracking().ToList();
            List<TaggingVersion> taggingVersions = DbContext.TaggingVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(taggings);
            Assert.NotNull(taggingVersions);

            Assert.AreEqual(1, taggings.Count());
            Assert.AreEqual(2, taggingVersions.Count());

            Assert.AreEqual(tagging.CreatedAt.ToString(), taggings[0].CreatedAt.ToString());
            Assert.AreEqual(tagging.Version, taggings[0].Version);
//            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggings[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Removed, taggings[0].WorkflowState);
            Assert.AreEqual(tagging.Id, taggings[0].Id);
            Assert.AreEqual(tagging.TaggerId, taggings[0].TaggerId);
            Assert.AreEqual(tagging.CheckListId, checklist.Id);
            Assert.AreEqual(tagging.TagId, tag.Id);

            //Version 1
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggingVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, taggingVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), taggingVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Created, taggingVersions[0].WorkflowState);
            Assert.AreEqual(tagging.Id, taggingVersions[0].TaggingId);
            Assert.AreEqual(tagging.TaggerId, taggingVersions[0].TaggerId);
            Assert.AreEqual(tag.Id, taggingVersions[0].TagId);
            Assert.AreEqual(checklist.Id, taggingVersions[0].CheckListId);

            //Version 2 Deleted Version
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggingVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, taggingVersions[1].Version);
//            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggingVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Removed, taggingVersions[1].WorkflowState);
            Assert.AreEqual(tagging.Id, taggingVersions[1].TaggingId);
            Assert.AreEqual(tagging.TaggerId, taggingVersions[1].TaggerId);
            Assert.AreEqual(tag.Id, taggingVersions[1].TagId);
            Assert.AreEqual(checklist.Id, taggingVersions[1].CheckListId);
        }
    }
}