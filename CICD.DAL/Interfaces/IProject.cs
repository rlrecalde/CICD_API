using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Interfaces
{
    public interface IProject
    {
        IEnumerable<BO.Project> GetByUserId(int userId);

        BO.Project GetById(int projectId);

        BO.Project GetByName(string name);

        void Insert(BO.Project project);

        void Update(BO.Project project);

        void Delete(BO.Project project);
    }
}
