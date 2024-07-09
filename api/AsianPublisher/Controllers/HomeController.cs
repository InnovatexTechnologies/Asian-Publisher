using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AsianPublisher.Models;
using System.Data.SQLite;
using System.Data;
using Dapper;

namespace AsianPublisher.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {

                List<Order> records = db.Query<Order>("SELECT Orders.*, SUM(OrderMetas.price * OrderMetas.quantity) AS TotalAmount FROM Orders LEFT JOIN OrderMetas ON Orders.id = OrderMetas.orderId where isDispatch == 0 and status == 1 GROUP BY Orders.id").ToList();
                return View(records);
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "An error occurred while retrieving data from the database.");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
