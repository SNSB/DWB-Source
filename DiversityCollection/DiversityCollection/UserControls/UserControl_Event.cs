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
    public partial class UserControl_Event : UserControl__Data
    {

        #region Construction

        public UserControl_Event(iMainForm MainForm, System.Windows.Forms.BindingSource Source, string HelpNameSpace)
            : base(MainForm, Source, HelpNameSpace)
        {
            InitializeComponent();
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        public UserControl_Event()
        {
            InitializeComponent();
        }

        #endregion

        #region public interface

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);

            this.groupBoxEvent.Text = "Collection event";
            if (this.NumberOfSpecimenInEvent() > 1)
            {
                this.groupBoxEvent.BackColor = System.Drawing.Color.Wheat;
                this.groupBoxEvent.Text = "Collection event.   Number of specimen:   " + this.NumberOfSpecimenInEvent().ToString() + " ";
            }
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventReference, "ReferenceURI");

            // Check dates
            this.CheckDateSequence();

            // Withholding
            this.SetDataWithholdingControl(this.comboBoxDataWithholdingReasonEvent, this.pictureBoxWithholdingReasonEvent);
            this.setCollectionDateWithholding();

        }

        public override void SetBackgroundColor(Color Color)
        {
            if (this.NumberOfSpecimenInEvent() > 1)
            {
                Color = System.Drawing.Color.Wheat;
                this.groupBoxEvent.Text = "Collection event.   Number of specimen:   " + this.NumberOfSpecimenInEvent().ToString() + " ";
            }
            base.SetBackgroundColor(Color);
        }

        public void setSource(System.Windows.Forms.BindingSource Source)
        {
            try
            {
                this._Source = Source;
                this.initControl();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public int EventID
        {
            get
            {
                int ID = -1;
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    int.TryParse(R["CollectionEventID"].ToString(), out ID);
                }
                return ID;
            }
        }

        #endregion

        #region Events

        private void initControl()
        {
            this.comboBoxDataWithholdingReasonEvent.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "DataWithholdingReason", true));
            this.comboBoxCollectionDateCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", _Source, "CollectionDateCategory", true));

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxDataWithholdingReasonEvent);
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);

            //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxCollectionDateCategory, "CollEventDateCategory_Enum", con, true, true, true);
            this._EnumComboBoxes = new Dictionary<ComboBox, string>();
            this._EnumComboBoxes.Add(this.comboBoxCollectionDateCategory, "CollEventDateCategory_Enum");
            this.InitLookupSources();

            this.textBoxEventNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "Notes", true));
            this.textBoxCountryCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "CountryCache", true));
            this.textBoxCollectorsEventNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "CollectorsEventNumber", true));
            this.textBoxEventLocality.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "LocalityDescription", true));
            this.textBoxLocalityDescriptionVerbatim.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "LocalityVerbatim", true));
            this.textBoxHabitatDesciption.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "HabitatDescription", true));
            this.textBoxCollectionTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "CollectionTime", true));
            this.textBoxCollectionTimeSpan.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "CollectionTimeSpan", true));
            this.textBoxCollectingMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", _Source, "CollectingMethod", true));
            this.textBoxEventReferenceDetails.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ReferenceDetails", true));

            DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventReference, R, "CollectionEvent", "ReferenceTitle", "ReferenceURI", this._Source);

            this.userControlDatePanelEventDate.setDataBindings(this._Source, "CollectionDay", "CollectionMonth", "CollectionYear", "CollectionDateSupplement");
            this.userControlDatePanelEventEnd.setDataBindings(this._Source, "CollectionEndDay", "CollectionEndMonth", "CollectionEndYear", "");

            this.userControlHierarchySelectorCollectionDateCategory.initHierarchyForEnum("CollEventDateCategory_Enum", "CollectionDateCategory", this.comboBoxCollectionDateCategory, this._Source);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCollectorsEventNumber);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCountryCache);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCollectingMethod);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void CheckDateSequence()
        {
            if (this._Source.Current != null)
            {
                bool WrongDateOrder = false;
                System.Data.DataRowView RL = (System.Data.DataRowView)this._Source.Current;
                if (RL != null)
                {
                    if (!RL["CollectionDate"].Equals(System.DBNull.Value) &&
                        !RL["CollectionEndDay"].Equals(System.DBNull.Value) &&
                        !RL["CollectionEndMonth"].Equals(System.DBNull.Value) &&
                        !RL["CollectionEndYear"].Equals(System.DBNull.Value))
                    {
                        System.DateTime DTStart;
                        System.DateTime DTEnd;
                        if (System.DateTime.TryParse(RL["CollectionDate"].ToString(), out DTStart) &&
                        System.DateTime.TryParse(RL["CollectionEndYear"].ToString() + "/" + RL["CollectionEndMonth"].ToString() + "/" + RL["CollectionEndDay"].ToString(), out DTEnd))
                        {
                            if (DTEnd < DTStart)
                            {
                                System.Windows.Forms.MessageBox.Show("The start date of the collection event is after the end date");
                                WrongDateOrder = true;
                            }
                        }
                    }
                }
                if (WrongDateOrder)
                {
                    this.userControlDatePanelEventDate.BackColor = System.Drawing.Color.Pink;
                    this.userControlDatePanelEventEnd.BackColor = System.Drawing.Color.Pink;
                }
                else
                {
                    this.userControlDatePanelEventDate.BackColor = System.Drawing.Color.White;
                    this.userControlDatePanelEventEnd.BackColor = System.Drawing.Color.White;
                }
            }
        }

        public override void InitLookupSources() { this.InitEnums(); }

        #region DataWithholding
        private void buttonCountryEditing_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = DiversityWorkbench.Gazetteer.Countries();
            if (dt.Rows.Count > 0 && dt.Columns.Count == 2)
            {
                string ValueColumn = dt.Columns[0].ColumnName;
                string DisplayColumn = dt.Columns[1].ColumnName;
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, DisplayColumn, ValueColumn, "Select a country", "Please select a country from the list", this.textBoxCountryCache.Text, false, true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.textBoxCountryCache.Text = f.SelectedValue;
                    this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"] = f.SelectedValue;
                }
            }
            else
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Edit the country", this.textBoxCountryCache.Text);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.textBoxCountryCache.Text = f.EditedText;
                    this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows[0]["CountryCache"] = f.EditedText;
                }
            }
        }

        private void comboBoxDataWithholdingReasonEvent_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT DataWithholdingReason FROM CollectionEvent ORDER BY DataWithholdingReason";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxDataWithholdingReasonEvent.DataSource = dt;
                this.comboBoxDataWithholdingReasonEvent.DisplayMember = "DataWithholdingReason";
                this.comboBoxDataWithholdingReasonEvent.ValueMember = "DataWithholdingReason";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxDataWithholdingReasonEvent_TextChanged(object sender, EventArgs e)
        {
            this.SetDataWithholdingControl(this.comboBoxDataWithholdingReasonEvent, this.pictureBoxWithholdingReasonEvent);
        }

        private void buttonWithholdingReasonCollectionDate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    System.Data.DataTable dt = new DataTable();
                    string Reason = "";
                    if (!R["DataWithholdingReasonDate"].Equals(System.DBNull.Value))
                        Reason = R["DataWithholdingReasonDate"].ToString();//DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    string SQL = "SELECT DISTINCT DataWithholdingReasonDate FROM CollectionEvent WHERE DataWithholdingReasonDate <> '' ORDER BY DataWithholdingReasonDate";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DataWithholdingReasonDate", "DataWithholdingReasonDate", "Withhold date", "Please enter the reason for withholding the collection date", Reason, false);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        R["DataWithholdingReasonDate"] = f.String;
                        this.setCollectionDateWithholding();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setCollectionDateWithholding()
        {
            try
            {
                string Reason = "";
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    if (!R["DataWithholdingReasonDate"].Equals(System.DBNull.Value) && R["DataWithholdingReasonDate"].ToString().Length > 0)
                    {
                        Reason = R["DataWithholdingReasonDate"].ToString();
                    }
                }
                string Tooltip = "";
                if (Reason.Length > 0)
                {
                    this.buttonWithholdingReasonCollectionDate.Image = this.imageListDataWithholding.Images[0];
                    Tooltip = "Reason for withholding the collection date: " + Reason;
                }
                else
                {
                    this.buttonWithholdingReasonCollectionDate.Image = this.imageListDataWithholding.Images[1];
                    Tooltip = "Collection date not withheld";
                }
                this.toolTip.SetToolTip(this.buttonWithholdingReasonCollectionDate, Tooltip);
            }
            catch (System.Exception ex)
            { }
        }

        #endregion

        #region Spliter and size
        private void UserControl_Event_SizeChanged(object sender, EventArgs e)
        {
            if (this.splitContainerOverviewEventData.SplitterDistance < _SplitterDistance && this.Height > 200)
                this.splitContainerOverviewEventData.SplitterDistance = _SplitterDistance;
        }

        private int _SplitterDistance = 150;

        private void splitContainerOverviewEventData_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this._SplitterDistance = this.splitContainerOverviewEventData.SplitterDistance;
            if (this._SplitterDistance < 144)
            {
                this._SplitterDistance = 144;
                this.splitContainerOverviewEventData.SplitterDistance = this._SplitterDistance;
            }
        }

        #endregion

        /// <summary>
        /// Number of specimen linked to the current CollectionEvent
        /// </summary>
        /// <returns>Specimen count</returns>
        private int NumberOfSpecimenInEvent()
        {
            int SpecimenInEvent = 1;
            string SqlCount = "SELECT COUNT(*) FROM CollectionSpecimen WHERE CollectionEventID = " + EventID.ToString();
            int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlCount), out SpecimenInEvent);
            return SpecimenInEvent;
        }

        private void textBoxCollectorsEventNumber_KeyDown(object sender, KeyEventArgs e)
        {
            //1
        }

        private void textBoxCollectorsEventNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //2
        }

        private void textBoxCollectorsEventNumber_KeyUp(object sender, KeyEventArgs e)
        {
            //3
        }

        #endregion

    }
}
