using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Configuration;
using System.Xml;
using CFCNET;
using CFCNET.COMPONENT;
using CFCNET.DATA;
using CFCNET.DATA.DB;
using CFCNET.HTTP;
using CFCNET.HTTP.SERVER;
using CFCNET.UI;
using CFCNET.WIN32;
using TechWorking;

namespace TechWorkingServer
{
	[CFCDatabase("TECHWORKING_DATABASE")]
	public class TechWorkingDatabase : CFCDatabaseContext
	{
		public static readonly TechWorkingDatabase Default = GetDatabase<TechWorkingDatabase>();

		public TechWorkingDatabase() {
		}
	}
	public class TechWorkingDatabaseAccesser : CFCDatabaseAccesser<TechWorkingDatabase>
	{
		private static readonly object __locker = new object();
		private static __FileInfo __databaseFile = null;

		public TechWorkingDatabaseAccesser() {
		}
		public override DatabaseFamily DatabaseFamily {
			get => DatabaseFamily.SQLite;
		}
		public override TechWorkingDatabase Database {
			get => TechWorkingDatabase.Default;
		}
		public override __FileInfo DatabaseFile {
			get {
				lock (__locker) {
					var x = __databaseFile;
					if (x == null) {
						var d = new __DirectoryInfo(string.Format(@"{0}\App_Data", HttpRuntime.AppDomainAppPath));
						if (!d.Exists) {
							d.Create();
						}
						__databaseFile = x = new __FileInfo(string.Format(@"{0}\TechWorkingDB.db", d.FullName));
					}
					return x;
				}
			}
		}

		protected override CFCDatabaseContext OnGetDatabase() {
			return TechWorkingDatabase.Default;
		}
	}
	public static class __Department
	{
		#region __Table
		[DatabaseTable(typeof(TechWorkingDatabase))]
		[Obfuscation(Feature = "renaming", Exclude = true)]
		public class __Table : DbTableDescriptor
		{
			public const string TABLE_NAME = "DEPARTMENT_TABLE";

			public __Table() : base(TABLE_NAME, TABLE_NAME) {
			}
			[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
			public DbColumnName DEPARTMENT_ID {
				get => this["DEPARTMENT_ID"];
			}
			[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0, Unique = true)]
			public DbColumnName DEPARTMENT_NAME {
				get => this["DEPARTMENT_NAME"];
			}
			[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0, Unique = true)]
			public DbColumnName DEPARTMENT_LEADER {
				get => this["DEPARTMENT_LEADER"];
			}

