using System;
using System.Collections.Generic;
using System.Text;

namespace CICD.BO.CustomExceptions
{
    public class UnexpectedException : CustomExceptionBase
    {
        public UnexpectedException()
        {

        }

        public UnexpectedException(ErrorData errorData)
        {
            base.ErrorData = errorData;
        }
    }
}
