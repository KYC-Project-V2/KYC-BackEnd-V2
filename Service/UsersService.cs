using iTextSharp.text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UsersService : BaseService<UserDetail>
    {
        public UsersService(IRepository<UserDetail> repository)
        {
            Repository = repository;
        }
        public override async Task<UserDetail> Get(string Id)
        {
            var response = await Repository.Get(Id);
            if (response == null)
            {
                var loginUser = new UserDetail();
                loginUser.ErrorMessage = "No User Found";
                return loginUser;
            }
            return response;
        }

        public override async Task<List<UserDetail>> Get()
        {
            var response = await Repository.Get();
            if (response == null)
            {
                var loginUser = new List<UserDetail>();
                loginUser.Add(new UserDetail
                {
                    ErrorMessage = "No User Found"
                    
                });
                
                return loginUser;
            }
            return response;
        }

        public override async Task<string> AddUser(UserDetail userDetail)
        {
            var response = await Repository.AddUser(userDetail);
            
            if (string.IsNullOrEmpty(response))
            {
                response = "User Creation Failed";
            }
            return response;
        }

        public override async Task<string> UpdateUser(UserDetail userDetail)
        {
            var response = await Repository.UpdateUser(userDetail);
            if (string.IsNullOrEmpty(response))
            {
                response = "User Updation Failed";
            }
            return response;
        }
    }
}
