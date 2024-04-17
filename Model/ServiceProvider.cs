﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ServiceProvider
    {
        public int? ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string RequestNumber { get; set; }
        public string? RequestToken { get; set; }
        //public string? GST { get; set; }
        //public string? PAN { get; set; }
        public bool? IsGSTVerificationStatus { get; set; } = false;
        public bool? IsPanVerficationStatus { get; set; } = false;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public int? StateId { get; set; }
        public string? PostalCode { get; set; }
        public int? CountryId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IPAddressRange { get; set; }
        public string? ReturnUrl { get; set; }
        public DateTime? IssueDateTime { get; set; }
        public string? CustomerRepresentativeName { get; set; }
        public int? ApiStatus { get; set; }
        public string? ErrorMessage { get; set; }

    }
    public class ServiceProviderRequest {
        public string RequestNumber { get; set; }
    }
    public class ServiceProviderList
    {
        public string RequestNumber { get; set; }
        public string ProviderName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
