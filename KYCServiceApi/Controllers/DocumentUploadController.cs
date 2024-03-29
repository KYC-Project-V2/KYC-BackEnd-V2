using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using System.Net.Http.Headers;
using Utility;

namespace KYCServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentUploadController : BaseController
    {
        private readonly IService<AadharInfo> _AadharInfoService;
        public DocumentUploadController(
            IService<AadharInfo> AadharInfoService)
        {
            _AadharInfoService = AadharInfoService;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload")]
        public async Task<IActionResult> Upload(string requestNo, string documentType)
        {
            try
            {
                var documentDetail = new DocumentDetail();
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Document", requestNo);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)?.FileName?.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var filePath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    //TODO: add service 
                    //Need to add logic for valid document
                    switch (documentType)
                    {
                        case "Aadhar":
                            //documentDetail = await KYCUtility.GetAadharCardQRCode(fullPath);
                            documentDetail.OCRInformation = await KYCUtility.TessaractOCR(fullPath);
                            if (documentDetail.OCRInformation.Contains("Your Aadhaar No") || documentDetail.OCRInformation.Contains("Your Aadhaar Number") || documentDetail.OCRInformation.Contains("Enrolment No"))
                            {
                                documentDetail.IsVaidDocumentType = true;
                            }
                            documentDetail.FolderPath = filePath;
                            documentDetail.FullPath = fullPath;
                            documentDetail.RequestNo = requestNo;
                            break;
                        case "Voter":
                            documentDetail.OCRInformation = await KYCUtility.TessaractOCR(fullPath);
                            if (documentDetail.OCRInformation.Contains("ELECTION COMMISSION OF INDIA IDENTITY CARD") || documentDetail.OCRInformation.Contains("Elector’s N"))
                            {
                                documentDetail.IsVaidDocumentType = true;
                            }
                            documentDetail.FolderPath = filePath;
                            documentDetail.FullPath = fullPath;
                            documentDetail.RequestNo = requestNo;
                            break;
                        case "PanCard":
                            documentDetail.OCRInformation = await KYCUtility.TessaractOCR(fullPath);
                            if (documentDetail.OCRInformation.Contains("INCOME TAX DEPARTMENT") || documentDetail.OCRInformation.Contains("Permanent Account Number"))
                            {
                                documentDetail.IsVaidDocumentType = true;
                            }
                            documentDetail.FolderPath = filePath;
                            documentDetail.FullPath = fullPath;
                            documentDetail.RequestNo = requestNo;
                            break;
                        case "DrivingLicence":
                            documentDetail.OCRInformation = await KYCUtility.TessaractOCR(fullPath);
                            if (documentDetail.OCRInformation.Contains("DrivingLicence Number") || documentDetail.OCRInformation.Contains("Driving Licence Number"))
                            {
                                documentDetail.IsVaidDocumentType = true;
                            }
                            documentDetail.FolderPath = filePath;
                            documentDetail.FullPath = fullPath;
                            documentDetail.RequestNo = requestNo;
                            break;
                        case "BusinessPanCard":
                            documentDetail.OCRInformation = await KYCUtility.TessaractOCR(fullPath);
                            if (documentDetail.OCRInformation.Contains("BusinessPanCard") || documentDetail.OCRInformation.Contains("Business PanCard"))
                            {
                                documentDetail.IsVaidDocumentType = true;
                            }
                            documentDetail.FolderPath = filePath;
                            documentDetail.FullPath = fullPath;
                            documentDetail.RequestNo = requestNo;
                            break;
                        case "BusinessDocument":
                            documentDetail.OCRInformation = await KYCUtility.TessaractOCR(fullPath);
                            if (documentDetail.OCRInformation.Contains("BusinessDocument") || documentDetail.OCRInformation.Contains("Business Document"))
                            {
                                documentDetail.IsVaidDocumentType = true;
                            }
                            documentDetail.FolderPath = filePath;
                            documentDetail.FullPath = fullPath;
                            documentDetail.RequestNo = requestNo;
                            break;
                    }
                    return Ok(documentDetail);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
