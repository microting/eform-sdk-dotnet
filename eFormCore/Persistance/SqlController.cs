using eFormShared;
using eFormData;
using eFormSqlController.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Data.Entity;

namespace eFormSqlController
{
    public class SqlController : LogWriter
    {
        #region var
        string connectionStr;
        bool msSql = true;
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

            if (connectionStr.ToLower().Contains("uid=") || connectionStr.ToLower().Contains("pwd="))
                msSql = false;
            else
                msSql = true;

            #region migrate if needed
            try
            {
                using (var db = GetContext())
                {
                    db.Database.CreateIfNotExists();
                    var match = db.settings.Count();
                }
            }
            catch
            {
                MigrateDb();
            }
            #endregion

            //region set default for settings if needed
            if (SettingCheckAll().Count > 0)
                SettingCreateDefaults();
        }

        private MicrotingContextInterface GetContext()
        {
            if (msSql)
                return new MicrotingDbMs(connectionStr);
            else
                return new MicrotingDbMy(connectionStr);
        }

        public bool MigrateDb()
        {
            var configuration = new Configuration();
            configuration.TargetDatabase = new DbConnectionInfo(connectionStr, "System.Data.SqlClient");
            var migrator = new DbMigrator(configuration);
            migrator.Update();
            //migrator.Update("201708311254324_ChangingFieldDefaultValueMaxLength");
            return true;
        }
        #endregion

        #region public
        #region public template
        public int TemplateCreate(MainElement mainElement)
        {
            try
            {
                int id = EformCreateDb(mainElement);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("TemplatCreate failed", ex);
            }
        }

