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
    public class APIStatusService : BaseService<APIStatus>
    {
        public APIStatusService(IRepository<APIStatus> repository)
        {
            Repository = repository;
        }
        public override async Task<APIStatus> Get(APIStatus apiStatus)
        {
            var response = await Repository.Get(apiStatus);
            return response;
        }
        public override async Task<APIStatus> Post(APIStatus apiStatus)
        {
            var response = await Repository.Post(apiStatus);
            return response;
        }
        public override async Task<APIStatus> Get(string tokenNumber)
        {
            var response = await Repository.Get(tokenNumber);
            return response;
        }
    }
}
