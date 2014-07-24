using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Core
{
    public class LogManagerResult
    {
        public static readonly LogManagerResult Success = new LogManagerResult();

        public LogManagerResult(params string[] errors)
        {
            this.Errors = errors;
        }

        public IEnumerable<string> Errors { get; private set; }

        public bool IsSuccess
        {
            get { return Errors == null || !Errors.Any(); }
        }

        public bool IsError
        {
            get { return !IsSuccess; }
        }
    }
}