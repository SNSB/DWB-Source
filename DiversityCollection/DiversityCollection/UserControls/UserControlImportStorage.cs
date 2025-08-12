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
    public partial class UserControlImportStorage : UserControl, iUserControlImportInterface
    {

        #region Parameter

        private DiversityCollection.Import _Import;
        private string _StepKey;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        private DiversityCollection.iImportInterface _iImportInterface;

        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabControl> _TabContols;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesStorage;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesProcessing;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesTransaction;

        #endregion

        #region Construction

        public UserControlImportStorage()
        {
            InitializeComponent();
        }
        
        #endregion

        #region Interface

        public void Reset() 
        {
            foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvTP in this._TabPages)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabControl> kvTC in this._TabContols)
                {
                    kvTC.Value.TabPages.Clear();
                }
            }
            this._TabPages = null;
            this._TabContols = null;
            this.tabControlImportSteps.TabPages.Clear();
            this.panelSelectionStorage.Controls.Clear();

            this._ImportSteps = null;
            this._TabPagesProcessing = null;
            this._TabPagesStorage = null;
            this._TabPagesTransaction = null;


        }

        public string StepKey() { return this._StepKey; }
        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return this.panelSelectionStorage; }
        
        public void UpdateSelectionPanel() 
        {
            if (this.panelSelectionStorage.Controls.Count == 0)
                return;
            System.Collections.Generic.SortedDictionary<string, DiversityCollection.UserControls.UserControlImportSelector> Selectors = new SortedDictionary<string, UserControlImportSelector>();
            foreach (System.Object o in this.panelSelectionStorage.Controls)
            {
                if (o.GetType() == typeof(DiversityCollection.UserControls.UserControlImportSelector))
                {
                    DiversityCollection.UserControls.UserControlImportSelector S = (DiversityCollection.UserControls.UserControlImportSelector)o;
                    Selectors.Add(S.ImportSteps()[0].StepKey(), S);
                }
            }
            this.panelSelectionStorage.Controls.Clear();
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.UserControls.UserControlImportSelector> KV in Selectors)
                {
                    this.panelSelectionStorage.Controls.Add(KV.Value);
                    KV.Value.Dock = DockStyle.Top;
                    KV.Value.BringToFront();
                }
            }
        }

        public void initUserControl(DiversityCollection.iImportInterface I, DiversityCollection.Import_Step SuperiorImportStep)
        {
            this._iImportInterface = I;
            this._Import = I.getImport();
            this._SuperiorImportStep = SuperiorImportStep;
            this.AddImportStep();
            this.toolTip.SetToolTip(this.buttonAdd, "Add a new " + this._SuperiorImportStep.TableName());
            this.toolTip.SetToolTip(this.buttonRemove, "Hide the selected " + this._SuperiorImportStep.TableName());
            this.toolTip.SetToolTip(this.buttonRecover, "Show any hidden " + this._SuperiorImportStep.TableName());
        }

        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
            // setting the superior controls
            if (ImportStep.SuperiorImportStep != null)
                ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep.SuperiorImportStep);

            // setting the controls for the part
            this.tabControlImportSteps.TabPages.Clear();
            string Key = ImportStep.StepKey();
            bool ControlsAdded = false;
            foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvTP in this._TabPages)
            {
                if (Key.StartsWith(kvTP.Key))
                {
                    this.tabControlImportSteps.TabPages.Add(kvTP.Value);
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabControl> kvTC in this._TabContols)
                    {
                        if (Key.StartsWith(kvTC.Key))
                        {
                            kvTC.Value.TabPages.Clear();
                            if (Key == kvTC.Key && this._TabPagesStorage.ContainsKey(Key))
                            {
                                kvTC.Value.TabPages.Add(this._TabPagesStorage[Key]);
                                ControlsAdded = true;
                            }
                            else
                            {
                                if (ImportStep.StepDetailKey() == (int)DiversityCollection.Import.ImportStepStorage.Processing)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvI in this._TabPagesProcessing)
                                    {
                                        if (Key.StartsWith(kvI.Key))
                                        {
                                            kvTC.Value.TabPages.Add(kvI.Value);
                                            ControlsAdded = true;
                                            break;
                                        }
                                    }
                                }
                                else if (ImportStep.StepDetailKey() == (int)DiversityCollection.Import.ImportStepStorage.Transaction)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvA in this._TabPagesTransaction)
                                    {
                                        if (Key.StartsWith(kvA.Key))
                                        {
                                            kvTC.Value.TabPages.Add(kvA.Value);
                                            ControlsAdded = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        if (ControlsAdded)
                            break;
                    }
                    break;
                }
                if (ControlsAdded)
                    break;
            }
        }

        public void AddImportStep()
        {
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(null, (int)DiversityCollection.Import.ImportStep.Storage);
            try
            {
                // the tabpage for the organism
                string Title = "Part " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage TStep = new TabPage(Title);
                // the tabcontrol for the details
                System.Windows.Forms.TabControl TControlUnitDetails = new TabControl();
                TControlUnitDetails.Tag = Title;
                TControlUnitDetails.Dock = DockStyle.Fill;
                TStep.Controls.Add(TControlUnitDetails);
                string StepKey = DiversityCollection.Import.getImportStepKey(Import.ImportStep.Storage, NextImportStepNumber);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Part " + NextImportStepNumber.ToString(),
                    "Part " + NextImportStepNumber.ToString(),
                    StepKey,
                    "CollectionSpecimenPart",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    1,
                    (iUserControlImportInterface)this,
                    null,
                    this.panelSelectionStorage);

                System.Windows.Forms.TabPage TPageStorage = new TabPage("Storage");
                this.AddStepControls(TPageStorage, IS);
                if (this._TabPagesStorage == null)
                    this._TabPagesStorage = new SortedList<string, TabPage>();
                this._TabPagesStorage.Add(IS.StepKey(), TPageStorage);

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                if (this._TabContols == null)
                    this._TabContols = new SortedList<string, TabControl>();
                this._TabContols.Add(IS.StepKey(), TControlUnitDetails);

                System.Windows.Forms.TabPage TPageProcessing = new TabPage();
                TPageProcessing.ImageIndex = this.tabPageProcessing.ImageIndex;
                TPageProcessing.Text = this.tabPageProcessing.Text;
                if (this._TabPagesProcessing == null)
                    this._TabPagesProcessing = new SortedList<string, TabPage>();
                this._TabPagesProcessing.Add(IS.StepKey(), TPageProcessing);
                this.initProcessing(TPageProcessing, IS);

                System.Windows.Forms.TabPage TPageTransaction = new TabPage();
                TPageTransaction.ImageIndex = this.tabPageTransaction.ImageIndex;
                TPageTransaction.Text = this.tabPageTransaction.Text;
                if (this._TabPagesTransaction == null)
                    this._TabPagesTransaction = new SortedList<string, TabPage>();
                this._TabPagesTransaction.Add(IS.StepKey(), TPageTransaction);
                this.initTransaction(TPageTransaction, IS);

                //IS.UserControlImportStep.IsCurrent = true;
                IS.setStepError();
            }
            catch (System.Exception ex) { }

        }

        public void AddImportStep(string StepKey)
        {
            try
            {
                int PartNumber = DiversityCollection.Import_Step.StepKeyPartParallelNumber(StepKey, 0);
                string PartKey = DiversityCollection.Import_Step.getImportStepKey(Import.ImportStep.Storage, PartNumber);
                string FirstStepKey = DiversityCollection.Import_Step.getImportStepKey(StepKey, 1);
                DiversityCollection.Import_Step S = DiversityCollection.Import_Step.GetImportStep(FirstStepKey);
                if (!DiversityCollection.Import.ImportSteps.ContainsKey(PartKey))
                {
                    this.AddImportStep();
                }
                if (DiversityCollection.Import.ImportSteps.ContainsKey(PartKey)
                    && !DiversityCollection.Import.ImportSteps.ContainsKey(StepKey))
                {
                    DiversityCollection.Import.ImportStepStorage I = DiversityCollection.Import_Step.ImportStepStorageKey(StepKey);
                    switch (I)
                    {
                        case Import.ImportStepStorage.Processing:
                            if (S != null)
                                S.getUserControlImportInterface().AddImportStep(StepKey);
                            else
                                this.userControlImportProcessing.AddImportStep(StepKey);
                            break;
                        case Import.ImportStepStorage.Transaction:
                            if (S != null)
                                S.getUserControlImportInterface().AddImportStep(StepKey);
                            else
                                this.userControlImportTransaction.AddImportStep(StepKey);
                            break;
                    }
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

        #region Add and remove events

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string CurrentPosition = this._Import.CurrentPosition;
            this.AddImportStep();
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

        private void AddStepControls(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        {
            System.Windows.Forms.TabControl TDetails = new TabControl();
            System.Windows.Forms.TabPage TPartCollection = new TabPage(this.tabPageCollection.Text);
            System.Windows.Forms.TabPage TPartIdentifier = new TabPage(this.tabPageIdentifier.Text);
            System.Windows.Forms.TabPage TPartPreparation = new TabPage(this.tabPagePreparation.Text);
            this.AddStepControlsNumbers(TPartIdentifier, ImportStep);
            this.AddStepControlsCollection(TPartCollection, ImportStep);
            this.AddStepControlsPeparation(TPartPreparation, ImportStep);
            TDetails.TabPages.Add(TPartIdentifier);
            TDetails.TabPages.Add(TPartCollection);
            TDetails.TabPages.Add(TPartPreparation);
            T.Controls.Add(TDetails);
            TDetails.Dock = DockStyle.Fill;

            //DiversityCollection.UserControls.UserControlImport_Column UCResponsible = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICResponsible = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ResponsibleName", UCResponsible);
            //ICResponsible.CanBeTransformed = true;
            //ICResponsible.TypeOfEntry = Import_Column.EntryType.Text;
            //ICResponsible.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICResponsible.TypeOfSource = Import_Column.SourceType.Any;
            //UCResponsible.initUserControl(ICResponsible, this._Import);
            //UCResponsible.Dock = DockStyle.Top;
            //T.Controls.Add(UCResponsible);

            //DiversityCollection.UserControls.UserControlImport_Column UCPreparationMethod = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICPreparationMethod = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "PreparationMethod", UCPreparationMethod);
            //ICPreparationMethod.CanBeTransformed = true;
            //ICPreparationMethod.TypeOfEntry = Import_Column.EntryType.Text;
            //ICPreparationMethod.TypeOfFixing = Import_Column.FixingType.None;
            //ICPreparationMethod.TypeOfSource = Import_Column.SourceType.File;
            //UCPreparationMethod.initUserControl(ICPreparationMethod, this._Import);
            //UCPreparationMethod.Dock = DockStyle.Top;
            //T.Controls.Add(UCPreparationMethod);

            //DiversityCollection.UserControls.UserControlImport_Column UCCollectionID = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICCollectionID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionID", UCCollectionID);
            //ICCollectionID.CanBeTransformed = true;
            //ICCollectionID.MustSelect = true;
            //ICCollectionID.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            //ICCollectionID.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICCollectionID.TypeOfSource = Import_Column.SourceType.Interface;
            //ICCollectionID.setLookupTable(DiversityCollection.LookupTable.DtCollectionWithHierarchy, "DisplayText", "CollectionID");
            //UCCollectionID.initUserControl(ICCollectionID, this._Import);
            //UCCollectionID.Dock = DockStyle.Top;
            //T.Controls.Add(UCCollectionID);

            //DiversityCollection.UserControls.UserControlImport_Column UCMaterialCategory = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICMaterialCategory = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "MaterialCategory", UCMaterialCategory);
            //ICMaterialCategory.CanBeTransformed = true;
            //ICMaterialCategory.MustSelect = true;
            //ICMaterialCategory.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            //ICMaterialCategory.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICMaterialCategory.TypeOfSource = Import_Column.SourceType.File;
            //ICMaterialCategory.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollMaterialCategory_Enum", true, false, true), "DisplayText", "Code");
            //UCMaterialCategory.initUserControl(ICMaterialCategory, this._Import);
            //UCMaterialCategory.Dock = DockStyle.Top;
            //T.Controls.Add(UCMaterialCategory);

            //DiversityCollection.UserControls.UserControlImport_Column UCStorageContainer = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICStorageContainer = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "StorageContainer", UCStorageContainer);
            //ICStorageContainer.CanBeTransformed = true;
            //ICStorageContainer.TypeOfEntry = Import_Column.EntryType.Text;
            //ICStorageContainer.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICStorageContainer.TypeOfSource = Import_Column.SourceType.Any;
            //UCStorageContainer.initUserControl(ICStorageContainer, this._Import);
            //UCStorageContainer.Dock = DockStyle.Top;
            //T.Controls.Add(UCStorageContainer);

            //DiversityCollection.UserControls.UserControlImport_Column UCStock = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICStock = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Stock", UCStock);
            //ICStock.CanBeTransformed = true;
            //ICStock.TypeOfEntry = Import_Column.EntryType.Text;
            //ICStock.TypeOfFixing = Import_Column.FixingType.None;
            //ICStock.TypeOfSource = Import_Column.SourceType.File;
            //UCStock.initUserControl(ICStock, this._Import);
            //UCStock.Dock = DockStyle.Top;
            //T.Controls.Add(UCStock);

            //DiversityCollection.UserControls.UserControlImport_Column UCStockUnit = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICStockUnit = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "StockUnit", UCStockUnit);
            //ICStockUnit.CanBeTransformed = true;
            //ICStockUnit.TypeOfEntry = Import_Column.EntryType.Text;
            //ICStockUnit.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICStockUnit.TypeOfSource = Import_Column.SourceType.Any;
            //UCStockUnit.initUserControl(ICStockUnit, this._Import);
            //UCStockUnit.Dock = DockStyle.Top;
            //T.Controls.Add(UCStockUnit);

            //DiversityCollection.UserControls.UserControlImport_Column UCStatus = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICVernacularTerm = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Status", UCStatus);
            //ICVernacularTerm.CanBeTransformed = true;
            //ICVernacularTerm.TypeOfEntry = Import_Column.EntryType.Text;
            //ICVernacularTerm.TypeOfFixing = Import_Column.FixingType.None;
            //ICVernacularTerm.TypeOfSource = Import_Column.SourceType.File;
            //UCStatus.initUserControl(ICVernacularTerm, this._Import);
            //UCStatus.Dock = DockStyle.Top;
            //T.Controls.Add(UCStatus);

            //DiversityCollection.UserControls.UserControlImport_Column UCPreparationMethodID = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICPreparationMethodID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "PreparationMethodID", UCPreparationMethodID);
            //ICPreparationMethodID.CanBeTransformed = true;
            //ICPreparationMethodID.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            //ICPreparationMethodID.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICPreparationMethodID.TypeOfSource = Import_Column.SourceType.Interface;
            //ICPreparationMethodID.setLookupTable(DiversityCollection.LookupTable.DtProcessing, "DisplayText", "ProcessingID");
            //UCPreparationMethodID.initUserControl(ICPreparationMethodID, this._Import);
            //UCPreparationMethodID.Dock = DockStyle.Top;
            //T.Controls.Add(UCPreparationMethodID);

            //DiversityCollection.UserControls.UserControlImport_Column UCAccessionNumber = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICAccessionNumber = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AccessionNumber", UCAccessionNumber);
            //ICAccessionNumber.CanBeTransformed = true;
            //ICAccessionNumber.TypeOfEntry = Import_Column.EntryType.Text;
            //ICAccessionNumber.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICAccessionNumber.TypeOfSource = Import_Column.SourceType.Any;
            //UCAccessionNumber.initUserControl(ICAccessionNumber, this._Import);
            //UCAccessionNumber.Dock = DockStyle.Top;
            //T.Controls.Add(UCAccessionNumber);

            //DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Notes", UCNotes);
            //ICNotes.CanBeTransformed = true;
            //ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
            //ICNotes.TypeOfFixing = Import_Column.FixingType.None;
            //ICNotes.TypeOfSource = Import_Column.SourceType.Any;
            //UCNotes.initUserControl(ICNotes, this._Import);
            //UCNotes.Dock = DockStyle.Top;
            //T.Controls.Add(UCNotes);

            //DiversityCollection.UserControls.UserControlImport_Column UCPartSublabel = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICPartSublabel = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "PartSublabel", UCPartSublabel);
            //ICPartSublabel.CanBeTransformed = true;
            //ICPartSublabel.TypeOfEntry = Import_Column.EntryType.Text;
            //ICPartSublabel.TypeOfFixing = Import_Column.FixingType.None;
            //ICPartSublabel.TypeOfSource = Import_Column.SourceType.File;
            //UCPartSublabel.initUserControl(ICPartSublabel, this._Import);
            //UCPartSublabel.Dock = DockStyle.Top;
            //T.Controls.Add(UCPartSublabel);

            //DiversityCollection.UserControls.UserControlImport_Column UCStorageLocation = new UserControlImport_Column();
            //DiversityCollection.Import_Column ICStorageLocation = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "StorageLocation", UCStorageLocation);
            //ICStorageLocation.CanBeTransformed = true;
            //ICStorageLocation.MustSelect = true;
            //ICStorageLocation.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            //ICStorageLocation.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICStorageLocation.TypeOfSource = Import_Column.SourceType.Any;
            //UCStorageLocation.initUserControl(ICStorageLocation, this._Import);
            //UCStorageLocation.Dock = DockStyle.Top;
            //T.Controls.Add(UCStorageLocation);

            //UCStorageLocation.setInterface();

        }

        private void AddStepControlsPeparation(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        {
            DiversityCollection.UserControls.UserControlImport_Column UCStatus = new UserControlImport_Column();
            DiversityCollection.Import_Column ICStatus = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Status", UCStatus);
            ICStatus.CanBeTransformed = true;
            ICStatus.TypeOfEntry = Import_Column.EntryType.Text;
            ICStatus.TypeOfFixing = Import_Column.FixingType.None;
            ICStatus.TypeOfSource = Import_Column.SourceType.File;
            UCStatus.initUserControl(ICStatus, this._Import);
            UCStatus.Dock = DockStyle.Top;
            T.Controls.Add(UCStatus);

            DiversityCollection.UserControls.UserControlImport_Column UCResponsible = new UserControlImport_Column();
            DiversityCollection.Import_Column ICResponsible = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ResponsibleName", UCResponsible);
            ICResponsible.CanBeTransformed = true;
            ICResponsible.TypeOfEntry = Import_Column.EntryType.Text;
            ICResponsible.TypeOfFixing = Import_Column.FixingType.Schema;
            ICResponsible.TypeOfSource = Import_Column.SourceType.Any;
            UCResponsible.initUserControl(ICResponsible, this._Import);
            UCResponsible.Dock = DockStyle.Top;
            T.Controls.Add(UCResponsible);

            DiversityCollection.UserControls.UserControlImport_Column UCPreparationDate = new UserControlImport_Column();
            DiversityCollection.Import_Column ICPreparationDate = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "PreparationDate", UCPreparationDate);
            ICPreparationDate.CanBeTransformed = true;
            ICPreparationDate.TypeOfEntry = Import_Column.EntryType.Date;
            ICPreparationDate.TypeOfFixing = Import_Column.FixingType.None;
            ICPreparationDate.TypeOfSource = Import_Column.SourceType.File;
            UCPreparationDate.initUserControl(ICPreparationDate, this._Import);
            UCPreparationDate.Dock = DockStyle.Top;
            T.Controls.Add(UCPreparationDate);

            DiversityCollection.UserControls.UserControlImport_Column UCPreparationMethod = new UserControlImport_Column();
            DiversityCollection.Import_Column ICPreparationMethod = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "PreparationMethod", UCPreparationMethod);
            ICPreparationMethod.CanBeTransformed = true;
            ICPreparationMethod.TypeOfEntry = Import_Column.EntryType.Text;
            ICPreparationMethod.TypeOfFixing = Import_Column.FixingType.None;
            ICPreparationMethod.TypeOfSource = Import_Column.SourceType.File;
            UCPreparationMethod.initUserControl(ICPreparationMethod, this._Import);
            UCPreparationMethod.Dock = DockStyle.Top;
            T.Controls.Add(UCPreparationMethod);

            DiversityCollection.UserControls.UserControlImport_Column UCPreparationMethodID = new UserControlImport_Column();
            DiversityCollection.Import_Column ICPreparationMethodID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "PreparationMethodID", UCPreparationMethodID);
            ICPreparationMethodID.CanBeTransformed = true;
            ICPreparationMethodID.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICPreparationMethodID.TypeOfFixing = Import_Column.FixingType.Schema;
            ICPreparationMethodID.TypeOfSource = Import_Column.SourceType.Interface;
            ICPreparationMethodID.setLookupTable(DiversityCollection.LookupTable.DtProcessing, "DisplayText", "ProcessingID");
            UCPreparationMethodID.initUserControl(ICPreparationMethodID, this._Import);
            UCPreparationMethodID.Dock = DockStyle.Top;
            T.Controls.Add(UCPreparationMethodID);

            DiversityCollection.UserControls.UserControlImport_Column UCMaterialCategory = new UserControlImport_Column();
            DiversityCollection.Import_Column ICMaterialCategory = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "MaterialCategory", UCMaterialCategory);
            ICMaterialCategory.CanBeTransformed = true;
            ICMaterialCategory.MustSelect = true;
            ICMaterialCategory.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICMaterialCategory.TypeOfFixing = Import_Column.FixingType.Schema;
            ICMaterialCategory.TypeOfSource = Import_Column.SourceType.Any;
            ICMaterialCategory.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollMaterialCategory_Enum", true, false, true), "DisplayText", "Code");
            UCMaterialCategory.initUserControl(ICMaterialCategory, this._Import);
            UCMaterialCategory.Dock = DockStyle.Top;
            T.Controls.Add(UCMaterialCategory);
        }

        private void AddStepControlsCollection(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        {
            DiversityCollection.UserControls.UserControlImport_Column UCStorageContainer = new UserControlImport_Column();
            DiversityCollection.Import_Column ICStorageContainer = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "StorageContainer", UCStorageContainer);
            ICStorageContainer.CanBeTransformed = true;
            ICStorageContainer.TypeOfEntry = Import_Column.EntryType.Text;
            ICStorageContainer.TypeOfFixing = Import_Column.FixingType.Schema;
            ICStorageContainer.TypeOfSource = Import_Column.SourceType.Any;
            ICStorageContainer.MultiColumn = true;
            UCStorageContainer.initUserControl(ICStorageContainer, this._Import);
            UCStorageContainer.Dock = DockStyle.Top;
            T.Controls.Add(UCStorageContainer);

            DiversityCollection.UserControls.UserControlImport_Column UCStockUnit = new UserControlImport_Column();
            DiversityCollection.Import_Column ICStockUnit = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "StockUnit", UCStockUnit);
            ICStockUnit.CanBeTransformed = true;
            ICStockUnit.TypeOfEntry = Import_Column.EntryType.Text;
            ICStockUnit.TypeOfFixing = Import_Column.FixingType.Schema;
            ICStockUnit.TypeOfSource = Import_Column.SourceType.Any;
            UCStockUnit.initUserControl(ICStockUnit, this._Import);
            UCStockUnit.Dock = DockStyle.Top;
            T.Controls.Add(UCStockUnit);

            DiversityCollection.UserControls.UserControlImport_Column UCStock = new UserControlImport_Column();
            DiversityCollection.Import_Column ICStock = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Stock", UCStock);
            ICStock.CanBeTransformed = true;
            ICStock.TypeOfEntry = Import_Column.EntryType.Text;
            ICStock.TypeOfFixing = Import_Column.FixingType.None;
            ICStock.TypeOfSource = Import_Column.SourceType.File;
            UCStock.initUserControl(ICStock, this._Import);
            UCStock.Dock = DockStyle.Top;
            T.Controls.Add(UCStock);

            DiversityCollection.UserControls.UserControlImport_Column UCCollectionID = new UserControlImport_Column();
            DiversityCollection.Import_Column ICCollectionID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionID", UCCollectionID);
            ICCollectionID.CanBeTransformed = true;
            ICCollectionID.MustSelect = true;
            ICCollectionID.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICCollectionID.TypeOfFixing = Import_Column.FixingType.Schema;
            ICCollectionID.TypeOfSource = Import_Column.SourceType.Any;
            ICCollectionID.setLookupTable(DiversityCollection.LookupTable.DtCollectionWithHierarchy, "DisplayText", "CollectionID");
            UCCollectionID.initUserControl(ICCollectionID, this._Import);
            UCCollectionID.Dock = DockStyle.Top;
            T.Controls.Add(UCCollectionID);
        }

        private void AddStepControlsNumbers(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        {

            DiversityCollection.UserControls.UserControlImport_Column UCDataWitholdingReason = new UserControlImport_Column();
            DiversityCollection.Import_Column ICDataWitholdingReason = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "DataWitholdingReason", UCDataWitholdingReason);
            ICDataWitholdingReason.CanBeTransformed = true;
            ICDataWitholdingReason.TypeOfEntry = Import_Column.EntryType.Text;
            ICDataWitholdingReason.TypeOfFixing = Import_Column.FixingType.None;
            ICDataWitholdingReason.TypeOfSource = Import_Column.SourceType.Any;
            UCDataWitholdingReason.initUserControl(ICDataWitholdingReason, this._Import);
            UCDataWitholdingReason.Dock = DockStyle.Top;
            T.Controls.Add(UCDataWitholdingReason);

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

            DiversityCollection.UserControls.UserControlImport_Column UCPartSublabel = new UserControlImport_Column();
            DiversityCollection.Import_Column ICPartSublabel = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "PartSublabel", UCPartSublabel);
            ICPartSublabel.CanBeTransformed = true;
            ICPartSublabel.TypeOfEntry = Import_Column.EntryType.Text;
            ICPartSublabel.TypeOfFixing = Import_Column.FixingType.None;
            ICPartSublabel.TypeOfSource = Import_Column.SourceType.File;
            UCPartSublabel.initUserControl(ICPartSublabel, this._Import);
            UCPartSublabel.Dock = DockStyle.Top;
            T.Controls.Add(UCPartSublabel);

            DiversityCollection.UserControls.UserControlImport_Column UCAccessionNumber = new UserControlImport_Column();
            DiversityCollection.Import_Column ICAccessionNumber = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AccessionNumber", UCAccessionNumber);
            ICAccessionNumber.CanBeTransformed = true;
            ICAccessionNumber.TypeOfEntry = Import_Column.EntryType.Text;
            ICAccessionNumber.TypeOfFixing = Import_Column.FixingType.Schema;
            ICAccessionNumber.TypeOfSource = Import_Column.SourceType.Any;
            UCAccessionNumber.initUserControl(ICAccessionNumber, this._Import);
            UCAccessionNumber.Dock = DockStyle.Top;
            T.Controls.Add(UCAccessionNumber);

            DiversityCollection.UserControls.UserControlImport_Column UCStorageLocation = new UserControlImport_Column();
            DiversityCollection.Import_Column ICStorageLocation = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "StorageLocation", UCStorageLocation);
            ICStorageLocation.CanBeTransformed = true;
            ICStorageLocation.MultiColumn = true;
            ICStorageLocation.MustSelect = true;
            ICStorageLocation.TypeOfEntry = Import_Column.EntryType.Text;
            ICStorageLocation.TypeOfFixing = Import_Column.FixingType.Schema;
            ICStorageLocation.TypeOfSource = Import_Column.SourceType.Any;
            UCStorageLocation.initUserControl(ICStorageLocation, this._Import);
            UCStorageLocation.Dock = DockStyle.Top;
            T.Controls.Add(UCStorageLocation);

            //DiversityCollection.Import_Column ICCollectionSpecimenID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionSpecimenID", 1, null
            //    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
            //ICCollectionSpecimenID.IsSelected = true;
            //ICCollectionSpecimenID.CanBeTransformed = false;
            //ICCollectionSpecimenID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

            UCStorageLocation.setInterface();
        }
        
        #endregion

        #region Processing

        private void initProcessing(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step IS)
        {
            try
            {
                //this._SuperiorImportStep = IS;
                //T.Controls.Add(this.userControlImportProcessing);
                //this.userControlImportProcessing.initUserControl(this._iImportInterface, IS);
                //this.userControlImportProcessing.Dock = DockStyle.Fill;

                DiversityCollection.UserControls.UserControlImportProcessing UI = new UserControlImportProcessing();
                this._SuperiorImportStep = IS;
                T.Controls.Add(UI);
                UI.initUserControl(this._iImportInterface, IS);
                UI.Dock = DockStyle.Fill;
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Transaction

        private void initTransaction(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step IS)
        {
            try
            {
                //this._SuperiorImportStep = IS;
                //T.Controls.Add(this.userControlImportTransaction);
                //this.userControlImportTransaction.initUserControl(this._iImportInterface, IS);
                //this.userControlImportTransaction.Dock = DockStyle.Fill;

                DiversityCollection.UserControls.UserControlImportTransaction UI = new UserControlImportTransaction();
                this._SuperiorImportStep = IS;
                T.Controls.Add(UI);
                UI.initUserControl(this._iImportInterface, IS);
                UI.Dock = DockStyle.Fill;
            }
            catch (System.Exception ex) { }
        }

        #endregion

    }
}
