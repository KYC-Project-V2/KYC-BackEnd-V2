using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPVerificationController : BaseController
    {
        private readonly IService<OTPVerification> _service;
        public OTPVerificationController(
            IService<OTPVerification> service)
        {
            _service = service;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OTPVerification model)
        {
            var response = await _service.Put(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OTPVerification model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] OTPVerification model)
        {
            var response = await _service.Delete(model);
            return Ok(response);
        }

        [HttpPut]
        [Route("Verify")]
        public async Task<IActionResult> Verify([FromBody] OTPVerification model)
        {
            var response = await _service.Get(model);
            return Ok(response);
        }
    }
}
