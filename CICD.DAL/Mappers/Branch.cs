using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public class Branch : IBranch
    {
        public BO.Branch? DbToBo(Model.Branch? branchModel)
        {
            if (branchModel == null)
                return null;

            var branchBo = new BO.Branch
            {
                Id = branchModel.Id,
                Name = branchModel.Name,
                Project = new BO.Project { Id = branchModel.ProjectId, User = new BO.User() },
                LastCommit = new BO.Commit(),
            };

            return branchBo;
        }

        public Model.Branch? BoToDb(BO.Branch? branchBo)
        {
            if (branchBo == null)
                return null;

            var branchModel = new Model.Branch
            {
                Id = branchBo.Id,
                ProjectId = branchBo.Project.Id,
                Name = branchBo.Name,
                Deleted = false,
            };

            return branchModel;
        }

        public IEnumerable<BO.Branch> DbsToBos(IEnumerable<Model.Branch> branchModels)
        {
            var branchBos = new List<BO.Branch>();

            foreach (var branchModel in branchModels)
            {
                BO.Branch? branchBo = this.DbToBo(branchModel);

                if (branchBo != null)
                    branchBos.Add(branchBo);
            }

            return branchBos;
        }

        public IEnumerable<Model.Branch> BosToDbs(IEnumerable<BO.Branch> branchBos)
        {
            var branchModels = new List<Model.Branch>();

            foreach (var branchBo in branchBos)
            {
                Model.Branch? branchModel = this.BoToDb(branchBo);

                if (branchModel != null)
                    branchModels.Add(branchModel);
            }

            return branchModels;
        }
    }
}
