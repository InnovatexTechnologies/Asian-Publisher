using System.Data.SQLite;
using System.Data;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using static System.Reflection.Metadata.BlobBuilder;


namespace AsianPublisher.Models
{
    public class BookAuthor
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public string idPrefix { get; set; } = "";
        public string bookId { get; set; } = "";
        public Book bookNav { get; set; }
        public string authorId { get; set; } = "";
        public string name { get; set; } = "";
        public int BookCount { get; set; } = 0;
        public Author authorNav { get; set; }
        // Additional methods
        public static List<BookAuthor> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
                    query = "SELECT id,name from BookAuthors";
                    return db.Query<BookAuthor>(query).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from BookAuthors";
                    return db.Query<BookAuthor>(query).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name,Reff2.id, Reff2.name FROM BookAuthors myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id left join Authors Reff2 on myBaseTable.authorId=Reff2.id  where " + whereClause;
                    return db.Query<BookAuthor, Book, Author, BookAuthor>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.bookNav = ref1;
                        myBaseTable.authorNav = ref2;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.*,Reff2.* FROM BookAuthors myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id left join Authors Reff2 on myBaseTable.authorId=Reff2.id  where " + whereClause;
                    return db.Query<BookAuthor, Book, Author, BookAuthor>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.bookNav = ref1;
                        myBaseTable.authorNav = ref2;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).ToList();
                }
                else
                {
                    query = @"
                    SELECT BookAuthors.id,COUNT(BookAuthors.id) AS BookCount ,Authors.*,Authors.id, Authors.name,Authors.content,Authors.content2
                    FROM Authors 
                    JOIN BookAuthors ON Authors.id = BookAuthors.authorId 
                    left join Books on BookAuthors.bookId = Books.id 
                    GROUP BY Authors.id, Authors.name";

                    var results = db.Query<BookAuthor, Author, Book, BookAuthor>(
                    query,
                    (bookAuthor, author, book) =>
                    {
                        bookAuthor.authorNav = author;
                        bookAuthor.bookNav = book;
                        return bookAuthor;
                    }
                    );

                    return results.ToList();
                    //query = " select Authors.*, Authors.name,count(BookAuthors.id) as BookCount from Authors  " +
                    //        " join BookAuthors on Authors.id = BookAuthors.authorId GROUP BY Authors.id, Authors.name";
                    //return db.Query<BookAuthor>(query).ToList();
                }
            }
        }
        public static BookAuthor GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from BookAuthors WHERE id = @id";
                    return db.Query<BookAuthor>(query, new { id = id }).FirstOrDefault() ?? new BookAuthor();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from BookAuthors WHERE id = @id";
                    return db.Query<BookAuthor>(query, new { id = id }).FirstOrDefault() ?? new BookAuthor();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name,Reff2.id, Reff2.name FROM BookAuthors myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id left join Authors Reff2 on myBaseTable.authorId=Reff2.id  WHERE myBaseTable.id = @id";
                    return db.Query<BookAuthor, Book, Author, BookAuthor>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.bookNav = ref1;
                        myBaseTable.authorNav = ref2;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new BookAuthor();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.*,Reff2.* FROM BookAuthors myBaseTable left join Books Reff1 on myBaseTable.bookId=Reff1.id left join Authors Reff2 on myBaseTable.authorId=Reff2.id  WHERE myBaseTable.id = @id";
                    return db.Query<BookAuthor, Book, Author, BookAuthor>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.bookNav = ref1;
                        myBaseTable.authorNav = ref2;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new BookAuthor();
                }
                else
                {
                    query = "";
                    return db.Query<BookAuthor>(query).FirstOrDefault() ?? new BookAuthor();
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
                        numId = db.ExecuteScalar<int>("select Max(numId) from BookAuthors where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        string extension = "";
                        string sql = "INSERT INTO BookAuthors (id, numId, idPrefix, bookId, authorId) VALUES (@id, @numId, @idPrefix, @bookId, @authorId)";
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
                        string sql = "UPDATE BookAuthors SET numId = @numId, idPrefix = @idPrefix, bookId = @bookId, authorId = @authorId WHERE id = @id;";
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
                        string sql = "DELETE FROM BookAuthors WHERE id = @id;";
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
