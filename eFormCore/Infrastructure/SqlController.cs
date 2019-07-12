/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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
using eFormShared;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;
using Microting.eForm.Infrastructure.Models.reply;
//using eFormSqlController.Migrations;

namespace Microting.eForm.Infrastructure
{
    public class SqlController : LogWriter
    {
        #region var
        string connectionStr;       
        Log log;
        Tools t = new Tools();
        List<Holder> converter;

        object _lockQuery = new object();
        object _lockWrite = new object();
        #endregion

        #region con
        public SqlController(string connectionString)
        {
            connectionStr = connectionString;          

            #region migrate if needed
            try
            {
                using (var db = GetContext())
                {
                    //TODO! THIS part need to be redone in some form in EF Core!
                   
                    db.Database.Migrate();
                    db.Database.EnsureCreated();

                   var match = db.settings.Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
//                Console.WriteLine(ex.InnerException.Message);
                throw ex;
                //TODO! THIS part need to be redone in some form in EF Core!
               // MigrateDb();
            }
            #endregion

            //region set default for settings if needed
            if (SettingCheckAll().Count > 0)
                SettingCreateDefaults();
        }

        private MicrotingDbAnySql GetContext()
        {

            DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();

            if (connectionStr.ToLower().Contains("convert zero datetime"))
            {
                dbContextOptionsBuilder.UseMySql(connectionStr);
            }
            else
            {
                dbContextOptionsBuilder.UseSqlServer(connectionStr);
            }
            dbContextOptionsBuilder.UseLazyLoadingProxies(true);
            return new MicrotingDbAnySql(dbContextOptionsBuilder.Options);

        }

        public bool MigrateDb()
        {
            //var configuration = new Configuration();
            //configuration.TargetDatabase = new DbConnectionInfo(connectionStr, "System.Data.SqlClient");
            //var migrator = new DbMigrator(configuration);

            //migrator.Update();
            return true;
        }
        #endregion

        #region public
        #region public template
        
       //TODO
        public int TemplateCreate(MainElement mainElement)
        {
            try
            {
                int Id = EformCreateDb(mainElement);
                return Id;
            }
            catch (Exception ex)
            {
                throw new Exception("TemplatCreate failed", ex);
            }
        }

        //TODO
        public MainElement TemplateRead(int templateId)
        {
            try
            {
                using (var db = GetContext())
                {
                    MainElement mainElement = null;
                    GetConverter();

                    check_lists mainCl = db.check_lists.SingleOrDefault(x => x.Id == templateId && (x.ParentId == null || x.ParentId == 0));

                    if (mainCl == null)
                        return null;

                    mainElement = new MainElement(mainCl.Id, mainCl.Label, t.Int(mainCl.DisplayIndex), mainCl.FolderName, t.Int(mainCl.Repeated), DateTime.Now, DateTime.Now.AddDays(2), "da",
                        t.Bool(mainCl.MultiApproval), t.Bool(mainCl.FastNavigation), t.Bool(mainCl.DownloadEntities), t.Bool(mainCl.ManualSync), mainCl.CaseType, "", "", t.Bool(mainCl.QuickSyncEnabled), new List<Element>(), mainCl.Color);

                    //getting elements
                    List<check_lists> lst = db.check_lists.Where(x => x.ParentId == templateId).ToList();
                    foreach (check_lists cl in lst)
                    {
                        mainElement.ElementList.Add(GetElement(cl.Id));
                    }

                    //return
                    return mainElement;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TemplatRead failed", ex);
            }
        }

        //TODO
        public Template_Dto TemplateItemRead(int templateId)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists checkList = db.check_lists.SingleOrDefault(x => x.Id == templateId);

                    if (checkList == null)
                        return null;

                    List<SiteName_Dto> sites = new List<SiteName_Dto>();
                    foreach (check_list_sites check_list_site in checkList.CheckListSites.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToList())
                    {
                        SiteName_Dto site = new SiteName_Dto((int)check_list_site.Site.MicrotingUid, check_list_site.Site.Name, check_list_site.Site.CreatedAt, check_list_site.Site.UpdatedAt);
                        sites.Add(site);
                    }
                    bool hasCases = false;
                    if (checkList.Cases.Count() > 0)
                        hasCases = true;
                    #region load fields
                    Field_Dto fd1 = null;
                    Field_Dto fd2 = null;
                    Field_Dto fd3 = null;
                    Field_Dto fd4 = null;
                    Field_Dto fd5 = null;
                    Field_Dto fd6 = null;
                    Field_Dto fd7 = null;
                    Field_Dto fd8 = null;
                    Field_Dto fd9 = null;
                    Field_Dto fd10 = null;

                    fields f1 = db.fields.SingleOrDefault(x => x.Id == checkList.Field1);
                    if (f1 != null)
                        fd1 = new Field_Dto(f1.Id, f1.Label, f1.Description, (int)f1.FieldTypeId, f1.FieldType.FieldType, (int)f1.CheckListId);

                    fields f2 = db.fields.SingleOrDefault(x => x.Id == checkList.Field2);
                    if (f2 != null)
                        fd2 = new Field_Dto(f2.Id, f2.Label, f2.Description, (int)f2.FieldTypeId, f2.FieldType.FieldType, (int)f2.CheckListId);

                    fields f3 = db.fields.SingleOrDefault(x => x.Id == checkList.Field3);
                    if (f3 != null)
                        fd3 = new Field_Dto(f3.Id, f3.Label, f3.Description, (int)f3.FieldTypeId, f3.FieldType.FieldType, (int)f3.CheckListId);

                    fields f4 = db.fields.SingleOrDefault(x => x.Id == checkList.Field4);
                    if (f4 != null)
                        fd4 = new Field_Dto(f4.Id, f4.Label, f4.Description, (int)f4.FieldTypeId, f4.FieldType.FieldType, (int)f4.CheckListId);

                    fields f5 = db.fields.SingleOrDefault(x => x.Id == checkList.Field5);
                    if (f5 != null)
                        fd5 = new Field_Dto(f5.Id, f5.Label, f5.Description, (int)f5.FieldTypeId, f5.FieldType.FieldType, (int)f5.CheckListId);

                    fields f6 = db.fields.SingleOrDefault(x => x.Id == checkList.Field6);
                    if (f6 != null)
                        fd6 = new Field_Dto(f6.Id, f6.Label, f6.Description, (int)f6.FieldTypeId, f6.FieldType.FieldType, (int)f6.CheckListId);

                    fields f7 = db.fields.SingleOrDefault(x => x.Id == checkList.Field7);
                    if (f7 != null)
                        fd7 = new Field_Dto(f7.Id, f7.Label, f7.Description, (int)f7.FieldTypeId, f7.FieldType.FieldType, (int)f7.CheckListId);

                    fields f8 = db.fields.SingleOrDefault(x => x.Id == checkList.Field8);
                    if (f8 != null)
                        fd8 = new Field_Dto(f8.Id, f8.Label, f8.Description, (int)f8.FieldTypeId, f8.FieldType.FieldType, (int)f8.CheckListId);

                    fields f9 = db.fields.SingleOrDefault(x => x.Id == checkList.Field9);
                    if (f9 != null)
                        fd9 = new Field_Dto(f9.Id, f9.Label, f9.Description, (int)f9.FieldTypeId, f9.FieldType.FieldType, (int)f9.CheckListId);

                    fields f10 = db.fields.SingleOrDefault(x => x.Id == checkList.Field10);
                    if (f10 != null)
                        fd10 = new Field_Dto(f10.Id, f10.Label, f10.Description, (int)f10.FieldTypeId, f10.FieldType.FieldType, (int)f10.CheckListId);
                    #endregion

                    #region loadtags
                    List<taggings> matches = checkList.Taggings.ToList();
                    List<KeyValuePair<int, string>> check_list_tags = new List<KeyValuePair<int, string>>();
                    foreach (taggings tagging in matches)
                    {
                        KeyValuePair<int, string> kvp = new KeyValuePair<int, string>((int)tagging.TagId, tagging.Tag.Name);
                        check_list_tags.Add(kvp);
                    }
                    #endregion

                    Template_Dto templateDto = new Template_Dto(checkList.Id, checkList.CreatedAt, checkList.UpdatedAt, checkList.Label, checkList.Description, (int)checkList.Repeated, checkList.FolderName, checkList.WorkflowState, sites, hasCases, checkList.DisplayIndex, fd1, fd2, fd3, fd4, fd5, fd6, fd7, fd8, fd9, fd10, check_list_tags);
                    return templateDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
            }
        }

