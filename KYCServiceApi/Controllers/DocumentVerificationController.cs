using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentVerificationController : BaseController
    {
        private readonly IService<DocumentVerification> _service;
        public DocumentVerificationController(
            IService<DocumentVerification> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DocumentVerification model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
