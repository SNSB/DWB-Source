using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.PostgreSQL
{
    public partial class FormConnect : Form
    {
        //private System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Server> _ServerList;
        //private static string Password = "";

        #region Construction
        
        /// <summary>
        /// Login for Postgres database
        /// </summary>
        /// <param name="Server">Name resp. IP of the postgres server</param>
        /// <param name="Port">Port of the postgres server</param>
        /// <param name="Database">Database on the postgres server</param>
        /// <param name="User">Name of the login on the postgres server</param>
        public FormConnect(string Server, int Port, string Database, string User)
        {
            InitializeComponent();
            this.textBoxPort.Text = Port.ToString();
            this.textBoxRole.Text = User;
            this.textBoxDatabase.Text = Database;
            this.textBoxServer.Text = Server;
            this.initForm();
        }

        public FormConnect()
        {
            InitializeComponent();
            this.textBoxPort.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Port.ToString();
            this.textBoxRole.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Role; ;
            this.textBoxDatabase.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Database;
            this.textBoxServer.Text = DiversityWorkbench.PostgreSQL.Settings.Default.Server;
            this.textBoxPassword.Text = DiversityWorkbench.PostgreSQL.Connection.Password;
            this.initForm();
        }
        
        #endregion

        #region From and events

        private void initForm()
        {
            this.textBoxRole.Enabled = true;
            this.textBoxPassword.Enabled = true;
            this.buildHierarchy();
            this.Height = (int)(230 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void FormConnectToDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bool OK = true;
                //if (this.comboBoxDatabase.Text.Length == 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("No database has been selected");
                //    OK = false;
                //}
                //if (this.textBoxPort.Text.Length == 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("No port was given");
                //    OK = false;
                //}
                //if (this.comboBoxServer.Text.Length == 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("No server has been selected");
                //    OK = false;
                //}
                //if (OK && this.DialogResult == DialogResult.OK)
                //{
                //    if (this._ForMainModule)
                //    {
                //        Settings.DatabaseName = this.comboBoxDatabase.Text;
                //        Settings.DatabasePort = System.Int32.Parse(this.textBoxPort.Text);
                //        Settings.DatabaseServer = this.comboBoxServer.Text;
                //        Settings.IsTrustedConnection = this.radioButtonAuthentication.Checked;
                //    }
                //    else if (this._LeaveMainConnectionUnchanged)
                //    {
                //        this.LocalServerConnection.DatabaseName = this.comboBoxDatabase.Text;
                //        this.LocalServerConnection.DatabaseServerPort = int.Parse(this.textBoxPort.Text);
                //        this.LocalServerConnection.DatabaseServer = this.comboBoxServer.Text;
                //    }
                //    else
                //    {
                //        if (Settings.DatabaseName.Length == 0)
                //            Settings.DatabaseName = this.comboBoxDatabase.Text;
                //        if (Settings.DatabasePort == 0)
                //            Settings.DatabasePort = System.Int32.Parse(this.textBoxPort.Text);
                //        if (Settings.DatabaseServer.Length == 0)
                //            Settings.DatabaseServer = this.comboBoxServer.Text;
                //    }
                //    Settings.IsLocalExpressDatabase = false;
                //    if (!this.radioButtonAuthentication.Checked && !this._LeaveMainConnectionUnchanged)
                //    {
                //        Settings.DatabaseUser = this.textBoxUser.Text;
                //        Settings.Password = this.textBoxPassword.Text;
                //    }
                //    else if (this._LeaveMainConnectionUnchanged)
                //    {
                //        this.LocalServerConnection.DatabaseUser = this.textBoxUser.Text;
                //        this.LocalServerConnection.DatabasePassword = this.textBoxPassword.Text;
                //    }
                //    {
                //        if (DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList == null)
                //            DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList = new System.Collections.Specialized.StringCollection();
                //        if (!DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Contains(Settings.DatabaseServer))
                //        {
                //            DiversityWorkbench.WorkbenchSettings.Default.DatabaseServerList.Add(Settings.DatabaseServer);
                //            DiversityWorkbench.WorkbenchSettings.Default.Save();
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
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

        public void buildHierarchy()
        {
            try
            {
                this.toolStripDropDownButton.DropDownItems.Clear();
                //foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Server> KV in DiversityWorkbench.PostgreSQL.Connection.FormerConnections())
                //{
                //    System.Windows.Forms.ToolStripMenuItem MS = new ToolStripMenuItem(KV.Value.Name + ", " + KV.Value.Port.ToString(), this.imageListPgObjects.Images[0], this.ToolStripMenuItem_Click);
                //    MS.Tag = KV.Value;
                //    this.toolStripDropDownButton.DropDownItems.Add(MS);
                //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Role> KV_role in KV.Value.Roles)
                //    {
                //        System.Windows.Forms.ToolStripMenuItem MS_role = new ToolStripMenuItem(KV_role.Value.Name, this.imageListPgObjects.Images[2], this.ToolStripMenuItem_Click);
                //        MS_role.Tag = KV.Value;
                //        MS.DropDownItems.Add(MS_role);
                //        foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV_DB in KV_role.Value.Databases)
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

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem M = (System.Windows.Forms.ToolStripMenuItem)sender;
            if (M.Tag != null)
            {
                if (M.Tag.GetType() == typeof(DiversityWorkbench.PostgreSQL.Server))
                {
                    DiversityWorkbench.PostgreSQL.Server S =(DiversityWorkbench.PostgreSQL.Server)M.Tag;
                    this.textBoxServer.Text = S.Name;
                    this.textBoxPort.Text = S.Port.ToString();
                }
                else if (M.Tag.GetType() == typeof(DiversityWorkbench.PostgreSQL.Role))
                {
                    //DiversityWorkbench.PostgreSQL.Role R = (DiversityWorkbench.PostgreSQL.Role)M.Tag;
                    //this.textBoxServer.Text = R.Server.Name;
                    //this.textBoxPort.Text = R.Server.Port.ToString();
                    //this.textBoxRole.Text = R.Name;
                }
                else if (M.Tag.GetType() == typeof(DiversityWorkbench.PostgreSQL.Database))
                {
                    DiversityWorkbench.PostgreSQL.Database D = (DiversityWorkbench.PostgreSQL.Database)M.Tag;
                    //this.textBoxServer.Text = D.Role.Server.Name;
                    //this.textBoxPort.Text = D.Role.Server.Port.ToString();
                    //this.textBoxDatabase.Text = D.Name;
                    //this.textBoxRole.Text = D.Role.Name;
                }
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

        private string HierarchyString(string Key)
        {
            if (Key.Length == 0) return "";
            string Hierarchy = "";
            //try
            //{
            //    string Select = this._QueryCondition.HierarchyColumn + " = '" + Key + "'";
            //    System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Select, this._QueryCondition.OrderColumn);
            //    if (rr.Length > 0)
            //    {
            //        Hierarchy = this.HierarchyString(rr[0][this._QueryCondition.HierarchyParentColumn].ToString());
            //        if (Hierarchy.Length > 0) Hierarchy += " - ";
            //        Hierarchy += rr[0][this._QueryCondition.HierarchyDisplayColumn].ToString();
            //    }
            //}
            //catch (System.Exception ex) { }
            return Hierarchy;
        }

        private string getHierarchyChildValueList(string ParentID)
        {
            string Childs = "";
            //try
            //{
            //    System.Collections.Generic.List<string> ChildIDList = new List<string>();
            //    string Restriction = this._QueryCondition.HierarchyParentColumn + " = ";
            //    int i;
            //    if (!int.TryParse(ParentID, out i))
            //        Restriction += "'";
            //    Restriction += ParentID;
            //    if (!int.TryParse(ParentID, out i))
            //        Restriction += "'";
            //    System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Restriction);
            //    foreach (System.Data.DataRow R in rr)
            //    {
            //        ChildIDList.Add(R[this._QueryCondition.HierarchyColumn].ToString());
            //        this.getHierarchyChildValueList(R[this._QueryCondition.HierarchyColumn].ToString(), ref ChildIDList);
            //    }
            //    foreach (string s in ChildIDList)
            //    {
            //        //if (Childs.Length > 0)
            //        Childs += ", ";
            //        if (!this._QueryCondition.IsNumeric) Childs += "'";
            //        Childs += s;
            //        if (!this._QueryCondition.IsNumeric) Childs += "'";
            //    }
            //}
            //catch (System.Exception ex) { }
            return Childs;
        }

        private void getHierarchyChildValueList(string ParentID, ref System.Collections.Generic.List<string> ChildIDList)
        {
            try
            {
                //string Restriction = this._QueryCondition.HierarchyParentColumn + " = ";
                //int i;
                //if (!int.TryParse(ParentID, out i)) Restriction += "'";
                //Restriction += ParentID;
                //if (!int.TryParse(ParentID, out i)) Restriction += "'";

                //System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Restriction);
                //foreach (System.Data.DataRow R in rr)
                //{
                //    ChildIDList.Add(R[this._QueryCondition.HierarchyColumn].ToString());
                //    this.getHierarchyChildValueList(R[this._QueryCondition.HierarchyColumn].ToString(), ref ChildIDList);
                //}
            }
            catch (System.Exception ex) { }
        }

        #endregion

        //#region Authentication

        //private void ChangeAuthentication()
        //{
        //    if (this.radioButtonAuthentication.Checked)
        //    {
        //        this.textBoxPassword.Enabled = false;
        //        this.textBoxPassword.BackColor = System.Drawing.SystemColors.ActiveBorder;
        //        this.textBoxUser.Enabled = false;
        //        this.textBoxUser.BackColor = System.Drawing.SystemColors.ActiveBorder;
        //    }
        //    else
        //    {
        //        this.textBoxPassword.Enabled = true;
        //        this.textBoxPassword.BackColor = System.Drawing.Color.White;
        //        this.textBoxUser.Enabled = true;
        //        this.textBoxUser.BackColor = System.Drawing.Color.White;
        //    }
        //    this.pictureBoxDatabaseLogin.Enabled = this.radioButtonAuthentication.Checked;
        //    this.pictureBoxWindowsLogin.Enabled = !this.radioButtonAuthentication.Checked;

        //    this.comboBoxDatabase.Enabled = false;
        //    this.comboBoxDatabase.DataSource = null;
        //    this.userControlDialogPanel.buttonOK.Enabled = false;
        //}

        //private void radioButtonAuthentication_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.ChangeAuthentication();
        //}

        //private void textBoxUser_Leave(object sender, EventArgs e)
        //{
        //    if (this.textBoxUser.Text != Settings.Default.Role)
        //    {
        //        Settings.Default.Role = this.textBoxUser.Text;
        //    }
        //}

        //private void textBoxPassword_Leave(object sender, EventArgs e)
        //{
        //    if (this._LeaveMainConnectionUnchanged)
        //    {
        //        this.LocalServerConnection.DatabasePassword = this.textBoxPassword.Text;
        //    }
        //    else
        //    {
        //        if (this.textBoxPassword.Text != Settings.Password)
        //        {
        //            this.ChangeAuthentication();
        //            DiversityWorkbench.User.Password = this.textBoxPassword.Text;
        //        }
        //    }
        //}

        //#endregion

        //#region Connection

        //private string ConnectionString
        //{
        //    get
        //    {
        //        string conStr = "";
        //        if (this.comboBoxServer.Text.Length > 0 && this.comboBoxDatabase.Text.Length > 0)
        //        {
        //            conStr = "Data Source=" + this.comboBoxServer.Text;
        //            if (this.textBoxPort.Text.Length > 0) conStr += "," + this.textBoxPort.Text;
        //            conStr += ";initial catalog=";
        //            if (this.comboBoxDatabase.Text.Length > 0) conStr += this.comboBoxDatabase.Text + ";";
        //            else conStr += "master;";
        //            if (this.radioButtonAuthentication.Checked)
        //            {
        //                conStr += "Integrated Security=True";
        //            }
        //            else
        //            {
        //                if (this.textBoxUser.Text.Length > 0 && this.textBoxPassword.Text.Length > 0)
        //                    conStr += "user id=" + this.textBoxUser.Text + ";password=" + this.textBoxPassword.Text;
        //                else conStr = "";
        //            }
        //        }
        //        return conStr;
        //    }
        //}

        //private string ConnectionStringWithoutPassword
        //{
        //    get
        //    {
        //        string conStr = "";
        //        if (this.comboBoxServer.Text.Length > 0 && this.comboBoxDatabase.Text.Length > 0)
        //        {
        //            conStr = "Data Source=" + this.comboBoxServer.Text;
        //            if (this.textBoxPort.Text.Length > 0) conStr += "," + this.textBoxPort.Text;
        //            conStr += ";initial catalog=";
        //            if (this.comboBoxDatabase.Text.Length > 0) conStr += this.comboBoxDatabase.Text + ";";
        //            else conStr += "master;";
        //            if (this.radioButtonAuthentication.Checked)
        //            {
        //                conStr += "Integrated Security=True";
        //            }
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

        //private void buttonConnectToServer_Click(object sender, EventArgs e)
        //{
        //    if (this.buttonConnect.Text == DiversityWorkbench.Forms.FormDatabaseConnectionText.Reset)
        //    {
        //        //this.comboBoxServer.Enabled = true;
        //        //this.textBoxPort.Enabled = true;
        //        this.buttonConnect.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Connect_to_server;
        //        this.buttonConnect.ImageIndex = 2;
        //        this.groupBoxServer.Enabled = true;
        //        this.groupBoxLogin.Enabled = true;
        //        this.comboBoxDatabase.Enabled = false;
        //    }
        //    else
        //    {
        //        this.buttonConnect.Text = DiversityWorkbench.Forms.FormDatabaseConnectionText.Reset;
        //        this.buttonConnect.ImageIndex = 1;
        //        //this.comboBoxServer.Enabled = false;
        //        //this.textBoxPort.Enabled = false;
        //        this.groupBoxLogin.Enabled = false;
        //        this.groupBoxServer.Enabled = false;
        //        if (!this._OnlyLogin)
        //            this.comboBoxDatabase.Enabled = true;
        //        try
        //        {
        //            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //            if (!this._OnlyLogin)
        //                this.comboBoxDatabase.Text = "";
        //            this.userControlDialogPanel.buttonOK.Enabled = false;
        //            if (this._OnlyLogin)
        //            {
        //                if (this.ServerConnection.ConnectionString.Length > 0)
        //                {
        //                    string SQL = "SELECT dbo.BaseURL()";
        //                    string BaseURL = "";
        //                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ServerConnection.ConnectionString);
        //                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //                    try
        //                    {
        //                        con.Open();
        //                        BaseURL = C.ExecuteScalar().ToString();
        //                    }
        //                    catch (System.Exception ex) { }
        //                    finally
        //                    {
        //                        con.Close();
        //                        con.Dispose();
        //                    }

        //                    if (BaseURL.Length > 0)
        //                        this.userControlDialogPanel.buttonOK.Enabled = true;
        //                    else
        //                        this.userControlDialogPanel.buttonOK.Enabled = false;
        //                }
        //            }
        //            else if (this.ConnectionStringMaster.Length > 0)
        //            {
        //                System.Data.DataTable dt = this.DatabaseList;
        //                if (dt.Columns.Count > 0 && dt.Columns[0].ColumnName == "DatabaseName" && dt.Rows.Count > 0)
        //                {
        //                    this.comboBoxDatabase.DataSource = dt;
        //                    this.comboBoxDatabase.DisplayMember = "DatabaseName";
        //                    this.comboBoxDatabase.ValueMember = "DatabaseName";
        //                    this.comboBoxDatabase.Enabled = true;
        //                }
        //                else
        //                {
        //                    this.comboBoxDatabase.Enabled = false;
        //                    if (dt.Rows.Count == 0)
        //                    {
        //                        string Message = "No available databases";
        //                        System.Windows.Forms.MessageBox.Show(Message);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                System.Windows.Forms.MessageBox.Show("invalid login");
        //                this.comboBoxDatabase.Enabled = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "ConnectionString: " + this.ConnectionStringWithoutPassword);
        //            ConnectionOK = false;
        //        }
        //        finally
        //        {
        //            this.Cursor = System.Windows.Forms.Cursors.Default;
        //        }
        //    }
        //}

        //private bool ConnectionOK
        //{
        //    set
        //    {
        //        if (this.comboBoxDatabase.Text.Length > 0)
        //        {
        //            this.userControlDialogPanel.buttonOK.Enabled = value;
        //        }
        //        else
        //        {
        //            this.userControlDialogPanel.buttonOK.Enabled = false;
        //        }
        //    }
        //}

        //private System.Data.DataTable DatabaseList
        //{
        //    get
        //    {
        //        System.Data.DataTable dt = new DataTable();
        //        if (this.ConnectionStringMaster.Length > 0)
        //        {
        //            string SQL = "SELECT name as DatabaseName FROM sys.databases where name not in ( 'master', 'model', 'tempdb', 'msdb')"
        //                + " AND name LIKE '" + this._ModuleName + "%'";
        //            SQL += " ORDER BY name";
        //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringMaster);
        //            try
        //            {
        //                ad.Fill(dt);
        //            }
        //            catch (System.Exception ex)
        //            {
        //                System.Windows.Forms.MessageBox.Show(ex.Message);
        //                return dt;
        //            }
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringMaster);
        //            con.Open();
        //            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("", con);
        //            foreach (System.Data.DataRow R in dt.Rows)
        //            {
        //                string SQL = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Version]') AND " +
        //                    "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
        //                    "BEGIN SELECT dbo.Version() END " +
        //                    "ELSE BEGIN SELECT NULL END";
        //                string SqlModule = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND " +
        //                    "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
        //                    "BEGIN SELECT dbo.DiversityWorkbenchModule() END " +
        //                    "ELSE BEGIN SELECT NULL END";
        //                C.CommandText = SQL;
        //                try
        //                {
        //                    if (this._EmptyDatabase)
        //                    {
        //                        string Test = C.ExecuteScalar().ToString();
        //                        if (Test.Length > 0)
        //                            R.Delete();
        //                        else
        //                        {
        //                            C.CommandText = SqlModule;
        //                            Test = C.ExecuteScalar().ToString();
        //                            if (Test.Length > 0)
        //                                R.Delete();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this._DatabaseVersion = C.ExecuteScalar().ToString();
        //                        if (this._DatabaseVersion.Length == 0)
        //                            R.Delete();
        //                        C.CommandText = SqlModule;
        //                        string Module = C.ExecuteScalar().ToString();
        //                        if (this._ModuleName != C.ExecuteScalar().ToString())
        //                        {
        //                            R.Delete();
        //                        }
        //                    }
        //                }
        //                catch { R.Delete(); }
        //            }
        //            con.Close();
        //        }
        //        return dt;
        //    }
        //}


        //#endregion

        #region Properties

        //public DiversityWorkbench.ServerConnection ServerConnection
        //{
        //    get
        //    {
        //        DiversityWorkbench.ServerConnection S = new ServerConnection();
        //        S.ModuleName = this._ModuleName;
        //        S.DatabaseName = this.comboBoxDatabase.Text;
        //        S.DatabaseServer = this.comboBoxServer.Text;
        //        S.DatabaseServerPort = System.Int32.Parse(this.textBoxPort.Text);
        //        S.DatabaseUser = this.textBoxUser.Text;
        //        S.DatabasePassword = this.textBoxPassword.Text;
        //        S.IsTrustedConnection = this.radioButtonAuthentication.Checked;
        //        try { string s = S.ConnectionString; }
        //        catch { }
        //        return S;
        //    }
        //}

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

        #endregion

        #region Database

        //private void comboBoxDatabase_TextChanged(object sender, EventArgs e)
        //{
        //    this.setDatabase();
        //}

        //private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    this.setDatabase();
        //}

        private void setDatabase()
        {
            //if (this.comboBoxDatabase.SelectedValue == null || this.comboBoxDatabase.SelectedIndex == -1 || this.comboBoxDatabase.Text.Trim().Length == 0)
            //{
            //    this.userControlDialogPanel.buttonOK.Enabled = false;
            //}
            //else
            //    this.userControlDialogPanel.buttonOK.Enabled = true;
        }

        #endregion

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.Connection.Password = this.textBoxPassword.Text;
        }

        private void textBoxRole_TextChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.Settings.Default.Role = this.textBoxRole.Text;
        }

        private void textBoxDatabase_TextChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.Settings.Default.Database = this.textBoxDatabase.Text;
        }

        private void textBoxServer_TextChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.Settings.Default.Server = this.textBoxServer.Text;
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
                    else
                        DiversityWorkbench.PostgreSQL.Settings.Default.Port = Port;
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
        }

        private void textBoxPort_Leave(object sender, EventArgs e)
        {
            bool OK = true;
            int Port;
            if (int.TryParse(this.textBoxPort.Text, out Port))
            {
                if (Port > 65535 || Port < 1024)
                {
                    OK = false;
                }
                else
                    DiversityWorkbench.PostgreSQL.Settings.Default.Port = Port;
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

        private void toolStripButtonRemoveConnection_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.Connection.RemoveConnection(this.textBoxServer.Text, int.Parse(this.textBoxPort.Text), this.textBoxDatabase.Text, this.textBoxRole.Text);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.Settings.Default.Role = this.textBoxRole.Text;
            DiversityWorkbench.PostgreSQL.Settings.Default.Server = this.textBoxServer.Text;
            DiversityWorkbench.PostgreSQL.Settings.Default.Port = int.Parse(this.textBoxPort.Text);
            DiversityWorkbench.PostgreSQL.Settings.Default.Database = this.textBoxDatabase.Text;
            DiversityWorkbench.PostgreSQL.Settings.Default.Save();
            DiversityWorkbench.PostgreSQL.Connection.Password = this.textBoxPassword.Text;

            DiversityWorkbench.PostgreSQL.Connection.ResetDefaultConnectionString();

            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                DiversityWorkbench.PostgreSQL.Connection.AddConnection(this.textBoxServer.Text, int.Parse(this.textBoxPort.Text), this.textBoxDatabase.Text, this.textBoxRole.Text);
                this.buttonConnect.Image = this.imageListConnectionState.Images[0];
            }
            else this.buttonConnect.Image = this.imageListConnectionState.Images[1];
        }

    }
}
