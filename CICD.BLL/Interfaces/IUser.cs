using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Interfaces
{
    public interface IUser
    {
        IEnumerable<BO.User> Get();

        void Insert(BO.User user);

        void Update(BO.User user);

        void Delete(BO.User user);
    }
}
