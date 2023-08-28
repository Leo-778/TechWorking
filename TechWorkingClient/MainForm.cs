using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CFCNET;
using CFCNET.APPFRAME;
using CFCNET.APPFRAME.CONTROL;
using CFCNET.COMMAND;
using CFCNET.COMPONENT;
using CFCNET.DATA;
using CFCNET.DATA.DB;
using CFCNET.HTTP;
using CFCNET.UI;
using CFCNET.WIN32;
using TechWorking;
using TechWorkingServer;

namespace TechWorkingClient
{
	using __Settings = Properties.Settings;

	//public partial class MainForm : XSRForm, IStatusNotifyHandler
	//{
	//	#region LoginInformation
	//	[Obfuscation(Feature = "all", Exclude = false)]
	//	private class LoginInformation
	//	{
	//		#region LoginDialog
	//		[UIStatePropertyPersist("1386998E-D1A6-4EFB-8449-17E8C9DB23E0")]
	//		private class LoginDialog : XSRForm
	//		{
	//			#region PasswordTextBox
	//			private class PasswordTextBox : LabelTextBox
	//			{
	//				public PasswordTextBox() {
	//					base.AllowCustomFilterChar = true;
	//				}
	//				public override bool AllowCustomFilterChar {
	//					get => base.AllowCustomFilterChar;
	//					set { }
	//				}
	//				protected override void OnCustomFilterChar(TextBoxFiterCharEventArgs e) {
	//					var c = e.ConvertedChar;
	//					e.Cancel = !((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'Z'));
	//				}
	//			}
	//			#endregion
	//			#region UIStatePersistStore
	//			private class UIStatePersistStore : IArchiveSerializable
	//			{
	//				public UIStatePersistStore() {
	//				}
	//				public UIStatePersistStore(LoginDialog dlg) {
	//					this.AccountDescription = dlg.AccountDescription.Text;
	//					this.Password = dlg.Password.Text;
	//					this.Remember = dlg.Remember.Checked;
	//					this.HideChars = dlg.HideChars.Checked;
	//				}
	//				public string AccountDescription {
	//					get;
	//					set;
	//				} = string.Empty;
	//				public string Password {
	//					get;
	//					set;
	//				} = string.Empty;
	//				public bool Remember {
	//					get;
	//					set;
	//				} = true;
	//				public bool HideChars {
	//					get;
	//					set;
	//				} = true;
	//				public void Restore(LoginDialog dlg) {
	//					if (!string.IsNullOrWhiteSpace(this.AccountDescription)) {
	//						dlg.AccountDescription.Text = this.AccountDescription;
	//					}
	//					if (!string.IsNullOrWhiteSpace(this.Password)) {
	//						dlg.Password.Text = this.Password;
	//					}
	//					dlg.Remember.Checked = this.Remember;
	//					dlg.HideChars.Checked = this.HideChars;
	//				}
	//				public void Serialize(CFCArchive ar) {
	//					if (ar.IsStoring) {
	//						var ver = 0;
	//						ar.Write(ver);
	//						ar.Write(DataUtility.Null2Empty(this.AccountDescription));
	//						ar.Write(DataUtility.Null2Empty(this.Password));
	//						ar.Write(this.Remember);
	//						ar.Write(this.HideChars);
	//					} else {
	//						var ver = ar.ReadInt32();
	//						this.AccountDescription = ar.ReadString();
	//						this.Password = ar.ReadString();
	//						this.Remember = ar.ReadBoolean();
	//						this.HideChars = ar.ReadBoolean();
	//					}
	//					ExtendSerializeSupport.ExtendSerializeDefault(this, ar);
	//				}
	//			}
	//			#endregion

	//			public readonly LabelTextBox AccountDescription;
	//			public readonly LabelTextBox Password;
	//			private readonly CheckBox Remember;
	//			private readonly CheckBox HideChars;
	//			private readonly Button OK;
	//			private readonly Button Cancel;
	//			private readonly LoginInformation Info;
	//			private readonly ITechWorking Server;

	//			public LoginDialog(LoginInformation info, ITechWorking svr) {
	//				this.Info = info ?? throw new ArgumentNullException("info");
	//				this.Server = svr ?? throw new ArgumentNullException("svr");
	//				var ch = new FormLayoutHelper(this, new Margin(12, 18, 12, 12), 280) { LabelSpace = 8 };
	//				var acc = this.AccountDescription = ch.CreateLabelControl<LabelTextBox>("账户名:", -8, Anchors.TopHoriz);
	//				var psw = this.Password = ch.CreateLabelControl<PasswordTextBox>("密码:", -8, Anchors.TopHoriz);
	//				var rem = this.Remember = ch.CreateButton<CheckBox>("记住密码", Anchors.TopLeft);
	//				var hcs = this.HideChars = ch.CreateButton<CheckBox>("隐藏密码字符", Anchors.TopLeft);
	//				var ok = this.OK = ch.CreateButton(FormButtons.OK);
	//				var ccl = this.Cancel = ch.CreateButton(FormButtons.Cancel);
	//				ch.BeginForm();
	//				ch.SetLabelWidth();
	//				acc.Bounds = ch.LeftAlloc(acc.Height);
	//				acc.Text = DataUtility.PackString(Encoding.UTF8.GetString(info.AccountDescription));
	//				acc.TextChanged += this.Account_TextChanged;
	//				psw.Bounds = ch.LeftAlloc(psw.Height);
	//				psw.Text = DataUtility.PackString(Encoding.UTF8.GetString(info.Password));
	//				psw.TextChanged += this.Password_TextChanged;
	//				rem.Bounds = ch.LeftAllocRowWithPreferredSize(rem);
	//				hcs.Bounds = GRect.Offset(ch.LeftAllocRowWithPreferredSize(hcs), 0, -4);
	//				hcs.Checked = true;
	//				hcs.CheckedChanged += this.HideChars_CheckedChanged;
	//				psw.HorizAlignExternalControl(rem);
	//				psw.HorizAlignExternalControl(hcs);
	//				ch.NextRow(15);
	//				ccl.Bounds = ch.RightAllocButtonBounds();
	//				ccl.Click += this.Cancel_Click;
	//				ok.Bounds = ch.RightAllocButtonBounds();
	//				ok.Click += this.OK_Click;
	//				ch.EndForm("登录", ok, ccl, FormBorderStyle.FixedDialog);
	//				this.LoadPersistProperties();
	//				this.UpdateState();
	//			}

	//			protected override void OnClosed(EventArgs e) {
	//				this.SavePersistProperties();
	//				base.OnClosed(e);
	//			}

	//			[UIStateSerialize]
	//			internal IArchiveSerializable PersistStore {
	//				get {
	//					var x = new UIStatePersistStore(this);
	//					if (!this.Remember.Checked) {
	//						x.Password = string.Empty;
	//					}
	//					return x;
	//				}
	//				set {
	//					if (value is UIStatePersistStore stg) {
	//						stg.Restore(this);
	//					}
	//				}
	//			}

	//			private void UpdateState() {
	//				var acc = this.AccountDescription;
	//				var psw = this.Password;
	//				var psi = psw.InnerTextBox;
	//				var pca = this.HideChars.Checked ? '●' : '\0';
	//				if (psi.PasswordChar != pca) {
	//					psi.PasswordChar = pca;
	//				}
	//				this.OK.Enabled = !string.IsNullOrEmpty(acc.Text) && !string.IsNullOrEmpty(psw.Text);
	//			}
	//			private void Account_TextChanged(object sender, EventArgs e) {
	//				this.UpdateState();
	//			}
	//			private void Password_TextChanged(object sender, EventArgs e) {
	//				this.UpdateState();
	//			}
	//			private void HideChars_CheckedChanged(object sender, EventArgs e) {
	//				this.UpdateState();
	//			}
	//			private void OK_Click(object sender, EventArgs e) {
	//				try {
	//					var lc = new LoginInformation {
	//						AccountDescription = Encoding.UTF8.GetBytes(DataUtility.PackString(this.AccountDescription.Text).ToUpper()),
	//						Password = Encoding.UTF8.GetBytes(DataUtility.PackString(this.Password.Text))
	//					};
	//					this.Server.Verify(lc.GetLoginToken());
	//					this.Info.AccountDescription = ArraySupport.Clone(lc.AccountDescription);
	//					this.Info.Password = ArraySupport.Clone(lc.Password);
	//					StatusNotifyHandlerManager.ShowStateText("登录成功");
	//					this.DialogResult = DialogResult.OK;
	//					this.Close();
	//				}
	//				catch (Exception ec) {
	//					this.ReportErrorMessage(ec);
	//				}
	//			}
	//			private void Cancel_Click(object sender, EventArgs e) {
	//				this.DialogResult = DialogResult.Cancel;
	//				this.Close();
	//			}
	//		}
	//		#endregion

	//		private static readonly object __locker = new object();
	//		private static LoginInformation __default = null;

	//		private byte[] AccountDescription = Encoding.UTF8.GetBytes(__Settings.Default.Account);
	//		private byte[] Password = new byte[] { 48, 48, 48, 48, 48, 48 };//[UTF8]:"000000"

