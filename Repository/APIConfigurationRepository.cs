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
    public class APIConfigurationRepository : BaseRepository<APIConfiguration>
    {
        public APIConfigurationRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<List<APIConfiguration>> Get()
        {
            List<APIConfiguration> models = new List<APIConfiguration>();
            var storedProcedureName = "GetAPIConfiguration";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<APIConfiguration>(reader);
                        models.Add(model);
                    }
                }
            }
            return models;
        }
        public override async Task<APIConfiguration> Put(APIConfiguration aPIConfiguration)
        {
            string cmdText = "UpsertAPIConfiguration";
            var emailConfigurationResponce = new APIConfiguration();
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (APIConfiguration)Convert.ChangeType(aPIConfiguration, typeof(APIConfiguration));
                    var apiConfigurationDetails = new Dictionary<string, object>();
                    var modelProperties = typeof(APIConfiguration).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        var modelPropertyValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        var searcValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        if (modelPropertyValue != null)
                        {
                            apiConfigurationDetails.Add(property.Name, modelPropertyValue);
                        }
                    }
                    if (apiConfigurationDetails.Any())
                    {
                        foreach (var f in apiConfigurationDetails)
                        {
                            command.Parameters.Add(new SqlParameter($"{f.Key}", f.Value));
                        }
                    }
                    using (var response = command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await response.Result.ReadAsync())
                            {
                                emailConfigurationResponce = Load<APIConfiguration>((IDataReader)response);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return emailConfigurationResponce;
        }
        public override async Task<bool> Delete(APIConfiguration aPIConfiguration)
        {
            var commandText = "RemoveAPIConfiguration";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@Id", aPIConfiguration.Id));
                    command.Parameters.Add(new SqlParameter("@UpdatedDate", aPIConfiguration.UpdatedDate));
                    command.Parameters.Add(new SqlParameter("@UpdatedBy", aPIConfiguration.UpdatedBy));
                    var response = await command.ExecuteNonQueryAsync();
                    return response > 1;
                }
            }
        }
    }
}
