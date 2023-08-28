using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CFCNET;
using CFCNET.COMPONENT;
using CFCNET.DATA;
using CFCNET.HTTP;
using CFCNET.UI;
using CFCNET.WIN32;
using System.Xml.Schema;

namespace TechWorking
{
	public sealed class LoginToken : IArchiveSerializable, IXmlSerializable
	{
		public LoginToken() {
		}
		public LoginToken(byte[] token) {
			this.Token = ArraySupport.Null2Empty(token);
		}
		public bool IsEmpty {
			get => ArraySupport.IsNullOrEmpty(this.Token);
		}
		public byte[] Token {
			get;
			private set;
		} = ArraySupport.Create<byte>(0x50, 6);
		public void Serialize(CFCArchive ar) {
			if (ar.IsStoring) {
				var ver = 0;
				ar.Write(ver);
				ar.WriteByteArray(ArraySupport.Null2Empty(this.Token));
			} else {
				var ver = ar.ReadInt32();
				this.Token = ar.ReadByteArray();
			}
		}
		public override int GetHashCode() {
			return HashCodeGenerator.GetHashCode(this.Token);
		}
		public override bool Equals(object obj) {
			if (obj is LoginToken t) {
				return DataUtility.CompareBytes(this.Token, t.Token) == 0;
			}
			return false;
		}

		XmlSchema IXmlSerializable.GetSchema() {
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader) {
			XmlUtility.ReadXml(reader, this);
		}
		void IXmlSerializable.WriteXml(XmlWriter writer) {
			XmlUtility.WriteXml(writer, this);
		}
	}
	public sealed class AccountDescription : IComparable<AccountDescription>, IComparable, IArchiveSerializable, IXmlSerializable
	{
		#region Collection
		public sealed class Collection : TSerializableCollection<AccountDescription>
		{
			public Collection() {
			}
			protected override bool SerializeItemType {
				get => false;
			}
			protected override void OnSerializeAddItem(int index, AccountDescription c) {
			}
			protected override AccountDescription OnCreateItem(Type type) {
				return new AccountDescription();
			}
		}
		#endregion

		public AccountDescription() {
		}
		public AccountDescription(string accountName, string department, string password, TechWorkingPermission permission, bool checkError) {
			this.AccountName = Utility.NormalizeAccountName(accountName, checkError);
			this.Department = Utility.NormalizeDepartmentName(department, checkError);
			this.Password = Utility.NormalizePassword(password, checkError);
			this.Permission = permission;
		}
		public bool IsEmpty {
			get => string.IsNullOrEmpty(this.AccountName) || string.IsNullOrEmpty(this.Department);
		}
		public string AccountName {
			get;
			private set;
		} = string.Empty;
		public string Department {
			get;
			private set;
		} = string.Empty;
		public string Password {
			get;
			private set;
		} = string.Empty;
		public TechWorkingPermission Permission {
			get;
			private set;
		} = TechWorkingPermission.Visitor;
		public void Serialize(CFCArchive ar) {
			if (ar.IsStoring) {
				var ver = 0;
				ar.Write(ver);
				ar.Write(this.AccountName);
				ar.Write(this.Department);
				ar.Write((int)this.Permission);
			} else {
				var ver = ar.ReadInt32();
				this.AccountName = ar.ReadString();
				this.Department = ar.ReadString();
				this.Permission = (TechWorkingPermission)ar.ReadInt32();
			}
		}
		public override int GetHashCode() {
			return
				HashCodeGenerator.GetHashCodeIgnoreCase(this.AccountName) ^
				HashCodeGenerator.GetHashCodeIgnoreCase(this.Department) ^
				HashCodeGenerator.GetHashCodeIgnoreCase(this.Permission);
		}
		public override bool Equals(object obj) {
			if (obj is AccountDescription y) {
				return Compare(this, y) == 0 && this.Permission == y.Permission;
			}
			return false;
		}

