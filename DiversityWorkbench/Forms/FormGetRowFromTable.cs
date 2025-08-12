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
    public partial class FormGetRowFromTable : Form
    {
        private System.Data.DataTable _DT;
        private System.Collections.Generic.List<int> _HiddenColumns;
        public FormGetRowFromTable(string Title, string Header, System.Data.DataTable DT, bool AutoresizeColumns = false, System.Collections.Generic.List<int> HiddenColumns = null)
        {
            InitializeComponent();
            if (HiddenColumns != null) _HiddenColumns = HiddenColumns;
            else _HiddenColumns = new List<int>();
            this.Text = Title;
            this.labelHeader.Text = Header;
            this.dataGridView.DataSource = DT;
            this._DT = DT;
            this.initForm(AutoresizeColumns);
        }

        private void initForm(bool AutoresizeColumns)
        {
            for (int i = 0; i < this._DT.Columns.Count; i++)
            {
                if (!_HiddenColumns.Contains(i))
                    this.toolStripComboBoxColumn.Items.Add(this._DT.Columns[i].ColumnName);
            }
            //foreach (System.Data.DataColumn DC in this._DT.Columns)
            //{
            //    if (!_HiddenColumns.Contains(DC))
            //        this.toolStripComboBoxColumn.Items.Add(DC.ColumnName);
            //}
            this.toolStripComboBoxOperator.Items.Add("~");
            this.toolStripComboBoxOperator.Items.Add("=");
            this.toolStripComboBoxOperator.Items.Add(">");
            this.toolStripComboBoxOperator.Items.Add("<");
            //this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            if (AutoresizeColumns)
            {
                for (int i = 0; i < this.dataGridView.Columns.Count; i++)
                {
                    if (_HiddenColumns.Contains(i))
                        this.dataGridView.Columns[i].Visible = false;
                    else
                        this.dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                //foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                //{

                //    C.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //}
            }
        }

        /// <summary>
        /// Setting the index of the column that should be selected in the filter
        /// </summary>
        /// <param name="ColumnIndex"></param>
        public void setFilterColumn(int ColumnIndex)
        {
            this.toolStripComboBoxColumn.SelectedIndex = ColumnIndex;
        }

        /// <summary>
        /// Setting the index of the operator
        /// </summary>
        /// <param name="OperatorIndex">0: ~, 1: =, 2: >, ... </param>
        public void setOperator(int OperatorIndex)
        {
            this.toolStripComboBoxOperator.SelectedIndex = OperatorIndex;
        }

        public System.Data.DataRow SeletedRow()
        {
            if (this.dataGridView.SelectedCells != null && this.dataGridView.SelectedCells.Count > 0)
            {
                return ((System.Data.DataRowView)this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].DataBoundItem).Row;
            }
            else return null;
        }

        public System.Collections.Generic.List<System.Data.DataRow> SeletedRows()
        {
            System.Collections.Generic.List<System.Data.DataRow> RR = new List<DataRow>();
            if (this.dataGridView.SelectedRows != null && this.dataGridView.SelectedRows.Count > 0)
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.dataGridView.Rows[R.Index].DataBoundItem;
                    if (!RR.Contains(RV.Row))
                        RR.Add(RV.Row);
                }
            }
            else if (this.dataGridView.SelectedCells != null && this.dataGridView.SelectedCells.Count > 0)
            {
                foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.dataGridView.Rows[C.RowIndex].DataBoundItem;
                    if (!RR.Contains(RV.Row))
                        RR.Add(RV.Row);
                }
            }
            return RR;
        }

        private void toolStripButtonFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string Filter = this.toolStripComboBoxColumn.SelectedItem.ToString();
                switch (this.toolStripComboBoxOperator.SelectedItem.ToString())
                {
                    case "=":
                        Filter += " = '" + this.toolStripTextBoxValue.Text + "'";
                        break;
                    case "~":
                        Filter += " LIKE '" + this.toolStripTextBoxValue.Text + "%'";
                        break;
                    case ">":
                        Filter += " > '" + this.toolStripTextBoxValue.Text + "'";
                        break;
                    case "<":
                        Filter += " < '" + this.toolStripTextBoxValue.Text + "'";
                        break;
                }
                System.Data.DataView DV = new DataView(this._DT, Filter, "", DataViewRowState.Unchanged);
                this.dataGridView.DataSource = DV;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonRemoveFilter_Click(object sender, EventArgs e)
        {
            this.dataGridView.DataSource = this._DT;
        }

        #region Details

        private string _DetailLinkColumn = "";
        private IWorkbenchUnit _WorkbenchUnit;

        public void SetDetailLinkColumn(string ColumnName, IWorkbenchUnit workbenchUnit)
        {
            _DetailLinkColumn = ColumnName;
            _WorkbenchUnit = workbenchUnit;
            this.toolStripButtonDetails.Visible = true;
        }
        private void toolStripButtonDetails_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count == 1)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.dataGridView.Rows[this.dataGridView.SelectedRows[0].Index].DataBoundItem;
                DiversityWorkbench.Forms.FormRemoteQuery f = new FormRemoteQuery(RV[_DetailLinkColumn].ToString(), _WorkbenchUnit);
                f.ShowDialog();
            }
            else System.Windows.Forms.MessageBox.Show("Please select a valid row");
        }

        #endregion

    }
}
