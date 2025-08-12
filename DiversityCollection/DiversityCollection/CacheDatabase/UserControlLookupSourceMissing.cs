using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class UserControlLookupSourceMissing : UserControl
    {
        private string _TargetTable;
        private string _SourceView;

        public UserControlLookupSourceMissing(string TargetTable, string SourceView)
        {
            InitializeComponent();
            this._TargetTable = TargetTable;
            this._SourceView = SourceView;
            this.labelMissingSource.Text = SourceView;
            this.SetCount();
        }

        private void buttonRemoveSource_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "DELETE " +
                    " FROM \"" + this._TargetTable + "\"" +
                    " WHERE \"SourceView\" = '" + this._SourceView + "'";
                if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))
                {
                    System.Windows.Forms.MessageBox.Show("Data removed");
                    this.SetCount();
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetCount()
        {
            try
            {
                string SQL = "SELECT COUNT(*) " +
                    " FROM \"" + this._TargetTable + "\"" +
                    " WHERE \"SourceView\" = '" + this._SourceView + "' ";
                this.labelCount.Text = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                if (this.labelCount.Text == "0")
                    this.buttonRemoveSource.Enabled = false;
                else
                    this.buttonRemoveSource.Enabled = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setHeader(string Header)
        {
            this.labelHeader.Text = Header;
        }

    }
}
