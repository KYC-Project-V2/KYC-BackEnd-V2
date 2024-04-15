using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AadharInfoRepository : BaseRepository<AadharInfo>
    {
        public AadharInfoRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<AadharInfo> Get(string code)
        {
            AadharInfo model = null;
            var storedProcedureName = "GetAadharInfo";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNo", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<AadharInfo>(reader);
                    }
                }
            }
            return model;
        }

        public override async Task<AadharInfo> Post(AadharInfo aadharInfo)
        {
            string cmdText = "UpsertAadharInfo";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (AadharInfo)Convert.ChangeType(aadharInfo, typeof(AadharInfo));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(AadharInfo).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                          
                            case "Id":
                                parameters.Add("AadharId", model.Id);
                                break;
                            default:
                                var modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if (modelPropertyValue != null &&   property.Name != "IsVaidDocumentType")
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
                    try
                    {
                        using (var response = command.ExecuteReaderAsync())
                        {
                            while (await response.Result.ReadAsync())
                            {
                                aadharInfo = Load<AadharInfo>((IDataReader)response);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    
                }
            }
            return aadharInfo;
        }
    }
}
