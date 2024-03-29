using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TemplateTypeService : BaseService<TemplateType>
    {
        public TemplateTypeService(IRepository<TemplateType> repository)
        {
            Repository = repository;
        }

        public override async Task<List<TemplateType>> Get()
        {
            var response = await Repository.Get();
            return response;
        }
    }
}
