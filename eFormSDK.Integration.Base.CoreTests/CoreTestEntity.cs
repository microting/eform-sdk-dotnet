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
            Assert.AreEqual(matchEntityGroupAllSearchCreated.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreated.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreated.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectCreated

            Assert.NotNull(matchEntityGroupAllSelectCreated);
            Assert.AreEqual(matchEntityGroupAllSelectCreated.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreated.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreated.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchRemoved

            Assert.NotNull(matchEntityGroupAllSearchRemoved);
            Assert.AreEqual(matchEntityGroupAllSearchRemoved.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemoved.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectRemoved

            Assert.NotNull(matchEntityGroupAllSelectRemoved);
            Assert.AreEqual(matchEntityGroupAllSelectRemoved.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemoved.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchNotRemoved

            Assert.NotNull(matchEntityGroupAllSearchNotRemoved);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemoved.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemoved.EntityGroups.Count, 20);

            #endregion

            #region matchEntityGroupAllSelectNotRemoved

            Assert.NotNull(matchEntityGroupAllSelectNotRemoved);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemoved.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemoved.EntityGroups.Count, 20);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDesc.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDesc.EntityGroups.Count, 10);

            #endregion

            #region matchEntityGroupAllSelectCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDesc.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDesc.EntityGroups.Count, 10);

            #endregion

            #region matchEntityGroupAllSearchRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDesc.PageNum, 0);

            #endregion

            #region matchEntityGroupAllSelectRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDesc.PageNum, 0);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDesc.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDesc.PageNum, 0);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDesc.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDesc.PageNum, 0);

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoNameFilter.EntityGroups.Count(), 20);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoNameFilter.EntityGroups.Count(), 20);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.EntityGroups.Count(), 20);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.EntityGroups.Count(), 20);

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoSort.EntityGroups.Count(), 20);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoSort.EntityGroups.Count(), 20);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoSort.EntityGroups.Count(), 20);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoSort.EntityGroups.Count(), 20);

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
            Assert.AreEqual(matchEntityGroupAllSearchCreated.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreated.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreated.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreated

            Assert.NotNull(matchEntityGroupAllSelectCreated);
            Assert.AreEqual(matchEntityGroupAllSelectCreated.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreated.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreated.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemoved

            Assert.NotNull(matchEntityGroupAllSearchRemoved);
            Assert.AreEqual(matchEntityGroupAllSearchRemoved.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemoved.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemoved

            Assert.NotNull(matchEntityGroupAllSelectRemoved);
            Assert.AreEqual(matchEntityGroupAllSelectRemoved.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemoved.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemoved

            Assert.NotNull(matchEntityGroupAllSearchNotRemoved);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemoved.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemoved.EntityGroups.Count, 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemoved

            Assert.NotNull(matchEntityGroupAllSelectNotRemoved);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemoved.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemoved.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemoved.EntityGroups.Count, 10);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDesc.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDesc.EntityGroups.Count, 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedWDesc

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDesc.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDesc.EntityGroups.Count, 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDesc.PageNum, 0);

            #endregion

            #region matchEntityGroupAllSelectRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDesc.PageNum, 0);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDesc.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDesc.PageNum, 0);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDesc

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDesc.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDesc.PageNum, 0);

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedNoSort.EntityGroups.Count(), 10);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedWDescNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoSort.PageNum, 0);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedWDescNoSort.EntityGroups.Count(), 10);

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
            Assert.AreEqual(matchEntityGroupAllSearchCreatedB.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedB.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedB.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedB

            Assert.NotNull(matchEntityGroupAllSelectCreatedB);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedB.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedB.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedB.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedB

            Assert.NotNull(matchEntityGroupAllSearchRemovedB);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedB.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedB.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedB.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedB

            Assert.NotNull(matchEntityGroupAllSelectRemovedB);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedB.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedB.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedB.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedB

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedB);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedB.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedB.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedB.EntityGroups.Count, 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedB

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedB);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedB.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedB.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedB.EntityGroups.Count, 10);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDesc

            Assert.NotNull(matchEntityGroupAllSearchCreatedBWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDesc.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDesc.EntityGroups.Count, 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDesc

            Assert.NotNull(matchEntityGroupAllSelectCreatedBWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDesc.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDesc.EntityGroups.Count, 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSearchRemovedBWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDesc.PageNum, 1);

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSelectRemovedBWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDesc.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDesc.PageNum, 1);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBWDesc);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDesc.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDesc.PageNum, 1);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDesc

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBWDesc);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDesc.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDesc.PageNum, 1);

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedBNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedBNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedBNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedBNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchCreatedBWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectCreatedBWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchRemovedBWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectRemovedBWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.EntityGroups.Count(), 10);

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedBNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedBNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedBNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedBNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedBNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedBNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBNoSort.EntityGroups.Count(), 10);

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchCreatedBWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDescNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchCreatedBWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectCreatedBWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDescNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectCreatedBWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchRemovedBWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDescNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchRemovedBWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectRemovedBWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDescNoSort.NumOfElements, 10);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDescNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectRemovedBWDescNoSort.EntityGroups.Count(), 5);

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSearchNotRemovedBWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDescNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDescNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSearchNotRemovedBWDescNoSort.EntityGroups.Count(), 10);

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDescNoSort

            Assert.NotNull(matchEntityGroupAllSelectNotRemovedBWDescNoSort);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDescNoSort.NumOfElements, 20);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDescNoSort.PageNum, 1);
            Assert.AreEqual(matchEntityGroupAllSelectNotRemovedBWDescNoSort.EntityGroups.Count(), 10);

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
            Assert.AreEqual(1, items.Count());
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
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, items[0].WorkflowState);
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
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, items[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Created, result_item.WorkflowState);
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
            Assert.AreEqual(1, items.Count());
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
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, items[0].WorkflowState);
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
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, items[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Created, result_item.WorkflowState);
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