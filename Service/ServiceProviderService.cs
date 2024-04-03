using Model;

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
        public ServiceProviderService(IRepository<SProvider> repository)
        {
            Repository = repository;
        }
        public override async Task<SProvider> Get(string code)
        {
            var response = await Repository.Get(code);
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
            return reposnse;
        }
    }
}
