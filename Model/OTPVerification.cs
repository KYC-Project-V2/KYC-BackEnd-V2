using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OTPVerification
    {
        public int Id { get; set; } 
        public string? RequestNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? OtpNumber { get; set; }
        public string? Subject { get; set; }
        public string? Type { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public bool? IsVerified { get; set; }
    }
}