        //TODO
        public List<Template_Dto> TemplateItemReadAll(bool includeRemoved, string siteWorkflowState, string searchKey, bool descendingSort, string sortParameter, List<int> tagIds)
        {
            string methodName = t.GetMethodName("SQLController");
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(includeRemoved), includeRemoved);
            log.LogVariable(methodName, nameof(searchKey), searchKey);
            log.LogVariable(methodName, nameof(descendingSort), descendingSort);
            log.LogVariable(methodName, nameof(sortParameter), sortParameter);
            log.LogVariable(methodName, nameof(tagIds), tagIds.ToString());
            try
            {
                List<Template_Dto> templateList = new List<Template_Dto>();

                using (var db = GetContext())
                {
                    List<check_lists> matches = null;
                    IQueryable<check_lists> sub_query = db.check_lists.Where(x => x.ParentId == null);

                    if (!includeRemoved)
                        sub_query = sub_query.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                    if (searchKey != null && searchKey != "")
                    {
                        sub_query = sub_query.Where(x => x.Label.Contains(searchKey) || x.Description.Contains(searchKey));
                    }

                    if (tagIds.Count > 0)
                    {
                        List<int?> check_list_ids = db.taggings.Where(x => tagIds.Contains((int)x.TagId)).Select(x => x.CheckListId).ToList();
                        if (check_list_ids != null)
                        {
                            sub_query = sub_query.Where(x => check_list_ids.Contains(x.Id));
                        }
                        
                    }


                    switch (sortParameter)
                    {
                        case Constants.Constants.eFormSortParameters.Label:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.Label);
                            else
                                sub_query = sub_query.OrderBy(x => x.Label);
                            break;
                        case Constants.Constants.eFormSortParameters.Description:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.Description);
                            else
                                sub_query = sub_query.OrderBy(x => x.Description);
                            break;
                        case Constants.Constants.eFormSortParameters.CreatedAt:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.CreatedAt);
                            else
                                sub_query = sub_query.OrderBy(x => x.CreatedAt);
                            break;
                        case Constants.Constants.eFormSortParameters.Tags:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.CreatedAt);
                            else
                                sub_query = sub_query.OrderBy(x => x.CreatedAt);
                            break;
                        default:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.Id);
                            else
                                sub_query = sub_query.OrderBy(x => x.Id);
                            break;
                    }

                    matches = sub_query.ToList();

                    foreach (check_lists checkList in matches)
                    {
                        List<SiteName_Dto> sites = new List<SiteName_Dto>();
                        List<check_list_sites> check_list_sites = null;

                        if (siteWorkflowState == Constants.Constants.WorkflowStates.Removed)
                            check_list_sites = checkList.CheckListSites.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed).ToList();
                        else
                            check_list_sites = checkList.CheckListSites.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToList();

                        foreach (check_list_sites check_list_site in check_list_sites)
                        {
                            try
                            {
                                SiteName_Dto site = new SiteName_Dto((int)check_list_site.Site.MicrotingUid, check_list_site.Site.Name, check_list_site.Site.CreatedAt, check_list_site.Site.UpdatedAt);
                                sites.Add(site);
                            } catch (Exception innerEx)
                            {
                                log.LogException(methodName, "could not add the site to the sites for site.Id : " + check_list_site.Site.Id.ToString() + " and got exception : " + innerEx.Message, innerEx, false);
                                throw new Exception("Error adding site to sites for site.Id : " + check_list_site.Site.Id.ToString(), innerEx);
                            }
                            
                        }
                        bool hasCases = false;
                        if (checkList.Cases.Count() > 0)
                            hasCases = true;

                        #region loadtags
                        List<taggings> tagging_matches = checkList.Taggings.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToList();
                        List<KeyValuePair<int, string>> check_list_tags = new List<KeyValuePair<int, string>>();
                        foreach (taggings tagging in tagging_matches)
                        {
                            KeyValuePair<int, string> kvp = new KeyValuePair<int, string>((int)tagging.TagId, tagging.Tag.Name);
                            check_list_tags.Add(kvp);
                        }
                        #endregion
                        try
                        {
                            Template_Dto templateDto = new Template_Dto(checkList.Id, checkList.CreatedAt, checkList.UpdatedAt, checkList.Label, checkList.Description, (int)checkList.Repeated, checkList.FolderName, checkList.WorkflowState, sites, hasCases, checkList.DisplayIndex, check_list_tags);
                            templateList.Add(templateDto);
                        }
                        catch (Exception innerEx)
                        {
                            log.LogException(methodName, "could not add the templateDto to the templateList for templateId : " + checkList.Id.ToString() + " and got exception : " + innerEx.Message, innerEx, false);
                            throw new Exception("Error adding template to templateList for templateId : " + checkList.Id.ToString(), innerEx);
                        }
                    }
                    return templateList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TemplateSimpleReadAll failed", ex);
            }


        }

        //TODO
        public List<Field_Dto> TemplateFieldReadAll(int templateId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    MainElement mainElement = TemplateRead(templateId);
                    List<Field_Dto> fieldLst = new List<Field_Dto>();

                    foreach (DataItem dataItem in mainElement.DataItemGetAll())
                    {
                        fields field = db.fields.Single(x => x.Id == dataItem.Id);
                        Field_Dto fieldDto = new Field_Dto(field.Id, field.Label, field.Description, (int)field.FieldTypeId, field.FieldType.FieldType, (int)field.CheckListId);
                        if (field.ParentFieldId != null)
                        {
                            fieldDto.ParentName = db.fields.Where(x => x.Id == field.ParentFieldId).First().Label;
                        }
                        else
                        {
                            fieldDto.ParentName = field.CheckList.Label;
                        }
                        fieldLst.Add(fieldDto);
                    }


                    return fieldLst;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public bool TemplateDisplayIndexChange(int templateId, int newDisplayIndex)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists checkList = db.check_lists.SingleOrDefault(x => x.Id == templateId);

                    if (checkList == null)
                        return false;

                    checkList.DisplayIndex = newDisplayIndex;

                    checkList.Update(db);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
            }
        }

        //TODO
        public bool TemplateUpdateFieldIdsForColumns(int templateId, int? fieldId1, int? fieldId2, int? fieldId3, int? fieldId4, int? fieldId5, int? fieldId6, int? fieldId7, int? fieldId8, int? fieldId9, int? fieldId10)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists checkList = db.check_lists.SingleOrDefault(x => x.Id == templateId);

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
                    
                    checkList.Update(db);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
            }
        }

        /// <summary>
        /// Deletes Template from DB with given templateId
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool TemplateDelete(int templateId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    check_lists check_list = db.check_lists.Single(x => x.Id == templateId);

                    if (check_list != null)
                    {
                        check_list.Delete((db));

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public bool TemplateSetTags(int templateId, List<int> tagIds)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    check_lists checkList = db.check_lists.Single(x => x.Id == templateId);

                    if (checkList != null)
                    {
                        // Delete all not wanted taggings first
                        List<taggings> clTaggings = checkList.Taggings.Where(x => !tagIds.Contains((int)x.TagId)).ToList();
//                        int index = 0;
                        foreach (taggings tagging in clTaggings)
                        {
                            taggings currentTagging = db.taggings.Single(x => x.Id == tagging.Id);
                            if (currentTagging != null)
                            {
                                currentTagging.Delete(db);
                            }
                        }

                        // set all new taggings
                        foreach (int Id in tagIds)
                        {
                            tags tag = db.tags.Single(x => x.Id == Id);
                            if (tag != null)
                            {
                                taggings currentTagging = db.taggings.SingleOrDefault(x => x.TagId == tag.Id && x.CheckListId == templateId);

                                if (currentTagging == null)
                                {
                                    taggings tagging = new taggings();
                                    tagging.CheckListId = templateId;
                                    tagging.TagId = tag.Id;

                                    tagging.Create(db);
                                } else {
                                    if (currentTagging.WorkflowState != Constants.Constants.WorkflowStates.Created)
                                    {
                                        currentTagging.WorkflowState = Constants.Constants.WorkflowStates.Created;
                                        currentTagging.Update(db);
                                    }
                                }                                
                            }
                        }

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public void SetJasperExportEnabled(int eFormId, bool isEnabled)
        {
            string methodName = t.GetMethodName("SQLController"); 
            using (var db = GetContext())
            {
                check_lists checkList = db.check_lists.SingleOrDefault(x => x.Id == eFormId);
                if (checkList != null)
                {
                    checkList.JasperExportEnabled = isEnabled;
                    checkList.Update(db);
                }
            }
        }

        public void SetDocxExportEnabled(int eFormId, bool isEnabled)
        {
            string methodName = t.GetMethodName("SQLController"); 
            using (var db = GetContext())
            {
                check_lists checkList = db.check_lists.SingleOrDefault(x => x.Id == eFormId);
                if (checkList != null)
                {
                    checkList.DocxExportEnabled = isEnabled;
                    checkList.Update(db);
                }
            }
        }
        
        #endregion

        #region public (pre)case

        //TODO
        public void CheckListSitesCreate(int checkListId, int siteUId, string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    int siteId = db.sites.Single(x => x.MicrotingUid == siteUId).Id;

                    check_list_sites cLS = new check_list_sites();
                    cLS.CheckListId = checkListId;
                    cLS.CreatedAt = DateTime.Now;
                    cLS.UpdatedAt = DateTime.Now;
                    cLS.LastCheckId = "0";
                    cLS.MicrotingUid = microtingUId;
                    cLS.SiteId = siteId;
                    cLS.Version = 1;
                    cLS.WorkflowState = Constants.Constants.WorkflowStates.Created;

                    db.check_list_sites.Add(cLS);
                    db.SaveChanges();

                    db.check_list_site_versions.Add(MapCheckListSiteVersions(cLS));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckListSitesCreate failed", ex);
            }
        }

        //TODO
        public List<string> CheckListSitesRead(int templateId, int microtingUid, string workflowState)
        {
            try
            {
                using (var db = GetContext())
                {
                    sites site = db.sites.Single(x => x.MicrotingUid == microtingUid);
                    IQueryable<check_list_sites> sub_query = db.check_list_sites.Where(x => x.SiteId == site.Id && x.CheckListId == templateId);
                    if (workflowState == Constants.Constants.WorkflowStates.NotRemoved)
                        sub_query = sub_query.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed);

                    return sub_query.Select(x => x.MicrotingUid).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckListSitesRead failed", ex);
            }
        }

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
        public int CaseCreate(int checkListId, int siteUId, string microtingUId, string microtingCheckId, string caseUId, string custom, DateTime createdAt)
        {
            try
            {
                using (var db = GetContext())
                {
                    string caseType = db.check_lists.Single(x => x.Id == checkListId).CaseType;
                    int siteId = db.sites.Single(x => x.MicrotingUid == siteUId).Id;

                    cases aCase = null;
                    // Lets see if we have an existing case with the same parameters in the db first.
                    // This is to handle none gracefull shutdowns.                
                    aCase = db.cases.SingleOrDefault(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == microtingCheckId);

                    if (aCase == null)
                    {
                        aCase = new cases();
                        aCase.Status = 66;
                        aCase.Type = caseType;
                        aCase.CreatedAt = createdAt;
                        aCase.UpdatedAt = createdAt;
                        aCase.CheckListId = checkListId;
                        aCase.MicrotingUid = microtingUId;
                        aCase.MicrotingCheckUid = microtingCheckId;
                        aCase.CaseUid = caseUId;
                        aCase.WorkflowState = Constants.Constants.WorkflowStates.Created;
                        aCase.Version = 1;
                        aCase.SiteId = siteId;

                        aCase.Custom = custom;

                        db.cases.Add(aCase);
                        db.SaveChanges();

                        db.case_versions.Add(MapCaseVersions(aCase));
                        db.SaveChanges();
                    }
                    else
                    {
                        aCase.Status = 66;
                        aCase.Type = caseType;
                        aCase.CheckListId = checkListId;
                        aCase.MicrotingUid = microtingUId;
                        aCase.MicrotingCheckUid = microtingCheckId;
                        aCase.CaseUid = caseUId;
                        aCase.WorkflowState = Constants.Constants.WorkflowStates.Created;
                        aCase.Version = 1;
                        aCase.SiteId = siteId;
                        aCase.UpdatedAt = DateTime.Now;
                        aCase.Version = aCase.Version + 1;
                        aCase.Custom = custom;

                        db.case_versions.Add(MapCaseVersions(aCase));
                        db.SaveChanges();
                    }

                    return aCase.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseCreate failed", ex);
            }
        }

        /// <summary>
        /// Reads case from DB with given microtingUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string CaseReadLastCheckIdByMicrotingUId(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_list_sites match = db.check_list_sites.SingleOrDefault(x => x.MicrotingUid == microtingUId);
                    if (match == null)
                        return null;
                    else
                        return match.LastCheckId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByMuuId failed", ex);
            }
        }

        //TODO
        public void CaseUpdateRetrived(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases match = db.cases.SingleOrDefault(x => x.MicrotingUid == microtingUId);

                    if (match != null)
                    {
                        match.Status = 77;
                        match.UpdatedAt = DateTime.Now;
                        match.Version = match.Version + 1;

                        db.case_versions.Add(MapCaseVersions(match));
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdateRetrived failed", ex);
            }
        }

        //TODO
        public void CaseUpdateCompleted(string microtingUId, string microtingCheckId, DateTime doneAt, int workerMicrotingUId, int unitMicrotingUid)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases caseStd = db.cases.SingleOrDefault(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == microtingCheckId);

                    if (caseStd == null)
                        caseStd = db.cases.Single(x => x.MicrotingUid == microtingUId);

                    int userId = db.workers.Single(x => x.MicrotingUid == workerMicrotingUId).Id;
                    int unitId = db.units.Single(x => x.MicrotingUid == unitMicrotingUid).Id;

                    caseStd.Status = 100;
                    caseStd.DoneAt = doneAt;
                    caseStd.UpdatedAt = DateTime.Now;
                    caseStd.WorkerId = userId;
                    caseStd.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    caseStd.Version = caseStd.Version + 1;
                    caseStd.UnitId = unitId;
                    caseStd.MicrotingCheckUid = microtingCheckId;
                    #region - update "check_list_sites" if needed
                    check_list_sites match = db.check_list_sites.SingleOrDefault(x => x.MicrotingUid == microtingUId);
                    if (match != null)
                    {
                        match.LastCheckId = microtingCheckId;
                        match.Version = match.Version + 1;
                        match.UpdatedAt = DateTime.Now;

                        db.SaveChanges();

                        db.check_list_site_versions.Add(MapCheckListSiteVersions(match));
                        db.SaveChanges();
                    }
                    #endregion

                    db.case_versions.Add(MapCaseVersions(caseStd));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdateCompleted failed", ex);
            }
        }

        //TODO
        public void CaseRetract(string microtingUId, string microtingCheckId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases match = db.cases.Single(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == microtingCheckId);

                    match.UpdatedAt = DateTime.Now;
                    match.WorkflowState = Constants.Constants.WorkflowStates.Retracted;
                    match.Version = match.Version + 1;

                    db.case_versions.Add(MapCaseVersions(match));
                    db.SaveChanges();
                }   
            }
            catch (Exception ex)
            {
                throw new Exception("CaseRetract failed", ex);
            }
        }

        /// <summary>
        /// Deletes Case from DB with given microtingUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CaseDelete(string microtingUId)

        {
            try
            {
                using (var db = GetContext())
                {
                    List<cases> matches = db.cases.Where(x => x.MicrotingUid == microtingUId && x.WorkflowState != Constants.Constants.WorkflowStates.Removed && x.WorkflowState != Constants.Constants.WorkflowStates.Retracted).ToList();
                    if (matches.Count() > 1)
                    {
                        return false;
                    }
                    if (matches != null && matches.Count() == 1)
                    {
                        cases aCase = matches.First();
                        if (aCase.WorkflowState != Constants.Constants.WorkflowStates.Removed)
                        {
                            aCase.UpdatedAt = DateTime.Now;
                            aCase.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                            aCase.Version = aCase.Version + 1;

                            db.case_versions.Add(MapCaseVersions(aCase));
                            db.SaveChanges();
                        }
                        log.LogStandard(t.GetMethodName("SQLController"), "Case successfully marked as removed for microtingUId " + microtingUId);
                        return true;
                    } else
                    {
                        //log.LogStandard(t.GetMethodName("SQLController"), "Could not find a case with microtingUId " + microtingUId);
                        return false;
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseDelete failed", ex);
            }
        }

        //TODO
        public bool CaseDeleteResult(int caseId)
        {

            try
            {
                using (var db = GetContext())
                {
                    cases aCase = db.cases.SingleOrDefault(x => x.Id == caseId);

                    if (aCase != null)
                    {
                        aCase.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                        aCase.UpdatedAt = DateTime.Now;
                        aCase.Version = aCase.Version + 1;

                        db.case_versions.Add(MapCaseVersions(aCase));
                        db.SaveChanges();

                        return true;
                    } else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseDelete failed", ex);
            }
        }

        //TODO
        public void CaseDeleteReversed(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {

                    check_list_sites site = db.check_list_sites.Single(x => x.MicrotingUid == microtingUId);

                    site.UpdatedAt = DateTime.Now;
                    site.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                    site.Version = site.Version + 1;

                    db.check_list_site_versions.Add(MapCheckListSiteVersions(site));
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseDelete failed", ex);
            }
        }
        #endregion

        #region public "reply"
        #region check
        
        //TODO
        public List<int> ChecksCreate(Response response, string xmlString, int xmlIndex)
        {
            List<int> uploadedDataIds = new List<int>();
            try
            {
                using (var db = GetContext())
                {
                    int elementId;
                    int userUId = int.Parse(response.Checks[xmlIndex].WorkerId);
                    int userId = db.workers.Single(x => x.MicrotingUid == userUId).Id;
                    List<string> elements = t.LocateList(xmlString, "<ElementList>", "</ElementList>");
                    List<Field_Dto> TemplatFieldLst = null;
                    cases responseCase = null;
                    List<int?> case_fields = new List<int?>();
                    List<int> fieldTypeIds = db.field_types.Where(x => x.FieldType == Constants.Constants.FieldTypes.Picture || x.FieldType == Constants.Constants.FieldTypes.Signature || x.FieldType == Constants.Constants.FieldTypes.Audio).Select(x => x.Id).ToList();

                    try //if a reversed case, case needs to be created
                    {
                        check_list_sites cLS = db.check_list_sites.Single(x => x.MicrotingUid == response.Value);
                        int caseId = CaseCreate((int)cLS.CheckListId, (int)cLS.Site.MicrotingUid, response.Value, response.Checks[xmlIndex].Id, "ReversedCase", "", DateTime.Now);
                        responseCase = db.cases.Single(x => x.Id == caseId);
                    }
                    catch //already created case Id retrived
                    {
                        responseCase = db.cases.Single(x => x.MicrotingUid == response.Value);
                    }

                    check_lists cl = responseCase.CheckList;

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
                    //cl.field_1

                    TemplatFieldLst = TemplateFieldReadAll((int)responseCase.CheckListId);

                    foreach (string elementStr in elements)
                    {
                        #region foreach element

                        int cl_id = int.Parse(t.Locate(elementStr, "<Id>", "</"));
                        int case_id = responseCase.Id;

                        check_list_values clv = null;
                        clv = db.check_list_values.SingleOrDefault(x => x.CheckListId == cl_id && x.CaseId == case_id);

                        if (clv == null)
                        {
                            clv = new check_list_values();
                            clv.CreatedAt = DateTime.Now;
                            clv.UpdatedAt = DateTime.Now;
                            clv.CheckListId = int.Parse(t.Locate(elementStr, "<Id>", "</"));
                            clv.CaseId = responseCase.Id;
                            clv.Status = t.Locate(elementStr, "<Status>", "</");
                            clv.Version = 1;
                            clv.UserId = userId;
                            clv.WorkflowState = Constants.Constants.WorkflowStates.Created;

                            db.check_list_values.Add(clv);
                            db.SaveChanges();

                            db.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                            db.SaveChanges();
                        }

                        

                        #region foreach (string dataItemStr in dataItems)
                        elementId = clv.Id;
                        List<string> dataItems = t.LocateList(elementStr, "<DataItem>", "</DataItem>");

                        if (dataItems != null)
                        {
                            foreach (string dataItemStr in dataItems)
                            {


                                int field_id = int.Parse(t.Locate(dataItemStr, "<Id>", "</"));

                                fields f = db.fields.Single(x => x.Id == field_id);
                                field_values fieldV = null;


                                if (!fieldTypeIds.Contains((int)f.FieldTypeId)) 
                                {
                                    fieldV = db.field_values.SingleOrDefault(x => x.FieldId == field_id && x.CaseId == case_id && x.CheckListId == cl_id && x.WorkerId == userId);
                                }


                                if (fieldV == null)
                                {
                                    fieldV = new field_values();

                                    //= new field_values();

                                    #region if contains a file
                                    string urlXml = t.Locate(dataItemStr, "<URL>", "</URL>");
                                    if (urlXml != "" && urlXml != "none")
                                    {
                                        uploaded_data dU = null;
                                        string fileLocation = t.Locate(dataItemStr, "<URL>", "</");

                                        //dU = db.uploaded_data.SingleOrDefault(x => x.uploader_id == userId && x.file_location == fileLocation && x.uploader_type == Constants.UploaderTypes.System);
                                        ////

                                        //if (dU == null)
                                        //{
                                        dU = new uploaded_data();
                                        dU.CreatedAt = DateTime.Now;
                                        dU.UpdatedAt = DateTime.Now;
                                        dU.Extension = t.Locate(dataItemStr, "<Extension>", "</");
                                        dU.UploaderId = userId;
                                        dU.UploaderType = Constants.Constants.UploaderTypes.System;
                                        dU.WorkflowState = Constants.Constants.WorkflowStates.PreCreated;
                                        dU.Version = 1;
                                        dU.Local = 0;
                                        dU.FileLocation = fileLocation;

                                        db.uploaded_data.Add(dU);
                                        db.SaveChanges();

                                        db.uploaded_data_versions.Add(MapUploadedDataVersions(dU));
                                        db.SaveChanges();
                                        //}
                                        fieldV.UploadedDataId = dU.Id;
                                        uploadedDataIds.Add(dU.Id);

                                    }
                                    #endregion

                                    fieldV.CreatedAt = DateTime.Now;
                                    fieldV.UpdatedAt = DateTime.Now;
                                    #region fieldV.value = t.Locate(xml, "<Value>", "</");
                                    string extractedValue = t.Locate(dataItemStr, "<Value>", "</");

                                    if (extractedValue.Length > 8)
                                    {
                                        if (extractedValue.StartsWith(@"<![CDATA["))
                                        {
                                            extractedValue = extractedValue.Substring(9);
                                            extractedValue = extractedValue.Substring(0, extractedValue.Length - 3);
                                        }
                                    }

                                    if (f.FieldType.FieldType == Constants.Constants.FieldTypes.Number || 
                                        f.FieldType.FieldType == Constants.Constants.FieldTypes.NumberStepper)
                                    {
                                        extractedValue = extractedValue.Replace(",", "|");
                                        extractedValue = extractedValue.Replace(".", ",");
                                    }
                                    
                                    fieldV.Value = extractedValue;                                    
                                    fields _field = db.fields.SingleOrDefault(x => x.Id == field_id);
                                    if (_field.FieldType.FieldType == Constants.Constants.FieldTypes.EntitySearch || _field.FieldType.FieldType == Constants.Constants.FieldTypes.EntitySelect)
                                    {
                                        if (!string.IsNullOrEmpty(extractedValue) && extractedValue != "null")
                                        {
                                            int Id = EntityItemRead(extractedValue).Id;
                                            fieldV.Value = Id.ToString();
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
                                    //
                                    fieldV.WorkflowState = Constants.Constants.WorkflowStates.Created;
                                    fieldV.Version = 1;
                                    fieldV.CaseId = responseCase.Id;
                                    fieldV.FieldId = field_id;
                                    fieldV.WorkerId = userId;
                                    fieldV.CheckListId = clv.CheckListId;
                                    fieldV.DoneAt = t.Date(response.Checks[xmlIndex].Date);

                                    db.field_values.Add(fieldV);
                                    db.SaveChanges();

                                    db.field_value_versions.Add(MapFieldValueVersions(fieldV));
                                    db.SaveChanges();

                                    #region update case field_values
                                    if (case_fields.Contains(fieldV.FieldId))
                                    {
                                        field_types field_type = db.fields.First(x => x.Id == fieldV.FieldId).FieldType;
                                        string new_value = fieldV.Value;

                                        if (field_type.FieldType == Constants.Constants.FieldTypes.EntitySearch || field_type.FieldType == Constants.Constants.FieldTypes.EntitySelect)
                                        {
                                            try
                                            {
                                                if (fieldV.Value != "" || fieldV.Value != null)
                                                {
                                                    int Id = int.Parse(fieldV.Value);
                                                    entity_items match = db.entity_items.SingleOrDefault(x => x.Id == Id);
                                                    
                                                    if (match != null)
                                                    {
                                                        new_value = match.Name;
                                                    }

                                                }
                                            }
                                            catch { }
                                        }

                                        if (field_type.FieldType == "SingleSelect")
                                        {
                                            string key = fieldV.Value;
                                            string fullKey = t.Locate(fieldV.Field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                                            new_value = t.Locate(fullKey, "<key>", "</key>");
                                        }

                                        if (field_type.FieldType == "MultiSelect")
                                        {
                                            new_value = "";

                                            string keys = fieldV.Value;
                                            List<string> keyLst = keys.Split('|').ToList();

                                            foreach (string key in keyLst)
                                            {
                                                string fullKey = t.Locate(fieldV.Field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                                                if (new_value != "")
                                                {
                                                    new_value += "\n" + t.Locate(fullKey, "<key>", "</key>");
                                                }
                                                else
                                                {
                                                    new_value += t.Locate(fullKey, "<key>", "</key>");
                                                }
                                            }
                                        }


                                        int i = case_fields.IndexOf(fieldV.FieldId);
                                        switch (i)
                                        {
                                            case 0:
                                                responseCase.FieldValue1 = new_value;
                                                break;
                                            case 1:
                                                responseCase.FieldValue2 = new_value;
                                                break;
                                            case 2:
                                                responseCase.FieldValue3 = new_value;
                                                break;
                                            case 3:
                                                responseCase.FieldValue4 = new_value;
                                                break;
                                            case 4:
                                                responseCase.FieldValue5 = new_value;
                                                break;
                                            case 5:
                                                responseCase.FieldValue6 = new_value;
                                                break;
                                            case 6:
                                                responseCase.FieldValue7 = new_value;
                                                break;
                                            case 7:
                                                responseCase.FieldValue8 = new_value;
                                                break;
                                            case 8:
                                                responseCase.FieldValue9 = new_value;
                                                break;
                                            case 9:
                                                responseCase.FieldValue10 = new_value;
                                                break;
                                        }
                                        responseCase.Version = responseCase.Version + 1;
                                        //TODO! THIS part need to be redone in some form in EF Core!
                                        //db.cases.AddOrUpdate(responseCase);
                                        db.SaveChanges();
                                        db.case_versions.Add(MapCaseVersions(responseCase));
                                        db.SaveChanges();
                                    }

                                    #endregion

                                    #region remove dataItem duplicate from TemplatDataItemLst
                                    int index = 0;
                                    foreach (var field in TemplatFieldLst)
                                    {
                                        if (fieldV.FieldId == field.Id)
                                        {
                                            TemplatFieldLst.RemoveAt(index);
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
                    foreach (var field in TemplatFieldLst)
                    {
                        //field_values fieldV = new field_values();

                        field_values fieldV = null;

                        fieldV = db.field_values.SingleOrDefault(x => x.FieldId == field.Id && x.CaseId == responseCase.Id && x.CheckListId == field.CheckListId && x.WorkerId == userId);

                        if (fieldV == null)
                        {
                            fieldV = new field_values();
                            fieldV.CreatedAt = DateTime.Now;
                            fieldV.UpdatedAt = DateTime.Now;

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

                            db.field_values.Add(fieldV);
                            db.SaveChanges();

                            db.field_value_versions.Add(MapFieldValueVersions(fieldV));
                            db.SaveChanges();
                        }

                        
                    }
                    #endregion
                }
                return uploadedDataIds;
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckCreateDb failed", ex);
            }
        }

        /// <summary>
        /// Updates Case Field Value in DB with given caseId
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateCaseFieldValue(int caseId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases match = db.cases.SingleOrDefault(x => x.Id == caseId);


                    if (match != null)
                    {
                        List<int?> case_fields = new List<int?>();

                        check_lists cl = match.CheckList;
                        field_values fv = null;
                        #region field_value and field matching
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field1);
                        if (fv != null)
                        {
                            match.FieldValue1 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field2);
                        if (fv != null)
                        {
                            match.FieldValue2 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field3);
                        if (fv != null)
                        {
                            match.FieldValue3 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field4);
                        if (fv != null)
                        {
                            match.FieldValue4 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field5);
                        if (fv != null)
                        {
                            match.FieldValue5 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field6);
                        if (fv != null)
                        {
                            match.FieldValue6 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field7);
                        if (fv != null)
                        {
                            match.FieldValue7 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field8);
                        if (fv != null)
                        {
                            match.FieldValue8 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field9);
                        if (fv != null)
                        {
                            match.FieldValue9 = fv.Value;
                        }
                        fv = db.field_values.SingleOrDefault(x => x.CaseId == caseId && x.FieldId == cl.Field10);
                        if (fv != null)
                        {
                            match.FieldValue10 = fv.Value;
                        }

                        match.Version += 1;
                        match.UpdatedAt = DateTime.Now;
                        //TODO! THIS part need to be redone in some form in EF Core!
                        //db.cases.AddOrUpdate(match);
                        db.SaveChanges();
                        db.case_versions.Add(MapCaseVersions(match));
                        db.SaveChanges();


                        #endregion
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateCaseFieldValue failed", ex);
            }
        }

        /// <summary>
        /// Reads check from DB with given microtingUId and checkUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <param name="checkUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ReplyElement CheckRead(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    var aCase = db.cases.Single(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                    var mainCheckList = db.check_lists.Single(x => x.Id == aCase.CheckListId);

                    ReplyElement replyElement = new ReplyElement();

                    replyElement.Id = (int)aCase.CheckListId;
                    replyElement.CaseType = aCase.Type;
                    replyElement.Custom = aCase.Custom;
                    replyElement.DoneAt = (DateTime)aCase.DoneAt;
                    replyElement.DoneById = (int)aCase.WorkerId;
                    replyElement.ElementList = new List<Element>();
                    //replyElement.EndDate
                    replyElement.FastNavigation = t.Bool(mainCheckList.FastNavigation);
                    replyElement.Label = mainCheckList.Label;
                    //replyElement.Language
                    replyElement.ManualSync = t.Bool(mainCheckList.ManualSync);
                    replyElement.MultiApproval = t.Bool(mainCheckList.MultiApproval);
                    replyElement.Repeated = (int)mainCheckList.Repeated;
                    //replyElement.StartDate
                    replyElement.UnitId = (int)aCase.UnitId;
                    replyElement.MicrotingUId = aCase.MicrotingCheckUid;
                    replyElement.SiteMicrotingUUID = (int)aCase.Site.MicrotingUid;

                    foreach (check_lists checkList in aCase.CheckList.Children.OrderBy(x => x.DisplayIndex))
                    {
                        replyElement.ElementList.Add(SubChecks(checkList.Id, aCase.Id));
                    }
                    return replyElement;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckRead failed", ex);
            }
        }

        //TODO
        private Element SubChecks(int parentId, int caseId)
        {
            try
            {
                using (var db = GetContext())
                {
                    var checkList = db.check_lists.Single(x => x.Id == parentId);
                    //Element element = new Element();
                    if (checkList.Children.Count() > 0)
                    {
                        List<Element> elementList = new List<Element>();
                        foreach (check_lists subList in checkList.Children.OrderBy(x => x.DisplayIndex))
                        {
                            elementList.Add(SubChecks(subList.Id, caseId));
                        }
                        GroupElement element = new GroupElement(checkList.Id, 
                            checkList.Label, 
                            (int)checkList.DisplayIndex, 
                            checkList.Description, 
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
                        foreach (fields field in checkList.Fields.Where(x => x.ParentFieldId == null).OrderBy(x => x.DisplayIndex).ToList())
                        {
                            if (field.FieldType.FieldType == "FieldGroup")
                            {
                                List<DataItem> dataItemSubList = new List<DataItem>();
                                foreach (fields subField in field.Children.OrderBy(x => x.DisplayIndex))
                                {
                                    Field _field = FieldRead(subField.Id);

                                    _field.FieldValues = new List<FieldValue>();
                                    foreach (field_values fieldValue in subField.FieldValues.Where(x => x.CaseId == caseId).ToList())
                                    {
                                        FieldValue answer = FieldValueRead(fieldValue, false);
                                        _field.FieldValues.Add(answer);
                                    }
                                    dataItemSubList.Add(_field);
                                }

                                CDataValue description = new CDataValue();
                                description.InderValue = field.Description;
                                FieldContainer fG = new FieldContainer(field.Id, field.Label, description, field.Color, (int)field.DisplayIndex, field.DefaultValue, dataItemSubList);
                                fG.OriginalId = field.OriginalId;
                                dataItemList.Add(fG);
                            }
                            else
                            {
                                Field _field = FieldRead(field.Id);
                                _field.FieldValues = new List<FieldValue>();
                                foreach (field_values fieldValue in field.FieldValues.Where(x => x.CaseId == caseId).ToList())
                                {
                                    FieldValue answer = FieldValueRead(fieldValue, false);
                                    _field.FieldValues.Add(answer);
                                }
                                dataItemList.Add(_field);
                            }
                        }
                        DataElement dataElement = new DataElement(checkList.Id, 
                            checkList.Label, 
                            (int)checkList.DisplayIndex, 
                            checkList.Description, 
                            t.Bool(checkList.ApprovalEnabled), 
                            t.Bool(checkList.ReviewEnabled), 
                            t.Bool(checkList.DoneButtonEnabled), 
                            t.Bool(checkList.ExtraFieldsEnabled), 
                            "", 
                            t.Bool(checkList.QuickSyncEnabled), 
                            dataItemGroupList, 
                            dataItemList);
                        dataElement.OriginalId = checkList.OriginalId;
                        //return dataElement;
                        return new CheckListValue(dataElement, CheckListValueStatusRead(caseId, checkList.Id));
                    }
                    //return element;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckRead failed", ex);
            }
        }

        public List<field_values> ChecksRead(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    var aCase = db.cases.Single(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                    int caseId = aCase.Id;

                    List<field_values> lst = db.field_values.Where(x => x.CaseId == caseId).ToList();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckRead failed", ex);
            }
        }

        public Field FieldRead(int Id)
        {

            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    fields fieldDb = db.fields.SingleOrDefault(x => x.Id == Id);

                    if (fieldDb != null)
                    {
                        Field field = new Field();
                        field.Label = fieldDb.Label;
                        field.Description = new CDataValue();
                        field.Description.InderValue = fieldDb.Description;
                        field.FieldType = fieldDb.FieldType.FieldType;
                        field.FieldValue = fieldDb.DefaultValue;
                        field.EntityGroupId = fieldDb.EntityGroupId;
                        field.Color = fieldDb.Color;
                        field.Id = fieldDb.Id;

                        if (field.FieldType == "SingleSelect")
                        {
                            field.KeyValuePairList = PairRead(fieldDb.KeyValuePairList);
                        }

                        if (field.FieldType == "MultiSelect")
                        {
                            field.KeyValuePairList = PairRead(fieldDb.KeyValuePairList);
                        }
                        return field;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        // Rename method to something more intuitive!
        public FieldValue FieldValueRead(field_values reply, bool joinUploadedData)
        {
            try
            {
                using (var db = GetContext())
                {

                    fields field = db.fields.Single(x => x.Id == reply.FieldId);
                    FieldValue field_value = new FieldValue();
                    field_value.Accuracy = reply.Accuracy;
                    field_value.Altitude = reply.Altitude;
                    field_value.Color = field.Color;
                    field_value.Date = reply.Date;
                    field_value.FieldId = t.Int(reply.FieldId);
                    field_value.FieldType = field.FieldType.FieldType;
                    field_value.DateOfDoing = t.Date(reply.DoneAt);
                    field_value.Description = new CDataValue();
                    field_value.Description.InderValue = field.Description;
                    field_value.DisplayOrder = t.Int(field.DisplayIndex);
                    field_value.Heading = reply.Heading;
                    field_value.Id = reply.Id;
                    field_value.OriginalId = reply.Field.OriginalId;
                    field_value.Label = field.Label;
                    field_value.Latitude = reply.Latitude;
                    field_value.Longitude = reply.Longitude;
                    field_value.Mandatory = t.Bool(field.Mandatory);
                    field_value.ReadOnly = t.Bool(field.ReadOnly);
                    #region answer.UploadedDataId = reply.uploaded_data_id;
                    if (reply.UploadedDataId.HasValue)
                        if (reply.UploadedDataId > 0)
                        {
                            string locations = "";
                            int uploadedDataId;
                            uploaded_data uploadedData;
                            if (joinUploadedData)
                            {
                                List<field_values> lst = db.field_values.Where(x => x.CaseId == reply.CaseId && x.FieldId == reply.FieldId).ToList();

                                foreach (field_values fV in lst)
                                {
                                    uploadedDataId = (int)fV.UploadedDataId;

                                    uploadedData = db.uploaded_data.Single(x => x.Id == uploadedDataId);

                                    if (uploadedData.FileName != null)
                                        locations += uploadedData.FileLocation + uploadedData.FileName + Environment.NewLine;
                                    else
                                        locations += "File attached, awaiting download" + Environment.NewLine;
                                }
                                field_value.UploadedData = locations.TrimEnd();
                            }
                            else
                            {
                                locations = "";
                                UploadedData uploadedDataObj = new UploadedData();
                                uploadedData = reply.UploadedData;
                                uploadedDataObj.Checksum = uploadedData.Checksum;
                                uploadedDataObj.Extension = uploadedData.Extension;
                                uploadedDataObj.CurrentFile = uploadedData.CurrentFile;
                                uploadedDataObj.UploaderId = uploadedData.UploaderId;
                                uploadedDataObj.UploaderType = uploadedData.UploaderType;
                                uploadedDataObj.FileLocation = uploadedData.FileLocation;
                                uploadedDataObj.FileName = uploadedData.FileName;
                                uploadedDataObj.Id = uploadedData.Id;
                                field_value.UploadedDataObj = uploadedDataObj;
                                field_value.UploadedData = "";
                            }

                        }
                    #endregion
                    field_value.Value = reply.Value;
                    #region answer.ValueReadable = reply.value 'ish' //and if needed: answer.KeyValuePairList = ReadPairs(...);
                    field_value.ValueReadable = reply.Value;

                    if (field_value.FieldType == Constants.Constants.FieldTypes.EntitySearch || field_value.FieldType == Constants.Constants.FieldTypes.EntitySelect)
                    {
                        try
                        {
                            if (reply.Value != "" || reply.Value != null)
                            {
								int Id = int.Parse(reply.Value);
                                entity_items match = db.entity_items.SingleOrDefault(x => x.Id == Id);

                                if (match != null)
                                {
                                    field_value.ValueReadable = match.Name;
                                    field_value.Value = match.Id.ToString();
                                    field_value.MicrotingUuid = match.MicrotingUid;
                                }

                            }
                        }
                        catch { }
                    }

                    if (field_value.FieldType == Constants.Constants.FieldTypes.SingleSelect)
                    {
                        string key = field_value.Value;
                        string fullKey = t.Locate(field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                        field_value.ValueReadable = t.Locate(fullKey, "<key>", "</key>");

                        field_value.KeyValuePairList = PairRead(field.KeyValuePairList);
                    }

                    if (field_value.FieldType == Constants.Constants.FieldTypes.MultiSelect)
                    {
                        field_value.ValueReadable = "";

                        string keys = field_value.Value;
                        List<string> keyLst = keys.Split('|').ToList();

                        foreach (string key in keyLst)
                        {
                            string fullKey = t.Locate(field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                            if (field_value.ValueReadable != "")
                                field_value.ValueReadable += '|';
                            field_value.ValueReadable += t.Locate(fullKey, "<key>", "</key>");
                        }

                        field_value.KeyValuePairList = PairRead(field.KeyValuePairList);
                    }

                    if (field_value.FieldType == Constants.Constants.FieldTypes.Number ||
                        field_value.FieldType == Constants.Constants.FieldTypes.NumberStepper)
                    {
                        if (reply.Value != null)
                        {
                            field_value.ValueReadable = reply.Value.Replace(",", ".");
                            field_value.Value = reply.Value.Replace(",", ".");
                        }
                        else
                        {
                            field_value.ValueReadable = "";
                            field_value.Value = "";
                        }
                    }
                    #endregion

                    return field_value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueRead failed", ex);
            }
        }

//        public FieldValue FieldValueRead(int Id)
//        {
//            try
//            {
//                using (var db = GetContext())
//                {
//                    field_values reply = db.field_values.Single(x => x.Id == Id);
//                    //fields field = db.fields.Single(x => x.Id == reply.field_id);
//                    return FieldValueRead(reply, true);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("FieldValueUpdate failed", ex);
//            }
//        }

        public List<FieldValue> FieldValueReadList(int fieldId, int instancesBackInTime)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<field_values> matches = db.field_values.Where(x => x.FieldId == fieldId).OrderByDescending(z => z.CreatedAt).ToList();

                    if (matches.Count() > instancesBackInTime)
                        matches = matches.GetRange(0, instancesBackInTime);

                    List<FieldValue> rtnLst = new List<FieldValue>();

                    foreach (var item in matches)
                        rtnLst.Add(FieldValueRead(item, true));

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueReadList failed", ex);
            }
        }

        public List<FieldValue> FieldValueReadList(int fieldId, List<int> caseIds)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<field_values> matches = db.field_values.Where(x => x.FieldId == fieldId && caseIds.Contains(x.Id)).ToList();
                    
                    List<FieldValue> rtnLst = new List<FieldValue>();

                    foreach (var item in matches)
                        rtnLst.Add(FieldValueRead(item, true));

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueReadList failed", ex);
            }
        }

        public List<FieldValue> FieldValueReadList(List<int> caseIds)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<field_values> matches = db.field_values.Where(x => caseIds.Contains(x.Id)).ToList();
                    List<FieldValue> rtnLst = new List<FieldValue>();

                    foreach (var item in matches)
                        rtnLst.Add(FieldValueRead(item, true));

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueReadList failed", ex);
            }
        }

        public List<CheckListValue> CheckListValueReadList(List<int> caseIds)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<check_list_values> matches = db.check_list_values.Where(x => caseIds.Contains(x.Id)).ToList();
                    List<CheckListValue> rtnLst = new List<CheckListValue>();

                    foreach (var item in matches)
                    {
                        CheckListValue checkListValue = new CheckListValue();
                        checkListValue.Id = item.Id;
                        checkListValue.Label = item.Status;
                        rtnLst.Add(checkListValue);
                    }
                        

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueReadList failed", ex);
            }
        }

        public void FieldValueUpdate(int caseId, int fieldValueId, string value)
        {
            try
            {
                using (var db = GetContext())
                {
                    field_values fieldMatch = db.field_values.Single(x => x.Id == fieldValueId);

                    fieldMatch.Value = value;
                    fieldMatch.UpdatedAt = DateTime.Now;
                    fieldMatch.Version = fieldMatch.Version + 1;

                    db.field_value_versions.Add(MapFieldValueVersions(fieldMatch));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueUpdate failed", ex);
            }
        }

        public List<List<KeyValuePair>> FieldValueReadAllValues(int fieldId, List<int> caseIds,
            string customPathForUploadedData)
        {
            return FieldValueReadAllValues(fieldId, caseIds, customPathForUploadedData, ".", "");
        }

        public List<List<KeyValuePair>> FieldValueReadAllValues(int fieldId, List<int> caseIds, string customPathForUploadedData, string decimalSeparator, string thousandSeparator)
        {
            try
            {
                using (var db = GetContext())
                {
                    fields matchField = db.fields.Single(x => x.Id == fieldId);

                    List<field_values> matches = db.field_values.Where(x => x.FieldId == fieldId && caseIds.Contains((int)x.CaseId)).ToList();

                    List<List<KeyValuePair>> rtrnLst = new List<List<KeyValuePair>>();
                    List<KeyValuePair> replyLst1 = new List<KeyValuePair>();
                    rtrnLst.Add(replyLst1);

                    switch (matchField.FieldType.FieldType)
                    {
                        #region special dataItem
                        case Constants.Constants.FieldTypes.CheckBox:
                            foreach (field_values item in matches)
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
                            foreach (field_values item in matches)
                            {
                                if (item.Value != null)
                                {
                                    if (lastCaseId == (int)item.CaseId)
                                    {

                                        foreach (KeyValuePair kvp in replyLst1)
                                        {
                                            if (kvp.Key == item.CaseId.ToString())
                                            {
                                                if (customPathForUploadedData != null)
                                                    if (kvp.Value.Contains("jpg") || kvp.Value.Contains("jpeg") || kvp.Value.Contains("png"))
                                                        kvp.Value = kvp.Value + "|" + customPathForUploadedData + item.UploadedData.FileName;
                                                    else
                                                        kvp.Value = customPathForUploadedData + item.UploadedData.FileName;
                                                else
                                                    if (kvp.Value.Contains("jpg") || kvp.Value.Contains("jpeg") || kvp.Value.Contains("png"))
                                                    kvp.Value = kvp.Value + "|" + item.UploadedData.FileLocation + item.UploadedData.FileName;
                                                else
                                                    kvp.Value = item.UploadedData.FileLocation + item.UploadedData.FileName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lastIndex++;
                                        if (item.UploadedDataId != null)
                                        {
                                            if (customPathForUploadedData != null)

                                                replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), customPathForUploadedData + item.UploadedData.FileName, false, ""));
                                            else
                                                replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), item.UploadedData.FileLocation + item.UploadedData.FileName, false, ""));
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
                                var kVP = PairRead(matchField.KeyValuePairList);

                                foreach (field_values item in matches)
                                    replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), PairMatch(kVP, item.Value), false, ""));
                            }
                            break;

                        case Constants.Constants.FieldTypes.MultiSelect:
                            {
                                var kVP = PairRead(matchField.KeyValuePairList);
                                rtrnLst = new List<List<KeyValuePair>>();
                                List<KeyValuePair> replyLst = null;
                                int index = 0;
                                string valueExt = "";

                                foreach (var key in kVP)
                                {
                                    replyLst = new List<KeyValuePair>();
                                    index++;

                                    foreach (field_values item in matches)
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
                                foreach (field_values item in matches)
                                {
                                    try
                                    {
                                        if (item.Value != "" || item.Value != null)
                                        {
                                            entity_items match = db.entity_items.SingleOrDefault(x => x.Id.ToString() == item.Value);

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
                            foreach (field_values item in matches)
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
                            foreach (field_values item in matches)
                            {
                                replyLst1.Add(new KeyValuePair(item.CaseId.ToString(), item.Value, false, ""));
                            }
                            break;
                    }

                    return rtrnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        public string CheckListValueStatusRead(int caseId, int checkListId)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_list_values clv = db.check_list_values.Single(x => x.CaseId == caseId && x.CheckListId == checkListId);
                    return clv.Status;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckListValueStatusRead failed", ex);
            }
        }

        public void CheckListValueStatusUpdate(int caseId, int checkListId, string value)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_list_values match = db.check_list_values.Single(x => x.CaseId == caseId && x.CheckListId == checkListId);

                    match.Status = value;
                    match.UpdatedAt = DateTime.Now;
                    match.Version = match.Version + 1;

                    db.check_list_value_versions.Add(MapCheckListValueVersions(match));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueUpdate failed", ex);
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
        public notifications NotificationCreate(string notificationUId, string microtingUId, string activity)
        {
            string methodName = t.GetMethodName("SQLController");

            using (var db = GetContext())
            {
                if (db.notifications.Count(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId) == 0)
                {
                    log.LogStandard(methodName, "SAVING notificationUId : " + notificationUId + " microtingUId : " + microtingUId + " action : " + activity);

                    notifications aNote = new notifications();

                    aNote.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    aNote.CreatedAt = DateTime.Now;
                    aNote.UpdatedAt = DateTime.Now;
                    aNote.NotificationUid = notificationUId;
                    aNote.MicrotingUid = microtingUId;
                    aNote.Activity = activity;

                    db.notifications.Add(aNote);
                    db.SaveChanges();
                    return aNote;
                }
                else
                {
                    notifications aNote = db.notifications.SingleOrDefault(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId);
                    return aNote;
                }

                //TODO else log warning
            }
        }

        /// <summary>
        /// Returns a Note Data transfer object from DB
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Note_Dto NotificationReadFirst()
        {
            try
            {
                using (var db = GetContext())
                {
                    notifications aNoti = db.notifications.FirstOrDefault(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                    if (aNoti != null)
                    {
                        Note_Dto aNote = new Note_Dto(aNoti.NotificationUid, aNoti.MicrotingUid, aNoti.Activity);
                        return aNote;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
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
        public void NotificationUpdate(string notificationUId, string microtingUId, string workflowState, string exception, string stacktrace)
        {
            try
            {
                using (var db = GetContext())
                {
                    notifications aNoti = db.notifications.Single(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId);
                    aNoti.WorkflowState = workflowState;
                    aNoti.UpdatedAt = DateTime.Now;
                    aNoti.Exception = exception;
                    aNoti.Stacktrace = stacktrace;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
            }
        }
        #endregion

        #region file
        
        //TODO
        public UploadedData FileRead()
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data dU = db.uploaded_data.FirstOrDefault(x => x.WorkflowState == Constants.Constants.WorkflowStates.PreCreated);

                    if (dU != null)
                    {
                        UploadedData ud = new UploadedData();
                        ud.Checksum = dU.Checksum;
                        ud.Extension = dU.Extension;
                        ud.CurrentFile = dU.CurrentFile;
                        ud.UploaderId = dU.UploaderId;
                        ud.UploaderType = dU.UploaderType;
                        ud.FileLocation = dU.FileLocation;
                        ud.FileName = dU.FileName;
                        ud.Id = dU.Id;
                        return ud;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FileRead failed", ex);
            }
        }

        //TODO
        public Case_Dto FileCaseFindMUId(string urlString)
        {
            try
            {
                using (var db = GetContext())
                {
                    try
                    {
                        uploaded_data dU = db.uploaded_data.Where(x => x.FileLocation == urlString).First();
                        field_values fV = db.field_values.Single(x => x.UploadedDataId == dU.Id);
                        cases aCase = db.cases.Single(x => x.Id == fV.CaseId);

                        return CaseReadByCaseId(aCase.Id);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
//                Log.LogCritical(t.GetMethodName("Core"), ex.Message);
                throw new Exception("FileRead failed", ex);
            }
        }

        //TODO
        public void FileProcessed(string urlString, string checkSum, string fileLocation, string fileName, int Id)
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data uD = db.uploaded_data.Single(x => x.Id == Id);

                    uD.Checksum = checkSum;
                    uD.FileLocation = fileLocation;
                    uD.FileName = fileName;
                    uD.Local = 1;
                    uD.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    uD.UpdatedAt = DateTime.Now;
                    uD.Version = uD.Version + 1;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FileProcessed failed", ex);
            }
        }

        /// <summary>
        /// Returns uploaded data object from DB with give Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public uploaded_data GetUploadedData(int Id)
        {
            try
            {
                using (var db = GetContext())
                {
                    return db.uploaded_data.SingleOrDefault(x => x.Id == Id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get uploaded data object failed", ex);
            }
        }

        //TODO
        public bool UpdateUploadedData(uploaded_data uploadedData)
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data uD = db.uploaded_data.Single(x => x.Id == uploadedData.Id);
                    uD.TranscriptionId = uploadedData.TranscriptionId;
                    uD.UpdatedAt = DateTime.Now;
                    uD.Version = uploadedData.Version + 1;
                    //db.uploaded_data.Add(uploadedData);

                    db.SaveChanges();

                    db.uploaded_data_versions.Add(MapUploadedDataVersions(uploadedData));
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateUploadedData failed", ex);
            }
        }

        /// <summary>
        /// Returns field_values object from DB with given transcription Id
        /// </summary>
        /// <param name="transcriptionId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public field_values GetFieldValueByTranscriptionId(int transcriptionId)
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data ud = GetUploaded_DataByTranscriptionId(transcriptionId);
                    if (ud != null)
                    {
                        return db.field_values.SingleOrDefault(x => x.UploadedDataId == ud.Id);
                    } else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get uploaded data object failed", ex);
            }
        }

        //TODO
        public uploaded_data GetUploaded_DataByTranscriptionId(int transcriptionId)
        {

            try
            {
                using (var db = GetContext())
                {
                    return db.uploaded_data.SingleOrDefault(x => x.TranscriptionId == transcriptionId);                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get uploaded data object failed", ex);
            }
        }

        /// <summary>
        /// Deletes file from DB with given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DeleteFile(int Id)
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data uD = db.uploaded_data.Single(x => x.Id == Id);

                    uD.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                    uD.UpdatedAt = DateTime.Now;
                    uD.Version = uD.Version + 1;
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FileProcessed failed", ex);
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
        public Case_Dto CaseLookup(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases aCase = db.cases.Single(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                    return CaseReadByCaseId(aCase.Id);
                }
            } catch  (Exception ex)
            {
                throw new Exception("CaseReadByMuuId failed", ex);
            }
        }
        
        /// <summary>
        /// Finds a Case_Dto based on microtingUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Case_Dto CaseReadByMUId(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    try
                    {
                        cases aCase = db.cases.Single(x => x.MicrotingUid == microtingUId);
                        return CaseReadByCaseId(aCase.Id);
                    }
                    catch { }

                    try
                    {
                        check_list_sites cls = db.check_list_sites.Single(x => x.MicrotingUid == microtingUId);
                        check_lists cL = db.check_lists.Single(x => x.Id == cls.CheckListId);

                        #region string stat = aCase.workflow_state ...
                        string stat = "";
                        if (cls.WorkflowState == Constants.Constants.WorkflowStates.Created)
                            stat = Constants.Constants.WorkflowStates.Created;

                        if (cls.WorkflowState == Constants.Constants.WorkflowStates.Removed)
                            stat = "Deleted";
                        #endregion

                        int remoteSiteId = (int)db.sites.Single(x => x.Id == (int)cls.SiteId).MicrotingUid;
                        Case_Dto rtrnCase = new Case_Dto(null, stat, remoteSiteId, cL.CaseType, "ReversedCase", cls.MicrotingUid, cls.LastCheckId, null, cL.Id, null);
                        return rtrnCase;
                    }
                    catch(Exception ex1)
                    {
                        throw new Exception("CaseReadByMuuId failed", ex1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByMuuId failed", ex);
            }
        }

        /// <summary>
        /// Reads a Case from DB with given case id
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Case_Dto CaseReadByCaseId(int caseId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases aCase = db.cases.Single(x => x.Id == caseId);
                    check_lists cL = db.check_lists.Single(x => x.Id == aCase.CheckListId);

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

                    int remoteSiteId = (int)db.sites.Single(x => x.Id == (int)aCase.SiteId).MicrotingUid;
                    Case_Dto cDto = new Case_Dto(aCase.Id, stat, remoteSiteId, cL.CaseType, aCase.CaseUid, aCase.MicrotingUid, aCase.MicrotingCheckUid, aCase.Custom, cL.Id, aCase.WorkflowState);
                    return cDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByCaseId failed", ex);
            }
        }

        /// <summary>
        /// Return a list of Case data transfer objects from db with given caseUId
        /// </summary>
        /// <param name="caseUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Case_Dto> CaseReadByCaseUId(string caseUId)
        {
            try
            {
                if (caseUId == "")
                    throw new Exception("CaseReadByCaseUId failed. Due invalid input:''. This would return incorrect data");

                if (caseUId == "ReversedCase")
                    throw new Exception("CaseReadByCaseUId failed. Due invalid input:'ReversedCase'. This would return incorrect data");

                using (var db = GetContext())
                {
                    List<cases> matches = db.cases.Where(x => x.CaseUid == caseUId).ToList();
                    List<Case_Dto> lstDto = new List<Case_Dto>();

                    foreach (cases aCase in matches)
                        lstDto.Add(CaseReadByCaseId(aCase.Id));

                    return lstDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByCaseUId failed", ex);
            }
        }
        
        /// <summary>
        /// Returns a cases object from DB with given microtingUId and checkUId
        /// </summary>
        /// <param name="microtingUId"></param>
        /// <param name="checkUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public cases CaseReadFull(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases match = db.cases.SingleOrDefault(x => x.MicrotingUid == microtingUId && x.MicrotingCheckUid == checkUId);
                    match.SiteId = db.sites.SingleOrDefault(x => x.Id == match.SiteId).MicrotingUid;

                    if (match.UnitId != null)
                        match.UnitId = db.units.SingleOrDefault(x => x.Id == match.UnitId).MicrotingUid;

                    if (match.WorkerId != null)
                        match.WorkerId = db.workers.SingleOrDefault(x => x.Id == match.WorkerId).MicrotingUid;
                    return match;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        /// <summary>
        /// Returns an ID of a specific Case from DB with given templateId and workflowstate
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="workflowState"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int? CaseReadFirstId(int? templateId, string workflowState)
        {
            string methodName = t.GetMethodName("SQLController");
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(templateId), templateId);
            log.LogVariable(methodName, nameof(workflowState), workflowState);
            try
            {
                using (var db = GetContext())
                {
                    //cases dbCase = null;
                    IQueryable<cases> sub_query = db.cases.Where(x => x.CheckListId == templateId && x.Status == 100);
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

                    try
                    {
                        return sub_query.First().Id;
                    } catch (Exception ex)
                    {
                        throw new Exception("CaseReadFirstId failed", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        //TODO
        public CaseList CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState,
            string searchKey, bool descendingSort, string sortParameter, int offSet, int pageSize)
        {
                        
            try
            {
                using (var db = GetContext())
                {
                    if (start == null)
                        start = DateTime.MinValue;
                    if (end == null)
                        end = DateTime.MaxValue;

                    //db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                    List<cases> matches = null;
                    IQueryable<cases> sub_query = db.cases.Where(x => x.DoneAt > start && x.DoneAt < end);
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
                    if (searchKey != null && searchKey != "")
                    {
                        sub_query = sub_query.Where(x => x.FieldValue1.Contains(searchKey) || x.FieldValue2.Contains(searchKey) || x.FieldValue3.Contains(searchKey) || x.FieldValue4.Contains(searchKey) || x.FieldValue5.Contains(searchKey) || x.FieldValue6.Contains(searchKey) || x.FieldValue7.Contains(searchKey) || x.FieldValue8.Contains(searchKey) || x.FieldValue9.Contains(searchKey) || x.FieldValue10.Contains(searchKey) || x.Id.ToString().Contains(searchKey) || x.Site.Name.Contains(searchKey) || x.Worker.FirstName.Contains(searchKey) || x.Worker.LastName.Contains(searchKey));
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

                    matches = sub_query.ToList();
                    
                    List<Case> rtrnLst = new List<Case>();
                    int numOfElements = 0;
                    numOfElements = matches.Count();
                    List<cases> dbCases = null;

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
                        Case nCase = new Case();
                        nCase.CaseType = dbCase.Type;
                        nCase.CaseUId = dbCase.CaseUid;
                        nCase.CheckUIid = dbCase.MicrotingCheckUid;
                        nCase.CreatedAt = dbCase.CreatedAt;
                        nCase.Custom = dbCase.Custom;
                        nCase.DoneAt = dbCase.DoneAt;
                        nCase.Id = dbCase.Id;
                        nCase.MicrotingUId = dbCase.MicrotingUid;
                        nCase.SiteId = dbCase.Site.MicrotingUid;
                        nCase.SiteName = dbCase.Site.Name;
                        nCase.Status = dbCase.Status;
                        nCase.TemplatId = dbCase.CheckListId;
                        nCase.UnitId = dbCase.Unit.MicrotingUid;
                        nCase.UpdatedAt = dbCase.UpdatedAt;
                        nCase.Version = dbCase.Version;
                        nCase.WorkerName = dbCase.Worker.FirstName + " " + dbCase.Worker.LastName;
                        nCase.WorkflowState = dbCase.WorkflowState;
                        nCase.FieldValue1 = dbCase.FieldValue1;
                        nCase.FieldValue2 = dbCase.FieldValue2;
                        nCase.FieldValue3 = dbCase.FieldValue3;
                        nCase.FieldValue4 = dbCase.FieldValue4;
                        nCase.FieldValue5 = dbCase.FieldValue5;
                        nCase.FieldValue6 = dbCase.FieldValue6;
                        nCase.FieldValue7 = dbCase.FieldValue7;
                        nCase.FieldValue8 = dbCase.FieldValue8;
                        nCase.FieldValue9 = dbCase.FieldValue9;
                        nCase.FieldValue10 = dbCase.FieldValue10;

                        rtrnLst.Add(nCase);
                    }
                    #endregion

                    CaseList caseList = new CaseList(numOfElements, pageSize, rtrnLst);
                    
                    
                    return caseList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
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
        public List<Case> CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState, string searchKey, bool descendingSort, string sortParameter)
        {            
            string methodName = t.GetMethodName("SQLController");
            log.LogStandard(methodName, "called");
            log.LogVariable(methodName, nameof(templatId), templatId);
            log.LogVariable(methodName, nameof(start), start);
            log.LogVariable(methodName, nameof(end), end);
            log.LogVariable(methodName, nameof(workflowState), workflowState);
            log.LogVariable(methodName, nameof(searchKey), searchKey);
            log.LogVariable(methodName, nameof(descendingSort), descendingSort);
            log.LogVariable(methodName, nameof(sortParameter), sortParameter);

            CaseList cl = CaseReadAll(templatId, start, end, workflowState, searchKey, descendingSort, sortParameter, 0,
                10000000);
            
            List<Case> rtnCaseList = new List<Case>();
            
            foreach (Case @case in cl.Cases)
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
        public List<Case_Dto> CaseFindCustomMatchs(string customString)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<Case_Dto> foundCasesThatMatch = new List<Case_Dto>();

                    List<cases> lstMatchs = db.cases.Where(x => x.Custom == customString && x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToList();

                    foreach (cases match in lstMatchs)
                        foundCasesThatMatch.Add(CaseReadByCaseId(match.Id));

                    return foundCasesThatMatch;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseFindCustomMatchs failed", ex);
            }
        }

        //TODO
        public bool CaseUpdateFieldValues(int caseId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases lstMatchs = db.cases.SingleOrDefault(x => x.Id == caseId);

                    if (lstMatchs == null)
                        return false;

                    lstMatchs.UpdatedAt = DateTime.Now;
                    lstMatchs.Version = lstMatchs.Version + 1;
                    List<int?> case_fields = new List<int?>();
                    check_lists cl = lstMatchs.CheckList;

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

                    List<field_values> field_values = db.field_values.Where(x => x.CaseId == lstMatchs.Id && case_fields.Contains(x.FieldId)).ToList();

                    foreach (field_values item in field_values)
                    {
                        field_types field_type = item.Field.FieldType;
                        string new_value = item.Value;

                        if (field_type.FieldType == Constants.Constants.FieldTypes.EntitySearch || field_type.FieldType == Constants.Constants.FieldTypes.EntitySelect)
                        {
                            try
                            {
                                if (item.Value != "" || item.Value != null)
                                {
                                    entity_items match = db.entity_items.SingleOrDefault(x => x.Id == int.Parse(item.Value));

                                    if (match != null)
                                    {
                                        new_value = match.Name;
                                    }

                                }
                            }
                            catch { }
                        }

                        if (field_type.FieldType == Constants.Constants.FieldTypes.SingleSelect)
                        {
                            string key = item.Value;
                            string fullKey = t.Locate(item.Field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                            new_value = t.Locate(fullKey, "<key>", "</key>");
                        }

                        if (field_type.FieldType == Constants.Constants.FieldTypes.MultiSelect)
                        {
                            new_value = "";

                            string keys = item.Value;
                            List<string> keyLst = keys.Split('|').ToList();

                            foreach (string key in keyLst)
                            {
                                string fullKey = t.Locate(item.Field.KeyValuePairList, "<" + key + ">", "</" + key + ">");
                                if (new_value != "")
                                {
                                    new_value += "\n" + t.Locate(fullKey, "<key>", "</key>");
                                }
                                else
                                {
                                    new_value += t.Locate(fullKey, "<key>", "</key>");
                                }
                            }
                        }


                        int i = case_fields.IndexOf(item.FieldId);
                        switch (i)
                        {
                            case 0:
                                lstMatchs.FieldValue1 = new_value;
                                break;
                            case 1:
                                lstMatchs.FieldValue2 = new_value;
                                break;
                            case 2:
                                lstMatchs.FieldValue3 = new_value;
                                break;
                            case 3:
                                lstMatchs.FieldValue4 = new_value;
                                break;
                            case 4:
                                lstMatchs.FieldValue5 = new_value;
                                break;
                            case 5:
                                lstMatchs.FieldValue6 = new_value;
                                break;
                            case 6:
                                lstMatchs.FieldValue7 = new_value;
                                break;
                            case 7:
                                lstMatchs.FieldValue8 = new_value;
                                break;
                            case 8:
                                lstMatchs.FieldValue9 = new_value;
                                break;
                            case 9:
                                lstMatchs.FieldValue10 = new_value;
                                break;
                        }
                    }

                    //TODO! THIS part need to be redone in some form in EF Core!
                    //db.cases.AddOrUpdate(lstMatchs);
                    db.SaveChanges();
                    db.case_versions.Add(MapCaseVersions(lstMatchs));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
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
        public List<SiteName_Dto> SiteGetAll(bool includeRemoved)
        {
            List<SiteName_Dto> siteList = new List<SiteName_Dto>();
            using (var db = GetContext())
            {
                List<sites> matches = null;
                if(includeRemoved)
                    matches = db.sites.ToList();
                else
                    matches = db.sites.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToList();

                foreach (sites aSite in matches)
                {
                    SiteName_Dto siteNameDto = new SiteName_Dto((int)aSite.MicrotingUid, aSite.Name, aSite.CreatedAt, aSite.UpdatedAt);
                    siteList.Add(siteNameDto);
                }
            }
            return siteList;

        }

        //TODO
        public List<Site_Dto> SimpleSiteGetAll(string workflowState, int? offSet, int? limit)
        {
            List<Site_Dto> siteList = new List<Site_Dto>();
            using (var db = GetContext())
            {
                List<sites> matches = null;
                switch (workflowState)
                {
                    case Constants.Constants.WorkflowStates.NotRemoved:
                        matches = db.sites.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToList();
                        break;
                    case Constants.Constants.WorkflowStates.Removed:
                        matches = db.sites.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed).ToList();
                        break;
                    case Constants.Constants.WorkflowStates.Created:
                        matches = db.sites.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToList();
                        break;
                    default:
                        matches = db.sites.ToList();
                        break;
                }
                foreach (sites aSite in matches)
                {
                    units unit = null;
                    workers worker = null;
                    int? unitCustomerNo = null;
                    int? unitOptCode = null;
                    int? unitMicrotingUid = null;
                    int? workerMicrotingUid = null;
                    string workerFirstName = null;
                    string workerLastName = null;
                    try
                    {
                        unit = aSite.Units.First();
                        unitCustomerNo = (int)unit.CustomerNo;
                        unitOptCode = (int)unit.OtpCode;
                        unitMicrotingUid = (int)unit.MicrotingUid;
                    }
                    catch { }

                    try
                    {
                        worker = aSite.SiteWorkers.First().Worker;
                        workerMicrotingUid = worker.MicrotingUid;
                        workerFirstName = worker.FirstName;
                        workerLastName = worker.LastName;
                    }
                    catch { }

                    try
                    {
                        Site_Dto siteDto = new Site_Dto((int)aSite.MicrotingUid, aSite.Name, workerFirstName, workerLastName, unitCustomerNo, unitOptCode, unitMicrotingUid, workerMicrotingUid);
                        siteList.Add(siteDto);
                    }
                    catch
                    {
                        Site_Dto siteDto = new Site_Dto((int)aSite.MicrotingUid, aSite.Name, workerFirstName, workerLastName, unitCustomerNo, unitOptCode, unitMicrotingUid, workerMicrotingUid);
                        siteList.Add(siteDto);
                    }
                }
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
        public int SiteCreate(int microtingUid, string name)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = new sites();
                    site.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    site.Version = 1;
                    site.CreatedAt = DateTime.Now;
                    site.UpdatedAt = DateTime.Now;
                    site.MicrotingUid = microtingUid;
                    site.Name = name;


                    db.sites.Add(site);
                    db.SaveChanges();

                    db.site_versions.Add(MapSiteVersions(site));
                    db.SaveChanges();

                    return site.Id;
                }
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
        public SiteName_Dto SiteRead(int microting_uid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    sites site = db.sites.SingleOrDefault(x => x.MicrotingUid == microting_uid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                    if (site != null)
                        return new SiteName_Dto((int)site.MicrotingUid, site.Name, site.CreatedAt, site.UpdatedAt);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        //TODO
        public Site_Dto SiteReadSimple(int microting_uid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    sites site = db.sites.SingleOrDefault(x => x.MicrotingUid == microting_uid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);
                    if (site == null)
                        return null;

                    site_workers site_worker = db.site_workers.Where(x => x.SiteId == site.Id).ToList().First();
                    workers worker = db.workers.Single(x => x.Id == site_worker.WorkerId);
                    List<units> units = db.units.Where(x => x.SiteId == site.Id).ToList();

                    if (units.Count() > 0 && worker != null)
                    {
                        units unit = units.First();
                        return new Site_Dto((int)site.MicrotingUid, site.Name, worker.FirstName, worker.LastName, (int)unit.CustomerNo, (int)unit.OtpCode, (int)unit.MicrotingUid, worker.MicrotingUid);
                    }
                    else
                        return null;
                }
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
        public bool SiteUpdate(int microting_uid, string name)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = db.sites.SingleOrDefault(x => x.MicrotingUid == microting_uid);

                    if (site != null)
                    {
                        site.Version = site.Version + 1;
                        site.UpdatedAt = DateTime.Now;

                        site.Name = name;

                        db.site_versions.Add(MapSiteVersions(site));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
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
        public bool SiteDelete(int microting_uid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = db.sites.SingleOrDefault(x => x.MicrotingUid == microting_uid);

                    if (site != null)
                    {
                        site.Version = site.Version + 1;
                        site.UpdatedAt = DateTime.Now;

                        site.WorkflowState = Constants.Constants.WorkflowStates.Removed;

                        db.site_versions.Add(MapSiteVersions(site));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
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
        public List<Worker_Dto> WorkerGetAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                List<Worker_Dto> listWorkerDto = new List<Worker_Dto>();

                using (var db = GetContext())
                {
                    List<workers> matches = null;

                    switch (workflowState)
                    {
                        case Constants.Constants.WorkflowStates.NotRemoved:
                            matches = db.workers.Where(x => x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToList();
                            break;
                        case Constants.Constants.WorkflowStates.Removed:
                            matches = db.workers.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Removed).ToList();
                            break;
                        case Constants.Constants.WorkflowStates.Created:
                            matches = db.workers.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToList();
                            break;
                        default:
                            matches = db.workers.ToList();
                            break;
                    }
                    foreach (workers worker in matches)
                    {
                        Worker_Dto workerDto = new Worker_Dto(worker.MicrotingUid, worker.FirstName, worker.LastName, worker.Email, worker.CreatedAt, worker.UpdatedAt);
                        listWorkerDto.Add(workerDto);
                    }
                    return listWorkerDto;
                }
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
        public int WorkerCreate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    workers worker = new workers();
                    worker.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    worker.Version = 1;
                    worker.CreatedAt = DateTime.Now;
                    worker.UpdatedAt = DateTime.Now;
                    worker.MicrotingUid = microtingUid;
                    worker.FirstName = firstName;
                    worker.LastName = lastName;
                    worker.Email = email;


                    db.workers.Add(worker);
                    db.SaveChanges();

                    db.worker_versions.Add(MapWorkerVersions(worker));
                    db.SaveChanges();

                    return worker.Id;
                }
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
        public string WorkerNameRead(int workerId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    workers worker = db.workers.SingleOrDefault(x => x.Id == workerId && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                    if (worker == null)
                        return null;
                    else
                        return worker.FirstName + " " + worker.LastName;
                }
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
        public Worker_Dto WorkerRead(int microting_uid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    workers worker = db.workers.SingleOrDefault(x => x.MicrotingUid == microting_uid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                    if (worker != null)
                        return new Worker_Dto((int)worker.MicrotingUid, worker.FirstName, worker.LastName, worker.Email, worker.CreatedAt, worker.UpdatedAt);
                    else
                        return null;
                }
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
        public bool WorkerUpdate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    workers worker = db.workers.SingleOrDefault(x => x.MicrotingUid == microtingUid);

                    if (worker != null)
                    {
                        worker.Version = worker.Version + 1;
                        worker.UpdatedAt = DateTime.Now;

                        worker.FirstName = firstName;
                        worker.LastName = lastName;
                        worker.Email = email;

                        db.worker_versions.Add(MapWorkerVersions(worker));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
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
        public bool WorkerDelete(int microtingUid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    workers worker = db.workers.SingleOrDefault(x => x.MicrotingUid == microtingUid);

                    if (worker != null)
                    {
                        worker.Version = worker.Version + 1;
                        worker.UpdatedAt = DateTime.Now;

                        worker.WorkflowState = Constants.Constants.WorkflowStates.Removed;

                        db.worker_versions.Add(MapWorkerVersions(worker));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
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
        public int SiteWorkerCreate(int microtingUId, int siteUId, int workerUId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    int localSiteId = db.sites.Single(x => x.MicrotingUid == siteUId).Id;
                    int localWorkerId = db.workers.Single(x => x.MicrotingUid == workerUId).Id;

                    site_workers site_worker = new site_workers();
                    site_worker.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    site_worker.Version = 1;
                    site_worker.CreatedAt = DateTime.Now;
                    site_worker.UpdatedAt = DateTime.Now;
                    site_worker.MicrotingUid = microtingUId;
                    site_worker.SiteId = localSiteId;
                    site_worker.WorkerId = localWorkerId;


                    db.site_workers.Add(site_worker);
                    db.SaveChanges();

                    db.site_worker_versions.Add(MapSiteWorkerVersions(site_worker));
                    db.SaveChanges();

                    return site_worker.Id;
                }
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
        public Site_Worker_Dto SiteWorkerRead(int? siteWorkerMicrotingUid, int? siteId, int? workerId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    site_workers site_worker = null;
                    if (siteWorkerMicrotingUid == null)
                    {
                        sites site = db.sites.Single(x => x.MicrotingUid == siteId);
                        workers worker = db.workers.Single(x => x.MicrotingUid == workerId);
                        site_worker = db.site_workers.SingleOrDefault(x => x.SiteId == site.Id && x.WorkerId == worker.Id);
                    }
                    else
                    {
                        site_worker = db.site_workers.SingleOrDefault(x => x.MicrotingUid == siteWorkerMicrotingUid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);
                    }


                    if (site_worker != null)
                        return new Site_Worker_Dto((int)site_worker.MicrotingUid, (int)site_worker.Site.MicrotingUid, (int)site_worker.Worker.MicrotingUid);
                    else
                        return null;
                }
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
        public bool SiteWorkerUpdate(int microtingUid, int siteId, int workerId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    site_workers site_worker = db.site_workers.SingleOrDefault(x => x.MicrotingUid == microtingUid);

                    if (site_worker != null)
                    {
                        site_worker.Version = site_worker.Version + 1;
                        site_worker.UpdatedAt = DateTime.Now;

                        site_worker.SiteId = siteId;
                        site_worker.WorkerId = workerId;

                        db.site_worker_versions.Add(MapSiteWorkerVersions(site_worker));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
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
        public bool SiteWorkerDelete(int microting_uid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    site_workers site_worker = db.site_workers.SingleOrDefault(x => x.MicrotingUid == microting_uid);

                    if (site_worker != null)
                    {
                        site_worker.Version = site_worker.Version + 1;
                        site_worker.UpdatedAt = DateTime.Now;

                        site_worker.WorkflowState = Constants.Constants.WorkflowStates.Removed;

                        db.site_worker_versions.Add(MapSiteWorkerVersions(site_worker));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
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
        public List<Unit_Dto> UnitGetAll()
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                List<Unit_Dto> listWorkerDto = new List<Unit_Dto>();
                using (var db = GetContext())
                {
                    foreach (units unit in db.units.ToList())
                    {
                        Unit_Dto unitDto = new Unit_Dto((int)unit.MicrotingUid, (int)unit.CustomerNo, (int)unit.OtpCode, (int)unit.Site.MicrotingUid, unit.CreatedAt, unit.UpdatedAt);
                        listWorkerDto.Add(unitDto);
                    }
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
        public int UnitCreate(int microtingUid, int customerNo, int otpCode, int siteUId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    int localSiteId = db.sites.Single(x => x.MicrotingUid == siteUId).Id;

                    units unit = new units();
                    unit.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    unit.Version = 1;
                    unit.CreatedAt = DateTime.Now;
                    unit.UpdatedAt = DateTime.Now;
                    unit.MicrotingUid = microtingUid;
                    unit.CustomerNo = customerNo;
                    unit.OtpCode = otpCode;
                    unit.SiteId = localSiteId;


                    db.units.Add(unit);
                    db.SaveChanges();

                    db.unit_versions.Add(MapUnitVersions(unit));
                    db.SaveChanges();

                    return unit.Id;
                }
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
        public Unit_Dto UnitRead(int microtingUid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.MicrotingUid == microtingUid && x.WorkflowState == Constants.Constants.WorkflowStates.Created);

                    if (unit != null)
                        return new Unit_Dto((int)unit.MicrotingUid, (int)unit.CustomerNo, (int)unit.OtpCode, (int)unit.SiteId, unit.CreatedAt, unit.UpdatedAt);
                    else
                        return null;
                }
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
        public bool UnitUpdate(int microtingUid, int customerNo, int otpCode, int siteId)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.MicrotingUid == microtingUid);

                    if (unit != null)
                    {
                        unit.Version = unit.Version + 1;
                        unit.UpdatedAt = DateTime.Now;

                        unit.CustomerNo = customerNo;
                        unit.OtpCode = otpCode;

                        db.unit_versions.Add(MapUnitVersions(unit));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
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
        public bool UnitDelete(int microtingUid)
        {
            string methodName = t.GetMethodName("SQLController");
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.MicrotingUid == microtingUid);

                    if (unit != null)
                    {
                        unit.Version = unit.Version + 1;
                        unit.UpdatedAt = DateTime.Now;

                        unit.WorkflowState = Constants.Constants.WorkflowStates.Removed;

                        db.unit_versions.Add(MapUnitVersions(unit));
                        db.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion
        #endregion

        #region public entity
        #region entityGroup
        
        //TODO
        public EntityGroupList EntityGroupAll(string sort, string nameFilter, int offSet, int pageSize, string entityType, bool desc, string workflowState)
        {

            if (entityType != Constants.Constants.FieldTypes.EntitySearch && entityType != Constants.Constants.FieldTypes.EntitySelect)
                throw new Exception("EntityGroupAll failed. EntityType:" + entityType + " is not an known type");
            if (workflowState != Constants.Constants.WorkflowStates.NotRemoved && workflowState != Constants.Constants.WorkflowStates.Created && workflowState != Constants.Constants.WorkflowStates.Removed)
                throw new Exception("EntityGroupAll failed. workflowState:" + workflowState + " is not an known workflow state");

            List<entity_groups> eG = null;
            List<EntityGroup> e_G = new List<EntityGroup>();
            int numOfElements = 0;
            try
            {
                using (var db = GetContext())
                {
                    var source = db.entity_groups.Where(x => x.Type == entityType);
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
                        eG = source.ToList();
                    }
                    else
                    {
                        eG = source.Skip(offSet).Take(pageSize).ToList();
                    }

                    foreach (entity_groups eg in eG)
                    {
                        EntityGroup g = new EntityGroup(eg.Id, eg.Name, eg.Type, eg.MicrotingUid, new List<EntityItem>(), eg.WorkflowState, eg.CreatedAt, eg.UpdatedAt);
                        e_G.Add(g);
                    }
                    return new EntityGroupList(numOfElements, offSet, e_G);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupAll failed", ex);
            }
        }

        /// <summary>
        /// Crates an Entity Group in DB with given name and type of entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public EntityGroup EntityGroupCreate(string name, string entityType)
        {
            try
            {
                if (entityType != Constants.Constants.FieldTypes.EntitySearch && entityType != Constants.Constants.FieldTypes.EntitySelect)
                    throw new Exception("EntityGroupCreate failed. EntityType:" + entityType + " is not an known type");

                using (var db = GetContext())
                {
                    entity_groups eG = new entity_groups();

                    eG.CreatedAt = DateTime.Now;
                    eG.Name = name;
                    eG.Type = entityType;
                    eG.UpdatedAt = DateTime.Now;
                    eG.Version = 1;
                    eG.WorkflowState = Constants.Constants.WorkflowStates.Created;

                    db.entity_groups.Add(eG);
                    db.SaveChanges();

                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));
                    db.SaveChanges();

                    return new EntityGroup(eG.Id, eG.Name, eG.Type, eG.MicrotingUid, new List<EntityItem>(), eG.WorkflowState, eG.CreatedAt, eG.UpdatedAt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupCreate failed", ex);
            }
        }

        //TODO
        public EntityGroup EntityGroupReadSorted(string entityGroupMUId, string sort, string nameFilter)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.MicrotingUid == entityGroupMUId);

                    if (eG == null)
                        return null;

                    List<EntityItem> lst = new List<EntityItem>();
                    EntityGroup rtnEG = new EntityGroup(eG.Id, eG.Name, eG.Type, eG.MicrotingUid, lst, eG.WorkflowState, eG.CreatedAt, eG.UpdatedAt);

                    List<entity_items> eILst = null;

                    if (nameFilter == "")
                    {
                        if (sort == Constants.Constants.EntityItemSortParameters.Id)
                        {
                            eILst = db.entity_items.Where(x => x.EntityGroupId == eG.Id && x.WorkflowState != Constants.Constants.WorkflowStates.Removed && x.WorkflowState != Constants.Constants.WorkflowStates.FailedToSync).OrderBy(x => x.Id).ToList();
                        }
                        else
                        {
                            eILst = db.entity_items.Where(x => x.EntityGroupId == eG.Id && x.WorkflowState != Constants.Constants.WorkflowStates.Removed && x.WorkflowState != Constants.Constants.WorkflowStates.FailedToSync).OrderBy(x => x.Name).ToList();
                        }
                    }
                    else
                    {
                        if (sort == Constants.Constants.EntityItemSortParameters.Id)
                        {
                            eILst = db.entity_items.Where(x => x.EntityGroupId == eG.Id && x.WorkflowState != Constants.Constants.WorkflowStates.Removed && x.WorkflowState != Constants.Constants.WorkflowStates.FailedToSync && x.Name.Contains(nameFilter)).OrderBy(x => x.Id).ToList();
                        }
                        else
                        {
                            eILst = db.entity_items.Where(x => x.EntityGroupId == eG.Id && x.WorkflowState != Constants.Constants.WorkflowStates.Removed && x.WorkflowState != Constants.Constants.WorkflowStates.FailedToSync && x.Name.Contains(nameFilter)).OrderBy(x => x.Name).ToList();
                        }
                    }

                    if (eILst.Count > 0)
                        foreach (entity_items item in eILst)
                        {
                            EntityItem eI = new EntityItem(item.Id, item.Name, item.Description, item.EntityItemUid, item.MicrotingUid, item.WorkflowState);
                            lst.Add(eI);
                        }

                    return rtnEG;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupRead failed", ex);
            }
        }

        /// <summary>
        /// Reads Entity Group from DB with given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public EntityGroup EntityGroupRead(int Id) 
        {
            using (var db = GetContext()) {
                entity_groups eg = db.entity_groups.SingleOrDefault(x => x.Id == Id);
                if (eg != null) {
                    List<EntityItem> egl = new List<EntityItem>();
                    return new EntityGroup(eg.Id, eg.Name, eg.Type, eg.MicrotingUid, egl);
                } else {
                    throw new NullReferenceException("No EntityGroup found by Id " + Id.ToString());
                }
            }
        }

        //TODO
        public EntityGroup EntityGroupRead(string entityGroupMUId)
        {
            return EntityGroupReadSorted(entityGroupMUId, "Id", "");
        }

        /// <summary>
        /// Updates Entity Group in DB with given EntitygroupId and entityGroupMUId
        /// </summary>
        /// <param name="entityGroupId"></param>
        /// <param name="entityGroupMUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool EntityGroupUpdate(int entityGroupId, string entityGroupMUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.Id == entityGroupId);

                    if (eG == null)
                        return false;

                    eG.MicrotingUid = entityGroupMUId;
                    eG.UpdatedAt = DateTime.Now;
                    eG.Version = eG.Version + 1;

                    db.SaveChanges();

                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupUpdate failed", ex);
            }
        }

        /// <summary>
        /// Updates Entity Group Name in DB with given name and entitygroupMUId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entityGroupMUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool EntityGroupUpdateName(string name, string entityGroupMUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.MicrotingUid == entityGroupMUId);

                    if (eG == null)
                        return false;

                    eG.Name = name;
                    eG.UpdatedAt = DateTime.Now;
                    eG.Version = eG.Version + 1;

                    db.SaveChanges();

                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupUpdate failed", ex);
            }
        }

        /// <summary>
        /// Deletes Entity Group in DB with given entitygROUPMUId
        /// </summary>
        /// <param name="entityGroupMUId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string EntityGroupDelete(string entityGroupMUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<string> killLst = new List<string>();

                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.MicrotingUid == entityGroupMUId && x.WorkflowState != Constants.Constants.WorkflowStates.Removed);

                    if (eG == null)
                        return null;

                    killLst.Add(eG.MicrotingUid);

                    eG.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                    eG.UpdatedAt = DateTime.Now;
                    eG.Version = eG.Version + 1;
                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));

                    List<entity_items> lst = db.entity_items.Where(x => x.EntityGroupId == eG.Id && x.WorkflowState != Constants.Constants.WorkflowStates.Removed).ToList();
                    if (lst != null)
                    {
                        foreach (entity_items item in lst)
                        {
                            item.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                            item.UpdatedAt = DateTime.Now;
                            item.Version = item.Version + 1;
                            item.Synced = t.Bool(false);
                            db.entity_item_versions.Add(MapEntityItemVersions(item));

                            killLst.Add(item.MicrotingUid);
                        }
                    }

                    db.SaveChanges();

                    return eG.Type;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupDelete failed", ex);
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
        public entity_items EntityItemRead(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    return db.entity_items.Single(x => x.MicrotingUid == microtingUId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemRead failed", ex);
            }
        }

        /// <summary>
        /// Reads an Entity Item from DB with given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public EntityItem EntityItemRead(int Id)
        {
            using (var db = GetContext())
            {
                entity_items et = db.entity_items.FirstOrDefault(x => x.Id == Id);
                if (et != null)
                {
                    EntityItem entityItem = new EntityItem(et.Id, et.Name, et.Description, et.EntityItemUid, et.MicrotingUid);
                    entityItem.EntityItemGroupId = et.EntityGroupId;
                    entityItem.Id = et.Id;
                    return entityItem;
                }
                else
                {
                    throw new NullReferenceException("No EntityItem found for Id " + Id.ToString());
                }
            }
        }

        
        /// <summary>
        /// Reads an entity item group with given id, name and description from DB
        /// </summary>
        /// <param name="entityItemGroupId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public EntityItem EntityItemRead(int entityItemGroupId, string name, string description)
        {
            using (var db = GetContext())
            {
                entity_items et = db.entity_items.SingleOrDefault(x => x.Name == name && x.Description == description && x.EntityGroupId == entityItemGroupId);
                if (et != null)
                {
                    return new EntityItem(et.Id, et.Name, et.Description, et.EntityItemUid, et.MicrotingUid, et.WorkflowState);
                }
                else
                {
                    return null;
                }
            }
        }

    
        //TODO
        public EntityItem EntityItemCreate(int entityItemGroupId, EntityItem entityItem)
        {

            using (var db = GetContext())
            {
                entity_items eI = new entity_items();

                eI.WorkflowState = Constants.Constants.WorkflowStates.Created;
                eI.Version = 1;
                eI.CreatedAt = DateTime.Now;
                eI.UpdatedAt = DateTime.Now;
                eI.EntityGroupId = entityItemGroupId;
                eI.EntityItemUid = entityItem.EntityItemUId;
                eI.MicrotingUid = entityItem.MicrotingUUID;
                eI.Name = entityItem.Name;
                eI.Description = entityItem.Description;
                eI.DisplayIndex = entityItem.DisplayIndex;
                //eI.migrated_entity_group_id = true;
                eI.Synced = t.Bool(false);

                db.entity_items.Add(eI);
                db.SaveChanges();

                db.entity_item_versions.Add(MapEntityItemVersions(eI));
                db.SaveChanges();
            }
            return entityItem;
        }

        /// <summary>
        /// Updates an Entity Item in DB
        /// </summary>
        /// <param name="entityItem"></param>
        public void EntityItemUpdate(EntityItem entityItem)
        {
            using (var db = GetContext())
            {
                var match = db.entity_items.SingleOrDefault(x => x.Id == entityItem.Id);
                match.Description = entityItem.Description;
                match.Name = entityItem.Name;
                match.Synced = t.Bool(false);
                match.UpdatedAt = DateTime.Now;
                match.DisplayIndex = entityItem.DisplayIndex;
                match.Version = match.Version + 1;
                match.EntityItemUid = entityItem.EntityItemUId;
                match.WorkflowState = entityItem.WorkflowState;

                db.SaveChanges();

                db.entity_item_versions.Add(MapEntityItemVersions(match));
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes an Entity Item in DB with given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void EntityItemDelete(int Id)
        {
            using (var db = GetContext())
            {
                entity_items et = db.entity_items.SingleOrDefault(x => x.Id == Id);
                if (et == null)
                {
                    throw new NullReferenceException("EntityItem not found with Id " + Id.ToString());
                }
                else
                {
                    et.Synced = t.Bool(true);
                    et.UpdatedAt = DateTime.Now;
                    et.Version = et.Version + 1;
                    et.WorkflowState = Constants.Constants.WorkflowStates.Removed;

                    db.SaveChanges();

                    db.entity_item_versions.Add(MapEntityItemVersions(et));
                    db.SaveChanges();
                }
            }
        }
        #endregion
        #endregion

        #region folders

        /// <summary>
        /// Returns a list of Folders of type of folder data transfer objects from DB
        /// </summary>
        /// <param name="includeRemoved"></param>
        /// <returns></returns>
        public List<Folder_Dto> FolderGetAll(bool includeRemoved)
        {
            List<Folder_Dto> folderDtos = new List<Folder_Dto>();
            using (var db = GetContext())
            {
                List<folders> matches = null;
                matches = includeRemoved ? db.folders.ToList() : db.folders.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToList();

                foreach (folders folder in matches)
                {
                    Folder_Dto folderDto = new Folder_Dto(folder.Id, folder.Name, folder.Description, folder.ParentId, folder.CreatedAt, folder.UpdatedAt, folder.MicrotingUid);
                    folderDtos.Add(folderDto);
                }
            }

            return folderDtos;
        }

        /// <summary>
        /// Reads a Folder of type Folder data transfer object from DB with specific Microting_UID
        /// </summary>
        /// <param name="microting_uid"></param>
        /// <returns></returns>
        public Folder_Dto FolderReadByMicrotingUUID(int microting_uid)
        {
            using (var db = GetContext())
            {
                folders folder = db.folders.SingleOrDefault(x => x.MicrotingUid == microting_uid);

                if (folder == null)
                {
                    return null;
                }

                Folder_Dto folderDto = new Folder_Dto(folder.Id, folder.Name, folder.Description, folder.ParentId, folder.CreatedAt, folder.UpdatedAt, folder.MicrotingUid);
                return folderDto;
            }
        }

        /// <summary>
        /// Reads a folder from DB with given ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public Folder_Dto FolderRead(int Id)
        {
            using (var db = GetContext())
            {
                folders folder = db.folders.SingleOrDefault(x => x.Id == Id);

                if (folder == null)
                {
                    throw new NullReferenceException($"Could not find area with Id: {Id}");
                }

                Folder_Dto folderDto = new Folder_Dto(folder.Id, folder.Name, folder.Description, folder.ParentId, folder.CreatedAt, folder.UpdatedAt, folder.MicrotingUid);
                return folderDto;
            }
        }

        /// <summary>
        /// Creates a folder in DB with given name, description, parent id and microtingUUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="parent_id"></param>
        /// <param name="microtingUUID"></param>
        /// <returns></returns>
        public int FolderCreate(string name, string description, int? parent_id, int microtingUUID)
        {
            folders folder = new folders
            {
                Name = name,
                Description = description,
                ParentId = parent_id,
                MicrotingUid = microtingUUID
            };

            folder.Save(GetContext());
            
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
        public void FolderUpdate(int Id, string name, string description, int? parent_id)
        {
            using (var db = GetContext())
            {
                folders folder = db.folders.SingleOrDefault(x => x.Id == Id);

                if (folder == null)
                {
                    throw new NullReferenceException($"Could not find area with Id: {Id}");
                }

                folder.Name = name;
                folder.Description = description;
                folder.ParentId = parent_id;

                folder.Update(db);
            }
        }

        /// <summary>
        /// Deletes a Folder in DB with given ID
        /// </summary>
        /// <param name="Id"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void FolderDelete(int Id)
        {
            using (var db = GetContext())
            {
                folders folder = db.folders.SingleOrDefault(x => x.Id == Id);

                if (folder == null)
                {
                    throw new NullReferenceException($"Could not find area with Id: {Id}");
                }

                folder.Delete(db);
            }
        }
        
        #endregion
        
        
        #region public setting
        
        //TODO
        public bool SettingCreateDefaults()
        {
            //key point
            SettingCreate(Settings.firstRunDone);
            SettingCreate(Settings.logLevel);
            SettingCreate(Settings.logLimit);
            SettingCreate(Settings.knownSitesDone);
            SettingCreate(Settings.fileLocationPicture);
            SettingCreate(Settings.fileLocationPdf);
            SettingCreate(Settings.fileLocationJasper);
            SettingCreate(Settings.token);
            SettingCreate(Settings.comAddressBasic);
            SettingCreate(Settings.comAddressApi);
            SettingCreate(Settings.comAddressPdfUpload);
            SettingCreate(Settings.comOrganizationId);
            SettingCreate(Settings.awsAccessKeyId);
            SettingCreate(Settings.awsSecretAccessKey);
            SettingCreate(Settings.awsEndPoint);
            SettingCreate(Settings.unitLicenseNumber);
            SettingCreate(Settings.httpServerAddress);
            SettingCreate(Settings.maxParallelism);
            SettingCreate(Settings.numberOfWorkers);
            SettingCreate(Settings.comSpeechToText);
            SettingCreate(Settings.swiftEnabled);
            SettingCreate(Settings.swiftUserName);
            SettingCreate(Settings.swiftPassword);
            SettingCreate(Settings.swiftEndPoint);
            SettingCreate(Settings.keystoneEndPoint);
            SettingCreate(Settings.customerNo);
            SettingCreate(Settings.s3Enabled);
            SettingCreate(Settings.s3AccessKeyId);
            SettingCreate(Settings.s3SecrectAccessKey);
            SettingCreate(Settings.s3Endpoint);

            return true;
        }

        /// <summary>
        /// Creates setting with specific name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public bool SettingCreate(Settings name)
        {
            using (var db = GetContext())
            {
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
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            Id = 5;
                            defaultValue = "output/dataFolder/picture/";
                        }
                        else
                        {
                            Id = 5; 
                            defaultValue = @"output\dataFolder\picture\";
                        } 
                        break;
                    case Settings.fileLocationPdf:
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {                            
                            Id = 6;
                            defaultValue = "output/dataFolder/pdf/";
                        }
                        else
                        {
                            Id = 6; 
                            defaultValue = @"output\dataFolder\pdf\";
                        } 
                        break;
                    case Settings.fileLocationJasper:
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            Id = 7;
                            defaultValue = "output/dataFolder/reports/";
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
                    case Settings.s3Endpoint: Id = 30; defaultValue = "XXX"; break;

                    default:
                        throw new IndexOutOfRangeException(name.ToString() + " is not a known/mapped Settings type");
                }
                #endregion

                settings matchId = db.settings.SingleOrDefault(x => x.Id == Id);
                settings matchName = db.settings.SingleOrDefault(x => x.Name == name.ToString());

                if (matchName == null)
                {
                    if (matchId != null)
                    {
                        #region there is already a setting with that Id but different name
                        //the old setting data is copied, and new is added
                        settings newSettingBasedOnOld = new settings();
                        newSettingBasedOnOld.Id = (db.settings.Select(x => (int?)x.Id).Max() ?? 0) + 1;
                        newSettingBasedOnOld.Name = matchId.Name.ToString();
                        newSettingBasedOnOld.Value = matchId.Value;

                        db.settings.Add(newSettingBasedOnOld);

                        matchId.Name = name.ToString();
                        matchId.Value = defaultValue;

                        db.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        //its a new setting
                        settings newSetting = new settings();
                        newSetting.Id = Id;
                        newSetting.Name = name.ToString();
                        newSetting.Value = defaultValue;

                        db.settings.Add(newSetting);
                    }
                    db.SaveChanges();
                }
                else
                    if (string.IsNullOrEmpty(matchName.Value))
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
        public string SettingRead(Settings name)
        {
            try
            {
                using (var db = GetContext())
                {
                    settings match = db.settings.Single(x => x.Name == name.ToString());

                    if (match.Value == null)
                        return "";

                    return match.Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
            }
        }

        /// <summary>
        /// Updates specific setting with new value in DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newValue"></param>
        /// <exception cref="Exception"></exception>
        public void SettingUpdate(Settings name, string newValue)
        {
            try
            {
                using (var db = GetContext())
                {
                    settings match = db.settings.SingleOrDefault(x => x.Name == name.ToString());

                    if (match == null)
                    {
                        SettingCreate(name);
                        match = db.settings.Single(x => x.Name == name.ToString());
                    }

                    match.Value = newValue;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
            }
        }

        //TODO
        public List<string> SettingCheckAll()
        {
            List<string> result = new List<string>();
            try
            {
                using (var db = GetContext())
                {
                    if (db.field_types.Count() == 0)
                    {
                        #region prime FieldTypes
                        //UnitTest_TruncateTable(typeof(field_types).Name);

                        FieldTypeAdd(1, Constants.Constants.FieldTypes.Text, "Simple text field");
                        FieldTypeAdd(2, Constants.Constants.FieldTypes.Number, "Simple number field");
                        FieldTypeAdd(3, Constants.Constants.FieldTypes.None, "Simple text to be displayed");
                        FieldTypeAdd(4, Constants.Constants.FieldTypes.CheckBox, "Simple check box field");
                        FieldTypeAdd(5, Constants.Constants.FieldTypes.Picture, "Simple picture field");
                        FieldTypeAdd(6, Constants.Constants.FieldTypes.Audio, "Simple audio field");
                        FieldTypeAdd(7, Constants.Constants.FieldTypes.Movie, "Simple movie field");
                        FieldTypeAdd(8, Constants.Constants.FieldTypes.SingleSelect, "Single selection list");
                        FieldTypeAdd(9, Constants.Constants.FieldTypes.Comment, "Simple comment field");
                        FieldTypeAdd(10, Constants.Constants.FieldTypes.MultiSelect, "Simple multi select list");
                        FieldTypeAdd(11, Constants.Constants.FieldTypes.Date, "Date selection");
                        FieldTypeAdd(12, Constants.Constants.FieldTypes.Signature, "Simple signature field");
                        FieldTypeAdd(13, Constants.Constants.FieldTypes.Timer, "Simple timer field");
                        FieldTypeAdd(14, Constants.Constants.FieldTypes.EntitySearch, "Autofilled searchable items field");
                        FieldTypeAdd(15, Constants.Constants.FieldTypes.EntitySelect, "Autofilled single selection list");
                        FieldTypeAdd(16, Constants.Constants.FieldTypes.ShowPdf, "Show PDF");
                        FieldTypeAdd(17, Constants.Constants.FieldTypes.FieldGroup, "Field group");
                        FieldTypeAdd(18, Constants.Constants.FieldTypes.SaveButton, "Save eForm");
                        FieldTypeAdd(19, Constants.Constants.FieldTypes.NumberStepper, "Number stepper field");
                        #endregion
                    }

                    int countVal = db.settings.Count(x => x.Value == "");
                    int countSet = db.settings.Count();

                    if (countSet == 0)
                    {
                        result.Add("NO SETTINGS PRESENT, NEEDS PRIMING!");
                        return result;
                    }

                    foreach (var setting in Enum.GetValues(typeof(Settings)))
                    {
                        try
                        {
                            string readSetting = SettingRead((Settings)setting);
                            if (readSetting == "" || readSetting == null)
                                result.Add(setting.ToString() + " has an empty value!");
                        }
                        catch
                        {
                            result.Add("There is no setting for " + setting + "! You need to add one");
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
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
            try
            {
                string logLevel = SettingRead(Settings.logLevel);
                int logLevelInt = int.Parse(logLevel);
                if (log == null)
                    log = new Log(core, this, logLevelInt);
                return log;
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("SQLController") + " failed", ex);
            }
        }

        //TODO
        public override string WriteLogEntry(LogEntry logEntry)
        {
            lock (_lockWrite)
            {
                try
                {
                    using (var db = GetContext())
                    {
                        logs newLog = new logs();
                        newLog.CreatedAt = logEntry.Time;
                        newLog.Level = logEntry.Level;
                        newLog.Message = logEntry.Message;
                        newLog.Type = logEntry.Type;

                        db.logs.Add(newLog);
                        db.SaveChanges();
                        

                        var oldColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("[DBG] " + logEntry.Type + ": " + logEntry.Message);
                        Console.ForegroundColor = oldColor;
                        
                        if (logEntry.Level < 0)
                            WriteLogExceptionEntry(logEntry);

                        #region clean up of log table
                        int limit = t.Int(SettingRead(Settings.logLimit));
                        if (limit > 0)
                        {
                            List<logs> killList = db.logs.Where(x => x.Id <= newLog.Id - limit).ToList();

                            if (killList.Count > 0)
                            {
                                db.logs.RemoveRange(killList);
                                db.SaveChanges();
                            }
                        }
                        #endregion
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    return t.PrintException(t.GetMethodName("SQLController") + " failed", ex);
                }
            }
        }

        
        //TODO
        private string WriteLogExceptionEntry(LogEntry logEntry)
        {
            try
            {
                using (var db = GetContext())
                {
                    log_exceptions newLog = new log_exceptions();
                    newLog.CreatedAt = logEntry.Time;
                    newLog.Level = logEntry.Level;
                    newLog.Message = logEntry.Message;
                    newLog.Type = logEntry.Type;

                    db.log_exceptions.Add(newLog);
                    db.SaveChanges();
                                        
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERR] " + logEntry.Type + ": " + logEntry.Message);
                    Console.ForegroundColor = oldColor;

                    #region clean up of log exception table
                    int limit = t.Int(SettingRead(Settings.logLimit));
                    if (limit > 0)
                    {
                        List<log_exceptions> killList = db.log_exceptions.Where(x => x.Id <= newLog.Id - limit).ToList();

                        if (killList.Count > 0)
                        {
                            db.log_exceptions.RemoveRange(killList);
                            db.SaveChanges();
                        }
                    }
                    #endregion
                }
                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName("SQLController") + " failed", ex);
            }
        }

        /// <summary>
        /// Writes into the log if it failed to write.
        /// </summary>
        /// <param name="logEntries"></param>
        public override void WriteIfFailed(string logEntries)
        {
            lock (_lockWrite)
            {
                try
                {
                    File.AppendAllText(@"expection.txt",
                        DateTime.Now.ToString() + " // " + "L:" + "-22" + " // " + "Write logic failed" + " // " + Environment.NewLine
                        + logEntries + Environment.NewLine);
                }
                catch
                {
                    //magic
                }
            }
        }
        #endregion
        #endregion

        #region private
        #region EformCreateDb
        
        //TODO
        private int EformCreateDb(MainElement mainElement)
        {
            try
            {
                using (var db = GetContext())
                {
                    GetConverter();

                    #region mainElement
                    check_lists cl = new check_lists();
                    cl.CreatedAt = DateTime.Now;
                    cl.UpdatedAt = DateTime.Now;
                    cl.Label = mainElement.Label;
                    //description - used for non-MainElements
                    //serialized_default_values - Ruby colume
                    cl.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    cl.ParentId = null; //MainElements never have parents ;)
                    cl.Repeated = mainElement.Repeated;
                    cl.QuickSyncEnabled = t.Bool(mainElement.EnableQuickSync);
                    cl.Version = 1;
                    cl.CaseType = mainElement.CaseType;
                    cl.FolderName = mainElement.CheckListFolderName;
                    cl.DisplayIndex = mainElement.DisplayOrder;
                    //report_file_name - Ruby colume
                    cl.ReviewEnabled = 0; //used for non-MainElements
                    cl.ManualSync = t.Bool(mainElement.ManualSync);
                    cl.ExtraFieldsEnabled = 0; //used for non-MainElements
                    cl.DoneButtonEnabled = 0; //used for non-MainElements
                    cl.ApprovalEnabled = 0; //used for non-MainElements
                    cl.MultiApproval = t.Bool(mainElement.MultiApproval);
                    cl.FastNavigation = t.Bool(mainElement.FastNavigation);
                    cl.DownloadEntities = t.Bool(mainElement.DownloadEntities);
                    cl.OriginalId = mainElement.Id.ToString();

                    db.check_lists.Add(cl);
                    db.SaveChanges();

                    db.check_list_versions.Add(MapCheckListVersions(cl));
                    db.SaveChanges();

                    int mainId = cl.Id;
                    mainElement.Id = mainId;
                    #endregion

                    CreateElementList(mainId, mainElement.ElementList);

                    return mainId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCreateDb failed", ex);
            }
        }

        //TODO
        private void CreateElementList(int parentId, List<Element> lstElement)
        {
            foreach (Element element in lstElement)
            {
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    CreateDataElement(parentId, dataE);
                }

                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    CreateGroupElement(parentId, groupE);
                }
            }
        }

        //TODO
        private void CreateGroupElement(int parentId, GroupElement groupElement)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists cl = new check_lists();
                    cl.CreatedAt = DateTime.Now;
                    cl.UpdatedAt = DateTime.Now;
                    cl.Label = groupElement.Label;
                    if (groupElement.Description != null)
                        cl.Description = groupElement.Description.InderValue;
                    else
                        cl.Description = "";
                    //serialized_default_values - Ruby colume
                    cl.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    cl.ParentId = parentId;
                    //repeated - used for mainElements
                    cl.Version = 1;
                    //case_type - used for mainElements
                    //folder_name - used for mainElements
                    cl.DisplayIndex = groupElement.DisplayOrder;
                    //report_file_name - Ruby colume
                    cl.ReviewEnabled = t.Bool(groupElement.ReviewEnabled);
                    //manualSync - used for mainElements
                    cl.ExtraFieldsEnabled = t.Bool(groupElement.ExtraFieldsEnabled);
                    cl.DoneButtonEnabled = t.Bool(groupElement.DoneButtonEnabled);
                    cl.ApprovalEnabled = t.Bool(groupElement.ApprovalEnabled);
                    //MultiApproval - used for mainElements
                    //FastNavigation - used for mainElements
                    //DownloadEntities - used for mainElements

                    db.check_lists.Add(cl);
                    db.SaveChanges();

                    db.check_list_versions.Add(MapCheckListVersions(cl));
                    db.SaveChanges();

                    CreateElementList(cl.Id, groupElement.ElementList);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateGroupElement failed", ex);
            }
        }

        //TODO
        private void CreateDataElement(int parentId, DataElement dataElement)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists cl = new check_lists();
                    cl.CreatedAt = DateTime.Now;
                    cl.UpdatedAt = DateTime.Now;
                    cl.Label = dataElement.Label;
                    if (dataElement.Description != null)
                        cl.Description = dataElement.Description.InderValue;
                    else
                        cl.Description = "";

                    //serialized_default_values - Ruby colume
                    cl.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    cl.ParentId = parentId;
                    //repeated - used for mainElements
                    cl.Version = 1;
                    //case_type - used for mainElements
                    //folder_name - used for mainElements
                    cl.DisplayIndex = dataElement.DisplayOrder;
                    //report_file_name - Ruby colume
                    cl.ReviewEnabled = t.Bool(dataElement.ReviewEnabled);
                    //manualSync - used for mainElements
                    cl.ExtraFieldsEnabled = t.Bool(dataElement.ExtraFieldsEnabled);
                    cl.DoneButtonEnabled = t.Bool(dataElement.DoneButtonEnabled);
                    cl.ApprovalEnabled = t.Bool(dataElement.ApprovalEnabled);
                    cl.OriginalId = dataElement.Id.ToString();
                    //MultiApproval - used for mainElements
                    //FastNavigation - used for mainElements
                    //DownloadEntities - used for mainElements

                    db.check_lists.Add(cl);
                    db.SaveChanges();

                    db.check_list_versions.Add(MapCheckListVersions(cl));
                    db.SaveChanges();

                    //if (dataElement.DataItemGroupList != null)
                    //{
                    //    foreach (DataItemGroup dataItemGroup in dataElement.DataItemGroupList)
                    //    {
                    //        //CDataValue description = new CDataValue();
                    //        //description.InderValue = dataItemGroup.Description;
                    //        //FieldGroup fg = new FieldGroup(int.Parse(dataItemGroup.Id), dataItemGroup.Label, description, dataItemGroup.Color, dataItemGroup.DisplayOrder, "", dataItemGroup.DataItemList);
                    //        //CreateDataItemGroup(cl.Id, fg);
                    //        CreateDataItemGroup(cl.Id, (FieldGroup)dataItemGroup);
                    //    }
                    //}

                    if (dataElement.DataItemList != null)
                    {
                        foreach (DataItem dataItem in dataElement.DataItemList)
                        {
                            CreateDataItem(null, cl.Id, dataItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataElement failed", ex);
            }
        }

        
        //TODO
        private void CreateDataItemGroup(int elementId, FieldContainer fieldGroup)
        {
            try
            {
                using (var db = GetContext())
                {
                    string typeStr = fieldGroup.GetType().ToString().Remove(0, 10); //10 = "eFormData.".Length
                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();
                    field.ParentFieldId = null;
                    field.Color = fieldGroup.Color;
                    //CDataValue description = new CDataValue();
                    //description.InderValue = fieldGroup.Description;
                    field.Description = fieldGroup.Description.InderValue;
                    field.DisplayIndex = fieldGroup.DisplayOrder;
                    field.Label = fieldGroup.Label;

                    field.CreatedAt = DateTime.Now;
                    field.UpdatedAt = DateTime.Now;
                    field.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    field.CheckListId = elementId;
                    field.FieldTypeId = fieldTypeId;
                    field.Version = 1;

                    field.DefaultValue = fieldGroup.Value;

                    db.fields.Add(field);
                    db.SaveChanges();

                    db.field_versions.Add(MapFieldVersions(field));
                    db.SaveChanges();

                    if (fieldGroup.DataItemList != null)
                    {
                        foreach (DataItem dataItem in fieldGroup.DataItemList)
                        {
                            CreateDataItem(field.Id, elementId, dataItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataItemGroup failed", ex);
            }
        }

        
        //TODO
        private void CreateDataItem(int? parentFieldId, int elementId, DataItem dataItem)
        {
            try
            {
                using (var db = GetContext())
                {
                    string typeStr = dataItem.GetType().ToString().Remove(0, 10); //10 = "eFormData.".Length

                    /*
                     * Hack for making the FieldContainer work, since it's actually a FieldGroup 
                     */
                    if (typeStr.Equals("FieldContainer"))
                        typeStr = "FieldGroup";

                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();
                    field.Color = dataItem.Color;
                    field.ParentFieldId = parentFieldId;
                    if (dataItem.Description != null)
                        field.Description = dataItem.Description.InderValue;
                    else
                        field.Description = "";
                    field.DisplayIndex = dataItem.DisplayOrder;
                    field.Label = dataItem.Label;
                    field.Mandatory = t.Bool(dataItem.Mandatory);
                    field.ReadOnly = t.Bool(dataItem.ReadOnly);
                    field.Dummy = t.Bool(dataItem.Dummy);

                    field.CreatedAt = DateTime.Now;
                    field.UpdatedAt = DateTime.Now;
                    field.WorkflowState = Constants.Constants.WorkflowStates.Created;
                    field.CheckListId = elementId;
                    field.FieldTypeId = fieldTypeId;
                    field.Version = 1;
                    field.OriginalId = dataItem.Id.ToString();

                    bool isSaved = false; // This is done, because we need to have the current field Id, for giving it onto the child fields in a FieldGroup

                    #region dataItem type
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
                            field.SplitScreen = t.Bool(comment.SplitScreen);
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
                            field.KeyValuePairList = PairBuild(multiSelect.KeyValuePairList);
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
                            field.KeyValuePairList = PairBuild(singleSelect.KeyValuePairList);
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
                            field.SplitScreen = t.Bool(timer.StopOnSave);
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
                            db.fields.Add(field);
                            db.SaveChanges();

                            db.field_versions.Add(MapFieldVersions(field));
                            db.SaveChanges();
                            isSaved = true;
                            if (fg.DataItemList != null)
                            {
                                foreach (DataItem data_item in fg.DataItemList)
                                {
                                    CreateDataItem(field.Id, elementId, data_item);
                                }
                            }
                            //CreateDataItemGroup(elementId, (FieldGroup)dataItem);
                            break;

                        default:
                            throw new IndexOutOfRangeException(dataItem.GetType().ToString() + " is not a known/mapped DataItem type");
                    }
                    #endregion
                    if (!isSaved)
                    {
                        db.fields.Add(field);
                        db.SaveChanges();

                        db.field_versions.Add(MapFieldVersions(field));
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataItem failed", ex);
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
        private Element GetElement(int elementId)
        {
            try
            {
                using (var db = GetContext())
                {
                    Element element;

                    //getting element's possible element children
                    List<check_lists> lstElement = db.check_lists.Where(x => x.ParentId == elementId).ToList();


                    if (lstElement.Count > 0) //GroupElement
                    {
                        //list for the DataItems
                        List<Element> lst = new List<Element>();

                        //the actual DataElement
                        try
                        {
                            check_lists cl = db.check_lists.Single(x => x.Id == elementId);
                            GroupElement gElement = new GroupElement(cl.Id, 
                                cl.Label, 
                                t.Int(cl.DisplayIndex), 
                                cl.Description, 
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
                                lst.Add(GetElement(subElement.Id));
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
                            check_lists cl = db.check_lists.Single(x => x.Id == elementId);
                            DataElement dElement = new DataElement(cl.Id, 
                                cl.Label,
                                t.Int(cl.DisplayIndex), 
                                cl.Description, 
                                t.Bool(cl.ApprovalEnabled), 
                                t.Bool(cl.ReviewEnabled),
                                t.Bool(cl.DoneButtonEnabled), 
                                t.Bool(cl.ExtraFieldsEnabled), 
                                "", 
                                t.Bool(cl.QuickSyncEnabled), 
                                new List<DataItemGroup>(), 
                                new List<DataItem>());

                            //the actual DataItems
                            List<fields> lstFields = db.fields.Where(x => x.CheckListId == elementId && x.ParentFieldId == null).ToList();
                            foreach (var field in lstFields)
                            {
                                GetDataItem(dElement.DataItemList, dElement.DataItemGroupList, field.Id);
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
            }
            catch (Exception ex)
            {
                throw new Exception("GetElement failed", ex);
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
        private void GetDataItem(List<DataItem> lstDataItem, List<DataItemGroup> lstDataItemGroup, int dataItemId)
        {
            try
            {
                using (var db = GetContext())
                {
                    fields f = db.fields.Single(x => x.Id == dataItemId);
                    string fieldTypeStr = Find(t.Int(f.FieldTypeId));

                    //KEY POINT - mapping
                    switch (fieldTypeStr)
                    {
                        case Constants.Constants.FieldTypes.Audio:
                            lstDataItem.Add(new Audio(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                t.Int(f.Multi)));
                            break;

                        case Constants.Constants.FieldTypes.CheckBox:
                            lstDataItem.Add(new CheckBox(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                t.Bool(f.DefaultValue), t.Bool(f.Selected)));
                            break;

                        case Constants.Constants.FieldTypes.Comment:
                            lstDataItem.Add(new Comment(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                f.DefaultValue, t.Int(f.MaxLength), t.Bool(f.SplitScreen)));
                            break;

                        case Constants.Constants.FieldTypes.Date:
                            lstDataItem.Add(new Date(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                DateTime.Parse(f.MinValue), DateTime.Parse(f.MaxValue), f.DefaultValue));
                            break;

                        case Constants.Constants.FieldTypes.None:
                            lstDataItem.Add(new None(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy)));
                            break;

                        case Constants.Constants.FieldTypes.Number:
                            lstDataItem.Add(new Number(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                f.MinValue, f.MaxValue, int.Parse(f.DefaultValue), t.Int(f.DecimalCount), f.UnitName));
                            break;

                        case Constants.Constants.FieldTypes.NumberStepper:
                            lstDataItem.Add(new NumberStepper(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                f.MinValue, f.MaxValue, int.Parse(f.DefaultValue), t.Int(f.DecimalCount), f.UnitName));
                            break;

                        case Constants.Constants.FieldTypes.MultiSelect:
                            lstDataItem.Add(new MultiSelect(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                PairRead(f.KeyValuePairList)));
                            break;

                        case Constants.Constants.FieldTypes.Picture:
                            lstDataItem.Add(new Picture(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                t.Int(f.Multi), t.Bool(f.GeolocationEnabled)));
                            break;

                        case Constants.Constants.FieldTypes.SaveButton:
                            lstDataItem.Add(new SaveButton(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                f.DefaultValue));
                            break;

                        case Constants.Constants.FieldTypes.ShowPdf:
                            lstDataItem.Add(new ShowPdf(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                f.DefaultValue));
                            break;

                        case Constants.Constants.FieldTypes.Signature:
                            lstDataItem.Add(new Signature(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy)));
                            break;

                        case Constants.Constants.FieldTypes.SingleSelect:
                            lstDataItem.Add(new SingleSelect(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                PairRead(f.KeyValuePairList)));
                            break;

                        case Constants.Constants.FieldTypes.Text:
                            lstDataItem.Add(new Text(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                f.DefaultValue, t.Int(f.MaxLength), t.Bool(f.GeolocationEnabled), t.Bool(f.GeolocationForced), t.Bool(f.GeolocationHidden), t.Bool(f.BarcodeEnabled), f.BarcodeType));
                            break;

                        case Constants.Constants.FieldTypes.Timer:
                            lstDataItem.Add(new Timer(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                t.Bool(f.StopOnSave)));
                            break;

                        case Constants.Constants.FieldTypes.EntitySearch:
                            lstDataItem.Add(new EntitySearch(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                t.Int(f.DefaultValue), t.Int(f.EntityGroupId), t.Bool(f.IsNum), f.QueryType, t.Int(f.MinValue), t.Bool(f.BarcodeEnabled), f.BarcodeType));
                            break;

                        case Constants.Constants.FieldTypes.EntitySelect:
                            lstDataItem.Add(new EntitySelect(t.Int(f.Id), t.Bool(f.Mandatory), t.Bool(f.ReadOnly), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), t.Bool(f.Dummy),
                                t.Int(f.DefaultValue), t.Int(f.EntityGroupId)));
                            break;

                        case Constants.Constants.FieldTypes.FieldGroup:
                            List<DataItem> lst = new List<DataItem>();
                            //CDataValue description = new CDataValue();
                            //description.InderValue = f.description;
                            lstDataItemGroup.Add(new FieldGroup(f.Id.ToString(), f.Label, f.Description, f.Color, t.Int(f.DisplayIndex), f.DefaultValue, lst));
                            //lstDataItemGroup.Add(new DataItemGroup(f.Id.ToString(), f.label, f.description, f.color, t.Int(f.display_index), f.default_value, lst));

                            //the actual DataItems
                            List<fields> lstFields = db.fields.Where(x => x.ParentFieldId == f.Id).ToList();
                            foreach (var field in lstFields)
                                GetDataItem(lst, null, field.Id); //null, due to FieldGroup, CANT have fieldGroups under them
                            break;

                        default:
                            throw new IndexOutOfRangeException(f.FieldTypeId + " is not a known/mapped DataItem type");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetDataItem failed", ex);
            }
        }

        //TODO
        private void GetConverter()
        {
            try
            {
                using (var db = GetContext())
                {
                    converter = new List<Holder>();

                    List<field_types> lstFieldType = db.field_types.ToList();

                    foreach (var fieldType in lstFieldType)
                    {
                        converter.Add(new Holder(fieldType.Id, fieldType.FieldType));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetConverter failed", ex);
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
        public List<Tag> GetAllTags(bool includeRemoved)
        {
            List<Tag> tags = new List<Tag>();
            try
            {
                using (var db = GetContext())
                {
                    List<tags> matches = null;
                    if (!includeRemoved)
                        matches = db.tags.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToList();
                    else
                        matches = db.tags.ToList();

                    foreach (tags tag in matches)
                    {
                        Tag t = new Tag(tag.Id, tag.Name, tag.TaggingsCount);
                        tags.Add(t);
                    }
                }
                return tags;
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllTags failed", ex);
            }
        }

        /// <summary>
        /// Create tag with given name and saves it in DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int TagCreate(string name)
        {
            try
            {
                using (var db = GetContext())
                {
                    tags tag = db.tags.SingleOrDefault(x => x.Name == name);
                    if (tag == null)
                    {
                        tag = new tags();
                        tag.Name = name;
                        tag.WorkflowState = Constants.Constants.WorkflowStates.Created;
                        tag.Version = 1;
                        db.tags.Add(tag);
                        db.SaveChanges();

                        db.tag_versions.Add(MapTagVersions(tag));
                        db.SaveChanges();
                        return tag.Id;
                    } else
                    {
                        tag.WorkflowState = Constants.Constants.WorkflowStates.Created;
                        tag.Version += 1;
                        db.SaveChanges();

                        db.tag_versions.Add(MapTagVersions(tag));
                        db.SaveChanges();
                        return tag.Id;
                    }                    
                }
                //return ;
            }
            catch (Exception ex)
            {
                throw new Exception("TagCreate failed", ex);
            }
        }

        /// <summary>
        /// Deletes tag with specific id from DB
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool TagDelete(int tagId)
        {
            try
            {
                using (var db = GetContext())
                {
                    tags tag = db.tags.SingleOrDefault(x => x.Id == tagId);
                    if (tag != null)                    
                    {
                        tag.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                        tag.Version += 1;
                        db.SaveChanges();

                        db.tag_versions.Add(MapTagVersions(tag));
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("TagDelete failed", ex);
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
                selected = bool.Parse(t.Locate(inderStr.ToLower(), "<selected>", "</"));
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

        #region mappers
       /// <summary>
       /// Mapping case versions
       /// </summary>
       /// <param name="aCase"></param>
       /// <returns></returns>
        private case_versions MapCaseVersions(cases aCase)
        {
            case_versions caseVer = new case_versions();
            caseVer.Status = aCase.Status;
            caseVer.DoneAt = aCase.DoneAt;
            caseVer.UpdatedAt = aCase.UpdatedAt;
            caseVer.WorkerId = aCase.WorkerId;
            caseVer.WorkflowState = aCase.WorkflowState;
            caseVer.Version = aCase.Version;
            caseVer.MicrotingCheckUid = aCase.MicrotingCheckUid;
            caseVer.UnitId = aCase.UnitId;

            caseVer.Type = aCase.Type;
            caseVer.CreatedAt = aCase.CreatedAt;
            caseVer.CheckListId = aCase.CheckListId;
            caseVer.MicrotingUid = aCase.MicrotingUid;
            caseVer.SiteId = aCase.SiteId;
            caseVer.FieldValue1 = aCase.FieldValue1;
            caseVer.FieldValue2 = aCase.FieldValue2;
            caseVer.FieldValue3 = aCase.FieldValue3;
            caseVer.FieldValue4 = aCase.FieldValue4;
            caseVer.FieldValue5 = aCase.FieldValue5;
            caseVer.FieldValue6 = aCase.FieldValue6;
            caseVer.FieldValue7 = aCase.FieldValue7;
            caseVer.FieldValue8 = aCase.FieldValue8;
            caseVer.FieldValue9 = aCase.FieldValue9;
            caseVer.FieldValue10 = aCase.FieldValue10;

            caseVer.CaseId = aCase.Id; //<<--

            return caseVer;
        }

        private check_list_versions MapCheckListVersions(check_lists checkList)
        {
            check_list_versions clv = new check_list_versions();
            clv.CreatedAt = checkList.CreatedAt;
            clv.UpdatedAt = checkList.UpdatedAt;
            clv.Label = checkList.Label;
            clv.Description = checkList.Description;
            clv.Custom = checkList.Custom;
            clv.WorkflowState = checkList.WorkflowState;
            clv.ParentId = checkList.ParentId;
            clv.Repeated = checkList.Repeated;
            clv.Version = checkList.Version;
            clv.CaseType = checkList.CaseType;
            clv.FolderName = checkList.FolderName;
            clv.DisplayIndex = checkList.DisplayIndex;
            clv.ReviewEnabled = checkList.ReviewEnabled;
            clv.ManualSync = checkList.ManualSync;
            clv.ExtraFieldsEnabled = checkList.ExtraFieldsEnabled;
            clv.DoneButtonEnabled = checkList.DoneButtonEnabled;
            clv.ApprovalEnabled = checkList.ApprovalEnabled;
            clv.MultiApproval = checkList.MultiApproval;
            clv.FastNavigation = checkList.FastNavigation;
            clv.DownloadEntities = checkList.DownloadEntities;
            clv.Field1 = checkList.Field1;
            clv.Field2 = checkList.Field2;
            clv.Field3 = checkList.Field3;
            clv.Field4 = checkList.Field4;
            clv.Field5 = checkList.Field5;
            clv.Field6 = checkList.Field6;
            clv.Field7 = checkList.Field7;
            clv.Field8 = checkList.Field8;
            clv.Field9 = checkList.Field9;
            clv.Field10 = checkList.Field10;

            clv.CheckListId = checkList.Id; //<<--

            return clv;
        }

        private check_list_value_versions MapCheckListValueVersions(check_list_values checkListValue)
        {
            check_list_value_versions clvv = new check_list_value_versions();
            clvv.Version = checkListValue.Version;
            clvv.CreatedAt = checkListValue.CreatedAt;
            clvv.UpdatedAt = checkListValue.UpdatedAt;
            clvv.CheckListId = checkListValue.CheckListId;
            clvv.CaseId = checkListValue.CaseId;
            clvv.Status = checkListValue.Status;
            clvv.UserId = checkListValue.UserId;
            clvv.WorkflowState = checkListValue.WorkflowState;
            clvv.CheckListDuplicateId = checkListValue.CheckListDuplicateId;

            clvv.CheckListValueId = checkListValue.Id; //<<--

            return clvv;
        }

        private field_versions MapFieldVersions(fields field)
        {
            field_versions fv = new field_versions();

            fv.Version = field.Version;
            fv.CreatedAt = field.CreatedAt;
            fv.UpdatedAt = field.UpdatedAt;
            fv.Custom = field.Custom;
            fv.WorkflowState = field.WorkflowState;
            fv.CheckListId = field.CheckListId;
            fv.Label = field.Label;
            fv.Description = field.Description;
            fv.FieldTypeId = field.FieldTypeId;
            fv.DisplayIndex = field.DisplayIndex;
            fv.Dummy = field.Dummy;
            fv.ParentFieldId = field.ParentFieldId;
            fv.Optional = field.Optional;
            fv.Multi = field.Multi;
            fv.DefaultValue = field.DefaultValue;
            fv.Selected = field.Selected;
            fv.MinValue = field.MinValue;
            fv.MaxValue = field.MaxValue;
            fv.MaxLength = field.MaxLength;
            fv.SplitScreen = field.SplitScreen;
            fv.DecimalCount = field.DecimalCount;
            fv.UnitName = field.UnitName;
            fv.KeyValuePairList = field.KeyValuePairList;
            fv.GeolocationEnabled = field.GeolocationEnabled;
            fv.GeolocationForced = field.GeolocationForced;
            fv.GeolocationHidden = field.GeolocationHidden;
            fv.StopOnSave = field.StopOnSave;
            fv.Mandatory = field.Mandatory;
            fv.ReadOnly = field.ReadOnly;
            fv.Color = field.Color;
            fv.BarcodeEnabled = field.BarcodeEnabled;
            fv.BarcodeType = field.BarcodeType;

            fv.FieldId = field.Id; //<<--

            return fv;
        }

        private field_value_versions MapFieldValueVersions(field_values fieldValue)
        {
            field_value_versions fvv = new field_value_versions();

            fvv.CreatedAt = fieldValue.CreatedAt;
            fvv.UpdatedAt = fieldValue.UpdatedAt;
            fvv.Value = fieldValue.Value;
            fvv.Latitude = fieldValue.Latitude;
            fvv.Longitude = fieldValue.Longitude;
            fvv.Altitude = fieldValue.Altitude;
            fvv.Heading = fieldValue.Heading;
            fvv.Date = fieldValue.Date;
            fvv.Accuracy = fieldValue.Accuracy;
            fvv.UploadedDataId = fieldValue.UploadedDataId;
            fvv.Version = fieldValue.Version;
            fvv.CaseId = fieldValue.CaseId;
            fvv.FieldId = fieldValue.FieldId;
            fvv.WorkerId = fieldValue.WorkerId;
            fvv.WorkflowState = fieldValue.WorkflowState;
            fvv.CheckListId = fieldValue.CheckListId;
            fvv.CheckListDuplicateId = fieldValue.CheckListDuplicateId;
            fvv.DoneAt = fieldValue.DoneAt;

            fvv.FieldValueId = fieldValue.Id; //<<--

            return fvv;
        }

        private uploaded_data_versions MapUploadedDataVersions(uploaded_data uploadedData)
        {
            uploaded_data_versions udv = new uploaded_data_versions();

            udv.CreatedAt = uploadedData.CreatedAt;
            udv.UpdatedAt = uploadedData.UpdatedAt;
            udv.Checksum = uploadedData.Checksum;
            udv.Extension = uploadedData.Extension;
            udv.CurrentFile = uploadedData.CurrentFile;
            udv.UploaderId = uploadedData.UploaderId;
            udv.UploaderType = uploadedData.UploaderType;
            udv.WorkflowState = uploadedData.WorkflowState;
            udv.ExpirationDate = uploadedData.ExpirationDate;
            udv.Version = uploadedData.Version;
            udv.Local = uploadedData.Local;
            udv.FileLocation = uploadedData.FileLocation;
            udv.FileName = uploadedData.FileName;

            udv.DataUploadedId = uploadedData.Id; //<<--

            return udv;
        }

        private check_list_site_versions MapCheckListSiteVersions(check_list_sites checkListSite)
        {
            check_list_site_versions checkListSiteVer = new check_list_site_versions();
            checkListSiteVer.CheckListId = checkListSite.CheckListId;
            checkListSiteVer.CreatedAt = checkListSite.CreatedAt;
            checkListSiteVer.UpdatedAt = checkListSite.UpdatedAt;
            checkListSiteVer.LastCheckId = checkListSite.LastCheckId;
            checkListSiteVer.MicrotingUid = checkListSite.MicrotingUid;
            checkListSiteVer.SiteId = checkListSite.SiteId;
            checkListSiteVer.Version = checkListSite.Version;
            checkListSiteVer.WorkflowState = checkListSite.WorkflowState;

            checkListSiteVer.CheckListSiteId = checkListSite.Id; //<<--

            return checkListSiteVer;
        }

        private entity_group_versions MapEntityGroupVersions(entity_groups entityGroup)
        {
            entity_group_versions entityGroupVer = new entity_group_versions();
            entityGroupVer.CreatedAt = entityGroup.CreatedAt;
            entityGroupVer.EntityGroupId = entityGroup.Id;
            entityGroupVer.MicrotingUid = entityGroup.MicrotingUid;
            entityGroupVer.Name = entityGroup.Name;
            entityGroupVer.Type = entityGroup.Type;
            entityGroupVer.UpdatedAt = entityGroup.UpdatedAt;
            entityGroupVer.Version = entityGroup.Version;
            entityGroupVer.WorkflowState = entityGroup.WorkflowState;

            entityGroupVer.EntityGroupId = entityGroup.Id; //<<--

            return entityGroupVer;
        }

        private entity_item_versions MapEntityItemVersions(entity_items entityItem)
        {
            entity_item_versions entityItemVer = new entity_item_versions();
            entityItemVer.WorkflowState = entityItem.WorkflowState;
            entityItemVer.Version = entityItem.Version;
            entityItemVer.CreatedAt = entityItem.CreatedAt;
            entityItemVer.UpdatedAt = entityItem.UpdatedAt;
            entityItemVer.EntityItemUid = entityItem.EntityItemUid;
            entityItemVer.MicrotingUid = entityItem.MicrotingUid;
            entityItemVer.EntityGroupId = entityItem.EntityGroupId;
            entityItemVer.Name = entityItem.Name;
            entityItemVer.Description = entityItem.Description;
            entityItemVer.Synced = entityItem.Synced;
            entityItemVer.DisplayIndex = entityItem.DisplayIndex;

            entityItemVer.EntityItemsId = entityItem.Id; //<<--

            return entityItemVer;
        }

        private site_worker_versions MapSiteWorkerVersions(site_workers site_workers)
        {
            site_worker_versions siteWorkerVer = new site_worker_versions();
            siteWorkerVer.WorkflowState = site_workers.WorkflowState;
            siteWorkerVer.Version = site_workers.Version;
            siteWorkerVer.CreatedAt = site_workers.CreatedAt;
            siteWorkerVer.UpdatedAt = site_workers.UpdatedAt;
            siteWorkerVer.MicrotingUid = site_workers.MicrotingUid;
            siteWorkerVer.SiteId = site_workers.SiteId;
            siteWorkerVer.WorkerId = site_workers.WorkerId;

            siteWorkerVer.SiteWorkerId = site_workers.Id; //<<--

            return siteWorkerVer;
        }

        private site_versions MapSiteVersions(sites site)
        {
            site_versions siteVer = new site_versions();
            siteVer.WorkflowState = site.WorkflowState;
            siteVer.Version = site.Version;
            siteVer.CreatedAt = site.CreatedAt;
            siteVer.UpdatedAt = site.UpdatedAt;
            siteVer.MicrotingUid = site.MicrotingUid;
            siteVer.Name = site.Name;

            siteVer.SiteId = site.Id; //<<--

            return siteVer;
        }

        private unit_versions MapUnitVersions(units units)
        {
            unit_versions unitVer = new unit_versions();
            unitVer.WorkflowState = units.WorkflowState;
            unitVer.Version = units.Version;
            unitVer.CreatedAt = units.CreatedAt;
            unitVer.UpdatedAt = units.UpdatedAt;
            unitVer.MicrotingUid = units.MicrotingUid;
            unitVer.SiteId = units.SiteId;
            unitVer.CustomerNo = units.CustomerNo;
            unitVer.OtpCode = units.OtpCode;

            unitVer.UnitId = units.Id; //<<--

            return unitVer;
        }

        private worker_versions MapWorkerVersions(workers workers)
        {
            worker_versions workerVer = new worker_versions();
            workerVer.WorkflowState = workers.WorkflowState;
            workerVer.Version = workers.Version;
            workerVer.CreatedAt = workers.CreatedAt;
            workerVer.UpdatedAt = workers.UpdatedAt;
            workerVer.MicrotingUid = workers.MicrotingUid;
            workerVer.FirstName = workers.FirstName;
            workerVer.LastName = workers.LastName;

            workerVer.WorkerId = workers.Id; //<<--

            return workerVer;
        }

        private tag_versions MapTagVersions(tags tags)
        {
            tag_versions tagVer = new tag_versions();
            tagVer.WorkflowState = tags.WorkflowState;
            tagVer.Version = tags.Version;
            tagVer.CreatedAt = tags.CreatedAt;
            tagVer.UpdatedAt = tags.UpdatedAt;
            tagVer.Name = tags.Name;

            return tagVer;
        }

        
        #endregion
        #endregion       

        /// <summary>
        /// Adding field type to DB 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="fieldType"></param>
        /// <param name="description"></param>
        private void FieldTypeAdd(int Id, string fieldType, string description)
        {
            using (var db = GetContext())
            {
                if (db.field_types.Count(x => x.FieldType == fieldType) == 0)
                {
                    field_types fT = new field_types();
                    fT.FieldType = fieldType;
                    fT.Description = description;

                    db.field_types.Add(fT);
                    db.SaveChanges();
    
                }                
            }
        }
    }    
}