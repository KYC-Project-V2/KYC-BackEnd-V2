using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class RequestVerification
    {
        public int? CertificateId { get; set; }
        public string RequestNumber { get; set; }
        public string? DomainName { get; set; }
        public string? Origin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiredOn { get; set; }
        public string Url { get; set; }
        public string RedirectUrl { get; set; }
        public bool Status { get; set; }
        public string StatusMessgae { get; set; }
        public string HtmlBody { get; set; }

        public string? CertificatePath { get; set; }
    }
}
