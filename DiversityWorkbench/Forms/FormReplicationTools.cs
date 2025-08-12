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
    public partial class FormReplicationTools : Form
    {

        System.Collections.Generic.List<string> _TablesForRowGUIDSync;
        private DiversityWorkbench.Data.Table _Table_RowGUIDSync;
        private System.Data.DataTable _DtSubscriber;

        public FormReplicationTools(System.Collections.Generic.List<string> TablesForRowGUIDSync)
        {
            InitializeComponent();
            this._TablesForRowGUIDSync = TablesForRowGUIDSync;
            this.initForm();
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void initForm()
        {
            try
            {
                this.comboBoxSyncRowGUIDtable.DataSource = this._TablesForRowGUIDSync;
                this.dataGridViewSyncRowGUID.RowHeadersVisible = false;
                this.labelPublisher.Text = DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.Database + " on " + DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.DataSource;
                this.labelSubscriber.Text = DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber.Database + " on " + DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber.DataSource;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void comboBoxSyncRowGUIDtable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.SearchForRowGUIDs();
        }

        private void SearchForRowGUIDs()
        {
            this.dataGridViewSyncRowGUID.DataSource = null;
            this._DisplayColumn = null;
            try
            {
                if (this.comboBoxSyncRowGUIDtable.SelectedItem.ToString().Length > 0)
                {
                    this._Table_RowGUIDSync = new Data.Table(this.comboBoxSyncRowGUIDtable.SelectedItem.ToString(), DiversityWorkbench.Settings.ConnectionString);
                    string PK = this._Table_RowGUIDSync.PrimaryKeyColumnList[0];
                    string SQL = "SELECT cast(1 as bit) AS OK, " + PK + ", " + this.DisplayColumn() + ", '' AS [" + this.DisplayColumn() + " of publisher], RowGUID, '' AS [RowGUID of publisher] FROM [" + this._Table_RowGUIDSync.Name + "] ";
                    this._DtSubscriber = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter adSub = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber.ConnectionString);
                    adSub.Fill(this._DtSubscriber);
                    System.Data.DataTable dtPub = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter adPub = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.ConnectionString);
                    adPub.Fill(dtPub);
                    System.Collections.Generic.List<System.Data.DataRow> FittingRows = new List<DataRow>();
                    foreach (System.Data.DataRow R in this._DtSubscriber.Rows)
                    {
                        string PkValue = R[PK].ToString();
                        System.Data.DataRow[] rr = dtPub.Select(PK + " = '" + R[PK].ToString() + "'", "");
                        if (rr.Length == 1)
                        {
                            if (rr[0]["RowGUID"].ToString() != R["RowGUID"].ToString())
                            {
                                R[3] = rr[0][this.DisplayColumn()].ToString();
                                R[5] = rr[0]["RowGUID"].ToString();
                                if (R[2].ToString() != R[3].ToString())
                                    R[0] = false;
                                //R["[RowGUID of publisher]"] = rr[0]["RowGUID"].ToString();
                                //R["[" + this.DisplayColumn() + " of publisher]"] = rr[0][this.DisplayColumn()].ToString();
                            }
                            else
                            {
                                R.Delete();
                            }
                        }
                        else
                            R.Delete();
                    }
                    this._DtSubscriber.AcceptChanges();
                    this.dataGridViewSyncRowGUID.DataSource = this._DtSubscriber;
                    this.dataGridViewSyncRowGUID.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    this.dataGridViewSyncRowGUID.ReadOnly = false;
                    for (int i = 1; i < this.dataGridViewSyncRowGUID.Columns.Count; i++)
                        this.dataGridViewSyncRowGUID.Columns[i].ReadOnly = true;
                }
            }
            catch (System.Exception ex)
            {
            }
            if (this._DtSubscriber.Rows.Count == 0)
                System.Windows.Forms.MessageBox.Show("No differences found");
        }

        private string _DisplayColumn;
        private string DisplayColumn()
        {
            if (this._DisplayColumn == null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> KV in this._Table_RowGUIDSync.Columns)
                {
                    if (KV.Key == "DisplayText")
                    {
                        this._DisplayColumn = KV.Key;
                        break;
                    }
                    if (KV.Key.ToLower().StartsWith(this._Table_RowGUIDSync.Name.ToLower()) && KV.Key.ToLower().EndsWith("name"))
                    {
                        this._DisplayColumn = KV.Key;
                        break;
                    }
                    if (KV.Key.ToLower().StartsWith(this._Table_RowGUIDSync.Name.ToLower()) && KV.Key.ToLower().EndsWith("title"))
                    {
                        this._DisplayColumn = KV.Key;
                        break;
                    }
                    if (KV.Key.ToLower() == this._Table_RowGUIDSync.Name.ToLower())
                    {
                        this._DisplayColumn = KV.Key;
                        break;
                    }
                    if (this._Table_RowGUIDSync.PrimaryKeyColumnList[0].ToLower() == KV.Key.ToLower() + "id")
                    {
                        this._DisplayColumn = KV.Key;
                        break;
                    }
                }
            }
            return this._DisplayColumn;
        }

        private void buttonSyncRowGUIDupdate_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "";
                foreach (System.Data.DataRow R in this._DtSubscriber.Rows)
                {
                    if (R[0].ToString().ToLower() != "true")
                        continue;
                    SQL = "UPDATE T SET T.RowGUID = '" + R["RowGUID of publisher"].ToString()
                        + "' FROM " + this._Table_RowGUIDSync.Name + " AS T "
                        + "WHERE T." + this._Table_RowGUIDSync.PrimaryKeyColumnList[0] + " = '" + R[this._Table_RowGUIDSync.PrimaryKeyColumnList[0]].ToString() + "'";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
                System.Windows.Forms.MessageBox.Show("Update finished");
                this.SearchForRowGUIDs();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Update failed");
            }
        }

    }
}
