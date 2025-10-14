-- MariaDB dump 10.19  Distrib 10.6.16-MariaDB, for debian-linux-gnu (x86_64)
--
-- Host: 127.0.0.1    Database: 420_SDK
-- ------------------------------------------------------
-- Server version	10.8.8-MariaDB-1:10.8.8+maria~ubu2204

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `AnswerValueVersions`
--

DROP TABLE IF EXISTS `AnswerValueVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `AnswerValueVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `AnswerId` int(11) NOT NULL DEFAULT 0,
  `QuestionId` int(11) NOT NULL DEFAULT 0,
  `OptionId` int(11) NOT NULL DEFAULT 0,
  `Value` longtext DEFAULT NULL,
  `AnswerValueId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_answer_value_versions_answerValueId` (`AnswerValueId`),
  CONSTRAINT `FK_answer_value_versions_answer_values_AnswerValueId` FOREIGN KEY (`AnswerValueId`) REFERENCES `AnswerValues` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `AnswerValues`
--

DROP TABLE IF EXISTS `AnswerValues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `AnswerValues` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `AnswerId` int(11) NOT NULL DEFAULT 0,
  `QuestionId` int(11) NOT NULL DEFAULT 0,
  `OptionId` int(11) NOT NULL DEFAULT 0,
  `Value` longtext DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_answer_values_answerId` (`AnswerId`),
  KEY `IX_answer_values_optionsId` (`OptionId`),
  KEY `IX_answer_values_questionId` (`QuestionId`),
  CONSTRAINT `FK_answer_values_answers_AnswerId` FOREIGN KEY (`AnswerId`) REFERENCES `Answers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_answer_values_options_OptionId` FOREIGN KEY (`OptionId`) REFERENCES `Options` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_answer_values_questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `AnswerVersions`
--

DROP TABLE IF EXISTS `AnswerVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `AnswerVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UnitId` int(11) DEFAULT NULL,
  `SiteId` int(11) NOT NULL DEFAULT 0,
  `AnswerDuration` int(11) NOT NULL DEFAULT 0,
  `LanguageId` int(11) NOT NULL DEFAULT 0,
  `SurveyConfigurationId` int(11) DEFAULT NULL,
  `FinishedAt` datetime(6) NOT NULL,
  `QuestionSetId` int(11) NOT NULL DEFAULT 0,
  `UtcAdjusted` tinyint(1) NOT NULL DEFAULT 0,
  `TimeZone` longtext DEFAULT NULL,
  `AnswerId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Answers`
--

DROP TABLE IF EXISTS `Answers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Answers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UnitId` int(11) DEFAULT NULL,
  `SiteId` int(11) NOT NULL DEFAULT 0,
  `AnswerDuration` int(11) NOT NULL DEFAULT 0,
  `LanguageId` int(11) NOT NULL DEFAULT 0,
  `SurveyConfigurationId` int(11) DEFAULT NULL,
  `FinishedAt` datetime(6) NOT NULL,
  `QuestionSetId` int(11) NOT NULL DEFAULT 0,
  `UtcAdjusted` tinyint(1) NOT NULL DEFAULT 0,
  `TimeZone` longtext DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_answers_languageId` (`LanguageId`),
  KEY `IX_answers_questionSetId` (`QuestionSetId`),
  KEY `IX_answers_siteId` (`SiteId`),
  KEY `IX_answers_surveyConfigurationId` (`SurveyConfigurationId`),
  KEY `IX_answers_unitId` (`UnitId`),
  CONSTRAINT `FK_answers_languages_LanguageId` FOREIGN KEY (`LanguageId`) REFERENCES `Languages` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_answers_question_sets_QuestionSetId` FOREIGN KEY (`QuestionSetId`) REFERENCES `QuestionSets` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_answers_sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_answers_survey_configurations_SurveyConfigurationId` FOREIGN KEY (`SurveyConfigurationId`) REFERENCES `SurveyConfigurations` (`Id`),
  CONSTRAINT `FK_answers_units_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `Units` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CaseVersions`
--

DROP TABLE IF EXISTS `CaseVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CaseVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CaseId` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `DoneAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `UnitId` int(11) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `Type` varchar(255) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `MicrotingCheckUid` int(11) DEFAULT NULL,
  `CaseUid` varchar(255) DEFAULT NULL,
  `Custom` longtext DEFAULT NULL,
  `FieldValue1` longtext DEFAULT NULL,
  `FieldValue2` longtext DEFAULT NULL,
  `FieldValue3` longtext DEFAULT NULL,
  `FieldValue4` longtext DEFAULT NULL,
  `FieldValue5` longtext DEFAULT NULL,
  `FieldValue6` longtext DEFAULT NULL,
  `FieldValue7` longtext DEFAULT NULL,
  `FieldValue8` longtext DEFAULT NULL,
  `FieldValue9` longtext DEFAULT NULL,
  `FieldValue10` longtext DEFAULT NULL,
  `FolderId` int(11) DEFAULT NULL,
  `IsArchived` tinyint(1) NOT NULL DEFAULT 0,
  `DoneAtUserModifiable` datetime(6) DEFAULT NULL,
  `ReceivedByServerAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Cases`
--

DROP TABLE IF EXISTS `Cases`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Cases` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `DoneAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `UnitId` int(11) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `Type` varchar(255) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `MicrotingCheckUid` int(11) DEFAULT NULL,
  `CaseUid` varchar(255) DEFAULT NULL,
  `Custom` longtext DEFAULT NULL,
  `FieldValue1` longtext DEFAULT NULL,
  `FieldValue2` longtext DEFAULT NULL,
  `FieldValue3` longtext DEFAULT NULL,
  `FieldValue4` longtext DEFAULT NULL,
  `FieldValue5` longtext DEFAULT NULL,
  `FieldValue6` longtext DEFAULT NULL,
  `FieldValue7` longtext DEFAULT NULL,
  `FieldValue8` longtext DEFAULT NULL,
  `FieldValue9` longtext DEFAULT NULL,
  `FieldValue10` longtext DEFAULT NULL,
  `FolderId` int(11) DEFAULT NULL,
  `IsArchived` tinyint(1) NOT NULL DEFAULT 0,
  `DoneAtUserModifiable` datetime(6) DEFAULT NULL,
  `ReceivedByServerAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_cases_check_list_id` (`CheckListId`),
  KEY `IX_cases_done_by_user_id` (`WorkerId`),
  KEY `IX_cases_site_id` (`SiteId`),
  KEY `IX_cases_unit_id` (`UnitId`),
  KEY `IX_cases_CheckListId` (`CheckListId`),
  KEY `IX_cases_MicrotingUid_MicrotingCheckUid` (`MicrotingUid`,`MicrotingCheckUid`),
  KEY `IX_cases_FolderId` (`FolderId`),
  CONSTRAINT `FK_cases_check_lists_CheckListId` FOREIGN KEY (`CheckListId`) REFERENCES `CheckLists` (`Id`),
  CONSTRAINT `FK_cases_folders_FolderId` FOREIGN KEY (`FolderId`) REFERENCES `Folders` (`Id`),
  CONSTRAINT `FK_cases_sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`),
  CONSTRAINT `FK_cases_units_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `Units` (`Id`),
  CONSTRAINT `FK_cases_workers_WorkerId` FOREIGN KEY (`WorkerId`) REFERENCES `Workers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckListSiteVersions`
--

