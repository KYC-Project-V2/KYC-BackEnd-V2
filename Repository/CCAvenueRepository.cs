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
    public class CCAvenueRepository : BaseRepository<CCAvenueResponse>
    {
        public CCAvenueRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }
    
        public override async Task<CCAvenueResponse> Post(CCAvenueResponse ccAvenueResponse)
        {
            string cmdText = "UpsertCCAvenuePayDetails";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (CCAvenueResponse)Convert.ChangeType(ccAvenueResponse, typeof(CCAvenueResponse));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(CCAvenueResponse).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "CCAvenueId":
                            case "CreatedDate":
                            case "UpdatedDate":
                            case "UpdatedBy":
                                break;
                            default:
                                var modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if(modelPropertyValue== null)
                                    modelPropertyValue=DBNull.Value;
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
                    using (var response = command.ExecuteReaderAsync())
                    {
                        while (await response.Result.ReadAsync())
                        {
                            ccAvenueResponse = Load<CCAvenueResponse>((IDataReader)response);
                        }
                    }
                }
            }
            return ccAvenueResponse;
        }
    }
}
