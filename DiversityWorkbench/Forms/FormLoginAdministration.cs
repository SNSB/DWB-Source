using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormLoginAdministration : Form
    {

        #region Parameter

        // Toni 20140130 begin: Startup flag for first click on login; System user to reject check boxex fo own login
        private bool startUp = true;
        private string _SystemUser = "";

        public string SystemUser
        {
            get
            {
                if (_SystemUser == "")
                {
                    string SQL = "SELECT SYSTEM_USER";
                    try
                    {
                        _SystemUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                    catch { }
                }
                return _SystemUser;
            }
        }
        // Toni 20140130 end

        private System.Data.DataTable dtProjectsBlocked;
        private System.Data.DataTable dtProjectsAccess;
        private System.Data.DataTable dtProjectsReadOnly;
        private System.Data.DataTable dtProjectsLocked;
        private System.Data.DataTable _dtLogins;
        private System.Data.DataTable _dtDatabases;
        private System.Data.DataTable _dtDatabasesForUserInfo;
        private System.Data.DataTable _dtSecurityAdmin;

        private System.Data.DataTable dtUser;
        private System.Data.DataTable dtRoles;
        private System.Data.DataTable dtUserRoles;
        private Microsoft.Data.SqlClient.SqlConnection _SqlConnection;

        public Microsoft.Data.SqlClient.SqlConnection SqlConnection
        {
            get
            {
                if (this._SqlConnection == null)
                    this._SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                return _SqlConnection;
            }
            set { _SqlConnection = value; }
        }

        //private string User = "";
        private string _LoginName = "";

        private int ProjectID = -1;

        //private System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> _Databases;
        private System.Collections.Generic.List<System.Windows.Forms.TreeNode> _DatabaseNodes;
        private DiversityWorkbench.ServerConnection _ServerConnection;

        private readonly System.Drawing.Color _ColorDatabaseLoginLocked = System.Drawing.Color.Red;
        private readonly System.Drawing.Color _ColorDatabaseLoginWindows = System.Drawing.Color.DarkBlue;
        private readonly System.Drawing.Color _ColorDatabaseLoginSql = System.Drawing.Color.DarkGreen;

        private readonly System.Drawing.Color _ColorDatabaseUser = System.Drawing.Color.Black;
        private readonly System.Drawing.Color _ColorDatabaseNoUser = System.Drawing.Color.LightGray;
        //private readonly System.Drawing.Color _ColorDatabaseNoProject = System.Drawing.Color.Black;
        //private readonly System.Drawing.Color _ColorDatabaseWithProject = System.Drawing.Color.Green;

        //private enum AccessToDatabase { NoUser, User, NoProjectsExist };
        private bool _UserHasAccessToDatabase;
        private bool _ProjectsDoExist;

        private string _LoginAgentURI = "";
        private string _LoginAgentName = "";
        //private string _LoginAgentDatabase = "";

        private System.Windows.Forms.BindingSource _AgentBindingSource;
        private System.Data.DataSet _DsUserProxy;
        private System.Data.DataTable _DtUserProxyAgentInfo;
        private Microsoft.Data.SqlClient.SqlDataAdapter _adUserProxy;

        private System.Windows.Forms.BindingSource _ProjectBindingSource;
        private System.Data.DataSet _DsProjectProxy;
        private System.Data.DataTable _dtProjectProxy;
        private Microsoft.Data.SqlClient.SqlDataAdapter _adProjectProxy;

        private bool _ForSingleDatabase = false;

        private bool _ListAllModules = false;

        private bool _LockingAvailable = false;

        #endregion

        #region Construction

        /// <summary>
        /// Administration of the logins on a server for all Diversity Workbench databases
        /// </summary>
        public FormLoginAdministration()
        {
            InitializeComponent();
            this.initForm();
        }

        /// <summary>
        /// Administration of the logins on a server for a single database
        /// </summary>
        public FormLoginAdministration(Microsoft.Data.SqlClient.SqlConnection Connection)
        {
            InitializeComponent();
            this._ForSingleDatabase = true;
            this._SqlConnection = Connection;
            this._ServerConnection = new ServerConnection(Connection.ConnectionString);
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {
            this.initLogins();
            this.initDatabaseTree();
            this.groupBoxLogin.Enabled = false;
            this.toolStripButtonLoginCreate.Visible = UserIsSecurityAdmin;
            this.toolStripButtonLoginCopy.Visible = UserIsSecurityAdmin;
            this.toolStripButtonLoginDelete.Visible = UserIsSecurityAdmin;
            ///TODO Entfernen fuer neue Version
#if !DEBUG
            this.toolStripButtonLoginMissing.Visible = false;
#endif
#if DEBUG
            this.initPrivayInfo();
#endif
        }

        public void setHelpProvider(string HelpNamespace, string Keyword)
        {
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpKeyword(this, Keyword);
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
            //DiversityWorkbench.Feedback.SendFeedback(
            //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
            //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
            //    null,
            //    null);
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Server

        private void buttonShowCurrentActivity_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.ShowServerActiviy(this.Width, this.Height);
            //string SQL = "sp_who2";
            //System.Data.DataTable dt = new DataTable();
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dt);
            //foreach (System.Data.DataRow R in dt.Rows)
            //{
            //    if (R["Hostname"].ToString().Replace(".", "").Trim().Length == 0
            //        || R["DBName"].ToString() == "msdb")
            //        R.Delete();
            //}
            //dt.AcceptChanges();
            //System.Windows.Forms.Form F = new Form();

            //Bitmap bmp = (Bitmap)this.buttonShowCurrentActivity.Image;
            //Bitmap newBmp = new Bitmap(bmp);
            //Bitmap targetBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), System.Drawing.Imaging.PixelFormat.Format64bppArgb);
            //IntPtr Hicon = targetBmp.GetHicon();
            //Icon myIcon = Icon.FromHandle(Hicon);

            //F.Icon = myIcon;
            //System.Windows.Forms.DataGridView G = new DataGridView();
            //G.DataSource = dt;
            //G.ReadOnly = true;
            //G.AllowUserToAddRows = false;
            //G.RowHeadersVisible = false;
            //F.Controls.Add(G);
            //F.Text = "Current activity on server";
            //F.Width = 100 * dt.Columns.Count + 50;
            //F.Height = 30 * dt.Rows.Count + 60;
            //F.StartPosition = FormStartPosition.CenterParent;
            //F.Text += " " + DiversityWorkbench.Settings.DatabaseServer;
            ////F.ShowIcon = false;
            //G.Dock = DockStyle.Fill;
            //G.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            //F.ShowDialog();
        }

        private void buttonLinkedServer_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;

            DiversityWorkbench.Forms.FormLinkedServer f = new FormLinkedServer();
            f.setHelp("Linked server");
            f.ShowDialog();
        }

        #endregion

        #region Logins

        private void initLogins()
        {
            this.treeViewLogins.Nodes.Clear();
            foreach (System.Data.DataRow R in this.DtLogins.Rows)
            {
                System.Windows.Forms.TreeNode N = new TreeNode(R[0].ToString());
                N.Tag = R;
                if (R["is_disabled"].ToString().ToLower() == "true" || R["is_disabled"].ToString() != "0") // Toni 20140130 nicht bool sondern int!
                    N.ForeColor = this._ColorDatabaseLoginLocked;
                else if (R["type"].ToString() == "U")
                    N.ForeColor = this._ColorDatabaseLoginWindows;
                else N.ForeColor = this._ColorDatabaseLoginSql;
                this.treeViewLogins.Nodes.Add(N);
            }
            this.treeViewLogins.ExpandAll();
        }

        public System.Data.DataTable DtLogins
        {
            get
            {
                if (this._dtLogins == null)
                {
                    string SQL = "use master; " +
                        "select 'dbo' as name, 0 as is_disabled, 'S' as type " +
                        "union " +
                        "select p.name, p.is_disabled, p.type " +
                        "from sys.server_principals p " +
                        "where p.name not like '##%##' " +
                        "and p.type in ('S', 'U') " +
                        "and p.name not like 'NT-%' " +
                        "and p.name <> 'sa'  ";
                    if (this._Filter.Length > 0)
                        SQL += "and p.name like '" + this._Filter + "' ";
                    SQL += "order by type desc, name;";
                    this._dtLogins = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(this._dtLogins);
                    }
                    catch (System.Exception ex) { }
                }
                return _dtLogins;
            }
            //set { _dtLogins = value; }
        }

        private string _Filter = "";

        private void toolStripButtonFilter_Click(object sender, EventArgs e)
        {
            this._Filter = this.toolStripTextBoxFilter.Text;
            this._dtLogins = null;
            this.initLogins();
        }


        public System.Data.DataTable DtSecurityAdmin
        {
            get
            {
                if (this._dtSecurityAdmin == null)
                {
                    this._dtSecurityAdmin = new DataTable();
                    string SQL = "sp_helpsrvrolemember 'sysadmin'";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtSecurityAdmin);
                }
                return _dtSecurityAdmin;
            }
        }

        private void treeViewLogins_Click(object sender, EventArgs e)
        {
            this.groupBoxLogin.Enabled = true;
            // Toni 20140130 begin: Initialize users after first click on login
            if (startUp && treeViewLogins.SelectedNode != null)
            {
                startUp = false;
                treeViewLogins_AfterSelect(sender, null);
            }
            // Toni 20140130 end
        }

        private void treeViewLogins_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.groupBoxLogin.Enabled)
            {
                this.LoginAgentURI = "";
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.tabControlUserDetails.Enabled = false;
                this.groupBoxUser.Enabled = false;
                this.setDatabasesAccordingToLogin();
                this.groupBoxLogin.Text = this.CurrentLogin();
                this.groupBoxLogin.ForeColor = this.treeViewLogins.SelectedNode.ForeColor;
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewLogins.SelectedNode.Tag;
                if (R[2].ToString() == "S" && R[0].ToString() != "dbo")
                {
                    this.toolStripButtonLoginCopy.Visible = UserIsSecurityAdmin;
                    this.toolStripButtonLoginDelete.Visible = UserIsSecurityAdmin;
                }
                else
                {
                    this.toolStripButtonLoginCopy.Visible = false;
                    this.toolStripButtonLoginDelete.Visible = false;
                }
                if (R[0].ToString() == "dbo")
                {
                    this.buttonChangePW.Visible = false;
                    this.buttonLoginOverview.Visible = false;
                    this.buttonLoginStatistics.Visible = false;
                    this.labelDefaultDB.Visible = false;
                    this.textBoxDefaultDB.Visible = false;
                    this.labelDbUserInfo.Visible = false;
                    this.comboBoxDBUserInfo.Visible = false;
                }
                else
                {
                    this.buttonChangePW.Visible = true;
                    this.buttonLoginOverview.Visible = true;
                    this.buttonLoginStatistics.Visible = true;
                    this.labelDefaultDB.Visible = true;
                    this.textBoxDefaultDB.Visible = true;
                    this.labelDbUserInfo.Visible = true;
                    this.comboBoxDBUserInfo.Visible = true;
                }
                // Toni 20140130 begin: Initialize access to check boxes
                this.checkBoxLoginHasAccess.Enabled = UserIsSecurityAdmin;
                this.checkBoxSecurityAdmin.Enabled = UserIsSecurityAdmin;
                // Toni 20140130 end
                if (this.LoginIsDisabled)
                {
                    this.checkBoxLoginHasAccess.Checked = false;
                    this.pictureBoxLogin.Image = this.imageListLogin.Images[1];
                    this.checkBoxSecurityAdmin.Enabled = false;
                    this.buttonChangePW.Enabled = false;
                }
                else
                {
                    this.checkBoxLoginHasAccess.Checked = true;
                    this.pictureBoxLogin.Image = this.imageListLogin.Images[0];
                    this.checkBoxSecurityAdmin.Enabled = UserIsSecurityAdmin;
                    this.buttonChangePW.Enabled = true;
                }
                // Toni 20140130 begin: Don't allow check boxex for own account and dbo
                if (this.LoginName == SystemUser || this.LoginName == "dbo")
                {
                    this.checkBoxLoginHasAccess.Enabled = false;
                    this.checkBoxSecurityAdmin.Enabled = false;
                }
                // Toni 20140130 end
                this.checkBoxSecurityAdmin.Checked = this.LoginIsSecurityAdmin;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            this.treeViewDatabases.SelectedNode = null;
            this.treeViewDatabases_AfterSelect(null, null);
        }

        private string CurrentLogin()
        {
            try
            {
                if (this.treeViewLogins.SelectedNode != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewLogins.SelectedNode.Tag;
                    this.LoginName = R[0].ToString();
                    return R[0].ToString();
                }
                else
                {
                    this.LoginName = "";
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                this.LoginName = "";
                return "";
            }

        }

        //    public void CreateLogin(string Login, string Password, string Database)
        //    {
        //        string SQL = "CREATE LOGIN " + Login +
        //            " WITH PASSWORD = '" + Password + "', DEFAULT_DATABASE=[" + Database + "], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON;" +
        //            " USE " + Database + "; " +
        //            " CREATE USER " + Login + " FOR LOGIN " + Login + " WITH DEFAULT_SCHEMA=[dbo]; GO ";

        //        /*
        //         * Funktion von P.Grobe:
        //         * 
        //         * 	SET NOCOUNT ON;
        //DECLARE @ErrorMessage nvarchar(4000),  @ErrorSeverity int;
        //DECLARE @DropUserStatement nvarchar(200) = 'DROP USER  ' + @LoginName;
        //DECLARE @DefaultRole nvarchar(200) = 'Editor';

        //IF @Action='Create' BEGIN
        //    BEGIN TRY
        //        DECLARE @CreateDB_UserStatement nvarchar(200);
        //        SET @CreateDB_UserStatement = 'CREATE USER ' +	@LoginName + ' FOR LOGIN ' + @LoginName;
        //        EXEC sp_executesql @CreateDB_UserStatement;
        //    END TRY
        //    BEGIN CATCH
        //        SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY();
        //        RAISERROR(@ErrorMessage, @ErrorSeverity, 1);
        //    END CATCH;

        //    BEGIN TRY
        //        -- DECLARE @UserRolesStatement nvarchar(200) = 'ALTER ROLE '+@DefaultRole+' ADD MEMBER ' + @LoginName; -- geht nicht, wegen reserviertem Wort `User`!?!
        //        EXEC sp_addrolemember @DefaultRole, @LoginName;
        //    END TRY
        //    BEGIN CATCH
        //        SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY();
        //        EXEC sp_executesql @DropUserStatement;
        //        RAISERROR(@ErrorMessage, @ErrorSeverity, 1);
        //    END CATCH;
        //END ELSE BEGIN
        //    IF @Action='Remove' BEGIN
        //        EXEC sp_droprolemember @DefaultRole, @LoginName;
        //        EXEC sp_executesql @DropUserStatement;
        //    END
        //END

        //         * */
        //    }

        private void toolStripButtonLoginCreate_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Login L = new Login("");
            this.CreateLogin(ref L);
            L.SetLinkToDiversityAgents(DiversityWorkbench.Settings.ServerConnection);
        }

        private string CreateLogin(ref DiversityWorkbench.Login Login)
        {
            string LoginName = "";
            string Database = DiversityWorkbench.Settings.DatabaseName;
            DiversityWorkbench.Forms.FormCreateLogin f = new FormCreateLogin(Database);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Login = f.Login();
                    LoginName = f.LoginName;
                    string SQL = "use master; " +
                        "select p.name, p.is_disabled, p.type " +
                        "from sys.server_principals p " +
                        "where p.name = '" + f.LoginName + "' ;";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtLogins);
                    System.Data.DataRow[] rr = this._dtLogins.Select("name = '" + f.LoginName + "'");
                    System.Windows.Forms.TreeNode N = new TreeNode(f.LoginName);
                    N.Tag = rr[0];
                    if (f.IsSqlServerLogin)
                        N.ForeColor = this._ColorDatabaseLoginSql;
                    else N.ForeColor = this._ColorDatabaseLoginWindows;
                    this.treeViewLogins.Nodes.Add(N);
                    this.treeViewLogins.SelectedNode = N;
                    Login.SetLinkToDiversityAgents(DiversityWorkbench.Settings.ServerConnection);
                }
                catch (Exception ex)
                {
                    LoginName = "";
                }
            }
            return LoginName;
        }

        private void toolStripButtonLoginCopy_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;

            try
            {
                // getting the values of the template
                DiversityWorkbench.Login SourceLogin = new Login(this.CurrentLogin());

                DiversityWorkbench.Login Target = new Login("");
                this.CreateLogin(ref Target);
                string TargetName = Target.LoginName();// this.CreateLogin();
                if (TargetName.Length > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                        {
                            if (KV.Value.ServerConnection.ModuleName != KVconn.Value.ModuleName)
                                continue;
                            if (KV.Value.ServerConnectionList()[KVconn.Key].IsAddedRemoteConnection)
                                continue;

                            SourceLogin.CopyDatabaseSettings(KVconn.Value, Target);
                        }
                    }
                    //this.initForm();
                    this.treeViewLogins_AfterSelect(null, null);
                    System.Windows.Forms.MessageBox.Show("Login " + Target + " created");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        ///  Transfer all setting from a source login to a target 
        ///  Not transferred will be if the login is disabled and if the login is a system administrator
        /// </summary>
        /// <param name="LoginSource">The source login for the settigs</param>
        /// <param name="LoginTarget">The target login where the settings should be transferred to</param>
        /// <returns></returns>
        //private bool TransferLoginSetting(string LoginSource, string LoginTarget)
        //{
        //    bool OK = true;

        //    try
        //    {
        //        // setting the default database
        //        string SQL = "SELECT LOGINPROPERTY('" + LoginSource + "', 'DefaultDatabase')";
        //        string DefaultDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //        SQL = "Exec sp_defaultdb @loginame='" + LoginTarget + "', @defdb='" + DefaultDB + "'";
        //        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);

        //        DiversityWorkbench.Login _LoginSource = new Login(LoginSource);

        //        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
        //        {
        //            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
        //            {
        //                if (KV.Value.ServerConnection.ModuleName != KVconn.Value.ModuleName)
        //                    continue;
        //                if (KV.Value.ServerConnectionList()[KVconn.Key].IsAddedRemoteConnection)
        //                    continue;

        //                if (_LoginSource.LoginHasAccessToDatabase(KVconn.Value))
        //                {
        //                    System.Data.DataTable Roles = _LoginSource.UserRoles(KVconn.Value);
        //                    foreach (System.Data.DataRow R in Roles.Rows)
        //                    {
        //                    }
        //                    System.Data.DataTable Projects = _LoginSource.AccessibleProjects(KVconn.Value);
        //                    foreach (System.Data.DataRow R in Projects.Rows)
        //                    {
        //                    }
        //                    System.Data.DataTable ReadOnly = _LoginSource.ReadOnlyProjects(KVconn.Value);
        //                    foreach (System.Data.DataRow R in ReadOnly.Rows)
        //                    {
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OK = false;
        //    }
        //    return OK;
        //}

        private void toolStripButtonLoginDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Login LoginToDelete = new Login(this.CurrentLogin());
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                    {
                        if (KV.Value.ServerConnection.ModuleName != KVconn.Value.ModuleName)
                            continue;
                        if (KV.Value.ServerConnectionList()[KVconn.Key].IsAddedRemoteConnection)
                            continue;
                        // Markus 5.6.23: Kein Löschen auf Linked Server
                        if (KVconn.Value.LinkedServer.Length > 0)
                            continue;

                        LoginToDelete.DeleteDatabaseUser(KVconn.Value);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            string SQL = "USE [master]; DROP LOGIN [" + this.CurrentLogin() + "];";
            string Message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
            {
                this._dtLogins = null;
                this.initForm();
            }
            else
            {
                if (Message.Length > 0)
                    Message = "Deleting login failed:\r\n" + Message;
                else Message = "Deleting login failed";
                System.Windows.Forms.MessageBox.Show(Message);
            }

            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;
        }

        #endregion

        #region Login

        private void checkBoxLoginHasAccess_CheckedChanged(object sender, EventArgs e)
        {
            //this.LoginIsDisabled = !this.checkBoxLoginHasAccess.Checked;
        }

        private void setLoginInfos(string Login)
        {
            //if (Login.IndexOf("\\") > -1)
            //    Login = Login.Substring(Login.IndexOf("\\") + 1);
            this.labelLoginInfo.Text = "";
            this.textBoxDefaultDB.Text = "";
            string SQL = "SELECT LOGINPROPERTY('" + Login + "', 'DefaultDatabase')";
            string Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            int i;
            this.textBoxDefaultDB.Text = Info;
            SQL = "select LOGINPROPERTY('" + Login + "','BadPasswordCount')";
            Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (int.TryParse(Info, out i) && i > 0)
            {
                this.labelLoginInfo.Text = i.ToString() + " failed logins. ";
                SQL = "select convert(varchar(10), LOGINPROPERTY('" + Login + "','BadPasswordTime'),  120)";
                Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Info.Length > 0)
                    this.labelLoginInfo.Text += "Last trial: " + Info + ". ";
            }
            SQL = "select convert(varchar(10), LOGINPROPERTY('" + Login + "','PasswordLastSetTime'),  120)";
            Info = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Info.Length > 0)
                this.labelLoginInfo.Text += "Last change of PW: " + Info + ". ";
        }

        private void checkBoxLoginHasAccess_Click(object sender, EventArgs e)
        {
            if (this.LoginIsDisabled)
                this.LoginIsDisabled = false;
            else
                this.LoginIsDisabled = true;
            //this.LoginIsDisabled = !this.checkBoxLoginHasAccess.Checked;
            this.groupBoxLogin.ForeColor = this.treeViewLogins.SelectedNode.ForeColor; // Toni 20140130 actualize colours
            // Toni 20140130 begin: Don't allow check boxes for own account and dbo
            if (this.LoginName == SystemUser || this.LoginName == "dbo")
            {
                this.checkBoxLoginHasAccess.Enabled = false;
                this.checkBoxSecurityAdmin.Enabled = false;
            }
            else
            {
                this.checkBoxLoginHasAccess.Enabled = UserIsSecurityAdmin;
                this.checkBoxSecurityAdmin.Enabled = UserIsSecurityAdmin;
            }
            // Toni 20140130 end
        }

        private void checkBoxSecurityAdmin_Click(object sender, EventArgs e)
        {
            if (this.LoginIsSecurityAdmin)
                this.LoginIsSecurityAdmin = false;
            else
                this.LoginIsSecurityAdmin = true;
            this.checkBoxSecurityAdmin.Checked = this.LoginIsSecurityAdmin; // Toni 20140130 actualize display
        }

        private bool? _userIsSecurityAdmin = null;
        private bool UserIsSecurityAdmin
        {
            get
            {
                if (_userIsSecurityAdmin == null)
                {
                    try
                    {
                        System.Data.DataRow[] rr = this.DtSecurityAdmin.Select("MemberName = '" + this.SystemUser + "'");
                        if (rr.Length > 0)
                        {
                            _userIsSecurityAdmin = true;
                        }
                        //else this.checkBoxSecurityAdmin.ForeColor = System.Drawing.Color.Gray; // Toni 20140130 don't change colour!
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return _userIsSecurityAdmin == true;
            }
        }

        private bool LoginIsSecurityAdmin
        {
            get
            {
                bool IsAdmin = false;
                try
                {
                    System.Data.DataRow[] rr = this.DtSecurityAdmin.Select("MemberName = '" + this._LoginName + "'");
                    if (rr.Length > 0)
                    {
                        IsAdmin = true;
                    }
                    //else this.checkBoxSecurityAdmin.ForeColor = System.Drawing.Color.Gray; // Toni 20140130 don't change colour!
                }
                catch (Exception ex)
                {
                }
                return IsAdmin;
            }
            set
            {
                if (value)
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you really want to allow the login\r\n" + this.CurrentLogin() + "\r\nto administrate the access to the server?", "Security admin?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;
                }
                string SQL = "";
                if (value)
                    SQL = "sp_addsrvrolemember '" + this.CurrentLogin() + "', 'sysadmin' ";
                else SQL = "sp_dropsrvrolemember '" + this.CurrentLogin() + "', 'sysadmin' ";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                {
                    this._dtSecurityAdmin = null;
                    this.checkBoxSecurityAdmin.Checked = this.LoginIsSecurityAdmin;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Operation failed");
                    this.checkBoxSecurityAdmin.Checked = this.LoginIsSecurityAdmin;
                }
            }
        }

        private bool LoginIsDisabled
        {
            get
            {
                if (this.treeViewLogins.SelectedNode.Tag == null)
                    return false;
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewLogins.SelectedNode.Tag;
                if (R["is_disabled"].ToString().ToLower() == "false" || R["is_disabled"].ToString() == "0")
                    return false;
                else
                    return true;
            }
            set
            {
                string SQL = "ALTER LOGIN [" + this.CurrentLogin() + "] ";
                if (!value) // Toni 20140130 expression inverted...
                    SQL += "ENABLE;";
                else
                    SQL += "DISABLE;";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    if (value)
                    {
                        System.Data.DataRow R = (System.Data.DataRow)this.treeViewLogins.SelectedNode.Tag; // Toni 20140130 Update data in tag
                        R["is_disabled"] = 1; // Toni 20140130 Update data in tag
                        this.treeViewLogins.SelectedNode.ForeColor = this._ColorDatabaseLoginLocked;
                        this.pictureBoxLogin.Image = this.imageListLogin.Images[1]; // Toni 20140130 Image index corrected
                    }
                    else
                    {
                        System.Data.DataRow R = (System.Data.DataRow)this.treeViewLogins.SelectedNode.Tag;
                        R["is_disabled"] = 0; // Toni 20140130 Update data in tag
                        if (R["type"].ToString() == "U")
                            this.treeViewLogins.SelectedNode.ForeColor = this._ColorDatabaseLoginWindows;
                        else this.treeViewLogins.SelectedNode.ForeColor = this._ColorDatabaseLoginSql;
                        this.pictureBoxLogin.Image = this.imageListLogin.Images[0]; // Toni 20140130 Image index corrected
                    }
                }
                catch (System.Exception ex) { }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public string LoginName
        {
            get { return _LoginName; }
            set
            {
                _LoginName = value;
                this.setLoginInfos(value);
            }
        }

        /// <summary>
        /// Create a new login on the server
        /// </summary>
        /// <returns>If the creation was successful</returns>
        //private bool CreateLogin()
        //{
        //    string Login = this.NewLoginName();
        //    if (Login.Length == 0)
        //        return false;
        //    string Password = "";
        //    DiversityWorkbench.Forms.FormGetString f = new FormGetString("Password", "Please enter the password for the new login", "");
        //    if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
        //        Password = f.String;
        //    else return false;
        //    string Database = "";
        //    DiversityWorkbench.Forms.FormGetStringFromList fd = new DiversityWorkbench.Forms.FormGetStringFromList(this.DtDatabases, "Please select the default database");
        //    if (fd.DialogResult == System.Windows.Forms.DialogResult.OK && fd.String.Length > 0)
        //        Database = fd.String;
        //    else return false;
        //    string SQL = "use master; CREATE LOGIN " + Login +
        //        " WITH PASSWORD = '" + Password + "', DEFAULT_DATABASE=[" + Database + "], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON;";
        //    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
        //        return true;
        //    return false;
        //}

        /// <summary>
        /// Ask the user to provide a name for the login and test if this name is allready used
        /// </summary>
        /// <returns>The name of the login</returns>
        private string NewLoginName()
        {
            string Login = "";
            DiversityWorkbench.Forms.FormGetString f = new FormGetString("New login", "Please enter the name for the new login", "");
            while (Login.Length == 0)
            {
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.Data.DataRow[] rr = this.DtLogins.Select("name = '" + f.String + "'");
                    if (rr.Length == 0)
                        Login = f.String;
                    else
                        System.Windows.Forms.MessageBox.Show("The name " + f.String + " is allready used");
                }
                else return "";
            }
            return "";
        }

        private bool DeleteLogin()
        {
            return false;
        }

        public string LoginAgentURI
        {
            get
            {
                // Markus 18.6.24: Checking existence before getting infos
                if (this._LoginAgentURI.Length == 0)
                {
                    if (this.DatabaseForUserInfo.Length > 0)
                    {
                        string SQL = this.SqlLoginAgentInfo(AgentInfo.Count); 
                        //"USE " + this.DatabaseForUserInfo + "; " +
                        //    "SELECT  U.AgentURI " +
                        //    "FROM UserProxy U, Agent A " +
                        //    "WHERE dbo.BaseURL() + CAST( A.AgentID as varchar) = U.AgentURI " +
                        //    "AND U.LoginName = " +
                        //    "N'" + this.LoginName + "'";
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.Settings.Connection);
                        if (DiversityWorkbench.Settings.Connection.State == ConnectionState.Closed)
                            DiversityWorkbench.Settings.Connection.Open();
                        try
                        {
                            int i;
                            bool AgentInfoExists = int.TryParse(C.ExecuteScalar()?.ToString(), out i) && i > 0;
                            if (AgentInfoExists)
                            {
                                C.CommandText = this.SqlLoginAgentInfo(AgentInfo.URI);
                                this._LoginAgentURI = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                C.CommandText = this.SqlLoginAgentInfo(AgentInfo.Name);
                                //C.CommandText = "USE " + this.DatabaseForUserInfo + "; " +
                                //"SELECT  A.AgentName " +
                                //"FROM UserProxy U, Agent A " +
                                //"WHERE dbo.BaseURL() + CAST( A.AgentID as varchar) = U.AgentURI " +
                                //"AND U.LoginName = " +
                                //"N'" + this.LoginName + "'";
                                this._LoginAgentName = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            this._LoginAgentName = "";
                            this._LoginAgentURI = "";
                        }
                    }
                }
                return _LoginAgentURI;
            }
            set
            {
                _LoginAgentURI = value;
                if (this._LoginAgentURI.Length == 0)
                {
                    this._LoginAgentName = "";
                }
            }
        }

        private enum AgentInfo{Count, URI, Name}
        private string SqlLoginAgentInfo(AgentInfo agentInfo)
        {
            string SQL = "USE " + this.DatabaseForUserInfo + "; " +
                "SELECT ";
            switch (agentInfo)
            {
                case AgentInfo.Count:
                    SQL += " COUNT(*) ";
                    break;
                case AgentInfo.URI:
                    SQL += " U.AgentURI ";
                    break;
                case AgentInfo.Name:
                    SQL += " A.AgentName ";
                    break;
            }
            SQL += "FROM UserProxy U, Agent A " +
                "WHERE dbo.BaseURL() + CAST( A.AgentID as varchar) = U.AgentURI " +
                "AND U.LoginName = " +
                "N'" + this.LoginName + "'";
            return SQL;
        }

        private string DatabaseForUserInfo
        {
            get
            {
                string DB = "";
                DB = this.comboBoxDBUserInfo.Text;
                return DB;
            }
        }

        private void buttonChangePW_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormPassword f = new FormPassword(this._LoginName);
                f.ShowDialog();
            }
            catch (System.Exception ex) { }
        }

        private void buttonLoginStatistics_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormLoginStatistics f = new FormLoginStatistics(this._LoginName);
            f.ShowDialog();
        }

        private void buttonLoginOverview_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityWorkbench.Forms.FormLoginOverview f = new FormLoginOverview(this.CurrentLogin());
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();
        }

        #endregion

        #region Databases

        private void initDatabaseTree()
        {
            this.treeViewDatabases.Nodes.Clear();
            this._DatabaseNodes = new List<TreeNode>();
            if (this._ForSingleDatabase)
            {
                System.Drawing.Font Font = new Font(this.treeViewDatabases.Font, FontStyle.Bold);
                System.Drawing.Font FontDB = new Font(this.treeViewDatabases.Font, FontStyle.Underline);
                System.Windows.Forms.TreeNode N = new TreeNode(this._ServerConnection.ModuleName + "         ");
                N.NodeFont = Font;
                this.treeViewDatabases.Nodes.Add(N);

                System.Windows.Forms.TreeNode NC = new TreeNode(this._ServerConnection.DatabaseName);
                NC.NodeFont = FontDB;
                NC.Tag = this._ServerConnection;
                NC.BackColor = System.Drawing.Color.Yellow;
                N.Nodes.Add(NC);
                this._DatabaseNodes.Add(NC);
            }
            else
            {
                //this._Databases = new Dictionary<string, ServerConnection>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                {
                    // the modules
                    System.Drawing.Font Font = new Font(this.treeViewDatabases.Font, FontStyle.Bold);
                    System.Drawing.Font FontDB = new Font(this.treeViewDatabases.Font, FontStyle.Underline);
                    System.Windows.Forms.TreeNode N = new TreeNode(KV.Key + "         ");
                    N.NodeFont = Font;

                    if (KV.Key == DiversityWorkbench.Settings.ModuleName)
                        N.BackColor = System.Drawing.Color.Yellow;

                    this.treeViewDatabases.Nodes.Add(N);
                    string x = KV.Value.ServerConnection.ModuleName;
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                    {
                        if (KV.Value.ServerConnection.ModuleName != KVconn.Value.ModuleName)
                            continue;
                        if (KV.Value.ServerConnectionList()[KVconn.Key].IsAddedRemoteConnection)
                            continue;
                        if (KV.Value.ServerConnection.LinkedServer.Length > 0)
                            continue;
                        if (KV.Value.ServerConnectionList()[KVconn.Key].LinkedServer.Length > 0)
                            continue;

                        System.Windows.Forms.TreeNode NC = new TreeNode(KVconn.Key);
                        NC.NodeFont = FontDB;
                        NC.Tag = KVconn.Value;
                        if (KV.Key == DiversityWorkbench.Settings.ModuleName)
                            NC.BackColor = System.Drawing.Color.Yellow;
                        N.Nodes.Add(NC);
                        this._DatabaseNodes.Add(NC);

                        if (KV.Value.ServerConnection.ModuleName == "DiversityAgents")
                        {
                            if (this._dtDatabasesForUserInfo == null)
                            {
                                this._dtDatabasesForUserInfo = new DataTable();
                                System.Data.DataColumn C = new DataColumn("DatabaseName", typeof(string));
                                this._dtDatabasesForUserInfo.Columns.Add(C);
                            }
                            System.Data.DataRow R = this._dtDatabasesForUserInfo.NewRow();
                            R[0] = KVconn.Key;
                            this._dtDatabasesForUserInfo.Rows.Add(R);
                            if (this.comboBoxDBUserInfo.DataSource == null)
                            {
                                this.comboBoxDBUserInfo.DataSource = this._dtDatabasesForUserInfo;
                                this.comboBoxDBUserInfo.DisplayMember = "DatabaseName";
                                this.comboBoxDBUserInfo.ValueMember = "DatabaseName";
                            }
                        }
                    }
                }
            }
            this.treeViewDatabases.ExpandAll();
        }

        /// <summary>
        /// A list of all databases as contained in the database nodes used for the tree view
        /// </summary>
        public System.Data.DataTable DtDatabases
        {
            get
            {
                if (this._dtDatabases == null)
                {
                    this._dtDatabases = new DataTable();
                    System.Data.DataColumn C = new DataColumn("Database", typeof(string));
                    foreach (System.Windows.Forms.TreeNode N in this._DatabaseNodes)
                    {
                        System.Data.DataRow R = this._dtDatabases.NewRow();
                        R[0] = N.Text;
                        this._dtDatabases.Rows.Add(R);
                    }
                }
                return _dtDatabases;
            }
            //set { _dtDatabases = value; }
        }

        private void treeViewDatabases_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeViewDatabases.SelectedNode != null &&
                this.treeViewDatabases.SelectedNode.Tag != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.groupBoxDatabase.Text = this.CurrentServerConnection().DatabaseName;
                this.setUserControlsAccordingToDatabase();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                this.setUserControlsAccordingToDatabase();
                this.groupBoxDatabase.Text = "";
            }
            this.setAgentUri();
            this.setProjectProxy();
            //this.initProjects();
        }

        private bool UserHasAccessToDatabase(DiversityWorkbench.ServerConnection SC)
        {
            _UserHasAccessToDatabase = false;
            try
            {
                DiversityWorkbench.Login L = new Login(this.CurrentLogin());
                _UserHasAccessToDatabase = L.LoginHasAccessToDatabase(SC);
                if (_UserHasAccessToDatabase)
                    L.SetLinkToDiversityAgents(SC);
                return _UserHasAccessToDatabase;
            }
            catch (System.Exception ex)
            {
                _UserHasAccessToDatabase = false;
            }
            return _UserHasAccessToDatabase;
        }

        public void UserHasAccessToDatabase(bool HasAccess)
        {
            _UserHasAccessToDatabase = HasAccess;
        }

        public bool ProjectsAndUserProxyDoExist(DiversityWorkbench.ServerConnection SC)
        {
            try
            {
                int i;
                //Check if the table UserProxy and the column AgentURI exists
                string SQL = "use " + SC.DatabaseName + "; select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'AgentURI'";
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i > 0)
                {
                    SQL = "use " + SC.DatabaseName + "; " +
                    "SELECT COUNT(*) AS Anzahl " +
                    "FROM UserProxy AS U INNER JOIN " +
                    "ProjectUser AS PU ON U.LoginName = PU.LoginName RIGHT OUTER JOIN " +
                    "ProjectProxy AS P ON PU.ProjectID = P.ProjectID";
                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
                    {
                        this._ProjectsDoExist = true;
                        SQL = "use " + SC.DatabaseName + "; " +
                            "SELECT COUNT(*) AS Anzahl " +
                            "FROM UserProxy " +
                            "WHERE (UserProxy.LoginName = N'" + this.CurrentLogin() + "')";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        try
                        {
                            con.Open();
                            if (int.TryParse(c.ExecuteScalar()?.ToString(), out i) && i == 0)
                            {
                                this.AddCurrentUserToUserProxy();
                            }
                        }
                        catch { }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }
                    else
                        this._ProjectsDoExist = false;
                }
                else
                    this._ProjectsDoExist = false;
            }
            catch (System.Exception ex)
            {
                this._ProjectsDoExist = false;
            }

            return _ProjectsDoExist;
        }

        private void AddCurrentUserToUserProxy()
        {
            string SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                "INSERT INTO UserProxy " +
                "(LoginName, CombinedNameCache) " +
                "VALUES     ('" + this.CurrentLogin() + "', '" + this.CurrentLogin() + "')";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                c.ExecuteNonQuery();
            }
            catch { }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void setDatabasesAccordingToLogin()
        {
            try
            {
                if (this.LoginIsDisabled)
                {
                    this.groupBoxDatabases.Enabled = false;
                }
                else
                {
                    this.groupBoxDatabases.Enabled = true;
                    foreach (System.Windows.Forms.TreeNode N in this._DatabaseNodes)
                    {
                        DiversityWorkbench.ServerConnection SC = (DiversityWorkbench.ServerConnection)N.Tag;
                        this.CurrentServerConnection();
                        //this.groupBoxProjects.Enabled = true;
                        if (this.UserHasAccessToDatabase(SC))
                            N.ForeColor = this._ColorDatabaseUser;
                        else N.ForeColor = this._ColorDatabaseNoUser;
                    }
                }
            }
            catch (System.Exception ex) { }

        }

        /// <summary>
        /// setting the forecolor of the current database node
        /// </summary>
        private void setDatabaseNodeAccordingToLogin()
        {
            try
            {
                if (this.UserHasAccessToDatabase(this.CurrentServerConnection()))
                    this.treeViewDatabases.SelectedNode.ForeColor = this._ColorDatabaseUser;
                else this.treeViewDatabases.SelectedNode.ForeColor = this._ColorDatabaseNoUser;
            }
            catch (System.Exception ex) { }
        }

        private DiversityWorkbench.ServerConnection CurrentServerConnection()
        {
            try
            {
                if (this.treeViewDatabases.SelectedNode != null)
                {
                    return (DiversityWorkbench.ServerConnection)this.treeViewDatabases.SelectedNode.Tag;
                }
            }
            catch (System.Exception ex) { }
            return null;
        }

        private void toolStripButtonLoginMissing_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT s.name as [Database user], s.islogin " +
                    "FROM sys.sysusers s  " +
                    "left outer JOIN master.sys.server_principals p ON substring(s.name, charindex('\\', s.name) + 1, 255) = substring(p.name, charindex('\\', p.name) + 1, 255) " +
                    "where p.name is null and s.islogin = 1 and s.isntuser = 1 and s.issqluser = 0 and s.hasdbaccess = 1 and s.name <> 'dbo' " +
                    "UNION " +
                    "SELECT s.name as [Database user], s.islogin " +
                    "FROM sys.sysusers s  " +
                    "left outer JOIN master.sys.server_principals p ON p.name = s.name  " +
                    "where p.name is null and s.islogin = 1 and s.isntuser = 0 and s.issqluser = 1 and s.hasdbaccess = 1 and s.name <> 'dbo' " +
                    "ORDER BY s.islogin, s.name";
                string Database = this.CurrentServerConnection().DatabaseName;
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.CurrentServerConnection().ConnectionString);// DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, bool> Dict = new Dictionary<string, bool>();
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Dict.Add(R["Database user"].ToString(), false);
                    }
                    DiversityWorkbench.Forms.FormGetMultiFromList f = new FormGetMultiFromList(Database + ": No login", "The following users in the database " + Database + " have no valid login.\r\nPlease select those that should be DELETED", Dict);
                    IntPtr Hicon = DiversityWorkbench.Properties.Resources.LoginMissing.GetHicon();
                    f.Icon = Icon.FromHandle(Hicon);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        System.Collections.Generic.List<string> L = f.SelectedItems();
                        foreach (string S in L)
                        {
                            // special treatment for modules
                            string Message = "";
                            string Module = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT dbo.DiversityWorkbenchModule()", this.CurrentServerConnection().ConnectionString);
                            bool OK = true;
                            switch (Module)
                            {
                                case "DiversityCollection":
                                    // nicht aus UserProxy loeschen da sonst nicht mehr nachvollziehbar ist wer was eingegeben hat.
                                    SQL = "Delete U from [" + this.CurrentServerConnection().DatabaseName + "].[dbo].[CollectionManager] U WHERE U.LoginName = '" + S + "'; ";
                                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, this.CurrentServerConnection().ConnectionString, ref Message);
                                    break;
                            }
                            if (OK)
                            {
                                SQL = "DROP USER [" + S + "]";
                                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, this.CurrentServerConnection().ConnectionString, ref Message);
                            }
                            if (!OK)
                                System.Windows.Forms.MessageBox.Show("Removing user " + S + " failed: " + Message);
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Database " + Database + "\r\nNo users with missing login detected");
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripButtonSynchronizeUserProxy_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "use master; " +
                    "select p.name, U.LoginName " +
                    "from sys.server_principals p, " +
                    "[" + this.CurrentServerConnection().DatabaseName + "].[dbo].[UserProxy] U " +
                    "where p.name not like '##%##'  " +
                    "and p.type in ('U')  " +
                    "and p.name not like 'NT-%'  " +
                    "and p.name <> 'sa' " +
                    "and p.name like '%\\%' " +
                    "and p.name not like 'NT %\\%' " +
                    "and U.LoginName like '%\\%' " +
                    "and substring(p.name, charindex('\\', p.name), 255) = substring(U.LoginName, charindex('\\', U.LoginName), 255)  " +
                    "and substring(U.LoginName, 1, charindex('\\', U.LoginName) - 1) <>substring(p.name, 1, charindex('\\', p.name) - 1) " +
                    "order by type desc, name;";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 0)
                    System.Windows.Forms.MessageBox.Show("No differences found");
                else
                {
                    string Message = "";
                    string SqlDulicates = "select U.LoginName from  [" + this.CurrentServerConnection().DatabaseName + "].[dbo].[UserProxy] p, " +
                        "[" + this.CurrentServerConnection().DatabaseName + "].[dbo].[UserProxy] U " +
                        "where p.LoginName like '%\\%'  " +
                        "and U.LoginName like '%\\%'  " +
                        "and substring(p.LoginName, charindex('\\', p.LoginName), 255) = substring(U.LoginName, charindex('\\', U.LoginName), 255)   " +
                        "and substring(U.LoginName, 1, charindex('\\', U.LoginName) - 1) <> substring(p.LoginName, 1, charindex('\\', p.LoginName) - 1)  " +
                        "order by substring(U.LoginName, charindex('\\', U.LoginName), 255);";
                    System.Data.DataTable dtDuplicates = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter adDuplicates = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlDulicates, DiversityWorkbench.Settings.ConnectionString);
                    adDuplicates.Fill(dtDuplicates);
                    if (dtDuplicates.Rows.Count > 0)
                    {
                        System.Collections.Generic.Dictionary<string, bool> Dict = new Dictionary<string, bool>();
                        foreach (System.Data.DataRow R in dtDuplicates.Rows)
                        {
                            Dict.Add(R["LoginName"].ToString(), false);
                        }
                        DiversityWorkbench.Forms.FormGetMultiFromList f = new FormGetMultiFromList("Duplicates", "The following logins are duplicates.\r\nPlease select those that should be DELETED", Dict);
                        f.ForeColor = System.Drawing.Color.Red;
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            System.Collections.Generic.List<string> L = f.SelectedItems();
                            foreach (string S in L)
                            {
                                if (System.Windows.Forms.MessageBox.Show("Remove user " + S + "?", "Remove user", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    SQL = "Delete U from [" + this.CurrentServerConnection().DatabaseName + "].[dbo].[UserProxy] U WHERE U.LoginName = '" + S + "'";
                                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                }
                            }
                        }
                        dtDuplicates.Clear();
                        adDuplicates.Fill(dtDuplicates);
                        if (dtDuplicates.Rows.Count == 0)
                        {
                            dt.Clear();
                            ad.Fill(dt);
                        }
                        else
                            return;
                    }
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Message += R["name"].ToString() + " <> " + R["LoginName"].ToString() + "\r\n";
                    }
                    Message = "The following differing logins exist:\r\n\r\n" + Message + "\r\nDo you want to correct these?";
                    if (System.Windows.Forms.MessageBox.Show(Message, "Correct logins?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Message = "";
                        SQL = "use master; " +
                            "UPDATE U SET U.LoginName = p.name " +
                            "from sys.server_principals p, " +
                            "[" + this.CurrentServerConnection().DatabaseName + "].[dbo].[UserProxy] U " +
                            "where p.name not like '##%##'  " +
                            "and p.type in ('U')  " +
                            "and p.name not like 'NT-%'  " +
                            "and p.name <> 'sa'   " +
                            "and p.name like '%\\%' " +
                            "and p.name not like 'NT %\\%' " +
                            "and U.LoginName like '%\\%' " +
                            "and substring(p.name, charindex('\\', p.name), 255) = substring(U.LoginName, charindex('\\', U.LoginName), 255)  " +
                            "and substring(U.LoginName, 1, charindex('\\', U.LoginName) - 1) <> substring(p.name, 1, charindex('\\', p.name) - 1)";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                        if (Message.Length > 0)
                            System.Windows.Forms.MessageBox.Show(Message);
                        else
                        {
                            this.toolStripButtonSynchronizeUserProxy.Enabled = false;
                            System.Windows.Forms.MessageBox.Show("Logins corrected");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripButtonListAllDatabases_Click(object sender, EventArgs e)
        {
            if (this._ListAllModules)
                return;

            this._ListAllModules = true;
            this.toolStripButtonListAllDatabases.BackColor = System.Drawing.Color.Green;
            DiversityWorkbench.Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.Description D = new Description(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.Exsiccate E = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.ScientificTerm S = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.Reference L = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
            DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);

            this.treeViewDatabases.Nodes.Clear();
            this.initDatabaseTree();
            this.toolStripButtonListAllDatabases.DisplayStyle = ToolStripItemDisplayStyle.Image;
        }

        private void toolStripButtonDatabaseOverview_Click(object sender, EventArgs e)
        {
            if (this.CurrentServerConnection() != null)
            {
                DiversityWorkbench.Forms.FormDatabaseOverview f = new FormDatabaseOverview(this.CurrentServerConnection());
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select a database");
        }

        #endregion

        #region User

        #region User specific functions
        /// <summary>
        /// set the state of the user according to the settings, i.e. create or remove a user
        /// </summary>
        private void setUserState()
        {
            try
            {
                if (this.radioButtonLoginOnly.Checked)
                {
                    if (this.UserHasAccessToDatabase(this.CurrentServerConnection()))
                    {
                        if (this.DeleteDatabaseUser())
                        {
                            this.setDatabaseNodeAccordingToLogin();
                        }
                    }
                }
                else if (this.radioButtonUser.Checked)
                {
                    if (!this.UserHasAccessToDatabase(this.CurrentServerConnection()))
                    {
                        if (this.CreateDatabaseUser())
                        {
                            if (this.ProjectsAndUserProxyDoExist(this.CurrentServerConnection()))
                                this.InsertUserInUserProxy();
                            this.setDatabaseNodeAccordingToLogin();
                        }
                    }
                }
                this.setUserControlsAccordingToDatabase();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        /// <summary>
        /// set the user controls according to the values found in the database
        /// </summary>
        private void setUserControlsAccordingToDatabase()
        {
            if (this.CurrentServerConnection() != null)
            {
                this.tabControlUserDetails.Enabled = true;
                this.groupBoxUser.Enabled = true;
                this.tableLayoutPanelProjects.Enabled = true;
                this.tableLayoutPanelRoles.Enabled = true;
                if (this.UserHasAccessToDatabase(this.CurrentServerConnection()))
                {
                    this.tabControlUserDetails.Visible = true;
                    this.userControlModuleRelatedEntryAgent.Visible = true;
                    this.radioButtonUser.Checked = true;
                    this.tableLayoutPanelRoles.Enabled = true;
                    this.SetRoles();
                    this.SetUserRoles();
                    if (this.ProjectsAndUserProxyDoExist(this.CurrentServerConnection()))
                    {
                        this.initProjects();
                        if (!this.tabControlUserDetails.TabPages.Contains(this.tabPageProjects))
                            this.tabControlUserDetails.TabPages.Add(this.tabPageProjects);
                        this.SetUserProjects();
                    }
                    else
                    {
                        if (this.tabControlUserDetails.TabPages.Contains(this.tabPageProjects))
                            this.tabControlUserDetails.TabPages.Remove(this.tabPageProjects);
                    }
                    if (this.UserSettingsAvailable())
                    {
                        if (!this.tabControlUserDetails.TabPages.Contains(this.tabPageSettings))
                            this.tabControlUserDetails.TabPages.Add(this.tabPageSettings);
                        this.DisplayUserSettings();
                    }
                    else
                    {
                        if (this.tabControlUserDetails.TabPages.Contains(this.tabPageSettings))
                            this.tabControlUserDetails.TabPages.Remove(this.tabPageSettings);
                    }
                    this.checkBoxIsDBO.Visible = true;
                    string SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; select count(*) from sys.database_role_members M, sys.database_principals R, sys.database_principals L " +
                        "where M.member_principal_id = R.principal_id and M.role_principal_id = L.principal_id " +
                        "and L.name = 'db_owner' and R.type IN ('S' , 'U') and L.type = 'R' " +
                        "and R.name LIKE '" + this.LoginName.Replace("\\", "_") + "' ";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "0")
                        this.checkBoxIsDBO.Checked = false;
                    else
                        this.checkBoxIsDBO.Checked = true;

                    this.SetPrivacyConsentState();
                }
                else
                {
                    this.checkBoxIsDBO.Visible = false;
                    this.checkBoxPrivacyConsent.Visible = false;
                    this.tabControlUserDetails.Visible = false;
                    this.userControlModuleRelatedEntryAgent.Visible = false;
                    this.radioButtonLoginOnly.Checked = true;
                    this.tableLayoutPanelRoles.Enabled = false;
                    if (this.dtRoles == null) this.dtRoles = new DataTable();
                    this.dtRoles.Clear();
                    if (this.dtUserRoles == null) this.dtUserRoles = new DataTable();
                    this.dtUserRoles.Clear();
                    if (this.tabControlUserDetails.TabPages.Contains(this.tabPageProjects))
                        this.tabControlUserDetails.TabPages.Remove(this.tabPageProjects);
                    if (this.tabControlUserDetails.TabPages.Contains(this.tabPageSettings))
                        this.tabControlUserDetails.TabPages.Remove(this.tabPageSettings);
                }
                this.groupBoxDatabase.ForeColor = this.treeViewDatabases.SelectedNode.ForeColor;

                if (this.CurrentServerConnection().ModuleName == "DiversityProjects")
                {
                    this.buttonSynchronizeProjects.Enabled = false;
                    this.userControlModuleRelatedEntryProject.buttonDeleteURI.Enabled = false;
                    this.userControlModuleRelatedEntryProject.buttonOpenModule.Enabled = false;
                }
                else
                {
                    this.buttonSynchronizeProjects.Enabled = true;
                    this.userControlModuleRelatedEntryProject.buttonDeleteURI.Enabled = true;
                    this.userControlModuleRelatedEntryProject.buttonOpenModule.Enabled = true;
                }
            }
            else
            {
                this.tabControlUserDetails.Enabled = false;
            }
        }

        /// <summary>
        /// Setting the privacy consent state in dependece of the availability of the columns in table UserProxy
        /// </summary>
        private void SetPrivacyConsentState()
        {
            try
            {
                /// dbo is not included in PrivacyConsent as this is anonmy by default
                bool IsDbo = false;
                if (this.LoginName.ToLower() == "dbo")
                    IsDbo = true;
                /// Check if the columns for PrivacyConsent are established
                string SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; " +
                    "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'PrivacyConsent'";
                string Privacy = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Privacy == "1" && !IsDbo) // PrivacyConsent is established
                {
                    this.checkBoxPrivacyConsent.Visible = true;
                    SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; " +
                        "select U.PrivacyConsent FROM [UserProxy] U where U.LoginName = '" + this.LoginName + "'";
                    Privacy = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Privacy == "")
                    {
                        this.checkBoxPrivacyConsent.Checked = false;
                        this.checkBoxPrivacyConsent.Text = "     undeciced";
                        this.checkBoxPrivacyConsent.ForeColor = System.Drawing.Color.Gray;
                        this.toolTip.SetToolTip(this.checkBoxPrivacyConsent, "Decision for privacy consent missing");
                        this.buttonSetPrivacyConsent.Visible = true;
                    }
                    else
                    {
                        if (Privacy == "True")
                        {
                            this.buttonSetPrivacyConsent.Visible = false;
                            this.toolTip.SetToolTip(this.checkBoxPrivacyConsent, "Privacy consent accepted");
                            this.checkBoxPrivacyConsent.ForeColor = System.Drawing.Color.Green;
                            this.buttonSetPrivacyConsent.Visible = true;
                        }
                        else
                        {
                            this.checkBoxPrivacyConsent.Checked = true;
                            this.checkBoxPrivacyConsent.Checked = false;
                            this.toolTip.SetToolTip(this.checkBoxPrivacyConsent, "Privacy consent rejected");
                            this.checkBoxPrivacyConsent.ForeColor = System.Drawing.Color.Red;
                        }
                        SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; " +
                            "select convert(nvarchar(10), U.PrivacyConsentDate, 120)  FROM [UserProxy] U where U.LoginName = '" + this.LoginName + "'";
                        Privacy = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (Privacy.Length == 0)
                            Privacy = "date?";
                        this.checkBoxPrivacyConsent.Text = "     " + Privacy;
                    }
                }
                else
                {
                    this.checkBoxPrivacyConsent.Visible = false;
                    this.buttonSetPrivacyConsent.Visible = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSetPrivacyConsent_Click(object sender, EventArgs e)
        {
            string SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; " +
                "UPDATE U SET U.PrivacyConsent = 1 FROM [UserProxy] U where U.LoginName = '" + this.LoginName + "'";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            this.SetPrivacyConsentState();
        }

        private void setAgentUri()
        {
            if (this._DsUserProxy != null
                //&& this._DsUserProxy.HasChanges()
                && this._AgentBindingSource != null)
            {
                this._AgentBindingSource.EndEdit();
                this._adUserProxy.Update(this._DtUserProxyAgentInfo);
            }
            if (this.UserHasAccessToDatabase(this.CurrentServerConnection()) && this.ProjectsAndUserProxyDoExist(this.CurrentServerConnection()))
            {
                try
                {
                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryAgent.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                    this.userControlModuleRelatedEntryAgent.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
                    if (this._DsUserProxy == null)
                    {
                        this._DsUserProxy = new DataSet();
                        this._DtUserProxyAgentInfo = new DataTable("UserProxy");
                        this._DsUserProxy.Tables.Add(this._DtUserProxyAgentInfo);
                    }
                    string SQL = "SELECT LoginName, CombinedNameCache, AgentURI " +
                        "FROM " + this.CurrentServerConnection().DatabaseName + ".dbo.UserProxy " +
                        "WHERE     (LoginName = N'" + this.LoginName + "')";
                    if (this._adUserProxy == null)
                        this._adUserProxy = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    else
                        this._adUserProxy.SelectCommand.CommandText = SQL;
                    if (this._DtUserProxyAgentInfo.Rows.Count > 0)
                    {
                        this._adUserProxy.Update(this._DtUserProxyAgentInfo);
                        this._DtUserProxyAgentInfo.Clear();
                    }
                    this._adUserProxy.Fill(this._DtUserProxyAgentInfo);
                    Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(this._adUserProxy);
                    if (this._AgentBindingSource == null)
                    {
                        this._AgentBindingSource = new System.Windows.Forms.BindingSource(this.components);
                        ((System.ComponentModel.ISupportInitialize)(this._AgentBindingSource)).BeginInit();
                        this._AgentBindingSource.DataMember = "UserProxy";
                        this._AgentBindingSource.DataSource = this._DsUserProxy;
                        ((System.ComponentModel.ISupportInitialize)(this._AgentBindingSource)).EndInit();
                        this.userControlModuleRelatedEntryAgent.bindToData("UserProxy", "CombinedNameCache", "AgentURI", this._AgentBindingSource);
                    }
                    if (this._DtUserProxyAgentInfo.Rows[0]["AgentURI"].Equals(System.DBNull.Value)
                        && this.LoginAgentURI != null && this.LoginAgentURI.Length > 0)
                    {
                        this._DtUserProxyAgentInfo.Rows[0]["AgentURI"] = this.LoginAgentURI;
                        this._DtUserProxyAgentInfo.Rows[0]["CombinedNameCache"] = this._LoginAgentName;
                    }
                }
                catch (System.Exception ex) { }
            }
            else
            {
                if (this._DtUserProxyAgentInfo != null && this._DtUserProxyAgentInfo.Rows.Count > 0)
                {
                    if (this._DsUserProxy.HasChanges())
                        this._adUserProxy.Update(this._DtUserProxyAgentInfo);
                    this._DtUserProxyAgentInfo.Clear();
                }
            }
        }

        private void radioButtonLoginOnly_Click(object sender, EventArgs e)
        {
            this.setUserState();
        }

        private void radioButtonUser_Click(object sender, EventArgs e)
        {
            this.setUserState();
        }

        private void radioButtonUserWithProjectAccess_Click(object sender, EventArgs e)
        {
            this.setUserState();
        }

        private bool CreateDatabaseUser()
        {
            try
            {
                string SQL = "";
                string Message = "";
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                SQL = "use " + SC.DatabaseName + " IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this.LoginName + "') " +
                    "DROP USER [" + this.LoginName + "] " +
                    "IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this.LoginName + "') " +
                    "CREATE USER [" + this.LoginName + "] FOR LOGIN [" + this.LoginName + "] WITH DEFAULT_SCHEMA=[dbo];";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                    return true;
                else
                {
                    System.Windows.Forms.MessageBox.Show(Message);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
            }
            return false;
        }

        private bool DeleteDatabaseUser()
        {
            string message = "";
            string SQL = "";
            DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
            SQL = "use " + SC.DatabaseName + "; " +
                "DELETE FROM UserProxy WHERE LoginName = '" + this.LoginName + "'; ";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref message);
            if (message != "")
            {
                MessageBox.Show(message);
                return false;
            }
            SQL = "use " + SC.DatabaseName + "; " +
                "IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this.LoginName + "') " +
                "DROP USER [" + this.LoginName + "]; ";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref message))
                return true;
            if (message != "")
                MessageBox.Show(message);
            return false;
        }

        private bool InsertUserInUserProxy()
        {
            try
            {
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string Message = "";
                string SQL = "use " + SC.DatabaseName + "; " +
                    "SELECT CombinedNameCache " +
                    "FROM UserProxy  U, ProjectUser P " +
                    "WHERE (U.LoginName = N'" + this.LoginName + "') AND U.LoginName = P.LoginName";
                string PresentUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                bool UserIsPresent = false;
                if (PresentUser.Length > 0)
                {
                    Message = "In the database " + SC.DatabaseName +
                        "\r\na user with the same name\r\n" + this.LoginName +
                        "\r\nhas been found (table UserProxy)\r\n\r\n" +
                        "Should this user be replaced?\r\n\r\n(Choose no if user should be kept)";
                    System.Windows.Forms.DialogResult Result = System.Windows.Forms.MessageBox.Show(Message, "Remove user?", MessageBoxButtons.YesNoCancel);
                    if (Result == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "use " + SC.DatabaseName + "; " +
                            "DELETE FROM UserProxy WHERE LoginName = '" + this.LoginName + "'";
                        if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                            return false;
                    }
                    else if (Result == System.Windows.Forms.DialogResult.No)
                    {
                        System.Windows.Forms.MessageBox.Show("User will be kept");
                        UserIsPresent = true;
                        return true;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Adding of user canceled");
                        return false;
                    }
                }
                //DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                if (!UserIsPresent)
                {
                    DiversityWorkbench.Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
                    DiversityWorkbench.Forms.FormRemoteQuery f = new FormRemoteQuery(A);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        string AgentURI = f.URI;
                        string CombinedNameCache = f.DisplayText.Trim();
                        SQL = "SELECT CombinedNameCache " +
                            "FROM UserProxy  U " +
                            "WHERE (U.LoginName = N'" + this.LoginName + "')";
                        string UserInProxyTable = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (UserInProxyTable.Length == 0)
                        {
                            SQL = "use " + SC.DatabaseName + ";  " +
                                "INSERT INTO UserProxy (LoginName, CombinedNameCache, AgentURI) " +
                                "VALUES ('" + this.LoginName + "', '" + CombinedNameCache + "', '" + AgentURI + "') ";
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                return true;
                        }
                        else
                        {
                            SQL = "use " + SC.DatabaseName + ";  " +
                                "UPDATE U SET CombinedNameCache = '" + CombinedNameCache + "', AgentURI = '" + AgentURI + "' " +
                                "FROM UserProxy U WHERE U.LoginName = '" + this.LoginName + "' ";
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private bool DeleteUserFromUserProxy()
        {
            string message = "";
            string SQL = "";
            DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
            SQL = "use " + SC.DatabaseName + "; " +
               "DELETE FROM [UserProxy] WHERE LoginName = '" + this.LoginName + "';  ";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref message))
                return true;
            if (message != "")
                MessageBox.Show(message);
            return false;
        }

        private void checkBoxIsDBO_Click(object sender, EventArgs e)
        {
            string SQL = "";
            if (this.checkBoxIsDBO.Checked)
                SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; EXEC sp_addrolemember N'db_owner', N'" + this.LoginName + "'";
            else
                SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; EXEC sp_droprolemember N'db_owner', N'" + this.LoginName + "'";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
            this.setUserControlsAccordingToDatabase();
        }

        #endregion

        #region Settings

        private bool UserSettingsAvailable()
        {
            bool SettingsAvailable = false;
            // vorerst abgeschaltet - wird nicht benutzt
            try
            {
                //DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                //string SQL = "use " + SC.DatabaseName + "; " +
                //    "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C " +
                //    "where C.TABLE_NAME = 'UserProxy' " +
                //    "and C.COLUMN_NAME = 'Settings' " +
                //    "and C.DATA_TYPE = 'XML'";
                //if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) == "1")
                //    SettingsAvailable = true;
            }
            catch (System.Exception ex) { }
            return SettingsAvailable;
        }

        private void buildSettingsTree(string XML)
        {
            try
            {
                this.treeViewSettings.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XML, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    this.treeViewSettings.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    this.treeViewSettings.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = this.treeViewSettings.Nodes[0];
                    // SECTION 3. Populate the TreeView with the DOM nodes.
                    this.AddSettingsNode(dom.DocumentElement, tNode);
                    this.treeViewSettings.ExpandAll();
                }
                else
                {
                    if (dom.DocumentElement.Attributes["error_message"].InnerText.Length > 0)
                    {
                        string Message = dom.DocumentElement.Attributes["error_message"].InnerText;
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                }
            }
            catch { }
        }

        private void AddSettingsNode(System.Xml.XmlNode inXmlNode, System.Windows.Forms.TreeNode inTreeNode)
        {
            System.Xml.XmlNode xNode;
            System.Windows.Forms.TreeNode tNode;
            System.Xml.XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new System.Windows.Forms.TreeNode(xNode.Name));
                    if (i >= inTreeNode.Nodes.Count)
                        continue;
                    tNode = inTreeNode.Nodes[i];
                    this.AddSettingsNode(xNode, tNode);
                }
            }
            else
            {
                if (inXmlNode.Value != null)
                {
                    inTreeNode.Parent.Tag = inXmlNode;//.Value;
                    inTreeNode.Parent.Text = inTreeNode.Parent.Text + ": " + inXmlNode.Value;
                    inTreeNode.Remove();
                }
            }
        }

        private string XmlFromSettingsTree()
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "ImageDescriptionTemplate.xml");

            string XML = "";
            if (this.treeViewSettings.Nodes.Count == 0) return "";
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            W.WriteStartElement(this.treeViewSettings.Nodes[0].Text);
            foreach (System.Windows.Forms.TreeNode N in this.treeViewSettings.Nodes[0].Nodes)
            {
                this.XmlFromSettingsTreeAddChild(N, ref W);
            }
            W.WriteEndElement();
            W.Flush();
            W.Close();
            System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
            XML = R.ReadToEnd();
            R.Close();
            return XML;
        }

        private void XmlFromSettingsTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            string Node = TreeNode.Text.Replace(" ", "_");
            if (Node.IndexOf(":") > -1) Node = Node.Substring(0, Node.IndexOf(":"));
            if (TreeNode.Tag != null)
            {
                if (this.treeViewSettings.SelectedNode.Tag.GetType() == typeof(System.Xml.XmlNode))
                {
                    W.WriteElementString(Node, TreeNode.Tag.ToString());
                }
                else if (this.treeViewSettings.SelectedNode.Tag.GetType() == typeof(System.Xml.XmlText))
                {
                    System.Xml.XmlText T = (System.Xml.XmlText)TreeNode.Tag;
                    W.WriteElementString(Node, T.Value.ToString());
                }
            }
            else
            {
                W.WriteStartElement(Node);
                foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
                    this.XmlFromSettingsTreeAddChild(NChild, ref W);
                W.WriteEndElement();
            }
        }

        private void buttonAddSettingsNode_Click(object sender, EventArgs e)
        {
            if (this.treeViewSettings.Nodes.Count > 0
                && this.treeViewSettings.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new node");
                return;
            }
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New node", "Please enter the name of the new node", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Collections.Generic.List<char> NotAllowedSigns = new List<char>();
                NotAllowedSigns.Add(' ');
                NotAllowedSigns.Add('(');
                NotAllowedSigns.Add(')');
                NotAllowedSigns.Add('<');
                NotAllowedSigns.Add('>');
                NotAllowedSigns.Add('"');
                NotAllowedSigns.Add('\'');
                NotAllowedSigns.Add('/');
                foreach (char C in NotAllowedSigns)
                {
                    if (f.String.IndexOf(C) > -1)
                    {
                        System.Windows.Forms.MessageBox.Show("Names can not contain " + C.ToString());
                        return;
                    }
                }
                if (this.treeViewSettings.Nodes.Count == 0)
                    this.treeViewSettings.Nodes.Add(f.String);
                else
                {
                    if (this.treeViewSettings.SelectedNode == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select the parent entry for the new node");
                        return;
                    }
                    else
                    {
                        this.treeViewSettings.SelectedNode.Nodes.Add(f.String);
                        this.treeViewSettings.SelectedNode.Expand();
                    }
                }
            }
        }

        private void buttonRemoveSettingsNode_Click(object sender, EventArgs e)
        {
            if (this.treeViewSettings.SelectedNode != null)
            {
                this.treeViewSettings.SelectedNode.Remove();
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select the node you want to remove");
        }

        private void buttonSearchSettingsTemplate_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormSelectXmlTemplate f = new DiversityWorkbench.Forms.FormSelectXmlTemplate(
                this.DtUserProxy, "LoginName", "Settings");
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.Template.Length > 0)
            {
                this.Settings = f.Template;
            }
        }

        private System.Data.DataTable _DtUserProxy;
        private System.Data.DataTable DtUserProxy
        {
            get
            {
                if (this._DtUserProxy == null)
                {
                    DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                    string SQL = "use " + SC.DatabaseName + "; " +
                        "SELECT LoginName, Settings " +
                        "FROM UserProxy " +
                        "WHERE NOT (Settings IS NULL) " +
                        "ORDER BY LoginName";
                    this._DtUserProxy = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtUserProxy);
                }
                return this._DtUserProxy;
            }
        }

        private string _Settings;
        private string Settings
        {
            get { return this._Settings; }
            set
            {
                this._Settings = value;
                this.buildSettingsTree(this._Settings);
            }
        }

        private void DisplayUserSettings()
        {
            this.textBoxContentSettings.Text = "";
            this.labelContentSettings.Text = "";
            DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
            string SQL = "use " + SC.DatabaseName + "; " +
                "SELECT CAST(Settings as nvarchar(max)) " +
                "FROM UserProxy " +
                "WHERE LoginName = '" + this.LoginName + "'";
            this.Settings = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        }

        private void SaveUserSettings()
        {
            DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
            string SQL = "use " + SC.DatabaseName + "; " +
                "UPDATE U SET Settings = '" +
                this.XmlFromSettingsTree() + "' " +
                "FROM UserProxy U " +
                "WHERE U.LoginName = '" + this.LoginName + "'";
            if (FormFunctions.SqlExecuteNonQuery(SQL))
                System.Windows.Forms.MessageBox.Show("Settings saved");
            else System.Windows.Forms.MessageBox.Show("Error while saving settings");
        }

        private void treeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeViewSettings.SelectedNode.Tag != null)
            {
                try
                {
                    if (this.treeViewSettings.SelectedNode.Tag.GetType() == typeof(System.Xml.XmlNode))
                    {
                        XmlNode N = (XmlNode)this.treeViewSettings.SelectedNode.Tag;
                        if (N.Name == "#text")
                        {
                            if (this.treeViewSettings.SelectedNode.Text.IndexOf(":") > -1)
                                this.labelContentSettings.Text = this.treeViewSettings.SelectedNode.Text.Substring(0, this.treeViewSettings.SelectedNode.Text.IndexOf(":") + 1);
                            else
                                this.labelContentSettings.Text = this.treeViewSettings.SelectedNode.Text + ":";
                        }
                        else
                            this.labelContentSettings.Text = N.Name + ":";
                        this.textBoxContentSettings.Text = N.Value;
                    }
                    else if (this.treeViewSettings.SelectedNode.Tag.GetType() == typeof(System.Xml.XmlText))
                    {
                        System.Xml.XmlText N = (System.Xml.XmlText)this.treeViewSettings.SelectedNode.Tag;
                        if (N.Name == "#text")
                        {
                            if (this.treeViewSettings.SelectedNode.Text.IndexOf(":") > -1)
                                this.labelContentSettings.Text = this.treeViewSettings.SelectedNode.Text.Substring(0, this.treeViewSettings.SelectedNode.Text.IndexOf(":") + 1);
                            else
                                this.labelContentSettings.Text = this.treeViewSettings.SelectedNode.Text + ":";
                        }
                        else
                            this.labelContentSettings.Text = N.Name + ":";
                        this.textBoxContentSettings.Text = N.Value;
                    }
                }
                catch (System.Exception ex) { }
            }
            else
            {
                this.textBoxContentSettings.Text = "";
                string Node = this.treeViewSettings.SelectedNode.Text;
                if (Node.IndexOf(":") > -1)
                    Node = Node.Substring(0, Node.IndexOf(":"));
                this.labelContentSettings.Text = Node;
            }
        }

        private void textBoxSettingsContent_Leave(object sender, EventArgs e)
        {
            XmlNode N = new XmlNode();
            if (this.treeViewSettings.SelectedNode.Tag != null)
            {
                if (this.treeViewSettings.SelectedNode.Tag.GetType() == typeof(System.Xml.XmlNode))
                {
                    N = (XmlNode)this.treeViewSettings.SelectedNode.Tag;
                    N.Value = this.textBoxContentSettings.Text;
                    this.treeViewSettings.SelectedNode.Tag = N;
                    this.treeViewSettings.SelectedNode.Text = N.Name + ": " + N.Value;
                }
                else if (this.treeViewSettings.SelectedNode.Tag.GetType() == typeof(System.Xml.XmlText))
                {
                    System.Xml.XmlText T = (System.Xml.XmlText)this.treeViewSettings.SelectedNode.Tag;
                    T.Value = this.textBoxContentSettings.Text;
                    this.treeViewSettings.SelectedNode.Tag = T;
                    this.treeViewSettings.SelectedNode.Text = T.ParentNode.Name + ": " + T.Value;
                }
            }
            else
            {
                N = new XmlNode();
                string Name = this.treeViewSettings.SelectedNode.Text;
                if (Name.IndexOf(":") > -1)
                {
                    Name = Name.Substring(0, Name.IndexOf(":"));
                }
                N.Name = Name;
                N.Value = this.textBoxContentSettings.Text;
                this.treeViewSettings.SelectedNode.Tag = N;
                this.treeViewSettings.SelectedNode.Text = N.Name + ": " + N.Value;
            }
            this.SaveUserSettings();
        }

        #endregion

        #region Roles

        private void buttonRoleAdd_Click(object sender, EventArgs e)
        {
            if (this.listBoxRoles.SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a role to be inserted!");
                return;
            }
            string Role = this.listBoxRoles.SelectedValue.ToString();
            string Member = this.CurrentLogin();
            this.RoleAdd(Role, Member);
            this.SetUserRoles();
        }

        private void RoleAdd(string Role, string Login)
        {
            DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
            string SQL = "use " + SC.DatabaseName + "; " +
               "EXEC sp_addrolemember N'" + Role + "', N'" + Login + "'";
            try
            {
                string ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                if (this._ForSingleDatabase && this._ServerConnection.ConnectionString.Length > 0)
                    ConnectionString = this._ServerConnection.ConnectionString;
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                c.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonRoleRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxUserRoles.SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a role to be removed!");
                return;
            }
            string Role = this.listBoxUserRoles.SelectedValue.ToString();
            string Member = this.CurrentLogin();
            this.RoleRemove(Role, Member);
            this.SetUserRoles();
        }

        private void RoleRemove(string Role, string Login)
        {
            DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
            string SQL = "use " + SC.DatabaseName + "; " +
               "EXEC sp_droprolemember N'" + Role + "', N'" + Login + "'";
            try
            {
                string ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                if (this._ForSingleDatabase && this._ServerConnection.ConnectionString.Length > 0)
                    ConnectionString = this._ServerConnection.ConnectionString;
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                c.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetRoles()
        {
            try
            {
                string SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                    "SELECT name FROM sysusers WHERE (uid = gid) AND (name <> N'public') AND (name NOT LIKE 'db_%') " +
                    " ORDER BY name ";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (this.dtRoles == null) this.dtRoles = new DataTable();
                else this.dtRoles.Clear();
                a.Fill(this.dtRoles);
                this.listBoxRoles.DataSource = this.dtRoles;
                this.listBoxRoles.DisplayMember = "name";
                this.listBoxRoles.ValueMember = "name";
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetUserRoles()
        {
            try
            {
                string SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                    "SELECT sysusers_1.name " +
                    "FROM sysmembers INNER JOIN " +
                    "sysusers ON sysmembers.memberuid = sysusers.uid INNER JOIN " +
                    "sysusers sysusers_1 ON sysmembers.groupuid = sysusers_1.uid " +
                    "WHERE (replace(sysusers.name, '\', '\\') = N'" + this.CurrentLogin() + "') " +
                    "ORDER BY sysusers_1.name";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (this.dtUserRoles == null) this.dtUserRoles = new DataTable();
                else this.dtUserRoles.Clear();
                a.Fill(this.dtUserRoles);
                this.listBoxUserRoles.DataSource = this.dtUserRoles;
                this.listBoxUserRoles.DisplayMember = "name";
                this.listBoxUserRoles.ValueMember = "name";
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonRoleOverview_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxRoles.SelectedItem;
                string Role = R[0].ToString();
                string ConnectionString = this.CurrentServerConnection().ConnectionString;
                DiversityWorkbench.Forms.FormDatabaseRoles f = new FormDatabaseRoles(Role, ConnectionString);
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Projects

        private bool _ProjectsProvideReadOnly;
        private bool ProjectsProvideReadOnly
        {
            get
            {
                return this._ProjectsProvideReadOnly;
            }
        }

        private void listBoxProjectsReadOnly_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxProjectsReadOnly.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsReadOnly.SelectedItem;
                    int ProjectID;
                    if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
                        this.setCurrentProject(ProjectID);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void initProjects()
        {
            try
            {
                if (this.CurrentServerConnection() != null)
                {
                    string SQL = "use " + this.CurrentServerConnection().DatabaseName + "; SELECT COUNT(*) " +
                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                        "WHERE (TABLE_NAME = 'ProjectUser') AND (COLUMN_NAME = 'ReadOnly')";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                    {
                        this.splitContainerProjectAccessible.Panel2Collapsed = false;
                        this._ProjectsProvideReadOnly = true;
                    }
                    else
                    {
                        this.splitContainerProjectAccessible.Panel2Collapsed = true;
                        this._ProjectsProvideReadOnly = false;
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private void setProjectProxy()
        {
            if (this._DsProjectProxy != null
                && this._ProjectBindingSource != null)
            {
                this._ProjectBindingSource.EndEdit();
                this._adProjectProxy.Update(this._dtProjectProxy);
            }
            if (this.UserHasAccessToDatabase(this.CurrentServerConnection()) && this.ProjectsAndUserProxyDoExist(this.CurrentServerConnection()))
            {
                try
                {
                    DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryProject.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
                    this.userControlModuleRelatedEntryProject.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
                    if (this._DsProjectProxy == null)
                    {
                        this._DsProjectProxy = new DataSet();
                        this._dtProjectProxy = new DataTable("ProjectProxy");
                        this._DsProjectProxy.Tables.Add(this._dtProjectProxy);
                    }
                    string SQL = "SELECT ProjectID, Project, ProjectURI " +
                        "FROM " + this.CurrentServerConnection().DatabaseName + ".dbo.ProjectProxy";
                    if (this._adProjectProxy == null)
                        this._adProjectProxy = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    else
                        this._adProjectProxy.SelectCommand.CommandText = SQL;
                    if (this._dtProjectProxy.Rows.Count > 0)
                    {
                        this._adProjectProxy.Update(this._dtProjectProxy);
                        this._dtProjectProxy.Clear();
                    }
                    this._adProjectProxy.Fill(this._dtProjectProxy);
                    Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(this._adProjectProxy);
                    if (this._ProjectBindingSource == null)
                    {
                        this._ProjectBindingSource = new System.Windows.Forms.BindingSource(this.components);
                        ((System.ComponentModel.ISupportInitialize)(this._ProjectBindingSource)).BeginInit();
                        this._ProjectBindingSource.DataMember = "ProjectProxy";
                        this._ProjectBindingSource.DataSource = this._DsProjectProxy;
                        ((System.ComponentModel.ISupportInitialize)(this._ProjectBindingSource)).EndInit();
                        this.userControlModuleRelatedEntryProject.bindToData("ProjectProxy", "Project", "ProjectURI", this._ProjectBindingSource);
                    }

                }
                catch (System.Exception ex) { }
            }
            else
            {
                if (this._dtProjectProxy != null && this._dtProjectProxy.Rows.Count > 0)
                {
                    if (this._DsProjectProxy.HasChanges())
                        this._adProjectProxy.Update(this._dtProjectProxy);
                    this._dtProjectProxy.Clear();
                }
            }
            try
            {
                if (this.CurrentServerConnection() != null)
                {
                    string SQLUserProxy = "use master; " +
                        "select p.name, U.LoginName " +
                        "from sys.server_principals p, " +
                        "[" + this.CurrentServerConnection().DatabaseName + "].[dbo].[UserProxy] U " +
                        "where p.name not like '##%##'  " +
                        "and p.type in ('U')  " +
                        "and p.name not like 'NT-%'  " +
                        "and p.name <> 'sa' " +
                        "and p.name like '%\\%' " +
                        "and p.name not like 'NT %\\%' " +
                        "and U.LoginName like '%\\%' " +
                        "and substring(p.name, charindex('\\', p.name), 255) = substring(U.LoginName, charindex('\\', U.LoginName), 255)  " +
                        "and substring(U.LoginName, 1, charindex('\\', U.LoginName) - 1) <>substring(p.name, 1, charindex('\\', p.name) - 1) " +
                        "order by type desc, name;";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLUserProxy, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count == 0)
                        this.toolStripButtonSynchronizeUserProxy.Visible = false;// this.toolStripDatabase.Visible = false;
                    else
                        this.toolStripButtonSynchronizeUserProxy.Visible = true;// this.toolStripDatabase.Visible = true;
                }
                else
                    this.toolStripButtonSynchronizeUserProxy.Visible = true;// this.toolStripDatabase.Visible = true;
            }
            catch (System.Exception ex)
            {
                this.toolStripButtonSynchronizeUserProxy.Visible = false;
            }
        }

        private void setCurrentProject(int ProjectID)
        {
            if (this._dtProjectProxy != null)
            {
                for (int i = 0; i < this._dtProjectProxy.Rows.Count; i++)
                {
                    if (ProjectID.ToString() == this._dtProjectProxy.Rows[i]["ProjectID"].ToString())
                    {
                        if (this._ProjectBindingSource != null)
                            this._ProjectBindingSource.Position = i;
                        break;
                    }
                }
            }
        }

        #region Adding and removing within the lists

        private void buttonProjectAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxProjectsNotAvailable.SelectedValue == null)
                {
                    MessageBox.Show("Please select a project to be inserted!");
                    return;
                }
                //MW 28.4.2015: Enable multi transfer of projects
                this.ProjectID = System.Int32.Parse(this.listBoxProjectsNotAvailable.SelectedValue.ToString());

                string ProjectsToAdd = "";
                foreach (System.Object O in this.listBoxProjectsNotAvailable.SelectedItems)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    if (ProjectsToAdd.Length > 0) ProjectsToAdd += ", ";
                    ProjectsToAdd += R["ProjectID"].ToString();
                }
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string SQL = "use " + SC.DatabaseName + "; " +
                "INSERT INTO ProjectUser (LoginName, ProjectID) " +
                "SELECT '" + this.LoginName + "', ProjectID " +
                "FROM ProjectProxy WHERE ProjectID IN (" + ProjectsToAdd + ")";

                //string SQL = "use " + SC.DatabaseName + "; " +
                //   "INSERT INTO ProjectUser (LoginName, ProjectID) VALUES ('" + this.LoginName + "', " + this.ProjectID.ToString() + ")";
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                try
                {
                    Cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonProjectRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxProjectsAvailable.SelectedValue == null)
                {
                    MessageBox.Show("Please select a project to be removed!");
                    return;
                }
                this.ProjectID = System.Int32.Parse(this.listBoxProjectsAvailable.SelectedValue.ToString());
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string SQL = "use " + SC.DatabaseName + "; " +
                   "DELETE FROM ProjectUser WHERE LoginName = '" + this.LoginName + "' AND ProjectID = " + this.ProjectID.ToString();
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonProjectAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxProjectsNotAvailable.Items.Count == 0)
                    return;
                string ProjectsToAdd = "";
                foreach (System.Object O in this.listBoxProjectsNotAvailable.Items)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    if (ProjectsToAdd.Length > 0) ProjectsToAdd += ", ";
                    ProjectsToAdd += R["ProjectID"].ToString();
                }
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string SQL = "use " + SC.DatabaseName + "; " +
                "INSERT INTO ProjectUser (LoginName, ProjectID) " +
                "SELECT '" + this.LoginName + "', ProjectID " +
                "FROM ProjectProxy WHERE ProjectID IN (" + ProjectsToAdd + ")";

                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                try
                {
                    Cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonProjectRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxProjectsAvailable.Items.Count == 0)
                    return;
                string ProjectsToRemove = "";
                foreach (System.Object O in this.listBoxProjectsAvailable.Items)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    if (ProjectsToRemove.Length > 0) ProjectsToRemove += ", ";
                    ProjectsToRemove += R["ProjectID"].ToString();
                }
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string SQL = "use " + SC.DatabaseName + "; " +
                   "DELETE FROM ProjectUser WHERE LoginName = '" + this.LoginName + "' AND ProjectID IN (" + ProjectsToRemove + ")";
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        private void radioButtonOrderProjectByID_CheckedChanged(object sender, EventArgs e)
        {
            SetUserProjects();
        }

        private void SetUserProjects()
        {
            try
            {
                _LockingAvailable = this.ProjectLockingAvailable();
                string IsLocked = "";
                string IsLockedWhereClause = "";
                if (_LockingAvailable)
                {
                    IsLocked = ", ProjectProxy.IsLocked ";
                    IsLockedWhereClause = " AND (ProjectProxy.IsLocked = 0 OR ProjectProxy.IsLocked IS NULL) ";
                }
                this.listBoxProjectsLocked.Visible = _LockingAvailable;
                this.pictureBoxProjectsLocked.Visible = _LockingAvailable;

                // Accessible Projects
                string SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                    "SELECT ProjectProxy.ProjectID, ProjectProxy.Project " + IsLocked +
                    "FROM ProjectUser INNER JOIN " +
                    "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " +
                    "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this.CurrentLogin() + "') ";
                if (this.ProjectsProvideReadOnly)
                    SQL += " AND (ProjectUser.ReadOnly = 0 OR ProjectUser.ReadOnly IS NULL) ";
                SQL += " ORDER BY ProjectProxy.Project";
                if (this.radioButtonOrderProjectByID.Checked)
                {
                    SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                    "SELECT ProjectProxy.ProjectID, cast(ProjectProxy.ProjectID as varchar) + ' - ' + ProjectProxy.Project AS Project " + IsLocked +
                    "FROM ProjectUser INNER JOIN " +
                    "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " +
                    "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this.CurrentLogin() + "') ";
                    if (this.ProjectsProvideReadOnly)
                        SQL += " AND ProjectUser.ReadOnly = 0 ";
                    SQL += "ORDER BY ProjectProxy.ProjectID";
                }
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (this.dtProjectsAccess == null) this.dtProjectsAccess = new DataTable();
                else this.dtProjectsAccess.Clear();
                try
                {
                    a.Fill(this.dtProjectsAccess);
                    this.listBoxProjectsAvailable.DataSource = this.dtProjectsAccess;
                    this.listBoxProjectsAvailable.DisplayMember = "Project";
                    this.listBoxProjectsAvailable.ValueMember = "ProjectID";
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }

                // Read Only Projects
                if (ProjectsProvideReadOnly)
                {
                    SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                        "SELECT ProjectProxy.ProjectID, ProjectProxy.Project " + IsLocked +
                        "FROM ProjectUser INNER JOIN " +
                        "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " + IsLockedWhereClause +
                        "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this.CurrentLogin() + "') ";
                    if (this.ProjectsProvideReadOnly)
                        SQL += " AND ProjectUser.ReadOnly = 1 ";
                    SQL += " ORDER BY ProjectProxy.Project";
                    if (this.radioButtonOrderProjectByID.Checked)
                    {
                        SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                        "SELECT ProjectProxy.ProjectID, cast(ProjectProxy.ProjectID as varchar) + ' - ' + ProjectProxy.Project AS Project " + IsLocked +
                        "FROM ProjectUser INNER JOIN " +
                        "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " + IsLockedWhereClause +
                        "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this.CurrentLogin() + "') ";
                        if (this.ProjectsProvideReadOnly)
                            SQL += " AND ProjectUser.ReadOnly = 1 ";
                        SQL += "ORDER BY ProjectProxy.ProjectID";
                    }
                    a.SelectCommand.CommandText = SQL;
                    if (this.dtProjectsReadOnly == null) this.dtProjectsReadOnly = new DataTable();
                    else this.dtProjectsReadOnly.Clear();
                    a.Fill(this.dtProjectsReadOnly);
                    this.listBoxProjectsReadOnly.DataSource = this.dtProjectsReadOnly;
                    this.listBoxProjectsReadOnly.DisplayMember = "Project";
                    this.listBoxProjectsReadOnly.ValueMember = "ProjectID";

                    if (_LockingAvailable)
                    {
                        SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                            "SELECT ProjectProxy.ProjectID, ProjectProxy.Project " + IsLocked +
                            "FROM ProjectUser INNER JOIN " +
                            "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID AND ProjectProxy.IsLocked = 1 " +
                            "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this.CurrentLogin() + "') ";
                        if (this.ProjectsProvideReadOnly)
                            SQL += " AND ProjectUser.ReadOnly = 1 ";
                        SQL += " ORDER BY ProjectProxy.Project";
                        if (this.radioButtonOrderProjectByID.Checked)
                        {
                            SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                            "SELECT ProjectProxy.ProjectID, cast(ProjectProxy.ProjectID as varchar) + ' - ' + ProjectProxy.Project AS Project " + IsLocked +
                            "FROM ProjectUser INNER JOIN " +
                            "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID AND ProjectProxy.IsLocked = 1 " +
                            "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this.CurrentLogin() + "') ";
                            if (this.ProjectsProvideReadOnly)
                                SQL += " AND ProjectUser.ReadOnly = 1 ";
                            SQL += "ORDER BY ProjectProxy.ProjectID";
                        }
                        a.SelectCommand.CommandText = SQL;
                        if (this.dtProjectsLocked == null) this.dtProjectsLocked = new DataTable();
                        else this.dtProjectsLocked.Clear();
                        a.Fill(this.dtProjectsLocked);
                        this.listBoxProjectsLocked.DataSource = this.dtProjectsLocked;
                        this.listBoxProjectsLocked.DisplayMember = "Project";
                        this.listBoxProjectsLocked.ValueMember = "ProjectID";
                        //this.listBoxProjectsLocked.SelectedIndex = -1;
                    }
                }

                // not accessible Projects
                SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                    "SELECT ProjectID, Project " + IsLocked +
                    "FROM         ProjectProxy " +
                    "WHERE     (NOT (ProjectID IN " +
                    "(SELECT     ProjectID " +
                    "FROM          ProjectUser " +
                    "WHERE      (replace(LoginName, '\', '\\') = N'" + this.CurrentLogin() + "')))) ORDER BY Project";
                if (this.radioButtonOrderProjectByID.Checked)
                    SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                        "SELECT ProjectID, cast(ProjectProxy.ProjectID as varchar) + ' - ' + Project AS Project " + IsLocked +
                        "FROM         ProjectProxy " +
                        "WHERE     (NOT (ProjectID IN " +
                        "(SELECT     ProjectID " +
                        "FROM          ProjectUser " +
                        "WHERE      (replace(LoginName, '\', '\\') = N'" + this.CurrentLogin() + "')))) ORDER BY ProjectID";
                a.SelectCommand.CommandText = SQL;

                //blocked projects
                if (this.dtProjectsBlocked == null) this.dtProjectsBlocked = new DataTable();
                else this.dtProjectsBlocked.Clear();
                a.Fill(this.dtProjectsBlocked);
                this.listBoxProjectsNotAvailable.DataSource = this.dtProjectsBlocked;
                this.listBoxProjectsNotAvailable.DisplayMember = "Project";
                this.listBoxProjectsNotAvailable.ValueMember = "ProjectID";

                this.buttonProjectUserAvailableIsLocked.Visible = _LockingAvailable;
                this.buttonProjectUserNotAvailableIsLocked.Visible = _LockingAvailable;

                if (_LockingAvailable)
                {
                    this.initProjectLocking();
                }
                //else
                //{
                //    this.buttonProjectUserAvailableIsLocked.Visible = false;
                //    this.buttonProjectUserNotAvailableIsLocked.Visible = false;
                //}
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonSynchronizeProjects_Click(object sender, EventArgs e)
        {
            if (this._dtProjectProxy != null)
            {
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                DiversityWorkbench.Forms.FormLoginAdminGetProjects f = new FormLoginAdminGetProjects(this._dtProjectProxy, SC, this.helpProvider.HelpNamespace);
                if (SC.ModuleName.StartsWith("Diversity"))
                {
                    string Type = SC.ModuleName.Substring(9);
                    f.ProjectType = Type;
                }
                f.setHelp("Login administration");
                f.ShowDialog();
                if (f.AddedProjectRows.Count > 0)
                {
                    foreach (System.Data.DataRow R in f.AddedProjectRows)
                    {
                        System.Data.DataRow[] rr = this._dtProjectProxy.Select("ProjectID = " + R["ProjectID"].ToString());
                        if (rr.Length == 0)
                        {
                            System.Data.DataRow RN = this._dtProjectProxy.NewRow();
                            RN["ProjectID"] = R["ProjectID"];
                            RN["Project"] = R["Project"];
                            RN["ProjectURI"] = R["ProjectURI"];
                            this._dtProjectProxy.Rows.Add(RN);
                        }
                        else
                        {
                            if (R["Project"].ToString() == rr[0]["Project"].ToString())
                            {
                                rr[0]["ProjectURI"] = R["ProjectURI"];
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("The ProjectID " + R["ProjectID"].ToString() + " for the project " + R["Project"].ToString() + " is used for the project " + rr[0]["Project"].ToString());
                            }
                        }
                    }
                    this.setProjectProxy();
                    this.SetUserProjects();
                }
                //this._adProjectProxy.Update(this._dtProjectProxy);
            }
            else System.Windows.Forms.MessageBox.Show("Please select a database containing projects");
        }

        private void buttonShowProject_Click(object sender, EventArgs e)
        {
        }

        private void listBoxProjectsNotAvailable_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsNotAvailable.SelectedItem;
            int ProjectID;
            if (R != null && int.TryParse(R["ProjectID"].ToString(), out ProjectID))
                this.setCurrentProject(ProjectID);
        }

        private void listBoxProjectsAvailable_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsAvailable.SelectedItem;
                int ProjectID;
                if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
                    this.setCurrentProject(ProjectID);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonRemoveProject_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if anything 
                if (this.listBoxProjectsNotAvailable.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("In the No access list, no project had been selected");
                    return;
                }
                string Project = "";
                if (this.listBoxProjectsNotAvailable.SelectedItem.GetType() == typeof(System.Data.DataRowView))
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsNotAvailable.SelectedItem;
                    Project = R["Project"].ToString();
                }
                else return;
                if (System.Windows.Forms.MessageBox.Show("Do really want to delete the project " + Project + "?\r\nAny related data must be removed from this project first!", "Delete project?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    return;

                string ProjectID = "";
                if (this.listBoxProjectsNotAvailable.SelectedValue.GetType() == typeof(System.Data.DataRowView))
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsNotAvailable.SelectedValue;
                    ProjectID = R["ProjectID"].ToString();
                }
                else if (this.listBoxProjectsNotAvailable.SelectedValue.GetType() == typeof(int))
                {
                    ProjectID = this.listBoxProjectsNotAvailable.SelectedValue.ToString();
                }

                // Check the presence of users in this project
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string SQL = "use " + SC.DatabaseName + "; " +
                    "SELECT U.CombinedNameCache " +
                    "FROM  ProjectUser AS P INNER JOIN " +
                    "UserProxy AS U ON P.LoginName = U.LoginName " +
                    "WHERE P.ProjectID = " + ProjectID +
                    " ORDER BY U.CombinedNameCache";
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();

                System.Data.DataTable dtProjectUser = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.CurrentServerConnection().ConnectionString);
                a.Fill(dtProjectUser);
                if (dtProjectUser.Rows.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show(dtProjectUser.Rows.Count.ToString() + " Users will be removed from this project", "Remove project user", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }
                string ProjectRelatedTable = "";
                string RelatedDataName = "";
                switch (SC.ModuleName)
                {
                    case "DiversityCollection":
                        ProjectRelatedTable = "CollectionProject";
                        RelatedDataName = "specimen(s)";
                        break;
                    case "DiversityDescriptions":
                        if (ProjectID == "0")
                            ProjectRelatedTable = "(SELECT 0 AS ProjectID)";
                        else
                            ProjectRelatedTable = "(SELECT [Description].id AS DescriptionID, Project.ProjectProxyID AS ProjectID FROM [Description] INNER JOIN Project ON [Description].project_id=Project.id)";
                        RelatedDataName = "description(s)";
                        break;
                    case "DiversityAgents":
                        ProjectRelatedTable = "AgentProject";
                        RelatedDataName = "agent(s)";
                        break;
                    case "DiversityReferences":
                        ProjectRelatedTable = "ReferenceProject";
                        RelatedDataName = "reference(s)";
                        break;
                    case "DiversitySamplingPlots":
                        ProjectRelatedTable = "SamplingProject";
                        RelatedDataName = "sampling plot(s)";
                        break;
                    case "DiversityTaxonNames":
                        ProjectRelatedTable = "TaxonNameProject";
                        RelatedDataName = "taxonomic name(s)";
                        break;
                }
                if (ProjectRelatedTable.Length > 0)
                {
                    Cmd.CommandText = "use " + SC.DatabaseName + "; " +
                        "SELECT COUNT(*) " +
                        "FROM  " + ProjectRelatedTable + " AS P " +
                        "WHERE P.ProjectID = " + ProjectID;
                    try
                    {
                        string Result = Cmd.ExecuteScalar().ToString();
                        if (Result != "0")
                        {
                            System.Windows.Forms.MessageBox.Show(Result + " " + RelatedDataName + " are still in this project.\r\nThese must be removed first");
                            return;
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                Cmd.CommandText = "use " + SC.DatabaseName + "; DELETE FROM ProjectUser WHERE ProjectID = " + ProjectID;
                try
                {
                    Cmd.ExecuteNonQuery();
                    Cmd.CommandText = "use " + SC.DatabaseName + "; DELETE FROM ProjectProxy WHERE ProjectID = " + ProjectID;
                    Cmd.ExecuteNonQuery();
                    System.Windows.Forms.MessageBox.Show("Project " + Project + " deleted");
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Deleting failed:\r\n" + ex.Message);
                }
                this.SqlConnection.Close();
                this.initProjects();
                this.SetUserProjects();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Deleting failed:\r\n" + ex.Message);
            }
        }

        #region Read only

        private void buttonProjectsReadOnlyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ProjectID = System.Int32.Parse(this.listBoxProjectsAvailable.SelectedValue.ToString());
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string SQL = "use " + SC.DatabaseName + "; " +
                   "UPDATE PU SET ReadOnly = 1 FROM ProjectUser PU WHERE LoginName = '" + this.LoginName + "' AND ProjectID = " + this.ProjectID.ToString();
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch (System.Exception ex) { }
        }

        private void buttonProjectsReadOnlyRemove_Click(object sender, EventArgs e)
        {
            try
            {
                this.ProjectID = System.Int32.Parse(this.listBoxProjectsReadOnly.SelectedValue.ToString());
                DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                string SQL = "use " + SC.DatabaseName + "; " +
                   "UPDATE PU SET ReadOnly = 0 FROM ProjectUser PU WHERE LoginName = '" + this.LoginName + "' AND ProjectID = " + this.ProjectID.ToString();
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch (System.Exception ex) { }
        }

        private void pictureBoxProjectsLocked_Click(object sender, EventArgs e)
        {
            try
            {
                this.ProjectID = -1;
                if (this.listBoxProjectsLocked.SelectedValue != null)
                    ProjectID = System.Int32.Parse(this.listBoxProjectsLocked.SelectedValue.ToString());
                else if (this.listBoxProjectsLocked.Items.Count == 1)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsLocked.Items[0];
                    ProjectID = int.Parse(R["ProjectID"].ToString());
                }
                if (ProjectID > -1)
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to remove read only restriction from the locked project?", "Remove Read Only", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    DiversityWorkbench.ServerConnection SC = this.CurrentServerConnection();
                    string SQL = "use " + SC.DatabaseName + "; " +
                       "UPDATE PU SET ReadOnly = 0 FROM ProjectUser PU WHERE LoginName = '" + this.LoginName + "' AND ProjectID = " + this.ProjectID.ToString();
                    Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                    if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                    Cmd.ExecuteNonQuery();
                    this.SqlConnection.Close();
                    this.SetUserProjects();
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Users for Project

        private void buttonProjectUserNotAvailable_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectsNotAvailable.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
            }
            else
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsNotAvailable.SelectedItem;
                this.ShowProjectUserList(R);
            }
        }

        private void buttonProjectUserAvailable_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectsAvailable.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
            }
            else
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsAvailable.SelectedItem;
                this.ShowProjectUserList(R);
            }
        }

        private void ShowProjectUserList(System.Data.DataRowView R)
        {
            try
            {
                // getting the roles
                this._ProjectRolesID = int.Parse(R["ProjectID"].ToString());
                System.Data.DataTable dt = this.GetDtProjectRoles(this._ProjectRolesID);
                string Project = R["Project"].ToString();
                this._fProjectRoles = new FormTableContent("Project " + Project, Project + ":\r\nAll users that have access to the project and their roles" +
                    "    ROLES ARE NOT RESTRICTED TO PROJECTS !!!" +
                    "\r\n(X = disabled)", dt);
                this._fProjectRoles.RowHeaderVisible(true);
                this._fProjectRoles.dataGridView.RowHeadersWidth = 24;
                this._fProjectRoles.dataGridView.Columns[0].CellTemplate.Style.ForeColor = System.Drawing.Color.Red;
                System.Drawing.Font fRoles = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Bold);
                //foreach (System.Windows.Forms.DataGridViewColumn C in this._fProjectRoles.dataGridView.Columns)
                //    C.ReadOnly = true;
                for (int i = 5; i < this._fProjectRoles.dataGridView.Columns.Count; i++)
                {
                    if (this._ProjectsProvideReadOnly && i == 5) continue;
                    this._fProjectRoles.dataGridView.Columns[i].CellTemplate.Style.ForeColor = System.Drawing.Color.Green;
                    this._fProjectRoles.dataGridView.Columns[i].CellTemplate.Style.Font = fRoles;
                    this._fProjectRoles.dataGridView.Columns[i].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //this._fProjectRoles.dataGridView.Columns[i].ReadOnly = false;
                }
                // Context menu
                System.Windows.Forms.ContextMenuStrip CMS = new System.Windows.Forms.ContextMenuStrip();
                CMS.SuspendLayout();
                // Add role
                System.Windows.Forms.ToolStripMenuItem toolStripMenuAddRole = new System.Windows.Forms.ToolStripMenuItem();
                toolStripMenuAddRole.Image = global::DiversityWorkbench.Properties.Resources.Add;
                toolStripMenuAddRole.Name = "toolStripMenuAddRole";
                toolStripMenuAddRole.Size = new System.Drawing.Size(185, 22);
                toolStripMenuAddRole.Text = "Add role";
                // Remove role
                System.Windows.Forms.ToolStripMenuItem toolStripMenuRemoveRole = new System.Windows.Forms.ToolStripMenuItem();
                toolStripMenuRemoveRole.Image = global::DiversityWorkbench.Properties.Resources.Delete;
                toolStripMenuRemoveRole.Name = "toolStripMenuRemoveRole";
                toolStripMenuRemoveRole.Size = new System.Drawing.Size(185, 22);
                toolStripMenuRemoveRole.Text = "Remove role";

                CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    toolStripMenuAddRole,
                    toolStripMenuRemoveRole});
                CMS.Name = "contextMenuStripDataGrid";
                this.helpProvider.SetShowHelp(CMS, true);
                CMS.Size = new System.Drawing.Size(186, 48);
                CMS.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripDataGrid_ItemClicked);
                CMS.ResumeLayout(false);

                this._fProjectRoles.dataGridView.ContextMenuStrip = CMS;
                this._fProjectRoles.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }

        private System.Data.DataTable GetDtProjectRoles(int ProjectID)
        {
            System.Data.DataTable dt = new DataTable();
            try
            {
                string SQL = "USE " + this.CurrentServerConnection().DatabaseName + "; " +
                    " SELECT DISTINCT " +
                    " case when s.is_disabled = 1 then 'X' else NULL end as [X], " +
                    " CASE WHEN U.CombinedNameCache IS NULL or rtrim(U.CombinedNameCache) = '' THEN P.LoginName ELSE U.CombinedNameCache END AS [Name of user], " +
                    " P.LoginName AS [Login]," +
                    " U.AgentURI AS [URI of user], " +
                    " case when s.type = 'S' then 'SQL-Server' else 'Windows' end as [Type] ";
                if (this._ProjectsProvideReadOnly)
                    SQL += ", case when P.[ReadOnly] = 1 then 'ReadOnly' else NULL end AS [Read only]  ";
                foreach (System.Data.DataRow rRoles in this.dtRoles.Rows)
                {
                    SQL += ", '' AS [" + rRoles[0].ToString() + "]";
                }
                SQL += " FROM sys.server_principals S, ProjectUser AS P LEFT OUTER JOIN " +
                    " UserProxy AS U ON P.LoginName = U.LoginName " +
                    " WHERE P.ProjectID = " + ProjectID.ToString() +
                    " and (substring(s.Name, charindex('\\', s.Name) + 1, 255) = P.LoginName OR P.LoginName='dbo')" +
                    " ORDER BY [Name of user]";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow rUser in dt.Rows)
                {
                    SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                        "SELECT sysusers_1.name  " +
                        "FROM sysmembers INNER JOIN  " +
                        "sysusers ON sysmembers.memberuid = sysusers.uid INNER JOIN  " +
                        "sysusers sysusers_1 ON sysmembers.groupuid = sysusers_1.uid  " +
                        "WHERE substring(sysusers.Name, charindex('\', sysusers.Name) + 1, 255) = N'" + rUser["Login"].ToString() + "' " +
                        "ORDER BY sysusers_1.name;";
                    System.Data.DataTable dtUserRoles = new DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtUserRoles, ref Message);
                    foreach (System.Data.DataRow rUserRole in dtUserRoles.Rows)
                    {
                        if (rUser.Table.Columns.Contains(rUserRole["name"].ToString()))
                            rUser[rUserRole["name"].ToString()] = "•";
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }

        private DiversityWorkbench.Forms.FormTableContent _fProjectRoles;
        private int _ProjectRolesID;

        private void contextMenuStripDataGrid_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this._fProjectRoles != null)
            {
                string RoleColumn = this._fProjectRoles.dataGridView.Columns[this._fProjectRoles.dataGridView.SelectedCells[0].ColumnIndex].Name;
                string Login = this._fProjectRoles.dataGridView.Rows[this._fProjectRoles.dataGridView.SelectedCells[0].RowIndex].Cells[2].Value.ToString();
                System.Data.DataRow[] rr = this.dtRoles.Select("name = '" + RoleColumn + "'", "");
                if (rr.Length == 1)
                {
                    if (e.ClickedItem.Name == "toolStripMenuAddRole")
                    {
                        this.RoleAdd(RoleColumn, Login);
                        this._fProjectRoles.dataGridView.DataSource = this.GetDtProjectRoles(this._ProjectRolesID);
                    }
                    else if (e.ClickedItem.Name == "toolStripMenuRemoveRole")
                    {
                        this.RoleRemove(RoleColumn, Login);
                        this._fProjectRoles.dataGridView.DataSource = this.GetDtProjectRoles(this._ProjectRolesID);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please select a role colum");
                }
            }
        }


        #endregion

        #region Locking Projects

        private bool ProjectLockingAvailable()
        {
            bool LockingAvailable = false;
            string SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'ProjectProxy' and C.COLUMN_NAME = 'IsLocked';";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
            LockingAvailable = (Result == "1");
            return LockingAvailable;
        }

        private void initProjectLocking()
        {
            this.setProjectUserAvailableIsLockedState();
            this.setProjectUserNotAvailableIsLockedState();
            //if (LockingAvailable)
            //{
            //    LockingAvailable = true;
            //    if (this.listBoxProjectsAvailable.SelectedItem != null)
            //    {
            //    }
            //    if (this.listBoxProjectsNotAvailable.SelectedItem != null)
            //    {
            //    }
            //}
        }

        private void buttonProjectUserNotAvailableIsLocked_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectsNotAvailable.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsNotAvailable.SelectedItem;
                if (this.ChangeProjectLockedState(R))
                    this.SetUserProjects();
            }
        }

        private void buttonProjectUserAvailableIsLocked_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectsAvailable.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectsAvailable.SelectedItem;
                if (this.ChangeProjectLockedState(R))
                    this.SetUserProjects();
            }
        }

        private bool ChangeProjectLockedState(System.Data.DataRowView R)
        {
            if (!_LockingAvailable)
                return false;

            bool IsAdmin = false;
            if (DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                IsAdmin = true;

            if (!IsAdmin)
            {
                System.Windows.Forms.MessageBox.Show("To change the locked state of a project you have to be dbo (= database owner)");
                return false;
            }
            bool OK = false;
            bool IsLocked = false;
            if (!R["IsLocked"].Equals(System.DBNull.Value))
            {
                bool.TryParse(R["IsLocked"].ToString(), out IsLocked);
            }
            int ProjectID;
            if (R != null && int.TryParse(R["ProjectID"].ToString(), out ProjectID))
            {
                string Message = "Do you want to ";
                if (IsLocked) Message += "unlock project " + R["Project"].ToString() + "?\r\nThe read only state for users will be kept";
                else Message += "lock project " + R["Project"].ToString() + "?\r\nThe access for users will be set to read only for all datasets that are linked to this project.\r\nPlease make sure that you really want to lock this project"; ;
                if (System.Windows.Forms.MessageBox.Show(Message, "Change locking of project", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    string SQL = "use " + this.CurrentServerConnection().DatabaseName + "; " +
                    "UPDATE P SET P.IsLocked = ";
                    if (IsLocked) SQL += "0";
                    else SQL += "1";
                    SQL += " FROM ProjectProxy P WHERE P.ProjectID = " + ProjectID.ToString();
                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    if (OK)
                    {
                        if (IsLocked) Message = "Project " + R["Project"].ToString() + " is now unlocked. The read only state for users has not been changed";
                        else Message = "Project " + R["Project"].ToString() + " is now locked. The access for users is set to read only";
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                }
            }
            return OK;
        }

        private void setProjectUserAvailableIsLockedState()
        {
            System.Data.DataRowView R = null;
            if (this.listBoxProjectsAvailable.SelectedItem != null)
            {
                R = (System.Data.DataRowView)this.listBoxProjectsAvailable.SelectedItem;
            }
            this.setProjectLockedState(R, this.buttonProjectUserAvailableIsLocked);
        }

        private void setProjectUserNotAvailableIsLockedState()
        {
            System.Data.DataRowView R = null;
            if (this.listBoxProjectsNotAvailable.SelectedItem != null)
            {
                R = (System.Data.DataRowView)this.listBoxProjectsNotAvailable.SelectedItem;
            }
            this.setProjectLockedState(R, this.buttonProjectUserNotAvailableIsLocked);
        }

        private void setProjectLockedState(System.Data.DataRowView R, System.Windows.Forms.Button button)
        {
            if (!_LockingAvailable)
                return;

            if (R != null)
            {
                string Project = R["Project"].ToString();
                bool IsLocked = false;
                if (!R["IsLocked"].Equals(System.DBNull.Value))
                {
                    bool.TryParse(R["IsLocked"].ToString(), out IsLocked);
                }
                if (IsLocked)
                {
                    button.Image = Properties.Resources.Project;
                    this.toolTip.SetToolTip(button, "Project " + Project + " is locked, access restricted to read only");
                }
                else
                {
                    button.Image = Properties.Resources.ProjectOpen;
                    this.toolTip.SetToolTip(button, "Data in project " + Project + " can be edited");
                }
            }
            else
            {
                button.Image = Properties.Resources.ProjectGrey;
                this.toolTip.SetToolTip(button, "No project selected");
            }
        }

        private void listBoxProjectsNotAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setProjectUserNotAvailableIsLockedState();
        }

        private void listBoxProjectsAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setProjectUserAvailableIsLockedState();
        }

        #endregion

        #endregion

        #endregion

        #region Lists

        //private void InitLists()
        //{
        //    this.SetLogins();
        //    this.SetUser();
        //    this.SetUserProjects();
        //    this.SetRoles();
        //}

        //private void SetLogins()
        //{
        //    try
        //    {
        //        string SQL = "SELECT name FROM sysusers WHERE (issqluser = 1 OR isntuser = 1) " +
        //            " AND name NOT IN (SELECT LoginName FROM UserProxy) and name <> 'sys' and name <> 'INFORMATION_SCHEMA' " +
        //            " ORDER BY name ";
        //        Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
        //        if (this.dtLogins == null) this.dtLogins = new DataTable();
        //        else this.dtLogins.Clear();
        //        a.Fill(this.dtLogins);
        //        //this.listBoxDatabaseAccounts.DataSource = this.dtLogins;
        //        //this.listBoxDatabaseAccounts.DisplayMember = "name";
        //        //this.listBoxDatabaseAccounts.ValueMember = "name";

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void SetUser()
        //{
        //    try
        //    {
        //        string SQL = "SELECT LoginName, CombinedNameCache, UserURI FROM UserProxy WHERE LoginName IN (SELECT name FROM sysusers WHERE issqluser = 1 OR isntuser = 1) ORDER BY LoginName";
        //        Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
        //        if (this.dtUser == null) this.dtUser = new DataTable();
        //        else this.dtUser.Clear();
        //        a.Fill(this.dtUser);
        //        //this.listBoxUser.DataSource = this.dtUser;
        //        //this.listBoxUser.DisplayMember = "LoginName";
        //        //this.listBoxUser.ValueMember = "LoginName";
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void listBoxUser_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    //int i = this.listBoxUser.SelectedIndex;
        //    //if (i > -1)
        //    //{
        //    //    System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxUser.Items[i];
        //    //    this.User = rv[0].ToString();
        //    //    this.SetUserProjects();
        //    //    this.setUserRoles();
        //    //    this.textBoxCombinedNameCache.Text = rv[1].ToString();
        //    //    if (rv[2].Equals(System.DBNull.Value) || rv[2].ToString().Length == 0)
        //    //        this.labelUserURI.Visible = false;
        //    //    else
        //    //    {
        //    //        this.labelUserURI.Text = "URI: " + rv[2].ToString();// "URI of user: " + rv[2].ToString();
        //    //        this.labelUserURI.Visible = true;
        //    //    }
        //    //    this.setAgent(this.User);
        //    //}
        //}

        //private void SetServerLogins()
        //{
        //    string SQL = "SELECT name FROM syslogins WHERE (issqluser = 1 OR isntuser = 1) " +
        //        " AND name NOT IN (SELECT LoginName FROM UserProxy) " +
        //        " ORDER BY name ";
        //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
        //    if (this.dtLogins == null) this.dtLogins = new DataTable();
        //    else this.dtLogins.Clear();
        //    a.Fill(this.dtLogins);
        //    //this.listBoxDatabaseAccounts.DataSource = this.dtLogins;
        //    //this.listBoxDatabaseAccounts.DisplayMember = "name";
        //    //this.listBoxDatabaseAccounts.ValueMember = "name";
        //}


        #endregion

        #region Privacy consent info

        //private string _PrivacyConsentInfoRoutine = "PrivacyConsentInfo";
        private string _PrivactInfoSite;
        private void initPrivayInfo()
        {
            string SQL = "SELECT dbo." + DiversityWorkbench.Database.PrivacyConsentInfoRoutine + "()";
            string Message = "";
            string Site = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
            if (Message.Length == 0 && Site.Length > 0)
            {
                this.buttonSetPrivacyConsentInfoSite.Visible = true;
                this._PrivactInfoSite = Site;
            }
        }

        private void buttonSetPrivacyConsentInfoSite_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new FormWebBrowser(this._PrivactInfoSite);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (DiversityWorkbench.Database.CreateOrUpdatePrivacyConsentProcedure(f.URL))
                    this._PrivactInfoSite = f.URL;
            }
        }


        #endregion

    }
}
