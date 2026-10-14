namespace DiversityProjects.GFBio
{
    partial class FormGFBioImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGFBioImport));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.pictureBoxLogin = new System.Windows.Forms.PictureBox();
            this.labelKey = new System.Windows.Forms.Label();
            this.textBoxKey = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageImport = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelImport = new System.Windows.Forms.TableLayoutPanel();
            this.panelImport = new System.Windows.Forms.Panel();
            this.radioButtonImportNewProject = new System.Windows.Forms.RadioButton();
            this.radioButtonImportIntoProject = new System.Windows.Forms.RadioButton();
            this.textBoxImportNewProject = new System.Windows.Forms.TextBox();
            this.comboBoxImportIntoProject = new System.Windows.Forms.ComboBox();
            this.labelParentProject = new System.Windows.Forms.Label();
            this.comboBoxParentProject = new System.Windows.Forms.ComboBox();
            this.buttonImportStart = new System.Windows.Forms.Button();
            this.labelLicenseHolder = new System.Windows.Forms.Label();
            this.labelIPR = new System.Windows.Forms.Label();
            this.comboBoxAgentsDatabase = new System.Windows.Forms.ComboBox();
            this.labelAgentDatabase = new System.Windows.Forms.Label();
            this.labelAgentsProject = new System.Windows.Forms.Label();
            this.comboBoxAgentsProject = new System.Windows.Forms.ComboBox();
            this.labelSubmitter = new System.Windows.Forms.Label();
            this.labelPublisher = new System.Windows.Forms.Label();
            this.comboBoxAgentRoleDefault = new System.Windows.Forms.ComboBox();
            this.labelAgentRoleDefault = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryLicenseHolder = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryIPR = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryInstitution = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryPublisher = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.panelNameGeneration = new System.Windows.Forms.Panel();
            this.radioButtonAutogenerateName = new System.Windows.Forms.RadioButton();
            this.radioButtonKeepKeyAsName = new System.Windows.Forms.RadioButton();
            this.radioButtonEditProjectName = new System.Windows.Forms.RadioButton();
            this.tabPageGFBio = new System.Windows.Forms.TabPage();
            this.imageListTabControl = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlWebViewGFBio = new DiversityWorkbench.UserControls.UserControlWebView();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogin)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageImport.SuspendLayout();
            this.tableLayoutPanelImport.SuspendLayout();
            this.panelNameGeneration.SuspendLayout();
            this.tabPageGFBio.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 8;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelUser, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxUser, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelPassword, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxPassword, 4, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonConnect, 7, 1);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxLogin, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelKey, 5, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxKey, 6, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tabControl, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(660, 646);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 3);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(223, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Import data from GFBio";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUser.Location = new System.Drawing.Point(23, 19);
            this.labelUser.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(32, 24);
            this.labelUser.TabIndex = 2;
            this.labelUser.Text = "User:";
            this.labelUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxUser
            // 
            this.textBoxUser.BackColor = System.Drawing.Color.Pink;
            this.textBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUser.Location = new System.Drawing.Point(55, 21);
            this.textBoxUser.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(174, 20);
            this.textBoxUser.TabIndex = 3;
            this.textBoxUser.TextChanged += new System.EventHandler(this.textBoxUser_TextChanged);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPassword.Location = new System.Drawing.Point(232, 19);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 24);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Password:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BackColor = System.Drawing.Color.Pink;
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword.Location = new System.Drawing.Point(288, 21);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(174, 20);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Image = global::DiversityProjects.Properties.Resources.Connection;
            this.buttonConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonConnect.Location = new System.Drawing.Point(580, 19);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 24);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // pictureBoxLogin
            // 
            this.pictureBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLogin.Image = global::DiversityProjects.Properties.Resources.Login;
            this.pictureBoxLogin.Location = new System.Drawing.Point(3, 22);
            this.pictureBoxLogin.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.pictureBoxLogin.Name = "pictureBoxLogin";
            this.pictureBoxLogin.Size = new System.Drawing.Size(17, 21);
            this.pictureBoxLogin.TabIndex = 6;
            this.pictureBoxLogin.TabStop = false;
            // 
            // labelKey
            // 
            this.labelKey.AutoSize = true;
            this.labelKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelKey.Location = new System.Drawing.Point(462, 19);
            this.labelKey.Margin = new System.Windows.Forms.Padding(0);
            this.labelKey.Name = "labelKey";
            this.labelKey.Size = new System.Drawing.Size(28, 24);
            this.labelKey.TabIndex = 7;
            this.labelKey.Text = "Key:";
            this.labelKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxKey
            // 
            this.textBoxKey.BackColor = System.Drawing.Color.Pink;
            this.textBoxKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxKey.Location = new System.Drawing.Point(490, 21);
            this.textBoxKey.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.Size = new System.Drawing.Size(87, 20);
            this.textBoxKey.TabIndex = 8;
            this.textBoxKey.TextChanged += new System.EventHandler(this.textBoxKey_TextChanged);
            // 
            // tabControl
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.tabControl, 8);
            this.tabControl.Controls.Add(this.tabPageImport);
            this.tabControl.Controls.Add(this.tabPageGFBio);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageListTabControl;
            this.tabControl.Location = new System.Drawing.Point(3, 46);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(654, 597);
            this.tabControl.TabIndex = 9;
            // 
            // tabPageImport
            // 
            this.tabPageImport.Controls.Add(this.tableLayoutPanelImport);
            this.tabPageImport.ImageIndex = 1;
            this.tabPageImport.Location = new System.Drawing.Point(4, 23);
            this.tabPageImport.Name = "tabPageImport";
            this.tabPageImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImport.Size = new System.Drawing.Size(646, 570);
            this.tabPageImport.TabIndex = 1;
            this.tabPageImport.Text = "Import";
            this.tabPageImport.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelImport
            // 
            this.tableLayoutPanelImport.ColumnCount = 7;
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanelImport.Controls.Add(this.panelImport, 0, 5);
            this.tableLayoutPanelImport.Controls.Add(this.radioButtonImportNewProject, 0, 7);
            this.tableLayoutPanelImport.Controls.Add(this.radioButtonImportIntoProject, 0, 6);
            this.tableLayoutPanelImport.Controls.Add(this.textBoxImportNewProject, 2, 7);
            this.tableLayoutPanelImport.Controls.Add(this.comboBoxImportIntoProject, 2, 6);
            this.tableLayoutPanelImport.Controls.Add(this.labelParentProject, 5, 6);
            this.tableLayoutPanelImport.Controls.Add(this.comboBoxParentProject, 5, 7);
            this.tableLayoutPanelImport.Controls.Add(this.buttonImportStart, 6, 9);
            this.tableLayoutPanelImport.Controls.Add(this.labelLicenseHolder, 0, 0);
            this.tableLayoutPanelImport.Controls.Add(this.labelIPR, 0, 1);
            this.tableLayoutPanelImport.Controls.Add(this.comboBoxAgentsDatabase, 2, 9);
            this.tableLayoutPanelImport.Controls.Add(this.labelAgentDatabase, 1, 9);
            this.tableLayoutPanelImport.Controls.Add(this.labelAgentsProject, 0, 10);
            this.tableLayoutPanelImport.Controls.Add(this.comboBoxAgentsProject, 3, 10);
            this.tableLayoutPanelImport.Controls.Add(this.labelSubmitter, 0, 4);
            this.tableLayoutPanelImport.Controls.Add(this.labelPublisher, 0, 3);
            this.tableLayoutPanelImport.Controls.Add(this.comboBoxAgentRoleDefault, 3, 11);
            this.tableLayoutPanelImport.Controls.Add(this.labelAgentRoleDefault, 0, 11);
            this.tableLayoutPanelImport.Controls.Add(this.userControlModuleRelatedEntryLicenseHolder, 1, 0);
            this.tableLayoutPanelImport.Controls.Add(this.userControlModuleRelatedEntryIPR, 1, 1);
            this.tableLayoutPanelImport.Controls.Add(this.userControlModuleRelatedEntryInstitution, 1, 4);
            this.tableLayoutPanelImport.Controls.Add(this.userControlModuleRelatedEntryPublisher, 1, 3);
            this.tableLayoutPanelImport.Controls.Add(this.panelNameGeneration, 1, 8);
            this.tableLayoutPanelImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImport.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelImport.Name = "tableLayoutPanelImport";
            this.tableLayoutPanelImport.RowCount = 12;
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.Size = new System.Drawing.Size(640, 564);
            this.tableLayoutPanelImport.TabIndex = 1;
            // 
            // panelImport
            // 
            this.panelImport.AutoScroll = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.panelImport, 7);
            this.panelImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImport.Location = new System.Drawing.Point(3, 118);
            this.panelImport.Name = "panelImport";
            this.panelImport.Size = new System.Drawing.Size(634, 288);
            this.panelImport.TabIndex = 0;
            // 
            // radioButtonImportNewProject
            // 
            this.radioButtonImportNewProject.AutoSize = true;
            this.radioButtonImportNewProject.Checked = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.radioButtonImportNewProject, 2);
            this.radioButtonImportNewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonImportNewProject.Location = new System.Drawing.Point(3, 439);
            this.radioButtonImportNewProject.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.radioButtonImportNewProject.Name = "radioButtonImportNewProject";
            this.radioButtonImportNewProject.Size = new System.Drawing.Size(135, 21);
            this.radioButtonImportNewProject.TabIndex = 1;
            this.radioButtonImportNewProject.TabStop = true;
            this.radioButtonImportNewProject.Text = "Create new project";
            this.radioButtonImportNewProject.UseVisualStyleBackColor = true;
            this.radioButtonImportNewProject.Click += new System.EventHandler(this.radioButtonImportNewProject_Click);
            // 
            // radioButtonImportIntoProject
            // 
            this.radioButtonImportIntoProject.AutoSize = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.radioButtonImportIntoProject, 2);
            this.radioButtonImportIntoProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonImportIntoProject.Location = new System.Drawing.Point(3, 412);
            this.radioButtonImportIntoProject.Name = "radioButtonImportIntoProject";
            this.radioButtonImportIntoProject.Size = new System.Drawing.Size(135, 21);
            this.radioButtonImportIntoProject.TabIndex = 2;
            this.radioButtonImportIntoProject.Text = "Insert in existing project";
            this.radioButtonImportIntoProject.UseVisualStyleBackColor = true;
            this.radioButtonImportIntoProject.Click += new System.EventHandler(this.radioButtonImportIntoProject_Click);
            // 
            // textBoxImportNewProject
            // 
            this.textBoxImportNewProject.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelImport.SetColumnSpan(this.textBoxImportNewProject, 3);
            this.textBoxImportNewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxImportNewProject.Location = new System.Drawing.Point(144, 439);
            this.textBoxImportNewProject.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxImportNewProject.Name = "textBoxImportNewProject";
            this.textBoxImportNewProject.Size = new System.Drawing.Size(286, 20);
            this.textBoxImportNewProject.TabIndex = 3;
            this.textBoxImportNewProject.TextChanged += new System.EventHandler(this.textBoxImportNewProject_TextChanged);
            // 
            // comboBoxImportIntoProject
            // 
            this.comboBoxImportIntoProject.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelImport.SetColumnSpan(this.comboBoxImportIntoProject, 3);
            this.comboBoxImportIntoProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxImportIntoProject.Enabled = false;
            this.comboBoxImportIntoProject.FormattingEnabled = true;
            this.comboBoxImportIntoProject.Location = new System.Drawing.Point(144, 412);
            this.comboBoxImportIntoProject.Name = "comboBoxImportIntoProject";
            this.comboBoxImportIntoProject.Size = new System.Drawing.Size(286, 21);
            this.comboBoxImportIntoProject.TabIndex = 4;
            this.comboBoxImportIntoProject.SelectionChangeCommitted += new System.EventHandler(this.comboBoxImportIntoProject_SelectionChangeCommitted);
            // 
            // labelParentProject
            // 
            this.labelParentProject.AutoSize = true;
            this.labelParentProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParentProject.Location = new System.Drawing.Point(436, 409);
            this.labelParentProject.Name = "labelParentProject";
            this.labelParentProject.Size = new System.Drawing.Size(148, 27);
            this.labelParentProject.TabIndex = 10;
            this.labelParentProject.Text = "Parent project:";
            this.labelParentProject.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxParentProject
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.comboBoxParentProject, 2);
            this.comboBoxParentProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxParentProject.DropDownWidth = 200;
            this.comboBoxParentProject.FormattingEnabled = true;
            this.comboBoxParentProject.Location = new System.Drawing.Point(436, 439);
            this.comboBoxParentProject.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.comboBoxParentProject.Name = "comboBoxParentProject";
            this.helpProvider.SetShowHelp(this.comboBoxParentProject, true);
            this.comboBoxParentProject.Size = new System.Drawing.Size(201, 21);
            this.comboBoxParentProject.TabIndex = 11;
            // 
            // buttonImportStart
            // 
            this.buttonImportStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImportStart.Enabled = false;
            this.buttonImportStart.Image = global::DiversityProjects.Properties.Resources.Import;
            this.buttonImportStart.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonImportStart.Location = new System.Drawing.Point(590, 486);
            this.buttonImportStart.Name = "buttonImportStart";
            this.tableLayoutPanelImport.SetRowSpan(this.buttonImportStart, 3);
            this.buttonImportStart.Size = new System.Drawing.Size(47, 75);
            this.buttonImportStart.TabIndex = 12;
            this.buttonImportStart.Text = "Start import";
            this.buttonImportStart.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonImportStart.UseVisualStyleBackColor = true;
            this.buttonImportStart.Click += new System.EventHandler(this.buttonImportStart_Click);
            // 
            // labelLicenseHolder
            // 
            this.labelLicenseHolder.AutoSize = true;
            this.labelLicenseHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLicenseHolder.Location = new System.Drawing.Point(3, 0);
            this.labelLicenseHolder.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelLicenseHolder.Name = "labelLicenseHolder";
            this.labelLicenseHolder.Size = new System.Drawing.Size(92, 28);
            this.labelLicenseHolder.TabIndex = 13;
            this.labelLicenseHolder.Text = "License - Holder:";
            this.labelLicenseHolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIPR
            // 
            this.labelIPR.AutoSize = true;
            this.labelIPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIPR.Location = new System.Drawing.Point(3, 28);
            this.labelIPR.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelIPR.Name = "labelIPR";
            this.labelIPR.Size = new System.Drawing.Size(92, 25);
            this.labelIPR.TabIndex = 15;
            this.labelIPR.Text = "License - IPR:";
            this.labelIPR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxAgentsDatabase
            // 
            this.comboBoxAgentsDatabase.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelImport.SetColumnSpan(this.comboBoxAgentsDatabase, 4);
            this.comboBoxAgentsDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAgentsDatabase.FormattingEnabled = true;
            this.comboBoxAgentsDatabase.Location = new System.Drawing.Point(144, 486);
            this.comboBoxAgentsDatabase.Name = "comboBoxAgentsDatabase";
            this.comboBoxAgentsDatabase.Size = new System.Drawing.Size(440, 21);
            this.comboBoxAgentsDatabase.TabIndex = 17;
            this.comboBoxAgentsDatabase.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAgentsDatabase_SelectionChangeCommitted);
            // 
            // labelAgentDatabase
            // 
            this.labelAgentDatabase.AutoSize = true;
            this.labelAgentDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAgentDatabase.Location = new System.Drawing.Point(98, 483);
            this.labelAgentDatabase.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAgentDatabase.Name = "labelAgentDatabase";
            this.labelAgentDatabase.Size = new System.Drawing.Size(43, 27);
            this.labelAgentDatabase.TabIndex = 18;
            this.labelAgentDatabase.Text = "Agents:";
            this.labelAgentDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAgentsProject
            // 
            this.labelAgentsProject.AutoSize = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.labelAgentsProject, 3);
            this.labelAgentsProject.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelAgentsProject.Location = new System.Drawing.Point(99, 510);
            this.labelAgentsProject.Name = "labelAgentsProject";
            this.labelAgentsProject.Size = new System.Drawing.Size(136, 27);
            this.labelAgentsProject.TabIndex = 19;
            this.labelAgentsProject.Text = "Project in agents database:";
            this.labelAgentsProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxAgentsProject
            // 
            this.comboBoxAgentsProject.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelImport.SetColumnSpan(this.comboBoxAgentsProject, 3);
            this.comboBoxAgentsProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAgentsProject.FormattingEnabled = true;
            this.comboBoxAgentsProject.Location = new System.Drawing.Point(241, 513);
            this.comboBoxAgentsProject.Name = "comboBoxAgentsProject";
            this.comboBoxAgentsProject.Size = new System.Drawing.Size(343, 21);
            this.comboBoxAgentsProject.TabIndex = 20;
            this.comboBoxAgentsProject.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAgentsProject_SelectionChangeCommitted);
            // 
            // labelSubmitter
            // 
            this.labelSubmitter.AutoSize = true;
            this.labelSubmitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSubmitter.Location = new System.Drawing.Point(3, 90);
            this.labelSubmitter.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSubmitter.Name = "labelSubmitter";
            this.labelSubmitter.Size = new System.Drawing.Size(92, 25);
            this.labelSubmitter.TabIndex = 21;
            this.labelSubmitter.Text = "Inst.address:";
            this.labelSubmitter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPublisher
            // 
            this.labelPublisher.AutoSize = true;
            this.labelPublisher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPublisher.Location = new System.Drawing.Point(3, 62);
            this.labelPublisher.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPublisher.Name = "labelPublisher";
            this.labelPublisher.Size = new System.Drawing.Size(92, 28);
            this.labelPublisher.TabIndex = 23;
            this.labelPublisher.Text = "Tech.Cont./Publ.:";
            this.labelPublisher.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxAgentRoleDefault
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.comboBoxAgentRoleDefault, 3);
            this.comboBoxAgentRoleDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAgentRoleDefault.FormattingEnabled = true;
            this.comboBoxAgentRoleDefault.Location = new System.Drawing.Point(241, 540);
            this.comboBoxAgentRoleDefault.Name = "comboBoxAgentRoleDefault";
            this.comboBoxAgentRoleDefault.Size = new System.Drawing.Size(343, 21);
            this.comboBoxAgentRoleDefault.TabIndex = 25;
            this.comboBoxAgentRoleDefault.DropDown += new System.EventHandler(this.comboBoxAgentRoleDefault_DropDown);
            // 
            // labelAgentRoleDefault
            // 
            this.labelAgentRoleDefault.AutoSize = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.labelAgentRoleDefault, 3);
            this.labelAgentRoleDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAgentRoleDefault.Location = new System.Drawing.Point(3, 537);
            this.labelAgentRoleDefault.Name = "labelAgentRoleDefault";
            this.labelAgentRoleDefault.Size = new System.Drawing.Size(232, 27);
            this.labelAgentRoleDefault.TabIndex = 26;
            this.labelAgentRoleDefault.Text = "Default for missing roles:";
            this.labelAgentRoleDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryLicenseHolder
            // 
            this.userControlModuleRelatedEntryLicenseHolder.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.userControlModuleRelatedEntryLicenseHolder, 6);
            this.userControlModuleRelatedEntryLicenseHolder.DependsOnUri = "";
            this.userControlModuleRelatedEntryLicenseHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryLicenseHolder.Domain = "";
            this.userControlModuleRelatedEntryLicenseHolder.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryLicenseHolder.Location = new System.Drawing.Point(95, 3);
            this.userControlModuleRelatedEntryLicenseHolder.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.userControlModuleRelatedEntryLicenseHolder.Module = null;
            this.userControlModuleRelatedEntryLicenseHolder.Name = "userControlModuleRelatedEntryLicenseHolder";
            this.userControlModuleRelatedEntryLicenseHolder.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryLicenseHolder.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryLicenseHolder.ShowInfo = false;
            this.userControlModuleRelatedEntryLicenseHolder.Size = new System.Drawing.Size(542, 22);
            this.userControlModuleRelatedEntryLicenseHolder.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryLicenseHolder.TabIndex = 27;
            // 
            // userControlModuleRelatedEntryIPR
            // 
            this.userControlModuleRelatedEntryIPR.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.userControlModuleRelatedEntryIPR, 6);
            this.userControlModuleRelatedEntryIPR.DependsOnUri = "";
            this.userControlModuleRelatedEntryIPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryIPR.Domain = "";
            this.userControlModuleRelatedEntryIPR.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryIPR.Location = new System.Drawing.Point(95, 28);
            this.userControlModuleRelatedEntryIPR.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.userControlModuleRelatedEntryIPR.Module = null;
            this.userControlModuleRelatedEntryIPR.Name = "userControlModuleRelatedEntryIPR";
            this.userControlModuleRelatedEntryIPR.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryIPR.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryIPR.ShowInfo = false;
            this.userControlModuleRelatedEntryIPR.Size = new System.Drawing.Size(542, 22);
            this.userControlModuleRelatedEntryIPR.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryIPR.TabIndex = 28;
            // 
            // userControlModuleRelatedEntryInstitution
            // 
            this.userControlModuleRelatedEntryInstitution.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.userControlModuleRelatedEntryInstitution, 6);
            this.userControlModuleRelatedEntryInstitution.DependsOnUri = "";
            this.userControlModuleRelatedEntryInstitution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryInstitution.Domain = "";
            this.userControlModuleRelatedEntryInstitution.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryInstitution.Location = new System.Drawing.Point(95, 90);
            this.userControlModuleRelatedEntryInstitution.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.userControlModuleRelatedEntryInstitution.Module = null;
            this.userControlModuleRelatedEntryInstitution.Name = "userControlModuleRelatedEntryInstitution";
            this.userControlModuleRelatedEntryInstitution.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryInstitution.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryInstitution.ShowInfo = false;
            this.userControlModuleRelatedEntryInstitution.Size = new System.Drawing.Size(542, 22);
            this.userControlModuleRelatedEntryInstitution.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryInstitution.TabIndex = 29;
            this.toolTip.SetToolTip(this.userControlModuleRelatedEntryInstitution, "Institution of the Technical Contact/Publisher stored as address");
            // 
            // userControlModuleRelatedEntryPublisher
            // 
            this.userControlModuleRelatedEntryPublisher.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.userControlModuleRelatedEntryPublisher, 6);
            this.userControlModuleRelatedEntryPublisher.DependsOnUri = "";
            this.userControlModuleRelatedEntryPublisher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryPublisher.Domain = "";
            this.userControlModuleRelatedEntryPublisher.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryPublisher.Location = new System.Drawing.Point(95, 65);
            this.userControlModuleRelatedEntryPublisher.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.userControlModuleRelatedEntryPublisher.Module = null;
            this.userControlModuleRelatedEntryPublisher.Name = "userControlModuleRelatedEntryPublisher";
            this.userControlModuleRelatedEntryPublisher.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryPublisher.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryPublisher.ShowInfo = false;
            this.userControlModuleRelatedEntryPublisher.Size = new System.Drawing.Size(542, 22);
            this.userControlModuleRelatedEntryPublisher.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryPublisher.TabIndex = 30;
            // 
            // panelNameGeneration
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.panelNameGeneration, 6);
            this.panelNameGeneration.Controls.Add(this.radioButtonAutogenerateName);
            this.panelNameGeneration.Controls.Add(this.radioButtonKeepKeyAsName);
            this.panelNameGeneration.Controls.Add(this.radioButtonEditProjectName);
            this.panelNameGeneration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNameGeneration.Location = new System.Drawing.Point(95, 460);
            this.panelNameGeneration.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelNameGeneration.Name = "panelNameGeneration";
            this.panelNameGeneration.Size = new System.Drawing.Size(545, 20);
            this.panelNameGeneration.TabIndex = 31;
            // 
            // radioButtonAutogenerateName
            // 
            this.radioButtonAutogenerateName.AutoSize = true;
            this.radioButtonAutogenerateName.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonAutogenerateName.Location = new System.Drawing.Point(287, 0);
            this.radioButtonAutogenerateName.Name = "radioButtonAutogenerateName";
            this.radioButtonAutogenerateName.Size = new System.Drawing.Size(173, 20);
            this.radioButtonAutogenerateName.TabIndex = 1;
            this.radioButtonAutogenerateName.Text = "Autogeneration of project name";
            this.radioButtonAutogenerateName.UseVisualStyleBackColor = true;
            this.radioButtonAutogenerateName.Click += new System.EventHandler(this.radioButtonAutogenerateName_Click);
            // 
            // radioButtonKeepKeyAsName
            // 
            this.radioButtonKeepKeyAsName.AutoSize = true;
            this.radioButtonKeepKeyAsName.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonKeepKeyAsName.Location = new System.Drawing.Point(107, 0);
            this.radioButtonKeepKeyAsName.Name = "radioButtonKeepKeyAsName";
            this.radioButtonKeepKeyAsName.Size = new System.Drawing.Size(180, 20);
            this.radioButtonKeepKeyAsName.TabIndex = 0;
            this.radioButtonKeepKeyAsName.Text = "Use key as name for new project";
            this.radioButtonKeepKeyAsName.UseVisualStyleBackColor = true;
            this.radioButtonKeepKeyAsName.Click += new System.EventHandler(this.radioButtonKeepKeyAsName_Click);
            // 
            // radioButtonEditProjectName
            // 
            this.radioButtonEditProjectName.AutoSize = true;
            this.radioButtonEditProjectName.Checked = true;
            this.radioButtonEditProjectName.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonEditProjectName.Location = new System.Drawing.Point(0, 0);
            this.radioButtonEditProjectName.Name = "radioButtonEditProjectName";
            this.radioButtonEditProjectName.Size = new System.Drawing.Size(107, 20);
            this.radioButtonEditProjectName.TabIndex = 2;
            this.radioButtonEditProjectName.TabStop = true;
            this.radioButtonEditProjectName.Text = "Edit project name";
            this.radioButtonEditProjectName.UseVisualStyleBackColor = true;
            this.radioButtonEditProjectName.Click += new System.EventHandler(this.radioButtonEditProjectName_Click);
            // 
            // tabPageGFBio
            // 
            this.tabPageGFBio.Controls.Add(this.userControlWebViewGFBio);
            this.tabPageGFBio.ImageIndex = 0;
            this.tabPageGFBio.Location = new System.Drawing.Point(4, 23);
            this.tabPageGFBio.Name = "tabPageGFBio";
            this.tabPageGFBio.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGFBio.Size = new System.Drawing.Size(646, 570);
            this.tabPageGFBio.TabIndex = 0;
            this.tabPageGFBio.Text = "GFBio";
            this.tabPageGFBio.UseVisualStyleBackColor = true;
            // 
            // imageListTabControl
            // 
            this.imageListTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabControl.ImageStream")));
            this.imageListTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabControl.Images.SetKeyName(0, "IcoGFBio.ico");
            this.imageListTabControl.Images.SetKeyName(1, "Import.ico");
            // 
            // userControlWebViewGFBio
            // 
            this.userControlWebViewGFBio.AllowScripting = false;
            this.userControlWebViewGFBio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebViewGFBio.Location = new System.Drawing.Point(3, 3);
            this.userControlWebViewGFBio.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebViewGFBio.Name = "userControlWebViewGFBio";
            this.userControlWebViewGFBio.ScriptErrorsSuppressed = false;
            this.userControlWebViewGFBio.Size = new System.Drawing.Size(640, 564);
            this.userControlWebViewGFBio.TabIndex = 1;
            this.userControlWebViewGFBio.Url = null;
            // 
            // FormGFBioImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 646);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetHelpKeyword(this, "GFBio");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this, "GFBio");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGFBioImport";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GFBio Import";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGFBioImport_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogin)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageImport.ResumeLayout(false);
            this.tableLayoutPanelImport.ResumeLayout(false);
            this.tableLayoutPanelImport.PerformLayout();
            this.panelNameGeneration.ResumeLayout(false);
            this.panelNameGeneration.PerformLayout();
            this.tabPageGFBio.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.PictureBox pictureBoxLogin;
        private System.Windows.Forms.Label labelKey;
        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGFBio;
        private System.Windows.Forms.TabPage tabPageImport;
        private System.Windows.Forms.ImageList imageListTabControl;
        private System.Windows.Forms.Label labelParentProject;
        private System.Windows.Forms.ComboBox comboBoxParentProject;
        private System.Windows.Forms.Panel panelImport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImport;
        private System.Windows.Forms.RadioButton radioButtonImportNewProject;
        private System.Windows.Forms.RadioButton radioButtonImportIntoProject;
        private System.Windows.Forms.TextBox textBoxImportNewProject;
        private System.Windows.Forms.ComboBox comboBoxImportIntoProject;
        private System.Windows.Forms.Button buttonImportStart;
        private System.Windows.Forms.Label labelLicenseHolder;
        private System.Windows.Forms.Label labelIPR;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ComboBox comboBoxAgentsDatabase;
        private System.Windows.Forms.Label labelAgentDatabase;
        private System.Windows.Forms.Label labelAgentsProject;
        private System.Windows.Forms.ComboBox comboBoxAgentsProject;
        private System.Windows.Forms.Label labelSubmitter;
        private System.Windows.Forms.Label labelPublisher;
        private System.Windows.Forms.ComboBox comboBoxAgentRoleDefault;
        private System.Windows.Forms.Label labelAgentRoleDefault;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryLicenseHolder;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIPR;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryInstitution;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryPublisher;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelNameGeneration;
        private System.Windows.Forms.RadioButton radioButtonAutogenerateName;
        private System.Windows.Forms.RadioButton radioButtonKeepKeyAsName;
        private System.Windows.Forms.RadioButton radioButtonEditProjectName;
        private DiversityWorkbench.UserControls.UserControlWebView userControlWebViewGFBio;
    }
}