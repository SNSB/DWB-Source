using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{
    /*
     * Parallelitaet notwendig wenn e.g. Wirt-Parasit so exportiert werden soll
     * dann muessen auch Filterkritierien auf diesen Stufen moeglich sein, also fuer jede Tabelle
     * 
     * auch zwischen gleichen Tabellen muss Abhaengigkeit moeglich sein e.g. Wirt-Parasit
     * 
     * Startpunkt an allen stellen, e.g. Tabelle oder Tabelle mit Einschraenkung e.g. Wirt
     * 
     * Gruppierung, Max, Min, Sum ermoeglichen
     * 
     * Eine Spalte kann mehrfach verwendet, umformatiert und mit anderen zusammengefasst werden
     * 
     * Spalten die zu Modulen verbinden bieten die Felder aus den Modulen an
     * diese koennen sich unterscheiden - e.g. DTN <> IndexFungorum <> CoL
     * 
     * */

    public partial class FormExport : Form, iExporter
    {

        #region Construction and form

        public FormExport(Export.Table StartTable, System.Collections.Generic.List<int> ListOfIDs, System.Collections.Generic.List<Export.Table> TemplateTables, string HelpNamespace)
        {
            InitializeComponent();
            if (ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No data found");
            }
            else
            {
                try
                {
                    Exporter.IExporter = this;
                    this.helpProvider.HelpNamespace = HelpNamespace;
                    this.Text = "Export " + StartTable.DisplayText.ToLower();
                    Exporter.StartTable = StartTable;
                    Exporter.ListOfIDs = ListOfIDs;
                    this.numericUpDownTestExport.Maximum = ListOfIDs.Count;
                    this.InitSourceTables();
                    this.setExportPaths();
                    this.initSQLite();
                    // Remove sobald fertig bzw. benoetigt
                    this.tabControlSchemaResult.TabPages.Remove(this.tabPageHeader);
#if !DEBUG
                    // ToDo - funktioniert noch nicht
                    this.buttonShowTestExportSQL.Visible = false;
#endif
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        private void FormExport_SizeChanged(object sender, EventArgs e)
        {
            //System.Drawing.Point P = new Point(this.Width - this.buttonFeedback.Width, 0);
            //this.buttonFeedback.Location = P;
        }

        #endregion

        #region Source tables

        public void MarkCurrentSourceTable(Export.Table CurrentTable)
        {
            int Scroll = this.panelSourceTables.VerticalScroll.Value;
            try
            {
                if (CurrentTable != Exporter.CurrentSourceTable)
                {
                    Exporter.CurrentSourceTable = CurrentTable;
                    foreach (DiversityWorkbench.Export.Table T in Exporter.SourceTableList())
                    {
                        if (T.UserControlTable != null)
                            T.UserControlTable.MarkCurrent(CurrentTable);
                    }
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in Exporter.SourceTables)
                    //{
                    //    if (KV.Value.UserControlTable != null)
                    //        KV.Value.UserControlTable.MarkCurrent(CurrentTable);
                    //}
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (this.panelSourceTables.VerticalScroll.Value != Scroll)
                this.panelSourceTables.VerticalScroll.Value = Scroll;
        }

        public void ShowCurrentSourceTableColumns(Export.Table Table)
        {
            string Column = "";
            try
            {
                this.labelColumnsOfTable.Text = Table.TableAlias;
                // Removing the controls of the previous table
                System.Collections.Generic.List<System.Windows.Forms.Control> LL = new List<Control>();
                foreach (System.Windows.Forms.Control C in this.panelSourceTableColumns.Controls)
                    LL.Add(C);
                this.panelSourceTableColumns.Controls.Clear();
                foreach (System.Windows.Forms.Control C in LL)
                    C.Dispose();
                // Adding the controls of the current table
                if (Table != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.TableColumn> KV in Table.TableColumns)
                    {
                        if (Column == "CollectionSpecimenID")
                        { }
                        if (Column == "FamilyCache")
                        { }
                        Column = KV.Key;
                        DiversityWorkbench.Export.UserControlTableColumn U = new UserControlTableColumn(KV.Value, this);
                        U.Dock = DockStyle.Top;
                        this.panelSourceTableColumns.Controls.Add(U);
                        U.BringToFront();
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void InitSourceTables()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.panelSourceTables.SuspendLayout();
            try
            {
                System.Collections.Generic.List<System.Windows.Forms.Control> LL = new List<Control>();
                foreach (System.Windows.Forms.Control C in this.panelSourceTables.Controls)
                    LL.Add(C);
                this.panelSourceTables.Controls.Clear();
                foreach (System.Windows.Forms.Control C in LL)
                    C.Dispose();
                Export.Exporter.ResetSourceTableList();
                this.SuspendLayout();
                int Scroll = this.panelSourceTables.VerticalScroll.Value;
                foreach (Export.Table T in Exporter.SourceTableList())
                {
                    DiversityWorkbench.Export.UserControlTable U = new UserControlTable(T, this);
                    U.Dock = DockStyle.Top;
                    this.panelSourceTables.Controls.Add(U);
                    U.BringToFront();
                }
                this.panelSourceTables.VerticalScroll.Value = Scroll;
                this.ResumeLayout();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.panelSourceTables.ResumeLayout();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void AddChildTables(Export.Table ParentTable)
        {
            foreach (Export.Table T in Exporter.SourceTableList())
            {
                if (T.ParentTable == ParentTable)
                {
                    DiversityWorkbench.Export.UserControlTable U = new UserControlTable(T, this);
                    U.Dock = DockStyle.Top;
                    this.panelSourceTables.Controls.Add(U);
                    U.BringToFront();
                    this.AddChildTables(T);
                }
            }
        }

        public void AddSourceTable(Export.Table Table, Export.Table ParentTable)
        {
            DiversityWorkbench.Export.Exporter.AddSourceTable(Table, ParentTable);
            try
            {
                this.InitSourceTables();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void AddMultiSourceTables(Export.Table Table, Export.Table ParentTable)
        {
            DiversityWorkbench.Export.Exporter.AddMultiSourceTables(Table, ParentTable);
            try
            {
                this.InitSourceTables();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void RemoveSourceTable(Export.Table Table)
        {
            try
            {
                if (Table.ParentTable == null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> TT in Export.Exporter.SourceTableDictionary())
                    {
                        if (TT.Value.ParentTable == Table)
                            TT.Value.ParentTable = null;
                    }
                }
                Export.Exporter.RemoveSourceTable(Table);
                this.InitSourceTables();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region File columns

        public void ShowFileColumns()
        {
            this.SuspendLayout();
            this.panelFileColumns.SuspendLayout();
            bool isFirstControl = true;
            System.Collections.Generic.List<System.Windows.Forms.Control> LL = new List<Control>();
            foreach (System.Windows.Forms.Control C in this.panelFileColumns.Controls)
                LL.Add(C);
            this.panelFileColumns.Controls.Clear();
            foreach (System.Windows.Forms.Control C in LL)
                C.Dispose();
            foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in Exporter.FileColumnList)
            {
                Export.UserControlFileColumn U = new UserControlFileColumn(KV.Value, this, this.helpProvider.HelpNamespace);
                if (isFirstControl)
                    U.SetSeparatorVisibility(false);
                this.panelFileColumns.Controls.Add(U);
                U.Dock = DockStyle.Left;
                U.BringToFront();
                isFirstControl = false;
            }
            this.panelFileColumns.ResumeLayout();
            this.ResumeLayout();
        }

        #endregion

        #region Test

        private async void buttonTestExport_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                Exporter.ResetExportResults();
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ExportResult = await Exporter.TestExport(this.numericUpDownTestExport.Value, this);
                this.dataGridViewTestExport.Rows.Clear();
                this.dataGridViewTestExport.Columns.Clear();
                if (ExportResult.Count > 0)
                {
                    foreach (string S in ExportResult[0])
                    {
                        this.dataGridViewTestExport.Columns.Add(S, S);
                    }
                    for (int i = 1; i < ExportResult.Count; i++)
                    {
                        this.dataGridViewTestExport.Rows.Add();
                        System.Windows.Forms.DataGridViewRow R = new DataGridViewRow();
                        for (int ii = 0; ii < ExportResult[i].Count; ii++)
                        {
                            this.dataGridViewTestExport.Rows[i - 1].Cells[ii].Value = ExportResult[i][ii];
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonShowTestExport_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Form F = new Form();
            F.StartPosition = FormStartPosition.CenterParent;
            F.Icon = this.Icon;
            F.Text = "Test export";
            F.Width = this.Width - 10;
            F.Height = this.Height - 10;
            System.Windows.Forms.DataGridView DG = new DataGridView();
            DG.AllowUserToAddRows = false;
            DG.AllowUserToDeleteRows = false;
            DG.AllowUserToOrderColumns = false;
            DG.ReadOnly = true;
            DG.Dock = DockStyle.Fill;
            DG.RowHeadersVisible = false;
            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewTestExport.Columns)
                DG.Columns.Add(C.Name, C.HeaderText);
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewTestExport.Rows)
            {
                DG.Rows.Add();
                for (int i = 0; i < R.Cells.Count; i++)
                {
                    DG.Rows[DG.Rows.Count - 1].Cells[i].Value = R.Cells[i].Value;
                }
            }
            F.Controls.Add(DG);
            F.ShowDialog();
        }

        private void buttonShowTestExportSQL_Click(object sender, EventArgs e)
        {
            string Message = Exporter.SqlCommandsToString();
            if (Message.Length == 0)
            {
                Message = "No SQL statements documented.";
                if (!Exporter.SqlDocumentationActive)
                    Message += "\r\nPlease activate documentation via checkbox next to the button";
                else Message += "\r\nPlease restart Test";
            }
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("SQL statements", Message, true);
            f.ShowDialog();
            //System.Windows.Forms.MessageBox.Show(Message);
            //System.Windows.Forms.MessageBox.Show("available in upcoming version");
        }
        private void checkBoxGetSql_CheckedChanged(object sender, EventArgs e)
        {
            Exporter.SqlDocumentationActive = this.checkBoxGetSql.Checked;
        }

        #endregion

        #region Export

        private void setExportPaths()
        {
            //string ExportDirectory = ...Windows.Forms.Application.StartupPath + "\\Export";
            //System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(ExportDirectory);
            //if (!D.Exists)
            //    D.Create();
            this.labelDirectoryForExortFile.Text = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export);// D.FullName + "\\";

            string ExportFile = Exporter.StartTable.TableName + "_" + System.DateTime.Now.ToString("yyyyMMdd_hhmmss"); // System.DateTime.Now.Year.ToString();
            //if (System.DateTime.Now.Month < 10) ExportFile += "0";
            //ExportFile += System.DateTime.Now.Month.ToString();
            //if (System.DateTime.Now.Day < 10) ExportFile += "0";
            //ExportFile += System.DateTime.Now.Day.ToString() + "_";
            //if (System.DateTime.Now.Hour < 10) ExportFile += "0";
            //ExportFile += System.DateTime.Now.Hour.ToString();
            //if (System.DateTime.Now.Minute < 10) ExportFile += "0";
            //ExportFile += System.DateTime.Now.Minute.ToString();
            //if (System.DateTime.Now.Second < 10) ExportFile += "0";
            //ExportFile += System.DateTime.Now.Second.ToString();
            this.textBoxExportFileName.Text = ExportFile;
            this.textBoxSchema.TextAlign = HorizontalAlignment.Right;
            this.textBoxSchema.Text = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export) + ExportFile + ".xml";
        }

        private void buttonSetDirectoryForExortFile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog objDialog = new FolderBrowserDialog();
            objDialog.Description = "Folder for export file";
            objDialog.SelectedPath = this.labelDirectoryForExortFile.Text;       // Vorgabe Pfad (und danach der gewählte Pfad)
            DialogResult objResult = objDialog.ShowDialog(this);
            if (objResult == DialogResult.OK)
            {
                this.labelDirectoryForExortFile.Text = objDialog.SelectedPath;
                if (!this.labelDirectoryForExortFile.Text.EndsWith("\\"))
                    this.labelDirectoryForExortFile.Text += "\\";
            }
        }

        private async void buttonStartExport_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (this.checkBoxExportIncludeSchema.Checked)
                DiversityWorkbench.Export.Exporter.SaveSchemaFile(true, this.labelDirectoryForExortFile.Text + this.textBoxExportFileName.Text + ".xml");

            string Message = "";
            if (this.comboBoxExportFileNameExtension.SelectedItem.ToString() == ".txt")
                Message = await DiversityWorkbench.Export.Exporter.ExportToFile(this.labelDirectoryForExortFile.Text + this.textBoxExportFileName.Text + ".txt", this.textBoxExport, this);
            else
                Message = await DiversityWorkbench.Export.Exporter.ExportToXML(this.labelDirectoryForExortFile.Text + this.textBoxExportFileName.Text + ".xml", this.textBoxExport, this, (int)this.numericUpDownTestExport.Maximum);
            if (Message.Length == 0)
                Message = "Export finished";
            System.Windows.Forms.MessageBox.Show(Message);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonExportOpenFile_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo FI = new System.IO.FileInfo(this.labelDirectoryForExortFile.Text + this.textBoxExportFileName.Text + this.comboBoxExportFileNameExtension.Text);// + this.labelExportFileNameExtension.Text);
            if (FI.Exists)
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(FI.FullName);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
        }

        public void ShowExportProgress(int Position, int Max, string Message)
        {
            if (this.progressBarExport.Maximum != Max)
                this.progressBarExport.Maximum = Max;
            if (Position <= this.progressBarExport.Maximum)
                this.progressBarExport.Value = Position;
            if (this.labelExportProgress.Text != Message)
            {
                this.labelExportProgress.Text = Message;
                Application.DoEvents();
            }
        }

        #endregion

        #region SQLite

        private void initSQLite()
        {
            this.labelSQLiteDirectory.Text = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export);// ...Windows.Forms.Application.StartupPath + "\\Export";
        }

        private readonly string _SQLiteExportTableName = "ExportDWB";

        private DiversityWorkbench.SqlLite.Database _SQLiteDB;

        public DiversityWorkbench.SqlLite.Database SQLiteDB
        {
            get
            {
                if (this._SQLiteDB == null)
                {
                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export) + this.textBoxSQLiteDB.Text + ".sqlite";
                    this._SQLiteDB = new SqlLite.Database(Path);
                }
                return _SQLiteDB;
            }
            //set { _SQLiteDB = value; }
        }

        private bool SQLiteHeaderNamesOK()
        {
            bool OK = true;
            System.Collections.Generic.List<string> UnfitHeaders = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Export.FileColumn> KV in DiversityWorkbench.Export.Exporter.FileColumnList)
            {
                if (!KV.Value.IsSeparatedFromPreviousColumn)
                    continue;
                string Header = KV.Value.Header;
                if (!DiversityWorkbench.SqlLite.Database.IsValidName(KV.Value.Header))
                {
                    UnfitHeaders.Add(Header);
                    OK = false;
                }
            }
            if (UnfitHeaders.Count > 0)
            {
                string Message = "The following headers must be changed to a column compatible string:\r\n";
                foreach (string H in UnfitHeaders)
                {
                    Message += H + "\r\n";
                }
                System.Windows.Forms.MessageBox.Show(Message);
            }
            return OK;
        }

        private async void buttonSQLiteExport_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;

            if (!this.SQLiteHeaderNamesOK())
                return;
            if (DiversityWorkbench.Export.Exporter.FileColumnList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No columns selected");
                return;
            }
            try
            {
                //System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(...Windows.Forms.Application.StartupPath + "\\Export");
                //if (!DI.Exists)
                //    DI.Create();
                if (this.textBoxSQLiteDB.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a name for the database");
                    return;
                }
                System.Collections.Generic.Dictionary<string, string> TableColumns = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Export.FileColumn> KV in DiversityWorkbench.Export.Exporter.FileColumnList)
                {
                    if (!KV.Value.IsSeparatedFromPreviousColumn)
                        continue;
                    string DataType = KV.Value.TableColumn.DataType;
                    if (KV.Value.TableColumnUnitValue != null)
                    {
                        DataType += "(";
                        if (KV.Value.TableColumn.DataTypeLength == -1)
                            DataType += "4000";
                        else DataType += KV.Value.TableColumn.DataTypeLength.ToString();
                        DataType += ")";
                    }
                    string ExportColumn = KV.Value.Header.Replace(" ", "");
                    if (TableColumns.ContainsKey(ExportColumn))
                    {
                        System.Windows.Forms.MessageBox.Show("The column names must be unique.\r\nColumn\r\n" + ExportColumn + "\r\nis used more than once");
                        return;
                    }
                    TableColumns.Add(ExportColumn, DataType);
                    ColumnValues.Add(ExportColumn, "");
                }
                if (!this.SQLiteDB.AddTable(_SQLiteExportTableName, TableColumns))
                    return;
                System.Collections.Generic.List<System.Collections.Generic.List<string>> LL = await Exporter.ExportResults(null, this);
                this.progressBarSQLite.Value = 0;
                this.progressBarSQLite.Maximum = LL.Count;
                for (int l = 1; l < LL.Count; l++)
                {
                    for (int c = 0; c < LL[l].Count; c++)
                    {
                        ColumnValues[LL[0][c]] = LL[l][c];
                    }
                    this._SQLiteDB.InsertData(_SQLiteExportTableName, ColumnValues);
                    if (l <= this.progressBarSQLite.Maximum)
                        this.progressBarSQLite.Value = l;
                }
                this.progressBarSQLite.Value = this.progressBarSQLite.Maximum;
                System.Windows.Forms.MessageBox.Show("Export finished");
                this.buttonSQLiteView.Enabled = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSQLiteView_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;


            try
            {
                System.Data.DataTable dt = this.SQLiteDB.ViewTableContent(this._SQLiteExportTableName);
                DiversityWorkbench.Forms.FormTableContent f = new Forms.FormTableContent("Export content", "Content as exported to SQLite database " + this.SQLiteDB.DatabaseName(), dt);
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void textBoxSQLiteDB_TextChanged(object sender, EventArgs e)
        {
            this.buttonSQLiteView.Enabled = false;
            this._SQLiteDB = null;
        }

        #endregion

        #region Schema

        private void buttonOpenSchema_Click(object sender, EventArgs e)
        {
            //System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(...Windows.Forms.Application.StartupPath + "\\Export");
            //if (!D.Exists)
            //    D.Create();
            openFileDialog.Filter = "XML Files|*.xml";
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Export);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.SuspendLayout();
                    this.groupBoxFileColumns.SuspendLayout();
                    this.groupBoxSourceTables.SuspendLayout();
                    this.textBoxSchema.Text = openFileDialog.FileName;
                    this.textBoxSchema.TextAlign = HorizontalAlignment.Left;
                    if (Export.Exporter.LoadSchemaFile(openFileDialog.FileName))
                    {
                        string HtmlFile = DiversityWorkbench.Export.Exporter.ShowConvertedFile(openFileDialog.FileName);
                        System.Uri U = new Uri(HtmlFile);
                        this.webBrowserSchema.Url = U;
                        this.InitSourceTables();
                        this.ShowFileColumns();
                    }
                    this.groupBoxSourceTables.ResumeLayout();
                    this.groupBoxFileColumns.ResumeLayout();
                    this.ResumeLayout();
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonSaveSchema_Click(object sender, EventArgs e)
        {
            try
            {
                if (Export.Exporter.FileColumns.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No data for export selected");
                    return;
                }
                //if (!DiversityWorkbench.Export.Exporter.ImportPreconditionsOK())
                //{
                //    if (System.Windows.Forms.MessageBox.Show("Save incorrect schema?", "Save", MessageBoxButtons.YesNo) == DialogResult.No)
                //        return;
                //}
                string SchemaFile = DiversityWorkbench.Export.Exporter.SaveSchemaFile(true, this.textBoxSchema.Text);
                this.textBoxSchema.Text = SchemaFile;
                this.textBoxSchema.TextAlign = HorizontalAlignment.Left;
                string HtmlFile = DiversityWorkbench.Export.Exporter.ShowConvertedFile(SchemaFile);
                System.Uri U = new Uri(HtmlFile);
                this.webBrowserSchema.Url = U;
            }
            catch (Exception ex)
            {
            }
        }

        private void buttonShowSchema_Click(object sender, EventArgs e)
        {
            if (this.webBrowserSchema.Url == null)
                return;
            if (this.webBrowserSchema.Url.ToString().Length > 0)
            {
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.webBrowserSchema.Url.ToString());
                f.ShowDialog();
            }
        }

        private void buttonResetSchema_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Export.Exporter.ResetUserActions();

            Export.Exporter.InitSourceTables();
            this.InitSourceTables();
            this.ShowCurrentSourceTableColumns(null);
            this.webBrowserSchema.Url = null;
            this.ShowFileColumns();
            this.setExportPaths();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        #endregion

        #region Obsolet

        //private void BuildTemplateTableHierarchy()
        //{
        //    for (int i = 0; i < Exporter.TemplateTables.Count; i++)
        //    {
        //        this.imageListSourceTables.Images.Add(Exporter.TemplateTables[i].Image);
        //    }
        //    this.treeViewTemplateTables.ImageList = this.imageListSourceTables;
        //    for (int i = 0; i < Exporter.TemplateTables.Count; i++)
        //    {
        //        if (Exporter.TemplateTables[i].ParentTable == null)
        //        {
        //            System.Windows.Forms.TreeNode N = new TreeNode(Exporter.TemplateTables[i].DisplayText, i, i);
        //            N.Tag = Exporter.TemplateTables[i];
        //            this.treeViewTemplateTables.Nodes.Add(N);
        //            //this.treeViewSourceTables.No
        //        }
        //        else
        //        {
        //            foreach (System.Windows.Forms.TreeNode N in this.treeViewTemplateTables.Nodes)
        //            {
        //                DiversityWorkbench.Export.Table ParentTable = (DiversityWorkbench.Export.Table)N.Tag;
        //                if (Exporter.TemplateTables[i].ParentTable.TableName == ParentTable.TableName)
        //                {
        //                    System.Windows.Forms.TreeNode NC = new TreeNode(Exporter.TemplateTables[i].DisplayText, i, i);
        //                    NC.Tag = Exporter.TemplateTables[i];
        //                    N.Nodes.Add(NC);
        //                }
        //                else
        //                    this.AddTemplateTableHierarchyNode(N, Exporter.TemplateTables[i], i);
        //            }
        //        }
        //    }
        //    this.treeViewTemplateTables.ExpandAll();
        //}

        //private void AddTemplateTableHierarchyNode(System.Windows.Forms.TreeNode N, DiversityWorkbench.Export.Table Table, int ImageIndex)
        //{
        //    foreach (System.Windows.Forms.TreeNode NP in N.Nodes)
        //    {
        //        DiversityWorkbench.Export.Table ParentTable = (DiversityWorkbench.Export.Table)NP.Tag;
        //        if (Table.ParentTable.TableName == ParentTable.TableName)
        //        {
        //            System.Windows.Forms.TreeNode NC = new TreeNode(Table.DisplayText, ImageIndex, ImageIndex);
        //            NC.Tag = Table;
        //            N.Nodes.Add(NC);
        //        }
        //        else
        //            this.AddTemplateTableHierarchyNode(N, Table, ImageIndex);
        //    }
        //}

        //private void buttonUseSourceTable_Click(object sender, EventArgs e)
        //{
        //    if (this.treeViewTemplateTables.SelectedNode != null)
        //    {
        //        try
        //        {
        //            //DiversityWorkbench.Export.Table T = (DiversityWorkbench.Export.Table)this.treeViewTemplateTables.SelectedNode.Tag;
        //            //this._Exporter.SourceTables.Add(T.Position, T);
        //            //this.panelSourceTablesAboveStart.Controls.Clear();
        //            //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in this._Exporter.SourceTables)
        //            //{
        //            //    DiversityWorkbench.Export.UserControlTable U = new UserControlTable(KV.Value, this);
        //            //    U.Dock = DockStyle.Top;
        //            //    this.panelSourceTablesAboveStart.Controls.Add(U);
        //            //}
        //        }
        //        catch (System.Exception ex) { }
        //    }
        //}

        //private void fillSourceTables()
        //{

        //}

        //#region Selecting tables

        //private void treeViewTemplateTables_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    this.ActivatePotentialTargetTables();
        //    this.MarkSelectedNode();
        //}

        //private void ActivatePotentialTargetTables()
        //{
        //    //DiversityWorkbench.Export.Table Template = (DiversityWorkbench.Export.Table)this.treeViewTemplateTables.SelectedNode.Tag;
        //    //foreach (System.Windows.Forms.Control C in this.panelSourceTables.Controls)
        //    //{
        //    //    DiversityWorkbench.Export.UserControlTable U = (DiversityWorkbench.Export.UserControlTable)C;
        //    //    U.SetPotentialJoinTableTarget(Template);
        //    //}
        //}

        //private void MarkSelectedNode()
        //{
        //    foreach (System.Windows.Forms.TreeNode N in this.treeViewTemplateTables.Nodes)
        //    {
        //        if (N == this.treeViewTemplateTables.SelectedNode)
        //            N.BackColor = System.Drawing.Color.Pink;
        //        else
        //            N.BackColor = System.Drawing.Color.White;
        //        this.MarkSelectedNode(N);
        //    }
        //}

        //private void MarkSelectedNode(System.Windows.Forms.TreeNode ParentNode)
        //{
        //    foreach (System.Windows.Forms.TreeNode N in ParentNode.Nodes)
        //    {
        //        if (N == this.treeViewTemplateTables.SelectedNode)
        //            N.BackColor = System.Drawing.Color.Pink;
        //        else
        //            N.BackColor = System.Drawing.Color.White;
        //        this.MarkSelectedNode(N);
        //    }
        //}

        //#endregion

        //#region Drag & Drop
        //#endregion

        //#region Table templates

        //private void treeViewTemplateTables_DragDrop(object sender, DragEventArgs e)
        //{
        //    //try
        //    //{
        //    //    System.Data.DataRowView rvName = (System.Data.DataRowView)e.Data.GetData("System.Data.DataRowView");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw;
        //    //}
        //}

        //private void treeViewTemplateTables_MouseDown(object sender, MouseEventArgs e)
        //{

        //}

        //private void treeViewTemplateTables_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //    //System.Windows.Forms.TreeNode N = (System.Windows.Forms.TreeNode)e.Item;
        //    //if (N != null)
        //    //{
        //    //    DiversityWorkbench.Export.Table T = (DiversityWorkbench.Export.Table)N.Tag;
        //    //    this.treeViewTemplateTables.DoDragDrop(T, DragDropEffects.All);
        //    //}
        //    //this.treeViewTemplateTables.DoDragDrop(e.Item, System.Windows.Forms.DragDropEffects.All);
        //}

        //#endregion

        //private void panelFileColums_DragDrop(object sender, DragEventArgs e)
        //{

        //}

        //private void panelFileColums_DragEnter(object sender, DragEventArgs e)
        //{
        //    DiversityWorkbench.Export.TableColumn TC = (DiversityWorkbench.Export.TableColumn)e.Data.GetData("DiversityWorkbench.Export.TableColumn");
        //    Export.FileColumn FC = new FileColumn(TC);
        //    //if (T.ParentTable.TableName == this._Table.TableName)
        //    //{
        //    //    e.Effect = DragDropEffects.Copy;
        //    //}
        //    //else
        //    //    e.Effect = DragDropEffects.None;
        //}

        //private void labelFileColums_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetType() == typeof(DiversityWorkbench.Export.TableColumn) ||
        //        e.Data.GetType() == typeof(DiversityWorkbench.Export.TableColumnUnitValue))
        //    {
        //        e.Effect = DragDropEffects.Copy;
        //    }
        //    else
        //        e.Effect = DragDropEffects.None;
        //}

        //private void labelFileColums_DragDrop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetType() == typeof(DiversityWorkbench.Export.TableColumn))
        //    {
        //        DiversityWorkbench.Export.Table T = (DiversityWorkbench.Export.Table)e.Data.GetData("DiversityWorkbench.Export.Table");
        //        //this._Exporter.
        //    }
        //    else if (e.Data.GetType() == typeof(DiversityWorkbench.Export.TableColumnUnitValue))
        //    {
        //    }
        //    //this._iExporter.AddSourceTable(T, this._Table);
        //}

        #endregion

    }
}
