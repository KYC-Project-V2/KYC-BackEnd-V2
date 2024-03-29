using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateConfirmationController : BaseController
    {
        private readonly IService<CertificateConfirmation> _service;
        private readonly IConfiguration _configuration;
        public CertificateConfirmationController(IService<CertificateConfirmation> service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.GetAll(_configuration.GetValue<string>("CCNoofDays"));
            return Ok(response);
        }
    }
}
