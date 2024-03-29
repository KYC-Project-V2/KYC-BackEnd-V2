using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Service
{
    public class EmailService : BaseService<Email>
    {
        private readonly IService<EmailConfiguration> _emailConfigurationservice;

        public EmailService(IService<EmailConfiguration> emailConfigurationservice)
        {
            _emailConfigurationservice = emailConfigurationservice;
        }
        public override async Task<Email> Post(Email email)
        {
            
            var emailconfig = await _emailConfigurationservice.Get();
            emailconfig = emailconfig.Where(conf => conf.Status).ToList();
            email.emailConfiguration = emailconfig.FirstOrDefault();

            var emailResponse = await KYCUtility.SendMail(email);

            return emailResponse;
        }
    }
}
