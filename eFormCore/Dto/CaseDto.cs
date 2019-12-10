namespace Microting.eForm.Dto
{
    public class CaseDto
    {
        #region con
        public CaseDto()
        {
        }

        #endregion

        #region var
        /// <summary>
        /// Local case identifier
        /// </summary>
        public int? CaseId { get; set; }

        /// <summary>
        /// Status of the case
        /// </summary>
        public string Stat { get; set; }

        /// <summary>
        /// Unique identifier of device
        /// </summary>
        public int SiteUId { get; set; }

        /// <summary>
        /// Identifier of a collection of cases in your system
        /// </summary>
        public string CaseType { get; set; }

        /// <summary>
        /// Unique identifier of a group of case(s) in your system
        /// </summary>
        public string CaseUId { get; set; }

        /// <summary>
        ///Unique identifier of that specific eForm in Microting system
        /// </summary>
        public int? MicrotingUId { get; set; }

        /// <summary>
        /// Unique identifier of that check of the eForm. Only used if repeat
        /// </summary>
        public int? CheckUId { get; set; }

        /// <summary>
        /// Custom data. Only used in special cases
        /// </summary>
        public string Custom { get; set; }

        /// <summary>
        /// Unique identifier of device
        /// </summary>
        public int CheckListId { get; set; }

        /// <summary>
        /// WorkflowState
        /// </summary>
        public string WorkflowState { get; set; }

        #endregion

        public override string ToString()
        {
            string caseIdStr;

            if (CaseId == null)
                caseIdStr = "";
            else
                caseIdStr = CaseId.ToString();

            if (CheckUId == null) return "CaseId:" + caseIdStr + " / Stat:" + Stat + " / SiteUId:" + SiteUId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + ".";
//            if (CheckUId == "") return "CaseId:" + caseIdStr + " / Stat:" + Stat + " / SiteUId:" + SiteUId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + ".";
            return "CaseId:" + caseIdStr + " / Stat:" + Stat + " / SiteUId:" + SiteUId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + " / CheckId:" + CheckUId + ".";
        }
    }
}