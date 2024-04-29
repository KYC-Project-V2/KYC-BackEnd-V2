using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var response = await _service.GetCustomerData(customerRequest);
            return Ok(response);
        }



        [HttpGet]
        [Route("GetAllCustomer")]
        public async Task<IActionResult> GetAllCustomer(int status,int page, int perPage)
        {
            var response = await _service.GetAllCustomer(status, page, perPage);
            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateKYCCustomerDetails")]
        public async Task<IActionResult> UpdateKYCCustomerDetaiuls([FromBody] CustomerUpdate customerUpdate)
        {
            string certificates = string.Empty;
            string certificatesPath = string.Empty;
            var response = await _service.UpdateKYCCustomerDetails(customerUpdate, certificates, certificatesPath);
            return Ok(response);
        }


    }
}
