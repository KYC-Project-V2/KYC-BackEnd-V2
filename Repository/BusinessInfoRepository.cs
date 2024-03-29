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
    public class BusinessInfoRepository : BaseRepository<BusinessInfo>
    {
        public BusinessInfoRepository(ISqlConnectionInformation sqlConnectionInformation) 
        {
            ConnectionInformation = sqlConnectionInformation;
        }
      
        public override async Task<List<BusinessInfo>> Get(int Id)
        {
            List<BusinessInfo> models = new List<BusinessInfo>();
            var storedProcedureName = "GetPersonalInfos";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = Load<BusinessInfo>(reader);
                        models.Add(model);
                    }
                }

            }

            return models;
        }
        public override async Task<BusinessInfo> Get(string code)
        {
            var model = new BusinessInfo();
            var storedProcedureName = "GetBusinessInfoByCode";
            using (SqlConnection connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@RequestNumber", code));
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        model = Load<BusinessInfo>(reader);
                    }
                }

            }
            return model;
        }
        public override async Task<BusinessInfo> Put(BusinessInfo businessInfo)
        {
            string cmdText = "UpsertBusinessInfo";
            var businessInfoResponce = new BusinessInfo();

            using (var connection = new SqlConnection(ConnectionInformation.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection) { CommandType = CommandType.StoredProcedure })
                {
                    var model = (BusinessInfo)Convert.ChangeType(businessInfo, typeof(BusinessInfo));

                    var businessInfoDetails = new Dictionary<string, object>();
                    var modelProperties = typeof(BusinessInfo).GetProperties().ToList();
                    foreach (var property in modelProperties)
                    {
                        switch (property.Name)
                        {
                            case "CompanyRevenue":
                            case "Website":
                            case "GstNumber":
                            case "NearBy":
                            case "RegistrationNumber":
                            case "PanCardNumber":
                            case "InusranceNumber":
                            case "LicenseNumber":
                            case "Rating":
                            case "Purpose":
                            case "OwnerFirstName":
                            case "OwnerLastName":
                                businessInfoDetails.Add(property.Name, "");
                                break;
                            case "TotalEmp":
                            case "GrossAnnualSales":
                                businessInfoDetails.Add(property.Name, 0);
                                break;
                            case "ContactMiddleName":
                                businessInfoDetails.Add(property.Name, businessInfo.ContactMiddleName ?? "");
                                break;
                            case "ContactAddress":
                                businessInfoDetails.Add(property.Name, businessInfo.ContactAddress ?? "");
                                break;
                            default:

                                var modelPropertyValue = model.GetType().GetProperty(property.Name).GetValue(model);
                                var searcValue = model.GetType().GetProperty(property.Name).GetValue(model);

                                if (modelPropertyValue != null)
                                {
                                    businessInfoDetails.Add(property.Name, modelPropertyValue);

                                }
                                break;
                        }
                    }

                    if (businessInfoDetails.Any())
                    {
                        foreach (var f in businessInfoDetails)
                        {

                            command.Parameters.Add(new SqlParameter($"{f.Key}", f.Value));
                        }
                    }
                 
                        using (var response = command.ExecuteReaderAsync())
                        {
                            while (await response.Result.ReadAsync())
                            {
                                businessInfoResponce = Load<BusinessInfo>((IDataReader)response);
                            }
                        }
                }
            }
            return businessInfoResponce;
        }

    }
}
