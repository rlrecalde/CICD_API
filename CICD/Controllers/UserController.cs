using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CICD.Controllers
{
    [Route("[controller]")]
    [ServiceFilter(typeof(Filters.ApiActionFilter))]
    [ServiceFilter(typeof(Filters.ApiExceptionFilter))]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BLL.Interfaces.IUser _userBusiness;
        private readonly Mappers.IUser _userMapper;

        public UserController(BLL.Interfaces.IUser userBusiness,
                              Mappers.IUser userMapper)
        {
            this._userBusiness = userBusiness;
            this._userMapper = userMapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DTO.User>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Get()
        {
            IEnumerable<BO.User> userBos = this._userBusiness.Get();
            IEnumerable<DTO.User> userDtos = this._userMapper.BosToDtos(userBos);

            return base.Ok(userDtos);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(DTO.User))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Create([FromBody] DTO.User user)
        {
            BO.User userBo = this._userMapper.DtoToBo(user);

            this._userBusiness.Insert(userBo);

            DTO.User userDto = this._userMapper.BoToDto(userBo);
            return base.Created(string.Empty, userDto);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Update([FromBody] DTO.User user)
        {
            BO.User userBo = this._userMapper.DtoToBo(user);

            this._userBusiness.Update(userBo);

            return base.Ok();
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Delete(int userId)
        {
            this._userBusiness.Delete(new BO.User { Id = userId });

            return base.Ok();
        }
    }
}
