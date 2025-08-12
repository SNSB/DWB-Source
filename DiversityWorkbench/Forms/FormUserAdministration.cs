using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormUserAdministration : Form
    {

        #region Parameter
        private System.Data.DataTable dtProjectsBlocked;
        private System.Data.DataTable dtProjectsAccess;
        private System.Data.DataTable dtLogins;
        private System.Data.DataTable dtUser;
        private System.Data.DataTable dtRoles;
        private System.Data.DataTable dtUserRoles;
        private Microsoft.Data.SqlClient.SqlConnection SqlConnection;

        private string User = "";
        private string LoginName = "";
        private int ProjectID = -1;

        #endregion

        #region Construction

        public FormUserAdministration(string ConnectionString, string PathHelpProvider)
        {
            InitializeComponent();
            // online manual
            this.helpProvider.HelpNamespace = PathHelpProvider;
            try
            {
                this.SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", this.SqlConnection);
                this.SqlConnection.Open();
                c.ExecuteNonQuery();
                this.SqlConnection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("unvalid connection");
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                this.Close();
            }
            this.InitLists();
            this.initAgent();
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


        #region Lists

        private void InitLists()
        {
            this.SetLogins();
            this.SetUser();
            this.SetUserProjects();
            this.SetRoles();
        }

        private void SetLogins()
        {
            try
            {
                string SQL = "SELECT name FROM sysusers WHERE (issqluser = 1 OR isntuser = 1) " +
                    " AND name NOT IN (SELECT LoginName FROM UserProxy) and name <> 'sys' and name <> 'INFORMATION_SCHEMA' " +
                    " ORDER BY name ";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
                if (this.dtLogins == null) this.dtLogins = new DataTable();
                else this.dtLogins.Clear();
                a.Fill(this.dtLogins);
                this.listBoxDatabaseAccounts.DataSource = this.dtLogins;
                this.listBoxDatabaseAccounts.DisplayMember = "name";
                this.listBoxDatabaseAccounts.ValueMember = "name";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetUser()
        {
            try
            {
                string SQL = "SELECT LoginName, CombinedNameCache, UserURI FROM UserProxy WHERE LoginName IN (SELECT name FROM sysusers WHERE issqluser = 1 OR isntuser = 1) ORDER BY LoginName";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
                if (this.dtUser == null) this.dtUser = new DataTable();
                else this.dtUser.Clear();
                a.Fill(this.dtUser);
                this.listBoxUser.DataSource = this.dtUser;
                this.listBoxUser.DisplayMember = "LoginName";
                this.listBoxUser.ValueMember = "LoginName";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetUserProjects()
        {
            try
            {
                string SQL = "SELECT ProjectProxy.ProjectID, ProjectProxy.Project " +
                    "FROM ProjectUser INNER JOIN " +
                    "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " +
                    "WHERE (ProjectUser.LoginName = N'" + this.User + "') ORDER BY ProjectProxy.Project";
                if (this.radioButtonOrderProjectByID.Checked)
                    SQL = "SELECT ProjectProxy.ProjectID, cast(ProjectProxy.ProjectID as varchar) + ' - ' + ProjectProxy.Project AS Project " +
                    "FROM ProjectUser INNER JOIN " +
                    "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " +
                    "WHERE (ProjectUser.LoginName = N'" + this.User + "') ORDER BY ProjectProxy.ProjectID";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
                if (this.dtProjectsAccess == null) this.dtProjectsAccess = new DataTable();
                else this.dtProjectsAccess.Clear();
                a.Fill(this.dtProjectsAccess);
                this.listBoxProjectsAvailable.DataSource = this.dtProjectsAccess;
                this.listBoxProjectsAvailable.DisplayMember = "Project";
                this.listBoxProjectsAvailable.ValueMember = "ProjectID";

                SQL = "SELECT ProjectID, Project " +
                    "FROM         ProjectProxy " +
                    "WHERE     (NOT (ProjectID IN " +
                    "(SELECT     ProjectID " +
                    "FROM          ProjectUser " +
                    "WHERE      (LoginName = N'" + this.User + "')))) ORDER BY Project";
                if (this.radioButtonOrderProjectByID.Checked)
                    SQL = "SELECT ProjectID, cast(ProjectProxy.ProjectID as varchar) + ' - ' + Project AS Project " +
                        "FROM         ProjectProxy " +
                        "WHERE     (NOT (ProjectID IN " +
                        "(SELECT     ProjectID " +
                        "FROM          ProjectUser " +
                        "WHERE      (LoginName = N'" + this.User + "')))) ORDER BY ProjectID";
                a.SelectCommand.CommandText = SQL;
                if (this.dtProjectsBlocked == null) this.dtProjectsBlocked = new DataTable();
                else this.dtProjectsBlocked.Clear();
                a.Fill(this.dtProjectsBlocked);
                this.listBoxProjectsNotAvailable.DataSource = this.dtProjectsBlocked;
                this.listBoxProjectsNotAvailable.DisplayMember = "Project";
                this.listBoxProjectsNotAvailable.ValueMember = "ProjectID";
            }
            catch { }
        }

        private void listBoxUser_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int i = this.listBoxUser.SelectedIndex;
            if (i > -1)
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxUser.Items[i];
                this.User = rv[0].ToString();
                this.SetUserProjects();
                this.setUserRoles();
                this.textBoxCombinedNameCache.Text = rv[1].ToString();
                if (rv[2].Equals(System.DBNull.Value) || rv[2].ToString().Length == 0)
                    this.labelUserURI.Visible = false;
                else
                {
                    this.labelUserURI.Text = "URI: " + rv[2].ToString();// "URI of user: " + rv[2].ToString();
                    this.labelUserURI.Visible = true;
                }
                this.setAgent(this.User);
            }
        }

        private void SetRoles()
        {
            string SQL = "SELECT name FROM sysusers WHERE (uid = gid) AND (name <> N'public') AND (name NOT LIKE 'db_%') " +
                " ORDER BY name ";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
            if (this.dtRoles == null) this.dtRoles = new DataTable();
            else this.dtRoles.Clear();
            a.Fill(this.dtRoles);
            this.listBoxRoles.DataSource = this.dtRoles;
            this.listBoxRoles.DisplayMember = "name";
            this.listBoxRoles.ValueMember = "name";
        }

        private void setUserRoles()
        {
            string SQL = "SELECT sysusers_1.name " +
                "FROM sysmembers INNER JOIN " +
                "sysusers ON sysmembers.memberuid = sysusers.uid INNER JOIN " +
                "sysusers sysusers_1 ON sysmembers.groupuid = sysusers_1.uid " +
                "WHERE (sysusers.name = N'" + this.User + "') " +
                "ORDER BY sysusers_1.name";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
            if (this.dtUserRoles == null) this.dtUserRoles = new DataTable();
            else this.dtUserRoles.Clear();
            a.Fill(this.dtUserRoles);
            this.listBoxUserRoles.DataSource = this.dtUserRoles;
            this.listBoxUserRoles.DisplayMember = "name";
            this.listBoxUserRoles.ValueMember = "name";
        }

        private void SetServerLogins()
        {
            string SQL = "SELECT name FROM syslogins WHERE (issqluser = 1 OR isntuser = 1) " +
                " AND name NOT IN (SELECT LoginName FROM UserProxy) " +
                " ORDER BY name ";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnection);
            if (this.dtLogins == null) this.dtLogins = new DataTable();
            else this.dtLogins.Clear();
            a.Fill(this.dtLogins);
            this.listBoxDatabaseAccounts.DataSource = this.dtLogins;
            this.listBoxDatabaseAccounts.DisplayMember = "name";
            this.listBoxDatabaseAccounts.ValueMember = "name";
        }

        #endregion

        #region Buttons

        private void buttonUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxDatabaseAccounts.SelectedIndex;
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDatabaseAccounts.Items[i];
                this.LoginName = rv[0].ToString();
                string SQL = "INSERT INTO UserProxy (LoginName, CombinedNameCache) VALUES ('" + this.LoginName + "', '" + this.LoginName + "')";
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetLogins();
                this.SetUser();
                this.SetUserProjects();
            }
            catch { }
        }

        private void buttonUserRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxUser.SelectedIndex;
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxUser.Items[i];
                this.LoginName = rv[0].ToString();
                string SQL = "DELETE FROM UserProxy WHERE LoginName = '" + this.LoginName + "'";
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetLogins();
                this.SetUser();
                this.SetUserProjects();
            }
            catch { }
        }

        private void buttonRoleAdd_Click(object sender, EventArgs e)
        {
            string Role = this.listBoxRoles.SelectedValue.ToString();
            string Member = this.listBoxUser.SelectedValue.ToString();
            string SQL = "EXEC sp_addrolemember N'" + Role + "', N'" + Member + "'";
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                c.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex) { }
            this.setUserRoles();
        }

        private void buttonRoleRemove_Click(object sender, EventArgs e)
        {
            string Role = this.listBoxUserRoles.SelectedValue.ToString();
            string Member = this.listBoxUser.SelectedValue.ToString();
            string SQL = "EXEC sp_droprolemember N'" + Role + "', N'" + Member + "'";
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                c.ExecuteNonQuery();
                con.Close();
            }
            catch { }
            this.setUserRoles();
        }

        private void buttonProjectAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ProjectID = System.Int32.Parse(this.listBoxProjectsNotAvailable.SelectedValue.ToString());
                string SQL = "INSERT INTO ProjectUser (LoginName, ProjectID) VALUES ('" + this.User + "', " + this.ProjectID.ToString() + ")";
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch { }
        }

        private void buttonProjectRemove_Click(object sender, EventArgs e)
        {
            try
            {
                this.ProjectID = System.Int32.Parse(this.listBoxProjectsAvailable.SelectedValue.ToString());
                string SQL = "DELETE FROM ProjectUser WHERE LoginName = '" + this.User + "' AND ProjectID = " + this.ProjectID.ToString();
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed") this.SqlConnection.Open();
                Cmd.ExecuteNonQuery();
                this.SqlConnection.Close();
                this.SetUserProjects();
            }
            catch { }
        }

        #endregion

        #region Roles

        private void showRolePermissions(string Role)
        {
            string SQL = "select sys.objects.name, sys.database_permissions.permission_name, sys.database_permissions.state_desc " +
                "from sys.database_permissions , sys.database_principals , sys.objects " +
                "where sys.database_principals.principal_id = sys.database_permissions.grantee_principal_id " +
                "and sys.database_permissions.major_id = sys.objects.object_id " +
                "and sys.database_principals.name = '" + Role + "' " +
                "order by sys.objects.name, sys.database_permissions.permission_name, sys.database_permissions.state_desc";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.dataGridViewRolePermissions.DataSource = dt;
                this.dataGridViewRolePermissions.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void showRoleMembers(string Role)
        {
            string SQL = "select pU.name " +
                "from sys.database_principals pR, sys.database_role_members, sys.database_principals pU " +
                "where sys.database_role_members.role_principal_id = pR.principal_id " +
                "and sys.database_role_members.member_principal_id = pU.principal_id " +
                "and pR.name = '" + Role + "' and pU.type <> 'R' " +
                "order  by pU.name";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.listBoxRoleMembers.DataSource = dt;
                this.listBoxRoleMembers.DisplayMember = "name";
                this.listBoxRoleMembers.Enabled = false;
                this.listBoxRoleMembers.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void showIncludedRoles(string Role)
        {
            string SQL = "select pU.name " +
                "from sys.database_principals pR, sys.database_role_members, sys.database_principals pU " +
                "where sys.database_role_members.role_principal_id = pR.principal_id " +
                "and sys.database_role_members.member_principal_id = pU.principal_id " +
                "and pR.name = '" + Role + "'  and pU.type = 'R' " +
                "order  by pU.name";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.listBoxIncludedRoles.DataSource = dt;
                this.listBoxIncludedRoles.DisplayMember = "name";
                this.listBoxIncludedRoles.Enabled = false;
                this.listBoxIncludedRoles.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxRoles.SelectedValue != null)
            {
                string Role = this.listBoxRoles.SelectedValue.ToString();
                this.showRolePermissions(Role);
                this.showRoleMembers(Role);
                this.showIncludedRoles(Role);
            }
        }

        #endregion

        #region User

        private void buttonCreateUser_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormCreateUser f = new DiversityWorkbench.Forms.FormCreateUser(this.SqlConnection);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                string SQL = "INSERT INTO UserProxy (LoginName, CombinedNameCache";
                if (f.UserURI.Length > 0) SQL += ", UserURI";
                SQL += ") VALUES ('" + f.Login + "', '" + f.UserName + "'";
                if (f.UserURI.Length > 0) SQL += ", " + f.UserURI;
                SQL += ")";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                try
                {
                    if (this.SqlConnection.State == ConnectionState.Closed)
                        this.SqlConnection.Open();
                    C.ExecuteNonQuery();
                    this.SqlConnection.Close();
                    this.InitLists();
                }
                catch
                {
                }
            }
        }

        private void buttonSaveUserCombinedNameCache_Click(object sender, EventArgs e)
        {
            if (this.listBoxUser.SelectedIndex > -1)
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxUser.Items[this.listBoxUser.SelectedIndex];
                string SQL = "UPDATE UserProxy SET CombinedNameCache = '" + this.textBoxCombinedNameCache.Text + " ' WHERE LoginName = '" + rv[0].ToString() + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                    rv.BeginEdit();
                    rv[1] = this.textBoxCombinedNameCache.Text;
                    rv.EndEdit();
                }
                catch { }
            }
        }

        private void buttonSynchronizeUser_Click(object sender, EventArgs e)
        {
            int i = this.listBoxUser.SelectedIndex;
            System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxUser.Items[this.listBoxUser.SelectedIndex];
            if (rv["UserURI"].Equals(System.DBNull.Value))
            {
                string LoginName = rv["LoginName"].ToString();
                //if (LoginName.IndexOf("\\") > -1) LoginName = LoginName.Substring(LoginName.IndexOf("\\") + 1);
                string idUri = global::DiversityWorkbench.Properties.Settings.Default.DiversityWokbenchIDUrl + "users/";
                string SQL = "UPDATE UserProxy SET UserURI = " + idUri + " + CAST(DiversityUsers.dbo.UserInfo_AssociatedLoginName.UserID AS VARCHAR) " +
                    "FROM DiversityUsers.dbo.UserInfo_AssociatedLoginName, UserProxy " +
                    "WHERE DiversityUsers.dbo.UserInfo_AssociatedLoginName.LoginName = '" + LoginName + "' " +
                    "AND UserProxy.LoginName = '" + rv["LoginName"] + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    C.CommandText = "SELECT TOP 1 UserURI FROM UserProxy WHERE LoginName = '" + rv["LoginName"].ToString() + "'";
                    rv["UserURI"] = C.ExecuteScalar()?.ToString();
                    con.Close();
                }
                catch { }
            }

            if (!rv["UserURI"].Equals(System.DBNull.Value))
            {
                string idUri = global::DiversityWorkbench.Properties.Settings.Default.DiversityWokbenchIDUrl + "users/";
                string SQL = "UPDATE UserProxy SET CombinedNameCache = DiversityUsers.dbo.UserInfo.CombinedNameCache " +
                    "FROM UserProxy INNER JOIN DiversityUsers.dbo.UserInfo ON UserProxy.UserURI = " + idUri + " + CAST(DiversityUsers.dbo.UserInfo.UserID AS VARCHAR)";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
                catch { }
            }
            this.InitLists();
            if (i > -1)
                this.listBoxUser.SelectedIndex = i;
        }

        #endregion

        #region Project

        private void buttonCreateProject_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New project", "Please enter a name for the new project", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                string SQL = "SELECT MIN(ProjectID) FROM ProjectProxy";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                try
                {
                    if (this.SqlConnection.State == ConnectionState.Closed)
                        this.SqlConnection.Open();
                    int ID = -1;
                    try { ID = int.Parse(C.ExecuteScalar()?.ToString()); } catch { }
                    ID--;
                    C.CommandText = "INSERT INTO ProjectProxy (ProjectID, Project) VALUES (" + ID.ToString() + ", '" + f.String + "')"; ;
                    C.ExecuteNonQuery();
                    this.SqlConnection.Close();
                    this.InitLists();
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private void buttonSynchronizeProjects_Click(object sender, EventArgs e)
        {
            string SQL = "UPDATE ProjectProxy " +
                "SET Project = DiversityProjects.dbo.Project.Project " +
                "FROM ProjectProxy INNER JOIN " +
                "DiversityProjects.dbo.Project ON ProjectProxy.ProjectID = DiversityProjects.dbo.Project.ProjectID AND  " +
                "ProjectProxy.Project <> DiversityProjects.dbo.Project.Project";
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
            try
            {
                if (this.SqlConnection.State == ConnectionState.Closed)
                    this.SqlConnection.Open();
                C.ExecuteNonQuery();
                C.CommandText = "INSERT INTO ProjectProxy (ProjectID, Project) " +
                    "SELECT ProjectID, Project " +
                    "FROM DiversityProjects.dbo.Project " +
                    "WHERE (ProjectID NOT IN " +
                    "(SELECT ProjectID " +
                    "FROM ProjectProxy)) AND (ProjectID > 0)";
                C.ExecuteNonQuery();
                //string Message = "";
                //try { Message = C.ExecuteScalar()?.ToString(); }
                //catch { }
                //if (Message.Length > 0) System.Windows.Forms.MessageBox.Show(Message);
                this.SqlConnection.Close();
                this.InitLists();
            }
            catch
            {
            }

        }

        #endregion

        #region Agent

        private void initAgent()
        {
            try
            {
                this.userControlModuleRelatedEntryAgentURI.comboBoxLocalValues.Visible = false;
                string SQL = "SELECT COUNT(*) AS Anzahl " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE TABLE_NAME = 'UserProxy' AND COLUMN_NAME = 'AgentURI'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                int i = 0;
                if (int.TryParse(C.ExecuteScalar()?.ToString(), out i) && i == 1)
                {
                    this.userControlModuleRelatedEntryAgentURI.Visible = true;
                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryAgentURI.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                    this.userControlModuleRelatedEntryAgentURI.buttonOpenModule.Click += new System.EventHandler(this.ConnectToDiversityAgents_Click);
                    this.userControlModuleRelatedEntryAgentURI.buttonDeleteURI.Click += new System.EventHandler(this.RemoveConnectionToDiversityAgents_Click);
                    this.userControlModuleRelatedEntryAgentURI.textBoxValue.Enabled = false;
                    this.userControlModuleRelatedEntryAgentURI.comboBoxLocalValues.Enabled = false;
                }
                else
                    this.userControlModuleRelatedEntryAgentURI.Visible = false;
                con.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ConnectToDiversityAgents_Click(object sender, EventArgs e)
        {
            if (this.userControlModuleRelatedEntryAgentURI.labelURI.Text.Length > 0)
            {
                string URI = this.userControlModuleRelatedEntryAgentURI.labelURI.Text;
                string SQL = "UPDATE UserProxy SET AgentURI = '" + URI + "' WHERE LoginName = '" + this.User + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                try
                {
                    C.ExecuteNonQuery();
                }
                catch { }
                con.Close();
                this.setAgent(this.User);
            }
        }

        private void RemoveConnectionToDiversityAgents_Click(object sender, EventArgs e)
        {
            this.userControlModuleRelatedEntryAgentURI.textBoxValue.Text = "";
            string SQL = "UPDATE UserProxy SET AgentURI = NULL WHERE LoginName = '" + this.User + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                C.ExecuteNonQuery();
            }
            catch { }
            con.Close();
        }

        private void setAgent(string Login)
        {
            try
            {
                string SQL = "SELECT COUNT(*) AS Anzahl " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE TABLE_NAME = 'UserProxy' AND COLUMN_NAME = 'AgentURI'";
                string AgentURI = "";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                int i = 0;
                if (int.TryParse(C.ExecuteScalar()?.ToString(), out i) && i == 1)
                {
                    C.CommandText = "SELECT AgentURI FROM UserProxy WHERE LoginName = '" + Login + "'";
                    AgentURI = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    this.userControlModuleRelatedEntryAgentURI.labelURI.Text = AgentURI;
                    if (AgentURI.Length > 0)
                    {
                        this.userControlModuleRelatedEntryAgentURI.textBoxValue.Text = this.textBoxCombinedNameCache.Text;
                        this.userControlModuleRelatedEntryAgentURI.buttonOpenModule.Visible = false;
                    }
                    else
                    {
                        this.userControlModuleRelatedEntryAgentURI.textBoxValue.Text = "";
                        this.userControlModuleRelatedEntryAgentURI.buttonOpenModule.Visible = true;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Help
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

        #endregion

    }
}