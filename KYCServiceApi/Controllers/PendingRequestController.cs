using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;
using Service;
namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingRequestController : BaseController
    {
        private readonly IService<PendingRequest> _service;
        public PendingRequestController(
           IService<PendingRequest> service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string code)
        {
            var response = await _service.Get(code);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PendingRequest pendingRequest)
        {
            var response = await _service.Post(pendingRequest);
            return Ok(response);
        }
    }
}
