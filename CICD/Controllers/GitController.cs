using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class GitController : ControllerBase
    {
        private readonly BLL.Interfaces.IGit _gitBusiness;
        private readonly Mappers.IProject _projectMapper;
        private readonly Mappers.IBranch _branchMapper;

        public GitController(BLL.Interfaces.IGit gitBusiness,
                             Mappers.IProject projectMapper,
                             Mappers.IBranch branchMapper)
        {
            this._gitBusiness = gitBusiness;
            this._projectMapper = projectMapper;
            this._branchMapper = branchMapper;
        }

        [HttpPost("head-branch/")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DTO.Branch))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> GetHeadBranch([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            BO.Branch headBranch = await this._gitBusiness.GetHeadBranch(projectBo);
            
            DTO.Branch headBranchDto = this._branchMapper.BoToDto(headBranch);
            return base.Ok(headBranchDto);
        }

        [HttpPost("clone/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Clone([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            await this._gitBusiness.Clone(projectBo);

            return base.Ok();
        }

        [HttpPost("fetch/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Fetch([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            await this._gitBusiness.Fetch(projectBo);

            return base.Ok();
        }

        [HttpPost("switch/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Switch([FromBody] DTO.Branch branch)
        {
            BO.Branch branchBo = this._branchMapper.DtoToBo(branch);

            await this._gitBusiness.Switch(branchBo);

            return base.Ok();
        }

        [HttpPost("pull/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Pull([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            await this._gitBusiness.Pull(projectBo);

            return base.Ok();
        }
    }
}
