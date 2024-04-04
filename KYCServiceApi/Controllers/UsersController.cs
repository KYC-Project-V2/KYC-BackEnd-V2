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
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var response = await _service.Get(userId);
            return Ok(response);
        }


        // GET all user bydefaul no need to pass method name //https://localhost:44372/api/Users
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
            var response = await _service.Post(userDetail);
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Put([FromBody] UserDetail userDetail)
        {
            var response = _service.Put(userDetail);
            return Ok(response);
        }
    }
}
