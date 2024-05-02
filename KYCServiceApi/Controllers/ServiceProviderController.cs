using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Model;

using Service;
using Utility;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ServiceProviderController : BaseController
    {
        private readonly IService<SProvider> _service;
        private readonly IConfiguration _configuration;
        IService<TemplateConfiguration> _templateConfigurationservice;
        private readonly IConverter _converter;
        public ServiceProviderController(
            IService<SProvider> service, IConfiguration configuration, IService<TemplateConfiguration> templateConfigurationservice, IConverter converter)
        {
            _service = service;
            _configuration = configuration;
            _templateConfigurationservice = templateConfigurationservice;
            _converter = converter;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string requestNumber, string requestToken)
        {
            var SProvider = new SProvider();
            SProvider.RequestNumber = requestNumber;
            SProvider.RequestToken = requestToken;//HttpUtility.UrlDecode(
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            SProvider.SaltKey = saltkey;
            var response = await _service.Get(SProvider);
            return Ok(response);
        }
        [HttpGet("Tokencode")]
        public async Task<IActionResult> Get([FromQuery] string tokencode)
        {
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            var responsedecrypt = KYCUtility.decrypt(tokencode, saltkey);
            var responsetokencodesplit= responsedecrypt.Split('&');
            var SProvider = new SProvider();
            SProvider.RequestNumber = responsetokencodesplit[0].Split('=')[1];
            SProvider.RequestToken = responsetokencodesplit[1].Split('=')[1];
            SProvider.SaltKey = saltkey;
            var response = await _service.Get(SProvider);
            return Ok(response);
        }
        [HttpGet("ValidateTokencode")]
        public async Task<IActionResult> ValidateTokencode([FromQuery] string tokenurl, [FromQuery] string requesttoken)
        {
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            var responsedecrypt = KYCUtility.decrypt(tokenurl, saltkey);
            var responsetokencodesplit = responsedecrypt.Split('&');
            var SProvider = new SProvider();
            SProvider.RequestNumber = responsetokencodesplit[0].Split('=')[1];
            SProvider.RequestToken = responsetokencodesplit[1].Split('=')[1];
            if(SProvider.RequestToken!= requesttoken)
            {
                return Ok(new { ErrorMessage = "Invalid" });
            }
            else
            {
                SProvider.SaltKey = saltkey;
                var response = await _service.Get(SProvider);
                return Ok(response);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SProvider sprovider)
        {
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            sprovider.SaltKey = saltkey;
            var response = await _service.Post(sprovider);
            return Ok(response);
        }
        [HttpGet("APIDownload")]
        public async Task<IActionResult> Post([FromQuery] string tokencode)
        {
            //var isprod = _configuration.GetValue<bool>("IsProd");
            //var kycweburl = _configuration.GetValue<string>("KYCWebUrl");
            //var kycapiurl = _configuration.GetValue<string>("KYCAPIUrl");

            var templateconfigResponse = await _templateConfigurationservice.Get(8);
            var templateconfig = templateconfigResponse.FirstOrDefault();
            var apidownloadBody = templateconfig.Body;

            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            var responsedecrypt = KYCUtility.decrypt(tokencode.Replace(" ","+"), saltkey);
            var responsetokencodesplit = responsedecrypt.Split('&');
            var SProvider = new SProvider();
            SProvider.RequestNumber = responsetokencodesplit[0].Split('=')[1];
            SProvider.RequestToken = responsetokencodesplit[1].Split('=')[1];
            SProvider.SaltKey = saltkey;
            var response = await _service.Get(SProvider);

            apidownloadBody = apidownloadBody.Replace("{Name of Service Provider}", response?.ProviderName);
            apidownloadBody = apidownloadBody.Replace("{requestNumber}", response?.RequestNumber);
            apidownloadBody = apidownloadBody.Replace("{requestToken}", response?.RequestToken);
            apidownloadBody = apidownloadBody.Replace("{tokenCode}", response?.Tokencode);
            apidownloadBody = apidownloadBody.Replace("{email}", response?.Email);
            apidownloadBody = apidownloadBody.Replace("{phoneNumber}", response?.PhoneNumber);
            apidownloadBody = apidownloadBody.Replace("{returnURL}", response?.ReturnUrl);
            apidownloadBody = apidownloadBody.Replace("{ReturnURL}", response?.ReturnUrl);
            apidownloadBody = apidownloadBody.Replace("{RequestToken}", response?.RequestToken);
            apidownloadBody = apidownloadBody.Replace("{RequestToken}", response?.RequestToken);
            apidownloadBody = apidownloadBody.Replace("{RequestToken}", response?.RequestToken);
            apidownloadBody = apidownloadBody.Replace("{RequestToken}", response?.RequestToken);
            apidownloadBody = apidownloadBody.Replace("{RequestToken}", response?.RequestToken);
            //if (isprod)
            //{
            //    apidownloadBody = apidownloadBody.Replace("{{stageurl}}", string.Empty);
            //    apidownloadBody = apidownloadBody.Replace("{{produrl}}", kycweburl + "/welcomerequest?id=" + tokencode);

            //    apidownloadBody = apidownloadBody.Replace("{{apistageurl}}", string.Empty);
            //    apidownloadBody = apidownloadBody.Replace("{{apiprodurl}}", kycapiurl);
            //}
            //else
            //{
            //    apidownloadBody = apidownloadBody.Replace("{{stageurl}}", kycweburl + "/welcomerequest?id=" + tokencode);
            //    apidownloadBody = apidownloadBody.Replace("{{produrl}}", string.Empty);

            //    apidownloadBody = apidownloadBody.Replace("{{apistageurl}}", kycapiurl);
            //    apidownloadBody = apidownloadBody.Replace("{{apiprodurl}}", string.Empty);
            //}

            var apidownloadFilePath = KYCUtility.CreatePdfWithHtmlContentAPIDownload(apidownloadBody, _converter).Result;
            // Check if the file exists
            if (!System.IO.File.Exists(apidownloadFilePath))
            {
                return NotFound(); // Return a 404 Not Found response if the file doesn't exist
            }

            // Return the file as a download
            return PhysicalFile(apidownloadFilePath, "application/pdf", "APIDownload.pdf");
            //return Ok("Success");
        }

        [HttpGet("GetAllServiceProvider")]
        public async Task<IActionResult> GetAllServiceProvider()
        {           
            var response = await _service.GetAllServiceProvider();
            return Ok(response);
        }

        [HttpPost("GetServiceProvider")]
        public async Task<IActionResult> GetServiceProvider(ServiceProviderRequest serviceProviderRequest)
        {
            var response = await _service.GetServiceProvider(serviceProviderRequest);
            return Ok(response);
        }

        [HttpPost("UpdateServiceProvider")]
        public async Task<IActionResult> UpdateServiceProvider(UpdateServiceProvider updateServiceProvider)
        {
            var response = await _service.UpdateServiceProvider(updateServiceProvider);
            return Ok(response);
        }
    }
}