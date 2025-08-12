using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class UserControlTable : UserControl
    {

        #region Parameter

        private string _TableName;
        private int _ProjectID;
        
        #endregion

        #region Construction

        public UserControlTable(string TableName, int ProjectID)
        {
            InitializeComponent();
            this._TableName = TableName;
            this._ProjectID = ProjectID;
        }
        
        #endregion

        #region Interface

        public void RequeyContent()
        {
            this.dataGridViewSource.DataSource = DiversityCollection.CacheDatabase.CacheDB.DataTable(this._TableName, this._ProjectID, true);
            this.labelSource.Text = "Source (" + this.dataGridViewSource.Rows.Count + ")";
            this.dataGridViewCache.DataSource = DiversityCollection.CacheDatabase.CacheDB.DataTable(this._TableName, this._ProjectID, false);
            this.labelCache.Text = "Cache content (" + this.dataGridViewCache.Rows.Count + ")";
            System.Windows.Forms.TabPage TP = (System.Windows.Forms.TabPage)this.Parent;
            System.Windows.Forms.TabControl TC = (System.Windows.Forms.TabControl)TP.Parent;
            if (TC != null)
            {
                if (this.dataGridViewCache.Rows.Count == 0 && this.dataGridViewSource.Rows.Count == 0)
                {
                    if (TC.TabPages.Contains(TP))
                        TC.TabPages.Remove(TP);
                }
                else if (!TC.TabPages.Contains(TP))
                    TC.TabPages.Add(TP);
            }
        }

        public void ClearCacheData()
        {
            bool OK = true;
            try
            {
                this.progressBar.Value = 0;
                this.progressBar.Maximum = this.dataGridViewSource.Rows.Count;
                string SQL = "DELETE T FROM " + this._TableName + " AS T WHERE T.ProjectID = " + this._ProjectID.ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    this.dataGridViewCache.DataSource = DiversityCollection.CacheDatabase.CacheDB.DataTable(this._TableName, this._ProjectID, false);
                    this.labelCache.Text = "Cache content (" + this.dataGridViewCache.Rows.Count + ")";
                }
                else OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
        }

        public void ExportData()
        {
            string Message = "";
            bool OK = true;
            try
            {
                this.progressBar.Value = 0;
                this.progressBar.Maximum = this.dataGridViewSource.Rows.Count;
                string SQL = "DELETE T FROM " + this._TableName + " AS T WHERE T.ProjectID = " + this._ProjectID.ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    this.dataGridViewCache.DataSource = null;
                    System.Data.DataTable dtSource = (System.Data.DataTable)this.dataGridViewSource.DataSource;
                    //System.Data.DataTable dtSource = DiversityCollection.CacheDatabase.CacheDB.DataTable(this._TableName, this._ProjectID, true);
                    foreach (System.Data.DataRow R in dtSource.Rows)
                    {
                        SQL = "ProjectID ";
                        System.Collections.Generic.Dictionary<string, string> Columns = DiversityCollection.CacheDatabase.CacheDB.CollectionTableColumns(this._TableName);
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Columns)
                        {
                            SQL += ", " + KV.Key;
                        }

                        SQL = "INSERT INTO " + this._TableName + " (" + SQL + ") VALUES (" + this._ProjectID.ToString();
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            SQL += ", ";
                            if (R[C.ColumnName].Equals(System.DBNull.Value))
                                SQL += "NULL";
                            else
                            {
                                if (Columns[C.ColumnName] == "datetime"
                                    || Columns[C.ColumnName] == "smalldatetime")
                                    SQL += "CONVERT(DATETIME, '" + R[C.ColumnName].ToString() + "', 102)";
                                else
                                    SQL += "'" + R[C.ColumnName].ToString().Replace("'", "''") + "'";
                            }
                        }
                        SQL += ")";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        {
                            Message += "Failed insert:\r\n" + SQL + "\r\n";
                        }
                        if (this.progressBar.Value > this.progressBar.Maximum)
                            this.progressBar.Value++;
                    }
                    this.dataGridViewCache.DataSource = DiversityCollection.CacheDatabase.CacheDB.DataTable(this._TableName, this._ProjectID, false);
                    this.labelCache.Text = "Cache content (" + this.dataGridViewCache.Rows.Count + ")";
                }
                else OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
        }

        #endregion

    }
}
