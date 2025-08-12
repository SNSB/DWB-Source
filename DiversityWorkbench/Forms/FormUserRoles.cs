using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormUserRoles : Form
    {
        #region Parameter
        //private System.Data.DataTable dtProjectsBlocked;
        //private System.Data.DataTable dtProjectsAccess;
        private System.Data.DataTable dtLogins;
        //private System.Data.DataTable dtUser;
        private System.Data.DataTable dtRoles;
        private System.Data.DataTable dtUserRoles;
        //private Microsoft.Data.SqlClient.SqlConnection SqlConnection;

        //private string User = "";
        private string LoginName = "";
        //private int ProjectID = -1;

        #endregion

        public FormUserRoles()
        {
            InitializeComponent();
            this.InitLists();
        }

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
            this.SetRoles();
        }

        private void SetLogins()
        {
            try
            {
                string SQL = "SELECT name FROM sysusers WHERE (issqluser = 1 OR isntuser = 1) " +
                    " AND name <> 'sys' and name <> 'INFORMATION_SCHEMA' " +
                    " ORDER BY name ";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
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

        private void listBoxDatabaseAccounts_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int i = this.listBoxDatabaseAccounts.SelectedIndex;
            if (i > -1)
            {
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDatabaseAccounts.Items[i];
                this.LoginName = rv[0].ToString();
                this.setUserRoles();
            }
        }

        private void SetRoles()
        {
            string SQL = "SELECT name FROM sysusers WHERE (uid = gid) AND (name <> N'public') " +
                " ORDER BY name ";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
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
                "WHERE (sysusers.name = N'" + this.LoginName + "') " +
                "ORDER BY sysusers_1.name";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
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
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            if (this.dtLogins == null) this.dtLogins = new DataTable();
            else this.dtLogins.Clear();
            a.Fill(this.dtLogins);
            this.listBoxDatabaseAccounts.DataSource = this.dtLogins;
            this.listBoxDatabaseAccounts.DisplayMember = "name";
            this.listBoxDatabaseAccounts.ValueMember = "name";
        }


        #endregion

        #region Buttons

        private void buttonRoleAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxDatabaseAccounts.SelectedIndex;
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDatabaseAccounts.Items[i];
                this.LoginName = rv[0].ToString();
                string SQL = "exec sp_addrolemember N'" + this.listBoxRoles.SelectedValue.ToString() + "', N'" + this.listBoxDatabaseAccounts.SelectedValue.ToString() + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                if (con.State.ToString() == "Closed") con.Open();
                Cmd.ExecuteNonQuery();
                con.Close();
                this.SetRoles();
                this.setUserRoles();
            }
            catch { }
        }

        private void buttonRoleRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxDatabaseAccounts.SelectedIndex;
                System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDatabaseAccounts.Items[i];
                this.LoginName = rv[0].ToString();
                string SQL = "exec sp_droprolemember N'" + this.listBoxRoles.SelectedValue.ToString() + "', N'" + this.listBoxDatabaseAccounts.SelectedValue.ToString() + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand Cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                if (con.State.ToString() == "Closed") con.Open();
                Cmd.ExecuteNonQuery();
                con.Close();
                this.SetRoles();
                this.setUserRoles();
            }
            catch { }
        }


        #endregion		



    }
}