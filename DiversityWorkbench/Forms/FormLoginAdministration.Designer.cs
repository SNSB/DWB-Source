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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoginAdministration));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            treeViewLogins = new System.Windows.Forms.TreeView();
            labelLogins = new System.Windows.Forms.Label();
            toolStripLoginList = new System.Windows.Forms.ToolStrip();
            toolStripButtonLoginCreate = new System.Windows.Forms.ToolStripButton();
            toolStripButtonLoginCopy = new System.Windows.Forms.ToolStripButton();
            toolStripButtonLoginDelete = new System.Windows.Forms.ToolStripButton();
            toolStripButtonFilter = new System.Windows.Forms.ToolStripButton();
            toolStripTextBoxFilter = new System.Windows.Forms.ToolStripTextBox();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            groupBoxLogin = new System.Windows.Forms.GroupBox();
            tableLayoutPanelServerLogin = new System.Windows.Forms.TableLayoutPanel();
            checkBoxLoginHasAccess = new System.Windows.Forms.CheckBox();
            splitContainerLogin = new System.Windows.Forms.SplitContainer();
            groupBoxDatabases = new System.Windows.Forms.GroupBox();
            treeViewDatabases = new System.Windows.Forms.TreeView();
            toolStripDatabase = new System.Windows.Forms.ToolStrip();
            toolStripButtonDatabaseOverview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSynchronizeUserProxy = new System.Windows.Forms.ToolStripButton();
            toolStripButtonListAllDatabases = new System.Windows.Forms.ToolStripButton();
            toolStripButtonLoginMissing = new System.Windows.Forms.ToolStripButton();
            groupBoxDatabase = new System.Windows.Forms.GroupBox();
            tableLayoutPanelLogin = new System.Windows.Forms.TableLayoutPanel();
            groupBoxUser = new System.Windows.Forms.GroupBox();
            tableLayoutPanelUser = new System.Windows.Forms.TableLayoutPanel();
            radioButtonLoginOnly = new System.Windows.Forms.RadioButton();
            pictureBoxUser = new System.Windows.Forms.PictureBox();
            radioButtonUser = new System.Windows.Forms.RadioButton();
            userControlModuleRelatedEntryAgent = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            checkBoxIsDBO = new System.Windows.Forms.CheckBox();
            checkBoxPrivacyConsent = new System.Windows.Forms.CheckBox();
            buttonSetPrivacyConsent = new System.Windows.Forms.Button();
            tabControlUserDetails = new System.Windows.Forms.TabControl();
            tabPageRoles = new System.Windows.Forms.TabPage();
            tableLayoutPanelRoles = new System.Windows.Forms.TableLayoutPanel();
            listBoxUserRoles = new System.Windows.Forms.ListBox();
            labelUserRoles = new System.Windows.Forms.Label();
            buttonRoleRemove = new System.Windows.Forms.Button();
            listBoxRoles = new System.Windows.Forms.ListBox();
            buttonRoleAdd = new System.Windows.Forms.Button();
            labelRoles = new System.Windows.Forms.Label();
            buttonRoleOverview = new System.Windows.Forms.Button();
            tabPageProjects = new System.Windows.Forms.TabPage();
            tableLayoutPanelProjects = new System.Windows.Forms.TableLayoutPanel();
            labelProjectsNotAvailable = new System.Windows.Forms.Label();
            labelProjectsAvailable = new System.Windows.Forms.Label();
            buttonProjectRemove = new System.Windows.Forms.Button();
            buttonProjectAdd = new System.Windows.Forms.Button();
            listBoxProjectsNotAvailable = new System.Windows.Forms.ListBox();
            labelOrderProject = new System.Windows.Forms.Label();
            radioButtonOrderProjectByName = new System.Windows.Forms.RadioButton();
            radioButtonOrderProjectByID = new System.Windows.Forms.RadioButton();
            buttonSynchronizeProjects = new System.Windows.Forms.Button();
            userControlModuleRelatedEntryProject = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            splitContainerProjectAccessible = new System.Windows.Forms.SplitContainer();
            listBoxProjectsAvailable = new System.Windows.Forms.ListBox();
            tableLayoutPanelProjectsReadOnly = new System.Windows.Forms.TableLayoutPanel();
            buttonProjectsReadOnlyAdd = new System.Windows.Forms.Button();
            buttonProjectsReadOnlyRemove = new System.Windows.Forms.Button();
            listBoxProjectsReadOnly = new System.Windows.Forms.ListBox();
            labelProjectsReadOnly = new System.Windows.Forms.Label();
            listBoxProjectsLocked = new System.Windows.Forms.ListBox();
            buttonProjectAddAll = new System.Windows.Forms.Button();
            buttonProjectRemoveAll = new System.Windows.Forms.Button();
            buttonRemoveProject = new System.Windows.Forms.Button();
            buttonProjectUserNotAvailable = new System.Windows.Forms.Button();
            buttonProjectUserAvailable = new System.Windows.Forms.Button();
            buttonProjectUserNotAvailableIsLocked = new System.Windows.Forms.Button();
            buttonProjectUserAvailableIsLocked = new System.Windows.Forms.Button();
            pictureBoxProjectsLocked = new System.Windows.Forms.PictureBox();
            tabPageSettings = new System.Windows.Forms.TabPage();
            tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            labelContentSettings = new System.Windows.Forms.Label();
            textBoxContentSettings = new System.Windows.Forms.TextBox();
            treeViewSettings = new System.Windows.Forms.TreeView();
            labelSettingsHeader = new System.Windows.Forms.Label();
            buttonSearchSettingsTemplate = new System.Windows.Forms.Button();
            buttonRemoveSettingsNode = new System.Windows.Forms.Button();
            buttonAddSettingsNode = new System.Windows.Forms.Button();
            imageListUserDetails = new System.Windows.Forms.ImageList(components);
            pictureBoxLogin = new System.Windows.Forms.PictureBox();
            labelDbUserInfo = new System.Windows.Forms.Label();
            buttonChangePW = new System.Windows.Forms.Button();
            comboBoxDBUserInfo = new System.Windows.Forms.ComboBox();
            pictureBoxSecurityAdmin = new System.Windows.Forms.PictureBox();
            checkBoxSecurityAdmin = new System.Windows.Forms.CheckBox();
            labelDefaultDB = new System.Windows.Forms.Label();
            textBoxDefaultDB = new System.Windows.Forms.TextBox();
            labelLoginInfo = new System.Windows.Forms.Label();
            buttonLoginStatistics = new System.Windows.Forms.Button();
            buttonLoginOverview = new System.Windows.Forms.Button();
            buttonShowCurrentActivity = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            buttonLinkedServer = new System.Windows.Forms.Button();
            buttonSetPrivacyConsentInfoSite = new System.Windows.Forms.Button();
            imageListLogin = new System.Windows.Forms.ImageList(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            toolTip = new System.Windows.Forms.ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            toolStripLoginList.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            groupBoxLogin.SuspendLayout();
            tableLayoutPanelServerLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerLogin).BeginInit();
            splitContainerLogin.Panel1.SuspendLayout();
            splitContainerLogin.Panel2.SuspendLayout();
            splitContainerLogin.SuspendLayout();
            groupBoxDatabases.SuspendLayout();
            toolStripDatabase.SuspendLayout();
            groupBoxDatabase.SuspendLayout();
            tableLayoutPanelLogin.SuspendLayout();
            groupBoxUser.SuspendLayout();
            tableLayoutPanelUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxUser).BeginInit();
            tabControlUserDetails.SuspendLayout();
            tabPageRoles.SuspendLayout();
            tableLayoutPanelRoles.SuspendLayout();
            tabPageProjects.SuspendLayout();
            tableLayoutPanelProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProjectAccessible).BeginInit();
            splitContainerProjectAccessible.Panel1.SuspendLayout();
            splitContainerProjectAccessible.Panel2.SuspendLayout();
            splitContainerProjectAccessible.SuspendLayout();
            tableLayoutPanelProjectsReadOnly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProjectsLocked).BeginInit();
            tabPageSettings.SuspendLayout();
            tableLayoutPanelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSecurityAdmin).BeginInit();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpNavigator(splitContainerMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProvider.SetHelpString(splitContainerMain, "Login administration");
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(treeViewLogins);
            splitContainerMain.Panel1.Controls.Add(labelLogins);
            splitContainerMain.Panel1.Controls.Add(toolStripLoginList);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(tableLayoutPanelMain);
            helpProvider.SetShowHelp(splitContainerMain, true);
            splitContainerMain.Size = new System.Drawing.Size(967, 630);
            splitContainerMain.SplitterDistance = 192;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // treeViewLogins
            // 
            treeViewLogins.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewLogins.Location = new System.Drawing.Point(0, 23);
            treeViewLogins.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewLogins.Name = "treeViewLogins";
            treeViewLogins.ShowLines = false;
            treeViewLogins.ShowPlusMinus = false;
            treeViewLogins.ShowRootLines = false;
            treeViewLogins.Size = new System.Drawing.Size(192, 582);
            treeViewLogins.TabIndex = 1;
            treeViewLogins.AfterSelect += treeViewLogins_AfterSelect;
            treeViewLogins.Click += treeViewLogins_Click;
            // 
            // labelLogins
            // 
            labelLogins.Dock = System.Windows.Forms.DockStyle.Top;
            labelLogins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelLogins.Location = new System.Drawing.Point(0, 0);
            labelLogins.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelLogins.Name = "labelLogins";
            labelLogins.Size = new System.Drawing.Size(192, 23);
            labelLogins.TabIndex = 2;
            labelLogins.Text = "Logins";
            labelLogins.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLoginList
            // 
            toolStripLoginList.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripLoginList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripLoginList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonLoginCreate, toolStripButtonLoginCopy, toolStripButtonLoginDelete, toolStripButtonFilter, toolStripTextBoxFilter });
            toolStripLoginList.Location = new System.Drawing.Point(0, 605);
            toolStripLoginList.Name = "toolStripLoginList";
            toolStripLoginList.Size = new System.Drawing.Size(192, 25);
            toolStripLoginList.TabIndex = 0;
            toolStripLoginList.Text = "toolStrip1";
            // 
            // toolStripButtonLoginCreate
            // 
            toolStripButtonLoginCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLoginCreate.Image = Properties.Resources.Login;
            toolStripButtonLoginCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLoginCreate.Name = "toolStripButtonLoginCreate";
            toolStripButtonLoginCreate.Size = new System.Drawing.Size(23, 22);
            toolStripButtonLoginCreate.Text = "New login";
            toolStripButtonLoginCreate.Click += toolStripButtonLoginCreate_Click;
            // 
            // toolStripButtonLoginCopy
            // 
            toolStripButtonLoginCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLoginCopy.Image = Properties.Resources.CopyAgent;
            toolStripButtonLoginCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLoginCopy.Name = "toolStripButtonLoginCopy";
            toolStripButtonLoginCopy.Size = new System.Drawing.Size(23, 22);
            toolStripButtonLoginCopy.Text = "Copy the selected login";
            toolStripButtonLoginCopy.Click += toolStripButtonLoginCopy_Click;
            // 
            // toolStripButtonLoginDelete
            // 
            toolStripButtonLoginDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLoginDelete.Image = Properties.Resources.Delete;
            toolStripButtonLoginDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLoginDelete.Name = "toolStripButtonLoginDelete";
            toolStripButtonLoginDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonLoginDelete.Text = "Delete login and remove all settings in the databases";
            toolStripButtonLoginDelete.Click += toolStripButtonLoginDelete_Click;
            // 
            // toolStripButtonFilter
            // 
            toolStripButtonFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonFilter.Image = Properties.Resources.Filter;
            toolStripButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonFilter.Name = "toolStripButtonFilter";
            toolStripButtonFilter.Size = new System.Drawing.Size(23, 22);
            toolStripButtonFilter.Text = "Filter for logins";
            toolStripButtonFilter.Click += toolStripButtonFilter_Click;
            // 
            // toolStripTextBoxFilter
            // 
            toolStripTextBoxFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripTextBoxFilter.Name = "toolStripTextBoxFilter";
            toolStripTextBoxFilter.Size = new System.Drawing.Size(58, 25);
            toolStripTextBoxFilter.ToolTipText = "Text for filtering logins. Use % as wildcard";
            toolStripTextBoxFilter.TextChanged += toolStripTextBoxFilter_TextChanged;
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 5;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 233F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tableLayoutPanelMain.Controls.Add(groupBoxLogin, 0, 1);
            tableLayoutPanelMain.Controls.Add(buttonShowCurrentActivity, 2, 0);
            tableLayoutPanelMain.Controls.Add(buttonFeedback, 4, 0);
            tableLayoutPanelMain.Controls.Add(buttonLinkedServer, 3, 0);
            tableLayoutPanelMain.Controls.Add(buttonSetPrivacyConsentInfoSite, 1, 0);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 2;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.Size = new System.Drawing.Size(770, 630);
            tableLayoutPanelMain.TabIndex = 2;
            // 
            // groupBoxLogin
            // 
            tableLayoutPanelMain.SetColumnSpan(groupBoxLogin, 5);
            groupBoxLogin.Controls.Add(tableLayoutPanelServerLogin);
            groupBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxLogin.Location = new System.Drawing.Point(4, 33);
            groupBoxLogin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            groupBoxLogin.Name = "groupBoxLogin";
            groupBoxLogin.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLogin.Size = new System.Drawing.Size(762, 594);
            groupBoxLogin.TabIndex = 0;
            groupBoxLogin.TabStop = false;
            groupBoxLogin.Text = "Login";
            // 
            // tableLayoutPanelServerLogin
            // 
            tableLayoutPanelServerLogin.ColumnCount = 7;
            tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelServerLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelServerLogin.Controls.Add(checkBoxLoginHasAccess, 1, 0);
            tableLayoutPanelServerLogin.Controls.Add(splitContainerLogin, 0, 3);
            tableLayoutPanelServerLogin.Controls.Add(pictureBoxLogin, 0, 0);
            tableLayoutPanelServerLogin.Controls.Add(labelDbUserInfo, 5, 0);
            tableLayoutPanelServerLogin.Controls.Add(buttonChangePW, 0, 2);
            tableLayoutPanelServerLogin.Controls.Add(comboBoxDBUserInfo, 5, 1);
            tableLayoutPanelServerLogin.Controls.Add(pictureBoxSecurityAdmin, 2, 0);
            tableLayoutPanelServerLogin.Controls.Add(checkBoxSecurityAdmin, 3, 0);
            tableLayoutPanelServerLogin.Controls.Add(labelDefaultDB, 4, 0);
            tableLayoutPanelServerLogin.Controls.Add(textBoxDefaultDB, 4, 1);
            tableLayoutPanelServerLogin.Controls.Add(labelLoginInfo, 2, 2);
            tableLayoutPanelServerLogin.Controls.Add(buttonLoginStatistics, 6, 1);
            tableLayoutPanelServerLogin.Controls.Add(buttonLoginOverview, 6, 2);
            tableLayoutPanelServerLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelServerLogin.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelServerLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelServerLogin.Name = "tableLayoutPanelServerLogin";
            tableLayoutPanelServerLogin.RowCount = 4;
            tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelServerLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelServerLogin.Size = new System.Drawing.Size(754, 575);
            tableLayoutPanelServerLogin.TabIndex = 0;
            // 
            // checkBoxLoginHasAccess
            // 
            checkBoxLoginHasAccess.AutoSize = true;
            checkBoxLoginHasAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxLoginHasAccess.Location = new System.Drawing.Point(31, 3);
            checkBoxLoginHasAccess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxLoginHasAccess.Name = "checkBoxLoginHasAccess";
            tableLayoutPanelServerLogin.SetRowSpan(checkBoxLoginHasAccess, 2);
            checkBoxLoginHasAccess.Size = new System.Drawing.Size(98, 42);
            checkBoxLoginHasAccess.TabIndex = 0;
            checkBoxLoginHasAccess.Text = "Enabled";
            checkBoxLoginHasAccess.UseVisualStyleBackColor = true;
            checkBoxLoginHasAccess.CheckedChanged += checkBoxLoginHasAccess_CheckedChanged;
            checkBoxLoginHasAccess.Click += checkBoxLoginHasAccess_Click;
            // 
            // splitContainerLogin
            // 
            tableLayoutPanelServerLogin.SetColumnSpan(splitContainerLogin, 7);
            splitContainerLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            splitContainerLogin.Location = new System.Drawing.Point(4, 84);
            splitContainerLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerLogin.Name = "splitContainerLogin";
            // 
            // splitContainerLogin.Panel1
            // 
            splitContainerLogin.Panel1.Controls.Add(groupBoxDatabases);
            // 
            // splitContainerLogin.Panel2
            // 
            splitContainerLogin.Panel2.Controls.Add(groupBoxDatabase);
            splitContainerLogin.Panel2.Padding = new System.Windows.Forms.Padding(0, 23, 0, 0);
            splitContainerLogin.Size = new System.Drawing.Size(746, 488);
            splitContainerLogin.SplitterDistance = 247;
            splitContainerLogin.SplitterWidth = 5;
            splitContainerLogin.TabIndex = 5;
            // 
            // groupBoxDatabases
            // 
            groupBoxDatabases.Controls.Add(treeViewDatabases);
            groupBoxDatabases.Controls.Add(toolStripDatabase);
            groupBoxDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxDatabases.Location = new System.Drawing.Point(0, 0);
            groupBoxDatabases.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxDatabases.Name = "groupBoxDatabases";
            groupBoxDatabases.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxDatabases.Size = new System.Drawing.Size(247, 488);
            groupBoxDatabases.TabIndex = 1;
            groupBoxDatabases.TabStop = false;
            groupBoxDatabases.Text = "Databases";
            // 
            // treeViewDatabases
            // 
            treeViewDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewDatabases.Location = new System.Drawing.Point(4, 16);
            treeViewDatabases.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewDatabases.Name = "treeViewDatabases";
            treeViewDatabases.Size = new System.Drawing.Size(239, 444);
            treeViewDatabases.TabIndex = 0;
            treeViewDatabases.AfterSelect += treeViewDatabases_AfterSelect;
            // 
            // toolStripDatabase
            // 
            toolStripDatabase.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripDatabase.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonDatabaseOverview, toolStripButtonSynchronizeUserProxy, toolStripButtonListAllDatabases, toolStripButtonLoginMissing });
            toolStripDatabase.Location = new System.Drawing.Point(4, 460);
            toolStripDatabase.Name = "toolStripDatabase";
            toolStripDatabase.Size = new System.Drawing.Size(239, 25);
            toolStripDatabase.TabIndex = 1;
            toolStripDatabase.Text = "toolStrip1";
            // 
            // toolStripButtonDatabaseOverview
            // 
            toolStripButtonDatabaseOverview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDatabaseOverview.Image = Properties.Resources.DatabaseLogin;
            toolStripButtonDatabaseOverview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDatabaseOverview.Name = "toolStripButtonDatabaseOverview";
            toolStripButtonDatabaseOverview.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDatabaseOverview.Text = "Show overview for database";
            toolStripButtonDatabaseOverview.Click += toolStripButtonDatabaseOverview_Click;
            // 
            // toolStripButtonSynchronizeUserProxy
            // 
            toolStripButtonSynchronizeUserProxy.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonSynchronizeUserProxy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSynchronizeUserProxy.Image = Properties.Resources.SynchronizeAgent;
            toolStripButtonSynchronizeUserProxy.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSynchronizeUserProxy.Name = "toolStripButtonSynchronizeUserProxy";
            toolStripButtonSynchronizeUserProxy.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSynchronizeUserProxy.Text = "Synchronize logins in table UserProxy with server  logins ";
            toolStripButtonSynchronizeUserProxy.Click += toolStripButtonSynchronizeUserProxy_Click;
            // 
            // toolStripButtonListAllDatabases
            // 
            toolStripButtonListAllDatabases.Image = ResourceWorkbench.DatabaseList;
            toolStripButtonListAllDatabases.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonListAllDatabases.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonListAllDatabases.Name = "toolStripButtonListAllDatabases";
            toolStripButtonListAllDatabases.Size = new System.Drawing.Size(115, 22);
            toolStripButtonListAllDatabases.Text = "List all databases";
            toolStripButtonListAllDatabases.Click += toolStripButtonListAllDatabases_Click;
            // 
            // toolStripButtonLoginMissing
            // 
            toolStripButtonLoginMissing.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonLoginMissing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLoginMissing.Image = Properties.Resources.LoginMissing;
            toolStripButtonLoginMissing.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLoginMissing.Name = "toolStripButtonLoginMissing";
            toolStripButtonLoginMissing.Size = new System.Drawing.Size(23, 22);
            toolStripButtonLoginMissing.Text = "Search for database users without a valid login on the server";
            toolStripButtonLoginMissing.Click += toolStripButtonLoginMissing_Click;
            // 
            // groupBoxDatabase
            // 
            groupBoxDatabase.Controls.Add(tableLayoutPanelLogin);
            groupBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxDatabase.Location = new System.Drawing.Point(0, 23);
            groupBoxDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxDatabase.Name = "groupBoxDatabase";
            groupBoxDatabase.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxDatabase.Size = new System.Drawing.Size(494, 465);
            groupBoxDatabase.TabIndex = 5;
            groupBoxDatabase.TabStop = false;
            groupBoxDatabase.Text = "Database";
            // 
            // tableLayoutPanelLogin
            // 
            tableLayoutPanelLogin.ColumnCount = 1;
            tableLayoutPanelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelLogin.Controls.Add(groupBoxUser, 0, 0);
            tableLayoutPanelLogin.Controls.Add(tabControlUserDetails, 0, 1);
            tableLayoutPanelLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tableLayoutPanelLogin.ForeColor = System.Drawing.Color.Black;
            tableLayoutPanelLogin.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelLogin.Name = "tableLayoutPanelLogin";
            tableLayoutPanelLogin.RowCount = 2;
            tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelLogin.Size = new System.Drawing.Size(486, 446);
            tableLayoutPanelLogin.TabIndex = 4;
            // 
            // groupBoxUser
            // 
            groupBoxUser.Controls.Add(tableLayoutPanelUser);
            groupBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxUser.ForeColor = System.Drawing.Color.Black;
            groupBoxUser.Location = new System.Drawing.Point(4, 3);
            groupBoxUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxUser.Name = "groupBoxUser";
            groupBoxUser.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxUser.Size = new System.Drawing.Size(478, 83);
            groupBoxUser.TabIndex = 3;
            groupBoxUser.TabStop = false;
            groupBoxUser.Text = "User";
            // 
            // tableLayoutPanelUser
            // 
            tableLayoutPanelUser.ColumnCount = 6;
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelUser.Controls.Add(radioButtonLoginOnly, 1, 0);
            tableLayoutPanelUser.Controls.Add(pictureBoxUser, 0, 0);
            tableLayoutPanelUser.Controls.Add(radioButtonUser, 2, 0);
            tableLayoutPanelUser.Controls.Add(userControlModuleRelatedEntryAgent, 0, 1);
            tableLayoutPanelUser.Controls.Add(checkBoxIsDBO, 5, 0);
            tableLayoutPanelUser.Controls.Add(checkBoxPrivacyConsent, 3, 0);
            tableLayoutPanelUser.Controls.Add(buttonSetPrivacyConsent, 4, 0);
            tableLayoutPanelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelUser.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelUser.Name = "tableLayoutPanelUser";
            tableLayoutPanelUser.RowCount = 2;
            tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelUser.Size = new System.Drawing.Size(470, 64);
            tableLayoutPanelUser.TabIndex = 3;
            // 
            // radioButtonLoginOnly
            // 
            radioButtonLoginOnly.AutoSize = true;
            radioButtonLoginOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonLoginOnly.ForeColor = System.Drawing.Color.Gray;
            radioButtonLoginOnly.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            radioButtonLoginOnly.Location = new System.Drawing.Point(31, 3);
            radioButtonLoginOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonLoginOnly.Name = "radioButtonLoginOnly";
            radioButtonLoginOnly.Size = new System.Drawing.Size(100, 24);
            radioButtonLoginOnly.TabIndex = 2;
            radioButtonLoginOnly.TabStop = true;
            radioButtonLoginOnly.Text = "Not in database";
            radioButtonLoginOnly.UseVisualStyleBackColor = true;
            radioButtonLoginOnly.Click += radioButtonLoginOnly_Click;
            // 
            // pictureBoxUser
            // 
            pictureBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxUser.Image = Properties.Resources.Agent;
            pictureBoxUser.Location = new System.Drawing.Point(4, 3);
            pictureBoxUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxUser.Name = "pictureBoxUser";
            pictureBoxUser.Size = new System.Drawing.Size(19, 24);
            pictureBoxUser.TabIndex = 3;
            pictureBoxUser.TabStop = false;
            // 
            // radioButtonUser
            // 
            radioButtonUser.AutoSize = true;
            radioButtonUser.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonUser.ForeColor = System.Drawing.Color.Black;
            radioButtonUser.Location = new System.Drawing.Point(139, 3);
            radioButtonUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonUser.Name = "radioButtonUser";
            radioButtonUser.Size = new System.Drawing.Size(81, 24);
            radioButtonUser.TabIndex = 4;
            radioButtonUser.TabStop = true;
            radioButtonUser.Text = "In database";
            radioButtonUser.UseVisualStyleBackColor = true;
            radioButtonUser.Click += radioButtonUser_Click;
            // 
            // userControlModuleRelatedEntryAgent
            // 
            userControlModuleRelatedEntryAgent.CanDeleteConnectionToModule = true;
            tableLayoutPanelUser.SetColumnSpan(userControlModuleRelatedEntryAgent, 6);
            userControlModuleRelatedEntryAgent.DependsOnUri = "";
            userControlModuleRelatedEntryAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryAgent.Domain = "";
            userControlModuleRelatedEntryAgent.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryAgent.Location = new System.Drawing.Point(4, 32);
            userControlModuleRelatedEntryAgent.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            userControlModuleRelatedEntryAgent.Module = null;
            userControlModuleRelatedEntryAgent.Name = "userControlModuleRelatedEntryAgent";
            userControlModuleRelatedEntryAgent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryAgent.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryAgent.ShowInfo = false;
            userControlModuleRelatedEntryAgent.Size = new System.Drawing.Size(462, 30);
            userControlModuleRelatedEntryAgent.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryAgent.TabIndex = 6;
            // 
            // checkBoxIsDBO
            // 
            checkBoxIsDBO.AutoSize = true;
            checkBoxIsDBO.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxIsDBO.Location = new System.Drawing.Point(411, 3);
            checkBoxIsDBO.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxIsDBO.Name = "checkBoxIsDBO";
            checkBoxIsDBO.Size = new System.Drawing.Size(55, 24);
            checkBoxIsDBO.TabIndex = 7;
            checkBoxIsDBO.Text = "Is dbo";
            toolTip.SetToolTip(checkBoxIsDBO, "If the login is a database owner of the database");
            checkBoxIsDBO.UseVisualStyleBackColor = true;
            checkBoxIsDBO.Click += checkBoxIsDBO_Click;
            // 
            // checkBoxPrivacyConsent
            // 
            checkBoxPrivacyConsent.AutoCheck = false;
            checkBoxPrivacyConsent.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxPrivacyConsent.Enabled = false;
            checkBoxPrivacyConsent.Image = Properties.Resources.Paragraf;
            checkBoxPrivacyConsent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxPrivacyConsent.Location = new System.Drawing.Point(228, 0);
            checkBoxPrivacyConsent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            checkBoxPrivacyConsent.Name = "checkBoxPrivacyConsent";
            checkBoxPrivacyConsent.Size = new System.Drawing.Size(117, 30);
            checkBoxPrivacyConsent.TabIndex = 5;
            checkBoxPrivacyConsent.UseVisualStyleBackColor = true;
            checkBoxPrivacyConsent.Visible = false;
            // 
            // buttonSetPrivacyConsent
            // 
            buttonSetPrivacyConsent.BackColor = System.Drawing.Color.PaleGreen;
            buttonSetPrivacyConsent.FlatAppearance.BorderSize = 0;
            buttonSetPrivacyConsent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonSetPrivacyConsent.Image = Properties.Resources.Paragraf;
            buttonSetPrivacyConsent.Location = new System.Drawing.Point(349, 0);
            buttonSetPrivacyConsent.Margin = new System.Windows.Forms.Padding(0);
            buttonSetPrivacyConsent.Name = "buttonSetPrivacyConsent";
            buttonSetPrivacyConsent.Size = new System.Drawing.Size(14, 27);
            buttonSetPrivacyConsent.TabIndex = 8;
            toolTip.SetToolTip(buttonSetPrivacyConsent, "Set the privacy consent to YES");
            buttonSetPrivacyConsent.UseVisualStyleBackColor = false;
            buttonSetPrivacyConsent.Visible = false;
            buttonSetPrivacyConsent.Click += buttonSetPrivacyConsent_Click;
            // 
            // tabControlUserDetails
            // 
            tabControlUserDetails.Controls.Add(tabPageRoles);
            tabControlUserDetails.Controls.Add(tabPageProjects);
            tabControlUserDetails.Controls.Add(tabPageSettings);
            tabControlUserDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlUserDetails.ImageList = imageListUserDetails;
            tabControlUserDetails.Location = new System.Drawing.Point(4, 92);
            tabControlUserDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlUserDetails.Name = "tabControlUserDetails";
            tabControlUserDetails.SelectedIndex = 0;
            tabControlUserDetails.Size = new System.Drawing.Size(478, 351);
            tabControlUserDetails.TabIndex = 5;
            // 
            // tabPageRoles
            // 
            tabPageRoles.Controls.Add(tableLayoutPanelRoles);
            tabPageRoles.ImageIndex = 0;
            tabPageRoles.Location = new System.Drawing.Point(4, 23);
            tabPageRoles.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageRoles.Name = "tabPageRoles";
            tabPageRoles.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageRoles.Size = new System.Drawing.Size(470, 324);
            tabPageRoles.TabIndex = 0;
            tabPageRoles.Text = "Roles";
            tabPageRoles.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRoles
            // 
            tableLayoutPanelRoles.ColumnCount = 3;
            tableLayoutPanelRoles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelRoles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelRoles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelRoles.Controls.Add(listBoxUserRoles, 2, 1);
            tableLayoutPanelRoles.Controls.Add(labelUserRoles, 2, 0);
            tableLayoutPanelRoles.Controls.Add(buttonRoleRemove, 1, 3);
            tableLayoutPanelRoles.Controls.Add(listBoxRoles, 0, 1);
            tableLayoutPanelRoles.Controls.Add(buttonRoleAdd, 1, 2);
            tableLayoutPanelRoles.Controls.Add(labelRoles, 0, 0);
            tableLayoutPanelRoles.Controls.Add(buttonRoleOverview, 1, 1);
            tableLayoutPanelRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelRoles.Location = new System.Drawing.Point(4, 3);
            tableLayoutPanelRoles.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelRoles.Name = "tableLayoutPanelRoles";
            tableLayoutPanelRoles.RowCount = 4;
            tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelRoles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanelRoles.Size = new System.Drawing.Size(462, 318);
            tableLayoutPanelRoles.TabIndex = 0;
            // 
            // listBoxUserRoles
            // 
            listBoxUserRoles.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            listBoxUserRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxUserRoles.FormattingEnabled = true;
            listBoxUserRoles.IntegralHeight = false;
            listBoxUserRoles.ItemHeight = 13;
            listBoxUserRoles.Location = new System.Drawing.Point(249, 16);
            listBoxUserRoles.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            listBoxUserRoles.Name = "listBoxUserRoles";
            tableLayoutPanelRoles.SetRowSpan(listBoxUserRoles, 3);
            listBoxUserRoles.Size = new System.Drawing.Size(209, 299);
            listBoxUserRoles.TabIndex = 4;
            // 
            // labelUserRoles
            // 
            labelUserRoles.AutoSize = true;
            labelUserRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            labelUserRoles.ForeColor = System.Drawing.Color.Green;
            labelUserRoles.Location = new System.Drawing.Point(253, 0);
            labelUserRoles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelUserRoles.Name = "labelUserRoles";
            labelUserRoles.Size = new System.Drawing.Size(205, 13);
            labelUserRoles.TabIndex = 18;
            labelUserRoles.Text = "Granted";
            // 
            // buttonRoleRemove
            // 
            buttonRoleRemove.Dock = System.Windows.Forms.DockStyle.Top;
            buttonRoleRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonRoleRemove.Location = new System.Drawing.Point(216, 158);
            buttonRoleRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonRoleRemove.Name = "buttonRoleRemove";
            buttonRoleRemove.Size = new System.Drawing.Size(29, 24);
            buttonRoleRemove.TabIndex = 25;
            buttonRoleRemove.Tag = "";
            buttonRoleRemove.Text = "<";
            buttonRoleRemove.UseVisualStyleBackColor = true;
            buttonRoleRemove.Click += buttonRoleRemove_Click;
            // 
            // listBoxRoles
            // 
            listBoxRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxRoles.FormattingEnabled = true;
            listBoxRoles.IntegralHeight = false;
            listBoxRoles.ItemHeight = 13;
            listBoxRoles.Location = new System.Drawing.Point(4, 16);
            listBoxRoles.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            listBoxRoles.Name = "listBoxRoles";
            tableLayoutPanelRoles.SetRowSpan(listBoxRoles, 3);
            listBoxRoles.Size = new System.Drawing.Size(208, 299);
            listBoxRoles.TabIndex = 12;
            // 
            // buttonRoleAdd
            // 
            buttonRoleAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonRoleAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonRoleAdd.Location = new System.Drawing.Point(216, 128);
            buttonRoleAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonRoleAdd.Name = "buttonRoleAdd";
            buttonRoleAdd.Size = new System.Drawing.Size(29, 24);
            buttonRoleAdd.TabIndex = 24;
            buttonRoleAdd.Text = ">";
            buttonRoleAdd.UseVisualStyleBackColor = true;
            buttonRoleAdd.Click += buttonRoleAdd_Click;
            // 
            // labelRoles
            // 
            labelRoles.AutoSize = true;
            labelRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRoles.ForeColor = System.Drawing.Color.Crimson;
            labelRoles.Location = new System.Drawing.Point(4, 0);
            labelRoles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelRoles.Name = "labelRoles";
            labelRoles.Size = new System.Drawing.Size(204, 13);
            labelRoles.TabIndex = 17;
            labelRoles.Text = "Available";
            // 
            // buttonRoleOverview
            // 
            buttonRoleOverview.Image = Properties.Resources.RoleOverview;
            buttonRoleOverview.Location = new System.Drawing.Point(212, 16);
            buttonRoleOverview.Margin = new System.Windows.Forms.Padding(0, 3, 7, 3);
            buttonRoleOverview.Name = "buttonRoleOverview";
            buttonRoleOverview.Size = new System.Drawing.Size(29, 28);
            buttonRoleOverview.TabIndex = 26;
            toolTip.SetToolTip(buttonRoleOverview, "Show permissions of the selected role");
            buttonRoleOverview.UseVisualStyleBackColor = true;
            buttonRoleOverview.Click += buttonRoleOverview_Click;
            // 
            // tabPageProjects
            // 
            tabPageProjects.Controls.Add(tableLayoutPanelProjects);
            tabPageProjects.ImageIndex = 1;
            tabPageProjects.Location = new System.Drawing.Point(4, 23);
            tabPageProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageProjects.Name = "tabPageProjects";
            tabPageProjects.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageProjects.Size = new System.Drawing.Size(470, 324);
            tabPageProjects.TabIndex = 1;
            tabPageProjects.Text = "Projects";
            tabPageProjects.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjects
            // 
            tableLayoutPanelProjects.ColumnCount = 4;
            tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            tableLayoutPanelProjects.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjects.Controls.Add(labelProjectsNotAvailable, 0, 0);
            tableLayoutPanelProjects.Controls.Add(labelProjectsAvailable, 3, 0);
            tableLayoutPanelProjects.Controls.Add(buttonProjectRemove, 1, 5);
            tableLayoutPanelProjects.Controls.Add(buttonProjectAdd, 1, 4);
            tableLayoutPanelProjects.Controls.Add(listBoxProjectsNotAvailable, 0, 1);
            tableLayoutPanelProjects.Controls.Add(labelOrderProject, 0, 8);
            tableLayoutPanelProjects.Controls.Add(radioButtonOrderProjectByName, 2, 8);
            tableLayoutPanelProjects.Controls.Add(radioButtonOrderProjectByID, 1, 8);
            tableLayoutPanelProjects.Controls.Add(buttonSynchronizeProjects, 1, 10);
            tableLayoutPanelProjects.Controls.Add(userControlModuleRelatedEntryProject, 0, 9);
            tableLayoutPanelProjects.Controls.Add(splitContainerProjectAccessible, 3, 1);
            tableLayoutPanelProjects.Controls.Add(buttonProjectAddAll, 1, 3);
            tableLayoutPanelProjects.Controls.Add(buttonProjectRemoveAll, 1, 6);
            tableLayoutPanelProjects.Controls.Add(buttonRemoveProject, 0, 10);
            tableLayoutPanelProjects.Controls.Add(buttonProjectUserNotAvailable, 1, 0);
            tableLayoutPanelProjects.Controls.Add(buttonProjectUserAvailable, 2, 0);
            tableLayoutPanelProjects.Controls.Add(buttonProjectUserNotAvailableIsLocked, 1, 2);
            tableLayoutPanelProjects.Controls.Add(buttonProjectUserAvailableIsLocked, 2, 2);
            tableLayoutPanelProjects.Controls.Add(pictureBoxProjectsLocked, 2, 7);
            tableLayoutPanelProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelProjects.Location = new System.Drawing.Point(4, 3);
            tableLayoutPanelProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelProjects.Name = "tableLayoutPanelProjects";
            tableLayoutPanelProjects.RowCount = 11;
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjects.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelProjects.Size = new System.Drawing.Size(462, 318);
            tableLayoutPanelProjects.TabIndex = 0;
            // 
            // labelProjectsNotAvailable
            // 
            labelProjectsNotAvailable.AutoSize = true;
            labelProjectsNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectsNotAvailable.ForeColor = System.Drawing.Color.Red;
            labelProjectsNotAvailable.Image = Properties.Resources.NoAccess16;
            labelProjectsNotAvailable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelProjectsNotAvailable.Location = new System.Drawing.Point(4, 0);
            labelProjectsNotAvailable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectsNotAvailable.Name = "labelProjectsNotAvailable";
            labelProjectsNotAvailable.Size = new System.Drawing.Size(202, 26);
            labelProjectsNotAvailable.TabIndex = 16;
            labelProjectsNotAvailable.Text = "      No access";
            labelProjectsNotAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProjectsAvailable
            // 
            labelProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectsAvailable.ForeColor = System.Drawing.Color.Green;
            labelProjectsAvailable.Image = Properties.Resources.ProjectOpen;
            labelProjectsAvailable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelProjectsAvailable.Location = new System.Drawing.Point(256, 3);
            labelProjectsAvailable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            labelProjectsAvailable.Name = "labelProjectsAvailable";
            labelProjectsAvailable.Size = new System.Drawing.Size(202, 23);
            labelProjectsAvailable.TabIndex = 15;
            labelProjectsAvailable.Text = "      Accessible";
            labelProjectsAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonProjectRemove
            // 
            tableLayoutPanelProjects.SetColumnSpan(buttonProjectRemove, 2);
            buttonProjectRemove.Dock = System.Windows.Forms.DockStyle.Top;
            buttonProjectRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectRemove.ForeColor = System.Drawing.Color.Red;
            buttonProjectRemove.Location = new System.Drawing.Point(214, 117);
            buttonProjectRemove.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            buttonProjectRemove.Name = "buttonProjectRemove";
            buttonProjectRemove.Size = new System.Drawing.Size(34, 23);
            buttonProjectRemove.TabIndex = 23;
            buttonProjectRemove.Text = "<";
            toolTip.SetToolTip(buttonProjectRemove, "Remove the selected project from the Accessible list");
            buttonProjectRemove.UseVisualStyleBackColor = true;
            buttonProjectRemove.Click += buttonProjectRemove_Click;
            // 
            // buttonProjectAdd
            // 
            tableLayoutPanelProjects.SetColumnSpan(buttonProjectAdd, 2);
            buttonProjectAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonProjectAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectAdd.ForeColor = System.Drawing.Color.Green;
            buttonProjectAdd.Location = new System.Drawing.Point(214, 94);
            buttonProjectAdd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            buttonProjectAdd.Name = "buttonProjectAdd";
            buttonProjectAdd.Size = new System.Drawing.Size(34, 23);
            buttonProjectAdd.TabIndex = 22;
            buttonProjectAdd.Text = ">";
            toolTip.SetToolTip(buttonProjectAdd, "Add the selected project to the Accessible list");
            buttonProjectAdd.UseVisualStyleBackColor = true;
            buttonProjectAdd.Click += buttonProjectAdd_Click;
            // 
            // listBoxProjectsNotAvailable
            // 
            listBoxProjectsNotAvailable.BackColor = System.Drawing.Color.Pink;
            listBoxProjectsNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectsNotAvailable.FormattingEnabled = true;
            listBoxProjectsNotAvailable.IntegralHeight = false;
            listBoxProjectsNotAvailable.ItemHeight = 13;
            listBoxProjectsNotAvailable.Location = new System.Drawing.Point(4, 26);
            listBoxProjectsNotAvailable.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            listBoxProjectsNotAvailable.Name = "listBoxProjectsNotAvailable";
            tableLayoutPanelProjects.SetRowSpan(listBoxProjectsNotAvailable, 7);
            listBoxProjectsNotAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listBoxProjectsNotAvailable.Size = new System.Drawing.Size(206, 210);
            listBoxProjectsNotAvailable.TabIndex = 6;
            listBoxProjectsNotAvailable.Click += listBoxProjectsNotAvailable_Click;
            listBoxProjectsNotAvailable.SelectedIndexChanged += listBoxProjectsNotAvailable_SelectedIndexChanged;
            // 
            // labelOrderProject
            // 
            labelOrderProject.AutoSize = true;
            labelOrderProject.Dock = System.Windows.Forms.DockStyle.Right;
            labelOrderProject.Location = new System.Drawing.Point(146, 236);
            labelOrderProject.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelOrderProject.Name = "labelOrderProject";
            labelOrderProject.Size = new System.Drawing.Size(64, 23);
            labelOrderProject.TabIndex = 39;
            labelOrderProject.Text = "Order by: ID";
            labelOrderProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButtonOrderProjectByName
            // 
            radioButtonOrderProjectByName.AutoSize = true;
            radioButtonOrderProjectByName.Checked = true;
            tableLayoutPanelProjects.SetColumnSpan(radioButtonOrderProjectByName, 2);
            radioButtonOrderProjectByName.Dock = System.Windows.Forms.DockStyle.Left;
            radioButtonOrderProjectByName.Location = new System.Drawing.Point(235, 236);
            radioButtonOrderProjectByName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            radioButtonOrderProjectByName.Name = "radioButtonOrderProjectByName";
            radioButtonOrderProjectByName.Size = new System.Drawing.Size(53, 23);
            radioButtonOrderProjectByName.TabIndex = 40;
            radioButtonOrderProjectByName.TabStop = true;
            radioButtonOrderProjectByName.Text = "Name";
            radioButtonOrderProjectByName.UseVisualStyleBackColor = true;
            // 
            // radioButtonOrderProjectByID
            // 
            radioButtonOrderProjectByID.AutoSize = true;
            radioButtonOrderProjectByID.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonOrderProjectByID.Location = new System.Drawing.Point(214, 239);
            radioButtonOrderProjectByID.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            radioButtonOrderProjectByID.Name = "radioButtonOrderProjectByID";
            radioButtonOrderProjectByID.Size = new System.Drawing.Size(17, 17);
            radioButtonOrderProjectByID.TabIndex = 41;
            radioButtonOrderProjectByID.Text = "ID";
            radioButtonOrderProjectByID.UseVisualStyleBackColor = true;
            radioButtonOrderProjectByID.CheckedChanged += radioButtonOrderProjectByID_CheckedChanged;
            // 
            // buttonSynchronizeProjects
            // 
            tableLayoutPanelProjects.SetColumnSpan(buttonSynchronizeProjects, 3);
            buttonSynchronizeProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSynchronizeProjects.Image = ResourceWorkbench.Download;
            buttonSynchronizeProjects.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonSynchronizeProjects.Location = new System.Drawing.Point(210, 290);
            buttonSynchronizeProjects.Margin = new System.Windows.Forms.Padding(0);
            buttonSynchronizeProjects.Name = "buttonSynchronizeProjects";
            buttonSynchronizeProjects.Size = new System.Drawing.Size(252, 28);
            buttonSynchronizeProjects.TabIndex = 42;
            buttonSynchronizeProjects.Text = "Load projects from DiversityProjects";
            buttonSynchronizeProjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonSynchronizeProjects, "Load projects defined in the module DiversityProjects");
            buttonSynchronizeProjects.UseVisualStyleBackColor = true;
            buttonSynchronizeProjects.Click += buttonSynchronizeProjects_Click;
            // 
            // userControlModuleRelatedEntryProject
            // 
            userControlModuleRelatedEntryProject.CanDeleteConnectionToModule = true;
            tableLayoutPanelProjects.SetColumnSpan(userControlModuleRelatedEntryProject, 4);
            userControlModuleRelatedEntryProject.DependsOnUri = "";
            userControlModuleRelatedEntryProject.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryProject.Domain = "";
            userControlModuleRelatedEntryProject.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryProject.Location = new System.Drawing.Point(5, 262);
            userControlModuleRelatedEntryProject.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlModuleRelatedEntryProject.Module = null;
            userControlModuleRelatedEntryProject.Name = "userControlModuleRelatedEntryProject";
            userControlModuleRelatedEntryProject.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryProject.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryProject.ShowInfo = false;
            userControlModuleRelatedEntryProject.Size = new System.Drawing.Size(452, 25);
            userControlModuleRelatedEntryProject.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryProject.TabIndex = 43;
            // 
            // splitContainerProjectAccessible
            // 
            splitContainerProjectAccessible.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerProjectAccessible.Location = new System.Drawing.Point(252, 26);
            splitContainerProjectAccessible.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            splitContainerProjectAccessible.Name = "splitContainerProjectAccessible";
            splitContainerProjectAccessible.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerProjectAccessible.Panel1
            // 
            splitContainerProjectAccessible.Panel1.Controls.Add(listBoxProjectsAvailable);
            // 
            // splitContainerProjectAccessible.Panel2
            // 
            splitContainerProjectAccessible.Panel2.Controls.Add(tableLayoutPanelProjectsReadOnly);
            tableLayoutPanelProjects.SetRowSpan(splitContainerProjectAccessible, 7);
            splitContainerProjectAccessible.Size = new System.Drawing.Size(206, 210);
            splitContainerProjectAccessible.SplitterDistance = 123;
            splitContainerProjectAccessible.SplitterWidth = 2;
            splitContainerProjectAccessible.TabIndex = 44;
            // 
            // listBoxProjectsAvailable
            // 
            listBoxProjectsAvailable.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            listBoxProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectsAvailable.FormattingEnabled = true;
            listBoxProjectsAvailable.IntegralHeight = false;
            listBoxProjectsAvailable.ItemHeight = 13;
            listBoxProjectsAvailable.Location = new System.Drawing.Point(0, 0);
            listBoxProjectsAvailable.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            listBoxProjectsAvailable.Name = "listBoxProjectsAvailable";
            listBoxProjectsAvailable.Size = new System.Drawing.Size(206, 123);
            listBoxProjectsAvailable.TabIndex = 5;
            listBoxProjectsAvailable.Click += listBoxProjectsAvailable_Click;
            listBoxProjectsAvailable.SelectedIndexChanged += listBoxProjectsAvailable_SelectedIndexChanged;
            // 
            // tableLayoutPanelProjectsReadOnly
            // 
            tableLayoutPanelProjectsReadOnly.ColumnCount = 3;
            tableLayoutPanelProjectsReadOnly.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProjectsReadOnly.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectsReadOnly.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectsReadOnly.Controls.Add(buttonProjectsReadOnlyAdd, 1, 0);
            tableLayoutPanelProjectsReadOnly.Controls.Add(buttonProjectsReadOnlyRemove, 2, 0);
            tableLayoutPanelProjectsReadOnly.Controls.Add(listBoxProjectsReadOnly, 0, 1);
            tableLayoutPanelProjectsReadOnly.Controls.Add(labelProjectsReadOnly, 0, 0);
            tableLayoutPanelProjectsReadOnly.Controls.Add(listBoxProjectsLocked, 0, 2);
            tableLayoutPanelProjectsReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelProjectsReadOnly.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelProjectsReadOnly.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            tableLayoutPanelProjectsReadOnly.Name = "tableLayoutPanelProjectsReadOnly";
            tableLayoutPanelProjectsReadOnly.RowCount = 3;
            tableLayoutPanelProjectsReadOnly.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjectsReadOnly.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelProjectsReadOnly.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjectsReadOnly.Size = new System.Drawing.Size(206, 85);
            tableLayoutPanelProjectsReadOnly.TabIndex = 0;
            // 
            // buttonProjectsReadOnlyAdd
            // 
            buttonProjectsReadOnlyAdd.Dock = System.Windows.Forms.DockStyle.Right;
            buttonProjectsReadOnlyAdd.Image = Properties.Resources.ArrowDownSmall;
            buttonProjectsReadOnlyAdd.Location = new System.Drawing.Point(112, 0);
            buttonProjectsReadOnlyAdd.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonProjectsReadOnlyAdd.Name = "buttonProjectsReadOnlyAdd";
            buttonProjectsReadOnlyAdd.Size = new System.Drawing.Size(35, 23);
            buttonProjectsReadOnlyAdd.TabIndex = 0;
            toolTip.SetToolTip(buttonProjectsReadOnlyAdd, "Change selected project to read only");
            buttonProjectsReadOnlyAdd.UseVisualStyleBackColor = true;
            buttonProjectsReadOnlyAdd.Click += buttonProjectsReadOnlyAdd_Click;
            // 
            // buttonProjectsReadOnlyRemove
            // 
            buttonProjectsReadOnlyRemove.Dock = System.Windows.Forms.DockStyle.Left;
            buttonProjectsReadOnlyRemove.Image = Properties.Resources.ArrowUpSmall;
            buttonProjectsReadOnlyRemove.Location = new System.Drawing.Point(155, 0);
            buttonProjectsReadOnlyRemove.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            buttonProjectsReadOnlyRemove.Name = "buttonProjectsReadOnlyRemove";
            buttonProjectsReadOnlyRemove.Size = new System.Drawing.Size(35, 23);
            buttonProjectsReadOnlyRemove.TabIndex = 1;
            toolTip.SetToolTip(buttonProjectsReadOnlyRemove, "Remove selected project from read only list");
            buttonProjectsReadOnlyRemove.UseVisualStyleBackColor = true;
            buttonProjectsReadOnlyRemove.Click += buttonProjectsReadOnlyRemove_Click;
            // 
            // listBoxProjectsReadOnly
            // 
            listBoxProjectsReadOnly.BackColor = System.Drawing.SystemColors.ControlLight;
            tableLayoutPanelProjectsReadOnly.SetColumnSpan(listBoxProjectsReadOnly, 3);
            listBoxProjectsReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectsReadOnly.FormattingEnabled = true;
            listBoxProjectsReadOnly.IntegralHeight = false;
            listBoxProjectsReadOnly.ItemHeight = 13;
            listBoxProjectsReadOnly.Location = new System.Drawing.Point(0, 23);
            listBoxProjectsReadOnly.Margin = new System.Windows.Forms.Padding(0);
            listBoxProjectsReadOnly.Name = "listBoxProjectsReadOnly";
            listBoxProjectsReadOnly.Size = new System.Drawing.Size(206, 40);
            listBoxProjectsReadOnly.TabIndex = 2;
            listBoxProjectsReadOnly.Click += listBoxProjectsReadOnly_Click;
            // 
            // labelProjectsReadOnly
            // 
            labelProjectsReadOnly.ForeColor = System.Drawing.Color.DimGray;
            labelProjectsReadOnly.Image = Properties.Resources.ProjectGrey;
            labelProjectsReadOnly.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelProjectsReadOnly.Location = new System.Drawing.Point(0, 0);
            labelProjectsReadOnly.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            labelProjectsReadOnly.Name = "labelProjectsReadOnly";
            labelProjectsReadOnly.Size = new System.Drawing.Size(93, 23);
            labelProjectsReadOnly.TabIndex = 3;
            labelProjectsReadOnly.Text = "      Read Only";
            labelProjectsReadOnly.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBoxProjectsLocked
            // 
            listBoxProjectsLocked.BackColor = System.Drawing.SystemColors.ControlLight;
            tableLayoutPanelProjectsReadOnly.SetColumnSpan(listBoxProjectsLocked, 3);
            listBoxProjectsLocked.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectsLocked.ForeColor = System.Drawing.Color.Red;
            listBoxProjectsLocked.FormattingEnabled = true;
            listBoxProjectsLocked.IntegralHeight = false;
            listBoxProjectsLocked.ItemHeight = 13;
            listBoxProjectsLocked.Location = new System.Drawing.Point(0, 63);
            listBoxProjectsLocked.Margin = new System.Windows.Forms.Padding(0);
            listBoxProjectsLocked.Name = "listBoxProjectsLocked";
            listBoxProjectsLocked.SelectionMode = System.Windows.Forms.SelectionMode.None;
            listBoxProjectsLocked.Size = new System.Drawing.Size(206, 22);
            listBoxProjectsLocked.TabIndex = 4;
            toolTip.SetToolTip(listBoxProjectsLocked, "Locked projects");
            listBoxProjectsLocked.Visible = false;
            // 
            // buttonProjectAddAll
            // 
            tableLayoutPanelProjects.SetColumnSpan(buttonProjectAddAll, 2);
            buttonProjectAddAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonProjectAddAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectAddAll.ForeColor = System.Drawing.Color.Green;
            buttonProjectAddAll.Location = new System.Drawing.Point(214, 71);
            buttonProjectAddAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            buttonProjectAddAll.Name = "buttonProjectAddAll";
            buttonProjectAddAll.Size = new System.Drawing.Size(34, 23);
            buttonProjectAddAll.TabIndex = 45;
            buttonProjectAddAll.Text = ">>";
            toolTip.SetToolTip(buttonProjectAddAll, "Add all projects to the Accessible list");
            buttonProjectAddAll.UseVisualStyleBackColor = true;
            buttonProjectAddAll.Click += buttonProjectAddAll_Click;
            // 
            // buttonProjectRemoveAll
            // 
            tableLayoutPanelProjects.SetColumnSpan(buttonProjectRemoveAll, 2);
            buttonProjectRemoveAll.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonProjectRemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectRemoveAll.ForeColor = System.Drawing.Color.Red;
            buttonProjectRemoveAll.Location = new System.Drawing.Point(214, 140);
            buttonProjectRemoveAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            buttonProjectRemoveAll.Name = "buttonProjectRemoveAll";
            buttonProjectRemoveAll.Size = new System.Drawing.Size(34, 23);
            buttonProjectRemoveAll.TabIndex = 46;
            buttonProjectRemoveAll.Text = "<<";
            toolTip.SetToolTip(buttonProjectRemoveAll, "Remove all projects from the Accessible list");
            buttonProjectRemoveAll.UseVisualStyleBackColor = true;
            buttonProjectRemoveAll.Click += buttonProjectRemoveAll_Click;
            // 
            // buttonRemoveProject
            // 
            buttonRemoveProject.Dock = System.Windows.Forms.DockStyle.Left;
            buttonRemoveProject.Image = Properties.Resources.Delete;
            buttonRemoveProject.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonRemoveProject.Location = new System.Drawing.Point(0, 290);
            buttonRemoveProject.Margin = new System.Windows.Forms.Padding(0);
            buttonRemoveProject.Name = "buttonRemoveProject";
            buttonRemoveProject.Size = new System.Drawing.Size(125, 28);
            buttonRemoveProject.TabIndex = 47;
            buttonRemoveProject.Text = "Remove project";
            buttonRemoveProject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonRemoveProject, "Remove the project selected in the No access list from the database");
            buttonRemoveProject.UseVisualStyleBackColor = true;
            buttonRemoveProject.Click += buttonRemoveProject_Click;
            // 
            // buttonProjectUserNotAvailable
            // 
            buttonProjectUserNotAvailable.BackColor = System.Drawing.Color.Pink;
            buttonProjectUserNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonProjectUserNotAvailable.FlatAppearance.BorderSize = 0;
            buttonProjectUserNotAvailable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonProjectUserNotAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectUserNotAvailable.ForeColor = System.Drawing.Color.Red;
            buttonProjectUserNotAvailable.Image = Properties.Resources.Agent;
            buttonProjectUserNotAvailable.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            buttonProjectUserNotAvailable.Location = new System.Drawing.Point(210, 0);
            buttonProjectUserNotAvailable.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectUserNotAvailable.Name = "buttonProjectUserNotAvailable";
            tableLayoutPanelProjects.SetRowSpan(buttonProjectUserNotAvailable, 2);
            buttonProjectUserNotAvailable.Size = new System.Drawing.Size(21, 45);
            buttonProjectUserNotAvailable.TabIndex = 48;
            buttonProjectUserNotAvailable.Text = "?";
            buttonProjectUserNotAvailable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            toolTip.SetToolTip(buttonProjectUserNotAvailable, "Show Users for selected project");
            buttonProjectUserNotAvailable.UseVisualStyleBackColor = false;
            buttonProjectUserNotAvailable.Click += buttonProjectUserNotAvailable_Click;
            // 
            // buttonProjectUserAvailable
            // 
            buttonProjectUserAvailable.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            buttonProjectUserAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonProjectUserAvailable.FlatAppearance.BorderSize = 0;
            buttonProjectUserAvailable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonProjectUserAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectUserAvailable.ForeColor = System.Drawing.Color.Green;
            buttonProjectUserAvailable.Image = Properties.Resources.Agent;
            buttonProjectUserAvailable.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            buttonProjectUserAvailable.Location = new System.Drawing.Point(231, 0);
            buttonProjectUserAvailable.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectUserAvailable.Name = "buttonProjectUserAvailable";
            tableLayoutPanelProjects.SetRowSpan(buttonProjectUserAvailable, 2);
            buttonProjectUserAvailable.Size = new System.Drawing.Size(21, 45);
            buttonProjectUserAvailable.TabIndex = 49;
            buttonProjectUserAvailable.Text = "?";
            buttonProjectUserAvailable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            toolTip.SetToolTip(buttonProjectUserAvailable, "Show users for selected project");
            buttonProjectUserAvailable.UseVisualStyleBackColor = false;
            buttonProjectUserAvailable.Click += buttonProjectUserAvailable_Click;
            // 
            // buttonProjectUserNotAvailableIsLocked
            // 
            buttonProjectUserNotAvailableIsLocked.Dock = System.Windows.Forms.DockStyle.Top;
            buttonProjectUserNotAvailableIsLocked.FlatAppearance.BorderSize = 0;
            buttonProjectUserNotAvailableIsLocked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonProjectUserNotAvailableIsLocked.Image = Properties.Resources.ProjectOpen;
            buttonProjectUserNotAvailableIsLocked.Location = new System.Drawing.Point(210, 45);
            buttonProjectUserNotAvailableIsLocked.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectUserNotAvailableIsLocked.Name = "buttonProjectUserNotAvailableIsLocked";
            buttonProjectUserNotAvailableIsLocked.Size = new System.Drawing.Size(21, 23);
            buttonProjectUserNotAvailableIsLocked.TabIndex = 50;
            buttonProjectUserNotAvailableIsLocked.UseVisualStyleBackColor = true;
            buttonProjectUserNotAvailableIsLocked.Visible = false;
            buttonProjectUserNotAvailableIsLocked.Click += buttonProjectUserNotAvailableIsLocked_Click;
            // 
            // buttonProjectUserAvailableIsLocked
            // 
            buttonProjectUserAvailableIsLocked.Dock = System.Windows.Forms.DockStyle.Top;
            buttonProjectUserAvailableIsLocked.FlatAppearance.BorderSize = 0;
            buttonProjectUserAvailableIsLocked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonProjectUserAvailableIsLocked.Image = Properties.Resources.ProjectGrey;
            buttonProjectUserAvailableIsLocked.Location = new System.Drawing.Point(231, 45);
            buttonProjectUserAvailableIsLocked.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectUserAvailableIsLocked.Name = "buttonProjectUserAvailableIsLocked";
            buttonProjectUserAvailableIsLocked.Size = new System.Drawing.Size(21, 23);
            buttonProjectUserAvailableIsLocked.TabIndex = 51;
            buttonProjectUserAvailableIsLocked.UseVisualStyleBackColor = true;
            buttonProjectUserAvailableIsLocked.Visible = false;
            buttonProjectUserAvailableIsLocked.Click += buttonProjectUserAvailableIsLocked_Click;
            // 
            // pictureBoxProjectsLocked
            // 
            pictureBoxProjectsLocked.Dock = System.Windows.Forms.DockStyle.Bottom;
            pictureBoxProjectsLocked.Image = Properties.Resources.Project;
            pictureBoxProjectsLocked.Location = new System.Drawing.Point(231, 215);
            pictureBoxProjectsLocked.Margin = new System.Windows.Forms.Padding(0);
            pictureBoxProjectsLocked.Name = "pictureBoxProjectsLocked";
            pictureBoxProjectsLocked.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            pictureBoxProjectsLocked.Size = new System.Drawing.Size(21, 21);
            pictureBoxProjectsLocked.TabIndex = 52;
            pictureBoxProjectsLocked.TabStop = false;
            toolTip.SetToolTip(pictureBoxProjectsLocked, "Locked projects");
            pictureBoxProjectsLocked.Visible = false;
            pictureBoxProjectsLocked.Click += pictureBoxProjectsLocked_Click;
            // 
            // tabPageSettings
            // 
            tabPageSettings.Controls.Add(tableLayoutPanelSettings);
            tabPageSettings.ImageIndex = 2;
            tabPageSettings.Location = new System.Drawing.Point(4, 23);
            tabPageSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSettings.Name = "tabPageSettings";
            tabPageSettings.Size = new System.Drawing.Size(470, 324);
            tabPageSettings.TabIndex = 2;
            tabPageSettings.Text = "Settings";
            tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSettings
            // 
            tableLayoutPanelSettings.ColumnCount = 5;
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.Controls.Add(labelContentSettings, 0, 2);
            tableLayoutPanelSettings.Controls.Add(textBoxContentSettings, 0, 3);
            tableLayoutPanelSettings.Controls.Add(treeViewSettings, 0, 1);
            tableLayoutPanelSettings.Controls.Add(labelSettingsHeader, 0, 0);
            tableLayoutPanelSettings.Controls.Add(buttonSearchSettingsTemplate, 4, 0);
            tableLayoutPanelSettings.Controls.Add(buttonRemoveSettingsNode, 3, 0);
            tableLayoutPanelSettings.Controls.Add(buttonAddSettingsNode, 2, 0);
            tableLayoutPanelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            tableLayoutPanelSettings.RowCount = 4;
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.Size = new System.Drawing.Size(470, 324);
            tableLayoutPanelSettings.TabIndex = 1;
            // 
            // labelContentSettings
            // 
            labelContentSettings.AutoSize = true;
            tableLayoutPanelSettings.SetColumnSpan(labelContentSettings, 5);
            labelContentSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            labelContentSettings.Location = new System.Drawing.Point(4, 271);
            labelContentSettings.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelContentSettings.Name = "labelContentSettings";
            labelContentSettings.Size = new System.Drawing.Size(462, 13);
            labelContentSettings.TabIndex = 7;
            labelContentSettings.Text = "Content:";
            labelContentSettings.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxContentSettings
            // 
            tableLayoutPanelSettings.SetColumnSpan(textBoxContentSettings, 5);
            textBoxContentSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxContentSettings.Location = new System.Drawing.Point(4, 287);
            textBoxContentSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxContentSettings.Multiline = true;
            textBoxContentSettings.Name = "textBoxContentSettings";
            textBoxContentSettings.Size = new System.Drawing.Size(462, 34);
            textBoxContentSettings.TabIndex = 6;
            textBoxContentSettings.Text = "Content";
            textBoxContentSettings.Leave += textBoxSettingsContent_Leave;
            // 
            // treeViewSettings
            // 
            tableLayoutPanelSettings.SetColumnSpan(treeViewSettings, 5);
            treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewSettings.Location = new System.Drawing.Point(4, 38);
            treeViewSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewSettings.Name = "treeViewSettings";
            treeViewSettings.Size = new System.Drawing.Size(462, 223);
            treeViewSettings.TabIndex = 0;
            treeViewSettings.AfterSelect += treeViewSettings_AfterSelect;
            // 
            // labelSettingsHeader
            // 
            labelSettingsHeader.AutoSize = true;
            tableLayoutPanelSettings.SetColumnSpan(labelSettingsHeader, 2);
            labelSettingsHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSettingsHeader.Location = new System.Drawing.Point(4, 0);
            labelSettingsHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSettingsHeader.Name = "labelSettingsHeader";
            labelSettingsHeader.Size = new System.Drawing.Size(270, 35);
            labelSettingsHeader.TabIndex = 3;
            labelSettingsHeader.Text = "Edit the settings for the user";
            labelSettingsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSearchSettingsTemplate
            // 
            buttonSearchSettingsTemplate.Dock = System.Windows.Forms.DockStyle.Right;
            buttonSearchSettingsTemplate.Location = new System.Drawing.Point(356, 3);
            buttonSearchSettingsTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonSearchSettingsTemplate.Name = "buttonSearchSettingsTemplate";
            buttonSearchSettingsTemplate.Size = new System.Drawing.Size(110, 29);
            buttonSearchSettingsTemplate.TabIndex = 4;
            buttonSearchSettingsTemplate.Text = "Search template";
            buttonSearchSettingsTemplate.UseVisualStyleBackColor = true;
            buttonSearchSettingsTemplate.Click += buttonSearchSettingsTemplate_Click;
            // 
            // buttonRemoveSettingsNode
            // 
            buttonRemoveSettingsNode.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonRemoveSettingsNode.Image = Properties.Resources.Delete;
            buttonRemoveSettingsNode.Location = new System.Drawing.Point(319, 3);
            buttonRemoveSettingsNode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonRemoveSettingsNode.Name = "buttonRemoveSettingsNode";
            buttonRemoveSettingsNode.Size = new System.Drawing.Size(29, 29);
            buttonRemoveSettingsNode.TabIndex = 2;
            buttonRemoveSettingsNode.UseVisualStyleBackColor = true;
            buttonRemoveSettingsNode.Click += buttonRemoveSettingsNode_Click;
            // 
            // buttonAddSettingsNode
            // 
            buttonAddSettingsNode.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonAddSettingsNode.Image = Properties.Resources.Add;
            buttonAddSettingsNode.Location = new System.Drawing.Point(282, 3);
            buttonAddSettingsNode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAddSettingsNode.Name = "buttonAddSettingsNode";
            buttonAddSettingsNode.Size = new System.Drawing.Size(29, 29);
            buttonAddSettingsNode.TabIndex = 1;
            buttonAddSettingsNode.UseVisualStyleBackColor = true;
            buttonAddSettingsNode.Click += buttonAddSettingsNode_Click;
            // 
            // imageListUserDetails
            // 
            imageListUserDetails.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListUserDetails.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListUserDetails.ImageStream");
            imageListUserDetails.TransparentColor = System.Drawing.Color.Transparent;
            imageListUserDetails.Images.SetKeyName(0, "Group.ico");
            imageListUserDetails.Images.SetKeyName(1, "Project.ico");
            imageListUserDetails.Images.SetKeyName(2, "Settings.ico");
            // 
            // pictureBoxLogin
            // 
            pictureBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxLogin.Location = new System.Drawing.Point(4, 10);
            pictureBoxLogin.Margin = new System.Windows.Forms.Padding(4, 10, 4, 3);
            pictureBoxLogin.Name = "pictureBoxLogin";
            tableLayoutPanelServerLogin.SetRowSpan(pictureBoxLogin, 2);
            pictureBoxLogin.Size = new System.Drawing.Size(19, 35);
            pictureBoxLogin.TabIndex = 2;
            pictureBoxLogin.TabStop = false;
            // 
            // labelDbUserInfo
            // 
            labelDbUserInfo.AutoSize = true;
            labelDbUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDbUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelDbUserInfo.ForeColor = System.Drawing.Color.Black;
            labelDbUserInfo.Location = new System.Drawing.Point(517, 0);
            labelDbUserInfo.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelDbUserInfo.Name = "labelDbUserInfo";
            labelDbUserInfo.Size = new System.Drawing.Size(209, 16);
            labelDbUserInfo.TabIndex = 6;
            labelDbUserInfo.Text = "Database for user informations:";
            labelDbUserInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonChangePW
            // 
            tableLayoutPanelServerLogin.SetColumnSpan(buttonChangePW, 2);
            buttonChangePW.Dock = System.Windows.Forms.DockStyle.Right;
            buttonChangePW.Enabled = false;
            buttonChangePW.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            buttonChangePW.Image = Properties.Resources.KeyBig;
            buttonChangePW.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonChangePW.Location = new System.Drawing.Point(0, 48);
            buttonChangePW.Margin = new System.Windows.Forms.Padding(0);
            buttonChangePW.Name = "buttonChangePW";
            buttonChangePW.Size = new System.Drawing.Size(133, 33);
            buttonChangePW.TabIndex = 7;
            buttonChangePW.Text = "Change password";
            buttonChangePW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonChangePW, "Change the password for the selected login");
            buttonChangePW.UseVisualStyleBackColor = true;
            buttonChangePW.Click += buttonChangePW_Click;
            // 
            // comboBoxDBUserInfo
            // 
            comboBoxDBUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxDBUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            comboBoxDBUserInfo.FormattingEnabled = true;
            comboBoxDBUserInfo.Location = new System.Drawing.Point(513, 16);
            comboBoxDBUserInfo.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            comboBoxDBUserInfo.Name = "comboBoxDBUserInfo";
            comboBoxDBUserInfo.Size = new System.Drawing.Size(209, 21);
            comboBoxDBUserInfo.TabIndex = 7;
            // 
            // pictureBoxSecurityAdmin
            // 
            pictureBoxSecurityAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxSecurityAdmin.Image = Properties.Resources.Security;
            pictureBoxSecurityAdmin.Location = new System.Drawing.Point(137, 10);
            pictureBoxSecurityAdmin.Margin = new System.Windows.Forms.Padding(4, 10, 4, 3);
            pictureBoxSecurityAdmin.Name = "pictureBoxSecurityAdmin";
            tableLayoutPanelServerLogin.SetRowSpan(pictureBoxSecurityAdmin, 2);
            pictureBoxSecurityAdmin.Size = new System.Drawing.Size(19, 35);
            pictureBoxSecurityAdmin.TabIndex = 8;
            pictureBoxSecurityAdmin.TabStop = false;
            // 
            // checkBoxSecurityAdmin
            // 
            checkBoxSecurityAdmin.AutoSize = true;
            checkBoxSecurityAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxSecurityAdmin.Location = new System.Drawing.Point(160, 3);
            checkBoxSecurityAdmin.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            checkBoxSecurityAdmin.Name = "checkBoxSecurityAdmin";
            tableLayoutPanelServerLogin.SetRowSpan(checkBoxSecurityAdmin, 2);
            checkBoxSecurityAdmin.Size = new System.Drawing.Size(142, 42);
            checkBoxSecurityAdmin.TabIndex = 9;
            checkBoxSecurityAdmin.Text = "System administrator";
            toolTip.SetToolTip(checkBoxSecurityAdmin, "Allow login to administrate the server");
            checkBoxSecurityAdmin.UseVisualStyleBackColor = true;
            checkBoxSecurityAdmin.Click += checkBoxSecurityAdmin_Click;
            // 
            // labelDefaultDB
            // 
            labelDefaultDB.AutoSize = true;
            labelDefaultDB.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDefaultDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelDefaultDB.Location = new System.Drawing.Point(306, 0);
            labelDefaultDB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDefaultDB.Name = "labelDefaultDB";
            labelDefaultDB.Size = new System.Drawing.Size(203, 16);
            labelDefaultDB.TabIndex = 10;
            labelDefaultDB.Text = "Default database";
            labelDefaultDB.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxDefaultDB
            // 
            textBoxDefaultDB.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxDefaultDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBoxDefaultDB.Location = new System.Drawing.Point(306, 16);
            textBoxDefaultDB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            textBoxDefaultDB.Name = "textBoxDefaultDB";
            textBoxDefaultDB.ReadOnly = true;
            textBoxDefaultDB.Size = new System.Drawing.Size(203, 20);
            textBoxDefaultDB.TabIndex = 11;
            // 
            // labelLoginInfo
            // 
            labelLoginInfo.AutoSize = true;
            tableLayoutPanelServerLogin.SetColumnSpan(labelLoginInfo, 4);
            labelLoginInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLoginInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelLoginInfo.Location = new System.Drawing.Point(137, 48);
            labelLoginInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelLoginInfo.Name = "labelLoginInfo";
            labelLoginInfo.Size = new System.Drawing.Size(585, 33);
            labelLoginInfo.TabIndex = 12;
            labelLoginInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonLoginStatistics
            // 
            buttonLoginStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            buttonLoginStatistics.Image = Properties.Resources.Graph;
            buttonLoginStatistics.Location = new System.Drawing.Point(726, 16);
            buttonLoginStatistics.Margin = new System.Windows.Forms.Padding(0);
            buttonLoginStatistics.Name = "buttonLoginStatistics";
            buttonLoginStatistics.Size = new System.Drawing.Size(28, 27);
            buttonLoginStatistics.TabIndex = 13;
            toolTip.SetToolTip(buttonLoginStatistics, "Show the activity of the login");
            buttonLoginStatistics.UseVisualStyleBackColor = true;
            buttonLoginStatistics.Click += buttonLoginStatistics_Click;
            // 
            // buttonLoginOverview
            // 
            buttonLoginOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonLoginOverview.Image = Properties.Resources.AgentOverview;
            buttonLoginOverview.Location = new System.Drawing.Point(726, 50);
            buttonLoginOverview.Margin = new System.Windows.Forms.Padding(0, 2, 0, 3);
            buttonLoginOverview.Name = "buttonLoginOverview";
            buttonLoginOverview.Size = new System.Drawing.Size(28, 28);
            buttonLoginOverview.TabIndex = 14;
            toolTip.SetToolTip(buttonLoginOverview, "Show a summary for the login");
            buttonLoginOverview.UseVisualStyleBackColor = true;
            buttonLoginOverview.Click += buttonLoginOverview_Click;
            // 
            // buttonShowCurrentActivity
            // 
            buttonShowCurrentActivity.Dock = System.Windows.Forms.DockStyle.Right;
            buttonShowCurrentActivity.Image = Properties.Resources.ServerIO;
            buttonShowCurrentActivity.Location = new System.Drawing.Point(673, 0);
            buttonShowCurrentActivity.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            buttonShowCurrentActivity.Name = "buttonShowCurrentActivity";
            buttonShowCurrentActivity.Size = new System.Drawing.Size(31, 33);
            buttonShowCurrentActivity.TabIndex = 1;
            toolTip.SetToolTip(buttonShowCurrentActivity, "Show the current activity on the server");
            buttonShowCurrentActivity.UseVisualStyleBackColor = true;
            buttonShowCurrentActivity.Click += buttonShowCurrentActivity_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.Image = Properties.Resources.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(736, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(30, 33);
            buttonFeedback.TabIndex = 2;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback to the software developer");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // buttonLinkedServer
            // 
            buttonLinkedServer.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonLinkedServer.Image = Properties.Resources.ServerLinked;
            buttonLinkedServer.Location = new System.Drawing.Point(704, 0);
            buttonLinkedServer.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonLinkedServer.Name = "buttonLinkedServer";
            buttonLinkedServer.Size = new System.Drawing.Size(28, 33);
            buttonLinkedServer.TabIndex = 3;
            toolTip.SetToolTip(buttonLinkedServer, "Administrate linked servers");
            buttonLinkedServer.UseVisualStyleBackColor = true;
            buttonLinkedServer.Click += buttonLinkedServer_Click;
            // 
            // buttonSetPrivacyConsentInfoSite
            // 
            buttonSetPrivacyConsentInfoSite.Dock = System.Windows.Forms.DockStyle.Right;
            buttonSetPrivacyConsentInfoSite.Image = Properties.Resources.Paragraf;
            buttonSetPrivacyConsentInfoSite.Location = new System.Drawing.Point(641, 0);
            buttonSetPrivacyConsentInfoSite.Margin = new System.Windows.Forms.Padding(0);
            buttonSetPrivacyConsentInfoSite.Name = "buttonSetPrivacyConsentInfoSite";
            buttonSetPrivacyConsentInfoSite.Size = new System.Drawing.Size(28, 33);
            buttonSetPrivacyConsentInfoSite.TabIndex = 4;
            toolTip.SetToolTip(buttonSetPrivacyConsentInfoSite, "Set the site where more information about the storage and processing of personal data can be found");
            buttonSetPrivacyConsentInfoSite.UseVisualStyleBackColor = true;
            buttonSetPrivacyConsentInfoSite.Visible = false;
            buttonSetPrivacyConsentInfoSite.Click += buttonSetPrivacyConsentInfoSite_Click;
            // 
            // imageListLogin
            // 
            imageListLogin.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListLogin.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListLogin.ImageStream");
            imageListLogin.TransparentColor = System.Drawing.Color.Transparent;
            imageListLogin.Images.SetKeyName(0, "Login.ico");
            imageListLogin.Images.SetKeyName(1, "LoginLocked.ico");
            // 
            // FormLoginAdministration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(967, 630);
            Controls.Add(splitContainerMain);
            helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProvider.SetHelpString(this, "Login administration");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormLoginAdministration";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Login administration";
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel1.PerformLayout();
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            toolStripLoginList.ResumeLayout(false);
            toolStripLoginList.PerformLayout();
            tableLayoutPanelMain.ResumeLayout(false);
            groupBoxLogin.ResumeLayout(false);
            tableLayoutPanelServerLogin.ResumeLayout(false);
            tableLayoutPanelServerLogin.PerformLayout();
            splitContainerLogin.Panel1.ResumeLayout(false);
            splitContainerLogin.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerLogin).EndInit();
            splitContainerLogin.ResumeLayout(false);
            groupBoxDatabases.ResumeLayout(false);
            groupBoxDatabases.PerformLayout();
            toolStripDatabase.ResumeLayout(false);
            toolStripDatabase.PerformLayout();
            groupBoxDatabase.ResumeLayout(false);
            tableLayoutPanelLogin.ResumeLayout(false);
            groupBoxUser.ResumeLayout(false);
            tableLayoutPanelUser.ResumeLayout(false);
            tableLayoutPanelUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxUser).EndInit();
            tabControlUserDetails.ResumeLayout(false);
            tabPageRoles.ResumeLayout(false);
            tableLayoutPanelRoles.ResumeLayout(false);
            tableLayoutPanelRoles.PerformLayout();
            tabPageProjects.ResumeLayout(false);
            tableLayoutPanelProjects.ResumeLayout(false);
            tableLayoutPanelProjects.PerformLayout();
            splitContainerProjectAccessible.Panel1.ResumeLayout(false);
            splitContainerProjectAccessible.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerProjectAccessible).EndInit();
            splitContainerProjectAccessible.ResumeLayout(false);
            tableLayoutPanelProjectsReadOnly.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxProjectsLocked).EndInit();
            tabPageSettings.ResumeLayout(false);
            tableLayoutPanelSettings.ResumeLayout(false);
            tableLayoutPanelSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogin).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSecurityAdmin).EndInit();
            ResumeLayout(false);

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