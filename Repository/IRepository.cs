using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T>
    {
        Task<List<T>> Get();
        Task<List<T>> Get(int Id);
        Task<List<T>> GetAll(string code);
        Task<T> Get(string code);
        Task<T> Get(bool status);
        Task<T> Get(T model);
        T Load<T>(IDataReader dataReader) where T : new();
        Task<T> Post(T model);
        Task<T> Put(T model);
        Task<bool> Delete(T model);
        Task<UserDetailResponse> AddUser(T model);
        Task<UserDetailResponse> UpdateUser(T model);
        Task<List<CustomerList>> GetAllCustomer(int status);
        Task<CustomerResponse> UpdateKYCCustomerDetails(CustomerUpdate model);
        Task<List<T>> GetCertificate(string requestNo, bool certificateType);
    }
}
