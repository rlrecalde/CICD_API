using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Interfaces
{
    public interface IUser
    {
        IEnumerable<BO.User> Get();

        BO.User GetByName(string name);

        void Insert(BO.User user);

        void Update(BO.User user);

        void Delete(BO.User user);
    }
}
