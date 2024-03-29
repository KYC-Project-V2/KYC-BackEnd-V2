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
    public class CCAvenueResponseService : BaseService<CCAvenueResponse>
    {
        public CCAvenueResponseService(IRepository<CCAvenueResponse> repository)
        {
            Repository = repository;
        }
        public override async Task<CCAvenueResponse> Post(CCAvenueResponse ccAvenueResponse)
        {
            var response = await Repository.Post(ccAvenueResponse);
            return response;
        }
    }
}
