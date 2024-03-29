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
    public class BusinessInCorpInfoRepository : BaseRepository<BusinessInCorpInfo>
    {
        public BusinessInCorpInfoRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<BusinessInCorpInfo> Get(string code)
        {
            BusinessInCorpInfo model = null;
            var storedProcedureName = "GetBusInCorpInfo";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNo", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<BusinessInCorpInfo>(reader);
                    }
                }
            }
            return model;
        }
        public override async Task<BusinessInCorpInfo> Post(BusinessInCorpInfo BusinessInCorpInfo)
        {
            string cmdText = "UpsertBusInCorpInfo";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (BusinessInCorpInfo)Convert.ChangeType(BusinessInCorpInfo, typeof(BusinessInCorpInfo));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(BusinessInCorpInfo).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "DOB":
                            case "Sex":
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
                            BusinessInCorpInfo = Load<BusinessInCorpInfo>((IDataReader)response);
                        }
                    }
                }
            }
            return BusinessInCorpInfo;
        }
    }
}
