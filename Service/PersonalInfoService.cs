using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PersonalInfoService : BaseService<PersonalInfo>
    {
        public PersonalInfoService(IRepository<PersonalInfo> repository)
        {
            Repository = repository;
        }

        public override async Task<List<PersonalInfo>> Get(int Id)
        {
            var response = await Repository.Get(Id);
            return response;
        }
        public override async Task<PersonalInfo> Get(string code)
        {
            var response = await Repository.Get(code);
            return response;
        }

        public override async Task<PersonalInfo> Put(PersonalInfo personalInfo)
        {
            var response = await Repository.Put(personalInfo);
            return response;
        }
    }
}
