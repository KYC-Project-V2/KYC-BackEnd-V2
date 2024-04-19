using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TemplateConfigurationController : BaseController
    {
        private readonly IService<TemplateConfiguration> _service;
        public TemplateConfigurationController(
            IService<TemplateConfiguration> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int Id)
        {
            var response = await _service.Get(Id);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TemplateConfiguration templateConfiguration)
        {
            var response = _service.Put(templateConfiguration);
            return Ok(templateConfiguration);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] TemplateConfiguration templateConfiguration)
        {
            var response = await _service.Delete(templateConfiguration);
            return Ok(response);
        }
    }
}