	//		public LoginInformation() {
	//		}
	//		public DialogResult ShowDialog(ITechWorking svr) {
	//			using (var dlg = new LoginDialog(this, svr)) {
	//				return dlg.ShowDialog(CFCForm.GetRegisteredMainFrame());
	//			}
	//		}
	//		public LoginToken GetLoginToken() {
	//			var bs = ArraySupport.Concat(this.AccountDescription, new byte[] { 0, 0 }, this.Password);
	//			return new LoginToken(bs);
	//		}
	//		public static LoginInformation GetDefault() {
	//			lock (__locker) {
	//				var x = __default;
	//				if (x == null) {
	//					__default = x = new LoginInformation();
	//				}
	//				return x;
	//			}
	//		}
	//	}
	//	#endregion
	//	#region Commands
	//	[ImageList]
	//	public enum Commands
	//	{
	//		[CommandNotation("测试A...\n测试A", ImageIndex = 1)]
	//		TestA,
	//		[CommandNotation("测试B...\n测试B", ImageIndex = 2)]
	//		TestB,
	//	}
	//	#endregion
	//	#region CommandToolBar
	//	private class CommandToolBar : TransparentBackCommandToolBar
	//	{
	//		public CommandToolBar() {
	//		}
	//		public override int PreferredHeight {
	//			get {
	//				return 24;
	//			}
	//		}
	//		public override bool FirstSeparaterVisible {
	//			get {
	//				return true;
	//			}
	//		}
	//	}
	//	#endregion
	//	#region CatalogListView
	//	private class CatalogListView : CFCListView
	//	{
	//		private ImageList m_imageList;

	//		public CatalogListView() {
	//			m_imageList = ImageListManager.LoadDefault().CreateImageList(37);
	//			this.SmallImageList = m_imageList;
	//		}
	//		public void CreateLeftColumns() {
	//			this.Columns.Clear();
	//			this.AddColumns("目录", 200);
	//			this.FullRowSelect = false;
	//			this.HideSelection = false;
	//			this.AllowDrop = true;
	//			this.MultiSelect = false;
	//		}
	//		public void CreateRightColumns() {
	//			this.Columns.Clear();
	//			this.AddColumns("姓名", 200);
	//			this.AddColumns("部门", 200);
	//			this.AddColumns("权限", 200);
	//			this.FullRowSelect = true;
	//			this.HideSelection = false;
	//			this.AllowDrop = true;
	//			this.MultiSelect = true;
	//		}
	//		protected override void Dispose(bool disposing) {
	//			if (disposing) {
	//				ImageList imageList = m_imageList;
	//				m_imageList = null;
	//				this.SmallImageList = null;
	//				imageList?.Dispose();
	//			}
	//			base.Dispose(disposing);
	//		}
	//		[Command(Commands.TestB)]
	//		private void Comman_TestB(CommandArgs e) {
	//			if (e.IsUpdate) {
	//				e.Command.Enabled = true;
	//			} else {
	//				MessageBox.Show("测试B");
	//			}
	//		}
	//	}
	//	#endregion
	//	#region LVItem
	//	private class LVItem : ListViewItem
	//	{
	//		private string m_name;

	//		public LVItem(string name) {
	//			this.m_name = name;
	//			this.Checked = false;
	//			this.Font=new Font("新宋体",13);
	//			this.Update();

	//		}
	//		public LVItem(string[] name) {
	//			string ID = name[0];
	//			string Name = name[1];
	//			string Department = name[2];
	//			base.SubItems.Clear();
	//			this.Text = ID;
	//			base.SubItems.AddRange(new string[] {Name, Department });
	//		}
	//		public LVItem(Account account) {
	//			if (account == null) {
	//				throw new ArgumentNullException("account");
	//			}
	//			this.Checked = false;
	//			string Name = account.AccountName;
	//			string Department = account.Department;
	//			string Permission = EnumNotationAttribute.GetEnumNotation(account.Permission);
	//			this.Text = Name;
	//			this.SubItems.AddRange(new string[2] {  Department, Permission });
	//		}

	//		public string ItemName {
	//			get {
	//				return this.m_name;
	//			}
	//			set {
	//				this.m_name = value;
	//			}
	//		}
	//		public void Update() {
	//			this.Text = this.m_name;
	//		}
	//	}
	//	#endregion
	//	#region CatalogType
	//	public enum CatalogType
	//	{
	//		[EnumNotation("工作台")]
	//		Console,
	//		[EnumNotation("员工管理")]
	//		EmployeeManagement,
	//		[EnumNotation("部门架构")]
	//		DepartmentStructure,
	//		[EnumNotation("工作任务")]
	//		WorkTask
	//	}
	//	#endregion

	//	private const uint __Mask_FirstActivated = 0x0001;

	//	private static readonly object __locker = new object();
	//	private static IHttpUri __uri = null;
	//	private CFCStatusBar StatusBar;
	//	private AccountEditerPage AccountPage;
	//	private DepartmentEditerPage DepartmentPage;
	//	private ReferBitwiseState State = new ReferBitwiseState(0);

	//	private CommandToolBar ViewToolBar;
	//	private CatalogListView leftView;
	//	private CatalogListView rightView;
	//	private SplitContainer m_SplitContainer;

	//	public MainForm() {

	//	}
	//	private void InitForm() {
	//		var ch = new FormLayoutHelper(this, new Margin(0), 1100) { LabelSpace = 8 };
	//		var sb = this.StatusBar = ch.CreateControl<CFCStatusBar>(Anchors.BottomHoriz);
	//		this.ViewToolBar = ch.CreateToolBar<CommandToolBar>(Anchors.TopHoriz);
	//		this.ViewToolBar.GetCommandTarget += ViewToolBar_GetCommandTarget;
	//		this.ViewToolBar.GetNextCommandChain += ViewToolBar_GetNextCommandChain;

	//		m_SplitContainer = ch.CreateControl<SplitContainer>(Anchors.All);
	//		m_SplitContainer.IsSplitterFixed = false;
	//		ch.BeginForm();
	//		this.ViewToolBar.Bounds = ch.LeftAlloc(this.ViewToolBar.PreferredHeight);

	//		ch.NextRow(4);

	//		m_SplitContainer.Bounds = ch.LeftAlloc(520);

	//		sb.Bounds = ch.LeftAlloc(sb.PreferredHeight);
	//		ch.EndForm("TechWorking", FormBorderStyle.Sizable);

	//		m_SplitContainer.FixedPanel = FixedPanel.Panel1;
	//		m_SplitContainer.SplitterDistance = 250;
	//		m_SplitContainer.Panel1MinSize = 200;
	//		m_SplitContainer.Panel2MinSize = 200;
	//		m_SplitContainer.BorderStyle = BorderStyle.None;
	//		{
	//			var pane1 = m_SplitContainer.Panel1;
	//			var lh = new FormLayoutHelper(pane1, pane1.Width) { LabelSpace = 8 };
	//			this.leftView = lh.CreateControl<CatalogListView>(Anchors.All);

	//			lh.BeginForm();
	//			this.leftView.Bounds = lh.LeftAlloc(leftView.PreferredSize);
	//			leftView.CreateLeftColumns();
	//			leftView.DoubleClick += LeftView_DoubleClick;
	//			lh.EndForm();

	//			leftView.BeginUpdate();
	//			ListView.ListViewItemCollection items = leftView.Items;
	//			ListViewGroupCollection groups = leftView.Groups;
	//			items.Clear();
	//			groups.Clear();
	//			foreach(CatalogType a in Enum.GetValues(typeof(CatalogType))) {
	//				items.Add(new LVItem(EnumNotationAttribute.GetEnumNotation(a)));
	//			}
	//			leftView.EndUpdate();
	//			leftView.SelectedItemChanged += LeftView_SelectedItemChanged;

	//		}
	//		{
	//			var pane2 = m_SplitContainer.Panel2;
	//			var lh = new FormLayoutHelper(pane2, pane2.Width) { LabelSpace = 8 };
	//			this.rightView = lh.CreateControl<CatalogListView>(Anchors.All);
	//			//var pg = this.AccountPage = lh.CreateControl<AccountEditerPage>(Anchors.All);

	//			lh.BeginForm();
	//			//pg.Bounds = lh.LeftAlloc(pg.PreferredSize);
	//			this.rightView.Bounds = lh.LeftAlloc(rightView.PreferredSize);

	//			lh.EndForm();

	//			rightView.CreateRightColumns();
	//			rightView.BeginUpdate();
	//			ListView.ListViewItemCollection items = rightView.Items;
	//			ListViewGroupCollection groups = rightView.Groups;
	//			items.Clear();
	//			groups.Clear();
	//			//LVItem[] lis = new LVItem[] {
	//			//	new LVItem(new string[]{ "123","zhangsan","aa"}),
	//			//	new LVItem(new string[]{ "124","lisi","bb"}),
	//			//	new LVItem(new string[]{ "126","wangwu","cc"}),
	//			//};
	//			//var aa = MainForm.__GetServer();
	//			//var bb = MainForm.__GetLoginToken();
	//			var accs = MainForm.__GetServer()?.LoadAccounts(MainForm.__GetLoginToken());
	//			foreach (var acc in accs) {
	//				items.Add(new LVItem(acc));
	//			}
	//			//items.AddRange(lis);
	//			rightView.EndUpdate();
	//		}

	//		this.ViewToolBar.Initialize(__GetToolBarCommands());

	//		this.StartPosition = FormStartPosition.CenterScreen;
	//		this.ShowInTaskbar = true;
	//	}

