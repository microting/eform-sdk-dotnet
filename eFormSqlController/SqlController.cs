using eFormRequest;
using eFormResponse;
using MiniTrols;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace eFormSqlController
{
    public class SqlController
    {
        #region var
        List<Holder> converter;
        Tools tool = new Tools();
        object _lockQuery = new object();
        string connectionStr;
        int userId;
        #endregion

        #region con
        public SqlController(string connectionString, int userId)
        {
            connectionStr = connectionString;
            this.userId = userId;
        }
        #endregion

        #region public
        public int           EformCreate(MainElement mainElement)
        {
            int id = EformCreateDb(mainElement, "");
            return id;
        }

        public MainElement   EformRead(int templatId)
        {
            MainElement mainElement = new MainElement();
            mainElement = EformReadDb(templatId);

            return mainElement;
        }

        public void          EformCheckCreate(Response response, string xml)
        {
            EformCheckCreateDb(response, xml);
        }

        public int           CaseCreate(string microtingUuid, int checkListId, int siteId, string caseType, string numberPlate, string roadNumber)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases aCase = new cases();

                    aCase.status = 66;
                    aCase.created_by_user_id = userId; 
                    aCase.type = caseType;
                    aCase.created_at = DateTime.Now;
                    aCase.updated_at = DateTime.Now;
                    aCase.check_list_id = checkListId;
                    aCase.microting_api_id = microtingUuid;
                    aCase.workflow_state = "created";
                    aCase.version = 1;
                    aCase.site_id = siteId;
                    aCase.reg_number = numberPlate;
                    aCase.vejenummer = roadNumber;

                    db.cases.Add(aCase);
                    db.SaveChanges();

                    db.case_versions.Add(MapCaseVersion(aCase));
                    db.SaveChanges();

                    return aCase.id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseCreate failed", ex);
            }
        }

        public void          CaseUpdate(string microtingUuid, DateTime dateOfDoing, int doneByUserId, string microtingCheckId, int unitId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases caseStd = db.cases.Where(x => x.microting_api_id == microtingUuid).ToList().First();

                    caseStd.status = 100;
                    caseStd.date_of_doing = dateOfDoing;
                    caseStd.updated_at = DateTime.Now;
                    caseStd.done_by_user_id = doneByUserId;
                    caseStd.workflow_state = "created";
                    caseStd.version = caseStd.version + 1;
                    caseStd.microting_check_id = microtingCheckId;
                    caseStd.unit_id = unitId;

                    db.case_versions.Add(MapCaseVersion(caseStd));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseUpdate failed", ex);
            }
        }

        public List<string>  CaseFindMatchs(string microtingUuid)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases match = db.cases.Where(x => x.microting_api_id == microtingUuid).ToList().First();
                    List<string> foundCasesThatMatch = new List<string>();

                    if (match.vejenummer == "unique") //There are no linked cases
                    {
                        foundCasesThatMatch.Add("{" + match.site_id + "}[" + match.microting_api_id + "]");
                        return foundCasesThatMatch;
                    }
                    else //There are linked cases
                    {
                        List<cases> lstMatchs = db.cases.Where(x => x.vejenummer == match.vejenummer).ToList();

                        foreach (cases aCase in lstMatchs)
                        {
                            foundCasesThatMatch.Add("{" + aCase.site_id + "}[" + aCase.microting_api_id + "]");
                        }
                        return foundCasesThatMatch;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseFindMatchs failed", ex);
            }
        }

        public void          CaseDelete(string microtingUuid)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    cases aCase = db.cases.Where(x => x.microting_api_id == microtingUuid).ToList().First();

                    aCase.updated_at = DateTime.Now;
                    aCase.workflow_state = "removed";
                    aCase.version = aCase.version + 1;
                    aCase.removed_by_user_id = userId;

                    db.case_versions.Add(MapCaseVersion(aCase));
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CaseDelete failed", ex);
            }
        }

        public int           NotificationCreate(string microting_uuid, string content)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    notifications aNoti = new notifications();

                    aNoti.microting_uuid = microting_uuid;
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

        public string        NotificationRead()
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

        public void          NotificationProcessed(string content)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    var lst = db.notifications.Where(x => x.content == content).ToList();
                    
                    foreach (notifications aNoti in lst)
                    {
                        aNoti.workflow_state = "processed";
                        aNoti.updated_at = DateTime.Now;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("NotificationProcessed failed", ex);
            }
        }

        public string        FileRead()
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

        public void          FileProcessed(string urlStr, string chechSum, string fileLocation, string fileName)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    try
                    {
                        uploaded_data uD = db.uploaded_data.Where(x => x.file_location == urlStr).ToList().First();

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

        public List<string>  SitesList()
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    List<string> lst = new List<string>();

                    List<sites> lstSite = db.sites.ToList();
                    foreach (sites site in lstSite)
                    {
                        lst.Add(site.microting_uuid);
                    }

                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SitesList failed", ex);
            }
        }

        public void Test()
        {
            //do stuff
        }
        #endregion

        #region private
        #region EformCreateDb
        //TODO rollback for failed inserts ?

        private int EformCreateDb(MainElement mainElement, string caseType)
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
                    cl.case_type = caseType; //hmm
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
                    mainElement.Id = mainId.ToString();
                    #endregion

                    #region foreach element
                    foreach (Element dE in mainElement.ElementList)
                    {
                        if (dE.GetType() == typeof(DataElement))
                        {
                            CreateDataElement(mainId, (DataElement)dE);
                        }

                        if (dE.GetType() == typeof(GroupElement))
                        {
                            throw new NotImplementedException("TODO - Get to work");
                        }
                    }
                    #endregion

                    return mainId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformCreateDb failed", ex);
            }
        }

        private void CreateDataElement(int mainId, DataElement dataElement)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    check_lists cl = new check_lists();
                    cl.created_at = DateTime.Now;
                    cl.updated_at = DateTime.Now;
                    cl.text = dataElement.Label;
                    cl.description = dataElement.Description;
                    //serialized_default_values - Ruby colume
                    cl.workflow_state = "created";
                    cl.parent_id = mainId;
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

                    foreach (eFormRequest.DataItem dataItem in dataElement.DataItemList)
                    {
                        CreateDataItem(cl.id, dataItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateDataElement failed", ex);
            }
        }

        private void CreateDataItem(int elementId, eFormRequest.DataItem dataItem)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    string typeStr = dataItem.GetType().ToString().Remove(0, 13).Replace("_", ""); //13 = "eFormRequest.".Length
                    int fieldTypeId = Find(typeStr);

                    fields field = new fields();
                    
                    field.color = dataItem.Color;
                    field.description = dataItem.Description;
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

                        case "Number":
                            Number number = (Number)dataItem;
                            field.min_value = number.MinValue.ToString();
                            field.max_value = number.MaxValue.ToString();
                            field.default_value = number.DefaultValue.ToString();
                            field.decimal_count = number.DecimalCount;
                            field.unit_name = number.UnitName;
                            break;

                        case "None":
                            break;

                        case "CheckBox":
                            Check_Box checkBox = (Check_Box)dataItem;
                            field.default_value = checkBox.DefaultValue.ToString();
                            field.selected = Bool(checkBox.Selected);
                            break;

                        case "Picture":
                            Picture picture = (Picture)dataItem;
                            field.multi = picture.Multi;
                            field.geolocation_enabled = Bool(picture.GeolocationEnabled);
                            break;

                        case "Audio":
                            Audio audio = (Audio)dataItem;
                            field.multi = audio.Multi;
                            break;

                        case "SingleSelect":
                            Single_Select singleSelect = (Single_Select)dataItem;
                            field.key_value_pair_list = WritePairs(singleSelect.KeyValuePairList);
                            break;

                        case "MultiSelect":
                            Multi_Select multiSelect = (Multi_Select)dataItem;
                            field.key_value_pair_list = WritePairs(multiSelect.KeyValuePairList);
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

                        case "Signature":
                            break;

                        case "Timer":
                            eFormRequest.Timer timer = (eFormRequest.Timer)dataItem;
                            field.split_screen = Bool(timer.StopOnSave);
                            break;

                        case "EntitySelect":
                            throw new NotImplementedException("TODO");

                        case "EntitySearch":
                            throw new NotImplementedException("TODO");

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
        private MainElement EformReadDb(int mainId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    MainElement mainElement = null;
                    GetConverter();

                    try
                    {
                        //getting mainElement
                        check_lists mainCl = db.check_lists.Where(x => x.id == mainId).ToList().First();

                        mainElement = new MainElement(mainCl.id.ToString(), mainCl.description, Int(mainCl.display_index), mainCl.folder_name, Int(mainCl.repeated), DateTime.Now, DateTime.Now.AddDays(2), "da",
                            Bool(mainCl.multi_approval), Bool(mainCl.fast_navigation), Bool(mainCl.download_entities), Bool(mainCl.manual_sync), "", "", new List<Element>());


                        //getting elements
                        List<check_lists> lst = db.check_lists.Where(x => x.parent_id == mainId).ToList();

                        foreach (check_lists cl in lst)
                        {
                            mainElement.ElementList.Add(GetElement(cl.id));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to find matching check_list:" + mainId, ex);
                    }
                    
                    //return
                    return mainElement;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("EformReadDb failed", ex);
            }
        }

        private Element GetElement(int elementId)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    Element element = null;

                    //getting mainElement
                    List<check_lists> lstElement = db.check_lists.Where(x => x.parent_id == elementId).ToList();

                    if (lstElement.Count > 0)
                    {
                        //GroupElement
                        throw new NotImplementedException("TODO - Here be dragons");
                    }
                    else
                    {
                        //DataElement

                        //list for the DataItems
                        List<eFormRequest.DataItem> lst = new List<eFormRequest.DataItem>();

                        //the actual DataElement
                        try
                        {

                            check_lists cl = db.check_lists.Where(x => x.id == elementId).ToList().First();
                            element = new DataElement(cl.id.ToString(), cl.text, Int(cl.display_index), cl.description, Bool(cl.approval_enabled), Bool(cl.review_enabled), Bool(cl.done_button_enabled),
                                Bool(cl.extra_fields_enabled), "", lst);

                            //the actual DataItems
                            List<fields> lstFields = db.fields.Where(x => x.check_list_id == elementId).ToList();
                            foreach (var field in lstFields)
                            {
                                lst.Add(GetDataItem(field.id));
                            }
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

        private eFormRequest.DataItem GetDataItem(int dataItemId)
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
                        case "Text":
                            return new Text(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index), 
                                f.default_value, Int(f.max_length), Bool(f.geolocation_enabled), Bool(f.geolocation_forced), Bool(f.geolocation_hidden), Bool(f.barcode_enabled), f.barcode_type);

                        case "Number":
                            return new Number(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                int.Parse(f.min_value), int.Parse(f.max_value), int.Parse(f.default_value), Int(f.decimal_count), f.unit_name);

                        case "None":
                            return new None(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index));

                        case "CheckBox":
                            return new Check_Box(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Bool(f.default_value), Bool(f.selected));

                        case "Picture":
                            return new Picture(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Int(f.multi), Bool(f.geolocation_enabled));

                        case "Audio":
                            return new Audio(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Int(f.multi));

                        case "SingleSelect":
                            return new Single_Select(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                ReadPairs(f.key_value_pair_list));

                        case "MultiSelect":
                            return new Multi_Select(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                ReadPairs(f.key_value_pair_list));

                        case "Comment":
                            return new Comment(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                f.default_value, Int(f.max_length), Bool(f.split_screen));

                        case "Date":
                            return new Date(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                DateTime.Parse(f.min_value), DateTime.Parse(f.max_value), f.default_value);

                        case "Signature":
                            return new Signature(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index));

                        case "Timer":
                            return new eFormRequest.Timer(f.id.ToString(), Bool(f.mandatory), Bool(f.read_only), f.text, f.description, f.color, Int(f.display_index),
                                Bool(f.stop_on_save));

                        case "EntitySelect":
                            throw new NotImplementedException("TODO");
                        //return new Entity_Select(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        //    0); //TODO

                        case "EntitySearch":
                            throw new NotImplementedException("TODO");
                        //return new Entity_Search(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        //    0); //TODO

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

        private void         EformCheckCreateDb(Response response, string xml)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    int elementId;
                    int userId = int.Parse(response.Checks[0].WorkerId);

                    List<string> elements = tool.LocateList(xml, "<ElementList>", "</ElementList>");

                    foreach (string elementStr in elements)
                    {
                        cases c = db.cases.Where(x => x.microting_api_id == response.Value).ToList().First();
                        int caseId = c.id;

                        check_list_values clv = new check_list_values();

                        clv.created_at = DateTime.Now;
                        clv.updated_at = DateTime.Now;
                        clv.check_list_id = int.Parse(tool.Locate(elementStr, "<Id>", "</"));
                        clv.case_id = caseId;
                        clv.status = tool.Locate(elementStr, "<Status>", "</");
                        clv.version = 1;
                        clv.user_id = userId;
                        clv.workflow_state = "created";

                        db.check_list_values.Add(clv);
                        db.SaveChanges();

                        db.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                        db.SaveChanges();

                        #region foreach dataItem
                        elementId = clv.id;
                        List<string> dataItems = tool.LocateList(elementStr, "<DataItem>", "</DataItem>");

                        foreach (string dataItemStr in dataItems)
                        {
                            CreateDataItemCheck(dataItemStr, clv.case_id, clv.user_id, elementId, response.Checks[0].Date);
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

        private void         CreateDataItemCheck(string xml, int? caseId, int? userId, int elementId, string dateOfDoing)
        {
            try
            {
                using (var db = new MicrotingDb(connectionStr))
                {
                    field_values fieldV = new field_values();

                    #region if contains a file
                    if (tool.Locate(xml, "<URL>", "</URL>") != "")
                    {
                        uploaded_data uD = new uploaded_data();

                        uD.created_at = DateTime.Now;
                        uD.updated_at = DateTime.Now;
                        uD.extension = tool.Locate(xml, "<Extension>", "</");
                        uD.uploader_id = userId;
                        uD.uploader_type = "system";
                        uD.workflow_state = "pre_created";
                        uD.version = 1;
                        uD.local = 0;
                        uD.file_location = tool.Locate(xml, "<URL>", "</");

                        db.uploaded_data.Add(uD);
                        db.SaveChanges();

                        db.uploaded_data_versions.Add(MapUploadedDataVersions(uD));
                        db.SaveChanges();



                        fieldV.uploaded_data_id = uD.id;
                    }
                    #endregion

                    fieldV.created_at = DateTime.Now;
                    fieldV.updated_at = DateTime.Now;
                    fieldV.value = tool.Locate(xml, "<Value>", "</");
                    //geo
                        fieldV.latitude = tool.Locate(xml, "<Latitude>", "</");
                        fieldV.longitude = tool.Locate(xml, "<Longitude>", "</");
                        fieldV.altitude = tool.Locate(xml, "<Altitude>", "</");
                        fieldV.heading = tool.Locate(xml, "<Heading>", "</");
                        fieldV.accuracy = tool.Locate(xml, "<Accuracy>", "</");
                        fieldV.date = Date(tool.Locate(xml, "<Date>", "</"));
                    //
                    fieldV.workflow_state = "created";
                    fieldV.version = 1;
                    fieldV.case_id = caseId;
                    fieldV.field_id = int.Parse(tool.Locate(xml, "<Id>", "</"));
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

        #region Get*
        private bool Bool(short? input)
        {
            if (input == null)
                return false;
            string temp = input.ToString();
            if (temp == "0" || temp == "false" || temp == "")
                return false;
            if (temp == "1" || temp == "true")
                return true;
            throw new Exception(temp + ": was not found to be a bool");
        }

        private bool Bool(string input)
        {
            if (input == null || input == "0" || input == "false" || input == "")
                return false;
            if (input == "1" || input == "true")
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

        private DateTime? Date(string input)
        {
            if (input == "")
                return null;
            else
                return DateTime.Parse(input);
        }

        private string Find(int fieldTypeId)
        {
            foreach (var holder in converter)
            {
                if (holder.Index == fieldTypeId)
                    return holder.FieldType;
            }
            return null;
        }

        private int Find(string typeStr)
        {
            foreach (var holder in converter)
            {
                if (holder.FieldType == typeStr)
                    return holder.Index;
            }
            return -1;
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
            str = tool.Locate(str, "<hash>", "</hash>");

            bool flag = true;
            int index = 1;
            string keyValue, displayIndex;
            bool selected;

            while (flag)
            {
                string inderStr = tool.Locate(str, "<" + index + ">", "</" + index + ">");

                keyValue = tool.Locate(inderStr, "<key>", "</");
                selected = bool.Parse(tool.Locate(inderStr.ToLower(), "<selected>", "</"));
                displayIndex = tool.Locate(inderStr, "<displayIndex>", "</");

                list.Add(new KeyValuePair(index.ToString(), keyValue, selected, displayIndex));

                index += 1;

                if (tool.Locate(str, "<" + index + ">", "</" + index + ">") == "")
                    flag = false;
            }

            return list;
        }
        #endregion

        #region mappers
        private case_versions MapCaseVersion(cases aCase)
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

        private check_list_versions MapCheckListVersions(check_lists checkList)
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
            fvv.date_of_doing = fieldValue.date_of_doing;

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

            udv.uploaded_data_id = uploadedData.id; //<<--

            return udv;
        }
        #endregion
        #endregion
    }

    #region help class
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
    #endregion
}
