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
        /// <summary>
        /// the tab pages for the steps
        /// </summary>
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        /// <summary>
        /// the tab pages for the analysis controls
        /// </summary>
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesAnalysis;
        /// <summary>
        /// the tab pages for the method controls
        /// </summary>
        //System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesMethod;
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

            this._ImportSteps = null;
            //this._TabPagesMethod = null;
            this._TabPagesAnalysis = null;
        }

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

        public static System.Collections.Generic.List<string> _StepsCalledForSettingUserControl;
        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
            // setting the superior controls
            if (ImportStep.SuperiorImportStep != null)
            {
                // "_08:01_02:01_00"; "_08:01_02:01"
                // "_08:01_02:01_00_00:01"; "_08:01_02:01_00"
                // "_08:01_02:01_00"; "_08:01_02:01"
                // "_08:01_02:01_00"; "_08:01_02:01"
                // "_08:01_02:01_00"; "_08:01_02:01"
                // Endlosschleife
                /// TODO: notlösung bis Ursache gefunden
                //if (ImportStep.StepKey().StartsWith("_07:01_02:") &&
                //    ImportStep.StepKey().EndsWith("_00"))
                //{
                //    if (DiversityCollection.UserControls.UserControlImportAnalysis._StepsCalledForSettingUserControl == null)
                //        DiversityCollection.UserControls.UserControlImportAnalysis._StepsCalledForSettingUserControl = new List<string>();
                //    if (!DiversityCollection.UserControls.UserControlImportAnalysis._StepsCalledForSettingUserControl.Contains(ImportStep.StepKey()))
                //    {
                //        DiversityCollection.UserControls.UserControlImportAnalysis._StepsCalledForSettingUserControl.Add(ImportStep.StepKey());
                //        ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep);
                //    }
                //    else
                //    {
                //        DiversityCollection.UserControls.UserControlImportAnalysis._StepsCalledForSettingUserControl = new List<string>();
                //    }
                //}
                //else
                    ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep);
            }

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


            // aus Identification uebernommen - Testweise

            //Key = ImportStep.StepKey();
            //bool ControlsAdded = false;
            ////if (this._TabPagesAnalysis == null)
            ////    return;
            //if (this._TabPages == null)
            //    return;
            //foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvTP in this._TabPages)
            //{
            //    if (Key.StartsWith(kvTP.Key))
            //    {
            //        this.tabControlImportSteps.TabPages.Add(kvTP.Value);
            //        foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabControl> kvTC in this._TabContols)
            //        {
            //            if (Key.StartsWith(kvTC.Key))
            //            {
            //                // clear the tabcontrol of the unit before adding the tabpage of the current data
            //                kvTC.Value.TabPages.Clear();

            //                // Adding the controls for the Unit if a unit was selected
            //                if (Key == kvTC.Key && this._TabPagesAnalysis.ContainsKey(Key))
            //                {
            //                    kvTC.Value.TabPages.Add(this._TabPagesAnalysis[Key]);
            //                    ControlsAdded = true;
            //                }
            //                else // Adding the controls of dependent data like method if one of those where selected
            //                {
            //                    if (ImportStep.StepDetailKey() == (int)DiversityCollection.Import.ImportStepUnit.Identification)
            //                    {
            //                        foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvI in this._TabPagesMethod)
            //                        {
            //                            if (Key.StartsWith(kvI.Key))
            //                            {
            //                                kvTC.Value.TabPages.Add(kvI.Value);

            //                                // zum Test
            //                                //System.Windows.Forms.TabControl TC = (System.Windows.Forms.TabControl)kvTC.Value.Controls[0];
            //                                //TC.TabPages.Add(kvI.Value);
            //                                //System.Windows.Forms.TabPage TX = new TabPage(Key + " U");
            //                                //kvTC.Value.TabPages.Add(TX);


            //                                ControlsAdded = true;
            //                                break;
            //                            }
            //                        }
            //                    }
            //                    else if (ImportStep.StepDetailKey() == (int)DiversityCollection.Import.ImportStepUnit.Analysis)
            //                    {
            //                        foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvA in this._TabPagesAnalysis)
            //                        {
            //                            if (Key.StartsWith(kvA.Key))
            //                            {
            //                                kvTC.Value.TabPages.Add(kvA.Value);
            //                                ControlsAdded = true;
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }
            //                break;
            //            }
            //            if (ControlsAdded)
            //                break;
            //        }
            //        break;
            //    }
            //    if (ControlsAdded)
            //        break;
            //}
            //if (this.tabControlImportSteps.TabPages.Count == 0 && ImportStep.IsVisible())
            //{
            //    string FirstChildKey = DiversityCollection.Import.getImportStepKeyFirstChild(ImportStep);
            //    this.tabControlImportSteps.TabPages.Add(this._TabPages[FirstChildKey]);
            //}

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

                if (this._TabPagesAnalysis == null)
                    this._TabPagesAnalysis = new SortedList<string, TabPage>();
                this._TabPagesAnalysis.Add(IS.StepKey(), TStep);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                if (this._TabContols == null)
                    this._TabContols = new SortedList<string, TabControl>();

                this.AddAnalysisSteps(TStep, IS);

                //System.Windows.Forms.TabPage TPageMethod = new TabPage();
                //TPageMethod.ImageIndex = this.tabPageMethod.ImageIndex;
                //// Test
                //TPageMethod.Text = this.tabPageMethod.Text;// +" " + IS.StepKey();
                //// alle Tab pages des User Controls - hier landen alle Identifications aller Units
                //if (this._TabPagesMethod == null)
                //    this._TabPagesMethod = new SortedList<string, TabPage>();
                //this._TabPagesMethod.Add(IS.StepKey(), TPageMethod);
                //this.initMethod(TPageMethod, IS);


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

        #region Method

        private void initMethod(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step IS)
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
                DiversityCollection.UserControls.UserControlImportMethod UI = new UserControlImportMethod();
                this._SuperiorImportStep = IS;
                T.Controls.Add(UI);
                UI.initUserControl(this._iImportInterface, IS);
                UI.Dock = DockStyle.Fill;
            }
            catch (System.Exception ex) { }
        }

        #endregion


        #region Auxillary functions

        /// <summary>
        /// Adding all controls for an analysis import step
        /// </summary>
        /// <param name="T">The tabpage</param>
        /// <param name="ImportStep">the Import step</param>
        private void AddAnalysisSteps(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)// System.Windows.Forms.TabPage T, string TableAlias)
        {
            string StepKey = ImportStep.StepKey();
            try
            {
                //the tabcontrol holding the tabpage for analysis and method data 
                System.Windows.Forms.TabControl TabControlStep = new System.Windows.Forms.TabControl();
                TabControlStep.Dock = DockStyle.Fill;
                TabControlStep.ImageList = this.tabControlAnalysis.ImageList;
                T.Controls.Add(TabControlStep);
                this._TabContols.Add(ImportStep.StepKey(), TabControlStep);

                // the tab page holding the infos related to the analysis
                System.Windows.Forms.TabPage TPanalysis = new TabPage("Analysis");
                if (this._TabPagesAnalysis == null)
                    this._TabPagesAnalysis = new SortedList<string, TabPage>();
                this._TabPagesAnalysis.Add(ImportStep.StepKey(), TPanalysis);
                TPanalysis.ImageIndex = this.tabPageAnalysis.ImageIndex;
                //string Title = this.tabPageAnalysis.Text;
                TabControlStep.TabPages.Add(TPanalysis);
                this.AddStepControlsAnalysis(TPanalysis, ImportStep);
                TabControlStep.TabPages.Add(TPanalysis);

                System.Windows.Forms.TabPage TPResponsible = new TabPage("Responsible, date, notes");
                TPResponsible.ImageIndex = this.tabPageResponsible.ImageIndex;
                this.AddStepControlsResponsible(TPResponsible, ImportStep);
                TabControlStep.TabPages.Add(TPResponsible);

                return;

                //if (this._TabPagesMethod == null)
                //    this._TabPagesMethod = new SortedList<string, TabPage>();

                //System.Windows.Forms.TabControl TCMethod = new TabControl();
                //TCMethod.Dock = DockStyle.Fill;
                //TCMethod.ImageList = this.tabControlAnalysis.ImageList;


                //System.Windows.Forms.TabPage TPMethod = new TabPage("Method");
                //TCMethod.TabPages.Add(TPMethod);
                //TPMethod.Controls.Add(TCMethod);
                //TPMethod.ImageIndex = this.tabPageMethod.ImageIndex;
                ////TabControlAnalysis.TabPages.Add(TPMethod);

                //DiversityCollection.UserControls.UserControlImportMethod UcM = new UserControlImportMethod();
                //TPMethod.Controls.Add(UcM);
                //UcM.Dock = DockStyle.Fill;
                //UcM.initUserControl(this._iImportInterface, ImportStep);
                //string StepKeyMethod = DiversityCollection.Import.getImportStepKey(ImportStep, Import.ImportStepUnitAnalysisMethod.Method);
                //DiversityCollection.Import_Step IEMethod = DiversityCollection.Import_Step.GetImportStep("Method", "The method used for the analysis of the specimen", StepKeyMethod,
                //    "IdentificationUnitAnalysisMethod",
                //    null, ImportStep, 2, (UserControls.iUserControlImportInterface)UcM,
                //    DiversityCollection.Specimen.ImageForTable("CollectionEventMethod", false), this._SuperiorImportStep.SelectionPanel);
                //UcM.ImportStepMethod = IEMethod;
                ////TabControlAnalysis.TabPages.Add(TPMethod);





                //string StepKeyMethod = DiversityCollection.Import.getImportStepKey(ImportStep, Import.ImportStepUnitAnalysisMethod.Method);
                //// "_08:01_02:01_00"

                //DiversityCollection.Import_Step IEMethod = DiversityCollection.Import_Step.GetImportStep("Method", "The method used for the analysis of the specimen", StepKeyMethod,
                //    "IdentificationUnitAnalysisMethod",
                //    null, ImportStep, 2, (UserControls.iUserControlImportInterface)this,
                //    DiversityCollection.Specimen.ImageForTable("CollectionEventMethod", false), this._SuperiorImportStep.SelectionPanel);
                ////IEMethod.CanHide(true);
                ////IEMethod.IsVisible(false);

                //TabControlAnalysis.TabPages.Add(this.tabPageMethod);
                //this.userControlImportMethod.ImportStepMethod = IEMethod;
                //this.userControlImportMethod.initUserControl(this._iImportInterface, ImportStep);

            }
            catch (System.Exception ex) { }

        }

        /// <summary>
        /// Adding the controls that hold the columns that are directly linked to the table Analysis (e.g. not to Method)
        /// </summary>
        /// <param name="T"></param>
        /// <param name="ImportStep"></param>
        private void AddStepControlsAnalysis(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        {
            try
            {
                System.Windows.Forms.TabControl TCanalysis = new TabControl();
                TCanalysis.Dock = DockStyle.Fill;
                T.Controls.Add(TCanalysis);

                System.Windows.Forms.TabPage TPanalysisType = new TabPage("Analysis type");
                TCanalysis.TabPages.Add(TPanalysisType);
                this.AddStepControlsAnalysisType(TPanalysisType, ImportStep);

                System.Windows.Forms.TabPage TPanalysisResponsible = new TabPage("Responsible etc.");
                TCanalysis.TabPages.Add(TPanalysisResponsible);
                this.AddStepControlsResponsible(TPanalysisResponsible, ImportStep);

            }
            catch (System.Exception ex) { }
        }

        private void AddStepControlsAnalysisType(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
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
            try
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
            catch (System.Exception ex)
            {
            }
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
