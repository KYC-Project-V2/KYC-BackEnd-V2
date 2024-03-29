using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService : BaseService<Authentication>
    {
        public AuthenticationService(IRepository<Authentication> repository)
        {
            Repository = repository;
        }
        public override async Task<Authentication> Get(Authentication model)
        {
            var response = await Repository.Get(model);
            if(response == null || response.SecretKey != model.SecretKey)
            {
                return null;
            }
            return response;
        }
    }
}
