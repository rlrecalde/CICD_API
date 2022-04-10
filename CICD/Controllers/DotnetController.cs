using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class DotnetController : ControllerBase
    {
        private readonly BLL.Interfaces.IDotnet _dotnetBusiness;
        private readonly Mappers.IProject _projectMapper;

        public DotnetController(BLL.Interfaces.IDotnet dotnetBusiness,
                                Mappers.IProject projectMapper)
        {
            this._dotnetBusiness = dotnetBusiness;
            this._projectMapper = projectMapper;
        }

        [HttpGet("versions/")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<string>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> GetVersions()
        {
            IEnumerable<string> versions = await this._dotnetBusiness.GetVersions();

            return base.Ok(versions);
        }

        [HttpPost("build/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Build([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            await this._dotnetBusiness.Build(projectBo);

            return base.Ok();
        }

        [HttpPost("test/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Test([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            await this._dotnetBusiness.Test(projectBo);

            return base.Ok();
        }
    }
}
