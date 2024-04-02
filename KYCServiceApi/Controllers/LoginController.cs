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
    public class LoginController : BaseController
    {
        private readonly IService<LoginUser> _service;
        public LoginController(
            IService<LoginUser> service)
        {
            _service = service;
        }
        
        // GET api/<LoginController>/5
        [HttpGet()]
        public async Task<IActionResult> Login([FromQuery] string UserId, [FromQuery] string Password)
        {
            var model = new LoginUser
            {
                UserId = UserId,
                Password = Password,
                 
            };
            var response = await _service.Get(model);
            return Ok(response);
        }


    }
}
