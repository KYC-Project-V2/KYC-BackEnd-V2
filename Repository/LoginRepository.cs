using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class LoginRepository : BaseRepository<LoginUser>
    {
        public LoginRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<LoginUser> Get(LoginUser model)
        {
            LoginUser response = null;
            var storedProcedureName = "GetUserDetails";
            //try
            //{
            //    byte[] encData_byte = new byte[model.Password.Length];
            //    encData_byte = System.Text.Encoding.UTF8.GetBytes(model.Password);
            //    string encodedData = Convert.ToBase64String(encData_byte);
            //    model.Password = encodedData;
            //    //return encodedData;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error in base64Encode" + ex.Message);
            //}
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@UserId", model.UserId));
                    command.Parameters.Add(new SqlParameter("@Password", model.Password));

                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        response = Load<LoginUser>(reader);
                    }
                }

            }

            return response;
        }
    }
}
