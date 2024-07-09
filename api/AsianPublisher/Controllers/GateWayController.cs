using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.Data;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Transactions;
using System.Web;
using Dapper;
using AsianPublisher.Models;
using RestSharp;
using System.Net.Mail;
using System.Net.Mime;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace AsianPublisher.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //[EnableCors("AllowAllOrigins")]
    [AllowAnonymous]
    public class GateWayController : Controller
    {
        [HttpPost]
        public IActionResult Response(string encData, string merchId)
        {
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            int iterations = 65536;
            int keysize = 256;
            // string plaintext = "{\"payInstrument\":{\"headDetails\":{\"version\":\"OTSv1.1\",\"payMode\":\"SL\",\"channel\":\"ECOMM\",\"api\":\"SALE\",\"stage\":1,\"platform\":\"WEB\"},\"merchDetails\":{\"merchId\":8952,\"userId\":\"\",\"password\":\"Test@123\",\"merchTxnId\":\"1234567890\",\"merchType\":\"R\",\"mccCode\":562,\"merchTxnDate\":\"2019-12-24 20:46:00\"},\"payDetails\":{\"prodDetails\":[{\"prodName\": \"NSE\",\"prodAmount\": 10.00}],\"amount\":10.00,\"surchargeAmount\":0.00,\"totalAmount\":10.00,\"custAccNo\":null,\"custAccIfsc\":null,\"clientCode\":\"12345\",\"txnCurrency\":\"INR\",\"remarks\":null,\"signature\":\"7c643bbd9418c23e972f5468377821d9f0486601e1749930816c409fddbc7beb5d2943d832b6382d3d4a8bd7755e914922fb85aa8c234210bf2993566686a46a\"},\"responseUrls\":{\"returnUrl\":\"http://172.21.21.136:9001/payment/ots/v1/merchresp\",\"cancelUrl\":null,\"notificationUrl\":null},\"payModeSpecificData\":{\"subChannel\":[\"BQ\"],\"bankDetails\":null,\"emiDetails\":null,\"multiProdDetails\":null,\"cardDetails\":null},\"extras\":{\"udf1\":null,\"udf2\":null,\"udf3\":null,\"udf4\":null,\"udf5\":null},\"custDetails\":{\"custFirstName\":null,\"custLastName\":null,\"custEmail\":\"test@gm.com\",\"custMobile\":null,\"billingInfo\":null}}} ";
            string hashAlgorithm = "SHA1";

            //string passphrase1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
            //string salt1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
            string passphrase1 = "5681B1EAAC5ED0FEF24D64EFF522FCE4";//aes response key
            string salt1 = "5681B1EAAC5ED0FEF24D64EFF522FCE4";//aes response key
            string Decryptval = Utility.decrypt(encData, passphrase1, salt1, iv, iterations);


            Payresponse.Rootobject root = new Payresponse.Rootobject();
            Payresponse.Parent objectres = new Payresponse.Parent();
            objectres = JsonConvert.DeserializeObject<Payresponse.Parent>(Decryptval);


            string message = objectres.payInstrument.responseDetails.message;
            string statusCode = objectres.payInstrument.responseDetails.statusCode;
            string bankTxnId = objectres.payInstrument.payModeSpecificData.bankDetails.bankTxnId;
            string atomTxnId = objectres.payInstrument.payDetails.atomTxnId;
            string txnCompleteDate = objectres.payInstrument.payDetails.txnCompleteDate;
            string amount = objectres.payInstrument.payDetails.amount;

            if (message == "SUCCESS")
            {
                using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
                {
                    db.Open();
                    try
                    {
                        //string sqlp = $"UPDATE Orders SET status = 1 WHERE Id = " + objectres.payInstrument.merchDetails.merchTxnId;
                        //int affectedRowsp = db.Execute(sqlp);
                        db.Execute("UPDATE Orders SET status = 1 WHERE Id = '" + objectres.payInstrument.merchDetails.merchTxnId + "'");
                        //db.Execute("UPDATE Orders SET status = 1 WHERE Id = 'F1'");

                        string userId = db.ExecuteScalar<string>("select userId from Orders where id = '" + objectres.payInstrument.merchDetails.merchTxnId + "'");
                        User user = db.Query<User>("select * from Users where id = '" + userId + "'").FirstOrDefault();


                        var client = new RestClient("https://public.doubletick.io/whatsapp/message/template");
                        var request = new RestRequest("", Method.Post);
                        request.AddHeader("Authorization", "key_8qXN4cMQ0G");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("accept", "application/json");
                        var body = $@"{{""messages"":[{{""to"":""{"+91" + user.mobileNo}"",""content"":{{""templateName"":""order_confirm"",""language"":""en"",""templateData"":{{""body"":{{""placeholders"":[""{user.name}"",""{objectres.payInstrument.merchDetails.merchTxnId}""]}}}}}}}}]}}";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        RestResponse response = client.Execute(request);

                        client = new RestClient("https://public.doubletick.io/whatsapp/message/template");
                        request = new RestRequest("", Method.Post);
                        request.AddHeader("Authorization", "key_8qXN4cMQ0G");
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("accept", "application/json");
                        body = $@"{{""messages"":[{{""to"":""+919873620572"",""content"":{{""templateName"":""order_canceled"",""language"":""en"",""templateData"":{{""body"":{{""placeholders"":[""Anirudh Agarwal"",""F34""]}}}}}}}}]}}";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        response = client.Execute(request);



                        MailMessage mailo = new MailMessage();
                        //SmtpClient SmtpServero = new SmtpClient("smtp.gmail.com");
                        //mailo.From = new MailAddress("marwarisoftware@gmail.com");
                        SmtpClient SmtpServero = new SmtpClient("sg2plzcpnl491289.prod.sin2.secureserver.net");
                        mailo.From = new MailAddress("sales@asianpublishers.co.in");
                        mailo.To.Add(user.email);
                        mailo.Subject = "Order Confirmation - Your OrderId is " + objectres.payInstrument.merchDetails.merchTxnId;
                        mailo.Body = "Dear " + user.name + ", " + Environment.NewLine
                            + "We are thrilled to inform you that your order has been successfully placed! Your OrderId is: " + objectres.payInstrument.merchDetails.merchTxnId +
                            "." + Environment.NewLine + "Thank you for choosing Asian Publishers. We're committed to providing you with top-quality " +
                            "service and products. If you have any questions or require assistance, feel free to reach out to us." + Environment.NewLine +
                            "Warm Regards" + Environment.NewLine + "Team Asian Publishers";
                        SmtpServero.Port = 587;
                        SmtpServero.DeliveryMethod = SmtpDeliveryMethod.Network;
                        SmtpServero.UseDefaultCredentials = false;
                        SmtpServero.Credentials = new System.Net.NetworkCredential("sales@asianpublishers.co.in", "sa_753951");
                        SmtpServero.EnableSsl = true;
                        SmtpServero.Send(mailo);
                        SmtpServero.Dispose();


                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("sg2plzcpnl491289.prod.sin2.secureserver.net");
                        mail.From = new MailAddress("sales@asianpublishers.co.in");
                        mail.To.Add("smittal@asianpublishers.co.in");
                        mail.Subject = "New Order Received - OrderId is " + objectres.payInstrument.merchDetails.merchTxnId;
                        mail.Body = "Dear Asian Publishers Team," + Environment.NewLine +
                            "Order " + objectres.payInstrument.merchDetails.merchTxnId + " has been received on your website. Please take action as needed." + Environment.NewLine +
                             "Thank you.";

                        string id = objectres.payInstrument.merchDetails.merchTxnId;
                        byte[] pdfResult = Utility.OrderPdf(id);
                        MemoryStream pdfStream = new MemoryStream(pdfResult);
                        Attachment pdfAttachment = new Attachment(pdfStream, "order.pdf", MediaTypeNames.Application.Pdf);

                        mail.Attachments.Add(pdfAttachment);


                        SmtpServer.Port = 587;
                        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("sales@asianpublishers.co.in", "sa_753951");
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                        SmtpServer.Dispose();


                        ////Whatsapp
                        //var cliento = new RestClient("https://app.messageautosender.com/message/new");
                        //var requestNo = new RestRequest("", Method.Post);

                        //requestNo.AlwaysMultipartFormData = true;
                        //requestNo.AddParameter("username", "asianpublishers");
                        //requestNo.AddParameter("password", "Asian@#123");
                        //requestNo.AddParameter("receiverMobileNo", user.mobileNo);

                        //string msgo = "Dear " + user.name + ", " + Environment.NewLine
                        //    + "We are thrilled to inform you that your order has been successfully placed! Your OrderId is: " + objectres.payInstrument.merchDetails.merchTxnId +
                        //    "." + Environment.NewLine + "Thank you for choosing Asian Publishers. We're committed to providing you with top-quality " +
                        //    "service and products. If you have any questions or require assistance, feel free to reach out to us." + Environment.NewLine +
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
                        //    "Order " + objectres.payInstrument.merchDetails.merchTxnId + " has been received on your website. Please take action as needed." + Environment.NewLine +
                        //    "Thank you.";// + Environment.NewLine+
                        ////"Warm Regards," + Environment.NewLine +
                        ////"Team Marwari Software";

                        //requestN.AddParameter("message", msg);

                        //client.Execute(requestN);




                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return Redirect(Utility.ReactSuccessurl + "?message=" + HttpUtility.UrlEncode(message));
        }
    }
}
