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

namespace AsianPublisher.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class BookAuthorApiController : ControllerBase
    {
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<BookAuthorApiController> _logger;

        public BookAuthorApiController(IConfiguration _configuration, ILogger<BookAuthorApiController> logger, IWebHostEnvironment _webHost)
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
                List<BookAuthor> records = BookAuthor.Get(Utility.FillStyle.Custom);
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
                BookAuthor records = BookAuthor.GetById(id, Utility.FillStyle.AllProperties);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving data from the database.");
            }
        }
        // POST: Create a new record
        [HttpPost]
        public IActionResult Post(BookAuthor model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        model.idPrefix = "F";
                        model.numId = db.ExecuteScalar<int>("select Max(numId) from BookAuthors where idPrefix = 'F'") + 1;
                        model.id = model.idPrefix + model.numId;
                        string sql = "INSERT INTO BookAuthors (id, numId, idPrefix, bookId, authorId) VALUES (@id, @numId, @idPrefix, @bookId, @authorId)";
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
        public IActionResult Put(BookAuthor model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "update BookAuthors set bookId=@bookId ,authorId=@authorId  where id = @id";
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
        public IActionResult Delete(BookAuthor model)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "DELETE FROM BookAuthors WHERE id = @id;";
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
