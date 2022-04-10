using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IBranch
    {
        IEnumerable<BO.Branch> GetByProjectId(int projectId);

        void Insert(BO.Branch branch);

        void InsertMany(IEnumerable<BO.Branch> branches);

        void MarkAsDeleted(BO.Branch branch);

        void MarkManyAsDeleted(IEnumerable<BO.Branch> branches);
    }
}
