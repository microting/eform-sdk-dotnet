using eFormCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestEntity : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        #region Entity
        #region Entity Group

        [Test]
        public void SQL_EntityGroup_EntityGroupAll_ReturnsEntityGroups()
        {

            // Arrance
            #region Arrance

            #region Entity Groups

            #region EntitySearch
            #region Created
            #region eG1
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2 = testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3 = testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4 = testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5 = testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6 = testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7 = testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8 = testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9 = testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10 = testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1Removed = testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2Removed = testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3Removed = testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4Removed = testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5Removed = testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6Removed = testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7Removed = testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8Removed = testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9Removed = testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10Removed = testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1Retracted = testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2Retracted = testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3Retracted = testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4Retracted = testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5Retracted = testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6Retracted = testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7Retracted = testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8Retracted = testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9Retracted = testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10Retracted = testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion

            #region EntitySelect
            #region Created
            #region eG1
            entity_groups eG1Select = testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2Select = testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3Select = testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4Select = testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5Select = testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6Select = testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7Select = testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8Select = testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9Select = testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10Select = testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
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
            EntityGroupList matchEntityGroupAllSearchCreated = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreated = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc
            EntityGroupList matchEntityGroupAllSearchCreatedNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 20, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 20, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc
            EntityGroupList matchEntityGroupAllSearchCreatedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 20, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
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
        public void SQL_EntityGroup_EntityGroupAll_ReturnsEntityGroupsPaged()
        {

            // Arrance
            #region Arrance

            #region Entity Groups

            #region EntitySearch
            #region Created
            #region eG1
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2 = testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3 = testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4 = testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5 = testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6 = testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7 = testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8 = testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9 = testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10 = testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1Removed = testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2Removed = testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3Removed = testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4Removed = testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5Removed = testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6Removed = testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7Removed = testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8Removed = testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9Removed = testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10Removed = testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1Retracted = testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2Retracted = testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3Retracted = testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4Retracted = testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5Retracted = testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6Retracted = testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7Retracted = testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8Retracted = testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9Retracted = testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10Retracted = testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion

            #region EntitySelect
            #region Created
            #region eG1
            entity_groups eG1Select = testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2Select = testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3Select = testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4Select = testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5Select = testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6Select = testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7Select = testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8Select = testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9Select = testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10Select = testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
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
            EntityGroupList matchEntityGroupAllSearchCreated = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreated = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemoved = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDesc = sut.EntityGroupAll("id", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc
            EntityGroupList matchEntityGroupAllSearchCreatedNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoNameFilter = sut.EntityGroupAll("id", "", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc
            EntityGroupList matchEntityGroupAllSearchCreatedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 0, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
            #endregion


            #endregion

            #endregion

            #endregion

            #region pageindex 1

            #region pageSize 5

            #region Default Sorting
            #region entityGroup Asc
            EntityGroupList matchEntityGroupAllSearchCreatedB = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedB = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedB = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedB = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedB = sut.EntityGroupAll("id", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedB = sut.EntityGroupAll("id", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedBWDesc = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDesc = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDesc = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDesc = sut.EntityGroupAll("id", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDesc = sut.EntityGroupAll("id", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDesc = sut.EntityGroupAll("id", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
            #endregion

            #endregion

            #region Sorting W.O nameFilter

            #region entityGroup Asc
            EntityGroupList matchEntityGroupAllSearchCreatedBNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBNoNameFilter = sut.EntityGroupAll("id", "", 1, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBNoNameFilter = sut.EntityGroupAll("id", "", 1, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedBWDescNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDescNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDescNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDescNoNameFilter = sut.EntityGroupAll("id", "", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDescNoNameFilter = sut.EntityGroupAll("id", "", 1, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDescNoNameFilter = sut.EntityGroupAll("id", "", 1, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
            #endregion

            #endregion

            #region sorting W.O sort param

            #region entityGroup Asc
            EntityGroupList matchEntityGroupAllSearchCreatedBNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, false, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, false, Constants.WorkflowStates.NotRemoved);
            #endregion

            #region entityGroup Desc
            EntityGroupList matchEntityGroupAllSearchCreatedBWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Created);
            EntityGroupList matchEntityGroupAllSelectCreatedBWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Created);

            EntityGroupList matchEntityGroupAllSearchRemovedBWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.Removed);
            EntityGroupList matchEntityGroupAllSelectRemovedBWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 5, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.Removed);

            EntityGroupList matchEntityGroupAllSearchNotRemovedBWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySearch, true, Constants.WorkflowStates.NotRemoved);
            EntityGroupList matchEntityGroupAllSelectNotRemovedBWDescNoSort = sut.EntityGroupAll("", "EntityGroup", 1, 10, Constants.FieldTypes.EntitySelect, true, Constants.WorkflowStates.NotRemoved);
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
        public void SQL_EntityGroup_EntityGroupCreate_CreatesNewEntityGroup()
        {

            // Arrance

            // Act
            var matchEntitySearch = sut.EntityGroupCreate("eG1", Constants.FieldTypes.EntitySearch);
            var matchEntitySelect = sut.EntityGroupCreate("eG2", Constants.FieldTypes.EntitySelect);
            // Assert
            Assert.AreEqual("eG1", matchEntitySearch.Name);
            Assert.AreEqual("eG2", matchEntitySelect.Name);



        }

        //TODO René needs to make migration
        [Test]
        public void SQL_EntityGroup_EntityGroupReadSorted_ReadsByParameter()
        {

            //// Arrance
            //#region Arrance

            //#region Entity Groups

            //#region EntitySearch
            //#region Created
            //#region eG1
            //entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2 = testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3 = testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4 = testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5 = testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6 = testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7 = testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8 = testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9 = testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10 = testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed 

            //#region eG1
            //entity_groups eG1Removed = testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2Removed = testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3Removed = testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4Removed = testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5Removed = testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6Removed = testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7Removed = testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8Removed = testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9Removed = testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10Removed = testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted 

            //#region eG1
            //entity_groups eG1Retracted = testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2Retracted = testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3Retracted = testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4Retracted = testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5Retracted = testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6Retracted = testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7Retracted = testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8Retracted = testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9Retracted = testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10Retracted = testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion

            //#region EntitySelect
            //#region Created
            //#region eG1
            //entity_groups eG1Select = testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2Select = testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3Select = testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4Select = testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5Select = testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6Select = testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7Select = testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8Select = testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9Select = testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10Select = testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed 

            //#region eG1
            //entity_groups eG1SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted 

            //#region eG1
            //entity_groups eG1SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion
            //#endregion

            //#region EntityItems

            //#region Created

            //entity_items eI1 = testHelpers.CreateEntityItem("D1", 1, "", "eIUIdC1", "mUIdC1", "EntityItem1Created", 1, 1, Constants.WorkflowStates.Created);

            //#endregion

            //#endregion

            //#endregion
            //// Act
            //#region EntitySearch 

            //#region Created
            //var entityGroupSortByIdCreated = sut.EntityGroupReadSorted("microtingUIdC", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameCreated = sut.EntityGroupReadSorted("microtingUIdC", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameCreated = sut.EntityGroupReadSorted("microtingUIdC", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Removed
            //var entityGroupSortByIdRemoved = sut.EntityGroupReadSorted("microtingUIdR", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameRemoved = sut.EntityGroupReadSorted("microtingUIdR", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameRemoved = sut.EntityGroupReadSorted("microtingUIdR", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Retracted
            //var entityGroupSortByIdRetracted = sut.EntityGroupReadSorted("microtingUIdT", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameRetracted = sut.EntityGroupReadSorted("microtingUIdT", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameRetracted = sut.EntityGroupReadSorted("microtingUIdT", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion


            //#endregion

            //#region EntitySelect 

            //#region Created
            //var entityGroupSortByIdSCreated = sut.EntityGroupReadSorted("microtingUIdSC", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameSCreated = sut.EntityGroupReadSorted("microtingUIdSC", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameSCreated = sut.EntityGroupReadSorted("microtingUIdSC", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Removed
            //var entityGroupSortByIdSRemoved = sut.EntityGroupReadSorted("microtingUIdSR", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameSRemoved = sut.EntityGroupReadSorted("microtingUIdSR", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameSRemoved = sut.EntityGroupReadSorted("microtingUIdSR", Constants.EntityItemSortParameters.Name, "eG");
            //#endregion

            //#region Retracted
            //var entityGroupSortByIdSRetracted = sut.EntityGroupReadSorted("microtingUIdST", Constants.EntityItemSortParameters.Id, "");
            //var entityGroupSortByIdNameSRetracted = sut.EntityGroupReadSorted("microtingUIdST", Constants.EntityItemSortParameters.Id, "eG");
            //var entityGroupSortByNameSRetracted = sut.EntityGroupReadSorted("microtingUIdST", Constants.EntityItemSortParameters.Name, "eG");
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

        //TODO René needs to make migration
        [Test]
        public void SQL_EntityGroup_EntityGroupRead_ReadsEntityGroup()
        {

            //// Arrance
            //#region Entity Groups

            //#region EntitySearch
            //#region Created
            //#region eG1
            //entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2 = testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3 = testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4 = testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5 = testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6 = testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7 = testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8 = testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9 = testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10 = testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed 

            //#region eG1
            //entity_groups eG1Removed = testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2Removed = testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3Removed = testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4Removed = testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5Removed = testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6Removed = testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7Removed = testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8Removed = testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9Removed = testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10Removed = testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted 

            //#region eG1
            //entity_groups eG1Retracted = testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2Retracted = testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3Retracted = testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4Retracted = testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5Retracted = testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6Retracted = testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7Retracted = testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8Retracted = testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9Retracted = testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10Retracted = testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion

            //#region EntitySelect
            //#region Created
            //#region eG1
            //entity_groups eG1Select = testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG2
            //entity_groups eG2Select = testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG3
            //entity_groups eG3Select = testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG4
            //entity_groups eG4Select = testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG5
            //entity_groups eG5Select = testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG6
            //entity_groups eG6Select = testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG7
            //entity_groups eG7Select = testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG8
            //entity_groups eG8Select = testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG9
            //entity_groups eG9Select = testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#region eG10
            //entity_groups eG10Select = testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            //#endregion

            //#endregion

            //#region Removed 

            //#region eG1
            //entity_groups eG1SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            //#endregion

            //#endregion

            //#region Retracted 

            //#region eG1
            //entity_groups eG1SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG2
            //entity_groups eG2SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG3
            //entity_groups eG3SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG4
            //entity_groups eG4SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG5
            //entity_groups eG5SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG6
            //entity_groups eG6SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG7
            //entity_groups eG7SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG8
            //entity_groups eG8SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG9
            //entity_groups eG9SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#region eG10
            //entity_groups eG10SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            //#endregion

            //#endregion

            //#endregion
            //#endregion
            //// Act
            //#region search
            //var readSearchCreated = sut.EntityGroupRead("microtingUIdC");
            //var readSearchRemoved = sut.EntityGroupRead("microtingUIdR");
            //var readSearchRetracted = sut.EntityGroupRead("microtingUIdT");
            //#endregion

            //#region select
            //var readSelectCreated = sut.EntityGroupRead("microtingUIdSC");
            //var readSelectRemoved = sut.EntityGroupRead("microtingUIdSR");
            //var readSelectRetracted = sut.EntityGroupRead("microtingUIdST");
            //#endregion
            //// Assert




        }


        [Test]
        public void SQL_EntityGroup_EntityGroupUpdate_UpdatesEntityGroup()
        {

            // Arrance
            #region Entity Groups

            #region EntitySearch
            #region Created
            #region eG1
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2 = testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3 = testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4 = testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5 = testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6 = testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7 = testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8 = testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9 = testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10 = testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1Removed = testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2Removed = testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3Removed = testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4Removed = testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5Removed = testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6Removed = testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7Removed = testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8Removed = testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9Removed = testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10Removed = testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1Retracted = testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2Retracted = testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3Retracted = testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4Retracted = testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5Retracted = testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6Retracted = testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7Retracted = testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8Retracted = testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9Retracted = testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10Retracted = testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion

            #region EntitySelect
            #region Created
            #region eG1
            entity_groups eG1Select = testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2Select = testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3Select = testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4Select = testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5Select = testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6Select = testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7Select = testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8Select = testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9Select = testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10Select = testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion
            #endregion
            // Act

            var updateEG1 = sut.EntityGroupUpdate(eG1.Id, eG1.MicrotingUid);
            var updateEG1removed = sut.EntityGroupUpdate(eG1Removed.Id, eG1Removed.MicrotingUid);
            var updateEG1retracted = sut.EntityGroupUpdate(eG1Retracted.Id, eG1Retracted.MicrotingUid);




            // Assert
            Assert.True(updateEG1);
            Assert.True(updateEG1removed);
            Assert.True(updateEG1retracted);

        }


        [Test]
        public void SQL_EntityGroup_EntityGroupUpdateName_UpdatesEnityGroupName()
        {

            // Arrance
            #region Entity Groups

            #region EntitySearch
            #region Created
            #region eG1
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2 = testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3 = testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4 = testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5 = testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6 = testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7 = testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8 = testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9 = testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10 = testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1Removed = testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2Removed = testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3Removed = testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4Removed = testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5Removed = testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6Removed = testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7Removed = testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8Removed = testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9Removed = testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10Removed = testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1Retracted = testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2Retracted = testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3Retracted = testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4Retracted = testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5Retracted = testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6Retracted = testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7Retracted = testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8Retracted = testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9Retracted = testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10Retracted = testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion

            #region EntitySelect
            #region Created
            #region eG1
            entity_groups eG1Select = testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2Select = testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3Select = testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4Select = testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5Select = testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6Select = testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7Select = testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8Select = testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9Select = testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10Select = testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion
            #endregion
            // Act
            var EG1nameUpdate = sut.EntityGroupUpdateName(eG1.Name, eG1.MicrotingUid);
            var EG2nameUpdate = sut.EntityGroupUpdateName(eG2.Name, eG2.MicrotingUid);
            var EG3nameUpdate = sut.EntityGroupUpdateName(eG3.Name, eG3.MicrotingUid);

            // Assert
            Assert.True(EG1nameUpdate);
            Assert.True(EG2nameUpdate);
            Assert.True(EG3nameUpdate);
        }

        //TODO René needs to make migration
        [Test]
        public void SQL_EntityGroup_EntityGroupUpdateItems_UpdatesEntotyGroupItems()
        {

            // Arrance

            // Act

            // Assert

        }


        [Test]
        public void SQL_EntityGroup_EntityGroupDelete_DeletesEntityGroup()
        {

            // Arrance
            #region Entity Groups

            #region EntitySearch
            #region Created
            #region eG1
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2 = testHelpers.CreateEntityGroup("microtingUIdC2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3 = testHelpers.CreateEntityGroup("microtingUIdC3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4 = testHelpers.CreateEntityGroup("microtingUIdC4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5 = testHelpers.CreateEntityGroup("microtingUIdC5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6 = testHelpers.CreateEntityGroup("microtingUIdC6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7 = testHelpers.CreateEntityGroup("microtingUIdC7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8 = testHelpers.CreateEntityGroup("microtingUIdC8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9 = testHelpers.CreateEntityGroup("microtingUIdC9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10 = testHelpers.CreateEntityGroup("microtingUIdC10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1Removed = testHelpers.CreateEntityGroup("microtingUIdR1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2Removed = testHelpers.CreateEntityGroup("microtingUIdR2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3Removed = testHelpers.CreateEntityGroup("microtingUIdR3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4Removed = testHelpers.CreateEntityGroup("microtingUIdR4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5Removed = testHelpers.CreateEntityGroup("microtingUIdR5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6Removed = testHelpers.CreateEntityGroup("microtingUIdR6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7Removed = testHelpers.CreateEntityGroup("microtingUIdR7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8Removed = testHelpers.CreateEntityGroup("microtingUIdR8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9Removed = testHelpers.CreateEntityGroup("microtingUIdR9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10Removed = testHelpers.CreateEntityGroup("microtingUIdR10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1Retracted = testHelpers.CreateEntityGroup("microtingUIdT1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2Retracted = testHelpers.CreateEntityGroup("microtingUIdT2", "EntityGroup2", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3Retracted = testHelpers.CreateEntityGroup("microtingUIdT3", "EntityGroup3", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4Retracted = testHelpers.CreateEntityGroup("microtingUIdT4", "EntityGroup4", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5Retracted = testHelpers.CreateEntityGroup("microtingUIdT5", "EntityGroup5", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6Retracted = testHelpers.CreateEntityGroup("microtingUIdT6", "EntityGroup6", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7Retracted = testHelpers.CreateEntityGroup("microtingUIdT7", "EntityGroup7", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8Retracted = testHelpers.CreateEntityGroup("microtingUIdT8", "EntityGroup8", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9Retracted = testHelpers.CreateEntityGroup("microtingUIdT9", "EntityGroup9", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10Retracted = testHelpers.CreateEntityGroup("microtingUIdT10", "EntityGroup10", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion

            #region EntitySelect
            #region Created
            #region eG1
            entity_groups eG1Select = testHelpers.CreateEntityGroup("microtingUIdSC1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG2
            entity_groups eG2Select = testHelpers.CreateEntityGroup("microtingUIdSC2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG3
            entity_groups eG3Select = testHelpers.CreateEntityGroup("microtingUIdSC3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG4
            entity_groups eG4Select = testHelpers.CreateEntityGroup("microtingUIdSC4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG5
            entity_groups eG5Select = testHelpers.CreateEntityGroup("microtingUIdSC5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG6
            entity_groups eG6Select = testHelpers.CreateEntityGroup("microtingUIdSC6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG7
            entity_groups eG7Select = testHelpers.CreateEntityGroup("microtingUIdSC7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG8
            entity_groups eG8Select = testHelpers.CreateEntityGroup("microtingUIdSC8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG9
            entity_groups eG9Select = testHelpers.CreateEntityGroup("microtingUIdSC9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #region eG10
            entity_groups eG10Select = testHelpers.CreateEntityGroup("microtingUIdSC10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            #endregion

            #endregion

            #region Removed 

            #region eG1
            entity_groups eG1SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG2
            entity_groups eG2SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG3
            entity_groups eG3SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG4
            entity_groups eG4SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG5
            entity_groups eG5SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG6
            entity_groups eG6SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG7
            entity_groups eG7SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG8
            entity_groups eG8SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG9
            entity_groups eG9SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #region eG10
            entity_groups eG10SelectRemoved = testHelpers.CreateEntityGroup("microtingUIdSR10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region Retracted 

            #region eG1
            entity_groups eG1SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST1", "EntityGroup1Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG2
            entity_groups eG2SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST2", "EntityGroup2Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG3
            entity_groups eG3SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST3", "EntityGroup3Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG4
            entity_groups eG4SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST4", "EntityGroup4Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG5
            entity_groups eG5SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST5", "EntityGroup5Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG6
            entity_groups eG6SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST6", "EntityGroup6Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG7
            entity_groups eG7SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST7", "EntityGroup7Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG8
            entity_groups eG8SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST8", "EntityGroup8Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG9
            entity_groups eG9SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST9", "EntityGroup9Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #region eG10
            entity_groups eG10SelectRetracted = testHelpers.CreateEntityGroup("microtingUIdST10", "EntityGroup10Select", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Retracted);
            #endregion

            #endregion

            #endregion
            #endregion
            // Act
            var EG1Delete = sut.EntityGroupDelete(eG1.MicrotingUid);
            var EG2Delete = sut.EntityGroupDelete(eG2.MicrotingUid);
            var EG3Delete = sut.EntityGroupDelete(eG3.MicrotingUid);
            // Assert
            Assert.AreEqual(EG1Delete, Constants.FieldTypes.EntitySearch);
            Assert.AreEqual(EG2Delete, Constants.FieldTypes.EntitySearch);
            Assert.AreEqual(EG3Delete, Constants.FieldTypes.EntitySearch);
        }

        [Test]
        public void SQL_EntitySearchItemCreate_CreatesEntitySearchItem()
        {
            // Arrance
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            EntityItem eT = new EntityItem
            {
                Name = "Jon Doe",
                Description = "",
                EntityItemUId = "",
                WorkflowState = Constants.WorkflowStates.Created
            };

            // Act
            sut.EntityItemCreate(eG1.Id, eT);

            List<entity_items> items = DbContext.entity_items.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
        }

        [Test]
        public void SQL_EntitySearchItemDelete_DeletesEntitySearchItem()
        {
            // Arrance
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySearch, Constants.WorkflowStates.Created);
            entity_items et = testHelpers.CreateEntityItem("", 0, eG1.Id, "", "", "Jon Doe", 1, 0, Constants.WorkflowStates.Created);

            // Act
            sut.EntityItemDelete(et.Id);
            List<entity_items> items = DbContext.entity_items.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, items[0].WorkflowState);
        }


        [Test]
        public void SQL_EntitySelectItemCreate_CreatesEntitySelectItem()
        {
            // Arrance
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            EntityItem eT = new EntityItem
            {
                Name = "Jon Doe",
                Description = "",
                EntityItemUId = "",
                WorkflowState = Constants.WorkflowStates.Created
            };

            // Act
            sut.EntityItemCreate(eG1.Id, eT);

            List<entity_items> items = DbContext.entity_items.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
        }

        [Test]
        public void SQL_EntitySelectItemDelete_DeletesEntitySelectItem()
        {
            // Arrance
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            entity_items et = testHelpers.CreateEntityItem("", 0, eG1.Id, "", "", "Jon Doe", 1, 0, Constants.WorkflowStates.Created);

            // Act
            sut.EntityItemDelete(et.Id);
            List<entity_items> items = DbContext.entity_items.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, items[0].WorkflowState);
        }

        #endregion

        #region Entity Item

        #endregion

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