using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IGitHub
    {
        IEnumerable<BO.Branch> GetBranches(BO.Project project);

        BO.Commit GetLastCommitByBranch(BO.Branch branch);

        void SendComment(BO.Comment comment);
    }
}
