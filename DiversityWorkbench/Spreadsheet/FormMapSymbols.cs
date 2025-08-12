using DiversityWorkbench.DwbManual;
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
    public partial class FormMapSymbols : Form, iMapSymbolForm
    {

        #region Properties

        private Sheet _Sheet;
        private Setting _Setting;
        private bool _ReadOnly;

        #endregion

        #region Construction and init

        public FormMapSymbols(Sheet Sheet, Setting Setting, bool ReadOnly = false)
        {
            InitializeComponent();
            this._Sheet = Sheet;
            this._Setting = Setting;
            this._ReadOnly = ReadOnly;
            this.initForm();
        }

        private void initForm()
        {
            try
            {
                this.SetTransparency(this._Sheet.GeographyTransparency);
                // Symbols
                if (this._Sheet.GeographySymbolTableAlias.Length > 0 && this._Sheet.GeographySymbolColumn.Length > 0)
                {
                    this.InitSymbols(this._Sheet.GeographySymbolTableAlias, this._Sheet.GeographySymbolColumn);
                }
                else
                    this.labelSymbolTable.BackColor = System.Drawing.Color.Pink;
                // Colors
                if (this._Sheet.GeographyColorTableAlias.Length > 0 && this._Sheet.GeographyColorColumn.Length > 0)
                {
                    this.InitColors(this._Sheet.GeographyColorTableAlias, this._Sheet.GeographyColorColumn);
                }
                else
                    this.labelColorsTable.BackColor = System.Drawing.Color.Pink;
                if (this._Sheet.GeographyKeyColumn.Length > 0 && this._Sheet.GeographyKeyTableAlias.Length > 0)
                    this.InitFilterSource(this._Sheet.GeographyKeyTableAlias, this._Sheet.GeographyKeyColumn);
                this.setFilterUsage();
                this.InitSymbolSize();//this._Sheet.GeographySymbolSizeTableAlias, this._Sheet.GeographySymbolSizeColumn);
                this.setMapPath(this._Sheet.GeographyMap);
                this.initEvaluation();
                this.checkBoxShowDetailsInMap.Checked = this._Sheet.ShowDetailsInMap;
                this.checkBoxKeepLastSymbol.Checked = this._Sheet.KeepLastSymbol;
                this.tabControlMain.TabPages.Remove(this.tabPageFilter);
                if (this._ReadOnly)
                {
                    this.numericUpDownTransparency.Enabled = false;
                    this.buttonSaveSettings.Enabled = false;
                    this.trackBarTransparency.Enabled = false;
                    this.tableLayoutPanelColors.Enabled = false;
                    this.tableLayoutPanelEvaluation.Enabled = false;
                    this.tableLayoutPanelFilter.Enabled = false;
                    this.tableLayoutPanelMap.Enabled = false;
                    this.tableLayoutPanelSymbols.Enabled = false;
                    this.comboBoxColorsColumn.Items.Add(this._Sheet.GeographyColorColumn);
                    if (this.comboBoxSymbolColumn.DataSource != null)
                    {
                        System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxSymbolColumn.DataSource;
                        int i = 0;
                        foreach (System.Data.DataRow r in dt.Rows)
                        {
                            if (r[0].ToString() == this._Sheet.GeographySymbolColumn)
                            {
                                this.comboBoxSymbolColumn.SelectedIndex = i;
                                break;
                            }
                            i++;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Symbols

        private void InitSymbols(string TableAlias, string SymbolColumn)
        {
            this.InitSymbolTable(TableAlias);
            this.setSymbolColumn(SymbolColumn);
        }

        private void InitSymbolSource(string SymbolSourceTable, string SymbolSourceColumn)
        {
            this.InitSymbols();
        }

        private void InitSymbolTable(string TableAlias)
        {
            this._Sheet.GeographySymbolTableAlias = TableAlias;
            if (this.comboBoxSymbolTable.Items.Count == 0)
            {
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                {
                    this.comboBoxSymbolTable.Items.Add(KV.Key);
                    if (KV.Value == TableAlias)
                    {
                        this.comboBoxSymbolTable.SelectedIndex = i;
                    }
                    i++;
                }
            }
            try
            {
                this.comboBoxSymbolColumn.DataSource = this.TableColumnTable(this._Sheet.GeographySymbolTableAlias);
                this.comboBoxSymbolColumn.DisplayMember = "Display";
                this.comboBoxSymbolColumn.ValueMember = "Value";
                this.comboBoxSymbolColumn.Enabled = true;
            }
            catch (System.Exception ex)
            { }
        }

        private void comboBoxSymbolTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string Alias = "";
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().DisplayText == this.comboBoxSymbolTable.SelectedItem.ToString())
                {
                    Alias = DC.Value.DataTable().Alias();
                    this.InitSymbolTable(Alias);
                    this.labelSymbolTable.BackColor = System.Drawing.Color.Transparent;
                    this.labelSymbolColumn.BackColor = System.Drawing.Color.Pink;
                    break;
                }
            }
        }

        private void comboBoxSymbolColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxSymbolColumn.SelectedItem;
            int i = int.Parse(R[0].ToString());
            string Column = this._Sheet.SelectedColumns()[i].Column.Name;
            this._Sheet.GeographySymbolSourceRestriction = "";
            // try to set the restriction
            DiversityWorkbench.Spreadsheet.DataTable DT = this._Sheet.SelectedColumns()[i].DataTable();
            if (DT.RestrictionColumns.Count == 1)
            {
                Spreadsheet.DataColumn DC = DT.DataColumns()[DT.RestrictionColumns[0]];
                if (DC.RestrictionValue != null && DC.RestrictionValue.Length > 0)
                {
                    this._Sheet.GeographySymbolSourceRestriction = "[" + DT.RestrictionColumns[0] + "] = " + DC.RestrictionValue;
                }
            }

            this.setSymbolColumn(Column);
            this.labelSymbolColumn.BackColor = System.Drawing.Color.Transparent;
        }

        private void setSymbolColumn(string Column)
        {
            this._Sheet.GeographySymbolColumn = Column;
            //this._Sheet.MapSymbols().Clear(); da sind sonst alle weg ...
            try
            {
                System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxSymbolColumn.DataSource;
                int i = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string SymbolColumn = this._Sheet.DataTables()[this._Sheet.GeographySymbolTableAlias].DataColumns()[this._Sheet.GeographySymbolColumn].DisplayText;
                    if (R[1].ToString() == SymbolColumn)
                    {
                        this.comboBoxSymbolColumn.SelectedIndex = i;
                        break;
                    }
                    i++;
                }
            }
            catch { }
            this.InitSymbols();
        }

        private void comboBoxSymbolTable_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxSymbolTable.Items.Count == 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                {
                    this.comboBoxSymbolTable.Items.Add(KV.Key);
                }
            }
        }

        private void InitSymbols()
        {
            //this.groupBoxSymbols.Controls.Clear();
            try
            {
                if (this._Sheet.GeographySymbolSourceColumn.Length > 0 && this._Sheet.GeographySymbolSourceTable.Length > 0)
                {
                    this.buttonSymbolFindSource.Image = DiversityWorkbench.Properties.Resources.FilterClear;
                    this.toolTip.SetToolTip(this.buttonSymbolFindSource, "Remove the source for the symbol values");
                }
                else
                {
                    this.buttonSymbolFindSource.Image = DiversityWorkbench.Properties.Resources.Find;
                    this.toolTip.SetToolTip(this.buttonSymbolFindSource, "Find the source for the symbol values");
                }

                //this._Sheet.ResetMapSymbols();
                this.panelSymbols.Controls.Clear();
                this.groupBoxSymbols.Text = "Symbols for values in column " + this._Sheet.GeographySymbolColumn;
                System.Data.DataTable dt = new System.Data.DataTable();
                bool MissingSymbolInList = false;
                if (this._Sheet.getDataDistinctContentForMapSymbols(ref dt))
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Value = "";
                        if (!R[0].Equals(System.DBNull.Value) && R[0].ToString().Length > 0)
                            Value = R[0].ToString();
                        else
                            MissingSymbolInList = true;

                        if (this._Sheet.MapSymbols().ContainsKey(Value))
                        {
                            MapSymbol MS = this._Sheet.MapSymbols()[Value];
                            DiversityWorkbench.Spreadsheet.UserControlSetMapSymbol U = new UserControlSetMapSymbol(ref this._Sheet, ref MS);
                            U.Dock = DockStyle.Top;
                            this.panelSymbols.Controls.Add(U);
                            U.BringToFront();
                        }
                        else
                        {
                            MapSymbol MS = new MapSymbol(Value, 1, "Circle");
                            this._Sheet.MapSymbols().Add(Value, MS);
                            DiversityWorkbench.Spreadsheet.UserControlSetMapSymbol U = new UserControlSetMapSymbol(ref this._Sheet, ref MS);
                            U.Dock = DockStyle.Top;
                            this.panelSymbols.Controls.Add(U);
                            U.BringToFront();
                        }
                    }
                }
                if (MissingSymbolInList)
                {
                    this.panelSymbolMissingValue.Visible = false;
                }
                else
                {
                    this.panelSymbolMissingValue.Controls.Clear();
                    //string MissingValue = "";
                    MapSymbol Missing = this._Sheet.MapSymbolForMissing;// new MapSymbol(MissingValue, 1, "Circle");
                    if (!this._Sheet.MapSymbols().ContainsKey(""))
                        this._Sheet.MapSymbols().Add("", Missing);
                    DiversityWorkbench.Spreadsheet.UserControlSetMapSymbol Umissing = new UserControlSetMapSymbol(ref this._Sheet, ref Missing);
                    Umissing.Dock = DockStyle.Fill;
                    this.panelSymbolMissingValue.Controls.Add(Umissing);
                    this.panelSymbolMissingValue.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonSymbolFindSource_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._Sheet.GeographySymbolSourceTable.Length > 0 || this._Sheet.GeographySymbolSourceColumn.Length > 0)
                {
                    this._Sheet.GeographySymbolSourceColumn = "";
                    this._Sheet.GeographySymbolSourceTable = "";
                    this._Sheet.GeographySymbolSourceRestriction = "";
                    this._Sheet.ResetMapSymbols();
                    this.InitSymbols();
                }
                else
                {
                    string Message = "";
                    this._Sheet.ResetMapSymbols();

                    // try to get list
                    bool DefaultDetected = false;
                    try
                    {
                        string SymbolColumn = this._Sheet.GeographySymbolColumn;
                        string SymbolTable = this._Sheet.DataTables()[this._Sheet.GeographySymbolTableAlias].Name;
                        if (SymbolColumn == "AnalysisResult" && SymbolTable == "IdentificationUnitAnalysis")
                        {
                            // getting the ID of the Analysis
                            System.Data.DataRow[] RR = this._Sheet.DT().Select("", "AnalysisID DESC");
                            string AnalysisID = RR[0]["AnalysisID"].ToString();
                            string sql = "SELECT AnalysisResult FROM AnalysisResult WHERE AnalysisID = " + AnalysisID +
                                "ORDER BY AnalysisResult";
                            System.Data.DataTable dtResult = new System.Data.DataTable();
                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(sql, ref dtResult, ref Message);
                            if (dtResult.Rows.Count > 0)
                            {
                                sql = "SELECT TOP (1) DisplayText FROM Analysis WHERE AnalysisID = " + AnalysisID;
                                string Analysis = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql);
                                Message = "";
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    Message += "\r\n\t" + dtResult.Rows[i][0].ToString();
                                    if (i > 10)
                                    {
                                        Message += "\r\n\t...";
                                        break;
                                    }
                                }
                                if (System.Windows.Forms.MessageBox.Show("Use values for " + Analysis + " from table AnalysisResult:" + Message, "Use analysis results", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    this._Sheet.GeographySymbolSourceColumn = "AnalysisResult";
                                    this._Sheet.GeographySymbolSourceTable = "AnalysisResult";
                                    this._Sheet.GeographySymbolSourceRestriction = "AnalysisID = " + AnalysisID;
                                    this.InitSymbols();
                                    DefaultDetected = true;
                                }
                            }
                        }
                    }
                    catch { }

                    // Manual setting of the list
                    if (!DefaultDetected)
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "select t.TABLE_NAME from INFORMATION_SCHEMA.TABLES t " +
                            "where t.TABLE_TYPE = 'BASE TABLE'  " +
                            "and t.TABLE_NAME not like '%_log' " +
                            "order by t.TABLE_NAME";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Please select the source table for the symbol values", true);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            SQL = "SELECT * FROM [" + f.SelectedString + "]";
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            DiversityWorkbench.Forms.FormEditTable fe = new DiversityWorkbench.Forms.FormEditTable(ad, "Source for symbols", "Please select the column containing the values of the symbols", false, true);
                            fe.ShowRowHeader(false);
                            fe.ShowDialog();
                            if (fe.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                this._Sheet.GeographySymbolSourceColumn = fe.SelectedColumn();
                                this._Sheet.GeographySymbolSourceTable = f.SelectedString;
                                this._Sheet.GeographySymbolSourceRestriction = fe.Filter();
                                this.InitSymbols();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Symbol size

        private void InitSymbolSize()
        {
            this.SuspendLayout();
            try
            {
                if (this._Sheet.GeographySymbolSizeTableAlias.Length > 0)
                {
                    this.InitSymbolSizeTable(this._Sheet.GeographySymbolSizeTableAlias);
                    if (this._Sheet.GeographySymbolSizeColumn.Length > 0)
                    {
                        this.setSymbolSizeColumn(this._Sheet.GeographySymbolSizeColumn);
                    }
                }
                if (this._Sheet.GeographySymbolSize > 0)
                {
                    // setting the text for the factor
                    double SymbolSizeAfter = this._Sheet.GeographySymbolSize % 1;
                    string MaskedTextAfterPoint = (SymbolSizeAfter * 100).ToString();
                    string MaskedTextBeforePoint = (this._Sheet.GeographySymbolSize - SymbolSizeAfter).ToString();
                    while (MaskedTextBeforePoint.Length < 2)
                        MaskedTextBeforePoint = " " + MaskedTextBeforePoint;
                    while (MaskedTextAfterPoint.Length < 2)
                        MaskedTextAfterPoint += "0";
                    this.maskedTextBoxSymbolSize.Text = MaskedTextBeforePoint + MaskedTextAfterPoint;// this._Sheet.GeographySymbolSize.ToString();
                    this.maskedTextBoxSymbolSizeSingle.Text = MaskedTextBeforePoint + MaskedTextAfterPoint;
                }
                else
                {
                    this.maskedTextBoxSymbolSize.Text = " 000";
                    this.maskedTextBoxSymbolSizeSingle.Text = " 000";
                }
                this.checkBoxSymbolSizeLinkedToColumnValue.Checked = this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.labelSymbolSizeColumn.Visible = this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.labelSymbolSizeTable.Visible = this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.comboBoxSymbolSizeColumn.Visible = this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.comboBoxSymbolSizeTable.Visible = this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.maskedTextBoxSymbolSize.Visible = this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.labelSymbolSize.Visible = this._Sheet.GeographySymbolSizeLinkedToColumn;
                if (this._Sheet.GeographySymbolSizeLinkedToColumn && !this._Sheet.GeographySymbolSizeCanBeLinkedToColumnValue())
                {
                    if (this._Sheet.GeographySymbolSizeTableAlias.Length == 0)
                        this.labelSymbolSizeTable.BackColor = System.Drawing.Color.Pink;
                    else
                        this.labelSymbolSizeTable.BackColor = System.Drawing.SystemColors.Window;

                    if (this._Sheet.GeographySymbolSizeColumn.Length == 0)
                        this.labelSymbolSizeColumn.BackColor = System.Drawing.Color.Pink;
                    else
                        this.labelSymbolSizeColumn.BackColor = System.Drawing.SystemColors.Window;
                    if (this._Sheet.GeographySymbolSize == 0)
                        this.maskedTextBoxSymbolSize.BackColor = System.Drawing.Color.Pink;
                    else
                        this.maskedTextBoxSymbolSize.BackColor = System.Drawing.SystemColors.Window;
                }
                else
                {
                    this.labelSymbolSizeColumn.BackColor = System.Drawing.SystemColors.Window;
                    this.labelSymbolSizeTable.BackColor = System.Drawing.SystemColors.Window;
                    this.maskedTextBoxSymbolSize.BackColor = System.Drawing.SystemColors.Window;
                }

                this.labelSymbolSizeSingle.Visible = !this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.maskedTextBoxSymbolSizeSingle.Visible = !this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();
                this.buttonSymbolSizeSingle.Visible = !this._Sheet.GeographySymbolSizeLinkedToColumn;// .GeographySymbolSizeCanBeLinkedToColumnValue();

                foreach (System.Windows.Forms.Control C in this.panelSymbols.Controls)
                {
                    try
                    {
                        Spreadsheet.UserControlSetMapSymbol U = (Spreadsheet.UserControlSetMapSymbol)C;
                        U.SizeEnabled(!this._Sheet.GeographySymbolSizeLinkedToColumn);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                if (this.panelSymbolMissingValue.Controls.Count == 1)
                {
                    if (this.panelSymbolMissingValue.Controls[0].GetType() == typeof(Spreadsheet.UserControlSetMapSymbol))
                    {
                        Spreadsheet.UserControlSetMapSymbol UC = (Spreadsheet.UserControlSetMapSymbol)this.panelSymbolMissingValue.Controls[0];
                        UC.SizeEnabled(!this._Sheet.GeographySymbolSizeLinkedToColumn);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.ResumeLayout();
        }

        //private void InitSymbolSize(string TableAlias, string Column)
        //{
        //    this.InitSymbolSizeTable(TableAlias);
        //    this.setSymbolSizeColumn(Column);
        //    double SymbolSizeAfter = this._Sheet.GeographySymbolSize % 1;
        //    string MaskedTextAfterPoint = (SymbolSizeAfter*100).ToString();
        //    string MaskedTextBeforePoint = (this._Sheet.GeographySymbolSize - SymbolSizeAfter).ToString();
        //    while (MaskedTextBeforePoint.Length < 2)
        //        MaskedTextBeforePoint = " " + MaskedTextBeforePoint;
        //    while (MaskedTextAfterPoint.Length < 2)
        //        MaskedTextAfterPoint += "0";
        //    this.maskedTextBoxSymbolSize.Text = MaskedTextBeforePoint + MaskedTextAfterPoint;// this._Sheet.GeographySymbolSize.ToString();
        //}

        private void InitSymbolSizeTable(string TableAlias)
        {
            bool AliasChanged = false;
            if (TableAlias != this._Sheet.GeographySymbolSizeTableAlias)
                AliasChanged = true;
            this._Sheet.GeographySymbolSizeTableAlias = TableAlias;
            if (this.comboBoxSymbolSizeTable.Items.Count == 0)
            {
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                {
                    this.comboBoxSymbolSizeTable.Items.Add(KV.Key);
                    if (KV.Value == TableAlias)
                    {
                        this.comboBoxSymbolSizeTable.SelectedIndex = i;
                    }
                    i++;
                }
            }
            if (AliasChanged || this.comboBoxSymbolSizeColumn.DataSource == null)
            {
                try
                {
                    this.comboBoxSymbolSizeColumn.DataSource = this.TableColumnTable(this._Sheet.GeographySymbolSizeTableAlias);
                    this.comboBoxSymbolSizeColumn.DisplayMember = "Display";
                    this.comboBoxSymbolSizeColumn.ValueMember = "Value";
                    this.comboBoxSymbolSizeColumn.Enabled = true;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void setSymbolSizeColumn(string Column)
        {
            if (this._Sheet.GeographySymbolSizeColumn != Column)
            {
                this._Sheet.GeographySymbolSizeColumn = Column;
                this.InitSymbols();
            }
            else if (this.comboBoxSymbolSizeColumn.SelectedIndex == -1 && this.comboBoxSymbolSizeColumn.DataSource != null)
            {
                try
                {
                    System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxSymbolSizeColumn.DataSource;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][1].ToString() == this._Sheet.GeographySymbolSizeColumn)
                        {
                            this.comboBoxSymbolSizeColumn.SelectedIndex = i;
                            break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private void checkBoxSymbolSizeLinkedToColumnValue_Click(object sender, EventArgs e)
        {
            this._Sheet.GeographySymbolSizeLinkedToColumn = this.checkBoxSymbolSizeLinkedToColumnValue.Checked;
            //if (this.checkBoxSymbolSizeLinkedToColumnValue.Checked && this._Sheet.GeographySymbolSize == 0)
            //{
            //        this._Sheet.GeographySymbolSize = 1;
            //        this._Sheet.GeographySymbolSize = 0;
            //}
            //else if (this.checkBoxSymbolSizeLinkedToColumnValue.Checked && this._Sheet.GeographySymbolSize == 0)
            //{
            //}
            this.InitSymbolSize();
            //this.setSymbolSizeLinkedToColumn();
        }

        //private void setSymbolSizeLinkedToColumn()
        //{
        //    this.maskedTextBoxSymbolSize.Enabled = checkBoxSymbolSizeLinkedToColumnValue.Checked;
        //    double d;
        //    if (checkBoxSymbolSizeLinkedToColumnValue.Checked)
        //    {
        //        if (double.TryParse(this.maskedTextBoxSymbolSize.Text, out d) && d > 0)
        //            this._Sheet.GeographySymbolSize = d;
        //        else
        //        {
        //            this.maskedTextBoxSymbolSize.Text = "0100";
        //            this._Sheet.GeographySymbolSize = 1;
        //        }
        //    }
        //    else
        //    {
        //        d = 0;
        //        this._Sheet.GeographySymbolSize = d;
        //        this.maskedTextBoxSymbolSize.Text = " 000";// d.ToString();
        //    }
        //    this.InitSymbolSize();
        //}

        private void maskedTextBoxSymbolSize_TextChanged(object sender, EventArgs e)
        {
            double d;
            bool SizeChanged = false;
            if (double.TryParse(this.maskedTextBoxSymbolSize.Text, out d))
            {
                if (this._Sheet.GeographySymbolSize != d)
                {
                    this._Sheet.GeographySymbolSize = d;
                    SizeChanged = true;
                }
            }
            else
            {
                this._Sheet.GeographySymbolSize = 0;
            }
            if (SizeChanged)
                this.InitSymbolSize();
        }

        private void comboBoxSymbolSizeTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string Alias = "";
            bool NewAlias = false;
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().DisplayText == this.comboBoxSymbolSizeTable.SelectedItem.ToString())
                {
                    Alias = DC.Value.DataTable().Alias();
                    if (Alias != this._Sheet.GeographySymbolSizeTableAlias)
                        NewAlias = true;
                    this.InitSymbolSizeTable(Alias);
                    break;
                }
            }
        }

        private void comboBoxSymbolSizeTable_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxSymbolSizeTable.Items.Count == 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                {
                    this.comboBoxSymbolSizeTable.Items.Add(KV.Key);
                }
            }
        }

        private void comboBoxSymbolSizeColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxSymbolSizeColumn.SelectedItem;
            this._Sheet.GeographySymbolSizeColumn = R[1].ToString();
            this.InitSymbolSize();
        }

        private void buttonSymbolSizeSingle_Click(object sender, EventArgs e)
        {
            try
            {
                double d = 0;
                if (double.TryParse(this.maskedTextBoxSymbolSizeSingle.Text, out d) && d > 0)
                {
                    foreach (System.Windows.Forms.Control C in this.panelSymbols.Controls)
                    {
                        Spreadsheet.UserControlSetMapSymbol U = (Spreadsheet.UserControlSetMapSymbol)C;
                        U.setSymbolSize(d);
                    }
                    if (this.panelSymbolMissingValue.Controls.Count == 1)
                    {
                        if (this.panelSymbolMissingValue.Controls[0].GetType() == typeof(Spreadsheet.UserControlSetMapSymbol))
                        {
                            Spreadsheet.UserControlSetMapSymbol UC = (Spreadsheet.UserControlSetMapSymbol)this.panelSymbolMissingValue.Controls[0];
                            UC.setSymbolSize(d);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Color

        private void InitColors(string TableAlias, string ColorColumn)
        {
            try
            {
                this.InitColorTables(TableAlias);
                this.setColorColumn(ColorColumn);
                System.Collections.Generic.SortedDictionary<double, MapColor> SD = new SortedDictionary<double, MapColor>();
                foreach (MapColor MC in this._Sheet.MapColors())
                {
                    double SortingValue = MC.SortingValue();
                    if (SD.ContainsKey(MC.SortingValue()))
                    {
                        System.Collections.Generic.List<double> SortingValues = new List<double>();
                        foreach (MapColor mc in this._Sheet.MapColors())
                        {
                            SortingValues.Add(mc.SortingValue());
                        }
                        int i = 1;
                        while (SortingValues.Contains(i))
                            i++;
                        MC.SetSortingValue(i);
                        SortingValue = MC.SortingValue();
                    }
                    SD.Add(MC.SortingValue(), MC);
                }
                this.groupBoxColors.SuspendLayout();
                foreach (System.Windows.Forms.Control C in this.groupBoxColors.Controls)
                    C.Dispose();
                this.groupBoxColors.Controls.Clear();
                foreach (System.Collections.Generic.KeyValuePair<double, MapColor> KV in SD)
                    this.AddMapColorControl(KV.Value);
                this.groupBoxColors.ResumeLayout();
                this.buttonColorAdd.Enabled = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void InitColorTables(string TableAlias)
        {
            if (this.comboBoxColorsTable.Items.Count == 0)
            {
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                {
                    this.comboBoxColorsTable.Items.Add(KV.Key);
                    if (KV.Value == TableAlias)
                    {
                        this.comboBoxColorsTable.SelectedIndex = i;
                        this.InitColorColumnSource(KV.Value);
                    }
                    i++;
                }
            }
        }

        private void comboBoxColorsTable_DropDown(object sender, EventArgs e)
        {
            this.InitColorTables("");
        }

        private void InitColorColumnSource(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyColorTableAlias = TableAlias;
                this.comboBoxColorsColumn.DataSource = this.TableColumns(this._Sheet.GeographyColorTableAlias);
                this.comboBoxColorsColumn.Enabled = true;
                if (this._Sheet.GeographyColorColumn != null && this._Sheet.GeographyColorColumn.Length > 0)
                {
                    int i = 0;
                    foreach (System.Object I in this.comboBoxColorsColumn.Items)
                    {
                        if (I.ToString() == this._Sheet.GeographyColorColumn)
                        {
                            this.comboBoxColorsColumn.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void comboBoxColorsTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().DisplayText == this.comboBoxColorsTable.SelectedItem.ToString())
                {
                    string Alias = DC.Value.DataTable().Alias();
                    this.InitColorColumnSource(Alias);
                    this.labelColorsTable.BackColor = System.Drawing.Color.Transparent;
                    this.labelColorsColumn.BackColor = System.Drawing.Color.Pink;
                    break;
                }
            }
        }

        private void comboBoxColorsColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.setColorColumn(this.comboBoxColorsColumn.SelectedItem.ToString());
            this.labelColorsColumn.BackColor = System.Drawing.Color.Transparent;
        }

        private void setColorColumn(string ColorColumn)
        {
            if (this._Sheet.GeographyColorColumn != ColorColumn)
            {
                this._Sheet.GeographyColorColumn = ColorColumn;
                this.groupBoxColors.Text = "Colors for values in column " + this._Sheet.GeographyColorColumn;
                this.buttonColorAdd.Enabled = true;
                this.buttonColorAdd.BackColor = System.Drawing.Color.Pink;
                foreach (System.Windows.Forms.Control C in this.panelColors.Controls)
                {
                    Spreadsheet.UserControlSetMapColor U = (Spreadsheet.UserControlSetMapColor)C;
                    U.RemoveColorControl();
                }
            }
        }

        private void AddMapColorControl(MapColor MapColor)
        {
            UserControlSetMapColor U = new UserControlSetMapColor(this.panelColors, ref this._Sheet, ref MapColor);
            U.Dock = DockStyle.Top;
            U.SendToBack();
            this.groupBoxColors.Controls.Add(U);
        }

        private void buttonColorAdd_Click(object sender, EventArgs e)
        {
            MapColor MC = new MapColor(System.Windows.Media.Brushes.Red, "=", "", "");
            this._Sheet.MapColors().Add(MC);
            this.AddMapColorControl(MC);
            this.buttonColorAdd.BackColor = System.Drawing.Color.Transparent;
        }

        private void buttonColorRefresh_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control C in this.panelColors.Controls)
                C.Dispose();
            this.panelColors.Controls.Clear();
            this.InitColors(this._Sheet.GeographyColorTableAlias, this._Sheet.GeographyColorColumn);
        }

        #endregion

        #region Transparency

        private void trackBarTransparency_Scroll(object sender, EventArgs e)
        {
            this.SetTransparency(this.trackBarTransparency.Value);
            //his.SetTransparency((byte)((float)255 * ((float)100 - this.trackBarTransparency.Value) / (float)100));
        }

        private void numericUpDownTransparency_ValueChanged(object sender, EventArgs e)
        {
            this.SetTransparency((int)this.numericUpDownTransparency.Value);
            //this.SetTransparency((byte)(255.0 * (100.0 - (double)this.numericUpDownTransparency.Value) / 100.0));
        }

        private void SetTransparency(int Transparency)
        {
            try
            {
                if (this._Sheet.GeographyTransparency != Transparency)
                {
                    this._Sheet.GeographyTransparency = (byte)Transparency;
                }
                this.trackBarTransparency.Value = Transparency;
                this.numericUpDownTransparency.Value = Transparency;
                float factor = (float)(((float)255 - (float)Transparency) / (float)255);
                this.numericUpDownTransparency.BackColor = DiversityWorkbench.Forms.FormFunctions.paleColor(System.Drawing.Color.Red, factor);
            }
            catch (System.Exception ex)
            {
            }
        }

        //private void SetTransparency(byte Transparency)
        //{
        //    try
        //    {
        //        if (this._Sheet.GeographyTransparency != Transparency)
        //        {
        //            this._Sheet.GeographyTransparency = Transparency;
        //        }
        //        double Value = (double)this._Sheet.GeographyTransparency;
        //        Value = 99.0 - ((Value / 255.0) * 100.0);
        //        this.trackBarTransparency.Value = (int)Value;
        //        this.numericUpDownTransparency.Value = (int)Value;
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //}

        #endregion

        #region Map

        private void buttonFindMap_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = "Images|*.png";
            this.openFileDialog.InitialDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Maps";
            //this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.ShowDialog();
            if (this.openFileDialog.FileName.Length > 0)
            {
                string XmlConfigFilePath = this.openFileDialog.FileName;//.ToLower().Replace(".jpg", ".xml");
                XmlConfigFilePath = XmlConfigFilePath.Substring(0, XmlConfigFilePath.Length - 4) + ".xml";
                System.IO.FileInfo FI = new System.IO.FileInfo(XmlConfigFilePath);
                if (FI.Exists)
                    this.setMapPath(this.openFileDialog.FileName);
                else
                    System.Windows.Forms.MessageBox.Show("No valid map");
            }
        }

        private void setMapPath(string Path)
        {
            this._Sheet.GeographyMap = Path;
            this.textBoxMapFile.Text = Path;
            if (Path.Length > 0)
            {
                try
                {
                    System.Drawing.Bitmap B = new Bitmap(Path);
                    this.pictureBoxMap.Image = B;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, Path);
                }
            }
        }

        private void checkBoxShowDetailsInMap_Click(object sender, EventArgs e)
        {
            this._Sheet.ShowDetailsInMap = !this._Sheet.ShowDetailsInMap;
            this.checkBoxShowDetailsInMap.Checked = this._Sheet.ShowDetailsInMap;
        }

        private void checkBoxShowAllDetailsInMap_Click(object sender, EventArgs e)
        {
            this._Sheet.ShowAllDetailsInMap = !this._Sheet.ShowAllDetailsInMap;
            this.checkBoxShowAllDetailsInMap.Checked = this._Sheet.ShowAllDetailsInMap;
        }

        #endregion

        #region Filter

        private void InitFilterSource(string TableAlias, string SymbolColumn)
        {
            this.InitFilterTable(TableAlias);
            this.setFilterColumn(SymbolColumn);
        }

        private void InitFilterTable(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyKeyTableAlias = TableAlias;
                if (this.comboBoxFilterTable.Items.Count == 0)
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                    {
                        this.comboBoxFilterTable.Items.Add(KV.Key);
                        if (KV.Value == TableAlias)
                        {
                            this.comboBoxFilterTable.SelectedIndex = i;
                        }
                        i++;
                    }
                }
                try
                {
                    this.comboBoxFilterColumn.DataSource = this.TableColumnTable(this._Sheet.GeographyKeyTableAlias);
                    this.comboBoxFilterColumn.DisplayMember = "Display";
                    this.comboBoxFilterColumn.ValueMember = "Value";
                    this.comboBoxFilterColumn.Enabled = true;
                }
                catch (System.Exception ex)
                { }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void InitFilterColumnSource(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyKeyTableAlias = TableAlias;
                this.InitColumnSource(TableAlias, this.comboBoxFilterColumn);
            }
            catch (System.Exception ex)
            { }
        }

        private void setFilterColumn(string Column)
        {
            this._Sheet.GeographyKeyColumn = Column;
            System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxFilterColumn.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString() == this._Sheet.GeographyKeyColumn)
                {
                    this.comboBoxFilterColumn.SelectedIndex = i;
                    break;
                }
            }
            this.buttonFilterAdd.Enabled = true;
            this.initFilterList();
        }

        private void buttonFilterAdd_Click(object sender, EventArgs e)
        {
            // potential filters
            System.Collections.Generic.Dictionary<string, MapFilter.FilterTypes> Filter = new Dictionary<string, MapFilter.FilterTypes>();
            Filter.Add("Geography", MapFilter.FilterTypes.Geography);
            Filter.Add("Color", MapFilter.FilterTypes.Color);
            Filter.Add("Symbol", MapFilter.FilterTypes.Symbol);

            // already inserted
            System.Collections.Generic.List<MapFilter.FilterTypes> A = new List<MapFilter.FilterTypes>();
            foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> KV in this._Sheet.MapFilterList)
                A.Add(KV.Value.FilterType());

            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, MapFilter.FilterTypes> KV in Filter)
            {
                if (!A.Contains(KV.Value))
                    L.Add(KV.Key);
            }
            if (L.Count > 0)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(L, "Filter type", "Please select the type of the filter", true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    MapFilter MF = new MapFilter(Filter[f.SelectedString], this._Sheet);
                    this._Sheet.AddMapFilter(MF);
                    this.initFilterList();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No filter type left");
            }
        }

        public void initFilterList()
        {
            this.panelFilter.Controls.Clear();
            if (this._Sheet.MapFilterList.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> KV in this._Sheet.MapFilterList)
                {
                    UserControlMapFilter U = new UserControlMapFilter(KV.Value, this, this._Sheet);
                    U.Dock = DockStyle.Top;
                    this.panelFilter.Controls.Add(U);
                    U.BringToFront();
                }
            }
        }

        private void checkBoxFilterUsage_Click(object sender, EventArgs e)
        {
            this._Sheet.GeographyUseKeyFilter = !this._Sheet.GeographyUseKeyFilter;
            this.setFilterUsage();
        }

        private void setFilterUsage()
        {
            this.comboBoxFilterColumn.Enabled = this._Sheet.GeographyUseKeyFilter;
            this.comboBoxFilterTable.Enabled = this._Sheet.GeographyUseKeyFilter;
            this.buttonFilterAdd.Enabled = this._Sheet.GeographyUseKeyFilter;
            this.checkBoxFilterUsage.Checked = this._Sheet.GeographyUseKeyFilter;
            if (this._Sheet.GeographyUseKeyFilter)
                this.labelFilterTable.BackColor = System.Drawing.Color.Pink;
            else
                this.labelFilterTable.BackColor = System.Drawing.Color.Transparent;
        }

        private void comboBoxFilterTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().DisplayText == this.comboBoxFilterTable.SelectedItem.ToString())
                {
                    string Alias = DC.Value.DataTable().Alias();
                    this.InitFilterColumnSource(Alias);
                    this.labelFilterTable.BackColor = System.Drawing.Color.Transparent;
                    this.labelFilterColumn.BackColor = System.Drawing.Color.Pink;
                    break;
                }
            }
        }

        private void comboBoxFilterTable_DropDown(object sender, EventArgs e)
        {
            this.InitFilterTable("");
        }

        private void comboBoxFilterColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxFilterColumn.SelectedItem;
            this.setFilterColumn(R[1].ToString());// this.comboBoxFilterColumn.SelectedText.ToString());
            this.labelFilterColumn.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion

        #region Common

        private System.Collections.Generic.Dictionary<string, string> _SelectedTables;
        private System.Collections.Generic.Dictionary<string, string> SelectedTables()
        {
            if (this._SelectedTables == null)
            {
                this._SelectedTables = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DataTable().DisplayText != null && !this._SelectedTables.ContainsKey(DC.Value.DataTable().DisplayText))
                    {
                        this._SelectedTables.Add(DC.Value.DataTable().DisplayText, DC.Value.DataTable().Alias());
                    }
                }
            }
            return this._SelectedTables;
        }

        private System.Collections.Generic.List<string> TableColumns(string TableAlias)
        {
            System.Collections.Generic.List<string> List = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == TableAlias && DC.Value.IsVisible && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    List.Add(DC.Value.Name);
                }
            }
            return List;
        }

        private System.Collections.Generic.Dictionary<string, string> TableColumnDictionary(string TableAlias)
        {
            System.Collections.Generic.Dictionary<string, string> D = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == TableAlias && DC.Value.IsVisible && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    D.Add(DC.Value.Name, DC.Value.DisplayText);
                }
            }
            return D;
        }

        private System.Data.DataTable TableColumnTable(string TableAlias)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataColumn cValue = new System.Data.DataColumn("Value", typeof(string));
            dt.Columns.Add(cValue);
            System.Data.DataColumn cDisplay = new System.Data.DataColumn("Display", typeof(string));
            dt.Columns.Add(cDisplay);
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == TableAlias && DC.Value.IsVisible && DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    System.Data.DataRow R = dt.NewRow();
                    R["Value"] = DC.Key;
                    R["Display"] = DC.Value.DisplayText;
                    dt.Rows.Add(R);
                }
            }
            return dt;
        }

        private void InitColumnSource(string TableAlias, System.Windows.Forms.ComboBox ComboBox)
        {
            try
            {
                ComboBox.DataSource = this.TableColumnTable(TableAlias);
                ComboBox.DisplayMember = "Display";
                ComboBox.ValueMember = "Value";
                ComboBox.Enabled = true;
            }
            catch (System.Exception ex)
            { }
        }



        #endregion

        #region Form

        private void FormMapSymbols_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._Sheet.GeographySymbolSizeLinkedToColumn && !this._Sheet.GeographySymbolSizeCanBeLinkedToColumnValue())
            {
                System.Windows.Forms.MessageBox.Show("To link the size to the values in a data column, you must specify the column and set the calcultation factor to a value above 0");
                e.Cancel = true;
            }
        }

        #endregion

        #region Legend and Feedback

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            this._Setting.WriteSettings(this._Sheet.Target());
        }

        private void buttonShowLegend_Click(object sender, EventArgs e)
        {
            FormMapLegend f = new FormMapLegend(this._Sheet);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Show();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Evaluation

        private void initEvaluation()
        {
            try
            {
                int i = 0;
                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityGazetteer"))
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnections())
                    {
                        this.comboBoxEvaluationGeographyGazetteer.Items.Add(KV.Value.DisplayText);
                        if (KV.Value.DisplayText == this._Sheet.EvaluationGazetteer())
                            this.comboBoxEvaluationGeographyGazetteer.SelectedIndex = i;
                        i++;
                    }

                }
                this.EvaluationInitWGS84Table(this._Sheet.GeographyWGS84TableAlias);
                this.EvaluationSetWGS84Column(this._Sheet.GeographyWGS84Column);

                this.EvaluationInitUnitGeoTable(this._Sheet.GeographyUnitGeoTableAlias);
                this.EvaluationSetUnitGeoColumn(this._Sheet.GeographyUnitGeoColumn);

                this.EvaluationInitFilterTable(this._Sheet.GeographyKeyTableAlias);
                this.EvaluationSetFilterColumn(this._Sheet.GeographyKeyColumn);

                this.EvaluationInitSymbols();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #region Filter

        private void comboBoxEvaluationFilterTable_DropDown(object sender, EventArgs e)
        {
            this.EvaluationInitFilterTable("");
        }

        private void EvaluationInitFilterTable(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyKeyTableAlias = TableAlias;
                if (this.comboBoxEvaluationFilterTable.Items.Count == 0)
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                    {
                        this.comboBoxEvaluationFilterTable.Items.Add(KV.Key);
                        if (KV.Value == TableAlias)
                        {
                            this.comboBoxEvaluationFilterTable.SelectedIndex = i;
                        }
                        i++;
                    }
                }
                try
                {
                    this.comboBoxEvaluationFilterColumn.DataSource = this.TableColumnTable(this._Sheet.GeographyKeyTableAlias);
                    this.comboBoxEvaluationFilterColumn.DisplayMember = "Display";
                    this.comboBoxEvaluationFilterColumn.ValueMember = "Value";
                    this.comboBoxEvaluationFilterColumn.Enabled = true;
                }
                catch (System.Exception ex)
                { }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void EvaluationInitFilterColumnSource(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyKeyTableAlias = TableAlias;
                this.InitColumnSource(TableAlias, this.comboBoxEvaluationFilterColumn);
            }
            catch (System.Exception ex)
            { }
        }

        private void comboBoxEvaluationFilterTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().DisplayText == this.comboBoxEvaluationFilterTable.SelectedItem.ToString())
                {
                    string Alias = DC.Value.DataTable().Alias();
                    this.EvaluationInitFilterColumnSource(Alias);
                    this.labelEvaluationFilterTable.BackColor = System.Drawing.Color.Transparent;
                    this.labelEvaluationFilterColumn.BackColor = System.Drawing.Color.Pink;
                    break;
                }
            }
        }

        private void comboBoxEvaluationFilterColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxEvaluationFilterColumn.SelectedItem;
            this.EvaluationSetFilterColumn(R[1].ToString());
            this.labelEvaluationFilterColumn.BackColor = System.Drawing.Color.Transparent;
        }

        private void EvaluationSetFilterColumn(string Column)
        {
            this._Sheet.GeographyKeyColumn = Column;
            System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxEvaluationFilterColumn.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString() == this._Sheet.GeographyKeyColumn)
                {
                    this.comboBoxEvaluationFilterColumn.SelectedIndex = i;
                    break;
                }
            }
            //this.buttonFilterAdd.Enabled = true;
            //this.initFilterList();
        }

        #endregion

        #region Symbols

        private System.Collections.Generic.List<string> EvaluationMissingSymbols()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Data.DataRow R in this.DtEvaluationSymbolValues().Rows)
            {
                if (!this._Sheet.EvaluationSymbolValueSequence().ContainsKey(R[0].ToString())
                    && R[0].ToString().Length > 0) // auf Wunsch von Marcel keine Leeren
                    L.Add(R[0].ToString());
            }
            // Auf Wunsch von Marcel entfernt
            //if (!L.Contains("") && !this._Sheet.EvaluationSymbolValueSequence().ContainsKey(""))
            //    L.Add("");
            return L;
        }

        private System.Data.DataTable _DtEvaluationSymbolValues;
        private System.Data.DataTable DtEvaluationSymbolValues()
        {
            if (this._DtEvaluationSymbolValues == null)
            {
                this._DtEvaluationSymbolValues = new System.Data.DataTable();
                this._Sheet.getDataDistinctContentForMapSymbols(ref this._DtEvaluationSymbolValues);
            }
            return this._DtEvaluationSymbolValues;
        }

        public void EvaluationAddSymbol(string SymbolValue)
        {
            int iNext = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this._Sheet.EvaluationSymbolValueSequence())
            {
                if (KV.Value >= iNext)
                    iNext = KV.Value + 1;
            }
            this._Sheet.EvaluationSymbolValueSequence().Add(SymbolValue, iNext);
        }

        private void toolStripButtonEvaluationSymbolsAdd_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> L = this.EvaluationMissingSymbols();
            if (L.Count > 0)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(L, "Symbol value", "Please select the value that should be added next", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.EvaluationAddSymbol(f.SelectedString);
                    this.EvaluationInitSymbols();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No further value left");
            }
        }

        private void EvaluationInitSymbols()
        {
            this.panelEvaluationSymbols.Controls.Clear();
            System.Collections.Generic.SortedDictionary<int, string> SortedEval = new SortedDictionary<int, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this._Sheet.EvaluationSymbolValueSequence())
            {
                if (!SortedEval.ContainsKey(KV.Value))
                    SortedEval.Add(KV.Value, KV.Key);
                else
                {
                    int i = 0;
                    while (SortedEval.ContainsKey(i))
                        i++;
                    SortedEval.Add(i, KV.Key);
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in SortedEval)
            {
                try
                {
                    System.Windows.Forms.Button B = new Button();
                    if (this._Sheet.MapSymbols().ContainsKey(KV.Value))
                    {
                        MapSymbol MS = this._Sheet.MapSymbols()[KV.Value];
                        B.Image = MS.Image;// this._Sheet.MapSymbols()[KV.Key].Image;
                        B.ImageAlign = ContentAlignment.MiddleCenter;
                        B.Text = KV.Value + "    =";
                        B.TextAlign = ContentAlignment.MiddleLeft;
                        B.Height = 24;
                        //B.Width = 24 + KV.Key.Length * 4;
                        B.FlatStyle = FlatStyle.Flat;
                        B.Dock = DockStyle.Top;
                        System.Drawing.Font F = new System.Drawing.Font(B.Font.FontFamily, 10, FontStyle.Bold);
                        B.Font = F;
                        B.Margin = new Padding(0);
                        B.Padding = new Padding(0);
                        B.Click += new EventHandler(buttonEvaluationSymbolRemove_Click);
                        // Adding a context menu to enable exclusion
                        B.ContextMenuStrip = this.contextMenuStripSymbolEvaluation;
                        B.Tag = MS;
                        if (MS.IsExcluded)
                        {
                            B.BackColor = System.Drawing.Color.Pink;
                            this.toolTip.SetToolTip(B, MS.Value + " is excluded from the evaluation. Click to remove it from the list");
                        }
                        else
                        {
                            B.BackColor = System.Drawing.Color.White;
                            this.toolTip.SetToolTip(B, "Remove the entry " + KV.Value + " from the list");
                        }
                        this.panelEvaluationSymbols.Controls.Add(B);
                        B.BringToFront();
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void buttonEvaluationSymbolRemove_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                this.EvaluationRemoveSymbol(B.Text.Replace("  =", "").Trim());
                this.EvaluationInitSymbols();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void EvaluationRemoveSymbol(string Value)
        {
            this._Sheet.EvaluationSymbolValueSequence().Remove(Value);
        }

        private void toolStripButtonEvaluationSymbolsClear_Click(object sender, EventArgs e)
        {
            this._Sheet.EvaluationClearSymbols();
            this.EvaluationInitSymbols();
        }

        private void checkBoxKeepLastSymbol_Click(object sender, EventArgs e)
        {
            this._Sheet.KeepLastSymbol = !this._Sheet.KeepLastSymbol;
            this.checkBoxKeepLastSymbol.Checked = this._Sheet.KeepLastSymbol;
        }

        private void tabPageEvaluation_Enter(object sender, EventArgs e)
        {
            this.EvaluationInitSymbols();
        }

        private void maskedTextBoxSymbolSizeSingle_TextChanged(object sender, EventArgs e)
        {
            double Size;
            if (double.TryParse(this.maskedTextBoxSymbolSizeSingle.Text, out Size))
                this._Sheet.GeographySymbolSize = Size;
        }

        #endregion

        #region Gazetteer

        private void comboBoxEvaluationGeographyGazetteer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Sheet.EvaluationSetGazetteer(this.comboBoxEvaluationGeographyGazetteer.SelectedItem.ToString());
        }

        #endregion

        #region WGS84

        private void EvaluationInitWGS84Table(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyWGS84TableAlias = TableAlias;
                if (this.comboBoxEvaluationWgs84Table.Items.Count == 0)
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                    {
                        this.comboBoxEvaluationWgs84Table.Items.Add(KV.Key);
                        if (KV.Value == TableAlias)
                        {
                            this.comboBoxEvaluationWgs84Table.SelectedIndex = i;
                        }
                        i++;
                    }
                }
                try
                {
                    this.comboBoxEvaluationWgs84Column.DataSource = this.TableColumnTable(this._Sheet.GeographyWGS84TableAlias);
                    this.comboBoxEvaluationWgs84Column.DisplayMember = "Display";
                    this.comboBoxEvaluationWgs84Column.ValueMember = "Value";
                    this.comboBoxEvaluationWgs84Column.Enabled = true;
                }
                catch (System.Exception ex)
                { }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void EvaluationInitWGS84ColumnSource(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyWGS84TableAlias = TableAlias;
                this.InitColumnSource(TableAlias, this.comboBoxEvaluationWgs84Column);
            }
            catch (System.Exception ex)
            { }
        }

        private void EvaluationSetWGS84Column(string Column)
        {
            this._Sheet.GeographyWGS84Column = Column;
            System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxEvaluationWgs84Column.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString() == this._Sheet.GeographyWGS84Column)
                {
                    this.comboBoxEvaluationWgs84Column.SelectedIndex = i;
                    break;
                }
            }
        }

        private void comboBoxEvaluationWgs84Table_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().DisplayText == this.comboBoxEvaluationWgs84Table.SelectedItem.ToString())
                {
                    string Alias = DC.Value.DataTable().Alias();
                    this.EvaluationInitWGS84ColumnSource(Alias);
                    try
                    {
                        System.Data.DataTable dtTest = (System.Data.DataTable)this.comboBoxEvaluationWgs84Column.DataSource;
                        if (dtTest.Rows.Count > 1)
                        {
                            this.labelEvaluationWgs84Table.BackColor = System.Drawing.Color.Transparent;
                            this.labelEvaluationWgs84Column.BackColor = System.Drawing.Color.Pink;
                        }
                        else if (dtTest.Rows.Count == 1)
                        {
                            this.EvaluationSetWGS84Column(dtTest.Rows[0][1].ToString());
                            this.labelEvaluationWgs84Column.BackColor = System.Drawing.Color.Transparent;
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    break;
                }
            }
        }

        private void comboBoxEvaluationWgs84Table_DropDown(object sender, EventArgs e)
        {
            this.EvaluationInitWGS84Table("");
        }

        private void comboBoxEvaluationWgs84Column_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxEvaluationWgs84Column.SelectedItem;
            this.EvaluationSetWGS84Column(R[1].ToString());
            this.labelEvaluationWgs84Column.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion

        #region Unit geography

        private void EvaluationInitUnitGeoTable(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyUnitGeoTableAlias = TableAlias;
                if (this.comboBoxEvaluationUnitGeoTable.Items.Count == 0)
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.SelectedTables())
                    {
                        this.comboBoxEvaluationUnitGeoTable.Items.Add(KV.Key);
                        if (KV.Value == TableAlias)
                        {
                            this.comboBoxEvaluationUnitGeoTable.SelectedIndex = i;
                        }
                        i++;
                    }
                }
                try
                {
                    this.comboBoxEvaluationUnitGeoColumn.DataSource = this.TableColumnTable(this._Sheet.GeographyUnitGeoTableAlias);
                    this.comboBoxEvaluationUnitGeoColumn.DisplayMember = "Display";
                    this.comboBoxEvaluationUnitGeoColumn.ValueMember = "Value";
                    this.comboBoxEvaluationUnitGeoColumn.Enabled = true;
                }
                catch (System.Exception ex)
                { }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void EvaluationInitUnitGeoColumnSource(string TableAlias)
        {
            try
            {
                this._Sheet.GeographyUnitGeoTableAlias = TableAlias;
                this.InitColumnSource(TableAlias, this.comboBoxEvaluationUnitGeoColumn);
            }
            catch (System.Exception ex)
            { }
        }

        private void EvaluationSetUnitGeoColumn(string Column)
        {
            this._Sheet.GeographyUnitGeoColumn = Column;
            System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxEvaluationUnitGeoColumn.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString() == this._Sheet.GeographyUnitGeoColumn)
                {
                    this.comboBoxEvaluationUnitGeoColumn.SelectedIndex = i;
                    break;
                }
            }
        }

        private void comboBoxEvaluationUnitGeoTable_DropDown(object sender, EventArgs e)
        {
            this.EvaluationInitUnitGeoTable("");
        }

        private void comboBoxEvaluationUnitGeoTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().DisplayText == this.comboBoxEvaluationUnitGeoTable.SelectedItem.ToString())
                {
                    string Alias = DC.Value.DataTable().Alias();
                    this.EvaluationInitUnitGeoColumnSource(Alias);
                    try
                    {
                        System.Data.DataTable dtTest = (System.Data.DataTable)this.comboBoxEvaluationUnitGeoColumn.DataSource;
                        if (dtTest.Rows.Count > 1)
                        {
                            this.labelEvaluationUnitGeoTable.BackColor = System.Drawing.Color.Transparent;
                            this.labelEvaluationUnitGeoColumn.BackColor = System.Drawing.Color.Pink;
                        }
                        else if (dtTest.Rows.Count == 1)
                        {
                            this.EvaluationSetUnitGeoColumn(dtTest.Rows[0][1].ToString());
                            this.labelEvaluationUnitGeoColumn.BackColor = System.Drawing.Color.Transparent;
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    break;
                }
            }
        }

        private void comboBoxEvaluationUnitGeoColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxEvaluationUnitGeoColumn.SelectedItem;
            this.EvaluationSetUnitGeoColumn(R[1].ToString());
            this.labelEvaluationUnitGeoColumn.BackColor = System.Drawing.Color.Transparent;
        }


        #endregion

        #endregion

        #region Context menu for symbol
        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetMapSymbolExclusion(sender, true);
        }

        private void includeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetMapSymbolExclusion(sender, false);
        }

        private void SetMapSymbolExclusion(object sender, bool IsExcluded)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    if (sourceControl.GetType() == typeof(System.Windows.Forms.Button))
                    {
                        System.Windows.Forms.Button B = (System.Windows.Forms.Button)sourceControl;
                        if (B.Tag != null)
                        {
                            try
                            {
                                MapSymbol MS = (MapSymbol)B.Tag;
                                MS.IsExcluded = IsExcluded;
                                if (IsExcluded)
                                {
                                    B.BackColor = System.Drawing.Color.Pink;
                                    this.toolTip.SetToolTip(B, MS.Value + " is excluded from the evaluation. Click to remove it from the list");
                                }
                                else
                                {
                                    B.BackColor = System.Drawing.Color.White;
                                    this.toolTip.SetToolTip(B, "Remove the entry " + MS.Value + " from the list");
                                }
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// #35
        /// setting the keyword for the help provider
        /// </summary>
        public void setKeyword(string Keyword)
        {
            this.helpProvider.SetHelpKeyword(this, Keyword);
        }


        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion
    }
}
