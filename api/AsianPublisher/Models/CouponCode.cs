using System.Data.SQLite;
using System.Data;
using Dapper;


namespace AsianPublisher.Models
{
	public class CouponCode
	{
		public string id { get; set; } = "";
		public int numId { get; set; }= 0; 
		public string idPrefix { get; set; } = "";
		public string name { get; set; } = "";
		public int validUpTo { get; set; }= 0; 
		public string couponType { get; set; } = "";
		public int status { get; set; }= 0; 
		public string couponValue { get; set; } = "";
		public string minimumOrderValue { get; set; } = "";
		public string maximumDiscountValue { get; set; } = "";
	// Additional methods
	public static List<CouponCode> Get(Utility.FillStyle fillStyle = Utility.FillStyle.AllProperties, Dictionary<string, string>? paramList = null)
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
				query = "SELECT id,name from CouponCodes";
			return db.Query<CouponCode>(query).OrderBy(o => o.name).ToList();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from CouponCodes";
			return db.Query<CouponCode>(query).OrderBy(o => o.name).ToList();
			}
			else
			{
				query = "";
			return db.Query<CouponCode>(query).OrderBy(o => o.name).ToList();
			}
		}
	}
	public static CouponCode GetById(string id, Utility.FillStyle fillStyle = Utility.FillStyle.Basic)
	{
		using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
		{
			db.Open();
			string query = "";
			if (fillStyle == Utility.FillStyle.Basic)
			{
				query = "SELECT id,name from CouponCodes WHERE id = @id";
			return db.Query<CouponCode>(query,new {id=id}).FirstOrDefault()??new CouponCode();
			}
			else if (fillStyle == Utility.FillStyle.AllProperties)
			{
				query = "SELECT * from CouponCodes WHERE id = @id";
			return db.Query<CouponCode>(query,new {id=id}).FirstOrDefault()??new CouponCode();
			}
			else
			{
				query = "";
			return db.Query<CouponCode>(query).FirstOrDefault()??new CouponCode();
			}
		}
	}
	public bool Save(string  validUpTo)
	{
		using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
		{
			db.Open();
			using (var transaction = db.BeginTransaction())
			{
				try
				{
					idPrefix = "F";
					numId = db.ExecuteScalar<int>("select Max(numId) from CouponCodes where idPrefix = 'F'") + 1;
					id = idPrefix + numId;
					 string extension = "";
					this.validUpTo = int.Parse(DateTime.Parse(validUpTo).ToString("yyyyMMdd"));
					string sql = "INSERT INTO CouponCodes (id, numId, idPrefix, name, validUpTo, couponType, status, couponValue, minimumOrderValue, maximumDiscountValue) VALUES (@id, @numId, @idPrefix, @name, @validUpTo, @couponType, @status, @couponValue, @minimumOrderValue, @maximumDiscountValue)";
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
	public bool Update(string  validUpTo)
	{
		using (IDbConnection db = new SQLiteConnection(Utility.ConnString))
		{
			db.Open();
			using (var transaction = db.BeginTransaction())
			{
				try
				{
					 string extension = "";
					this.validUpTo = int.Parse(DateTime.Parse(validUpTo).ToString("yyyyMMdd"));
					string sql = "UPDATE CouponCodes SET numId = @numId, idPrefix = @idPrefix, name = @name, validUpTo = @validUpTo, couponType = @couponType, status = @status, couponValue = @couponValue, minimumOrderValue = @minimumOrderValue, maximumDiscountValue = @maximumDiscountValue WHERE id = @id;";
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
					string sql = "DELETE FROM CouponCodes WHERE id = @id;";
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
