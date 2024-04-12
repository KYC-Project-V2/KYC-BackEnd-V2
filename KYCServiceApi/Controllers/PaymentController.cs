
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
        public async Task<IActionResult> Post([FromBody] Payment bypasspayment)
        {
            try
            {
                Payment payment = new Payment();
                var responseCode = bypasspayment == null ? HttpContext.Request.Form["Response Code"].ToString() : bypasspayment.ResponseCode;
                var redirectUrl = _configuration.GetValue<string>("KYCWebUrl");
                //var redirectUrl = paymentConfiguration.AppRedirectUrl;
                var optionalFields = bypasspayment == null ? HttpContext.Request.Form["optional fields"].ToString() : bypasspayment.OptionalFields;
                var requestNumber = optionalFields.Split('|')[0];
                var paymentConfiguration = await _paymentCongurationService.Get(true);
                var secreteKey = paymentConfiguration.AccessCode;

                if (!string.IsNullOrEmpty(responseCode) && responseCode != "E00335")
                {

                    payment.ResponseCode = responseCode;
                    payment.UniqueRefNumber = bypasspayment == null ? HttpContext.Request.Form["Unique Ref Number"].ToString() : bypasspayment.UniqueRefNumber;
                    payment.ServiceTaxAmount = bypasspayment == null ? HttpContext.Request.Form["Service Tax Amount"].ToString() : bypasspayment.ServiceTaxAmount;
                    payment.ProcessingFeeAmount = bypasspayment == null ? HttpContext.Request.Form["Processing Fee Amount"].ToString() : bypasspayment.ProcessingFeeAmount;
                    payment.TotalAmount = bypasspayment == null ? HttpContext.Request.Form["Total Amount"].ToString() : bypasspayment.TotalAmount;
                    payment.TransactionAmount = bypasspayment == null ? HttpContext.Request.Form["Transaction Amount"].ToString() : bypasspayment.TransactionAmount;

                    var transactionDate = bypasspayment == null ? HttpContext.Request.Form["Transaction Date"].ToString() : bypasspayment.TransactionDate.ToString();
                    try
                    {
                        payment.TransactionDate = !string.IsNullOrEmpty(transactionDate) ? DateTime.ParseExact(transactionDate, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US")) : null;
                    } catch{}
                    payment.InterchangeValue = bypasspayment == null ? HttpContext.Request.Form["Interchange Value"].ToString() : bypasspayment.InterchangeValue;
                    payment.PaymentMode = bypasspayment == null ? HttpContext.Request.Form["Payment Mode"].ToString() : bypasspayment.PaymentMode;
                    payment.SubMerchantId = bypasspayment == null ? HttpContext.Request.Form["SubMerchantId"].ToString() : bypasspayment.SubMerchantId;
                    payment.ReferenceNo = bypasspayment == null ? HttpContext.Request.Form["ReferenceNo"].ToString() : bypasspayment.ReferenceNo;
                    payment.ID = bypasspayment == null ? HttpContext.Request.Form["ID"].ToString() : bypasspayment.ID;
                    payment.RS = bypasspayment == null ? HttpContext.Request.Form["RS"].ToString() : bypasspayment.RS;
                    payment.TPS = bypasspayment == null ? HttpContext.Request.Form["TPS"].ToString() : bypasspayment.TPS;
                    payment.MandatoryFields = bypasspayment == null ? HttpContext.Request.Form["mandatory fields"].ToString() : bypasspayment.MandatoryFields;
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

                if (bypasspayment is null)
                   return Redirect(redirectUrl);
                else
                    return Ok(redirectUrl);
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
