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
    public class FoldersUTest : DbTestFixture
    {
        [Test]
        public async Task Folders_Create_DoesCreate()
        {
            //Arrange

            Random rnd = new Random();

            Folder parentFolder = new Folder
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await parentFolder.Create(DbContext).ConfigureAwait(false);

            Folder folder = new Folder
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255),
                ParentId = parentFolder.Id
            };

            //Act

            await folder.Create(DbContext).ConfigureAwait(false);

            List<Folder> folders = DbContext.Folders.AsNoTracking().ToList();
            List<FolderVersion> folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();

            Assert.That(folders, Is.Not.EqualTo(null));
            Assert.That(folderVersions, Is.Not.EqualTo(null));

            Assert.That(folders.Count(), Is.EqualTo(2));
            Assert.That(folderVersions.Count(), Is.EqualTo(2));

            Assert.That(folders[1].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folders[1].Version, Is.EqualTo(folder.Version));
            Assert.That(folders[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(folders[1].Id, Is.EqualTo(folder.Id));
            Assert.That(folders[1].Description, Is.EqualTo(folder.Description));
            Assert.That(folders[1].Name, Is.EqualTo(folder.Name));
            Assert.That(folders[1].MicrotingUid, Is.EqualTo(folder.MicrotingUid));
            Assert.That(parentFolder.Id, Is.EqualTo(folder.ParentId));

            //Versions

            Assert.That(folderVersions[1].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folderVersions[1].Version, Is.EqualTo(1));
            Assert.That(folderVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(folderVersions[1].FolderId, Is.EqualTo(folder.Id));
            Assert.That(folderVersions[1].Description, Is.EqualTo(folder.Description));
            Assert.That(folderVersions[1].Name, Is.EqualTo(folder.Name));
            Assert.That(folderVersions[1].MicrotingUid, Is.EqualTo(folder.MicrotingUid));
            Assert.That(folderVersions[1].ParentId, Is.EqualTo(parentFolder.Id));
        }

        [Test]
        public async Task Folders_Update_DoesUpdate()
        {
            //Arrange

            Random rnd = new Random();

            Folder parentFolder = new Folder
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await parentFolder.Create(DbContext).ConfigureAwait(false);

            Folder folder = new Folder
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255),
                ParentId = parentFolder.Id
            };
            await folder.Create(DbContext).ConfigureAwait(false);

            //Act
            DateTime? oldUpdatedAt = folder.UpdatedAt;
            string oldDescription = folder.Description;
            string oldName = folder.Name;
            int? oldMicrotingUid = folder.MicrotingUid;

            folder.Description = Guid.NewGuid().ToString();
            folder.Name = Guid.NewGuid().ToString();
            folder.MicrotingUid = rnd.Next(1, 255);
            await folder.Update(DbContext).ConfigureAwait(false);

            List<Folder> folders = DbContext.Folders.AsNoTracking().ToList();
            List<FolderVersion> folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();

            Assert.That(folders, Is.Not.EqualTo(null));
            Assert.That(folderVersions, Is.Not.EqualTo(null));

            Assert.That(folders.Count(), Is.EqualTo(2));
            Assert.That(folderVersions.Count(), Is.EqualTo(3));

            Assert.That(folders[1].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folders[1].Version, Is.EqualTo(folder.Version));
            Assert.That(folders[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(folders[1].Id, Is.EqualTo(folder.Id));
            Assert.That(folders[1].Description, Is.EqualTo(folder.Description));
            Assert.That(folders[1].Name, Is.EqualTo(folder.Name));
            Assert.That(folders[1].MicrotingUid, Is.EqualTo(folder.MicrotingUid));
            Assert.That(parentFolder.Id, Is.EqualTo(folder.ParentId));

            //Old Version

            Assert.That(folderVersions[1].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folderVersions[1].Version, Is.EqualTo(1));
            Assert.That(folderVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(folderVersions[1].FolderId, Is.EqualTo(folder.Id));
            Assert.That(folderVersions[1].Description, Is.EqualTo(oldDescription));
            Assert.That(folderVersions[1].Name, Is.EqualTo(oldName));
            Assert.That(folderVersions[1].MicrotingUid, Is.EqualTo(oldMicrotingUid));
            Assert.That(folderVersions[1].ParentId, Is.EqualTo(parentFolder.Id));

            //New Version

            Assert.That(folderVersions[2].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folderVersions[2].Version, Is.EqualTo(2));
            Assert.That(folderVersions[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(folderVersions[2].FolderId, Is.EqualTo(folder.Id));
            Assert.That(folderVersions[2].Description, Is.EqualTo(folder.Description));
            Assert.That(folderVersions[2].Name, Is.EqualTo(folder.Name));
            Assert.That(folderVersions[2].MicrotingUid, Is.EqualTo(folder.MicrotingUid));
            Assert.That(folderVersions[2].ParentId, Is.EqualTo(parentFolder.Id));
        }

        [Test]
        public async Task Folders_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Random rnd = new Random();

            Folder parentFolder = new Folder
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await parentFolder.Create(DbContext).ConfigureAwait(false);

            Folder folder = new Folder
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255),
                ParentId = parentFolder.Id
            };
            await folder.Create(DbContext).ConfigureAwait(false);

            //Act

            await folder.Delete(DbContext);

            List<Folder> folders = DbContext.Folders.AsNoTracking().ToList();
            List<FolderVersion> folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();

            Assert.That(folders, Is.Not.EqualTo(null));
            Assert.That(folderVersions, Is.Not.EqualTo(null));

            Assert.That(folders.Count(), Is.EqualTo(2));
            Assert.That(folderVersions.Count(), Is.EqualTo(3));

            Assert.That(folders[1].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folders[1].Version, Is.EqualTo(folder.Version));
            Assert.That(folders[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
            Assert.That(folders[1].Id, Is.EqualTo(folder.Id));
            Assert.That(folders[1].Description, Is.EqualTo(folder.Description));
            Assert.That(folders[1].Name, Is.EqualTo(folder.Name));
            Assert.That(folders[1].MicrotingUid, Is.EqualTo(folder.MicrotingUid));
            Assert.That(parentFolder.Id, Is.EqualTo(folder.ParentId));

            //Old Version

            Assert.That(folderVersions[1].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folderVersions[1].Version, Is.EqualTo(1));
            Assert.That(folderVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(folderVersions[1].FolderId, Is.EqualTo(folder.Id));
            Assert.That(folderVersions[1].Description, Is.EqualTo(folder.Description));
            Assert.That(folderVersions[1].Name, Is.EqualTo(folder.Name));
            Assert.That(folderVersions[1].MicrotingUid, Is.EqualTo(folder.MicrotingUid));
            Assert.That(folderVersions[1].ParentId, Is.EqualTo(parentFolder.Id));

            //New Version

            Assert.That(folderVersions[2].CreatedAt.ToString(), Is.EqualTo(folder.CreatedAt.ToString()));
            Assert.That(folderVersions[2].Version, Is.EqualTo(2));
            Assert.That(folderVersions[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
            Assert.That(folderVersions[2].FolderId, Is.EqualTo(folder.Id));
            Assert.That(folderVersions[2].Description, Is.EqualTo(folder.Description));
            Assert.That(folderVersions[2].Name, Is.EqualTo(folder.Name));
            Assert.That(folderVersions[2].MicrotingUid, Is.EqualTo(folder.MicrotingUid));
            Assert.That(folderVersions[2].ParentId, Is.EqualTo(parentFolder.Id));
        }
    }
}