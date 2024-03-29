using Dynamsoft.DBR;
using Model;
using Model.DocumentQR;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Service
{
    public class DriverLicenseInfoService : BaseService<DriverLicenseInfo>
    {
        public DriverLicenseInfoService(IRepository<DriverLicenseInfo> repository)
        {
            Repository = repository;
        }

    }
}