DROP TABLE IF EXISTS `CheckListSiteVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckListSiteVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CheckListSiteId` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `MicrotingUid` int(11) NOT NULL,
  `LastCheckId` int(11) NOT NULL,
  `FolderId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckListSites`
--

DROP TABLE IF EXISTS `CheckListSites`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckListSites` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `MicrotingUid` int(11) NOT NULL,
  `LastCheckId` int(11) NOT NULL,
  `FolderId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_check_list_sites_check_list_id` (`CheckListId`),
  KEY `IX_check_list_sites_site_id` (`SiteId`),
  KEY `IX_check_list_sites_FolderId` (`FolderId`),
  CONSTRAINT `FK_check_list_sites_check_lists_CheckListId` FOREIGN KEY (`CheckListId`) REFERENCES `CheckLists` (`Id`),
  CONSTRAINT `FK_check_list_sites_folders_FolderId` FOREIGN KEY (`FolderId`) REFERENCES `Folders` (`Id`),
  CONSTRAINT `FK_check_list_sites_sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckListTranslationVersions`
--

DROP TABLE IF EXISTS `CheckListTranslationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckListTranslationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `CheckListId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Text` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `CheckListTranslationId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckListTranslations`
--

DROP TABLE IF EXISTS `CheckListTranslations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckListTranslations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `CheckListId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Text` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CheckLisTranslations_CheckListId` (`CheckListId`),
  CONSTRAINT `FK_CheckLisTranslations_CheckLists_CheckListId` FOREIGN KEY (`CheckListId`) REFERENCES `CheckLists` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckListValueVersions`
--

DROP TABLE IF EXISTS `CheckListValueVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckListValueVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CheckListValueId` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `Status` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  `CaseId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `CheckListDuplicateId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckListValues`
--

DROP TABLE IF EXISTS `CheckListValues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckListValues` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `Status` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  `CaseId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `CheckListDuplicateId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckListVersions`
--

DROP TABLE IF EXISTS `CheckListVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckListVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CheckListId` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Label` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Custom` longtext DEFAULT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `Repeated` int(11) DEFAULT NULL,
  `DisplayIndex` int(11) DEFAULT NULL,
  `CaseType` varchar(255) DEFAULT NULL,
  `FolderName` varchar(255) DEFAULT NULL,
  `ReviewEnabled` smallint(6) DEFAULT NULL,
  `ManualSync` smallint(6) DEFAULT NULL,
  `ExtraFieldsEnabled` smallint(6) DEFAULT NULL,
  `DoneButtonEnabled` smallint(6) DEFAULT NULL,
  `ApprovalEnabled` smallint(6) DEFAULT NULL,
  `MultiApproval` smallint(6) DEFAULT NULL,
  `FastNavigation` smallint(6) DEFAULT NULL,
  `DownloadEntities` smallint(6) DEFAULT NULL,
  `Field1` int(11) DEFAULT NULL,
  `Field2` int(11) DEFAULT NULL,
  `Field3` int(11) DEFAULT NULL,
  `Field4` int(11) DEFAULT NULL,
  `Field5` int(11) DEFAULT NULL,
  `Field6` int(11) DEFAULT NULL,
  `Field7` int(11) DEFAULT NULL,
  `Field8` int(11) DEFAULT NULL,
  `Field9` int(11) DEFAULT NULL,
  `Field10` int(11) DEFAULT NULL,
  `QuickSyncEnabled` smallint(6) DEFAULT NULL,
  `OriginalId` longtext DEFAULT NULL,
  `Color` longtext DEFAULT NULL,
  `DocxExportEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `JasperExportEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `ExcelExportEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `ReportH1` longtext DEFAULT NULL,
  `ReportH2` longtext DEFAULT NULL,
  `ReportH3` longtext DEFAULT NULL,
  `ReportH4` longtext DEFAULT NULL,
  `ReportH5` longtext DEFAULT NULL,
  `IsEditable` tinyint(1) NOT NULL DEFAULT 1,
  `IsHidden` tinyint(1) NOT NULL DEFAULT 0,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  `IsAchievable` tinyint(1) NOT NULL DEFAULT 0,
  `IsDoneAtEditable` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CheckLists`
--

DROP TABLE IF EXISTS `CheckLists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `CheckLists` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Label` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Custom` longtext DEFAULT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `Repeated` int(11) DEFAULT NULL,
  `DisplayIndex` int(11) DEFAULT NULL,
  `CaseType` varchar(255) DEFAULT NULL,
  `FolderName` varchar(255) DEFAULT NULL,
  `ReviewEnabled` smallint(6) DEFAULT NULL,
  `ManualSync` smallint(6) DEFAULT NULL,
  `ExtraFieldsEnabled` smallint(6) DEFAULT NULL,
  `DoneButtonEnabled` smallint(6) DEFAULT NULL,
  `ApprovalEnabled` smallint(6) DEFAULT NULL,
  `MultiApproval` smallint(6) DEFAULT NULL,
  `FastNavigation` smallint(6) DEFAULT NULL,
  `DownloadEntities` smallint(6) DEFAULT NULL,
  `Field1` int(11) DEFAULT NULL,
  `Field2` int(11) DEFAULT NULL,
  `Field3` int(11) DEFAULT NULL,
  `Field4` int(11) DEFAULT NULL,
  `Field5` int(11) DEFAULT NULL,
  `Field6` int(11) DEFAULT NULL,
  `Field7` int(11) DEFAULT NULL,
  `Field8` int(11) DEFAULT NULL,
  `Field9` int(11) DEFAULT NULL,
  `Field10` int(11) DEFAULT NULL,
  `QuickSyncEnabled` smallint(6) DEFAULT NULL,
  `OriginalId` longtext DEFAULT NULL,
  `Color` longtext DEFAULT NULL,
  `DocxExportEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `JasperExportEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `ExcelExportEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `ReportH1` longtext DEFAULT NULL,
  `ReportH2` longtext DEFAULT NULL,
  `ReportH3` longtext DEFAULT NULL,
  `ReportH4` longtext DEFAULT NULL,
  `ReportH5` longtext DEFAULT NULL,
  `IsEditable` tinyint(1) NOT NULL DEFAULT 1,
  `IsHidden` tinyint(1) NOT NULL DEFAULT 0,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  `IsAchievable` tinyint(1) NOT NULL DEFAULT 0,
  `IsDoneAtEditable` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `EntityGroupVersions`
--

DROP TABLE IF EXISTS `EntityGroupVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `EntityGroupVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EntityGroupId` int(11) NOT NULL DEFAULT 0,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `MicrotingUid` longtext DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Editable` tinyint(1) NOT NULL DEFAULT 0,
  `Locked` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `EntityGroups`
--

DROP TABLE IF EXISTS `EntityGroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `EntityGroups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `MicrotingUid` longtext DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Editable` tinyint(1) NOT NULL DEFAULT 0,
  `Locked` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `EntityItemVersions`
--

DROP TABLE IF EXISTS `EntityItemVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `EntityItemVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EntityItemId` int(11) NOT NULL DEFAULT 0,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `EntityGroupId` int(11) DEFAULT NULL,
  `EntityItemUid` varchar(50) DEFAULT NULL,
  `MicrotingUid` longtext DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Synced` smallint(6) DEFAULT NULL,
  `DisplayIndex` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `EntityItems`
--

