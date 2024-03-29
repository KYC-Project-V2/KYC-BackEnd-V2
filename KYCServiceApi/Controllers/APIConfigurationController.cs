using Microsoft.AspNetCore.Mvc;

using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIConfigurationController : BaseController
    {
        private readonly IService<APIConfiguration> _service;
        public APIConfigurationController(
            IService<APIConfiguration> service)
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
        public async Task<IActionResult> Put([FromBody] APIConfiguration aPIConfiguration)
        {
            var response = _service.Put(aPIConfiguration);
            return Ok(aPIConfiguration);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] APIConfiguration aPIConfiguration)
        {
            var response = await _service.Delete(aPIConfiguration);
            return Ok(response);
        }
    }
}
