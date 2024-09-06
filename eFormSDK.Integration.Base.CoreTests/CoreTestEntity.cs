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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using EntityGroup = Microting.eForm.Infrastructure.Data.Entities.EntityGroup;
using EntityItem = Microting.eForm.Infrastructure.Data.Entities.EntityItem;

namespace eFormSDK.Integration.Base.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTestEntity : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            await sql.SettingUpdate(Settings.comAddressNewApi, "none");

            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            await sut.StartSqlOnly(ConnectionString);
            path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            //sut.StartLog(new CoreBase());
        }

        #region Entity

        [Test]
        public async Task Core_EntityGroup_Advanced_EntityGroupAll_ReturnsEntityGroups()
        {
            // Arrance

            #region Arrance

            #region Entity Groups

            #region EntitySearch

            #region Created

            #region eG1

            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG2

            EntityGroup eG2 = await testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG3

            EntityGroup eG3 = await testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG4

            EntityGroup eG4 = await testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG5

            EntityGroup eG5 = await testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG6

            EntityGroup eG6 = await testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG7

            EntityGroup eG7 = await testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG8

            EntityGroup eG8 = await testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG9

            EntityGroup eG9 = await testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG10

            EntityGroup eG10 = await testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #region Removed

            #region eG1

            EntityGroup eG1Removed = await testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG2

            EntityGroup eG2Removed = await testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG3

            EntityGroup eG3Removed = await testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG4

            EntityGroup eG4Removed = await testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG5

            EntityGroup eG5Removed = await testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG6

            EntityGroup eG6Removed = await testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG7

            EntityGroup eG7Removed = await testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG8

            EntityGroup eG8Removed = await testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG9

            EntityGroup eG9Removed = await testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG10

            EntityGroup eG10Removed = await testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #endregion

            #region Retracted

            #region eG1

            EntityGroup eG1Retracted = await testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG2

            EntityGroup eG2Retracted = await testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG3

            EntityGroup eG3Retracted = await testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG4

            EntityGroup eG4Retracted = await testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG5

            EntityGroup eG5Retracted = await testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG6

            EntityGroup eG6Retracted = await testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG7

            EntityGroup eG7Retracted = await testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG8

            EntityGroup eG8Retracted = await testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG9

            EntityGroup eG9Retracted = await testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG10

            EntityGroup eG10Retracted = await testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #endregion

            #endregion

            #region EntitySelect

            #region Created

            #region eG1

            EntityGroup eG1Select = await testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG2

            EntityGroup eG2Select = await testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG3

            EntityGroup eG3Select = await testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG4

            EntityGroup eG4Select = await testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG5

            EntityGroup eG5Select = await testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG6

            EntityGroup eG6Select = await testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG7

            EntityGroup eG7Select = await testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG8

            EntityGroup eG8Select = await testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG9

            EntityGroup eG9Select = await testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG10

            EntityGroup eG10Select = await testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #region Removed

            #region eG1

            EntityGroup eG1SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG2

            EntityGroup eG2SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG3

            EntityGroup eG3SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG4

            EntityGroup eG4SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG5

            EntityGroup eG5SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG6

            EntityGroup eG6SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG7

            EntityGroup eG7SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG8

            EntityGroup eG8SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG9

            EntityGroup eG9SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG10

            EntityGroup eG10SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR10",
                "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #endregion

            #region Retracted

            #region eG1

            EntityGroup eG1SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST1",
                "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG2

            EntityGroup eG2SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST2",
                "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG3

            EntityGroup eG3SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST3",
                "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG4

            EntityGroup eG4SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST4",
                "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG5

            EntityGroup eG5SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST5",
                "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG6

            EntityGroup eG6SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST6",
                "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG7

            EntityGroup eG7SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST7",
                "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG8

            EntityGroup eG8SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST8",
                "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG9

            EntityGroup eG9SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST9",
                "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG10

            EntityGroup eG10SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST10",
                "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion

            // Act

            #region pageindex 0

            #region pageSize 10

            #region Default Sorting

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreated = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreated = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                0, 20, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                0, 20, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 20, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 20, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoNameFilter =
                await sut.Advanced_EntityGroupAll("id", "", 0, 20, Constants.FieldTypes.EntitySearch, true,
                    Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoNameFilter =
                await sut.Advanced_EntityGroupAll("id", "", 0, 20, Constants.FieldTypes.EntitySelect, true,
                    Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 20, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 20, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #endregion

            #endregion


            // Assert

            #region pageIndex 0

            #region pageSize 10

            #region Def Sort

            #region Asc

            #region matchEntityGroupAllSearchCreated

            Assert.NotNull(matchEntityGroupAllSearchCreated);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreated.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreated.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreated

            Assert.NotNull(matchEntityGroupAllSelectCreated);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreated.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreated.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemoved

            Assert.NotNull(matchEntityGroupAllSearchRemoved);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemoved

            Assert.NotNull(matchEntityGroupAllSelectRemoved);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemoved

            Assert.NotNull(matchEntityGroupAllSearchNotRemoved);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectNotRemoved

            Assert.NotNull(matchEntityGroupAllSelectNotRemoved);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.EntityGroups.Count));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDesc);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDesc);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDesc);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.PageNum));

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoNameFilter);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoNameFilter);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoSort);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoSort);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoSort.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion
        }

        [Test]
        public async Task Core_EntityGroup_Advanced_EntityGroupAll_ReturnsEntityGroupsPaged()
        {
            // Arrance

            #region Arrance

            #region Entity Groups

            #region EntitySearch

            #region Created

            #region eG1

            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG2

            EntityGroup eG2 = await testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG3

            EntityGroup eG3 = await testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG4

            EntityGroup eG4 = await testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG5

            EntityGroup eG5 = await testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG6

            EntityGroup eG6 = await testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG7

            EntityGroup eG7 = await testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG8

            EntityGroup eG8 = await testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG9

            EntityGroup eG9 = await testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #region eG10

            EntityGroup eG10 = await testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #region Removed

            #region eG1

            EntityGroup eG1Removed = await testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG2

            EntityGroup eG2Removed = await testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG3

            EntityGroup eG3Removed = await testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG4

            EntityGroup eG4Removed = await testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG5

            EntityGroup eG5Removed = await testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG6

            EntityGroup eG6Removed = await testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG7

            EntityGroup eG7Removed = await testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG8

            EntityGroup eG8Removed = await testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG9

            EntityGroup eG9Removed = await testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #region eG10

            EntityGroup eG10Removed = await testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);

            #endregion

            #endregion

            #region Retracted

            #region eG1

            EntityGroup eG1Retracted = await testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG2

            EntityGroup eG2Retracted = await testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG3

            EntityGroup eG3Retracted = await testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG4

            EntityGroup eG4Retracted = await testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG5

            EntityGroup eG5Retracted = await testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG6

            EntityGroup eG6Retracted = await testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG7

            EntityGroup eG7Retracted = await testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG8

            EntityGroup eG8Retracted = await testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG9

            EntityGroup eG9Retracted = await testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG10

            EntityGroup eG10Retracted = await testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);

            #endregion

            #endregion

            #endregion

            #region EntitySelect

            #region Created

            #region eG1

            EntityGroup eG1Select = await testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG2

            EntityGroup eG2Select = await testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG3

            EntityGroup eG3Select = await testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG4

            EntityGroup eG4Select = await testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG5

            EntityGroup eG5Select = await testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG6

            EntityGroup eG6Select = await testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG7

            EntityGroup eG7Select = await testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG8

            EntityGroup eG8Select = await testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG9

            EntityGroup eG9Select = await testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #region eG10

            EntityGroup eG10Select = await testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #region Removed

            #region eG1

            EntityGroup eG1SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG2

            EntityGroup eG2SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG3

            EntityGroup eG3SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG4

            EntityGroup eG4SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG5

            EntityGroup eG5SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG6

            EntityGroup eG6SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG7

            EntityGroup eG7SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG8

            EntityGroup eG8SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG9

            EntityGroup eG9SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #region eG10

            EntityGroup eG10SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR10",
                "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);

            #endregion

            #endregion

            #region Retracted

            #region eG1

            EntityGroup eG1SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST1",
                "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG2

            EntityGroup eG2SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST2",
                "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG3

            EntityGroup eG3SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST3",
                "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG4

            EntityGroup eG4SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST4",
                "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG5

            EntityGroup eG5SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST5",
                "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG6

            EntityGroup eG6SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST6",
                "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG7

            EntityGroup eG7SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST7",
                "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG8

            EntityGroup eG8SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST8",
                "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG9

            EntityGroup eG9SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST9",
                "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #region eG10

            EntityGroup eG10SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST10",
                "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion

            // Act

            #region pageindex 0

            #region pageSize 5

            #region Default Sorting

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreated = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreated = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemoved = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoNameFilter =
                await sut.Advanced_EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySearch, true,
                    Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoNameFilter =
                await sut.Advanced_EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySelect, true,
                    Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #endregion

            #endregion

            #region pageindex 1

            #region pageSize 5

            #region Default Sorting

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedB = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedB = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedB = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedB = await sut.Advanced_EntityGroupAll("id", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedB = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedB = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedBWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDesc = await sut.Advanced_EntityGroupAll("id",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedBNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBNoNameFilter = await sut.Advanced_EntityGroupAll("id", "",
                1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 1, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 1, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedBWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDescNoNameFilter = await sut.Advanced_EntityGroupAll("id",
                "", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter =
                await sut.Advanced_EntityGroupAll("id", "", 1, 10, Constants.FieldTypes.EntitySearch, true,
                    Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter =
                await sut.Advanced_EntityGroupAll("id", "", 1, 10, Constants.FieldTypes.EntitySelect, true,
                    Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedBNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedBWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDescNoSort = await sut.Advanced_EntityGroupAll("",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #endregion

            #endregion


            // Assert

            #region pageIndex 0

            #region pageSize 5

            #region Def Sort

            #region Asc

            #region matchEntityGroupAllSearchCreated

            Assert.NotNull(matchEntityGroupAllSearchCreated);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreated.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreated.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreated

            Assert.NotNull(matchEntityGroupAllSelectCreated);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreated.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreated.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemoved

            Assert.NotNull(matchEntityGroupAllSearchRemoved);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemoved.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemoved

            Assert.NotNull(matchEntityGroupAllSelectRemoved);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemoved.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemoved

            Assert.NotNull(matchEntityGroupAllSearchNotRemoved);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectNotRemoved

            Assert.NotNull(matchEntityGroupAllSelectNotRemoved);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.EntityGroups.Count));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDesc);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDesc);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDesc);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.PageNum));

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoNameFilter);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoNameFilter);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoSort);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoSort);
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion

            #region pageIndex 1

            #region pageSize 5

            #region Def Sort

            #region Asc

            #region matchEntityGroupAllSearchCreatedB

            Assert.NotNull(matchEntityGroupAllSearchCreatedB);
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedB.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedB.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedB

            Assert.NotNull(matchEntityGroupAllSelectCreatedB);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedB.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedB

            Assert.NotNull(matchEntityGroupAllSearchRemovedB);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedB.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedB

            Assert.NotNull(matchEntityGroupAllSelectRemovedB);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedB.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedB

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedB);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedB.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedB.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedB

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedB);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedB.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedB.EntityGroups.Count));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDesc

            Assert.NotNull(matchEntityGroupAllSearchCreatedBWDesc);
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDesc.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDesc

            Assert.NotNull(matchEntityGroupAllSelectCreatedBWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDesc.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSearchRemovedBWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSelectRemovedBWDesc);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBWDesc);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBWDesc);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDesc.PageNum));

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedBNoNameFilter);
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedBNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedBNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedBNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedBWDescNoNameFilter);
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedBWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedBWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedBWDescNoNameFilter);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedBNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedBNoSort);
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedBNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedBNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedBNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedBWDescNoSort);
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedBWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedBWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedBWDescNoSort);
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBWDescNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBWDescNoSort);
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion
        }


        [Test]
        public async Task Core_EntitySearchItemCreate_CreatesEntitySearchItem()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);

            // Act
            await sut.EntitySearchItemCreate(eG1.Id, "Jon Doe", "", "");

            List<EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.That(items.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Core_EntitySearchItemDelete_DeletesEntitySearchItem()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            EntityItem et = await testHelpers.CreateEntityItem("", 0, eG1.Id, "", "", "Jon Doe", 1, 0,
                Constants.WorkflowStates.Created);

            // Act
            await sut.EntityItemDelete(et.Id);
            List<EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.That(items.Count(), Is.EqualTo(1));
            Assert.That(items[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        }

        [Test]
        public async Task Core_EntitySearchItemCreateExistingRemovedItem_ChangesWorkflowStateToCreated()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            EntityItem et = await testHelpers.CreateEntityItem("", 0, eG1.Id, "", "", "Jon Doe", 1, 0,
                Constants.WorkflowStates.Removed);

            // Act
            Microting.eForm.Infrastructure.Models.EntityItem result_item =
                await sut.EntitySearchItemCreate(eG1.Id, "Jon Doe", "", "");
            List<EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.That(items.Count(), Is.EqualTo(1));
            Assert.That(items[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(result_item.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        }

        [Test]
        public async Task Core_EntitySelectItemCreate_CreatesEntitySelectItem()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);

            // Act
            await sut.EntitySelectItemCreate(eG1.Id, "Jon Doe", 0, "");

            List<EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.That(items.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Core_EntitySelectItemDelete_DeletesEntitySelectItem()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            EntityItem et = await testHelpers.CreateEntityItem("", 0, eG1.Id, "", "", "Jon Doe", 1, 0,
                Constants.WorkflowStates.Created);

            // Act
            await sut.EntityItemDelete(et.Id);
            List<EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.That(items.Count(), Is.EqualTo(1));
            Assert.That(items[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        }

        [Test]
        public async Task Core_EntitySelectItemCreateExistingRemovedItem_ChangesWorkflowStateToCreated()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            EntityItem et = await testHelpers.CreateEntityItem("", 0, eG1.Id, "", "", "Jon Doe", 1, 0,
                Constants.WorkflowStates.Removed);

            // Act
            Microting.eForm.Infrastructure.Models.EntityItem result_item =
                await sut.EntitySelectItemCreate(eG1.Id, "Jon Doe", 0, "");
            List<EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.That(items.Count(), Is.EqualTo(1));
            Assert.That(items[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(result_item.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        }

        #endregion

        #region eventhandlers

        public void EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        #endregion
    }
}