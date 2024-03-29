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
    public class DomainRepository : BaseRepository<Domain>
    {
        public DomainRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<List<Domain>> GetAll(string code)
        {
            List<Domain> models = new List<Domain>();
            var storedProcedureName = "GetDomains";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", code));

                    var reader =  await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var model = Load<Domain>(reader);
                        models.Add(model);
                    }
                }

            }

            return models;
        }
        public override async Task<Domain> Post(Domain domain)
        {
            string cmdText = "UpsertDomain";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (Domain)Convert.ChangeType(domain, typeof(Domain));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(Domain).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        
                        switch (property.Name)
                        {
                            case "ResponseCode":
                            case "UniqueRefNumber":
                            case "ServiceTaxAmount":
                            case "ProcessingFeeAmount":
                            case "TotalAmount":
                            case "TransactionAmount":
                            case "TransactionDate":
                            case "InterchangeValue":
                            case "TDR":
                            case "PaymentMode":
                            case "SubMerchantId":
                            case "ReferenceNo":
                            case "MerchantId":
                            case "RS":
                            case "TPS":
                            case "OptionalFields":
                            case "MandatoryFields":
                                break;
                            default:
                                var modelPropertyNameValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if (modelPropertyNameValue != null)
                                {
                                    parameters.Add(property.Name, modelPropertyNameValue);
                                }
                                break;
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
                            domain = Load<Domain>((IDataReader)response);
                        }
                    }
                }
            }
            return domain;
        }

        public override async Task<bool> Delete(Domain model)
        {
            var commandText = "RemoveDomain";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", model.RequestNumber));
                    command.Parameters.Add(new SqlParameter("@Id", model.Id));

                    var response = await command.ExecuteNonQueryAsync();
                    return response > 1;
                }
            }
        }
    }
}
