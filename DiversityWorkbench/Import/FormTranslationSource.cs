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
    public partial class FormTranslationSource : Form
    {
        #region Construction

        /// <summary>
        /// Form for getting either a file containing values for the translation or a table from the database
        /// </summary>
        /// <param name="ForDictionary">If a file should be selected</param>
        public FormTranslationSource(bool ForDictionary = true, string TranslationTable = "", string FromColumn = "", string IntoColumn = "")
        {
            InitializeComponent();
            try
            {
                if (ForDictionary)
                    this.setEncodingList();
                this.splitContainer.Panel1Collapsed = !ForDictionary;
                this.splitContainer.Panel2Collapsed = ForDictionary;
                if (!ForDictionary)
                {
                    if (TranslationTable != null && TranslationTable.Length > 0)
                    {
                        TranslationSourceTable = TranslationTable;
                        TranslationFromColumn = FromColumn;
                        TranslationIntoColumn = IntoColumn;
                        this.initSourceGrid();
                    }
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Dictionary

        private void readTranslationSource(
            System.IO.FileInfo File,
            System.Text.Encoding Encoding)
        {
            try
            {
                this._TranslationDictionary = null;
                int iLine = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(File.FullName, Encoding);
                using (sr)
                {
                    String line;
                    this.TranslationHeaderFrom = "";
                    this.TranslationHeaderTo = "";
                    // reading the first for lines into the datagrid
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            // reading the columns
                            if (this.checkBoxFirstLine.Checked && iLine == 0)
                            {
                                this.TranslationHeaderFrom = line.Substring(0, line.IndexOf("\t")).ToString().Trim();
                                this.TranslationHeaderTo = line.Substring(line.IndexOf("\t")).ToString().Trim();
                            }
                            else
                            {
                                this.TranslationDictionary.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim(), line.Substring(line.IndexOf("\t")).ToString().Trim());
                            }
                            iLine++;
                        }
                    }
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.DataColumn dc1 = new System.Data.DataColumn("From");
                dt.Columns.Add(dc1);
                System.Data.DataColumn dc2 = new System.Data.DataColumn("To");
                dt.Columns.Add(dc2);
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.TranslationDictionary)
                {
                    System.Data.DataRow r = dt.NewRow();
                    r[0] = KV.Key;
                    r[1] = KV.Value;
                    dt.Rows.Add(r);
                }
                this.dataGridView.DataSource = dt;
                this.dataGridView.ReadOnly = true;
                this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                this.dataGridView.AllowUserToAddRows = false;
                this.dataGridView.RowHeadersVisible = false;
            }
            catch (System.IO.IOException IOex)
            {
                System.Windows.Forms.MessageBox.Show(IOex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(IOex);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Import));// ...Windows.Forms.Application.StartupPath + "\\Import");
            if (!D.Exists)
                D.Create();
            this.openFileDialog.InitialDirectory = D.FullName;
            this.openFileDialog.Filter = "Text Files|*.txt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.labelFile.Text = f.FullName;
                    this.readTranslationSource(f, this.Encoding);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void checkBoxFirstLine_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEncoding_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (this.Encoding != null)
            //{
            //    if (DiversityWorkbench.Import.Import.Encoding != this.Encoding)
            //    {
            //        DiversityWorkbench.Import.Import.Encoding = this.Encoding;
            //    }
            //}
        }

        private string TranslationHeaderFrom = "";
        private string TranslationHeaderTo = "";

        private System.Collections.Generic.SortedDictionary<string, string> _TranslationDictionary;

        public System.Collections.Generic.SortedDictionary<string, string> TranslationDictionary
        {
            get
            {
                if (this._TranslationDictionary == null)
                {
                    this._TranslationDictionary = new SortedDictionary<string, string>();
                }
                return this._TranslationDictionary;
            }
        }

        #region Encoding
        
        private void setEncodingList()
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Text.Encoding> KV in Import.Encodings)
                {
                    this.comboBoxEncoding.Items.Add(KV.Key);
                }
                this.comboBoxEncoding.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Text.Encoding Encoding
        {
            get
            {
                System.Text.Encoding E = System.Text.Encoding.Default;
                if (Import.Encodings.Count > 0 && this.comboBoxEncoding.SelectedItem.ToString().Length > 0)
                {
                    E = Import.Encodings[this.comboBoxEncoding.SelectedItem.ToString()];
                }
                return E;
            }
        }

        #endregion

        #endregion

        #region Source table

        private string _TranslationSourceTable;
        public string TranslationSourceTable { get => _TranslationSourceTable;
            set
            {
                if (this.comboBoxSourceTable.DataSource == null)
                    this.comboBoxSourceTable_DropDown(null, null);
                _TranslationSourceTable = value;
                try
                {
                    System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxSourceTable.DataSource;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][0].ToString() == _TranslationSourceTable)
                        {
                            this.comboBoxSourceTable.SelectedIndex = i;
                            break;
                        }
                    }
                }
                catch { }

                this.initSourceTableColumns();
            }
        }

        private string _TranslationFromColumn;
        public string TranslationFromColumn { get => _TranslationFromColumn;
            set
            {
                _TranslationFromColumn = value;
                if (this.comboBoxFromColumn.DataSource != null)
                {
                    try
                    {
                        System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxFromColumn.DataSource;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][0].ToString() == _TranslationFromColumn)
                            {
                                this.comboBoxFromColumn.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private string _TranslationIntoColumn;
        public string TranslationIntoColumn { get => _TranslationIntoColumn;
            set
            {
                _TranslationIntoColumn = value;
                if (this.comboBoxIntoColumn.DataSource != null)
                {
                    try
                    {
                        System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxIntoColumn.DataSource;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][0].ToString() == _TranslationIntoColumn)
                            {
                                this.comboBoxIntoColumn.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private void comboBoxSourceTable_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxSourceTable.DataSource == null)
            {
                string SQL = "SELECT TABLE_NAME " +
                    "FROM INFORMATION_SCHEMA.TABLES AS T " +
                    "WHERE (TABLE_TYPE = 'base table') AND (TABLE_NAME NOT LIKE '%_log') " +
                    "ORDER BY TABLE_NAME";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                this.comboBoxSourceTable.DataSource = dt;
                this.comboBoxSourceTable.DisplayMember = "TABLE_NAME";
                this.comboBoxSourceTable.ValueMember = "TABLE_NAME";
            }
            this._TranslationSourceTable = "";
            this._TranslationIntoColumn = "";
            this._TranslationFromColumn = "";
        }

        private void comboBoxSourceTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.TranslationSourceTable = this.comboBoxSourceTable.SelectedValue.ToString();
            this.initSourceTableColumns();
        }

        private void initSourceTableColumns()
        {
            string SQL = "SELECT COLUMN_NAME " +
                "FROM INFORMATION_SCHEMA.COLUMNS AS T " +
                "WHERE (TABLE_NAME = '" + this.TranslationSourceTable + "') " +
                "ORDER BY COLUMN_NAME";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            this.comboBoxFromColumn.DataSource = dt;
            this.comboBoxFromColumn.DisplayMember = "COLUMN_NAME";
            this.comboBoxFromColumn.ValueMember = "COLUMN_NAME";
            this.comboBoxIntoColumn.DataSource = dt.Copy();
            this.comboBoxIntoColumn.DisplayMember = "COLUMN_NAME";
            this.comboBoxIntoColumn.ValueMember = "COLUMN_NAME";
            this.dataGridViewSourceTable.DataSource = null;
        }

        private void comboBoxFromColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.TranslationFromColumn = this.comboBoxFromColumn.SelectedValue.ToString();
            this.initSourceGrid();
        }

        private void comboBoxIntoColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.TranslationIntoColumn = this.comboBoxIntoColumn.SelectedValue.ToString();
            this.initSourceGrid();
        }

        
        private void initSourceGrid()
        {
            if (this.TranslationFromColumn.Length > 0 && this.TranslationIntoColumn.Length > 0 && this.TranslationSourceTable.Length > 0)
            {
                string SQL = "SELECT " + this.TranslationFromColumn + ", " + this.TranslationIntoColumn + " FROM " + this.TranslationSourceTable +
                    " WHERE " + this.TranslationFromColumn + " <> '' AND " + this.TranslationIntoColumn + " <> '' " +
                    " GROUP BY " + this.TranslationFromColumn + ", " + this.TranslationIntoColumn +
                    " HAVING COUNT(*) = 1 " +
                    " ORDER BY " + this.TranslationFromColumn;
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                this.dataGridViewSourceTable.DataSource = dt;
            }
            else
                this.dataGridViewSourceTable.DataSource = null;
        }

        #endregion

    }
}
