namespace DiversityWorkbench.Forms
{
    partial class FormCreateLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateLogin));
            this.tableLayoutPanelLogin = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelLogin = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.textBoxPassword1 = new System.Windows.Forms.TextBox();
            this.textBoxPassword2 = new System.Windows.Forms.TextBox();
            this.comboBoxUserName = new System.Windows.Forms.ComboBox();
            this.radioButtonWindowsAuthentication = new System.Windows.Forms.RadioButton();
            this.radioButtonSqlServerAuthentication = new System.Windows.Forms.RadioButton();
            this.labelTitle = new System.Windows.Forms.Label();
            this.comboBoxTitle = new System.Windows.Forms.ComboBox();
            this.labelGivenName = new System.Windows.Forms.Label();
            this.textBoxGivenName = new System.Windows.Forms.TextBox();
            this.labelInheritedName = new System.Windows.Forms.Label();
            this.textBoxInheritedName = new System.Windows.Forms.TextBox();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelCity = new System.Windows.Forms.Label();
            this.textBoxCity = new System.Windows.Forms.TextBox();
            this.comboBoxCountry = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxDiversityAgents = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDiversityAgents = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonAgentFromDatabase = new System.Windows.Forms.RadioButton();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.radioButtonNewAgent = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanelGetAgent = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryAgentFromDatabase = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelLogin.SuspendLayout();
            this.groupBoxDiversityAgents.SuspendLayout();
            this.tableLayoutPanelDiversityAgents.SuspendLayout();
            this.tableLayoutPanelGetAgent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelLogin
            // 
            this.tableLayoutPanelLogin.ColumnCount = 2;
            this.tableLayoutPanelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogin.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelLogin.Controls.Add(this.labelUserName, 0, 2);
            this.tableLayoutPanelLogin.Controls.Add(this.labelLogin, 0, 3);
            this.tableLayoutPanelLogin.Controls.Add(this.labelPassword, 0, 4);
            this.tableLayoutPanelLogin.Controls.Add(this.textBoxLogin, 1, 3);
            this.tableLayoutPanelLogin.Controls.Add(this.textBoxPassword1, 1, 4);
            this.tableLayoutPanelLogin.Controls.Add(this.textBoxPassword2, 1, 5);
            this.tableLayoutPanelLogin.Controls.Add(this.comboBoxUserName, 1, 2);
            this.tableLayoutPanelLogin.Controls.Add(this.radioButtonWindowsAuthentication, 0, 1);
            this.tableLayoutPanelLogin.Controls.Add(this.radioButtonSqlServerAuthentication, 1, 1);
            this.tableLayoutPanelLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLogin.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLogin.Name = "tableLayoutPanelLogin";
            this.tableLayoutPanelLogin.RowCount = 6;
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLogin.Size = new System.Drawing.Size(367, 165);
            this.tableLayoutPanelLogin.TabIndex = 2;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelLogin.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(361, 43);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Create a new login";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUserName.Enabled = false;
            this.labelUserName.Location = new System.Drawing.Point(3, 66);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(78, 27);
            this.labelUserName.TabIndex = 1;
            this.labelUserName.Text = "User name:";
            this.labelUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLogin.Location = new System.Drawing.Point(3, 93);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(78, 26);
            this.labelLogin.TabIndex = 2;
            this.labelLogin.Text = "Login:";
            this.labelLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPassword.Location = new System.Drawing.Point(3, 119);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(78, 23);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogin.Location = new System.Drawing.Point(87, 96);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(277, 20);
            this.textBoxLogin.TabIndex = 5;
            this.textBoxLogin.TextChanged += new System.EventHandler(this.textBoxLogin_TextChanged);
            // 
            // textBoxPassword1
            // 
            this.textBoxPassword1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword1.Location = new System.Drawing.Point(87, 122);
            this.textBoxPassword1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxPassword1.Name = "textBoxPassword1";
            this.textBoxPassword1.PasswordChar = '*';
            this.textBoxPassword1.Size = new System.Drawing.Size(277, 20);
            this.textBoxPassword1.TabIndex = 6;
            this.textBoxPassword1.TextChanged += new System.EventHandler(this.textBoxPassword1_TextChanged);
            // 
            // textBoxPassword2
            // 
            this.textBoxPassword2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword2.Location = new System.Drawing.Point(87, 142);
            this.textBoxPassword2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxPassword2.Name = "textBoxPassword2";
            this.textBoxPassword2.PasswordChar = '*';
            this.textBoxPassword2.Size = new System.Drawing.Size(277, 20);
            this.textBoxPassword2.TabIndex = 7;
            this.textBoxPassword2.TextChanged += new System.EventHandler(this.textBoxPassword2_TextChanged);
            // 
            // comboBoxUserName
            // 
            this.comboBoxUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxUserName.Enabled = false;
            this.comboBoxUserName.FormattingEnabled = true;
            this.comboBoxUserName.Location = new System.Drawing.Point(87, 69);
            this.comboBoxUserName.Name = "comboBoxUserName";
            this.comboBoxUserName.Size = new System.Drawing.Size(277, 21);
            this.comboBoxUserName.TabIndex = 10;
            this.comboBoxUserName.TextChanged += new System.EventHandler(this.comboBoxUserName_TextChanged);
            // 
            // radioButtonWindowsAuthentication
            // 
            this.radioButtonWindowsAuthentication.AutoSize = true;
            this.radioButtonWindowsAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonWindowsAuthentication.Enabled = false;
            this.radioButtonWindowsAuthentication.Location = new System.Drawing.Point(3, 46);
            this.radioButtonWindowsAuthentication.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.radioButtonWindowsAuthentication.Name = "radioButtonWindowsAuthentication";
            this.radioButtonWindowsAuthentication.Size = new System.Drawing.Size(81, 17);
            this.radioButtonWindowsAuthentication.TabIndex = 12;
            this.radioButtonWindowsAuthentication.Text = "Windows or";
            this.radioButtonWindowsAuthentication.UseVisualStyleBackColor = true;
            this.radioButtonWindowsAuthentication.CheckedChanged += new System.EventHandler(this.radioButtonWindowsAuthentication_CheckedChanged);
            // 
            // radioButtonSqlServerAuthentication
            // 
            this.radioButtonSqlServerAuthentication.AutoSize = true;
            this.radioButtonSqlServerAuthentication.Checked = true;
            this.radioButtonSqlServerAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonSqlServerAuthentication.Location = new System.Drawing.Point(87, 46);
            this.radioButtonSqlServerAuthentication.Name = "radioButtonSqlServerAuthentication";
            this.radioButtonSqlServerAuthentication.Size = new System.Drawing.Size(277, 17);
            this.radioButtonSqlServerAuthentication.TabIndex = 13;
            this.radioButtonSqlServerAuthentication.TabStop = true;
            this.radioButtonSqlServerAuthentication.Text = "SQL-Server authentication";
            this.radioButtonSqlServerAuthentication.UseVisualStyleBackColor = true;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Location = new System.Drawing.Point(65, 27);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(43, 13);
            this.labelTitle.TabIndex = 14;
            this.labelTitle.Text = "Title:";
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.Location = new System.Drawing.Point(65, 43);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(43, 21);
            this.comboBoxTitle.TabIndex = 15;
            // 
            // labelGivenName
            // 
            this.labelGivenName.AutoSize = true;
            this.labelGivenName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGivenName.Location = new System.Drawing.Point(114, 27);
            this.labelGivenName.Name = "labelGivenName";
            this.labelGivenName.Size = new System.Drawing.Size(94, 13);
            this.labelGivenName.TabIndex = 16;
            this.labelGivenName.Text = "Given name:";
            // 
            // textBoxGivenName
            // 
            this.textBoxGivenName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGivenName.Location = new System.Drawing.Point(114, 43);
            this.textBoxGivenName.Name = "textBoxGivenName";
            this.textBoxGivenName.Size = new System.Drawing.Size(94, 20);
            this.textBoxGivenName.TabIndex = 17;
            // 
            // labelInheritedName
            // 
            this.labelInheritedName.AutoSize = true;
            this.labelInheritedName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInheritedName.Location = new System.Drawing.Point(214, 27);
            this.labelInheritedName.Name = "labelInheritedName";
            this.labelInheritedName.Size = new System.Drawing.Size(144, 13);
            this.labelInheritedName.TabIndex = 18;
            this.labelInheritedName.Text = "Inh. name";
            // 
            // textBoxInheritedName
            // 
            this.textBoxInheritedName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInheritedName.Location = new System.Drawing.Point(214, 43);
            this.textBoxInheritedName.Name = "textBoxInheritedName";
            this.textBoxInheritedName.Size = new System.Drawing.Size(144, 20);
            this.textBoxInheritedName.TabIndex = 19;
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountry.Location = new System.Drawing.Point(65, 67);
            this.labelCountry.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(46, 27);
            this.labelCountry.TabIndex = 20;
            this.labelCountry.Text = "Country:";
            this.labelCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCity.Location = new System.Drawing.Point(65, 94);
            this.labelCity.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(46, 13);
            this.labelCity.TabIndex = 21;
            this.labelCity.Text = "City:";
            this.labelCity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCity
            // 
            this.tableLayoutPanelDiversityAgents.SetColumnSpan(this.textBoxCity, 2);
            this.textBoxCity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCity.Location = new System.Drawing.Point(114, 97);
            this.textBoxCity.Name = "textBoxCity";
            this.textBoxCity.Size = new System.Drawing.Size(244, 20);
            this.textBoxCity.TabIndex = 22;
            // 
            // comboBoxCountry
            // 
            this.tableLayoutPanelDiversityAgents.SetColumnSpan(this.comboBoxCountry, 2);
            this.comboBoxCountry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCountry.FormattingEnabled = true;
            this.comboBoxCountry.Location = new System.Drawing.Point(114, 70);
            this.comboBoxCountry.Name = "comboBoxCountry";
            this.comboBoxCountry.Size = new System.Drawing.Size(244, 21);
            this.comboBoxCountry.TabIndex = 23;
            // 
            // groupBoxDiversityAgents
            // 
            this.groupBoxDiversityAgents.Controls.Add(this.tableLayoutPanelDiversityAgents);
            this.groupBoxDiversityAgents.Controls.Add(this.tableLayoutPanelGetAgent);
            this.groupBoxDiversityAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDiversityAgents.Location = new System.Drawing.Point(0, 165);
            this.groupBoxDiversityAgents.Name = "groupBoxDiversityAgents";
            this.groupBoxDiversityAgents.Size = new System.Drawing.Size(367, 48);
            this.groupBoxDiversityAgents.TabIndex = 4;
            this.groupBoxDiversityAgents.TabStop = false;
            this.groupBoxDiversityAgents.Text = "Informations about the user as stored in DiversityAgents";
            // 
            // tableLayoutPanelDiversityAgents
            // 
            this.tableLayoutPanelDiversityAgents.ColumnCount = 4;
            this.tableLayoutPanelDiversityAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDiversityAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDiversityAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelDiversityAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.radioButtonAgentFromDatabase, 0, 6);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.labelDatabase, 0, 0);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.labelTitle, 1, 1);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.comboBoxTitle, 1, 2);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.labelGivenName, 2, 1);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.textBoxGivenName, 2, 2);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.labelInheritedName, 3, 1);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.textBoxInheritedName, 3, 2);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.labelCountry, 1, 3);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.labelCity, 1, 4);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.comboBoxDatabase, 1, 0);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.comboBoxCountry, 2, 3);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.textBoxCity, 2, 4);
            this.tableLayoutPanelDiversityAgents.Controls.Add(this.radioButtonNewAgent, 0, 2);
            this.tableLayoutPanelDiversityAgents.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelDiversityAgents.Location = new System.Drawing.Point(3, 45);
            this.tableLayoutPanelDiversityAgents.Name = "tableLayoutPanelDiversityAgents";
            this.tableLayoutPanelDiversityAgents.RowCount = 7;
            this.tableLayoutPanelDiversityAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDiversityAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDiversityAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDiversityAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDiversityAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDiversityAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDiversityAgents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDiversityAgents.Size = new System.Drawing.Size(361, 157);
            this.tableLayoutPanelDiversityAgents.TabIndex = 0;
            this.tableLayoutPanelDiversityAgents.Visible = false;
            // 
            // radioButtonAgentFromDatabase
            // 
            this.radioButtonAgentFromDatabase.AutoSize = true;
            this.tableLayoutPanelDiversityAgents.SetColumnSpan(this.radioButtonAgentFromDatabase, 2);
            this.radioButtonAgentFromDatabase.Location = new System.Drawing.Point(3, 123);
            this.radioButtonAgentFromDatabase.Name = "radioButtonAgentFromDatabase";
            this.radioButtonAgentFromDatabase.Size = new System.Drawing.Size(98, 17);
            this.radioButtonAgentFromDatabase.TabIndex = 25;
            this.radioButtonAgentFromDatabase.TabStop = true;
            this.radioButtonAgentFromDatabase.Text = "From database:";
            this.radioButtonAgentFromDatabase.UseVisualStyleBackColor = true;
            this.radioButtonAgentFromDatabase.CheckedChanged += new System.EventHandler(this.radioButtonInstution_CheckedChanged);
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.Location = new System.Drawing.Point(3, 0);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(56, 27);
            this.labelDatabase.TabIndex = 0;
            this.labelDatabase.Text = "Database:";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDatabase
            // 
            this.tableLayoutPanelDiversityAgents.SetColumnSpan(this.comboBoxDatabase, 3);
            this.comboBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Location = new System.Drawing.Point(65, 3);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(293, 21);
            this.comboBoxDatabase.TabIndex = 1;
            this.comboBoxDatabase.SelectedIndexChanged += new System.EventHandler(this.comboBoxDatabase_SelectedIndexChanged);
            this.comboBoxDatabase.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDatabase_SelectionChangeCommitted);
            // 
            // radioButtonNewAgent
            // 
            this.radioButtonNewAgent.AutoSize = true;
            this.radioButtonNewAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonNewAgent.Location = new System.Drawing.Point(3, 43);
            this.radioButtonNewAgent.Name = "radioButtonNewAgent";
            this.radioButtonNewAgent.Size = new System.Drawing.Size(56, 21);
            this.radioButtonNewAgent.TabIndex = 24;
            this.radioButtonNewAgent.TabStop = true;
            this.radioButtonNewAgent.Text = "New:";
            this.radioButtonNewAgent.UseVisualStyleBackColor = true;
            this.radioButtonNewAgent.CheckedChanged += new System.EventHandler(this.radioButtonAddress_CheckedChanged);
            // 
            // tableLayoutPanelGetAgent
            // 
            this.tableLayoutPanelGetAgent.ColumnCount = 2;
            this.tableLayoutPanelGetAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGetAgent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGetAgent.Controls.Add(this.userControlModuleRelatedEntryAgentFromDatabase, 1, 0);
            this.tableLayoutPanelGetAgent.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelGetAgent.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelGetAgent.Name = "tableLayoutPanelGetAgent";
            this.tableLayoutPanelGetAgent.RowCount = 1;
            this.tableLayoutPanelGetAgent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGetAgent.Size = new System.Drawing.Size(361, 29);
            this.tableLayoutPanelGetAgent.TabIndex = 1;
            // 
            // userControlModuleRelatedEntryAgentFromDatabase
            // 
            this.userControlModuleRelatedEntryAgentFromDatabase.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryAgentFromDatabase.DependsOnUri = "";
            this.userControlModuleRelatedEntryAgentFromDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryAgentFromDatabase.Domain = "";
            this.userControlModuleRelatedEntryAgentFromDatabase.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryAgentFromDatabase.Location = new System.Drawing.Point(3, 3);
            this.userControlModuleRelatedEntryAgentFromDatabase.Module = null;
            this.userControlModuleRelatedEntryAgentFromDatabase.Name = "userControlModuleRelatedEntryAgentFromDatabase";
            this.userControlModuleRelatedEntryAgentFromDatabase.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryAgentFromDatabase.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryAgentFromDatabase.ShowInfo = false;
            this.userControlModuleRelatedEntryAgentFromDatabase.Size = new System.Drawing.Size(355, 23);
            this.userControlModuleRelatedEntryAgentFromDatabase.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryAgentFromDatabase.TabIndex = 26;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 213);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(367, 27);
            this.userControlDialogPanel.TabIndex = 3;
            // 
            // FormCreateLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 240);
            this.Controls.Add(this.groupBoxDiversityAgents);
            this.Controls.Add(this.userControlDialogPanel);
            this.Controls.Add(this.tableLayoutPanelLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCreateLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create new login";
            this.tableLayoutPanelLogin.ResumeLayout(false);
            this.tableLayoutPanelLogin.PerformLayout();
            this.groupBoxDiversityAgents.ResumeLayout(false);
            this.tableLayoutPanelDiversityAgents.ResumeLayout(false);
            this.tableLayoutPanelDiversityAgents.PerformLayout();
            this.tableLayoutPanelGetAgent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogin;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.TextBox textBoxPassword1;
        private System.Windows.Forms.TextBox textBoxPassword2;
        private System.Windows.Forms.ComboBox comboBoxUserName;
        private System.Windows.Forms.RadioButton radioButtonWindowsAuthentication;
        private System.Windows.Forms.RadioButton radioButtonSqlServerAuthentication;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ComboBox comboBoxTitle;
        private System.Windows.Forms.Label labelGivenName;
        private System.Windows.Forms.TextBox textBoxGivenName;
        private System.Windows.Forms.Label labelInheritedName;
        private System.Windows.Forms.TextBox textBoxInheritedName;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.TextBox textBoxCity;
        private System.Windows.Forms.ComboBox comboBoxCountry;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDiversityAgents;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.RadioButton radioButtonNewAgent;
        private System.Windows.Forms.RadioButton radioButtonAgentFromDatabase;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryAgentFromDatabase;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.GroupBox groupBoxDiversityAgents;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGetAgent;
    }
}