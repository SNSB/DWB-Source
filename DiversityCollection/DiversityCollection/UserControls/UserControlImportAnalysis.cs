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
    public partial class UserControlImportAnalysis : UserControl, iUserControlImportInterface
    {

        #region Parameter

        private DiversityCollection.FormImportWizard _FormImportWizard;
        private DiversityCollection.iImportInterface _iImportInterface;
        private DiversityCollection.Import _Import;
        private string _StepKey;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabControl> _TabContols;

        #endregion

        #region Construction
        
        public UserControlImportAnalysis()
        {
            InitializeComponent();
        }

        #endregion

        #region Control events
        
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string CurrentPosition = this._Import.CurrentPosition;
            this.AddImportStep();
            this._ImportSteps.Last().Value.UserControlImportStep.IsCurrent = true;
            this._ImportSteps.Last().Value.setStepError();
            //this.buttonRemove.Visible = true;
            this._Import.ImportStepsShow();
            this._Import.CurrentPosition = CurrentPosition;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this.HideImportStep();
        }

        private void buttonRecover_Click(object sender, EventArgs e)
        {
            this.ShowHiddenImportSteps();
        }

        #endregion

        #region Interface

        public void Reset() { }

        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return null; }

        public void UpdateSelectionPanel() { }

        public string StepKey() { return this._StepKey; }

        public void initUserControl(DiversityCollection.iImportInterface I, DiversityCollection.Import_Step SuperiorImportStep)
        {
            this._iImportInterface = I;
            this._SuperiorImportStep = SuperiorImportStep;
            this._Import = I.getImport();
            this.AddImportStep();
            string Table = this._SuperiorImportStep.TableName();
            if (this._StepKey != null)
            {
                Table = this._ImportSteps[this._StepKey].TableName();
            }
            this.toolTip.SetToolTip(this.buttonAdd, "Add a new " + Table);
            this.toolTip.SetToolTip(this.buttonRemove, "Hide the selected " + Table);
            this.toolTip.SetToolTip(this.buttonRecover, "Show any hidden " + Table);
        }

        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
            // setting the superior controls
            if (ImportStep.SuperiorImportStep != null)
                ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep);

            // setting the controls for the step
            this.tabControlImportSteps.TabPages.Clear();
            string Key = ImportStep.StepKey();
            if (this._TabPages.ContainsKey(Key))
            {
                this.tabControlImportSteps.TabPages.Add(this._TabPages[Key]);
                if (this._TabContols.ContainsKey(Key))
                {
                    //this._TabContols[Key].TabPages.Clear();
                    //if (this._Import.CurrentPosition == Key && this._TaxonomicGroupTabPages.ContainsKey(Key))
                    //    this._TabContols[Key].TabPages.Add(this._TaxonomicGroupTabPages[Key]);
                }
            }
        }
        
        public void AddImportStep()
        {
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._SuperiorImportStep, (int)Import.ImportStepUnit.Analysis);
            try
            {
                string Title = "Analysis " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage TStep = new TabPage(Title);
                string StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepUnit.Analysis, this._SuperiorImportStep, NextImportStepNumber);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Analysis " + NextImportStepNumber.ToString(),
                    "Analysis " + NextImportStepNumber.ToString(),
                    StepKey,
                    "IdentificationUnitAnalysis",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    2,
                    (iUserControlImportInterface)this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Analysis),
                    null);

                this._StepKey = IS.StepKey();

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                if (this._TabContols == null)
                    this._TabContols = new SortedList<string, TabControl>();

                this.AddAnalysisSteps(TStep, IS);

                //IS.UserControlImportStep.IsCurrent = true;
                IS.setStepError();

                this._SuperiorImportStep.getUserControlImportInterface().UpdateSelectionPanel();
            }
            catch (System.Exception ex) { }
        }

        public void AddImportStep(string StepKey)
        {
            try
            {
                if (!DiversityCollection.Import.ImportSteps.ContainsKey(StepKey))
                {
                    if (this._SuperiorImportStep == null)
                    {
                        int OrganismNumber = DiversityCollection.Import_Step.StepKeyPartParallelNumber(StepKey, 0);
                        string OrganismKey = DiversityCollection.Import_Step.getImportStepKey(Import.ImportStep.Organism, OrganismNumber);

                        this._SuperiorImportStep = DiversityCollection.Import_Step.GetImportStep(OrganismKey);
                    }
                    this.AddImportStep();
                }
            }
            catch (System.Exception ex) { }
        }

        public void HideImportStep()
        {
            this._Import.HideCurrentImportStep();
        }
        public void ShowHiddenImportSteps()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in this._ImportSteps)
                IS.Value.IsVisible(true);
            this._Import.ImportStepsShow();
        }

        
        #endregion

        #region Auxillary functions

        private void AddAnalysisSteps(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)// System.Windows.Forms.TabPage T, string TableAlias)
        {
            string StepKey = ImportStep.StepKey();
            try
            {
                System.Windows.Forms.TabControl TabControlAnalysis = new System.Windows.Forms.TabControl();
                TabControlAnalysis.Dock = DockStyle.Fill;
                TabControlAnalysis.ImageList = this.tabControlAnalysis.ImageList;
                T.Controls.Add(TabControlAnalysis);
                this._TabContols.Add(ImportStep.StepKey(), TabControlAnalysis);

                System.Windows.Forms.TabPage TPanalysis = new TabPage("Analysis");
                TPanalysis.ImageIndex = this.tabPageAnalysis.ImageIndex;
                string Title = this.tabPageAnalysis.Text;
                TabControlAnalysis.TabPages.Add(TPanalysis);
                this.AddStepControlsAnalysis(TPanalysis, ImportStep);

                System.Windows.Forms.TabPage TPResponsible = new TabPage("Responsible, date, notes");
                TPResponsible.ImageIndex = this.tabPageResponsible.ImageIndex;
                this.AddStepControlsResponsible(TPResponsible, ImportStep);
                TabControlAnalysis.TabPages.Add(TPResponsible);

            }
            catch (System.Exception ex) { }

        }

        private void AddStepControlsAnalysis(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        {
            try
            {
                DiversityCollection.UserControls.UserControlImport_Column UCToolUsage = new UserControlImport_Column();
                DiversityCollection.Import_Column ICToolUsage = DiversityCollection.Import_Column.GetImportColumn(
                    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ToolUsage", UCToolUsage);
                ICToolUsage.CanBeTransformed = true;
                ICToolUsage.TypeOfEntry = Import_Column.EntryType.Text;
                ICToolUsage.TypeOfFixing = Import_Column.FixingType.None;
                ICToolUsage.TypeOfSource = Import_Column.SourceType.File;
                UCToolUsage.initUserControl(ICToolUsage, this._Import);
                UCToolUsage.Dock = DockStyle.Top;
                T.Controls.Add(UCToolUsage);

                DiversityCollection.UserControls.UserControlImport_Column UCnumber = new UserControlImport_Column();
                DiversityCollection.Import_Column ICnumber = DiversityCollection.Import_Column.GetImportColumn(
                    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AnalysisNumber", UCnumber);
                ICnumber.CanBeTransformed = true;
                ICnumber.MustSelect = true;
                ICnumber.TypeOfEntry = Import_Column.EntryType.Text;
                ICnumber.TypeOfFixing = Import_Column.FixingType.Schema;
                ICnumber.TypeOfSource = Import_Column.SourceType.Any;
                UCnumber.initUserControl(ICnumber, this._Import);
                UCnumber.Dock = DockStyle.Top;
                T.Controls.Add(UCnumber);

                DiversityCollection.UserControls.UserControlImport_Column UCResult = new UserControlImport_Column();
                DiversityCollection.Import_Column ICResult = DiversityCollection.Import_Column.GetImportColumn(
                    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AnalysisResult", UCResult);
                ICResult.CanBeTransformed = true;
                ICResult.MustSelect = true;
                ICResult.TypeOfEntry = Import_Column.EntryType.Text;
                ICResult.TypeOfFixing = Import_Column.FixingType.Schema;
                ICResult.TypeOfSource = Import_Column.SourceType.Any;
                UCResult.initUserControl(ICResult, this._Import);
                UCResult.Dock = DockStyle.Top;
                T.Controls.Add(UCResult);

                DiversityCollection.UserControls.UserControlImport_Column UCanalysis = new UserControlImport_Column();
                DiversityCollection.Import_Column ICanalysis = DiversityCollection.Import_Column.GetImportColumn
                    (ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AnalysisID", UCanalysis);
                ICanalysis.CanBeTransformed = true;
                ICanalysis.TypeOfEntry = Import_Column.EntryType.MandatoryList;
                ICanalysis.TypeOfFixing = Import_Column.FixingType.Schema;
                ICanalysis.TypeOfSource = Import_Column.SourceType.Interface;
                ICanalysis.MustSelect = true;
                ICanalysis.setLookupTable(DiversityCollection.LookupTable.DtAnalysisHierarchy, "HierarchyDisplayText", "AnalysisID");
                //ICanalysis.setLookupTable(this.DtAnalysis, "DisplayText", "AnalysisID");
                UCanalysis.initUserControl(ICanalysis, this._Import);
                UCanalysis.Dock = DockStyle.Top;
                T.Controls.Add(UCanalysis);

                DiversityCollection.Import_Column ICIdentificationUnitID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationUnitID", 1, null
                    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                ICIdentificationUnitID.IsSelected = true;
                ICIdentificationUnitID.CanBeTransformed = false;
                ICIdentificationUnitID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

                DiversityCollection.Import_Column ICCollectionSpecimenID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionSpecimenID", 1, null
                    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                ICCollectionSpecimenID.IsSelected = true;
                ICCollectionSpecimenID.CanBeTransformed = false;
                ICCollectionSpecimenID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

            }
            catch (System.Exception ex) { }
        }

        private void AddStepControlsResponsible(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        {
            DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
            DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Notes", UCNotes);
            ICNotes.CanBeTransformed = true;
            ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
            ICNotes.TypeOfFixing = Import_Column.FixingType.None;
            ICNotes.TypeOfSource = Import_Column.SourceType.Any;
            UCNotes.initUserControl(ICNotes, this._Import);
            UCNotes.Dock = DockStyle.Top;
            T.Controls.Add(UCNotes);

            DiversityCollection.UserControls.UserControlImport_Column UCDate = new UserControlImport_Column();
            DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AnalysisDate", UCNotes);
            ICDate.CanBeTransformed = true;
            ICDate.TypeOfEntry = Import_Column.EntryType.Date;
            ICDate.TypeOfFixing = Import_Column.FixingType.Import;
            ICDate.TypeOfSource = Import_Column.SourceType.Any;
            UCDate.initUserControl(ICDate, this._Import);
            UCDate.Dock = DockStyle.Top;
            T.Controls.Add(UCDate);

            DiversityCollection.UserControls.UserControlImport_Column UCResponsible = new UserControlImport_Column();
            DiversityCollection.Import_Column ICResponsible = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ResponsibleName", UCResponsible);
            ICResponsible.CanBeTransformed = true;
            //ICResponsible.MustSelect = true;
            ICResponsible.TypeOfEntry = Import_Column.EntryType.Text;
            ICResponsible.TypeOfFixing = Import_Column.FixingType.Schema;
            ICResponsible.TypeOfSource = Import_Column.SourceType.Any;
            UCResponsible.initUserControl(ICResponsible, this._Import);
            UCResponsible.Dock = DockStyle.Top;
            T.Controls.Add(UCResponsible);

        }

        private System.Data.DataTable _DtAnalysis;
        private System.Data.DataTable DtAnalysis
        {
            get
            {
                if (this._DtAnalysis == null)
                {
                    this._DtAnalysis = new System.Data.DataTable("Analysis");
                    string SQL = "SELECT NULL AS AnalysisID, NULL AS DisplayText UNION SELECT AnalysisID, DisplayText FROM Analysis ORDER BY Analysis";
                    System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        a.Fill(this._DtAnalysis);
                    }
                    catch { }
                }
                return this._DtAnalysis;
            }
        }
        
        
        #endregion

    }
}
