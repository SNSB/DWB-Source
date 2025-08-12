using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench
{
    public partial class FormDatabaseConnection : Form
    {

        #region Parameter
        private string _ModuleName;
        System.Version _AssemblyVersion;
        private string _DatabaseVersion;
        private bool _IsLocal = false;

        public enum DatabaseRestriction { Module, Version, Any };
        private DatabaseRestriction _DatabaseRestriction = DatabaseRestriction.Module;

        private bool _ForMainModule = true;
        #endregion

        #region Construction

        public FormDatabaseConnection()
        {
            InitializeComponent();

            // set local parameters
            this._ModuleName = Settings.ModuleName;

            // init form values
            this.textBoxPort.Text = Settings.DatabasePort.ToString();
            this.radioButtonRestrictToModule.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Restrict_to + " " + Settings.ModuleName + "...";
            //this.radioButtonRestrictToModule.Text = "Restrict to " + Settings.ModuleName + "...";
            this.textBoxUser.Text = Settings.DatabaseUser;
            this.textBoxPassword.Text = DiversityWorkbench.User.Password;
            this.textBoxLocalFile.Text = Settings.SqlExpressDbFileName;
            this.comboBoxServer.Text = Settings.DatabaseServer;
            //if (Settings.IsLocalExpressDatabase)
            //    this.textBoxLocalFile.Text = Settings.SqlExpressDbFileName;
            //else
            //    this.comboBoxServer.Text = Settings.DatabaseServer;
            this.comboBoxDatabase.Text = Settings.DatabaseName;
            this.radioButtonAuthentication.Checked = Settings.IsTrustedConnection;
            this.radioButtonSQLAuthentication.Checked = !Settings.IsTrustedConnection;

            this.ChangeAuthentication();
            this.userControlDialogPanel.buttonOK.Enabled = false;

            this.labelSelectRemoteLocal.Text = "Choose if the database is \r\nremote or local (SQLExpress)";
            this.labelLocal.Text = "Select the local database file for the database " + this._ModuleName;
            this.radioButtonRemote.Checked = !Settings.IsLocalExpressDatabase;
            this.radioButtonLocal.Checked = Settings.IsLocalExpressDatabase;
            this.switchRemoteLocal();
            this.Database_Restriction = DatabaseRestriction.Module;
        }

        public FormDatabaseConnection(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)
        {
            InitializeComponent();

            try
            {
                this._ModuleName = IWorkbenchUnit.getServerConnection().ModuleName;

                // init form values
                this.textBoxPort.Text = Settings.DatabasePort.ToString();
                //System.Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                //this.radioButtonRestrictToVersion.Text = "Restrict to " + Settings.ModuleName + "... v. " + v.Major.ToString() + "." + v.Minor.ToString();
                this.radioButtonRestrictToModule.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Restrict_to + " " + Settings.ModuleName + "...";
                //this.radioButtonRestrictToModule.Text = "Restrict to " + Settings.ModuleName + "...";
                this.textBoxUser.Text = Settings.DatabaseUser;
                this.textBoxPassword.Text = DiversityWorkbench.User.Password;
                this.comboBoxServer.Text = Settings.DatabaseServer;
                this.comboBoxDatabase.Text = Settings.DatabaseName;
                this.radioButtonAuthentication.Checked = Settings.IsTrustedConnection;
                this.radioButtonSQLAuthentication.Checked = !Settings.IsTrustedConnection;
                this.textBoxLocalFile.Text = Settings.SqlExpressDbFileName;

                this.ChangeAuthentication();
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._ForMainModule = false;
                this.labelSelectRemoteLocal.Text = "Choose if the database is \r\nremote or local (SQLExpress)";
                this.labelLocal.Text = "Select the local database file for the database " + this._ModuleName;
                this.radioButtonRemote.Checked = !Settings.IsLocalExpressDatabase;
                this.radioButtonLocal.Checked = Settings.IsLocalExpressDatabase;
                this.switchRemoteLocal();
                this.Database_Restriction = DatabaseRestriction.Module;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)");
            }
        }

        public FormDatabaseConnection(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, System.Version Version)
            : this(IWorkbenchUnit)
        {
            try
            {
                this._AssemblyVersion = Version;
                this.radioButtonRestrictToVersion.Visible = true;
                this.radioButtonRestrictToVersion.Checked = true;
                this.radioButtonRestrictToVersion.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Restrict_to + " " + Settings.ModuleName + "... v. " +
                    this._AssemblyVersion.Major.ToString() + "." + this._AssemblyVersion.Minor.ToString();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit, System.Version Version)");
            }
        }

        public FormDatabaseConnection(DiversityWorkbench.ServerConnection ServerConnection)
        {
            InitializeComponent();
            try
            {
                this.radioButtonRestrictToVersion.Visible = false;
                this.radioButtonRestrictToModule.Visible = true;
                //this.radioButtonShowAllDatabases.Visible = false;
                //this.radioButtonRestrictToVersion.Visible = true;
                //this.radioButtonRestrictToVersion.Checked = true;
                //this.ServerConnection = ServerConnection;
                if (ServerConnection != null)
                {
                    this.textBoxPort.Text = ServerConnection.DatabaseServerPort.ToString();
                    //this.radioButtonRestrictToVersion.Text = "Restrict to " + ServerConnection.ModuleName;
                    this.radioButtonRestrictToVersion.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Restrict_to + " " + ServerConnection.ModuleName;
                    //this.radioButtonRestrictToModule.Text = "Restrict to " + Settings.ModuleName + "...";
                    this.radioButtonRestrictToModule.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Restrict_to + " " + ServerConnection.ModuleName;
                    this.textBoxUser.Text = ServerConnection.DatabaseUser;
                    this.textBoxPassword.Text = DiversityWorkbench.User.Password;
                    this.comboBoxServer.Text = ServerConnection.DatabaseServer;
                    this.comboBoxDatabase.Text = ServerConnection.DatabaseName;
                    this.radioButtonAuthentication.Checked = ServerConnection.IsTrustedConnection;
                    this.radioButtonSQLAuthentication.Checked = !ServerConnection.IsTrustedConnection;
                    this.textBoxLocalFile.Text = ServerConnection.SqlExpressDbFileName;
                    this._ModuleName = ServerConnection.ModuleName;
                    this.labelLocal.Text = "Select the local database file for the database " + ServerConnection.ModuleName;
                    this.radioButtonRemote.Checked = !ServerConnection.IsLocalExpressDatabase;
                    this.radioButtonLocal.Checked = ServerConnection.IsLocalExpressDatabase;
                }
                this.ChangeAuthentication();
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._ForMainModule = false;
                this.labelSelectRemoteLocal.Text = "Choose if the database is \r\nremote or local (SQLExpress)";
                this.switchRemoteLocal();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(DiversityWorkbench.ServerConnection ServerConnection)");
            }
        }

        public FormDatabaseConnection(System.Version Version)
            : this()
        {
            try
            {
                this._AssemblyVersion = Version;
                this.radioButtonRestrictToVersion.Visible = true;
                this.radioButtonRestrictToVersion.Checked = true;
                this.radioButtonRestrictToVersion.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Restrict_to + " " + Settings.ModuleName + "... v. " +
                    this._AssemblyVersion.Major.ToString() + "." + this._AssemblyVersion.Minor.ToString();
                this.Database_Restriction = DatabaseRestriction.Module;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormDatabaseConnection(System.Version Version)");
            }
        }

        #endregion

        #region Server

        private void comboBoxServer_DropDown(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                if (this.comboBoxServer.DataSource == null)
                {
                    System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
                    System.Data.DataTable dataTable = instance.GetDataSources();
                    if (dataTable.Rows.Count > 0)
                    {
                        this.comboBoxServer.DataSource = dataTable;
                        this.comboBoxServer.DisplayMember = dataTable.Columns[0].ColumnName.ToString();
                        this.comboBoxServer.ValueMember = dataTable.Columns[0].ColumnName.ToString();
                    }
                    else
                        this.comboBoxServer.DataSource = null;
                }
            }
            catch { }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }
        
        private void comboBoxServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxServer.SelectedIndex > -1)
            {
                if (this.comboBoxServer.SelectedValue.ToString() != "")
                {
                    System.Data.DataRowView rv = (System.Data.DataRowView)this.comboBoxServer.SelectedItem;
                }
                this.comboBoxDatabase.DataSource = null;
            }
            ConnectionOK = false;
            this.labelMessage.Text = "Please select a server from the list or type the name or the IP-address of the server";
        }

        #endregion

        #region Form

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void FormDatabaseConnection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (this.radioButtonLocal.Checked)
                {
                    Settings.DatabaseName = this._ModuleName;
                    //Settings.DatabasePort = 1433;
                    //Settings.DatabaseServer = this.textBoxLocalFile.Text;
                    Settings.IsTrustedConnection = true;
                    Settings.IsLocalExpressDatabase = true;
                    Settings.SqlExpressDbFileName = this.textBoxLocalFile.Text;
                }
                else
                {
                    if (this._ForMainModule)
                    {
                        Settings.DatabaseName = this.comboBoxDatabase.Text;
                        Settings.DatabasePort = System.Int32.Parse(this.textBoxPort.Text);
                        Settings.DatabaseServer = this.comboBoxServer.Text;
                        Settings.IsTrustedConnection = this.radioButtonAuthentication.Checked;
                        Settings.IsLocalExpressDatabase = false;
                        if (!this.radioButtonAuthentication.Checked)
                        {
                            Settings.DatabaseUser = this.textBoxUser.Text;
                            Settings.Password = this.textBoxPassword.Text;
                        }
                    }
                    else
                    {
                    }
                }
            }
        }
        
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            this.ConnectionOK = false;
        }

        private void textBoxUser_TextChanged(object sender, EventArgs e)
        {
            this.ConnectionOK = false;
        }

        private string Message(string Resource)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms.FormDatabaseConnectionText));
            string Message = resources.GetString(Resource);
            return Message;
        }

        #endregion

        #region Properties
        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = new ServerConnection();
                S.ModuleName = this._ModuleName;
                if (this.radioButtonLocal.Checked)
                {
                    S.DatabaseName = this._ModuleName;
                    S.SqlExpressDbFileName = this.textBoxLocalFile.Text;
                    S.DatabaseServerPort = 5432;
                    S.DatabaseUser = "";
                    S.DatabasePassword = "";
                    S.IsTrustedConnection = true;
                    S.IsLocalExpressDatabase = this.radioButtonLocal.Checked;
                }
                else
                {
                    S.DatabaseName = this.comboBoxDatabase.Text;
                    S.DatabaseServer = this.comboBoxServer.Text;
                    S.DatabaseServerPort = System.Int32.Parse(this.textBoxPort.Text);
                    S.DatabaseUser = this.textBoxUser.Text;
                    S.DatabasePassword = this.textBoxPassword.Text;
                    S.IsTrustedConnection = this.radioButtonAuthentication.Checked;
                    S.IsLocalExpressDatabase = this.radioButtonLocal.Checked;
                }
                //DiversityWorkbench.ServerConnection.setServerConnection(S);
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
            catch  { }
        }

        public string FormTitle { set { this.Text = value; } }
        
        public DatabaseRestriction Database_Restriction
        {
            get { return _DatabaseRestriction; }
            set 
            { 
                _DatabaseRestriction = value;
                if (this._DatabaseRestriction == DatabaseRestriction.Module)
                    this.radioButtonRestrictToModule.Checked = true;
                if (this._DatabaseRestriction == DatabaseRestriction.Version)
                    this.radioButtonRestrictToVersion.Checked = true;
                if (this._DatabaseRestriction == DatabaseRestriction.Any)
                    this.radioButtonShowAllDatabases.Checked = true;
            }
        }

        #endregion

        #region Database

        private void comboBoxDatabase_TextChanged(object sender, EventArgs e)
        {
            this.setDatabase();
        }

        private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setDatabase();
        }

        private void setDatabase()
        {
            if (this.comboBoxDatabase.SelectedValue == null || this.comboBoxDatabase.SelectedIndex == -1 || this.comboBoxDatabase.Text.Trim().Length == 0)
            {
                this.userControlDialogPanel.buttonOK.Enabled = false;
            }
            else
                this.userControlDialogPanel.buttonOK.Enabled = true;
        }

        #endregion 

        #region Authentication
        
        private void ChangeAuthentication()
        {
            if (this.radioButtonAuthentication.Checked)
            {
                this.textBoxPassword.Enabled = false;
                this.textBoxPassword.BackColor = System.Drawing.SystemColors.ActiveBorder;
                this.textBoxUser.Enabled = false;
                this.textBoxUser.BackColor = System.Drawing.SystemColors.ActiveBorder;
            }
            else
            {
                this.textBoxPassword.Enabled = true;
                this.textBoxPassword.BackColor = System.Drawing.Color.White;
                this.textBoxUser.Enabled = true;
                this.textBoxUser.BackColor = System.Drawing.Color.White;
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
            if (this.textBoxPassword.Text != Settings.Password)
            {
                this.ChangeAuthentication();
                DiversityWorkbench.User.Password = this.textBoxPassword.Text;
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
                return conStr;
            }
        }

        private void buttonConnectToServer_Click(object sender, EventArgs e)
        {
            if (this.buttonConnectToServer.Text == DiversityWorkbench.Forms.FormDatabaseConnectionText.Reset)
            {
                this.comboBoxServer.Enabled = true;
                this.buttonConnectToServer.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Connect_to_server;
                this.buttonConnectToServer.ImageIndex = 2;
                this.textBoxPort.Enabled = true;
                this.groupBoxLogin.Enabled = true;
                this.radioButtonRestrictToVersion.Enabled = true;
                this.radioButtonRestrictToModule.Enabled = true;
                this.radioButtonShowAllDatabases.Enabled = true;
                this.comboBoxDatabase.Enabled = false;
            }
            else
            {
                this.buttonConnectToServer.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Reset;
                this.buttonConnectToServer.ImageIndex = 1;
                this.comboBoxServer.Enabled = false;
                this.textBoxPort.Enabled = false;
                this.groupBoxLogin.Enabled = false;
                this.radioButtonRestrictToVersion.Enabled = false;
                this.radioButtonRestrictToModule.Enabled = false;
                this.radioButtonShowAllDatabases.Enabled = false;
                this.comboBoxDatabase.Enabled = true;
                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.comboBoxDatabase.Text = "";
                    this.userControlDialogPanel.buttonOK.Enabled = false;
                    if (this.ConnectionStringMaster.Length > 0)
                    {
                        System.Data.DataTable dt = this.DatabaseList;
                        if (dt.Columns.Count > 0 && dt.Columns[0].ColumnName == "DatabaseName" && dt.Rows.Count > 0)
                        {
                            this.comboBoxDatabase.DataSource = dt;
                            this.comboBoxDatabase.DisplayMember = "DatabaseName";
                            this.comboBoxDatabase.ValueMember = "DatabaseName";
                            this.comboBoxDatabase.Enabled = true;
                        }
                        else
                        {
                            this.comboBoxDatabase.Enabled = false;
                            if (dt.Rows.Count == 0)
                            {
                                string Message = "No available databases";
                                if (this.radioButtonRestrictToVersion.Checked) Message += "\r\nwith name " + this._ModuleName + "...";
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

        private System.Data.DataTable DatabaseList
        {
            get
            {
                System.Data.DataTable dt = new DataTable();
                if (this.ConnectionStringMaster.Length > 0)
                {
                    string SQL = "SELECT name as DatabaseName FROM sys.databases where name not in ( 'master', 'model', 'tempdb', 'msdb')";
                    if (!this.radioButtonShowAllDatabases.Checked && this._ModuleName != "")
                        SQL = SQL + " AND name LIKE '" + this._ModuleName + "%'";
                    SQL += " ORDER BY name";
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringMaster);
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
                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.ConnectionStringMaster);
                    con.Open();
                    System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand("", con);
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
                            this._DatabaseVersion = C.ExecuteScalar().ToString();
                            if (this._DatabaseVersion.Length == 0)
                                R.Delete();
                            C.CommandText = SqlModule;
                            if (this._ModuleName != C.ExecuteScalar().ToString())
                            {
                                if (this.Database_Restriction == DatabaseRestriction.Module)
                                    R.Delete();
                            }
                            else
                            {
                                try
                                {
                                    if (this.radioButtonRestrictToVersion.Checked && (
                                        this.VersionMajor != this._AssemblyVersion.Major.ToString() ||
                                        this.VersionMinor != this._AssemblyVersion.Minor.ToString()))
                                        R.Delete();
                                }
                                catch { R.Delete(); }
                            }
                        }
                        catch { R.Delete(); }
                    }
                    con.Close();
                }
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
                return conStr;
            }
        }

        //private string ConnectionStringMasterWithoutPassword
        //{
        //    get
        //    {
        //        string conStr = "";
        //        if (this.comboBoxServer.Text.Length > 0)
        //        {
        //            conStr = "Data Source=" + this.comboBoxServer.Text;
        //            if (this.textBoxPort.Text.Length > 0) conStr += "," + this.textBoxPort.Text;
        //            conStr += ";initial catalog=master;";
        //            if (this.radioButtonAuthentication.Checked)
        //                conStr += "Integrated Security=True";
        //            else
        //            {
        //                if (this.textBoxUser.Text.Length > 0 && this.textBoxPassword.Text.Length > 0)
        //                {
        //                    conStr += "user id=" + this.textBoxUser.Text + ";password=";
        //                    for (int i = 0; i < this.textBoxPassword.Text.Length; i++) { conStr += "*"; };
        //                }
        //                else conStr = "";
        //            }
        //        }
        //        return conStr;
        //    }
        //}
        
        #endregion

        #region Version
        private string VersionMajor
        {
            get
            {
                if (this._DatabaseVersion == null) return null;
                else
                {
                    try
                    {
                        string V = this._DatabaseVersion;
                        if (this._DatabaseVersion.IndexOf(".") > -1)
                            V = this._DatabaseVersion.Substring(0, this._DatabaseVersion.IndexOf(".")).Replace("0", "");
                        return V;

                    }
                    catch
                    {
                        return "";
                    }
                }
            }
        }

        private string VersionMinor
        {
            get
            {
                if (this._DatabaseVersion == null) return null;
                else
                {
                    try
                    {
                        string V = this._DatabaseVersion.Substring(this._DatabaseVersion.IndexOf(".") + 1);
                        V = V.Substring(0, this._DatabaseVersion.IndexOf(".")).Replace("0", "");
                        if (V.Length == 0) V = "0";
                        return V;

                    }
                    catch
                    {
                        return "";
                    }
                }
            }
        }
        
        #endregion

        #region obsolete
        private void comboBoxDatabase_DropDown(object sender, System.EventArgs e)
        {
            //try
            //{
            //    this.fillDtDatabases(true);
            //    if (this.dtDatabases.Columns.Count > 0)
            //    {
            //        this.comboBoxDatabase.DataSource = this.dtDatabases;
            //        this.comboBoxDatabase.DisplayMember = "DatabaseName";
            //        this.comboBoxDatabase.ValueMember = "DatabaseName";
            //    }
            //    else
            //    {
            //        this.comboBoxDatabase.DataSource = null;
            //        //if (this.IsTrustedConnection || (this.DatabaseUser != "" && this.Password != ""))
            //        if (this._IsTrustedConnection || (this.textBoxPassword.Text != "" && this.textBoxUser.Text != ""))
            //                System.Windows.Forms.MessageBox.Show("unvalid login");
            //        this.comboBoxDatabase.Text = "";
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK);
            //}
        }

        /// <summary>
        /// filling the datatable holding the databases for the current server
        /// </summary>
        private void fillDtDatabases(bool filtered)
        {
            //filtered = !this.checkBoxShowAllDatabases.Checked;
            //string SQL = "";
            //if (this.dtDatabases == null)
            //    this.dtDatabases = new System.Data.DataTable("Databases");
            //this.dtDatabases.Clear();
            //if (this.comboBoxServer.Text != "" && this._ServerVersion != "")
            //{
            //    if (_ServerVersion == "2000")
            //    {
            //        SQL = "SELECT Catalog_name AS DatabaseName FROM master.information_schema.schemata " +
            //            "WHERE catalog_name not in ('master', 'pubs', 'model', 'msdb', 'Northwind', 'tempdb')";
            //        if (filtered && this._ModuleName != "")
            //            SQL = SQL + " AND catalog_name LIKE '" + this._ModuleName + "%'";
            //        SQL += " ORDER BY Catalog_name";
            //    }
            //    else if (_ServerVersion == "2005")
            //    {
            //        SQL = "SELECT name as DatabaseName FROM sys.databases where owner_sid <> 0x01";
            //        if (filtered && this._ModuleName != "")
            //            SQL = SQL + " AND name LIKE '" + this._ModuleName + "%'";
            //        SQL += " ORDER BY name";
            //    }
            //}
            //else
            //{
            //    SQL = "";
            //}
            //if (this.CheckDatabase("master") == true && SQL != "")
            //{
            //    try
            //    {
            //        if (this.ConnectionString.Length > 0)
            //        {
            //            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.ConnectionString);
            //            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, con);
            //            ad.Fill(this.dtDatabases);
            //        }
            //        // Checking if database is accessible
            //        int l = this.dtDatabases.Rows.Count;
            //        for (int i = l; i > 0; i--)
            //        {
            //            string DB = this.dtDatabases.Rows[i - 1][0].ToString();
            //            if (!this.CheckDatabase(DB))
            //            {
            //                this.dtDatabases.Rows[i - 1].Delete();
            //                this.dtDatabases.AcceptChanges();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex.Source.ToString(), ex.TargetSite.ToString(), ex.Message);
            //    }
            //}
            //if (this.dtDatabases != null)
            //{
            //    if (this.dtDatabases.Rows.Count > 0 && this.comboBoxDatabase.DataSource != null)
            //        this.comboBoxDatabase.SelectedIndex = 0;
            //}
        }

        /// <summary>
        /// Checking if a database can be accessed with the current login
        /// </summary>
        /// <param name="Database">Name of the database</param>
        /// <returns>true if database is accessible, else false</returns>
        private bool CheckDatabase(string Database)
        {
            string SQL = "SELECT 1";
            bool OK = true;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this.ConnectionString.Length > 0)
                    con.ConnectionString = this.ConnectionString;
                else
                {
                    OK = false;
                }
                if (OK)
                {
                    System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return OK;
        }

        private void buttonShowAllDatabases_Click(object sender, System.EventArgs e)
        {
            this.fillDtDatabases(false);
        }

        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //    this.buttonTestConnection.ImageIndex = 2;
            //    //// Server
            //    this._ServerVersion = this.ServerVersion;
            //    //this._ServerVersion = this.ServerVersion;
            //    //if (_ServerVersion != "")
            //    //    this.labelMessage.Text = "Provider:\r\nMicrosoft SQL Server " + _ServerVersion;
            //    //else
            //    //{
            //    //    this.labelMessage.Text = "Please select a server from the list or type the name or the IP-address of the server";
            //    //    return;
            //    //}
            //    if (this._DatabaseServer != this.comboBoxServer.Text)
            //        this._DatabaseServer = this.comboBoxServer.Text;
            //    if (this._DatabaseName == "System.Data.DataRowView" || this._DatabaseName == "" || this._DatabaseName == "master")
            //    {
            //        if (this.dtDatabases == null) this.dtDatabases = new DataTable();
            //        if (this.dtDatabases.Rows.Count == 0)
            //        {
            //            this.fillDtDatabases(true);
            //        }
            //        if (this.dtDatabases.Rows.Count > 0 && this.comboBoxDatabase.Items.Count > 0)
            //        {
            //            this.comboBoxDatabase.SelectedIndex = 0;
            //            this._DatabaseName = this.comboBoxDatabase.SelectedValue.ToString();
            //        }
            //    }
            //    // Database
            //    if (this.ConnectionString.Length > 0)
            //    {
            //        string SQL = "SELECT 1";
            //        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.ConnectionString);
            //        System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            //        con.Open();
            //        C.ExecuteNonQuery();
            //        con.Close();
            //        ConnectionOK = true;
            //    }
            //    else
            //        return;
            //}
            //catch (Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "ConnectionString: " + this.ConnectionString);
            //    ConnectionOK = false;
            //}
            //finally
            //{
            //    this.Cursor = System.Windows.Forms.Cursors.Default;
            //}
        }

        private bool ConnectionOK
        {
            set
            {
                if (this.comboBoxDatabase.Text.Length > 0)
                {
                    this.userControlDialogPanel.buttonOK.Enabled = value;
                    //if (value) this.buttonTestConnection.ImageIndex = 0;
                    //else this.buttonTestConnection.ImageIndex = 1;
                }
                else
                {
                    this.userControlDialogPanel.buttonOK.Enabled = false;
                    //this.buttonTestConnection.ImageIndex = 1;
                }
            }
        }
        
        #endregion        

        #region Local Express files

        private void buttonOpenLocalFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            if (this.textBoxLocalFile.Text.Length > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxLocalFile.Text);
                this.openFileDialog.InitialDirectory = FI.DirectoryName;
            }
            else
                this.openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            this.openFileDialog.Filter = "Database files|*.MDF";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxLocalFile.Text = f.FullName;
                    this.userControlDialogPanel.buttonOK.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void radioButtonRemote_CheckedChanged(object sender, EventArgs e)
        {
            //DiversityWorkbench.Settings.IsLocalExpressDatabase = this.radioButtonLocal.Checked;
            this.switchRemoteLocal();
        }

        private void switchRemoteLocal()
        {
            this.splitContainerMain.Panel1Collapsed = !this.radioButtonRemote.Checked;
            this.splitContainerMain.Panel2Collapsed = this.radioButtonRemote.Checked;
            //DiversityWorkbench.Settings.IsLocalExpressDatabase = this.radioButtonLocal.Checked;
            if (this.radioButtonLocal.Checked)
            {
                this.Height = 180;
                this.Width = 480;
                if (this.textBoxLocalFile.Text.Length > 0)
                {
                    System.IO.FileInfo F = new System.IO.FileInfo(this.textBoxLocalFile.Text);
                    if (F.Exists)
                        this.userControlDialogPanel.buttonOK.Enabled = true;
                }
            }
            else
            {
                this.Height = 450;
                this.Width = 300;
            }
        }
        #endregion

    }
}