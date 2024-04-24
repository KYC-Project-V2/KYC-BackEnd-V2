using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class APIStatusService : BaseService<APIStatus>
    {
        public APIStatusService(IRepository<APIStatus> repository)
        {
            Repository = repository;
        }
       
    }
}
