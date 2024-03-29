using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class PaymentConfigurationService : BaseService<PaymentConfiguration>
    {
        public PaymentConfigurationService(IRepository<PaymentConfiguration> repository)
        {
            Repository = repository;
        }
        public override async Task<List<PaymentConfiguration>> Get()
        {
            var response = await Repository.Get();
            return response;
        }

        public override async Task<PaymentConfiguration> Put(PaymentConfiguration paymentConfiguration)
        {
            if (paymentConfiguration.Status)
            {
                var models = await Repository.Get();
                var model = models.FirstOrDefault(item => item.Status);
                if (model != null && model.Id != paymentConfiguration.Id)
                {
                    model.Status = false;
                    await Repository.Put(model);
                }
            }
            var response = await Repository.Put(paymentConfiguration);
            return response;
        }

        public override async Task<PaymentConfiguration> Get(bool status)
        {
            var response = await Repository.Get(status);
            return response;
        }

    }
}
