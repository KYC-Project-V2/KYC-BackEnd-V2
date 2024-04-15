using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class X509Certificate
    {
        public string RequestNumber { get; set; }
        public string DomainName { get; set; }
        public bool IsProvisional { get; set; } = true;
        public string? CARootPath { get; set; }

        public byte[]? CertificateBytes { get; set; }
        public string? SerialNumber { get; set; }
    }
}
