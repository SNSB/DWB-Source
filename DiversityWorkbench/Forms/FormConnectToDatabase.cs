using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Net.NetworkInformation;

namespace DiversityWorkbench.Forms
{
    public partial class FormConnectToDatabase : Form
    {

        #region Parameter

        private string _ModuleName;
        System.Version _AssemblyVersion;
        private string _DatabaseVersion;
        bool _EmptyDatabase;
        //private bool _IsLocal = false;

        public enum DatabaseRestriction { Module, Version, Any };
        private DatabaseRestriction _DatabaseRestriction = DatabaseRestriction.Module;

        private bool _ForMainModule = true;
        private bool _LeaveMainConnectionUnchanged = false;
        private bool _OnlyLogin = false;

        public enum DatabaseManagementSystem { MSSQL, Postgres }
        private DatabaseManagementSystem _DatabaseManagementSystem = DatabaseManagementSystem.MSSQL;
        private System.Collections.Specialized.StringCollection _PreviousConnections;

        private DiversityWorkbench.ServerConnection _LocalServerConnection;

        public DiversityWorkbench.ServerConnection LocalServerConnection
        {
            get
            {
                if (this._LocalServerConnection == null)
                    this._LocalServerConnection = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);
                return _LocalServerConnection;
            }
            set { _LocalServerConnection = value; }
        }

        private bool _EnableKeyControl = false;

        #endregion

        #region Construction

        public FormConnectToDatabase(string ModulName = "", System.Collections.Generic.Dictionary<string, string> Versions = null, bool EnableKeyControl = false)
        {
            InitializeComponent();
            this._EnableKeyControl = EnableKeyControl;
            this.initForm();
            // set local parameters
            if (Settings.ModuleName != null && Settings.ModuleName.Length > 0)
                this._ModuleName = Settings.ModuleName;
            else if (ModulName.Length > 0)
                this._ModuleName = ModulName;
            else
            {
                this._ModuleName = Application.ProductName.ToString();
            }

            // init form values
            this.textBoxPort.Text = Settings.DatabasePort.ToString();
            this.textBoxUser.Text = Settings.DatabaseUser;
            this.textBoxPassword.Text = DiversityWorkbench.User.Password;
            this.comboBoxServer.Text = Settings.DatabaseServer;
            this.comboBoxDatabase.Text = Settings.DatabaseName;
            this.radioButtonAuthentication.Checked = Settings.IsTrustedConnection;
            this.radioButtonSQLAuthentication.Checked = !Settings.IsTrustedConnection;

            if (Settings.Password.Length > 0)
                this.textBoxPassword.Text = Settings.Password;

            this.ChangeAuthentication();
            this.userControlDialogPanel.buttonOK.Enabled = false;

            //this.Database_Restriction = DatabaseRestriction.Module;
            this._PreviousConnections = DiversityWorkbench.WorkbenchSettings.Default.PreviousConnections;
            this.initPreviousConnections();

            this.setEncryptionControls();
            this._VersionsForCreation = Versions;
        }

        public FormConnectToDatabase(DiversityWorkbench.ServerConnection ServerConnection)
        {
            InitializeComponent();
            this.initForm();
            try
            {
                if (ServerConnection != null)
                {
                    this.textBoxPort.Text = ServerConnection.DatabaseServerPort.ToString();
                    this.textBoxUser.Text = ServerConnection.DatabaseUser;
                    this.textBoxPassword.Text = DiversityWorkbench.User.Password;
                    this.textBoxPort.Text = ServerConnection.DatabaseServerPort.ToString();
                    this.comboBoxServer.Text = ServerConnection.DatabaseServer;
                    this.comboBoxDatabase.Text = ServerConnection.DatabaseName;
                    this.radioButtonAuthentication.Checked = ServerConnection.IsTrustedConnection;
                    this.radioButtonSQLAuthentication.Checked = !ServerConnection.IsTrustedConnection;
                    this._ModuleName = ServerConnection.ModuleName;
                }
                this.ChangeAuthentication();
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._ForMainModule = false;
                this.setEncryptionControls();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(DiversityWorkbench.ServerConnection ServerConnection)");
            }
        }

        public FormConnectToDatabase(DiversityWorkbench.ServerConnection ServerConnection, bool LeaveMainConnectionUnchanged)
        {
            InitializeComponent();
            this.initForm();
            this._LeaveMainConnectionUnchanged = LeaveMainConnectionUnchanged;
            try
            {
                if (ServerConnection != null)
                {
                    this.textBoxPort.Text = ServerConnection.DatabaseServerPort.ToString();
                    this.textBoxUser.Text = ServerConnection.DatabaseUser;
                    this.textBoxPassword.Text = DiversityWorkbench.User.Password;
                    this.textBoxPort.Text = ServerConnection.DatabaseServerPort.ToString();
                    this.comboBoxServer.Text = ServerConnection.DatabaseServer;
                    this.comboBoxDatabase.Text = ServerConnection.DatabaseName;
                    this.radioButtonAuthentication.Checked = ServerConnection.IsTrustedConnection;
                    this.radioButtonSQLAuthentication.Checked = !ServerConnection.IsTrustedConnection;
                    this._ModuleName = ServerConnection.ModuleName;
                }
                this.ChangeAuthentication();
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._ForMainModule = false;
                this.setEncryptionControls();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(DiversityWorkbench.ServerConnection ServerConnection)");
            }
        }

        /// <summary>
        /// Only get login data
        /// </summary>
        /// <param name="OnlyLogin">If a change of the server, port and database should be impossible</param>
        /// <param name="ServerConnection"></param>
        public FormConnectToDatabase(bool OnlyLogin, DiversityWorkbench.ServerConnection ServerConnection)
        {
            InitializeComponent();
            this.initForm();
            this._LeaveMainConnectionUnchanged = true;
            this._OnlyLogin = OnlyLogin;
            try
            {
                if (ServerConnection != null)
                {
                    this.textBoxPort.Text = ServerConnection.DatabaseServerPort.ToString();
                    this.textBoxUser.Text = ServerConnection.DatabaseUser;
                    this.textBoxPassword.Text = DiversityWorkbench.User.Password;
                    this.textBoxPort.Text = ServerConnection.DatabaseServerPort.ToString();
                    this.comboBoxServer.Text = ServerConnection.DatabaseServer;
                    this.comboBoxDatabase.Text = ServerConnection.DatabaseName;
                    this.radioButtonAuthentication.Checked = ServerConnection.IsTrustedConnection;
                    this.radioButtonSQLAuthentication.Checked = !ServerConnection.IsTrustedConnection;
                    this._ModuleName = ServerConnection.ModuleName;
                }
                if (OnlyLogin)
                {
                    this.textBoxPort.Enabled = false;
                    this.comboBoxDatabase.Enabled = false;
                    this.comboBoxServer.Enabled = false;
                }
                this.ChangeAuthentication();
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._ForMainModule = false;
                this.setEncryptionControls();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(DiversityWorkbench.ServerConnection ServerConnection)");
            }
        }

