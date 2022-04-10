using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Interfaces
{
    public interface IProjectTransaction
    {
        void Delete(BO.Project project);
    }
}
