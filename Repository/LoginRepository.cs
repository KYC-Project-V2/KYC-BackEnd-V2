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
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@UserName", model.UserName));
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
