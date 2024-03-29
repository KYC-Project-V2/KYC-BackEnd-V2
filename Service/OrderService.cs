using Humanizer;
using Model;
using Repository;
using System.Text;
using Utility;
using System.Security.Cryptography;

namespace Service
{
    public class OrderService : BaseService<Order>
    {
        private readonly IRepository<RequestOrigin> _requestOriginRepository;
        private readonly IRepository<PersonalInfo> _personalInfoRepository;
        private readonly IRepository<BusinessInfo> _businessInfoRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<State> _stateRepository;

        private readonly IRepository<AadharInfo> _aadharInfoRepository;
        private readonly IRepository<PanCardInfo> _panCardInfoRepository;
        private readonly IRepository<BusinessPanCardInfo> _businessPanCardInfoRepository;
        private readonly IRepository<BusinessInCorpInfo> _businessInCorpInfoRepository;
        private readonly IRepository<VoterInfo> _voterInfoRepository;
        private readonly IRepository<DriverLicenseInfo> _driverLicenseInfoRepository;


        private readonly IService<Email> _emailService;
        private readonly IService<TemplateConfiguration> _templateConfigurationservice;
        private readonly IService<Certificate> _certificateService;
        private readonly IService<Domain> _domainService;


        public OrderService(
            IRepository<RequestOrigin> requestOriginRepository,
            IRepository<PersonalInfo> personalInfoRepository,
            IRepository<BusinessInfo> businessInfoRepository,
            IRepository<Domain> domainRepository,
            IRepository<Payment> paymentRepository,
            IService<Email> emailService,
            IService<TemplateConfiguration> templateConfigurationservice,
            IRepository<AadharInfo> aadharInfoRepository,
            IRepository<PanCardInfo> panCardInfoRepository,
            IRepository<BusinessPanCardInfo> businessPanCardInfoRepository,
            IRepository<BusinessInCorpInfo> businessInCorpInfoRepository,
            IService<Certificate> certificateService,
            IRepository<State> stateRepository,
            IRepository<VoterInfo> voterInfoRepository,
            IRepository<DriverLicenseInfo> driverLicenseInfoRepository,
            IService<Domain> domainService)
        {
            _requestOriginRepository = requestOriginRepository;
            _personalInfoRepository = personalInfoRepository;
            _businessInfoRepository = businessInfoRepository;
            _domainRepository = domainRepository;
            _paymentRepository = paymentRepository;
            _aadharInfoRepository = aadharInfoRepository;
            _panCardInfoRepository = panCardInfoRepository;
            _businessPanCardInfoRepository = businessPanCardInfoRepository;
            _businessInCorpInfoRepository = businessInCorpInfoRepository;
            _emailService = emailService;
            _templateConfigurationservice = templateConfigurationservice;
            _certificateService = certificateService;
            _stateRepository = stateRepository;
            _voterInfoRepository = voterInfoRepository;
            _driverLicenseInfoRepository = driverLicenseInfoRepository;
            _domainService = domainService;
        }
        public override async Task<Order> Post(Order order)
        {
            //var requestOriginResponse = await _requestOriginRepository.Post(order.RequestOrigin);
            var name = "";
            var emailId = "";
            if (order != null && order.RequestOrigin != null)
            {
                if (order.RequestOrigin.RequestTypeId == 1)
                {
                    order.PersonalInfo.RequestTypeId = order.RequestOrigin.RequestTypeId;
                    order.PersonalInfo.RequestNo = order.RequestOrigin.RequestNo;
                    await _personalInfoRepository.Put(order.PersonalInfo);
                    name = order.PersonalInfo.FirstName + " " + order.PersonalInfo.LastName;
                    emailId = order.PersonalInfo.Email;
                }
                else if (order.RequestOrigin.RequestTypeId == 2)
                {
                    order.BusinessInfo.RequestTypeId = order.RequestOrigin.RequestTypeId;
                    order.BusinessInfo.RequestNo = order.RequestOrigin.RequestNo;
                    await _businessInfoRepository.Put(order.BusinessInfo);
                    name = order.BusinessInfo.ContactFirstName + " " + order.BusinessInfo.ContactLastName;
                    emailId = order.BusinessInfo.ContactEmail;
                }
            }

            var canPayment = order.Domains.Exists(item => item.PaymentId != order.Payment?.PaymentId);

            foreach (var domain in order.Domains)
            {
                domain.RequestNumber = order.RequestOrigin.RequestNo;
                if (canPayment && (domain.PaymentId == null || domain.PaymentId == 0))
                {
                    domain.PaymentId = order.Payment?.PaymentId;
                }
                await _domainRepository.Post(domain);
            }
            //TODO: We can remove if post method will return updated data
            order.Domains = await _domainRepository.GetAll(order.RequestOrigin.RequestNo);

            if (canPayment)
            {
                SentPaymentEmail(order, name);
                GenerateCertificate(order);

            }
            if (!string.IsNullOrEmpty(order.RequestOrigin.CheckList))
            {
                var lstCheckList = order.RequestOrigin.CheckList.Split(',').ToArray();

                foreach (var list in lstCheckList)
                {
                    switch (list)
                    {
                        case "PAN":
                            if (order.RequestOrigin.RequestTypeId == 1)
                            {
                                order.PanCardInfo = await _panCardInfoRepository.Get(order.RequestOrigin.RequestNo);
                            }
                            else
                            {
                                order.BusinessPanCardInfo = await _businessPanCardInfoRepository.Get(order.RequestOrigin.RequestNo);
                            }
                            break;

                        case "Aadhar":
                            order.AadharInfo = await _aadharInfoRepository.Get(order.RequestOrigin.RequestNo);
                            break;
                        case "GST":
                            order.BusinessInCorpInfo = await _businessInCorpInfoRepository.Get(order.RequestOrigin.RequestNo);
                            break;
                        case "Certificate Of Incorporation (COI)":
                            order.VoterInfo = await _voterInfoRepository.Get(order.RequestOrigin.RequestNo);
                            break;
                        case "Certificate Of Registration (COR)":
                            order.DriverLicenseInfo = await _driverLicenseInfoRepository.Get(order.RequestOrigin.RequestNo);
                            break;
                    }
                }
            }

            return order;
        }

