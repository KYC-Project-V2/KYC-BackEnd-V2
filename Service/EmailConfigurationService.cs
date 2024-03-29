using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service
{

    public class EmailConfigurationService : BaseService<EmailConfiguration>
    {
        public EmailConfigurationService(IRepository<EmailConfiguration> repository)
        {
            Repository = repository;
        }
        public override async Task<List<EmailConfiguration>> Get()
        {
            var response = await Repository.Get();
            return response;
        }
        public override async Task<EmailConfiguration> Put(EmailConfiguration emailConfiguration)
        {
            if (emailConfiguration.Status)
            {
                var models = await Repository.Get();
                var model = models.FirstOrDefault(item => item.Status);
                if (model != null && model.Id != emailConfiguration.Id)
                {
                    model.Status = false;
                    await Repository.Put(model);
                }  
            }
            var response = await Repository.Put(emailConfiguration);
            return response;
        }
    }
}
