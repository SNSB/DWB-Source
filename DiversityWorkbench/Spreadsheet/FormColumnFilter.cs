using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormColumnFilter : Form
    {

        #region Parameter

        private DiversityWorkbench.Spreadsheet.DataColumn _DataColumn;
        private DiversityWorkbench.ServerConnection _SC;
        private System.Collections.Generic.List<string> _FilterValueList;
        //private string _LinkedFilterValue;

        #endregion

        #region Construction

        public FormColumnFilter(DiversityWorkbench.Spreadsheet.DataColumn DataColumn)
        {
            InitializeComponent();
            this._DataColumn = DataColumn;
            this.initForm();
        }

        public FormColumnFilter(DiversityWorkbench.Spreadsheet.DataColumn DataColumn, System.Collections.Generic.List<string> FilterValueList)
        {
            InitializeComponent();
            this._DataColumn = DataColumn;
            this._FilterValueList = FilterValueList;
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {
            this.splitContainerFilter.Panel1Collapsed = false;
            this.splitContainerFilter.Panel2Collapsed = true;
            this.labelHeader.Text = "Filter for " + this._DataColumn.DisplayText + " in table " + this._DataColumn.DataTable().DisplayText;
            this.textBoxFilter.Text = this._DataColumn.FilterValue;
            this.labelOperator.Text = this._DataColumn.FilterOperator;
            this.toolTip.SetToolTip(this.labelOperator, "= " + this._DataColumn.DataTable().Sheet().FilterOperatorToolTip(this._DataColumn.FilterOperator));
            this.initLookupSource();
            this.initFilterControls();
            this.setOrderBy();
            this.userControlDialogPanel.buttonCancel.Enabled = false;
            if (this._FilterValueList != null)
                this.buttonAdd.Visible = true;
            if (this._DataColumn.FilterOperator.Length > 0)
                this.GetModuleLinkedFilterValues();
        }

        private void initLookupSource()
        {
            if (this._DataColumn.LookupSource != null && this._DataColumn.LookupSource.Rows.Count > 0)
            {
                System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                foreach (System.Data.DataRow R in this._DataColumn.LookupSource.Rows)
                    StringCollection.Add(R["Value"].ToString());

                this.textBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                this.textBoxFilter.AutoCompleteCustomSource = StringCollection;
                this.textBoxFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }

        #endregion

        #region Events etc.

        private DiversityWorkbench.ServerConnection SC()
        {
            if (this._SC == null)
            {
                try
                {
                    // Markus 20.8.24: Check if null
                    if (this._DataColumn.FilterModuleLinkRoot.Length == 0 && this._DataColumn.FilterValue != null && this._DataColumn.FilterValue.Length > 0)
                    {
                        this._SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(this._DataColumn.FilterValue);
                    }
                    else if (this._DataColumn.FilterModuleLinkRoot.Length > 0)
                    {
                        this._SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(this._DataColumn.FilterModuleLinkRoot);
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
            return this._SC;
        }

        private void initFilterControls()
        {
            try
            {
                // Try to change the filteroperator for the first call
                if (this._DataColumn.FilterOperator == "="
                    && this._DataColumn.FilterModuleLinkRoot.Length > 0
                    && this._DataColumn.FilterValue == this._DataColumn.FilterModuleLinkRoot
                    && this.SC() != null)
                {
                    switch (this._SC.ModuleName)
                    {
                        case "DiversityTaxonNames":
                            this._DataColumn.FilterOperator = "+H+S";
                            break;
                    }
                }

                if (this._DataColumn.FilterOperator.Length > 1) // special filters related to a module, e.g. TaxonNames where synonyms should be included
                {
                    this.textBoxFilter.ReadOnly = true;
                    this.textBoxFilter.Multiline = true;
                    this.linkLabel.Visible = true;
                    this.linkLabel.Text = this._DataColumn.FilterModuleLinkRoot;// this.LinkedFilterURI();
                    if (this.SC() != null)
                    {
                        if (this._DataColumn.FilterModuleLinkRoot.Length == 0 && this._DataColumn.FilterValue.Length > 0 && this.SC() != null)
                        {
                            this._DataColumn.FilterModuleLinkRoot = this._DataColumn.FilterValue;
                        }
                    }
                    if (this._DataColumn.FilterModuleLinkRoot.Length == 0)// this.linkLabel.Text.Length == 0)
                    {
                        this.linkLabel.Text = "No connection. Please connect to source database";
                        this.textBoxFilter.Visible = false;
                        this.labelModuleInfoSource.Visible = false;
                        this.labelModuleInfoSourceHeader.Visible = false;
                        this.Height = (int)(80 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    }
                    else
                    {
                        this.Height = (int)(300 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        if (this._DataColumn.FilterValue.Length == 0)
                            this.GetModuleLinkedFilterValues();
                        else
                            this.setModuleLinkedFilterValueDisplayText();
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._DataColumn.DataTable().Sheet().FilterOperatorListColumn(this._DataColumn))
                        {
                            if (KV.Value == this._DataColumn.FilterOperator)
                            {
                                this.labelModuleInfoOperator.Text = KV.Value;
                                this.labelModuleInfo.Text = KV.Key;
                                break;
                            }
                        }
                        if (this.SC() != null)
                        {
                            this.labelModuleInfoSource.Visible = true;
                            this.labelModuleInfoSourceHeader.Visible = true;
                            this.labelModuleInfoSource.Text = this.SC().DisplayText;
                        }
                        this.splitContainerFilter.Panel1Collapsed = true;
                        this.splitContainerFilter.Panel2Collapsed = false;
                    }
                }
                else
                {
                    this.linkLabel.Visible = false;
                    this.textBoxFilter.ReadOnly = false;
                    switch (this._DataColumn.FilterOperator)
                    {
                        case "|":
                        case "∉":
                            this.textBoxFilter.Multiline = true;
                            this.Height = (int)(210 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                            break;
                        case "Ø":
                            this.textBoxFilter.Multiline = false;
                            this.textBoxFilter.ReadOnly = true;
                            this.textBoxFilter.Text = "NULL";
                            this.Height = (int)(107 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                            break;
                        case "•":
                            this.textBoxFilter.Multiline = false;
                            this.textBoxFilter.ReadOnly = true;
                            this.textBoxFilter.Text = "''";
                            this.Height = (int)(107 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                            break;
                        default:
                            this.textBoxFilter.Multiline = false;
                            this.Height = (int)(107 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                            if (this._DataColumn.FilterOperator.Length == 0)
                            {
                                this.textBoxFilter.ReadOnly = true;
                                this.textBoxFilter.Text = "";
                            }
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void GetModuleLinkedFilterValues()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                if (this.SC() != null)
                {
                    switch (this.SC().ModuleName)
                    {
                        case "DiversityTaxonNames":
                            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
                            switch (this._DataColumn.FilterOperator)
                            {
                                case "+H":
                                    DD = DiversityWorkbench.TaxonName.SubTaxa(this._DataColumn.FilterModuleLinkRoot);// this.linkLabel.Text);
                                    break;
                                case "+S":
                                    DD = DiversityWorkbench.TaxonName.Synonyms(this._DataColumn.FilterModuleLinkRoot);// this.linkLabel.Text);
                                    break;
                                case "+H+S":
                                    DD = DiversityWorkbench.TaxonName.SubTaxaSynonyms(this._DataColumn.FilterModuleLinkRoot);// this.linkLabel.Text);
                                    break;
                            }
                            //string Message = "";
                            this._ModuleLinkedFilterValueDisplayText = null;
                            this.setModuleLinkedFilterValueDisplayText(DD);

                            this.splitContainerFilter.Panel1Collapsed = true;
                            this.splitContainerFilter.Panel2Collapsed = false;

                            //if (System.Windows.Forms.MessageBox.Show(Message, "Set Filter", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                            //{
                            this._DataColumn.FilterValue = this._DataColumn.FilterModuleLinkRoot;//._LinkedFilterValue;
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DD)
                            {
                                if (KV.Key == this._DataColumn.FilterModuleLinkRoot)//._LinkedFilterValue)
                                    continue;
                                if (this._DataColumn.FilterValue.Length > 0)
                                    this._DataColumn.FilterValue += "\r\n";
                                this._DataColumn.FilterValue += KV.Key;
                            }
                            this.textBoxFilter.Text = this._DataColumn.FilterValue;
                            //}
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private System.Collections.Generic.List<string> _ModuleLinkedFilterValueDisplayText;
        private void setModuleLinkedFilterValueDisplayText()
        {
            System.Collections.Generic.Dictionary<string, string> DD = null;
            this.setModuleLinkedFilterValueDisplayText(DD);
        }

        private void setModuleLinkedFilterValueDisplayText(System.Collections.Generic.Dictionary<string, string> DD)
        {
            if (this._ModuleLinkedFilterValueDisplayText == null)
            {
                this._ModuleLinkedFilterValueDisplayText = new List<string>();
                if (this.SC() != null)
                {
                    switch (this.SC().ModuleName)
                    {
                        case "DiversityTaxonNames":
                            if (DD == null)
                            {
                                DD = new Dictionary<string, string>();
                                System.Data.DataTable dt = new System.Data.DataTable();
                                System.Data.DataColumn dc = new System.Data.DataColumn("NameID", typeof(string));
                                dt.Columns.Add(dc);
                                string[] FF = this._DataColumn.FilterValueArray();// .FilterValue.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                //if (FF.Length == 1 && this._DataColumn.FilterValue.IndexOf("\n") > -1)
                                //{
                                //    FF = this._DataColumn.FilterValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                //}
                                if (FF.Length > 0)
                                {
                                    for (int iF = 0; iF < FF.Length; iF++)
                                    {
                                        string ID = FF[iF].Substring(this.SC().BaseURL.Length);
                                        System.Data.DataRow R = dt.NewRow();
                                        R[0] = ID;
                                        dt.Rows.Add(R);
                                    }
                                    DiversityWorkbench.TaxonName.setServerConnection(this._DataColumn.FilterModuleLinkRoot);
                                    DiversityWorkbench.TaxonName.getTaxa(ref DD, dt);
                                }
                            }
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DD)
                            {
                                this._ModuleLinkedFilterValueDisplayText.Add(KV.Value);
                            }
                            break;
                    }
                }
            }
            string Message = "";
            foreach (string S in this._ModuleLinkedFilterValueDisplayText)
            {
                if (Message.Length > 0)
                    Message += "\r\n";
                Message += S;
            }
            this.textBoxModuleInfo.Text = Message;
        }

        private void comboBoxFilterOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._DataColumn.FilterOperator = this._DataColumn.DataTable().Sheet().FilterOperatorListColumn(this._DataColumn)[this.comboBoxFilterOperator.SelectedItem.ToString()];
            this.labelOperator.Text = this._DataColumn.FilterOperator;
            this.toolTip.SetToolTip(this.labelOperator, "= " + this._DataColumn.DataTable().Sheet().FilterOperatorToolTip(this._DataColumn.FilterOperator));
            this._ModuleLinkedFilterValueDisplayText = null;
            if (this._DataColumn.FilterOperator.Length > 0 && this._DataColumn.FilterModuleLinkRoot.Length > 0)
                this._DataColumn.FilterValue = "";
            this.initFilterControls();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            this._DataColumn.FilterValue = this.textBoxFilter.Text;
        }

        private void comboBoxFilterOperator_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxFilterOperator.Items.Count == 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._DataColumn.DataTable().Sheet().FilterOperatorListColumn(this._DataColumn))
                    {
                        switch (KV.Value)
                        {
                            case "<":
                            case ">":
                            case "¬":
                            case "~":
                                if (this._DataColumn.LookupSource == null && (this._DataColumn.IsLinkColumn == null || !(bool)this._DataColumn.IsLinkColumn))
                                    goto default;
                                break;
                            case "+H":
                            case "+S":
                            case "+H+S":
                                if (this._DataColumn.IsLinkColumn != null && (bool)this._DataColumn.IsLinkColumn)
                                    goto default;
                                break;
                            default:
                                this.comboBoxFilterOperator.Items.Add(KV.Key);//.Value);
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonOrderBy_Click(object sender, EventArgs e)
        {
            switch (this._DataColumn.OrderDirection)
            {
                case DataColumn.OrderByDirection.none:
                    this._DataColumn.OrderDirection = DataColumn.OrderByDirection.ascending;
                    this._DataColumn.OrderSequence = 1;
                    break;
                case DataColumn.OrderByDirection.ascending:
                    this._DataColumn.OrderDirection = DataColumn.OrderByDirection.descending;
                    this._DataColumn.OrderSequence = 1;
                    break;
                case DataColumn.OrderByDirection.descending:
                    this._DataColumn.OrderDirection = DataColumn.OrderByDirection.none;
                    this._DataColumn.OrderSequence = null;
                    break;
            }
            this.setOrderBy();
        }

        private void setOrderBy()
        {
            switch (this._DataColumn.OrderDirection)
            {
                case DataColumn.OrderByDirection.none:
                    this.buttonOrderBy.Text = "-";
                    this.toolTip.SetToolTip(this.buttonOrderBy, "No sorting");
                    break;
                case DataColumn.OrderByDirection.ascending:
                    this.buttonOrderBy.Text = "↑";
                    this.toolTip.SetToolTip(this.buttonOrderBy, "Sort data ascending according to this column");
                    break;
                case DataColumn.OrderByDirection.descending:
                    this.buttonOrderBy.Text = "↓";
                    this.toolTip.SetToolTip(this.buttonOrderBy, "Sort data descending according to this column");
                    break;
            }
        }

        private void textBoxFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._DataColumn.SqlForColumn.Length == 0 && this._DataColumn.Column.DataType != null && this._DataColumn.Column.DataTypeBasicType != null)
                {
                    switch (this._DataColumn.Column.DataTypeBasicType)
                    {
                        case Data.Column.DataTypeBase.date:
                            DiversityWorkbench.Forms.FormGetDate f = new Forms.FormGetDate(false);
                            f.ShowDialog();
                            if (f.DialogResult == DialogResult.OK)
                            {
                                this.textBoxFilter.Text = f.Date.Year.ToString() + "-" + f.Date.Month.ToString() + "-" + f.Date.Day.ToString();
                            }
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonRemoveModuleFilter_Click(object sender, EventArgs e)
        {
            this._DataColumn.FilterOperator = "=";
            this._DataColumn.FilterValue = "";
            this.Close();
        }

        private void buttonModuleFilterEdit_Click(object sender, EventArgs e)
        {
            this.splitContainerFilter.Panel1Collapsed = false;
            this.splitContainerFilter.Panel2Collapsed = true;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (this._FilterValueList != null && this._FilterValueList.Count > 0)
            {
                if (this._DataColumn.FilterOperator == "∉" || this._DataColumn.FilterOperator == "|")
                {
                    System.Collections.Generic.Dictionary<string, bool> FilterDict = new Dictionary<string, bool>();
                    foreach (string s in this._FilterValueList)
                        FilterDict.Add(s, false);
                    DiversityWorkbench.Forms.FormGetMultiFromList fMulti = new Forms.FormGetMultiFromList("Add filter values", "Please select the filter values from the list", FilterDict);
                    fMulti.ShowDialog();
                    if (fMulti.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (string s in fMulti.SelectedItems())
                        {
                            if (this.textBoxFilter.Text.Length > 0)
                                this.textBoxFilter.Text += "\r\n";
                            this.textBoxFilter.Text += s;
                            this._FilterValueList.Remove(s);
                        }
                    }
                }
                else
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._FilterValueList, "Add filter value", "Please select a filter value from the list", true);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (this.textBoxFilter.Multiline)
                        {
                            if (this.textBoxFilter.Text.Length > 0)
                                this.textBoxFilter.Text += "\r\n";
                            this.textBoxFilter.Text += f.SelectedString;
                        }
                        else
                        {
                            this.textBoxFilter.Text = f.SelectedString;
                        }
                        this._FilterValueList.Remove(f.SelectedString);
                    }
                }
            }
        }

        #endregion

    }
}
