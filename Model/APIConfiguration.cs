using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class APIConfiguration
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? SandboxEndPoint { get; set; }
        public string? ApiEndPoint { get; set; }
        public string? Token { get; set; }
        public string? Version { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
