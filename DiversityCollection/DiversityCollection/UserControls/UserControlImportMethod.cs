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
    public partial class UserControlImportMethod : UserControl, iUserControlImportInterface
    {
        #region Parameter

        private DiversityCollection.Import _Import;
        private string _StepKey;
        private DiversityCollection.Import_Step _SuperiorImportStep;
        private DiversityCollection.Import_Step _ImportStepMethod;
        private DiversityCollection.iImportInterface _iImportInterface;
        private string _MethodTableName;
        private int? _MethodID;

        private string _ParameterTableName;

        System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> _ImportSteps;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPages;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabControl> _TabContols;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesMethod;
        System.Collections.Generic.SortedList<string, System.Windows.Forms.TabPage> _TabPagesParameter;

        System.Collections.Generic.List<DiversityCollection.Import_Column> _ParameterMethodIDColumns;

        #endregion

        #region Construction
        
        public UserControlImportMethod()
        {
            InitializeComponent();
            this.userControlImport_Column_Method.comboBoxForAll.SelectionChangeCommitted += this.setMethodID;
        }
        
        #endregion

        #region Interface

        public string MethodTableName
        {
            get
            {
                if (this._MethodTableName != null && this._MethodTableName.Length > 0)
                    return this._MethodTableName;

                this._MethodTableName = "";
                switch (this._SuperiorImportStep.TableName())
                {
                    case "CollectionEvent":
                        this._MethodTableName = "CollectionEventMethod";
                        break;
                    case "CollectionSpecimenProcessing":
                        this._MethodTableName = "CollectionSpecimenProcessingMethod";
                        break;
                    case "IdentificationUnitAnalysis":
                        this._MethodTableName = "IdentificationUnitAnalysisMethod";
                        break;
                }
                return this._MethodTableName;
            }
        }

        public string ParameterTableName
        {
            get
            {
                if (this._MethodTableName != null && this._MethodTableName.Length > 0)
                    return this._MethodTableName;

                this._MethodTableName = "";
                switch (this._SuperiorImportStep.TableName())
                {
                    case "CollectionEvent":
                        this._MethodTableName = "CollectionEventParameterValue";
                        break;
                    case "CollectionSpecimenProcessing":
                        this._MethodTableName = "CollectionSpecimenProcessingMethodParameter";
                        break;
                    case "IdentificationUnitAnalysis":
                        this._MethodTableName = "IdentificationUnitAnalysisMethodParameter";
                        break;
                }
                return this._MethodTableName;
            }
        }

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
            this.panelSelectionParameter.Controls.Clear();

            this._ImportSteps = null;
            this._TabPagesParameter = null;
            this._TabPagesMethod = null;
        }

        public string StepKey() { return this._StepKey; }
        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return this.panelSelectionParameter; }

        public void UpdateSelectionPanel()
        {
            if (this.panelSelectionParameter.Controls.Count == 0)
                return;
            System.Collections.Generic.SortedDictionary<string, DiversityCollection.UserControls.UserControlImportSelector> Selectors = new SortedDictionary<string, UserControlImportSelector>();
            foreach (System.Object o in this.panelSelectionParameter.Controls)
            {
                if (o.GetType() == typeof(DiversityCollection.UserControls.UserControlImportSelector))
                {
                    DiversityCollection.UserControls.UserControlImportSelector S = (DiversityCollection.UserControls.UserControlImportSelector)o;
                    Selectors.Add(S.ImportSteps()[0].StepKey(), S);
                }
            }
            this.panelSelectionParameter.Controls.Clear();
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.UserControls.UserControlImportSelector> KV in Selectors)
                {
                    this.panelSelectionParameter.Controls.Add(KV.Value);
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
            this.toolTip.SetToolTip(this.buttonAdd, "Add a new parameter");//" + this._SuperiorImportStep.TableName());
            this.toolTip.SetToolTip(this.buttonRemove, "Hide the selected parameter");// " + this._SuperiorImportStep.TableName());
            this.toolTip.SetToolTip(this.buttonRecover, "Show any hidden parameter");// " + this._SuperiorImportStep.TableName());
        }

        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
            // setting the superior controls
            if (ImportStep.SuperiorImportStep != null)
            {
                // wird nicht bei Method von Event aufgerufen, nur bei Parameter
                // "_02_14_00:01"; "_02_14"
                // "_08:01_02:01_00_00:01"; "_08:01_02:01_00"
                ImportStep.SuperiorImportStep.getUserControlImportInterface().showStepControls(ImportStep.SuperiorImportStep);
            }

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
                            if (Key == kvTC.Key && this._TabPagesParameter.ContainsKey(Key))
                            {
                                kvTC.Value.TabPages.Add(this._TabPagesParameter[Key]);
                                ControlsAdded = true;
                            }
                            //else // Adding the controls of dependent data like identification of analysis if one of those where selected
                            //{
                            //    if (ImportStep.StepDetailKey() == (int)DiversityCollection.Import.ImportStepEventMethod.Parameter)
                            //    {
                            //        foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.TabPage> kvI in this._TabPagesParameter)
                            //        {
                            //            if (Key.StartsWith(kvI.Key))
                            //            {
                            //                kvTC.Value.TabPages.Add(kvI.Value);

                            //                // zum Test
                            //                //System.Windows.Forms.TabControl TC = (System.Windows.Forms.TabControl)kvTC.Value.Controls[0];
                            //                //TC.TabPages.Add(kvI.Value);
                            //                //System.Windows.Forms.TabPage TX = new TabPage(Key + " U");
                            //                //kvTC.Value.TabPages.Add(TX);


                            //                ControlsAdded = true;
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
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

        public DiversityCollection.Import_Step ImportStepMethod
        {
            set 
            {
                this._ImportStepMethod = value;
            }
            //switch (this._SuperiorImportStep.TableName())
            //{
            //    case "CollectionEvent":
            //        if (this._ImportStepMethod == null)
            //        {
            //            this._ImportStepMethod = DiversityCollection.Import_Step.GetImportStep(
            //        }
            //        break;
            //    case "CollectionSpecimenProcessing":
            //        break;
            //    case "IdentificationUnitAnalysis":
            //        break;
            //}
            //return this._ImportStepMethod;
        }

        public void AddImportStep()
        {
            //int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this, (int)Import.ImportStepUnit.Identification);
            int NextImportStepNumber = 1;
            switch (this._SuperiorImportStep.TableName())
            {
                case "CollectionEvent":
                    NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._ImportStepMethod, (int)Import.ImportStepEvent.Method);
                    //StepKey = DiversityCollection.Import.getImportStepKey(this._StepKey, NextImportStepNumber);
                    break;
                case "CollectionSpecimenProcessing":
                    break;
                case "IdentificationUnitAnalysis":
                    //NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._ImportStepMethod, (int)Import.ImportStepUnitAnalysisMethod.Method);
                    break;
            }

            string SQL = "SELECT MethodID, DisplayText " +
                "FROM Method " +
                "WHERE (OnlyHierarchy = 0 or OnlyHierarchy IS NULL) ";
            switch (this._SuperiorImportStep.TableName())
            {
                case "CollectionEvent":
                    SQL += " AND (ForCollectionEvent = 1) ";
                    break;
                case "CollectionSpecimenProcessing":
                    SQL += " AND (MethodID IN(SELECT MethodID FROM MethodForProcessing)) ";
                    break;
                case "IdentificationUnitAnalysis":
                    SQL += " AND (MethodID IN(SELECT MethodID FROM MethodForAnalysis)) ";
                    break;
            }

            SQL += " ORDER BY DisplayText";
            System.Data.DataTable dtMethod = new DataTable();
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMethod);

            DiversityCollection.Import_Column ICMethod;

            switch (this._SuperiorImportStep.TableName())
            {
                //case "CollectionEvent":
                //    ICMethod = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Method)
                //    , "CollectionEventMethod", "MethodID", this.userControlImport_Column_Method);                    
                //    break;
                //case "CollectionSpecimenProcessing":
                //    SQL += " AND (MethodID IN(SELECT MethodID FROM MethodForProcessing)) ";
                //    break;
                case "IdentificationUnitAnalysis":
                    ICMethod = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Method)
                    , "IdentificationUnitAnalysisMethod", "MethodID", this.userControlImport_Column_Method);
                    break;
                default:
                    ICMethod = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Method)
                    , "CollectionEventMethod", "MethodID", this.userControlImport_Column_Method);
                    break;
            }

            ICMethod.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            ICMethod.TypeOfFixing = Import_Column.FixingType.Schema;
            ICMethod.TypeOfSource = Import_Column.SourceType.Interface;
            ICMethod.setLookupTable(dtMethod, "DisplayText", "MethodID");
            ICMethod.MustSelect = true;
            ICMethod.IsSelected = true;
            this.userControlImport_Column_Method.initUserControl(ICMethod, this._Import);
            this.userControlImport_Column_Method.setInterface();

            switch (this._SuperiorImportStep.TableName())
            {
                case "CollectionEvent":
                    DiversityCollection.Import_Column ICCollectionEventID = DiversityCollection.Import_Column.GetImportColumn(this._ImportStepMethod.StepKey(), this._ImportStepMethod.TableName(), this._ImportStepMethod.TableAlias(), "CollectionEventID", 1, null
                        , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                    ICCollectionEventID.IsSelected = true;
                    ICCollectionEventID.CanBeTransformed = false;
                    ICCollectionEventID.ParentTableAlias(this._SuperiorImportStep.TableAlias());
                    break;
                case "CollectionSpecimenProcessing":
                    break;
                case "IdentificationUnitAnalysis":
                    DiversityCollection.Import_Column ICUnitID = DiversityCollection.Import_Column.GetImportColumn(this._ImportStepMethod.StepKey(), this._ImportStepMethod.TableName(), this._ImportStepMethod.TableAlias(), "IdentificationUnitID", 1, null
                        , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                    ICUnitID.IsSelected = true;
                    ICUnitID.CanBeTransformed = false;
                    ICUnitID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

                    DiversityCollection.Import_Column ICSpecimenID = DiversityCollection.Import_Column.GetImportColumn(this._ImportStepMethod.StepKey(), this._ImportStepMethod.TableName(), this._ImportStepMethod.TableAlias(), "CollectionSpecimenID", 1, null
                        , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                    ICSpecimenID.IsSelected = true;
                    ICSpecimenID.CanBeTransformed = false;
                    ICSpecimenID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

                    DiversityCollection.Import_Column ICAnalysisID = DiversityCollection.Import_Column.GetImportColumn(this._ImportStepMethod.StepKey(), this._ImportStepMethod.TableName(), this._ImportStepMethod.TableAlias(), "AnalysisID", 1, null
                        , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                    ICAnalysisID.IsSelected = true;
                    ICAnalysisID.CanBeTransformed = false;
                    ICAnalysisID.ParentTableAlias(this._SuperiorImportStep.TableAlias());

                    DiversityCollection.Import_Column ICAnalysisNumber = DiversityCollection.Import_Column.GetImportColumn(this._ImportStepMethod.StepKey(), this._ImportStepMethod.TableName(), this._ImportStepMethod.TableAlias(), "AnalysisNumber", 1, null
                        , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                    ICAnalysisNumber.IsSelected = true;
                    ICAnalysisNumber.CanBeTransformed = false;
                    ICAnalysisNumber.ParentTableAlias(this._SuperiorImportStep.TableAlias());

                    break;
                default:
                    break;
            }

            this.AddImportStepParameter();

            //try
            //{
            //    // the tabpage for the first parameter
            //    string Title = "Parameter " + (NextImportStepNumber).ToString();
            //    System.Windows.Forms.TabPage TStep = new TabPage(Title);
            //    // the tabcontrol for the details
            //    //System.Windows.Forms.TabControl TControlParameter = new TabControl();
            //    //TControlParameter.Tag = Title;
            //    //TControlParameter.Dock = DockStyle.Fill;
            //    //TStep.Controls.Add(TControlParameter);
            //    string StepKey = "";
            //    //string StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepEvent.Method, this._ImportStepMethod, NextImportStepNumber);
            //    switch (this._SuperiorImportStep.TableName())
            //    {
            //        case "CollectionEvent":
            //            StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepEvent.Method, this._ImportStepMethod, NextImportStepNumber);
            //            break;
            //        case "CollectionSpecimenProcessing":
            //            break;
            //        case "IdentificationUnitAnalysis":
            //            break;
            //        default:
            //            break;
            //    }
            //    DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
            //        "Parameter " + NextImportStepNumber.ToString(),
            //        "Parameter " + NextImportStepNumber.ToString(),
            //        StepKey,
            //        this.MethodTableName,
            //        NextImportStepNumber,
            //        this._ImportStepMethod,
            //        2,
            //        (iUserControlImportInterface)this,
            //        null,
            //        this.panelSelectionParameter);

            //    // nur kontrolle
            //    this._StepKey = StepKey;

            //    //System.Windows.Forms.TabPage TPageParameter = new TabPage("Parameter");
            //    this.AddStepControlsParameter(TStep, IS);
            //    //this.AddStepControls(TPageParameter, IS);
            //    if (this._TabPagesParameter == null)
            //        this._TabPagesParameter = new SortedList<string, TabPage>();
            //    this._TabPagesParameter.Add(IS.StepKey(), TStep);

            //    //this.initParameter(TStep, IS);

            //    //if (this._TabPagesMethod == null)
            //    //    this._TabPagesMethod = new SortedList<string, TabPage>();
            //    //this._TabPagesMethod.Add(IS.StepKey(), TPageParameter);

            //    if (this._ImportSteps == null)
            //        this._ImportSteps = new SortedList<string, Import_Step>();
            //    this._ImportSteps.Add(IS.StepKey(), IS);

            //    if (this._TabPages == null)
            //        this._TabPages = new SortedList<string, TabPage>();
            //    this._TabPages.Add(IS.StepKey(), TStep);

            //    if (this._TabContols == null)
            //        this._TabContols = new SortedList<string, TabControl>();


            //   // this._TabContols.Add(IS.StepKey(), TControlParameter);
            //    this._TabContols.Add(IS.StepKey(), this.tabControlImportSteps);

            //    //System.Windows.Forms.TabPage TPageParameter = new TabPage();
            //    //TPageParameter.ImageIndex = this.tabPageParameter.ImageIndex;
            //    //// Test
            //    //TPageParameter.Text = this.tabPageParameter.Text;// +" " + IS.StepKey();

            //    // alle Tab pages des User Controls - hier landen alle Identifications aller Units
            //    //if (this._TabPagesParameter == null)
            //    //    this._TabPagesParameter = new SortedList<string, TabPage>();
            //    //this._TabPagesParameter.Add(IS.StepKey(), TPageParameter);
            //    //this.initParameter(TPageParameter, IS);

            //    //IS.UserControlImportStep.IsCurrent = true;
            //    IS.setStepError();
            //}
            //catch (System.Exception ex) { }

        }

        public void AddImportStepParameter()
        {
            int NextImportStepNumber = 1;
            switch (this._SuperiorImportStep.TableName())
            {
                case "CollectionEvent":
                    NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._ImportStepMethod, (int)Import.ImportStepEventMethod.Parameter);
                    break;
                case "CollectionSpecimenProcessing":
                    break;
                case "IdentificationUnitAnalysis":
                    //NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(this._ImportStepMethod, (int)Import.ImportStepUnitAnalysisMethodParameter.Parameter);
                    break;
            }

            try
            {
                string Title = "Parameter " + (NextImportStepNumber).ToString();
                System.Windows.Forms.TabPage TStep = new TabPage(Title);
                TStep.ImageIndex = 0;
                string StepKey = "";
                switch (this._SuperiorImportStep.TableName())
                {
                    case "CollectionEvent":
                        StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepEventMethod.Parameter, this._ImportStepMethod, NextImportStepNumber);
                        // "_02_14_00:01"
                        break;
                    case "CollectionSpecimenProcessing":
                        break;
                    case "IdentificationUnitAnalysis":
                        //StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepUnitAnalysisMethodParameter.Parameter, this._ImportStepMethod, NextImportStepNumber);
                        // "_08:01_02:01_00_00:01"
                        break;
                    default:
                        break;
                }
                byte Level = (byte)(this._ImportStepMethod.Level + 1);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Parameter " + NextImportStepNumber.ToString(),
                    "Parameter " + NextImportStepNumber.ToString(),
                    StepKey,
                    this.ParameterTableName,
                    NextImportStepNumber,
                    this._ImportStepMethod,
                    Level,
                    (iUserControlImportInterface)this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Parameter),
                    this.panelSelectionParameter);

                //IS.IsVisible(false);
                //IS.CanHide(true);

                // nur kontrolle
                this._StepKey = StepKey;
                // "_08:01_02:01_00_00:01"

                this.AddStepControlsParameter(TStep, IS);
                if (this._TabPagesParameter == null)
                    this._TabPagesParameter = new SortedList<string, TabPage>();
                this._TabPagesParameter.Add(IS.StepKey(), TStep);

                if (this._ImportSteps == null)
                    this._ImportSteps = new SortedList<string, Import_Step>();
                this._ImportSteps.Add(IS.StepKey(), IS);

                if (this._TabPages == null)
                    this._TabPages = new SortedList<string, TabPage>();
                this._TabPages.Add(IS.StepKey(), TStep);

                if (this._TabContols == null)
                    this._TabContols = new SortedList<string, TabControl>();

                this._TabContols.Add(IS.StepKey(), this.tabControlImportSteps);

                IS.setStepError();
            }
            catch (System.Exception ex) { }
        }

        public void AddImportStep(string StepKey)
        {
            try
            {
                int MethodNumber = DiversityCollection.Import_Step.StepKeyPartParallelNumber(StepKey, 0);
                string MethodKey = DiversityCollection.Import_Step.getImportStepKey(Import.ImportStepEvent.Method, MethodNumber);
                string FirstStepKey = DiversityCollection.Import_Step.getImportStepKey(StepKey, 1);
                DiversityCollection.Import_Step S = DiversityCollection.Import_Step.GetImportStep(FirstStepKey);
                if (!DiversityCollection.Import.ImportSteps.ContainsKey(MethodKey))
                {
                    this.AddImportStep();
                }
                if (DiversityCollection.Import.ImportSteps.ContainsKey(MethodKey)
                    && !DiversityCollection.Import.ImportSteps.ContainsKey(StepKey))
                {
                    DiversityCollection.Import.ImportStepUnit I = DiversityCollection.Import_Step.ImportStepUnitKey(StepKey);
                    switch (I)
                    {
                        case Import.ImportStepUnit.Identification:
                            if (S != null)
                                S.getUserControlImportInterface().AddImportStep(StepKey);
                            else
                            {
                                //this.userControlImportMethodParameter.AddImportStep(StepKey);
                            }
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

            this.AddImportStepParameter();
            //this.AddImportStep();

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

        public int? MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }

        private void setMethodID(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.userControlImport_Column_Method.comboBoxForAll.SelectedItem;
            this.MethodID = int.Parse(R["MethodID"].ToString());
            if (this._ParameterMethodIDColumns == null)
                this._ParameterMethodIDColumns = new List<Import_Column>();
                System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT ParameterID, DisplayText " +
                "FROM Parameter " +
                "WHERE MethodID = " + MethodID.ToString() +
                "ORDER BY DisplayText";
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            foreach (DiversityCollection.Import_Column IC in this._ParameterMethodIDColumns)
            {
                IC.setLookupTable(dt, "DisplayText", "ParameterID");
                DiversityCollection.UserControls.UserControlImport_Column UC = (DiversityCollection.UserControls.UserControlImport_Column)IC.ImportColumnControl;
                UC.initUserControl(IC, this._Import);
            }
        }

        private void AddStepControlsParameter(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step ImportStep)
        {
            try
            {
                DiversityCollection.UserControls.UserControlImport_Column UCValue = new UserControlImport_Column();
                DiversityCollection.Import_Column ICValue = DiversityCollection.Import_Column.GetImportColumn(
                    ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "Value", UCValue);
                ICValue.CanBeTransformed = true;
                ICValue.MustSelect = true;
                ICValue.TypeOfEntry = Import_Column.EntryType.Text;
                ICValue.TypeOfFixing = Import_Column.FixingType.Schema;
                ICValue.TypeOfSource = Import_Column.SourceType.Any;
                UCValue.initUserControl(ICValue, this._Import);
                UCValue.Dock = DockStyle.Top;
                T.Controls.Add(UCValue);

                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT ParameterID, DisplayText " +
                    "FROM Parameter ";
                if (this._MethodID != null)
                    SQL += "WHERE MethodID = " + this._MethodID.ToString();
                SQL += " ORDER BY DisplayText";
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);

                DiversityCollection.UserControls.UserControlImport_Column UCParameterID = new UserControlImport_Column();
                DiversityCollection.Import_Column ICParameterID = DiversityCollection.Import_Column.GetImportColumn
                    (ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "ParameterID", UCParameterID);
                ICParameterID.CanBeTransformed = true;
                ICParameterID.TypeOfEntry = Import_Column.EntryType.MandatoryList;
                ICParameterID.TypeOfFixing = Import_Column.FixingType.Schema;
                ICParameterID.TypeOfSource = Import_Column.SourceType.Interface;
                ICParameterID.MustSelect = true;
                ICParameterID.setLookupTable(dt, "DisplayText", "ParameterID");
                UCParameterID.initUserControl(ICParameterID, this._Import);
                UCParameterID.Dock = DockStyle.Top;
                T.Controls.Add(UCParameterID);

                if (this._ParameterMethodIDColumns == null)
                    this._ParameterMethodIDColumns = new List<Import_Column>();
                this._ParameterMethodIDColumns.Add(ICParameterID);

                DiversityCollection.Import_Column ICMethodID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "MethodID", 1, null
                    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                ICMethodID.IsSelected = true;
                ICMethodID.CanBeTransformed = false;
                ICMethodID.ParentTableAlias(this._ImportStepMethod.TableAlias());

                switch (this._SuperiorImportStep.TableName())
                {
                    case "CollectionEvent":
                        DiversityCollection.Import_Column ICCollectionEventID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionEventID", 1, null
                            , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                        ICCollectionEventID.IsSelected = true;
                        ICCollectionEventID.CanBeTransformed = false;
                        ICCollectionEventID.ParentTableAlias(this._ImportStepMethod.TableAlias());
                        break;
                    case "CollectionSpecimenProcessing":
                        break;
                    case "IdentificationUnitAnalysis":
                        DiversityCollection.Import_Column ICCollectionSpecimenID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "CollectionSpecimenID", 1, null
                            , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                        ICCollectionSpecimenID.IsSelected = true;
                        ICCollectionSpecimenID.CanBeTransformed = false;
                        ICCollectionSpecimenID.ParentTableAlias(this._ImportStepMethod.TableAlias());

                        DiversityCollection.Import_Column ICIdentificationUnitID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "IdentificationUnitID", 1, null
                            , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                        ICIdentificationUnitID.IsSelected = true;
                        ICIdentificationUnitID.CanBeTransformed = false;
                        ICIdentificationUnitID.ParentTableAlias(this._ImportStepMethod.TableAlias());

                        DiversityCollection.Import_Column ICAnalysisID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AnalysisID", 1, null
                            , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                        ICAnalysisID.IsSelected = true;
                        ICAnalysisID.CanBeTransformed = false;
                        ICAnalysisID.ParentTableAlias(this._ImportStepMethod.TableAlias());

                        DiversityCollection.Import_Column ICAnalysisNumber = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), ImportStep.TableName(), ImportStep.TableAlias(), "AnalysisNumber", 1, null
                            , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                        ICAnalysisNumber.IsSelected = true;
                        ICAnalysisNumber.CanBeTransformed = false;
                        ICAnalysisNumber.ParentTableAlias(this._ImportStepMethod.TableAlias());

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        private void AddStepControls(System.Windows.Forms.TabPage TP, DiversityCollection.Import_Step ImportStep)//, string TableAlias, int NextStepNumber)
        {
            try
            {
                DiversityCollection.UserControls.UserControlImport_Column UCParameterID = new UserControlImport_Column();
                DiversityCollection.Import_Column ICParameterID;
                switch (this._SuperiorImportStep.TableName())
                {
                    //case "CollectionEvent":
                    //    ICParameterID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), "IdentificationUnit", ImportStep.TableAlias(), "ColonisedSubstratePart", UCColonisedSubstratePart);
                    //    break;
                    //case "CollectionSpecimenProcessing":
                    //    break;
                    //case "IdentificationUnitAnalysis":
                    //    break;
                    default:
                        ICParameterID = DiversityCollection.Import_Column.GetImportColumn(ImportStep.StepKey(), "CollectionEventParameterValue", ImportStep.TableAlias(), "ParameterID", UCParameterID);
                        break;
                }
                ICParameterID.CanBeTransformed = true;
                ICParameterID.TypeOfEntry = Import_Column.EntryType.Text;
                ICParameterID.TypeOfFixing = Import_Column.FixingType.Schema;
                ICParameterID.TypeOfSource = Import_Column.SourceType.Any;
                UCParameterID.initUserControl(ICParameterID, this._Import);
                UCParameterID.Dock = DockStyle.Top;
                TP.Controls.Add(UCParameterID);

                
                //System.Windows.Forms.TabControl TC = new TabControl();
                //TP.Controls.Add(TC);
                //TC.Dock = DockStyle.Fill;

                //System.Windows.Forms.TabPage T = new TabPage("Method");
                //TC.TabPages.Add(T);

                //string StepKey = ImportStep.StepKey();// DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStep.Organism, null, NextStepNumber); //DiversityCollection.Import.getImportStepKey(Import.ImportStep.Organism, this._Import.LastStepNumber(DiversityCollection.Import.ImportStep.Organism));

                //// RELATION
                //if (!this._StepKey.EndsWith("01"))
                //{
                //    System.Windows.Forms.TabPage T3 = new TabPage("Relation");
                //    DiversityCollection.UserControls.UserControlImport_Column UCColonisedSubstratePart = new UserControlImport_Column();
                //    TC.TabPages.Add(T3);

                //    DiversityCollection.Import_Column ICColonisedSubstratePart =
                //    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "ColonisedSubstratePart", UCColonisedSubstratePart);
                //    ICColonisedSubstratePart.CanBeTransformed = true;
                //    ICColonisedSubstratePart.TypeOfEntry = Import_Column.EntryType.Text;
                //    ICColonisedSubstratePart.TypeOfFixing = Import_Column.FixingType.Schema;
                //    ICColonisedSubstratePart.TypeOfSource = Import_Column.SourceType.Any;
                //    UCColonisedSubstratePart.initUserControl(ICColonisedSubstratePart, this._Import);
                //    UCColonisedSubstratePart.Dock = DockStyle.Top;
                //    T3.Controls.Add(UCColonisedSubstratePart);

                //    DiversityCollection.UserControls.UserControlImport_Column UCRelationType = new UserControlImport_Column();
                //    DiversityCollection.Import_Column ICRelationType = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "RelationType", UCRelationType);
                //    ICRelationType.CanBeTransformed = true;
                //    ICRelationType.MustSelect = false;
                //    ICRelationType.TypeOfEntry = Import_Column.EntryType.MandatoryList;
                //    ICRelationType.TypeOfFixing = Import_Column.FixingType.Schema;
                //    ICRelationType.TypeOfSource = Import_Column.SourceType.Any;
                //    ICRelationType.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollUnitRelationType_Enum", true, true, true), "DisplayText", "Code");
                //    ICRelationType.DisplayColumn = "DisplayText";
                //    ICRelationType.ValueColumn = "Code";
                //    UCRelationType.initUserControl(ICRelationType, this._Import);
                //    UCRelationType.Dock = DockStyle.Top;
                //    T3.Controls.Add(UCRelationType);

                //    DiversityCollection.UserControls.UserControlImport_Column UCRelatedUnitID = new UserControlImport_Column();
                //    DiversityCollection.Import_Column ICRelatedUnitID = DiversityCollection.Import_Column.GetImportColumn(StepKey, ImportStep.TableName(), ImportStep.TableAlias(), "RelatedUnitID", 1, null
                //    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                //    ICRelatedUnitID.IsSelected = false;
                //    ICRelatedUnitID.CanBeTransformed = false;
                //    ICRelatedUnitID.ParentTableAlias(this._SuperiorImportStep.TableAlias());
                //    UCRelatedUnitID.initUserControl(ICRelatedUnitID, this._Import);
                //    UCRelatedUnitID.Dock = DockStyle.Top;
                //    string Title = "Growing on Organism " + this._SuperiorImportStep.TableAlias().Replace("IdentificationUnit", "");
                //    UCRelatedUnitID.setTitle(Title);
                //    T3.Controls.Add(UCRelatedUnitID);
                //}

                //// NOTES


                //DiversityCollection.UserControls.UserControlImport_Column UCNumberOfUnits = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICNumberOfUnits =
                //    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "NumberOfUnits", UCNumberOfUnits);
                //ICNumberOfUnits.CanBeTransformed = true;
                //ICNumberOfUnits.TypeOfEntry = Import_Column.EntryType.Text;
                //ICNumberOfUnits.TypeOfFixing = Import_Column.FixingType.Schema;
                //ICNumberOfUnits.TypeOfSource = Import_Column.SourceType.Any;
                //UCNumberOfUnits.initUserControl(ICNumberOfUnits, this._Import);
                //UCNumberOfUnits.Dock = DockStyle.Top;
                //T.Controls.Add(UCNumberOfUnits);

                //DiversityCollection.UserControls.UserControlImport_Column UCExsiccataNumber = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICExsiccataNumber =
                //    DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "ExsiccataNumber", UCExsiccataNumber);
                //ICExsiccataNumber.CanBeTransformed = true;
                //ICExsiccataNumber.TypeOfEntry = Import_Column.EntryType.Text;
                //ICExsiccataNumber.TypeOfFixing = Import_Column.FixingType.Schema;
                //ICExsiccataNumber.TypeOfSource = Import_Column.SourceType.Any;
                //UCExsiccataNumber.initUserControl(ICExsiccataNumber, this._Import);
                //UCExsiccataNumber.Dock = DockStyle.Top;
                //T.Controls.Add(UCExsiccataNumber);


                //// TAXONOMY

                //DiversityCollection.UserControls.UserControlImport_Column UCFamily = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICFamily = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "FamilyCache", UCFamily);
                //ICFamily.CanBeTransformed = true;
                //ICFamily.TypeOfEntry = Import_Column.EntryType.Text;
                //ICFamily.TypeOfFixing = Import_Column.FixingType.Schema;
                //ICFamily.TypeOfSource = Import_Column.SourceType.Any;
                //UCFamily.initUserControl(ICFamily, this._Import);
                //UCFamily.Dock = DockStyle.Top;
                //T.Controls.Add(UCFamily);

                //DiversityCollection.UserControls.UserControlImport_Column UCOrder = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICOrder = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "OrderCache", UCOrder);
                //ICOrder.CanBeTransformed = true;
                //ICOrder.TypeOfEntry = Import_Column.EntryType.Text;
                //ICOrder.TypeOfFixing = Import_Column.FixingType.Schema;
                //ICOrder.TypeOfSource = Import_Column.SourceType.Any;
                //UCOrder.initUserControl(ICOrder, this._Import);
                //UCOrder.Dock = DockStyle.Top;
                //T.Controls.Add(UCOrder);

                //DiversityCollection.UserControls.UserControlImport_Column UCHierarchyCache = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICHierarchyCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "HierarchyCache", UCHierarchyCache);
                //ICHierarchyCache.CanBeTransformed = true;
                //ICHierarchyCache.MultiColumn = true;
                //ICHierarchyCache.TypeOfEntry = Import_Column.EntryType.Text;
                //ICHierarchyCache.TypeOfFixing = Import_Column.FixingType.Schema;
                //ICHierarchyCache.TypeOfSource = Import_Column.SourceType.Any;
                //UCHierarchyCache.initUserControl(ICHierarchyCache, this._Import);
                //UCHierarchyCache.Dock = DockStyle.Top;
                //T.Controls.Add(UCHierarchyCache);

                //DiversityCollection.UserControls.UserControlImport_Column UCIdentifier = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICIdentifier = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "UnitIdentifier", UCIdentifier);
                //ICIdentifier.CanBeTransformed = true;
                //ICIdentifier.TypeOfEntry = Import_Column.EntryType.Text;
                //ICIdentifier.TypeOfFixing = Import_Column.FixingType.None;
                //ICIdentifier.TypeOfSource = Import_Column.SourceType.File;
                //UCIdentifier.initUserControl(ICIdentifier, this._Import);
                //UCIdentifier.Dock = DockStyle.Top;
                ////UCNumber.SendToBack();
                //T.Controls.Add(UCIdentifier);

                //DiversityCollection.UserControls.UserControlImport_Column UCTaxonomicGroup = new UserControlImport_Column();
                //DiversityCollection.Import_Column ICTaxonomicGroup = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "TaxonomicGroup", UCTaxonomicGroup);
                //ICTaxonomicGroup.CanBeTransformed = true;
                //ICTaxonomicGroup.MustSelect = true;
                //ICTaxonomicGroup.TypeOfEntry = Import_Column.EntryType.MandatoryList;
                //ICTaxonomicGroup.TypeOfFixing = Import_Column.FixingType.Schema;
                //ICTaxonomicGroup.TypeOfSource = Import_Column.SourceType.Any;
                //ICTaxonomicGroup.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollTaxonomicGroup_Enum", true, true, true), "DisplayText", "Code");
                //ICTaxonomicGroup.DisplayColumn = "DisplayText";
                //ICTaxonomicGroup.ValueColumn = "Code";
                //UCTaxonomicGroup.initUserControl(ICTaxonomicGroup, this._Import);
                //UCTaxonomicGroup.Dock = DockStyle.Top;
                ////UColl.SendToBack();
                //T.Controls.Add(UCTaxonomicGroup);


                //UCTaxonomicGroup.setInterface();

                //DiversityCollection.Import_Column ICLastIdentificationCache = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnit", ImportStep.TableAlias(), "LastIdentificationCache", null);
                //ICLastIdentificationCache.CanBeTransformed = false;
                ////ICLastIdentificationCache.MustSelect = true;
                //ICLastIdentificationCache.IsSelected = true;
                //ICLastIdentificationCache.ValueIsFixed = true;
                //ICLastIdentificationCache.TypeOfSource = Import_Column.SourceType.Database;
                //ICLastIdentificationCache.Value = "Organism";
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Parameter

        private void initParameter(System.Windows.Forms.TabPage T, DiversityCollection.Import_Step IS)
        {
            try
            {
                //int i = T.Controls.Count;
                //// test - hier gibt es nur ein userControlImportIdentification fuer alle
                ////this._SuperiorImportStep = IS;
                ////T.Controls.Add(this.userControlImportIdentification);
                ////this.userControlImportIdentification.initUserControl(this._iImportInterface, IS);
                ////this.userControlImportIdentification.Dock = DockStyle.Fill;
                //// TODO - bei allen anpassen
                //// test - geaendert zu vielen - passt
                //DiversityCollection.UserControls.UserControlImportIdentification UI = new UserControlImportIdentification();
                //this._SuperiorImportStep = IS;
                //T.Controls.Add(UI);
                //UI.initUserControl(this._iImportInterface, IS);
                //UI.Dock = DockStyle.Fill;
            }
            catch (System.Exception ex) { }
        }

        #endregion



    }
}
