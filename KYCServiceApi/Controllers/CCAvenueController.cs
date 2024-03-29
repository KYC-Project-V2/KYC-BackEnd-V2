using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Utility;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CCAvenueController : BaseController
    {
        private readonly IService<CCAvenue> _service;
        private readonly IService<CCAvenueResponse> _ccAvenueservice;
        private readonly IConfiguration _configuration;
        public CCAvenueController(
            IService<CCAvenue> service, IService<CCAvenueResponse> ccAvenueservice, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
            _ccAvenueservice = ccAvenueservice;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CCAvenue model)
        {
            var response = await _service.Post(model);
            return Ok(response);
        }
        [HttpPost]
        [Route("CCAResponse")]
        public async Task<IActionResult> CCAResponse()
        {
            var redirectUrl = _configuration.GetValue<string>("KYCWebUrl");

            var decryptedText = KYCUtility.CCADecrypt(HttpContext.Request.Form["encResp"].ToString());

            try
            {
                if (decryptedText?.Split('&')[3].Split('=')[0] == "order_status" && decryptedText?.Split('&')[3].Split('=')[1] == "Success")
                {

                    CCAvenueResponse ccAvenue = new CCAvenueResponse();
                    ccAvenue.RequestNumber = decryptedText?.Split('&')[0].Split('=')[1];
                    ccAvenue.TrackingId = decryptedText?.Split('&')[1].Split('=')[1];
                    ccAvenue.BankRefno = decryptedText?.Split('&')[2].Split('=')[1];
                    ccAvenue.OrderStatus = decryptedText?.Split('&')[3].Split('=')[1];
                    ccAvenue.FailureMessage = decryptedText?.Split('&')[4].Split('=')[1];
                    ccAvenue.PaymentMode = decryptedText?.Split('&')[5].Split('=')[1];
                    ccAvenue.CardName = decryptedText?.Split('&')[6].Split('=')[1];
                    ccAvenue.StatusCode = decryptedText?.Split('&')[7].Split('=')[1];
                    ccAvenue.StatusMessage = decryptedText?.Split('&')[8].Split('=')[1];
                    ccAvenue.Currency = decryptedText?.Split('&')[9].Split('=')[1];
                    ccAvenue.Amount = decryptedText?.Split('&')[10].Split('=')[1];
                    ccAvenue.BillingName = decryptedText?.Split('&')[11].Split('=')[1];
                    ccAvenue.Vault = decryptedText?.Split('&')[31].Split('=')[1];
                    ccAvenue.OfferType = decryptedText?.Split('&')[32].Split('=')[1];
                    ccAvenue.OfferCode = decryptedText?.Split('&')[33].Split('=')[1];
                    ccAvenue.DiscountValue = decryptedText?.Split('&')[34].Split('=')[1];
                    ccAvenue.MerAmount = decryptedText?.Split('&')[35].Split('=')[1];
                    ccAvenue.EciValue = decryptedText?.Split('&')[36].Split('=')[1];
                    ccAvenue.Retry = decryptedText?.Split('&')[37].Split('=')[1];
                    ccAvenue.ResponseCode = decryptedText?.Split('&')[38].Split('=')[1];
                    ccAvenue.TransDate = decryptedText?.Split('&')[40].Split('=')[1];
                    ccAvenue.BinCountry = decryptedText?.Split('&')[41].Split('=')[1];
                    var response = _ccAvenueservice.Post(ccAvenue).Result;
                    redirectUrl = redirectUrl + "?rn=" + decryptedText;
                }
            }
            catch(Exception ex)
            {

            }    
            return Redirect(redirectUrl);
        }
    }
}
