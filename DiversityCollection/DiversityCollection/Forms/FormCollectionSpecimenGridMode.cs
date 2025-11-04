#define Autosuggest

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using DiversityWorkbench.DwbManual;

namespace DiversityCollection.Forms
{

    //public struct GridModeQueryField
    //{
    //    /// <summary>
    //    /// The name of the table in the database
    //    /// </summary>
    //    public string Table;
    //    /// <summary>
    //    /// the name of the table in the grid mode
    //    /// </summary>
    //    public string AliasForTable;
    //    /// <summary>
    //    ///  the name of the column in the database
    //    /// </summary>
    //    public string Column;
    //    /// <summary>
    //    /// the name of the column in the gridmode
    //    /// </summary>
    //    public string AliasForColumn;
    //    /// <summary>
    //    /// a restriction for selecting dataset from a table, e.g. LocalisationSystemID = 7
    //    /// </summary>
    //    public string Restriction;
    //    public string DatasourceTable;
    //    public bool IsVisible;
    //    public bool IsHidden;
    //    /// <summary>
    //    /// The column to delete in which the link to an external module is stored
    //    /// </summary>
    //    public string RemoveLinkColumn;
    //    public string Entity;
    //}

    public partial class FormCollectionSpecimenGridMode : Form
    {

        #region Parameter
        // GridMode
        public enum ViewMode { Default, Scanmode, Gridmode };
        public enum ImageIndexForTreeView { Null, Project, Event, Time, Localisation, Habitat, Specimen, Depositor, Label, Plant, Identification, Analysis, Agent, Storage, Transaction };

        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields;
        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeHiddenQueryFields;
        private System.Collections.Generic.List<string> _GridModeColumnList;
        private System.Collections.Generic.Dictionary<string, string> _GridModeTableList;

        private System.Collections.Generic.Dictionary<string, string> _ProjectSettings;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private System.Collections.Generic.List<int> _IDs;
        private int _SpecimenID = 0;
        private int _ProjectID = 0;
        private string _TaxonomicGroup = "plant";
        private DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState _ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace;
        private bool _StopReplacing = false;
        public static bool StopReplacing = false;
        private System.Collections.Generic.Dictionary<string, string> _BlockedColumns;
        private System.Windows.Forms.DataGridViewCellStyle _StyleBlocked;
        private System.Windows.Forms.DataGridViewCellStyle _StyleUnblocked;

        private System.Collections.Generic.Dictionary<string, string> _LinkColumns;
        private System.Windows.Forms.DataGridViewCellStyle _StyleLink;

        private System.Collections.Generic.Dictionary<string, string> _RemoveColumns;
        private System.Windows.Forms.DataGridViewCellStyle _StyleRemove;

        private System.Collections.Generic.Dictionary<string, string> _NewIdentificationColumns;
        private System.Windows.Forms.DataGridViewCellStyle _StyleNewIdentification;

        private enum SaveModes { Single, All };
        private FormCollectionSpecimenGridMode.SaveModes _SaveMode = SaveModes.Single;
        private DiversityCollection.Forms.FormGridFunctions.FormState _FormState = Forms.FormGridFunctions.FormState.Loading;

        private DiversityCollection.Forms.FormGridFunctions _Grid;

        #region Data

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimenImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAgent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProject;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelation;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelationInvers;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterPart;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProcessing;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterTransaction;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnit;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnitInPart;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterIdentification;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAnalysis;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisation;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisationSystem;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProperty;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSuperiorList;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeries;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSpecimenExtern;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesUnit;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesUnitExtern;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesLocalisation;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterCollection;

        DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageDataTable _CollectionEventSeriesImageDataTable = new DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageDataTable();

        private string _SqlSpecimenFields = " CollectionSpecimenID, Accession_number, Data_withholding_reason, Data_withholding_reason_for_collection_event, Data_withholding_reason_for_collector, " +
            "Collectors_event_number, Collection_day, Collection_month, Collection_year, Collection_date_supplement, Collection_time, Collection_time_span, Country,  " +
            "Locality_description, Habitat_description, Collecting_method, Collection_event_notes, Named_area, NamedAreaLocation2, Remove_link_to_gazetteer,  " +
            "Distance_to_location, Direction_to_location, Longitude, Latitude, Coordinates_accuracy, Link_to_GoogleMaps, _CoordinatesLocationNotes, Altitude_from, Altitude_to,  " +
            "Altitude_accuracy, Notes_for_Altitude, MTB, Quadrant, Notes_for_MTB, Sampling_plot, Link_to_SamplingPlots, Remove_link_to_SamplingPlots,  " +
            "Accuracy_of_sampling_plot, Latitude_of_sampling_plot, Longitude_of_sampling_plot, Geographic_region, Lithostratigraphy, Chronostratigraphy, Biostratigraphy,  " +
            "Collectors_name, Link_to_DiversityAgents, Remove_link_for_collector, Collectors_number, Notes_about_collector, Accession_day, Accession_month,  " +
            "Accession_year, Accession_date_supplement, Depositors_name, Depositors_link_to_DiversityAgents, Remove_link_for_Depositor, Depositors_accession_number,  " +
            "Exsiccata_abbreviation, Link_to_DiversityExsiccatae, Remove_link_to_exsiccatae, Exsiccata_number, Original_notes, Additional_notes, Internal_notes, Label_title,  " +
            "Label_type, Label_transcription_state, Label_transcription_notes, Problems, External_datasource, External_identifier, " + //Reference_title_for_specimen, Link_to_DiversityReferences_for_specimen, Remove_link_of_reference_for_specimen, " +
            "Taxonomic_group, Relation_type, Colonised_substrate_part, Life_stage,  " +
            "Gender, Number_of_units, Circumstances, Order_of_taxon, Family_of_taxon, Identifier_of_organism, Description_of_organism, Only_observed, Notes_for_organism,  " +
            "Taxonomic_name, Link_to_DiversityTaxonNames, Remove_link_for_identification, Vernacular_term, Identification_day, Identification_month, Identification_year,  " +
            "Identification_category, Identification_qualifier, Type_status, Type_notes, Notes_for_identification, " + // Reference_title, Link_to_DiversityReferences, Remove_link_for_reference,  " +
            "Determiner, Link_to_DiversityAgents_for_determiner, Remove_link_for_determiner, Analysis, AnalysisID, Analysis_number,  " +
            "Analysis_result, Taxonomic_group_of_second_organism, Life_stage_of_second_organism, Gender_of_second_organism, Number_of_units_of_second_organism,  " +
            "Circumstances_of_second_organism, Identifier_of_second_organism, Description_of_second_organism, Only_observed_of_second_organism,  " +
            "Notes_for_second_organism, Taxonomic_name_of_second_organism, Link_to_DiversityTaxonNames_of_second_organism, Remove_link_for_second_organism,  " +
            "Vernacular_term_of_second_organism, Identification_day_of_second_organism, Identification_month_of_second_organism,  " +
            "Identification_year_of_second_organism, Identification_category_of_second_organism, Identification_qualifier_of_second_organism,  " +
            "Type_status_of_second_organism, Type_notes_of_second_organism, Notes_for_identification_of_second_organism, " + //Reference_title_of_second_organism, Link_to_DiversityReferences_of_second_organism, Remove_link_for_reference_of_second_organism,  " +
            "Determiner_of_second_organism,  " +
            "Link_to_DiversityAgents_for_determiner_of_second_organism, Remove_link_for_determiner_of_second_organism, Collection, Material_category, Storage_location,  " +
            "Stock, Part_accession_number, Storage_container, Preparation_method, Preparation_date, Notes_for_part, Related_specimen_URL,  " +
            "Related_specimen_display_text, Link_to_DiversityCollection_for_relation, Type_of_relation, Relation_is_internal, Related_specimen_description, Related_specimen_notes, _TransactionID, _Transaction, _CollectionEventID, _IdentificationUnitID,  " +
            "_IdentificationSequence, _SecondUnitID, _SecondSequence, _SpecimenPartID, _CoordinatesAverageLatitudeCache, _CoordinatesAverageLongitudeCache,  " +
            "_GeographicRegionPropertyURI, _LithostratigraphyPropertyURI, _ChronostratigraphyPropertyURI, _BiostratigraphyPropertyURI, _NamedAverageLatitudeCache,  " +
            "_NamedAverageLongitudeCache, _LithostratigraphyPropertyHierarchyCache, _ChronostratigraphyPropertyHierarchyCache, _BiostratigraphyPropertyHierarchyCache,  " +
            "_SecondUnitFamilyCache, _SecondUnitOrderCache, _AverageAltitudeCache ";


        //private string _SqlSpecimenFields = " CollectionSpecimenID, Accession_number, Data_withholding_reason, Data_withholding_reason_for_collection_event, Data_withholding_reason_for_collector, " +
        //    "Collectors_event_number, Collection_day, Collection_month, Collection_year, Collection_date_supplement, Collection_time, Collection_time_span, Country, " +
        //    "Locality_description, Habitat_description, Collecting_method, Collection_event_notes, Named_area, NamedAreaLocation2, Remove_link_to_gazetteer, " +
        //    "Distance_to_location, Direction_to_location, Longitude, Latitude, Coordinates_accuracy, Link_to_GoogleMaps, _CoordinatesLocationNotes, Altitude_from, Altitude_to, " +
        //    "Altitude_accuracy, Notes_for_Altitude, MTB, Quadrant, Notes_for_MTB, Sampling_plot, Link_to_SamplingPlots, Remove_link_to_SamplingPlots, " +
        //    "Accuracy_of_sampling_plot, Latitude_of_sampling_plot, Longitude_of_sampling_plot, Geographic_region, Lithostratigraphy, Chronostratigraphy, Collectors_name, " +
        //    "Link_to_DiversityAgents, Remove_link_for_collector, Collectors_number, Notes_about_collector, Accession_day, Accession_month, Accession_year, " +
        //    "Accession_date_supplement, Depositors_name, Depositors_link_to_DiversityAgents, Remove_link_for_Depositor, Depositors_accession_number, " +
        //    "Exsiccata_abbreviation, Link_to_DiversityExsiccatae, Remove_link_to_exsiccatae, Exsiccata_number, Original_notes, Additional_notes, Internal_notes, Label_title, " +
        //    "Label_type, Label_transcription_state, Label_transcription_notes, Problems, Taxonomic_group, Relation_type, Colonised_substrate_part, Life_stage, Gender, " +
        //    "Number_of_units, Circumstances, Order_of_taxon, Family_of_taxon, Identifier_of_organism, Description_of_organism, Only_observed, Notes_for_organism, " +
        //    "Taxonomic_name, Link_to_DiversityTaxonNames, Remove_link_for_identification, Vernacular_term, Identification_day, Identification_month, Identification_year, " +
        //    "Identification_category, Identification_qualifier, Type_status, Type_notes, Notes_for_identification, Reference_title, Link_to_DiversityReferences, " +
        //    "Remove_link_for_reference, Determiner, Link_to_DiversityAgents_for_determiner, Remove_link_for_determiner, Analysis, AnalysisID, Analysis_number, " +
        //    "Analysis_result, Taxonomic_group_of_second_organism, Life_stage_of_second_organism, Gender_of_second_organism, Number_of_units_of_second_organism, " +
        //    "Circumstances_of_second_organism, Identifier_of_second_organism, Description_of_second_organism, Only_observed_of_second_organism, " +
        //    "Notes_for_second_organism, Taxonomic_name_of_second_organism, Link_to_DiversityTaxonNames_of_second_organism, Remove_link_for_second_organism, " +
        //    "Vernacular_term_of_second_organism, Identification_day_of_second_organism, Identification_month_of_second_organism, " +
        //    "Identification_year_of_second_organism, Identification_category_of_second_organism, Identification_qualifier_of_second_organism,  " +
        //    "Type_status_of_second_organism, Type_notes_of_second_organism, Notes_for_identification_of_second_organism, Reference_title_of_second_organism, " +
        //    "Link_to_DiversityReferences_of_second_organism, Remove_link_for_reference_of_second_organism, Determiner_of_second_organism, " +
        //    "Link_to_DiversityAgents_for_determiner_of_second_organism, Remove_link_for_determiner_of_second_organism, Collection, Material_category, Storage_location, " +
        //    "Stock, Preparation_method, Preparation_date, Notes_for_part, _TransactionID, _Transaction, On_loan, _CollectionEventID, _IdentificationUnitID, " +
        //    "_IdentificationSequence, _SecondUnitID, _SecondSequence, _SpecimenPartID, _CoordinatesAverageLatitudeCache, _CoordinatesAverageLongitudeCache, " +
        //    "_GeographicRegionPropertyURI, _LithostratigraphyPropertyURI, _ChronostratigraphyPropertyURI, _NamedAverageLatitudeCache, _NamedAverageLongitudeCache, " +
        //    "_LithostratigraphyPropertyHierarchyCache, _ChronostratigraphyPropertyHierarchyCache, _SecondUnitFamilyCache, _SecondUnitOrderCache, " +
        //    "_AverageAltitudeCache ";

        //private string _SqlSpecimenFields = "CollectionSpecimenID, Accession_number, " +
        //    "Data_withholding_reason, Data_withholding_reason_for_collection_event,Data_withholding_reason_for_collector,  " +
        //    "Collectors_event_number, Collection_day, Collection_month, Collection_year, Collection_date_supplement, " +
        //    "Collection_time, Collection_time_span,  " +
        //    "Country, Locality_description, Habitat_description, Collecting_method, Collection_event_notes, " +
        //    "Named_area,  NamedAreaLocation2, Remove_link_to_gazetteer, Distance_to_location, Direction_to_location, " +
        //    "Longitude, Latitude, Coordinates_accuracy, Link_to_GoogleMaps,  " +
        //    "Altitude_from, Altitude_to, Altitude_accuracy, Notes_for_Altitude " +
        //    "MTB, Quadrant, Notes_for_MTB, " +
        //    "Sampling_plot, Link_to_SamplingPlots, Remove_link_to_SamplingPlots,  Accuracy_of_sampling_plot, Latitude_of_sampling_plot, Longitude_of_sampling_plot, " +
        //    "Geographic_region, Lithostratigraphy, Chronostratigraphy,  " +
        //    "Collectors_name, Link_to_DiversityAgents, Remove_link_for_collector, Collectors_number, Notes_about_collector, " +
        //    "Accession_day, Accession_month,  Accession_year, Accession_date_supplement, " +
        //    "Depositors_name, Depositors_link_to_DiversityAgents, Remove_link_for_Depositor, Depositors_accession_number, " +
        //    "Exsiccata_abbreviation, Link_to_DiversityExsiccatae, Remove_link_to_exsiccatae, Exsiccata_number, " +
        //    "Original_notes, Additional_notes, Internal_notes, " +
        //    "Label_title, Label_type, Label_transcription_state, Label_transcription_notes, Problems, " +
        //    "Taxonomic_group, Relation_type, Colonised_substrate_part, Life_stage, Gender, Number_of_units, Circumstances, Order_of_taxon, Family_of_taxon,  " +
        //    "Identifier_of_organism, Description_of_organism, Only_observed, Notes_for_organism, " +
        //    "Taxonomic_name, Link_to_DiversityTaxonNames,  " +
        //    "Remove_link_for_identification, Vernacular_term, Identification_day, Identification_month, Identification_year, Identification_category, Identification_qualifier, Type_status, " +
        //    "Type_notes, Notes_for_identification, Reference_title, Link_to_DiversityReferences, Remove_link_for_reference, " +
        //    "Determiner, Link_to_DiversityAgents_for_determiner, Remove_link_for_determiner, " +
        //    "AnalysisID, Analysis, Analysis_number, Analysis_result, " +
        //    "Taxonomic_group_of_second_organism, Life_stage_of_second_organism, Gender_of_second_organism, " +
        //    "Number_of_units_of_second_organism, Circumstances_of_second_organism, Identifier_of_second_organism, " +
        //    "Description_of_second_organism, Only_observed_of_second_organism, Notes_for_second_organism, " +
        //    "Taxonomic_name_of_second_organism, Link_to_DiversityTaxonNames_of_second_organism, Remove_link_for_second_organism,  " +
        //    "Vernacular_term_of_second_organism, Identification_day_of_second_organism, Identification_month_of_second_organism, " +
        //    "Identification_year_of_second_organism, Identification_category_of_second_organism, Identification_qualifier_of_second_organism, " +
        //    "Type_status_of_second_organism, Type_notes_of_second_organism, Notes_for_identification_of_second_organism, " +
        //    "Reference_title_of_second_organism, Link_to_DiversityReferences_of_second_organism, Remove_link_for_reference_of_second_organism, " +
        //    "Determiner_of_second_organism, Link_to_DiversityAgents_for_determiner_of_second_organism, Remove_link_for_determiner_of_second_organism, " +
        //    "Collection, Material_category, Storage_location, Stock, Preparation_method, Preparation_date, Notes_for_part, " +
        //    "_TransactionID, _Transaction, On_loan, _CollectionEventID, _IdentificationUnitID, _IdentificationSequence, _SecondUnitID, _SecondSequence,  " +
        //    "_SpecimenPartID, _CoordinatesAverageLatitudeCache, _CoordinatesAverageLongitudeCache, _CoordinatesLocationNotes, " +
        //    "_GeographicRegionPropertyURI, _LithostratigraphyPropertyURI, _ChronostratigraphyPropertyURI, " +
        //    "_NamedAverageLatitudeCache, _NamedAverageLongitudeCache, _LithostratigraphyPropertyHierarchyCache, " +
        //    "_ChronostratigraphyPropertyHierarchyCache, _SecondUnitFamilyCache, _SecondUnitOrderCache, _AverageAltitudeCache ";



        #endregion

        private string _SourceFunction;
        private string SourceFunction
        {
            get
            {
                if (this._SourceFunction == null || this._SourceFunction.Length == 0)
                {
                    this._SourceFunction = "FirstLines_4";
                    string SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_NAME = '" + this._SourceFunction + "'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result != "1")
                        this._SourceFunction = "FirstLines_3";
                }
                return this._SourceFunction;
            }
        }
        
        #endregion

        #region Construction

        public FormCollectionSpecimenGridMode(System.Collections.Generic.List<int> IDs, string FormTitle, int ProjectID)
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please connect to a database");
                this.Close();
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this._FormState = Forms.FormGridFunctions.FormState.Loading;
                InitializeComponent();
                //this.setCulture();
                this._ProjectID = ProjectID;
                if (FormTitle.Length > 0) this.Text = FormTitle;
                this.initForm();
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                this._IDs = IDs;
                if (IDs.Count > 0)
                    this._SpecimenID = IDs[0];
                this.userControlImageSpecimenImage.MaxSizeOfImageVisible = true;
                this.GridModeFillTable();

                this.GridModeSetColumnVisibility(this.dataGridView);
#if Autosuggest
                this.GridModeSetColumnVisibility(this.dataGridViewAutosuggest);
#endif

                this.setRemoveCellStyle(this.dataGridView);
                if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                    this.setRemoveCellStyle(this.dataGridViewAutosuggest);
                setLinkCellStyle(this.dataGridView);

                if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                    this.setLinkCellStyle(this.dataGridViewAutosuggest);

                //this.setNewIdentificationCellStyle();
                this.setIconsInTreeView();
                this.setTitleInTreeView();
                this.dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this._FormState = Forms.FormGridFunctions.FormState.Editing;
                DiversityWorkbench.Entity.setEntity(this, this.toolTip);
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms.FormCollectionSpecimenGridModeText));
                DiversityWorkbench.Entity.setEntity(this, resources);

                this._Grid = new Forms.FormGridFunctions(Forms.FormGridFunctions.GridType.Specimen, this.treeViewGridModeFieldSelector, this.dataGridView);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region Form

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void FormCollectionSpecimenGridMode_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimenGridMode.CollSpecimenRelationType_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collSpecimenRelationType_EnumTableAdapter.Fill(this.dataSetCollectionSpecimenGridMode.CollSpecimenRelationType_Enum);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimenGridMode.CollectionExternalDatasource". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionExternalDatasourceTableAdapter.Fill(this.dataSetCollectionSpecimenGridMode.CollectionExternalDatasource);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.firstLinesCollectionSpecimenTableAdapter.Fill(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.firstLinesCollectionSpecimenTableAdapter.Fill(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen);

        }

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void initForm()
        {
            try
            {
                string TableName = "CollectionSpecimen";
                bool OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                if (!OK)
                {
                    this.dataGridView.ReadOnly = true;
                    this.dataGridView.AllowUserToAddRows = false;
                    this.dataGridView.AllowUserToDeleteRows = false;

#if Autosuggest
                    this.dataGridViewAutosuggest.ReadOnly = true;
                    this.dataGridViewAutosuggest.AllowUserToAddRows = false;
                    this.dataGridViewAutosuggest.AllowUserToDeleteRows = false;
#endif

                    this.tableLayoutPanelDataHandling.Visible = false;
                }

                this.AcceptButton = this.userControlDialogPanel.buttonOK;
                // Markus 18.7.22 - auskommentiert - ansonsten wird bei Esc formular geschlossen
                //this.CancelButton = this.userControlDialogPanel.buttonCancel;
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                this.textBoxGridModeReplace.Width = 80;
                this.textBoxGridModeReplaceWith.Width = 80;
                this.comboBoxReplace.Width = 100;
                this.comboBoxReplaceWith.Width = 100;
                this.FillPicklists();

                this.setColumnsAndNodesCorrespondingToPermissions(this.dataGridView);
#if Autosuggest
                this.setColumnsAndNodesCorrespondingToPermissions(this.dataGridViewAutosuggest);
#endif

                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility.Length > 0)
                    this.GridModeSetFieldVisibilityForNodes();
                this.GridModeSetToolTipsInTree();
                this.setImageVisibility(DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.HeaderDisplay);
                this.userControlImageSpecimenImage.ImagePath = "";

                this.setGridColumnHeaders(this.dataGridView);
#if Autosuggest
                this.setGridColumnHeaders(this.dataGridViewAutosuggest);
#endif

                this.enableReplaceButtons(this.dataGridView, false);
                this.userControlDialogPanel.buttonOK.Click += new EventHandler(CheckForChangesAndAskForSaving);
                foreach(System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
                    this.SetToolTipsInTreeView(N);

                this.setColumnSequence(this.dataGridView);
                this.setColumnWidths(this.dataGridView);

#if Autosuggest
                this.setColumnSequence(this.dataGridViewAutosuggest);
                this.setColumnWidths(this.dataGridViewAutosuggest);
#endif

                this.setAutosuggestControls();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FillPicklists()
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSpecimenImageType, "CollSpecimenImageType_Enum", con, true, true, true);

            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollCircumstances_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollIdentificationCategory_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollIdentificationQualifier_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollMaterialCategory_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollTaxonomicGroup_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollTypeStatus_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollUnitRelationType_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollLabelTranscriptionState_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollLabelType_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionSpecimenGridMode.CollSpecimenRelationType_Enum, false);

            string SQL = "SELECT CollectionID, CollectionName " +
                "FROM Collection ORDER BY CollectionName ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(this.dataSetCollectionSpecimenGridMode.Collection);
            }
            catch  {} 

            ad.SelectCommand.CommandText = "";
            try
            {
                ad.SelectCommand.CommandText = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                ad.Fill(this.dataSetCollectionSpecimenGridMode.ProjectList);
            }
            catch  {}

            try
            {
                ad.SelectCommand.CommandText = "SELECT ExternalDatasourceID, ExternalDatasourceName " +
                "FROM CollectionExternalDatasource " +
                "ORDER BY ExternalDatasourceName ";
                ad.Fill(this.dataSetCollectionSpecimenGridMode.CollectionExternalDatasource);
            }
            catch { } 

            System.Data.DataTable dtCountry = LookupTable.DtCountryAll();// DiversityWorkbench.Gazetteer.Countries();

            //System.Data.DataRow[] rr = dtCountry.Select(dtCountry.Columns[0].ColumnName + " = 'Germany'");

            if (dtCountry.Rows.Count > 0)
            {
                //System.Data.DataRow Rnull = this.dataSetCollectionSpecimenGridMode.GeoCountries.NewGeoCountriesRow();
                //Rnull["NameEn"] = "";
                //this.dataSetCollectionSpecimenGridMode.GeoCountries.Rows.Add(Rnull);
                System.Data.DataRow[] RR = dtCountry.Select("", dtCountry.Columns[0].ColumnName);
                foreach (System.Data.DataRow RC in RR)
                {
                    System.Data.DataRow RCountry = this.dataSetCollectionSpecimenGridMode.GeoCountries.NewGeoCountriesRow();
                    RCountry["NameEn"] = RC[0].ToString();
                    this.dataSetCollectionSpecimenGridMode.GeoCountries.Rows.Add(RCountry);
                }

                //System.Data.DataRow[] rrTest = this.dataSetCollectionSpecimenGridMode.GeoCountries.Select("NameEn = 'Germany'");
                //foreach (System.Data.DataRow RC in dtCountry.Rows)
                //{
                //    System.Data.DataRow RCountry = this.dataSetCollectionSpecimenGridMode.GeoCountries.NewGeoCountriesRow();
                //    RCountry["NameEn"] = RC[0].ToString();
                //    this.dataSetCollectionSpecimenGridMode.GeoCountries.Rows.Add(RCountry);
                //}
            }
            else
            {
                ad.SelectCommand.CommandText = "SELECT '' AS NameEn " +
                "UNION " +
                "SELECT DISTINCT CountryCache AS NameEn FROM CollectionEvent " +
                "ORDER BY NameEn";
                try
                {
                    ad.Fill(this.dataSetCollectionSpecimenGridMode.GeoCountries);
                }
                catch { }

                if (this.dataSetCollectionSpecimenGridMode.GeoCountries.Rows.Count == 0)
                {
                    ad.SelectCommand.CommandText = "SELECT DISTINCT CountryCache AS NameEn " +
                        "FROM CollectionEvent ORDER BY NameEn";
                    try
                    {
                        ad.Fill(this.dataSetCollectionSpecimenGridMode.GeoCountries);
                    }
                    catch { }
                }
            }

            //this.setAnalysisSource();

            if (this.dataSetCollectionSpecimenGridMode.Analysis.Rows.Count == 0)
            {
                ad.SelectCommand.CommandText = "SELECT P.AnalysisID, P.DisplayText " +
                    "+ CASE WHEN A.DisplayText IS NULL THEN '' ELSE ' / ' + A.DisplayText END " +
                    "+ CASE WHEN P.Description IS NULL OR P.Description = '' THEN '' ELSE ': ' + P.Description END " +
                    "AS DisplayText " +
                    "FROM dbo.AnalysisProjectList(" + this._ProjectID.ToString() + ") AS P LEFT OUTER JOIN " +
                    "Analysis AS A ON A.AnalysisID = P.AnalysisParentID " +
                    "WHERE (P.OnlyHierarchy = 0)";
                //SELECT A.AnalysisID, CASE WHEN PP.DisplayText IS NULL THEN '' ELSE PP.DisplayText + ' - ' END + CASE WHEN P.DisplayText IS NULL " +
                //    "THEN '' ELSE P.DisplayText + ' - ' END + A.DisplayText + CASE WHEN A.[Description] IS NULL OR " +
                //    "A.[Description] = A.[DisplayText] THEN '' ELSE ': ' + A.[Description] END AS DisplayText " +
                //    "FROM Analysis AS P RIGHT OUTER JOIN " +
                //    "Analysis AS A ON P.AnalysisID = A.AnalysisParentID LEFT OUTER JOIN " +
                //    "Analysis AS PP ON P.AnalysisParentID = PP.AnalysisID " +
                //    "ORDER BY DisplayText";
                try
                {
                    ad.Fill(this.dataSetCollectionSpecimenGridMode.Analysis);
                }
                catch { }
            }
        }

        //private void setAnalysisSource()
        //{
        //    //if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count < 1) return;
        //    //if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows[0]["TaxonomicGroup"].Equals(System.DBNull.Value)) return;
        //    //string TaxGroup = this.dataSetCollectionSpecimen.IdentificationUnit.Rows[0]["TaxonomicGroup"].ToString();
        //    //if (TaxGroup.Length == 0) return;
        //    //if (TaxGroup == this._TaxonomicGroup) return;

        //    //this._TaxonomicGroup = TaxGroup;
        //    //this.dataSetCollectionSpecimenGridMode.Analysis.Clear();
        //    //string SQL = "SELECT AnalysisID, DisplayTextHierarchy AS DisplayText " +
        //    //    "FROM dbo.AnalysisList(" + this._ProjectID.ToString() + ", '" + this._TaxonomicGroup + "')" +
        //    //    "ORDER BY DisplayTextHierarchy";
        //    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //    //try
        //    //{
        //    //    ad.Fill(this.dataSetCollectionSpecimenGridMode.Analysis);
        //    //}
        //    //catch { }
        //}

        private void setAnalysis()
        {
            int? ID = this.AnalysisIDsforCurrentDataset();
            if (ID == null) return;
            else
            {
                try
                {
                    if (this.dataSetCollectionSpecimenGridMode.Analysis.Rows.Count == 0)
                    {
                        string SQL = "SELECT A.AnalysisID, CASE WHEN PP.DisplayText IS NULL THEN '' ELSE PP.DisplayText + ' - ' END + CASE WHEN P.DisplayText IS NULL " +
                            "THEN '' ELSE P.DisplayText + ' - ' END + A.DisplayText + CASE WHEN A.[Description] IS NULL OR " +
                            "A.[Description] = A.[DisplayText] THEN '' ELSE ': ' + A.[Description] END AS DisplayText " +
                            "FROM Analysis AS P RIGHT OUTER JOIN " +
                            "Analysis AS A ON P.AnalysisID = A.AnalysisParentID LEFT OUTER JOIN " +
                            "Analysis AS PP ON P.AnalysisParentID = PP.AnalysisID " +
                            "ORDER BY DisplayText";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        try
                        {
                            ad.Fill(this.dataSetCollectionSpecimenGridMode.Analysis);
                        }
                        catch { }
                    }
                    int AnalysisID = (int)ID;
                    string Analysis = "";
                    System.Data.DataRow[] RR = this.dataSetCollectionSpecimenGridMode.Analysis.Select("AnalysisID = " + ID.ToString());
                    if (RR.Length == 0) return;
                    Analysis = RR[0]["DisplayText"].ToString();
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID"] = AnalysisID;
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfCurrentLine]["Analysis"] = Analysis;
                    if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfCurrentLine]["Analysis_number"].Equals(System.DBNull.Value))
                        this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfCurrentLine]["Analysis_number"] = "1";
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
        }

        private int? AnalysisIDsforCurrentDataset()
        {
            int ID;
            int UnitID;
            if (this.ProjectSettings.ContainsKey("IdentificationUnitAnalysis.AnalysisID")
                && this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"].Length > 0)
                if (int.TryParse(this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"], out ID))
                    return ID;

            if (!int.TryParse(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfCurrentLine]["_IdentificationUnitID"].ToString(), out UnitID))
                return null;
            System.Data.DataTable dtAnalysis = new DataTable();
            string SQL = "SELECT AnalysisID, DisplayTextHierarchy AS DisplayText " +
                "FROM dbo.AnalysisListForUnit (" + UnitID.ToString() + ") " +
                "ORDER BY DisplayTextHierarchy";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dtAnalysis);
                if (dtAnalysis.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No analysis tyes are available for this organism");
                    return null;
                }
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtAnalysis, "DisplayText", "AnalysisID", "Analysis", "Please select the analysis you want to insert");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (int.TryParse(f.SelectedValue, out ID))
                        return ID;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return null;
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Feedback.SendFeedback(
                    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                    "",
                    this.SpecimenID.ToString());


                //DiversityWorkbench.Feedback.SendFeedback(
                //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                //    "",
                //    this.SpecimenID.ToString());
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void FormCollectionSpecimenGridMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.CheckForChangesAndAskForSaving(null, null);
                string ColumnWidths = "";
                string ColumnPositions = "";
#if Autosuggest

                ColumnWidths = this.getColumnWidths(this.dataGridViewActive);
                ColumnPositions = this.getColumnSequence(this.dataGridViewActive);
#else
                ColumnWidths = this.getColumnWidths(this.dataGridView);
                ColumnPositions = this.getColumnSequence(this.dataGridView);
#endif
                //foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                //{
                //    ColumnWidths += C.Width + " ";
                //    ColumnPositions += C.DisplayIndex.ToString() + " ";
                //}
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnWidth = ColumnWidths;
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence = ColumnPositions;
                DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.HeaderDisplay = this.CurrentHeaderDisplaySettings;
                DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Save();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private string Message(string Resource)
        {
            string Message = "";
            try
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms.FormCollectionSpecimenGridModeText));
                Message = resources.GetString(Resource);

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Message;
        }

