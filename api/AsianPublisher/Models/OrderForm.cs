using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
    public class OrderForm
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public string idPrefix { get; set; } = "";
        public string name { get; set; } = "";
        public string address { get; set; } = "";
        public string mobileNo { get; set; } = "";
        public string city { get; set; } = "";
        public string email { get; set; } = "";
        public string description { get; set; } = "";
        public decimal quantity { get; set; } = 0;
        public string bookId { get; set; } = "";
        public Book? bookNav { get; set; } = new Book();
        // Additional methods
        public static List<OrderForm> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
                        whereClause += " and myBaseTable." + obj.Key + " = @" + obj.Key;
                    }
                }
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from OrderForms";
                    return db.Query<OrderForm>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from OrderForms";
                    return db.Query<OrderForm>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name FROM OrderForms myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id  where " + whereClause;
                    return db.Query<OrderForm, Book, OrderForm>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.bookNav = ref1;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.* FROM OrderForms myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id  where " + whereClause;
                    return db.Query<OrderForm, Book, OrderForm>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.bookNav = ref1;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).OrderBy(o => o.name).ToList();
                }
                else
                {
                    query = "";
                    return db.Query<OrderForm>(query).OrderBy(o => o.name).ToList();
                }
            }
        }
        public static OrderForm GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from OrderForms WHERE id = @id";
                    return db.Query<OrderForm>(query, new { id = id }).FirstOrDefault() ?? new OrderForm();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from OrderForms WHERE id = @id";
                    return db.Query<OrderForm>(query, new { id = id }).FirstOrDefault() ?? new OrderForm();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name FROM OrderForms myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id  WHERE myBaseTable.id = @id";
                    return db.Query<OrderForm, Book, OrderForm>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.bookNav = ref1;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new OrderForm();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.* FROM OrderForms myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id  WHERE myBaseTable.id = @id";
                    return db.Query<OrderForm, Book, OrderForm>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.bookNav = ref1;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new OrderForm();
                }
                else
                {
                    query = "";
                    return db.Query<OrderForm>(query).FirstOrDefault() ?? new OrderForm();
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
                        numId = db.ExecuteScalar<int>("select Max(numId) from OrderForms where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        string extension = "";
                        string sql = "INSERT INTO OrderForms (id, numId, idPrefix, name, address, mobileNo, description, city, email, quantity, bookId, mobileNo) VALUES (@id, @numId, @idPrefix, @name, @address, @mobileNo, @description, @city, @email, @quantity, @bookId, @mobileNo)";
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
                        string extension = "";
                        string sql = "UPDATE OrderForms SET numId = @numId, idPrefix = @idPrefix, name = @name, address = @address, mobileNo = @mobileNo, description = @description, city = @city, email = @email, quantity = @quantity, bookId = @bookId, mobileNo=@mobileNo WHERE id = @id;";
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
                        string sql = "DELETE FROM OrderForms WHERE id = @id;";
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
