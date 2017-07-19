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

namespace eFormSqlController
{
    public class SqlController : LogWriter
    {
        #region var
        List<Holder> converter;
        object _lockQuery = new object();
        string connectionStr;
        Tools t = new Tools();

        object _writeLock = new object();
        #endregion

        #region con
        public SqlController(string connectionString)
        {
            try
            {
                connectionStr = connectionString;
                PrimeDb(); //if needed
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public SqlController(string connectionString, bool primeDb)
        {
            try
            {
                connectionStr = connectionString;
                if (primeDb)
                    PrimeDb(); //if needed
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public bool MigrateDb()
        {
            var configuration = new Configuration();
            configuration.TargetDatabase = new DbConnectionInfo(connectionStr, "System.Data.SqlClient");
            var migrator = new DbMigrator(configuration);
            migrator.Update();
            return true;
        }

        private void PrimeDb()
        {
            int settingsCount = 0;
                
            try
            #region checks database connectionString works
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    settingsCount = db.settings.Count();
                }
            }
            #endregion
            catch (Exception ex)
            #region if failed, will try to update context
            {
                if (ex.Message.Contains("context has changed") || ex.Message.Contains("'cases'"))
                {
                    MigrateDb();
                }
                else         
                    throw ex;
            }
            #endregion

            if (SettingCheckAll())
                return;

            #region prime db
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (settingsCount != Enum.GetNames(typeof(Settings)).Length)
                    {
                        if (settingsCount == 0)
                        {
                            #region prime Settings
                            UnitTest_TruncateTable(typeof(settings).Name);

                            SettingCreate(Settings.firstRunDone, 1);
                            SettingCreate(Settings.knownSitesDone, 2);
                            SettingCreate(Settings.logLevel, 3);
                            SettingCreate(Settings.logLimit, 4);
                            SettingCreate(Settings.fileLocationPicture, 5);
                            SettingCreate(Settings.fileLocationPdf, 6);
                            SettingCreate(Settings.token, 7);
                            SettingCreate(Settings.comAddressBasic, 8);
                            SettingCreate(Settings.comAddressApi, 9);
                            SettingCreate(Settings.comOrganizationId, 10);
                            SettingCreate(Settings.awsAccessKeyId, 11);
                            SettingCreate(Settings.awsSecretAccessKey, 12);
                            SettingCreate(Settings.awsEndPoint, 13);
                            SettingCreate(Settings.unitLicenseNumber, 14);
                            SettingCreate(Settings.comAddressPdfUpload, 15);

                            SettingUpdate(Settings.firstRunDone, "false");
                            SettingUpdate(Settings.knownSitesDone, "false");
                            SettingUpdate(Settings.logLevel, "true");
                            SettingUpdate(Settings.logLimit, "250");
                            SettingUpdate(Settings.fileLocationPicture, "dataFolder/picture/");
                            SettingUpdate(Settings.fileLocationPdf, "dataFolder/pdf/");
                            SettingUpdate(Settings.token, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                            SettingUpdate(Settings.comAddressBasic, "https://basic.microting.com");
                            SettingUpdate(Settings.comAddressApi, "https://xxxxxx.xxxxxx.com");
                            SettingUpdate(Settings.comOrganizationId, "0");
                            #endregion
                        }
                        else
                            throw new Exception("Settings needs to be corrected. Please either inspect or clear the Settings table in the Microting database");
                    }
                }
            }
            catch (Exception ex)
            {
                // This is here because, the priming process of the DB, will require us to go through the process of migrating the DB multiple times.
                if (ex.Message.Contains("context has changed")) 
                {
                    var configuration = new Configuration();
                    configuration.TargetDatabase = new DbConnectionInfo(connectionStr, "System.Data.SqlClient");
                    var migrator = new DbMigrator(configuration);
                    migrator.Update();
                    PrimeDb(); // It's on purpose we call our self until we have no more migrations.
                }
                else
                    throw new Exception(t.GetMethodName() + " failed", ex);
            }
            #endregion
        }
        #endregion

        #region public
        #region public template
        public int                  TemplateCreate(MainElement mainElement)
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

        public MainElement          TemplateRead(int templateId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    MainElement mainElement = null;
                    GetConverter();
                    
                    check_lists mainCl = db.check_lists.SingleOrDefault(x => x.id == templateId);

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

        public Template_Dto         TemplateItemRead(int templateId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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
                    Template_Dto templateDto = new Template_Dto(checkList.id, checkList.created_at, checkList.updated_at, checkList.label, checkList.description, (int)checkList.repeated, checkList.folder_name, checkList.workflow_state, sites, hasCases, checkList.display_index);
                    return templateDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public List<Template_Dto>   TemplateItemReadAll(bool includeRemoved)
        {
            try
            {
                List<Template_Dto> templateList = new List<Template_Dto>();

                using (var db = new MicrotingDb(connectionStr))
                {
                    List<check_lists> matches = null;

                    if (includeRemoved)
                        matches = db.check_lists.Where(x => x.parent_id != null).ToList();
                    else
                        matches = db.check_lists.Where(x => x.parent_id == null && x.workflow_state == "created").ToList();

                    foreach (check_lists checkList in matches)
                    {
                        List<SiteName_Dto> sites = new List<SiteName_Dto>();
                        foreach (check_list_sites check_list_site in checkList.check_list_sites.Where(x => x.workflow_state != "removed").ToList())
                        {
                            SiteName_Dto site = new SiteName_Dto((int)check_list_site.site.microting_uid, check_list_site.site.name, check_list_site.site.created_at, check_list_site.site.updated_at);
                            sites.Add(site);
                        }
                        bool hasCases = false;
                        if (checkList.cases.Count() > 0)
                             hasCases = true;
                        Template_Dto templateDto = new Template_Dto(checkList.id, checkList.created_at, checkList.updated_at, checkList.label, checkList.description, (int)checkList.repeated, checkList.folder_name, checkList.workflow_state, sites, hasCases, checkList.display_index);
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

        public List<fields>         TemplateFieldReadAll(int templateId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    MainElement mainElement = TemplateRead(templateId);
                    List<fields> fieldLst = new List<fields>();

                    foreach (var dataItem in mainElement.DataItemGetAll())
                        fieldLst.Add(db.fields.Single(x => x.id == dataItem.Id));

                    return fieldLst;
                }
            }
            catch (Exception ex)
            {
                //logger.LogException(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool                 TemplateDisplayIndexChange(int templateId, int newDisplayIndex)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists checkList = db.check_lists.SingleOrDefault(x => x.id == templateId);

                    if (checkList == null)
                        return false;

                    checkList.updated_at = DateTime.Now;
                    checkList.version = checkList.version + 1;
                    checkList.display_index = newDisplayIndex;

                    db.version_check_lists.Add(MapCheckListVersions(checkList));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public bool                 TemplateDelete(int templateId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    check_lists check_list = db.check_lists.Single(x => x.id == templateId);

                    if (check_list != null)
                    {
                        check_list.version = check_list.version + 1;
                        check_list.updated_at = DateTime.Now;

                        check_list.workflow_state = "removed";

                        db.version_check_lists.Add(MapCheckListVersions(check_list));
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
        
        #region public (pre)case
        public void                 CheckListSitesCreate(int checkListId, int siteUId, string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

                    db.version_check_list_sites.Add(MapCheckListSiteVersions(cLS));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckListSitesCreate failed", ex);
            }
        }

        public int                  CheckListSitesRead(int templateId, int siteUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    sites site = db.sites.Single(x => x.microting_uid == siteUId);
                    return int.Parse(db.check_list_sites.Single(x => x.site_id == site.id && x.check_list_id == templateId && x.workflow_state != "removed").microting_uid);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckListSitesCreate failed", ex);
            }
        }

        public int                  CaseCreate(int checkListId, int siteUId, string microtingUId, string microtingCheckId, string caseUId, string custom, DateTime createdAt)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

                        db.version_cases.Add(MapCaseVersions(aCase));
                        db.SaveChanges();
                    } else
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

                        db.version_cases.Add(MapCaseVersions(aCase));
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

        public string               CaseReadCheckIdByMUId(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public void                 CaseUpdateRetrived(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases match = db.cases.SingleOrDefault(x => x.microting_uid == microtingUId);

                    if (match != null)
                    {
                        match.status = 77;
                        match.updated_at = DateTime.Now;
                        match.version = match.version + 1;

                        db.version_cases.Add(MapCaseVersions(match));
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdateRetrived failed", ex);
            }
        }

        public void                 CaseUpdateCompleted(string microtingUId, string microtingCheckId, DateTime doneAt, int userUId, int unitUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

                        db.version_check_list_sites.Add(MapCheckListSiteVersions(match));
                        db.SaveChanges();
                    }
                    #endregion

                    db.version_cases.Add(MapCaseVersions(caseStd));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdateCompleted failed", ex);
            }
        }

        public void                 CaseRetract(string microtingUId, string microtingCheckId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases match = db.cases.Single(x => x.microting_uid == microtingUId && x.microting_check_uid == microtingCheckId);

                    match.updated_at = DateTime.Now;
                    match.workflow_state = "retracted";
                    match.version = match.version + 1;
        
                    db.version_cases.Add(MapCaseVersions(match));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseRetract failed", ex);
            }
        }
        
        public void                 CaseDelete(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases aCase = db.cases.Single(x => x.microting_uid == microtingUId && x.workflow_state != "removed" && x.workflow_state != "retracted");

                    aCase.updated_at = DateTime.Now;
                    aCase.workflow_state = "removed";
                    aCase.version = aCase.version + 1;

                    db.version_cases.Add(MapCaseVersions(aCase));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseDelete failed", ex);
            }
        }

        public void                 CaseDeleteReversed(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_list_sites site = db.check_list_sites.Single(x => x.microting_uid == microtingUId);

                    site.updated_at = DateTime.Now;
                    site.workflow_state = "removed";
                    site.version = site.version + 1;

                    db.version_check_list_sites.Add(MapCheckListSiteVersions(site));
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
        public void                 ChecksCreate(Response response, string xmlString, int xmlIndex)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    int elementId;
                    int userUId = int.Parse(response.Checks[xmlIndex].WorkerId);
                    int userId = db.workers.Single(x => x.microting_uid == userUId).id;
                    List<string> elements = t.LocateList(xmlString, "<ElementList>", "</ElementList>");
                    List<fields> TemplatFieldLst = null;
                    cases responseCase = null;

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

                        db.version_check_list_values.Add(MapCheckListValueVersions(clv));
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
                                    dU.uploader_type = "system";
                                    dU.workflow_state = "pre_created";
                                    dU.version = 1;
                                    dU.local = 0;
                                    dU.file_location = t.Locate(dataItemStr, "<URL>", "</");

                                    db.data_uploaded.Add(dU);
                                    db.SaveChanges();

                                    db.version_data_uploaded.Add(MapUploadedDataVersions(dU));
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
                                fieldV.latitude = t.Locate(dataItemStr, " <Latitude>", "</");
                                fieldV.longitude = t.Locate(dataItemStr, "<Longitude>", "</");
                                fieldV.altitude = t.Locate(dataItemStr, "<Altitude>", "</");
                                fieldV.heading = t.Locate(dataItemStr, "<Heading>", "</");
                                fieldV.accuracy = t.Locate(dataItemStr, "<Accuracy>", "</");
                                fieldV.date = t.Date(t.Locate(dataItemStr, "<Date>", "</"));
                                //
                                fieldV.workflow_state = "created";
                                fieldV.version = 1;
                                fieldV.case_id = responseCase.id;
                                fieldV.field_id = int.Parse(t.Locate(dataItemStr, "<Id>", "</"));
                                fieldV.user_id = userId;
                                fieldV.check_list_id = clv.check_list_id;
                                fieldV.done_at = t.Date(response.Checks[xmlIndex].Date);

                                db.field_values.Add(fieldV);
                                db.SaveChanges();

                                db.version_field_values.Add(MapFieldValueVersions(fieldV));
                                db.SaveChanges();

                                #region remove dataItem duplicate from TemplatDataItemLst
                                int index = 0;
                                foreach (var field in TemplatFieldLst)
                                {
                                    if (fieldV.field_id == field.id)
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
                        fieldV.workflow_state = "created";
                        fieldV.version = 1;
                        fieldV.case_id = responseCase.id;
                        fieldV.field_id = field.id;
                        fieldV.user_id = userId;
                        fieldV.check_list_id = field.check_list_id;
                        fieldV.done_at = t.Date(response.Checks[xmlIndex].Date);

                        db.field_values.Add(fieldV);
                        db.SaveChanges();

                        db.version_field_values.Add(MapFieldValueVersions(fieldV));
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

        public ReplyElement         CheckRead(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        private Element             SubChecks(int parentId, int caseId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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
                    } else
                    {
                        List<DataItemGroup> dataItemGroupList = new List<DataItemGroup>();
                        List<DataItem> dataItemList = new List<DataItem>();
                        foreach (fields field in checkList.fields.Where(x => x.parent_field_id == null).ToList())
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
                                FieldGroup fG = new FieldGroup(field.id.ToString(), field.label, field.description, field.color, (int)field.display_index, field.default_value, dataItemSubList);
                                dataItemGroupList.Add(fG);
                            } else
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

        public List<field_values>   ChecksRead(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public Field                FieldRead(int id)
        {

            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public FieldValue           FieldValueRead(fields question, field_values reply, bool joinUploadedData)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

                                    uploadedData = db.data_uploaded.Single(x => x.id == uploadedDataId);

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
                                answer.UploadedDataObj = uploadedDataObj;
                                answer.UploadedData = "";
                            }
                            
                        }
                    #endregion
                    answer.Value = reply.value;
                    #region answer.ValueReadable = reply.value 'ish' //and if needed: answer.KeyValuePairList = ReadPairs(...);

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
                        } catch { }                                              
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

        public FieldValue           FieldValueRead(int id)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public List<FieldValue>     FieldValueReadList(int id, int instances)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public void                 FieldValueUpdate(int caseId, int fieldId, string value)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    field_values fieldMatch = db.field_values.Single(x => x.case_id == caseId && x.field_id == fieldId);

                    fieldMatch.value = value;
                    fieldMatch.updated_at = DateTime.Now;
                    fieldMatch.version = fieldMatch.version + 1;

                    db.version_field_values.Add(MapFieldValueVersions(fieldMatch));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FieldValueUpdate failed", ex);
            }
        }

        public List<List<string>>   FieldValueReadAllValues(int fieldId, List<int> caseIds, string customPathForUploadedData)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    fields matchField = db.fields.Single(x => x.id == fieldId);

                    List<field_values> matches = db.field_values.Where(x => x.field_id == fieldId && caseIds.Contains((int)x.case_id)).ToList();

                    List<List<string>> rtrnLst = new List<List<string>>();
                    List<string> replyLst1 = new List<string>();
                    rtrnLst.Add(replyLst1);

                    switch (matchField.field_type.field_type)
                    {
                        #region special dataItem
                        case "CheckBox":
                            foreach (field_values item in matches)
                            {
                                if (item.value == "checked")
                                    replyLst1.Add("1");
                                else
                                    replyLst1.Add("0");
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
                                        if (customPathForUploadedData != null)
                                            replyLst1[lastIndex] = replyLst1[lastIndex] + "|" + customPathForUploadedData + item.uploaded_data.file_name;
                                        else
                                            replyLst1[lastIndex] = replyLst1[lastIndex] + "|" + item.uploaded_data.file_location + item.uploaded_data.file_name;
                                    }
                                    else
                                    {
                                        lastIndex++;
                                        if (item.uploaded_data_id != null)
                                        {
                                            if (customPathForUploadedData != null)
                                                replyLst1.Add(customPathForUploadedData + item.uploaded_data.file_name);
                                            else
                                                replyLst1.Add(item.uploaded_data.file_location + item.uploaded_data.file_name);
                                        } else
                                        {
                                            replyLst1.Add("UPLOADED DATA IS NOT READY FOR FIELD_VALUE ID : " + item.id.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    lastIndex++;
                                    replyLst1.Add("");
                                }
                                lastCaseId = (int)item.case_id;
                            }
                            break;

                        case "SingleSelect":
                            {
                                var kVP = PairRead(matchField.key_value_pair_list);

                                foreach (field_values item in matches)
                                    replyLst1.Add(PairMatch(kVP, item.value));
                            }
                            break;

                        case "MultiSelect":
                            {
                                var kVP = PairRead(matchField.key_value_pair_list);

                                rtrnLst = new List<List<string>>();
                                List<string> replyLst = null;
                                int index = 0;
                                string valueExt = "";

                                foreach (var key in kVP)
                                {
                                    replyLst = new List<string>();
                                    index++;

                                    foreach (field_values item in matches)
                                    {
                                        valueExt = "|" + item.value + "|";
                                        if (valueExt.Contains("|" + index.ToString() + "|"))
                                            replyLst.Add("1");
                                        else
                                            replyLst.Add("0");
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
                                                replyLst1.Add(match.name);
                                            } else
                                            {
                                                replyLst1.Add("");
                                            }

                                        }
                                    }
                                    catch {
                                        replyLst1.Add("");
                                    }
                                }
                            }
                            break;
                        #endregion

                        default:
                            foreach (field_values item in matches)
                                replyLst1.Add(item.value);
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

        public string               CheckListValueStatusRead(int caseId, int checkListId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public void                 CheckListValueStatusUpdate(int caseId, int checkListId, string value)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_list_values match = db.check_list_values.Single(x => x.case_id == caseId && x.check_list_id == checkListId);

                    match.status = value;
                    match.updated_at = DateTime.Now;
                    match.version = match.version + 1;

                    db.version_check_list_values.Add(MapCheckListValueVersions(match));
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
        public void                 NotificationCreate(string notificationUId, string microtingUId, string activity)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (db.notifications.Count(x => x.notification_uid == notificationUId) == 0)
                    {
                        notifications aNote = new notifications();

                        aNote.workflow_state = "created";
                        aNote.created_at = DateTime.Now;
                        aNote.updated_at = DateTime.Now;
                        aNote.notification_uid = notificationUId;
                        aNote.microting_uid = microtingUId;
                        aNote.activity = activity;

                        db.notifications.Add(aNote);
                        db.SaveChanges();
                    }

                    //TODO else log warning
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public Note_Dto             NotificationReadFirst()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    notifications aNoti = db.notifications.FirstOrDefault(x => x.workflow_state == "created");

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

        public void                 NotificationProcessed(string notificationUId, string workflowState)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    notifications aNoti = db.notifications.Single(x => x.notification_uid == notificationUId);
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
        public UploadedData         FileRead()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    uploaded_data dU = db.data_uploaded.FirstOrDefault(x => x.workflow_state == "pre_created");

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

        public Case_Dto             FileCaseFindMUId(string urlString)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    try
                    {
                        uploaded_data dU = db.data_uploaded.Where(x => x.file_location == urlString).First();
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

        public void                 FileProcessed(string urlString, string chechSum, string fileLocation, string fileName, int id)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    uploaded_data uD = db.data_uploaded.Single(x => x.id == id);

                    uD.checksum = chechSum;
                    uD.file_location = fileLocation;
                    uD.file_name = fileName;
                    uD.local = 1;
                    uD.workflow_state = "created";
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
        #endregion
        #endregion

        #region public (post)case
        public Case_Dto             CaseReadByMUId(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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
                        if (cls.workflow_state == "created")
                            stat = "Created";

                        if (cls.workflow_state == "removed")
                            stat = "Deleted";
                        #endregion

                        int remoteSiteId = (int)db.sites.Single(x => x.id == (int)cls.site_id).microting_uid;
                        Case_Dto rtrnCase = new Case_Dto(null, stat, remoteSiteId, cL.case_type, "ReversedCase", cls.microting_uid, cls.last_check_id, null, cL.id);
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

        public Case_Dto             CaseReadByCaseId(int caseId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases aCase = db.cases.Single(x => x.id == caseId);
                    check_lists cL = db.check_lists.Single(x => x.id == aCase.check_list_id);

                    #region string stat = aCase.workflow_state ...
                    string stat = "";
                    if (aCase.workflow_state == "created" && aCase.status != 77)
                        stat = "Created";

                    if (aCase.workflow_state == "created" && aCase.status == 77)
                        stat = "Retrived";

                    if (aCase.workflow_state == "retracted")
                        stat = "Completed";

                    if (aCase.workflow_state == "removed")
                        stat = "Deleted";
                    #endregion

                    int remoteSiteId = (int)db.sites.Single(x => x.id == (int)aCase.site_id).microting_uid;
                    Case_Dto cDto = new Case_Dto(aCase.id, stat, remoteSiteId, cL.case_type, aCase.case_uid, aCase.microting_uid, aCase.microting_check_uid, aCase.custom, cL.id);
                    return cDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByCaseUId failed", ex);
            }
        }

        public List<Case_Dto>       CaseReadByCaseUId(string caseUId)
        {
            try
            {
                if (caseUId == "")
                    throw new Exception("CaseReadByCaseUId failed. Due invalid input:''. This would return incorrect data");

                if (caseUId == "ReversedCase")
                    throw new Exception("CaseReadByCaseUId failed. Due invalid input:'ReversedCase'. This would return incorrect data");

                using (var db = new MicrotingDb(connectionStr))
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

        public cases                CaseReadFull(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public List<Case>           CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (start == null)
                        start = DateTime.MinValue;
                    if (end == null)
                        end = DateTime.MaxValue;


                    List<cases> matches = null;
                    switch (workflowState)
                    {
                        case "not_retracted":
                            if (templatId == null)
                                matches = db.cases.Where(x => x.done_at > start && x.done_at < end && x.workflow_state != "retracted").ToList();
                            else
                                matches = db.cases.Where(x => x.check_list_id == templatId && x.done_at > start && x.done_at < end && x.workflow_state != "retracted").ToList();
                            break;
                        case "not_removed":
                            if (templatId == null)
                                matches = db.cases.Where(x => x.done_at > start && x.done_at < end && x.workflow_state != "removed").ToList();
                            else
                                matches = db.cases.Where(x => x.check_list_id == templatId && x.done_at > start && x.done_at < end && x.workflow_state != "removed").ToList();
                            break;
                        case "created":
                            if (templatId == null)
                                matches = db.cases.Where(x => x.done_at > start && x.done_at < end && x.workflow_state == "created").ToList();
                            else
                                matches = db.cases.Where(x => x.check_list_id == templatId && x.done_at > start && x.done_at < end && x.workflow_state == "created").ToList();
                            break;
                        case "retracted":
                            if (templatId == null)
                                matches = db.cases.Where(x => x.done_at > start && x.done_at < end && x.workflow_state == "retracted").ToList();
                            else
                                matches = db.cases.Where(x => x.check_list_id == templatId && x.done_at > start && x.done_at < end && x.workflow_state == "retracted").ToList();
                            break;
                        case "removed":
                            if (templatId == null)
                                matches = db.cases.Where(x => x.done_at > start && x.done_at < end && x.workflow_state == "removed").ToList();
                            else
                                matches = db.cases.Where(x => x.check_list_id == templatId && x.done_at > start && x.done_at < end && x.workflow_state == "removed").ToList();
                            break;
                        default:
                            if (templatId == null)
                                matches = db.cases.Where(x => x.done_at > start && x.done_at < end).ToList();
                            else
                                matches = db.cases.Where(x => x.check_list_id == templatId && x.done_at > start && x.done_at < end).ToList();
                            break;
                    }               

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

        public List<Case_Dto>       CaseFindCustomMatchs(string customString)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<Case_Dto> foundCasesThatMatch = new List<Case_Dto>();

                    List<cases> lstMatchs = db.cases.Where(x => x.custom == customString && x.workflow_state == "created").ToList();

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
        #endregion

        #region public interaction tables
        public int                  InteractionCaseCreate(int templateId, string caseUId, List<int> siteUIds, string custom, bool connected, List<string> replacements)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases newCase = new a_interaction_cases();

                    newCase.workflow_state = "created";
                    newCase.version = 1;
                    newCase.created_at = DateTime.Now;
                    newCase.updated_at = DateTime.Now;
                    newCase.case_uid = caseUId;
                    newCase.custom = custom;
                    newCase.connected = t.Bool(connected);
                    newCase.template_id = templateId;
                    newCase.replacements = t.TextLst(replacements);
                    newCase.synced = 1;

                    db.a_interaction_cases.Add(newCase);
                    db.SaveChanges();

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(newCase));
                    db.SaveChanges();

                    a_interaction_case_lists newSite;
                    foreach (int siteId in siteUIds)
                    {
                        newSite = new a_interaction_case_lists();

                        newSite.workflow_state = "created";
                        newSite.version = 1;
                        newSite.created_at = DateTime.Now;
                        newSite.updated_at = DateTime.Now;
                        newSite.a_interaction_case_id = newCase.id;
                        newSite.siteId = siteId;
                        newSite.stat = "Created";

                        db.a_interaction_case_lists.Add(newSite);
                        db.SaveChanges();

                        db.a_interaction_case_list_versions.Add(MapInteractionCaseListVersions(newSite));
                        db.SaveChanges();
                    }

                    return newCase.id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public a_interaction_cases  InteractionCaseReadFirstCreate()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases match = db.a_interaction_cases.FirstOrDefault(x => x.workflow_state == "created");

                    if (match == null)
                        return null;

                    match.workflow_state = "creating";
                    match.updated_at = DateTime.Now;
                    match.version = match.version + 1;

                    db.SaveChanges();

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(match));
                    db.SaveChanges();

                    return match;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public a_interaction_cases  InteractionCaseReadFirstDelete()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases match = db.a_interaction_cases.FirstOrDefault(x => x.workflow_state == "delete");

                    if (match == null)
                        return null;

                    match.workflow_state = "deleting";
                    match.updated_at = DateTime.Now;
                    match.version = match.version + 1;

                    db.SaveChanges();

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(match));
                    db.SaveChanges();

                    return match;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public List<a_interaction_case_lists> InteractionCaseListRead(int interactionCaseId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases match = db.a_interaction_cases.SingleOrDefault(x => x.id == interactionCaseId);

                    if (match == null)
                        return null;

                    List<a_interaction_case_lists> rtnLst = new List<a_interaction_case_lists>();

                    foreach (var item in match.a_interaction_case_lists)
                        rtnLst.Add(item);

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public bool                 InteractionCaseUpdate(Case_Dto caseDto)
        {
            if (caseDto.Stat == "Created")
                return true;

            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_case_lists matchSite = db.a_interaction_case_lists.SingleOrDefault(x => x.microting_uid == caseDto.MicrotingUId);

                    if (matchSite == null)
                        return false;

                    a_interaction_cases matchCase = matchSite.a_interaction_case;

                    matchCase.updated_at = DateTime.Now;
                    matchCase.version = matchCase.version + 1;
                    matchCase.synced = 0;

                    matchSite.case_id = caseDto.CaseId;
                    matchSite.check_uid = caseDto.CheckUId;
                    matchSite.stat = caseDto.Stat;
                    matchSite.updated_at = DateTime.Now;
                    matchSite.version = matchSite.version + 1;

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(matchCase));
                    db.a_interaction_case_list_versions.Add(MapInteractionCaseListVersions(matchSite));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public void                 InteractionCaseProcessedCreate(int interactionCaseId, List<int> siteUIds, List<string> microtingUIds)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases matchCase = db.a_interaction_cases.Single(x => x.id == interactionCaseId);
                    matchCase.workflow_state = "processed";
                    matchCase.updated_at = DateTime.Now;
                    matchCase.version = matchCase.version + 1;
                    matchCase.synced = 0;

                    int index = 0;
                    int count = siteUIds.Count();

                    while (index < count)
                    {
                        int siteId = siteUIds[index];
                        int iCaseId = matchCase.id;
                        a_interaction_case_lists matchSite = db.a_interaction_case_lists.Single(x => x.a_interaction_case_id == iCaseId && x.siteId == siteId);
                        matchSite.updated_at = DateTime.Now;
                        matchSite.version = matchSite.version + 1;
                        matchSite.microting_uid = microtingUIds[index];
                        matchSite.stat = "Sent";

                        try
                        {
                            matchSite.case_id = CaseReadFull(microtingUIds[index], null).id;
                        }
                        catch
                        {

                        }

                        db.a_interaction_case_list_versions.Add(MapInteractionCaseListVersions(matchSite));
                        index++;
                    }

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(matchCase));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public void                 InteractionCaseProcessedDelete(int interactionCaseId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases matchCase = db.a_interaction_cases.Single(x => x.id == interactionCaseId);
                    matchCase.workflow_state = "removed";
                    matchCase.updated_at = DateTime.Now;
                    matchCase.version = matchCase.version + 1;
                    matchCase.synced = 0;

                    db.SaveChanges();

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(matchCase));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public bool                 InteractionCaseDelete(int interactionCaseId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases newSite = db.a_interaction_cases.SingleOrDefault(x => x.id == interactionCaseId);

                    if (newSite == null)
                        return false;

                    newSite.workflow_state = "delete";
                    newSite.version = newSite.version + 1;
                    newSite.updated_at = DateTime.Now;

                    db.SaveChanges();

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(newSite));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public void                 InteractionCaseFailed(int interactionCaseId, string expectionString)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases matchCase = db.a_interaction_cases.Single(x => x.id == interactionCaseId);
                    matchCase.workflow_state = "failed to sync";
                    matchCase.updated_at = DateTime.Now;
                    matchCase.version = matchCase.version + 1;
                    matchCase.synced = 0;
                    matchCase.expectionString = expectionString;

                    db.SaveChanges();

                    db.a_interaction_case_versions.Add(MapInteractionCaseVersions(matchCase));
                    db.SaveChanges();
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
        public List<SiteName_Dto>   SiteGetAll()
        {
            List<SiteName_Dto> siteList = new List<SiteName_Dto>();
            using (var db = new MicrotingDb(connectionStr))
            {
                foreach (sites aSite in db.sites.ToList())
                {
                    SiteName_Dto siteNameDto = new SiteName_Dto((int)aSite.microting_uid, aSite.name, aSite.created_at, aSite.updated_at);
                    siteList.Add(siteNameDto);
                }
            }
            return siteList;

        }

        public List<Site_Dto>       SimpleSiteGetAll(string workflowState, int? offSet, int? limit)
        {
            List<Site_Dto> siteList = new List<Site_Dto>();
            using (var db = new MicrotingDb(connectionStr))
            {
                List<sites> matches = null;
                switch (workflowState)
                {
                    case "not_removed":
                        matches = db.sites.Where(x => x.workflow_state != "removed").ToList();
                        break;
                    case "removed":
                        matches = db.sites.Where(x => x.workflow_state == "removed").ToList();
                        break;
                    case "created":
                        matches = db.sites.Where(x => x.workflow_state == "created").ToList();
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
                    } catch { }

                    try
                    {                       
                        Site_Dto siteDto = new Site_Dto((int)aSite.microting_uid, aSite.name, workerFirstName, workerLastName, unitCustomerNo, unitOptCode, unitMicrotingUid, workerMicrotingUid);
                        siteList.Add(siteDto);
                    } catch
                    {
                        Site_Dto siteDto = new Site_Dto((int)aSite.microting_uid, aSite.name, workerFirstName, workerLastName, unitCustomerNo, unitOptCode, unitMicrotingUid, workerMicrotingUid);
                        siteList.Add(siteDto);
                    }                                       
                }
            }
            return siteList;

        }

        public int                  SiteCreate(int microtingUid, string name)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = new sites();
                    site.workflow_state = "created";
                    site.version = 1;
                    site.created_at = DateTime.Now;
                    site.updated_at = DateTime.Now;
                    site.microting_uid = microtingUid;
                    site.name = name;


                    db.sites.Add(site);
                    db.SaveChanges();

                    db.version_sites.Add(MapSiteVersions(site));
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

        public SiteName_Dto         SiteRead(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid && x.workflow_state == "created");

                    if (site != null)
                        return new SiteName_Dto((int)site.microting_uid, site.name, site.created_at, site.updated_at);
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

        public Site_Dto             SiteReadSimple(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid && x.workflow_state == "created");
                    site_workers site_worker = db.site_workers.Where(x => x.site_id == site.id).ToList().First();
                    workers worker = db.workers.Single(x => x.id == site_worker.worker_id);
                    units unit = db.units.Where(x => x.site_id == site.id).ToList().First();

                    if (site != null)
                        return new Site_Dto((int)site.microting_uid, site.name, worker.first_name, worker.last_name, (int)unit.customer_no, (int)unit.otp_code, (int)unit.microting_uid, worker.microting_uid);
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

        public bool                 SiteUpdate(int microting_uid, string name)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid);

                    if (site != null)
                    {
                        site.version = site.version + 1;
                        site.updated_at = DateTime.Now;

                        site.name = name;

                        db.version_sites.Add(MapSiteVersions(site));
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

        public bool                 SiteDelete(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    sites site = db.sites.SingleOrDefault(x => x.microting_uid == microting_uid);

                    if (site != null)
                    {
                        site.version = site.version + 1;
                        site.updated_at = DateTime.Now;

                        site.workflow_state = "removed";

                        db.version_sites.Add(MapSiteVersions(site));
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
        public List<Worker_Dto>     WorkerGetAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName();
            try
            {
                List<Worker_Dto> listWorkerDto = new List<Worker_Dto>();

                using (var db = new MicrotingDb(connectionStr))
                {
                    List<workers> matches = null;

                    switch (workflowState)
                    {
                        case "not_removed":
                            matches = db.workers.Where(x => x.workflow_state != "removed").ToList();
                            break;
                        case "removed":
                            matches = db.workers.Where(x => x.workflow_state == "removed").ToList();
                            break;
                        case "created":
                            matches = db.workers.Where(x => x.workflow_state == "created").ToList();
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

        public int                  WorkerCreate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    workers worker = new workers();
                    worker.workflow_state = "created";
                    worker.version = 1;
                    worker.created_at = DateTime.Now;
                    worker.updated_at = DateTime.Now;
                    worker.microting_uid = microtingUid;
                    worker.first_name = firstName;
                    worker.last_name = lastName;
                    worker.email = email;


                    db.workers.Add(worker);
                    db.SaveChanges();

                    db.version_workers.Add(MapWorkerVersions(worker));
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

        public Worker_Dto           WorkerRead(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    //logger.LogEverything("siteName:" + siteName + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    workers worker = db.workers.SingleOrDefault(x => x.microting_uid == microting_uid && x.workflow_state == "created");

                    if (worker != null)
                        return new Worker_Dto((int)worker.microting_uid, worker.first_name, worker.last_name, worker.email, worker.created_at, worker.updated_at);
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

        public bool                 WorkerUpdate(int microtingUid, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

                        db.version_workers.Add(MapWorkerVersions(worker));
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

        public bool                 WorkerDelete(int microtingUid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    workers worker = db.workers.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (worker != null)
                    {
                        worker.version = worker.version + 1;
                        worker.updated_at = DateTime.Now;

                        worker.workflow_state = "removed";

                        db.version_workers.Add(MapWorkerVersions(worker));
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
        public int                  SiteWorkerCreate(int microtingUId, int siteUId, int workerUId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    int localSiteId = db.sites.Single(x => x.microting_uid == siteUId).id;
                    int localWorkerId = db.workers.Single(x => x.microting_uid == workerUId).id;

                    site_workers site_worker = new site_workers();
                    site_worker.workflow_state = "created";
                    site_worker.version = 1;
                    site_worker.created_at = DateTime.Now;
                    site_worker.updated_at = DateTime.Now;
                    site_worker.microting_uid = microtingUId;
                    site_worker.site_id = localSiteId;
                    site_worker.worker_id = localWorkerId;


                    db.site_workers.Add(site_worker);
                    db.SaveChanges();

                    db.version_site_workers.Add(MapSiteWorkerVersions(site_worker));
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

        public Site_Worker_Dto      SiteWorkerRead(int? microtingUid, int? siteId, int? workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    site_workers site_worker = null;
                    if (microtingUid == null)
                    {
                        sites site = db.sites.Single(x => x.microting_uid == siteId);
                        workers worker = db.workers.Single(x => x.microting_uid == workerId);
                        site_worker = db.site_workers.SingleOrDefault(x => x.site_id == site.id && x.worker_id == worker.id);
                    } else
                    {
                        site_worker = db.site_workers.SingleOrDefault(x => x.microting_uid == microtingUid && x.workflow_state == "created");
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

        public bool                 SiteWorkerUpdate(int microtingUid, int siteId, int workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    site_workers site_worker = db.site_workers.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (site_worker != null)
                    {
                        site_worker.version = site_worker.version + 1;
                        site_worker.updated_at = DateTime.Now;

                        site_worker.site_id = siteId;
                        site_worker.worker_id = workerId;

                        db.version_site_workers.Add(MapSiteWorkerVersions(site_worker));
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

        public bool                 SiteWorkerDelete(int microting_uid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    site_workers site_worker = db.site_workers.SingleOrDefault(x => x.microting_uid == microting_uid);

                    if (site_worker != null)
                    {
                        site_worker.version = site_worker.version + 1;
                        site_worker.updated_at = DateTime.Now;

                        site_worker.workflow_state = "removed";

                        db.version_site_workers.Add(MapSiteWorkerVersions(site_worker));
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
        public List<Unit_Dto>       UnitGetAll()
        {
            string methodName = t.GetMethodName();
            try
            {
                List<Unit_Dto> listWorkerDto = new List<Unit_Dto>();
                using (var db = new MicrotingDb(connectionStr))
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

        public int                  UnitCreate(int microtingUid, int customerNo, int otpCode, int siteUId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");
                    int localSiteId = db.sites.Single(x => x.microting_uid == siteUId).id;

                    units unit = new units();
                    unit.workflow_state = "created";
                    unit.version = 1;
                    unit.created_at = DateTime.Now;
                    unit.updated_at = DateTime.Now;
                    unit.microting_uid = microtingUid;
                    unit.customer_no = customerNo;
                    unit.otp_code = otpCode;
                    unit.site_id = localSiteId;


                    db.units.Add(unit);
                    db.SaveChanges();

                    db.version_units.Add(MapUnitVersions(unit));
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

        public Unit_Dto             UnitRead(int microtingUid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.microting_uid == microtingUid && x.workflow_state == "created");

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

        public bool                 UnitUpdate(int microtingUid, int customerNo, int otpCode, int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (unit != null)
                    {
                        unit.version = unit.version + 1;
                        unit.updated_at = DateTime.Now;

                        unit.customer_no = customerNo;
                        unit.otp_code = otpCode;

                        db.version_units.Add(MapUnitVersions(unit));
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

        public bool                 UnitDelete(int microtingUid)
        {
            string methodName = t.GetMethodName();
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //logger.LogEverything(methodName + " called");

                    units unit = db.units.SingleOrDefault(x => x.microting_uid == microtingUid);

                    if (unit != null)
                    {
                        unit.version = unit.version + 1;
                        unit.updated_at = DateTime.Now;

                        unit.workflow_state = "removed";

                        db.version_units.Add(MapUnitVersions(unit));
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
            if (workflowState != "not_removed" && workflowState != "created" && workflowState != "removed")
                throw new Exception("EntityGroupAll failed. workflowState:" + workflowState + " is not an known workflow state");

            List<entity_groups> eG = null;
            List<EntityGroup> e_G = new List<EntityGroup>();
            int numOfElements = 0;
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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
                            source = source.Where(x => x.workflow_state != "removed");
                            break;
                        case "removed":
                            source = source.Where(x => x.workflow_state == "removed");
                            break;
                        case "created":
                            source = source.Where(x => x.workflow_state == "created");
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


        public EntityGroup                  EntityGroupCreate(string name, string entityType)
        {
            try
            {
                if (entityType != "EntitySearch" && entityType != "EntitySelect")
                    throw new Exception("EntityGroupCreate failed. EntityType:" + entityType + " is not an known type");

                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_groups eG = new entity_groups();

                    eG.created_at = DateTime.Now;
                    //eG.id = xxx;
                    //eG.microtingUId = xxx;
                    eG.name = name;
                    eG.type = entityType;
                    eG.updated_at = DateTime.Now;
                    eG.version = 1;
                    eG.workflow_state = "created";

                    db.entity_groups.Add(eG);
                    db.SaveChanges();

                    db.version_entity_groups.Add(MapEntityGroupVersions(eG));
                    db.SaveChanges();

                    return new EntityGroup(eG.id, eG.name, eG.type, eG.microting_uid, new List<EntityItem>(),eG.workflow_state, eG.created_at, eG.updated_at);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupCreate failed", ex);
            }
        }
        public EntityGroup          EntityGroupReadSorted(string entityGroupMUId, string sort, string nameFilter)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != "removed" && x.workflow_state != "failed_to_sync").OrderBy(x => x.id).ToList();
                        }
                        else
                        {
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != "removed" && x.workflow_state != "failed_to_sync").OrderBy(x => x.name).ToList();
                        }
                    } else
                    {
                        if (sort == "id")
                        {
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != "removed" && x.workflow_state != "failed_to_sync" && x.name.Contains(nameFilter)).OrderBy(x => x.id).ToList();
                        }
                        else
                        {
                            eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != "removed" && x.workflow_state != "failed_to_sync" && x.name.Contains(nameFilter)).OrderBy(x => x.name).ToList();
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

        public EntityGroup          EntityGroupRead(string entityGroupMUId)
        {
            return EntityGroupReadSorted(entityGroupMUId, "id", "");
        }

        public bool                 EntityGroupUpdate(int entityGroupId, string entityGroupMUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.id == entityGroupId);

                    if (eG == null)
                        return false;

                    eG.microting_uid = entityGroupMUId;
                    eG.updated_at = DateTime.Now;
                    eG.version = eG.version + 1;

                    db.SaveChanges();

                    db.version_entity_groups.Add(MapEntityGroupVersions(eG));
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
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.microting_uid == entityGroupMUId);

                    if (eG == null)
                        return false;

                    eG.name = name;
                    eG.updated_at = DateTime.Now;
                    eG.version = eG.version + 1;

                    db.SaveChanges();

                    db.version_entity_groups.Add(MapEntityGroupVersions(eG));
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupUpdate failed", ex);
            }
        }

        public void                 EntityGroupUpdateItems(EntityGroup entityGroup)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

        public string               EntityGroupDelete(string entityGroupMUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> killLst = new List<string>();

                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.microting_uid == entityGroupMUId && x.workflow_state != "removed");

                    if (eG == null)
                        return null;

                    killLst.Add(eG.microting_uid);

                    eG.workflow_state = "removed";
                    eG.updated_at = DateTime.Now;
                    eG.version = eG.version + 1;
                    db.version_entity_groups.Add(MapEntityGroupVersions(eG));

                    List<entity_items> lst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != "removed").ToList();
                    if (lst != null)
                    {
                        foreach (entity_items item in lst)
                        {
                            item.workflow_state = "removed";
                            item.updated_at = DateTime.Now;
                            item.version = item.version + 1;
                            item.synced = t.Bool(false);
                            db.version_entity_items.Add(MapEntityItemVersions(item));

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
        public entity_items         EntityItemRead(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    return db.entity_items.Single(x => x.microting_uid == microtingUId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemRead failed", ex);
            }
        }

        public entity_items         EntityItemSyncedRead()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    return db.entity_items.FirstOrDefault(x => x.synced == 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemSyncedRead failed", ex);
            }
        }

        public void                 EntityItemSyncedProcessed(string entityGroupMUId, string entityItemId, string microting_uid, string workflowState)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_items eItem = db.entity_items.SingleOrDefault(x => x.entity_item_uid == entityItemId && x.entity_group_id == entityGroupMUId);

                    if (eItem != null)
                    {
                        eItem.workflow_state = workflowState;
                        eItem.updated_at = DateTime.Now;
                        eItem.version = eItem.version + 1;
                        eItem.synced = 1;

                        if (workflowState == "created")
                            eItem.microting_uid = microting_uid; //<<---

                        db.version_entity_items.Add(MapEntityItemVersions(eItem));
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
        public void                 SettingCreate(Settings name, int id)
        {
            using (var db = new MicrotingDb(connectionStr))
            {
                settings set = new settings();
                set.id = id;
                set.name = name.ToString();
                set.value = "";

                db.settings.Add(set);
                db.SaveChanges();
            }
        }

        public string               SettingRead  (Settings name)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    settings match = db.settings.SingleOrDefault(x => x.name == name.ToString());
                    return match.value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public void                 SettingUpdate(Settings name, string newValue)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    settings match = db.settings.Single(x => x.name == name.ToString());
                    match.value = newValue;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public bool                 SettingCheckAll()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (db.field_types.Count() != 18)
                    {
                        #region prime FieldTypes
                        UnitTest_TruncateTable(typeof(field_types).Name);

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

                    if (countVal > 0)
                        return false;

                    if (countSet < Enum.GetNames(typeof(Settings)).Length)
                        return false;

                    int failed = 0;
                    failed += SettingCheck(Settings.awsAccessKeyId);
                    failed += SettingCheck(Settings.awsEndPoint);
                    failed += SettingCheck(Settings.awsSecretAccessKey);
                    failed += SettingCheck(Settings.comAddressApi);
                    failed += SettingCheck(Settings.comAddressBasic);
                    failed += SettingCheck(Settings.comOrganizationId);
                    failed += SettingCheck(Settings.comAddressPdfUpload);
                    failed += SettingCheck(Settings.fileLocationPdf);
                    failed += SettingCheck(Settings.fileLocationPicture);
                    failed += SettingCheck(Settings.firstRunDone);
                    failed += SettingCheck(Settings.knownSitesDone);
                    failed += SettingCheck(Settings.logLevel);
                    failed += SettingCheck(Settings.logLimit);
                    failed += SettingCheck(Settings.token);
                    failed += SettingCheck(Settings.unitLicenseNumber);
                    if (failed > 0)
                        return false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }
        #endregion

        #region public write log
        public override string      WriteLogEntry(LogEntry logEntry)
        {
            lock (_writeLock)
            {
                try
                {
                    using (var db = new MicrotingDb(connectionStr))
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

        private string              WriteLogExceptionEntry(LogEntry logEntry)
        {
                try
                {
                    using (var db = new MicrotingDb(connectionStr))
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

        public override void        WriteIfFailed(string logEntries)
        {
            lock (_writeLock)
            {
                try
                {
                    File.AppendAllText(@"log\\expection.txt",
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
        private int     EformCreateDb           (MainElement mainElement)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    GetConverter();

                    #region mainElement
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.label = mainElement.Label;
                    //description - used for non-MainElements
                    //serialized_default_values - Ruby colume
                    cl.workflow_state = "created";
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

                    db.version_check_lists.Add(MapCheckListVersions(cl));
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

        private void    CreateElementList      (int parentId, List<Element> lstElement)
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

        private void    CreateGroupElement     (int parentId, GroupElement groupElement)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.label = groupElement.Label;
                    cl.description = groupElement.Description.InderValue;
                    //serialized_default_values - Ruby colume
                    cl.workflow_state = "created";
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

                    db.version_check_lists.Add(MapCheckListVersions(cl));
                    db.SaveChanges();

                    CreateElementList(cl.id, groupElement.ElementList);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataElement failed", ex);
            }
        }

        private void    CreateDataElement      (int parentId, DataElement dataElement)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.label = dataElement.Label;
                    cl.description = dataElement.Description.InderValue;
                    //serialized_default_values - Ruby colume
                    cl.workflow_state = "created";
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

                    db.version_check_lists.Add(MapCheckListVersions(cl));
                    db.SaveChanges();

                    if (dataElement.DataItemGroupList != null)
                    {
                        foreach (DataItemGroup dataItemGroup in dataElement.DataItemGroupList)
                        {
                            CreateDataItemGroup(cl.id, (FieldGroup)dataItemGroup);
                        }
                    }

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

        private void    CreateDataItemGroup    (int elementId, FieldGroup fieldGroup)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    string typeStr = fieldGroup.GetType().ToString().Remove(0, 10); //10 = "eFormData.".Length
                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();
                    field.parent_field_id = null;
                    field.color = fieldGroup.Color;
                    field.description = fieldGroup.Description;
                    field.display_index = fieldGroup.DisplayOrder;
                    field.label = fieldGroup.Label;

                    field.created_at = DateTime.Now;
                    field.updated_at = DateTime.Now;
                    field.workflow_state = "created";
                    field.check_list_id = elementId;
                    field.field_type_id = fieldTypeId;
                    field.version = 1;

                    field.default_value = fieldGroup.Value;

                    db.fields.Add(field);
                    db.SaveChanges();

                    db.version_fields.Add(MapFieldVersions(field));
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

        private void    CreateDataItem         (int? parentFieldId, int elementId, DataItem dataItem)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    string typeStr = dataItem.GetType().ToString().Remove(0, 10); //10 = "eFormData.".Length
                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();
                    field.color = dataItem.Color;
                    field.parent_field_id = parentFieldId;
                    field.description = dataItem.Description.InderValue;
                    field.display_index = dataItem.DisplayOrder;
                    field.label = dataItem.Label;
                    field.mandatory = t.Bool(dataItem.Mandatory);
                    field.read_only = t.Bool(dataItem.ReadOnly);
                    field.dummy = t.Bool(dataItem.Dummy);

                    field.created_at = DateTime.Now;
                    field.updated_at = DateTime.Now;
                    field.workflow_state = "created";
                    field.check_list_id = elementId;
                    field.field_type_id = fieldTypeId;
                    field.version = 1;

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

                        default:
                            throw new IndexOutOfRangeException(dataItem.GetType().ToString() + " is not a known/mapped DataItem type");
                    }
                    #endregion

                    db.fields.Add(field);
                    db.SaveChanges();

                    db.version_fields.Add(MapFieldVersions(field));
                    db.SaveChanges();
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
                using (var db = new MicrotingDb(connectionStr))
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
                using (var db = new MicrotingDb(connectionStr))
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
                                long.Parse(f.min_value), long.Parse(f.max_value), int.Parse(f.default_value), t.Int(f.decimal_count), f.unit_name));
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

                            lstDataItemGroup.Add(new FieldGroup(f.id.ToString(), f.label, f.description, f.color, t.Int(f.display_index), f.default_value, lst));

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
                using (var db = new MicrotingDb(connectionStr))
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
        private void    EntityItemCreateUpdate(string entityGroupMUId, EntityItem entityItem)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
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

                                db.version_entity_items.Add(MapEntityItemVersions(match));
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

                            db.version_entity_items.Add(MapEntityItemVersions(match));
                            db.SaveChanges();

                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        #region new
                        entity_items eI = new entity_items();

                        eI.workflow_state = "created";
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

                        db.version_entity_items.Add(MapEntityItemVersions(eI));
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

        private void    EntityItemDelete(string entityGroupMUId, string entityItemUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_items match = db.entity_items.Single(x => x.entity_item_uid == entityItemUId && x.entity_group_id == entityGroupMUId);

                    match.synced = t.Bool(false);
                    match.updated_at = DateTime.Now;
                    match.version = match.version + 1;
                    match.workflow_state = "removed";

                    db.SaveChanges();

                    db.version_entity_items.Add(MapEntityItemVersions(match));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemUpdate failed", ex);
            }
        }
        #endregion

        #region help methods
        private string  Find(int fieldTypeId)
        {
            foreach (var holder in converter)
            {
                if (holder.Index == fieldTypeId)
                    return holder.FieldType;
            }
            throw new Exception("Find failed. Not known fieldType for fieldTypeId: " + fieldTypeId);
        }

        private int     Find(string typeStr)
        {
            foreach (var holder in converter)
            {
                if (holder.FieldType == typeStr)
                    return holder.Index;
            }
            throw new Exception("Find failed. Not known fieldTypeId for typeStr: " + typeStr);
        }

        private string  PairBuild(List<KeyValuePair> lst)
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

        private string  PairMatch(List<KeyValuePair> keyValuePairs, string match)
        {
            foreach (var item in keyValuePairs)
            {
                if (item.Key == match)
                    return item.Value;
            }

            return null;
        }

        private int     SettingCheck(Settings setting)
        {
            try
            {
                SettingRead(setting);
                return 0;
            }
            catch
            {    
                return 1;
            }
        }
        #endregion

        #region mappers
        private a_interaction_case_list_versions MapInteractionCaseListVersions(a_interaction_case_lists interactionCaseList)
        {
            a_interaction_case_list_versions ver = new a_interaction_case_list_versions();
            ver.case_id = interactionCaseList.case_id;
            ver.check_uid = interactionCaseList.check_uid;
            ver.created_at = interactionCaseList.created_at;
            ver.microting_uid = interactionCaseList.microting_uid;
            ver.siteId = interactionCaseList.siteId;
            ver.stat = interactionCaseList.stat;
            ver.version = interactionCaseList.version;
            ver.updated_at = interactionCaseList.updated_at;
            ver.version = interactionCaseList.version;
            ver.workflow_state = interactionCaseList.workflow_state;

            ver.a_interaction_case_list_version_id = interactionCaseList.id; //<<--

            return ver;
        }

        private a_interaction_case_versions MapInteractionCaseVersions(a_interaction_cases interactionCase)
        {
            a_interaction_case_versions ver = new a_interaction_case_versions();
            ver.case_uid = interactionCase.case_uid;
            ver.connected = interactionCase.connected;
            ver.created_at = interactionCase.created_at;
            ver.custom = interactionCase.custom;
            ver.expectionString = interactionCase.expectionString;
            ver.replacements = interactionCase.replacements;
            ver.synced = interactionCase.synced;
            ver.template_id = interactionCase.template_id;
            ver.updated_at = interactionCase.updated_at;
            ver.version = interactionCase.version;
            ver.workflow_state = interactionCase.workflow_state;

            ver.a_interaction_case_id = interactionCase.id; //<<--

            return ver;
        }

        private case_versions               MapCaseVersions(cases aCase)
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

            caseVer.case_id = aCase.id; //<<--

            return caseVer;
        }

        private check_list_versions         MapCheckListVersions(check_lists checkList)
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

            clv.check_list_id = checkList.id; //<<--

            return clv;
        }

        private check_list_value_versions   MapCheckListValueVersions(check_list_values checkListValue)
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

        private field_versions              MapFieldVersions(fields field)
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

        private field_value_versions        MapFieldValueVersions(field_values fieldValue)
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

        private uploaded_data_versions      MapUploadedDataVersions(uploaded_data uploadedData)
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

        private check_list_site_versions    MapCheckListSiteVersions(check_list_sites checkListSite)
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

        private entity_group_versions       MapEntityGroupVersions(entity_groups entityGroup)
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

        private entity_item_versions        MapEntityItemVersions(entity_items entityItem)
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

        private site_worker_versions        MapSiteWorkerVersions(site_workers site_workers)
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

        private site_versions               MapSiteVersions(sites site)
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

        private unit_versions               MapUnitVersions(units units)
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

        private worker_versions             MapWorkerVersions(workers workers)
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
        #endregion
        #endregion

        #region unit test
        public List<string>         UnitTest_FindAllActiveCases()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<cases> lstCases = db.cases.Where(x => x.workflow_state != "removed" && x.workflow_state != "retracted").ToList();
                    foreach (cases aCase in lstCases)
                    {
                        lstMUId.Add(aCase.microting_uid);
                    }

                    List<check_list_sites> lstCLS = db.check_list_sites.Where(x => x.workflow_state != "removed" && x.workflow_state != "retracted").ToList();
                    foreach (check_list_sites cLS in lstCLS)
                    {
                        lstMUId.Add(cLS.microting_uid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnitTest_FindAllActiveCases failed", ex);
            }
        }

        public List<string>         UnitTest_EntitiesFindAllActive()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<entity_groups> lstEGs = db.entity_groups.Where(x => x.workflow_state != "removed").ToList();
                    foreach (entity_groups eG in lstEGs)
                    {
                        lstMUId.Add(eG.microting_uid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnitTest_FindAllActiveEntities failed", ex);
            }
        }

        public bool                 UnitTest_EntitiesAllSynced(string entityGroupId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<entity_items> lstEGs = db.entity_items.Where(x => x.workflow_state == "failed to sync" && x.entity_group_id == entityGroupId).ToList();

                    if (lstEGs.Count > 0)
                        return false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnitTest_FindAllActiveEntities failed", ex);
            }
        }

        public List<string>         UnitTest_FindAllActiveInteraction()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<a_interaction_cases> lst = db.a_interaction_cases.Where(x => x.workflow_state == "created" || x.workflow_state == "creating" || x.workflow_state == "delete" || x.workflow_state == "deleting").ToList();
                    foreach (a_interaction_cases item in lst)
                    {
                        lstMUId.Add(item.id.ToString());
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnitTest_FindAllActiveEntities failed", ex);
            }
        }

        public List<notifications>  UnitTest_FindAllNotifications()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<notifications> lst = db.notifications.ToList();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public List<string>         UnitTest_FindAllActiveNotifications()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<notifications> lst = db.notifications.Where(x => x.workflow_state == "created").ToList();
                    foreach (notifications note in lst)
                    {
                        lstMUId.Add(note.microting_uid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnitTest_FindAllActiveEntities failed", ex);
            }
        }

        public List<a_interaction_case_lists> UnitTest_FindAllActiveInteractionCaseLists(int interactionCaseId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<a_interaction_case_lists> lst = db.a_interaction_case_lists.Where(x => x.a_interaction_case_id == interactionCaseId).ToList();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnitTest_FindAllActiveEntities failed", ex);
            }
        }

        public a_interaction_cases  UnitTest_FindInteractionCase(int interactionCaseId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    a_interaction_cases match = db.a_interaction_cases.SingleOrDefault(x => x.id == interactionCaseId);
                    return match;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnitTest_FindAllActiveEntities failed", ex);
            }
        }

        public bool                 UnitTest_TruncateTable(string tableName)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM [dbo].[" + tableName + "];");
                    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('" + tableName + "', RESEED, 1);");

                    return true;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }
        }

        public bool                 UnitTest_TruncateTablesIfEmpty()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> tableLst = new List<string>();

                    var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;

                    var tables = metadata.GetItemCollection(DataSpace.SSpace)
                      .GetItems<EntityContainer>()
                      .Single()
                      .BaseEntitySets
                      .OfType<EntitySet>()
                      .Where(s => !s.MetadataProperties.Contains("Type")
                        || s.MetadataProperties["Type"].ToString() == "Tables");

                    foreach (var table in tables)
                    {
                        var tableName = table.MetadataProperties.Contains("Table")
                            && table.MetadataProperties["Table"].Value != null
                          ? table.MetadataProperties["Table"].Value.ToString()
                          : table.Name;

                        var tableSchema = table.MetadataProperties["Schema"].Value.ToString();

                        tableLst.Add(tableName);
                    }

                    int count;
                    foreach (var tableName in tableLst)
                    {
                        count = db.Database.SqlQuery<int>("SELECT COUNT(*) FROM " + tableName).Single();

                        if (count == 0)
                            UnitTest_TruncateTable(tableName);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }
        }

        private void                FieldTypeAdd(int id, string fieldType, string description)
        {
            using (var db = new MicrotingDb(connectionStr))
            {
                field_types fT = new field_types();
                fT.id = id;
                fT.field_type = fieldType;
                fT.description = description;

                db.field_types.Add(fT);
                db.SaveChanges();
            }
        }
        #endregion
    }

    public enum Settings
    {
        firstRunDone,
        knownSitesDone,
        logLevel,
        logLimit,
        fileLocationPicture,
        fileLocationPdf,
        token,
        comAddressApi,
        comAddressBasic,
        comAddressPdfUpload,
        comOrganizationId,
        awsAccessKeyId,
        awsSecretAccessKey,
        awsEndPoint,
        unitLicenseNumber
    }
}