using Model;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class CustomerRepository : BaseRepository<CustomerDetail>
    {
        public CustomerRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<CustomerDetail> Get(string Id)
        {
            CustomerDetail response = null;
            var storedProcedureName = "GetCustomersData";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNo", Id));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        response = Load<CustomerDetail>(reader);
                    }
                }

            }

            return response;
        }


        public override async Task<List<CustomerList>> GetAllCustomer()
        {
            List<CustomerList> models = new List<CustomerList>();
            var storedProcedureName = "GetCustomersData";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var model = Load<CustomerList>(reader);
                        models.Add(model);
                    }
                }

            }

            return models;
        }

        //public override async Task<UserDetailResponse> AddUser(UserDetail userDetail)
        //{
        //    string cmdText = "AddUpdateUserDetails";            
        //    UserDetailResponse userDetailResponse =null;
        //    try
        //    {
        //        byte[] encData_byte = new byte[userDetail.Password.Length];
        //        encData_byte = System.Text.Encoding.UTF8.GetBytes(userDetail.Password);
        //        string encodedData = Convert.ToBase64String(encData_byte);
        //        userDetail.Password = encodedData;
        //        //return encodedData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error in base64Encode" + ex.Message);
        //    }
        //    using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
        //    {
        //        connection.Open();

        //        using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
        //        {
        //            Random generator = new Random();
        //            var randomNumber = generator.Next(0, 1000000).ToString("D6");

        //            command.Parameters.Add(new SqlParameter("@RoleId", userDetail.RoleId));
        //            command.Parameters.Add(new SqlParameter("@UserId", randomNumber));
        //            command.Parameters.Add(new SqlParameter("@UserName", userDetail.UserName));
        //            command.Parameters.Add(new SqlParameter("@Password", userDetail.Password));
        //            command.Parameters.Add(new SqlParameter("@Status", userDetail.Status));
        //            command.Parameters.Add(new SqlParameter("@AddressLine1", userDetail.AddressLine1));
        //            command.Parameters.Add(new SqlParameter("@AddressLine2", userDetail.AddressLine2));
        //            command.Parameters.Add(new SqlParameter("@City", userDetail.City));
        //            command.Parameters.Add(new SqlParameter("@StateId", userDetail.StateId));
        //            command.Parameters.Add(new SqlParameter("@PostalCode", userDetail.PostalCode));
        //            command.Parameters.Add(new SqlParameter("@CountryId", userDetail.CountryId));
        //            command.Parameters.Add(new SqlParameter("@Reference", userDetail.Reference));
        //            command.Parameters.Add(new SqlParameter("@Email", userDetail.Email));
        //            command.Parameters.Add(new SqlParameter("@PhoneNumber", userDetail.PhoneNumber));
        //            command.Parameters.Add(new SqlParameter("@CreatedBy", userDetail.UserId));
        //            command.Parameters.Add(new SqlParameter("@iSAddUser", "Y"));
        //            try
        //            {
        //                var response = await command.ExecuteNonQueryAsync();
        //                if (response > 0)
        //                {
        //                    userDetailResponse = new UserDetailResponse();
        //                    userDetailResponse.UserId = randomNumber;
        //                    userDetailResponse.Message = "User Created Successfully.";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw;
        //            }
                   


        //        }
        //    }
        //    return userDetailResponse;
        //}

        //public override async Task<UserDetailResponse> UpdateUser(UserDetail userDetail)
        //{
        //    string cmdText = "AddUpdateUserDetails";
        //    UserDetailResponse userDetailResponse = null;

        //    try
        //    {
        //        byte[] encData_byte = new byte[userDetail.Password.Length];
        //        encData_byte = System.Text.Encoding.UTF8.GetBytes(userDetail.Password);
        //        string encodedData = Convert.ToBase64String(encData_byte);
        //        userDetail.Password = encodedData;
        //        //return encodedData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error in base64Encode" + ex.Message);
        //    }
        //    using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
        //    {
        //        connection.Open();

        //        using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
        //        {
        //            command.Parameters.Add(new SqlParameter("@Id", userDetail.Id));
        //            command.Parameters.Add(new SqlParameter("@RoleId", userDetail.RoleId));
        //            command.Parameters.Add(new SqlParameter("@UserId", userDetail.UserId));
        //            command.Parameters.Add(new SqlParameter("@UserName", userDetail.UserName));
        //            command.Parameters.Add(new SqlParameter("@Password", userDetail.Password));
        //            command.Parameters.Add(new SqlParameter("@Status", userDetail.Status));
        //            command.Parameters.Add(new SqlParameter("@AddressLine1", userDetail.AddressLine1));
        //            command.Parameters.Add(new SqlParameter("@AddressLine2", userDetail.AddressLine2));
        //            command.Parameters.Add(new SqlParameter("@City", userDetail.City));
        //            command.Parameters.Add(new SqlParameter("@StateId", userDetail.StateId));
        //            command.Parameters.Add(new SqlParameter("@PostalCode", userDetail.PostalCode));
        //            command.Parameters.Add(new SqlParameter("@CountryId", userDetail.CountryId));
        //            command.Parameters.Add(new SqlParameter("@Reference", userDetail.Reference));
        //            command.Parameters.Add(new SqlParameter("@Email", userDetail.Email));
        //            command.Parameters.Add(new SqlParameter("@PhoneNumber", userDetail.PhoneNumber));
        //            command.Parameters.Add(new SqlParameter("@UpdatedBy", userDetail.UserId));
        //            command.Parameters.Add(new SqlParameter("@iSAddUser", "N"));
        //            try
        //            {
        //                var response = await command.ExecuteNonQueryAsync();
        //                if (response > 0)
        //                {
        //                    userDetailResponse = new UserDetailResponse();
        //                    userDetailResponse.UserId = Convert.ToString(userDetail?.UserId);
        //                    userDetailResponse.Message = "User Updated Successfully.";
                            
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                throw ex;
        //            }

        //        }
        //    }
        //    return userDetailResponse;
        //}

          
    }
}
