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
    public class PanCardInfoRepository : BaseRepository<PanCardInfo>
    {
        public PanCardInfoRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<PanCardInfo> Get(string code)
        {
            PanCardInfo model = null;
            var storedProcedureName = "GetPanCardInfo";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNo", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<PanCardInfo>(reader);
                    }
                }
            }
            return model;
        }

        public override async Task<PanCardInfo> Post(PanCardInfo PanCardInfo)
        {
            string cmdText = "UpsertPanCardInfo";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (PanCardInfo)Convert.ChangeType(PanCardInfo, typeof(PanCardInfo));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(PanCardInfo).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            default:
                                var modelPropertyValue = model.GetType().GetProperty(property.Name)?.GetValue(model);
                                if (modelPropertyValue != null &&  property.Name != "IsVaidDocumentType")
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
                                PanCardInfo = Load<PanCardInfo>((IDataReader)response);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                   
                }
            }
            return PanCardInfo;
        }
    }
}
