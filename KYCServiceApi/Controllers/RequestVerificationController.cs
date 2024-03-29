using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestVerificationController : BaseController
    {
        private readonly IService<RequestVerification> _service;
        private readonly IConfiguration _configuration;

        public RequestVerificationController(
            IService<RequestVerification> service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var origin = Request.Headers["Origin"].FirstOrDefault();
            var requestVerification = new RequestVerification
            {
                Origin = origin,
                RedirectUrl = _configuration.GetValue<string>("KYCWebUrl")
            };
            var response = await _service.Post(requestVerification);
            return Ok(response.HtmlBody);
        }

        [HttpGet, Route("details")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetails([FromQuery] string dn)
        {
            var response = await _service.Get(dn);
            return Ok(response);
        }
    }
}
