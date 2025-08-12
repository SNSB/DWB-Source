namespace DiversityWorkbench
{
    partial class FormDatabaseConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseConnection));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelRemoteDB = new System.Windows.Forms.TableLayoutPanel();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.buttonConnectToServer = new System.Windows.Forms.Button();
            this.imageListConnectionState = new System.Windows.Forms.ImageList(this.components);
            this.radioButtonRestrictToVersion = new System.Windows.Forms.RadioButton();
            this.radioButtonRestrictToModule = new System.Windows.Forms.RadioButton();
            this.radioButtonShowAllDatabases = new System.Windows.Forms.RadioButton();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelLogin = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonAuthentication = new System.Windows.Forms.RadioButton();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.radioButtonSQLAuthentication = new System.Windows.Forms.RadioButton();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.tableLayoutPanelLocal = new System.Windows.Forms.TableLayoutPanel();
            this.labelLocalFile = new System.Windows.Forms.Label();
            this.textBoxLocalFile = new System.Windows.Forms.TextBox();
            this.buttonOpenLocalFile = new System.Windows.Forms.Button();
            this.labelLocal = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonRemote = new System.Windows.Forms.RadioButton();
            this.radioButtonLocal = new System.Windows.Forms.RadioButton();
            this.labelSelectRemoteLocal = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControlDialogPanel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelRemoteDB.SuspendLayout();
            this.groupBoxLogin.SuspendLayout();
            this.tableLayoutPanelLogin.SuspendLayout();
            this.tableLayoutPanelLocal.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            resources.ApplyResources(this.splitContainerMain, "splitContainerMain");
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelRemoteDB);
            this.helpProvider.SetShowHelp(this.splitContainerMain.Panel1, ((bool)(resources.GetObject("splitContainerMain.Panel1.ShowHelp"))));
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelLocal);
            this.helpProvider.SetShowHelp(this.splitContainerMain.Panel2, ((bool)(resources.GetObject("splitContainerMain.Panel2.ShowHelp"))));
            this.helpProvider.SetShowHelp(this.splitContainerMain, ((bool)(resources.GetObject("splitContainerMain.ShowHelp"))));
            // 
            // tableLayoutPanelRemoteDB
            // 
            resources.ApplyResources(this.tableLayoutPanelRemoteDB, "tableLayoutPanelRemoteDB");
            this.tableLayoutPanelRemoteDB.Controls.Add(this.labelDatabase, 0, 7);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.comboBoxDatabase, 0, 8);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.buttonConnectToServer, 0, 6);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.radioButtonRestrictToVersion, 0, 3);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.radioButtonRestrictToModule, 0, 4);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.radioButtonShowAllDatabases, 0, 5);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.textBoxPort, 1, 1);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.comboBoxServer, 0, 1);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.groupBoxLogin, 0, 2);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.labelPort, 1, 0);
            this.tableLayoutPanelRemoteDB.Controls.Add(this.labelMessage, 0, 0);
            this.tableLayoutPanelRemoteDB.Name = "tableLayoutPanelRemoteDB";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelRemoteDB, ((bool)(resources.GetObject("tableLayoutPanelRemoteDB.ShowHelp"))));
            // 
            // labelDatabase
            // 
            this.tableLayoutPanelRemoteDB.SetColumnSpan(this.labelDatabase, 2);
            resources.ApplyResources(this.labelDatabase, "labelDatabase");
            this.labelDatabase.Name = "labelDatabase";
            this.helpProvider.SetShowHelp(this.labelDatabase, ((bool)(resources.GetObject("labelDatabase.ShowHelp"))));
            // 
            // comboBoxDatabase
            // 
            this.tableLayoutPanelRemoteDB.SetColumnSpan(this.comboBoxDatabase, 2);
            resources.ApplyResources(this.comboBoxDatabase, "comboBoxDatabase");
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.helpProvider.SetShowHelp(this.comboBoxDatabase, ((bool)(resources.GetObject("comboBoxDatabase.ShowHelp"))));
            this.comboBoxDatabase.SelectedIndexChanged += new System.EventHandler(this.comboBoxDatabase_SelectedIndexChanged);
            this.comboBoxDatabase.TextChanged += new System.EventHandler(this.comboBoxDatabase_TextChanged);
            // 
            // buttonConnectToServer
            // 
            this.tableLayoutPanelRemoteDB.SetColumnSpan(this.buttonConnectToServer, 2);
            resources.ApplyResources(this.buttonConnectToServer, "buttonConnectToServer");
            this.buttonConnectToServer.ImageList = this.imageListConnectionState;
            this.buttonConnectToServer.Name = "buttonConnectToServer";
            this.helpProvider.SetShowHelp(this.buttonConnectToServer, ((bool)(resources.GetObject("buttonConnectToServer.ShowHelp"))));
            this.buttonConnectToServer.UseVisualStyleBackColor = true;
            this.buttonConnectToServer.Click += new System.EventHandler(this.buttonConnectToServer_Click);
            // 
            // imageListConnectionState
            // 
            this.imageListConnectionState.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListConnectionState.ImageStream")));
            this.imageListConnectionState.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListConnectionState.Images.SetKeyName(0, "Database.ico");
            this.imageListConnectionState.Images.SetKeyName(1, "NoDatabase.ico");
            this.imageListConnectionState.Images.SetKeyName(2, "NETHOOD.ICO");
            // 
            // radioButtonRestrictToVersion
            // 
            this.tableLayoutPanelRemoteDB.SetColumnSpan(this.radioButtonRestrictToVersion, 2);
            resources.ApplyResources(this.radioButtonRestrictToVersion, "radioButtonRestrictToVersion");
            this.radioButtonRestrictToVersion.Name = "radioButtonRestrictToVersion";
            this.helpProvider.SetShowHelp(this.radioButtonRestrictToVersion, ((bool)(resources.GetObject("radioButtonRestrictToVersion.ShowHelp"))));
            this.radioButtonRestrictToVersion.UseVisualStyleBackColor = true;
            // 
            // radioButtonRestrictToModule
            // 
            this.radioButtonRestrictToModule.Checked = true;
            this.tableLayoutPanelRemoteDB.SetColumnSpan(this.radioButtonRestrictToModule, 2);
            resources.ApplyResources(this.radioButtonRestrictToModule, "radioButtonRestrictToModule");
            this.radioButtonRestrictToModule.Name = "radioButtonRestrictToModule";
            this.helpProvider.SetShowHelp(this.radioButtonRestrictToModule, ((bool)(resources.GetObject("radioButtonRestrictToModule.ShowHelp"))));
            this.radioButtonRestrictToModule.TabStop = true;
            this.radioButtonRestrictToModule.UseVisualStyleBackColor = true;
            // 
            // radioButtonShowAllDatabases
            // 
            this.tableLayoutPanelRemoteDB.SetColumnSpan(this.radioButtonShowAllDatabases, 2);
            resources.ApplyResources(this.radioButtonShowAllDatabases, "radioButtonShowAllDatabases");
            this.radioButtonShowAllDatabases.Name = "radioButtonShowAllDatabases";
            this.helpProvider.SetShowHelp(this.radioButtonShowAllDatabases, ((bool)(resources.GetObject("radioButtonShowAllDatabases.ShowHelp"))));
            this.radioButtonShowAllDatabases.UseVisualStyleBackColor = true;
            // 
            // textBoxPort
            // 
            resources.ApplyResources(this.textBoxPort, "textBoxPort");
            this.textBoxPort.Name = "textBoxPort";
            this.helpProvider.SetShowHelp(this.textBoxPort, ((bool)(resources.GetObject("textBoxPort.ShowHelp"))));
            // 
            // comboBoxServer
            // 
            resources.ApplyResources(this.comboBoxServer, "comboBoxServer");
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Name = "comboBoxServer";
            this.helpProvider.SetShowHelp(this.comboBoxServer, ((bool)(resources.GetObject("comboBoxServer.ShowHelp"))));
            this.comboBoxServer.DropDown += new System.EventHandler(this.comboBoxServer_DropDown);
            this.comboBoxServer.SelectedIndexChanged += new System.EventHandler(this.comboBoxServer_SelectedIndexChanged);
            // 
            // groupBoxLogin
            // 
            this.tableLayoutPanelRemoteDB.SetColumnSpan(this.groupBoxLogin, 2);
            this.groupBoxLogin.Controls.Add(this.tableLayoutPanelLogin);
            resources.ApplyResources(this.groupBoxLogin, "groupBoxLogin");
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.helpProvider.SetShowHelp(this.groupBoxLogin, ((bool)(resources.GetObject("groupBoxLogin.ShowHelp"))));
            this.groupBoxLogin.TabStop = false;
            // 
            // tableLayoutPanelLogin
            // 
            resources.ApplyResources(this.tableLayoutPanelLogin, "tableLayoutPanelLogin");
            this.tableLayoutPanelLogin.Controls.Add(this.radioButtonAuthentication, 0, 0);
            this.tableLayoutPanelLogin.Controls.Add(this.textBoxPassword, 1, 3);
            this.tableLayoutPanelLogin.Controls.Add(this.textBoxUser, 1, 2);
            this.tableLayoutPanelLogin.Controls.Add(this.radioButtonSQLAuthentication, 0, 1);
            this.tableLayoutPanelLogin.Controls.Add(this.labelPassword, 0, 3);
            this.tableLayoutPanelLogin.Controls.Add(this.labelUser, 0, 2);
            this.tableLayoutPanelLogin.Name = "tableLayoutPanelLogin";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelLogin, ((bool)(resources.GetObject("tableLayoutPanelLogin.ShowHelp"))));
            // 
            // radioButtonAuthentication
            // 
            this.radioButtonAuthentication.Checked = true;
            this.tableLayoutPanelLogin.SetColumnSpan(this.radioButtonAuthentication, 2);
            resources.ApplyResources(this.radioButtonAuthentication, "radioButtonAuthentication");
            this.radioButtonAuthentication.Name = "radioButtonAuthentication";
            this.helpProvider.SetShowHelp(this.radioButtonAuthentication, ((bool)(resources.GetObject("radioButtonAuthentication.ShowHelp"))));
            this.radioButtonAuthentication.TabStop = true;
            this.radioButtonAuthentication.CheckedChanged += new System.EventHandler(this.radioButtonAuthentication_CheckedChanged);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BackColor = System.Drawing.SystemColors.ActiveBorder;
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.Name = "textBoxPassword";
            this.helpProvider.SetShowHelp(this.textBoxPassword, ((bool)(resources.GetObject("textBoxPassword.ShowHelp"))));
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            this.textBoxPassword.Leave += new System.EventHandler(this.textBoxPassword_Leave);
            // 
            // textBoxUser
            // 
            this.textBoxUser.BackColor = System.Drawing.SystemColors.ActiveBorder;
            resources.ApplyResources(this.textBoxUser, "textBoxUser");
            this.textBoxUser.Name = "textBoxUser";
            this.helpProvider.SetShowHelp(this.textBoxUser, ((bool)(resources.GetObject("textBoxUser.ShowHelp"))));
            this.textBoxUser.TextChanged += new System.EventHandler(this.textBoxUser_TextChanged);
            this.textBoxUser.Leave += new System.EventHandler(this.textBoxUser_Leave);
            // 
            // radioButtonSQLAuthentication
            // 
            this.tableLayoutPanelLogin.SetColumnSpan(this.radioButtonSQLAuthentication, 2);
            resources.ApplyResources(this.radioButtonSQLAuthentication, "radioButtonSQLAuthentication");
            this.radioButtonSQLAuthentication.Name = "radioButtonSQLAuthentication";
            this.helpProvider.SetShowHelp(this.radioButtonSQLAuthentication, ((bool)(resources.GetObject("radioButtonSQLAuthentication.ShowHelp"))));
            this.radioButtonSQLAuthentication.TabStop = true;
            // 
            // labelPassword
            // 
            resources.ApplyResources(this.labelPassword, "labelPassword");
            this.labelPassword.Name = "labelPassword";
            this.helpProvider.SetShowHelp(this.labelPassword, ((bool)(resources.GetObject("labelPassword.ShowHelp"))));
            // 
            // labelUser
            // 
            resources.ApplyResources(this.labelUser, "labelUser");
            this.labelUser.Name = "labelUser";
            this.helpProvider.SetShowHelp(this.labelUser, ((bool)(resources.GetObject("labelUser.ShowHelp"))));
            // 
            // labelPort
            // 
            resources.ApplyResources(this.labelPort, "labelPort");
            this.labelPort.Name = "labelPort";
            this.helpProvider.SetShowHelp(this.labelPort, ((bool)(resources.GetObject("labelPort.ShowHelp"))));
            // 
            // labelMessage
            // 
            resources.ApplyResources(this.labelMessage, "labelMessage");
            this.labelMessage.Name = "labelMessage";
            this.helpProvider.SetShowHelp(this.labelMessage, ((bool)(resources.GetObject("labelMessage.ShowHelp"))));
            // 
            // tableLayoutPanelLocal
            // 
            resources.ApplyResources(this.tableLayoutPanelLocal, "tableLayoutPanelLocal");
            this.tableLayoutPanelLocal.Controls.Add(this.labelLocalFile, 0, 1);
            this.tableLayoutPanelLocal.Controls.Add(this.textBoxLocalFile, 1, 1);
            this.tableLayoutPanelLocal.Controls.Add(this.buttonOpenLocalFile, 2, 1);
            this.tableLayoutPanelLocal.Controls.Add(this.labelLocal, 0, 0);
            this.tableLayoutPanelLocal.Name = "tableLayoutPanelLocal";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelLocal, ((bool)(resources.GetObject("tableLayoutPanelLocal.ShowHelp"))));
            // 
            // labelLocalFile
            // 
            resources.ApplyResources(this.labelLocalFile, "labelLocalFile");
            this.labelLocalFile.Name = "labelLocalFile";
            this.helpProvider.SetShowHelp(this.labelLocalFile, ((bool)(resources.GetObject("labelLocalFile.ShowHelp"))));
            // 
            // textBoxLocalFile
            // 
            resources.ApplyResources(this.textBoxLocalFile, "textBoxLocalFile");
            this.textBoxLocalFile.Name = "textBoxLocalFile";
            this.helpProvider.SetShowHelp(this.textBoxLocalFile, ((bool)(resources.GetObject("textBoxLocalFile.ShowHelp"))));
            // 
            // buttonOpenLocalFile
            // 
            resources.ApplyResources(this.buttonOpenLocalFile, "buttonOpenLocalFile");
            this.buttonOpenLocalFile.Image = global::DiversityWorkbench.ResourceWorkbench.Open;
            this.buttonOpenLocalFile.Name = "buttonOpenLocalFile";
            this.helpProvider.SetShowHelp(this.buttonOpenLocalFile, ((bool)(resources.GetObject("buttonOpenLocalFile.ShowHelp"))));
            this.buttonOpenLocalFile.UseVisualStyleBackColor = true;
            this.buttonOpenLocalFile.Click += new System.EventHandler(this.buttonOpenLocalFile_Click);
            // 
            // labelLocal
            // 
            resources.ApplyResources(this.labelLocal, "labelLocal");
            this.tableLayoutPanelLocal.SetColumnSpan(this.labelLocal, 3);
            this.labelLocal.Name = "labelLocal";
            this.helpProvider.SetShowHelp(this.labelLocal, ((bool)(resources.GetObject("labelLocal.ShowHelp"))));
            // 
            // tableLayoutPanelMain
            // 
            resources.ApplyResources(this.tableLayoutPanelMain, "tableLayoutPanelMain");
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonRemote, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonLocal, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelSelectRemoteLocal, 0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, ((bool)(resources.GetObject("tableLayoutPanelMain.ShowHelp"))));
            // 
            // radioButtonRemote
            // 
            resources.ApplyResources(this.radioButtonRemote, "radioButtonRemote");
            this.radioButtonRemote.Checked = true;
            this.radioButtonRemote.Name = "radioButtonRemote";
            this.helpProvider.SetShowHelp(this.radioButtonRemote, ((bool)(resources.GetObject("radioButtonRemote.ShowHelp"))));
            this.radioButtonRemote.TabStop = true;
            this.radioButtonRemote.UseVisualStyleBackColor = true;
            this.radioButtonRemote.CheckedChanged += new System.EventHandler(this.radioButtonRemote_CheckedChanged);
            // 
            // radioButtonLocal
            // 
            resources.ApplyResources(this.radioButtonLocal, "radioButtonLocal");
            this.radioButtonLocal.Name = "radioButtonLocal";
            this.helpProvider.SetShowHelp(this.radioButtonLocal, ((bool)(resources.GetObject("radioButtonLocal.ShowHelp"))));
            this.radioButtonLocal.UseVisualStyleBackColor = true;
            // 
            // labelSelectRemoteLocal
            // 
            resources.ApplyResources(this.labelSelectRemoteLocal, "labelSelectRemoteLocal");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelSelectRemoteLocal, 2);
            this.labelSelectRemoteLocal.Name = "labelSelectRemoteLocal";
            this.helpProvider.SetShowHelp(this.labelSelectRemoteLocal, ((bool)(resources.GetObject("labelSelectRemoteLocal.ShowHelp"))));
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(this.userControlDialogPanel, "userControlDialogPanel");
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.helpProvider.SetShowHelp(this.userControlDialogPanel, ((bool)(resources.GetObject("userControlDialogPanel.ShowHelp"))));
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // FormDatabaseConnection
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "FormDatabaseConnection";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatabaseConnection_FormClosing);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelRemoteDB.ResumeLayout(false);
            this.tableLayoutPanelRemoteDB.PerformLayout();
            this.groupBoxLogin.ResumeLayout(false);
            this.tableLayoutPanelLogin.ResumeLayout(false);
            this.tableLayoutPanelLogin.PerformLayout();
            this.tableLayoutPanelLocal.ResumeLayout(false);
            this.tableLayoutPanelLocal.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.RadioButton radioButtonSQLAuthentication;
        private System.Windows.Forms.RadioButton radioButtonAuthentication;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.ImageList imageListConnectionState;
        private System.Windows.Forms.Button buttonConnectToServer;
        private System.Windows.Forms.RadioButton radioButtonShowAllDatabases;
        private System.Windows.Forms.RadioButton radioButtonRestrictToVersion;
        private System.Windows.Forms.RadioButton radioButtonRestrictToModule;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.RadioButton radioButtonRemote;
        private System.Windows.Forms.RadioButton radioButtonLocal;
        private System.Windows.Forms.Label labelSelectRemoteLocal;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLocal;
        private System.Windows.Forms.Label labelLocalFile;
        private System.Windows.Forms.TextBox textBoxLocalFile;
        private System.Windows.Forms.Button buttonOpenLocalFile;
        private System.Windows.Forms.Label labelLocal;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRemoteDB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogin;
    }
}