        public override async Task<Order> Get(string requestNumber)
        {
            var order = new Order();
            order.RequestOrigin = await _requestOriginRepository.Get(requestNumber);

            order.PersonalInfo = await _personalInfoRepository.Get(requestNumber);
            order.BusinessInfo = await _businessInfoRepository.Get(requestNumber);

            order.Domains = await _domainRepository.GetAll(requestNumber);
            order.Payments = await _paymentRepository.GetAll(requestNumber);

            if (order.Payments != null && order.Payments.Any())
            {
                order.Payment = order.Payments.Last();
            }

            if (!string.IsNullOrEmpty(order.RequestOrigin.CheckList))
            {
                var lstCheckList = order.RequestOrigin.CheckList.Split(',').ToArray();

                foreach (var list in lstCheckList)
                {
                    switch (list)
                    {
                        case "PAN":
                            if (order.RequestOrigin.RequestTypeId == 1)
                            {
                                order.PanCardInfo = await _panCardInfoRepository.Get(requestNumber);
                            }
                            else
                            {
                                order.BusinessPanCardInfo = await _businessPanCardInfoRepository.Get(requestNumber);
                            }
                            break;

                        case "Aadhar":
                            order.AadharInfo = await _aadharInfoRepository.Get(requestNumber);
                            break;
                        case "GST":
                            order.BusinessInCorpInfo = await _businessInCorpInfoRepository.Get(requestNumber);
                            break;
                        case "Certificate Of Incorporation (COI)":
                            order.VoterInfo = await _voterInfoRepository.Get(requestNumber);
                            break;
                        case "Certificate Of Registration (COR)":
                            order.DriverLicenseInfo = await _driverLicenseInfoRepository.Get(requestNumber);
                            break;
                    }
                }
            }
            return order;
        }

        public override async Task<Order> Put(Order order)
        {
            var requestOriginResponse = await _requestOriginRepository.Post(order.RequestOrigin);
            order.RequestOrigin = requestOriginResponse;
            var templateconfig = await _templateConfigurationservice.Get(3);
            var htmlBody = templateconfig.FirstOrDefault().Body;
            if (order.RequestOrigin.RequestTypeId == 1)
            {
                htmlBody = htmlBody.Replace("{{name}}", order.PersonalInfo.FirstName);
            }
            else
            {
                htmlBody = htmlBody.Replace("{{name}}", order.BusinessInfo.ContactFirstName);
            }
            htmlBody = htmlBody.Replace("{{requestnumber}}", order.RequestOrigin.RequestNo);

            var response = await _certificateService.GetAll(order.RequestOrigin.RequestNo);

            StringBuilder tbody = new StringBuilder();

            foreach (Certificate c in response)
            {
                tbody.Append("<tr><td style='border: 1px solid #000000;text-align: center;'>" + c.DomainName + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.Certificates + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.ExpiredOn + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.ExpireDate + "</td><td style='border: 1px solid #000000;text-align: center;'>" + c.CreatedDate + "</td></tr>");

            }
            htmlBody = htmlBody.Replace("{{tablerows}}", tbody.ToString());


            var email = new Email
            {
                FromAddess = templateconfig.FirstOrDefault().Sender,
                ToAddress = order.RequestOrigin.RequestTypeId == 1 ? order.PersonalInfo.Email : order.BusinessInfo.ContactEmail,
                Subject = templateconfig.FirstOrDefault().Subject,
                Body = htmlBody,
            };
            var emailResponse = await _emailService.Post(email);
            return order;
        }

