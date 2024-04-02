using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LoginService : BaseService<LoginUser>
    {
        public LoginService(IRepository<LoginUser> repository)
        {
            Repository = repository;
        }
        public override async Task<LoginUser> Get(LoginUser model)
        {
            var response = await Repository.Get(model);
            if (response == null)
            {
               var loginUser = new LoginUser();
                loginUser.ErrorMessage = "Invalid User";
                return loginUser;
            }
            return response;
        }
    }
}
