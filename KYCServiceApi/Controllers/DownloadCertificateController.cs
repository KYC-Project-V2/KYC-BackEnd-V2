using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Model;
using Org.BouncyCastle.Asn1.X509;
using Service;
using System.Reflection;
using System.Text;
using Twilio.Rest;
using Utility;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DownloadCertificateController : BaseController
    {
        private readonly IService<RequestVerification> _service;
        private readonly IService<RootCertificate> _rootCertificateService;
        private readonly IConfiguration _configuration;
        private readonly IService<Certificate> _certificateService;
        public DownloadCertificateController(
            IService<RequestVerification> service, IConfiguration configuration, IService<RootCertificate> rootCertificateService, IService<Certificate> certificateService)
        {
            _service = service;
            _configuration = configuration;
            _rootCertificateService = rootCertificateService;
            _certificateService = certificateService;
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
        [HttpGet, Route("DownloadCACertificate")]
        public async Task<IActionResult> CreateX509CACertificate()
        {
            var apidownloadFilebytes = KYCUtility.GetX509CACertificate();
            //MemoryStream stream = new MemoryStream(apidownloadFilebytes);

            // Return the file as a download
            //return File(stream, "application/cer", "CACertificate.pfx");
            var certificate = new RootCertificate();
            certificate.CreatedDate = DateTime.Now;
            certificate.ExpireDate = DateTime.Now.AddYears(24);
            certificate.Certificates = Convert.ToBase64String(apidownloadFilebytes);
            var rootcertificate = await _rootCertificateService.Post(certificate);
            byte[] byteArray = Convert.FromBase64String(rootcertificate.Certificates);
            MemoryStream stream = new MemoryStream(byteArray);

            //Return the file as a download
            return File(stream, "application/cer", "CACertificate.pfx");
        }
        [HttpGet, Route("DownloadMyCertificate")]
        public async Task<IActionResult> GetX509Certificate([FromQuery] string domainname)
        {
            var certificate = await _certificateService.Get(domainname);
            byte[] byteArray = Convert.FromBase64String(certificate.CertificatePath);
            MemoryStream stream = new MemoryStream(byteArray);

            // Return the file as a download
            return File(stream, "application/cer", "Certificate.cer");
        }
        [HttpGet, Route("DownloadExternalCertificate")]
        public async Task<IActionResult> GetX509ExternalCertificate([FromQuery] string domainname)
        {
            var x509certificate = new Model.X509Certificate();
            Guid newGuid = Guid.NewGuid();
            x509certificate.IsProvisional = false;
            x509certificate.DomainName = domainname;
            x509certificate.RequestNumber = newGuid.ToString();
            var rootcertificate = await _rootCertificateService.Get(string.Empty);
            x509certificate.CARootPath = rootcertificate.Certificates;
            var apidownloadFilebytes = KYCUtility.GetX509Certificate(x509certificate);
            MemoryStream stream = new MemoryStream(apidownloadFilebytes.CertificateBytes);

            // Return the file as a download
            return File(stream, "application/cer", "Certificate.cer");
        }
    }
}