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
    public class BusinessPanCardInfoRepository : BaseRepository<BusinessPanCardInfo>
    {
        public BusinessPanCardInfoRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<BusinessPanCardInfo> Get(string code)
        {
            BusinessPanCardInfo model = null;
            var storedProcedureName = "GetBusPanCardInfo";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNo", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<BusinessPanCardInfo>(reader);
                    }
                }
            }
            return model;
        }
        public override async Task<BusinessPanCardInfo> Post(BusinessPanCardInfo BusinessPanCardInfo)
        {
            string cmdText = "UpsertBusPanCardInfo";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (BusinessPanCardInfo)Convert.ChangeType(BusinessPanCardInfo, typeof(BusinessPanCardInfo));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(BusinessPanCardInfo).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "Sex":
                                break;
                            case "DOB":
                                var modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if (modelPropertyValue != null)
                                {
                                    parameters.Add("DOI", modelPropertyValue);

                                }
                                break;
                            default:
                                modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
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
                            BusinessPanCardInfo = Load<BusinessPanCardInfo>((IDataReader)response);
                        }
                    }
                }
            }
            return BusinessPanCardInfo;
        }
    }
}