#endregion

#region TreeView
        
        private void setIconsInTreeView()
        {
            try
            {
                this.treeViewGridModeFieldSelector.ImageList = DiversityCollection.Specimen.ImageListTablesAndFields;
                foreach (System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
                {
                    int iIndex = -1;
                    switch (N.Name)
                    {
                        case "NodeWithholding":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Project, false);
                            break;
                        case "NodeEvent":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Event, false);
                            this.setIconsInTreeView(N);
                            break;
                        case "NodeCollectionDate":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.DateTime, false);
                            break;
                        case "NodeCoordinates":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Localisation, false);
                            break;
                        case "NodeCollectionSiteProperties":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Habitat, false);
                            break;
                        case "NodeCollectionAgent":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Agent, false);
                            this.setIconsInTreeView(N);
                            break;
                        case "NodeSpecimen":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Specimen, false);
                            this.setIconsInTreeView(N);
                            break;
                        case "NodeDepositor":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Depositor, false);
                            break;
                        case "NodeLabel":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Label, false);
                            break;
                        case "NodeUnit":
                        case "NodeSecondUnit":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Unit, false);
                            this.setIconsInTreeView(N);
                            break;
                        case "NodeStorage":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Storage, false);
                            break;
                        case "NodeTransaction":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Transaction, false);
                            break;
                        case "NodeRelation":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Relation, false);
                            break;
                    }
                    N.ImageIndex = iIndex;
                    N.SelectedImageIndex = iIndex;
                }

            }
            catch { }
        }

        private void setIconsInTreeView(System.Windows.Forms.TreeNode TreeNode)
        {
            foreach (System.Windows.Forms.TreeNode N in TreeNode.Nodes)
            {
                int iIndex = -1;
                switch (N.Name)
                {
                    case "NodeSecondUnitIdentification":
                    case "NodeIdentification":
                        iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Identification, false);
                        break;
                    case "NodeAnalysis":
                        iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Analysis, false);
                        break;
                    case "node_Data_withholding_reason_for_collection_event":
                    case "NodeDataWithholdingReason":
                    case "Node_Data_withholding_reason_for_collector":
                        iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Project, false);
                        break;
                }
                N.ImageIndex = iIndex;
                N.SelectedImageIndex = iIndex;
            }
        }

        private void setTitleInTreeView()
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
            {
                try
                {
                    GridModeQueryField Q = this.GridModeQueryFieldOfNode(N);
                    if (Q.Entity != null && Q.Entity.Length > 0 && (Q.Restriction == null || Q.Restriction.Length == 0))
                    {
                        System.Collections.Generic.Dictionary<string, string> EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Entity);
                        if (EntityInfo["DisplayTextOK"] == "True")
                            N.Text = EntityInfo["DisplayText"];
                    }
                    else if (N.Tag != null)
                    {
                        System.Collections.Generic.Dictionary<string, string> EntityInfo = DiversityWorkbench.Entity.EntityInformation(N.Tag.ToString());
                        if (EntityInfo["DisplayTextOK"] == "True")
                            N.Text = EntityInfo["DisplayText"];
                    }
                    else
                    {
                        if (N.Text == "Second organism")
                            switch (DiversityWorkbench.Settings.Language)
                            {
                                case "de-DE":
                                    N.Text = "Zweiter Organismus";
                                    break;
                            }
                    }
                    foreach (System.Windows.Forms.TreeNode childN in N.Nodes)
                        this.setTitleInTreeView(childN);

                }
                catch { }
            }
        }

        private void setTitleInTreeView(System.Windows.Forms.TreeNode TreeNode)
        {
            try
            {
                GridModeQueryField Q = this.GridModeQueryFieldOfNode(TreeNode);
                System.Collections.Generic.Dictionary<string, string> EntityInfo = new Dictionary<string, string>();
                if (Q.Entity != null && Q.Entity.Length > 0 
                    && (Q.Restriction == null || Q.Restriction.Length == 0)
                    && !Q.AliasForColumn.StartsWith("Link_to")
                    && !Q.AliasForColumn.StartsWith("Remove_link"))
                {
                    if (TreeNode.Text == "Link to Transaction")
                    {
                        TreeNode.Text = DiversityCollection.Forms.FormCollectionSpecimenGridModeText.Link_to_Transaction;
                    }
                    else
                    {
                        EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Entity);
                        if (EntityInfo["DisplayTextOK"] == "True")
                            TreeNode.Text = EntityInfo["DisplayText"];
                        else
                        {
                        }
                    }
                }
                else if (TreeNode.Tag != null)
                {
                    if (Q.Entity != null && Q.Entity.Length > 0  && Q.Restriction != null
                        && (Q.Entity.EndsWith(".DistanceToLocation")
                        || Q.Entity.EndsWith(".DirectionToLocation")
                        || Q.Entity.EndsWith(".LocationAccuracy")))
                    {
                        EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Entity);
                        if (EntityInfo["DisplayTextOK"] == "True")
                            TreeNode.Text = EntityInfo["DisplayText"];
                        else
                        {

                        }
                    }
                    else if (!TreeNode.Tag.ToString().Contains(";"))
                    {
                        EntityInfo = DiversityWorkbench.Entity.EntityInformation(TreeNode.Tag.ToString());
                        if (EntityInfo["DisplayTextOK"] == "True")
                            TreeNode.Text = EntityInfo["DisplayText"];
                        else
                        {
                        }
                    }
                    else
                    {
                        string Title = TreeNode.Text;
                        if (Title.EndsWith("SamplingPlots"))
                        {
                            EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.13");
                            Title = Title.Replace("SamplingPlots", EntityInfo["DisplayText"]);
                        }
                        if (Title.StartsWith("Link to"))
                        {
                            Title = Title.Replace("Link to", DiversityCollection.Forms.FormCollectionSpecimenGridModeText.Link_to);
                        }
                        else if (Title.StartsWith("Remove link to"))
                        {
                            switch (DiversityWorkbench.Settings.Language)
                            {
                                case "de-DE":
                                    Title = Title.Replace("Remove link to", "Lösche Verbindung zu");
                                    break;
                            }
                        }
                        else if (Title.StartsWith("Remove link for"))
                        {
                            Title = Title.Replace("Remove link for", DiversityCollection.Forms.FormCollectionSpecimenGridModeText.Remove_link_for);
                        }
                        else
                        {
                            switch (Title)
                            {
                                case "Named area":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.7");
                                    break;
                                case "MTB":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.3");
                                    break;
                                case "Quadrant":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.3.Quadrant");
                                    break;
                                case "Notes for MTB":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.LocationNotes");
                                    break;
                                case "Longitude":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.AverageLongitudeCache");
                                    break;
                                case "Latitude":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.AverageLatitudeCache");
                                    break;
                                case "Altitude to":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.4.AltitudeTo");
                                    break;
                                case "Altitude from":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.AverageAltitudeCache");
                                    break;
                                case "Sampling plot":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.13");
                                    break;
                                case "Geographic region":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID.10");
                                    break;
                                case "Lithostratigraphy":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID.30");
                                    break;
                                case "Chronostratigraphy":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID.20");
                                    break;
                                case "Biostratigraphy":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID.60");
                                    break;
                                default:
                                    EntityInfo = new Dictionary<string, string>();
                                    break;
                            }
                            if (EntityInfo.ContainsKey("DisplayTextOK")
                                && EntityInfo["DisplayTextOK"] == "True")
                                Title = EntityInfo["DisplayText"];
                            else
                            {
                            }
                        }
                        TreeNode.Text = Title;
                    }
                }
                else
                {
                }
                foreach (System.Windows.Forms.TreeNode childN in TreeNode.Nodes)
                    this.setTitleInTreeView(childN);
            }
            catch { }
        }
        
        private void RemoveNodeFromTree(string ColumnAlias, System.Windows.Forms.TreeNode ParentNode)
        {
            try
            {
                if (ParentNode == null)
                {
                    foreach (System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
                    {
                        this.RemoveNodeFromTree(ColumnAlias, N);
                    }
                }
                else
                {
                    foreach (System.Windows.Forms.TreeNode N in ParentNode.Nodes)
                    {
                        if (N.Tag != null)
                        {
                            char[] charSeparators = new char[] { ';' };
                            string[] TagContent = N.Tag.ToString().Split(charSeparators);
                            if (TagContent[4] == ColumnAlias)
                            {
                                N.Remove();
                                if (this.GridModeQueryFields.Count > 0)
                                {
                                    for (int i = 0; i < this.GridModeQueryFields.Count; i++)
                                    {
                                        if (this.GridModeQueryFields[i].AliasForColumn == ColumnAlias)
                                        {
                                            DiversityCollection.Forms.GridModeQueryField Gnew = new Forms.GridModeQueryField();
                                            Gnew.AliasForColumn = this.GridModeQueryFields[i].AliasForColumn;
                                            Gnew.AliasForTable = this.GridModeQueryFields[i].AliasForTable;
                                            Gnew.Column = this.GridModeQueryFields[i].Column;
                                            Gnew.DatasourceTable = this.GridModeQueryFields[i].DatasourceTable;
                                            Gnew.IsHidden = this.GridModeQueryFields[i].IsHidden;
                                            Gnew.IsVisible = false;
                                            Gnew.Restriction = this.GridModeQueryFields[i].Restriction;
                                            Gnew.Table = this.GridModeQueryFields[i].Table;
                                            this.GridModeQueryFields[i] = Gnew;
                                            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                                            {
                                                if (C.DataPropertyName == ColumnAlias)
                                                {
                                                    C.Visible = false;
                                                    break;
                                                }
                                            }
                                            if (this.GridModeColumnList.Contains(ColumnAlias))
                                                this.GridModeColumnList.Remove(ColumnAlias);
                                        }
                                    }
                                }
                                return;
                            }
                        }
                    }
                    foreach (System.Windows.Forms.TreeNode N in ParentNode.Nodes)
                    {
                        this.RemoveNodeFromTree(ColumnAlias, N);
                    }
                }
            }
            catch { }
        }
        
#endregion

#region Permissions and related functions

        private void setColumnsAndNodesCorrespondingToPermissions(System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                bool OK;
                string TableName = "";

                // Checking the permissions for tables
                // CollectionSpecimen
                TableName = "CollectionSpecimen";
                // Check UPDATE
                OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                this.buttonGridModeAppend.Enabled = OK;
                this.buttonGridModeInsert.Enabled = OK;
                this.buttonGridModeRemove.Enabled = OK;
                this.buttonGridModeReplace.Enabled = OK;
                this.buttonGridModeSave.Enabled = OK;
                this.buttonGridModeUndo.Enabled = OK;
                this.buttonGridModeUndoSingleLine.Enabled = OK;
                this.buttonSaveAll.Enabled = OK;
                dataGridView.ReadOnly = !OK;

                // Check INSERT
                OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                this.buttonGridModeCopy.Enabled = OK;

                // Check DELETE
                OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                this.buttonGridModeDelete.Visible = OK;

                if (!DiversityCollection.Forms.FormCollectionSpecimen.HasTransactionAccess)
                {
                    //this.RemoveNodeFromTree("CollectionSpecimenTransaction", null);
                    this.RemoveNodeFromTree("_Transaction", null);
                    this.RemoveNodeFromTree("_TransactionID", null);
                }

                // setting the columns if GridView is not ReadOnly
                if (!dataGridView.ReadOnly)
                {
                    // CollectionSpecimenTransaction
                    TableName = "CollectionSpecimenTransaction";
                    // Check UPDATE
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.setGridColumnAccordingToPermission(dataGridView, TableName, OK);

                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Setting the ReadOnly state of the columns in the grid view according to the permissions of this table
        /// </summary>
        /// <param name="TableName">The name of the table</param>
        /// <param name="HasPermission">The permission to change the content of this table</param>
        private void setGridColumnAccordingToPermission(System.Windows.Forms.DataGridView dataGridView, string TableName, bool HasPermission)
        {
            System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> FieldList = new List<GridModeQueryField>();
            try
            {
                foreach (DiversityCollection.Forms.GridModeQueryField G in this.GridModeQueryFields)
                {
                    if (G.Table == TableName && !FieldList.Contains(G))
                        FieldList.Add(G);
                }
                foreach (System.Windows.Forms.DataGridViewColumn Col in dataGridView.Columns)
                {
                    foreach (DiversityCollection.Forms.GridModeQueryField GF in FieldList)
                    {
                        if (Col.DataPropertyName == GF.AliasForColumn)
                            Col.ReadOnly = HasPermission;
                    }
                }

            }
            catch  { }
        }

        /// <summary>
        /// setting the column to enabled or not if the user has the permission and access to the corresponding module
        /// </summary>
        /// <param name="ColumnName">The name of the column in the datatable, enter '' if unknown and use AliasForColumn to specify</param>
        /// <param name="AliasForColumn">the name of the column in the GridMode</param>
        /// <param name="TableName">The name of the table</param>
        /// <param name="HasPermission">If the user has permission to change the contents of this column</param>
        private void setGridColumnAccordingToPermission(System.Windows.Forms.DataGridView dataGridView, string ColumnName, string AliasForColumn, string TableName, bool HasPermission)
        {
            bool HasAccessToModule = false;
            try
            {
                switch (AliasForColumn)
                {
                    case "Named_area":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityGazetteer");
                        //this.treeViewGridModeFieldSelector.Nodes["NodeNamedArea"].Checked = false;
                        if (!HasAccessToModule)
                            this.RemoveNodeFromTree(AliasForColumn, null);
                        //this.RemoveNodeFromTree("NodeNamedArea", null);
                        break;
                    case "Geographic_region":
                    case "Lithostratigraphy":
                    case "Chronostratigraphy":
                    case "Biostratigraphy":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms");
                        if (!HasAccessToModule)
                        {
                            this.RemoveNodeFromTree(AliasForColumn, null);
                        }
                        break;
                    case "Link_to_DiversityTaxonNames":
                    case "Link_to_DiversityTaxonNames_of_second_organism":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames");
                        if (!HasAccessToModule)
                        {
                            this.RemoveNodeFromTree(AliasForColumn, null);
                            //this.RemoveNodeFromTree("NodeSecondIdentLink", null);
                        }
                        break;
                    case "Link_to_DiversityAgents":
                    case "Depositors_link_to_DiversityAgents":
                    case "Link_to_DiversityAgents_for_responsible":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents");
                        if (!HasAccessToModule)
                        {
                            this.RemoveNodeFromTree(AliasForColumn, null);
                            //this.RemoveNodeFromTree("NodeCollectorsAgentURI", null);
                            //this.RemoveNodeFromTree("NodeLink_to_DiversityAgents_for_responsible", null);
                        }
                        break;
                    case "Link_to_DiversityExsiccatae":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityExsiccatae");
                        if (!HasAccessToModule)
                        {
                            this.RemoveNodeFromTree(AliasForColumn, null);
                        }
                        break;
                    case "Link_to_DiversityReferences":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences");
                        if (!HasAccessToModule)
                        {
                            this.RemoveNodeFromTree(AliasForColumn, null);
                        }
                        break;
                }
                string DataProperty = AliasForColumn;
                if (DataProperty.Length == 0)
                {
                    foreach (DiversityCollection.Forms.GridModeQueryField G in this.GridModeQueryFields)
                    {
                        if (G.Table == TableName && G.Column == ColumnName)
                        {
                            DataProperty = G.AliasForColumn;
                            break;
                        }
                    }
                }
                if (DataProperty.Length > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewColumn Col in dataGridView.Columns)
                    {
                        if (Col.DataPropertyName == DataProperty)
                        {
                            if (HasPermission && HasAccessToModule) Col.ReadOnly = false;
                            else Col.ReadOnly = true;
                            break;
                        }
                    }
                }

            }
            catch { }
        }

#endregion

#region Specimen

        private bool setSpecimen(int SpecimenID)
        {
            bool OK = true;
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return false;
            try
            {
                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    this.updateSpecimen();
                }
                this.userControlImageSpecimenImage.ImagePath = "";
                this.fillSpecimen(SpecimenID);
                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    this.setHeader();
                    this.tableLayoutPanelHeader.Visible = true;
                }
                else
                {
                    if (SpecimenID != 0)
                        System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Entity.Message("You_have_no_access_to_this_dataset"));
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private bool setSpecimen(string AccessionNumber)
        {
            bool OK = false;
            string SQL = "SELECT CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
                "FROM CollectionSpecimen INNER JOIN " +
                "CollectionSpecimenID_UserAvailable ON  " +
                "CollectionSpecimen.CollectionSpecimenID = CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
                "WHERE (CollectionSpecimen.AccessionNumber = N'" + AccessionNumber + "')";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                int ID;
                OK = int.TryParse(c.ExecuteScalar()?.ToString(), out ID);
                if (!OK)
                {
                    System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Entity.Message("The_entry") + " " + AccessionNumber + " " + DiversityWorkbench.Entity.Message("could_not_be_found_in_the_database"));
                    return OK;
                }
                //int IDv = int.Parse(C.ExecuteScalar()?.ToString());
                con.Close();
                OK = this.setSpecimen(ID);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void setHeader()
        {
            try
            {
                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count > 0)
                    {
                        System.Data.DataRow[] rrType = this.dataSetCollectionSpecimen.Identification.Select("TypeStatus <> '' AND TypeStatus NOT LIKE 'not %'", "IdentificationSequence DESC");
                        if (rrType.Length > 0)
                        {
                            this.textBoxHeaderIdentification.BackColor = System.Drawing.Color.Red;
                            System.Data.DataRow rType = rrType[0];
                            if (!rType["TaxonomicName"].Equals(System.DBNull.Value))
                                this.textBoxHeaderIdentification.Text = rType["TaxonomicName"].ToString() + "\r\n" + rType["TypeStatus"].ToString();
                            else
                                this.textBoxHeaderIdentification.Text = rType["TypeStatus"].ToString();
                        }
                        else
                        {
                            this.textBoxHeaderIdentification.BackColor = System.Drawing.SystemColors.Control;
                            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder");
                            System.Data.DataRow r = rr[0];
                            if (!r["LastIdentificationCache"].Equals(System.DBNull.Value))
                                this.textBoxHeaderIdentification.Text = r["LastIdentificationCache"].ToString();
                            else
                                this.textBoxHeaderIdentification.Text = "";
                        }
                    }
                    else
                    {
                        this.textBoxHeaderIdentification.Text = "";
                        this.textBoxHeaderIdentification.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
                else
                {
                    this.textBoxHeaderID.Text = "";
                    this.textBoxHeaderAccessionNumber.Text = "";
                    this.textBoxHeaderIdentification.Text = "";
                    this.textBoxHeaderVersion.Text = "";
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillSpecimen(int SpecimenID)
        {
            try
            {
                if (this._SpecimenID != SpecimenID)
                    this.dataSetCollectionSpecimen.Clear();
                else
                {
                    this.dataSetCollectionSpecimen.CollectionAgent.Clear();
                    this.dataSetCollectionSpecimen.CollectionEvent.Clear();
                    this.dataSetCollectionSpecimen.CollectionEventImage.Clear();
                    this.dataSetCollectionSpecimen.CollectionEventLocalisation.Clear();
                    this.dataSetCollectionSpecimen.CollectionEventProperty.Clear();
                    this.dataSetCollectionSpecimen.CollectionEventSeries.Clear();
                    this.dataSetCollectionSpecimen.CollectionProject.Clear();
                    this.dataSetCollectionSpecimen.CollectionProjectList.Clear();
                    this.dataSetCollectionSpecimen.CollectionSpecimen.Clear();
                    this.dataSetCollectionSpecimen.CollectionSpecimenPart.Clear();
                    this.dataSetCollectionSpecimen.CollectionSpecimenPrinting.Clear();
                    this.dataSetCollectionSpecimen.CollectionSpecimenProcessing.Clear();
                    this.dataSetCollectionSpecimen.CollectionSpecimenRelation.Clear();
                    this.dataSetCollectionSpecimen.CollectionSpecimenRelationInvers.Clear();
                    this.dataSetCollectionSpecimen.CollectionSpecimenTransaction.Clear();
                    this.dataSetCollectionSpecimen.Identification.Clear();
                    this.dataSetCollectionSpecimen.IdentificationUnit.Clear();
                    this.dataSetCollectionSpecimen.IdentificationUnitAnalysis.Clear();
                    this.dataSetCollectionSpecimen.IdentificationUnitInPart.Clear();
                    this.dataSetCollectionSpecimen.IdentificationUnitInPartDisplayList.Clear();
                    this.dataSetCollectionSpecimen.IdentificationUnitInPartHideList.Clear();
                    this.dataSetCollectionSpecimen.IdentificationUnitNotInPartList.Clear();
                }

                string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
                    " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable)";

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimen);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAgent, DiversityCollection.CollectionSpecimen.SqlAgent + WhereClause + " ORDER BY CollectorsSequence", this.dataSetCollectionSpecimen.CollectionAgent);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProject, DiversityCollection.CollectionSpecimen.SqlProject + WhereClause + " ORDER BY ProjectID", this.dataSetCollectionSpecimen.CollectionProject);
                if (this._SpecimenID != SpecimenID)
                    this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this.dataSetCollectionSpecimen.CollectionSpecimenImage);

                string SQL = "SELECT R.CollectionSpecimenID, R.RelatedSpecimenURI, S.AccessionNumber AS RelatedSpecimenDisplayText, R.RelationType, R.RelatedSpecimenCollectionID, " +
                    "R.RelatedSpecimenDescription, R.Notes, R.IsInternalRelationCache  " +
                    "FROM CollectionSpecimenRelation R, CollectionSpecimen S  " +
                    "WHERE (R.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                    "AND (S.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                    "AND (R.IsInternalRelationCache = 1)  " +
                    "AND rtrim(substring(R.RelatedSpecimenURI, len(dbo.BaseURL()) + 1, 255)) = '" + SpecimenID.ToString() + "'  " +
                    "AND S.CollectionSpecimenID = R.CollectionSpecimenID " +
                    "ORDER BY RelatedSpecimenDisplayText ";

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenPart);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterRelation, DiversityCollection.CollectionSpecimen.SqlRelation + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenRelation);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnit, DiversityCollection.CollectionSpecimen.SqlUnit + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnit);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, DiversityCollection.CollectionSpecimen.SqlIdentification + WhereClause + " ORDER BY IdentificationSequence", this.dataSetCollectionSpecimen.Identification);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAnalysis, DiversityCollection.CollectionSpecimen.SqlAnalysis + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnitAnalysis);

                if (this._SaveMode == SaveModes.Single)
                {
                    this.listBoxSpecimenImage.Items.Clear();
                    if (this.ShowImagesSpecimen)
                    {
                        this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages,
                            this.dataSetCollectionSpecimen.CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                    }

                    if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count == 0) this.tableLayoutPanelSpecimenImage.Enabled = false;
                    if (!this.ShowImagesSpecimen && this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > 0)
                        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.Color.Yellow;
                    else if (!this.ShowImagesSpecimen && this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count == 0)
                        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.SystemColors.Control;
                }

                // Event
                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
                    {
                        int EventID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString());
                        WhereClause = " WHERE CollectionEventID = " + EventID.ToString();

                        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEvent, DiversityCollection.CollectionSpecimen.SqlEvent + WhereClause, this.dataSetCollectionSpecimen.CollectionEvent);
                        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterLocalisation, DiversityCollection.CollectionSpecimen.SqlEventLocalisation + WhereClause, this.dataSetCollectionSpecimen.CollectionEventLocalisation);
                        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProperty, DiversityCollection.CollectionSpecimen.SqlEventProperty + WhereClause, this.dataSetCollectionSpecimen.CollectionEventProperty);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void updateSpecimen()
        {
            if (this.sqlDataAdapterSpecimen != null)
            {
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimen", this.sqlDataAdapterSpecimen, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this.collectionSpecimenImageBindingSource);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionAgent", this.sqlDataAdapterAgent, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionProject", this.sqlDataAdapterProject, this.BindingContext);

                // if datasets of this table were deleted, this must happen before deleting the parent tables
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenPart", this.sqlDataAdapterPart, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenRelation", this.sqlDataAdapterRelation, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnit", this.sqlDataAdapterUnit, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Identification", this.sqlDataAdapterIdentification, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitAnalysis", this.sqlDataAdapterAnalysis, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEvent", this.sqlDataAdapterEvent, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventImage", this.sqlDataAdapterEventImage, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventLocalisation", this.sqlDataAdapterLocalisation, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventProperty", this.sqlDataAdapterProperty, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Collection", this.sqlDataAdapterCollection, this.BindingContext);
            }
        }

#endregion