		public static bool IsNullOrEmpty(AccountDescription value) {
			return value is null || value.IsEmpty;
		}
		public static int Compare(AccountDescription x, AccountDescription y) {
			if (x == y) {
				return 0;
			} else if (x is null) {
				return -1;
			} else if (y is null) {
				return 1;
			} else {
				var cmp = string.Compare(x.AccountName, y.AccountName, true);
				if (cmp == 0) {
					cmp = string.Compare(x.Department, y.Department, true);
				}
				return cmp < 0 ? -1 : (cmp > 0 ? 1 : 0);
			}
		}

		int IComparable<AccountDescription>.CompareTo(AccountDescription other) {
			return Compare(this, other);
		}
		int IComparable.CompareTo(object obj) {
			return Compare(this, obj as AccountDescription);
		}
		XmlSchema IXmlSerializable.GetSchema() {
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader) {
			XmlUtility.ReadXml(reader, this);
		}
		void IXmlSerializable.WriteXml(XmlWriter writer) {
			XmlUtility.WriteXml(writer, this);
		}
	}
	public sealed class Account : IComparable<Account>, IComparable, IArchiveSerializable, IXmlSerializable
	{
		#region Collection
		public sealed class Collection : TSerializableCollection<Account>
		{
			public Collection() {
			}
			protected override bool SerializeItemType {
				get => false;
			}
			protected override void OnSerializeAddItem(int index, Account c) {
			}
			protected override Account OnCreateItem(Type type) {
				return new Account();
			}
		}
		#endregion

		public Account() {
		}
		public Account(Guid accountID, string accountName, string department, TechWorkingPermission permission, bool checkError) {
			this.AccountID = accountID;
			this.AccountName = Utility.NormalizeAccountName(accountName, checkError);
			this.Department = Utility.NormalizeDepartmentName(department, checkError);
			this.Permission = permission;
		}
		public bool IsEmpty {
			get => string.IsNullOrEmpty(this.AccountName) || string.IsNullOrEmpty(this.Department) || this.AccountID == Guid.Empty;
		}
		public Guid AccountID {
			get;
			private set;
		} = Guid.Empty;
		public string AccountName {
			get;
			private set;
		} = string.Empty;
		public string Department {
			get;
			private set;
		} = string.Empty;
		public TechWorkingPermission Permission {
			get;
			private set;
		} = TechWorkingPermission.Visitor;
		public void Serialize(CFCArchive ar) {
			if (ar.IsStoring) {
				var ver = 0;
				ar.Write(ver);
				ar.Write(this.AccountID);
				ar.Write(this.AccountName);
				ar.Write(this.Department);
				ar.Write((int)this.Permission);
			} else {
				var ver = ar.ReadInt32();
				this.AccountID = ar.ReadGuid();
				this.AccountName = ar.ReadString();
				this.Department = ar.ReadString();
				this.Permission = (TechWorkingPermission)ar.ReadInt32();
			}
		}
		public override int GetHashCode() {
			return 
				this.AccountID.GetHashCode() ^
				HashCodeGenerator.GetHashCodeIgnoreCase(this.AccountName) ^
				HashCodeGenerator.GetHashCodeIgnoreCase(this.Department) ^
				HashCodeGenerator.GetHashCodeIgnoreCase(this.Permission);
		}
		public override bool Equals(object obj) {
			if (obj is Account y) {
				return Compare(this, y) == 0 && this.Permission == y.Permission;
			}
			return false;
		}

		public static bool IsNullOrEmpty(Account value) {
			return value is null || value.IsEmpty;
		}
		public static int Compare(Account x, Account y) {
			if (x == y) {
				return 0;
			} else if (x is null) {
				return -1;
			} else if (y is null) {
				return 1;
			} else {
				var cmp = string.Compare(x.AccountName, y.AccountName, true);
				if (cmp == 0) {
					cmp = x.AccountID.CompareTo(y.AccountID);
					if (cmp == 0) {
						cmp = string.Compare(x.Department, y.Department, true);
					}
				}
				return cmp < 0 ? -1 : (cmp > 0 ? 1 : 0);
			}
		}

