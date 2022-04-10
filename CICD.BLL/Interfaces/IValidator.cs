using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IValidator<T>
    {
        void Validate(T value);
    }
}
