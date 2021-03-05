using Aspose.Pdf;
using Aspose.Pdf.Text;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

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

            // -------------------------------------------------------------
            // Add image
            var imageFileName = System.IO.Path.Combine(_dataDir, "logo.png");
            var imageStream = new FileStream(imageFileName, FileMode.Open);
            // Add image to Images collection of Page Resources
            page.Resources.Images.Add(imageStream);

            // Using GSave operator: this operator saves current graphics state
            page.Contents.Add(new Aspose.Pdf.Operators.GSave());

            var _logoPlaceHolder = new Rectangle(20, 730, 120, 830);
            // Create Matrix object
            var matrix = new Matrix(new[] {
                _logoPlaceHolder.URX - _logoPlaceHolder.LLX, 0, 0,
                _logoPlaceHolder.URY - _logoPlaceHolder.LLY,
                _logoPlaceHolder.LLX, _logoPlaceHolder.LLY });

            // Using ConcatenateMatrix (concatenate matrix) operator: defines how image must be placed

            page.Contents.Add(new Aspose.Pdf.Operators.ConcatenateMatrix(matrix));

            var ximage = page.Resources.Images[page.Resources.Images.Count];
            // Using Do operator: this operator draws image
            page.Contents.Add(new Aspose.Pdf.Operators.Do(ximage.Name));
            // Using GRestore operator: this operator restores graphics state
            page.Contents.Add(new Aspose.Pdf.Operators.GRestore());

            // -------------------------------------------------------------
            // Add Header
            var header = new TextFragment("New ferry routes in Fall 2020");
            header.TextState.Font = FontRepository.FindFont("Arial");
            header.TextState.FontSize = 24;
            header.HorizontalAlignment = HorizontalAlignment.Center;
            header.Position = new Position(130, 720);
            page.Paragraphs.Add(header);

            // Add description 
            var descriptionText = "Visitors must buy tickets online and tickets are limited to 5,000 per day. Ferry service is operating at half capacity and on a reduced schedule. Expect lineups.";
            var description = new TextFragment(descriptionText);
            description.TextState.Font = FontRepository.FindFont("Times New Roman");
            description.TextState.FontSize = 14;
            description.HorizontalAlignment = HorizontalAlignment.Left;
            page.Paragraphs.Add(description);


            // Add table
            var table = new Table
            {
                ColumnWidths = "200",
                Border = new BorderInfo(BorderSide.Box, 1f, Color.DarkSlateGray),
                DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Color.Black),
                DefaultCellPadding = new MarginInfo(4.5, 4.5, 4.5, 4.5),
                Margin =
                {
                    Bottom = 10
                },
                DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("Helvetica")
                }
            };

            var headerRow = table.Rows.Add();
            headerRow.Cells.Add("Departs City");
            headerRow.Cells.Add("Departs Island");
            foreach (Cell headerRowCell in headerRow.Cells)
            {
                headerRowCell.BackgroundColor = Color.Gray;
                headerRowCell.DefaultCellTextState.ForegroundColor = Color.WhiteSmoke;
            }

            var time = new TimeSpan(6, 0, 0);
            var incTime = new TimeSpan(0, 30, 0);
            for (int i = 0; i < 10; i++)
            {
                var dataRow = table.Rows.Add();
                dataRow.Cells.Add(time.ToString(@"hh\:mm"));
                time = time.Add(incTime);
                dataRow.Cells.Add(time.ToString(@"hh\:mm"));
            }

            page.Paragraphs.Add(table);

            document.Save(System.IO.Path.Combine(_dataDir, "Complex.pdf"));
        }
    }
}
