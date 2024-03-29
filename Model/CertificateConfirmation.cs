using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CertificateConfirmation
    {
        public int Id { get; set; }
        public int PendingDays { get; set; }
        public bool Weekly { get; set; } = false;
        public bool Daily { get; set; } = false;
        public string ErrorMsg { get; set; }
        public string NextEmailTiggeringDate { get; set; }
        public string ExpireDate { get; set; }
        public int IsActive { get; set; }
        public string RequestNumber { get; set; }
        public string Email { get; set; }
        public string ContactEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
    }
}
