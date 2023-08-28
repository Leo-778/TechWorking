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
	public class TechWorkingSettings : ConfigurationSection
	{
		private static readonly char[] __trimChars = " \t\r\n\b".ToCharArray();
		private static readonly object __locker = new object();
		private static TechWorkingSettings __loaded = null;

		public TechWorkingSettings() {
		}
		public Department.Collection Departments {
			get;
		} = new Department.Collection();
		public InternalAccount.Collection Accounts {
			get;
		} = new InternalAccount.Collection();

		public static TechWorkingSettings Load() {
			lock (__locker) {
				var x = __loaded;
				if (x == null) {
					__loaded = x = __Load();
				}
				return x;
			}
		}

		protected override void DeserializeSection(XmlReader reader) {
			var xd = new XmlDocument();
			var xn = xd.ReadNode(reader);
			var dcc = this.Departments; dcc.Clear();
			var acc = this.Accounts; acc.Clear();
			var dc_id = new TKeyDictionary<Guid>();
			var dc_name = new StringKeyDictionary<Department>();
			foreach (var xe in __GetElements(xn, "departments")) {
				foreach (var c in __GetElements(xe, "department")) {
					var name = string.Empty;
					var id = Guid.Empty;
					foreach (var x in c.Attributes) {
						if (x is XmlAttribute a) {
							switch (__Trim(a.Name).ToLower()) {
							case "name":
								name = Utility.NormalizeDepartmentName(a.Value, true);
								break;
							case "id":
								id = XmlConvert.ToGuid(a.Value);
								break;
							}
						}
					}
					if (id == Guid.Empty) {
						throw new __ExceptionExt("初始化设置项错误：没有设置部门 ID 或部门 ID 为空。");
					}
					if (string.IsNullOrEmpty(name)) {
						throw new __ExceptionExt("初始化设置项错误：没有设置部门名称或部门名称为空。");
					}
					if (!dc_id.NotContainsAdd(id)) {
						throw new __ExceptionExt("初始化设置项错误：部门 ID 重定义。");
					}
					if (dc_name.Contains(name)) {
						throw new __ExceptionExt("初始化设置项错误：部门名称重定义。");
					}
					var d = new Department(id, name, "123",false);
					dcc.Add(d);
					dc_name[d.Name] = d;
				}
			}
			var ac_id = new TKeyDictionary<Guid>();
			var ac_name = new StringKeyDictionary();
			foreach (var xe in __GetElements(xn, "accounts")) {
				foreach (var c in __GetElements(xe, "account")) {
					var depart = string.Empty;
					var name = string.Empty;
					var psw = string.Empty;
					var pms = TechWorkingPermission.Visitor;
					var id = Guid.Empty;
					foreach (var x in c.Attributes) {
						if (x is XmlAttribute a) {
							switch (__Trim(a.Name).ToLower()) {
							case "department":
								depart = Utility.NormalizeDepartmentName(a.Value, true);
								break;
							case "name":
								name = Utility.NormalizeAccountName(a.Value, true);
								break;
							case "password":
								psw = Utility.NormalizePassword(a.Value, true);
								break;
							case "permission":
								pms = Utility.ParsePermisson(a.Value);
								break;
							case "id":
								id = XmlConvert.ToGuid(a.Value);
								break;
							}
						}
					}
					if (id == Guid.Empty) {
						throw new __ExceptionExt("初始化设置项错误：没有设置账户 ID 或账户 ID 为空。");
					}
					if (string.IsNullOrEmpty(depart)) {
						throw new __ExceptionExt("初始化设置项错误：没有设置账户部门或账户部门为空。");
					}
					if (string.IsNullOrEmpty(name)) {
						throw new __ExceptionExt("初始化设置项错误：没有设置账户名或账户名为空。");
					}
					if (string.IsNullOrEmpty(psw)) {
						throw new __ExceptionExt("初始化设置项错误：没有设置账户密码或账户密码为空。");
					}
					if (!ac_name.NotContainsAdd(name)) {
						throw new __ExceptionExt("初始化设置项错误：账户名重定义。");
					}
					if (!ac_id.NotContainsAdd(id)) {
						throw new __ExceptionExt("初始化设置项错误：账户 ID 重定义。");
					}
					var d = dc_name[depart] ?? throw new __ExceptionExt(string.Format("初始化设置项错误：在已经初始化的部门集合中没有找到名称为“{0}”的部门。", depart));
					acc.Add(new InternalAccount(d.ID, id, name, psw, pms, true));
				}
			}
		}

		private static XmlElement[] __GetElements(XmlNode xn, string name) {
			var xes = new List<XmlElement>();
			if (xn != null) {
				var cc = xn.ChildNodes;
				var n = cc.Count;
				for (var i = 0; i < n; i++) {
					if (cc[i] is XmlElement xe && string.Compare(xe.Name, name, true) == 0) {
						xes.Add(xe);
					}
				}
			}
			return xes.ToArray();
		}
		private static string __Trim(string value) {
			return (value ?? string.Empty).Trim(__trimChars);
		}
		private static TechWorkingSettings __Load() {
			var sectionName = "techWorkingSettings";
			try {
				var sec = ConfigurationManager.GetSection(sectionName) ?? throw new Exception("加载配置返回为空。");
				var s = sec is TechWorkingSettings __s ? __s : throw new InvalidCastException(string.Format("“<{0}>”返回的配置必须是“{1}”。", sectionName, typeof(TechWorkingSettings).FullName));
				return s;
			}
			catch (Exception ec) {
				throw new InvalidOperationException(string.Format("加载配置节“{0}”失败。\r\n详细信息:\r\n{1}", sectionName, ec.Message));
			}
		}
	}
}