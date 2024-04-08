using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BaseService<T>: IService<T>
    {
        public IRepository<T> Repository { get; set; }

        public virtual async Task<UserDetailResponse> AddUser(T model)
        {
            return await Repository.AddUser(model);
        }
        public virtual async Task<UserDetailResponse> UpdateUser(T model)
        {
            return await Repository.UpdateUser(model);
        }
        public virtual async Task<List<CustomerList>> GetAllCustomer()
        {
            return await Repository.GetAllCustomer();
        }
        public virtual Task<bool> Delete(T model)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<List<T>> Get(int Id)
        {
            return await Repository.Get(Id);
        }

        public virtual async Task<List<T>> Get()
        {
            return await Repository.Get();
        }

        public virtual async Task<T> Get(string value)
        {
            return await Repository.Get(value);
            //throw new NotImplementedException();
        }
        public virtual async Task<T> Get(bool value)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> Get(T model)
        {
            return await Repository.Get(model);
        }

        public virtual async Task<List<T>> GetAll(string value)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> Post(T model)
        {
            return await Repository.Post(model);
        }

        public virtual Task<List<T>> Post(List<T> model)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Put(T model)
        {
            throw new NotImplementedException();
        }

        
    }
}