	//	private void InitAccountPage() {
	//		var ch = new FormLayoutHelper(this, new Margin(0), 1100) { LabelSpace = 8 };
	//		var pg = this.AccountPage = ch.CreateControl<AccountEditerPage>(Anchors.All);
	//		var sb = this.StatusBar = ch.CreateControl<CFCStatusBar>(Anchors.BottomHoriz);
	//		ch.BeginForm();
	//		pg.Bounds = ch.LeftAlloc(700);
	//		sb.Bounds = ch.LeftAlloc(sb.PreferredHeight);
	//		ch.EndForm("TechWorking", FormBorderStyle.Sizable);
	//		this.StartPosition = FormStartPosition.CenterScreen;
	//		this.ShowInTaskbar = true;
	//	}

	//	private void InitDepartmentPage() {
	//		var ch = new FormLayoutHelper(this, new Margin(0), 1100) { LabelSpace = 8 };
	//		var pg = this.DepartmentPage = ch.CreateControl<DepartmentEditerPage>(Anchors.All);
	//		var sb = this.StatusBar = ch.CreateControl<CFCStatusBar>(Anchors.BottomHoriz);
	//		ch.BeginForm();
	//		pg.Bounds = ch.LeftAlloc(700);
	//		sb.Bounds = ch.LeftAlloc(sb.PreferredHeight);
	//		ch.EndForm("TechWorking", FormBorderStyle.Sizable);
	//		this.StartPosition = FormStartPosition.CenterScreen;
	//		this.ShowInTaskbar = true;
	//	}

	//	private void LeftView_DoubleClick(object sender, EventArgs e) {
	//		//using (AccountEditerPage dialog = new AccountEditerPage()) {
	//		//	dialog.Show();
	//		//}
	//	}

	//	private void LeftView_SelectedItemChanged(object sender, EventArgs e) {

	//	}

	//	public void Login() {
	//		try {
	//			var svc = __GetServer();
	//			var x = LoginInformation.GetDefault();
	//			if (x.ShowDialog(svc) != DialogResult.OK) {
	//				throw new __ExceptionExt("没有完成登录，后续的操作需要登录才可进行。");
	//			} else {
	//				try {
	//					InitForm();
	//					//InitAccountPage();
	//					//InitDepartmentPage();
	//				}
	//				catch {

	//				}
	//			}
	//		}
	//		catch (Exception ec) {
	//			this.ReportErrorMessage(ec);
	//		}
	//	}

	//	public static ISessionService __GetHostService() {
	//		lock (__locker) {
	//			var u = __uri;
	//			if (u == null) {
	//				__uri = u = new HttpUriImpl(__Settings.Default.HostUrl);
	//			}
	//			return SessionServiceCache.GetService(u);
	//		}
	//	}
	//	public static ITechWorking __GetServer() {
	//		return __GetHostService().GetInvoker<ITechWorking>();
	//	}
	//	public static LoginToken __GetLoginToken() {
	//		return LoginInformation.GetDefault().GetLoginToken();
	//	}

	//	protected override void OnHandleCreated(EventArgs e) {
	//		base.OnHandleCreated(e);
	//		this.RegisterMainFrame();
	//	}
	//	protected override void OnActivated(EventArgs e) {
	//		base.OnActivated(e);
	//		if (this.State.Add(__Mask_FirstActivated)) {
	//			this.Login();
	//		}
	//	}

	//	string IStatusNotifyHandler.StateText {
	//		get => this.StatusBar.StateText;
	//		set { }
	//		//=> this.StatusBar.StateText = value;
	//	}
	//	IProgressBar IStatusNotifyHandler.GetStateProgressBar() {
	//		var x = this.StatusBar is IStatusNotifyHandler __h ? __h : null;
	//		return x?.GetStateProgressBar();
	//	}

	//	public object NextChain {
	//		get {
	//			return this.leftView;
	//		}
	//	}
	//	private void ViewToolBar_GetCommandTarget(object sender, GetCommandTargetEventArgs e) {
	//		e.CommandTarget = this;
	//	}
	//	private void ViewToolBar_GetNextCommandChain(object sender, NextCommandChainEventArgs e) {
	//		e.CommandChain = this;
	//	}
	//	[Command(Commands.TestA)]
	//	private void Comman_TestA(CommandArgs e) {
	//		if (e.IsUpdate) {
	//			e.Command.Enabled = true;
	//		} else {
	//			MessageBox.Show("测试A");
	//		}
	//	}
	//	private static object[] __GetToolBarCommands() {
	//		return new ICommandItem[] {
	//				 CommandItem.GetNamed(PredefCommands.EDIT_UNDO),
	//				 CommandItem.GetNamed(PredefCommands.EDIT_REDO),
	//				 CommandSeparator.Default,
	//				 CommandItem.GetNamed(PredefCommands.EDIT_ZOOMIN),
	//				 CommandItem.GetNamed(PredefCommands.EDIT_ZOOMOUT),
	//				 CommandItem.GetNamed(PredefCommands.EDIT_RECT_ZOOMIN),
	//				 CommandItem.GetNamed(PredefCommands.EDIT_CANCEL_RECT_ZOOMIN),
	//				 CommandSeparator.Default,
	//				 CommandItem.GetNamed(Commands.TestA),
	//				 CommandItem.GetNamed(Commands.TestB),
	//			};
	//	}
	//}

	public partial class MainForm : XSRForm, IStatusNotifyHandler
	{
		#region SettingsEntry
		private class SettingsEntry : IListItem
		{
			private readonly MainForm m_dialog;
			private readonly WorkSettingsBase m_settings;
			private WorkingEditerPageBase m_panel;

			public SettingsEntry(MainForm form, WorkSettingsBase settings) {
				if (form == null) {
					throw new ArgumentNullException("form");
				}
				m_dialog = form;
				m_settings = settings;
			}
			public string Name {
				get {
					return m_settings.Name;
				}
			}
			public WorkSettingsBase Settings {
				get {
					return m_settings;
				}
			}
			public WorkingEditerPageBase GetPanel(bool create) {
				WorkingEditerPageBase panel = m_panel;
				if ((panel == null) && create) {
					panel = m_settings.OnCreaterEditerPage();
					m_panel = panel;
					m_panel.Disposed += new EventHandler(this.Panel_Disposed);
				}
				return panel;
			}
			private void Panel_Disposed(object sender, EventArgs e) {
				m_panel = null;
			}
		}
		#endregion
		#region SettingsSelectControl
		private class SettingsSelectControl : EntrySelector
		{
			private MainForm m_dialog;

			public SettingsSelectControl(MainForm owner) {
				if (owner == null) {
					throw new ArgumentNullException("owner");
				}
				this.Owner = owner;
				base.ItemHeight = 36;
				base.Items.AddRange(owner.m_entrys);
			}
			public MainForm Owner {
				get {
					return m_dialog;
				}
				private set {
					m_dialog = value;
				}
			}
			public void UpdateActivePanelChanged(Control oldPanel, Control newPanel) {
				int itemIndexFromPanel = this.GetItemIndexFromPanel(newPanel);
				if (itemIndexFromPanel >= 0) {
					base.SelectedIndex = itemIndexFromPanel;
				}
			}
			protected override void OnParentChanged(EventArgs args) {
				base.OnParentChanged(args);
				if (this.Owner != null) {
					base.DelayUpdateScrollBar();
				}
			}
			protected override void OnSelectedIndexChanged(EventArgs args) {
				var selectedItem = base.SelectedItem;
				var entry = (selectedItem != null) ? (selectedItem as SettingsEntry) : null;
				if (entry != null) {
					var owner = this.Owner;
					if (owner != null) {
						owner.ActivePanel = entry.GetPanel(true);
					}
				}
			}
			private int GetItemIndexFromPanel(Control panel) {
				if (panel != null) {
					WorkingEditerPageBase panel2 = panel as WorkingEditerPageBase;
					if (panel2 != null) {
						EntrySelector.ItemCollection items = base.Items;
						for (int i = items.Count - 1; i >= 0; i--) {
							SettingsEntry entry = items[i] as SettingsEntry;
							if ((entry != null) && (entry.Settings == panel2.Settings)) {
								return i;
							}
						}
					}
				}
				return -1;
			}
		}
		#endregion
		#region LoginInformation
		[Obfuscation(Feature = "all", Exclude = false)]
		private class LoginInformation
		{
			#region LoginDialog
			[UIStatePropertyPersist("1386998E-D1A6-4EFB-8449-17E8C9DB23E0")]
			private class LoginDialog : XSRForm
			{
				#region PasswordTextBox
				private class PasswordTextBox : LabelTextBox
				{
					public PasswordTextBox() {
						base.AllowCustomFilterChar = true;
					}
					public override bool AllowCustomFilterChar {
						get => base.AllowCustomFilterChar;
						set { }
					}
					protected override void OnCustomFilterChar(TextBoxFiterCharEventArgs e) {
						var c = e.ConvertedChar;
						e.Cancel = !((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'Z'));
					}
				}
				#endregion
				#region UIStatePersistStore
				private class UIStatePersistStore : IArchiveSerializable
				{
					public UIStatePersistStore() {
					}
					public UIStatePersistStore(LoginDialog dlg) {
						this.Account = dlg.Account.Text;
						this.Password = dlg.Password.Text;
						this.Remember = dlg.Remember.Checked;
						this.HideChars = dlg.HideChars.Checked;
					}
					public string Account {
						get;
						set;
					} = string.Empty;
					public string Password {
						get;
						set;
					} = string.Empty;
					public bool Remember {
						get;
						set;
					} = true;
					public bool HideChars {
						get;
						set;
					} = true;
					public void Restore(LoginDialog dlg) {
						if (!string.IsNullOrWhiteSpace(this.Account)) {
							dlg.Account.Text = this.Account;
						}
						if (!string.IsNullOrWhiteSpace(this.Password)) {
							dlg.Password.Text = this.Password;
						}
						dlg.Remember.Checked = this.Remember;
						dlg.HideChars.Checked = this.HideChars;
					}
					public void Serialize(CFCArchive ar) {
						if (ar.IsStoring) {
							var ver = 0;
							ar.Write(ver);
							ar.Write(DataUtility.Null2Empty(this.Account));
							ar.Write(DataUtility.Null2Empty(this.Password));
							ar.Write(this.Remember);
							ar.Write(this.HideChars);
						} else {
							var ver = ar.ReadInt32();
							this.Account = ar.ReadString();
							this.Password = ar.ReadString();
							this.Remember = ar.ReadBoolean();
							this.HideChars = ar.ReadBoolean();
						}
						ExtendSerializeSupport.ExtendSerializeDefault(this, ar);
					}
				}
				#endregion

