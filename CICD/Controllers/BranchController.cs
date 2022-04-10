using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly BLL.Interfaces.IBranch _branchBusiness;
        private readonly Mappers.IBranch _branchMapper;

        public BranchController(BLL.Interfaces.IBranch branchBusiness,
                                Mappers.IBranch branchMapper)
        {
            this._branchBusiness = branchBusiness;
            this._branchMapper = branchMapper;
        }

        [HttpGet("{projectId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DTO.Branch>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult GetByProjectId(int projectId)
        {
            IEnumerable<BO.Branch> branchesBo = this._branchBusiness.GetByProjectId(projectId);
            IEnumerable<DTO.Branch> branchesDto = this._branchMapper.BosToDtos(branchesBo);

            return base.Ok(branchesDto);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(DTO.Branch))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Create([FromBody] DTO.Branch branch)
        {
            BO.Branch branchBo = this._branchMapper.DtoToBo(branch);

            this._branchBusiness.Insert(branchBo);

            DTO.Branch branchDto = this._branchMapper.BoToDto(branchBo);
            return base.Created(string.Empty, branchDto);
        }

        [HttpPost("many/")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(IEnumerable<DTO.Branch>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult CreateMany([FromBody] IEnumerable<DTO.Branch> branches)
        {
            IEnumerable<BO.Branch> branchBos = this._branchMapper.DtosToBos(branches);

            this._branchBusiness.InsertMany(branchBos);

            IEnumerable<DTO.Branch> branchDtos = this._branchMapper.BosToDtos(branchBos);
            return base.Created(string.Empty, branchDtos);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult MarkAsDeleted([FromBody] DTO.Branch branch)
        {
            BO.Branch branchBo = this._branchMapper.DtoToBo(branch);

            this._branchBusiness.MarkAsDeleted(branchBo);

            return base.Ok();
        }

        [HttpPut("many/")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult MarkManyAsDeleted([FromBody] IEnumerable<DTO.Branch> branches)
        {
            IEnumerable<BO.Branch> branchBos = this._branchMapper.DtosToBos(branches);

            this._branchBusiness.MarkManyAsDeleted(branchBos);

            return base.Ok();
        }
    }
}
