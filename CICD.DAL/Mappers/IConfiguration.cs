using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public interface IConfiguration
    {
        BO.Configuration? DbToBo(Model.Configuration? configurationModel);

        Model.Configuration? BoToDb(BO.Configuration? configurationBo);

        IEnumerable<BO.Configuration> DbsToBos(IEnumerable<Model.Configuration> configurationModels);

        IEnumerable<Model.Configuration> BosToDbs(IEnumerable<BO.Configuration> configurationBos);
    }
}