				public readonly LabelTextBox Account;
				public readonly LabelTextBox Password;
				private readonly CheckBox Remember;
				private readonly CheckBox HideChars;
				private readonly Button OK;
				private readonly Button Cancel;
				private readonly LoginInformation Info;
				private readonly ITechWorking Server;

				public LoginDialog(LoginInformation info, ITechWorking svr) {
					this.Info = info ?? throw new ArgumentNullException("info");
					this.Server = svr ?? throw new ArgumentNullException("svr");
					var ch = new FormLayoutHelper(this, new Margin(12, 18, 12, 12), 280) { LabelSpace = 8 };
					var acc = this.Account = ch.CreateLabelControl<LabelTextBox>("账户名:", -8, Anchors.TopHoriz);
					var psw = this.Password = ch.CreateLabelControl<PasswordTextBox>("密码:", -8, Anchors.TopHoriz);
					var rem = this.Remember = ch.CreateButton<CheckBox>("记住密码", Anchors.TopLeft);
					var hcs = this.HideChars = ch.CreateButton<CheckBox>("隐藏密码字符", Anchors.TopLeft);
					var ok = this.OK = ch.CreateButton(FormButtons.OK);
					var ccl = this.Cancel = ch.CreateButton(FormButtons.Cancel);
					ch.BeginForm();
					ch.SetLabelWidth();
					acc.Bounds = ch.LeftAlloc(acc.Height);
					acc.Text = DataUtility.PackString(Encoding.UTF8.GetString(info.Account));
					acc.TextChanged += this.Account_TextChanged;
					psw.Bounds = ch.LeftAlloc(psw.Height);
					psw.Text = DataUtility.PackString(Encoding.UTF8.GetString(info.Password));
					psw.TextChanged += this.Password_TextChanged;
					rem.Bounds = ch.LeftAllocRowWithPreferredSize(rem);
					hcs.Bounds = GRect.Offset(ch.LeftAllocRowWithPreferredSize(hcs), 0, -4);
					hcs.Checked = true;
					hcs.CheckedChanged += this.HideChars_CheckedChanged;
					psw.HorizAlignExternalControl(rem);
					psw.HorizAlignExternalControl(hcs);
					ch.NextRow(15);
					ccl.Bounds = ch.RightAllocButtonBounds();
					ccl.Click += this.Cancel_Click;
					ok.Bounds = ch.RightAllocButtonBounds();
					ok.Click += this.OK_Click;
					ch.EndForm("登录", ok, ccl, FormBorderStyle.FixedDialog);
					this.LoadPersistProperties();
					this.UpdateState();
				}

				protected override void OnClosed(EventArgs e) {
					this.SavePersistProperties();
					base.OnClosed(e);
				}

				[UIStateSerialize]
				internal IArchiveSerializable PersistStore {
					get {
						var x = new UIStatePersistStore(this);
						if (!this.Remember.Checked) {
							x.Password = string.Empty;
						}
						return x;
					}
					set {
						if (value is UIStatePersistStore stg) {
							stg.Restore(this);
						}
					}
				}

				private void UpdateState() {
					var acc = this.Account;
					var psw = this.Password;
					var psi = psw.InnerTextBox;
					var pca = this.HideChars.Checked ? '●' : '\0';
					if (psi.PasswordChar != pca) {
						psi.PasswordChar = pca;
					}
					this.OK.Enabled = !string.IsNullOrEmpty(acc.Text) && !string.IsNullOrEmpty(psw.Text);
				}
				private void Account_TextChanged(object sender, EventArgs e) {
					this.UpdateState();
				}
				private void Password_TextChanged(object sender, EventArgs e) {
					this.UpdateState();
				}
				private void HideChars_CheckedChanged(object sender, EventArgs e) {
					this.UpdateState();
				}
				private void OK_Click(object sender, EventArgs e) {
					try {
						var lc = new LoginInformation {
							Account = Encoding.UTF8.GetBytes(DataUtility.PackString(this.Account.Text).ToUpper()),
							Password = Encoding.UTF8.GetBytes(DataUtility.PackString(this.Password.Text))
						};
						this.Server.Verify(lc.GetLoginToken());
						this.Info.Account = ArraySupport.Clone(lc.Account);
						this.Info.Password = ArraySupport.Clone(lc.Password);
						StatusNotifyHandlerManager.ShowStateText("登录成功");
						this.DialogResult = DialogResult.OK;
						this.Close();
					}
					catch (Exception ec) {
						this.ReportErrorMessage(ec);
					}
				}
				private void Cancel_Click(object sender, EventArgs e) {
					this.DialogResult = DialogResult.Cancel;
					this.Close();
				}
			}
			#endregion

			private static readonly object __locker = new object();
			private static LoginInformation __default = null;

			private byte[] Account = Encoding.UTF8.GetBytes(__Settings.Default.Account);
			private byte[] Password = new byte[] { 48, 48, 48, 48, 48, 48 };//[UTF8]:"000000"

			public LoginInformation() {
			}
			public DialogResult ShowDialog(ITechWorking svr) {
				using (var dlg = new LoginDialog(this, svr)) {
					return dlg.ShowDialog(CFCForm.GetRegisteredMainFrame());
				}
			}
			public LoginToken GetLoginToken() {
				var bs = ArraySupport.Concat(this.Account, new byte[] { 0, 0 }, this.Password);
				return new LoginToken(bs);
			}
			public static LoginInformation GetDefault() {
				lock (__locker) {
					var x = __default;
					if (x == null) {
						__default = x = new LoginInformation();
					}
					return x;
				}
			}
		}
		#endregion

		private const uint __Mask_FirstActivated = 0x0001;

		private static readonly object __locker = new object();
		private static IHttpUri __uri = null;
		private readonly CFCStatusBar StatusBar;
		private readonly ReferBitwiseState State = new ReferBitwiseState(0);
		private readonly BorderContainerControl m_leftContainer;
		private readonly SettingsSelectControl m_selectControl;
		private readonly __WndProcExtensionControl m_rightContainer;
		private readonly SettingsEntry[] m_entrys = null;

		public MainForm() {

			m_entrys = __CreateEntries(this);

			var ch = new FormLayoutHelper(this, new Margin(2, 2, 2, 2), 1100) { LabelSpace = 8 };
			m_leftContainer = ch.CreateControl<BorderContainerControl>(Anchors.VertLeft);
			m_selectControl = new SettingsSelectControl(this);
			m_leftContainer.View = m_selectControl;
			m_rightContainer = ch.CreateControl<__WndProcExtensionControl>(Anchors.All);
			var sb = this.StatusBar = ch.CreateControl<CFCStatusBar>(Anchors.BottomHoriz);
			ch.BeginForm();
			var rectangle = ch.LeftAlloc(700);
			m_leftContainer.Bounds = GRect.SubLeftSect(ref rectangle, 200, 4);
			m_rightContainer.BorderStyle = BorderStyle.None;
			m_rightContainer.Bounds = rectangle;
			sb.Bounds = ch.LeftAlloc(sb.PreferredHeight);
			ch.EndForm("TechWorking", FormBorderStyle.Sizable);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.ShowInTaskbar = true;
		}
		public void Login() {
			try {
				var svc = __GetServer();
				var x = LoginInformation.GetDefault();
				if (x.ShowDialog(svc) != DialogResult.OK) {
					throw new __ExceptionExt("没有完成登录，后续的操作需要登录才可进行。");
				} else if(m_selectControl.Items.Count >0){
					m_selectControl.SelectedIndex = 0;
				}
			}
			catch (Exception ec) {
				this.ReportErrorMessage(ec);
			}
		}
		private WorkingEditerPageBase ActivePanel {
			get {
				if (m_rightContainer.Controls.Count > 0) {
					return m_rightContainer.Controls[0] as WorkingEditerPageBase;
				}
				return null;
			}
			set {
				m_rightContainer.Controls.Clear();
				if (value != null) {
					value.Dock = DockStyle.Fill;
					m_rightContainer.Controls.Add(value);
				}
			}
		}
		private SettingsSelectControl SelectControl {
			get {
				return m_leftContainer.GetView<SettingsSelectControl>();
			}
		}
		public static ISessionService __GetHostService() {
			lock (__locker) {
				var u = __uri;
				if (u == null) {
					__uri = u = new HttpUriImpl(__Settings.Default.HostUrl);
				}
				return SessionServiceCache.GetService(u);
			}
		}
		public static ITechWorking __GetServer() {
			return __GetHostService().GetInvoker<ITechWorking>();
		}
		public static LoginToken __GetLoginToken() {
			return LoginInformation.GetDefault().GetLoginToken();
		}

