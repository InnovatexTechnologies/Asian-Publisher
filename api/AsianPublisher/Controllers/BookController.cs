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
using DocumentFormat.OpenXml.Wordprocessing;

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    public class BookController : Controller
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<BookController> _logger;

        public BookController(IConfiguration _configuration, ILogger<BookController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [HttpGet]
        public IActionResult Index(string languageId, string iSBN, string bookId)
        {
            try
            {
                if (!string.IsNullOrEmpty(languageId))
                {
                    ViewBag.languageId = languageId;
                }
                if (!string.IsNullOrEmpty(iSBN))
                {
                    ViewBag.iSBN = iSBN;
                }
                if (!string.IsNullOrEmpty(bookId))
                {
                    ViewBag.bookId = bookId;
                }
                ViewBag.Languages = Language.Get(Utility.FillStyle.Basic);
                //ViewBag.Books = Book.Get(Utility.FillStyle.Basic);
                //string namePattern = "%" + Book.GetById(bookId, Utility.FillStyle.Basic).name + "%";
                string namePattern = "%" + bookId + "%";
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("languageId", languageId);
                param.Add("iSBN", iSBN);
                param.Add("name", namePattern);
                // List<Book> records = Book.Get(Utility.FillStyle.WithBasicNav, param).OrderBy(o => o.name).ToList();
                List<Book> records = Book.Get(Utility.FillStyle.Custom, param).OrderBy(o => o.name).ToList();
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
        public IActionResult Index(string id, string languageId, string iSBN, string bookId)
        {
            return RedirectToAction("Index", new { languageId = languageId, iSBN = iSBN, bookId = bookId });
        }
        // GET: Display form to create a new record
        [HttpGet]
        public IActionResult Create(string url)
        {
            try
            {
                ViewBag.url = url;
                ViewBag.Languages = Language.Get(Utility.FillStyle.AllProperties);
                ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
                return View(new Book());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Create(Book model, string url, IFormFile samplePdfFile, IFormFile imageFile)
        {
            string path = webHost.WebRootPath;
            if (model.Save(samplePdfFile, imageFile, path))
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
            Book model = Book.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            ViewBag.Languages = Language.Get(Utility.FillStyle.AllProperties);
            ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Edit(Book model, string url, IFormFile samplePdfFile, IFormFile imageFile)
        {
            string path = webHost.WebRootPath;
            if (model.Update(samplePdfFile, imageFile, path))
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
            Book model = Book.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Delete(Book model, string url, string samplePdfFile, string image)
        {
            string path = webHost.WebRootPath;
            string result = model.Delete(samplePdfFile, image, path);
            if (result == "true")
            {
                return Redirect(url);
            }
            else
            {
                ViewBag.error = result;
                return View(model);
            }
        }
        public FileContentResult GenerateExcel(string id, string languageId, string iSBN, string bookId)
        {
            string namePattern = "%" + Book.GetById(bookId, Utility.FillStyle.Basic).name + "%";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("languageId", languageId);
            param.Add("iSBN", iSBN);
            param.Add("name", namePattern);
            List<Book> model = Book.Get(Utility.FillStyle.Custom, param);
            int v = 1;
            DataTable dt = new DataTable("Books");
            dt.Columns.AddRange(new DataColumn[9]
            {
    new DataColumn("S.No.",typeof(int)),
    new DataColumn("BookId"),
    new DataColumn("ISBN"),
    new DataColumn("Name"),
    new DataColumn("Authors"),
    new DataColumn("Course"),
    new DataColumn("BookCode"),
    new DataColumn("Language"),
    new DataColumn("MRP",typeof(decimal)),
            });
            foreach (Book obj1 in model)
            {
                dt.Rows.Add(
                    v, obj1.id
            , obj1.iSBN, obj1.name, string.Join(", ", obj1.authors.Select(o => o.name).ToList()), string.Join(", ", obj1.courseSemesters.Select(o => o.combine).ToList()), obj1.bookCode, obj1.languageNav?.name, obj1.mRP
                );
                v++;
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Book.xlsx");
                }
            }
        }
        public FileContentResult GeneratePdf(string id, string languageId, string iSBN, string bookId)
        {
            string namePattern = "%" + Book.GetById(bookId, Utility.FillStyle.Basic).name + "%";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("languageId", languageId);
            param.Add("iSBN", iSBN);
            param.Add("name", namePattern);
            List<Book> model = Book.Get(Utility.FillStyle.Custom, param);
            iTextSharp.text.Font fonta = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, BaseColor.BLACK);
            iTextSharp.text.Font fontb = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontc = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontd = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            MemoryStream mmstream = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 15, 15, 15, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, mmstream);
            doc.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            iTextSharp.text.Paragraph report = new iTextSharp.text.Paragraph("Books", fontb);
            report.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            report.Font = fontb;
            doc.Add(report);
            if (languageId != null)
            {
                string a = "";
                a = Language.GetById(languageId).name;
                string b = "Language";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontd);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontd;
                doc.Add(report1);
            }
            PdfPTable table = new PdfPTable(7);
            float[] widths = new float[] { .1f, .3f, .5f, .5f, .5f, .2f, .2f };
            table.SetWidths(widths);
            table.SpacingBefore = 20;
            table.TotalWidth = 560;
            table.LockedWidth = true;
            PdfPCell cell;
            cell = new PdfPCell(new Phrase("SR.No", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("ISBN", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Name", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Authors", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Course", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Language", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("MRP", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            int v = 1;
            foreach (Book obj1 in model)
            {
                cell = new PdfPCell(new Phrase(v.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.iSBN, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.name, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(string.Join(", ", obj1.authors.Select(o => o.name).ToList()), fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(string.Join(", ", obj1.courseSemesters.Select(o => o.combine).ToList()), fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.languageNav?.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.mRP.ToString(), fonta));
                cell.HorizontalAlignment = 2;
                table.AddCell(cell);
                v++;
            }
            doc.Add(table);
            pdfWriter.CloseStream = false;
            doc.Close();
            byte[] bytea = mmstream.ToArray();
            return File(bytea, "application/pdf", "Book.pdf");
        }
    }
}

