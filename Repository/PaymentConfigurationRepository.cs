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
    public class PaymentConfigurationRepository : BaseRepository<PaymentConfiguration>
    {
        public PaymentConfigurationRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<List<PaymentConfiguration>> Get()
        {
            List<PaymentConfiguration> models = new List<PaymentConfiguration>();
            var storedProcedureName = "GetPaymentConfiguration";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<PaymentConfiguration>(reader);
                        models.Add(model);
                    }
                }
            }
            return models;
        }
        public override async Task<PaymentConfiguration> Put(PaymentConfiguration paymentConfiguration)
        {
            string cmdText = "UpsertPaymentConfiguration";
            var paymentConfigurationResponce = new PaymentConfiguration();
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (PaymentConfiguration)Convert.ChangeType(paymentConfiguration, typeof(PaymentConfiguration));
                    var paymentConfigurationDetails = new Dictionary<string, object>();
                    var modelProperties = typeof(PaymentConfiguration).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        var modelPropertyValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        var searcValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        if (modelPropertyValue != null)
                        {
                            paymentConfigurationDetails.Add(property.Name, modelPropertyValue);
                        }
                    }
                    if (paymentConfigurationDetails.Any())
                    {
                        foreach (var f in paymentConfigurationDetails)
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
                                paymentConfigurationResponce = Load<PaymentConfiguration>((IDataReader)response);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return paymentConfigurationResponce;
        }

        public override async Task<PaymentConfiguration> Get(bool status)
        {
            var storedProcedureName = "GetActivePaymentConfiguration";
            var model = new PaymentConfiguration();
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@Status", status));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        model = Load<PaymentConfiguration>(reader);
                    }
                }
            }
            return model;

        }
    }
}
