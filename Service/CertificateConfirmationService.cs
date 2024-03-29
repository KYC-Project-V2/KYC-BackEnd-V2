using Model;

using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Utility;

namespace Service
{
    public class CertificateConfirmationService : BaseService<CertificateConfirmation>
    {
        private readonly IRepository<TemplateConfiguration> _TemplateConfigurationService;

        public CertificateConfirmationService(IRepository<CertificateConfirmation> repository, IRepository<TemplateConfiguration> templateConfigurationService)
        {
            Repository = repository;
            _TemplateConfigurationService = templateConfigurationService;
        }
        public override async Task<List<CertificateConfirmation>> GetAll(string noofDays)
        {
            bool IsWeekDays = true;
            bool IsDaily = false;
            List<CertificateConfirmation> Certificate = new();
            List<CertificateConfirmation> response = await Repository.GetAll(noofDays);
            var templateConfigurations = await _TemplateConfigurationService.Get(1);

            if (response != null && response.Count > 0)
            {
                foreach (CertificateConfirmation confirmation in response)
                {
                    try
                    {
                        confirmation.Email = !string.IsNullOrEmpty(confirmation.Email) ? confirmation.Email : confirmation.ContactEmail;
                        confirmation.FirstName = !string.IsNullOrEmpty(confirmation.FirstName) ? confirmation.FirstName : confirmation.ContactLastName;
                        confirmation.LastName = !string.IsNullOrEmpty(confirmation.LastName) ? confirmation.LastName : confirmation.LastName;

                        TemplateConfiguration templateConfiguration = new TemplateConfiguration();

                        //DateTime dateTime = new DateTime();
                        //if (Convert.ToDateTime(confirmation.ExpireDate).Date < dateTime.Date)
                        //{
                        //    templateConfiguration = templateConfigurations.Where(t => t.Type == "Expire Template").FirstOrDefault(); // Need to write Active condition once it is inserted to DB.
                        //}
                        //else
                        //{
                        //    templateConfiguration = templateConfigurations.Where(t => t.Type == "Reminder Template").FirstOrDefault();
                        //}

                        templateConfiguration = templateConfigurations.Where(t => t.TemplateTypeId == 1).FirstOrDefault();

                        var email = new Email
                        {
                            ToAddress = confirmation.Email,
                            Subject = templateConfiguration.Subject,
                            Body = templateConfiguration.Body
                        };
                        var emailResponse = await KYCUtility.SendMail(email);
                        if (emailResponse != null)//----MailSent Successfully----//
                        {
                            confirmation.Daily = IsDaily;
                            confirmation.Weekly = IsWeekDays;

                            // In this method we have the Update the certificate 
                            // var updatedccresponse = await Post(confirmation);
                        }
                        confirmation.ErrorMsg = "Success";
                    }
                    catch (Exception ex)
                    {
                        confirmation.ErrorMsg = "Failure Error at: " + ex.Message;
                    }
                    Certificate.Add(confirmation);
                }
            }
            return response;
        }
        public override async Task<CertificateConfirmation> Post(CertificateConfirmation Certificate)
        {
            var response = await Repository.Post(Certificate);
            return response;
        }
    }
}
