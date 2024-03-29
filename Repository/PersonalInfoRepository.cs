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
    public class PersonalInfoRepository : BaseRepository<PersonalInfo>
    {
        public PersonalInfoRepository(ISqlConnectionInformation sqlConnectionInformation)
        {
            ConnectionInformation = sqlConnectionInformation;
        }

        public override async Task<PersonalInfo> Get(string code)
        {
            PersonalInfo model = null;
            var storedProcedureName = "GetPersonalInfoByCode";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<PersonalInfo>(reader);
                    }
                }
            }
            return model;
        }

        public override async Task<List<PersonalInfo>> Get(int Id)
        {
            List<PersonalInfo> models = new List<PersonalInfo>();
            var storedProcedureName = "GetPersonalInfos";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<PersonalInfo>(reader);
                        models.Add(model);
                    }
                }

            }

            return models;
        }

        public override async Task<PersonalInfo> Put(PersonalInfo personalInfo)
        {
            string cmdText = "UpsertPersonalInfo";
            var personalInfoResponse = new PersonalInfo();

            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (PersonalInfo)Convert.ChangeType(personalInfo, typeof(PersonalInfo));

                    var personalInfoDetails = new Dictionary<string, object>();
                    var modelProperties = typeof(PersonalInfo).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        var modelPropertyValue = model.GetType().GetProperty(property.Name).GetValue(model);
                        var searcValue = model.GetType().GetProperty(property.Name).GetValue(model);

                        if (modelPropertyValue != null)
                        {
                            personalInfoDetails.Add(property.Name, modelPropertyValue);

                        }
                    }

                    if (personalInfoDetails.Any())
                    {
                        foreach (var f in personalInfoDetails)
                        {

                            command.Parameters.Add(new SqlParameter($"{f.Key}", f.Value));
                        }
                    }

                    using (var response = command.ExecuteReaderAsync())
                    {
                        while (await response.Result.ReadAsync())
                        {
                            personalInfoResponse = Load<PersonalInfo>((IDataReader)response);
                        }
                    }
                }
            }
            return personalInfoResponse;
        }
    }
}