			protected override void OnCreated(CFCDbConnection cnn) {
				var h = new SimpleDatabaseHolder(cnn, TechWorkingDatabase.Default);
				var cmd = h.LoadCommand<__Insert>();
				foreach (var d in TechWorkingSettings.Load().Departments) {
					cmd.Insert(d);
				}
			}
		}
		#endregion
		#region __TestByID
		public class __TestByID : TDbCommand<__Table>
		{
			public __TestByID(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT COUNT(*) FROM {0} WHERE {1}",
					t.TableNameSQL,
					this.GetConditionSQL(t.DEPARTMENT_ID));
			}
			public bool Exists(Guid departmentID) {
				return this.ExecuteScalar(departmentID) > 0;
			}
		}
		#endregion
		#region __TestByName
		public class __TestByName : TDbCommand<__Table>
		{
			public __TestByName(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT COUNT(*) FROM {0} WHERE {1}",
					t.TableNameSQL,
					this.GetConditionSQL(t.DEPARTMENT_NAME));
			}
			public bool Exists(string departmentName) {
				return this.ExecuteScalar(departmentName) > 0;
			}
		}
		#endregion
		#region __LoadByID
		public class __LoadByID : TDbCommand<__Table>
		{
			public __LoadByID(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT {0} FROM {1} WHERE {2}",
					t.DEPARTMENT_NAME,
					t.TableNameSQL,
					this.GetConditionSQL(t.DEPARTMENT_ID));
			}
			public Department Load(Guid departmentID) {
				using (var rd = this.ExecuteReader(CommandBehavior.SequentialAccess, departmentID)) {
					if (rd.Read()) {
						return new Department(departmentID, rd.GetString(0),rd.GetString(1), false);
					}
					return null;
				}
			}
			public Department.Collection Load(Guid[] departmentIDs) {
				var cc = new Department.Collection();
				foreach (var id in Utility.UniqueElements(departmentIDs)) {
					var c = this.Load(id);
					if (c != null) {
						cc.Add(c);
					}
				}
				return cc;
			}
		}
		#endregion
		#region __LoadByName
		public class __LoadByName : TDbCommand<__Table>
		{
			public __LoadByName(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT {0} FROM {1} WHERE {2}",
					t.DEPARTMENT_ID,
					t.TableNameSQL,
					this.GetConditionSQL(t.DEPARTMENT_NAME));
			}
			public Department Load(string departmentName) {
				var name = Utility.NormalizeDepartmentName(departmentName, false);
				using (var rd = this.ExecuteReader(CommandBehavior.SequentialAccess, name)) {
					if (rd.Read()) {
						return new Department(rd.GetGuid(0), name,rd.GetString(0), false);
					}
					return null;
				}
			}
			public Department.Collection Load(string[] departmentNames) {
				var cc = new Department.Collection();
				foreach (var name in Utility.UniqueElements(departmentNames)) {
					var c = this.Load(name);
					if (c != null) {
						cc.Add(c);
					}
				}
				return cc;
			}
		}
		#endregion
		#region __LoadAll
		public class __LoadAll : TDbCommand<__Table>
		{
			public __LoadAll(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT {0} FROM {1}",
					this.GetSelectFieldsSQL(t.DEPARTMENT_ID, t.DEPARTMENT_NAME),
					t.TableNameSQL);
			}
			public Department.Collection Load() {
				var cc = new Department.Collection();
				using (var rd = this.ExecuteReader(CommandBehavior.SequentialAccess)) {
					while (rd.Read()) {
						cc.Add(new Department(rd.GetGuid(0), rd.GetString(1), "张三", false));
					}
				}
				return cc;
			}
		}
		#endregion
		#region __Insert
		public class __Insert : TDbCommand<__Table>
		{
			public __Insert(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.SetInsertSQL(this.GetType(),
					t.DEPARTMENT_ID,
					t.DEPARTMENT_NAME,
					t.DEPARTMENT_LEADER);
			}
			public void Insert(DepartmentDescription value) {
				if (DepartmentDescription.IsNullOrEmpty(value)) {
					throw new Exception("不能插入空的部门。");
				}
				var n = this.ExecuteNonQuery(
					Guid.NewGuid(),
					value.Name,
					value.Leader);
				if (n <= 0) {
					throw new __ExceptionExt(XSR.SplitSelect("插入记录失败。|Failed to insert the record."));
				}
			}
			public void Insert(Department value) {
				if (Department.IsNullOrEmpty(value)) {
					throw new Exception("不能插入空的部门。");
				}
				var n = this.ExecuteNonQuery(
					value.ID,
					value.Name,
					value.Leader);
				if (n <= 0) {
					throw new __ExceptionExt(XSR.SplitSelect("插入记录失败。|Failed to insert the record."));
				}
			}
		}
		#endregion
		#region __Rename
		public class __Rename : TDbCommand<__Table>
		{
			public __Rename(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "UPDATE {0} SET {1} WHERE {2}",
					t.TableNameSQL,
					this.GetUpdateFieldsSQL(t.DEPARTMENT_NAME),
					this.GetConditionSQL(t.DEPARTMENT_ID));
			}
			public void Update(Guid departmentID, string departmentName) {
				var n = this.ExecuteNonQuery(
					Utility.NormalizeDepartmentName(departmentName, true),
					departmentID);
				if (n <= 0) {
					throw new __ExceptionExt("重命名部门失败。");
				}
			}
		}
		#endregion
		#region  __Delete
		public class __Delete: TDbCommand<__Table>
		{
			public __Delete(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "DELETE FROM {0} WHERE {1}",
					t.TableNameSQL,
					this.GetConditionSQL(t.DEPARTMENT_ID)
				);
			}
			public void Delete(DepartmentDescription value) {
				if (DepartmentDescription.IsNullOrEmpty(value)) {
					throw new Exception("不能删除空的部门。");
				}
				var n = this.ExecuteNonQuery(
					value.Name);
				if (n <= 0) {
					throw new __ExceptionExt(XSR.SplitSelect("删除记录失败,目标不存在|Failed to delete record, target does not exist."));
				}
			}
			public void Delete(Department value) {
				if (Department.IsNullOrEmpty(value)) {
					throw new Exception("不能删除空的部门。");
				}
				var n = this.ExecuteNonQuery(
					value.ID);
				if (n <= 0) {
					throw new __ExceptionExt(XSR.SplitSelect("删除记录失败,目标不存在|Failed to delete record, target does not exist."));
				}
			}
		}
		#endregion
	}
	public static class __Account
	{
		#region __Table
		[DatabaseTable(typeof(TechWorkingDatabase))]
		[Obfuscation(Feature = "renaming", Exclude = true)]
		public class __Table : DbTableDescriptor
		{
			public const string TABLE_NAME = "ACCOUNT_TABLE";

			public __Table() : base(TABLE_NAME, TABLE_NAME) {
			}
			[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
			public DbColumnName ACCOUNT_ID {
				get => this["ACCOUNT_ID"];
			}
			[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0, Unique = true)]
			public DbColumnName ACCOUNT_NAME {
				get => this["ACCOUNT_NAME"];
			}
			[DbColumnDescriptor(DbFieldType.Guid, false, false, 16, 0, 0)]
			public DbColumnName DEPARTMENT_ID {
				get => this["DEPARTMENT_ID"];
			}
			[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
			public DbColumnName PASSWORD {
				get => this["PASSWORD"];
			}
			[DbColumnDescriptor(DbFieldType.Integer, false, false, 4, 0, 0)]
			public DbColumnName PERMISSION {
				get => this["PERMISSION"];
			}

			protected override void OnCreated(CFCDbConnection cnn) {
				var h = new SimpleDatabaseHolder(cnn, TechWorkingDatabase.Default);
				var cmd = h.LoadCommand<__Insert>();
				foreach (var a in TechWorkingSettings.Load().Accounts) {
					cmd.Insert(a);
				}
			}
		}
		#endregion
		#region __TestByID
		public class __TestByID : TDbCommand<__Table>
		{
			public __TestByID(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT COUNT(*) FROM {0} WHERE {1}",
					t.TableNameSQL,
					this.GetConditionSQL(t.ACCOUNT_ID));
			}
			public bool Exists(Guid accountID) {
				return this.ExecuteScalar(accountID) > 0;
			}
		}
		#endregion
		#region __TestByName
		public class __TestByName : TDbCommand<__Table>
		{
			public __TestByName(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT COUNT(*) FROM {0} WHERE {1}",
					t.TableNameSQL,
					this.GetConditionSQL(t.ACCOUNT_NAME));
			}
			public bool Exists(string accountName) {
				return this.ExecuteScalar(accountName) > 0;
			}
		}
		#endregion
		#region __LoadByID
		public class __LoadByID : TDbCommand<__Table>
		{
			public __LoadByID(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT {0} FROM {1} WHERE {2}",
					this.GetSelectFieldsSQL(
						t.DEPARTMENT_ID,
						t.ACCOUNT_NAME,
						t.PASSWORD,
						t.PERMISSION),
					t.TableNameSQL,
					this.GetConditionSQL(t.ACCOUNT_ID));
			}
			public InternalAccount Load(Guid accountID) {
				using (var rd = this.ExecuteReader(CommandBehavior.SequentialAccess, accountID)) {
					if (rd.Read()) {
						return new InternalAccount(
							rd.GetGuid(0),
							accountID,
							rd.GetString(1),
							rd.GetString(2),
							(TechWorkingPermission)rd.GetInt32(3),
							false);
					}
					return null;
				}
			}
			public InternalAccount.Collection Load(Guid[] accountIDs) {
				var cc = new InternalAccount.Collection();
				foreach (var id in Utility.UniqueElements(accountIDs)) {
					var c = this.Load(id);
					if (c != null) {
						cc.Add(c);
					}
				}
				return cc;
			}
		}
		#endregion
		#region __LoadByName
		public class __LoadByName : TDbCommand<__Table>
		{
			public __LoadByName(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT {0} FROM {1} WHERE {2}",
					this.GetSelectFieldsSQL(
						t.DEPARTMENT_ID,
						t.ACCOUNT_ID,
						t.PASSWORD,
						t.PERMISSION),
					this.GetTablesSQL(),
					this.GetConditionSQL(t.ACCOUNT_NAME));
			}
			public InternalAccount Load(string accountName) {
				var name = Utility.NormalizeAccountName(accountName, false);
				using (var rd = this.ExecuteReader(CommandBehavior.SequentialAccess, name)) {
					if (rd.Read()) {
						return new InternalAccount(
							rd.GetGuid(0),
							rd.GetGuid(1),
							name,
							rd.GetString(2),
							(TechWorkingPermission)rd.GetInt32(3),
							false);
					}
					return null;
				}
			}
			public InternalAccount.Collection Load(string[] accountNames) {
				var cc = new InternalAccount.Collection();
				foreach (var name in Utility.UniqueElements(accountNames)) {
					var c = this.Load(name);
					if (c != null) {
						cc.Add(c);
					}
				}
				return cc;
			}
		}
		#endregion
		#region __LoadAll
		public class __LoadAll : TDbCommand<__Table>
		{
			public __LoadAll(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "SELECT {0} FROM {1}",
					this.GetSelectFieldsSQL(
						t.DEPARTMENT_ID,
						t.ACCOUNT_ID,
						t.ACCOUNT_NAME,
						t.PASSWORD,
						t.PERMISSION),
					t.TableNameSQL);
			}
			public InternalAccount.Collection Load() {
				var cc = new InternalAccount.Collection();
				using (var rd = this.ExecuteReader(CommandBehavior.SequentialAccess)) {
					while (rd.Read()) {
						var a = new InternalAccount(
							rd.GetGuid(0),
							rd.GetGuid(1),
							rd.GetString(2),
							rd.GetString(3),
							(TechWorkingPermission)rd.GetInt32(4),
							false);
						cc.Add(a);
					}
				}
				return cc;
			}
		}
		#endregion
		#region __Insert
		public class __Insert : TDbCommand<__Table>
		{
			public __Insert(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.SetInsertSQL(this.GetType(),
					t.ACCOUNT_ID,
					t.ACCOUNT_NAME,
					t.DEPARTMENT_ID,
					t.PASSWORD,
					t.PERMISSION);
			}
			public void Insert(InternalAccount value) {
				if (InternalAccount.IsNullOrEmpty(value)) {
					throw new Exception("不能插入空的账户。");
				}
				var n = this.ExecuteNonQuery(
					value.AccountID,
					value.AccountName,
					value.DepartmentID,
					value.Password,
					(int)value.Permission);
				if (n <= 0) {
					throw new __ExceptionExt(XSR.SplitSelect("插入记录失败。|Failed to insert the record."));
				}
			}
		}
		#endregion
		#region __Rename
		public class __Rename : TDbCommand<__Table>
		{
			public __Rename(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "UPDATE {0} SET {1} WHERE {2}",
					t.TableNameSQL,
					this.GetUpdateFieldsSQL(t.ACCOUNT_NAME),
					this.GetConditionSQL(t.ACCOUNT_ID));
			}
			public void Update(Guid accountID, string accountName) {
				var n = this.ExecuteNonQuery(
					Utility.NormalizeAccountName(accountName, true),
					accountID);
				if (n <= 0) {
					throw new __ExceptionExt("重命名账户失败。");
				}
			}
		}
		#endregion
		#region __UpdatePassword
		public class __UpdatePassword : TDbCommand<__Table>
		{
			public __UpdatePassword(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "UPDATE {0} SET {1} WHERE {2}",
					t.TableNameSQL,
					this.GetUpdateFieldsSQL(t.PASSWORD),
					this.GetConditionSQL(t.ACCOUNT_ID));
			}
			public void Update(Guid accountID, string password) {
				var n = this.ExecuteNonQuery(
					Utility.NormalizePassword(password, true),
					accountID);
				if (n <= 0) {
					throw new __ExceptionExt("修改账户密码失败。");
				}
			}
		}
		#endregion
		#region __UpdateDepartment
		public class __UpdateDepartment : TDbCommand<__Table>
		{
			public __UpdateDepartment(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "UPDATE {0} SET {1} WHERE {2}",
					t.TableNameSQL,
					this.GetUpdateFieldsSQL(t.DEPARTMENT_ID),
					this.GetConditionSQL(t.ACCOUNT_ID));
			}
			public void Update(Guid accountID, string department) {
				var n = this.ExecuteNonQuery(
					Utility.NormalizeDepartmentName(department, true),
					accountID);
				if (n <= 0) {
					throw new __ExceptionExt("修改账户密码失败。");
				}
			}
		}
		#endregion
		#region __UpdatePermission
		public class __UpdatePermission : TDbCommand<__Table>
		{
			public __UpdatePermission(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "UPDATE {0} SET {1} WHERE {2}",
					t.TableNameSQL,
					this.GetUpdateFieldsSQL(t.PERMISSION),
					this.GetConditionSQL(t.ACCOUNT_ID));
			}
			public void Update(Guid accountID, TechWorkingPermission value) {
				var n = this.ExecuteNonQuery(
					(int)value,
					accountID);
				if (n <= 0) {
					throw new __ExceptionExt("修改账户权限失败。");
				}
			}
		}
		#endregion
		#region __Delete
		public class __Delete : TDbCommand<__Table>
		{
			public __Delete(IDatabaseHolder db) : base(db) {
				var t = this.Table;
				this.FormatSetSQL(this.GetType(), "DELETE FROM {0} WHERE {1}",
					t.TableNameSQL,
					this.GetConditionSQL(t.ACCOUNT_ID)
				);
			}
			public void Delete(Account value) {
				if (Account.IsNullOrEmpty(value)) {
					throw new Exception("不能删除空的员工。");
				}
				var n = this.ExecuteNonQuery(
					value.AccountID);
				if (n <= 0) {
					throw new __ExceptionExt(XSR.SplitSelect("删除记录失败,目标不存在|Failed to delete record, target does not exist."));
				}
			}
		}
		#endregion
	}

	//public static class __Task
	//{
	//	#region __Table
	//	[DatabaseTable(typeof(TechWorkingDatabase))]
	//	[Obfuscation(Feature = "renaming", Exclude = true)]
	//	public class __Table : DbTableDescriptor
	//	{
	//		public const string TABLE_NAME = "TASK_TABLE";
	//		public __Table() : base(TABLE_NAME, TABLE_NAME) {
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
	//		public DbColumnName TASK_ID {
	//			get => this["TASK_ID"];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0, Unique = true)]
	//		public DbColumnName TASK_NAME {
	//			get => this["TASK_NAME"];
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
	//		public DbColumnName INITIATOR_ID {
	//			get => this["INITIATOR_ID  "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
	//		public DbColumnName RESPONSIBLE_PERSON_ID {
	//			get => this["RESPONSIBLE_PERSON_ID"];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName PRIORITY {
	//			get => this["PRIORITY"];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName TYPE {
	//			get => this["TYPE "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.DBDate, false, false, 255, 0, 0)]
	//		public DbColumnName PLANNED_START_TIME {
	//			get => this["PLANNED_START_TIME "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.DBDate, false, false, 255, 0, 0)]
	//		public DbColumnName PLANNED_COMPLETION_TIME {
	//			get => this["PLANNED_COMPLETION_TIME "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName DESCRIPTION {
	//			get => this["DESCRIPTION "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName COMPLETION_PROGRESS {
	//			get => this["COMPLETION_PROGRESS "];
	//		}
	//	}
	//	#endregion
	//}

	//public static class __TaskParticipants
	//{
	//	#region __Table
	//	[DatabaseTable(typeof(TechWorkingDatabase))]
	//	[Obfuscation(Feature = "renaming", Exclude = true)]
	//	public class __Table : DbTableDescriptor
	//	{
	//		public const string TABLE_NAME = "TASK_TABLE";
	//		public __Table() : base(TABLE_NAME, TABLE_NAME) {
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
	//		public DbColumnName RECORD_ID {
	//			get => this["RECORD_ID "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, false, 16, 0, 0)]
	//		public DbColumnName TASK_ID {
	//			get => this["TASK_ID  "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, false, 16, 0, 0)]
	//		public DbColumnName PARTICIPANT_ID {
	//			get => this["PARTICIPANT_ID   "];
	//		}
	//	}
	//	#endregion
	//}

	//public static class __ATTACHMENTS
	//{
	//	#region __Table	
	//	[DatabaseTable(typeof(TechWorkingDatabase))]
	//	[Obfuscation(Feature = "renaming", Exclude = true)]
	//	public class __Table : DbTableDescriptor
	//	{
	//		public const string TABLE_NAME = "TASK_TABLE";
	//		public __Table() : base(TABLE_NAME, TABLE_NAME) {
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
	//		public DbColumnName ATTACHMENT_ID {
	//			get => this["ATTACHMENT_ID  "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, false, 16, 0, 0)]
	//		public DbColumnName TASK_ID {
	//			get => this["TASK_ID  "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName ATTACHMENT_FILE_NAME {
	//			get => this["ATTACHMENT_FILE_NAME "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName ATTACHMENT_PATH {
	//			get => this["ATTACHMENT_PATH "];
	//		}
	//	}
	//	#endregion
	//}

	//public static class __TaskLogs
	//{
	//	#region __Table
	//	[DatabaseTable(typeof(TechWorkingDatabase))]
	//	[Obfuscation(Feature = "renaming", Exclude = true)]
	//	public class __Table : DbTableDescriptor
	//	{
	//		public const string TABLE_NAME = "TASK_TABLE";
	//		public __Table() : base(TABLE_NAME, TABLE_NAME) {
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, true, 16, 0, 0)]
	//		public DbColumnName LOG_ID {
	//			get => this["LOG_ID   "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.Guid, false, false, 16, 0, 0)]
	//		public DbColumnName TASK_ID {
	//			get => this["TASK_ID  "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName LOG_CONTENT {
	//			get => this["LOG_CONTENT  "];
	//		}
	//		[DbColumnDescriptor(DbFieldType.VarChar, false, false, 255, 0, 0)]
	//		public DbColumnName LOG_TIMESTAMP {
	//			get => this["LOG_TIMESTAMP  "];
	//		}
	//	}
	//	#endregion
	//}
}