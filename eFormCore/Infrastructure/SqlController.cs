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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Extensions;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using Microting.eForm.Infrastructure.Models.reply;
using Case = Microting.eForm.Infrastructure.Data.Entities.Case;
using CheckListValue = Microting.eForm.Infrastructure.Data.Entities.CheckListValue;
using Comment = Microting.eForm.Infrastructure.Models.Comment;
using EntityGroup = Microting.eForm.Infrastructure.Data.Entities.EntityGroup;
using EntityItem = Microting.eForm.Infrastructure.Data.Entities.EntityItem;
using Field = Microting.eForm.Infrastructure.Models.Field;
using FieldGroup = Microting.eForm.Infrastructure.Models.FieldGroup;
using FieldValue = Microting.eForm.Infrastructure.Data.Entities.FieldValue;
using KeyValuePair = Microting.eForm.Dto.KeyValuePair;
using Picture = Microting.eForm.Infrastructure.Models.Picture;
using Tag = Microting.eForm.Infrastructure.Data.Entities.Tag;
using Text = Microting.eForm.Infrastructure.Models.Text;
using UploadedData = Microting.eForm.Infrastructure.Data.Entities.UploadedData;

//using eFormSqlController.Migrations;

namespace Microting.eForm.Infrastructure
{
    public class SqlController : LogWriter
    {
        #region var

        private Log log;
        private readonly Tools t = new Tools();
        private List<Holder> converter = null;
        private readonly bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        //private int logLimit = 0;
        private readonly DbContextHelper dbContextHelper;
        #endregion

        #region con
        public SqlController(DbContextHelper dbContextHelper)
        {
            this.dbContextHelper = dbContextHelper;
            string methodName = "SqlController.SqlController";

            #region migrate if needed
            try
            {
                using var db = GetContext();
                if (db.Database.GetPendingMigrations().Any())
                {
                    WriteDebugConsoleLogEntry(new LogEntry(2, methodName, "db.Database.Migrate() called"));
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            #endregion

            //region set default for settings if needed
            if (SettingCheckAll().GetAwaiter().GetResult().Count > 0)
            {
                bool result = SettingCreateDefaults().GetAwaiter().GetResult();
            }
            if (SettingRead(Settings.translationsMigrated).GetAwaiter().GetResult() == "false")
            {
                Language.AddDefaultLanguages(GetContext()).GetAwaiter().GetResult();
                CheckList.MoveTranslations(GetContext()).GetAwaiter().GetResult();
                Data.Entities.Field.MoveTranslations(GetContext()).GetAwaiter().GetResult();
                Site.AddLanguage(GetContext()).GetAwaiter().GetResult();
                SettingUpdate(Settings.translationsMigrated, "true").GetAwaiter().GetResult();
            }
        }

        private MicrotingDbContext GetContext()
        {
            return dbContextHelper.GetDbContext();
        }
        #endregion

        #region public
        #region public template

       //TODO
        public async Task<int> TemplateCreate(MainElement mainElement)
        {
            string methodName = "SqlController.TemplateCreate";
            try
            {
                var result = await EformCreateDb(mainElement).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<MainElement> ReadeForm(int templateId, Language language)
        {
            string methodName = "SqlController.TemplateRead";
            try
            {
                await using var db = GetContext();
                MainElement mainElement = null;
                GetConverter();

                CheckList mainCl = await db.CheckLists.SingleOrDefaultAsync(x => x.Id == templateId && (x.ParentId == null || x.ParentId == 0));

                if (mainCl == null)
                    return null;

                CheckListTranslation checkListTranslation =
                    await db.CheckListTranslations.SingleOrDefaultAsync(x => x.CheckListId == mainCl.Id && x.LanguageId == language.Id);
                if (checkListTranslation == null)
                {
                    checkListTranslation = await db.CheckListTranslations.FirstAsync(x => x.CheckListId == mainCl.Id);
                    language = db.Languages.Single(x => x.Id == checkListTranslation.LanguageId);
                }

                mainElement = new MainElement(mainCl.Id, checkListTranslation.Text, t.Int(mainCl.DisplayIndex), mainCl.FolderName, t.Int(mainCl.Repeated), DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "da",
                    t.Bool(mainCl.MultiApproval), t.Bool(mainCl.FastNavigation), t.Bool(mainCl.DownloadEntities), t.Bool(mainCl.ManualSync), mainCl.CaseType, "", "", t.Bool(mainCl.QuickSyncEnabled), new List<Element>(), mainCl.Color);

                //getting elements
                List<CheckList> lst = db.CheckLists.Where(x => x.ParentId == templateId).ToList();
                foreach (CheckList cl in lst)
                {
                    mainElement.ElementList.Add(await GetElement(cl.Id, language));
                }

                //return
                return mainElement;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<Template_Dto> TemplateItemRead(int templateId, Language language)
        {
            string methodName = "SqlController.TemplateItemRead";

            try
            {
                await using var db = GetContext();
                CheckList checkList = await db.CheckLists.SingleOrDefaultAsync(x => x.Id == templateId);

                if (checkList == null)
                    return null;

                List<SiteNameDto> sites = new List<SiteNameDto>();
                List<CheckListSite> checkListSites =
                    await db.CheckListSites.Where(x => x.CheckListId == checkList.Id && x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToListAsync();
                foreach (CheckListSite checkListSite in checkListSites)
                {
                    Site site = await db.Sites.SingleAsync(x => x.Id == checkListSite.SiteId);
                    SiteNameDto siteNameDto = new SiteNameDto((int)site.MicrotingUid, site.Name, site.CreatedAt, site.UpdatedAt);
                    sites.Add(siteNameDto);
                }
                bool hasCases = db.Cases.Where(x => x.CheckListId == checkList.Id).AsQueryable().Count() != 0;

                CheckListTranslation checkListTranslation =
                    await db.CheckListTranslations.SingleOrDefaultAsync(x =>
                        x.CheckListId == checkList.Id && x.LanguageId == language.Id);
                if (checkListTranslation == null)
                {
                    checkListTranslation =
                        await db.CheckListTranslations.FirstAsync(x => x.CheckListId == checkList.Id);
                    language = await db.Languages.SingleAsync(x => x.Id == checkListTranslation.LanguageId);
                }

                #region load fields
                FieldDto fd1 = null;
                FieldDto fd2 = null;
                FieldDto fd3 = null;
                FieldDto fd4 = null;
                FieldDto fd5 = null;
                FieldDto fd6 = null;
                FieldDto fd7 = null;
                FieldDto fd8 = null;
                FieldDto fd9 = null;
                FieldDto fd10 = null;

                Data.Entities.Field f1 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field1);
                if (f1 != null)
                {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f1.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f1.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f1.FieldTypeId);
                    fd1 = new FieldDto(f1.Id, translation.Text, translation.Description, (int)f1.FieldTypeId, fieldType.Type, (int)f1.CheckListId);
                }

                Data.Entities.Field f2 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field2);
                if (f2 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f2.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f2.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f2.FieldTypeId);
                    fd2 = new FieldDto(f2.Id, translation.Text, translation.Description, (int)f2.FieldTypeId, f2.FieldType.Type, (int)f2.CheckListId);
                }

                Data.Entities.Field f3 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field3);
                if (f3 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f3.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f3.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f3.FieldTypeId);
                    fd3 = new FieldDto(f3.Id, translation.Text, translation.Description, (int)f3.FieldTypeId, f3.FieldType.Type, (int)f3.CheckListId);
                }

                Data.Entities.Field f4 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field4);
                if (f4 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f4.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f4.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f4.FieldTypeId);
                    fd4 = new FieldDto(f4.Id, translation.Text, translation.Description, (int)f4.FieldTypeId, f4.FieldType.Type, (int)f4.CheckListId);
                }

                Data.Entities.Field f5 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field5);
                if (f5 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f5.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f5.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f5.FieldTypeId);
                    fd5 = new FieldDto(f5.Id, translation.Text, translation.Description, (int)f5.FieldTypeId, f5.FieldType.Type, (int)f5.CheckListId);
                }