#region Specimen images

        private void listBoxSpecimenImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListSpecimenImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void listBoxSpecimenImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListSpecimenImages.ImageSize.Height;
            e.ItemWidth = this.imageListSpecimenImages.ImageSize.Width;
        }

        private void listBoxSpecimenImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxSpecimenImage.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > i)
                {
                    this.tableLayoutPanelSpecimenImage.Enabled = true;
                    System.Data.DataRow r = this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows[i];
                    this.userControlImageSpecimenImage.ImagePath = r["URI"].ToString();
                    this.collectionSpecimenImageBindingSource.Position = i;
                    //System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetCollectionSpecimen, "CollectionSpecimenImage"];
                    //BMB.Position = i;
                }
                else
                    this.tableLayoutPanelSpecimenImage.Enabled = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenImageRow R = this.dataSetCollectionSpecimen.CollectionSpecimenImage.NewCollectionSpecimenImageRow();
                        R.CollectionSpecimenID = this.SpecimenID;
                        R.URI = f.URIImage;
                        this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Add(R);
                        this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this.dataSetCollectionSpecimen.CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string URL = this.userControlImageSpecimenImage.ImagePath;
                if (URL.Length > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenImage.Select("URI = '" + URL + "'");
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            r.Delete();
                            this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this.dataSetCollectionSpecimen.CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                            if (this.listBoxSpecimenImage.Items.Count > 0) this.listBoxSpecimenImage.SelectedIndex = 0;
                            else
                            {
                                this.listBoxSpecimenImage.SelectedIndex = -1;
                                this.userControlImageSpecimenImage.ImagePath = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
#endregion

#region Grid

        private void GridModeFillTable()
        {
            try
            {
                //this.firstLinesCollectionSpecimenTableAdapter.Fill(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen);
                string SQL = this.GridModeFillCommand(); 
                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Clear();
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen);
                this.dataGridViewGridMode_RowEnter(null, null);
                if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                    this.dataGridViewAutosuggest_RowEnter(null, null);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) 
            {
                string M = ex.Message;
                if (M.IndexOf("timeout") > -1) M += "\r\nPlease ask your database administrator to recreate the index for the table IdentificationUnit";
                System.Windows.Forms.MessageBox.Show(M);
            }
        }

#region Formatting of the grid view

        private string getColumnWidths(System.Windows.Forms.DataGridView dataGridView)
        {
            string ColumnWidths = "";
            foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
            {
                ColumnWidths += C.Width + " ";
            }
            return ColumnWidths;
        }

        private string getColumnSequence(System.Windows.Forms.DataGridView dataGridView)
        {
            string ColumnPositions = "";
            foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
            {
                ColumnPositions += C.DisplayIndex.ToString() + " ";
            }
            return ColumnPositions;
        }

        private void setColumnWidths(System.Windows.Forms.DataGridView dataGridView)
        {
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnWidth.Length > 0)
            {
                string[] ColumnWidths = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnWidth.Split(new char[] { ' ' });
                for (int i = 0; i < ColumnWidths.Length; i++)
                {
                    int Width = 0;
                    if (int.TryParse(ColumnWidths[i], out Width))
                    {
                        try
                        {
                            if (dataGridView.Columns.Count > i)
                                dataGridView.Columns[i].Width = Width;
                        }
                        catch { }
                    }
                }
            }
        }

        private void setColumnSequence(System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence.Length > 0)
                {
                    string[] Sequence = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence.Split(new char[] { ' ' });
                    for (int i = 0; i < Sequence.Length; i++)
                    {
                        int Display = 0;
                        if (int.TryParse(Sequence[i], out Display))
                        {
                            if (Display < dataGridView.Columns.Count)
                                dataGridView.Columns[i].DisplayIndex = Display;
                        }
                    }
                }
                else
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                    {
                        C.DisplayIndex = C.Index;
                    }
                }

            }
            catch { }
        }

        private void setGridColumnHeaders(System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                {
                    string Property = C.DataPropertyName;
                    string Header = C.HeaderText;
                    string Entity = "";
                    if (!Property.EndsWith("_second_organism"))
                    {
                        foreach (GridModeQueryField Q in this.GridModeQueryFields)
                        {
                            if (Q.AliasForColumn == Property)
                            {
                                System.Collections.Generic.Dictionary<string, string> EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.Column);
                                if (EntityInfo["DisplayTextOK"] == "True")
                                {
                                    Entity = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.Column)["DisplayText"];
                                }
                                break;
                            }
                        }
                    }

                    if (C.HeaderText == "CollectionSpecimenID")
                        C.HeaderText = "ID";
                    if (C.HeaderText.StartsWith("Link_to_"))
                    {
                        string[] HeaderParts = C.HeaderText.Split(new char[]{'_'});
                        string LastPart = HeaderParts[HeaderParts.Length - 1];
                        string NewHeader = "";
                        for (int hh = 0; hh < HeaderParts.Length - 1; hh++)
                        {
                            NewHeader += HeaderParts[hh] + "_";
                        }
                        for (int ii = 0; ii < LastPart.Length; ii++)
                        {
                            if (LastPart[ii].ToString().ToUpper() == LastPart[ii].ToString() && ii > 0)
                                NewHeader += "_";
                            NewHeader += LastPart[ii];
                        }
                    }
                    if (C.HeaderText.StartsWith("Google"))
                        C.HeaderText = "Google Maps";
                    if (C.HeaderText == "_TransactionID")
                    {
                        C.HeaderText = "Link to Trans.";
                        C.Width = 50;
                    }
                    if (C.HeaderText == "On_loan")
                    {
                        C.HeaderText = "Link to loan";
                        C.Width = 50;
                    }
                    C.HeaderText = C.HeaderText.Replace("_", " ");
                    if (Entity.Length > 0)
                    {
                        C.HeaderText = Entity;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setGridColumnToolTips(System.Windows.Forms.DataGridView dataGridView, string Description, string DataPropertyName)
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                {
                    if (DataPropertyName == C.DataPropertyName)
                    {
                        C.ToolTipText = Description;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonOptRowHeight_Click(object sender, EventArgs e)
        {
            this.dataGridViewActive.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        private void buttonOptColumnWidth_Click(object sender, EventArgs e)
        {
            this.dataGridViewActive.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
            //    return;
            try
            {
                DiversityCollection.Forms.FormGridFunctions.DrawRowNumber(this.dataGridView, this.dataGridView.Font, e);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

#endregion

#region Indices of Grid

        private int DatasetIndexOfCurrentLine
        {
            get
            {
                int i = 0;
                try
                {
                    if (this.dataGridViewActive.SelectedCells.Count > 0)
                    {
                        int CollectionSpecimenID = 0;
                        if (int.TryParse(this.dataGridViewActive.Rows[this.dataGridViewActive.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out CollectionSpecimenID))
                        {
                            for (i = 0; i < this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count; i++)
                            {
                                if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].RowState == DataRowState.Deleted
                                    || this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].RowState == DataRowState.Detached)
                                    continue;
                                if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i]["CollectionSpecimenID"].ToString() == CollectionSpecimenID.ToString())
                                    break;
                            }
                        }
                    }

                }
                catch { }
                return i;
            }
        }

        private int DatasetIndexOfLine(int SpecimenID)
        {
            int i = 0;
            try
            {
                for (i = 0; i < this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count; i++)
                {
                    if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i]["CollectionSpecimenID"].ToString() == SpecimenID.ToString())
                        break;
                }

            }
            catch{ }
            return i;
        }

        private int GridIndexOfDataline(int SpecimenID)
        {
            int i = 0;
            try
            {
                if (this.dataGridViewActive.Rows.Count > 0)
                {
                    for (i = 0; i < this.dataGridViewActive.Rows.Count; i++)
                    {
                        if (this.dataGridViewActive.Rows[i].Cells[0].Value.ToString() == SpecimenID.ToString())
                            break;
                    }
                }

            }
            catch { }
            return i;
        }
        
#endregion

#region Sorting

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (this.dataGridView.SortedColumn != null)
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    {
                        C.DividerWidth = 0;
                    }
                    this.dataGridView.Columns[this.dataGridView.SortedColumn.Index].DividerWidth = 2;
                }

            }
            catch { }

            this.MarkWholeColumn(this.dataGridView, e.ColumnIndex);

        }
        
#endregion

#region Button events for column settings and requery

        private void buttonGridRequery_Click(object sender, EventArgs e)
        {
            try
            {
                this._GridModeQueryFields = null;
                this._GridModeColumnList = null;
                this._GridModeTableList = null;
                this.GridModeSaveFieldVisibility();
                this.GridModeFillTable();
            }
            catch { }
        }

        private void buttonResetSequence_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence = "";
            this.setColumnSequence(dataGridView);
#if Autosuggest
            this.setColumnSequence(dataGridViewAutosuggest);
#endif
        }

        private void buttonGridModeUpdateColumnSettings_Click(object sender, EventArgs e)
        {
            this._GridModeColumnList = null;
            this._GridModeQueryFields = null;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility = "";
            this.GridModeSetColumnVisibility(this.dataGridView);
#if Autosuggest
            this.GridModeSetColumnVisibility(this.dataGridViewAutosuggest);
#endif
            this.enableReplaceButtons(this.dataGridView, false);
        }

        private void buttonGridModeFindUsedColumns_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.FindUsedDataColumns(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen, this.treeViewGridModeFieldSelector);
            this.buttonGridModeUpdateColumnSettings_Click(null, null);
        }

#endregion

#region Button events for Finding, Copy and Saving and related functions

        private void buttonGridModeFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count == 0 
                    || this.dataGridViewActive.SelectedCells.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show(this.Message("Nothing_selected"));
                    return;
                }
                if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.SaveAll();
                int ID = 0;
                if (int.TryParse(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine]["CollectionSpecimenID"].ToString(), out ID))
                {
                    this.DialogResult = DialogResult.OK;
                    this._SpecimenID = ID;
                    this.Close();
                }

            }
            catch { }
        }

        private void buttonGridModeSave_Click(object sender, EventArgs e)
        {
            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine].BeginEdit();
            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine].EndEdit();
            this.GridModeUpdate(DatasetIndexOfCurrentLine);
        }

        private void buttonSaveAll_Click(object sender, EventArgs e)
        {
            this._SaveMode = SaveModes.All;
            this.SaveAll();
            this._SaveMode = SaveModes.Single;
        }

        private void CheckForChangesAndAskForSaving(object sender, EventArgs e)
        {
            try
            {
                this.textBoxHeaderAccessionNumber.Focus();
                bool ChangesInData = false;
                for (int i = 0; i < this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count; i++)
                {
                    if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].RowState == DataRowState.Modified)
                    {
                        ChangesInData = true;
                        break;
                    }
                    //this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].EndEdit();
                }

                //for (int i = 0; i < this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count; i++)
                //{
                //    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].BeginEdit();
                //    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].EndEdit();
                //}
                if (this.dataSetCollectionSpecimenGridMode.HasChanges() && ChangesInData)
                {
                    if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        this.SaveAll();
                }

            }
            catch { }
        }

        private void SaveAll()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.progressBarSaveAll.Visible = true;
                this.progressBarSaveAll.Maximum = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count;
                this.progressBarSaveAll.Value = 0;
                for (int i = 0; i < this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count; i++)
                {
                    if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].RowState == DataRowState.Deleted
                        || this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].RowState == DataRowState.Detached
                        || this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].RowState == DataRowState.Unchanged)
                        continue;
                    this.dataGridViewActive.Rows[i].Cells[0].Selected = true;
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].BeginEdit();
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[i].EndEdit();
                    this.GridModeUpdate(i);
                    this.progressBarSaveAll.Value = i;
                }
            }
            catch { }
            finally { this.progressBarSaveAll.Visible = false; }
            this.Cursor = Cursors.Default;
        }

        private void buttonGridModeCopy_Click(object sender, EventArgs e)
        {
            bool MoreThanOneRow = false;
            if (this.dataGridViewActive.SelectedRows.Count > 1) MoreThanOneRow = true;
            try
            {
                if (this.dataGridViewActive.SelectedCells.Count > 1)
                {
                    System.Collections.Generic.List<int> RowIndex = new List<int>();
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridViewActive.SelectedCells)
                        if (!RowIndex.Contains(C.RowIndex)) RowIndex.Add(C.RowIndex);
                    if (RowIndex.Count > 1) MoreThanOneRow = true;
                }
                if (MoreThanOneRow)
                {
                    System.Windows.Forms.MessageBox.Show(this.Message("Only_single_datasets_can_be_copied"));
                    return;
                }
                else
                {
                    // Save the current dataset
                    this.buttonGridModeSave_Click(null, null);
                }
                //Creating the DataSet used as as template for the Copy form
                DiversityCollection.Datasets.DataSetCollectionSpecimen ds = (DiversityCollection.Datasets.DataSetCollectionSpecimen)this.dataSetCollectionSpecimen.Clone();
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventRow RE = ds.CollectionEvent.NewCollectionEventRow();
                    foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.CollectionEvent.Columns)
                    {
                        RE[C.ColumnName] = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0][C.ColumnName];
                    }
                    ds.CollectionEvent.Rows.Add(RE);
                }
                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenRow RS = ds.CollectionSpecimen.NewCollectionSpecimenRow();
                foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.CollectionSpecimen.Columns)
                {
                    RS[C.ColumnName] = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0][C.ColumnName];
                }
                ds.CollectionSpecimen.Rows.Add(RS);

                DiversityCollection.Forms.FormCopyDataset f = new FormCopyDataset(ds);
                f.AllowMultiCopy = true;

                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (f.AccessionNumbers.Count > 0)
                    {
                        foreach (string AccNr in f.AccessionNumbers)
                        {
                            string AccessionNumber = AccNr;
                            DiversityCollection.Forms.FormCopyDataset.EventCopyMode EventCopyMode = f.ModeForEventCopy;
                            bool CopyIdentification = f.CopyUnits;
                            int NumberOfCopies = f.NumberOfCopies;
                            int SpecimenID = -1;
                            int OriginalID = (int)this.SpecimenID;
                            try
                            {
                                SpecimenID = this.CopySpecimen(OriginalID, AccessionNumber, f.ModeForEventCopy, f.CopyUnits);
                                System.Data.DataTable dt = new DataTable();
                                string SQL = "SELECT " + this._SqlSpecimenFields + " FROM " + this.SourceFunction + " ('" + SpecimenID.ToString() + "')";
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                ad.Fill(dt);
                                for (int i = 0; i < NumberOfCopies; i++)
                                {
                                    DiversityCollection.Datasets.DataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimenRow Rnew = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.NewFirstLinesCollectionSpecimenRow();
                                    string MissungColumns = "";
                                    foreach (System.Data.DataColumn C in Rnew.Table.Columns)
                                    {
                                        if (dt.Columns.Contains(C.ColumnName))
                                        Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
                                        else { if (MissungColumns.Length > 0) MissungColumns += ", "; MissungColumns += C.ColumnName; }
                                    }
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Add(Rnew);
                                    System.Windows.Forms.DataGridViewCell Cell = this.dataGridViewActive.Rows[this.dataGridViewActive.Rows.Count - 2].Cells[0];
                                    if (Cell.Visible)
                                        this.dataGridViewActive.CurrentCell = Cell;
                                }
                            }
                            catch (Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonGridModeDelete_Click(object sender, EventArgs e)
        {
            bool DeleteSelection = false;
            this.buttonGridModeDelete.Enabled = false;
            System.Collections.Generic.List<int> IDsToDelete = new List<int>();
            try
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewActive.SelectedRows)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)R.DataBoundItem;
                    int ID = 0;
                    if (int.TryParse(RV["CollectionSpecimenID"].ToString(), out ID))
                    {
                        string AccessionNumber = RV["Accession_Number"].ToString();
                        if (!DeleteSelection)
                        {
                            string Message = DiversityWorkbench.Entity.Message("Do_you_want_to_delete_the_dataset");
                            if (AccessionNumber.Length > 0) Message += "\r\n" + DiversityWorkbench.Entity.EntityInformation("CollectionSpecimen.AccessionNumber")["DisplayText"] + ": " + AccessionNumber;
                            else Message += " ID " + ID.ToString();
                            if (this.dataGridViewActive.SelectedRows.Count > 1)
                                Message += " " + DiversityWorkbench.Entity.Message("and") + " " + (this.dataGridViewActive.SelectedRows.Count - 1).ToString() + " " + this.Message("further_datasets") + "?";
                            if (System.Windows.Forms.MessageBox.Show(Message, DiversityWorkbench.Entity.Message("delete_entry"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return;
                            else
                                DeleteSelection = true;
                        }
                    }
                    if (DeleteSelection)
                    {
                        IDsToDelete.Add(ID);
                        //this.deleteSpecimen(ID);
                        //RV.Delete();
                        //this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.AcceptChanges();
                    }
                }
                if (DeleteSelection)
                {
                    foreach (int ID in IDsToDelete)
                    {
                        this.deleteSpecimen(ID);
                        //RV.Delete();
                        //this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.AcceptChanges();
                    }
                    this.GridModeFillTable();
                }
            }
            catch { }
        }

        /// <summary>
        /// delete a specimen from the database
        /// </summary>
        /// <param name="ID">the Primary key of table CollectionSpecimen corresponding to the item that should be deleted</param>
        private void deleteSpecimen(int ID)
        {
            try
            {
                string SQL = "DELETE FROM CollectionSpecimen WHERE CollectionSpecimenID = " + ID.ToString();
                Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
                com.CommandType = System.Data.CommandType.Text;
                Conn.Open();
                com.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count > 0)
                this.buttonGridModeDelete.Enabled = true;
            else this.buttonGridModeDelete.Enabled = false;
        }

#endregion

#region Button events for filter and remove

        private void buttonRemoveRowFromGrid_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.RemoveRowFromGrid(this.dataGridViewActive, this.dataSetCollectionSpecimenGridMode);
            //if (this.dataGridView.SelectedRows.Count > 0)
            //{
            //    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
            //    {
            //        try
            //        {
            //            System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
            //            Rdata.Delete();
            //            Rdata.Row.AcceptChanges();
            //        }
            //        catch (System.Exception ex) { }
            //    }
            //    this.dataSetCollectionSpecimenGridMode.AcceptChanges();
            //}
            //else
            //    System.Windows.Forms.MessageBox.Show("No rows selected");
        }

        private void buttonFilterGrid_Click(object sender, EventArgs e)
        {
            this.filterGrid(this.dataGridViewActive);


            //if (this.dataGridView.SelectedCells != null)
            //{
            //    try
            //    {
            //        if (this.dataSetCollectionSpecimenGridMode.HasChanges())
            //        {
            //            if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //            {
            //                this.textBoxHeaderAccessionNumber.Focus();
            //                this.SaveAll();
            //                System.Windows.Forms.MessageBox.Show("Please try to filter again");
            //                this.dataGridView.CurrentCell = null;
            //                return;
            //            }
            //        }
            //        DiversityCollection.Forms.FormGridFunctions.FilterGrid(this.dataGridView);
            //    }
            //    catch (System.Exception ex)
            //    {
            //    }
            //    //if (this.dataGridView.SelectedCells[0].Value != null)
            //    //{
            //    //    string Filter = this.dataGridView.SelectedCells[0].Value.ToString();
            //    //    System.Collections.Generic.List<DataRow> RowsToRemove = new List<DataRow>();
            //    //    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
            //    //    {
            //    //        if ((R.Cells[this.dataGridView.SelectedCells[0].ColumnIndex].Value == null && Filter.Length > 0)
            //    //            || R.Cells[this.dataGridView.SelectedCells[0].ColumnIndex].Value.ToString() != Filter)
            //    //        {
            //    //            if (R.DataBoundItem != null)
            //    //            {
            //    //                System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
            //    //                RowsToRemove.Add(Rdata.Row);
            //    //            }
            //    //        }
            //    //    }
            //    //    foreach (System.Data.DataRow R in RowsToRemove)
            //    //        R.Delete();
            //    this.dataSetCollectionSpecimenGridMode.AcceptChanges();
            //    //}
            //}
        }

        private void filterGrid(System.Windows.Forms.DataGridView dataGrid)
        {
            if (dataGrid.SelectedCells != null)
            {
                try
                {
                    if (this.dataSetCollectionSpecimenGridMode.HasChanges())
                    {
                        if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.textBoxHeaderAccessionNumber.Focus();
                            this.SaveAll();
                            System.Windows.Forms.MessageBox.Show("Please try to filter again");
                            dataGrid.CurrentCell = null;
                            return;
                        }
                    }
                    DiversityCollection.Forms.FormGridFunctions.FilterGrid(this.dataGridView);
                }
                catch (System.Exception ex)
                {
                }
                this.dataSetCollectionSpecimenGridMode.AcceptChanges();
            }
        }

        #endregion

        #region Field visibility, handling of the tree

        private void SetToolTipsInTreeView(System.Windows.Forms.TreeNode Node)
        {
            try
            {
                foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
                {
                    if (N.Tag != null)
                    {
                        string[] Parameter = this.GridModeFieldTagArray(N.Tag.ToString());
                        if (Parameter.Length > 4)
                        {
                            string Description = "";
                            string AliasForColumn = Parameter[4];
                            switch (AliasForColumn)
                            {
                                case "Named_area":
                                    Description = "A named location selected from a thesaurus (e. g. 'Germany, Bavaria, Kleindingharting')";
                                    break;
                                case "Longitude":
                                    Description = "The longitude of the coordinates as WGS84";
                                    break;
                                case "Latitude":
                                    Description = "The latitude of the coordinates as WGS84";
                                    break;
                                case "Link_to_GoogleMaps":
                                    Description = "Retrieve the coordinates as WGS84 from Google Maps";
                                    break;
                                case "Altitude_from":
                                    Description = "Altitude (mNN) - lower value of the range";
                                    break;
                                case "Altitude_to":
                                    Description = "Altitude (mNN) - higher value of the range";
                                    break;
                                case "MTB":
                                    Description = "TK25 as used in Germany, Switzerland and Austria, older term MTB (= MessTischBlatt)";
                                    break;
                                case "Quadrant":
                                    Description = "The quadrants of TK25 as used in Germany, Switzerland and Austria, older term MTB (= MessTischBlatt)";
                                    break;
                                case "Notes_for_MTB":
                                    Description = "Notes related to TK25 as used in Germany, Switzerland and Austria, older term MTB (= MessTischBlatt)";
                                    break;
                                case "Geographic_region":
                                    Description = "Geographic regions as provided by the herbarium Görlitz (GLM)";
                                    break;
                                case "Lithostratigraphy":
                                    Description = "Lithostratigraphy according to the Bayerischen Staatssammlung für Paläontologie";
                                    break;
                                case "Chronostratigraphy":
                                    Description = "Chronostratigraphy according to the Bayerischen Staatssammlung für Paläontologie";
                                    break;
                                case "Biostratigraphy":
                                    Description = "Biostratigraphy";
                                    break;
                                case "_Transaction":
                                    Description = "The first transaction in which a part of the specimen is involved";
                                    break;
                                //case "On_loan":
                                //    Description = "The first part of the specimen that is on loan";
                                //    break;
                                case "_TransactionID":
                                    Description = "Link to the first transaction in which a part of the specimen is involved";
                                    break;
                                default:
                                    Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(Parameter[0], Parameter[3]);
                                    if (Parameter[4].StartsWith("Remove_"))
                                    {
                                        if (Parameter[3] == "Location2")
                                        {
                                            Description = Parameter[4].Replace("_", " ") + ". ";
                                            switch (Parameter[1])
                                            {
                                                case "NamedArea":
                                                    Description += "(Link to module DiversityGazetteer)";
                                                    break;
                                                case "SamplingPlot":
                                                    Description += "(Link to module DiversitySamplingPlots)";
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Description = Parameter[4].Replace("_", " ") + " - " + Description;
                                        }
                                    }
                                    break;
                            }
                            N.ToolTipText = Description;
                            this.setGridColumnToolTips(this.dataGridView, Description, AliasForColumn);
#if Autosuggest
                            this.setGridColumnToolTips(this.dataGridViewAutosuggest, Description, AliasForColumn);
#endif
                        }
                    }
                    this.SetToolTipsInTreeView(N);
                }

            }
            catch { }
        }

        private void GridModeSetFieldVisibilityForNodes()
        {
            try
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility.Length > 0 
                    && this.GridModeQueryFields.Count > 0)
                {
                    foreach (System.Windows.Forms.TreeNode Node in this.treeViewGridModeFieldSelector.Nodes)
                    {
                        this.GridModeSetFieldVisibilityForChildNodes(Node);
                    }
                }

            }
            catch  {  }
        }

        private void GridModeSetFieldVisibilityForChildNodes(System.Windows.Forms.TreeNode Node)
        {
            try
            {
                foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
                {
                    if (N.Tag != null)
                    {
                        string[] Parameter = this.GridModeFieldTagArray(N.Tag.ToString());
                        if (Parameter.Length > 4)
                        {
                            string AliasForColumn = Parameter[4];
                            if (AliasForColumn.Length == 0) AliasForColumn = Parameter[3];
                            N.Checked = GridModeFieldVisibility(AliasForColumn);
                        }
                    }
                    this.GridModeSetFieldVisibilityForChildNodes(N);
                }

            }
            catch  { }
        }

        private void GridModeSetToolTipsInTree()
        {
            foreach (System.Windows.Forms.TreeNode Node in this.treeViewGridModeFieldSelector.Nodes)
            {
                this.GridModeSetToolTipsInTree(Node);
            }
        }

        private void GridModeSetToolTipsInTree(System.Windows.Forms.TreeNode Node)
        {
            try
            {
                if (Node.Tag != null)
                {
                    string[] Parameter = this.GridModeFieldTagArray(Node.Tag.ToString());
                    if (Parameter.Length > 4)
                    {
                        string Table = Parameter[0];
                        string Column = Parameter[4];
                        Node.ToolTipText = this.FormFunctions.ColumnDescription(Table, Column);
                    }
                }
                foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
                    this.GridModeSetToolTipsInTree(N);

            }
            catch { }
        }

        private bool GridModeFieldVisibility(string ColumnName)
        {
            bool OK = false;
            try
            {
                foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                {
                    if (Q.AliasForColumn == ColumnName)
                        return Q.IsVisible;
                }

            }
            catch { }
            return OK;
        }

        private void GridModeSaveFieldVisibility()
        {
            string Visibility = "";
            try
            {
                if (this.GridModeQueryFields.Count > 0)
                {
                    foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                    {
                        if (Q.IsVisible) Visibility += "1";
                        else Visibility += "0";
                    }
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility = Visibility;
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                }

            }
            catch { }
        }

        private void treeViewGridModeFieldSelector_MouseUp(object sender, MouseEventArgs e)
        {
            //System.Windows.Forms.TreeNode N = this.treeViewGridModeFieldSelector.GetNodeAt(e.X, e.Y);
            //bool OK = N.Checked;
            //if (N.Tag == null && N.IsExpanded)
            //{
            //    foreach (System.Windows.Forms.TreeNode tn in N.Nodes)
            //    {
            //        tn.Checked = OK;
            //    }
            //}
        }

        private void treeViewGridModeFieldSelector_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //System.Windows.Forms.TreeNode N = e.Node; // this.treeViewGridModeFieldSelector.SelectedNode;
            //bool OK = N.Checked;
            //if (N.Tag == null && N.IsExpanded)//
            //{
            //    foreach (System.Windows.Forms.TreeNode tn in N.Nodes)
            //    {
            //        tn.Checked = OK;
            //    }
            //}
        }

        private void treeViewGridModeFieldSelector_AfterCheck(object sender, TreeViewEventArgs e)
        {
            System.Windows.Forms.TreeNode N = e.Node; // this.treeViewGridModeFieldSelector.SelectedNode;
            bool OK = N.Checked;
            try
            {
                if (N.Tag == null || (N.Tag != null && N.Tag.ToString().IndexOf(";") == -1))// && N.IsExpanded
                {
                    foreach (System.Windows.Forms.TreeNode tn in N.Nodes)
                    {
                        tn.Checked = OK;
                    }
                }

            }
            catch { }
        }

#endregion

