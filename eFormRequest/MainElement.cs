using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace eFormRequest
{
    [XmlRoot(ElementName = "Main")]
    [Serializable()]
    public class MainElement : ReplyElement
    {
        #region con
        public MainElement()
        {
            ElementList = new List<Element>();
        }

        public MainElement(int id, string label, int displayOrder, string checkListFolderName, int repeated, DateTime startDate, DateTime endDate, string language,
            bool multiApproval, bool fastNavigation, bool downloadEntities, bool manualSync, string caseType, string pushMessageTitle, string pushMessageBody, List<Element> elementList)
        {
            ElementList = new List<Element>();

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
}
