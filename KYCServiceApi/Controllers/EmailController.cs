using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmailController : BaseController
    {
        private readonly IService<Email> _service;
        public EmailController(
            IService<Email> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Email model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