		protected override void OnHandleCreated(EventArgs e) {
			base.OnHandleCreated(e);
			this.RegisterMainFrame();
		}
		protected override void OnActivated(EventArgs e) {
			base.OnActivated(e);
			if (this.State.Add(__Mask_FirstActivated)) {
				this.Login();
			}
		}
		protected override void OnLayout(LayoutEventArgs levent) {
			base.OnLayout(levent);
		}
		private void SettingsPanel_ChangingView(object sender, TEventArgs<Control, Control> e) {
			//if (e.Value1 != null)
			//{
			//    XAppSettingsPanel panel = e.Value1 as XAppSettingsPanel;
			//    if (panel != null)
			//    {
			//        panel.OnSaveEditing();
			//    }
			//}
		}
		private void SettingsPanel_ViewChanged(object sender, TEventArgs<Control, Control> e) {
			SettingsSelectControl selectControl = this.SelectControl;
			if (selectControl != null) {
				selectControl.UpdateActivePanelChanged(e.Value1, e.Value2);
			}
		}
		private static SettingsEntry[] __CreateEntries(MainForm dialog) {
			return new SettingsEntry[] { new SettingsEntry(dialog, new DepartmentSettings()), new SettingsEntry(dialog, new AccountSettings()) };
		}

		string IStatusNotifyHandler.StateText {
			get => this.StatusBar.StateText;
			set => this.StatusBar.StateText = value;
		}
		IProgressBar IStatusNotifyHandler.GetStateProgressBar() {
			var x = this.StatusBar is IStatusNotifyHandler __h ? __h : null;
			return x?.GetStateProgressBar();
		}
	}

	public abstract class WorkSettingsBase
	{
		public abstract string Name {
			get;
		}
		public abstract WorkingEditerPageBase OnCreaterEditerPage();
	}

	public abstract class WorkingEditerPageBase : XSRUserControl
	{
		public WorkingEditerPageBase() {
		}
		public WorkSettingsBase Settings {
			get;
			protected set;
		}
	}
	public class AccountSettings : WorkSettingsBase
	{
		public AccountSettings() {

		}
		public override string Name {
			get {
				return "人员管理";
			}
		}
		public override WorkingEditerPageBase OnCreaterEditerPage() {
			return new AccountEditerPage(this);
		}
	}
	public class AccountEditerPage : WorkingEditerPageBase, ICommandChain
	{
		#region AccountListView
		private class AccountListView : CFCListView
		{
			public AccountListView() {
				this.SmallImageList = null;
			}
			public void CreateColumns() {
				this.Columns.Clear();
				AddColumns("姓名|Name", 150, " 部门|Department", 150, "权限|Permission");
				this.FullRowSelect = true;
				this.HideSelection = false;
				this.MultiSelect = true;
				this.CheckBoxes = false;
				AllowDrop = true;
			}
			protected override void Dispose(bool disposing) {
				base.Dispose(disposing);
			}
		}
		#endregion
		#region LVItem
		private class LVItem : ListViewItem
		{
			public Account account;

			public LVItem(Account account) {
				if (account == null) {
					throw new ArgumentNullException("account");
				}
				this.account = account;
				this.Checked = false;
				this.Update();
			}

			public void Update() {
				string Name = account.AccountName;
				string Department = account.Department;
				string Permission = EnumNotationAttribute.GetEnumNotation(account.Permission);
				this.Text = Name;
				this.SubItems.AddRange(new string[2] { Department, Permission });
			}
		}
		#endregion
		#region Commands
		[ImageList]
		private enum Commands
		{
			[CommandNotation("全选", "Select All")]
			SelectAll,
			[CommandNotation("增加", "Add", ImageIndex = 434)]
			Add,
			[CommandNotation("删除", "Delete", ImageIndex = 1085)]
			Delete,
			[CommandNotation("修改", "Change")]
			Change,
			[CommandNotation("批量导入", "Batch Import", ImageIndex = 995)]
			BatchImport,
		}
		#endregion
		#region CommandToolBar
		private class CommandToolBar : TransparentBackCommandToolBar
		{
			public CommandToolBar() {
			}
			public override int PreferredHeight {
				get {
					return 24;
				}
			}
			public override bool FirstSeparaterVisible {
				get {
					return true;
				}
			}
		}
		#endregion

		#region AddDialog
		protected class AddDialog : XSRForm
		{
			private LabelTextBox m_password;
			private LabelTextBox m_name;
			private LabelEnumComboBox m_department;
			private LabelEnumComboBox m_permission;

			private Button m_ok;
			private Button m_cancle;

			public AccountDescription m_account;

			public AddDialog(AccountDescription acc) {
				m_account = acc;

				InitForm();
			}
			private void InitForm() {
				var lh = new FormLayoutHelper(this, new Margin(10, 8, 10, 10), 400) { LabelSpace = 8 };
				m_name = lh.CreateLabelControl<LabelTextBox>("姓名: |Name", -8, Anchors.TopHoriz);
				m_password = lh.CreateLabelControl<LabelTextBox>("密码: |Password", -8, Anchors.TopHoriz);
				m_department = lh.CreateLabelControl<LabelEnumComboBox>("部门: | Department", -8, Anchors.TopHoriz);
				m_permission = lh.CreateLabelControl<LabelEnumComboBox>("权限: | Permission", -8, Anchors.TopHoriz);
				m_ok = lh.CreateButton(FormButtons.OK);
				m_cancle = lh.CreateButton(FormButtons.Cancel);

				lh.BeginForm();
				m_name.Bounds = lh.LeftAlloc(m_name.Height);
				m_password.Bounds = lh.LeftAlloc(m_password.Height);
				m_department.Bounds = lh.LeftAlloc(m_department.Height);
				m_permission.Bounds = lh.LeftAlloc(m_permission.Height);

				m_name.Text = m_account.AccountName;
				m_password.Text ="123456";
				m_password.Enabled = false;

				m_department.Items.Clear();
				var deps=MainForm.__GetServer().LoadDepartments(MainForm.__GetLoginToken());
				foreach(var dep in deps) {
					m_department.Items.Add(dep.Name);
				}
				m_permission.AddItems(typeof(TechWorkingPermission), TechWorkingPermission.All);
				m_department.SetSelectedValue(m_account.AccountName,deps[0].Name);
				m_permission.SetSelectedValue(m_account.Permission, TechWorkingPermission.All);

				m_cancle.Bounds = lh.RightAlloc(70, 24);
				m_ok.Bounds = lh.RightAlloc(70, 24);
				m_cancle.Click += this.Cancle_Click;
				m_ok.Click += this.Ok_Click;

				lh.EndForm("添加新员工|Add New Employee", this.m_ok, this.m_cancle, FormBorderStyle.FixedDialog);
				


			}

			private void Ok_Click(object sender, EventArgs e) {
				try {
					var name = this.m_name.Text;
					name = DataTableColumnDefinition.NormmalizeColumnName(name, true);
					if (string.IsNullOrEmpty(name)) {
						throw new Exception(XSR.Select("员工姓名不能为空!", "Employee name cannot be empty!"));
					}
					var department = this.m_department.Text;
					department = DataTableColumnDefinition.NormmalizeColumnName(department, true);
					if (string.IsNullOrEmpty(department)) {
						throw new Exception(XSR.Select("所在部门不能为空!", "Department cannot be empty!"));
					}

					m_account=new AccountDescription(name, department,m_password.Text, m_permission.GetSelectedValueT(TechWorkingPermission.All), false);

					this.DialogResult = DialogResult.OK;
					this.Close();

				}
				catch (Exception exc) {
					ErrorMessageBox.Show(exc, ShowErrorOption.Message);
				}
			}
			private void Cancle_Click(object sender, EventArgs e) {
				try {
					this.DialogResult = DialogResult.Cancel;
					this.Close();
				}
				catch (Exception exc) {
					ErrorMessageBox.Show(exc, ShowErrorOption.Message);
				}
			}
			private void AddNewEmployee() {

			}
		}
		#endregion

		#region EditorDialog
		public class EditorDialog : XSRForm
		{
			private LabelTextBox m_name;
			private LabelEnumComboBox m_department;
			private LabelEnumComboBox m_permission;

			private Button m_ok;
			private Button m_cancle;

			public Account m_account;

			public EditorDialog(Account acc) {
				m_account = acc;
				InitForm();
			}

