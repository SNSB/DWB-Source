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
    public partial class FormDatabaseRoles : Form
    {

        #region Parameter

        private string _Role;
        private System.Data.DataTable _DtRoles;
        private string _ConnectionString;
        
        #endregion

        #region Construction

        /// <summary>
        /// Showing the permissions of a database role and the relations to other roles
        /// </summary>
        /// <param name="Role">The name of the database role</param>
        public FormDatabaseRoles(string Role, string ConnectionString)
        {
            InitializeComponent();
            this._Role = Role;
            this._ConnectionString = ConnectionString;
            this.initForm();
        }
        
        #endregion

        #region From
        
        private void initForm()
        {
            bool IsSysAdmin = false;
            if (DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                IsSysAdmin = true;
            this.toolStripContainedRoles.Enabled = IsSysAdmin;
            this.toolStripInRoles.Enabled = IsSysAdmin;
            this.toolStripPermissions.Enabled = IsSysAdmin;

            this.Text = "Database role " + this._Role;
            this.labelHeader.Text = "Overview for the database role " + this._Role;
            this.labelContainedInRoles.Text = "Roles that inherit permission of " + this._Role + ":";
            this.labelContainedRoles.Text = this._Role + " is inheriting permissions of:";

            // the permissions
            string SQL = "select sys.schemas.name as [Schema], sys.objects.name AS [Object in database], sys.database_permissions.permission_name AS Permission, sys.database_permissions.state_desc as State " +
                ", sys.objects.type_desc AS [Type of object], sys.objects.create_date AS [Created], sys.objects.modify_date AS [Modified] " +
                "from sys.database_permissions , sys.database_principals , sys.objects, sys.schemas " +
                "where sys.database_principals.principal_id = sys.database_permissions.grantee_principal_id " +
                "and sys.database_permissions.major_id = sys.objects.object_id " +
                "and sys.database_principals.name = '" + _Role + "' and sys.schemas.schema_id = sys.objects.schema_id " +
                "order by sys.objects.name, sys.database_permissions.permission_name, sys.database_permissions.state_desc";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            this._DtRoles = new DataTable();
            try
            {
                ad.Fill(this._DtRoles);
                this.dataGridViewPermissions.DataSource = this._DtRoles;
                this.dataGridViewPermissions.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            // the contained roles
            ad.SelectCommand.CommandText = "select P.name AS ContainedRole " +
                "from sys.database_principals P, sys.database_role_members R, sys.database_principals M " +
                "where P.type = 'R' " +
                "and M.type = 'R' " +
                "and P.is_fixed_role = 0 " +
                "and P.principal_id = R.role_principal_id " +
                "and M.principal_id = R.member_principal_id " +
                "and M.Name = '" + _Role + "'";
            System.Data.DataTable dtContainedRoles = new DataTable();
            try
            {
                ad.Fill(dtContainedRoles);
                this.listBoxContainedRoles.DataSource = dtContainedRoles;
                this.listBoxContainedRoles.DisplayMember = "ContainedRole";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            // the roles where the role is contained
            ad.SelectCommand.CommandText = "select M.name AS SuperiorRole " +
                "from sys.database_principals P, sys.database_role_members R, sys.database_principals M " +
                "where P.type = 'R' " +
                "and M.type = 'R' " +
                "and P.is_fixed_role = 0 " +
                "and P.principal_id = R.role_principal_id " +
                "and M.principal_id = R.member_principal_id " +
                "and P.Name = '" + _Role + "'";
            System.Data.DataTable dtInRole = new DataTable();
            try
            {
                ad.Fill(dtInRole);
                this.listBoxContainedInRoles.DataSource = dtInRole;
                this.listBoxContainedInRoles.DisplayMember = "SuperiorRole";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Administration

        #region Permissions

        private void toolStripButtonPermissionsAdd_Click(object sender, EventArgs e)
        {
            string Schema = "dbo";
            string Type = "Table";
            string Object = "";
            string Permission = "SELECT";
            System.Data.DataTable dtSchemata = new DataTable();
            string SQL = "SELECT schema_name " +
                "FROM information_schema.schemata s " +
                "where s.SCHEMA_NAME not like 'db[_]%' " +
                "and s.SCHEMA_NAME <> 'sys' " +
                "and s.SCHEMA_NAME <> 'INFORMATION_SCHEMA' " +
                "and s.SCHEMA_NAME <> 'guest'";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            ad.Fill(dtSchemata);
            if (dtSchemata.Rows.Count > 0)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtSchemata, "Please select the schema");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else Schema = f.String;
            }
            System.Collections.Generic.List<string> Types = new List<string>();
            Types.Add("Table");
            Types.Add("View");
            //Types.Add("Procedure");
            //Types.Add("Function");
            DiversityWorkbench.Forms.FormGetStringFromList fType = new DiversityWorkbench.Forms.FormGetStringFromList(Types, "Type", "Please select the type", true);
            fType.ShowDialog();
            if (fType.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return;
            else Type = fType.String;
            switch (Type)
            {
                case "Table":
                    SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T " +
                        "where t.TABLE_TYPE = 'BASE TABLE'";
                    break;
                case "View":
                    SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T " +
                        "where t.TABLE_TYPE = 'VIEW'";
                    break;
            }
            System.Data.DataTable dtObject = new DataTable();
            ad.SelectCommand.CommandText = SQL;
            ad.Fill(dtObject);
            DiversityWorkbench.Forms.FormGetStringFromList fObject = new DiversityWorkbench.Forms.FormGetStringFromList(dtObject, "Please select the " + Type.ToLower());
            fObject.ShowDialog();
            if (fObject.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return;
            else Object = fObject.String;
            System.Collections.Generic.List<string> Permissions = new List<string>();
            switch (Type)
            {
                case "Table":
                case "View":
                    Permissions.Add("SELECT");
                    Permissions.Add("UPDATE");
                    Permissions.Add("INSERT");
                    Permissions.Add("DELETE");
                    break;
            }
            DiversityWorkbench.Forms.FormGetStringFromList fPermission = new DiversityWorkbench.Forms.FormGetStringFromList(Permissions, "Permission", "Please select the permission", true);
            fPermission.ShowDialog();
            if (fPermission.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return;
            else Permission = fPermission.String;
            SQL = "GRANT " + Permission + " ON " + Schema + ".[" + Object + "] TO [" + this._Role + "];";
            if (this.SqlExecuteNonQuery(SQL))
            {
                this.initForm();
                this.SetSqlForSkript(SQL);
            }
            else System.Windows.Forms.MessageBox.Show("Granting permission failed");
        }

        private void toolStripButtonPermissionsRemove_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = this._DtRoles.Rows[this.dataGridViewPermissions.SelectedCells[0].RowIndex];
            string SQL = "REVOKE " + R["Permission"].ToString() + " ON " + R["Schema"].ToString() + ".[" + R[1].ToString() + "] FROM [" + this._Role + "];";
            if (this.SqlExecuteNonQuery(SQL))
            {
                this.initForm();
                this.SetSqlForSkript(SQL);
            }
            else System.Windows.Forms.MessageBox.Show("Revoke failed");
        }
        
        #endregion

        #region Roles

        #region Roles inheriting permissions of
        
        private void toolStripButtonContainedRolesAdd_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT name FROM sysusers WHERE (uid = gid) AND (name <> N'public') AND (name NOT LIKE 'db_%') and Name <> '" + _Role + "'";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            ad.Fill(dt);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Please select the role that should be added");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                SQL = "EXEC sp_addrolemember N'" + f.String + "', N'" + this._Role + "';";
                if (this.SqlExecuteNonQuery(SQL))
                {
                    this.initForm();
                    this.SetSqlForSkript(SQL);
                }
                else System.Windows.Forms.MessageBox.Show("Adding a new role failed");
            }
        }

        private void toolStripButtonContainedRolesRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxContainedRoles.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxContainedRoles.SelectedItem;
                string SQL = "EXEC sp_droprolemember N'" + R[0].ToString() + "', N'" + this._Role + "';";
                if (this.SqlExecuteNonQuery(SQL))
                {
                    this.initForm();
                    this.SetSqlForSkript(SQL);
                }
                else System.Windows.Forms.MessageBox.Show("Removing the role " + R[0].ToString() + " failed");
            }
        }
        
        #endregion        

        #region Roles that inherit permissions of the current role
        
        private void toolStripButtonInRolesAdd_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT name FROM sysusers WHERE (uid = gid) AND (name <> N'public') AND (name NOT LIKE 'db_%') and Name <> '" + _Role + "'";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            ad.Fill(dt);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Please select the role that should be added");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                SQL = "EXEC sp_addrolemember N'" + this._Role + "', N'" + f.String + "';";
                if (this.SqlExecuteNonQuery(SQL))
                {
                    this.initForm();
                    this.SetSqlForSkript(SQL);
                }
                else System.Windows.Forms.MessageBox.Show("Adding a new role failed");
            }
        }

        private void toolStripButtonInRolesRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxContainedInRoles.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxContainedInRoles.SelectedItem;
                string SQL = "EXEC sp_droprolemember N'" + this._Role + "', N'" + R[0].ToString() + "';";
                if (this.SqlExecuteNonQuery(SQL))
                {
                    this.initForm();
                    this.SetSqlForSkript(SQL);
                }
                else System.Windows.Forms.MessageBox.Show("Removing the role " + R[0].ToString() + " failed");
            }
        }
        
        #endregion

        #endregion

        #region Auxillary

        private void SetSqlForSkript(string SQL)
        {
            this.textBoxSQL.Text += SQL + "\r\n";
            this.textBoxSQL.Visible = true;
            this.labelSql.Visible = true;
            this.textBoxSQL.BackColor = System.Drawing.Color.Pink;
            this.textBoxSQL.ForeColor = System.Drawing.Color.Black;
        }

        private bool SqlExecuteNonQuery(string SQL)
        {
            bool OK = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex) { OK = false; }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return OK;
        }
        
        #endregion  

        #endregion

    }
}
