using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Model;
using Service;



namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : BaseController
    {
        private readonly IService<UserDetail> _service;
        public UsersController(IService<UserDetail> service)
        {
            _service = service;
        }

        // GET single user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var response = await _service.Get(id);
            return Ok(response);
        }


        
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var response = await _service.Get();
            return Ok(response);
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDetail userDetail)
        {
            var response = await _service.AddUser(userDetail);
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Put([FromBody] UserDetail userDetail)
        {
            var response = await _service.UpdateUser(userDetail);
            return Ok(response);
        }
    }
}
