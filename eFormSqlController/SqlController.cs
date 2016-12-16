using eFormRequest;
using eFormResponse;
using Trools;

using System;
using System.Collections.Generic;
using System.Linq;

namespace eFormSqlController
{
    public class SqlController
    {
        #region var
        List<Holder> converter;
        object _lockQuery = new object();
        string connectionStr;
        int userId;
        Tools t = new Tools();
        #endregion

        #region con
        public SqlController(string connectionString, int userId)
        {
            connectionStr = connectionString;
            this.userId = userId;
        }
        #endregion

        #region public
        #region templat
        public int          TemplatCreate(MainElement mainElement)
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

        public MainElement  TemplatRead(int templatId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    MainElement mainElement = null;
                    GetConverter();

                    check_lists mainCl = null;
                    try
                    {
                        //getting mainElement
                        mainCl = db.check_lists.Where(x => x.id == templatId).ToList().First();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to find matching check_list:" + templatId, ex);
                    }

                    mainElement = new MainElement(mainCl.id, mainCl.text, Int(mainCl.display_index), mainCl.folder_name, Int(mainCl.repeated), DateTime.Now, DateTime.Now.AddDays(2), "da",
                        Bool(mainCl.multi_approval), Bool(mainCl.fast_navigation), Bool(mainCl.download_entities), Bool(mainCl.manual_sync), mainCl.case_type, "", "", new List<Element>());


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
        #endregion

        #region case
        public int                  CaseCreate(string microtingUId, int checkListId, int siteId, string caseUId, DateTime navision_time, string reg_number, string vejenummer)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists chkLst = db.check_lists.Where(x => x.id == checkListId).ToList().First();

                    cases aCase = new cases();
                    aCase.status = 66;
                    aCase.created_by_user_id = userId;
                    aCase.type = chkLst.case_type;
                    aCase.created_at = DateTime.Now;
                    aCase.updated_at = DateTime.Now;
                    aCase.check_list_id = checkListId;
                    aCase.microting_api_id = microtingUId;
                    aCase.serialized_values = caseUId;
                    aCase.workflow_state = "created";
                    aCase.version = 1;
                    aCase.site_id = siteId;

                    aCase.navision_time = navision_time;
                    aCase.reg_number = reg_number;
                    aCase.vejenummer = vejenummer;

                    db.cases.Add(aCase);
                    db.SaveChanges();

                    db.case_versions.Add(MapCaseVersions(aCase));
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
                        check_list_sites match = db.check_list_sites.Where(x => x.microting_check_list_uuid == microtingUId).ToList().First();
                        check_lists cL = db.check_lists.Where(x => x.id == match.check_list_id).ToList().First();
                        Case_Dto cDto = new Case_Dto((int)match.site_id, cL.case_type, "ReversedCase", match.microting_check_list_uuid, match.last_check_id);
                        return cDto;
                    }
                    catch { }

                    try
                    {
                        cases aCase = db.cases.Where(x => x.microting_api_id == microtingUId).ToList().First();
                        check_lists cL = db.check_lists.Where(x => x.id == aCase.check_list_id).ToList().First();
                        Case_Dto cDto = new Case_Dto((int)aCase.site_id, cL.case_type, aCase.serialized_values, microtingUId, null);
                        return cDto;
                    }
                    catch { }

