using Microsoft.AspNetCore.Mvc;

using Model;

using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : BaseController
    {
        private readonly IService<Certificate> _service;
        public CertificateController(
            IService<Certificate> service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string code)
        {
            var response = await _service.GetAll(code);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Certificate certificate)
        {
            var response = await _service.Post(certificate);
            return Ok(response);
        }
    }
}