#region Datahandling

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewGridMode_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.buttonGridModeDelete.Enabled = false;
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                this.labelGridViewReplaceColumnName.Text = ColumnName;
                if (this.buttonGridModeFind.Enabled == false)
                    this.buttonGridModeFind.Enabled = true;

                if (this.dataGridView.SelectedCells.Count > 0)
                    this.enableReplaceButtons(this.dataGridView, true);
                else this.enableReplaceButtons(this.dataGridView, false);

                if (e.ColumnIndex == this.dataGridView.SelectedCells[0].ColumnIndex)
                {
                    if (this.IsLinkColumn)
                    {
                        this.GetRemoteValues(this.dataGridView.SelectedCells[0]);
                    }
                    else
                    {
                        switch (ColumnName)
                        {
                            case "Link_to_GoogleMaps":
                                this.GetCoordinatesFromGoogleMaps();
                                break;
                            //case "On_loan":
                            case "_TransactionID":
                                int TransactionID;
                                if (int.TryParse(this.dataGridView.SelectedCells[0].Value.ToString(), out TransactionID))
                                {
                                    DiversityCollection.Forms.FormTransaction f = new FormTransaction(TransactionID);
                                    f.ShowDialog();
                                }
                                else
                                {
                                    string Message = "";
                                    //if (ColumnName == "On_loan") Message = "This dataset is not on loan";
                                    if (ColumnName == "_TransactionID") Message = "This dataset is not involved in a transaction";
                                    if (Message.Length > 0) System.Windows.Forms.MessageBox.Show(Message);
                                }
                                break;
                            case "Remove_link_to_gazetteer":
                            case "Remove_link_to_SamplingPlots":
                            case "Remove_link_for_collector":
                            case "Remove_link_for_Depositor":
                            case "Remove_link_to_exsiccatae":
                            case "Remove_link_of_reference_for_specimen":
                            case "Remove_link_for_identification":
                            case "Remove_link_for_reference":
                            case "Remove_link_for_reference_of_second_organism":
                            case "Remove_link_for_determiner":
                            case "Remove_link_for_responsible_of_second_organism":
                            case "Remove_link_for_second_organism":
                                this.RemoveLink(this.dataGridView.SelectedCells[0]);
                                break;
                            case "Analysis":
                                this.setAnalysis();
                                break;
                            case "Analysis_result":
                                if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID"].Equals(System.DBNull.Value))
                                    this.setAnalysis();
                                break;
                            case "Relation_is_internal":
                                System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                                bool IsInternal = bool.Parse(R["Relation_is_internal"].ToString()); // contains the previous setting i.e. the contrary to the new
                                string test = this.dataGridView.SelectedCells[0].Value.ToString();
                                if (IsInternal)
                                {
                                    R["Link_to_DiversityCollection_for_relation"] = "";
                                }
                                else
                                {
                                    string URL = R["Related_specimen_URL"].ToString();
                                    if (R["Related_specimen_URL"].ToString().StartsWith("http") && R["Related_specimen_URL"].ToString().ToLower().IndexOf("/collection") > -1)
                                        R["Link_to_DiversityCollection_for_relation"] = R["Related_specimen_URL"];
                                }
                                break;
                        }
                    }
                }
                if (this.textBoxHeaderID.Text.Length == 0)
                    this.setSpecimen(this.SpecimenID);

            }
            catch { }
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                // #276 - if allready set to column by e.g. dataGridViewGridMode_CellClick no need to set it again
                bool ColumnNameSet = this.labelGridViewReplaceColumnName.Text == ColumnName;
                if (!ColumnNameSet)
                    this.labelGridViewReplaceColumnName.Text = ColumnName;
                if (this.buttonGridModeFind.Enabled == false)
                    this.buttonGridModeFind.Enabled = true;
                if (!ColumnNameSet) // #276
                {
                    if ((this.dataGridView.SelectedCells.Count > 0 && ColumnName != "CollectionSpecimenID")
                        && (typeof(System.Windows.Forms.DataGridViewTextBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
                        && ColumnName != "CollectionSpecimenID")
                        //|| typeof(System.Windows.Forms.DataGridViewComboBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
                        )
                        this.enableReplaceButtons(this.dataGridView, true);
                    else this.enableReplaceButtons(this.dataGridView, false);
                }
                if (this.dataGridView.SelectedCells.Count == 1)
                    this.labelGridCounter.Text = "line " + (this.dataGridView.SelectedCells[0].RowIndex + 1).ToString() + " of " + (this.dataGridView.Rows.Count - 1).ToString();
                else if (this.dataGridView.SelectedCells.Count > 1)
                {
                    int StartLine = this.dataGridView.SelectedCells[0].RowIndex + 1;
                    if (this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1 < StartLine)
                        StartLine = this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1;
                    this.labelGridCounter.Text = "line " + StartLine.ToString() + " to " +
                        (this.dataGridView.SelectedCells.Count + StartLine - 1).ToString() + " of " +
                        (this.dataGridView.Rows.Count - 1).ToString();
                }
                else
                    this.labelGridCounter.Text = "line 1 of " + (this.dataGridView.Rows.Count - 1).ToString();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridView.SelectedCells.Count > 0 &&
                         this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                         this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0)
                    this.checkForMissingAndDefaultValues(this.dataGridView.SelectedCells[0], false);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void checkForMissingAndDefaultValues(System.Windows.Forms.DataGridViewCell Cell, bool Silent, string EditedValue = "", string DataProperty = "")
        {
            try
            {
                if (this.dataGridView.SelectedCells.Count > 0 &&
                     this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                     this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0)
                {
                    if (!this.dataGridView.Columns[Cell.ColumnIndex].Displayed && !this.dataGridView.Columns[Cell.ColumnIndex].Visible)
                        return;

                    //if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                    //{
                    //    this.updateSpecimen();
                    //}
                    string ColumnName = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
                    string Message = "";
                    string Value = Cell.EditedFormattedValue.ToString();
                    System.Data.DataRow Rcurr = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex];

                    // Checking if a correct value was entered
                    if (!DiversityCollection.Forms.FormGridFunctions.ValueIsValid(this.dataGridView, Cell.ColumnIndex, Value))
                    {
                        System.Windows.Forms.MessageBox.Show(Value + " is not a valid value for " + ColumnName + ".");
                    }

                    switch (ColumnName)
                    {
                        case "Taxonomic_name_of_second_organism":
                        case "Life_stage_of_second_organism":
                        case "Gender_of_second_organism":
                        case "Number_of_units_of_second_organism":
                        case "Circumstances_of_second_organism":
                        case "Notes_for_organism_of_second_organism":
                        case "Vernacular_term_of_second_organism":
                        case "Identification_day_of_second_organism":
                        case "Identification_month_of_second_organism":
                        case "Identification_year_of_second_organism":
                        case "Identification_category_of_second_organism":
                        case "Identification_qualifier_of_second_organism":
                        case "Type_status_of_second_organism":
                        case "Type_notes_of_second_organism":
                        case "Notes_for_identification_of_second_organism":
                        case "Reference_title_of_second_organism":
                        case "Responsible_of_second_organism":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Taxonomic_group_of_second_organism"].Equals(System.DBNull.Value)
                                && !Silent)
                                System.Windows.Forms.MessageBox.Show("Please select a taxonomic group for this organism");
                            break;
                        case "AnalysisID":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Analysis_number"].Equals(System.DBNull.Value))
                            {
                                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Analysis_number"] = 1;
                            }
                            goto case "Exsiccata_number";
                        case "Analysis_number":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["AnalysisID"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnitAnalysis.AnalysisID")
                                    && this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"].Length > 0)
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["AnalysisID"] = this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"];
                                else
                                    if (!Silent) Message += "Please select an analysis type\r\n";
                            }
                            goto case "Exsiccata_number";
                        case "Analysis_result":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Analysis_number"].Equals(System.DBNull.Value))
                            {
                                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Analysis_number"] = 1;
                            }
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["AnalysisID"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnitAnalysis.AnalysisID")
                                    && this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"].Length > 0)
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["AnalysisID"] = this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"];
                                else
                                    if (!Silent) Message += "Please select an analysis type\r\n";
                            }
                            goto case "Exsiccata_number";
                        case "Exsiccata_number":
                        case "Relation_type":
                        case "Colonised_substrate_part":
                        case "Life_stage":
                        case "Gender":
                        case "Number_of_units":
                        case "Circumstances":
                        case "Order_of_taxon":
                        case "Family_of_taxon":
                        case "Notes_for_organism":
                        case "Taxonomic_name":
                        case "Vernacular_term":
                        case "Identification_day":
                        case "Identification_month":
                        case "Identification_year":
                        case "Identification_category":
                        case "Identification_qualifier":
                        case "Type_status":
                        case "Type_notes":
                        case "Notes_for_identification":
                        case "Reference_title":
                        case "Responsible":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Taxonomic_group"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup")
                                    && this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Taxonomic_group"] = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
                                else
                                    if (!Silent) Message += "Please select a taxonomic group for this organism";
                            }
                            break;
                        case "Collection":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Material_category"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Material_category"] = this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"];
                                else
                                    if (!Silent) Message = "Please select a material category for this part.";
                            }
                            break;
                        case "Material_category":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Collection"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Collection"] = this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
                                else
                                {
#if DEBUG
                                    if (this.checkBoxAutoSuggest.Checked && DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                                    {
                                        if (!Silent)
                                        {

                                        }
                                    }
                                    if (this.dataGridView.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value != null)
                                    {
                                        string Text = this.dataGridView.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value.ToString();
                                    }
#endif
                                    if (!Silent) Message = "Please select a collection for this part or set a default collection in the settings.";
                                }
                            }
                            break;
                        case "Storage_location":
                        case "Stock":
                        case "Preparation_method":
                        case "Preparation_date":
                        case "Storage_container":
                        case "Part_accession_number":
                        case "Notes_for_part":
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Material_category"].Equals(System.DBNull.Value))
                            {
                                if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest && 
                                    this.dataGridViewAutosuggest.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].EditedFormattedValue.ToString().Length > 0 &&
                                    this.dataGridViewAutosuggest.Columns[Cell.ColumnIndex].DataPropertyName == "Material_category")
                                {
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Material_category"] = this.dataGridViewAutosuggest.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].EditedFormattedValue.ToString();
                                }
                                else if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Material_category"] = this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"];
                                else
                                {
                                    if (!Silent) Message = "Please select a material category for this part.\r\n";
                                }
                            }
                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Collection"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Collection"] = this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
                                else if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection > -1)
                                {
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Collection"] = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection;
                                }
                                else
                                {
                                    if (!Silent)
                                    {
                                        System.Data.DataTable dtCollection = DiversityCollection.LookupTable.DtCollectionWithHierarchy;
                                        DiversityWorkbench.Forms.FormGetItemFromHierarchy f = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(dtCollection, "CollectionID", "CollectionParentID", "DisplayText", "CollectionID",
                                            "Please select a collection", "Collection");
                                        f.ShowDialog();
                                        if (f.DialogResult == DialogResult.OK && f.SelectedValue.Length > 0)
                                        {
                                            int CollID;
                                            if (int.TryParse(f.SelectedValue, out CollID))
                                            {
                                                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Cell.RowIndex]["Collection"] = CollID;
                                            }
                                        }
                                        else
                                            Message += "Please select a collection for this part.";
                                    }

                                }
                            }
                            break;
                        case "Altitude_from":
                            float AltTo = 0;
                            float AltFrom = 0;
                            if (float.TryParse(Value, out AltFrom))
                            {
                                if (!Rcurr["Altitude_to"].Equals(System.DBNull.Value))
                                {
                                    if (float.TryParse(Rcurr["Altitude_from"].ToString(), out AltTo))
                                        Rcurr["_AverageAltitudeCache"] = (AltFrom + AltTo) / 2;
                                }
                                else
                                {
                                    Rcurr["_AverageAltitudeCache"] = AltFrom;
                                }
                                Rcurr["Altitude_from"] = AltFrom;
                            }
                            break;
                        case "Altitude_to":
                            if (float.TryParse(Value, out AltTo))
                            {
                                if (!Rcurr["Altitude_from"].Equals(System.DBNull.Value))
                                {
                                    if (float.TryParse(Rcurr["Altitude_from"].ToString(), out AltFrom))
                                        Rcurr["_AverageAltitudeCache"] = (AltFrom + AltTo) / 2;
                                }
                                else
                                {
                                    Rcurr["_AverageAltitudeCache"] = AltTo;
                                }
                                Rcurr["Altitude_to"] = AltTo;
                            }
                            break;
                    }
                    if (Message.Length > 0)
                        if (!Silent) 
