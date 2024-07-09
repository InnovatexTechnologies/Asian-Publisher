using System.Data.SQLite;
using System.Data;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;


namespace AsianPublisher.Models
{
    public class OrderMeta
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public string idPrefix { get; set; } = "";
        public string orderId { get; set; } = "";
        public Order? orderNav { get; set; } 
        public string bookId { get; set; } = "";
        public Book? bookNav { get; set; }
        public decimal quantity { get; set; } = 0;
        public decimal price { get; set; } = 0;

        [NotMapped]
        public string LanguageId { get; set; } = "";
        public string BookName { get; set; } = "";
        public string BookLID { get; set; } = "";
        public string LanguageName { get; set; } = "";
        // Additional methods
        public static List<OrderMeta> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string whereClause = "1=1";
                if (paramList == null)
                {
                    paramList = new Dictionary<string, string>();
                }
                foreach (var obj in paramList)
                {
                    if (!string.IsNullOrEmpty(obj.Value))
                    {
                        if (obj.Key == "orderId")
                        {
                            whereClause += " and myBaseTable." + obj.Key + " = '" + obj.Value + "'";
                        }
                        else
                        {
                            whereClause += " and myBaseTable." + obj.Key + " = @" + obj.Key;
                        }
                    }
                }
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from OrderMetas";
                    return db.Query<OrderMeta>(query).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from OrderMetas";
                    return db.Query<OrderMeta>(query).ToList();
                } 
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*, Reff1.id AS OrderId, Reff1.name AS OrderName, " +
        "Reff2.id AS BookId,Reff2.languageid as BookLID , Reff2.name AS BookName, Reff3.name AS LanguageName, Reff3.id as LanguageId " +
        "FROM OrderMetas myBaseTable " +
        "LEFT JOIN Orders Reff1 ON myBaseTable.orderId = Reff1.id " +
        "LEFT JOIN Books Reff2 ON myBaseTable.bookId = Reff2.id " +
        "LEFT JOIN languages Reff3 ON Reff2.languageid = Reff3.id " +
        "WHERE " + whereClause;

                    return db.Query<OrderMeta, Order, Book, Language, OrderMeta>(query,
                    (myBaseTable, ref1, ref2, ref3) =>
                    {
                        myBaseTable.orderNav = ref1;
                        myBaseTable.bookNav = ref2;
                        myBaseTable.LanguageName = ref3.name;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.*,Reff2.* FROM OrderMetas myBaseTable left join Orders Reff1 on myBaseTable.orderId=Reff1.id left join Books Reff2 on myBaseTable.bookId=Reff2.id  where " + whereClause;
                    return db.Query<OrderMeta, Order, Book, OrderMeta>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.orderNav = ref1;
                        myBaseTable.bookNav = ref2;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).ToList();
                }
                else
                {
                    query = "SELECT myBaseTable.*, Reff1.id AS OrderId, Reff1.name AS OrderName, " +
        "Reff2.id AS BookId,Reff2.languageid as BookLID , Reff2.name AS BookName, Reff3.name AS LanguageName, Reff3.id as LanguageId " +
        "FROM OrderMetas myBaseTable " +
        "LEFT JOIN Orders Reff1 ON myBaseTable.orderId = Reff1.id " +
        "LEFT JOIN Books Reff2 ON myBaseTable.bookId = Reff2.id " +
        "LEFT JOIN languages Reff3 ON Reff2.languageid = Reff3.id " +
        "WHERE " + whereClause;
                    return db.Query<OrderMeta>(query).ToList();
                }
            }
        }
        
        public static OrderMeta GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from OrderMetas WHERE id = @id";
                    return db.Query<OrderMeta>(query, new { id = id }).FirstOrDefault() ?? new OrderMeta();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from OrderMetas WHERE orderId = @id";
                    return db.Query<OrderMeta>(query, new { id = id }).FirstOrDefault() ?? new OrderMeta();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name,Reff2.id, Reff2.name FROM OrderMetas myBaseTable left join Orders Reff1 on myBaseTable.orderId=Reff1.id left join Books Reff2 on myBaseTable.bookId=Reff2.id  WHERE myBaseTable.id = @id";
                    return db.Query<OrderMeta, Order, Book, OrderMeta>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.orderNav = ref1;
                        myBaseTable.bookNav = ref2;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new OrderMeta();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.*,Reff2.* FROM OrderMetas myBaseTable left join Orders Reff1 on myBaseTable.orderId=Reff1.id left join Books Reff2 on myBaseTable.bookId=Reff2.id  WHERE myBaseTable.id = @id";
                    return db.Query<OrderMeta, Order, Book, OrderMeta>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.orderNav = ref1;
                        myBaseTable.bookNav = ref2;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new OrderMeta();
                }
                else
                {
                    query = "";
                    return db.Query<OrderMeta>(query).FirstOrDefault() ?? new OrderMeta();
                }
            }
        }
        public bool Save()
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        idPrefix = "F";
                        numId = db.ExecuteScalar<int>("select Max(numId) from OrderMetas where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        string sql = "INSERT INTO OrderMetas (id, numId, idPrefix, orderId, bookId, quantity, price) VALUES (@id, @numId, @idPrefix, @orderId, @bookId, @quantity, @price)";
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
        public bool Update()
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sql = "UPDATE OrderMetas SET numId = @numId, idPrefix = @idPrefix, orderId = @orderId, bookId = @bookId, quantity = @quantity, price = @price WHERE id = @id;";
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
                        string sql = "DELETE FROM OrderMetas WHERE id = @id;";
                        int affectedRows = db.Execute(sql, new { id = this.id }, transaction);
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
