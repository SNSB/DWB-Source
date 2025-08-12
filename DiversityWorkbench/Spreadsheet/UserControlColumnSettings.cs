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
    public partial class UserControlColumnSettings : UserControl
    {

        #region Properties

        private DataColumn _DataColumn;
        private iTableSettings _iTableSettings;
        private System.Windows.Forms.TextBox _TextBoxTableDisplayText;
        
        #endregion

        #region Construction

        public UserControlColumnSettings(DataColumn Column, iTableSettings iTableSettings, System.Windows.Forms.TextBox TextBoxTableDisplayText)
        {
            InitializeComponent();
            this._DataColumn = Column;
            this._iTableSettings = iTableSettings;
            this._TextBoxTableDisplayText = TextBoxTableDisplayText;
            this.initControl();
        }
        
        #endregion

        public void ResetFilterAndSorting()
        {
            this.comboBoxFilter.Text = "";

            this._DataColumn.OrderDirection = DataColumn.OrderByDirection.none;
            this._DataColumn.OrderSequence = null;
            this.setOrder();

            Sheet.RebuildNeeded = true;
        }

        #region Events

        private void initControl()
        {
            try
            {
                if (this._DataColumn.IsIdentity
                    || this._DataColumn.IsReadOnly()
                    || this._DataColumn.DataTable().Type() == DataTable.TableType.Lookup)
                {
                    this.comboBoxInsertDefault.Enabled = false;
                }

                System.Drawing.Font Fpk = new Font(this.labelName.Font.FontFamily, this.labelName.Font.Size, FontStyle.Bold);
                this.labelName.Text = this._DataColumn.Name + " = ";
                if (this._DataColumn.Column.Table == null || this._DataColumn.DataRetrievalType == DataColumn.RetrievalType.ViewOnly)
                {
                    this.labelName.ForeColor = DiversityWorkbench.Spreadsheet.FormSpreadsheet.ColorViewOnly();// System.Drawing.Color.Blue;
                    this.comboBoxInsertDefault.Enabled = false;
                }
                else if (!this._DataColumn.Column.IsNullable && this._DataColumn.Column.ColumnDefault == null)
                    this.labelName.ForeColor = System.Drawing.Color.Red;
                else if (this._DataColumn.LinkedModule != RemoteLink.LinkedModule.None)
                    this.labelName.ForeColor = System.Drawing.Color.Blue;
                if (this._DataColumn.DataTable().PrimaryKeyColumnList.Contains(this._DataColumn.Name))
                    this.labelName.Font = Fpk;
                if (this._DataColumn.DisplayText != null)
                    this.textBoxDisplay.Text = this._DataColumn.DisplayText;
                if (this._DataColumn.FilterExclude)
                {
                    this.comboBoxFilter.Enabled = false;
                    this.textBoxFilterOperator.Enabled = false;
                }
                this.comboBoxFilter.Text = this._DataColumn.FilterValue;
                this.checkBoxIsVisible.Checked = this._DataColumn.IsVisible;
                this.checkBoxIsHidden.Checked = this._DataColumn.IsHidden;
                this.checkBoxIsHidden.Enabled = this._DataColumn.IsVisible;
                if (this._DataColumn.IsOutdated)
                {
                    this.Enabled = false;
                    //this.checkBoxIsHidden.Enabled = false;
                    //this.checkBoxIsVisible.Enabled = false;
                }

                // Restriction handling
                if (this._DataColumn.DataTable().RestrictionColumns != null &&
                    this._DataColumn.DataTable().RestrictionColumns.Contains(this._DataColumn.Name))
                {
                    this.labelRestriction.Visible = true;
                    /// not allowed - otherwise no removal possible
                    //this.checkBoxIsVisible.Visible = false;

                    this.BackColor = System.Drawing.Color.Pink;

                    // Fixing a missing lookup if present in the template
                    try
                    {
                        if (this._DataColumn.DataTable().TemplateAlias != null)
                        {
                            string TA = this._DataColumn.DataTable().TemplateAlias;
                            if (this._DataColumn.DataTable().Sheet().DataTables()[TA].DataColumns()[this._DataColumn.Name].SqlLookupSource != null)
                            {
                                string SQL = this._DataColumn.DataTable().Sheet().DataTables()[TA].DataColumns()[this._DataColumn.Name].SqlLookupSource;
                                if (SQL.Length > 0 && this._DataColumn.SqlLookupSource == null)
                                {
                                    this._DataColumn.SqlLookupSource = SQL;
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }

                    if (this._DataColumn.LookupSource != null)
                    {
                        this.textBoxRestriction.Visible = false;
                        this.comboBoxRestriction.Visible = true;
                        this.comboBoxRestriction.DataSource = this._DataColumn.LookupSource;
                        this.comboBoxRestriction.DisplayMember = "Display";
                        this.comboBoxRestriction.ValueMember = "Value";
                        if (this._DataColumn.RestrictionValue != null)
                        {
                            for (int i = 0; i < this._DataColumn.LookupSource.Rows.Count; i++)
                            {
                                if (this._DataColumn.LookupSource.Rows[i]["Value"].ToString() == this._DataColumn.RestrictionValue)
                                {
                                    this.comboBoxRestriction.SelectedIndex = i;
                                    break;
                                }
                            }
                            // do not change the restriction for the template e.g. WGS84 should stay WGS84
                            if (this._DataColumn.DataTable().TemplateAlias != null && 
                                this._DataColumn.DataTable().TemplateAlias == this._DataColumn.DataTable().Alias() &&
                                this._DataColumn.DataTable().Type() == DataTable.TableType.Parallel)
                            {
                                this.comboBoxRestriction.Enabled = false;
                            }
                        }
                        // setting the display text to the next available System
                        this.comboBoxRestriction_SelectionChangeCommitted(null, null);
                        // reset for start of form
                        Sheet.RebuildNeeded = false;

                        this.comboBoxFilter.Enabled = false;
                        this.comboBoxInsertDefault.Enabled = false;
                        this.textBoxFilterOperator.Enabled = false;
                        this.buttonOrderDirection.Enabled = false;
                    }
                    else
                    {
                        this.comboBoxRestriction.Visible = false;
                        this.textBoxRestriction.Visible = true;
                        if (this._DataColumn.RestrictionValue != null)
                            this.textBoxRestriction.Text = this._DataColumn.RestrictionValue;
                    }
                    this.Height = 50;
                }
                else
                {
                    this.labelRestriction.Visible = false;
                    this.textBoxRestriction.Visible = false;
                    this.comboBoxRestriction.Visible = false;
                    this.Height = 24;
                }

                if (this._DataColumn.DefaultForAdding != null)
                    this.comboBoxInsertDefault.Text = this._DataColumn.DefaultForAdding;

                if (!this._DataColumn.Column.IsNullable
                    && !this._DataColumn.Column.IsIdentity
                    && !this._DataColumn.DataTable().PrimaryKeyColumnList.Contains(this._DataColumn.Name)
                    && this._DataColumn.Column.ColumnDefault == null
                    && (this._DataColumn.Column.ForeignRelationTable == null
                    || (this._DataColumn.DataTable().ParentTable() != null && this._DataColumn.Column.ForeignRelationTable != this._DataColumn.DataTable().ParentTable().Name)))
                {
                    this.comboBoxInsertDefault.BackColor = System.Drawing.Color.Pink;
                }
                else
                {
                }
                if (this._DataColumn.LookupSource != null)
                {
                    this.toolTip.SetToolTip(this.comboBoxInsertDefault, "Select a value from the list as a default for new data");
                    this.setLookupSource();
                }
                else
                {
                    this.toolTip.SetToolTip(this.comboBoxInsertDefault, "Enter a default for new data");
                    this.comboBoxInsertDefault.Text = this._DataColumn.DefaultForAdding;
                }

                if (this._DataColumn.DataTable().Type() == DataTable.TableType.Lookup)
                {
                    this.pictureBoxAdding.Enabled = false;
                    this.comboBoxInsertDefault.Enabled = false;
                }
                if (this._DataColumn.IsRequired && this.labelName.ForeColor == System.Drawing.SystemColors.ControlText)
                    this.labelName.ForeColor = System.Drawing.Color.Purple;
                this.setOrder();
                this.setFilter();
                this.setControl();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void HeaderVisible(bool IsVisible)
        {
            this.labelHeaderNameInDatabase.Visible = IsVisible;
            this.labelHeaderAdding.Visible = IsVisible;
            this.labelHeaderAlias.Visible = IsVisible;
            this.labelHeaderFilter.Visible = IsVisible;
            this.pictureBoxHidden.Visible = IsVisible;
            this.pictureBoxVisible.Visible = IsVisible;
            if (IsVisible)
                this.Height += 14;
        }

        private void setLookupSource()
        {
            return;
            try
            {
                if (this._DataColumn.LookupSource != null)
                {
                    string PreviousFilter = "";
                    if (this._DataColumn.FilterValue != null)
                        PreviousFilter = this._DataColumn.FilterValue;

                    this.comboBoxInsertDefault.DropDownStyle = ComboBoxStyle.DropDownList;
                    System.Data.DataTable dtInsertDefault = this._DataColumn.LookupSource.Copy();
                    this.comboBoxInsertDefault.DataSource = dtInsertDefault;// this._DataColumn.LookupSource;
                    this.comboBoxInsertDefault.ValueMember = "Value";
                    this.comboBoxInsertDefault.DisplayMember = "Display";
                    if (this._DataColumn.DefaultForAdding != null && this._DataColumn.DefaultForAdding.Length > 0)
                    {
                        bool ValueFound = false;
                        for (int i = 0; i < this._DataColumn.LookupSource.Rows.Count; i++)
                        {
                            if (this._DataColumn.DefaultForAdding == this._DataColumn.LookupSource.Rows[i]["Value"].ToString())
                            {
                                this.comboBoxInsertDefault.SelectedIndex = i;
                                ValueFound = true;
                                break;
                            }
                        }
                        if (!ValueFound)
                            this.comboBoxInsertDefault.Text = this._DataColumn.DefaultForAdding;
                    }

                    this.comboBoxFilter.DropDownStyle = ComboBoxStyle.DropDownList;
                    this.comboBoxFilter.DataSource = this._DataColumn.LookupSource;
                    if (PreviousFilter.Length > 0)
                        this._DataColumn.FilterValue = PreviousFilter;
                    this.comboBoxFilter.ValueMember = "Value";
                    this.comboBoxFilter.DisplayMember = "Display";
                    if (this._DataColumn.FilterValue != null && this._DataColumn.FilterValue.Length > 0)
                    {
                        bool ValueFound = false;
                        for (int i = 0; i < this._DataColumn.LookupSource.Rows.Count; i++)
                        {
                            if (this._DataColumn.FilterValue == this._DataColumn.LookupSource.Rows[i]["Value"].ToString())
                            {
                                this.comboBoxFilter.SelectedIndex = i;
                                ValueFound = true;
                                break;
                            }
                        }
                        if (!ValueFound)
                            this.comboBoxFilter.Text = this._DataColumn.FilterValue;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setLookupSource(System.Windows.Forms.ComboBox CB)
        {
            try
            {
                if (this._DataColumn.LookupSource != null)
                {
                    string PreviousFilter = "";
                    if (this._DataColumn.FilterValue != null)
                        PreviousFilter = this._DataColumn.FilterValue;

                    CB.DropDownStyle = ComboBoxStyle.DropDownList;
                    System.Data.DataTable dtInsertDefault = this._DataColumn.LookupSource.Copy();
                    CB.DataSource = dtInsertDefault;// this._DataColumn.LookupSource;
                    CB.ValueMember = "Value";
                    CB.DisplayMember = "Display";
                    if (this._DataColumn.DefaultForAdding != null && this._DataColumn.DefaultForAdding.Length > 0)
                    {
                        bool ValueFound = false;
                        for (int i = 0; i < this._DataColumn.LookupSource.Rows.Count; i++)
                        {
                            if (this._DataColumn.DefaultForAdding == this._DataColumn.LookupSource.Rows[i]["Value"].ToString())
                            {
                                CB.SelectedIndex = i;
                                ValueFound = true;
                                break;
                            }
                        }
                        if (!ValueFound)
                            CB.Text = this._DataColumn.DefaultForAdding;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setControl()
        {
            if (this._DataColumn.IsVisible)
            {
                if (this._DataColumn.IsHidden)
                    this.BackColor = System.Drawing.Color.LightYellow;
                else
                    this.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                this.BackColor = System.Drawing.SystemColors.Control;
            }
            this.checkBoxIsHidden.Enabled = this._DataColumn.IsVisible;
        }

        private void checkBoxIsVisible_Click(object sender, EventArgs e)
        {
            if (!this.checkBoxIsVisible.Checked &&
                this._DataColumn.IsRequired)
            {
                if (this._DataColumn.IsHidden)
                {
                    System.Windows.Forms.MessageBox.Show("This column is required and can not be excluded. Please hide the column instead");
                    this.checkBoxIsVisible.Checked = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("This column is required and can not be excluded. The column is hidden instead");
                    this._DataColumn.IsHidden = true;
                    this.checkBoxIsHidden.Checked = true;
                    this.checkBoxIsVisible.Checked = true;
                }
            }
            if (!this.checkBoxIsVisible.Checked &&
                !this._DataColumn.Column.IsNullable &&
                this._DataColumn.ColumnDefault == null &&
                (this._DataColumn.DefaultForAdding == null || this._DataColumn.DefaultForAdding.Length == 0) &&
                this._DataColumn.RestrictionValue == null &&
                this._DataColumn.Column.Table != null &&
                this._DataColumn.Column.Name != "RowGUID") // Markus 24.7.23 - do not check RowGUID
            {
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Spreadsheet.DataColumn> KV in this._DataColumn.DataTable().Sheet().SelectedColumns())
                {
                    if (KV.Value.DataTable().Alias() == this._DataColumn.DataTable().Alias()
                        && KV.Value.Type() == DataColumn.ColumnType.Data
                        && KV.Value.IsVisible
                        && KV.Value.Name != this._DataColumn.Name)
                    {
                        i++;
                    }
                }
                if (i > 0)
                    System.Windows.Forms.MessageBox.Show("This column can not be empty. Please enter a default for adding new data");
            }
            this._DataColumn.IsVisible = this.checkBoxIsVisible.Checked;
            this._DataColumn.DataTable().Sheet().ResetSelectedColumns();
            if (this.checkBoxIsVisible.Checked && !this.ParentIsSelected() && this._DataColumn.DataTable().ParentTable().Type() != DataTable.TableType.RootHidden)
            {
                this.checkBoxIsVisible.Checked = false;
                this._DataColumn.IsVisible = this.checkBoxIsVisible.Checked;
                System.Windows.Forms.MessageBox.Show("The table\r\n\t" + this._DataColumn.DataTable().DisplayText + "\r\ncan not be included\r\nbecause it depends on the table\r\n\t" + this._DataColumn.DataTable().ParentTable().DisplayText + "\r\nwhich had not been selected");
                return;
            }
            else if (!this.checkBoxIsVisible.Checked && !this.AnyOtherColumnIsSelected())
            {
                System.Collections.Generic.List<string> TT = this.DependendTablesAreSelected();
                if (TT.Count > 0)
                {
                    this.checkBoxIsVisible.Checked = true;
                    this._DataColumn.IsVisible = this.checkBoxIsVisible.Checked;
                    string Message = "The table\r\n\t" + this._DataColumn.DataTable().DisplayText + "\r\ncan not be excluded\r\nbecause the following tables dependent on it:";
                    foreach (string T in TT)
                        Message +="\r\n\t" + T;
                    System.Windows.Forms.MessageBox.Show(Message);
                    return;
                }
            }
            if (!this.checkBoxIsVisible.Checked && (this._DataColumn.OrderSequence != null || this._DataColumn.OrderDirection != DataColumn.OrderByDirection.none))
            {
                this._DataColumn.OrderSequence = null;
                this._DataColumn.OrderDirection = DataColumn.OrderByDirection.none;
                this.setOrder();
            }

            this._DataColumn.IsVisible = this.checkBoxIsVisible.Checked;
            if (!this._DataColumn.IsVisible)
            {
                this._DataColumn.IsHidden = false;
                this.checkBoxIsHidden.Checked = false;
            }
            if (this._DataColumn.IsVisible && this._DataColumn.Column.Name == "RowGUID") // Markus 24.7.23: Hiding RowGUID by default
            {
                this.checkBoxIsHidden.Checked = true;
                this._DataColumn.IsHidden = true;
            }
            this._DataColumn.DataTable().Sheet().ResetSelectedColumns();
            Sheet.RebuildNeeded = true;
            this.setControl();
        }

        private void checkBoxIsHidden_Click(object sender, EventArgs e)
        {
            this._DataColumn.IsHidden = this.checkBoxIsHidden.Checked;
            Sheet.RebuildNeeded = true;
            this.setControl();
        }

        private bool ParentIsSelected()
        {
            bool ParentSelected = false;
            if (this._DataColumn.DataTable().ParentTable() != null &&
                this._DataColumn.DataTable().ParentTable().Type() != DataTable.TableType.Project &&
                (this._DataColumn.DataTable().Type() != DataTable.TableType.Root || this._DataColumn.DataTable().Type() != DataTable.TableType.Target))
            {
                bool TableIsSelected = false;
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._DataColumn.DataTable().Sheet().SelectedColumns())
                {
                    if (DC.Value.DataTable().Alias() == this._DataColumn.DataTable().Alias())
                    {
                        TableIsSelected = true;
                        break;
                    }
                }
                if (TableIsSelected)
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._DataColumn.DataTable().Sheet().SelectedColumns())
                    {
                        if (DC.Value.DataTable().Alias() == this._DataColumn.DataTable().ParentTable().Alias())
                        {
                            ParentSelected = true;
                            break;
                        }
                    }
                    // Markus 8.5.2019: Falls ParentTable keine eingeblendete Spalte hat - Test ob Spalten nur verborgen sind
                    if (!ParentSelected)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._DataColumn.DataTable().ParentTable().DataColumns())
                        {
                            if (DC.Value.IsVisible && DC.Value.IsHidden)
                            {
                                ParentSelected = true;
                                break;
                            }
                        }
                    }
                }
                else
                    ParentSelected = true;
            }
            else if (this._DataColumn.DataTable().ParentTable() == null)
            {
                 ParentSelected = true;
            }
            else if (this._DataColumn.DataTable().ParentTable().Type() == DataTable.TableType.Project ||
                this._DataColumn.DataTable().ParentTable().Type() != DataTable.TableType.Root)
            {
                ParentSelected = true;
            }
            return ParentSelected;
        }

        private bool AnyOtherColumnIsSelected()
        {
            bool Any = false;
            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._DataColumn.DataTable().DataColumns())
            {
                if (DC.Key != this._DataColumn.Name && DC.Value.IsVisible)
                {
                    Any = true;
                    break;
                }
            }
            return Any;
        }

        private System.Collections.Generic.List<string> DependendTablesAreSelected()
        {
            System.Collections.Generic.List<string> Tables = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._DataColumn.DataTable().Sheet().SelectedColumns())
            {
                if (DC.Value.DataTable().ParentTable() != null && DC.Value.DataTable().ParentTable().Alias() == this._DataColumn.DataTable().Alias())
                {
                    if (!Tables.Contains(DC.Value.DataTable().DisplayText))
                        Tables.Add(DC.Value.DataTable().DisplayText);
                }
            }
            return Tables;
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string Description = this._DataColumn.Column.Name + ":\r\n";
                Description += DiversityWorkbench.Forms.FormFunctions.getColumnDescription(this._DataColumn.DataTable().Name,
                    this._DataColumn.Column.Name,
                    DiversityWorkbench.Settings.Context,
                    DiversityWorkbench.Settings.Language);
                Description += "\r\nType: " + this._DataColumn.Column.DataType;
                if (this._DataColumn.Column.DataTypeLength > 0)
                    Description += " " + this._DataColumn.Column.DataTypeLength.ToString();
                if (this._DataColumn.LinkedModule == RemoteLink.LinkedModule.None)
                    System.Windows.Forms.MessageBox.Show(Description);
                else
                {
                    System.Collections.Generic.List<string> Settings = this._DataColumn.FixedSourceGetSetting();
                    if (Settings != null && Settings.Count > 0)
                    {
                        FormUserSettings f = new FormUserSettings(Settings, this._DataColumn.Column.Name, Description, false);
                        f.ShowDialog();
                        f.Dispose();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show(Description);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #region Restriction

        private void comboBoxRestriction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this._DataColumn.IsRestrictionColumn
                && this.comboBoxRestriction.SelectedValue != null
                && this._DataColumn.RestrictionValue != this.comboBoxRestriction.SelectedValue.ToString())
            {
                this._DataColumn.RestrictionValue = this.comboBoxRestriction.SelectedValue.ToString();
                this.setTableDisplayText();
            }
            foreach(System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._DataColumn.DataTable().DataColumns())
            {
                if (DC.Value.RemoteLinkIsOptional)
                {
                    if (DC.Value.RemoteLinks.Count > 0)
                    {
                        foreach (DiversityWorkbench.Spreadsheet.RemoteLink R in DC.Value.RemoteLinks)
                        {
                            if (R.DecisionColumnValues.Contains(this._DataColumn.RestrictionValue))
                            {
                                DC.Value.LinkedModule = R.LinkedToModule;
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            Sheet.RebuildNeeded = true;
        }

        private void textBoxRestriction_Leave(object sender, EventArgs e)
        {
            this._DataColumn.RestrictionValue = this.textBoxRestriction.Text;
            this.setTableDisplayText();
            Sheet.RebuildNeeded = true;
        }
        
        #endregion

        #region Display text

        private void setTableDisplayText()
        {
            this._DataColumn.DataTable().setDisplayTextByRestriction();
            this._iTableSettings.SetTabName(this._DataColumn.DataTable().Alias(), this._DataColumn.DataTable().DisplayText);
            this._TextBoxTableDisplayText.Text = this._DataColumn.DataTable().DisplayText;
        }

        private void textBoxDisplay_Leave(object sender, EventArgs e)
        {
            this._DataColumn.DisplayText = this.textBoxDisplay.Text;
            //if (DiversityWorkbench.Entity.EntityTablesExist 
            //    && (this._DataColumn.DataTable().Type() == DataTable.TableType.Root || this._DataColumn.DataTable().Type() == DataTable.TableType.Target))
            //    DiversityWorkbench.Entity.setEntityRepresentation(this._DataColumn.Table.Name + "." + this._DataColumn.Name, this.textBoxDisplay.Text, Entity.EntityInformationField.DisplayText, true);
            Sheet.RebuildNeeded = true;
        }
        
        #endregion

        #region Default

        private void comboBoxInsertDefault_DropDown(object sender, EventArgs e)
        {

        }

        private void comboBoxInsertDefault_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxInsertDefault.DropDownStyle == ComboBoxStyle.DropDownList)
                this._DataColumn.DefaultForAdding = comboBoxInsertDefault.SelectedValue.ToString();
        }

        private void comboBoxInsertDefault_TextChanged(object sender, EventArgs e)
        {
            if (this._DataColumn.DefaultForAdding != null &&
                comboBoxInsertDefault.Text != "System.Data.DataRowView" &&
                this._DataColumn.DefaultForAdding != comboBoxInsertDefault.Text)
                Sheet.RebuildNeeded = true;
            if (this.comboBoxInsertDefault.DropDownStyle != ComboBoxStyle.DropDownList)
            {
                this._DataColumn.DefaultForAdding = comboBoxInsertDefault.Text;
                if (this._DataColumn.DefaultForAdding != comboBoxInsertDefault.Text)
                    comboBoxInsertDefault.Text = this._DataColumn.DefaultForAdding;
            }
        }
        
        #endregion   
     
        #region Filter

        private void textBoxFilter_Leave(object sender, EventArgs e)
        {
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            this._DataColumn.FilterValue = this.textBoxFilter.Text;
        }

        private void comboBoxFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxFilter.Text == "System.Data.DataRowView")
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxFilter.SelectedItem;
                    if (R != null)
                        this._DataColumn.FilterValue = R["Value"].ToString();
                }
                else
                {
                    if (this.comboBoxFilter.DropDownStyle == ComboBoxStyle.DropDownList)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxFilter.SelectedItem;
                        if (R != null)
                            this._DataColumn.FilterValue = R["Value"].ToString();
                    }
                    else
                        this._DataColumn.FilterValue = this.comboBoxFilter.Text;
                }
            }
            catch (System.Exception ex)
            {
            }
            //this._DataColumn.FilterValue = this.comboBoxFilter.Text;
        }

        private void textBoxFilterOperator_Click(object sender, EventArgs e)
        {
            if (this._DataColumn.DataTable().FilterOperator == "Ø"
                || this._DataColumn.DataTable().FilterOperator == "◊")
            {
                System.Windows.Forms.MessageBox.Show("Not possible with NULL operations (Ø or ◊) on table");
                return;
            }

            DiversityWorkbench.Spreadsheet.FormColumnFilter f = new FormColumnFilter(this._DataColumn);
            f.ShowDialog();
            this.setFilter();
            Sheet.RebuildNeeded = true;
        }

        private void setFilter()
        {
            this.textBoxFilterOperator.Text = this._DataColumn.FilterOperator;
            this.textBoxFilter.Text = this._DataColumn.FilterValue;
            this.comboBoxFilter.Text = this._DataColumn.FilterValue;
        }
        
        private void textBoxFilter_Click(object sender, EventArgs e)
        {
            switch (this._DataColumn.Column.DataType.ToLower())
            {
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    DiversityWorkbench.Forms.FormGetDate f = new Forms.FormGetDate(false);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        this.textBoxFilter.Text = f.Date.Year.ToString() + "-" + f.Date.Month.ToString() + "-" + f.Date.Day.ToString();
                    }
                    break;
            }
        }

        private void comboBoxFilter_Click(object sender, EventArgs e)
        {
            if (this._DataColumn.DataTable().FilterOperator == "Ø"
                || this._DataColumn.DataTable().FilterOperator == "◊")
            {

                System.Windows.Forms.MessageBox.Show("Not possible with NULL operations (Ø or ◊) on table");
                this.textBoxFilterOperator.Focus();
                this.comboBoxFilter.Text = "";
                return;
            }

            switch (this._DataColumn.Column.DataType.ToLower())
            {
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    DiversityWorkbench.Forms.FormGetDate f = new Forms.FormGetDate(false);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        this.comboBoxFilter.Text = f.Date.Year.ToString() + "-" + f.Date.Month.ToString() + "-" + f.Date.Day.ToString();
                    }
                    break;
            }
        }

        #endregion

        #region Order

        private void buttonOrderDirection_Click(object sender, EventArgs e)
        {
            if (this._DataColumn.DataTable().FilterOperator == "Ø"
                || this._DataColumn.DataTable().FilterOperator == "◊")
            {
                System.Windows.Forms.MessageBox.Show("Not possible with NULL operations (Ø or ◊) on table");
                return;
            }

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
            this.setOrder();
            Sheet.RebuildNeeded = true;
        }

        private void setOrder()
        {
            switch (this._DataColumn.OrderDirection)
            {
                case DataColumn.OrderByDirection.none:
                    this.buttonOrderDirection.Text = "-";
                    break;
                case DataColumn.OrderByDirection.ascending:
                    this.buttonOrderDirection.Text = "↑";
                    break;
                case DataColumn.OrderByDirection.descending:
                    this.buttonOrderDirection.Text = "↓";
                    break;
            }
            if (this._DataColumn.OrderSequence != null)
                this.labelOrderSequence.Text = this._DataColumn.OrderSequence.ToString();
            else this.labelOrderSequence.Text = "";
        }

        #endregion

        #region Setting the lookup source

        private void comboBoxFilter_Enter(object sender, EventArgs e)
        {
            if (this.comboBoxFilter.DataSource == null)
                this.setLookupSource(this.comboBoxFilter);
        }

        private void comboBoxInsertDefault_Enter(object sender, EventArgs e)
        {
            if (this.comboBoxInsertDefault.DataSource == null)
                this.setLookupSource(this.comboBoxInsertDefault);
        }

        private void comboBoxRestriction_Enter(object sender, EventArgs e)
        {
            if (this.comboBoxRestriction.DataSource == null)
                this.setLookupSource(this.comboBoxRestriction);
        }
        
        #endregion

        #endregion

    }
}
