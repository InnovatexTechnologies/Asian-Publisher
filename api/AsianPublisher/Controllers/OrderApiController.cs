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
using static System.Data.Entity.Infrastructure.Design.Executor;
using iTextSharp.text;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.Mail;
using RestSharp;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data.Entity.Core.Objects;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AsianPublisher.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class OrderApiController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<OrderApiController> _logger;

        public OrderApiController(IConfiguration _configuration, ILogger<OrderApiController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Order> records = Order.Get(Utility.FillStyle.AllProperties);
                return Ok(new { Message = "Success", Orders = records });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        [Authorize]
        [HttpGet("OrderListByUser")]
        public IActionResult OrderListByUser()
        {
            try
            {
                string userId = Request.Headers.ContainsKey("userId") ? Request.Headers["userId"].FirstOrDefault() : "";
                Dictionary<string, string> paramlist = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(userId))
                {
                    paramlist.Add("userId", userId);
                }
                List<Order> records = new List<Order>();
                if (!string.IsNullOrEmpty(userId))
                {
                    records = Order.Get(Utility.FillStyle.AllProperties, paramlist);
                }
                return Ok(new { Message = "Success", Orders = records });
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
                Order records = Order.GetById(id, Utility.FillStyle.AllProperties);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }


        // POST: Create a new record
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(Order model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        User user = db.Query<User>("Select * from Users where mobileNo = '" + model.mobileNo + "'").FirstOrDefault();
                        if (user != null)
                        {
                            model.userId = user.id;
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
                            model.userId = user.id;


                            // mail code
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("sg2plzcpnl491289.prod.sin2.secureserver.net");
                            mail.From = new MailAddress("sales@asianpublishers.co.in");
                            mail.To.Add("");
                            mail.Subject = "Asian Publisher(Login Credentials)";

                            mail.Body = "Auto Generated Login Credentials : " + Environment.NewLine + "UserName : " + user.name + Environment.NewLine + "Password : " + user.password + Environment.NewLine + " Please reset your password after first login";

                            SmtpServer.Port = 587;
                            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("sales@asianpublishers.co.in", "sa_753951");
                            SmtpServer.EnableSsl = true;

                            SmtpServer.Send(mail);
                            SmtpServer.Dispose();


                            var client = new RestClient("https://public.doubletick.io/whatsapp/message/template");
                            var requestN = new RestRequest("", Method.Post);
                            requestN.AddHeader("Authorization", "key_8qXN4cMQ0G");
                            requestN.AddHeader("Content-Type", "application/json");
                            requestN.AddHeader("accept", "application/json");
                            var body = $@"{{""messages"":[{{""to"":""{"+91" + user.mobileNo}"",""content"":{{""templateName"":""auto_generated_password"",""language"":""en"",""templateData"":{{""body"":{{""placeholders"":[""{user.password}""]}}}}}}}}]}}";
                            requestN.AddParameter("application/json", body, ParameterType.RequestBody);
                            RestResponse responseN = client.Execute(requestN);
                        }

                        model.idPrefix = "F";
                        model.numId = db.ExecuteScalar<int>("select Max(numId) from Orders where idPrefix = 'F'") + 1;
                        model.id = model.idPrefix + model.numId;

                        string Tok_id = "";

                        string payInstrument = "";

                        Payrequest.RootObject rt = new Payrequest.RootObject();
                        Payrequest.MsgBdy mb = new Payrequest.MsgBdy();
                        Payrequest.HeadDetails hd = new Payrequest.HeadDetails();
                        Payrequest.MerchDetails md = new Payrequest.MerchDetails();
                        Payrequest.PayDetails pd = new Payrequest.PayDetails();
                        Payrequest.CustDetails cd = new Payrequest.CustDetails();
                        Payrequest.Extras ex = new Payrequest.Extras();

                        Payrequest.Payrequest pr = new Payrequest.Payrequest();


                        hd.version = "OTSv1.1";
                        hd.api = "AUTH";
                        hd.platform = "FLASH";

                        md.merchId = Utility.merchId;
                        md.userId = Utility.userId;
                        md.password = Utility.password;
                        md.merchTxnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        md.merchTxnId = model.id;


                        pd.amount = model.orderMetas.Sum(o => o.price * o.quantity).ToString();
                        //pd.product = "NSE";
                        pd.product = "PUBLISHERS";
                        pd.custAccNo = model.id;
                        pd.txnCurrency = "INR";

                        cd.custEmail = model.email;
                        cd.custMobile = model.mobileNo;

                        ex.udf1 = "";
                        ex.udf2 = "";
                        ex.udf3 = "";
                        ex.udf4 = "";
                        ex.udf5 = "";


                        pr.headDetails = hd;
                        pr.merchDetails = md;
                        pr.payDetails = pd;
                        pr.custDetails = cd;
                        pr.extras = ex;

                        rt.payInstrument = pr;

                        var json = JsonConvert.SerializeObject(rt);


                        //string passphrase = "A4476C2062FFA58980DC8F79EB6A799E";
                        //string salt = "A4476C2062FFA58980DC8F79EB6A799E";
                        byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                        int iterations = 65536;
                        int keysize = 256;
                        string hashAlgorithm = "SHA1";
                        string Encryptval = Utility.Encrypt(json, Utility.passphrase, Utility.salt, iv, iterations);

                        //string testurleq = "https://caller.atomtech.in/ots/aipay/auth?merchId=8952&encData=" + Encryptval;
                        string testurleq = "https://payment1.atomtech.in/ots/aipay/auth?merchId=" + Utility.merchId + "&encData=" + Encryptval;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testurleq);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                        request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                        Encoding encoding = new UTF8Encoding();
                        byte[] data = encoding.GetBytes(json);
                        request.ProtocolVersion = HttpVersion.Version11;
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        request.ContentLength = data.Length;
                        Stream stream = request.GetRequestStream();
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        string jsonresponse = response.ToString();

                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string temp = null;
                        string status = "";
                        while ((temp = reader.ReadLine()) != null)
                        {
                            jsonresponse += temp;
                        }
                        var result = jsonresponse.Replace("System.Net.HttpWebResponse", "");
                        var uri = new Uri("http://atom.in?" + result);
                        //var uri = new Uri("https://psa.atomtech.in/staticdata/ots/js/atomcheckout.js?" + result);


                        var query = HttpUtility.ParseQueryString(uri.Query);

                        string encData = query.Get("encData");
                        string Decryptval = Utility.decrypt(encData, Utility.passphrase1, Utility.salt1, iv, iterations);

                        Payverify.Payverify objectres = new Payverify.Payverify();
                        objectres = JsonConvert.DeserializeObject<Payverify.Payverify>(Decryptval);
                        string txnMessage = objectres.responseDetails.txnMessage;

                        model.tokenId = objectres.atomTokenId;
                        model.merchId = Utility.merchId;
                        model.date = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                        model.time = int.Parse(DateTime.Now.ToString("HHmmssfff"));
                        string sql = $"INSERT INTO Orders (id, numId, idPrefix, name, email, address,docketDate,docketNo,courierName, city,state,isDispatch, country, pincode, mobileNo,date,time,tokenId,merchId,status,userId) VALUES (@id, @numId, @idPrefix, @name, @email, @address,@docketDate,@docketNo,@courierName, @city,@state,0, @country, @pincode, @mobileNo, @date,@time,@tokenId,@merchId,@status,@userId)";
                        int affectedRows = db.Execute(sql, model, transaction);

                        int counter = 1;
                        string orderId = model.id;
                        foreach (OrderMeta meta in model.orderMetas)
                        {
                            meta.idPrefix = "F";
                            meta.numId = db.ExecuteScalar<int>("select Max(numId) from OrderMetas where idPrefix = 'F'") + 1;
                            meta.id = meta.idPrefix + meta.numId;
                            string sqlm = $"INSERT INTO OrderMetas (id, numId, idPrefix, orderId, bookId, quantity, price) VALUES (@id, @numId, @idPrefix, '{orderId}', @bookId, @quantity, @price)";
                            int affectedRowsm = db.Execute(sqlm, meta, transaction);
                        }

                        transaction.Commit();

                        model.orderMetas = null;

                        return Ok(new { Message = "Success", Order = model });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(new { ex.Message });
                    }
                }
            }
        }
  
        [HttpPut]
        public IActionResult Put(string id)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        db.Execute("UPDATE Orders SET isDispatch = -1 WHERE Id = '" + id + "'", null, transaction);
                        transaction.Commit();
                        string userId = db.ExecuteScalar<string>("select userId from Orders where id = '" + id + "'");
                        User user = db.Query<User>("select * from Users where id = '" + userId + "'").FirstOrDefault();


                        var client = new RestClient("https://public.doubletick.io/whatsapp/message/template");
                        //client.Timeout = -1;
                        var request = new RestRequest("", Method.Post);
                        request.AddHeader("Authorization", "key_8qXN4cMQ0G");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("accept", "application/json");
                        var body = $@"{{""messages"":[{{""to"":""{"+91"+user.mobileNo}"",""content"":{{""templateName"":""order_canceled"",""language"":""en"",""templateData"":{{""body"":{{""placeholders"":[""{user.name}"",""{id}""]}}}}}}}}]}}";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        RestResponse response = client.Execute(request);

                        client = new RestClient("https://public.doubletick.io/whatsapp/message/template");
                        request = new RestRequest("", Method.Post);
                        request.AddHeader("Authorization", "key_8qXN4cMQ0G");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("accept", "application/json");
                        body = $@"{{""messages"":[{{""to"":""+919873620572"",""content"":{{""templateName"":""order_canceled_own"",""language"":""en"",""templateData"":{{""body"":{{""placeholders"":[""{id}""]}}}}}}}}]}}";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        response = client.Execute(request);

                        //MailMessage mailo = new MailMessage();
                        //SmtpClient SmtpServero = new SmtpClient("sg2plzcpnl491289.prod.sin2.secureserver.net");
                        //mailo.From = new MailAddress("sales@asianpublishers.co.in");
                        //mailo.To.Add(user.email);
                        //mailo.Subject = "Order Cancellation - Your OrderId is " + id;
                        //mailo.Body = "Dear " + user.name + ", " + Environment.NewLine
                        //    + "Your order has been cancelled successfully! Your OrderId is: " + id +
                        //    "." + Environment.NewLine +
                        //    "Warm Regards" + Environment.NewLine + "Team Asian Publishers";
                        //SmtpServero.Port = 587;
                        //SmtpServero.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //SmtpServero.UseDefaultCredentials = false;
                        //SmtpServero.Credentials = new System.Net.NetworkCredential("sales@asianpublishers.co.in", "sa_753951");
                        //SmtpServero.EnableSsl = true;
                        //SmtpServero.Send(mailo);
                        //SmtpServero.Dispose();


                        //MailMessage mailN = new MailMessage();
                        //SmtpClient SmtpServerN = new SmtpClient("sg2plzcpnl491289.prod.sin2.secureserver.net");
                        //mailN.From = new MailAddress("sales@asianpublishers.co.in");
                        //mailN.To.Add("smittal@asianpublishers.co.in");
                        //mailN.Subject = "Order Cancellation - Your OrderId is " + id;
                        //mailN.Body = "Dear Asian Publishers Team, " + Environment.NewLine +
                        //"Order " + id + " has been cancelled from the Customer." + Environment.NewLine +
                        //"Thank you.";
                        //SmtpServerN.Port = 587;
                        //SmtpServerN.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //SmtpServerN.UseDefaultCredentials = false;
                        //SmtpServerN.Credentials = new System.Net.NetworkCredential("sales@asianpublishers.co.in", "sa_753951");
                        //SmtpServerN.EnableSsl = true;
                        //SmtpServerN.Send(mailN);
                        //SmtpServerN.Dispose();



                        ////Whatsapp
                        //var cliento = new RestClient("https://app.messageautosender.com/message/new");
                        //var requestNo = new RestRequest("", Method.Post);

                        //requestNo.AlwaysMultipartFormData = true;
                        //requestNo.AddParameter("username", "asianpublishers");
                        //requestNo.AddParameter("password", "Asian@#123");
                        //requestNo.AddParameter("receiverMobileNo", user.mobileNo);

                        //string msgo = "Dear " + user.name + ", " + Environment.NewLine
                        //    + "Your order has been cancelled successfully! Your OrderId is: " + id +
                        //    "." + Environment.NewLine +
                        //    "Warm Regards" + Environment.NewLine + "Team Asian Publishers";
                        //requestNo.AddParameter("message", msgo);
                        //cliento.Execute(requestNo);



                        ////Whatsapp
                        //var client = new RestClient("https://app.messageautosender.com/message/new");
                        //var requestN = new RestRequest("", Method.Post);

                        //requestN.AlwaysMultipartFormData = true;
                        //requestN.AddParameter("username", "asianpublishers");
                        //requestN.AddParameter("password", "Asian@#123");
                        //requestN.AddParameter("receiverMobileNo", "9873620572");

                        //string msg = "Dear Asian Publishers Team, " + Environment.NewLine +
                        //    "Order " + id + " has been cancelled from the Customer." + Environment.NewLine +
                        //    "Thank you.";
                        ////"Warm Regards," + Environment.NewLine +
                        ////"Team Marwari Software";

                        //requestN.AddParameter("message", msg);

                        //client.Execute(requestN);



                        return Ok(new { Message = "Order Cancelled" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(new { ex.Message });
                    }
                }
            }
        }

        //POST: Create a new record
        //[HttpPut]
        //public IActionResult Put(Order model)
        //{
        //    using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
        //    {
        //        db.Open();
        //        using (var transaction = db.BeginTransaction())
        //        {
        //            try
        //            {
        //                model.date = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        //                model.time = int.Parse(DateTime.Now.ToString("HHmmssfff"));
        //                string sql = "UPDATE Orders SET numId = @numId, idPrefix = @idPrefix, name = @name, email = @email, address = @address, city = @city,state = @state, country=@country, pincode=@pincode, mobileNo = @mobileNo,date=@date,time=@time, tokenId=@tokenId ,status=@status WHERE id = @id;";
        //                //foreach (OrderMeta meta in model.orderMetas)
        //                //{
        //                //    string sqld = "DELETE FROM OrderMetas WHERE orderId = " + model.id;
        //                //    int affectedRowsd = db.Execute(sqld, transaction);
        //                //    transaction.Commit();
        //                //    meta.idPrefix = "F";
        //                //    meta.numId = db.ExecuteScalar<int>("select Max(numId) from OrderMetas where idPrefix = 'F'") + 1;
        //                //    meta.id = meta.idPrefix + meta.numId;
        //                //    string sqlm = $"INSERT INTO OrderMetas (id, numId, idPrefix, orderId, bookId, quantity, price) VALUES (@id, @numId, @idPrefix, {model.id}, @bookId, @quantity, @price)";
        //                //    int affectedRowsm = db.Execute(sqlm, this, transaction);
        //                //    transaction.Commit();
        //                //    return Ok(new { Message = "Success" });
        //                //}
        //                int affectedRows = db.Execute(sql, model, transaction);
        //                transaction.Commit();
        //                return Ok(new { Message = "Updated" });
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                return BadRequest(new { ex.Message });
        //            }
        //        }
        //    }

        //}




        // POST: Create a new record
        //[HttpDelete]
        //public IActionResult Delete(Order model)
        //{
        //    using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
        //    {
        //        db.Open();
        //        using (var transaction = db.BeginTransaction())
        //        {
        //            try
        //            {
        //                string sql = "DELETE FROM Orders WHERE id = @id;";
        //                int affectedRows = db.Execute(sql, new { id = model.id }, transaction);
        //                transaction.Commit();
        //                return Ok(new { Message = "Deleted" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest(new { ex.Message });
        //            }
        //        }
        //    }
        //}
    }
}
