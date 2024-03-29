using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SqlConnectionInformation : ISqlConnectionInformation
    {
        public string ConnectionString { get; set; }
        public SqlConnectionInformation(AppSettings appSettings)
        {
            ConnectionString = $@"Data Source={appSettings.DatabaseServer};Initial Catalog={appSettings.DatabaseName};Application Name={appSettings.ApplicationName};uid={appSettings.DatabaseUser};password={appSettings.DatabasePassword}";
        }
    }
}
