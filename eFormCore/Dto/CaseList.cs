using System.Collections.Generic;

namespace Microting.eForm.Dto
{
    public class CaseList
    {
        public CaseList()
        {
            
        }

        public CaseList(int numOfElements, int pageNum, List<Case> caseList)
        {
            this.NumOfElements = numOfElements;
            this.PageNum = pageNum;
            this.Cases = caseList;
        }

        public int NumOfElements { get; }
        public int PageNum { get; }
        public List<Case> Cases { get; }
    }
}