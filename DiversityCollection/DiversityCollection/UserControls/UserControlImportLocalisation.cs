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
    public partial class UserControlImportLocalisation : UserControl, iUserControlImportInterface
    {
        #region Parameter

        private int _LocalisationSystemID;
        private string _LocalisationSystem;

        private DiversityCollection.FormImportWizard _FormImportWizard;
        private DiversityCollection.Import _Import;
        private string _ParsingMethodName;
        private System.Data.DataTable _DtRecordingMethods;
        private string _DisplayTitle1;

        public string DisplayTitle1
        {
            get 
            {
                if (this._DisplayTitle1 == null)
                {
                    System.Data.DataTable dtLocalisationSystem = DiversityCollection.LookupTable.DtLocalisationSystem;
                    System.Data.DataRow[] R = dtLocalisationSystem.Select("LocalisationSystemID = " + this._LocalisationSystemID.ToString());
                    if (R.Length > 0)
                    {
                        this._DisplayTitle1 = R[0]["DescriptionLocation1"].ToString();
                        this._DisplayTitle2 = R[0]["DescriptionLocation2"].ToString();
                    }
                }
                return _DisplayTitle1;
            }
            //set { _DisplayTitle1 = value; }
        }
        private string _DisplayTitle2;

        public string DisplayTitle2
        {
            get { return _DisplayTitle2; }
            //set { _DisplayTitle2 = value; }
        }

        private DiversityCollection.Import_Column _ICLocalisationSystemID;
        //public DiversityCollection.Import_Column ICLocalisationSystemID
        //{
        //    get
        //    {
        //        if (this._ICLocalisationSystemID == null)
        //        {
        //            this._ICLocalisationSystemID = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventLocalisation", "LocalisationSystemID", null);
        //            this._ICLocalisationSystemID.PresetValueColumn = "LocalisationSystemID";
        //            this._ICLocalisationSystemID.PresetValue = this._LocalisationSystemID.ToString();
        //            this._ICLocalisationSystemID.TableAlias = this.TableAlias;
        //            this._ICLocalisationSystemID.TypeOfEntry = Import_Column.EntryType.Text;
        //            this._ICLocalisationSystemID.TypeOfFixing = Import_Column.FixingType.Schema;
        //            this._ICLocalisationSystemID.StepKey = this.StepKey;
        //            this._ICLocalisationSystemID.Value  = this._LocalisationSystemID.ToString();
        //        }
        //        return _ICLocalisationSystemID;
        //    }
        //}

        private DiversityCollection.Import_Column _IC1;
        public DiversityCollection.Import_Column IC1
        {
            get 
            {
                if (this._IC1 == null)
                    this._IC1 = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "Location1", this.userControlImport_Column_Location1);
                if (this._IC1.Table == null)
                {
                    this._IC1 = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", "Location1", this.userControlImport_Column_Location1);
                    this._IC1.PresetValue = this._LocalisationSystemID.ToString();
                    this._IC1.PresetValueColumn = "LocalisationSystemID";
                    this._IC1.TableAlias = this.TableAlias;
                    this._IC1.TypeOfEntry = Import_Column.EntryType.Text;
                    this._IC1.TypeOfFixing = Import_Column.FixingType.None;
                    this._IC1.StepKey = this._StepKey;
                    this._IC1.setDisplayTitle(this.DisplayTitle1);
                }
                if (this._IC1.TypeOfEntry == Import_Column.EntryType.Boolean)
                    this._IC1.TypeOfEntry = Import_Column.EntryType.Text;
                return _IC1; 
            }
            //set { _IC1 = value; }
        }

        private DiversityCollection.Import_Column _IC2;
        public DiversityCollection.Import_Column IC2
        {
            get
            {
                if (this._IC2 == null)
                    this._IC2 = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "Location2", this.userControlImport_Column_Location2);
                if (this._IC2.Table == null)
                {
                    this._IC2 = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "Location2", this.userControlImport_Column_Location2);
                    this._IC2.PresetValue = this._LocalisationSystemID.ToString();
                    this._IC2.PresetValueColumn = "LocalisationSystemID";
                    this._IC2.TableAlias = this.TableAlias;
                    this._IC2.TypeOfEntry = Import_Column.EntryType.Text;
                    this._IC2.TypeOfFixing = Import_Column.FixingType.None;
                    this._IC2.StepKey = this._StepKey;
                    this._IC2.setDisplayTitle(this.DisplayTitle2);
                }
                if (this._IC2.TypeOfEntry == Import_Column.EntryType.Boolean)
                    this._IC2.TypeOfEntry = Import_Column.EntryType.Text;
                return _IC2;
            }
            //set { _IC2 = value; }
        }

        private DiversityCollection.Import_Column _ICRecordingMethod;
        public DiversityCollection.Import_Column ICRecordingMethod
        {
            get 
            {
                if (this._ICRecordingMethod == null)
                {
                    this._ICRecordingMethod = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "RecordingMethod", this.userControlImport_Column_RecordingMethod);
                    this._ICRecordingMethod.PresetValueColumn = "LocalisationSystemID";
                    this._ICRecordingMethod.PresetValue = this._LocalisationSystemID.ToString();
                    this._ICRecordingMethod.TableAlias = this.TableAlias;
                    this._ICRecordingMethod.TypeOfEntry =  Import_Column.EntryType.ListAndText;
                    this._ICRecordingMethod.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICRecordingMethod.StepKey = this._StepKey;
                    this._ICRecordingMethod.setLookupTable(this.DtRecordingMethods,"RecordingMethod","RecordingMethod");
                }
                return _ICRecordingMethod; 
            }
        }

        private DiversityCollection.Import_Column _ICAccuracy;
        public DiversityCollection.Import_Column ICAccuracy
        {
            get 
            {
                if (this._ICAccuracy == null)
                {
                    this._ICAccuracy = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "LocationAccuracy", this.userControlImport_Column_Accuracy);
                    this._ICAccuracy.PresetValueColumn = "LocalisationSystemID";
                    this._ICAccuracy.PresetValue = this._LocalisationSystemID.ToString();
                    this._ICAccuracy.TableAlias = this.TableAlias;
                    this._ICAccuracy.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICAccuracy.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICAccuracy.StepKey = this._StepKey;
                }
                return _ICAccuracy; 
            }
        }

        private DiversityCollection.Import_Column _ICResponsible;
        public DiversityCollection.Import_Column ICResponsible
        {
            get 
            {
                if (this._ICResponsible == null)
                {
                    this._ICResponsible = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "ResponsibleName", this.userControlImport_Column_Accuracy);
                    this._ICResponsible.PresetValueColumn = "LocalisationSystemID";
                    this._ICResponsible.PresetValue = this._LocalisationSystemID.ToString();
                    this._ICResponsible.TableAlias = this.TableAlias;
                    this._ICResponsible.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICResponsible.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICResponsible.StepKey = this._StepKey;
                }
                return _ICResponsible; 
            }
        }

        private DiversityCollection.Import_Column _ICGeography;
        public DiversityCollection.Import_Column ICGeography
        {
            get
            {
                if (this._ICGeography == null)
                {
                    this._ICGeography = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "Geography", this.userControlImport_Column_Geography);
                    this._ICGeography.PresetValueColumn = "LocalisationSystemID";
                    this._ICGeography.PresetValue = this._LocalisationSystemID.ToString();
                    this._ICGeography.TableAlias = this.TableAlias;
                    this._ICGeography.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICGeography.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICGeography.StepKey = this._StepKey;
                }
                return _ICGeography;
            }
        }

        private DiversityCollection.Import_Column _ICLocationNotes;
        public DiversityCollection.Import_Column ICLocationNotes
        {
            get
            {
                if (this._ICLocationNotes == null)
                {
                    this._ICLocationNotes = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "LocationNotes", this.userControlImport_Column_LocationNotes);
                    this._ICLocationNotes.PresetValueColumn = "LocalisationSystemID";
                    this._ICLocationNotes.PresetValue = this._LocalisationSystemID.ToString();
                    this._ICLocationNotes.TableAlias = this.TableAlias;
                    this._ICLocationNotes.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICLocationNotes.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICLocationNotes.StepKey = this._StepKey;
                }
                return _ICLocationNotes;
            }
        }

        private string _StepKey
        {
            get
            {
                /*
                 * LocalisationSystemID	LocalisationSystemName
                    1	Top50 (deutsche Landesvermessung)
                    2	Gauss-Krüger coordinates
                    3	MTB (A, CH, D)
                    4	Altitude (mNN)
                    5	mNN (barometric)
                    6	Greenwich Coordinates
                    7	Named area (DiversityGazetteer)
                    8	Coordinates WGS84
                    9	Coordinates
                    10	Exposition
                    11	Slope
                    12	Coordinates PD
                    13	Sampling plot
                    14	Depth
                    15	Height
                    16	Dutch RD coordinates
                 * */
                string Key = "";
                switch (this._LocalisationSystemID)
                {
                    case 2:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.GaussKrueger);
                        break;
                    case 3:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.MTB);
                        break;
                    case 4:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Altitude);
                        break;
                    case 8:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Coordinates);
                        break;
                    case 7:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Place);
                        break;
                    case 10:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Exposition);
                        break;
                    case 11:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Slope);
                        break;
                    case 13:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Plot);
                        break;
                    case 14:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Depth);
                        break;
                    case 15:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Height);
                        break;
                }
                //if (Key.Length == 1) Key = "0" + Key;
                //Key = DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Event) + Key;
                return Key;
            }
        }

        public string TableAlias
        {
            get
            {
                string Alias = "CollectionEventLocalisation_";
                switch (this._LocalisationSystemID)
                {
                    case 2:
                        Alias += (DiversityCollection.Import.ImportStepEvent.GaussKrueger).ToString();
                        break;
                    case 3:
                        Alias += (DiversityCollection.Import.ImportStepEvent.MTB).ToString();
                        break;
                    case 4:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Altitude).ToString();
                        break;
                    case 8:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Coordinates).ToString();
                        break;
                    case 7:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Place).ToString();
                        break;
                    case 10:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Exposition).ToString();
                        break;
                    case 11:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Slope).ToString();
                        break;
                    case 13:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Plot).ToString();
                        break;
                    case 14:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Depth).ToString();
                        break;
                    case 15:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Height).ToString();
                        break;
                }
                return Alias;
            }
        }

        
        #endregion

        #region Construction, Properties and init

        public UserControlImportLocalisation()
        {
            InitializeComponent();
        }
        
        public void initControl(int LocalisationSystemID
            , DiversityCollection.FormImportWizard FormImportWizard
            , DiversityCollection.Import Import)
        {
            try
            {
                this._LocalisationSystemID = LocalisationSystemID;
                this._FormImportWizard = FormImportWizard;
                this._Import = Import;
                System.Data.DataTable dtLocalisationSystem = DiversityCollection.LookupTable.DtLocalisationSystem;
                System.Data.DataRow[] R = dtLocalisationSystem.Select("LocalisationSystemID = " + this._LocalisationSystemID.ToString());
                if (R.Length > 0)
                {

                    this.initLocalisationSystemID();

                    this.labelHeader.Text = R[0]["Description"].ToString();

                    this.userControlImport_Column_Location1.initUserControl(this.IC1, this._Import);
                    this.IC1.setDisplayTitle(R[0]["DisplayTextLocation1"].ToString());
                    this.userControlImport_Column_Location2.initUserControl(this.IC2, this._Import);
                    this.IC2.setDisplayTitle(R[0]["DisplayTextLocation2"].ToString());
                    this.userControlImport_Column_Responsible.initUserControl(this.ICResponsible, this._Import);
                    this.userControlImport_Column_LocationNotes.initUserControl(this.ICLocationNotes, this._Import);

                    this._LocalisationSystem = R[0]["DisplayText"].ToString();
                    this.userControlImport_Column_RecordingMethod.initUserControl(this.ICRecordingMethod, this._Import);
                    this.userControlImport_Column_Accuracy.initUserControl(this.ICAccuracy, this._Import);

                    if (this._LocalisationSystemID == 8)
                    {
                        this.userControlImport_Column_Geography.initUserControl(this.ICGeography, this._Import);
                        this.ICGeography.Separator = "geography::STGeomFromText('";
                        this.ICGeography.RegularExpressionPattern = "$";
                        this.ICGeography.RegularExpressionReplacement = "', 4326)";
                        this.userControlImport_Column_Geography.Visible = true;
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private void initLocalisationSystemID()
        {
            this._ICLocalisationSystemID = DiversityCollection.Import_Column.GetImportColumn(this._StepKey, "CollectionEventLocalisation", this.TableAlias, "LocalisationSystemID", null);
            this._ICLocalisationSystemID.PresetValueColumn = "LocalisationSystemID";
            this._ICLocalisationSystemID.PresetValue = this._LocalisationSystemID.ToString();
            this._ICLocalisationSystemID.TableAlias = this.TableAlias;
            this._ICLocalisationSystemID.TypeOfEntry = Import_Column.EntryType.Database;
            this._ICLocalisationSystemID.TypeOfFixing = Import_Column.FixingType.Schema;
            this._ICLocalisationSystemID.StepKey = this._StepKey;
            this._ICLocalisationSystemID.ValueIsFixed = true;
            this._ICLocalisationSystemID.TypeOfSource = Import_Column.SourceType.Database;
            this._ICLocalisationSystemID.Value = this._LocalisationSystemID.ToString();
        }

        private string ParsingMethodName
        {
            get 
            {
                if (this._ParsingMethodName == null)
                {
                    System.Data.DataTable dtLocalisationSystem = DiversityCollection.LookupTable.DtLocalisationSystem;
                    System.Data.DataRow[] R = dtLocalisationSystem.Select("LocalisationSystemID = " + this._LocalisationSystemID.ToString());
                    if (R.Length > 0)
                    {
                        _ParsingMethodName = R[0]["ParsingMethodName"].ToString();
                    }
                }
                return _ParsingMethodName; 
            }
            //set { _ParsingMethodName = value; }
        }

        private System.Data.DataTable DtRecordingMethods
        {
            get 
            {
                if (this._DtRecordingMethods == null)
                {
                    string SQL = "SELECT DISTINCT L.RecordingMethod FROM CollectionEventLocalisation " + 
                        " AS L INNER JOIN CollectionEventID_UserAvailable AS A ON L.CollectionEventID = A.CollectionEventID " +
                        " WHERE L.RecordingMethod <> N'' AND L.LocalisationSystemID = " + this._LocalisationSystemID.ToString() + 
                        " ORDER BY L.RecordingMethod";
                    this._DtRecordingMethods = new DataTable();
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtRecordingMethods);
                }
                return _DtRecordingMethods; 
            }
            //set { _DtRecordingMethods = value; }
        }
        
        private void initFormatList()
        {
            switch (this.ParsingMethodName)
            {
                case "Altitude":
                case "Coordinates":
                case "Exposition":
                case "Gazetteer":
                case "Place":
                case "GK":
                case "Height":
                case "MTB":
                case "RD":
                case "SamplingPlot":
                case "Slope":
                case "Top50":
                    break;
            }
        }

        #endregion

        #region Interface

        public void Reset() { }

        public string StepKey() { return this._StepKey; }
        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return null; }
        public void UpdateSelectionPanel() { }
        public void initUserControl(DiversityCollection.iImportInterface I, DiversityCollection.Import_Step SuperiorImportStep)
        {
        }

        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
        }

        public void AddImportStep()
        {
        }

        public void AddImportStep(string StepKey)
        {
            try
            {
            }
            catch (System.Exception ex) { }
        }

        public void HideImportStep()
        {
        }

        public void ShowHiddenImportSteps()
        {
        }

        #endregion

    }
}
