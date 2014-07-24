using Core.Repository;
using Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Core.Core
{
    public class LogManager : ILogManager, IDisposable
    {
        IDisposable cleanup;
        IQueryable<Log> query;

        public LogManager(IQueryable<Log> query, IDisposable cleanup)
        {
            this.cleanup = cleanup;
            this.query = query;
        }

        public void Dispose()
        {
            if (this.cleanup != null)
            {
                cleanup.Dispose();
                cleanup = null;
            }
        }

        public Task<LogManagerResult<QueryResult>> QueryLogsAsync(string filter, int start, int count)
        {
            var logs = query.Where(c => c.Message.Contains(filter)).OrderByDescending(o=> o.Timestamp);
            var result = new QueryResult();
            result.Start = start;
            result.Count = count;
            result.Filter = filter;
            result.Logs = logs.Skip(start).Take(count).Select(x =>
                                                            new LogResult
                                                            {
                                                                AppDomainName = x.AppDomainName,
                                                                EventID = x.EventID,
                                                                FormattedMessage = x.FormattedMessage,
                                                                LogID = x.LogID,
                                                                MachineName = x.MachineName,
                                                                Message = x.Message,
                                                                Priority = x.Priority,
                                                                ProcessID = x.ProcessID,
                                                                ProcessName = x.ProcessName,
                                                                Severity = x.Severity,
                                                                ThreadName = x.ThreadName,
                                                                Timestamp = x.Timestamp,
                                                                Title = x.Title,
                                                                Win32ThreadId = x.Win32ThreadId
                                                            });


            result.Total = logs.Count();
            return Task.FromResult(new LogManagerResult<QueryResult>(result));
        }

        private LogResult[] GetLogs()
        {
            return new LogResult[]
            { 
                new LogResult() 
                              {
                                AppDomainName = "A",
                                EventID =1,
                                FormattedMessage = "FormattedMessage",
                                LogID = 1,
                                MachineName ="",
                                Message = "AMessage",
                                Priority = 1,
                                ProcessID = "",
                                ProcessName = "ProcessName",
                                Severity = "Severity",
                                ThreadName = "ThreadName",
                                Timestamp = DateTime.Now,
                                Title ="Title",
                                Win32ThreadId = "Win32ThreadId"
                              }, 
                new LogResult()
                              {
                                AppDomainName = "A",
                                EventID =1,
                                FormattedMessage = "FormattedMessage",
                                LogID = 1,
                                MachineName ="",
                                Message = "SomeMessage",
                                Priority = 1,
                                ProcessID = "",
                                ProcessName = "ProcessName",
                                Severity = "Severity",
                                ThreadName = "ThreadName",
                                Timestamp = DateTime.Now,
                                Title ="Title",
                                Win32ThreadId = "Win32ThreadId"
                              }
            };
        }
    }
}