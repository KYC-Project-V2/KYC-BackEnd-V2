using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessInCorpInfoController : BaseController
    {
        private readonly IService<BusinessInCorpInfo> _service;
        public BusinessInCorpInfoController(
            IService<BusinessInCorpInfo> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BusinessInCorpInfo model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
