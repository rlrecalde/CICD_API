using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public class User : IUser
    {
        public BO.User? DbToBo(Model.User? userModel)
        {
            if (userModel == null)
                return null;

            var userBo = new BO.User
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Token = userModel.Token,
            };

            return userBo;
        }

        public Model.User? BoToDb(BO.User? userBo)
        {
            if (userBo == null)
                return null;

            var userModel = new Model.User
            {
                Id = userBo.Id,
                Name= userBo.Name,
                Token= userBo.Token,
            };

            return userModel;
        }

        public IEnumerable<BO.User> DbsToBos(IEnumerable<Model.User> userModels)
        {
            var userBos = new List<BO.User>();

            foreach (var userModel in userModels)
            {
                BO.User? userBo = this.DbToBo(userModel);

                if (userBo != null)
                    userBos.Add(userBo);
            }

            return userBos;
        }

        public IEnumerable<Model.User> BosToDbs(IEnumerable<BO.User> userBos)
        {
            var userModels = new List<Model.User>();

            foreach (var userBo in userBos)
            {
                Model.User? userModel = this.BoToDb(userBo);

                if (userModel != null)
                    userModels.Add(userModel);
            }

            return userModels;
        }
    }
}
