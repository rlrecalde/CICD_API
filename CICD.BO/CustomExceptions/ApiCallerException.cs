using System;
using System.Collections.Generic;
using System.Text;

namespace CICD.BO.CustomExceptions
{
    public class ApiCallerException : CustomExceptionBase
    {
        public ApiCallerException()
        {

        }

        public ApiCallerException(ErrorData errorData)
        {
            base.ErrorData = errorData;
        }
    }
}
