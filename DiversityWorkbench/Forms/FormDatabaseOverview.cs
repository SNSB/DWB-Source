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
    public partial class FormDatabaseOverview : Form
    {

        #region Parameter
        private DiversityWorkbench.ServerConnection _ServerConnection;

        #endregion

        #region Construction

        public FormDatabaseOverview(DiversityWorkbench.ServerConnection SC)
        {
            InitializeComponent();
            this._ServerConnection = SC;
            this.labelHeader.Text = "Overview for database " + SC.DatabaseName;
            this.Text = SC.DatabaseName;
            this.SetUser();
            this.SetRoles();
            this.setProjects();
        }

        #endregion

        private System.Data.DataTable _DtUser;
        private void SetUser()
        {
            try
            {
                // A.L.20200312: UserURI not in every database available
                //string SQL = "SELECT LoginName, CombinedNameCache, UserURI FROM UserProxy WHERE LoginName IN (SELECT name FROM sysusers WHERE issqluser = 1 OR isntuser = 1) ORDER BY LoginName";
                string SQL = "SELECT LoginName, CombinedNameCache FROM UserProxy WHERE LoginName IN (SELECT name FROM sysusers WHERE issqluser = 1 OR isntuser = 1) ORDER BY LoginName";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                if (this._DtUser == null) this._DtUser = new DataTable();
                else this._DtUser.Clear();
                a.Fill(this._DtUser);
                this.listBoxUser.DataSource = this._DtUser;
                this.listBoxUser.DisplayMember = "LoginName";
                this.listBoxUser.ValueMember = "LoginName";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable _DtRoles;
        private void SetRoles()
        {
            string SQL = "SELECT name FROM sysusers WHERE (uid = gid) AND (name <> N'public') AND name not like 'db_%'" +
                " ORDER BY name ";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (this._DtRoles == null) this._DtRoles = new DataTable();
            else this._DtRoles.Clear();
            a.Fill(this._DtRoles);
            this.listBoxRoles.DataSource = this._DtRoles;
            this.listBoxRoles.DisplayMember = "name";
            this.listBoxRoles.ValueMember = "name";
        }

        private void toolStripButtonDeleteUser_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string LoginName = "";
            string messsage = "";
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUser.SelectedItem;
            LoginName = R["LoginName"].ToString();
            if (MessageBox.Show(string.Format("Do you really want to delete the login '{0}' from the database?", LoginName), "Delete user", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                SQL = "use " + this._ServerConnection.DatabaseName + "; " +
                    "DELETE FROM UserProxy WHERE LoginName = '" + LoginName + "'; ";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref messsage);
                if (messsage != "")
                {
                    MessageBox.Show(messsage);
                    return;
                }
                SQL = "use " + this._ServerConnection.DatabaseName + "; " +
                    "IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + LoginName + "') " +
                    "DROP USER [" + LoginName + "]; ";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref messsage))
                    this.SetUser();
                else
                    MessageBox.Show(messsage);
            }
        }

        private void setProjects()
        {
            try
            {
                string SQL = "SELECT ProjectID, Project FROM ProjectProxy ORDER BY Project";
                System.Data.DataTable dtProject = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                a.Fill(dtProject);
                this.listBoxProjects.DataSource = dtProject;
                this.listBoxProjects.DisplayMember = "Project";
                this.listBoxProjects.ValueMember = "ProjectID";
            }
            catch (System.Exception ex)
            {
                this.tabControlProperties.TabPages.Remove(this.tabPageProjects);
            }
        }

        private void listBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ProjectID = "";
            if (this.listBoxProjects.SelectedValue.GetType() == typeof(System.Data.DataRowView))
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjects.SelectedValue;
                ProjectID = R["ProjectID"].ToString();
            }
            else if (this.listBoxProjects.SelectedValue.GetType() == typeof(int))
            {
                ProjectID = this.listBoxProjects.SelectedValue.ToString();
            }
            string SQL = "SELECT U.CombinedNameCache " +
                "FROM  ProjectUser AS P INNER JOIN " +
                "UserProxy AS U ON P.LoginName = U.LoginName " +
                "WHERE P.ProjectID = " + ProjectID +
                " ORDER BY U.CombinedNameCache";
            System.Data.DataTable dtProjectUser = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            a.Fill(dtProjectUser);
            this.listBoxProjectUser.DataSource = dtProjectUser;
            this.listBoxProjectUser.DisplayMember = "CombinedNameCache";
            this.listBoxProjectUser.ValueMember = "CombinedNameCache";

        }



    }
}
