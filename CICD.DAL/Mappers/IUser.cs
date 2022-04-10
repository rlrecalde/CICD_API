using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public interface IUser
    {
        BO.User? DbToBo(Model.User? userModel);

        Model.User? BoToDb(BO.User? userBo);

        IEnumerable<BO.User> DbsToBos(IEnumerable<Model.User> userModels);

        IEnumerable<Model.User> BosToDbs(IEnumerable<BO.User> userBos);
    }
}
