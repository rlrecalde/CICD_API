namespace CICD.Mappers
{
    public class Commit : ICommit
    {
        public BO.Commit DtoToBo(DTO.Commit commitDto)
        {
            var commitBo = new BO.Commit
            {
                Id = commitDto.Id,
                BranchId = commitDto.BranchId,
                CommitterLogin = commitDto.CommitterLogin ?? string.Empty,
                CommitterName = commitDto.CommitterName ?? string.Empty,
                Date = commitDto.Date,
                Message = commitDto.Message ?? string.Empty,
            };

            return commitBo;
        }

        public DTO.Commit BoToDto(BO.Commit commitBo)
        {
            var commitDto = new DTO.Commit
            {
                Id = commitBo.Id,
                BranchId = commitBo.BranchId,
                CommitterLogin = commitBo.CommitterLogin,
                CommitterName = commitBo.CommitterName,
                Date = commitBo.Date,
                Message = commitBo.Message,
            };

            return commitDto;
        }

        public IEnumerable<BO.Commit> DtosToBos(IEnumerable<DTO.Commit> commitDtos)
        {
            var commitBos = new List<BO.Commit>();

            foreach (var commitDto in commitDtos)
            {
                BO.Commit commitBo = this.DtoToBo(commitDto);

                commitBos.Add(commitBo);
            }

            return commitBos;
        }

        public IEnumerable<DTO.Commit> BosToDtos(IEnumerable<BO.Commit> commitBos)
        {
            var commitDtos = new List<DTO.Commit>();

            foreach (var commitBo in commitBos)
            {
                DTO.Commit commitDto = this.BoToDto(commitBo);

                commitDtos.Add(commitDto);
            }

            return commitDtos;
        }
    }
}
