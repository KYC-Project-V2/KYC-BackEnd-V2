using Microsoft.Extensions.Configuration;
using Model;
using Repository;
using System;
using Twilio.Http;

namespace Service
{
    public class RequestVerificationService : BaseService<RequestVerification>
    {
        private readonly IRepository<RequestOrigin> _requestOriginRepository;
        private readonly IRepository<PersonalInfo> _personalInfoRepository;
        private readonly IRepository<BusinessInfo> _businessInfoRepository;
        private readonly IRepository<Certificate> _certificateRepository;
        private readonly IService<PaymentConfiguration> _paymentCongurationService;
        private readonly IConfiguration _configuration;
        public RequestVerificationService(
              IRepository<RequestOrigin> requestOriginRepository,
            IRepository<PersonalInfo> personalInfoRepository,
            IRepository<BusinessInfo> businessInfoRepository,
            IRepository<Certificate> certificateRepository,
            IService<PaymentConfiguration> paymentCongurationService,
            IConfiguration configuration)
        {

            _requestOriginRepository = requestOriginRepository;
            _personalInfoRepository = personalInfoRepository;
            _businessInfoRepository = businessInfoRepository;
            _certificateRepository = certificateRepository;
            _paymentCongurationService = paymentCongurationService;
            _configuration = configuration;
        }

        public override async Task<RequestVerification> Get(string domainName)
        {
            RequestVerification requestVerification = null;

            if (domainName.Contains("://"))
            {
                domainName = domainName.Split("://")[1];
            }

            if (domainName.Contains(":"))
            {
                domainName = domainName.Split(':')[0];
            }

            if (domainName.Contains("/"))
            {
                domainName = domainName.Split("/")[0];
            }

            if (domainName.Contains("?"))
            {
                domainName = domainName.Split("?")[0];
            }

            var certificate = await _certificateRepository.Get(domainName);

            if (certificate == null)
            {
                return requestVerification;
            }

            var requestOrigin = await _requestOriginRepository.Get(certificate.RequestNumber);
            if (requestOrigin == null)
            {
                return requestVerification;
            }

            var personalInfo = await _personalInfoRepository.Get(requestOrigin.RequestNo);
            var businessInfo = await _businessInfoRepository.Get(requestOrigin.RequestNo);

            requestVerification = new RequestVerification
            {
                CertificateId = certificate.Id,
                RequestNumber = requestOrigin.RequestNo,
                DomainName = certificate.DomainName,
                FirstName = personalInfo != null ? personalInfo.FirstName : businessInfo.ContactFirstName,
                LastName = personalInfo != null ? personalInfo.LastName : businessInfo.ContactLastName,
                Phone = personalInfo != null ? personalInfo.Phone : businessInfo.ContactPhone,
                Email = personalInfo != null ? personalInfo.Email : businessInfo.OfficialEmail
            };

            var noDays = Convert.ToInt32(certificate.ExpiredOn);

            requestVerification.CreatedDate = certificate.CreatedDate;
            requestVerification.ExpiryDate = certificate.ExpireDate;
            requestVerification.ExpiredOn = certificate.ExpiredOn;

            if (noDays > 0)
            {
                requestVerification.Status = true;
                requestVerification.StatusMessgae = "Valid";
            }
            else
            {
                requestVerification.Status = false;
                requestVerification.StatusMessgae = "Certificate Expired";
            }
            return requestVerification;
        }
        public override async Task<RequestVerification> Post(RequestVerification model)
        {
            RequestVerification requestVerification = null;
            
            model.DomainName = !string.IsNullOrEmpty(model.DomainName) ? model.DomainName : model.Origin;
            var domainName = model.DomainName;

            if (domainName.Contains("://"))
            {
                domainName = domainName.Split("://")[1];
            }

            if (domainName.Contains(":"))
            {
                domainName = domainName.Split(':')[0];
            }

            if (domainName.Contains("/"))
            {
                domainName = domainName.Split("/")[0];
            }

            if (domainName.Contains("?"))
            {
                domainName = domainName.Split("?")[0];
            }

            if (string.IsNullOrEmpty(model.DomainName))
            {
                return requestVerification;
            }

            var certificate = await _certificateRepository.Get(domainName);

            if (certificate == null)
            {
                return requestVerification;
            }

            requestVerification = new RequestVerification
            {
                CertificateId = certificate.Id,
                RequestNumber = certificate.RequestNumber,
                DomainName = certificate.DomainName,
                CreatedDate = certificate.CreatedDate,
                ExpiryDate = certificate.ExpireDate,
                ExpiredOn = certificate.ExpiredOn,
            };

            var noDays = Convert.ToInt32(certificate.ExpiredOn);

            //requestVerification.RedirectUrl = model.RedirectUrl + "/Domain_Status?dn=" + Utility.KYCUtility.encrypt(requestVerification.DomainName, paymentConfiguration.AccessCode);
            requestVerification.RedirectUrl = model.RedirectUrl + "/Domain_Status?dn=" + domainName;
            var documentUrl = _configuration.GetValue<string>("DocumentUrl");
            if (noDays > 0)
            {
                requestVerification.Url = documentUrl + "/valid.png";
            }
            else
            {
                requestVerification.Url = documentUrl + "/invalid.png";
            }

            var htmlBody = "<div>  <a href='{{redirectUrl}}' target='_blank' style='cursor: pointer;'>    <img style='width: 200px;cursor: pointer;' src='{{url}}'>  </a></div>";
            htmlBody = htmlBody.Replace("{{redirectUrl}}", requestVerification.RedirectUrl);
            htmlBody = htmlBody.Replace("{{url}}", requestVerification.Url);
            requestVerification.HtmlBody = htmlBody;

            return requestVerification;
        }
    }
}
