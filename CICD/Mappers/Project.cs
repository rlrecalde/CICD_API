namespace CICD.Mappers
{
    public class Project : IProject
    {
        private readonly IUser _userMapper;

        public Project(IUser userMapper)
        {
            this._userMapper = userMapper;
        }

        public BO.Project DtoToBo(DTO.Project projectDto)
        {
            var projectBo = new BO.Project
            {
                Id = projectDto.Id,
                User = this._userMapper.DtoToBo(projectDto.User) ?? new BO.User(),
                Name = projectDto.Name,
                RelativePath = projectDto.RelativePath,
                DotnetVersion = projectDto.DotnetVersion,
                Test = projectDto.Test,
                Deploy = projectDto.Deploy,
                DeployPort = projectDto.DeployPort,
            };

            return projectBo;
        }

        public DTO.Project BoToDto(BO.Project projectBo)
        {
            var projectDto = new DTO.Project
            {
                Id = projectBo.Id,
                User = this._userMapper.BoToDto(projectBo.User),
                Name = projectBo.Name,
                RelativePath = projectBo.RelativePath,
                DotnetVersion = projectBo.DotnetVersion,
                Test = projectBo.Test,
                Deploy = projectBo.Deploy,
                DeployPort= projectBo.DeployPort,
            };

            return projectDto;
        }

        public IEnumerable<BO.Project> DbsToBos(IEnumerable<DTO.Project> projectDtos)
        {
            var projectBos = new List<BO.Project>();

            foreach (var projectDto in projectDtos)
            {
                BO.Project projectBo = this.DtoToBo(projectDto);

                projectBos.Add(projectBo);
            }

            return projectBos;
        }

        public IEnumerable<DTO.Project> BosToDtos(IEnumerable<BO.Project> projectBos)
        {
            var projectDtos = new List<DTO.Project>();

            foreach (var projectBo in projectBos)
            {
                DTO.Project projectDto = this.BoToDto(projectBo);

                projectDtos.Add(projectDto);
            }

            return projectDtos;
        }
    }
}
