using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
	public class Contact
	{
		public string id { get; set; } = "";
		public int numId { get; set; }= 0; 
		public string idPrefix { get; set; } = "";
		public string name { get; set; } = "";
		public string email { get; set; } = "";
		public string address { get; set; } = "";
		public string mobileNo { get; set; } = "";
		public string message { get; set; } = "";
	// Additional methods
	public static List<Contact> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
				query = "SELECT id,name from Contacts";
			return db.Query<Contact>(query).OrderBy(o => o.name).ToList();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from Contacts";
			return db.Query<Contact>(query).OrderBy(o => o.name).ToList();
			}
			else
			{
				query = "";
			return db.Query<Contact>(query).OrderBy(o => o.name).ToList();
			}
		}
	}
	public static Contact GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
	{
		using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
		{
			db.Open();
			string query = "";
			if (fillStyle == Utility.FillStyle.Basic)
			{
				query = "SELECT id,name from Contacts WHERE id = @id";
			return db.Query<Contact>(query,new {id=id}).FirstOrDefault()??new Contact();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from Contacts WHERE id = @id";
			return db.Query<Contact>(query,new {id=id}).FirstOrDefault()??new Contact();
			}
			else
			{
				query = "";
			return db.Query<Contact>(query).FirstOrDefault()??new Contact();
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
					numId = db.ExecuteScalar<int>("select Max(numId) from Contacts where idPrefix = 'F'") + 1;
					id = idPrefix + numId;
					 string extension = "";
					string sql = "INSERT INTO Contacts (id, numId, idPrefix, name, email, address,mobileNo, message) VALUES (@id, @numId, @idPrefix, @name, @email, @address, @mobileNo, @message)";
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
					string sql = "UPDATE Contacts SET numId = @numId, idPrefix = @idPrefix, name = @name, address=@address,mobileNo =@mobileNo, message = @message WHERE id = @id;";
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
					string sql = "DELETE FROM Contacts WHERE id = @id;";
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
