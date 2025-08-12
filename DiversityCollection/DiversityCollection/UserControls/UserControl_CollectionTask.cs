using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_CollectionTask : UserControl__Data
    {
        #region Construction
        public UserControl_CollectionTask(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            try
            {
                this._Source = Source;
                this._HelpNamespace = HelpNamespace;
                this.initControl();
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                this.FormFunctions.setDescriptions(this);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Init
        private void initControl()
        {
            try
            {
                this.textBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Description", true));
                this.textBoxNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
                this.textBoxTaskNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "NumberValue", true));
                this.textBoxURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "URI", true));

                this.comboBoxResult.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Result", true));
                this.comboBoxTaskModuleList.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DisplayText", true));

                this.checkBoxTaskBool.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._Source, "BoolValue", true));

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);
                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxDescription);
                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxNotes);
                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxTaskNumber);
                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxURI);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                //this.CheckIfClientIsUpToDate();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //public override void SetPosition(int Position)
        //{
        //    base.SetPosition(Position);
        //    if (this._Source.Current != null)
        //    {
        //        System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
        //        this.setDetailControls(R);
        //    }
        //}

        #endregion


        #region Showing Detail Controls

        private enum CollectionTaskDetail { Module, ModuleList, Result, Number, Bool, Date, Uri, Description, Notes }
        private Task.TaskModuleType _taskModuleType = Task.TaskModuleType.None;
        private Task.TaskResultType _taskResultType = Task.TaskResultType.None;
        private Task.TaskDateType _taskDateType = Task.TaskDateType.None;

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

        public void setDetailControls(int TaskID, int CollectionTaskID)
        {
            try
            {
                // ensure TaskPanels
                if (this.TaskPanels.Count == 0)
                    return;

                foreach (System.Collections.Generic.KeyValuePair<CollectionTaskDetail, System.Windows.Forms.TableLayoutPanel> KV in this.TaskPanels)
                    this.TaskVisible[KV.Key] = false;

                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;

                {

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
                                foreach (System.Data.DataRow rr in DiversityCollection.LookupTable.DtTaskModule(TaskID).Rows)
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
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setDetailControls(System.Data.DataRowView R)
        {
            this.setDetailControls(R.Row);
        }

        public void setDetailControls(System.Data.DataRow R)
        {
            try
            {
                // ensure TaskPanels
                if (this.TaskPanels.Count == 0)
                    return;

                this.labelTaskHaeder.Text = CollectionTaskDisplay.NodeText(R, false);

                foreach (System.Collections.Generic.KeyValuePair<CollectionTaskDetail, System.Windows.Forms.TableLayoutPanel> KV in this.TaskPanels)
                    this.TaskVisible[KV.Key] = false;

                if (R != null)
                {
                    int CollectionTaskID = int.Parse(R["CollectionTaskID"].ToString());
                    int TaskID = int.Parse(R["TaskID"].ToString());
                    System.Data.DataSet ds = (System.Data.DataSet)this._Source.DataSource;
                    for (int i = 0; i < ds.Tables["CollectionTask"].Rows.Count; i++)
                    {
                        if (ds.Tables["CollectionTask"].Rows[i]["CollectionTaskID"].ToString() == CollectionTaskID.ToString())
                        {
                            this._Source.Position = i;
                            break;
                        }
                    }
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
                                foreach (System.Data.DataRow rr in DiversityCollection.LookupTable.DtTaskModule(TaskID).Rows)
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
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void flowLayoutPanelTask_SizeChanged(object sender, EventArgs e)
        {
            this.setTaskDetailControlsWidth();
        }

        private void bindModuleControl()
        {
            this.userControlModuleRelatedEntryValue.bindToData("CollectionTask", "DisplayText", "ModuleUri", this._Source);
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

                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
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
            //this._CollectionTask.saveItem();
            //this.setDateControls((int)this._CollectionTask.TaskID);
        }

        private void dateTimePickerTaskEnd_CloseUp(object sender, EventArgs e)
        {
            //this._CollectionTask.saveItem();
            //this.setDateControls((int)this._CollectionTask.TaskID);
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

                //if (this._Source.Current != null)
                //{
                //    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                //    R.BeginEdit();
                //    if (ForStart)
                //        R["TaskStart"] = System.DBNull.Value;
                //    else
                //        R["TaskEnd"] = System.DBNull.Value;
                //    R.EndEdit();
                //    this._CollectionTask.saveItem();
                //    this.setDateControls((int)this._CollectionTask.TaskID);
                //}
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
                //if (this._Source.Current != null)
                //{
                //    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                //    if (this.textBoxTaskNumber.Text.Length == 0) // && !R["NumberValue"].Equals(System.DBNull.Value))
                //    {
                //        R.BeginEdit();
                //        R["NumberValue"] = System.DBNull.Value;
                //        R.EndEdit();
                //        this._CollectionTask.saveItem();
                //    }
                //    else if (this.textBoxTaskNumber.Text.Length > 0)
                //    {
                //        if (this.textBoxTaskNumber.Text.IndexOf(",") > -1)
                //            this.textBoxTaskNumber.Text = this.textBoxTaskNumber.Text.Replace(",", ".");
                //        Double dd;
                //        if (!Double.TryParse(this.textBoxTaskNumber.Text, out dd))
                //        {
                //            System.Windows.Forms.MessageBox.Show(this.textBoxTaskNumber.Text + " is not a valid numeric value.");
                //        }
                //    }
                //}
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
            //if (this.comboBoxResult.DataSource == null && this.comboBoxResult.DropDownStyle == ComboBoxStyle.DropDown && this._TaskID != null)
            //{
            //    string SQL = "SELECT DISTINCT Result FROM CollectionTask WHERE TaskID = " + this._TaskID.ToString();
            //    System.Data.DataTable dataTable = new DataTable();
            //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            //    this.comboBoxResult.DataSource = dataTable;
            //    this.comboBoxResult.DisplayMember = "Result";
            //    this.comboBoxResult.ValueMember = "Result";
            //}
        }

        private void comboBoxResult_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (this._Source.Current != null)
            //{
            //    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            //    R.BeginEdit();
            //    R["Result"] = this.comboBoxResult.SelectedValue;
            //    R.EndEdit();
            //}
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

        #region URL

        private void buttonUrlOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    int i = this._Source.Position;
            //    this.dataSetCollectionTask.CollectionTask.Rows[i]["URI"] = f.URL;
            //    this.textBoxURI.Text = f.URL;
            //}
        }

        private void SetUriVisibility(bool IsVisible)
        {
            this.labelURI.Visible = IsVisible;
            this.textBoxURI.Visible = IsVisible;
            this.buttonUrlOpen.Visible = IsVisible;
            //this.splitContainerURI.Panel2Collapsed = !IsVisible || this.textBoxURI.Text.Length == 0;
        }

        #endregion


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
                    double Divisor = this.TaskWidthDivisors[KV.Key];
                    this.TaskPanels[KV.Key].Visible = KV.Value;
                    if (KV.Value)
                    {
                        Width = Full;
                        //Width = Full / this.TaskWidthDivisors[KV.Key];
                        //if (KV.Key == CollectionTaskDetail.Date)
                        //{
                        //    System.Data.DataRowView rowView = (System.Data.DataRowView)this._Source.Current;
                        //    string DateType = DiversityCollection.LookupTable.TaskDateType(int.Parse(rowView["TaskID"].ToString()));
                        //    if (DateType.IndexOf("from to") == -1)
                        //    {
                        //        Divisor = Divisor * 2;
                        //        Width = Width / 2;
                        //    }
                        //}
                        //else if (KV.Key > CollectionTaskDetail.Date && i != 0)
                        //{
                        //    Width = Frei;
                        //    //Width = Full;
                        //    switch (i)
                        //    {
                        //        case 1:
                        //            Divisor = 1.33333333333;
                        //            break;
                        //        case 2:
                        //            Divisor = 2;
                        //            break;
                        //        case 3:
                        //            Divisor = 4;
                        //            break;
                        //    }
                        //}
                        //else
                        //    Width = (int)(Full / Divisor);

                        this.TaskPanels[KV.Key].Width = Width;
                        //Frei = Frei - this.TaskPanels[KV.Key].Width;
                        //i = i + (int)((double)4 / Divisor);
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
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
