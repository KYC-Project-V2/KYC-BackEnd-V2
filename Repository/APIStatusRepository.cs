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
    public class APIStatusRepository : BaseRepository<APIStatus>
    {
        public APIStatusRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
        public override async Task<APIStatus> Get(APIStatus apiStatus)
        {
            try
            {
                APIStatus models = new APIStatus();
                var storedProcedureName = "GetAPIStatus";
                using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })

                    {
                        var model = (APIStatus)Convert.ChangeType(apiStatus, typeof(APIStatus));

                        var parameters = new Dictionary<string, object>();
                        var modelProperties = typeof(APIStatus).GetProperties().ToList();
                        foreach (var property in modelProperties)
                        {
                            var modelPropertyNameValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                            if (modelPropertyNameValue != null && property.Name != "OrderNumber"
                                && property.Name != "CertificateExpire" && property.Name != "Status" && property.Name != "TokenID"&& property.Name!= "RequestErrorMessage" && property.Name!= "TokenErrorMessage")
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
                        var reader = await command.ExecuteReaderAsync();
                        
                        while (reader.Read())
                        {
                            model = Load<APIStatus>(reader);
                            if(!string.IsNullOrEmpty(model.RequestErrorMessage) || !string.IsNullOrEmpty(model.TokenErrorMessage))
                            {
                                models.RequestErrorMessage = model.RequestErrorMessage;
                                return models;
                            }
                            else
                            {
                                models.CertificateExpire = model.CertificateExpire;
                                models.OrderNumber = model.OrderNumber;
                                models.Status = model.Status;
                                models.CustomerNumber = apiStatus.CustomerNumber;
                                return models;
                            }
                            
                        }
                    }
                   
                   
                }
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in retrieving data from the database", ex);
            }
        }
    }
    
}

