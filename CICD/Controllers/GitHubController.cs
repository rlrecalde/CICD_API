using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly BLL.Interfaces.IGitHub _gitHubBusiness;
        private readonly Mappers.IProject _projectMapper;
        private readonly Mappers.IBranch _branchMapper;
        private readonly Mappers.ICommit _commitMapper;
        private readonly Mappers.IComment _commentMapper;

        public GitHubController(BLL.Interfaces.IGitHub gitHubBusiness,
                                Mappers.IProject projectMapper,
                                Mappers.IBranch branchMapper,
                                Mappers.ICommit commitMapper,
                                Mappers.IComment commentMapper)
        {
            this._gitHubBusiness = gitHubBusiness;
            this._projectMapper = projectMapper;
            this._branchMapper = branchMapper;
            this._commitMapper = commitMapper;
            this._commentMapper = commentMapper;
        }

        [HttpPost("branches/")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DTO.Branch>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult GetBranches([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            IEnumerable<BO.Branch> branchBos = this._gitHubBusiness.GetBranches(projectBo);

            IEnumerable<DTO.Branch> branchDtos = this._branchMapper.BosToDtos(branchBos);
            return base.Ok(branchDtos);
        }

        [HttpPost("last-commit/")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DTO.Commit))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult GetLastCommitByBranch([FromBody] DTO.Branch branch)
        {
            BO.Branch branchBo = this._branchMapper.DtoToBo(branch);

            BO.Commit commitBo = this._gitHubBusiness.GetLastCommitByBranch(branchBo);

            DTO.Commit commitDto = this._commitMapper.BoToDto(commitBo);
            return base.Ok(commitDto);
        }

        [HttpPost("comment/")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult SendComment([FromBody] DTO.Comment comment)
        {
            BO.Comment commentBo = this._commentMapper.DtoToBo(comment);

            this._gitHubBusiness.SendComment(commentBo);

            return base.Created(string.Empty, comment);
        }
    }
}
