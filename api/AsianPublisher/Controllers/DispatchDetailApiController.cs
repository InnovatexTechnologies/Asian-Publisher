//using System;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Data.SQLite;
//using System.Data;
//using System.Linq;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Dapper;
//using AsianPublisher.Models;

//namespace AsianPublisher.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    [EnableCors("AllowAllOrigins")]
//    public class DispatchDetailApiController : ControllerBase
//    {
//        private IConfiguration configuration;
//        private readonly IWebHostEnvironment webHost;
//        private readonly ILogger<DispatchDetailApiController> _logger;

//        public DispatchDetailApiController(IConfiguration _configuration, ILogger<DispatchDetailApiController> logger, IWebHostEnvironment _webHost)
//        {
//            configuration = _configuration;
//            _logger = logger;
//            webHost = _webHost;
//        }
//        // List
//        [AllowAnonymous]
//        [HttpGet]
//        public IActionResult Get()
//        {
//            try
//            {
//                List<DispatchDetail> records = DispatchDetail.Get(Utility.FillStyle.AllProperties);
//                return Ok(records);
//            }
//            catch (Exception ex)
//            {
//                // Log the exception or handle it as needed
//                return StatusCode(500, "An error occurred while retrieving data from the database.");
//            }
//        }
//        // List
//        [HttpGet("{id}")]
//        public IActionResult Get(string id)
//        {
//            try
//            {
//                DispatchDetail records = DispatchDetail.GetById(id, Utility.FillStyle.AllProperties);
//                return Ok(records);
//            }
//            catch (Exception ex)
//            {
//                // Log the exception or handle it as needed
//                return StatusCode(500, "An error occurred while retrieving data from the database.");
//            }
//        }
//        // POST: Create a new record
//        [HttpPost]
//        public IActionResult Post(DispatchDetail model)
//        {
//            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
//            {
//                db.Open();
//                using (var transaction = db.BeginTransaction())
//                {
//                    try
//                    {
//                        model.idPrefix = "F";
//                        model.numId = db.ExecuteScalar<int>("select Max(numId) from DispatchDetails where idPrefix = 'F'") + 1;
//                        model.id = model.idPrefix + model.numId;
//                        string sql = "INSERT INTO DispatchDetails (id, numId, idPrefix, courierName, dopicNo, dopicDate, orderId) VALUES (@id, @numId, @idPrefix, @courierName, @dopicNo, @dopicDate, @orderId)";
//                        int affectedRows = db.Execute(sql, model, transaction);
//                        db.Execute("update Orders set isDispatch = 1 where id = '"+model.orderId+"'","",transaction);
//                        transaction.Commit();
//                        return Ok(new { Message = "Success" });
//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        return BadRequest(new { ex.Message });
//                    }
//                }
//            }
//        }
//        // POST: Create a new record
//        [HttpPut]
//        public IActionResult Put(DispatchDetail model)
//        {
//            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
//            {
//                db.Open();
//                using (var transaction = db.BeginTransaction())
//                {
//                    try
//                    {
//                        string sql = "update DispatchDetails set courierName=@courierName ,dopicNo=@dopicNo ,dopicDate=@dopicDate ,orderId=@orderId  where id = @id";
//                        int affectedRows = db.Execute(sql, model, transaction);
//                        transaction.Commit();
//                        return Ok(new { Message = "Updated" });
//                    }
//                    catch (Exception ex)
//                    {
//                        transaction.Rollback();
//                        return BadRequest(new { ex.Message });
//                    }
//                }
//            }
//        }
//        // POST: Create a new record
//        [HttpDelete]
//        public IActionResult Delete(string id)
//        {
//            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
//            {
//                db.Open();
//                using (var transaction = db.BeginTransaction())
//                {
//                    try
//                    {
//                        string sql = "DELETE FROM DispatchDetails WHERE id = @id;";
//                        int affectedRows = db.Execute(sql, new { id = id }, transaction);
//                        transaction.Commit();
//                        return Ok(new { Message = "Deleted" });
//                    }
//                    catch (Exception ex)
//                    {
//                        return BadRequest(new { ex.Message });
//                    }
//                }
//            }
//        }
//    }
//}