		int IComparable<Account>.CompareTo(Account other) {
			return Compare(this, other);
		}
		int IComparable.CompareTo(object obj) {
			return Compare(this, obj as Account);
		}
		XmlSchema IXmlSerializable.GetSchema() {
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader) {
			XmlUtility.ReadXml(reader, this);
		}
		void IXmlSerializable.WriteXml(XmlWriter writer) {
			XmlUtility.WriteXml(writer, this);
		}
	}
	public sealed class InternalAccount : IComparable<InternalAccount>, IComparable, IArchiveSerializable, IXmlSerializable
	{
		#region Collection
		public sealed class Collection : TSerializableCollection<InternalAccount>
		{
			public Collection() {
			}
			protected override bool SerializeItemType {
				get => false;
			}
			protected override void OnSerializeAddItem(int index, InternalAccount c) {
			}
			protected override InternalAccount OnCreateItem(Type type) {
				return new InternalAccount();
			}
		}
		#endregion

		public InternalAccount() {
		}
		public InternalAccount(Guid departmentID, Guid accountID, string accountName, string password, TechWorkingPermission permission, bool checkError) {
			this.DepartmentID = departmentID;
			this.AccountID = accountID;
			this.AccountName = Utility.NormalizeAccountName(accountName, checkError);
			this.Password = Utility.NormalizePassword(password, checkError);
			this.Permission = permission;
		}
		public bool IsEmpty {
			get => 
				string.IsNullOrEmpty(this.AccountName) ||
				this.AccountID == Guid.Empty ||
				this.DepartmentID == Guid.Empty ||
				string.IsNullOrEmpty(this.Password);
		}
		public Guid DepartmentID {
			get;
			private set;
		} = Guid.Empty;
		public Guid AccountID {
			get;
			private set;
		} = Guid.Empty;
		public string AccountName {
			get;
			private set;
		} = string.Empty;
		public string Password {
			get;
			private set;
		} = string.Empty;
		public TechWorkingPermission Permission {
			get;
			private set;
		} = TechWorkingPermission.Visitor;
		public void AssertPermission(TechWorkingPermission value) {
			if (value != TechWorkingPermission.Visitor && (this.Permission & value) == 0) {
				throw new __ExceptionExt(string.Format("要求“{0}”身份才能完成指定的操作。", EnumNotationAttribute.GetEnumNotation(value)));
			}
		}
		public void Serialize(CFCArchive ar) {
			if (ar.IsStoring) {
				var ver = 0;
				ar.Write(ver);
				ar.Write(this.DepartmentID);
				ar.Write(this.AccountID);
				ar.Write(this.AccountName);
				ar.Write(this.Password);
				ar.Write((int)this.Permission);
			} else {
				var ver = ar.ReadInt32();
				this.DepartmentID = ar.ReadGuid();
				this.AccountID = ar.ReadGuid();
				this.AccountName = ar.ReadString();
				this.Password = ar.ReadString();
				this.Permission = (TechWorkingPermission)ar.ReadInt32();
			}
		}
		public override int GetHashCode() {
			return
				this.DepartmentID.GetHashCode() ^
				this.AccountID.GetHashCode() ^
				HashCodeGenerator.GetHashCodeIgnoreCase(this.AccountName) ^
				HashCodeGenerator.GetHashCode(this.Password) ^
				HashCodeGenerator.GetHashCodeIgnoreCase(this.Permission);
		}
		public override bool Equals(object obj) {
			if (obj is InternalAccount y) {
				return 
					Compare(this, y) == 0 &&
					this.Permission == y.Permission &&
					this.Password == y.Password;
			}
			return false;
		}

		public static bool IsNullOrEmpty(InternalAccount value) {
			return value is null || value.IsEmpty;
		}
		public static int Compare(InternalAccount x, InternalAccount y) {
			if (x == y) {
				return 0;
			} else if (x is null) {
				return -1;
			} else if (y is null) {
				return 1;
			} else {
				var cmp = string.Compare(x.AccountName, y.AccountName, true);
				if (cmp == 0) {
					cmp = x.AccountID.CompareTo(y.AccountID);
					if (cmp == 0) {
						cmp = x.DepartmentID.CompareTo(y.DepartmentID);
					}
				}
				return cmp < 0 ? -1 : (cmp > 0 ? 1 : 0);
			}
		}

