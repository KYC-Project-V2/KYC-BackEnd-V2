﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        public bool? IsGSTVerificationStatus { get; set; } = false;
        public bool? IsPanVerficationStatus { get; set; } = false;

        public string ? GST { get; set; }
        public string? PAN { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? StateId { get; set; }
        public string? PostalCode { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IPAddressRange { get; set; }
        public string? ReturnUrl { get; set; }
        public DateTime? IssueDateTime { get; set; }
        public string? CustomerRepresentativeName { get; set; }
        public string? ApiStatus { get; set; }
        public string? Comments { get; set; }
        public string? VerificationStatus { get; set; }
        public string? PreviousComments { get; set; }
        public string? ErrorMessage { get; set; }

    }
    public class ServiceProviderRequest {
        public string RequestNumber { get; set; }
        public int LoggedInUserId { get; set; }

    }
    public class ServiceProviderResponse 
    {
        public string Message { get; set; }

    }
    public class ServiceProviderList
    {
        public string RequestNumber { get; set; }
        public string ProviderName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string? VerificationStatus { get; set; }
        public string? ErrorMessage { get; set; }

    }

    public class UpdateServiceProvider
    {
        public string RequestNumber { get; set; }
        public string ProviderName { get; set; }
        public bool IsPANVerificationStatus { get; set; }
        public bool IsGSTVerificationStatus { get; set; }
        public string Email { get; set; }
        public string? Comments { get; set; }
        public int LoggedInUserId { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
