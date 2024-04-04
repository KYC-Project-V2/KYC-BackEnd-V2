using Model;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.NetworkInformation;
using Utility;

namespace Repository
{
    public class ServiceProviderRepository : BaseRepository<SProvider>
    {
        public ServiceProviderRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }



        public override async Task<SProvider> Get(SProvider ServiceProvider)
        {
            SProvider model = null;
            var storedProcedureName = "GetServiceProviderInfo";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", ServiceProvider.RequestNumber));
                    command.Parameters.Add(new SqlParameter("@TokenNumber", ServiceProvider.RequestToken));
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            if (await reader.ReadAsync())
                            {
                                if (reader.GetString(reader.GetOrdinal("RequestErrorMessage")) != "" || reader.GetString(reader.GetOrdinal("TokenErrorMessage")) != "")
                                {
                                    ServiceProvider.RequestErrorMessage = reader.GetString(reader.GetOrdinal("RequestErrorMessage"));
                                    ServiceProvider.TokenErrorMessage = reader.GetString(reader.GetOrdinal("TokenErrorMessage"));
                                    ServiceProvider.SaltKey = null;
                                    return ServiceProvider;
                                }
                                SProvider insertedRecord = new SProvider
                                {
                                    // Example: Map the columns to properties of YourModel
                                    ProviderId = reader.GetInt32(reader.GetOrdinal("ProviderId")),
                                    ProviderName = reader.GetString(reader.GetOrdinal("ProviderName")),
                                    RequestNumber = reader.GetString(reader.GetOrdinal("RequestNumber")),
                                    RequestToken = KYCUtility.decrypt(reader.GetString(reader.GetOrdinal("RequestToken")), ServiceProvider.SaltKey),
                                    GST = KYCUtility.decrypt(reader.GetString(reader.GetOrdinal("GST")), ServiceProvider.SaltKey),
                                    PAN = KYCUtility.decrypt(reader.GetString(reader.GetOrdinal("PAN")), ServiceProvider.SaltKey),
                                    AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1")),
                                    AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2")),
                                    City = reader.GetString(reader.GetOrdinal("City")),
                                    StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
                                    PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                    CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    IPAddressRange = reader.GetString(reader.GetOrdinal("IPAddressRange")),
                                    ReturnUrl = reader.GetString(reader.GetOrdinal("ReturnUrl")),
                                    ApiStatus = reader.GetInt32(reader.GetOrdinal("ApiStatus")),
                                    ApiStatusText = reader.GetString(reader.GetOrdinal("ApiStatusText")),
                                    RequestErrorMessage = reader.GetString(reader.GetOrdinal("RequestErrorMessage")),
                                    TokenErrorMessage = reader.GetString(reader.GetOrdinal("TokenErrorMessage")),
                                    // Map other columns as needed
                                };

                                ServiceProvider = insertedRecord;
                            }
                            else
                            {
                                throw new Exception("No record returned after insert.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error retrieving inserted record: {ex.Message}");
                        }

                    }
                }
            }
            return ServiceProvider;
        }
        public override async Task<SProvider> Post(SProvider ServiceProvider)
        {
            string cmdText = "UpsertServiceProvider";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (SProvider)Convert.ChangeType(ServiceProvider, typeof(SProvider));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(SProvider).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        var modelPropertyNameValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                        if (modelPropertyNameValue != null &&  property.Name != "SaltKey" && property.Name != "ApiStatusText" && property.Name != "RequestErrorMessage" && property.Name != "TokenErrorMessage")
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

                    // Execute the insert query and get the inserted record
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            if (await reader.ReadAsync())
                            {
                                SProvider insertedRecord = new SProvider
                                {
                                    // Example: Map the columns to properties of YourModel
                                    ProviderId = reader.GetInt32(reader.GetOrdinal("ProviderId")),
                                    ProviderName = reader.GetString(reader.GetOrdinal("ProviderName")),
                                    RequestNumber = reader.GetString(reader.GetOrdinal("RequestNumber")),
                                    RequestToken =   KYCUtility.decrypt(reader.GetString(reader.GetOrdinal("RequestToken")), ServiceProvider.SaltKey),
                                    GST = KYCUtility.decrypt(reader.GetString(reader.GetOrdinal("GST")), ServiceProvider.SaltKey),
                                    PAN = KYCUtility.decrypt(reader.GetString(reader.GetOrdinal("PAN")), ServiceProvider.SaltKey),
                                    AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1")),
                                    AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2")),
                                    City = reader.GetString(reader.GetOrdinal("City")),
                                    StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
                                    PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                    CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    IPAddressRange = reader.GetString(reader.GetOrdinal("IPAddressRange")),
                                    ReturnUrl = reader.GetString(reader.GetOrdinal("ReturnUrl")),
                                    ApiStatus = reader.GetInt32(reader.GetOrdinal("ApiStatus")),
                                    ApiStatusText = reader.GetString(reader.GetOrdinal("ApiStatusText"))
                                    // Map other columns as needed
                                };

                                ServiceProvider = insertedRecord;
                            }
                            else
                            {
                                throw new Exception("No record returned after insert.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error retrieving inserted record: {ex.Message}");
                        }
                        
                    }
                }
            }
            return ServiceProvider;
        }
    }
}