System.Windows.Forms.MessageBox.Show(Message);
                }
                if (this.dataGridView.SelectedCells.Count > 0)
                    this.setCellBlockings(this.dataGridView, Cell.RowIndex);

            }
            catch { }
        }


        private void addMissingPart(System.Windows.Forms.DataGridViewCell Cell)
        {

        }

        private void dataGridViewGridMode_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this._FormState == Forms.FormGridFunctions.FormState.Loading)
                    return;
                int SpecimenID = 0;
                if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex < this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count &&
                    int.TryParse(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine]["CollectionSpecimenID"].ToString(), out SpecimenID))
                {
                    if (SpecimenID != this._SpecimenID)
                    {
                        this.setSpecimen(SpecimenID);
                        this._SpecimenID = SpecimenID;
                    }
                }
                else if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count)
                {
                    this.insertNewDataset(this.dataGridView.SelectedCells[0].RowIndex);
                }
                else if (this.dataGridView.SelectedCells.Count == 0
                    && this.dataGridView.Rows.Count > 0
                    && this.dataGridView.Rows[0].Cells[0].Value != null)
                {
                    if (int.TryParse(this.dataGridView.Rows[0].Cells[0].Value.ToString(), out SpecimenID))
                    {
                        this.setSpecimen(SpecimenID);
                        this._SpecimenID = SpecimenID;
                    }
                }
                else if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count == 0)
                {
                    this.insertNewDataset(0);
                }
                this.setCellBlockings();
                this.setRemoveCellStyle(this.dataGridView);

            }
            catch { }
        }

        private void dataGridViewGridMode_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            // Sicherung nur bei Schliessen des Formulars oder expliziter Anforderung
            // ansonsten wird bei Laden bereits gesichert
            //if (this.dataGridView.SelectedCells != null && this.dataGridView.SelectedCells.Count > 0)
            //{
            //    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.dataGridView.SelectedCells[0].RowIndex].BeginEdit();
            //    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.dataGridView.SelectedCells[0].RowIndex].EndEdit();
            //    this.GridModeUpdate(this.dataGridView.SelectedCells[0].RowIndex);
            //}
        }

        private string GridModeFillCommand()
        {
            string SQL = "SELECT " + this._SqlSpecimenFields + " FROM dbo." + this.SourceFunction + " ";

            try
            {
                string WhereClause = "";
                int ii = 0;
                foreach (int i in this._IDs)
                {
                    ii++;
                    if (WhereClause.Length + i.ToString().Length + 2 >= 8000)
                    {
                        string Message = "The last " + (this._IDs.Count - ii).ToString() + "\r\nof " + this._IDs.Count.ToString() + " items are not shown";
                        System.Windows.Forms.MessageBox.Show(Message);
                        break;
                    }
                    WhereClause += i.ToString() + ", ";
                }
                if (WhereClause.Length == 0) WhereClause = " ('') ";
                else
                    WhereClause = " ('" + WhereClause.Substring(0, WhereClause.Length - 2) + "')";

                SQL += WhereClause + " ORDER BY Accession_number ";
            }
            catch { }
            return SQL;
        }

        private void updateSpecimenFromGrid(System.Windows.Forms.DataGridViewRow Row)
        {
            try
            {
                for (int i = 0; i < this.dataGridView.Columns.Count; i++)
                {
                    string DataColumn = this.dataGridView.Columns[i].DataPropertyName;
                    DiversityCollection.Forms.GridModeQueryField Q = this.GridModeGetQueryField(DataColumn);
                    System.Data.DataTable DT = this.dataSetCollectionSpecimen.Tables[Q.Table];
                    if (Q.Restriction.Length > 0)
                    {
                        System.Data.DataRow[] RR = DT.Select(Q.Restriction);
                    }
                    else
                    {
                        System.Data.DataRow R = DT.Rows[0];
                    }
                }

            }
            catch { }
        }

        private void GridModeUpdate(int Index)
        {
            try
            {
                if (this.dataSetCollectionSpecimenGridMode.HasChanges())
                {
                    System.Data.DataRow RDataset = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index];
                    if (RDataset.RowState == DataRowState.Modified || RDataset.RowState == DataRowState.Added)
                    {
                        // setting the dataset
                        // the dataset is filled with the original data from the database as a basis for comparision with the data in the grid
                        int CollectionSpecimenID = int.Parse(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["CollectionSpecimenID"].ToString());
                        this._SpecimenID = CollectionSpecimenID;
                        this.fillSpecimen(this._SpecimenID);

                        // if no data 
                        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count == 0)
                            this.setSpecimen(this._SpecimenID);

                        // getting a list of all tables (Alias + TableName) from the grid
                        System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();
                        int iii = 0;
                        foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                        {
                            if (Q.AliasForTable != null)
                            {
                                if (!Tables.ContainsKey(Q.AliasForTable))
                                    Tables.Add(Q.AliasForTable, Q.Table);
                            }
                            else
                            {
                                if (!Tables.ContainsKey(Q.Table))
                                    Tables.Add(Q.Table, Q.Table);
                            }
                            iii++;
                        }

                        // for every table (Alias) perform update of the entries as compared with the original data
                        // or insert if the data are missing
                        // the only exception is CollectionAgent as there the name is part of the PK
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Tables)
                        {
                            System.Collections.Generic.Dictionary<string, string> TableColumns = new Dictionary<string, string>();
                            System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
                            
                            // get all columns of the current table
                            iii = 0;
                            foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                            {
                                if (!TableColumns.ContainsKey(Q.AliasForColumn)
                                    && Q.AliasForTable == KV.Key
                                    && Q.Column.Length > 0
                                    && !Q.AliasForColumn.StartsWith("Remove")
                                    && Q.AliasForColumn != "Link_to_DiversityCollection_for_relation")
                                {
                                    TableColumns.Add(Q.AliasForColumn, Q.Column);
                                    ColumnValues.Add(Q.Column, "");
                                }
                                iii++;
                            }
                            
                            // no rows in the table - add new entry
                            // check if there are any values
                            bool AnyValuePresent = false;
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVc in TableColumns)
                            {
                                if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Columns.Contains(KVc.Key))
                                {
                                    ColumnValues[KVc.Value] = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index][KVc.Key].ToString();
                                    if (ColumnValues[KVc.Value].Length > 0)
                                        AnyValuePresent = true;
                                }
                            }

                            if (this.dataSetCollectionSpecimen.Tables[KV.Value].Rows.Count == 0 &&
                                AnyValuePresent)
                                this.GridModeInsertNewData(KV.Value, KV.Key, Index);
                            else if (KV.Value == "CollectionEvent" &&
                                this.dataSetCollectionSpecimen.Tables[KV.Value].Rows.Count == 0 &&
                                !AnyValuePresent)
                            {
                                bool EventDependentDataPresent = false;
                                foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                                {
                                    if (Q.Table == "CollectionEventLocalisation" || Q.Table == "CollectionEventProperty")
                                    {
                                        if (!RDataset[Q.AliasForColumn].Equals(System.DBNull.Value))
                                        {
                                            EventDependentDataPresent = true;
                                            break;
                                        }
                                    }
                                }
                                if (EventDependentDataPresent)
                                    this.GridModeInsertNewData(KV.Value, KV.Key, Index);
                            }
                            else if (AnyValuePresent)
                            {
                                string WhereClause = "";
                                string Value = "";
                                bool containsHyphen = false;
                                
                                // get the primary key columns of the table
                                System.Data.DataColumn[] PK = this.dataSetCollectionSpecimen.Tables[KV.Value].PrimaryKey;
                                System.Collections.Generic.List<string> PKColumns = new List<string>();
                                for (int i = 0; i < PK.Length; i++)
                                    PKColumns.Add(PK[i].ColumnName);

                                // get the primary key with its original values
                                System.Collections.Generic.Dictionary<string, string> PKvalues = new Dictionary<string, string>();
                                foreach (System.Data.DataColumn Col in PK)
                                {
                                    // get the column name in the view
                                    string ColumnAlias = "";
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KvPK in TableColumns)
                                    {
                                        if (KvPK.Value == Col.ColumnName)
                                        {
                                            ColumnAlias = KvPK.Key;
                                            PKvalues.Add(KvPK.Key, "");
                                            break;
                                        }
                                    }
                                    
                                    // get the value of the column
                                    // TODO: Notlösung - bei ' wird mit LIKE und _ gesucht
                                    if (ColumnAlias.Length > 0 && !RDataset[ColumnAlias].Equals(System.DBNull.Value))
                                    {
                                        PKvalues[ColumnAlias] = RDataset[ColumnAlias].ToString();
                                        Value = RDataset[ColumnAlias].ToString();
                                        if (Value.IndexOf("'") > -1)
                                        {
                                            containsHyphen = true;
                                        }
                                        if (WhereClause.Length > 0) WhereClause += " AND ";
                                        if (containsHyphen)
                                        {
                                            string[] WW = Value.Split(new char[]{'\''});
                                            for (int i = 0; i < WW.Length; i++)
                                            {
                                                if (WW[i].Length > 0)
                                                {
                                                    if (i > 0 && WhereClause.Length > 0) WhereClause += " AND ";
                                                    WhereClause += " " + Col.ColumnName;
                                                    WhereClause += " LIKE '";
                                                    if (i > 0) WhereClause += "%";
                                                    WhereClause += WW[i];
                                                    if (i < WW.Length) WhereClause += "%";
                                                    WhereClause += "' ";
                                                }
                                            }
                                        }
                                        else WhereClause += " " + Col.ColumnName + " = '" + Value + "' ";
                                    }
                                    else
                                    {
                                        if (Col.ColumnName != "CollectionSpecimenID")
                                        {
                                            switch (Col.ColumnName)
                                            {
                                                case "IdentificationUnitID":
                                                    switch (KV.Key)
                                                    {
                                                        case "Identification":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " IdentificationUnitID = " + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_IdentificationUnitID"].ToString();
                                                            break;
                                                        case "SecondUnit":
                                                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondUnitID"].Equals(System.DBNull.Value))
                                                                this.GridModeInsertNewData(KV.Value, KV.Key, Index);
                                                            //continue;
                                                            WhereClause += " IdentificationUnitID = " + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondUnitID"].ToString();
                                                            break;
                                                        case "SecondUnitIdentification":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondUnitID"].Equals(System.DBNull.Value))
                                                            {
                                                                string Message = "Please enter the taxonomic group for the second organism\r\n";
                                                                if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value))
                                                                    Message += "Taxonomic name: " + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].ToString() + "\r\n";
                                                                if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Accession_number"].Equals(System.DBNull.Value))
                                                                    Message += "Accession number: " + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Accession_number"].ToString();
                                                                Message += "\r\n\r\nThe new identification can not be saved";
                                                                System.Windows.Forms.MessageBox.Show(Message, "Missing taxonomic group");
                                                                return;
                                                            }
                                                            WhereClause += " IdentificationUnitID = " + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondUnitID"].ToString();
                                                            break;
                                                        case "IdentificationUnitAnalysis":
                                                            break;
                                                    }
                                                    break;
                                                case "IdentificationSequence":
                                                    switch (KV.Key)
                                                    {
                                                        case "Identification":
                                                            if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_IdentificationSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_IdentificationSequence"].ToString();
                                                            }
                                                            break;
                                                        case "SecondUnitIdentification":
                                                            if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondSequence"].ToString();
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }

                                // get a possible restriction
                                foreach (DiversityCollection.Forms.GridModeQueryField Qrest in this.GridModeQueryFields)
                                {
                                    try
                                    {
                                        if (Qrest.AliasForTable == KV.Key && Qrest.Restriction != null && Qrest.Restriction.Length > 0)
                                        {
                                            if (WhereClause.Length > 0)
                                                WhereClause += " AND " + Qrest.Restriction;
                                            else
                                                WhereClause = Qrest.Restriction;
                                            break;
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        System.Windows.Forms.MessageBox.Show(ex.Message);
                                    }
                                }
                                try
                                {
                                    int DisplayOrder = 1;
                                    System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select(WhereClause);

                                    // special treatment for Agent as CollectorsName is contained in the PK
                                    if (RR.Length == 0 && KV.Value == "CollectionAgent" && WhereClause.StartsWith(" CollectorsName "))
                                    {
                                        string NewCollectorsName = ""; 
                                        if (containsHyphen)
                                            NewCollectorsName = Value.Trim();
                                        else
                                            NewCollectorsName = WhereClause.Substring(18).Replace("'", "").Trim();
                                        if (NewCollectorsName.Length > 0)
                                        {
                                            RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select("", "CollectorsSequence");
                                            RR[0]["CollectorsName"] = NewCollectorsName;
                                        }
                                        if (RR.Length > 1)
                                        {
                                            RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select("CollectorsName <> ''", "CollectorsSequence");
                                            if (RR.Length > 1)
                                                RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select("CollectorsName = '" + RR[0].ToString() + "'", "CollectorsSequence");
                                        }
                                    }

                                    if (RR.Length == 1)// && WhereClause.Length > 0)
                                    {
                                        // Check all columns in the table if the value has changed and write changes in the dataset
                                        foreach (System.Collections.Generic.KeyValuePair<string, string> KvTableColumn in TableColumns)
                                        {
                                            if (!PKColumns.Contains(KvTableColumn.Value) &&
                                                RR[0].Table.Columns.Contains(KvTableColumn.Value) &&
                                                RR[0][KvTableColumn.Value].ToString() != RDataset[KvTableColumn.Key].ToString())
                                            {
                                                RR[0][KvTableColumn.Value] = RDataset[KvTableColumn.Key];
                                            }
                                        }

                                        if (KV.Key == "SecondUnit")
                                        {
                                            System.Data.DataRow[] RRDisplayOrder = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder");
                                            if (RRDisplayOrder.Length > 0)
                                            {
                                                if (int.TryParse(RRDisplayOrder[0]["DisplayOrder"].ToString(), out DisplayOrder))
                                                {
                                                    System.Data.DataRow[] RRsecond = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > " + DisplayOrder.ToString(), "DisplayOrder");
                                                    if (RRsecond.Length > 0)
                                                    {
                                                        if (int.TryParse(RRsecond[0]["DisplayOrder"].ToString(), out DisplayOrder))
                                                        {
                                                            RR[0]["DisplayOrder"] = DisplayOrder;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (KV.Key == "CollectionSpecimen")
                                        {
                                            if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_CollectionEventID"].Equals(System.DBNull.Value) &&
                                                this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
                                                this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"] = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_CollectionEventID"];
                                        }
                                        //else if (KV.Key == "CollectionSpecimenRelation")
                                        //{
                                        //}
                                    }
                                    else if (containsHyphen && RR.Length > 1)
                                    {
                                        // if the PK contains values that can not be separated on the basis of like statements
                                        // according to the parts split by the hyphens, e.g. "a'b'c'd" and "a'c'b'd"
                                        System.Data.DataRow Rhyphen = RR[0];
                                        for (int i = 0; i < RR.Length; i++)
                                        {
                                            foreach (System.Data.DataColumn C in RR[0].Table.Columns)
                                            {
                                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVpk in PKvalues)
                                                {
                                                    string Column = this.ColumnNameForAlias(KVpk.Key);
                                                    if (Column == C.ColumnName &&
                                                        KVpk.Value == RR[i][Column].ToString())
                                                    {
                                                        Rhyphen = RR[i];
                                                        goto Found;
                                                    }
                                                }
                                            }
                                        }
                                        Found:
                                        // Check all columns in the table if the value has changed and write changes in the dataset
                                        foreach (System.Collections.Generic.KeyValuePair<string, string> KvTableColumn in TableColumns)
                                        {
                                            if (!PKColumns.Contains(KvTableColumn.Value) &&
                                                Rhyphen.Table.Columns.Contains(KvTableColumn.Value) &&
                                                Rhyphen[KvTableColumn.Value].ToString() != RDataset[KvTableColumn.Key].ToString())
                                            {
                                                Rhyphen[KvTableColumn.Value] = RDataset[KvTableColumn.Key];
                                            }
                                        }

                                    }
                                    else
                                    {
                                        this.GridModeInsertNewData(KV.Value, KV.Key, Index);
                                    }
                                }
                                catch (System.Exception ex)
                                {}
                            }
                        }
                        this.updateSpecimen();
                        RDataset.AcceptChanges();
                    }
                }
            }
            catch (System.Exception ex) 
            { }
        }

        private void GridModeInsertNewData(string Table, string AliasForTable, int Index)
        {
            bool PKcontainsIdentity = false;
            // security checks for dependent tables
            // Second Identification
            try
            {
                if (Table == "Identification" && AliasForTable == "SecondUnitIdentification")
                {
                    if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_group_of_second_organism"].Equals(System.DBNull.Value) ||
                        this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_group_of_second_organism"].ToString().Length == 0)
                    {
                        string Message = "Please enter the taxonomic group for the second organism ";
                        if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value))
                            Message += this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].ToString() + " ";
                        if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Accession_number"].Equals(System.DBNull.Value))
                            Message += this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Accession_number"].ToString();
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                }
                // get all columns of the table
                System.Collections.Generic.Dictionary<string, string> TableColumns = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
                foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                {
                    if (!TableColumns.ContainsKey(Q.AliasForColumn) 
                        && Q.AliasForTable == AliasForTable
                        && !Q.AliasForColumn.StartsWith("Remove")
                        && Q.AliasForColumn != "Link_to_DiversityCollection_for_relation")
                    {
                        TableColumns.Add(Q.AliasForColumn, Q.Column);
                        ColumnValues.Add(Q.Column, "");
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> KVc in TableColumns)
                {
                    ColumnValues[KVc.Value] = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index][KVc.Key].ToString();
                }

                // get the primary key columns of the table
                System.Data.DataColumn[] PK = this.dataSetCollectionSpecimen.Tables[Table].PrimaryKey;
                System.Collections.Generic.Dictionary<string, string> PKColumns = new Dictionary<string, string>();
                for (int i = 0; i < PK.Length; i++)
                {
                    PKColumns.Add(PK[i].ColumnName, "");
                    if (PK[i].AutoIncrement)
                        PKcontainsIdentity = true;
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PKColumns)
                {
                    if (!TableColumns.ContainsValue(KV.Key))
                    {
                        string AliasForColumn = "";
                        foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                        {
                            if (Q.Column == KV.Key
                                && (Q.AliasForTable == AliasForTable
                                || AliasForTable.StartsWith(Q.AliasForTable)))
                            {
                                AliasForColumn = Q.AliasForColumn;
                                break;
                            }
                        }
                        if (AliasForColumn.Length > 0)
                        {
                            TableColumns.Add(AliasForColumn, KV.Key);
                            ColumnValues.Add(KV.Key, "");
                        }
                        else
                        {
                            TableColumns.Add(KV.Key, KV.Key);
                            ColumnValues.Add(KV.Key, "");
                        }
                    }
                }

                // if there is a restriction, get the column and the value
                string ResColumn = "";
                string ResValue = "";
                char[] charSeparators = new char[] { '=' };
                string[] Parameter = null;
                foreach (DiversityCollection.Forms.GridModeQueryField Qrest in this.GridModeQueryFields)
                {
                    if (Qrest.AliasForTable == AliasForTable && Qrest.Restriction != null && Qrest.Restriction.Length > 0)
                    {
                        Parameter = Qrest.Restriction.Split(charSeparators);
                        break;
                    }
                }
                if (Parameter != null && Parameter.Length > 1)
                {
                    ResColumn = Parameter[0];
                    ResValue = Parameter[1];
                }
                if (ColumnValues.ContainsKey(ResColumn))
                    ColumnValues[ResColumn] = ResValue;

                foreach (System.Collections.Generic.KeyValuePair<string, string> KVpk in PKColumns)
                {
                    if (ColumnValues[KVpk.Key] == "")
                    {
                        // try to get the value from the table
                        string AliasForColumn = "";
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in TableColumns)
                        {
                            if (KVcol.Value == KVpk.Key)
                            {
                                AliasForColumn = KVcol.Key;
                                break;
                            }
                        }
                        if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Columns.Contains(AliasForColumn))
                        {
                            foreach (DiversityCollection.Forms.GridModeQueryField Qrest in this.GridModeQueryFields)
                            {
                                if (Qrest.Column == KVpk.Key)
                                {
                                    AliasForColumn = Qrest.AliasForColumn;
                                    break;
                                }
                            }
                        }
                        ColumnValues[KVpk.Key] = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index][AliasForColumn].ToString();
                    }
                }

                // test if all PrimaryKey values are set
                string TaxonomicGroup = "";
                int DisplayOrder = 1;
                string LastIdentification = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KVpk in PKColumns)
                {
                    if (ColumnValues[KVpk.Key].Length == 0)
                    {
                        // the value for a primary key column is missing
                        switch (KVpk.Key)
                        {
                            case "CollectionSpecimenID":
                                // if a new line is entered in the datagrid
                                if (Table == "CollectionSpecimen")
                                {
                                    int CollectionSpecimenID = 0;
                                    if (int.TryParse(this.InsertNewSpecimen(Index).ToString(), out CollectionSpecimenID))
                                    {
                                        if (CollectionSpecimenID < 0)
                                            return;
                                        else
                                            ColumnValues["CollectionSpecimenID"] = CollectionSpecimenID.ToString();
                                    }
                                }
                                break;
                            case "CollectionEventID":
                                // if a previously empty part related to the event is filled
                                string Locality = "";
                                if (!this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen[Index]["Locality_description"].Equals(System.DBNull.Value))
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen[Index]["Locality_description"].ToString();
                                Locality = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen[Index]["Locality_description"].ToString();
                                int EventID = this.createEvent(Index);
                                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen[Index]["_CollectionEventID"] = EventID;
                                ColumnValues["CollectionEventID"] = EventID.ToString();
                                if (Locality.Length > 0 
                                    && ColumnValues.ContainsKey("LocalityDescription"))
                                    ColumnValues["LocalityDescription"] = Locality;
                                break;
                            case "IdentificationUnitID":
                                // if a previously empty part related to the unit or second unit is filled
                                switch (AliasForTable)
                                {
                                    case "IdentificationUnit":
                                        TaxonomicGroup = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_group"].ToString();
                                        if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
                                            LastIdentification = TaxonomicGroup;
                                        else
                                            LastIdentification = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name"].ToString();
                                        DisplayOrder = 1;
                                        //this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Last_identification"] = LastIdentification;
                                        break;
                                    case "SecondUnit":
                                        TaxonomicGroup = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
                                        if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
                                            LastIdentification = TaxonomicGroup;
                                        else
                                            LastIdentification = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
                                        DisplayOrder = 2;
                                        break;
                                }
                                if (TaxonomicGroup.Length > 0 && LastIdentification.Length > 0)
                                {
                                    int UnitID = this.createIdentificationUnit(Index, AliasForTable);
                                    ColumnValues[KVpk.Key] = UnitID.ToString();
                                    switch (AliasForTable)
                                    {
                                        case "IdentificationUnit":
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_IdentificationUnitID"] = UnitID;
                                            break;
                                        case "SecondUnit":
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondUnitID"] = UnitID;
                                            break;
                                    }
                                }
                                break;
                            case "IdentificationSequence":
                                // if a previously empty part related to the identification of the unit or second unit is filled
                                System.Data.DataRow[] RRIdent = this.dataSetCollectionSpecimen.Identification.Select("", "IdentificationSequence DESC");
                                if (RRIdent.Length > 0)
                                {
                                    int Sequence = int.Parse(RRIdent[0]["IdentificationSequence"].ToString()) + 1;
                                    ColumnValues[KVpk.Key] = Sequence.ToString();
                                    switch (AliasForTable)
                                    {
                                        case "Identification":
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_IdentificationSequence"] = Sequence;
                                            break;
                                        case "IdentificationSecondUnit":
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondSequence"] = Sequence;
                                            break;
                                    }
                                }
                                else
                                {
                                    ColumnValues[KVpk.Key] = "1";
                                    switch (AliasForTable)
                                    {
                                        case "Identification":
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_IdentificationSequence"] = 1;
                                            break;
                                        case "IdentificationSecondUnit":
                                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SecondSequence"] = 1;
                                            break;
                                    }
                                }
                                break;
                            case "SpecimenPartID":
                                System.Data.DataRow[] RRPart = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("", "SpecimenPartID DESC");
                                if (RRPart.Length > 0)
                                {
                                    int PartID = int.Parse(RRPart[0]["SpecimenPartID"].ToString()) + 1;
                                    ColumnValues[KVpk.Key] = PartID.ToString();
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SpecimenPartID"] = PartID;
                                }
                                else
                                {
                                    int PartID = this.createCollectionSpecimenPart(Index, AliasForTable);
                                    ColumnValues[KVpk.Key] = PartID.ToString();
                                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["_SpecimenPartID"] = PartID;
                                }
                                break;
                        }
                    }
                }

                System.Data.DataRow Rnew = this.dataSetCollectionSpecimen.Tables[Table].NewRow();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
                {
                    if (Rnew.Table.Columns.Contains(KV.Key))
                    {
                        if (KV.Value.Length > 0)
                            Rnew[KV.Key] = KV.Value;
                        else
                        {
                            if (Rnew.Table.Columns[KV.Key].DataType == typeof(int) ||
                                Rnew.Table.Columns[KV.Key].DataType == typeof(System.Int16) ||
                                Rnew.Table.Columns[KV.Key].DataType == typeof(System.DateTime))
                                Rnew[KV.Key] = System.DBNull.Value;
                            else if (Rnew[KV.Key].GetType() == typeof(string))
                                Rnew[KV.Key] = "";
                        }
                    }
                }
                if (Table == "IdentificationUnit" &&
                    (Rnew["LastIdentificationCache"].Equals(System.DBNull.Value) ||
                    Rnew["LastIdentificationCache"].ToString().Length == 0)
                    && !Rnew["TaxonomicGroup"].Equals(System.DBNull.Value))
                {
                    if (LastIdentification.Length > 0)
                        Rnew["LastIdentificationCache"] = LastIdentification;
                    else Rnew["LastIdentificationCache"] = Rnew["TaxonomicGroup"];
                }

                if (Table == "IdentificationUnit" &&
                    Rnew["DisplayOrder"].Equals(System.DBNull.Value))
                    Rnew["DisplayOrder"] = DisplayOrder;

                if (Table == "CollectionEvent" &&
                    Rnew["Version"].Equals(System.DBNull.Value))
                    Rnew["Version"] = 1;

                if (Table == "CollectionSpecimen" &&
                    Rnew["Version"].Equals(System.DBNull.Value))
                    Rnew["Version"] = 1;

                if (Table == "CollectionSpecimen" &&
                    Rnew["CollectionEventID"].Equals(System.DBNull.Value) &&
                    !this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen[Index]["_CollectionEventID"].Equals(System.DBNull.Value))
                    Rnew["CollectionEventID"] = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen[Index]["_CollectionEventID"];

                if (Table == "CollectionSpecimenPart" &&
                    Rnew["SpecimenPartID"].Equals(System.DBNull.Value))
                {
                    if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                    {
                        string SQL = "";

                        //Rnew["SpecimenPartID"] = -1; // Test mit -1
                    }
                    else
                        Rnew["SpecimenPartID"] = 1;

                }

                if (Table == "IdentificationUnitAnalysis" &&
                    Rnew["AnalysisNumber"].Equals(System.DBNull.Value))
                    Rnew["AnalysisNumber"] = "1";


                // setting default for responsible
                if (Table == "Identification" &&
                    Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
                    Specimen.DefaultUseIdentificationResponsible)
                {
                    Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
                    Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
                }

                if (Table == "CollectionAgent" &&
                    Rnew["CollectorsName"].Equals(System.DBNull.Value) &&
                    Specimen.DefaultUseCollector)
                {
                    Rnew["CollectorsName"] = Specimen.DefaultResponsibleName;
                    Rnew["CollectorsAgentURI"] = Specimen.DefaultResponsibleURI;
                }

                if (Table == "CollectionEventLocalisation" &&
                    Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
                    Specimen.DefaultUseLocalisationResponsible)
                {
                    Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
                    Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
                }

                if (Table == "CollectionEventProperty" &&
                    Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
                    Specimen.DefaultUseEventPropertiyResponsible)
                {
                    Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
                    Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
                }

                if (Table == "IdentificationUnitAnalysis" &&
                    Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
                    Specimen.DefaultUseAnalyisisResponsible)
                {
                    Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
                    Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
                }

                if (Table == "CollectionSpecimenProcessing" &&
                    Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
                    Specimen.DefaultUseProcessingResponsible)
                {
                    Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
                    Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
                }

                if (Table == "CollectionSpecimenRelation" &&
                    Rnew["IsInternalRelationCache"].Equals(System.DBNull.Value))
                    Rnew["IsInternalRelationCache"] = 0;


                this.dataSetCollectionSpecimen.Tables[Table].Rows.Add(Rnew);
                //if (Table == "CollectionSpecimenRelation")
                //{
                //    //Rnew.BeginEdit();
                //    //Rnew.EndEdit();
                //    //this.dataSetCollectionSpecimen.Tables[Table].AcceptChanges();
                //}
                if (PKcontainsIdentity)
                    Rnew.AcceptChanges();
                //if (Table == "CollectionSpecimenPart")
                //{
                //    Rnew.BeginEdit();
                //    Rnew.EndEdit();
                //    this.dataSetCollectionSpecimen.Tables[Table].AcceptChanges();
                //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenPart", this.sqlDataAdapterPart, this.BindingContext);
                //}
            }
            catch (System.Exception ex) 
            { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

#region Remote services

        private void GetCoordinatesFromGoogleMaps()
        {
            string Latitude = "";
            string Longitude = "";
            try
            {
                if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine]["Latitude"].Equals(System.DBNull.Value))
                {
                    Latitude = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine]["Latitude"].ToString();
                    Longitude = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine]["Longitude"].ToString();
                }
                if (Latitude.Length == 0)
                {
                    System.Windows.Forms.DataGridViewRow Row = this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex];
                    foreach (System.Windows.Forms.DataGridViewCell C in Row.Cells)
                    {
                        if (this.dataGridView.Columns[C.ColumnIndex].DataPropertyName == "Latitude"
                            && C.Value != null)
                            Latitude = C.Value.ToString();
                        if (this.dataGridView.Columns[C.ColumnIndex].DataPropertyName == "Longitude"
                            && C.Value != null)
                            Longitude = C.Value.ToString();
                        if (Longitude.Length > 0 && Latitude.Length > 0) break;
                    }
                }
                bool OK = false;
                DiversityWorkbench.Forms.FormGoogleMapsCoordinates f;

                System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");

                double la = 0.0;
                double lo = 0.0;
                string Lat = Latitude.ToString(InvC).Replace(",", ".");
                string Long = Longitude.ToString(InvC).Replace(",", ".");
                if (Lat.Length > 0 && Long.Length > 0)
                {
                    OK = true;
                    if (!double.TryParse(Lat, NumberStyles.Float, InvC, out la)) OK = false;
                    if (!double.TryParse(Long, NumberStyles.Float, InvC, out lo)) OK = false;
                }
                if (!OK)
                {
                    // try to find existing coordinates
                    System.Data.DataRow[] rrC = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("NOT AverageLatitudeCache IS NULL AND NOT AverageLongitudeCache IS NULL");
                    if (rrC.Length > 0)
                    {
                        OK = true;
                        if (!double.TryParse(rrC[0]["AverageLatitudeCache"].ToString(), NumberStyles.Float, InvC, out la)) OK = false;
                        if (!double.TryParse(rrC[0]["AverageLongitudeCache"].ToString(), NumberStyles.Float, InvC, out lo)) OK = false;
                    }
                }

                if (!OK)
                {
                    try
                    {
                        System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("AverageLatitudeCache  <> 0 AND AverageLongitudeCache <> 0", "AverageLatitudeCache DESC");
                        if (rr.Length > 0)
                        {
                            OK = true;
                            if (!double.TryParse(rr[0]["AverageLatitudeCache"].ToString(), NumberStyles.Float, InvC, out la)) OK = false;
                            if (!double.TryParse(rr[0]["AverageLongitudeCache"].ToString(), NumberStyles.Float, InvC, out lo)) OK = false;
                        }
                    }
                    catch { OK = false; }
                }
                if (OK) f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(la, lo);
                else f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(0.0, 0.0);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    try
                    {
                        if (f.Longitude < 180 && f.Longitude > -180 && f.Latitude < 180 && f.Latitude > -180 && f.LatitudeAccuracy != 0.0 && f.LongitudeAccuracy != 0.0)
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                            R["Longitude"] = f.Longitude.ToString("F09", InvC);
                            R["Latitude"] = f.Latitude.ToString("F09", InvC);
                            R["Coordinates_accuracy"] = f.Accuracy.ToString("F00", InvC) + " m";
                            R["_CoordinatesLocationAccuracy"] = f.Accuracy.ToString("F00", InvC) + " m";
                            R["_CoordinatesAverageLongitudeCache"] = f.Longitude;
                            R["_CoordinatesAverageLatitudeCache"] = f.Latitude;
                            string Notes = "";
                            if (!R["_CoordinatesLocationNotes"].Equals(System.DBNull.Value)) Notes = R["_CoordinatesLocationNotes"].ToString();
                            if (Notes.Length > 0)
                            {
                                if (Notes.IndexOf("Derived from Google Maps") == -1)
                                    Notes += ". Derived from Google Maps";
                            }
                            else Notes = "Derived from Google Maps";
                            R["_CoordinatesLocationNotes"] = Notes;
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

            }
            catch { }
        }

        private void GetRemoteValues(System.Windows.Forms.DataGridViewCell Cell)
        {
            DiversityWorkbench.Forms.FormRemoteQuery f;
            try
            {
                string ValueColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
                string DisplayColumn = ValueColumn;
                bool IsListInDatabase = false;
                string ListInDatabase = "";
                DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit;
                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RemoteValueBindings = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
                switch (ValueColumn)
                {
                    case "NamedAreaLocation2":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityGazetteer")) return;
                        ValueColumn = "NamedAreaLocation2";
                        DisplayColumn = "Named_area";
                        DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbCountry = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbCountry.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbCountry.Column = "Country";
                        RvbCountry.RemoteParameter = "Country";
                        RemoteValueBindings.Add(RvbCountry);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLatitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbLatitude.Column = "_NamedAverageLatitudeCache";
                        RvbLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLongitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbLongitude.Column = "_NamedAverageLongitudeCache";
                        RvbLongitude.RemoteParameter = "Longitude";
                        RemoteValueBindings.Add(RvbLongitude);
                        break;

                    case "Link_to_SamplingPlots":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversitySamplingPlots")) return;
                        ValueColumn = "Link_to_SamplingPlots";
                        DisplayColumn = "Sampling_plot";
                        DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotLatitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSamplingPlotLatitude.Column = "Latitude_of_sampling_plot";
                        RvbSamplingPlotLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotLongitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSamplingPlotLongitude.Column = "Longitude_of_sampling_plot";
                        RvbSamplingPlotLongitude.RemoteParameter = "Longitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLongitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotAccuracy = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotAccuracy.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSamplingPlotAccuracy.Column = "Accuracy_of_sampling_plot";
                        RvbSamplingPlotAccuracy.RemoteParameter = "Accuracy";
                        RemoteValueBindings.Add(RvbSamplingPlotAccuracy);
                        break;

                    case "Geographic_region":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_GeographicRegionPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Geographic regions";
                        DiversityWorkbench.ScientificTerm S_Geo = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Geo;
                        break;
                    case "Lithostratigraphy":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_LithostratigraphyPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Lithostratigraphy";
                        DiversityWorkbench.ScientificTerm S_Litho = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Litho;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyLitho = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbHierarchyLitho.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbHierarchyLitho.Column = "_LithostratigraphyPropertyHierarchyCache";
                        RvbHierarchyLitho.RemoteParameter = "HierarchyCache";
                        RemoteValueBindings.Add(RvbHierarchyLitho);
                        break;

                    case "Chronostratigraphy":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_ChronostratigraphyPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Chronostratigraphy";
                        DiversityWorkbench.ScientificTerm S_Chrono = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Chrono;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyChrono = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbHierarchyChrono.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbHierarchyChrono.Column = "_ChronostratigraphyPropertyHierarchyCache";
                        RvbHierarchyChrono.RemoteParameter = "HierarchyCache";
                        RemoteValueBindings.Add(RvbHierarchyChrono);
                        break;

                    case "Biostratigraphy":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_BiostratigraphyPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Biostratigraphy";
                        DiversityWorkbench.ScientificTerm S_Bio = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Bio;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyBio = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbHierarchyBio.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbHierarchyBio.Column = "_BiostratigraphyPropertyHierarchyCache";
                        RvbHierarchyBio.RemoteParameter = "HierarchyCache";
                        RemoteValueBindings.Add(RvbHierarchyBio);
                        break;

                    case "Link_to_DiversityTaxonNames":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames")) return;
                        ValueColumn = "Link_to_DiversityTaxonNames";
                        DisplayColumn = "Taxonomic_name";
                        DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbFamily.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbFamily.Column = "Family_of_taxon";
                        RvbFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbOrder.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbOrder.Column = "Order_of_taxon";
                        RvbOrder.RemoteParameter = "Order";
                        RemoteValueBindings.Add(RvbOrder);
                        break;

                    case "Link_to_DiversityTaxonNames_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames")) return;
                        ValueColumn = "Link_to_DiversityTaxonNames_of_second_organism";
                        DisplayColumn = "Taxonomic_name_of_second_organism";
                        DiversityWorkbench.TaxonName Tsecond = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Tsecond;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSecondFamily.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSecondFamily.Column = "_SecondUnitFamilyCache";
                        RvbSecondFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbSecondFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSecondOrder.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSecondOrder.Column = "Order_of_taxon";
                        RvbSecondOrder.RemoteParameter = "_SecondUnitOrderCache";
                        RemoteValueBindings.Add(RvbSecondOrder);
                        break;

                    case "Link_to_DiversityAgents":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents";
                        DisplayColumn = "Collectors_name";
                        DiversityWorkbench.Agent Agent = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Agent;
                        break;

                    case "Depositors_link_to_DiversityAgents":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Depositors_link_to_DiversityAgents";
                        DisplayColumn = "Depositors_name";
                        DiversityWorkbench.Agent Depositor = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Depositor;
                        break;

                    case "Link_to_DiversityAgents_for_responsible":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_responsible";
                        DisplayColumn = "ResponsibleName";
                        DiversityWorkbench.Agent Identifier = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier;
                        break;

                    case "Link_to_DiversityAgents_for_determiner":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_determiner";
                        DisplayColumn = "Determiner";
                        DiversityWorkbench.Agent Determiner = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Determiner;
                        break;

                    case "Link_to_DiversityAgents_for_determiner_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_determiner_of_second_organism";
                        DisplayColumn = "Determiner_of_second_organism";
                        DiversityWorkbench.Agent Identifier_2 = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier_2;
                        break;

                    case "Link_to_DiversityAgents_for_responsible_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_responsible_of_second_organism";
                        DisplayColumn = "Responsible_of_second_organism";
                        DiversityWorkbench.Agent Identifier2 = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier2;
                        break;

                    case "Link_to_DiversityExsiccatae":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityExsiccatae")) return;
                        ValueColumn = "Link_to_DiversityExsiccatae";
                        DisplayColumn = "Exsiccata_abbreviation";
                        DiversityWorkbench.Exsiccate Exs = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Exs;
                        break;

                    case "Link_to_DiversityReferences":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
                        ValueColumn = "Link_to_DiversityReferences";
                        DisplayColumn = "Reference_title";
                        DiversityWorkbench.Reference Ref = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Ref;
                        break;

                    case "Link_to_DiversityReferences_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
                        ValueColumn = "Link_to_DiversityReferences_of_second_organism";
                        DisplayColumn = "Reference_title_of_second_organism";
                        DiversityWorkbench.Reference Ref2 = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Ref2;
                        break;

                    case "Link_to_DiversityReferences_for_specimen":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
                        ValueColumn = "Link_to_DiversityReferences_for_specimen";
                        DisplayColumn = "Reference_title_for_specimen";
                        DiversityWorkbench.Reference RefSpecimen = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)RefSpecimen;
                        break;

                    case "Link_to_DiversityCollection_for_relation":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityCollection")) return;
                        ValueColumn = "Link_to_DiversityCollection_for_relation";
                        DisplayColumn = "Related_specimen_display_text";
                        DiversityWorkbench.CollectionSpecimen RelSpecimen = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)RelSpecimen;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSpecimenUri = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSpecimenUri.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSpecimenUri.Column = "Related_specimen_URL";
                        RvbSpecimenUri.RemoteParameter = "_URI";
                        RemoteValueBindings.Add(RvbSpecimenUri);


                        break;

                    default:
                        DiversityWorkbench.TaxonName Default = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Default;
                        break;
                }

                if (this.firstLinesCollectionSpecimenBindingSource != null && IWorkbenchUnit != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                    string URI = "";
                    if (RU != null)
                        if (!RU[ValueColumn].Equals(System.DBNull.Value)) URI = RU[ValueColumn].ToString();
                    if (URI.Length == 0)
                    {
                        if (IsListInDatabase)
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit, ListInDatabase);
                        }
                        else
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit);
                        }
                    }
                    else
                    {
                        f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, IWorkbenchUnit);
                    }
                    try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
                    catch { }
                    f.TopMost = true;
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.DisplayText != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                        R.BeginEdit();
                        R[ValueColumn] = f.URI;
                        R[DisplayColumn] = f.DisplayText;
                        R.EndEdit();
                        //this.labelURI.Text = f.URI;
                        if (RemoteValueBindings != null && RemoteValueBindings.Count > 0)
                        {
                            foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in RemoteValueBindings)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> P in IWorkbenchUnit.UnitValues())
                                {
                                    if (RVB.RemoteParameter == P.Key)
                                    {
                                        System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;
                                        RV.BeginEdit();
                                        RV[RVB.Column] = P.Value;
                                        RV.EndEdit();
                                    }
                                }
                            }
                            if (ValueColumn == "Link_to_DiversityCollection_for_relation")
                            {
                                R["Relation_is_internal"] = 1;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Collections.Generic.Dictionary<string, string> LinkColumns
        {
            get
            {
                if (this._LinkColumns == null)
                {
                    this._LinkColumns = new Dictionary<string, string>();
                    foreach(System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    {
                        if (C.DataPropertyName.StartsWith("Link_to_"))
                        {
                            this._LinkColumns.Add(C.DataPropertyName, "");
                        }
                    }
                }
                return this._LinkColumns;
            }
        }

        private void setLinkCellStyle(System.Windows.Forms.DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                this.setLinkCellStyle(i, dataGrid);
        }

        private void setLinkCellStyle(int RowIndex, System.Windows.Forms.DataGridView dataGrid)
        {
            if (this._StyleLink == null)
            {
                this._StyleLink = new DataGridViewCellStyle();
                this._StyleLink.BackColor = System.Drawing.Color.Blue;
                this._StyleLink.SelectionBackColor = System.Drawing.Color.Blue;
                this._StyleLink.ForeColor = System.Drawing.Color.Blue;
                this._StyleLink.SelectionForeColor = System.Drawing.Color.Blue;
                //this._StyleRemove.Tag = "Link";
            }
            try
            {
                foreach (System.Windows.Forms.DataGridViewCell Cell in dataGrid.Rows[RowIndex].Cells)
                {
                    if (this.LinkColumns.ContainsKey(dataGrid.Columns[Cell.ColumnIndex].DataPropertyName))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell LinkCell in dataGrid.Rows[RowIndex].Cells)
                        {
                            if (dataGrid.Columns[LinkCell.ColumnIndex].DataPropertyName ==
                                dataGrid.Columns[Cell.ColumnIndex].DataPropertyName)
                            {
                                LinkCell.Style = this._StyleLink;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Removing links and Columns for removing links to external services

        private void RemoveLink(System.Windows.Forms.DataGridViewCell Cell)
        {
            string DisplayColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
            string ValueColumn = "";
            string Table = "";
            string LinkColumn = "";
            try
            {
                foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                {
                    if (Q.AliasForColumn == DisplayColumn)
                    {
                        ValueColumn = Q.Column;
                        Table = Q.AliasForTable;
                        break;
                    }
                }

                foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                {
                    if (Q.AliasForTable == Table && Q.Column == ValueColumn)
                    {
                        LinkColumn = Q.AliasForColumn;
                        break;
                    }
                }

                if (this.firstLinesCollectionSpecimenBindingSource != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                    RU[LinkColumn] = System.DBNull.Value;
                }

            }
            catch { }
        }

        /// <summary>
        /// Dictionary of columns that remove links of related modules etc.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> RemoveColumns
        {
            get
            {
                if (this._RemoveColumns == null)
                {
                    this._RemoveColumns = new Dictionary<string, string>();
                    this._RemoveColumns.Add("Remove_link_to_SamplingPlots", "");
                    this._RemoveColumns.Add("Remove_link_to_gazetteer", "");
                    this._RemoveColumns.Add("Remove_link_for_collector", "");
                    this._RemoveColumns.Add("Remove_link_for_Depositor", "");
                    this._RemoveColumns.Add("Remove_link_to_exsiccatae", "");
                    this._RemoveColumns.Add("Remove_link_of_reference_for_specimen", "");
                    this._RemoveColumns.Add("Remove_link_for_identification", "");
                    this._RemoveColumns.Add("Remove_link_for_reference", "");
                    this._RemoveColumns.Add("Remove_link_for_determiner", "");
                    this._RemoveColumns.Add("Remove_link_for_second_organism", "");
                    this._RemoveColumns.Add("Remove_link_for_reference_of_second_organism", "");
                    this._RemoveColumns.Add("Remove_link_for_determiner_of_second_organism", "");
                }
                return this._RemoveColumns;
            }
        }

        //private void setRemoveCellStyle()
        //{
        //    for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
        //        this.setRemoveCellStyle(i);
        //}

        private void setRemoveCellStyle(System.Windows.Forms.DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                this.setRemoveCellStyle(i, dataGrid);
        }

        private void setRemoveCellStyle(int RowIndex, System.Windows.Forms.DataGridView dataGrid)
        {
            if (this._StyleRemove == null)
            {
                this._StyleRemove = new DataGridViewCellStyle();
                this._StyleRemove.BackColor = System.Drawing.Color.Red;
                this._StyleRemove.SelectionBackColor = System.Drawing.Color.Red;
                this._StyleRemove.ForeColor = System.Drawing.Color.Red;
                this._StyleRemove.SelectionForeColor = System.Drawing.Color.Red;
                this._StyleRemove.Tag = "Remove";
            }
            try
            {
                foreach (System.Windows.Forms.DataGridViewCell Cell in dataGrid.Rows[RowIndex].Cells)
                {
                    if (this.RemoveColumns.ContainsKey(dataGrid.Columns[Cell.ColumnIndex].DataPropertyName))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell RemoveCell in dataGrid.Rows[RowIndex].Cells)
                        {
                            if (dataGrid.Columns[RemoveCell.ColumnIndex].DataPropertyName ==
                                dataGrid.Columns[Cell.ColumnIndex].DataPropertyName)
                            {
                                RemoveCell.Style = this._StyleRemove;
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        //private void setRemoveCellStyle(int RowIndex)
        //{
        //    if (this._StyleRemove == null)
        //    {
        //        this._StyleRemove = new DataGridViewCellStyle();
        //        this._StyleRemove.BackColor = System.Drawing.Color.Red;
        //        this._StyleRemove.SelectionBackColor = System.Drawing.Color.Red;
        //        this._StyleRemove.ForeColor = System.Drawing.Color.Red;
        //        this._StyleRemove.SelectionForeColor = System.Drawing.Color.Red;
        //        this._StyleRemove.Tag = "Remove";
        //    }
        //    try
        //    {
        //        foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
        //        {
        //            if (this.RemoveColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
        //            {
        //                foreach (System.Windows.Forms.DataGridViewCell RemoveCell in this.dataGridView.Rows[RowIndex].Cells)
        //                {
        //                    if (this.dataGridView.Columns[RemoveCell.ColumnIndex].DataPropertyName ==
        //                        this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
        //                    {
        //                        RemoveCell.Style = this._StyleRemove;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

        #endregion


        #endregion

        #region Blocking of Cells that are linked to external services

        private void setCellBlockings()
        {
            for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
                this.setCellBlockings(this.dataGridView, i);
        }

        private void setCellBlockings(System.Windows.Forms.DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                this.setCellBlockings(dataGrid, i);
        }

        private void setCellBlockings(System.Windows.Forms.DataGridView dataGridView, int RowIndex)
        {
            if (this._StyleBlocked == null)
            {
                this._StyleBlocked = new DataGridViewCellStyle();
                this._StyleBlocked.BackColor = System.Drawing.Color.Yellow;
                this._StyleBlocked.SelectionBackColor = System.Drawing.Color.Yellow;
                this._StyleBlocked.ForeColor = System.Drawing.Color.Blue;
                this._StyleBlocked.SelectionForeColor = System.Drawing.Color.Blue;
                this._StyleBlocked.Tag = "Blocked";
            }
            if (this._StyleUnblocked == null)
            {
                this._StyleUnblocked = new DataGridViewCellStyle();
                this._StyleUnblocked.BackColor = System.Drawing.SystemColors.Window;
                this._StyleUnblocked.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                this._StyleUnblocked.ForeColor = System.Drawing.Color.Black;
                this._StyleUnblocked.SelectionForeColor = System.Drawing.SystemColors.Window;
                this._StyleUnblocked.Tag = "";
            }
            try
            {
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this.BlockedColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName ==
                                this.BlockedColumns[this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName])
                            {
                                if (Cell.EditedFormattedValue.ToString().Length > 0)
                                {
                                    CellToBlock.Style = this._StyleBlocked;
                                    CellToBlock.ReadOnly = true;
                                    if (this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName == "Related_specimen_display_text")
                                    {
                                        this.dataGridView.Rows[RowIndex].Cells[CellToBlock.ColumnIndex - 1].Style = this._StyleBlocked;
                                        this.dataGridView.Rows[RowIndex].Cells[CellToBlock.ColumnIndex - 1].ReadOnly = true;
                                    }
                                }
                                else
                                {
                                    CellToBlock.Style = this._StyleUnblocked;
                                    CellToBlock.ReadOnly = false;
                                    if (this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName == "Related_specimen_display_text")
                                    {
                                        this.dataGridView.Rows[RowIndex].Cells[CellToBlock.ColumnIndex - 1].Style = this._StyleUnblocked;
                                        this.dataGridView.Rows[RowIndex].Cells[CellToBlock.ColumnIndex - 1].ReadOnly = false;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private System.Collections.Generic.Dictionary<string, string> BlockedColumns
        {
            get
            {
                if (this._BlockedColumns == null)
                {
                    this._BlockedColumns = new Dictionary<string, string>();
                    this._BlockedColumns.Add("Link_to_DiversityAgents", "Collectors_name");
                    this._BlockedColumns.Add("Depositors_link_to_DiversityAgents", "Depositors_name");
                    this._BlockedColumns.Add("Link_to_DiversityExsiccatae", "Exsiccata_abbreviation");
                    this._BlockedColumns.Add("Link_to_DiversityReferences_for_specimen", "Reference_title_for_specimen");
                    this._BlockedColumns.Add("Link_to_DiversityTaxonNames", "Taxonomic_name");
                    this._BlockedColumns.Add("Link_to_DiversityReferences", "Reference_title");
                    this._BlockedColumns.Add("Link_to_DiversityAgents_for_responsible", "Responsible");

                    this._BlockedColumns.Add("Link_to_DiversityTaxonNames_of_second_organism", "Taxonomic_name_of_second_organism");
                    this._BlockedColumns.Add("Link_to_DiversityReferences_of_second_organism", "Reference_title_of_second_organism");
                    this._BlockedColumns.Add("Link_to_DiversityAgents_for_responsible_of_second_organism", "Responsible_of_second_organism");
                    this._BlockedColumns.Add("Link_to_DiversityAgents_for_determiner", "Determiner");

                    this._BlockedColumns.Add("NamedAreaLocation2", "Named_area");
                    this._BlockedColumns.Add("Link_to_SamplingPlots", "Sampling_plot");

                    this._BlockedColumns.Add("Link_to_DiversityCollection_for_relation", "Related_specimen_display_text");
                    //this._BlockedColumns.Add("Link_to_DiversityCollection_for_relation", "Related_specimen_URL");
                }
                return this._BlockedColumns;
            }
        }
        
#endregion

#region Copy

        /// <summary>
        /// create a copy of a collection specimen
        /// </summary>
        /// <param name="OriginalID">The CollectionSpecimenID of the original dataset</param>
        /// <param name="AccessionNumber">The new AccessionNumber</param>
        /// <param name="EventCopyMode">The mode for the copy of the collection event</param>
        /// <param name="CopyUnits">If the identification units should be copied</param>
        /// <returns></returns>
        private int CopySpecimen(int OriginalID, string AccessionNumber, DiversityCollection.Forms.FormCopyDataset.EventCopyMode EventCopyMode, bool CopyUnits)
        {
            string SQL = "execute dbo.procCopyCollectionSpecimen2 NULL , " + OriginalID.ToString() + ", '" + AccessionNumber + "'";
            switch (EventCopyMode)
            {
                case FormCopyDataset.EventCopyMode.NewEvent:
                    SQL += ", 1";
                    break;
                case FormCopyDataset.EventCopyMode.SameEvent:
                    SQL += ", 0";
                    break;
                case FormCopyDataset.EventCopyMode.NoEvent:
                    SQL += ", -1";
                    break;
            }
            if (CopyUnits) SQL += ", 1";
            else SQL += ", 0";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            int ID = 0;
            try
            {
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (ID == 2)
                {
                    cmd.CommandText = "SELECT MAX(CollectionSpecimenID) FROM CollectionSpecimen WHERE LogUpdatedBy = SYSTEM_USER OR LogUpdatedBy = cast([dbo].[UserID]() as varchar)";
                    ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            con.Close();
            return ID;
        }

        private int? NewIdentificationUnitID()
        {
            int? UnitID = null;
            try
            {
                if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count > 0)
                {
                    System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder ASC");
                    if (RR.Length > 0)
                    {
                        int ID = 0;
                        if (int.TryParse(RR[0]["IdentificationUnitID"].ToString(), out ID))
                            UnitID = ID;
                    }
                }

            }
            catch { }
            return UnitID;
        }

        private int? NewEventID()
        {
            int? EventID = null;
            try
            {
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    int ID = 0;
                    if (int.TryParse(this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionEventID"].ToString(), out ID))
                        EventID = ID;
                }

            }
            catch { }
            return EventID;
        }

#endregion

#region Events for new Entries

        private void insertNewDataset(int Index)
        {
            if ((this.dataGridView.SelectedCells.Count > 0 &&
                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0 &&
                this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count)
                || this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count == 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to create a new dataset?", "New dataset?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        string AccessionNumber = "";
                        if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0)
                        {
                            System.Data.DataRow[] RR = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Select("Accession_number <> ''", "Accession_number DESC");
                            if (RR.Length > 0)
                                AccessionNumber = RR[0]["Accession_number"].ToString();
                            else if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count - 1]["Accession_number"].Equals(System.DBNull.Value) &&
                                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count - 1]["Accession_number"].ToString().Length > 0)
                                AccessionNumber = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count - 1]["Accession_number"].ToString();
                        }
                        DiversityCollection.Forms.FormAccessionNumber f = new Forms.FormAccessionNumber(AccessionNumber, true, true, false);
                        f.ShowDialog();
                        if (f.DialogResult == DialogResult.OK)
                            AccessionNumber = f.AccessionNumber;
                        if (AccessionNumber.Length == 0 &&
                            System.Windows.Forms.MessageBox.Show("You did not give an accession number.\r\nDo you want to insert a dataset without an accession number?", "Insert without accession number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return;
                        int ID = this.InsertNewSpecimen(AccessionNumber);
                        this._IDs.Add(ID);
                        System.Data.DataTable dt = new DataTable();
                        string SQL = "SELECT " + this._SqlSpecimenFields +
                            " FROM dbo." + this.SourceFunction + " ('" + ID.ToString() + "')";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        DiversityCollection.Datasets.DataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimenRow Rnew = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.NewFirstLinesCollectionSpecimenRow();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (System.Data.DataColumn C in Rnew.Table.Columns)
                            {
                                if (Rnew.Table.Columns.Contains(C.ColumnName) && dt.Columns.Contains(C.ColumnName))
                                    Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
                                else
                                {
                                    // some columns had been removed, e.g. "Reference_title", "Link_to_DiversityReferences"
                                }
                            }
                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Add(Rnew);
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }


        /// <summary>
        /// inserting a new specimen into the table CollectionSpecimen
        /// </summary>
        /// <param name="Index">Index of the row in the grid view</param>
        /// <returns>the CollectionSpecimenID of the new specimen</returns>
        private int InsertNewSpecimen(int Index)
        {
            try
            {
                int ID = -1;
                int ProjectID = -1;
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Select a project", "Please select a project from the list, where the new dataset should belong to");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (!int.TryParse(f.SelectedValue.ToString(), out ProjectID))
                        return ID;
                    SQL = this.GridModeInsertCommandForNewData(Index, "CollectionSpecimen");
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    SQL = "INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID) VALUES (" + ID.ToString() + ", " + ProjectID.ToString() + ")";
                    cmd.CommandText = SQL;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return ID;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
        }

        private int InsertNewSpecimen(string AccessionNumber)
        {
            try
            {
                int ID = -1;
                int ProjectID = -1;
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Select a project", "Please select a project from the list, where the new dataset should belong to", this._ProjectID.ToString());
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (!int.TryParse(f.SelectedValue.ToString(), out ProjectID))
                        return ID;
                    SQL = "INSERT INTO CollectionSpecimen (Version, AccessionNumber) VALUES (1, '" + AccessionNumber + "');  (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    SQL = "INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID) VALUES (" + ID.ToString() + ", " + ProjectID.ToString() + ")";
                    cmd.CommandText = SQL;
                    cmd.ExecuteNonQuery();
                    if (this.ProjectSettings.Count > 0)
                    {
                        if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
                            this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
                        {
                            SQL = "INSERT INTO IdentificationUnit " +
                                "(CollectionSpecimenID, LastIdentificationCache, TaxonomicGroup, DisplayOrder) " +
                                "VALUES (" + ID.ToString() + ", '" + this.ProjectSettings["IdentificationUnit.TaxonomicGroup"]
                                + "', '" + this.ProjectSettings["IdentificationUnit.TaxonomicGroup"] + "', 1)";
                            cmd.CommandText = SQL;
                            cmd.ExecuteNonQuery();
                        }
                        if ((this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                            && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                            || (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                            && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0))
                        {
                            SQL = "INSERT INTO CollectionSpecimenPart " +
                                "(CollectionSpecimenID, SpecimenPartID, CollectionID, MaterialCategory) " +
                                "VALUES (" + ID.ToString() + ", 1, ";
                            if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                            && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                                SQL += this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
                            else
                                SQL += " NULL ";
                            SQL += ", ";
                            if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                            && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                                SQL += " '" + this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"] + "'";
                            else
                                SQL += " NULL ";
                            SQL += ")";
                            cmd.CommandText = SQL;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                }
                return ID;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
        }

        private int createEvent(int Index)
        {
            int EventID = -1;
            try
            {
                string SQL = this.GridModeInsertCommandForNewData(Index, "CollectionEvent");//"INSERT INTO CollectionEvent (LocalityDescription)VALUES ('" + LocalityDescription + "') (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                EventID = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return EventID;
        }

        private int createIdentificationUnit(int Index, string AliasForTable)
        {
            int UnitID = -1;
            try
            {
                string SQL = this.GridModeInsertCommandForNewData(Index, AliasForTable);
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                UnitID = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return UnitID;
        }

        private int createCollectionSpecimenPart(int Index, string AliasForTable)
        {
            int PartID = -1;
            try
            {
                string SQL = this.GridModeInsertCommandForNewData(Index, AliasForTable);
                using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    con.Open();
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    PartID = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return PartID;
        }

        /// <summary>
        /// Returns the SQL command for the new data
        /// </summary>
        /// <param name="Index">Index of the row in the grid view</param>
        /// <param name="AliasForTable">The alias for the table</param>
        /// <returns>SQL</returns>
        private string GridModeInsertCommandForNewData(int Index, string AliasForTable)
        {
            string SQL = "";
            string SqlColumns = "INSERT INTO " + this.GridModeTableName(AliasForTable) + " (";
            string SqlValues = "VALUES ( ";
            if (this.GridModeTableName(AliasForTable) == "CollectionSpecimen" ||
                this.GridModeTableName(AliasForTable) == "CollectionEvent")
            {
                SqlColumns += " Version, ";
                SqlValues += " 1, ";
            }
            else if (this.GridModeTableName(AliasForTable) == "IdentificationUnit")
            {
                string TaxonomicGroup = "";
                int DisplayOrder = 1;
                string LastIdentification = "";
                SqlColumns += " CollectionSpecimenID, ";
                SqlValues += this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["CollectionSpecimenID"].ToString() + ", ";
                switch (AliasForTable)
                {
                    case "IdentificationUnit":
                        TaxonomicGroup = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_group"].ToString();
                        if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
                            LastIdentification = TaxonomicGroup;
                        else
                            LastIdentification = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name"].ToString();
                        DisplayOrder = 1;
                        break;
                    case "SecondUnit":
                        TaxonomicGroup = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
                        if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
                            this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
                            LastIdentification = TaxonomicGroup;
                        else
                            LastIdentification = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
                        DisplayOrder = 2;
                        break;
                }
                SqlColumns += " LastIdentificationCache, ";
                SqlValues += " '" + LastIdentification + "', ";
                SqlColumns += " DisplayOrder, ";
                SqlValues += " " + DisplayOrder.ToString() + ", ";
            }
            else if (this.GridModeTableName(AliasForTable) == "CollectionSpecimenPart")
            {
                SqlColumns += " CollectionSpecimenID, ";
                SqlValues += this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index]["CollectionSpecimenID"].ToString() + ", ";
            }
            if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > Index)
            {
                System.Data.DataRow R = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index];
                foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Columns)
                {
                    DiversityCollection.Forms.GridModeQueryField GMQF = this.GridModeGetQueryField(C.ColumnName);
                    if (GMQF.AliasForTable == AliasForTable &&
                        !this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index][GMQF.AliasForColumn].Equals(System.DBNull.Value))
                    {
                        SqlColumns += GMQF.Column + ", ";
                        SqlValues += "'" + this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[Index][GMQF.AliasForColumn].ToString() + "', ";
                    }
                }
            }
            if (SqlValues.EndsWith(", ")) SqlValues = SqlValues.Substring(0, SqlValues.Length - 2);
            if (SqlColumns.EndsWith(", ")) SqlColumns = SqlColumns.Substring(0, SqlColumns.Length - 2);
            SqlValues += ") ";
            SqlColumns += ") ";
            SQL = SqlColumns + SqlValues + " (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
            return SQL;
        }

#endregion
      
        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //try
            //{
            //    if (this.dataGridView.Rows.Count == 0) return;
            //    foreach (System.Data.DataRow R in this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows)
            //    {
            //        if (R.RowState == DataRowState.Deleted)
            //        {
            //        }
            //    }

            //}
            //catch { }
        }

#endregion

#region Handling the visibility of the columns in the grid

        /// <summary>
        /// setting the visibility of the columns in the datagrid based on the definitions in the query fields
        /// </summary>
        private void GridModeSetColumnVisibility(System.Windows.Forms.DataGridView dataGridView)
        {
            foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
            {
                if (this.GridModeColumnList.Contains(C.DataPropertyName))
                    C.Visible = true;
                else C.Visible = false;
            }
        }

        /// <summary>
        /// the list of the table aliases and their table names
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> GridModeTableList
        {
            get
            {
                if (this._GridModeTableList == null) this._GridModeTableList = new Dictionary<string, string>();
                int i = 1;
                try
                {
                    foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                    {
                        if (Q.IsVisible && !this._GridModeTableList.ContainsKey(Q.AliasForTable))
                        {
                            if (Q.AliasForTable.Length == 0)
                                this._GridModeTableList.Add(Q.AliasForTable, Q.Table);
                            else this._GridModeTableList.Add(Q.AliasForTable, Q.AliasForTable);
                        }
                        i++;
                    }

                }
                catch { }
                return this._GridModeTableList;
            }
        }

        /// <summary>
        /// The alias for a table. If the table is listed more then once, the first alias will be returned
        /// </summary>
        /// <param name="TableName">The name of the table</param>
        /// <returns>The alias</returns>
        private string GridModeTableAlias(string TableName)
        {
            string Alias = "";
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.GridModeTableList)
                {
                    if (KV.Value == TableName)
                    {
                        Alias = KV.Key;
                        break;
                    }
                }

            }
            catch { }
            return Alias;
        }

        /// <summary>
        /// The name of the table
        /// </summary>
        /// <param name="Alias">The alias of the table</param>
        /// <returns>The table</returns>
        private string GridModeTableName(string Alias)
        {
            string Table = "";
            try
            {
                foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                {
                    if (Q.AliasForTable == Alias)
                    {
                        Table = Q.Table;
                        break;
                    }
                }

            }
            catch { }
            return Table;
        }

        /// <summary>
        /// a list of all column names
        /// </summary>
        private System.Collections.Generic.List<string> GridModeColumnList
        {
            get
            {
                if (this._GridModeColumnList == null) this._GridModeColumnList = new List<string>();
                try
                {
                    foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                    {
                        if (Q.IsVisible && !this._GridModeColumnList.Contains(Q.AliasForColumn))
                            this._GridModeColumnList.Add(Q.AliasForColumn);
                    }

                }
                catch  {}
                return this._GridModeColumnList;
            }
        }

        /// <summary>
        /// the grid mode field as specified in the treeview
        /// </summary>
        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> GridModeQueryFields
        {
            get
            {
                if (this._GridModeQueryFields == null)
                {
                    this._GridModeQueryFields = new List<GridModeQueryField>();
                    foreach (System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
                    {
                        if (N.Tag != null && N.Tag.ToString().Contains(";"))
                        {
                            try
                            {
                                string[] Parameter = this.GridModeFieldTagArray(N.Tag.ToString());
                                if (Parameter.Length > 1)
                                {
                                    DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
                                    if (Parameter.Length < 4) continue;

                                    Q.Table = Parameter[0];

                                    Q.AliasForTable = Parameter[1];
                                    if (Q.AliasForTable.Length == 0) Q.AliasForTable = Q.Table;

                                    Q.Restriction = Parameter[2];

                                    Q.Column = Parameter[3];

                                    if (Parameter.Length > 4)
                                        Q.AliasForColumn = Parameter[4];
                                    else Q.AliasForColumn = "";
                                    if (Q.AliasForColumn.Length == 0)
                                        Q.AliasForColumn = Q.Column;

                                    if (Parameter.Length > 5)
                                        Q.DatasourceTable = Parameter[5];
                                    else Q.DatasourceTable = "";

                                    if (N.Checked)
                                        Q.IsVisible = true;
                                    else Q.IsVisible = false;
                                    Q.IsHidden = false;
                                    if (Parameter.Length > 7)
                                        Q.RemoveLinkColumn = Parameter[6];
                                    else Q.RemoveLinkColumn = "";
                                    this._GridModeQueryFields.Add(Q);
                                }

                            }
                            catch { }
                        }
                        else if (N.Tag != null)
                        {
                        }
                        this.GridModeQueryFieldsAddChildNodes(N);
                    }
                    foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeHiddenQueryFields)
                        this._GridModeQueryFields.Add(Q);
                }
                string Visibility = "";
                try
                {
                    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility.Length == 0)
                    {
                        foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                        {
                            if (!Q.IsHidden)
                            {
                                if (Q.IsVisible) Visibility += "1";
                                else Visibility += "0";
                            }
                        }
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility = Visibility;
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                    }
                    else
                    {
                        Visibility = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility;
                        for (int i = 0; i < this._GridModeQueryFields.Count; i++)
                        {
                            bool IsVisible = false;
                            if (Visibility.Substring(0, 1) == "1") IsVisible = true;
                            this.GridModeSetIsVisibleForQueryField(i, IsVisible);
                            if (Visibility.Length > 1)
                                Visibility = Visibility.Substring(1);
                            else break;
                        }
                    }

                }
                catch  { }
                return this._GridModeQueryFields;
            }
        }

        /// <summary>
        /// All fields that exist in the database view FirstLinesCollectionSpecimen
        /// but not in the selection tree
        /// </summary>
        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> GridModeHiddenQueryFields
        {
            get
            {
                if (this._GridModeHiddenQueryFields == null)
                {
                    this._GridModeHiddenQueryFields = new List<GridModeQueryField>();

                    try
                    {
                        DiversityCollection.Forms.GridModeQueryField Q_EventID = new Forms.GridModeQueryField();
                        Q_EventID.Table = "CollectionEvent";
                        Q_EventID.Column = "CollectionEventID";
                        Q_EventID.AliasForColumn = "_CollectionEventID";
                        Q_EventID.AliasForTable = "CollectionEvent";
                        Q_EventID.IsVisible = false;
                        Q_EventID.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_EventID);

                        DiversityCollection.Forms.GridModeQueryField Q_IdentificationUnitID = new Forms.GridModeQueryField();
                        Q_IdentificationUnitID.Table = "IdentificationUnit";
                        Q_IdentificationUnitID.Column = "IdentificationUnitID";
                        Q_IdentificationUnitID.AliasForColumn = "_IdentificationUnitID";
                        Q_IdentificationUnitID.AliasForTable = "IdentificationUnit";
                        Q_IdentificationUnitID.IsVisible = false;
                        Q_IdentificationUnitID.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_IdentificationUnitID);

                        DiversityCollection.Forms.GridModeQueryField Q_IdentificationSequence = new Forms.GridModeQueryField();
                        Q_IdentificationSequence.Table = "Identification";
                        Q_IdentificationSequence.Column = "IdentificationSequence";
                        Q_IdentificationSequence.AliasForColumn = "_IdentificationSequence";
                        Q_IdentificationSequence.AliasForTable = "Identification";
                        Q_IdentificationSequence.IsVisible = false;
                        Q_IdentificationSequence.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_IdentificationSequence);


                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber = new Forms.GridModeQueryField();
                        Q_AnalysisNumber.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber.Column = "AnalysisNumber";
                        Q_AnalysisNumber.AliasForColumn = "Analysis_number";
                        Q_AnalysisNumber.AliasForTable = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber.IsVisible = false;
                        Q_AnalysisNumber.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber);

                        //DiversityCollection.Forms.GridModeQueryField Q_AnalysisID = new Forms.GridModeQueryField();
                        //Q_AnalysisID.Table = "IdentificationUnitAnalysis";
                        //Q_AnalysisID.Column = "AnalysisID";
                        //Q_AnalysisID.AliasForColumn = "AnalysisID";
                        //Q_AnalysisID.AliasForTable = "IdentificationUnitAnalysis";
                        //Q_AnalysisID.IsVisible = false;
                        //Q_AnalysisID.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_AnalysisID);


                        DiversityCollection.Forms.GridModeQueryField Q_SecondUnitID = new Forms.GridModeQueryField();
                        Q_SecondUnitID.Table = "Identification";
                        Q_SecondUnitID.Column = "IdentificationUnitID";
                        Q_SecondUnitID.AliasForColumn = "_SecondUnitID";
                        Q_SecondUnitID.AliasForTable = "SecondUnit";
                        Q_SecondUnitID.IsVisible = false;
                        Q_SecondUnitID.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_SecondUnitID);

                        DiversityCollection.Forms.GridModeQueryField Q_SecondSequence = new Forms.GridModeQueryField();
                        Q_SecondSequence.Table = "Identification";
                        Q_SecondSequence.Column = "IdentificationSequence";
                        Q_SecondSequence.AliasForColumn = "_SecondSequence";
                        Q_SecondSequence.AliasForTable = "SecondUnitIdentification";
                        Q_SecondSequence.IsVisible = false;
                        Q_SecondSequence.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_SecondSequence);

                        DiversityCollection.Forms.GridModeQueryField Q_SpecimenPartID = new Forms.GridModeQueryField();
                        Q_SpecimenPartID.Table = "CollectionSpecimenPart";
                        Q_SpecimenPartID.Column = "SpecimenPartID";
                        Q_SpecimenPartID.AliasForColumn = "_SpecimenPartID";
                        Q_SpecimenPartID.AliasForTable = "CollectionSpecimenPart";
                        Q_SpecimenPartID.IsVisible = false;
                        Q_SpecimenPartID.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_SpecimenPartID);



                        //DiversityCollection.Forms.GridModeQueryField Q_CoordinatesLocationAccuracy = new Forms.GridModeQueryField();
                        //Q_CoordinatesLocationAccuracy.Table = "CollectionEventLocalisation";
                        //Q_CoordinatesLocationAccuracy.Column = "LocationAccuracy";
                        //Q_CoordinatesLocationAccuracy.AliasForColumn = "_CoordinatesLocationAccuracy";
                        //Q_CoordinatesLocationAccuracy.AliasForTable = "CoordinatesWGS84";
                        //Q_CoordinatesLocationAccuracy.Restriction = "LocalisationSystemID=8";
                        //Q_CoordinatesLocationAccuracy.IsVisible = false;
                        //Q_CoordinatesLocationAccuracy.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_CoordinatesLocationAccuracy);


                        //DiversityCollection.Forms.GridModeQueryField Q_CoordinatesLocationNotes = new Forms.GridModeQueryField();
                        //Q_CoordinatesLocationNotes.Table = "CollectionEventLocalisation";
                        //Q_CoordinatesLocationNotes.Column = "LocationNotes";
                        //Q_CoordinatesLocationNotes.AliasForColumn = "_CoordinatesLocationNotes";
                        //Q_CoordinatesLocationNotes.AliasForTable = "CoordinatesWGS84";
                        //Q_CoordinatesLocationNotes.Restriction = "LocalisationSystemID=8";
                        //Q_CoordinatesLocationNotes.IsVisible = false;
                        //Q_CoordinatesLocationNotes.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_CoordinatesLocationNotes);


                        DiversityCollection.Forms.GridModeQueryField Q_CoordinatesAverageLongitudeCache = new Forms.GridModeQueryField();
                        Q_CoordinatesAverageLongitudeCache.Table = "CollectionEventLocalisation";
                        Q_CoordinatesAverageLongitudeCache.Column = "AverageLongitudeCache";
                        Q_CoordinatesAverageLongitudeCache.AliasForColumn = "_CoordinatesAverageLongitudeCache";
                        Q_CoordinatesAverageLongitudeCache.AliasForTable = "CoordinatesWGS84";
                        Q_CoordinatesAverageLongitudeCache.Restriction = "LocalisationSystemID=8";
                        Q_CoordinatesAverageLongitudeCache.IsVisible = false;
                        Q_CoordinatesAverageLongitudeCache.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_CoordinatesAverageLongitudeCache);


                        DiversityCollection.Forms.GridModeQueryField Q_CoordinatesAverageLatitudeCache = new Forms.GridModeQueryField();
                        Q_CoordinatesAverageLatitudeCache.Table = "CollectionEventLocalisation";
                        Q_CoordinatesAverageLatitudeCache.Column = "AverageLatitudeCache";
                        Q_CoordinatesAverageLatitudeCache.AliasForColumn = "_CoordinatesAverageLatitudeCache";
                        Q_CoordinatesAverageLatitudeCache.AliasForTable = "CoordinatesWGS84";
                        Q_CoordinatesAverageLatitudeCache.Restriction = "LocalisationSystemID=8";
                        Q_CoordinatesAverageLatitudeCache.IsVisible = false;
                        Q_CoordinatesAverageLatitudeCache.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_CoordinatesAverageLatitudeCache);



                        //DiversityCollection.Forms.GridModeQueryField Q_NamedAreaLocation2 = new Forms.GridModeQueryField();
                        //Q_NamedAreaLocation2.Table = "CollectionEventLocalisation";
                        //Q_NamedAreaLocation2.Column = "Location2";
                        //Q_NamedAreaLocation2.AliasForColumn = "_NamedAreaLocation2";
                        //Q_NamedAreaLocation2.AliasForTable = "NamedArea";
                        //Q_NamedAreaLocation2.Restriction = "LocalisationSystemID=7";
                        //Q_NamedAreaLocation2.IsVisible = false;
                        //Q_NamedAreaLocation2.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_NamedAreaLocation2);

                        DiversityCollection.Forms.GridModeQueryField Q_NamedAverageLatitudeCache = new Forms.GridModeQueryField();
                        Q_NamedAverageLatitudeCache.Table = "CollectionEventLocalisation";
                        Q_NamedAverageLatitudeCache.Column = "AverageLatitudeCache";
                        Q_NamedAverageLatitudeCache.AliasForColumn = "_NamedAverageLatitudeCache";
                        Q_NamedAverageLatitudeCache.AliasForTable = "NamedArea";
                        Q_NamedAverageLatitudeCache.Restriction = "LocalisationSystemID=7";
                        Q_NamedAverageLatitudeCache.IsVisible = false;
                        Q_NamedAverageLatitudeCache.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_NamedAverageLatitudeCache);

                        DiversityCollection.Forms.GridModeQueryField Q_NamedAverageLongitudeCache = new Forms.GridModeQueryField();
                        Q_NamedAverageLongitudeCache.Table = "CollectionEventLocalisation";
                        Q_NamedAverageLongitudeCache.Column = "AverageLongitudeCache";
                        Q_NamedAverageLongitudeCache.AliasForColumn = "_NamedAverageLongitudeCache";
                        Q_NamedAverageLongitudeCache.AliasForTable = "NamedArea";
                        Q_NamedAverageLongitudeCache.Restriction = "LocalisationSystemID=7";
                        Q_NamedAverageLongitudeCache.IsVisible = false;
                        Q_NamedAverageLongitudeCache.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_NamedAverageLongitudeCache);


                        DiversityCollection.Forms.GridModeQueryField Q_NamedAverageAltitudeCache = new Forms.GridModeQueryField();
                        Q_NamedAverageAltitudeCache.Table = "CollectionEventLocalisation";
                        Q_NamedAverageAltitudeCache.Column = "AverageAltitudeCache";
                        Q_NamedAverageAltitudeCache.AliasForColumn = "_AverageAltitudeCache";
                        Q_NamedAverageAltitudeCache.AliasForTable = "Altitude";
                        Q_NamedAverageAltitudeCache.Restriction = "LocalisationSystemID=4";
                        Q_NamedAverageAltitudeCache.IsVisible = false;
                        Q_NamedAverageAltitudeCache.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_NamedAverageAltitudeCache);


                        DiversityCollection.Forms.GridModeQueryField Q_GeographicRegionPropertyURI = new Forms.GridModeQueryField();
                        Q_GeographicRegionPropertyURI.Table = "CollectionEventProperty";
                        Q_GeographicRegionPropertyURI.Column = "PropertyURI";
                        Q_GeographicRegionPropertyURI.AliasForColumn = "_GeographicRegionPropertyURI";
                        Q_GeographicRegionPropertyURI.AliasForTable = "GeographicRegion";
                        Q_GeographicRegionPropertyURI.Restriction = "PropertyID=10";
                        Q_GeographicRegionPropertyURI.IsVisible = false;
                        Q_GeographicRegionPropertyURI.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_GeographicRegionPropertyURI);


                        DiversityCollection.Forms.GridModeQueryField Q_LithostratigraphyPropertyURI = new Forms.GridModeQueryField();
                        Q_LithostratigraphyPropertyURI.Table = "CollectionEventProperty";
                        Q_LithostratigraphyPropertyURI.Column = "PropertyURI";
                        Q_LithostratigraphyPropertyURI.AliasForColumn = "_LithostratigraphyPropertyURI";
                        Q_LithostratigraphyPropertyURI.AliasForTable = "Lithostratigraphy";
                        Q_LithostratigraphyPropertyURI.Restriction = "PropertyID=30";
                        Q_LithostratigraphyPropertyURI.IsVisible = false;
                        Q_LithostratigraphyPropertyURI.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_LithostratigraphyPropertyURI);


                        DiversityCollection.Forms.GridModeQueryField Q_ChronostratigraphyPropertyURI = new Forms.GridModeQueryField();
                        Q_ChronostratigraphyPropertyURI.Table = "CollectionEventProperty";
                        Q_ChronostratigraphyPropertyURI.Column = "PropertyURI";
                        Q_ChronostratigraphyPropertyURI.AliasForColumn = "_ChronostratigraphyPropertyURI";
                        Q_ChronostratigraphyPropertyURI.AliasForTable = "Chronostratigraphy";
                        Q_ChronostratigraphyPropertyURI.Restriction = "PropertyID=20";
                        Q_ChronostratigraphyPropertyURI.IsVisible = false;
                        Q_ChronostratigraphyPropertyURI.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_ChronostratigraphyPropertyURI);

                        DiversityCollection.Forms.GridModeQueryField Q_BiostratigraphyPropertyURI = new Forms.GridModeQueryField();
                        Q_BiostratigraphyPropertyURI.Table = "CollectionEventProperty";
                        Q_BiostratigraphyPropertyURI.Column = "PropertyURI";
                        Q_BiostratigraphyPropertyURI.AliasForColumn = "_BiostratigraphyPropertyURI";
                        Q_BiostratigraphyPropertyURI.AliasForTable = "Biostratigraphy";
                        Q_BiostratigraphyPropertyURI.Restriction = "PropertyID=60";
                        Q_BiostratigraphyPropertyURI.IsVisible = false;
                        Q_BiostratigraphyPropertyURI.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_BiostratigraphyPropertyURI);

                        DiversityCollection.Forms.GridModeQueryField Q_SecondUnitFamilyCache = new Forms.GridModeQueryField();
                        Q_SecondUnitFamilyCache.Table = "Identification";
                        Q_SecondUnitFamilyCache.Column = "FamilyCache";
                        Q_SecondUnitFamilyCache.AliasForColumn = "_SecondUnitFamilyCache";
                        Q_SecondUnitFamilyCache.AliasForTable = "SecondUnitIdentification";
                        Q_SecondUnitFamilyCache.IsVisible = false;
                        Q_SecondUnitFamilyCache.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_SecondUnitFamilyCache);

                        DiversityCollection.Forms.GridModeQueryField Q_SecondUnitOrderCache = new Forms.GridModeQueryField();
                        Q_SecondUnitOrderCache.Table = "Identification";
                        Q_SecondUnitOrderCache.Column = "OrderCache";
                        Q_SecondUnitOrderCache.AliasForColumn = "_SecondUnitOrderCache";
                        Q_SecondUnitOrderCache.AliasForTable = "SecondUnitIdentification";
                        Q_SecondUnitOrderCache.IsVisible = false;
                        Q_SecondUnitOrderCache.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_SecondUnitOrderCache);

                    }
                    catch  { }
                }
                return this._GridModeHiddenQueryFields;
            }
        }

        /// <summary>
        /// Set the visibilty of a QueryField base on the index
        /// </summary>
        /// <param name="Index">the index of the query field</param>
        /// <param name="IsVisible">The visibility</param>
        private void GridModeSetIsVisibleForQueryField(int Index, bool IsVisible)
        {
            try
            {
                DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
                Q.AliasForColumn = this._GridModeQueryFields[Index].AliasForColumn;
                Q.AliasForTable = this._GridModeQueryFields[Index].AliasForTable;
                Q.Column = this._GridModeQueryFields[Index].Column;
                Q.DatasourceTable = this._GridModeQueryFields[Index].DatasourceTable;
                Q.IsVisible = IsVisible;
                Q.Restriction = this._GridModeQueryFields[Index].Restriction;
                Q.Table = this._GridModeQueryFields[Index].Table;
                Q.AliasForTable = this._GridModeQueryFields[Index].AliasForTable;
                Q.RemoveLinkColumn = this._GridModeQueryFields[Index].RemoveLinkColumn;
                this._GridModeQueryFields[Index] = Q;

            }
            catch  { }
        }

        /// <summary>
        /// A QueryField based on the name of the column alias
        /// </summary>
        /// <param name="ColumnAlias">the alias for the column</param>
        /// <returns>The Query field</returns>
        private DiversityCollection.Forms.GridModeQueryField GridModeGetQueryField(string ColumnAlias)
        {
            DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
            try
            {
                foreach (DiversityCollection.Forms.GridModeQueryField QF in this.GridModeQueryFields)
                {
                    if (QF.AliasForColumn == ColumnAlias)
                        return QF;
                }

            }
            catch { }
            return Q;
        }

        /// <summary>
        /// Getting the parameters of the node tags:
        /// string[] separated by ";"
        /// 0 - Table
        /// 1 - AliasForTable (may be empty if table name is unique)
        /// 2 - The restriction used for the view; necessary for an update in the dataset
        /// 3 - Column
        /// 4 - The alias for the column - unique in view
        /// 5 - The datasource table of the column
        /// 7 - RemoveLinkColumn - a dummy column for the buttons for removing a link
        /// </summary>
        /// <param name="Node">the node with the parameters encoded in the tag</param>
        private void GridModeQueryFieldsAddChildNodes(System.Windows.Forms.TreeNode Node)
        {
            char[] charSeparators = new char[] { ';' };
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                try
                {
                    if (N.Tag != null && N.Tag.ToString().Contains(";"))
                    {
                        string[] Parameter = this.GridModeFieldTagArray(N.Tag.ToString());
                        if (Parameter.Length > 1)
                        {
                            DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
                            if (Parameter.Length < 4) continue;

                            Q.Table = Parameter[0];

                            Q.AliasForTable = Parameter[1];
                            if (Q.AliasForTable.Length == 0) Q.AliasForTable = Q.Table;

                            Q.Restriction = Parameter[2];

                            Q.Column = Parameter[3];

                            if (Parameter.Length > 4)
                                Q.AliasForColumn = Parameter[4];
                            else Q.AliasForColumn = "";
                            if (Q.AliasForColumn.Length == 0)
                                Q.AliasForColumn = Q.Column;

                            if (Parameter.Length > 5)
                                Q.DatasourceTable = Parameter[5];
                            else Q.DatasourceTable = "";

                            if (N.Checked)
                                Q.IsVisible = true;
                            else Q.IsVisible = false;
                            Q.IsHidden = false;
                            if (Parameter.Length > 7)
                                Q.RemoveLinkColumn = Parameter[7];
                            else Q.RemoveLinkColumn = "";
                            if (Q.Table.Length > 0)
                                Q.Entity = Q.Table;
                            if (Q.Column.Length > 0)
                                Q.Entity += "." + Q.Column;
                            this._GridModeQueryFields.Add(Q);
                        }
                    }
                    else if (N.Tag != null)
                    {
                    }
                    this.GridModeQueryFieldsAddChildNodes(N);

                }
                catch {}
            }
        }

        private GridModeQueryField GridModeQueryFieldOfNode(System.Windows.Forms.TreeNode N)
        {
            DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
            try
            {
                if (N.Tag != null)
                {
                    string[] Parameter = this.GridModeFieldTagArray(N.Tag.ToString());
                    if (Parameter.Length > 1)
                    {
                        Q.Table = Parameter[0];

                        Q.AliasForTable = Parameter[1];
                        if (Q.AliasForTable.Length == 0) Q.AliasForTable = Q.Table;

                        Q.Restriction = Parameter[2];

                        Q.Column = Parameter[3];

                        if (Parameter.Length > 4)
                            Q.AliasForColumn = Parameter[4];
                        else Q.AliasForColumn = "";
                        if (Q.AliasForColumn.Length == 0)
                            Q.AliasForColumn = Q.Column;

                        if (Parameter.Length > 5)
                            Q.DatasourceTable = Parameter[5];
                        else Q.DatasourceTable = "";

                        if (N.Checked)
                            Q.IsVisible = true;
                        else Q.IsVisible = false;
                        Q.IsHidden = false;
                        if (Parameter.Length > 7)
                            Q.RemoveLinkColumn = Parameter[7];
                        else Q.RemoveLinkColumn = "";
                        if (Q.Table.Length > 0)
                            Q.Entity = Q.Table;
                        if (Q.Column.Length > 0)
                            Q.Entity += "." + Q.Column;
                    }
                }

            }
            catch  { }
            return Q;
        }

        /// <summary>
        /// a parameter array based on a string
        /// </summary>
        /// <param name="Tag">the string separated with ";"</param>
        /// <returns>a string array</returns>
        private string[] GridModeFieldTagArray(string Tag)
        {
            char[] charSeparators = new char[] { ';' };
            string[] Parameter = Tag.Split(charSeparators);
            return Parameter;
        }
        
