using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormImportWizardSplitSettings : Form
    {
        #region Parameter
        
        private DiversityCollection.Import_Column _IC;
        //private System.Data.DataTable _DtTranslation;

        private System.Data.DataTable _DtEnum;

        private System.Windows.Forms.DataGridViewTextBoxColumn sourceValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn databaseValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn databaseValueDataGridViewComboBoxColumn;
        
        #endregion

        #region Construction and form
        
        public FormImportWizardSplitSettings(DiversityCollection.Import_Column IC)
        {
            InitializeComponent();
            this._IC = IC;
            this.initForm();
        }

        private void initForm()
        {
            this.labelHeader.Text = "Define the transformation settings for column " + this._IC.Column + " in table " + this._IC.Table + ".";

            //Splitting
            if (this._IC.Splitters != null && this._IC.Splitters.Count > 0)
            {
                this.checkBoxSplitColumn.Checked = true;
                this.numericUpDownPosition.Value = this._IC.SplitPosition;
                foreach (string s in this._IC.Splitters)
                    this.listBoxSplitters.Items.Add(s);
            }
            else
            {
                this.checkBoxSplitColumn.Checked = false;
            }
            this.tableLayoutPanelSplitting.Enabled = this.checkBoxSplitColumn.Checked;

            // Transformation
            if (this._IC.RegularExpressionPattern != null && this._IC.RegularExpressionPattern.Length > 0)
                this.textBoxRegex.Text = this._IC.RegularExpressionPattern;
            if (this._IC.RegularExpressionReplacement != null && this._IC.RegularExpressionReplacement.Length > 0)
                this.textBoxReplaceBy.Text = this._IC.RegularExpressionReplacement;
            if (this._IC.RegularExpressionPattern != null && this._IC.RegularExpressionPattern.Length > 0 &&
                this._IC.RegularExpressionReplacement != null && this._IC.RegularExpressionReplacement.Length > 0)
                this.checkBoxRegex.Checked = true;
            else
                this.checkBoxRegex.Checked = false;
            this.tableLayoutPanelRegex.Enabled = this.checkBoxRegex.Checked;

            // Translation
            if (this._IC.TypeOfEntry == Import_Column.EntryType.MandatoryList)
            {
                this.sourceValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.sourceValueDataGridViewTextBoxColumn.HeaderText = "Source";
                this.databaseValueDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
                this.databaseValueDataGridViewComboBoxColumn.HeaderText = this._IC.ValueColumn;
                this.dataGridViewTranslation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    this.sourceValueDataGridViewTextBoxColumn,
                    this.databaseValueDataGridViewComboBoxColumn});
                this.databaseValueDataGridViewComboBoxColumn.DataSource = this._IC.getLookUpTable();
                this.databaseValueDataGridViewComboBoxColumn.DisplayMember = this._IC.getLookUpTable().Columns[this._IC.DisplayColumn].ColumnName;
                this.databaseValueDataGridViewComboBoxColumn.ValueMember = this._IC.getLookUpTable().Columns[this._IC.ValueColumn].ColumnName;
            }
            else
            {
                this.sourceValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.databaseValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.dataGridViewTranslation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    this.sourceValueDataGridViewTextBoxColumn,
                    this.databaseValueDataGridViewTextBoxColumn});
            }
            if (this._IC.TranslationDictionary != null &&
                this._IC.TranslationDictionary.Count > 0)
            {
                this.checkBoxTranslate.Checked = true;
                this.dataGridViewTranslation.Rows.Add(this._IC.TranslationDictionary.Count);
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._IC.TranslationDictionary)
                {
                    this.dataGridViewTranslation.Rows[i].Cells[0].Value = KV.Key;
                    this.dataGridViewTranslation.Rows[i].Cells[1].Value = KV.Value;
                    i++;
                }
            }
            else
                this.checkBoxTranslate.Checked = false;
            this.checkBoxTranslate_CheckedChanged(null, null);

            // Prefix
            if (this._IC.Separator != null)
                this.textBoxPrefix.Text = this._IC.Separator;
        }

        private void FormImportWizardSplitSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                this.SaveSettings();
            }
        }

        private void SaveSettings()
        {
            this._IC.Splitters.Clear();

            // Splitting
            if (this.checkBoxSplitColumn.Checked)
            {
                foreach (object itemChecked in listBoxSplitters.Items)
                {
                    string Splitter = itemChecked.ToString();
                    this._IC.Splitters.Add(Splitter);
                }
                this._IC.SplitPosition = (int)this.numericUpDownPosition.Value;
            }

            // Regex
            this._IC.RegularExpressionPattern = "";
            this._IC.RegularExpressionReplacement = "";
            if (this.checkBoxRegex.Checked)
            {
                this._IC.RegularExpressionPattern = this.textBoxRegex.Text;
                this._IC.RegularExpressionReplacement = this.textBoxReplaceBy.Text;
            }

            // Translation
            if (this.checkBoxTranslate.Checked)
            {
                //if (this._IC.TranslationDictionary == null)
                //    this._IC.TranslationDictionary = new Dictionary<string, string>();
                //else this._IC.TranslationDictionary.Clear();
                this._IC.TranslationDictionary.Clear();
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewTranslation.Rows)
                {
                    if (R.Cells[0].Value != null && R.Cells[1].Value != null)
                        this._IC.TranslationDictionary.Add(R.Cells[0].Value.ToString(), R.Cells[1].Value.ToString());
                }
                //foreach (System.Data.DataRow R in this.DtTranslation.Rows)// this.dataSetImportWizard.DataTableTranslation.Rows)
                //{
                //    this._IC.TranslationDictionary[R[0].ToString()] = R[1].ToString();
                //}
            }
            else if (this._IC.TranslationDictionary != null) 
                this._IC.TranslationDictionary.Clear();
        }
        
        #endregion

        #region Splitting
        
        private void checkBoxSplitColumn_CheckedChanged(object sender, EventArgs e)
        {
            this.tableLayoutPanelSplitting.Enabled = this.checkBoxSplitColumn.Checked;
        }

        private void toolStripButtonAddSplitter_Click(object sender, EventArgs e)
        {
            string Splitter = "";
            DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("Please enter the splitter", "", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                Splitter = f.String;
            else return;
            //bool Include = false;
            //if (System.Windows.Forms.MessageBox.Show("Should the splitter \r\n" + Splitter + "\r\n be imported", "Import splitter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    Include = true;
            this.listBoxSplitters.Items.Add(Splitter);
        }

        private void toolStripButtonRemoveSplitter_Click(object sender, EventArgs e)
        {
            if (this.listBoxSplitters.SelectedIndex == -1 || this.listBoxSplitters.Items.Count == 0)
                return;
            int i = this.listBoxSplitters.SelectedIndex;
            this.listBoxSplitters.Items.RemoveAt(i);
        }

        #endregion

        #region Regex
        
        private void textBoxRegex_TextChanged(object sender, EventArgs e)
        {
            this._IC.RegularExpressionPattern = this.textBoxRegex.Text;
        }

        private void textBoxReplaceBy_TextChanged(object sender, EventArgs e)
        {
            this._IC.RegularExpressionReplacement = this.textBoxReplaceBy.Text;
        }

        private void checkBoxRegex_CheckedChanged(object sender, EventArgs e)
        {
            this.tableLayoutPanelRegex.Enabled = this.checkBoxRegex.Checked;
        }
        
        #endregion

        #region Translation
        
        private void buttonRefreshTranslationList_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewTest.Rows.Count == 0)
            {
                this.buttonTest_Click(null, null);
            }
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewTest.Rows)
            {
                if (R.Cells[2].Value != null && 
                    ((this.radioButtonTranslateErrors.Checked && R.Cells[2].Value.ToString() == "False")
                    || this.radioButtonTranslateAll.Checked) && 
                    !this._IC.TranslationDictionary.ContainsKey(R.Cells[1].Value.ToString()))
                {
                    this._IC.TranslationDictionary.Add(R.Cells[1].Value.ToString(), "");
                }
            }
            this.dataGridViewTranslation.Rows.Clear();
            if (this._IC.TranslationDictionary.Count > 0)
            {
                this.dataGridViewTranslation.Rows.Add(this._IC.TranslationDictionary.Count);
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._IC.TranslationDictionary)
                {
                    this.dataGridViewTranslation.Rows[i].Cells[0].Value = KV.Key;
                    this.dataGridViewTranslation.Rows[i].Cells[1].Value = KV.Value;
                    i++;
                }
            }
        }

        private string SqlForEnumerationTable
        {
            get
            {
                string SQL = "";

                try
                {
                    SQL = "SELECT C.COLUMN_NAME, C.TABLE_NAME " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME " +
                        "WHERE (K.TABLE_NAME = '" + this._IC.Table + "') and K.COLUMN_NAME = '" + this._IC.Column + "'";
                    System.Data.DataTable dt = new DataTable();
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    string Table = dt.Rows[0][0].ToString();
                    string Column = dt.Rows[0][1].ToString();
                    SQL = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C " +
                        "WHERE C.TABLE_NAME = '" + Table + "'" +
                        "AND C.COLUMN_NAME <> '" + Column + "'" +
                        "AND C.COLUMN_NAME NOT LIKE 'Log%' " +
                        "AND C.COLUMN_NAME <> 'RowGUID' " +
                        "AND C.COLUMN_NAME NOT LIKE '%ID' " +
                        "AND C.COLUMN_NAME NOT LIKE '%URI'";
                    System.Data.DataTable dtColumns = new DataTable();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtColumns);
                    string DisplayColumn = Column;
                    System.Collections.Generic.List<string> DisplayColumnList = new List<string>();
                    DisplayColumnList.Add("DisplayText");
                    DisplayColumnList.Add("Display");
                    foreach (System.Data.DataRow R in dtColumns.Rows)
                    {
                        string Col = R[0].ToString();
                        if (DisplayColumnList.Contains(Col))
                        {
                            DisplayColumn = Col;
                            break;
                        }
                        if (Col.IndexOf(Table) > -1 && (Col.IndexOf("Name") > -1 || Col.IndexOf("Title") > -1))
                        {
                            DisplayColumn = Col;
                            break;
                        }
                    }
                    SQL = "SELECT " + Column + " AS [Key], " + DisplayColumn + " AS DisplayText " +
                        "FROM  " + Table + "ORDER BY DisplayText";
                }
                catch (Exception)
                {
                }

                return SQL;
            }
        }

        private void checkBoxTranslate_CheckedChanged(object sender, EventArgs e)
        {
            this.dataGridViewTranslation.Enabled = this.checkBoxTranslate.Checked;
            this.buttonClearTranslation.Enabled = this.checkBoxTranslate.Checked;
            this.buttonRefreshTranslationList.Enabled = this.checkBoxTranslate.Checked;
            this.radioButtonTranslateAll.Enabled = this.checkBoxTranslate.Checked;
            this.radioButtonTranslateErrors.Enabled = this.checkBoxTranslate.Checked;
            this.toolStripTranslate.Enabled = this.checkBoxTranslate.Checked;
        }

        public System.Data.DataTable DtEnum
        {
            get 
            {
                if (this._DtEnum == null)
                {
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(this.SqlForEnumerationTable, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtEnum);
                }
                return _DtEnum; 
            }
            //set { _DtEnum = value; }
        }

        //public System.Data.DataTable DtTranslation
        //{
        //    get 
        //    {
        //        if (this._DtTranslation == null)
        //        {
        //            this._DtTranslation = new DataTable();
        //            System.Data.DataColumn C1 = new DataColumn("Source", System.Type.GetType("System.String"));
        //            this._DtTranslation.Columns.Add(C1);
        //            System.Data.DataColumn C2 = new DataColumn(this._IC.Column, System.Type.GetType("System.String"));
        //            this._DtTranslation.Columns.Add(C2);
        //        }
        //        return _DtTranslation; 
        //    }
        //    //set { _DtTranslation = value; }
        //}
        
        private void buttonClearTranslation_Click(object sender, EventArgs e)
        {
            //if (this._IC.TranslationDictionary != null)
            //    this._IC.TranslationDictionary.Clear();
            //else this._IC.TranslationDictionary = new Dictionary<string, string>();
            this._IC.TranslationDictionary.Clear();
            //this.DtTranslation.Clear();
            this.dataGridViewTranslation.Rows.Clear();
        }

        private void toolStripButtonTranslationAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("New Translation", "Please enter a string that should be translated", "");
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            {
                this.dataGridViewTranslation.Rows.Add();
                this.dataGridViewTranslation.Rows[this.dataGridViewTranslation.Rows.Count].Cells[0].Value = f.String;
            }
        }

        private void toolStripButtonTranslationRemove_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewTranslation.SelectedCells == null)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            this.dataGridViewTranslation.Rows.RemoveAt(this.dataGridViewTranslation.SelectedCells[0].RowIndex);
        }

        private void toolStripButtonTranslationRefreshList_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewTest.Rows.Count == 0)
            {
                this.buttonTest_Click(null, null);
            }
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewTest.Rows)
            {
                if (R.Cells[2].Value != null &&
                    ((this.radioButtonTranslateErrors.Checked && R.Cells[2].Value.ToString() == "False")
                    || this.radioButtonTranslateAll.Checked) &&
                    !this._IC.TranslationDictionary.ContainsKey(R.Cells[1].Value.ToString()))
                {
                    this._IC.TranslationDictionary.Add(R.Cells[1].Value.ToString(), "");
                }
            }
            this.dataGridViewTranslation.Rows.Clear();
            if (this._IC.TranslationDictionary.Count > 0)
            {
                this.dataGridViewTranslation.Rows.Add(this._IC.TranslationDictionary.Count);
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._IC.TranslationDictionary)
                {
                    this.dataGridViewTranslation.Rows[i].Cells[0].Value = KV.Key;
                    this.dataGridViewTranslation.Rows[i].Cells[1].Value = KV.Value;
                    i++;
                }
            }
        }

        private void toolStripButtonTranslationClearList_Click(object sender, EventArgs e)
        {
            this._IC.TranslationDictionary.Clear();
            this.dataGridViewTranslation.Rows.Clear();
        }

        private void dataGridViewTranslation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion

        #region Test
        
        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (this._IC.ColumnInSourceFile == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a column in the file");
                return;
            }
            this.dataGridViewTest.Columns.Clear();
            this.SaveSettings();
            this.dataGridViewTest.DataSource = null;
            System.Data.DataTable dt = new DataTable();
            System.Data.DataColumn C1 = new DataColumn("Source", System.Type.GetType("System.String"));
            dt.Columns.Add(C1);
            System.Data.DataColumn C2 = new DataColumn(this._IC.Column, System.Type.GetType("System.String"));
            dt.Columns.Add(C2);
            System.Data.DataColumn C3 = new DataColumn("OK", System.Type.GetType("System.Boolean"));
            dt.Columns.Add(C3);
            if (this._IC.AlternativeColumn != null && this._IC.AlternativeColumn.Length > 0)
            {
                System.Data.DataColumn C4 = new DataColumn(this._IC.AlternativeColumn, System.Type.GetType("System.String"));
                dt.Columns.Add(C4);
            }
            foreach (string s in this._IC.ValueList)
            {
                System.Data.DataRow R = dt.NewRow();
                R[0] = s;
                string TransformedValue  = this._IC.TransformedValue(s);
                //if (this._IC.Separator != null && this._IC.Separator.Length > 0)
                //    TransformedValue = this._IC.Separator + TransformedValue;
                R[1] = TransformedValue;
                if (this._IC.TypeOfEntry == Import_Column.EntryType.MandatoryList)
                {
                    bool isTextColumn = false;
                    if (this._IC.DataType() == "char" || this._IC.DataType() == "ntext" || this._IC.DataType() == "nvarchar" || this._IC.DataType() == "varchar")
                        isTextColumn = true;
                    string WhereClause = this._IC.getLookUpTable().Columns[this._IC.ValueColumn].ColumnName + " = ";
                    if (isTextColumn) WhereClause += "'";
                    WhereClause += TransformedValue;
                    if (isTextColumn) WhereClause += "'";
                    try
                    {
                        System.Data.DataRow[] rr = this._IC.getLookUpTable().Select(WhereClause);
                        if (rr.Length > 0)
                        {
                            R[2] = 1;
                        }
                        else R[2] = 0;
                    }
                    catch (System.Exception ex) { R[2] = 0; }
                }
                else
                {
                    switch (this._IC.DataType())
                    {
                        case "tinyint":
                            System.Byte ii;
                            if (System.Byte.TryParse(R[1].ToString(), out ii))
                                R[2] = 1;
                            else R[2] = 0;
                            break;
                        case "int":
                            int i;
                            if (int.TryParse(R[1].ToString(), out i))
                                R[2] = 1;
                            else R[2] = 0;
                            break;
                        default:
                            R[2] = 1;
                            break;
                    }
                }
                if (this._IC.AlternativeColumn != null &&
                    this._IC.AlternativeColumn.Length > 0 &&
                    R[2].ToString() == "False")
                {
                    R[4] = R[1];
                    R[1] = "";
                }
                dt.Rows.Add(R);
            }
            this.dataGridViewTest.DataSource = dt;
            if (this.dataGridViewTest.Rows.Count > 0)
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewTest.Rows)
                {
                    if (R.Cells[2].Value != null && R.Cells[2].Value.ToString() == "True")
                    {
                        R.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                        R.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        R.DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
                        R.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }
        
        private void dataGridViewTest_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion


    }
}
