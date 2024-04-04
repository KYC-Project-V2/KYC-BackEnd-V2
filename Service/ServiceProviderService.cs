using Model;
using Org.BouncyCastle.Asn1.X509;
using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Service
{
    public class ServiceProviderService : BaseService<SProvider>
    {
        private readonly IService<TemplateConfiguration> _templateConfigurationservice;
        private readonly IService<Email> _emailService;
        public ServiceProviderService(IRepository<SProvider> repository, IService<TemplateConfiguration> templateConfigurationservice, IService<Email> emailService)
        {
            Repository = repository;
            _templateConfigurationservice = templateConfigurationservice;
            _emailService = emailService;
        }
        public override async Task<SProvider> Get(SProvider sprovider)
        {
            var response = await Repository.Get(sprovider);
            return response;
        }
        public override async Task<SProvider> Post(SProvider sprovider)
        {
            Random generator = new Random();
            int random = generator.Next(1, 1000000);
            sprovider.RequestNumber = "KYC-" + DateTime.Now.ToString("MMddyyyy") + "-" + random.ToString().PadLeft(3, '0');
            sprovider.RequestToken = KYCUtility.GenerateRandomString(10);
            //sprovider.RequestNumber = KYCUtility.encrypt(sprovider.RequestNumber, sprovider.SaltKey);
            sprovider.RequestToken = KYCUtility.encrypt(sprovider.RequestToken, sprovider.SaltKey);
            sprovider.PAN = KYCUtility.encrypt(sprovider.PAN, sprovider.SaltKey);
            sprovider.GST = KYCUtility.encrypt(sprovider.GST, sprovider.SaltKey);
            sprovider.CreatedDate = DateTime.Now;
            var reposnse = await Repository.Post(sprovider);
            if(reposnse != null && reposnse.ProviderId!=null && reposnse.ProviderId != 0)
            {
                SendEmail(reposnse);
            }
            return reposnse;
        }
        private async void SendEmail(SProvider sprovider)
        {
            var templateconfigResponse = await _templateConfigurationservice.Get(7);
            var templateconfig = templateconfigResponse.FirstOrDefault();
            var emailBody = templateconfig.Body;
            emailBody = emailBody.Replace("{{name}}", sprovider.ProviderName);
            emailBody = emailBody.Replace("{{requestnumber}}", sprovider.RequestNumber);
            var email = new Email
            {
                ToAddress = sprovider.Email,
                Subject = templateconfig.Subject,
                FromAddess = templateconfig.Sender,
                AttachmentPath = null,
                Body = emailBody
            };
            await _emailService.Post(email);
        }
    }
}
