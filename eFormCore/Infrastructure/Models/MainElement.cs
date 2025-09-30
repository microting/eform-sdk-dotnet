﻿/*
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
using System.Text;
using System.Xml.Serialization;
using Google.Protobuf;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure.Models.Proto;
using Newtonsoft.Json;

namespace Microting.eForm.Infrastructure.Models
{
    public class CoreElement
    {
        // con
        public CoreElement()
        {
            ElementList = new List<Element>();
        }

        public CoreElement(int id, string label, int displayOrder, string checkListFolderName, int repeated,
            DateTime startDate, DateTime endDate, string language,
            bool multiApproval, bool fastNavigation, bool downloadEntities, bool manualSync, string caseType,
            List<Element> elementList, string color)
        {
            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            CheckListFolderName = checkListFolderName;
            Repeated = repeated;
            StartDate = startDate;
            EndDate = endDate;
            Language = language;
            MultiApproval = multiApproval;
            FastNavigation = fastNavigation;
            DownloadEntities = downloadEntities;
            ManualSync = manualSync;
            CaseType = caseType;
            ElementList = elementList;
            Color = color;
        }
        //

        // var
        public int Id { get; set; }
        public string OriginalId { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }
        public string CheckListFolderName { get; set; }
        public int Repeated { get; set; }
        public int? MicrotingUId { get; set; }

        public string Color { get; set; }
//        public string OriginalId { get; set; }

        [XmlIgnore] public string CaseType { get; set; }

        // public string/DateTime StartDate { get; set; }
        [XmlIgnore] public DateTime StartDate { get; set; }

        [XmlElement("StartDate")]
        public string StartDateString
        {
            get { return StartDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { StartDate = DateTime.Parse(value); }
        }
        //

        // public string/DateTime EndDate { get; set; }
        [XmlIgnore] public DateTime EndDate { get; set; }

        [XmlElement("EndDate")]
        public string EndDateString
        {
            get { return EndDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { EndDate = DateTime.Parse(value); }
        }
        //

        public string Language { get; set; }
        public bool MultiApproval { get; set; }
        public bool FastNavigation { get; set; }
        public bool DownloadEntities { get; set; }
        public bool ManualSync { get; set; }
        public bool EnableQuickSync { get; set; }

        [XmlArray("ElementList"), XmlArrayItem(typeof(Element), ElementName = "Element")]
        public List<Element> ElementList { get; set; }
        //

        public List<DataItem> DataItemGetAll()
        {
            try
            {
                return DataItemGetAllFromList(ElementList, new List<DataItem>());
            }
            catch (Exception ex)
            {
                throw new Exception("DataItemGetAll failed, to get all DataItems", ex);
            }
        }

        private List<DataItem> DataItemGetAllFromList(List<Element> elements, List<DataItem> dataItemLst)
        {
            foreach (Element element in elements)
            {
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;
                    foreach (DataItem item in dataE.DataItemList)
                    {
                        dataItemLst.Add(item);
                    }

                    foreach (DataItemGroup item in dataE.DataItemGroupList)
                    {
                        foreach (DataItem subItem in item.DataItemList)
                        {
                            dataItemLst.Add(subItem);
                        }
                    }
                }

                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;
                    DataItemGetAllFromList(groupE.ElementList, dataItemLst);
                }
            }

            return dataItemLst;
        }

        public List<Element> ElementGetAll()
        {
            return ElementGetAllFromList(ElementList, new List<Element>());
        }

        private List<Element> ElementGetAllFromList(List<Element> elements, List<Element> returnElements)
        {
            foreach (Element element in elements)
            {
                if (element.GetType() == typeof(DataElement))
                {
                    returnElements.Add(element);
                }

                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;
                    ElementGetAllFromList(groupE.ElementList, returnElements);
                }
            }

            return returnElements;
        }
    }

    // MainElement : CoreElement
    [XmlRoot(ElementName = "Main")]
    [Serializable]
    public class MainElement : CoreElement
    {
        // con
        public MainElement()
        {
            ElementList = new List<Element>();
        }

        public MainElement(CoreElement coreElement)
        {
            Id = coreElement.Id;
            Label = coreElement.Label;
            DisplayOrder = coreElement.DisplayOrder;
            CheckListFolderName = coreElement.CheckListFolderName;
            Repeated = coreElement.Repeated;
            StartDate = coreElement.StartDate;
            EndDate = coreElement.EndDate;
            Language = coreElement.Language;
            MultiApproval = coreElement.MultiApproval;
            FastNavigation = coreElement.FastNavigation;
            DownloadEntities = coreElement.DownloadEntities;
            ManualSync = coreElement.ManualSync;
            CaseType = coreElement.CaseType;
            ElementList = coreElement.ElementList;
            EnableQuickSync = coreElement.EnableQuickSync;
            Color = coreElement.Color;

            PushMessageTitle = "";
            PushMessageBody = "";
        }

        public MainElement(int id, string label, int displayOrder, string checkListFolderName, int repeated,
            DateTime startDate, DateTime endDate, string language,
            bool multiApproval, bool fastNavigation, bool downloadEntities, bool manualSync, string caseType,
            string pushMessageTitle, string pushMessageBody, bool enableQuickSync, List<Element> elementList,
            string color)
        {
            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            CheckListFolderName = checkListFolderName;
            Repeated = repeated;
            StartDate = startDate;
            EndDate = endDate;
            Language = language;
            MultiApproval = multiApproval;
            FastNavigation = fastNavigation;
            DownloadEntities = downloadEntities;
            ManualSync = manualSync;
            CaseType = caseType;
            PushMessageTitle = pushMessageTitle;
            PushMessageBody = pushMessageBody;
            EnableQuickSync = enableQuickSync;
            ElementList = elementList;
            Color = color;
        }
        //

        // var
        // public string PushMessageTitle { get; set; }
        private string pushMessageTitle;

        public string PushMessageTitle
        {
            get
            {
                if (pushMessageTitle.Length > 255)
                    //return pushMessageTitle.Substring(0, 255);
                    return pushMessageTitle.AsSpan().Slice(0, 255).ToString();
                return pushMessageTitle;
            }
            set { pushMessageTitle = value; }
        }

        //
        public string PushMessageBody { get; set; }

        public bool BadgeCountEnabled { get; set; }
        //

        // public
        public MainElement XmlToClass(string xmlStr)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(xmlStr);
                XmlSerializer serializer = new XmlSerializer(typeof(MainElement));
                StreamReader reader = new StreamReader(new MemoryStream(byteArray));

                MainElement main = null;
                main = (MainElement)serializer.Deserialize(reader);
                reader.Close();

                return main;
            }
            catch (Exception ex)
            {
                throw new Exception("MainElement failed, to convert XML", ex);
            }
        }

        public MainElement JsonToClass(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<MainElement>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("MainElement failed, to convert Json", ex);
            }
        }

        public string ClassToXml()
        {
            try
            {
                var serializer = new XmlSerializer(GetType());
                using StringWriter writer = new Utf8StringWriter();
                serializer.Serialize(writer, this);
                var xmlStr = writer.ToString();
                return xmlStr;
            }
            catch (Exception ex)
            {
                throw new Exception("MainElement failed to convert Class", ex);
            }
        }

        public string ClassToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(this);
            }
            catch (Exception ex)
            {
                throw new Exception("MainElement failed to convert Class", ex);
            }
        }

        public byte[] ClassToProto()
        {
            try
            {
                var protoMessage = ConvertToProtoMessage();
                return protoMessage.ToByteArray();
            }
            catch (Exception ex)
            {
                throw new Exception("MainElement failed to convert Class to Proto", ex);
            }
        }

        public MainElement ProtoToClass(byte[] protoData)
        {
            try
            {
                var protoMessage = MainElementProto.Parser.ParseFrom(protoData);
                return ConvertFromProtoMessage(protoMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("MainElement failed to convert Proto", ex);
            }
        }

        private MainElementProto ConvertToProtoMessage()
        {
            var proto = new MainElementProto
            {
                Id = this.Id,
                OriginalId = this.OriginalId ?? "",
                Label = this.Label ?? "",
                DisplayOrder = this.DisplayOrder,
                CheckListFolderName = this.CheckListFolderName ?? "",
                Repeated = this.Repeated,
                Color = this.Color ?? "",
                CaseType = this.CaseType ?? "",
                StartDate = this.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = this.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Language = this.Language ?? "",
                MultiApproval = this.MultiApproval,
                FastNavigation = this.FastNavigation,
                DownloadEntities = this.DownloadEntities,
                ManualSync = this.ManualSync,
                EnableQuickSync = this.EnableQuickSync,
                PushMessageTitle = this.PushMessageTitle ?? "",
                PushMessageBody = this.PushMessageBody ?? "",
                BadgeCountEnabled = this.BadgeCountEnabled
            };

            if (this.MicrotingUId.HasValue)
            {
                proto.MicrotingUid = this.MicrotingUId.Value;
            }

            // Convert ElementList (simplified - only handles basic structure)
            if (this.ElementList != null)
            {
                foreach (var element in this.ElementList)
                {
                    proto.ElementList.Add(ConvertElementToProto(element));
                }
            }

            return proto;
        }

        private ElementProto ConvertElementToProto(Element element)
        {
            var proto = new ElementProto
            {
                Id = element.Id,
                Label = element.Label ?? "",
                DisplayOrder = element.DisplayOrder,
                Description = element.Description?.InderValue ?? "",
                ApprovalEnabled = element.ApprovalEnabled,
                ReviewEnabled = element.ReviewEnabled,
                DoneButtonEnabled = element.DoneButtonEnabled,
                ExtraFieldsEnabled = element.ExtraFieldsEnabled,
                PinkBarText = element.PinkBarText ?? "",
                QuickSyncEnabled = element.QuickSyncEnabled,
                OriginalId = element.OriginalId ?? ""
            };

            if (element is DataElement dataElement)
            {
                proto.DataElement = new DataElementProto();
                // Add data items if needed
            }
            else if (element is GroupElement groupElement)
            {
                var groupProto = new GroupElementProto();
                foreach (var childElement in groupElement.ElementList)
                {
                    groupProto.ElementList.Add(ConvertElementToProto(childElement));
                }
                proto.GroupElement = groupProto;
            }

            return proto;
        }

        private MainElement ConvertFromProtoMessage(MainElementProto proto)
        {
            var mainElement = new MainElement
            {
                Id = proto.Id,
                OriginalId = proto.OriginalId,
                Label = proto.Label,
                DisplayOrder = proto.DisplayOrder,
                CheckListFolderName = proto.CheckListFolderName,
                Repeated = proto.Repeated,
                Color = proto.Color,
                CaseType = proto.CaseType,
                StartDate = DateTime.Parse(proto.StartDate),
                EndDate = DateTime.Parse(proto.EndDate),
                Language = proto.Language,
                MultiApproval = proto.MultiApproval,
                FastNavigation = proto.FastNavigation,
                DownloadEntities = proto.DownloadEntities,
                ManualSync = proto.ManualSync,
                EnableQuickSync = proto.EnableQuickSync,
                PushMessageTitle = proto.PushMessageTitle,
                PushMessageBody = proto.PushMessageBody,
                BadgeCountEnabled = proto.BadgeCountEnabled
            };

            if (proto.MicrotingUid != 0)
            {
                mainElement.MicrotingUId = proto.MicrotingUid;
            }

            mainElement.ElementList = new List<Element>();
            foreach (var elementProto in proto.ElementList)
            {
                mainElement.ElementList.Add(ConvertElementFromProto(elementProto));
            }

            return mainElement;
        }

        private Element ConvertElementFromProto(ElementProto proto)
        {
            Element element;

            if (proto.ElementTypeCase == ElementProto.ElementTypeOneofCase.DataElement)
            {
                element = new DataElement
                {
                    DataItemList = new List<DataItem>(),
                    DataItemGroupList = new List<DataItemGroup>()
                };
            }
            else if (proto.ElementTypeCase == ElementProto.ElementTypeOneofCase.GroupElement)
            {
                var groupElement = new GroupElement
                {
                    ElementList = new List<Element>()
                };

                foreach (var childProto in proto.GroupElement.ElementList)
                {
                    groupElement.ElementList.Add(ConvertElementFromProto(childProto));
                }

                element = groupElement;
            }
            else
            {
                element = new DataElement
                {
                    DataItemList = new List<DataItem>(),
                    DataItemGroupList = new List<DataItemGroup>()
                };
            }

            element.Id = proto.Id;
            element.Label = proto.Label;
            element.DisplayOrder = proto.DisplayOrder;
            element.Description = new CDataValue { InderValue = proto.Description };
            element.ApprovalEnabled = proto.ApprovalEnabled;
            element.ReviewEnabled = proto.ReviewEnabled;
            element.DoneButtonEnabled = proto.DoneButtonEnabled;
            element.ExtraFieldsEnabled = proto.ExtraFieldsEnabled;
            element.PinkBarText = proto.PinkBarText;
            element.QuickSyncEnabled = proto.QuickSyncEnabled;
            element.OriginalId = proto.OriginalId;

            return element;
        }
        //

        // private
        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
        //
    }
    //

    // ReplyElement : CoreElement
    public class ReplyElement : CoreElement
    {
        // con
        public ReplyElement()
        {
            ElementList = new List<Element>();
        }

        public ReplyElement(CoreElement coreElement)
        {
            Id = coreElement.Id;
            Label = coreElement.Label;
            DisplayOrder = coreElement.DisplayOrder;
            CheckListFolderName = coreElement.CheckListFolderName;
            Repeated = coreElement.Repeated;
            StartDate = coreElement.StartDate;
            EndDate = coreElement.EndDate;
            Language = coreElement.Language;
            MultiApproval = coreElement.MultiApproval;
            FastNavigation = coreElement.FastNavigation;
            DownloadEntities = coreElement.DownloadEntities;
            ManualSync = coreElement.ManualSync;
            CaseType = coreElement.CaseType;
            ElementList = coreElement.ElementList;
            MicrotingUId = coreElement.MicrotingUId;
//            OriginalId = coreElement.OriginalId;
        }
        //

        // var
        public string Custom { get; set; }
        public DateTime DoneAt { get; set; }
        public int DoneById { get; set; }
        public int UnitId { get; set; }
        public int SiteMicrotingUuid { get; set; }
        public bool JasperExportEnabled { get; set; }

        public bool DocxExportEnabled { get; set; }
//        public string OriginalId { get; set; }
        //
    }
    //
}