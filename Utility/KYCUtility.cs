using Dynamsoft.DBR;
using Model;
using Model.DocumentQR;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Xml;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Collections;
using System.Numerics;
using System.IO.Compression;
using CCA.Util;
using System.Security.Cryptography;
using Tesseract;
using System.Drawing.Printing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Utility
{
    public static class KYCUtility
    {
        public static async Task<DocumentDetail> GetAadharCardQRCode(string path)
        {
            var response = new DocumentDetail();
            string errorMsg = string.Empty;
            BarcodeReader.InitLicense("t0068NQAAAJvpwWrQuuRVKhVH3D8xHOqLKoXXXGFuW6gTKBPu3t/0ntocNxJU12tl2MJMZa0FdEu68f63BA4zbbsaGxAPmMg=", out errorMsg);
            BarcodeReader reader = new();
            Dynamsoft.TextResult[] results;
            string xmldata = string.Empty;
            //ResponseViewModel responseViewModel = new();
            bool IsXML = false;
            CultureInfo customCulture = new CultureInfo("en-US", true);
            customCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;
            XMLResponse parcedxml = new();
            try
            {
                results = reader.DecodeFile(path, "");
                if (results != null && results[0].BarcodeFormat > 0)
                {
                    xmldata = results[0].BarcodeText;
                    parcedxml = XmlToJson(xmldata);
                    IsXML = parcedxml != null && parcedxml.PrintLetterBarcodeData != null ? true : false;
                }
                else
                {
                    errorMsg = "QR Code not recognized";
                }
                reader.Dispose();
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                reader.Dispose();
            }
            if (IsXML)
            {
                response.Id = parcedxml.PrintLetterBarcodeData.uid;
                response.Name = parcedxml.PrintLetterBarcodeData.name;
                response.DOB = DateTime.ParseExact("01/06/" + parcedxml.PrintLetterBarcodeData.yob, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                response.Sex = parcedxml.PrintLetterBarcodeData.gender;
                response.Address = parcedxml.PrintLetterBarcodeData.co + ", " + parcedxml.PrintLetterBarcodeData.house + ", " + parcedxml.PrintLetterBarcodeData.street + ", " + parcedxml.PrintLetterBarcodeData.lm + ", " + parcedxml.PrintLetterBarcodeData.loc + ", " + parcedxml.PrintLetterBarcodeData.vtc + ", " + parcedxml.PrintLetterBarcodeData.dist + ", " + parcedxml.PrintLetterBarcodeData.state + ", " + parcedxml.PrintLetterBarcodeData.pc;
            }
            else
            {
                BigInteger positiveValue = BigInteger.Parse(xmldata);


                byte[] array = positiveValue.ToByteArray();

                var bigarray = new System.Numerics.BigInteger(array).ToByteArray();
                byte[] msgInBytes = null;
                System.Array.Reverse(bigarray);
                msgInBytes = Decompress(bigarray);
                int[] delimiters = locateDelimiters(msgInBytes);
                var details = new Hashtable();
                string referenceId = getValueInRange(msgInBytes, delimiters[1] + 1, delimiters[2]);
                string name = getValueInRange(msgInBytes, delimiters[2] + 1, delimiters[3]);
                string yob = getValueInRange(msgInBytes, delimiters[3] + 1, delimiters[4]);
                string gender = getValueInRange(msgInBytes, delimiters[4] + 1, delimiters[5]);
                string co = getValueInRange(msgInBytes, delimiters[5] + 1, delimiters[6]);
                string dist = getValueInRange(msgInBytes, delimiters[6] + 1, delimiters[7]);
                string lm = getValueInRange(msgInBytes, delimiters[7] + 1, delimiters[8]);
                string house = getValueInRange(msgInBytes, delimiters[8] + 1, delimiters[9]);
                string loc = getValueInRange(msgInBytes, delimiters[9] + 1, delimiters[10]);
                string pc = getValueInRange(msgInBytes, delimiters[10] + 1, delimiters[11]);
                string po = getValueInRange(msgInBytes, delimiters[11] + 1, delimiters[12]);
                string state = getValueInRange(msgInBytes, delimiters[12] + 1, delimiters[13]);
                string street = getValueInRange(msgInBytes, delimiters[13] + 1, delimiters[14]);
                string subdist = getValueInRange(msgInBytes, delimiters[14] + 1, delimiters[15]);
                string mobile = getValueInRange(msgInBytes, delimiters[16] + 1, delimiters[17]);

                details.Add("referanceid", referenceId.Substring(0, 4));
                details.Add("name", name);
                details.Add("yob", yob);
                details.Add("gender", gender);
                details.Add("co", co);
                details.Add("dist", dist);
                details.Add("lm", lm);
                details.Add("house", house);
                details.Add("loc", loc);
                details.Add("pc", pc);
                details.Add("po", po);
                details.Add("state", state);
                details.Add("street", street);
                details.Add("subdist", subdist);
                details.Add("mob", mobile);

                response.Id = referenceId.Substring(0, 4);
                response.Name = name;
                response.DOB = DateTime.ParseExact(yob.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(yob);
                response.Sex = gender;
                response.Address = co + ", " + house + ", " + street + ", " + lm + ", " + loc + ", " + dist + ", " + state + ", " + pc;
            }

            return response;
        }
        private static String getValueInRange(byte[] msgInBytes, int start, int end)
        {
            msgInBytes = msgInBytes[start..end];
            byte[] converted = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"),
                    Encoding.UTF8, msgInBytes);
            return System.Text.Encoding.UTF8.GetString(converted);
        }
        private static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
        

       
        public static XMLResponse XmlToJson(string xml)
        {
            XMLResponse xmlResponse = new();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                var jsonText = JsonConvert.SerializeXmlNode(doc);
                xmlResponse = JsonConvert.DeserializeObject<XMLResponse>(jsonText);
            }
            catch (Exception ex)
            {

            }
            return xmlResponse;
        }
        private static int[] locateDelimiters(byte[] msgInBytes)
        {
            int NUMBER_OF_PARAMS_IN_SECURE_QR_CODE = 30;
            int[] delimiters = new int[NUMBER_OF_PARAMS_IN_SECURE_QR_CODE + 1];
            int index = 0;
            int delimiterIndex;
            for (int i = 0; i <= NUMBER_OF_PARAMS_IN_SECURE_QR_CODE; i++)
            {
                delimiterIndex = getNextDelimiterIndex(msgInBytes, index);
                delimiters[i] = delimiterIndex;
                index = delimiterIndex + 1;
            }
            return delimiters;
        }
        private static int getNextDelimiterIndex(byte[] msgInBytes, int index)
        {
            int i = index;
            for (; i < msgInBytes.Length; i++)
            {
                if (msgInBytes[i] == 255)
                {
                    break;
                }
            }
            return i;
        }
        public static async Task<DocumentDetail> GetOCRCode(string path)
        {
            var response = new DocumentDetail();
            try
            {
                Dynamsoft.OCR.Tesseract ocr = new Dynamsoft.OCR.Tesseract("t0085oQAAAIZYCmoNPvAcd0XOOWTkG9ibNXjW/E/yu9oeseopqeDV6J1B4Vo5SGi0dLvBCp5jeMahy8AEoF1fhJNLyrHm9Zi/TXI/+DBaJoWePfIEF3Af2w==");
                // Specify language folder
                ocr.TessDataPath = Path.Combine(Directory.GetCurrentDirectory(), "OCRTessdata");
                // OCR in English
                ocr.Language = "eng";
                // Get the result
                ocr.ResultFormat = 0; // Save as text
                string[] Filepath = { path };//Kalyani_Pan.JPG //PanCard-2 //Kalyani_VoterID.JPG
                byte[] resultbyte = ocr.Recognize(Filepath);
                response.OCRInformation = Encoding.ASCII.GetString(resultbyte, 0, resultbyte.Length);
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public static async Task<OTPVerification> GenerateOTP(OTPVerification otp)
        {
            Random generator = new Random();
            int random = generator.Next(1, 1000000);
            otp.OtpNumber = random.ToString().PadLeft(6, '0');
            return otp;
        }

        public static async Task<Email> SendMail(Email model)
        {
            var fromaddress = model != null && !string.IsNullOrEmpty(model.FromAddess)
                ? model.FromAddess : model.emailConfiguration.Sender;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromaddress);
            message.To.Add(new MailAddress(model.ToAddress));
            message.Subject = model.Subject;
            message.IsBodyHtml = Constants.IsBodyHtml;
            message.Body = model.Body;
            Attachment attachment=null;
            try
            {
                if (!string.IsNullOrEmpty(model.AttachmentPath))
                {
                    attachment = new Attachment(model.AttachmentPath);
                    message.Attachments.Add(attachment);
                }
                using (SmtpClient smtp = new SmtpClient(model.emailConfiguration.Server, model.emailConfiguration.Port))
                {
                    smtp.UseDefaultCredentials = Constants.UseDefaultCredentials;
                    smtp.Credentials = new NetworkCredential(model.emailConfiguration.UserName, model.emailConfiguration.Password);
                    smtp.EnableSsl = Constants.EnableSsl;
                    smtp.Send(message);
                }
            }
            catch
            {
                //Handle or log exception
            }
            finally
            {
                if (attachment != null) attachment.Dispose();
            }
            return model;
        }

        public static async Task<SMSDetails> SendSMS(SMSDetails model)
        {
            var accountSid = "ACd7a1c105e61e73fbc81a5b5467ac1de5";
            var authToken = "14aa4e229c77c009bee33dd321e0824f";
            TwilioClient.Init(accountSid, authToken);
            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("+91" + model.Phone));
            messageOptions.From = new PhoneNumber("+13614597169");
            messageOptions.Body = model.Body;
            var message = MessageResource.Create(messageOptions);
            return model;
        }

        public static async Task<CCAvenue> CCAEncrypt(CCAvenue model, PaymentConfiguration paymentConfiguration)
        {
            var dateToUse = DateTime.Now;
            var referenceId = new DateTimeOffset(dateToUse).ToUnixTimeMilliseconds();
            string accesscode = paymentConfiguration.AccessCode;

            string mandatoryFields = encrypt($"{referenceId}|45|{model.Amount}", accesscode);
            string optionalFields = encrypt($"{model.RequestNumber}|{model.Phone}|{model.Email}", accesscode);
            string encreferenceId = encrypt(referenceId.ToString(), accesscode);
            string encsubmerchantId = encrypt("45", accesscode);
            string transactionAmount = encrypt(model.Amount.ToString(), accesscode);
            string paymentreturnUrl = encrypt(paymentConfiguration.PaymentReturnUrl, accesscode);
            string paymode = encrypt("9", accesscode);
            string merchantId = paymentConfiguration.MerchantId;
            model.PaymentUrl = paymentConfiguration.PaymentBaseUrl + $"?merchantid={merchantId}&mandatory fields={mandatoryFields}&optional fields={optionalFields}&returnurl={paymentreturnUrl}&Reference No={encreferenceId}&submerchantid={encsubmerchantId}&transaction amount={transactionAmount}&paymode={paymode}";
            return model;
        }
        public static string CCADecrypt(string encyptedText)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            var workingKey = "536098CA80266C0B0644456CA467A128";
            var strEncRequest = ccaCrypto.Decrypt(encyptedText, workingKey);
            return strEncRequest;
        }
        public static string encrypt(string textToEncrypt, string key)
        {

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.ECB;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
        public static string decrypt(string encryptedText, string key)
        {

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndael.Key = keyBytes;
                rijndael.Mode = CipherMode.ECB; // Use Electronic Codebook mode.
                rijndael.Padding = PaddingMode.PKCS7; // Use PKCS7 padding.

                using (MemoryStream ms = new MemoryStream(encryptedBytes))
                {
                    using (ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, null))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cs))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        public static async Task<string> TessaractOCR(string filePath)
        {
            // Initialize Tesseract engine
            var tessdataPath = Path.Combine(Directory.GetCurrentDirectory(), "OCRTessdata");
            using (var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default))
            {
                // Load the image
                using (var img = Pix.LoadFromFile(filePath))
                {
                    // Perform OCR on the image
                    using (var page = engine.Process(img))
                    {
                        // Get the recognized text
                        var text = page.GetText();

                        // Extract key-value pairs from the recognized text
                        var keyValuePairs = ExtractKeyValuePairs(text);
                        StringBuilder response = new StringBuilder();
                        foreach (var kvp in keyValuePairs)
                        {
                            response.Append($"{kvp.Key}: {kvp.Value}");
                        }
                        return response.ToString();
                    }
                }
            }
        }

        private static Dictionary<string, string> ExtractKeyValuePairs(string text)
        {
            var keyValuePairs = new Dictionary<string, string>();

            // Split the text into lines
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Find the column boundaries based on the maximum column width
            int maxColumnWidth = lines.Max(line => line.Length);
            int columnCount = 2; // Assuming 2 columns: Key and Value
            int columnWidth = maxColumnWidth / columnCount;

            foreach (var line in lines)
            {
                // Split the line into columns based on the calculated width
                var columns = SplitLineIntoColumns(line, columnWidth);

                if (columns.Length >= columnCount)
                {
                    var key = columns[0].Trim();
                    var value = columns[1].Trim();

                    // Add the key-value pair to the dictionary
                    keyValuePairs[key] = value;
                }
            }

            return keyValuePairs;
        }

        private static string[] SplitLineIntoColumns(string line, int columnWidth)
        {
            var columns = new List<string>();
            int startIndex = 0;

            while (startIndex < line.Length)
            {
                int endIndex = Math.Min(startIndex + columnWidth, line.Length - 1);
                string column = line.Substring(startIndex, endIndex - startIndex + 1).Trim();
                columns.Add(column);
                startIndex += columnWidth;
            }

            return columns.ToArray();
        }
        public static async Task<string> CreatePdfWithHtmlContent(string htmlContent)
        {
            // HTML content to be converted to PDF
           System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
           
            // Create a new MemoryStream to hold the PDF content
            MemoryStream memoryStream = new MemoryStream();

            // Create a Document and PdfWriter
            Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            // Open the document for writing
            document.Open();

            // Convert HTML to PDF
            using (TextReader htmlReader = new StringReader(htmlContent))
            {
                try
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, htmlReader);
                }
                catch (Exception ex)
                {

                }
            }

            // Close the document
            document.Close();

            // Set the content type and return the PDF as a FileResult
            byte[] pdfBytes = memoryStream.ToArray();

            var dateToUse = DateTime.Now;
            var invId = new DateTimeOffset(dateToUse).ToUnixTimeSeconds().ToString();

            // Save the PDF to the root folder
            var folderName = Path.Combine("Document");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            string filePath = Path.Combine(pathToSave, "Invoice_" + invId + ".pdf");
            File.WriteAllBytes(filePath, memoryStream.ToArray());
            memoryStream.Close();

            return filePath;
        }
        public static async Task<string> CreatePdfWithHtmlContentAPIDownload(string htmlContent)
        {
            // HTML content to be converted to PDF
            //htmlContent = "<html><body><div>Hello, World! This is a PDF created using iTextSharp.</div></body></html>";

            // File path where the PDF will be saved
            var folderName = Path.Combine("Document");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var dateToUse = DateTime.Now;
            var apiId = new DateTimeOffset(dateToUse).ToUnixTimeSeconds().ToString();
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            string filePath = Path.Combine(pathToSave, "APIDownload_"+ apiId + ".pdf");

            // Create a MemoryStream to hold the PDF content
            using (MemoryStream outputStream = new MemoryStream())
            {
                // Create a Document
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, outputStream);

                // Open the document
                document.Open();

                // Create an XMLWorkerHelper instance
                XMLWorkerHelper worker = XMLWorkerHelper.GetInstance();

                // Parse the HTML content and write it to the PDF document
                using (StringReader htmlReader = new StringReader(htmlContent))
                {
                    worker.ParseXHtml(writer, document, htmlReader);
                }

                // Close the document
                document.Close();

                // Save the PDF content to the file
                File.WriteAllBytes(filePath, outputStream.ToArray());
            }

            return filePath;
        }

        public static string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string alphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(alphanumericChars.Length);
                sb.Append(alphanumericChars[index]);
            }

            return sb.ToString();
        }
        public static byte[] GetX509CACertificate()
        {
            // Create a root CA certificate
            X509Certificate2 caCertificate = CreateCACertificate("cn=Asstitvatech.com.com ,O=Asstitvatech, L=Delhi, S=Delhi, C=IN");
            byte[] certBytes = caCertificate.Export(X509ContentType.Pfx);
            return certBytes;
        }
        public static Model.X509Certificate GetX509Certificate(Model.X509Certificate certificate)
        {
            byte[] byteArray = Convert.FromBase64String(certificate.CARootPath);

            var caCertificate = new X509Certificate2(byteArray);

            string serialnumber = certificate.RequestNumber.Replace("KYC-", "").Replace("-", "");
            // Generate a new serial number for the certificate
            byte[] serialNumberBytes = Encoding.UTF8.GetBytes(serialnumber);
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(serialNumberBytes);
            }
            var subjectName = "CN = " + certificate.DomainName + " ,C = IN";
            // Create a personal certificate signed by the CA
            X509Certificate2 personalCertificate = CreatePersonalCertificate(subjectName, caCertificate, certificate.IsProvisional, serialNumberBytes);
            certificate.SerialNumber = personalCertificate.SerialNumber;
            certificate.CertificateBytes = personalCertificate.Export(X509ContentType.Cert);
            return certificate;
        }
        public static byte[] GetX509ExternalCertificate(string subjectName)
        {
            DateTime validFrom = DateTime.Now.AddDays(0);
            DateTime validTo = validFrom.AddMonths(1);

            // Generate a new RSA key pair
            using (RSA rsa = RSA.Create(2048))
            {
                // Create a certificate request
                CertificateRequest request = new CertificateRequest("CN=" + subjectName + " ,C = IN", rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

                // Set certificate extensions for security
                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, true, 0, true));
                request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign, false));

                // Create and sign the certificate
                X509Certificate2 certificate = request.CreateSelfSigned(validFrom, validTo);

                // Save the certificate to a .pfx file
                //string filePath = (System.IO.Directory.GetCurrentDirectory().Replace("bin\\Debug\\net6.0-windows", "") + @"\astitva1.pfx");
                string password = "tomorrow@123"; // Password to protect the .pfx file
                byte[] pfxBytes = certificate.Export(X509ContentType.Pfx, password);
                return pfxBytes;
            }
        }
        static X509Certificate2 CreateCACertificate(string subjectName)
        {
            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(subjectName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
                request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

                var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(24));
                return new X509Certificate2(certificate.Export(X509ContentType.Pfx), "", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
        }

        static X509Certificate2 CreatePersonalCertificate(string subjectName, X509Certificate2 caCertificate, bool isProvisional,byte[] serialNumberBytes)
        {
            
            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(subjectName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
                request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

                var certificate = request.Create(caCertificate,DateTimeOffset.UtcNow.AddDays(0), isProvisional ?  DateTimeOffset.UtcNow.AddMonths(1) : DateTimeOffset.UtcNow.AddYears(1), serialNumberBytes);
                return new X509Certificate2(certificate.Export(X509ContentType.Pfx), "", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
        }
       
    }
}