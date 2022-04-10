using System;
using System.Collections.Generic;
using System.Text;

namespace CICD.BO.CustomExceptions
{
    public class NotFoundException : CustomExceptionBase
    {
        public NotFoundException()
        {

        }

        public NotFoundException(ErrorData errorData)
        {
            base.ErrorData = errorData;
        }
    }
}
