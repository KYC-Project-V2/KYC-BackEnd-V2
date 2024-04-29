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
    public class CertificateRepository : BaseRepository<Certificate>
    {
        public CertificateRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<List<Certificate>> GetAll(string code)
        {
            List<Certificate> models = new List<Certificate>();
            var storedProcedureName = "GetCertificate";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", code));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<Certificate>(reader);
                        if(model!=null)
                          model.ExpiredOn =Math.Round((model.ExpireDate - System.DateTime.Now).TotalDays).ToString();
                        models.Add(model);
                    }
                }
            }

            return models;
        }

        public override async Task<Certificate> Get(string name)
        {
            Certificate model = null;
            var storedProcedureName = "GetDomainCertificate";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@Name", name));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        model = Load<Certificate>(reader);
                        model.ExpiredOn = Math.Round((model.ExpireDate - DateTime.Now).TotalDays).ToString();
                    }
                }
            }
            return model;
        }

        public override async Task<Certificate> Post(Certificate certificate)
        {
            string cmdText = "UpsertCertificate";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (Certificate)Convert.ChangeType(certificate, typeof(Certificate));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(Certificate).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "ExpiredOn":
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
                    using (var response = command.ExecuteReaderAsync())
                    {
                        while (await response.Result.ReadAsync())
                        {
                            certificate = Load<Certificate>((IDataReader)response);
                        }
                    }
                }
            }
            return certificate;
        }

        public override async Task<List<Certificate>> GetCertificate(string requestNo, bool certificateType)
        {
            List<Certificate> models = new List<Certificate>();
            var storedProcedureName = "GetCertificateDataForEmail";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", requestNo));
                    command.Parameters.Add(new SqlParameter("@CertificateType", certificateType));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<Certificate>(reader);
                        model.ExpiredOn = Math.Round((model.ExpireDate - System.DateTime.Now).TotalDays).ToString();
                        models.Add(model);
                    }
                }
            }

            return models;
        }
    }
}
