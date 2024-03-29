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
    public class TemplateTypeRepository : BaseRepository<TemplateType>
    {
        public TemplateTypeRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<List<TemplateType>> Get()
        {
            List<TemplateType> models = new List<TemplateType>();
            var storedProcedureName = "GetTemplateTypes";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<TemplateType>(reader);
                        models.Add(model);
                    }
                }

            }
            return models;
        }
    }
}
