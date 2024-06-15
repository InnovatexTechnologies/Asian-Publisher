using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
    public class Semester
    {
        public string id { get; set; } = "";
        public int numId { get; set; } = 0;
        public string idPrefix { get; set; } = "";
        public string name { get; set; } = "";
        public string alias { get; set; } = "";
        public string academicId { get; set; } = "";
        public string type => "Semester";
        public AcademicYear academicNav { get; set; }
        public string semesterCategoryId { get; set; } = "";
        public SemesterCategory semesterNav { get; set; }
        // Additional methods
        public static List<Semester> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
                    query = "SELECT id,name from Semesters";
                    return db.Query<Semester>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Semesters";
                    return db.Query<Semester>(query).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name,Reff2.id, Reff2.name FROM Semesters myBaseTable left join AcademicYears Reff1 on myBaseTable.academicId=Reff1.id left join SemesterCategories Reff2 on myBaseTable.semesterCategoryId=Reff2.id  where " + whereClause;
                    return db.Query<Semester, AcademicYear, SemesterCategory, Semester>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.academicNav = ref1;
                        myBaseTable.semesterNav = ref2;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).OrderBy(o => o.name).ToList();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.*,Reff2.* FROM Semesters myBaseTable left join AcademicYears Reff1 on myBaseTable.academicId=Reff1.id left join SemesterCategories Reff2 on myBaseTable.semesterCategoryId=Reff2.id  where " + whereClause;
                    return db.Query<Semester, AcademicYear, SemesterCategory, Semester>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.academicNav = ref1;
                        myBaseTable.semesterNav = ref2;
                        return myBaseTable;
                    }, Utility.CreateDynamicObject(paramList)).OrderBy(o => o.name).ToList();
                }
                else
                {
                    query = "";
                    return db.Query<Semester>(query).OrderBy(o => o.name).ToList();
                }
            }
        }
        public static Semester GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
        {
            using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
            {
                db.Open();
                string query = "";
                if (fillStyle == Utility.FillStyle.Basic)
                {
                    query = "SELECT id,name from Semesters WHERE id = @id";
                    return db.Query<Semester>(query, new { id = id }).FirstOrDefault() ?? new Semester();
                }
                else if (fillStyle == Utility.FillStyle.AllProperties)
                {
                    query = "SELECT * from Semesters WHERE id = @id";
                    return db.Query<Semester>(query, new { id = id }).FirstOrDefault() ?? new Semester();
                }
                else if (fillStyle == Utility.FillStyle.WithBasicNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.id, Reff1.name,Reff2.id, Reff2.name FROM Semesters myBaseTable left join AcademicYears Reff1 on myBaseTable.academicId=Reff1.id left join SemesterCategories Reff2 on myBaseTable.semesterCategoryId=Reff2.id  WHERE myBaseTable.id = @id";
                    return db.Query<Semester, AcademicYear, SemesterCategory, Semester>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.academicNav = ref1;
                        myBaseTable.semesterNav = ref2;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new Semester();
                }
                else if (fillStyle == Utility.FillStyle.WithFullNav)
                {
                    query = "SELECT myBaseTable.*,Reff1.*,Reff2.* FROM Semesters myBaseTable left join AcademicYears Reff1 on myBaseTable.academicId=Reff1.id left join SemesterCategories Reff2 on myBaseTable.semesterCategoryId=Reff2.id  WHERE myBaseTable.id = @id";
                    return db.Query<Semester, AcademicYear, SemesterCategory, Semester>(query,
                    (myBaseTable, ref1, ref2) =>
                    {
                        myBaseTable.academicNav = ref1;
                        myBaseTable.semesterNav = ref2;
                        return myBaseTable;
                    }, new { id = id }).FirstOrDefault() ?? new Semester();
                }
                else
                {
                    query = "";
                    return db.Query<Semester>(query).FirstOrDefault() ?? new Semester();
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
                        numId = db.ExecuteScalar<int>("select Max(numId) from Semesters where idPrefix = 'F'") + 1;
                        id = idPrefix + numId;
                        string extension = "";
                        string sql = "INSERT INTO Semesters (id, numId, idPrefix, name, alias, academicId, semesterCategoryId) VALUES (@id, @numId, @idPrefix, @name, @alias, @academicId, @semesterCategoryId)";
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
                        string sql = "UPDATE Semesters SET numId = @numId, idPrefix = @idPrefix, name = @name, alias = @alias, academicId = @academicId, semesterCategoryId = @semesterCategoryId WHERE id = @id;";
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
                        string sql = "DELETE FROM Semesters WHERE id = @id;";
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
