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
    public class AuthenticationRepository : BaseRepository<Authentication>
    {
        public AuthenticationRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<Authentication> Get(Authentication model)
        {
            Authentication response = null;
            var storedProcedureName = "GetUser";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@Code", model.Code));

                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        response = Load<Authentication>(reader);
                    }
                }

            }

            return response;
        }
    }
}
