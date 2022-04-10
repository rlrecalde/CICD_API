using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly BLL.Interfaces.IConfiguration _configurationBusiness;
        private readonly Mappers.IConfiguration _configurationMapper;

        public ConfigurationController(BLL.Interfaces.IConfiguration configurationBusiness,
                                       Mappers.IConfiguration configurationMapper)
        {
            this._configurationBusiness = configurationBusiness;
            this._configurationMapper = configurationMapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DTO.Configuration>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Get()
        {
            IEnumerable<BO.Configuration> configurationsBo = this._configurationBusiness.Get();
            IEnumerable<DTO.Configuration> configurationsDto = this._configurationMapper.BosToDtos(configurationsBo);

            return base.Ok(configurationsDto);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(DTO.Configuration))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Create([FromBody] DTO.Configuration configuration)
        {
            BO.Configuration configurationBo = this._configurationMapper.DtoToBo(configuration);

            this._configurationBusiness.Insert(configurationBo);
            
            DTO.Configuration configurationDto = this._configurationMapper.BoToDto(configurationBo);
            return base.Created(string.Empty, configurationDto);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Update([FromBody] DTO.Configuration configuration)
        {
            BO.Configuration configurationBo = this._configurationMapper.DtoToBo(configuration);

            this._configurationBusiness.Update(configurationBo);

            return base.Ok();
        }
    }
}
