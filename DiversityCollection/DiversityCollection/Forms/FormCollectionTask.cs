using DiversityWorkbench.DwbManual;
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
    public partial class FormCollectionTask : Form
    {
        #region Parameter

        private DiversityCollection.CollectionTask _CollectionTask;
        private int? _TaskID = null;
        private int? _CollectionTaskID = null;
        private Task.TaskModuleType _taskModuleType = Task.TaskModuleType.None;
        private Task.TaskResultType _taskResultType = Task.TaskResultType.None;
        private Task.TaskDateType _taskDateType = Task.TaskDateType.None;

        private bool _ScanMode = false;
        private enum ScanStep { Idle, Scanning };
        private ScanStep _CurrentScanStep = ScanStep.Idle;
        private DiversityCollection.UserControls.iMainForm _iMainForm;

        public int ID { get { return int.Parse(this.dataSetCollectionTask.CollectionTask.Rows[this.collectionTaskBindingSource.Position][0].ToString()); } }

        #endregion

        #region Construction
        public FormCollectionTask(DiversityCollection.UserControls.iMainForm iMainForm)
        {
            InitializeComponent();
            // #256
            if (this.userControlQueryList == null)
            {
                this.userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
                this.splitContainerMain.Panel1.Controls.Add(this.userControlQueryList);
                this.userControlQueryList.Dock = DockStyle.Fill;
            }
            if (this.userControlImageCollectionImage == null)
                this.userControlImageCollectionImage = new DiversityWorkbench.UserControls.UserControlImage();
            if (this.userControlModuleRelatedEntryCollectionImageCreator == null)
                this.userControlModuleRelatedEntryValue = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            if (this.userControlModuleRelatedEntryCollectionImageLicenseHolder == null)
                this.userControlModuleRelatedEntryCollectionImageLicenseHolder = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            if (this.userControlModuleRelatedEntryCollectionImageCreator == null)
                this.userControlModuleRelatedEntryCollectionImageCreator = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            if (this.userControlModuleRelatedEntryValue == null)
                this.userControlModuleRelatedEntryValue = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();


            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerMain.Panel2.Visible = false;
            this._iMainForm = iMainForm;
            this.initForm();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();

            // Manual #35
            this.KeyPreview = true;
            KeyDown += Form_KeyDown;
            this.KeyDown += new KeyEventHandler(this.Form_KeyDown);
            this.helpProvider.SetHelpKeyword(this, "task_dc");
        }

        public FormCollectionTask(int? ID, DiversityCollection.UserControls.iMainForm iMainForm)
            : this(iMainForm)
        {
            if (ID != null)
                this._CollectionTask.setItem((int)ID);
            this.splitContainerMain.Panel1Collapsed = true;
            //this.userControlDialogPanel.Visible = true;

            // Manual #35
            this.KeyPreview = true;
            KeyDown += Form_KeyDown;
            this.KeyDown += new KeyEventHandler(this.Form_KeyDown);
            this.helpProvider.SetHelpKeyword(this, "task_dc");
        }


        #endregion

        #region Form

        private void initForm()
        {
            System.Data.DataSet Dataset = this.dataSetCollectionTask;
            if (this._CollectionTask == null)
                this._CollectionTask = new CollectionTask(ref Dataset, this.dataSetCollectionTask.CollectionTask,
                    ref this.treeViewTask, this, this.userControlQueryList, this.splitContainerMain,
                    this.splitContainerData,
                    this.helpProvider, this.toolTip, ref this.collectionTaskBindingSource);
            this._CollectionTask.setFormCollectionTask(this);
            this._CollectionTask.initForm();
            this._CollectionTask.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
            this._CollectionTask.setToolStripButtonNewEvent(this.toolStripButtonNew);
            this.setToolStripButtonNewEvent(toolStripButtonNew);
            this._CollectionTask.setToolStripButtonIncludeIDEvent(this.toolStripButtonIncludeID);
            this._CollectionTask.setToolStripButtonSetParentWithHierarchyEvent(this.toolStripButtonSetParent);
            this._CollectionTask.setToolStripButtonRemoveParentEvent(this.toolStripButtonRemoveParent);
            this._CollectionTask.UserControlImageCollectionImage = this.userControlImageCollectionImage;
            this._CollectionTask.ListBoxImages = this.listBoxCollectionImage;
            this._CollectionTask.ImageListCollectionImages = this.imageListCollectionImages;
            this._CollectionTask.ImageToolStripItem = this.imagesToolStripMenuItem;
            this._CollectionTask.SplitContainerDataAndImages = this.splitContainerImagesAndData;
            this._CollectionTask.SplitContainerImagesAndLabel = this.splitContainerImageAndLabel;
            this._CollectionTask.LabelHeader = this.labelHeader;
            this.setPermissions();
            if (this.userControlQueryList != null)
            {
                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.userControlQueryList.RememberQuerySettingsIdentifier = "CollectionTask";
                this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();
                if (this.userControlQueryList.RememberQuerySettings() && this.userControlQueryList.ListOfIDs.Count > 0)
                {
                    this.userControlQueryList.listBoxQueryResult.SelectedIndex = -1;
                    this.userControlQueryList.listBoxQueryResult.SelectedIndex = this.userControlQueryList.RememberedIndex();
                }
            }

            this.InitFlorplanControls();

            this.splitContainerImagesAndData.Panel1Collapsed = true;
        }

        private void FormCollectionTask_Load(object sender, EventArgs e)
        {
        }

        private void FormCollectionTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._CollectionTask.saveItem();

            if (this.userControlQueryList.RememberQuerySettings())
                this.userControlQueryList.RememberQueryConditionSettings_SaveToFile();
            else
                this.userControlQueryList.RememberQueryConditionSettings_RemoveFile();
        }

        private void setPermissions()
        {
            DiversityWorkbench.Forms.FormFunctions FF = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            this._CollectionTask.setToolbarPermission(ref this.toolStripButtonDelete, "CollectionTask", "Delete");
            this._CollectionTask.setToolbarPermission(ref this.toolStripButtonNew, "CollectionTask", "Insert");
        }

        public void SetFormAccordingToItem()
        {
            /*
             * if linked to module and task results present then refer to task results
             */
            if (this._CollectionTask.ID != null)
            {
                try
                {
                    if (collectionTaskBindingSource.Current != null)
                    {
                        int TaskID;
                        if (int.TryParse(this._CollectionTask.TaskID.ToString(), out TaskID))
                        {
                            this._TaskID = this._CollectionTask.TaskID;
                            this.setDetailControls(TaskID);


                            if (this.treeViewTask.SelectedNode != null)
                            {
                                this.pictureBoxType.Image = this.treeViewTask.ImageList.Images[this.treeViewTask.SelectedNode.ImageIndex]; // DiversityCollection.Resource.Task;
                                this.labelTaskHaeder.Text = this.treeViewTask.SelectedNode.Text;
                                this.toolStripHierarchyAdding.Items.Clear();
                                this._CollectionTask.PresetTaskID = null;
                                System.Data.DataTable dtTasks = this._CollectionTask.DtTasks(this._CollectionTask.ID);
                                foreach(System.Data.DataRow r in dtTasks.Rows)
                                {
                                    int iTask;
                                    int.TryParse(r[0].ToString(), out iTask);
                                    string taskType = DiversityCollection.LookupTable.TaskType(iTask);
                                    System.Drawing.Image i = DiversityCollection.Specimen.TaskTypeImage(false, taskType);
                                    System.Windows.Forms.ToolStripButton toolStripButton = new ToolStripButton(i);
                                    this.setToolStripButtonNewWithPresetEvent(toolStripButton);
                                    toolStripButton.Tag = iTask.ToString();
                                    toolStripButton.ToolTipText = r[1].ToString();
                                    this._CollectionTask.setToolStripButtonNewEvent(toolStripButton);
                                    this.toolStripHierarchyAdding.Items.Add(toolStripButton);
                                }
                            }
                                                                                                                                           //Task.TaskImage(_taskModuleType);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        public void setToolStripButtonNewEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        public void setToolStripButtonNewWithPresetEvent(System.Windows.Forms.ToolStripButton B)
        {
            try
            {
                B.Click += new System.EventHandler(this.toolStripButtonNewWithPreset_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            this._CollectionTask.PresetTaskID = null;
        }

        public void toolStripButtonNewWithPreset_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripButton toolStripButton = (System.Windows.Forms.ToolStripButton)sender;
            this._CollectionTask.PresetTaskID = int.Parse(toolStripButton.Tag.ToString());
        }


        private void bindModuleControl()
        {
            this.userControlModuleRelatedEntryValue.bindToData("CollectionTask", "DisplayText", "ModuleUri", this.collectionTaskBindingSource);
            switch (_taskModuleType)
            {
                case Task.TaskModuleType.Agent:
                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                    break;
                case Task.TaskModuleType.Collection:
                    DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)C;
                    break;
                case Task.TaskModuleType.Gazetteer:
                    DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;
                    break;
                case Task.TaskModuleType.Project:
                    DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
                    break;
                case Task.TaskModuleType.Reference:
                    DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)R;
                    break;
                case Task.TaskModuleType.SamplingPlot:
                    DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)SP;
                    break;
                case Task.TaskModuleType.ScientificTerm:
                    DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)ST;
                    break;
                case Task.TaskModuleType.TaxonName:
                    DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                    this.userControlModuleRelatedEntryValue.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
                    break;
                case Task.TaskModuleType.None:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Data handling and controls

        #region Date

        private void setDateControls(int TaskID)
        {
            try
            {
                string CustomFormat = "yyyy-MM-dd  HH:mm";
                _taskDateType = Task.TaskDateType.None;
                string DateType = DiversityCollection.LookupTable.TaskDateType(TaskID);
                _taskDateType = DiversityCollection.Task.TypeOfTaskDate(DateType);

                switch (_taskDateType)
                {
                    case Task.TaskDateType.None:
                        this.labelTaskStart.Visible = false;
                        this.labelTaskEnd.Visible = false;
                        this.dateTimePickerTaskStart.Visible = false;
                        this.dateTimePickerTaskEnd.Visible = false;
                        CustomFormat = "";
                        break;
                    case Task.TaskDateType.Date:
                        this.labelTaskStart.Visible = true;
                        this.labelTaskStart.Text = "Date:";
                        this.labelTaskEnd.Visible = false;
                        this.dateTimePickerTaskStart.Visible = true;
                        this.dateTimePickerTaskEnd.Visible = false;
                        this.buttonTaskDateEndDelete.Visible = false;
                        CustomFormat = "yyyy-MM-dd";
                        break;
                    case Task.TaskDateType.Time:
                        this.labelTaskStart.Visible = true;
                        this.labelTaskStart.Text = "Time:";
                        this.labelTaskEnd.Visible = false;
                        this.dateTimePickerTaskStart.Visible = true;
                        this.dateTimePickerTaskEnd.Visible = false;
                        this.buttonTaskDateEndDelete.Visible = false;
                        CustomFormat = "HH:mm";
                        break;
                    case Task.TaskDateType.DateAndTime:
                        this.labelTaskStart.Visible = true;
                        this.labelTaskStart.Text = "Date:";
                        this.labelTaskEnd.Visible = false;
                        this.dateTimePickerTaskStart.Visible = true;
                        this.dateTimePickerTaskEnd.Visible = false;
                        this.buttonTaskDateEndDelete.Visible = false;
                        break;
                    case Task.TaskDateType.DateFromTo:
                        this.labelTaskStart.Visible = true;
                        this.labelTaskStart.Text = "Date:";
                        this.labelTaskEnd.Visible = true;
                        this.labelTaskEnd.Text = "until:";
                        this.dateTimePickerTaskStart.Visible = true;
                        this.dateTimePickerTaskEnd.Visible = true;
                        this.buttonTaskDateEndDelete.Visible = true;
                        CustomFormat = "yyyy-MM-dd";
                        break;
                    case Task.TaskDateType.TimeFromTo:
                        this.labelTaskStart.Visible = true;
                        this.labelTaskStart.Text = "Time:";
                        this.labelTaskEnd.Visible = true;
                        this.labelTaskEnd.Text = "until:";
                        this.dateTimePickerTaskStart.Visible = true;
                        this.dateTimePickerTaskEnd.Visible = true;
                        this.buttonTaskDateEndDelete.Visible = true;
                        CustomFormat = "HH:mm";
                        break;
                    case Task.TaskDateType.DateAndTimeFromTo:
                        this.labelTaskStart.Visible = true;
                        this.labelTaskStart.Text = "Date:";
                        this.labelTaskEnd.Visible = true;
                        this.labelTaskEnd.Text = "until:";
                        this.dateTimePickerTaskStart.Visible = true;
                        this.dateTimePickerTaskEnd.Visible = true;
                        this.buttonTaskDateEndDelete.Visible = true;
                        break;
                }
                this.dateTimePickerTaskEnd.CustomFormat = CustomFormat;
                this.dateTimePickerTaskStart.CustomFormat = CustomFormat;

                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                if (R["TaskStart"].Equals(System.DBNull.Value))
                {
                    this.labelTaskStart.ForeColor = System.Drawing.Color.Gray;
                    this.dateTimePickerTaskStart.CustomFormat = "-";
                }
                else
                {
                    this.labelTaskStart.ForeColor = System.Drawing.Color.Black;
                }
                if (R["TaskEnd"].Equals(System.DBNull.Value))
                {
                    this.labelTaskEnd.ForeColor = System.Drawing.Color.Gray;
                    this.dateTimePickerTaskEnd.CustomFormat = "-";
                }
                else
                {
                    this.labelTaskEnd.ForeColor = System.Drawing.Color.Black;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void dateTimePickerTaskStart_CloseUp(object sender, EventArgs e)
        {
            this._CollectionTask.saveItem();
            this.setDateControls((int)this._CollectionTask.TaskID);
        }

        private void dateTimePickerTaskEnd_CloseUp(object sender, EventArgs e)
        {
            this._CollectionTask.saveItem();
            this.setDateControls((int)this._CollectionTask.TaskID);
        }

        private void buttonTaskDateDelete_Click(object sender, EventArgs e)
        {
            this.deleteDateValue();
        }

        private void buttonTaskDateEndDelete_Click(object sender, EventArgs e)
        {
            this.deleteDateValue(false);
        }

        private void deleteDateValue(bool ForStart = true)
        {
            try
            {

                if (this.collectionTaskBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                    R.BeginEdit();
                    if (ForStart)
                        R["TaskStart"] = System.DBNull.Value;
                    else
                        R["TaskEnd"] = System.DBNull.Value;
                    R.EndEdit();
                    this._CollectionTask.saveItem();
                    this.setDateControls((int)this._CollectionTask.TaskID);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Numeric values

        private void textBoxTaskNumber_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxTaskNumber_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                if (this.collectionTaskBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                    if (this.textBoxTaskNumber.Text.Length == 0) // && !R["NumberValue"].Equals(System.DBNull.Value))
                    {
                        R.BeginEdit();
                        R["NumberValue"] = System.DBNull.Value;
                        R.EndEdit();
                        this._CollectionTask.saveItem();
                    }
                    else if (this.textBoxTaskNumber.Text.Length > 0)
                    {
                        if (this.textBoxTaskNumber.Text.IndexOf(",") > -1)
                            this.textBoxTaskNumber.Text = this.textBoxTaskNumber.Text.Replace(",", ".");
                        Double dd;
                        if (!Double.TryParse(this.textBoxTaskNumber.Text, out dd))
                        {
                            System.Windows.Forms.MessageBox.Show(this.textBoxTaskNumber.Text + " is not a valid numeric value.");
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

        #region Results

        private void comboBoxResult_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxResult.DataSource == null && this.comboBoxResult.DropDownStyle == ComboBoxStyle.DropDown && this._TaskID != null)
            {
                string SQL = "SELECT DISTINCT Result FROM CollectionTask WHERE TaskID = " + this._TaskID.ToString();
                System.Data.DataTable dataTable = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
                this.comboBoxResult.DataSource = dataTable;
                this.comboBoxResult.DisplayMember = "Result";
                this.comboBoxResult.ValueMember = "Result";
            }
        }

        private void comboBoxResult_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.collectionTaskBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                R.BeginEdit();
                R["Result"] = this.comboBoxResult.SelectedValue;
                R.EndEdit();
            }
        }

        private void comboBoxResult_TextUpdate(object sender, EventArgs e)
        {
            if (this._taskResultType == Task.TaskResultType.Number)
            {
                string Text = this.comboBoxResult.Text;
                float f;
                if (!float.TryParse(Text, out f))
                {
                    System.Windows.Forms.MessageBox.Show(Text + " is not a valid number");
                    this.comboBoxResult.Text = "";
                }
            }
        }

        #endregion

        #region Collection

        private void setCollectionControls(int? CollectionID = null)
        {
            int iCollectionID = -1;
            if (CollectionID ==  null)
            {
                string SQL = "SELECT CollectionID FROM CollectionTask WHERE CollectionTaskID = " + this._CollectionTask.ID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                int.TryParse(Result, out iCollectionID);
            }
            else
                iCollectionID = (int)CollectionID;
            if (iCollectionID > -1)
            {
                string SQL = "SELECT Type FROM Collection WHERE CollectionID = " + iCollectionID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result.Length > 0)
                {
                    System.Drawing.Image I = DiversityCollection.Specimen.CollectionTypeImage(false, Result);
                    this.labelCollection.Image = I;
                }
                SQL = "SELECT DisplayText FROM dbo.CollectionHierarchyAll() WHERE CollectionID = " + iCollectionID.ToString();
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.labelCollection.Text = "      " + Result;
            }
        }

        private void buttonCollection_Click(object sender, EventArgs e)
        {
            if (this.collectionTaskBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                if (R["CollectionTaskParentID"].Equals(System.DBNull.Value))
                {
                    System.Windows.Forms.MessageBox.Show("The collection of the basic task can not be changed");
                    return;
                }
                int CollectionID;
                if (int.TryParse(R["CollectionID"].ToString(), out CollectionID))
                {
                    string SQL = "SELECT H.DisplayText, H.CollectionID FROM [dbo].[CollectionChildNodes] (" + CollectionID.ToString() + ") C INNER JOIN [dbo].[CollectionHierarchyAll] () H ON C.CollectionID = H.CollectionID ORDER BY H.DisplayText";
                    System.Data.DataTable dtColl = new DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtColl);
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtColl, "DisplayText", "CollectionID", "Collection", "Please select a collection", "", false, true, false, DiversityCollection.Resource.Collection);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        if (int.TryParse(f.SelectedValue, out CollectionID))
                        {
                            R.BeginEdit();
                            R["CollectionID"] = CollectionID;
                            R.EndEdit();
                            this.setCollectionControls(CollectionID);
                        }
                    }
                }
            }
        }

        #endregion

        private void numericUpDownDisplayOrder_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownDisplayOrder.Value == 0)
            {
                numericUpDownDisplayOrder.BackColor = System.Drawing.Color.Red;
                this.toolTip.SetToolTip(numericUpDownDisplayOrder, "Will not be included in report");
            }
            else
            {
                numericUpDownDisplayOrder.BackColor = System.Drawing.SystemColors.Window;
                this.toolTip.SetToolTip(numericUpDownDisplayOrder, "Position in report");
            }
        }

        #region URL

        private void buttonUrlOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int i = this.collectionTaskBindingSource.Position;
                this.dataSetCollectionTask.CollectionTask.Rows[i]["URI"] = f.URL;
                this.textBoxURI.Text = f.URL;
            }
        }

        private void SetUriVisibility(bool IsVisible)
        {
            this.labelURI.Visible = IsVisible;
            this.textBoxURI.Visible = IsVisible;
            this.buttonUrlOpen.Visible = IsVisible;
            this.splitContainerURI.Panel2Collapsed = !IsVisible || this.textBoxURI.Text.Length == 0;
        }

        #endregion

        #endregion

        #region Menue
        private void showURIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainerURI.Panel2Collapsed = this.showURIToolStripMenuItem.Checked;
        }

        #region Scanner
        private void scanModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._ScanMode = !this._ScanMode;
            this.splitContainerMain.Panel1Collapsed = this._ScanMode;
            this.pictureBoxScanner.Visible = this._ScanMode;
            this.textBoxScanner.Visible = this._ScanMode;
        }

        private void textBoxScanner_MouseEnter(object sender, EventArgs e)
        {
            if (this.scanModeToolStripMenuItem.Checked)
            {
                this.textBoxScanner.Focus();
                this.textBoxScanner.SelectAll();
            }
        }

        private void textBoxScanner_TextChanged(object sender, EventArgs e)
        {
            if (this.scanModeToolStripMenuItem.Checked)
            {
                this.startScanner();
            }
        }

        private void timerScanning_Tick(object sender, EventArgs e)
        {
            {
                this.timerScanning.Stop();
                string ScannedIdentifier = this.textBoxScanner.Text;
                string SQL = "SELECT dbo.StableIdentifierBase() + 'CollectionTask/'";
                string StableIdentierBase = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                if (StableIdentierBase.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("The stable identifier for the database must be defined before this function is available. Please turn to the manual for details", "Missing stable identifier", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
                string sCollectionTaskID = "";
                int CollectionTaskID;
                bool ScannedCollectionTaskIdentified = false;
                if (StableIdentierBase.Length > 0 && ScannedIdentifier.StartsWith(StableIdentierBase))
                {
                    sCollectionTaskID = this.textBoxScanner.Text.Substring(this.textBoxScanner.Text.LastIndexOf('/') + 1);
                    if (int.TryParse(sCollectionTaskID, out CollectionTaskID))
                    {
                        ScannedCollectionTaskIdentified = true;
                        this._CollectionTask.setItem(CollectionTaskID);
                        this.timerScanning.Stop();
                    }
                }
                //else
                //{
                //    SQL = "SELECT MIN(CollectionTaskID) AS ID " +
                //        "FROM CollectionTask AS C " +
                //        "GROUP BY DisplayText " +
                //        "HAVING (DisplayText = '" + ScannedIdentifier.Replace("'", "''") + "') AND (COUNT(*) = 1) ";
                //    sCollectionTaskID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //    if (int.TryParse(sCollectionTaskID, out CollectionTaskID))
                //        ScannedCollectionTaskIdentified = true;
                //}
                if (!ScannedCollectionTaskIdentified)
                {
                    if (this.textBoxScanner.Text.Length > 0)
                    {
                        System.Windows.Forms.MessageBox.Show("The identifier " + this.textBoxScanner.Text + " is not corresponding to a valid stable identifier for a collection task");
                    }
                }
                //{
                //    if (int.TryParse(this.textBoxScanner.Text, out CollectionTaskID))
                //    {
                //        //System.Windows.Forms.MessageBox.Show("No dataset could be found with the ID\r\n" + this.textBoxScanner.Text);
                //    }
                //}
                //else
            }
            this._CurrentScanStep = ScanStep.Idle;
            this.textBoxScanner.SelectAll();
            this.timerScanning.Stop();
        }

        private void startScanner()
        {
            if (this._CurrentScanStep == ScanStep.Idle && this.scanModeToolStripMenuItem.Checked)
            {
                this._CurrentScanStep = ScanStep.Scanning;
                this.timerScanning.Start();
            }
        }


        #endregion

        #region showing header regions
        private void imagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (header == Header.Image)
                    header = Header.None;
                else header = Header.Image;
                //this.ShowImage = !this._ShowImage;
                this.setVisibilityOfImagesAndLabel();
            }
            catch { }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (header == Header.Report)
                    header = Header.None;
                else header = Header.Report;
                //this.ShowLabel = !this._ShowLabel;
                this.setVisibilityOfImagesAndLabel();
            }
            catch { }
        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (header == Header.Chart)
                    header = Header.None;
                else header = Header.Chart;
                //this.ShowChart = !this._ShowChart;
                this.setVisibilityOfImagesAndLabel();
            }
            catch { }
        }

        private void floorplanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (header == Header.Plan)
                    header = Header.None;
                else header = Header.Plan;
                //this.ShowChart = !this._ShowChart;
                this.setVisibilityOfImagesAndLabel();
            }
            catch { }
        }

        #endregion

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
            //DiversityWorkbench.Feedback.SendFeedback(
            //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
            //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
            //    this.userControlQueryList.QueryString(),
            //    this.dataSetCollectionTask.CollectionTask.Rows[this.collectionTaskBindingSource.Position][0].ToString());
        }

        private void toolStripMenuItemHistory_Click(object sender, EventArgs e)
        {
            string Title = "History of " + this.dataSetCollectionTask.CollectionTask.Rows[0]["DisplayText"].ToString() + " (CollectionTaskID: " + this.dataSetCollectionTask.CollectionTask.Rows[0]["CollectionTaskID"].ToString() + ")";
            try
            {
                bool HistoryPresent = false;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();

                if (this.dataSetCollectionTask.CollectionTask.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(int.Parse(this.dataSetCollectionTask.CollectionTask.Rows[this.collectionTaskBindingSource.Position][0].ToString()), "CollectionTaskID", this.dataSetCollectionTask.CollectionTask.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetCollectionTask.CollectionTaskImage.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(int.Parse(this.dataSetCollectionTask.CollectionTask.Rows[this.collectionTaskBindingSource.Position][0].ToString()), "CollectionTaskID", this.dataSetCollectionTask.CollectionTaskImage.TableName, ""));
                    HistoryPresent = true;
                }
                if (HistoryPresent)
                {
                    DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                    f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                    f.ShowDialog();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No history data found");
                }

            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #region import

        private void importWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartWizard();//, this.ImportStepsEvent(true));
        }

        private void prometheusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Windows.MessageBox.Show("available in upcoming version");
            DiversityCollection.Tasks.FormImportPrometheus f = new Tasks.FormImportPrometheus();
            //DiversityWorkbench.Prometheus.FormImport f = new DiversityWorkbench.Prometheus.FormImport();
            f.ShowDialog();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void StartWizard()
        {
            try
            {
                this.Enabled = false;

                // Show start window
                DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard();
                fs.StartPosition = FormStartPosition.CenterParent;
                fs.Show();
                Application.DoEvents();

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                fs.ShowCurrentStep("Resetting the templates for the tables\r\n..........");
                DiversityWorkbench.Import.Import.ResetTemplate();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportSteps = null;
                fs.ShowCurrentStep("Getting the steps for CollectionTask\r\n|.........");
                DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn = null;
                ImportSteps = this.ImportStepsCollectionTask;
                if (ImportSteps == null)
                    ImportSteps = new Dictionary<string, DiversityWorkbench.Import.Step>();
                fs.ShowCurrentStep("Import steps for CollectionTask created. Opening import form\r\n||||||||..");
                DiversityWorkbench.Import.FormWizard f = new DiversityWorkbench.Import.FormWizard("CollectionTask", DiversityCollection.Properties.Settings.Default.DatabaseVersion, ImportSteps, this.helpProvider.HelpNamespace, fs);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Import.Import.SchemaFileSource, string> SFS = new Dictionary<DiversityWorkbench.Import.Import.SchemaFileSource, string>();
                SFS.Add(DiversityWorkbench.Import.Import.SchemaFileSource.SNSB, "https://github.com/SNSB/DWB-Contrib/tree/master/DiversityCollection/Import/Schemas");
                SFS.Add(DiversityWorkbench.Import.Import.SchemaFileSource.ZFMK, "https://github.com/ZFMK/Labels-and-Imports-for-DiversityWorkbench/tree/master/ImportSchedules");
                DiversityWorkbench.Import.iWizardInterface i = f;
                i.SetSourcesForSchemaFiles(SFS);

                f.Height = this.Height - 10;
                f.StartPosition = FormStartPosition.CenterParent;

                fs.Close();

                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Enabled = true;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportStepsCollectionTask
        {
            get
            {
                /*SELECT     CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, 
                CollectionOwner, DisplayOrder, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
                FROM         Collection*/

                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> IS = new Dictionary<string, DiversityWorkbench.Import.Step>();

                //TODO: Attachment ueber CollectionID klappt nicht
                DiversityWorkbench.Import.DataTable DT = this.ImportWizardDataTable("CollectionTask", "", "CollectionTask", DiversityWorkbench.Import.DataTable.Parallelity.unique, 1, "", "CollectionTaskID");
                DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStepTemplate(DT, DiversityCollection.Specimen.ImageForTable(DT.TableName, false), 0, true, null);
                S.MustSelect = true;
                IS.Add(DT.PositionKey, S);

                return IS;
            }
        }

        private DiversityWorkbench.Import.DataTable ImportWizardDataTable(string TableName, string ParentTableAlias, string DisplayText, DiversityWorkbench.Import.DataTable.Parallelity P, int SequencePosition, string IgnoredColumns, string AttachmentColumns)
        { 
            System.Collections.Generic.List<string> lAC = new List<string>();
            AttachmentColumns = AttachmentColumns.Replace(" ", "");
            string[] ACC = AttachmentColumns.Split(new char[] { ',' });
            for (int i = 0; i < ACC.Length; i++)
                lAC.Add(ACC[i]);
            System.Collections.Generic.List<string> IC = new List<string>();
            IgnoredColumns = IgnoredColumns.Replace(" ", "");
            string[] CC = IgnoredColumns.Split(new char[] { ',' });
            for (int i = 0; i < CC.Length; i++)
                IC.Add(CC[i]);
            DiversityWorkbench.Import.DataTable DT = DiversityWorkbench.Import.DataTable.GetTableTemplate(TableName, ParentTableAlias, DisplayText, DiversityCollection.Specimen.ImageForTable(TableName, false), P, SequencePosition, IC, lAC);
            return DT;
        }

        #endregion

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;

                // Show start window
                DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard("Starting export wizard...", DiversityWorkbench.Import.FormStartWizard.Direction.Export);
                fs.Show();
                Application.DoEvents();

                if (this.userControlQueryList.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    fs.Close();
                    return;
                }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityWorkbench.Export.Exporter.ResetAll();

                // getting the IDs
                System.Collections.Generic.List<int> L = new List<int>();
                foreach (int ID in this.userControlQueryList.ListOfIDs)
                {
                    L.Add(ID);
                }
                if (L.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No data found");
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }

                // getting the template tables
                //DiversityWorkbench.Export.Table TSeries = DiversityWorkbench.Export.Exporter.AddTemplateTable("CollectionEventSeries", "Event series", DiversityWorkbench.Export.Table.Parallelity.unique, DiversityCollection.Specimen.getImage(Specimen.OverviewImage.EventSeries), null, null, null, "");

                //System.Collections.Generic.Dictionary<string, string> TM = new Dictionary<string, string>();
                //TM.Add("ReferenceURI", "DiversityReferences");
                //DiversityWorkbench.Export.Table TEvent = DiversityWorkbench.Export.Exporter.AddTemplateTable("CollectionEvent", "Event", DiversityWorkbench.Export.Table.Parallelity.unique, DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Event), TSeries, null, TM, "");

                //// Localisation
                //this.ExportWizardLocalisationTables(TEvent);
                //// Site properties
                //this.ExportWizardEventPropertyTables(TEvent);

                //DiversityWorkbench.Export.Table TEventMethod = DiversityWorkbench.Export.Exporter.AddTemplateTable("CollectionEventMethod", "Event method", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Tool), TEvent, null, null, "");
                //DiversityWorkbench.Export.Table TEventMethodParameter = DiversityWorkbench.Export.Exporter.AddTemplateTable("CollectionEventParameterValue", "Event method parameter", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Parameter), TEventMethod, null, null, "");

                //System.Collections.Generic.Dictionary<string, string> T_EventImage_M = new Dictionary<string, string>();
                //T_EventImage_M.Add("CreatorAgentURI", "DiversityAgents");
                //T_EventImage_M.Add("LicenseHolderAgentURI", "DiversityAgents");
                //DiversityWorkbench.Export.Table TEventImage = DiversityWorkbench.Export.Exporter.AddTemplateTable("CollectionEventImage", "Event image", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.ImageArea), TEvent, null, T_EventImage_M, "");

                //// IdentifierEvent
                //System.Collections.Generic.Dictionary<string, string> T_IdentifierEvent_Joins = new Dictionary<string, string>();
                //T_IdentifierEvent_Joins.Add("CollectionEventID", "CollectionEventID");
                //System.Collections.Generic.List<string> IdentifierSortingColumns = new List<string>();
                //IdentifierSortingColumns.Add("ID");
                //DiversityWorkbench.Export.Table T_IdentifierEvent = DiversityWorkbench.Export.Exporter.AddTemplateTable("IdentifierEvent", "Identifier for event", DiversityWorkbench.Export.Table.Parallelity.referencing, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.ID), TEvent, IdentifierSortingColumns, null, ""/*ReferencedTable = 'CollectionEvent'"*/, T_IdentifierEvent_Joins, null);

                //// AnnotationEvent
                //System.Collections.Generic.Dictionary<string, string> T_AnnotationEvent_Joins = new Dictionary<string, string>();
                //T_AnnotationEvent_Joins.Add("CollectionEventID", "CollectionEventID");
                //System.Collections.Generic.List<string> AnnotationSortingColumns = new List<string>();
                //AnnotationSortingColumns.Add("AnnotationID");
                //DiversityWorkbench.Export.Table T_AnnotationEvent = DiversityWorkbench.Export.Exporter.AddTemplateTable("AnnotationEvent", "Annotation for event", DiversityWorkbench.Export.Table.Parallelity.referencing, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Annotation), TEvent, AnnotationSortingColumns, null, ""/*ReferencedTable = 'CollectionEvent'"*/, T_AnnotationEvent_Joins, null);

                // CollectionTask
                DiversityWorkbench.Export.Table TCollectionTask = DiversityWorkbench.Export.Exporter.AddTemplateTable("CollectionTask", "Task", DiversityWorkbench.Export.Table.Parallelity.unique, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.CollectionTask), null, null, null, "");
                TCollectionTask.IsStartPoint = true;

                // CollectionTaskImage
                System.Collections.Generic.Dictionary<string, string> T_CollectionTaskImage_M = new Dictionary<string, string>();
                T_CollectionTaskImage_M.Add("CreatorAgentURI", "DiversityAgents");
                T_CollectionTaskImage_M.Add("LicenseHolderAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table TCollectionTaskImage = DiversityWorkbench.Export.Exporter.AddTemplateTable("CollectionTaskImage", "Image", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.ImageArea), TCollectionTask, null, T_CollectionTaskImage_M, "");

                // Table list
                System.Collections.Generic.List<DiversityWorkbench.Export.Table> TT = new List<DiversityWorkbench.Export.Table>();
                TT.Add(TCollectionTask);
                TT.Add(TCollectionTaskImage);

                DiversityWorkbench.Export.FormExport f = new DiversityWorkbench.Export.FormExport(TCollectionTask, L, TT, this.helpProvider.HelpNamespace);
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                f.StartPosition = FormStartPosition.CenterParent;

                fs.Close();
                f.ShowDialog();

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.Enabled = true;
            }
        }

        private void tableEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing();
        }

        private void spreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartSpreadSheet();
        }


        #endregion

        #region Images & Report & Chart

        private enum Header {None, Image, Report, Chart, Plan }
        private Header header = Header.None;

        #region Visibility

        //private bool _ShowImage = false;
        //private bool _ShowLabel = false;
        //private bool _ShowChart = false;
        //private bool _ShowPlan = false;

        //private bool ShowPlan
        //{
        //    get { return this._ShowPlan; }
        //    set
        //    {
        //        this._ShowPlan = value;
        //        if (value) this.floorplanToolStripMenuItem.BackColor = System.Drawing.Color.Red;
        //        else this.floorplanToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //        this._ShowImage = false;
        //        this._ShowLabel = false;
        //        this._ShowChart = false;
        //        this.printToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //}


        //private bool ShowChart
        //{
        //    get { return this._ShowChart; }
        //    set
        //    {
        //        this._ShowChart = value;
        //        if (value) this.chartToolStripMenuItem.BackColor = System.Drawing.Color.Red;
        //        else this.chartToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //        this._ShowImage = false;
        //        this._ShowLabel = false;
        //        this.printToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //}


        //private bool ShowLabel
        //{
        //    get { return this._ShowLabel; }
        //    set
        //    {
        //        this._ShowLabel = value;
        //        if (value) this.printToolStripMenuItem.BackColor = System.Drawing.Color.Red;
        //        else this.printToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //        this._ShowImage = false;
        //        this._ShowChart = false;
        //        this.chartToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //}

        //private bool ShowImage
        //{
        //    get { return this._ShowImage; }
        //    set
        //    {
        //        this._ShowImage = value;
        //        //if (value) this.imagesToolStripMenuItem.BackColor = System.Drawing.Color.Red;
        //        //else this.imagesToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //        this._ShowLabel = false;
        //        this._ShowChart = false;
        //        this.printToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //        this.chartToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //}


        private void setVisibilityOfImagesAndLabel(/*bool ShowImage, bool ShowLabel*/)
        {
            // reset backcolors beside image (done by CollectionTask)
            this.printToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.chartToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.floorplanToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            // image | report
            //       | report | chart
            //                 chart | plan
            switch (header)
            {
                case Header.None:
                    this.splitContainerImagesAndData.Panel1Collapsed = true;
                    break;
                case Header.Image:
                    // show image
                    this.splitContainerImageAndLabel.Panel1Collapsed = false;
                    // hide rest
                    this.splitContainerImageAndLabel.Panel2Collapsed = true;
                    goto default;
                case Header.Report:
                    // hide image
                    this.splitContainerImageAndLabel.Panel1Collapsed = true;
                    // show others
                    this.splitContainerImageAndLabel.Panel2Collapsed = false;
                    // show report
                    this.splitContainerLabelAndChart.Panel1Collapsed = false;
                    // hide others
                    this.splitContainerLabelAndChart.Panel2Collapsed = true;
                    // set backcolor
                    this.printToolStripMenuItem.BackColor = System.Drawing.Color.Red;
                    goto default;
                case Header.Chart:
                    // hide image
                    this.splitContainerImageAndLabel.Panel1Collapsed = true;
                    // show others
                    this.splitContainerImageAndLabel.Panel2Collapsed = false;
                    // hide report
                    this.splitContainerLabelAndChart.Panel1Collapsed = true;
                    // show others
                    this.splitContainerLabelAndChart.Panel2Collapsed = false;
                    // show chart
                    this.splitContainerChartAndPlan.Panel1Collapsed = false;
                    // hide plan
                    this.splitContainerChartAndPlan.Panel2Collapsed = true;
                    // set backcolor
                    this.chartToolStripMenuItem.BackColor = System.Drawing.Color.Red;
                    goto default;
                case Header.Plan:
                    // hide image
                    this.splitContainerImageAndLabel.Panel1Collapsed = true;
                    // show others
                    this.splitContainerImageAndLabel.Panel2Collapsed = false;
                    // hide report
                    this.splitContainerLabelAndChart.Panel1Collapsed = true;
                    // show others
                    this.splitContainerLabelAndChart.Panel2Collapsed = false;
                    // hide chart
                    this.splitContainerChartAndPlan.Panel1Collapsed = true;
                    // show plan
                    this.splitContainerChartAndPlan.Panel2Collapsed = false;
                    // set backcolor
                    this.floorplanToolStripMenuItem.BackColor = System.Drawing.Color.Red;
                    goto default;
                default:
                    this.splitContainerImagesAndData.Panel1Collapsed = false;
                    break;
            }


            //this.splitContainerImagesAndData.Panel1Collapsed = !(ShowImage || this.ShowLabel || this.ShowChart || this.ShowPlan);
            //// showing anything
            //if (ShowImage || this.ShowLabel || this.ShowChart || this.ShowPlan)
            //{
            //    // collapsing splitcontainers according to selected object
            //    // image | label
            //    //       | label | chart
            //    //                 chart | plan

            //    // showing the image
            //    this.splitContainerImageAndLabel.Panel1Collapsed = !ShowImage;
            //    this.splitContainerImageAndLabel.Panel2Collapsed = ShowImage;
            //    // showing anything else
            //    if (!ShowImage)
            //    {
            //        // showing the label
            //        this.splitContainerLabelAndChart.Panel1Collapsed = !ShowLabel;
            //        this.splitContainerLabelAndChart.Panel2Collapsed = ShowLabel;
            //        // showing anything else
            //        if (!ShowLabel)
            //        {
            //            if (ShowChart)
            //            {

            //            }
            //            else
            //            {

            //            }
            //        }
            //    }

            //}



            this._CollectionTask.setFormControls();
        }

        #endregion

        #region Images
        private void listBoxCollectionImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListCollectionImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void listBoxCollectionImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListCollectionImages.ImageSize.Height;
            e.ItemWidth = this.imageListCollectionImages.ImageSize.Width;
        }

        private void listBoxCollectionImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxCollectionImage.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this.dataSetCollectionTask.CollectionTaskImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this.dataSetCollectionTask.CollectionTaskImage.Rows.Count > i)
                {
                    this.tableLayoutPanelImage.Enabled = true;
                    this.toolStripButtonImageDelete.Enabled = true;
                    this.toolStripButtonImageDescription.Enabled = true;
                    System.Data.DataRow r = this.dataSetCollectionTask.CollectionTaskImage.Rows[i];
                    this.userControlImageCollectionImage.ImagePath = r["URI"].ToString();
                    this.collectionTaskImageBindingSource.Position = i;
                }
                else
                {
                    this.tableLayoutPanelImage.Enabled = false;
                    this.toolStripButtonImageDelete.Enabled = false;
                    this.toolStripButtonImageDescription.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollectionTask.CollectionTaskImageRow R = this.dataSetCollectionTask.CollectionTaskImage.NewCollectionTaskImageRow();
                        R.CollectionTaskID = this.ID;
                        R.URI = f.URIImage;
                        R.Description = f.Exif;
                        this.dataSetCollectionTask.CollectionTaskImage.Rows.Add(R);
                        this._CollectionTask.setFormControls();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string URL = this.userControlImageCollectionImage.ImagePath;
                if (URL.Length > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetCollectionTask.CollectionTaskImage.Select("URI = '" + URL + "'");
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            r.Delete();
                            this._CollectionTask.setFormControls();
                            if (this.listBoxCollectionImage.Items.Count > 0) this.listBoxCollectionImage.SelectedIndex = 0;
                            else
                            {
                                this.listBoxCollectionImage.SelectedIndex = -1;
                                this.userControlImageCollectionImage.ImagePath = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Chart

        private void buttonCreateChart_Click(object sender, EventArgs e)
        {
#if xDEBUG
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;
#endif

            try
            {
                // Adding the titles
                this.chartTask.Titles.Clear();
                foreach(string T in this.ChartTitles((int)this._CollectionTask.ID))
                this.chartTask.Titles.Add(T);

                System.Data.DataTable dataTable = new DataTable();
                System.Collections.Generic.Dictionary<string, string> Spalten = new Dictionary<string, string>();
                this.GetChartData(ref dataTable, ref Spalten);

                this.chartTask.DataSource = dataTable;
                this.chartTask.Series.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                {
                    this.chartTask.Series.Add(KV.Key);
                    this.chartTask.Series[KV.Key].YValueMembers = KV.Value;
                    this.chartTask.Series[KV.Key].XValueMember = "Zeitpunkt";
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Collections.Generic.List<string> ChartTitles(int ID)
        {
            System.Collections.Generic.List<string> Titles = new List<string>();
            string SQL = "SELECT C.DisplayText " +
                "FROM CollectionTask C " +
                "WHERE C.CollectionTaskID = " + this._CollectionTask.ID.ToString();
            string Title = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            Titles.Add(Title);
            return Titles;
        }

        private void GetChartData(ref System.Data.DataTable dataTable, ref System.Collections.Generic.Dictionary<string, string> Spalten)
        {
            try
            {
                // getting the series
                string SQL = "SELECT C.DisplayText " +
                    "FROM CollectionTask AS C INNER JOIN " +
                    "Task AS T ON C.TaskID = T.TaskID " +
                    "WHERE C.DisplayText <> '' AND(C.DisplayOrder > 0) AND(NOT(C.TaskStart IS NULL)) AND(NOT(C.NumberValue IS NULL)) AND(T.NumberType <> N'') " +
                    "AND((C.CollectionTaskID = " + ID.ToString() +
                    ") " +
                    "OR C.CollectionTaskID IN(" +
                    "SELECT CollectionTaskID FROM[dbo].[CollectionTaskChildNodes](" + ID.ToString() +
                    "))) " +
                    "GROUP BY C.DisplayText";
                System.Data.DataTable dataTableSeries = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableSeries);
                Spalten.Clear();
                for (int i = 0; i < dataTableSeries.Rows.Count; i++)
                {
                    string series = dataTableSeries.Rows[i][0].ToString();
                    Spalten.Add(series, "Wert_" + (i + 1).ToString());
                }

                // getting start and end
                SQL = "declare @ID int; " +
                    "set @ID = " + ID.ToString() + "; " +
                    "declare @start date; " +
                    "declare @end date; " +
                    "set @start = (SELECT cast(min(CONVERT(varchar(7), C.TaskStart, 120)) + '-01' as date) " +
                    "FROM CollectionTask AS C INNER JOIN " +
                    "Task AS T ON C.TaskID = T.TaskID " +
                    "WHERE(C.DisplayText <> N'') AND(NOT(C.TaskStart IS NULL)) AND(NOT(C.NumberValue IS NULL)) AND(C.DisplayOrder > 0) AND(T.NumberType <> N'') AND(C.CollectionTaskID = @ID OR " +
                    "C.CollectionTaskID IN " +
                    "(SELECT CollectionTaskID " +
                    "FROM dbo.CollectionTaskChildNodes(@ID) AS CollectionTaskChildNodes_1))); " +
                    "set @end = (SELECT cast(max(CONVERT(varchar(7), C.TaskStart, 120)) + '-01' as date) " +
                    "FROM CollectionTask AS C INNER JOIN " +
                    "Task AS T ON C.TaskID = T.TaskID " +
                    "WHERE(C.DisplayText <> N'') AND(NOT(C.TaskStart IS NULL)) AND(NOT(C.NumberValue IS NULL)) AND(C.DisplayOrder > 0) AND(T.NumberType <> N'') AND(C.CollectionTaskID = @ID OR " +
                    "C.CollectionTaskID IN " +
                    "(SELECT CollectionTaskID " +
                    "FROM dbo.CollectionTaskChildNodes(@ID) AS CollectionTaskChildNodes_1))); ";
                // inserting start
                SQL += "declare @Werte table(Zeitpunkt date ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value + " float";
                SQL += "); insert into @Werte(Zeitpunkt";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value;
                SQL += ") Values(@start";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", 0";
                SQL += "); ";
                // filling the table with months
                SQL += "while (@end > (select max(Zeitpunkt) from @Werte)) " +
                    "begin " +
                    "insert into @Werte(Zeitpunkt";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value;
                SQL += ") Values((select DateAdd(month, 1, max(Zeitpunkt)) from @Werte) ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", 0";
                SQL += ") " +
                    "end; ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                {
                    // getting the values
                    SQL += " declare @" + KV.Value + " table(Monat date, Wert float) " +
                        "insert into @" + KV.Value + " (Monat, Wert) " +
                        "SELECT cast(min(CONVERT(varchar(7), C.TaskStart, 120)) +'-01' as date), sum(C.NumberValue) " +
                        "FROM CollectionTask AS C INNER JOIN " +
                        "Task AS T ON C.TaskID = T.TaskID " +
                        "WHERE(C.DisplayText = N'" + KV.Key + "') AND(NOT(C.TaskStart IS NULL)) AND(NOT(C.NumberValue IS NULL)) AND(C.DisplayOrder > 0) AND(T.NumberType <> N'') AND(C.CollectionTaskID = @ID OR " +
                        "C.CollectionTaskID IN " +
                        "(SELECT        CollectionTaskID " +
                        "FROM            dbo.CollectionTaskChildNodes(@ID) AS CollectionTaskChildNodes_1)) " +
                        "GROUP BY C.DisplayText, CONVERT(varchar(7), C.TaskStart, 120); ";
                    // transfer values in main table
                    SQL += "update W set " + KV.Value + " = W1.Wert " +
                        "from @Werte W inner " +
                        "join @" + KV.Value + " W1 on W.Zeitpunkt = W1.Monat ";
                }
                SQL += "select CONVERT(varchar(7), Zeitpunkt, 120) AS Zeitpunkt ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value;
                SQL += " from @Werte";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Report
        private void toolStripButtonPrintReport_Click(object sender, EventArgs e)
        {
            int ID = (int)this._CollectionTask.ID;
            //string File = this._CollectionTask.CreateXmlReport(ID, this.textBoxSchemaFile.Text, QRcode);
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(Folder.Report() + "Task.XML");
            DiversityCollection.XmlExport xmlExport = new XmlExport(this.textBoxSchemaFile.Text, XmlFile.FullName);
            string File = xmlExport.CreateXmlForCollectionTask(this.ID, "", this._qRcodeSource);
            if (File.Length > 0)
            {
                try
                {
                    System.Uri URI = new Uri(File);
                    this.webBrowserLabel.Url = URI;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserLabel.ShowPrintPreviewDialog();
        }

        private void toolStripButtonOpenSchemaFile_Click(object sender, EventArgs e)
        {
            string Path = Folder.Report(Folder.ReportFolder.TaskSchema);
            if (this.textBoxSchemaFile.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchemaFile.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialogReportSchema = new OpenFileDialog();
            this.openFileDialogReportSchema.RestoreDirectory = true;
            this.openFileDialogReportSchema.Multiselect = false;
            this.openFileDialogReportSchema.InitialDirectory = Path;
            this.openFileDialogReportSchema.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialogReportSchema.ShowDialog();
                if (this.openFileDialogReportSchema.FileName.Length > 0)
                {
                    this.textBoxSchemaFile.Tag = this.openFileDialogReportSchema.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogReportSchema.FileName);
                    this.textBoxSchemaFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region QR code
        private bool _PrintQRcode = false;
        private DiversityCollection.XmlExport.QRcodeSourceCollectionTask _qRcodeSource = XmlExport.QRcodeSourceCollectionTask.None;
        private void checkBoxQRcode_Click(object sender, EventArgs e)
        {
            this.SetQrCodeControls();
        }

        private void SetQrCodeControls()
        {
            this._PrintQRcode = !this._PrintQRcode;
            this.checkBoxLabelQRcode.Checked = this._PrintQRcode;
            this.comboBoxLabelQRcode.Enabled = this._PrintQRcode;
            this.comboBoxLabelQRcode.Visible = this._PrintQRcode;
            this.comboBoxLabelQRcodeType.Enabled = this._PrintQRcode;
            this.pictureBoxLabelQRcodeSource.Enabled = this._PrintQRcode;
            if (this._PrintQRcode)
                this.checkBoxQRcode.Image = DiversityCollection.Resource.QRcode;
            else
            {
                this.checkBoxQRcode.Image = DiversityCollection.Resource.QRcodeGray;
                this._qRcodeSource = XmlExport.QRcodeSourceCollectionTask.None;
            }

        }

        private void comboBoxLabelQRcode_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxLabelQRcode.Items.Count == 0)
            {
                this.comboBoxLabelQRcode.Items.Add("");
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSourceCollectionTask.StableIdentifier);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSourceCollectionTask.GUID);
            }
        }

        private void comboBoxLabelQRcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(this.comboBoxLabelQRcode.SelectedItem.ToString())
            {
                case "StableIdentifier":
                    this._qRcodeSource = XmlExport.QRcodeSourceCollectionTask.StableIdentifier;
                    break;
                case "GUID":
                    this._qRcodeSource = XmlExport.QRcodeSourceCollectionTask.GUID;
                    break;
                default:
                    this._qRcodeSource = XmlExport.QRcodeSourceCollectionTask.None;
                    break;
            }
        }

        #endregion


        #endregion

        #region Floor plan

        private WpfControls.Geometry.UserControlGeometry _UserControlGeometry;
        private void InitFlorplanControls()
        {
            _UserControlGeometry = new WpfControls.Geometry.UserControlGeometry();
            elementHost.Child = _UserControlGeometry;
            _UserControlGeometry.SetAddButtonsEnabled(false);
        }

        private void GetFloorPlan()
        {
            if (header == Header.Plan)
            {
                if (this._CollectionTask.ID != null)
                {
                    string SQL = "SELECT CollectionID FROM CollectionTask AS T WHERE CollectionTaskID = " + this._CollectionTask.ID.ToString();
                    string CollectionID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    int ID;
                    if (int.TryParse(CollectionID, out ID))
                    {
                        SQL = "SELECT LocationPlan FROM dbo.CollectionHierarchySuperior(" + ID.ToString() + ") WHERE CollectionID = " + ID.ToString();
                        string FloorPlan = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (FloorPlan.Length > 0)
                        {
                            this._UserControlGeometry.SetImage(FloorPlan);
                            SQL = "SELECT LocationGeometry.ToString() FROM dbo.CollectionHierarchySuperior(" + ID.ToString() + ") WHERE CollectionID = " + ID.ToString();
                            string Geometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            string ParentGeometry = "";
                            if (Geometry.Length == 0)
                            {
                                SQL = "SELECT [LocationGeometry].ToString() FROM dbo.CollectionHierarchySuperior(" + ID.ToString() + ") WHERE CollectionID = " + ID.ToString();
                            }
                            else
                                SQL = "SELECT P.[LocationGeometry].ToString() FROM [dbo].[Collection] C INNER JOIN dbo.CollectionHierarchySuperior(" + ID.ToString() + ") P ON C.CollectionParentID = P.CollectionID AND C.CollectionID = " + ID.ToString();
                            ParentGeometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                            this._UserControlGeometry.SetRectangleAndPolygonGeometry(Geometry, ParentGeometry);
                            SQL = "SELECT LocationPlanWidth FROM dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") WHERE CollectionID = " + CollectionID.ToString();
                            string Scale = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            this._UserControlGeometry.SetScaleLine(Scale);
                        }
                        else
                        {
                            this._UserControlGeometry.SetImage("");
                            this._UserControlGeometry.SetRectangleAndPolygonGeometry("");
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Showing Detail Controls

        private enum CollectionTaskDetail { Module, ModuleList, Part, Result, Number, Bool, Date, DateBegin, DateEnd, Uri, Description, Notes }

        private System.Collections.Generic.Dictionary<CollectionTaskDetail, System.Windows.Forms.TableLayoutPanel> _TaskPanels;
        private System.Collections.Generic.Dictionary<CollectionTaskDetail, bool> TaskVisible;
        private System.Collections.Generic.Dictionary<CollectionTaskDetail, int> TaskWidthDivisors;
        private System.Collections.Generic.Dictionary<CollectionTaskDetail, System.Windows.Forms.TableLayoutPanel> TaskPanels
        {
            get
            {
                if (this._TaskPanels == null)
                {
                    this._TaskPanels = new Dictionary<CollectionTaskDetail, TableLayoutPanel>();
                    this.TaskWidthDivisors = new Dictionary<CollectionTaskDetail, int>();
                    this.TaskVisible = new Dictionary<CollectionTaskDetail, bool>();

                    this._TaskPanels.Add(CollectionTaskDetail.Module, this.tableLayoutPanelTaskModule);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Module, 1);
                    this.TaskVisible.Add(CollectionTaskDetail.Module, false);

                    this._TaskPanels.Add(CollectionTaskDetail.ModuleList, this.tableLayoutPanelTaskModuleList);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.ModuleList, 2);
                    this.TaskVisible.Add(CollectionTaskDetail.ModuleList, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Part, this.tableLayoutPanelSpecimenPart);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Part, 1);
                    this.TaskVisible.Add(CollectionTaskDetail.Part, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Result, this.tableLayoutPanelTaskResult);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Result, 2);
                    this.TaskVisible.Add(CollectionTaskDetail.Result, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Number, this.tableLayoutPanelTaskNumber);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Number, 4);
                    this.TaskVisible.Add(CollectionTaskDetail.Number, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Bool, this.tableLayoutPanelTaskBool);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Bool, 4);
                    this.TaskVisible.Add(CollectionTaskDetail.Bool, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Date, this.tableLayoutPanelTaskDate);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Date, 2);
                    this.TaskVisible.Add(CollectionTaskDetail.Date, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Uri, this.tableLayoutPanelTaskUri);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Uri, 1);
                    this.TaskVisible.Add(CollectionTaskDetail.Uri, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Description, this.tableLayoutPanelTaskDescription);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Description, 1);
                    this.TaskVisible.Add(CollectionTaskDetail.Description, false);

                    this._TaskPanels.Add(CollectionTaskDetail.Notes, this.tableLayoutPanelTaskNotes);
                    this.TaskWidthDivisors.Add(CollectionTaskDetail.Notes, 1);
                    this.TaskVisible.Add(CollectionTaskDetail.Notes, false);

                }
                return this._TaskPanels;
            }
        }

        private void setDetailControls(int TaskID)
        {
            try
            {
                // ensure TaskPanels
                if (this.TaskPanels.Count == 0)
                    return;

                foreach (System.Collections.Generic.KeyValuePair<CollectionTaskDetail, System.Windows.Forms.TableLayoutPanel> KV in this.TaskPanels)
                    this.TaskVisible[KV.Key] = false;

                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                if (R["CollectionTaskID"].ToString() != this._CollectionTask.ID.ToString())
                {
                    System.Data.DataSet ds =  (System.Data.DataSet)this.collectionTaskBindingSource.DataSource;
                    System.Data.DataTable dt = ds.Tables["CollectionTask"];
                    int i = 0;
                    foreach(System.Data.DataRow rr in dt.Rows)
                    {
                        if (rr["CollectionTaskID"].ToString() == this._CollectionTask.ID.ToString())
                            break;
                        i++;
                    }
                    this.collectionTaskBindingSource.Position = i;
                    R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                }

                {
                    // Collection
                    this.setCollectionControls();

                    // Part
                    this.setPartControl(TaskID);

                    // Module
                    if (DiversityCollection.LookupTable.TaskModuleType(TaskID).Length > 0)
                    {
                        _taskModuleType = Task.TaskModuleType.None;
                        string ModuleType = DiversityCollection.LookupTable.TaskModuleType(TaskID);
                        _taskModuleType = DiversityCollection.Task.TypeOfTaskModule(ModuleType);
                        this.labelModuleTitle.Text = DiversityCollection.LookupTable.TaskModuleTitle(TaskID);

                        if (DiversityCollection.LookupTable.DtTaskModule(TaskID).Rows.Count > 0)
                        {
                            this.pictureBoxTaskModuleList.Image = DiversityCollection.Specimen.ImageForModule(ModuleType);
                            this.toolTip.SetToolTip(this.pictureBoxTaskModuleList, ModuleType);
                            this.TaskVisible[CollectionTaskDetail.ModuleList] = true;
                            this.comboBoxTaskModuleList.DataSource = DiversityCollection.LookupTable.DtTaskModule(TaskID);
                            this.comboBoxTaskModuleList.DisplayMember = "DisplayText";
                            this.comboBoxTaskModuleList.ValueMember = "DisplayText";
                            if (!R["DisplayText"].Equals(System.DBNull.Value))
                            {
                                int i = 0;
                                foreach(System.Data.DataRow rr in DiversityCollection.LookupTable.DtTaskModule(TaskID).Rows)
                                {
                                    if (rr["DisplayText"].ToString() == R["DisplayText"].ToString())
                                    {
                                        this.comboBoxTaskModuleList.SelectedIndex = i;
                                        break;
                                    }
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            this.TaskVisible[CollectionTaskDetail.Module] = true;
                            this.pictureBoxTaskModule.Image = DiversityCollection.Specimen.ImageForModule(ModuleType);
                            this.toolTip.SetToolTip(this.pictureBoxTaskModule, ModuleType);
                            this.bindModuleControl();
                        }
                    }

                    // Result
                    if (DiversityCollection.LookupTable.TaskResultType(TaskID).Length > 0)
                    {
                        this.TaskVisible[CollectionTaskDetail.Result] = true;
                        this.labelResult.Text = DiversityCollection.LookupTable.TaskResultType(TaskID);
                        if (DiversityCollection.LookupTable.DtTaskResult(TaskID).Rows.Count > 1)
                        {
                            this.comboBoxResult.DropDownStyle = ComboBoxStyle.DropDownList;
                            this.comboBoxResult.DataSource = DiversityCollection.LookupTable.DtTaskResult(TaskID);
                            this.comboBoxResult.DisplayMember = "Result";
                            this.comboBoxResult.ValueMember = "Result";
                            if (!R["Result"].Equals(System.DBNull.Value))
                            {
                                int i = 0;
                                foreach (System.Data.DataRow rr in DiversityCollection.LookupTable.DtTaskResult(TaskID).Rows)
                                {
                                    if (rr["Result"].ToString() == R["Result"].ToString())
                                    {
                                        this.comboBoxResult.SelectedIndex = i;
                                        break;
                                    }
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            this.comboBoxResult.DropDownStyle = ComboBoxStyle.DropDown;
                            this.comboBoxResult.DataSource = null;
                        }
                    }

                    //Number
                    if (DiversityCollection.LookupTable.TaskNumberType(TaskID).Length > 0)
                    {
                        this.TaskVisible[CollectionTaskDetail.Number] = true;
                        this.labelTaskNumber.Text = DiversityCollection.LookupTable.TaskNumberType(TaskID) + ":";
                    }

                    //Bool
                    if (DiversityCollection.LookupTable.TaskBoolType(TaskID).Length > 0)
                    {
                        this.TaskVisible[CollectionTaskDetail.Bool] = true;
                        this.labelTaskBool.Text = DiversityCollection.LookupTable.TaskBoolType(TaskID) + ":";
                    }

                    //Date
                    if (DiversityCollection.LookupTable.TaskDateType(TaskID).Length > 0)
                    {
                        this.TaskVisible[CollectionTaskDetail.Date] = true;
                        if (DiversityCollection.LookupTable.TaskDateBeginType(TaskID).Length > 0)
                        {
                            this.TaskVisible[CollectionTaskDetail.DateBegin] = true;
                            if (DiversityCollection.LookupTable.TaskDateBeginType(TaskID).Length > 0)
                                this.labelTaskStart.Text = DiversityCollection.LookupTable.TaskDateBeginType(TaskID) + ":";
                            else
                                this.labelTaskStart.Text = "Start:";
                        }
                        if (DiversityCollection.LookupTable.TaskDateEndType(TaskID).Length > 0)
                        {
                            this.TaskVisible[CollectionTaskDetail.DateEnd] = true;
                            if (DiversityCollection.LookupTable.TaskDateEndType(TaskID).Length > 0)
                                this.labelTaskEnd.Text = DiversityCollection.LookupTable.TaskDateEndType(TaskID) + ":";
                            else
                                this.labelTaskEnd.Text = "End:";
                        }
                        this.setDateControls(TaskID);
                    }

                    // Description
                    if (DiversityCollection.LookupTable.TaskDescriptionType(TaskID).Length > 0)
                    {
                        this.TaskVisible[CollectionTaskDetail.Description] = true;
                        this.labelDescription.Text = DiversityCollection.LookupTable.TaskDescriptionType(TaskID) + ":";
                    }

                    // Notes
                    if (DiversityCollection.LookupTable.TaskNotesType(TaskID).Length > 0)
                    {
                        this.TaskVisible[CollectionTaskDetail.Notes] = true;
                        this.labelNotes.Text = DiversityCollection.LookupTable.TaskNotesType(TaskID) + ":";
                    }

                    //Uri
                    if (DiversityCollection.LookupTable.TaskUriType(TaskID).Length > 0)
                    {
                        this.TaskVisible[CollectionTaskDetail.Uri] = true;
                        this.labelURI.Text = DiversityCollection.LookupTable.TaskUriType(TaskID) + ":";
                    }
                    this.SetUriVisibility(DiversityCollection.LookupTable.TaskUriType(TaskID).Length > 0);

                }
                this.setTaskDetailControlsWidth();
                if (header == Header.Plan)
                    this.GetFloorPlan();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void flowLayoutPanelTask_SizeChanged(object sender, EventArgs e)
        {
            this.setTaskDetailControlsWidth();
        }

        private void setTaskDetailControlsWidth()
        {
            try
            {
                if (this.TaskPanels == null)
                    return;
                int i = 0;
                int Full = flowLayoutPanelTask.Width;
                int Frei = Full;
                int Width = 1;
                this.tableLayoutPanelTaskHeader.Width = (int)Full;
                foreach (System.Collections.Generic.KeyValuePair<CollectionTaskDetail, bool> KV in this.TaskVisible)
                {
                    double Divisor = 1.0;
                    if (this.TaskWidthDivisors.ContainsKey(KV.Key))
                        Divisor = this.TaskWidthDivisors[KV.Key];
                    else { }
                    if (TaskPanels.ContainsKey(KV.Key))
                    {
                        this.TaskPanels[KV.Key].Visible = KV.Value;
                        if (KV.Value)
                        {
                            Width = Full / this.TaskWidthDivisors[KV.Key];
                            if (KV.Key == CollectionTaskDetail.Date)
                            {
                                string DateType = DiversityCollection.LookupTable.TaskDateType((int)this._TaskID);
                                if (DateType.IndexOf("from to") == -1)
                                {
                                    Divisor = Divisor * 2;
                                    Width = Width / 2;
                                }
                            }
                            else if (KV.Key > CollectionTaskDetail.Date && i != 0)
                            {
                                Width = Frei;
                                //Width = Full;
                                switch (i)
                                {
                                    case 1:
                                        Divisor = 1.33333333333;
                                        break;
                                    case 2:
                                        Divisor = 2;
                                        break;
                                    case 3:
                                        Divisor = 4;
                                        break;
                                }
                            }
                            else
                                Width = (int)(Full / Divisor);

                            this.TaskPanels[KV.Key].Width = Width;
                            Frei = Frei - this.TaskPanels[KV.Key].Width;
                            i = i + (int)((double)4 / Divisor);
                        }
                        else
                            this.TaskPanels[KV.Key].Width = 1;

                        //this.TaskPanels[KV.Key].Width = Width;

                        if (i >= 4)
                        {
                            i = 0;
                            Frei = Full;
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSpecimenPart_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityCollection.Forms.FormCollectionSpecimen formCollectionSpecimen = new FormCollectionSpecimen(true);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            formCollectionSpecimen.ShowDialog();
            if (formCollectionSpecimen.DialogResult == DialogResult.OK)
            {
                int? PartID = formCollectionSpecimen.getSpecimenPartID();
                int CollectionSpecimenID = formCollectionSpecimen.ID;
                if (PartID != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                    R["CollectionSpecimenID"] = CollectionSpecimenID;
                    R["SpecimenPartID"] = PartID;
                    if (R["DisplayText"].Equals(System.DBNull.Value) || R["DisplayText"].ToString().Trim().Length == 0)
                    {
                        string Display = formCollectionSpecimen.getAccessionNumber();
                        if (Display.Length > 0) Display += " - ";
                        Display += formCollectionSpecimen.getSpecimenPartDisplayText();
                        R["DisplayText"] = Display;
                    }
                    this.setPartControl((int)this._CollectionTask.TaskID);
                }
            }
        }

        private void buttonSpecimenPartDetails_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
            int CollectionSpecimenID;
            if (int.TryParse( R["CollectionSpecimenID"].ToString(), out CollectionSpecimenID))
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityCollection.Forms.FormCollectionSpecimen formCollectionSpecimen = new FormCollectionSpecimen(CollectionSpecimenID, false, false, FormCollectionSpecimen.ViewMode.SingleInspectionMode, true);
                formCollectionSpecimen.Width = this.Width - 20;
                formCollectionSpecimen.Height = this.Height - 20;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                formCollectionSpecimen.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Missing entry for specimen part");
            }
        }

        private void setPartControl(int TaskID)
        {
            try
            {
                if (DiversityCollection.LookupTable.TaskSpecimenPartType(TaskID).Length > 0)
                {
                    this.TaskVisible[CollectionTaskDetail.Part] = true;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskBindingSource.Current;
                    if (!R["SpecimenPartID"].Equals(System.DBNull.Value))
                    {
                        System.Data.DataTable dt = new DataTable("CollectionSpecimenPart");
                        string SQL = "SELECT * FROM CollectionSpecimenPart WHERE SpecimenPartID = " + R["SpecimenPartID"].ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                        if (dt.Rows.Count == 1)
                        {
                            string Specimen = "";
                            int CollectionSpecimenID;
                            if (int.TryParse(dt.Rows[0]["CollectionSpecimenID"].ToString(), out CollectionSpecimenID))
                            {
                                System.Data.DataTable dtSpecimen = new DataTable("CollectionSpecimen");
                                SQL = "SELECT * FROM CollectionSpecimen WHERE CollectionSpecimenID = " + CollectionSpecimenID.ToString();
                                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSpecimen);
                                DiversityCollection.HierarchyNode Nspecimen = new HierarchyNode(dtSpecimen.Rows[0]);
                                Specimen = Nspecimen.setText().ToString() + " - ";
                                Nspecimen = null;
                            }
                            DiversityCollection.HierarchyNode N = new HierarchyNode(dt.Rows[0]);
                            this.textBoxSpecimenPart.Text = Specimen + N.setText().Trim();
                            this.pictureBoxSpecimenPart.Image = DiversityCollection.Specimen.MaterialCategoryImage(false, dt.Rows[0]["MaterialCategory"].ToString());
                            N = null;
                            if (this._iMainForm != null)
                            {
                                int PartID;
                                if (int.TryParse(R["SpecimenPartID"].ToString(), out PartID))
                                    this._iMainForm.setSpecimenPart(PartID);
                            }
                        }
                    }
                    else
                    {
                        this.textBoxSpecimenPart.Text = "";
                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Hierarchy specials

        private void toolStripButtonSetCollection_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("available in upcoming version");
        }

        private void toolStripMenuItemHierarchy_Click(object sender, EventArgs e)
        {
            this._CollectionTask.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Show);
            this.hierarchyToolStripMenuItem.Image = toolStripMenuItemHierarchy.Image;
            this.splitContainerTask.Panel1Collapsed = false;
            this._CollectionTask.buildHierarchy();
        }

        private void toolStripMenuItemHierarchySuperior_Click(object sender, EventArgs e)
        {
            this._CollectionTask.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Children);
            this.hierarchyToolStripMenuItem.Image = toolStripMenuItemHierarchySuperior.Image;
            this.splitContainerTask.Panel1Collapsed = false;
            this._CollectionTask.buildHierarchy();
        }

        private void toolStripMenuItemHierarchyNo_Click(object sender, EventArgs e)
        {
            this._CollectionTask.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Hide);
            this.hierarchyToolStripMenuItem.Image = toolStripMenuItemHierarchyNo.Image;
            this.splitContainerTask.Panel1Collapsed = true;
        }

        #endregion

        #region Table editor

        private void TableEditing()
        {
            try
            {
                string Table = "CollectionTask";
                //if (this.userControlQueryList.OptimizingIsUsed())
                //{
                //    this.TableEditingForOptimizing(Table, Icon, BlockedColumns);
                //}
                //else
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    if (this.userControlQueryList.ListOfIDs.Count == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Nothing selected");
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                    System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
                    DiversityWorkbench.Data.Table T = new DiversityWorkbench.Data.Table(Table);
                    foreach (string PK in T.PrimaryKeyColumnList)
                        ReadOnlyColumns.Add(PK);
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> DC in T.Columns)
                    {
                        if (DC.Key.StartsWith("Log") && !ReadOnlyColumns.Contains(DC.Key))
                            ReadOnlyColumns.Add(DC.Key);
                        else if (DC.Value.IsIdentity && !ReadOnlyColumns.Contains(DC.Key))
                            ReadOnlyColumns.Add(DC.Key);
                        else if (DC.Key.EndsWith("ID") && !ReadOnlyColumns.Contains(DC.Key))
                            ReadOnlyColumns.Add(DC.Key);
                    }
                    //if (BlockedColumns != null)
                    //{
                    //    foreach (string C in BlockedColumns)
                    //    {
                    //        if (!ReadOnlyColumns.Contains(C))
                    //            ReadOnlyColumns.Add(C);
                    //    }
                    //}

                    string IDs = "";
                    foreach (int i in this.userControlQueryList.ListOfIDs)
                    {
                        if (IDs.Length > 0) IDs += ",";
                        IDs += i.ToString();
                    }
                    string SQL = "SELECT * FROM " + Table + " T WHERE T.CollectionTaskID IN (" + IDs + ")";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000));
                    ad.SelectCommand.CommandTimeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000;
                    System.Drawing.Image Icon = DiversityCollection.Specimen.ImageForTable(Table, false);
                    DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, ReadOnlyColumns, DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout, null, Table);
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.Width = this.Width - 10;
                    f.Height = this.Height - 10;
                    f.setHelpProvider(this.helpProvider.HelpNamespace, "Table editor");
                    bool SetTimeout = false;
                    try
                    {
                        f.ShowDialog();
                        //this.set(this.ID);
                    }
                    catch (System.Exception ex)
                    {
                        SetTimeout = true;
                    }
                    if (SetTimeout)
                    {
                        int? Timeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout;
                        DiversityWorkbench.Forms.FormGetInteger ftimeout = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Set timeout", "A timeout occured. Please set the seconds you are prepared to wait");
                        ftimeout.ShowDialog();
                        if (ftimeout.DialogResult == System.Windows.Forms.DialogResult.OK && ftimeout.Integer != null)
                        {
                            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout = (int)ftimeout.Integer;
                            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "TableEditing(string Table, System.Drawing.Image Icon)");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        #endregion

        #region Spreadsheet

        private void StartSpreadSheet()
        {
            try
            {
                this.Enabled = false;
                string CurrentConnection = DiversityWorkbench.Settings.ConnectionString;
                DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard("Starting spreadsheet...", "", DiversityCollection.Resource.SpeadsheetEditor);
                int X = this.Location.X + (int)(this.Width / 2) - (int)(fs.Width / 2);
                int Y = this.Location.Y + (int)(this.Height / 2) - (int)(fs.Height / 2);
                System.Drawing.Point P = new Point(X, Y);
                fs.Location = P;
                fs.StartPosition = FormStartPosition.Manual;//.CenterScreen;

                fs.Show();
                fs.ShowCurrentStep("Getting definitions of the tables");
                Application.DoEvents();

                DiversityWorkbench.Spreadsheet.Sheet Sheet = Spreadsheet.Target.TargetSheet(Spreadsheet.Target.SheetTarget.CollectionTask);
                if (Sheet != null)
                {
                    //DiversityCollection.Forms.FormCollectionSpecimen fMain = new FormCollectionSpecimen(-1, false, false, ViewMode.SpreadsheetMode, "");
                    //Sheet.setInterfaceMainForm(fMain);

                    fs.ShowCurrentStep("Reading settings");
                    Application.DoEvents();
                    DiversityWorkbench.Spreadsheet.FormSpreadsheet f = new DiversityWorkbench.Spreadsheet.FormSpreadsheet(Sheet, this.helpProvider.HelpNamespace);
                    f.setImageColumn(Sheet.DataTables()["F0_CTI"].DataColumns()["URI"]);

                    // setting the image column if present
                    //switch (Sheet.Target())
                    //{
                    //    case "Event":
                    //        if (Sheet.DataTables().ContainsKey("B3_EI"))
                    //            f.setImageColumn(Sheet.DataTables()["B3_EI"].DataColumns()["URI"]);
                    //        else if (Sheet.DataTables().ContainsKey("C6_Im"))
                    //            f.setImageColumn(Sheet.DataTables()["C6_Im"].DataColumns()["URI"]);
                    //        break;
                    //    case "Parts":
                    //        if (Sheet.DataTables().ContainsKey("F8_Im"))
                    //            f.setImageColumn(Sheet.DataTables()["F8_Im"].DataColumns()["URI"]);
                    //        break;
                    //    case "Image":
                    //        if (Sheet.DataTables().ContainsKey("D0_Im"))
                    //            f.setImageColumn(Sheet.DataTables()["D0_Im"].DataColumns()["URI"]);
                    //        break;
                    //    case "Minerals":
                    //    case "Organisms":
                    //        if (Sheet.DataTables().ContainsKey("E6_Im"))
                    //            f.setImageColumn(Sheet.DataTables()["E6_Im"].DataColumns()["URI"]);
                    //        break;
                    //    case "Specimen":
                    //        if (Sheet.DataTables().ContainsKey("C6_Im"))
                    //            f.setImageColumn(Sheet.DataTables()["C6_Im"].DataColumns()["URI"]);
                    //        break;
                    //    case "TK25":
                    //        if (Sheet.DataTables().ContainsKey("E3_Im"))
                    //            f.setImageColumn(Sheet.DataTables()["E3_Im"].DataColumns()["URI"]);
                    //        break;
                    //}

                    //if (Spreadsheet.Target.StarterTarget() != null)
                    //    f.IsStarter = true;
                    //else
                    {
                        f.IsStarter = false;
                        f.setSize(this.Width, this.Height);
                        f.setLocation(this.Location.X, this.Location.Y);
                        f.StartPosition = FormStartPosition.Manual;
                    }
                    //if (Target == Spreadsheet.Target.SheetTarget.TK25)
                    //    f.SetMapColumns("Geography", "A2_EL", "IdentificationUnitID", "E0_U");
                    //else if (Target == Spreadsheet.Target.SheetTarget.WGS84)
                    //    f.SetMapColumns("Geography", "A2_EL", "IdentificationUnitID", "E0_U");
                    fs.Close();
                    if (!f.IsDisposed)
                    {
                        f.ShowDialog();
                        //if (f.IsStarter)
                        //{
                        //    Spreadsheet.Target.SetStart(Target);
                        //}
                        //else Spreadsheet.Target.SetStart(null);
                        //if (DiversityWorkbench.Settings.ConnectionString != CurrentConnection)
                        //{
                        //    System.Windows.Forms.MessageBox.Show("Connection changed to new database");
                        //    this.setDatabase();
                        //}
                        //fMain.Close();
                        //fMain.Dispose();
                    }
                }
                else
                {
                    fs.Close();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event delegates to form and controls
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
