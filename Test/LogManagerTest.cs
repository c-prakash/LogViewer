using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Repository;
using Core.Core;

namespace Test
{
    [TestClass]
    public class LogManagerTest
    {
        [TestMethod]
        public void QueryLog()
        {
            var logManager = new LogManager(new LoggingEntities().Logs, new LoggingEntities());

            var criteria = new QueryCriteria() { Message = "test", StartDate = new DateTime(2014, 07, 02), EventId = 100, Priority = 0 };
            var result = logManager.QueryLogsAsync(criteria, 0, 100);

            Assert.IsTrue(result.Result.Result.Total > 0, "Success");
        }
    }
}
