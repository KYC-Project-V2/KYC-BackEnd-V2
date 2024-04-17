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
                var templateconfig = await _templateConfigurationservice.Get(3);
                var htmlBody = templateconfig.FirstOrDefault().Body;

                if (customerUpdate.AddharVerificationStatus && customerUpdate.PanVerificationStatus)
                {
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
                    var template = @"<?xml version='1.0'?> <html><body><div style='margin-top: 10px;border-radius: 5px; font-size: large; align-items: center;display: flex; flex-direction: column; box-sizing: border-box;padding: 0.5rem 0.5rem; background-color: #F47216; color: #FFFFFF;'>KYC Verification</div>  
                                    <div style='margin-top: 25px;font-size: large; margin-left: 30px; margin-right: 30px;'> <div>Hello {{name}}</div>  
                                    <div style='margin-top:20px'>This is to inform you that your Aadhar is valid but your Pan card is not valid.<br/>So, Please read the comments below for more information.</div>  
                                    <div style='margin-top:10px;margin-bottom:25px'>KYC Request Number : <b>{{requestnumber}}</b> </div>  <div style='margin-top:100px; '> 
                                    <div style='margin-top:10px;margin-bottom:25px'> Comments :<b>{{comment}}</b> </div> <div style='margin-top:30px;'>Thanks, </div> <div>Astitva</div> </div> </body> </html>";
                    template = template.Replace("{{name}}", customerUpdate.RequesterName);
                    template = template.Replace("{{requestnumber}}", customerUpdate.RequestNo);
                    template = template.Replace("{{comment}}", customerUpdate.Comments);
                    templateconfig[0].Body = template;
                    htmlBody = templateconfig[0].Body;
                }
                else if (!customerUpdate.AddharVerificationStatus && customerUpdate.PanVerificationStatus)
                {
                    var template = @"<?xml version='1.0'?> <html><body><div style='margin-top: 10px;border-radius: 5px; font-size: large; align-items: center;display: flex; flex-direction: column; box-sizing: border-box;padding: 0.5rem 0.5rem; background-color: #F47216; color: #FFFFFF;'>KYC Verification</div>  
                                    <div style='margin-top: 25px;font-size: large; margin-left: 30px; margin-right: 30px;'> <div>Hello {{name}}</div>  
                                    <div style='margin-top:20px'>This is to inform you that your Pan is valid but your Aadhar card is not valid.<br/>So, Please read the comments below for more information.</div>  
                                    <div style='margin-top:10px;margin-bottom:25px'>KYC Request Number : <b>{{requestnumber}}</b> </div>  <div style='margin-top:100px; '> 
                                    <div style='margin-top:10px;margin-bottom:25px'> Comments :<b>{{comment}}</b> </div> <div style='margin-top:30px;'>Thanks, </div> <div>Astitva</div> </div> </body> </html>";
                    template = template.Replace("{{name}}", customerUpdate.RequesterName);
                    template = template.Replace("{{requestnumber}}", customerUpdate.RequestNo);
                    template = template.Replace("{{comment}}", customerUpdate.Comments);
                    templateconfig[0].Body = template;
                    htmlBody = templateconfig[0].Body;
                }
                else if (!customerUpdate.AddharVerificationStatus && !customerUpdate.PanVerificationStatus)
                {
                    var template = @"<?xml version='1.0'?> <html><body><div style='margin-top: 10px;border-radius: 5px; font-size: large; align-items: center;display: flex; flex-direction: column; box-sizing: border-box;padding: 0.5rem 0.5rem; background-color: #F47216; color: #FFFFFF;'>KYC Verification</div>  
                                    <div style='margin-top: 25px;font-size: large; margin-left: 30px; margin-right: 30px;'> <div>Hello {{name}}</div>  
                                    <div style='margin-top:20px'>This is to inform you that your Pan and Aadhar card is not valid.<br/>So, Please read the comments below for more information.</div>  
                                    <div style='margin-top:10px;margin-bottom:25px'>KYC Request Number : <b>{{requestnumber}}</b> </div>  <div style='margin-top:100px; '> 
                                    <div style='margin-top:10px;margin-bottom:25px'> Comments :<b>{{comment}}</b> </div> <div style='margin-top:30px;'>Thanks, </div> <div>Astitva</div> </div> </body> </html>";
                    template = template.Replace("{{name}}", customerUpdate.RequesterName);
                    template = template.Replace("{{requestnumber}}", customerUpdate.RequestNo);
                    template = template.Replace("{{comment}}", customerUpdate.Comments);
                    templateconfig[0].Body = template;
                    htmlBody = templateconfig[0].Body;
                }
                var email = new Email
                {
                    FromAddess = templateconfig.FirstOrDefault().Sender,
                    ToAddress = customerUpdate.Email,
                    Subject = templateconfig.FirstOrDefault().Subject,
                    Body = htmlBody,
                };
                var emailResponse = await _emailService.Post(email);
            }

            return response;
        }

    }
}
