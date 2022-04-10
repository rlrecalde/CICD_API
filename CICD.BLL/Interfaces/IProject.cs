using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IProject
    {
        IEnumerable<BO.Project> GetByUserId(int userId);

        void Insert(BO.Project project);

        void Update(BO.Project project);

        Task Delete(BO.Project project);
    }
}