			private void InitForm() {
				var lh = new FormLayoutHelper(this, new Margin(10, 8, 10, 10), 400) { LabelSpace = 8 };
				m_name = lh.CreateLabelControl<LabelTextBox>("姓名: |Name", -8, Anchors.TopHoriz);
				m_department = lh.CreateLabelControl<LabelEnumComboBox>("部门: | Department", -8, Anchors.TopHoriz);
				m_permission = lh.CreateLabelControl<LabelEnumComboBox>("权限: | Permission", -8, Anchors.TopHoriz);
				m_ok = lh.CreateButton(FormButtons.OK);
				m_cancle = lh.CreateButton(FormButtons.Cancel);

				lh.BeginForm();
				m_name.Bounds = lh.LeftAlloc(m_name.Height);
				m_department.Bounds = lh.LeftAlloc(m_department.Height);
				m_permission.Bounds = lh.LeftAlloc(m_permission.Height);

				m_department.Items.Clear();
				var deps = MainForm.__GetServer().LoadDepartments(MainForm.__GetLoginToken());
				foreach (var dep in deps) {
					m_department.Items.Add(dep.Name);
				}
				m_permission.AddItems(typeof(TechWorkingPermission), TechWorkingPermission.All);

				m_name.Text = m_account.AccountName;
				m_department.SetSelectedValue(m_account.AccountName, deps[0].Name);
				m_permission.SetSelectedValue(m_account.Permission, TechWorkingPermission.All);
				m_cancle.Bounds = lh.RightAlloc(70, 24);
				m_ok.Bounds = lh.RightAlloc(70, 24);
				m_cancle.Click += this.Cancle_Click;
				m_ok.Click += this.Ok_Click;
				lh.EndForm("编辑员工信息|Edit Employee Information", this.m_ok, this.m_cancle, FormBorderStyle.FixedDialog);
			}
			private void Ok_Click(object sender, EventArgs e) {
				try {
					var name = this.m_name.Text;
					name = DataTableColumnDefinition.NormmalizeColumnName(name, true);
					if (string.IsNullOrEmpty(name)) {
						throw new Exception(XSR.Select("员工姓名不能为空!", "Employee name cannot be empty!"));
					}
					var department = this.m_department.Text;
					department = DataTableColumnDefinition.NormmalizeColumnName(department, true);
					if (string.IsNullOrEmpty(department)) {
						throw new Exception(XSR.Select("所在部门不能为空!", "Department cannot be empty!"));
					}

					m_account = new Account(m_account.AccountID,name, department,  m_permission.GetSelectedValueT(TechWorkingPermission.All), false);

					this.DialogResult = DialogResult.OK;
					this.Close();

				}
				catch (Exception exc) {
					ErrorMessageBox.Show(exc, ShowErrorOption.Message);
				}
			}
			private void Cancle_Click(object sender, EventArgs e) {
				try {
					this.DialogResult = DialogResult.Cancel;
					this.Close();
				}
				catch (Exception exc) {
					ErrorMessageBox.Show(exc, ShowErrorOption.Message);
				}
			}
		}
		#endregion

		private AccountListView View;
		private CommandToolBar ViewToolBar;

		public AccountEditerPage(AccountSettings settings) {
			this.Settings = settings ?? throw new ArgumentNullException("AccountSettings settings");

			InitForm();
		}
		private void InitForm() {
			var ch = new FormLayoutHelper(this, new Margin(2, 0, 2, 0), 600) { LabelSpace = 4 };
			this.ViewToolBar = ch.CreateToolBar<CommandToolBar>(Anchors.TopHoriz);
			this.ViewToolBar.GetCommandTarget += ViewToolBar_GetCommandTarget;
			this.ViewToolBar.GetNextCommandChain += ViewToolBar_GetNextCommandChain;
			this.View = ch.CreateControl<AccountListView>(Anchors.All);
			ch.BeginForm();
			this.ViewToolBar.Bounds = ch.LeftAlloc(this.ViewToolBar.PreferredHeight);
			this.View.Bounds = ch.LeftAlloc(500);
			this.View.CreateColumns();
			ch.EndForm("人员管理页面");

			this.ViewToolBar.Initialize(__GetToolBarCommands());


			View.BeginUpdate();
			ListView.ListViewItemCollection items = View.Items;
			ListViewGroupCollection groups = View.Groups;
			items.Clear();
			groups.Clear();
			var accs = MainForm.__GetServer()?.LoadAccounts(MainForm.__GetLoginToken());
			foreach (var acc in accs) {
				items.Add(new LVItem(acc));
			}
			View.EndUpdate();

			View.BuildMenu += View_BuildMenu;
			View.ItemDrag += View_ItemDrag;
			View.DragEnter += View_DragEnter;
			View.DragOver += View_DragOver;
			View.DragDrop += View_DragDrop;
			View.DoubleClick += View_DoubleClick;

		}

		private void View_DoubleClick(object sender, EventArgs e) {
			View.BeginUpdate();
			var acc = (View.SelectedItem as LVItem).account;
			using (EditorDialog dialog = new EditorDialog(acc)) {
				var result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					if (!acc.Permission.Equals(dialog.m_account.Permission)) {
						MainForm.__GetServer().ChangeAccountPermission(MainForm.__GetLoginToken(), acc.AccountName, dialog.m_account.Permission);
					}
					if (!string.Equals(acc.AccountName, dialog.m_account.AccountName)) {
						MainForm.__GetServer().ChangeAccountName(MainForm.__GetLoginToken(), acc.AccountName, dialog.m_account.AccountName);
					}
					if (!string.Equals(acc.Department, dialog.m_account.Department)) {
						MainForm.__GetServer().ChangeAccountDepartment(MainForm.__GetLoginToken(), acc.AccountName, dialog.m_account.Department);
					}

				}
			}
			this.RefreshView();
			View.EndUpdate();
		}

		private void View_DragDrop(object sender, DragEventArgs e) {
			LVItem draggedItem = e.Data.GetData(typeof(LVItem)) as LVItem;
			if (draggedItem != null) {
				e.Effect = DragDropEffects.None;
				Point dpt = new Point(e.X, e.Y);
				Point lpt = View.PointToClient(dpt);
				int idx = View.InsertionMark.NearestIndex(lpt);
				int c = View.Items.Count;
				int index = draggedItem.Index;
				ListViewItem hitItem = null;
				if (idx >= 0 && idx < c && !(hitItem = View.Items[idx]).Selected) {
					int sc = View.SelectedIndices.Count;
					if (sc > 0) {
						int[] ids = new int[sc];
						View.SelectedIndices.CopyTo(ids, 0);
						Array.Sort(ids);
						View.BeginUpdate();
						try {
							List<ListViewItem> list = new List<ListViewItem>();
							for (int i = sc - 1; i >= 0; i--) {
								ListViewItem item = View.Items[ids[i]];
								list.Insert(0, item);
								item.Remove();
							}
							idx = hitItem.Index;
							for (int i = sc - 1; i >= 0; i--) {
								View.Items.Insert(idx, list[i]);
							}
						}
						finally {
							View.EndUpdate();
						}
					}
				}
			}
			e.Effect = DragDropEffects.None;
			View.InsertionMark.Index = -1;
		}
		private void View_DragOver(object sender, DragEventArgs e) {
			bool mark = false;
			LVItem draggedItem = e.Data.GetData(typeof(LVItem)) as LVItem;
			if (draggedItem != null) {
				e.Effect = DragDropEffects.Move;
				Point dpt = new Point(e.X, e.Y);
				Point lpt = View.PointToClient(dpt);
				ListViewItem item = View.GetItemAt(lpt.X, lpt.Y);
				if (item != null) {
					int idx = item.Index;
					View.EnsureVisible(idx);
					if (item != draggedItem) {
						View.InsertionMark.Index = idx;
						mark = true;
					}
				}
			} else {
			}
			if (!mark) {
				View.InsertionMark.Index = -1;
			}
		}
		private void View_DragEnter(object sender, DragEventArgs e) {
			LVItem draggedItem = e.Data.GetData(typeof(LVItem)) as LVItem;
			if (draggedItem != null) {
				e.Effect = DragDropEffects.Move;
			} else {
				e.Effect = DragDropEffects.None;
			}
		}
		private void View_ItemDrag(object sender, ItemDragEventArgs e) {
			View.DoDragDrop(e.Item, DragDropEffects.Move);
		}
		private void View_BuildMenu(object sender, MenuBuilder e) {
			if (e.CanAddSeparator) {
				e.AddSeparator();
			}
			e.Add(Commands.SelectAll);
			e.Add(Commands.Add);
			e.Add(Commands.Delete);
			e.Add(Commands.Change);
		}
		public object NextChain {
			get {
				return this.View;
			}
		}
		private void ViewToolBar_GetCommandTarget(object sender, GetCommandTargetEventArgs e) {
			e.CommandTarget = this;
		}
		private void ViewToolBar_GetNextCommandChain(object sender, NextCommandChainEventArgs e) {
			e.CommandChain = this;
		}
		private void RefreshView() {
			View.BeginUpdate();
			ListView.ListViewItemCollection items = View.Items;
			ListViewGroupCollection groups = View.Groups;
			items.Clear();
			groups.Clear();
			var accs = MainForm.__GetServer()?.LoadAccounts(MainForm.__GetLoginToken());
			foreach (var acc in accs) {
				items.Add(new LVItem(acc));
			}
			View.EndUpdate();
		}

