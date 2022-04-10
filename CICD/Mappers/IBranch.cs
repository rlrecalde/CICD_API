namespace CICD.Mappers
{
    public interface IBranch
    {
        BO.Branch DtoToBo(DTO.Branch branchDto);

        DTO.Branch BoToDto(BO.Branch branchBo);

        IEnumerable<BO.Branch> DtosToBos(IEnumerable<DTO.Branch> branchDtos);

        IEnumerable<DTO.Branch> BosToDtos(IEnumerable<BO.Branch> branchBos);
    }
}
