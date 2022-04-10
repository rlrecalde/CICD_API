namespace CICD.Mappers
{
    public class User : IUser
    {
        public BO.User DtoToBo(DTO.User userDto)
        {
            var userBo = new BO.User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Token = userDto.Token,
                IsDefault = userDto.IsDefault,
            };

            return userBo;
        }

        public DTO.User BoToDto(BO.User userBo)
        {
            var userDto = new DTO.User
            {
                Id = userBo.Id,
                Name = userBo.Name,
                Token = userBo.Token,
                IsDefault = userBo.IsDefault,
            };

            return userDto;
        }

        public IEnumerable<BO.User> DtosToBos(IEnumerable<DTO.User> userDtos)
        {
            var userBos = new List<BO.User>();

            foreach (var userDto in userDtos)
            {
                BO.User userBo = this.DtoToBo(userDto);

                userBos.Add(userBo);
            }

            return userBos;
        }

        public IEnumerable<DTO.User> BosToDtos(IEnumerable<BO.User> userBos)
        {
            var userDtos = new List<DTO.User>();

            foreach (var userBo in userBos)
            {
                DTO.User userDto = this.BoToDto(userBo);

                userDtos.Add(userDto);
            }

            return userDtos;
        }
    }
}
