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
    public partial class FormTableContent : Form
    {
        #region Parameter

        private Dictionary<int, string> _DataErrorCols;
        private System.Data.DataTable _DT;

        #endregion

        #region Construction
        public FormTableContent(string Title, string Header, System.Data.DataTable DT, bool AllowExport = false)
        {
            InitializeComponent();
            try
            {
                this._DT = DT;
                this.dataGridView.DataSource = _DT;
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                {
                    C.ReadOnly = true;
                    C.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                this.dataGridView.AutoResizeColumns();
                this.Height = 140 + this.dataGridView.Rows.Count * 13;
                this.Width = 250 + this.dataGridView.ColumnCount * 50;
                this.Text = Title;
                this.labelHeader.Text = Header;
                this.buttonExport.Visible = AllowExport;
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region public interface
        public void setIcon(System.Drawing.Image Image)
        {
            this.Icon = System.Drawing.Icon.FromHandle(((Bitmap)Image).GetHicon());
        }

        public void RowHeaderVisible(bool IsVisible)
        {
            this.dataGridView.RowHeadersVisible = IsVisible;
        }

        public void SetWidth(int Width)
        {
            this.Width = Width;
        }

        public System.Data.DataTable DataTable() { return _DT; }

        public void AllowEdit(bool EditContent, bool AddRows, bool DeleteRows)
        {
            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
            {
                C.ReadOnly = !EditContent;
            }
            this.dataGridView.AllowUserToAddRows = AddRows;
            this.dataGridView.AllowUserToResizeColumns = EditContent;
            this.dataGridView.AllowUserToDeleteRows = DeleteRows;
            this.dataGridView.ReadOnly = !EditContent;
        }

        public void setDataGridViewAutoSizeColumnMode(DataGridViewAutoSizeColumnMode Mode)
        {
            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
            {
                C.ReadOnly = true;
                C.AutoSizeMode = Mode;
            }
            this.dataGridView.AutoResizeColumns();
        }

        public void setDataGridColumnForeColor(int Column, System.Drawing.Color Color)
        {
            this.dataGridView.Columns[Column].DefaultCellStyle.ForeColor = Color;
        }

        public void setDataGridColumnFont(int Column, System.Drawing.Font Font)
        {
            this.dataGridView.Columns[Column].DefaultCellStyle.Font = Font;
        }

        #endregion

        #region Errorhandling
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (_DataErrorCols == null)
                _DataErrorCols = new Dictionary<int, string>();

            if (!_DataErrorCols.ContainsKey(e.ColumnIndex))
            {
                _DataErrorCols.Add(e.ColumnIndex, e.Exception.Message + "\r\n" + e.Exception.StackTrace);
                this.buttonDataErrors.Visible = true;
            }
        }

        private void buttonDataErrors_Click(object sender, EventArgs e)
        {
            string message = "";
            foreach (KeyValuePair<int, string> item in _DataErrorCols)
                message += string.Format("Column {1}, '{0}':\r\n{2}\r\n\r\n", this.dataGridView.Columns[item.Key].Name, item.Key, item.Value);

            FormEditText fe = new DiversityWorkbench.Forms.FormEditText("Data error(s)", message, true);
            fe.ShowDialog();
        }

        #endregion

        #region Export

        private void buttonExport_Click(object sender, EventArgs e)
        {
            string TableName = ((System.Data.DataTable)this.dataGridView.DataSource).TableName;
            System.IO.StreamWriter sw;
            string GridViewExportFile =
                DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) +
                System.Windows.Forms.Application.ProductName.ToString() + "_" + TableName + "_Export_" +
                System.DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt";
            if (System.IO.File.Exists(GridViewExportFile))
                sw = new System.IO.StreamWriter(GridViewExportFile, true);
            else
                sw = new System.IO.StreamWriter(GridViewExportFile);
            try
            {
                sw.WriteLine("Export from " + TableName);
                sw.WriteLine();
                sw.WriteLine("User:\t" + System.Environment.UserName);
                sw.Write("Date:\t");
                sw.WriteLine(DateTime.Now);
                sw.WriteLine();
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                {
                    if (C.Visible)
                        sw.Write(C.DataPropertyName + "\t");
                }
                sw.WriteLine();
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                {
                    foreach (System.Windows.Forms.DataGridViewCell Cell in R.Cells)
                    {
                        if (this.dataGridView.Columns[Cell.ColumnIndex].Visible)
                        {
                            if (Cell.Value == null)
                                sw.Write("\t");
                            else
                                sw.Write(Cell.Value.ToString() + "\t");
                        }
                    }
                    sw.WriteLine();
                }
                System.Windows.Forms.MessageBox.Show("Data were exported to " + GridViewExportFile);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                sw.Close();
            }
        }

        #endregion
    }
}
