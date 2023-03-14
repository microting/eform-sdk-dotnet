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

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(2, folders.Count());
            Assert.AreEqual(2, folderVersions.Count());

            Assert.AreEqual(folder.CreatedAt.ToString(), folders[1].CreatedAt.ToString());
            Assert.AreEqual(folder.Version, folders[1].Version);
            Assert.AreEqual(folders[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folders[1].Id);
            Assert.AreEqual(folder.Description, folders[1].Description);
            Assert.AreEqual(folder.Name, folders[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folders[1].MicrotingUid);
            Assert.AreEqual(folder.ParentId, parentFolder.Id);

            //Versions

            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[1].CreatedAt.ToString());
            Assert.AreEqual(1, folderVersions[1].Version);
            Assert.AreEqual(folderVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[1].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[1].Description);
            Assert.AreEqual(folder.Name, folderVersions[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[1].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[1].ParentId);
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

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(2, folders.Count());
            Assert.AreEqual(3, folderVersions.Count());

            Assert.AreEqual(folder.CreatedAt.ToString(), folders[1].CreatedAt.ToString());
            Assert.AreEqual(folder.Version, folders[1].Version);
            Assert.AreEqual(folders[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folders[1].Id);
            Assert.AreEqual(folder.Description, folders[1].Description);
            Assert.AreEqual(folder.Name, folders[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folders[1].MicrotingUid);
            Assert.AreEqual(folder.ParentId, parentFolder.Id);

            //Old Version

            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[1].CreatedAt.ToString());
            Assert.AreEqual(1, folderVersions[1].Version);
            Assert.AreEqual(folderVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[1].FolderId);
            Assert.AreEqual(oldDescription, folderVersions[1].Description);
            Assert.AreEqual(oldName, folderVersions[1].Name);
            Assert.AreEqual(oldMicrotingUid, folderVersions[1].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[1].ParentId);

            //New Version

            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[2].CreatedAt.ToString());
            Assert.AreEqual(2, folderVersions[2].Version);
            Assert.AreEqual(folderVersions[2].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[2].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[2].Description);
            Assert.AreEqual(folder.Name, folderVersions[2].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[2].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[2].ParentId);
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

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(2, folders.Count());
            Assert.AreEqual(3, folderVersions.Count());

            Assert.AreEqual(folder.CreatedAt.ToString(), folders[1].CreatedAt.ToString());
            Assert.AreEqual(folder.Version, folders[1].Version);
            Assert.AreEqual(folders[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(folder.Id, folders[1].Id);
            Assert.AreEqual(folder.Description, folders[1].Description);
            Assert.AreEqual(folder.Name, folders[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folders[1].MicrotingUid);
            Assert.AreEqual(folder.ParentId, parentFolder.Id);

            //Old Version

            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[1].CreatedAt.ToString());
            Assert.AreEqual(1, folderVersions[1].Version);
            Assert.AreEqual(folderVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[1].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[1].Description);
            Assert.AreEqual(folder.Name, folderVersions[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[1].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[1].ParentId);

            //New Version

            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[2].CreatedAt.ToString());
            Assert.AreEqual(2, folderVersions[2].Version);
            Assert.AreEqual(folderVersions[2].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(folder.Id, folderVersions[2].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[2].Description);
            Assert.AreEqual(folder.Name, folderVersions[2].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[2].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[2].ParentId);
        }
    }
}