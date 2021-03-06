using Aspose.Pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Converter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConverterHtmlController : ControllerBase
    {
        private static readonly string _dataDir = "..\\..\\..\\Samples";

        [HttpGet]
        public void ConvertHTMLtoPDF()
        {
            HtmlLoadOptions options = new HtmlLoadOptions();
            
            var docFileName = System.IO.Path.Combine(_dataDir, "exames.html");
            Document pdfDocument = new Document(docFileName, options);
            
            var newDocFileName = System.IO.Path.Combine(_dataDir, "Exame.pdf");
            pdfDocument.Save(newDocFileName);
        }

        [HttpGet("{url}")]
        public async Task ConvertHTMLtoPDFAdvanced_WebPageAsync(string url)
        {
            // Set page size A3 and Landscape orientation;    
            HtmlLoadOptions options = new HtmlLoadOptions(url)
            {
                PageInfo = { Width = 842, Height = 1191, IsLandscape = true }
            };
            try
            {
                Document pdfDocument = new Document(await GetContentFromUrlAsStreamAsync(url), options);

                var docFileName = System.IO.Path.Combine(_dataDir, "html_test.pdf");

                pdfDocument.Save(docFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task<Stream> GetContentFromUrlAsStreamAsync(string url, ICredentials credentials = null)
        {
            var handler = new HttpClientHandler { Credentials = credentials };

            HttpClient httpClient = new HttpClient(handler);

            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return httpClient.GetStreamAsync(url).GetAwaiter().GetResult();
            }

            return null;
        }
    }
}