DROP TABLE IF EXISTS `EntityItems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `EntityItems` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `EntityGroupId` int(11) NOT NULL DEFAULT 0,
  `EntityItemUid` varchar(50) DEFAULT NULL,
  `MicrotingUid` longtext DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Synced` smallint(6) DEFAULT NULL,
  `DisplayIndex` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ExtraFieldValueVersions`
--

DROP TABLE IF EXISTS `ExtraFieldValueVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ExtraFieldValueVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `ExtraFieldValueId` int(11) NOT NULL,
  `DoneAt` datetime(6) DEFAULT NULL,
  `Date` datetime(6) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `CaseId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `CheckListDuplicateId` int(11) DEFAULT NULL,
  `CheckListValueId` int(11) DEFAULT NULL,
  `UploadedDataId` int(11) DEFAULT NULL,
  `Value` longtext DEFAULT NULL,
  `Latitude` varchar(255) DEFAULT NULL,
  `Longitude` varchar(255) DEFAULT NULL,
  `Altitude` varchar(255) DEFAULT NULL,
  `Heading` varchar(255) DEFAULT NULL,
  `Accuracy` varchar(255) DEFAULT NULL,
  `FieldType` longtext DEFAULT NULL,
  `FieldTypeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ExtraFieldValues`
--

DROP TABLE IF EXISTS `ExtraFieldValues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ExtraFieldValues` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `DoneAt` datetime(6) DEFAULT NULL,
  `Date` datetime(6) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `CaseId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `CheckListDuplicateId` int(11) DEFAULT NULL,
  `CheckListValueId` int(11) DEFAULT NULL,
  `UploadedDataId` int(11) DEFAULT NULL,
  `Value` longtext DEFAULT NULL,
  `Latitude` varchar(255) DEFAULT NULL,
  `Longitude` varchar(255) DEFAULT NULL,
  `Altitude` varchar(255) DEFAULT NULL,
  `Heading` varchar(255) DEFAULT NULL,
  `Accuracy` varchar(255) DEFAULT NULL,
  `FieldType` longtext DEFAULT NULL,
  `FieldTypeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldOptionTranslationVersions`
--

DROP TABLE IF EXISTS `FieldOptionTranslationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldOptionTranslationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `FieldOptionId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Text` longtext DEFAULT NULL,
  `FieldOptionTranslationId` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldOptionTranslations`
--

DROP TABLE IF EXISTS `FieldOptionTranslations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldOptionTranslations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `FieldOptionId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Text` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_FieldOptionTranslations_FieldOptionId` (`FieldOptionId`),
  CONSTRAINT `FK_FieldOptionTranslations_FieldOptions_FieldOptionId` FOREIGN KEY (`FieldOptionId`) REFERENCES `FieldOptions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldOptionVersions`
--

DROP TABLE IF EXISTS `FieldOptionVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldOptionVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `FieldId` int(11) NOT NULL,
  `Key` longtext DEFAULT NULL,
  `Selected` tinyint(1) NOT NULL,
  `DisplayOrder` longtext DEFAULT NULL,
  `FieldOptionId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldOptions`
--

DROP TABLE IF EXISTS `FieldOptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldOptions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `FieldId` int(11) NOT NULL,
  `Key` longtext DEFAULT NULL,
  `Selected` tinyint(1) NOT NULL,
  `DisplayOrder` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_FieldOptions_FieldId` (`FieldId`),
  CONSTRAINT `FK_FieldOptions_Fields_FieldId` FOREIGN KEY (`FieldId`) REFERENCES `Fields` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldTranslationVersions`
--

DROP TABLE IF EXISTS `FieldTranslationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldTranslationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `FieldId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Text` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `FieldTranslationId` int(11) NOT NULL,
  `DefaultValue` longtext DEFAULT NULL,
  `UploadedDataId` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldTranslations`
--

DROP TABLE IF EXISTS `FieldTranslations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldTranslations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `FieldId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Text` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `DefaultValue` longtext DEFAULT NULL,
  `UploadedDataId` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_FieldTranslations_FieldId` (`FieldId`),
  CONSTRAINT `FK_FieldTranslations_Fields_FieldId` FOREIGN KEY (`FieldId`) REFERENCES `Fields` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldTypes`
--

DROP TABLE IF EXISTS `FieldTypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldTypes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Type` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `FieldTypes`
--

LOCK TABLES `FieldTypes` WRITE;
/*!40000 ALTER TABLE `FieldTypes` DISABLE KEYS */;
INSERT INTO `FieldTypes` VALUES (1,'Text','Simple text field'),(2,'Number','Simple number field'),(3,'None','Simple text to be displayed'),(4,'CheckBox','Simple check box field'),(5,'Picture','Simple picture field'),(6,'Audio','Simple audio field'),(7,'Movie','Simple movie field'),(8,'SingleSelect','Single selection list'),(9,'Comment','Simple comment field'),(10,'MultiSelect','Simple multi select list'),(11,'Date','Date selection'),(12,'Signature','Simple signature field'),(13,'Timer','Simple timer field'),(14,'EntitySearch','Autofilled searchable items field'),(15,'EntitySelect','Autofilled single selection list'),(16,'ShowPdf','Show PDF'),(17,'FieldGroup','Field group'),(18,'SaveButton','Save eForm'),(19,'NumberStepper','Number stepper field'),(20,'ShowPicture','Show picture');
/*!40000 ALTER TABLE `FieldTypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `FieldValueVersions`
--

DROP TABLE IF EXISTS `FieldValueVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldValueVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FieldValueId` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `DoneAt` datetime(6) DEFAULT NULL,
  `Date` datetime(6) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `CaseId` int(11) DEFAULT NULL,
  `FieldId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `CheckListDuplicateId` int(11) DEFAULT NULL,
  `UploadedDataId` int(11) DEFAULT NULL,
  `Value` longtext DEFAULT NULL,
  `Latitude` varchar(255) DEFAULT NULL,
  `Longitude` varchar(255) DEFAULT NULL,
  `Altitude` varchar(255) DEFAULT NULL,
  `Heading` varchar(255) DEFAULT NULL,
  `Accuracy` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldValues`
--

