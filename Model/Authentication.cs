using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Authentication
    {
        // Repository checks
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? DomainName { get; set; }
        public string? SecretKey { get; set; }
        public string? ReturnUrl { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
