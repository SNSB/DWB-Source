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
    public partial class UserControlImportIdentification : UserControl, iUserControlImportInterface
    {
 
        #region Parameter
        
        private DiversityCollection.FormImportWizard _FormImportWizard;
        private DiversityCollection.iImportInterface _iImportInterface;
        //private string _TableAlias;
        private DiversityCollection.Import _Import;
        private string _StepKey;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabControl> _TabControls;

        #endregion

        #region Construction
        
        public UserControlImportIdentification()
        {
            InitializeComponent(); 
            /// TODO: erstes Anlegen erfolgt bei Erstellung des UserControlImportUnit im Designer
            /// zweites Anlegen erfolgt in initIdentification in UserControlImportUnit
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

        public string StepKey() { return this._StepKey; }
        public void UpdateSelectionPanel() { }

        public void initUserControl(DiversityCollection.iImportInterface I, DiversityCollection.Import_Step SuperiorImportStep)
        {
            this._iImportInterface = I;
            this._SuperiorImportStep = SuperiorImportStep;
            this._Import = I.getImport();
            this.AddImportStep();

            string Table = this._ImportSteps[this._StepKey].TableName();
            /// TODO - korrektur auch in anderen usercontrols auf gleicher Zwischenebene
            this.toolTip.SetToolTip(this.buttonAdd, "Add a new " + Table);
            this.toolTip.SetToolTip(this.buttonRemove, "Hide the selected " + Table);
            this.toolTip.SetToolTip(this.buttonRecover, "Show any hidden " + Table);
        }

        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
            // setting the superior controls
            if (ImportStep.SuperiorImportStep != null)
                ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep);

            // falsches UserControl - hier wird immer das letzte angesteuert

            // clearing the tabcontrol before adding the tabpage for the current data
            this.tabControlImportSteps.TabPages.Clear();
            string Key = ImportStep.StepKey();
            // setting the controls for the step
            if (this._TabPages.ContainsKey(Key)) // TODO: einmal nur eine: {[_07:01_01:01, TabPage: {Identification 1}]}, beim anderen aufruf die restlichen 4 ohne den ersten. also werden vermutlich 2 Usercontrols angelegt
            {
                this.tabControlImportSteps.TabPages.Add(this._TabPages[Key]);
            }
        }
        
        public void AddImportStep()
        {
            if (this._SuperiorImportStep == null)
            {
               // System.Windows.Forms.MessageBox.Show("Bug in UserControlImportIdentification in AddImportStep: this._SuperiorImportStep = null");
                return;
            }
            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._SuperiorImportStep, (int)Import.ImportStepUnit.Identification);
            try
            {
                string StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepUnit.Identification, this._SuperiorImportStep, NextImportStepNumber);
                // Test 
                string Title = "Identification " + (NextImportStepNumber).ToString();// +"_" + StepKey;
                System.Windows.Forms.TabPage TStep = new TabPage(Title);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Identification " + NextImportStepNumber.ToString(),
                    "Identification " + NextImportStepNumber.ToString(),
                    StepKey,
                    "Identification",
                    NextImportStepNumber,
                    this._SuperiorImportStep,
                    2,
                    (iUserControlImportInterface)this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Identification),null
                    );

                // Zur Kontrolle
                //if (this._StepKey != null) 
                //    System.Windows.Forms.MessageBox.Show("bug");
                this._StepKey = IS.StepKey();

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                if (this._TabControls == null)
                    this._TabControls = new SortedList<string, TabControl>();

                this.AddIdentificationSteps(TStep, IS);

                IS.setStepError();

                this._SuperiorImportStep.getUserControlImportInterface().UpdateSelectionPanel();


                // Test
                //string Key = IS.StepKey();
                //if (DiversityCollection.Import.ImportSteps[IS.StepKey()].getUserControlImportInterface() == null)
                //{
                //    DiversityCollection.Import.ImportColumns[IS.StepKey()].ImportColumnControl = (DiversityCollection.iImportColumnControl)this;
                //}

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

                        //string KeyOfSuperiorImportStep = StepKey.Substring(0, 6);
                        //this._SuperiorImportStep = DiversityCollection.Import.ImportSteps[KeyOfSuperiorImportStep];
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

        private void AddIdentificationSteps(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)// System.Windows.Forms.TabPage T, string TableAlias)
        {
            string StepKey = ImportStep.StepKey();
            try
            {
                System.Windows.Forms.TabControl TabControlIdentification = new System.Windows.Forms.TabControl();
                TabControlIdentification.Dock = DockStyle.Fill;
                TabControlIdentification.ImageList = this.tabControlIdentification.ImageList;
                T.Controls.Add(TabControlIdentification);
                this._TabControls.Add(ImportStep.StepKey(), TabControlIdentification);

                System.Windows.Forms.TabPage TPname = new TabPage("Taxonomic name");
                TPname.ImageIndex = this.tabPageName.ImageIndex;
                string Title = this.tabPageName.Text;
                TabControlIdentification.TabPages.Add(TPname);
                this.AddStepControlsName(TPname, ImportStep);

                System.Windows.Forms.TabPage TPResponsible = new TabPage("Responsible, date");
                TPResponsible.ImageIndex = this.tabPageResponsible.ImageIndex;
                this.AddStepControlsResponsible(TPResponsible, ImportStep);
                TabControlIdentification.TabPages.Add(TPResponsible);

                System.Windows.Forms.TabPage TPReference = new TabPage("Reference, notes");
                TPReference.ImageIndex = this.tabPageReference.ImageIndex;
                this.AddStepControlsReference(TPReference, ImportStep);
                TabControlIdentification.TabPages.Add(TPReference);
            }
            catch (System.Exception ex) { }
        }

        private void AddStepControlsName(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        {
            DiversityCollection.UserControls.UserControlImport_Column UCIdentificationQualifier = new UserControlImport_Column();
            DiversityCollection.Import_Column ICIdentificationQualifier = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationQualifier", UCIdentificationQualifier);
            ICIdentificationQualifier.CanBeTransformed = true;
            ICIdentificationQualifier.MustSelect = false;
            ICIdentificationQualifier.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICIdentificationQualifier.TypeOfFixing = Import_Column.FixingType.Schema;
            ICIdentificationQualifier.TypeOfSource = Import_Column.SourceType.Any;
            ICIdentificationQualifier.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollIdentificationQualifier_Enum", true, true, true), "DisplayText", "Code");
            ICIdentificationQualifier.DisplayColumn = "DisplayText";
            ICIdentificationQualifier.ValueColumn = "Code";
            UCIdentificationQualifier.initUserControl(ICIdentificationQualifier, this._Import);
            UCIdentificationQualifier.Dock = DockStyle.Top;
            T.Controls.Add(UCIdentificationQualifier);

            DiversityCollection.UserControls.UserControlImport_Column UCVernacularTerm = new UserControlImport_Column();
            DiversityCollection.Import_Column ICVernacularTerm = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "VernacularTerm", UCVernacularTerm);
            ICVernacularTerm.CanBeTransformed = true;
            ICVernacularTerm.TypeOfEntry = Import_Column.EntryType.Text;
            ICVernacularTerm.TypeOfFixing = Import_Column.FixingType.None;
            ICVernacularTerm.TypeOfSource = Import_Column.SourceType.File;
            UCVernacularTerm.initUserControl(ICVernacularTerm, this._Import);
            UCVernacularTerm.Dock = DockStyle.Top;
            T.Controls.Add(UCVernacularTerm);

            DiversityCollection.UserControls.UserControlImport_Column UCnameURI = new UserControlImport_Column();
            DiversityCollection.Import_Column ICnameURI = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "NameURI", UCnameURI);
            ICnameURI.CanBeTransformed = true;
            ICnameURI.MultiColumn = false;
            ICnameURI.MustSelect = false;
            ICnameURI.TypeOfEntry = Import_Column.EntryType.Text;
            ICnameURI.TypeOfFixing = Import_Column.FixingType.Schema;
            ICnameURI.TypeOfSource = Import_Column.SourceType.Any;
            UCnameURI.initUserControl(ICnameURI, this._Import);
            UCnameURI.Dock = DockStyle.Top;
            T.Controls.Add(UCnameURI);

            DiversityCollection.UserControls.UserControlImport_Column UCname = new UserControlImport_Column();
            DiversityCollection.Import_Column ICname = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "TaxonomicName", UCname);
            ICname.CanBeTransformed = true;
            ICname.MultiColumn = true;
            ICname.MustSelect = true;
            ICname.TypeOfEntry = Import_Column.EntryType.Text;
            ICname.TypeOfFixing = Import_Column.FixingType.Schema;
            ICname.TypeOfSource = Import_Column.SourceType.Any;
            UCname.initUserControl(ICname, this._Import);
            UCname.Dock = DockStyle.Top;
            T.Controls.Add(UCname);

            DiversityCollection.Import_Column ICSequence = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationSequence", 1, null
                ,Import_Column.SourceType.Database,Import_Column.FixingType.Schema,Import_Column.EntryType.Database); // new Import_Column();
            ICSequence.IsSelected = true;
            ICSequence.CanBeTransformed = false;
            ICSequence.ValueIsFixed = true;
            ICSequence.Value = (ImportStep.StepParallelNumber() * -1).ToString();

            DiversityCollection.Import_Column ICIdentificationUnitID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationUnitID", 1, null
                ,Import_Column.SourceType.Database,Import_Column.FixingType.None,Import_Column.EntryType.Database); // new Import_Column();
            ICIdentificationUnitID.IsSelected = true;
            ICIdentificationUnitID.CanBeTransformed = false;
            ICIdentificationUnitID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

            DiversityCollection.Import_Column ICCollectionSpecimenID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionSpecimenID", 1, null
                ,Import_Column.SourceType.Database,Import_Column.FixingType.None,Import_Column.EntryType.Database); // new Import_Column();
            ICCollectionSpecimenID.IsSelected = true;
            ICCollectionSpecimenID.CanBeTransformed = false;
            ICCollectionSpecimenID.ParentTableAlias(this._SuperiorImportStep.TableAlias());
        }

        //private void AddStepControlsDate(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        //{
        //    DiversityCollection.UserControls.UserControlImportDate UCIdentificationDate = new UserControlImportDate();
        //    DiversityCollection.Import_Column ICD = DiversityCollection.Import_Column.GetImportColumn(
        //        ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDay", UCIdentificationDate.userControlImport_ColumnDay);
        //    ICD.CanBeTransformed = true;
        //    DiversityCollection.Import_Column ICM = DiversityCollection.Import_Column.GetImportColumn(
        //        ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationMonth", UCIdentificationDate.userControlImport_ColumnMonth);
        //    ICM.CanBeTransformed = true;
        //    DiversityCollection.Import_Column ICY = DiversityCollection.Import_Column.GetImportColumn(
        //        ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationYear", UCIdentificationDate.userControlImport_ColumnYear);
        //    ICY.CanBeTransformed = true;
        //    DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(
        //        ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDateSupplement", UCIdentificationDate.userControlImport_ColumnSupplement);
        //    ICDate.CanBeTransformed = true;
        //    UCIdentificationDate.initUserControl(ICD, ICM, ICY, ICDate,
        //        this._StepKey, this._Import, this._FormImportWizard);

        //    UCIdentificationDate.Dock = DockStyle.Top;
        //    T.Controls.Add(UCIdentificationDate);
        //}

        private void AddStepControlsResponsible(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        {

    //        DiversityCollection.Import_Column ICD = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
    //, "CollectionSpecimen", "AccessionDay", this.userControlImportDate_AccessionDate.userControlImport_ColumnDay);
    //        ICD.CanBeTransformed = true;
    //        DiversityCollection.Import_Column ICM = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
    //            , "CollectionSpecimen", "AccessionMonth", this.userControlImportDate_AccessionDate.userControlImport_ColumnMonth);
    //        ICM.CanBeTransformed = true;
    //        DiversityCollection.Import_Column ICY = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
    //            , "CollectionSpecimen", "AccessionYear", this.userControlImportDate_AccessionDate.userControlImport_ColumnYear);
    //        ICY.CanBeTransformed = true;
    //        DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
    //            , "CollectionSpecimen", "AccessionDateSupplement", this.userControlImportDate_AccessionDate.userControlImport_ColumnSupplement);
    //        ICDate.AlternativeColumn = "AccessionDateSupplement";
    //        ICDate.CanBeTransformed = true;
    //        this.userControlImportDate_AccessionDate.initUserControl(ICD, ICM, ICY, ICDate,
    //            DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession), this.Import, this);




            //DiversityCollection.Import_Column ICD = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDay", this.userControlImportDate.userControlImport_ColumnDay);
            //ICD.CanBeTransformed = true;
            //DiversityCollection.Import_Column ICM = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationMonth", this.userControlImportDate.userControlImport_ColumnMonth);
            //ICM.CanBeTransformed = true;
            //DiversityCollection.Import_Column ICY = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationYear", this.userControlImportDate.userControlImport_ColumnYear);
            //ICY.CanBeTransformed = true;
            //DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDateSupplement", this.userControlImportDate.userControlImport_ColumnSupplement);
            //ICDate.CanBeTransformed = true;
            //this.userControlImportDate.initUserControl(ICD, ICM, ICY, ICDate,
            //    this._StepKey, this._Import, this._FormImportWizard);

            //DiversityCollection.UserControls.UserControlImportDate UCIdentificationDate = new UserControlImportDate();

            //DiversityCollection.Import_Column ICD = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDay", UCIdentificationDate.userControlImport_ColumnDay);
            //ICD.CanBeTransformed = true;
            //DiversityCollection.Import_Column ICM = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationMonth", UCIdentificationDate.userControlImport_ColumnMonth);
            //ICM.CanBeTransformed = true;
            //DiversityCollection.Import_Column ICY = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationYear", UCIdentificationDate.userControlImport_ColumnYear);
            //ICY.CanBeTransformed = true;
            //DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(
            //    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDateSupplement", UCIdentificationDate.userControlImport_ColumnSupplement);
            //ICDate.CanBeTransformed = true;
            //UCIdentificationDate.initUserControl(ICD, ICM, ICY, ICDate,
            //    this._StepKey, this._Import, this._FormImportWizard);

            //UCIdentificationDate.Dock = DockStyle.Top;
            //T.Controls.Add(UCIdentificationDate);


            DiversityCollection.UserControls.UserControlImport_Column UserControlImport_ColumnSupplement = new UserControlImport_Column();
            DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDateSupplement", UserControlImport_ColumnSupplement);
            ICDate.CanBeTransformed = true;
            ICDate.TypeOfEntry = Import_Column.EntryType.Text;
            ICDate.TypeOfFixing = Import_Column.FixingType.Import;
            ICDate.StepKey = ImportStep.StepKey();
            UserControlImport_ColumnSupplement.initUserControl(ICDate, this._Import);
            UserControlImport_ColumnSupplement.Dock = DockStyle.Top;
            T.Controls.Add(UserControlImport_ColumnSupplement);

            DiversityCollection.UserControls.UserControlImport_Column UserControlImport_ColumnYear = new UserControlImport_Column();
            DiversityCollection.Import_Column ICY = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationYear", UserControlImport_ColumnYear);
            ICY.CanBeTransformed = true;
            ICY.TypeOfEntry = Import_Column.EntryType.Text;
            ICY.TypeOfFixing = Import_Column.FixingType.Import;
            ICY.StepKey = ImportStep.StepKey();
            UserControlImport_ColumnYear.initUserControl(ICY, this._Import);
            UserControlImport_ColumnYear.Dock = DockStyle.Top;
            T.Controls.Add(UserControlImport_ColumnYear);

            DiversityCollection.UserControls.UserControlImport_Column UCIdentificationMonth = new UserControlImport_Column();
            DiversityCollection.Import_Column ICM = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationMonth", UCIdentificationMonth);
            ICM.CanBeTransformed = true;
            ICM.TypeOfEntry = Import_Column.EntryType.Text;
            ICM.TypeOfFixing = Import_Column.FixingType.Import;
            ICM.StepKey = ImportStep.StepKey();
            UCIdentificationMonth.initUserControl(ICM, this._Import);
            UCIdentificationMonth.Dock = DockStyle.Top;
            T.Controls.Add(UCIdentificationMonth);

            DiversityCollection.UserControls.UserControlImport_Column UCIdentificationDay = new UserControlImport_Column();
            DiversityCollection.Import_Column ICD = DiversityCollection.Import_Column.GetImportColumn(
                ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationDay", UCIdentificationDay);
            ICD.CanBeTransformed = true;
            ICD.TypeOfEntry = Import_Column.EntryType.Text;
            ICD.TypeOfFixing = Import_Column.FixingType.Import;
            ICD.StepKey = ImportStep.StepKey();
            UCIdentificationDay.initUserControl(ICD, this._Import);
            UCIdentificationDay.Dock = DockStyle.Top;
            T.Controls.Add(UCIdentificationDay);
            
            DiversityCollection.UserControls.UserControlImport_Column UCResponsible = new UserControlImport_Column();
            DiversityCollection.Import_Column ICResponsible = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ResponsibleName", UCResponsible);
            ICResponsible.CanBeTransformed = true;
            ICResponsible.TypeOfEntry = Import_Column.EntryType.Text;
            ICResponsible.TypeOfFixing = Import_Column.FixingType.Schema;
            ICResponsible.TypeOfSource = Import_Column.SourceType.Any;
            ICResponsible.MultiColumn = true;
            UCResponsible.initUserControl(ICResponsible, this._Import);
            UCResponsible.Dock = DockStyle.Top;
            T.Controls.Add(UCResponsible);
        }

        private void AddStepControlsReference(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        {
            DiversityCollection.UserControls.UserControlImport_Column UCNotes = new UserControlImport_Column();
            DiversityCollection.Import_Column ICNotes = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Notes", UCNotes);
            ICNotes.CanBeTransformed = true;
            ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
            ICNotes.TypeOfFixing = Import_Column.FixingType.None;
            ICNotes.TypeOfSource = Import_Column.SourceType.Any;
            UCNotes.initUserControl(ICNotes, this._Import);
            UCNotes.Dock = DockStyle.Top;
            T.Controls.Add(UCNotes);

            DiversityCollection.UserControls.UserControlImport_Column UCTypeNotes = new UserControlImport_Column();
            DiversityCollection.Import_Column ICTypeNotes = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "TypeNotes", UCNotes);
            ICTypeNotes.CanBeTransformed = true;
            ICTypeNotes.TypeOfEntry = Import_Column.EntryType.Text;
            ICTypeNotes.TypeOfFixing = Import_Column.FixingType.None;
            ICTypeNotes.TypeOfSource = Import_Column.SourceType.Any;
            UCTypeNotes.initUserControl(ICTypeNotes, this._Import);
            UCTypeNotes.Dock = DockStyle.Top;
            T.Controls.Add(UCTypeNotes);

            DiversityCollection.UserControls.UserControlImport_Column UCTypeStatus = new UserControlImport_Column();
            DiversityCollection.Import_Column ICTypeStatus = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "TypeStatus", UCTypeStatus);
            ICTypeStatus.CanBeTransformed = true;
            ICTypeStatus.MustSelect = false;
            ICTypeStatus.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICTypeStatus.TypeOfFixing = Import_Column.FixingType.Schema;
            ICTypeStatus.TypeOfSource = Import_Column.SourceType.Any;
            ICTypeStatus.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollTypeStatus_Enum", true, true, true), "DisplayText", "Code");
            ICTypeStatus.DisplayColumn = "DisplayText";
            ICTypeStatus.ValueColumn = "Code";
            UCTypeStatus.initUserControl(ICTypeStatus, this._Import);
            UCTypeStatus.Dock = DockStyle.Top;
            T.Controls.Add(UCTypeStatus);

            DiversityCollection.UserControls.UserControlImport_Column UCReference = new UserControlImport_Column();
            DiversityCollection.Import_Column ICReference = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ReferenceTitle", UCReference);
            ICReference.CanBeTransformed = true;
            ICReference.TypeOfEntry = Import_Column.EntryType.Text;
            ICReference.TypeOfFixing = Import_Column.FixingType.Schema;
            ICReference.TypeOfSource = Import_Column.SourceType.Any;
            UCReference.initUserControl(ICReference, this._Import);
            UCReference.Dock = DockStyle.Top;
            T.Controls.Add(UCReference);
        }
        
        #endregion


    }
}
