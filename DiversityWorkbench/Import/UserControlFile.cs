using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;

namespace DiversityWorkbench.Import
{
    public partial class UserControlFile : UserControl//, iDisposableControl
    {
        #region Parameter

        private DiversityWorkbench.Import.iWizardInterface _WizardInterface;
        //private bool? _SelectedImportSchemaIsValid;

        #endregion

        #region Construction and control

        public UserControlFile(DiversityWorkbench.Import.iWizardInterface WizardInterface)
        {
            InitializeComponent();
            this._WizardInterface = WizardInterface;
            this.initControl();
            if (WizardInterface.SourcesForSchemaFiles() != null)
            {
                this.toolStripDropDownButtonGitHub.Visible = true;
                if (WizardInterface.SourcesForSchemaFiles().ContainsKey(Import.SchemaFileSource.SNSB))
                {
                    this.sNSBToolStripMenuItem.Tag = WizardInterface.SourcesForSchemaFiles()[Import.SchemaFileSource.SNSB];
                    this.sNSBToolStripMenuItem.Visible = true;
                }
                else this.sNSBToolStripMenuItem.Visible = false;
                if (WizardInterface.SourcesForSchemaFiles().ContainsKey(Import.SchemaFileSource.ZFMK))
                {
                    this.zFMKToolStripMenuItem.Tag = WizardInterface.SourcesForSchemaFiles()[Import.SchemaFileSource.ZFMK];
                    this.zFMKToolStripMenuItem.Visible = true;
                }
                else this.zFMKToolStripMenuItem.Visible = false;
            }
            else
                this.toolStripDropDownButtonGitHub.Visible = false;
        }

        private void initControl()
        {
#if !DEBUG
            //this.labelSeparator.Visible = false;
            //this.comboBoxSeparator.Visible = false;
            //this.checkBoxTranslateReturn.Visible = false;
#endif

            this.comboBoxSeparator.Items.Clear();
            this.comboBoxSeparator.Items.Add("TAB");
            this.comboBoxSeparator.Items.Add(";");
            switch (DiversityWorkbench.Import.Import.Separator)
            {
                case Import.separator.SEMICOLON:
                    this.comboBoxSeparator.SelectedIndex = 1;
                    break;
                default:
                    this.comboBoxSeparator.SelectedIndex = 0;
                    break;
            }

            this.numericUpDownStartLine.Maximum = DiversityWorkbench.Import.Import.LastLine;
            this.numericUpDownEndLine.Maximum = DiversityWorkbench.Import.Import.LastLine;

            this.checkBoxTranslateReturn.Checked = Import.TranslateReturn;

            if (DiversityWorkbench.Import.Import.StartLine > -1
                && DiversityWorkbench.Import.Import.StartLine >= this.numericUpDownStartLine.Minimum
                && DiversityWorkbench.Import.Import.StartLine <= this.numericUpDownStartLine.Maximum)
                this.numericUpDownStartLine.Value = DiversityWorkbench.Import.Import.StartLine;

            if (DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition)
                this.numericUpDownStartLine.Minimum = 2;
            else
                this.numericUpDownStartLine.Minimum = 1;

            if (DiversityWorkbench.Import.Import.EndLine > -1
                && DiversityWorkbench.Import.Import.EndLine >= this.numericUpDownEndLine.Minimum
                && DiversityWorkbench.Import.Import.EndLine <= this.numericUpDownEndLine.Maximum)
                this.numericUpDownEndLine.Value = DiversityWorkbench.Import.Import.EndLine;
            this.checkBoxFirstLine.Checked = DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition;
            this.textBoxFile.Text = DiversityWorkbench.Import.Import.FileName;
            this.adaptDefaultDirectoryToolStripMenuItem.Checked = DiversityWorkbench.Import.SettingsImport.Default.AdaptDefaultDirectory;
            this.textBoxSchema.Text = DiversityWorkbench.Import.Import.SchemaName;
            if (this.textBoxFile.Text.Length > 0 && this.textBoxSchema.Text.Length > 0)
            {
                string HtmlFile = DiversityWorkbench.Import.Import.ShowConvertedFile(this.textBoxSchema.Text);
                System.Uri U = new Uri(HtmlFile);
                this.webBrowserSchema.Url = U;
            }
            this.setEncodingList();
            if (DiversityWorkbench.Import.Import.Encoding != null)
            {
                int i = 0;
                bool EncodingPresent = false;
                foreach (System.Object O in this.comboBoxEncoding.Items)
                {
                    if (Import.Encodings[O.ToString()] == DiversityWorkbench.Import.Import.Encoding)
                    {
                        EncodingPresent = true;
                        break;
                    }
                    i++;
                }
                if (EncodingPresent)
                    this.comboBoxEncoding.SelectedIndex = i;
            }
            this.initDefaultDuplicateCheck();
            this.checkBoxRecordSQL.Checked = RecordSQL.Record;
            this.ShowMessage();
        }

