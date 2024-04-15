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
    public class CustomerController : BaseController
    {
        private readonly IService<CustomerDetail> _service;
        public CustomerController(IService<CustomerDetail> service)
        {
            _service = service;
        }

        // GET single user/5
        [HttpPost]
        [Route("GetCustomer")]
        public async Task<IActionResult> GetCustomer(CustomerRequest customerRequest)
        {
            var response = await _service.Get(customerRequest.RequestNo);
            return Ok(response);
        }


        
        [HttpGet]
        [Route("GetAllCustomer")]
        public async Task<IActionResult> GetAllCustomer(int status)
        {
            var response = await _service.GetAllCustomer(status);
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateKYCCustomerDetails")]
        public async Task<IActionResult> UpdateKYCCustomerDetaiuls([FromBody] CustomerUpdate customerUpdate)
        {
            var response = await _service.UpdateKYCCustomerDetails(customerUpdate);
            return Ok(response);
        }

        
    }
}
