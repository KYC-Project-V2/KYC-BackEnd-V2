using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInfoController : BaseController
    {
        private readonly IService<PersonalInfo> _service;
        public PersonalInfoController(
            IService<PersonalInfo> service)
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

        public async Task<IActionResult> Put([FromBody] PersonalInfo personalInfo)
        {
            var response = _service.Put(personalInfo);
            return Ok(personalInfo);
        }
    }
}
