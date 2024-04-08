using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CustomerList
    {
        public int RequestNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Phone { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
