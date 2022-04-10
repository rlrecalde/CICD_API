namespace CICD.Mappers
{
    public interface ICommit
    {
        BO.Commit DtoToBo(DTO.Commit commitDto);

        DTO.Commit BoToDto(BO.Commit commitBo);

        IEnumerable<BO.Commit> DtosToBos(IEnumerable<DTO.Commit> commitDtos);

        IEnumerable<DTO.Commit> BosToDtos(IEnumerable<BO.Commit> commitBos);
    }
}
