using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public class Commit : ICommit
    {
        public BO.Commit? DbToBo(Model.Commit? commitModel)
        {
            if (commitModel == null)
                return null;

            var commitBo = new BO.Commit
            {
                Id = commitModel.Sha,
                BranchId = commitModel.BranchId,
                CommitterLogin = commitModel.CommitterLogin,
                CommitterName = commitModel.CommitterName,
                Date = commitModel.Date,
                Message = commitModel.Message,
            };

            return commitBo;
        }

        public Model.Commit? BoToDb(BO.Commit? commitBo)
        {
            if (commitBo == null)
                return null;

            var commitModel = new Model.Commit
            {
                Sha = commitBo.Id,
                BranchId = commitBo.BranchId,
                CommitterLogin = commitBo.CommitterLogin,
                CommitterName = commitBo.CommitterName,
                Date = commitBo.Date,
                Message = commitBo.Message,
            };

            return commitModel;
        }

        public IEnumerable<BO.Commit> DbsToBos(IEnumerable<Model.Commit> commitModels)
        {
            var commitBos = new List<BO.Commit>();

            foreach (var commitModel in commitModels)
            {
                BO.Commit? commitBo = this.DbToBo(commitModel);

                if (commitBo != null)
                    commitBos.Add(commitBo);
            }

            return commitBos;
        }

        public IEnumerable<Model.Commit> BosToDbs(IEnumerable<BO.Commit> commitBos)
        {
            var commitModels = new List<Model.Commit>();

            foreach (var commitBo in commitBos)
            {
                Model.Commit? commitModel = this.BoToDb(commitBo);

                if (commitModel != null)
                    commitModels.Add(commitModel);
            }

            return commitModels;
        }
    }
}
