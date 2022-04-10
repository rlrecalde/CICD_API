using System;
using System.Collections.Generic;
using System.Text;

namespace CICD.BO.CustomExceptions
{
    public class ConflictException : CustomExceptionBase
    {
        public ConflictException()
        {

        }

        public ConflictException(ErrorData errorData)
        {
            base.ErrorData = errorData;
        }
    }
}
