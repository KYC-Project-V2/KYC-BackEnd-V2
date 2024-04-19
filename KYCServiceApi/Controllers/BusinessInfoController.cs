using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessInfoController : BaseController
    {
        private readonly IService<BusinessInfo> _service;
        public BusinessInfoController(
           IService<BusinessInfo> service)
        {
            _service = service;
        }
        //[HttpGet]
        //public async Task<IActionResult> Get([FromQuery] int Id)
        //{
        //    var response = await _service.Get(Id);
        //    return Ok(response);
        //}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string code)
        {
            var response = await _service.Get(code);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BusinessInfo businessInfo)
        {
            var response = _service.Put(businessInfo);
            return Ok(businessInfo);
        }
    }
}
