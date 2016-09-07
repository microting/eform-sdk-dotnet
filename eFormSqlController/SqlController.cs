using eFormRequest;
using eFormResponse;
using MiniTrols;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace eFormSqlController
{
    public class SqlController
    {
        #region var
        SqlConnection connect;
        SqlCommand command;
        List<Holder> converter;

        private Tools tool = new Tools();

        object _lockQuery = new object();
        string dbName = "TestDB";
        #endregion

        #region con
        public SqlController(string connectionString)
        {
            connect = new SqlConnection(connectionString.Trim() + "; MultipleActiveResultSets=true");
            connect.Open();
        }
        #endregion

        #region public
        public int           EformCreate(MainElement mainElement)
        {
            int id = 0;
            id = EformCreateDb(mainElement, "");

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

        public int           CaseCreate(string checkListId, string microtingApiId, string siteId, string numberPlate, string roadNumber)
        {
            string sqlQuery = "INSERT INTO [" + dbName + "].[microting].[cases] ([status],[created_by_user_id],[type],[created_at],[updated_at],[check_list_id],[microting_api_id],[workflow_state]," +
                "[version],[site_id],[reg_number],[vejenummer]) VALUES ('" +

                "66" + "','" +
                "7" + "','" +
                "WasteControlCase" + "','" +
                tool.Now() + "','" +
                tool.Now() + "','" +
                checkListId + "','" +
                microtingApiId + "','" +
                "created" + "','" +
                "1" + "','" +
                siteId + "','" +
                numberPlate + "','" +
                roadNumber + "')" +

                " SELECT SCOPE_IDENTITY();";

            string caseId = QueryDbSimple(sqlQuery);
            if (caseId == "")
                throw new Exception("Was unable to create a case in Cases");

            return int.Parse(caseId);
        }

        public void          CaseUpdate(string microtingApiId, string dateOfDoing, string doneByUserId, string microtingCheckId, string unitId)
        {
            int version = int.Parse(QueryDbSimple("SELECT [version] FROM [" + dbName + "].[microting].[cases] WHERE [microting_api_id] = '" + microtingApiId + "';"));
            version++;

            string sqlQuery = "UPDATE [" + dbName + "].[microting].[cases] SET " +
                "[status] = '100'," +
                "[date_of_doing] = '" + dateOfDoing + "'," +
                "[done_by_user_id] = '" + doneByUserId + "'," +
                "[workflow_state] = '" + "created" + "'," +
                "[version] = '" + version.ToString() + "'," +
                "[microting_check_id] = '" + microtingCheckId + "'," +
                "[unit_id] = '" + unitId + "' " +

                "WHERE [microting_api_id] = '" + microtingApiId + "'";

            SqlDataReader sR = QueryDb(sqlQuery);
            while (sR.Read())
            {
                if (sR.RecordsAffected != 1)
                    throw new Exception("Was unable to update " + microtingApiId + " case in Cases");
            }
        }

        public List<string>  CaseFindMatchs(string microtingApiId)
        {
            List<string> lst = new List<string>();

            try
            {
                SqlDataReader sR = QueryDb("DECLARE @var nvarchar(255); SELECT @var = [vejenummer] FROM [" + dbName + "].[microting].[cases] WHERE [microting_api_id] = '" + microtingApiId +
                    "'; SELECT[site_id],[microting_api_id] FROM [" + dbName + "].[microting].[cases] WHERE [vejenummer] = @var;");
                while (sR.Read())
                {
                    lst.Add("{" + Str(sR, "site_id") + "}[" + Str(sR, "microting_api_id") + "]");
                }
                sR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst;
        }

        public void          CaseDelete(string microtingApiId)
        {
            try
            {
                int version = int.Parse(QueryDbSimple("SELECT [version] FROM [" + dbName + "].[microting].[cases] WHERE [microting_api_id] = '" + microtingApiId + "';"));
                version++;

                string sqlQuery = "UPDATE [" + dbName + "].[microting].[cases] SET " +
                    "[updated_at] = '" + tool.Now() + "'," +
                    "[workflow_state] = '" + "removed" + "'," +
                    "[version] = '" + version.ToString() + "'," +
                    "[removed_by_user_id] = '" + "7" + "' " +

                    "WHERE [microting_api_id] = '" + microtingApiId + "'";

                SqlDataReader sR = QueryDb(sqlQuery);
                while (sR.Read())
                {
                    if (sR.RecordsAffected != 1)
                        throw new Exception("Was unable to \"deleted\" " + microtingApiId + " case in Cases");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int           NotificationCreate(string microting_uuid, string content)
        {
            string sqlQuery = "INSERT INTO [" + dbName + "].[microting].[notifications] ([microting_uuid],[content],[workflow_state],[created_at],[updated_at]) VALUES ('" +

                microting_uuid + "','" +
                content + "','" +
                "created" + "','" +
                tool.Now() + "','" +
                tool.Now() + "')" +

                " SELECT SCOPE_IDENTITY();";

            string caseId = QueryDbSimple(sqlQuery);
            if (caseId == "")
                throw new Exception("Was unable to create a case in Cases");

            return int.Parse(caseId);
        }

        public string        NotificationRead()
        {
            string content = "";
            string sqlQuery = "SELECT TOP 1 [content] FROM [" + dbName + "].[microting].[notifications] WHERE [workflow_state] = 'created';";

            SqlDataReader sR = QueryDb(sqlQuery);
            while (sR.Read())
            {
                content = Str(sR, "content");
            }
            return content;
        }

        public void          NotificationProcessed(string content)
        {
            string sqlQuery = "UPDATE [" + dbName + "].[microting].[notifications] SET " +
                "[workflow_state] = '" + "processed" + "', " +
                "[updated_at] = '" + tool.Now() + "'" +
                "WHERE [content] = '" + content + "'";

            SqlDataReader sR = QueryDb(sqlQuery);
            while (sR.Read())
            {
                if (sR.RecordsAffected != 1)
                    throw new Exception("Was unable to update " + content + " notification in DB");
            }
        }

        public string        FileRead()
        {
            string fileLocation = "";
            string sqlQuery = "SELECT TOP 1 [file_location] FROM [" + dbName + "].[microting].[uploaded_data] WHERE [workflow_state] = 'pre_created';";

            SqlDataReader sR = QueryDb(sqlQuery);
            while (sR.Read())
            {
                fileLocation = Str(sR, "file_location");
            }
            return fileLocation;
        }

        public void          FileProcessed(string urlStr, string chechSum, string fileLocation, string fileName)
        {
            string sqlQuery = "UPDATE [" + dbName + "].[microting].[uploaded_data] SET " +
                "[checksum] = '" + chechSum + "', " +
                "[file_location] = '" + fileLocation + "', " +
                "[file_name] = '" + fileName + "', " +
                "[local] = '" + "1" + "', " +
                "[workflow_state] = '" + "created" + "', " +
                "[updated_at] = '" + tool.Now() + "', " +
                "[version] = '" + "2" + "' " +
                "WHERE [file_location] = '" + urlStr + "'";

            SqlDataReader sR = QueryDb(sqlQuery);
            while (sR.Read())
            {
                if (sR.RecordsAffected != 1)
                    throw new Exception("Was unable to update " + urlStr + " file_location in DB");
            }
        }

        public List<string>  SitesList()
        {
            List<string> lst = new List<string>();

            try
            {
                SqlDataReader sR = QueryDb("SELECT [microting_uuid] FROM [" + dbName + "].[microting].[sites]");
                while (sR.Read())
                {
                    lst.Add(Str(sR, "microting_uuid"));
                }
                sR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst;
        }
        #endregion

        #region private
        #region EformCreateDb
        //TODO rollback for failed inserts ?

        private int EformCreateDb(MainElement mainElement, string caseType)
        {
            try
            {
                #region get converter
                converter = new List<Holder>();

                SqlDataReader sR = QueryDb("SELECT * FROM [" + dbName + "].[microting].[field_types]");
                while (sR.Read())
                {
                    converter.Add(new Holder(Int(sR, "id"), Str(sR, "field_type")));
                }
                sR.Close();
                #endregion

                #region mainElement
                //KEY POINT - mapping
                string sqlQuery = "INSERT INTO [" + dbName + "].[microting].[check_lists] ([created_at],[updated_at],[text],[description],[workflow_state],[parent_id],[repeated],[version],"+
                    "[case_type],[folder_name],[display_index],[report_file_name],[review_enabled],[manual_sync],[extra_fields_enabled],[done_button_enabled]," +
                    "[approval_enabled],[multi_approval],[fast_navigation],[download_entities]) VALUES ('" +

                    tool.Now() + "','" +
                    tool.Now() + "','" +
                    mainElement.Label + "','" +
                    "','" + //description - used for non-mainElements
                    "created','" + //workflow
                    "0','" + //mainElements never have parents ;)
                    mainElement.Repeated + "','" +
                    "1','" + //always version 1, or it's an update

                    caseType + "','" +
                    mainElement.CheckListFolderName + "','" +
                    mainElement.DisplayOrder + "','" +
                    "','" + //report_file_name - Ruby colume
                    "0','" + //review_enabled - used for non-mainElements
                    Bool(mainElement.ManualSync) + "','" +
                    "0','" + //extra_fields_enabledreview_enabled - used for non-mainElements
                    "0','" + //done_button_enabled - used for non-mainElements

                    "0','" + //approval_enabled - used for non-mainElements
                    Bool(mainElement.MultiApproval) + "','" +
                    Bool(mainElement.FastNavigation) + "','" +
                    Bool(mainElement.DownloadEntities) + "')" +
                    
                    " SELECT SCOPE_IDENTITY();";

                string mainId = QueryDbSimple(sqlQuery);
                mainElement.Id = mainId;
                if (mainId == "")
                        throw new Exception("Was unable to create a MainElement in Check_lists");
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

                return int.Parse(mainId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateDataElement(string mainId, DataElement dataElement)
        {
            //KEY POINT - mapping
            string sqlQuery = "INSERT INTO [" + dbName + "].[microting].[check_lists] ([created_at],[updated_at],[text],[description],[workflow_state],[parent_id],[repeated],[version]," +
                "[case_type],[folder_name],[display_index],[report_file_name],[review_enabled],[manual_sync],[extra_fields_enabled],[done_button_enabled]," +
                "[approval_enabled],[multi_approval],[fast_navigation],[download_entities]) VALUES ('" +

                tool.Now() + "','" +
                tool.Now() + "','" +
                dataElement.Label + "','" +
                dataElement.Description + "','" +
                "created','" + //workflow
                mainId + "','" + //mainElement's Id
                "0','" + //repeated - used for mainElements
                "1','" + //always version 1, or it's an update

                "','" + //caseType - used for mainElements
                "0','" + //checkListFolderName - used for mainElements
                dataElement.DisplayOrder + "','" +
                "','" + //report_file_name - Ruby colume
                Bool(dataElement.ReviewEnabled) + "','" +
                "0','" + //manualSync - used for mainElements
                Bool(dataElement.ExtraFieldsEnabled) + "','" +
                Bool(dataElement.DoneButtonEnabled) + "','" +

                Bool(dataElement.ApprovalEnabled) + "','" +
                "0','" + //MultiApproval - used for mainElements
                "0','" + //FastNavigation - used for mainElements
                "0')" + //DownloadEntities - used for mainElements

                " SELECT SCOPE_IDENTITY();";

            string elementId = QueryDbSimple(sqlQuery);
            if (elementId == "")
                throw new Exception("Was unable to create an Element in Check_lists");

            foreach (eFormRequest.DataItem dataItem in dataElement.DataItemList)
            {
                CreateDataItem(elementId, dataItem);
            }
        }

        private void CreateDataItem(string elementId, eFormRequest.DataItem dataItem)
        {
            string typeStr = dataItem.GetType().ToString().Remove(0, 13).Replace("_",""); //13 = "eFormRequest.".Length
            int fieldTypeId = Find(typeStr);

            Field field = new Field();

            field.color = dataItem.Color;
            field.description = dataItem.Description;
            field.display_index = dataItem.DisplayOrder.ToString();
            field.text = dataItem.Label;
            field.mandatory = Bool(dataItem.Mandatory);
            field.read_only = Bool(dataItem.ReadOnly);

            field.created_at = tool.Now();
            field.updated_at = tool.Now();
            field.workflow_state = "created";
            field.check_list_id = elementId;
            field.field_type_id = fieldTypeId.ToString();
            field.version = "1";

            #region dataItem type
            //KEY POINT - mapping
            switch (typeStr) //"eFormRequest.".Length
            {
                case "Text":
                    Text text = (Text)dataItem;
                    field.default_value = text.Value;
                    field.max_length = text.MaxLength.ToString();
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
                    field.decimal_count = number.DecimalCount.ToString();
                    field.unit_name = number.UnitName;
                    break;

                case "None":
                    break;

                case "CheckBox":
                    Check_Box checkBox = (Check_Box)dataItem;
                    field.default_value = Bool(checkBox.DefaultValue);
                    field.selected = Bool(checkBox.Selected);
                    break;

                case "Picture":
                    Picture picture = (Picture)dataItem;
                    field.multi = picture.Multi.ToString();
                    field.geolocation_enabled = Bool(picture.GeolocationEnabled);
                    break;

                case "Audio":
                    Audio audio = (Audio)dataItem;
                    field.multi = audio.Multi.ToString();
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
                    field.max_length = comment.Maxlength.ToString();
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

            #region insert
            //KEY POINT - mapping
            string sqlQuery = "INSERT INTO [" + dbName + "].[microting].[fields] ([created_at],[updated_at],[workflow_state],[check_list_id],[text],[description],[field_type_id]," +
                "[display_index],[version],[parent_field_id],[multi],[default_value],[selected],[min_value],[max_value],[max_length],[split_screen],[decimal_count],[unit_name]," +
                "[key_value_pair_list],[geolocation_enabled],[geolocation_forced],[geolocation_hidden],[stop_on_save],[mandatory],[read_only],[color],[barcode_enabled],[barcode_type]) VALUES ('" +

                field.created_at + "','" +
                field.updated_at + "','" +
                field.workflow_state + "','" +
                field.check_list_id + "','" +
                field.text + "','" +
                field.description + "','" +
                field.field_type_id + "','" +
                field.display_index + "','" +
                field.version + "','" +
                field.parent_field_id + "','" +
                field.multi + "','" +
                field.default_value + "','" +
                field.selected + "','" +
                field.min_value + "','" +
                field.max_value + "','" +
                field.max_length + "','" +
                field.split_screen + "','" +
                field.decimal_count + "','" +
                field.unit_name + "','" +
                field.key_value_pair_list + "','" +
                field.geolocation_enabled + "','" +
                field.geolocation_forced + "','" +
                field.geolocation_hidden + "','" +
                field.stop_on_save + "','" +
                field.mandatory + "','" +
                field.read_only + "','" +
                field.color + "','" +
                field.barcode_enabled + "','" +
                field.barcode_type + "')" +

                " SELECT SCOPE_IDENTITY();";

            sqlQuery = sqlQuery.Replace("'null'", "null");
            string dataItemId = QueryDbSimple(sqlQuery);
            if (dataItemId == "")
                throw new Exception("Was unable to create an Element in Check_lists");
            #endregion;
        }
        #endregion

        #region EformReadDb
        private MainElement EformReadDb(int templatId)
        {
            string mainId = templatId.ToString();
            MainElement mainElement = null;
            try
            {
                #region get converter
                converter = new List<Holder>();

                SqlDataReader sR = QueryDb("SELECT * FROM [" + dbName + "].[microting].[field_types]");
                while (sR.Read())
                {
                    converter.Add(new Holder(Int(sR,"id"), Str(sR,"field_type")));
                }
                sR.Close();
                #endregion

                #region get eForm
                //KEY POINT - mapping
                //getting mainElement
                sR = QueryDb("SELECT * FROM [" + dbName + "].[microting].[check_lists] WHERE [id] = '" + mainId + "'");
                while (sR.Read())
                {
                    mainId = Str(sR,"id");
                    mainElement = new MainElement(mainId, Str(sR, "description"), Int(sR, "display_index"), Str(sR, "folder_name"), Int(sR, "repeated"), DateTime.Now, DateTime.Now.AddDays(2), "da", Bool(sR, "multi_approval"), Bool(sR, "fast_navigation"), Bool(sR, "download_entities"), Bool(sR, "manual_sync"), new List<Element>());
                }
                sR.Close();


                //KEY POINT - mapping
                //getting elements
                sR = QueryDb("SELECT [id] FROM [" + dbName + "].[microting].[check_lists] WHERE [parent_id] = '" + mainId + "'");
                while (sR.Read())
                {
                    string elemId = Str(sR, "id");
                    mainElement.ElementList.Add(GetElement(elemId));
                }
                sR.Close();
                #endregion

                return mainElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Element GetElement(string ownId)
        {
            Element element = null;

            SqlDataReader sR  = QueryDb("SELECT [id] FROM [" + dbName + "].[microting].[check_lists] WHERE [parent_id] = '" + ownId + "'");

            if (sR.HasRows)
            {
                //GroupElement

                throw new NotImplementedException("TODO - Here be dragons");
                //while (sR.Read())
                //{
                //    SqlDataReader 

                //    sR = QueryDb("SELECT * FROM [" + dbName + "].[microting].[check_lists] WHERE [parent_id] = '" + ownId + "'");
                //    //TODO Lacks GroupElements and more levels...
                //    string groupId = Str(sR, "id");
                //    List<Element> lst = new List<Element>();
                //    element = new GroupElement(groupId, Str(sR, "text"), Int(sR, "display_index"), Str(sR, "description"), Bool(sR, "approval_enabled"), Bool(sR, "review_enabled"), Bool(sR, "done_button_enabled"), Bool(sR, "extra_fields_enabled"), "", lst);

                //    GetElement(groupId, lst);
                //}
                //sR.Close();
            }
            else
            {
                //DataElement

                sR.Close();

                List<eFormRequest.DataItem> lst = new List<eFormRequest.DataItem>();


                sR = QueryDb("SELECT * FROM [" + dbName + "].[microting].[check_lists] WHERE [id] = '" + ownId + "'");
                while (sR.Read())
                {
                    element = new DataElement(Str(sR, "id"), Str(sR, "text"), Int(sR, "display_index"), Str(sR, "description"), Bool(sR, "approval_enabled"), Bool(sR, "review_enabled"), Bool(sR, "done_button_enabled"), Bool(sR, "extra_fields_enabled"), "", lst);
                }
                sR.Close();


                sR = QueryDb("SELECT * FROM [" + dbName + "].[microting].[fields] WHERE [check_list_id] = '" + ownId + "'");
                while (sR.Read())
                {
                    lst.Add(GetDataItem(sR));
                }
                sR.Close();
            }

            return element;
        }

        private eFormRequest.DataItem GetDataItem(SqlDataReader sR)
        {
            int fieldTypeId = Int(sR, "field_type_id");
            string fieldTypeStr = Find(fieldTypeId);

            //KEY POINT - mapping
            switch (fieldTypeStr)
            {
                case "Text":
                    return new Text(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Str(sR, "default_value"), Int(sR, "max_length"), Bool(sR, "geolocation_enabled"), Bool(sR, "geolocation_forced"), Bool(sR, "geolocation_hidden"), Bool(sR, "barcode_enabled"), Str(sR, "barcode_type"));

                case "Number":
                    return new Number(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Int(sR, "min_value"), Int(sR, "max_value"), Int(sR, "default_value"), Int(sR, "decimal_count"), Str(sR, "unit_name"));

                case "None":
                    return new None(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"));

                case "CheckBox":
                    return new Check_Box(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Bool(sR, "default_value"), Bool(sR, "selected"));

                case "Picture":
                    return new Picture(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Int(sR, "multi"), Bool(sR, "geolocation_enabled"));

                case "Audio":
                    return new Audio(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Int(sR, "multi"));

                case "SingleSelect":
                    return new Single_Select(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        ReadPairs(Str(sR, "key_value_pair_list")));

                case "MultiSelect":
                    return new Multi_Select(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        ReadPairs(Str(sR, "key_value_pair_list")));

                case "Comment":
                    return new Comment(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Str(sR, "default_value"), Int(sR, "max_length"), Bool(sR, "split_screen"));

                case "Date":
                    return new Date(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Date(sR, "min_value"), Date(sR, "max_value"), Str(sR, "default_value"));

                case "Signature":
                    return new Signature(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"));

                case "Timer":
                    return new eFormRequest.Timer(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                        Bool(sR, "stop_on_save"));

                case "EntitySelect":
                    throw new NotImplementedException("TODO");
                    //return new Entity_Select(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                    //    0); //TODO

                case "EntitySearch":
                    throw new NotImplementedException("TODO");
                    //return new Entity_Search(Str(sR, "id"), Bool(sR, "mandatory"), Bool(sR, "read_only"), Str(sR, "text"), Str(sR, "description"), Str(sR, "color"), Int(sR, "display_index"),
                    //    0); //TODO

                default:
                    throw new IndexOutOfRangeException(fieldTypeId + " is not a known/mapped DataItem type");
            }
        }
        #endregion

        #region EformCheckCreateDb
        //TODO rollback for failed inserts ?

        private void         EformCheckCreateDb(Response response, string xml)
        {
            try
            {
                string elementId = "";
                List<string> elements = tool.LocateList(xml, "<ElementList>", "</ElementList>");

                foreach (string elementStr in elements)
                {
                    CheckListValues chkLstV = new CheckListValues();

                    chkLstV.created_at = tool.Now();
                    chkLstV.updated_at = tool.Now();
                    chkLstV.check_list_id = tool.Locate(elementStr, "<Id>", "</");
                    chkLstV.case_id = QueryDbSimple("SELECT [id] FROM [" + dbName + "].[microting].[cases] WHERE [microting_api_id] = '" + response.Value + "';");
                    chkLstV.status = tool.Locate(elementStr, "<Status>", "</");
                    chkLstV.version = "1";
                    chkLstV.user_id = response.Checks[0].WorkerId;
                    chkLstV.workflow_state = "created";

                    string sqlQuery = "INSERT INTO [" + dbName + "].[microting].[check_list_values] ([created_at],[updated_at],[check_list_id],[case_id],[status],[version],[user_id],[workflow_state],[check_list_duplicate_id]) VALUES ('" +
                        chkLstV.created_at + "','" +
                        chkLstV.updated_at + "','" +
                        chkLstV.check_list_id + "','" +
                        chkLstV.case_id + "','" +
                        chkLstV.status + "','" +
                        chkLstV.version + "','" +
                        chkLstV.user_id + "','" +
                        chkLstV.workflow_state + "','" +
                        chkLstV.check_list_duplicate_id + "')" +

                        " SELECT SCOPE_IDENTITY();";

                    sqlQuery = sqlQuery.Replace("'null'", "null");
                    elementId = QueryDbSimple(sqlQuery);
                    if (elementId == "")
                        throw new Exception("Was unable to create an Element in check_list_values");


                    #region foreach dataItem
                    List<string> dataItems = tool.LocateList(elementStr, "<DataItem>", "</DataItem>");

                    foreach (string dataItemStr in dataItems)
                    {
                        CreateDataItemCheck(dataItemStr, chkLstV.case_id, chkLstV.user_id, elementId, response.Checks[0].Date);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void         CreateDataItemCheck(string xml, string caseId, string userId, string elementId, string dateOfDoing)
        {
            FieldValues fieldV = new FieldValues();

            #region if contains a file
            if (tool.Locate(xml, "<URL>", "</URL>") != "")
            {
                string sqlQueryUpload = "INSERT INTO [" + dbName + "].[microting].[uploaded_data] ([created_at],[updated_at],[extension],[uploader_id],[uploader_type]," +
                    "[workflow_state],[version],[local],[file_location]) VALUES ('" +

                    tool.Now() + "','" +
                    tool.Now() + "','" +
                    tool.Locate(xml, "<Extension>", "</") + "','" +
                    userId + "','" +
                    "system" + "','" +
                    "pre_created" + "','" +
                    "1" + "','" +
                    "0" + "','" +
                    tool.Locate(xml, "<URL>", "</") + "')" +

                    " SELECT SCOPE_IDENTITY();";

                sqlQueryUpload = sqlQueryUpload.Replace("'null'", "null");
                string urlId = QueryDbSimple(sqlQueryUpload);
                if (urlId == "")
                    throw new Exception("Was unable to create an Data in uploaded_data");

                fieldV.uploaded_data_id = urlId;
            }
            #endregion

            fieldV.created_at = tool.Now();
            fieldV.updated_at = tool.Now();
            fieldV.value = tool.Locate(xml, "<Value>", "</");
            //geo
                fieldV.latitude = tool.Locate(xml, "<Latitude>", "</");
                fieldV.longitude = tool.Locate(xml, "<Longitude>", "</");
                fieldV.altitude = tool.Locate(xml, "<Altitude>", "</");
                fieldV.heading = tool.Locate(xml, "<Heading>", "</");
                fieldV.accuracy = tool.Locate(xml, "<Accuracy>", "</");
                fieldV.date = tool.Locate(xml, "<Date>", "</");
            //
            fieldV.workflow_state = "created";
            fieldV.version = "1";
            fieldV.case_id = caseId;
            fieldV.field_id = tool.Locate(xml, "<Id>", "</");
            fieldV.user_id = userId;
            fieldV.check_list_id = elementId;
            fieldV.date_of_doing = dateOfDoing;

            string sqlQuery = "INSERT INTO [" + dbName + "].[microting].[field_values] ([created_at],[updated_at],[value],[latitude],[longitude],[altitude],[heading],[date],[accuracy]," + 
                "[uploaded_data_id],[workflow_state],[version],[case_id],[field_id],[user_id],[check_list_id],[check_list_duplicate_id],[date_of_doing]) VALUES ('" +

                fieldV.created_at + "','" +
                fieldV.updated_at + "','" +
                fieldV.value + "','" +
                fieldV.latitude + "','" +
                fieldV.longitude + "','" +
                fieldV.altitude + "','" +
                fieldV.heading + "','" +
                fieldV.date + "','" +
                fieldV.accuracy + "','" +
                fieldV.uploaded_data_id + "','" +
                fieldV.workflow_state + "','" +
                fieldV.version + "','" +
                fieldV.case_id + "','" +
                fieldV.field_id + "','" +
                fieldV.user_id + "','" +
                fieldV.check_list_id + "','" +
                fieldV.check_list_duplicate_id + "','" +
                fieldV.date_of_doing + "')" +

                " SELECT SCOPE_IDENTITY();";

            sqlQuery = sqlQuery.Replace("'null'", "null");
            string fieldId = QueryDbSimple(sqlQuery);
            if (fieldId == "")
                throw new Exception("Was unable to create an DataItem in field_values");
        }
        #endregion

        #region Get*
        private string Str(SqlDataReader sqlReader, string columeName)
        {
            return sqlReader[columeName].ToString();
        }

        private bool Bool(SqlDataReader sqlReader, string columeName)
        {
            string temp = sqlReader[columeName].ToString();
            if (temp == "0" || temp == "false" || temp == "")
                return false;
            if (temp == "1" || temp == "true")
                return true;
            throw new Exception(temp + ": was not found to be a bool");
        }

        private string Bool(bool inputBool)
        {
            if (inputBool == false)
                return "0";
            else
                return "1";
        }

        private int Int(SqlDataReader sqlReader, string columeName)
        {
            var temp = sqlReader[columeName];
            string str = temp.ToString();
            if (str == "")
                return 0;
            return int.Parse(str);
        }

        private DateTime Date(SqlDataReader sqlReader, string columeName)
        {
            string temp = sqlReader[columeName].ToString();
            return DateTime.Parse(temp);
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

        #region Query
        private string QueryDbSimple(string query)
        {
            try
            {
                lock (_lockQuery)
                {
                    SqlDataReader sqlReader = QueryDb(query);
                    while (sqlReader.Read())
                    {
                        var strMaybe = sqlReader.GetValue(0);
                        sqlReader.Close();

                        return strMaybe.ToString();
                    }
                    throw new Exception("No valid reply from DB");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QueryDbSimple failed '" + query + "'", ex);
            }
        }

        private SqlDataReader QueryDb(string query)
        {
            try
            {
                lock (_lockQuery)
                {
                    command = new SqlCommand(query, connect);
                    SqlDataReader sR = command.ExecuteReader();
                    command.Dispose();

                    return sR;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QueryDb failed '" + query + "'", ex);
            }
        }
        #endregion
        #endregion
    }

    #region help classes
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

    internal class Field
    {
        internal Field()
        {
            created_at = "null";
            updated_at = "null";
            workflow_state = "null";
            check_list_id = "null";
            text = "null";
            description = "null";
            field_type_id = "null";
            display_index = "null";
            version = "null";
            parent_field_id = "null";
            multi = "null";
            default_value = "null";
            selected = "null";
            min_value = "null";
            max_value = "null";
            max_length = "null";
            split_screen = "null";
            decimal_count = "null";
            unit_name = "null";
            key_value_pair_list = "null";
            geolocation_enabled = "null";
            geolocation_forced = "null";
            geolocation_hidden = "null";
            stop_on_save = "null";
            mandatory = "null";
            read_only = "null";
            color = "null";
            barcode_enabled = "null";
            barcode_type = "null";
        }

        #region get
        internal string created_at { get; set; }
        internal string updated_at { get; set; }
        internal string workflow_state { get; set; }
        internal string check_list_id { get; set; }
        internal string text { get; set; }
        internal string description { get; set; }
        internal string field_type_id { get; set; }
        internal string display_index { get; set; }
        internal string version { get; set; }
        internal string parent_field_id { get; set; }
        internal string multi { get; set; }
        internal string default_value { get; set; }
        internal string selected { get; set; }
        internal string min_value { get; set; }
        internal string max_value { get; set; }
        internal string max_length { get; set; }
        internal string split_screen { get; set; }
        internal string decimal_count { get; set; }
        internal string unit_name { get; set; }
        internal string key_value_pair_list { get; set; }
        internal string geolocation_enabled { get; set; }
        internal string geolocation_forced { get; set; }
        internal string geolocation_hidden { get; set; }
        internal string stop_on_save { get; set; }
        internal string mandatory { get; set; }
        internal string read_only { get; set; }
        internal string color { get; set; }
        internal string barcode_enabled { get; set; }
        internal string barcode_type { get; set; }
        #endregion
    }

    internal class CheckListValues
    {
        internal CheckListValues()
        {
            created_at = "null";
            updated_at = "null";
            check_list_id = "null";
            case_id = "null";
            status = "null";
            version = "null";
            user_id = "null";
            workflow_state = "null";
            check_list_duplicate_id = "null";
        }

        internal string created_at { get; set; }
        internal string updated_at { get; set; }
        internal string check_list_id { get; set; }
        internal string case_id { get; set; }
        internal string status { get; set; }
        internal string version { get; set; }
        internal string user_id { get; set; }
        internal string workflow_state { get; set; }
        internal string check_list_duplicate_id { get; set; }
    }

    internal class FieldValues
    {
        internal FieldValues()
        {
            created_at = "null";
            updated_at = "null";
            value = "null";
            latitude = "null";
            longitude = "null";
            altitude = "null";
            heading = "null";
            date = "null";
            accuracy = "null";
            uploaded_data_id = "null";
            workflow_state = "null";
            version = "null";
            case_id = "null";
            field_id = "null";
            user_id = "null";
            check_list_id = "null";
            check_list_duplicate_id = "null";
            date_of_doing = "null";
        }

        internal string created_at { get; set; }
        internal string updated_at { get; set; }
        internal string value { get; set; }
        internal string latitude { get; set; }
        internal string longitude { get; set; }
        internal string altitude { get; set; }
        internal string heading { get; set; }
        internal string date { get; set; }
        internal string accuracy { get; set; }
        internal string uploaded_data_id { get; set; }
        internal string workflow_state { get; set; }
        internal string version { get; set; }
        internal string case_id { get; set; }
        internal string field_id { get; set; }
        internal string user_id { get; set; }
        internal string check_list_id { get; set; }
        internal string check_list_duplicate_id { get; set; }
        internal string date_of_doing { get; set; }
    }
    #endregion
}
