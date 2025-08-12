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
    public partial class UserControlImportEventProperty : UserControl
    {

        #region Parameter

        private int _PropertyID;
        private string _Property;

        private DiversityCollection.FormImportWizard _FormImportWizard;
        private DiversityCollection.Import _Import;
        private string _DisplayText;

        public string DisplayText
        {
            get 
            {
                if (this._DisplayText == null)
                {
                    System.Data.DataTable dtProperty = DiversityCollection.LookupTable.DtProperty;
                    System.Data.DataRow[] R = dtProperty.Select("PropertyID = " + this._PropertyID.ToString());
                    if (R.Length > 0)
                    {
                        this._DisplayText = R[0]["PropertyName"].ToString();
                    }
                }
                return _DisplayText;
            }
        }

        private DiversityCollection.Import_Column _ICPropertyID;

        private DiversityCollection.Import_Column _ICDisplayText;
        public DiversityCollection.Import_Column ICDisplayText
        {
            get 
            {
                if (this._ICDisplayText == null)
                    this._ICDisplayText = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", this.TableAlias, "DisplayText", this.userControlImport_Column_DisplayText);
                if (this._ICDisplayText.Table == null)
                {
                    this._ICDisplayText = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", "DisplayText", this.userControlImport_Column_DisplayText);
                    this._ICDisplayText.PresetValue = this._PropertyID.ToString();
                    this._ICDisplayText.PresetValueColumn = "PropertyID";
                    this._ICDisplayText.TableAlias = this.TableAlias;
                    this._ICDisplayText.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICDisplayText.TypeOfFixing = Import_Column.FixingType.None;
                    this._ICDisplayText.StepKey = this.StepKey;
                    this._ICDisplayText.setDisplayTitle(this.DisplayText);
                }
                if (this._ICDisplayText.TypeOfEntry == Import_Column.EntryType.Boolean)
                    this._ICDisplayText.TypeOfEntry = Import_Column.EntryType.Text;
                return _ICDisplayText; 
            }
        }

        private DiversityCollection.Import_Column _ICPropertyValue;
        public DiversityCollection.Import_Column ICPropertyValue
        {
            get
            {
                if (this._ICPropertyValue == null)
                    this._ICPropertyValue = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", this.TableAlias, "PropertyValue", this.userControlImport_Column_PropertyValue);
                if (this._ICPropertyValue.Table == null)
                {
                    this._ICPropertyValue = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", this.TableAlias, "PropertyValue", this.userControlImport_Column_PropertyValue);
                    this._ICPropertyValue.PresetValue = this._PropertyID.ToString();
                    this._ICPropertyValue.PresetValueColumn = "PropertyID";
                    this._ICPropertyValue.TableAlias = this.TableAlias;
                    this._ICPropertyValue.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICPropertyValue.TypeOfFixing = Import_Column.FixingType.None;
                    this._ICPropertyValue.StepKey = this.StepKey;
                }
                if (this._ICPropertyValue.TypeOfEntry == Import_Column.EntryType.Boolean)
                    this._ICPropertyValue.TypeOfEntry = Import_Column.EntryType.Text;
                return _ICPropertyValue;
            }
        }

        private DiversityCollection.Import_Column _ICResponsible;
        public DiversityCollection.Import_Column ICResponsible
        {
            get 
            {
                if (this._ICResponsible == null)
                {
                    this._ICResponsible = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", this.TableAlias, "ResponsibleName", this.userControlImport_Column_Responsible);
                    this._ICResponsible.PresetValueColumn = "PropertyID";
                    this._ICResponsible.PresetValue = this._PropertyID.ToString();
                    this._ICResponsible.TableAlias = this.TableAlias;
                    this._ICResponsible.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICResponsible.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICResponsible.StepKey = this.StepKey;
                }
                return _ICResponsible; 
            }
        }

        private DiversityCollection.Import_Column _ICHierarchy;
        public DiversityCollection.Import_Column ICHierarchy
        {
            get
            {
                if (this._ICHierarchy == null)
                {
                    this._ICHierarchy = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", this.TableAlias, "PropertyHierarchyCache", this.userControlImport_Column_Hierarchy);
                    this._ICHierarchy.PresetValueColumn = "PropertyID";
                    this._ICHierarchy.PresetValue = this._PropertyID.ToString();
                    this._ICHierarchy.TableAlias = this.TableAlias;
                    this._ICHierarchy.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICHierarchy.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICHierarchy.StepKey = this.StepKey;
                    this._ICHierarchy.MultiColumn = true;
                }
                return _ICHierarchy;
            }
        }

        private DiversityCollection.Import_Column _ICNotes;
        public DiversityCollection.Import_Column ICNotes
        {
            get
            {
                if (this._ICNotes == null)
                {
                    this._ICNotes = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", this.TableAlias, "Notes", this.userControlImport_Column_Notes);
                    this._ICNotes.PresetValueColumn = "PropertyID";
                    this._ICNotes.PresetValue = this._PropertyID.ToString();
                    this._ICNotes.TableAlias = this.TableAlias;
                    this._ICNotes.TypeOfEntry = Import_Column.EntryType.Text;
                    this._ICNotes.TypeOfFixing = Import_Column.FixingType.Schema;
                    this._ICNotes.StepKey = this.StepKey;
                }
                return _ICNotes;
            }
        }

        private string StepKey
        {
            get
            {
                /*
                 * PropertyID	LocalisationSystemName
                    20	Chronostratigraphy
                    30	Lithostratigraphy
                 * */
                string Key = "";
                switch (this._PropertyID)
                {
                    case 20:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Chronostratigraphy);
                        break;
                    case 30:
                        Key += DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Lithostratigraphy);
                        break;
                }
                return Key;
            }
        }

        public string TableAlias
        {
            get
            {
                string Alias = "CollectionEventProperty_";
                switch (this._PropertyID)
                {
                    case 20:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Chronostratigraphy).ToString();
                        break;
                    case 30:
                        Alias += (DiversityCollection.Import.ImportStepEvent.Lithostratigraphy).ToString();
                        break;
                }
                return Alias;
            }
        }

        
        #endregion

        #region Construction, Properties and init

        public UserControlImportEventProperty()
        {
            InitializeComponent();
        }
        
        public void initControl(int PropertyID
            , DiversityCollection.FormImportWizard FormImportWizard
            , DiversityCollection.Import Import)
        {
            try
            {
                this._PropertyID = PropertyID;
                this._FormImportWizard = FormImportWizard;
                this._Import = Import;
                System.Data.DataTable dtProperty = DiversityCollection.LookupTable.DtProperty;
                System.Data.DataRow[] R = dtProperty.Select("PropertyID = " + this._PropertyID.ToString());
                if (R.Length > 0)
                {

                    this.initPropertyID();

                    this.labelHeader.Text = R[0]["Description"].ToString();

                    this.userControlImport_Column_DisplayText.initUserControl(this.ICDisplayText, this._Import);
                    this.userControlImport_Column_PropertyValue.initUserControl(this.ICPropertyValue, this._Import);
                    this.userControlImport_Column_Responsible.initUserControl(this.ICResponsible, this._Import);
                    this.userControlImport_Column_Hierarchy.initUserControl(this.ICHierarchy, this._Import);
                    this.userControlImport_Column_Notes.initUserControl(this.ICNotes, this._Import);

                    this._Property = R[0]["DisplayText"].ToString();
                }
            }
            catch (System.Exception ex) { }
        }

        private void initPropertyID()
        {
            this._ICPropertyID = DiversityCollection.Import_Column.GetImportColumn(this.StepKey, "CollectionEventProperty", this.TableAlias, "PropertyID", null);
            this._ICPropertyID.PresetValueColumn = "PropertyID";
            this._ICPropertyID.PresetValue = this._PropertyID.ToString();
            this._ICPropertyID.TableAlias = this.TableAlias;
            this._ICPropertyID.TypeOfEntry = Import_Column.EntryType.Database;
            this._ICPropertyID.TypeOfFixing = Import_Column.FixingType.Schema;
            this._ICPropertyID.StepKey = this.StepKey;
            this._ICPropertyID.ValueIsFixed = true;
            this._ICPropertyID.TypeOfSource = Import_Column.SourceType.Database;
            this._ICPropertyID.Value = this._PropertyID.ToString();
        }

        #endregion

        #region Interface

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
