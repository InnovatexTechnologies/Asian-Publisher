using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
	public class Language
	{
		public string id { get; set; } = "";
		public int numId { get; set; }= 0; 
		public string idPrefix { get; set; } = "";
		public string name { get; set; } = "";
	// Additional methods
	public static List<Language> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
				query = "SELECT id,name from Languages";
			return db.Query<Language>(query).OrderBy(o => o.name).ToList();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from Languages";
			return db.Query<Language>(query).OrderBy(o => o.name).ToList();
			}
			else
			{
				query = "";
			return db.Query<Language>(query).OrderBy(o => o.name).ToList();
			}
		}
	}
	public static Language GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
	{
		using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
		{
			db.Open();
			string query = "";
			if (fillStyle == Utility.FillStyle.Basic)
			{
				query = "SELECT id,name from Languages WHERE id = @id";
			return db.Query<Language>(query,new {id=id}).FirstOrDefault()??new Language();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from Languages WHERE id = @id";
			return db.Query<Language>(query,new {id=id}).FirstOrDefault()??new Language();
			}
			else
			{
				query = "";
			return db.Query<Language>(query).FirstOrDefault()??new Language();
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
					numId = db.ExecuteScalar<int>("select Max(numId) from Languages where idPrefix = 'F'") + 1;
					id = idPrefix + numId;
					 string extension = "";
					string sql = "INSERT INTO Languages (id, numId, idPrefix, name) VALUES (@id, @numId, @idPrefix, @name)";
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
					string sql = "UPDATE Languages SET numId = @numId, idPrefix = @idPrefix, name = @name WHERE id = @id;";
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
					string sql = "DELETE FROM Languages WHERE id = @id;";
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
