using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Model;

using Service;
using System.Web;
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
        public ServiceProviderController(
            IService<SProvider> service, IConfiguration configuration, IService<TemplateConfiguration> templateConfigurationservice)
        {
            _service = service;
            _configuration = configuration;
            _templateConfigurationservice = templateConfigurationservice;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string requestNumber, string requestToken)
        {
            var SProvider=new SProvider();
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
            var response = KYCUtility.decrypt(tokencode, saltkey);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SProvider sprovider)
        {
            var saltkey = _configuration.GetValue<string>("SecreteEncryptDecryptKey");
            sprovider.SaltKey=saltkey;
            var response = await _service.Post(sprovider);
            return Ok(response);
        }
        [HttpPost("APIDownload")]
        public async Task<IActionResult> Post([FromBody] APIDownload apiDownload)
        {
            var templateconfigResponse = await _templateConfigurationservice.Get(8);
            var templateconfig = templateconfigResponse.FirstOrDefault();
            var apidownloadBody = templateconfig.Body;
            apidownloadBody = apidownloadBody.Replace("{{stageurl}}", apiDownload.UIStgPath);
            apidownloadBody = apidownloadBody.Replace("{{produrl}}", apiDownload.UIPrdPath);

            apidownloadBody = apidownloadBody.Replace("{{apistageurl}}", apiDownload.APIStgPath);
            apidownloadBody = apidownloadBody.Replace("{{apiprodurl}}", apiDownload.APIPrdPath);

            var apidownloadFilePath = KYCUtility.CreatePdfWithHtmlContentAPIDownload(apidownloadBody).Result;
            // Check if the file exists
            if (!System.IO.File.Exists(apidownloadFilePath))
            {
                return NotFound(); // Return a 404 Not Found response if the file doesn't exist
            }

            // Return the file as a download
            return PhysicalFile(apidownloadFilePath, "application/pdf", "APIDownload.pdf");
            //return Ok("Success");
        }

    }
}