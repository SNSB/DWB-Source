using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class FormFileDataGrid : Form
    {

        #region Parameter

        private System.Windows.Forms.DataGridView _Grid;

        #endregion

        #region Construction and Form

        public FormFileDataGrid(System.Windows.Forms.DataGridView DataGrid, DiversityWorkbench.Import.DataColumn DC, int? ScrollToColumn)
        {
            InitializeComponent();
            try
            {
                int iFirstColumn = 0;
                if (DataGrid.SelectedCells.Count > 0)
                    iFirstColumn = DataGrid.SelectedCells[0].ColumnIndex;
                if (DC.FileColumn != null)
                    iFirstColumn = (int)DC.FileColumn;
                if (ScrollToColumn != null)
                    iFirstColumn = (int)ScrollToColumn;
                this._Grid = DataGrid;
                if (this._Grid != null)
                    this._Grid.CellDoubleClick += this.panelGrid_DoubleClick;
                this._Grid.Dock = DockStyle.Fill;
                this._Grid.ReadOnly = true;
                this._Grid.AllowUserToAddRows = false;
                this._Grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                this._Grid.FirstDisplayedScrollingColumnIndex = iFirstColumn;
                this._Grid.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                this._Grid.Columns[iFirstColumn].Selected = true;
                this.StartPosition = FormStartPosition.CenterParent;
                this.labelHeader.Text = "Select the column in the file for column " + DC.DisplayText + " in table " + DC.DataTable.GetDisplayText();
                System.Collections.Generic.List<int> MultiColumns = new List<int>();
                if (DC.MultiColumns != null)
                {
                    foreach (DiversityWorkbench.Import.ColumnMulti CM in DC.MultiColumns)
                    {
                        MultiColumns.Add(CM.ColumnInFile);
                    }
                }
                this.setColumnColors(DC.FileColumn, MultiColumns);
                this.panelGrid.Controls.Add(this._Grid);
            }
            catch (System.Exception ex)
            {
            }
        }

        public FormFileDataGrid(System.Windows.Forms.DataGridView DataGrid, int? ScrollToColumn)
        {
            InitializeComponent();
            try
            {
                int iFirstColumn = 0;
                if (DataGrid.SelectedCells.Count > 0)
                    iFirstColumn = DataGrid.SelectedCells[0].ColumnIndex;
                if (ScrollToColumn != null)
                    iFirstColumn = (int)ScrollToColumn;
                this._Grid = DataGrid;
                if (this._Grid != null)
                    this._Grid.CellDoubleClick += this.panelGrid_DoubleClick;
                this._Grid.Dock = DockStyle.Fill;
                this._Grid.ReadOnly = true;
                this._Grid.AllowUserToAddRows = false;
                this._Grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                this._Grid.FirstDisplayedScrollingColumnIndex = iFirstColumn;
                this._Grid.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                this._Grid.Columns[iFirstColumn].Selected = true;
                this.StartPosition = FormStartPosition.CenterParent;
                this.labelHeader.Text = "Select the column in the file";
                System.Collections.Generic.List<int> MultiColumns = new List<int>();
                this.panelGrid.Controls.Add(this._Grid);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setColumnColors(int? FileColumn, System.Collections.Generic.List<int> MultiColumns)
        {
            int iPosition = 2;
            for (int i = 0; i < this._Grid.Columns.Count; i++)
            {
                if (MultiColumns.Contains(i))
                {
                    this._Grid.Columns[i].HeaderText = iPosition.ToString();
                    iPosition++;
                }
                if (FileColumn != null && FileColumn == i)
                {
                    this._Grid.Columns[i].HeaderText = "1";
                }
                for (int iLine = 0; iLine < this._Grid.Rows.Count; iLine++)
                {
                    if (iLine + 1 < DiversityWorkbench.Import.Import.StartLine ||
                        iLine + 1 > DiversityWorkbench.Import.Import.EndLine)
                        this._Grid.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.SystemColors.Control;
                    else if (FileColumn != null && FileColumn == i)
                        this._Grid.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.Color.LightCyan;
                    else if (MultiColumns.Contains(i))
                        this._Grid.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.Color.LightGreen;
                    else
                        this._Grid.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.SystemColors.Window;
                }
            }
            // Toni 20201029: If a column was already selected, set grid to preset 
            if (FileColumn != null && FileColumn < this._Grid.ColumnCount)
            {
                this._Grid.ClearSelection();
                this._Grid.Columns[(int)FileColumn].Selected = true;
            }
        }

        private void panelGrid_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region Interface

        public int? SelectedFileColumn()
        {
            if (this._Grid.SelectedCells != null)
                return this._Grid.SelectedCells[0].ColumnIndex;
            else return null;
        }

        #endregion
    }

}
