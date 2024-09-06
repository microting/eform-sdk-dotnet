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

            Assert.That(taggings.Count(), Is.EqualTo(1));
            Assert.That(taggingVersions.Count(), Is.EqualTo(1));

            Assert.That(taggings[0].CreatedAt.ToString(), Is.EqualTo(tagging.CreatedAt.ToString()));
            Assert.That(taggings[0].Version, Is.EqualTo(tagging.Version));
            //            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggings[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(taggings[0].WorkflowState));
            Assert.That(taggings[0].Id, Is.EqualTo(tagging.Id));
            Assert.That(taggings[0].TaggerId, Is.EqualTo(tagging.TaggerId));
            Assert.That(tag.Id, Is.EqualTo(tagging.TagId));
            Assert.That(checklist.Id, Is.EqualTo(tagging.CheckListId));

            //Version 1
            Assert.That(taggingVersions[0].CreatedAt.ToString(), Is.EqualTo(tagging.CreatedAt.ToString()));
            Assert.That(taggingVersions[0].Version, Is.EqualTo(tagging.Version));
            //            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggingVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(taggingVersions[0].WorkflowState));
            Assert.That(taggingVersions[0].Id, Is.EqualTo(tagging.Id));
            Assert.That(taggingVersions[0].TaggerId, Is.EqualTo(tagging.TaggerId));
            Assert.That(taggingVersions[0].TagId, Is.EqualTo(tag.Id));
            Assert.That(taggingVersions[0].CheckListId, Is.EqualTo(checklist.Id));
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

            Assert.That(taggings.Count(), Is.EqualTo(1));
            Assert.That(taggingVersions.Count(), Is.EqualTo(2));

            Assert.That(taggings[0].CreatedAt.ToString(), Is.EqualTo(tagging.CreatedAt.ToString()));
            Assert.That(taggings[0].Version, Is.EqualTo(tagging.Version));
            //            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggings[0].UpdatedAt.ToString());
            Assert.That(taggings[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
            Assert.That(taggings[0].Id, Is.EqualTo(tagging.Id));
            Assert.That(taggings[0].TaggerId, Is.EqualTo(tagging.TaggerId));
            Assert.That(checklist.Id, Is.EqualTo(tagging.CheckListId));
            Assert.That(tag.Id, Is.EqualTo(tagging.TagId));

            //Version 1
            Assert.That(taggingVersions[0].CreatedAt.ToString(), Is.EqualTo(tagging.CreatedAt.ToString()));
            Assert.That(taggingVersions[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), taggingVersions[0].UpdatedAt.ToString());
            Assert.That(taggingVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(taggingVersions[0].TaggingId, Is.EqualTo(tagging.Id));
            Assert.That(taggingVersions[0].TaggerId, Is.EqualTo(tagging.TaggerId));
            Assert.That(taggingVersions[0].TagId, Is.EqualTo(tag.Id));
            Assert.That(taggingVersions[0].CheckListId, Is.EqualTo(checklist.Id));

            //Version 2 Deleted Version
            Assert.That(taggingVersions[1].CreatedAt.ToString(), Is.EqualTo(tagging.CreatedAt.ToString()));
            Assert.That(taggingVersions[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggingVersions[1].UpdatedAt.ToString());
            Assert.That(taggingVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
            Assert.That(taggingVersions[1].TaggingId, Is.EqualTo(tagging.Id));
            Assert.That(taggingVersions[1].TaggerId, Is.EqualTo(tagging.TaggerId));
            Assert.That(taggingVersions[1].TagId, Is.EqualTo(tag.Id));
            Assert.That(taggingVersions[1].CheckListId, Is.EqualTo(checklist.Id));
        }
    }
}