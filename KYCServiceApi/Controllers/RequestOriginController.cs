using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Model;

using Service;


namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestOriginController : BaseController
    {
        private readonly IService<RequestOrigin> _service;
        public RequestOriginController(
            IService<RequestOrigin> service)
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
        public async Task<IActionResult> Post([FromBody] RequestOrigin requestOrigin)
        {
            var response = await _service.Post(requestOrigin);
            return Ok(response);
        }

    }
}