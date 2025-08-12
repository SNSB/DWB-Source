using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class UserControlTableSetting : UserControl
    {

        #region Parameter

        private DataTable _DataTable;
        private iTableSettings _iTableSettings;
        private bool _ForAdding = true;
        private string _TableAlias = "";
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Spreadsheet.UserControlColumnSettings> _ColumnControls;
        
        #endregion

        #region Construction
        
        public UserControlTableSetting(DataTable Table, iTableSettings iTableSettings)
        {
            InitializeComponent();
            this._DataTable = Table;
            this._iTableSettings = iTableSettings;
            this.initControl();
        }

        public UserControlTableSetting(DataTable Table, iTableSettings iTableSettings, bool ForAdding)
        {
            InitializeComponent();
            this._ForAdding = ForAdding;
            if (this._ForAdding && Table.Type() == DataTable.TableType.Parallel)
            {
                this._TableAlias = Table.GetDataTableParallel(Table);
                this._DataTable = Table.Sheet().DataTables()[this._TableAlias];
            }
            else
            {
                this._DataTable = Table;
                this._TableAlias = Table.Alias();
            }
            this._iTableSettings = iTableSettings;
            this.initControl();
        }

        #endregion

        #region Init and Events

        private void initControl()
        {
            try
            {
                if (this._DataTable.Description().Length > 0)
                {
                    this.labelDescription.Text = this._DataTable.Description();
                    this.labelDescription.Visible = true;
                }
                if (this.labelDescription.Text != this._DataTable.Alias())
                    this.labelDescription.Text += " [Alias: " + this._DataTable.Alias() + "]";
                else
                    this.labelDescription.Text = "Alias: " + this._DataTable.Alias();
                this.textBoxDisplayText.Text = this._DataTable.DisplayText;
                this.textBoxDisplayText.BackColor = this._DataTable.ColorBack();
                this.labelTableName.Text = this._DataTable.Name;
                if (this._DataTable.Type() == DataTable.TableType.Parallel && !this._ForAdding && false) // Not used any more - redesigned
                {
                    this.buttonAdd.Visible = true;
                    if (this._DataTable.TemplateAlias != this._DataTable.Alias())
                        this.buttonRemove.Visible = true;
                    else
                        this.buttonRemove.Visible = false;
                }
                else
                {
                    this.buttonAdd.Visible = false;
                    this.buttonRemove.Visible = false;
                }
                this.panelColumns.Controls.Clear();
                this.setFilterOperator();
                int i = 1;
                this._ColumnControls = new Dictionary<string, UserControlColumnSettings>();
                if (this._DataTable.RestrictionColumns != null)
                {
                    foreach (string R in this._DataTable.RestrictionColumns)
                    {
                        UserControlColumnSettings UC = new UserControlColumnSettings(this._DataTable.DataColumns()[R], this._iTableSettings, this.textBoxDisplayText);
                        this.panelColumns.Controls.Add(UC);
                        this._ColumnControls.Add(R, UC);
                        UC.Dock = DockStyle.Top;
                        UC.BringToFront();
                        if (i == 1)
                        {
                            UC.HeaderVisible(true);
                            i++;
                        }
                        else
                            UC.HeaderVisible(false);
                    }

                    if (this.panelColumns.Controls.Count > 0)
                    {
                        System.Windows.Forms.Panel P = new Panel();
                        P.Height = 10;
                        this.panelColumns.Controls.Add(P);
                        P.Dock = DockStyle.Top;
                        P.BringToFront();
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> KV in this._DataTable.DataColumns())
                {
                    if (this._DataTable.RestrictionColumns != null && this._DataTable.RestrictionColumns.Contains(KV.Key))
                        continue;
                    UserControlColumnSettings UC = new UserControlColumnSettings(KV.Value, this._iTableSettings, this.textBoxDisplayText);
                    this.panelColumns.Controls.Add(UC);
                    this._ColumnControls.Add(KV.Key, UC);
                    UC.Dock = DockStyle.Top;
                    UC.BringToFront();
                    if (i == 1)
                    {
                        UC.HeaderVisible(true);
                        i++;
                    }
                    else
                        UC.HeaderVisible(false);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void textBoxDisplayText_Leave(object sender, EventArgs e)
        {
            this.setTableDisplayText();
            //this._DataTable.DisplayText = this.textBoxDisplayText.Text;
            //this._iTableSettings.SetTabName(this._DataTable.Alias(), this.textBoxDisplayText.Text);
        }

        private void setTableDisplayText()
        {
            if (this._DataTable.DisplayText != this.textBoxDisplayText.Text)
            {
                this._DataTable.DisplayText = this.textBoxDisplayText.Text;
                this._iTableSettings.SetTabName(this._DataTable.Alias(), this.textBoxDisplayText.Text);
                Sheet.RebuildNeeded = true;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string Alias = this._DataTable.GetDataTableParallel(this._DataTable);
                this._iTableSettings.AddTable(Alias);
                Sheet.RebuildNeeded = true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string Alias = this._DataTable.Alias();
                this._iTableSettings.RemoveTable(Alias);
                Sheet.RebuildNeeded = true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._DataTable.FilterOperator = this.comboBoxOperator.SelectedItem.ToString();
            this.setFilterOperator();
            Sheet.RebuildNeeded = true;
        }

        private void setFilterOperator()
        {
            try
            {
                this.comboBoxOperator.Text = this._DataTable.FilterOperator;
                this.labelOperatorExplanation.Text = "= " + this._DataTable.Sheet().FilterOperatorToolTip(this._DataTable.FilterOperator);
                if (this._DataTable.FilterOperator == "◊" || this._DataTable.FilterOperator == "Ø")
                {
                    bool ColumnFilterHasBeenReset = false;
                    foreach (System.Collections.Generic.KeyValuePair<string, Spreadsheet.UserControlColumnSettings> UC in this._ColumnControls)
                    {
                        if ((this._DataTable.DataColumns()[UC.Key].FilterValue != null 
                            && this._DataTable.DataColumns()[UC.Key].FilterValue.Length > 0) 
                            || this._DataTable.DataColumns()[UC.Key].OrderDirection != DataColumn.OrderByDirection.none)
                            ColumnFilterHasBeenReset = true;
                        UC.Value.ResetFilterAndSorting();
                    }
                    //foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._DataTable.DataColumns())
                    //{
                    //    if (DC.Value.Type() != DataColumn.ColumnType.Operation
                    //        && DC.Value.Type() != DataColumn.ColumnType.Spacer)
                    //    {
                    //        //DC.Value.FilterOperator = "";
                    //        if (DC.Value.FilterValue.Length > 0 || DC.Value.OrderDirection != DataColumn.OrderByDirection.none || DC.Value.OrderSequence != null)
                    //            ColumnFilterHasBeenReset = true;
                    //        DC.Value.FilterValue = "";
                    //        DC.Value.OrderDirection = DataColumn.OrderByDirection.none;
                    //        DC.Value.OrderSequence = null;
                    //    }
                    //}
                    if (ColumnFilterHasBeenReset)
                        System.Windows.Forms.MessageBox.Show("Column filters and sorting have been reset");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxOperator_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxOperator.Items.Count == 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._DataTable.Sheet().FilterOperatorListTable())
                    this.comboBoxOperator.Items.Add(KV.Value);
            }
        }
        
        #endregion

        #region Interface
        
        public void Dispose()
        {
            foreach (System.Windows.Forms.Control C in this.panelColumns.Controls)
            {
                this.panelColumns.Controls.Remove(C);
                C.Dispose();
            }
        }

        public string TableAlias() { return this._TableAlias; }
        
        #endregion
    }
}
