using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core
{
    public class QueryCriteria
    {
        public QueryCriteria()
        {
            this.StartDate = null;
            this.EndDate = null;
            this.EventId = -1;
            this.Priority = -1;
        }

        public int EventId { get; set; }

        public int Priority { get; set; }

        public string Severity { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MachineName { get; set; }

        public string Message { get; set; }

        public string FormattedMessage { get; set; }

    }
}
