using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormExportTaxa : Form
    {

        #region Parameter

        private System.Collections.Generic.SortedDictionary<string, string> _TaxonList;
        private string _FileName;
        private bool _FirstLineContainsColumnDefinition = true;
        private enum TransferSetting { All, Selected, Linked }
        private TransferSetting _TransferSetting = TransferSetting.Linked;

        #endregion

        #region Construction

        public FormExportTaxa()
        {
            InitializeComponent();
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {
            this.initSourceFileControls();
            this.tabControlSource.TabPages.Remove(this.tabPageSourceTable);
            this.tabControlSource.TabPages.Remove(this.tabPageSourceTaxonNames);
        }

        #endregion


        #region Source file

        private void initSourceFileControls()
        {
            foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].DatabaseServices())
            {
                if (DS.IsCacheDB || DS.IsForeignSource || DS.IsListInDatabase || DS.IsWebservice)
                    continue;
                this.toolStripComboBoxSourceFileDataSource.Items.Add(DS.DisplayText);
            }
            this.setEncodingList();
            this.checkBoxSourceFileFirstLine.Checked = this._FirstLineContainsColumnDefinition;
            foreach(System.Collections.Generic.KeyValuePair<Parser, string> KV in this.TaxonParser)
                this.toolStripComboBoxSourceFileParser.Items.Add(KV.Value);
            this.toolStripComboBoxSourceFileParser.SelectedIndex = 0;
        }

        #region Reading the source file
        private void buttonSourceFileOpen_Click(object sender, EventArgs e)
        {
            if (this.comboBoxSourceFileEncoding.SelectedIndex == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select an encoding");
                return;
            }
            //DiversityWorkbench.Import.Import.Reset();
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            if (this.textBoxSourceFile.Text.Length > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSourceFile.Text);
                this.openFileDialog.InitialDirectory = FI.DirectoryName;
            }
            else
            {
                System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export));// ...Windows.Forms.Application.StartupPath + "\\Import");
                if (!D.Exists)
                    D.Create();
                this.openFileDialog.InitialDirectory = D.FullName;
            }
            this.openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxSourceFile.Text = f.FullName;
                    this._FileName = f.FullName;
                    this.SetFile(true);
                    //this.ResetFile();
                }
                //this.ShowMessage();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetFile(bool setLineRange)
        {
            try
            {
                if (this.comboBoxSourceFileEncoding.SelectedIndex < 1 || this.textBoxSourceFile.Text.Length == 0)
                    return;
                System.IO.FileInfo f = new System.IO.FileInfo(this._FileName);

                this.dataGridViewSourceFile.AllowUserToAddRows = false;
                this.dataGridViewSourceFile.ReadOnly = true;

                if (this.readFileInDataGridView(f, this.dataGridViewSourceFile, this.Encoding))
                {
                    if (this._FirstLineContainsColumnDefinition)
                    {
                        this.FreezeHeaderline();
                    }

                    // Grid
                    this.dataGridViewSourceFile.Visible = true;
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewSourceFile.Columns)
                        C.SortMode = DataGridViewColumnSortMode.NotSortable;

                }
                else
                {
                    this.dataGridViewSourceFile.Visible = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.IO.StreamReader StreamReader(string File, System.Text.Encoding Encoding)
        {
            System.IO.StreamReader sr;
            sr = new System.IO.StreamReader(File, Encoding);
            return sr;
        }


        private bool readFileInDataGridView(
            System.IO.FileInfo File,
            System.Windows.Forms.DataGridView Grid,
            System.Text.Encoding Encoding)
        {
            Grid.Columns.Clear();
            try
            {
                // Count the file lines and insert grid columns and rows
                System.IO.StreamReader sr = this.StreamReader(File.FullName, Encoding);
                using (sr)
                {
                    String line;
                    bool emptyLine;
                    List<string> columns = new List<string>();
                    int iLine = 0;
                    //int iColumn = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        // Insert the columns
                        emptyLine = DiversityWorkbench.Import.Import.SplitLine(line, columns);

                        // Ignore leading empty lines
                        if (iLine == 0 && emptyLine)
                            continue;

                        if (iLine == 0)
                        {
                            //string HeaederText = "";
                            //if (this._FirstLineContainsColumnDefinition)
                            //    HeaederText = columns[0];
                            //else
                            //    HeaederText = "Taxa";
                            Grid.Columns.Add("Line", "Line");
                            Grid.Columns.Add("Source", "Source file");
                            Grid.Columns.Add("DTN", "DiversityTaxonNames");
                            Grid.Columns.Add("Link", "Link");
                            //for (iColumn = 0; iColumn < columns.Count; iColumn++)
                            //    Grid.Columns.Add("Column_" + iColumn.ToString(), "");
                        }
                        break;
                        //iLine++;
                    }
                    //Grid.Rows.Add(iLine);
                }

                // Read the file into the data grid view
                sr = this.StreamReader(File.FullName, Encoding);
                using (sr)
                {
                    String line;
                    bool emptyLine;
                    List<string> columns = new List<string>();
                    //int iColumn = 0;
                    int iLine = 0;
                    //bool HeaderPassed = false;
                    //bool EmptyLinePassed = false;
                    //int iHeader = 0;
                    // reading the first for lines into the datagrid
                    while ((line = sr.ReadLine()) != null)
                    {
                        // reading the columns
                        emptyLine = DiversityWorkbench.Import.Import.SplitLine(line, columns);

                        // Ignore leading empty lines
                        if ((this._FirstLineContainsColumnDefinition && iLine == 0) || emptyLine)
                        {
                            iLine++;
                            continue;
                        }

                        // reading the lines
                        if (Grid.RowCount <= iLine)
                            Grid.Rows.Add(1);
                        //iColumn = 0;
                        //if (!HeaderPassed || emptyLine)
                        //{
                        //    // skip the header lines
                        //    if (emptyLine && iHeader > 1)
                        //        EmptyLinePassed = true;
                        //    iHeader++;
                        //    if (iHeader > 3 && EmptyLinePassed)
                        //        HeaderPassed = true;
                        //    Grid.Rows[iLine].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                        //}
                        Grid[0, iLine - 1].Value = iLine;
                        Grid[1, iLine - 1].Value = columns[0];
                        //for (iColumn = 0; iColumn < columns.Count; iColumn++)
                        //{
                        //    // Check if column index is exceeded
                        //    //if (Grid.ColumnCount <= iColumn)
                        //    //{
                        //    //    if (this._FirstLineContainsColumnDefinition)
                        //    //    {
                        //    //        // Ask if reading shall be aborted
                        //    //        if (System.Windows.Forms.MessageBox.Show("In line " + iLine.ToString() + " the value " + line + " was outside the range of columns defined in the header line.\r\n\r\nStop reading the file?", "Value outside header", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        //    //            return false;
                        //    //        else
                        //    //            break;
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        //Insert missing column
                        //    //        string columnName = columns[iColumn];
                        //    //        if (columnName.IndexOf(' ') > -1)
                        //    //            columnName = columnName.Substring(0, columnName.IndexOf(' '));
                        //    //        Grid.Columns.Add("Column_" + iColumn.ToString(), columnName);
                        //    //    }
                        //    //}
                        //    // Insert column
                        //    Grid[iColumn, iLine].Value = columns[iColumn];
                        //}
                        iLine++;
                    }
                }
                foreach (System.Windows.Forms.DataGridViewColumn C in Grid.Columns)
                    C.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            }
            catch (System.IO.IOException IOex)
            {
                System.Windows.Forms.MessageBox.Show(IOex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(IOex);
                return false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }


        private System.Text.Encoding Encoding
        {
            get
            {
                System.Text.Encoding E = System.Text.Encoding.UTF8;
                if (DiversityWorkbench.Import.Import.Encodings.Count > 0)
                {
                    E = DiversityWorkbench.Import.Import.Encodings[this.comboBoxSourceFileEncoding.SelectedItem.ToString()];
                }
                return E;
            }
        }

        private void setEncodingList()
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Text.Encoding> KV in DiversityWorkbench.Import.Import.Encodings)
                {
                    this.comboBoxSourceFileEncoding.Items.Add(KV.Key);
                }
                this.comboBoxSourceFileEncoding.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FreezeHeaderline()
        {
            try
            {
                if (this.dataGridViewSourceFile.Rows.Count > 0)
                {
                    if (this._FirstLineContainsColumnDefinition)
                        this.dataGridViewSourceFile.Rows[0].Frozen = true;
                    else
                        this.dataGridViewSourceFile.Rows[0].Frozen = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void checkBoxSourceFileFirstLine_Click(object sender, EventArgs e)
        {
            this._FirstLineContainsColumnDefinition = this.checkBoxSourceFileFirstLine.Checked;
        }

        #endregion

        #region Transfer to main list
        private void SourceFileTransferAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonSourceFileTransferSetting.Image = SourceFileTransferAllToolStripMenuItem.Image;
            this._TransferSetting = TransferSetting.All;
            this.toolStripDropDownButtonSourceFileTransferSetting.ToolTipText = this.SourceFileTransferAllToolStripMenuItem.ToolTipText;
        }

        private void SourceFileTransferSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonSourceFileTransferSetting.Image = SourceFileTransferSelectedToolStripMenuItem.Image;
            this._TransferSetting = TransferSetting.Selected;
            this.toolStripDropDownButtonSourceFileTransferSetting.ToolTipText = this.SourceFileTransferSelectedToolStripMenuItem.ToolTipText;
        }

        private void SourceFileTransferLinkedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonSourceFileTransferSetting.Image = SourceFileTransferLinkedToolStripMenuItem.Image;
            this._TransferSetting = TransferSetting.Linked;
            this.toolStripDropDownButtonSourceFileTransferSetting.ToolTipText = this.SourceFileTransferLinkedToolStripMenuItem.ToolTipText;
        }

        private void toolStripButtonSourceFileAdd_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Compare with DTN
        
        private enum Parser { Whole, ExclAuth, OnePart, TwoParts, TreeParts, FourParts}
        private System.Collections.Generic.Dictionary<Parser, string> _TaxonParser;
        private System.Collections.Generic.Dictionary<Parser, string> TaxonParser
        {
            get
            {
                if (this._TaxonParser == null)
                {
                    this._TaxonParser = new Dictionary<Parser, string>();
                    this._TaxonParser.Add(Parser.Whole, "Whole name");
                    this._TaxonParser.Add(Parser.ExclAuth, "Excl. auth.");
                    this._TaxonParser.Add(Parser.FourParts, "first 4 parts");
                    this._TaxonParser.Add(Parser.TreeParts, "first 3 parts");
                    this._TaxonParser.Add(Parser.TwoParts, "first 2 parts");
                    this._TaxonParser.Add(Parser.OnePart, "first part");
                }
                return this._TaxonParser;
            }
        }

        private void toolStripButtonSourceFileEditLine_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBoxSourceFileDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._DTN_Projects = new DataTable();

            this._TaxonDatabase = this.toolStripComboBoxSourceFileDataSource.SelectedItem.ToString();
            this._TaxonDatabaseConnectionString = DiversityWorkbench.Settings.ConnectionString;
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnections())
            {
                if (KV.Value.DisplayText == this._TaxonDatabase)
                {
                    this._TaxonDatabaseConnectionString = KV.Value.ConnectionString;
                    this._TaxonDatabase = KV.Value.DatabaseName;
                    this._TaxonDatabaseBaseURL = KV.Value.BaseURL;
                    this._TaxonDatabaseLinkedServer = KV.Value.LinkedServer;
                }
            }
        }

        private string _TaxonDatabase = "";
        private string _TaxonDatabaseConnectionString = "";
        private string _TaxonDatabaseBaseURL = "";
        private string _TaxonDatabaseLinkedServer = "";

        private void toolStripButtonSourceFileLinkToDiversityTaxonNames_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewSourceFile.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a file containing data for the export");
                return;
            }
            if (this._TaxonDatabase.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a database");
                return;
            }
            Parser P = Parser.Whole;
            foreach(System.Collections.Generic.KeyValuePair<Parser, string> KV in this.TaxonParser)
            {
                if (KV.Value == this.toolStripComboBoxSourceFileParser.SelectedItem.ToString())
                {
                    P = KV.Key;
                    break;
                }
            }
            switch(P)
            {
                case Parser.Whole:
                    break;
                case Parser.ExclAuth:
                    break;
                case Parser.FourParts:
                    break;
                case Parser.TreeParts:
                    break;
                case Parser.TwoParts:
                    break;
                case Parser.OnePart:
                    break;
            }
        }

        private System.Data.DataTable _DTN_Projects;
        private void toolStripComboBoxSourceFileProject_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (this._DTN_Projects == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a source database");
                    return;
                }
                if (this._DTN_Projects.Rows.Count == 0)
                {
                    string SelectedDatabase = this.toolStripComboBoxSourceFileDataSource.SelectedItem.ToString();
                    string ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                    string Database = this.toolStripComboBoxSourceFileDataSource.Text;
                    string LinkedServer = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnections())
                    {
                        if (KV.Value.DisplayText == SelectedDatabase)
                        {
                            ConnectionString = KV.Value.ConnectionString;
                            Database = KV.Value.DatabaseName;
                            LinkedServer = KV.Value.LinkedServer;
                            break;
                        }
                    }
                    string SQL = "SELECT NULL AS ProjectID, NULL AS Project " +
                        "UNION SELECT ProjectID, Project " +
                        "FROM " + this.SynchronizeColTaxTextPrefix + "ProjectList " +
                        "ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                    ad.Fill(this._DTN_Projects);
                    foreach (System.Data.DataRow R in this._DTN_Projects.Rows)
                        toolStripComboBoxSourceFileProject.Items.Add(R["Project"].ToString());
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private string SynchronizeColTaxTextPrefix
        {
            get
            {
                string Prefix = this._TaxonDatabase + ".dbo.";
                if (this._TaxonDatabaseLinkedServer.Length > 0)
                    Prefix = "[" + this._TaxonDatabaseLinkedServer + "]." + Prefix;
                return Prefix;
            }
        }

        private void SynchronizeColTaxTextFuzzySearch(Parser ParseOption, string Taxon, string Prefix, string ConnectionString, 
            ref string TaxonNameCache, ref string NameURI)
        {
            switch(ParseOption)
            {
                case Parser.Whole:
                    break;
            }
        }



        #endregion

        #region Project

        private System.Collections.Generic.Dictionary<int, string> _Projects;
        private void toolStripButtonSourceProjectAdd_Click(object sender, EventArgs e)
        {
            System.Data.DataTable Projects = DiversityCollection.LookupTable.DtProjectList.Copy();
            if (this._Projects == null)
                this._Projects = new Dictionary<int, string>();
            foreach (System.Data.DataRow R in Projects.Rows)
            {
                if (this._Projects.ContainsKey(int.Parse(R[1].ToString())))
                    R.Delete();
            }
            Projects.AcceptChanges();
            if (Projects.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No further projects available");
                return;
            }
            DiversityCollection.Forms.FormGetProject f = new FormGetProject(Projects);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (f.ProjectID != null)
                    {
                        if (!this._Projects.ContainsKey((int)f.ProjectID))
                        {
                            this._Projects.Add((int)f.ProjectID, f.Project);
                            this.FillProjectList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void FillProjectList()
        {
            this.listBoxSourceProjects.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in this._Projects)
                this.listBoxSourceProjects.Items.Add(KV.Value);
        }

        private void toolStripButtonSourceProjectRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxSourceProjects.SelectedIndex > -1)
            {
                int ProjectID = -1;
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in this._Projects)
                {
                    if (this.listBoxSourceProjects.SelectedItem.ToString() == KV.Value)
                    {
                        ProjectID = KV.Key;
                        break;
                    }
                }
                this._Projects.Remove(ProjectID);
                this.FillProjectList();
            }
        }

        #endregion

        #endregion


    }
}
