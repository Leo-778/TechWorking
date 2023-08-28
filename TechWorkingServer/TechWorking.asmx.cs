using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
	[WebService(Namespace = "http://carbonsoft.com.cn")]
	[Obfuscation(Feature = "all", Exclude = false)]
	public partial class TechWorking : WebFileTransportService, ITechWorking
	{
		public TechWorking() {
		}
		[WebMethod(true)]
		public bool AssertUserLogin() {
			return true;
		}
		[WebMethod(true)]
		public bool AssertManagerLogin() {
			return true;
		}
		[WebMethod]
		public string TestDatabase() {
#if DEBUG
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockWrite(true);
				try {
					return "OK!";
				}
				catch (Exception ec) {
					throw ec;
				}
				finally {
					asc.Unlock();
				}
			}
#else
			return "OK!";
#endif
		}

		private InternalAccount Verify(CFCDatabaseAccesser<TechWorkingDatabase> asc, LoginToken token) {
			if (asc == null) {
				throw new ArgumentNullException("asc");
			}
			var bs = token?.Token;
			if (ArraySupport.IsNullOrEmpty(bs)) {
				throw new ArgumentNullException("token");
			}
			var acc = string.Empty;
			var psw = string.Empty;
			var n = bs.Length - 1;
			for (var i = 0; i < n; i++) {
				if (bs[i] == 0 && bs[i + 1] == 0) {
					acc = Encoding.UTF8.GetString(ArraySupport.Left(bs, i));
					psw = Encoding.UTF8.GetString(ArraySupport.Right(bs, bs.Length - i - 2));
					break;
				}
			}
			if (string.IsNullOrEmpty(acc)) {
				throw new __ExceptionExt("解编账户信息失败。");
			}
			var a = asc.LoadCommand<__Account.__LoadByName>().Load(acc);
			if (a == null) {
				throw new __ExceptionExt(string.Format("账户“{0}”没有注册。", acc));
			}
			if (a.Password != psw) {
				throw new __ExceptionExt("验证账户失败。");
			}
			return a;
		}
		private Department.Collection LoadDepartments(CFCDatabaseAccesser<TechWorkingDatabase> asc) {
			return asc.LoadCommand<__Department.__LoadAll>().Load();
		}
		private TDictionary<Guid, Department> MappingDepartments_ID(CFCDatabaseAccesser<TechWorkingDatabase> asc) {
			var dc = new TDictionary<Guid, Department>();
			foreach (var d in this.LoadDepartments(asc)) {
				dc[d.ID] = d;
			}
			return dc;
		}
		private StringKeyDictionary<Department> MappingDepartments_Name(CFCDatabaseAccesser<TechWorkingDatabase> asc) {
			var dc = new StringKeyDictionary<Department>();
			foreach (var d in this.LoadDepartments(asc)) {
				dc[d.Name] = d;
			}
			return dc;
		}

		void ITechWorking.Verify(LoginToken token) {
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockRead(true);
				try {
					this.Verify(asc, token);
				}
				finally {
					asc.Unlock();
				}
			}
		}
		DateTime ITechWorking.GetUtcNow() {
			return DateTime.UtcNow;
		}
		DateTime ITechWorking.GetUtcToday() {
			return DateTime.Today.ToUniversalTime();
		}
		Department.Collection ITechWorking.LoadDepartments(LoginToken token) {
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockRead(true);
				try {
					this.Verify(asc, token).AssertPermission(TechWorkingPermission.Member);
					return this.LoadDepartments(asc);
				}
				finally {
					asc.Unlock();
				}
			}
		}
		Department ITechWorking.GetDepartment(LoginToken token, string departmentName) {
			var acc = Utility.NormalizeAccountName(departmentName, true);
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockRead(true);
				try {
					this.Verify(asc, token).AssertPermission(TechWorkingPermission.Member);
					var a = asc.LoadCommand<__Department.__LoadByName>().Load(acc) ?? throw new __ExceptionExt(string.Format("没有找到名称为“{0}”的账户。", acc));
					return new Department(a.ID,a.Name,a.Leader,false);
				}
				finally {
					asc.Unlock();
				}
			}
		}
		Department ITechWorking.GetDepartment(LoginToken token, Guid departmentID) {
			var acc = departmentID;
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockRead(true);
				try {
					this.Verify(asc, token).AssertPermission(TechWorkingPermission.Member);
					var a = asc.LoadCommand<__Department.__LoadByID>().Load(acc) ?? throw new __ExceptionExt(string.Format("没有找到ID为“{0}”的账户。", acc));
					return new Department(a.ID, a.Name, a.Leader, false);
				}
				finally {
					asc.Unlock();
				}
			}
		}
		Account.Collection ITechWorking.LoadAccounts(LoginToken token) {
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockRead(true);
				try {
					this.Verify(asc, token).AssertPermission(TechWorkingPermission.Member);
					var dc = this.MappingDepartments_ID(asc);
					var cc = new Account.Collection();
					foreach (var a in asc.LoadCommand<__Account.__LoadAll>().Load()) {
						var dp = DataUtility.Null2Empty(dc[a.DepartmentID]?.Name);
						cc.Add(new Account(a.AccountID, a.AccountName, dp, a.Permission, false));
					}
					return cc;
				}
				finally {
					asc.Unlock();
				}
			}
		}
		Account ITechWorking.GetMyAccount(LoginToken token) {
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockRead(true);
				try {
					var a = this.Verify(asc, token); a.AssertPermission(TechWorkingPermission.Member);
					var dp = DataUtility.Null2Empty(this.MappingDepartments_ID(asc)[a.DepartmentID]?.Name);
					return new Account(a.AccountID, a.AccountName, dp, a.Permission, false);
				}
				finally {
					asc.Unlock();
				}
			}
		}
		Account ITechWorking.GetAccount(LoginToken token, string accountName) {
			var acc = Utility.NormalizeAccountName(accountName, true);
			using (var asc = new TechWorkingDatabaseAccesser()) {
				asc.LockRead(true);
				try {
					this.Verify(asc, token).AssertPermission(TechWorkingPermission.Member);
					var a = asc.LoadCommand<__Account.__LoadByName>().Load(acc) ?? throw new __ExceptionExt(string.Format("没有找到名称为“{0}”的账户。", acc));
					var dp = DataUtility.Null2Empty(this.MappingDepartments_ID(asc)[a.DepartmentID]?.Name);
					return new Account(a.AccountID, a.AccountName, dp, a.Permission, false);
				}
				finally {
					asc.Unlock();
				}
			}
		}
		void ITechWorking.AppendDepartments(LoginToken token, DepartmentDescription.Collection dcc) {
			if (dcc != null && dcc.Count > 0) {
				using (var asc = new TechWorkingDatabaseAccesser()) {
					var cnn = asc.Connection;
					asc.LockWrite(true);
					try {
						try {
							this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
							var cmdTest = asc.LoadCommand<__Department.__TestByName>();
							var cmdInsert = asc.LoadCommand<__Department.__Insert>();
							foreach (var d in dcc) {
								if (!d.IsEmpty && !cmdTest.Exists(d.Name)) {
									cmdInsert.Insert(new Department(Guid.NewGuid(), d.Name, d.Leader,true));
								}
							}
							cnn.Commit();
						}
						catch (Exception ec) {
							cnn.Rollback();
							throw ec;
						}
					}
					finally {
						asc.Unlock();
					}
				}
			}
		}
		void ITechWorking.AppendAccounts(LoginToken token, AccountDescription.Collection acc) {
			if (acc != null && acc.Count > 0) {
				using (var asc = new TechWorkingDatabaseAccesser()) {
					var cnn = asc.Connection;
					asc.LockWrite(true);
					try {
						try {
							this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
							var dc = this.MappingDepartments_Name(asc);
							var cmdTest = asc.LoadCommand<__Account.__TestByName>();
							var cmdInsert = asc.LoadCommand<__Account.__Insert>();
							foreach (var a in acc) {
								if (!a.IsEmpty && !cmdTest.Exists(a.AccountName)) {
									var pd = dc[a.Department] ?? throw new __ExceptionExt(string.Format("没有找到部门“{0}”的定义", a.Department));
									cmdInsert.Insert(new InternalAccount(pd.ID, Guid.NewGuid(), a.AccountName, "123456", a.Permission, true));
								}
							}
							cnn.Commit();
						}
						catch (Exception ec) {
							cnn.Rollback();
							throw ec;
						}
					}
					finally {
						asc.Unlock();
					}
				}
			}
		}
		void ITechWorking.ChangeDepartmentName(LoginToken token, string oldDepartmentName, string newDepartmentName) {
			var nameOld = Utility.NormalizeDepartmentName(oldDepartmentName, true);
			var nameNew = Utility.NormalizeDepartmentName(newDepartmentName, true);
			if (string.Compare(nameOld, nameNew, true) != 0) {
				using (var asc = new TechWorkingDatabaseAccesser()) {
					var cnn = asc.Connection;
					asc.LockWrite(true);
					try {
						try {
							this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
							if (asc.LoadCommand<__Department.__TestByName>().Exists(nameNew)) {
								throw new __ExceptionExt(string.Format("部门名称“{0}”已经被使用。", nameNew));
							}
							var d = asc.LoadCommand<__Department.__LoadByName>().Load(nameOld) ?? throw new __ExceptionExt(string.Format("没有找到名称为“{0}”的部门。", nameOld));
							asc.LoadCommand<__Department.__Rename>().Update(d.ID, nameNew);
							cnn.Commit();
						}
						catch (Exception ec) {
							cnn.Rollback();
							throw ec;
						}
					}
					finally {
						asc.Unlock();
					}
				}
			}
		}
		void ITechWorking.ChangeMyAccountName(LoginToken token, string newAccountName) {
			var nameNew = Utility.NormalizeAccountName(newAccountName, true);
			using (var asc = new TechWorkingDatabaseAccesser()) {
				var cnn = asc.Connection;
				asc.LockWrite(true);
				try {
					try {
						var a = this.Verify(asc, token); a.AssertPermission(TechWorkingPermission.Member);
						if (string.Compare(a.AccountName, nameNew, true) != 0) {
							if (asc.LoadCommand<__Account.__TestByName>().Exists(nameNew)) {
								throw new __ExceptionExt(string.Format("账户名“{0}”已经被使用。", nameNew));
							}
							asc.LoadCommand<__Account.__Rename>().Update(a.AccountID, nameNew);
							cnn.Commit();
						}
					}
					catch (Exception ec) {
						cnn.Rollback();
						throw ec;
					}
				}
				finally {
					asc.Unlock();
				}
			}
		}
		void ITechWorking.ChangeMyPassword(LoginToken token, string newPassword) {
			var psw = Utility.NormalizePassword(newPassword, true);
			using (var asc = new TechWorkingDatabaseAccesser()) {
				var cnn = asc.Connection;
				asc.LockWrite(true);
				try {
					try {
						var a = this.Verify(asc, token);
						a.AssertPermission(TechWorkingPermission.Member);
						asc.LoadCommand<__Account.__UpdatePassword>().Update(a.AccountID, psw);
						cnn.Commit();
					}
					catch (Exception ec) {
						cnn.Rollback();
						throw ec;
					}
				}
				finally {
					asc.Unlock();
				}
			}
		}
		void ITechWorking.ChangeAccountName(LoginToken token, string oldAccountName, string newAccountName) {
			var nameOld = Utility.NormalizeAccountName(oldAccountName, true);
			var nameNew = Utility.NormalizeAccountName(newAccountName, true);
			if (string.Compare(nameOld, nameNew, true) != 0) {
				using (var asc = new TechWorkingDatabaseAccesser()) {
					var cnn = asc.Connection;
					asc.LockWrite(true);
					try {
						try {
							this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
							if (asc.LoadCommand<__Account.__TestByName>().Exists(nameNew)) {
								throw new __ExceptionExt(string.Format("账户名“{0}”已经被使用。", nameNew));
							}
							var a = asc.LoadCommand<__Account.__LoadByName>().Load(nameOld) ?? throw new __ExceptionExt(string.Format("没有找到名称为“{0}”的账户。", nameOld));
							asc.LoadCommand<__Account.__Rename>().Update(a.AccountID, nameNew);
							cnn.Commit();
						}
						catch (Exception ec) {
							cnn.Rollback();
							throw ec;
						}
					}
					finally {
						asc.Unlock();
					}
				}
			}
		}
		void ITechWorking.ChangeAccountPassword(LoginToken token, string accountName, string newPassword) {
			var acc = Utility.NormalizeAccountName(accountName, true);
			var psw = Utility.NormalizePassword(newPassword, true);
			using (var asc = new TechWorkingDatabaseAccesser()) {
				var cnn = asc.Connection;
				asc.LockWrite(true);
				try {
					try {
						this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
						var a = asc.LoadCommand<__Account.__LoadByName>().Load(acc) ?? throw new __ExceptionExt(string.Format("没有找到名称为“{0}”的账户。", acc));
						asc.LoadCommand<__Account.__UpdatePassword>().Update(a.AccountID, psw);
						cnn.Commit();
					}
					catch (Exception ec) {
						cnn.Rollback();
						throw ec;
					}
				}
				finally {
					asc.Unlock();
				}
			}
		}
		void ITechWorking.ChangeAccountPermission(LoginToken token, string accountName, TechWorkingPermission permission) {
			var acc = Utility.NormalizeAccountName(accountName, true);
			using (var asc = new TechWorkingDatabaseAccesser()) {
				var cnn = asc.Connection;
				asc.LockWrite(true);
				try {
					try {
						this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
						var a = asc.LoadCommand<__Account.__LoadByName>().Load(acc) ?? throw new __ExceptionExt(string.Format("没有找到名称为“{0}”的账户。", acc));
						asc.LoadCommand<__Account.__UpdatePermission>().Update(a.AccountID, permission);
						cnn.Commit();
					}
					catch (Exception ec) {
						cnn.Rollback();
						throw ec;
					}
				}
				finally {
					asc.Unlock();
				}
			}
		}
		void ITechWorking.ChangeAccountDepartment(LoginToken token, string accountName, string newDepartment) {
			var acc = Utility.NormalizeAccountName(accountName, true);
			var dep = Utility.NormalizePassword(newDepartment, true);
			using (var asc = new TechWorkingDatabaseAccesser()) {
				var cnn = asc.Connection;
				asc.LockWrite(true);
				try {
					try {
						this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
						var a = asc.LoadCommand<__Account.__LoadByName>().Load(acc) ?? throw new __ExceptionExt(string.Format("没有找到名称为“{0}”的账户。", acc));
						asc.LoadCommand<__Account.__UpdateDepartment>().Update(a.AccountID, newDepartment);
						cnn.Commit();
					}
					catch (Exception ec) {
						cnn.Rollback();
						throw ec;
					}
				}
				finally {
					asc.Unlock();
				}
			}

		}
		void ITechWorking.DeleteAccounts(LoginToken token, Account.Collection acc) {
			if(acc!=null && acc.Count > 0) {
				using(var asc =new TechWorkingDatabaseAccesser()) {
					var cnn = asc.Connection;
					asc.LockWrite(true);
					try {
						try {
							this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
							var cmdTest = asc.LoadCommand<__Account.__TestByName>();
							var cmdDelete = asc.LoadCommand<__Account.__Delete>();
							foreach(var a in acc) {
								if(!a.IsEmpty && cmdTest.Exists(a.AccountName)) {
									//asc.Unlock();
									//ITechWorking techWorking = this;
									//Account tmp = techWorking.GetAccount(token, a.AccountName);
									//asc.LockWrite(true);
									cmdDelete.Delete(a);
								}
							}
							cnn.Commit();
						}
						catch (Exception e){
							cnn.Rollback();
							throw e;
						}
					}
					finally {
						asc.Unlock();
					}
				}
			}
		}
		void ITechWorking.DeleteDepartments(LoginToken token, Department.Collection dcc) {
			if(dcc!=null && dcc.Count > 0) {
				using(var asc=new TechWorkingDatabaseAccesser()) {
					var cnn = asc.Connection;
					asc.LockWrite(true);
					try {
						try {
							this.Verify(asc, token).AssertPermission(TechWorkingPermission.Manager);
							var cmdTest = asc.LoadCommand<__Department.__TestByName>();
							var cmdDelete = asc.LoadCommand<__Department.__Delete>();
							foreach(var d in dcc) {
								if(!d.IsEmpty && cmdTest.Exists(d.Name)){
									cmdDelete.Delete(d);
								}
							}
							cnn.Commit();
						}
						catch(Exception e) {
							cnn.Rollback();
							throw e;
						}
					}
					finally {
						asc.Unlock();
					}
				}
			}
		}
	}
}
