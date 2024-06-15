using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Data.SQLite;
using Dapper;
using AsianPublisher.Models;
using DocumentFormat.OpenXml.EMMA;

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    public class AuthorController : Controller
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IConfiguration _configuration, ILogger<AuthorController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                List<Author> records = Author.Get(Utility.FillStyle.AllProperties, param).OrderBy(o => o.name).ToList(); 
                return View(records);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // List
        [HttpPost]
        public IActionResult Index(string id)
        {
            return RedirectToAction("Index");
        }
        // GET: Display form to create a new record
        [HttpGet]
        public IActionResult Create(string url)
        {
            try
            {
                ViewBag.url = url;
                return View(new Author());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Create(Author model, string url)
        {
            if (model.Save())
            {
                return Redirect(url);
            }
            else
            {
                return StatusCode(500, "An error occurred while creating a new record.");
            }
        }
        [HttpGet]
        public IActionResult Edit(string id, string url)
        {
            Author model = Author.GetById(id, Utility.FillStyle.AllProperties);
            ViewBag.url = url;
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Edit(Author model, string url)
        {
            if (model.Update())
            {
                return Redirect(url);
            }
            else
            {
                return StatusCode(500, "An error occurred while updating a new record.");
            }
        }
        [HttpGet]
        public IActionResult Delete(string id, string url)
        {
            Author model = Author.GetById(id, Utility.FillStyle.AllProperties);
            ViewBag.url = url;
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Delete(Author model, string url)
        {
            string result = model.Delete();
            if (result == "true")
            {
                return Redirect(url);
            }
            else
            {
                ViewBag.error = result;
                return View(model);
                //return StatusCode(500, "An error occurred while deleting a new record.");
            }
        }
        public FileContentResult GenerateExcel(string id)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            List<Author> model = Author.Get(Utility.FillStyle.AllProperties, param);
            int v = 1;
            DataTable dt = new DataTable("Authors");
            dt.Columns.AddRange(new DataColumn[7]
            {
    new DataColumn("S.No.",typeof(int)),
    new DataColumn("Name"),
    new DataColumn("MobileNo"),
    new DataColumn("EmailId"),
    new DataColumn("Content"),
    new DataColumn("Content 2"),
    new DataColumn("Code"),
            });
            foreach (Author obj1 in model)
            {
                dt.Rows.Add(
                    v
            , obj1.name, obj1.mobileNo, obj1.emailId, obj1.content, obj1.content2, obj1.code
                );
                v++;
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Author.xlsx");
                }
            }
        }
        public FileContentResult GeneratePdf(string id)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            List<Author> model = Author.Get(Utility.FillStyle.AllProperties, param);
            iTextSharp.text.Font fonta = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, BaseColor.BLACK);
            iTextSharp.text.Font fontb = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontc = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontd = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            MemoryStream mmstream = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 15, 15, 15, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, mmstream);
            doc.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            iTextSharp.text.Paragraph report = new iTextSharp.text.Paragraph("Authors", fontb);
            report.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            report.Font = fontb;
            doc.Add(report);
            PdfPTable table = new PdfPTable(7);
            float[] widths = new float[] { .2f, .6f, .4f, .4f, .5f, .5f, .5f };
            table.SetWidths(widths);
            table.SpacingBefore = 20;
            table.TotalWidth = 560;
            table.LockedWidth = true;
            PdfPCell cell;
            cell = new PdfPCell(new Phrase("SR.No", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Name", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("MobileNo", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("EmailId", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Content", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Content 2", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Code", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            int v = 1;
            foreach (Author obj1 in model)
            {
                cell = new PdfPCell(new Phrase(v.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.name, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.mobileNo, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.emailId, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.content, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.content2, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.code, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                v++;
            }
            doc.Add(table);
            pdfWriter.CloseStream = false;
            doc.Close();
            byte[] bytea = mmstream.ToArray();
            return File(bytea, "application/pdf", "Author.pdf");
        }
    }
}
