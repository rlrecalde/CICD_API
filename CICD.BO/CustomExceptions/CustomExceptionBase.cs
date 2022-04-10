using System;
using System.Collections.Generic;
using System.Text;

namespace CICD.BO.CustomExceptions
{
    public class CustomExceptionBase : Exception
    {
        public CustomExceptionBase()
        {
            this.ErrorData = new ErrorData();
        }

        public CustomExceptionBase(ErrorData errorData)
        {
            this.ErrorData = errorData;
        }

        public ErrorData? ErrorData { get; set; }
    }
}
