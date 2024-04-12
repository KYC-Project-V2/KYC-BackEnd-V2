using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class X509Certificate
    {
        public string RequestNumber { get; set; }
        public string DomainName { get; set; }
        public bool IsProvisional { get; set; } = true;
    }
}
