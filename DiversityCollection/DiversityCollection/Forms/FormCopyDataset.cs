using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormCopyDataset : Form
    {
        #region Parameter

        public enum EventCopyMode { SameEvent, NewEvent, NoEvent, OnlyEvent, None }

        private DiversityCollection.Datasets.DataSetCollectionSpecimen _DsCollectionSpecimen;
        private EventCopyMode _EventCopyMode = EventCopyMode.None;
        private bool _CopyUnits = true;
        private string _OriAccNr;
        private string _AccNr;
        private bool _EventChecked = false;
        private System.Data.DataTable _dtProjects;
        private bool _PartAccessionNumberIncludedInGenerationOfAccessionNumber = false;

        #endregion

        #region Construction

        public FormCopyDataset(DiversityCollection.Datasets.DataSetCollectionSpecimen DsCollectionSpecimen)
        {
            InitializeComponent();
            this.treeViewOriginal.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewCopy.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewNewEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewNoEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewSameEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewOnlyEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this._DsCollectionSpecimen = DsCollectionSpecimen;
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            this.initForm();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            // deleting all surplus data in the dataset - leaving only one
            while (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 1)
            {
                this._DsCollectionSpecimen.CollectionSpecimen.Rows[1].Delete();
                this._DsCollectionSpecimen.AcceptChanges();
            }
            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenRow R = this._DsCollectionSpecimen.CollectionSpecimen.NewCollectionSpecimenRow();
            if (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
            {
                foreach (System.Data.DataColumn C in this._DsCollectionSpecimen.CollectionSpecimen.Columns)
                {
                    R[C] = this._DsCollectionSpecimen.CollectionSpecimen.Rows[0][C];
                }
                // Accession number present
                if (!R.IsAccessionNumberNull()
                    && !R.AccessionNumber.Equals(System.DBNull.Value) 
                    && R.AccessionNumber.Length > 0)
                {
                    this._OriAccNr = R.AccessionNumber;
                    if (this._OriAccNr.Length > 0)
                    {
                        this._AccNr = this.getNextFreeAccessionNumber(this._OriAccNr, 1000);
                        this.Text = "Copy dataset of " + this._OriAccNr;
                    }
                    else
                        this.checkBoxAccNr.Checked = false;
                    if (this._AccNr.Length == 0)
                        this._AccNr = "Copy of " + _OriAccNr;
                    this.textBoxAccessionNumber.Text = _AccNr;
                    this.textBoxAccNrInitials.Text = _OriAccNr;
                    this.labelHeaderAccNr.Text = "The original dataset has the accession number " + _OriAccNr + ".";
                }
                else // Accession number missing
                {
                    this.labelHeaderAccNr.Text = "The original dataset does not have an accession number.";
                }

                R.CollectionSpecimenID = -1;
                R.AccessionNumber = this.textBoxAccessionNumber.Text;
                this._DsCollectionSpecimen.CollectionSpecimen.Rows.Add(R);
                this.initTrees();
                this.InitAutoCompletion();
            }
            this.CheckAll();
            if (this._DsCollectionSpecimen.CollectionEvent.Rows.Count > 0 && this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
            {
                this.userControlDialogPanel.buttonOK.Enabled = false;
                string sql = "SELECT COUNT(*) FROM CollectionSpecimen WHERE CollectionEventID = " + this._DsCollectionSpecimen.CollectionEvent.Rows[0]["CollectionEventID"].ToString();
                int i;
                this.buttonOnlyEvent.Enabled = int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql), out i) && i > 1;
            }
            tabControlMain.TabPages.Remove(this.tabPageMultiCopy);

            this.treeViewRelationFromOriginal.ExpandAll();
            this.treeViewRelationToOriginal.ExpandAll();

            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxRelationTypeFromOriginal, "CollSpecimenRelationType_Enum", DiversityWorkbench.Settings.Connection);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxRelationTypeToOriginal, "CollSpecimenRelationType_Enum", DiversityWorkbench.Settings.Connection);

            this._dtProjects = new DataTable();
            string SQL = "SELECT       P.Project, NULL, P.ProjectID " +
                "FROM            ProjectProxy P " +
                "where P.ProjectID not in ( " +
                "SELECT       C.ProjectID " +
                "FROM            CollectionProject AS C " +
                "WHERE        (C.CollectionSpecimenID = " + this._DsCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString() + ")) " +
                "ORDER BY P.Project ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(_dtProjects);
            foreach (System.Data.DataRow RProject in _dtProjects.Rows)
            {
                bool isSelected = false;
                if (!RProject[1].Equals(System.DBNull.Value)) isSelected = true;
                this.checkedListBoxProjects.Items.Add(RProject[0].ToString(), isSelected);
            }


            this.checkBoxIncludeAgent.Checked = FormCopyDataset._IncludeCollectionAgent;
            this.checkBoxIncludeAnalysis.Checked = FormCopyDataset._IncludeIdentificationUnitAnalysis;
            this.checkBoxIncludeAnalysisMethods.Checked = FormCopyDataset._IncludeIdentificationUnitAnalysisMethod;
            this.checkBoxIncludeAnnotations.Checked = FormCopyDataset._IncludeAnnotation;
            this.checkBoxIncludeEventImages.Checked = FormCopyDataset._IncludeCollectionEventImage;
            this.checkBoxIncludeEventMethod.Checked = FormCopyDataset._IncludeCollectionEventMethod;
            this.checkBoxIncludeExternalIdentifier.Checked = FormCopyDataset._IncludeExternalIdentifier;
            this.checkBoxIncludeIdentification.Checked = FormCopyDataset._IncludeIdentification;
            this.checkBoxIncludeImageProperties.Checked = FormCopyDataset._IncludeCollectionSpecimenImageProperty;
            this.checkBoxIncludeProcessing.Checked = FormCopyDataset._IncludeCollectionSpecimenProcessing;
            this.checkBoxIncludeProcessingMethods.Checked = FormCopyDataset._IncludeCollectionSpecimenProcessingMethod;
            this.checkBoxIncludeReferences.Checked = FormCopyDataset._IncludeCollectionSpecimenReference;
            this.checkBoxIncludeRelations.Checked = FormCopyDataset._IncludeCollectionSpecimenRelation;
            this.checkBoxIncludeSpecimenImages.Checked = FormCopyDataset._IncludeCollectionSpecimenImage;
            this.checkBoxIncludeTransactions.Checked = FormCopyDataset._IncludeCollectionSpecimenTransaction;
        }

        private void InitAutoCompletion()
        {
            string SQL = "SELECT MIN(AccessionNumber) AS AccessionNumber " +
                "FROM  CollectionSpecimen_Core " +
                "WHERE (LEN(AccessionNumber) > 1) " +
                "GROUP BY LEN(AccessionNumber), SUBSTRING(AccessionNumber, 1, 1) " +
                "ORDER BY AccessionNumber";
            System.Data.DataTable DT = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(DT);
            System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
            foreach (System.Data.DataRow R in DT.Rows)
                StringCollection.Add(R[0].ToString());

            this.textBoxAccNrInitials.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxAccNrInitials.AutoCompleteCustomSource = StringCollection;
            this.textBoxAccNrInitials.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void CheckAll()
        {
            if (this._EventChecked
                && (this.textBoxAccessionNumber.Text.Length > 0
                || !this.checkBoxAccNr.Checked))
                this.userControlDialogPanel.buttonOK.Enabled = true;
            else if (this._DsCollectionSpecimen.CollectionEvent.Rows.Count > 0 && this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                this.userControlDialogPanel.buttonOK.Enabled = false;
        }
        
        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Event

        private void initTrees()
        {
            if (this._DsCollectionSpecimen.CollectionEvent.Rows.Count > 0 && this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
            {
                this._EventChecked = false;
                this.treeViewCopy.Nodes.Clear();
                this.treeViewOriginal.Nodes.Clear();
                this.treeViewNewEvent.Nodes.Clear();
                this.treeViewNoEvent.Nodes.Clear();
                this.treeViewSameEvent.Nodes.Clear();
                this.treeViewOnlyEvent.Nodes.Clear();

                // Original
                DiversityCollection.HierarchyNode HNOriEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], true);
                this.treeViewOriginal.Nodes.Add(HNOriEvent);
                DiversityCollection.HierarchyNode HNOriSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);
                HNOriEvent.Nodes.Add(HNOriSpecimen);
                this.treeViewOriginal.ExpandAll();


                // Copy
                DiversityCollection.HierarchyNode HNCopyEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], false);
                this.treeViewCopy.Nodes.Add(HNCopyEvent);
                if (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 1)
                {
                    DiversityCollection.HierarchyNode HNCopySpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[1], false);
                    HNCopyEvent.Nodes.Add(HNCopySpecimen);
                }
                this.treeViewCopy.ExpandAll();

                // Same event
                this.InitEventTreeView(this.treeViewSameEvent, EventCopyMode.SameEvent);
                //DiversityCollection.HierarchyNode HNSameEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], true);
                //this.treeViewSameEvent.Nodes.Add(HNSameEvent);
                //DiversityCollection.HierarchyNode HNSameEventSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);
                //HNSameEvent.Nodes.Add(HNSameEventSpecimen);
                //if (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 1)
                //{
                //    DiversityCollection.HierarchyNode HNSameEventNewSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[1], false);
                //    HNSameEvent.Nodes.Add(HNSameEventNewSpecimen);
                //}
                //this.treeViewSameEvent.ExpandAll();


                // New event
                this.InitEventTreeView(this.treeViewNewEvent, EventCopyMode.NewEvent);
                //DiversityCollection.HierarchyNode HNOldEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], true);
                //this.treeViewNewEvent.Nodes.Add(HNOldEvent);
                //DiversityCollection.HierarchyNode HNNewEventSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);
                //HNOldEvent.Nodes.Add(HNNewEventSpecimen);
                //DiversityCollection.HierarchyNode HNNewEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], false);
                //this.treeViewNewEvent.Nodes.Add(HNNewEvent);
                //if (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 1)
                //{
                //    DiversityCollection.HierarchyNode HNNewEventNewSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[1], false);
                //    HNNewEvent.Nodes.Add(HNNewEventNewSpecimen);
                //}
                //this.treeViewNewEvent.ExpandAll();

                // No event
                this.InitEventTreeView(this.treeViewNoEvent, EventCopyMode.NoEvent);
                //DiversityCollection.HierarchyNode HNNoEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], true);
                //this.treeViewNoEvent.Nodes.Add(HNNoEvent);
                //DiversityCollection.HierarchyNode HNNoEventSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);
                //HNNoEvent.Nodes.Add(HNNoEventSpecimen);
                //if (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 1)
                //{
                //    DiversityCollection.HierarchyNode HNNoEventNewSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[1], false);
                //    this.treeViewNoEvent.Nodes.Add(HNNoEventNewSpecimen);
                //}
                //this.treeViewNoEvent.ExpandAll();

                // Only event
                this.InitEventTreeView(this.treeViewOnlyEvent, EventCopyMode.OnlyEvent);


                this.userControlDialogPanel.buttonOK.Enabled = false;
                this._EventCopyMode = EventCopyMode.None;
                //this._EventChecked = true;
                //this.setTreeEnabled();
                this.buttonNewEvent.Enabled = true;
                this.buttonNoEvent.Enabled = true;
                this.buttonSameEvent.Enabled = true;
            }
            else // no event present
            {
                this.tabControlMain.TabPages.Remove(this.tabPageEvent);
                this._EventCopyMode = EventCopyMode.NoEvent;
                this._EventChecked = true;
                this.userControlDialogPanel.buttonOK.Enabled = true;
            }
        }


        private void InitEventTreeView(System.Windows.Forms.TreeView treeView, EventCopyMode eventCopyMode)
        {
            //bool NewEvent = eventCopyMode == EventCopyMode.NewEvent || eventCopyMode == EventCopyMode.OnlyEvent;
            //bool NewSpecimen = eventCopyMode == EventCopyMode.NewEvent || eventCopyMode == EventCopyMode.SameEvent || eventCopyMode == EventCopyMode.NoEvent;

            DiversityCollection.HierarchyNode HN_OriEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], true);
            DiversityCollection.HierarchyNode HN_OriSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);

            DiversityCollection.HierarchyNode HN_NewEvent = new HierarchyNode(this._DsCollectionSpecimen.CollectionEvent.Rows[0], false);
            DiversityCollection.HierarchyNode HN_NewSpecimen = null;
            if (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 1)
            {
                HN_NewSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[1], false);
            }
            else
                HN_NewSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], false);

            treeView.Nodes.Add(HN_OriEvent);

            switch(eventCopyMode)
            {
                case EventCopyMode.NewEvent:
                    HN_OriEvent.Nodes.Add(HN_OriSpecimen);
                    treeView.Nodes.Add(HN_NewEvent);
                    HN_NewEvent.Nodes.Add(HN_NewSpecimen);
                    break;
                case EventCopyMode.NoEvent:
                    HN_OriEvent.Nodes.Add(HN_OriSpecimen);
                    treeView.Nodes.Add(HN_NewSpecimen);
                    break;
                case EventCopyMode.None:
                    break;
                case EventCopyMode.SameEvent:
                    HN_OriEvent.Nodes.Add(HN_OriSpecimen);
                    HN_OriEvent.Nodes.Add(HN_NewSpecimen);
                    break;
                case EventCopyMode.OnlyEvent:
                    treeView.Nodes.Add(HN_NewEvent);
                    HN_NewEvent.Nodes.Add(HN_OriSpecimen);
                    break;
            }

            //treeView.Nodes.Add(HN_NewEvent);

            treeView.ExpandAll();
        }

        public EventCopyMode ModeForEventCopy { get { return this._EventCopyMode; } }

        #region Click events for buttons setting the Event Copy type

        private void buttonSameEvent_Click(object sender, EventArgs e)
        {
            this.setEventMode(EventCopyMode.SameEvent);
            //if (!this.AccessionNumberIsNew(this.textBoxAccessionNumber.Text)) return;
            ////this.DialogResult = DialogResult.OK;
            //this._EventCopyMode = EventCopyMode.SameEvent;
            //this._EventChecked = true;
            //this.CheckAll();
            //this.setTreeEnabled();
        }

        private void buttonNewEvent_Click(object sender, EventArgs e)
        {
            this.setEventMode(EventCopyMode.NewEvent);
            //if (!this.AccessionNumberIsNew(this.textBoxAccessionNumber.Text)) return;
            ////this.DialogResult = DialogResult.OK;
            //this._EventCopyMode = EventCopyMode.NewEvent;
            //this._EventChecked = true;
            //this.CheckAll();
            //this.setTreeEnabled();
        }

        private void buttonNoEvent_Click(object sender, EventArgs e)
        {
            this.setEventMode(EventCopyMode.NoEvent);
            //if (!this.AccessionNumberIsNew(this.textBoxAccessionNumber.Text)) return;
            ////this.DialogResult = DialogResult.OK;
            //this._EventCopyMode = EventCopyMode.NoEvent;
            //this._EventChecked = true;
            //this.CheckAll();
            //this.setTreeEnabled();
        }

        private void buttonOnlyEvent_Click(object sender, EventArgs e)
        {
            this.setEventMode(EventCopyMode.OnlyEvent);
        }

        private void setEventMode(EventCopyMode eventCopyMode)
        {
            if (!this.AccessionNumberIsNew(this.textBoxAccessionNumber.Text)) return;
            this._EventCopyMode = eventCopyMode;
            this._EventChecked = true;
            this.CheckAll();
            this.setTreeEnabled();
            System.Collections.Generic.List<System.Windows.Forms.TabPage> pages = new List<TabPage>();
            pages.Add(this.tabPageAccNr);
            pages.Add(this.tabPageProjects);
            pages.Add(this.tabPageRelations);
            //pages.Add(this.tabPageIncludedData);
            pages.Add(this.tabPageMultiCopy);
            foreach(System.Windows.Forms.TabPage tabPage in pages)
            {
                if (eventCopyMode == EventCopyMode.OnlyEvent && this.tabControlMain.TabPages.Contains(tabPage))
                    tabControlMain.TabPages.Remove(tabPage);
                else if (eventCopyMode != EventCopyMode.OnlyEvent && !this.tabControlMain.TabPages.Contains(tabPage))
                    tabControlMain.TabPages.Add(tabPage);
            }
            foreach(System.Windows.Forms.Control C in this.tableLayoutPanelIncluded.Controls)
            {
                C.Visible = eventCopyMode != EventCopyMode.OnlyEvent || C.Name.IndexOf("Event") > -1;
            }
            //this.checkBoxIncludeAgent.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeAnalysis.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeAnalysisMethods.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeAnnotations.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeExternalIdentifier.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeIdentification.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeImageProperties.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeProcessing.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeProcessingMethods.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeReferences.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeRelations.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeSpecimenImages.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
            //this.checkBoxIncludeTransactions.Enabled = eventCopyMode != EventCopyMode.OnlyEvent;
        }

        private void setTreeEnabled()
        {
            this.treeViewNewEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewNoEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewSameEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewOnlyEvent.BackColor = System.Drawing.SystemColors.Control;
            if (this._EventCopyMode == EventCopyMode.NewEvent) this.treeViewNewEvent.BackColor = System.Drawing.SystemColors.Window;
            if (this._EventCopyMode == EventCopyMode.NoEvent) this.treeViewNoEvent.BackColor = System.Drawing.SystemColors.Window;
            if (this._EventCopyMode == EventCopyMode.SameEvent) this.treeViewSameEvent.BackColor = System.Drawing.SystemColors.Window;
            if (this._EventCopyMode == EventCopyMode.OnlyEvent) this.treeViewOnlyEvent.BackColor = System.Drawing.SystemColors.Window;
            this.userControlDialogPanel.buttonOK.Enabled = true;
        }

        private void setEventOptions()
        {
            if (this._EventCopyMode == EventCopyMode.NewEvent)
            {
                this.checkBoxIncludeEventMethod.Enabled = true;
                this.checkBoxIncludeEventImages.Enabled = true;
            }
            else
            {
                this.checkBoxIncludeEventMethod.Enabled = false;
                this.checkBoxIncludeEventImages.Enabled = false;
            }
        }

        #endregion
        
        #endregion

        #region AccessionNumber

        private void buttonAccessionNumberSearch_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormAccessionNumber f = new Forms.FormAccessionNumber(this.textBoxAccNrInitials.Text, false, this.checkBoxAccessionNumberSearchIncludePart.Checked);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string NextAccNr = f.AccessionNumber;
                this.SetPartAccessionNumberIncludedInGenerationOfAccessionNumber(f.IncludePartInSearch());
                if (NextAccNr.Length > 0)
                {
                    this.textBoxAccessionNumber.Text = NextAccNr;
                    this._DsCollectionSpecimen.CollectionSpecimen.Rows[1]["AccessionNumber"] = NextAccNr;
                }
                else
                    System.Windows.Forms.MessageBox.Show("No free accession number could be found");
            }
        }

        private string getNextFreeAccessionNumber(string AccNr, int Timeout)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string NextAccNr = "";
            string SQL = "SELECT [dbo].[NextFreeAccNumber] ('" + AccNr + "', 1, ";
            if (this.checkBoxAccessionNumberSearchIncludePart.Checked) SQL += "1";
            else SQL += "0";
            SQL += ")";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(Timeout));
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                NextAccNr = C.ExecuteScalar()?.ToString() ?? string.Empty;
            }
            catch(System.Exception ex)
            {

            }
            finally { this.Cursor = System.Windows.Forms.Cursors.Default; }
            con.Close();
            return NextAccNr;
        }

        private bool AccessionNumberIsNew(string AccessionNumber)
        {
            if (AccessionNumber.Length == 0) 
                return true;
            bool OK = true;
            string SQL = "SELECT COUNT(*) FROM CollectionSpecimen WHERE AccessionNumber = '" + this.textBoxAccessionNumber.Text + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                int i = 0;
                if (!int.TryParse(C.ExecuteScalar()?.ToString(), out i))
                    OK = false;
                if (i > 0) OK = false;
            }
            catch
            {
                OK = false;
            }
            con.Close();
            if (!OK) System.Windows.Forms.MessageBox.Show("The accession number " + this.textBoxAccessionNumber.Text + " is allready present");
            return OK;
        }
        
        public string AccessionNumber 
        { 
            get 
            {
                if (this.checkBoxAccNr.Checked) return this.textBoxAccessionNumber.Text;
                else return "";
            } 
        }

        public bool CreateAccessionNumbers { get { return this.checkBoxAccNr.Checked; } }

        private void textBoxAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            bool OK = false;
            if (this.textBoxAccessionNumber.Text.Length > 0)
            {
                if (this.AccessionNumberIsNew(this.textBoxAccessionNumber.Text))
                {
                    OK = true;
                    this.textBoxAccessionNumber.BackColor = System.Drawing.SystemColors.Window;
                    if (this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 1)
                        this._DsCollectionSpecimen.CollectionSpecimen.Rows[1]["AccessionNumber"] = this.textBoxAccessionNumber.Text;
                }
                else this.textBoxAccessionNumber.BackColor = System.Drawing.Color.Pink;
            }
            else 
                this.textBoxAccessionNumber.BackColor = System.Drawing.Color.Pink;
            this.buttonNewEvent.Enabled = OK;
            this.buttonNoEvent.Enabled = OK;
            this.buttonSameEvent.Enabled = OK;
            //if (OK)
            //    this.initTrees();
            this.CheckAll();
        }

        private void checkBoxAccNr_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckAll();
            //this.buttonAccessionNumberNext.Enabled = this.checkBoxAccNr.Checked;
            this.buttonAccessionNumberSearch.Enabled = this.checkBoxAccNr.Checked;
            this.textBoxAccNrInitials.Enabled = this.checkBoxAccNr.Checked;
            this.textBoxAccessionNumber.Enabled = this.checkBoxAccNr.Checked;
            this.labelAccessionNumber.Enabled = this.checkBoxAccNr.Checked;
            this.checkedListBoxMultiCopyAccNr.Visible = this.checkBoxAccNr.Checked;
            //this.checkedListBoxMultiCopyAccNr.Visible = true;
        }

        public bool PartAccessionNumberIncludedInGenerationOfAccessionNumber()
        { return this._PartAccessionNumberIncludedInGenerationOfAccessionNumber; }
        public void SetPartAccessionNumberIncludedInGenerationOfAccessionNumber(bool Include)
        {
            this._PartAccessionNumberIncludedInGenerationOfAccessionNumber = Include;
            this.AllowMultiCopy = !this._PartAccessionNumberIncludedInGenerationOfAccessionNumber;
        }

        private void checkBoxAccessionNumberSearchIncludePart_Click(object sender, EventArgs e)
        {
            this.textBoxAccessionNumber.Text = this.getNextFreeAccessionNumber(this._OriAccNr, 1000);
            this.SetPartAccessionNumberIncludedInGenerationOfAccessionNumber(this.checkBoxAccessionNumberSearchIncludePart.Checked);
            
        }

        #endregion

        #region Identification

        private void initIdentification()
        {
            if (this._DsCollectionSpecimen.Identification.Rows.Count > 0)
                this.checkBoxCopyIdentifactions.Visible = true;
            else
                this.checkBoxCopyIdentifactions.Visible = false;
        }

        public bool CopyUnits 
        { 
            get 
            {
                if (this.checkBoxCopyIdentifactions.Checked) return true;
                else return false;
                //return this._CopyUnits; 
            } 
        }
        
        #endregion

        #region Multi Copy

        public int NumberOfCopies
        {
            get
            {
                try
                {
                    return (int)this.numericUpDownNumberOfCopies.Value;
                }
                catch
                {
                    return 1;
                }
            }
        }

        public bool AllowMultiCopy
        {
            set
            {
                if (value)
                {
                    if (!this.tabControlMain.Contains(this.tabPageMultiCopy))
                        tabControlMain.TabPages.Add(this.tabPageMultiCopy);
                }
                else
                {
                    if (this.tabControlMain.Contains(this.tabPageMultiCopy))
                        tabControlMain.TabPages.Remove(this.tabPageMultiCopy);
                    this.numericUpDownNumberOfCopies.Value = 1;
                    this.checkedListBoxMultiCopyAccNr.Items.Clear();
                }
            }
        }

        private void numericUpDownNumberOfCopies_Leave(object sender, EventArgs e)
        {
            if (this.numericUpDownNumberOfCopies.Value > 1)
            {
                this.userControlDialogPanel.buttonOK.Enabled = false;
                this.CheckMultiCopyAccessionNumbers();
            }
        }

        private void CheckMultiCopyAccessionNumbers()
        {
            try
            {
                bool OK = false;
                this.checkedListBoxMultiCopyAccNr.Items.Clear();
                string NextAccNr = this.textBoxAccessionNumber.Text;
                for (int i = 0; i < (int)this.numericUpDownNumberOfCopies.Value; i++)
                {
                    if (i > this.checkedListBoxMultiCopyAccNr.Items.Count) 
                        break;
                    bool AccOk = (NextAccNr.Length > 0);
                    this.checkedListBoxMultiCopyAccNr.Items.Add(NextAccNr, AccOk);
                    NextAccNr = this.getNextFreeAccessionNumber(NextAccNr, 1000);
                }
                this.userControlDialogPanel.buttonOK.Enabled = true;
            }
            catch
            {
                this.userControlDialogPanel.buttonOK.Enabled = false;
            }
        }

        public System.Collections.Generic.List<string> AccessionNumbers
        {
            get
            {
                System.Collections.Generic.List<string> AccNr = new List<string>();
                if (this.CreateAccessionNumbers)
                {
                    foreach (System.Object o in this.checkedListBoxMultiCopyAccNr.CheckedItems)
                        AccNr.Add(o.ToString());
                    if (AccNr.Count == 0 && this.AccessionNumber.Length > 0)
                        AccNr.Add(this.AccessionNumber);
                }
                else
                {
                    for (int i = 0; i < this.NumberOfCopies; i++)
                        AccNr.Add("");
                }
                return AccNr;
            }
        }
        
        private void numericUpDownNumberOfCopies_ValueChanged(object sender, EventArgs e)
        {
            this.checkedListBoxMultiCopyAccNr.Enabled = false;
            this.buttonRequeryMultiCopyList.Enabled = true;
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderSize = 2;
        }

        private void buttonRequeryMultiCopyList_Click(object sender, EventArgs e)
        {
            if (this.textBoxAccessionNumber.Text.Length > 0)
                this.checkedListBoxMultiCopyAccNr.Enabled = true;
            else
                System.Windows.Forms.MessageBox.Show("Only available for valid accession numbers");
            this.buttonRequeryMultiCopyList.Enabled = false;
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderSize = 0;

        }

        #endregion

        #region Projects

        public System.Collections.Generic.List<int> SelectedAdditionalProjectIDs()
        {
            System.Collections.Generic.List<int> L = new List<int>();
            foreach (int i in this.checkedListBoxProjects.CheckedIndices)
            {
                L.Add(int.Parse(this._dtProjects.Rows[i][2].ToString()));
            }
            return L;
        }

        #endregion

        #region Relations

        public bool RelationAddToOriginal
        {
            get
            {
                return this.checkBoxRelationToOriginal.Checked;
            }
        }
        public bool RelationAddFromOriginal
        {
            get
            {
                return this.checkBoxRelationFromOriginal.Checked;
            }
        }
        public string RelationTypeFromOriginal
        {
            get
            {
                return this.comboBoxRelationTypeFromOriginal.SelectedValue.ToString();
            }
        }
        public string RelationTypeToOriginal
        {
            get
            {
                return this.comboBoxRelationTypeToOriginal.SelectedValue.ToString();
            }
        }

        #endregion

        #region Included tables

        private static bool _IncludeIdentification = true;
        private static bool _IncludeIdentificationUnitAnalysis = false;
        private static bool _IncludeIdentificationUnitAnalysisMethod = false;
        private static bool _IncludeCollectionSpecimenProcessing = false;
        private static bool _IncludeCollectionSpecimenProcessingMethod = false;
        private static bool _IncludeCollectionAgent = true;
        private static bool _IncludeCollectionSpecimenImage = false;
        private static bool _IncludeCollectionSpecimenImageProperty = false;
        private static bool _IncludeCollectionEventImage = false;
        private static bool _IncludeCollectionEventMethod = false;
        private static bool _IncludeCollectionSpecimenRelation = false;
        private static bool _IncludeCollectionSpecimenReference = false;
        private static bool _IncludeCollectionSpecimenTransaction = false;
        private static bool _IncludeExternalIdentifier = false;
        private static bool _IncludeAnnotation = false;

        public System.Collections.Generic.List<string> IncludedTableList()
        {
            FormCopyDataset._IncludeAnnotation = this.checkBoxIncludeAnnotations.Checked;
            FormCopyDataset._IncludeCollectionAgent = this.checkBoxIncludeAgent.Checked;
            FormCopyDataset._IncludeCollectionEventImage = this.checkBoxIncludeEventImages.Checked;
            FormCopyDataset._IncludeCollectionEventMethod = this.checkBoxIncludeEventMethod.Checked;
            FormCopyDataset._IncludeCollectionSpecimenImage = this.checkBoxIncludeSpecimenImages.Checked;
            FormCopyDataset._IncludeCollectionSpecimenImageProperty = this.checkBoxIncludeImageProperties.Checked;
            FormCopyDataset._IncludeCollectionSpecimenProcessing = this.checkBoxIncludeProcessing.Checked;
            FormCopyDataset._IncludeCollectionSpecimenProcessingMethod = this.checkBoxIncludeProcessingMethods.Checked;
            FormCopyDataset._IncludeCollectionSpecimenReference = this.checkBoxIncludeReferences.Checked;
            FormCopyDataset._IncludeCollectionSpecimenRelation = this.checkBoxIncludeRelations.Checked;
            FormCopyDataset._IncludeCollectionSpecimenTransaction = this.checkBoxIncludeTransactions.Checked;
            FormCopyDataset._IncludeExternalIdentifier = this.checkBoxIncludeExternalIdentifier.Checked;
            FormCopyDataset._IncludeIdentification = this.checkBoxIncludeIdentification.Checked;
            FormCopyDataset._IncludeIdentificationUnitAnalysis = this.checkBoxIncludeAnalysis.Checked;
            FormCopyDataset._IncludeIdentificationUnitAnalysisMethod = this.checkBoxIncludeAnalysisMethods.Checked;
            FormCopyDataset._IncludeCollectionSpecimenReference = this.checkBoxIncludeReferences.Checked;

            System.Collections.Generic.List<string> L = new List<string>();
            L.Add("IdentificationUnit");
            L.Add("CollectionSpecimenPart");
            if (this.checkBoxIncludeIdentification.Checked)
            {
                L.Add("Identification");
            }
            if (this.checkBoxIncludeAnalysis.Checked)
            {
                L.Add("IdentificationUnitAnalysis");
            }
            if (this.checkBoxIncludeAnalysisMethods.Checked)
            {
                L.Add("IdentificationUnitAnalysisMethod");
                L.Add("IdentificationUnitAnalysisMethodParameter");
            }
            if (this.checkBoxIncludeProcessing.Checked)
            {
                L.Add("CollectionSpecimenProcessing");
            }
            if (this.checkBoxIncludeProcessingMethods.Checked)
            {
                L.Add("CollectionSpecimenProcessingMethod");
                L.Add("CollectionSpecimenProcessingMethodParameter");
            }
            if (this.checkBoxIncludeAgent.Checked)
            {
                L.Add("CollectionAgent");
            }
            if (this.checkBoxIncludeSpecimenImages.Checked)
            {
                L.Add("CollectionSpecimenImage");
            }
            if (this.checkBoxIncludeImageProperties.Checked)
            {
                L.Add("CollectionSpecimenImageProperty");
            }
            if (this.checkBoxIncludeEventImages.Checked)
            {
                L.Add("CollectionEventImage");
            }
            if (this.checkBoxIncludeEventMethod.Checked)
            {
                L.Add("CollectionEventMethod");
            }
            if (this.checkBoxIncludeRelations.Checked)
            {
                L.Add("CollectionSpecimenRelation");
            }
            if (this.checkBoxIncludeReferences.Checked)
            {
                L.Add("CollectionSpecimenReference");
            }
            if (this.checkBoxIncludeTransactions.Checked)
            {
                L.Add("CollectionSpecimenTransaction");
            }
            if (this.checkBoxIncludeExternalIdentifier.Checked)
            {
                L.Add("ExternalIdentifier");
            }
            if (this.checkBoxIncludeAnnotations.Checked)
            {
                L.Add("Annotation");
            }
            return L;
        }

        public string IncludedTables()
        {
            string Tables = "|";
            foreach (string T in this.IncludedTableList())
            {
                Tables += T + "|";
            }
            return Tables;
        }
        
        private void checkBoxIncludeAnalysisMethods_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxIncludeAnalysisMethods.Checked)
                this.checkBoxIncludeAnalysis.Checked = true;
        }

        private void checkBoxIncludeProcessingMethods_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxIncludeProcessingMethods.Checked)
                this.checkBoxIncludeProcessing.Checked = true;
        }

        private void checkBoxIncludeImageProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxIncludeImageProperties.Checked)
                this.checkBoxIncludeSpecimenImages.Checked = true;
        }

        private void buttonIncludeAll_Click(object sender, EventArgs e)
        {
            this.setIncludedForAll(true);
        }

        private void buttonIncludeNone_Click(object sender, EventArgs e)
        {
            this.setIncludedForAll(false);
        }

        private void setIncludedForAll(bool IsIncluded)
        {
            this.checkBoxIncludeAnnotations.Checked = IsIncluded;
            this.checkBoxIncludeAgent.Checked = IsIncluded;
            this.checkBoxIncludeEventImages.Checked = IsIncluded;
            this.checkBoxIncludeEventMethod.Checked = IsIncluded;
            this.checkBoxIncludeSpecimenImages.Checked = IsIncluded;
            this.checkBoxIncludeImageProperties.Checked = IsIncluded;
            this.checkBoxIncludeProcessing.Checked = IsIncluded;
            this.checkBoxIncludeProcessingMethods.Checked = IsIncluded;
            this.checkBoxIncludeReferences.Checked = IsIncluded;
            this.checkBoxIncludeRelations.Checked = IsIncluded;
            this.checkBoxIncludeTransactions.Checked = IsIncluded;
            this.checkBoxIncludeExternalIdentifier.Checked = IsIncluded;
            this.checkBoxIncludeIdentification.Checked = IsIncluded;
            this.checkBoxIncludeAnalysis.Checked = IsIncluded;
            this.checkBoxIncludeAnalysisMethods.Checked = IsIncluded;
            this.checkBoxIncludeReferences.Checked = IsIncluded;
        }

        #endregion

    }
}