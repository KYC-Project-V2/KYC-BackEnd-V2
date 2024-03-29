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
    public class PaymentRepository : BaseRepository<Payment>
    {
        public PaymentRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<List<Payment>> GetAll(string code)
        {
            List<Payment> models = new List<Payment>();
            var storedProcedureName = "GetPaymentInfoByRequestNumber";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var entity = Load<Payment>(reader);
                        models.Add(entity);
                    }
                }

            }
            return models;
        }

        public override async Task<Payment> Get(string code)
        {
            var model = new Payment();
            var storedProcedureName = "GetPaymentDetail";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@PaymentId", Convert.ToInt64(code)));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<Payment>(reader);
                    }
                }

            }
            return model;
        }

        public override async Task<Payment> Post(Payment payment)
        {
            string cmdText = "UpsertPayDetails";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (Payment)Convert.ChangeType(payment, typeof(Payment));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(Payment).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "PaymentId":
                            case "CreatedDate":
                            case "UpdatedDate":
                            case "UpdatedBy":
                            case "KycPrice":
                            case "KycGST":
                                break;
                            default:
                                var modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if (modelPropertyValue == null)
                                {
                                    modelPropertyValue = DBNull.Value;
                                }
                                parameters.Add(property.Name, modelPropertyValue);
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
                    using (var response = await command.ExecuteReaderAsync())
                    {
                        while (await response.ReadAsync())
                        {
                            payment = Load<Payment>((IDataReader)response);
                        }
                    }
                }
            }
            return payment;
        }
    }
}
