﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CustomerDetail
    {
        public string RequestNo { get; set; }
        public string DomainName { get; set; }        
        public string RequesterName { get; set; }
        public bool PanVerificationStatus { get; set; }
        public bool AddharVerificationStatus { get; set; }
        //public string Address { get; set; }
        public string Phone { get; set; }
        public string PanId { get; set; }
        public string AadharId { get; set; }        
        //public int StateId { get; set; }        
        //public string City { get; set; }
        public string Email { get; set; }
       
       // public int ZipCode { get; set; }
        public int RequestTypeId { get; set; }
        public string CustomerStatus { get; set; }
        public string AdharOCRInformation { get; set; }
        public string PanOCRInformation { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Comments { get; set; }
        public int LoggedInUserId { get; set; }
        public string CustomerRepresentative { get; set; }
        public string PreviousComments { get; set; }
        public string? ErrorMessage { get; set;}

    }
    public class CustomerList
    {
        public int SrNo { get; set; }
        public string RequestNo { get; set; }
        public string DomainName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string CertificateStatus { get; set; }
        
    }

    public class CustomerListResponse 
    {
       
        public int page { get; set; }
        public int perPage { get; set; }
        public int Total { get; set; }

        public int TotalPages { get; set; }
        public List<CustomerList> data { get; set; }
        public string? ErrorMessage { get; set; }
    }
    public class CustomerUpdate
    {
        public string RequestNo { get; set; }
        public string RequesterName { get; set; }
        public bool AddharVerificationStatus { get; set; }
        public bool PanVerificationStatus { get; set; }
        public string Email { get; set; }
        public int LoggedInUserId { get; set; }
        public string Comments { get; set; }
    }
    public class CustomerResponse
    {
        public string Message { get; set; }
        
    }
    public class CustomerRequest
    {
        public string RequestNo { get; set; }
        public int LoggedInUserId { get; set; }
    }
    
}
