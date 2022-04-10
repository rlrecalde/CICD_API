using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class CommitController : ControllerBase
    {
        private readonly BLL.Interfaces.ICommit _commitBusiness;
        private readonly Mappers.ICommit _commitMapper;

        public CommitController(BLL.Interfaces.ICommit commitBusiness,
                                Mappers.ICommit commitMapper)
        {
            this._commitBusiness = commitBusiness;
            this._commitMapper = commitMapper;
        }

        [HttpGet("{branchId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DTO.Commit))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult GetLastCommitByBranchId(int branchId)
        {
            BO.Commit commitBo = this._commitBusiness.GetLastCommitByBranchId(branchId);
            DTO.Commit commitDto = this._commitMapper.BoToDto(commitBo);

            return base.Ok(commitDto);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Create([FromBody] DTO.Commit commit)
        {
            BO.Commit commitBo = this._commitMapper.DtoToBo(commit);

            this._commitBusiness.Insert(commitBo);

            return base.Ok();
        }
    }
}
