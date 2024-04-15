using Model;
using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RootCertificateService : BaseService<RootCertificate>
    {
        public RootCertificateService(IRepository<RootCertificate> repository)
        {
            Repository = repository;
        }
      
        public override async Task<RootCertificate> Post(RootCertificate Certificate)
        {
            var response = await Repository.Post(Certificate);
            return response;
        }

        public override async Task<RootCertificate> Get(string rootid)
        {
            var response = await Repository.Get(rootid);
            return response;
        }
    }
}
