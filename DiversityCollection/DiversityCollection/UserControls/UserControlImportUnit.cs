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
    public partial class UserControlImportUnit : UserControl, iUserControlImportInterface
    {

        #region Parameter
        
        private DiversityCollection.Import _Import;
        private string _StepKey;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        private DiversityCollection.iImportInterface _iImportInterface;

        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabControl> _TabContols;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesTaxonomicGroup;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesIdentification;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesAnalysis;

        #endregion

        #region Construction
        
        public UserControlImportUnit()
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
            this.panelSelectionUnit.Controls.Clear();

            this._ImportSteps = null;
            this._TabPagesAnalysis = null;
            this._TabPagesIdentification = null;
            this._TabPagesTaxonomicGroup = null;

        }

        public string StepKey() { return this._StepKey; }
        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return this.panelSelectionUnit; }

        public void UpdateSelectionPanel()
        {
            if (this.panelSelectionUnit.Controls.Count == 0)
                return;
            System.Collections.Generic.SortedDictionary<string, DiversityCollection.UserControls.UserControlImportSelector> Selectors = new SortedDictionary<string, UserControlImportSelector>();
            foreach (System.Object o in this.panelSelectionUnit.Controls)
            {
                if (o.GetType() == typeof(DiversityCollection.UserControls.UserControlImportSelector))
                {
                    DiversityCollection.UserControls.UserControlImportSelector S = (DiversityCollection.UserControls.UserControlImportSelector)o;
                    Selectors.Add(S.ImportSteps()[0].StepKey(), S);
                }
            }
            this.panelSelectionUnit.Controls.Clear();
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.UserControls.UserControlImportSelector> KV in Selectors)
                {
                    this.panelSelectionUnit.Controls.Add(KV.Value);
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

            // setting the controls for the unit
            this.tabControlImportSteps.TabPages.Clear();
            string Key = ImportStep.StepKey();
            bool ControlsAdded = false;
            if (this._TabPages == null)
                return;
            foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvTP in this._TabPages)
            {
                if (Key.StartsWith(kvTP.Key))
                {
                    this.tabControlImportSteps.TabPages.Add(kvTP.Value);
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabControl> kvTC in this._TabContols)
                    {
                        if (Key.StartsWith(kvTC.Key))
                        {
                            // clear the tabcontrol of the unit before adding the tabpage of the current data
                            kvTC.Value.TabPages.Clear();

                            // Adding the controls for the Unit if a unit was selected
                            if (Key == kvTC.Key && this._TabPagesTaxonomicGroup.ContainsKey(Key))
                            {
                                kvTC.Value.TabPages.Add(this._TabPagesTaxonomicGroup[Key]);
                                ControlsAdded = true;
                            }
                            else // Adding the controls of dependent data like identification of analysis if one of those where selected
                            {
                                if (ImportStep.StepDetailKey() == (int)DiversityCollection.Import.ImportStepUnit.Identification)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvI in this._TabPagesIdentification)
                                    {
                                        if (Key.StartsWith(kvI.Key))
                                        {
                                            kvTC.Value.TabPages.Add(kvI.Value);

                                            // zum Test
                                            //System.Windows.Forms.TabControl TC = (System.Windows.Forms.TabControl)kvTC.Value.Controls[0];
                                            //TC.TabPages.Add(kvI.Value);
                                            //System.Windows.Forms.TabPage TX = new TabPage(Key + " U");
                                            //kvTC.Value.TabPages.Add(TX);


                                            ControlsAdded = true;
                                            break;
                                        }
                                    }
                                }
                                else if (ImportStep.StepDetailKey() == (int)DiversityCollection.Import.ImportStepUnit.Analysis)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvA in this._TabPagesAnalysis)
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
            if (this.tabControlImportSteps.TabPages.Count == 0 && ImportStep.IsVisible())
            {
                string FirstChildKey = DiversityCollection.Import.getImportStepKeyFirstChild(ImportStep);
                this.tabControlImportSteps.TabPages.Add(this._TabPages[FirstChildKey]);
            }

        }
        
        public void AddImportStep()
        {
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(null, (int)DiversityCollection.Import.ImportStep.Organism);
            try
            {
                // the tabpage for the organism
                string Title = "Organism " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage TStep = new TabPage(Title);
                // the tabcontrol for the details
                System.Windows.Forms.TabControl TControlUnitDetails = new TabControl();
                TControlUnitDetails.Tag = Title;
                TControlUnitDetails.Dock = DockStyle.Fill;
                TStep.Controls.Add(TControlUnitDetails);
                string StepKey = DiversityCollection.Import.getImportStepKey(Import.ImportStep.Organism, NextImportStepNumber);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Organism " + NextImportStepNumber.ToString(),
                    "Organism " + NextImportStepNumber.ToString(),
                    StepKey,
                    "IdentificationUnit",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    1,
                    (iUserControlImportInterface)this,
                    null,
                    this.panelSelectionUnit);

                // nur kontrolle
                this._StepKey = StepKey;

                System.Windows.Forms.TabPage TPageTaxonomicGroup = new TabPage("Common informations");
                this.AddStepControls(TPageTaxonomicGroup, IS);
                if (this._TabPagesTaxonomicGroup == null)
                    this._TabPagesTaxonomicGroup = new SortedList<string, TabPage>();
                this._TabPagesTaxonomicGroup.Add(IS.StepKey(), TPageTaxonomicGroup);

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                if (this._TabContols == null)
                    this._TabContols = new SortedList<string, TabControl>();
                this._TabContols.Add(IS.StepKey(), TControlUnitDetails);

                System.Windows.Forms.TabPage TPageIdentification = new TabPage();
                TPageIdentification.ImageIndex = this.tabPageIdentifications.ImageIndex;
                // Test
                TPageIdentification.Text = this.tabPageIdentifications.Text;// +" " + IS.StepKey();

                // alle Tab pages des User Controls - hier landen alle Identifications aller Units
                if (this._TabPagesIdentification == null)
                    this._TabPagesIdentification = new SortedList<string, TabPage>();
                this._TabPagesIdentification.Add(IS.StepKey(), TPageIdentification);
                this.initIdentification(TPageIdentification, IS);

                System.Windows.Forms.TabPage TPageAnalysis = new TabPage();
                TPageAnalysis.ImageIndex = this.tabPageAnalysis.ImageIndex;
                TPageAnalysis.Text = this.tabPageAnalysis.Text;
                if (this._TabPagesAnalysis == null)
                    this._TabPagesAnalysis = new SortedList<string, TabPage>();
                this._TabPagesAnalysis.Add(IS.StepKey(), TPageAnalysis);
                this.initAnalysis(TPageAnalysis, IS);

                //IS.UserControlImportStep.IsCurrent = true;
                IS.setStepError();
            }
            catch (System.Exception ex) { }

        }

        public void AddImportStep(string StepKey)
        {
            try
            {
                int OrganismNumber = DiversityCollection.Import_Step.StepKeyPartParallelNumber(StepKey, 0);
                string OrganismKey = DiversityCollection.Import_Step.getImportStepKey(Import.ImportStep.Organism, OrganismNumber);
                string FirstStepKey = DiversityCollection.Import_Step.getImportStepKey(StepKey, 1);
                DiversityCollection.Import_Step S = DiversityCollection.Import_Step.GetImportStep(FirstStepKey);
                if (!DiversityCollection.Import.ImportSteps.ContainsKey(OrganismKey))
                {
                    this.AddImportStep();
                }
                if (DiversityCollection.Import.ImportSteps.ContainsKey(OrganismKey)
                    && !DiversityCollection.Import.ImportSteps.ContainsKey(StepKey))
                {
                    DiversityCollection.Import.ImportStepUnit I = DiversityCollection.Import_Step.ImportStepUnitKey(StepKey);
                    switch(I)
                    {
                        case Import.ImportStepUnit.Identification:
                            if (S != null)
                                S.getUserControlImportInterface().AddImportStep(StepKey);
                            else
                                this.userControlImportIdentification.AddImportStep(StepKey);
                            break;
                        case Import.ImportStepUnit.Analysis:
                            if (S != null)
                                S.getUserControlImportInterface().AddImportStep(StepKey);
                            else
                                this.userControlImportAnalysis.AddImportStep(StepKey);
                            break;
                    }
                }

            }
            catch (System.Exception ex) { }

        }

        public void HideImportStep()
        {
            this._Import.HideCurrentImportStep();//.HideImportStep(DiversityCollection.Import.ImportStep.Organism);
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
        
        private void AddStepControls(System.Windows.Forms.TabPage TP, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        {
            try
            {
                System.Windows.Forms.TabControl TC = new TabControl();
                TP.Controls.Add(TC);
                TC.Dock = DockStyle.Fill;

                System.Windows.Forms.TabPage T = new TabPage("Taxonomy and numbers");
                System.Windows.Forms.TabPage T2 = new TabPage("Notes etc.");
                TC.TabPages.Add(T);
                TC.TabPages.Add(T2);
                //System.Windows.Forms.Panel T = new Panel();
                //TP.Controls.Add(T);
                //T.Dock = DockStyle.Fill;
                string StepKey = ImportStep.StepKey();// DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStep.Organism, null, NextStepNumber); //DiversityCollection.Import.getImportStepKey(Import.ImportStep.Organism, this._Import.LastStepNumber(DiversityCollection.Import.ImportStep.Organism));

                // RELATION

                if (!this._StepKey.EndsWith("01"))
                {
                    System.Windows.Forms.TabPage T3 = new TabPage("Relation");
                    DiversityCollection.UserControls.UserControlImport_Column UCColonisedSubstratePart = new UserControlImport_Column();
                    TC.TabPages.Add(T3);

                    DiversityCollection.Import_Column ICColonisedSubstratePart =
                    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "ColonisedSubstratePart", UCColonisedSubstratePart);
                    ICColonisedSubstratePart.CanBeTransformed = true;
                    ICColonisedSubstratePart.TypeOfEntry = Import_Column.EntryType.Text;
                    ICColonisedSubstratePart.TypeOfFixing = Import_Column.FixingType.Schema;
                    ICColonisedSubstratePart.TypeOfSource = Import_Column.SourceType.Any;
                    UCColonisedSubstratePart.initUserControl(ICColonisedSubstratePart, this._Import);
                    UCColonisedSubstratePart.Dock = DockStyle.Top;
                    T3.Controls.Add(UCColonisedSubstratePart);

                    DiversityCollection.UserControls.UserControlImport_Column UCRelationType = new UserControlImport_Column();
                    DiversityCollection.Import_Column ICRelationType = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "RelationType", UCRelationType);
                    ICRelationType.CanBeTransformed = true;
                    ICRelationType.MustSelect = false;
                    ICRelationType.TypeOfEntry = Import_Column.EntryType.MandatoryList;
                    ICRelationType.TypeOfFixing = Import_Column.FixingType.Schema;
                    ICRelationType.TypeOfSource = Import_Column.SourceType.Any;
                    ICRelationType.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollUnitRelationType_Enum", true, true, true), "DisplayText", "Code");
                    ICRelationType.DisplayColumn = "DisplayText";
                    ICRelationType.ValueColumn = "Code";
                    UCRelationType.initUserControl(ICRelationType, this._Import);
                    UCRelationType.Dock = DockStyle.Top;
                    T3.Controls.Add(UCRelationType);

                    DiversityCollection.UserControls.UserControlImport_Column UCRelatedUnitID = new UserControlImport_Column();
                    //DiversityCollection.Import_Column ICRelatedUnitID = DiversityCollection.Import_Column.GetImportColumn(StepKey, ImportStep.TableName(), ImportStep.TableAlias(), "RelatedUnitID", 1, null
                    //, Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                    DiversityCollection.Import_Column ICRelatedUnitID = DiversityCollection.Import_Column.GetImportColumn(StepKey, ImportStep.TableName(), ImportStep.TableAlias(), "RelatedUnitID", 1, UCRelatedUnitID
                    , Import_Column.SourceType.Database, Import_Column.FixingType.Schema, Import_Column.EntryType.Database); // new Import_Column();
                    ICRelatedUnitID.IsSelected = false;
                    ICRelatedUnitID.CanBeTransformed = false;
                    ICRelatedUnitID.ParentTableAlias(this._SuperiorImportStep.TableAlias());
                    UCRelatedUnitID.initUserControl(ICRelatedUnitID, this._Import);
                    UCRelatedUnitID.Dock = DockStyle.Top;
                    string Title = "Growing on Organism " + this._SuperiorImportStep.TableAlias().Replace("IdentificationUnit", "");
                    UCRelatedUnitID.setTitle(Title);
                    T3.Controls.Add(UCRelatedUnitID);
                }

                // NOTES

                DiversityCollection.UserControls.UserControlImport_Column UCOnlyObserved = new UserControlImport_Column();
                DiversityCollection.Import_Column ICOnlyObserved =
                    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "OnlyObserved", UCOnlyObserved);
                ICOnlyObserved.CanBeTransformed = true;
                ICOnlyObserved.TypeOfEntry = Import_Column.EntryType.Boolean;
                ICOnlyObserved.TypeOfFixing = Import_Column.FixingType.Schema;
                ICOnlyObserved.TypeOfSource = Import_Column.SourceType.Any;
                UCOnlyObserved.initUserControl(ICOnlyObserved, this._Import);
                UCOnlyObserved.Dock = DockStyle.Top;
                T2.Controls.Add(UCOnlyObserved);

                DiversityCollection.UserControls.UserControlImport_Column UCNumberOfUnits = new UserControlImport_Column();
                DiversityCollection.Import_Column ICNumberOfUnits =
                    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "NumberOfUnits", UCNumberOfUnits);
                ICNumberOfUnits.CanBeTransformed = true;
                ICNumberOfUnits.TypeOfEntry = Import_Column.EntryType.Text;
                ICNumberOfUnits.TypeOfFixing = Import_Column.FixingType.Schema;
                ICNumberOfUnits.TypeOfSource = Import_Column.SourceType.Any;
                UCNumberOfUnits.initUserControl(ICNumberOfUnits, this._Import);
                UCNumberOfUnits.Dock = DockStyle.Top;
                T.Controls.Add(UCNumberOfUnits);

                DiversityCollection.UserControls.UserControlImport_Column UCUnitDescription = new UserControlImport_Column();
                DiversityCollection.Import_Column ICUnitDescription =
                    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "UnitDescription", UCUnitDescription);
                ICUnitDescription.CanBeTransformed = true;
                ICUnitDescription.TypeOfEntry = Import_Column.EntryType.Text;
                ICUnitDescription.TypeOfFixing = Import_Column.FixingType.Schema;
                ICUnitDescription.TypeOfSource = Import_Column.SourceType.Any;
                UCUnitDescription.initUserControl(ICUnitDescription, this._Import);
                UCUnitDescription.Dock = DockStyle.Top;
                T2.Controls.Add(UCUnitDescription);

                DiversityCollection.UserControls.UserControlImport_Column UCExsiccataNumber = new UserControlImport_Column();
                DiversityCollection.Import_Column ICExsiccataNumber =
                    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "ExsiccataNumber", UCExsiccataNumber);
                ICExsiccataNumber.CanBeTransformed = true;
                ICExsiccataNumber.TypeOfEntry = Import_Column.EntryType.Text;
                ICExsiccataNumber.TypeOfFixing = Import_Column.FixingType.Schema;
                ICExsiccataNumber.TypeOfSource = Import_Column.SourceType.Any;
                UCExsiccataNumber.initUserControl(ICExsiccataNumber, this._Import);
                UCExsiccataNumber.Dock = DockStyle.Top;
                T.Controls.Add(UCExsiccataNumber);

                DiversityCollection.UserControls.UserControlImport_Column UCGender = new UserControlImport_Column();
                DiversityCollection.Import_Column ICGender = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "Gender", UCGender);
                ICGender.CanBeTransformed = true;
                ICGender.TypeOfEntry = Import_Column.EntryType.Text;
                ICGender.TypeOfFixing = Import_Column.FixingType.Schema;
                ICGender.TypeOfSource = Import_Column.SourceType.Any;
                UCGender.initUserControl(ICGender, this._Import);
                UCGender.Dock = DockStyle.Top;
                T2.Controls.Add(UCGender);

                DiversityCollection.UserControls.UserControlImport_Column UCLifeStage = new UserControlImport_Column();
                DiversityCollection.Import_Column ICLifeStage = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "LifeStage", UCLifeStage);
                ICLifeStage.CanBeTransformed = true;
                ICLifeStage.TypeOfEntry = Import_Column.EntryType.Text;
                ICLifeStage.TypeOfFixing = Import_Column.FixingType.Schema;
                ICLifeStage.TypeOfSource = Import_Column.SourceType.Any;
                UCLifeStage.initUserControl(ICLifeStage, this._Import);
                UCLifeStage.Dock = DockStyle.Top;
                T2.Controls.Add(UCLifeStage);


                // TAXONOMY

                DiversityCollection.UserControls.UserControlImport_Column UCFamily = new UserControlImport_Column();
                DiversityCollection.Import_Column ICFamily = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "FamilyCache", UCFamily);
                ICFamily.CanBeTransformed = true;
                ICFamily.TypeOfEntry = Import_Column.EntryType.Text;
                ICFamily.TypeOfFixing = Import_Column.FixingType.Schema;
                ICFamily.TypeOfSource = Import_Column.SourceType.Any;
                UCFamily.initUserControl(ICFamily, this._Import);
                UCFamily.Dock = DockStyle.Top;
                T.Controls.Add(UCFamily);

                DiversityCollection.UserControls.UserControlImport_Column UCOrder = new UserControlImport_Column();
                DiversityCollection.Import_Column ICOrder = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "OrderCache", UCOrder);
                ICOrder.CanBeTransformed = true;
                ICOrder.TypeOfEntry = Import_Column.EntryType.Text;
                ICOrder.TypeOfFixing = Import_Column.FixingType.Schema;
                ICOrder.TypeOfSource = Import_Column.SourceType.Any;
                UCOrder.initUserControl(ICOrder, this._Import);
                UCOrder.Dock = DockStyle.Top;
                T.Controls.Add(UCOrder);

                DiversityCollection.UserControls.UserControlImport_Column UCHierarchyCache = new UserControlImport_Column();
                DiversityCollection.Import_Column ICHierarchyCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "HierarchyCache", UCHierarchyCache);
                ICHierarchyCache.CanBeTransformed = true;
                ICHierarchyCache.MultiColumn = true;
                ICHierarchyCache.TypeOfEntry = Import_Column.EntryType.Text;
                ICHierarchyCache.TypeOfFixing = Import_Column.FixingType.Schema;
                ICHierarchyCache.TypeOfSource = Import_Column.SourceType.Any;
                UCHierarchyCache.initUserControl(ICHierarchyCache, this._Import);
                UCHierarchyCache.Dock = DockStyle.Top;
                T.Controls.Add(UCHierarchyCache);

                DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
                DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "Notes", UCNotes);
                ICNotes.CanBeTransformed = true;
                ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
                ICNotes.TypeOfFixing = Import_Column.FixingType.None;
                ICNotes.TypeOfSource = Import_Column.SourceType.Any;
                UCNotes.initUserControl(ICNotes, this._Import);
                UCNotes.Dock = DockStyle.Top;
                //UCNotes.SendToBack();
                T2.Controls.Add(UCNotes);

                DiversityCollection.UserControls.UserControlImport_Column UCIdentifier = new UserControlImport_Column();
                DiversityCollection.Import_Column ICIdentifier = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "UnitIdentifier", UCIdentifier);
                ICIdentifier.CanBeTransformed = true;
                ICIdentifier.TypeOfEntry = Import_Column.EntryType.Text;
                ICIdentifier.TypeOfFixing = Import_Column.FixingType.None;
                ICIdentifier.TypeOfSource = Import_Column.SourceType.File;
                UCIdentifier.initUserControl(ICIdentifier, this._Import);
                UCIdentifier.Dock = DockStyle.Top;
                //UCNumber.SendToBack();
                T.Controls.Add(UCIdentifier);

                DiversityCollection.UserControls.UserControlImport_Column UCTaxonomicGroup = new UserControlImport_Column();
                DiversityCollection.Import_Column ICTaxonomicGroup = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "TaxonomicGroup", UCTaxonomicGroup);
                ICTaxonomicGroup.CanBeTransformed = true;
                ICTaxonomicGroup.MustSelect = true;
                ICTaxonomicGroup.TypeOfEntry = Import_Column.EntryType.MandatoryList;
                ICTaxonomicGroup.TypeOfFixing = Import_Column.FixingType.Schema;
                ICTaxonomicGroup.TypeOfSource = Import_Column.SourceType.Any;
                ICTaxonomicGroup.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollTaxonomicGroup_Enum", true, true, true), "DisplayText", "Code");
                ICTaxonomicGroup.DisplayColumn = "DisplayText";
                ICTaxonomicGroup.ValueColumn = "Code";
                UCTaxonomicGroup.initUserControl(ICTaxonomicGroup, this._Import);
                UCTaxonomicGroup.Dock = DockStyle.Top;
                //UColl.SendToBack();
                T.Controls.Add(UCTaxonomicGroup);


                UCTaxonomicGroup.setInterface();

                DiversityCollection.Import_Column ICLastIdentificationCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "LastIdentificationCache", null);
                ICLastIdentificationCache.CanBeTransformed = false;
                //ICLastIdentificationCache.MustSelect = true;
                ICLastIdentificationCache.IsSelected = true;
                ICLastIdentificationCache.ValueIsFixed = true;
                //ICLastIdentificationCache.TypeOfEntry = Import_Column.EntryType.Text;
                //ICLastIdentificationCache.TypeOfFixing = Import_Column.FixingType.None;
                ICLastIdentificationCache.TypeOfSource = Import_Column.SourceType.Database;
                ICLastIdentificationCache.Value = "Organism";
            }
            catch (System.Exception ex) { }
        }

        //private void AddStepControlsTaxonomy(System.Windows.Forms.TabPage TP, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        //{
        //    System.Windows.Forms.Panel T = new Panel();
        //    TP.Controls.Add(T);
        //    T.Dock = DockStyle.Fill;
        //    string StepKey = ImportStep.StepKey();// DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStep.Organism, null, NextStepNumber); //DiversityCollection.Import.getImportStepKey(Import.ImportStep.Organism, this._Import.LastStepNumber(DiversityCollection.Import.ImportStep.Organism));

        //    DiversityCollection.UserControls.UserControlImport_Column UCNumberOfUnits = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICNumberOfUnits =
        //        DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "NumberOfUnits", UCNumberOfUnits);
        //    ICNumberOfUnits.CanBeTransformed = true;
        //    ICNumberOfUnits.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICNumberOfUnits.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICNumberOfUnits.TypeOfSource = Import_Column.SourceType.Any;
        //    UCNumberOfUnits.initUserControl(ICNumberOfUnits, this._Import);
        //    UCNumberOfUnits.Dock = DockStyle.Top;
        //    T.Controls.Add(UCNumberOfUnits);

        //    DiversityCollection.UserControls.UserControlImport_Column UCExsiccataNumber = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICExsiccataNumber =
        //        DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "ExsiccataNumber", UCExsiccataNumber);
        //    ICExsiccataNumber.CanBeTransformed = true;
        //    ICExsiccataNumber.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICExsiccataNumber.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICExsiccataNumber.TypeOfSource = Import_Column.SourceType.Any;
        //    UCExsiccataNumber.initUserControl(ICExsiccataNumber, this._Import);
        //    UCExsiccataNumber.Dock = DockStyle.Top;
        //    T.Controls.Add(UCExsiccataNumber);

        //    DiversityCollection.UserControls.UserControlImport_Column UCFamily = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICFamily = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "FamilyCache", UCFamily);
        //    ICFamily.CanBeTransformed = true;
        //    ICFamily.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICFamily.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICFamily.TypeOfSource = Import_Column.SourceType.Any;
        //    UCFamily.initUserControl(ICFamily, this._Import);
        //    UCFamily.Dock = DockStyle.Top;
        //    T.Controls.Add(UCFamily);

        //    DiversityCollection.UserControls.UserControlImport_Column UCHierarchyCache = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICHierarchyCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "HierarchyCache", UCHierarchyCache);
        //    ICHierarchyCache.CanBeTransformed = true;
        //    ICHierarchyCache.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICHierarchyCache.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICHierarchyCache.TypeOfSource = Import_Column.SourceType.Any;
        //    UCHierarchyCache.initUserControl(ICHierarchyCache, this._Import);
        //    UCHierarchyCache.Dock = DockStyle.Top;
        //    T.Controls.Add(UCHierarchyCache);

        //    DiversityCollection.UserControls.UserControlImport_Column UCIdentifier = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICIdentifier = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "UnitIdentifier", UCIdentifier);
        //    ICIdentifier.CanBeTransformed = true;
        //    ICIdentifier.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICIdentifier.TypeOfFixing = Import_Column.FixingType.None;
        //    ICIdentifier.TypeOfSource = Import_Column.SourceType.File;
        //    UCIdentifier.initUserControl(ICIdentifier, this._Import);
        //    UCIdentifier.Dock = DockStyle.Top;
        //    //UCNumber.SendToBack();
        //    T.Controls.Add(UCIdentifier);

        //    DiversityCollection.UserControls.UserControlImport_Column UCTaxonomicGroup = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICTaxonomicGroup = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "TaxonomicGroup", UCTaxonomicGroup);
        //    ICTaxonomicGroup.CanBeTransformed = true;
        //    ICTaxonomicGroup.MustSelect = true;
        //    ICTaxonomicGroup.TypeOfEntry = Import_Column.EntryType.MandatoryList;
        //    ICTaxonomicGroup.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICTaxonomicGroup.TypeOfSource = Import_Column.SourceType.Any;
        //    ICTaxonomicGroup.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollTaxonomicGroup_Enum", true, true, true), "DisplayText", "Code");
        //    ICTaxonomicGroup.DisplayColumn = "DisplayText";
        //    ICTaxonomicGroup.ValueColumn = "Code";
        //    UCTaxonomicGroup.initUserControl(ICTaxonomicGroup, this._Import);
        //    UCTaxonomicGroup.Dock = DockStyle.Top;
        //    //UColl.SendToBack();
        //    T.Controls.Add(UCTaxonomicGroup);


        //    UCTaxonomicGroup.setInterface();

        //    DiversityCollection.Import_Column ICLastIdentificationCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "LastIdentificationCache", null);
        //    ICLastIdentificationCache.CanBeTransformed = false;
        //    //ICLastIdentificationCache.MustSelect = true;
        //    ICLastIdentificationCache.IsSelected = true;
        //    ICLastIdentificationCache.ValueIsFixed = true;
        //    //ICLastIdentificationCache.TypeOfEntry = Import_Column.EntryType.Text;
        //    //ICLastIdentificationCache.TypeOfFixing = Import_Column.FixingType.None;
        //    ICLastIdentificationCache.TypeOfSource = Import_Column.SourceType.Database;
        //    ICLastIdentificationCache.Value = "Organism";
        //}

        //private void AddStepControlsNotes(System.Windows.Forms.TabPage TP, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        //{
        //    System.Windows.Forms.TabControl TC = new TabControl();
        //    TP.Controls.Add(TC);
        //    TC.Dock = DockStyle.Fill;

        //    System.Windows.Forms.TabPage T = new TabPage("Taxonomy and numbers");
        //    System.Windows.Forms.TabPage T2 = new TabPage("Notes etc.");
        //    TC.TabPages.Add(T);
        //    TC.TabPages.Add(T2);
        //    //System.Windows.Forms.Panel T = new Panel();
        //    //TP.Controls.Add(T);
        //    //T.Dock = DockStyle.Fill;
        //    string StepKey = ImportStep.StepKey();// DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStep.Organism, null, NextStepNumber); //DiversityCollection.Import.getImportStepKey(Import.ImportStep.Organism, this._Import.LastStepNumber(DiversityCollection.Import.ImportStep.Organism));

        //    DiversityCollection.UserControls.UserControlImport_Column UCOnlyObserved = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICOnlyObserved =
        //        DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "OnlyObserved", UCOnlyObserved);
        //    ICOnlyObserved.CanBeTransformed = true;
        //    ICOnlyObserved.TypeOfEntry = Import_Column.EntryType.Boolean;
        //    ICOnlyObserved.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICOnlyObserved.TypeOfSource = Import_Column.SourceType.Any;
        //    UCOnlyObserved.initUserControl(ICOnlyObserved, this._Import);
        //    UCOnlyObserved.Dock = DockStyle.Top;
        //    T2.Controls.Add(UCOnlyObserved);

        //    DiversityCollection.UserControls.UserControlImport_Column UCNumberOfUnits = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICNumberOfUnits =
        //        DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "NumberOfUnits", UCNumberOfUnits);
        //    ICNumberOfUnits.CanBeTransformed = true;
        //    ICNumberOfUnits.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICNumberOfUnits.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICNumberOfUnits.TypeOfSource = Import_Column.SourceType.Any;
        //    UCNumberOfUnits.initUserControl(ICNumberOfUnits, this._Import);
        //    UCNumberOfUnits.Dock = DockStyle.Top;
        //    T.Controls.Add(UCNumberOfUnits);

        //    DiversityCollection.UserControls.UserControlImport_Column UCUnitDescription = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICUnitDescription =
        //        DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "UnitDescription", UCUnitDescription);
        //    ICUnitDescription.CanBeTransformed = true;
        //    ICUnitDescription.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICUnitDescription.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICUnitDescription.TypeOfSource = Import_Column.SourceType.Any;
        //    UCUnitDescription.initUserControl(ICUnitDescription, this._Import);
        //    UCUnitDescription.Dock = DockStyle.Top;
        //    T2.Controls.Add(UCUnitDescription);

        //    DiversityCollection.UserControls.UserControlImport_Column UCExsiccataNumber = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICExsiccataNumber =
        //        DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "ExsiccataNumber", UCExsiccataNumber);
        //    ICExsiccataNumber.CanBeTransformed = true;
        //    ICExsiccataNumber.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICExsiccataNumber.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICExsiccataNumber.TypeOfSource = Import_Column.SourceType.Any;
        //    UCExsiccataNumber.initUserControl(ICExsiccataNumber, this._Import);
        //    UCExsiccataNumber.Dock = DockStyle.Top;
        //    T.Controls.Add(UCExsiccataNumber);

        //    DiversityCollection.UserControls.UserControlImport_Column UCGender = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICGender = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "Gender", UCGender);
        //    ICGender.CanBeTransformed = true;
        //    ICGender.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICGender.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICGender.TypeOfSource = Import_Column.SourceType.Any;
        //    UCGender.initUserControl(ICGender, this._Import);
        //    UCGender.Dock = DockStyle.Top;
        //    T2.Controls.Add(UCGender);

        //    DiversityCollection.UserControls.UserControlImport_Column UCLifeStage = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICLifeStage = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "LifeStage", UCLifeStage);
        //    ICLifeStage.CanBeTransformed = true;
        //    ICLifeStage.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICLifeStage.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICLifeStage.TypeOfSource = Import_Column.SourceType.Any;
        //    UCLifeStage.initUserControl(ICLifeStage, this._Import);
        //    UCLifeStage.Dock = DockStyle.Top;
        //    T2.Controls.Add(UCLifeStage);

        //    DiversityCollection.UserControls.UserControlImport_Column UCFamily = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICFamily = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "FamilyCache", UCFamily);
        //    ICFamily.CanBeTransformed = true;
        //    ICFamily.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICFamily.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICFamily.TypeOfSource = Import_Column.SourceType.Any;
        //    UCFamily.initUserControl(ICFamily, this._Import);
        //    UCFamily.Dock = DockStyle.Top;
        //    T.Controls.Add(UCFamily);

        //    DiversityCollection.UserControls.UserControlImport_Column UCHierarchyCache = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICHierarchyCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "HierarchyCache", UCHierarchyCache);
        //    ICHierarchyCache.CanBeTransformed = true;
        //    ICHierarchyCache.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICHierarchyCache.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICHierarchyCache.TypeOfSource = Import_Column.SourceType.Any;
        //    UCHierarchyCache.initUserControl(ICHierarchyCache, this._Import);
        //    UCHierarchyCache.Dock = DockStyle.Top;
        //    T.Controls.Add(UCHierarchyCache);

        //    DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "Notes", UCNotes);
        //    ICNotes.CanBeTransformed = true;
        //    ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICNotes.TypeOfFixing = Import_Column.FixingType.None;
        //    ICNotes.TypeOfSource = Import_Column.SourceType.Any;
        //    UCNotes.initUserControl(ICNotes, this._Import);
        //    UCNotes.Dock = DockStyle.Top;
        //    //UCNotes.SendToBack();
        //    T2.Controls.Add(UCNotes);

        //    DiversityCollection.UserControls.UserControlImport_Column UCIdentifier = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICIdentifier = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "UnitIdentifier", UCIdentifier);
        //    ICIdentifier.CanBeTransformed = true;
        //    ICIdentifier.TypeOfEntry = Import_Column.EntryType.Text;
        //    ICIdentifier.TypeOfFixing = Import_Column.FixingType.None;
        //    ICIdentifier.TypeOfSource = Import_Column.SourceType.File;
        //    UCIdentifier.initUserControl(ICIdentifier, this._Import);
        //    UCIdentifier.Dock = DockStyle.Top;
        //    //UCNumber.SendToBack();
        //    T.Controls.Add(UCIdentifier);

        //    DiversityCollection.UserControls.UserControlImport_Column UCTaxonomicGroup = new UserControlImport_Column();
        //    DiversityCollection.Import_Column ICTaxonomicGroup = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "TaxonomicGroup", UCTaxonomicGroup);
        //    ICTaxonomicGroup.CanBeTransformed = true;
        //    ICTaxonomicGroup.MustSelect = true;
        //    ICTaxonomicGroup.TypeOfEntry = Import_Column.EntryType.MandatoryList;
        //    ICTaxonomicGroup.TypeOfFixing = Import_Column.FixingType.Schema;
        //    ICTaxonomicGroup.TypeOfSource = Import_Column.SourceType.Any;
        //    ICTaxonomicGroup.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollTaxonomicGroup_Enum", true, true, true), "DisplayText", "Code");
        //    ICTaxonomicGroup.DisplayColumn = "DisplayText";
        //    ICTaxonomicGroup.ValueColumn = "Code";
        //    UCTaxonomicGroup.initUserControl(ICTaxonomicGroup, this._Import);
        //    UCTaxonomicGroup.Dock = DockStyle.Top;
        //    //UColl.SendToBack();
        //    T.Controls.Add(UCTaxonomicGroup);


        //    UCTaxonomicGroup.setInterface();

        //    DiversityCollection.Import_Column ICLastIdentificationCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "LastIdentificationCache", null);
        //    ICLastIdentificationCache.CanBeTransformed = false;
        //    //ICLastIdentificationCache.MustSelect = true;
        //    ICLastIdentificationCache.IsSelected = true;
        //    ICLastIdentificationCache.ValueIsFixed = true;
        //    //ICLastIdentificationCache.TypeOfEntry = Import_Column.EntryType.Text;
        //    //ICLastIdentificationCache.TypeOfFixing = Import_Column.FixingType.None;
        //    ICLastIdentificationCache.TypeOfSource = Import_Column.SourceType.Database;
        //    ICLastIdentificationCache.Value = "Organism";
        //}

        #endregion

        #region Identification

        private void initIdentification(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step IS)
        {
            try
            {
                int i = T.Controls.Count;
                // test - hier gibt es nur ein userControlImportIdentification fuer alle
                //this._SuperiorImportStep = IS;
                //T.Controls.Add(this.userControlImportIdentification);
                //this.userControlImportIdentification.initUserControl(this._iImportInterface, IS);
                //this.userControlImportIdentification.Dock = DockStyle.Fill;
                // TODO - bei allen anpassen
                // test - geaendert zu vielen - passt
                DiversityCollection.UserControls.UserControlImportIdentification UI = new UserControlImportIdentification();
                this._SuperiorImportStep = IS;
                T.Controls.Add(UI);
                UI.initUserControl(this._iImportInterface, IS);
                UI.Dock = DockStyle.Fill;
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region Analysis

        private void initAnalysis(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step IS)
        {
            try
            {
                DiversityCollection.UserControls.UserControlImportAnalysis UI = new UserControlImportAnalysis();
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
