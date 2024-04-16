using Model;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Repository
{
    public class DashboardRepository : BaseRepository<Dashboard>
    {
        
        public DashboardRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<Dashboard> GetDashboardData()
        {
            Dashboard response = null;
            var storedProcedureName = "GetDashboardData";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        response = Load<Dashboard>(reader);
                    }
                }
            }
            return response;
        }
    }
}
