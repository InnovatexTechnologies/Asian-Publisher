using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
	public class Catalogue
	{
		public string id { get; set; } = "";
		public int numId { get; set; }= 0; 
		public string idPrefix { get; set; } = "";
		public string courseId { get; set; } = "";
		public Course courseNav { get; set; } 
		public string semesterId { get; set; } = "";
		public Semester semesterNav { get; set; } 
		public string bookId { get; set; } = "";
		public Book bookNav { get; set; } 
		public string code { get; set; } = "";
	// Additional methods
	public static List<Catalogue> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
				query = "SELECT id,name from Catalogues";
			return db.Query<Catalogue>(query).ToList();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from Catalogues";
			return db.Query<Catalogue>(query).ToList();
			}
			else if (fillStyle == Utility.FillStyle.WithBasicNav)
			{
				query = "SELECT myBaseTable.*,Reff1.id, Reff1.name,Reff2.id, Reff2.name,Reff3.id, Reff3.name FROM Catalogues myBaseTable " +
						" left join Courses Reff1 on myBaseTable.courseId=Reff1.id left join Semesters Reff2 on myBaseTable.semesterId=Reff2.id " +
						" left join Books Reff3 on myBaseTable.bookId=Reff3.id where "+whereClause ;
				return db.Query<Catalogue, Course, Semester, Book, Catalogue>(query,
				(myBaseTable,ref1,ref2,ref3) =>
				{
					myBaseTable.courseNav =ref1 ;
					myBaseTable.semesterNav =ref2 ;
					myBaseTable.bookNav =ref3 ;
					return myBaseTable;
				},Utility.CreateDynamicObject(paramList)).ToList();
			}
			else if (fillStyle == Utility.FillStyle.WithFullNav)
			{
				query = "SELECT myBaseTable.*,Reff1.*,Reff2.*,Reff3.* FROM Catalogues myBaseTable left join Courses Reff1 on myBaseTable.courseId=Reff1.id " +
						" left join Semesters Reff2 on myBaseTable.semesterId=Reff2.id left join Books Reff3 on myBaseTable.bookId=Reff3.id where "+whereClause ;
				return db.Query<Catalogue, Course, Semester, Book, Catalogue>(query,
				(myBaseTable,ref1,ref2,ref3) =>
				{
					myBaseTable.courseNav =ref1 ;
					myBaseTable.semesterNav =ref2 ;
					myBaseTable.bookNav =ref3 ;
					return myBaseTable;
				},Utility.CreateDynamicObject(paramList)).ToList();
			}
			else
			{
				query = "";
			return db.Query<Catalogue>(query).ToList();
			}
		}
	}
	public static Catalogue GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
	{
		using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
		{
			db.Open();
			string query = "";
			if (fillStyle == Utility.FillStyle.Basic)
			{
				query = "SELECT id,name from Catalogues WHERE id = @id";
			return db.Query<Catalogue>(query,new {id=id}).FirstOrDefault()??new Catalogue();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from Catalogues WHERE id = @id";
			return db.Query<Catalogue>(query,new {id=id}).FirstOrDefault()??new Catalogue();
			}
			else if (fillStyle == Utility.FillStyle.WithBasicNav)
			{
				query = "SELECT myBaseTable.*,Reff1.id, Reff1.name,Reff2.id, Reff2.name,Reff3.id, Reff3.name FROM Catalogues myBaseTable " +
						" left join Courses Reff1 on myBaseTable.courseId=Reff1.id left join Semesters Reff2 on myBaseTable.semesterId=Reff2.id " +
						" left join Books Reff3 on myBaseTable.bookId=Reff3.id  WHERE myBaseTable.id = @id";
				return db.Query<Catalogue, Course, Semester, Book, Catalogue>(query,
				(myBaseTable,ref1,ref2,ref3) =>
				{
					myBaseTable.courseNav =ref1 ;
					myBaseTable.semesterNav =ref2 ;
					myBaseTable.bookNav =ref3 ;
					return myBaseTable;
				} ,new {id=id} ).FirstOrDefault()??new Catalogue();
			}
			else if (fillStyle == Utility.FillStyle.WithFullNav)
			{
				query = "SELECT myBaseTable.*,Reff1.*,Reff2.*,Reff3.* FROM Catalogues myBaseTable left join Courses Reff1 on myBaseTable.courseId=Reff1.id " +
						" left join Semesters Reff2 on myBaseTable.semesterId=Reff2.id left join Books Reff3 on myBaseTable.bookId=Reff3.id WHERE myBaseTable.id = @id";
				return db.Query<Catalogue, Course, Semester, Book, Catalogue>(query,
				(myBaseTable,ref1,ref2,ref3) =>
				{
					myBaseTable.courseNav =ref1 ;
					myBaseTable.semesterNav =ref2 ;
					myBaseTable.bookNav =ref3 ;
					return myBaseTable;
				} ,new {id=id} ).FirstOrDefault()??new Catalogue();
			}
			else
			{
				query = "";
			return db.Query<Catalogue>(query).FirstOrDefault()??new Catalogue();
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
					numId = db.ExecuteScalar<int>("select Max(numId) from Catalogues where idPrefix = 'F'") + 1;
					id = idPrefix + numId;
					string sql = "INSERT INTO Catalogues (id, numId, idPrefix, courseId, semesterId, bookId, code) VALUES (@id, @numId, @idPrefix, @courseId, @semesterId, @bookId, @code)";
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
					string sql = "UPDATE Catalogues SET numId = @numId, idPrefix = @idPrefix, courseId = @courseId, semesterId = @semesterId, bookId = @bookId, code = @code WHERE id = @id;";
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
					string sql = "DELETE FROM Catalogues WHERE id = @id;";
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
