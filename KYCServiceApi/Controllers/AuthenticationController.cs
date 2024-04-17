using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;
using Repository;
using Service;
using Utility;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IJWTService _IJWTService;
        private readonly IService<Authentication> _service;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IJWTService iJWTService,
            IService<Authentication> service,
            IConfiguration configuration)
        {
            _IJWTService = iJWTService;
            _service = service;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string code, string secretKey)
        {
            Tokens tokens = null;
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            
            var user = new Authentication
            {
                Code = code,
                SecretKey = KYCUtility.encrypt(secretKey, saltkey)
            };
            var response = await _service.Get(user);
            if(response == null)
            {
                return Unauthorized();
            }
            else
            {
                tokens = await _IJWTService.Authenticate(user);
                tokens.ReturnUrl = response.ReturnUrl;
            }
            return Ok(tokens);
        }
    }
}
