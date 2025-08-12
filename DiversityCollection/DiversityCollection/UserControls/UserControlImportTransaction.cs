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
    public partial class UserControlImportTransaction : UserControl, iUserControlImportInterface
    {

        #region Parameter

        private DiversityCollection.FormImportWizard _FormImportWizard;
        private DiversityCollection.iImportInterface _iImportInterface;
        private DiversityCollection.Import _Import;
        private string _StepKey;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabControl> _TabControls;

        #endregion

        #region Construction
        
        public UserControlImportTransaction()
        {
            InitializeComponent();
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
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._SuperiorImportStep, (int)Import.ImportStepStorage.Transaction);
            try
            {
                string Title = "Transaction " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage TStep = new TabPage(Title);
                string StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepStorage.Transaction, this._SuperiorImportStep, NextImportStepNumber);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Transaction " + NextImportStepNumber.ToString(),
                    "Transaction " + NextImportStepNumber.ToString(),
                    StepKey,
                    "CollectionSpecimenTransaction",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    2,
                    (iUserControlImportInterface)this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Transaction),
                    null);

                this._StepKey = IS.StepKey();

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                //if (this._TabControls == null)
                //    this._TabControls = new SortedList<string, TabControl>();

                this.AddTransactionSteps(TStep, IS);

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

        #region Auxillary functions

        private void AddTransactionSteps(TabPage TStep, Import_Step IS)
        {
            DiversityCollection.UserControls.UserControlImport_Column UCAccessionNumber = new UserControlImport_Column();
            DiversityCollection.Import_Column ICAccessionNumber = DiversityCollection.Import_Column.GetImportColumn(IS.StepKey(), IS.TableName(), IS.TableAlias(), "AccessionNumber", UCAccessionNumber);
            ICAccessionNumber.CanBeTransformed = true;
            ICAccessionNumber.TypeOfEntry = Import_Column.EntryType.Text;
            ICAccessionNumber.TypeOfFixing = Import_Column.FixingType.None;
            ICAccessionNumber.TypeOfSource = Import_Column.SourceType.Any;
            UCAccessionNumber.initUserControl(ICAccessionNumber, this._Import);
            UCAccessionNumber.Dock = DockStyle.Top;
            TStep.Controls.Add(UCAccessionNumber);

            DiversityCollection.UserControls.UserControlImport_Column UCTransaction = new UserControlImport_Column();
            DiversityCollection.Import_Column ICTransaction = DiversityCollection.Import_Column.GetImportColumn(IS.StepKey(), IS.TableName(), IS.TableAlias(), "TransactionID", UCTransaction);
            ICTransaction.CanBeTransformed = false;
            ICTransaction.MustSelect = true;
            ICTransaction.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICTransaction.TypeOfFixing = Import_Column.FixingType.Schema;
            ICTransaction.TypeOfSource = Import_Column.SourceType.Interface;
            ICTransaction.setLookupTable(DiversityCollection.LookupTable.DtTransaction, "TransactionTitle", "TransactionID");
            UCTransaction.initUserControl(ICTransaction, this._Import);
            UCTransaction.Dock = DockStyle.Top;
            TStep.Controls.Add(UCTransaction);

            UCTransaction.initUserControl(ICTransaction, this._Import);
        }

        #endregion
    }
}
