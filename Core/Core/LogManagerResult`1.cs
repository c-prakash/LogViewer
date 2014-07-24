using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Core
{
    public class LogManagerResult<T> : LogManagerResult
    {
       public LogManagerResult(T result)
        {
            Result = result;
        }

       public LogManagerResult(params string[] errors)
            : base(errors)
        {
        }

        public T Result { get; private set; }
    }
}