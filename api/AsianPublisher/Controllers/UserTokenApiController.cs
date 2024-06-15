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
using RestSharp;
using System.Net.Mail;
using Org.BouncyCastle.Asn1.Ocsp;

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class UserTokenApiController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<UserTokenApiController> _logger;
        private readonly LoginController _loginController;

        public UserTokenApiController(IConfiguration _configuration, ILogger<UserTokenApiController> logger, IWebHostEnvironment _webHost, LoginController loginController)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
            _loginController = loginController;
        }
        // List
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<LoginUser> records = LoginUser.Get(Utility.FillStyle.AllProperties);
                return Ok(records);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // List
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                LoginUser records = LoginUser.GetById(id, Utility.FillStyle.AllProperties);
                return Ok(records);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [Authorize]
        [HttpPost]
        public IActionResult Post(User model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                try
                {
                    User user = db.Query<User>("Select * from Users where mobileNo = '" + model.mobileNo + "'").FirstOrDefault();
                    if (user != null)
                    {
                        model.id = user.id;
                        model.name = user.name;
                        model.password = user.password;
                    }
                    else
                    {
                        user = new User();
                        user.idPrefix = "F";
                        user.numId = db.ExecuteScalar<int>("select Max(numId) from Users where idPrefix = 'F'") + 1;
                        user.id = user.idPrefix + user.numId;
                        user.name = model.name;
                        user.mobileNo = model.mobileNo;
                        user.email = model.email;
                        user.address = model.address;
                        //user.password = model.email.Substring(0, 4) + "@123";
                        int length = 8;
                        string password = Utility.GenerateRandomPassword(length);
                        user.password = password;
                        db.Execute("INSERT INTO Users (id, numId, idPrefix, name, email, mobileNo, address,password) VALUES (@id, @numId, @idPrefix, @name, @email, @mobileNo, @address,@password)", user);

                        // mail code
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("sg2plzcpnl491289.prod.sin2.secureserver.net");
                        mail.From = new MailAddress("sales@asianpublishers.co.in");
                        mail.To.Add(user.email);
                        mail.Subject = "Asian Publisher(Auto Generated Password)";

                        mail.Body = " Auto Generated Password : " + user.password + Environment.NewLine + " Please reset your password through the website";

                        SmtpServer.Port = 587;
                        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("sales@asianpublishers.co.in", "sa_753951");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        SmtpServer.Dispose();



                        var client = new RestClient("https://public.doubletick.io/whatsapp/message/template");
                        var request = new RestRequest("", Method.Post);
                        request.AddHeader("Authorization", "key_8qXN4cMQ0G");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("accept", "application/json");
                        var body = $@"{{""messages"":[{{""to"":""{"+91" + user.mobileNo}"",""content"":{{""templateName"":""auto_generated_password"",""language"":""en"",""templateData"":{{""body"":{{""placeholders"":[""{user.password}""]}}}}}}}}]}}";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        RestResponse response = client.Execute(request);

                        //Whatsapp
                        //var client = new RestClient("https://app.messageautosender.com/message/new");
                        //var requestN = new RestRequest("", Method.Post);

                        //requestN.AlwaysMultipartFormData = true;

                        //requestN.AddParameter("username", "asianpublishers");
                        //requestN.AddParameter("password", "Asian@#123");

                        //requestN.AddParameter("receiverMobileNo", user.mobileNo);

                        //string msg = "Auto Generated Password : " + user.password + Environment.NewLine + " Please reset your password through the website";

                        //requestN.AddParameter("message", msg);

                        //client.Execute(requestN);

                        model.id = user.id;
                        model.name = user.name;
                        model.password = user.password;

                    }


                    if (!string.IsNullOrEmpty(model.name) && !string.IsNullOrEmpty(model.password))
                    {
                        //    LoginUser usern = new LoginUser();
                        //    usern.id = model.id;
                        //    usern.name = model.name;
                        //    usern.password = model.password;  
                        //    usern.mobileNo = model.mobileNo;
                        //    usern.email = model.email;
                        //LoginController login = new LoginController(configuration,_logger,webHost);
                        //login.Index(usern);
                        _loginController.Index(model);
                    }
                    return Ok(new { response = Utility.response, UserId = model.id });

                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    return BadRequest(new { ex.Message });
                }

            }
        }
        // POST: Create a new record
        [HttpPut]
        public IActionResult Put(LoginUser model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "update LoginUsers set name=@name ,password=@password,,email=@email,mobileNo=@mobileNo  where id = @id";
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
        public IActionResult Delete(LoginUser model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "DELETE FROM LoginUsers WHERE id = @id;";
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

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.Session.Clear();
            return RedirectToAction("LogIn", "LogIn");
        }
    }
}