#endregion

#region Translation of Alias

        private string ColumnNameForAlias(string Alias)
        {
            string ColumnName = "";
            foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
            {
                if (Q.AliasForColumn == Alias)
                {
                    ColumnName = Q.Column;
                    break;
                }
            }
            return ColumnName;
        }

        private string TableNameForAlias(string Alias)
        {
            string TableName = "";
            foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
            {
                if (Q.AliasForTable == Alias)
                {
                    TableName = Q.Table;
                    break;
                }
            }
            if (TableName.Length == 0)
            {
                foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                {
                    if (Q.AliasForColumn == Alias)
                    {
                        TableName = Q.Table;
                        break;
                    }
                }
            }
            return TableName;
        }
        
#endregion

#endregion

#region Properties

        public int SpecimenID { get { return this._SpecimenID; } }

        public System.Collections.Generic.Dictionary<string, string> ProjectSettings
        {
            get 
            {
                if (this._ProjectSettings == null 
                    && this._ProjectID != null)
                {
                    this._ProjectSettings = new Dictionary<string,string>();
                    try
                    {
                        string SQL = "SELECT ProjectSetting, Value FROM [DiversityProjects].[dbo].[ProjectSettings_Defaults] (" + this._ProjectID.ToString() + ")";
                        System.Data.DataTable dt = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            this._ProjectSettings.Add(R["ProjectSetting"].ToString(), R["Value"].ToString());
                        }
                    }
                    catch { }
                }
                return _ProjectSettings; 
            }
            //set { _ProjectSettings = value; }
        }

