using System.Data.SQLite;
using System.Data;
using Dapper;
using System.Data.Common;


namespace AsianPublisher.Models
{
    public class Order
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public string courierName { get; set; } = "";
        public string docketNo { get; set; } = "";
        public int docketDate { get; set; } = 0;
        public int date { get; set; } = 0;
        public string dateNew => FormatDate(date);
        private string FormatDate(int dateValue)
        {
            if (dateValue == 0)
            {
                return ""; // Or any other default value you prefer
            }

            string dateString = dateValue.ToString("00000000"); // Pad with leading zeros if needed
            DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", null);
            return date.ToString("dd-MMM-yyyy");
        }
        public int time { get; set; } = 0;
        public string timeNew => FormatTime(time);
        private string FormatTime(int timeValue)
        {
            string timeString = timeValue.ToString("000000000");
            DateTime time = DateTime.ParseExact(timeString, "HHmmssfff", null);
            return time.ToString("HH:mm:ss");
        }
        public int status { get; set; } = 0;
        public string statusN { get; set; } = "";
        public string userId { get; set; } = "";
        public string merchId { get; set; } = "";
        public string idPrefix { get; set; } = "";
        public string tokenId { get; set; } = "";
        public int isDispatch { get; set; } = 0;
        public string isDispatchN { get; set; } = "";
        public string name { get; set; } = "";
        public string city { get; set; } = "";
        public string email { get; set; } = "";
        public string address { get; set; } = "";
        public string state { get; set; } = "";
        public string mobileNo { get; set; } = "";
        public string country { get; set; } = "";
        public string pincode { get; set; } = "";
        public List<OrderMeta>? orderMetas { get; set; } = new List<OrderMeta>();
        public decimal TotalAmount { get; set; } = 0;

        public static List<Order> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string whereClause = "1=1";
                if (paramList == null)
                {
                    paramList = new Dictionary<string, string>();
                }

                // Default last 30 days date calculation
                var currentDate = DateTime.Now;
                var past30DaysDate = currentDate.AddDays(-30);

                // Check and apply each filter if provided
                if (paramList.ContainsKey("fromDate") && DateTime.TryParseExact(paramList["fromDate"], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime fromDate))
                {
                    whereClause += $" and Orders.date >= {fromDate:yyyyMMdd}";
                }
                else
                {
                    whereClause += $" and Orders.date >= {past30DaysDate:yyyyMMdd}";
                }

                if (paramList.ContainsKey("toDate") && DateTime.TryParseExact(paramList["toDate"], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime toDate))
                {
                    whereClause += $" and Orders.date <= {toDate:yyyyMMdd}";
                }
                else
                {
                    whereClause += $" and Orders.date <= {currentDate:yyyyMMdd}";
                }

                if (paramList.ContainsKey("status") && int.TryParse(paramList["status"], out int status))
                {
                    whereClause += $" and Orders.status = {status}";
                }

                if (paramList.ContainsKey("isDispatch") && int.TryParse(paramList["isDispatch"], out int isDispatch))
                {
                    whereClause += $" and Orders.isDispatch = {isDispatch}";
                }

                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id, name FROM Orders WHERE " + whereClause;
                    return db.Query<Order>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT Orders.*, SUM(OrderMetas.price * OrderMetas.quantity) AS TotalAmount FROM Orders LEFT JOIN OrderMetas ON Orders.id = OrderMetas.orderId WHERE " + whereClause + " GROUP BY Orders.id";
                    return db.Query<Order>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.* FROM Orders myBaseTable WHERE " + whereClause;
                    return db.Query<Order>(query, Utility.CreateDynamicObject(paramList)).OrderBy(o => o.name).ToList();
                }
                else
                {
                    query = @"SELECT myBaseTable.*, O.*
                      FROM Orders myBaseTable
                      LEFT JOIN OrderMetas O ON myBaseTable.Id = O.OrderId
                      WHERE " + whereClause;

                    var ordersDictionary = new Dictionary<string, Order>();

                    var result = db.Query<Order, OrderMeta, Order>(
                        query,
                        (order, orderMeta) =>
                        {
                            if (!ordersDictionary.TryGetValue(order.id, out var currentOrder))
                            {
                                currentOrder = order;
                                currentOrder.orderMetas = new List<OrderMeta>();
                                ordersDictionary.Add(currentOrder.id, currentOrder);
                            }

                            if (orderMeta != null)
                            {
                                orderMeta.orderNav = currentOrder;
                                currentOrder.orderMetas.Add(orderMeta);
                            }

                            return currentOrder;
                        })
                        .Distinct()
                        .ToList();

                    return result;
                }
            }
        }

        public static Order GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from Orders WHERE id = @id";
                    return db.Query<Order>(query, new { id = id }).FirstOrDefault() ?? new Order();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Orders WHERE id = @id";
                    return db.Query<Order>(query, new { id = id }).FirstOrDefault() ?? new Order();
                }
                else
                {
                    query = "";
                    return db.Query<Order>(query).FirstOrDefault() ?? new Order();
                }
            }
        }
        public bool Save(string docketDate)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        idPrefix = "F";
                        numId = db.ExecuteScalar<int>("select Max(numId) from Orders where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        //tokenId = Utility.Tok_id;
                        string extension = "";
                        string sql = "INSERT INTO Orders (id, numId, idPrefix, name, email, address,docketDate,docketNo,courierName, city, state,isDispatch, country, pincode, mobileNo,date,time,tokenId,merchId,status,userId) VALUES (@id, @numId, @idPrefix, @name, @email, @address,@docketDate,@docketNo,@courierName, @city, @state,0, @country, @pincode, @mobileNo,@date,@time,@tokenId,@merchId,@status,@userId)";
                        int affectedRows = db.Execute(sql, this, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        public bool Update(string docketDate)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string extension = "";
                        string sql = "UPDATE Orders SET numId = @numId, idPrefix = @idPrefix, name = @name, email = @email, address = @address,docketDate=@docketDate,docketNo=@docketNo,courierName=@courierName, city = @city,state = @state,isDispatch=@isDispatch, country=@country, pincode=@pincode, mobileNo = @mobileNo,date=@date,time=@time, tokenId=@tokenId ,merchId=@merchId,status=@status,userId=@userId WHERE id = @id;";
                        int affectedRows = db.Execute(sql, this, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        public string Delete()
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        int affectedRowsOrderMetas = db.Execute("DELETE FROM OrderMetas WHERE orderId = @orderId", new { orderId = this.id }, transaction);
                        int affectedRowsOrders = db.Execute("DELETE FROM Orders WHERE id = @id", new { id = this.id }, transaction);
                        transaction.Commit();
                        return "true";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return ex.Message;
                    }
                }
            }
        }
    }
}
