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
    public partial class UserControlImportSpecimenRelation : UserControl, iUserControlImportInterface
    {

        #region Parameter

        private DiversityCollection.Import _Import;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        private DiversityCollection.iImportInterface _iImportInterface;
        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        private string _StepKey;

        #endregion

        #region Construction
       
        public UserControlImportSpecimenRelation()
        {
            InitializeComponent();
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
            this._Import = I.getImport();
            this._SuperiorImportStep = SuperiorImportStep;
            this.AddImportStep();

            this.splitContainer.SplitterDistance = 40;

            this.toolTip.SetToolTip(this.buttonAdd, "Add a new " + this._SuperiorImportStep.TableName());
            this.toolTip.SetToolTip(this.buttonRemove, "Hide the selected " + this._SuperiorImportStep.TableName());
            this.toolTip.SetToolTip(this.buttonRecover, "Show any hidden " + this._SuperiorImportStep.TableName());
        }

        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
            this.tabControl.TabPages.Clear();
            string Key = ImportStep.StepKey();
            if (this._TabPages.ContainsKey(Key) && ImportStep.IsVisible())
                this.tabControl.TabPages.Add(this._TabPages[Key]);
            if (this.tabControl.TabPages.Count == 0 && ImportStep.IsVisible())
            {
                string FirstChildKey = DiversityCollection.Import.getImportStepKeyFirstChild(ImportStep);
                this.tabControl.TabPages.Add(this._TabPages[FirstChildKey]);
            }
            if (ImportStep.SuperiorImportStep != null)
                ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep);
        }

        public void AddImportStep()
        {
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(null, (int)DiversityCollection.Import.ImportStep.Relation);
            try
            {
                string Title = "Relation " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage T = new TabPage(Title);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Relation " + NextImportStepNumber.ToString(),
                    "Relation " + NextImportStepNumber.ToString(),
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Relation, NextImportStepNumber),
                    "CollectionSpecimenRelation",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    1,
                    (iUserControlImportInterface)this,
                    null,
                    this.panelSelection);

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                this.AddStepControls(T, IS);//TableAlias, NextImportStepNumber);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), T);

                IS.setStepError();
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
            this._Import.HideCurrentImportStep();//.HideImportStep(DiversityCollection.Import.ImportStep.Collector);
            this.showStepControls(DiversityCollection.Import.ImportSteps[this._Import.CurrentPosition]);
        }
        public void ShowHiddenImportSteps()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in this._ImportSteps)
                IS.Value.IsVisible(true);
            this._Import.ImportStepsShow();
        }

        #endregion

        #region Control events

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string CurrentPosition = this._Import.CurrentPosition;
            this.AddImportStep();
            //this.buttonRemove.Visible = true;
            this._Import.ImportStepsShow();
            if (CurrentPosition.Length > 0)
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

        #region Auxillary functions

        private void AddStepControls(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)// string TableAlias, int NextImportStepNumber)
        {
            string StepKey = ImportStep.StepKey();//DiversityCollection.Import.getImportStepKey(Import.ImportStep.Collector, this._Import.LastStepNumber(DiversityCollection.Import.ImportStep.Collector));

            DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
            DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionSpecimenRelation", ImportStep.TableAlias(), "Notes", UCNotes);
            ICNotes.CanBeTransformed = true;
            ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
            ICNotes.TypeOfFixing = Import_Column.FixingType.None;
            ICNotes.TypeOfSource = Import_Column.SourceType.Any;
            UCNotes.initUserControl(ICNotes, this._Import);
            UCNotes.Dock = DockStyle.Top;
            T.Controls.Add(UCNotes);

            DiversityCollection.UserControls.UserControlImport_Column UCRelatedSpecimenCollectionID = new UserControlImport_Column();
            DiversityCollection.Import_Column ICRelatedSpecimenCollectionID = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionSpecimenRelation", ImportStep.TableAlias(), "RelatedSpecimenCollectionID", UCRelatedSpecimenCollectionID);
            ICRelatedSpecimenCollectionID.CanBeTransformed = false;
            ICRelatedSpecimenCollectionID.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICRelatedSpecimenCollectionID.TypeOfFixing = Import_Column.FixingType.Schema;
            ICRelatedSpecimenCollectionID.TypeOfSource = Import_Column.SourceType.Interface;
            ICRelatedSpecimenCollectionID.setLookupTable(DiversityCollection.LookupTable.DtCollectionWithHierarchy, "DisplayText", "CollectionID");
            UCRelatedSpecimenCollectionID.initUserControl(ICRelatedSpecimenCollectionID, this._Import);
            UCRelatedSpecimenCollectionID.Dock = DockStyle.Top;
            T.Controls.Add(UCRelatedSpecimenCollectionID);

            DiversityCollection.UserControls.UserControlImport_Column UCRelationType = new UserControlImport_Column();
            DiversityCollection.Import_Column ICRelationType = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionSpecimenRelation", ImportStep.TableAlias(), "RelationType", UCRelationType);
            ICRelationType.CanBeTransformed = true;
            ICRelationType.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICRelationType.TypeOfFixing = Import_Column.FixingType.Schema;
            ICRelationType.TypeOfSource = Import_Column.SourceType.Any;
            ICRelationType.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollSpecimenRelationType_Enum", false, true, true), "DisplayText", "Code");
            UCRelationType.initUserControl(ICRelationType, this._Import);
            UCRelationType.Dock = DockStyle.Top;
            T.Controls.Add(UCRelationType);

            DiversityCollection.UserControls.UserControlImport_Column UCRelatedSpecimenDescription = new UserControlImport_Column();
            DiversityCollection.Import_Column ICRelatedSpecimenDescription = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionSpecimenRelation", ImportStep.TableAlias(), "RelatedSpecimenDescription", UCRelatedSpecimenDescription);
            ICRelatedSpecimenDescription.CanBeTransformed = true;
            ICRelatedSpecimenDescription.MustSelect = false;
            ICRelatedSpecimenDescription.TypeOfEntry = Import_Column.EntryType.Text;
            ICRelatedSpecimenDescription.TypeOfFixing = Import_Column.FixingType.Schema;
            ICRelatedSpecimenDescription.TypeOfSource = Import_Column.SourceType.File;
            UCRelatedSpecimenDescription.initUserControl(ICRelatedSpecimenDescription, this._Import);
            UCRelatedSpecimenDescription.Dock = DockStyle.Top;
            T.Controls.Add(UCRelatedSpecimenDescription);

            DiversityCollection.UserControls.UserControlImport_Column UCRelatedSpecimenDisplayText = new UserControlImport_Column();
            DiversityCollection.Import_Column ICRelatedSpecimenDisplayText = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionSpecimenRelation", ImportStep.TableAlias(), "RelatedSpecimenDisplayText", UCRelatedSpecimenDisplayText);
            ICRelatedSpecimenDisplayText.CanBeTransformed = true;
            ICRelatedSpecimenDisplayText.MustSelect = true;
            ICRelatedSpecimenDisplayText.TypeOfEntry = Import_Column.EntryType.Text;
            ICRelatedSpecimenDisplayText.TypeOfFixing = Import_Column.FixingType.Schema;
            ICRelatedSpecimenDisplayText.TypeOfSource = Import_Column.SourceType.File;
            UCRelatedSpecimenDisplayText.initUserControl(ICRelatedSpecimenDisplayText, this._Import);
            UCRelatedSpecimenDisplayText.Dock = DockStyle.Top;
            T.Controls.Add(UCRelatedSpecimenDisplayText);

            DiversityCollection.UserControls.UserControlImport_Column UCRelatedSpecimenURI = new UserControlImport_Column();
            DiversityCollection.Import_Column ICRelatedSpecimenURI = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionSpecimenRelation", ImportStep.TableAlias(), "RelatedSpecimenURI", UCRelatedSpecimenURI);
            ICRelatedSpecimenURI.CanBeTransformed = true;
            ICRelatedSpecimenURI.MustSelect = true;
            ICRelatedSpecimenURI.TypeOfEntry = Import_Column.EntryType.Text;
            ICRelatedSpecimenURI.TypeOfFixing = Import_Column.FixingType.Schema;
            ICRelatedSpecimenURI.TypeOfSource = Import_Column.SourceType.File;
            UCRelatedSpecimenURI.initUserControl(ICRelatedSpecimenURI, this._Import);
            UCRelatedSpecimenURI.Dock = DockStyle.Top;
            T.Controls.Add(UCRelatedSpecimenURI);

            UCRelatedSpecimenURI.setInterface();

            DiversityCollection.Import_Column ICisInternal =
                DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionSpecimenRelation", ImportStep.TableAlias(), "IsInternalRelationCache", 1, null
                , Import_Column.SourceType.Database, Import_Column.FixingType.Schema, Import_Column.EntryType.Database); // new Import_Column();
            ICisInternal.StepKey = StepKey;
            ICisInternal.IsSelected = true;
            ICisInternal.CanBeTransformed = false;
            ICisInternal.ValueIsFixed = true;
            ICisInternal.Value = "0";
        }

        #endregion        

    }
}
