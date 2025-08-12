namespace DiversityWorkbench.PostgreSQL
{
    partial class FormConnect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConnect));
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxRole = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelRole = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelServer = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.imageListConnectionState = new System.Windows.Forms.ImageList(this.components);
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.textBoxDatabase = new System.Windows.Forms.TextBox();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.toolStripConnections = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButtonRemoveConnection = new System.Windows.Forms.ToolStripButton();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.imageListPgObjects = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripConnections.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxPassword, 2);
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword.Enabled = false;
            this.textBoxPassword.Location = new System.Drawing.Point(67, 132);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(214, 20);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // textBoxRole
            // 
            this.textBoxRole.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxRole, 2);
            this.textBoxRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRole.Enabled = false;
            this.textBoxRole.Location = new System.Drawing.Point(67, 106);
            this.textBoxRole.Name = "textBoxRole";
            this.textBoxRole.Size = new System.Drawing.Size(214, 20);
            this.textBoxRole.TabIndex = 2;
            this.textBoxRole.TextChanged += new System.EventHandler(this.textBoxRole_TextChanged);
            // 
            // labelPassword
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelPassword, 2);
            this.labelPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelPassword.Location = new System.Drawing.Point(0, 129);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(64, 26);
            this.labelPassword.TabIndex = 23;
            this.labelPassword.Text = "Password:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRole
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelRole, 2);
            this.labelRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRole.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelRole.Location = new System.Drawing.Point(0, 103);
            this.labelRole.Margin = new System.Windows.Forms.Padding(0);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(64, 26);
            this.labelRole.TabIndex = 22;
            this.labelRole.Text = "Role:";
            this.labelRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelPassword, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxPassword, 2, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxRole, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelRole, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelServer, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonConnect, 2, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxServer, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxDatabase, 2, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatabase, 0, 7);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripConnections, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelPort, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxPort, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxDatabase, 2, 7);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOK, 3, 7);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 8;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(284, 220);
            this.tableLayoutPanelMain.TabIndex = 13;
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelServer, 3);
            this.labelServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelServer.Location = new System.Drawing.Point(3, 25);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(226, 52);
            this.labelServer.TabIndex = 8;
            this.labelServer.Text = "Name or IP-adress of the server";
            this.labelServer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonConnect.ImageKey = "NETHOOD.ICO";
            this.buttonConnect.ImageList = this.imageListConnectionState;
            this.buttonConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonConnect.Location = new System.Drawing.Point(94, 165);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(30, 10, 30, 0);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(108, 28);
            this.buttonConnect.TabIndex = 7;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // imageListConnectionState
            // 
            this.imageListConnectionState.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListConnectionState.ImageStream")));
            this.imageListConnectionState.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListConnectionState.Images.SetKeyName(0, "Database.ico");
            this.imageListConnectionState.Images.SetKeyName(1, "NoDatabase.ico");
            this.imageListConnectionState.Images.SetKeyName(2, "NETHOOD.ICO");
            // 
            // textBoxServer
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxServer, 3);
            this.textBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxServer.Location = new System.Drawing.Point(3, 80);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(226, 20);
            this.textBoxServer.TabIndex = 10;
            this.textBoxServer.TextChanged += new System.EventHandler(this.textBoxServer_TextChanged);
            // 
            // textBoxDatabase
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxDatabase, 2);
            this.textBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatabase.Location = new System.Drawing.Point(67, 106);
            this.textBoxDatabase.Name = "textBoxDatabase";
            this.textBoxDatabase.Size = new System.Drawing.Size(214, 20);
            this.textBoxDatabase.TabIndex = 11;
            this.textBoxDatabase.TextChanged += new System.EventHandler(this.textBoxDatabase_TextChanged);
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDatabase, 2);
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDatabase.Location = new System.Drawing.Point(0, 193);
            this.labelDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(64, 27);
            this.labelDatabase.TabIndex = 8;
            this.labelDatabase.Text = "Database:";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripConnections
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.toolStripConnections, 4);
            this.toolStripConnections.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripConnections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton,
            this.toolStripButtonRemoveConnection});
            this.toolStripConnections.Location = new System.Drawing.Point(0, 0);
            this.toolStripConnections.Name = "toolStripConnections";
            this.toolStripConnections.Size = new System.Drawing.Size(284, 25);
            this.toolStripConnections.TabIndex = 24;
            this.toolStripConnections.Text = "toolStrip1";
            // 
            // toolStripDropDownButton
            // 
            this.toolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton.Name = "toolStripDropDownButton";
            this.toolStripDropDownButton.Size = new System.Drawing.Size(133, 22);
            this.toolStripDropDownButton.Text = "Previous connections";
            // 
            // toolStripButtonRemoveConnection
            // 
            this.toolStripButtonRemoveConnection.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonRemoveConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveConnection.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonRemoveConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveConnection.Name = "toolStripButtonRemoveConnection";
            this.toolStripButtonRemoveConnection.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveConnection.Text = "Remove the current connection";
            this.toolStripButtonRemoveConnection.Click += new System.EventHandler(this.toolStripButtonRemoveConnection_Click);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelPort.Location = new System.Drawing.Point(235, 25);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(46, 52);
            this.labelPort.TabIndex = 7;
            this.labelPort.Text = "Port";
            this.labelPort.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(235, 80);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(46, 20);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.Text = "5432";
            this.textBoxPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxPort.TextChanged += new System.EventHandler(this.textBoxPort_TextChanged);
            this.textBoxPort.Leave += new System.EventHandler(this.textBoxPort_Leave);
            // 
            // comboBoxDatabase
            // 
            this.comboBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Location = new System.Drawing.Point(67, 196);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(162, 21);
            this.comboBoxDatabase.TabIndex = 25;
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Location = new System.Drawing.Point(235, 196);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(46, 21);
            this.buttonOK.TabIndex = 26;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // imageListPgObjects
            // 
            this.imageListPgObjects.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPgObjects.ImageStream")));
            this.imageListPgObjects.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListPgObjects.Images.SetKeyName(0, "Postgres.ico");
            this.imageListPgObjects.Images.SetKeyName(1, "Database.ico");
            this.imageListPgObjects.Images.SetKeyName(2, "Login.ico");
            // 
            // FormConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 220);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Postgres";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStripConnections.ResumeLayout(false);
            this.toolStripConnections.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxRole;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ImageList imageListConnectionState;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.TextBox textBoxDatabase;
        private System.Windows.Forms.ToolStrip toolStripConnections;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveConnection;
        private System.Windows.Forms.ImageList imageListPgObjects;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.Button buttonOK;
    }
}