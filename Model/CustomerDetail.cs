using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CustomerDetail
    {
        public string RequestNo { get; set; }
        public string DomainName { get; set; }        
        public string RequesterName { get; set; }
        public bool PanVerificationStatus { get; set; }
        public bool AddharVerificationStatus { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PanId { get; set; }
        public string AadharId { get; set; }        
        public int StateId { get; set; }        
        public string City { get; set; }
        public string Email { get; set; }
       
        public int ZipCode { get; set; }
        public int RequestTypeId { get; set; }
        public string CustomerStatus { get; set; }
        public string AdharOCRInformation { get; set; }
        public string PanOCRInformation { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime ExpireDate { get; set; }    
        public string? ErrorMessage { get; set;}

    }
    public class CustomerList
    {
        public string RequestNo { get; set; }
        public string DomainName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string CertificateStatus { get; set; }
        public string? ErrorMessage { get; set; }
    }
    public class CustomerRequest
    {
        public string RequestNo { get; set; }       
    }
    
}
