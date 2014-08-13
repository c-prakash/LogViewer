using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Core.Repository;
using LinqKit;

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

        public Task<LogManagerResult<QueryResult>> QueryLogsAsync(QueryCriteria queryCriteria, int start, int count)
        {
            var expression = this.CreatePredicate<Log>(queryCriteria);
            var logs = query.AsExpandable().Where(expression);

            var result = new QueryResult();
            result.Start = start;
            result.Count = count;
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

        private Expression<Func<T, bool>> CreatePredicate<T>(QueryCriteria queryCriteria) where T : Log
        {
            var inner = PredicateBuilder.True<T>();

            if (!string.IsNullOrWhiteSpace(queryCriteria.MachineName))
                inner = inner.And(x => x.Message.Contains(queryCriteria.Message));

            if (queryCriteria.StartDate != null && queryCriteria.StartDate > DateTime.MinValue)
                inner = inner.And(x => x.Timestamp >= queryCriteria.StartDate);

            if (queryCriteria.EndDate != null && queryCriteria.EndDate > DateTime.MinValue)
                inner = inner.And(x => x.Timestamp == queryCriteria.EndDate);

            if (queryCriteria.EventId > -1)
                inner = inner.And(x => x.EventID == queryCriteria.EventId);

            if (queryCriteria.Priority > -1)
                inner = inner.And(x => x.Priority == queryCriteria.Priority);

            if (!string.IsNullOrWhiteSpace(queryCriteria.Severity))
                inner = inner.And(x => x.Severity == queryCriteria.Severity);

            if (!string.IsNullOrWhiteSpace(queryCriteria.MachineName))
                inner = inner.And(x => x.MachineName == queryCriteria.MachineName);

            if (!string.IsNullOrWhiteSpace(queryCriteria.FormattedMessage))
                inner = inner.And(x => x.FormattedMessage.Contains(queryCriteria.FormattedMessage));

            return inner;
        }
    }
}