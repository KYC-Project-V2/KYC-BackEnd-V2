using Microsoft.AspNetCore.Mvc;
using Utility;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HtmlToPDFController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> CreatePdfWithHtmlContent()
        {
            //var response = await KYCUtility.CreatePdfWithHtmlContent("");
            return Ok("");
        }
    }
}
