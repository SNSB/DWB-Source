using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormManagers : Form
    {

        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionManager;
        private string ManagerDisplayText
        {
            get
            {
                if (this.toolStripComboBoxManagerListBy.SelectedItem.ToString() == "Login")
                    return "LoginName";
                else
                    return "CombinedNameCache";
            }
        }
        
        #endregion

        #region Construction

        public FormManagers()
        {
            InitializeComponent();
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();

            this.initForm();
        }
        private void initForm()
        {
            string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                "AdministrativeContactName, AdministrativeContactAgentURI, Description,  " +
                "Location, CollectionOwner, DisplayText " +
                "FROM [dbo].[CollectionHierarchyAll] () " +
                "ORDER BY DisplayText";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this.dataSetTransaction.Collection);

            System.Data.DataTable dtAdminColl = this.dataSetTransaction.Collection.Copy();
            this.listBoxAdministratingCollection.DataSource = dtAdminColl;
            this.listBoxAdministratingCollection.DisplayMember = "DisplayText";
            this.listBoxAdministratingCollection.ValueMember = "CollectionID";

            this.initManagerList();
        }

        private void initManagerList()
        {
            // #133
            bool IsAdmin = false;
            string SqlIsAdmin = "select sysusers_1.name  from sys.sysusers AS sysusers_1 WHERE (sysusers_1.name = N'Administrator')";
            IsAdmin = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlIsAdmin).Length > 0;

            string SQL = "SELECT CASE WHEN [CombinedNameCache] IS NULL OR UserProxy.LoginName = 'dbo' THEN [LoginName] ELSE [" + this.ManagerDisplayText + "] END AS [User], UserProxy.LoginName " + // #152
                "FROM sys.sysmembers INNER JOIN " +
                "sys.sysusers ON sys.sysmembers.memberuid = sys.sysusers.uid INNER JOIN " +
                "sys.sysusers AS sysusers_1 ON sys.sysmembers.groupuid = sysusers_1.uid INNER JOIN " +
                "UserProxy ON sys.sysusers.name COLLATE DATABASE_DEFAULT = UserProxy.LoginName COLLATE DATABASE_DEFAULT ";
            if (!IsAdmin) //#133
                SQL += "INNER JOIN master.sys.server_principals p ON p.name COLLATE DATABASE_DEFAULT = sys.sysusers.name COLLATE DATABASE_DEFAULT or sys.sysusers.name = 'dbo' and p.name = 'sa' ";
            SQL += "WHERE (sysusers_1.name = N'CollectionManager') " +
                "OR UserProxy.LoginName = N'dbo' " +
                "ORDER BY [User]";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.listBoxUser.DataSource = dt;
            this.listBoxUser.DisplayMember = "User";
            this.listBoxUser.ValueMember = "LoginName";
        }
        
        #endregion

        #region Managers

        private void buttonCollectionAdd_Click(object sender, EventArgs e)
        {
            foreach (System.Object O in this.listBoxAdministratingCollection.SelectedItems)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)O;
                this.ChangeManagerAccess(this.listBoxUser.SelectedValue.ToString(), R["CollectionID"].ToString(), ManagerAccess.Allow);
            }

            this.setCollectionManager();
        }

        private void buttonCollectionRemove_Click(object sender, EventArgs e)
        {
            foreach (System.Object O in this.listBoxCollectionManager.SelectedItems)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)O;
                this.ChangeManagerAccess(this.listBoxUser.SelectedValue.ToString(), R["AdministratingCollectionID"].ToString(), ManagerAccess.Deny);
            }

            this.setCollectionManager();
        }

        private enum ManagerAccess { Allow, Deny }
        private void ChangeManagerAccess(string LoginName, string AdministratingCollectionID, ManagerAccess Access)
        {
            string SQL = "";
            switch (Access)
            {
                case ManagerAccess.Allow:
                    SQL = "INSERT INTO CollectionManager (LoginName, AdministratingCollectionID) " +
                        "VALUES ('" + LoginName + "', " + AdministratingCollectionID + ")";
                    break;
                case ManagerAccess.Deny:
                    SQL = "DELETE FROM CollectionManager " +
                        "WHERE LoginName = '" + LoginName + "' AND AdministratingCollectionID = " + AdministratingCollectionID;
                    break;
            }
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        }

        private void listBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setCollectionManager();
        }

        private void setCollectionManager()
        {
            //#51 - Check permission
            bool OK = DiversityWorkbench.Forms.FormFunctions.Permissions("ManagerCollectionList", "SELECT");
            if (this.listBoxUser.SelectedIndex > -1 && OK)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUser.SelectedItem;
                string Login = R["LoginName"].ToString();
                string SQL = "SELECT M.LoginName, M.AdministratingCollectionID, C.DisplayText AS CollectionName " +
                    "FROM  CollectionManager AS M INNER JOIN " +
                    "[dbo].[CollectionHierarchyAll] () AS C ON M.AdministratingCollectionID = C.CollectionID " +
                    "WHERE (M.LoginName = N'" + Login + "') ORDER BY CollectionName";
                if (this._SqlDataAdapterCollectionManager == null)
                {
                    this._SqlDataAdapterCollectionManager = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                }
                this._SqlDataAdapterCollectionManager.SelectCommand.CommandText = SQL;
                System.Data.DataTable dt = new DataTable();
                this._SqlDataAdapterCollectionManager.Fill(dt);
                this.listBoxCollectionManager.DataSource = dt;
                this.listBoxCollectionManager.DisplayMember = "CollectionName";
                this.listBoxCollectionManager.ValueMember = "AdministratingCollectionID";
            }
            else
                this.listBoxCollectionManager.DataSource = null;
        }

        private void toolStripComboBoxManagerListBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.initManagerList();
        }

        private void toolStripButtonSynchronizeLogins_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "use master; " +
                    "select p.name, M.LoginName " +
                    "from sys.server_principals p, " +
                    "[" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[CollectionManager] M " +
                    "where p.name not like '##%##'  " +
                    "and p.type in ('U')  " +
                    "and p.name not like 'NT-%'  " +
                    "and p.name <> 'sa' " +
                    "and p.name like '%\\%' " +
                    "and p.name not like 'NT %\\%' " +
                    "and M.LoginName like '%\\%' " +
                    "and substring(p.name, charindex('\\', p.name), 255) = substring(M.LoginName, charindex('\\', M.LoginName), 255)  " +
                    "and substring(M.LoginName, 1, charindex('\\', M.LoginName) - 1) <>substring(p.name, 1, charindex('\\', p.name) - 1) " +
                    "order by type desc, name;";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 0)
                    System.Windows.Forms.MessageBox.Show("No differences found");
                else
                {
                    string Message = "";
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Message += R["name"].ToString() + " <> " + R["LoginName"].ToString() + "\r\n";
                    }
                    Message = "The following differing logins exist:\r\n" + Message + "Do you want to correct these?";
                    if (System.Windows.Forms.MessageBox.Show(Message, "Correct logins?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string SqlDulicates = "select U.LoginName, U.AdministratingCollectionID from  [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[CollectionManager] p, " +
                            "[" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[CollectionManager] U " +
                            "where p.LoginName like '%\\%'  " +
                            "and U.LoginName like '%\\%'  " +
                            "and U.AdministratingCollectionID = P.AdministratingCollectionID " +
                            "and substring(p.LoginName, charindex('\\', p.LoginName), 255) = substring(U.LoginName, charindex('\\', U.LoginName), 255)   " +
                            "and substring(U.LoginName, 1, charindex('\\', U.LoginName) - 1) <> substring(p.LoginName, 1, charindex('\\', p.LoginName) - 1)  " +
                            "order by substring(U.LoginName, charindex('\\', U.LoginName), 255), U.AdministratingCollectionID;";
                                                System.Data.DataTable dtDuplicates = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter adDuplicates = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlDulicates, DiversityWorkbench.Settings.ConnectionString);
                        adDuplicates.Fill(dtDuplicates);
                        if (dtDuplicates.Rows.Count > 0)
                        {
                            System.Collections.Generic.Dictionary<string, bool> Dict = new Dictionary<string, bool>();
                            foreach (System.Data.DataRow R in dtDuplicates.Rows)
                            {
                                Dict.Add(R["LoginName"].ToString() + ": " + R["AdministratingCollectionID"].ToString(), false);
                            }
                            DiversityWorkbench.Forms.FormGetMultiFromList f = new DiversityWorkbench.Forms.FormGetMultiFromList("Duplicates", "The following permissions are duplicates.\r\nPlease select those that should be deleted", Dict);
                            f.ShowDialog();
                            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                System.Collections.Generic.List<string> L = f.SelectedItems();
                                foreach (string S in L)
                                {
                                    if (System.Windows.Forms.MessageBox.Show("Remove permission " + S + "?", "Remove permission", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        string Login = S.Substring(0, S.IndexOf(":"));
                                        int CollectionID;
                                        if (!int.TryParse(S.Substring(S.IndexOf(":") + 1), out CollectionID))
                                            return;
                                        SQL = "Delete U from [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[CollectionManager] U WHERE U.LoginName = '" + Login + "' AND AdministratingCollectionID = " + CollectionID.ToString();
                                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                    }
                                }
                            }
                            else
                                return;
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



                        Message = "";
                        SQL = "use master; " +
                            "UPDATE M SET M.LoginName = p.name " +
                            "from sys.server_principals p, " +
                            "[" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[CollectionManager] M " +
                            "where p.name not like '##%##'  " +
                            "and p.type in ('U')  " +
                            "and p.name not like 'NT-%'  " +
                            "and p.name <> 'sa'   " +
                            "and p.name like '%\\%' " +
                            "and p.name not like 'NT %\\%' " +
                            "and M.LoginName like '%\\%' " +
                            "and substring(p.name, charindex('\\', p.name), 255) = substring(M.LoginName, charindex('\\', M.LoginName), 255)  " +
                            "and substring(M.LoginName, 1, charindex('\\', M.LoginName) - 1) <> substring(p.name, 1, charindex('\\', p.name) - 1)";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                        if (Message.Length > 0)
                            System.Windows.Forms.MessageBox.Show(Message);
                        this.initManagerList();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripComboBoxManagerListBy_DropDownClosed(object sender, EventArgs e)
        {
            this.initManagerList();
        }


        private void listBoxAdministratingCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //System.Data.DataRow r = (System.Data.DataRow)this.listBoxAdministratingCollection.SelectedItem;
                int ID;
                if (int.TryParse(this.listBoxAdministratingCollection.SelectedValue.ToString(), out ID))
                {
                    string SQL = "SELECT [CollectionName] + case when [Description] <> '' then ': ' + [Description] else '' end + case when [Type] is null then '' else '. Type: ' + [Type] end " +
                        "FROM[dbo].[Collection] where CollectionID = " + ID.ToString();
                    this.labelCollectionDetails.Text = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                    this.buttonCollectionDetails.Tag = ID;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonCollectionDetails_Click(object sender, EventArgs e)
        {
            int ID;
            if (this.buttonCollectionDetails.Tag != null && int.TryParse(this.buttonCollectionDetails.Tag.ToString(), out ID))
            {
                DiversityCollection.Forms.FormCollection form = new Forms.FormCollection(ID, null, true);
                //form.TopLevel = false;
                //form.Parent = this;
                form.ShowInTaskbar = true;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Width = this.Width - 20;
                form.Height = this.Height - 20;
                form.ShowDialog();
            }
        }

        #endregion

        #region External credentials
        private void buttonExternalAdd_Click(object sender, EventArgs e)
        {
            //string SQL = "INSERT INTO CollectionManager (LoginName, AdministratingCollectionID) VALUES ('" +
            //    this.listBoxUser.SelectedValue.ToString() + "', " + this.listBoxAdministratingCollection.SelectedValue.ToString() + ")";
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            //con.Open();
            //try
            //{
            //    C.ExecuteNonQuery();
            //}
            //catch { }
            //con.Close();
            //this.setCollectionManager();
        }

        private void buttonExternalRemove_Click(object sender, EventArgs e)
        {
            //string SQL = "DELETE FROM CollectionManager WHERE LoginName = '" +
            //    this.listBoxUser.SelectedValue.ToString() + "' AND CollectionID = " + this.listBoxCollectionManager.SelectedValue.ToString();
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            //con.Open();
            //C.ExecuteNonQuery();
            //con.Close();
            //this.setCollectionManager();
        }

        private void listBoxExternalUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.dataSetTransaction.CollectionManager.Clear();
            //this.setExternalCredentials();
        }


        private void setExternalCredentials()
        {
            //if (this.listBoxUser.SelectedIndex > -1)
            //{
            //    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUser.SelectedItem;
            //    string Login = R["LoginName"].ToString();
            //    string SQL = "SELECT CollectionManager.LoginName, CollectionManager.AdministratingCollectionID, Collection.CollectionName " +
            //        "FROM  CollectionManager INNER JOIN " +
            //        "Collection ON CollectionManager.AdministratingCollectionID = Collection.CollectionID " +
            //        "WHERE (CollectionManager.LoginName = N'" + Login + "') ORDER BY Collection.CollectionName";
            //    if (this._SqlDataAdapterCollectionManager == null)
            //    {
            //        this._SqlDataAdapterCollectionManager = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    }
            //    this._SqlDataAdapterCollectionManager.SelectCommand.CommandText = SQL;
            //    System.Data.DataTable dt = new DataTable();
            //    this._SqlDataAdapterCollectionManager.Fill(dt);
            //    this.listBoxCollectionManager.DataSource = dt;
            //    this.listBoxCollectionManager.DisplayMember = "CollectionName";
            //    this.listBoxCollectionManager.ValueMember = "AdministratingCollectionID";
            //}
            //else
            //    this.listBoxCollectionManager.DataSource = null;
        }

        #endregion

        #region Form
        
        private void FormManagers_FormClosing(object sender, FormClosingEventArgs e)
        {
            DiversityCollection.LookupTable.ResetTransaction();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

    }
}