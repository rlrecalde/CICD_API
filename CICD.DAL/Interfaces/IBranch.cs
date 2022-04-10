using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Interfaces
{
    public interface IBranch
    {
        IEnumerable<BO.Branch> GetByProjectId(int projectId);

        void Insert(BO.Branch branch);

        void MarkAsDeleted(BO.Branch branch);

        void Delete(BO.Branch branch);
    }
}
