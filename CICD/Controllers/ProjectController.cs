using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly BLL.Interfaces.IProject _projectBusiness;
        private readonly Mappers.IProject _projectMapper;

        public ProjectController(BLL.Interfaces.IProject projectBusiness,
                                 Mappers.IProject projectMapper)
        {
            this._projectBusiness = projectBusiness;
            this._projectMapper = projectMapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DTO.Project>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult GetByUserId(int userId)
        {
            IEnumerable<BO.Project> projectBos = this._projectBusiness.GetByUserId(userId);
            IEnumerable<DTO.Project> projectDtos = this._projectMapper.BosToDtos(projectBos);

            return base.Ok(projectDtos);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(DTO.Project))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Create([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            this._projectBusiness.Insert(projectBo);

            DTO.Project projectDto = this._projectMapper.BoToDto(projectBo);
            return base.Created(string.Empty, projectDto);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Update([FromBody] DTO.Project project)
        {
            BO.Project projectBo = this._projectMapper.DtoToBo(project);

            this._projectBusiness.Update(projectBo);

            return base.Ok();
        }

        [HttpDelete("{projectId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Delete(int projectId)
        {
            await this._projectBusiness.Delete(new BO.Project { Id = projectId });

            return base.Ok();
        }
    }
}
