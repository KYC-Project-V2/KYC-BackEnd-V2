using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverLicenseInfoController : BaseController
    {
        private readonly IService<DriverLicenseInfo> _service;
        public DriverLicenseInfoController(
            IService<DriverLicenseInfo> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DriverLicenseInfo model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
