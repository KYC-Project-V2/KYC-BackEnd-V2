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
        public async Task<IActionResult> GetAllCustomer()
        {
            var response = await _service.GetAllCustomer();
            return Ok(response);
        }

        //[HttpPost]
        //[Route("AddUser")]
        //public async Task<IActionResult> AddUser([FromBody] UserDetail userDetail)
        //{
        //    var response = await _service.AddUser(userDetail);
        //    return Ok(response);
        //}

        //[HttpPut]
        //[Route("UpdateUser")]
        //public async Task<IActionResult> Put([FromBody] UserDetail userDetail)
        //{
        //    var response = await _service.UpdateUser(userDetail);
        //    return Ok(response);
        //}
    }
}
