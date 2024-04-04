using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace Repository
{
    public class UsersRepository : BaseRepository<UserDetail>
    {
        public UsersRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<UserDetail> Get(string userId)
        {
            UserDetail response = null;
            var storedProcedureName = "GetUserData";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        response = Load<UserDetail>(reader);
                    }
                }

            }

            return response;
        }


        public override async Task<List<UserDetail>> Get()
        {
            List<UserDetail> models = new List<UserDetail>();
            var storedProcedureName = "GetUserData";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<UserDetail>(reader);
                        models.Add(model);
                    }
                }

            }

            return models;
        }

        public override async Task<UserDetail> Post(UserDetail userDetail)
        {
            string cmdText = "AddUpdateUserDetails";
            var userAddDetail = new UserDetail();
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RoleId", userDetail.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserId", userDetail.UserId));
                    command.Parameters.Add(new SqlParameter("@UserName", userDetail.UserName));
                    command.Parameters.Add(new SqlParameter("@Password", userDetail.Password));
                    command.Parameters.Add(new SqlParameter("@Status", userDetail.Status));
                    command.Parameters.Add(new SqlParameter("@AddressLine1", userDetail.AddressLine1));
                    command.Parameters.Add(new SqlParameter("@AddressLine2", userDetail.AddressLine2));
                    command.Parameters.Add(new SqlParameter("@City", userDetail.City));
                    command.Parameters.Add(new SqlParameter("@StateId", userDetail.StateId));
                    command.Parameters.Add(new SqlParameter("@PostalCode", userDetail.PostalCode));
                    command.Parameters.Add(new SqlParameter("@CountryId", userDetail.Country));
                    command.Parameters.Add(new SqlParameter("@Reference", userDetail.Reference));
                    command.Parameters.Add(new SqlParameter("@Email", userDetail.Email));
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", userDetail.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@CreatedBy", userDetail.UserId));
                    command.Parameters.Add(new SqlParameter("@iSAddUser", "Y"));

                    using (var response = command.ExecuteReaderAsync())
                    {
                        while (await response.Result.ReadAsync())
                        {
                            userAddDetail = Load<UserDetail>((IDataReader)response);
                        }
                    }
                }
            }
            return userAddDetail;
        }



        public override async Task<UserDetail> Put(UserDetail userDetail)
        {
            string cmdText = "AddUpdateUserDetails";
            var userUpdateDetail = new UserDetail();

            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RoleId", userDetail.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserId", userDetail.UserId));
                    command.Parameters.Add(new SqlParameter("@UserName", userDetail.UserName));
                    command.Parameters.Add(new SqlParameter("@Password", userDetail.Password));
                    command.Parameters.Add(new SqlParameter("@Status", userDetail.Status));
                    command.Parameters.Add(new SqlParameter("@AddressLine1", userDetail.AddressLine1));
                    command.Parameters.Add(new SqlParameter("@AddressLine2", userDetail.AddressLine2));
                    command.Parameters.Add(new SqlParameter("@City", userDetail.City));
                    command.Parameters.Add(new SqlParameter("@StateId", userDetail.StateId));
                    command.Parameters.Add(new SqlParameter("@PostalCode", userDetail.PostalCode));
                    command.Parameters.Add(new SqlParameter("@CountryId", userDetail.Country));
                    command.Parameters.Add(new SqlParameter("@Reference", userDetail.Reference));
                    command.Parameters.Add(new SqlParameter("@Email", userDetail.Email));
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", userDetail.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@CreatedBy", userDetail.UserId));
                    command.Parameters.Add(new SqlParameter("@iSAddUser", "N"));
                    try
                    {
                        using (var response = command.ExecuteReaderAsync())
                        {
                            while (await response.Result.ReadAsync())
                            {
                                userUpdateDetail = Load<UserDetail>((IDataReader)response);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    
                }
            }
            return userUpdateDetail;
        }


    }
}
