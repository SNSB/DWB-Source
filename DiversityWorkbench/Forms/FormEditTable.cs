using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormEditTable : Form
    {
        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _Adapter;
        private System.Data.DataTable _dt;
        private bool _AllowEditing = true;
        private bool _AllowFilter = false;
        private int _PositionSelectorColumn = -1;
        private int _PositionIDColumn = -1;
        
        #endregion

        #region Construction

        public FormEditTable(Microsoft.Data.SqlClient.SqlDataAdapter Adapter, string TitleForm, string TitleHeader)
        {
            InitializeComponent();
            if (TitleForm.Length > 0)
                this.Text = TitleForm;
            else this.Text = "Edit contents of table";
            if (TitleHeader.Length > 0)
                this.labelHeader.Text = TitleHeader;
            else this.labelHeader.Text = "Edit the contents of the table and click OK to save the changes and close the form";
            this._Adapter = Adapter;
            this._dt = new DataTable();
            try
            {
                this._Adapter.Fill(_dt);
                this.dataGridView.DataSource = _dt;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public FormEditTable(Microsoft.Data.SqlClient.SqlDataAdapter Adapter, string TitleForm, string TitleHeader, bool AllowEditing)
            : this(Adapter, TitleForm, TitleHeader)
        {
            this._AllowEditing = AllowEditing;
            if (!AllowEditing)
            {
                if (TitleForm.Length == 0)
                    this.Text = "Inspect contents of table";
                if (TitleHeader.Length == 0)
                    this.labelHeader.Text = "Inspect the contents of the table";
                this.dataGridView.ReadOnly = true;
                this.dataGridView.AllowDrop = false;
                this.dataGridView.AllowUserToAddRows = false;
                this.dataGridView.AllowUserToDeleteRows = false;
            }
        }

        public FormEditTable(Microsoft.Data.SqlClient.SqlDataAdapter Adapter, string TitleForm, string TitleHeader, bool AllowEditing, bool AllowFilter)
            : this(Adapter, TitleForm, TitleHeader, AllowEditing)
        {
            this._AllowEditing = AllowEditing;
            this._AllowFilter = AllowFilter;
            this.setFilterControls(AllowFilter);
        }

        /// <summary>
        /// For selecting datasets
        /// </summary>
        /// <param name="Adapter">The sql data adapter that holds the data</param>
        /// <param name="TitleForm">Title of the form</param>
        /// <param name="TitleHeader">Text in the header line</param>
        /// <param name="AllowEditing"></param>
        /// <param name="PositionSelectorColumn">Position of the colum for seletion of the rows</param>
        /// <param name="PositionIDColumn">Position of the column containing a unique ID</param>
        public FormEditTable(Microsoft.Data.SqlClient.SqlDataAdapter Adapter, string TitleForm, string TitleHeader, bool AllowEditing, int PositionSelectorColumn, int PositionIDColumn)
            : this(Adapter, TitleForm, TitleHeader, AllowEditing)
        {
            this._PositionSelectorColumn = PositionSelectorColumn;
            this._PositionIDColumn = PositionIDColumn;
            this.buttonAll.Visible = true;
            this.buttonNone.Visible = true;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            if (!AllowEditing)
            {
                this.dataGridView.ReadOnly = false;
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                {
                    C.ReadOnly = true;
                }
                this.dataGridView.Columns[PositionSelectorColumn].ReadOnly = false;
            }
            this.dataGridView.Columns[PositionIDColumn].Visible = false;
        }
        
        #endregion

        #region Form

        private void FormEditTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && this._AllowEditing)
            {
                Microsoft.Data.SqlClient.SqlCommandBuilder B = new Microsoft.Data.SqlClient.SqlCommandBuilder(this._Adapter);
                this._Adapter.Update(_dt);
            }
        }
        
        #endregion

        #region public interface
        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        public int NumberOfDatasets
        {
            get
            {
                if (this._PositionSelectorColumn > -1)
                {
                    if (this.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        //foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                        //    R.Cells[this._PositionSelectorColumn].Value = 0;
                        return 0;
                    }
                    int i = 0;
                    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                    {
                        if (R.Cells[this._PositionSelectorColumn].Value.ToString() == "True")
                            i++;
                    }
                    return i;
                }
                return this.dataGridView.Rows.Count;
            }
        }

        public System.Data.DataTable DataTable
        {
            get
            {
                if (this.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    return null;
                return this._dt;
            }
        }

        public void setIcon(System.Drawing.Icon Icon)
        {
            this.Icon = Icon;
        }

        public void setIcon(System.Drawing.Image Image)
        {
            Bitmap theBitmap = new Bitmap(Image, new Size(16, 16));
            IntPtr Hicon = theBitmap.GetHicon();// Get an Hicon for myBitmap.
            Icon newIcon = Icon.FromHandle(Hicon);// Create a new icon from the handle.
            this.setIcon(newIcon);
        }

        public void ShowRowHeader(bool Show)
        {
            this.dataGridView.RowHeadersVisible = Show;
        }

        public string SelectedColumn()
        {
            return this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName;
        }

        public string Filter()
        {
            return this._Filter;
        }
        
        #endregion

        #region Events
        
        private void buttonAll_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                R.Cells[this._PositionSelectorColumn].Value = 1;
        }

        private void buttonNone_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.SuspendLayout();
            //foreach (System.Data.DataRow R in this._dt.Rows)
            //    R[0] = 0;
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                R.Cells[this._PositionSelectorColumn].Value = 0;
            this.ResumeLayout();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region Filter

        private string _Filter = "";
        private string _FilterColumn = "";

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            if (this._FilterColumn.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a column for the filter");
                return;
            }
            if (this.comboBoxFilter.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select the type of filtering the values");
                return;
            }
            if (this.textBoxFilter.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter the value for the filter");
                return;
            }
            this._Filter = "[" + this._FilterColumn + "] ";
            if (this.comboBoxFilter.SelectedItem.ToString() == "~")
                this._Filter += "LIKE ";
            else
                this._Filter += this.comboBoxFilter.SelectedItem.ToString();
            this._Filter += "'" + this.textBoxFilter.Text + "'";
            this._Adapter.SelectCommand.CommandText += " WHERE " + this._Filter;
            this._dt.Clear();
            try
            {
                this._Adapter.Fill(_dt);
                this.dataGridView.DataSource = _dt;
                this.buttonFilter.BackColor = System.Drawing.SystemColors.Control;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setFilterControls(bool Visible)
        {
            this.buttonFilter.Visible = Visible;
            this.buttonFilter.Enabled = false;
            this.comboBoxFilter.Visible = Visible;
            this.comboBoxFilter.Enabled = false;
            this.textBoxFilter.Visible = Visible;
            this.textBoxFilter.Enabled = false;
            this.labelFilterColumn.Visible = Visible;
        }
        
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this._AllowFilter)
            {
                this._FilterColumn = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName;
                this.labelFilterColumn.Text = this._FilterColumn;
                this.buttonFilter.Enabled = true;
                this.comboBoxFilter.Enabled = true;
                this.textBoxFilter.Enabled = true;
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            this.buttonFilter.BackColor = System.Drawing.Color.Red;
        }

        #endregion

    }
}
