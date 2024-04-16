using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Dashboard
    {
        public int PendingCustomerQueueCount { get; set; }
        public int IssuedCustomerQueueCount { get; set; }
        public int TotalCustomerRequestCount { get; set; }
        public int PendingServiceProvidersQueueCount { get; set; }
        public int IssuedServiceProvidersQueueCount { get; set; }
        public int TotalServiceProvidersRequestCount { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
