using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    public class Payment 
    {


        public long? PaymentId { get; set; }
        public string? ResponseCode { get; set; }
        public string? UniqueRefNumber { get; set; }
        public string? ServiceTaxAmount { get; set; }
        public string? ProcessingFeeAmount { get; set; }
        public string? TotalAmount { get; set; }
        
        public string? TransactionAmount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? InterchangeValue { get; set; }

        public string? TDR { get; set; }
        public string? PaymentMode { get; set; }
        public string? SubMerchantId { get; set; }
        public string? ReferenceNo { get; set; }
        public string? RequestNumber { get; set; }
        public string? ID { get; set; }
        public string? RS { get; set; }
        public string? TPS { get; set; }
        public string? OptionalFields { get; set; }
        public string? MandatoryFields { get; set; }
        public string? CreatedBy { get; set; }

        public int KycPrice { get; set; }
        public int KycGST { get; set; }
    }
}
