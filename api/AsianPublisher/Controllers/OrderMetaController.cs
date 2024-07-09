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
    public class OrderMetaController : Controller
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<OrderMetaController> _logger;

        public OrderMetaController(IConfiguration _configuration, ILogger<OrderMetaController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [HttpGet]
        public IActionResult Index(string orderId, string bookId, string name, string email, string address, string date, int status, int isDispatch)
        {
            try
            {
                if (!string.IsNullOrEmpty(orderId))
                {
                    ViewBag.orderId = orderId;
                }
                if (!string.IsNullOrEmpty(bookId))
                {
                    ViewBag.bookId = bookId;
                }

                ViewBag.Orders = Order.Get(Utility.FillStyle.Basic);
                ViewBag.Books = Book.Get(Utility.FillStyle.Basic);

                ViewBag.name = name;
                ViewBag.email = email;
                ViewBag.address = address;
                ViewBag.date = date;
                ViewBag.status = status;
                ViewBag.isDispatch = isDispatch;

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("orderId", orderId);
                param.Add("bookId", bookId);
                List<OrderMeta> records = OrderMeta.Get(Utility.FillStyle.Custom, param).ToList();
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
        public IActionResult Index(string id, string orderId, string bookId)
        {
            return RedirectToAction("Index", new { orderId = orderId, bookId = bookId });
        }
        // GET: Display form to create a new record
        [HttpGet]
        public IActionResult Create(string url)
        {
            try
            {
                ViewBag.url = url;
                ViewBag.Orders = Order.Get(Utility.FillStyle.AllProperties);
                ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
                return View(new OrderMeta());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Create(OrderMeta model, string url)
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
            OrderMeta model = OrderMeta.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            ViewBag.Orders = Order.Get(Utility.FillStyle.AllProperties);
            ViewBag.Books = Book.Get(Utility.FillStyle.AllProperties);
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Edit(OrderMeta model, string url)
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
            OrderMeta model = OrderMeta.GetById(id, Utility.FillStyle.WithBasicNav);
            ViewBag.url = url;
            if (model == null)
            {
                return NotFound(); // Return 404 Not Found if the record is not found
            }
            return View(model);
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Delete(OrderMeta model, string url)
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
            }
        }
        public FileContentResult GenerateExcel(string id, string orderId, string bookId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("orderId", orderId);
            param.Add("bookId", bookId);
            List<OrderMeta> model = OrderMeta.Get(Utility.FillStyle.WithBasicNav, param);
            int v = 1;
            DataTable dt = new DataTable("OrderMetas");
            dt.Columns.AddRange(new DataColumn[5]
            {
    new DataColumn("S.No."),
    new DataColumn("Order"),
    new DataColumn("BookId"),
    new DataColumn("Quantity"),
    new DataColumn("Price"),
            });
            foreach (OrderMeta obj1 in model)
            {
                dt.Rows.Add(
                    v.ToString()
            , obj1.orderNav?.name, obj1.bookNav?.name, obj1.quantity, obj1.price
                );
                v++;
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderMeta.xlsx");
                }
            }
        }
        public FileContentResult GeneratePdf(string id, string orderId, string bookId)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("orderId", orderId);
            param.Add("bookId", bookId);
            List<OrderMeta> model = OrderMeta.Get(Utility.FillStyle.WithBasicNav, param);
            iTextSharp.text.Font fonta = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.UNDEFINED, BaseColor.BLACK);
            iTextSharp.text.Font fontb = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontc = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontd = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            MemoryStream mmstream = new MemoryStream();
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 15, 15, 15, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, mmstream);
            doc.Open();
            PdfContentByte cb = pdfWriter.DirectContent;
            iTextSharp.text.Paragraph report = new iTextSharp.text.Paragraph("OrderMetas", fontb);
            report.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            report.Font = fontb;
            doc.Add(report);
            if (orderId != null)
            {
                string a = "";
                a = Order.GetById(orderId).name;
                string b = "Order";
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
            PdfPTable table = new PdfPTable(5);
            float[] widths = new float[] { .6f, .6f, .6f, .6f, .6f };
            table.SetWidths(widths);
            table.SpacingBefore = 20;
            table.TotalWidth = 560;
            table.LockedWidth = true;
            PdfPCell cell;
            cell = new PdfPCell(new Phrase("SR.No", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Order", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("BookId", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Quantity", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Price", fontd));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            int v = 1;
            foreach (OrderMeta obj1 in model)
            {
                cell = new PdfPCell(new Phrase(v.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.orderNav?.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.bookNav?.name, fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.quantity.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(obj1.price.ToString(), fonta));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                v++;
            }
            doc.Add(table);
            pdfWriter.CloseStream = false;
            doc.Close();
            byte[] bytea = mmstream.ToArray();
            return File(bytea, "application/pdf", "OrderMeta.pdf");
        }
    }
}
