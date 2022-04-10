namespace CICD.Mappers
{
    public interface IUser
    {
        BO.User DtoToBo(DTO.User userDto);

        DTO.User BoToDto(BO.User userBo);

        IEnumerable<BO.User> DtosToBos(IEnumerable<DTO.User> userDtos);

        IEnumerable<DTO.User> BosToDtos(IEnumerable<BO.User> userBos);
    }
}
