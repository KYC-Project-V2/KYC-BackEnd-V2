using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserDetail
    {
        public string ?Id { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Reference { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleId { get; set; }
        public string? ErrorMessage { get; set;}

    }

    public class UserDetailResponse {
        public string UserId { get; set; }
        public string Message { get; set; }

    }
}
