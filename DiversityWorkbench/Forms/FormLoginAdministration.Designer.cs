namespace DiversityWorkbench.Forms
{
    partial class FormLoginAdministration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoginAdministration));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.treeViewLogins = new System.Windows.Forms.TreeView();
            this.labelLogins = new System.Windows.Forms.Label();
            this.toolStripLoginList = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLoginCreate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoginCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoginDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxFilter = new System.Windows.Forms.ToolStripTextBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelServerLogin = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxLoginHasAccess = new System.Windows.Forms.CheckBox();
            this.splitContainerLogin = new System.Windows.Forms.SplitContainer();
            this.groupBoxDatabases = new System.Windows.Forms.GroupBox();
            this.treeViewDatabases = new System.Windows.Forms.TreeView();
            this.toolStripDatabase = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDatabaseOverview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSynchronizeUserProxy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonListAllDatabases = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoginMissing = new System.Windows.Forms.ToolStripButton();
            this.groupBoxDatabase = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelLogin = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxUser = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelUser = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonLoginOnly = new System.Windows.Forms.RadioButton();
            this.pictureBoxUser = new System.Windows.Forms.PictureBox();
            this.radioButtonUser = new System.Windows.Forms.RadioButton();
            this.userControlModuleRelatedEntryAgent = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.checkBoxIsDBO = new System.Windows.Forms.CheckBox();
            this.checkBoxPrivacyConsent = new System.Windows.Forms.CheckBox();
            this.buttonSetPrivacyConsent = new System.Windows.Forms.Button();
            this.tabControlUserDetails = new System.Windows.Forms.TabControl();
            this.tabPageRoles = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRoles = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxUserRoles = new System.Windows.Forms.ListBox();
            this.labelUserRoles = new System.Windows.Forms.Label();
            this.buttonRoleRemove = new System.Windows.Forms.Button();
            this.listBoxRoles = new System.Windows.Forms.ListBox();
            this.buttonRoleAdd = new System.Windows.Forms.Button();
            this.labelRoles = new System.Windows.Forms.Label();
            this.buttonRoleOverview = new System.Windows.Forms.Button();
            this.tabPageProjects = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelProjects = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectsNotAvailable = new System.Windows.Forms.Label();
            this.labelProjectsAvailable = new System.Windows.Forms.Label();
            this.buttonProjectRemove = new System.Windows.Forms.Button();
            this.buttonProjectAdd = new System.Windows.Forms.Button();
            this.listBoxProjectsNotAvailable = new System.Windows.Forms.ListBox();
            this.labelOrderProject = new System.Windows.Forms.Label();
            this.radioButtonOrderProjectByName = new System.Windows.Forms.RadioButton();
            this.radioButtonOrderProjectByID = new System.Windows.Forms.RadioButton();
            this.buttonSynchronizeProjects = new System.Windows.Forms.Button();
            this.userControlModuleRelatedEntryProject = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.splitContainerProjectAccessible = new System.Windows.Forms.SplitContainer();
            this.listBoxProjectsAvailable = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelProjectsReadOnly = new System.Windows.Forms.TableLayoutPanel();
            this.buttonProjectsReadOnlyAdd = new System.Windows.Forms.Button();
            this.buttonProjectsReadOnlyRemove = new System.Windows.Forms.Button();
            this.listBoxProjectsReadOnly = new System.Windows.Forms.ListBox();
            this.labelProjectsReadOnly = new System.Windows.Forms.Label();
            this.listBoxProjectsLocked = new System.Windows.Forms.ListBox();
            this.buttonProjectAddAll = new System.Windows.Forms.Button();
            this.buttonProjectRemoveAll = new System.Windows.Forms.Button();
            this.buttonRemoveProject = new System.Windows.Forms.Button();
            this.buttonProjectUserNotAvailable = new System.Windows.Forms.Button();
            this.buttonProjectUserAvailable = new System.Windows.Forms.Button();
            this.buttonProjectUserNotAvailableIsLocked = new System.Windows.Forms.Button();
            this.buttonProjectUserAvailableIsLocked = new System.Windows.Forms.Button();
            this.pictureBoxProjectsLocked = new System.Windows.Forms.PictureBox();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            this.labelContentSettings = new System.Windows.Forms.Label();
            this.textBoxContentSettings = new System.Windows.Forms.TextBox();
            this.treeViewSettings = new System.Windows.Forms.TreeView();
            this.labelSettingsHeader = new System.Windows.Forms.Label();
            this.buttonSearchSettingsTemplate = new System.Windows.Forms.Button();
            this.buttonRemoveSettingsNode = new System.Windows.Forms.Button();
            this.buttonAddSettingsNode = new System.Windows.Forms.Button();
            this.imageListUserDetails = new System.Windows.Forms.ImageList(this.components);
            this.pictureBoxLogin = new System.Windows.Forms.PictureBox();
            this.labelDbUserInfo = new System.Windows.Forms.Label();
            this.buttonChangePW = new System.Windows.Forms.Button();
            this.comboBoxDBUserInfo = new System.Windows.Forms.ComboBox();
            this.pictureBoxSecurityAdmin = new System.Windows.Forms.PictureBox();
            this.checkBoxSecurityAdmin = new System.Windows.Forms.CheckBox();
            this.labelDefaultDB = new System.Windows.Forms.Label();
            this.textBoxDefaultDB = new System.Windows.Forms.TextBox();
            this.labelLoginInfo = new System.Windows.Forms.Label();
            this.buttonLoginStatistics = new System.Windows.Forms.Button();
            this.buttonLoginOverview = new System.Windows.Forms.Button();
            this.buttonShowCurrentActivity = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonLinkedServer = new System.Windows.Forms.Button();
            this.buttonSetPrivacyConsentInfoSite = new System.Windows.Forms.Button();
            this.imageListLogin = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.toolStripLoginList.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxLogin.SuspendLayout();
            this.tableLayoutPanelServerLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogin)).BeginInit();
            this.splitContainerLogin.Panel1.SuspendLayout();
            this.splitContainerLogin.Panel2.SuspendLayout();
            this.splitContainerLogin.SuspendLayout();
            this.groupBoxDatabases.SuspendLayout();
            this.toolStripDatabase.SuspendLayout();
            this.groupBoxDatabase.SuspendLayout();
            this.tableLayoutPanelLogin.SuspendLayout();
            this.groupBoxUser.SuspendLayout();
            this.tableLayoutPanelUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).BeginInit();
            this.tabControlUserDetails.SuspendLayout();
            this.tabPageRoles.SuspendLayout();
            this.tableLayoutPanelRoles.SuspendLayout();
            this.tabPageProjects.SuspendLayout();
            this.tableLayoutPanelProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProjectAccessible)).BeginInit();
            this.splitContainerProjectAccessible.Panel1.SuspendLayout();
            this.splitContainerProjectAccessible.Panel2.SuspendLayout();
            this.splitContainerProjectAccessible.SuspendLayout();
            this.tableLayoutPanelProjectsReadOnly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProjectsLocked)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            this.tableLayoutPanelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSecurityAdmin)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpNavigator(this.splitContainerMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this.splitContainerMain, "Login administration");
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.treeViewLogins);
            this.splitContainerMain.Panel1.Controls.Add(this.labelLogins);
            this.splitContainerMain.Panel1.Controls.Add(this.toolStripLoginList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetShowHelp(this.splitContainerMain, true);
            this.splitContainerMain.Size = new System.Drawing.Size(829, 546);
            this.splitContainerMain.SplitterDistance = 165;
            this.splitContainerMain.TabIndex = 0;
            // 
            // treeViewLogins
            // 
            this.treeViewLogins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLogins.Location = new System.Drawing.Point(0, 20);
            this.treeViewLogins.Name = "treeViewLogins";
            this.treeViewLogins.ShowLines = false;
            this.treeViewLogins.ShowPlusMinus = false;
            this.treeViewLogins.ShowRootLines = false;
            this.treeViewLogins.Size = new System.Drawing.Size(165, 501);
            this.treeViewLogins.TabIndex = 1;
            this.treeViewLogins.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLogins_AfterSelect);
            this.treeViewLogins.Click += new System.EventHandler(this.treeViewLogins_Click);
            // 
            // labelLogins
            // 
            this.labelLogins.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLogins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLogins.Location = new System.Drawing.Point(0, 0);
            this.labelLogins.Margin = new System.Windows.Forms.Padding(3);
            this.labelLogins.Name = "labelLogins";
            this.labelLogins.Size = new System.Drawing.Size(165, 20);
            this.labelLogins.TabIndex = 2;
            this.labelLogins.Text = "Logins";
            this.labelLogins.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLoginList
            // 
            this.toolStripLoginList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripLoginList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripLoginList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLoginCreate,
            this.toolStripButtonLoginCopy,
            this.toolStripButtonLoginDelete,
            this.toolStripButtonFilter,
            this.toolStripTextBoxFilter});
            this.toolStripLoginList.Location = new System.Drawing.Point(0, 521);
            this.toolStripLoginList.Name = "toolStripLoginList";
            this.toolStripLoginList.Size = new System.Drawing.Size(165, 25);
            this.toolStripLoginList.TabIndex = 0;
            this.toolStripLoginList.Text = "toolStrip1";
            // 
            // toolStripButtonLoginCreate
            // 
            this.toolStripButtonLoginCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoginCreate.Image = global::DiversityWorkbench.Properties.Resources.Login;
            this.toolStripButtonLoginCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoginCreate.Name = "toolStripButtonLoginCreate";
            this.toolStripButtonLoginCreate.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoginCreate.Text = "New login";
            this.toolStripButtonLoginCreate.Click += new System.EventHandler(this.toolStripButtonLoginCreate_Click);
            // 
            // toolStripButtonLoginCopy
            // 
            this.toolStripButtonLoginCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoginCopy.Image = global::DiversityWorkbench.Properties.Resources.CopyAgent;
            this.toolStripButtonLoginCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoginCopy.Name = "toolStripButtonLoginCopy";
            this.toolStripButtonLoginCopy.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoginCopy.Text = "Copy the selected login";
            this.toolStripButtonLoginCopy.Click += new System.EventHandler(this.toolStripButtonLoginCopy_Click);
            // 
            // toolStripButtonLoginDelete
            // 
            this.toolStripButtonLoginDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoginDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonLoginDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoginDelete.Name = "toolStripButtonLoginDelete";
            this.toolStripButtonLoginDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoginDelete.Text = "Delete login and remove all settings in the databases";
            this.toolStripButtonLoginDelete.Click += new System.EventHandler(this.toolStripButtonLoginDelete_Click);
            // 
            // toolStripButtonFilter
            // 
            this.toolStripButtonFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.toolStripButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFilter.Name = "toolStripButtonFilter";
            this.toolStripButtonFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFilter.Text = "Filter for logins";
            this.toolStripButtonFilter.Click += new System.EventHandler(this.toolStripButtonFilter_Click);
            // 
            // toolStripTextBoxFilter
            // 
            this.toolStripTextBoxFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxFilter.Name = "toolStripTextBoxFilter";
            this.toolStripTextBoxFilter.Size = new System.Drawing.Size(50, 25);
            this.toolStripTextBoxFilter.ToolTipText = "Text for filtering logins. Use % as wildcard";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 5;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxLogin, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonShowCurrentActivity, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonLinkedServer, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSetPrivacyConsentInfoSite, 1, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(660, 546);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // groupBoxLogin
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxLogin, 5);
            this.groupBoxLogin.Controls.Add(this.tableLayoutPanelServerLogin);
            this.groupBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxLogin.Location = new System.Drawing.Point(3, 29);
            this.groupBoxLogin.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(654, 514);
            this.groupBoxLogin.TabIndex = 0;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "Login";
            // 
            // tableLayoutPanelServerLogin
            // 
            this.tableLayoutPanelServerLogin.ColumnCount = 7;
            this.tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelServerLogin.Controls.Add(this.checkBoxLoginHasAccess, 1, 0);
            this.tableLayoutPanelServerLogin.Controls.Add(this.splitContainerLogin, 0, 3);
            this.tableLayoutPanelServerLogin.Controls.Add(this.pictureBoxLogin, 0, 0);
            this.tableLayoutPanelServerLogin.Controls.Add(this.labelDbUserInfo, 5, 0);
            this.tableLayoutPanelServerLogin.Controls.Add(this.buttonChangePW, 0, 2);
            this.tableLayoutPanelServerLogin.Controls.Add(this.comboBoxDBUserInfo, 5, 1);
            this.tableLayoutPanelServerLogin.Controls.Add(this.pictureBoxSecurityAdmin, 2, 0);
            this.tableLayoutPanelServerLogin.Controls.Add(this.checkBoxSecurityAdmin, 3, 0);
            this.tableLayoutPanelServerLogin.Controls.Add(this.labelDefaultDB, 4, 0);
            this.tableLayoutPanelServerLogin.Controls.Add(this.textBoxDefaultDB, 4, 1);
            this.tableLayoutPanelServerLogin.Controls.Add(this.labelLoginInfo, 2, 2);
            this.tableLayoutPanelServerLogin.Controls.Add(this.buttonLoginStatistics, 6, 1);
            this.tableLayoutPanelServerLogin.Controls.Add(this.buttonLoginOverview, 6, 2);
            this.tableLayoutPanelServerLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelServerLogin.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelServerLogin.Name = "tableLayoutPanelServerLogin";
            this.tableLayoutPanelServerLogin.RowCount = 4;
            this.tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelServerLogin.Size = new System.Drawing.Size(648, 495);
            this.tableLayoutPanelServerLogin.TabIndex = 0;
            // 
            // checkBoxLoginHasAccess
            // 
            this.checkBoxLoginHasAccess.AutoSize = true;
            this.checkBoxLoginHasAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxLoginHasAccess.Location = new System.Drawing.Point(25, 3);
            this.checkBoxLoginHasAccess.Name = "checkBoxLoginHasAccess";
            this.tableLayoutPanelServerLogin.SetRowSpan(this.checkBoxLoginHasAccess, 2);
            this.checkBoxLoginHasAccess.Size = new System.Drawing.Size(86, 36);
            this.checkBoxLoginHasAccess.TabIndex = 0;
            this.checkBoxLoginHasAccess.Text = "Enabled";
            this.checkBoxLoginHasAccess.UseVisualStyleBackColor = true;
            this.checkBoxLoginHasAccess.CheckedChanged += new System.EventHandler(this.checkBoxLoginHasAccess_CheckedChanged);
            this.checkBoxLoginHasAccess.Click += new System.EventHandler(this.checkBoxLoginHasAccess_Click);
            // 
            // splitContainerLogin
            // 
            this.tableLayoutPanelServerLogin.SetColumnSpan(this.splitContainerLogin, 7);
            this.splitContainerLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainerLogin.Location = new System.Drawing.Point(3, 74);
            this.splitContainerLogin.Name = "splitContainerLogin";
            // 
            // splitContainerLogin.Panel1
            // 
            this.splitContainerLogin.Panel1.Controls.Add(this.groupBoxDatabases);
            // 
            // splitContainerLogin.Panel2
            // 
            this.splitContainerLogin.Panel2.Controls.Add(this.groupBoxDatabase);
            this.splitContainerLogin.Panel2.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.splitContainerLogin.Size = new System.Drawing.Size(642, 418);
            this.splitContainerLogin.SplitterDistance = 213;
            this.splitContainerLogin.TabIndex = 5;
            // 
            // groupBoxDatabases
            // 
            this.groupBoxDatabases.Controls.Add(this.treeViewDatabases);
            this.groupBoxDatabases.Controls.Add(this.toolStripDatabase);
            this.groupBoxDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDatabases.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDatabases.Name = "groupBoxDatabases";
            this.groupBoxDatabases.Size = new System.Drawing.Size(213, 418);
            this.groupBoxDatabases.TabIndex = 1;
            this.groupBoxDatabases.TabStop = false;
            this.groupBoxDatabases.Text = "Databases";
            // 
            // treeViewDatabases
            // 
            this.treeViewDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDatabases.Location = new System.Drawing.Point(3, 16);
            this.treeViewDatabases.Name = "treeViewDatabases";
            this.treeViewDatabases.Size = new System.Drawing.Size(207, 374);
            this.treeViewDatabases.TabIndex = 0;
            this.treeViewDatabases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDatabases_AfterSelect);
            // 
            // toolStripDatabase
            // 
            this.toolStripDatabase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripDatabase.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDatabaseOverview,
            this.toolStripButtonSynchronizeUserProxy,
            this.toolStripButtonListAllDatabases,
            this.toolStripButtonLoginMissing});
            this.toolStripDatabase.Location = new System.Drawing.Point(3, 390);
            this.toolStripDatabase.Name = "toolStripDatabase";
            this.toolStripDatabase.Size = new System.Drawing.Size(207, 25);
            this.toolStripDatabase.TabIndex = 1;
            this.toolStripDatabase.Text = "toolStrip1";
            // 
            // toolStripButtonDatabaseOverview
            // 
            this.toolStripButtonDatabaseOverview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDatabaseOverview.Image = global::DiversityWorkbench.Properties.Resources.DatabaseLogin;
            this.toolStripButtonDatabaseOverview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDatabaseOverview.Name = "toolStripButtonDatabaseOverview";
            this.toolStripButtonDatabaseOverview.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDatabaseOverview.Text = "Show overview for database";
            this.toolStripButtonDatabaseOverview.Click += new System.EventHandler(this.toolStripButtonDatabaseOverview_Click);
            // 
            // toolStripButtonSynchronizeUserProxy
            // 
            this.toolStripButtonSynchronizeUserProxy.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSynchronizeUserProxy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSynchronizeUserProxy.Image = global::DiversityWorkbench.Properties.Resources.SynchronizeAgent;
            this.toolStripButtonSynchronizeUserProxy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSynchronizeUserProxy.Name = "toolStripButtonSynchronizeUserProxy";
            this.toolStripButtonSynchronizeUserProxy.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSynchronizeUserProxy.Text = "Synchronize logins in table UserProxy with server  logins ";
            this.toolStripButtonSynchronizeUserProxy.Click += new System.EventHandler(this.toolStripButtonSynchronizeUserProxy_Click);
            // 
            // toolStripButtonListAllDatabases
            // 
            this.toolStripButtonListAllDatabases.Image = global::DiversityWorkbench.ResourceWorkbench.DatabaseList;
            this.toolStripButtonListAllDatabases.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonListAllDatabases.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonListAllDatabases.Name = "toolStripButtonListAllDatabases";
            this.toolStripButtonListAllDatabases.Size = new System.Drawing.Size(115, 22);
            this.toolStripButtonListAllDatabases.Text = "List all databases";
            this.toolStripButtonListAllDatabases.Click += new System.EventHandler(this.toolStripButtonListAllDatabases_Click);
            // 
            // toolStripButtonLoginMissing
            // 
            this.toolStripButtonLoginMissing.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonLoginMissing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLoginMissing.Image = global::DiversityWorkbench.Properties.Resources.LoginMissing;
            this.toolStripButtonLoginMissing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoginMissing.Name = "toolStripButtonLoginMissing";
            this.toolStripButtonLoginMissing.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLoginMissing.Text = "Search for database users without a valid login on the server";
            this.toolStripButtonLoginMissing.Click += new System.EventHandler(this.toolStripButtonLoginMissing_Click);
            // 
            // groupBoxDatabase
            // 
            this.groupBoxDatabase.Controls.Add(this.tableLayoutPanelLogin);
            this.groupBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDatabase.Location = new System.Drawing.Point(0, 20);
            this.groupBoxDatabase.Name = "groupBoxDatabase";
            this.groupBoxDatabase.Size = new System.Drawing.Size(425, 398);
            this.groupBoxDatabase.TabIndex = 5;
            this.groupBoxDatabase.TabStop = false;
            this.groupBoxDatabase.Text = "Database";
            // 
            // tableLayoutPanelLogin
            // 
            this.tableLayoutPanelLogin.ColumnCount = 1;
            this.tableLayoutPanelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogin.Controls.Add(this.groupBoxUser, 0, 0);
            this.tableLayoutPanelLogin.Controls.Add(this.tabControlUserDetails, 0, 1);
            this.tableLayoutPanelLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelLogin.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanelLogin.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelLogin.Name = "tableLayoutPanelLogin";
            this.tableLayoutPanelLogin.RowCount = 2;
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLogin.Size = new System.Drawing.Size(419, 379);
            this.tableLayoutPanelLogin.TabIndex = 4;
            // 
            // groupBoxUser
            // 
            this.groupBoxUser.Controls.Add(this.tableLayoutPanelUser);
            this.groupBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxUser.ForeColor = System.Drawing.Color.Black;
            this.groupBoxUser.Location = new System.Drawing.Point(3, 3);
            this.groupBoxUser.Name = "groupBoxUser";
            this.groupBoxUser.Size = new System.Drawing.Size(413, 72);
            this.groupBoxUser.TabIndex = 3;
            this.groupBoxUser.TabStop = false;
            this.groupBoxUser.Text = "User";
            // 
            // tableLayoutPanelUser
            // 
            this.tableLayoutPanelUser.ColumnCount = 6;
            this.tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUser.Controls.Add(this.radioButtonLoginOnly, 1, 0);
            this.tableLayoutPanelUser.Controls.Add(this.pictureBoxUser, 0, 0);
            this.tableLayoutPanelUser.Controls.Add(this.radioButtonUser, 2, 0);
            this.tableLayoutPanelUser.Controls.Add(this.userControlModuleRelatedEntryAgent, 0, 1);
            this.tableLayoutPanelUser.Controls.Add(this.checkBoxIsDBO, 5, 0);
            this.tableLayoutPanelUser.Controls.Add(this.checkBoxPrivacyConsent, 3, 0);
            this.tableLayoutPanelUser.Controls.Add(this.buttonSetPrivacyConsent, 4, 0);
            this.tableLayoutPanelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUser.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelUser.Name = "tableLayoutPanelUser";
            this.tableLayoutPanelUser.RowCount = 2;
            this.tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUser.Size = new System.Drawing.Size(407, 53);
            this.tableLayoutPanelUser.TabIndex = 3;
            // 
            // radioButtonLoginOnly
            // 
            this.radioButtonLoginOnly.AutoSize = true;
            this.radioButtonLoginOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonLoginOnly.ForeColor = System.Drawing.Color.Gray;
            this.radioButtonLoginOnly.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonLoginOnly.Location = new System.Drawing.Point(25, 3);
            this.radioButtonLoginOnly.Name = "radioButtonLoginOnly";
            this.radioButtonLoginOnly.Size = new System.Drawing.Size(100, 20);
            this.radioButtonLoginOnly.TabIndex = 2;
            this.radioButtonLoginOnly.TabStop = true;
            this.radioButtonLoginOnly.Text = "Not in database";
            this.radioButtonLoginOnly.UseVisualStyleBackColor = true;
            this.radioButtonLoginOnly.Click += new System.EventHandler(this.radioButtonLoginOnly_Click);
            // 
            // pictureBoxUser
            // 
            this.pictureBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxUser.Image = global::DiversityWorkbench.Properties.Resources.Agent;
            this.pictureBoxUser.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxUser.Name = "pictureBoxUser";
            this.pictureBoxUser.Size = new System.Drawing.Size(16, 20);
            this.pictureBoxUser.TabIndex = 3;
            this.pictureBoxUser.TabStop = false;
            // 
            // radioButtonUser
            // 
            this.radioButtonUser.AutoSize = true;
            this.radioButtonUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonUser.ForeColor = System.Drawing.Color.Black;
            this.radioButtonUser.Location = new System.Drawing.Point(131, 3);
            this.radioButtonUser.Name = "radioButtonUser";
            this.radioButtonUser.Size = new System.Drawing.Size(81, 20);
            this.radioButtonUser.TabIndex = 4;
            this.radioButtonUser.TabStop = true;
            this.radioButtonUser.Text = "In database";
            this.radioButtonUser.UseVisualStyleBackColor = true;
            this.radioButtonUser.Click += new System.EventHandler(this.radioButtonUser_Click);
            // 
            // userControlModuleRelatedEntryAgent
            // 
            this.userControlModuleRelatedEntryAgent.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelUser.SetColumnSpan(this.userControlModuleRelatedEntryAgent, 6);
            this.userControlModuleRelatedEntryAgent.DependsOnUri = "";
            this.userControlModuleRelatedEntryAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryAgent.Domain = "";
            this.userControlModuleRelatedEntryAgent.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryAgent.Location = new System.Drawing.Point(3, 28);
            this.userControlModuleRelatedEntryAgent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlModuleRelatedEntryAgent.Module = null;
            this.userControlModuleRelatedEntryAgent.Name = "userControlModuleRelatedEntryAgent";
            this.userControlModuleRelatedEntryAgent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryAgent.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryAgent.ShowInfo = false;
            this.userControlModuleRelatedEntryAgent.Size = new System.Drawing.Size(401, 23);
            this.userControlModuleRelatedEntryAgent.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryAgent.TabIndex = 6;
            // 
            // checkBoxIsDBO
            // 
            this.checkBoxIsDBO.AutoSize = true;
            this.checkBoxIsDBO.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxIsDBO.Location = new System.Drawing.Point(349, 3);
            this.checkBoxIsDBO.Name = "checkBoxIsDBO";
            this.checkBoxIsDBO.Size = new System.Drawing.Size(55, 20);
            this.checkBoxIsDBO.TabIndex = 7;
            this.checkBoxIsDBO.Text = "Is dbo";
            this.toolTip.SetToolTip(this.checkBoxIsDBO, "If the login is a database owner of the database");
            this.checkBoxIsDBO.UseVisualStyleBackColor = true;
            this.checkBoxIsDBO.Click += new System.EventHandler(this.checkBoxIsDBO_Click);
            // 
            // checkBoxPrivacyConsent
            // 
            this.checkBoxPrivacyConsent.AutoCheck = false;
            this.checkBoxPrivacyConsent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPrivacyConsent.Enabled = false;
            this.checkBoxPrivacyConsent.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.checkBoxPrivacyConsent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxPrivacyConsent.Location = new System.Drawing.Point(218, 0);
            this.checkBoxPrivacyConsent.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxPrivacyConsent.Name = "checkBoxPrivacyConsent";
            this.checkBoxPrivacyConsent.Size = new System.Drawing.Size(100, 26);
            this.checkBoxPrivacyConsent.TabIndex = 5;
            this.checkBoxPrivacyConsent.UseVisualStyleBackColor = true;
            this.checkBoxPrivacyConsent.Visible = false;
            // 
            // buttonSetPrivacyConsent
            // 
            this.buttonSetPrivacyConsent.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonSetPrivacyConsent.FlatAppearance.BorderSize = 0;
            this.buttonSetPrivacyConsent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetPrivacyConsent.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.buttonSetPrivacyConsent.Location = new System.Drawing.Point(321, 0);
            this.buttonSetPrivacyConsent.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetPrivacyConsent.Name = "buttonSetPrivacyConsent";
            this.buttonSetPrivacyConsent.Size = new System.Drawing.Size(12, 23);
            this.buttonSetPrivacyConsent.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonSetPrivacyConsent, "Set the privacy consent to YES");
            this.buttonSetPrivacyConsent.UseVisualStyleBackColor = false;
            this.buttonSetPrivacyConsent.Visible = false;
            this.buttonSetPrivacyConsent.Click += new System.EventHandler(this.buttonSetPrivacyConsent_Click);
            // 
            // tabControlUserDetails
            // 
            this.tabControlUserDetails.Controls.Add(this.tabPageRoles);
            this.tabControlUserDetails.Controls.Add(this.tabPageProjects);
            this.tabControlUserDetails.Controls.Add(this.tabPageSettings);
            this.tabControlUserDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlUserDetails.ImageList = this.imageListUserDetails;
            this.tabControlUserDetails.Location = new System.Drawing.Point(3, 81);
            this.tabControlUserDetails.Name = "tabControlUserDetails";
            this.tabControlUserDetails.SelectedIndex = 0;
            this.tabControlUserDetails.Size = new System.Drawing.Size(413, 295);
            this.tabControlUserDetails.TabIndex = 5;
            // 
            // tabPageRoles
            // 
            this.tabPageRoles.Controls.Add(this.tableLayoutPanelRoles);
            this.tabPageRoles.ImageIndex = 0;
            this.tabPageRoles.Location = new System.Drawing.Point(4, 23);
            this.tabPageRoles.Name = "tabPageRoles";
            this.tabPageRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRoles.Size = new System.Drawing.Size(405, 268);
            this.tabPageRoles.TabIndex = 0;
            this.tabPageRoles.Text = "Roles";
            this.tabPageRoles.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRoles
            // 
            this.tableLayoutPanelRoles.ColumnCount = 3;
            this.tableLayoutPanelRoles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRoles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRoles.Controls.Add(this.listBoxUserRoles, 2, 1);
            this.tableLayoutPanelRoles.Controls.Add(this.labelUserRoles, 2, 0);
            this.tableLayoutPanelRoles.Controls.Add(this.buttonRoleRemove, 1, 3);
            this.tableLayoutPanelRoles.Controls.Add(this.listBoxRoles, 0, 1);
            this.tableLayoutPanelRoles.Controls.Add(this.buttonRoleAdd, 1, 2);
            this.tableLayoutPanelRoles.Controls.Add(this.labelRoles, 0, 0);
            this.tableLayoutPanelRoles.Controls.Add(this.buttonRoleOverview, 1, 1);
            this.tableLayoutPanelRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRoles.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelRoles.Name = "tableLayoutPanelRoles";
            this.tableLayoutPanelRoles.RowCount = 4;
            this.tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelRoles.Size = new System.Drawing.Size(399, 262);
            this.tableLayoutPanelRoles.TabIndex = 0;
            // 
            // listBoxUserRoles
            // 
            this.listBoxUserRoles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.listBoxUserRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUserRoles.FormattingEnabled = true;
            this.listBoxUserRoles.IntegralHeight = false;
            this.listBoxUserRoles.Location = new System.Drawing.Point(215, 16);
            this.listBoxUserRoles.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.listBoxUserRoles.Name = "listBoxUserRoles";
            this.tableLayoutPanelRoles.SetRowSpan(this.listBoxUserRoles, 3);
            this.listBoxUserRoles.Size = new System.Drawing.Size(181, 243);
            this.listBoxUserRoles.TabIndex = 4;
            // 
            // labelUserRoles
            // 
            this.labelUserRoles.AutoSize = true;
            this.labelUserRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUserRoles.ForeColor = System.Drawing.Color.Green;
            this.labelUserRoles.Location = new System.Drawing.Point(218, 0);
            this.labelUserRoles.Name = "labelUserRoles";
            this.labelUserRoles.Size = new System.Drawing.Size(178, 13);
            this.labelUserRoles.TabIndex = 18;
            this.labelUserRoles.Text = "Granted";
            // 
            // buttonRoleRemove
            // 
            this.buttonRoleRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRoleRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoleRemove.Location = new System.Drawing.Point(187, 133);
            this.buttonRoleRemove.Name = "buttonRoleRemove";
            this.buttonRoleRemove.Size = new System.Drawing.Size(25, 21);
            this.buttonRoleRemove.TabIndex = 25;
            this.buttonRoleRemove.Tag = "";
            this.buttonRoleRemove.Text = "<";
            this.buttonRoleRemove.UseVisualStyleBackColor = true;
            this.buttonRoleRemove.Click += new System.EventHandler(this.buttonRoleRemove_Click);
            // 
            // listBoxRoles
            // 
            this.listBoxRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRoles.FormattingEnabled = true;
            this.listBoxRoles.IntegralHeight = false;
            this.listBoxRoles.Location = new System.Drawing.Point(3, 16);
            this.listBoxRoles.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.listBoxRoles.Name = "listBoxRoles";
            this.tableLayoutPanelRoles.SetRowSpan(this.listBoxRoles, 3);
            this.listBoxRoles.Size = new System.Drawing.Size(181, 243);
            this.listBoxRoles.TabIndex = 12;
            // 
            // buttonRoleAdd
            // 
            this.buttonRoleAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRoleAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoleAdd.Location = new System.Drawing.Point(187, 106);
            this.buttonRoleAdd.Name = "buttonRoleAdd";
            this.buttonRoleAdd.Size = new System.Drawing.Size(25, 21);
            this.buttonRoleAdd.TabIndex = 24;
            this.buttonRoleAdd.Text = ">";
            this.buttonRoleAdd.UseVisualStyleBackColor = true;
            this.buttonRoleAdd.Click += new System.EventHandler(this.buttonRoleAdd_Click);
            // 
            // labelRoles
            // 
            this.labelRoles.AutoSize = true;
            this.labelRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRoles.ForeColor = System.Drawing.Color.Crimson;
            this.labelRoles.Location = new System.Drawing.Point(3, 0);
            this.labelRoles.Name = "labelRoles";
            this.labelRoles.Size = new System.Drawing.Size(178, 13);
            this.labelRoles.TabIndex = 17;
            this.labelRoles.Text = "Available";
            // 
            // buttonRoleOverview
            // 
            this.buttonRoleOverview.Image = global::DiversityWorkbench.Properties.Resources.RoleOverview;
            this.buttonRoleOverview.Location = new System.Drawing.Point(184, 16);
            this.buttonRoleOverview.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.buttonRoleOverview.Name = "buttonRoleOverview";
            this.buttonRoleOverview.Size = new System.Drawing.Size(25, 24);
            this.buttonRoleOverview.TabIndex = 26;
            this.toolTip.SetToolTip(this.buttonRoleOverview, "Show permissions of the selected role");
            this.buttonRoleOverview.UseVisualStyleBackColor = true;
            this.buttonRoleOverview.Click += new System.EventHandler(this.buttonRoleOverview_Click);
            // 
            // tabPageProjects
            // 
            this.tabPageProjects.Controls.Add(this.tableLayoutPanelProjects);
            this.tabPageProjects.ImageIndex = 1;
            this.tabPageProjects.Location = new System.Drawing.Point(4, 23);
            this.tabPageProjects.Name = "tabPageProjects";
            this.tabPageProjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProjects.Size = new System.Drawing.Size(405, 268);
            this.tabPageProjects.TabIndex = 1;
            this.tabPageProjects.Text = "Projects";
            this.tabPageProjects.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjects
            // 
            this.tableLayoutPanelProjects.ColumnCount = 4;
            this.tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjects.Controls.Add(this.labelProjectsNotAvailable, 0, 0);
            this.tableLayoutPanelProjects.Controls.Add(this.labelProjectsAvailable, 3, 0);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectRemove, 1, 5);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectAdd, 1, 4);
            this.tableLayoutPanelProjects.Controls.Add(this.listBoxProjectsNotAvailable, 0, 1);
            this.tableLayoutPanelProjects.Controls.Add(this.labelOrderProject, 0, 8);
            this.tableLayoutPanelProjects.Controls.Add(this.radioButtonOrderProjectByName, 2, 8);
            this.tableLayoutPanelProjects.Controls.Add(this.radioButtonOrderProjectByID, 1, 8);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonSynchronizeProjects, 1, 10);
            this.tableLayoutPanelProjects.Controls.Add(this.userControlModuleRelatedEntryProject, 0, 9);
            this.tableLayoutPanelProjects.Controls.Add(this.splitContainerProjectAccessible, 3, 1);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectAddAll, 1, 3);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectRemoveAll, 1, 6);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonRemoveProject, 0, 10);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectUserNotAvailable, 1, 0);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectUserAvailable, 2, 0);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectUserNotAvailableIsLocked, 1, 2);
            this.tableLayoutPanelProjects.Controls.Add(this.buttonProjectUserAvailableIsLocked, 2, 2);
            this.tableLayoutPanelProjects.Controls.Add(this.pictureBoxProjectsLocked, 2, 7);
            this.tableLayoutPanelProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjects.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelProjects.Name = "tableLayoutPanelProjects";
            this.tableLayoutPanelProjects.RowCount = 11;
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelProjects.Size = new System.Drawing.Size(399, 262);
            this.tableLayoutPanelProjects.TabIndex = 0;
            // 
            // labelProjectsNotAvailable
            // 
            this.labelProjectsNotAvailable.AutoSize = true;
            this.labelProjectsNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsNotAvailable.ForeColor = System.Drawing.Color.Red;
            this.labelProjectsNotAvailable.Image = global::DiversityWorkbench.Properties.Resources.NoAccess16;
            this.labelProjectsNotAvailable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelProjectsNotAvailable.Location = new System.Drawing.Point(3, 0);
            this.labelProjectsNotAvailable.Name = "labelProjectsNotAvailable";
            this.labelProjectsNotAvailable.Size = new System.Drawing.Size(175, 23);
            this.labelProjectsNotAvailable.TabIndex = 16;
            this.labelProjectsNotAvailable.Text = "      No access";
            this.labelProjectsNotAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProjectsAvailable
            // 
            this.labelProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsAvailable.ForeColor = System.Drawing.Color.Green;
            this.labelProjectsAvailable.Image = global::DiversityWorkbench.Properties.Resources.ProjectOpen;
            this.labelProjectsAvailable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelProjectsAvailable.Location = new System.Drawing.Point(220, 3);
            this.labelProjectsAvailable.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelProjectsAvailable.Name = "labelProjectsAvailable";
            this.labelProjectsAvailable.Size = new System.Drawing.Size(176, 20);
            this.labelProjectsAvailable.TabIndex = 15;
            this.labelProjectsAvailable.Text = "      Accessible";
            this.labelProjectsAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonProjectRemove
            // 
            this.tableLayoutPanelProjects.SetColumnSpan(this.buttonProjectRemove, 2);
            this.buttonProjectRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonProjectRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectRemove.ForeColor = System.Drawing.Color.Red;
            this.buttonProjectRemove.Location = new System.Drawing.Point(184, 102);
            this.buttonProjectRemove.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonProjectRemove.Name = "buttonProjectRemove";
            this.buttonProjectRemove.Size = new System.Drawing.Size(30, 20);
            this.buttonProjectRemove.TabIndex = 23;
            this.buttonProjectRemove.Text = "<";
            this.toolTip.SetToolTip(this.buttonProjectRemove, "Remove the selected project from the Accessible list");
            this.buttonProjectRemove.UseVisualStyleBackColor = true;
            this.buttonProjectRemove.Click += new System.EventHandler(this.buttonProjectRemove_Click);
            // 
            // buttonProjectAdd
            // 
            this.tableLayoutPanelProjects.SetColumnSpan(this.buttonProjectAdd, 2);
            this.buttonProjectAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonProjectAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectAdd.ForeColor = System.Drawing.Color.Green;
            this.buttonProjectAdd.Location = new System.Drawing.Point(184, 82);
            this.buttonProjectAdd.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonProjectAdd.Name = "buttonProjectAdd";
            this.buttonProjectAdd.Size = new System.Drawing.Size(30, 20);
            this.buttonProjectAdd.TabIndex = 22;
            this.buttonProjectAdd.Text = ">";
            this.toolTip.SetToolTip(this.buttonProjectAdd, "Add the selected project to the Accessible list");
            this.buttonProjectAdd.UseVisualStyleBackColor = true;
            this.buttonProjectAdd.Click += new System.EventHandler(this.buttonProjectAdd_Click);
            // 
            // listBoxProjectsNotAvailable
            // 
            this.listBoxProjectsNotAvailable.BackColor = System.Drawing.Color.Pink;
            this.listBoxProjectsNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsNotAvailable.FormattingEnabled = true;
            this.listBoxProjectsNotAvailable.IntegralHeight = false;
            this.listBoxProjectsNotAvailable.Location = new System.Drawing.Point(3, 23);
            this.listBoxProjectsNotAvailable.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.listBoxProjectsNotAvailable.Name = "listBoxProjectsNotAvailable";
            this.tableLayoutPanelProjects.SetRowSpan(this.listBoxProjectsNotAvailable, 7);
            this.listBoxProjectsNotAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxProjectsNotAvailable.Size = new System.Drawing.Size(178, 167);
            this.listBoxProjectsNotAvailable.TabIndex = 6;
            this.listBoxProjectsNotAvailable.Click += new System.EventHandler(this.listBoxProjectsNotAvailable_Click);
            this.listBoxProjectsNotAvailable.SelectedIndexChanged += new System.EventHandler(this.listBoxProjectsNotAvailable_SelectedIndexChanged);
            // 
            // labelOrderProject
            // 
            this.labelOrderProject.AutoSize = true;
            this.labelOrderProject.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOrderProject.Location = new System.Drawing.Point(117, 190);
            this.labelOrderProject.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelOrderProject.Name = "labelOrderProject";
            this.labelOrderProject.Size = new System.Drawing.Size(64, 20);
            this.labelOrderProject.TabIndex = 39;
            this.labelOrderProject.Text = "Order by: ID";
            this.labelOrderProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButtonOrderProjectByName
            // 
            this.radioButtonOrderProjectByName.AutoSize = true;
            this.radioButtonOrderProjectByName.Checked = true;
            this.tableLayoutPanelProjects.SetColumnSpan(this.radioButtonOrderProjectByName, 2);
            this.radioButtonOrderProjectByName.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonOrderProjectByName.Location = new System.Drawing.Point(202, 190);
            this.radioButtonOrderProjectByName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.radioButtonOrderProjectByName.Name = "radioButtonOrderProjectByName";
            this.radioButtonOrderProjectByName.Size = new System.Drawing.Size(53, 20);
            this.radioButtonOrderProjectByName.TabIndex = 40;
            this.radioButtonOrderProjectByName.TabStop = true;
            this.radioButtonOrderProjectByName.Text = "Name";
            this.radioButtonOrderProjectByName.UseVisualStyleBackColor = true;
            // 
            // radioButtonOrderProjectByID
            // 
            this.radioButtonOrderProjectByID.AutoSize = true;
            this.radioButtonOrderProjectByID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonOrderProjectByID.Location = new System.Drawing.Point(184, 193);
            this.radioButtonOrderProjectByID.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.radioButtonOrderProjectByID.Name = "radioButtonOrderProjectByID";
            this.radioButtonOrderProjectByID.Size = new System.Drawing.Size(15, 14);
            this.radioButtonOrderProjectByID.TabIndex = 41;
            this.radioButtonOrderProjectByID.Text = "ID";
            this.radioButtonOrderProjectByID.UseVisualStyleBackColor = true;
            this.radioButtonOrderProjectByID.CheckedChanged += new System.EventHandler(this.radioButtonOrderProjectByID_CheckedChanged);
            // 
            // buttonSynchronizeProjects
            // 
            this.tableLayoutPanelProjects.SetColumnSpan(this.buttonSynchronizeProjects, 3);
            this.buttonSynchronizeProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSynchronizeProjects.Image = global::DiversityWorkbench.ResourceWorkbench.Download;
            this.buttonSynchronizeProjects.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSynchronizeProjects.Location = new System.Drawing.Point(181, 238);
            this.buttonSynchronizeProjects.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSynchronizeProjects.Name = "buttonSynchronizeProjects";
            this.buttonSynchronizeProjects.Size = new System.Drawing.Size(218, 24);
            this.buttonSynchronizeProjects.TabIndex = 42;
            this.buttonSynchronizeProjects.Text = "Load projects from DiversityProjects";
            this.buttonSynchronizeProjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonSynchronizeProjects, "Load projects defined in the module DiversityProjects");
            this.buttonSynchronizeProjects.UseVisualStyleBackColor = true;
            this.buttonSynchronizeProjects.Click += new System.EventHandler(this.buttonSynchronizeProjects_Click);
            // 
            // userControlModuleRelatedEntryProject
            // 
            this.userControlModuleRelatedEntryProject.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelProjects.SetColumnSpan(this.userControlModuleRelatedEntryProject, 4);
            this.userControlModuleRelatedEntryProject.DependsOnUri = "";
            this.userControlModuleRelatedEntryProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryProject.Domain = "";
            this.userControlModuleRelatedEntryProject.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryProject.Location = new System.Drawing.Point(3, 213);
            this.userControlModuleRelatedEntryProject.Module = null;
            this.userControlModuleRelatedEntryProject.Name = "userControlModuleRelatedEntryProject";
            this.userControlModuleRelatedEntryProject.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryProject.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryProject.ShowInfo = false;
            this.userControlModuleRelatedEntryProject.Size = new System.Drawing.Size(393, 22);
            this.userControlModuleRelatedEntryProject.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryProject.TabIndex = 43;
            // 
            // splitContainerProjectAccessible
            // 
            this.splitContainerProjectAccessible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerProjectAccessible.Location = new System.Drawing.Point(217, 23);
            this.splitContainerProjectAccessible.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.splitContainerProjectAccessible.Name = "splitContainerProjectAccessible";
            this.splitContainerProjectAccessible.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerProjectAccessible.Panel1
            // 
            this.splitContainerProjectAccessible.Panel1.Controls.Add(this.listBoxProjectsAvailable);
            // 
            // splitContainerProjectAccessible.Panel2
            // 
            this.splitContainerProjectAccessible.Panel2.Controls.Add(this.tableLayoutPanelProjectsReadOnly);
            this.tableLayoutPanelProjects.SetRowSpan(this.splitContainerProjectAccessible, 7);
            this.splitContainerProjectAccessible.Size = new System.Drawing.Size(179, 167);
            this.splitContainerProjectAccessible.SplitterDistance = 99;
            this.splitContainerProjectAccessible.SplitterWidth = 2;
            this.splitContainerProjectAccessible.TabIndex = 44;
            // 
            // listBoxProjectsAvailable
            // 
            this.listBoxProjectsAvailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.listBoxProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsAvailable.FormattingEnabled = true;
            this.listBoxProjectsAvailable.IntegralHeight = false;
            this.listBoxProjectsAvailable.Location = new System.Drawing.Point(0, 0);
            this.listBoxProjectsAvailable.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.listBoxProjectsAvailable.Name = "listBoxProjectsAvailable";
            this.listBoxProjectsAvailable.Size = new System.Drawing.Size(179, 99);
            this.listBoxProjectsAvailable.TabIndex = 5;
            this.listBoxProjectsAvailable.Click += new System.EventHandler(this.listBoxProjectsAvailable_Click);
            this.listBoxProjectsAvailable.SelectedIndexChanged += new System.EventHandler(this.listBoxProjectsAvailable_SelectedIndexChanged);
            // 
            // tableLayoutPanelProjectsReadOnly
            // 
            this.tableLayoutPanelProjectsReadOnly.ColumnCount = 3;
            this.tableLayoutPanelProjectsReadOnly.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProjectsReadOnly.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectsReadOnly.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjectsReadOnly.Controls.Add(this.buttonProjectsReadOnlyAdd, 1, 0);
            this.tableLayoutPanelProjectsReadOnly.Controls.Add(this.buttonProjectsReadOnlyRemove, 2, 0);
            this.tableLayoutPanelProjectsReadOnly.Controls.Add(this.listBoxProjectsReadOnly, 0, 1);
            this.tableLayoutPanelProjectsReadOnly.Controls.Add(this.labelProjectsReadOnly, 0, 0);
            this.tableLayoutPanelProjectsReadOnly.Controls.Add(this.listBoxProjectsLocked, 0, 2);
            this.tableLayoutPanelProjectsReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjectsReadOnly.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelProjectsReadOnly.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tableLayoutPanelProjectsReadOnly.Name = "tableLayoutPanelProjectsReadOnly";
            this.tableLayoutPanelProjectsReadOnly.RowCount = 3;
            this.tableLayoutPanelProjectsReadOnly.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjectsReadOnly.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProjectsReadOnly.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjectsReadOnly.Size = new System.Drawing.Size(179, 66);
            this.tableLayoutPanelProjectsReadOnly.TabIndex = 0;
            // 
            // buttonProjectsReadOnlyAdd
            // 
            this.buttonProjectsReadOnlyAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonProjectsReadOnlyAdd.Image = global::DiversityWorkbench.Properties.Resources.ArrowDownSmall;
            this.buttonProjectsReadOnlyAdd.Location = new System.Drawing.Point(98, 0);
            this.buttonProjectsReadOnlyAdd.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonProjectsReadOnlyAdd.Name = "buttonProjectsReadOnlyAdd";
            this.buttonProjectsReadOnlyAdd.Size = new System.Drawing.Size(30, 20);
            this.buttonProjectsReadOnlyAdd.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonProjectsReadOnlyAdd, "Change selected project to read only");
            this.buttonProjectsReadOnlyAdd.UseVisualStyleBackColor = true;
            this.buttonProjectsReadOnlyAdd.Click += new System.EventHandler(this.buttonProjectsReadOnlyAdd_Click);
            // 
            // buttonProjectsReadOnlyRemove
            // 
            this.buttonProjectsReadOnlyRemove.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonProjectsReadOnlyRemove.Image = global::DiversityWorkbench.Properties.Resources.ArrowUpSmall;
            this.buttonProjectsReadOnlyRemove.Location = new System.Drawing.Point(134, 0);
            this.buttonProjectsReadOnlyRemove.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonProjectsReadOnlyRemove.Name = "buttonProjectsReadOnlyRemove";
            this.buttonProjectsReadOnlyRemove.Size = new System.Drawing.Size(30, 20);
            this.buttonProjectsReadOnlyRemove.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonProjectsReadOnlyRemove, "Remove selected project from read only list");
            this.buttonProjectsReadOnlyRemove.UseVisualStyleBackColor = true;
            this.buttonProjectsReadOnlyRemove.Click += new System.EventHandler(this.buttonProjectsReadOnlyRemove_Click);
            // 
            // listBoxProjectsReadOnly
            // 
            this.listBoxProjectsReadOnly.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanelProjectsReadOnly.SetColumnSpan(this.listBoxProjectsReadOnly, 3);
            this.listBoxProjectsReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsReadOnly.FormattingEnabled = true;
            this.listBoxProjectsReadOnly.IntegralHeight = false;
            this.listBoxProjectsReadOnly.Location = new System.Drawing.Point(0, 20);
            this.listBoxProjectsReadOnly.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxProjectsReadOnly.Name = "listBoxProjectsReadOnly";
            this.listBoxProjectsReadOnly.Size = new System.Drawing.Size(179, 26);
            this.listBoxProjectsReadOnly.TabIndex = 2;
            this.listBoxProjectsReadOnly.Click += new System.EventHandler(this.listBoxProjectsReadOnly_Click);
            // 
            // labelProjectsReadOnly
            // 
            this.labelProjectsReadOnly.ForeColor = System.Drawing.Color.DimGray;
            this.labelProjectsReadOnly.Image = global::DiversityWorkbench.Properties.Resources.ProjectGrey;
            this.labelProjectsReadOnly.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelProjectsReadOnly.Location = new System.Drawing.Point(0, 0);
            this.labelProjectsReadOnly.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelProjectsReadOnly.Name = "labelProjectsReadOnly";
            this.labelProjectsReadOnly.Size = new System.Drawing.Size(80, 20);
            this.labelProjectsReadOnly.TabIndex = 3;
            this.labelProjectsReadOnly.Text = "      Read Only";
            this.labelProjectsReadOnly.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBoxProjectsLocked
            // 
            this.listBoxProjectsLocked.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutPanelProjectsReadOnly.SetColumnSpan(this.listBoxProjectsLocked, 3);
            this.listBoxProjectsLocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsLocked.ForeColor = System.Drawing.Color.Red;
            this.listBoxProjectsLocked.FormattingEnabled = true;
            this.listBoxProjectsLocked.IntegralHeight = false;
            this.listBoxProjectsLocked.Location = new System.Drawing.Point(0, 46);
            this.listBoxProjectsLocked.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxProjectsLocked.Name = "listBoxProjectsLocked";
            this.listBoxProjectsLocked.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBoxProjectsLocked.Size = new System.Drawing.Size(179, 20);
            this.listBoxProjectsLocked.TabIndex = 4;
            this.toolTip.SetToolTip(this.listBoxProjectsLocked, "Locked projects");
            this.listBoxProjectsLocked.Visible = false;
            // 
            // buttonProjectAddAll
            // 
            this.tableLayoutPanelProjects.SetColumnSpan(this.buttonProjectAddAll, 2);
            this.buttonProjectAddAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonProjectAddAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectAddAll.ForeColor = System.Drawing.Color.Green;
            this.buttonProjectAddAll.Location = new System.Drawing.Point(184, 62);
            this.buttonProjectAddAll.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonProjectAddAll.Name = "buttonProjectAddAll";
            this.buttonProjectAddAll.Size = new System.Drawing.Size(30, 20);
            this.buttonProjectAddAll.TabIndex = 45;
            this.buttonProjectAddAll.Text = ">>";
            this.toolTip.SetToolTip(this.buttonProjectAddAll, "Add all projects to the Accessible list");
            this.buttonProjectAddAll.UseVisualStyleBackColor = true;
            this.buttonProjectAddAll.Click += new System.EventHandler(this.buttonProjectAddAll_Click);
            // 
            // buttonProjectRemoveAll
            // 
            this.tableLayoutPanelProjects.SetColumnSpan(this.buttonProjectRemoveAll, 2);
            this.buttonProjectRemoveAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectRemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectRemoveAll.ForeColor = System.Drawing.Color.Red;
            this.buttonProjectRemoveAll.Location = new System.Drawing.Point(184, 122);
            this.buttonProjectRemoveAll.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonProjectRemoveAll.Name = "buttonProjectRemoveAll";
            this.buttonProjectRemoveAll.Size = new System.Drawing.Size(30, 20);
            this.buttonProjectRemoveAll.TabIndex = 46;
            this.buttonProjectRemoveAll.Text = "<<";
            this.toolTip.SetToolTip(this.buttonProjectRemoveAll, "Remove all projects from the Accessible list");
            this.buttonProjectRemoveAll.UseVisualStyleBackColor = true;
            this.buttonProjectRemoveAll.Click += new System.EventHandler(this.buttonProjectRemoveAll_Click);
            // 
            // buttonRemoveProject
            // 
            this.buttonRemoveProject.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRemoveProject.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveProject.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveProject.Location = new System.Drawing.Point(0, 238);
            this.buttonRemoveProject.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemoveProject.Name = "buttonRemoveProject";
            this.buttonRemoveProject.Size = new System.Drawing.Size(107, 24);
            this.buttonRemoveProject.TabIndex = 47;
            this.buttonRemoveProject.Text = "Remove project";
            this.buttonRemoveProject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonRemoveProject, "Remove the project selected in the No access list from the database");
            this.buttonRemoveProject.UseVisualStyleBackColor = true;
            this.buttonRemoveProject.Click += new System.EventHandler(this.buttonRemoveProject_Click);
            // 
            // buttonProjectUserNotAvailable
            // 
            this.buttonProjectUserNotAvailable.BackColor = System.Drawing.Color.Pink;
            this.buttonProjectUserNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectUserNotAvailable.FlatAppearance.BorderSize = 0;
            this.buttonProjectUserNotAvailable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectUserNotAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectUserNotAvailable.ForeColor = System.Drawing.Color.Red;
            this.buttonProjectUserNotAvailable.Image = global::DiversityWorkbench.Properties.Resources.Agent;
            this.buttonProjectUserNotAvailable.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonProjectUserNotAvailable.Location = new System.Drawing.Point(181, 0);
            this.buttonProjectUserNotAvailable.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectUserNotAvailable.Name = "buttonProjectUserNotAvailable";
            this.tableLayoutPanelProjects.SetRowSpan(this.buttonProjectUserNotAvailable, 2);
            this.buttonProjectUserNotAvailable.Size = new System.Drawing.Size(18, 39);
            this.buttonProjectUserNotAvailable.TabIndex = 48;
            this.buttonProjectUserNotAvailable.Text = "?";
            this.buttonProjectUserNotAvailable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip.SetToolTip(this.buttonProjectUserNotAvailable, "Show Users for selected project");
            this.buttonProjectUserNotAvailable.UseVisualStyleBackColor = false;
            this.buttonProjectUserNotAvailable.Click += new System.EventHandler(this.buttonProjectUserNotAvailable_Click);
            // 
            // buttonProjectUserAvailable
            // 
            this.buttonProjectUserAvailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonProjectUserAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProjectUserAvailable.FlatAppearance.BorderSize = 0;
            this.buttonProjectUserAvailable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectUserAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectUserAvailable.ForeColor = System.Drawing.Color.Green;
            this.buttonProjectUserAvailable.Image = global::DiversityWorkbench.Properties.Resources.Agent;
            this.buttonProjectUserAvailable.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonProjectUserAvailable.Location = new System.Drawing.Point(199, 0);
            this.buttonProjectUserAvailable.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectUserAvailable.Name = "buttonProjectUserAvailable";
            this.tableLayoutPanelProjects.SetRowSpan(this.buttonProjectUserAvailable, 2);
            this.buttonProjectUserAvailable.Size = new System.Drawing.Size(18, 39);
            this.buttonProjectUserAvailable.TabIndex = 49;
            this.buttonProjectUserAvailable.Text = "?";
            this.buttonProjectUserAvailable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip.SetToolTip(this.buttonProjectUserAvailable, "Show users for selected project");
            this.buttonProjectUserAvailable.UseVisualStyleBackColor = false;
            this.buttonProjectUserAvailable.Click += new System.EventHandler(this.buttonProjectUserAvailable_Click);
            // 
            // buttonProjectUserNotAvailableIsLocked
            // 
            this.buttonProjectUserNotAvailableIsLocked.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonProjectUserNotAvailableIsLocked.FlatAppearance.BorderSize = 0;
            this.buttonProjectUserNotAvailableIsLocked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectUserNotAvailableIsLocked.Image = global::DiversityWorkbench.Properties.Resources.ProjectOpen;
            this.buttonProjectUserNotAvailableIsLocked.Location = new System.Drawing.Point(181, 39);
            this.buttonProjectUserNotAvailableIsLocked.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectUserNotAvailableIsLocked.Name = "buttonProjectUserNotAvailableIsLocked";
            this.buttonProjectUserNotAvailableIsLocked.Size = new System.Drawing.Size(18, 20);
            this.buttonProjectUserNotAvailableIsLocked.TabIndex = 50;
            this.buttonProjectUserNotAvailableIsLocked.UseVisualStyleBackColor = true;
            this.buttonProjectUserNotAvailableIsLocked.Visible = false;
            this.buttonProjectUserNotAvailableIsLocked.Click += new System.EventHandler(this.buttonProjectUserNotAvailableIsLocked_Click);
            // 
            // buttonProjectUserAvailableIsLocked
            // 
            this.buttonProjectUserAvailableIsLocked.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonProjectUserAvailableIsLocked.FlatAppearance.BorderSize = 0;
            this.buttonProjectUserAvailableIsLocked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProjectUserAvailableIsLocked.Image = global::DiversityWorkbench.Properties.Resources.ProjectGrey;
            this.buttonProjectUserAvailableIsLocked.Location = new System.Drawing.Point(199, 39);
            this.buttonProjectUserAvailableIsLocked.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProjectUserAvailableIsLocked.Name = "buttonProjectUserAvailableIsLocked";
            this.buttonProjectUserAvailableIsLocked.Size = new System.Drawing.Size(18, 20);
            this.buttonProjectUserAvailableIsLocked.TabIndex = 51;
            this.buttonProjectUserAvailableIsLocked.UseVisualStyleBackColor = true;
            this.buttonProjectUserAvailableIsLocked.Visible = false;
            this.buttonProjectUserAvailableIsLocked.Click += new System.EventHandler(this.buttonProjectUserAvailableIsLocked_Click);
            // 
            // pictureBoxProjectsLocked
            // 
            this.pictureBoxProjectsLocked.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBoxProjectsLocked.Image = global::DiversityWorkbench.Properties.Resources.Project;
            this.pictureBoxProjectsLocked.Location = new System.Drawing.Point(199, 172);
            this.pictureBoxProjectsLocked.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxProjectsLocked.Name = "pictureBoxProjectsLocked";
            this.pictureBoxProjectsLocked.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.pictureBoxProjectsLocked.Size = new System.Drawing.Size(18, 18);
            this.pictureBoxProjectsLocked.TabIndex = 52;
            this.pictureBoxProjectsLocked.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxProjectsLocked, "Locked projects");
            this.pictureBoxProjectsLocked.Visible = false;
            this.pictureBoxProjectsLocked.Click += new System.EventHandler(this.pictureBoxProjectsLocked_Click);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.tableLayoutPanelSettings);
            this.tabPageSettings.ImageIndex = 2;
            this.tabPageSettings.Location = new System.Drawing.Point(4, 23);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Size = new System.Drawing.Size(405, 268);
            this.tabPageSettings.TabIndex = 2;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSettings
            // 
            this.tableLayoutPanelSettings.ColumnCount = 5;
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSettings.Controls.Add(this.labelContentSettings, 0, 2);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxContentSettings, 0, 3);
            this.tableLayoutPanelSettings.Controls.Add(this.treeViewSettings, 0, 1);
            this.tableLayoutPanelSettings.Controls.Add(this.labelSettingsHeader, 0, 0);
            this.tableLayoutPanelSettings.Controls.Add(this.buttonSearchSettingsTemplate, 4, 0);
            this.tableLayoutPanelSettings.Controls.Add(this.buttonRemoveSettingsNode, 3, 0);
            this.tableLayoutPanelSettings.Controls.Add(this.buttonAddSettingsNode, 2, 0);
            this.tableLayoutPanelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            this.tableLayoutPanelSettings.RowCount = 4;
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.Size = new System.Drawing.Size(405, 268);
            this.tableLayoutPanelSettings.TabIndex = 1;
            // 
            // labelContentSettings
            // 
            this.labelContentSettings.AutoSize = true;
            this.tableLayoutPanelSettings.SetColumnSpan(this.labelContentSettings, 5);
            this.labelContentSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelContentSettings.Location = new System.Drawing.Point(3, 219);
            this.labelContentSettings.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelContentSettings.Name = "labelContentSettings";
            this.labelContentSettings.Size = new System.Drawing.Size(399, 13);
            this.labelContentSettings.TabIndex = 7;
            this.labelContentSettings.Text = "Content:";
            this.labelContentSettings.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxContentSettings
            // 
            this.tableLayoutPanelSettings.SetColumnSpan(this.textBoxContentSettings, 5);
            this.textBoxContentSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContentSettings.Location = new System.Drawing.Point(3, 235);
            this.textBoxContentSettings.Multiline = true;
            this.textBoxContentSettings.Name = "textBoxContentSettings";
            this.textBoxContentSettings.Size = new System.Drawing.Size(399, 30);
            this.textBoxContentSettings.TabIndex = 6;
            this.textBoxContentSettings.Text = "Content";
            this.textBoxContentSettings.Leave += new System.EventHandler(this.textBoxSettingsContent_Leave);
            // 
            // treeViewSettings
            // 
            this.tableLayoutPanelSettings.SetColumnSpan(this.treeViewSettings, 5);
            this.treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSettings.Location = new System.Drawing.Point(3, 34);
            this.treeViewSettings.Name = "treeViewSettings";
            this.treeViewSettings.Size = new System.Drawing.Size(399, 176);
            this.treeViewSettings.TabIndex = 0;
            this.treeViewSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewSettings_AfterSelect);
            // 
            // labelSettingsHeader
            // 
            this.labelSettingsHeader.AutoSize = true;
            this.tableLayoutPanelSettings.SetColumnSpan(this.labelSettingsHeader, 2);
            this.labelSettingsHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSettingsHeader.Location = new System.Drawing.Point(3, 0);
            this.labelSettingsHeader.Name = "labelSettingsHeader";
            this.labelSettingsHeader.Size = new System.Drawing.Size(237, 31);
            this.labelSettingsHeader.TabIndex = 3;
            this.labelSettingsHeader.Text = "Edit the settings for the user";
            this.labelSettingsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSearchSettingsTemplate
            // 
            this.buttonSearchSettingsTemplate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSearchSettingsTemplate.Location = new System.Drawing.Point(308, 3);
            this.buttonSearchSettingsTemplate.Name = "buttonSearchSettingsTemplate";
            this.buttonSearchSettingsTemplate.Size = new System.Drawing.Size(94, 25);
            this.buttonSearchSettingsTemplate.TabIndex = 4;
            this.buttonSearchSettingsTemplate.Text = "Search template";
            this.buttonSearchSettingsTemplate.UseVisualStyleBackColor = true;
            this.buttonSearchSettingsTemplate.Click += new System.EventHandler(this.buttonSearchSettingsTemplate_Click);
            // 
            // buttonRemoveSettingsNode
            // 
            this.buttonRemoveSettingsNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemoveSettingsNode.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveSettingsNode.Location = new System.Drawing.Point(277, 3);
            this.buttonRemoveSettingsNode.Name = "buttonRemoveSettingsNode";
            this.buttonRemoveSettingsNode.Size = new System.Drawing.Size(25, 25);
            this.buttonRemoveSettingsNode.TabIndex = 2;
            this.buttonRemoveSettingsNode.UseVisualStyleBackColor = true;
            this.buttonRemoveSettingsNode.Click += new System.EventHandler(this.buttonRemoveSettingsNode_Click);
            // 
            // buttonAddSettingsNode
            // 
            this.buttonAddSettingsNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddSettingsNode.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAddSettingsNode.Location = new System.Drawing.Point(246, 3);
            this.buttonAddSettingsNode.Name = "buttonAddSettingsNode";
            this.buttonAddSettingsNode.Size = new System.Drawing.Size(25, 25);
            this.buttonAddSettingsNode.TabIndex = 1;
            this.buttonAddSettingsNode.UseVisualStyleBackColor = true;
            this.buttonAddSettingsNode.Click += new System.EventHandler(this.buttonAddSettingsNode_Click);
            // 
            // imageListUserDetails
            // 
            this.imageListUserDetails.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListUserDetails.ImageStream")));
            this.imageListUserDetails.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListUserDetails.Images.SetKeyName(0, "Group.ico");
            this.imageListUserDetails.Images.SetKeyName(1, "Project.ico");
            this.imageListUserDetails.Images.SetKeyName(2, "Settings.ico");
            // 
            // pictureBoxLogin
            // 
            this.pictureBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLogin.Location = new System.Drawing.Point(3, 9);
            this.pictureBoxLogin.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.pictureBoxLogin.Name = "pictureBoxLogin";
            this.tableLayoutPanelServerLogin.SetRowSpan(this.pictureBoxLogin, 2);
            this.pictureBoxLogin.Size = new System.Drawing.Size(16, 30);
            this.pictureBoxLogin.TabIndex = 2;
            this.pictureBoxLogin.TabStop = false;
            // 
            // labelDbUserInfo
            // 
            this.labelDbUserInfo.AutoSize = true;
            this.labelDbUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDbUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDbUserInfo.ForeColor = System.Drawing.Color.Black;
            this.labelDbUserInfo.Location = new System.Drawing.Point(444, 0);
            this.labelDbUserInfo.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDbUserInfo.Name = "labelDbUserInfo";
            this.labelDbUserInfo.Size = new System.Drawing.Size(180, 14);
            this.labelDbUserInfo.TabIndex = 6;
            this.labelDbUserInfo.Text = "Database for user informations:";
            this.labelDbUserInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonChangePW
            // 
            this.tableLayoutPanelServerLogin.SetColumnSpan(this.buttonChangePW, 2);
            this.buttonChangePW.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonChangePW.Enabled = false;
            this.buttonChangePW.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonChangePW.Image = global::DiversityWorkbench.Properties.Resources.KeyBig;
            this.buttonChangePW.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonChangePW.Location = new System.Drawing.Point(0, 42);
            this.buttonChangePW.Margin = new System.Windows.Forms.Padding(0);
            this.buttonChangePW.Name = "buttonChangePW";
            this.buttonChangePW.Size = new System.Drawing.Size(114, 29);
            this.buttonChangePW.TabIndex = 7;
            this.buttonChangePW.Text = "Change password";
            this.buttonChangePW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonChangePW, "Change the password for the selected login");
            this.buttonChangePW.UseVisualStyleBackColor = true;
            this.buttonChangePW.Click += new System.EventHandler(this.buttonChangePW_Click);
            // 
            // comboBoxDBUserInfo
            // 
            this.comboBoxDBUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDBUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDBUserInfo.FormattingEnabled = true;
            this.comboBoxDBUserInfo.Location = new System.Drawing.Point(441, 14);
            this.comboBoxDBUserInfo.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.comboBoxDBUserInfo.Name = "comboBoxDBUserInfo";
            this.comboBoxDBUserInfo.Size = new System.Drawing.Size(180, 21);
            this.comboBoxDBUserInfo.TabIndex = 7;
            // 
            // pictureBoxSecurityAdmin
            // 
            this.pictureBoxSecurityAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSecurityAdmin.Image = global::DiversityWorkbench.Properties.Resources.Security;
            this.pictureBoxSecurityAdmin.Location = new System.Drawing.Point(117, 9);
            this.pictureBoxSecurityAdmin.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.pictureBoxSecurityAdmin.Name = "pictureBoxSecurityAdmin";
            this.tableLayoutPanelServerLogin.SetRowSpan(this.pictureBoxSecurityAdmin, 2);
            this.pictureBoxSecurityAdmin.Size = new System.Drawing.Size(16, 30);
            this.pictureBoxSecurityAdmin.TabIndex = 8;
            this.pictureBoxSecurityAdmin.TabStop = false;
            // 
            // checkBoxSecurityAdmin
            // 
            this.checkBoxSecurityAdmin.AutoSize = true;
            this.checkBoxSecurityAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxSecurityAdmin.Location = new System.Drawing.Point(136, 3);
            this.checkBoxSecurityAdmin.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.checkBoxSecurityAdmin.Name = "checkBoxSecurityAdmin";
            this.tableLayoutPanelServerLogin.SetRowSpan(this.checkBoxSecurityAdmin, 2);
            this.checkBoxSecurityAdmin.Size = new System.Drawing.Size(142, 36);
            this.checkBoxSecurityAdmin.TabIndex = 9;
            this.checkBoxSecurityAdmin.Text = "System administrator";
            this.toolTip.SetToolTip(this.checkBoxSecurityAdmin, "Allow login to administrate the server");
            this.checkBoxSecurityAdmin.UseVisualStyleBackColor = true;
            this.checkBoxSecurityAdmin.Click += new System.EventHandler(this.checkBoxSecurityAdmin_Click);
            // 
            // labelDefaultDB
            // 
            this.labelDefaultDB.AutoSize = true;
            this.labelDefaultDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefaultDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultDB.Location = new System.Drawing.Point(281, 0);
            this.labelDefaultDB.Name = "labelDefaultDB";
            this.labelDefaultDB.Size = new System.Drawing.Size(157, 14);
            this.labelDefaultDB.TabIndex = 10;
            this.labelDefaultDB.Text = "Default database";
            this.labelDefaultDB.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxDefaultDB
            // 
            this.textBoxDefaultDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDefaultDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDefaultDB.Location = new System.Drawing.Point(281, 14);
            this.textBoxDefaultDB.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxDefaultDB.Name = "textBoxDefaultDB";
            this.textBoxDefaultDB.ReadOnly = true;
            this.textBoxDefaultDB.Size = new System.Drawing.Size(157, 20);
            this.textBoxDefaultDB.TabIndex = 11;
            // 
            // labelLoginInfo
            // 
            this.labelLoginInfo.AutoSize = true;
            this.tableLayoutPanelServerLogin.SetColumnSpan(this.labelLoginInfo, 4);
            this.labelLoginInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoginInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoginInfo.Location = new System.Drawing.Point(117, 42);
            this.labelLoginInfo.Name = "labelLoginInfo";
            this.labelLoginInfo.Size = new System.Drawing.Size(504, 29);
            this.labelLoginInfo.TabIndex = 12;
            this.labelLoginInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonLoginStatistics
            // 
            this.buttonLoginStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonLoginStatistics.Image = global::DiversityWorkbench.Properties.Resources.Graph;
            this.buttonLoginStatistics.Location = new System.Drawing.Point(624, 14);
            this.buttonLoginStatistics.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoginStatistics.Name = "buttonLoginStatistics";
            this.buttonLoginStatistics.Size = new System.Drawing.Size(24, 23);
            this.buttonLoginStatistics.TabIndex = 13;
            this.toolTip.SetToolTip(this.buttonLoginStatistics, "Show the activity of the login");
            this.buttonLoginStatistics.UseVisualStyleBackColor = true;
            this.buttonLoginStatistics.Click += new System.EventHandler(this.buttonLoginStatistics_Click);
            // 
            // buttonLoginOverview
            // 
            this.buttonLoginOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoginOverview.Image = global::DiversityWorkbench.Properties.Resources.AgentOverview;
            this.buttonLoginOverview.Location = new System.Drawing.Point(624, 44);
            this.buttonLoginOverview.Margin = new System.Windows.Forms.Padding(0, 2, 0, 3);
            this.buttonLoginOverview.Name = "buttonLoginOverview";
            this.buttonLoginOverview.Size = new System.Drawing.Size(24, 24);
            this.buttonLoginOverview.TabIndex = 14;
            this.toolTip.SetToolTip(this.buttonLoginOverview, "Show a summary for the login");
            this.buttonLoginOverview.UseVisualStyleBackColor = true;
            this.buttonLoginOverview.Click += new System.EventHandler(this.buttonLoginOverview_Click);
            // 
            // buttonShowCurrentActivity
            // 
            this.buttonShowCurrentActivity.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonShowCurrentActivity.Image = global::DiversityWorkbench.Properties.Resources.ServerIO;
            this.buttonShowCurrentActivity.Location = new System.Drawing.Point(577, 0);
            this.buttonShowCurrentActivity.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonShowCurrentActivity.Name = "buttonShowCurrentActivity";
            this.buttonShowCurrentActivity.Size = new System.Drawing.Size(27, 29);
            this.buttonShowCurrentActivity.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonShowCurrentActivity, "Show the current activity on the server");
            this.buttonShowCurrentActivity.UseVisualStyleBackColor = true;
            this.buttonShowCurrentActivity.Click += new System.EventHandler(this.buttonShowCurrentActivity_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(631, 0);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(26, 29);
            this.buttonFeedback.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback to the software developer");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonLinkedServer
            // 
            this.buttonLinkedServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLinkedServer.Image = global::DiversityWorkbench.Properties.Resources.ServerLinked;
            this.buttonLinkedServer.Location = new System.Drawing.Point(604, 0);
            this.buttonLinkedServer.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonLinkedServer.Name = "buttonLinkedServer";
            this.buttonLinkedServer.Size = new System.Drawing.Size(24, 29);
            this.buttonLinkedServer.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonLinkedServer, "Administrate linked servers");
            this.buttonLinkedServer.UseVisualStyleBackColor = true;
            this.buttonLinkedServer.Click += new System.EventHandler(this.buttonLinkedServer_Click);
            // 
            // buttonSetPrivacyConsentInfoSite
            // 
            this.buttonSetPrivacyConsentInfoSite.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSetPrivacyConsentInfoSite.Image = global::DiversityWorkbench.Properties.Resources.Paragraf;
            this.buttonSetPrivacyConsentInfoSite.Location = new System.Drawing.Point(550, 0);
            this.buttonSetPrivacyConsentInfoSite.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetPrivacyConsentInfoSite.Name = "buttonSetPrivacyConsentInfoSite";
            this.buttonSetPrivacyConsentInfoSite.Size = new System.Drawing.Size(24, 29);
            this.buttonSetPrivacyConsentInfoSite.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonSetPrivacyConsentInfoSite, "Set the site where more information about the storage and processing of personal " +
        "data can be found");
            this.buttonSetPrivacyConsentInfoSite.UseVisualStyleBackColor = true;
            this.buttonSetPrivacyConsentInfoSite.Visible = false;
            this.buttonSetPrivacyConsentInfoSite.Click += new System.EventHandler(this.buttonSetPrivacyConsentInfoSite_Click);
            // 
            // imageListLogin
            // 
            this.imageListLogin.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLogin.ImageStream")));
            this.imageListLogin.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLogin.Images.SetKeyName(0, "Login.ico");
            this.imageListLogin.Images.SetKeyName(1, "LoginLocked.ico");
            // 
            // FormLoginAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 546);
            this.Controls.Add(this.splitContainerMain);
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this, "Login administration");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLoginAdministration";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login administration";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.toolStripLoginList.ResumeLayout(false);
            this.toolStripLoginList.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.groupBoxLogin.ResumeLayout(false);
            this.tableLayoutPanelServerLogin.ResumeLayout(false);
            this.tableLayoutPanelServerLogin.PerformLayout();
            this.splitContainerLogin.Panel1.ResumeLayout(false);
            this.splitContainerLogin.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogin)).EndInit();
            this.splitContainerLogin.ResumeLayout(false);
            this.groupBoxDatabases.ResumeLayout(false);
            this.groupBoxDatabases.PerformLayout();
            this.toolStripDatabase.ResumeLayout(false);
            this.toolStripDatabase.PerformLayout();
            this.groupBoxDatabase.ResumeLayout(false);
            this.tableLayoutPanelLogin.ResumeLayout(false);
            this.groupBoxUser.ResumeLayout(false);
            this.tableLayoutPanelUser.ResumeLayout(false);
            this.tableLayoutPanelUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).EndInit();
            this.tabControlUserDetails.ResumeLayout(false);
            this.tabPageRoles.ResumeLayout(false);
            this.tableLayoutPanelRoles.ResumeLayout(false);
            this.tableLayoutPanelRoles.PerformLayout();
            this.tabPageProjects.ResumeLayout(false);
            this.tableLayoutPanelProjects.ResumeLayout(false);
            this.tableLayoutPanelProjects.PerformLayout();
            this.splitContainerProjectAccessible.Panel1.ResumeLayout(false);
            this.splitContainerProjectAccessible.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProjectAccessible)).EndInit();
            this.splitContainerProjectAccessible.ResumeLayout(false);
            this.tableLayoutPanelProjectsReadOnly.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProjectsLocked)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.tableLayoutPanelSettings.ResumeLayout(false);
            this.tableLayoutPanelSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSecurityAdmin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStrip toolStripLoginList;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoginCreate;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoginDelete;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.GroupBox groupBoxDatabases;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogin;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjects;
        private System.Windows.Forms.Label labelProjectsNotAvailable;
        private System.Windows.Forms.Label labelProjectsAvailable;
        private System.Windows.Forms.ListBox listBoxProjectsAvailable;
        private System.Windows.Forms.Button buttonProjectRemove;
        private System.Windows.Forms.Button buttonProjectAdd;
        private System.Windows.Forms.ListBox listBoxProjectsNotAvailable;
        private System.Windows.Forms.Label labelOrderProject;
        private System.Windows.Forms.RadioButton radioButtonOrderProjectByName;
        private System.Windows.Forms.RadioButton radioButtonOrderProjectByID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRoles;
        private System.Windows.Forms.ListBox listBoxUserRoles;
        private System.Windows.Forms.Label labelUserRoles;
        private System.Windows.Forms.Button buttonRoleRemove;
        private System.Windows.Forms.ListBox listBoxRoles;
        private System.Windows.Forms.Button buttonRoleAdd;
        private System.Windows.Forms.Label labelRoles;
        private System.Windows.Forms.GroupBox groupBoxUser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUser;
        private System.Windows.Forms.RadioButton radioButtonLoginOnly;
        private System.Windows.Forms.PictureBox pictureBoxUser;
        private System.Windows.Forms.RadioButton radioButtonUser;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryAgent;
        private System.Windows.Forms.TreeView treeViewDatabases;
        private System.Windows.Forms.TreeView treeViewLogins;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelServerLogin;
        private System.Windows.Forms.CheckBox checkBoxLoginHasAccess;
        private System.Windows.Forms.SplitContainer splitContainerLogin;
        private System.Windows.Forms.PictureBox pictureBoxLogin;
        private System.Windows.Forms.ImageList imageListLogin;
        private System.Windows.Forms.GroupBox groupBoxDatabase;
        private System.Windows.Forms.TabControl tabControlUserDetails;
        private System.Windows.Forms.TabPage tabPageRoles;
        private System.Windows.Forms.TabPage tabPageProjects;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSettings;
        private System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.Button buttonAddSettingsNode;
        private System.Windows.Forms.Button buttonRemoveSettingsNode;
        private System.Windows.Forms.Label labelSettingsHeader;
        public System.Windows.Forms.Button buttonSearchSettingsTemplate;
        private System.Windows.Forms.TextBox textBoxContentSettings;
        private System.Windows.Forms.Label labelContentSettings;
        private System.Windows.Forms.ImageList imageListUserDetails;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelDbUserInfo;
        private System.Windows.Forms.ComboBox comboBoxDBUserInfo;
        private System.Windows.Forms.Button buttonSynchronizeProjects;
        private System.Windows.Forms.PictureBox pictureBoxSecurityAdmin;
        private System.Windows.Forms.CheckBox checkBoxSecurityAdmin;
        private System.Windows.Forms.ToolTip toolTip;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryProject;
        private System.Windows.Forms.Label labelDefaultDB;
        private System.Windows.Forms.TextBox textBoxDefaultDB;
        private System.Windows.Forms.Label labelLoginInfo;
        private System.Windows.Forms.SplitContainer splitContainerProjectAccessible;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjectsReadOnly;
        private System.Windows.Forms.Button buttonProjectsReadOnlyAdd;
        private System.Windows.Forms.Button buttonProjectsReadOnlyRemove;
        private System.Windows.Forms.ListBox listBoxProjectsReadOnly;
        private System.Windows.Forms.Label labelProjectsReadOnly;
        private System.Windows.Forms.Label labelLogins;
        private System.Windows.Forms.Button buttonChangePW;
        private System.Windows.Forms.Button buttonLoginStatistics;
        private System.Windows.Forms.ToolStrip toolStripDatabase;
        private System.Windows.Forms.ToolStripButton toolStripButtonSynchronizeUserProxy;
        private System.Windows.Forms.Button buttonShowCurrentActivity;
        private System.Windows.Forms.Button buttonLoginOverview;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoginCopy;
        private System.Windows.Forms.ToolStripButton toolStripButtonListAllDatabases;
        private System.Windows.Forms.ToolStripButton toolStripButtonDatabaseOverview;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonRoleOverview;
        private System.Windows.Forms.Button buttonProjectAddAll;
        private System.Windows.Forms.Button buttonProjectRemoveAll;
        private System.Windows.Forms.Button buttonLinkedServer;
        private System.Windows.Forms.Button buttonRemoveProject;
        private System.Windows.Forms.CheckBox checkBoxIsDBO;
        private System.Windows.Forms.Button buttonProjectUserNotAvailable;
        private System.Windows.Forms.Button buttonProjectUserAvailable;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoginMissing;
        private System.Windows.Forms.Button buttonSetPrivacyConsentInfoSite;
        private System.Windows.Forms.CheckBox checkBoxPrivacyConsent;
        private System.Windows.Forms.Button buttonSetPrivacyConsent;
        private System.Windows.Forms.Button buttonProjectUserNotAvailableIsLocked;
        private System.Windows.Forms.Button buttonProjectUserAvailableIsLocked;
        private System.Windows.Forms.ListBox listBoxProjectsLocked;
        private System.Windows.Forms.PictureBox pictureBoxProjectsLocked;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxFilter;
        private System.Windows.Forms.ToolStripButton toolStripButtonFilter;
    }
}