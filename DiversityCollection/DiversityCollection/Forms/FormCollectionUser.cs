using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormCollectionUser : Form
    {
        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionUser;
        System.Data.DataTable _dtCollection;

        #endregion

        #region Construction

        public FormCollectionUser()
        {
            InitializeComponent();
            try
            {
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                   "AdministrativeContactName, AdministrativeContactAgentURI, Description,  " +
                   "Location, CollectionOwner, DisplayText " +
                   "FROM dbo.CollectionHierarchyAll() " +
                   "ORDER BY DisplayText";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._dtCollection = new DataTable();
                ad.Fill(this._dtCollection);

                this.listBoxCollection.DataSource = this._dtCollection;
                this.listBoxCollection.DisplayMember = "DisplayText";
                this.listBoxCollection.ValueMember = "CollectionID";

                SQL = "SELECT CASE WHEN [CombinedNameCache] IS NULL OR UserProxy.LoginName = 'dbo' THEN [LoginName] ELSE [CombinedNameCache] END AS [User], UserProxy.LoginName " + //#131
                    "FROM sys.sysusers INNER JOIN UserProxy ON sys.sysusers.name = UserProxy.LoginName and UserProxy.LoginName <> 'dbo' ORDER BY [User]";
                System.Data.DataTable dt = new DataTable();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dt);
                this.listBoxUser.DataSource = dt;
                this.listBoxUser.DisplayMember = "User";
                this.listBoxUser.ValueMember = "LoginName";

                //#131
                string TableName = "CollectionUser";
                bool OK = DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "INSERT");
                this.buttonCollectionAdd.Enabled = OK;

                OK = DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "DELETE");
                this.buttonCollectionRemove.Enabled = OK;


            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Users

        private void buttonCollectionAdd_Click(object sender, EventArgs e)
        {
            string SQL = "INSERT INTO CollectionUser (LoginName, CollectionID) VALUES ('" +
                this.listBoxUser.SelectedValue.ToString() + "', " + this.listBoxCollection.SelectedValue.ToString() + ")";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                C.ExecuteNonQuery();
            }
            catch { }
            con.Close();
            this.setCollectionUser();
        }

        private void buttonCollectionRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxCollectionUser.SelectedValue != null) //#51
            {
                string SQL = "DELETE FROM CollectionUser WHERE LoginName = '" +
                    this.listBoxUser.SelectedValue.ToString() + "' AND CollectionID = " + this.listBoxCollectionUser.SelectedValue.ToString();
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                this.setCollectionUser();
            }
            else { System.Windows.Forms.MessageBox.Show("No collection selected", "Nothing selected", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void listBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setCollectionUser();
        }

        private void setCollectionUser()
        {
            try
            {
                if (this.listBoxUser.SelectedIndex > -1)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUser.SelectedItem;
                    string Login = R["LoginName"].ToString();
                    string SQL = "SELECT CollectionUser.LoginName, CollectionUser.CollectionID, H.DisplayText " +
                        "FROM  CollectionUser INNER JOIN " +
                        "dbo.CollectionHierarchyAll() H ON CollectionUser.CollectionID = H.CollectionID " +
                        "WHERE (CollectionUser.LoginName = N'" + Login + "') ORDER BY H.DisplayText";
                    //if (this._SqlDataAdapterCollectionUser == null)
                    //{
                    //this._SqlDataAdapterCollectionUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    //}
                    //this._SqlDataAdapterCollectionUser.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    //this._SqlDataAdapterCollectionUser.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.listBoxCollectionUser.DataSource = dt;
                        this.listBoxCollectionUser.DisplayMember = "DisplayText";
                        this.listBoxCollectionUser.ValueMember = "CollectionID";
                    }
                    else
                    {
                        this.listBoxCollectionUser.DataSource = null;
                        this.listBoxCollectionUser.Items.Clear();
                        this.listBoxCollectionUser.Items.Add("NOT RESTRICTED");
                    }
                }
                else
                    this.listBoxCollectionUser.DataSource = null;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        private void FormCollectionUser_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