		int IComparable<InternalAccount>.CompareTo(InternalAccount other) {
			return Compare(this, other);
		}
		int IComparable.CompareTo(object obj) {
			return Compare(this, obj as InternalAccount);
		}
		XmlSchema IXmlSerializable.GetSchema() {
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader) {
			XmlUtility.ReadXml(reader, this);
		}
		void IXmlSerializable.WriteXml(XmlWriter writer) {
			XmlUtility.WriteXml(writer, this);
		}
	}
	public sealed class DepartmentDescription : IComparable<DepartmentDescription>, IComparable, IArchiveSerializable, IXmlSerializable
	{
		#region Collection
		public sealed class Collection : TSerializableCollection<DepartmentDescription>
		{
			public Collection() {
			}
			protected override bool SerializeItemType {
				get => false;
			}
			protected override void OnSerializeAddItem(int index, DepartmentDescription c) {
			}
			protected override DepartmentDescription OnCreateItem(Type type) {
				return new DepartmentDescription();
			}
		}
		#endregion

		public DepartmentDescription() {
		}
		public DepartmentDescription(string name,string leader, bool checkError) {
			this.Name = Utility.NormalizeDepartmentName(name, checkError);
			this.Leader= Utility.NormalizeDepartmentName(leader, checkError);
		}
		public bool IsEmpty {
			get => string.IsNullOrEmpty(this.Name);
		}
		public string Name {
			get;
			private set;
		} = string.Empty;
		public string Leader {
			get;
			private set;
		} = string.Empty;
		public void Serialize(CFCArchive ar) {
			if (ar.IsStoring) {
				var ver = 0;
				ar.Write(ver);
				ar.Write(this.Name);
				ar.Write(this.Leader);
			} else {
				var ver = ar.ReadInt32();
				this.Name = ar.ReadString();
				this.Leader = ar.ReadString();
			}
		}
		public override int GetHashCode() {
			return HashCodeGenerator.GetHashCodeIgnoreCase(this.Name);
		}
		public override bool Equals(object obj) {
			if (obj is DepartmentDescription y) {
				return Compare(this, y) == 0;
			}
			return false;
		}

		public static bool IsNullOrEmpty(DepartmentDescription value) {
			return value is null || value.IsEmpty;
		}
		public static int Compare(DepartmentDescription x, DepartmentDescription y) {
			if (x == y) {
				return 0;
			} else if (x is null) {
				return -1;
			} else if (y is null) {
				return 1;
			} else {
				var cmp = string.Compare(x.Name, y.Name, true);
				return cmp < 0 ? -1 : (cmp > 0 ? 1 : 0);
			}
		}

		int IComparable<DepartmentDescription>.CompareTo(DepartmentDescription other) {
			return Compare(this, other);
		}
		int IComparable.CompareTo(object obj) {
			return Compare(this, obj as DepartmentDescription);
		}
		XmlSchema IXmlSerializable.GetSchema() {
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader) {
			XmlUtility.ReadXml(reader, this);
		}
		void IXmlSerializable.WriteXml(XmlWriter writer) {
			XmlUtility.WriteXml(writer, this);
		}
	}
	public sealed class Department : IComparable<Department>, IComparable, IArchiveSerializable, IXmlSerializable
	{
		#region Collection
		public sealed class Collection : TSerializableCollection<Department>
		{
			public Collection() {
			}
			protected override bool SerializeItemType {
				get => false;
			}
			protected override void OnSerializeAddItem(int index, Department c) {
			}
			protected override Department OnCreateItem(Type type) {
				return new Department();
			}
		}
		#endregion

