using Model;
using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CertificateService : BaseService<Certificate>
    {
        public CertificateService(IRepository<Certificate> repository)
        {
            Repository = repository;
        }
        public override async Task<List<Certificate>> GetAll(string code)
        {
            var response = await Repository.GetAll(code);
            return response;
        }
        public override async Task<Certificate> Post(Certificate Certificate)
        {
            var response = await Repository.Post(Certificate);
            return response;
        }

        public override async Task<Certificate> Get(string name)
        {
            var response = await Repository.Get(name);
            return response;
        }
    }
}
