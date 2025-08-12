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
    public partial class UserControlImportProcessing : UserControl, iUserControlImportInterface
    {

        #region Parameter

        private DiversityCollection.FormImportWizard _FormImportWizard;
        private DiversityCollection.iImportInterface _iImportInterface;
        private DiversityCollection.Import _Import;
        private string _StepKey;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        //System.Collections.Generic.SortedList<string, System.Windows.Forms.TabControl> _TabControls;

        #endregion

        #region Construction

        public UserControlImportProcessing()
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

        public string StepKey() { return this._StepKey; }
        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return null; }
        public void UpdateSelectionPanel() { }
        public void initUserControl(DiversityCollection.iImportInterface I, DiversityCollection.Import_Step SuperiorImportStep)
        {
            this._iImportInterface = I;
            this._SuperiorImportStep = SuperiorImportStep;
            this._Import = I.getImport();
            this.AddImportStep();

            string Table = this._ImportSteps[this._StepKey].TableName();
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
            this.tabControlImportStep.TabPages.Clear();
            string Key = ImportStep.StepKey();
            if (this._TabPages.ContainsKey(Key))
            {
                this.tabControlImportStep.TabPages.Add(this._TabPages[Key]);
            }
        }

        public void AddImportStep()
        {
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._SuperiorImportStep, (int)Import.ImportStepStorage.Processing);
            try
            {
                string Title = "Processing " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage TStep = new TabPage(Title);
                string StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepStorage.Processing, this._SuperiorImportStep, NextImportStepNumber);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Processing " + NextImportStepNumber.ToString(),
                    "Processing " + NextImportStepNumber.ToString(),
                    StepKey,
                    "CollectionSpecimenProcessing",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    2,
                    (iUserControlImportInterface)this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Processing),
                    null);

                this._StepKey = IS.StepKey();

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                this.AddProcessingSteps(TStep, IS);

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
                    this.AddImportStep();
                }
            }
            catch (System.Exception ex) { }
        }

        public void HideImportStep()
        {
            this._Import.HideCurrentImportStep();//.HideImportStep(DiversityCollection.Import.ImportStep.Storage);
        }

        public void ShowHiddenImportSteps()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in this._ImportSteps)
                IS.Value.IsVisible(true);
            this._Import.ImportStepsShow();
        }

        #endregion

        #region Auxillary functions

        private void AddProcessingSteps(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)// System.Windows.Forms.TabPage T, string TableAlias)
        {
            string StepKey = ImportStep.StepKey();
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

                DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
                DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Notes", UCNotes);
                ICNotes.CanBeTransformed = true;
                ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
                ICNotes.TypeOfFixing = Import_Column.FixingType.None;
                ICNotes.TypeOfSource = Import_Column.SourceType.Any;
                UCNotes.initUserControl(ICNotes, this._Import);
                UCNotes.Dock = DockStyle.Top;
                T.Controls.Add(UCNotes);

                DiversityCollection.UserControls.UserControlImport_Column UCProcessingDuration = new UserControlImport_Column();
                DiversityCollection.Import_Column ICProcessingDuration = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ProcessingDuration", UCProcessingDuration);
                ICProcessingDuration.CanBeTransformed = true;
                ICProcessingDuration.TypeOfEntry = Import_Column.EntryType.Text;
                ICProcessingDuration.TypeOfFixing = Import_Column.FixingType.None;
                ICProcessingDuration.TypeOfSource = Import_Column.SourceType.Any;
                UCProcessingDuration.initUserControl(ICProcessingDuration, this._Import);
                UCProcessingDuration.Dock = DockStyle.Top;
                T.Controls.Add(UCProcessingDuration);

                DiversityCollection.UserControls.UserControlImport_Column UCProtocoll = new UserControlImport_Column();
                DiversityCollection.Import_Column ICProtocoll = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Protocoll", UCProtocoll);
                ICProtocoll.CanBeTransformed = true;
                ICProtocoll.TypeOfEntry = Import_Column.EntryType.Text;
                ICProtocoll.TypeOfFixing = Import_Column.FixingType.None;
                ICProtocoll.TypeOfSource = Import_Column.SourceType.Any;
                UCProtocoll.initUserControl(ICProtocoll, this._Import);
                UCProtocoll.Dock = DockStyle.Top;
                T.Controls.Add(UCProtocoll);

                DiversityCollection.UserControls.UserControlImport_Column UCResponsible = new UserControlImport_Column();
                DiversityCollection.Import_Column ICResponsible = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ResponsibleName", UCResponsible);
                ICResponsible.CanBeTransformed = true;
                ICResponsible.TypeOfFixing = Import_Column.FixingType.Schema;
                ICResponsible.TypeOfSource = Import_Column.SourceType.Any;
                UCResponsible.initUserControl(ICResponsible, this._Import);
                UCResponsible.Dock = DockStyle.Top;
                T.Controls.Add(UCResponsible);

                //DiversityCollection.UserControls.UserControlImport_Column UCProcessingDate = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICProcessingDate = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ProcessingDate", UCProcessingDate);
                //ICProcessingDate.CanBeTransformed = true;
                //ICProcessingDate.MustSelect = true;
                //ICProcessingDate.TypeOfEntry = Import_Column.EntryType.Date;
                //ICProcessingDate.TypeOfFixing = Import_Column.FixingType.Schema;
                //ICProcessingDate.TypeOfSource = Import_Column.SourceType.Any;
                //UCProcessingDate.initUserControl(ICProcessingDate, this._Import);
                //UCProcessingDate.Dock = DockStyle.Top;
                //T.Controls.Add(UCProcessingDate);

                DiversityCollection.UserControls.UserControlImport_Column UCProcessing = new UserControlImport_Column();
                DiversityCollection.Import_Column ICProcessing = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ProcessingID", UCProcessing);
                ICProcessing.CanBeTransformed = false;
                ICProcessing.MustSelect = true;
                ICProcessing.TypeOfEntry = Import_Column.EntryType.MandatoryList;
                ICProcessing.TypeOfFixing = Import_Column.FixingType.Schema;
                ICProcessing.TypeOfSource = Import_Column.SourceType.Interface;
                ICProcessing.setLookupTable(DiversityCollection.LookupTable.DtProcessingHierarchy, "HierarchyDisplayText", "ProcessingID");
                UCProcessing.initUserControl(ICProcessing, this._Import);
                UCProcessing.Dock = DockStyle.Top;
                T.Controls.Add(UCProcessing);

                DiversityCollection.Import_Column ICProcessingDate = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ProcessingDate", 1, null,
                     Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database);
                ICProcessingDate.CanBeTransformed = false;
                ICProcessingDate.MustSelect = true;
                ICProcessingDate.IsSelected = true;
                ICProcessingDate.ValueIsFixed = true;
                ICProcessingDate.Value = "getdate()";

                //DiversityCollection.Import_Column ICSpecimenPartID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "SpecimenPartID", 1, null
                //    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                //ICSpecimenPartID.IsSelected = true;
                //ICSpecimenPartID.CanBeTransformed = false;
                //ICSpecimenPartID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

                //DiversityCollection.Import_Column ICCollectionSpecimenID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionSpecimenID", 1, null
                //    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                //ICCollectionSpecimenID.IsSelected = true;
                //ICCollectionSpecimenID.CanBeTransformed = false;
                //ICCollectionSpecimenID.ParentTableAlias(this._SuperiorImportStep.TableAlias());
            }
            catch (System.Exception ex) { }
        }

        #endregion

    }
}
