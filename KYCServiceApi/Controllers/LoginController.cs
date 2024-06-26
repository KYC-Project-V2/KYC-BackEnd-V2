﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;



namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private readonly IService<LoginUser> _service;
        public LoginController(
            IService<LoginUser> service)
        {
            _service = service;
        }

        // Post api/<LoginController>/5
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserRequest loginUserRequest)
        {
            var model = new LoginUser
            {
                UserId = loginUserRequest.UserId,
                Password = loginUserRequest.Password,

            };
            var response = await _service.Get(model);
            return Ok(response);
        }


    }
}
