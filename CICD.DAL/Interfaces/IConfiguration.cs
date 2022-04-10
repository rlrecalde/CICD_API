using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Interfaces
{
    public interface IConfiguration
    {
        IEnumerable<BO.Configuration> Get();

        BO.Configuration GetByType(BO.ConfigurationType configurationType);

        void Insert(BO.Configuration configuration);

        void Update(BO.Configuration configuration);
    }
}
