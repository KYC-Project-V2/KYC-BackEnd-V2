using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Certificate
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; }
        public string DomainId { get; set; }
        public string DomainName { get; set; }
        public string Certificates { get; set; }
        public string ExpiredOn { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? NextEmailTiggeringDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