DROP TABLE IF EXISTS `FieldValues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldValues` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `DoneAt` datetime(6) DEFAULT NULL,
  `Date` datetime(6) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `CaseId` int(11) DEFAULT NULL,
  `FieldId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `CheckListDuplicateId` int(11) DEFAULT NULL,
  `UploadedDataId` int(11) DEFAULT NULL,
  `Value` longtext DEFAULT NULL,
  `Latitude` varchar(255) DEFAULT NULL,
  `Longitude` varchar(255) DEFAULT NULL,
  `Altitude` varchar(255) DEFAULT NULL,
  `Heading` varchar(255) DEFAULT NULL,
  `Accuracy` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_field_values_check_list_id` (`CheckListId`),
  KEY `IX_field_values_field_id` (`FieldId`),
  KEY `IX_field_values_uploaded_data_id` (`UploadedDataId`),
  KEY `IX_field_values_user_id` (`WorkerId`),
  KEY `IX_field_values_UploadedDataId` (`UploadedDataId`),
  CONSTRAINT `FK_field_values_uploaded_datas_UploadedDataId` FOREIGN KEY (`UploadedDataId`) REFERENCES `UploadedDatas` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FieldVersions`
--

DROP TABLE IF EXISTS `FieldVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FieldVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FieldId` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `ParentFieldId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `FieldTypeId` int(11) DEFAULT NULL,
  `Mandatory` smallint(6) DEFAULT NULL,
  `ReadOnly` smallint(6) DEFAULT NULL,
  `Label` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Color` varchar(255) DEFAULT NULL,
  `DisplayIndex` int(11) DEFAULT NULL,
  `Dummy` smallint(6) DEFAULT NULL,
  `DefaultValue` longtext DEFAULT NULL,
  `UnitName` varchar(255) DEFAULT NULL,
  `MinValue` varchar(255) DEFAULT NULL,
  `MaxValue` varchar(255) DEFAULT NULL,
  `MaxLength` int(11) DEFAULT NULL,
  `DecimalCount` int(11) DEFAULT NULL,
  `Multi` int(11) DEFAULT NULL,
  `Optional` smallint(6) DEFAULT NULL,
  `Selected` smallint(6) DEFAULT NULL,
  `Split` smallint(6) DEFAULT NULL,
  `GeolocationEnabled` smallint(6) DEFAULT NULL,
  `GeolocationForced` smallint(6) DEFAULT NULL,
  `GeolocationHidden` smallint(6) DEFAULT NULL,
  `StopOnSave` smallint(6) DEFAULT NULL,
  `IsNum` smallint(6) DEFAULT NULL,
  `BarcodeEnabled` smallint(6) DEFAULT NULL,
  `BarcodeType` varchar(255) DEFAULT NULL,
  `QueryType` varchar(255) DEFAULT NULL,
  `KeyValuePairList` longtext DEFAULT NULL,
  `Custom` longtext DEFAULT NULL,
  `EntityGroupId` int(11) DEFAULT NULL,
  `OriginalId` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Fields`
--

DROP TABLE IF EXISTS `Fields`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Fields` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `ParentFieldId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `FieldTypeId` int(11) DEFAULT NULL,
  `Mandatory` smallint(6) DEFAULT NULL,
  `ReadOnly` smallint(6) DEFAULT NULL,
  `Label` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Color` varchar(255) DEFAULT NULL,
  `DisplayIndex` int(11) DEFAULT NULL,
  `Dummy` smallint(6) DEFAULT NULL,
  `DefaultValue` longtext DEFAULT NULL,
  `UnitName` varchar(255) DEFAULT NULL,
  `MinValue` varchar(255) DEFAULT NULL,
  `MaxValue` varchar(255) DEFAULT NULL,
  `MaxLength` int(11) DEFAULT NULL,
  `DecimalCount` int(11) DEFAULT NULL,
  `Multi` int(11) DEFAULT NULL,
  `Optional` smallint(6) DEFAULT NULL,
  `Selected` smallint(6) DEFAULT NULL,
  `Split` smallint(6) DEFAULT NULL,
  `GeolocationEnabled` smallint(6) DEFAULT NULL,
  `GeolocationForced` smallint(6) DEFAULT NULL,
  `GeolocationHidden` smallint(6) DEFAULT NULL,
  `StopOnSave` smallint(6) DEFAULT NULL,
  `IsNum` smallint(6) DEFAULT NULL,
  `BarcodeEnabled` smallint(6) DEFAULT NULL,
  `BarcodeType` varchar(255) DEFAULT NULL,
  `QueryType` varchar(255) DEFAULT NULL,
  `KeyValuePairList` longtext DEFAULT NULL,
  `Custom` longtext DEFAULT NULL,
  `EntityGroupId` int(11) DEFAULT NULL,
  `parentid` int(11) DEFAULT NULL,
  `OriginalId` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_fields_check_list_id` (`CheckListId`),
  KEY `IX_fields_field_type_id` (`FieldTypeId`),
  KEY `IX_fields_parentid` (`parentid`),
  KEY `FK_fields_fields_ParentFieldId` (`ParentFieldId`),
  CONSTRAINT `FK_fields_check_lists_CheckListId` FOREIGN KEY (`CheckListId`) REFERENCES `CheckLists` (`Id`),
  CONSTRAINT `FK_fields_field_types_FieldTypeId` FOREIGN KEY (`FieldTypeId`) REFERENCES `FieldTypes` (`Id`),
  CONSTRAINT `FK_fields_fields_ParentFieldId` FOREIGN KEY (`ParentFieldId`) REFERENCES `Fields` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FolderTranslationVersions`
--

DROP TABLE IF EXISTS `FolderTranslationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FolderTranslationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `MicrotingUid` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `FolderId` int(11) NOT NULL,
  `FolderTranslationId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FolderTranslations`
--

DROP TABLE IF EXISTS `FolderTranslations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FolderTranslations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `MicrotingUid` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `FolderId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_FolderTranslations_FolderId` (`FolderId`),
  KEY `IX_FolderTranslations_LanguageId` (`LanguageId`),
  CONSTRAINT `FK_FolderTranslations_Folders_FolderId` FOREIGN KEY (`FolderId`) REFERENCES `Folders` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_FolderTranslations_Languages_LanguageId` FOREIGN KEY (`LanguageId`) REFERENCES `Languages` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FolderVersions`
--

DROP TABLE IF EXISTS `FolderVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `FolderVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `FolderId` int(11) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  `IsEditable` tinyint(1) NOT NULL DEFAULT 1,
  `ManagedByPlugin` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Folders`
--

