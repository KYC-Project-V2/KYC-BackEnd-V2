using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IJWTService _IJWTService;
        private readonly IService<Authentication> _service;

        public AuthenticationController(IJWTService iJWTService,
            IService<Authentication> service)
        {
            _IJWTService = iJWTService;
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string code, string secretKey)
        {
            Tokens tokens = null;
            var user = new Authentication
            {
                Code = code,
                SecretKey = secretKey
            };
            var response = await _service.Get(user);
            if(response == null)
            {
                return Unauthorized();
            }
            else
            {
                tokens = await _IJWTService.Authenticate(user);
            }
            return Ok(tokens);
        }
    }
}
