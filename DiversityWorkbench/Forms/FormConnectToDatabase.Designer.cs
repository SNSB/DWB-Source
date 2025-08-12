namespace DiversityWorkbench.Forms
{
    partial class FormConnectToDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConnectToDatabase));
            comboBoxServer = new System.Windows.Forms.ComboBox();
            comboBoxDatabase = new System.Windows.Forms.ComboBox();
            textBoxPort = new System.Windows.Forms.TextBox();
            groupBoxLogin = new System.Windows.Forms.GroupBox();
            tableLayoutPanelLogin = new System.Windows.Forms.TableLayoutPanel();
            radioButtonAuthentication = new System.Windows.Forms.RadioButton();
            textBoxPassword = new System.Windows.Forms.TextBox();
            textBoxUser = new System.Windows.Forms.TextBox();
            radioButtonSQLAuthentication = new System.Windows.Forms.RadioButton();
            labelPassword = new System.Windows.Forms.Label();
            labelUser = new System.Windows.Forms.Label();
            pictureBoxWindowsLogin = new System.Windows.Forms.PictureBox();
            pictureBoxDatabaseLogin = new System.Windows.Forms.PictureBox();
            panelPwButtons = new System.Windows.Forms.Panel();
            buttonClearPassword = new System.Windows.Forms.Button();
            checkBoxShowPw = new System.Windows.Forms.CheckBox();
            imageListPassword = new System.Windows.Forms.ImageList(components);
            groupBoxServer = new System.Windows.Forms.GroupBox();
            tableLayoutPanelServer = new System.Windows.Forms.TableLayoutPanel();
            labelPort = new System.Windows.Forms.Label();
            labelServer = new System.Windows.Forms.Label();
            buttonPing = new System.Windows.Forms.Button();
            buttonConnectToServer = new System.Windows.Forms.Button();
            imageListConnectionState = new System.Windows.Forms.ImageList(components);
            labelDatabase = new System.Windows.Forms.Label();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            toolStripPreviousConnections = new System.Windows.Forms.ToolStrip();
            toolStripDropDownButtonPreviousConnections = new System.Windows.Forms.ToolStripDropDownButton();
            buttonEncryptConnection = new System.Windows.Forms.Button();
            buttonCreateDatabase = new System.Windows.Forms.Button();
            helpProvider = new System.Windows.Forms.HelpProvider();
            toolTip = new System.Windows.Forms.ToolTip(components);
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            groupBoxLogin.SuspendLayout();
            tableLayoutPanelLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxWindowsLogin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDatabaseLogin).BeginInit();
            panelPwButtons.SuspendLayout();
            groupBoxServer.SuspendLayout();
            tableLayoutPanelServer.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            toolStripPreviousConnections.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxServer
            // 
            tableLayoutPanelServer.SetColumnSpan(comboBoxServer, 2);
            comboBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxServer.FormattingEnabled = true;
            comboBoxServer.Location = new System.Drawing.Point(4, 20);
            comboBoxServer.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            comboBoxServer.Name = "comboBoxServer";
            comboBoxServer.Size = new System.Drawing.Size(250, 23);
            comboBoxServer.TabIndex = 0;
            comboBoxServer.DropDown += comboBoxServer_DropDown;
            comboBoxServer.TextChanged += comboBoxServer_TextChanged;
            // 
            // comboBoxDatabase
            // 
            tableLayoutPanelMain.SetColumnSpan(comboBoxDatabase, 3);
            comboBoxDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            comboBoxDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxDatabase.Enabled = false;
            comboBoxDatabase.Location = new System.Drawing.Point(4, 289);
            comboBoxDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            comboBoxDatabase.Name = "comboBoxDatabase";
            comboBoxDatabase.Size = new System.Drawing.Size(323, 23);
            comboBoxDatabase.TabIndex = 8;
            comboBoxDatabase.SelectedIndexChanged += comboBoxDatabase_SelectedIndexChanged;
            comboBoxDatabase.TextChanged += comboBoxDatabase_TextChanged;
            comboBoxDatabase.KeyDown += comboBoxDatabase_KeyDown;
            comboBoxDatabase.Leave += comboBoxDatabase_Leave;
            // 
            // textBoxPort
            // 
            textBoxPort.Location = new System.Drawing.Point(258, 20);
            textBoxPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.Size = new System.Drawing.Size(53, 23);
            textBoxPort.TabIndex = 1;
            textBoxPort.Text = "5432";
            textBoxPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            textBoxPort.TextChanged += textBoxPort_TextChanged;
            // 
            // groupBoxLogin
            // 
            tableLayoutPanelMain.SetColumnSpan(groupBoxLogin, 3);
            groupBoxLogin.Controls.Add(tableLayoutPanelLogin);
            groupBoxLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxLogin.Location = new System.Drawing.Point(4, 78);
            groupBoxLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLogin.Name = "groupBoxLogin";
            groupBoxLogin.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLogin.Size = new System.Drawing.Size(323, 142);
            groupBoxLogin.TabIndex = 4;
            groupBoxLogin.TabStop = false;
            groupBoxLogin.Text = "Login";
            // 
            // tableLayoutPanelLogin
            // 
            tableLayoutPanelLogin.ColumnCount = 3;
            tableLayoutPanelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelLogin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLogin.Controls.Add(radioButtonAuthentication, 0, 0);
            tableLayoutPanelLogin.Controls.Add(textBoxPassword, 1, 3);
            tableLayoutPanelLogin.Controls.Add(textBoxUser, 1, 2);
            tableLayoutPanelLogin.Controls.Add(radioButtonSQLAuthentication, 0, 1);
            tableLayoutPanelLogin.Controls.Add(labelPassword, 0, 3);
            tableLayoutPanelLogin.Controls.Add(labelUser, 0, 2);
            tableLayoutPanelLogin.Controls.Add(pictureBoxWindowsLogin, 2, 0);
            tableLayoutPanelLogin.Controls.Add(pictureBoxDatabaseLogin, 2, 1);
            tableLayoutPanelLogin.Controls.Add(panelPwButtons, 2, 3);
            tableLayoutPanelLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelLogin.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanelLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelLogin.Name = "tableLayoutPanelLogin";
            tableLayoutPanelLogin.RowCount = 4;
            tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLogin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelLogin.Size = new System.Drawing.Size(315, 120);
            tableLayoutPanelLogin.TabIndex = 24;
            // 
            // radioButtonAuthentication
            // 
            radioButtonAuthentication.Checked = true;
            tableLayoutPanelLogin.SetColumnSpan(radioButtonAuthentication, 2);
            radioButtonAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonAuthentication.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            radioButtonAuthentication.Location = new System.Drawing.Point(4, 3);
            radioButtonAuthentication.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonAuthentication.Name = "radioButtonAuthentication";
            radioButtonAuthentication.Size = new System.Drawing.Size(281, 29);
            radioButtonAuthentication.TabIndex = 3;
            radioButtonAuthentication.TabStop = true;
            radioButtonAuthentication.Text = "Windows authentication";
            radioButtonAuthentication.CheckedChanged += radioButtonAuthentication_CheckedChanged;
            // 
            // textBoxPassword
            // 
            textBoxPassword.BackColor = System.Drawing.SystemColors.ActiveBorder;
            textBoxPassword.Dock = System.Windows.Forms.DockStyle.Top;
            textBoxPassword.Enabled = false;
            textBoxPassword.Location = new System.Drawing.Point(79, 96);
            textBoxPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new System.Drawing.Size(210, 23);
            textBoxPassword.TabIndex = 6;
            textBoxPassword.Leave += textBoxPassword_Leave;
            // 
            // textBoxUser
            // 
            textBoxUser.BackColor = System.Drawing.SystemColors.ActiveBorder;
            tableLayoutPanelLogin.SetColumnSpan(textBoxUser, 2);
            textBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxUser.Enabled = false;
            textBoxUser.Location = new System.Drawing.Point(79, 66);
            textBoxUser.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            textBoxUser.Name = "textBoxUser";
            textBoxUser.Size = new System.Drawing.Size(232, 23);
            textBoxUser.TabIndex = 5;
            textBoxUser.Leave += textBoxUser_Leave;
            // 
            // radioButtonSQLAuthentication
            // 
            tableLayoutPanelLogin.SetColumnSpan(radioButtonSQLAuthentication, 2);
            radioButtonSQLAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonSQLAuthentication.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            radioButtonSQLAuthentication.Location = new System.Drawing.Point(4, 38);
            radioButtonSQLAuthentication.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            radioButtonSQLAuthentication.Name = "radioButtonSQLAuthentication";
            radioButtonSQLAuthentication.Size = new System.Drawing.Size(281, 28);
            radioButtonSQLAuthentication.TabIndex = 4;
            radioButtonSQLAuthentication.Text = "SQL-Server authentication";
            // 
            // labelPassword
            // 
            labelPassword.Dock = System.Windows.Forms.DockStyle.Top;
            labelPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelPassword.Location = new System.Drawing.Point(4, 99);
            labelPassword.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new System.Drawing.Size(75, 14);
            labelPassword.TabIndex = 23;
            labelPassword.Text = "Password:";
            labelPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelUser
            // 
            labelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            labelUser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelUser.Location = new System.Drawing.Point(4, 69);
            labelUser.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            labelUser.Name = "labelUser";
            labelUser.Size = new System.Drawing.Size(75, 27);
            labelUser.TabIndex = 22;
            labelUser.Text = "User:";
            labelUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBoxWindowsLogin
            // 
            pictureBoxWindowsLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxWindowsLogin.Image = Properties.Resources.WindowsLogin;
            pictureBoxWindowsLogin.InitialImage = Properties.Resources.Agent;
            pictureBoxWindowsLogin.Location = new System.Drawing.Point(289, 5);
            pictureBoxWindowsLogin.Margin = new System.Windows.Forms.Padding(0, 5, 7, 0);
            pictureBoxWindowsLogin.Name = "pictureBoxWindowsLogin";
            pictureBoxWindowsLogin.Size = new System.Drawing.Size(19, 30);
            pictureBoxWindowsLogin.TabIndex = 24;
            pictureBoxWindowsLogin.TabStop = false;
            // 
            // pictureBoxDatabaseLogin
            // 
            pictureBoxDatabaseLogin.Image = Properties.Resources.DatabaseLogin;
            pictureBoxDatabaseLogin.Location = new System.Drawing.Point(289, 40);
            pictureBoxDatabaseLogin.Margin = new System.Windows.Forms.Padding(0, 5, 7, 0);
            pictureBoxDatabaseLogin.Name = "pictureBoxDatabaseLogin";
            pictureBoxDatabaseLogin.Size = new System.Drawing.Size(19, 18);
            pictureBoxDatabaseLogin.TabIndex = 25;
            pictureBoxDatabaseLogin.TabStop = false;
            // 
            // panelPwButtons
            // 
            panelPwButtons.Controls.Add(buttonClearPassword);
            panelPwButtons.Controls.Add(checkBoxShowPw);
            panelPwButtons.Dock = System.Windows.Forms.DockStyle.Left;
            panelPwButtons.Location = new System.Drawing.Point(289, 96);
            panelPwButtons.Margin = new System.Windows.Forms.Padding(0);
            panelPwButtons.Name = "panelPwButtons";
            panelPwButtons.Size = new System.Drawing.Size(26, 24);
            panelPwButtons.TabIndex = 28;
            // 
            // buttonClearPassword
            // 
            buttonClearPassword.Dock = System.Windows.Forms.DockStyle.Left;
            buttonClearPassword.Enabled = false;
            buttonClearPassword.FlatAppearance.BorderSize = 0;
            buttonClearPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonClearPassword.Image = Properties.Resources.Delete;
            buttonClearPassword.Location = new System.Drawing.Point(22, 0);
            buttonClearPassword.Margin = new System.Windows.Forms.Padding(0);
            buttonClearPassword.Name = "buttonClearPassword";
            buttonClearPassword.Size = new System.Drawing.Size(26, 24);
            buttonClearPassword.TabIndex = 26;
            toolTip.SetToolTip(buttonClearPassword, "Remove the password from the settings");
            buttonClearPassword.UseVisualStyleBackColor = true;
            buttonClearPassword.Visible = false;
            buttonClearPassword.Click += buttonClearPassword_Click;
            // 
            // checkBoxShowPw
            // 
            checkBoxShowPw.Appearance = System.Windows.Forms.Appearance.Button;
            checkBoxShowPw.AutoSize = true;
            checkBoxShowPw.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxShowPw.FlatAppearance.BorderSize = 0;
            checkBoxShowPw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxShowPw.ImageKey = "Visible.ico";
            checkBoxShowPw.ImageList = imageListPassword;
            checkBoxShowPw.Location = new System.Drawing.Point(0, 0);
            checkBoxShowPw.Margin = new System.Windows.Forms.Padding(0);
            checkBoxShowPw.Name = "checkBoxShowPw";
            checkBoxShowPw.Size = new System.Drawing.Size(22, 24);
            checkBoxShowPw.TabIndex = 27;
            toolTip.SetToolTip(checkBoxShowPw, "Show password");
            checkBoxShowPw.UseVisualStyleBackColor = true;
            checkBoxShowPw.CheckedChanged += checkBoxShowPw_CheckedChanged;
            // 
            // imageListPassword
            // 
            imageListPassword.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListPassword.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListPassword.ImageStream");
            imageListPassword.TransparentColor = System.Drawing.Color.Transparent;
            imageListPassword.Images.SetKeyName(0, "Delete.ico");
            imageListPassword.Images.SetKeyName(1, "Save.ico");
            imageListPassword.Images.SetKeyName(2, "Visible.ico");
            imageListPassword.Images.SetKeyName(3, "Hidden.ico");
            // 
            // groupBoxServer
            // 
            tableLayoutPanelMain.SetColumnSpan(groupBoxServer, 3);
            groupBoxServer.Controls.Add(tableLayoutPanelServer);
            groupBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxServer.Location = new System.Drawing.Point(4, 3);
            groupBoxServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxServer.Name = "groupBoxServer";
            groupBoxServer.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxServer.Size = new System.Drawing.Size(323, 69);
            groupBoxServer.TabIndex = 0;
            groupBoxServer.TabStop = false;
            groupBoxServer.Text = "Server";
            // 
            // tableLayoutPanelServer
            // 
            tableLayoutPanelServer.ColumnCount = 3;
            tableLayoutPanelServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelServer.Controls.Add(labelPort, 2, 0);
            tableLayoutPanelServer.Controls.Add(textBoxPort, 2, 1);
            tableLayoutPanelServer.Controls.Add(comboBoxServer, 0, 1);
            tableLayoutPanelServer.Controls.Add(labelServer, 0, 0);
            tableLayoutPanelServer.Controls.Add(buttonPing, 1, 0);
            tableLayoutPanelServer.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelServer.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanelServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelServer.Name = "tableLayoutPanelServer";
            tableLayoutPanelServer.RowCount = 2;
            tableLayoutPanelServer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelServer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelServer.Size = new System.Drawing.Size(315, 47);
            tableLayoutPanelServer.TabIndex = 0;
            // 
            // labelPort
            // 
            labelPort.AutoSize = true;
            labelPort.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelPort.Location = new System.Drawing.Point(258, 0);
            labelPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPort.Name = "labelPort";
            labelPort.Size = new System.Drawing.Size(53, 17);
            labelPort.TabIndex = 7;
            labelPort.Text = "Port";
            labelPort.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelServer
            // 
            labelServer.AutoSize = true;
            labelServer.Dock = System.Windows.Forms.DockStyle.Fill;
            labelServer.Location = new System.Drawing.Point(4, 0);
            labelServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelServer.Name = "labelServer";
            labelServer.Size = new System.Drawing.Size(231, 17);
            labelServer.TabIndex = 8;
            labelServer.Text = "Name or IP-adress of the server";
            // 
            // buttonPing
            // 
            buttonPing.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonPing.FlatAppearance.BorderSize = 0;
            buttonPing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonPing.Image = Properties.Resources.Radiobutton;
            buttonPing.Location = new System.Drawing.Point(239, 0);
            buttonPing.Margin = new System.Windows.Forms.Padding(0);
            buttonPing.Name = "buttonPing";
            buttonPing.Size = new System.Drawing.Size(15, 17);
            buttonPing.TabIndex = 0;
            toolTip.SetToolTip(buttonPing, "Send a ping to check the connection");
            buttonPing.UseVisualStyleBackColor = true;
            buttonPing.Visible = false;
            buttonPing.Click += buttonPing_Click;
            // 
            // buttonConnectToServer
            // 
            buttonConnectToServer.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonConnectToServer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonConnectToServer.ImageKey = "NETHOOD.ICO";
            buttonConnectToServer.ImageList = imageListConnectionState;
            buttonConnectToServer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonConnectToServer.Location = new System.Drawing.Point(35, 223);
            buttonConnectToServer.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            buttonConnectToServer.Name = "buttonConnectToServer";
            buttonConnectToServer.Size = new System.Drawing.Size(261, 33);
            buttonConnectToServer.TabIndex = 7;
            buttonConnectToServer.Text = "Connect to server";
            buttonConnectToServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonConnectToServer.UseVisualStyleBackColor = true;
            buttonConnectToServer.Click += buttonConnectToServer_Click;
            // 
            // imageListConnectionState
            // 
            imageListConnectionState.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListConnectionState.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListConnectionState.ImageStream");
            imageListConnectionState.TransparentColor = System.Drawing.Color.Transparent;
            imageListConnectionState.Images.SetKeyName(0, "Database.ico");
            imageListConnectionState.Images.SetKeyName(1, "NoDatabase.ico");
            imageListConnectionState.Images.SetKeyName(2, "NETHOOD.ICO");
            // 
            // labelDatabase
            // 
            tableLayoutPanelMain.SetColumnSpan(labelDatabase, 2);
            labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDatabase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelDatabase.Location = new System.Drawing.Point(27, 256);
            labelDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDatabase.Name = "labelDatabase";
            labelDatabase.Size = new System.Drawing.Size(300, 33);
            labelDatabase.TabIndex = 8;
            labelDatabase.Text = "Choose database:";
            labelDatabase.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 3;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.Controls.Add(groupBoxServer, 0, 1);
            tableLayoutPanelMain.Controls.Add(groupBoxLogin, 0, 2);
            tableLayoutPanelMain.Controls.Add(comboBoxDatabase, 0, 5);
            tableLayoutPanelMain.Controls.Add(buttonConnectToServer, 1, 3);
            tableLayoutPanelMain.Controls.Add(labelDatabase, 1, 4);
            tableLayoutPanelMain.Controls.Add(toolStripPreviousConnections, 1, 0);
            tableLayoutPanelMain.Controls.Add(buttonEncryptConnection, 2, 3);
            tableLayoutPanelMain.Controls.Add(buttonCreateDatabase, 0, 3);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 6;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.Size = new System.Drawing.Size(331, 327);
            tableLayoutPanelMain.TabIndex = 9;
            // 
            // toolStripPreviousConnections
            // 
            toolStripPreviousConnections.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripPreviousConnections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripDropDownButtonPreviousConnections });
            toolStripPreviousConnections.Location = new System.Drawing.Point(23, 0);
            toolStripPreviousConnections.Name = "toolStripPreviousConnections";
            toolStripPreviousConnections.Size = new System.Drawing.Size(285, 29);
            toolStripPreviousConnections.TabIndex = 9;
            toolStripPreviousConnections.Text = "toolStrip1";
            toolStripPreviousConnections.Visible = false;
            // 
            // toolStripDropDownButtonPreviousConnections
            // 
            toolStripDropDownButtonPreviousConnections.Image = Properties.Resources.Connection;
            toolStripDropDownButtonPreviousConnections.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButtonPreviousConnections.Name = "toolStripDropDownButtonPreviousConnections";
            toolStripDropDownButtonPreviousConnections.Size = new System.Drawing.Size(149, 26);
            toolStripDropDownButtonPreviousConnections.Text = "Previous connections";
            // 
            // buttonEncryptConnection
            // 
            buttonEncryptConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonEncryptConnection.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            buttonEncryptConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonEncryptConnection.ForeColor = System.Drawing.Color.Red;
            buttonEncryptConnection.Image = Properties.Resources.KeyGray;
            buttonEncryptConnection.Location = new System.Drawing.Point(308, 223);
            buttonEncryptConnection.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonEncryptConnection.Name = "buttonEncryptConnection";
            buttonEncryptConnection.Size = new System.Drawing.Size(19, 33);
            buttonEncryptConnection.TabIndex = 10;
            buttonEncryptConnection.TabStop = false;
            toolTip.SetToolTip(buttonEncryptConnection, "The connection is encrypted");
            buttonEncryptConnection.UseVisualStyleBackColor = true;
            buttonEncryptConnection.Click += buttonEncryptConnection_Click;
            // 
            // buttonCreateDatabase
            // 
            buttonCreateDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonCreateDatabase.FlatAppearance.BorderSize = 0;
            buttonCreateDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonCreateDatabase.Image = Properties.Resources.DatabaseAdd;
            buttonCreateDatabase.Location = new System.Drawing.Point(0, 223);
            buttonCreateDatabase.Margin = new System.Windows.Forms.Padding(0);
            buttonCreateDatabase.Name = "buttonCreateDatabase";
            buttonCreateDatabase.Size = new System.Drawing.Size(23, 33);
            buttonCreateDatabase.TabIndex = 11;
            toolTip.SetToolTip(buttonCreateDatabase, "Create a new database");
            buttonCreateDatabase.UseVisualStyleBackColor = true;
            buttonCreateDatabase.Visible = false;
            buttonCreateDatabase.Click += buttonCreateDatabase_Click;
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 327);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(331, 31);
            userControlDialogPanel.TabIndex = 9;
            // 
            // FormConnectToDatabase
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(331, 358);
            Controls.Add(tableLayoutPanelMain);
            Controls.Add(userControlDialogPanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormConnectToDatabase";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Connect to database";
            FormClosing += FormConnectToDatabase_FormClosing;
            groupBoxLogin.ResumeLayout(false);
            tableLayoutPanelLogin.ResumeLayout(false);
            tableLayoutPanelLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxWindowsLogin).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDatabaseLogin).EndInit();
            panelPwButtons.ResumeLayout(false);
            panelPwButtons.PerformLayout();
            groupBoxServer.ResumeLayout(false);
            tableLayoutPanelServer.ResumeLayout(false);
            tableLayoutPanelServer.PerformLayout();
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            toolStripPreviousConnections.ResumeLayout(false);
            toolStripPreviousConnections.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogin;
        private System.Windows.Forms.RadioButton radioButtonAuthentication;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.RadioButton radioButtonSQLAuthentication;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelServer;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelServer;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Button buttonConnectToServer;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ImageList imageListConnectionState;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.PictureBox pictureBoxWindowsLogin;
        private System.Windows.Forms.PictureBox pictureBoxDatabaseLogin;
        private System.Windows.Forms.ToolStrip toolStripPreviousConnections;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonPreviousConnections;
        private System.Windows.Forms.Button buttonClearPassword;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageListPassword;
        private System.Windows.Forms.Button buttonEncryptConnection;
        private System.Windows.Forms.Button buttonCreateDatabase;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.CheckBox checkBoxShowPw;
        private System.Windows.Forms.Panel panelPwButtons;
    }
}