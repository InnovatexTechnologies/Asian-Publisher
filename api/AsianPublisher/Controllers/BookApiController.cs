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
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class BookApiController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<BookApiController> _logger;

        public BookApiController(IConfiguration _configuration, ILogger<BookApiController> logger, IWebHostEnvironment _webHost)
        {
            configuration = _configuration;
            _logger = logger;
            webHost = _webHost;
        }
        // List
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                //string Author = Request.Headers.ContainsKey("Author") ? Request.Headers["Author"].FirstOrDefault() : "";
                //string Course = Request.Headers.ContainsKey("Course") ? Request.Headers["Course"].FirstOrDefault() : "";
                //string Semester = Request.Headers.ContainsKey("Semester") ? Request.Headers["Semester"].FirstOrDefault() : "";
                string[] Authors = Request.Headers.ContainsKey("Author") ? Request.Headers.GetCommaSeparatedValues("Author") : new string[0];
                string Author = string.Join(",", Authors);
                string[] Courses = Request.Headers.ContainsKey("Course") ? Request.Headers.GetCommaSeparatedValues("Course") : new string[0];
                string Course = string.Join(",", Courses);
                string[] Semesters = Request.Headers.ContainsKey("Semester") ? Request.Headers.GetCommaSeparatedValues("Semester") : new string[0];
                string Semester = string.Join(",", Semesters);

                string Filter = Request.Headers.ContainsKey("orderFilter") ? Request.Headers["orderFilter"].FirstOrDefault() : "";
                string name = Request.Headers.ContainsKey("search") ? Request.Headers["search"].FirstOrDefault() : "";

                Dictionary<string, string> paramlist = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(Author))
                {
                    paramlist.Add("ba.authorId", Author);
                }
                if (!string.IsNullOrEmpty(Course))
                {
                    paramlist.Add("c.courseId", Course);
                }
                if (!string.IsNullOrEmpty(Semester))
                {
                    paramlist.Add("c.semesterId", Semester);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    string namePattern = "%" + name + "%";
                    paramlist.Add("name", namePattern);
                }
                if (!string.IsNullOrEmpty(Filter))
                {
                    if (Filter.Contains("titleAscending"))
                    {
                        paramlist.Add("orderbytitle", Filter);
                    }
                    else if (Filter.Contains("priceAscending"))
                    {
                        paramlist.Add("orderbyprice", Filter);
                    }
                    else if (Filter.Contains("titleDescending"))
                    {
                        paramlist.Add("orderbytitledesc", Filter);
                    }
                    else if (Filter.Contains("priceDescending"))
                    {
                        paramlist.Add("orderbypricedesc", Filter);
                    }
                }
                List<Book> records = Book.Get(Utility.FillStyle.Custom, paramlist).ToList();
                if (string.IsNullOrEmpty(Filter))
                {
                    records = records.OrderBy(o => o.name).ToList();
                }
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // List
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                Book records = Book.GetById(id, Utility.FillStyle.Custom);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Post(Book model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        model.idPrefix = "F";
                        model.numId = db.ExecuteScalar<int>("select Max(numId) from Books where idPrefix = 'F'") + 1;
                        model.id = model.idPrefix + model.numId;
                        string sql = "INSERT INTO Books (id, numId, idPrefix, name, mRP, bookCode, iSBN, languageId, samplePdf, image, isFeatured,description) VALUES (@id, @numId, @idPrefix, @name, @mRP, @bookCode, @iSBN, @languageId, @samplePdf, @image, @isFeatured,@description)";
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
        public IActionResult Put(Book model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "update Books set name=@name ,mRP=@mRP ,bookCode=@bookCode ,iSBN=@iSBN ,languageId=@languageId ,samplePdf=@samplePdf,image=@image,isFeatured=@isFeatured,description=@description  where id = @id";
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
        public IActionResult Delete(Book model)
        { 
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try 
                    {
                        string sql = "DELETE FROM Books WHERE id = @id;";
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
