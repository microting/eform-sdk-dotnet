using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormShared
{
    public class Case
    {
        public int Id { get; set; }

        public string WorkflowState { get; set; }

        public int? Version { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DoneAt { get; set; }

        public int? SiteId { get; set; }

        public string SiteName { get; set; }

        public int? UnitId { get; set; }

        public string WorkerName { get; set; }

        public int? TemplatId { get; set; }

        public string CaseType { get; set; }

        public string MicrotingUId { get; set; }

        public string CheckUIid { get; set; }

        public string CaseUId { get; set; }

        public string Custom { get; set; }
    }
}
