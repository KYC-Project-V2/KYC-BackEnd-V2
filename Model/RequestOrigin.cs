using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class RequestOrigin
    {
        public string? RequestId { get; set; }
        public string? RequestNo { get; set; }
        public string? RequestDomainId { get; set; }
        public int? RequestTypeId { get; set; }
        public bool? Existing { get; set; }
        public string? CheckList { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Status { get; set; }

    }
}
