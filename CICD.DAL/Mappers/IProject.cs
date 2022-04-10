using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public interface IProject
    {
        BO.Project? DbToBo(Model.Project? projectModel);

        Model.Project? BoToDb(BO.Project? projectBo);

        IEnumerable<BO.Project> DbsToBos(IEnumerable<Model.Project> projectModels);

        IEnumerable<Model.Project> BosToDbs(IEnumerable<BO.Project> projectBos);
    }
}
