using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Interfaces
{
    public interface IGitHub
    {
        bool UserAuthentication(BO.User user);

        bool FileExists(BO.Project project);

        IEnumerable<BO.Project> GetProjects(BO.User user);

        IEnumerable<BO.Branch> GetBranches(BO.Project project);

        BO.Commit GetLastCommitByBranch(BO.Branch branch);

        void SendComment(BO.Comment comment);
    }
}
