namespace CICD.Mappers
{
    public class Comment : IComment
    {
        private readonly IBranch _branchMapper;

        public Comment(IBranch branchMapper)
        {
            this._branchMapper = branchMapper;
        }

        public BO.Comment DtoToBo(DTO.Comment commentDto)
        {
            var commentBo = new BO.Comment
            {
                Branch = this._branchMapper.DtoToBo(commentDto.Branch),
                Text = commentDto.Text,
            };

            return commentBo;
        }

        public DTO.Comment BoToDto(BO.Comment commentBo)
        {
            var commentDto = new DTO.Comment
            {
                Branch = this._branchMapper.BoToDto(commentBo.Branch),
                Text = commentBo.Text,
            };

            return commentDto;
        }

        public IEnumerable<BO.Comment> DtosToBos(IEnumerable<DTO.Comment> commentDtos)
        {
            var commentBos = new List<BO.Comment>();

            foreach (var commentDto in commentDtos)
            {
                BO.Comment commentBo = this.DtoToBo(commentDto);

                commentBos.Add(commentBo);
            }

            return commentBos;
        }

        public IEnumerable<DTO.Comment> BosToDtos(IEnumerable<BO.Comment> commentBos)
        {
            var commentDtos = new List<DTO.Comment>();

            foreach (var commentBo in commentBos)
            {
                DTO.Comment commentDto = this.BoToDto(commentBo);

                commentDtos.Add(commentDto);
            }

            return commentDtos;
        }
    }
}
