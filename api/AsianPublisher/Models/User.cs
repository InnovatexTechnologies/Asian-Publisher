using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
    public class User
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public int count { get; set; } = 0;
        public string idPrefix { get; set; } = "";
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public string mobileNo { get; set; } = "";
        public string address { get; set; } = "";
        public string password { get; set; } = "";
        // Additional methods
        public static List<User> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
                    query = "SELECT id,name from Users";
                    return db.Query<User>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Users";
                    return db.Query<User>(query).OrderBy(o => o.name).ToList();
                }
                else
                {
                    query = "";
                    return db.Query<User>(query).OrderBy(o => o.name).ToList();
                }
            }
        }
        public static User GetById(string email, string passsword, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from Users WHERE id = @id";
                    return db.Query<User>(query, new { id = email }).FirstOrDefault() ?? new User();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT *,Count(*) as count from Users WHERE email = '" + email + "' and password = '" + passsword + "'";
                    return db.Query<User>(query, new { email = email ,password=passsword}).FirstOrDefault() ?? new User();
                }
                else
                {
                    query = "";
                    return db.Query<User>(query).FirstOrDefault() ?? new User();
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
                        numId = db.ExecuteScalar<int>("select Max(numId) from Users where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        string extension = "";
                        string sql = "INSERT INTO Users (id, numId, idPrefix, name, email, mobileNo, address,password) VALUES (@id, @numId, @idPrefix, @name, @email, @mobileNo, @address,@password)";
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
                        string sql = "UPDATE Users SET numId = @numId, idPrefix = @idPrefix, name = @name, email = @email, mobileNo = @mobileNo, address = @address,password=@password WHERE id = @id;";
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
                        string sql = "DELETE FROM Users WHERE id = @id;";
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
