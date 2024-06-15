using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;  
using Dapper;
using AsianPublisher.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AsianPublisher.Controllers
{
    [Authorize]
    [Route("api/[controller]")]   
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class UserApiController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<UserApiController> _logger;

        public UserApiController(IConfiguration _configuration, ILogger<UserApiController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        List<User> records = AsianPublisher.Models.User.Get(Utility.FillStyle.AllProperties);
        //        return Ok(records);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it as needed
        //        return StatusCode(500, "An error occurred while retrieving data from the database.");
        //    }
        //}

        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
            //HttpContext.Session.Clear();
            return Ok("LogOut");
        }
        // List
        //[HttpGet("{id}")]
        public IActionResult Get(string email,string password)
        {
            try
            {
                User records = AsianPublisher.Models.User.GetById(email,password, Utility.FillStyle.AllProperties);
                if (records.count>0)
                {
                    return Ok(new { Message = "Success" ,UserId = records.id});
                }
                else
                { 
                    return Ok("User Not Exists");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed  
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Post(User model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        model.idPrefix = "F";
                        model.numId = db.ExecuteScalar<int>("select Max(numId) from Users where idPrefix = 'F'") + 1;
                        model.id = model.idPrefix + model.numId;
                        string sql = "INSERT INTO Users (id, numId, idPrefix, name, email, mobileNo, address,password) VALUES (@id, @numId, @idPrefix, @name, @email, @mobileNo, @address,@password)";
                        int affectedRows = db.Execute(sql, model, transaction);
                        transaction.Commit();
                        return Ok(new { Message = "Success" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(new { ex.Message });
                    }
                }
            }
        }
        // POST: Create a new record
        [HttpPut]
        public IActionResult Put(User model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "update Users set name=@name ,email=@email ,mobileNo=@mobileNo ,address=@address,password=@password  where id = @id";
                        int affectedRows = db.Execute(sql, model, transaction);
                        transaction.Commit();
                        return Ok(new { Message = "Updated" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(new { ex.Message });
                    }
                }
            }
        }
        // POST: Create a new record
        [HttpDelete]
        public IActionResult Delete(User model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "DELETE FROM Users WHERE id = @id;";
                        int affectedRows = db.Execute(sql, new { id = model.id }, transaction);
                        transaction.Commit();
                        return Ok(new { Message = "Deleted" });
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { ex.Message });
                    }
                }
            }
        }


       

    }
}
