using eFormRequest;
using eFormResponse;
using Trools;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace eFormSqlController
{
    public class SqlController
    {
        #region var
        List<Holder> converter;
        object _lockQuery = new object();
        string connectionStr;
        Tools t = new Tools();
        #endregion

        #region con
        public SqlController(string connectionString)
        {
            connectionStr = connectionString;

            using (var db = new MicrotingDb(connectionStr))
            {
                #region SettingCount
                if (SettingCount() < 9)
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[setting]");

                    SettingAdd(1, "firstRunDone");
                    SettingAdd(2, "logLevel");
                    SettingAdd(3, "comToken");
                    SettingAdd(4, "comAddress");
                    SettingAdd(5, "organizationId");
                    SettingAdd(6, "subscriberToken");
                    SettingAdd(7, "subscriberAddress");
                    SettingAdd(8, "subscriberName");
                    SettingAdd(9, "fileLocation");

                    SettingUpdate("firstRunDone", "false");
                }
                #endregion

                #region FieldTypeCount
                if (FieldTypeCount() < 18)
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[field_types]");

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
                }
                #endregion

                #region SettingRead
                if (!bool.Parse(SettingRead("firstRunDone")))
                {
                    var lines = File.ReadAllLines("input\\first_run.txt");
                    string name;
                    string value;

                    foreach (var item in lines)
                    {
                        string[] line = item.Split('|');

                        name = line[0];
                        value = line[1];

                        SettingUpdate(name, value);
                    }
                    SettingUpdate("firstRunDone", "true");
                    SettingUpdate("logLevel", "true");
                }
                #endregion
            }
        }
        #endregion

        #region public
        #region templat
        public int                  TemplatCreate(MainElement mainElement)
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

        public MainElement          TemplatRead(int templatId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    MainElement mainElement = null;
                    GetConverter();

                    check_lists mainCl = null;
                
                    //getting mainElement
                    mainCl = db.check_lists.Single(x => x.id == templatId);

                    mainElement = new MainElement(mainCl.id, mainCl.label, t.Int(mainCl.display_index), mainCl.folder_name, t.Int(mainCl.repeated), DateTime.Now, DateTime.Now.AddDays(2), "da",
                        t.Bool(mainCl.multi_approval), t.Bool(mainCl.fast_navigation), t.Bool(mainCl.download_entities), t.Bool(mainCl.manual_sync), mainCl.case_type, "", "", new List<Element>());

                    //getting elements
                    List<check_lists> lst = db.check_lists.Where(x => x.parent_id == templatId).ToList();
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

        public List<MainElement>    TemplatReadAll()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<check_lists> matches = null;
                    List<MainElement> rtrnLst = new List<MainElement>();

                    matches = db.check_lists.Where(x => x.parent_id == 0).ToList();

                    foreach (check_lists mainCl in matches)
                    {
                        MainElement mainElement = new MainElement(mainCl.id, mainCl.label, t.Int(mainCl.display_index), mainCl.folder_name, t.Int(mainCl.repeated), DateTime.Now, DateTime.Now.AddDays(2), "da",
                        t.Bool(mainCl.multi_approval), t.Bool(mainCl.fast_navigation), t.Bool(mainCl.download_entities), t.Bool(mainCl.manual_sync), mainCl.case_type, "", "", new List<Element>());

                        rtrnLst.Add(mainElement);
                    }

                    return rtrnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TemplatGetAll failed", ex);
            }
        }
        #endregion

        #region case
        public void                 CheckListSitesCreate(int checkListId, int siteId, string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
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

        public int                  CaseCreate(int checkListId, int siteId, string microtingUId, string caseUId, string custom)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists chkLst = db.check_lists.Single(x => x.id == checkListId);

                    cases aCase = new cases();
                    aCase.status = 66;
                    aCase.type = chkLst.case_type;
                    aCase.created_at = DateTime.Now;
                    aCase.updated_at = DateTime.Now;
                    aCase.check_list_id = checkListId;
                    aCase.microting_uid = microtingUId;
                    aCase.case_uid = caseUId;
                    aCase.workflow_state = "created";
                    aCase.version = 1;
                    aCase.site_id = siteId;

                    aCase.custom = custom;

                    db.cases.Add(aCase);
                    db.SaveChanges();

                    db.version_cases.Add(MapCaseVersions(aCase));
                    db.SaveChanges();

                    return aCase.id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseCreate failed", ex);
            }
        }

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
                    } catch { }

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

                        Case_Dto rtrnCase = new Case_Dto(stat, (int)cls.site_id, cL.case_type, "ReversedCase", cls.microting_uid, cls.last_check_id, cL.custom);
                        return rtrnCase;
                    } catch { }

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
                    if (aCase.workflow_state == "created" && aCase.unit_id == null)
                        stat = "Created";

                    if (aCase.workflow_state == "created" && aCase.unit_id != null) //TODO
                        stat = "Retrived";

                    if (aCase.workflow_state == "retracted")
                        stat = "Completed";

                    if (aCase.workflow_state == "removed")
                        stat = "Deleted";
                    #endregion

                    Case_Dto cDto = new Case_Dto(stat, (int)aCase.site_id, cL.case_type, aCase.case_uid, aCase.microting_uid, aCase.microting_check_uid, cL.custom);
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

        public string               CaseReadCheckIdByMUId(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_list_sites match = db.check_list_sites.SingleOrDefault(x => x.microting_uid == microtingUId);
                    return match.last_check_id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByMuuId failed", ex);
            }
        }

        public cases                CaseReadFull(string microtingUId, string checkUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases match = db.cases.SingleOrDefault(x => x.microting_uid == microtingUId && x.microting_check_uid == checkUId);
                    return match;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        public List<cases>          CaseReadAllIds(int templatId, DateTime? start, DateTime? end)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (start == null)
                        start = DateTime.MinValue;

                    if (end == null)
                        end = DateTime.MaxValue;

                    List<cases> matches = db.cases.Where(x => x.check_list_id == templatId && x.done_at > start && x.done_at < end).ToList();
                    return matches;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        public void                 CaseUpdate(string microtingUId, DateTime doneAt, int doneByUserId, string microtingCheckId, int unitId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases caseStd = db.cases.Single(x => x.microting_uid == microtingUId && x.microting_check_uid == null);

                    caseStd.status = 100;
                    caseStd.done_at = doneAt;
                    caseStd.updated_at = DateTime.Now;
                    caseStd.done_by_user_id = doneByUserId;
                    caseStd.workflow_state = "created";
                    caseStd.version = caseStd.version + 1;
                    caseStd.unit_id = unitId;
                    #region caseStd.microting_check_id = microtingCheckId; and update "check_list_sites" if needed
                    if (microtingCheckId != null)
                    {
                        check_list_sites match = db.check_list_sites.Single(x => x.microting_uid == microtingUId);
                        match.last_check_id = microtingCheckId;
                        match.version = match.version + 1;
                        match.updated_at = DateTime.Now;

                        db.SaveChanges();

                        db.version_check_list_sites.Add(MapCheckListSiteVersions(match));
                        db.SaveChanges();
                    }

                    caseStd.microting_check_uid = microtingCheckId;
                    #endregion

                    db.version_cases.Add(MapCaseVersions(caseStd));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdate failed", ex);
            }
        }

        public void                 CaseUpdate(string microtingUId, string checkUId, string newCaseUId, string newCustom)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases caseStd = db.cases.Single(x => x.microting_uid == microtingUId && x.microting_check_uid == checkUId);

                    caseStd.case_uid = newCaseUId;
                    caseStd.custom = newCustom;
                    caseStd.updated_at = DateTime.Now;
                    caseStd.version = caseStd.version + 1;
                    
                    db.version_cases.Add(MapCaseVersions(caseStd));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdate failed", ex);
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

        public int                  CaseCountResponses(string caseUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    return db.cases.Count(x => x.case_uid == caseUId && x.done_by_user_id != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseCountResponses failed", ex);
            }
        }

        public void                 CaseRetract(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //checks if infinity case
                    if (db.check_list_sites.Count(x => x.microting_uid == microtingUId) > 0)
                        return;

                    cases aCase = db.cases.Single(x => x.microting_uid == microtingUId);

                    aCase.updated_at = DateTime.Now;
                    aCase.workflow_state = "retracted";
                    aCase.version = aCase.version + 1;
        
                    db.version_cases.Add(MapCaseVersions(aCase));
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
                    cases aCase = db.cases.Single(x => x.microting_uid == microtingUId);

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

        #region check
        public void                 ChecksCreate(Response response, string xmlString)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    int elementId;
                    int userId = int.Parse(response.Checks[0].WorkerId);

                    List<string> elements = t.LocateList(xmlString, "<ElementList>", "</ElementList>");

                    int caseId = -1;

                    try
                    {
                        check_list_sites cLS = db.check_list_sites.Single(x => x.microting_uid == response.Value);
                        caseId = CaseCreate((int)cLS.check_list_id, (int)cLS.site_id, cLS.microting_uid, "ReversedCase", "");
                    }
                    catch { }

                    foreach (string elementStr in elements)
                    {
                        #region foreach element
                        if (caseId == -1)
                        {
                            try
                            {
                                cases c = db.cases.Single(x => x.microting_uid == response.Value);
                                caseId = c.id;
                            }
                            catch { }
                        }

                        if (caseId == -1)
                            throw new Exception("EformCheckCreateDb failed. Due to unable to find a matching case");


                        check_list_values clv = new check_list_values();

                        clv.created_at = DateTime.Now;
                        clv.updated_at = DateTime.Now;
                        clv.check_list_id = int.Parse(t.Locate(elementStr, "<Id>", "</"));
                        clv.case_id = caseId;
                        clv.status = t.Locate(elementStr, "<Status>", "</");
                        clv.version = 1;
                        clv.user_id = userId;
                        clv.workflow_state = "created";

                        db.check_list_values.Add(clv);
                        db.SaveChanges();

                        db.version_check_list_values.Add(MapCheckListValueVersions(clv));
                        db.SaveChanges();

                        #region foreach dataItem
                        elementId = clv.id;
                        List<string> dataItems = t.LocateList(elementStr, "<DataItem>", "</DataItem>");

                        if (dataItems != null)
                        {
                            foreach (string dataItemStr in dataItems)
                            {
                                field_values fieldV = new field_values();

                                #region if contains a file
                                string urlXml = t.Locate(xmlString, "<URL>", "</URL>");
                                if (urlXml != "" && urlXml != "none")
                                {
                                    data_uploaded dU = new data_uploaded();

                                    dU.created_at = DateTime.Now;
                                    dU.updated_at = DateTime.Now;
                                    dU.extension = t.Locate(xmlString, "<Extension>", "</");
                                    dU.uploader_id = userId;
                                    dU.uploader_type = "system";
                                    dU.workflow_state = "pre_created";
                                    dU.version = 1;
                                    dU.local = 0;
                                    dU.file_location = t.Locate(xmlString, "<URL>", "</");

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
                                string temp = t.Locate(xmlString, "<Value>", "</");

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
                                fieldV.latitude = t.Locate(xmlString, " < Latitude>", "</");
                                fieldV.longitude = t.Locate(xmlString, "<Longitude>", "</");
                                fieldV.altitude = t.Locate(xmlString, "<Altitude>", "</");
                                fieldV.heading = t.Locate(xmlString, "<Heading>", "</");
                                fieldV.accuracy = t.Locate(xmlString, "<Accuracy>", "</");
                                fieldV.date = t.Date(t.Locate(xmlString, "<Date>", "</"));
                                //
                                fieldV.workflow_state = "created";
                                fieldV.version = 1;
                                fieldV.case_id = caseId;
                                fieldV.field_id = int.Parse(t.Locate(xmlString, "<Id>", "</"));
                                fieldV.user_id = userId;
                                fieldV.check_list_id = elementId;
                                fieldV.done_at = t.Date(response.Checks[0].Date);

                                db.field_values.Add(fieldV);
                                db.SaveChanges();

                                db.version_field_values.Add(MapFieldValueVersions(fieldV));
                                db.SaveChanges();
                            }
                        }
                        #endregion

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckCreateDb failed", ex);
            }
        }

        public List<field_values>   ChecksRead(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    var aCase = db.cases.Single(x => x.microting_uid == microtingUId);
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

        public FieldValue           FieldValueRead(int id)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    field_values reply = db.field_values.Single(x => x.id == id);
                    fields question = db.fields.Single(x => x.id == reply.field_id);

                    FieldValue answer = new FieldValue();
                    answer.Accuracy = reply.accuracy;
                    answer.Altitude = reply.altitude;
                    answer.Color = question.color;
                    answer.Date = reply.date;
                    answer.FieldId = reply.field_id.ToString();
                    answer.FieldType = Find((int)question.field_type_id);
                    answer.DateOfDoing = t.Date(reply.done_at);
                    answer.Description = new eFormRequest.CDataValue();
                    answer.Description.InderValue = question.description;
                    answer.DisplayOrder = t.Int(question.display_index);
                    answer.Heading = reply.heading;
                    answer.Id = reply.id.ToString();
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
                            data_uploaded uploadedData;
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
                    #endregion
                    answer.Value = reply.value;
                    #region answer.ValueReadable = reply.value 'ish' //and if needed: answer.KeyValuePairList = ReadPairs(...);

                    if (answer.FieldType == "EntitySearch" || answer.FieldType == "EntitySelect")
                    {
                        entity_items match = db.entity_items.SingleOrDefault(x => x.microting_uid == reply.value);

                        if (match != null)
                            answer.ValueReadable = match.name;
                    }

                    if (answer.FieldType == "SingleSelect")
                    {
                        string key = answer.Value;
                        string fullKey = t.Locate(question.key_value_pair_list, "<" + key + ">", "</" + key + ">");
                        answer.ValueReadable = t.Locate(fullKey, "<key>", "</key>");

                        answer.KeyValuePairList = ReadPairs(question.key_value_pair_list);
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

                        answer.KeyValuePairList = ReadPairs(question.key_value_pair_list);
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

        public List<string>         FieldValueReadAllValues(int fieldId, DateTime? start, DateTime? end)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (start == null)
                        start = DateTime.MinValue;

                    if (end == null)
                        end = DateTime.MaxValue;

                    List<field_values> matches = db.field_values.Where(x => x.field_id == fieldId && x.done_at > start && x.done_at < end).ToList();
                    List<string> rtrnLst = new List<string>();

                    foreach (field_values item in matches)
                        rtrnLst.Add(item.value);

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
        public void                 NotificationCreate(string microtingUId, string transmission)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (db.notifications.Count(x => x.transmission == transmission) == 0)
                    {
                        notifications aNoti = new notifications();

                        aNoti.microting_uid = microtingUId;
                        aNoti.transmission = transmission;
                        aNoti.workflow_state = "created";
                        aNoti.created_at = DateTime.Now;
                        aNoti.updated_at = DateTime.Now;

                        db.notifications.Add(aNoti);
                        db.SaveChanges();
                    }

                    //TODO else log warning
                }
            }
            catch (Exception ex)
            {
                throw new Exception("NotificationRead failed", ex);
            }
        }

        public string               NotificationRead()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    notifications aNoti = db.notifications.FirstOrDefault(x => x.workflow_state == "created");
                        
                    if (aNoti != null)
                        return aNoti.transmission;
                    else
                        return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("NotificationRead failed", ex);
            }
        }

        public void                 NotificationProcessed(string transmission, string workflowState)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    notifications aNoti = db.notifications.Single(x => x.transmission == transmission);
                    aNoti.workflow_state = workflowState;
                    aNoti.updated_at = DateTime.Now;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("NotificationProcessed failed", ex);
            }
        }
        #endregion

        #region file
        public string               FileRead()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    data_uploaded dU = db.data_uploaded.SingleOrDefault(x => x.workflow_state == "pre_created");
                        
                    if (dU != null)
                        return dU.file_location;
                    else
                        return "";
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
                        data_uploaded dU = db.data_uploaded.Single(x => x.file_location == urlString);
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

        public void                 FileProcessed(string urlString, string chechSum, string fileLocation, string fileName)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    data_uploaded uD = db.data_uploaded.Single(x => x.file_location == urlString);

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

        #region entityGroup
        public int          EntityGroupCreate(string name, string entityType)
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

                    return eG.id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupCreate failed", ex);
            }
        }

        public EntityGroup  EntityGroupRead(string entityGroupMUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_groups eG = db.entity_groups.SingleOrDefault(x => x.microting_uid == entityGroupMUId);

                    if (eG == null)
                        return null;

                    List<EntityItem> lst = new List<EntityItem>();
                    EntityGroup rtnEG = new EntityGroup(eG.name, eG.type, eG.microting_uid, lst);

                    List<entity_items> eILst = db.entity_items.Where(x => x.entity_group_id == eG.microting_uid && x.workflow_state != "removed" && x.workflow_state != "failed_to_sync").ToList();

                    if (eILst.Count > 0)
                        foreach (entity_items item in eILst)
                        {
                            EntityItem eI = new EntityItem(item.name, item.description, item.entity_item_uid);
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

        public bool         EntityGroupUpdate(int entityGroupId, string entityGroupMUId)
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

        public void         EntityGroupUpdateItems(EntityGroup entityGroup)
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

        public string       EntityGroupDelete(string entityGroupMUId)
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

        #region entityItem sync
        public entity_items     EntityItemSyncedRead()
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

        public void             EntityItemSyncedProcessed(string entityGroupMUId, string entityItemId, string microting_uid, string workflowState)
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

        #region settings
        public string   SettingRead(string variableName)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    setting match = db.setting.Single(x => x.name == variableName);
                    return match.value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SettingGet failed.", ex);
            }
        }

        public void     SettingUpdate(string variableName, string variableValue)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    setting match = db.setting.Single(x => x.name == variableName);
                    match.value = variableValue;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SettingUpdate failed.", ex);
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
                    cl.parent_id = 0; //MainElements never have parents ;)
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
                        foreach (eFormRequest.DataItem dataItem in dataElement.DataItemList)
                        {
                            CreateDataItem(cl.id, dataItem);
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
                    string typeStr = fieldGroup.GetType().ToString().Remove(0, 13); //13 = "eFormRequest.".Length
                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();

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
                        foreach (eFormRequest.DataItem dataItem in fieldGroup.DataItemList)
                        {
                            CreateDataItem(field.id, dataItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataItemGroup failed", ex);
            }
        }

        private void    CreateDataItem         (int elementId, eFormRequest.DataItem dataItem)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    string typeStr = dataItem.GetType().ToString().Remove(0, 13); //13 = "eFormRequest.".Length
                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();
                    field.color = dataItem.Color;
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
                    switch (typeStr) //"eFormRequest.".Length
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
                            field.min_value = date.MinValue.ToString("yyyy-MM-dd HH:mm:ss");
                            field.max_value = date.MaxValue.ToString("yyyy-MM-dd HH:mm:ss");
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
                            field.key_value_pair_list = WritePairs(multiSelect.KeyValuePairList);
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
                            field.default_value = showPdf.DefaultValue;
                            break;

                        case "Signature":
                            break;

                        case "SingleSelect":
                            SingleSelect singleSelect = (SingleSelect)dataItem;
                            field.key_value_pair_list = WritePairs(singleSelect.KeyValuePairList);
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
                                t.Bool(cl.done_button_enabled), t.Bool(cl.extra_fields_enabled), "", new List<DataItemGroup>(), new List<eFormRequest.DataItem>());

                            //the actual DataItems
                            List<fields> lstFields = db.fields.Where(x => x.check_list_id == elementId).ToList();
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

        private void GetDataItem(List<eFormRequest.DataItem> lstDataItem, List<DataItemGroup> lstDataItemGroup, int dataItemId)
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
                            lstDataItem.Add(new Audio(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.multi)));
                            break;

                        case "CheckBox":
                            lstDataItem.Add(new CheckBox(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Bool(f.default_value), t.Bool(f.selected)));
                            break;

                        case "Comment":
                            lstDataItem.Add(new Comment(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.default_value, t.Int(f.max_length), t.Bool(f.split_screen)));
                            break;

                        case "Date":
                            lstDataItem.Add(new Date(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                DateTime.Parse(f.min_value), DateTime.Parse(f.max_value), f.default_value));
                            break;

                        case "None":
                            lstDataItem.Add(new None(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy)));
                            break;

                        case "Number":
                            lstDataItem.Add(new Number(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                long.Parse(f.min_value), long.Parse(f.max_value), int.Parse(f.default_value), t.Int(f.decimal_count), f.unit_name));
                            break;

                        case "MultiSelect":
                            lstDataItem.Add(new MultiSelect(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                ReadPairs(f.key_value_pair_list)));
                            break;

                        case "Picture":
                            lstDataItem.Add(new Picture(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.multi), t.Bool(f.geolocation_enabled)));
                            break;

                        case "SaveButton":
                            lstDataItem.Add(new SaveButton(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.default_value));
                            break;

                        case "ShowPdf":
                            lstDataItem.Add(new ShowPdf(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                f.default_value));
                            break;

                        case "Signature":
                            lstDataItem.Add(new Signature(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy)));
                            break;

                        case "SingleSelect":
                            lstDataItem.Add(new SingleSelect(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                ReadPairs(f.key_value_pair_list)));
                            break;

                        case "Text":
                            lstDataItem.Add(new Text(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy), 
                                f.default_value, t.Int(f.max_length), t.Bool(f.geolocation_enabled), t.Bool(f.geolocation_forced), t.Bool(f.geolocation_hidden), t.Bool(f.barcode_enabled), f.barcode_type));
                            break;

                        case "Timer":
                            lstDataItem.Add(new Timer(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Bool(f.stop_on_save)));
                            break;
                            
                        case "EntitySearch":
                            lstDataItem.Add(new EntitySearch(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.default_value), t.Int(f.entity_group_id), t.Bool(f.is_num), f.query_type, t.Int(f.min_value), t.Bool(f.barcode_enabled), f.barcode_type));
                            break;

                        case "EntitySelect":
                            lstDataItem.Add(new EntitySelect(f.id.ToString(), t.Bool(f.mandatory), t.Bool(f.read_only), f.label, f.description, f.color, t.Int(f.display_index), t.Bool(f.dummy),
                                t.Int(f.default_value), t.Int(f.entity_group_id)));
                            break;

                        case "FieldGroup":
                            List<eFormRequest.DataItem> lst = new List<eFormRequest.DataItem>();

                            lstDataItemGroup.Add(new FieldGroup(f.id.ToString(), f.label, f.description, f.color, t.Int(f.display_index), f.default_value, lst));

                            //the actual DataItems
                            List<fields> lstFields = db.fields.Where(x => x.check_list_id == f.id).ToList();
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

        #region public help methods
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

        private string WritePairs(List<KeyValuePair> lst)
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

        private List<KeyValuePair> ReadPairs(string str)
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
        #endregion

        #region mappers
        private version_cases               MapCaseVersions(cases aCase)
        {
            version_cases caseVer = new version_cases();
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

        private version_check_lists         MapCheckListVersions(check_lists checkList)
        {
            version_check_lists clv = new version_check_lists();
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

        private version_check_list_values   MapCheckListValueVersions(check_list_values checkListValue)
        {
            version_check_list_values clvv = new version_check_list_values();
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

        private version_fields              MapFieldVersions(fields field)
        {
            version_fields fv = new version_fields();

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

        private version_field_values        MapFieldValueVersions(field_values fieldValue)
        {
            version_field_values fvv = new version_field_values();

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

        private version_data_uploaded       MapUploadedDataVersions(data_uploaded uploadedData)
        {
            version_data_uploaded udv = new version_data_uploaded();

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

        private version_check_list_sites    MapCheckListSiteVersions(check_list_sites checkListSite)
        {
            version_check_list_sites checkListSiteVer = new version_check_list_sites();
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

        private version_entity_groups       MapEntityGroupVersions(entity_groups entityGroup)
        {
            version_entity_groups entityGroupVer = new version_entity_groups();
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

        private version_entity_items        MapEntityItemVersions(entity_items entityItem)
        {
            version_entity_items entityItemVer = new version_entity_items();
            entityItemVer.workflow_state = entityItem.workflow_state;
            entityItemVer.version = entityItem.version;
            entityItemVer.created_at = entityItem.created_at;
            entityItemVer.updated_at = entityItem.updated_at;
            entityItemVer.entity_item_uid = entityItem.entity_item_uid;
            entityItemVer.microting_uid = entityItem.microting_uid;
            entityItemVer.name = entityItem.name;
            entityItemVer.description = entityItem.description;
            entityItemVer.synced = entityItem.synced;

            entityItemVer.entity_items_id = entityItem.id; //<<--

            return entityItemVer;
        }
        #endregion
        #endregion

        #region unit test
        public List<string> UnitTest_CheckCase()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lstMUId = new List<string>();

                    List<cases> lstCases = db.cases.Where(x => x.workflow_state == "created").ToList();
                    foreach (cases aCase in lstCases)
                    {
                        lstMUId.Add(aCase.microting_uid);
                    }

                    List<check_list_sites> lstCLS = db.check_list_sites.Where(x => x.workflow_state == "created").ToList();
                    foreach (check_list_sites cLS in lstCLS)
                    {
                        lstMUId.Add(cLS.microting_uid);
                    }

                    return lstMUId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckCase failed", ex);
            }
        }

        public List<string> UnitTest_FindAllActiveCases()
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
                throw new Exception("FindAllActiveCases failed", ex);
            }
        }

        public List<string> UnitTest_FindAllActiveEntities()
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
                throw new Exception("FindAllActiveCases failed", ex);
            }
        }

        public bool     UnitTest_CleanAndResetDB()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[cases]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_cases]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[check_list_sites]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_check_list_sites]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[check_list_values]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_check_list_values]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[check_lists]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_check_lists]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[entity_groups]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_entity_groups]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[entity_items]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_entity_items]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[field_values]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_field_values]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[fields]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_fields]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[notifications]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[outlook]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[sites]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_sites]");
                    //---
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[data_uploaded]");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[version_data_uploaded]");
                    //---
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[setting]");
                    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[field_types]");
                    //---

                    return true;
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.ToString();
                return false;
            }
        }

        private int     FieldTypeCount()
        {
            using (var db = new MicrotingDb(connectionStr))
            {
                return db.field_types.Count();
            }
        }

        private void    FieldTypeAdd(int id, string fieldType, string description)
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

        private int     SettingCount()
        {
            using (var db = new MicrotingDb(connectionStr))
            {
                return db.setting.Count();
            }
        }

        private void    SettingAdd(int id, string name)
        {
            using (var db = new MicrotingDb(connectionStr))
            {
                setting set = new setting();
                set.id = id;
                set.name = name;

                db.setting.Add(set);
                db.SaveChanges();
            }
        }
        #endregion
    }
}