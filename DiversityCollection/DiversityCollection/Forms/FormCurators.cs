using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection
{
    public partial class FormManagers : Form
    {

        #region Parameter
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionCurator;
        
        #endregion

        #region Construction
        public FormManagers()
        {
            InitializeComponent();
            string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                "AdministrativeContactName, AdministrativeContactAgentURI, Description,  " +
                "Location, CollectionOwner " +
                "FROM Collection " +
                "ORDER BY CollectionName";
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this.dataSetTransaction.Collection);

            System.Data.DataTable dtAdminColl = this.dataSetTransaction.Collection.Copy();
            this.listBoxAdministratingCollection.DataSource = dtAdminColl;
            this.listBoxAdministratingCollection.DisplayMember = "CollectionName";
            this.listBoxAdministratingCollection.ValueMember = "CollectionID";

            SQL = "SELECT CASE WHEN [CombinedNameCache] IS NULL THEN [LoginName] ELSE [CombinedNameCache] END AS [User], UserProxy.LoginName " +
                "FROM sys.sysmembers INNER JOIN " +
                "sys.sysusers ON sys.sysmembers.memberuid = sys.sysusers.uid INNER JOIN " +
                "sys.sysusers AS sysusers_1 ON sys.sysmembers.groupuid = sysusers_1.uid INNER JOIN " +
                "UserProxy ON sys.sysusers.name = UserProxy.LoginName " +
                "WHERE (sysusers_1.name = N'DiversityCollectionCurator') " +
                "OR (sysusers_1.name = N'DiversityCollectionManager') " +
                "ORDER BY [User]";
            System.Data.DataTable dt = new DataTable();
            ad.SelectCommand.CommandText = SQL;
            ad.Fill(dt);
            this.listBoxUser.DataSource = dt;
            this.listBoxUser.DisplayMember = "User";
            this.listBoxUser.ValueMember = "LoginName";

            //this.setCollectionCurator();
        }
        
        #endregion

        #region Curators
        private void buttonCollectionAdd_Click(object sender, EventArgs e)
        {
            string SQL = "INSERT INTO CollectionCurator (LoginName, AdministratingCollectionID) VALUES ('" +
                this.listBoxUser.SelectedValue.ToString() + "', " + this.listBoxAdministratingCollection.SelectedValue.ToString() + ")";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                C.ExecuteNonQuery();
            }
            catch { }
            con.Close();
            this.setCollectionCurator();
        }

        private void buttonCollectionRemove_Click(object sender, EventArgs e)
        {
            string SQL = "DELETE FROM CollectionCurator WHERE LoginName = '" +
                this.listBoxUser.SelectedValue.ToString() + "' AND AdministratingCollectionID = " + this.listBoxCollectionCurator.SelectedValue.ToString();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            C.ExecuteNonQuery();
            con.Close();
            this.setCollectionCurator();
        }

        private void listBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataSetTransaction.CollectionCurator.Clear();
            this.setCollectionCurator();
        }

        private void setCollectionCurator()
        {
            if (this.listBoxUser.SelectedIndex > -1)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUser.SelectedItem;
                string Login = R["LoginName"].ToString();
                string SQL = "SELECT CollectionCurator.LoginName, CollectionCurator.AdministratingCollectionID, Collection.CollectionName " +
                    "FROM  CollectionCurator INNER JOIN " +
                    "Collection ON CollectionCurator.AdministratingCollectionID = Collection.CollectionID " +
                    "WHERE (CollectionCurator.LoginName = N'" + Login + "') ORDER BY Collection.CollectionName";
                if (this._SqlDataAdapterCollectionCurator == null)
                {
                    this._SqlDataAdapterCollectionCurator = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                }
                this._SqlDataAdapterCollectionCurator.SelectCommand.CommandText = SQL;
                System.Data.DataTable dt = new DataTable();
                this._SqlDataAdapterCollectionCurator.Fill(dt);
                this.listBoxCollectionCurator.DataSource = dt;
                this.listBoxCollectionCurator.DisplayMember = "CollectionName";
                this.listBoxCollectionCurator.ValueMember = "AdministratingCollectionID";
            }
            else
                this.listBoxCollectionCurator.DataSource = null;
        }

        #endregion

        #region External credentials
        private void buttonExternalAdd_Click(object sender, EventArgs e)
        {
            //string SQL = "INSERT INTO CollectionCurator (LoginName, AdministratingCollectionID) VALUES ('" +
            //    this.listBoxUser.SelectedValue.ToString() + "', " + this.listBoxAdministratingCollection.SelectedValue.ToString() + ")";
            //System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            //con.Open();
            //try
            //{
            //    C.ExecuteNonQuery();
            //}
            //catch { }
            //con.Close();
            //this.setCollectionCurator();
        }

        private void buttonExternalRemove_Click(object sender, EventArgs e)
        {
            //string SQL = "DELETE FROM CollectionCurator WHERE LoginName = '" +
            //    this.listBoxUser.SelectedValue.ToString() + "' AND CollectionID = " + this.listBoxCollectionCurator.SelectedValue.ToString();
            //System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            //con.Open();
            //C.ExecuteNonQuery();
            //con.Close();
            //this.setCollectionCurator();
        }

        private void listBoxExternalUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.dataSetTransaction.CollectionCurator.Clear();
            //this.setExternalCredentials();
        }


        private void setExternalCredentials()
        {
            //if (this.listBoxUser.SelectedIndex > -1)
            //{
            //    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUser.SelectedItem;
            //    string Login = R["LoginName"].ToString();
            //    string SQL = "SELECT CollectionCurator.LoginName, CollectionCurator.AdministratingCollectionID, Collection.CollectionName " +
            //        "FROM  CollectionCurator INNER JOIN " +
            //        "Collection ON CollectionCurator.AdministratingCollectionID = Collection.CollectionID " +
            //        "WHERE (CollectionCurator.LoginName = N'" + Login + "') ORDER BY Collection.CollectionName";
            //    if (this._SqlDataAdapterCollectionCurator == null)
            //    {
            //        this._SqlDataAdapterCollectionCurator = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    }
            //    this._SqlDataAdapterCollectionCurator.SelectCommand.CommandText = SQL;
            //    System.Data.DataTable dt = new DataTable();
            //    this._SqlDataAdapterCollectionCurator.Fill(dt);
            //    this.listBoxCollectionCurator.DataSource = dt;
            //    this.listBoxCollectionCurator.DisplayMember = "CollectionName";
            //    this.listBoxCollectionCurator.ValueMember = "AdministratingCollectionID";
            //}
            //else
            //    this.listBoxCollectionCurator.DataSource = null;
        }

        #endregion


    }
}