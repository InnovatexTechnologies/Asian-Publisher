using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
    public class Author
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public string idPrefix { get; set; } = "";
        public string name { get; set; } = "";
        public string mobileNo { get; set; } = "";
        public string code { get; set; } = "";
        public string emailId { get; set; } = "";
        public string content { get; set; } = "";
        public string content2 { get; set; } = "";
        public string type => "Author";
        // Additional methods
        public static List<Author> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
                    query = "SELECT id,name from Authors";
                    return db.Query<Author>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Authors";
                    return db.Query<Author>(query).OrderBy(o => o.name).ToList();
                }
                else
                {
                    query = "";
                    return db.Query<Author>(query).OrderBy(o => o.name).ToList();
                }
            }
        }
        public static Author GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from Authors WHERE id = @id";
                    return db.Query<Author>(query, new { id = id }).FirstOrDefault() ?? new Author();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Authors WHERE id = @id";
                    return db.Query<Author>(query, new { id = id }).FirstOrDefault() ?? new Author();
                }
                else
                {
                    query = "";
                    return db.Query<Author>(query).FirstOrDefault() ?? new Author();
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
                        numId = db.ExecuteScalar<int>("select Max(numId) from Authors where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        string extension = "";
                        string sql = "INSERT INTO Authors (id, numId, idPrefix, name, mobileNo, emailId, content, content2, code) VALUES (@id, @numId, @idPrefix, @name, @mobileNo, @emailId, @content, @content2, @code)";
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
                        string sql = "UPDATE Authors SET numId = @numId, idPrefix = @idPrefix, name = @name, mobileNo = @mobileNo, emailId = @emailId, content = @content, content2 = @content2 , code=@code WHERE id = @id;";
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
                        string sql = "DELETE FROM Authors WHERE id = @id;";
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
