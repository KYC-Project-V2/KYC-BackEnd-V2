using Model;

using Repository;

using System.Security.Cryptography;
using System.Text;

using Utility;

namespace Service
{
    public class OTPVerificationService : BaseService<OTPVerification>
    {
        private readonly IService<Domain> _domainservice;
        private readonly IService<Certificate> _certificateservice;
        private readonly IService<TemplateConfiguration> _templateConfigurationservice;
        private readonly IService<Email> _emailService;

        public OTPVerificationService(IRepository<OTPVerification> repository,
            IService<Domain> domainservice, IService<Certificate> certificateservice,
            IService<EmailConfiguration> emailConfigurationservice,
            IService<TemplateConfiguration> templateConfigurationservice,
            IService<Email> emailService)
        {
            Repository = repository;
            _domainservice = domainservice;
            _certificateservice = certificateservice;
            _templateConfigurationservice = templateConfigurationservice;
            _emailService = emailService;
        }

        public override async Task<OTPVerification> Put(OTPVerification model)
        {
            var otpResponse = await KYCUtility.GenerateOTP(model);
            model.OtpNumber = otpResponse.OtpNumber;
            var templateconfigs = await _templateConfigurationservice.Get(2);
            var templateConfig = templateconfigs.FirstOrDefault();

            if (model.Type == "1")
            {
                var htmlBody = templateConfig != null && !string.IsNullOrEmpty(templateConfig.Body) ? templateConfig.Body : "";
                htmlBody = htmlBody.Replace("{{name}}", model.Name);
                htmlBody = htmlBody.Replace("{{otpNumber}}", model.OtpNumber);
                htmlBody = htmlBody.Replace("{{requestnumber}}", model.RequestNumber);

                var email = new Email
                {
                    FromAddess = templateConfig.Sender,
                    ToAddress = model.Email,
                    Subject = templateConfig.Subject,
                    OtpNumber = model.OtpNumber,
                    Body = htmlBody
                };
                var emailResponse = await _emailService.Post(email);
            }
            else if (model.Type == "2")
            {
                var smsBody = "Your OTP for KYC Verification is " + model.OtpNumber;
                var smsDetails = new SMSDetails
                {
                    Phone = model.Phone,
                    OtpNumber = model.OtpNumber,
                    Body = smsBody
                };
                var smsResponse = await KYCUtility.SendSMS(smsDetails);
            }
            var response = await Repository.Put(model);

            return response;
        }

        public override async Task<OTPVerification> Post(OTPVerification model)
        {
            var response = await Repository.Get(model.RequestNumber);
            if (response != null)
            {
                var isverified = response.OtpNumber == model.OtpNumber;
                if (response != null && isverified)
                {
                    var domains = await _domainservice.GetAll(model.RequestNumber);
                    //if (domains != null && domains.Any())
                    //{
                    //    Certificate certificate;
                    //    var expiryDate = DateTime.Now.AddYears(1);
                    //    //TODO: Remove hardcode value 3
                    //    var templateConfigurations = await _templateConfigurationservice.Get(3);
                    //    var expiryTemplteConfig = templateConfigurations.FirstOrDefault();

                    //    //var days = expiryTemplteConfig != null && expiryTemplteConfig.Days > 0 ?
                    //    //    expiryTemplteConfig.Days - (expiryTemplteConfig.Days + expiryTemplteConfig.Days) : -30;

                    //    //var nextEmailTiggeringDate = expiryDate.AddDays(Convert.ToInt32(days));
                    //    foreach (var domain in domains)
                    //    {
                    //        certificate = new Certificate();
                    //        certificate.DomainId = domain.Id;
                    //        certificate.DomainName = domain.Name;
                    //        certificate.RequestNumber = domain.RequestNumber;
                    //        certificate.CreatedDate = DateTime.Now;
                    //        certificate.Certificates = ComputeStringToSha256Hash(domain.Name);
                    //        certificate.ExpireDate = expiryDate;
                    //        certificate.CreatedBy = "";
                    //        //certificate.NextEmailTiggeringDate = nextEmailTiggeringDate;
                    //        await _certificateservice.Post(certificate);
                    //    }
                    //}
                    return model;
                }
            }
            return null;
        }

        public override async Task<bool> Delete(OTPVerification model)
        {
            var response = await Repository.Delete(model);
            return response;
        }

        public override async Task<OTPVerification> Get(OTPVerification model)
        {
            var response = await Repository.Get(model.RequestNumber);
            response.IsVerified = response.OtpNumber == model.OtpNumber;
            return response;
        }

        static string ComputeStringToSha256Hash(string plainText)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }

    }
}
