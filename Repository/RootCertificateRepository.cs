using Model;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using Microsoft.Extensions.DependencyInjection;
using Utility;

namespace Repository
{
    public class RootCertificateRepository : BaseRepository<RootCertificate>
    {
        public RootCertificateRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<RootCertificate> Get(string rootid)
        {
            RootCertificate model = null;
            var storedProcedureName = "GetRootCertificate";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    //command.Parameters.Add(new SqlParameter("@Name", name));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        model = Load<RootCertificate>(reader);
                    }
                }
            }
            return model;
        }

        public override async Task<RootCertificate> Post(RootCertificate certificate)
        {
            string cmdText = "UpsertRootCertificate";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (RootCertificate)Convert.ChangeType(certificate, typeof(RootCertificate));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(RootCertificate).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "UpdateDate":
                            case "UpdatedBy":
                            case "Id":
                                break;
                            default:
                                var modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if (modelPropertyValue != null)
                                {
                                    parameters.Add(property.Name, modelPropertyValue);

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
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            if (await reader.ReadAsync())
                            {
                                RootCertificate rootCertificate = new RootCertificate
                                {
                                    // Example: Map the columns to properties of YourModel
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Certificates = reader.GetString(reader.GetOrdinal("Certificates")),
                                    ExpireDate = reader.GetDateTime(reader.GetOrdinal("ExpireDate")),
                                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    // Map other columns as needed
                                };

                                certificate = rootCertificate;
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
            return certificate;
        }
    }
}
