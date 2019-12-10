using System;

namespace Microting.eForm.Dto
{
    public class UnitDto
    {
        #region var
        /// <summary>
        ///...
        /// </summary>
        public int UnitUId { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public int CustomerNo { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public int OtpCode { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public int SiteUId { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        public string WorkflowState { get; set; }

        #endregion
    }
}