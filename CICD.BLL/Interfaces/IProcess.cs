using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IProcess
    {
        Task<BO.ProcessResponse> ExecuteAsync(BO.Process process, bool redirectOutput = false);
    }
}