        /// <summary>
        /// Show a message in the webbrowser what the user should to as long as no valid schema is selected
        /// or the selected schema is not valid
        /// </summary>
        private void ShowMessage()
        {
            if (DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid != null && (bool)DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid)
                return;

            try
            {
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                // creating the file for the message
                string SchemaFileName = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Import) + "ImportMessage.xml";
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(SchemaFileName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("ImportSchedule");
                W.WriteStartElement("Message");
                if (DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid == null)
                {
                    W.WriteAttributeString("Type", "ToDo");
                    W.WriteStartElement("Notes");
                    if (this.textBoxFile.Text.Length == 0)
                        W.WriteElementString("Note", "Please select a file from where the data should be imported (format: text, tab-separated)");
                    if (this.textBoxSchema.Text.Length == 0 && this.comboBoxEncoding.Text.Length == 0)
                        W.WriteElementString("Note", "Please select an encoding or a valid schema");
                    W.WriteEndElement();//ToDo
                }
                else if (!(bool)DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid)
                {
                    W.WriteAttributeString("Type", "Error");
                    W.WriteStartElement("Notes");
                    if (DiversityWorkbench.Import.Import.SchemaModule != DiversityWorkbench.Settings.ModuleName)
                        W.WriteElementString("Note", "The selected schema belongs to the module " + DiversityWorkbench.Import.Import.SchemaModule + ". The current module is " + DiversityWorkbench.Settings.ModuleName);
                    if (DiversityWorkbench.Import.Import.SchemaTarget != DiversityWorkbench.Import.Import.Target)
                        W.WriteElementString("Note", "The target of the selected schema is " + DiversityWorkbench.Import.Import.SchemaTarget + ". The current target is " + DiversityWorkbench.Import.Import.Target);
                    if (this.textBoxSchema.Text.Length == 0 && this.comboBoxEncoding.Text.Length == 0)
                        W.WriteElementString("Note", "Please select an encoding or a valid schema");
                    W.WriteEndElement();//Errors
                }
                W.WriteEndElement();//Message
                W.WriteEndElement();//ImportSchema
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                string HtmlFile = DiversityWorkbench.Import.Import.ShowConvertedFile(SchemaFileName);
                System.Uri U = new Uri(HtmlFile);
                this.webBrowserSchema.Url = U;
            }
            catch (System.Exception ex) { }
        }

        //public void DisposeComponents()
        //{
        //    this.toolTip.Dispose();
        //    this.openFileDialog.Dispose();
        //    this.imageListLanguage.Dispose();
        //    this.toolStripLanguage.Dispose();
        //}

        #endregion

        #region Encoding

        //private System.Collections.Generic.Dictionary<string, System.Text.Encoding> _Encodings = new Dictionary<string, Encoding>();

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

        public System.Text.Encoding Encoding
        {
            get
            {
                System.Text.Encoding E = System.Text.Encoding.UTF8;
                if (Import.Encodings.Count > 0)
                {
                    E = Import.Encodings[this.comboBoxEncoding.SelectedItem.ToString()];
                }
                return E;
            }
        }

        public void setEncoding(string Encoding)
        {
            int i = 0;
            foreach (System.Object o in this.comboBoxEncoding.Items)
            {
                if (o.ToString() == Encoding)
                    break;
                i++;
            }
            this.comboBoxEncoding.SelectedIndex = i;
        }

