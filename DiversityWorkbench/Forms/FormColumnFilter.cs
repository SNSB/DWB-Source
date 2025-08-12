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
    public partial class FormColumnFilter : Form
    {
        private System.Data.DataTable _DataTable;
        private string _ConnectionString;

        public FormColumnFilter(System.Data.DataTable Table, string HeaderInfo, string ConnectionString)
        {
            InitializeComponent();
            this.labelHeader.Text = HeaderInfo;
            this._DataTable = Table;
            this._ConnectionString = ConnectionString;
            foreach (System.Data.DataColumn C in this._DataTable.Columns)
            {
                if (C.DataType.FullName == "System.Guid")
                    continue;
                DiversityWorkbench.UserControls.UserControlColumnFilter U = new UserControls.UserControlColumnFilter(C);
                this.Height += U.Height;
                this.panelColumns.Controls.Add(U);
                U.Dock = DockStyle.Top;
                U.BringToFront();
            }
        }
        
        public FormColumnFilter(System.Data.DataTable Table)
        {
            InitializeComponent();
            if (Table != null)
            {
                this._DataTable = Table;
                foreach (System.Data.DataColumn C in this._DataTable.Columns)
                {
                    DiversityWorkbench.UserControls.UserControlColumnFilter U = new UserControls.UserControlColumnFilter(C);
                    this.Height += U.Height;
                    this.panelColumns.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
            }
        }

        public string Filter()
        {
            string ColumnFilter = "";
            foreach (System.Windows.Forms.Control C in this.panelColumns.Controls)
            {
                DiversityWorkbench.UserControls.UserControlColumnFilter CF = (DiversityWorkbench.UserControls.UserControlColumnFilter)C;
                if (CF.Filter().Length == 0)
                    continue;
                if (ColumnFilter.Length > 0) 
                    ColumnFilter += " AND ";
                ColumnFilter += CF.Filter();
            }
            return ColumnFilter;
        }

        public System.Collections.Generic.Dictionary<string, ReplicationFilter> Filters()
        {
            System.Collections.Generic.Dictionary<string, ReplicationFilter> filters = new Dictionary<string, ReplicationFilter>();
            foreach (System.Windows.Forms.Control C in this.panelColumns.Controls)
            {
                DiversityWorkbench.UserControls.UserControlColumnFilter CF = (DiversityWorkbench.UserControls.UserControlColumnFilter)C;
                if (CF.Filter().Length == 0)
                    continue;
                if (CF.FilterSQL.Length > 0)
                {
                    ReplicationFilter filter = new ReplicationFilter(CF.FilterOperator, CF.FilterValue, CF.FilterSQL);
                    filters.Add(CF.ColumnName, filter);
                }
            }
            return filters;
        }

        private void buttonShowData_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "  SELECT * FROM " + this._DataTable.TableName;
                if (this.Filter().Length > 0)
                    SQL += " WHERE " + this.Filter();
                System.Data.DataTable dt = new DataTable();
                if (this._ConnectionString.Length == 0)
                    this._ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormTableContent f = new FormTableContent(this._DataTable.TableName, "Filtered content of " + this._DataTable.TableName, dt);
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonShowFilter_Click(object sender, EventArgs e)
        {
            string Filter = this.Filter();
            if (Filter.Length > 0)
                System.Windows.Forms.MessageBox.Show(this.Filter(), "Filter");
            else
                System.Windows.Forms.MessageBox.Show("No filter established so far", "Filter");
        }

    }
}
