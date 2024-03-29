using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JWTService : IJWTService
    {
        private readonly IJWTRepository Repository;
        public JWTService(IJWTRepository repository)
        {
            Repository = repository;
        }
        public async Task<Tokens> Authenticate(Authentication users)
        {
            var response = await Repository.Authenticate(users);
            return response;
        }
    }
}
