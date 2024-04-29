using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class APIStatus
    {
        public string? RequestNumber { get; set; }
        public string? TokenNumber { get; set; }
        public string? CustomerNumber { get; set; }
        public string? OrderNumber { get; set; }
        public string? CertificateExpire { get; set; }
        public string? Status { get; set; }
        public string? TokenID { get; set; }
        public string? RequestErrorMessage { get; set; }
        public string? TokenErrorMessage { get; set; }

        public string? DomainName { get; set; }
    }

}