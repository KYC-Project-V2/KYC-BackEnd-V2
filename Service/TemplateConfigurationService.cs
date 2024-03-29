using Model;

using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TemplateConfigurationService : BaseService<TemplateConfiguration>
    {
        public TemplateConfigurationService(IRepository<TemplateConfiguration> repository)
        {
            Repository = repository;
        }
        public override async Task<List<TemplateConfiguration>> Get(int Id)
        {
            var response = await Repository.Get(Id);
            return response;
        }
        public override async Task<TemplateConfiguration> Put(TemplateConfiguration templateConfiguration)
        {
            var response = await Repository.Put(templateConfiguration);
            return response;
        }
        public override async Task<bool> Delete(TemplateConfiguration templateConfiguration)
        {
            var response = await Repository.Delete(templateConfiguration);
            return response;
        }
    }
}
