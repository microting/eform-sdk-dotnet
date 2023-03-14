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
    public class TagsUTest : DbTestFixture
    {
        [Test]
        public async Task Tags_Create_DoesCreate()
        {
            //Arrange

            Random rnd = new Random();

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };

            //Act

            await tag.Create(DbContext).ConfigureAwait(false);

            List<Tag> tags = DbContext.Tags.AsNoTracking().ToList();
            List<TagVersion> tagVersions = DbContext.TagVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(tags);
            Assert.NotNull(tagVersions);

            Assert.AreEqual(1, tags.Count());
            Assert.AreEqual(1, tagVersions.Count());

            Assert.AreEqual(tag.CreatedAt.ToString(), tags[0].CreatedAt.ToString());
            Assert.AreEqual(tag.Version, tags[0].Version);
//            Assert.AreEqual(tag.UpdatedAt.ToString(), tags[0].UpdatedAt.ToString());
            Assert.AreEqual(tags[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tag.Name, tags[0].Name);
            Assert.AreEqual(tag.Id, tags[0].Id);

            //Versions
            Assert.AreEqual(tag.CreatedAt.ToString(), tagVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, tagVersions[0].Version);
//            Assert.AreEqual(tag.UpdatedAt.ToString(), tagVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(tagVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tag.Name, tagVersions[0].Name);
            Assert.AreEqual(tag.Id, tagVersions[0].TagId);
        }

        [Test]
        public async Task Tags_Update_DoesUpdate()
        {
            Random rnd = new Random();

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };

            await tag.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = tag.UpdatedAt;
            string oldName = tag.Name;
            int? oldTaggingsCount = tag.TaggingsCount;
            int? oldId = tag.Id;

            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);

            await tag.Update(DbContext).ConfigureAwait(false);

            List<Tag> tags = DbContext.Tags.AsNoTracking().ToList();
            List<TagVersion> tagVersions = DbContext.TagVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(tags);
            Assert.NotNull(tagVersions);

            Assert.AreEqual(1, tags.Count());
            Assert.AreEqual(2, tagVersions.Count());

            Assert.AreEqual(tag.CreatedAt.ToString(), tags[0].CreatedAt.ToString());
            Assert.AreEqual(tag.Version, tags[0].Version);
//            Assert.AreEqual(tag.UpdatedAt.ToString(), tags[0].UpdatedAt.ToString());
            Assert.AreEqual(tags[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tag.Name, tags[0].Name);
            Assert.AreEqual(tag.Id, tags[0].Id);

            //Version 1 Old Version
            Assert.AreEqual(tag.CreatedAt.ToString(), tagVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, tagVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), tagVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(tagVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(oldName, tagVersions[0].Name);
            Assert.AreEqual(oldId, tagVersions[0].TagId);

            //Version 2 Updated Version
            Assert.AreEqual(tag.CreatedAt.ToString(), tagVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, tagVersions[1].Version);
//            Assert.AreEqual(tag.UpdatedAt.ToString(), tagVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(tagVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tag.Name, tagVersions[1].Name);
            Assert.AreEqual(tag.Id, tagVersions[1].TagId);
        }

        [Test]
        public async Task Tags_Delete_DoesSetWorkflowStateToRemoved()
        {
            Random rnd = new Random();

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };

            await tag.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = tag.UpdatedAt;

            await tag.Delete(DbContext);

            List<Tag> tags = DbContext.Tags.AsNoTracking().ToList();
            List<TagVersion> tagVersions = DbContext.TagVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(tags);
            Assert.NotNull(tagVersions);

            Assert.AreEqual(1, tags.Count());
            Assert.AreEqual(2, tagVersions.Count());

            Assert.AreEqual(tag.CreatedAt.ToString(), tags[0].CreatedAt.ToString());
            Assert.AreEqual(tag.Version, tags[0].Version);
//            Assert.AreEqual(tag.UpdatedAt.ToString(), tags[0].UpdatedAt.ToString());
            Assert.AreEqual(tags[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(tag.Name, tags[0].Name);
            Assert.AreEqual(tag.Id, tags[0].Id);

            //Version 1 Old Version
            Assert.AreEqual(tag.CreatedAt.ToString(), tagVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, tagVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), tagVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(tagVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tag.Name, tagVersions[0].Name);
            Assert.AreEqual(tag.Id, tagVersions[0].TagId);

            //Version 2 Updated Version
            Assert.AreEqual(tag.CreatedAt.ToString(), tagVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, tagVersions[1].Version);
//            Assert.AreEqual(tag.UpdatedAt.ToString(), tagVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(tagVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(tag.Name, tagVersions[1].Name);
            Assert.AreEqual(tag.Id, tagVersions[1].TagId);
        }
    }
}