using System;
using Aspose.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace Converter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneratePdfController : ControllerBase
    {
        private static readonly string _dataDir = "..\\..\\..\\Samples";

        [HttpGet]
        public void GeneratePdf()
        {
            // Initialize document object
            Document document = new Document();
            // Add page
            Page page = document.Pages.Add();
            // Add text to new page
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Hello World!"));
            // Save updated PDF
            var outputFileName = System.IO.Path.Combine(_dataDir, "HelloWorld_out.pdf");
            document.Save(outputFileName);
        }
    }
}
