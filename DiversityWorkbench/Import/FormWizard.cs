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
    public partial class FormWizard : Form, iWizardInterface
    {

        #region Parameter

        //public enum Direction { Import, Export }

        //private DiversityWorkbench.Import.Import.Direction _Direction;

        //private string _FileName;
        //public string FileName()
        //{
        //    if (this._FileName == null)
        //        this._FileName = "";
        //    return _FileName;
        //}

        //private string _SchemaName;
        //public string SchemaName()
        //{
        //    if (this._SchemaName == null)
        //        this._SchemaName = "";
        //    return _SchemaName;
        //}

        //private string _Encoding;
        //public string Encoding()
        //{
        //    if (this._Encoding == null)
        //        this._Encoding = "";
        //    return _Encoding;
        //}

        //private int _StartLine;
        //public int StartLine() { return this._StartLine; }
        //private int _EndLine;
        //public int EndLine() { return this._EndLine; }

        //private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTable> _DataTables;

        //public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTable> DataTables
        //{
        //    get { return _DataTables; }
        //    set { _DataTables = value; }
        //}

        //private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> _ImportSteps;
        private iStartingMessage _iStartingMessage;
        
        #endregion

        #region Construction

        /// <summary>
        /// a import formular for importing data into the database
        /// </summary>
        /// <param name="Target">The target of the import within the database and the schema file</param>
        /// <param name="ImportSteps">Dictionary of the steps</param>
        public FormWizard(string Target, string DatabaseVersion, System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportSteps, string HelpNameSpace)
        {
            InitializeComponent();
            DiversityWorkbench.Import.Import.ResetTemplate();
            DiversityWorkbench.Import.Import.Reset();
            DiversityWorkbench.Import.Import.TableListForImport = null;
            DiversityWorkbench.Import.Import.FileName = "";
            DiversityWorkbench.Import.Import.Target = Target;
            DiversityWorkbench.Import.Import.DBversion = DatabaseVersion;
            DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = null;
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in ImportSteps)
            {
                if (!DiversityWorkbench.Import.Import.TemplateSteps.ContainsKey(KV.Key))
                    DiversityWorkbench.Import.Import.TemplateSteps.Add(KV.Key, KV.Value);
            }
            this.helpProvider.HelpNamespace = HelpNameSpace;
            this.initForm();
        }

        /// <summary>
        /// a import formular for importing data into the database
        /// </summary>
        /// <param name="Target">The target of the import within the database and the schema file</param>
        /// <param name="ImportSteps">Dictionary of the steps</param>
        /// <param name="HelpNameSpace">The name of the help space</param>
        /// <param name="IMessage">The interface showing the current step of the start routines</param>
        public FormWizard(string Target, string DatabaseVersion, System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportSteps, string HelpNameSpace, iStartingMessage IMessage)
        {
            InitializeComponent();
            this._iStartingMessage = IMessage;
            this._iStartingMessage.ShowCurrentStep("Reset the templates for the import tables\r\n|||||||||.");
            DiversityWorkbench.Import.Import.ResetTemplate();
            DiversityWorkbench.Import.Import.Reset();
            DiversityWorkbench.Import.Import.TableListForImport = null;
            DiversityWorkbench.Import.Import.FileName = "";
            DiversityWorkbench.Import.Import.Target = Target;
            DiversityWorkbench.Import.Import.DBversion = DatabaseVersion;
            DiversityWorkbench.Import.Import.SelectedImportSchemaIsValid = null;
            this._iStartingMessage.ShowCurrentStep("Creating the templates for the import tables\r\n|||||||||.");
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in ImportSteps)
            {
                if (!DiversityWorkbench.Import.Import.TemplateSteps.ContainsKey(KV.Key))
                    DiversityWorkbench.Import.Import.TemplateSteps.Add(KV.Key, KV.Value);
            }
            this.helpProvider.HelpNamespace = HelpNameSpace;
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {
            this.Text = "Import";
            if (this._iStartingMessage != null)
                this._iStartingMessage.ShowCurrentStep("Creating the steps for the import\r\n||||||||||");
            foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in DiversityWorkbench.Import.Import.Steps)
            {
                if (KV.Value.IsSelected)
                {
                    string Target = KV.Value.DisplayText.ToLower();
                    if (Target.IndexOf(" ") > -1)
                        Target = Target.Substring(0, Target.IndexOf(" ")).Trim();
                    this.Text = this.Text + " " + Target + " data";
                    break;
                }
            }

            this.showSteps();
            if (this._iStartingMessage != null)
                this._iStartingMessage.ShowCurrentStep("Setting the start step for the import\r\n||||||||||");
            this.SetStartStep();
            if (this.Width < 1100)
                this.Width = 1100;
        }

        public void SetStartStep()
        {
            // Markus 26.2.2019: removing all controls in the panelFile from previous actions
            foreach (System.Windows.Forms.Control C in this.panelFile.Controls)
            {
                //bool IsDisposableControl = false;
                //try
                //{
                //    DiversityWorkbench.Import.iDisposableControl i = (DiversityWorkbench.Import.iDisposableControl)C;
                //    IsDisposableControl = true;
                //}
                //catch { }
                //if (IsDisposableControl)
                //{
                //    DiversityWorkbench.Import.iDisposableControl UC = (DiversityWorkbench.Import.iDisposableControl)C;
                //    UC.DisposeComponents();
                //}
                C.Dispose();
            }
            this.panelFile.Controls.Clear();
            // insert new control
            DiversityWorkbench.Import.Step SF = DiversityWorkbench.Import.Step.GetStep(Step.StepType.File, this.imageListSteps.Images[0]);
            DiversityWorkbench.Import.UserControlStep Uf = new UserControlStep(SF, this, this.helpProvider.HelpNamespace);
            this.panelFile.Controls.Add(Uf);
            // setting the header
            this.setDataHeader(SF);
            // Markus 26.2.2019: removing all controls in the DataColumnPanel from previous actions
            foreach (System.Windows.Forms.Control C in this.DataColumnPanel().Controls)
            {
                //bool IsDisposableControl = false;
                //try
                //{
                //    DiversityWorkbench.Import.iDisposableControl i = (DiversityWorkbench.Import.iDisposableControl)C;
                //    IsDisposableControl = true;
                //}
                //catch { }
                //if (IsDisposableControl)
                //{
                //    DiversityWorkbench.Import.iDisposableControl UC = (DiversityWorkbench.Import.iDisposableControl)C;
                //    UC.DisposeComponents();
                //}
                C.Dispose();
            }
            this.DataColumnPanel().Controls.Clear();
            // Adding the new control
            DiversityWorkbench.Import.UserControlFile Ufile = new UserControlFile(this);
            Ufile.Dock = DockStyle.Fill;
            this.DataColumnPanel().Controls.Add(Ufile);
        }

        private void showSteps()
        {
            this.RefreshStepSelectionPanel();
            this.RefreshStepPanel();
            foreach (System.Windows.Forms.Control C in this.panelData.Controls)
            {
                //bool IsDisposableControl = false;
                //try
                //{
                //    DiversityWorkbench.Import.iDisposableControl i = (DiversityWorkbench.Import.iDisposableControl)C;
                //    IsDisposableControl = true;
                //}
                //catch { }
                //if (IsDisposableControl)
                //{
                //    DiversityWorkbench.Import.iDisposableControl UC = (DiversityWorkbench.Import.iDisposableControl)C;
                //    UC.DisposeComponents();
                //}
                C.Dispose();
            }
            this.panelData.Controls.Clear();
            foreach (System.Windows.Forms.Control C in this.panelFileColumn.Controls)
            {
                //bool IsDisposableControl = false;
                //try
                //{
                //    DiversityWorkbench.Import.iDisposableControl i = (DiversityWorkbench.Import.iDisposableControl)C;
                //    IsDisposableControl = true;
                //}
                //catch { }
                //if (IsDisposableControl)
                //{
                //    DiversityWorkbench.Import.iDisposableControl UC = (DiversityWorkbench.Import.iDisposableControl)C;
                //    UC.DisposeComponents();
                //}
                C.Dispose();
            }
            this.panelFileColumn.Controls.Clear();
        }

        public void RefreshStepSelectionPanel()
        {
            this.panelStepSelection.SuspendLayout();
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                foreach (System.Windows.Forms.Control C in this.panelStepSelection.Controls)
                {
                    //bool IsDisposableControl = false;
                    //try
                    //{
                    //    DiversityWorkbench.Import.iDisposableControl i = (DiversityWorkbench.Import.iDisposableControl)C;
                    //    IsDisposableControl = true;
                    //}
                    //catch { }
                    //if (IsDisposableControl)
                    //{
                    //    DiversityWorkbench.Import.iDisposableControl UC = (DiversityWorkbench.Import.iDisposableControl)C;
                    //    UC.DisposeComponents();
                    //}
                    C.Dispose();
                }
                this.panelStepSelection.Controls.Clear();
                System.Collections.Generic.Stack<string> StepStack = new Stack<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in DiversityWorkbench.Import.Import.Steps)
                {
                    StepStack.Push(KV.Key);
                }
                while (StepStack.Count > 0)
                {
                    string sStep = StepStack.Pop();
                    DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Import.Steps[sStep];
                    if (S != null && S.TypeOfStep == Step.StepType.Table)
                    {
                        DiversityWorkbench.Import.UserControlStepSelector ucS = new UserControlStepSelector(S, this);
                        ucS.Dock = DockStyle.Top;
                        ucS.SendToBack();
                        try
                        {
                            this.panelStepSelection.Controls.Add(ucS);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "this.panelStepSelection.Controls.Add(ucS);");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.panelStepSelection.ResumeLayout();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public void RefreshStepPanel()
        {
            this.panelSteps.SuspendLayout();
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                foreach (System.Windows.Forms.Control C in this.panelSteps.Controls)
                {
                    C.Dispose();
                }
                this.panelSteps.Controls.Clear();

                foreach (System.Windows.Forms.Control C in this.panelData.Controls)
                {
                    C.Dispose();
                }
                this.panelData.Controls.Clear();

                this.setDataHeader(null);
                this.AddFinishSteps();
                System.Collections.Generic.Stack<string> StepStack = new Stack<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in DiversityWorkbench.Import.Import.Steps)
                {
                    if (KV.Value.TypeOfStep == Step.StepType.Table)
                        StepStack.Push(KV.Key);
                }
                while (StepStack.Count > 0)
                {
                    try
                    {
                        string sStep = StepStack.Pop();
                        DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Import.Steps[sStep];
                        if (S != null && S.IsSelected)
                        {
                            if (S.TypeOfStep == Step.StepType.Table)
                            {
                                if (this.ShowLoggingColumns())
                                {
                                    DiversityWorkbench.Import.StepColumnGroup SL = new StepColumnGroup(this.imageListLogging.Images[1], "Logging", DiversityWorkbench.Import.Import.Tables[S.DataTableTemplate.TableAlias].LoggingColumns);
                                    DiversityWorkbench.Import.UserControlStep uCSL = new UserControlStep(S, SL, this, this.helpProvider.HelpNamespace);
                                    uCSL.Dock = DockStyle.Top;
                                    try
                                    {
                                        this.panelSteps.Controls.Add(uCSL);
                                    }
                                    catch (System.Exception ex)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "this.panelSteps.Controls.Add(uCSL)");
                                    }
                                }
                                if (S.StepColumnGroups != null && S.StepColumnGroups.Count > 0)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.StepColumnGroup> KV in S.StepColumnGroups)
                                    {
                                        DiversityWorkbench.Import.UserControlStep uCSg = new UserControlStep(S, KV.Value, this, this.helpProvider.HelpNamespace);
                                        uCSg.Dock = DockStyle.Top;
                                        try
                                        {
                                            this.panelSteps.Controls.Add(uCSg);
                                        }
                                        catch (System.Exception ex)
                                        {
                                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "this.panelSteps.Controls.Add(uCSg)");
                                        }
                                    }
                                }
                                DiversityWorkbench.Import.UserControlStep ucS = new UserControlStep(S, this, this.helpProvider.HelpNamespace);
                                ucS.Dock = DockStyle.Top;
                                try
                                {
                                    this.panelSteps.Controls.Add(ucS);
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "this.panelSteps.Controls.Add(ucS)");
                                }
                            }
                        }
                    }
                    catch (System.Exception ex) 
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                this.AddHeaderSteps();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.panelSteps.ResumeLayout();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public System.Windows.Forms.Panel DataColumnPanel()
        {
            return this.panelData;
        }

        public System.Windows.Forms.DataGridView DataGridView()
        {
            return this.dataGridView;
        }

        public System.Windows.Forms.Panel DataHeaderPanel()
        {
            return this.panelDataHeader;
        }

        #region Progress bar

        public void setProgressBarPosition(int Position)
        {
            if (Position >= this.progressBar.Minimum &&
                Position <= this.progressBar.Maximum)
                this.progressBar.Value = Position;

            if (Position % 100 == 0)
                System.Windows.Forms.Application.DoEvents();
        }

        public void setProgressBarMaximum(int Maximum)
        {
            this.progressBar.Maximum = Maximum;
        }

        public void setProgressBarVisibility(bool IsVisible)
        {
            this.progressBar.Visible = IsVisible;
        }

        public void SetLockState(bool locked)
        {
            this.toolStrip.Enabled = !locked;
            this.splitContainerMain.Enabled = !locked;
        }
     
        #endregion

        /// <summary>
        /// setting the header of a step in the main form
        /// </summary>
        /// <param name="Step"></param>
        public void setDataHeader(DiversityWorkbench.Import.Step Step)
        {
            if (Step == null)
            {
                this.pictureBoxDataHeaderStep.Image = null;
                this.labelDataHeaderStep.Text = "";
                this.setTableMessage(null);
                this.pictureBoxDataHeaderMergeHandling.Image = null;
                this.labelDataHeaderMergeHandling.Text = "";
            }
            else
            {
                this.pictureBoxDataHeaderStep.Image = Step.Image;
                this.labelDataHeaderStep.Text = Step.DisplayText;
                if (Step.TypeOfStep != DiversityWorkbench.Import.Step.StepType.Table)
                {
                    this.labelDataHeaderStep.ForeColor = System.Drawing.Color.Blue;

                    this.pictureBoxDataHeaderMergeHandling.Image = null;
                    this.labelDataHeaderMergeHandling.Text = "";
                }
                else
                {
                    this.labelDataHeaderStep.ForeColor = System.Drawing.Color.Black;

                    this.setMergeHandlingControls(Step);
                }
            }
            this.labelDataHeaderGroup.Visible = false;
            this.pictureBoxDataHeaderGroup.Visible = false;
        }

        /// <summary>
        /// setting the header of a step group in the main form
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="Group"></param>
        public void setDataHeader(DiversityWorkbench.Import.Step Step, DiversityWorkbench.Import.StepColumnGroup Group)
        {
            this.pictureBoxDataHeaderStep.Image = Step.Image;
            this.labelDataHeaderStep.Text = Step.DisplayText;
            this.labelDataHeaderGroup.Visible = true;
            this.labelDataHeaderGroup.Text = Group.DisplayText;
            this.pictureBoxDataHeaderGroup.Visible = true;
            this.pictureBoxDataHeaderGroup.Image = Group.Image;
            this.labelDataHeaderStep.ForeColor = System.Drawing.Color.Black;

            this.setMergeHandlingControls(Step);
        }

        public System.Collections.Generic.Dictionary<Import.SchemaFileSource, string> SourcesForSchemaFiles()
        {
            return this._SourcesForSchemaFiles;
        }

        public void SetSourcesForSchemaFiles(System.Collections.Generic.Dictionary<Import.SchemaFileSource, string> Sources)
        {
            this._SourcesForSchemaFiles = Sources;
        }

        private System.Collections.Generic.Dictionary<Import.SchemaFileSource, string> _SourcesForSchemaFiles;


        private void setMergeHandlingControls(DiversityWorkbench.Import.Step Step)
        {
            this.labelDataHeaderMergeHandling.Text = DiversityWorkbench.Import.Import.Tables[Step.TableAlias].MergeHandling.ToString();
            if (DiversityWorkbench.Import.Import.Tables[Step.TableAlias].IsForAttachment)
            {
                this.pictureBoxDataHeaderMergeHandling.Image = this.imageListMergeHandling.Images[3];
                this.labelDataHeaderMergeHandling.Text = "Attach";
            }
            else
            {
                this.labelDataHeaderMergeHandling.Text = DiversityWorkbench.Import.Import.Tables[Step.TableAlias].MergeHandling.ToString();
                switch (DiversityWorkbench.Import.Import.Tables[Step.TableAlias].MergeHandling)
                {
                    case DataTable.Merging.Insert:
                        this.pictureBoxDataHeaderMergeHandling.Image = this.imageListMergeHandling.Images[0];
                        break;
                    case DataTable.Merging.Merge:
                        this.pictureBoxDataHeaderMergeHandling.Image = this.imageListMergeHandling.Images[1];
                        break;
                    case DataTable.Merging.Update:
                        this.pictureBoxDataHeaderMergeHandling.Image = this.imageListMergeHandling.Images[2];
                        break;
                    case DataTable.Merging.Attach:
                        this.pictureBoxDataHeaderMergeHandling.Image = this.imageListMergeHandling.Images[3];
                        break;
                }
            }
        }

        /// <summary>
        /// if the file and encoding are selected
        /// </summary>
        /// <returns></returns>
        public bool FileIsOK()
        {
            if (DiversityWorkbench.Import.Import.FileName.Length > 0
                && DiversityWorkbench.Import.Import.Encoding != null
                && DiversityWorkbench.Import.Import.StartLine > 0
                && DiversityWorkbench.Import.Import.StartLine <= DiversityWorkbench.Import.Import.EndLine)
                return true;
            else return false;
        }

        /// <summary>
        /// Adding the steps before the table steps like File and Attachment
        /// </summary>
        private void AddHeaderSteps()
        {
            DiversityWorkbench.Import.Step SM = DiversityWorkbench.Import.Step.GetStep(Step.StepType.Merging, this.imageListSteps.Images[2]);
            DiversityWorkbench.Import.UserControlStep Um = new UserControlStep(SM, this, this.helpProvider.HelpNamespace);
            Um.Dock = DockStyle.Top;
            this.panelSteps.Controls.Add(Um);

            bool HasAttachmentColumns = false;
            foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
            {
                if (KV.Value.AttachmentColumns != null && KV.Value.AttachmentColumns.Count > 0)
                {
                    HasAttachmentColumns = true;
                    break;
                }
            }
            if (HasAttachmentColumns)
            {
                DiversityWorkbench.Import.Step SA = DiversityWorkbench.Import.Step.GetStep(Step.StepType.Attachment, this.imageListSteps.Images[1]);
                DiversityWorkbench.Import.UserControlStep Ua = new UserControlStep(SA, this, this.helpProvider.HelpNamespace);
                Ua.Dock = DockStyle.Top;
                this.panelSteps.Controls.Add(Ua);
            }
        }

        /// <summary>
        /// Adding the last steps after the table steps like Analysis and Import
        /// </summary>
        private void AddFinishSteps()
        {
            DiversityWorkbench.Import.Step SI = DiversityWorkbench.Import.Step.GetStep(Step.StepType.Import, this.imageListSteps.Images[4]);
            DiversityWorkbench.Import.UserControlStep Ui = new UserControlStep(SI, this, this.helpProvider.HelpNamespace);
            Ui.Dock = DockStyle.Top;
            this.panelSteps.Controls.Add(Ui);

            DiversityWorkbench.Import.Step SA = DiversityWorkbench.Import.Step.GetStep(Step.StepType.Testing, this.imageListSteps.Images[3]);
            DiversityWorkbench.Import.UserControlStep Ua = new UserControlStep(SA, this, this.helpProvider.HelpNamespace);
            Ua.Dock = DockStyle.Top;
            this.panelSteps.Controls.Add(Ua);
        }

        public void setTableMessage(DiversityWorkbench.Import.DataTable DataTable)
        {
            try
            {
                System.Collections.Generic.List<System.Windows.Forms.Control> TableMessageControls = new List<Control>();
                // removing the old message controls
                foreach (System.Windows.Forms.Control C in this.panelDataHeader.Controls)
                {
                    if (C.Tag != null && C.Tag.ToString() == "TableMessage")
                        TableMessageControls.Add(C);
                }
                foreach (System.Windows.Forms.Control C in TableMessageControls)
                {
                    if (this.panelDataHeader.Controls.Contains(C))
                    {
                        C.Dispose();
                    }
                    else
                    {
                    }
                }
                // adding the message contols according to the current step
                if (DataTable != null)
                {
                    string Message = "";
                    DiversityWorkbench.Import.DataTable.TableMessageType MessageType = DataTable.GetTableMessage(ref Message);
                    DiversityWorkbench.Import.UserControlTableMessage U = new UserControlTableMessage(MessageType, Message);
                    U.Dock = DockStyle.Right;
                    U.Tag = "TableMessage";
                    this.panelDataHeader.Controls.Add(U);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void FormWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.splitContainerMain.Enabled)
            {
                if (MessageBox.Show("Import is running!\r\n\r\nAbort import?", "Abort import?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
                    e.Cancel = true;
            }
        }
       
        #endregion

        #region File
        
        private void SetFile()
        {
            try
            {
                if (DiversityWorkbench.Import.Import.Encoding != null
                    || DiversityWorkbench.Import.Import.FileName.Length == 0)
                    return;
                System.IO.FileInfo f = new System.IO.FileInfo(DiversityWorkbench.Import.Import.FileName);
                DiversityWorkbench.Import.Import.readFileInDataGridView(f, this.dataGridView, DiversityWorkbench.Import.Import.Encoding, null);
                this.dataGridView.ReadOnly = true;
                this.dataGridView.AllowUserToAddRows = false;
                this.dataGridView.AllowUserToDeleteRows = false;
                //this.setDataGridColorRange();
                //DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    C.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.setDataGridColorRange();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Datagrid

        public void FreezeHaederline()
        {
            try 
            {
                if (this.dataGridView.Rows.Count > 0)
                { 
                    if (DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition)
                        this.dataGridView.Rows[0].Frozen = true;
                    else
                        this.dataGridView.Rows[0].Frozen = false; 
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        public void setDataGridColorRange()
        {
            int iAttachmentColumn = -1;
            if (DiversityWorkbench.Import.Import.AttachmentColumn != null && DiversityWorkbench.Import.Import.AttachmentColumn.FileColumn != null)
            {
                iAttachmentColumn = (int)DiversityWorkbench.Import.Import.AttachmentColumn.FileColumn;
            }
            for (int iLine = 0; iLine < this.dataGridView.Rows.Count; iLine++)
            {
                if (iLine + 1 < (int)DiversityWorkbench.Import.Import.StartLine ||
                    iLine + 1 > (int)DiversityWorkbench.Import.Import.EndLine)
                {
                    for (int i = 0; i < this.dataGridView.Columns.Count; i++)
                    {
                        this.dataGridView.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                    this.dataGridView.Rows[iLine].DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    for (int i = 0; i < this.dataGridView.Columns.Count; i++)
                    {
                        if (iAttachmentColumn == i)
                            this.dataGridView.Rows[iLine].Cells[i].Style.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
                        else
                            this.dataGridView.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.SystemColors.Window;
                    }
                    this.dataGridView.Rows[iLine].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }
            }
            this._GridHeadersSet = false;
            this.dataGridView_Paint(null, null);
        }

        public void setDataGridLineColor(int iLine, System.Drawing.Color Color)
        {
            try
            {
                for (int i = 0; i < this.dataGridView.Columns.Count; i++)
                {
                    this.dataGridView.Rows[iLine - 1].Cells[i].Style.BackColor = Color;
                }
            }
            catch (System.Exception ex) { }
        }

        private System.Collections.Generic.Dictionary<int, string> _GridHeaders;

        public void SetGridHeaders()
        {
            try
            {
                this._GridHeaders = new Dictionary<int, string>();
                foreach (string TA in DiversityWorkbench.Import.Import.TableListForImport)//.SelectedTableAliases)
                {
                    if (DiversityWorkbench.Import.Import.Tables.ContainsKey(TA))
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> DC in
                            DiversityWorkbench.Import.Import.Tables[TA].DataColumns)
                        {
                            if (DC.Value.IsSelected
                                && DC.Value.TypeOfSource == DataColumn.SourceType.File
                                && DC.Value.FileColumn != null)
                            {
                                string Header = DC.Value.DataTable.GetDisplayText() + "\r\n  " + DC.Value.DisplayText;
                                if (_GridHeaders.ContainsKey((int)DC.Value.FileColumn))
                                    _GridHeaders[(int)DC.Value.FileColumn] += "\r\n" + Header;
                                else _GridHeaders.Add((int)DC.Value.FileColumn, Header);
                                if (DC.Value.MultiColumns.Count > 0)
                                {
                                    foreach (DiversityWorkbench.Import.ColumnMulti CM in DC.Value.MultiColumns)
                                    {
                                        if (_GridHeaders.ContainsKey(CM.ColumnInFile))
                                            _GridHeaders[CM.ColumnInFile] += "\r\n" + Header;
                                        else _GridHeaders.Add(CM.ColumnInFile, Header);
                                    }
                                }
                            }
                        }
                    }
                }

                this.dataGridView.AllowUserToDeleteRows = false;
                this.dataGridView.AllowUserToAddRows = false;
                this.dataGridView.ReadOnly = true;
                this._GridHeadersSet = false;
                this.dataGridView_Paint(null, null);
            }
            catch (System.Exception ex) { }
        }

        private bool _GridHeadersSet = false;

        public void setActiveDataGridRow(int RowNumber)
        {
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                R.Selected = false;
            if (this.dataGridView.Rows.Count >= RowNumber)
                this.dataGridView.Rows[RowNumber - 1].Selected = true;
        }

        private void dataGridView_Paint(object sender, PaintEventArgs e)
        {
            if (this._GridHeaders != null && !this._GridHeadersSet)
            {
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    C.HeaderText = "";
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in _GridHeaders)
                {
                    if (this.dataGridView.Columns.Count > KV.Key)
                        this.dataGridView.Columns[KV.Key].HeaderText = KV.Value;
                }
                this._GridHeadersSet = true;
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string rowNumber =
                (e.RowIndex + 1).ToString()
                .PadLeft(this.dataGridView.RowCount.ToString().Length);

            // Schriftgröße:
            SizeF size = e.Graphics.MeasureString(rowNumber, this.Font);

            // Breite des ZeilenHeaders anpassen:
            if (this.dataGridView.RowHeadersWidth < (int)(size.Width + 20))
                this.dataGridView.RowHeadersWidth = (int)(size.Width + 20);

            // ZeilenNr zeichnen:
            e.Graphics.DrawString(
                rowNumber,
                this.Font,
                SystemBrushes.ControlText,
                e.RowBounds.Location.X + 15,
                e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void toolStripComboBoxColumnHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxColumnHeaders.SelectedIndex == 0)
                this.dataGridView.ColumnHeadersVisible = true;
            else this.dataGridView.ColumnHeadersVisible = false;
        }

        #endregion

        #region Logging
        
        private void toolStripButtonLogging_Click(object sender, EventArgs e)
        {
            if (this.toolStripButtonLogging.Tag.ToString() == "0")
            {
                this.toolStripButtonLogging.Text = "Show logging columns";
                this.toolStripButtonLogging.Tag = "1";
                this.toolStripButtonLogging.Image = this.imageListLogging.Images[1];
            }
            else
            {
                this.toolStripButtonLogging.Text = "Hide logging columns";
                this.toolStripButtonLogging.Tag = "0";
                this.toolStripButtonLogging.Image = this.imageListLogging.Images[0];
            }
            this.RefreshStepPanel();
        }

        public bool ShowLoggingColumns()
        {
            if (this.toolStripButtonLogging.Tag.ToString() == "0") return false;
            else return true;
        }
        
        #endregion

        #region Feedback
        
        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }
        
        #endregion

    }
}
