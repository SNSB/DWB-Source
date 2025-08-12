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
    public partial class FormGetMultiWithIdFromList : Form
    {
        #region Parameter
        private System.Collections.Generic.Dictionary<int, string> _Items;
        private bool _OnlyView;

        public System.Collections.Generic.List<int> SelectedItems
        {
            get
            {
                System.Collections.Generic.List<int> result = new List<int>();
                try
                {
                    for (int rowIdx = 0; rowIdx < this.dataGridView.RowCount; rowIdx++)
                    {
                        if ((bool)this.dataGridView["ColumnSelect", rowIdx].Value)
                        {
                            int id = (int)this.dataGridView["ColumnId", rowIdx].Value;
                            if (!result.Contains(id))
                                result.Add(id);
                        }
                    }
                }
                catch (Exception) { }
                return result;
            }
            set
            {
                try
                {
                    for (int rowIdx = 0; rowIdx < this.dataGridView.RowCount; rowIdx++)
                    {
                        int id = (int)this.dataGridView["ColumnId", rowIdx].Value;
                        this.dataGridView["ColumnSelect", rowIdx].Value = value.Contains(id);
                    }
                }
                catch (Exception) { }
            }
        }
        #endregion

        #region Construction
        /// <summary>
        /// Form for getting several items from a list.
        /// </summary>
        /// <param name="Title">Title of the form</param>
        /// <param name="Header">Header text with explanation</param>
        /// <param name="Items">The list of the items - numeric ID and item name</param>
        /// <param name="ColHeader">'true' if column header shall be shown</param>
        /// <param name="OnlyView">'true' if values shall only be displayed</param>
        public FormGetMultiWithIdFromList(string Title, string Header, System.Collections.Generic.Dictionary<int, string> Items, bool ColHeader, bool OnlyView = false)
        {
            InitializeComponent();
            _Items = Items;
            _OnlyView = OnlyView;
            if (Title != null && Title != "")
                this.Text = Title;
            if (Header != null && Header != "")
                this.labelHeader.Text = Header;
            //else
            //    this.labelHeader.Visible = false;
            this.dataGridView.ColumnHeadersVisible = ColHeader;
            this.initForm();
        }
        #endregion

        #region Public
        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }
        #endregion

        #region Private
        private void initForm()
        {
            this.dataGridView.Rows.Add(_Items.Count);
            int rowIdx = 0;
            foreach (KeyValuePair<int, string> item in _Items)
            {
                this.dataGridView["ColumnSelect", rowIdx].Value = false;
                this.dataGridView["ColumnId", rowIdx].Value = item.Key;
                this.dataGridView["ColumnItem", rowIdx].Value = item.Value;
                rowIdx++;
            }

            if (_OnlyView)
            {
                this.dataGridView.Columns["ColumnSelect"].Visible = false;
                this.labelHeader.Visible = false;
                this.buttonAll.Visible = false;
                this.buttonNone.Visible = false;
                this.userControlDialogPanel.Visible = false;
                this.Icon = DiversityWorkbench.Properties.Resources.Lupe1;
            }
        }
        #endregion

        #region Events
        private void buttonAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.dataGridView.SuspendLayout();
            try
            {
                for (int rowIdx = 0; rowIdx < this.dataGridView.RowCount; rowIdx++)
                {
                    this.dataGridView["ColumnSelect", rowIdx].Value = true;
                }
            }
            catch (Exception) { }
            this.dataGridView.ResumeLayout();
            this.Cursor = Cursors.Default;
        }

        private void buttonNone_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.dataGridView.SuspendLayout();
            try
            {
                for (int rowIdx = 0; rowIdx < this.dataGridView.RowCount; rowIdx++)
                {
                    this.dataGridView["ColumnSelect", rowIdx].Value = false;
                }
            }
            catch (Exception) { }
            this.dataGridView.ResumeLayout();
            this.Cursor = Cursors.Default;
        }
        #endregion
    }
}
