namespace CICD.Mappers
{
    public interface IConfiguration
    {
        BO.Configuration DtoToBo(DTO.Configuration configurationDto);

        DTO.Configuration BoToDto(BO.Configuration configurationBo);

        IEnumerable<BO.Configuration> DtosToBos(IEnumerable<DTO.Configuration> configurationDtos);

        IEnumerable<DTO.Configuration> BosToDtos(IEnumerable<BO.Configuration> configurationBos);
    }
}
