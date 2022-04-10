namespace CICD.Mappers
{
    public class Configuration : IConfiguration
    {
        public BO.Configuration DtoToBo(DTO.Configuration configurationDto)
        {
            var configurationBo = new BO.Configuration
            {
                Id = configurationDto.Id,
                ConfigurationType = (BO.ConfigurationType)configurationDto.ConfigurationTypeId,
                Value = configurationDto.Value,
            };

            return configurationBo;
        }

        public DTO.Configuration BoToDto(BO.Configuration configurationBo)
        {
            var configurationDto = new DTO.Configuration
            {
                Id = configurationBo.Id,
                ConfigurationTypeId = (int)configurationBo.ConfigurationType,
                Value = configurationBo.Value,
            };

            return configurationDto;
        }

        public IEnumerable<BO.Configuration> DtosToBos(IEnumerable<DTO.Configuration> configurationDtos)
        {
            var configurationBos = new List<BO.Configuration>();

            foreach (var configurationDto in configurationDtos)
            {
                BO.Configuration configurationBo = this.DtoToBo(configurationDto);

                configurationBos.Add(configurationBo);
            }

            return configurationBos;
        }

        public IEnumerable<DTO.Configuration> BosToDtos(IEnumerable<BO.Configuration> configurationBos)
        {
            var configurationDtos = new List<DTO.Configuration>();

            foreach (var configurationBo in configurationBos)
            {
                DTO.Configuration configurationDto = this.BoToDto(configurationBo);

                configurationDtos.Add(configurationDto);
            }

            return configurationDtos;
        }
    }
}
