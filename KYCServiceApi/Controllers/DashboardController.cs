using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Model;
using Service;



namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DashboardController : BaseController
    {
        private readonly IService<Dashboard> _service;
        public DashboardController(IService<Dashboard> service)
        {
            _service = service;
        }

        // GET Dashboard Data
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.GetDashboardData();
            return Ok(response);
        }
    }
}
