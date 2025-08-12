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
    public partial class UserControlImportCollector : UserControl, iUserControlImportInterface
    {

        #region Parameter
        
        private System.DateTime _CollectorsSequence;
        private DiversityCollection.Import _Import;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        private DiversityCollection.iImportInterface _iImportInterface;
        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        private string _StepKey;
        #endregion

        #region Construction
        
        public UserControlImportCollector()
        {
            InitializeComponent();
        }

        #endregion

        #region Interface

        public void Reset() { }

        //public void initUserControl(DiversityCollection.FormImportWizard f, string TableAlias, string StepKey, DiversityCollection.Import Import)
        //{

        //    this._FormImportWizard = f;
        //    this._TableAlias = TableAlias;
        //    this._Import = Import;

        //    this.tabControlCollector.TabPages.Clear();
        //    this.AddItem(); ;

        //    this.splitContainer.SplitterDistance = 40;

        //}

        //public void initUserControl(DiversityCollection.iImportInterface I, System.Windows.Forms.TabPage ParentTabPage, DiversityCollection.Import_Step SuperiorImportStep)
        //{
        //    this._FormImportWizard = (DiversityCollection.FormImportWizard)I;
        //    this._Import = I.getImport();
        //    this._SuperiorImportStep = SuperiorImportStep;
        //    this.tabControlCollector.TabPages.Clear();
        //    this._ParentTabPage = ParentTabPage;
        //    this.tabControlCollector.Dock = DockStyle.Fill;
        //    this.AddItem();

        //    this.splitContainer.SplitterDistance = 40;
        //}

        public string StepKey() { return this._StepKey; }
        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return this.panelSelection; }

        public void UpdateSelectionPanel() {}

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
            this.tabControlCollector.TabPages.Clear();
            string Key = ImportStep.StepKey();
            if (this._TabPages.ContainsKey(Key) && ImportStep.IsVisible())
                this.tabControlCollector.TabPages.Add(this._TabPages[Key]);
            if (this.tabControlCollector.TabPages.Count == 0 && ImportStep.IsVisible())
            {
                string FirstChildKey = DiversityCollection.Import.getImportStepKeyFirstChild(ImportStep);
                this.tabControlCollector.TabPages.Add(this._TabPages[FirstChildKey]);
            }
            if (ImportStep.SuperiorImportStep != null)
                ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep);
        }

        public void AddImportStep()
        {
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(null, (int)DiversityCollection.Import.ImportStep.Collector);
            try
            {
                string Title = "Collector " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage T = new TabPage(Title);
                DiversityCollection.Import_Step Collector = DiversityCollection.Import_Step.GetImportStep(
                    "Collector " + NextImportStepNumber.ToString(),
                    "Collector " + NextImportStepNumber.ToString(),
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Collector, NextImportStepNumber),
                    "CollectionAgent",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    1,
                    (iUserControlImportInterface)this,
                    null,
                    this.panelSelection);

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(Collector.StepKey(), Collector);

                this.AddStepControls(T, Collector);//TableAlias, NextImportStepNumber);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(Collector.StepKey(), T);

                //Collector.UserControlImportStep.IsCurrent = true;
                Collector.setStepError();
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

            DiversityCollection.UserControls.UserControlImport_Column UCDataWithholdingReason = new UserControlImport_Column();
            DiversityCollection.Import_Column ICDataWithholdingReason = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionAgent", ImportStep.TableAlias(), "DataWithholdingReason", UCDataWithholdingReason);
            ICDataWithholdingReason.CanBeTransformed = true;
            ICDataWithholdingReason.TypeOfEntry = Import_Column.EntryType.Text;
            ICDataWithholdingReason.TypeOfFixing = Import_Column.FixingType.Schema;
            ICDataWithholdingReason.TypeOfSource = Import_Column.SourceType.Any;
            UCDataWithholdingReason.initUserControl(ICDataWithholdingReason, this._Import);
            UCDataWithholdingReason.Dock = DockStyle.Top;
            T.Controls.Add(UCDataWithholdingReason);

            DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
            DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionAgent", ImportStep.TableAlias(), "Notes", UCNotes);
            ICNotes.CanBeTransformed = true;
            ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
            ICNotes.TypeOfFixing = Import_Column.FixingType.None;
            ICNotes.TypeOfSource = Import_Column.SourceType.Any;
            UCNotes.initUserControl(ICNotes, this._Import);
            UCNotes.Dock = DockStyle.Top;
            T.Controls.Add(UCNotes);

            DiversityCollection.UserControls.UserControlImport_Column UCNumber = new UserControlImport_Column();
            DiversityCollection.Import_Column ICNumber = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionAgent", ImportStep.TableAlias(), "CollectorsNumber", UCNumber);
            ICNumber.CanBeTransformed = true;
            ICNumber.TypeOfEntry = Import_Column.EntryType.Text;
            ICNumber.TypeOfFixing = Import_Column.FixingType.None;
            ICNumber.TypeOfSource = Import_Column.SourceType.File;
            UCNumber.initUserControl(ICNumber, this._Import);
            UCNumber.Dock = DockStyle.Top;
            T.Controls.Add(UCNumber);

            DiversityCollection.UserControls.UserControlImport_Column UColl = new UserControlImport_Column();
            DiversityCollection.Import_Column ICCollector = DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionAgent", ImportStep.TableAlias(), "CollectorsName", UColl);
            ICCollector.CanBeTransformed = true;
            ICCollector.MustSelect = true;
            ICCollector.TypeOfEntry = Import_Column.EntryType.Text;
            ICCollector.TypeOfFixing = Import_Column.FixingType.Schema;
            ICCollector.TypeOfSource = Import_Column.SourceType.Any;
            UColl.initUserControl(ICCollector, this._Import);
            UColl.Dock = DockStyle.Top;
            T.Controls.Add(UColl);

            UColl.setInterface();

            this._CollectorsSequence = System.DateTime.Now;
            DiversityCollection.Import_Column ICSequence = 
                DiversityCollection.Import_Column.GetImportColumn(StepKey, "CollectionAgent", ImportStep.TableAlias(), "CollectorsSequence", 1, null
                ,Import_Column.SourceType.Database, Import_Column.FixingType.Schema, Import_Column.EntryType.Database); // new Import_Column();
            ICSequence.StepKey = StepKey;
            ICSequence.IsSelected = true;
            ICSequence.CanBeTransformed = false;
            ICSequence.ValueIsFixed = true;

            ICSequence.Value = "getdate() + " + ImportStep.StepParallelNumber().ToString();
            //ICSequence.Value = "CONVERT(DATETIME, '" + System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString() 
            //    + " " + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + ":" + System.DateTime.Now.Second.ToString() + "', 102)";
        }
        
        #endregion        


    }
}
