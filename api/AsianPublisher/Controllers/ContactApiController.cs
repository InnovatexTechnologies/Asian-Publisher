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
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class ContactApiController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<ContactApiController> _logger;

        public ContactApiController(IConfiguration _configuration, ILogger<ContactApiController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Contact> records = Contact.Get(Utility.FillStyle.AllProperties);
                return Ok(records);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // List
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                Contact records = Contact.GetById(id, Utility.FillStyle.AllProperties);
                return Ok(records);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(Contact model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        model.idPrefix = "F";
                        model.numId = db.ExecuteScalar<int>("select Max(numId) from Contacts where idPrefix = 'F'") + 1;
                        model.id = model.idPrefix + model.numId;
                        string sql = "INSERT INTO Contacts (id, numId, idPrefix, name, email,address,mobileNo, message) VALUES (@id, @numId, @idPrefix, @name, @email, @address,@mobileNo, @message)";
                        int affectedRows = db.Execute(sql, model, transaction);
                        transaction.Commit();
                        // mail code  
                        MailMessage mail = new MailMessage();
                        //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        SmtpClient SmtpServer = new SmtpClient("sg2plzcpnl491289.prod.sin2.secureserver.net");
                        mail.From = new MailAddress("sales@asianpublishers.co.in");
                        mail.To.Add(model.email);
                        mail.Subject = "Contact Mail (Asian Publishers)";

                        mail.Body = "Here is the query received from your website asianpublisher.in : " + Environment.NewLine + "Name : " + model.name + Environment.NewLine + "Email Id : " + model.email + Environment.NewLine + "Mobile No. : " + model.mobileNo + Environment.NewLine + "Message : " + model.message;

                        SmtpServer.Port = 587;
                        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("sales@asianpublishers.co.in", "sa_753951");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        SmtpServer.Dispose();
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
        public IActionResult Put(Contact model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "update Contacts set name=@name ,email=@email ,address=@address,mobileNo =@mobileNo,message=@message  where id = @id";
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
        public IActionResult Delete(Contact model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "DELETE FROM Contacts WHERE id = @id;";
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
