using Model;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Repository
{
    public class CertificateConfirmationRepository : BaseRepository<CertificateConfirmation>
    {
        public CertificateConfirmationRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<List<CertificateConfirmation>> GetAll(string noofdays)
        {
            List<CertificateConfirmation> models = new List<CertificateConfirmation>();
            var storedProcedureName = "GetCertificateConfirmation";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<CertificateConfirmation>(reader);
                        models.Add(model);
                    }
                }
            }
            return models;
        }

        public override async Task<CertificateConfirmation> Post(CertificateConfirmation certificate)
        {
            if(certificate.PendingDays > 7 && certificate.Weekly == true)
            {
                // Update the Certificate Table with the GetDate + 7
            }
            if (certificate.PendingDays != 0 && certificate.PendingDays < 7 && certificate.Daily == true)
            {
                // Update the Certificate Table with the GetDate + 1
            }

            string cmdText = "InsertOrUpsertCertificateConformationEmail";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (CertificateConfirmation)Convert.ChangeType(certificate, typeof(CertificateConfirmation));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(CertificateConfirmation).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "ErrorMsg":
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
                            certificate = Load<CertificateConfirmation>((IDataReader)response);
                        }
                    }
                }
            }
            return certificate;
        }
    }
}
