using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model
{
    public class EmailConfiguration
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool Status { get; set; }

    }
}