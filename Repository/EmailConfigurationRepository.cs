using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace Repository
{
    public class EmailConfigurationRepository : BaseRepository<EmailConfiguration>
    {
        public EmailConfigurationRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<List<EmailConfiguration>> Get()
        {
            List<EmailConfiguration> models = new List<EmailConfiguration>();
            var storedProcedureName = "GetEmailConfiguration";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<EmailConfiguration>(reader);
                        models.Add(model);
                    }
                }
            }
            return models;
        }
        public override async Task<EmailConfiguration> Put(EmailConfiguration emailConfiguration)
        {
            string cmdText = "UpsertEmailConfiguration";
            var emailConfigurationResponce = new EmailConfiguration();
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (EmailConfiguration)Convert.ChangeType(emailConfiguration, typeof(EmailConfiguration));
                    var emailConfigurationDetails = new Dictionary<string, object>();
                    var modelProperties = typeof(EmailConfiguration).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        var modelPropertyValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        var searcValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        if (modelPropertyValue != null)
                        {
                            emailConfigurationDetails.Add(property.Name, modelPropertyValue);
                        }
                    }
                    if (emailConfigurationDetails.Any())
                    {
                        foreach (var f in emailConfigurationDetails)
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
                                emailConfigurationResponce = Load<EmailConfiguration>((IDataReader)response);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return emailConfigurationResponce;
        }
    }
}