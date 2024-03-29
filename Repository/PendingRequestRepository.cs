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
    public class PendingRequestRepository : BaseRepository<PendingRequest>
    {
        public PendingRequestRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<PendingRequest> Get(string code)
        {
            List<PendingRequest> models = new List<PendingRequest>();
            var storedProcedureName = "GetRequestOriginByCode";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", code));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var entity = Load<PendingRequest>(reader);
                        models.Add(entity);
                    }
                }
            }
            var model = models != null && models.Any() ? models.FirstOrDefault(item => item.RequestNo == code) : null;
            return model;
        }
        public override async Task<PendingRequest> Post(PendingRequest pendingRequest)
        {
            string cmdText = "";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (PendingRequest)Convert.ChangeType(pendingRequest, typeof(PendingRequest));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(RequestOrigin).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        var modelPropertyNameValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                        if (modelPropertyNameValue != null)
                        {
                            parameters.Add(property.Name, modelPropertyNameValue);
                        }
                    }
                    if (parameters.Any())
                    {
                        foreach (var f in parameters)
                        {
                            command.Parameters.Add(new SqlParameter($"{f.Key}", f.Value));
                        }
                    }
                    using (var response = command.ExecuteReaderAsync())
                    {
                        while (await response.Result.ReadAsync())
                        {
                            pendingRequest = Load<PendingRequest>((IDataReader)response);
                        }
                    }
                }
            }
            return pendingRequest;
        }
    }
}
