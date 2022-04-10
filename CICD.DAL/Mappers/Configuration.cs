using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public class Configuration : IConfiguration
    {
        public BO.Configuration? DbToBo(Model.Configuration? configurationModel)
        {
            if (configurationModel == null)
                return null;

            var configurationBo = new BO.Configuration
            {
                Id = configurationModel.Id,
                ConfigurationType = (BO.ConfigurationType)configurationModel.ConfigurationTypeId,
                Value = configurationModel.Value,
            };

            return configurationBo;
        }

        public Model.Configuration? BoToDb(BO.Configuration? configurationBo)
        {
            if (configurationBo == null)
                return null;

            var configurationModel = new Model.Configuration
            {
                Id = configurationBo.Id,
                ConfigurationTypeId = (int)configurationBo.ConfigurationType,
                Value = configurationBo.Value,
            };

            return configurationModel;
        }

        public IEnumerable<BO.Configuration> DbsToBos(IEnumerable<Model.Configuration> configurationModels)
        {
            var configurationBos = new List<BO.Configuration>();

            foreach (var configurationModel in configurationModels)
            {
                BO.Configuration? configurationBo = this.DbToBo(configurationModel);

                if (configurationBo != null)
                    configurationBos.Add(configurationBo);
            }

            return configurationBos;
        }

        public IEnumerable<Model.Configuration> BosToDbs(IEnumerable<BO.Configuration> configurationBos)
        {
            var configurationModels = new List<Model.Configuration>();

            foreach (var configurationBo in configurationBos)
            {
                Model.Configuration? configurationModel = this.BoToDb(configurationBo);

                if (configurationModel != null)
                    configurationModels.Add(configurationModel);
            }

            return configurationModels;
        }
    }
}
