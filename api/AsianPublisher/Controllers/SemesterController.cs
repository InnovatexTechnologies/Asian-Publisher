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
    public class SemesterController : Controller
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<SemesterController> _logger;

        public SemesterController(IConfiguration _configuration, ILogger<SemesterController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [HttpGet]
        public IActionResult Index(string academicId, string semesterId)
        {
            try
            {
                if (!string.IsNullOrEmpty(academicId))
                {
                    ViewBag.academicId = academicId;
                }
                if (!string.IsNullOrEmpty(semesterId))
                {
                    ViewBag.semesterId = semesterId;
                }

                ViewBag.AcademicYears = AcademicYear.Get(Utility.FillStyle.Basic);
                ViewBag.SemesterCategories = SemesterCategory.Get(Utility.FillStyle.Basic);

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("academicId", academicId);
                param.Add("semesterId", semesterId);
                List<Semester> records = Semester.Get(Utility.FillStyle.WithBasicNav, param).OrderBy(o => o.name).ToList(); 
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
        public IActionResult Index(string id, string academicId, string semesterId)
        {
            return RedirectToAction("Index", new { academicId = academicId, semesterId = semesterId });
        }
        // GET: Display form to create a new record
        [HttpGet]
        public IActionResult Create(string url)
        {
            try
            {

                ViewBag.url = url;
                ViewBag.AcademicYears = AcademicYear.Get(Utility.FillStyle.AllProperties);
                ViewBag.SemesterCategories = SemesterCategory.Get(Utility.FillStyle.AllProperties);
                return View(new Semester());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Create(Semester model, string url)
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
            Semester model = Semester.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            ViewBag.AcademicYears = AcademicYear.Get(Utility.FillStyle.AllProperties);
            ViewBag.SemesterCategories = SemesterCategory.Get(Utility.FillStyle.AllProperties);
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Edit(Semester model, string url)
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
            Semester model = Semester.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Delete(Semester model, string url)
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
        public FileContentResult GenerateExcel(string id, string academicId, string semesterId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("academicId", academicId);
            param.Add("semesterId", semesterId);
            List<Semester> model = Semester.Get(Utility.FillStyle.WithBasicNav, param);
            int v = 1;
            DataTable dt = new DataTable("Semesters");
            dt.Columns.AddRange(new DataColumn[4]
            {
    new DataColumn("S.No.",typeof(int)),
    new DataColumn("Name"),
    new DataColumn("Academic Year"),
    new DataColumn("Semester Category"),
            });
            foreach (Semester obj1 in model)
            {
                dt.Rows.Add(
                    v
            , obj1.name, obj1.academicNav?.name, obj1.semesterNav?.name
                );
                v++;
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Semester.xlsx");
                }
            }
        }
        public FileContentResult GeneratePdf(string id, string academicId, string semesterId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("academicId", academicId);
            param.Add("semesterId", semesterId);
            List<Semester> model = Semester.Get(Utility.FillStyle.WithBasicNav, param);
            iTextSharp.text.Font fonta = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, BaseColor.BLACK);
            iTextSharp.text.Font fontb = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontc = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontd = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            MemoryStream mmstream = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 15, 15, 15, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, mmstream);
            doc.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            iTextSharp.text.Paragraph report = new iTextSharp.text.Paragraph("Semesters", fontb);
            report.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            report.Font = fontb;
            doc.Add(report);
            if (academicId != null)
            {
                string a = "";
                a = AcademicYear.GetById(academicId).name;
                string b = "AcademicYear";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontc);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontc;
                doc.Add(report1);
            }
            if (semesterId != null)
            {
                string a = "";
                a = SemesterCategory.GetById(semesterId).name;
                string b = "SemesterCategory";
                iTextSharp.text.Paragraph report1 = new iTextSharp.text.Paragraph($"{b}={a}", fontc);
                report.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                report1.Font = fontc;
                doc.Add(report1);
            }
            PdfPTable table = new PdfPTable(4);
            float[] widths = new float[] { .1f, .6f, .6f, .6f };
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
            cell = new PdfPCell(new Phrase("Academic Year", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Semester Category", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            int v = 1;
            foreach (Semester obj1 in model)
            {
                cell = new PdfPCell(new Phrase(v.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.academicNav?.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.semesterNav?.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                v++;
            }
            doc.Add(table);
            pdfWriter.CloseStream = false;
            doc.Close();
            byte[] bytea = mmstream.ToArray();
            return File(bytea, "application/pdf", "Semester.pdf");
        }
    }
}
