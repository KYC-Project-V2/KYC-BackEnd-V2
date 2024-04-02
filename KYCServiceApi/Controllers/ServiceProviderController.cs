using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Model;

using Service;


namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ServiceProviderController : BaseController
    {
        private readonly IService<SProvider> _service;
        private readonly IConfiguration _configuration;
        public ServiceProviderController(
            IService<SProvider> service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string code)
        {
            var response = await _service.Get(code);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SProvider requestOrigin)
        {
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            requestOrigin.SaltKey=saltkey;
            var response = await _service.Post(requestOrigin);
            return Ok(response);
        }

    }
}