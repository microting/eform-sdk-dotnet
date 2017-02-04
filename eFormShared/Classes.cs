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

    #region simple_site_Dto
    public class Simple_Site_Dto
    {
        #region con
        public Simple_Site_Dto()
        {
        }

        public Simple_Site_Dto(int microtingUid, string name, string userFirstName, string userLastName, int customerNo, int otpCode)
        {
            if (name == null)
                name = "";
            if (userFirstName == null)
                userFirstName = "";
            if (userLastName == null)
                userLastName = "";

            MicrotingUid = microtingUid;
            Name = name;
            FirstName = userFirstName;
            LastName = userLastName;
            CustomerNo = customerNo;
            OtpCode = otpCode;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int MicrotingUid { get; }

        /// <summary>
        ///...
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///...
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string LastName { get; }

        /// <summary>
        ///...
        /// </summary>
        public int CustomerNo { get; }

        /// <summary>
        ///...
        /// </summary>
        public int OtpCode { get; }

        #endregion

        public override string ToString()
        {
            return "Site:" + MicrotingUid + " / SiteName:" + Name + " / FirstName:" + FirstName + " / LastName:" + LastName + " / CustomerNo:" + CustomerNo + " / OtpCode:" + OtpCode + ".";
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

        public Site_Dto(int microtingUid, string name)
        {
            if (name == null)
                name = "";

            MicrotingUid = microtingUid;
            Name = name;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int MicrotingUid { get; }

        /// <summary>
        ///...
        /// </summary>
        public string Name { get; }
       
        #endregion

        public override string ToString()
        {
            return "Site:" + MicrotingUid + " / SiteName:" + Name + ".";
        }
    }
    #endregion

    #region Worker_Dto
    public class Worker_Dto
    {
        #region con
        public Worker_Dto()
        {
        }

        public Worker_Dto(int microtingUid, string first_name, string last_name, string email)
        {
            if (first_name == null)
                first_name = "";

            if (last_name == null)
                last_name = "";

            if (email == null)
                email = "";

            MicrotingUid = microtingUid;
            FirstName = first_name;
            LastName = last_name;
            Email = email;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int MicrotingUid { get; }

        /// <summary>
        ///...
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string LastName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string Email { get; }

        #endregion

        public override string ToString()
        {
            return "Worker:" + MicrotingUid + " / FirstName:" + FirstName + " / LastName:" + LastName + " / Email:" + Email + ".";
        }
    }
    #endregion

    #region Site_Worker_Dto
    public class Site_Worker_Dto
    {
        #region con
        public Site_Worker_Dto()
        {
        }

        public Site_Worker_Dto(int microtingUid, int site_id, int worker_id)
        {

            MicrotingUid = microtingUid;
            SiteId = site_id;
            WorkerId = worker_id;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int MicrotingUid { get; }

        /// <summary>
        ///...
        /// </summary>
        public int SiteId { get; }

        /// <summary>
        ///...
        /// </summary>
        public int WorkerId { get; }

        #endregion

        public override string ToString()
        {
            return "Site:" + MicrotingUid + " / SiteId:" + SiteId + " / WorkerId:" + WorkerId + ".";
        }
    }
    #endregion

    #region Unit_Dto
    public class Unit_Dto
    {
        #region con
        public Unit_Dto()
        {
        }

        public Unit_Dto(int microtingUid, int customer_no, int otp_code, int site_id)
        {

            MicrotingUid = microtingUid;
            CustomerNo = customer_no;
            OtpCode = otp_code;
            OtpCode = site_id;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int MicrotingUid { get; }

        /// <summary>
        ///...
        /// </summary>
        public int CustomerNo { get; }

        /// <summary>
        ///...
        /// </summary>
        public int OtpCode { get; }

        /// <summary>
        ///...
        /// </summary>
        public int SiteId { get; }

        #endregion

        public override string ToString()
        {
            return "Site:" + MicrotingUid + " / CustomerNo:" + CustomerNo + " / OtpCode:" + OtpCode + ".";
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
