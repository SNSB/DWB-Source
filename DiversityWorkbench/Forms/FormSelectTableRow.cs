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
    public partial class FormSelectTableRow : Form
    {
        #region Parameter

        private System.Data.DataTable _DataTable;
        private System.Data.DataRow _DataRow;

        #endregion

        #region Construction

        public FormSelectTableRow(System.Data.DataTable DT, string Header)
        {
            InitializeComponent();
            this.labelHeader.Text = Header;
            this._DataTable = DT;
            this.dataGridView.DataSource = this._DataTable;
            this.dataGridView.AutoResizeColumns();
            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                C.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.userControlDialogPanel.buttonOK.Enabled = false;
        }

        public FormSelectTableRow(System.Data.DataTable DT, string Header, int DecisiveColumn, string Result, System.Drawing.Color ColorTrue, System.Drawing.Color ColorFalse)
            : this(DT, Header)
        {
            //InitializeComponent();
            //this.labelHeader.Text = Header;
            //this._DataTable = DT;
            //this.dataGridView.DataSource = this._DataTable;
            //this.dataGridView.AutoResizeColumns();
            //this.userControlDialogPanel.buttonOK.Enabled = false;
            this.MarkResults(DecisiveColumn, Result, ColorTrue, ColorFalse);
        }

        //private void initForm(System.Data.DataTable DT, string Header)
        //{
        //    this.labelHeader.Text = Header;
        //    this._DataTable = DT;
        //    this.dataGridView.DataSource = this._DataTable;
        //    this.dataGridView.AutoResizeColumns();
        //    this.userControlDialogPanel.buttonOK.Enabled = false;
        //}

        #endregion

        #region Functions etc.

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        public System.Data.DataRow SelectedRow
        {
            get
            {
                return this._DataRow;
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.userControlDialogPanel.buttonOK.Enabled = true;
            int i = this.dataGridView.SelectedCells[0].RowIndex;
            this._DataRow = this._DataTable.Rows[i];
        }

        private void MarkResults(int DecisiveColumn, string Result, System.Drawing.Color ColorTrue, System.Drawing.Color ColorFalse)
        {
            try
            {
                DataGridViewCellStyle StyleTrue = new DataGridViewCellStyle();
                StyleTrue.BackColor = ColorTrue;
                DataGridViewCellStyle StyleFalse = new DataGridViewCellStyle();
                StyleFalse.BackColor = ColorFalse;
                StyleFalse.SelectionBackColor = ColorFalse;

                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                {
                    for (int iDGV = 0; iDGV < this.dataGridView.Columns.Count; iDGV++)
                    {
                        if (R.Cells[DecisiveColumn].Value.ToString() == Result)
                            R.Cells[iDGV].Style.BackColor = ColorTrue;
                        else
                        {
                            R.Cells[iDGV].Style = StyleFalse;
                            R.Cells[iDGV].Style.BackColor = ColorFalse;
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion


    }
}
