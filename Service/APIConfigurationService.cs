using Model;

using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class APIConfigurationService : BaseService<APIConfiguration>
    {
        public APIConfigurationService(IRepository<APIConfiguration> repository)
        {
            Repository = repository;
        }
        public override async Task<List<APIConfiguration>> Get()
        {
            var response = await Repository.Get();
            return response;
        }
        public override async Task<APIConfiguration> Put(APIConfiguration aPIConfiguration)
        {
            if (aPIConfiguration.IsActive)
            {
                var models = await Repository.Get();
                var model = models.FirstOrDefault(item => item.IsActive);
                if (model != null && model.Id != aPIConfiguration.Id)
                {
                    model.IsActive = false;
                    await Repository.Put(model);
                }
            }
            var response = await Repository.Put(aPIConfiguration);
            return response;
        }
        public override async Task<bool> Delete(APIConfiguration aPIConfiguration)
        {
            var response = await Repository.Delete(aPIConfiguration);
            return response;
        }
    }
}
