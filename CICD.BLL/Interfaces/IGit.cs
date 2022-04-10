using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IGit
    {
        Task Clone(BO.Project project);

        Task Fetch(BO.Project project);

        Task Switch(BO.Branch branch);

        Task Pull(BO.Project project);

        Task<BO.Branch> GetHeadBranch(BO.Project project);
    }
}
