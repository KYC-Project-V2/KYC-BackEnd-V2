using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BusinessInfo
    {
        public string? CompanyName { get; set; }
        public string? CompanyRevenue { get; set; }
        public string? RequestNo { get; set; }
        public int? RequestTypeId { get; set; }
        public string? OwnerFirstName { get; set; }
        public string? OwnerLastName { get; set; }
        public string? OfficialEmail { get; set; }
        public string? OfficialPhone { get; set; }

        public string? Website { get; set; } 
        public string? Address { get; set; }
        public string? NearBy { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public string? GstNumber { get; set; }
        public string? TotalEmp { get; set; }
        public decimal? GrossAnnualSales { get; set; }
        public string? RegistrationNumber { get; set; }

        public string? InusranceNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public string? PanCardNumber { get; set; }
        public string? Purpose { get; set; }
        public string? ContactFirstName { get; set; }

        public string? ContactMiddleName { get; set; }

        public string? ContactLastName { get; set; }

        public string? ContactEmail { get; set; }

        public string? ContactPhone { get; set; }

        public string? ContactAddress { get; set; }

        public string? ContactCity { get; set; }

        public string? ContactZipCode { get; set; }

        public string? Designation { get; set; }

        public int? StateId { get; set; }
        public int? ContactStateId { get; set; }

    }
}
