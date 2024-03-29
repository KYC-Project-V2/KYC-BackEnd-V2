using Model;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace Repository
{
    public class RequestOriginRepository : BaseRepository<RequestOrigin>
    {
        public RequestOriginRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }



        public override async Task<RequestOrigin> Get(string code)
        {
            RequestOrigin model = null;
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
                        model = Load<RequestOrigin>(reader);
                    }
                }
            }
            return model;
        }
        public override async Task<RequestOrigin> Post(RequestOrigin requestOrigin)
        {
            string cmdText = "UpsertRequestOrigin";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (RequestOrigin)Convert.ChangeType(requestOrigin, typeof(RequestOrigin));

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
                            requestOrigin = Load<RequestOrigin>((IDataReader)response);
                        }
                    }
                }
            }
            return requestOrigin;
        }
    }
}
