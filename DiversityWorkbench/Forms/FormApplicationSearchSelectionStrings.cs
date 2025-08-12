using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormApplicationSearchSelectionStrings : Form
    {
        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _ad;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterUserProxy;

        private Microsoft.Data.SqlClient.SqlDataAdapter _adTest;
        private System.Data.DataTable _dtTest;
        private Microsoft.Data.SqlClient.SqlConnection _con;
        private Microsoft.Data.SqlClient.SqlCommand _C;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private string _ItemTable;
        private string _SqlInitialQuery;
        private System.Data.DataTable _dtUser;
        
        #endregion

        #region Construction and interface

        public FormApplicationSearchSelectionStrings(string PathHelpProvider)
        {
            InitializeComponent();
            // online manual
            this.helpProvider.HelpNamespace = PathHelpProvider;
            this.helpProvider.SetHelpKeyword(this.splitContainerMain, "Predefined queries");
            this.helpProvider.SetHelpNavigator(this.splitContainerMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetShowHelp(this.splitContainerMain, true);
            this.initForm();
        }

        public void SetDefaults(string ItemTable, string InitialQuery)
        {
            this._ItemTable = ItemTable;
            this._SqlInitialQuery = InitialQuery;
        }
        
        #endregion

        #region Form
        
        private void initForm()
        {
            try
            {
                bool ForAdmin;
                string TableName = "ApplicationSearchSelectionStrings";
                ForAdmin = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                string SQL = "";
                // Lookup table for user
                SQL = "SELECT ID AS LoginName, CASE WHEN CombinedNameCache IS NULL THEN LoginName ELSE CombinedNameCache END AS CombinedNameCache " +
                    "FROM UserProxy ORDER BY CASE WHEN CombinedNameCache IS NULL THEN LoginName ELSE CombinedNameCache END";
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterUserProxy, this.dataSetApplication.UserProxy, SQL, DiversityWorkbench.Settings.ConnectionString);
                // the queries
                if (ForAdmin)
                {
                    SQL = "SELECT UserName, SQLStringIdentifier, ItemTable, SQLString, Description " +
                       "FROM ApplicationSearchSelectionStrings " +
                       "ORDER BY SQLStringIdentifier, UserName";
                }
                else
                {
                    TableName = "ApplicationSearchSelectionStrings_Core";
                    bool OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                    if (OK)
                    {
                        SQL = "SELECT USER_NAME() AS UserName, SQLStringIdentifier, ItemTable, SQLString, Description " +
                           "FROM ApplicationSearchSelectionStrings_Core " +
                           "ORDER BY SQLStringIdentifier, UserName";
                        this.dataGridViewTable.Columns[0].Visible = false;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("You have not the necessary rights to edit the queries");
                        this.Close();
                    }
                }
                this._ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions f = new FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                f.initSqlAdapter(ref this._ad, SQL, this.dataSetApplication.ApplicationSearchSelectionStrings);

                this._adTest = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT 1", DiversityWorkbench.Settings.ConnectionString);
                this._dtTest = new DataTable();
                this.dataGridViewQuery.DataSource = this._dtTest;
                this._con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                this._C = new Microsoft.Data.SqlClient.SqlCommand("", _con);
            }
            catch { }
        }

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void FormApplicationSearchSelectionStrings_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool Save = false;
            if (this.DialogResult == DialogResult.OK)
                Save = true;
            else
            {
                if (this.dataSetApplication.HasChanges())
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to save the changes", "Save changes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        Save = true;
                }
            }
            if (Save)
            {
                this.SaveData();
            }
        }

        private void FormApplicationSearchSelectionStrings_Load(object sender, EventArgs e)
        {
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetApplication.UserProxy". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.userProxyTableAdapter.Fill(this.dataSetApplication.UserProxy);
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }
        
        #endregion

        #region Grid

        private void dataGridViewTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                System.Windows.Forms.DataGridViewRow r = this.dataGridViewTable.Rows[e.RowIndex];
                this.labelTestCount.Text = "SELECT COUNT(*) FROM " + r.Cells[2].Value.ToString();
                this.labelTestQuery.Text = "SELECT * FROM " + r.Cells[2].Value.ToString();
                this.textBoxSQL.Text = r.Cells[3].Value.ToString();
                this.textBoxDescription.Text = r.Cells[4].Value.ToString();
                this._dtTest.Clear();
                this.textBoxTestCount.Text = "";
            }
            catch { }
        }
        
        private void dataGridViewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (this.dataGridViewTable.Rows[e.RowIndex].Cells[0].Value.ToString().Length == 0)
            {
                string UserName = "";
                string SQL = "SELECT USER_NAME()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    con.Open();
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    UserName = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                    this.dataGridViewTable.Rows[e.RowIndex].Cells[0].Value = UserName;
                }
                catch { }
            }
            //System.Data.DataRow R = this.dataSetApplication.ApplicationSearchSelectionStrings.Rows[e.RowIndex];
            //if (R["UserName"].Equals(System.DBNull.Value))
            //{
            //}
        }

        #endregion

        #region Test and save

        private void buttonTestCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.WhereClauseIsValid(this.textBoxSQL.Text))
                {
                    return;
                }

                this.textBoxTestCount.Text = "";
                this._C.CommandText = this.labelTestCount.Text + " " + this.textBoxSQL.Text;
                _con.Open();
                this.textBoxTestCount.Text = _C.ExecuteScalar()?.ToString();
                _con.Close();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            catch { }
        }

        private void buttonTestQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.WhereClauseIsValid(this.textBoxSQL.Text))
                {
                    return;
                }

                this._dtTest.Clear();
                this._adTest.SelectCommand.CommandText = this.labelTestQuery.Text + " " + this.textBoxSQL.Text;
                this._adTest.Fill(this._dtTest);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonSaveSQL_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.WhereClauseIsValid(this.textBoxSQL.Text))
                {
                    return;
                }

                if (dataGridViewTable.SelectedCells.Count > 0)
                {
                    int i = this.dataGridViewTable.SelectedCells[0].RowIndex;
                    this.dataSetApplication.ApplicationSearchSelectionStrings.Rows[i]["SQLString"] = this.textBoxSQL.Text;
                    this.dataSetApplication.ApplicationSearchSelectionStrings.Rows[i]["Description"] = this.textBoxDescription.Text;
                }
            }
            catch { }
            this.buttonSaveSQL.BackColor = System.Drawing.SystemColors.Control;

        }

        private void textBoxSQL_KeyUp(object sender, KeyEventArgs e)
        {
            this.buttonSaveSQL.BackColor = System.Drawing.Color.Red;
        }

        private void textBoxDescription_KeyUp(object sender, KeyEventArgs e)
        {
            this.buttonSaveSQL.BackColor = System.Drawing.Color.Red;
        }

        private void SaveData()
        {
            try
            {
                if (!this.WhereClauseIsValid(this.textBoxSQL.Text))
                {
                    return;
                }
                DiversityWorkbench.Forms.FormFunctions f = new FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                f.updateTable(this.dataSetApplication, "ApplicationSearchSelectionStrings", this._ad, this.BindingContext);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSaveAll_Click(object sender, EventArgs e)
        {
            this.SaveData();
            this.initForm();
        }

        private void textBoxSQL_TextChanged(object sender, EventArgs e)
        {
            if (this.WhereClauseIsValid(this.textBoxSQL.Text))
                this.textBoxSQL.BackColor = System.Drawing.SystemColors.Window;
            else
                this.textBoxSQL.BackColor = System.Drawing.Color.Pink;
        }

        private bool WhereClauseIsValid(string WhereClause, bool ShowMessage = true)
        {
            if (WhereClause.ToLower().Trim().StartsWith("where ") || WhereClause.ToLower().Trim().Length == 0)
                return true;
            else
            {
                System.Windows.Forms.MessageBox.Show("The WHERE clause is not valid");
                return false;
            }
        }

        #endregion

        #region New queries, Copy, Delete

        private void buttonNewEntry_Click(object sender, EventArgs e)
        {
            this.InsertQuery("", this._SqlInitialQuery, "");
        }

        private void buttonCopyQuery_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewTable.SelectedRows.Count == 1)
            {
                System.Data.DataRow R = this.dataSetApplication.ApplicationSearchSelectionStrings.Rows[this.dataGridViewTable.SelectedRows[0].Index];
                this.InsertQuery(R["SQLStringIdentifier"].ToString(), R["SQLString"].ToString(), R["Description"].ToString());
            }
            else
                System.Windows.Forms.MessageBox.Show("Please select the query that should be copied");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewTable.SelectedRows.Count == 1)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure that you want to delete the selected query", "Delete?", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!this.WhereClauseIsValid(this.textBoxSQL.Text, false))
                        this.textBoxSQL.Text = "";
                    System.Data.DataRow R = this.dataSetApplication.ApplicationSearchSelectionStrings.Rows[this.dataGridViewTable.SelectedRows[0].Index];
                    R.Delete();
                    this.SaveData();
                    this.initForm();
                }
            }
        }

        private void InsertQuery(string Title, string Query, string Description = "")
        {
            try
            {
                if (Title.Length == 0)
                {
                    DiversityWorkbench.Forms.FormGetString f = new FormGetString("New query", "Please enter the title for the new query", "");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        Title = f.String;
                }
                DiversityWorkbench.Forms.FormGetStringFromList fU = new DiversityWorkbench.Forms.FormGetStringFromList(dtUser(), "UserLogin", "ID", "Login", "Please select the user");
                fU.ShowDialog();
                if (fU.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    DiversityWorkbench.Datasets.DataSetApplication.ApplicationSearchSelectionStringsRow R = this.dataSetApplication.ApplicationSearchSelectionStrings.NewApplicationSearchSelectionStringsRow();
                    R.UserName = fU.SelectedValue;
                    R.SQLStringIdentifier = Title;
                    R.SQLString = Query;
                    R.Description = Description;
                    R.ItemTable = this._ItemTable;
                    this.dataSetApplication.ApplicationSearchSelectionStrings.Rows.Add(R);
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable dtUser()
        {
            if (this._dtUser == null)
            {
                this._dtUser = new DataTable();
                string Message = "";
                string SQL = "SELECT ID, " +
                    "LoginName + CASE WHEN [CombinedNameCache] <> '' AND [CombinedNameCache] <> [LoginName] THEN '(= ' + [CombinedNameCache] + ')' ELSE '' END AS UserLogin " +
                    "FROM UserProxy " +
                    "ORDER BY UserLogin ";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtUser, ref Message);
            }
            return this._dtUser;
        }


        #endregion

    }
}