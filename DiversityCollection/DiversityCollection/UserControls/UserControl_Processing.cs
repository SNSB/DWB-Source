using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_Processing : UserControl__Data
    {

        #region Construction

        public UserControl_Processing(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Init

        private void initControl()
        {
            this.textBoxProcessingProtocoll.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Protocoll", true));
            //this.textBoxProcessingDuration.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ProcessingDuration", true));

            this.userControlDurationProcessing.textBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ProcessingDuration", true));
            this.userControlDurationProcessing.textBox.TextChanged += this.textBoxProcessingDuration_TextChanged;
            //System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            //string Duration = R["ProcessingDuration"].ToString();
            //this.userControlDurationProcessing.Duration = Duration;

            //this.userControlDurationProcessing.Duration = this._Source[;

            this.textBoxProcessingNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxSpecimenProcessingID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "SpecimenProcessingID", true));
            //this.comboBoxProcessingID.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ProcessingID", true));
            //this.textBoxProcessingDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ProcessingDate", true));

            this.dateTimePickerProcessingDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._Source, "ProcessingDate", true));

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxProcessingNotes);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxProcessingProtocoll);

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryProcessingResponsible, A, "CollectionSpecimenProcessing", "ResponsibleName", "ResponsibleAgentURI", this._Source);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void textBoxProcessingDuration_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    if ((!R["ProcessingDuration"].Equals(System.DBNull.Value) && R["ProcessingDuration"].ToString() != this.userControlDurationProcessing.textBox.Text) ||
                        (R["ProcessingDuration"].Equals(System.DBNull.Value) && this.userControlDurationProcessing.textBox.Text.Length > 0))
                    {
                        R.BeginEdit();
                        R["ProcessingDuration"] = this.userControlDurationProcessing.textBox.Text;
                        R.EndEdit();
                    }
                }
                if (this._iMainForm.SelectedPartHierarchyNode() != null)
                {
                    DiversityCollection.HierarchyNode N = (DiversityCollection.HierarchyNode)this._iMainForm.SelectedPartHierarchyNode();
                    N.setText();
                    this.groupBoxProcessing.Text = N.Text;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //this.userControlDurationProcessing.setControls(this.userControlDurationProcessing.textBox.Text);
            //this._IsIsoFormat = this.ConvertToIsoFormat(this.userControlDurationProcessing.textBox.Text);
            //this.setControls();
        }


        #endregion

        #region Events

        #region Date
        private void dateTimePickerProcessingDate_DropDown(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!R["ProcessingDate"].Equals(System.DBNull.Value))
                {
                    System.DateTime DT;
                    if (System.DateTime.TryParse(R["ProcessingDate"].ToString(), out DT))
                    {
                        this.dateTimePickerProcessingDate.Value = DT;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dateTimePickerProcessingDate_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            System.DateTime DT;
            if (System.DateTime.TryParse(this.dateTimePickerProcessingDate.Value.ToShortDateString(), out DT))
            {
            }
            this.setDate();
        }

        private void setDate()
        {
            bool DateIsSet = false;
            try
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    if (!R["ProcessingDate"].Equals(System.DBNull.Value))
                        DateIsSet = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (DateIsSet)
                this.dateTimePickerProcessingDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            else
                this.dateTimePickerProcessingDate.CustomFormat = "-";
            this.dateTimePickerProcessingDuration.Enabled = DateIsSet;
        }

        private void buttonProcessingDateDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                R.BeginEdit();
                R["ProcessingDate"] = System.DBNull.Value;
                //this.textBoxProcessingDate.Text = null;
                R.EndEdit();
                this.setDate();
                this._iMainForm.saveSpecimen();// this.sqlDataAdapterProcessing.Update(this.dataSetCollectionSpecimen.CollectionSpecimenProcessing);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region Duration
        private void dateTimePickerProcessingDuration_CloseUp(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (R["ProcessingDate"].Equals(System.DBNull.Value) || !_PeriodDurationAsIsoPeriod)
                {
                    string Duration = this.dateTimePickerProcessingDuration.Value.ToString("yyyy-MM-dd HH:mm:ss"); 
                    R["ProcessingDuration"] = Duration;
                    this.setDurationText(Duration);
                    //this.userControlDurationProcessing.textBox.Text = Duration;
                }
                else
                {
                    System.DateTime DTfrom;
                    System.DateTime DTto = this.dateTimePickerProcessingDuration.Value;
                    if (System.DateTime.TryParse(R["ProcessingDate"].ToString(), out DTfrom))
                    {
                        System.TimeSpan dateTimeDuration = DTto.Subtract(DTfrom);
                        int Days = dateTimeDuration.Days;
                        int Years = 0;
                        int Months = 0;
                        while (Days > 365)
                        {
                            Years++;
                            Days = Days - 365;
                        }
                        while (Days > 30)
                        {
                            Months++;
                            Days = Days - 30;
                        }
                        int hour = dateTimeDuration.Hours;//0;// 
                        int minute = dateTimeDuration.Minutes;//0; //
                        int second = dateTimeDuration.Seconds;//0;// 
                        string Duration = DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriod(Years, Months, Days, hour, minute, second);
                        R["ProcessingDuration"] = Duration;
                        this.setDurationText(Duration);
                        //this.userControlDurationProcessing.textBox.Text = Duration;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setDurationText(string Duration)
        {
            try
            {
                this.userControlDurationProcessing.textBox.Text = Duration;

                //System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                //if (_PeriodDurationAsIsoPeriod)
                //{

                //}
                //else
                //{
                //}


                //if (R["ProcessingDate"].Equals(System.DBNull.Value) || !_PeriodDurationAsIsoPeriod)
                //{
                //    R["ProcessingDuration"] = Duration;
                //    this.userControlDurationProcessing.textBox.Text = Duration;
                //}
                //else
                //{
                //    System.DateTime DTfrom;
                //    System.DateTime DTto;
                //    if (System.DateTime.TryParse(R["ProcessingDate"].ToString(), out DTfrom) &&
                //        System.DateTime.TryParse(Duration, out DTto))
                //    {
                //        System.TimeSpan dateTimeDuration = DTto.Subtract(DTfrom);
                //        int Days = dateTimeDuration.Days;
                //        int Years = 0;
                //        int Months = 0;
                //        while (Days > 365)
                //        {
                //            Years++;
                //            Days = Days - 365;
                //        }
                //        while (Days > 30)
                //        {
                //            Months++;
                //            Days = Days - 30;
                //        }
                //        int hour = 0;// dateTimeDuration.Hours;
                //        int minute = 0; //dateTimeDuration.Minutes;
                //        int second = 0;// dateTimeDuration.Seconds;
                //        R["ProcessingDuration"] = Duration;
                //        this.userControlDurationProcessing.textBox.Text = Duration;
                //    }
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void labelProcessingDuration_Click(object sender, EventArgs e)
        {
        }

        private void buttonProcessingDurationType_Click(object sender, EventArgs e)
        {
            this.setPeriodDurationType();
        }


        bool _PeriodDurationAsIsoPeriod = true;
        private void setPeriodDurationType()
        {
            _PeriodDurationAsIsoPeriod = !_PeriodDurationAsIsoPeriod;
            this.userControlDurationProcessing.setUseIsoFormat(_PeriodDurationAsIsoPeriod);
            if(_PeriodDurationAsIsoPeriod)
            {
                this.toolTip.SetToolTip(this.buttonProcessingDurationType, "Save duration in ISO format for period. Click to change for date format");
                this.buttonProcessingDurationType.Image = DiversityCollection.Resource.ISO;
            }
            else
            {
                this.toolTip.SetToolTip(this.buttonProcessingDurationType, "Save duration as date. Click to change to ISO format for period");
                this.buttonProcessingDurationType.Image = DiversityCollection.Resource.ISOGrey;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R.BeginEdit();
            R.EndEdit();
            if ((R["ProcessingDuration"].Equals(System.DBNull.Value) || R["ProcessingDuration"].ToString().Length == 0) && !_PeriodDurationAsIsoPeriod)
            {
                R["ProcessingDuration"] = "-";
            }

            this.setDurationText(R["ProcessingDuration"].ToString());

            //this.dateTimePickerProcessingDuration_CloseUp(null, null);

            //string Duration = "";
            //if (!R["ProcessingDuration"].Equals(System.DBNull.Value))
            //{
            //    Duration = R["ProcessingDuration"].ToString();
            //}
            //if (Duration.Length > 0 && DiversityWorkbench.Forms.FormFunctions.IsIsoFormatPeriod(Duration))
            //{
            //}
            //else
            //{
            //    //if (_PeriodDurationAsIsoPeriod)
            //    //    R["ProcessingDuration"] = "";
            //    //else
            //    //    R["ProcessingDuration"] = "-";
            //}
        }

        //private void setDuration(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
        //        string Duration = this.userControlDurationProcessing.Duration;
        //        R["ProcessingDuration"] = Duration;
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        #endregion

        #endregion

        private System.Data.DataTable _dtProcessing;

        private System.Data.DataTable DtProcessing
        {
            get
            {
                if (this._dtProcessing == null)
                {
                    string SQL = "SELECT ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI " +
                    "FROM Processing ORDER BY DisplayText";
                    this._dtProcessing = new DataTable("Processing");
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(_dtProcessing);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                if (this._dtProcessing.Rows.Count == 0)
                {
                    string SQL = "SELECT ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI " +
                    "FROM Processing ORDER BY DisplayText";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(_dtProcessing);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _dtProcessing;
            }
        }

        #region Public functions
        
        public void SetProcessingDisplayText(string Processing)
        {
            this.groupBoxProcessing.Text = Processing;
            //this.comboBoxProcessingID.Text = Processing;
        }

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryProcessingResponsible, "ResponsibleAgentURI");
            this.setDate();
        }


        //public void setProcessingSource()
        //{
        //    try
        //    {
        //        if (this.comboBoxProcessingID.SelectedIndex > -1)
        //        {
        //            int Position = this.comboBoxProcessingID.SelectedIndex;
        //            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxProcessingID.SelectedItem;
        //            int ProcessingIDBefore = int.Parse(R["ProcessingID"].ToString());
        //            this._dtProcessing = null;
        //            int ProcessingIDAfter = int.Parse(this.DtProcessing.Rows[Position]["ProcessingID"].ToString());
        //            if (ProcessingIDAfter == ProcessingIDBefore)
        //            {
        //                this.comboBoxProcessingID.DataSource = null;
        //                this.comboBoxProcessingID.DataSource = this.DtProcessing;
        //                this.comboBoxProcessingID.DisplayMember = "DisplayText";
        //                this.comboBoxProcessingID.ValueMember = "ProcessingID";
        //                this.comboBoxProcessingID.SelectedIndex = Position;
        //            }
        //        }
        //        else
        //        {
        //            this._dtProcessing = null;
        //            this.comboBoxProcessingID.DataSource = null;
        //            this.comboBoxProcessingID.DataSource = this.DtProcessing;
        //            this.comboBoxProcessingID.DisplayMember = "DisplayText";
        //            this.comboBoxProcessingID.ValueMember = "ProcessingID";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        #endregion

    }
}
