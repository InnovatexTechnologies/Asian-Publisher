using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
	public class LoginUser
	{
		public string id { get; set; } = "";
		public int numId { get; set; }= 0; 
		public string idPrefix { get; set; } = "";
		public string name { get; set; } = "";
		public string password { get; set; } = "";
		public string email { get; set; } = "";
		public string mobileNo { get; set; } = "";
	// Additional methods
	public static List<LoginUser> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
				query = "SELECT id,name from LoginUsers";
			return db.Query<LoginUser>(query).OrderBy(o => o.name).ToList();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from LoginUsers";
			return db.Query<LoginUser>(query).OrderBy(o => o.name).ToList();
			}
			else
			{
				query = "";
			return db.Query<LoginUser>(query).OrderBy(o => o.name).ToList();
			}
		}
	}
	public static LoginUser GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
	{
		using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
		{
			db.Open();
			string query = "";
			if (fillStyle == Utility.FillStyle.Basic)
			{
				query = "SELECT id,name from LoginUsers WHERE id = @id";
			return db.Query<LoginUser>(query,new {id=id}).FirstOrDefault()??new LoginUser();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from LoginUsers WHERE id = @id";
			return db.Query<LoginUser>(query,new {id=id}).FirstOrDefault()??new LoginUser();
			}
			else
			{
				query = "";
			return db.Query<LoginUser>(query).FirstOrDefault()??new LoginUser();
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
					numId = db.ExecuteScalar<int>("select Max(numId) from LoginUsers where idPrefix = 'F'") + 1;
					id = idPrefix + numId;
					 string extension = "";
					string sql = "INSERT INTO LoginUsers (id, numId, idPrefix, name, password,email,mobileNo) VALUES (@id, @numId, @idPrefix, @name, @password,@email,@mobileNo)";
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
					string sql = "UPDATE LoginUsers SET numId = @numId, idPrefix = @idPrefix, name = @name, password = @password,,email=@email,mobileNo=@mobileNo WHERE id = @id;";
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
					string sql = "DELETE FROM LoginUsers WHERE id = @id;";
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
