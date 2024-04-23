using Model;
using Org.BouncyCastle.Asn1.X509;
//using Org.BouncyCastle.Crypto.Tls;
using Repository;
using System.Text;
//using Twilio.Rest;
using Utility;

namespace Service
{
    public class CustomerService : BaseService<CustomerDetail>
    {
        private readonly IService<TemplateConfiguration> _templateConfigurationservice;
        private readonly IService<Certificate> _certificateService;
        private readonly IService<Email> _emailService;
        private readonly IService<Domain> _domainService;
        private readonly IService<RootCertificate> _rootCertificateService;
        public CustomerService(IRepository<CustomerDetail> repository, 
            IService<TemplateConfiguration> templateConfigurationservice, 
            IService<Certificate> certificateService, 
            IService<Email> emailService,
            IService<Domain> domainService,
            IService<RootCertificate> rootCertificateService)
        {
            Repository = repository;
            _templateConfigurationservice = templateConfigurationservice;
            _certificateService = certificateService;
            _emailService = emailService;
            _domainService = domainService;
            _rootCertificateService = rootCertificateService;
        }
        public override async Task<CustomerDetail> Get(string Id)
        {
            var response = await Repository.Get(Id);
            if (response == null)
            {
                var customer = new CustomerDetail();
                customer.ErrorMessage = "No User Found";
                return customer;
            }
            return response;
        }

        public override async Task<List<CustomerList>> GetAllCustomer(int status)
        {
            var response = await Repository.GetAllCustomer(status);
            if (response == null)
            {
                var customerList = new List<CustomerList>();
                customerList.Add(new CustomerList
                {
                    ErrorMessage = "No Customers Found"

                });

                return customerList;
            }
            else // Filters Customers 
            {
                if (status == 1) // Get the Issued Records
                {
                    response = response.Where(i => i.CertificateStatus == "Issued").ToList();
                }
                else // Get the Pending Records
                {
                    response = response.Where(i => i.CertificateStatus != "Issued").ToList();
                }
            }
            return response;
        }

        public override async Task<CustomerResponse> UpdateKYCCustomerDetails(CustomerUpdate customerUpdate, string certificates, string certificatesPath)
        {
            // Get the CertificatePath and Certificate Values

            if (customerUpdate != null)
            {
                if (customerUpdate.AddharVerificationStatus && customerUpdate.PanVerificationStatus)
                {
                    var domains = await _domainService.GetAll(customerUpdate.RequestNo);
                    if (domains != null)
                    {
                        var getDomain = domains.FirstOrDefault();
                        var x509certificate = new Model.X509Certificate();
                        x509certificate.IsProvisional = false;
                        x509certificate.DomainName = getDomain.Name;
                        x509certificate.RequestNumber = customerUpdate.RequestNo;
                        var rootcertificate = await _rootCertificateService.Get(string.Empty);
                        x509certificate.CARootPath = rootcertificate.Certificates;
                        var apidownloadFilebytes = KYCUtility.GetX509Certificate(x509certificate);
                        certificates = apidownloadFilebytes.SerialNumber != null ? apidownloadFilebytes.SerialNumber  : "" ;
                        certificatesPath = (apidownloadFilebytes.CertificateBytes != null) ? Convert.ToBase64String(apidownloadFilebytes.CertificateBytes) : "";
                    }
                }
            }

            var response = await Repository.UpdateKYCCustomerDetails(customerUpdate, certificates, certificatesPath);

            if (response == null)
            {
                response = new CustomerResponse();
                response.Message = "KYC Verification is Failed";
            }
            else if (!string.IsNullOrEmpty(response?.Message))
            {
                var custRequest = new CustomerRequest
                {
                    RequestNo= customerUpdate.RequestNo,
                    LoggedInUserId  =   customerUpdate.LoggedInUserId
                };
                var custData = await Repository.GetCustomerData(custRequest);
                string htmlBody = string.Empty;
                var template = await _templateConfigurationservice.Get(9);
                htmlBody = template.FirstOrDefault().Body;
                if (customerUpdate.AddharVerificationStatus && customerUpdate.PanVerificationStatus)
                {
                    var templateconfig = await _templateConfigurationservice.Get(3);
                    htmlBody = templateconfig.FirstOrDefault().Body;
                    htmlBody = htmlBody.Replace("{{name}}", customerUpdate.RequesterName);
                    htmlBody = htmlBody.Replace("{{requestnumber}}", customerUpdate.RequestNo);
                    var certData = await _certificateService.GetCertificate(customerUpdate.RequestNo, Convert.ToBoolean(1));
                    StringBuilder tbody = new StringBuilder();
                    foreach (Certificate c in certData)
                    {
                        tbody.Append("<tr><td style='border: 1px solid #000000;text-align: center;'>" + c.DomainName + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.Certificates + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.ExpiredOn + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.ExpireDate + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.CreatedDate + "</td></tr>");

                    }
                    htmlBody = htmlBody.Replace("{{tablerows}}", tbody.ToString());

                }
                else if (customerUpdate.AddharVerificationStatus && !customerUpdate.PanVerificationStatus)
                {
                    string bodyMsg = (custData.RequestTypeId == 1) ? "This is to inform you that your Aadhar is valid but your Pan card is not valid." : "This is to inform you that your GST is valid but your Pan card is not valid.";
                    htmlBody = htmlBody.Replace("{{name}}", customerUpdate.RequesterName);
                    htmlBody = htmlBody.Replace("{{bodyMessage}}", bodyMsg);
                    htmlBody = htmlBody.Replace("{{requestnumber}}", customerUpdate.RequestNo);
                    htmlBody = htmlBody.Replace("{{comment}}", customerUpdate.Comments);
                }
                else if (!customerUpdate.AddharVerificationStatus && customerUpdate.PanVerificationStatus)
                {
                    string bodyMsg = (custData.RequestTypeId == 1) ? "This is to inform you that your Pan is valid but your Aadhar card is not valid." : "This is to inform you that your Pan is valid but your GST is not valid.";

                    htmlBody = htmlBody.Replace("{{name}}", customerUpdate.RequesterName);
                    htmlBody = htmlBody.Replace("{{bodyMessage}}", bodyMsg);
                    htmlBody = htmlBody.Replace("{{requestnumber}}", customerUpdate.RequestNo);
                    htmlBody = htmlBody.Replace("{{comment}}", customerUpdate.Comments);                    
                }
                else if (!customerUpdate.AddharVerificationStatus && !customerUpdate.PanVerificationStatus)
                {
                    string bodyMsg = (custData.RequestTypeId == 1) ? "This is to inform you that your Pan and Aadhar card is not valid." : "This is to inform you that your Pan and GST is not valid.";
                   
                    htmlBody = htmlBody.Replace("{{name}}", customerUpdate.RequesterName);
                    htmlBody = htmlBody.Replace("{{bodyMessage}}", bodyMsg);
                    htmlBody = htmlBody.Replace("{{requestnumber}}", customerUpdate.RequestNo);
                    htmlBody = htmlBody.Replace("{{comment}}", customerUpdate.Comments);
                   
                }
                var email = new Email
                {
                    FromAddess = template.FirstOrDefault().Sender,
                    ToAddress = customerUpdate.Email,
                    Subject = template.FirstOrDefault().Subject,
                    Body = htmlBody,
                };
                var emailResponse = await _emailService.Post(email);
            }

            return response;
        }

    }
}
