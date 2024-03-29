using Model;

using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RequestOriginService : BaseService<RequestOrigin>
    {
        public RequestOriginService(IRepository<RequestOrigin> repository)
        {
            Repository = repository;
        }
        public override async Task<RequestOrigin> Get(string code)
        {
            var response = await Repository.Get(code);
            return response;
        }
        public override async Task<RequestOrigin> Post(RequestOrigin requestOrigin)
        {
            Random generator = new Random();
            int random = generator.Next(1, 1000000);
            requestOrigin.RequestId = Guid.NewGuid().ToString();
            requestOrigin.RequestNo = "KYC-" + DateTime.Now.ToString("MMddyyyy") + "-" + random.ToString().PadLeft(3, '0');
            requestOrigin.CreatedDate = DateTime.Now;
            var reposnse = await Repository.Post(requestOrigin);
            return reposnse;
        }
    }
}
