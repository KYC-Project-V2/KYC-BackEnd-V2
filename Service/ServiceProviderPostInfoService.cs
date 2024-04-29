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
    public class ServiceProviderPostInfoService : BaseService<APIStatus>
    {
        public ServiceProviderPostInfoService(IRepository<APIStatus> repository)
        {
            Repository = repository;
        }
        public override async Task<APIStatus> Post(APIStatus sprovider)
        {
            var reposnse = await Repository.Post(sprovider);
            return reposnse;
        }
    }
}