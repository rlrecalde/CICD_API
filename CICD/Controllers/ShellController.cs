using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class ShellController : ControllerBase
    {
        private readonly BLL.Interfaces.IShell _shellBusiness;
        private readonly Mappers.IProject _projectMapper;

        public ShellController(BLL.Interfaces.IShell shellBusiness,
                               Mappers.IProject projectMapper)
        {
            this._shellBusiness = shellBusiness;
            this._projectMapper = projectMapper;
        }

        [HttpPost("dockerize/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Dockerize([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            await this._shellBusiness.Dockerize(projectBo);

            return base.Ok();
        }
    }
}
