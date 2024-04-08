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
    public class CustomerService : BaseService<CustomerDetail>
    {
        public CustomerService(IRepository<CustomerDetail> repository)
        {
            Repository = repository;
        }
        public override async Task<CustomerDetail> Get(string Id)
        {
            var response = await Repository.Get(Id);
            if (response == null)
            {
                var loginUser = new CustomerDetail();
                loginUser.ErrorMessage = "No User Found";
                return loginUser;
            }
            return response;
        }

        public override async Task<List<CustomerList>> GetAllCustomer()
        {
            var response = await Repository.GetAllCustomer();
            if (response == null)
            {
                var customerList = new List<CustomerList>();
                customerList.Add(new CustomerList
                {
                    ErrorMessage = "No User Found"
                    
                });
                
                return customerList;
            }
            return response;
        }

        //public override async Task<UserDetailResponse> AddUser(UserDetail userDetail)
        //{
        //    var response = await Repository.AddUser(userDetail);
            
        //    if (response==null)
        //    {
        //        response = new UserDetailResponse();
        //        response.Message = "User Creation Failed";
        //    }
        //    return response;
        //}

        //public override async Task<UserDetailResponse> UpdateUser(UserDetail userDetail)
        //{
        //    var response = await Repository.UpdateUser(userDetail);
        //    if (response==null)
        //    {
        //        response = new UserDetailResponse();
        //        response.Message = "User Updation Failed";
        //        response.UserId = userDetail.UserId.ToString();
        //    }
        //    return response;
        //}
    }
}
