using eFormData;
using eFormShared;
using eFormSqlController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace eFormCore.Helpers
{
    public class TestHelpers
    {
        private MicrotingDbMs DbContext;
        private string returnXML;
        private string returnJSON;

        public TestHelpers()
        {
            string ConnectionString = @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests;Integrated Security=True";
            DbContext = new MicrotingDbMs(ConnectionString);
        }

        #region helperMethods
        public workers CreateWorker(string email, string firstName, string lastName, int microtingUId)
        {
            workers worker = new workers();
            worker.first_name = firstName;
            worker.last_name = lastName;
            worker.email = email;
            worker.created_at = DateTime.Now;
            worker.updated_at = DateTime.Now;
            worker.microting_uid = microtingUId;
            worker.workflow_state = Constants.WorkflowStates.Created;
            worker.version = 69;
            DbContext.workers.Add(worker);
            DbContext.SaveChanges();

            return worker;
        }
        public sites CreateSite(string name, int microtingUId)
        {

            sites site = new sites();
            site.name = name;
            site.microting_uid = microtingUId;
            site.updated_at = DateTime.Now;
            site.created_at = DateTime.Now;
            site.version = 64;
            site.workflow_state = Constants.WorkflowStates.Created;
            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            return site;
        }
        public units CreateUnit(int microtingUId, int otpCode, sites site, int customerNo)
        {

            units unit = new units();
            unit.microting_uid = microtingUId;
            unit.otp_code = otpCode;
            unit.site = site;
            unit.site_id = site.id;
            unit.created_at = DateTime.Now;
            unit.customer_no = customerNo;
            unit.updated_at = DateTime.Now;
            unit.version = 9;
            unit.workflow_state = Constants.WorkflowStates.Created;

            DbContext.units.Add(unit);
            DbContext.SaveChanges();

            return unit;
        }
        public site_workers CreateSiteWorker(int microtingUId, sites site, workers worker)
        {
            site_workers site_workers = new site_workers();
            site_workers.created_at = DateTime.Now;
            site_workers.microting_uid = microtingUId;
            site_workers.updated_at = DateTime.Now;
            site_workers.version = 63;
            site_workers.site = site;
            site_workers.site_id = site.id;
            site_workers.worker = worker;
            site_workers.worker_id = worker.id;
            site_workers.workflow_state = Constants.WorkflowStates.Created;
            DbContext.site_workers.Add(site_workers);
            DbContext.SaveChanges();
            return site_workers;
        }
        public check_lists CreateTemplate(DateTime cl_ca, DateTime cl_ua, string label, string description, string caseType, string folderName, int displayIndex, int repeated)
        {
            
            check_lists cl1 = new check_lists();
            cl1.created_at = cl_ca;
            cl1.updated_at = cl_ua;
            cl1.label = label;
            cl1.description = description;
            cl1.workflow_state = Constants.WorkflowStates.Created;
            cl1.case_type = caseType;
            cl1.folder_name = folderName;
            cl1.display_index = displayIndex;
            cl1.repeated = repeated;
            
            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();
            return cl1;
        }
        public check_lists CreateSubTemplate(string label, string description, string caseType, int displayIndex, int repeated, check_lists parentId)
        {
            check_lists cl2 = new check_lists();
            cl2.created_at = DateTime.Now;
            cl2.updated_at = DateTime.Now;
            cl2.label = label;
            cl2.description = description;
            cl2.workflow_state = Constants.WorkflowStates.Created;
            cl2.case_type = caseType;
            cl2.display_index = displayIndex;
            cl2.repeated = repeated;
            cl2.parent_id = parentId.id;

            DbContext.check_lists.Add(cl2);
            DbContext.SaveChanges();
            return cl2;
        }
        public fields CreateField(short? barcodeEnabled, string barcodeType, check_lists checkList, string color, string custom, int? decimalCount, string defaultValue, string description, int? displayIndex, short? dummy, field_types ft, short? geolocationEnabled, short? geolocationForced, short? geolocationHidden, short? isNum, string label, short? mandatory, int maxLength, string maxValue, string minValue, short? multi, short? optional, string queryType, short? readOnly, short? selected, short? splitScreen, short? stopOnSave, string unitName, int version)
        {

            fields f = new fields();
            f.field_type = ft;

            f.barcode_enabled = barcodeEnabled;
            f.barcode_type = barcodeType;
            f.check_list_id = checkList.id;
            f.color = color;
            f.created_at = DateTime.Now;
            f.custom = custom;
            f.decimal_count = decimalCount;
            f.default_value = defaultValue;
            f.description = description;
            f.display_index = displayIndex;
            f.dummy = dummy;
            f.geolocation_enabled = geolocationEnabled;
            f.geolocation_forced = geolocationForced;
            f.geolocation_hidden = geolocationHidden;
            f.is_num = isNum;
            f.label = label;
            f.mandatory = mandatory;
            f.max_length = maxLength;
            f.max_value = maxValue;
            f.min_value = minValue;
            f.multi = multi;
            f.optional = optional;
            f.query_type = queryType;
            f.read_only = readOnly;
            f.selected = selected;
            f.split_screen = splitScreen;
            f.stop_on_save = stopOnSave;
            f.unit_name = unitName;
            f.updated_at = DateTime.Now;
            f.version = version;
            f.workflow_state = Constants.WorkflowStates.Created;

            DbContext.fields.Add(f);
            DbContext.SaveChanges();
            Thread.Sleep(2000);

            return f;
        }
        public cases CreateCase(string caseUId, check_lists checkList, DateTime created_at, string custom, DateTime done_at, workers doneByUserId, string microtingCheckId, string microtingUId, sites site, int? status, string caseType, units unit, DateTime updated_at, int version, workers worker, string WorkFlowState)
        {

            cases aCase = new cases();

            aCase.case_uid = caseUId;
            aCase.check_list = checkList;
            aCase.check_list_id = checkList.id;
            aCase.created_at = created_at;
            aCase.custom = custom;
            aCase.done_at = done_at;
            aCase.done_by_user_id = worker.id;
            aCase.microting_check_uid = microtingCheckId;
            aCase.microting_uid = microtingUId;
            aCase.site = site;
            aCase.site_id = site.id;
            aCase.status = status;
            aCase.type = caseType;
            aCase.unit = unit;
            aCase.unit_id = unit.id;
            aCase.updated_at = updated_at;
            aCase.version = version;
            aCase.worker = worker;
            aCase.workflow_state = WorkFlowState;
            DbContext.cases.Add(aCase);
            DbContext.SaveChanges();

            return aCase;
        }
        public field_values CreateFieldValue(cases aCase, check_lists checkList, fields f, int? ud_id, int? userId, string value, int? version, workers worker)
        {
            field_values fv = new field_values();
            fv.case_id = aCase.id;
            fv.check_list = checkList;
            fv.check_list_id = checkList.id;
            fv.created_at = DateTime.Now;
            fv.date = DateTime.Now;
            fv.done_at = DateTime.Now;
            fv.field = f;
            fv.field_id = f.id;
            fv.updated_at = DateTime.Now;
            if (ud_id != null)
            {
                fv.uploaded_data_id = ud_id;
            }
            fv.user_id = userId;
            fv.value = value;
            fv.version = version;
            fv.worker = worker;
            fv.workflow_state = Constants.WorkflowStates.Created;

            DbContext.field_values.Add(fv);
            DbContext.SaveChanges();
            return fv;
        }
        public check_list_values CreateCheckListValue(cases aCase, check_lists checkList, string status, int? userId, int? version)
        {
            check_list_values CLV = new check_list_values();

            CLV.case_id = aCase.id;
            CLV.check_list_id = checkList.id;
            CLV.created_at = DateTime.Now;
            CLV.status = status;
            CLV.updated_at = DateTime.Now;
            CLV.user_id = userId;
            CLV.version = version;
            CLV.workflow_state = Constants.WorkflowStates.Created;
            
            DbContext.check_list_values.Add(CLV);
            DbContext.SaveChanges();
            return CLV;

        }
        public uploaded_data CreateUploadedData(string checkSum, string currentFile, string extension, string fileLocation, string fileName, short? local, workers worker, string uploaderType, int version)
        {
            uploaded_data UD = new uploaded_data();
            
            UD.checksum = checkSum;
            UD.created_at = DateTime.Now;
            UD.current_file = currentFile;
            UD.expiration_date = DateTime.Now.AddYears(1);
            UD.extension = extension;
            UD.file_location = fileLocation;
            UD.file_name = fileName;
            UD.local = local;
            UD.updated_at = DateTime.Now;
            UD.uploader_id = worker.id;
            UD.uploader_type = uploaderType;
            UD.version = version;
            UD.workflow_state = Constants.WorkflowStates.Created;

            DbContext.uploaded_data.Add(UD);
            DbContext.SaveChanges();
            return UD;
        }
        public entity_groups CreateEntityGroup(string microtingUId, string name, string entityType, string workflowState)
        {

            var lists =  DbContext.entity_groups.Where(x => x.microting_uid == microtingUId).ToList();

            if (lists.Count == 0)
            {
                entity_groups eG = new entity_groups();

                eG.created_at = DateTime.Now;
                //eG.id = xxx;
                eG.microting_uid = microtingUId;
                eG.name = name;
                eG.type = entityType;
                eG.updated_at = DateTime.Now;
                eG.version = 1;
                eG.workflow_state = workflowState;

                DbContext.entity_groups.Add(eG);
                DbContext.SaveChanges();
                return eG;
            } else {
                throw new ArgumentException("microtingUId already exists: " + microtingUId);
            }
            
        }
        public entity_items CreateEntityItem(string description, int displayIndex, string entityGroupId, string entityItemUId, string microtingUId, string name, short? synced, int version, string workflowState)
        {
            entity_items eI = new entity_items();
            eI.created_at = DateTime.Now;
            eI.description = description;
            eI.display_index = displayIndex;
            eI.entity_group_id = entityGroupId;
            eI.entity_item_uid = entityItemUId;
            eI.microting_uid = microtingUId;
            eI.name = name;
            eI.synced = synced;
            eI.updated_at = DateTime.Now;
            eI.version = version;
            eI.workflow_state = workflowState;

            DbContext.entity_items.Add(eI);
            DbContext.SaveChanges();

            return eI;
        }
        public tags CreateTag(string name, string workflowState, int version)
        {
            tags tag = new tags();
            tag.name = name;
            tag.workflow_state = workflowState;
            tag.version = version;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            return tag;
        }
        public check_list_sites CreateCheckListSite(check_lists checklist, DateTime createdAt,
            sites site, DateTime updatedAt, int version, string workflowState, string microting_uid)
        {
            check_list_sites cls = new check_list_sites();
            cls.check_list = checklist;
            cls.created_at = createdAt;
            cls.site = site;
            cls.updated_at = updatedAt;
            cls.version = version;
            cls.workflow_state = workflowState;
            cls.microting_uid = microting_uid;

            DbContext.check_list_sites.Add(cls);
            DbContext.SaveChanges();
            return cls;
        }
        #endregion

        #region
        public string CreateMultiPictureXMLResult(bool create)
        {
            if (create)
            {
                sites site = CreateSite("TestSite1", 12334);
                units unit = CreateUnit(20934, 234234, site, 24234);
                workers worker = CreateWorker("sfsdfsdf23ref@invalid.com", "John", "Doe", 2342341);
                site_workers sw = CreateSiteWorker(242345, site, worker);
                DateTime cl1_ca = DateTime.Now;
                DateTime cl1_ua = DateTime.Now;
                check_lists cl1 = CreateTemplate(cl1_ca, cl1_ua, "MultiPictureXMLResult", "MultiPictureXMLResult_Description", "", "", 0, 0);
                check_lists cl2 = CreateSubTemplate("Sub1", "Sub1Description", "", 0, 0, cl1);
                fields f1 = CreateField(0, "", cl2, Constants.FieldColors.Blue, "", null, "", "PictureDescription", 0, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 0, 0, "Take picture", 0, 0, "", "", 0, 0, "", 0, 0, 0, 0, "", 0);
                check_list_sites cls = CreateCheckListSite(cl1, cl1_ca, site, cl1_ua, 0, Constants.WorkflowStates.Created, "MultiPictureTestInMultipleChecks");
                //returnXML = ;
                return "MultiPictureTestInMultipleChecks";
            } else
            {
                sites site = DbContext.sites.First();
                units unit = DbContext.units.First();
                workers worker = DbContext.workers.First();
                check_lists cl1 = DbContext.check_lists.ToList()[0];
                check_lists cl2 = DbContext.check_lists.ToList()[1];
                fields f1 = DbContext.fields.First();

                #region return xml
                return $@"<?xml version='1.0' encoding='UTF-8'?>
                <Response>
                    <Value type='success'>MultiPictureTestInMultipleChecks</Value>
                    <Checks>
                        <Check worker='John Doe' worker_id='{worker.microting_uid}' date='2018-04-25 14:29:21 +0200' unit_id='{unit.microting_uid}' id='7'>
                            <ElementList>
                                <Id>{cl2.id}</Id>
                                <Status>approved</Status>
                                <DataItemList>
                                    <DataItem>
                                        <Id>{f1.id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                </DataItemList>
                                <ExtraDataItemList></ExtraDataItemList>
                            </ElementList>
                        </Check>
                        <Check worker='John Doe' worker_id='{worker.microting_uid}' date='2018-04-25 14:29:52 +0200' unit_id='{unit.microting_uid}' id='12'>
                            <ElementList>
                                <Id>{cl2.id}</Id>
                                <Status>approved</Status>
                                <DataItemList>
                                    <DataItem>
                                        <Id>{f1.id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                    <DataItem>
                                        <Id>{f1.id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                </DataItemList>
                                <ExtraDataItemList></ExtraDataItemList>
                            </ElementList>
                        </Check>
                        <Check worker='John Doe' worker_id='{worker.microting_uid}' date='2018-04-25 14:39:43 +0200' unit_id='{unit.microting_uid}' id='17'>
                            <ElementList>
                                <Id>{cl2.id}</Id>
                                <Status>approved</Status>
                                <DataItemList>
                                    <DataItem>
                                        <Id>{f1.id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                    <DataItem>
                                        <Id>{f1.id}</Id>
                                        <Geolocation>
                                            <Latitude></Latitude>
                                            <Longitude></Longitude>
                                            <Altitude></Altitude>
                                            <Heading></Heading>
                                            <Accuracy></Accuracy>
                                            <Date></Date>
                                        </Geolocation>
                                        <Value></Value>
                                        <Extension>.jpeg</Extension>
                                        <URL>https://www.microting.com/wp-content/themes/Artificial-Reason-WP/img/close.png</URL>
                                    </DataItem>
                                </DataItemList>
                                <ExtraDataItemList></ExtraDataItemList>
                            </ElementList>
                        </Check>
                    </Checks>
                </Response>".Replace("'", "\"");
                #endregion
            }
            
            
        }
        #endregion
    }
}
