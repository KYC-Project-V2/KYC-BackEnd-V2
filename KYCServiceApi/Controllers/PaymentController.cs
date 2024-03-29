
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Model;
using Newtonsoft.Json;
using Service;
using System.Reflection;
using System.Web;
using Twilio.TwiML.Voice;
using Utility;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IService<Payment> _service;
        private readonly IService<PaymentConfiguration> _paymentCongurationService;
        private readonly IConfiguration _configuration;
        public PaymentController(
            IService<Payment> service, IService<PaymentConfiguration> paymentCongurationService, IConfiguration configuration)
        {
            _service = service;
            _paymentCongurationService = paymentCongurationService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                Payment payment = new Payment();
                var responseCode = HttpContext.Request.Form["Response Code"].ToString();
                var redirectUrl = _configuration.GetValue<string>("KYCWebUrl");
                //var redirectUrl = paymentConfiguration.AppRedirectUrl;
                var optionalFields = HttpContext.Request.Form["optional fields"].ToString();
                var requestNumber = optionalFields.Split('|')[0];
                var paymentConfiguration = await _paymentCongurationService.Get(true);
                var secreteKey = paymentConfiguration.AccessCode;

                if (!string.IsNullOrEmpty(responseCode) && responseCode != "E00335")
                {

                    payment.ResponseCode = responseCode;
                    payment.UniqueRefNumber = HttpContext.Request.Form["Unique Ref Number"].ToString();
                    payment.ServiceTaxAmount = HttpContext.Request.Form["Service Tax Amount"].ToString();
                    payment.ProcessingFeeAmount = HttpContext.Request.Form["Processing Fee Amount"].ToString();
                    payment.TotalAmount = HttpContext.Request.Form["Total Amount"].ToString();
                    payment.TransactionAmount = HttpContext.Request.Form["Transaction Amount"].ToString();

                    var transactionDate = HttpContext.Request.Form["Transaction Date"].ToString();
                    payment.TransactionDate = !string.IsNullOrEmpty(transactionDate) ? DateTime.ParseExact(transactionDate, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US")) : null;
                    payment.InterchangeValue = HttpContext.Request.Form["Interchange Value"].ToString();
                    payment.PaymentMode = HttpContext.Request.Form["Payment Mode"].ToString();
                    payment.SubMerchantId = HttpContext.Request.Form["SubMerchantId"].ToString();
                    payment.ReferenceNo = HttpContext.Request.Form["ReferenceNo"].ToString();
                    payment.ID = HttpContext.Request.Form["ID"].ToString();
                    payment.RS = HttpContext.Request.Form["RS"].ToString();
                    payment.TPS = HttpContext.Request.Form["TPS"].ToString();
                    payment.MandatoryFields = HttpContext.Request.Form["mandatory fields"].ToString();
                    payment.OptionalFields = optionalFields;
                    payment.RequestNumber = requestNumber;


                    payment = await _service.Post(payment);
                }

                //responseCode = KYCUtility.encrypt(responseCode, paymentConfiguration.AccessCode); E00335

                //redirectUrl = redirectUrl + $"?rc={responseCode}&rn=" + KYCUtility.encrypt(payment.RequestNumber, paymentConfiguration.AccessCode)
                //    + "&pid=" + KYCUtility.encrypt(response.PaymentId.ToString(), paymentConfiguration.AccessCode);

                requestNumber = KYCUtility.encrypt(requestNumber, secreteKey);
                requestNumber = HttpUtility.UrlEncode(requestNumber);
                string paymentId = null;

                if(payment.PaymentId != null && payment.PaymentId > 0)
                {
                    paymentId = KYCUtility.encrypt(payment.PaymentId.ToString(), secreteKey);
                    paymentId = HttpUtility.UrlEncode(paymentId);
                }

                redirectUrl = redirectUrl + $"/kycverification?rc={responseCode}&rn={requestNumber}&pid={paymentId}";


                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
        public async Task<IActionResult> Get([FromQuery] string code)
        {
            var response = await _service.Get(code);
            return Ok(response);
        }

        [HttpPost, Route("add")]
        public async Task<IActionResult> Add([FromQuery] string requestNumber, string gst, string tamt)
        {
            Random rnd = new Random();
            var payment = await _service.Get("1");
            payment.UniqueRefNumber = Convert.ToString(rnd.Next());
            payment.PaymentId = null;
            payment.RequestNumber = requestNumber;
            payment.ResponseCode = "E000";
            payment.TotalAmount = tamt;
            payment.TransactionAmount = tamt;
            var response = await _service.Post(payment);
            return Ok(response);
        }
    }
}
