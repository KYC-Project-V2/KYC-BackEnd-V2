using Model;

using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Utility;

namespace Service
{
    public class PaymentService : BaseService<Payment>
    {
      
        public PaymentService(IRepository<Payment> repository)
        {
            Repository = repository;
        }
        public override async Task<Payment> Get(string code)
        {
            var response = await Repository.Get(code);
            return response;
        }
        public override async Task<Payment> Post(Payment payment)
        {
            var response = await Repository.Post(payment);
            return response;
        }
    }
}