DROP TABLE IF EXISTS `Folders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Folders` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  `IsEditable` tinyint(1) NOT NULL DEFAULT 1,
  `ManagedByPlugin` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_folders_parentid` (`ParentId`),
  CONSTRAINT `FK_folders_folders_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `Folders` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LanguageQuestionSetVersions`
--

DROP TABLE IF EXISTS `LanguageQuestionSetVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `LanguageQuestionSetVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `LanguageId` int(11) NOT NULL,
  `QuestionSetId` int(11) NOT NULL,
  `LanguageQuestionSetId` int(11) NOT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_LanguageQuestionSetVersions_LanguageQuestionSetId` (`LanguageQuestionSetId`),
  CONSTRAINT `FK_LanguageQuestionSetVersions_LanguageQuestionSets_LanguageQue~` FOREIGN KEY (`LanguageQuestionSetId`) REFERENCES `LanguageQuestionSets` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LanguageQuestionSets`
--

DROP TABLE IF EXISTS `LanguageQuestionSets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `LanguageQuestionSets` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `LanguageId` int(11) NOT NULL,
  `QuestionSetId` int(11) NOT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_LanguageQuestionSets_LanguageId` (`LanguageId`),
  KEY `IX_LanguageQuestionSets_QuestionSetId` (`QuestionSetId`),
  CONSTRAINT `FK_LanguageQuestionSets_languages_LanguageId` FOREIGN KEY (`LanguageId`) REFERENCES `Languages` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_LanguageQuestionSets_question_sets_QuestionSetId` FOREIGN KEY (`QuestionSetId`) REFERENCES `QuestionSets` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LanguageVersions`
--

DROP TABLE IF EXISTS `LanguageVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `LanguageVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `LanguageCode` longtext DEFAULT NULL,
  `LanguageId` int(11) NOT NULL DEFAULT 0,
  `IsActive` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  KEY `IX_language_versions_languageId` (`LanguageId`),
  CONSTRAINT `FK_language_versions_languages_LanguageId` FOREIGN KEY (`LanguageId`) REFERENCES `Languages` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `LanguageVersions`
--

LOCK TABLES `LanguageVersions` WRITE;
/*!40000 ALTER TABLE `LanguageVersions` DISABLE KEYS */;
INSERT INTO `LanguageVersions` VALUES (1,1,'created','2021-11-30 19:13:01.303334','2021-11-30 19:13:01.303378','Dansk','da',1,1),(2,1,'created','2021-11-30 19:13:01.416989','2021-11-30 19:13:01.416989','English','en-US',2,1),(3,1,'created','2021-11-30 19:13:01.462160','2021-11-30 19:13:01.462161','Deutsch','de-DE',3,1),(4,2,'created','2021-11-30 19:13:01.303334','2024-06-20 04:10:38.203493','Dansk','da',1,1),(5,2,'created','2021-11-30 19:13:01.462160','2024-06-20 04:10:38.551701','Deutsch','de-DE',3,1),(6,1,'created','2024-06-20 04:10:38.973603','2024-06-20 04:10:38.973605','українська','uk-UA',4,0),(7,1,'created','2024-06-20 04:10:39.228305','2024-06-20 04:10:39.228307','Polski','pl-PL',5,0),(8,1,'created','2024-06-20 04:10:39.491396','2024-06-20 04:10:39.491398','Norsk','no-NO',6,0),(9,1,'created','2024-06-20 04:10:39.875586','2024-06-20 04:10:39.875588','Svenska','sv-SE',7,0),(10,1,'created','2024-06-20 04:10:40.179595','2024-06-20 04:10:40.179597','Española','es-ES',8,0),(11,1,'created','2024-06-20 04:10:40.446787','2024-06-20 04:10:40.446789','Français','fr-FR',9,0),(12,1,'created','2024-06-20 04:10:40.752881','2024-06-20 04:10:40.752883','Italiana','it-IT',10,0),(13,1,'created','2024-06-20 04:10:41.104410','2024-06-20 04:10:41.104412','Neerlandais','nl-NL',11,0),(14,1,'created','2024-06-20 04:10:41.330388','2024-06-20 04:10:41.330390','Portugues do Brasil','pt-BR',12,0),(15,1,'created','2024-06-20 04:10:41.641572','2024-06-20 04:10:41.641574','Português','pt-PT',13,0),(16,1,'created','2024-06-20 04:10:41.938579','2024-06-20 04:10:41.938581','Suomalainen','fi-FI',14,0),(17,1,'created','2024-06-20 04:10:42.238166','2024-06-20 04:10:42.238167','Türkçe','tr-TR',15,0),(18,1,'created','2024-06-20 04:10:42.435563','2024-06-20 04:10:42.435565','Eesti','et-ET',16,0),(19,1,'created','2024-06-20 04:10:42.694959','2024-06-20 04:10:42.694961','Latviski','lv-LV',17,0),(20,1,'created','2024-06-20 04:10:42.931006','2024-06-20 04:10:42.931009','Lietuvių','lt-LT',18,0),(21,1,'created','2024-06-20 04:10:43.067721','2024-06-20 04:10:43.067723','Română','ro-RO',19,0),(22,1,'created','2024-06-20 04:10:43.302681','2024-06-20 04:10:43.302684','български','bg-BG',20,0),(23,1,'created','2024-06-20 04:10:43.562886','2024-06-20 04:10:43.562887','Slovenský','sk-SK',21,0),(24,1,'created','2024-06-20 04:10:43.733840','2024-06-20 04:10:43.733843','Slovenščina','sl-SL',22,0),(25,1,'created','2024-06-20 04:10:43.874318','2024-06-20 04:10:43.874320','Íslenska','is-IS',23,0),(26,1,'created','2024-06-20 04:10:44.197956','2024-06-20 04:10:44.197958','Čeština','cs-CZ',24,0),(27,1,'created','2024-06-20 04:10:44.436527','2024-06-20 04:10:44.436529','Hrvatski','hr-HR',25,0),(28,1,'created','2024-06-20 04:10:44.695423','2024-06-20 04:10:44.695425','Ελληνικά','el-GR',26,0),(29,1,'created','2024-06-20 04:10:44.952111','2024-06-20 04:10:44.952114','Magyar','hu-HU',27,0);
/*!40000 ALTER TABLE `LanguageVersions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Languages`
--

DROP TABLE IF EXISTS `Languages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Languages` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `LanguageCode` longtext DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Languages`
--

