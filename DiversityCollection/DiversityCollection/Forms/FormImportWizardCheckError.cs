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
    /// <summary>
    /// Form to decide what is to do if a conflict occurs during the import
    /// Possible scenarios:
    /// INSERT 
    ///     Problem: a dataset where the PK allready exists: 
    ///         CheckError:
    ///             Ignore
    ///             Update
    ///         if NeededAction = NoAction: disable Update - just inform and no entry in the Report
    ///         Automatic:
    ///             Error message, No action
    /// UPDATE 
    ///     Problem: several matching datasets exist:
    ///         CheckError:
    ///             Ignore
    ///             Insert as new dataset (for tables with identity column)
    ///             Update of selected dataset (return Where Clause)
    ///         Automatic:
    /// MERGE
    ///     Problem: there are more than one dataset that could be updated
    ///         CheckError:
    ///             Ignore
    ///             Update: select the dataset that should be updated (see UPDATE - Update)
    ///             Insert: if possible, e.g. for tables with identity column import as new dataset
    ///         Automatic:
    /// </summary>
    public partial class FormImportWizardCheckError : Form
    {

        #region Parameter
        
        private DiversityCollection.Import_Table _ImportTable;
        private string _WhereClause;

        #endregion

        #region Construction

        public FormImportWizardCheckError(
            DiversityCollection.Import_Table Table,
            int Line,
            string Message)
        {
            InitializeComponent();
            this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm";
            try
            {
                this.textBoxTable.Text = Table.TableName;
                if (Table.TableAlias != Table.TableName) 
                    this.textBoxTable.Text += " (" + Table.TableAlias + ")";
                this.textBoxAction.Text = Table.TreatmentOfData.ToString() + ". Line in File: " + Line.ToString();
                this.textBoxProblem.Text = Message;
                //if (Table.ActionNeeded == Import_Table.NeededAction.SelectTarget)
                //    Message += "\r\n\r\nPlease select the dataset that should be updated";
                this._ImportTable = Table;
                //this._ColumnValueDictionary = ColumnValueDictionary;
                this.dataGridView.Columns.Add("Source", "Source");
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Table.ColumnValueDictionary())
                {
                    this.dataGridView.Columns.Add(KV.Key, KV.Key);
                }
                if (Table.IdentityColumn() != null && Table.IdentityColumn().Length > 0)
                {
                    if (!Table.ColumnValueDictionary().ContainsKey(Table.IdentityColumn()))
                        this.dataGridView.Columns.Add(Table.IdentityColumn(), Table.IdentityColumn());
                }
                this.dataGridView.Rows.Add(1);
                this.dataGridView.Rows[0].Cells["Source"].Value = "File";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Table.ColumnValueDictionary())
                {
                    this.dataGridView.Rows[0].Cells[KV.Key].Value = KV.Value;
                }
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT * FROM " + this._ImportTable.TableName + " WHERE " + this._ImportTable.WhereClause();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataColumn C in dt.Columns)
                    {
                        if (!this.dataGridView.Columns.Contains(C.ColumnName))
                            this.dataGridView.Columns.Add(C.ColumnName, C.ColumnName);
                    }

                    this.dataGridView.Rows.Add(1);
                    this.dataGridView.Rows[1].DefaultCellStyle.BackColor = System.Drawing.Color.Black;
                    this.dataGridView.Rows[1].Height = 6;

                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        System.Windows.Forms.DataGridViewRow dgR = new DataGridViewRow();
                        this.dataGridView.Rows.Add(dgR);
                        this.dataGridView.Rows[this.dataGridView.Rows.Count - 1].Cells["Source"].Value = "Database";
                        foreach (System.Data.DataColumn C in dt.Columns)// System.Collections.Generic.KeyValuePair<string, string> KV in Table.ColumnValueDictionary())
                        {
                            this.dataGridView.Rows[this.dataGridView.Rows.Count - 1].Cells[C.ColumnName].Value = R[C.ColumnName].ToString();
                            //dgR.Cells[KV.Key].Value = R[KV.Key].ToString();
                        }
                        if (Table.IdentityColumn() != null && Table.IdentityColumn().Length > 0)
                        {
                            if (!Table.ColumnValueDictionary().ContainsKey(Table.IdentityColumn()))
                                this.dataGridView.Rows[this.dataGridView.Rows.Count - 1].Cells[Table.IdentityColumn()].Value = R[Table.IdentityColumn()].ToString();
                        }
                        this.dataGridView.Rows[this.dataGridView.Rows.Count - 1].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        dgR.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
                this.dataGridView.Columns[0].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                this.dataGridView.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                switch (Table.TreatmentOfData)
                {
                    case Import_Table.DataTreatment.Insert:
                        this.buttonInsert.Enabled = false;
                        if (Table.ActionNeeded == Import_Table.NeededAction.NoAction)
                            this.buttonUpdate.Enabled = false;
                        break;
                    case Import_Table.DataTreatment.Merge:
                        break;
                    case Import_Table.DataTreatment.Update:
                        if (dt.Rows.Count > 0)
                            this.buttonInsert.Enabled = false;
                        if (dt.Rows.Count == 0) 
                            this.buttonUpdate.Enabled = false;
                        break;
                }
                switch (Table.ActionNeeded)
                {
                    case Import_Table.NeededAction.SelectTarget:
                        this.buttonUpdate.Enabled = false;
                        break;
                    case Import_Table.NeededAction.Update:
                        this.buttonUpdate.Enabled = true;
                        break;
                }
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region Events
        
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ImportTable.ActionNeeded = Import_Table.NeededAction.Insert;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ImportTable.ActionNeeded = Import_Table.NeededAction.Update;
        }

        private void buttonIgnore_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this._ImportTable.ActionNeeded = Import_Table.NeededAction.NoAction;
        }

        private void dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count == 1 &&
                this.dataGridView.SelectedRows[0].Index > 1 &&
            (
                this._ImportTable.ActionNeeded == Import_Table.NeededAction.SelectTarget
                || this._ImportTable.ActionNeeded == Import_Table.NeededAction.Update))
            {
                this._WhereClause = "";
                foreach (string s in this._ImportTable.ColumnList)
                {
                    if (this._ImportTable.ColumnValueDictionary().ContainsKey(s) &&
                        this.dataGridView.Columns.Contains(s) &&
                        this._ImportTable.PrimaryKeyColumnList.Contains(s))
                    {
                        if (this._WhereClause.Length > 0) this._WhereClause += " AND ";
                        this._WhereClause += s + " = '" + this.dataGridView.SelectedRows[0].Cells[s].Value.ToString() + "'";
                        //this._ImportTable.ColumnValueDictionary()[s] = this.dataGridView.SelectedRows[0].Cells[s].Value.ToString();
                    }
                }
                if (this._WhereClause.Length > 0) 
                    this._WhereClause = " " + this._WhereClause;
                if (this._ImportTable.IdentityColumn() != null && this._ImportTable.IdentityColumn().Length > 0 && this._WhereClause.IndexOf(" " + this._ImportTable.IdentityColumn() + " = ") == -1)
                {
                    if (!this._ImportTable.ColumnValueDictionary().ContainsKey(this._ImportTable.IdentityColumn()))
                    {
                        this._ImportTable.ColumnValueDictionary().Add(this._ImportTable.IdentityColumn(), this.dataGridView.SelectedRows[0].Cells[this._ImportTable.IdentityColumn()].Value.ToString());
                    }
                    else if (this._ImportTable.ActionNeeded == Import_Table.NeededAction.SelectTarget || this._ImportTable.ActionNeeded == Import_Table.NeededAction.Update)
                    {
                        if (this._WhereClause.Length > 0) this._WhereClause += " AND ";
                        this._WhereClause += this._ImportTable.IdentityColumn() + " = " + this.dataGridView.SelectedRows[0].Cells[this._ImportTable.IdentityColumn()].Value.ToString();
                        //this._ImportTable.ColumnValueDictionary()[this._ImportTable.IdentityColumn()] = this.dataGridView.SelectedRows[0].Cells[this._ImportTable.IdentityColumn()].Value.ToString();
                    }
                }
                if (this._WhereClause.Length > 0)
                    this._ImportTable.WhereClause(this._WhereClause);
                this._ImportTable.CompareAndInsertValues(this._ImportTable.iLine);
                if (this._ImportTable.ActionNeeded == Import_Table.NeededAction.Update)
                    this.buttonUpdate.Enabled = true;
            }
            else
            {
                this.buttonUpdate.Enabled = false;
            }
        }
        
        #endregion

        #region Interface
        
        public string WhereClause()
        { return this._WhereClause; }

        #endregion
    }
}
