using Microsoft.AspNetCore.Mvc;

using Model;
using Service;
using System.Reflection;
using System.Text;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadCertificateController : BaseController
    {
        private readonly IService<RequestVerification> _service;
        private readonly IConfiguration _configuration;

        public DownloadCertificateController(
            IService<RequestVerification> service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetScript([FromQuery] string dn)
        {
            var origin = dn;
            var requestVerification = new RequestVerification
            {
                Origin = origin,
                RedirectUrl = _configuration.GetValue<string>("KYCWebUrl")
            };
            var response = await _service.Post(requestVerification);
            return Ok(response.HtmlBody);
        }

        [HttpGet, Route("CertificateDetails")]
        public async Task<IActionResult> GetCertificateDetails([FromQuery] string dn)
        {
            var response = await _service.Get(dn);

            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = response.GetType().GetProperties();
            sb.Append("KYC Certificate details.\n");
            sb.Append("---------------------------------\n");
            sb.Append("\n");
            foreach (PropertyInfo pi in properties)
            {
                var value = pi.GetValue(response, null);
                if (value != null)
                {
                    sb.Append($"{pi.Name}: {pi.GetValue(response, null)}\n");
                }
            }
            sb.Append("\n");
            sb.Append("Verified by AstitvaTech.com");
            return Ok(sb.ToString());
        }
    }
}