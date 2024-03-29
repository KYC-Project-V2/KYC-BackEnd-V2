using Model;

using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DomainService : BaseService<Domain>
    {
        public DomainService(IRepository<Domain> repository)
        {
            Repository = repository;
        }
        public override async Task<List<Domain>> GetAll(string code)
        {
            var response = await Repository.GetAll(code);
            return response;
        }
        public override async Task<Domain> Post(Domain domain)
        {
            var response = await Repository.Post(domain);
            return response;
        }

        public override async Task<bool> Delete(Domain domain)
        {
            var response = await Repository.Delete(domain);
            return response;
        }

        public override async Task<List<Domain>> Post(List<Domain> models)
        {
            var domains = new List<Domain>();
            foreach (var domain in models)
            {
                var response = await Repository.Post(domain);
                domains.Add(response);
            }

            return domains;
            
        }
    }
}
