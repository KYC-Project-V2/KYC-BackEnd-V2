using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model
{
    public class Domain
    {
        public string? Id { get; set; }
        public string? RequestNumber { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public long? PaymentId{ get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        public string? ResponseCode { get; set; }
        public string? UniqueRefNumber { get; set; }
        public string? ServiceTaxAmount { get; set; }
        public string? ProcessingFeeAmount { get; set; }
        public string? TotalAmount { get; set; }

        public string? TransactionAmount { get; set; }
        public string? TransactionDate { get; set; }
        public string? InterchangeValue { get; set; }

        public string? TDR { get; set; }
        public string? PaymentMode { get; set; }
        public string? SubMerchantId { get; set; }
        public string? ReferenceNo { get; set; }
        public string? MerchantId { get; set; }
        public string? RS { get; set; }
        public string? TPS { get; set; }
        public string? OptionalFields { get; set; }
        public string? MandatoryFields { get; set; }
    }
}
