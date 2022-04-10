namespace CICD.Mappers
{
    public class Branch : IBranch
    {
        private readonly ICommit _commitMapper;
        private readonly IProject _projectMapper;

        public Branch(ICommit commitMapper,
                      IProject projectMapper)
        {
            this._commitMapper = commitMapper;
            this._projectMapper = projectMapper;
        }

        public BO.Branch DtoToBo(DTO.Branch branchDto)
        {
            var branchBo = new BO.Branch
            {
                Id = branchDto.Id,
                Name = branchDto.Name,
                LastCommit = this._commitMapper.DtoToBo(branchDto.LastCommit),
                Project = this._projectMapper.DtoToBo(branchDto.Project),
            };

            return branchBo;
        }

        public DTO.Branch BoToDto(BO.Branch branchBo)
        {
            var branchDto = new DTO.Branch
            {
                Id = branchBo.Id,
                Name = branchBo.Name,
                LastCommit = this._commitMapper.BoToDto(branchBo.LastCommit),
                Project = this._projectMapper.BoToDto(branchBo.Project),
            };

            return branchDto;
        }

        public IEnumerable<BO.Branch> DtosToBos(IEnumerable<DTO.Branch> branchDtos)
        {
            var branchBos = new List<BO.Branch>();

            foreach (var branchDto in branchDtos)
            {
                BO.Branch branchBo = this.DtoToBo(branchDto);

                branchBos.Add(branchBo);
            }

            return branchBos;
        }

        public IEnumerable<DTO.Branch> BosToDtos(IEnumerable<BO.Branch> branchBos)
        {
            var branchDtos = new List<DTO.Branch>();

            foreach (var branchBo in branchBos)
            {
                DTO.Branch branchDto = this.BoToDto(branchBo);

                branchDtos.Add(branchDto);
            }

            return branchDtos;
        }
    }
}
