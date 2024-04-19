using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailConfigurationController : BaseController
    {
        private readonly IService<EmailConfiguration> _service;
        public EmailConfigurationController(
            IService<EmailConfiguration> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.Get();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EmailConfiguration emailConfiguration)
        {
            var response = _service.Put(emailConfiguration);
            return Ok(emailConfiguration);
        }
    }
}