#endregion

#region Visibility of images and column selection tree

        private void setImageVisibility(string Visibility)
        {
            try
            {
                if (Visibility.Length != 2) return;

                if (Visibility.Substring(0, 1) == "1")
                    this.buttonHeaderShowSelectionTree.BackColor = System.Drawing.Color.Red;
                else
                    this.buttonHeaderShowSelectionTree.BackColor = System.Drawing.SystemColors.Control;


                if (Visibility.Substring(1, 1) == "1")
                    this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.Color.Red;
                else
                {
                    if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > 0)
                        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.Color.Yellow;
                    else
                        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.SystemColors.Control;
                }

            }
            catch { }

            this.setVisibility();
        }

        private void buttonHeaderShowSelectionTree_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonHeaderShowSelectionTree.BackColor == System.Drawing.SystemColors.Control)
                    this.buttonHeaderShowSelectionTree.BackColor = System.Drawing.Color.Red;
                else this.buttonHeaderShowSelectionTree.BackColor = System.Drawing.SystemColors.Control;

            }
            catch { }
            this.setVisibility();
        }

        private void buttonHeaderShowSpecimenImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonHeaderShowSpecimenImage.BackColor == System.Drawing.Color.Red)
                {
                    if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > 0)
                        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.Color.Yellow;
                    else
                        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.Color.Red;
                    if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > 0
                        && this.listBoxSpecimenImage.Items.Count == 0)
                        this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages,
                            this.dataSetCollectionSpecimen.CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                }

            }
            catch { }
            this.setVisibility();
        }

        private void setVisibility()
        {
            try
            {
                if (this.ShowColumnSelectionTree || this.ShowImagesSpecimen)
                {
                    this.splitContainerMain.Panel1Collapsed = false;
                    if (this.ShowImagesSpecimen) this.splitContainerTreeView.Panel2Collapsed = false;
                    else this.splitContainerTreeView.Panel2Collapsed = true;
                    if (this.ShowColumnSelectionTree) this.splitContainerTreeView.Panel1Collapsed = false;
                    else this.splitContainerTreeView.Panel1Collapsed = true;
                }
                else
                    this.splitContainerMain.Panel1Collapsed = true;

            }
            catch { }
        }
        
        private bool ShowColumnSelectionTree
        {
            get
            {
                if (this.buttonHeaderShowSelectionTree.BackColor == System.Drawing.SystemColors.Control)
                    return false;
                else return true;
            }
        }

        private bool ShowImagesSpecimen
        {
            get
            {
                if (this.buttonHeaderShowSpecimenImage.BackColor == System.Drawing.Color.Red)
                    return true;
                else
                    return false;
            }
        }

        private string CurrentHeaderDisplaySettings
        {
            get
            {
                string Setting = "11";

                try
                {
                    if (this.ShowColumnSelectionTree) Setting = "1";
                    else Setting = "0";
                    if (this.ShowImagesSpecimen) Setting += "1";
                    else Setting += "0";

                }
                catch 
                {
                    Setting = "11";
                }
                return Setting;
            }
        }

#endregion

#region Copy & Paste from Excel & Co.

        private void dataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Shift && e.KeyCode == Keys.Insert) || (e.Control && e.KeyCode == Keys.V))
            {
                this.PasteFromClipboard(this.dataGridView);
            }
            else if (!e.Control && !e.Shift && (e.KeyCode == Keys.RButton || e.KeyCode == Keys.Back))
            {
                this.ClearGridViewCells(this.dataGridView);
            }
            //else
            //{
            //    System.Windows.Forms.DataGridView dataGridView = (System.Windows.Forms.DataGridView)sender;
            //}
        }

        private void contextMenuStripDataGrid_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "toolStripMenuItemClearCells")
            {
                this.ClearGridViewCells(this.dataGridView);
                return;
            }
            else if ((System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.CommaSeparatedValue)
                || System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.Text))
                && e.ClickedItem.Name == "toolStripMenuItemCopyFromClipboard")
            {
                this.PasteFromClipboard(this.dataGridView);
                return;

            }
            else if (e.ClickedItem.Name == "toolStripMenuItemCopyFromClipboard")
            {
                System.Windows.Forms.MessageBox.Show("Only text and spreadsheet values can be copied");
                return;
            }
        }

        private void PasteFromClipboard(System.Windows.Forms.DataGridView dataGridView)
        {
            if (System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.CommaSeparatedValue)
                || System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.Text))
            {
                // finding the top row
                int IndexTopRow = dataGridView.Rows.Count;
                if (dataGridView.SelectedCells.Count > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
                        if (IndexTopRow > C.RowIndex) IndexTopRow = C.RowIndex;
                }

                // parsing the content of the clipboard
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = DiversityCollection.Forms.FormGridFunctions.ClipBoardValues;// this.ClipBoardValues;
                System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = DiversityCollection.Forms.FormGridFunctions.GridColums(dataGridView);
                if (!DiversityCollection.Forms.FormGridFunctions.CanCopyClipboardInDataGrid(IndexTopRow, ClipBoardValues, GridColums, dataGridView))
                    return;

                try
                {
                    for (int ii = 0; ii < GridColums.Count; ii++) // the columns
                    {
                        for (int i = 0; i < ClipBoardValues.Count; i++) // the rows
                        {
                            if (dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].ReadOnly)
                                continue;
                            if (DiversityCollection.Forms.FormGridFunctions.ValueIsValid(dataGridView, GridColums[ii].Index, ClipBoardValues[i][ii]))
                            {
                                dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].Value = ClipBoardValues[i][ii];
                                this.checkForMissingAndDefaultValues(dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index], true);
                            }
                            else
                            {
                                string Message = ClipBoardValues[i][ii] + " is not a valid value for "
                                    + dataGridView.Columns[GridColums[ii].Index].DataPropertyName
                                    + "\r\n\r\nDo you want to try to insert the other values?";
                                if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo) == DialogResult.No)
                                    break;
                            }
                            if (i + IndexTopRow + 3 > dataGridView.Rows.Count)
                                continue;
                        }
                    }
                }
                catch { }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Only text and spreadsheet values can be copied");
                return;
            }
        }

        private void ClearGridViewCells(System.Windows.Forms.DataGridView dataGridView)
        {
            foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
            {
                try
                {
                    C.Value = null;
                    if (C.Value != null)
                    {
                        C.Value = "";
                        if (C.Value != "")
                            C.Value = System.DBNull.Value;
                    }
                }
                catch (System.Exception ex) { }
            }
        }

#endregion

