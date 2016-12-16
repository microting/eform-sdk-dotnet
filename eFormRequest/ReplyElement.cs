using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFormRequest
{
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
        public DateTime DateOfDoing { get; set; }
        public int DoneById { get; set; }
        public int UnitId { get; set; }
        #endregion
    }
}