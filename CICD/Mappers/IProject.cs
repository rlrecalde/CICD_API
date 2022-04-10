namespace CICD.Mappers
{
    public interface IProject
    {
        BO.Project DtoToBo(DTO.Project projectDto);

        DTO.Project BoToDto(BO.Project projectBo);

        IEnumerable<BO.Project> DbsToBos(IEnumerable<DTO.Project> projectDtos);

        IEnumerable<DTO.Project> BosToDtos(IEnumerable<BO.Project> projectBos);
    }
}
