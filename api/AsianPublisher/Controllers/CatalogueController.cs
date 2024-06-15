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

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    public class CatalogueController : Controller
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<CatalogueController> _logger;

        public CatalogueController(IConfiguration _configuration, ILogger<CatalogueController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }  
        // List
        [HttpGet]
        public IActionResult Index(string courseId, string semesterId, string bookId)
        {
            try
            {
                if (!string.IsNullOrEmpty(courseId))
                {
                    ViewBag.courseId = courseId;
                }
                if (!string.IsNullOrEmpty(semesterId))
                {
                    ViewBag.semesterId = semesterId;
                }
                if (!string.IsNullOrEmpty(bookId))
                {
                    ViewBag.bookId = bookId;
                }

                ViewBag.Courses = Course.Get(Utility.FillStyle.Basic);
                ViewBag.Semesters = Semester.Get(Utility.FillStyle.Basic);
                ViewBag.Books = Book.Get(Utility.FillStyle.Basic);

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("courseId", courseId);
                param.Add("semesterId", semesterId);
                param.Add("bookId", bookId);
                List<Catalogue> records = Catalogue.Get(Utility.FillStyle.WithBasicNav, param);
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
        public IActionResult Index(string id, string courseId, string semesterId, string bookId)
        {
            return RedirectToAction("Index", new { courseId = courseId, semesterId = semesterId, bookId = bookId});
        }
        // GET: Display form to create a new record
        [HttpGet]
        public IActionResult Create(string courseId, string semesterId, string bookId, string code,string url)
        {
            try
            {
                ViewBag.courseId = courseId;
                ViewBag.semesterId = semesterId;
                ViewBag.bookId = bookId;
                ViewBag.code = code;
                ViewBag.url = url;
                ViewBag.Courses = Course.Get(Utility.FillStyle.AllProperties);
                ViewBag.Semesters = Semester.Get(Utility.FillStyle.AllProperties);
                ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
                return View(new Catalogue() { courseId = ViewBag.courseId,semesterId = ViewBag.semesterId, bookId = ViewBag.bookId, code = ViewBag.code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Create(Catalogue model, string url)
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
            Catalogue model = Catalogue.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            ViewBag.Courses = Course.Get(Utility.FillStyle.AllProperties);
            ViewBag.Semesters = Semester.Get(Utility.FillStyle.AllProperties);
            ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Edit(Catalogue model, string url)
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
            Catalogue model = Catalogue.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Delete(Catalogue model, string url)
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
        public FileContentResult GenerateExcel(string id, string courseId, string semesterId, string bookId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("courseId", courseId);
            param.Add("semesterId", semesterId);
            param.Add("bookId", bookId);
            List<Catalogue> model = Catalogue.Get(Utility.FillStyle.WithBasicNav, param);
            int v = 1;
            DataTable dt = new DataTable("Catalogues");
            dt.Columns.AddRange(new DataColumn[4]
            {
    new DataColumn("S.No.",typeof(int)),
    new DataColumn("Course"),
    new DataColumn("Semester"),
    new DataColumn("Book"),
            });
            foreach (Catalogue obj1 in model)
            {
                dt.Rows.Add(
                    v
            , obj1.courseNav?.name, obj1.semesterNav?.name, obj1.bookNav?.name
                );
                v++;
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Catalogue.xlsx");
                }
            }
        }
        public FileContentResult GeneratePdf(string id, string courseId, string semesterId, string bookId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("courseId", courseId);
            param.Add("semesterId", semesterId);
            param.Add("bookId", bookId);
            List<Catalogue> model = Catalogue.Get(Utility.FillStyle.WithBasicNav, param);
            iTextSharp.text.Font fonta = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, BaseColor.BLACK);
            iTextSharp.text.Font fontb = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontc = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontd = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            MemoryStream mmstream = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 15, 15, 15, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, mmstream);
            doc.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            iTextSharp.text.Paragraph report = new iTextSharp.text.Paragraph("Catalogues", fontb);
            report.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            report.Font = fontb;
            doc.Add(report);
            if (courseId != null)
            {
                string a = "";
                a = Course.GetById(courseId).name;
                string b = "Course";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontd);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontd;
                doc.Add(report1);
            }
            if (semesterId != null)
            {
                string a = "";
                a = Semester.GetById(semesterId).name;
                string b = "Semester";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontd);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontd;
                doc.Add(report1);
            }
            if (bookId != null)
            {
                string a = "";
                a = Book.GetById(bookId).name;
                string b = "Book";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontd);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontd;
                doc.Add(report1);
            }
            PdfPTable table = new PdfPTable(4);
            float[] widths = new float[] { .2f, .6f, .6f, .6f };
            table.SetWidths(widths);
            table.SpacingBefore = 20;
            table.TotalWidth = 560;
            table.LockedWidth = true;
            PdfPCell cell;
            cell = new PdfPCell(new Phrase("SR.No", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Course", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Semester", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Book", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            int v = 1;
            foreach (Catalogue obj1 in model)
            {
                cell = new PdfPCell(new Phrase(v.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.courseNav?.name, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.semesterNav?.name, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.bookNav?.name, fonta));
                cell.HorizontalAlignment = 0;
                table.AddCell(cell);
                v++;
            }
            doc.Add(table);
            pdfWriter.CloseStream = false;
            doc.Close();
            byte[] bytea = mmstream.ToArray();
            return File(bytea, "application/pdf", "Catalogue.pdf");
        }
    }
}
