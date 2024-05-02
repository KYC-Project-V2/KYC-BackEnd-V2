using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TemplateTypeController : BaseController
    {
        private readonly IService<TemplateType> _service;
        public TemplateTypeController(
            IService<TemplateType> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.Get();
            return Ok(response);
        }
    }
}
