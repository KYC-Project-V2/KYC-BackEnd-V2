using System.IO;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
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
            var response = await KYCUtility.CreatePdfWithHtmlContent("");
            return Ok(response);
        }
    }
}
