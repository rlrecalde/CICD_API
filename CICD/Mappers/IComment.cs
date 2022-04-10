namespace CICD.Mappers
{
    public interface IComment
    {
        BO.Comment DtoToBo(DTO.Comment commentDto);

        DTO.Comment BoToDto(BO.Comment commentBo);

        IEnumerable<BO.Comment> DtosToBos(IEnumerable<DTO.Comment> commentDtos);

        IEnumerable<DTO.Comment> BosToDtos(IEnumerable<BO.Comment> commentBos);
    }
}
