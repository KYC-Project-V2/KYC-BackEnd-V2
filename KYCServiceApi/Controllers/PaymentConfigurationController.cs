using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentConfigurationController : BaseController
    {
        private readonly IService<PaymentConfiguration> _service;
        public PaymentConfigurationController(
            IService<PaymentConfiguration> service)
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
        public async Task<IActionResult> Put([FromBody] PaymentConfiguration paymentConfiguration)
        {
            var response = _service.Put(paymentConfiguration);
            return Ok(paymentConfiguration);
        }

        [HttpGet]
        [Route("GetActivePaymentConfiguration")]
        public async Task<IActionResult> Get(bool status)
        {
            var response = await _service.Get(status);
            return Ok(response);
        }
    }
}