        private void comboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Encoding != null)
            {
                bool SetLineRange = true;
                if (this._WizardInterface.DataGridView().Rows.Count > 0)
                    SetLineRange = false;
                if (DiversityWorkbench.Import.Import.Encoding != this.Encoding)
                {
                    DiversityWorkbench.Import.Import.Encoding = this.Encoding;
                    // #253 Check encoding of file
                    this.CheckFileEncoding(DiversityWorkbench.Import.Import.FileName);
                    this.ShowMessage();
                    this.SetFile(SetLineRange);
                }
            }
        }

        #endregion

        #region File

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                //DiversityWorkbench.Import.Import.Reset();
                bool DefaultDirectoryDoesExist = false;
                this.openFileDialog = new OpenFileDialog();
                if (DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory != null && DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory.Length > 0)
                {
                    // Markus 23.04.24 - Checking if directory exists - Anforderung Birgit Rach - Exception when changing servers
                    System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory);
                    if (directoryInfo.Exists)
                    {
                        this.openFileDialog.InitialDirectory = DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory;
                        this.openFileDialog.RestoreDirectory = true;
                        DefaultDirectoryDoesExist = true;
                    }
                }
                //this.openFileDialog.RestoreDirectory = true;
                this.openFileDialog.Multiselect = false;
                if (this.textBoxFile.Text.Length > 0)
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxFile.Text);
                    this.openFileDialog.InitialDirectory = FI.DirectoryName;
                }
                else
                {
                    string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Import);
                    if (DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory != null && DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory.Length > 0)
                        Path = DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory;
                    System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Path);// ...Windows.Forms.Application.StartupPath + "\\Import");
                    if (!D.Exists)
                    {
                        try
                        {
                            D.Create();
                            this.openFileDialog.InitialDirectory = D.FullName;
                            DefaultDirectoryDoesExist = true;
                        }
                        catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
                }
                if (DiversityWorkbench.Import.SettingsImport.Default.AdaptDefaultDirectory 
                    && this.openFileDialog.InitialDirectory != null 
                    && this.openFileDialog.InitialDirectory.Length > 0 
                    && DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory != this.openFileDialog.InitialDirectory
                    && DefaultDirectoryDoesExist)
                {
                    DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory = this.openFileDialog.InitialDirectory;
                    DiversityWorkbench.Import.SettingsImport.Default.Save();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            //this.openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
#if DEBUG
#endif
            this.openFileDialog.Filter = "Text Files (*.csv;*.txt)|*.csv;*.txt|All Files|*.*";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    // Check encoding #253
                    this.CheckFileEncoding(f.FullName);
                    //try
                    //{
                    //    System.Text.Encoding encoding = Forms.FormFunctions.DetectEncoding(f.FullName);
                    //    if (encoding != null && encoding.BodyName != this.Encoding.BodyName)
                    //    {
                    //        System.Windows.Forms.MessageBox.Show("The encoding of the file (" + encoding.BodyName + ")\r\n" +
                    //            "does not match the current encoding (" + this.Encoding.BodyName + ")", "Wrong encoding", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                    //}
                    //catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

                    if (f.DirectoryName != null && f.DirectoryName.Length > 0 && SettingsImport.Default.DefaultDirectory != f.DirectoryName)
                    {
                        SettingsImport.Default.DefaultDirectory = f.DirectoryName;
                        SettingsImport.Default.Save();
                    }
                    this.textBoxFile.Text = f.FullName;
                    DiversityWorkbench.Import.Import.FileName = f.FullName;
                    this.SetFile(true);
                }
                // Vorschlag Toni - fehlende Teile übernommen
                //if (this.openFileDialog.FileName.Length > 0)
                //{
                //    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                //    if (f.DirectoryName != null && f.DirectoryName.Length > 0 && SettingsImport.Default.DefaultDirectory != f.DirectoryName)
                //    {
                //        SettingsImport.Default.DefaultDirectory = f.DirectoryName;
                //        SettingsImport.Default.Save();
                //    }

                //}
                this.ShowMessage();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool CheckFileEncoding(string FileName)
        {
            // Check encoding #253
            try
            {
                Tuple<Encoding, bool> tuple = DiversityWorkbench.Forms.FormFunctions.DetectEncoding(FileName);
                System.Text.Encoding encoding = tuple.Item1;
                if (encoding != null && encoding.BodyName != this.Encoding.BodyName && tuple.Item2)
                {
                    string SelectedEncoding = this.Encoding.BodyName;
                    if (this.Encoding.CodePage == 1252) SelectedEncoding = this.Encoding.WebName;
                    System.Windows.Forms.MessageBox.Show("The encoding of the file (" + encoding.BodyName + ")\r\n" +
                    "does not match the current encoding (" + SelectedEncoding + ")", "Wrong encoding", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
                else if (!tuple.Item2) //encoding.BodyName == "utf-8")
                {
                    System.Windows.Forms.MessageBox.Show("The encoding of the file could not be detected due to missing BOM (= Byte order mark)\r\n" +
                    "Please check the content for deviating special signs", "Encoding?", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else { /* ToDo */ }
                    return true;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); return false; }
        }

        private void SetFile(bool setLineRange, bool ReloadFile = false)
        {
            try
            {
                if ((this.comboBoxEncoding.SelectedIndex < 1 || this.textBoxFile.Text.Length == 0) && !ReloadFile)
                    return;

                string File = this.textBoxFile.Text;
                if (File.Length == 0 && DiversityWorkbench.Import.Import.FileName.Length > 0 && ReloadFile)
                    File = DiversityWorkbench.Import.Import.FileName;
                if (File.Length == 0)
                    return;

                System.IO.FileInfo f = new System.IO.FileInfo(File);

                this._WizardInterface.DataGridView().AllowUserToAddRows = false;
                this._WizardInterface.DataGridView().ReadOnly = true;

                DiversityWorkbench.Import.Import.FileName = f.FullName;
                if (DiversityWorkbench.Import.Import.readFileInDataGridView(f, this._WizardInterface.DataGridView(), this.Encoding, null))
                {

                    //int Maximum = this._WizardInterface.DataGridView().Rows.Count;
                    DiversityWorkbench.Import.Import.LastLine = this._WizardInterface.DataGridView().Rows.Count;

                    int Minimum = 1;
                    if (DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition)
                        Minimum = 2;
                    this._WizardInterface.FreezeHaederline();

                    // Startline
                    this.numericUpDownStartLine.Maximum = DiversityWorkbench.Import.Import.LastLine;
                    if (DiversityWorkbench.Import.Import.StartLine < Minimum)
                        DiversityWorkbench.Import.Import.StartLine = Minimum;
                    this.numericUpDownStartLine.Value = DiversityWorkbench.Import.Import.StartLine;

                    // Endline
                    this.numericUpDownEndLine.Maximum = DiversityWorkbench.Import.Import.LastLine;
                    if (DiversityWorkbench.Import.Import.EndLine > DiversityWorkbench.Import.Import.LastLine ||
                        DiversityWorkbench.Import.Import.EndLine < DiversityWorkbench.Import.Import.StartLine ||
                        setLineRange) // || DiversityWorkbench.Import.Import.EndLine == DiversityWorkbench.Import.Import.StartLine)
                        DiversityWorkbench.Import.Import.EndLine = DiversityWorkbench.Import.Import.LastLine;
                    this.numericUpDownEndLine.Value = DiversityWorkbench.Import.Import.EndLine;

                    // Grid
                    this._WizardInterface.DataGridView().Visible = true;
                    this._WizardInterface.DataGridView().Dock = DockStyle.Fill;
                    foreach (System.Windows.Forms.DataGridViewColumn C in this._WizardInterface.DataGridView().Columns)
                        C.SortMode = DataGridViewColumnSortMode.NotSortable;
                    this._WizardInterface.setDataGridColorRange();

                }
                else
                {
                    this._WizardInterface.DataGridView().Visible = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public bool FileIsOK()
        {
            if (this.textBoxFile.Text.Length > 0
                && DiversityWorkbench.Import.Import.FileName.Length > 0
                && this.comboBoxEncoding.Text.Length > 0
                && DiversityWorkbench.Import.Import.Encoding != null
                && this.numericUpDownStartLine.Value > 0
                && DiversityWorkbench.Import.Import.StartLine > 0
                && this.numericUpDownStartLine.Value <= this.numericUpDownEndLine.Value
                && DiversityWorkbench.Import.Import.StartLine <= DiversityWorkbench.Import.Import.EndLine)
                return true;
            else return false;
        }

        private void adaptDefaultDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.SettingsImport.Default.AdaptDefaultDirectory = adaptDefaultDirectoryToolStripMenuItem.Checked;
        }

        #endregion

        #region Line range

        private void labelEndLine_DoubleClick(object sender, EventArgs e)
        {
            if (numericUpDownEndLine.Value < numericUpDownEndLine.Maximum)
                numericUpDownEndLine.Value = numericUpDownEndLine.Maximum;
        }

        private void labelStartLine_DoubleClick(object sender, EventArgs e)
        {
            if (numericUpDownStartLine.Value > numericUpDownStartLine.Minimum)
                numericUpDownStartLine.Value = numericUpDownStartLine.Minimum;
        }

        private void numericUpDownEndLine_ValueChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.EndLine = (int)numericUpDownEndLine.Value;
            if (DiversityWorkbench.Import.Import.StartLine > DiversityWorkbench.Import.Import.EndLine)
            {
                if (DiversityWorkbench.Import.Import.EndLine >= this.numericUpDownStartLine.Minimum)
                    this.numericUpDownStartLine.Value = DiversityWorkbench.Import.Import.EndLine;
                else
                    this.numericUpDownEndLine.Value = this.numericUpDownStartLine.Minimum;
            }
            this._WizardInterface.setDataGridColorRange();
        }

        private void numericUpDownStartLine_ValueChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.StartLine = (int)numericUpDownStartLine.Value;
            if (DiversityWorkbench.Import.Import.EndLine < DiversityWorkbench.Import.Import.StartLine)
            {
                this.numericUpDownEndLine.Value = DiversityWorkbench.Import.Import.StartLine;
            }
            this._WizardInterface.setDataGridColorRange();
        }

        private void checkBoxFirstLine_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition = this.checkBoxFirstLine.Checked;
                if (this.checkBoxFirstLine.Checked)
                    this.numericUpDownStartLine.Minimum = 2;
                else this.numericUpDownStartLine.Minimum = 1;
                this._WizardInterface.FreezeHaederline();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Schema

        private void buttonOpenSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxFile.Text.Length == 0 || DiversityWorkbench.Import.Import.FileName.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select the source file for the import");
                return;
            }
            this.openFileDialog = new OpenFileDialog();
            if (DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory != null && DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory.Length > 0)
                this.openFileDialog.InitialDirectory = DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory;
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            if (this.textBoxSchema.Text.Length > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchema.Text);
                this.openFileDialog.InitialDirectory = FI.DirectoryName;
            }
            else
            {
                System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Import));// ...Windows.Forms.Application.StartupPath + "\\Import");
                if (!D.Exists)
                    D.Create();
                this.openFileDialog.InitialDirectory = D.FullName;
            }
            if (this.openFileDialog.InitialDirectory != null && this.openFileDialog.InitialDirectory.Length > 0)
            {
                DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory = this.openFileDialog.InitialDirectory;
                DiversityWorkbench.Import.SettingsImport.Default.Save();
            }
            this.openFileDialog.Filter = "Schema Files|*.xml|All Files|*.*";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxSchema.Text = f.FullName;
                    DiversityWorkbench.Import.Import.SchemaName = f.FullName;
                    if (DiversityWorkbench.Import.Import.LoadSchemaFile(this.textBoxSchema.Text))
                    {
                        int oldEncoding = this.comboBoxEncoding.SelectedIndex;
                        this.setEncoding(DiversityWorkbench.Import.Import.EncodingDisplayText(DiversityWorkbench.Import.Import.Encoding));
                        if (oldEncoding != this.comboBoxEncoding.SelectedIndex)
                            SetFile(false); // Toni 20180608: Reload file after schema since encoding might have changes
                        if (DiversityWorkbench.Import.Import.StartLine >= this.numericUpDownStartLine.Minimum && DiversityWorkbench.Import.Import.StartLine <= this.numericUpDownStartLine.Maximum)
                            this.numericUpDownStartLine.Value = DiversityWorkbench.Import.Import.StartLine;
                        else
                            this.numericUpDownStartLine.Value = this.numericUpDownStartLine.Minimum;
                        if (DiversityWorkbench.Import.Import.EndLine <= this.numericUpDownEndLine.Maximum)
                            this.numericUpDownEndLine.Value = DiversityWorkbench.Import.Import.EndLine;
                        else
                            this.numericUpDownEndLine.Value = this.numericUpDownEndLine.Maximum;
                        this.checkBoxFirstLine.Checked = DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition;
                        switch (DiversityWorkbench.Import.Import.Separator)
                        {
                            case Import.separator.SEMICOLON:
                                this.comboBoxSeparator.SelectedIndex = 1;
                                break;
                            default:
                                this.comboBoxSeparator.SelectedIndex = 0;
                                break;
                        }
                        string HtmlFile = DiversityWorkbench.Import.Import.ShowConvertedFile(this.textBoxSchema.Text);
                        System.Uri U = new Uri(HtmlFile);
                        this.webBrowserSchema.Url = U;
                        DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = true;
                        this._WizardInterface.RefreshStepSelectionPanel();
                        if (this._WizardInterface.DataGridView().Rows.Count == 0)
                            this.SetFile(false);
                        this._WizardInterface.SetGridHeaders();
                        this._WizardInterface.RefreshStepPanel();
                        this._WizardInterface.SetStartStep();
                    }
                    else
                    {
                        DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = false;
                        this.ShowMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonRemoveSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxSchema.Text.Length > 0)
            {
                DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = false;
                this.textBoxSchema.Text = "";
                DiversityWorkbench.Import.Import.SchemaName = "";
                DiversityWorkbench.Import.Import.Reset();
                this._WizardInterface.RefreshStepSelectionPanel();
                this._WizardInterface.SetGridHeaders();
                this._WizardInterface.RefreshStepPanel();
                this._WizardInterface.SetStartStep();
                this.ShowMessage();
            }
        }

        private void buttonShowSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxSchema.Text.Length > 0)
            {
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.webBrowserSchema.Url.ToString());
                f.AllowNavigation(false);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No schema selected");
        }


        private void sNSBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(this.sNSBToolStripMenuItem.Tag.ToString());
            info.UseShellExecute = true;
            System.Diagnostics.Process.Start(info);
        }

        private void zFMKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(this.zFMKToolStripMenuItem.Tag.ToString());
            info.UseShellExecute = true;
            System.Diagnostics.Process.Start(info);
        }

        private void toolStripButtonRemoveSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxSchema.Text.Length > 0)
            {
                DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = false;
                this.textBoxSchema.Text = "";
                DiversityWorkbench.Import.Import.SchemaName = "";
                DiversityWorkbench.Import.Import.Reset();
                this._WizardInterface.RefreshStepSelectionPanel();
                this._WizardInterface.SetGridHeaders();
                this._WizardInterface.RefreshStepPanel();
                this._WizardInterface.SetStartStep();
                this.ShowMessage();
            }
        }

        private void toolStripButtonOpenSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxFile.Text.Length == 0 || DiversityWorkbench.Import.Import.FileName.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select the source file for the import");
                return;
            }
            this.openFileDialog = new OpenFileDialog();
            if (DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory != null && DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory.Length > 0)
                this.openFileDialog.InitialDirectory = DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory;
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            if (this.textBoxSchema.Text.Length > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchema.Text);
                this.openFileDialog.InitialDirectory = FI.DirectoryName;
            }
            else
            {
                string Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Import);
                if (DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory != null && DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory.Length > 0)
                    Path = DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory;
                System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Path);// ...Windows.Forms.Application.StartupPath + "\\Import");
                if (!D.Exists)
                    D.Create();
                this.openFileDialog.InitialDirectory = D.FullName;
            }
            if (this.openFileDialog.InitialDirectory != null && this.openFileDialog.InitialDirectory.Length > 0 && DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory != this.openFileDialog.InitialDirectory)
            {
                DiversityWorkbench.Import.SettingsImport.Default.DefaultDirectory = this.openFileDialog.InitialDirectory;
                DiversityWorkbench.Import.SettingsImport.Default.Save();
            }
            this.openFileDialog.Filter = "Schema Files|*.xml|All Files|*.*";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxSchema.Text = f.FullName;
                    DiversityWorkbench.Import.Import.SchemaName = f.FullName;
                    if (DiversityWorkbench.Import.Import.LoadSchemaFile(this.textBoxSchema.Text))
                    {
                        switch (DiversityWorkbench.Import.Import.Separator)
                        {
                            case Import.separator.SEMICOLON:
                                this.comboBoxSeparator.SelectedIndex = 1;
                                break;
                            default:
                                this.comboBoxSeparator.SelectedIndex = 0;
                                break;
                        }
                        int oldEncoding = this.comboBoxEncoding.SelectedIndex;
                        this.setEncoding(DiversityWorkbench.Import.Import.EncodingDisplayText(DiversityWorkbench.Import.Import.Encoding));
                        if (oldEncoding != this.comboBoxEncoding.SelectedIndex)
                            SetFile(false); // Toni 20180608: Reload file after schema since encoding might have changes
                        if (DiversityWorkbench.Import.Import.StartLine >= this.numericUpDownStartLine.Minimum && DiversityWorkbench.Import.Import.StartLine <= this.numericUpDownStartLine.Maximum)
                            this.numericUpDownStartLine.Value = DiversityWorkbench.Import.Import.StartLine;
                        else
                            this.numericUpDownStartLine.Value = this.numericUpDownStartLine.Minimum;
                        int? Endline = null;
                        if (DiversityWorkbench.Import.Import.EndLine <= this.numericUpDownEndLine.Maximum)
                        {
                            this.numericUpDownEndLine.Value = DiversityWorkbench.Import.Import.EndLine;
                            Endline = DiversityWorkbench.Import.Import.EndLine;
                        }
                        else
                            this.numericUpDownEndLine.Value = this.numericUpDownEndLine.Maximum;
                        this.checkBoxFirstLine.Checked = DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition;
                        this.checkBoxTranslateReturn.Checked = DiversityWorkbench.Import.Import.TranslateReturn;
                        string HtmlFile = DiversityWorkbench.Import.Import.ShowConvertedFile(this.textBoxSchema.Text);
                        System.Uri U = new Uri(HtmlFile);
                        this.webBrowserSchema.Url = U;
                        DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = true;
                        this._WizardInterface.RefreshStepSelectionPanel();
                        if (this._WizardInterface.DataGridView().Rows.Count == 0)
                            this.SetFile(false);
                        this._WizardInterface.SetGridHeaders();
                        this._WizardInterface.RefreshStepPanel();
                        this._WizardInterface.SetStartStep();
                        this.SetFile(true, true);
                        if (Endline != null && Import.EndLine != (int)Endline)
                        {
                            Import.EndLine = (int)Endline;
                            this.numericUpDownEndLine.Value = Import.EndLine;
                        }
                    }
                    else
                    {
                        DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = false;
                        this.ShowMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonShowSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxSchema.Text.Length > 0)
            {
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.webBrowserSchema.Url.ToString());
                f.AllowNavigation(false);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No schema selected");
        }

        #endregion

        #region Language

        private void toolStripMenuItemGerman_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.CurrentLanguage = DiversityWorkbench.Import.Import.Langage.de;
            if (DiversityWorkbench.Import.Import.CurrentLanguage != DiversityWorkbench.Import.Import.Langage.de)
                this.toolStripMenuItemGerman.Enabled = false;
            this.toolStripDropDownButtonLanguage_Paint(null, null);
            //this.SetCurrentLanguage();
        }

        private void toolStripMenuItemEnglish_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.CurrentLanguage = DiversityWorkbench.Import.Import.Langage.en;
            if (DiversityWorkbench.Import.Import.CurrentLanguage != DiversityWorkbench.Import.Import.Langage.en)
                this.toolStripMenuItemEnglish.Enabled = false;
            this.toolStripDropDownButtonLanguage_Paint(null, null);
            //this.SetCurrentLanguage();
        }

        private void toolStripMenuItemSpanish_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.CurrentLanguage = DiversityWorkbench.Import.Import.Langage.sp;
            if (DiversityWorkbench.Import.Import.CurrentLanguage != DiversityWorkbench.Import.Import.Langage.sp)
                this.toolStripMenuItemSpanish.Enabled = false;
            this.toolStripDropDownButtonLanguage_Paint(null, null);
            //this.SetCurrentLanguage();
        }

        private void toolStripMenuItemFrensh_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.CurrentLanguage = DiversityWorkbench.Import.Import.Langage.fr;
            if (DiversityWorkbench.Import.Import.CurrentLanguage != DiversityWorkbench.Import.Import.Langage.fr)
                this.toolStripMenuItemFrensh.Enabled = false;
            this.toolStripDropDownButtonLanguage_Paint(null, null);
            //this.SetCurrentLanguage();
        }

        private void toolStripMenuItemItaly_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.CurrentLanguage = DiversityWorkbench.Import.Import.Langage.it;
            if (DiversityWorkbench.Import.Import.CurrentLanguage != DiversityWorkbench.Import.Import.Langage.it)
                this.toolStripMenuItemItaly.Enabled = false;
            this.toolStripDropDownButtonLanguage_Paint(null, null);
            //this.SetCurrentLanguage();
        }

        private void uSAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.CurrentLanguage = DiversityWorkbench.Import.Import.Langage.US;
            this.toolStripDropDownButtonLanguage_Paint(null, null);
        }

        private void invariantCultureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.CurrentLanguage = DiversityWorkbench.Import.Import.Langage.IVL;
            this.toolStripDropDownButtonLanguage_Paint(null, null);
        }

        //public void SetCurrentLanguage()
        //{
        //}

        private void toolStripDropDownButtonLanguage_Paint(object sender, PaintEventArgs e)
        {
            switch (DiversityWorkbench.Import.Import.CurrentLanguage)
            {
                case Import.Langage.de:
                    this.toolStripDropDownButtonLanguage.Image = this.toolStripMenuItemGerman.Image;
                    break;
                case Import.Langage.US:
                    this.toolStripDropDownButtonLanguage.Image = this.uSAToolStripMenuItem.Image;
                    break;
                case Import.Langage.en:
                    this.toolStripDropDownButtonLanguage.Image = this.toolStripMenuItemEnglish.Image;
                    break;
                case Import.Langage.fr:
                    this.toolStripDropDownButtonLanguage.Image = this.toolStripMenuItemFrensh.Image;
                    break;
                case Import.Langage.sp:
                    this.toolStripDropDownButtonLanguage.Image = this.toolStripMenuItemSpanish.Image;
                    break;
                case Import.Langage.it:
                    this.toolStripDropDownButtonLanguage.Image = this.toolStripMenuItemItaly.Image;
                    break;
                case Import.Langage.IVL:
                    this.toolStripDropDownButtonLanguage.Image = this.invariantCultureToolStripMenuItem.Image;
                    break;
            }
            this.toolStripDropDownButtonLanguage.Text = DiversityWorkbench.Import.Import.CurrentLanguage.ToString();
        }

        #endregion

        #region Default duplicate check

        private void initDefaultDuplicateCheck()
        {
            if (DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn != null)
            {
                this.checkBoxUseDefaultDuplicateCheck.Visible = true;
                this.checkBoxUseDefaultDuplicateCheck.Checked = DiversityWorkbench.Import.Import.UseDefaultDuplicateCheck;
                this.checkBoxUseDefaultDuplicateCheck.Text = "Use default duplicate check for " + DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName;
            }
            else
            {
                this.checkBoxUseDefaultDuplicateCheck.Visible = false;
                DiversityWorkbench.Import.Import.UseDefaultDuplicateCheck = false;
            }
        }

        private void checkBoxUseDefaultDuplicateCheck_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.UseDefaultDuplicateCheck = this.checkBoxUseDefaultDuplicateCheck.Checked;
        }

        #endregion

        #region Include pre and postfix - replaced by option in attachment step

        private void initIncludePreAndPostfixForComparision()
        {
            //replaced by option in attachment step
            //this.checkBoxIncludePreAndPostfix.Checked = DiversityWorkbench.Import.Import.AttachmentUseTransformation;
        }

        private void checkBoxIncludePreAndPostfix_Click(object sender, EventArgs e)
        {
            //replaced by option in attachment step
            //DiversityWorkbench.Import.Import.AttachmentUseTransformation = this.checkBoxIncludePreAndPostfix.Checked;
        }

        #endregion


        #region Separator
        private void comboBoxSeparator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (comboBoxSeparator.SelectedItem)
            {
                case ";":
                    DiversityWorkbench.Import.Import.Separator = Import.separator.SEMICOLON;
                    break;
                default:
                    DiversityWorkbench.Import.Import.Separator = Import.separator.TAB;
                    break;
            }
            // #253
            this.SetFile(false);
        }

        #endregion

        #region RecordSQL
        private void checkBoxRecordSQL_CheckedChanged(object sender, EventArgs e)
        {
            RecordSQL.Record = this.checkBoxRecordSQL.Checked;
        }

        private void buttonOpenRecordSQL_Click(object sender, EventArgs e)
        {
            if (RecordSQL.LastRecordFile != null && RecordSQL.LastRecordFile.Length > 0)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(RecordSQL.LastRecordFile);
                if (fileInfo.Exists)
                {
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(fileInfo.FullName);
                    info.UseShellExecute = true;
                    System.Diagnostics.Process.Start(info);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("No file present");
        }

        #endregion

        #region Translate return
        private void checkBoxTranslateReturn_CheckedChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.TranslateReturn = this.checkBoxTranslateReturn.Checked;
        }

        #endregion

    }
}