        public FormConnectToDatabase(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)
        {
            InitializeComponent();
            this.initForm();
            try
            {
                this._ModuleName = IWorkbenchUnit.getServerConnection().ModuleName;

                this.textBoxUser.Text = Settings.DatabaseUser;
                this.textBoxPassword.Text = DiversityWorkbench.User.Password;
                this.textBoxPort.Text = Settings.DatabasePort.ToString();
                this.comboBoxServer.Text = Settings.DatabaseServer;
                this.comboBoxDatabase.Text = Settings.DatabaseName;
                this.radioButtonAuthentication.Checked = Settings.IsTrustedConnection;
                this.radioButtonSQLAuthentication.Checked = !Settings.IsTrustedConnection;

                this.ChangeAuthentication();
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._ForMainModule = false;
                this.setEncryptionControls();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)");
            }
        }

        public FormConnectToDatabase(bool EmptyDatabase)
        {
            InitializeComponent();
            this.initForm();
            try
            {
                this._ModuleName = "";
                this._EmptyDatabase = EmptyDatabase;

                this.textBoxUser.Text = Settings.DatabaseUser;
                this.textBoxPassword.Text = DiversityWorkbench.User.Password;
                this.textBoxPort.Text = Settings.DatabasePort.ToString();
                this.comboBoxServer.Text = Settings.DatabaseServer;
                this.comboBoxDatabase.Text = Settings.DatabaseName;
                this.radioButtonAuthentication.Checked = Settings.IsTrustedConnection;
                this.radioButtonSQLAuthentication.Checked = !Settings.IsTrustedConnection;

                this.ChangeAuthentication();
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._ForMainModule = false;
                this.setEncryptionControls();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(ModuleName)");
            }
        }

        public FormConnectToDatabase(DatabaseManagementSystem DBMS, System.Collections.Specialized.StringCollection PreviousConnections, string Module = "DiversityCollectionCache")
        {
            InitializeComponent();
            this.initForm();
            this.Height = (int)(this.Height * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

            try
            {
                this._ModuleName = Module;
                this._DatabaseManagementSystem = DBMS;
                this._LeaveMainConnectionUnchanged = true;
                if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
                {
                    this.radioButtonSQLAuthentication.Text = "Postgres authentication";
                    this.textBoxPort.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString();
                    this.textBoxUser.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Role;
                    this.comboBoxDatabase.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Database;
                    this.comboBoxServer.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Server;
                    this.textBoxPassword.Text = DiversityWorkbench.PostgreSQL.Connection.Password;

                    /// TODO: disabled until solution is found
                    DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted = false;

                    this.radioButtonAuthentication.Checked = DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted;
                    this.radioButtonSQLAuthentication.Checked = !DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted;
                    this.radioButtonAuthentication.Enabled = false;
                }
                if (PreviousConnections.Count > 0)
                {
                    this._PreviousConnections = PreviousConnections;
                    this.initPreviousConnections();
                }
                this.ChangeAuthentication();
                this.setEncryptionControls();
            }
            catch (System.Exception ex)
            { }
        }

        #endregion

        #region From and events

        private void initForm()
        {
            this.CancelButton = this.userControlDialogPanel.buttonCancel;
            this.AcceptButton = this.userControlDialogPanel.buttonOK;
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void comboBoxServer_DropDown(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this._DatabaseManagementSystem == DatabaseManagementSystem.MSSQL)
                {
                    if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList == null)
                    {
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList = new System.Collections.Specialized.StringCollection();
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Add("localhost");
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Add(global::DiversityWorkbench.Properties.Settings.Default.TrainingsServer);
#if DEBUG
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Add(global::DiversityWorkbench.Properties.Settings.Default.DevelopmentServer);
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Add("127.0.0.1");
#endif
                    }
                    if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList != null &&
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Count > 0)
                    {
                        this.comboBoxServer.Items.Clear();
                        foreach (string s in DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList)
                            this.comboBoxServer.Items.Add(s);
                    }
                    this.comboBoxDatabase.DataSource = null;
                }
                else if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
                {
                    this.comboBoxServer.Items.Clear();
                    foreach (string C in DiversityWorkbench.PostgreSQL.Connection.PreviousConnections())
                    {
                        string[] cc = C.Split(new char[] { '|' });
                        if (!this.comboBoxServer.Items.Contains(cc[0]))
                            this.comboBoxServer.Items.Add(cc[0]);
                    }
                    if (this.comboBoxServer.Items.Count == 0)
                        this.comboBoxServer.Items.Add("localhost");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void FormConnectToDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bool OK = true;
                if (this.comboBoxDatabase.Text.Length == 0)
                {
                    OK = false;
                    if (this.DialogResult == DialogResult.OK)
                        System.Windows.Forms.MessageBox.Show("No database has been selected");
                }
                if (this.textBoxPort.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No port was given");
                    OK = false;
                }
                if (this.comboBoxServer.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No server has been selected");
                    OK = false;
                }
                if (OK && this.DialogResult == DialogResult.OK
                    && this._DatabaseManagementSystem == DatabaseManagementSystem.MSSQL)
                {
                    if (this._ForMainModule)
                    {
                        Settings.DatabaseName = this.comboBoxDatabase.Text;
                        Settings.DatabasePort = System.Int32.Parse(this.textBoxPort.Text);
                        Settings.DatabaseServer = this.comboBoxServer.Text;
                        Settings.IsTrustedConnection = this.radioButtonAuthentication.Checked;
                        if (Settings.IsTrustedConnection)
                            Settings.AddPreviousConnection(Settings.DatabaseServer, Settings.DatabasePort);
                        else
                            Settings.AddPreviousConnection(Settings.DatabaseServer, Settings.DatabasePort, Settings.DatabaseUser);
                    }
                    else if (this._LeaveMainConnectionUnchanged)
                    {
                        this.LocalServerConnection.DatabaseName = this.comboBoxDatabase.Text;
                        this.LocalServerConnection.DatabaseServerPort = int.Parse(this.textBoxPort.Text);
                        this.LocalServerConnection.DatabaseServer = this.comboBoxServer.Text;
                    }
                    else
                    {
                        if (Settings.DatabaseName.Length == 0)
                            Settings.DatabaseName = this.comboBoxDatabase.Text;
                        if (Settings.DatabasePort == 0)
                            Settings.DatabasePort = System.Int32.Parse(this.textBoxPort.Text);
                        if (Settings.DatabaseServer.Length == 0)
                            Settings.DatabaseServer = this.comboBoxServer.Text;
                    }
                    Settings.IsLocalExpressDatabase = false;
                    if (!this.radioButtonAuthentication.Checked && !this._LeaveMainConnectionUnchanged)
                    {
                        Settings.DatabaseUser = this.textBoxUser.Text;
                        Settings.Password = this.textBoxPassword.Text;
                    }
                    else if (this._LeaveMainConnectionUnchanged)
                    {
                        this.LocalServerConnection.DatabaseUser = this.textBoxUser.Text;
                        this.LocalServerConnection.DatabasePassword = this.textBoxPassword.Text;
                    }
                    if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList == null)
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList = new System.Collections.Specialized.StringCollection();
                    if (!DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Contains(Settings.DatabaseServer))
                    {
                        DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Add(Settings.DatabaseServer);
                        DiversityWorkbench.WorkbenchSettings.Default.Save();
                    }
                    if (this._ForMainModule && DiversityWorkbench.Database.ColumnPrivacyConsentDoesExist())
                    {
                        DiversityWorkbench.Database.PrivacyConsent PC = DiversityWorkbench.Database.PrivacyConsent.undecided;
                        OK = DiversityWorkbench.Database.PrivacyConsentOK(ref PC);
                        if (!OK)
                        {
                            Settings.DatabaseName = "";
                            switch (PC)
                            {
                                case DiversityWorkbench.Database.PrivacyConsent.rejected:
                                    System.Windows.Forms.MessageBox.Show("Thanks for your interest in the DiversityWorkbench. To get access please contact the administrator or consent to the treatment of your personal data");
                                    break;
                                case DiversityWorkbench.Database.PrivacyConsent.NoAccess:
                                    System.Windows.Forms.MessageBox.Show("You have no access to this database. To get access please contact the administrator");
                                    break;
                                default:
                                    System.Windows.Forms.MessageBox.Show("An error occurred. Please contact the administrator");
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            bool OK = true;
            int Port;
            if (this.textBoxPort.Text.Length > 3)
            {
                if (int.TryParse(this.textBoxPort.Text, out Port))
                {
                    if (Port > 65535 || Port < 1024)
                    {
                        OK = false;
                    }
                }
                else
                {
                    OK = false;
                }
                if (!OK)
                {
                    System.Windows.Forms.MessageBox.Show("The entry " + this.textBoxPort.Text + " is not a valid port");

                }
            }
            DiversityWorkbench.LinkedServer.ResetLinkedServer();
        }

        private void comboBoxServer_TextChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.LinkedServer.ResetLinkedServer();
            this.buttonPing.Visible = this.comboBoxServer.Text.Length > 0;
        }

        #endregion

        #region Authentication

        private void ChangeAuthentication()
        {
            if (this.radioButtonAuthentication.Checked)
            {
                this.panelPwButtons.Enabled = false;
                this.textBoxPassword.Enabled = false;
                this.textBoxPassword.BackColor = System.Drawing.SystemColors.ActiveBorder;
                this.textBoxUser.Enabled = false;
                this.textBoxUser.BackColor = System.Drawing.SystemColors.ActiveBorder;
                this.AcceptButton = this.buttonConnectToServer;
            }
            else
            {
                this.panelPwButtons.Enabled = true;
                this.textBoxPassword.Enabled = true;
                this.textBoxPassword.BackColor = System.Drawing.Color.White;
                this.textBoxUser.Enabled = true;
                this.textBoxUser.BackColor = System.Drawing.Color.White;
            }
            this.pictureBoxDatabaseLogin.Enabled = this.radioButtonAuthentication.Checked;
            this.pictureBoxWindowsLogin.Enabled = !this.radioButtonAuthentication.Checked;

            if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
            {
                DiversityWorkbench.PostgreSQL.Settings.Default.IsTrusted = this.radioButtonAuthentication.Checked;
            }

            this.comboBoxDatabase.Enabled = false;
            this.comboBoxDatabase.DataSource = null;
            this.userControlDialogPanel.buttonOK.Enabled = false;
        }

        private void radioButtonAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeAuthentication();
        }

        private void textBoxUser_Leave(object sender, EventArgs e)
        {
            if (this.textBoxUser.Text != Settings.DatabaseUser)
            {
                this.ChangeAuthentication();
            }
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (this._LeaveMainConnectionUnchanged)
            {
                this.LocalServerConnection.DatabasePassword = this.textBoxPassword.Text;
            }
            else
            {
                if (this.textBoxPassword.Text != Settings.Password)
                {
                    this.ChangeAuthentication();
                    DiversityWorkbench.User.Password = this.textBoxPassword.Text;
                }
            }
        }

        #endregion

        #region Connection

        private string ConnectionString
        {
            get
            {
                string conStr = "";
                if (this.comboBoxServer.Text.Length > 0 && this.comboBoxDatabase.Text.Length > 0)
                {
                    if (this._DatabaseManagementSystem == DatabaseManagementSystem.MSSQL)
                    {
                        conStr = "Data Source=" + this.comboBoxServer.Text;
                        if (this.textBoxPort.Text.Length > 0) conStr += "," + this.textBoxPort.Text;
                        conStr += ";initial catalog=";
                        if (this.comboBoxDatabase.Text.Length > 0) conStr += this.comboBoxDatabase.Text + ";";
                        else conStr += "master;";
                        if (this.radioButtonAuthentication.Checked)
                        {
                            conStr += "Integrated Security=True";
                        }
                        else
                        {
                            if (this.textBoxUser.Text.Length > 0 && this.textBoxPassword.Text.Length > 0)
                                conStr += "user id=" + this.textBoxUser.Text + ";password=" + this.textBoxPassword.Text;
                            else conStr = "";
                        }
                    }
                    else if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
                    {

                    }
                    // MW 2018/10/01: Encrypted connection
                    if (conStr.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                        conStr += ";Encrypt=true;TrustServerCertificate=true";
                }
                return conStr;
            }
        }

        private string ConnectionStringWithoutPassword
        {
            get
            {
                string conStr = "";
                if (this.comboBoxServer.Text.Length > 0 && this.comboBoxDatabase.Text.Length > 0)
                {
                    conStr = "Data Source=" + this.comboBoxServer.Text;
                    if (this.textBoxPort.Text.Length > 0) conStr += "," + this.textBoxPort.Text;
                    conStr += ";initial catalog=";
                    if (this.comboBoxDatabase.Text.Length > 0) conStr += this.comboBoxDatabase.Text + ";";
                    else conStr += "master;";
                    if (this.radioButtonAuthentication.Checked)
                    {
                        conStr += "Integrated Security=True";
                    }
                    else
                    {
                        if (this.textBoxUser.Text.Length > 0 && this.textBoxPassword.Text.Length > 0)
                        {
                            conStr += "user id=" + this.textBoxUser.Text + ";password=";
                            for (int i = 0; i < this.textBoxPassword.Text.Length; i++) { conStr += "*"; };
                        }
                        else conStr = "";
                    }
                }
                // MW 2018/10/01: Encrypted connection
                if (conStr.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                    conStr += ";Encrypt=true;TrustServerCertificate=true";
                return conStr;
            }
        }

        private void buttonConnectToServer_Click(object sender, EventArgs e)
        {
            if (this.buttonConnectToServer.Text == DiversityWorkbench.Forms.FormDatabaseConnectionText.Reset)
            {
                this.buttonConnectToServer.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Connect_to_server;
                this.buttonConnectToServer.ImageIndex = 2;
                this.groupBoxServer.Enabled = true;
                this.groupBoxLogin.Enabled = true;
                this.comboBoxDatabase.Enabled = false;
                this.buttonCreateDatabase.Visible = false;
            }
            else
            {
                this.buttonConnectToServer.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Reset;
                this.buttonConnectToServer.ImageIndex = 1;
                this.groupBoxLogin.Enabled = false;
                this.groupBoxServer.Enabled = false;
                if (!this._OnlyLogin)
                    this.comboBoxDatabase.Enabled = true;
                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    if (!this._OnlyLogin)
                        this.comboBoxDatabase.Text = "";
                    this.userControlDialogPanel.buttonOK.Enabled = false;
                    if (this._DatabaseManagementSystem == DatabaseManagementSystem.MSSQL)
                    {
                        if (this._OnlyLogin)
                        {
                            if (this.ServerConnection.ConnectionString.Length > 0)
                            {
                                const string sql = "SELECT dbo.BaseURL()";
                                string baseUrl = "";
                                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ServerConnection.ConnectionString);
                                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(sql, con);
                                try
                                {
                                    con.Open();
                                    baseUrl = c.ExecuteScalar()?.ToString() ?? string.Empty;
                                }
                                catch (System.Exception ex)
                                {
                                    // ignored
                                }
                                finally
                                {
                                    con.Close();
                                    con.Dispose();
                                }

                                this.userControlDialogPanel.buttonOK.Enabled = baseUrl.Length > 0;
                            }
                        }
                        else if (this.ConnectionStringMaster.Length > 0)
                        {
                            System.Data.DataTable dt = this.DatabaseList;
                            if (dt.Columns.Count > 0 && dt.Columns[0].ColumnName == "DatabaseName" && dt.Rows.Count > 0)
                            {
                                this.comboBoxDatabase.DataSource = dt;
                                this.comboBoxDatabase.DisplayMember = "DatabaseName";
                                this.comboBoxDatabase.ValueMember = "DatabaseName";
                                this.comboBoxDatabase.Enabled = true;
                                this.buttonCreateDatabase.Visible = false;
                            }
                            else
                            {
                                string SQL = "SELECT USER_NAME();";
                                string UserName = "";
                                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringMaster);
                                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                                try
                                {
                                    con.Open();
                                    UserName = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                }
                                catch (System.Exception ex) { }
                                finally
                                {
                                    con.Close();
                                    con.Dispose();
                                }
                                bool SysAdmin = UserName == "dbo";

                                if (SysAdmin && dt.Rows.Count == 0 && this._VersionsForCreation != null && this._VersionsForCreation.Count > 0)
                                {
                                    this.buttonCreateDatabase.Visible = true;
                                }
                                this.comboBoxDatabase.Enabled = false;
                                if (dt.Rows.Count == 0)
                                {
                                    string Message = "No available databases";
                                    if (this._VersionsForCreation == null || (this._VersionsForCreation != null && this._VersionsForCreation.Count == 0))
                                        Message += " and script for creation of database is missing";
                                    if (!SysAdmin)
                                        Message += " and creation of databases only available with sysadmin rights";
                                    System.Windows.Forms.MessageBox.Show(Message);
                                }
                            }
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("invalid login");
                            this.comboBoxDatabase.Enabled = false;
                        }
                    }
                    else if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
                    {
                        DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentServer(this.comboBoxServer.Text);
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentPort(int.Parse(this.textBoxPort.Text));
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase("");
                        if (this.radioButtonSQLAuthentication.Checked)
                        {
                            DiversityWorkbench.PostgreSQL.Connection.SetCurrentRole(this.textBoxUser.Text);
                            DiversityWorkbench.PostgreSQL.Connection.Password = this.textBoxPassword.Text;
                        }
                        this.comboBoxDatabase.Items.Clear();
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV in DiversityWorkbench.PostgreSQL.Connection.Databases(this._ModuleName))
                        {
                            this.comboBoxDatabase.Items.Add(KV.Key);
                        }
                        if (this.comboBoxDatabase.Items.Count > 0)
                        {
                            this.comboBoxDatabase.Enabled = true;
                            this.comboBoxDatabase.SelectedIndex = 0;
                        }
                        else
                        {
                            this.comboBoxDatabase.Enabled = false;
                            string ExceptionMessage = "";
                            string conStrTest = DiversityWorkbench.PostgreSQL.Connection.ConnectionString("postgres", ref ExceptionMessage);
                            if (conStrTest.Length > 0)
                            {
                                ConnectionOK = true;
                                this.comboBoxDatabase.Text = "postgres";
                                System.Windows.Forms.MessageBox.Show("No available cache databases\r\nyou are connected to the database postgres", "Connected to postgres", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.userControlDialogPanel.buttonOK.Enabled = true;
                            }
                            else if (ExceptionMessage.Length > 0)
                            {
                                ConnectionOK = false;
                                System.Windows.Forms.MessageBox.Show(ExceptionMessage, "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("No available databases");
                                ConnectionOK = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "ConnectionString: " + this.ConnectionStringWithoutPassword);
                    ConnectionOK = false;
                }
                finally
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
        }

        private bool setDatabaseSource()
        {
            bool OK = false;
            System.Data.DataTable dt = this.DatabaseList;
            if (dt.Columns.Count > 0 && dt.Columns[0].ColumnName == "DatabaseName" && dt.Rows.Count > 0)
            {
                this.comboBoxDatabase.DataSource = dt;
                this.comboBoxDatabase.DisplayMember = "DatabaseName";
                this.comboBoxDatabase.ValueMember = "DatabaseName";
                this.comboBoxDatabase.Enabled = true;
                this.buttonCreateDatabase.Visible = false;
                OK = true;
            }
            return OK;
        }

        private bool ConnectionOK
        {
            set
            {
                if (this.comboBoxDatabase.Text.Length > 0)
                {
                    this.userControlDialogPanel.buttonOK.Enabled = value;
                }
                else
                {
                    this.userControlDialogPanel.buttonOK.Enabled = false;
                }
            }
        }

        private System.Data.DataTable DatabaseList
        {
            get
            {
                System.Data.DataTable dt = new DataTable();
                if (this._DatabaseManagementSystem == DatabaseManagementSystem.MSSQL)
                {
                    if (this.ConnectionStringMaster.Length > 0)
                    {
                        string SQL = "SELECT name as DatabaseName FROM sys.databases where name not in ( 'master', 'model', 'tempdb', 'msdb')"
                            + " AND name LIKE '" + this._ModuleName + "%'" +
                            "AND HAS_DBACCESS(name) = 1 " +
                            " ORDER BY name";
                        
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringMaster);
                        try
                        {
                            ad.Fill(dt);
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                            return dt;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringMaster);
                        con.Open();
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("", con);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            string SQL = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Version]') AND " +
                                "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                                "BEGIN SELECT dbo.Version() END " +
                                "ELSE BEGIN SELECT NULL END";
                            string SqlModule = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND " +
                                "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                                "BEGIN SELECT dbo.DiversityWorkbenchModule() END " +
                                "ELSE BEGIN SELECT NULL END";
                            C.CommandText = SQL;
                            try
                            {
                                if (this._EmptyDatabase)
                                {
                                    string Test = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                    if (Test.Length > 0)
                                        R.Delete();
                                    else
                                    {
                                        C.CommandText = SqlModule;
                                        Test = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                        if (Test.Length > 0)
                                            R.Delete();
                                    }
                                }
                                else
                                {
                                    this._DatabaseVersion = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                    if (this._DatabaseVersion.Length == 0)
                                        R.Delete();
                                    C.CommandText = SqlModule;
                                    string Module = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                    if (this._ModuleName != Module)
                                    {
                                        R.Delete();
                                    }
                                }
                            }
                            catch { R.Delete(); }
                        }
                        con.Close();
                    }
                }
                else if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
                {

                }
                // Toni 20210420: Accept changes to remove deleted entries
                dt.AcceptChanges();
                return dt;
            }
        }

        private string ConnectionStringMaster
        {
            get
            {
                string conStr = "";
                if (this.comboBoxServer.Text.Length > 0)
                {
                    conStr = "Data Source=" + this.comboBoxServer.Text;
                    if (this.textBoxPort.Text.Length > 0) conStr += "," + this.textBoxPort.Text;
                    conStr += ";initial catalog=master;";
                    if (this.radioButtonAuthentication.Checked)
                        conStr += "Integrated Security=True";
                    else
                    {
                        if (this.textBoxUser.Text.Length > 0 && this.textBoxPassword.Text.Length > 0)
                            conStr += "user id=" + this.textBoxUser.Text + ";password=" + this.textBoxPassword.Text;
                        else conStr = "";
                    }
                }
                // MW 2018/10/01: Encrypted connection
                if (conStr.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                    conStr += ";Encrypt=true;TrustServerCertificate=true";
                return conStr;
            }
        }

        #endregion

        #region Properties

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = new ServerConnection();
                S.ModuleName = this._ModuleName;
                S.DatabaseName = this.comboBoxDatabase.Text;
                S.DatabaseServer = this.comboBoxServer.Text;
                S.DatabaseServerPort = System.Int32.Parse(this.textBoxPort.Text);
                S.DatabaseUser = this.textBoxUser.Text;
                S.DatabasePassword = this.textBoxPassword.Text;
                S.IsTrustedConnection = this.radioButtonAuthentication.Checked;
                try { string s = S.ConnectionString; }
                catch { }
                return S;
            }
        }

        public void setHelpProviderNameSpace(string HelpNameSpace, string Keyword)
        {
            try
            {
                this.helpProvider.HelpNamespace = HelpNameSpace;
                this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
                this.helpProvider.SetHelpKeyword(this, Keyword);
            }
            catch { }
        }

        public string FormTitle { set { this.Text = value; } }

        public string Password { get { return this.textBoxPassword.Text; } }
        public string Server { get { return this.comboBoxServer.Text; } }
        public string Database { get { return this.comboBoxDatabase.Text; } }
        public string User { get { return this.textBoxUser.Text; } }
        public bool IsTrusted { get { return this.radioButtonAuthentication.Checked; } }
        public int Port { get { return int.Parse(this.textBoxPort.Text); } }

        //public DatabaseRestriction Database_Restriction
        //{
        //    get { return _DatabaseRestriction; }
        //    set
        //    {
        //        _DatabaseRestriction = value;
        //        if (this._DatabaseRestriction == DatabaseRestriction.Module)
        //            this.radioButtonRestrictToModule.Checked = true;
        //        if (this._DatabaseRestriction == DatabaseRestriction.Version)
        //            this.radioButtonRestrictToVersion.Checked = true;
        //        if (this._DatabaseRestriction == DatabaseRestriction.Any)
        //            this.radioButtonShowAllDatabases.Checked = true;
        //    }
        //}

        #endregion

        #region Database

        private void comboBoxDatabase_TextChanged(object sender, EventArgs e)
        {
            if (!this._EnableKeyControl)
                this.setDatabase();
        }

        private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_EnableKeyControl)
            {
                if (this.comboBoxDatabase.SelectedIndex > -1)
                    this.userControlDialogPanel.buttonOK.Enabled = true;
            }
            else
                this.setDatabase();
        }

        private void comboBoxDatabase_Leave(object sender, EventArgs e)
        {
            if (_EnableKeyControl)
                this.setDatabase();
        }

        private void setDatabase()
        {
            if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
            {
                if (this.comboBoxDatabase.SelectedItem != null && this.comboBoxDatabase.Text.Trim().Length > 0)
                {
                    this.AcceptButton = this.userControlDialogPanel.buttonOK;
                    this.userControlDialogPanel.buttonOK.Enabled = true;
                    this.userControlDialogPanel.buttonOK.Focus();
                }
                else
                    this.userControlDialogPanel.buttonOK.Enabled = false;
            }
            else
            {
                if (this.comboBoxDatabase.SelectedValue == null || this.comboBoxDatabase.SelectedIndex == -1 || this.comboBoxDatabase.Text.Trim().Length == 0)
                {
                    this.userControlDialogPanel.buttonOK.Enabled = false;
                }
                else
                {
                    this.userControlDialogPanel.buttonOK.Enabled = true;
                    this.AcceptButton = this.userControlDialogPanel.buttonOK;
                    this.userControlDialogPanel.buttonOK.Focus();
                }
            }
        }

        private void comboBoxDatabase_KeyDown(object sender, KeyEventArgs e)
        {
            if (_EnableKeyControl && (e.KeyValue == (decimal)System.Windows.Forms.Keys.Down || e.KeyValue == (decimal)System.Windows.Forms.Keys.Up))
            {
                this.comboBoxDatabase.DroppedDown = true;
            }
        }

        #endregion

        #region Previous connections

        //public System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Server> ServerList
        //{
        //    get 
        //    { 
        //        if (this._ServerList == null)
        //        {
        //            this._ServerList = new Dictionary<string, Server>();
        //            foreach(string Connection in DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList)
        //            {
        //                string[] CC = Connection.Split(new char[] { '|' });
        //                string Server = CC[0];
        //                int Port;
        //                int.TryParse(CC[1], out Port);
        //                string ServerKey = Server + "|" + Port.ToString();
        //                string Database = CC[2];
        //                string Role = CC[3];
        //                if (this._ServerList.ContainsKey(ServerKey))
        //                {
        //                    if (this._ServerList[ServerKey].Databases.ContainsKey(Database))
        //                    {
        //                        if (!this._ServerList[ServerKey].Databases[Database].Roles.ContainsKey(Role))
        //                        {
        //                            DiversityWorkbench.PostgreSQL.Role R = new Role(Role, this._ServerList[ServerKey].Databases[Database]);
        //                            this._ServerList[ServerKey].Databases[Database].Roles.Add(Role, R);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        DiversityWorkbench.PostgreSQL.Database D = new Database(Database, this._ServerList[ServerKey]);
        //                        DiversityWorkbench.PostgreSQL.Role R = new Role(Role, D);
        //                        D.Roles.Add(Role, R);
        //                        this._ServerList[ServerKey].Databases.Add(Database, D);
        //                    }
        //                }
        //                else
        //                {
        //                    DiversityWorkbench.PostgreSQL.Server S = new Server(Server, Port);
        //                    DiversityWorkbench.PostgreSQL.Database D = new Database(Database, S);
        //                    DiversityWorkbench.PostgreSQL.Role R = new Role(Role, D);
        //                    D.Roles.Add(Role, R);
        //                    S.Databases.Add(Database, D);
        //                    this._ServerList.Add(ServerKey, S);
        //                }
        //            }
        //        }
        //        return _ServerList; 
        //    }
        //    set { _ServerList = value; }
        //}

        //private void AddConnection(string Server, int Port, string Database, string User)
        //{
        //    string NewConnection = Server + "|" + Port.ToString() + "|" + Database + "|" + User;
        //    if (!DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Contains(NewConnection))
        //        DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Add(NewConnection);
        //}

        //private void RemoveConnection(string Server, int Port, string Database, string User)
        //{
        //    string Connection = Server + "|" + Port.ToString() + "|" + Database + "|" + User;
        //    if (DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Contains(Connection))
        //        DiversityWorkbench.PostgreSQL.Settings.Default.ConnectionList.Remove(Connection);
        //}

        public void initPreviousConnections()
        {
            if (this._PreviousConnections != null && this._PreviousConnections.Count > 0)
            {
                this.toolStripPreviousConnections.Visible = true;
                this.Height += 30;
                try
                {
                    this.toolStripDropDownButtonPreviousConnections.DropDownItems.Clear();

                    System.Collections.Generic.SortedDictionary<string, string[]> SD = new SortedDictionary<string, string[]>();
                    foreach (string CON in this._PreviousConnections)
                    {
                        string[] CC = CON.Split(new char[] { '|' });
                        string Title = CC[0] + ", " + CC[1];
                        if (CC.Length > 2)
                            Title += " as " + CC[2];
                        SD.Add(Title, CC);
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, string[]> KV in SD)
                    {
                        System.Windows.Forms.ToolStripMenuItem MS = new ToolStripMenuItem(KV.Key, null, this.ToolStripMenuItem_Click);
                        MS.Tag = KV.Value;
                        this.toolStripDropDownButtonPreviousConnections.DropDownItems.Add(MS);
                    }

                    //foreach(string CON in this._PreviousConnections)
                    //{
                    //    string[] CC = CON.Split(new char[] { '|' });
                    //    string Title = CC[0] + ", " + CC[1];
                    //    if (CC.Length > 2)
                    //        Title += " as " + CC[2];
                    //    System.Windows.Forms.ToolStripMenuItem MS = new ToolStripMenuItem(Title , null, this.ToolStripMenuItem_Click);
                    //    MS.Tag = CC;
                    //    this.toolStripDropDownButtonPreviousConnections.DropDownItems.Add(MS);
                    //}
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Server> KV in DiversityWorkbench.PostgreSQL.Connection.PreviousConnections())
                    //{
                    //    System.Windows.Forms.ToolStripMenuItem MS = new ToolStripMenuItem(KV.Value.Name + ", " + KV.Value.Port.ToString(), this.imageListPgObjects.Images[0], this.ToolStripMenuItem_Click);
                    //    MS.Tag = KV.Value;
                    //    this.toolStripDropDownButtonPreviousConnections.DropDownItems.Add(MS);
                    //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Role> KV_role in KV.Value.Roles)
                    //    {
                    //        System.Windows.Forms.ToolStripMenuItem MS_role = new ToolStripMenuItem(KV_role.Value.Name, this.imageListPgObjects.Images[2], this.ToolStripMenuItem_Click);
                    //        MS_role.Tag = KV.Value;
                    //        MS.DropDownItems.Add(MS_role);
                    //        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV_DB in KV_role.Value.Databases)
                    //        {
                    //            System.Windows.Forms.ToolStripMenuItem MS_DB = new ToolStripMenuItem(KV_DB.Value.Name, this.imageListPgObjects.Images[1], this.ToolStripMenuItem_Click);
                    //            MS_DB.Tag = KV_DB.Value;
                    //            MS_role.DropDownItems.Add(MS_DB);
                    //        }
                    //    }
                    //    //this.appendHierarchyDatabases(MS);
                    //}
                    //System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(this._QueryCondition.HierarchyParentColumn + " IS NULL", this._QueryCondition.OrderColumn);
                    //for (int i = 0; i < rr.Length; i++)
                    //{
                    //    string Display = rr[i][_QueryCondition.HierarchyDisplayColumn].ToString();
                    //    System.Windows.Forms.ToolStripMenuItem M = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                    //    M.Tag = rr[i];
                    //    this.appendHierarchyChilds(M);
                    //    this.toolStripDropDownButton.DropDownItems.Add(M);
                    //}
                    //this.comboBoxQueryCondition.DataSource = this._QueryCondition.dtValues;
                    //this.comboBoxQueryCondition.DisplayMember = this._QueryCondition.dtValues.Columns[2].ColumnName;
                    //this.comboBoxQueryCondition.ValueMember = this._QueryCondition.dtValues.Columns[0].ColumnName;
                }
                catch (System.Exception ex) { }
            }
            else
                this.toolStripPreviousConnections.Visible = false;
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem M = (System.Windows.Forms.ToolStripMenuItem)sender;
            if (M.Tag != null && this.groupBoxServer.Enabled) // Toni 20210420: Overwrite settings only if input of data is allowed 
            {
                string[] CC = (string[])M.Tag;
                this.comboBoxServer.Text = CC[0];
                this.textBoxPort.Text = CC[1];
                if (CC.Length > 2)
                {
                    this.radioButtonSQLAuthentication.Checked = true;
                    this.textBoxUser.Text = CC[2];
                }
                else
                    this.radioButtonAuthentication.Checked = true;

                //if (M.Tag.GetType() == typeof(DiversityWorkbench.PostgreSQL.Server))
                //{
                //    DiversityWorkbench.PostgreSQL.Server S = (DiversityWorkbench.PostgreSQL.Server)M.Tag;
                //    this.textBoxServer.Text = S.Name;
                //    this.textBoxPort.Text = S.Port.ToString();
                //}
                //else if (M.Tag.GetType() == typeof(DiversityWorkbench.PostgreSQL.Role))
                //{
                //    DiversityWorkbench.PostgreSQL.Role R = (DiversityWorkbench.PostgreSQL.Role)M.Tag;
                //    this.textBoxServer.Text = R.Server.Name;
                //    this.textBoxPort.Text = R.Server.Port.ToString();
                //    this.textBoxRole.Text = R.Name;
                //}
                //else if (M.Tag.GetType() == typeof(DiversityWorkbench.PostgreSQL.Database))
                //{
                //    DiversityWorkbench.PostgreSQL.Database D = (DiversityWorkbench.PostgreSQL.Database)M.Tag;
                //    this.textBoxServer.Text = D.Role.Server.Name;
                //    this.textBoxPort.Text = D.Role.Server.Port.ToString();
                //    this.textBoxDatabase.Text = D.Name;
                //    this.textBoxRole.Text = D.Role.Name;
                //}
                //System.Data.DataRow R = (System.Data.DataRow)M.Tag;
                //this.toolStrip.Tag = R;
                //string ToolTip = this.HierarchyString(R[this._QueryCondition.HierarchyColumn].ToString());
                //if (ToolTip.Length == 0)
                //{
                //    ToolTip = "Select an item from the hierarchy";
                //    this.comboBoxQueryCondition.SelectedIndex = -1;
                //}
                //else
                //{
                //    string ValueColumn = this.comboBoxQueryCondition.ValueMember;
                //    int i = 0;
                //    for (i = 0; i < this._QueryCondition.dtValues.Rows.Count; i++)
                //    {
                //        if (this._QueryCondition.dtValues.Rows[i][ValueColumn].ToString() == R[this._QueryCondition.HierarchyColumn].ToString())
                //            break;
                //    }
                //    this.comboBoxQueryCondition.SelectedIndex = i;
                //}
            }
        }


        //private string HierarchyString(string Key)
        //{
        //    if (Key.Length == 0) return "";
        //    string Hierarchy = "";
        //    //try
        //    //{
        //    //    string Select = this._QueryCondition.HierarchyColumn + " = '" + Key + "'";
        //    //    System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Select, this._QueryCondition.OrderColumn);
        //    //    if (rr.Length > 0)
        //    //    {
        //    //        Hierarchy = this.HierarchyString(rr[0][this._QueryCondition.HierarchyParentColumn].ToString());
        //    //        if (Hierarchy.Length > 0) Hierarchy += " - ";
        //    //        Hierarchy += rr[0][this._QueryCondition.HierarchyDisplayColumn].ToString();
        //    //    }
        //    //}
        //    //catch (System.Exception ex) { }
        //    return Hierarchy;
        //}

        //private string getHierarchyChildValueList(string ParentID)
        //{
        //    string Childs = "";
        //    //try
        //    //{
        //    //    System.Collections.Generic.List<string> ChildIDList = new List<string>();
        //    //    string Restriction = this._QueryCondition.HierarchyParentColumn + " = ";
        //    //    int i;
        //    //    if (!int.TryParse(ParentID, out i))
        //    //        Restriction += "'";
        //    //    Restriction += ParentID;
        //    //    if (!int.TryParse(ParentID, out i))
        //    //        Restriction += "'";
        //    //    System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Restriction);
        //    //    foreach (System.Data.DataRow R in rr)
        //    //    {
        //    //        ChildIDList.Add(R[this._QueryCondition.HierarchyColumn].ToString());
        //    //        this.getHierarchyChildValueList(R[this._QueryCondition.HierarchyColumn].ToString(), ref ChildIDList);
        //    //    }
        //    //    foreach (string s in ChildIDList)
        //    //    {
        //    //        //if (Childs.Length > 0)
        //    //        Childs += ", ";
        //    //        if (!this._QueryCondition.IsNumeric) Childs += "'";
        //    //        Childs += s;
        //    //        if (!this._QueryCondition.IsNumeric) Childs += "'";
        //    //    }
        //    //}
        //    //catch (System.Exception ex) { }
        //    return Childs;
        //}

        //private void getHierarchyChildValueList(string ParentID, ref System.Collections.Generic.List<string> ChildIDList)
        //{
        //    try
        //    {
        //        //string Restriction = this._QueryCondition.HierarchyParentColumn + " = ";
        //        //int i;
        //        //if (!int.TryParse(ParentID, out i)) Restriction += "'";
        //        //Restriction += ParentID;
        //        //if (!int.TryParse(ParentID, out i)) Restriction += "'";

        //        //System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Restriction);
        //        //foreach (System.Data.DataRow R in rr)
        //        //{
        //        //    ChildIDList.Add(R[this._QueryCondition.HierarchyColumn].ToString());
        //        //    this.getHierarchyChildValueList(R[this._QueryCondition.HierarchyColumn].ToString(), ref ChildIDList);
        //        //}
        //    }
        //    catch (System.Exception ex) { }
        //}

        #endregion

        #region Password

        private void checkBoxShowPw_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxShowPw.Checked)
            {
                this.checkBoxShowPw.ImageKey = "Hidden.ico";
                this.textBoxPassword.PasswordChar = '\0';
            }
            else
            {
                this.checkBoxShowPw.ImageKey = "Visible.ico";
                this.textBoxPassword.PasswordChar = '*';
            }
        }

        private void buttonClearPassword_Click(object sender, EventArgs e)
        {
            // Hinweis: buttonClearPassword ist noch da, aber unsichtbar geschaltet.
            // Unter checkBoxShowPw liegt das panelPwButtons, in dem beide Buttons enthalten sind.
            // Bei Bedarf einfach "reaktivieren"....
            //var sha1 = new SHA1CryptoServiceProvider();
            //var sha1data = sha1.ComputeHash(data);
            //DiversityWorkbench.Settings.Password = this.textBoxPassword.Text;
        }

        private enum PasswordState { Saved, Missing }
        private PasswordState _PasswordState = PasswordState.Missing;

        #endregion

        #region Encryption

        private void buttonEncryptConnection_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.IsEncryptedConnection = !DiversityWorkbench.Settings.IsEncryptedConnection;
            this.setEncryptionControls();
        }

        private void setEncryptionControls()
        {
            if (this._DatabaseManagementSystem == DatabaseManagementSystem.Postgres)
                this.buttonEncryptConnection.Visible = false;
            else
            {
                if (DiversityWorkbench.Settings.IsEncryptedConnection)
                {
                    this.buttonEncryptConnection.Image = DiversityWorkbench.Properties.Resources.KeyGreen;
                    this.buttonEncryptConnection.FlatAppearance.BorderSize = 0;
                    this.toolTip.SetToolTip(this.buttonEncryptConnection, "Connection is encrypted");
                }
                else
                {
                    this.buttonEncryptConnection.Image = DiversityWorkbench.Properties.Resources.KeyGray;
                    this.buttonEncryptConnection.FlatAppearance.BorderSize = 1;
                    this.toolTip.SetToolTip(this.buttonEncryptConnection, "Connection is NOT encrypted");
                }
            }
        }

        #endregion

        #region EU DSGVO

        //private enum PrivacyConsent { undecided, consented, rejected }

        //private UserSettings.PrivacyConsent PrivacyConsentState()
        //{
        //    UserSettings.PrivacyConsent Consent = UserSettings.PrivacyConsent.undecided;
        //    string SQL = "select case when PrivacyConsent = 1 then 'consented' else case when PrivacyConsent is null then 'undecided' else 'rejected' end end " +
        //        "from UserProxy where LoginName = SUSER_SNAME()";
        //    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //    if (Result == UserSettings.PrivacyConsent.consented.ToString())
        //        Consent = UserSettings.PrivacyConsent.consented;
        //    else if (Result == UserSettings.PrivacyConsent.rejected.ToString())
        //        Consent = UserSettings.PrivacyConsent.rejected;
        //    return Consent;
        //}

        //private void SetPrivacyConsentState(bool Consented)
        //{
        //    string SQL = "UPDATE U SET PrivacyConsent = ";
        //    if (Consented) SQL += "1";
        //    else SQL += "0";
        //    SQL += " from UserProxy U where LoginName = SUSER_SNAME()";
        //    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        //}

        //private bool ColumnPrivacyConsentDoesExist()
        //{
        //    bool Exists = false;
        //    string SQL = "select count(*) " +
        //        "from INFORMATION_SCHEMA.COLUMNS C " +
        //        "where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'PrivacyConsent'";
        //    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //    if (Result == "1")
        //        Exists = true;
        //    return Exists;
        //}

        #endregion

        #region OpenID
        /*
        http://madskristensen.net/post/openid-implementation-in-csharp-and-aspnet

        1. Webinterface das eine OpenID authentifiziert und bei Erfolg einen
        Benutzer mit Passwort in der Datenbank anlegt. Abspeichern von OpenID,
        Username, Passwort im Klartext in einer sichern Datenbank.

        2. DWB-Client stellt OpenID als Login-Verfahren zur Verfügung.

        3. User gibt im Client seine openID ein. siehe Filmchen:
        http://madskristensen.net/posts/files/OpenID.wmv ...


        4. Anfrage vom Client über Webservice ob OpenID authentifiziert werden
        konnte. Dann wird Benutzername und das Passwort abgerufen. Dabei wird
        wiederum die OpenID autentifiziert. Das ist wichtig, damit man nicht
        einfach alle OpenIDs durchgehen und abrufen kann.

        5. Login in den Server mit den abgerufenen usernamen und passwort.


        Damit wäre ein SSO auch über Server hinweg möglich.

        Problem: Wie verwaltet man die Autorisierung? Wo ist wer Gast, User,
        Editor, Admin?
        Wie immer: Authentisierung: Wer bist du?
        Autorisierung: Ich kenne dich aber was darfst du?

        Hört sich einfach an, aber die Tücke sind die Details...

        Und noch ein paar links zu C# auch von MS...

        http://openid.net/developers/libraries/
         * */

        #endregion

        #region Creation of database
        private void buttonCreateDatabase_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.createDatabase();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void createDatabase()
        {
            // setting the values
            if (this.radioButtonAuthentication.Checked && !Settings.IsTrustedConnection)
                Settings.IsTrustedConnection = this.radioButtonAuthentication.Checked;
            if (Settings.DatabasePort != System.Int32.Parse(this.textBoxPort.Text))
                Settings.DatabasePort = System.Int32.Parse(this.textBoxPort.Text);
            if (Settings.DatabaseServer != this.comboBoxServer.Text)
                Settings.DatabaseServer = this.comboBoxServer.Text;
            if (Settings.DatabaseUser != this.textBoxUser.Text)
                Settings.DatabaseUser = this.textBoxUser.Text;
            if (Settings.Password != this.textBoxPassword.Text)
                Settings.Password = this.textBoxPassword.Text;

            string Server = "";
            if (this.comboBoxServer.Text.Length > 0)
                Server = this.comboBoxServer.Text;
            else
                Server = DiversityWorkbench.Settings.DatabaseServer;
            int Port = DiversityWorkbench.Settings.DatabasePort;

            if (this.textBoxPort.Text.Length > 0)
            {
                int iPort;
                if (int.TryParse(this.textBoxPort.Text, out iPort))
                    Port = iPort;
            }

            if (_VersionsForCreation != null && _VersionsForCreation.Count > 0)
            {
                if (DiversityWorkbench.Database.CreateDatabase(Server, Port, _VersionsForCreation))
                {
                    this.comboBoxDatabase.Text = Settings.DatabaseName;
                    if (this.setDatabaseSource())
                        this.setDatabase();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Sorry, resources are missing");
            }
            //#if DEBUG
            //#else
            //            System.Windows.Forms.MessageBox.Show("available in upcoming version");
            //            return;
            //#endif
        }

        private System.Collections.Generic.Dictionary<string, string> _VersionsForCreation;

        #endregion

        #region Ping
        private void buttonPing_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Ping p = new Ping();
            PingReply r;
            string s;
            s = this.comboBoxServer.Text;
            r = p.Send(s);
            string Message = "";
            if (r.Status == IPStatus.Success)
            {
                Message = "Ping to " + s.ToString() + "[" + r.Address.ToString() + "]" + " Successful"
                   + " Response delay = " + r.RoundtripTime.ToString() + " ms" + "\n";
                // #120
                if (this.TestPort(this.comboBoxServer.Text, int.Parse(this.textBoxPort.Text)))
                    Message += "\nPort " + this.textBoxPort.Text + " is open";
                else
                    Message += "\nPort " + this.textBoxPort.Text + " is closed";
                this.Cursor = System.Windows.Forms.Cursors.Default;
                System.Windows.Forms.MessageBox.Show(Message, "Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                Message = "Failed to contact server " + this.comboBoxServer.Text;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                System.Windows.Forms.MessageBox.Show(Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        /// <summary>
        /// #120
        /// Tests if the port is open
        /// Server: Servername or IP
        /// Port: Portnumber
        /// </summary>
        private bool TestPort(string Server, int Port)
        {
            bool OK = false;
            try
            {
                System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient(Server, Port);
                OK = true;
                tcpClient.Close();
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        #endregion
    }
}
