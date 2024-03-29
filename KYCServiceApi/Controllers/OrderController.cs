using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;
using Service;
using System.Reflection;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IService<Order> _service;
        public OrderController(
            IService<Order> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
                var response = await _service.Post(order);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Order order)
        {
            var response = await _service.Put(order);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string requestNumber)
        {
            var response = await _service.Get(requestNumber);
            return Ok(response);
        }
    }
}
