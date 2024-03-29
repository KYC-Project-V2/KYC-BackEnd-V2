using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPanCardInfoController : BaseController
    {
        private readonly IService<BusinessPanCardInfo> _service;
        public BusinessPanCardInfoController(
            IService<BusinessPanCardInfo> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BusinessPanCardInfo model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
