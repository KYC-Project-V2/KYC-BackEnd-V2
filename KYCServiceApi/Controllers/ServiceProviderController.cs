using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Model;

using Service;
using System.Web;
using Utility;

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
        public async Task<IActionResult> Get([FromQuery] string requestNumber, string requestToken)
        {
            var SProvider=new SProvider();
            SProvider.RequestNumber = requestNumber;
            SProvider.RequestToken = requestToken;//HttpUtility.UrlDecode(
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            SProvider.SaltKey = saltkey;
            var response = await _service.Get(SProvider);
            return Ok(response);
        }
        [HttpGet("Tokencode")]
        public async Task<IActionResult> Get([FromQuery] string tokencode)
        {
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            var response = KYCUtility.decrypt(tokencode, saltkey);
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