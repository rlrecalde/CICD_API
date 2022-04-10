using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public interface ICommit
    {
        BO.Commit? DbToBo(Model.Commit? commitModel);

        Model.Commit? BoToDb(BO.Commit? commitBo);

        IEnumerable<BO.Commit> DbsToBos(IEnumerable<Model.Commit> commitModels);

        IEnumerable<Model.Commit> BosToDbs(IEnumerable<BO.Commit> commitBos);
    }
}
