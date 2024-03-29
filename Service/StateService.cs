using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class StateService : BaseService<State>
    {
        public StateService(IRepository<State> repository)
        {
            Repository = repository;
        }

        public override async Task<List<State>> Get()
        {
            var response = await Repository.Get();
            return response;
        }
    }
}
