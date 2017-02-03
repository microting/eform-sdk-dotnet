using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace eFormShared
{
    #region Case_Dto
    public class Case_Dto
    {
        #region con
        public Case_Dto()
        {
        }

        public Case_Dto(string stat, int siteId, string caseType, string caseUId, string microtingUId, string checkUId, string custom)
        {
            if (caseType == null)
                caseType = "";
            if (caseUId == null)
                caseUId = "";
            if (microtingUId == null)
                microtingUId = "";
            if (checkUId == null)
                checkUId = "";

            Stat = stat;
            SiteId = siteId;
            CaseType = caseType;
            CaseUId = caseUId;
            MicrotingUId = microtingUId;
            CheckUId = checkUId;
            Custom = custom;
        }
        #endregion

        #region var
        /// <summary>
        /// Status of the case
        /// </summary>
        public string Stat { get; }

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
        /// Custom data. Only used in special cases
        /// </summary>
        public string Custom { get; }
        #endregion

        public override string ToString()
        {
            if (CheckUId == null) return "Stat:" + Stat + " / Site:" + SiteId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + ".";
            if (CheckUId == "") return "Stat:" + Stat + " / Site:" + SiteId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + ".";
            return "Stat:" + Stat + " / Site:" + SiteId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + " / CheckId:" + CheckUId + ".";
        }
    }
    #endregion

    #region File_Dto
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
    #endregion

    #region Site_Dto
    public class Site_Dto
    {
        #region con
        public Site_Dto()
        {
        }

        public Site_Dto(int siteId, string siteName, string userFirstName, string userLastName, string resetCode)
        {
            if (siteName == null)
                siteName = "";
            if (userFirstName == null)
                userFirstName = "";
            if (userLastName == null)
                userLastName = "";
            if (resetCode == null)
                resetCode = "";

            SiteId = siteId;
            SiteName = siteName;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            ResetCode = resetCode;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int SiteId { get; }

        /// <summary>
        ///...
        /// </summary>
        public string SiteName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string UserFirstName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string UserLastName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string ResetCode { get; }
        #endregion

        public override string ToString()
        {
            return "Site:" + SiteId + " / SiteName:" + SiteName + " / UserFirstName:" + UserFirstName + " / UserLastName:" + UserLastName + " / ResetCode:" + ResetCode + ".";
        }
    }
    #endregion

    #region Holder
    public class Holder
    {
        public Holder(int index, string field_type)
        {
            Index = index;
            FieldType = field_type;
        }

        public int Index { get; }

        public string FieldType { get; }
    }
    #endregion

    #region EntityItemUpdateInfo
    public class EntityItemUpdateInfo
    {
        public EntityItemUpdateInfo(string entityItemMUid, string status)
        {
            EntityItemMUid = entityItemMUid;
            Status = status;
        }

        public string EntityItemMUid { get; set; }
        public string Status { get; set; }
    }
    #endregion

    #region KeyValuePair
    [Serializable()]
    public class KeyValuePair
    {
        #region con
        internal KeyValuePair()
        {

        }

        public KeyValuePair(string key, string value, bool selected, string displayOrder)
        {
            Key = key;
            Value = value;
            Selected = selected;
            DisplayOrder = displayOrder;
        }
        #endregion

        #region var
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public string DisplayOrder { get; set; }
        #endregion
    }
    #endregion

    #region CDataValue
    public class CDataValue
    {
        [XmlIgnore]
        public string InderValue { get; set; }

        [XmlText]
        public XmlNode[] CDataWrapper
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(InderValue) };
            }
            set
            {
                if (value == null)
                {
                    InderValue = null;
                    return;
                }

                if (value.Length != 1)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Invalid array length {0}", value.Length));
                }

                InderValue = value[0].Value;
            }
        }
    }
    #endregion
}