        public MainElement TemplateRead(int templateId)
        {
            try
            {
                using (var db = GetContext())
                {
                    MainElement mainElement = null;
                    GetConverter();

                    check_lists mainCl = db.check_lists.SingleOrDefault(x => x.id == templateId && (x.parent_id == null || x.parent_id == 0));

                    if (mainCl == null)
                        return null;

                    mainElement = new MainElement(mainCl.id, mainCl.label, t.Int(mainCl.display_index), mainCl.folder_name, t.Int(mainCl.repeated), DateTime.Now, DateTime.Now.AddDays(2), "da",
                        t.Bool(mainCl.multi_approval), t.Bool(mainCl.fast_navigation), t.Bool(mainCl.download_entities), t.Bool(mainCl.manual_sync), mainCl.case_type, "", "", new List<Element>());

                    //getting elements
                    List<check_lists> lst = db.check_lists.Where(x => x.parent_id == templateId).ToList();
                    foreach (check_lists cl in lst)
                    {
                        mainElement.ElementList.Add(GetElement(cl.id));
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

        public Template_Dto TemplateItemRead(int templateId)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists checkList = db.check_lists.SingleOrDefault(x => x.id == templateId);

                    if (checkList == null)
                        return null;

                    List<SiteName_Dto> sites = new List<SiteName_Dto>();
                    foreach (check_list_sites check_list_site in checkList.check_list_sites.Where(x => x.workflow_state != "removed").ToList())
                    {
                        SiteName_Dto site = new SiteName_Dto((int)check_list_site.site.microting_uid, check_list_site.site.name, check_list_site.site.created_at, check_list_site.site.updated_at);
                        sites.Add(site);
                    }
                    bool hasCases = false;
                    if (checkList.cases.Count() > 0)
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

                    fields f1 = db.fields.SingleOrDefault(x => x.id == checkList.field_1);
                    if (f1 != null)
                        fd1 = new Field_Dto(f1.id, f1.label, f1.description, (int)f1.field_type_id, f1.field_type.field_type, (int)f1.check_list_id);

                    fields f2 = db.fields.SingleOrDefault(x => x.id == checkList.field_2);
                    if (f2 != null)
                        fd2 = new Field_Dto(f2.id, f2.label, f2.description, (int)f2.field_type_id, f2.field_type.field_type, (int)f2.check_list_id);

                    fields f3 = db.fields.SingleOrDefault(x => x.id == checkList.field_3);
                    if (f3 != null)
                        fd3 = new Field_Dto(f3.id, f3.label, f3.description, (int)f3.field_type_id, f3.field_type.field_type, (int)f3.check_list_id);

                    fields f4 = db.fields.SingleOrDefault(x => x.id == checkList.field_4);
                    if (f4 != null)
                        fd4 = new Field_Dto(f4.id, f4.label, f4.description, (int)f4.field_type_id, f4.field_type.field_type, (int)f4.check_list_id);

                    fields f5 = db.fields.SingleOrDefault(x => x.id == checkList.field_5);
                    if (f5 != null)
                        fd5 = new Field_Dto(f5.id, f5.label, f5.description, (int)f5.field_type_id, f5.field_type.field_type, (int)f5.check_list_id);

                    fields f6 = db.fields.SingleOrDefault(x => x.id == checkList.field_6);
                    if (f6 != null)
                        fd6 = new Field_Dto(f6.id, f6.label, f6.description, (int)f6.field_type_id, f6.field_type.field_type, (int)f6.check_list_id);

                    fields f7 = db.fields.SingleOrDefault(x => x.id == checkList.field_7);
                    if (f7 != null)
                        fd7 = new Field_Dto(f7.id, f7.label, f7.description, (int)f7.field_type_id, f7.field_type.field_type, (int)f7.check_list_id);

                    fields f8 = db.fields.SingleOrDefault(x => x.id == checkList.field_8);
                    if (f8 != null)
                        fd8 = new Field_Dto(f8.id, f8.label, f8.description, (int)f8.field_type_id, f8.field_type.field_type, (int)f8.check_list_id);

                    fields f9 = db.fields.SingleOrDefault(x => x.id == checkList.field_9);
                    if (f9 != null)
                        fd9 = new Field_Dto(f9.id, f9.label, f9.description, (int)f9.field_type_id, f9.field_type.field_type, (int)f9.check_list_id);

                    fields f10 = db.fields.SingleOrDefault(x => x.id == checkList.field_10);
                    if (f10 != null)
                        fd10 = new Field_Dto(f10.id, f10.label, f10.description, (int)f10.field_type_id, f10.field_type.field_type, (int)f10.check_list_id);
                    #endregion

                    #region loadtags
                    List<taggings> matches = checkList.taggings.ToList();
                    List<KeyValuePair<int, string>> check_list_tags = new List<KeyValuePair<int, string>>();
                    foreach (taggings tagging in matches)
                    {
                        KeyValuePair<int, string> kvp = new KeyValuePair<int, string>((int)tagging.tag_id, tagging.tag.name);
                        check_list_tags.Add(kvp);
                    }
                    #endregion

                    Template_Dto templateDto = new Template_Dto(checkList.id, checkList.created_at, checkList.updated_at, checkList.label, checkList.description, (int)checkList.repeated, checkList.folder_name, checkList.workflow_state, sites, hasCases, checkList.display_index, fd1, fd2, fd3, fd4, fd5, fd6, fd7, fd8, fd9, fd10, check_list_tags);
                    return templateDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public List<Template_Dto> TemplateItemReadAll(bool includeRemoved, string siteWorkflowState, string searchKey, bool descendingSort, string sortParameter, List<int> tagIds)
        {
            string methodName = t.GetMethodName();
            log.LogStandard("Not Specified", methodName + " called");
            log.LogVariable("Not Specified", nameof(includeRemoved), includeRemoved);
            log.LogVariable("Not Specified", nameof(searchKey), searchKey);
            log.LogVariable("Not Specified", nameof(descendingSort), descendingSort);
            log.LogVariable("Not Specified", nameof(sortParameter), sortParameter);
            log.LogVariable("Not Specified", nameof(tagIds), tagIds.ToString());
            try
            {
                List<Template_Dto> templateList = new List<Template_Dto>();

                using (var db = GetContext())
                {
                    List<check_lists> matches = null;
                    IQueryable<check_lists> sub_query = db.check_lists.Where(x => x.parent_id == null);

                    if (!includeRemoved)
                        sub_query = sub_query.Where(x => x.workflow_state == Constants.WorkflowStates.Created);

                    switch (sortParameter)
                    {
                        case Constants.TamplateSortParameters.Label:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.label);
                            else
                                sub_query = sub_query.OrderBy(x => x.label);
                            break;
                        case Constants.TamplateSortParameters.Description:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.description);
                            else
                                sub_query = sub_query.OrderBy(x => x.description);
                            break;
                        case Constants.TamplateSortParameters.CreatedAt:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.created_at);
                            else
                                sub_query = sub_query.OrderBy(x => x.created_at);
                            break;
                        default:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.id);
                            else
                                sub_query = sub_query.OrderBy(x => x.id);
                            break;
                    }

                    matches = sub_query.ToList();

                    //if (includeRemoved)
                    //    matches = db.check_lists.Where(x => x.parent_id == null).ToList();
                    //else
                    //    matches = db.check_lists.Where(x => x.parent_id == null && x.workflow_state == Constants.WorkflowStates.Created).ToList();

                    foreach (check_lists checkList in matches)
                    {
                        List<SiteName_Dto> sites = new List<SiteName_Dto>();
                        List<check_list_sites> check_list_sites = null;

                        if (siteWorkflowState == Constants.WorkflowStates.Removed)
                            check_list_sites = checkList.check_list_sites.Where(x => x.workflow_state == Constants.WorkflowStates.Removed).ToList();
                        else
                            check_list_sites = checkList.check_list_sites.Where(x => x.workflow_state != Constants.WorkflowStates.Removed).ToList();

                        foreach (check_list_sites check_list_site in check_list_sites)
                        {
                            SiteName_Dto site = new SiteName_Dto((int)check_list_site.site.microting_uid, check_list_site.site.name, check_list_site.site.created_at, check_list_site.site.updated_at);
                            sites.Add(site);
                        }
                        bool hasCases = false;
                        if (checkList.cases.Count() > 0)
                            hasCases = true;

                        #region loadtags
                        List<taggings> tagging_matches = checkList.taggings.ToList();
                        List<KeyValuePair<int, string>> check_list_tags = new List<KeyValuePair<int, string>>();
                        foreach (taggings tagging in tagging_matches)
                        {
                            KeyValuePair<int, string> kvp = new KeyValuePair<int, string>((int)tagging.tag_id, tagging.tag.name);
                            check_list_tags.Add(kvp);
                        }
                        #endregion

                        Template_Dto templateDto = new Template_Dto(checkList.id, checkList.created_at, checkList.updated_at, checkList.label, checkList.description, (int)checkList.repeated, checkList.folder_name, checkList.workflow_state, sites, hasCases, checkList.display_index, check_list_tags);
                        templateList.Add(templateDto);
                    }
                    return templateList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TemplateSimpleReadAll failed", ex);
            }


        }

        public List<Field_Dto> TemplateFieldReadAll(int templateId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    MainElement mainElement = TemplateRead(templateId);
                    List<Field_Dto> fieldLst = new List<Field_Dto>();

                    foreach (var dataItem in mainElement.DataItemGetAll())
                    {
                        fields field = db.fields.Single(x => x.id == dataItem.Id);
                        Field_Dto fieldDto = new Field_Dto(field.id, field.label, field.description, (int)field.field_type_id, field.field_type.field_type, (int)field.check_list_id);
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

        public bool TemplateDisplayIndexChange(int templateId, int newDisplayIndex)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists checkList = db.check_lists.SingleOrDefault(x => x.id == templateId);

                    if (checkList == null)
                        return false;

                    checkList.updated_at = DateTime.Now;
                    checkList.version = checkList.version + 1;
                    checkList.display_index = newDisplayIndex;

                    db.check_list_versions.Add(MapCheckListVersions(checkList));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public bool TemplateUpdateFieldIdsForColumns(int templateId, int? fieldId1, int? fieldId2, int? fieldId3, int? fieldId4, int? fieldId5, int? fieldId6, int? fieldId7, int? fieldId8, int? fieldId9, int? fieldId10)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists checkList = db.check_lists.SingleOrDefault(x => x.id == templateId);

                    if (checkList == null)
                        return false;

                    checkList.updated_at = DateTime.Now;
                    checkList.version = checkList.version + 1;
                    checkList.field_1 = fieldId1;
                    checkList.field_2 = fieldId2;
                    checkList.field_3 = fieldId3;
                    checkList.field_4 = fieldId4;
                    checkList.field_5 = fieldId5;
                    checkList.field_6 = fieldId6;
                    checkList.field_7 = fieldId7;
                    checkList.field_8 = fieldId8;
                    checkList.field_9 = fieldId9;
                    checkList.field_10 = fieldId10;

                    db.check_list_versions.Add(MapCheckListVersions(checkList));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public bool TemplateDelete(int templateId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    check_lists check_list = db.check_lists.Single(x => x.id == templateId);

                    if (check_list != null)
                    {
                        check_list.version = check_list.version + 1;
                        check_list.updated_at = DateTime.Now;

                        check_list.workflow_state = "removed";

                        db.check_list_versions.Add(MapCheckListVersions(check_list));
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

        public bool TemplateSetTags(int templateId, List<int> tagIds)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    check_lists check_list = db.check_lists.Single(x => x.id == templateId);

                    if (check_list != null)
                    {
                        // Delete all not wanted taggings first
                        List<taggings> cl_taggings = check_list.taggings.Where(x => !tagIds.Contains(x.id)).ToList();
                        foreach (taggings tagging in cl_taggings)
                        {
                            taggings current_tagging = db.taggings.Single(x => x.id == tagging.id);
                            if (current_tagging != null)
                            {
                                current_tagging.version = check_list.version + 1;
                                current_tagging.updated_at = DateTime.Now;

                                current_tagging.workflow_state = Constants.WorkflowStates.Removed;

                                db.tagging_versions.Add(MapTaggingVersions(current_tagging));
                                db.SaveChanges();
                            }
                        }

                        // set all new taggings
                        foreach (int id in tagIds)
                        {
                            tags tag = db.tags.Single(x => x.id == id);
                            if (tag != null)
                            {
                                taggings tagging = new taggings();
                                tagging.check_list_id = templateId;
                                tagging.tag_id = tag.id;
                                tagging.created_at = DateTime.Now;
                                tagging.workflow_state = Constants.WorkflowStates.Created;
                                tagging.version = 1;

                                db.taggings.Add(tagging);
                                db.SaveChanges();

                                db.tagging_versions.Add(MapTaggingVersions(tagging));
                                db.SaveChanges();
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
        #endregion

        #region public (pre)case
        public void CheckListSitesCreate(int checkListId, int siteUId, string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    int siteId = db.sites.Single(x => x.microting_uid == siteUId).id;

                    check_list_sites cLS = new check_list_sites();
                    cLS.check_list_id = checkListId;
                    cLS.created_at = DateTime.Now;
                    cLS.updated_at = DateTime.Now;
                    cLS.last_check_id = "0";
                    cLS.microting_uid = microtingUId;
                    cLS.site_id = siteId;
                    cLS.version = 1;
                    cLS.workflow_state = "created";

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

        public List<string> CheckListSitesRead(int templateId, int siteUId, string workflowState)
        {
            try
            {
                using (var db = GetContext())
                {
                    sites site = db.sites.Single(x => x.microting_uid == siteUId);
                    IQueryable<check_list_sites> sub_query = db.check_list_sites.Where(x => x.site_id == site.id && x.check_list_id == templateId);
                    if (workflowState == "not_removed")
                        sub_query = sub_query.Where(x => x.workflow_state != "removed");

                    return sub_query.Select(x => x.microting_uid).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckListSitesRead failed", ex);
            }
        }

        public int CaseCreate(int checkListId, int siteUId, string microtingUId, string microtingCheckId, string caseUId, string custom, DateTime createdAt)
        {
            try
            {
                using (var db = GetContext())
                {
                    string caseType = db.check_lists.Single(x => x.id == checkListId).case_type;
                    int siteId = db.sites.Single(x => x.microting_uid == siteUId).id;

                    cases aCase = null;
                    // Lets see if we have an existing case with the same parameters in the db first.
                    // This is to handle none gracefull shutdowns.                
                    aCase = db.cases.SingleOrDefault(x => x.microting_uid == microtingUId && x.microting_check_uid == microtingCheckId);

                    if (aCase == null)
                    {
                        aCase = new cases();
                        aCase.status = 66;
                        aCase.type = caseType;
                        aCase.created_at = createdAt;
                        aCase.updated_at = createdAt;
                        aCase.check_list_id = checkListId;
                        aCase.microting_uid = microtingUId;
                        aCase.microting_check_uid = microtingCheckId;
                        aCase.case_uid = caseUId;
                        aCase.workflow_state = "created";
                        aCase.version = 1;
                        aCase.site_id = siteId;

                        aCase.custom = custom;

                        db.cases.Add(aCase);
                        db.SaveChanges();

                        db.case_versions.Add(MapCaseVersions(aCase));
                        db.SaveChanges();
                    }
                    else
                    {
                        aCase.status = 66;
                        aCase.type = caseType;
                        aCase.check_list_id = checkListId;
                        aCase.microting_uid = microtingUId;
                        aCase.microting_check_uid = microtingCheckId;
                        aCase.case_uid = caseUId;
                        aCase.workflow_state = "created";
                        aCase.version = 1;
                        aCase.site_id = siteId;
                        aCase.updated_at = DateTime.Now;
                        aCase.version = aCase.version + 1;
                        aCase.custom = custom;

                        db.case_versions.Add(MapCaseVersions(aCase));
                        db.SaveChanges();
                    }

                    return aCase.id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseCreate failed", ex);
            }
        }

        public string CaseReadCheckIdByMUId(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_list_sites match = db.check_list_sites.SingleOrDefault(x => x.microting_uid == microtingUId);
                    if (match == null)
                        return null;
                    else
                        return match.last_check_id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByMuuId failed", ex);
            }
        }

        public void CaseUpdateRetrived(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases match = db.cases.SingleOrDefault(x => x.microting_uid == microtingUId);

                    if (match != null)
                    {
                        match.status = 77;
                        match.updated_at = DateTime.Now;
                        match.version = match.version + 1;

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

        public void CaseUpdateCompleted(string microtingUId, string microtingCheckId, DateTime doneAt, int userUId, int unitUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases caseStd = db.cases.SingleOrDefault(x => x.microting_uid == microtingUId && x.microting_check_uid == microtingCheckId);

                    if (caseStd == null)
                        caseStd = db.cases.Single(x => x.microting_uid == microtingUId);

                    int userId = db.workers.Single(x => x.microting_uid == userUId).id;
                    int unitId = db.units.Single(x => x.microting_uid == unitUId).id;

                    caseStd.status = 100;
                    caseStd.done_at = doneAt;
                    caseStd.updated_at = DateTime.Now;
                    caseStd.done_by_user_id = userId;
                    caseStd.workflow_state = "created";
                    caseStd.version = caseStd.version + 1;
                    caseStd.unit_id = unitId;
                    caseStd.microting_check_uid = microtingCheckId;
                    #region - update "check_list_sites" if needed
                    check_list_sites match = db.check_list_sites.SingleOrDefault(x => x.microting_uid == microtingUId);
                    if (match != null)
                    {
                        match.last_check_id = microtingCheckId;
                        match.version = match.version + 1;
                        match.updated_at = DateTime.Now;

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

        public void CaseRetract(string microtingUId, string microtingCheckId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases match = db.cases.Single(x => x.microting_uid == microtingUId && x.microting_check_uid == microtingCheckId);

                    match.updated_at = DateTime.Now;
                    match.workflow_state = "retracted";
                    match.version = match.version + 1;

                    db.case_versions.Add(MapCaseVersions(match));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseRetract failed", ex);
            }
        }

        public void CaseDelete(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases aCase = db.cases.Single(x => x.microting_uid == microtingUId && x.workflow_state != "removed" && x.workflow_state != "retracted");

                    aCase.updated_at = DateTime.Now;
                    aCase.workflow_state = Constants.WorkflowStates.Removed;
                    aCase.version = aCase.version + 1;

                    db.case_versions.Add(MapCaseVersions(aCase));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseDelete failed", ex);
            }
        }

        public bool CaseDeleteResult(int caseId)
        {

            try
            {
                using (var db = GetContext())
                {
                    cases aCase = db.cases.SingleOrDefault(x => x.id == caseId);

                    if (aCase != null)
                    {
                        aCase.workflow_state = Constants.WorkflowStates.Removed;
                        aCase.updated_at = DateTime.Now;
                        aCase.version = aCase.version + 1;

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

        public void CaseDeleteReversed(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_list_sites site = db.check_list_sites.Single(x => x.microting_uid == microtingUId);

                    site.updated_at = DateTime.Now;
                    site.workflow_state = "removed";
                    site.version = site.version + 1;

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
        public void ChecksCreate(Response response, string xmlString, int xmlIndex)
        {
            try
            {
                using (var db = GetContext())
                {
                    int elementId;
                    int userUId = int.Parse(response.Checks[xmlIndex].WorkerId);
                    int userId = db.workers.Single(x => x.microting_uid == userUId).id;
                    List<string> elements = t.LocateList(xmlString, "<ElementList>", "</ElementList>");
                    List<Field_Dto> TemplatFieldLst = null;
                    cases responseCase = null;
                    List<int?> case_fields = new List<int?>();

                    try //if a reversed case, case needs to be created
                    {
                        check_list_sites cLS = db.check_list_sites.Single(x => x.microting_uid == response.Value);
                        int caseId = CaseCreate((int)cLS.check_list_id, (int)cLS.site.microting_uid, response.Value, response.Checks[xmlIndex].Id, "ReversedCase", "", DateTime.Now);
                        responseCase = db.cases.Single(x => x.id == caseId);
                    }
                    catch //already created case id retrived
                    {
                        responseCase = db.cases.Single(x => x.microting_uid == response.Value);
                    }

                    check_lists cl = responseCase.check_list;

                    case_fields.Add(cl.field_1);
                    case_fields.Add(cl.field_2);
                    case_fields.Add(cl.field_3);
                    case_fields.Add(cl.field_4);
                    case_fields.Add(cl.field_5);
                    case_fields.Add(cl.field_6);
                    case_fields.Add(cl.field_7);
                    case_fields.Add(cl.field_8);
                    case_fields.Add(cl.field_9);
                    case_fields.Add(cl.field_10);
                    //cl.field_1

                    TemplatFieldLst = TemplateFieldReadAll((int)responseCase.check_list_id);

                    foreach (string elementStr in elements)
                    {
                        #region foreach element
                        check_list_values clv = new check_list_values();
                        clv.created_at = DateTime.Now;
                        clv.updated_at = DateTime.Now;
                        clv.check_list_id = int.Parse(t.Locate(elementStr, "<Id>", "</"));
                        clv.case_id = responseCase.id;
                        clv.status = t.Locate(elementStr, "<Status>", "</");
                        clv.version = 1;
                        clv.user_id = userId;
                        clv.workflow_state = "created";

                        db.check_list_values.Add(clv);
                        db.SaveChanges();

                        db.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                        db.SaveChanges();

                        #region foreach (string dataItemStr in dataItems)
                        elementId = clv.id;
                        List<string> dataItems = t.LocateList(elementStr, "<DataItem>", "</DataItem>");

                        if (dataItems != null)
                        {
                            foreach (string dataItemStr in dataItems)
                            {
                                field_values fieldV = new field_values();

                                #region if contains a file
                                string urlXml = t.Locate(dataItemStr, "<URL>", "</URL>");
                                if (urlXml != "" && urlXml != "none")
                                {
                                    uploaded_data dU = new uploaded_data();

                                    dU.created_at = DateTime.Now;
                                    dU.updated_at = DateTime.Now;
                                    dU.extension = t.Locate(dataItemStr, "<Extension>", "</");
                                    dU.uploader_id = userId;
                                    dU.uploader_type = Constants.UploaderTypes.System;
                                    dU.workflow_state = Constants.WorkflowStates.PreCreated;
                                    dU.version = 1;
                                    dU.local = 0;
                                    dU.file_location = t.Locate(dataItemStr, "<URL>", "</");

                                    db.uploaded_data.Add(dU);
                                    db.SaveChanges();

                                    db.uploaded_data_versions.Add(MapUploadedDataVersions(dU));
                                    db.SaveChanges();

                                    fieldV.uploaded_data_id = dU.id;
                                }
                                #endregion

                                fieldV.created_at = DateTime.Now;
                                fieldV.updated_at = DateTime.Now;
                                #region fieldV.value = t.Locate(xml, "<Value>", "</");
                                string temp = t.Locate(dataItemStr, "<Value>", "</");

                                if (temp.Length > 8)
                                {
                                    if (temp.StartsWith(@"<![CDATA["))
                                    {
                                        temp = temp.Substring(9);
                                        temp = temp.Substring(0, temp.Length - 3);
                                    }
                                }

                                fieldV.value = temp;
                                #endregion
                                //geo
                                fieldV.latitude = t.Locate(dataItemStr, "<Latitude>", "</");
                                fieldV.longitude = t.Locate(dataItemStr, "<Longitude>", "</");
                                fieldV.altitude = t.Locate(dataItemStr, "<Altitude>", "</");
                                fieldV.heading = t.Locate(dataItemStr, "<Heading>", "</");
                                fieldV.accuracy = t.Locate(dataItemStr, "<Accuracy>", "</");
                                fieldV.date = t.Date(t.Locate(dataItemStr, "<Date>", "</"));
                                //
                                fieldV.workflow_state = Constants.WorkflowStates.Created;
                                fieldV.version = 1;
                                fieldV.case_id = responseCase.id;
                                fieldV.field_id = int.Parse(t.Locate(dataItemStr, "<Id>", "</"));
                                fieldV.user_id = userId;
                                fieldV.check_list_id = clv.check_list_id;
                                fieldV.done_at = t.Date(response.Checks[xmlIndex].Date);

                                db.field_values.Add(fieldV);
                                db.SaveChanges();

                                db.field_value_versions.Add(MapFieldValueVersions(fieldV));
                                db.SaveChanges();

                                #region update case field_values
                                if (case_fields.Contains(fieldV.field_id))
                                {
                                    field_types field_type = db.fields.First(x => x.id == fieldV.field_id).field_type;
                                    string new_value = fieldV.value;

                                    if (field_type.field_type == "EntitySearch" || field_type.field_type == "EntitySelect")
                                    {
                                        try
                                        {
                                            if (fieldV.value != "" || fieldV.value != null)
                                            {
                                                entity_items match = db.entity_items.SingleOrDefault(x => x.microting_uid == fieldV.value);

                                                if (match != null)
                                                {
                                                    new_value = match.name;
                                                }

                                            }
                                        }
                                        catch { }
                                    }

                                    if (field_type.field_type == "SingleSelect")
                                    {
                                        string key = fieldV.value;
                                        string fullKey = t.Locate(fieldV.field.key_value_pair_list, "<" + key + ">", "</" + key + ">");
                                        new_value = t.Locate(fullKey, "<key>", "</key>");
                                    }

                                    if (field_type.field_type == "MultiSelect")
                                    {
                                        new_value = "";

                                        string keys = fieldV.value;
                                        List<string> keyLst = keys.Split('|').ToList();

                                        foreach (string key in keyLst)
                                        {
                                            string fullKey = t.Locate(fieldV.field.key_value_pair_list, "<" + key + ">", "</" + key + ">");
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


                                    int i = case_fields.IndexOf(fieldV.field_id);
                                    switch (i)
                                    {
                                        case 0:
                                            responseCase.field_value_1 = new_value;
                                            break;
                                        case 1:
                                            responseCase.field_value_2 = new_value;
                                            break;
                                        case 2:
                                            responseCase.field_value_3 = new_value;
                                            break;
                                        case 3:
                                            responseCase.field_value_4 = new_value;
                                            break;
                                        case 4:
                                            responseCase.field_value_5 = new_value;
                                            break;
                                        case 5:
                                            responseCase.field_value_6 = new_value;
                                            break;
                                        case 6:
                                            responseCase.field_value_7 = new_value;
                                            break;
                                        case 7:
                                            responseCase.field_value_8 = new_value;
                                            break;
                                        case 8:
                                            responseCase.field_value_9 = new_value;
                                            break;
                                        case 9:
                                            responseCase.field_value_10 = new_value;
                                            break;
                                    }
                                    responseCase.version = responseCase.version + 1;
                                    db.cases.AddOrUpdate(responseCase);
                                    db.SaveChanges();
                                    db.case_versions.Add(MapCaseVersions(responseCase));
                                    db.SaveChanges();
                                }

                                #endregion

                                #region remove dataItem duplicate from TemplatDataItemLst
                                int index = 0;
                                foreach (var field in TemplatFieldLst)
                                {
                                    if (fieldV.field_id == field.Id)
                                    {
                                        TemplatFieldLst.RemoveAt(index);
                                        break;
                                    }

                                    index++;
                                }
                                #endregion
                            }
                        }
                        #endregion
                        #endregion
                    }

                    #region foreach (var field in TemplatFieldLst)
                    foreach (var field in TemplatFieldLst)
                    {
                        field_values fieldV = new field_values();

                        fieldV.created_at = DateTime.Now;
                        fieldV.updated_at = DateTime.Now;

                        fieldV.value = null;

                        //geo
                        fieldV.latitude = null;
                        fieldV.longitude = null;
                        fieldV.altitude = null;
                        fieldV.heading = null;
                        fieldV.accuracy = null;
                        fieldV.date = null;
                        //
                        fieldV.workflow_state = Constants.WorkflowStates.Created;
                        fieldV.version = 1;
                        fieldV.case_id = responseCase.id;
                        fieldV.field_id = field.Id;
                        fieldV.user_id = userId;
                        fieldV.check_list_id = field.CheckListId;
                        fieldV.done_at = t.Date(response.Checks[xmlIndex].Date);

                        db.field_values.Add(fieldV);
                        db.SaveChanges();

                        db.field_value_versions.Add(MapFieldValueVersions(fieldV));
                        db.SaveChanges();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckCreateDb failed", ex);
            }
        }

        public ReplyElement CheckRead(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    var aCase = db.cases.Single(x => x.microting_uid == microtingUId && x.microting_check_uid == checkUId);
                    var mainCheckList = db.check_lists.Single(x => x.id == aCase.check_list_id);

                    ReplyElement replyElement = new ReplyElement();

                    replyElement.Id = (int)aCase.check_list_id;
                    replyElement.CaseType = aCase.type;
                    replyElement.Custom = aCase.custom;
                    replyElement.DoneAt = (DateTime)aCase.done_at;
                    replyElement.DoneById = (int)aCase.done_by_user_id;
                    replyElement.ElementList = new List<Element>();
                    //replyElement.EndDate
                    replyElement.FastNavigation = t.Bool(mainCheckList.fast_navigation);
                    replyElement.Label = mainCheckList.label;
                    //replyElement.Language
                    replyElement.ManualSync = t.Bool(mainCheckList.manual_sync);
                    replyElement.MultiApproval = t.Bool(mainCheckList.multi_approval);
                    replyElement.Repeated = (int)mainCheckList.repeated;
                    //replyElement.StartDate
                    replyElement.UnitId = (int)aCase.unit_id;

                    foreach (check_lists checkList in aCase.check_list.children)
                    {
                        replyElement.ElementList.Add(SubChecks(checkList.id, aCase.id));
                    }
                    return replyElement;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckRead failed", ex);
            }
        }

        private Element SubChecks(int parentId, int caseId)
        {
            try
            {
                using (var db = GetContext())
                {
                    var checkList = db.check_lists.Single(x => x.id == parentId);
                    //Element element = new Element();
                    if (checkList.children.Count() > 0)
                    {
                        List<Element> elementList = new List<Element>();
                        foreach (check_lists subList in checkList.children)
                        {
                            elementList.Add(SubChecks(subList.id, caseId));
                        }
                        GroupElement element = new GroupElement(checkList.id, checkList.label, (int)checkList.display_index, checkList.description, t.Bool(checkList.approval_enabled), t.Bool(checkList.review_enabled), t.Bool(checkList.done_button_enabled), t.Bool(checkList.extra_fields_enabled), "", elementList);
                        return element;
                    }
                    else
                    {
                        List<DataItemGroup> dataItemGroupList = new List<DataItemGroup>();
                        List<DataItem> dataItemList = new List<DataItem>();
                        foreach (fields field in checkList.fields.Where(x => x.parent_field_id == null).OrderBy(x => x.display_index).ToList())
                        {
                            if (field.field_type.field_type == "FieldGroup")
                            {
                                List<DataItem> dataItemSubList = new List<DataItem>();
                                foreach (fields subField in field.children)
                                {
                                    Field _field = FieldRead(subField.id);

                                    _field.FieldValues = new List<FieldValue>();
                                    foreach (field_values fieldValue in subField.field_values.Where(x => x.case_id == caseId).ToList())
                                    {
                                        FieldValue answer = FieldValueRead(subField, fieldValue, false);
                                        _field.FieldValues.Add(answer);
                                    }
                                    dataItemSubList.Add(_field);
                                }

                                CDataValue description = new CDataValue();
                                description.InderValue = field.description;
                                FieldContainer fG = new FieldContainer(field.id, field.label, description, field.color, (int)field.display_index, field.default_value, dataItemSubList);
                                dataItemList.Add(fG);
                            }
                            else
                            {
                                Field _field = FieldRead(field.id);
                                _field.FieldValues = new List<FieldValue>();
                                foreach (field_values fieldValue in field.field_values.Where(x => x.case_id == caseId).ToList())
                                {
                                    FieldValue answer = FieldValueRead(field, fieldValue, false);
                                    _field.FieldValues.Add(answer);
                                }
                                dataItemList.Add(_field);
                            }
                        }
                        DataElement dataElement = new DataElement(checkList.id, checkList.label, (int)checkList.display_index, checkList.description, t.Bool(checkList.approval_enabled), t.Bool(checkList.review_enabled), t.Bool(checkList.done_button_enabled), t.Bool(checkList.extra_fields_enabled), "", dataItemGroupList, dataItemList);
                        //return dataElement;
                        return new CheckListValue(dataElement, CheckListValueStatusRead(caseId, checkList.id));
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
                    var aCase = db.cases.Single(x => x.microting_uid == microtingUId && x.microting_check_uid == checkUId);
                    int caseId = aCase.id;

                    List<field_values> lst = db.field_values.Where(x => x.case_id == caseId).ToList();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckRead failed", ex);
            }
        }

        public Field FieldRead(int id)
        {

            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    fields fieldDb = db.fields.SingleOrDefault(x => x.id == id);

                    if (fieldDb != null)
                    {
                        Field field = new Field();
                        field.Label = fieldDb.label;
                        field.Description = new CDataValue();
                        field.Description.InderValue = fieldDb.description;
                        field.FieldType = fieldDb.field_type.field_type;
                        field.FieldValue = fieldDb.default_value;

                        if (field.FieldType == "SingleSelect")
                        {
                            field.KeyValuePairList = PairRead(fieldDb.key_value_pair_list);
                        }

                        if (field.FieldType == "MultiSelect")
                        {
                            field.KeyValuePairList = PairRead(fieldDb.key_value_pair_list);
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

        public FieldValue FieldValueRead(fields question, field_values reply, bool joinUploadedData)
        {
            try
            {
                using (var db = GetContext())
                {
                    FieldValue answer = new FieldValue();
                    answer.Accuracy = reply.accuracy;
                    answer.Altitude = reply.altitude;
                    answer.Color = question.color;
                    answer.Date = reply.date;
                    answer.FieldId = t.Int(reply.field_id);
                    answer.FieldType = question.field_type.field_type;
                    answer.DateOfDoing = t.Date(reply.done_at);
                    answer.Description = new CDataValue();
                    answer.Description.InderValue = question.description;
                    answer.DisplayOrder = t.Int(question.display_index);
                    answer.Heading = reply.heading;
                    answer.Id = reply.id;
                    answer.Label = question.label;
                    answer.Latitude = reply.latitude;
                    answer.Longitude = reply.longitude;
                    answer.Mandatory = t.Bool(question.mandatory);
                    answer.ReadOnly = t.Bool(question.read_only);
                    #region answer.UploadedDataId = reply.uploaded_data_id;
                    if (reply.uploaded_data_id.HasValue)
                        if (reply.uploaded_data_id > 0)
                        {
                            string locations = "";
                            int uploadedDataId;
                            uploaded_data uploadedData;
                            if (joinUploadedData)
                            {
                                List<field_values> lst = db.field_values.Where(x => x.case_id == reply.case_id && x.field_id == reply.field_id).ToList();

                                foreach (field_values fV in lst)
                                {
                                    uploadedDataId = (int)fV.uploaded_data_id;

                                    uploadedData = db.uploaded_data.Single(x => x.id == uploadedDataId);

                                    if (uploadedData.file_name != null)
                                        locations += uploadedData.file_location + uploadedData.file_name + Environment.NewLine;
                                    else
                                        locations += "File attached, awaiting download" + Environment.NewLine;
                                }
                                answer.UploadedData = locations.TrimEnd();
                            }
                            else
                            {
                                locations = "";
                                UploadedData uploadedDataObj = new UploadedData();
                                uploadedData = reply.uploaded_data;
                                uploadedDataObj.Checksum = uploadedData.checksum;
                                uploadedDataObj.Extension = uploadedData.extension;
                                uploadedDataObj.CurrentFile = uploadedData.current_file;
                                uploadedDataObj.UploaderId = uploadedData.uploader_id;
                                uploadedDataObj.UploaderType = uploadedData.uploader_type;
                                uploadedDataObj.FileLocation = uploadedData.file_location;
                                uploadedDataObj.FileName = uploadedData.file_name;
                                uploadedDataObj.Id = uploadedData.id;
                                answer.UploadedDataObj = uploadedDataObj;
                                answer.UploadedData = "";
                            }

                        }
                    #endregion
                    answer.Value = reply.value;
                    #region answer.ValueReadable = reply.value 'ish' //and if needed: answer.KeyValuePairList = ReadPairs(...);
                    answer.ValueReadable = reply.value;

                    if (answer.FieldType == "EntitySearch" || answer.FieldType == "EntitySelect")
                    {
                        try
                        {
                            if (reply.value != "" || reply.value != null)
                            {
                                entity_items match = db.entity_items.SingleOrDefault(x => x.microting_uid == reply.value);

                                if (match != null)
                                {
                                    answer.ValueReadable = match.name;
                                    answer.Value = match.entity_item_uid;
                                }

                            }
                        }
                        catch { }
                    }

                    if (answer.FieldType == "SingleSelect")
                    {
                        string key = answer.Value;
                        string fullKey = t.Locate(question.key_value_pair_list, "<" + key + ">", "</" + key + ">");
                        answer.ValueReadable = t.Locate(fullKey, "<key>", "</key>");

                        answer.KeyValuePairList = PairRead(question.key_value_pair_list);
                    }

                    if (answer.FieldType == "MultiSelect")
                    {
                        answer.ValueReadable = "";

                        string keys = answer.Value;
                        List<string> keyLst = keys.Split('|').ToList();

                        foreach (string key in keyLst)
                        {
                            string fullKey = t.Locate(question.key_value_pair_list, "<" + key + ">", "</" + key + ">");
                            if (answer.ValueReadable != "")
                                answer.ValueReadable += '|';
                            answer.ValueReadable += t.Locate(fullKey, "<key>", "</key>");
                        }

                        answer.KeyValuePairList = PairRead(question.key_value_pair_list);
                    }
                    #endregion

                    return answer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueRead failed", ex);
            }
        }

        public FieldValue FieldValueRead(int id)
        {
            try
            {
                using (var db = GetContext())
                {
                    field_values reply = db.field_values.Single(x => x.id == id);
                    fields question = db.fields.Single(x => x.id == reply.field_id);
                    return FieldValueRead(question, reply, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueUpdate failed", ex);
            }
        }

        public List<FieldValue> FieldValueReadList(int id, int instances)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<field_values> matches = db.field_values.Where(x => x.field_id == id).OrderByDescending(z => z.created_at).ToList();

                    if (matches.Count() > instances)
                        matches = matches.GetRange(0, instances);

                    List<FieldValue> rtnLst = new List<FieldValue>();

                    foreach (var item in matches)
                        rtnLst.Add(FieldValueRead(item.id));

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueUpdate failed", ex);
            }
        }

        public void FieldValueUpdate(int caseId, int fieldId, string value)
        {
            try
            {
                using (var db = GetContext())
                {
                    field_values fieldMatch = db.field_values.Single(x => x.case_id == caseId && x.field_id == fieldId);

                    fieldMatch.value = value;
                    fieldMatch.updated_at = DateTime.Now;
                    fieldMatch.version = fieldMatch.version + 1;

                    db.field_value_versions.Add(MapFieldValueVersions(fieldMatch));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueUpdate failed", ex);
            }
        }

        public List<List<KeyValuePair>> FieldValueReadAllValues(int fieldId, List<int> caseIds, string customPathForUploadedData)
        {
            try
            {
                using (var db = GetContext())
                {
                    fields matchField = db.fields.Single(x => x.id == fieldId);

                    List<field_values> matches = db.field_values.Where(x => x.field_id == fieldId && caseIds.Contains((int)x.case_id)).ToList();

                    List<List<KeyValuePair>> rtrnLst = new List<List<KeyValuePair>>();
                    List<KeyValuePair> replyLst1 = new List<KeyValuePair>();
                    rtrnLst.Add(replyLst1);

                    switch (matchField.field_type.field_type)
                    {
                        #region special dataItem
                        case "CheckBox":
                            foreach (field_values item in matches)
                            {
                                if (item.value == "checked")
                                    replyLst1.Add(new KeyValuePair(item.case_id.ToString(), "1", false, ""));
                                else
                                    replyLst1.Add(new KeyValuePair(item.case_id.ToString(), "0", false, ""));
                            }
                            break;
                        case "Signature":
                        case "Picture":
                            int lastCaseId = -1;
                            int lastIndex = -1;
                            foreach (field_values item in matches)
                            {
                                if (item.value != null)
                                {
                                    if (lastCaseId == (int)item.case_id)
                                    {

                                        foreach (KeyValuePair kvp in replyLst1)
                                        {
                                            if (kvp.Key == item.case_id.ToString())
                                            {
                                                if (customPathForUploadedData != null)
                                                    if (kvp.Value.Contains("http"))
                                                        kvp.Value = kvp.Value + "|" + customPathForUploadedData + item.uploaded_data.file_name;
                                                    else
                                                        kvp.Value = customPathForUploadedData + item.uploaded_data.file_name;
                                                else
                                                    if (kvp.Value.Contains("http"))
                                                    kvp.Value = kvp.Value + "|" + item.uploaded_data.file_location + item.uploaded_data.file_name;
                                                else
                                                    kvp.Value = item.uploaded_data.file_location + item.uploaded_data.file_name;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lastIndex++;
                                        if (item.uploaded_data_id != null)
                                        {
                                            if (customPathForUploadedData != null)

                                                replyLst1.Add(new KeyValuePair(item.case_id.ToString(), customPathForUploadedData + item.uploaded_data.file_name, false, ""));
                                            else
                                                replyLst1.Add(new KeyValuePair(item.case_id.ToString(), item.uploaded_data.file_location + item.uploaded_data.file_name, false, ""));
                                        }
                                        else
                                        {
                                            replyLst1.Add(new KeyValuePair(item.case_id.ToString(), "", false, ""));
                                        }
                                    }
                                }
                                else
                                {
                                    lastIndex++;
                                    replyLst1.Add(new KeyValuePair(item.case_id.ToString(), "", false, ""));
                                }
                                lastCaseId = (int)item.case_id;
                            }
                            break;

                        case "SingleSelect":
                            {
                                var kVP = PairRead(matchField.key_value_pair_list);

                                foreach (field_values item in matches)
                                    replyLst1.Add(new KeyValuePair(item.case_id.ToString(), PairMatch(kVP, item.value), false, ""));
                            }
                            break;

                        case "MultiSelect":
                            {
                                var kVP = PairRead(matchField.key_value_pair_list);
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
                                        valueExt = "|" + item.value + "|";
                                        if (valueExt.Contains("|" + index.ToString() + "|"))
                                            replyLst.Add(new KeyValuePair(item.case_id.ToString(), "1", false, ""));
                                        else
                                            replyLst.Add(new KeyValuePair(item.case_id.ToString(), "0", false, ""));
                                    }

                                    rtrnLst.Add(replyLst);
                                }

                            }
                            break;

                        case "EntitySelect":
                        case "EntitySearch":
                            {
                                foreach (field_values item in matches)
                                {
                                    try
                                    {
                                        if (item.value != "" || item.value != null)
                                        {
                                            entity_items match = db.entity_items.SingleOrDefault(x => x.microting_uid == item.value);

                                            if (match != null)
                                            {
                                                replyLst1.Add(new KeyValuePair(item.case_id.ToString(), match.name, false, ""));
                                            }
                                            else
                                            {
                                                replyLst1.Add(new KeyValuePair(item.case_id.ToString(), "", false, ""));
                                            }

                                        }
                                    }
                                    catch
                                    {
                                        replyLst1.Add(new KeyValuePair(item.case_id.ToString(), "", false, ""));
                                    }
                                }
                            }
                            break;
                        #endregion

                        default:
                            foreach (field_values item in matches)
                                replyLst1.Add(new KeyValuePair(item.case_id.ToString(), item.value, false, ""));
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
                    check_list_values clv = db.check_list_values.Single(x => x.case_id == caseId && x.check_list_id == checkListId);
                    return clv.status;
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
                    check_list_values match = db.check_list_values.Single(x => x.case_id == caseId && x.check_list_id == checkListId);

                    match.status = value;
                    match.updated_at = DateTime.Now;
                    match.version = match.version + 1;

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
        public void NotificationCreate(string notificationUId, string microtingUId, string activity)
        {
            using (var db = GetContext())
            {
                if (db.notifications.Count(x => x.notification_uid == notificationUId && x.microting_uid == microtingUId) == 0)
                {
                    log.LogStandard("Not Specified", "SAVING notificationUId : " + notificationUId + " microtingUId : " + microtingUId + " action : " + activity);

                    notifications aNote = new notifications();

                    aNote.workflow_state = Constants.WorkflowStates.Created;
                    aNote.created_at = DateTime.Now;
                    aNote.updated_at = DateTime.Now;
                    aNote.notification_uid = notificationUId;
                    aNote.microting_uid = microtingUId;
                    aNote.activity = activity;

                    db.notifications.Add(aNote);
                    db.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException($"Duplicate notification found for notificationid = '{notificationUId}' and MicrotingUid = '{microtingUId}'");
                }

                //TODO else log warning
            }
        }

        public Note_Dto NotificationReadFirst()
        {
            try
            {
                using (var db = GetContext())
                {
                    notifications aNoti = db.notifications.FirstOrDefault(x => x.workflow_state == Constants.WorkflowStates.Created);

                    if (aNoti != null)
                    {
                        Note_Dto aNote = new Note_Dto(aNoti.notification_uid, aNoti.microting_uid, aNoti.activity);
                        return aNote;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public void NotificationUpdate(string notificationUId, string microtingUId, string workflowState)
        {
            try
            {
                using (var db = GetContext())
                {
                    notifications aNoti = db.notifications.Single(x => x.notification_uid == notificationUId && x.microting_uid == microtingUId);
                    aNoti.workflow_state = workflowState;
                    aNoti.updated_at = DateTime.Now;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }
        #endregion

        #region file
        public UploadedData FileRead()
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data dU = db.uploaded_data.FirstOrDefault(x => x.workflow_state == Constants.WorkflowStates.PreCreated);

                    if (dU != null)
                    {
                        UploadedData ud = new UploadedData();
                        ud.Checksum = dU.checksum;
                        ud.Extension = dU.extension;
                        ud.CurrentFile = dU.current_file;
                        ud.UploaderId = dU.uploader_id;
                        ud.UploaderType = dU.uploader_type;
                        ud.FileLocation = dU.file_location;
                        ud.FileName = dU.file_name;
                        ud.Id = dU.id;
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

        public Case_Dto FileCaseFindMUId(string urlString)
        {
            try
            {
                using (var db = GetContext())
                {
                    try
                    {
                        uploaded_data dU = db.uploaded_data.Where(x => x.file_location == urlString).First();
                        field_values fV = db.field_values.Single(x => x.uploaded_data_id == dU.id);
                        cases aCase = db.cases.Single(x => x.id == fV.case_id);

                        return CaseReadByCaseId(aCase.id);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FileRead failed", ex);
            }
        }

        public void FileProcessed(string urlString, string chechSum, string fileLocation, string fileName, int id)
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data uD = db.uploaded_data.Single(x => x.id == id);

                    uD.checksum = chechSum;
                    uD.file_location = fileLocation;
                    uD.file_name = fileName;
                    uD.local = 1;
                    uD.workflow_state = Constants.WorkflowStates.Created;
                    uD.updated_at = DateTime.Now;
                    uD.version = uD.version + 1;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FileProcessed failed", ex);
            }
        }

        public uploaded_data GetUploadedData(int id)
        {
            try
            {
                using (var db = GetContext())
                {
                    return db.uploaded_data.SingleOrDefault(x => x.id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get uploaded data object failed", ex);
            }
        }

        public bool DeleteFile(int id)
        {
            try
            {
                using (var db = GetContext())
                {
                    uploaded_data uD = db.uploaded_data.Single(x => x.id == id);

                    uD.workflow_state = "removed";
                    uD.updated_at = DateTime.Now;
                    uD.version = uD.version + 1;
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
        public Case_Dto CaseReadByMUId(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    try
                    {
                        cases aCase = db.cases.Single(x => x.microting_uid == microtingUId);
                        return CaseReadByCaseId(aCase.id);
                    }
                    catch { }

                    try
                    {
                        check_list_sites cls = db.check_list_sites.Single(x => x.microting_uid == microtingUId);
                        check_lists cL = db.check_lists.Single(x => x.id == cls.check_list_id);

                        #region string stat = aCase.workflow_state ...
                        string stat = "";
                        if (cls.workflow_state == Constants.WorkflowStates.Created)
                            stat = "Created";

                        if (cls.workflow_state == Constants.WorkflowStates.Removed)
                            stat = "Deleted";
                        #endregion

                        int remoteSiteId = (int)db.sites.Single(x => x.id == (int)cls.site_id).microting_uid;
                        Case_Dto rtrnCase = new Case_Dto(null, stat, remoteSiteId, cL.case_type, "ReversedCase", cls.microting_uid, cls.last_check_id, null, cL.id, null);
                        return rtrnCase;
                    }
                    catch { }

                    throw new Exception("CaseReadByMuuId failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByMuuId failed", ex);
            }
        }

        public Case_Dto CaseReadByCaseId(int caseId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases aCase = db.cases.Single(x => x.id == caseId);
                    check_lists cL = db.check_lists.Single(x => x.id == aCase.check_list_id);

                    #region string stat = aCase.workflow_state ...
                    string stat = "";
                    if (aCase.workflow_state == Constants.WorkflowStates.Created && aCase.status != 77)
                        stat = "Created";

                    if (aCase.workflow_state == Constants.WorkflowStates.Created && aCase.status == 77)
                        stat = "Retrived";

                    if (aCase.workflow_state == Constants.WorkflowStates.Retracted)
                        stat = "Completed";

                    if (aCase.workflow_state == Constants.WorkflowStates.Removed)
                        stat = "Deleted";
                    #endregion

                    int remoteSiteId = (int)db.sites.Single(x => x.id == (int)aCase.site_id).microting_uid;
                    Case_Dto cDto = new Case_Dto(aCase.id, stat, remoteSiteId, cL.case_type, aCase.case_uid, aCase.microting_uid, aCase.microting_check_uid, aCase.custom, cL.id, aCase.workflow_state);
                    return cDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByCaseId failed", ex);
            }
        }

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
                    List<cases> matches = db.cases.Where(x => x.case_uid == caseUId).ToList();
                    List<Case_Dto> lstDto = new List<Case_Dto>();

                    foreach (cases aCase in matches)
                        lstDto.Add(CaseReadByCaseId(aCase.id));

                    return lstDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByCaseUId failed", ex);
            }
        }

        public cases CaseReadFull(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases match = db.cases.SingleOrDefault(x => x.microting_uid == microtingUId && x.microting_check_uid == checkUId);
                    match.site_id = db.sites.SingleOrDefault(x => x.id == match.site_id).microting_uid;

                    if (match.unit_id != null)
                        match.unit_id = db.units.SingleOrDefault(x => x.id == match.unit_id).microting_uid;

                    if (match.done_by_user_id != null)
                        match.done_by_user_id = db.workers.SingleOrDefault(x => x.id == match.done_by_user_id).microting_uid;
                    return match;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        public int? CaseReadFirstId(int? templateId)
        {
            try
            {
                using (var db = GetContext())
                {
                    //cases dbCase = null;
                    return db.cases.Where(x => x.check_list_id == templateId && x.status == 100).First().id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        public List<Case> CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState, string searchKey, bool descendingSort, string sortParameter)
        {
            string methodName = t.GetMethodName();
            log.LogStandard("Not Specified", methodName + " called");
            log.LogVariable("Not Specified", nameof(templatId), templatId);
            log.LogVariable("Not Specified", nameof(start), start);
            log.LogVariable("Not Specified", nameof(end), end);
            log.LogVariable("Not Specified", nameof(workflowState), workflowState);
            log.LogVariable("Not Specified", nameof(searchKey), searchKey);
            log.LogVariable("Not Specified", nameof(descendingSort), descendingSort);
            log.LogVariable("Not Specified", nameof(sortParameter), sortParameter);
            try
            {
                using (var db = GetContext())
                {
                    if (start == null)
                        start = DateTime.MinValue;
                    if (end == null)
                        end = DateTime.MaxValue;


                    List<cases> matches = null;
                    IQueryable<cases> sub_query = db.cases.Where(x => x.done_at > start && x.done_at < end);
                    switch (workflowState)
                    {
                        case "not_retracted":
                            sub_query = sub_query.Where(x => x.workflow_state != Constants.WorkflowStates.Retracted);
                            break;
                        case "not_removed":
                            sub_query = sub_query.Where(x => x.workflow_state != Constants.WorkflowStates.Removed);
                            break;
                        case "created":
                            sub_query = sub_query.Where(x => x.workflow_state == Constants.WorkflowStates.Created);
                            break;
                        case "retracted":
                            sub_query = sub_query.Where(x => x.workflow_state == Constants.WorkflowStates.Retracted);
                            break;
                        case "removed":
                            sub_query = sub_query.Where(x => x.workflow_state == Constants.WorkflowStates.Removed);
                            break;
                        default:
                            break;
                    }


                    if (templatId != null)
                    {
                        sub_query = sub_query.Where(x => x.check_list_id == templatId);
                    }
                    if (searchKey != null && searchKey != "")
                    {
                        sub_query = sub_query.Where(x => x.field_value_1.Contains(searchKey) || x.field_value_2.Contains(searchKey) || x.field_value_3.Contains(searchKey) || x.field_value_4.Contains(searchKey) || x.field_value_5.Contains(searchKey) || x.field_value_6.Contains(searchKey) || x.field_value_7.Contains(searchKey) || x.field_value_8.Contains(searchKey) || x.field_value_9.Contains(searchKey) || x.field_value_10.Contains(searchKey));
                    }

                    switch (sortParameter)
                    {
                        case Constants.CaseSortParameters.CreatedAt:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.id);
                            else
                                sub_query = sub_query.OrderBy(x => x.id);
                            break;
                        case Constants.CaseSortParameters.DoneAt:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.done_at);
                            else
                                sub_query = sub_query.OrderBy(x => x.done_at);
                            break;
                        case Constants.CaseSortParameters.WorkerName:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.id);
                            else
                                sub_query = sub_query.OrderBy(x => x.id);
                            break;
                        case Constants.CaseSortParameters.SiteName:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.id);
                            else
                                sub_query = sub_query.OrderBy(x => x.id);
                            break;
                        case Constants.CaseSortParameters.UnitId:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.unit_id);
                            else
                                sub_query = sub_query.OrderBy(x => x.unit_id);
                            break;
                        case Constants.CaseSortParameters.Status:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.status);
                            else
                                sub_query = sub_query.OrderBy(x => x.status);
                            break;
                        case Constants.CaseSortParameters.FieldValue1:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_1);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_1);
                            break;
                        case Constants.CaseSortParameters.FieldValue2:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_2);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_2);
                            break;
                        case Constants.CaseSortParameters.FieldValue3:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_3);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_3);
                            break;
                        case Constants.CaseSortParameters.FieldValue4:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_4);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_4);
                            break;
                        case Constants.CaseSortParameters.FieldValue5:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_5);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_5);
                            break;
                        case Constants.CaseSortParameters.FieldValue6:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_6);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_6);
                            break;
                        case Constants.CaseSortParameters.FieldValue7:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_7);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_7);
                            break;
                        case Constants.CaseSortParameters.FieldValue8:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_8);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_8);
                            break;
                        case Constants.CaseSortParameters.FieldValue9:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_9);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_9);
                            break;
                        case Constants.CaseSortParameters.FieldValue10:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.field_value_10);
                            else
                                sub_query = sub_query.OrderBy(x => x.field_value_10);
                            break;
                        default:
                            if (descendingSort)
                                sub_query = sub_query.OrderByDescending(x => x.id);
                            else
                                sub_query = sub_query.OrderBy(x => x.id);
                            break;
                    }

                    matches = sub_query.ToList();

                    //

                    List<Case> rtrnLst = new List<Case>();
                    #region cases -> Case
                    foreach (var dbCase in matches)
                    {
                        Case nCase = new Case();
                        nCase.CaseType = dbCase.type;
                        nCase.CaseUId = dbCase.case_uid;
                        nCase.CheckUIid = dbCase.microting_check_uid;
                        nCase.CreatedAt = dbCase.created_at;
                        nCase.Custom = dbCase.custom;
                        nCase.DoneAt = dbCase.done_at;
                        nCase.Id = dbCase.id;
                        nCase.MicrotingUId = dbCase.microting_uid;
                        nCase.SiteId = dbCase.site.microting_uid;
                        nCase.SiteName = dbCase.site.name;
                        nCase.Status = dbCase.status;
                        nCase.TemplatId = dbCase.check_list_id;
                        nCase.UnitId = dbCase.unit.microting_uid;
                        nCase.UpdatedAt = dbCase.updated_at;
                        nCase.Version = dbCase.version;
                        nCase.WorkerName = dbCase.worker.first_name + " " + dbCase.worker.last_name;
                        nCase.WorkflowState = dbCase.workflow_state;
                        nCase.FieldValue1 = dbCase.field_value_1;
                        nCase.FieldValue2 = dbCase.field_value_2;
                        nCase.FieldValue3 = dbCase.field_value_3;
                        nCase.FieldValue4 = dbCase.field_value_4;
                        nCase.FieldValue5 = dbCase.field_value_5;
                        nCase.FieldValue6 = dbCase.field_value_6;
                        nCase.FieldValue7 = dbCase.field_value_7;
                        nCase.FieldValue8 = dbCase.field_value_8;
                        nCase.FieldValue9 = dbCase.field_value_9;
                        nCase.FieldValue10 = dbCase.field_value_10;

                        rtrnLst.Add(nCase);
                    }
                    #endregion

                    return rtrnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        public List<Case_Dto> CaseFindCustomMatchs(string customString)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<Case_Dto> foundCasesThatMatch = new List<Case_Dto>();

                    List<cases> lstMatchs = db.cases.Where(x => x.custom == customString && x.workflow_state == Constants.WorkflowStates.Created).ToList();

                    foreach (cases match in lstMatchs)
                        foundCasesThatMatch.Add(CaseReadByCaseId(match.id));

                    return foundCasesThatMatch;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseFindCustomMatchs failed", ex);
            }
        }

        public bool CaseUpdateFieldValues(int caseId)
        {
            try
            {
                using (var db = GetContext())
                {
                    cases lstMatchs = db.cases.SingleOrDefault(x => x.id == caseId);

                    if (lstMatchs == null)
                        return false;

                    lstMatchs.updated_at = DateTime.Now;
                    lstMatchs.version = lstMatchs.version + 1;
                    List<int?> case_fields = new List<int?>();
                    check_lists cl = lstMatchs.check_list;

                    case_fields.Add(cl.field_1);
                    case_fields.Add(cl.field_2);
                    case_fields.Add(cl.field_3);
                    case_fields.Add(cl.field_4);
                    case_fields.Add(cl.field_5);
                    case_fields.Add(cl.field_6);
                    case_fields.Add(cl.field_7);
                    case_fields.Add(cl.field_8);
                    case_fields.Add(cl.field_9);
                    case_fields.Add(cl.field_10);

                    lstMatchs.field_value_1 = null;
                    lstMatchs.field_value_2 = null;
                    lstMatchs.field_value_3 = null;
                    lstMatchs.field_value_4 = null;
                    lstMatchs.field_value_5 = null;
                    lstMatchs.field_value_6 = null;
                    lstMatchs.field_value_7 = null;
                    lstMatchs.field_value_8 = null;
                    lstMatchs.field_value_9 = null;
                    lstMatchs.field_value_10 = null;

                    List<field_values> field_values = db.field_values.Where(x => x.case_id == lstMatchs.id && case_fields.Contains(x.field_id)).ToList();

                    foreach (field_values item in field_values)
                    {
                        field_types field_type = item.field.field_type;
                        string new_value = item.value;

                        if (field_type.field_type == "EntitySearch" || field_type.field_type == "EntitySelect")
                        {
                            try
                            {
                                if (item.value != "" || item.value != null)
                                {
                                    entity_items match = db.entity_items.SingleOrDefault(x => x.microting_uid == item.value);

                                    if (match != null)
                                    {
                                        new_value = match.name;
                                    }

                                }
                            }
                            catch { }
                        }

                        if (field_type.field_type == "SingleSelect")
                        {
                            string key = item.value;
                            string fullKey = t.Locate(item.field.key_value_pair_list, "<" + key + ">", "</" + key + ">");
                            new_value = t.Locate(fullKey, "<key>", "</key>");
                        }

                        if (field_type.field_type == "MultiSelect")
                        {
                            new_value = "";

                            string keys = item.value;
                            List<string> keyLst = keys.Split('|').ToList();

                            foreach (string key in keyLst)
                            {
                                string fullKey = t.Locate(item.field.key_value_pair_list, "<" + key + ">", "</" + key + ">");
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


                        int i = case_fields.IndexOf(item.field_id);
                        switch (i)
                        {
                            case 0:
                                lstMatchs.field_value_1 = new_value;
                                break;
                            case 1:
                                lstMatchs.field_value_2 = new_value;
                                break;
                            case 2:
                                lstMatchs.field_value_3 = new_value;
                                break;
                            case 3:
                                lstMatchs.field_value_4 = new_value;
                                break;
                            case 4:
                                lstMatchs.field_value_5 = new_value;
                                break;
                            case 5:
                                lstMatchs.field_value_6 = new_value;
                                break;
                            case 6:
                                lstMatchs.field_value_7 = new_value;
                                break;
                            case 7:
                                lstMatchs.field_value_8 = new_value;
                                break;
                            case 8:
                                lstMatchs.field_value_9 = new_value;
                                break;
                            case 9:
                                lstMatchs.field_value_10 = new_value;
                                break;
                        }
                    }

                    db.cases.AddOrUpdate(lstMatchs);
                    db.SaveChanges();
                    db.case_versions.Add(MapCaseVersions(lstMatchs));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }
        #endregion
        
        #region public site
        #region site
        public List<SiteName_Dto> SiteGetAll(bool includeRemoved)
        {
            List<SiteName_Dto> siteList = new List<SiteName_Dto>();
            using (var db = GetContext())
            {
                List<sites> matches = null;
                if(includeRemoved)
                    matches = db.sites.ToList();
                else
                    matches = db.sites.Where(x => x.workflow_state == Constants.WorkflowStates.Created).ToList();

                foreach (sites aSite in matches)
                {
                    SiteName_Dto siteNameDto = new SiteName_Dto((int)aSite.microting_uid, aSite.name, aSite.created_at, aSite.updated_at);
                    siteList.Add(siteNameDto);
                }
            }
            return siteList;

        }

        public List<Site_Dto> SimpleSiteGetAll(string workflowState, int? offSet, int? limit)
        {
            List<Site_Dto> siteList = new List<Site_Dto>();
            using (var db = GetContext())
            {
                List<sites> matches = null;
                switch (workflowState)
                {
                    case "not_removed":
                        matches = db.sites.Where(x => x.workflow_state != Constants.WorkflowStates.Removed).ToList();
                        break;
                    case "removed":
                        matches = db.sites.Where(x => x.workflow_state == Constants.WorkflowStates.Removed).ToList();
                        break;
                    case "created":
                        matches = db.sites.Where(x => x.workflow_state == Constants.WorkflowStates.Created).ToList();
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
                        unit = aSite.units.First();
                        unitCustomerNo = (int)unit.customer_no;
                        unitOptCode = (int)unit.otp_code;
                        unitMicrotingUid = (int)unit.microting_uid;
                    }
                    catch { }

                    try
                    {
                        worker = aSite.site_workers.First().worker;
                        workerMicrotingUid = worker.microting_uid;
                        workerFirstName = worker.first_name;
                        workerLastName = worker.last_name;
                    }
                    catch { }

                    try
                    {
                        Site_Dto siteDto = new Site_Dto((int)aSite.microting_uid, aSite.name, workerFirstName, workerLastName, unitCustomerNo, unitOptCode, unitMicrotingUid, workerMicrotingUid);
                        siteList.Add(siteDto);
                    }
                    catch
                    {
                        Site_Dto siteDto = new Site_Dto((int)aSite.microting_uid, aSite.name, workerFirstName, workerLastName, unitCustomerNo, unitOptCode, unitMicrotingUid, workerMicrotingUid);
                        siteList.Add(siteDto);
                    }
                }
            }
            return siteList;

        }

        public int SiteCreate(int microtingUid, string name)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = new sites();
                    site.workflow_state = Constants.WorkflowStates.Created;
                    site.version = 1;
                    site.created_at = DateTime.Now;
                    site.updated_at = DateTime.Now;
                    site.microting_uid = microtingUid;
                    site.name = name;


                    db.sites.Add(site);
                    db.SaveChanges();

                    db.site_versions.Add(MapSiteVersions(site));
                    db.SaveChanges();

                    return site.id;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public SiteName_Dto SiteRead(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid && x.workflow_state == Constants.WorkflowStates.Created);

                    if (site != null)
                        return new SiteName_Dto((int)site.microting_uid, site.name, site.created_at, site.updated_at);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public Site_Dto SiteReadSimple(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid && x.workflow_state == Constants.WorkflowStates.Created);
                    if (site == null)
                        return null;

                    site_workers site_worker = db.site_workers.Where(x => x.site_id == site.id).ToList().First();
                    workers worker = db.workers.Single(x => x.id == site_worker.worker_id);
                    units unit = db.units.Where(x => x.site_id == site.id).ToList().First();

                    if (unit != null && worker != null)
                        return new Site_Dto((int)site.microting_uid, site.name, worker.first_name, worker.last_name, (int)unit.customer_no, (int)unit.otp_code, (int)unit.microting_uid, worker.microting_uid);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool SiteUpdate(int microting_uid, string name)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid);

                    if (site != null)
                    {
                        site.version = site.version + 1;
                        site.updated_at = DateTime.Now;

                        site.name = name;

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

        public bool SiteDelete(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid);

                    if (site != null)
                    {
                        site.version = site.version + 1;
                        site.updated_at = DateTime.Now;

                        site.workflow_state = "removed";

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
        public List<Worker_Dto> WorkerGetAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName();
            try
            {
                List<Worker_Dto> listWorkerDto = new List<Worker_Dto>();

                using (var db = GetContext())
                {
                    List<workers> matches = null;

                    switch (workflowState)
                    {
                        case "not_removed":
                            matches = db.workers.Where(x => x.workflow_state != Constants.WorkflowStates.Removed).ToList();
                            break;
                        case "removed":
                            matches = db.workers.Where(x => x.workflow_state == Constants.WorkflowStates.Removed).ToList();
                            break;
                        case "created":
                            matches = db.workers.Where(x => x.workflow_state == Constants.WorkflowStates.Created).ToList();
                            break;
                        default:
                            matches = db.workers.ToList();
                            break;
                    }
                    foreach (workers worker in matches)
                    {
                        Worker_Dto workerDto = new Worker_Dto(worker.microting_uid, worker.first_name, worker.last_name, worker.email, worker.created_at, worker.updated_at);
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

        public int WorkerCreate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    workers worker = new workers();
                    worker.workflow_state = Constants.WorkflowStates.Created;
                    worker.version = 1;
                    worker.created_at = DateTime.Now;
                    worker.updated_at = DateTime.Now;
                    worker.microting_uid = microtingUid;
                    worker.first_name = firstName;
                    worker.last_name = lastName;
                    worker.email = email;


                    db.workers.Add(worker);
                    db.SaveChanges();

                    db.worker_versions.Add(MapWorkerVersions(worker));
                    db.SaveChanges();

                    return worker.id;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public string WorkerNameRead(int workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    workers worker = db.workers.SingleOrDefault(x => x.id == workerId && x.workflow_state == "created");

                    if (worker == null)
                        return null;
                    else
                        return worker.first_name + " " + worker.last_name;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public Worker_Dto WorkerRead(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    workers worker = db.workers.SingleOrDefault(x => x.microting_uid == microting_uid && x.workflow_state == Constants.WorkflowStates.Created);

                    if (worker != null)
                        return new Worker_Dto((int)worker.microting_uid, worker.first_name, worker.last_name, worker.email, worker.created_at, worker.updated_at);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool WorkerUpdate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    workers worker = db.workers.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (worker != null)
                    {
                        worker.version = worker.version + 1;
                        worker.updated_at = DateTime.Now;

                        worker.first_name = firstName;
                        worker.last_name = lastName;
                        worker.email = email;

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

        public bool WorkerDelete(int microtingUid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    workers worker = db.workers.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (worker != null)
                    {
                        worker.version = worker.version + 1;
                        worker.updated_at = DateTime.Now;

                        worker.workflow_state = "removed";

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
        public int SiteWorkerCreate(int microtingUId, int siteUId, int workerUId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    int localSiteId = db.sites.Single(x => x.microting_uid == siteUId).id;
                    int localWorkerId = db.workers.Single(x => x.microting_uid == workerUId).id;

                    site_workers site_worker = new site_workers();
                    site_worker.workflow_state = Constants.WorkflowStates.Created;
                    site_worker.version = 1;
                    site_worker.created_at = DateTime.Now;
                    site_worker.updated_at = DateTime.Now;
                    site_worker.microting_uid = microtingUId;
                    site_worker.site_id = localSiteId;
                    site_worker.worker_id = localWorkerId;


                    db.site_workers.Add(site_worker);
                    db.SaveChanges();

                    db.site_worker_versions.Add(MapSiteWorkerVersions(site_worker));
                    db.SaveChanges();

                    return site_worker.id;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public Site_Worker_Dto SiteWorkerRead(int? microtingUid, int? siteId, int? workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    site_workers site_worker = null;
                    if (microtingUid == null)
                    {
                        sites site = db.sites.Single(x => x.microting_uid == siteId);
                        workers worker = db.workers.Single(x => x.microting_uid == workerId);
                        site_worker = db.site_workers.SingleOrDefault(x => x.site_id == site.id && x.worker_id == worker.id);
                    }
                    else
                    {
                        site_worker = db.site_workers.SingleOrDefault(x => x.microting_uid == microtingUid && x.workflow_state == Constants.WorkflowStates.Created);
                    }


                    if (site_worker != null)
                        return new Site_Worker_Dto((int)site_worker.microting_uid, (int)site_worker.site_id, (int)site_worker.worker_id);
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

        public bool SiteWorkerUpdate(int microtingUid, int siteId, int workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    site_workers site_worker = db.site_workers.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (site_worker != null)
                    {
                        site_worker.version = site_worker.version + 1;
                        site_worker.updated_at = DateTime.Now;

                        site_worker.site_id = siteId;
                        site_worker.worker_id = workerId;

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

        public bool SiteWorkerDelete(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    site_workers site_worker = db.site_workers.SingleOrDefault(x => x.microting_uid == microting_uid);

                    if (site_worker != null)
                    {
                        site_worker.version = site_worker.version + 1;
                        site_worker.updated_at = DateTime.Now;

                        site_worker.workflow_state = "removed";

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
        public List<Unit_Dto> UnitGetAll()
        {
            string methodName = t.GetMethodName();
            try
            {
                List<Unit_Dto> listWorkerDto = new List<Unit_Dto>();
                using (var db = GetContext())
                {
                    foreach (units unit in db.units.ToList())
                    {
                        Unit_Dto unitDto = new Unit_Dto((int)unit.microting_uid, (int)unit.customer_no, (int)unit.otp_code, (int)unit.site.microting_uid, unit.created_at, unit.updated_at);
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

        public int UnitCreate(int microtingUid, int customerNo, int otpCode, int siteUId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");
                    int localSiteId = db.sites.Single(x => x.microting_uid == siteUId).id;

                    units unit = new units();
                    unit.workflow_state = Constants.WorkflowStates.Created;
                    unit.version = 1;
                    unit.created_at = DateTime.Now;
                    unit.updated_at = DateTime.Now;
                    unit.microting_uid = microtingUid;
                    unit.customer_no = customerNo;
                    unit.otp_code = otpCode;
                    unit.site_id = localSiteId;


                    db.units.Add(unit);
                    db.SaveChanges();

                    db.unit_versions.Add(MapUnitVersions(unit));
                    db.SaveChanges();

                    return unit.id;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public Unit_Dto UnitRead(int microtingUid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.microting_uid == microtingUid && x.workflow_state == Constants.WorkflowStates.Created);

                    if (unit != null)
                        return new Unit_Dto((int)unit.microting_uid, (int)unit.customer_no, (int)unit.otp_code, (int)unit.site_id, unit.created_at, unit.updated_at);
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

        public bool UnitUpdate(int microtingUid, int customerNo, int otpCode, int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (unit != null)
                    {
                        unit.version = unit.version + 1;
                        unit.updated_at = DateTime.Now;

                        unit.customer_no = customerNo;
                        unit.otp_code = otpCode;

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

        public bool UnitDelete(int microtingUid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = GetContext())
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (unit != null)
                    {
                        unit.version = unit.version + 1;
                        unit.updated_at = DateTime.Now;

                        unit.workflow_state = "removed";

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
        public EntityGroupList EntityGroupAll(string sort, string nameFilter, int pageIndex, int pageSize, string entityType, bool desc, string workflowState)
        {

            if (entityType != "EntitySearch" && entityType != "EntitySelect")
                throw new Exception("EntityGroupAll failed. EntityType:" + entityType + " is not an known type");
            if (workflowState != "not_removed" && workflowState != Constants.WorkflowStates.Created && workflowState != Constants.WorkflowStates.Removed)
                throw new Exception("EntityGroupAll failed. workflowState:" + workflowState + " is not an known workflow state");

            List<entity_groups> eG = null;
            List<EntityGroup> e_G = new List<EntityGroup>();
            int numOfElements = 0;
            try
            {
                using (var db = GetContext())
                {
                    var source = db.entity_groups.Where(x => x.type == entityType);
                    if (nameFilter != "")
                        source = source.Where(x => x.name.Contains(nameFilter));
                    if (sort == "id")
                        if (desc)
                            source = source.OrderByDescending(x => x.id);
                        else
                            source = source.OrderBy(x => x.id);
                    else
                        if (desc)
                        source = source.OrderByDescending(x => x.name);
                    else
                        source = source.OrderBy(x => x.id);

                    switch (workflowState)
                    {
                        case "not_removed":
                            source = source.Where(x => x.workflow_state != Constants.WorkflowStates.Removed);
                            break;
                        case "removed":
                            source = source.Where(x => x.workflow_state == Constants.WorkflowStates.Removed);
                            break;
                        case "created":
                            source = source.Where(x => x.workflow_state == Constants.WorkflowStates.Created);
                            break;
                    }

                    numOfElements = source.Count();
                    eG = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    foreach (entity_groups eg in eG)
                    {
                        EntityGroup g = new EntityGroup(eg.id, eg.name, eg.type, eg.microting_uid, new List<EntityItem>(), eg.workflow_state, eg.created_at, eg.updated_at);
                        e_G.Add(g);
                    }
                    return new EntityGroupList(numOfElements, pageIndex, e_G);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupAll failed", ex);
            }
        }

        public EntityGroup EntityGroupCreate(string name, string entityType)
        {
            try
            {
                if (entityType != "EntitySearch" && entityType != "EntitySelect")
                    throw new Exception("EntityGroupCreate failed. EntityType:" + entityType + " is not an known type");

                using (var db = GetContext())
                {
                    entity_groups eG = new entity_groups();

                    eG.created_at = DateTime.Now;
                    //eG.id = xxx;
                    //eG.microtingUId = xxx;
                    eG.name = name;
                    eG.type = entityType;
                    eG.updated_at = DateTime.Now;
                    eG.version = 1;
                    eG.workflow_state = Constants.WorkflowStates.Created;

                    db.entity_groups.Add(eG);
                    db.SaveChanges();

                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));
                    db.SaveChanges();

                    return new EntityGroup(eG.id, eG.name, eG.type, eG.microting_uid, new List<EntityItem>(), eG.workflow_state, eG.created_at, eG.updated_at);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupCreate failed", ex);
            }
        }

        public EntityGroup EntityGroupReadSorted(string entityGroupMUId, string sort, string nameFilter)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.microting_uid == entityGroupMUId);

                    if (eG == null)
                        return null;

                    List<EntityItem> lst = new List<EntityItem>();
                    EntityGroup rtnEG = new EntityGroup(eG.id, eG.name, eG.type, eG.microting_uid, lst, eG.workflow_state, eG.created_at, eG.updated_at);

                    List<entity_items> eILst = null;

                    if (nameFilter == "")
                    {
                        if (sort == "id")
                        {
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != Constants.WorkflowStates.Removed && x.workflow_state != Constants.WorkflowStates.FailedToSync).OrderBy(x => x.id).ToList();
                        }
                        else
                        {
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != Constants.WorkflowStates.Removed && x.workflow_state != Constants.WorkflowStates.FailedToSync).OrderBy(x => x.name).ToList();
                        }
                    }
                    else
                    {
                        if (sort == "id")
                        {
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != Constants.WorkflowStates.Removed && x.workflow_state != Constants.WorkflowStates.FailedToSync && x.name.Contains(nameFilter)).OrderBy(x => x.id).ToList();
                        }
                        else
                        {
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != Constants.WorkflowStates.Removed && x.workflow_state != Constants.WorkflowStates.FailedToSync && x.name.Contains(nameFilter)).OrderBy(x => x.name).ToList();
                        }
                    }

                    if (eILst.Count > 0)
                        foreach (entity_items item in eILst)
                        {
                            EntityItem eI = new EntityItem(item.name, item.description, item.entity_item_uid, item.workflow_state);
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

        public EntityGroup EntityGroupRead(string entityGroupMUId)
        {
            return EntityGroupReadSorted(entityGroupMUId, "id", "");
        }

        public bool EntityGroupUpdate(int entityGroupId, string entityGroupMUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.id == entityGroupId);

                    if (eG == null)
                        return false;

                    eG.microting_uid = entityGroupMUId;
                    eG.updated_at = DateTime.Now;
                    eG.version = eG.version + 1;

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

        public bool EntityGroupUpdateName(string name, string entityGroupMUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.microting_uid == entityGroupMUId);

                    if (eG == null)
                        return false;

                    eG.name = name;
                    eG.updated_at = DateTime.Now;
                    eG.version = eG.version + 1;

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

        public void EntityGroupUpdateItems(EntityGroup entityGroup)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<EntityItemUpdateInfo> rtnLst = new List<EntityItemUpdateInfo>();
                    EntityGroup eGNew = entityGroup;
                    EntityGroup eGFDb = EntityGroupRead(eGNew.EntityGroupMUId);

                    //same, new or update
                    foreach (EntityItem itemNew in eGNew.EntityGroupItemLst)
                    {
                        EntityItemCreateUpdate(entityGroup.EntityGroupMUId, itemNew);
                    }

                    //delete
                    bool stillInUse;
                    foreach (EntityItem itemFDb in eGFDb.EntityGroupItemLst)
                    {
                        stillInUse = false;
                        foreach (EntityItem itemNew in eGNew.EntityGroupItemLst)
                        {
                            if (itemNew.EntityItemUId == itemFDb.EntityItemUId)
                            {
                                stillInUse = true;
                                break;
                            }
                        }

                        if (!stillInUse)
                        {
                            EntityItemDelete(entityGroup.EntityGroupMUId, itemFDb.EntityItemUId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupUpdateItems failed", ex);
            }
        }

        public string EntityGroupDelete(string entityGroupMUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    List<string> killLst = new List<string>();

                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.microting_uid == entityGroupMUId && x.workflow_state != "removed");

                    if (eG == null)
                        return null;

                    killLst.Add(eG.microting_uid);

                    eG.workflow_state = "removed";
                    eG.updated_at = DateTime.Now;
                    eG.version = eG.version + 1;
                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));

                    List<entity_items> lst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != "removed").ToList();
                    if (lst != null)
                    {
                        foreach (entity_items item in lst)
                        {
                            item.workflow_state = "removed";
                            item.updated_at = DateTime.Now;
                            item.version = item.version + 1;
                            item.synced = t.Bool(false);
                            db.entity_item_versions.Add(MapEntityItemVersions(item));

                            killLst.Add(item.microting_uid);
                        }
                    }

                    db.SaveChanges();

                    return eG.type;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupDelete failed", ex);
            }
        }
        #endregion

        #region entityItem
        public entity_items EntityItemRead(string microtingUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    return db.entity_items.Single(x => x.microting_uid == microtingUId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemRead failed", ex);
            }
        }

        public entity_items EntityItemSyncedRead()
        {
            try
            {
                using (var db = GetContext())
                {
                    return db.entity_items.FirstOrDefault(x => x.synced == 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemSyncedRead failed", ex);
            }
        }

        public void EntityItemSyncedProcessed(string entityGroupMUId, string entityItemId, string microting_uid, string workflowState)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_items eItem = db.entity_items.SingleOrDefault(x => x.entity_item_uid == entityItemId && x.entity_group_id == entityGroupMUId);

                    if (eItem != null)
                    {
                        eItem.workflow_state = workflowState;
                        eItem.updated_at = DateTime.Now;
                        eItem.version = eItem.version + 1;
                        eItem.synced = 1;

                        if (workflowState == Constants.WorkflowStates.Created)
                            eItem.microting_uid = microting_uid; //<<---

                        db.entity_item_versions.Add(MapEntityItemVersions(eItem));
                        db.SaveChanges();
                    }
                    else
                    {
                        //TODO log warning
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemSyncedProcessed failed", ex);
            }
        }
        #endregion
        #endregion

        #region public setting
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

            return true;
        }

        public bool SettingCreate(Settings name)
        {
            using (var db = GetContext())
            {
                //key point
                #region id = settings.name
                int id = -1;
                string defaultValue = "default";
                switch (name)
                {
                    case Settings.firstRunDone: id = 1; defaultValue = "false"; break;
                    case Settings.logLevel: id = 2; defaultValue = "4"; break;
                    case Settings.logLimit: id = 3; defaultValue = "250"; break;
                    case Settings.knownSitesDone: id = 4; defaultValue = "false"; break;
                    case Settings.fileLocationPicture: id = 5; defaultValue = "dataFolder/picture/"; break;
                    case Settings.fileLocationPdf: id = 6; defaultValue = "dataFolder/pdf/"; break;
                    case Settings.fileLocationJasper: id = 7; defaultValue = "dataFolder/reports/"; break;
                    case Settings.token: id = 8; defaultValue = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"; break;
                    case Settings.comAddressBasic: id = 9; defaultValue = "https://basic.microting.com"; break;
                    case Settings.comAddressApi: id = 10; defaultValue = "https://xxxxxx.xxxxxx.com"; break;
                    case Settings.comAddressPdfUpload: id = 11; defaultValue = "https://xxxxxx.xxxxxx.com"; break;
                    case Settings.comOrganizationId: id = 12; defaultValue = "0"; break;
                    case Settings.awsAccessKeyId: id = 13; defaultValue = "XXX"; break;
                    case Settings.awsSecretAccessKey: id = 14; defaultValue = "XXX"; break;
                    case Settings.awsEndPoint: id = 15; defaultValue = "XXX"; break;
                    case Settings.unitLicenseNumber: id = 16; defaultValue = "0"; break;
                    case Settings.httpServerAddress: id = 17; defaultValue = "http://localhost:3000"; break;

                    default:
                        throw new IndexOutOfRangeException(name.ToString() + " is not a known/mapped Settings type");
                }
                #endregion

                settings matchId = db.settings.SingleOrDefault(x => x.id == id);
                settings matchName = db.settings.SingleOrDefault(x => x.name == name.ToString());

                if (matchName == null)
                {
                    if (matchId != null)
                    {
                        #region there is already a setting with that id but different name
                        //the old setting data is copied, and new is added
                        settings newSettingBasedOnOld = new settings();
                        newSettingBasedOnOld.id = (db.settings.Select(x => (int?)x.id).Max() ?? 0) + 1;
                        newSettingBasedOnOld.name = matchId.name.ToString();
                        newSettingBasedOnOld.value = matchId.value;

                        db.settings.Add(newSettingBasedOnOld);

                        matchId.name = name.ToString();
                        matchId.value = defaultValue;

                        db.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        //its a new setting
                        settings newSetting = new settings();
                        newSetting.id = id;
                        newSetting.name = name.ToString();
                        newSetting.value = defaultValue;

                        db.settings.Add(newSetting);
                    }
                    db.SaveChanges();
                }
                else
                    if (string.IsNullOrEmpty(matchName.value))
                    matchName.value = defaultValue;
            }

            return true;
        }

        public string SettingRead(Settings name)
        {
            try
            {
                using (var db = GetContext())
                {
                    settings match = db.settings.Single(x => x.name == name.ToString());

                    if (match.value == null)
                        return "";

                    return match.value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public void SettingUpdate(Settings name, string newValue)
        {
            try
            {
                using (var db = GetContext())
                {
                    settings match = db.settings.SingleOrDefault(x => x.name == name.ToString());

                    if (match == null)
                    {
                        SettingCreate(name);
                        match = db.settings.Single(x => x.name == name.ToString());
                    }

                    match.value = newValue;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public List<string> SettingCheckAll()
        {
            List<string> result = new List<string>();
            try
            {
                using (var db = GetContext())
                {
                    if (db.field_types.Count() != 18)
                    {
                        #region prime FieldTypes
                        //UnitTest_TruncateTable(typeof(field_types).Name);

                        FieldTypeAdd(1, "Text", "Simple text field");
                        FieldTypeAdd(2, "Number", "Simple number field");
                        FieldTypeAdd(3, "None", "Simple text to be displayed");
                        FieldTypeAdd(4, "CheckBox", "Simple check box field");
                        FieldTypeAdd(5, "Picture", "Simple picture field");
                        FieldTypeAdd(6, "Audio", "Simple audio field");
                        FieldTypeAdd(7, "Movie", "Simple movie field");
                        FieldTypeAdd(8, "SingleSelect", "Single selection list");
                        FieldTypeAdd(9, "Comment", "Simple comment field");
                        FieldTypeAdd(10, "MultiSelect", "Simple multi select list");
                        FieldTypeAdd(11, "Date", "Date selection");
                        FieldTypeAdd(12, "Signature", "Simple signature field");
                        FieldTypeAdd(13, "Timer", "Simple timer field");
                        FieldTypeAdd(14, "EntitySearch", "Autofilled searchable items field");
                        FieldTypeAdd(15, "EntitySelect", "Autofilled single selection list");
                        FieldTypeAdd(16, "ShowPdf", "Show PDF");
                        FieldTypeAdd(17, "FieldGroup", "Field group");
                        FieldTypeAdd(18, "SaveButton", "Save eForm");
                        #endregion
                    }

                    int countVal = db.settings.Count(x => x.value == "");
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
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }
        #endregion

        #region public write log
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
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public override string WriteLogEntry(LogEntry logEntry)
        {
            lock (_lockWrite)
            {
                try
                {
                    using (var db = GetContext())
                    {
                        logs newLog = new logs();
                        newLog.created_at = logEntry.Time;
                        newLog.level = logEntry.Level;
                        newLog.message = logEntry.Message;
                        newLog.type = logEntry.Type;

                        db.logs.Add(newLog);
                        db.SaveChanges();

                        if (logEntry.Level < 0)
                            WriteLogExceptionEntry(logEntry);

                        #region clean up of log table
                        int limit = t.Int(SettingRead(Settings.logLimit));
                        if (limit > 0)
                        {
                            List<logs> killList = db.logs.Where(x => x.id <= newLog.id - limit).ToList();

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
                    return t.PrintException(t.GetMethodName() + " failed", ex);
                }
            }
        }

        private string WriteLogExceptionEntry(LogEntry logEntry)
        {
            try
            {
                using (var db = GetContext())
                {
                    log_exceptions newLog = new log_exceptions();
                    newLog.created_at = logEntry.Time;
                    newLog.level = logEntry.Level;
                    newLog.message = logEntry.Message;
                    newLog.type = logEntry.Type;

                    db.log_exceptions.Add(newLog);
                    db.SaveChanges();

                    #region clean up of log exception table
                    int limit = t.Int(SettingRead(Settings.logLimit));
                    if (limit > 0)
                    {
                        List<log_exceptions> killList = db.log_exceptions.Where(x => x.id <= newLog.id - limit).ToList();

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
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }

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
        private int EformCreateDb(MainElement mainElement)
        {
            try
            {
                using (var db = GetContext())
                {
                    GetConverter();

                    #region mainElement
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.label = mainElement.Label;
                    //description - used for non-MainElements
                    //serialized_default_values - Ruby colume
                    cl.workflow_state = Constants.WorkflowStates.Created;
                    cl.parent_id = null; //MainElements never have parents ;)
                    cl.repeated = mainElement.Repeated;
                    cl.version = 1;
                    cl.case_type = mainElement.CaseType;
                    cl.folder_name = mainElement.CheckListFolderName;
                    cl.display_index = mainElement.DisplayOrder;
                    //report_file_name - Ruby colume
                    cl.review_enabled = 0; //used for non-MainElements
                    cl.manual_sync = t.Bool(mainElement.ManualSync);
                    cl.extra_fields_enabled = 0; //used for non-MainElements
                    cl.done_button_enabled = 0; //used for non-MainElements
                    cl.approval_enabled = 0; //used for non-MainElements
                    cl.multi_approval = t.Bool(mainElement.MultiApproval);
                    cl.fast_navigation = t.Bool(mainElement.FastNavigation);
                    cl.download_entities = t.Bool(mainElement.DownloadEntities);

                    db.check_lists.Add(cl);
                    db.SaveChanges();

                    db.check_list_versions.Add(MapCheckListVersions(cl));
                    db.SaveChanges();

                    int mainId = cl.id;
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

        private void CreateGroupElement(int parentId, GroupElement groupElement)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.label = groupElement.Label;
                    if (groupElement.Description != null)
                        cl.description = groupElement.Description.InderValue;
                    else
                        cl.description = "";
                    //serialized_default_values - Ruby colume
                    cl.workflow_state = Constants.WorkflowStates.Created;
                    cl.parent_id = parentId;
                    //repeated - used for mainElements
                    cl.version = 1;
                    //case_type - used for mainElements
                    //folder_name - used for mainElements
                    cl.display_index = groupElement.DisplayOrder;
                    //report_file_name - Ruby colume
                    cl.review_enabled = t.Bool(groupElement.ReviewEnabled);
                    //manualSync - used for mainElements
                    cl.extra_fields_enabled = t.Bool(groupElement.ExtraFieldsEnabled);
                    cl.done_button_enabled = t.Bool(groupElement.DoneButtonEnabled);
                    cl.approval_enabled = t.Bool(groupElement.ApprovalEnabled);
                    //MultiApproval - used for mainElements
                    //FastNavigation - used for mainElements
                    //DownloadEntities - used for mainElements

                    db.check_lists.Add(cl);
                    db.SaveChanges();

                    db.check_list_versions.Add(MapCheckListVersions(cl));
                    db.SaveChanges();

                    CreateElementList(cl.id, groupElement.ElementList);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateGroupElement failed", ex);
            }
        }

        private void CreateDataElement(int parentId, DataElement dataElement)
        {
            try
            {
                using (var db = GetContext())
                {
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.label = dataElement.Label;
                    if (dataElement.Description != null)
                        cl.description = dataElement.Description.InderValue;
                    else
                        cl.description = "";

                    //serialized_default_values - Ruby colume
                    cl.workflow_state = Constants.WorkflowStates.Created;
                    cl.parent_id = parentId;
                    //repeated - used for mainElements
                    cl.version = 1;
                    //case_type - used for mainElements
                    //folder_name - used for mainElements
                    cl.display_index = dataElement.DisplayOrder;
                    //report_file_name - Ruby colume
                    cl.review_enabled = t.Bool(dataElement.ReviewEnabled);
                    //manualSync - used for mainElements
                    cl.extra_fields_enabled = t.Bool(dataElement.ExtraFieldsEnabled);
                    cl.done_button_enabled = t.Bool(dataElement.DoneButtonEnabled);
                    cl.approval_enabled = t.Bool(dataElement.ApprovalEnabled);
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
                    //        //CreateDataItemGroup(cl.id, fg);
                    //        CreateDataItemGroup(cl.id, (FieldGroup)dataItemGroup);
                    //    }
                    //}

                    if (dataElement.DataItemList != null)
                    {
                        foreach (DataItem dataItem in dataElement.DataItemList)
                        {
                            CreateDataItem(null, cl.id, dataItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataElement failed", ex);
            }
        }

        private void CreateDataItemGroup(int elementId, FieldContainer fieldGroup)
        {
            try
            {
                using (var db = GetContext())
                {
                    string typeStr = fieldGroup.GetType().ToString().Remove(0, 10); //10 = "eFormData.".Length
                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();
                    field.parent_field_id = null;
                    field.color = fieldGroup.Color;
                    //CDataValue description = new CDataValue();
                    //description.InderValue = fieldGroup.Description;
                    field.description = fieldGroup.Description.InderValue;
                    field.display_index = fieldGroup.DisplayOrder;
                    field.label = fieldGroup.Label;

                    field.created_at = DateTime.Now;
                    field.updated_at = DateTime.Now;
                    field.workflow_state = Constants.WorkflowStates.Created;
                    field.check_list_id = elementId;
                    field.field_type_id = fieldTypeId;
                    field.version = 1;

                    field.default_value = fieldGroup.Value;

                    db.fields.Add(field);
                    db.SaveChanges();

                    db.field_versions.Add(MapFieldVersions(field));
                    db.SaveChanges();

                    if (fieldGroup.DataItemList != null)
                    {
                        foreach (DataItem dataItem in fieldGroup.DataItemList)
                        {
                            CreateDataItem(field.id, elementId, dataItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataItemGroup failed", ex);
            }
        }

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
                    field.color = dataItem.Color;
                    field.parent_field_id = parentFieldId;
                    if (dataItem.Description != null)
                        field.description = dataItem.Description.InderValue;
                    else
                        field.description = "";
                    field.display_index = dataItem.DisplayOrder;
                    field.label = dataItem.Label;
                    field.mandatory = t.Bool(dataItem.Mandatory);
                    field.read_only = t.Bool(dataItem.ReadOnly);
                    field.dummy = t.Bool(dataItem.Dummy);

                    field.created_at = DateTime.Now;
                    field.updated_at = DateTime.Now;
                    field.workflow_state = Constants.WorkflowStates.Created;
                    field.check_list_id = elementId;
                    field.field_type_id = fieldTypeId;
                    field.version = 1;

                    bool isSaved = false; // This is done, because we need to have the current field id, for giving it onto the child fields in a FieldGroup

                    #region dataItem type
                    //KEY POINT - mapping
                    switch (typeStr)
                    {
                        case "Audio":
                            Audio audio = (Audio)dataItem;
                            field.multi = audio.Multi;
                            break;

                        case "CheckBox":
                            CheckBox checkBox = (CheckBox)dataItem;
                            field.default_value = checkBox.DefaultValue.ToString();
                            field.selected = t.Bool(checkBox.Selected);
                            break;

                        case "Comment":
                            Comment comment = (Comment)dataItem;
                            field.default_value = comment.Value;
                            field.max_length = comment.Maxlength;
                            field.split_screen = t.Bool(comment.SplitScreen);
                            break;

                        case "Date":
                            Date date = (Date)dataItem;
                            field.default_value = date.DefaultValue;
                            field.min_value = date.MinValue.ToString("yyyy-MM-dd");
                            field.max_value = date.MaxValue.ToString("yyyy-MM-dd");
                            break;

                        case "None":
                            break;

                        case "Number":
                            Number number = (Number)dataItem;
                            field.min_value = number.MinValue.ToString();
                            field.max_value = number.MaxValue.ToString();
                            field.default_value = number.DefaultValue.ToString();
                            field.decimal_count = number.DecimalCount;
                            field.unit_name = number.UnitName;
                            break;

                        case "MultiSelect":
                            MultiSelect multiSelect = (MultiSelect)dataItem;
                            field.key_value_pair_list = PairBuild(multiSelect.KeyValuePairList);
                            break;

                        case "Picture":
                            Picture picture = (Picture)dataItem;
                            field.multi = picture.Multi;
                            field.geolocation_enabled = t.Bool(picture.GeolocationEnabled);
                            break;

                        case "SaveButton":
                            SaveButton saveButton = (SaveButton)dataItem;
                            field.default_value = saveButton.Value;
                            break;

                        case "ShowPdf":
                            ShowPdf showPdf = (ShowPdf)dataItem;
                            field.default_value = showPdf.Value;
                            break;

                        case "Signature":
                            break;

                        case "SingleSelect":
                            SingleSelect singleSelect = (SingleSelect)dataItem;
                            field.key_value_pair_list = PairBuild(singleSelect.KeyValuePairList);
                            break;

                        case "Text":
                            Text text = (Text)dataItem;
                            field.default_value = text.Value;
                            field.max_length = text.MaxLength;
                            field.geolocation_enabled = t.Bool(text.GeolocationEnabled);
                            field.geolocation_forced = t.Bool(text.GeolocationForced);
                            field.geolocation_hidden = t.Bool(text.GeolocationHidden);
                            field.barcode_enabled = t.Bool(text.BarcodeEnabled);
                            field.barcode_type = text.BarcodeType;
                            break;

                        case "Timer":
                            Timer timer = (Timer)dataItem;
                            field.split_screen = t.Bool(timer.StopOnSave);
                            break;

                        //-------

                        case "EntitySearch":
                            EntitySearch entitySearch = (EntitySearch)dataItem;
                            field.entity_group_id = entitySearch.EntityTypeId;
                            field.default_value = entitySearch.DefaultValue.ToString();
                            field.is_num = t.Bool(entitySearch.IsNum);
                            field.query_type = entitySearch.QueryType;
                            field.min_value = entitySearch.MinSearchLenght.ToString();
                            break;

                        case "EntitySelect":
                            EntitySelect entitySelect = (EntitySelect)dataItem;
                            field.entity_group_id = entitySelect.Source;
                            field.default_value = entitySelect.DefaultValue.ToString();
                            break;

                        case "FieldGroup":
                            FieldContainer fg = (FieldContainer)dataItem;
                            field.default_value = fg.Value;
                            db.fields.Add(field);
                            db.SaveChanges();

                            db.field_versions.Add(MapFieldVersions(field));
                            db.SaveChanges();
                            isSaved = true;
                            if (fg.DataItemList != null)
                            {
                                foreach (DataItem data_item in fg.DataItemList)
                                {
                                    CreateDataItem(field.id, elementId, data_item);
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
        private Element GetElement(int elementId)
        {
            try
            {
                using (var db = GetContext())
                {
                    Element element;

                    //getting element's possible element children
                    List<check_lists> lstElement = db.check_lists.Where(x => x.parent_id == elementId).ToList();


                    if (lstElement.Count > 0) //GroupElement
                    {
                        //list for the DataItems
                        List<Element> lst = new List<Element>();

                        //the actual DataElement
                        try
                        {
                            check_lists cl = db.check_lists.Single(x => x.id == elementId);
                            GroupElement gElement = new GroupElement(cl.id, cl.label, t.Int(cl.display_index), cl.description, t.Bool(cl.approval_enabled), t.Bool(cl.review_enabled),
                                t.Bool(cl.done_button_enabled), t.Bool(cl.extra_fields_enabled), "", lst);

                            //the actual Elements
                            foreach (var subElement in lstElement)
                            {
                                lst.Add(GetElement(subElement.id));
                            }
                            element = gElement;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to find check_list with id:" + elementId, ex);
                        }
                    }
                    else //DataElement
                    {
                        //the actual DataElement
                        try
                        {
                            check_lists cl = db.check_lists.Single(x => x.id == elementId);
                            DataElement dElement = new DataElement(cl.id, cl.label, t.Int(cl.display_index), cl.description, t.Bool(cl.approval_enabled), t.Bool(cl.review_enabled),
                                t.Bool(cl.done_button_enabled), t.Bool(cl.extra_fields_enabled), "", new List<DataItemGroup>(), new List<DataItem>());

                            //the actual DataItems
                            List<fields> lstFields = db.fields.Where(x => x.check_list_id == elementId && x.parent_field_id == null).ToList();
                            foreach (var field in lstFields)
                            {
                                GetDataItem(dElement.DataItemList, dElement.DataItemGroupList, field.id);
                            }
                            element = dElement;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to find check_list with id:" + elementId, ex);
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

        private void GetDataItem(List<DataItem> lstDataItem, List<DataItemGroup> lstDataItemGroup, int dataItemId)
        {
            try
            {
                using (var db = GetContext())
                {
                    fields f = db.fields.Single(x => x.id == dataItemId);
                    string fieldTypeStr = Find(t.Int(f.field_type_id));

                    //KEY POINT - mapping
                    switch (fieldTypeStr)
                    {
                        case "Audio":
                            lstDataItem.Add(new Audio(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.multi)));
                            break;

                        case "CheckBox":
                            lstDataItem.Add(new CheckBox(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Bool(f.default_value), t.Bool(f.selected)));
                            break;

                        case "Comment":
                            lstDataItem.Add(new Comment(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.default_value, t.Int(f.max_length), t.Bool(f.split_screen)));
                            break;

                        case "Date":
                            lstDataItem.Add(new Date(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                DateTime.Parse(f.min_value), DateTime.Parse(f.max_value), f.default_value));
                            break;

                        case "None":
                            lstDataItem.Add(new None(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy)));
                            break;

                        case "Number":
                            lstDataItem.Add(new Number(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.min_value, f.max_value, int.Parse(f.default_value), t.Int(f.decimal_count), f.unit_name));
                            break;

                        case "MultiSelect":
                            lstDataItem.Add(new MultiSelect(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                PairRead(f.key_value_pair_list)));
                            break;

                        case "Picture":
                            lstDataItem.Add(new Picture(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.multi), t.Bool(f.geolocation_enabled)));
                            break;

                        case "SaveButton":
                            lstDataItem.Add(new SaveButton(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.default_value));
                            break;

                        case "ShowPdf":
                            lstDataItem.Add(new ShowPdf(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.default_value));
                            break;

                        case "Signature":
                            lstDataItem.Add(new Signature(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy)));
                            break;

                        case "SingleSelect":
                            lstDataItem.Add(new SingleSelect(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                PairRead(f.key_value_pair_list)));
                            break;

                        case "Text":
                            lstDataItem.Add(new Text(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.default_value, t.Int(f.max_length), t.Bool(f.geolocation_enabled), t.Bool(f.geolocation_forced), t.Bool(f.geolocation_hidden), t.Bool(f.barcode_enabled), f.barcode_type));
                            break;

                        case "Timer":
                            lstDataItem.Add(new Timer(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Bool(f.stop_on_save)));
                            break;

                        case "EntitySearch":
                            lstDataItem.Add(new EntitySearch(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.default_value), t.Int(f.entity_group_id), t.Bool(f.is_num), f.query_type, t.Int(f.min_value), t.Bool(f.barcode_enabled), f.barcode_type));
                            break;

                        case "EntitySelect":
                            lstDataItem.Add(new EntitySelect(t.Int(f.id), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.default_value), t.Int(f.entity_group_id)));
                            break;

                        case "FieldGroup":
                            List<DataItem> lst = new List<DataItem>();
                            //CDataValue description = new CDataValue();
                            //description.InderValue = f.description;
                            lstDataItemGroup.Add(new FieldGroup(f.id.ToString(), f.label, f.description, f.color, t.Int(f.display_index), f.default_value, lst));
                            //lstDataItemGroup.Add(new DataItemGroup(f.id.ToString(), f.label, f.description, f.color, t.Int(f.display_index), f.default_value, lst));

                            //the actual DataItems
                            List<fields> lstFields = db.fields.Where(x => x.parent_field_id == f.id).ToList();
                            foreach (var field in lstFields)
                                GetDataItem(lst, null, field.id); //null, due to FieldGroup, CANT have fieldGroups under them
                            break;

                        default:
                            throw new IndexOutOfRangeException(f.field_type_id + " is not a known/mapped DataItem type");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetDataItem failed", ex);
            }
        }

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
                        converter.Add(new Holder(fieldType.id, fieldType.field_type));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetConverter failed", ex);
            }
        }
        #endregion

        #region EntityItem 
        private void EntityItemCreateUpdate(string entityGroupMUId, EntityItem entityItem)
        {
            try
            {
                using (var db = GetContext())
                {
                    var match = db.entity_items.SingleOrDefault(x => x.entity_item_uid == entityItem.EntityItemUId && x.entity_group_id == entityGroupMUId);

                    if (match != null)
                    {
                        #region same or update
                        if (match.name == entityItem.Name && match.description == entityItem.Description)
                        {
                            //same
                            if (match.workflow_state == "removed")
                            {
                                match.synced = t.Bool(false);
                                match.updated_at = DateTime.Now;
                                match.version = match.version + 1;
                                match.workflow_state = "updated";

                                db.SaveChanges();

                                db.entity_item_versions.Add(MapEntityItemVersions(match));
                                db.SaveChanges();
                            }

                            return;
                        }
                        else
                        {
                            //update
                            match.description = entityItem.Description;
                            match.name = entityItem.Name;
                            match.synced = t.Bool(false);
                            match.updated_at = DateTime.Now;
                            match.version = match.version + 1;
                            match.workflow_state = "updated";

                            db.SaveChanges();

                            db.entity_item_versions.Add(MapEntityItemVersions(match));
                            db.SaveChanges();

                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        #region new
                        entity_items eI = new entity_items();

                        eI.workflow_state = Constants.WorkflowStates.Created;
                        eI.version = 1;
                        eI.created_at = DateTime.Now;
                        eI.updated_at = DateTime.Now;
                        eI.entity_group_id = entityGroupMUId;
                        eI.entity_item_uid = entityItem.EntityItemUId;
                        eI.microting_uid = "";
                        eI.name = entityItem.Name;
                        eI.description = entityItem.Description;
                        eI.synced = t.Bool(false);

                        db.entity_items.Add(eI);
                        db.SaveChanges();

                        db.entity_item_versions.Add(MapEntityItemVersions(eI));
                        db.SaveChanges();

                        return;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemCreateUpdate failed", ex);
            }
        }

        private void EntityItemDelete(string entityGroupMUId, string entityItemUId)
        {
            try
            {
                using (var db = GetContext())
                {
                    entity_items match = db.entity_items.Single(x => x.entity_item_uid == entityItemUId && x.entity_group_id == entityGroupMUId);

                    match.synced = t.Bool(false);
                    match.updated_at = DateTime.Now;
                    match.version = match.version + 1;
                    match.workflow_state = "removed";

                    db.SaveChanges();

                    db.entity_item_versions.Add(MapEntityItemVersions(match));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemUpdate failed", ex);
            }
        }
        #endregion

        #region tags
        public List<Tag> GetAllTags(bool includeRemoved)
        {
            List<Tag> tags = new List<Tag>();
            try
            {
                using (var db = GetContext())
                {
                    List<tags> matches = null;
                    if (!includeRemoved)
                        matches = db.tags.Where(x => x.workflow_state == Constants.WorkflowStates.Created).ToList();
                    else
                        matches = db.tags.ToList();

                    foreach (tags tag in matches)
                    {
                        Tag t = new Tag(tag.id, tag.name, tag.taggings_count);
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

        public int TagCreate(string name)
        {
            try
            {
                using (var db = GetContext())
                {
                    tags tag = db.tags.SingleOrDefault(x => x.name == name);
                    if (tag == null)
                    {
                        tag = new tags();
                        tag.name = name;
                        tag.workflow_state = Constants.WorkflowStates.Created;
                        tag.version = 1;
                        db.tags.Add(tag);
                        db.SaveChanges();

                        db.tag_versions.Add(MapTagVersions(tag));
                        db.SaveChanges();
                        return tag.id;
                    } else
                    {
                        tag.workflow_state = Constants.WorkflowStates.Created;
                        tag.version += 1;
                        db.SaveChanges();

                        db.tag_versions.Add(MapTagVersions(tag));
                        db.SaveChanges();
                        return tag.id;
                    }                    
                }
                //return ;
            }
            catch (Exception ex)
            {
                throw new Exception("TagCreate failed", ex);
            }
        }

        public bool TagDelete(int tagId)
        {
            try
            {
                using (var db = GetContext())
                {
                    tags tag = db.tags.SingleOrDefault(x => x.id == tagId);
                    if (tag != null)                    
                    {
                        tag.workflow_state = Constants.WorkflowStates.Removed;
                        tag.version += 1;
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
        private string Find(int fieldTypeId)
        {
            foreach (var holder in converter)
            {
                if (holder.Index == fieldTypeId)
                    return holder.FieldType;
            }
            throw new Exception("Find failed. Not known fieldType for fieldTypeId: " + fieldTypeId);
        }

        private int Find(string typeStr)
        {
            foreach (var holder in converter)
            {
                if (holder.FieldType == typeStr)
                    return holder.Index;
            }
            throw new Exception("Find failed. Not known fieldTypeId for typeStr: " + typeStr);
        }

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

        private List<KeyValuePair> PairRead(string str)
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
       
        private case_versions MapCaseVersions(cases aCase)
        {
            case_versions caseVer = new case_versions();
            caseVer.status = aCase.status;
            caseVer.done_at = aCase.done_at;
            caseVer.updated_at = aCase.updated_at;
            caseVer.done_by_user_id = aCase.done_by_user_id;
            caseVer.workflow_state = aCase.workflow_state;
            caseVer.version = aCase.version;
            caseVer.microting_check_uid = aCase.microting_check_uid;
            caseVer.unit_id = aCase.unit_id;

            caseVer.type = aCase.type;
            caseVer.created_at = aCase.created_at;
            caseVer.check_list_id = aCase.check_list_id;
            caseVer.microting_uid = aCase.microting_uid;
            caseVer.site_id = aCase.site_id;
            caseVer.field_value_1 = aCase.field_value_1;
            caseVer.field_value_2 = aCase.field_value_2;
            caseVer.field_value_3 = aCase.field_value_3;
            caseVer.field_value_4 = aCase.field_value_4;
            caseVer.field_value_5 = aCase.field_value_5;
            caseVer.field_value_6 = aCase.field_value_6;
            caseVer.field_value_7 = aCase.field_value_7;
            caseVer.field_value_8 = aCase.field_value_8;
            caseVer.field_value_9 = aCase.field_value_9;
            caseVer.field_value_10 = aCase.field_value_10;

            caseVer.case_id = aCase.id; //<<--

            return caseVer;
        }

        private check_list_versions MapCheckListVersions(check_lists checkList)
        {
            check_list_versions clv = new check_list_versions();
            clv.created_at = checkList.created_at;
            clv.updated_at = checkList.updated_at;
            clv.label = checkList.label;
            clv.description = checkList.description;
            clv.custom = checkList.custom;
            clv.workflow_state = checkList.workflow_state;
            clv.parent_id = checkList.parent_id;
            clv.repeated = checkList.repeated;
            clv.version = checkList.version;
            clv.case_type = checkList.case_type;
            clv.folder_name = checkList.folder_name;
            clv.display_index = checkList.display_index;
            clv.review_enabled = checkList.review_enabled;
            clv.manual_sync = checkList.manual_sync;
            clv.extra_fields_enabled = checkList.extra_fields_enabled;
            clv.done_button_enabled = checkList.done_button_enabled;
            clv.approval_enabled = checkList.approval_enabled;
            clv.multi_approval = checkList.multi_approval;
            clv.fast_navigation = checkList.fast_navigation;
            clv.download_entities = checkList.download_entities;
            clv.field_1 = checkList.field_1;
            clv.field_2 = checkList.field_2;
            clv.field_3 = checkList.field_3;
            clv.field_4 = checkList.field_4;
            clv.field_5 = checkList.field_5;
            clv.field_6 = checkList.field_6;
            clv.field_7 = checkList.field_7;
            clv.field_8 = checkList.field_8;
            clv.field_9 = checkList.field_9;
            clv.field_10 = checkList.field_10;

            clv.check_list_id = checkList.id; //<<--

            return clv;
        }

        private check_list_value_versions MapCheckListValueVersions(check_list_values checkListValue)
        {
            check_list_value_versions clvv = new check_list_value_versions();
            clvv.version = checkListValue.version;
            clvv.created_at = checkListValue.created_at;
            clvv.updated_at = checkListValue.updated_at;
            clvv.check_list_id = checkListValue.check_list_id;
            clvv.case_id = checkListValue.case_id;
            clvv.status = checkListValue.status;
            clvv.user_id = checkListValue.user_id;
            clvv.workflow_state = checkListValue.workflow_state;
            clvv.check_list_duplicate_id = checkListValue.check_list_duplicate_id;

            clvv.check_list_value_id = checkListValue.id; //<<--

            return clvv;
        }

        private field_versions MapFieldVersions(fields field)
        {
            field_versions fv = new field_versions();

            fv.version = field.version;
            fv.created_at = field.created_at;
            fv.updated_at = field.updated_at;
            fv.custom = field.custom;
            fv.workflow_state = field.workflow_state;
            fv.check_list_id = field.check_list_id;
            fv.label = field.label;
            fv.description = field.description;
            fv.field_type_id = field.field_type_id;
            fv.display_index = field.display_index;
            fv.dummy = field.dummy;
            fv.parent_field_id = field.parent_field_id;
            fv.optional = field.optional;
            fv.multi = field.multi;
            fv.default_value = field.default_value;
            fv.selected = field.selected;
            fv.min_value = field.min_value;
            fv.max_value = field.max_value;
            fv.max_length = field.max_length;
            fv.split_screen = field.split_screen;
            fv.decimal_count = field.decimal_count;
            fv.unit_name = field.unit_name;
            fv.key_value_pair_list = field.key_value_pair_list;
            fv.geolocation_enabled = field.geolocation_enabled;
            fv.geolocation_forced = field.geolocation_forced;
            fv.geolocation_hidden = field.geolocation_hidden;
            fv.stop_on_save = field.stop_on_save;
            fv.mandatory = field.mandatory;
            fv.read_only = field.read_only;
            fv.color = field.color;
            fv.barcode_enabled = field.barcode_enabled;
            fv.barcode_type = field.barcode_type;

            fv.field_id = field.id; //<<--

            return fv;
        }

        private field_value_versions MapFieldValueVersions(field_values fieldValue)
        {
            field_value_versions fvv = new field_value_versions();

            fvv.created_at = fieldValue.created_at;
            fvv.updated_at = fieldValue.updated_at;
            fvv.value = fieldValue.value;
            fvv.latitude = fieldValue.latitude;
            fvv.longitude = fieldValue.longitude;
            fvv.altitude = fieldValue.altitude;
            fvv.heading = fieldValue.heading;
            fvv.date = fieldValue.date;
            fvv.accuracy = fieldValue.accuracy;
            fvv.uploaded_data_id = fieldValue.uploaded_data_id;
            fvv.version = fieldValue.version;
            fvv.case_id = fieldValue.case_id;
            fvv.field_id = fieldValue.field_id;
            fvv.user_id = fieldValue.user_id;
            fvv.workflow_state = fieldValue.workflow_state;
            fvv.check_list_id = fieldValue.check_list_id;
            fvv.check_list_duplicate_id = fieldValue.check_list_duplicate_id;
            fvv.done_at = fieldValue.done_at;

            fvv.field_value_id = fieldValue.id; //<<--

            return fvv;
        }

        private uploaded_data_versions MapUploadedDataVersions(uploaded_data uploadedData)
        {
            uploaded_data_versions udv = new uploaded_data_versions();

            udv.created_at = uploadedData.created_at;
            udv.updated_at = uploadedData.updated_at;
            udv.checksum = uploadedData.checksum;
            udv.extension = uploadedData.extension;
            udv.current_file = uploadedData.current_file;
            udv.uploader_id = uploadedData.uploader_id;
            udv.uploader_type = uploadedData.uploader_type;
            udv.workflow_state = uploadedData.workflow_state;
            udv.expiration_date = uploadedData.expiration_date;
            udv.version = uploadedData.version;
            udv.local = uploadedData.local;
            udv.file_location = uploadedData.file_location;
            udv.file_name = uploadedData.file_name;

            udv.data_uploaded_id = uploadedData.id; //<<--

            return udv;
        }

        private check_list_site_versions MapCheckListSiteVersions(check_list_sites checkListSite)
        {
            check_list_site_versions checkListSiteVer = new check_list_site_versions();
            checkListSiteVer.check_list_id = checkListSite.check_list_id;
            checkListSiteVer.created_at = checkListSite.created_at;
            checkListSiteVer.updated_at = checkListSite.updated_at;
            checkListSiteVer.last_check_id = checkListSite.last_check_id;
            checkListSiteVer.microting_uid = checkListSite.microting_uid;
            checkListSiteVer.site_id = checkListSite.site_id;
            checkListSiteVer.version = checkListSite.version;
            checkListSiteVer.workflow_state = checkListSite.workflow_state;

            checkListSiteVer.check_list_site_id = checkListSite.id; //<<--

            return checkListSiteVer;
        }

        private entity_group_versions MapEntityGroupVersions(entity_groups entityGroup)
        {
            entity_group_versions entityGroupVer = new entity_group_versions();
            entityGroupVer.created_at = entityGroup.created_at;
            entityGroupVer.id = entityGroup.id;
            entityGroupVer.microting_uid = entityGroup.microting_uid;
            entityGroupVer.name = entityGroup.name;
            entityGroupVer.type = entityGroup.type;
            entityGroupVer.updated_at = entityGroup.updated_at;
            entityGroupVer.version = entityGroup.version;
            entityGroupVer.workflow_state = entityGroup.workflow_state;

            entityGroupVer.entity_group_id = entityGroup.id; //<<--

            return entityGroupVer;
        }

        private entity_item_versions MapEntityItemVersions(entity_items entityItem)
        {
            entity_item_versions entityItemVer = new entity_item_versions();
            entityItemVer.workflow_state = entityItem.workflow_state;
            entityItemVer.version = entityItem.version;
            entityItemVer.created_at = entityItem.created_at;
            entityItemVer.updated_at = entityItem.updated_at;
            entityItemVer.entity_item_uid = entityItem.entity_item_uid;
            entityItemVer.microting_uid = entityItem.microting_uid;
            entityItemVer.name = entityItem.name;
            entityItemVer.description = entityItem.description;
            entityItemVer.synced = entityItem.synced;
            entityItemVer.display_index = entityItem.display_index;

            entityItemVer.entity_items_id = entityItem.id; //<<--

            return entityItemVer;
        }

        private site_worker_versions MapSiteWorkerVersions(site_workers site_workers)
        {
            site_worker_versions siteWorkerVer = new site_worker_versions();
            siteWorkerVer.workflow_state = site_workers.workflow_state;
            siteWorkerVer.version = site_workers.version;
            siteWorkerVer.created_at = site_workers.created_at;
            siteWorkerVer.updated_at = site_workers.updated_at;
            siteWorkerVer.microting_uid = site_workers.microting_uid;
            siteWorkerVer.site_id = site_workers.site_id;
            siteWorkerVer.worker_id = site_workers.worker_id;

            siteWorkerVer.site_worker_id = site_workers.id; //<<--

            return siteWorkerVer;
        }

        private site_versions MapSiteVersions(sites site)
        {
            site_versions siteVer = new site_versions();
            siteVer.workflow_state = site.workflow_state;
            siteVer.version = site.version;
            siteVer.created_at = site.created_at;
            siteVer.updated_at = site.updated_at;
            siteVer.microting_uid = site.microting_uid;
            siteVer.name = site.name;

            siteVer.site_id = site.id; //<<--

            return siteVer;
        }

        private unit_versions MapUnitVersions(units units)
        {
            unit_versions unitVer = new unit_versions();
            unitVer.workflow_state = units.workflow_state;
            unitVer.version = units.version;
            unitVer.created_at = units.created_at;
            unitVer.updated_at = units.updated_at;
            unitVer.microting_uid = units.microting_uid;
            unitVer.site_id = units.site_id;
            unitVer.customer_no = units.customer_no;
            unitVer.otp_code = units.otp_code;

            unitVer.unit_id = units.id; //<<--

            return unitVer;
        }

        private worker_versions MapWorkerVersions(workers workers)
        {
            worker_versions workerVer = new worker_versions();
            workerVer.workflow_state = workers.workflow_state;
            workerVer.version = workers.version;
            workerVer.created_at = workers.created_at;
            workerVer.updated_at = workers.updated_at;
            workerVer.microting_uid = workers.microting_uid;
            workerVer.first_name = workers.first_name;
            workerVer.last_name = workers.last_name;

            workerVer.worker_id = workers.id; //<<--

            return workerVer;
        }

        private tag_versions MapTagVersions(tags tags)
        {
            tag_versions tagVer = new tag_versions();
            tagVer.workflow_state = tags.workflow_state;
            tagVer.version = tags.version;
            tagVer.created_at = tags.created_at;
            tagVer.updated_at = tags.updated_at;
            tagVer.name = tags.name;

            return tagVer;
        }

        private tagging_versions MapTaggingVersions(taggings taggings)
        {
            tagging_versions taggingVer = new tagging_versions();
            taggingVer.workflow_state = taggings.workflow_state;
            taggingVer.version = taggings.version;
            taggingVer.created_at = taggings.created_at;
            taggingVer.updated_at = taggings.updated_at;
            taggingVer.check_list_id = taggings.check_list_id;
            taggingVer.tag_id = taggings.tag_id;
            taggingVer.tagger_id = taggings.tagger_id;

            return taggingVer;
        }
        #endregion
        #endregion       

        private void FieldTypeAdd(int id, string fieldType, string description)
        {
            using (var db = GetContext())
            {
                field_types fT = new field_types();
                fT.id = id;
                fT.field_type = fieldType;
                fT.description = description;

                db.field_types.Add(fT);
                db.SaveChanges();
            }
        }
    }

    public enum Settings
    {
        firstRunDone,
        knownSitesDone,
        logLevel,
        logLimit,
        fileLocationPicture,
        fileLocationPdf,
        fileLocationJasper,
        token,
        comAddressApi,
        comAddressBasic,
        comAddressPdfUpload,
        comOrganizationId,
        awsAccessKeyId,
        awsSecretAccessKey,
        awsEndPoint,
        unitLicenseNumber,
        httpServerAddress
    }
}