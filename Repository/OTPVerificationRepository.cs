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
    public class OTPVerificationRepository : BaseRepository<OTPVerification>
    {
        public OTPVerificationRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<OTPVerification> Put(OTPVerification oTPVerification)
        {
            string cmdText = "UpsertOTPVerification";
            var oTPVerificationoResponse = new OTPVerification();

            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (OTPVerification)Convert.ChangeType(oTPVerification, typeof(OTPVerification));

                    var oTPVerifications = new Dictionary<string, object>();
                    var modelProperties = typeof(OTPVerification).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {

                        switch (property.Name)
                        {
                            case "Subject":
                            case "Id":
                                break;
                            default:
                                var modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if (modelPropertyValue != null)
                                {
                                    oTPVerifications.Add(property.Name, modelPropertyValue);

                                }
                                break;
                        }
                        //var modelPropertyValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        //var searcValue = model.GetType().GetProperty(property.Name).GetValue(model);

                        //if (modelPropertyValue != null)
                        //{
                        //    personalInfoDetails.Add(property.Name, modelPropertyValue);

                        //}
                    }

                    if (oTPVerifications.Any())
                    {
                        foreach (var f in oTPVerifications)
                        {

                            command.Parameters.Add(new SqlParameter($"{f.Key}", f.Value));
                        }
                    }

                    using (var response =await command.ExecuteReaderAsync())
                    {
                        while (await response.ReadAsync())
                        {
                            oTPVerificationoResponse = Load<OTPVerification>((IDataReader)response);
                        }
                    }
                }
            }
            return oTPVerificationoResponse;
        }

        public override async Task<OTPVerification> Get(string code)
        {
            List<OTPVerification> models = new List<OTPVerification>();
            var storedProcedureName = "GetOtpVerification";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", code));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var entity = Load<OTPVerification>(reader);
                        models.Add(entity);
                    }
                }

            }
            var model = models != null && models.Any() ? models.FirstOrDefault(item => item.RequestNumber == code) : null;
            return model;
        }

        public override async Task<bool> Delete(OTPVerification model)
        {
            var commandText = "RemoveOTP";
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
