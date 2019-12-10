namespace Microting.eForm.Dto
{
    public class SiteDto
    {
        #region con
        public SiteDto()
        {
        }

        public SiteDto(int siteId, string siteName, string userFirstName, string userLastName, int? customerNo, int? otpCode, int? unitId, int? workerUid)
        {
            if (siteName == null)
                siteName = "";
            if (userFirstName == null)
                userFirstName = "";
            if (userLastName == null)
                userLastName = "";

            SiteId = siteId;
            SiteName = siteName;
            FirstName = userFirstName;
            LastName = userLastName;
            CustomerNo = customerNo;
            OtpCode = otpCode;
            UnitId = unitId;
            WorkerUid = workerUid;
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
        public string FirstName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string LastName { get; }

        /// <summary>
        ///...
        /// </summary>
        public int? CustomerNo { get; }

        /// <summary>
        ///...
        /// </summary>
        public int? OtpCode { get; }

        /// <summary>
        ///...
        /// </summary>
        public int? UnitId { get; }

        /// <summary>
        ///...
        /// </summary>
        public int? WorkerUid { get; }
        #endregion

        public override string ToString()
        {
            return "SiteId:" + SiteId + " / SiteName:" + SiteName + " / FirstName:" + FirstName + " / LastName:" + LastName + " / CustomerNo:" + CustomerNo + " / OtpCode:" + OtpCode + "UnitId:" + UnitId + "WorkerUid:" + WorkerUid + ".";
        }
    }
}