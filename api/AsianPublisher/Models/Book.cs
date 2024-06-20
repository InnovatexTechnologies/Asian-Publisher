using System.Data.SQLite;
using System.Data;
using Dapper;
using iTextSharp.text.pdf;


namespace AsianPublisher.Models
{
    public class Book
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public string idPrefix { get; set; } = "";
        public string name { get; set; } = "";
        public decimal mRP { get; set; } = 0;
        public string bookCode { get; set; } = "";
        public string description { get; set; } = "";
        public string iSBN { get; set; } = "";
        public string languageId { get; set; } = "";
        public bool isFeatured { get; set; }
        public Language? languageNav { get; set; }
        public string image { get; set; } = "";
        public string relatedBooks { get; set; } = "";
        public string samplePdf { get; set; } = "";
        public List<Author>? authors { get; set; }
        public List<CourseSemester>? courseSemesters { get; set; }
        // Additional methods
        public static List<Book> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
                        if (obj.Key == "name")
                        {
                            //whereClause += " and myBaseTable." + obj.Key + " like '%@" + obj.Key+"%'";
                            whereClause += " and myBaseTable." + obj.Key + " like @" + obj.Key;
                        }
                        else if (obj.Key.Contains("orderbytitledesc"))
                        {
                            whereClause += " order by myBaseTable.name desc";
                        }
                        else if (obj.Key.Contains("orderbypricedesc"))
                        {
                            whereClause += " order by myBaseTable.mRP desc";
                        }
                        else if (obj.Key.Contains("orderbytitle"))
                        {
                            whereClause += " order by myBaseTable.name";
                        }
                        else if (obj.Key.Contains("orderbyprice"))
                        {
                            whereClause += " order by myBaseTable.mRP";
                        }
                        else if (obj.Key.Contains("."))
                        {
                            if (obj.Value.Contains(","))
                            {
                                string[] arr = obj.Value.Split(',');
                                whereClause += " and ( ";
                                foreach (var b in arr)
                                {
                                    whereClause += " " + obj.Key + " = '" + b + "' or";
                                }

                                whereClause = whereClause.Substring(0, whereClause.Length - 2);
                                whereClause += " ) ";
                            }
                            else
                            {
                                whereClause += " and " + obj.Key + " = '" + obj.Value + "'";
                            }
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
                    query = "SELECT id,name from Books";
                    return db.Query<Book>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Books";
                    return db.Query<Book>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name FROM Books myBaseTable left join Languages Reff1 on myBaseTable.languageId=Reff1.id  where " + whereClause;
                    return db.Query<Book, Language, Book>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.languageNav = ref1;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.* FROM Books myBaseTable left join Languages Reff1 on myBaseTable.languageId=Reff1.id  where " + whereClause;
                    return db.Query<Book, Language, Book>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.languageNav = ref1;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).OrderBy(o => o.name).ToList();
                }
                else
                {
                    query = @"SELECT myBaseTable.*, l.id , l.name , a.id , a.name 
                            FROM Books myBaseTable
                            LEFT JOIN Languages l ON myBaseTable.languageId = l.id
                            LEFT JOIN BookAuthors ba ON myBaseTable.id = ba.bookId
                            LEFT JOIN Authors a ON ba.authorId = a.id
                            LEFT JOIN Catalogues c on myBaseTable.id = c.bookId
                            WHERE " + whereClause;
                            //WHERE " + whereClause + " order by c.semesterId";

                    var lookup = new Dictionary<string, Book>();

                    var result = db.Query<Book, Language, Author, Book>(query,
                        (book, language, author) =>
                        {
                            if (!lookup.TryGetValue(book.id, out var currentBook))
                            {
                                currentBook = book;
                                currentBook.languageNav = language;
                                //currentBook.authors = new List<Author>();
                                lookup.Add(currentBook.id, currentBook);
                            }

                            //if (author != null && !currentBook.authors.Any(a => a.id == author.id))
                            //    currentBook.authors.Add(author);
                            //currentBook.authors.Add(author);

                            return currentBook;
                        },
                        param: Utility.CreateDynamicObject(paramList));

                    //List<Book> books = lookup.Values.OrderBy(o => o.name).ToList();
                    List<Book> books = lookup.Values.ToList();

                    List<Catalogue> catalogues = Catalogue.Get(Utility.FillStyle.WithFullNav);

                    foreach (Book book in books)
                    {
                        List<Catalogue> thisBookcatalogues = catalogues.Where(o => o.bookId == book.id).ToList();

                        string authorsQuery = @"SELECT a.id, a.name
        FROM Authors a
        LEFT JOIN BookAuthors ba ON a.id = ba.authorId
        WHERE ba.bookId = @BookId";
                        List<Author> authors = db.Query<Author>(authorsQuery, new { BookId = book.id }).ToList();

                        // Add authors to the current book
                        book.authors = new List<Author>();                        
                        book.authors = authors;

                        book.courseSemesters = new List<CourseSemester>(); 
                        foreach (Catalogue catalogue in thisBookcatalogues)
                        {
                            book.courseSemesters.Add(new CourseSemester() { courseId = catalogue.courseNav.id, courseName = catalogue.courseNav.alias, courseType = catalogue.courseNav.type, semesterId = catalogue.semesterNav.id, semesterName = catalogue.semesterNav.alias, semesterType = catalogue.semesterNav.type });
                        }
                    }
                    return books;
                }
            }
        }
        public static Book GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from Books WHERE id = @id";
                    return db.Query<Book>(query, new { id = id }).FirstOrDefault() ?? new Book();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Books WHERE id = @id";
                    return db.Query<Book>(query, new { id = id }).FirstOrDefault() ?? new Book();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name FROM Books myBaseTable left join Languages Reff1 on myBaseTable.languageId=Reff1.id  WHERE myBaseTable.id = @id";
                    return db.Query<Book, Language, Book>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.languageNav = ref1;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new Book();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.* FROM Books myBaseTable left join Languages Reff1 on myBaseTable.languageId=Reff1.id  WHERE myBaseTable.id = @id";
                    return db.Query<Book, Language, Book>(query,
                    (myBaseTable, ref1) =>
                    {
                        myBaseTable.languageNav = ref1;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new Book();
                }
                else
                {
                    query = @"SELECT myBaseTable.*, l.id , l.name , a.id , a.name 
                            FROM Books myBaseTable
                            LEFT JOIN Languages l ON myBaseTable.languageId = l.id
                            LEFT JOIN BookAuthors ba ON myBaseTable.id = ba.bookId
                            LEFT JOIN Authors a ON ba.authorId = a.id
                            WHERE myBaseTable.id = @id";

                    var lookup = new Dictionary<string, Book>();

                    var result = db.Query<Book, Language, Author, Book>(query,
                        (book, language, author) =>
                        {
                            if (!lookup.TryGetValue(book.id, out var currentBook))
                            {
                                currentBook = book;
                                currentBook.languageNav = language;
                                //currentBook.authors = new List<Author>();
                                lookup.Add(currentBook.id, currentBook);
                            }

                            //if (author != null)
                            //    currentBook.authors.Add(author);

                            return currentBook;
                        },
                        param: new { id = id }).FirstOrDefault() ?? new Book();

                    List<Book> books = lookup.Values.OrderBy(o => o.name).ToList();

                    List<Catalogue> catalogues = Catalogue.Get(Utility.FillStyle.WithFullNav);

                    foreach (Book book in books)
                    {
                        List<Catalogue> thisBookcatalogues = catalogues.Where(o => o.bookId == book.id).ToList();
                        string authorsQuery = @"SELECT a.id, a.name
        FROM Authors a
        LEFT JOIN BookAuthors ba ON a.id = ba.authorId
        WHERE ba.bookId = @BookId";
                        List<Author> authors = db.Query<Author>(authorsQuery, new { BookId = book.id }).ToList();

                        // Add authors to the current book
                        book.authors = new List<Author>();
                        book.authors = authors;
                        book.courseSemesters = new List<CourseSemester>();
                        foreach (Catalogue catalogue in thisBookcatalogues)
                        {
                            book.courseSemesters.Add(new CourseSemester() { courseId = catalogue.courseNav.id, courseName = catalogue.courseNav.alias, courseType = catalogue.courseNav.type, semesterId = catalogue.semesterNav.id, semesterName = catalogue.semesterNav.alias, semesterType = catalogue.semesterNav.type });
                        }
                    }



                    return books.FirstOrDefault() ?? new Book();
                    //query = "";
                    //return db.Query<Book>(query).FirstOrDefault() ?? new Book();
                }
            }
        }
        public bool Save(IFormFile samplePdfFile, IFormFile image, string path)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        idPrefix = "F";
                        numId = db.ExecuteScalar<int>("select Max(numId) from Books where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        string extension = "";
                        if (samplePdfFile != null)
                        {
                            extension = Path.GetExtension(samplePdfFile.FileName);
                            if (extension == ".pdf")
                            {
                                string uploadsFolder = Path.Combine(path, "Pdf");
                                string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + " - " + samplePdfFile.FileName;
                                this.samplePdf = uniqueFileName;
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    samplePdfFile.CopyTo(stream);
                                }
                            }
                        }
                        if (image != null)
                        {
                            extension = Path.GetExtension(image.FileName);
                            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                            {
                                string uploadsFolder = Path.Combine(path, "Image");
                                string uniqueImageName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + " - " + image.FileName;
                                this.image = uniqueImageName;
                                string imagePath = Path.Combine(uploadsFolder, uniqueImageName);
                                using (var stream = new FileStream(imagePath, FileMode.Create))
                                {
                                    image.CopyTo(stream);
                                }
                            }
                        }
                        string sql = "INSERT INTO Books (id, numId, idPrefix, name, mRP, bookCode, iSBN, languageId, samplePdf, image, isFeatured,description,relatedBooks) VALUES (@id, @numId, @idPrefix, @name, @mRP, @bookCode, @iSBN, @languageId, @samplePdf , @image, @isFeatured,@description,@relatedBooks)";
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
        public bool Update(IFormFile samplePdfFile, IFormFile image, string path)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string extension = "";
                        if (samplePdfFile != null)
                        {
                            extension = Path.GetExtension(samplePdfFile.FileName);
                            if (extension == ".pdf")
                            {
                                string filename = (path + "/Pdf/" + this.samplePdf).Replace('/', '\\');
                                FileInfo FL = new FileInfo(filename);
                                if (FL.Exists)
                                {
                                    FL.Delete();
                                }
                                string uploadsFolder = Path.Combine(path, "Pdf");
                                string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + " - " + samplePdfFile.FileName;
                                this.samplePdf = uniqueFileName;
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    samplePdfFile.CopyTo(stream);
                                }
                            }
                        }
                        if (image != null)
                        {
                            extension = Path.GetExtension(image.FileName);
                            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                            {
                                string filename = (path + "/Image/" + this.image).Replace('/', '\\');
                                FileInfo FL = new FileInfo(filename);
                                if (FL.Exists)
                                {
                                    FL.Delete();
                                }
                                string uploadsFolder = Path.Combine(path, "Image");
                                string uniqueImageName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + " - " + image.FileName;
                                this.image = uniqueImageName;
                                string ImagePath = Path.Combine(uploadsFolder, uniqueImageName);
                                using (var stream = new FileStream(ImagePath, FileMode.Create))
                                {
                                    image.CopyTo(stream);
                                }
                            }
                        }
                        string sql = "UPDATE Books SET numId = @numId, idPrefix = @idPrefix, name = @name, mRP = @mRP, bookCode = @bookCode, iSBN = @iSBN, languageId = @languageId, samplePdf = @samplePdf, image = @image,isFeatured=@isFeatured,description=@description,relatedBooks=@relatedBooks WHERE id = @id;";
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
        public string Delete(string samplePdfFile, string image, string path)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            if (samplePdf != null)
                            {
                                string filename = (path + "/Pdf/" + samplePdfFile).Replace('/', '\\');
                                FileInfo FL = new FileInfo(filename);
                                if (FL.Exists)
                                {
                                    FL.Delete();
                                }
                            }
                            if (image != null)
                            {
                                string filename = (path + "/Image/" + image).Replace('/', '\\');
                                FileInfo FL = new FileInfo(filename);
                                if (FL.Exists)
                                {
                                    FL.Delete();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        string sql = "DELETE FROM Books WHERE id = @id;";
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
