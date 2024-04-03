using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CountryService : BaseService<Country>
    {
        public CountryService(IRepository<Country> repository)
        {
            Repository = repository;
        }

        public override async Task<List<Country>> Get()
        {
            var response = await Repository.Get();
            return response;
        }
    }
}
