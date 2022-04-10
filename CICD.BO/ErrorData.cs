using System;
using System.Collections.Generic;
using System.Text;

namespace CICD.BO
{
    public class ErrorData
    {
        public string Message { get; set; }

        public string? StackTrace { get; set; }

        public object? Data { get; set; }

        public ErrorData()
        {

        }

        public ErrorData(object? data)
        {
            this.Data = data;
        }

        public ErrorData(object? data, Exception ex) : this(data)
        {
            this.Message = ex.Message;
            this.StackTrace = ex.StackTrace;

            if (ex.InnerException != null)
            {
                this.Message += "\r\n" + ex.InnerException.Message;
                this.StackTrace += "\r\n" + "\r\n" + ex.InnerException.StackTrace;
            }
        }
    }
}