                    throw new Exception("CaseReadByMUId failed. Was unable to find a match in the database.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadByMuuId failed", ex);
            }
        }

        public List<Case_Dto>       CaseReadByCaseUId(string caseUId)
        {
            try
            {
                if (caseUId == "ReversedCase")
                    throw new Exception("CaseReadByCaseUId failed. Due invalid input:'ReversedCase'. This would return incorrect data");

                using (var db = new MicrotingDb(connectionStr))
                {
                    List<cases> matchs = db.cases.Where(x => x.serialized_values == caseUId).ToList();
                    List<Case_Dto> lstDto = new List<Case_Dto>();

                    foreach (cases aCase in matchs)
                    {
                        lstDto.Add(CaseReadByMUId(aCase.microting_api_id));
                    }
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
                    try
                    {
                        check_list_sites match = db.check_list_sites.Where(x => x.microting_check_list_uuid == microtingUId).ToList().First();
                        return match.last_check_id;
                    }
                    catch
                    {
                        return null;
                    }
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
                    try
                    {
                        cases match = db.cases.Where(x => x.microting_api_id == microtingUId && x.microting_check_id == checkUId).ToList().First();
                        return match;
                    }
                    catch
                    {
                        //no match found
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseReadFull failed", ex);
            }
        }

        public void                 CaseUpdate(string microtingUId, DateTime dateOfDoing, int doneByUserId, string microtingCheckId, int unitId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases caseStd = db.cases.Where(x => x.microting_api_id == microtingUId && x.microting_check_id == null).ToList().First();

                    caseStd.status = 100;
                    caseStd.date_of_doing = dateOfDoing;
                    caseStd.updated_at = DateTime.Now;
                    caseStd.done_by_user_id = doneByUserId;
                    caseStd.workflow_state = "created";
                    caseStd.version = caseStd.version + 1;
                    caseStd.unit_id = unitId;
                    #region caseStd.microting_check_id = microtingCheckId; and update "check_list_sites" if needed
                    if (microtingCheckId != null)
                    {
                        check_list_sites match = db.check_list_sites.Where(x => x.microting_check_list_uuid == microtingUId).ToList().First();
                        match.last_check_id = microtingCheckId;
                        match.version = match.version + 1;
                        match.updated_at = DateTime.Now;

                        db.SaveChanges();

                        db.check_list_site_versions.Add(MapCheckListSiteVersions(match));
                        db.SaveChanges();
                    }

                    caseStd.microting_check_id = microtingCheckId;
                    #endregion
                    
                    db.case_versions.Add(MapCaseVersions(caseStd));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdate failed", ex);
            }
        }

        public List<Case_Dto>       CaseFindMatchs(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    try
                    {
                        check_list_sites lookedUp = db.check_list_sites.Where(x => x.microting_check_list_uuid == microtingUId).ToList().First();
                        List<Case_Dto> foundCasesThatMatch = new List<Case_Dto>();
                        foundCasesThatMatch.Add(CaseReadByMUId(lookedUp.microting_check_list_uuid));

                        return foundCasesThatMatch;
                    }
                    catch
                    {
                        try
                        {
                            cases lookedUp = db.cases.Where(x => x.microting_api_id == microtingUId).ToList().First();
                            List<Case_Dto> foundCasesThatMatch = new List<Case_Dto>();

                            List<cases> lstMatchs = db.cases.Where(x => x.serialized_values == lookedUp.serialized_values).ToList();

                            foreach (cases match in lstMatchs)
                            {
                                foundCasesThatMatch.Add(CaseReadByMUId(match.microting_api_id));
                            }
                            return foundCasesThatMatch;
                        }
                        catch { }
                    }

                    List<Case_Dto> emptyCaseMatchs = new List<Case_Dto>();
                    return emptyCaseMatchs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseFindMatchs failed", ex);
            }
        }

        public string               CaseFindActive(string caseUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    if (caseUId == "ReversedCase")
                        throw new Exception("CaseFindActive failed. Due invalid input:'ReversedCase'. This would return incorrect data");

                    try
                    {
                        cases match = db.cases.Where(x => x.serialized_values == caseUId && x.workflow_state == "created").ToList().First();
                        return match.microting_api_id;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseFindMatchs failed", ex);
            }
        }

        public void                 CaseRetract(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases aCase = db.cases.Where(x => x.microting_api_id == microtingUId).ToList().First();

                    aCase.updated_at = DateTime.Now;
                    aCase.workflow_state = "retracted";
                    aCase.version = aCase.version + 1;
        
                    db.case_versions.Add(MapCaseVersions(aCase));
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
                    cases aCase = db.cases.Where(x => x.microting_api_id == microtingUId).ToList().First();

                    aCase.updated_at = DateTime.Now;
                    aCase.workflow_state = "removed";
                    aCase.version = aCase.version + 1;
                    aCase.removed_by_user_id = userId;

                    db.case_versions.Add(MapCaseVersions(aCase));
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
                    check_list_sites site = db.check_list_sites.Where(x => x.microting_check_list_uuid == microtingUId).ToList().First();

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

        #region check
        public void                 ChecksCreate(Response response, string xmlString)
        {
            EformCheckCreateDb(response, xmlString);
        }

        public List<field_values>   ChecksRead(string microtingUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    var aCase = db.cases.Where(x => x.microting_api_id == microtingUId).ToList().First();
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

        public Answer               ChecksReadAnswer(int id)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    field_values reply = db.field_values.Where(x => x.id == id).ToList().First();
                    fields question = db.fields.Where(x => x.id == reply.field_id).ToList().First();
                    
                    Answer answer = new Answer();
                    answer.Accuracy = reply.accuracy;
                    answer.Altitude = reply.altitude;
                    answer.Color = question.color;
                    answer.Date = reply.date;
                    answer.fieldType = Find((int)question.field_type_id);
                    answer.DateOfDoing = Date(reply.date_of_doing);
                    answer.Description = new eFormRequest.CDataValue();
                    answer.Description.InderValue = question.description;
                    answer.DisplayOrder = Int(question.display_index);
                    answer.Heading = reply.heading;
                    answer.Id = question.id.ToString();
                    answer.Label = question.text;
                    answer.Latitude = reply.latitude;
                    answer.Longitude = reply.longitude;
                    answer.Mandatory = Bool(question.mandatory);
                    answer.ReadOnly = Bool(question.read_only);
                    #region answer.UploadedDataId = reply.uploaded_data_id;
                    if (reply.uploaded_data_id.HasValue)
                        if (reply.uploaded_data_id > 0)
                        {
                            string locations = "";
                            int uploadedDataId;
                            uploaded_data uploadedData;
                            List<field_values> lst = db.field_values.Where(x => x.case_id == reply.case_id && x.field_id == reply.field_id).ToList();

                            foreach (field_values fV in lst)
                            {
                                uploadedDataId = (int)fV.uploaded_data_id;

                                uploadedData = db.uploaded_data.Where(x => x.id == uploadedDataId).ToList().First();

                                if (uploadedData.file_name != null)
                                    locations += uploadedData.file_location + uploadedData.file_name + Environment.NewLine;
                                else
                                    locations += "File attached, awaiting download" + Environment.NewLine;
                            }

                            answer.UploadedData = locations.TrimEnd();
                        }
                    #endregion
                    answer.Value = reply.value;

                    return answer;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ChecksReadParent failed", ex);
            }
        }
        #endregion

        #region notification
        public int                  NotificationCreate(string microtingUId, string content)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    notifications aNoti = new notifications();

                    aNoti.microting_uuid = microtingUId;
                    aNoti.content = content;
                    aNoti.workflow_state = "created";
                    aNoti.created_at = DateTime.Now;
                    aNoti.updated_at = DateTime.Now;

                    db.notifications.Add(aNoti);
                    db.SaveChanges();

                    return aNoti.id;
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
                    try
                    {
                        var temp = db.notifications.Where(x => x.workflow_state == "created").ToList();
                        notifications aNoti = temp.First();

                        return aNoti.content;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("NotificationRead failed", ex);
            }
        }

        public void                 NotificationProcessed(string content, string workflowState)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    var lst = db.notifications.Where(x => x.content == content && x.workflow_state == "created").ToList();
                    
                    notifications aNoti = lst[0];
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
                    try
                    {
                        uploaded_data uD = db.uploaded_data.Where(x => x.workflow_state == "pre_created").ToList().First();
                        return uD.file_location;
                    }
                    catch
                    {
                        return "";
                    }
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
                        uploaded_data uD = db.uploaded_data.Where(x => x.file_location == urlString).ToList().First();
                        field_values fV = db.field_values.Where(x => x.uploaded_data_id == uD.id).ToList().First();
                        cases aCase = db.cases.Where(x => x.id == fV.case_id).ToList().First();

                        return CaseReadByMUId(aCase.microting_api_id);
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
                    try
                    {
                        uploaded_data uD = db.uploaded_data.Where(x => x.file_location == urlString).ToList().First();

                        uD.checksum = chechSum;
                        uD.file_location = fileLocation;
                        uD.file_name = fileName;
                        uD.local = 1;
                        uD.workflow_state = "created";
                        uD.updated_at = DateTime.Now;
                        uD.version = uD.version + 1;

                        db.SaveChanges();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FileProcessed failed", ex);
            }
        }
        #endregion

        public List<int> SitesList()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<int> lst = new List<int>();

                    List<sites> lstSite = db.sites.ToList();
                    foreach (sites site in lstSite)
                    {
                        lst.Add(int.Parse(site.microting_uuid));
                    }

                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SitesList failed", ex);
            }
        }

        public void CheckListSitesCreate(int checkListId, int siteId, string microtingUId)
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
                    cLS.microting_check_list_uuid = microtingUId;
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

        #region entityGroup
        public int EntityGroupCreate(string name, string entityType)
        {
            try
            {
                if (entityType != "Entity_Search" && entityType != "Entity_Select")
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

                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));
                    db.SaveChanges();

                    return eG.id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupCreate failed", ex);
            }
        }

        public EntityGroup EntityGroupRead(string entityGroupMUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_groups eG;

                    List<entity_groups> eGLst = db.entity_groups.Where(x => x.microtingUId == entityGroupMUId && x.workflow_state == "created").ToList();

                    if (eGLst == null)
                        return null;
                    if (eGLst.Count == 0)
                        return null;
                    if (eGLst.Count > 1)
                        throw new Exception("EntityGroupRead failed. Due more than 1 match found");

                    eG = eGLst[0];

                    List<EntityItem> lst = new List<EntityItem>();
                    EntityGroup rtnEG = new EntityGroup(eG.id, eG.name, eG.type, eG.microtingUId, lst);

                    List<entity_items> eILst = db.entity_items.Where(x => x.group_id == eG.id && x.workflow_state == "created").ToList();

                    if (eILst != null)
                        if (eILst.Count > 0)
                            foreach (entity_items item in eILst)
                            {
                                EntityItem eI = new EntityItem(item.name, item.microtingUId, item.description);
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

        public bool EntityGroupUpdate(int entityGroupId, string entityGroupMUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_groups eG;
                    try
                    {
                        eG = db.entity_groups.Where(x => x.id == entityGroupId).ToList().First();
                    }
                    catch
                    {
                        return false;
                    }

                    eG.microtingUId = entityGroupMUId;
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
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<EntityItemUpdateInfo> rtnLst = new List<EntityItemUpdateInfo>();
                    EntityGroup eGNew = entityGroup;
                    EntityGroup eGFDb = EntityGroupRead(eGNew.EntityGroupMUId);

                    //same, new or update
                    foreach (EntityItem itemNew in eGNew.EntityGroupItemLst)
                    {
                        foreach (EntityItem itemFDb in eGFDb.EntityGroupItemLst)
                        {
                            //same
                            if (itemNew.EntityItemMUId == itemFDb.EntityItemMUId)
                            {
                                if (itemNew.Description == itemFDb.Description && itemNew.Name == itemFDb.Description)
                                {
                                    //identic
                                    break;
                                }
                                else
                                {
                                    //updated
                                    EntityItemUpdate(itemNew);
                                    break;
                                }
                            }
                            else
                            {
                                //new
                                EntityItemCreate(entityGroup.Id, itemNew);
                                break;
                            }
                        }
                    }

                    //delete
                    bool stillInUse;
                    foreach (EntityItem itemFDb in eGFDb.EntityGroupItemLst)
                    {
                        stillInUse = false;
                        foreach (EntityItem itemNew in eGNew.EntityGroupItemLst)
                        {
                            if (itemNew.EntityItemMUId == itemFDb.EntityItemMUId)
                            {
                                stillInUse = true;
                                break;
                            }
                        }

                        if (!stillInUse)
                        {
                            EntityItemDelete(itemFDb.EntityItemMUId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupUpdateItems failed", ex);
            }
        }

        public bool EntityGroupDelete(string entityGroupMUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_groups eG;
                    try
                    {
                        eG = db.entity_groups.Where(x => x.microtingUId == entityGroupMUId && x.workflow_state == "created").ToList().First();
                    }
                    catch
                    {
                        return false;
                    }

                    eG.workflow_state = "removed";
                    eG.updated_at = DateTime.Now;
                    eG.version = eG.version + 1;
                    db.entity_group_versions.Add(MapEntityGroupVersions(eG));

                    List<entity_items> lst = db.entity_items.Where(x => x.group_id == eG.id && x.workflow_state == "created").ToList();
                    if (lst != null)
                    {
                        foreach (entity_items item in lst)
                        {
                            item.workflow_state = "removed";
                            item.updated_at = DateTime.Now;
                            item.version = item.version + 1;
                            item.synced = Bool(false);
                            db.entity_item_versions.Add(MapEntityItemVersions(item));
                        }
                    }

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupDelete failed", ex);
            }
        }
        #endregion

        #region entityItem sync
        public entity_items         EntityItemSyncedRead()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    try
                    {
                        var temp = db.entity_items.Where(x => x.synced == 0 && x.workflow_state == "created").ToList();
                        return temp.First();
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemSyncedRead failed", ex);
            }
        }

        public void                 EntityItemSyncedProcessed(int entityItemId, string workflowState)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    var lst = db.entity_items.Where(x => x.id == entityItemId).ToList();

                    entity_items eItem = lst[0];
                    eItem.workflow_state = workflowState;
                    eItem.updated_at = DateTime.Now;
                    eItem.version = eItem.version + 1;
                    eItem.synced = 1;

                    db.entity_item_versions.Add(MapEntityItemVersions(eItem));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemSyncedProcessed failed", ex);
            }
        }
        #endregion
        #endregion

        #region private
        #region EformCreateDb
        private int EformCreateDb           (MainElement mainElement)
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
                    cl.text = mainElement.Label;
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
                    cl.manual_sync = Bool(mainElement.ManualSync);
                    cl.extra_fields_enabled = 0; //used for non-MainElements
                    cl.done_button_enabled = 0; //used for non-MainElements
                    cl.approval_enabled = 0; //used for non-MainElements
                    cl.multi_approval = Bool(mainElement.MultiApproval);
                    cl.fast_navigation = Bool(mainElement.FastNavigation);
                    cl.download_entities = Bool(mainElement.DownloadEntities);

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

        private void CreateElementList      (int parentId, List<Element> lstElement)
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

        private void CreateGroupElement     (int parentId, GroupElement groupElement)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.text = groupElement.Label;
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
                    cl.review_enabled = Bool(groupElement.ReviewEnabled);
                    //manualSync - used for mainElements
                    cl.extra_fields_enabled = Bool(groupElement.ExtraFieldsEnabled);
                    cl.done_button_enabled = Bool(groupElement.DoneButtonEnabled);
                    cl.approval_enabled = Bool(groupElement.ApprovalEnabled);
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
                throw new Exception("CreateDataElement failed", ex);
            }
        }

        private void CreateDataElement      (int parentId, DataElement dataElement)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.text = dataElement.Label;
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
                    cl.review_enabled = Bool(dataElement.ReviewEnabled);
                    //manualSync - used for mainElements
                    cl.extra_fields_enabled = Bool(dataElement.ExtraFieldsEnabled);
                    cl.done_button_enabled = Bool(dataElement.DoneButtonEnabled);
                    cl.approval_enabled = Bool(dataElement.ApprovalEnabled);
                    //MultiApproval - used for mainElements
                    //FastNavigation - used for mainElements
                    //DownloadEntities - used for mainElements

                    db.check_lists.Add(cl);
                    db.SaveChanges();

                    db.check_list_versions.Add(MapCheckListVersions(cl));
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

        private void CreateDataItemGroup    (int elementId, FieldGroup fieldGroup)
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
                    field.text = fieldGroup.Label;

                    field.created_at = DateTime.Now;
                    field.updated_at = DateTime.Now;
                    field.workflow_state = "created";
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

        private void CreateDataItem         (int elementId, eFormRequest.DataItem dataItem)
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
                    field.text = dataItem.Label;
                    field.mandatory = Bool(dataItem.Mandatory);
                    field.read_only = Bool(dataItem.ReadOnly);

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
                            field.selected = Bool(checkBox.Selected);
                            break;

                        case "Comment":
                            Comment comment = (Comment)dataItem;
                            field.default_value = comment.Value;
                            field.max_length = comment.Maxlength;
                            field.split_screen = Bool(comment.SplitScreen);
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
                            field.geolocation_enabled = Bool(picture.GeolocationEnabled);
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
                            field.geolocation_enabled = Bool(text.GeolocationEnabled);
                            field.geolocation_forced = Bool(text.GeolocationForced);
                            field.geolocation_hidden = Bool(text.GeolocationHidden);
                            field.barcode_enabled = Bool(text.BarcodeEnabled);
                            field.barcode_type = text.BarcodeType;
                            break;

                        case "Timer":
                            Timer timer = (Timer)dataItem;
                            field.split_screen = Bool(timer.StopOnSave);
                            break;

                    //-------

                        case "EntitySearch":
                            EntitySearch entitySearch = (EntitySearch)dataItem;
                            field.entity_group_id = entitySearch.EntityTypeId;
                            field.default_value = entitySearch.DefaultValue.ToString();
                            field.is_num = Bool(entitySearch.IsNum);
                            field.query_type = entitySearch.QueryType;
                            field.min_search_lenght = entitySearch.MinSearchLenght;
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

                    db.field_versions.Add(MapFieldVersions(field));
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
                            check_lists cl = db.check_lists.Where(x => x.id == elementId).ToList().First();
                            GroupElement gElement = new GroupElement(cl.id, cl.text, Int(cl.display_index), cl.description, Bool(cl.approval_enabled), Bool(cl.review_enabled), Bool(cl.done_button_enabled),
                                Bool(cl.extra_fields_enabled), "", lst);

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
                            check_lists cl = db.check_lists.Where(x => x.id == elementId).ToList().First();
                            DataElement dElement = new DataElement(cl.id, cl.text, Int(cl.display_index), cl.description, Bool(cl.approval_enabled), Bool(cl.review_enabled), Bool(cl.done_button_enabled),
                                Bool(cl.extra_fields_enabled), "", new List<DataItemGroup>(), new List<eFormRequest.DataItem>());

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
                    fields f = db.fields.Where(x => x.id == dataItemId).ToList().First();
                    string fieldTypeStr = Find(Int(f.field_type_id));

                    //KEY POINT - mapping
                    switch (fieldTypeStr)
                    {
                        case "Audio":
                            lstDataItem.Add(new Audio(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Int(f.multi)));
                            break;

                        case "CheckBox":
                            lstDataItem.Add(new CheckBox(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Bool(f.default_value), Bool(f.selected)));
                            break;

                        case "Comment":
                            lstDataItem.Add(new Comment(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                f.default_value, Int(f.max_length), Bool(f.split_screen)));
                            break;

                        case "Date":
                            lstDataItem.Add(new Date(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                DateTime.Parse(f.min_value), DateTime.Parse(f.max_value), f.default_value));
                            break;

                        case "None":
                            lstDataItem.Add(new None(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index)));
                            break;

                        case "Number":
                            lstDataItem.Add(new Number(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                long.Parse(f.min_value), long.Parse(f.max_value), int.Parse(f.default_value), Int(f.decimal_count), f.unit_name));
                            break;

                        case "MultiSelect":
                            lstDataItem.Add(new MultiSelect(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                ReadPairs(f.key_value_pair_list)));
                            break;

                        case "Picture":
                            lstDataItem.Add(new Picture(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Int(f.multi), Bool(f.geolocation_enabled)));
                            break;

                        case "SaveButton":
                            lstDataItem.Add(new SaveButton(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index), f.default_value));
                            break;

                        case "ShowPdf":
                            lstDataItem.Add(new ShowPdf(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                            f.default_value));
                            break;

                        case "Signature":
                            lstDataItem.Add(new Signature(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index)));
                            break;

                        case "SingleSelect":
                            lstDataItem.Add(new SingleSelect(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                ReadPairs(f.key_value_pair_list)));
                            break;

                        case "Text":
                            lstDataItem.Add(new Text(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index), 
                                f.default_value, Int(f.max_length), Bool(f.geolocation_enabled), Bool(f.geolocation_forced), Bool(f.geolocation_hidden), Bool(f.barcode_enabled), f.barcode_type));
                            break;

                        case "Timer":
                            lstDataItem.Add(new Timer(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Bool(f.stop_on_save)));
                            break;
                            
                        case "EntitySearch":
                            lstDataItem.Add(new EntitySearch(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Int(f.default_value), Int(f.entity_group_id), Bool(f.is_num), f.query_type, Int(f.min_search_lenght), Bool(f.barcode_enabled), f.barcode_type));
                            break;

                        case "EntitySelect":
                            lstDataItem.Add(new EntitySelect(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Int(f.default_value), Int(f.entity_group_id)));
                            break;

                        case "FieldGroup":
                            List<eFormRequest.DataItem> lst = new List<eFormRequest.DataItem>();

                            lstDataItemGroup.Add(new FieldGroup(f.id.ToString(), f.text, f.description, f.color, Int(f.display_index), f.default_value, lst));

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

        #region EformCheckCreateDb
        //TODO rollback for failed inserts ?

        private void            EformCheckCreateDb(Response response, string xml)
        {
            if (response.Checks.Count == 0)
                throw new Exception("response.Checks.count == 0. Hence there was no checks to create");

            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    int elementId;
                    int userId = int.Parse(response.Checks[0].WorkerId);

                    List<string> elements = t.LocateList(xml, "<ElementList>", "</ElementList>");

                    int caseId = -1;

                    try
                    {
                        check_list_sites cLS = db.check_list_sites.Where(x => x.microting_check_list_uuid == response.Value).ToList().First();
                        caseId = CaseCreate(cLS.microting_check_list_uuid, (int)cLS.check_list_id, (int)cLS.site_id, "ReversedCase", DateTime.MinValue, "", "");
                    }
                    catch { }

                    foreach (string elementStr in elements)
                    {
                        if (caseId == -1)
                        {
                            try
                            {
                                cases c = db.cases.Where(x => x.microting_api_id == response.Value).ToList().First();
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

                        db.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                        db.SaveChanges();

                        #region foreach dataItem
                        elementId = clv.id;
                        List<string> dataItems = t.LocateList(elementStr, "<DataItem>", "</DataItem>");

                        if (dataItems != null)
                        {
                            foreach (string dataItemStr in dataItems)
                            {
                                CreateDataItemCheck(dataItemStr, clv.case_id, clv.user_id, elementId, response.Checks[0].Date);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCheckCreateDb failed", ex);
            }
        }

        private void            CreateDataItemCheck(string xml, int? caseId, int? userId, int elementId, string dateOfDoing)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    field_values fieldV = new field_values();

                    #region if contains a file
                    string urlXml = t.Locate(xml, "<URL>", "</URL>");
                    if (urlXml != "" && urlXml != "none")
                    {
                        uploaded_data uD = new uploaded_data();

                        uD.created_at = DateTime.Now;
                        uD.updated_at = DateTime.Now;
                        uD.extension = t.Locate(xml, "<Extension>", "</");
                        uD.uploader_id = userId;
                        uD.uploader_type = "system";
                        uD.workflow_state = "pre_created";
                        uD.version = 1;
                        uD.local = 0;
                        uD.file_location = t.Locate(xml, "<URL>", "</");

                        db.uploaded_data.Add(uD);
                        db.SaveChanges();

                        db.uploaded_data_versions.Add(MapUploadedDataVersions(uD));
                        db.SaveChanges();



                        fieldV.uploaded_data_id = uD.id;
                    }
                    #endregion

                    fieldV.created_at = DateTime.Now;
                    fieldV.updated_at = DateTime.Now;
                    #region fieldV.value = t.Locate(xml, "<Value>", "</");
                    string temp = t.Locate(xml, "<Value>", "</");

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
                    fieldV.latitude = t.Locate(xml, " < Latitude>", "</");
                        fieldV.longitude = t.Locate(xml, "<Longitude>", "</");
                        fieldV.altitude = t.Locate(xml, "<Altitude>", "</");
                        fieldV.heading = t.Locate(xml, "<Heading>", "</");
                        fieldV.accuracy = t.Locate(xml, "<Accuracy>", "</");
                        fieldV.date = Date(t.Locate(xml, "<Date>", "</"));
                    //
                    fieldV.workflow_state = "created";
                    fieldV.version = 1;
                    fieldV.case_id = caseId;
                    fieldV.field_id = int.Parse(t.Locate(xml, "<Id>", "</"));
                    fieldV.user_id = userId;
                    fieldV.check_list_id = elementId;
                    fieldV.date_of_doing = Date(dateOfDoing);

                    db.field_values.Add(fieldV);
                    db.SaveChanges();

                    db.field_value_versions.Add(MapFieldValueVersions(fieldV));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformReadDb failed", ex);
            }
        }
        #endregion

        #region EntityItem 
        private void EntityItemCreate(int parentId, EntityItem entityItem)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    entity_items eI = new entity_items();

                    eI.created_at = DateTime.Now;
                    eI.description = entityItem.Description;
                    eI.group_id = parentId;
                    //eI.id = "";
                    eI.microtingUId = entityItem.EntityItemMUId;
                    eI.name = entityItem.Name;
                    eI.synced = Bool(false);
                    eI.updated_at = DateTime.Now;
                    eI.version = 1;
                    eI.workflow_state = "created";

                    db.entity_items.Add(eI);
                    db.SaveChanges();

                    db.entity_item_versions.Add(MapEntityItemVersions(eI));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemCreate failed", ex);
            }
        }

        private void EntityItemUpdate(EntityItem entityItem)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<entity_items> lst = db.entity_items.Where(x => x.microtingUId == entityItem.EntityItemMUId && x.workflow_state == "created").ToList();

                    if (lst == null)
                        throw new Exception("EntityGroupRead failed. Due list was null");
                    if (lst.Count == 0)
                        throw new Exception("EntityGroupRead failed. Due list was empty");
                    if (lst.Count > 1)
                        throw new Exception("EntityGroupRead failed. Due list was more than 1 match");
                    entity_items eI = lst[0];

                    eI.description = entityItem.Description;
                    eI.name = entityItem.Name;
                    eI.synced = Bool(false);
                    eI.updated_at = DateTime.Now;
                    eI.version = eI.version+1;

                    db.SaveChanges();

                    db.entity_item_versions.Add(MapEntityItemVersions(eI));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemUpdate failed", ex);
            }
        }

        private void EntityItemDelete(string EntityItemMUId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<entity_items> lst = db.entity_items.Where(x => x.microtingUId == EntityItemMUId && x.workflow_state == "created").ToList();

                    if (lst == null)
                        throw new Exception("EntityGroupRead failed. Due list was null");
                    if (lst.Count == 0)
                        throw new Exception("EntityGroupRead failed. Due list was empty");
                    if (lst.Count > 1)
                        throw new Exception("EntityGroupRead failed. Due list was more than 1 match");
                    entity_items eI = lst[0];

                    eI.updated_at = DateTime.Now;
                    eI.version = eI.version + 1;
                    eI.workflow_state = "removed";

                    db.SaveChanges();

                    db.entity_item_versions.Add(MapEntityItemVersions(eI));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EntityItemUpdate failed", ex);
            }
        }
        #endregion

        #region Get*
        private bool Bool(short? input)
        {
            if (input == null)
                return false;
            string temp = input.ToString();
            if (temp == "0" || temp.ToLower() == "false" || temp == "")
                return false;
            if (temp == "1" || temp.ToLower() == "true")
                return true;
            throw new Exception(temp + ": was not found to be a bool");
        }

        private bool Bool(string input)
        {
            if (input == null)
                return false;
            if (input == "0" || input.ToLower() == "false" || input == "")
                return false;
            if (input == "1" || input.ToLower() == "true")
                return true;
            throw new Exception(input + ": was not found to be a bool");
        }

        private short Bool(bool inputBool)
        {
            if (inputBool == false)
                return 0;
            else
                return 1;
        }

        private int Int(int? input)
        {
            if (input == null)
                return 0;

            string str = input.ToString();
            return int.Parse(str);
        }

        private int Int(string input)
        {
            return int.Parse(input);
        }

        private DateTime? Date(string input)
        {
            if (input == "")
                return null;
            else
                return DateTime.Parse(input);
        }

        private DateTime Date(DateTime? input)
        {
            if (input != null)
                return (DateTime)input;
            else
                return DateTime.MinValue;
        }

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
        #endregion

        #region KeyValuePairs
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
        private case_versions               MapCaseVersions(cases aCase)
        {
            case_versions caseVer = new case_versions();
            caseVer.status = aCase.status;
            caseVer.date_of_doing = aCase.date_of_doing;
            caseVer.updated_at = aCase.updated_at;
            caseVer.done_by_user_id = aCase.done_by_user_id;
            caseVer.workflow_state = aCase.workflow_state;
            caseVer.version = aCase.version;
            caseVer.microting_check_id = aCase.microting_check_id;
            caseVer.unit_id = aCase.unit_id;

            caseVer.created_by_user_id = aCase.created_by_user_id;
            caseVer.versioned_type = aCase.type; //<<--
            caseVer.created_at = aCase.created_at;
            caseVer.check_list_id = aCase.check_list_id;
            caseVer.microting_api_id = aCase.microting_api_id;
            caseVer.site_id = aCase.site_id;
            caseVer.reg_number = aCase.reg_number;
            caseVer.vejenummer = aCase.vejenummer;
            caseVer.case_id = aCase.id; //<<--

            return caseVer;
        }

        private check_list_versions         MapCheckListVersions(check_lists checkList)
        {
            check_list_versions clv = new check_list_versions();
            clv.created_at = checkList.created_at;
            clv.updated_at = checkList.updated_at;
            clv.text = checkList.text;
            clv.description = checkList.description;
            clv.serialized_default_values = checkList.serialized_default_values;
            clv.workflow_state = checkList.workflow_state;
            clv.parent_id = checkList.parent_id;
            clv.repeated = checkList.repeated;
            clv.version = checkList.version;
            clv.case_type = checkList.case_type;
            clv.folder_name = checkList.folder_name;
            clv.display_index = checkList.display_index;
            clv.report_file_name = checkList.report_file_name;
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
            fv.serialized_default_values = field.serialized_default_values;
            fv.workflow_state = field.workflow_state;
            fv.check_list_id = field.check_list_id;
            fv.text = field.text;
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
            fvv.date_of_doing = fieldValue.date_of_doing;

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

            udv.uploaded_data_id = uploadedData.id; //<<--

            return udv;
        }

        private check_list_site_versions    MapCheckListSiteVersions(check_list_sites checkListSite)
        {
            check_list_site_versions checkListSiteVer = new check_list_site_versions();
            checkListSiteVer.check_list_id = checkListSite.check_list_id;
            checkListSiteVer.created_at = checkListSite.created_at;
            checkListSiteVer.updated_at = checkListSite.updated_at;
            checkListSiteVer.last_check_id = checkListSite.last_check_id;
            checkListSiteVer.microting_check_list_uuid = checkListSite.microting_check_list_uuid;
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
            entityGroupVer.microtingUId = entityGroup.microtingUId;
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
            entityItemVer.created_at = entityItem.created_at;
            entityItemVer.description = entityItem.description;
            entityItemVer.id = entityItem.id;
            entityItemVer.microtingUId = entityItem.microtingUId;
            entityItemVer.name = entityItem.name;
            entityItemVer.synced = entityItem.synced;
            entityItemVer.updated_at = entityItem.updated_at;
            entityItemVer.version = entityItem.version;
            entityItemVer.workflow_state = entityItem.workflow_state;

            entityItemVer.group_id = entityItem.id; //<<--

            return entityItemVer;
        }
        #endregion
        #endregion
    }

    #region help classes
    public class Case_Dto
    {
        #region con
        public Case_Dto()
        {
        }

        public Case_Dto(int siteId, string caseType, string caseUId, string microtingUId, string checkUId)
        {
            if (caseType == null)
                caseType = "";
            if (caseUId == null)
                caseUId = "";
            if (microtingUId == null)
                microtingUId = "";
            if (checkUId == null)
                checkUId = "";

            SiteId = siteId;
            CaseType = caseType;
            CaseUId = caseUId;
            MicrotingUId = microtingUId;
            CheckUId = checkUId;
        }
        #endregion

        #region var
        /// <summary>
        /// Unique identifier of device
        /// </summary>
        public int SiteId { get; }

        /// <summary>
        /// Identifier of a collection of cases in your system
        /// </summary>
        public string CaseType { get; }

        /// <summary>
        /// Unique identifier of a group of case(s) in your system
        /// </summary>
        public string CaseUId { get; }

        /// <summary>
        ///Unique identifier of that specific eForm in Microting system
        /// </summary>
        public string MicrotingUId { get; }

        /// <summary>
        /// Unique identifier of that check of the eForm. Only used if repeat
        /// </summary>
        public string CheckUId { get; }
        #endregion

        public override string ToString()
        {
            return "Site:" + SiteId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + " / CheckId:" + CheckUId;
        }
    }

    public class File_Dto
    {
        #region con
        public File_Dto(int siteId, string caseType, string caseUId, string microtingUId, string checkUId, string fileLocation)
        {
            if (caseType == null)
                caseType = "";
            if (caseUId == null)
                caseUId = "";
            if (microtingUId == null)
                microtingUId = "";
            if (checkUId == null)
                checkUId = "";
            if (fileLocation == null)
                fileLocation = "";

            SiteId = siteId;
            CaseType = caseType;
            CaseUId = caseUId;
            MicrotingUId = microtingUId;
            CheckUId = checkUId;
            FileLocation = fileLocation;
        }
        #endregion

        #region var
        /// <summary>
        /// Unique identifier of device
        /// </summary>
        public int SiteId { get; }

        /// <summary>
        /// Identifier of a collection of cases in your system
        /// </summary>
        public string CaseType { get; }

        /// <summary>
        /// Unique identifier of a group of case(s) in your system
        /// </summary>
        public string CaseUId { get; }

        /// <summary>
        ///Unique identifier of that specific eForm in Microting system
        /// </summary>
        public string MicrotingUId { get; }

        /// <summary>
        /// Unique identifier of that check of the eForm. Only used if repeat
        /// </summary>
        public string CheckUId { get; }

        /// <summary>
        /// Location of the fil
        /// </summary>
        public string FileLocation { get; set; }
        #endregion

        public override string ToString()
        {
            return "Site:" + SiteId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + " / CheckId:" + CheckUId + " / FileLocation:" + FileLocation;
        }
    }

    internal class Holder
    {
        internal Holder(int index, string field_type)
        {
            Index = index;
            FieldType = field_type;
        }

        internal int Index { get; }

        internal string FieldType { get; }
    }

    internal class EntityItemUpdateInfo
    {
        internal EntityItemUpdateInfo(string entityItemMUid, string status)
        {
            EntityItemMUid = entityItemMUid;
            Status = status;
        }

        public string EntityItemMUid { get; set; }
        public string Status { get; set; }
    }
    #endregion
}