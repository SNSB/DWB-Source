using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class FormTask : Form
    {
        #region Parameter

        private DiversityCollection.Tasks.Task _Task;

        #endregion

        #region Construction

        public FormTask()
        {
            InitializeComponent();
            this.splitContainerData.Panel2Collapsed = false;
            this.splitContainerMain.Panel2.Visible = false;
            this.initForm();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }

        #endregion

        #region Form

        private void FormTask_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTask.TaskModule". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.taskModuleTableAdapter.Fill(this.dataSetTask.TaskModule);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTask.TaskResult". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.taskResultTableAdapter.Fill(this.dataSetTask.TaskResult);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTask.Task". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.taskTableAdapter.Fill(this.dataSetTask.Task);

        }

        private void initForm()
        {
            try
            {
                System.Data.DataSet Dataset = this.dataSetTask;
                if (this._Task == null)
                    this._Task = new Task(ref Dataset, this.dataSetTask.Task,
                        ref this.treeViewTask, this, this.userControlQueryList, this.splitContainerMain,
                        this.splitContainerData,
                        this.helpProvider, this.toolTip, ref this.taskBindingSource);
                this._Task.initForm();
                this._Task.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
                this._Task.setToolStripButtonNewEvent(this.toolStripButtonNew);
                this._Task.setToolStripButtonIncludeIDEvent(this.toolStripButtonIncludeID);
                this._Task.setToolStripButtonSetParentWithHierarchyEvent(this.toolStripButtonSetParent);
                this._Task.setToolStripButtonRemoveParentEvent(this.toolStripButtonRemoveParent);
                this._Task.setFormTask(this);
                this.setPermissions();

                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.userControlQueryList.RememberQuerySettingsIdentifier = "Task";
                this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();
                if (this.userControlQueryList.RememberQuerySettings() && this.userControlQueryList.ListOfIDs.Count > 0)
                {
                    int Index = -1;
                    int.TryParse(this.userControlQueryList.RememberedIndex().ToString(), out Index);
                    if (this.userControlQueryList.listBoxQueryResult.Items.Count > Index)
                        this.userControlQueryList.listBoxQueryResult.SelectedIndex = Index;
                    else
                        this.userControlQueryList.listBoxQueryResult.SelectedIndex = -1;
                }
                this.initTypeLists();
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void FormTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._Task.saveItem();
            DiversityCollection.LookupTable.ResetTask();

            if (this.userControlQueryList.RememberQuerySettings())
                this.userControlQueryList.RememberQueryConditionSettings_SaveToFile();
            else
                this.userControlQueryList.RememberQueryConditionSettings_RemoveFile();

        }

        #endregion

        #region Types

        private void buttonType_Click(object sender, EventArgs e)
        {
            this.EditType();
        }

        private void typeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditType();
        }

        private void EditType()
        {
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                DiversityCollection.Resource.Task,
                "TaskType_Enum",
                "Administration of task types",
                "",
                DiversityCollection.Specimen.TaskType_Images);//, Directory);
            f.HierarchyChangesEnabled = true;
            f.setHelp("Task");
            f.ShowDialog();
            if (f.DataHaveBeenChanged)
            {
                DiversityCollection.Specimen.TaskType_Images = f.Images;
                DiversityCollection.LookupTable.ResetTask();
                DiversityCollection.LookupTable.ResetTaskTypes();
                DiversityCollection.Specimen.ResetTaskType_Images();
            }
        }

        #endregion

        #region Details

        #region Init lists

        private void initTypeLists()
        {
            this.initTypeList();
            this.initDateTypeList();
            this.initModuleTypeList();
        }

        private void initTypeList()
        {
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxType, "TaskType_Enum", DiversityWorkbench.Settings.Connection, false, true, true);
        }

        private void initDateTypeList()
        {
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxDateType, "TaskDateType_Enum", DiversityWorkbench.Settings.Connection);
            //this.comboBoxDateType.Items.Clear();
            //this.comboBoxDateType.Items.Add("");
            //this.comboBoxDateType.Items.Add("Date");
            //this.comboBoxDateType.Items.Add("Date from to");
            //this.comboBoxDateType.Items.Add("Date & Time");
            //this.comboBoxDateType.Items.Add("Date & Time from to");
            //this.comboBoxDateType.Items.Add("Time");
            //this.comboBoxDateType.Items.Add("Time from to");

        }

        private void initModuleTypeList()
        {
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxModuleType, "TaskModuleType_Enum", DiversityWorkbench.Settings.Connection);
            //this.comboBoxModuleType.Items.Clear();
            //this.comboBoxModuleType.Items.Add("");
            //this.comboBoxModuleType.Items.Add("DiversityAgents");
            //this.comboBoxModuleType.Items.Add("DiversityCollection");
            //this.comboBoxModuleType.Items.Add("DiversityGazetteer");
            //this.comboBoxModuleType.Items.Add("DiversityProjects");
            //this.comboBoxModuleType.Items.Add("DiversitySamplingPlots");
            //this.comboBoxModuleType.Items.Add("DiversityScientificTerms");
            //this.comboBoxModuleType.Items.Add("DiversityTaxonNames");
        }

        #endregion

        public void SetFormAccordingToItem(bool ShowResults = false)
        {
            try
            {
                if (this._Task.ID != null)
                {
                    if (this.textBoxURI.Text.Length == 0)
                        this.splitContainerURI.Panel2Collapsed = true;
                    else
                        this.splitContainerURI.Panel2Collapsed = false;
                    {
                        string selectedModule = "";
                        if (this.taskBindingSource != null && this.taskBindingSource.Current != null)
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)this.taskBindingSource.Current;
                            if (!R["ModuleType"].Equals(System.DBNull.Value) && R["ModuleType"].ToString().Length > 0)
                                selectedModule = R["ModuleType"].ToString();
                        }
                        if (selectedModule == "")
                        {
                            if (this.comboBoxModuleType.SelectedItem != null && this.comboBoxModuleType.SelectedItem.GetType() == typeof(System.Data.DataRowView)) // && this.comboBoxModuleType.SelectedIndex > 0)
                            {
                                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxModuleType.SelectedItem;
                                selectedModule = R[this.comboBoxModuleType.DisplayMember].ToString();
                            }
                            else if (this.comboBoxModuleType.Text.Length > 0)
                                selectedModule = this.comboBoxModuleType.Text;
                        }
                        bool IsModuleRelated = DiversityCollection.Task.IsModuleRelated(selectedModule);
                        if (IsModuleRelated)
                        {
                            Task.TaskModuleType taskType = Task.TypeOfTaskModule(selectedModule);
                            switch (taskType)
                            {
                                case Task.TaskModuleType.Agent:
                                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                                    this.userControlModuleRelatedEntryTaskModule.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                                    break;
                                case Task.TaskModuleType.Gazetteer:
                                    DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                                    this.userControlModuleRelatedEntryTaskModule.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;
                                    break;
                                case Task.TaskModuleType.Project:
                                    DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                                    this.userControlModuleRelatedEntryTaskModule.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
                                    break;
                                case Task.TaskModuleType.Reference:
                                    DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                                    this.userControlModuleRelatedEntryTaskModule.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)R;
                                    break;
                                case Task.TaskModuleType.SamplingPlot:
                                    DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                                    this.userControlModuleRelatedEntryTaskModule.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)SP;
                                    break;
                                case Task.TaskModuleType.ScientificTerm:
                                    DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                                    this.userControlModuleRelatedEntryTaskModule.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)ST;
                                    break;
                                case Task.TaskModuleType.TaxonName:
                                    DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                                    this.userControlModuleRelatedEntryTaskModule.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
                                    break;
                            }
                            this.userControlModuleRelatedEntryTaskModule.bindToData("TaskModule", "DisplayText", "URI", this.taskModuleBindingSource);
                        }
                    }
                    this.setTaskControls();
                    this.setTaskImage();
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void setPermissions()
        {
            DiversityWorkbench.Forms.FormFunctions FF = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            this._Task.setToolbarPermission(ref this.toolStripButtonDelete, "Task", "Delete");
            this._Task.setToolbarPermission(ref this.toolStripButtonNew, "Task", "Insert");
        }


        private void setTaskImage()
        {
            try
            {
                if (this.taskBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.taskBindingSource.Current;
                    this.buttonType.Image = DiversityCollection.Specimen.ImageList.Images[DiversityCollection.Specimen.ImageIndex(R.Row, false)];
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonUriOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int i = this.taskBindingSource.Position;
                this.dataSetTask.Task.Rows[i]["TaskURI"] = f.URL;
                this.textBoxURI.Text = f.URL;
            }
        }

        #region Collection task configuration

        #region Common

        private void setTaskControls()
        {
            //this.setTaskControl(this.comboBoxType, this.labelType);            
            try
            {
                this.setTaskControl(this.textBoxDescriptionType, this.labelDescriptionType);
                this.setTaskControl(this.textBoxNotesType, this.labelNotesType);
                this.setTaskControl(this.textBoxUriType, this.labelUriType);
                this.setTaskControl(this.textBoxResponsibleType, this.labelResponsibleType);

                this.setTaskControl(this.comboBoxDateType, this.labelDateType);
                this.setTaskControl(this.textBoxDateEndType, this.labelDateEndType);
                this.setTaskControl(this.textBoxDateBeginType, this.labelDateBeginType);

                this.setTaskControl(this.textBoxMetricType, this.labelMetricType);
                this.setTaskControl(this.textBoxMetricUnit, this.labelMetricUnit);
                this.setTaskControl(this.textBoxNumberType, this.labelNumberType);
                this.setTaskControl(this.textBoxBoolType, this.labelBoolType);

                this.setTaskControl(this.textBoxSpecimenPartType, this.labelSpecimenPartType);
                this.setTaskControl(this.textBoxTransactionType, this.labelTransactionType);

                this.setResultControls();
                this.setTaskControl(this.textBoxModuleTitle);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void setTaskControl(System.Windows.Forms.Control control, System.Windows.Forms.Label label = null)
        {
            try
            {
                bool IsIncluded = false;
                if (control.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)control;
                    IsIncluded = (textBox.Text.Trim().Length > 0);
                }
                else if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox Box = (System.Windows.Forms.ComboBox)control;
                    IsIncluded = (Box.Text.Trim().Length > 0);
                }

                if (control.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)control;
                    if (IsIncluded)
                    {
                        textBox.BackColor = System.Drawing.SystemColors.Window;
                        textBox.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        textBox.BackColor = SystemColors.ControlLight;
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                    }
                }
                else if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox Box = (System.Windows.Forms.ComboBox)control;
                    if (IsIncluded)
                    {
                        Box.FlatStyle = FlatStyle.Standard;
                    }
                    else
                    {
                        Box.FlatStyle = FlatStyle.Flat;
                    }
                }
                if (label != null)
                {
                    if (IsIncluded)
                    {
                        label.ForeColor = System.Drawing.SystemColors.ControlText;
                    }
                    else
                    {
                        label.ForeColor = System.Drawing.SystemColors.ControlDark;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Date
        private void comboBoxDateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.setTaskImage();
            this.setTaskControl(this.comboBoxDateType, this.labelDateType);
            this.setTaskDateControls();
        }

        private void setTaskDateControls()
        {
            string DateType = this.comboBoxDateType.Text;
            this.textBoxDateBeginType.Enabled = false;
            this.textBoxDateEndType.Enabled = false;
            switch (DateType)
            {
                case "":
                    this.pictureBoxDateStartDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateStartTime.Image = DiversityCollection.Resource.TimeGrey;
                    this.labelDateEnd.ForeColor = System.Drawing.SystemColors.ControlDark;
                    this.pictureBoxDateEndDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateEndTime.Image = DiversityCollection.Resource.TimeGrey;
                    break;
                case "Date":
                    this.pictureBoxDateStartDate.Image = DiversityCollection.Resource.Kalender;
                    this.pictureBoxDateStartTime.Image = DiversityCollection.Resource.TimeGrey;
                    this.labelDateEnd.ForeColor = System.Drawing.SystemColors.ControlDark;
                    this.pictureBoxDateEndDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateEndTime.Image = DiversityCollection.Resource.TimeGrey;
                    this.textBoxDateBeginType.Enabled = true;
                    break;
                case "Date from to":
                    this.pictureBoxDateStartDate.Image = DiversityCollection.Resource.Kalender;
                    this.pictureBoxDateStartTime.Image = DiversityCollection.Resource.TimeGrey;
                    this.labelDateEnd.ForeColor = System.Drawing.SystemColors.ControlText;
                    this.pictureBoxDateEndDate.Image = DiversityCollection.Resource.Kalender;
                    this.pictureBoxDateEndTime.Image = DiversityCollection.Resource.TimeGrey;
                    this.textBoxDateBeginType.Enabled = true;
                    this.textBoxDateEndType.Enabled = true;
                    break;
                case "Date & Time":
                    this.pictureBoxDateStartDate.Image = DiversityCollection.Resource.Kalender;
                    this.pictureBoxDateStartTime.Image = DiversityCollection.Resource.Time;
                    this.labelDateEnd.ForeColor = System.Drawing.SystemColors.ControlDark;
                    this.pictureBoxDateEndDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateEndTime.Image = DiversityCollection.Resource.TimeGrey;
                    this.textBoxDateBeginType.Enabled = true;
                    break;
                case "Date & Time from to":
                    this.pictureBoxDateStartDate.Image = DiversityCollection.Resource.Kalender;
                    this.pictureBoxDateStartTime.Image = DiversityCollection.Resource.Time;
                    this.labelDateEnd.ForeColor = System.Drawing.SystemColors.ControlText;
                    this.pictureBoxDateEndDate.Image = DiversityCollection.Resource.Kalender;
                    this.pictureBoxDateEndTime.Image = DiversityCollection.Resource.Time;
                    this.textBoxDateBeginType.Enabled = true;
                    this.textBoxDateEndType.Enabled = true;
                    break;
                case "Time":
                    this.pictureBoxDateStartDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateStartTime.Image = DiversityCollection.Resource.Time;
                    this.labelDateEnd.ForeColor = System.Drawing.SystemColors.ControlDark;
                    this.pictureBoxDateEndDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateEndTime.Image = DiversityCollection.Resource.TimeGrey;
                    this.textBoxDateBeginType.Enabled = true;
                    break;
                case "Time from to":
                    this.pictureBoxDateStartDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateStartTime.Image = DiversityCollection.Resource.Time;
                    this.labelDateEnd.ForeColor = System.Drawing.SystemColors.ControlText;
                    this.pictureBoxDateEndDate.Image = DiversityCollection.Resource.KalenderGrey;
                    this.pictureBoxDateEndTime.Image = DiversityCollection.Resource.Time;
                    this.textBoxDateBeginType.Enabled = true;
                    this.textBoxDateEndType.Enabled = true;
                    break;
            }
        }

        #endregion

        #region Module

        private void comboBoxModuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setModuleControls();
            this.setModuleImage();
        }

        private void setModuleImage()
        {
            string ModuleType = this.comboBoxModuleType.Text;
            if (ModuleType.Length > 0)
                this.pictureBoxTaskModule.Image = DiversityCollection.Specimen.ImageForModule(ModuleType);
            else
                this.pictureBoxTaskModule.Image = null;
        }

        private void listBoxTaskModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setModuleControls();
        }

        private void setModuleControls()
        {
            if (this.taskBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.taskBindingSource.Current;
                this.toolStripTaskModule.Enabled = (!R["ModuleType"].Equals(System.DBNull.Value) && R["ModuleType"].ToString().Length > 0);
            }
            else
                this.toolStripTaskModule.Enabled = (this.textBoxModuleTitle.Text.Length > 0);
            this.setTaskControl(this.comboBoxType, this.labelType);
            this.tableLayoutPanelTaskModule.Enabled = (this.listBoxTaskModule.SelectedIndex > -1);
            if (this.listBoxTaskModule.SelectedIndex > -1)
            {
                this.labelDisplayTextType.Text = "      Displayt.:";
                this.labelDisplayTextType.Image = DiversityCollection.Resource.DiversityWorkbench;
            }
            else
            {
                this.labelDisplayTextType.Text = "Displaytext:";
                this.labelDisplayTextType.Image = null;
            }
        }

        private void toolStripButtonTaskModuleAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New module", "Please enter the display text of the new entry", "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityCollection.Tasks.DataSetTask.TaskModuleRow R = this.dataSetTask.TaskModule.NewTaskModuleRow();
                R.DisplayText = f.String;
                R.TaskID = (int)this._Task.ID;
                this.dataSetTask.TaskModule.Rows.Add(R);
                this._Task.saveDependentTables();
                this._Task.setItem((int)this._Task.ID);
                this.SetFormAccordingToItem(true);
            }
        }

        private void toolStripButtonTaskModuleDelete_Click(object sender, EventArgs e)
        {
            int TaskID;
            System.Data.DataRowView R = (System.Data.DataRowView)this.taskModuleBindingSource.Current;
            if (int.TryParse(R["TaskID"].ToString(), out TaskID))
            {
                //string SQL = "SELECT COUNT(*) AS Expr1 " +
                //    "FROM  CollectionEventParameterValue " +
                //    "WHERE     Value <> '' AND     ParameterID =   " + TaskID.ToString();
                //string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //if (Result != "0")
                //{
                //    string Message = "The parameter is " + Result + " times used for methods in collection events.\r\nDo you really want to delete it?";
                //    if (System.Windows.Forms.MessageBox.Show(Message, "Delete Parameter", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                //        return;
                //}
                //SQL = "SELECT COUNT(*) AS Expr1 " +
                //    "FROM  IdentificationUnitAnalysisMethodParameter " +
                //    "WHERE     Value <> '' AND     ParameterID =   " + TaskID.ToString();
                //Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //if (Result != "0")
                //{
                //    string Message = "The parameter is " + Result + " times used for methods in the analysis or organisms.\r\nDo you really want to delete it?";
                //    if (System.Windows.Forms.MessageBox.Show(Message, "Delete Parameter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                //        return;
                //}
                //SQL = "DELETE " +
                //    "FROM  CollectionEventParameterValue " +
                //    "WHERE ParameterID = " + TaskID.ToString();
                //DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                //SQL = "DELETE " +
                //    "FROM  IdentificationUnitAnalysisMethodParameter " +
                //    "WHERE ParameterID = " + TaskID.ToString();
                //DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                this.taskModuleBindingSource.RemoveCurrent();
                this._Task.saveDependentTables();
            }
        }

        #endregion

        #region Results

        private void textBoxTaskResultType_TextChanged(object sender, EventArgs e)
        {
            this.setResultControls();
        }

        private void textBoxTaskResultType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setResultControls();
        }


        private void setResultControls()
        {
            this.setTaskControl(this.textBoxTaskResultType);
            this.toolStripResult.Enabled = (this.textBoxTaskResultType.Text.Length > 0);
            this.tableLayoutPanelResult.Enabled = (this.listBoxResults.SelectedIndex > -1);
        }

        private void toolStripButtonResultAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New Result", "Please enter the name of the new result", "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityCollection.Tasks.DataSetTask.TaskResultRow R = this.dataSetTask.TaskResult.NewTaskResultRow();
                R.Result = f.String;
                R.TaskID = (int)this._Task.ID;
                this.dataSetTask.TaskResult.Rows.Add(R);
                this._Task.saveDependentTables();
                this._Task.setItem((int)this._Task.ID);
                this.SetFormAccordingToItem(true);
            }
        }

        private void toolStripButtonResultDelete_Click(object sender, EventArgs e)
        {
            int TaskID;
            System.Data.DataRowView R = (System.Data.DataRowView)this.taskResultBindingSource.Current;
            if (int.TryParse(R["TaskID"].ToString(), out TaskID))
            {
                this.taskResultBindingSource.RemoveCurrent();
                this._Task.saveDependentTables();
            }
        }

        private void buttonResultUri_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxResultUri.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxResultUri.Text = f.URL;
                System.Data.DataRowView R = (System.Data.DataRowView)this.taskResultBindingSource.Current;
                R["URI"] = f.URL;
            }
        }

        private void listBoxResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setResultControls();
        }

        #endregion

        #endregion

        #region Collection task controls

        //Number
        private void textBoxNumberType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxNumberType, this.labelNumberType);
        }

        private void textBoxNumberType_Leave(object sender, EventArgs e)
        {
            //this.setTaskControl(this.textBoxNumberType, this.labelNumberType);
        }

        private void textBoxNumberType_TextChanged(object sender, EventArgs e)
        {
            //this.setTaskControl(this.textBoxNumberType, this.labelNumberType);
        }

        //Bool

        private void textBoxBoolType_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxBoolType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxBoolType, this.labelBoolType);

        }

        // Uri

        private void textBoxUriType_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxUriType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxUriType, this.labelUriType);

        }

        // Description

        private void textBoxDescriptionType_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxDescriptionType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxDescriptionType, this.labelDescriptionType);

        }

        // Notes

        private void textBoxNotesType_TextChanged(object sender, EventArgs e)
        {
        }


        private void textBoxNotesType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxNotesType, this.labelNotesType);

        }

        #endregion

        private void textBoxDisplayText_TextChanged(object sender, EventArgs e)
        {
            if (this.taskBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.taskBindingSource.Current;
                if (R["TaskParentID"].Equals(System.DBNull.Value))
                {
                    string SQL = "SELECT COUNT(*) FROM Task WHERE TaskParentID IS NULL AND Type = '" + R["Type"].ToString() + "' AND DisplayText = '" + R["DisplayText"].ToString() + "' AND TaskID <> " + R["TaskID"].ToString();
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result != "0")
                        this.textBoxDisplayText.BackColor = System.Drawing.Color.Pink;
                    else
                        this.textBoxDisplayText.BackColor = System.Drawing.Color.White;
                }
            }
        }



        private void textBoxModuleTitle_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxModuleTitle);
        }

        private void textBoxSpecimenPartType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxSpecimenPartType, this.labelSpecimenPartType);
        }

        private void textBoxTransactionType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxTransactionType, this.labelTransactionType);
        }

        private void textBoxDateBeginType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxDateBeginType, this.labelDateBeginType);
        }

        private void textBoxDateEndType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxDateEndType, this.labelDateEndType);
        }

        private void textBoxResponsibleType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxResponsibleType, this.labelResponsibleType);
        }

        private void textBoxMetricType_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxMetricType, this.labelMetricType);
        }

        private void textBoxMetricUnit_BindingContextChanged(object sender, EventArgs e)
        {
            this.setTaskControl(this.textBoxMetricUnit, this.labelMetricUnit);
        }
        #endregion

        #region Import
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartWizard();//, this.ImportStepsEvent(true));
        }

        private void StartWizard()
        {
            DiversityWorkbench.Import.FormStartWizard fs = null;
            try
            {
                this.Enabled = false;

                // Show start window
                fs = new DiversityWorkbench.Import.FormStartWizard();
                fs.StartPosition = FormStartPosition.CenterParent;
                fs.Show();
                Application.DoEvents();

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                fs.ShowCurrentStep("Resetting the templates for the tables\r\n..........");
                DiversityWorkbench.Import.Import.ResetTemplate();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportSteps = null;
                fs.ShowCurrentStep("Getting the steps for Task\r\n|.........");
                DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn = null;
                ImportSteps = Tasks.Task.ImportStepsTask; // this.ImportStepsTask;
                if (ImportSteps == null)
                    ImportSteps = new Dictionary<string, DiversityWorkbench.Import.Step>();
                fs.ShowCurrentStep("Import steps for CollectionTask created. Opening import form\r\n||||||||..");
                DiversityWorkbench.Import.FormWizard f = new DiversityWorkbench.Import.FormWizard("Task", DiversityCollection.Properties.Settings.Default.DatabaseVersion, ImportSteps, this.helpProvider.HelpNamespace, fs);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Import.Import.SchemaFileSource, string> SFS = new Dictionary<DiversityWorkbench.Import.Import.SchemaFileSource, string>();
                SFS.Add(DiversityWorkbench.Import.Import.SchemaFileSource.SNSB, "https://github.com/SNSB/DWB-Contrib/tree/master/DiversityCollection/Import/Schemas");
                SFS.Add(DiversityWorkbench.Import.Import.SchemaFileSource.ZFMK, "https://github.com/ZFMK/Labels-and-Imports-for-DiversityWorkbench/tree/master/ImportSchedules");
                DiversityWorkbench.Import.iWizardInterface i = f;
                i.SetSourcesForSchemaFiles(SFS);

                f.Height = this.Height - 10;
                f.StartPosition = FormStartPosition.CenterParent;

                fs.Close();
                fs = null;

                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Enabled = true;
                if (fs != null)
                    fs.Close();
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        //private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportStepsTask
        //{
        //    get
        //    {
        //        System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> IS = new Dictionary<string, DiversityWorkbench.Import.Step>();
        //        try
        //        {
        //            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, DiversityWorkbench.Import.StepColumnGroup>();
        //            DiversityWorkbench.Import.StepColumnGroup GValues = this.ImportWizardStepColumnGroup("Date and values",
        //                DiversityCollection.Specimen.ImageForTable("Task", false),
        //                "DateType, DateBeginType, DateEndType, ResultType, NumberType, BoolType, MetricType");
        //            ColumnGroups.Add(GValues.DisplayText, GValues);

        //            DiversityWorkbench.Import.DataTable DT = this.ImportWizardDataTable("Task", "", "Task", DiversityWorkbench.Import.DataTable.Parallelity.unique, 1, "", "TaskID");
        //            DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStepTemplate(DT, DiversityCollection.Specimen.ImageForTable(DT.TableName, false), 0, true, ColumnGroups);
        //            S.MustSelect = true;
        //            IS.Add(DT.PositionKey, S);

        //            DiversityWorkbench.Import.DataTable DTTaskModule = this.ImportWizardDataTable("TaskModule", DT.TableAlias, "Module", DiversityWorkbench.Import.DataTable.Parallelity.parallel, 2, "", "");
        //            DTTaskModule.ParentTableAlias = DT.TableAlias;
        //            DTTaskModule.Image = DiversityCollection.Specimen.ImageForTable("TaskModule", false);
        //            DiversityWorkbench.Import.Step STaskModule = DiversityWorkbench.Import.Step.GetStepTemplate(DTTaskModule, DiversityCollection.Specimen.ImageForTable("TaskModule", false), 1, false, null);
        //            IS.Add(DTTaskModule.PositionKey, STaskModule);

        //            DiversityWorkbench.Import.DataTable DTTaskResult = this.ImportWizardDataTable("TaskResult", DT.TableAlias, "Result", DiversityWorkbench.Import.DataTable.Parallelity.parallel, 3, "", "");
        //            DTTaskResult.ParentTableAlias = DT.TableAlias;
        //            DTTaskResult.Image = DiversityCollection.Specimen.ImageForTable("TaskResult", false);
        //            DiversityWorkbench.Import.Step STaskResult = DiversityWorkbench.Import.Step.GetStepTemplate(DTTaskResult, DiversityCollection.Specimen.ImageForTable("TaskResult", false), 1, false, null);
        //            IS.Add(DTTaskResult.PositionKey, STaskResult);
        //        }
        //        catch (System.Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }

        //        return IS;
        //    }
        //}

        //private DiversityWorkbench.Import.StepColumnGroup ImportWizardStepColumnGroup(string DisplayText, System.Drawing.Image Image, string Columns)
        //{
        //    System.Collections.Generic.List<string> L = new List<string>();
        //    Columns = Columns.Replace(" ", "");
        //    string[] CC = Columns.Split(new char[] { ',' });
        //    for (int i = 0; i < CC.Length; i++)
        //        L.Add(CC[i]);
        //    DiversityWorkbench.Import.StepColumnGroup G = new DiversityWorkbench.Import.StepColumnGroup(Image, DisplayText, L);
        //    return G;
        //}


        //private DiversityWorkbench.Import.DataTable ImportWizardDataTable(string TableName, string ParentTableAlias, string DisplayText, DiversityWorkbench.Import.DataTable.Parallelity P, int SequencePosition, string IgnoredColumns, string AttachmentColumns)
        //{
        //    System.Collections.Generic.List<string> lAC = new List<string>();
        //    AttachmentColumns = AttachmentColumns.Replace(" ", "");
        //    string[] ACC = AttachmentColumns.Split(new char[] { ',' });
        //    for (int i = 0; i < ACC.Length; i++)
        //        lAC.Add(ACC[i]);
        //    System.Collections.Generic.List<string> IC = new List<string>();
        //    IgnoredColumns = IgnoredColumns.Replace(" ", "");
        //    string[] CC = IgnoredColumns.Split(new char[] { ',' });
        //    for (int i = 0; i < CC.Length; i++)
        //        IC.Add(CC[i]);
        //    DiversityWorkbench.Import.DataTable DT = DiversityWorkbench.Import.DataTable.GetTableTemplate(TableName, ParentTableAlias, DisplayText, DiversityCollection.Specimen.ImageForTable(TableName, false), P, SequencePosition, IC, lAC);
        //    return DT;
        //}

        #endregion

    }
}
