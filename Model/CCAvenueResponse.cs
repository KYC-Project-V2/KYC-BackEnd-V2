using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CCAvenueResponse
    {
        public Int64 CCAvenueId { get; set; }
        public string? RequestNumber { get; set; }
        public string? TrackingId { get; set; }
        public string? BankRefno { get; set; }
        public string? OrderStatus { get; set; }
        public string? FailureMessage { get; set; }
        public string? PaymentMode { get; set; }
        public string? CardName { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public string? Currency { get; set; } = "INR";
        public string? Amount { get; set; }
        public string? BillingName { get; set; }
        public string? Vault { get; set; }
        public string? OfferType { get; set; }
        public string? OfferCode { get; set; }
        public string? DiscountValue { get; set; }
        public string? MerAmount { get; set; }
        public string? EciValue { get; set; }
        public string? Retry { get; set; }
        public string? ResponseCode { get; set; }
        public string? TransDate { get; set; }
        public string? BinCountry { get; set; }
        public string? CreatedBy { get; set; }
    }
}
