using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TemplateConfiguration
    {
        public int? Id { get; set; }
        public int? TemplateTypeId { get; set; }
        public int? Days { get; set; }
        public bool? Weekly { get; set; }
        public bool? Daily { get; set; }
        public string? Sender { get; set; }

        public string? Subject { get; set; }
        public string? Body { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
