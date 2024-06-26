﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Model;
using Service;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Twilio.Rest;
using Utility;
using Certificate = Model.Certificate;

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
            var lstX509Certificates = KYCUtility.GetX509CACertificate();
            //MemoryStream stream = new MemoryStream(apidownloadFilebytes);

            // Return the file as a download
            //return File(stream, "application/cer", "CACertificate.pfx");
            var certificate = new RootCertificate();
            certificate.CreatedDate = DateTime.Now;
            certificate.ExpireDate = DateTime.Now.AddYears(24);
            certificate.Certificates = Convert.ToBase64String(lstX509Certificates[0].Export(X509ContentType.Pfx));
            certificate.CerCertificates = Convert.ToBase64String(lstX509Certificates[0].Export(X509ContentType.Cert));
            var rootcertificate = await _rootCertificateService.Post(certificate);
            byte[] byteArray = Convert.FromBase64String(rootcertificate.CerCertificates);
            MemoryStream stream = new MemoryStream(byteArray);

            //Return the file as a download
            return File(stream, "application/cer", "Astitvaroot.cer");
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
    }
}