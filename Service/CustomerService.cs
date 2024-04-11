using Model;
using Repository;

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
                var customer = new CustomerDetail();
                customer.ErrorMessage = "No User Found";
                return customer;
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

        public override async Task<CustomerResponse> UpdateKYCCustomerDetails(CustomerUpdate customerUpdate)
        {
            var response = await Repository.UpdateKYCCustomerDetails(customerUpdate);

            if (response == null)
            {
                response = new CustomerResponse();
                response.Message = "KYC Verification is Failed";
            }
            return response;
        }

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
