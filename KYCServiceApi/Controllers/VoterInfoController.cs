using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoterInfoController : BaseController
    {
        private readonly IService<VoterInfo> _service;
        public VoterInfoController(
            IService<VoterInfo> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VoterInfo model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
    }
}
