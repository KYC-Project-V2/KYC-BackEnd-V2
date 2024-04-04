using iTextSharp.text;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public override async Task<UserDetail> Get(string UserId)
        {
            var response = await Repository.Get(UserId);
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

        public override async Task<UserDetail> Post(UserDetail userDetail)
        {
            var response = await Repository.Post(userDetail);
            if (response == null)
            {
                var loginUser = new UserDetail();
                loginUser.ErrorMessage = "User Creation Failed";
                return loginUser;
            }
            return response;
        }

        public override async Task<UserDetail> Put(UserDetail userDetail)
        {
            UserDetail response = null;
            try
            {
                response = await Repository.Put(userDetail);
                if (response == null)
                {
                    var loginUser = new UserDetail();
                    loginUser.ErrorMessage = "User Updation Failed";
                    return loginUser;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
            return response;
        }
    }
}
