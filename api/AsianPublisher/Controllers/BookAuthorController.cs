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
using System.Security.Policy;

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    public class BookAuthorController : Controller
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<BookAuthorController> _logger;

        public BookAuthorController(IConfiguration _configuration, ILogger<BookAuthorController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [HttpGet]
        public IActionResult Index(string bookId, string authorId,string languageId,string iSBN, string bookIdN)
        {
            try
            {
                if (!string.IsNullOrEmpty(bookId))
                {
                    ViewBag.bookId = bookId;
                }
                if (!string.IsNullOrEmpty(authorId))
                {
                    ViewBag.authorId = authorId;
                }
                if (!string.IsNullOrEmpty(languageId))
                {
                    ViewBag.languageId = languageId;
                }
                if (!string.IsNullOrEmpty(iSBN))
                {
                    ViewBag.iSBN = iSBN;
                }
                if (!string.IsNullOrEmpty(bookIdN))
                {
                    ViewBag.bookIdN = bookIdN;
                }
                ViewBag.Books = Book.Get(Utility.FillStyle.Basic);
                ViewBag.Authors = Author.Get(Utility.FillStyle.Basic);

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("bookId", bookId);
                param.Add("authorId", authorId);
                List<BookAuthor> records = BookAuthor.Get(Utility.FillStyle.WithBasicNav, param);
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
        public IActionResult Index(string id, string bookId, string authorId,string languageId, string iSBN, string bookIdN)
        {
            return RedirectToAction("Index", new { bookId = bookId, authorId = authorId , languageId = languageId, iSBN = iSBN, bookIdN = bookIdN});
        }
        // GET: Display form to create a new record
        [HttpGet]
        public IActionResult Create(string bookId, string url)
        {
            try
            {
                ViewBag.bookId = bookId;

                ViewBag.url = url;
                ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
                ViewBag.Authors = Author.Get(Utility.FillStyle.AllProperties);
                return View(new BookAuthor() { bookId = ViewBag.bookId });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Create(BookAuthor model, string url)
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
            BookAuthor model = BookAuthor.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
            ViewBag.Authors = Author.Get(Utility.FillStyle.AllProperties);
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Edit(BookAuthor model, string url)
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
            BookAuthor model = BookAuthor.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Delete(BookAuthor model, string url)
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
        public FileContentResult GenerateExcel(string id, string bookId, string authorId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("bookId", bookId);
            param.Add("authorId", authorId);
            List<BookAuthor> model = BookAuthor.Get(Utility.FillStyle.WithBasicNav, param);
            int v = 1;
            DataTable dt = new DataTable("BookAuthors");
            dt.Columns.AddRange(new DataColumn[3]
            {
    new DataColumn("S.No.",typeof(int)),
    new DataColumn("Book"),
    new DataColumn("Author"),
            });
            foreach (BookAuthor obj1 in model)
            {
                dt.Rows.Add(
                    v
            , obj1.bookNav?.name, obj1.authorNav?.name
                );
                v++;
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BookAuthor.xlsx");
                }
            }
        }
        public FileContentResult GeneratePdf(string id, string bookId, string authorId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("bookId", bookId);
            param.Add("authorId", authorId);
            List<BookAuthor> model = BookAuthor.Get(Utility.FillStyle.WithBasicNav, param);
            iTextSharp.text.Font fonta = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, BaseColor.BLACK);
            iTextSharp.text.Font fontb = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontc = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontd = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            MemoryStream mmstream = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 15, 15, 15, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, mmstream);
            doc.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            iTextSharp.text.Paragraph report = new iTextSharp.text.Paragraph("BookAuthors", fontb);
            report.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            report.Font = fontb;
            doc.Add(report);
            if (bookId != null)
            {
                string a = "";
                a = Book.GetById(bookId).name;
                string b = "Book";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontc);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontc;
                doc.Add(report1);
            }
            if (authorId != null)
            {
                string a = "";
                a = Author.GetById(authorId).name;
                string b = "Author";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontc);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontc;
                doc.Add(report1);
            }
            PdfPTable table = new PdfPTable(3);
            float[] widths = new float[] { .3f, .6f, .6f };
            table.SetWidths(widths);
            table.SpacingBefore = 20;
            table.TotalWidth = 560;
            table.LockedWidth = true;
            PdfPCell cell;
            cell = new PdfPCell(new Phrase("SR.No", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Book", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Author", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            int v = 1;
            foreach (BookAuthor obj1 in model)
            {
                cell = new PdfPCell(new Phrase(v.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.bookNav?.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.authorNav?.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                v++;
            }
            doc.Add(table);
            pdfWriter.CloseStream = false;
            doc.Close();
            byte[] bytea = mmstream.ToArray();
            return File(bytea, "application/pdf", "BookAuthor.pdf");
        }
    }
}
