using Model;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Repository
{
    public class CustomerRepository : BaseRepository<CustomerDetail>
    {
        
        public CustomerRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<CustomerDetail> GetCustomerData(CustomerRequest serviceProviderRequest)
        {
            CustomerDetail response = null;
            var storedProcedureName = "GetCustomersData";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNo", serviceProviderRequest.RequestNo));
                    command.Parameters.Add(new SqlParameter("@LoggedInUserId", serviceProviderRequest.LoggedInUserId));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        response = Load<CustomerDetail>(reader);
                    }
                }

            }

            return response;
        }


        public override async Task<List<CustomerList>> GetAllCustomer(int status)
        {
            List<CustomerList> models = new List<CustomerList>();
            var storedProcedureName = "GetCustomersData";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@Status", status));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var model = Load<CustomerList>(reader);
                        models.Add(model);
                    }
                }

            }

            return models;
        }

        public override async Task<CustomerResponse> UpdateKYCCustomerDetails(CustomerUpdate customerUpdate, string certificate, string certificatePath)
        {
            string cmdText = "UpdateKYCCustomerDetails";
            CustomerResponse custDetail = null;
            
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    
                    command.Parameters.Add(new SqlParameter("@RequestNo", customerUpdate.RequestNo));
                    command.Parameters.Add(new SqlParameter("@AddharVerificationStatus", customerUpdate.AddharVerificationStatus));
                    command.Parameters.Add(new SqlParameter("@PanVerificationStatus", customerUpdate.PanVerificationStatus));
                    command.Parameters.Add(new SqlParameter("@Email", customerUpdate.Email));
                    command.Parameters.Add(new SqlParameter("@LoggedInUserId", customerUpdate.LoggedInUserId));
                    command.Parameters.Add(new SqlParameter("@Comments", customerUpdate.Comments));
                    command.Parameters.Add(new SqlParameter("@Certificates", certificate));
                    command.Parameters.Add(new SqlParameter("@CertificatesPath", certificatePath));
                    try
                    {
                        var response = await command.ExecuteNonQueryAsync();
                        if (response > 0)
                        {
                            custDetail = new CustomerResponse();
                            if (customerUpdate.AddharVerificationStatus && customerUpdate.PanVerificationStatus)
                            {
                                custDetail.Message = "KYC Verification is completed and additional details are sent to registered email id";
                            }
                            else
                            {
                                custDetail.Message = "Notification has been sent to registered email id of the customer";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                }
            }
            return custDetail;
        }
    }
}
