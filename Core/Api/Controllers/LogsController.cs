using Core.Core;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Core.Api.Controllers
{
    [RoutePrefix("api")]
    public class LogsController : ApiController
    {
        private ILogManager logManager = null;
        public LogsController()
            : this(new LogManager(new LoggingEntities().Logs, new LoggingEntities()))
        {
        }

        public LogsController(ILogManager logManager)
        {
            this.logManager = logManager;
        }

        [Route("logs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLogsAsync(string filter = "", int start = 0, int count = 100)
        {
            var result = await logManager.QueryLogsAsync(filter, start, count);
            if (result.IsSuccess)
            {
                return Ok(result.Result);
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, result.Errors));
        }

        [Route("logs/fromdate/{fromDate:datetime}/todate/{toDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLogsAsync(QueryCriteria queryCriteria, int start = 0, int count = 100)
        {
            var result = await logManager.QueryLogsAsync(queryCriteria, start, count);
            if (result.IsSuccess)
            {
                return Ok(result.Result);
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, result.Errors));
        }
    }
}
