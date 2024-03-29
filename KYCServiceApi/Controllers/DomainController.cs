using Microsoft.AspNetCore.Mvc;

using Model;

using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : BaseController
    {
        private readonly IService<Domain> _service;
        public DomainController(
            IService<Domain> service)
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
        public async Task<IActionResult> Post([FromBody] Domain model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Domain model)
        {
            var response = await _service.Delete(model);
            return Ok(response);
        }

        [HttpPost]
        [Route("updateDomains")]
        public async Task<IActionResult> updateDomains([FromBody] List<Domain> model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
