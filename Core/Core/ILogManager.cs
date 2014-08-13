using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core
{
    public interface ILogManager
    {
        Task<LogManagerResult<QueryResult>> QueryLogsAsync(string filter = null, int start = 0, int count = 100);

        Task<LogManagerResult<QueryResult>> QueryLogsAsync(QueryCriteria queryCriteria, int start = 0, int count = 100);
    }
}
