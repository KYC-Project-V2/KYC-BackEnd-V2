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
    public class VoterInfoRepository : BaseRepository<VoterInfo>
    {
        public VoterInfoRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<VoterInfo> Post(VoterInfo VoterInfo)
        {
            string cmdText = "UpsertVoterInfo";
            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (VoterInfo)Convert.ChangeType(VoterInfo, typeof(VoterInfo));

                    var parameters = new Dictionary<string, object>();
                    var modelProperties = typeof(VoterInfo).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "Address":
                            case "FolderPath":
                            case "OCRInformation":
                            case "FullPath":
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
                            VoterInfo = Load<VoterInfo>((IDataReader)response);
                        }
                    }
                }
            }
            return VoterInfo;
        }

        public override async Task<VoterInfo> Get(string code)
        {
            VoterInfo model = null;
            var storedProcedureName = "GetVoterInfo";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNo", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<VoterInfo>(reader);
                    }
                }
            }
            return model;
        }
    }
}
