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
    public class TemplateConfigurationRepository : BaseRepository<TemplateConfiguration>
    {
        public TemplateConfigurationRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<List<TemplateConfiguration>> Get(int Id)
        {
            List<TemplateConfiguration> models = new List<TemplateConfiguration>();
            var storedProcedureName = "GetTemplateConfiguration";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@TemplateTypeId", Id));

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<TemplateConfiguration>(reader);
                        models.Add(model);
                    }
                }
            }
            return models;
        }
        public override async Task<TemplateConfiguration> Put(TemplateConfiguration templateConfiguration)
        {
            string cmdText = "UpsertTemplateConfiguration";
            var emailConfigurationResponce = new TemplateConfiguration();
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (TemplateConfiguration)Convert.ChangeType(templateConfiguration, typeof(TemplateConfiguration));

                    command.Parameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
                    var apiConfigurationDetails = new Dictionary<string, object>();
                    var modelProperties = typeof(TemplateConfiguration).GetProperties().ToList();
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
                                emailConfigurationResponce = Load<TemplateConfiguration>((IDataReader)response);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return emailConfigurationResponce;
        }
        public override async Task<bool> Delete(TemplateConfiguration templateConfiguration)
        {
            var commandText = "RemoveTemplateConfiguration";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@Id", templateConfiguration.Id));
                    //command.Parameters.Add(new SqlParameter("@UpdatedDate", templateConfiguration.UpdatedDate));
                    //command.Parameters.Add(new SqlParameter("@UpdatedBy", templateConfiguration.UpdatedBy));
                    var response = await command.ExecuteNonQueryAsync();
                    return response > 1;
                }
            }
        }
    }
}
