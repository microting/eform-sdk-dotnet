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
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using EntityGroup = Microting.eForm.Infrastructure.Data.Entities.EntityGroup;

namespace eFormSDK.Integration.Base.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestEntity : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

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
        }

        #region Entity

        #region Entity Group

        [Test]
        public async Task SQL_EntityGroup_EntityGroupAll_ReturnsEntityGroups()
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

            EntityGroupList matchEntityGroupAllSearchCreated = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreated = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 20,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 20,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0,
                20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0,
                20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 20,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 20,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                20, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                20, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                0, 20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                0, 20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

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
        public async Task SQL_EntityGroup_EntityGroupAll_ReturnsEntityGroupsPaged()
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

            EntityGroupList matchEntityGroupAllSearchCreated = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreated = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemoved = await sut.EntityGroupAll("id", "EntityGroup", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDesc = await sut.EntityGroupAll("id", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoNameFilter = await sut.EntityGroupAll("id", "", 0, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0, 5,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoNameFilter = await sut.EntityGroupAll("id", "", 0,
                10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup", 0,
                5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #endregion

            #endregion

            #region pageindex 1

            #region pageSize 5

            #region Default Sorting

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedB = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedB = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedB = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedB = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedB = await sut.EntityGroupAll("id", "EntityGroup", 1, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedB = await sut.EntityGroupAll("id", "EntityGroup", 1, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedBWDesc = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDesc = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDesc = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDesc = await sut.EntityGroupAll("id", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDesc = await sut.EntityGroupAll("id", "EntityGroup", 1,
                10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDesc = await sut.EntityGroupAll("id", "EntityGroup", 1,
                10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedBNoNameFilter = await sut.EntityGroupAll("id", "", 1, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBNoNameFilter = await sut.EntityGroupAll("id", "", 1, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBNoNameFilter = await sut.EntityGroupAll("id", "", 1, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBNoNameFilter = await sut.EntityGroupAll("id", "", 1, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBNoNameFilter = await sut.EntityGroupAll("id", "", 1, 10,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBNoNameFilter = await sut.EntityGroupAll("id", "", 1, 10,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedBWDescNoNameFilter = await sut.EntityGroupAll("id", "", 1,
                5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDescNoNameFilter = await sut.EntityGroupAll("id", "", 1,
                5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDescNoNameFilter = await sut.EntityGroupAll("id", "", 1,
                5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDescNoNameFilter = await sut.EntityGroupAll("id", "", 1,
                5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter = await sut.EntityGroupAll("id", "",
                1, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter = await sut.EntityGroupAll("id", "",
                1, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);

            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc

            EntityGroupList matchEntityGroupAllSearchCreatedBNoSort = await sut.EntityGroupAll("", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBNoSort = await sut.EntityGroupAll("", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBNoSort = await sut.EntityGroupAll("", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBNoSort = await sut.EntityGroupAll("", "EntityGroup", 1, 5,
                Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBNoSort = await sut.EntityGroupAll("", "EntityGroup", 1,
                10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBNoSort = await sut.EntityGroupAll("", "EntityGroup", 1,
                10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);

            #endregion

            #region entityGroup Desc

            EntityGroupList matchEntityGroupAllSearchCreatedBWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDescNoSort = await sut.EntityGroupAll("", "EntityGroup",
                1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDescNoSort = await sut.EntityGroupAll("",
                "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDescNoSort = await sut.EntityGroupAll("",
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
        public async Task SQL_EntityGroup_EntityGroupCreate_CreatesNewEntityGroup()
        {
            // Arrance

            // Act
            var matchEntitySearch = await sut.EntityGroupCreate("eG1", Constants.FieldTypes.EntitySearch, "desc1");
            var matchEntitySelect = await sut.EntityGroupCreate("eG2", Constants.FieldTypes.EntitySelect, "desc2");
            // Assert
            Assert.AreEqual("eG1", matchEntitySearch.Name);
            Assert.AreEqual("eG2", matchEntitySelect.Name);
        }

        //TODO René needs to make migration
        [Test]
#pragma warning disable 1998
        public async Task SQL_EntityGroup_EntityGroupReadSorted_ReadsByParameter()
        {
            //// Arrance
            //#region Arrance

            //#region Entity Groups

            //#region EntitySearch
            //#region Created
            //#region eG1
            //entity_groups eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2 = await testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3 = await testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4 = await testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5 = await testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6 = await testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7 = await testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8 = await testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9 = await testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10 = await testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed

            //#region eG1
            //entity_groups eG1Removed = await testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2Removed = await testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3Removed = await testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4Removed = await testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5Removed = await testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6Removed = await testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7Removed = await testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8Removed = await testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9Removed = await testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10Removed = await testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted

            //#region eG1
            //entity_groups eG1Retracted = await testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2Retracted = await testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3Retracted = await testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4Retracted = await testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5Retracted = await testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6Retracted = await testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7Retracted = await testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8Retracted = await testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9Retracted = await testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10Retracted = await testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion

            //#region EntitySelect
            //#region Created
            //#region eG1
            //entity_groups eG1Select = await testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2Select = await testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3Select = await testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4Select = await testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5Select = await testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6Select = await testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7Select = await testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8Select = await testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9Select = await testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10Select = await testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed

            //#region eG1
            //entity_groups eG1SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted

            //#region eG1
            //entity_groups eG1SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion
            //#endregion

            //#region EntityItems

            //#region Created

            //entity_items eI1 = await testHelpers.CreateEntityItem("D1", 1, "", "eIUIdC1", "mUIdC1", "EntityItem1Created", 1, 1, Constants.WorkflowStates.Created);

            //#endregion

            //#endregion

            //#endregion
            //// Act
            //#region EntitySearch

            //#region Created
            //var entityGroupSortByIdCreated = await sut.EntityGroupReadSorted("microtingUIdC", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameCreated = await sut.EntityGroupReadSorted("microtingUIdC", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameCreated = await sut.EntityGroupReadSorted("microtingUIdC", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Removed
            //var entityGroupSortByIdRemoved = await sut.EntityGroupReadSorted("microtingUIdR", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameRemoved = await sut.EntityGroupReadSorted("microtingUIdR", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameRemoved = await sut.EntityGroupReadSorted("microtingUIdR", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Retracted
            //var entityGroupSortByIdRetracted = await sut.EntityGroupReadSorted("microtingUIdT", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameRetracted = await sut.EntityGroupReadSorted("microtingUIdT", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameRetracted = await sut.EntityGroupReadSorted("microtingUIdT", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion


            //#endregion

            //#region EntitySelect

            //#region Created
            //var entityGroupSortByIdSCreated = await sut.EntityGroupReadSorted("microtingUIdSC", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameSCreated = await sut.EntityGroupReadSorted("microtingUIdSC", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameSCreated = await sut.EntityGroupReadSorted("microtingUIdSC", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Removed
            //var entityGroupSortByIdSRemoved = await sut.EntityGroupReadSorted("microtingUIdSR", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameSRemoved = await sut.EntityGroupReadSorted("microtingUIdSR", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameSRemoved = await sut.EntityGroupReadSorted("microtingUIdSR", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Retracted
            //var entityGroupSortByIdSRetracted = await sut.EntityGroupReadSorted("microtingUIdST", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameSRetracted = await sut.EntityGroupReadSorted("microtingUIdST", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameSRetracted = await sut.EntityGroupReadSorted("microtingUIdST", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion


            //#endregion

            //// Assert
            //#region Created
            // Assert.NotNull(entityGroupSortByIdCreated);
            // Assert.NotNull(entityGroupSortByIdNameCreated);
            // Assert.NotNull(entityGroupSortByNameCreated);
            // Assert.AreEqual(10, entityGroupSortByIdCreated.EntityGroupMUId.Count());


            //#endregion
        }
#pragma warning restore 1998

        //TODO René needs to make migration
        [Test]
#pragma warning disable 1998
        public async Task SQL_EntityGroup_EntityGroupRead_ReadsEntityGroup()
        {
            //// Arrance
            //#region Entity Groups

            //#region EntitySearch
            //#region Created
            //#region eG1
            //entity_groups eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2 = await testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3 = await testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4 = await testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5 = await testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6 = await testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7 = await testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8 = await testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9 = await testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10 = await testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed

            //#region eG1
            //entity_groups eG1Removed = await testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2Removed = await testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3Removed = await testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4Removed = await testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5Removed = await testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6Removed = await testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7Removed = await testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8Removed = await testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9Removed = await testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10Removed = await testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted

            //#region eG1
            //entity_groups eG1Retracted = await testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2Retracted = await testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3Retracted = await testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4Retracted = await testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5Retracted = await testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6Retracted = await testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7Retracted = await testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8Retracted = await testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9Retracted = await testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10Retracted = await testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion

            //#region EntitySelect
            //#region Created
            //#region eG1
            //entity_groups eG1Select = await testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2Select = await testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3Select = await testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4Select = await testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5Select = await testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6Select = await testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7Select = await testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8Select = await testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9Select = await testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10Select = await testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed

            //#region eG1
            //entity_groups eG1SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRemoved = await testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted

            //#region eG1
            //entity_groups eG1SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRetracted = await testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion
            //#endregion
            //// Act
            //#region search
            //var readSearchCreated = await sut.EntityGroupRead("microtingUIdC");
            //var readSearchRemoved = await sut.EntityGroupRead("microtingUIdR");
            //var readSearchRetracted = await sut.EntityGroupRead("microtingUIdT");
            //#endregion

            //#region select
            //var readSelectCreated = await sut.EntityGroupRead("microtingUIdSC");
            //var readSelectRemoved = await sut.EntityGroupRead("microtingUIdSR");
            //var readSelectRetracted = await sut.EntityGroupRead("microtingUIdST");
            //#endregion
            //// Assert
        }
#pragma warning restore 1998

        [Test]
        public async Task SQL_EntityGroup_EntityGroupUpdate_UpdatesEntityGroup()
        {
            // Arrance

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

            // Act

            var updateEG1 = await sut.EntityGroupUpdate(eG1.Id, eG1.MicrotingUid);
            var updateEG1removed = await sut.EntityGroupUpdate(eG1Removed.Id, eG1Removed.MicrotingUid);
            var updateEG1retracted = await sut.EntityGroupUpdate(eG1Retracted.Id, eG1Retracted.MicrotingUid);


            // Assert
            Assert.True(updateEG1);
            Assert.True(updateEG1removed);
            Assert.True(updateEG1retracted);
        }


        [Test]
        public async Task SQL_EntityGroup_EntityGroupUpdateName_UpdatesEnityGroupName()
        {
            // Arrance

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

            // Act
            var EG1nameUpdate = await sut.EntityGroupUpdateName(eG1.Name, eG1.MicrotingUid);
            var EG2nameUpdate = await sut.EntityGroupUpdateName(eG2.Name, eG2.MicrotingUid);
            var EG3nameUpdate = await sut.EntityGroupUpdateName(eG3.Name, eG3.MicrotingUid);

            // Assert
            Assert.True(EG1nameUpdate);
            Assert.True(EG2nameUpdate);
            Assert.True(EG3nameUpdate);
        }

        //TODO René needs to make migration
        [Test]
#pragma warning disable 1998
        public async Task SQL_EntityGroup_EntityGroupUpdateItems_UpdatesEntityGroupItems()
        {
            // Arrance

            // Act

            // Assert
        }
#pragma warning restore 1998

        [Test]
        public async Task SQL_EntityGroup_EntityGroupDelete_DeletesEntityGroup()
        {
            // Arrance

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

            // Act
            var EG1Delete = await sut.EntityGroupDelete(eG1.MicrotingUid);
            var EG2Delete = await sut.EntityGroupDelete(eG2.MicrotingUid);
            var EG3Delete = await sut.EntityGroupDelete(eG3.MicrotingUid);
            // Assert
            Assert.AreEqual(EG1Delete, Constants.FieldTypes.EntitySearch);
            Assert.AreEqual(EG2Delete, Constants.FieldTypes.EntitySearch);
            Assert.AreEqual(EG3Delete, Constants.FieldTypes.EntitySearch);
        }

        [Test]
        public async Task SQL_EntitySearchItemCreate_CreatesEntitySearchItem()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            EntityItem eT = new EntityItem
            {
                Name = "Jon Doe",
                Description = "",
                EntityItemUId = "",
                WorkflowState = Constants.WorkflowStates.Created
            };

            // Act
            await sut.EntityItemCreate(eG1.Id, eT);

            List<Microting.eForm.Infrastructure.Data.Entities.EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
        }

        [Test]
        public async Task SQL_EntitySelectItemCreate_CreatesEntitySelectItem()
        {
            // Arrance
            EntityGroup eG1 = await testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1",
                Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            EntityItem eT = new EntityItem
            {
                Name = "Jon Doe",
                Description = "",
                EntityItemUId = "",
                WorkflowState = Constants.WorkflowStates.Created
            };

            // Act
            await sut.EntityItemCreate(eG1.Id, eT);

            List<Microting.eForm.Infrastructure.Data.Entities.EntityItem> items = DbContext.EntityItems.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
        }

        #endregion

        #region Entity Item

        #endregion

        #endregion

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