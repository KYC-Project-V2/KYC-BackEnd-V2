using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BusinessInfoService: BaseService<BusinessInfo>
    {
        public BusinessInfoService(IRepository<BusinessInfo> repository)
        {
            Repository = repository;
        }
        public override async Task<List<BusinessInfo>> Get(int Id)
        {
            var response = await Repository.Get(Id);
            return response;
        }
        public override async Task<BusinessInfo> Get(string code)
        {
            var response = await Repository.Get(code);
            return response;
        }
        public override async Task<BusinessInfo> Put(BusinessInfo businessInfo)
        {
            var response = await Repository.Put(businessInfo);
            return response;
        }

    }
}