                Data.Entities.Field f6 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field6);
                if (f6 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f6.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f6.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f6.FieldTypeId);
                    fd6 = new FieldDto(f6.Id, translation.Text, translation.Description, (int)f6.FieldTypeId, f6.FieldType.Type, (int)f6.CheckListId);
                }

                Data.Entities.Field f7 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field7);
                if (f7 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f7.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f7.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f7.FieldTypeId);
                    fd7 = new FieldDto(f7.Id, translation.Text, translation.Description, (int)f7.FieldTypeId, f7.FieldType.Type, (int)f7.CheckListId);
                }

                Data.Entities.Field f8 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field8);
                if (f8 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f8.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f8.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f8.FieldTypeId);
                    fd8 = new FieldDto(f8.Id, translation.Text, translation.Description, (int)f8.FieldTypeId, f8.FieldType.Type, (int)f8.CheckListId);
                }

                Data.Entities.Field f9 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field9);
                if (f9 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f9.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f9.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f9.FieldTypeId);
                    fd9 = new FieldDto(f9.Id, translation.Text, translation.Description, (int)f9.FieldTypeId, f9.FieldType.Type, (int)f9.CheckListId);
                }

                Data.Entities.Field f10 = await db.Fields.SingleOrDefaultAsync(x => x.Id == checkList.Field10);
                if (f10 != null) {
                    FieldTranslation translation = await db.FieldTranslations
                        .SingleOrDefaultAsync(x => x.FieldId == f10.Id && x.LanguageId == language.Id) ?? await db.FieldTranslations.FirstAsync(x => x.FieldId == f10.Id);
                    FieldType fieldType = await db.FieldTypes.SingleAsync(x => x.Id == f10.FieldTypeId);
                    fd10 = new FieldDto(f10.Id, translation.Text, translation.Description, (int)f10.FieldTypeId, f10.FieldType.Type, (int)f10.CheckListId);
                }
                #endregion

                #region loadtags
                List<Tagging> matches = checkList.Taggings.ToList();
                List<KeyValuePair<int, string>> checkListTags = new List<KeyValuePair<int, string>>();
                foreach (Tagging tagging in matches)
                {
                    KeyValuePair<int, string> kvp = new KeyValuePair<int, string>((int)tagging.TagId, tagging.Tag.Name);
                    checkListTags.Add(kvp);
                }
                #endregion

                Template_Dto templateDto = new Template_Dto()
                {
                    Id = checkList.Id,
                    CreatedAt = checkList.CreatedAt,
                    UpdatedAt = checkList.UpdatedAt,
                    DeployedSites = sites,
                    Description = checkListTranslation.Description,
                    DisplayIndex = checkList.DisplayIndex,
                    DocxExportEnabled = checkList.DocxExportEnabled,
                    ExcelExportEnabled = checkList.ExcelExportEnabled,
                    JasperExportEnabled = checkList.JasperExportEnabled,
                    Field1 = fd1,
                    Field2 = fd2,
                    Field3 = fd3,
                    Field4 = fd4,
                    Field5 = fd5,
                    Field6 = fd6,
                    Field7 = fd7,
                    Field8 = fd8,
                    Field9 = fd9,
                    Field10 = fd10,
                    Label = checkListTranslation.Text,
                    Tags = checkListTags,
                    HasCases = hasCases,
                    Repeated = (int) checkList.Repeated,
                    FolderName = checkList.FolderName,
                    WorkflowState = checkList.WorkflowState,
                    ReportH1 = checkList.ReportH1,
                    ReportH2 = checkList.ReportH2,
                    ReportH3 = checkList.ReportH3,
                    ReportH4 = checkList.ReportH4,
                    ReportH5 = checkList.ReportH5
                };
                return templateDto;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<List<Template_Dto>> TemplateItemReadAll(bool includeRemoved, string siteWorkflowState, string searchKey, bool descendingSort, string sortParameter, List<int> tagIds, TimeZoneInfo timeZoneInfo, Language language)
        {
            string methodName = "SqlController.TemplateItemReadAll";
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(includeRemoved), includeRemoved);
            log.LogVariable(methodName, nameof(searchKey), searchKey);
            log.LogVariable(methodName, nameof(descendingSort), descendingSort);
            log.LogVariable(methodName, nameof(sortParameter), sortParameter);
            log.LogVariable(methodName, nameof(tagIds), tagIds.ToString());
            try
            {
                List<Template_Dto> templateList = new List<Template_Dto>();

                await using var db = GetContext();
                var subQuery = db.CheckLists
                    .Where(x => x.ParentId == null)
                    .Join(db.CheckListTranslations,
                        list => list.Id,
                        translation => translation.CheckListId, (list, translation)
                            => new {list.Id,
                                translation.Text,
                                translation.Description,
                                list.CreatedAt,
                                list.WorkflowState,
                                list.FolderName,
                                list.Repeated,
                                list.DocxExportEnabled,
                                list.ExcelExportEnabled,
                                list.JasperExportEnabled,
                                list.DisplayIndex,
                                list.ReportH1,
                                list.ReportH2,
                                list.ReportH3,
                                list.ReportH4,
                                list.ReportH5,
                                translation.LanguageId
                            }).Where(x => x.LanguageId == language.Id);

                //Language language = await db.Languages.SingleAsync(x => x.Name == "Danish");

                var all = subQuery.ToList();

                if (!includeRemoved)
                    subQuery = subQuery.Where(x =>
                        x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                if (!string.IsNullOrEmpty(searchKey))
                {
                    subQuery = subQuery.Where(x => x.Text.Contains(searchKey)
                                || x.Description.Contains(searchKey));
                }

                if (tagIds.Count > 0)
                {
                    List<int?> checkListIds = db.Taggings
                        .Where(x => tagIds.Contains((int)x.TagId)
                                    && x.WorkflowState != Constants.Constants.WorkflowStates.Removed)
                        .Select(x => x.CheckListId).ToList();
                    subQuery = subQuery.Where(x => checkListIds.Contains(x.Id));
                }

                switch (sortParameter)
                {
                    case Constants.Constants.eFormSortParameters.Label:
                        if (descendingSort)
                            subQuery = subQuery.OrderByDescending(x => x.Text);
                        else
                            subQuery = subQuery.OrderBy(x => x.Text);
                        break;
                    case Constants.Constants.eFormSortParameters.Description:
                        if (descendingSort)
                            subQuery = subQuery.OrderByDescending(x => x.Description);
                        else
                            subQuery = subQuery.OrderBy(x => x.Description);
                        break;
                    case Constants.Constants.eFormSortParameters.CreatedAt:
                        if (descendingSort)
                            subQuery = subQuery.OrderByDescending(x => x.CreatedAt);
                        else
                            subQuery = subQuery.OrderBy(x => x.CreatedAt);
                        break;
                    case Constants.Constants.eFormSortParameters.Tags:
                        if (descendingSort)
                            subQuery = subQuery.OrderByDescending(x => x.CreatedAt);
                        else
                            subQuery = subQuery.OrderBy(x => x.CreatedAt);
                        break;
                    default:
                        if (descendingSort)
                            subQuery = subQuery.OrderByDescending(x => x.Id);
                        else
                            subQuery = subQuery.OrderBy(x => x.Id);
                        break;
                }

                var matches = await subQuery.ToListAsync().ConfigureAwait(false);

                foreach (var checkList in matches)
                {
                    List<SiteNameDto> sites = new List<SiteNameDto>();
                    List<CheckListSite> checkListSites = null;
                    int? folderId = null;

                    if (siteWorkflowState == Constants.Constants.WorkflowStates.Removed)
                    {
                        checkListSites = await db.CheckListSites.Where(x =>
                            x.WorkflowState == Constants.Constants.WorkflowStates.Removed
                            && x.CheckListId == checkList.Id).ToListAsync();
                    }
                    else
                    {
                        checkListSites = await db.CheckListSites.Where(x =>
                            x.WorkflowState != Constants.Constants.WorkflowStates.Removed
                            && x.CheckListId == checkList.Id).ToListAsync();
                    }

                    foreach (CheckListSite checkListSite in checkListSites)
                    {
                        try
                        {
                            Site dbSite = await db.Sites.SingleAsync(x => x.Id == checkListSite.SiteId);
                            SiteNameDto site = new SiteNameDto((int)dbSite.MicrotingUid, dbSite.Name, dbSite.CreatedAt, dbSite.UpdatedAt);
                            sites.Add(site);
                            folderId = checkListSite.FolderId;
                        } catch (Exception innerEx)
                        {
                            Site dbSite = await db.Sites.SingleAsync(x => x.Id == checkListSite.SiteId);
                            log.LogException(methodName, "could not add the site to the sites for site.Id : " + dbSite.Id.ToString() + " and got exception : " + innerEx.Message, innerEx);
                            throw new Exception("Error adding site to sites for site.Id : " + dbSite.Id.ToString(), innerEx);
                        }
                    }

                    var cases = db.Cases.Where(x => x.CheckListId == checkList.Id).AsQueryable();
                    bool hasCases = cases.Count() != 0;
                    if (hasCases)
                    {
                        var result = await cases.OrderBy(x => x.Id).LastAsync();
                        folderId ??= result.FolderId;
                    }

                    #region loadtags
                    List<Tagging> taggingMatches = await db.Taggings.Where(x =>
                        x.CheckListId == checkList.Id
                        && x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToListAsync();
                    List<KeyValuePair<int, string>> checkListTags = new List<KeyValuePair<int, string>>();
                    foreach (Tagging tagging in taggingMatches)
                    {
                        KeyValuePair<int, string> kvp = new KeyValuePair<int, string>((int)tagging.TagId, db.Tags.Single(x => x.Id == tagging.TagId).Name);
                        checkListTags.Add(kvp);
                    }
                    #endregion
                    try
                    {
                        Template_Dto templateDto = new Template_Dto()
                        {
                            Id = checkList.Id,
                            CreatedAt = TimeZoneInfo.ConvertTimeFromUtc((DateTime)checkList.CreatedAt, timeZoneInfo),
                            Label = checkList.Text,
                            Description = checkList.Description,
                            Repeated = (int)checkList.Repeated,
                            FolderName = checkList.FolderName,
                            WorkflowState = checkList.WorkflowState,
                            DeployedSites = sites,
                            HasCases = hasCases,
                            DisplayIndex = checkList.DisplayIndex,
                            Tags = checkListTags,
                            FolderId = folderId,
                            DocxExportEnabled =  checkList.DocxExportEnabled,
                            JasperExportEnabled = checkList.JasperExportEnabled,
                            ExcelExportEnabled = checkList.ExcelExportEnabled,
                            ReportH1 = checkList.ReportH1,
                            ReportH2 = checkList.ReportH2,
                            ReportH3 = checkList.ReportH3,
                            ReportH4 = checkList.ReportH4,
                            ReportH5 = checkList.ReportH5
                        };
                        templateList.Add(templateDto);
                    }
                    catch (Exception innerEx)
                    {
                        log.LogException(methodName, "could not add the templateDto to the templateList for templateId : " + checkList.Id.ToString() + " and got exception : " + innerEx.Message, innerEx);
                        throw new Exception("Error adding template to templateList for templateId : " + checkList.Id.ToString(), innerEx);
                    }
                }
                return templateList;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<List<FieldDto>> TemplateFieldReadAll(int templateId)
        {
            string methodName = "SqlController.TemplateFieldReadAll";
            try
            {
                await using var db = GetContext();
                Language language = await db.Languages.SingleAsync(x => x.Name == "Danish");
                MainElement mainElement = await ReadeForm(templateId, language);
                List<FieldDto> fieldLst = new List<FieldDto>();

                foreach (DataItem dataItem in mainElement.DataItemGetAll())
                {
                    Data.Entities.Field field = await db.Fields.SingleAsync(x => x.Id == dataItem.Id);
                    FieldDto fieldDto = new FieldDto(field.Id, field.Label, field.Description, (int)field.FieldTypeId, db.FieldTypes.Single(x => x.Id == field.FieldTypeId).Type, (int)field.CheckListId);
                    if (field.ParentFieldId != null)
                    {
                        fieldDto.ParentName = db.Fields.Where(x => x.Id == field.ParentFieldId).First().Label;
                    }
                    else
                    {
                        fieldDto.ParentName = db.CheckLists.Single(x => x.Id == field.CheckListId).Label;
                    }
                    fieldLst.Add(fieldDto);
                }

                return fieldLst;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<bool> TemplateDisplayIndexChange(int templateId, int newDisplayIndex)
        {
            string methodName = "SqlController.TemplateDisplayIndexChange";
            try
            {
                await using var db = GetContext();
                CheckList checkList = await db.CheckLists.SingleOrDefaultAsync(x => x.Id == templateId);

                if (checkList == null)
                    return false;

                checkList.DisplayIndex = newDisplayIndex;

                await checkList.Update(db).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<bool> TemplateUpdateFieldIdsForColumns(int templateId, int? fieldId1, int? fieldId2, int? fieldId3, int? fieldId4, int? fieldId5, int? fieldId6, int? fieldId7, int? fieldId8, int? fieldId9, int? fieldId10)
        {
            string methodName = "SqlController.TemplateUpdateFieldIdsForColumns";
            try
            {
                await using var db = GetContext();
                CheckList checkList = await db.CheckLists.SingleOrDefaultAsync(x => x.Id == templateId);

                if (checkList == null)
                    return false;

                checkList.Field1 = fieldId1;
                checkList.Field2 = fieldId2;
                checkList.Field3 = fieldId3;
                checkList.Field4 = fieldId4;
                checkList.Field5 = fieldId5;
                checkList.Field6 = fieldId6;
                checkList.Field7 = fieldId7;
                checkList.Field8 = fieldId8;
                checkList.Field9 = fieldId9;
                checkList.Field10 = fieldId10;

                await checkList.Update(db).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes Template from DB with given templateId
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> TemplateDelete(int templateId)
        {
            string methodName = "SqlController.TemplateDelete";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                CheckList checkList = await db.CheckLists.SingleAsync(x => x.Id == templateId);

                if (checkList != null)
                {
                    await checkList.Delete((db));

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<bool> TemplateSetTags(int templateId, List<int> tagIds)
        {
            string methodName = "SqlController.TemplateSetTags";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                CheckList checkList = await db.CheckLists.SingleAsync(x => x.Id == templateId);

                if (checkList != null)
                {
                    // Delete all not wanted taggings first
                    List<Tagging> clTaggings = await db.Taggings.Where(x => !tagIds.Contains((int)x.TagId) && x.CheckListId == checkList.Id).ToListAsync();
//                        int index = 0;
                    foreach (Tagging tagging in clTaggings)
                    {
                        Tagging currentTagging = await db.Taggings.SingleAsync(x => x.Id == tagging.Id);
                        if (currentTagging != null)
                        {
                            await currentTagging.Delete(db);
                        }
                    }

                    // set all new taggings
                    foreach (int Id in tagIds)
                    {
                        Tag tag = await db.Tags.SingleAsync(x => x.Id == Id);
                        if (tag != null)
                        {
                            Tagging currentTagging = await db.Taggings.SingleOrDefaultAsync(x => x.TagId == tag.Id && x.CheckListId == templateId);

                            if (currentTagging == null)
                            {
                                Tagging tagging = new Tagging();
                                tagging.CheckListId = templateId;
                                tagging.TagId = tag.Id;

                                await tagging.Create(db).ConfigureAwait(false);
                            } else {
                                if (currentTagging.WorkflowState != Constants.Constants.WorkflowStates.Created)
                                {
                                    currentTagging.WorkflowState = Constants.Constants.WorkflowStates.Created;
                                    await currentTagging.Update(db).ConfigureAwait(false);
                                }
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        #endregion

        #region public (pre)case

        //TODO
        public async Task CheckListSitesCreate(int checkListId, int siteUId, int microtingUId, int? folderId)
        {
            string methodName = "SqlController.CheckListSitesCreate";
            try
            {
                await using var db = GetContext();
                int siteId = db.Sites.SingleAsync(x => x.MicrotingUid == siteUId).GetAwaiter().GetResult().Id;

                CheckListSite cLs = new CheckListSite
                {
                    CheckListId = checkListId,
                    LastCheckId = 0,
                    MicrotingUid = microtingUId,
                    SiteId = siteId,
                    FolderId = folderId
                };
                await cLs.Create(db).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
#pragma warning disable 1998
        public async Task<List<int>> CheckListSitesRead(int templateId, int microtingUid, string workflowState)
        {
            string methodName = "SqlController.CheckListSitesRead";
            try
            {
                await using var db = GetContext();
                Site site = await db.Sites.SingleAsync(x => x.MicrotingUid == microtingUid);
                IQueryable<CheckListSite> sub_query = db.CheckListSites.Where(x => x.SiteId == site.Id && x.CheckListId == templateId);
                if (workflowState == Constants.Constants.WorkflowStates.NotRemoved)
                    sub_query = sub_query.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed);

                return sub_query.Select(x => x.MicrotingUid).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception( methodName + " failed", ex);
            }
        }
#pragma warning restore 1998


        /// <summary>
        /// Creates a case in DB with given parameters siteUId, microtingUId, microtingCheckId, caseUid, custom and createdAt
        /// After creation the method returns the ID of the newly created case
        /// </summary>
        /// <param name="checkListId"></param>
        /// <param name="siteUId"></param>
        /// <param name="microtingUId"></param>
        /// <param name="microtingCheckId"></param>
        /// <param name="caseUId"></param>
        /// <param name="custom"></param>
        /// <param name="createdAt"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> CaseCreate(int checkListId, int siteUId, int? microtingUId, int? microtingCheckId, string caseUId, string custom, DateTime createdAt, int? folderId)
        {
            string methodName = "SqlController.CaseCreate";
            log.LogStandard(methodName, "called");
            try
            {
                await using var db = GetContext();
                string caseType = db.CheckLists.SingleAsync(x => x.Id == checkListId).GetAwaiter().GetResult().CaseType;
                log.LogStandard(methodName, $"caseType is {caseType}");
                int siteId = db.Sites.SingleAsync(x => x.MicrotingUid == siteUId).GetAwaiter().GetResult().Id;
                log.LogStandard(methodName, $"siteId is {siteId}");

                Case aCase = null;
                // Lets see if we have an existing case with the same parameters in the db first.
                // This is to handle none gracefull shutdowns.
                aCase = await db.Cases.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == microtingCheckId);
                log.LogStandard(methodName, $"aCase found based on MicrotingUid == {microtingUId} and MicrotingCheckUid == {microtingCheckId}");

                if (aCase == null)
                {
                    aCase = new Case
                    {
                        Status = 66,
                        Type = caseType,
                        CheckListId = checkListId,
                        MicrotingUid = microtingUId,
                        MicrotingCheckUid = microtingCheckId,
                        CaseUid = caseUId,
                        SiteId = siteId,
                        Custom = custom,
                        FolderId = folderId
                    };

                    await aCase.Create(db).ConfigureAwait(false);
                }
                else
                {
                    aCase.Status = 66;
                    aCase.Type = caseType;
                    aCase.CheckListId = checkListId;
                    aCase.MicrotingUid = microtingUId;
                    aCase.MicrotingCheckUid = microtingCheckId;
                    aCase.CaseUid = caseUId;
                    aCase.SiteId = siteId;
                    aCase.Custom = custom;
                    aCase.FolderId = folderId;
                    await aCase.Update(db).ConfigureAwait(false);
                }
                log.LogStandard(methodName, $"aCase is created in db");

                return aCase.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads case from DB with given microtingUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int?> CaseReadLastCheckIdByMicrotingUId(int microtingUId)
        {
            string methodName = "SqlController.CaseReadLastCheckIdByMicrotingUId";
            try
            {
                await using var db = GetContext();
                CheckListSite match = await db.CheckListSites.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUId);
                return match?.LastCheckId;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task CaseUpdateRetrieved(int microtingUId)
        {
            string methodName = "SqlController.CaseUpdateRetrieved";
            try
            {
                await using var db = GetContext();
                Case match = await db.Cases.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUId);

                if (match != null)
                {
                    match.Status = 77;
                    await match.Update(db).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task CaseUpdateCompleted(int microtingUId, int microtingCheckId, DateTime doneAt, int workerMicrotingUId, int unitMicrotingUid)
        {
            string methodName = "SqlController.CaseUpdateCompleted";
            try
            {
                await using var db = GetContext();
                Case caseStd = await db.Cases.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == microtingCheckId);

                if (caseStd == null)
                    caseStd = await db.Cases.SingleAsync(x => x.MicrotingUid == microtingUId);

                int userId = db.Workers.SingleAsync(x => x.MicrotingUid == workerMicrotingUId).GetAwaiter().GetResult().Id;
                int unitId = db.Units.SingleAsync(x => x.MicrotingUid == unitMicrotingUid).GetAwaiter().GetResult().Id;

                caseStd.Status = 100;
                caseStd.DoneAt = doneAt;
                caseStd.WorkerId = userId;
                caseStd.UnitId = unitId;
                caseStd.MicrotingCheckUid = microtingCheckId;
                #region - update "check_list_sites" if needed
                CheckListSite match = await db.CheckListSites.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUId);
                if (match != null)
                {
                    match.LastCheckId = microtingCheckId;
                    await match.Update(db).ConfigureAwait(false);
                }
                #endregion

                await caseStd.Update(db).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task CaseRetract(int microtingUId, int microtingCheckId)
        {
            string methodName = "SqlController.CaseRetract";
            try
            {
                await using var db = GetContext();
                Case match = await db.Cases.SingleAsync(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == microtingCheckId);

                match.WorkflowState = Constants.Constants.WorkflowStates.Retracted;
                await match.Update(db).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes Case from DB with given microtingUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CaseDelete(int microtingUId)
        {
            string methodName = "SqlController.CaseDelete";
            try
            {
                await using var db = GetContext();
                List<Case> matches = await db.Cases.Where(x => x.MicrotingUid == microtingUId).ToListAsync();
                if (matches.Count == 1)
                {
                    Case aCase = matches.First();
                    if (aCase.WorkflowState != Constants.Constants.WorkflowStates.Retracted && aCase.WorkflowState != Constants.Constants.WorkflowStates.Removed)
                    {
                        await aCase.Delete((db));
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<bool> CaseDeleteResult(int caseId)
        {
            string methodName = "SqlController.CaseDeleteResult";
            try
            {
                await using var db = GetContext();
                Case aCase = await db.Cases.SingleOrDefaultAsync(x => x.Id == caseId);

                if (aCase != null)
                {
                    await aCase.Delete(db);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task CaseDeleteReversed(int microtingUId)
        {
            await using var db = GetContext();
            List<CheckListSite> checkListSites = await db.CheckListSites.Where(x => x.MicrotingUid == microtingUId).ToListAsync();

            if (!checkListSites.Any())
            {
                return;
            }
            if (checkListSites.Count == 1)
            {
                await checkListSites.First().Delete(db);
            }
            else
            {
                throw new Exception("There is more than one instance.");
            }
        }
        #endregion

        #region public "reply"
        #region check

        //TODO
        public async Task<List<int>> ChecksCreate(Response response, string xmlString, int xmlIndex)
        {
            string methodName = "SqlController.ChecksCreate";
            List<int> uploadedDataIds = new List<int>();
            try
            {
                await using var db = GetContext();
                //int elementId;
                int userUId = int.Parse(response.Checks[xmlIndex].WorkerId);
                int userId = db.Workers.SingleAsync(x => x.MicrotingUid == userUId).GetAwaiter().GetResult().Id;
                List<string> elements = t.LocateList(xmlString, "<ElementList>", "</ElementList>");
                List<FieldDto> eFormFieldList = null;
                Case responseCase = null;
                List<int?> caseFields = new List<int?>();
                List<int> fieldTypeIds = db.FieldTypes.Where(x => x.Type == Constants.Constants.FieldTypes.Picture || x.Type == Constants.Constants.FieldTypes.Signature || x.Type == Constants.Constants.FieldTypes.Audio).Select(x => x.Id).ToList();

                try //if a reversed case, case needs to be created
                {
                    CheckListSite cLS = await db.CheckListSites.SingleAsync(x => x.MicrotingUid == int.Parse(response.Value));
                    int caseId = await CaseCreate((int)cLS.CheckListId, (int)db.Sites.Single(x => x.Id == cLS.SiteId).MicrotingUid, int.Parse(response.Value), response.Checks[xmlIndex].Id, "ReversedCase", "", DateTime.UtcNow, cLS.FolderId);
                    responseCase = await db.Cases.SingleAsync(x => x.Id == caseId);
                }
                catch //already created case Id retrived
                {
                    responseCase = await db.Cases.SingleAsync(x => x.MicrotingUid == int.Parse(response.Value));
                }

                CheckList cl = db.CheckLists.Single(x => x.Id == responseCase.CheckListId);

                caseFields.Add(cl.Field1);
                caseFields.Add(cl.Field2);
                caseFields.Add(cl.Field3);
                caseFields.Add(cl.Field4);
                caseFields.Add(cl.Field5);
                caseFields.Add(cl.Field6);
                caseFields.Add(cl.Field7);
                caseFields.Add(cl.Field8);
                caseFields.Add(cl.Field9);
                caseFields.Add(cl.Field10);
                //cl.field_1

                eFormFieldList = await TemplateFieldReadAll((int)responseCase.CheckListId);

                foreach (string elementStr in elements)
                {
                    #region foreach element

                    int checkListId = int.Parse(t.Locate(elementStr, "<Id>", "</"));
                    int caseId = responseCase.Id;

                    CheckListValue clv = null;
                    clv = await db.CheckListValues.SingleOrDefaultAsync(x => x.CheckListId == checkListId && x.CaseId == caseId);

                    if (clv == null)
                    {
                        clv = new CheckListValue
                        {
                            CheckListId = int.Parse(t.Locate(elementStr, "<Id>", "</")),
                            CaseId = responseCase.Id,
                            Status = t.Locate(elementStr, "<Status>", "</"),
                            UserId = userId
                        };

                        await clv.Create(db).ConfigureAwait(false);
                    }

                    #region foreach (string dataItemStr in dataItems)
                    //elementId = clv.Id;
                    List<string> dataItems = t.LocateList(elementStr, "<DataItem>", "</DataItem>");

                    if (dataItems != null)
                    {
                        foreach (string dataItemStr in dataItems)
                        {

                            int field_id = int.Parse(t.Locate(dataItemStr, "<Id>", "</"));

                            Data.Entities.Field f = await db.Fields.SingleAsync(x => x.Id == field_id);
                            FieldType fieldType = db.FieldTypes.Single(x => x.Id == f.FieldTypeId);
                            FieldValue fieldV = null;


                            if (!fieldTypeIds.Contains((int)f.FieldTypeId))
                            {
                                fieldV = await db.FieldValues.SingleOrDefaultAsync(x => x.FieldId == field_id && x.CaseId == caseId && x.CheckListId == checkListId && x.WorkerId == userId);
                            }

                            if (fieldV == null)
                            {
                                fieldV = new FieldValue();

                                //= new field_values();

                                #region if contains a file
                                string urlXml = t.Locate(dataItemStr, "<URL>", "</URL>");
                                if (urlXml != "" && urlXml != "none")
                                {
                                    UploadedData dU = null;
                                    string fileLocation = t.Locate(dataItemStr, "<URL>", "</");
                                    dU = new UploadedData
                                    {
                                        Extension = t.Locate(dataItemStr, "<Extension>", "</"),
                                        UploaderId = userId,
                                        UploaderType = Constants.Constants.UploaderTypes.System,
                                        Local = 0,
                                        FileLocation = fileLocation
                                    };
                                    await dU.Create(db).ConfigureAwait(false);
                                    fieldV.UploadedDataId = dU.Id;
                                    uploadedDataIds.Add(dU.Id);

                                }
                                #endregion
                                #region fieldV.value = t.Locate(xml, "<Value>", "</");
                                string extractedValue = t.Locate(dataItemStr, "<Value>", "</");

                                if (extractedValue.Length > 8)
                                {
                                    if (extractedValue.StartsWith(@"<![CDATA["))
                                    {
                                        extractedValue = extractedValue.AsSpan(9).ToString();
                                        //extractedValue = extractedValue.Substring(9);
                                        extractedValue = extractedValue.AsSpan(0, extractedValue.Length - 3).ToString();
                                        //extractedValue = extractedValue.Substring(0, extractedValue.Length - 3);
                                    }
                                }

                                if (fieldType.Type == Constants.Constants.FieldTypes.Number ||
                                    fieldType.Type == Constants.Constants.FieldTypes.NumberStepper)
                                {
//                                        extractedValue = extractedValue.Replace(",", "|"); // commented as of 8. oct. 2019
                                    extractedValue = extractedValue.Replace(".", ",");
                                }

                                fieldV.Value = extractedValue;
                                Data.Entities.Field field = await db.Fields.SingleOrDefaultAsync(x => x.Id == field_id);
                                fieldType = await db.FieldTypes.SingleAsync(x => x.Id == field.FieldTypeId);
                                if (fieldType.Type == Constants.Constants.FieldTypes.EntitySearch
                                    || fieldType.Type == Constants.Constants.FieldTypes.EntitySelect)
                                {
                                    if (!string.IsNullOrEmpty(extractedValue) && extractedValue != "null")
                                    {
                                        int id = EntityItemRead(extractedValue).GetAwaiter().GetResult().Id;
                                        fieldV.Value = id.ToString();
                                    }
                                }

                                #endregion
                                //geo
                                fieldV.Latitude = t.Locate(dataItemStr, "<Latitude>", "</");
                                fieldV.Longitude = t.Locate(dataItemStr, "<Longitude>", "</");
                                fieldV.Altitude = t.Locate(dataItemStr, "<Altitude>", "</");
                                fieldV.Heading = t.Locate(dataItemStr, "<Heading>", "</");
                                fieldV.Accuracy = t.Locate(dataItemStr, "<Accuracy>", "</");
                                fieldV.Date = t.Date(t.Locate(dataItemStr, "<Date>", "</"));
                                fieldV.CaseId = responseCase.Id;
                                fieldV.FieldId = field_id;
                                fieldV.WorkerId = userId;
                                fieldV.CheckListId = clv.CheckListId;
                                fieldV.DoneAt = t.Date(response.Checks[xmlIndex].Date);

                                await fieldV.Create(db).ConfigureAwait(false);

                                #region update case field_values
                                if (caseFields.Contains(fieldV.FieldId))
                                {
                                    field = await db.Fields.FirstAsync(x => x.Id == fieldV.FieldId);
                                    fieldType = await db.FieldTypes.SingleAsync(x => x.Id == field.FieldTypeId);
                                    string newValue = fieldV.Value;

                                    if (fieldType.Type == Constants.Constants.FieldTypes.EntitySearch
                                        || fieldType.Type == Constants.Constants.FieldTypes.EntitySelect)
                                    {
                                        try
                                        {
                                            if (fieldV.Value != "" || fieldV.Value != null)
                                            {
                                                int Id = int.Parse(fieldV.Value);
                                                EntityItem match = await db.EntityItems.SingleOrDefaultAsync(x => x.Id == Id);

                                                if (match != null)
                                                {
                                                    newValue = match.Name;
                                                }

                                            }
                                        }
                                        catch { }
                                    }

                                    if (fieldType.Type == "SingleSelect")
                                    {
                                        //string key = ;
                                        //string fullKey = t.Locate(fieldV.Field.KeyValuePairList, $"<" + fieldV.Value + ">", "</" + fieldV.Value + ">");
                                        newValue = t.Locate(t.Locate(field.KeyValuePairList,
                                            $"<{fieldV.Value}>", "</" + fieldV.Value + ">"), "<key>", "</key>");
                                    }

                                    if (fieldType.Type == "MultiSelect")
                                    {
                                        newValue = "";

                                        //string keys = ;
                                        List<string> keyLst = fieldV.Value.Split('|').ToList();

                                        foreach (string key in keyLst)
                                        {
                                            string fullKey = t.Locate(fieldV.Field.KeyValuePairList, $"<{key}>",
                                                $"</{key}>");
                                            if (newValue != "")
                                            {
                                                newValue += "\n" + t.Locate(fullKey, "<key>", "</key>");
                                            }
                                            else
                                            {
                                                newValue += t.Locate(fullKey, "<key>", "</key>");
                                            }
                                        }
                                    }


                                    int i = caseFields.IndexOf(fieldV.FieldId);
                                    switch (i)
                                    {
                                        case 0:
                                            responseCase.FieldValue1 = newValue;
                                            break;
                                        case 1:
                                            responseCase.FieldValue2 = newValue;
                                            break;
                                        case 2:
                                            responseCase.FieldValue3 = newValue;
                                            break;
                                        case 3:
                                            responseCase.FieldValue4 = newValue;
                                            break;
                                        case 4:
                                            responseCase.FieldValue5 = newValue;
                                            break;
                                        case 5:
                                            responseCase.FieldValue6 = newValue;
                                            break;
                                        case 6:
                                            responseCase.FieldValue7 = newValue;
                                            break;
                                        case 7:
                                            responseCase.FieldValue8 = newValue;
                                            break;
                                        case 8:
                                            responseCase.FieldValue9 = newValue;
                                            break;
                                        case 9:
                                            responseCase.FieldValue10 = newValue;
                                            break;
                                    }
                                    await responseCase.Update(db).ConfigureAwait(false);
                                }

                                #endregion

                                #region remove dataItem duplicate from TemplatDataItemLst
                                int index = 0;
                                foreach (var foo in eFormFieldList)
                                {
                                    if (fieldV.FieldId == foo.Id)
                                    {
                                        eFormFieldList.RemoveAt(index);
                                        break;
                                    }

                                    index++;
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                    #endregion
                }

                #region foreach (var field in TemplatFieldLst)
                // We do this because even thought the user did not fill in information for a given field
                // we need the field_value to be populated.
                foreach (var field in eFormFieldList)
                {
                    //field_values fieldV = new field_values();

                    FieldValue fieldV = null;

                    fieldV = await db.FieldValues.SingleOrDefaultAsync(x => x.FieldId == field.Id && x.CaseId == responseCase.Id && x.CheckListId == field.CheckListId && x.WorkerId == userId);

                    if (fieldV == null)
                    {
                        fieldV = new FieldValue();
                        fieldV.CreatedAt = DateTime.UtcNow;
                        fieldV.UpdatedAt = DateTime.UtcNow;

                        fieldV.Value = null;

                        //geo
                        fieldV.Latitude = null;
                        fieldV.Longitude = null;
                        fieldV.Altitude = null;
                        fieldV.Heading = null;
                        fieldV.Accuracy = null;
                        fieldV.Date = null;
                        //
                        fieldV.WorkflowState = Constants.Constants.WorkflowStates.Created;
                        fieldV.Version = 1;
                        fieldV.CaseId = responseCase.Id;
                        fieldV.FieldId = field.Id;
                        fieldV.WorkerId = userId;
                        fieldV.CheckListId = field.CheckListId;
                        fieldV.DoneAt = t.Date(response.Checks[xmlIndex].Date);

                        await fieldV.Create(db).ConfigureAwait(false);
                    }


                }
                #endregion

                return uploadedDataIds;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates Case Field Value in DB with given caseId
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateCaseFieldValue(int caseId)
        {
            string methodName = "SqlController.UpdateCaseFieldValue";
            try
            {
                await using var db = GetContext();
                Case match = await db.Cases.SingleOrDefaultAsync(x => x.Id == caseId);

                if (match != null)
                {
                    CheckList cl = match.CheckList;
                    FieldValue fv = null;
                    #region field_value and field matching
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field1);
                    if (fv != null)
                    {
                        match.FieldValue1 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field2);
                    if (fv != null)
                    {
                        match.FieldValue2 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field3);
                    if (fv != null)
                    {
                        match.FieldValue3 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field4);
                    if (fv != null)
                    {
                        match.FieldValue4 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field5);
                    if (fv != null)
                    {
                        match.FieldValue5 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field6);
                    if (fv != null)
                    {
                        match.FieldValue6 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field7);
                    if (fv != null)
                    {
                        match.FieldValue7 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field8);
                    if (fv != null)
                    {
                        match.FieldValue8 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field9);
                    if (fv != null)
                    {
                        match.FieldValue9 = fv.Value;
                    }
                    fv = await db.FieldValues.SingleOrDefaultAsync(x => x.CaseId == caseId && x.FieldId == cl.Field10);
                    if (fv != null)
                    {
                        match.FieldValue10 = fv.Value;
                    }

                    await match.Update(db).ConfigureAwait(false);
//                        match.Version += 1;
//                        match.UpdatedAt = DateTime.UtcNow;
//                        //TODO! THIS part need to be redone in some form in EF Core!
//                        //db.cases.AddOrUpdate(match);
//                        db.SaveChanges();
//                        db.case_versions.Add(MapCaseVersions(match));
//                        db.SaveChanges();


                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads check from DB with given microtingUId and checkUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <param name="checkUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ReplyElement> CheckRead(int microtingUId, int checkUId, Language language)
        {
            string methodName = "SqlController.CheckRead";
            try
            {
                await using var db = GetContext();
                var aCase = await db.Cases.AsNoTracking().SingleAsync(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                var mainCheckList = await db.CheckLists.SingleAsync(x => x.Id == aCase.CheckListId);

                CheckListTranslation checkListTranslation =
                    await db.CheckListTranslations.SingleOrDefaultAsync(x =>
                        x.CheckListId == mainCheckList.Id && x.LanguageId == language.Id);
                if (checkListTranslation == null)
                {
                    language = await db.Languages.SingleAsync(x => x.LanguageCode == "da");
                    checkListTranslation =
                        await db.CheckListTranslations.SingleOrDefaultAsync(x =>
                            x.CheckListId == mainCheckList.Id && x.LanguageId == language.Id);
                }

                ReplyElement replyElement = new ReplyElement();
                if (aCase.CheckListId != null) replyElement.Id = (int) aCase.CheckListId;
                replyElement.CaseType = aCase.Type;
                replyElement.Custom = aCase.Custom;
                if (aCase.DoneAt != null) replyElement.DoneAt = (DateTime) aCase.DoneAt;
                if (aCase.WorkerId != null) replyElement.DoneById = (int) aCase.WorkerId;
                replyElement.ElementList = new List<Element>();
                //replyElement.EndDate
                replyElement.FastNavigation = t.Bool(mainCheckList.FastNavigation);
                replyElement.Label = checkListTranslation.Text;
                //replyElement.Language
                replyElement.ManualSync = t.Bool(mainCheckList.ManualSync);
                replyElement.MultiApproval = t.Bool(mainCheckList.MultiApproval);
                if (mainCheckList.Repeated != null) replyElement.Repeated = (int) mainCheckList.Repeated;
                //replyElement.StartDate
                if (aCase.UnitId != null) replyElement.UnitId = (int) aCase.UnitId;
                replyElement.MicrotingUId = (int)aCase.MicrotingCheckUid;
                Site site = await db.Sites.SingleAsync(x => x.Id == aCase.SiteId);
                if (site.MicrotingUid != null) replyElement.SiteMicrotingUuid = (int) site.MicrotingUid;
                replyElement.JasperExportEnabled = mainCheckList.JasperExportEnabled;
                replyElement.DocxExportEnabled = mainCheckList.DocxExportEnabled;

                CheckList checkList = await db.CheckLists.SingleAsync(x => x.Id == aCase.CheckListId);
                foreach (CheckList subCheckList in await db.CheckLists.Where(x =>
                    x.ParentId == checkList.Id).OrderBy(x => x.DisplayIndex).ToListAsync())
                {
                    replyElement.ElementList.Add(await SubChecks(subCheckList.Id, aCase.Id, language));
                }
                return replyElement;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        private async Task<Element> SubChecks(int parentId, int caseId, Language language)
        {
            string methodName = "SqlController.SubChecks";
            try
            {
                await using var db = GetContext();
                var checkList = await db.CheckLists.SingleAsync(x => x.Id == parentId);
                //Element element = new Element();
                if (await db.CheckLists.AnyAsync(x => x.ParentId == checkList.Id))
                {
                    List<Element> elementList = new List<Element>();
                    foreach (CheckList subList in await db.CheckLists.Where(x =>
                        x.ParentId == checkList.Id).OrderBy(x => x.DisplayIndex).ToListAsync())
                    {
                        elementList.Add(await SubChecks(subList.Id, caseId, language));
                    }
                    CheckListTranslation checkListTranslation =
                        await db.CheckListTranslations.SingleAsync(x =>
                            x.CheckListId == checkList.Id && x.LanguageId == language.Id);

                    GroupElement element = new GroupElement(checkList.Id,
                        checkListTranslation.Text,
                        (int)checkList.DisplayIndex,
                        checkListTranslation.Description,
                        t.Bool(checkList.ApprovalEnabled),
                        t.Bool(checkList.ReviewEnabled),
                        t.Bool(checkList.DoneButtonEnabled),
                        t.Bool(checkList.ExtraFieldsEnabled),
                        "",
                        t.Bool(checkList.QuickSyncEnabled),
                        elementList);
                    element.OriginalId = checkList.OriginalId;
                    return element;
                }
                else
                {
                    List<DataItemGroup> dataItemGroupList = new List<DataItemGroup>();
                    List<DataItem> dataItemList = new List<DataItem>();
                    List<Data.Entities.Field> fields = await db.Fields.Where(x =>
                            x.CheckListId == checkList.Id
                            && x.ParentFieldId == null).OrderBy(x => x.DisplayIndex)
                        .ToListAsync();
                    foreach (Data.Entities.Field field in fields)
                    {
                        FieldType fieldType = db.FieldTypes.Single(x => x.Id == field.FieldTypeId);
                        if (fieldType.Type == "FieldGroup")
                        {
                            List<DataItem> dataItemSubList = new List<DataItem>();
                            foreach (Data.Entities.Field subField in await db.Fields.Where(x =>
                                x.ParentFieldId == field.CheckListId).OrderBy(x => x.DisplayIndex).ToListAsync())
                            {
                                Field _field = await DbFieldToField(subField, language);
                                FieldTranslation fieldTranslation = await db.FieldTranslations.SingleAsync(x =>
                                    x.FieldId == _field.Id && x.LanguageId == language.Id);

                                _field.FieldValues = new List<Models.FieldValue>();
                                _field.Label = fieldTranslation.Text;
                                _field.Description = new CDataValue {InderValue = fieldTranslation.Description};
                                foreach (FieldValue fieldValue in await db.FieldValues.Where(x =>
                                    x.FieldId == subField.Id && x.CaseId == caseId).ToListAsync())
                                {
                                    Models.FieldValue answer = await ReadFieldValue(fieldValue, subField, false, language);
                                    _field.FieldValues.Add(answer);
                                }
                                dataItemSubList.Add(_field);
                            }

                            FieldTranslation fieldTranslationFc = await db.FieldTranslations.SingleAsync(x =>
                                x.FieldId == field.Id && x.LanguageId == language.Id);

                            FieldContainer fC = new FieldContainer()
                            {
                                Id = field.Id,
                                Label = fieldTranslationFc.Text,
                                Description = new CDataValue()
                                {
                                    InderValue = fieldTranslationFc.Description
                                },
                                Color = field.Color,
                                DisplayOrder = (int)field.DisplayIndex,
                                Value = field.DefaultValue,
                                DataItemList = dataItemSubList,
                                OriginalId = field.OriginalId,
                                FieldType = "FieldContainer"
                            };
                            dataItemList.Add(fC);
                        }
                        else
                        {
                            Field _field = await DbFieldToField(field, language);

                            FieldTranslation fieldTranslation = await db.FieldTranslations.SingleAsync(x =>
                                x.FieldId == _field.Id && x.LanguageId == language.Id);
                            _field.FieldValues = new List<Models.FieldValue>();

                            _field.Label = fieldTranslation.Text;
                            _field.Description = new CDataValue {InderValue = fieldTranslation.Description};
                            foreach (FieldValue fieldValue in await db.FieldValues.Where(x =>
                                x.FieldId == field.Id && x.CaseId == caseId).ToListAsync())
                            {
                                Models.FieldValue answer = await ReadFieldValue(fieldValue, field, false, language);
                                _field.FieldValues.Add(answer);
                            }
                            dataItemList.Add(_field);
                        }
                    }

                    CheckListTranslation checkListTranslation =
                        await db.CheckListTranslations.SingleAsync(x =>
                            x.CheckListId == checkList.Id && x.LanguageId == language.Id);
                    DataElement dataElement = new DataElement()
                    {
                        Id = checkList.Id,
                        Label = checkListTranslation.Text,
                        DisplayOrder = (int)checkList.DisplayIndex,
                        Description = new CDataValue()
                        {
                            InderValue = checkListTranslation.Description
                        },
                        ApprovalEnabled = t.Bool(checkList.ApprovalEnabled),
                        ReviewEnabled = t.Bool(checkList.ReviewEnabled),
                        DoneButtonEnabled = t.Bool(checkList.DoneButtonEnabled),
                        ExtraFieldsEnabled = t.Bool(checkList.ExtraFieldsEnabled),
                        QuickSyncEnabled = t.Bool(checkList.QuickSyncEnabled),
                        DataItemGroupList = dataItemGroupList,
                        DataItemList = dataItemList,
                        OriginalId = checkList.OriginalId
                    };
                    return new Models.CheckListValue(dataElement, await CheckListValueStatusRead(caseId, checkList.Id));
                }
                //return element;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public async Task<List<FieldValue>> ChecksRead(int microtingUId, int checkUId)
        {
            string methodName = "SqlController.ChecksRead";
            try
            {
                await using var db = GetContext();
                var aCase = await db.Cases.SingleAsync(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                int caseId = aCase.Id;

                List<FieldValue> lst = db.FieldValues.Where(x => x.CaseId == caseId).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception( methodName + " failed", ex);
            }
        }

        public async Task<Data.Entities.Field> FieldReadRaw(int id)
        {
            await using var db = GetContext();
            Data.Entities.Field fieldDb = await db.Fields.SingleOrDefaultAsync(x => x.Id == id);
            return fieldDb;
        }

        public async Task<CheckList> CheckListRead(int id)
        {
            await using var db = GetContext();
            return await db.CheckLists.SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<Field> DbFieldToField(Data.Entities.Field dbField, Language language)
        {
            using var db = GetContext();
            Field field = new Field()
            {
                Label = dbField.Label,
                Description = new CDataValue(),
                FieldType = db.FieldTypes.Single(x => x.Id == dbField.FieldTypeId).Type,
                FieldValue = dbField.DefaultValue,
                EntityGroupId = dbField.EntityGroupId,
                Color = dbField.Color,
                Id = dbField.Id
            };
            field.Description.InderValue = dbField.Description;

            if (field.FieldType == "SingleSelect")
            {
                var singleSelectFieldOptions =
                    await db.FieldOptions.Where(x => x.FieldId == field.Id)
                        .Join(db.FieldOptionTranslations,
                            option => option.Id,
                            translation => translation.FieldOptionId,
                            (option, translation) => new
                            {
                                option.Key,
                                option.DisplayOrder,
                                translation.Text,
                                translation.LanguageId
                            })
                        .Where(x => x.LanguageId == language.Id)
                        .Select(x => new KeyValuePair()
                        {
                            Key = x.Key,
                            Value = x.Text,
                            DisplayOrder = x.DisplayOrder
                        }).ToListAsync();
                field.KeyValuePairList = singleSelectFieldOptions;
            }

            if (field.FieldType == "MultiSelect")
            {
                var multiSelectFieldOptions =
                    await db.FieldOptions.Where(x => x.FieldId == field.Id)
                        .Join(db.FieldOptionTranslations,
                            option => option.Id,
                            translation => translation.FieldOptionId,
                            (option, translation) => new
                            {
                                option.Key,
                                option.DisplayOrder,
                                translation.Text,
                                translation.LanguageId
                            })
                        .Where(x => x.LanguageId == language.Id)
                        .Select(x => new KeyValuePair()
                        {
                            Key = x.Key,
                            Value = x.Text,
                            DisplayOrder = x.DisplayOrder
                        }).ToListAsync();
                field.KeyValuePairList = multiSelectFieldOptions;
            }

            return field;
        }

        public async Task<Field> FieldRead(int id, Language language)
        {

            string methodName = "SqlController.FieldRead";
            try
            {
                await using var db = GetContext();
                Data.Entities.Field dbField = await db.Fields.SingleOrDefaultAsync(x => x.Id == id);

                Field field = await DbFieldToField(dbField, language);
                return field;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        private async Task<Models.FieldValue> ReadFieldValue(FieldValue reply, Data.Entities.Field dbField, bool joinUploadedData, Language language)
        {
            string methodName = "SqlController.ReadFieldValue";
            try
            {
                await using var db = GetContext();
                Models.FieldValue fieldValue = new Models.FieldValue
                {
                    Accuracy = reply.Accuracy,
                    Altitude = reply.Altitude,
                    Color = dbField.Color,
                    Date = reply.Date,
                    FieldId = t.Int(reply.FieldId),
                    FieldType = db.FieldTypes.Single(x => x.Id == dbField.FieldTypeId).Type,
                    DateOfDoing = t.Date(reply.DoneAt),
                    Description = new CDataValue {InderValue = dbField.Description},
                    DisplayOrder = t.Int(dbField.DisplayIndex),
                    Heading = reply.Heading,
                    Id = reply.Id,
                    OriginalId = dbField.OriginalId,
                    Label = dbField.Label,
                    Latitude = reply.Latitude,
                    Longitude = reply.Longitude,
                    Mandatory = t.Bool(dbField.Mandatory),
                    ReadOnly = t.Bool(dbField.ReadOnly)
                };

                #region answer.UploadedDataId = reply.uploaded_data_id;
                if (reply.UploadedDataId.HasValue)
                    if (reply.UploadedDataId > 0)
                    {
                        string locations = "";
                        int uploadedDataId;
                        UploadedData uploadedData;
                        if (joinUploadedData)
                        {
                            List<FieldValue> lst = await db.FieldValues.AsNoTracking().Where(x => x.CaseId == reply.CaseId && x.FieldId == reply.FieldId).ToListAsync();

                            foreach (FieldValue fV in lst)
                            {
                                uploadedDataId = (int)fV.UploadedDataId;

                                uploadedData = await db.UploadedDatas.AsNoTracking().SingleAsync(x => x.Id == uploadedDataId);

                                if (uploadedData.FileName != null)
                                    locations += uploadedData.FileLocation + uploadedData.FileName + Environment.NewLine;
                                else
                                    locations += "File attached, awaiting download" + Environment.NewLine;
                            }
                            fieldValue.UploadedData = locations.TrimEnd();
                        }
                        else
                        {
                            locations = "";
                            uploadedData = await db.UploadedDatas.AsNoTracking().SingleAsync(x => x.Id == reply.UploadedDataId);
                            Models.UploadedData uploadedDataObj = new Models.UploadedData
                            {
                                Checksum = uploadedData.Checksum,
                                Extension = uploadedData.Extension,
                                CurrentFile = uploadedData.CurrentFile,
                                UploaderId = uploadedData.UploaderId,
                                UploaderType = uploadedData.UploaderType,
                                FileLocation = uploadedData.FileLocation,
                                FileName = uploadedData.FileName,
                                Id = uploadedData.Id
                            };
                            fieldValue.UploadedDataObj = uploadedDataObj;
                            fieldValue.UploadedData = "";
                        }

                    }
                #endregion
                fieldValue.Value = reply.Value;
                #region answer.ValueReadable = reply.value 'ish' //and if needed: answer.KeyValuePairList = ReadPairs(...);
                fieldValue.ValueReadable = reply.Value;

                if (fieldValue.FieldType == Constants.Constants.FieldTypes.EntitySearch || fieldValue.FieldType == Constants.Constants.FieldTypes.EntitySelect)
                {
                    try
                    {
                        if (reply.Value != "" || reply.Value != null)
                        {
                            int id = int.Parse(reply.Value);
                            EntityItem match = await db.EntityItems.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

                            if (match != null)
                            {
                                fieldValue.ValueReadable = match.Name;
                                fieldValue.Value = match.Id.ToString();
                                fieldValue.MicrotingUuid = match.MicrotingUid;
                            }
                        }
                    }
                    catch { }
                }

                if (fieldValue.FieldType == Constants.Constants.FieldTypes.SingleSelect)
                {
                    string key = fieldValue.Value;
                    int fieldId = fieldValue.FieldId;
                    FieldOption fieldOption = await db.FieldOptions.SingleOrDefaultAsync(x =>
                        x.FieldId == fieldId && x.Key == key);
                    if (fieldOption != null)
                    {
                        FieldOptionTranslation fieldOptionTranslation =
                            await db.FieldOptionTranslations.SingleAsync(x =>
                                x.FieldOptionId == fieldOption.Id && x.LanguageId == language.Id);
                        fieldValue.ValueReadable = fieldOptionTranslation.Text;
                    }
                    else
                    {
                        fieldValue.ValueReadable = "";
                    }

                    List<FieldOption> fieldOptions = await db.FieldOptions.Where(x => x.FieldId == fieldValue.FieldId).ToListAsync();

                    fieldValue.KeyValuePairList = new List<KeyValuePair>();
                    foreach (FieldOption option in fieldOptions)
                    {
                        var optionTranslation = await
                            db.FieldOptionTranslations.SingleAsync(x => x.FieldOptionId == option.Id && x.LanguageId == language.Id);
                        KeyValuePair keyValuePair = new KeyValuePair(option.Key, optionTranslation.Text, false,
                            option.DisplayOrder);
                        fieldValue.KeyValuePairList.Add(keyValuePair);
                    }
                }

                if (fieldValue.FieldType == Constants.Constants.FieldTypes.MultiSelect)
                {
                    fieldValue.ValueReadable = "";

                    string keys = fieldValue.Value;
                    List<string> keyLst = keys.Split('|').ToList();
                    int fieldId = fieldValue.FieldId;
                    fieldValue.ValueReadable = "";
                    foreach (string key in keyLst)
                    {
                        FieldOption fieldOption = await db.FieldOptions.SingleOrDefaultAsync(x =>
                            x.FieldId == fieldId && x.Key.ToString() == key);
                        if (fieldOption != null)
                        {
                            FieldOptionTranslation fieldOptionTranslation =
                                await db.FieldOptionTranslations.SingleAsync(x =>
                                    x.FieldOptionId == fieldOption.Id && x.LanguageId == language.Id);
                            if (fieldValue.ValueReadable != "")
                            {
                                fieldValue.ValueReadable += '|';
                            }
                            fieldValue.ValueReadable += fieldOptionTranslation.Text;
                        }
                    }

                    List<FieldOption> fieldOptions = await db.FieldOptions.Where(x => x.FieldId == fieldValue.FieldId).ToListAsync();
                    fieldValue.KeyValuePairList = new List<KeyValuePair>();
                    foreach (FieldOption option in fieldOptions)
                    {
                        var optionTranslation = await
                            db.FieldOptionTranslations.SingleAsync(x => x.FieldOptionId == option.Id && x.LanguageId == language.Id);
                        KeyValuePair keyValuePair = new KeyValuePair(option.Key, optionTranslation.Text, false,
                            option.DisplayOrder);
                    }
                }

                if (fieldValue.FieldType == Constants.Constants.FieldTypes.Number ||
                    fieldValue.FieldType == Constants.Constants.FieldTypes.NumberStepper)
                {
                    if (reply.Value != null)
                    {
                        fieldValue.ValueReadable = reply.Value.Replace(",", ".");
                        fieldValue.Value = reply.Value.Replace(",", ".");
                    }
                    else
                    {
                        fieldValue.ValueReadable = "";
                        fieldValue.Value = "";
                    }
                }
                #endregion

                return fieldValue;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        // Rename method to something more intuitive!
        public async Task<Models.FieldValue> FieldValueRead(FieldValue reply, bool joinUploadedData, Language language)
        {
            string methodName = "SqlController.FieldValueRead";

            try
            {
                await using var db = GetContext();
                Data.Entities.Field field = await db.Fields.SingleAsync(x => x.Id == reply.FieldId);

                return await ReadFieldValue(reply, field, joinUploadedData, language);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public async Task<List<Models.FieldValue>> FieldValueReadList(int fieldId, int instancesBackInTime, Language language)
        {
            string methodName = "SqlController.FieldValueReadList";
            try
            {
                await using var db = GetContext();
                List<FieldValue> matches = db.FieldValues.Where(x => x.FieldId == fieldId).OrderByDescending(z => z.CreatedAt).ToList();

                if (matches.Count() > instancesBackInTime)
                    matches = matches.GetRange(0, instancesBackInTime);

                List<Models.FieldValue> rtnLst = new List<Models.FieldValue>();

                foreach (var item in matches)
                    rtnLst.Add(await FieldValueRead(item, true, language));

                return rtnLst;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public async Task<List<Models.FieldValue>> FieldValueReadList(int fieldId, List<int> caseIds, Language language)
        {
            string methodName = "SqlController.FieldValueReadList";
            try
            {
                await using var db = GetContext();
                List<FieldValue> matches = db.FieldValues.Where(x => x.FieldId == fieldId && caseIds.Contains(x.Id)).ToList();

                List<Models.FieldValue> rtnLst = new List<Models.FieldValue>();

                foreach (var item in matches)
                    rtnLst.Add(await FieldValueRead(item, true, language));

                return rtnLst;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public async Task<List<Models.FieldValue>> FieldValueReadList(List<int> caseIds, Language language)
        {
            string methodName = "SqlController.FieldValueReadList";
            try
            {
                await using var db = GetContext();
                List<FieldValue> matches = db.FieldValues.Where(x => caseIds.Contains((int)x.CaseId)).ToList();
                List<Models.FieldValue> rtnLst = new List<Models.FieldValue>();

                foreach (var item in matches)
                    rtnLst.Add(await FieldValueRead(item, false, language));

                return rtnLst;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

#pragma warning disable 1998
        public async Task<List<Models.CheckListValue>> CheckListValueReadList(List<int> caseIds)
        {
            string methodName = "SqlController.CheckListValueReadList";
            try
            {
                await using var db = GetContext();
                List<CheckListValue> matches = db.CheckListValues.Where(x => caseIds.Contains((int)x.CaseId)).ToList();
                List<Models.CheckListValue> rtnLst = new List<Models.CheckListValue>();

                foreach (var item in matches)
                {
                    Models.CheckListValue checkListValue = new Models.CheckListValue
                    {
                        Id = item.Id, Label = item.Status
                    };
                    rtnLst.Add(checkListValue);
                }


                return rtnLst;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
#pragma warning restore 1998


        public async Task FieldValueUpdate(int caseId, int fieldValueId, string value)
        {
            string methodName = "SqlController.FieldValueUpdate";
            try
            {
                await using var db = GetContext();
                FieldValue fieldMatch = await db.FieldValues.SingleAsync(x => x.Id == fieldValueId);

                fieldMatch.Value = value;
                await fieldMatch.Update(db).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName +" failed", ex);
            }
        }

        public async Task<List<List<KeyValuePair>>> FieldValueReadAllValues(int fieldId, List<int> caseIds,
            string customPathForUploadedData, Language language)
        {
            return await FieldValueReadAllValues(fieldId, caseIds, customPathForUploadedData, ".", "", language);
        }

        public async Task<List<List<KeyValuePair>>> FieldValueReadAllValues(int fieldId, List<int> caseIds, string customPathForUploadedData, string decimalSeparator, string thousandSeparator, Language language)
        {
            string methodName = "SqlController.FieldValueReadAllValues";
            try
            {
                await using var db = GetContext();
                Data.Entities.Field matchField = await db.Fields.SingleAsync(x => x.Id == fieldId);

                List<FieldValue> matches = db.FieldValues.Where(x => x.FieldId == fieldId && caseIds.Contains((int)x.CaseId)).ToList();

                List<List<KeyValuePair>> rtrnLst = new List<List<KeyValuePair>>();
                List<KeyValuePair> replyLst1 = new List<KeyValuePair>();
                rtrnLst.Add(replyLst1);

                switch (db.FieldTypes.Single(x => x.Id == matchField.FieldTypeId).Type)
                {
                    #region special dataItem
                    case Constants.Constants.FieldTypes.CheckBox:
                        foreach (FieldValue item in matches)
                        {
                            if (item.Value == "checked")
                                replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), "1", false, ""));
                            else
                                replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), "0", false, ""));
                        }
                        break;
                    case Constants.Constants.FieldTypes.Signature:
                    case Constants.Constants.FieldTypes.Picture:
                        int lastCaseId = -1;
                        int lastIndex = -1;
                        foreach (FieldValue item in matches)
                        {
                            if (item.Value != null)
                            {
                                if (lastCaseId == (int)item.CaseId)
                                {

                                    foreach (KeyValuePair kvp in replyLst1)
                                    {
                                        if (kvp.Key == item.CaseId.ToString())
                                        {
                                            if (item.UploadedDataId.HasValue)
                                            {
                                                UploadedData uploadedData =
                                                    db.UploadedDatas.Single(x => x.Id == item.UploadedDataId);
                                                if (customPathForUploadedData != null)
                                                {
                                                    if (kvp.Value.Contains("jpg") || kvp.Value.Contains("jpeg") ||
                                                        kvp.Value.Contains("png"))
                                                        kvp.Value = kvp.Value + "|" + customPathForUploadedData +
                                                                    uploadedData.FileName;
                                                    else
                                                        kvp.Value = customPathForUploadedData +
                                                                    uploadedData.FileName;
                                                }
                                                else
                                                {
                                                    if (kvp.Value.Contains("jpg") || kvp.Value.Contains("jpeg") ||
                                                        kvp.Value.Contains("png"))
                                                        kvp.Value = kvp.Value + "|" +
                                                                    uploadedData.FileLocation +
                                                                    uploadedData.FileName;
                                                    else
                                                        kvp.Value = uploadedData.FileLocation +
                                                                    uploadedData.FileName;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lastIndex++;
                                    if (item.UploadedDataId.HasValue)
                                    {
                                        UploadedData uploadedData =
                                            db.UploadedDatas.Single(x => x.Id == item.UploadedDataId);
                                        if (customPathForUploadedData != null)
                                            replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), customPathForUploadedData + uploadedData.FileName, false, ""));
                                        else
                                            replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), uploadedData.FileLocation + uploadedData.FileName, false, ""));
                                    }
                                    else
                                    {
                                        replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), "", false, ""));
                                    }
                                }
                            }
                            else
                            {
                                lastIndex++;
                                replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), "", false, ""));
                            }
                            lastCaseId = (int)item.CaseId;
                        }
                        break;

                    case Constants.Constants.FieldTypes.SingleSelect:
                    {
                        var singleSelectFieldOptions =
                            await db.FieldOptions.Where(x => x.FieldId == matchField.Id)
                                .Join(db.FieldOptionTranslations,
                                    option => option.Id,
                                    translation => translation.FieldOptionId,
                                    (option, translation) => new
                                    {
                                        option.Key,
                                        option.DisplayOrder,
                                        translation.Text,
                                        translation.LanguageId
                                    })
                                .Where(x => x.LanguageId == language.Id)
                                .Select(x => new KeyValuePair()
                                {
                                    Key = x.Key,
                                    Value = x.Text,
                                    DisplayOrder = x.DisplayOrder
                                }).ToListAsync();
                        //var kVP = PairRead(matchField.KeyValuePairList);

                        foreach (FieldValue item in matches)
                            replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), PairMatch(singleSelectFieldOptions, item.Value), false, ""));
                    }
                        break;

                    case Constants.Constants.FieldTypes.MultiSelect:
                    {
                        var multiSelectFieldOptions =
                            await db.FieldOptions.Where(x => x.FieldId == matchField.Id)
                                .Join(db.FieldOptionTranslations,
                                    option => option.Id,
                                    translation => translation.FieldOptionId,
                                    (option, translation) => new
                                    {
                                        option.Key,
                                        option.DisplayOrder,
                                        translation.Text,
                                        translation.LanguageId
                                    })
                                .Where(x => x.LanguageId == language.Id)
                                .Select(x => new KeyValuePair()
                                {
                                    Key = x.Key,
                                    Value = x.Text,
                                    DisplayOrder = x.DisplayOrder
                                }).ToListAsync();
                        rtrnLst = new List<List<KeyValuePair>>();
                        List<KeyValuePair> replyLst = null;
                        int index = 0;
                        string valueExt = "";

                        foreach (var key in multiSelectFieldOptions)
                        {
                            replyLst = new List<KeyValuePair>();
                            index++;

                            foreach (FieldValue item in matches)
                            {
                                valueExt = "|" + item.Value + "|";
                                if (valueExt.Contains("|" + index.ToString() + "|"))
                                    replyLst.Add(new KeyValuePair(item.CaseId.ToString(), "1", false, ""));
                                else
                                    replyLst.Add(new KeyValuePair(item.CaseId.ToString(), "0", false, ""));
                            }

                            rtrnLst.Add(replyLst);
                        }

                    }
                        break;

                    case Constants.Constants.FieldTypes.EntitySelect:
                    case Constants.Constants.FieldTypes.EntitySearch:
                    {
                        foreach (FieldValue item in matches)
                        {
                            try
                            {
                                if (item.Value != "" || item.Value != null)
                                {
                                    EntityItem match = await db.EntityItems.SingleOrDefaultAsync(x => x.Id.ToString() == item.Value);

                                    if (match != null)
                                    {
                                        replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), match.Name, false, ""));
                                    }
                                    else
                                    {
                                        replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), "", false, ""));
                                    }

                                }
                            }
                            catch
                            {
                                replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), "", false, ""));
                            }
                        }
                    }
                        break;
                    case Constants.Constants.FieldTypes.Number:
                        foreach (FieldValue item in matches)
                        {
                            //string value = item.value.Replace(".", decimalSeparator);
                            if (!string.IsNullOrEmpty(thousandSeparator))
                            {
                                switch (thousandSeparator)
                                {
                                    case ".":
                                    {
                                        replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), String.Format("{0:#.##0.##}", item.Value), false, ""));
                                    }
                                        break;
                                    case ",":
                                    {
                                        replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), String.Format("{0:#,##0.##}", item.Value), false, ""));
                                    }
                                        break;

                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(decimalSeparator))
                                {
                                    string value = "";
                                    if (item.Value != null)
                                    {
                                        value = item.Value.Replace(".", decimalSeparator).Replace(",", decimalSeparator);
                                    }
                                    replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), value, false, ""));
                                }
                                else
                                {
                                    string value = "";
                                    if (item.Value != null)
                                    {
                                        value = item.Value.Replace(".", decimalSeparator).Replace(",", decimalSeparator);
                                    }
                                    replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), value, false, ""));
                                }
                            }
                        }
                        break;
                    #endregion

                    default:
                        foreach (FieldValue item in matches)
                        {
                            replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), item.Value, false, ""));
                        }
                        break;
                }

                return rtrnLst;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public async Task<string> CheckListValueStatusRead(int caseId, int checkListId)
        {
            string methodName = "SqlController.CheckListValueStatusRead";
            try
            {
                await using var db = GetContext();
                CheckListValue clv = await db.CheckListValues.SingleAsync(x => x.CaseId == caseId && x.CheckListId == checkListId);
                return clv.Status;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public async Task CheckListValueStatusUpdate(int caseId, int checkListId, string value)
        {
            string methodName = "SqlController.CheckListValueStatusUpdate";
            try
            {
                await using var db = GetContext();
                CheckListValue match = await db.CheckListValues.SingleAsync(x => x.CaseId == caseId && x.CheckListId == checkListId);

                match.Status = value;
                await match.Update(db).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region notification

        /// <summary>
        /// Creates Notification in DB with given values notificationUId, microtingUId and activity
        /// </summary>
        /// <param name="notificationUId"></param>
        /// <param name="microtingUId"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<Notification> NotificationCreate(string notificationUId, int microtingUId, string activity)
        {
            string methodName = "SqlController.NotificationCreate";

            await using var db = GetContext();
            if (!db.Notifications.Any(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId))
            {
                log.LogStandard(methodName, "SAVING notificationUId : " + notificationUId + " microtingUId : " + microtingUId + " action : " + activity);

                Notification aNote = new Notification
                {
                    NotificationUid = notificationUId, MicrotingUid = microtingUId, Activity = activity
                };

                await aNote.Create(db).ConfigureAwait(false);
                return aNote;
            }
            else
            {
                Notification aNote = await db.Notifications.SingleOrDefaultAsync(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId);
                return aNote;
            }

            //TODO else log warning
        }

        /// <summary>
        /// Returns a Note Data transfer object from DB
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<NoteDto> NotificationReadFirst()
        {
            string methodName = "SqlController.NotificationReadFirst";
            try
            {
                await using var db = GetContext();
                Notification aNoti = await db.Notifications.FirstOrDefaultAsync(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                if (aNoti != null)
                {
                    NoteDto aNote = new NoteDto(aNoti.NotificationUid, aNoti.MicrotingUid, aNoti.Activity);
                    return aNote;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates a Notification in DB with new values of notificationUId, microtingUId, workflowstate, exception and stack trace
        /// </summary>
        /// <param name="notificationUId"></param>
        /// <param name="microtingUId"></param>
        /// <param name="workflowState"></param>
        /// <param name="exception"></param>
        /// <param name="stacktrace"></param>
        /// <exception cref="Exception"></exception>
        public async Task NotificationUpdate(string notificationUId, int microtingUId, string workflowState, string exception, string stacktrace)
        {
            string methodName = "SqlController.NotificationUpdate";
            try
            {
                await using var db = GetContext();
                Notification aNoti = await db.Notifications.SingleAsync(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId);
                aNoti.WorkflowState = workflowState;
//                    aNoti.UpdatedAt = DateTime.UtcNow;
                aNoti.Exception = exception;
                aNoti.Stacktrace = stacktrace;
                await aNoti.Update(db).ConfigureAwait(false);

//                    db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region file

        //TODO
        public async Task<Models.UploadedData> FileRead()
        {
            string methodName = "SqlController.FileRead";
            try
            {
                await using var db = GetContext();
                UploadedData dU = await db.UploadedDatas.FirstOrDefaultAsync(x => x.WorkflowState == Constants.Constants.WorkflowStates.PreCreated);

                if (dU != null)
                {
                    Models.UploadedData ud = new Models.UploadedData
                    {
                        Checksum = dU.Checksum,
                        Extension = dU.Extension,
                        CurrentFile = dU.CurrentFile,
                        UploaderId = dU.UploaderId,
                        UploaderType = dU.UploaderType,
                        FileLocation = dU.FileLocation,
                        FileName = dU.FileName,
                        Id = dU.Id
                    };
                    return ud;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<CaseDto> FileCaseFindMUId(string urlString)
        {
            string methodName = "SqlController.FileCaseFindMUId";
            try
            {
                await using var db = GetContext();
                try
                {
                    UploadedData dU = db.UploadedDatas.Where(x => x.FileLocation == urlString).First();
                    FieldValue fV = await db.FieldValues.SingleAsync(x => x.UploadedDataId == dU.Id);
                    Case aCase = await db.Cases.SingleAsync(x => x.Id == fV.CaseId);

                    return await CaseReadByCaseId(aCase.Id);
                }
                catch
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
//                log.LogCritical(t.GetMethodName("Core"), ex.Message);
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task FileProcessed(string urlString, string checkSum, string fileLocation, string fileName, int Id)
        {
            string methodName = "SqlController.FileProcessed";
            try
            {
                await using var db = GetContext();
                UploadedData uD = await db.UploadedDatas.SingleAsync(x => x.Id == Id);

                uD.Checksum = checkSum;
                uD.FileLocation = fileLocation;
                uD.FileName = fileName;
                uD.Local = 1;
                uD.WorkflowState = Constants.Constants.WorkflowStates.Created;
                await uD.Update(db).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Returns uploaded data object from DB with give Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UploadedData> GetUploadedData(int Id)
        {
            string methodName = "SqlController.GetUploadedData";
            try
            {
                await using var db = GetContext();
                return await db.UploadedDatas.SingleOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<bool> UpdateUploadedData(UploadedData uploadedData)
        {
            string methodName = "SqlController.UpdateUploadedData";
            try
            {
                await using var db = GetContext();
                UploadedData uD = await db.UploadedDatas.SingleAsync(x => x.Id == uploadedData.Id);
                uD.TranscriptionId = uploadedData.TranscriptionId;
                await uD.Update(db).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Returns field_values object from DB with given transcription Id
        /// </summary>
        /// <param name="transcriptionId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<FieldValue> GetFieldValueByTranscriptionId(int transcriptionId)
        {
            string methodName = "SqlController.GetFieldValueByTranscriptionId";
            try
            {
                await using var db = GetContext();
                UploadedData ud = await GetUploaded_DataByTranscriptionId(transcriptionId);
                if (ud != null)
                {
                    return await db.FieldValues.SingleOrDefaultAsync(x => x.UploadedDataId == ud.Id);
                } else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<UploadedData> GetUploaded_DataByTranscriptionId(int transcriptionId)
        {
            string methodName = "SqlController.GetUploaded_DataByTranscriptionId";
            try
            {
                await using var db = GetContext();
                return await db.UploadedDatas.SingleOrDefaultAsync(x => x.TranscriptionId == transcriptionId);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes file from DB with given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> DeleteFile(int Id)
        {
            string methodName = "SqlController.DeleteFile";
            try
            {
                await using var db = GetContext();
                UploadedData uD = await db.UploadedDatas.SingleAsync(x => x.Id == Id);

                await uD.Delete(db);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion
        #endregion

        #region public (post)case

        /// <summary>
        /// Return a case data transfer object from DB with given microtingUId and checkUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <param name="checkUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CaseDto> CaseLookup(int microtingUId, int checkUId)
        {
            string methodName = "SqlController.CaseLookup";
            try
            {
                await using var db = GetContext();
                Case aCase = await db.Cases.SingleAsync(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                return await CaseReadByCaseId(aCase.Id);
            } catch  (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Finds a CaseDto based on microtingUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CaseDto> CaseReadByMUId(int microtingUId)
        {
            string methodName = "SqlController.CaseReadByMUId";
            try
            {
                await using var db = GetContext();
                try
                {
                    Case aCase = await db.Cases.SingleAsync(x => x.MicrotingUid == microtingUId);
                    return await CaseReadByCaseId(aCase.Id);
                }
                catch { }

                try
                {
                    CheckListSite cls = await db.CheckListSites.SingleAsync(x => x.MicrotingUid == microtingUId);
                    CheckList cL = await db.CheckLists.SingleAsync(x => x.Id == cls.CheckListId);

                    #region string stat = aCase.workflow_state ...
                    string stat = "";
                    if (cls.WorkflowState == Constants.Constants.WorkflowStates.Created)
                        stat = Constants.Constants.WorkflowStates.Created;

                    if (cls.WorkflowState == Constants.Constants.WorkflowStates.Removed)
                        stat = "Deleted";
                    #endregion

                    int remoteSiteId = (int)db.Sites.SingleAsync(x => x.Id == (int)cls.SiteId).GetAwaiter().GetResult().MicrotingUid;
                    CaseDto returnCase = new CaseDto()
                    {
                        CaseId = null,
                        Stat = stat,
                        SiteUId = remoteSiteId,
                        CaseType = cL.CaseType,
                        CaseUId = "ReversedCase",
                        MicrotingUId = cls.MicrotingUid,
                        CheckUId = cls.LastCheckId,
                        Custom = null,
                        CheckListId = cL.Id,
                        WorkflowState = null
                    };
                    return returnCase;
                }
                catch(Exception ex1)
                {
                    throw new Exception(methodName + " failed", ex1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads a Case from DB with given case id
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CaseDto> CaseReadByCaseId(int caseId)
        {
            string methodName = "SqlController.CaseReadByCaseId";
            try
            {
                await using var db = GetContext();
                Case aCase = await db.Cases.AsNoTracking().SingleAsync(x => x.Id == caseId);
                CheckList cL = await db.CheckLists.SingleAsync(x => x.Id == aCase.CheckListId);

                #region string stat = aCase.workflow_state ...
                string stat = "";
                if (aCase.WorkflowState == Constants.Constants.WorkflowStates.Created && aCase.Status != 77)
                    stat = "Created";

                if (aCase.WorkflowState == Constants.Constants.WorkflowStates.Created && aCase.Status == 77)
                    stat = "Retrived";

                if (aCase.WorkflowState == Constants.Constants.WorkflowStates.Retracted)
                    stat = "Completed";

                if (aCase.WorkflowState == Constants.Constants.WorkflowStates.Removed)
                    stat = "Deleted";
                #endregion

                int remoteSiteId = (int)db.Sites.SingleAsync(x => x.Id == (int)aCase.SiteId).GetAwaiter().GetResult().MicrotingUid;
                CaseDto caseDto = new CaseDto()
                {
                    CaseId = aCase.Id,
                    Stat = stat,
                    SiteUId = remoteSiteId,
                    CaseType = cL.CaseType,
                    CaseUId = aCase.CaseUid,
                    MicrotingUId = aCase.MicrotingUid,
                    CheckUId = aCase.MicrotingCheckUid,
                    Custom = aCase.Custom,
                    CheckListId = cL.Id,
                    WorkflowState = aCase.WorkflowState

                };
                return caseDto;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Return a list of Case data transfer objects from db with given caseUId
        /// </summary>
        /// <param name="caseUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<CaseDto>> CaseReadByCaseUId(string caseUId)
        {
            string methodName = "SqlController.CaseReadByCaseUId";
            try
            {
                if (caseUId == "")
                    throw new Exception(methodName + " failed. Due invalid input:''. This would return incorrect data");

                if (caseUId == "ReversedCase")
                    throw new Exception(methodName + " failed. Due invalid input:'ReversedCase'. This would return incorrect data");

                await using var db = GetContext();
                List<Case> matches = await db.Cases.Where(x => x.CaseUid == caseUId).ToListAsync();
                List<CaseDto> lstDto = new List<CaseDto>();

                foreach (Case aCase in matches)
                    lstDto.Add(await CaseReadByCaseId(aCase.Id));

                return lstDto;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Returns a cases object from DB with given microtingUId and checkUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <param name="checkUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Case> CaseReadFull(int microtingUId, int checkUId)
        {
            string methodName = "SqlController.CaseReadFull";
            try
            {
                await using var db = GetContext();
                Case match = await db.Cases.AsNoTracking().SingleOrDefaultAsync(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                match.SiteId = db.Sites.SingleOrDefaultAsync(x => x.Id == match.SiteId).GetAwaiter().GetResult().MicrotingUid;

                if (match.UnitId != null)
                    match.UnitId = db.Units.SingleOrDefaultAsync(x => x.Id == match.UnitId).GetAwaiter().GetResult().MicrotingUid;

                if (match.WorkerId != null)
                    match.WorkerId = db.Workers.SingleOrDefaultAsync(x => x.Id == match.WorkerId).GetAwaiter().GetResult().MicrotingUid;
                return match;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Returns an ID of a specific Case from DB with given templateId and workflowstate
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="workflowState"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int?> CaseReadFirstId(int? templateId, string workflowState)
        {
            string methodName = "SqlController.CaseReadFirstId";
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(templateId), templateId);
            log.LogVariable(methodName, nameof(workflowState), workflowState);
            try
            {
                await using var db = GetContext();
                //cases dbCase = null;
                IQueryable<Case> subQuery = db.Cases.Where(x => x.CheckListId == templateId && x.Status == 100);
                switch (workflowState)
                {
                    case Constants.Constants.WorkflowStates.NotRetracted:
                        subQuery = subQuery.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Retracted);
                        break;
                    case Constants.Constants.WorkflowStates.NotRemoved:
                        subQuery = subQuery.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed);
                        break;
                    case Constants.Constants.WorkflowStates.Created:
                        subQuery = subQuery.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created);
                        break;
                    case Constants.Constants.WorkflowStates.Retracted:
                        subQuery = subQuery.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Retracted);
                        break;
                    case Constants.Constants.WorkflowStates.Removed:
                        subQuery = subQuery.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed);
                        break;
                    default:
                        break;
                }

                try
                {
                    var result = await subQuery.FirstOrDefaultAsync().ConfigureAwait(false);
                    if (result != null)
                    {
                        return result.Id;
                    }

                    return null;
                } catch (Exception ex)
                {
                    throw new Exception(methodName + " failed", ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<CaseList> CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState,
            string searchKey, bool descendingSort, string sortParameter, int offSet, int pageSize, TimeZoneInfo timeZoneInfo)
        {

            string methodName = "SqlController.CaseReadAll";
            try
            {
                await using var db = GetContext();
                if (start == null)
                    start = DateTime.MinValue;
                if (end == null)
                    end = DateTime.MaxValue;

                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                List<Case> matches = null;
                IQueryable<Case> sub_query = db.Cases.Where(x => x.DoneAt > start && x.DoneAt < end);
                switch (workflowState)
                {
                    case Constants.Constants.WorkflowStates.NotRetracted:
                        sub_query = sub_query.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Retracted);
                        break;
                    case Constants.Constants.WorkflowStates.NotRemoved:
                        sub_query = sub_query.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed);
                        break;
                    case Constants.Constants.WorkflowStates.Created:
                        sub_query = sub_query.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created);
                        break;
                    case Constants.Constants.WorkflowStates.Retracted:
                        sub_query = sub_query.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Retracted);
                        break;
                    case Constants.Constants.WorkflowStates.Removed:
                        sub_query = sub_query.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed);
                        break;
                    default:
                        break;
                }


                if (templatId != null)
                {
                    sub_query = sub_query.Where(x => x.CheckListId == templatId);
                }

                if (!string.IsNullOrEmpty(searchKey))
                {
                    if (searchKey.Contains("!"))
                    {
                        searchKey = searchKey.ToLower().Replace("!", "");
                        IQueryable<Case> excludeQuery = db.Cases.Where(x => x.DoneAt > start && x.DoneAt < end);
                        excludeQuery = excludeQuery.Where(x => x.FieldValue1.ToLower().Contains(searchKey) ||
                                                               x.FieldValue2.ToLower().Contains(searchKey) ||
                                                               x.FieldValue3.ToLower().Contains(searchKey) ||
                                                               x.FieldValue4.ToLower().Contains(searchKey) ||
                                                               x.FieldValue5.ToLower().Contains(searchKey) ||
                                                               x.FieldValue6.ToLower().Contains(searchKey) ||
                                                               x.FieldValue7.ToLower().Contains(searchKey) ||
                                                               x.FieldValue8.ToLower().Contains(searchKey) ||
                                                               x.FieldValue9.ToLower().Contains(searchKey) ||
                                                               x.FieldValue10.ToLower().Contains(searchKey) ||
                                                               x.Id.ToString().Contains(searchKey) ||
                                                               x.Site.Name.ToLower().Contains(searchKey) ||
                                                               x.Worker.FirstName.ToLower().Contains(searchKey) ||
                                                               x.Worker.LastName.ToLower().Contains(searchKey) ||
                                                               x.DoneAt.ToString().Contains(searchKey));

                        sub_query = sub_query.Except(excludeQuery.ToList());
                    }
                    else
                    {
                        searchKey = searchKey.ToLower();
                        sub_query = sub_query.Where(x => x.FieldValue1.ToLower().Contains(searchKey) ||
                                                         x.FieldValue2.ToLower().Contains(searchKey) ||
                                                         x.FieldValue3.ToLower().Contains(searchKey) ||
                                                         x.FieldValue4.ToLower().Contains(searchKey) ||
                                                         x.FieldValue5.ToLower().Contains(searchKey) ||
                                                         x.FieldValue6.ToLower().Contains(searchKey) ||
                                                         x.FieldValue7.ToLower().Contains(searchKey) ||
                                                         x.FieldValue8.ToLower().Contains(searchKey) ||
                                                         x.FieldValue9.ToLower().Contains(searchKey) ||
                                                         x.FieldValue10.ToLower().Contains(searchKey) ||
                                                         x.Id.ToString().Contains(searchKey) ||
                                                         x.Site.Name.ToLower().Contains(searchKey) ||
                                                         x.Worker.FirstName.ToLower().Contains(searchKey) ||
                                                         x.Worker.LastName.ToLower().Contains(searchKey) ||
                                                         x.DoneAt.ToString().Contains(searchKey));
                    }
                }

                switch (sortParameter)
                {
                    case Constants.Constants.CaseSortParameters.CreatedAt:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.Id);
                        else
                            sub_query = sub_query.OrderBy(x => x.Id);
                        break;
                    case Constants.Constants.CaseSortParameters.DoneAt:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.DoneAt);
                        else
                            sub_query = sub_query.OrderBy(x => x.DoneAt);
                        break;
                    case Constants.Constants.CaseSortParameters.WorkerName:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.Worker.FirstName);
                        else
                            sub_query = sub_query.OrderBy(x => x.Worker.FirstName);
                        break;
                    case Constants.Constants.CaseSortParameters.SiteName:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.Site.Name);
                        else
                            sub_query = sub_query.OrderBy(x => x.Site.Name);
                        break;
                    case Constants.Constants.CaseSortParameters.UnitId:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.UnitId);
                        else
                            sub_query = sub_query.OrderBy(x => x.UnitId);
                        break;
                    case Constants.Constants.CaseSortParameters.Status:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.Status);
                        else
                            sub_query = sub_query.OrderBy(x => x.Status);
                        break;
                    case Constants.Constants.CaseSortParameters.Field1:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue1);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue1);
                        break;
                    case Constants.Constants.CaseSortParameters.Field2:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue2);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue2);
                        break;
                    case Constants.Constants.CaseSortParameters.Field3:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue3);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue3);
                        break;
                    case Constants.Constants.CaseSortParameters.Field4:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue4);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue4);
                        break;
                    case Constants.Constants.CaseSortParameters.Field5:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue5);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue5);
                        break;
                    case Constants.Constants.CaseSortParameters.Field6:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue6);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue6);
                        break;
                    case Constants.Constants.CaseSortParameters.Field7:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue7);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue7);
                        break;
                    case Constants.Constants.CaseSortParameters.Field8:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue8);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue8);
                        break;
                    case Constants.Constants.CaseSortParameters.Field9:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue9);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue9);
                        break;
                    case Constants.Constants.CaseSortParameters.Field10:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.FieldValue10);
                        else
                            sub_query = sub_query.OrderBy(x => x.FieldValue10);
                        break;
                    default:
                        if (descendingSort)
                            sub_query = sub_query.OrderByDescending(x => x.Id);
                        else
                            sub_query = sub_query.OrderBy(x => x.Id);
                        break;
                }

//                    string bla = sub_query.ToSql(db);
//                    log.LogStandard("SQLController", $"Query is {bla}");
                matches = await sub_query.AsNoTracking().ToListAsync();

                List<Dto.Case> rtrnLst = new List<Dto.Case>();
                int numOfElements = 0;
                numOfElements = matches.Count();
                List<Case> dbCases = null;

                if (numOfElements < pageSize)
                {
                    dbCases = matches.ToList();
                }
                else
                {
                    offSet = offSet * pageSize;
                    dbCases = matches.Skip(offSet).Take(pageSize).ToList();
                }

                #region cases -> Case
                foreach (var dbCase in dbCases)
                {
                    Site site = await db.Sites.SingleAsync(x => x.Id == dbCase.SiteId);
                    Unit unit = await db.Units.SingleAsync(x => x.Id == dbCase.UnitId);
                    Worker worker = await db.Workers.SingleAsync(x => x.Id == dbCase.WorkerId);
                    Dto.Case nCase = new Dto.Case
                    {
                        CaseType = dbCase.Type,
                        CaseUId = dbCase.CaseUid,
                        CheckUIid = dbCase.MicrotingCheckUid,
                        CreatedAt = TimeZoneInfo.ConvertTimeFromUtc((DateTime)dbCase.CreatedAt, timeZoneInfo),
                        Custom = dbCase.Custom,
                        DoneAt = TimeZoneInfo.ConvertTimeFromUtc((DateTime)dbCase.DoneAt, timeZoneInfo),
                        Id = dbCase.Id,
                        MicrotingUId = dbCase.MicrotingUid,
                        SiteId = site.MicrotingUid,
                        SiteName = site.Name,
                        Status = dbCase.Status,
                        TemplatId = dbCase.CheckListId,
                        UnitId = unit.MicrotingUid,
                        UpdatedAt = dbCase.UpdatedAt,
                        Version = dbCase.Version,
                        WorkerName = worker.FirstName + " " + worker.LastName,
                        WorkflowState = dbCase.WorkflowState,
                        FieldValue1 = dbCase.FieldValue1 == null || dbCase.FieldValue1 == "null" ? "" : dbCase.FieldValue1,
                        FieldValue2 = dbCase.FieldValue2 == null || dbCase.FieldValue2 == "null" ? "" : dbCase.FieldValue2,
                        FieldValue3 = dbCase.FieldValue3 == null || dbCase.FieldValue3 == "null" ? "" : dbCase.FieldValue3,
                        FieldValue4 = dbCase.FieldValue4 == null || dbCase.FieldValue4 == "null" ? "" : dbCase.FieldValue4,
                        FieldValue5 = dbCase.FieldValue5 == null || dbCase.FieldValue5 == "null" ? "" : dbCase.FieldValue5,
                        FieldValue6 = dbCase.FieldValue6 == null || dbCase.FieldValue6 == "null" ? "" : dbCase.FieldValue6,
                        FieldValue7 = dbCase.FieldValue7 == null || dbCase.FieldValue7 == "null" ? "" : dbCase.FieldValue7,
                        FieldValue8 = dbCase.FieldValue8 == null || dbCase.FieldValue8 == "null" ? "" : dbCase.FieldValue8,
                        FieldValue9 = dbCase.FieldValue9 == null || dbCase.FieldValue9 == "null" ? "" : dbCase.FieldValue9,
                        FieldValue10 = dbCase.FieldValue10 == null || dbCase.FieldValue10 == "null" ? "" : dbCase.FieldValue10
                    };

                    rtrnLst.Add(nCase);
                }
                #endregion

                CaseList caseList = new CaseList(numOfElements, pageSize, rtrnLst);


                return caseList;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Returns a list of Case objects from DB with given parameters as templateId, startTime, endTime, workflowstate, searchkey, descendingsort, sortparameter
        /// </summary>
        /// <param name="templatId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="workflowState"></param>
        /// <param name="searchKey"></param>
        /// <param name="descendingSort"></param>
        /// <param name="sortParameter"></param>
        /// <returns></returns>
        public async Task<List<Dto.Case>> CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState, string searchKey, bool descendingSort, string sortParameter, TimeZoneInfo timeZoneInfo)
        {
            string methodName = "SqlController.CaseReadAll";
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(templatId), templatId);
            log.LogVariable(methodName, nameof(start), start);
            log.LogVariable(methodName, nameof(end), end);
            log.LogVariable(methodName, nameof(workflowState), workflowState);
            log.LogVariable(methodName, nameof(searchKey), searchKey);
            log.LogVariable(methodName, nameof(descendingSort), descendingSort);
            log.LogVariable(methodName, nameof(sortParameter), sortParameter);

            CaseList cl = await CaseReadAll(templatId, start, end, workflowState, searchKey, descendingSort, sortParameter, 0,
                10000000, timeZoneInfo);

            List<Dto.Case> rtnCaseList = new List<Dto.Case>();

            foreach (Dto.Case @case in cl.Cases)
            {
                rtnCaseList.Add(@case);
            }

            return rtnCaseList;

        }

        /// <summary>
        /// Returns a list of case data transfer objects from DB with given Custom string
        /// </summary>
        /// <param name="customString"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<CaseDto>> CaseFindCustomMatchs(string customString)
        {
            string methodName = "SqlController.CaseFindCustomMatchs";
            try
            {
                await using var db = GetContext();
                List<CaseDto> foundCasesThatMatch = new List<CaseDto>();

                List<Case> lstMatchs = db.Cases.Where(x => x.Custom == customString && x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToList();

                foreach (Case match in lstMatchs)
                    foundCasesThatMatch.Add(await CaseReadByCaseId(match.Id));

                return foundCasesThatMatch;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<bool> CaseUpdateFieldValues(int caseId)
        {
            string methodName = "SqlController.CaseUpdateFieldValues";
            try
            {
                await using var db = GetContext();
                Case lstMatchs = await db.Cases.SingleOrDefaultAsync(x => x.Id == caseId);

                if (lstMatchs == null)
                    return false;

                lstMatchs.UpdatedAt = DateTime.UtcNow;
                lstMatchs.Version = lstMatchs.Version + 1;
                List<int?> case_fields = new List<int?>();
                CheckList cl = await db.CheckLists.SingleAsync(x => x.Id == lstMatchs.CheckListId);

                case_fields.Add(cl.Field1);
                case_fields.Add(cl.Field2);
                case_fields.Add(cl.Field3);
                case_fields.Add(cl.Field4);
                case_fields.Add(cl.Field5);
                case_fields.Add(cl.Field6);
                case_fields.Add(cl.Field7);
                case_fields.Add(cl.Field8);
                case_fields.Add(cl.Field9);
                case_fields.Add(cl.Field10);

                lstMatchs.FieldValue1 = null;
                lstMatchs.FieldValue2 = null;
                lstMatchs.FieldValue3 = null;
                lstMatchs.FieldValue4 = null;
                lstMatchs.FieldValue5 = null;
                lstMatchs.FieldValue6 = null;
                lstMatchs.FieldValue7 = null;
                lstMatchs.FieldValue8 = null;
                lstMatchs.FieldValue9 = null;
                lstMatchs.FieldValue10 = null;

                List<FieldValue> fieldValues = db.FieldValues.Where(x => x.CaseId == lstMatchs.Id && case_fields.Contains(x.FieldId)).ToList();

                foreach (FieldValue item in fieldValues)
                {
                    Data.Entities.Field field = await db.Fields.SingleAsync(x => x.Id == item.FieldId);
                    FieldType fieldType = db.FieldTypes.Single(x => x.Id == field.FieldTypeId);
                    string newValue = item.Value;

                    if (fieldType.Type == Constants.Constants.FieldTypes.EntitySearch || fieldType.Type == Constants.Constants.FieldTypes.EntitySelect)
                    {
                        try
                        {
                            if (item.Value != "" || item.Value != null)
                            {
                                EntityItem match = await db.EntityItems.SingleOrDefaultAsync(x => x.Id == int.Parse(item.Value));

                                if (match != null)
                                {
                                    newValue = match.Name;
                                }

                            }
                        }
                        catch { }
                    }

                    if (fieldType.Type == Constants.Constants.FieldTypes.SingleSelect)
                    {
                        string key = item.Value;
                        string fullKey = t.Locate(item.Field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                        newValue = t.Locate(fullKey, "<key>", "</key>");
                    }

                    if (fieldType.Type == Constants.Constants.FieldTypes.MultiSelect)
                    {
                        newValue = "";

                        string keys = item.Value;
                        List<string> keyLst = keys.Split('|').ToList();

                        foreach (string key in keyLst)
                        {
                            string fullKey = t.Locate(item.Field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                            if (newValue != "")
                            {
                                newValue += "\n" + t.Locate(fullKey, "<key>", "</key>");
                            }
                            else
                            {
                                newValue += t.Locate(fullKey, "<key>", "</key>");
                            }
                        }
                    }


                    int i = case_fields.IndexOf(item.FieldId);
                    switch (i)
                    {
                        case 0:
                            lstMatchs.FieldValue1 = newValue;
                            break;
                        case 1:
                            lstMatchs.FieldValue2 = newValue;
                            break;
                        case 2:
                            lstMatchs.FieldValue3 = newValue;
                            break;
                        case 3:
                            lstMatchs.FieldValue4 = newValue;
                            break;
                        case 4:
                            lstMatchs.FieldValue5 = newValue;
                            break;
                        case 5:
                            lstMatchs.FieldValue6 = newValue;
                            break;
                        case 6:
                            lstMatchs.FieldValue7 = newValue;
                            break;
                        case 7:
                            lstMatchs.FieldValue8 = newValue;
                            break;
                        case 8:
                            lstMatchs.FieldValue9 = newValue;
                            break;
                        case 9:
                            lstMatchs.FieldValue10 = newValue;
                            break;
                    }
                }

                await lstMatchs.Update(db).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName +  " failed", ex);
            }
        }
        #endregion

        #region public site
        #region site

        /// <summary>
        /// Returns a list of site data transfer objects from DB
        /// </summary>
        /// <param name="includeRemoved"></param>
        /// <returns></returns>
        public async Task<List<SiteNameDto>> SiteGetAll(bool includeRemoved)
        {
            List<SiteNameDto> siteList = new List<SiteNameDto>();
            await using var db = GetContext();
            List<Site> matches = null;
            if(includeRemoved)
                matches = await db.Sites.ToListAsync();
            else
                matches = await db.Sites.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToListAsync();

            foreach (Site aSite in matches)
            {
                SiteNameDto siteNameDto = new SiteNameDto((int)aSite.MicrotingUid, aSite.Name, aSite.CreatedAt, aSite.UpdatedAt);
                siteList.Add(siteNameDto);
            }

            return siteList;

        }

        //TODO
        public async Task<List<SiteDto>> SimpleSiteGetAll(string workflowState, int? offSet, int? limit)
        {
            List<SiteDto> siteList = new List<SiteDto>();
            await using var db = GetContext();
            List<Site> matches = null;
            switch (workflowState)
            {
                case Constants.Constants.WorkflowStates.NotRemoved:
                    matches = await db.Sites.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToListAsync().ConfigureAwait(false);
                    break;
                case Constants.Constants.WorkflowStates.Removed:
                    matches = await db.Sites.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed).ToListAsync().ConfigureAwait(false);
                    break;
                case Constants.Constants.WorkflowStates.Created:
                    matches = await db.Sites.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToListAsync().ConfigureAwait(false);
                    break;
                default:
                    matches = await db.Sites.ToListAsync().ConfigureAwait(false);
                    break;
            }
            foreach (Site aSite in matches)
            {
                Unit unit = null;
                Worker worker = null;
                int? unitCustomerNo = null;
                int? unitOptCode = null;
                int? unitMicrotingUid = null;
                int? workerMicrotingUid = null;
                string workerFirstName = null;
                string workerLastName = null;
                try
                {
                    unit = await db.Units.FirstAsync(x => x.SiteId == aSite.Id);
                    unitCustomerNo = unit.CustomerNo;
                    unitOptCode = unit.OtpCode ?? 0;
                    unitMicrotingUid = (int)unit.MicrotingUid;
                }
                catch { }

                try
                {
                    SiteWorker siteWorker = await db.SiteWorkers.FirstAsync(x => x.SiteId == aSite.Id);
                    worker = await db.Workers.SingleAsync(x => x.Id == siteWorker.WorkerId);
                    workerMicrotingUid = worker.MicrotingUid;
                    workerFirstName = worker.FirstName;
                    workerLastName = worker.LastName;
                }
                catch { }

                SiteDto siteDto = new SiteDto((int)aSite.MicrotingUid, aSite.Name, workerFirstName, workerLastName, unitCustomerNo, unitOptCode, unitMicrotingUid, workerMicrotingUid);
                siteList.Add(siteDto);
            }

            return siteList;

        }

        /// <summary>
        /// Creates a site in DB with given microtingUid and Name
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> SiteCreate(int microtingUid, string name)
        {
            string methodName = "SqlController.SiteCreate";
            try
            {
                await using var db = GetContext();
                Site site = new Site
                {
                    MicrotingUid = microtingUid,
                    Name = name
                };
                await site.Create(db).ConfigureAwait(false);

                return site.Id;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads a site from DB with given microting_uid
        /// </summary>
        /// <param name="microting_uid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<SiteNameDto> SiteRead(int microting_uid)
        {
            string methodName = "SqlController.SiteRead";
            try
            {
                await using var db = GetContext();
                Site site = await db.Sites.SingleOrDefaultAsync(x => x.MicrotingUid == microting_uid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                if (site != null)
                    return new SiteNameDto((int)site.MicrotingUid, site.Name, site.CreatedAt, site.UpdatedAt);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<SiteDto> SiteReadSimple(int microting_uid)
        {
            string methodName = "SqlController.SiteReadSimple";
            try
            {
                await using var db = GetContext();
                Site site = await db.Sites.SingleOrDefaultAsync(x => x.MicrotingUid == microting_uid);
                if (site == null)
                    return null;

                SiteWorker siteWorker = db.SiteWorkers.Where(x => x.SiteId == site.Id).ToList().First();
                Worker worker = await db.Workers.SingleAsync(x => x.Id == siteWorker.WorkerId);
                List<Unit> units = db.Units.Where(x => x.SiteId == site.Id).ToList();

                if (units.Count() > 0 && worker != null)
                {
                    Unit unit = units.First();
                    return new SiteDto((int)site.MicrotingUid, site.Name, worker.FirstName, worker.LastName, (int)unit.CustomerNo, unit.OtpCode ?? 0, (int)unit.MicrotingUid, worker.MicrotingUid);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates site in DB with new values of microting_uid and name
        /// </summary>
        /// <param name="microting_uid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SiteUpdate(int microting_uid, string name)
        {
            string methodName = "SqlController.SiteUpdate";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                Site site = await db.Sites.SingleOrDefaultAsync(x => x.MicrotingUid == microting_uid);

                if (site != null)
                {
//                        site.Version = site.Version + 1;
//                        site.UpdatedAt = DateTime.UtcNow;

                    site.Name = name;
                    await site.Update(db).ConfigureAwait(false);

//                        db.site_versions.Add(MapSiteVersions(site));
//                        db.SaveChanges();

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes site in DB with given microting_uid
        /// </summary>
        /// <param name="microting_uid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SiteDelete(int microting_uid)
        {
            string methodName = "SqlController.SiteDelete";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                Site site = await db.Sites.SingleOrDefaultAsync(x => x.MicrotingUid == microting_uid);

                if (site != null)
                {
                    await site.Delete(db);
//                        site.Version = site.Version + 1;
//                        site.UpdatedAt = DateTime.UtcNow;

//                        site.WorkflowState = Constants.Constants.WorkflowStates.Removed;

//                        db.site_versions.Add(MapSiteVersions(site));
//                        db.SaveChanges();

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region worker

        /// <summary>
        /// Returns a list of workers from DB
        /// </summary>
        /// <param name="workflowState"></param>
        /// <param name="offSet"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<WorkerDto>> WorkerGetAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = "SqlController.WorkerGetAll";
            try
            {
                List<WorkerDto> listWorkerDto = new List<WorkerDto>();

                await using var db = GetContext();
                List<Worker> matches = null;

                switch (workflowState)
                {
                    case Constants.Constants.WorkflowStates.NotRemoved:
                        matches = await db.Workers.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToListAsync();
                        break;
                    case Constants.Constants.WorkflowStates.Removed:
                        matches = await db.Workers.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed).ToListAsync();
                        break;
                    case Constants.Constants.WorkflowStates.Created:
                        matches = await db.Workers.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToListAsync();
                        break;
                    default:
                        matches = await db.Workers.ToListAsync();
                        break;
                }
                foreach (Worker worker in matches)
                {
                    WorkerDto workerDto = new WorkerDto(worker.MicrotingUid, worker.FirstName, worker.LastName, worker.Email, worker.CreatedAt, worker.UpdatedAt);
                    listWorkerDto.Add(workerDto);
                }
                return listWorkerDto;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }

        }

        /// <summary>
        /// Creates a Worker in DB with given microtingUid, first name, last name and email.
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> WorkerCreate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = "SqlController.WorkerCreate";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                Worker worker = new Worker
                {
                    MicrotingUid = microtingUid, FirstName = firstName, LastName = lastName, Email = email
                };
                await worker.Create(db).ConfigureAwait(false);

                return worker.Id;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads a workers name from DB with give workerId
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> WorkerNameRead(int workerId)
        {
            string methodName = "SqlController.WorkerNameRead";
            try
            {
                await using var db = GetContext();
                Worker worker = await db.Workers.SingleOrDefaultAsync(x => x.Id == workerId && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                if (worker == null)
                    return null;
                else
                    return worker.FirstName + " " + worker.LastName;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// returns a worker data transfer object from DB with given microting_uid
        /// </summary>
        /// <param name="microting_uid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<WorkerDto> WorkerRead(int microting_uid)
        {
            string methodName = "SqlController.WorkerRead";
            try
            {
                await using var db = GetContext();
                Worker worker = await db.Workers.SingleOrDefaultAsync(x => x.MicrotingUid == microting_uid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                if (worker != null)
                    return new WorkerDto((int)worker.MicrotingUid, worker.FirstName, worker.LastName, worker.Email, worker.CreatedAt, worker.UpdatedAt);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates worker in DB with new values of first name, last name and email.
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> WorkerUpdate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = "SqlController.WorkerUpdate";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                Worker worker = await db.Workers.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUid);

                if (worker != null)
                {
                    worker.FirstName = firstName;
                    worker.LastName = lastName;
                    worker.Email = email;
                    await worker.Update(db).ConfigureAwait(false);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }


        /// <summary>
        /// Deletes a worker with given microtingUid
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> WorkerDelete(int microtingUid)
        {
            string methodName = "SqlController.WorkerDelete";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");

                Worker worker = await db.Workers.AsNoTracking().SingleOrDefaultAsync(x => x.MicrotingUid == microtingUid);

                if (worker != null)
                {
                    await worker.Delete(db);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region site_worker

        /// <summary>
        /// Creates Site Worker in DB with given microtingUId, siteUId and workerUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <param name="siteUId"></param>
        /// <param name="workerUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> SiteWorkerCreate(int microtingUId, int siteUId, int workerUId)
        {
            string methodName = "SqlController.SiteWorkerCreate";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");

                int localSiteId = db.Sites.SingleAsync(x => x.MicrotingUid == siteUId).GetAwaiter().GetResult().Id;
                int localWorkerId = db.Workers.SingleAsync(x => x.MicrotingUid == workerUId).GetAwaiter().GetResult().Id;

                SiteWorker siteWorker = new SiteWorker();
//                    site_worker.WorkflowState = Constants.Constants.WorkflowStates.Created;
//                    site_worker.Version = 1;
//                    site_worker.CreatedAt = DateTime.UtcNow;
//                    site_worker.UpdatedAt = DateTime.UtcNow;
                siteWorker.MicrotingUid = microtingUId;
                siteWorker.SiteId = localSiteId;
                siteWorker.WorkerId = localWorkerId;
                await siteWorker.Create(db).ConfigureAwait(false);


//                    db.site_workers.Add(site_worker);
//                    db.SaveChanges();

//                    db.site_worker_versions.Add(MapSiteWorkerVersions(site_worker));
//                    db.SaveChanges();

                return siteWorker.Id;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads Site Worker from DB
        /// </summary>
        /// <param name="siteWorkerMicrotingUid"></param>
        /// <param name="siteId"></param>
        /// <param name="workerId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<SiteWorkerDto> SiteWorkerRead(int? siteWorkerMicrotingUid, int? siteId, int? workerId)
        {
            string methodName = "SqlController.SiteWorkerRead";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                SiteWorker siteWorker = null;
                if (siteWorkerMicrotingUid == null)
                {
                    Site site = await db.Sites.SingleAsync(x => x.MicrotingUid == siteId);
                    Worker worker = await db.Workers.SingleAsync(x => x.MicrotingUid == workerId);
                    siteWorker = await db.SiteWorkers.SingleOrDefaultAsync(x => x.SiteId == site.Id && x.WorkerId == worker.Id);
                }
                else
                {
                    siteWorker = await db.SiteWorkers.SingleOrDefaultAsync(x => x.MicrotingUid == siteWorkerMicrotingUid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);
                }


                if (siteWorker != null)
                    return new SiteWorkerDto((int)siteWorker.MicrotingUid, (int)db.Sites.Single(x => x.Id == siteWorker.SiteId).MicrotingUid, (int)db.Workers.Single(x => x.Id == siteWorker.WorkerId).MicrotingUid);
                else
                    return null;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates Site Worker with new values of microtingUid, siteId and workerId
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <param name="siteId"></param>
        /// <param name="workerId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SiteWorkerUpdate(int microtingUid, int siteId, int workerId)
        {
            string methodName = "SqlController.SiteWorkerUpdate";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");

                SiteWorker siteWorker = await db.SiteWorkers.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUid);

                if (siteWorker != null)
                {
//                        site_worker.Version = site_worker.Version + 1;
//                        site_worker.UpdatedAt = DateTime.UtcNow;

                    siteWorker.SiteId = siteId;
                    siteWorker.WorkerId = workerId;
                    await siteWorker.Update(db).ConfigureAwait(false);

//                        db.site_worker_versions.Add(MapSiteWorkerVersions(site_worker));
//                        db.SaveChanges();


                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes Site Worker from DB with given micrioting_uid
        /// </summary>
        /// <param name="microting_uid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SiteWorkerDelete(int microting_uid)
        {
            string methodName = "SqlController.SiteWorkerDelete";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");

                SiteWorker siteWorker = await db.SiteWorkers.SingleOrDefaultAsync(x => x.MicrotingUid == microting_uid);

                if (siteWorker != null)
                {
                    await siteWorker.Delete(db);
//                        site_worker.Version = site_worker.Version + 1;
//                        site_worker.UpdatedAt = DateTime.UtcNow;

//                        site_worker.WorkflowState = Constants.Constants.WorkflowStates.Removed;

//                        db.site_worker_versions.Add(MapSiteWorkerVersions(site_worker));
//                        db.SaveChanges();

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region unit

        /// <summary>
        /// Returns list of unit data transfer objects from DB
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<UnitDto>> UnitGetAll()
        {
            string methodName = "SqlController.UnitGetAll";
            try
            {
                List<UnitDto> listWorkerDto = new List<UnitDto>();
                await using var db = GetContext();
                foreach (Unit unit in await db.Units.ToListAsync())
                {
                    UnitDto unitDto = new UnitDto()
                    {
                        UnitUId = (int)unit.MicrotingUid,
                        CustomerNo = (int)unit.CustomerNo,
                        OtpCode = (int)unit.OtpCode,
                        SiteUId = (int)db.Sites.Single(x => x.Id == unit.SiteId).MicrotingUid,
                        CreatedAt = unit.CreatedAt,
                        UpdatedAt = unit.UpdatedAt,
                        WorkflowState = unit.WorkflowState
                    };
//                        UnitDto unitDto = new UnitDto((int)unit.MicrotingUid, (int)unit.CustomerNo, (int)unit.OtpCode,
//                            (int)unit.Site.MicrotingUid, unit.CreatedAt, unit.UpdatedAt);
                    listWorkerDto.Add(unitDto);
                }

                return listWorkerDto;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }


        /// <summary>
        /// Creates Unit in DB with given micriotingUid, customer number, one time password and siteUId
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <param name="customerNo"></param>
        /// <param name="otpCode"></param>
        /// <param name="siteUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> UnitCreate(int microtingUid, int customerNo, int otpCode, int siteUId)
        {
            string methodName = "SqlController.UnitCreate";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");
                int localSiteId = db.Sites.SingleAsync(x => x.MicrotingUid == siteUId).GetAwaiter().GetResult().Id;

                Unit unit = new Unit
                {
                    MicrotingUid = microtingUid,
                    CustomerNo = customerNo,
                    OtpCode = otpCode,
                    SiteId = localSiteId
                };

                await unit.Create(db).ConfigureAwait(false);

                return unit.Id;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads Unit from DB with given microtingUid
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UnitDto> UnitRead(int microtingUid)
        {
            string methodName = "SqlController.UnitRead";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");

                Unit unit = await db.Units.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                if (unit != null)
                    return new UnitDto()
                    {
                        UnitUId = (int)unit.MicrotingUid,
                        CustomerNo = (int)unit.CustomerNo,
                        OtpCode = unit.OtpCode,
                        SiteUId = (int)unit.SiteId,
                        CreatedAt = unit.CreatedAt,
                        UpdatedAt = unit.UpdatedAt,
                        WorkflowState = unit.WorkflowState
                    };
                else
                    return null;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates Unit in DB with new values
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <param name="customerNo"></param>
        /// <param name="otpCode"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UnitUpdate(int microtingUid, int customerNo, int otpCode, int siteId)
        {
            string methodName = "SqlController.UnitUpdate";
            try
            {
                await using var db = GetContext();
                //logger.LogEverything(methodName + " called");

                Unit unit = await db.Units.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUid);

                if (unit != null)
                {
                    unit.CustomerNo = customerNo;
                    unit.OtpCode = otpCode;
                    await unit.Update(db).ConfigureAwait(false);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes Unit from DB with given microtingUid
        /// </summary>
        /// <param name="microtingUid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UnitDelete(int microtingUid)
        {
            string methodName = "SqlController.UnitDelete";
            try
            {
                await using var db = GetContext();
                Unit unit = await db.Units.SingleOrDefaultAsync(x => x.MicrotingUid == microtingUid);

                if (unit != null)
                {
                    await unit.Delete(db);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion
        #endregion

        #region public entity
        #region entityGroup

        //TODO
        public async Task<EntityGroupList> EntityGroupAll(string sort, string nameFilter, int offSet, int pageSize, string entityType, bool desc, string workflowState)
        {

            if (entityType != Constants.Constants.FieldTypes.EntitySearch && entityType != Constants.Constants.FieldTypes.EntitySelect)
                throw new Exception("EntityGroupAll failed. EntityType:" + entityType + " is not an known type");
            if (workflowState != Constants.Constants.WorkflowStates.NotRemoved && workflowState != Constants.Constants.WorkflowStates.Created && workflowState != Constants.Constants.WorkflowStates.Removed)
                throw new Exception("EntityGroupAll failed. workflowState:" + workflowState + " is not an known workflow state");
            string methodName = "SqlController.EntityGroupAll";

            List<EntityGroup> eG = null;
            List<Models.EntityGroup> e_G = new List<Models.EntityGroup>();
            int numOfElements = 0;
            try
            {
                await using var db = GetContext();
                var source = db.EntityGroups.Where(x => x.Type == entityType);
                if (nameFilter != "")
                    source = source.Where(x => x.Name.Contains(nameFilter));
                if (sort == "Id")
                    if (desc)
                        source = source.OrderByDescending(x => x.Id);
                    else
                        source = source.OrderBy(x => x.Id);
                else
                if (desc)
                    source = source.OrderByDescending(x => x.Name);
                else
                    source = source.OrderBy(x => x.Id);

                switch (workflowState)
                {
                    case Constants.Constants.WorkflowStates.NotRemoved:
                        source = source.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed);
                        break;
                    case Constants.Constants.WorkflowStates.Removed:
                        source = source.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed);
                        break;
                    case Constants.Constants.WorkflowStates.Created:
                        source = source.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created);
                        break;
                }

                numOfElements = source.Count();
                if (numOfElements < pageSize)
                {
                    eG = await source.ToListAsync();
                }
                else
                {
                    eG = await source.Skip(offSet).Take(pageSize).ToListAsync();
                }

                foreach (EntityGroup eg in eG)
                {
//                        EntityGroup g = new EntityGroup(eg.Id, eg.Name, eg.Type, eg.MicrotingUid, new List<EntityItem>(), eg.WorkflowState, eg.CreatedAt, eg.UpdatedAt);
                    Models.EntityGroup g = new Models.EntityGroup()
                    {
                        Id = eg.Id,
                        Name = eg.Name,
                        Type = eg.Type,
                        MicrotingUUID = eg.MicrotingUid,
                        EntityGroupItemLst = new List<Models.EntityItem>(),
                        WorkflowState = eg.WorkflowState,
                        CreatedAt = eg.CreatedAt,
                        UpdatedAt = eg.UpdatedAt,
                        Description = eg.Description
                    };
                    e_G.Add(g);
                }
                return new EntityGroupList(numOfElements, offSet, e_G);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Crates an Entity Group in DB with given name and type of entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Models.EntityGroup> EntityGroupCreate(string name, string entityType, string description)
        {
            string methodName = "SqlController.EntityGroupCreate";
            try
            {
                if (entityType != Constants.Constants.FieldTypes.EntitySearch && entityType != Constants.Constants.FieldTypes.EntitySelect)
                    throw new Exception("EntityGroupCreate failed. EntityType:" + entityType + " is not an known type");

                await using var db = GetContext();
                EntityGroup eG = new EntityGroup {Name = name, Type = entityType, Description = description};

                await eG.Create(db).ConfigureAwait(false);

                return new Models.EntityGroup
                {
                    Id = eG.Id,
                    Name = eG.Name,
                    Type = eG.Type,
                    MicrotingUUID = eG.MicrotingUid,
                    EntityGroupItemLst = new List<Models.EntityItem>(),
                    WorkflowState = eG.WorkflowState,
                    CreatedAt = eG.CreatedAt,
                    UpdatedAt = eG.UpdatedAt,
                    Description = eG.Description
                };
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<Models.EntityGroup> EntityGroupReadSorted(string entityGroupMUId, string sort, string nameFilter)
        {
            string methodName = "SqlController.EntityGroupReadSorted";
            try
            {
                return await EntityGroup.ReadSorted(GetContext(), entityGroupMUId, sort, nameFilter);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads Entity Group from DB with given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<Models.EntityGroup> EntityGroupRead(int Id)
        {
            await using var db = GetContext();
            EntityGroup eg = await db.EntityGroups.SingleOrDefaultAsync(x => x.Id == Id);
            if (eg != null) {
                List<Models.EntityItem> egl = new List<Models.EntityItem>();
                return new Models.EntityGroup
                {
                    Id = eg.Id,
                    Name = eg.Name,
                    Type = eg.Type,
                    MicrotingUUID = eg.MicrotingUid,
                    EntityGroupItemLst = egl
                };
//                    return new EntityGroup(eg.Id, eg.Name, eg.Type, eg.MicrotingUid, egl);
            } else {
                throw new NullReferenceException("No EntityGroup found by Id " + Id.ToString());
            }
        }

        //TODO
        public async Task<Models.EntityGroup> EntityGroupRead(string entityGroupMUId)
        {
            return await EntityGroupReadSorted(entityGroupMUId, "Id", "");
        }

        /// <summary>
        /// Updates Entity Group in DB with given EntitygroupId and entityGroupMUId
        /// </summary>
        /// <param name="entityGroupId"></param>
        /// <param name="entityGroupMUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> EntityGroupUpdate(int entityGroupId, string entityGroupMUId)
        {
            string methodName = "SqlController.EntityGroupUpdate";
            try
            {
                await using var db = GetContext();
                EntityGroup eG = await db.EntityGroups.SingleOrDefaultAsync(x => x.Id == entityGroupId);

                if (eG == null)
                    return false;

                eG.MicrotingUid = entityGroupMUId;
                await eG.Update(db).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates Entity Group Name in DB with given name and entitygroupMUId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entityGroupMUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> EntityGroupUpdateName(string name, string entityGroupMUId)
        {
            string methodName = "SqlController.EntityGroupUpdateName";
            try
            {
                await using var db = GetContext();
                EntityGroup eG = await db.EntityGroups.SingleOrDefaultAsync(x => x.MicrotingUid == entityGroupMUId);

                if (eG == null)
                    return false;

                eG.Name = name;
                await eG.Update(db).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes Entity Group in DB with given entityGroupMUId
        /// </summary>
        /// <param name="entityGroupMUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> EntityGroupDelete(string entityGroupMUId)
        {
            string methodName = "SqlController.EntityGroupDelete";
            try
            {
                await using var db = GetContext();
                List<string> killLst = new List<string>();

                EntityGroup eG = await db.EntityGroups.SingleOrDefaultAsync(x => x.MicrotingUid == entityGroupMUId && x.WorkflowState != Constants.Constants.WorkflowStates.Removed);

                if (eG == null)
                    return null;

                killLst.Add(eG.MicrotingUid);

                await eG.Delete(db);

                List<EntityItem> lst = db.EntityItems.Where(x => x.EntityGroupId == eG.Id && x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToList();
                if (lst != null)
                {
                    foreach (EntityItem item in lst)
                    {
                        item.Synced = t.Bool(false);
                        await item.Update(db).ConfigureAwait(false);
                        await item.Delete(db);

                        killLst.Add(item.MicrotingUid);
                    }
                }

                return eG.Type;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region entityItem

        /// <summary>
        /// Reads an entity item from DB with given microtingUid
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EntityItem> EntityItemRead(string microtingUId)
        {
            string methodName = "SqlController.EntityItemRead";
            try
            {
                await using var db = GetContext();
                return await db.EntityItems.SingleAsync(x => x.MicrotingUid == microtingUId);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Reads an Entity Item from DB with given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<Models.EntityItem> EntityItemRead(int id)
        {
            await using var db = GetContext();
            EntityItem et = await db.EntityItems.FirstOrDefaultAsync(x => x.Id == id);
            if (et != null)
            {
                Models.EntityItem entityItem = new Models.EntityItem
                {
                    Id = et.Id,
                    Name = et.Name,
                    Description = et.Description,
                    EntityItemUId = et.EntityItemUid,
                    MicrotingUUID = et.MicrotingUid,
                    DisplayIndex = et.DisplayIndex,
                    EntityItemGroupId = et.EntityGroupId,
                    WorkflowState = et.WorkflowState
                };
                return entityItem;
            }

            throw new NullReferenceException("No EntityItem found for Id " + id.ToString());
        }


        /// <summary>
        /// Reads an entity item group with given id, name and description from DB
        /// </summary>
        /// <param name="entityItemGroupId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<Models.EntityItem> EntityItemRead(int entityItemGroupId, string name, string description)
        {
            await using var db = GetContext();
            EntityItem et = await db.EntityItems.SingleOrDefaultAsync(x => x.Name == name
                                                                           && x.Description == description
                                                                           && x.EntityGroupId == entityItemGroupId);
            if (et != null)
            {
                return new Models.EntityItem
                {
                    Id = et.Id,
                    Name = et.Name,
                    Description = et.Description,
                    EntityItemUId = et.EntityItemUid,
                    MicrotingUUID = et.MicrotingUid,
                    WorkflowState = et.WorkflowState
                };
            }

            return null;
        }


        //TODO
        public async Task<Models.EntityItem> EntityItemCreate(int entityItemGroupId, Models.EntityItem entityItem)
        {
            await using var db = GetContext();
            EntityItem eI = new EntityItem
            {
                EntityGroupId = entityItemGroupId,
                EntityItemUid = entityItem.EntityItemUId,
                MicrotingUid = entityItem.MicrotingUUID,
                Name = entityItem.Name,
                Description = entityItem.Description,
                DisplayIndex = entityItem.DisplayIndex,
                Synced = t.Bool(false)
            };
            await eI.Create(db).ConfigureAwait(false);
            return entityItem;
        }

        /// <summary>
        /// Updates an Entity Item in DB
        /// </summary>
        /// <param name="entityItem"></param>
        public async Task EntityItemUpdate(Models.EntityItem entityItem)
        {
            await using var db = GetContext();
            var match = await db.EntityItems.SingleOrDefaultAsync(x => x.Id == entityItem.Id);
            match.Description = entityItem.Description;
            match.Name = entityItem.Name;
            match.Synced = t.Bool(false);
            match.EntityItemUid = entityItem.EntityItemUId;
            match.WorkflowState = entityItem.WorkflowState;
            match.DisplayIndex = entityItem.DisplayIndex;
            await match.Update(db).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes an Entity Item in DB with given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <exception cref="NullReferenceException"></exception>
        public async Task EntityItemDelete(int Id)
        {
            await using var db = GetContext();
            EntityItem et = await db.EntityItems.SingleOrDefaultAsync(x => x.Id == Id);
            if (et == null)
            {
                throw new NullReferenceException("EntityItem not found with Id " + Id.ToString());
            }

            et.Synced = t.Bool(true);
            await et.Update(db).ConfigureAwait(false);
            await et.Delete(db);
        }
        #endregion
        #endregion

        #region folders

        /// <summary>
        /// Returns a list of Folders of type of folder data transfer objects from DB
        /// </summary>
        /// <param name="includeRemoved"></param>
        /// <returns></returns>
        public async Task<List<FolderDto>> FolderGetAll(bool includeRemoved)
        {
            List<FolderDto> folderDtos = new List<FolderDto>();
            await using var db = GetContext();
            List<Folder> matches = null;
            matches = includeRemoved ? await db.Folders.ToListAsync() : await db.Folders.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToListAsync();

            foreach (Folder folder in matches)
            {
                FolderDto folderDto = new FolderDto(folder.Id, folder.Name, folder.Description, folder.ParentId, folder.CreatedAt, folder.UpdatedAt, folder.MicrotingUid);
                folderDtos.Add(folderDto);
            }

            return folderDtos;
        }

        /// <summary>
        /// Reads a Folder of type Folder data transfer object from DB with specific Microting_UID
        /// </summary>
        /// <param name="microting_uid"></param>
        /// <returns></returns>
        public async Task<FolderDto> FolderReadByMicrotingUUID(int microting_uid)
        {
            await using var db = GetContext();
            Folder folder = await db.Folders.SingleOrDefaultAsync(x => x.MicrotingUid == microting_uid);

            if (folder == null)
            {
                return null;
            }

            FolderDto folderDto = new FolderDto(folder.Id, folder.Name, folder.Description, folder.ParentId, folder.CreatedAt, folder.UpdatedAt, folder.MicrotingUid);
            return folderDto;
        }

        /// <summary>
        /// Reads a folder from DB with given ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<FolderDto> FolderRead(int Id)
        {
            await using var db = GetContext();
            Folder folder = await db.Folders.SingleOrDefaultAsync(x => x.Id == Id);

            if (folder == null)
            {
                throw new NullReferenceException($"Could not find area with Id: {Id}");
            }

            FolderDto folderDto = new FolderDto(folder.Id, folder.Name, folder.Description, folder.ParentId, folder.CreatedAt, folder.UpdatedAt, folder.MicrotingUid);
            return folderDto;
        }

        /// <summary>
        /// Creates a folder in DB with given name, description, parent id and microtingUUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="parent_id"></param>
        /// <param name="microtingUUID"></param>
        /// <returns></returns>
        public async Task<int> FolderCreate(string name, string description, int? parent_id, int microtingUUID)
        {
            Folder folder = new Folder
            {
                Name = name,
                Description = description,
                ParentId = parent_id,
                MicrotingUid = microtingUUID
            };

            await folder.Create(GetContext()).ConfigureAwait(false);

            return folder.Id;
        }

        /// <summary>
        /// Updates Folder in DB with new values of Id, Name, Description and Parent Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="parent_id"></param>
        /// <exception cref="NullReferenceException"></exception>
        public async Task FolderUpdate(int Id, string name, string description, int? parent_id)
        {
            await using var db = GetContext();
            Folder folder = await db.Folders.SingleOrDefaultAsync(x => x.Id == Id).ConfigureAwait(false);

            if (folder == null)
            {
                throw new NullReferenceException($"Could not find area with Id: {Id}");
            }

            folder.Name = name;
            folder.Description = description;
            folder.ParentId = parent_id;

            await folder.Update(db).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a Folder in DB with given ID
        /// </summary>
        /// <param name="Id"></param>
        /// <exception cref="NullReferenceException"></exception>
        public async Task FolderDelete(int Id)
        {
            await using var db = GetContext();
            Folder folder = await db.Folders.SingleOrDefaultAsync(x => x.Id == Id).ConfigureAwait(false);

            if (folder == null)
            {
                throw new NullReferenceException($"Could not find area with Id: {Id}");
            }

            await folder.Delete(db).ConfigureAwait(false);
        }

        #endregion


        #region public setting

        //TODO
        public async Task<bool> SettingCreateDefaults()
        {
            string methodName = "SqlController.SettingCreateDefaults";

            WriteDebugConsoleLogEntry(new LogEntry(2, methodName, "called"));
            //key point
            await SettingCreate(Settings.firstRunDone);
            await SettingCreate(Settings.logLevel);
            await SettingCreate(Settings.logLimit);
            await SettingCreate(Settings.knownSitesDone);
            await SettingCreate(Settings.fileLocationPicture);
            await SettingCreate(Settings.fileLocationPdf);
            await SettingCreate(Settings.fileLocationJasper);
            await SettingCreate(Settings.token);
            await SettingCreate(Settings.comAddressBasic);
            await SettingCreate(Settings.comAddressApi);
            await SettingCreate(Settings.comAddressPdfUpload);
            await SettingCreate(Settings.comOrganizationId);
            await SettingCreate(Settings.awsAccessKeyId);
            await SettingCreate(Settings.awsSecretAccessKey);
            await SettingCreate(Settings.awsEndPoint);
            await SettingCreate(Settings.unitLicenseNumber);
            await SettingCreate(Settings.httpServerAddress);
            await SettingCreate(Settings.maxParallelism);
            await SettingCreate(Settings.numberOfWorkers);
            await SettingCreate(Settings.comSpeechToText);
            await SettingCreate(Settings.swiftEnabled);
            await SettingCreate(Settings.swiftUserName);
            await SettingCreate(Settings.swiftPassword);
            await SettingCreate(Settings.swiftEndPoint);
            await SettingCreate(Settings.keystoneEndPoint);
            await SettingCreate(Settings.customerNo);
            await SettingCreate(Settings.s3Enabled);
            await SettingCreate(Settings.s3AccessKeyId);
            await SettingCreate(Settings.s3SecrectAccessKey);
            await SettingCreate(Settings.s3Endpoint);
            await SettingCreate(Settings.s3BucketName);
            await SettingCreate(Settings.rabbitMqUser);
            await SettingCreate(Settings.rabbitMqPassword);
            await SettingCreate(Settings.rabbitMqHost);
            await SettingCreate(Settings.translationsMigrated);

            return true;
        }

        /// <summary>
        /// Creates setting with specific name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public async Task<bool> SettingCreate(Settings name)
        {
            await using var db = GetContext();
            //key point
            #region Id = settings.name
            int Id = -1;
            string defaultValue = "default";
            switch (name)
            {
                case Settings.firstRunDone: Id = 1; defaultValue = "false"; break;
                case Settings.logLevel: Id = 2; defaultValue = "4"; break;
                case Settings.logLimit: Id = 3; defaultValue = "25000"; break;
                case Settings.knownSitesDone: Id = 4; defaultValue = "false"; break;
                case Settings.fileLocationPicture:
                    if (isLinux)
                    {
                        Id = 5;
                        defaultValue = "/var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/output/datafolder/picture/";
                    }
                    else
                    {
                        Id = 5;
                        defaultValue = @"output\dataFolder\picture\";
                    }
                    break;
                case Settings.fileLocationPdf:
                    if (isLinux)
                    {
                        Id = 6;
                        defaultValue = "/var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/output/datafolder/pdf/";
                    }
                    else
                    {
                        Id = 6;
                        defaultValue = @"output\dataFolder\pdf\";
                    }
                    break;
                case Settings.fileLocationJasper:
                    if (isLinux)
                    {
                        Id = 7;
                        defaultValue = "/var/www/microting/eform-angular-frontend/eFormAPI/eFormAPI.Web/out/output/datafolder/reports/";
                    }
                    else
                    {
                        Id = 7;
                        defaultValue = @"output\dataFolder\reports\";
                    }
                    break;
                case Settings.token: Id = 8; defaultValue = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"; break;
                case Settings.comAddressBasic: Id = 9; defaultValue = "https://basic.microting.com"; break;
                case Settings.comAddressApi: Id = 10; defaultValue = "https://xxxxxx.xxxxxx.com"; break;
                case Settings.comAddressPdfUpload: Id = 11; defaultValue = "https://xxxxxx.xxxxxx.com"; break;
                case Settings.comOrganizationId: Id = 12; defaultValue = "0"; break;
                case Settings.awsAccessKeyId: Id = 13; defaultValue = "XXX"; break;
                case Settings.awsSecretAccessKey: Id = 14; defaultValue = "XXX"; break;
                case Settings.awsEndPoint: Id = 15; defaultValue = "XXX"; break;
                case Settings.unitLicenseNumber: Id = 16; defaultValue = "0"; break;
                case Settings.httpServerAddress: Id = 17; defaultValue = "http://localhost:3000"; break;
                case Settings.maxParallelism: Id = 18; defaultValue = "1"; break;
                case Settings.numberOfWorkers: Id = 19; defaultValue = "1"; break;
                case Settings.comSpeechToText: Id = 20; defaultValue = "https://xxxxxx.xxxxxx.com"; break;
                case Settings.swiftEnabled: Id = 21; defaultValue = "false"; break;
                case Settings.swiftUserName: Id = 22; defaultValue = "eformsdk"; break;
                case Settings.swiftPassword: Id = 23; defaultValue = "eformsdktosecretpassword"; break;
                case Settings.swiftEndPoint: Id = 24; defaultValue = "http://172.16.4.4:8080/swift/v1"; break;
                case Settings.keystoneEndPoint: Id = 25; defaultValue = "http://172.16.4.4:5000/v2.0"; break;
                case Settings.customerNo: Id = 26; defaultValue = "0"; break;
                case Settings.s3Enabled: Id = 27; defaultValue = "false"; break;
                case Settings.s3AccessKeyId: Id = 28; defaultValue = "XXX"; break;
                case Settings.s3SecrectAccessKey: Id = 29; defaultValue = "XXX"; break;
                case Settings.s3Endpoint: Id = 30; defaultValue = "https://s3.eu-central-1.amazonaws.com"; break;
                case Settings.s3BucketName: Id = 31; defaultValue = "microting-uploaded-data"; break;
                case Settings.rabbitMqUser: Id = 32; defaultValue = "admin"; break;
                case Settings.rabbitMqPassword: Id = 33; defaultValue = "password"; break;
                case Settings.rabbitMqHost: Id = 34; defaultValue = "localhost"; break;
                case Settings.translationsMigrated: Id = 35; defaultValue = "false"; break;

                default:
                    throw new IndexOutOfRangeException(name.ToString() + " is not a known/mapped Settings type");
            }
            #endregion

            Setting matchId = await db.Settings.SingleOrDefaultAsync(x => x.Id == Id);
            Setting matchName = await db.Settings.SingleOrDefaultAsync(x => x.Name == name.ToString());

            if (matchName == null)
            {
                if (matchId != null)
                {
                    #region there is already a setting with that Id but different name
                    //the old setting data is copied, and new is added
                    Setting newSettingBasedOnOld = new Setting();
                    newSettingBasedOnOld.Id = (db.Settings.Select(x => (int?)x.Id).Max() ?? 0) + 1;
                    newSettingBasedOnOld.Name = matchId.Name.ToString();
                    newSettingBasedOnOld.Value = matchId.Value;

                    db.Settings.Add(newSettingBasedOnOld);

                    matchId.Name = name.ToString();
                    matchId.Value = defaultValue;

                    db.SaveChanges();
                    #endregion
                }
                else
                {
                    //its a new setting
                    Setting newSetting = new Setting();
                    newSetting.Id = Id;
                    newSetting.Name = name.ToString();
                    newSetting.Value = defaultValue;

                    db.Settings.Add(newSetting);
                }
                db.SaveChanges();
            }
            else if (string.IsNullOrEmpty(matchName.Value))
            {
                matchName.Value = defaultValue;
            }

            return true;
        }

        /// <summary>
        /// Reads a specific setting from DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> SettingRead(Settings name)
        {
            string methodName = "SqlController.SettingRead";
            try
            {
                await using var db = GetContext();
                Setting match = await db.Settings.SingleAsync(x => x.Name == name.ToString());

                if (match.Value == null)
                    return "";

                return match.Value;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Updates specific setting with new value in DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newValue"></param>
        /// <exception cref="Exception"></exception>
        public async Task SettingUpdate(Settings name, string newValue)
        {
            string methodName = "SqlController.SettingUpdate";
            try
            {
                await using var db = GetContext();
                Setting match = await db.Settings.SingleOrDefaultAsync(x => x.Name == name.ToString());

                if (match == null)
                {
                    await SettingCreate(name);
                    match = await db.Settings.SingleAsync(x => x.Name == name.ToString());
                }

                match.Value = newValue;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public async Task<List<string>> SettingCheckAll()
        {
            string methodName = "SqlController.SettingCheckAll";
            List<string> result = new List<string>();
            try
            {
                await using var db = GetContext();
                if (await db.FieldTypes.CountAsync() == 0)
                {
                    #region prime FieldTypes
                    //UnitTest_TruncateTable(typeof(field_types).Name);

                    await FieldTypeAdd(1, Constants.Constants.FieldTypes.Text, "Simple text field");
                    await FieldTypeAdd(2, Constants.Constants.FieldTypes.Number, "Simple number field");
                    await FieldTypeAdd(3, Constants.Constants.FieldTypes.None, "Simple text to be displayed");
                    await FieldTypeAdd(4, Constants.Constants.FieldTypes.CheckBox, "Simple check box field");
                    await FieldTypeAdd(5, Constants.Constants.FieldTypes.Picture, "Simple picture field");
                    await FieldTypeAdd(6, Constants.Constants.FieldTypes.Audio, "Simple audio field");
                    await FieldTypeAdd(7, Constants.Constants.FieldTypes.Movie, "Simple movie field");
                    await FieldTypeAdd(8, Constants.Constants.FieldTypes.SingleSelect, "Single selection list");
                    await FieldTypeAdd(9, Constants.Constants.FieldTypes.Comment, "Simple comment field");
                    await FieldTypeAdd(10, Constants.Constants.FieldTypes.MultiSelect, "Simple multi select list");
                    await FieldTypeAdd(11, Constants.Constants.FieldTypes.Date, "Date selection");
                    await FieldTypeAdd(12, Constants.Constants.FieldTypes.Signature, "Simple signature field");
                    await FieldTypeAdd(13, Constants.Constants.FieldTypes.Timer, "Simple timer field");
                    await FieldTypeAdd(14, Constants.Constants.FieldTypes.EntitySearch, "Autofilled searchable items field");
                    await FieldTypeAdd(15, Constants.Constants.FieldTypes.EntitySelect, "Autofilled single selection list");
                    await FieldTypeAdd(16, Constants.Constants.FieldTypes.ShowPdf, "Show PDF");
                    await FieldTypeAdd(17, Constants.Constants.FieldTypes.FieldGroup, "Field group");
                    await FieldTypeAdd(18, Constants.Constants.FieldTypes.SaveButton, "Save eForm");
                    await FieldTypeAdd(19, Constants.Constants.FieldTypes.NumberStepper, "Number stepper field");
                    #endregion
                }

                int countVal = db.Settings.Count(x => x.Value == "");
                int countSet = db.Settings.Count();

                if (countSet == 0)
                {
                    result.Add("NO SETTINGS PRESENT, NEEDS PRIMING!");
                    return result;
                }

                foreach (var setting in Enum.GetValues(typeof(Settings)))
                {
                    try
                    {
                        string readSetting = await SettingRead((Settings)setting);
                        if (string.IsNullOrEmpty(readSetting))
                            result.Add(setting.ToString() + " has an empty value!");
                    }
                    catch
                    {
                        result.Add("There is no setting for " + setting + "! You need to add one");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region public write log

        /// <summary>
        /// Returns a log from corebase
        /// </summary>
        /// <param name="core"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Log StartLog(CoreBase core)
        {
            string methodName = "SqlController.StartLog";
            try
            {
                if (log == null)
                    log = new Log(this);
                return log;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public override void WriteLogEntry(LogEntry logEntry)
        {
            WriteDebugConsoleLogEntry(logEntry);

            if (logEntry.Level < 0)
                WriteLogExceptionEntry(logEntry);
        }

        private void WriteDebugConsoleLogEntry(LogEntry logEntry)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DBG] {logEntry.Type}: {logEntry.Message}");
            Console.ForegroundColor = oldColor;
        }

        private void WriteErrorConsoleLogEntry(LogEntry logEntry)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERR] {logEntry.Type}: {logEntry.Message}");
            Console.ForegroundColor = oldColor;
        }


        //TODO
        private void WriteLogExceptionEntry(LogEntry logEntry)
        {
            try
            {
                using var db = GetContext();
                WriteErrorConsoleLogEntry(logEntry);
            }
            catch (Exception)
            {
                //t.PrintException(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Writes into the log if it failed to write.
        /// </summary>
        /// <param name="logEntries"></param>
        public override void WriteIfFailed(string logEntries)
        {
//            lock (_lockWrite)
//            {
                try
                {
                    File.AppendAllText(@"expection.txt",
                        DateTime.UtcNow.ToString() + " // " + "L:" + "-22" + " // " + "Write logic failed" + " // " + Environment.NewLine
                        + logEntries + Environment.NewLine);
                }
                catch
                {
                    //magic
                }
//            }
        }
        #endregion
        #endregion

        #region private
        #region EformCreateDb

        //TODO
        private async Task<int> EformCreateDb(MainElement mainElement)
        {
            string methodName = "SqlController.EformCreateDb";
            try
            {
                await using var db = GetContext();
                GetConverter();

                Language language = await db.Languages.SingleAsync(x => x.Name == "Danish").ConfigureAwait(false);
                Language ukLanguage = await db.Languages.SingleAsync(x => x.Name == "English").ConfigureAwait(false);
                Language deLanguage = await db.Languages.SingleAsync(x => x.Name == "German").ConfigureAwait(false);

                #region mainElement

                CheckList cl = new CheckList
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    //Label = mainElement.Label,
                    WorkflowState = Constants.Constants.WorkflowStates.Created,
                    ParentId = null,
                    Repeated = mainElement.Repeated,
                    QuickSyncEnabled = t.Bool(mainElement.EnableQuickSync),
                    Version = 1,
                    CaseType = mainElement.CaseType,
                    FolderName = mainElement.CheckListFolderName,
                    DisplayIndex = mainElement.DisplayOrder,
                    ReviewEnabled = 0,
                    ManualSync = t.Bool(mainElement.ManualSync),
                    ExtraFieldsEnabled = 0,
                    DoneButtonEnabled = 0,
                    ApprovalEnabled = 0,
                    MultiApproval = t.Bool(mainElement.MultiApproval),
                    FastNavigation = t.Bool(mainElement.FastNavigation),
                    DownloadEntities = t.Bool(mainElement.DownloadEntities),
                    OriginalId = mainElement.Id.ToString()
                };
                //MainElements never have parents ;)
                //used for non-MainElements
                //used for non-MainElements
                //used for non-MainElements
                //used for non-MainElements

                await cl.Create(db).ConfigureAwait(false);
                CheckListTranslation checkListTranslation = new CheckListTranslation()
                {
                    CheckListId = cl.Id,
                    Text = mainElement.Label.Split("|")[0],
                    LanguageId = language.Id
                };
                await checkListTranslation.Create(db).ConfigureAwait(false);
                if (mainElement.Label.Split("|").Length > 1)
                {
                    checkListTranslation = new CheckListTranslation()
                    {
                        CheckListId = cl.Id,
                        Text = mainElement.Label.Split("|")[1],
                        LanguageId = ukLanguage.Id
                    };
                    await checkListTranslation.Create(db).ConfigureAwait(false);
                }
                if (mainElement.Label.Split("|").Length > 2)
                {
                    checkListTranslation = new CheckListTranslation()
                    {
                        CheckListId = cl.Id,
                        Text = mainElement.Label.Split("|")[2],
                        LanguageId = deLanguage.Id
                    };
                    await checkListTranslation.Create(db).ConfigureAwait(false);
                }

                int mainId = cl.Id;
                mainElement.Id = mainId;
                #endregion

                await CreateElementList(mainId, mainElement.ElementList, language);

                return mainId;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        private async Task CreateElementList(int parentId, List<Element> lstElement, Language language)
        {
            foreach (Element element in lstElement)
            {
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    await CreateDataElement(parentId, dataE, language);
                }

                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    await CreateGroupElement(parentId, groupE, language);
                }
            }
        }

        //TODO
        private async Task CreateGroupElement(int parentId, GroupElement groupElement, Language language)
        {
            string methodName = "SqlController.CreateGroupElement";
            try
            {
                await using var db = GetContext();
                CheckList cl = new CheckList
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    // Label = groupElement.Label,
                    WorkflowState = Constants.Constants.WorkflowStates.Created,
                    ParentId = parentId,
                    Version = 1,
                    DisplayIndex = groupElement.DisplayOrder,
                    ReviewEnabled = t.Bool(groupElement.ReviewEnabled),
                    ExtraFieldsEnabled = t.Bool(groupElement.ExtraFieldsEnabled),
                    DoneButtonEnabled = t.Bool(groupElement.DoneButtonEnabled),
                    ApprovalEnabled = t.Bool(groupElement.ApprovalEnabled),
                    // Description = groupElement.Description != null ? groupElement.Description.InderValue : ""
                };
                await cl.Create(db).ConfigureAwait(false);
                Language ukLanguage = await db.Languages.SingleAsync(x => x.Name == "English").ConfigureAwait(false);
                Language deLanguage = await db.Languages.SingleAsync(x => x.Name == "German").ConfigureAwait(false);

                CheckListTranslation checkListTranslation = new CheckListTranslation()
                {
                    CheckListId = cl.Id,
                    LanguageId = language.Id,
                    Text = groupElement.Label.Split("|")[0],
                    Description = groupElement.Description != null ? groupElement.Description.InderValue.Split("|")[0] : ""
                };
                await checkListTranslation.Create(db).ConfigureAwait(false);
                if (groupElement.Label.Split("|").Length > 1)
                {
                    checkListTranslation = new CheckListTranslation()
                    {
                        CheckListId = cl.Id,
                        LanguageId = ukLanguage.Id,
                        Text = groupElement.Label.Split("|")[1],
                        Description = groupElement.Description != null ?
                            (groupElement.Description.InderValue.Split("|").Length > 1
                                ? groupElement.Description.InderValue.Split("|")[1] : "")
                            : "",
                    };
                    await checkListTranslation.Create(db).ConfigureAwait(false);
                }

                if (groupElement.Label.Split("|").Length > 2)
                {
                    checkListTranslation = new CheckListTranslation()
                    {
                        CheckListId = cl.Id,
                        LanguageId = deLanguage.Id,
                        Text = groupElement.Label.Split("|")[2],
                        Description = groupElement.Description != null ?
                            (groupElement.Description.InderValue.Split("|").Length > 2
                                ? groupElement.Description.InderValue.Split("|")[2] : "")
                            : "",
                    };
                    await checkListTranslation.Create(db).ConfigureAwait(false);
                }

                await CreateElementList(cl.Id, groupElement.ElementList, language).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        private async Task CreateDataElement(int parentId, DataElement dataElement, Language language)
        {
            string methodName = "SqlController.CreateDataElement";
            try
            {
                await using var db = GetContext();
                CheckList cl = new CheckList
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    // Label = dataElement.Label,
                    WorkflowState = Constants.Constants.WorkflowStates.Created,
                    ParentId = parentId,
                    Version = 1,
                    DisplayIndex = dataElement.DisplayOrder,
                    ReviewEnabled = t.Bool(dataElement.ReviewEnabled),
                    ExtraFieldsEnabled = t.Bool(dataElement.ExtraFieldsEnabled),
                    DoneButtonEnabled = t.Bool(dataElement.DoneButtonEnabled),
                    ApprovalEnabled = t.Bool(dataElement.ApprovalEnabled),
                    OriginalId = dataElement.Id.ToString(),
                    // Description = dataElement.Description != null
                    //     ? dataElement.Description.InderValue
                    //     : ""
                };
                await cl.Create(db).ConfigureAwait(false);

                Language ukLanguage = await db.Languages.SingleAsync(x => x.Name == "English");
                Language deLanguage = await db.Languages.SingleAsync(x => x.Name == "German");

                CheckListTranslation checkListTranslation = new CheckListTranslation()
                {
                    CheckListId = cl.Id,
                    Text = dataElement.Label.Split("|")[0],
                    Description = dataElement.Description != null
                        ? (dataElement.Description.InderValue != null ? dataElement.Description.InderValue.Split("|")[0] : "")
                        : "",
                    LanguageId = language.Id
                };
                await checkListTranslation.Create(db);

                if (dataElement.Label.Split("|").Length > 1)
                {
                    checkListTranslation = new CheckListTranslation()
                    {
                        CheckListId = cl.Id,
                        Text = dataElement.Label.Split("|")[1],
                        Description = dataElement.Description != null
                            ? (dataElement.Description.InderValue.Split("|").Length > 1
                                ? dataElement.Description.InderValue.Split("|")[1] : "")
                            : "",
                        LanguageId = ukLanguage.Id
                    };
                    await checkListTranslation.Create(db);
                }
                if (dataElement.Label.Split("|").Length > 2)
                {
                    checkListTranslation = new CheckListTranslation()
                    {
                        CheckListId = cl.Id,
                        Text = dataElement.Label.Split("|")[2],
                        Description = dataElement.Description != null
                            ? (dataElement.Description.InderValue.Split("|").Length > 2
                                ? dataElement.Description.InderValue.Split("|")[2] : "")
                            : "",
                        LanguageId = deLanguage.Id
                    };
                    await checkListTranslation.Create(db);
                }

                if (dataElement.DataItemList != null)
                {
                    foreach (DataItem dataItem in dataElement.DataItemList)
                    {
                        await CreateDataItem(null, cl.Id, dataItem, language);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }


        //TODO
//        private void CreateDataItemGroup(int elementId, FieldContainer fieldGroup)
//        {
//            try
//            {
//                await using (var db = GetContext())
//                {
//                    string typeStr = fieldGroup.GetType().ToString().Remove(0, 10); //10 = "eFormData.".Length
//                    int fieldTypeId = Find(typeStr);
//
//                    fields field = new fields();
//                    field.ParentFieldId = null;
//                    field.Color = fieldGroup.Color;
//                    //CDataValue description = new CDataValue();
//                    //description.InderValue = fieldGroup.Description;
//                    field.Description = fieldGroup.Description.InderValue;
//                    field.DisplayIndex = fieldGroup.DisplayOrder;
//                    field.Label = fieldGroup.Label;
//
//                    field.CreatedAt = DateTime.UtcNow;
//                    field.UpdatedAt = DateTime.UtcNow;
//                    field.WorkflowState = Constants.Constants.WorkflowStates.Created;
//                    field.CheckListId = elementId;
//                    field.FieldTypeId = fieldTypeId;
//                    field.Version = 1;
//
//                    field.DefaultValue = fieldGroup.Value;
//
//                    db.fields.Add(field);
//                    db.SaveChanges();
//
//                    db.field_versions.Add(MapFieldVersions(field));
//                    db.SaveChanges();
//
//                    if (fieldGroup.DataItemList != null)
//                    {
//                        foreach (DataItem dataItem in fieldGroup.DataItemList)
//                        {
//                            CreateDataItem(field.Id, elementId, dataItem);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("CreateDataItemGroup failed", ex);
//            }
//        }


        //TODO
        private async Task CreateDataItem(int? parentFieldId, int elementId, DataItem dataItem, Language language)
        {
            string methodName = "SqlController.CreateDataItem";
            try
            {
                await using var db = GetContext();
                string typeStr = dataItem.GetType().Name;

                /*
                     * Hack for making the FieldContainer work, since it's actually a FieldGroup
                     */
                if (typeStr.Equals("FieldContainer"))
                    typeStr = "FieldGroup";

                int fieldTypeId = Find(typeStr);

                Language ukLanguage = await db.Languages.SingleAsync(x => x.Name == "English").ConfigureAwait(false);
                Language deLanguage = await db.Languages.SingleAsync(x => x.Name == "German").ConfigureAwait(false);

                Data.Entities.Field field = new Data.Entities.Field
                {
                    Color = dataItem.Color,
                    ParentFieldId = parentFieldId,
                    // Description = dataItem.Description != null ? dataItem.Description.InderValue : "",
                    DisplayIndex = dataItem.DisplayOrder,
                    // Label = dataItem.Label,
                    Mandatory = t.Bool(dataItem.Mandatory),
                    ReadOnly = t.Bool(dataItem.ReadOnly),
                    Dummy = t.Bool(dataItem.Dummy),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    WorkflowState = Constants.Constants.WorkflowStates.Created,
                    CheckListId = elementId,
                    FieldTypeId = fieldTypeId,
                    Version = 1,
                    OriginalId = dataItem.Id.ToString()
                };

                bool isSaved = false; // This is done, because we need to have the current field Id, for giving it onto the child fields in a FieldGroup

                #region dataItem type

                FieldTranslation fieldTranslation;
                //KEY POINT - mapping
                switch (typeStr)
                {
                    case Constants.Constants.FieldTypes.Audio:
                        Audio audio = (Audio)dataItem;
                        field.Multi = audio.Multi;
                        break;

                    case Constants.Constants.FieldTypes.CheckBox:
                        CheckBox checkBox = (CheckBox)dataItem;
                        field.DefaultValue = checkBox.DefaultValue.ToString();
                        field.Selected = t.Bool(checkBox.Selected);
                        break;

                    case Constants.Constants.FieldTypes.Comment:
                        Comment comment = (Comment)dataItem;
                        field.DefaultValue = comment.Value;
                        field.MaxLength = comment.Maxlength;
                        field.Split = t.Bool(comment.Split);
                        break;

                    case Constants.Constants.FieldTypes.Date:
                        Date date = (Date)dataItem;
                        field.DefaultValue = date.DefaultValue;
                        field.MinValue = date.MinValue.ToString("yyyy-MM-dd");
                        field.MaxValue = date.MaxValue.ToString("yyyy-MM-dd");
                        break;

                    case Constants.Constants.FieldTypes.None:
                        break;

                    case Constants.Constants.FieldTypes.Number:
                        Number number = (Number)dataItem;
                        field.MinValue = number.MinValue.ToString();
                        field.MaxValue = number.MaxValue.ToString();
                        field.DefaultValue = number.DefaultValue.ToString();
                        field.DecimalCount = number.DecimalCount;
                        field.UnitName = number.UnitName;
                        break;

                    case Constants.Constants.FieldTypes.NumberStepper:
                        NumberStepper numberStepper = (NumberStepper)dataItem;
                        field.MinValue = numberStepper.MinValue.ToString();
                        field.MaxValue = numberStepper.MaxValue.ToString();
                        field.DefaultValue = numberStepper.DefaultValue.ToString();
                        field.DecimalCount = numberStepper.DecimalCount;
                        field.UnitName = numberStepper.UnitName;
                        break;

                    case Constants.Constants.FieldTypes.MultiSelect:
                        MultiSelect multiSelect = (MultiSelect)dataItem;
                        await field.Create(db).ConfigureAwait(false);
                        fieldTranslation = new FieldTranslation()
                        {
                            LanguageId = language.Id,
                            Text = dataItem.Label.Split("|")[0],
                            Description = dataItem.Description != null ? dataItem.Description.InderValue.Split("|")[0] : "",
                            FieldId = field.Id
                        };
                        await fieldTranslation.Create(db).ConfigureAwait(false);

                        if (dataItem.Label.Split("|").Length > 1)
                        {
                            fieldTranslation = new FieldTranslation()
                            {
                                LanguageId = ukLanguage.Id,
                                Text = dataItem.Label.Split("|")[1],
                                Description = dataItem.Description != null ?
                                    (dataItem.Description.InderValue.Split("|").Length > 1
                                        ? dataItem.Description.InderValue.Split("|")[1] : "")
                                    : "",
                                FieldId = field.Id
                            };
                            await fieldTranslation.Create(db).ConfigureAwait(false);
                        }

                        if (dataItem.Label.Split("|").Length > 2)
                        {
                            fieldTranslation = new FieldTranslation()
                            {
                                LanguageId = deLanguage.Id,
                                Text = dataItem.Label.Split("|")[2],
                                Description = dataItem.Description != null ?
                                    (dataItem.Description.InderValue.Split("|").Length > 2
                                        ? dataItem.Description.InderValue.Split("|")[2] : "")
                                    : "",
                                FieldId = field.Id
                            };
                            await fieldTranslation.Create(db).ConfigureAwait(false);
                        }
                        isSaved = true;

                        //fieldTranslation.FieldId = field.Id;
                        //await fieldTranslation.Create(db);

                        foreach (KeyValuePair keyValuePair in multiSelect.KeyValuePairList)
                        {
                            FieldOption fieldOption = new FieldOption()
                            {
                                FieldId = field.Id,
                                Key = keyValuePair.Key,
                                DisplayOrder = keyValuePair.DisplayOrder
                            };
                            await fieldOption.Create(db).ConfigureAwait(false);
                            FieldOptionTranslation fieldOptionTranslation = new FieldOptionTranslation()
                            {
                                FieldOptionId = fieldOption.Id,
                                LanguageId = language.Id,
                                Text = keyValuePair.Value.Split("|")[0]
                            };
                            await fieldOptionTranslation.Create(db).ConfigureAwait(false);
                            if (keyValuePair.Value.Split("|").Length > 1)
                            {
                                fieldOptionTranslation = new FieldOptionTranslation()
                                {
                                    FieldOptionId = fieldOption.Id,
                                    LanguageId = ukLanguage.Id,
                                    Text = keyValuePair.Value.Split("|")[1]
                                };
                                await fieldOptionTranslation.Create(db).ConfigureAwait(false);
                            }
                            if (keyValuePair.Value.Split("|").Length > 2)
                            {
                                fieldOptionTranslation = new FieldOptionTranslation()
                                {
                                    FieldOptionId = fieldOption.Id,
                                    LanguageId = deLanguage.Id,
                                    Text = keyValuePair.Value.Split("|")[2]
                                };
                                await fieldOptionTranslation.Create(db).ConfigureAwait(false);
                            }
                        }

                        // field.KeyValuePairList = PairBuild(multiSelect.KeyValuePairList);
                        break;

                    case Constants.Constants.FieldTypes.Picture:
                        Picture picture = (Picture)dataItem;
                        field.Multi = picture.Multi;
                        field.GeolocationEnabled = t.Bool(picture.GeolocationEnabled);
                        break;

                    case Constants.Constants.FieldTypes.SaveButton:
                        SaveButton saveButton = (SaveButton)dataItem;
                        field.DefaultValue = saveButton.Value;
                        break;

                    case Constants.Constants.FieldTypes.ShowPdf:
                        ShowPdf showPdf = (ShowPdf)dataItem;
                        field.DefaultValue = showPdf.Value;
                        break;

                    case Constants.Constants.FieldTypes.Signature:
                        break;

                    case Constants.Constants.FieldTypes.SingleSelect:
                        SingleSelect singleSelect = (SingleSelect)dataItem;
                        await field.Create(db).ConfigureAwait(false);
                        fieldTranslation = new FieldTranslation()
                        {
                            LanguageId = language.Id,
                            Text = dataItem.Label.Split("|")[0],
                            Description = dataItem.Description != null ? dataItem.Description.InderValue.Split("|")[0] : "",
                            FieldId = field.Id
                        };
                        await fieldTranslation.Create(db).ConfigureAwait(false);

                        if (dataItem.Label.Split("|").Length > 1)
                        {
                            fieldTranslation = new FieldTranslation()
                            {
                                LanguageId = ukLanguage.Id,
                                Text = dataItem.Label.Split("|")[1],
                                Description = dataItem.Description != null ?
                                    (dataItem.Description.InderValue.Split("|").Length > 1
                                        ? dataItem.Description.InderValue.Split("|")[1] : "")
                                    : "",
                                FieldId = field.Id
                            };
                            await fieldTranslation.Create(db).ConfigureAwait(false);
                        }

                        if (dataItem.Label.Split("|").Length > 2)
                        {
                            fieldTranslation = new FieldTranslation()
                            {
                                LanguageId = deLanguage.Id,
                                Text = dataItem.Label.Split("|")[2],
                                Description = dataItem.Description != null ?
                                    (dataItem.Description.InderValue.Split("|").Length > 2
                                        ? dataItem.Description.InderValue.Split("|")[2] : "")
                                    : "",
                                FieldId = field.Id
                            };
                            await fieldTranslation.Create(db).ConfigureAwait(false);
                        }
                        isSaved = true;

                        //fieldTranslation.FieldId = field.Id;
                        //await fieldTranslation.Create(db);

                        foreach (KeyValuePair keyValuePair in singleSelect.KeyValuePairList)
                        {
                            FieldOption fieldOption = new FieldOption()
                            {
                                FieldId = field.Id,
                                Key = keyValuePair.Key,
                                DisplayOrder = keyValuePair.DisplayOrder
                            };
                            await fieldOption.Create(db).ConfigureAwait(false);
                            FieldOptionTranslation fieldOptionTranslation = new FieldOptionTranslation()
                            {
                                FieldOptionId = fieldOption.Id,
                                LanguageId = language.Id,
                                Text = keyValuePair.Value.Split("|")[0]
                            };
                            await fieldOptionTranslation.Create(db).ConfigureAwait(false);
                            if (keyValuePair.Value.Split("|").Length > 1)
                            {
                                fieldOptionTranslation = new FieldOptionTranslation()
                                {
                                    FieldOptionId = fieldOption.Id,
                                    LanguageId = ukLanguage.Id,
                                    Text = keyValuePair.Value.Split("|")[1]
                                };
                                await fieldOptionTranslation.Create(db).ConfigureAwait(false);
                            }
                            if (keyValuePair.Value.Split("|").Length > 2)
                            {
                                fieldOptionTranslation = new FieldOptionTranslation()
                                {
                                    FieldOptionId = fieldOption.Id,
                                    LanguageId = deLanguage.Id,
                                    Text = keyValuePair.Value.Split("|")[2]
                                };
                                await fieldOptionTranslation.Create(db).ConfigureAwait(false);
                            }
                        }
                        // field.KeyValuePairList = PairBuild(singleSelect.KeyValuePairList);
                        break;

                    case Constants.Constants.FieldTypes.Text:
                        Text text = (Text)dataItem;
                        field.DefaultValue = text.Value;
                        field.MaxLength = text.MaxLength;
                        field.GeolocationEnabled = t.Bool(text.GeolocationEnabled);
                        field.GeolocationForced = t.Bool(text.GeolocationForced);
                        field.GeolocationHidden = t.Bool(text.GeolocationHidden);
                        field.BarcodeEnabled = t.Bool(text.BarcodeEnabled);
                        field.BarcodeType = text.BarcodeType;
                        break;

                    case Constants.Constants.FieldTypes.Timer:
                        Timer timer = (Timer)dataItem;
                        field.Split = t.Bool(timer.StopOnSave);
                        break;

                    //-------

                    case Constants.Constants.FieldTypes.EntitySearch:
                        EntitySearch entitySearch = (EntitySearch)dataItem;
                        field.EntityGroupId = entitySearch.EntityTypeId;
                        field.DefaultValue = entitySearch.DefaultValue.ToString();
                        field.IsNum = t.Bool(entitySearch.IsNum);
                        field.QueryType = entitySearch.QueryType;
                        field.MinValue = entitySearch.MinSearchLenght.ToString();
                        break;

                    case Constants.Constants.FieldTypes.EntitySelect:
                        EntitySelect entitySelect = (EntitySelect)dataItem;
                        field.EntityGroupId = entitySelect.Source;
                        field.DefaultValue = entitySelect.DefaultValue.ToString();
                        break;

                    case Constants.Constants.FieldTypes.FieldGroup:
                        FieldContainer fg = (FieldContainer)dataItem;
                        field.DefaultValue = fg.Value;
                        await field.Create(db).ConfigureAwait(false);

                        fieldTranslation = new FieldTranslation()
                        {
                            LanguageId = language.Id,
                            Text = dataItem.Label.Split("|")[0],
                            Description = dataItem.Description != null ? dataItem.Description.InderValue.Split("|")[0] : "",
                            FieldId = field.Id
                        };
                        await fieldTranslation.Create(db).ConfigureAwait(false);

                        if (dataItem.Label.Split("|").Length > 1)
                        {
                            fieldTranslation = new FieldTranslation()
                            {
                                LanguageId = ukLanguage.Id,
                                Text = dataItem.Label.Split("|")[1],
                                Description = dataItem.Description != null ?
                                    (dataItem.Description.InderValue.Split("|").Length > 1
                                        ? dataItem.Description.InderValue.Split("|")[1] : "")
                                    : "",
                                FieldId = field.Id
                            };
                            await fieldTranslation.Create(db).ConfigureAwait(false);
                        }

                        if (dataItem.Label.Split("|").Length > 2)
                        {
                            fieldTranslation = new FieldTranslation()
                            {
                                LanguageId = deLanguage.Id,
                                Text = dataItem.Label.Split("|")[2],
                                Description = dataItem.Description != null ?
                                    (dataItem.Description.InderValue.Split("|").Length > 2
                                        ? dataItem.Description.InderValue.Split("|")[2] : "")
                                    : "",
                                FieldId = field.Id
                            };
                            await fieldTranslation.Create(db).ConfigureAwait(false);
                        }
                        isSaved = true;

                        if (fg.DataItemList != null)
                        {
                            foreach (DataItem data_item in fg.DataItemList)
                            {
                                await CreateDataItem(field.Id, elementId, data_item, language);
                            }
                        }
                        break;

                    default:
                        throw new IndexOutOfRangeException(dataItem.GetType().ToString() + " is not a known/mapped DataItem type");
                }
                #endregion

                if (!isSaved)
                {
                    await field.Create(db).ConfigureAwait(false);
                    fieldTranslation = new FieldTranslation()
                    {
                        LanguageId = language.Id,
                        Text = dataItem.Label.Split("|")[0],
                        Description = dataItem.Description != null ? dataItem.Description.InderValue.Split("|")[0] : "",
                        FieldId = field.Id
                    };
                    await fieldTranslation.Create(db).ConfigureAwait(false);

                    if (dataItem.Label.Split("|").Length > 1)
                    {
                        fieldTranslation = new FieldTranslation()
                        {
                            LanguageId = ukLanguage.Id,
                            Text = dataItem.Label.Split("|")[1],
                            Description = dataItem.Description != null ?
                                (dataItem.Description.InderValue.Split("|").Length > 1
                                    ? dataItem.Description.InderValue.Split("|")[1] : "")
                                : "",
                            FieldId = field.Id
                        };
                        await fieldTranslation.Create(db).ConfigureAwait(false);
                    }

                    if (dataItem.Label.Split("|").Length > 2)
                    {
                        fieldTranslation = new FieldTranslation()
                        {
                            LanguageId = deLanguage.Id,
                            Text = dataItem.Label.Split("|")[2],
                            Description = dataItem.Description != null ?
                                (dataItem.Description.InderValue.Split("|").Length > 2
                                    ? dataItem.Description.InderValue.Split("|")[2] : "")
                                : "",
                            FieldId = field.Id
                        };
                        await fieldTranslation.Create(db).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region EformReadDb

        /// <summary>
        /// Gets element with specifik id from DB
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<Element> GetElement(int elementId, Language language)
        {
            string methodName = "SqlController.GetElement";
            try
            {
                await using var db = GetContext();
                Element element;

                //getting element's possible element children
                List<CheckList> lstElement = db.CheckLists.Where(x => x.ParentId == elementId).ToList();


                if (lstElement.Count > 0) //GroupElement
                {
                    //list for the DataItems
                    List<Element> lst = new List<Element>();

                    //the actual DataElement
                    try
                    {
                        CheckList cl = await db.CheckLists.SingleAsync(x => x.Id == elementId);
                        CheckListTranslation checkListTranslation =
                            await db.CheckListTranslations.SingleAsync(x => x.CheckListId == cl.Id);

                        GroupElement gElement = new GroupElement(cl.Id,
                            checkListTranslation.Text,
                            t.Int(cl.DisplayIndex),
                            checkListTranslation.Description,
                            t.Bool(cl.ApprovalEnabled),
                            t.Bool(cl.ReviewEnabled),
                            t.Bool(cl.DoneButtonEnabled),
                            t.Bool(cl.ExtraFieldsEnabled),
                            "",
                            t.Bool(cl.QuickSyncEnabled),
                            lst);

                        //the actual Elements
                        foreach (var subElement in lstElement)
                        {
                            lst.Add(await GetElement(subElement.Id, language));
                        }
                        element = gElement;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to find check_list with Id:" + elementId, ex);
                    }
                }
                else //DataElement
                {
                    //the actual DataElement
                    try
                    {
                        CheckList cl = await db.CheckLists.SingleAsync(x => x.Id == elementId);
                        CheckListTranslation checkListTranslation =
                            await db.CheckListTranslations.SingleOrDefaultAsync(x =>
                                x.CheckListId == cl.Id && x.LanguageId == language.Id);
                        if (checkListTranslation == null)
                        {
                            checkListTranslation = await db.CheckListTranslations.FirstAsync(x => x.CheckListId == cl.Id);
                            language =
                                await db.Languages.SingleAsync(x => x.Id == checkListTranslation.LanguageId);
                        }

                        DataElement dElement = new DataElement(cl.Id,
                            checkListTranslation.Text,
                            t.Int(cl.DisplayIndex),
                            checkListTranslation.Description,
                            t.Bool(cl.ApprovalEnabled),
                            t.Bool(cl.ReviewEnabled),
                            t.Bool(cl.DoneButtonEnabled),
                            t.Bool(cl.ExtraFieldsEnabled),
                            "",
                            t.Bool(cl.QuickSyncEnabled),
                            new List<DataItemGroup>(),
                            new List<DataItem>());

                        //the actual DataItems
                        List<Data.Entities.Field> lstFields = db.Fields.Where(x =>
                            x.CheckListId == elementId && x.ParentFieldId == null).ToList();
                        foreach (var field in lstFields)
                        {
                            await GetDataItem(dElement.DataItemList, dElement.DataItemGroupList, field, language);
                        }
                        element = dElement;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to find check_list with Id:" + elementId, ex);
                    }
                }
                return element;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Gets data item with specific id from DB
        /// </summary>
        /// <param name="lstDataItem"></param>
        /// <param name="lstDataItemGroup"></param>
        /// <param name="dataItemId"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        private async Task GetDataItem(List<DataItem> lstDataItem, List<DataItemGroup> lstDataItemGroup, Data.Entities.Field field, Language language)
        {
            string methodName = "SqlController.GetDataItem";
            try
            {
                await using var db = GetContext();
                //                    fields field = db.fields.SingleAsync(x => x.Id == dataItemId);
                string fieldTypeStr = Find(t.Int(field.FieldTypeId));

                //KEY POINT - mapping
                FieldTranslation fieldTranslation = await db.FieldTranslations.SingleOrDefaultAsync(x =>
                    x.FieldId == field.Id && x.LanguageId == language.Id);
                if (fieldTranslation == null)
                {
                    fieldTranslation = await db.FieldTranslations.FirstAsync(x => x.FieldId == field.Id);
                    language = await db.Languages.SingleAsync(x => x.Id == fieldTranslation.LanguageId);
                }
                switch (fieldTypeStr)
                {
                    case Constants.Constants.FieldTypes.Audio:
                        lstDataItem.Add(new Audio(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            t.Int(field.Multi)));
                        break;

                    case Constants.Constants.FieldTypes.CheckBox:
                        lstDataItem.Add(new CheckBox(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            t.Bool(field.DefaultValue), t.Bool(field.Selected)));
                        break;

                    case Constants.Constants.FieldTypes.Comment:
                        lstDataItem.Add(new Comment(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            field.DefaultValue, t.Int(field.MaxLength), t.Bool(field.Split)));
                        break;

                    case Constants.Constants.FieldTypes.Date:
                        lstDataItem.Add(new Date(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            DateTime.Parse(field.MinValue), DateTime.Parse(field.MaxValue), field.DefaultValue));
                        break;

                    case Constants.Constants.FieldTypes.None:
                        lstDataItem.Add(new None(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy)));
                        break;

                    case Constants.Constants.FieldTypes.Number:
                        lstDataItem.Add(new Number(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            field.MinValue, field.MaxValue, int.Parse(field.DefaultValue), t.Int(field.DecimalCount), field.UnitName));
                        break;

                    case Constants.Constants.FieldTypes.NumberStepper:
                        lstDataItem.Add(new NumberStepper(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            field.MinValue, field.MaxValue, int.Parse(field.DefaultValue), t.Int(field.DecimalCount), field.UnitName));
                        break;

                    case Constants.Constants.FieldTypes.MultiSelect:
                        var multiSelectFieldOptions =
                            await db.FieldOptions.Where(x => x.FieldId == field.Id)
                                .Join(db.FieldOptionTranslations,
                                option => option.Id,
                                translation => translation.FieldOptionId,
                                (option, translation) => new
                                {
                                    option.Key,
                                    option.DisplayOrder,
                                    translation.Text,
                                    translation.LanguageId
                                })
                                .Where(x => x.LanguageId == language.Id)
                                .Select(x => new KeyValuePair()
                            {
                                Key = x.Key,
                                Value = x.Text,
                                DisplayOrder = x.DisplayOrder
                            }).ToListAsync();

                        lstDataItem.Add(new MultiSelect(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            multiSelectFieldOptions));
                        break;

                    case Constants.Constants.FieldTypes.Picture:
                        lstDataItem.Add(new Picture(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            t.Int(field.Multi), t.Bool(field.GeolocationEnabled)));
                        break;

                    case Constants.Constants.FieldTypes.SaveButton:
                        string defaultValue = field.DefaultValue;
                        switch (field.DefaultValue.Split("|").Length)
                        {
                            case 1:
                                defaultValue = field.DefaultValue;
                                break;
                            case 2:
                                defaultValue = language.LanguageCode == "da" ? field.DefaultValue.Split("|")[0] : field.DefaultValue.Split("|")[1];
                                break;
                            case 3:
                                defaultValue = language.LanguageCode == "da" ? field.DefaultValue.Split("|")[0] :
                                    language.LanguageCode == "en-US" ? field.DefaultValue.Split("|")[1] :
                                    field.DefaultValue.Split("|")[2];
                                break;
                        }
                        lstDataItem.Add(new SaveButton(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            defaultValue));
                        break;

                    case Constants.Constants.FieldTypes.ShowPdf:
                        lstDataItem.Add(new ShowPdf(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            field.DefaultValue));
                        break;

                    case Constants.Constants.FieldTypes.Signature:
                        lstDataItem.Add(new Signature(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy)));
                        break;

                    case Constants.Constants.FieldTypes.SingleSelect:
                        var singleSelectFieldOptions =
                            await db.FieldOptions.Where(x => x.FieldId == field.Id)
                                .Join(db.FieldOptionTranslations,
                                    option => option.Id,
                                    translation => translation.FieldOptionId,
                                    (option, translation) => new
                                    {
                                        option.Key,
                                        option.DisplayOrder,
                                        translation.Text,
                                        translation.LanguageId
                                    })
                                .Where(x => x.LanguageId == language.Id)
                                .Select(x => new KeyValuePair()
                                {
                                    Key = x.Key,
                                    Value = x.Text,
                                    DisplayOrder = x.DisplayOrder
                                }).ToListAsync();

                        lstDataItem.Add(new SingleSelect(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            singleSelectFieldOptions));
                        break;

                    case Constants.Constants.FieldTypes.Text:
                        lstDataItem.Add(new Text(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            field.DefaultValue, t.Int(field.MaxLength), t.Bool(field.GeolocationEnabled), t.Bool(field.GeolocationForced), t.Bool(field.GeolocationHidden), t.Bool(field.BarcodeEnabled), field.BarcodeType));
                        break;

                    case Constants.Constants.FieldTypes.Timer:
                        lstDataItem.Add(new Timer(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            t.Bool(field.StopOnSave)));
                        break;

                    case Constants.Constants.FieldTypes.EntitySearch:
                        lstDataItem.Add(new EntitySearch(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            t.Int(field.DefaultValue), t.Int(field.EntityGroupId), t.Bool(field.IsNum), field.QueryType, t.Int(field.MinValue), t.Bool(field.BarcodeEnabled), field.BarcodeType));
                        break;

                    case Constants.Constants.FieldTypes.EntitySelect:
                        lstDataItem.Add(new EntitySelect(t.Int(field.Id), t.Bool(field.Mandatory), t.Bool(field.ReadOnly), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), t.Bool(field.Dummy),
                            t.Int(field.DefaultValue), t.Int(field.EntityGroupId)));
                        break;

                    case Constants.Constants.FieldTypes.FieldGroup:
                        List<DataItem> lst = new List<DataItem>();
                        //CDataValue description = new CDataValue();
                        //description.InderValue = f.description;
                        lstDataItemGroup.Add(new FieldGroup(field.Id.ToString(), fieldTranslation.Text, fieldTranslation.Description, field.Color, t.Int(field.DisplayIndex), field.DefaultValue, lst));
                        //lstDataItemGroup.Add(new DataItemGroup(f.Id.ToString(), f.label, f.description, f.color, t.Int(f.display_index), f.default_value, lst));

                        //the actual DataItems
                        List<Data.Entities.Field> lstFields = db.Fields.Where(x => x.ParentFieldId == field.Id).ToList();
                        foreach (var subField in lstFields)
                            await GetDataItem(lst, null, subField, language); //null, due to FieldGroup, CANT have fieldGroups under them
                        break;

                    default:
                        throw new IndexOutOfRangeException(field.FieldTypeId + " is not a known/mapped DataItem type");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        private void GetConverter()
        {
            string methodName = "SqlController.GetConverter";
            if (converter == null)
            {
                try
                {
                    using var db = GetContext();
                    converter = new List<Holder>();

                    List<FieldType> lstFieldType = db.FieldTypes.ToList();

                    foreach (var fieldType in lstFieldType)
                    {
                        converter.Add(new Holder(fieldType.Id, fieldType.Type));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(methodName + " failed", ex);
                }
            }
        }
        #endregion

        #region tags

        /// <summary>
        /// Gets all tags from DB
        /// </summary>
        /// <param name="includeRemoved"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Dto.Tag>> GetAllTags(bool includeRemoved)
        {
            string methodName = "SqlController.GetAllTags";
            List<Dto.Tag> tags = new List<Dto.Tag>();
            try
            {
                await using var db = GetContext();
                List<Tag> matches = null;
                if (!includeRemoved)
                    matches = await db.Tags.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToListAsync();
                else
                    matches = await db.Tags.ToListAsync();

                foreach (Tag tag in matches)
                {
                    Dto.Tag t = new Dto.Tag(tag.Id, tag.Name, tag.TaggingsCount);
                    tags.Add(t);
                }

                return tags;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Create tag with given name and saves it in DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> TagCreate(string name)
        {
            string methodName = "SqlController.TagCreate";
            try
            {
                await using var db = GetContext();
                Tag tag = await db.Tags.SingleOrDefaultAsync(x => x.Name == name);
                if (tag == null)
                {
                    tag = new Tag();
                    tag.Name = name;
                    await tag.Create(db).ConfigureAwait(false);
                    return tag.Id;
                } else
                {
                    tag.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    await tag.Update(db).ConfigureAwait(false);
                    return tag.Id;
                }

                //return ;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes tag with specific id from DB
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> TagDelete(int tagId)
        {
            string methodName = "SqlController.TagDelete";
            try
            {
                await using var db = GetContext();
                Tag tag = await db.Tags.SingleOrDefaultAsync(x => x.Id == tagId);
                if (tag != null)
                {
                    await tag.Delete(db);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region help methods

        /// <summary>
        /// Finds field type by id
        /// </summary>
        /// <param name="fieldTypeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string Find(int fieldTypeId)
        {
            foreach (var holder in converter)
            {
                if (holder.Index == fieldTypeId)
                    return holder.FieldType;
            }
            throw new Exception("Find failed. Not known fieldType for fieldTypeId: " + fieldTypeId);
        }

        /// <summary>
        /// Find field type by type string
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private int Find(string typeStr)
        {
            foreach (var holder in converter)
            {
                if (holder.FieldType == typeStr)
                    return holder.Index;
            }
            throw new Exception("Find failed. Not known fieldTypeId for typeStr: " + typeStr);
        }

        //TODO
        private string PairBuild(List<KeyValuePair> lst)
        {
            string str = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><hash>";
            string inderStr;
            int index = 1;

            foreach (KeyValuePair kVP in lst)
            {
                inderStr = "<" + index + ">";

                inderStr += "<key>" + kVP.Value + "</key>";
                inderStr += "<selected>" + kVP.Selected + "</selected>";
                inderStr += "<displayIndex>" + kVP.DisplayOrder + "</displayIndex>";

                inderStr += "</" + index + ">";

                str += inderStr;
                index += 1;
            }

            str += "</hash>";
            return str;
        }

        //TODO
        public List<KeyValuePair> PairRead(string str)
        {
            List<KeyValuePair> list = new List<KeyValuePair>();
            str = t.Locate(str, "<hash>", "</hash>");

            bool flag = true;
            int index = 1;
            string keyValue, displayIndex;
            bool selected;

            while (flag)
            {
                string inderStr = t.Locate(str, "<" + index + ">", "</" + index + ">");

                keyValue = t.Locate(inderStr, "<key>", "</");
                //try
                //{
                    selected = bool.Parse(t.Locate(inderStr.ToLower(), "<selected>", "</"));
                //}
                // catch (Exception ex)
                // {
                //     log.LogCritical("bla bla", ex.Message);
                //     selected = false;
                // }

                displayIndex = t.Locate(inderStr, "<displayIndex>", "</");

                list.Add(new KeyValuePair(index.ToString(), keyValue, selected, displayIndex));

                index += 1;

                if (t.Locate(str, "<" + index + ">", "</" + index + ">") == "")
                    flag = false;
            }

            return list;
        }

        //ToDo
        private string PairMatch(List<KeyValuePair> keyValuePairs, string match)
        {
            foreach (var item in keyValuePairs)
            {
                if (item.Key == match)
                    return item.Value;
            }

            return null;
        }
        #endregion


        #endregion

        /// <summary>
        /// Adding field type to DB
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="fieldType"></param>
        /// <param name="description"></param>
        private async Task FieldTypeAdd(int Id, string fieldType, string description)
        {
            await using var db = GetContext();
            if (db.FieldTypes.Count(x => x.Type == fieldType) == 0)
            {
                FieldType fT = new FieldType {Type = fieldType, Description = description};
                await fT.Create(db).ConfigureAwait(false);
            }
        }
    }
}