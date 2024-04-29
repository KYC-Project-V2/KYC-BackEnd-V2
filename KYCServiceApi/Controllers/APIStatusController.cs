using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;
using Model;
using Repository;
using Utility;
using Twilio.Jwt.AccessToken;
namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIStatusController : ControllerBase
    {
        private readonly IConfiguration? _configuration;
        private readonly IService<APIStatus>? _apiservice;
       

        public APIStatusController(IConfiguration configuration, IService<APIStatus> apiservice)
        {
            _configuration = configuration;
            _apiservice = apiservice;   
           
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] APIStatus apiStatus)
        {
            try
            {
                var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
                var tokenresponse = KYCUtility.decrypt(apiStatus.TokenID, saltkey);
                var tokenresponsesplitwithand = tokenresponse.Split('&');
                var requestNumber = tokenresponsesplitwithand[0].Split('=')[1];
                var tokenNumber = KYCUtility.encrypt(tokenresponsesplitwithand[1].Split('=')[1], saltkey);
                apiStatus.RequestNumber = requestNumber;
                apiStatus.TokenNumber = tokenNumber;
                var apiresponse = await _apiservice.Get(apiStatus);
                if (apiresponse != null && !string.IsNullOrEmpty(apiresponse.RequestErrorMessage))
                {
                    return Ok(new { ErrorMessage = apiresponse.RequestErrorMessage });
                }
                else
                {
                    var responseObject = new
                    {
                        apiresponse.CustomerNumber,
                        apiresponse.OrderNumber,
                        apiresponse.CertificateExpire,
                        apiresponse.Status
                    };
                    return Ok(responseObject);
                }
            }
           catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("ServiceProviderPostInfo")]
        public async Task<IActionResult> ServiceProviderPostInfo([FromBody] APIStatus apiStatus)
        {
            try
            {
                var apiresponse = await _apiservice.Post(apiStatus);
                return Ok(new { ErrorMessage = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string tokenNumber)
        {
            try
            {
                var apiresponse = await _apiservice.Get(tokenNumber);
                return Ok(apiresponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
