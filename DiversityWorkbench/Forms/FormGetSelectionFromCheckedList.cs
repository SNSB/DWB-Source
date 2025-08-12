using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetSelectionFromCheckedList : Form
    {
        #region Parameter

        private System.Data.DataTable _dtList;
        private System.Data.DataTable _dtListOri;
        private System.Collections.Generic.List<bool> _OriginalSelection;
        private string _DisplayColumn;
        
        #endregion

        #region Construction

        public FormGetSelectionFromCheckedList(
            System.Data.DataTable dtList,
            string DisplayColumn,
            string TextForm,
            string TextHeader,
            System.Collections.Generic.List<string> RemoveFromList)
        {
            InitializeComponent();
            this._dtList = dtList.Copy();
            this._dtListOri = dtList.Copy();
            this._DisplayColumn = DisplayColumn;
            this.fillList(true);
            if (TextForm.Length == 0)
                this.Text = "Select from list";
            else this.Text = TextForm;
            if (TextHeader.Length == 0)
                this.labelHeader.Text = "Please select the items from the list";
            else this.labelHeader.Text = TextHeader;
            if (RemoveFromList.Count > 0)
            {
                foreach (string s in RemoveFromList)
                {
                    this.textBoxExclude.Text = s;
                    this.RemoveFromList();
                }
            }
            this.setCounter();
        }

        public FormGetSelectionFromCheckedList(
            System.Data.DataTable dtList,
            string DisplayColumn,
            string TextForm,
            string TextHeader)
        {
            InitializeComponent();
            this._dtList = dtList.Copy();
            this._dtListOri = dtList.Copy();
            this._DisplayColumn = DisplayColumn;
            this.fillList(true);
            if (TextForm.Length == 0)
                this.Text = "Select from list";
            else this.Text = TextForm;
            if (TextHeader.Length == 0)
                this.labelHeader.Text = "Please select the items from the list";
            else this.labelHeader.Text = TextHeader;
            this.setCounter();
        }

        public FormGetSelectionFromCheckedList(
            System.Collections.Generic.Dictionary<string, bool> Dictionary,
            string TextForm,
            string TextHeader)
        {
            InitializeComponent();
            this.ShowItemChangingControls(false);
            this._dtList = new DataTable();
            System.Data.DataColumn C = new DataColumn("Col", typeof(string));
            this._dtList.Columns.Add(C);
            this._DisplayColumn = "Col";
            this.checkedListBox.Items.Clear();
            this._OriginalSelection = new List<bool>();
            foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in Dictionary )
            {
                System.Data.DataRow R = this._dtList.NewRow();
                R[0] = KV.Key;
                this._dtList.Rows.Add(R);
                this.checkedListBox.Items.Add(KV.Key, KV.Value);
                this._OriginalSelection.Add(KV.Value);
            }
            this._dtListOri = this._dtList.Copy();
            if (TextForm.Length == 0)
                this.Text = "Select from list";
            else this.Text = TextForm;
            if (TextHeader.Length == 0)
                this.labelHeader.Text = "Please select the items from the list";
            else this.labelHeader.Text = TextHeader;
            this.setCounter();
        }

        #endregion

        #region Public properties

        public System.Data.DataTable SelectedItems
        {
            get
            {
                System.Data.DataTable dtList = this._dtList.Clone();
                for (int i = 0; i < this.checkedListBox.CheckedItems.Count; i++)
                {
                    System.Data.DataRow[] Rold = this._dtList.Select(this._DisplayColumn + " = '" + this.checkedListBox.CheckedItems[i].ToString() + "'");
                    if (Rold.Length > 0)
                    {
                        System.Data.DataRow Rnew = dtList.NewRow();
                        foreach (System.Data.DataColumn C in this._dtList.Columns)
                        {
                            Rnew[C.ColumnName] = Rold[0][C.ColumnName];
                        }
                        dtList.Rows.Add(Rnew);
                    }
                }
                dtList.AcceptChanges();
                return dtList;
            }
        }
        
        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Private functions

        public void ShowItemChangingControls(bool ShowControls)
        {
            this.labelExclude.Visible = ShowControls;
            this.labelRestrict.Visible = ShowControls;
            this.textBoxExclude.Visible = ShowControls;
            this.textBoxRestrict.Visible = ShowControls;
            this.buttonInclude.Visible = ShowControls;
            this.buttonRequery.Visible = ShowControls;
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            if (this._OriginalSelection != null)
            {
                for (int i = 0; i < this._OriginalSelection.Count; i++)
                    this._OriginalSelection[i] = true;
                this.fillList();
            }
            else
                this.fillList(true);
            this.setCounter();
        }

        private void buttonCheckNone_Click(object sender, EventArgs e)
        {
            if (this._OriginalSelection != null)
            {
                for (int i = 0; i < this._OriginalSelection.Count; i++)
                    this._OriginalSelection[i] = false;
                this.fillList();
            }
            else
                this.fillList(false);
            this.setCounter();
        }

        private void fillList(bool Checked)
        {
            this.checkedListBox.Items.Clear();
            foreach (System.Data.DataRow R in this._dtList.Rows)
            {
                this.checkedListBox.Items.Add(R[this._DisplayColumn].ToString(), Checked);
            }
            this.setCounter();
        }

        private void fillList()
        {
            this.checkedListBox.Items.Clear();
            for (int i = 0; i < this._dtList.Rows.Count; i++) //System.Data.DataRow R in this._dtList.Rows)
            {
                this.checkedListBox.Items.Add(this._dtList.Rows[i][this._DisplayColumn].ToString(), this._OriginalSelection[i]);
            }
            this.setCounter();
        }

        private void buttonRequery_Click(object sender, EventArgs e)
        {
            this.RemoveFromList();
            this.setCounter();
        }

        private void RemoveFromList()
        {
            try
            {
                string Ex = this.textBoxExclude.Text.Replace("%", "");
                Ex = Ex.Replace("*", "");
                System.Collections.Generic.List<System.Data.DataRow> RowsToRemove = new List<DataRow>();
                foreach (System.Data.DataRow R in this._dtList.Rows)
                {
                    bool DeleteRow = false;
                    if ((this.textBoxExclude.Text.StartsWith("%")
                        || this.textBoxExclude.Text.StartsWith("*"))
                        &&
                        (this.textBoxExclude.Text.EndsWith("%")
                        || this.textBoxExclude.Text.EndsWith("*")
                        ))
                    {
                        if (R[this._DisplayColumn].ToString().IndexOf(Ex) > 1)
                        {
                            DeleteRow = true;
                        }
                    }
                    else if (this.textBoxExclude.Text.StartsWith("%")
                        || this.textBoxExclude.Text.StartsWith("*"))
                    {
                        if (R[this._DisplayColumn].ToString().EndsWith(Ex))
                        {
                            DeleteRow = true;
                        }
                    }
                    else if (this.textBoxExclude.Text.EndsWith("%")
                        || this.textBoxExclude.Text.EndsWith("*"))
                    {
                        if (R[this._DisplayColumn].ToString().StartsWith(Ex))
                        {
                            DeleteRow = true;
                        }
                    }
                    else
                    {
                        if (R[this._DisplayColumn].ToString() == Ex)
                        {
                            DeleteRow = true;
                        }
                    }
                    if (DeleteRow)
                    {
                        RowsToRemove.Add(R);
                    }
                }
                if (RowsToRemove.Count > 0)
                {
                    this.buttonSelect.Enabled = false;
                    this.buttonRemove.Enabled = false;
                }
                foreach (System.Data.DataRow R in RowsToRemove)
                    R.Delete();
                this._dtList.AcceptChanges();
                this.fillList(true);
            }
            catch(System.Exception ex)
            { }
        }

        private void buttonInclude_Click(object sender, EventArgs e)
        {
            this.RestrictSelection();
            this.setCounter();
        }

        private void RestrictSelection()
        {
            try
            {
                string Restrict = this.textBoxRestrict.Text.Replace("%", "");
                System.Collections.Generic.List<System.Data.DataRow> RowsToRemove = new List<DataRow>();
                Restrict = Restrict.Replace("*", "");
                foreach (System.Data.DataRow R in this._dtList.Rows)
                {
                    bool DeleteRow = true;
                    if ((this.textBoxRestrict.Text.StartsWith("%")
                        || this.textBoxRestrict.Text.StartsWith("*"))
                        &&
                        (this.textBoxRestrict.Text.EndsWith("%")
                        || this.textBoxRestrict.Text.EndsWith("*")
                        ))
                    {
                        if (R[this._DisplayColumn].ToString().IndexOf(Restrict) > 1)
                        {
                            DeleteRow = false;
                        }
                    }
                    else if (this.textBoxRestrict.Text.StartsWith("%")
                        || this.textBoxRestrict.Text.StartsWith("*"))
                    {
                        if (R[this._DisplayColumn].ToString().EndsWith(Restrict))
                        {
                            DeleteRow = false;
                        }
                    }
                    else if (this.textBoxRestrict.Text.EndsWith("%")
                        || this.textBoxRestrict.Text.EndsWith("*"))
                    {
                        if (R[this._DisplayColumn].ToString().StartsWith(Restrict))
                        {
                            DeleteRow = false;
                        }
                    }
                    else
                    {
                        if (R[this._DisplayColumn].ToString() == Restrict)
                        {
                            DeleteRow = false;
                        }
                    }
                    if (DeleteRow)
                        RowsToRemove.Add(R);
                        //R.Delete();
                }
                if (RowsToRemove.Count > 0)
                {
                    this.buttonSelect.Enabled = false;
                    this.buttonRemove.Enabled = false;
                }
                foreach (System.Data.DataRow R in RowsToRemove)
                    R.Delete();
                this._dtList.AcceptChanges();
                this.fillList(true);
            }
            catch(System.Exception ex)
            { }
        }

        private void setCounter()
        {
            int iAll = this.checkedListBox.Items.Count;
            int iChe = this.checkedListBox.CheckedItems.Count;
            this.labelCounter.Text = iChe.ToString() + " items of " + iAll.ToString();
        }

        private void checkedListBox_Click(object sender, EventArgs e)
        {
            //this.setCounter();
            if (this._OriginalSelection != null)
            {
                for (int i = 0; i < this._OriginalSelection.Count; i++)
                {
                    string S = this.checkedListBox.SelectedItem.ToString();
                    if (this._dtListOri.Rows[i][0].ToString() == S)
                    {
                        if (this.checkedListBox.CheckedItems.Contains(this.checkedListBox.SelectedItem))
                            this._OriginalSelection[i] = false;
                        else
                        {
                            this._OriginalSelection[i] = true;
                        }
                        break;
                    }
                }
            }
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.setCounter();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.AddOrRemoveFromSelection(this.textBoxSelect.Text, true);
        }

        private void AddOrRemoveFromSelection(string SearchString, bool DoAdd)
        {
            if (this._OriginalSelection != null)
            {
                bool StartsWithWildcard = false;
                bool EndsWithWildcard = false;
                if (SearchString.StartsWith("*") || SearchString.StartsWith("%"))
                    StartsWithWildcard = true;
                if (SearchString.EndsWith("*") || SearchString.EndsWith("%"))
                    EndsWithWildcard = true;
                string[] SearchStringArray = SearchString.Split(new char[] { '*', '%' });
                System.Collections.Generic.List<string> SearchStringList = new List<string>();
                for (int i = 0; i < SearchStringArray.Length; i++ )
                {
                    if (SearchStringArray[i].Length > 0)
                        SearchStringList.Add(SearchStringArray[i]);
                }
                for (int i = 0; i < this._dtList.Rows.Count; i++) //System.Data.DataRow R in this._dtList.Rows)
                {
                    string Content = this._dtList.Rows[i][this._DisplayColumn].ToString();
                    bool DoesMatch = false;
                    for (int s = 0; s < SearchStringList.Count; s++ )
                    {
                        if (Content.IndexOf(SearchStringList[s]) > -1)
                        {
                            if ((s == 0 && Content.IndexOf(SearchStringList[s]) == 0) || StartsWithWildcard)
                                DoesMatch = true;
                            else if (s == 0 && Content.StartsWith(SearchStringList[s]))
                                DoesMatch = true;
                            else if (s > 0 && s < SearchStringList.Count - 1)
                                DoesMatch = true;
                            else if (s > 0 && s == SearchStringList.Count - 1 && Content.EndsWith(SearchStringList[s]))
                                DoesMatch = true;
                            else if (s > 0 && s == SearchStringList.Count - 1 && EndsWithWildcard)
                                DoesMatch = true;
                            else
                            {
                                DoesMatch = false;
                                break;
                            }
                        }
                        else
                        {
                            DoesMatch = false;
                            break;
                        }
                        if (DoesMatch)
                        {
                            Content = Content.Substring(Content.IndexOf(SearchStringList[s]) + SearchStringList[s].Length);
                        }
                    }
                    if (DoesMatch)
                    {
                        this._OriginalSelection[i] = DoAdd;
                    }
                }
                this.fillList();
                this.setCounter();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this.AddOrRemoveFromSelection(this.textBoxRemove.Text, false);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (this._dtListOri != null)
            {
                this._dtList = this._dtListOri.Copy();
                if (this._OriginalSelection != null)
                    this.fillList();
                else
                    this.fillList(true);
                this.setCounter();
                this.buttonRemove.Enabled = true;
                this.buttonSelect.Enabled = true;
            }
        }

        #endregion

    }
}
