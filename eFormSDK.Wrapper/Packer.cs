using eFormData;
using eFormShared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormSDK.Wrapper
{
    class Packer
    {
        #region Unpackers
        private DataItem UnpackDataItem(JObject dataItemObj)
        {
            DataItem dataItem = dataItemObj.ToObject<DataItem>();
            if (dataItemObj["Type"].ToString() == "Picture")
            {
                Picture picture = new Picture(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, (bool)dataItem.Dummy, 
                    int.Parse(dataItemObj["Multi"].ToString()), bool.Parse(dataItemObj["GeolocationEnabled"].ToString()));
                return picture;
            }
            else if (dataItemObj["Type"].ToString() == "SaveButton")
            {
                SaveButton saveButton = new SaveButton(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, (bool)dataItem.Dummy,
                    dataItemObj["Value"].ToString());
                return saveButton;
            }
            else if (dataItemObj["Type"].ToString() == "Timer")
            {
                Timer timer = new Timer(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, (bool)dataItem.Dummy,
                    bool.Parse(dataItemObj["StopOnSave"].ToString()));
                return timer;
            }
            else if (dataItemObj["Type"].ToString() == "None")
            {
                None none = new None(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, (bool)dataItem.Dummy);
                return none;
            }
            else if (dataItemObj["Type"].ToString() == "Signature")
            {
                Signature signature = new Signature(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, (bool)dataItem.Dummy);
                return signature;
            }
            else if (dataItemObj["Type"].ToString() == "Date")
            {
                Date date = new Date(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, (bool)dataItem.Dummy,
                    DateTime.ParseExact(dataItemObj["MinValue"].ToString(), "yyyy-MM-dd hh:mm:ss", null),
                    DateTime.ParseExact(dataItemObj["MaxValue"].ToString(), "yyyy-MM-dd hh:mm:ss", null),
                    dataItemObj["DefaultValue"].ToString());
                return date;
            }
            else if (dataItemObj["Type"].ToString() == "ShowPdf")
            {
                ShowPdf showPdf = new ShowPdf(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, dataItem.Dummy,
                    dataItemObj["Value"].ToString());
                return showPdf;
            }
            else if (dataItemObj["Type"].ToString() == "CheckBox")
            {
                CheckBox checkBox = new CheckBox(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, dataItem.Dummy,
                    bool.Parse(dataItemObj["DefaultValue"].ToString()), bool.Parse(dataItemObj["Selected"].ToString()));
                return checkBox;
            }
            else if (dataItemObj["Type"].ToString() == "MultiSelect")
            {
                List<KeyValuePair> keyValuePairList = new List<KeyValuePair>();
                foreach (JObject keyValuePairObj in dataItemObj["KeyValuePairList"])
                {
                    KeyValuePair keyValuePair = keyValuePairObj.ToObject<KeyValuePair>();
                    keyValuePairList.Add(keyValuePair);
                }
                MultiSelect multiSelect = new MultiSelect(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, dataItem.Dummy, keyValuePairList);
                return multiSelect;
            }
            else if (dataItemObj["Type"].ToString() == "SingleSelect")
            {
                List<KeyValuePair> keyValuePairList = new List<KeyValuePair>();
                foreach (JObject keyValuePairObj in dataItemObj["KeyValuePairList"])
                {
                    KeyValuePair keyValuePair = keyValuePairObj.ToObject<KeyValuePair>();
                    keyValuePairList.Add(keyValuePair);
                }
                SingleSelect singleSelect = new SingleSelect(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, dataItem.Dummy, keyValuePairList);
                return singleSelect;
            }
            else if (dataItemObj["Type"].ToString() == "Number")
            {
                Number number = new Number(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, dataItem.Dummy,
                    dataItemObj["MinValue"].ToString(), dataItemObj["MaxValue"].ToString(), 
                    int.Parse(dataItemObj["DefaultValue"].ToString()), int.Parse(dataItemObj["DecimalCount"].ToString()),
                    dataItemObj["UnitName"].ToString());
                return number;
            }
            else if (dataItemObj["Type"].ToString() == "Text")
            {
                Text text = new Text(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, dataItem.Dummy,
                    dataItemObj["Value"].ToString(), int.Parse(dataItemObj["MaxLength"].ToString()),
                    bool.Parse(dataItemObj["GeolocationEnabled"].ToString()), bool.Parse(dataItemObj["GeolocationForced"].ToString()),
                    bool.Parse(dataItemObj["GeolocationHidden"].ToString()), bool.Parse(dataItemObj["BarcodeEnabled"].ToString()),
                    dataItemObj["BarcodeType"].ToString());
                return text;
            }
            else if (dataItemObj["Type"].ToString() == "Comment")
            {
                Comment comment = new Comment(dataItem.Id, dataItem.Mandatory, dataItem.ReadOnly, dataItem.Label,
                    dataItem.Description.ToString(), dataItem.Color, dataItem.DisplayOrder, dataItem.Dummy,
                    dataItemObj["Value"].ToString(), int.Parse(dataItemObj["MaxLength"].ToString()),
                    bool.Parse(dataItemObj["SplitScreen"].ToString()));
                return comment;
            }
            else if (dataItemObj["Type"].ToString() == "FieldContainer")
            {
                List<DataItem> dataItemList = new List<DataItem>();
                foreach (JObject diObj in dataItemObj["DataItemList"])
                {
                    DataItem di = UnpackDataItem(diObj);
                    dataItemList.Add(di);
                }
                FieldContainer fieldContainer = new FieldContainer(dataItem.Id, dataItem.Label, dataItem.Description,
                    dataItem.Color, dataItem.DisplayOrder, dataItemObj["Value"].ToString(),dataItemList);
                return fieldContainer;
            }
            return dataItem;
        }

        private DataElement UnpackDataElement(JObject dataElementObj)
        {
            Element element = dataElementObj.ToObject<Element>();
            List<DataItem> dataItemList = new List<DataItem>();
            foreach (JObject dataItemObj in dataElementObj["DataItemList"])
            {
                DataItem dataItem = UnpackDataItem(dataItemObj);
                dataItemList.Add(dataItem);
            }
            List<DataItemGroup> dataItemGroupList = new List<DataItemGroup>();
            DataElement dataElement = new DataElement(element.Id, element.Label, element.DisplayOrder, element.Description.InderValue,
                element.ApprovalEnabled, element.ReviewEnabled, element.DoneButtonEnabled, element.ExtraFieldsEnabled,
                element.PinkBarText, dataItemGroupList, dataItemList);
            return dataElement;
        }

        public MainElement UnpackMainElement(String json)
        {
            MainElement mainElement = JsonConvert.DeserializeObject<MainElement>(json);
            mainElement.ElementList.Clear();

            JObject o = JObject.Parse(json);
            foreach( JObject elementObj in o["ElementList"])
            {
                if (elementObj["Type"].ToString() == "DataElement")
                {
                    DataElement dataElement = UnpackDataElement(elementObj);
                    mainElement.ElementList.Add(dataElement);
                }
            }
            return mainElement;
        }
        
        public List<int> UnpackIntList(String json)
        {
            List<int> result = new List<int>();
            JArray arr = JArray.Parse(json);
            foreach (JToken token in arr)
            {
                string val = token.ToString();
                result.Add(int.Parse(val));
            }
            return result;
        }
        #endregion

        #region Packers
        private void AddTypes(JArray dataItemArray, List<DataItem> dataItemList)
        {
            for (int i = 0; i < dataItemList.Count; i++)
                dataItemArray[i]["Type"] = dataItemList[i].GetType().Name;
        }

        private void AddTypes(JArray dataItemGroupArray, List<DataItemGroup> dataItemGroupList)
        {
            for (int i = 0; i < dataItemGroupList.Count; i++)
                AddTypes(dataItemGroupArray[i]["DataItemList"] as JArray, dataItemGroupList[i].DataItemList);
        }
        
        public string PackCoreElement(CoreElement coreElement)
        {
            JObject coreElementObj = new JObject();
            //string json = JsonConvert.SerializeObject(mainElement);
            if (coreElement is MainElement)
                coreElementObj = JObject.FromObject(coreElement as MainElement);
            else if (coreElement is ReplyElement)
               coreElementObj = JObject.FromObject(coreElement as ReplyElement);
            else
               coreElementObj = JObject.FromObject(coreElement);
            JArray a = coreElementObj["ElementList"] as JArray;
            for (int i = 0; i < coreElement.ElementList.Count; i++)
            {
                if (coreElement.ElementList[i] is DataElement)
                {
                    a[i]["Type"] = "DataElement";
                    AddTypes(a[i]["DataItemList"] as JArray, (coreElement.ElementList[i] as DataElement).DataItemList);
                    AddTypes(a[i]["DataItemGroupList"] as JArray, (coreElement.ElementList[i] as DataElement).DataItemGroupList);
                }
                else if (coreElement.ElementList[i] is CheckListValue)
                {
                    a[i]["Type"] = "CheckListValue";
                    AddTypes(a[i]["DataItemList"] as JArray, (coreElement.ElementList[i] as CheckListValue).DataItemList);
                    AddTypes(a[i]["DataItemGroupList"] as JArray, (coreElement.ElementList[i] as CheckListValue).DataItemGroupList);
                }
            }
            return coreElementObj.ToString();
        }

        public string PackStringList(List<String> stringList)
        {
            if (stringList == null)
            {
                stringList = new List<String>();
            }
            return JArray.FromObject(stringList).ToString();
        }

        public string PackSiteNameDtoList(List<SiteName_Dto> siteNameDtoList)
        {
            return JArray.FromObject(siteNameDtoList).ToString();
        }
        public string PackTemplateDto(Template_Dto templateDto )
        {
            return JObject.FromObject(templateDto).ToString();
        }

        #endregion
    }
}
