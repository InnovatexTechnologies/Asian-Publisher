using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System.Data;
using System.Data.SQLite;
using Dapper;
using AsianPublisher.Models;
using DocumentFormat.OpenXml.EMMA;

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    public class DispatchDetailController : Controller
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<DispatchDetailController> _logger;

        public DispatchDetailController(IConfiguration _configuration, ILogger<DispatchDetailController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        //[HttpGet]
        //public IActionResult Index(string orderId)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(orderId))
        //        {
        //            ViewBag.orderId = orderId;
        //        }
        //        ViewBag.Orders = Order.Get(Utility.FillStyle.Basic);
        //        Dictionary<string, string> param = new Dictionary<string, string>();
        //        param.Add("orderId", orderId);
        //        List<DispatchDetail> records = DispatchDetail.Get(Utility.FillStyle.WithBasicNav, param).ToList();
        //        return View(records);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it as needed
        //        return StatusCode(500, "An error occurred while retrieving data from the database.");
        //    }
        //}
        //// List
        //[HttpPost]
        //public IActionResult Index(string id, string orderId)
        //{
        //    return RedirectToAction("Index", new { orderId = orderId });
        //}
        // GET: Display form to create a new record
        [HttpGet]
        public IActionResult Create(string id, string url)
        {
            try
            {
                ViewBag.url = url;
                ViewBag.id = id;

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("id", id);
                List<Order> records = Order.Get(Utility.FillStyle.WithBasicNav, param).ToList();
                //List<DispatchDetail> records = DispatchDetail.Get(Utility.FillStyle.WithBasicNav, param).ToList();
                //if (records.Count == 0)
                //{
                //    return View(new DispatchDetail() { orderId = orderId });
                //}
                return View(records.FirstOrDefault());
                //return View(new DispatchDetail() { orderId=orderId});
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Create(Order model, string orderId, string url, string docketDate)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        model.docketDate = int.Parse(DateTime.Parse(docketDate).ToString("yyyyMMdd"));
                        string sql = "";
                        if (!string.IsNullOrEmpty(model.docketNo) && !string.IsNullOrEmpty(model.courierName) && model.docketDate!=0)
                        {
                            sql = "Update Orders set docketDate=@docketDate,docketNo=@docketNo,courierName=@courierName,isDispatch=1 where id=@id"; 
                        }
                        else
                        {
                            sql = "Update Orders set docketDate=@docketDate,docketNo=@docketNo,courierName=@courierName,isDispatch=0 where id=@id";
                        }
                        int affectedRows = db.Execute(sql, model, transaction);
                        transaction.Commit();
                        return RedirectToAction(actionName: "Index", controllerName: "Order");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(new { ex.Message });
                    }
                }
            }
            //if (!string.IsNullOrEmpty(model.id))
            //{
            //    if (model.Update(docketDate))
            //    {
            //        return RedirectToAction(actionName: "Index", controllerName: "Order");
            //    }
            //    else
            //    {
            //        return StatusCode(500, "An error occurred while creating a new record.");
            //    }
            //}    
            //else if (string.IsNullOrEmpty(model.id))
            //{
            //    if (model.Save(docketDate))
            //    {
            //        return RedirectToAction(actionName: "Index", controllerName: "Order");
            //    }
            //    else
            //    {
            //        return StatusCode(500, "An error occurred while creating a new record.");
            //    }
            //}
            //else
            //{
            //    return StatusCode(500, "An error occurred while creating a new record.");
            //}

        }
    }
}