		[Command(Commands.SelectAll)]
		private void Command_SelectAll(CommandArgs e) {
			if (e.IsUpdate) {
				e.Command.Enabled = View.Items.Count > 0;
			} else {
				View.BeginUpdate();
				foreach (ListViewItem v in View.Items) {
					v.Selected = true;
				}
				View.EndUpdate();
			}
		}
		[Command(Commands.Add)]
		private void Command_Add(CommandArgs e) {
			if (e.IsUpdate) {
				e.Command.Enabled = true;
			} else {
				View.BeginUpdate();
				AccountDescription acc = new AccountDescription();

				using (AddDialog dialog = new AddDialog(acc)) {
					var result = dialog.ShowDialog();
					if (result == DialogResult.OK) {
						acc = dialog.m_account;
						var accs = new AccountDescription.Collection();
						accs.Add(acc);
						MainForm.__GetServer().AppendAccounts(MainForm.__GetLoginToken(), accs);
						View.Items.Add(new LVItem(MainForm.__GetServer().GetAccount(MainForm.__GetLoginToken(),acc.AccountName)));
					}
				}
				View.EndUpdate();
			}
		}
		[Command(Commands.Delete)]
		private void Command_Delete(CommandArgs e) {
			var count = View.SelectedItems.Count;
			if (e.IsUpdate) {
				e.Command.Enabled = count >= 1 ? true : false;
			} else {
				View.BeginUpdate();
				Account.Collection acc = new Account.Collection();
				foreach (var item in View.SelectedItems) {
					acc.Add((item as LVItem).account);
					(item as LVItem).Remove();
				}
				try {
					MainForm.__GetServer()?.DeleteAccounts(MainForm.__GetLoginToken(), acc);
				}
				finally {
					RefreshView();
					View.EndUpdate();
				}
			}
		}
		[Command(Commands.Change)]
		private void Command_Change(CommandArgs e) {
			var count = View.SelectedItems.Count;
			if (e.IsUpdate) {
				e.Command.Enabled = count==1?true:false;
			} else {
				View.BeginUpdate();
				var acc = (View.SelectedItem as LVItem).account;
				using (EditorDialog dialog = new EditorDialog(acc)) {
					var result = dialog.ShowDialog();
					if (result == DialogResult.OK) {
						if (acc.Permission.Equals(dialog.m_account.Permission)) {
							MainForm.__GetServer().ChangeAccountPermission(MainForm.__GetLoginToken(), acc.AccountName, dialog.m_account.Permission);
						}
						if (!string.Equals(acc.AccountName, dialog.m_account.AccountName)) {
							MainForm.__GetServer().ChangeAccountName(MainForm.__GetLoginToken(), acc.AccountName, dialog.m_account.AccountName);
						} 
						if (!string.Equals(acc.Department, dialog.m_account.Department)) {
							MainForm.__GetServer().ChangeAccountDepartment(MainForm.__GetLoginToken(), acc.AccountName, dialog.m_account.Department);
						}
						
					}
				}
				this.RefreshView();
				View.EndUpdate();
			}
		}
		[Command(Commands.BatchImport)]
		private void Command_BatchImport(CommandArgs e) {
			if (e.IsUpdate) {
				e.Command.Enabled = true;
			} else {
				//导入Excel
			}
		}
		private static object[] __GetToolBarCommands() {
			return new ICommandItem[] {
					 CommandItem.GetNamed(PredefCommands.EDIT_UNDO),
					 CommandItem.GetNamed(PredefCommands.EDIT_REDO),
					 CommandSeparator.Default,
					 CommandItem.GetNamed(Commands.Delete),
					 CommandItem.GetNamed(Commands.Add),
					 CommandItem.GetNamed(Commands.BatchImport),
				};
		}
	}

	public class DepartmentSettings : WorkSettingsBase
	{
		public DepartmentSettings() {

		}
		public override string Name {
			get {
				return "部门管理";
			}
		}
		public override WorkingEditerPageBase OnCreaterEditerPage() {
			return new DepartmentEditerPage(this);
		}
	}
	public class DepartmentEditerPage : WorkingEditerPageBase
	{
		#region DepartmentListView
		private class DepartmentListView : CFCListView
		{
			private ImageList m_imageList;
			public DepartmentListView() {
				m_imageList = ImageListManager.LoadDefault().CreateImageList(37);
				this.SmallImageList = null;
			}
			public void CreateColumns() {
				this.Columns.Clear();
				AddColumns("部门名称|Name", 150, " 部门主管|Department Leader", 150);
				this.FullRowSelect = true;
				this.HideSelection = false;
				this.MultiSelect = true;
				this.CheckBoxes = false;
				AllowDrop = true;
			}
			protected override void Dispose(bool disposing) {
				if (disposing) {
					ImageList imageList = m_imageList;
					m_imageList = null;
					this.SmallImageList = null;
					imageList?.Dispose();
				}
				base.Dispose(disposing);
			}
		}
		#endregion
		#region LVItem
		private class LVItem : ListViewItem
		{
			public Department Department;

			public LVItem(Department Department) {
				if (Department == null) {
					throw new ArgumentNullException("Department");
				}
				this.Department = Department;
				this.Checked = false;
				this.Update();
			}

			public void Update() {
				string Name = Department.Name;
				string Leader = Department.Leader;
				this.Text = Name;
				this.SubItems.AddRange(new string[1] { Leader });
			}
		}
		#endregion
		#region Commands
		[ImageList]
		private enum Commands
		{
			[CommandNotation("全选", "Select All")]
			SelectAll,
			[CommandNotation("增加", "Add", ImageIndex = 434)]
			Add,
			[CommandNotation("删除", "Delete", ImageIndex = 1085)]
			Delete,
			[CommandNotation("修改", "Change")]
			Change,
			[CommandNotation("批量导入", "Batch Import", ImageIndex = 995)]
			BatchImport,
		}
		#endregion
		#region CommandToolBar
		private class CommandToolBar : TransparentBackCommandToolBar
		{
			public CommandToolBar() {
			}
			public override int PreferredHeight {
				get {
					return 24;
				}
			}
			public override bool FirstSeparaterVisible {
				get {
					return true;
				}
			}
		}
		#endregion

		#region EditorDialog
		protected class EditorDialog : XSRForm
		{
			private LabelTextBox m_name;
			private LabelTextBox m_leader;
			private bool m_isAdd = false;

			private Button m_ok;
			private Button m_cancle;

			public DepartmentDescription m_department;

			public EditorDialog(DepartmentDescription dep, bool isadd) {
				m_department = dep;
				m_isAdd = isadd;

				InitForm();
			}
			private void InitForm() {
				var lh = new FormLayoutHelper(this, new Margin(10, 8, 10, 10), 400) { LabelSpace = 8 };
				m_name = lh.CreateLabelControl<LabelTextBox>("部门名称: |Name", -8, Anchors.TopHoriz);
				m_leader = lh.CreateLabelControl<LabelTextBox>("部门领导: | Department Leader", -8, Anchors.TopHoriz);
				m_ok = lh.CreateButton(FormButtons.OK);
				m_cancle = lh.CreateButton(FormButtons.Cancel);

				lh.BeginForm();
				m_name.Bounds = lh.LeftAlloc(m_name.Height);
				m_leader.Bounds = lh.LeftAlloc(m_leader.Height);

				m_name.Text = m_department.Name;
				m_leader.Text = m_department.Leader;

				m_cancle.Bounds = lh.RightAlloc(70, 24);
				m_ok.Bounds = lh.RightAlloc(70, 24);
				m_cancle.Click += this.Cancle_Click;
				m_ok.Click += this.Ok_Click;

				if (m_isAdd) {
					lh.EndForm("添加新部门|Add New Employee", this.m_ok, this.m_cancle, FormBorderStyle.FixedDialog);
				} else {
					lh.EndForm("编辑部门信息|Edit Employee Information", this.m_ok, this.m_cancle, FormBorderStyle.FixedDialog);
				}


			}

			private void Ok_Click(object sender, EventArgs e) {
				try {
					var name = this.m_name.Text;
					name = DataTableColumnDefinition.NormmalizeColumnName(name, true);
					if (string.IsNullOrEmpty(name)) {
						throw new Exception(XSR.Select("部门名称不能为空!", "Employee name cannot be empty!"));
					}
					var department = this.m_leader.Text;
					department = DataTableColumnDefinition.NormmalizeColumnName(department, true);
					if (string.IsNullOrEmpty(department)) {
						throw new Exception(XSR.Select("部门领导不能为空!", "Department cannot be empty!"));
					}

					m_department = new DepartmentDescription(name, department, false);

					this.DialogResult = DialogResult.OK;
					this.Close();

				}
				catch (Exception exc) {
					ErrorMessageBox.Show(exc, ShowErrorOption.Message);
				}
			}
			private void Cancle_Click(object sender, EventArgs e) {
				try {
					this.DialogResult = DialogResult.Cancel;
					this.Close();
				}
				catch (Exception exc) {
					ErrorMessageBox.Show(exc, ShowErrorOption.Message);
				}
			}
			private void AddNewEmployee() {

			}

		}
		#endregion



		private DepartmentListView View;
		private CommandToolBar ViewToolBar;

		public DepartmentEditerPage(DepartmentSettings settings) {
			this.Settings = settings ?? throw new ArgumentNullException("DepartmentEditerPage settings");

			InitForm();
		}
		private void InitForm() {
			var ch = new FormLayoutHelper(this, new Margin(2, 0, 2, 0), 600) { LabelSpace = 4 };
			this.ViewToolBar = ch.CreateToolBar<CommandToolBar>(Anchors.TopHoriz);
			this.ViewToolBar.GetCommandTarget += ViewToolBar_GetCommandTarget;
			this.ViewToolBar.GetNextCommandChain += ViewToolBar_GetNextCommandChain;
			this.View = ch.CreateControl<DepartmentListView>(Anchors.All);
			ch.BeginForm();
			this.ViewToolBar.Bounds = ch.LeftAlloc(this.ViewToolBar.PreferredHeight);
			this.View.Bounds = ch.LeftAlloc(500);
			this.View.CreateColumns();
			ch.EndForm("人员管理页面");
			this.ViewToolBar.Initialize(__GetToolBarCommands());

			View.BeginUpdate();
			ListView.ListViewItemCollection items = View.Items;
			ListViewGroupCollection groups = View.Groups;
			items.Clear();
			groups.Clear();
			var deps = MainForm.__GetServer()?.LoadDepartments(MainForm.__GetLoginToken());
			foreach (var dep in deps) {
				items.Add(new LVItem(dep));
			}
			View.EndUpdate();


			View.BuildMenu += View_BuildMenu;
			View.ItemDrag += View_ItemDrag;
			View.DragEnter += View_DragEnter;
			View.DragOver += View_DragOver;
			View.DragDrop += View_DragDrop;
			View.DoubleClick += View_DoubleClick;
		}

		private void View_DoubleClick(object sender, EventArgs e) {
			View.BeginUpdate();
			var dep = (View.SelectedItem as LVItem).Department;

			var department = new DepartmentDescription(dep.Name, dep.Leader, false);
			using (EditorDialog dialog = new EditorDialog(department, false)) {
				var result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					if (!string.Equals(department.Name, dialog.m_department.Name)) {
						MainForm.__GetServer().ChangeDepartmentName(MainForm.__GetLoginToken(), department.Name, dialog.m_department.Name);
					}
					if (!string.Equals(department.Leader, dialog.m_department.Leader)) {

					}
				}
			}
			RefreshView();
			View.EndUpdate();
		}
		private void View_DragDrop(object sender, DragEventArgs e) {
			LVItem draggedItem = e.Data.GetData(typeof(LVItem)) as LVItem;
			if (draggedItem != null) {
				e.Effect = DragDropEffects.None;
				Point dpt = new Point(e.X, e.Y);
				Point lpt = View.PointToClient(dpt);
				int idx = View.InsertionMark.NearestIndex(lpt);
				int c = View.Items.Count;
				int index = draggedItem.Index;
				ListViewItem hitItem = null;
				if (idx >= 0 && idx < c && !(hitItem = View.Items[idx]).Selected) {
					int sc = View.SelectedIndices.Count;
					if (sc > 0) {
						int[] ids = new int[sc];
						View.SelectedIndices.CopyTo(ids, 0);
						Array.Sort(ids);
						View.BeginUpdate();
						try {
							List<ListViewItem> list = new List<ListViewItem>();
							for (int i = sc - 1; i >= 0; i--) {
								ListViewItem item = View.Items[ids[i]];
								list.Insert(0, item);
								item.Remove();
							}
							idx = hitItem.Index;
							for (int i = sc - 1; i >= 0; i--) {
								View.Items.Insert(idx, list[i]);
							}
						}
						finally {
							View.EndUpdate();
						}
					}
				}
			}
			e.Effect = DragDropEffects.None;
			View.InsertionMark.Index = -1;
		}
		private void View_DragOver(object sender, DragEventArgs e) {
			bool mark = false;
			LVItem draggedItem = e.Data.GetData(typeof(LVItem)) as LVItem;
			if (draggedItem != null) {
				e.Effect = DragDropEffects.Move;
				Point dpt = new Point(e.X, e.Y);
				Point lpt = View.PointToClient(dpt);
				ListViewItem item = View.GetItemAt(lpt.X, lpt.Y);
				if (item != null) {
					int idx = item.Index;
					View.EnsureVisible(idx);
					if (item != draggedItem) {
						View.InsertionMark.Index = idx;
						mark = true;
					}
				}
			} else {
			}
			if (!mark) {
				View.InsertionMark.Index = -1;
			}
		}
		private void View_DragEnter(object sender, DragEventArgs e) {
			LVItem draggedItem = e.Data.GetData(typeof(LVItem)) as LVItem;
			if (draggedItem != null) {
				e.Effect = DragDropEffects.Move;
			} else {
				e.Effect = DragDropEffects.None;
			}
		}
		private void View_ItemDrag(object sender, ItemDragEventArgs e) {
			View.DoDragDrop(e.Item, DragDropEffects.Move);
		}
		private void View_BuildMenu(object sender, MenuBuilder e) {
			if (e.CanAddSeparator) {
				e.AddSeparator();
			}
			e.Add(Commands.SelectAll);
			e.Add(Commands.Add);
			e.Add(Commands.Delete);
			e.Add(Commands.Change);
		}
		public object NextChain {
			get {
				return this.View;
			}
		}
		private void ViewToolBar_GetCommandTarget(object sender, GetCommandTargetEventArgs e) {
			e.CommandTarget = this;
		}
		private void ViewToolBar_GetNextCommandChain(object sender, NextCommandChainEventArgs e) {
			e.CommandChain = this;
		}
		private void RefreshView() {
			View.BeginUpdate();
			ListView.ListViewItemCollection items = View.Items;
			ListViewGroupCollection groups = View.Groups;
			items.Clear();
			groups.Clear();
			var deps = MainForm.__GetServer()?.LoadDepartments(MainForm.__GetLoginToken());
			foreach (var dep in deps) {
				items.Add(new LVItem(dep));
			}
			View.EndUpdate();
		}

		[Command(Commands.SelectAll)]
		private void Command_SelectAll(CommandArgs e) {
			if (e.IsUpdate) {
				e.Command.Enabled = View.Items.Count > 0;
			} else {
				View.BeginUpdate();
				foreach (ListViewItem v in View.Items) {
					v.Selected = true;
				}
				View.EndUpdate();
			}
		}
		[Command(Commands.Add)]
		private void Command_Add(CommandArgs e) {
			if (e.IsUpdate) {
				e.Command.Enabled = true;
			} else {
				View.BeginUpdate();
				DepartmentDescription dep = new DepartmentDescription();

				using (EditorDialog dialog = new EditorDialog(dep, true)) {
					var result = dialog.ShowDialog();
					if (result == DialogResult.OK) {
						dep = dialog.m_department;
						DepartmentDescription depDesc = new DepartmentDescription(dep.Name, dep.Leader,false);
						var deps = new DepartmentDescription.Collection();
						deps.Add(depDesc);
						MainForm.__GetServer().AppendDepartments(MainForm.__GetLoginToken(), deps);
						View.Items.Add(new LVItem(MainForm.__GetServer().GetDepartment(MainForm.__GetLoginToken(), dep.Name)));
					}
				}
				View.EndUpdate();
			}
		}
		[Command(Commands.Delete)]
		private void Command_Delete(CommandArgs e) {
			var count = View.SelectedItems.Count;
			if (e.IsUpdate) {
				e.Command.Enabled = count >= 1 ? true : false;
			} else {
				View.BeginUpdate();
				Department.Collection departments = new Department.Collection();
				foreach(var item in View.SelectedItems) {
					departments.Add((item as LVItem).Department);
					(item as LVItem).Remove();
				}
				try {
					MainForm.__GetServer()?.DeleteDepartments(MainForm.__GetLoginToken(), departments);
				}
				finally {
					RefreshView();
					View.EndUpdate();
				}
			}

		}
		[Command(Commands.Change)]
		private void Command_Change(CommandArgs e) {
			var count = View.SelectedItems.Count;
			if (e.IsUpdate) {
				e.Command.Enabled = count == 1 ? true : false;
			} else {
				View.BeginUpdate();
				var dep = (View.SelectedItem as LVItem).Department;

				var department = new DepartmentDescription(dep.Name, dep.Leader, false);
				using (EditorDialog dialog = new EditorDialog(department, false)) {
					var result = dialog.ShowDialog();
					if (result == DialogResult.OK) {
						if (!string.Equals(department.Name, dialog.m_department.Name)) {
							MainForm.__GetServer().ChangeDepartmentName(MainForm.__GetLoginToken(), department.Name, dialog.m_department.Name);
						}
						if (!string.Equals(department.Leader, dialog.m_department.Leader)) {

						}
					}
				}
				RefreshView();
				View.EndUpdate();
			}
		}
		[Command(Commands.BatchImport)]
		private void Command_BatchImport(CommandArgs e) {
			if (e.IsUpdate) {
				e.Command.Enabled = true;
			} else {
				//导入Excel
			}
		}

		private static object[] __GetToolBarCommands() {
			return new ICommandItem[] {
				 CommandItem.GetNamed(PredefCommands.EDIT_UNDO),
				 CommandItem.GetNamed(PredefCommands.EDIT_REDO),
				 CommandSeparator.Default,
				 CommandItem.GetNamed(Commands.Delete),
				 CommandItem.GetNamed(Commands.Add),
				 CommandItem.GetNamed(Commands.BatchImport),
			};
		}
	}


	public class ProcessSettings : WorkSettingsBase
	{
		public ProcessSettings() {

		}
		public override string Name {
			get {
				return "工作台";
			}
		}
		public override WorkingEditerPageBase OnCreaterEditerPage() {
			return new ProcessEditerPage(this);
		}
	}
	public class ProcessEditerPage : WorkingEditerPageBase
	{
		public ProcessEditerPage(ProcessSettings settings) {
			this.Settings = settings ?? throw new ArgumentNullException("ProcessSettings settings");

		}


	}
}
