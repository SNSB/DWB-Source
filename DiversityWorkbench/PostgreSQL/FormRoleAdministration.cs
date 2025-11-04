using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace DiversityWorkbench.PostgreSQL
{
    public partial class FormRoleAdministration : Form
    {

        #region Construction and form

        public FormRoleAdministration()
        {
            InitializeComponent();
            this.initLogins();
            this.initGroups();
        }
        
        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
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

        #region Logins

        private void initLogins()
        {
            try
            {
                this.listBoxRoles.Items.Clear();
                DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Role> KV in DiversityWorkbench.PostgreSQL.Connection.Logins())
                {
                    this.listBoxRoles.Items.Add(KV.Key);
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void toolStripButtonRoleAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New login", "Please enter the name of the new login", "");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.AddLogin(f.String))
                    {
                        DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
                        this.initLogins();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButtonRoleDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string Role = this.listBoxRoles.SelectedItem.ToString();
                if(DiversityWorkbench.PostgreSQL.Connection.DeleteRole(Role))
                {
                    DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
                    this.initLogins();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private DiversityWorkbench.PostgreSQL.Role CurrentRole()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.Roles().ContainsKey(this._CurrentGroupOrLogin))// this.CurrentRoleName()))
                return DiversityWorkbench.PostgreSQL.Connection.Roles()[this._CurrentGroupOrLogin];// this.CurrentRoleName()];
            else
                return null;
        }

        private void listBoxRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                if (this.listBoxRoles.SelectedIndex > -1)
                {
                    this._CurrentGroupOrLogin = this.listBoxRoles.SelectedItem.ToString();
                    this.pictureBoxDetailsHeader.Image = this.pictureBoxLogins.Image;
                    this.labelDetailsHeader.Text = this._CurrentGroupOrLogin;
                    this.tabControlRole.Enabled = true;
                    this.initRoleProperties(true);
                    this.initMembershipsLists(this.CurrentRole());
                    // seems to be needed
                    this.initPermissions();
                    this.listBoxGroups.SelectedIndex = -1;
                }
            }
            catch(System.Exception ex)
            { }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        #endregion

        #region Groups

        private void initGroups()
        {
            try
            {
                this.listBoxGroups.Items.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Role> KV in DiversityWorkbench.PostgreSQL.Connection.Groups())
                {
                    this.listBoxGroups.Items.Add(KV.Key);
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void listBoxGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                if (this.listBoxGroups.SelectedIndex > -1)
                {
                    this._CurrentGroupOrLogin = this.listBoxGroups.SelectedItem.ToString();
                    this.pictureBoxDetailsHeader.Image = this.pictureBoxGroups.Image;
                    this.labelDetailsHeader.Text = this._CurrentGroupOrLogin;
                    this.tabControlRole.Enabled = true;
                    this.tableLayoutPanelGrants.Visible = false;
                    this.initPermissions();
                    this.initRoleProperties(false);
                    this.initMembershipsLists(this.CurrentGroup());
                    this.listBoxRoles.SelectedIndex = -1;
                }
            }
            catch (System.Exception ex)
            { }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private string CurrentGroupName()
        {
            string Group = "";
            try
            {
                if (this.listBoxGroups.SelectedItem != null)
                    Group = this.listBoxGroups.SelectedItem.ToString();
            }
            catch (System.Exception ex)
            { }
            return Group;
        }

        private DiversityWorkbench.PostgreSQL.Role CurrentGroup()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.Roles().ContainsKey(this.CurrentGroupName()))
                return DiversityWorkbench.PostgreSQL.Connection.Roles()[this.CurrentGroupName()];
            else
                return null;
        }
        
        #endregion

        #region Role Properties

        private void initRoleProperties(bool IsRole)
        {
            this.labelRoleProperties.Text = this._CurrentGroupOrLogin;// .CurrentRoleName();
            this.checkBoxCreateDB.Checked = this.CurrentRole().CanCreateDatabase;
            this.checkBoxCreateRoles.Checked = this.CurrentRole().CanCreateRoles;
            this.checkBoxLogin.Checked = this.CurrentRole().CanLogin;
            this.checkBoxSuperuser.Checked = this.CurrentRole().IsSuperuser;
            this.checkBoxInherit.Checked = this.CurrentRole().Inherit;

            this.checkBoxLogin.Visible = IsRole;
            this.checkBoxSuperuser.Visible = IsRole;
            this.checkBoxCreateRoles.Visible = IsRole;
            this.checkBoxCreateDB.Visible = IsRole;

        }
        
        private void textBoxRoleDescription_Leave(object sender, EventArgs e)
        {
            string SQL = "COMMENT ON ROLE \"" + this._CurrentGroupOrLogin + "\" " + // .CurrentRoleName() + "\" " +
            "IS '" + this.textBoxRoleDescription.Text + "';";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);
        }

        private void checkBoxLogin_Click(object sender, EventArgs e)
        {
            string SQL = "ALTER ROLE \"" + this._CurrentGroupOrLogin + "\" "; // +this.CurrentRoleName() + "\" ";
            if (checkBoxCreateDB.Checked)
                SQL += "  LOGIN;";
            else
                SQL += "  NOLOGIN;";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL); 
        }

        private void checkBoxSuperuser_Click(object sender, EventArgs e)
        {
            string SQL = "ALTER ROLE \"" + this._CurrentGroupOrLogin + "\" ";// +this.CurrentRoleName() + "\" ";
            if (checkBoxCreateDB.Checked)
                SQL += "  SUPERUSER;";
            else
                SQL += "  NOSUPERUSER;";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL); 
        }

        private void checkBoxCreateRoles_Click(object sender, EventArgs e)
        {
            string SQL = "ALTER ROLE \"" + this._CurrentGroupOrLogin + "\" ";// +this.CurrentRoleName() + "\" ";
            if (checkBoxCreateRoles.Checked)
                SQL += "  CREATEROLE;";
            else
                SQL += "  NOCREATEROLE;";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
            DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
        }

        private void checkBoxCreateDB_Click(object sender, EventArgs e)
        {
            string SQL = "ALTER ROLE \"" + this._CurrentGroupOrLogin + "\" ";// +this.CurrentRoleName() + "\" ";
            if (checkBoxCreateDB.Checked)
                SQL += "  CREATEDB;";
            else
                SQL += "  NOCREATEDB;";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
            DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
        }
        
        private void checkBoxInherit_Click(object sender, EventArgs e)
        {
            string SQL = "ALTER ROLE \"" + this._CurrentGroupOrLogin + "\" ";// +this.CurrentRoleName() + "\" ";
            if (checkBoxCreateDB.Checked)
                SQL += "  INHERIT;";
            else
                SQL += "  NOINHERIT;";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
            DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
        }

        #endregion

        #region Membership

        private void initMembershipsLists(DiversityWorkbench.PostgreSQL.Role Role)
        {
            try
            {
                this.listBoxMemberInRoles.Items.Clear();
                this.listBoxAvailableRoles.Items.Clear();
                if (Role != null)
                {
                    bool IsGroup = false;
                    if (DiversityWorkbench.PostgreSQL.Connection.Groups().ContainsKey(Role.Name))
                        IsGroup = true;
                    this.labelMembership.Text = Role.Name;// +" membership";
                    if (IsGroup)
                    {
                        this.pictureBoxMembership.Image = this.pictureBoxGroups.Image;
                    }
                    else
                    {
                        this.pictureBoxMembership.Image = this.pictureBoxLogins.Image;
                    }
                    foreach (string M in DiversityWorkbench.PostgreSQL.Connection.Roles()[Role.Name].MemberShipList())
                    {
                        this.listBoxMemberInRoles.Items.Add(M);
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Role> KV in DiversityWorkbench.PostgreSQL.Connection.Groups())
                    {

                        if (Role != null && !DiversityWorkbench.PostgreSQL.Connection.Roles()[Role.Name].MemberShipList().Contains(KV.Key)
                            && KV.Key != Role.Name)
                            this.listBoxAvailableRoles.Items.Add(KV.Key);
                    }
                }
            }
            catch(System.Exception ex)
            {

            }
        }

        private string _CurrentGroupOrLogin = "";

        private void buttonAddToRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxAvailableRoles.SelectedItem == null) // .SelectedValue == null)
                {
                    System.Windows.Forms.MessageBox.Show("No role selected");
                    return;
                }
                string GrantedRole = this.listBoxAvailableRoles.SelectedItem.ToString();// this.listBoxAvailableRoles.SelectedValue.ToString();
                if (DiversityWorkbench.PostgreSQL.Connection.Roles()[this._CurrentGroupOrLogin].GrantMembership(GrantedRole, this.checkBoxGrantWithAdminOption.Checked))
                {
                    this.initMembershipsLists(this.CurrentRole());
                    this.initPermissions();
                }
                else
                    System.Windows.Forms.MessageBox.Show("Granting the membership failed");
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonRemoveFromRole_Click(object sender, EventArgs e)
        {
            if (this.listBoxMemberInRoles.SelectedItem == null) //   .SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show("No role selected");
                return;
            }
            string RevokedRole = this.listBoxMemberInRoles.SelectedItem.ToString();// this.listBoxMemberInRoles.SelectedValue.ToString();
            if (DiversityWorkbench.PostgreSQL.Connection.Roles()[this._CurrentGroupOrLogin].RevokeMembership(RevokedRole))
            {
                this.initMembershipsLists(this.CurrentRole());
                this.initPermissions();
            }
            else
                System.Windows.Forms.MessageBox.Show("Revoking the membership failed");
        }

        private void listBoxMemberInRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxMemberInRoles.SelectedItem != null)
            {
                string SeletedRole = this.listBoxMemberInRoles.SelectedItem.ToString();
                string SQL = "select m.admin_option " +
                    "from pg_auth_members m, pg_roles r, pg_roles g  " +
                    "where m.member = r.oid and m.roleid = g.oid and r.rolname = '" + this._CurrentGroupOrLogin + "' " +
                    "and g.rolname = '" + SeletedRole + "'";
                bool WithAdminOption = false;
                bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out WithAdminOption);
                this.checkBoxAdminOption.Checked = WithAdminOption;
            }
        }

        private void checkBoxAdminOption_Click(object sender, EventArgs e)
        {
            if (this.listBoxMemberInRoles.SelectedItem != null)
            {
                string SeletedRole = this.listBoxMemberInRoles.SelectedItem.ToString();
                string SQL = "";
                if (this.checkBoxAdminOption.Checked)
                {
                    SQL = "GRANT " + SeletedRole + " TO " + this._CurrentGroupOrLogin + " WITH ADMIN OPTION ";
                }
                else
                    SQL = "REVOKE ADMIN OPTION FOR " + SeletedRole + " FROM " + this._CurrentGroupOrLogin;
                string Message = "";
                bool OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL,ref Message);
            }
        }

        #endregion

        #region Groups

        private void toolStripButtonAddGroup_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New group", "Please enter the name of the new group", "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            {
                DiversityWorkbench.PostgreSQL.Connection.AddGroup(f.String);
                DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
                this.initGroups();
                this.initMembershipsLists(this.CurrentGroup());
            }
        }

        private void toolStripButtonDeleteGroup_Click(object sender, EventArgs e)
        {
            try
            {
                string Role = this.listBoxGroups.SelectedItem.ToString();
                if (DiversityWorkbench.PostgreSQL.Connection.DeleteRole(Role))
                {
                    DiversityWorkbench.PostgreSQL.Connection.ResetRoles();
                    this.initGroups();
                    this.initMembershipsLists(this.CurrentGroup());
                }
            }
            catch (Exception ex)
            {
            }

        }
        
        #endregion

        #region Permissions

        private enum DatabaseObjects { Database, Schema, Table, View, Function }

        private string _CurrentSchema = "";

        #region Building the hierarchy

        private string _HierarchyConnectionString;

        private void initPermissions()
        {
            this.labelPermissions.Text = this.CurrentGroupName();
            this.treeViewPermissions.Nodes.Clear();

            // Getting the databases
            //string SQL = "SELECT datname FROM pg_database " +
            //    "WHERE datname not like 'template%' " +
            //    "AND datname <> 'postgres' " +
            //    "ORDER BY datname";


            string SQL = "SELECT datname FROM pg_database " +
                "WHERE datname = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' ";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                string Role = this.CurrentGroupName();
                if (Role.Length == 0)
                    Role = this._CurrentGroupOrLogin;
                SQL = "SELECT * FROM has_database_privilege('" + Role + "', '" + R[0].ToString() + "', 'connect');";
                string Privilege = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
                bool connect = false;
                bool.TryParse(Privilege, out connect);
                System.Windows.Forms.TreeNode Ndb = new TreeNode(R[0].ToString());
                Ndb.ImageIndex = 0;
                Ndb.SelectedImageIndex = 0;
                Ndb.Tag = DatabaseObjects.Database;
                if (connect) Ndb.ForeColor = System.Drawing.Color.Green;
                else Ndb.ForeColor = System.Drawing.Color.Red;
                this.treeViewPermissions.Nodes.Add(Ndb);
                this._HierarchyConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(R[0].ToString());// .Postgres.PostgresDatabaseConnectionString(R[0].ToString());
                this.AddSchemaPermissions(Ndb, this._HierarchyConnectionString);
            }
            this.treeViewPermissions.ExpandAll();
            /*
             * SELECT has_table_privilege('joe', 'mytable', 'INSERT, SELECT WITH GRANT OPTION');
             * has_database_privilege(user, database, privilege)
             * has_function_privilege(user, function, privilege)
             * SELECT has_function_privilege('joeuser', 'myfunc(int, text)', 'execute');
             * has_schema_privilege(user, schema, privilege)
             * */
        }

        private void AddSchemaPermissions(System.Windows.Forms.TreeNode N, string ConnectionString)
        {
            try
            {
                string SQL = "select schema_name from information_schema.schemata " +
                    "where schema_name not LIKE 'pg_toast%' AND schema_name not LIKE 'pg_temp%' and schema_name <> 'information_schema' and schema_name <> 'pg_catalog';";
                //Npgsql.NpgsqlDataAdapter adSchema = new NpgsqlDataAdapter(SQL, ConnectionString);
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                //adSchema.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Role = this.CurrentGroupName();
                    if (Role.Length == 0)
                        Role = this._CurrentGroupOrLogin;
                    SQL = "SELECT has_schema_privilege('" + Role + "', '" + R[0].ToString() + "', 'Usage')";
                    string Privilege = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
                    bool Usage = false;
                    bool.TryParse(Privilege, out Usage);
                    System.Windows.Forms.TreeNode Nschema = new TreeNode(R[0].ToString());
                    Nschema.ImageIndex = 1;
                    Nschema.SelectedImageIndex = 1;
                    Nschema.Tag = DatabaseObjects.Schema;
                    if (Usage) Nschema.ForeColor = System.Drawing.Color.Green;
                    else Nschema.ForeColor = System.Drawing.Color.Red;
                    N.Nodes.Add(Nschema);
                    this._CurrentSchema = R[0].ToString();

                    //this.AddTablePermissions(R[0].ToString(), Nschema, ConnectionString);
                    //this.AddViewPermissions(R[0].ToString(), Nschema, ConnectionString);
                    //this.AddFunctionPermissions(R[0].ToString(), Nschema, ConnectionString);
                }
                //adSchema.Dispose();
            }
            catch (System.Exception ex)
            { }
        }

        private void AddTablePermissions(string Schema, System.Windows.Forms.TreeNode N, string ConnectionString)
        {
            try
            {
                this._CurrentSchema = Schema;
                string SQL = "select table_name from information_schema.tables P " +
                   "where P.table_type = 'BASE TABLE' " +
                   "and P.table_schema = '" + Schema + "';";
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Role = this.CurrentGroupName();
                    if (Role.Length == 0)
                        Role = this._CurrentGroupOrLogin;
                    SQL = "SELECT has_table_privilege('" + Role + "', '\"" + Schema + "\".\"" + R[0].ToString() + "\"', 'SELECT');";
                    string SelectPrivilege = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
                    bool Select = false;
                    bool.TryParse(SelectPrivilege, out Select);
                    System.Windows.Forms.TreeNode Ntable = new TreeNode(R[0].ToString());
                    Ntable.ImageIndex = 2;
                    Ntable.SelectedImageIndex = 2;
                    Ntable.Tag = DatabaseObjects.Table;
                    if (Select) Ntable.ForeColor = System.Drawing.Color.Green;
                    else Ntable.ForeColor = System.Drawing.Color.Red;
                    N.Nodes.Add(Ntable);
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void AddViewPermissions(string Schema, System.Windows.Forms.TreeNode N, string ConnectionString)
        {
            try
            {
                this._CurrentSchema = Schema;
                string SQL = "SELECT viewname FROM pg_catalog.pg_views where schemaname = '" + Schema + "';";
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Role = this.CurrentGroupName();
                    if (Role.Length == 0)
                        Role = this._CurrentGroupOrLogin;
                    SQL = "SELECT has_table_privilege('" + Role + "', '\"" + Schema + "\".\"" + R[0].ToString() + "\"', 'SELECT');";
                    string Privilege = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
                    bool Select = false;
                    bool.TryParse(Privilege, out Select);
                    System.Windows.Forms.TreeNode Nview = new TreeNode(R[0].ToString());
                    Nview.ImageIndex = 3;
                    Nview.SelectedImageIndex = 3;
                    Nview.Tag = DatabaseObjects.View;
                    if (Select) Nview.ForeColor = System.Drawing.Color.Green;
                    else Nview.ForeColor = System.Drawing.Color.Red;
                    N.Nodes.Add(Nview);
                }
                //adSchema.Dispose();
            }
            catch (System.Exception ex)
            { }
        }

        private void AddFunctionPermissions(string Schema, System.Windows.Forms.TreeNode N, string ConnectionString)
        {
            try
            {
                this._CurrentSchema = Schema;
                string SQL = "SELECT * FROM information_schema.routines where routine_schema = '" + Schema + "';";
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                //Npgsql.NpgsqlDataAdapter adSchema = new NpgsqlDataAdapter(SQL, ConnectionString);
                //adSchema.Fill(dt);
                //adSchema.Dispose();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Role = this.CurrentGroupName();
                    if (Role.Length == 0)
                        Role = this._CurrentGroupOrLogin;
                    SQL = "SELECT has_function_privilege('" + Role + "', '\"" + Schema + "\"." + R["routine_name"].ToString() + "()', 'execute');";
                    string Privilege = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
                    bool execute = false;
                    bool.TryParse(Privilege, out execute);
                    System.Windows.Forms.TreeNode Nfunction = new TreeNode(R["routine_name"].ToString() + "()");
                    Nfunction.ImageIndex = 4;
                    Nfunction.SelectedImageIndex = 4;
                    Nfunction.Tag = DatabaseObjects.Function;
                    if (execute) Nfunction.ForeColor = System.Drawing.Color.Green;
                    else Nfunction.ForeColor = System.Drawing.Color.Red;
                    N.Nodes.Add(Nfunction);
                }
            }
            catch (System.Exception ex)
            { }
        }
        
        #endregion

        private void treeViewPermissions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            System.Windows.Forms.TreeNode N = this.treeViewPermissions.SelectedNode;
            try
            {
                if (N.Nodes.Count == 0)
                {
                    if (N.Tag.ToString() == DatabaseObjects.Schema.ToString())
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        this.AddTablePermissions(N.Text, N, this._HierarchyConnectionString);
                        this.AddViewPermissions(N.Text, N, this._HierarchyConnectionString);
                        this.AddFunctionPermissions(N.Text, N, this._HierarchyConnectionString);
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                    }
                }
            }
            catch { }
            this.displayGrants(this.treeViewPermissions.SelectedNode);
            this.pictureBoxGrants.Image = this.treeViewPermissions.ImageList.Images[this.treeViewPermissions.SelectedNode.ImageIndex];
            this.tableLayoutPanelGrants.Visible = true;
            this.setCurrentSchema();
        }

        private void setCurrentSchema()
        {
            System.Windows.Forms.TreeNode N = this.treeViewPermissions.SelectedNode;
            DatabaseObjects DBobjet = (DatabaseObjects)N.Tag;
            if (DBobjet == DatabaseObjects.Database)
                return;
            else
            {
                 if(DBobjet == DatabaseObjects.Schema)
                 {
                     this._CurrentSchema = N.Text;
                 }
                 else
                 {
                     this._CurrentSchema = N.Parent.Text;
                 }
            }
        }

        private bool displayGrants(System.Windows.Forms.TreeNode Node)
        {
            bool OK = true;
            this.checkBoxGrantInsert.Visible = false;
            this.checkBoxGrantDelete.Visible = false;
            this.checkBoxGrantSelect.Visible = false;
            this.checkBoxGrantUpdate.Visible = false;
            this.checkBoxGrantExecute.Visible = false;
            this.checkBoxGrantConnect.Visible = false;
            this.checkBoxGrantUsage.Visible = false;

            this.checkBoxGrantWithGrantOption.Visible = false;
            this.checkBoxGrantAll.Visible = false;
            this.checkBoxGrantAll.Checked = false;
            this.checkBoxGrantAllTables.Visible = false;
            this.checkBoxGrantAllTables.Checked = false;

            DatabaseObjects DBobjet = (DatabaseObjects)Node.Tag;

            this.labelGrants.Text = "Grants for ";
            // enabling
            switch (DBobjet)
            {
                case DatabaseObjects.Function:
                    this.labelGrants.Text += "function " + Node.Text;
                    this.checkBoxGrantExecute.Visible = true;
                    this.checkBoxGrantAll.Visible = true;
                    this.checkBoxGrantWithGrantOption.Visible = true;
                    break;
                case DatabaseObjects.Table:
                    this.labelGrants.Text += "table " + Node.Text;
                    this.checkBoxGrantAll.Visible = true;
                    this.checkBoxGrantWithGrantOption.Visible = true;
                    goto case DatabaseObjects.View;
                case DatabaseObjects.View:
                    this.labelGrants.Text += "view " + Node.Text;
                    this.checkBoxGrantInsert.Visible = true;
                    this.checkBoxGrantDelete.Visible = true;
                    this.checkBoxGrantSelect.Visible = true;
                    this.checkBoxGrantUpdate.Visible = true;
                    this.checkBoxGrantAll.Visible = true;
                    this.checkBoxGrantWithGrantOption.Visible = true;
                    break;
                case DatabaseObjects.Schema:
                    this.labelGrants.Text += "schema " + Node.Text;
                    this.checkBoxGrantUsage.Visible = true;
                    this.checkBoxGrantAll.Visible = true;
                    this.checkBoxGrantAllTables.Visible = true;
                    this.checkBoxGrantWithGrantOption.Visible = true;
                    break;
                case DatabaseObjects.Database:
                    this.labelGrants.Text += "database " + Node.Text;
                    this.checkBoxGrantConnect.Visible = true;
                    this.checkBoxGrantAll.Visible = true;
                    this.checkBoxGrantWithGrantOption.Visible = true;
                    break;
            }

            // setting grants
            string SQL = "";
            bool HasPrivilege = false;
            string Role = this.CurrentGroupName();
            if (Role.Length == 0)
                Role = this._CurrentGroupOrLogin;

            switch(DBobjet)
            {
                case DatabaseObjects.Function:
                    SQL = "SELECT has_function_privilege('" + Role + "', '\"" + this._CurrentSchema + "\"." + Node.Text + "', 'execute');";
                    if (bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out HasPrivilege))
                        this.checkBoxGrantExecute.Checked = HasPrivilege;
                    break;
                case DatabaseObjects.Table:
                case DatabaseObjects.View:
                    SQL = "SELECT has_table_privilege('" + Role + "', '\"" + this._CurrentSchema + "\".\"" + Node.Text + "\"', 'select');";
                    if (bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out HasPrivilege))
                        this.checkBoxGrantSelect.Checked = HasPrivilege;
                    SQL = "SELECT has_table_privilege('" + Role + "', '\"" + this._CurrentSchema + "\".\"" + Node.Text + "\"', 'insert');";
                    if (bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out HasPrivilege))
                        this.checkBoxGrantInsert.Checked = HasPrivilege;
                    SQL = "SELECT has_table_privilege('" + Role + "', '\"" + this._CurrentSchema + "\".\"" + Node.Text + "\"', 'update');";
                    if (bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out HasPrivilege))
                        this.checkBoxGrantUpdate.Checked = HasPrivilege;
                    SQL = "SELECT has_table_privilege('" + Role + "', '\"" + this._CurrentSchema + "\".\"" + Node.Text + "\"', 'delete');";
                    if (bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out HasPrivilege))
                        this.checkBoxGrantDelete.Checked = HasPrivilege;
                    break;
                case DatabaseObjects.Schema:
                    SQL = "SELECT has_schema_privilege('" + Role + "', '" + Node.Text + "', 'Usage')";
                    if (bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out HasPrivilege))
                        this.checkBoxGrantUsage.Checked = HasPrivilege;
                    break;
                case DatabaseObjects.Database:
                    SQL = "SELECT * FROM has_database_privilege('" + Role + "', '" + Node.Text + "', 'connect');";
                    if (bool.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out HasPrivilege))
                        this.checkBoxGrantConnect.Checked = HasPrivilege;
                    break;
            }
            return OK;
        }

        #region Setting the grants
        
        private void checkBoxGrantSelect_Click(object sender, EventArgs e)
        {
            if (this.SetGrant("Select", this.checkBoxGrantSelect.Checked))
            {
                //this.checkBoxGrantSelect.Checked = this.checkBoxGrantSelect.Checked;
            }
        }

        private void checkBoxGrantInsert_Click(object sender, EventArgs e)
        {
            if (!this.SetGrant("Insert", this.checkBoxGrantInsert.Checked))
                this.checkBoxGrantInsert.Checked = !this.checkBoxGrantInsert.Checked;
        }

        private void checkBoxGrantUpdate_Click(object sender, EventArgs e)
        {
            if (!this.SetGrant("Update", this.checkBoxGrantUpdate.Checked))
                this.checkBoxGrantUpdate.Checked = !this.checkBoxGrantUpdate.Checked;
        }

        private void checkBoxGrantDelete_Click(object sender, EventArgs e)
        {
            if (!this.SetGrant("Delete", this.checkBoxGrantDelete.Checked))
                this.checkBoxGrantDelete.Checked = !this.checkBoxGrantDelete.Checked;
        }

        private void checkBoxGrantExecute_Click(object sender, EventArgs e)
        {
            if (!this.SetGrant("Execute", this.checkBoxGrantExecute.Checked))
            {
                //this.checkBoxGrantExecute.Checked = this.checkBoxGrantExecute.Checked;
            }
        }

        private void checkBoxGrantConnect_Click(object sender, EventArgs e)
        {
            if (!this.SetGrant("Connect", this.checkBoxGrantConnect.Checked))
            {
                //this.checkBoxGrantConnect.Checked = this.checkBoxGrantConnect.Checked;
            }
        }

        private void checkBoxGrantUsage_Click(object sender, EventArgs e)
        {
            if (!this.SetGrant("Usage", this.checkBoxGrantUsage.Checked))
            {
                //this.checkBoxGrantSelect.Checked = this.checkBoxGrantSelect.Checked;
            }
        }

        private void checkBoxGrantAll_Click(object sender, EventArgs e)
        {
            this.SetGrant("ALL", this.checkBoxGrantAll.Checked);
        }

        private void checkBoxGrantAllTables_Click(object sender, EventArgs e)
        {
            string SQL = "GRANT ALL ON ALL TABLES IN SCHEMA \"" + this._CurrentSchema + "\" TO \"" + this._CurrentGroupOrLogin + "\";";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
        }

        private bool SetGrant(string Grant, bool Granted)
        {
            bool OK = true;
            try
            {
                if (this.treeViewPermissions.SelectedNode == null)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    return false;
                }
                System.Windows.Forms.TreeNode N = this.treeViewPermissions.SelectedNode;
                DatabaseObjects DBobjet = (DatabaseObjects)N.Tag;
                string SQL = "";
                if (Granted)
                    SQL = "GRANT ";
                else
                    SQL = "REVOKE ";
                SQL += Grant;
                switch (DBobjet)
                {
                    case DatabaseObjects.Function:
                        SQL += " ON FUNCTION \"" + this._CurrentSchema + "\".";
                        break;
                    case DatabaseObjects.Table:
                    case DatabaseObjects.View:
                        SQL += " ON \"" + this._CurrentSchema + "\".";
                        break;
                    case DatabaseObjects.Schema:
                        SQL += " ON SCHEMA ";
                        break;
                    case DatabaseObjects.Database:
                        SQL += " ON DATABASE ";
                        break;
                }
                if (DBobjet == DatabaseObjects.Function)
                    SQL += N.Text;
                else
                    SQL += "\"" + N.Text + "\"";
                if (Granted)
                    SQL += " TO ";
                else
                    SQL += " FROM ";
                SQL += "\"" + this._CurrentGroupOrLogin + "\"";
                //SQL += "\"" + this.listBoxGroups.SelectedItem.ToString() + "\"";
                if (this.checkBoxGrantWithGrantOption.Checked)
                    SQL += " WITH GRANT OPTION";
                SQL += ";";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                if (OK)
                {
                    switch (DBobjet)
                    {
                        case DatabaseObjects.Function:
                        case DatabaseObjects.Schema:
                        case DatabaseObjects.Database:
                            if (Granted)
                                N.ForeColor = System.Drawing.Color.Green;
                            else
                                N.ForeColor = System.Drawing.Color.Red;
                            break;
                        case DatabaseObjects.Table:
                        case DatabaseObjects.View:
                            if (Grant.ToLower() == "select")
                            {
                                if (Granted)
                                    N.ForeColor = System.Drawing.Color.Green;
                                else
                                    N.ForeColor = System.Drawing.Color.Red;
                            }
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }
        
        #endregion

        #endregion

        #region Activity

        private void buttonShowActivity_Click(object sender, EventArgs e)
        {
            this.ShowActivity();
        }
        
        private void buttonActivityStop_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT pg_terminate_backend(pid) " +
                "FROM pg_stat_activity " +
                "WHERE pid <> pg_backend_pid() " +
                "AND datname = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "';";
            this.ShowActivity();
        }

        private void ShowActivity()
        {
            string SQL = "select * from pg_stat_activity;";
            System.Data.DataTable dtAcitvity = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtAcitvity, ref Message);
            this.dataGridViewActivity.DataSource = dtAcitvity;
        }

        #endregion

    }
}