#region Replace, Insert, Append, Clear Function for a single Column

        private void buttonMarkWholeColumn_Click(object sender, EventArgs e)
        {
            this.MarkWholeColumn(this.dataGridViewActive, this.dataGridViewActive.SelectedCells[0].ColumnIndex);
        }

        private void MarkWholeColumn(System.Windows.Forms.DataGridView dataGridView, int ColumnIndex)
        {
            DiversityCollection.Forms.FormGridFunctions.MarkWholeColumn(ColumnIndex, dataGridView);
            try
            {
                string ColumnName = dataGridView.Columns[ColumnIndex].DataPropertyName.ToString();
                if (ColumnName == "CollectionSpecimenID") return;

                string DisplayTextForColumn = ColumnName;
                if (DisplayTextForColumn.Length > 20)
                {
                    string[] Parts = DisplayTextForColumn.Split(new char[] { '_' });
                    int L = 20;
                    while (L > 2 && DisplayTextForColumn.Length > 20)
                    {
                        for (int i = 0; i < Parts.Length; i++)
                        {
                            if (Parts[i].Length > L)
                                Parts[i] = Parts[i].Substring(0, L) + ".";
                        }
                        DisplayTextForColumn = Parts[0];
                        for (int i = 1; i < Parts.Length; i++)
                        {
                            DisplayTextForColumn += " " + Parts[i];
                            if (!DisplayTextForColumn.EndsWith(".") && i < Parts.Length)
                                DisplayTextForColumn += " ";
                        }
                        L--;
                    }
                }
                this.labelGridViewReplaceColumnName.Text = DisplayTextForColumn;

            }
            catch { }
            this.enableReplaceButtons(dataGridView, true);
        }

        private void enableReplaceButtons(System.Windows.Forms.DataGridView dataGridView, bool IsEnabled)
        {
            System.Collections.Generic.List<int> ColList = new List<int>();
            for (int i = 0; i < dataGridView.SelectedCells.Count; i++)
                if (!ColList.Contains(dataGridView.SelectedCells[i].ColumnIndex)) ColList.Add(dataGridView.SelectedCells[i].ColumnIndex);
            if (IsEnabled && ColList.Count > 1) 
                IsEnabled = false;
            this.buttonGridModeReplace.Enabled = IsEnabled;
            this.buttonGridModeInsert.Enabled = IsEnabled;
            this.buttonGridModeRemove.Enabled = IsEnabled;
            this.buttonGridModeAppend.Enabled = IsEnabled;
            this.radioButtonGridModeAppend.Enabled = IsEnabled;
            this.radioButtonGridModeInsert.Enabled = IsEnabled;
            this.radioButtonGridModeRemove.Enabled = IsEnabled;
            this.radioButtonGridModeReplace.Enabled = IsEnabled;
            this.textBoxGridModeReplace.Enabled = IsEnabled;
            this.textBoxGridModeReplaceWith.Enabled = IsEnabled;
            this.comboBoxReplace.Enabled = IsEnabled;
            this.comboBoxReplaceWith.Enabled = IsEnabled;
            this.labelGridModeReplaceWith.Enabled = IsEnabled;
            this.labelGridViewReplaceColumn.Enabled = IsEnabled;
            this.labelGridViewReplaceColumnName.Enabled = IsEnabled;

            if (this.IsLinkColumn)
            {
                this.linkLabelLink.Enabled = IsEnabled;
                this.buttonGetLink.Enabled = IsEnabled;
            }
            else
            {
                this.buttonGetLink.Enabled = false;
                this.linkLabelLink.Enabled = false;
            }

            if (!IsEnabled) this.labelGridViewReplaceColumnName.Text = "?";
            if (dataGridView.SelectedCells.Count > 0 && ColList.Count == 1)
            {
                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Combo
                        = (System.Windows.Forms.DataGridViewComboBoxColumn)dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex];
                    if (Combo.DisplayMember == "DisplayText" && Combo.ValueMember == "Code")
                    {
                        this.comboBoxReplace.Width = 100;
                        this.comboBoxReplaceWith.Width = 100;
                        System.Object O = Combo.DataSource;
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)O;
                        DiversityCollection.Datasets.DataSetCollectionSpecimenGridMode DS = (DiversityCollection.Datasets.DataSetCollectionSpecimenGridMode)BS.DataSource;
                        System.Data.DataTable dtReplace = DS.Tables[BS.DataMember.ToString()].Copy();
                        System.Data.DataTable dtReplaceWith = DS.Tables[BS.DataMember.ToString()].Copy();

                        this.comboBoxReplace.DataSource = dtReplace;
                        this.comboBoxReplace.DisplayMember = Combo.DisplayMember;
                        this.comboBoxReplace.ValueMember = Combo.ValueMember;
                        DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplace, AutoCompleteMode.Suggest);

                        this.comboBoxReplaceWith.DataSource = dtReplaceWith;
                        this.comboBoxReplaceWith.DisplayMember = Combo.DisplayMember;
                        this.comboBoxReplaceWith.ValueMember = Combo.ValueMember;
                        DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplaceWith, AutoCompleteMode.Suggest);
                    }
                    else
                    {
                        this.comboBoxReplace.Width = 100;
                        this.comboBoxReplaceWith.Width = 100;
                        System.Object O = Combo.DataSource;
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)O;
                        DiversityCollection.Datasets.DataSetCollectionSpecimenGridMode DS = (DiversityCollection.Datasets.DataSetCollectionSpecimenGridMode)BS.DataSource;
                        System.Data.DataTable dtReplace = DS.Tables[BS.DataMember.ToString()].Copy();
                        System.Data.DataTable dtReplaceWith = DS.Tables[BS.DataMember.ToString()].Copy();

                        this.comboBoxReplace.DataSource = dtReplace;
                        this.comboBoxReplace.DisplayMember = Combo.DisplayMember;
                        this.comboBoxReplace.ValueMember = Combo.ValueMember;
                        DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplace, AutoCompleteMode.Suggest);

                        this.comboBoxReplaceWith.DataSource = dtReplaceWith;
                        this.comboBoxReplaceWith.DisplayMember = Combo.DisplayMember;
                        this.comboBoxReplaceWith.ValueMember = Combo.ValueMember;
                        DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplaceWith, AutoCompleteMode.Suggest);
                    }
                }
                else if (typeof(System.Windows.Forms.DataGridViewButtonCell)
                    == dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewButtonColumn Button
                        = (System.Windows.Forms.DataGridViewButtonColumn)dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex];
                    if (Button.Text == null
                        && (this.IsLinkColumn))
                    {
                        this.buttonGridModeReplace.Enabled = false;
                        this.buttonGridModeAppend.Enabled = false;

                        this.radioButtonGridModeReplace.Enabled = false;
                        this.radioButtonGridModeAppend.Enabled = false;

                        this.radioButtonGridModeRemove.Enabled = true;
                        this.radioButtonGridModeInsert.Enabled = true;
                        this.buttonGridModeInsert.Enabled = true;
                        this.buttonGridModeRemove.Enabled = true;

                        this.radioButtonGridModeReplace.Checked = false;
                        this.radioButtonGridModeInsert.Checked = false;
                        this.radioButtonGridModeAppend.Checked = false;

                        this.radioButtonGridModeRemove.Checked = true;
                    }
                    else if (Button.Text == "X"
                        && Button.DataPropertyName.StartsWith("Remove_link_"))
                    {
                        this.buttonGridModeReplace.Enabled = false;
                        this.buttonGridModeInsert.Enabled = false;
                        this.buttonGridModeAppend.Enabled = false;
                        this.buttonGridModeRemove.Enabled = false;

                        this.radioButtonGridModeReplace.Enabled = false;
                        this.radioButtonGridModeInsert.Enabled = false;
                        this.radioButtonGridModeAppend.Enabled = false;
                        this.radioButtonGridModeRemove.Enabled = false;

                    }
                }
                else if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                {
                    try
                    {
                        string DataProperty = dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName;
                        string Table = "";
                        string Column = "";
                        DiversityCollection.Forms.FormGridFunctions.getDataColumn(this.treeViewGridModeFieldSelector, Forms.FormGridFunctions.GridType.Specimen, DataProperty, ref Table, ref Column);
                        if (Table.Length > 0 && Column.Length > 0)
                        {
                            this.textBoxGridModeReplace.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                            this.textBoxGridModeReplace.AutoCompleteMode = AutoCompleteMode.Suggest;
                            this.textBoxGridModeReplace.AutoCompleteSource = AutoCompleteSource.CustomSource;

                            this.textBoxGridModeReplaceWith.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                            this.textBoxGridModeReplaceWith.AutoCompleteMode = AutoCompleteMode.Suggest;
                            this.textBoxGridModeReplaceWith.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                    catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
                }
            }
            this.setReplaceOptions();
        }

        private void buttonGridModeReplace_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear(this.dataGridViewActive);
        }

        private void buttonGridModeInsert_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear(this.dataGridViewActive);
        }

        private void buttonGridModeAppend_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear(this.dataGridViewActive);
        }

        private void buttonGridModeRemove_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear(this.dataGridViewActive);
        }

        private System.Collections.Generic.Dictionary<string, string> _LinkLabelLinkColumns;

        private void linkLabelLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this._LinkLabelLinkColumns != null && this._LinkLabelLinkColumns.Count > 0)
            {
                string Message = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._LinkLabelLinkColumns)
                {
                    Message += KV.Key + ": " + KV.Value + "\r\n";
                }
                System.Windows.Forms.MessageBox.Show(Message);
            }
            //if (this.linkLabelLink.Text.Length > 0)
            //{
            //    System.Windows.Forms.MessageBox.Show(this.linkLabelLink.Text + "\r\n" + this.toolTip.GetToolTip(this.linkLabelLink));
            //}
        }

        private void buttonLinkRemove_Click(object sender, EventArgs e)
        {
            this.linkLabelLink.Text = "";
            this.toolTip.SetToolTip(this.linkLabelLink, "");
            this.buttonGetLink.Visible = true;
            this.buttonLinkRemove.Visible = false;
            this.buttonGridModeInsert.Enabled = false;
        }

        private void buttonGetLink_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormRemoteQuery f;
            try
            {
                string ValueColumn = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName;
                string DisplayColumn = ValueColumn;
                bool IsListInDatabase = false;
                string ListInDatabase = "";
                DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit;
                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RemoteValueBindings = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
                if (this._LinkLabelLinkColumns == null)
                    this._LinkLabelLinkColumns = new Dictionary<string, string>();
                switch (ValueColumn)
                {
                    case "NamedAreaLocation2":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityGazetteer")) return;
                        ValueColumn = "NamedAreaLocation2";
                        DisplayColumn = "Named_area";
                        DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbCountry = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbCountry.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbCountry.Column = "Country";
                        RvbCountry.RemoteParameter = "Country";
                        RemoteValueBindings.Add(RvbCountry);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLatitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbLatitude.Column = "_NamedAverageLatitudeCache";
                        RvbLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLongitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbLongitude.Column = "_NamedAverageLongitudeCache";
                        RvbLongitude.RemoteParameter = "Longitude";
                        RemoteValueBindings.Add(RvbLongitude);
                        break;

                    case "Link_to_SamplingPlots":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversitySamplingPlots")) return;
                        ValueColumn = "Link_to_SamplingPlots";
                        DisplayColumn = "Sampling_plot";
                        DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotLatitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSamplingPlotLatitude.Column = "Latitude_of_sampling_plot";
                        RvbSamplingPlotLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotLongitude.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSamplingPlotLongitude.Column = "Longitude_of_sampling_plot";
                        RvbSamplingPlotLongitude.RemoteParameter = "Longitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLongitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotAccuracy = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotAccuracy.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSamplingPlotAccuracy.Column = "Accuracy_of_sampling_plot";
                        RvbSamplingPlotAccuracy.RemoteParameter = "Accuracy";
                        RemoteValueBindings.Add(RvbSamplingPlotAccuracy);
                        break;

                    case "Geographic_region":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_GeographicRegionPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Geographic regions";
                        DiversityWorkbench.ScientificTerm S_Geo = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Geo;
                        break;
                    case "Lithostratigraphy":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_LithostratigraphyPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Lithostratigraphy";
                        DiversityWorkbench.ScientificTerm S_Litho = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Litho;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyLitho = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbHierarchyLitho.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbHierarchyLitho.Column = "_LithostratigraphyPropertyHierarchyCache";
                        RvbHierarchyLitho.RemoteParameter = "HierarchyCache";
                        RemoteValueBindings.Add(RvbHierarchyLitho);
                        break;

                    case "Chronostratigraphy":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_ChronostratigraphyPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Chronostratigraphy";
                        DiversityWorkbench.ScientificTerm S_Chrono = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Chrono;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyChrono = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbHierarchyChrono.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbHierarchyChrono.Column = "_ChronostratigraphyPropertyHierarchyCache";
                        RvbHierarchyChrono.RemoteParameter = "HierarchyCache";
                        RemoteValueBindings.Add(RvbHierarchyChrono);
                        break;

                    case "Biostratigraphy":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
                        ValueColumn = "_BiostratigraphyPropertyURI";
                        IsListInDatabase = true;
                        ListInDatabase = "Biostratigraphy";
                        DiversityWorkbench.ScientificTerm S_Bio = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Bio;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyBio = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbHierarchyBio.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbHierarchyBio.Column = "_BiostratigraphyPropertyHierarchyCache";
                        RvbHierarchyBio.RemoteParameter = "HierarchyCache";
                        RemoteValueBindings.Add(RvbHierarchyBio);
                        break;

                    case "Link_to_DiversityTaxonNames":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames")) return;
                        ValueColumn = "Link_to_DiversityTaxonNames";
                        DisplayColumn = "Taxonomic_name";
                        DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbFamily.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbFamily.Column = "Family_of_taxon";
                        RvbFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbOrder.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbOrder.Column = "Order_of_taxon";
                        RvbOrder.RemoteParameter = "Order";
                        RemoteValueBindings.Add(RvbOrder);
                        break;

                    case "Link_to_DiversityTaxonNames_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames")) return;
                        ValueColumn = "Link_to_DiversityTaxonNames_of_second_organism";
                        DisplayColumn = "Taxonomic_name_of_second_organism";
                        DiversityWorkbench.TaxonName Tsecond = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Tsecond;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSecondFamily.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSecondFamily.Column = "_SecondUnitFamilyCache";
                        RvbSecondFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbSecondFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSecondOrder.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSecondOrder.Column = "Order_of_taxon";
                        RvbSecondOrder.RemoteParameter = "_SecondUnitOrderCache";
                        RemoteValueBindings.Add(RvbSecondOrder);
                        break;

                    case "Link_to_DiversityAgents":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents";
                        DisplayColumn = "Collectors_name";
                        DiversityWorkbench.Agent Agent = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Agent;
                        break;

                    case "Depositors_link_to_DiversityAgents":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Depositors_link_to_DiversityAgents";
                        DisplayColumn = "Depositors_name";
                        DiversityWorkbench.Agent Depositor = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Depositor;
                        break;

                    case "Link_to_DiversityAgents_for_responsible":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_responsible";
                        DisplayColumn = "ResponsibleName";
                        DiversityWorkbench.Agent Identifier = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier;
                        break;

                    case "Link_to_DiversityAgents_for_responsible_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_responsible_of_second_organism";
                        DisplayColumn = "Responsible_of_second_organism";
                        DiversityWorkbench.Agent Identifier2 = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier2;
                        break;

                    case "Link_to_DiversityExsiccatae":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityExsiccatae")) return;
                        ValueColumn = "Link_to_DiversityExsiccatae";
                        DisplayColumn = "Exsiccata_abbreviation";
                        DiversityWorkbench.Exsiccate Exs = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Exs;
                        break;

                    case "Link_to_DiversityReferences":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
                        ValueColumn = "Link_to_DiversityReferences";
                        DisplayColumn = "Reference_title";
                        DiversityWorkbench.Reference Ref = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Ref;
                        break;

                    case "Link_to_DiversityReferences_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
                        ValueColumn = "Link_to_DiversityReferences_of_second_organism";
                        DisplayColumn = "Reference_title_of_second_organism";
                        DiversityWorkbench.Reference Ref2 = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Ref2;
                        break;

                    case "Link_to_DiversityReferences_for_specimen":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
                        ValueColumn = "Link_to_DiversityReferences_for_specimen";
                        DisplayColumn = "Reference_title_for_specimen";
                        DiversityWorkbench.Reference Ref3 = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Ref3;
                        break;

                    case "Link_to_DiversityCollection_for_relation":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityCollection")) return;
                        ValueColumn = "Link_to_DiversityCollection_for_relation";
                        DisplayColumn = "Related_specimen_display_text";
                        DiversityWorkbench.CollectionSpecimen Rel = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Rel;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSpecimenUri = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSpecimenUri.BindingSource = this.firstLinesCollectionSpecimenBindingSource;
                        RvbSpecimenUri.Column = "Related_specimen_URL";
                        RvbSpecimenUri.RemoteParameter = "_URL";
                        RemoteValueBindings.Add(RvbSpecimenUri);

                        break;

                    case "Link_to_DiversityAgents_for_determiner":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_determiner";
                        DisplayColumn = "Determiner";
                        DiversityWorkbench.Agent Identifier1 = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier1;
                        break;

                    case "Link_to_DiversityAgents_for_determiner_of_second_organism":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "Link_to_DiversityAgents_for_determiner_of_second_organism";
                        DisplayColumn = "Determiner_of_second_organism";
                        DiversityWorkbench.Agent Identifier_2 = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier_2;
                        break;

                    default:
                        DiversityWorkbench.TaxonName Default = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Default;
                        break;
                }

                if (IWorkbenchUnit != null)
                {
                    //System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                    string URI = this.linkLabelLink.Text;
                    //if (RU != null)
                    //    if (!RU[ValueColumn].Equals(System.DBNull.Value)) URI = RU[ValueColumn].ToString();
                    if (URI.Length == 0)
                    {
                        if (IsListInDatabase)
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit, ListInDatabase);
                        }
                        else
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit);
                        }
                    }
                    else
                    {
                        f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, IWorkbenchUnit);
                    }
                    try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
                    catch { }
                    f.TopMost = true;
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.DisplayText != null)
                    {
                        //System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                        //R.BeginEdit();
                        this.linkLabelLink.Text = f.URI;
                        this.toolTip.SetToolTip(this.linkLabelLink, f.DisplayText);
                        if (RemoteValueBindings != null)
                        {
                            this._LinkLabelLinkColumns = new Dictionary<string, string>();
                            //for (int i = 0; i < RemoteValueBindings.Count; i++)
                            //    this._LinkLabelLinkColumns.Add(RemoteValueBindings[i].Column, RemoteValueBindings[i].RemoteParameter);
                        }
                        if (this.linkLabelLink.Text.Length > 0)
                        {
                            this.buttonGridModeInsert.Enabled = true;
                            this.buttonGetLink.Visible = false;
                            this.buttonLinkRemove.Visible = true;
                            this._LinkLabelLinkColumns.Add(DisplayColumn, f.DisplayText);
                            this._LinkLabelLinkColumns.Add(ValueColumn, f.URI);
                            if (RemoteValueBindings != null && RemoteValueBindings.Count > 0)
                            {
                                foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in RemoteValueBindings)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> P in IWorkbenchUnit.UnitValues())
                                    {
                                        if (RVB.RemoteParameter == P.Key)
                                        {
                                            System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;
                                            this._LinkLabelLinkColumns.Add(RVB.Column, P.Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void buttonGridModeUndo_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewActive.Rows)
            {
                this.UndoChanges(this.dataGridViewActive, R.Index);
            }
        }

        private void UndoChanges(System.Windows.Forms.DataGridView gridView)
        {
            foreach (System.Windows.Forms.DataGridViewRow R in gridView.Rows)
            {
                this.UndoChanges(gridView, R.Index);
            }
        }

        private void buttonGridModeUndoSingleLine_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewActive.SelectedCells.Count > 0)
            {
                this.UndoChanges(this.dataGridViewActive, this.dataGridViewActive.SelectedCells[0].RowIndex);
            }
        }

        private void UndoChanges(System.Windows.Forms.DataGridView dataGridView, int RowIndex)
        {
            try
            {
                int SpecimenID;
                if (int.TryParse(dataGridView.Rows[RowIndex].Cells[0].Value.ToString(), out SpecimenID))
                {
                    System.Data.DataRow Rori = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfLine(SpecimenID)];
                    if (Rori.RowState != DataRowState.Unchanged)
                    {
                        Rori.RejectChanges();
                    }
                }
            }
            catch { }
        }

        private void ColumnValues_ReplaceInsertClear(System.Windows.Forms.DataGridView dataGridView)
        {
            if (dataGridView.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing has been selected");
                return;
            }
            if (dataGridView.SelectedCells.Count > 0)
            {
                this._StopReplacing = false;
                int Index = dataGridView.SelectedCells[0].ColumnIndex;
                System.Collections.Generic.List<int> PK = new List<int>();
                if (dataGridView.SelectedCells.Count > 1)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
                    {
                        int iPK = 0;
                        try
                        {
                            if (dataGridView.Rows.Count > C.RowIndex &&
                                int.TryParse(dataGridView.Rows[C.RowIndex].Cells[0].Value.ToString(), out iPK))
                                PK.Add(iPK);
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
                else if (dataGridView.SelectedCells.Count == 1)
                {
                    if (System.Windows.Forms.MessageBox.Show("Only one field has been selected.\r\n Do you want to apply the changes to the whole column?", 
                        "Apply to whole column?", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                        {
                            int iPK = 0;
                            if (dataGridView.Rows[R.Index].Cells[0].Value != null)
                            {
                                if (int.TryParse(R.Cells[0].Value.ToString(), out iPK))
                                    PK.Add(iPK);
                            }
                        }
                    }
                    else
                    {
                        int iPK = 0;
                        if (int.TryParse(dataGridView.SelectedCells[0].Value.ToString(), out iPK))
                            PK.Add(iPK);
                    }
                }

                foreach (int ID in PK)
                {
                    try
                    {
                        System.Windows.Forms.DataGridViewRow Row = dataGridView.Rows[0];
                        foreach (System.Windows.Forms.DataGridViewRow RR in dataGridView.Rows)
                        {
                            if (RR.Cells[0].Value.ToString() == ID.ToString())
                            {
                                Row = RR;
                                break;
                            }
                        }

                        string ColumnName = dataGridView.Columns[Index].DataPropertyName;
                        if (Row.Cells[Index].Style.Tag != null && Row.Cells[Index].Style.Tag.ToString() == "Blocked")
                            continue;
                        if (Row.Index > this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count - 1)
                            break;
                        System.Type Type = Row.Cells[Index].ValueType;
                        string Value = "";
                        if (Row.Cells[Index].Value != null && !Row.Cells[Index].Value.Equals(System.DBNull.Value))
                            Value = Row.Cells[Index].Value.ToString();
                        string OriginalText = this.textBoxGridModeReplace.Text;
                        if (this.comboBoxReplace.Visible)
                        {
                            if (this.comboBoxReplace.SelectedIndex > -1)
                                OriginalText = this.comboBoxReplace.SelectedValue.ToString();
                            else OriginalText = "";
                        }
                        string NewText = this.textBoxGridModeReplaceWith.Text;
                        if (this.comboBoxReplaceWith.Visible)
                        {
                            if (this.comboBoxReplaceWith.SelectedIndex > -1)
                                NewText = this.comboBoxReplaceWith.SelectedValue.ToString();
                            else NewText = "";
                        }
                        bool IsValid = true;
                        string CorrectedValue = "";
                        if (this.IsLinkColumn)
                            CorrectedValue = this.linkLabelLink.Text;
                        else
                        {
                            CorrectedValue = DiversityCollection.Forms.FormGridFunctions.CheckReplaceValue(
                                dataGridView,
                                this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen,
                                Index,
                                Value,
                                OriginalText,
                                NewText,
                                ref IsValid,
                                this._ReplaceOptionState);
                        }
                        if (this._StopReplacing) return;
                        if (!IsValid) continue;
                        if (Type == typeof(string))
                        {
                            if (this.IsLinkColumn)
                            {
                                if (this._LinkLabelLinkColumns != null && this._LinkLabelLinkColumns.Count > 0)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._LinkLabelLinkColumns)
                                    {
                                        foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                                        {
                                            if (C.DataPropertyName == KV.Key)
                                                Row.Cells[C.Index].Value = KV.Value;
                                        }
                                    }
                                }
                                else if (this._ReplaceOptionState == Forms.FormGridFunctions.ReplaceOptionState.Clear)
                                {
                                    Row.Cells[Index].Value = null;
                                }
                            }
                            else
                            {
                                Row.Cells[Index].Value = CorrectedValue;
                                if (CorrectedValue.Length == 0 && dataGridView.Columns[Index].CellType.Name == "DataGridViewComboBoxCell") // && this.comboBoxReplaceWith.Visible && this.comboBoxReplaceWith.SelectedIndex == 0)
                                {
                                    Row.Cells[Index].Value = null;
                                }
                            }
                        }
                        else if (Type == typeof(int)
                            || Type == typeof(System.Byte)
                            || Type == typeof(System.DateTime)
                            || Type == typeof(System.Int16))
                        {
                            if (CorrectedValue.Length == 0)
                            {
                                Row.Cells[Index].Value = System.DBNull.Value;
                            }
                            else
                            {
                                if (Type == typeof(int))
                                {
                                    Row.Cells[Index].Value = int.Parse(CorrectedValue);
                                }
                                else if (Type == typeof(System.Byte))
                                {
                                    Row.Cells[Index].Value = System.Byte.Parse(CorrectedValue);
                                }
                                else if (Type == typeof(System.Int16))
                                {
                                    Row.Cells[Index].Value = System.Int16.Parse(CorrectedValue);
                                }
                                else if (Type == typeof(System.DateTime))
                                {
                                    Row.Cells[Index].Value = System.DateTime.Parse(CorrectedValue);
                                }
                            }
                        }
                    }
                    catch { }
                    if (this._StopReplacing)
                        break;
                }
            }
        }

        private string CheckReplaceValue(int ColumnIndex, string OriginalValue, string ReplacedValue, string Replacement, ref bool IsValid)
        {
            string TypeOfColumn = this.dataGridView.Columns[ColumnIndex].ValueType.ToString();
            System.Type Type = this.dataGridView.Columns[ColumnIndex].ValueType;
            //string x = this.dataGridView.Columns[ColumnIndex].DataPropertyName;
            string ColumnName = this.dataGridView.Columns[ColumnIndex].DataPropertyName; //this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Columns[ColumnIndex].ColumnName;
            string Value = "";
            IsValid = true;
            try
            {
                // constructing the new value
                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    switch (this.ReplaceOptionStatus)
                    {
                        case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Insert:
                            Value = Replacement;
                            break;
                        case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Append:
                            Value = Replacement;
                            break;
                        case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace:
                            if (OriginalValue == ReplacedValue)
                                Value = Replacement;
                            else Value = OriginalValue;
                            break;
                    }
                    //Value = Replacement;
                }
                else
                {
                    switch (this.ReplaceOptionStatus)
                    {
                        case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Insert:
                            Value = Replacement + OriginalValue;
                            break;
                        case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Append:
                            Value = OriginalValue + Replacement;
                            break;
                        case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace:
                            if (Type == typeof(int)
                            || Type == typeof(System.Byte)
                            || Type == typeof(System.DateTime)
                            || Type == typeof(System.Int16))
                            {
                                if (OriginalValue == ReplacedValue)
                                    Value = Replacement;
                                else
                                    Value = OriginalValue;
                            }
                            else
                            {
                                if (Replacement.Length < OriginalValue.Length && ReplacedValue.Length > 0)
                                    Value = OriginalValue.Replace(ReplacedValue, Replacement);
                                else if (OriginalValue.Length == 0 && ReplacedValue.Length == 0)
                                    Value = Replacement;
                                else if (OriginalValue == ReplacedValue && ReplacedValue.Length > 0)
                                    Value = Replacement;
                                else if (OriginalValue.IndexOf(ReplacedValue) > -1)
                                    Value = OriginalValue.Replace(ReplacedValue,Replacement) ;
                                else
                                    IsValid = false;
                            }
                            break;
                    }
                }

                if (!DiversityCollection.Forms.FormGridFunctions.ValueIsValid(this.dataGridView, ColumnIndex, Value))
                {
                    IsValid = false;
                    DiversityCollection.Forms.FormGridFunctions.AskIfReplacementShouldBeStopped(ColumnName, Value, this.ReplaceOptionStatus);
                }
            }
            catch { }
            return Value;
        }

        private void radioButtonGridModeReplace_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private void radioButtonGridModeInsert_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private void radioButtonGridModeRemove_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private void radioButtonGridModeAppend_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState ReplaceOptionStatus
        {
            get
            {
                if (this.radioButtonGridModeInsert.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Insert;
                if (this.radioButtonGridModeRemove.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Clear;
                if (this.radioButtonGridModeReplace.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace;
                if (this.radioButtonGridModeAppend.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Append;
                return this._ReplaceOptionState;
            }
        }

        private void setReplaceOptions()
        {
            bool ShowComboBoxes = false;
            bool RestrictToList = false;
            if (this.dataGridView.SelectedCells.Count > 0)
            {
                string DataProperty = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName;

                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Combo
                        = (System.Windows.Forms.DataGridViewComboBoxColumn)this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                    if (Combo.DisplayMember == "DisplayText" && Combo.ValueMember == "Code")
                    {
                        ShowComboBoxes = true;
                        RestrictToList = true;
                    }
                    else
                        ShowComboBoxes = true;
                }
            }
            if (RestrictToList)
            {
                this.comboBoxReplace.DropDownStyle = ComboBoxStyle.DropDownList;
                this.comboBoxReplaceWith.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            else
            {
                this.comboBoxReplace.DropDownStyle = ComboBoxStyle.DropDown;
                this.comboBoxReplaceWith.DropDownStyle = ComboBoxStyle.DropDown;
            }
            this.buttonGetLink.Visible = false;
            this.buttonLinkRemove.Visible = false;
            this.linkLabelLink.Visible = false;
            switch (this.ReplaceOptionStatus)
            {
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace:
                    this.buttonGridModeReplace.Visible = true;
                    this.buttonGridModeRemove.Visible = false;
                    this.buttonGridModeInsert.Visible = false;
                    this.buttonGridModeAppend.Visible = false;
                    this.textBoxGridModeReplace.Visible = !ShowComboBoxes;
                    this.textBoxGridModeReplaceWith.Visible = !ShowComboBoxes;
                    this.comboBoxReplace.Visible = ShowComboBoxes;
                    this.comboBoxReplaceWith.Visible = ShowComboBoxes;
                    this.labelGridModeReplaceWith.Visible = true;
                    break;
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Insert:
                    this.buttonGridModeReplace.Visible = false;
                    this.buttonGridModeRemove.Visible = false;
                    this.buttonGridModeInsert.Visible = true;
                    this.buttonGridModeAppend.Visible = false;
                    this.textBoxGridModeReplace.Visible = false;
                    this.textBoxGridModeReplaceWith.Visible = !ShowComboBoxes;
                    this.comboBoxReplace.Visible = false;
                    this.comboBoxReplaceWith.Visible = ShowComboBoxes;
                    this.labelGridModeReplaceWith.Visible = false;

                    if (this.IsLinkColumn)
                    {
                        this.buttonGetLink.Visible = true;
                        this.buttonGetLink.Enabled = true;
                        this.linkLabelLink.Text = "";
                        this.linkLabelLink.Visible = true;
                        this.linkLabelLink.Enabled = true;
                        this.buttonGridModeInsert.Enabled = false;
                        this.textBoxGridModeReplaceWith.Visible = false;
                        this.comboBoxReplaceWith.Visible = false;
                    }

                    break;
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Append:
                    this.buttonGridModeReplace.Visible = false;
                    this.buttonGridModeRemove.Visible = false;
                    this.buttonGridModeInsert.Visible = false;
                    this.buttonGridModeAppend.Visible = true;
                    this.textBoxGridModeReplace.Visible = false;
                    this.textBoxGridModeReplaceWith.Visible = !ShowComboBoxes;
                    this.comboBoxReplace.Visible = false;
                    this.comboBoxReplaceWith.Visible = ShowComboBoxes;
                    this.labelGridModeReplaceWith.Visible = false;
                    break;
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Clear:
                    this.buttonGridModeReplace.Visible = false;
                    this.buttonGridModeRemove.Visible = true;
                    this.buttonGridModeInsert.Visible = false;
                    this.buttonGridModeAppend.Visible = false;
                    this.textBoxGridModeReplace.Visible = false;
                    this.textBoxGridModeReplaceWith.Visible = false;
                    this.comboBoxReplace.Visible = false;
                    this.comboBoxReplaceWith.Visible = false;
                    this.labelGridModeReplaceWith.Visible = false;
                    break;
            }
        }

        private bool IsLinkColumn
        {
            get
            {
                if (this.dataGridView.SelectedCells == null || this.dataGridView.SelectedCells.Count == 0)
                    return false;

                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();

                if (ColumnName == "NamedAreaLocation2" ||
                    ColumnName == "Link_to_SamplingPlots" ||
                    ColumnName == "Geographic_region" ||
                    ColumnName == "Lithostratigraphy" ||
                    ColumnName == "Chronostratigraphy" ||
                    ColumnName == "Biostratigraphy" ||
                    ColumnName == "Link_to_DiversityTaxonNames" ||
                    ColumnName == "Link_to_DiversityTaxonNames_of_second_organism" ||
                    ColumnName == "Link_to_DiversityAgents" ||
                    ColumnName == "Depositors_link_to_DiversityAgents" ||
                    ColumnName == "Link_to_DiversityExsiccatae" ||
                    ColumnName == "Link_to_DiversityReferences" ||
                    ColumnName == "Link_to_DiversityReferences_of_second_organism" ||
                    ColumnName == "Link_to_DiversityReferences_for_specimen" ||
                    ColumnName == "Link_to_DiversityAgents_for_responsible" ||
                    ColumnName == "Link_to_DiversityAgents_for_responsible_of_second_organism" ||
                    ColumnName == "Link_to_DiversityAgents_for_determiner" ||
                    ColumnName == "Link_to_DiversityCollection_for_relation" ||
                    ColumnName == "Link_to_DiversityAgents_for_determiner_of_second_organism")
                {
                    return true;
                }
                else return false;
            }
        }

#endregion

#region Printing

        private void buttonGridModePrint_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter sw;
            string GridViewExportFile = Folder.Export() + System.Windows.Forms.Application.ProductName.ToString() + "_GridViewExport_" + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.ToShortTimeString().Replace(":", "") + System.DateTime.Now.Second.ToString() + ".txt";
            if (System.IO.File.Exists(GridViewExportFile))
                sw = new System.IO.StreamWriter(GridViewExportFile, true);
            else
                sw = new System.IO.StreamWriter(GridViewExportFile);
            try
            {
                sw.WriteLine("Export from Gridview");
                sw.WriteLine();
                sw.WriteLine("User:\t" + System.Environment.UserName);
                sw.Write("Date:\t");
                sw.WriteLine(DateTime.Now);
                sw.WriteLine();
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewActive.Columns)
                {
                    if (C.Visible)
                        sw.Write(C.DataPropertyName + "\t");
                }
                sw.WriteLine();
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewActive.Rows)
                {
                    foreach (System.Windows.Forms.DataGridViewCell Cell in R.Cells)
                    {
                        if (this.dataGridView.Columns[Cell.ColumnIndex].Visible)
                        {
                            if (Cell.Value == null)
                                sw.Write("\t");
                            else
                                sw.Write(Cell.Value.ToString() + "\t");
                        }
                    }
                    sw.WriteLine();
                }
                System.Windows.Forms.MessageBox.Show("Data were exported to " + GridViewExportFile);
            }
            catch
            {
            }
            finally
            {
                sw.Close();
            }
        }



#endregion

#region Autosuggest

        private System.Windows.Forms.DataGridView dataGridViewActive
        {
            get
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                    return this.dataGridViewAutosuggest;
                else
                    return this.dataGridView;
            }
        }

        private void checkBoxAutoSuggest_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnWidth = this.getColumnWidths(this.dataGridViewActive);
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence = this.getColumnSequence(this.dataGridViewActive);

            this.setColumnSequence(this.dataGridViewActive);
            this.setColumnWidths(this.dataGridViewActive);

            DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest = !DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.setAutosuggestControls();
        }

        private void setAutosuggestControls()
        {
#if Autosuggest
            System.Collections.Generic.List<System.Drawing.Point> SelectedCells = new List<Point>(); 
            //System.Drawing.Point CurrentPosition = new Point(-1, -1);
            if (!DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
            {
                SelectedCells = DiversityCollection.Forms.FormGridFunctions.SelectedGridPositions(this.dataGridViewAutosuggest);
            }
            else
            {
                SelectedCells = DiversityCollection.Forms.FormGridFunctions.SelectedGridPositions(this.dataGridView);
            }
            this.checkBoxAutoSuggest.Checked = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.splitContainerDataGrid.Panel1Collapsed = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.splitContainerDataGrid.Panel2Collapsed = !DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            if (SelectedCells.Count > 0)
            {
                DiversityCollection.Forms.FormGridFunctions.SetSelectionRange(this.dataGridViewActive, SelectedCells);
            }
            this.buttonResetAutosuggestForAllColumns.Enabled = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.buttonResetAutosuggestForSelectedColumn.Enabled = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
#else
            DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest = false;
            this.checkBoxAutoSuggest.Checked = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.splitContainerDataGrid.Panel1Collapsed = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.splitContainerDataGrid.Panel2Collapsed = !DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.buttonResetAutosuggestForAllColumns.Enabled = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.buttonResetAutosuggestForSelectedColumn.Enabled = DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest;
            this.checkBoxAutoSuggest.Enabled = false;
#endif
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                return;
            try
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                    // getting Table and ColumnName
                    string Table = this.TableNameForAlias(ColumnName);
                    string Column = this.ColumnNameForAlias(ColumnName);
                    System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                    textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

#region AutosuggestGrid

        private void dataGridViewAutosuggest_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewAutosuggest_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Autosuggest)
                return;
            try
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    string ColumnName = this.dataGridViewAutosuggest.Columns[this.dataGridViewAutosuggest.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                    // getting Table and ColumnName
                    string Table = this.TableNameForAlias(ColumnName);
                    string Column = this.ColumnNameForAlias(ColumnName);
                    System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                    textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonResetAutosuggestForAllColumns_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.resetAutoCompleteStringCollectionOnDemand();
        }

        private void buttonResetAutosuggestForSelectedColumn_Click(object sender, EventArgs e)
        {
            string ColumnName = this.dataGridViewAutosuggest.Columns[this.dataGridViewAutosuggest.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
            // getting Table and ColumnName
            string Table = this.TableNameForAlias(ColumnName);
            string Column = this.ColumnNameForAlias(ColumnName);
            DiversityWorkbench.Forms.FormFunctions.resetAutoCompleteStringCollectionOnDemand(Table, Column);
        }

        private void buttonAutosuggestSettingsForColumns_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCustomizeDisplay f = new FormCustomizeDisplay(DiversityCollection.Specimen.ImageList, FormCustomizeDisplay.Customization.Autocomplete);
            f.ShowDialog();
        }

        private void dataGridViewAutosuggest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.buttonGridModeDelete.Enabled = false;
                string ColumnName = this.dataGridViewAutosuggest.Columns[this.dataGridViewAutosuggest.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                this.labelGridViewReplaceColumnName.Text = ColumnName;
                if (this.buttonGridModeFind.Enabled == false)
                    this.buttonGridModeFind.Enabled = true;

                if (this.dataGridViewAutosuggest.SelectedCells.Count > 0)
                    this.enableReplaceButtons(this.dataGridViewAutosuggest, true);
                else this.enableReplaceButtons(this.dataGridViewAutosuggest, false);

                if (e.ColumnIndex == this.dataGridViewAutosuggest.SelectedCells[0].ColumnIndex)
                {
                    if (this.IsLinkColumn)
                    {
                        this.GetRemoteValues(this.dataGridViewAutosuggest.SelectedCells[0]);
                    }
                    else
                    {
                        switch (ColumnName)
                        {
                            case "Link_to_GoogleMaps":
                                this.GetCoordinatesFromGoogleMaps();
                                break;
                            //case "On_loan":
                            case "_TransactionID":
                                int TransactionID;
                                if (int.TryParse(this.dataGridViewAutosuggest.SelectedCells[0].Value.ToString(), out TransactionID))
                                {
                                    DiversityCollection.Forms.FormTransaction f = new FormTransaction(TransactionID);
                                    f.ShowDialog();
                                }
                                else
                                {
                                    string Message = "";
                                    //if (ColumnName == "On_loan") Message = "This dataset is not on loan";
                                    if (ColumnName == "_TransactionID") Message = "This dataset is not involved in a transaction";
                                    if (Message.Length > 0) System.Windows.Forms.MessageBox.Show(Message);
                                }
                                break;
                            case "Remove_link_to_gazetteer":
                            case "Remove_link_to_SamplingPlots":
                            case "Remove_link_for_collector":
                            case "Remove_link_for_Depositor":
                            case "Remove_link_to_exsiccatae":
                            case "Remove_link_of_reference_for_specimen":
                            case "Remove_link_for_identification":
                            case "Remove_link_for_reference":
                            case "Remove_link_for_reference_of_second_organism":
                            case "Remove_link_for_determiner":
                            case "Remove_link_for_responsible_of_second_organism":
                            case "Remove_link_for_second_organism":
                                this.RemoveLink(this.dataGridViewAutosuggest.SelectedCells[0]);
                                break;
                            case "Analysis":
                                this.setAnalysis();
                                break;
                            case "Analysis_result":
                                if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID"].Equals(System.DBNull.Value))
                                    this.setAnalysis();
                                break;
                            case "Relation_is_internal":
                                System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesCollectionSpecimenBindingSource.Current;
                                bool IsInternal = bool.Parse(R["Relation_is_internal"].ToString()); // contains the previous setting i.e. the contrary to the new
                                string test = this.dataGridViewAutosuggest.SelectedCells[0].Value.ToString();
                                if (IsInternal)
                                {
                                    R["Link_to_DiversityCollection_for_relation"] = "";
                                }
                                else
                                {
                                    string URL = R["Related_specimen_URL"].ToString();
                                    if (R["Related_specimen_URL"].ToString().StartsWith("http") && R["Related_specimen_URL"].ToString().ToLower().IndexOf("/collection") > -1)
                                        R["Link_to_DiversityCollection_for_relation"] = R["Related_specimen_URL"];
                                }
                                break;
                        }
                    }
                }
                if (this.textBoxHeaderID.Text.Length == 0)
                    this.setSpecimen(this.SpecimenID);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewAutosuggest_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridViewAutosuggest.SelectedCells.Count > 0 &&
                    this.dataGridViewAutosuggest.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0)
                {
                    string DataProperty = this.dataGridViewAutosuggest.Columns[this.dataGridViewAutosuggest.SelectedCells[0].ColumnIndex].DataPropertyName;
                    if (this.dataGridViewAutosuggest.SelectedCells[0].Value.ToString().Length == 0 && this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.dataGridViewAutosuggest.SelectedCells[0].RowIndex][DataProperty].Equals(System.DBNull.Value))
                    {
                        this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[this.dataGridViewAutosuggest.SelectedCells[0].RowIndex][DataProperty] = this.dataGridViewAutosuggest.SelectedCells[0].EditedFormattedValue.ToString();
                    }

                    this.checkForMissingAndDefaultValues(this.dataGridViewAutosuggest.SelectedCells[0], false);//, this.dataGridViewAutosuggest.SelectedCells[0].EditedFormattedValue.ToString(), this.dataGridViewAutosuggest.);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void dataGridViewAutosuggest_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //try
            //{
            //    string ColumnName = this.dataGridViewAutosuggest.Columns[this.dataGridViewAutosuggest.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
            //    this.labelGridViewReplaceColumnName.Text = ColumnName;
            //    if (this.buttonGridModeFind.Enabled == false)
            //        this.buttonGridModeFind.Enabled = true;
            //    if ((this.dataGridViewAutosuggest.SelectedCells.Count > 0 && ColumnName != "CollectionSpecimenID")
            //        && (typeof(System.Windows.Forms.DataGridViewTextBoxCell) == this.dataGridView.Columns[this.dataGridViewAutosuggest.SelectedCells[0].ColumnIndex].CellType
            //        && ColumnName != "CollectionSpecimenID")
            //        //|| typeof(System.Windows.Forms.DataGridViewComboBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
            //        )
            //        this.enableReplaceButtons(this.dataGridView, true);
            //    else this.enableReplaceButtons(this.dataGridView, false);
            //    if (this.dataGridViewAutosuggest.SelectedCells.Count == 1)
            //        this.labelGridCounter.Text = "line " + (this.dataGridViewAutosuggest.SelectedCells[0].RowIndex + 1).ToString() + " of " + (this.dataGridViewAutosuggest.Rows.Count - 1).ToString();
            //    else if (this.dataGridViewAutosuggest.SelectedCells.Count > 1)
            //    {
            //        int StartLine = this.dataGridView.SelectedCells[0].RowIndex + 1;
            //        if (this.dataGridViewAutosuggest.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1 < StartLine)
            //            StartLine = this.dataGridView.SelectedCells[this.dataGridViewAutosuggest.SelectedCells.Count - 1].RowIndex + 1;
            //        this.labelGridCounter.Text = "line " + StartLine.ToString() + " to " +
            //            (this.dataGridViewAutosuggest.SelectedCells.Count + StartLine - 1).ToString() + " of " +
            //            (this.dataGridViewAutosuggest.Rows.Count - 1).ToString();
            //    }
            //    else
            //        this.labelGridCounter.Text = "line 1 of " + (this.dataGridViewAutosuggest.Rows.Count - 1).ToString();
            //}
            //catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void dataGridViewAutosuggest_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (this.dataGridViewAutosuggest.SortedColumn != null)
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewAutosuggest.Columns)
                    {
                        C.DividerWidth = 0;
                    }
                    this.dataGridViewAutosuggest.Columns[this.dataGridViewAutosuggest.SortedColumn.Index].DividerWidth = 2;
                }

            }
            catch { }

            this.MarkWholeColumn(this.dataGridViewAutosuggest, e.ColumnIndex);

        }

        private void dataGridViewAutosuggest_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void dataGridViewAutosuggest_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                DiversityCollection.Forms.FormGridFunctions.DrawRowNumber(this.dataGridViewAutosuggest, this.dataGridViewAutosuggest.Font, e);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void dataGridViewAutosuggest_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (this.dataGridViewAutosuggest.SortedColumn != null)
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewAutosuggest.Columns)
                    {
                        C.DividerWidth = 0;
                    }
                    this.dataGridViewAutosuggest.Columns[this.dataGridViewAutosuggest.SortedColumn.Index].DividerWidth = 2;
                }

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

            this.MarkWholeColumn(this.dataGridViewAutosuggest, e.ColumnIndex);
        }

        private void dataGridViewAutosuggest_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this._FormState == Forms.FormGridFunctions.FormState.Loading)
                    return;
                int SpecimenID = 0;
                if (this.dataGridViewAutosuggest.SelectedCells.Count > 0 &&
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0 &&
                    this.dataGridViewAutosuggest.SelectedCells[0].RowIndex < this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count &&
                    int.TryParse(this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows[DatasetIndexOfCurrentLine]["CollectionSpecimenID"].ToString(), out SpecimenID))
                {
                    if (SpecimenID != this._SpecimenID)
                    {
                        this.setSpecimen(SpecimenID);
                        this._SpecimenID = SpecimenID;
                    }
                }
                else if (this.dataGridViewAutosuggest.SelectedCells.Count > 0 &&
                    this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count > 0 &&
                    this.dataGridViewAutosuggest.SelectedCells[0].RowIndex >= this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count)
                {
                    this.insertNewDataset(this.dataGridViewAutosuggest.SelectedCells[0].RowIndex);
                }
                else if (this.dataGridViewAutosuggest.SelectedCells.Count == 0
                    && this.dataGridViewAutosuggest.Rows.Count > 0
                    && this.dataGridViewAutosuggest.Rows[0].Cells[0].Value != null)
                {
                    if (int.TryParse(this.dataGridViewAutosuggest.Rows[0].Cells[0].Value.ToString(), out SpecimenID))
                    {
                        this.setSpecimen(SpecimenID);
                        this._SpecimenID = SpecimenID;
                    }
                }
                else if (this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Rows.Count == 0)
                {
                    this.insertNewDataset(0);
                }
                this.setCellBlockings(this.dataGridViewAutosuggest);
                this.setRemoveCellStyle(this.dataGridViewAutosuggest);

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void dataGridViewAutosuggest_KeyUp(object sender, KeyEventArgs e)
        {

        }

        #endregion

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
