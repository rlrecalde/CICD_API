using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IDotnet
    {
        Task<IEnumerable<string>> GetVersions();

        Task Build(BO.Project project);

        Task Test(BO.Project project);
    }
}
