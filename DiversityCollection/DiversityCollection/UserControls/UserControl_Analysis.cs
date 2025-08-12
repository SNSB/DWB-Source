using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_Analysis : UserControl__Data
    {

        #region Construction

        public UserControl_Analysis(iMainForm MainForm, System.Windows.Forms.BindingSource Source, string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Control

        private void initControl()
        {
            try
            {
                this.comboBoxAnalysisResult.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "AnalysisResult", true));
                this.comboBoxAnalysisSpecimenPart.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "SpecimenPartID", true));

                this.textBoxAnalysisNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AnalysisNumber", true));
                this.textBoxAnalysisNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
                this.textBoxAnalysisDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AnalysisDate", true));
                this.textBoxAnalysisResult.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AnalysisResult", true));
                this.textBoxAnalysisURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ExternalAnalysisURI", true));

                this.userControlRichEditSequence.DataBindings.Add(new System.Windows.Forms.Binding("EditText", this._Source, "AnalysisResult", true));

                //UserControlSetting Responsible = new UserControlSetting("ResponsibleAgentURI", "ResponsibleName");
                //this.UserControlSettings.Add("Responsible", Responsible);

                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryAnalysisResponsible, (DiversityWorkbench.IWorkbenchUnit)A, "IdentificationUnitAnalysis", "ResponsibleName", "ResponsibleAgentURI", this._Source);

                //this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryAnalysisResponsible, (DiversityWorkbench.IWorkbenchUnit)A, this.UserControlSettings["Responsible"], this._Source);

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxAnalysisResult);
                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxAnalysisSpecimenPart);

                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxAnalysisNumber);
                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxAnalysisURI);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                this.CheckIfClientIsUpToDate();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Interface
        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.SetResultControls();
            this.setAnalysisPartList();

            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryAnalysisResponsible, "ResponsibleAgentURI");

            //this.setUserControlModuleRelatedEntrySources(this.UserControlSettings["Responsible"], ref this.userControlModuleRelatedEntryAnalysisResponsible);

            //System.Collections.Generic.List<string> Settings = new List<string>();
            //Settings.Add("ModuleSource");
            //Settings.Add("IdentificationUnitAnalysis");
            //Settings.Add("ResponsibleAgentURI");
            //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryAnalysisResponsible);

        }

        public void setTitle(string Title)
        {
            this.groupBoxAnalysis.Text = Title;
        }

        #endregion

        #region Events
        private void buttonAnalysisURIOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxAnalysisURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxAnalysisURI.Text = f.URL;
        }

        private void dateTimePickerAnalysisDate_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            // Markus 5.7.23 : Isodate
            string DateString = this.dateTimePickerAnalysisDate.Value.ToString("yyyy-MM-dd");
            //DateString = this.dateTimePickerAnalysisDate.Value.Year.ToString() + "/" +
            //    this.dateTimePickerAnalysisDate.Value.Month.ToString() + "/" +
            //    this.dateTimePickerAnalysisDate.Value.Day.ToString();
            R["AnalysisDate"] = DateString;
            this.textBoxAnalysisDate.Text = DateString;
        }

        private void buttonAnalysisOpen_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormAnalysis f;
            if (this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysis.Rows.Count > 0)
            {
                try
                {
                    System.Data.DataRow r = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysis.Rows[this._Source.Position];
                    int AnalysisID = int.Parse(r["AnalysisID"].ToString());
                    f = new Forms.FormAnalysis((int?)AnalysisID);
                    f.ShowDialog();
                    //if (f.DialogResult == DialogResult.OK)
                    //{
                    //    r["AnalysisID"] = f.ID;
                    //}
                    //this.fillAnalysisList();

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void textBoxAnalysisDate_Validating(object sender, CancelEventArgs e)
        {
            bool OK = true;
            if (this.textBoxAnalysisDate.Text.Length == 0)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                R.BeginEdit();
                R["AnalysisDate"] = System.DBNull.Value;
                R.EndEdit();
            }
        }

        private void setAnalysisPartList()
        {
            this.comboBoxAnalysisSpecimenPart.DataSource = null;
            this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart.Clear();
            System.Data.DataRow R;
            try
            {
                if (this._iMainForm.SelectedUnitHierarchyNode() != null)
                    R = (System.Data.DataRow)this._iMainForm.SelectedUnitHierarchyNode().Tag;
                else if (this._iMainForm.SelectedPartHierarchyNode() != null)
                    R = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                else
                    return;
                int UnitID = 0;
                if (R.Table.TableName != "IdentificationUnitAnalysis") return;
                if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
                {
                    System.Data.DataRow rNull = this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart.NewRow();
                    rNull["DisplayText"] = System.DBNull.Value;
                    rNull["SpecimenPartID"] = System.DBNull.Value;
                    this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart.Rows.Add(rNull);
                    foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow rU in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Rows)
                    {
                        if (rU.RowState != DataRowState.Deleted && rU.RowState != DataRowState.Detached && rU.IdentificationUnitID == UnitID)
                        {
                            int PartID = rU.SpecimenPartID;
                            string Display = "";
                            System.Data.DataRow[] sP =
                                this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select("SpecimenPartID = " + PartID.ToString());
                            if (sP.Length > 0)
                            {
                                if (!sP[0]["AccessionNumber"].Equals(System.DBNull.Value)) Display = sP[0]["AccessionNumber"].ToString() + " - ";
                                if (!sP[0]["PartSublabel"].Equals(System.DBNull.Value)) Display += sP[0]["PartSublabel"].ToString() + " - ";
                                if (!sP[0]["StorageLocation"].Equals(System.DBNull.Value)) Display += sP[0]["StorageLocation"].ToString() + " - ";
                                if (!sP[0]["MaterialCategory"].Equals(System.DBNull.Value)) Display += sP[0]["MaterialCategory"].ToString() + " - ";
                                System.Data.DataRow rNew = this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart.NewRow();
                                rNew["DisplayText"] = Display;
                                rNew["SpecimenPartID"] = PartID;
                                this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart.Rows.Add(rNew);
                            }
                        }
                    }
                }

                int SpecimenPartID = -1;
                int i = 0;
                if (int.TryParse(R["SpecimenPartID"].ToString(), out SpecimenPartID))
                {
                    for (i = 0; i < this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart.Rows.Count; i++)
                    {
                        if (this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart.Rows[i]["SpecimenPartID"].ToString() == SpecimenPartID.ToString())
                            break;
                    }
                }

                this.comboBoxAnalysisSpecimenPart.DataSource = this._iMainForm.DataSetCollectionSpecimen().AnalysisOfPart;
                this.comboBoxAnalysisSpecimenPart.DisplayMember = "DisplayText";
                this.comboBoxAnalysisSpecimenPart.ValueMember = "SpecimenPartID";

                this.comboBoxAnalysisSpecimenPart.SelectedIndex = i;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxAnalysisSpecimenPart_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R;
                if (this._iMainForm.SelectedUnitHierarchyNode() != null)
                    R = (System.Data.DataRow)this._iMainForm.SelectedUnitHierarchyNode().Tag;
                else if (this._iMainForm.SelectedPartHierarchyNode() != null)
                    R = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                else
                    return;
                R.EndEdit();
                R.BeginEdit();
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                if (RV[0].Equals(System.DBNull.Value))
                    R["SpecimenPartID"] = System.DBNull.Value;
                else
                    R["SpecimenPartID"] = int.Parse(RV["SpecimenPartID"].ToString());
                this.comboBoxAnalysisSpecimenPart.Text = RV["DisplayText"].ToString();
                R.EndEdit();
            }
            catch { }
        }

        private System.Data.DataTable _dtAnalysisResult;
        private System.Data.DataView _dvAnalysisResult;

        private void SetResultControls()
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                this._MeasurementUnit = DiversityCollection.LookupTable.AnalysisMeasurementUnit(R["AnalysisID"].ToString());
                if (this._MeasurementUnit != null && this._MeasurementUnit.Length > 0)
                {
                    this.labelAnalysisMeasurementUnit.Text = this._MeasurementUnit;
                    if (Analysis.TypeOfSequence(this._MeasurementUnit) == Analysis.SequenceType.None)
                        this.labelAnalysisMeasurementUnit.Visible = true;
                    else
                    {
                        this.labelAnalysisMeasurementUnit.Visible = false;
                        this.buttonEditSequence.Text = this._MeasurementUnit;
                    }
                }
                else
                {
                    this.labelAnalysisMeasurementUnit.Visible = false;
                    this.labelAnalysisMeasurementUnit.Text = "";
                }
                bool ListRestriction = DiversityCollection.LookupTable.AnalysisResultsAreRestrictedToList(int.Parse(R["AnalysisID"].ToString()));
                if (ListRestriction)
                {
                    this.textBoxAnalysisResult.Visible = false;
                    //this.textBoxAnalysisResult.Dock = DockStyle.Right;
                    if (this.comboBoxAnalysisResult.DataSource == null)// || 1 == 1) // Test
                    {
                        this._dtAnalysisResult = DiversityCollection.LookupTable.AnalysisResults();
                        string Filter = "AnalysisID = " + R["AnalysisID"].ToString() + " OR AnalysisID IS NULL";
                        this._dvAnalysisResult = new DataView(this._dtAnalysisResult, Filter, "", DataViewRowState.Unchanged);
                        this.comboBoxAnalysisResult.DataSource = this._dvAnalysisResult;
                        this.comboBoxAnalysisResult.DisplayMember = "DisplayText";
                        this.comboBoxAnalysisResult.ValueMember = "AnalysisResult";
                    }
                    else
                    {
                        this._dvAnalysisResult.RowFilter = "AnalysisID = " + R["AnalysisID"].ToString() + " OR AnalysisID IS NULL";
                    }
                    if (!R["AnalysisResult"].Equals(System.DBNull.Value))
                    {
                        int i = 0;
                        foreach (System.Data.DataRow AR in this._dtAnalysisResult.Rows)
                        {
                            if (!AR["AnalysisID"].Equals(System.DBNull.Value) &&
                                AR["AnalysisID"].ToString().Length > 0 &&
                                AR["AnalysisID"].ToString() != R["AnalysisID"].ToString())
                                continue;
                            if (AR["AnalysisResult"].ToString() == R["AnalysisResult"].ToString())
                            {
                                break;
                            }
                            i++;
                        }
                        if (i < this.comboBoxAnalysisResult.Items.Count)
                            this.comboBoxAnalysisResult.SelectedIndex = i;
                    }
                    //this.comboBoxAnalysisResult.Dock = DockStyle.Fill;
                    this.comboBoxAnalysisResult.Visible = true;
                }
                else
                {
                    this.comboBoxAnalysisResult.Visible = false;
                    if (Analysis.TypeOfSequence(this._MeasurementUnit) != Analysis.SequenceType.None)
                    {
                        this.buttonEditSequence.Visible = true;
                        this.userControlRichEditSequence.Visible = true;
                        this.userControlRichEditSequence.Height = this.Height - 150;
                        this.textBoxAnalysisResult.Visible = false;
                        this.initSequenceEditing();
                    }
                    else
                    {
                        this.buttonEditSequence.Visible = false;
                        this.userControlRichEditSequence.Visible = false;
                        this.textBoxAnalysisResult.Height = this.Height - 150;
                        this.textBoxAnalysisResult.Visible = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #endregion

        #region Sequence editing

        private string _MeasurementUnit;

        private System.Collections.Generic.Dictionary<string, string> _SequenceSymbols = DiversityWorkbench.MolecularSequence.Nucleotides();
        private System.Collections.Generic.Dictionary<string, string> _AmbigiousSymbols = DiversityWorkbench.MolecularSequence.AmbigiousNucleotides();
        private void initSequenceEditing()
        {
            Analysis.SequenceType T = Analysis.TypeOfSequence(this._MeasurementUnit);
            switch(T)
            {
                case Analysis.SequenceType.Nucleotide:
                    this._SequenceSymbols = DiversityWorkbench.MolecularSequence.Nucleotides();
                    this._AmbigiousSymbols = DiversityWorkbench.MolecularSequence.AmbigiousNucleotides();
                    this.userControlRichEditSequence.SetControl(T.ToString(), 1, "-", true, this._SequenceSymbols, this._AmbigiousSymbols) ;
                    break;
                case Analysis.SequenceType.Protein:
                    this._SequenceSymbols = DiversityWorkbench.MolecularSequence.AminoAcids(3);
                    this._AmbigiousSymbols = DiversityWorkbench.MolecularSequence.AmbigiousAminoAcids(3);
                    this.userControlRichEditSequence.SetControl(T.ToString(), 3, "-", true, this._SequenceSymbols, this._AmbigiousSymbols);
                    break;
            }
            this.toolTip.SetToolTip(this.buttonEditSequence, "Edit the " + this._MeasurementUnit + " in a separate window");
        }

        private void buttonEditSequence_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            DiversityWorkbench.Forms.FormRichEdit fe = new DiversityWorkbench.Forms.FormRichEdit("Edit sequence", R["AnalysisResult"].ToString(), "Nucleotide", 1, "-", false, _SequenceSymbols, this._AmbigiousSymbols);
            fe.ShowDialog();
            if (fe.DialogResult == DialogResult.OK)
            {
                R["AnalysisResult"] = fe.EditedText;
                this.userControlRichEditSequence.EditText = R["AnalysisResult"].ToString();
            }
        }

        //private Analysis.SequenceType TypeOfSequence()
        //{
        //    Analysis.SequenceType sequence = Analysis.SequenceType.None;
        //    if (this._MeasurementUnit != null && this._MeasurementUnit == "DNA")
        //        sequence = Analysis.SequenceType.Nucleotide;
        //    return sequence;
        //}

        //private enum SequenceType { None, Nucleotide, Protein }

        #endregion


        #region Public interface

        public void setAnalysisTitle(string Title)
        {
            this.groupBoxAnalysis.Text = Title;
        }

        #endregion

    }
}
