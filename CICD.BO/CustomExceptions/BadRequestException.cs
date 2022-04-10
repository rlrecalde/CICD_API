using System;
using System.Collections.Generic;
using System.Text;

namespace CICD.BO.CustomExceptions
{
    public class BadRequestException : CustomExceptionBase
    {
        public BadRequestException()
        {

        }

        public BadRequestException(ErrorData errorData)
        {
            base.ErrorData = errorData;
        }
    }
}
