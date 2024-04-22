using Microsoft.IdentityModel.Tokens;
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
            sprovider.RequestToken = KYCUtility.encrypt(sprovider.RequestToken, sprovider.SaltKey);
            var response = await Repository.Get(sprovider);
            return response;
        }
        public override async Task<SProvider> Post(SProvider sprovider)
        {
            Random generator = new Random();
            int random = generator.Next(1, 1000000);
            string requestnumber = string.Empty;
            if (string.IsNullOrEmpty(sprovider.RequestNumber))
            {
                sprovider.RequestNumber = "KYC-" + DateTime.Now.ToString("MMddyyyy") + "-" + random.ToString().PadLeft(3, '0');
            }
            else
            {
                requestnumber=sprovider.RequestNumber;
            }
            if (string.IsNullOrEmpty(sprovider.RequestToken))
                sprovider.RequestToken = KYCUtility.encrypt(KYCUtility.GenerateRandomString(10), sprovider.SaltKey);
            else
                sprovider.RequestToken = sprovider.RequestToken;
            sprovider.PAN = KYCUtility.encrypt(sprovider.PAN, sprovider.SaltKey);
            sprovider.GST = KYCUtility.encrypt(sprovider.GST, sprovider.SaltKey);
            sprovider.CreatedDate = DateTime.Now;
            if(string.IsNullOrEmpty(requestnumber))
            sprovider.Tokencode = sprovider.Tokencode + KYCUtility.encrypt("RequestNumber=" + sprovider.RequestNumber + "&TokenNumber=" + KYCUtility.decrypt(sprovider.RequestToken, sprovider.SaltKey), sprovider.SaltKey);
            var reposnse = await Repository.Post(sprovider);
            if(reposnse != null && reposnse.ProviderId!=null && reposnse.ProviderId != 0)
            {
                SendEmail(reposnse, requestnumber);
            }
            return reposnse;
        }
        private async void SendEmail(SProvider sprovider, string requestnumber)
        {
            var templateconfigResponse = await _templateConfigurationservice.Get(7);
            var templateconfig = templateconfigResponse.FirstOrDefault();
            var emailBody = templateconfig.Body;
            emailBody = emailBody.Replace("{{name}}", sprovider.ProviderName);
            emailBody = emailBody.Replace("{{requestnumber}}", sprovider.RequestNumber);
            emailBody = emailBody.Replace("{{transactiontype}}", !string.IsNullOrEmpty(requestnumber) ? "modified" : "received");
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

        public override async Task<List<ServiceProviderList>> GetAllServiceProvider()
        {
            var response = await Repository.GetAllServiceProvider();
            if (response == null)
            {
                response = new List<ServiceProviderList>();
                response.Add(new ServiceProviderList
                {
                    ErrorMessage = "No Data Found."
                });
            }
            return response;
        }

        public override async Task<ServiceProvider> GetServiceProvider(ServiceProviderRequest serviceProviderRequest)
        {
            var response = await Repository.GetServiceProvider(serviceProviderRequest);
            if (response == null)
            {
                var ServiceProvider = new ServiceProvider();
                ServiceProvider.ErrorMessage = "No Data Found";
                return ServiceProvider;
            }
            return response;
        }

        public override async Task<ServiceProviderResponse> UpdateServiceProvider(UpdateServiceProvider updateServiceProvider)
        {
            var response = await Repository.UpdateServiceProvider(updateServiceProvider);
            if (response == null)
            {
                var ServiceProvider = new ServiceProviderResponse();
                ServiceProvider.Message = "Verification Failed";
                return ServiceProvider;
            }
            else if (!string.IsNullOrEmpty(response?.Message))
            {
                var templateconfig = await _templateConfigurationservice.Get(3);
                var htmlBody = templateconfig.FirstOrDefault().Body;
                htmlBody = htmlBody.Replace("{{name}}", updateServiceProvider.ProviderName);
                htmlBody = htmlBody.Replace("{{requestnumber}}", updateServiceProvider.RequestNumber);
                if (updateServiceProvider.IsGSTVerificationStatus && updateServiceProvider.IsPANVerificationStatus)
                {
                    var template = @"<?xml version='1.0'?> <html><body><div style='margin-top: 10px;border-radius: 5px; font-size: large; align-items: center;display: flex; flex-direction: column; box-sizing: border-box;padding: 0.5rem 0.5rem; background-color: #F47216; color: #FFFFFF;'>KYC Verification</div>  
                                    <div style='margin-top: 25px;font-size: large; margin-left: 30px; margin-right: 30px;'> <div>Hello {{name}}</div>  
                                    <div style='margin-top:20px'>Your KYC Verification Request is completed successfully.<br/>So, Please read the comments below for more information.</div>  
                                    <div style='margin-top:10px;margin-bottom:25px'>KYC Request Number : <b>{{requestnumber}}</b> </div>  <div style='margin-top:100px; '> 
                                    <div style='margin-top:10px;margin-bottom:25px'> Comments :<b>{{comment}}</b> </div> <div style='margin-top:30px;'>Thanks, </div> <div>Astitva</div> </div> </body> </html>";
                    template = template.Replace("{{name}}", updateServiceProvider.ProviderName);
                    template = template.Replace("{{requestnumber}}", updateServiceProvider.RequestNumber);
                    template = template.Replace("{{comment}}", updateServiceProvider.Comments);
                    templateconfig[0].Body = template;
                    htmlBody = templateconfig[0].Body;

                }
                else if (updateServiceProvider.IsGSTVerificationStatus && !updateServiceProvider.IsPANVerificationStatus)
                {
                    var template = @"<?xml version='1.0'?> <html><body><div style='margin-top: 10px;border-radius: 5px; font-size: large; align-items: center;display: flex; flex-direction: column; box-sizing: border-box;padding: 0.5rem 0.5rem; background-color: #F47216; color: #FFFFFF;'>KYC Verification</div>  
                                    <div style='margin-top: 25px;font-size: large; margin-left: 30px; margin-right: 30px;'> <div>Hello {{name}}</div>  
                                    <div style='margin-top:20px'>This is to inform you that your GST is valid but your Pan card is not valid.<br/>So, Please read the comments below for more information.</div>  
                                    <div style='margin-top:10px;margin-bottom:25px'>KYC Request Number : <b>{{requestnumber}}</b> </div>  <div style='margin-top:100px; '> 
                                    <div style='margin-top:10px;margin-bottom:25px'> Comments :<b>{{comment}}</b> </div> <div style='margin-top:30px;'>Thanks, </div> <div>Astitva</div> </div> </body> </html>";
                    template = template.Replace("{{name}}", updateServiceProvider.ProviderName);
                    template = template.Replace("{{requestnumber}}", updateServiceProvider.RequestNumber);
                    template = template.Replace("{{comment}}", updateServiceProvider.Comments);
                    templateconfig[0].Body = template;
                    htmlBody = templateconfig[0].Body;
                }
                else if (!updateServiceProvider.IsGSTVerificationStatus && updateServiceProvider.IsPANVerificationStatus)
                {
                    var template = @"<?xml version='1.0'?> <html><body><div style='margin-top: 10px;border-radius: 5px; font-size: large; align-items: center;display: flex; flex-direction: column; box-sizing: border-box;padding: 0.5rem 0.5rem; background-color: #F47216; color: #FFFFFF;'>KYC Verification</div>  
                                    <div style='margin-top: 25px;font-size: large; margin-left: 30px; margin-right: 30px;'> <div>Hello {{name}}</div>  
                                    <div style='margin-top:20px'>This is to inform you that your Pan is valid but your GST card is not valid.<br/>So, Please read the comments below for more information.</div>  
                                    <div style='margin-top:10px;margin-bottom:25px'>KYC Request Number : <b>{{requestnumber}}</b> </div>  <div style='margin-top:100px; '> 
                                    <div style='margin-top:10px;margin-bottom:25px'> Comments :<b>{{comment}}</b> </div> <div style='margin-top:30px;'>Thanks, </div> <div>Astitva</div> </div> </body> </html>";
                    template = template.Replace("{{name}}", updateServiceProvider.ProviderName);
                    template = template.Replace("{{requestnumber}}", updateServiceProvider.RequestNumber);
                    template = template.Replace("{{comment}}", updateServiceProvider.Comments);
                    templateconfig[0].Body = template;
                    htmlBody = templateconfig[0].Body;
                }
                else if (!updateServiceProvider.IsGSTVerificationStatus && !updateServiceProvider.IsPANVerificationStatus)
                {
                    var template = @"<?xml version='1.0'?> <html><body><div style='margin-top: 10px;border-radius: 5px; font-size: large; align-items: center;display: flex; flex-direction: column; box-sizing: border-box;padding: 0.5rem 0.5rem; background-color: #F47216; color: #FFFFFF;'>KYC Verification</div>  
                                    <div style='margin-top: 25px;font-size: large; margin-left: 30px; margin-right: 30px;'> <div>Hello {{name}}</div>  
                                    <div style='margin-top:20px'>This is to inform you that your Pan and GST card is not valid.<br/>So, Please read the comments below for more information.</div>  
                                    <div style='margin-top:10px;margin-bottom:25px'>KYC Request Number : <b>{{requestnumber}}</b> </div>  <div style='margin-top:100px; '> 
                                    <div style='margin-top:10px;margin-bottom:25px'> Comments :<b>{{comment}}</b> </div> <div style='margin-top:30px;'>Thanks, </div> <div>Astitva</div> </div> </body> </html>";
                    template = template.Replace("{{name}}", updateServiceProvider.ProviderName);
                    template = template.Replace("{{requestnumber}}", updateServiceProvider.RequestNumber);
                    template = template.Replace("{{comment}}", updateServiceProvider.Comments);
                    templateconfig[0].Body = template;
                    htmlBody = templateconfig[0].Body;
                }
                var email = new Email
                {
                    FromAddess = templateconfig.FirstOrDefault().Sender,
                    ToAddress = updateServiceProvider.Email,
                    Subject = templateconfig.FirstOrDefault().Subject,
                    Body = htmlBody,
                };
                var emailResponse = await _emailService.Post(email);
            }
            return response;
        }
    }
}
