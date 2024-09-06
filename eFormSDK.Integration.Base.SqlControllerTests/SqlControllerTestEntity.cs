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

            Assert.That(matchEntityGroupAllSearchCreated, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreated.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreated.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreated

            Assert.That(matchEntityGroupAllSelectCreated, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreated.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreated.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemoved

            Assert.That(matchEntityGroupAllSearchRemoved, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemoved

            Assert.That(matchEntityGroupAllSelectRemoved, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemoved

            Assert.That(matchEntityGroupAllSearchNotRemoved, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectNotRemoved

            Assert.That(matchEntityGroupAllSelectNotRemoved, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.EntityGroups.Count));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDesc

            Assert.That(matchEntityGroupAllSearchCreatedWDesc, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDesc

            Assert.That(matchEntityGroupAllSelectCreatedWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDesc

            Assert.That(matchEntityGroupAllSearchRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDesc

            Assert.That(matchEntityGroupAllSelectRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDesc

            Assert.That(matchEntityGroupAllSearchNotRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDesc

            Assert.That(matchEntityGroupAllSelectNotRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.PageNum));

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoNameFilter

            Assert.That(matchEntityGroupAllSearchCreatedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoNameFilter

            Assert.That(matchEntityGroupAllSelectCreatedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSearchRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSelectRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSearchNotRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSelectNotRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchCreatedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectCreatedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoSort

            Assert.That(matchEntityGroupAllSearchCreatedNoSort, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoSort

            Assert.That(matchEntityGroupAllSelectCreatedNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoSort

            Assert.That(matchEntityGroupAllSearchRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoSort

            Assert.That(matchEntityGroupAllSelectRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoSort

            Assert.That(matchEntityGroupAllSearchNotRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoSort

            Assert.That(matchEntityGroupAllSelectNotRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoSort

            Assert.That(matchEntityGroupAllSearchCreatedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.NumOfElements));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoSort

            Assert.That(matchEntityGroupAllSelectCreatedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSearchRemovedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSelectRemovedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSearchNotRemovedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.PageNum));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSelectNotRemovedWDescNoSort, Is.Not.EqualTo(null));
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

            Assert.That(matchEntityGroupAllSearchCreated, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreated.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreated.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreated

            Assert.That(matchEntityGroupAllSelectCreated, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreated.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreated.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreated.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemoved

            Assert.That(matchEntityGroupAllSearchRemoved, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemoved.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemoved

            Assert.That(matchEntityGroupAllSelectRemoved, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemoved.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemoved.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemoved

            Assert.That(matchEntityGroupAllSearchNotRemoved, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemoved.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectNotRemoved

            Assert.That(matchEntityGroupAllSelectNotRemoved, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemoved.EntityGroups.Count));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDesc

            Assert.That(matchEntityGroupAllSearchCreatedWDesc, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDesc

            Assert.That(matchEntityGroupAllSelectCreatedWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDesc

            Assert.That(matchEntityGroupAllSearchRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDesc

            Assert.That(matchEntityGroupAllSelectRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDesc

            Assert.That(matchEntityGroupAllSearchNotRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDesc

            Assert.That(matchEntityGroupAllSelectNotRemovedWDesc, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDesc.PageNum));

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoNameFilter

            Assert.That(matchEntityGroupAllSearchCreatedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoNameFilter

            Assert.That(matchEntityGroupAllSelectCreatedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSearchRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSelectRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSearchNotRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoNameFilter

            Assert.That(matchEntityGroupAllSelectNotRemovedNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchCreatedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectCreatedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedNoSort

            Assert.That(matchEntityGroupAllSearchCreatedNoSort, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedNoSort

            Assert.That(matchEntityGroupAllSelectCreatedNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedNoSort

            Assert.That(matchEntityGroupAllSearchRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedNoSort

            Assert.That(matchEntityGroupAllSelectRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedNoSort

            Assert.That(matchEntityGroupAllSearchNotRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedNoSort

            Assert.That(matchEntityGroupAllSelectNotRemovedNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedWDescNoSort

            Assert.That(matchEntityGroupAllSearchCreatedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedWDescNoSort

            Assert.That(matchEntityGroupAllSelectCreatedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSearchRemovedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSelectRemovedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSearchNotRemovedWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.NumOfElements));
            Assert.That(0, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedWDescNoSort

            Assert.That(matchEntityGroupAllSelectNotRemovedWDescNoSort, Is.Not.EqualTo(null));
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

            Assert.That(matchEntityGroupAllSearchCreatedB, Is.Not.EqualTo(null));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedB.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedB.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedB

            Assert.That(matchEntityGroupAllSelectCreatedB, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedB.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedB

            Assert.That(matchEntityGroupAllSearchRemovedB, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedB.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedB

            Assert.That(matchEntityGroupAllSelectRemovedB, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedB.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedB.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedB

            Assert.That(matchEntityGroupAllSearchNotRemovedB, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedB.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedB.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedB

            Assert.That(matchEntityGroupAllSelectNotRemovedB, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedB.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedB.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedB.EntityGroups.Count));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDesc

            Assert.That(matchEntityGroupAllSearchCreatedBWDesc, Is.Not.EqualTo(null));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDesc.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDesc.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDesc

            Assert.That(matchEntityGroupAllSelectCreatedBWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDesc.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDesc.EntityGroups.Count));

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDesc

            Assert.That(matchEntityGroupAllSearchRemovedBWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDesc

            Assert.That(matchEntityGroupAllSelectRemovedBWDesc, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDesc

            Assert.That(matchEntityGroupAllSearchNotRemovedBWDesc, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDesc.PageNum));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDesc

            Assert.That(matchEntityGroupAllSelectNotRemovedBWDesc, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDesc.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDesc.PageNum));

            #endregion

            #endregion

            #endregion

            #region No Name Filter

            #region Asc

            #region matchEntityGroupAllSearchCreatedBNoNameFilter

            Assert.That(matchEntityGroupAllSearchCreatedBNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBNoNameFilter

            Assert.That(matchEntityGroupAllSelectCreatedBNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBNoNameFilter

            Assert.That(matchEntityGroupAllSearchRemovedBNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBNoNameFilter

            Assert.That(matchEntityGroupAllSelectRemovedBNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBNoNameFilter

            Assert.That(matchEntityGroupAllSearchNotRemovedBNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBNoNameFilter

            Assert.That(matchEntityGroupAllSelectNotRemovedBNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchCreatedBWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectCreatedBWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchRemovedBWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectRemovedBWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter

            Assert.That(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter

            Assert.That(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter.EntityGroups.Count()));

            #endregion

            #endregion

            #endregion

            #region No Sort Param

            #region Asc

            #region matchEntityGroupAllSearchCreatedBNoSort

            Assert.That(matchEntityGroupAllSearchCreatedBNoSort, Is.Not.EqualTo(null));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBNoSort

            Assert.That(matchEntityGroupAllSelectCreatedBNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBNoSort

            Assert.That(matchEntityGroupAllSearchRemovedBNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBNoSort

            Assert.That(matchEntityGroupAllSelectRemovedBNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBNoSort

            Assert.That(matchEntityGroupAllSearchNotRemovedBNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBNoSort

            Assert.That(matchEntityGroupAllSelectNotRemovedBNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectNotRemovedBNoSort.EntityGroups.Count()));

            #endregion

            #endregion

            #region Desc

            #region matchEntityGroupAllSearchCreatedBWDescNoSort

            Assert.That(matchEntityGroupAllSearchCreatedBWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoSort.NumOfElements));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchCreatedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectCreatedBWDescNoSort

            Assert.That(matchEntityGroupAllSelectCreatedBWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectCreatedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchRemovedBWDescNoSort

            Assert.That(matchEntityGroupAllSearchRemovedBWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSearchRemovedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectRemovedBWDescNoSort

            Assert.That(matchEntityGroupAllSelectRemovedBWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoSort.PageNum));
            Assert.That(5, Is.EqualTo(matchEntityGroupAllSelectRemovedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSearchNotRemovedBWDescNoSort

            Assert.That(matchEntityGroupAllSearchNotRemovedBWDescNoSort, Is.Not.EqualTo(null));
            Assert.That(20, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoSort.NumOfElements));
            Assert.That(1, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoSort.PageNum));
            Assert.That(10, Is.EqualTo(matchEntityGroupAllSearchNotRemovedBWDescNoSort.EntityGroups.Count()));

            #endregion

            #region matchEntityGroupAllSelectNotRemovedBWDescNoSort

            Assert.That(matchEntityGroupAllSelectNotRemovedBWDescNoSort, Is.Not.EqualTo(null));
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
        public async Task SQL_EntityGroup_EntityGroupCreate_CreatesNewEntityGroup()
        {
            // Arrance

            // Act
            var matchEntitySearch = await sut.EntityGroupCreate("eG1", Constants.FieldTypes.EntitySearch, "desc1");
            var matchEntitySelect = await sut.EntityGroupCreate("eG2", Constants.FieldTypes.EntitySelect, "desc2");
            // Assert
            Assert.That(matchEntitySearch.Name, Is.EqualTo("eG1"));
            Assert.That(matchEntitySelect.Name, Is.EqualTo("eG2"));
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
            // Assert.That(entityGroupSortByIdCreated, Is.Not.EqualTo(null));
            // Assert.That(entityGroupSortByIdNameCreated, Is.Not.EqualTo(null));
            // Assert.That(entityGroupSortByNameCreated, Is.Not.EqualTo(null));
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
            Assert.That(updateEG1, Is.True);
            Assert.That(updateEG1removed, Is.True);
            Assert.That(updateEG1retracted, Is.True);
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
            Assert.That(EG1nameUpdate, Is.True);
            Assert.That(EG2nameUpdate, Is.True);
            Assert.That(EG3nameUpdate, Is.True);
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
            Assert.That(Constants.FieldTypes.EntitySearch, Is.EqualTo(EG1Delete));
            Assert.That(Constants.FieldTypes.EntitySearch, Is.EqualTo(EG2Delete));
            Assert.That(Constants.FieldTypes.EntitySearch, Is.EqualTo(EG3Delete));
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
            Assert.That(items.Count(), Is.EqualTo(1));
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
            Assert.That(items.Count(), Is.EqualTo(1));
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