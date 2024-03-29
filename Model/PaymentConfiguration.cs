using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PaymentConfiguration
    {
        public int Id { get; set; }
        public string? PaymentBaseUrl { get; set; }
        public string? MerchantId { get; set; }
        public string? AccessCode { get; set; }
        public string? AppRedirectUrl { get; set; }
        public string? PaymentReturnUrl { get; set; }
        public bool Status { get; set; }

    }
}
