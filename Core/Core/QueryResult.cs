using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Core
{
    public class QueryResult
    {
        public int Start { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public string Filter { get; set; }
        public IEnumerable<LogResult> Logs { get; set; }
    }
}