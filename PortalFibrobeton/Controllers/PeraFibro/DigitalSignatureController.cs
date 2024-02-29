using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Security;
using PortalFibrobeton.Models.Class.DigitalSignature;

namespace PortalFibrobeton.Controllers.PeraFibro
{
    public class DigitalSignatureController : Controller
    {
        // GET: DigitalSignature
        public ActionResult Index()
        {
            return View();
        }

        public static class CertificateData
        {
            public static List<CertificateViewModel> Certificates { get; set; } = new List<CertificateViewModel>();
        }

        // Yerel uygulamadan gelen sertifika bilgilerini alacak bir API endpoint'i
        [HttpPost]
        public JsonResult ReceiveCertificates(List<CertificateViewModel> certificates)
        {
            // Burada sertifika bilgilerini saklayın (örneğin bir oturum değişkeninde)
            CertificateData.Certificates = certificates;
            return Json(new { success = true, message = "Sertifikalar alındı." });
        }

        [HttpGet]
        public JsonResult GetCertificates()
        {
            return Json(CertificateData.Certificates, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListCertificate()
        {

            var certificates = Session["Certificates"] as List<CertificateViewModel>;

            if (certificates == null)
            {
                // Eğer sertifika bilgileri yoksa, bir hata mesajı döndürün
                return Json(new { success = false, message = "Sertifika bilgileri bulunamadı." }, JsonRequestBehavior.AllowGet);
            }

            return Json(certificates, JsonRequestBehavior.AllowGet);
        }

        //private X509Certificate2 FindCertificateByThumbPrint(string thumbPrint)
        //{
        //    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    store.Open(OpenFlags.ReadOnly);
        //    var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, false);
        //    store.Close();
        //    if (certs.Count > 0)
        //    {
        //        return certs[0];
        //    }
        //    return null;
        //}

        [HttpGet]
        public ActionResult UploadDocuments()
        {
            //X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadOnly);

            //var model = store.Certificates
            //    .Cast<X509Certificate2>()
            //    .Select(cert => new CertificateViewModel
            //    {
            //        Subject = cert.Subject,
            //        Thumbprint = cert.Thumbprint
            //    }).ToList();

            //store.Close();
            //return View(model);

            var certificates = Session["Certificates"] as List<CertificateViewModel>;
            if (certificates == null)
            {
                certificates = new List<CertificateViewModel>(); // Boş bir liste oluşturun veya hata mesajı yerine bir varsayılan değer ekleyebilirsiniz.
            }

            // Sertifika bilgilerini View'e gönderin
            return View(certificates.ToList());

        }


        static async Task SendThumbprintAndFileToApi(string thumbprint, string filePath)
        {
            using (var client = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                var thumbprintContent = new StringContent(thumbprint, Encoding.UTF8, "application/json");
                content.Add(thumbprintContent, "thumbprint");

                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                content.Add(fileContent, "file", Path.GetFileName(filePath));

                var response = await client.PostAsync("http://localhost:57525/DigitalSignature/SignAllPdf", content);

                // ...
            }
        }

        private X509Certificate2 FindCertificateByThumbprint(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            //Thumbprint ile sertifikayı bul
            var certificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            store.Close();

            //Eğer sertifika bulunursa, ilk sertifikayı döndür
            if(certificateCollection.Count > 0)
            {
                return certificateCollection[0];
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        public ActionResult SignAllPdf(int x, int y, int width, int height, string thumbPrint, HttpPostedFileBase file)
        {

            try
            {
                // Gerekli kontroller
                if (string.IsNullOrEmpty(thumbPrint))
                {
                    return Json(new { success = false, message = "Sertifika seçilmedi." });
                }

                if (Request.Files.Count == 0 || Request.Files[0] == null)
                {
                    return Json(new { success = false, message = "PDF dosyası bulunamadı." });
                }

                //X509Certificate2 cert = FindCertificateByThumbPrint(thumbPrint);
                X509Certificate2 cert = FindCertificateByThumbprint(thumbPrint);

                if (cert == null)
                    if (cert == null)
                {
                    return Json(new { success = false, message = "Sertifika dosyası bulunamadı." });
                }

                // PDF dosyasını kaydetmek için geçici bir yol oluştur
                string tempFilePath = Path.GetTempFileName();
                file.SaveAs(tempFilePath);


                // İmzalı PDF için geçici bir çıktı dosyası yolu oluştur
                string tempOutputPdfPath = Path.GetTempFileName();
                SignPdf(tempFilePath, tempOutputPdfPath, cert, x, y, width, height);

                // Geçici çıktı dosyasını oku ve kullanıcıya gönder
                byte[] signedPdfBytes = System.IO.File.ReadAllBytes(tempOutputPdfPath);
                return File(signedPdfBytes, "application/pdf", "signed_document.pdf");

            }
            catch (Exception ex)
            {
                // Hatanın detaylarını loglayın
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " İç hata: " + ex.InnerException.Message;
                }
                return Json(new { success = false, message = errorMessage });
            }
        }

        public void SignPdf(string inputPdfPath, string outputPdfPath, X509Certificate2 cert2, int x, int y, int width, int height)
        {

            // Sertifika zincirini oluşturun
            X509Chain x509Chain = new X509Chain();
            x509Chain.Build(cert2);
            ICollection<Org.BouncyCastle.X509.X509Certificate> chain = new List<Org.BouncyCastle.X509.X509Certificate>();

            foreach (X509ChainElement chainElement in x509Chain.ChainElements)
            {
                Org.BouncyCastle.X509.X509Certificate bcCert =
                    DotNetUtilities.FromX509Certificate(chainElement.Certificate);
                chain.Add(bcCert);
            }


            //X509CertificateParser certParser = new X509CertificateParser();
            //Org.BouncyCastle.X509.X509Certificate bcCert = certParser.ReadCertificate(cert2.RawData);
            //ICollection<Org.BouncyCastle.X509.X509Certificate> chain = new List<Org.BouncyCastle.X509.X509Certificate> { bcCert };

            RSA rsaPrivateKey = cert2.GetRSAPrivateKey();
            IExternalSignature pks = new X509Certificate2Signature(cert2, "SHA-256");

            List<byte[]> tempSignedPages = new List<byte[]>();
            PdfReader reader = new PdfReader(inputPdfPath);

            // Her sayfayı ayrı ayrı işle
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                using (MemoryStream fout = new MemoryStream())
                {
                    using (PdfReader singlePageReader = new PdfReader(inputPdfPath))
                    {
                        using (PdfStamper stamper = PdfStamper.CreateSignature(singlePageReader, fout, '\0', null, true))
                        {

                            PdfSignatureAppearance appearance = stamper.SignatureAppearance;
                            appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + width, y + height), i, "Signature" + i);
                            appearance.Reason = "Signing PDF";
                            appearance.Location = "Location";
                            MakeSignature.SignDetached(appearance, pks, chain.ToArray(), null, null, null, 0, CryptoStandard.CMS);
                        }
                    }
                    tempSignedPages.Add(fout.ToArray());
                }
            }

            // Geçici imzalı sayfaları birleştir
            using (Document document = new Document())
            {
                using (PdfCopy copy = new PdfCopy(document, new FileStream(outputPdfPath, FileMode.Create)))
                {
                    document.Open();
                    int pageCount = tempSignedPages.Count; // Toplam imzalı sayfa sayısı

                    for (int i = 0; i < pageCount; i++)
                    {
                        PdfReader tempReader = new PdfReader(tempSignedPages[i]);
                        // Her imzalı PDF'den ilgili sayfayı al (Örn: 1. PDF'den 1. sayfa, 2. PDF'den 2. sayfa, ...)
                        copy.AddPage(copy.GetImportedPage(tempReader, i + 1));
                        tempReader.Close();
                    }
                }
            }

        }
    }
}