using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AadharInfoController : BaseController
    {
        private readonly IService<AadharInfo> _service;
        public AadharInfoController(
            IService<AadharInfo> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AadharInfo model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
