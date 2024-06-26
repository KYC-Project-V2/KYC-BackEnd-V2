﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IService<T>
    {
        Task<List<T>> Get();
        Task<List<T>> Get(int Id);
        Task<T> Get(string value);
        Task<T> Get(bool value);
        Task<T> Get(T model);
        Task<List<T>> GetAll(string value);
        Task<T> Post(T model);
        Task<T> Put(T model);
        Task<bool> Delete(T model);
        Task<List<T>> Post(List<T> model);
        Task<UserDetailResponse> AddUser(T model);
        Task<UserDetailResponse> UpdateUser(T model);
        Task<CustomerListResponse> GetAllCustomer(int status, int page, int perPage);
        Task<CustomerResponse> UpdateKYCCustomerDetails(CustomerUpdate model, string certificates, string certificatesPath);
        Task<List<T>> GetCertificate(string requestNo, bool certificateType);
        Task<T> GetDashboardData();
        Task<CustomerDetail> GetCustomerData(CustomerRequest serviceProviderRequest);
        Task<List<ServiceProviderList>> GetAllServiceProvider();
        Task<ServiceProvider> GetServiceProvider(ServiceProviderRequest serviceProviderRequest);
        Task<ServiceProviderResponse> UpdateServiceProvider(UpdateServiceProvider updateServiceProvider);

    }
}
