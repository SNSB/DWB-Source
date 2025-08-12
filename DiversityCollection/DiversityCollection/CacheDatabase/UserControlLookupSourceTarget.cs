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
    public partial class UserControlLookupSourceTarget : UserControl
    {

        private string _PostgresDatabase;
        private string _TargetTable;
        private string _SourceView;
        
        public UserControlLookupSourceTarget(string PostgresDatabase, string TargetTable, string SourceView)
        {
            InitializeComponent();
            this._PostgresDatabase = PostgresDatabase;
            this._TargetTable = TargetTable;
            this._SourceView = SourceView;
            this.initControl();
        }

        private void initControl()
        {
            string SQL = "SELECT IncludeInTransfer, TransferProtocol, TransferErrors " +
                " FROM " + this._TargetTable +
                " WHERE SourceView = '" + this._SourceView + "' AND Target = N'" + this._PostgresDatabase + "'";
            string Message = "";
            System.Data.DataTable _dtTarget = new DataTable();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref _dtTarget, ref Message);
            if (_dtTarget.Rows.Count > 0)
            {
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(16);
                this.labelTarget.Text = this._PostgresDatabase;
                if (!_dtTarget.Rows[0]["TransferProtocol"].Equals(System.DBNull.Value) && _dtTarget.Rows[0]["TransferProtocol"].ToString().Length > 0)
                    this.buttonTransferProtocol.Enabled = true;
                if (!_dtTarget.Rows[0]["TransferErrors"].Equals(System.DBNull.Value) && _dtTarget.Rows[0]["TransferErrors"].ToString().Length > 0)
                    this.buttonTransferErrors.Enabled = true;
                if (!_dtTarget.Rows[0]["IncludeInTransfer"].Equals(System.DBNull.Value) && _dtTarget.Rows[0]["IncludeInTransfer"].ToString() == "True")
                    this.checkBoxIncludeInTransfer.Checked = true;
                else this.checkBoxIncludeInTransfer.Checked = false;
                if (DiversityWorkbench.PostgreSQL.Connection.Databases().ContainsKey(this._PostgresDatabase))
                    this.labelTarget.ForeColor = System.Drawing.Color.Black;
            }
            else
                this.Height = 0;
        }

    }
}
