using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace eFormRequest
{
    public class CoreElement
    {
        #region con
        public CoreElement()
        {
            ElementList = new List<Element>();
        }

        public CoreElement(int id, string label, int displayOrder, string checkListFolderName, int repeated, DateTime startDate, DateTime endDate, string language,
            bool multiApproval, bool fastNavigation, bool downloadEntities, bool manualSync, string caseType, List<Element> elementList)
        {
            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            CheckListFolderName = checkListFolderName;
            Repeated = repeated;
            SetStartDate(startDate);
            SetEndDate(endDate);
            Language = language;
            MultiApproval = multiApproval;
            FastNavigation = fastNavigation;
            DownloadEntities = downloadEntities;
            ManualSync = manualSync;
            CaseType = caseType;
            ElementList = elementList;
        }
        #endregion

        #region var
        public int Id { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }
        public string CheckListFolderName { get; set; }
        public int Repeated { get; set; }

        [XmlIgnore]
        public string CaseType { get; set; }

        #region public string/DateTime StartDate { get; set; }
        public string StartDate { get; set; }

        public DateTime GetStartDate()
        {
            return DateTime.Parse(StartDate);
        }

        public void SetStartDate(DateTime dateTime)
        {
            StartDate = dateTime.ToString();
        }
        #endregion

        #region public string/DateTime EndDate { get; set; }
        public string EndDate { get; set; }

        public DateTime GetEndDate()
        {
            return DateTime.Parse(EndDate);
        }

        public void SetEndDate(DateTime dateTime)
        {
            EndDate = dateTime.ToString();
        }
        #endregion

        public string Language { get; set; }
        public bool MultiApproval { get; set; }
        public bool FastNavigation { get; set; }
        public bool DownloadEntities { get; set; }
        public bool ManualSync { get; set; }

        [XmlArray("ElementList"), XmlArrayItem(typeof(Element), ElementName = "Element")]
        public List<Element> ElementList { get; set; }
        #endregion
    }

    #region MainElement : CoreElement
    [XmlRoot(ElementName = "Main")]
    [Serializable()]
    public class MainElement : CoreElement
    {
        #region con
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
            SetStartDate(coreElement.GetStartDate());
            SetEndDate(coreElement.GetEndDate());
            Language = coreElement.Language;
            MultiApproval = coreElement.MultiApproval;
            FastNavigation = coreElement.FastNavigation;
            DownloadEntities = coreElement.DownloadEntities;
            ManualSync = coreElement.ManualSync;
            CaseType = coreElement.CaseType;
            ElementList = coreElement.ElementList;

            PushMessageTitle = "";
            PushMessageBody = "";
        }

        public MainElement(int id, string label, int displayOrder, string checkListFolderName, int repeated, DateTime startDate, DateTime endDate, string language,
            bool multiApproval, bool fastNavigation, bool downloadEntities, bool manualSync, string caseType, string pushMessageTitle, string pushMessageBody, List<Element> elementList)
        {
            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            CheckListFolderName = checkListFolderName;
            Repeated = repeated;
            SetStartDate(startDate);
            SetEndDate(endDate);
            Language = language;
            MultiApproval = multiApproval;
            FastNavigation = fastNavigation;
            DownloadEntities = downloadEntities;
            ManualSync = manualSync;
            CaseType = caseType;
            PushMessageTitle = pushMessageTitle;
            PushMessageBody = pushMessageBody;
            ElementList = elementList;
        }
        #endregion

        #region var
        #region public string PushMessageTitle { get; set; }
        private string pushMessageTitle;
        public string PushMessageTitle
        {
            get
            {
                if (pushMessageTitle.Length > 255)
                    return pushMessageTitle.Substring(0, 255);
                return pushMessageTitle;
            }
            set
            {
                pushMessageTitle = value;
            }
        }
        #endregion
        public string PushMessageBody { get; set; }
        #endregion

        #region public
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
                throw new Exception("MainElement failed to convert XML", ex);
            }
        }

        public string ClassToXml()
        {
            try
            {
                var serializer = new XmlSerializer(this.GetType());
                string xmlStr;
                using (StringWriter writer = new Utf8StringWriter())
                {
                    serializer.Serialize(writer, this);
                    xmlStr = writer.ToString();
                }
                return xmlStr;
            }
            catch (Exception ex)
            {
                throw new Exception("MainElement failed to convert Class", ex);
            }
        }
        #endregion

        #region private
        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
        #endregion
    }
    #endregion

    #region ReplyElement : CoreElement
    public class ReplyElement : CoreElement
    {
        #region con
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
            SetStartDate(coreElement.GetStartDate());
            SetEndDate(coreElement.GetEndDate());
            Language = coreElement.Language;
            MultiApproval = coreElement.MultiApproval;
            FastNavigation = coreElement.FastNavigation;
            DownloadEntities = coreElement.DownloadEntities;
            ManualSync = coreElement.ManualSync;
            CaseType = coreElement.CaseType;
            ElementList = coreElement.ElementList;
        }
        #endregion

        #region var
        public string Custom { get; set; }
        public DateTime DoneAt { get; set; }
        public int DoneById { get; set; }
        public int UnitId { get; set; }
        #endregion
    }
    #endregion
}
