using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface ICommit
    {
        BO.Commit GetLastCommitByBranchId(int branchId);

        void Insert(BO.Commit commit);
    }
}