        private async void SentPaymentEmail(Order order,string name)
        {
            var payment = await _paymentRepository.Get(order.Payment?.PaymentId.ToString());
            payment.KycPrice = order.Payment.KycPrice;
            payment.KycGST = order.Payment.KycGST;
            order.Payment = payment;
            var templateconfigResponse = await _templateConfigurationservice.Get(1);
            var templateconfig = templateconfigResponse.FirstOrDefault();
            var emailBody = templateconfig.Body;

            emailBody = emailBody.Replace("{{name}}", name);
            emailBody = emailBody.Replace("{{requestnumber}}", order.RequestOrigin.RequestNo);
            emailBody = emailBody.Replace("{{uniqueRefNumber}}", payment.ReferenceNo);
            emailBody = emailBody.Replace("{{paymentMode}}", payment.PaymentMode);
            emailBody = emailBody.Replace("{{createdDate}}", payment.TransactionDate.ToString());
            emailBody = emailBody.Replace("{{domainTotalPrice}}", !string.IsNullOrEmpty(payment.TransactionAmount) ? payment.TransactionAmount : "0");
            emailBody = emailBody.Replace("{{serviceTaxAmount}}", !string.IsNullOrEmpty(payment.ServiceTaxAmount) && payment.ServiceTaxAmount != "null" ? payment.ServiceTaxAmount : "0");
            emailBody = emailBody.Replace("{{processingFeeAmount}}", !string.IsNullOrEmpty(payment.ProcessingFeeAmount) && payment.ServiceTaxAmount != "null" ? payment.ProcessingFeeAmount : "0");
            emailBody = emailBody.Replace("{{totalAmount}}", payment.TotalAmount);

            //----------Invoice--------------------//
            var invoiceTemplateconfigResponse = await _templateConfigurationservice.Get(6);
            var invoiceTemplateconfig = invoiceTemplateconfigResponse.FirstOrDefault();
            string invoiceFilePath = string.Empty;

            if (invoiceTemplateconfig != null)
            {
                State state = null;
                var states = await _stateRepository.Get();
                var invoiceBody = invoiceTemplateconfig?.Body;
                if (order.RequestOrigin.RequestTypeId == 1)
                {
                    invoiceBody = invoiceBody.Replace("{{Name}}", order.PersonalInfo.FirstName + " " + order.PersonalInfo.LastName);
                    invoiceBody = invoiceBody.Replace("{{Address}}", order.PersonalInfo.Address1);
                    invoiceBody = invoiceBody.Replace("{{Area}}", order.PersonalInfo.Address2);
                    invoiceBody = invoiceBody.Replace("{{City}}", order.PersonalInfo.City);
                    invoiceBody = invoiceBody.Replace("{{ZipCode}}", order.PersonalInfo.ZipCode);
                    state = states.FirstOrDefault(x => x.StateId == order.PersonalInfo.StateId);
                }
                else if (order.RequestOrigin.RequestTypeId == 2)
                {
                    invoiceBody = invoiceBody.Replace("{{Name}}", order.BusinessInfo.CompanyName);
                    invoiceBody = invoiceBody.Replace("{{Address}}", order.BusinessInfo.Address);
                    invoiceBody = invoiceBody.Replace("{{Area}}", order.BusinessInfo.NearBy);
                    invoiceBody = invoiceBody.Replace("{{City}}", order.BusinessInfo.City);
                    invoiceBody = invoiceBody.Replace("{{ZipCode}}", order.BusinessInfo.Zipcode);
                    state = states.FirstOrDefault(x => x.StateId == order.BusinessInfo.StateId);
                }

                invoiceBody = invoiceBody.Replace("{{State}}", state.StateName);
                invoiceBody = invoiceBody.Replace("{{StateCode}}", state.StateCode);
                invoiceBody = invoiceBody.Replace("{{StateAbbr}}", state.Abbreviation);

                var domain = order.Domains.FirstOrDefault();
                invoiceBody = invoiceBody.Replace("{{GstNumber}}", "");
                invoiceBody = invoiceBody.Replace("{{HSN}}", "");

                invoiceBody = invoiceBody.Replace("{{Particulors}}", domain.Name);
                invoiceBody = invoiceBody.Replace("{{Amount}}", order.Payment.KycPrice.ToString());
                invoiceBody = invoiceBody.Replace("{{GstAmount}}", order.Payment.KycGST.ToString());
                invoiceBody = invoiceBody.Replace("{{TotalAmount}}", payment.TotalAmount);

                invoiceBody = invoiceBody.Replace("{{InvoiceNumber}}", order.RequestOrigin.RequestNo);
                invoiceBody = invoiceBody.Replace("{{InvoiceDate}}", DateTime.Now.ToString("dd-MMM-yyyy"));

                invoiceBody = invoiceBody.Replace("{{CompanyName}}", "Astitva Communication Technologies Pvt Ltd");
                invoiceBody = invoiceBody.Replace("{{CompanyBuildingNumber}}", "Block E");
                invoiceBody = invoiceBody.Replace("{{CompanyArea}}", ", Kirti Nagar");
                invoiceBody = invoiceBody.Replace("{{CompanyState}}", " Delhi");
                invoiceBody = invoiceBody.Replace("{{CompanyZipCode}}", " 110015");
                invoiceBody = invoiceBody.Replace("{{CompanyStateCode}}", " 07");
                invoiceBody = invoiceBody.Replace("{{CompanyGSTNumber}}", "07AAOCS3972L1ZM");

                var totalAmount = Convert.ToInt32(Convert.ToDouble(payment.TotalAmount));
                invoiceBody = invoiceBody.Replace("{{TotalAmountInWord}}", "Indian Rupees " + totalAmount.ToWords().Titleize() + " Only");
                invoiceBody = invoiceBody.Replace("{{GstAmountInWord}}", "Indian Rupees " + order.Payment.KycGST.ToWords().Titleize() + " Only");

                invoiceFilePath = KYCUtility.CreatePdfWithHtmlContent(invoiceBody).Result;
            }
            //----------end of Invoice code--------//

            var email = new Email
            {
                ToAddress = order.RequestOrigin.RequestTypeId == 1 ? order.PersonalInfo.Email : order.BusinessInfo.ContactEmail,
                Subject = templateconfig.Subject,
                FromAddess = templateconfig.Sender,
                AttachmentPath = invoiceFilePath,
                Body = emailBody,

            };

            await _emailService.Post(email);
            //-----------Delete attached file-------//
            if (File.Exists(invoiceFilePath))
            {
                try
                {
                    File.Delete(invoiceFilePath);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async void GenerateCertificate(Order order)
        {
            var domains = await _domainService.GetAll(order.RequestOrigin.RequestNo);
            if (domains != null && domains.Any())
            {
                Certificate certificate;
                var expiryDate = DateTime.Now.AddYears(1);
                //TODO: Remove hardcode value 3
                var templateConfigurations = await _templateConfigurationservice.Get(3);
                var expiryTemplteConfig = templateConfigurations.FirstOrDefault();

                //var days = expiryTemplteConfig != null && expiryTemplteConfig.Days > 0 ?
                //    expiryTemplteConfig.Days - (expiryTemplteConfig.Days + expiryTemplteConfig.Days) : -30;

                //var nextEmailTiggeringDate = expiryDate.AddDays(Convert.ToInt32(days));
                foreach (var domain in domains)
                {
                    certificate = new Certificate();
                    certificate.DomainId = domain.Id;
                    certificate.DomainName = domain.Name;
                    certificate.RequestNumber = domain.RequestNumber;
                    certificate.CreatedDate = DateTime.Now;
                    certificate.Certificates = ComputeStringToSha256Hash(domain.Name);
                    certificate.ExpireDate = expiryDate;
                    certificate.CreatedBy = "";
                    //certificate.NextEmailTiggeringDate = nextEmailTiggeringDate;
                    await _certificateService.Post(certificate);
                }
            }
        }

        static string ComputeStringToSha256Hash(string plainText)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
    }
}
