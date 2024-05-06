using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Newtonsoft.Json;



namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CustomerController : BaseController
    {
        private readonly IService<CustomerDetail> _service;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(IService<CustomerDetail> service, ILogger<CustomerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET single user/5
        [HttpPost]
        [Route("GetCustomer")]
        public async Task<IActionResult> GetCustomer(CustomerRequest customerRequest)
        {
            _logger.LogInformation("GetCustomer method call started: "+ JsonConvert.SerializeObject(customerRequest)+" :"+DateTime.Now );
            var response = await _service.GetCustomerData(customerRequest);
            _logger.LogInformation("GetCustomer method call end: " + JsonConvert.SerializeObject(response) + " :" + DateTime.Now);
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
