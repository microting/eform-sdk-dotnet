/*
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

namespace Microting.eForm.Dto
{
    public class CaseDto
    {
        #region con

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

            if (CheckUId == null)
                return "CaseId:" + caseIdStr + " / Stat:" + Stat + " / SiteUId:" + SiteUId + " / CaseType:" + CaseType +
                       " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + ".";
//            if (CheckUId == "") return "CaseId:" + caseIdStr + " / Stat:" + Stat + " / SiteUId:" + SiteUId + " / CaseType:" + CaseType + " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + ".";
            return "CaseId:" + caseIdStr + " / Stat:" + Stat + " / SiteUId:" + SiteUId + " / CaseType:" + CaseType +
                   " / CaseUId:" + CaseUId + " / MicrotingUId:" + MicrotingUId + " / CheckId:" + CheckUId + ".";
        }
    }
}