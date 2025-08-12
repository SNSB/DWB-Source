using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormRequester : Form
    {
        #region Parameter
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionRequesterList;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionRequester;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionRequesterUpdate;
        public DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get 
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return _FormFunctions; 
            }
        }

        #endregion

        #region Construction
        DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        #endregion

        public FormRequester()
        {
            InitializeComponent();
            this.InitForm();
        }

        private void InitForm()
        {
            string SQL = "SELECT Collection.CollectionID, Collection.CollectionParentID, Collection.CollectionName, " +
                "Collection.CollectionAcronym, Collection.AdministrativeContactName, Collection.AdministrativeContactAgentURI, " +
                "Collection.Description, Collection.Location, Collection.CollectionOwner, Collection.DisplayOrder " +
                "FROM CollectionManager INNER JOIN Collection ON CollectionManager.AdministratingCollectionID = Collection.CollectionID " +
                "WHERE (CollectionManager.LoginName = USER_NAME()) " +
                "ORDER BY CollectionName";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dtColl = new DataTable();
            ad.Fill(dtColl);

            System.Data.DataTable dtAdminColl = dtColl.Copy();
            this.listBoxAdministratingCollection.DataSource = dtAdminColl;
            this.listBoxAdministratingCollection.DisplayMember = "CollectionName";
            this.listBoxAdministratingCollection.ValueMember = "CollectionID";

            SQL = "SELECT CombinedNameCache, LoginName " +
                "FROM UserGroups " +
                "WHERE (UserGroup = N'DiversityCollectionRequester') " +
                "ORDER BY CombinedNameCache";
            System.Data.DataTable dt = new DataTable();
            ad.SelectCommand.CommandText = SQL;
            ad.Fill(dt);
            this.listBoxUser.DataSource = dt;
            this.listBoxUser.DisplayMember = "CombinedNameCache";
            this.listBoxUser.ValueMember = "LoginName";
        }

        #region Requesters
        private void buttonCollectionAdd_Click(object sender, EventArgs e)
        {
            string SQL = "INSERT INTO CollectionRequester (LoginName, AdministratingCollectionID, IncludeSubcollections) VALUES ('" +
                this.listBoxUser.SelectedValue.ToString() + "', " + this.listBoxAdministratingCollection.SelectedValue.ToString() + ", ";
            if (this.checkBoxIncludeSubcollections.Checked) SQL += " 1)";
            else SQL += " 0)";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                C.ExecuteNonQuery();
            }
            catch { }
            con.Close();
            this.setCollectionRequester();
        }

        private void buttonCollectionRemove_Click(object sender, EventArgs e)
        {
            string SQL = "DELETE FROM CollectionRequester WHERE LoginName = '" +
                this.listBoxUser.SelectedValue.ToString() + "' AND AdministratingCollectionID = " + this.listBoxCollectionRequester.SelectedValue.ToString();
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            C.ExecuteNonQuery();
            con.Close();
            this.setCollectionRequester();
        }

        private void listBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.dataSetTransaction.CollectionRequester.Clear();
            this.setCollectionRequester();
            this.buildHierarchy();
        }

        private void setCollectionRequester()
        {
            if (this.listBoxUser.SelectedIndex > -1)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUser.SelectedItem;
                string Login = R["LoginName"].ToString();
                string SQL = "SELECT CollectionRequester.LoginName, CollectionRequester.AdministratingCollectionID, " +
                    "CollectionRequester.IncludeSubcollections, Collection.CollectionName " +
                    "FROM CollectionRequester INNER JOIN " +
                    "CollectionManager ON CollectionRequester.AdministratingCollectionID = CollectionManager.AdministratingCollectionID " +
                    "INNER JOIN Collection ON CollectionRequester.AdministratingCollectionID = Collection.CollectionID " +
                    "WHERE (CollectionManager.LoginName = USER_NAME()) " +
                    "AND (CollectionRequester.LoginName = N'" + Login + "')";
                if (this._SqlDataAdapterCollectionRequesterList == null)
                    this._SqlDataAdapterCollectionRequesterList = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapterCollectionRequesterList.SelectCommand.CommandText = SQL;
                System.Data.DataTable dt = new DataTable();
                this._SqlDataAdapterCollectionRequesterList.Fill(dt);
                this.listBoxCollectionRequester.DataSource = dt;
                this.listBoxCollectionRequester.DisplayMember = "CollectionName";
                this.listBoxCollectionRequester.ValueMember = "AdministratingCollectionID";

                SQL = "SELECT CollectionRequester.LoginName, CollectionRequester.AdministratingCollectionID, " +
                    "CollectionRequester.IncludeSubcollections " +
                    "FROM CollectionRequester " +
                    "WHERE (CollectionRequester.LoginName = N'" + Login + "')" +
                    "AND (AdministratingCollectionID IN " +
                    "(SELECT AdministratingCollectionID " +
                    "FROM CollectionManager " +
                    "WHERE (LoginName = USER_NAME())))";
                if (this._SqlDataAdapterCollectionRequester == null)
                    this._SqlDataAdapterCollectionRequester = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this.FormFunctions.updateTable(this.dataSetTransaction, "CollectionRequester", this._SqlDataAdapterCollectionRequester, this.BindingContext);
                this.dataSetTransaction.CollectionRequester.Clear();
                //this._SqlDataAdapterCollectionRequester.Fill(this.dataSetTransaction.CollectionRequester);
                this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterCollectionRequester, SQL, this.dataSetTransaction.CollectionRequester);
            }
            else
                this.listBoxCollectionRequester.DataSource = null;
        }

        #endregion

        #region Hierarchy
        private void listBoxCollectionRequester_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = this.listBoxCollectionRequester.SelectedIndex;
            this.collectionRequesterBindingSource.Position = i;
            this.buildHierarchy();            //if (this.listBoxCollectionRequester.SelectedIndex == -1)
            //    this.checkBoxIncludeSubcollections.Enabled = false;
            //else
            //{
            //    this.checkBoxIncludeSubcollections.Enabled = true;
            //    System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxCollectionRequester.SelectedItem;
            //    if (RV["IncludeSubcollections"].ToString() == "true")
            //        this.checkBoxIncludeSubcollections.Checked = true;
            //    else this.checkBoxIncludeSubcollections.Checked = false;
            //}

        }

        private void checkBoxIncludeSubcollections_CheckedChanged(object sender, EventArgs e)
        {
            //this.buildHierarchy();
        }
        
        private void checkBoxIncludeSubcollections_Click(object sender, EventArgs e)
        {
            //if (this.listBoxCollectionRequester.SelectedIndex != -1 && this.listBoxUser.SelectedIndex != -1)
            //{
            //    string SQL = "UPDATE CollectionRequester SET IncludeSubcollections = ";
            //    if (this.checkBoxIncludeSubcollections.Checked)
            //        SQL += " 1 ";
            //    else
            //        SQL += " 0 ";
            //    SQL += " WHERE LoginName = '" +
            //       this.listBoxUser.SelectedValue.ToString() + "' AND AdministratingCollectionID = " + this.listBoxCollectionRequester.SelectedValue.ToString();
            //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            //    try
            //    {
            //        con.Open();
            //        C.ExecuteNonQuery();
            //        con.Close();
            //        System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxCollectionRequester.SelectedItem;
            //        //RV.BeginEdit();
            //        //RV["IncludeSubcollections"] = this.checkBoxIncludeSubcollections.Checked;
            //        //RV.EndEdit();
            //        //RV.Row.AcceptChanges();
            //        int i = this.listBoxCollectionRequester.SelectedIndex;
            //        this.setCollectionRequester();
            //        this.listBoxCollectionRequester.SelectedIndex = i;
            //    }
            //    catch  { } 
            //    //this.buildHierarchy();
            //}
        }

        private void buildHierarchy()
        {
            try
            {
                this.treeViewCollectionHierarchy.Nodes.Clear();
                if (this.collectionRequesterBindingSource == null
                    || this.collectionRequesterBindingSource.Current == null) return;
                //if (this.listBoxCollectionRequester.SelectedIndex == -1
                //    || this.listBoxCollectionRequester.DataSource == null) return;
                int AdminCollID = 0;
                string CollectionID = "";
                System.Data.DataRowView RV = (System.Data.DataRowView)this.collectionRequesterBindingSource.Current;
                if (!int.TryParse(RV["AdministratingCollectionID"].ToString(), out AdminCollID))
                    return;
                
                //if (!int.TryParse(this.listBoxCollectionRequester.SelectedValue.ToString(), out AdminCollID))
                //    return;
                else
                    CollectionID = AdminCollID.ToString();
                string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                    "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                    "Location, CollectionOwner, DisplayOrder FROM Collection WHERE CollectionID = " + CollectionID;
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (this.checkBoxIncludeSubcollections.Checked)
                {
                    SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                    "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                    "Location, CollectionOwner, DisplayOrder  FROM dbo.CollectionChildNodes(" + CollectionID + ")";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dt);
                }
                DiversityWorkbench.Hierarchy H = new DiversityWorkbench.Hierarchy(this.treeViewCollectionHierarchy, dt, "CollectionID", "CollectionParentID", "CollectionName", "CollectionName");
                H.buildHierarchy();

            }
            catch  { }
        }
        
        #endregion

        private void FormRequester_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransaction.CollectionRequester". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionRequesterTableAdapter.Fill(this.dataSetTransaction.CollectionRequester);

        }


    }
}