		public Department() {
		}
		public Department(Guid id, string name, string leader,bool checkError) {
			this.ID = id;
			this.Name = Utility.NormalizeDepartmentName(name, checkError);
			this.Leader = leader;
		}
		public bool IsEmpty {
			get => string.IsNullOrEmpty(this.Name) || this.ID == Guid.Empty;
		}
		public Guid ID {
			get;
			private set;
		} = Guid.Empty;
		public string Name {
			get;
			private set;
		} = string.Empty;
		public string Leader {
			get;
			private set;
		} = string.Empty;
		public void Serialize(CFCArchive ar) {
			if (ar.IsStoring) {
				var ver = 0;
				ar.Write(ver);
				ar.Write(this.ID);
				ar.Write(this.Name);
				ar.Write(this.Leader);
			} else {
				var ver = ar.ReadInt32();
				this.ID = ar.ReadGuid();
				this.Name = ar.ReadString();
				this.Leader = ar.ReadString();
			}
		}
		public override int GetHashCode() {
			return this.ID.GetHashCode() ^ HashCodeGenerator.GetHashCodeIgnoreCase(this.Name);
		}
		public override bool Equals(object obj) {
			if (obj is Department y) {
				return Compare(this, y) == 0;
			}
			return false;
		}

		public static bool IsNullOrEmpty(Department value) {
			return value is null || value.IsEmpty;
		}
		public static int Compare(Department x, Department y) {
			if (x == y) {
				return 0;
			} else if (x is null) {
				return -1;
			} else if (y is null) {
				return 1;
			} else {
				var cmp = string.Compare(x.Name, y.Name, true);
				if (cmp == 0) {
					cmp = x.ID.CompareTo(y.ID);
				}
				return cmp < 0 ? -1 : (cmp > 0 ? 1 : 0);
			}
		}

		int IComparable<Department>.CompareTo(Department other) {
			return Compare(this, other);
		}
		int IComparable.CompareTo(object obj) {
			return Compare(this, obj as Department);
		}
		XmlSchema IXmlSerializable.GetSchema() {
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader) {
			XmlUtility.ReadXml(reader, this);
		}
		void IXmlSerializable.WriteXml(XmlWriter writer) {
			XmlUtility.WriteXml(writer, this);
		}
	}
	public static class Utility
	{
		#region PermissionParserSupport
		private class PermissionParserSupport
		{
			private static readonly object __locker = new object();
			private static PermissionParserSupport __default = null;
			private readonly StringKeyDictionary<TechWorkingPermission?> Items = new StringKeyDictionary<TechWorkingPermission?>();

