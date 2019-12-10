using System;

namespace Microting.eForm.Dto
{
    public class SiteNameDto
    {
        #region con
        public SiteNameDto()
        {
        }

        public SiteNameDto(int siteUId, string siteName, DateTime? createdAt, DateTime? updatedAt)
        {
            if (siteName == null)
                siteName = "";

            SiteUId = siteUId;
            SiteName = siteName;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int SiteUId { get; }

        /// <summary>
        ///...
        /// </summary>
        public string SiteName { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; }

        #endregion

        public override string ToString()
        {
            return "SiteUId:" + SiteUId + " / SiteName:" + SiteName + " / CreatedAt:" + CreatedAt + " / UpdatedAt:" + UpdatedAt + ".";
        }
    }
}