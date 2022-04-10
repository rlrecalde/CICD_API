using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public interface IBranch
    {
        BO.Branch? DbToBo(Model.Branch? branchModel);

        Model.Branch? BoToDb(BO.Branch? branchBo);

        IEnumerable<BO.Branch> DbsToBos(IEnumerable<Model.Branch> branchModels);

        IEnumerable<Model.Branch> BosToDbs(IEnumerable<BO.Branch> branchBos);
    }
}