			public PermissionParserSupport() {
				var cc = this.Items;
				var t = typeof(TechWorkingPermission);
				foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.Static)) {
					if (f.FieldType == t) {
						if (f.GetValue(true) is TechWorkingPermission p) {
							cc[f.Name] = p;
							var a = DataUtility.Null2Empty(f.GetCustomAttribute<EnumNotationAttribute>(false)?.Text);
							if (!string.IsNullOrEmpty(a)) {
								cc[a] = p;
							}
						}
					}
				}
			}
			public bool GetPermission(string name, out TechWorkingPermission value) {
				var x = this.Items[DataUtility.Null2Empty(name)];
				if (x != null) {
					value = x.Value;
					return true;
				} else {
					value = TechWorkingPermission.Visitor;
					return false;
				}
			}
			public static PermissionParserSupport GetDefault() {
				lock (__locker) {
					var x = __default;
					if (x == null) {
						__default = x = new PermissionParserSupport();
					}
					return x;
				}
			}
		}
		#endregion

		public static string NormalizeAccountName(string value, bool throwError) {
			var x = (string)NormalizedName.Normalize(value, throwError);
			if (throwError) {
				if (x.Length == 0) {
					throw new __ExceptionExt("账户名不能为空。");
				}
				if (x.Length > 64) {
					throw new __ExceptionExt("账户名长度不能超过 64。");
				}
			}
			return x;
		}
		public static string NormalizeDepartmentName(string value, bool throwError) {
			var x = (string)NormalizedPath.Normalize(value, throwError);
			if (throwError) {
				if (x.Length == 0) {
					throw new __ExceptionExt("部门名不能为空。");
				}
				if (x.Length > 220) {
					throw new __ExceptionExt("部门名长度不能超过 220。");
				}
			}
			return x;
		}
		public static string NormalizePassword(string value, bool throwError) {
			var x = DataUtility.PackString(value);
			if (throwError) {
				if (x.Length == 0) {
					throw new __ExceptionExt("密码不能为空。");
				}
				if (x.Length > 16) {
					throw new __ExceptionExt("密码长度不能超过 16。");
				}
			}
			return x;
		}
		public static string NormalizePermission(string value, bool throwError) {
			if (!string.IsNullOrEmpty(value)) {
				var dc = new StringKeyDictionary();
				foreach (var s in value.Split(",;|，；".ToCharArray())) {
					var x = DataUtility.PackString(s);
					if (x == "*") {
						return x;
					} else if (!string.IsNullOrEmpty(x)) {
						dc.Add(x);
					}
				}
				var m = string.Join("|", dc.ToArray());
				if (throwError && m.Length > 220) {
					throw new __ExceptionExt("权限长度不能超过 220。");
				}
				return m;
			}
			return string.Empty;
		}
		public static TechWorkingPermission ParsePermisson(string value) {
			var t = typeof(TechWorkingPermission);
			var ev = TechWorkingPermission.Visitor;
			var psr = PermissionParserSupport.GetDefault();
			foreach (var s in value.Split(",;|，；".ToCharArray())) {
				var x = DataUtility.PackString(s);
				if (x == "*") {
					return TechWorkingPermission.All;
				} else if (!string.IsNullOrEmpty(x)) {
					if (psr.GetPermission(x, out TechWorkingPermission y)) {
						ev |= y;
					}
				}
			}
			return ev;
		}
		public static string[] UniqueElements(string[] array) {
			var dc = new StringKeyDictionary();
			if (array != null) {
				foreach (var x in array) {
					if (!string.IsNullOrEmpty(x)) {
						dc.Add(x);
					}
				}
			}
			return ArraySupport.Sort(dc.ToArray(), false);
		}
		public static Guid[] UniqueElements(Guid[] array) {
			var dc = new TKeyDictionary<Guid>();
			if (array != null) {
				foreach (var x in array) {
					dc.Add(x);
				}
			}
			return ArraySupport.Sort(dc.ToArray(), false);
		}
	}
	[Flags]
	public enum TechWorkingPermission
	{
		[EnumNotation("访客")]
		Visitor = 0,
		[EnumNotation("部门员工")]
		Member = 0x0001,
		[EnumNotation("部门领导")]
		DepartmentLeader = 0x0002,
		[EnumNotation("公司领导")]
		CompanyLeader = 0x004,
		[EnumNotation("管理员")]
		Manager = 0x0008,
		All = Member | DepartmentLeader | CompanyLeader | Manager,
	}
	[WebSessionInvokerBind]
	public interface ITechWorking
	{
		void Verify(LoginToken token);
		DateTime GetUtcNow();
		DateTime GetUtcToday();
		Department.Collection LoadDepartments(LoginToken token);
		Department GetDepartment(LoginToken token, string departmentName);
		Department GetDepartment(LoginToken token, Guid departmentID);
		Account.Collection LoadAccounts(LoginToken token);
		Account GetMyAccount(LoginToken token);
		Account GetAccount(LoginToken token, string accountName);
		void AppendDepartments(LoginToken token, DepartmentDescription.Collection dcc);
		void AppendAccounts(LoginToken token, AccountDescription.Collection acc);
		void ChangeDepartmentName(LoginToken token, string oldDepartmentName, string newDepartmentName);
		void ChangeMyAccountName(LoginToken token, string newAccountName);
		void ChangeMyPassword(LoginToken token, string newPassword);
		void ChangeAccountName(LoginToken token, string oldAccountName, string newAccountName);
		void ChangeAccountPassword(LoginToken token, string accountName, string newPassword);
		void ChangeAccountPermission(LoginToken token, string accountName, TechWorkingPermission permission);
		void ChangeAccountDepartment(LoginToken token, string accountName, string newDepartment);
		void DeleteAccounts(LoginToken token, Account.Collection acc);
		void DeleteDepartments(LoginToken token, Department.Collection dcc);

		//

	}
}
