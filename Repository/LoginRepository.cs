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
            UserDetail userDetail = null;
            var storedProcedureName = "GetUserDetails";
            string decodePwd = string.Empty;


            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetUserData", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@UserId", model.UserId));

                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        userDetail = Load<UserDetail>(reader);
                        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                        System.Text.Decoder utf8Decode = encoder.GetDecoder();
                        byte[] todecode_byte = Convert.FromBase64String(userDetail.Password.ToString());
                        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                        char[] decoded_char = new char[charCount];
                        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                        decodePwd = new String(decoded_char);

                    }
                    connection.Close();
                }
                if (decodePwd != model.Password)
                {
                    response = new LoginUser();
                    response.ErrorMessage = "Invalid UserId or Password";
                    return response;
                }

                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    try
                    {
                        byte[] encData_byte = new byte[model.Password.Length];
                        encData_byte = System.Text.Encoding.UTF8.GetBytes(model.Password);
                        string encodedData = Convert.ToBase64String(encData_byte);
                        model.Password = encodedData;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error in base64Encode" + ex.Message);
                    }
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