LOCK TABLES `Languages` WRITE;
/*!40000 ALTER TABLE `Languages` DISABLE KEYS */;
INSERT INTO `Languages` VALUES (1,2,'created','2021-11-30 19:13:01.303334','2024-06-20 04:10:38.203493','Dansk','da',1),(2,1,'created','2021-11-30 19:13:01.416989','2021-11-30 19:13:01.416989','English','en-US',1),(3,2,'created','2021-11-30 19:13:01.462160','2024-06-20 04:10:38.551701','Deutsch','de-DE',1),(4,1,'created','2024-06-20 04:10:38.973603','2024-06-20 04:10:38.973605','українська','uk-UA',0),(5,1,'created','2024-06-20 04:10:39.228305','2024-06-20 04:10:39.228307','Polski','pl-PL',0),(6,1,'created','2024-06-20 04:10:39.491396','2024-06-20 04:10:39.491398','Norsk','no-NO',0),(7,1,'created','2024-06-20 04:10:39.875586','2024-06-20 04:10:39.875588','Svenska','sv-SE',0),(8,1,'created','2024-06-20 04:10:40.179595','2024-06-20 04:10:40.179597','Española','es-ES',0),(9,1,'created','2024-06-20 04:10:40.446787','2024-06-20 04:10:40.446789','Français','fr-FR',0),(10,1,'created','2024-06-20 04:10:40.752881','2024-06-20 04:10:40.752883','Italiana','it-IT',0),(11,1,'created','2024-06-20 04:10:41.104410','2024-06-20 04:10:41.104412','Neerlandais','nl-NL',0),(12,1,'created','2024-06-20 04:10:41.330388','2024-06-20 04:10:41.330390','Portugues do Brasil','pt-BR',0),(13,1,'created','2024-06-20 04:10:41.641572','2024-06-20 04:10:41.641574','Português','pt-PT',0),(14,1,'created','2024-06-20 04:10:41.938579','2024-06-20 04:10:41.938581','Suomalainen','fi-FI',0),(15,1,'created','2024-06-20 04:10:42.238166','2024-06-20 04:10:42.238167','Türkçe','tr-TR',0),(16,1,'created','2024-06-20 04:10:42.435563','2024-06-20 04:10:42.435565','Eesti','et-ET',0),(17,1,'created','2024-06-20 04:10:42.694959','2024-06-20 04:10:42.694961','Latviski','lv-LV',0),(18,1,'created','2024-06-20 04:10:42.931006','2024-06-20 04:10:42.931009','Lietuvių','lt-LT',0),(19,1,'created','2024-06-20 04:10:43.067721','2024-06-20 04:10:43.067723','Română','ro-RO',0),(20,1,'created','2024-06-20 04:10:43.302681','2024-06-20 04:10:43.302684','български','bg-BG',0),(21,1,'created','2024-06-20 04:10:43.562886','2024-06-20 04:10:43.562887','Slovenský','sk-SK',0),(22,1,'created','2024-06-20 04:10:43.733840','2024-06-20 04:10:43.733843','Slovenščina','sl-SL',0),(23,1,'created','2024-06-20 04:10:43.874318','2024-06-20 04:10:43.874320','Íslenska','is-IS',0),(24,1,'created','2024-06-20 04:10:44.197956','2024-06-20 04:10:44.197958','Čeština','cs-CZ',0),(25,1,'created','2024-06-20 04:10:44.436527','2024-06-20 04:10:44.436529','Hrvatski','hr-HR',0),(26,1,'created','2024-06-20 04:10:44.695423','2024-06-20 04:10:44.695425','Ελληνικά','el-GR',0),(27,1,'created','2024-06-20 04:10:44.952111','2024-06-20 04:10:44.952114','Magyar','hu-HU',0);
/*!40000 ALTER TABLE `Languages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `NotificationVersions`
--

DROP TABLE IF EXISTS `NotificationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `NotificationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `Transmission` longtext DEFAULT NULL,
  `NotificationUid` varchar(255) DEFAULT NULL,
  `Activity` longtext DEFAULT NULL,
  `Exception` longtext DEFAULT NULL,
  `Stacktrace` longtext DEFAULT NULL,
  `Version` int(11) NOT NULL,
  `NotificationId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Notifications`
--

DROP TABLE IF EXISTS `Notifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Notifications` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `Transmission` longtext DEFAULT NULL,
  `NotificationUid` varchar(255) DEFAULT NULL,
  `Activity` longtext DEFAULT NULL,
  `Exception` longtext DEFAULT NULL,
  `Stacktrace` longtext DEFAULT NULL,
  `Version` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `OptionTranslationVersions`
--

DROP TABLE IF EXISTS `OptionTranslationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `OptionTranslationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `OptionId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `OptionTranslationId` int(11) NOT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `OptionTranslations`
--

DROP TABLE IF EXISTS `OptionTranslations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `OptionTranslations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `OptionId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_OptionTranslations_LanguageId` (`LanguageId`),
  KEY `IX_OptionTranslations_OptionId` (`OptionId`),
  CONSTRAINT `FK_OptionTranslations_languages_LanguageId` FOREIGN KEY (`LanguageId`) REFERENCES `Languages` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_OptionTranslations_options_OptionId` FOREIGN KEY (`OptionId`) REFERENCES `Options` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `OptionVersions`
--

DROP TABLE IF EXISTS `OptionVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `OptionVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `NextQuestionId` int(11) DEFAULT NULL,
  `Weight` int(11) NOT NULL DEFAULT 0,
  `WeightValue` int(11) NOT NULL DEFAULT 0,
  `ContinuousOptionId` int(11) NOT NULL DEFAULT 0,
  `QuestionId` int(11) NOT NULL DEFAULT 0,
  `OptionIndex` int(11) NOT NULL DEFAULT 0,
  `OptionId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  `DisplayIndex` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_option_versions_optionId` (`OptionId`),
  CONSTRAINT `FK_option_versions_options_OptionId` FOREIGN KEY (`OptionId`) REFERENCES `Options` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Options`
--

DROP TABLE IF EXISTS `Options`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Options` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `NextQuestionId` int(11) DEFAULT NULL,
  `Weight` int(11) NOT NULL DEFAULT 0,
  `WeightValue` int(11) NOT NULL DEFAULT 0,
  `ContinuousOptionId` int(11) NOT NULL DEFAULT 0,
  `QuestionId` int(11) NOT NULL DEFAULT 0,
  `OptionIndex` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  `DisplayIndex` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_options_questionId` (`QuestionId`),
  CONSTRAINT `FK_options_questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `QuestionSetVersions`
--

DROP TABLE IF EXISTS `QuestionSetVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionSetVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `HasChild` tinyint(1) NOT NULL DEFAULT 0,
  `PossiblyDeployed` tinyint(1) NOT NULL DEFAULT 0,
  `ParentId` int(11) NOT NULL DEFAULT 0,
  `Share` tinyint(1) NOT NULL DEFAULT 0,
  `QuestionSetId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_question_set_versions_questionSetId` (`QuestionSetId`),
  CONSTRAINT `FK_question_set_versions_question_sets_QuestionSetId` FOREIGN KEY (`QuestionSetId`) REFERENCES `QuestionSets` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `QuestionSets`
--

DROP TABLE IF EXISTS `QuestionSets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionSets` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` longtext DEFAULT NULL,
  `HasChild` tinyint(1) NOT NULL DEFAULT 0,
  `PossiblyDeployed` tinyint(1) NOT NULL DEFAULT 0,
  `ParentId` int(11) NOT NULL DEFAULT 0,
  `Share` tinyint(1) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `QuestionTranslationVersions`
--

DROP TABLE IF EXISTS `QuestionTranslationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionTranslationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `QuestionId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `QuestionTranslationId` int(11) NOT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_QuestionTranslationVersions_QuestionTranslationId` (`QuestionTranslationId`),
  CONSTRAINT `FK_QuestionTranslationVersions_QuestionTranslations_QuestionTra~` FOREIGN KEY (`QuestionTranslationId`) REFERENCES `QuestionTranslations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `QuestionTranslations`
--

DROP TABLE IF EXISTS `QuestionTranslations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionTranslations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `QuestionId` int(11) NOT NULL,
  `LanguageId` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_QuestionTranslations_LanguageId` (`LanguageId`),
  KEY `IX_QuestionTranslations_QuestionId` (`QuestionId`),
  CONSTRAINT `FK_QuestionTranslations_languages_LanguageId` FOREIGN KEY (`LanguageId`) REFERENCES `Languages` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_QuestionTranslations_questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `QuestionVersions`
--

DROP TABLE IF EXISTS `QuestionVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `QuestionSetId` int(11) NOT NULL DEFAULT 0,
  `QuestionType` longtext DEFAULT NULL,
  `Minimum` int(11) NOT NULL DEFAULT 0,
  `Maximum` int(11) NOT NULL DEFAULT 0,
  `Type` longtext DEFAULT NULL,
  `RefId` int(11) NOT NULL DEFAULT 0,
  `QuestionIndex` int(11) NOT NULL DEFAULT 0,
  `Image` tinyint(1) NOT NULL DEFAULT 0,
  `ContinuousQuestionId` int(11) NOT NULL DEFAULT 0,
  `ImagePosition` longtext DEFAULT NULL,
  `Prioritised` tinyint(1) NOT NULL DEFAULT 0,
  `BackButtonEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `FontSize` longtext DEFAULT NULL,
  `MinDuration` int(11) NOT NULL DEFAULT 0,
  `MaxDuration` int(11) NOT NULL DEFAULT 0,
  `ValidDisplay` tinyint(1) NOT NULL DEFAULT 0,
  `QuestionId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_question_versions_questionId` (`QuestionId`),
  CONSTRAINT `FK_question_versions_questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Questions`
--

DROP TABLE IF EXISTS `Questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Questions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `QuestionSetId` int(11) NOT NULL DEFAULT 0,
  `QuestionType` longtext DEFAULT NULL,
  `Minimum` int(11) NOT NULL DEFAULT 0,
  `Maximum` int(11) NOT NULL DEFAULT 0,
  `Type` longtext DEFAULT NULL,
  `RefId` int(11) NOT NULL DEFAULT 0,
  `QuestionIndex` int(11) NOT NULL DEFAULT 0,
  `Image` tinyint(1) NOT NULL DEFAULT 0,
  `ContinuousQuestionId` int(11) NOT NULL DEFAULT 0,
  `ImagePosition` longtext DEFAULT NULL,
  `Prioritised` tinyint(1) NOT NULL DEFAULT 0,
  `BackButtonEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `FontSize` longtext DEFAULT NULL,
  `MinDuration` int(11) NOT NULL DEFAULT 0,
  `MaxDuration` int(11) NOT NULL DEFAULT 0,
  `ValidDisplay` tinyint(1) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_questions_questionSetId` (`QuestionSetId`),
  CONSTRAINT `FK_questions_question_sets_QuestionSetId` FOREIGN KEY (`QuestionSetId`) REFERENCES `QuestionSets` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SettingVersions`
--

DROP TABLE IF EXISTS `SettingVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SettingVersions` (
  `Id` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Value` longtext DEFAULT NULL,
  `ChangedByName` longtext DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Version` int(11) NOT NULL,
  `SettingId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Settings`
--

DROP TABLE IF EXISTS `Settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Settings` (
  `Id` int(11) NOT NULL DEFAULT 0,
  `Name` varchar(50) NOT NULL DEFAULT '',
  `Value` longtext DEFAULT NULL,
  `ChangedByName` longtext DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Version` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SiteSurveyConfigurationVersions`
--

DROP TABLE IF EXISTS `SiteSurveyConfigurationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SiteSurveyConfigurationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) NOT NULL DEFAULT 0,
  `SurveyConfigurationId` int(11) NOT NULL DEFAULT 0,
  `SiteSurveyConfigurationId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_site_survey_configuration_versions_siteSurveyConfigurationId` (`SiteSurveyConfigurationId`),
  CONSTRAINT `FK_site_survey_configuration_versions_site_survey_configuration~` FOREIGN KEY (`SiteSurveyConfigurationId`) REFERENCES `SiteSurveyConfigurations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SiteSurveyConfigurations`
--

DROP TABLE IF EXISTS `SiteSurveyConfigurations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SiteSurveyConfigurations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) NOT NULL DEFAULT 0,
  `SurveyConfigurationId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_site_survey_configurations_siteId` (`SiteId`),
  KEY `IX_site_survey_configurations_surveyConfigurationId` (`SurveyConfigurationId`),
  CONSTRAINT `FK_site_survey_configurations_sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_site_survey_configurations_survey_configurations_SurveyConfi~` FOREIGN KEY (`SurveyConfigurationId`) REFERENCES `SurveyConfigurations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SiteTagVersions`
--

DROP TABLE IF EXISTS `SiteTagVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SiteTagVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `TagId` int(11) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `SiteTagId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SiteTags`
--

DROP TABLE IF EXISTS `SiteTags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SiteTags` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `TagId` int(11) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SiteTags_SiteId` (`SiteId`),
  KEY `IX_SiteTags_TagId` (`TagId`),
  CONSTRAINT `FK_SiteTags_sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`),
  CONSTRAINT `FK_SiteTags_tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tags` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SiteVersions`
--

DROP TABLE IF EXISTS `SiteVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SiteVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `LanguageId` int(11) NOT NULL DEFAULT 0,
  `SearchableEntityItemId` int(11) NOT NULL DEFAULT 0,
  `SelectableEntityItemId` int(11) NOT NULL DEFAULT 0,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SiteWorkerVersions`
--

DROP TABLE IF EXISTS `SiteWorkerVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SiteWorkerVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SiteId` int(11) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `SiteWorkerId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SiteWorkers`
--

DROP TABLE IF EXISTS `SiteWorkers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SiteWorkers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SiteId` int(11) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_site_workers_site_id` (`SiteId`),
  KEY `IX_site_workers_worker_id` (`WorkerId`),
  CONSTRAINT `FK_site_workers_sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`),
  CONSTRAINT `FK_site_workers_workers_WorkerId` FOREIGN KEY (`WorkerId`) REFERENCES `Workers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Sites`
--

DROP TABLE IF EXISTS `Sites`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Sites` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `MicrotingUid` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `LanguageId` int(11) NOT NULL DEFAULT 0,
  `SearchableEntityItemId` int(11) NOT NULL DEFAULT 0,
  `SelectableEntityItemId` int(11) NOT NULL DEFAULT 0,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SurveyConfigurationVersions`
--

DROP TABLE IF EXISTS `SurveyConfigurationVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SurveyConfigurationVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Start` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Stop` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `TimeToLive` int(11) NOT NULL DEFAULT 0,
  `Name` longtext DEFAULT NULL,
  `TimeOut` int(11) NOT NULL DEFAULT 0,
  `SurveyConfigurationId` int(11) NOT NULL DEFAULT 0,
  `QuestionSetId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_survey_configuration_versions_surveyConfigurationId` (`SurveyConfigurationId`),
  CONSTRAINT `FK_survey_configuration_versions_survey_configurations_SurveyCo~` FOREIGN KEY (`SurveyConfigurationId`) REFERENCES `SurveyConfigurations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SurveyConfigurations`
--

DROP TABLE IF EXISTS `SurveyConfigurations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `SurveyConfigurations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Start` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `Stop` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `TimeToLive` int(11) NOT NULL DEFAULT 0,
  `Name` longtext DEFAULT NULL,
  `TimeOut` int(11) NOT NULL DEFAULT 0,
  `QuestionSetId` int(11) NOT NULL DEFAULT 0,
  `MicrotingUid` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_survey_configurations_QuestionSetId` (`QuestionSetId`),
  CONSTRAINT `FK_survey_configurations_question_sets_QuestionSetId` FOREIGN KEY (`QuestionSetId`) REFERENCES `QuestionSets` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TagVersions`
--

DROP TABLE IF EXISTS `TagVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TagVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `TaggingsCount` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `TagId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TaggingVersions`
--

DROP TABLE IF EXISTS `TaggingVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TaggingVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TagId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `TaggerId` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `TaggingId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Taggings`
--

DROP TABLE IF EXISTS `Taggings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Taggings` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TagId` int(11) DEFAULT NULL,
  `CheckListId` int(11) DEFAULT NULL,
  `TaggerId` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_taggings_check_list_id` (`CheckListId`),
  KEY `IX_taggings_tag_id` (`TagId`),
  CONSTRAINT `FK_taggings_check_lists_CheckListId` FOREIGN KEY (`CheckListId`) REFERENCES `CheckLists` (`Id`),
  CONSTRAINT `FK_taggings_tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tags` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Tags`
--

DROP TABLE IF EXISTS `Tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Tags` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `TaggingsCount` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UnitVersions`
--

DROP TABLE IF EXISTS `UnitVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `UnitVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MicrotingUid` int(11) DEFAULT NULL,
  `OtpCode` int(11) DEFAULT NULL,
  `CustomerNo` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `UnitId` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `Manufacturer` longtext DEFAULT NULL,
  `Model` longtext DEFAULT NULL,
  `Note` longtext DEFAULT NULL,
  `OsVersion` longtext DEFAULT NULL,
  `eFormVersion` longtext DEFAULT NULL,
  `InSightVersion` longtext DEFAULT NULL,
  `Os` longtext DEFAULT NULL,
  `LastIp` longtext DEFAULT NULL,
  `LeftMenuEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `PushEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `SeparateFetchSend` tinyint(1) NOT NULL DEFAULT 0,
  `SerialNumber` longtext DEFAULT NULL,
  `SyncDefaultDelay` int(11) NOT NULL DEFAULT 0,
  `SyncDelayEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `SyncDelayPrCheckList` int(11) NOT NULL DEFAULT 0,
  `SyncDialog` tinyint(1) NOT NULL DEFAULT 0,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Units`
--

DROP TABLE IF EXISTS `Units`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Units` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MicrotingUid` int(11) DEFAULT NULL,
  `OtpCode` int(11) DEFAULT NULL,
  `CustomerNo` int(11) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `Manufacturer` longtext DEFAULT NULL,
  `Model` longtext DEFAULT NULL,
  `Note` longtext DEFAULT NULL,
  `OsVersion` longtext DEFAULT NULL,
  `eFormVersion` longtext DEFAULT NULL,
  `InSightVersion` longtext DEFAULT NULL,
  `Os` longtext DEFAULT NULL,
  `LastIp` longtext DEFAULT NULL,
  `LeftMenuEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `PushEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `SeparateFetchSend` tinyint(1) NOT NULL DEFAULT 0,
  `SerialNumber` longtext DEFAULT NULL,
  `SyncDefaultDelay` int(11) NOT NULL DEFAULT 0,
  `SyncDelayEnabled` tinyint(1) NOT NULL DEFAULT 0,
  `SyncDelayPrCheckList` int(11) NOT NULL DEFAULT 0,
  `SyncDialog` tinyint(1) NOT NULL DEFAULT 0,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_units_site_id` (`SiteId`),
  CONSTRAINT `FK_units_sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `Sites` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UploadedDataVersions`
--

DROP TABLE IF EXISTS `UploadedDataVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `UploadedDataVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UploadedDataId` int(11) DEFAULT NULL,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UploaderId` int(11) DEFAULT NULL,
  `Checksum` varchar(255) DEFAULT NULL,
  `Extension` varchar(255) DEFAULT NULL,
  `CurrentFile` varchar(255) DEFAULT NULL,
  `UploaderType` varchar(255) DEFAULT NULL,
  `FileLocation` varchar(255) DEFAULT NULL,
  `FileName` varchar(255) DEFAULT NULL,
  `ExpirationDate` datetime(6) DEFAULT NULL,
  `Local` smallint(6) DEFAULT NULL,
  `TranscriptionId` int(11) DEFAULT NULL,
  `OriginalFileLocation` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UploadedDatas`
--

DROP TABLE IF EXISTS `UploadedDatas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `UploadedDatas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UploaderId` int(11) DEFAULT NULL,
  `Checksum` varchar(255) DEFAULT NULL,
  `Extension` varchar(255) DEFAULT NULL,
  `CurrentFile` varchar(255) DEFAULT NULL,
  `UploaderType` varchar(255) DEFAULT NULL,
  `FileLocation` varchar(255) DEFAULT NULL,
  `FileName` varchar(255) DEFAULT NULL,
  `ExpirationDate` datetime(6) DEFAULT NULL,
  `Local` smallint(6) DEFAULT NULL,
  `TranscriptionId` int(11) DEFAULT NULL,
  `OriginalFileLocation` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `WorkerVersions`
--

DROP TABLE IF EXISTS `WorkerVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `WorkerVersions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `MicrotingUid` int(11) NOT NULL DEFAULT 0,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  `Initials` varchar(3) DEFAULT NULL,
  `EmployeeNo` varchar(50) DEFAULT NULL,
  `PinCode` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Workers`
--

DROP TABLE IF EXISTS `Workers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Workers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `MicrotingUid` int(11) NOT NULL DEFAULT 0,
  `WorkflowState` varchar(255) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `IsLocked` tinyint(1) NOT NULL DEFAULT 0,
  `Initials` varchar(3) DEFAULT NULL,
  `EmployeeNo` varchar(50) DEFAULT NULL,
  `PinCode` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20180810124416_InitialCreate','5.0.11'),('20190116110009_AddingOriginalId','5.0.11'),('20190315092242_AddingModelseForInsight','5.0.11'),('20190318122928_FixingNamingOfSurveyConfigurationSites','5.0.11'),('20190319130214_AddingMissingForeignKeys','5.0.11'),('20190408081151_AddingFolders','5.0.11'),('20190408084408_AddingMissingParentId','5.0.11'),('20190509074123_RefactoringidtoId','5.0.11'),('20190514053645_RefactoringAttributeNames','5.0.11'),('20190515064952_FixingNamingForFieldValues','5.0.11'),('20190531092007_AddingMissingAIonLogs','5.0.11'),('20190711053344_AddingJasperDocxEnabledAttributesToCheckList','5.0.11'),('20190828054730_AddingNewVersionClasses','5.0.11'),('20190828074017_AddingMissingClasses','5.0.11'),('20190923100451_ChangeStringToInt','5.0.11'),('20190924172326_AddingNewIndexOnCases','5.0.11'),('20200116074236_AddingSiteTaggins','5.0.11'),('20200120093951_CleanupInSight','5.0.11'),('20200120164857_AddingTranslationsToInSight','5.0.11'),('20200120171433_AddingMicrotingUidToInSight','5.0.11'),('20200122103229_ChangingValueToBeStringForAnswerValue','5.0.11'),('20200222140656_AddinDisplayIndexToOptions','5.0.11'),('20200224084023_AddingAttributesToUnits','5.0.11'),('20200224092512_AddingMoreAttributesToUnits','5.0.11'),('20200226182616_MakingNextQuestionIdNullable','5.0.11'),('20200318150742_MakingUnitIdNullableForAnswers','5.0.11'),('20200427095029_AdjustTimeToUTC','5.0.11'),('20200513142551_AddingFolderIdToCasesAndCheckListSites','5.0.11'),('20200617160004_ChangingOptionsIndexToOptionIndex','5.0.11'),('20200620171527_AddingExcelExportEnabledToCheckList','5.0.11'),('20200701101500_LettingSurveyConfigurationIdBeNullable','5.0.11'),('20201116164405_AddingDescriptionToEntityGroup','5.0.11'),('20201130204234_FixingSplitScreen','5.0.11'),('20201220194822_FixingTableColumnNames','5.0.11'),('20201220201427_FixingQuestionSet','5.0.11'),('20201222125152_HugheTableRenaming','5.0.11'),('20201223104631_AddingTranslations','5.0.11'),('20201225165255_FixingBrokenTableNames','5.0.11'),('20201231062732_ChangingDescriptToLanguageCode','5.0.11'),('20210405153325_AddingExtraFieldValues','5.0.11'),('20210407134630_AddingFolderTranslations','5.0.11'),('20210609072417_AddingLinkingOfSitesAndEntities','5.0.11'),('20210730085329_AddingDefaultValueToFieldTranslations','5.0.11'),('20211014105943_CLAttributes','5.0.11'),('20211108111024_AddingIsArchivedToCases','6.0.0'),('20211116085744_AddingDoneAtEditable','6.0.0'),('20220207094729_AddingIsLockedToSiteUnitWorkers','6.0.2'),('20221016081344_AddingIsActiveToLanguage','8.0.6'),('20221129082337_AddingReceivedByServerAtToCases','8.0.6'),('20230506062507_AddingInitialsToWorkers','8.0.6'),('20230607084834_AddingOriginalFileLocationToUploadedData','8.0.6'),('20240619132520_AddPinCodeEmployeeNoToWorker','8.0.6');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-06-20  6:12:19