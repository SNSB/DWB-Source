using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using DiversityWorkbench.DwbManual;

namespace DiversityCollection.Forms
{
    //public struct AnalysisEntry
    //{
    //    public int AnalysisID;
    //    public string AnalysisType;
    //}

    public partial class FormIdentificationUnitGridMode : Form
    {
        #region Parameter

        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields;
        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeHiddenQueryFields;
        private System.Collections.Generic.List<string> _GridModeColumnList;
        private System.Collections.Generic.Dictionary<string, string> _GridModeTableList;
        private System.Collections.Generic.List<string> _ReadOnlyColumns;

        private System.Collections.Generic.Dictionary<string, string> _ProjectSettings;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private System.Collections.Generic.List<int> _IDs;
        private int _SpecimenID = 0;
        private int _UnitID = 0;
        private int _ProjectID = 0;

        /// <summary>
        /// If the data are restricted to the taxa within one specimen
        /// </summary>
        private bool _UseAsSpecimenTaxonList = false;

        private System.Drawing.Color _ColorOfNotPresentNodes = System.Drawing.Color.LightGray;
        private System.Drawing.Color _ColorOfNodes = System.Drawing.Color.Black;

        private System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> _AnalysisList;
        //private System.Collections.Generic.List<int> _AnalysisIDs;
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _TaxonAnalysisDict;

        private System.Collections.Generic.List<string> _AnalysisReadOnlyColumns;

        private string _TaxonomicGroup = "plant";
        private DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState _ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace;
        private bool _StopReplacing = false;
        private System.Collections.Generic.Dictionary<string, string> _BlockedColumns;
        //private System.Windows.Forms.DataGridViewCellStyle _StyleBlocked;
        //private System.Windows.Forms.DataGridViewCellStyle _StyleUnblocked;
        //private System.Windows.Forms.DataGridViewCellStyle _StyleReadOnly;

        private System.Collections.Generic.Dictionary<string, string> _RemoveColumns;
        //private System.Windows.Forms.DataGridViewCellStyle _StyleRemove;

        private System.Collections.Generic.Dictionary<string, string> _NewIdentificationColumns;
        //private System.Windows.Forms.DataGridViewCellStyle _StyleNewIdentification;

        private enum SaveModes { Single, All };
        private FormIdentificationUnitGridMode.SaveModes _SaveMode = SaveModes.Single;
        private DiversityCollection.Forms.FormGridFunctions.FormState _FormState = Forms.FormGridFunctions.FormState.Loading;

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

        DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageDataTable _CollectionEventSeriesImageDataTable = new Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageDataTable();
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeries;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesUnit;
        private string _sIDs;

        private string _SqlSpecimenFields = "IdentificationUnitID, CollectionSpecimenID, Accession_number, " +
            "Data_withholding_reason, Data_withholding_reason_for_collection_event,Data_withholding_reason_for_collector,  " +
            "Collectors_event_number, Collection_day, Collection_month, Collection_year, Collection_date_supplement, " +
            "Collection_time, Collection_time_span,  " +
            "Country, Locality_description, Habitat_description, Collecting_method, Collection_event_notes, " +
            "Named_area,  NamedAreaLocation2, Remove_link_to_gazetteer, Distance_to_location, Direction_to_location, " +
            "Longitude, Latitude, Coordinates_accuracy, Link_to_GoogleMaps,  " +
            "Altitude_from, Altitude_to, Altitude_accuracy, " +
            "MTB, Quadrant, Notes_for_MTB, " +
            "Sampling_plot, Link_to_SamplingPlots, Remove_link_to_SamplingPlots,  Accuracy_of_sampling_plot, Latitude_of_sampling_plot, Longitude_of_sampling_plot, " +
            "Geographic_region, Lithostratigraphy, Chronostratigraphy,  " +
            "Collectors_name, Link_to_DiversityAgents, Remove_link_for_collector, Collectors_number, Notes_about_collector, " +
            "Accession_day, Accession_month,  Accession_year, Accession_date_supplement, " +
            "Depositors_name, Depositors_link_to_DiversityAgents, Remove_link_for_Depositor, Depositors_accession_number, " +
            "Exsiccata_abbreviation, Link_to_DiversityExsiccatae, Remove_link_to_exsiccatae, Exsiccata_number, " +
            "Original_notes, Additional_notes, Internal_notes, " +
            "Label_title, Label_type, Label_transcription_state, Label_transcription_notes, Problems, " +
            "Taxonomic_group, Relation_type, Colonised_substrate_part, Related_organism, Life_stage, Gender, Number_of_units, Circumstances, Order_of_taxon, Family_of_taxon,  " +
            "Identifier_of_organism, Description_of_organism, Only_observed, Notes_for_organism, " +
            "Taxonomic_name, Link_to_DiversityTaxonNames,  " +
            "Remove_link_for_identification, Vernacular_term, Identification_day, Identification_month, Identification_year, Identification_category, Identification_qualifier, Type_status, " +
            "Type_notes, Notes_for_identification, Determiner, " + //Reference_title, Link_to_DiversityReferences, Remove_link_for_reference, 
            "Link_to_DiversityAgents_for_determiner, Remove_link_for_determiner, " +
            "AnalysisID_0, Analysis_0, Analysis_number_0, Analysis_result_0, " +
            "AnalysisID_1, Analysis_1, Analysis_number_1, Analysis_result_1, " +
            "AnalysisID_2, Analysis_2, Analysis_number_2, Analysis_result_2, " +
            "AnalysisID_3, Analysis_3, Analysis_number_3, Analysis_result_3, " +
            "AnalysisID_4, Analysis_4, Analysis_number_4, Analysis_result_4, " +
            "AnalysisID_5, Analysis_5, Analysis_number_5, Analysis_result_5, " +
            "AnalysisID_6, Analysis_6, Analysis_number_6, Analysis_result_6, " +
            "AnalysisID_7, Analysis_7, Analysis_number_7, Analysis_result_7, " +
            "AnalysisID_8, Analysis_8, Analysis_number_8, Analysis_result_8, " +
            "AnalysisID_9, Analysis_9, Analysis_number_9, Analysis_result_9, " +
            "Collection, Material_category, Storage_location, Stock, Preparation_method, Preparation_date, Notes_for_part, " +
            "_TransactionID, _Transaction, On_loan, _CollectionEventID, _IdentificationUnitID, _IdentificationSequence,  " +
            "_SpecimenPartID, _CoordinatesAverageLatitudeCache, _CoordinatesAverageLongitudeCache, _CoordinatesLocationNotes, " +
            "_GeographicRegionPropertyURI, _LithostratigraphyPropertyURI, _ChronostratigraphyPropertyURI, " +
            "_NamedAverageLatitudeCache, _NamedAverageLongitudeCache, _LithostratigraphyPropertyHierarchyCache, " +
            "_ChronostratigraphyPropertyHierarchyCache, _AverageAltitudeCache ";
        #endregion

        private string _SourceFunction;
        private string SourceFunction
        {
            get
            {
                if (this._SourceFunction == null || this._SourceFunction.Length == 0)
                {
                    this._SourceFunction = "FirstLinesUnit_4";
                    string SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_NAME = '" + this._SourceFunction + "'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result != "1")
                        this._SourceFunction = "FirstLinesUnit_3";
                }
                return this._SourceFunction;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// #122 - benötigt um die Form zu erstellen
        /// </summary>
        private FormIdentificationUnitGridMode()
        {
            InitializeComponent();
        }


        public FormIdentificationUnitGridMode(System.Collections.Generic.List<int> IDs, string FormTitle, int ProjectID) : this() //#122
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please connect to a database");
                this.Close();
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this._FormState = Forms.FormGridFunctions.FormState.Loading;
            //this.setCulture();
            this._ProjectID = ProjectID;
            this._IDs = IDs;
            for (int i = 0; i < _IDs.Count; i++)
            {
                if (i > 0) _sIDs += ", ";
                this._sIDs += _IDs[i].ToString();
            }
            //this.SetAnalysisIDs();
            if (FormTitle.Length > 0) this.Text = FormTitle;
            this.initForm();
            if (IDs.Count > 0)
                this._SpecimenID = IDs[0];
            this.userControlImageSpecimenImage.MaxSizeOfImageVisible = true;
            this.GridModeFillTable();
            this.GridModeSetColumnVisibility();
            this.setRemoveCellStyle();
            //this.setNewIdentificationCellStyle();
            this.setIconsIntreeViewData();
            this.setTitleIntreeViewData();
            this.dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this._FormState = Forms.FormGridFunctions.FormState.Editing;
            DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityCollection.Forms.FormCollectionSpecimenGridModeText));
            DiversityWorkbench.Entity.setEntity(this, resources);
        }

        public FormIdentificationUnitGridMode(int ID, string FormTitle, int ProjectID) : this() // #122
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please connect to a database");
                this.Close();
            }
            try
            {
                this._UseAsSpecimenTaxonList = true;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this._FormState = Forms.FormGridFunctions.FormState.Loading;
                InitializeComponent();
                //this.setCulture();
                this._ProjectID = ProjectID;
                this._IDs = new List<int>();
                this._IDs.Add(ID);
                this._sIDs = ID.ToString();
                // #35
                this.helpProvider.SetHelpKeyword(this, "gridspecimenunits_dc");
                //this.SetAnalysisIDs();
                if (FormTitle.Length > 0) this.Text = FormTitle;
                this.initForm();
                this._SpecimenID = ID;
                this.userControlImageSpecimenImage.MaxSizeOfImageVisible = true;
                this.GridModeFillTable();
                this.GridModeSetColumnVisibility();
                this.setRemoveCellStyle();
                //this.setNewIdentificationCellStyle();
                this.setIconsIntreeViewData();
                this.setTitleIntreeViewData();
                this.dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                DiversityWorkbench.Entity.setEntity(this, this.toolTip);
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityCollection.Forms.FormCollectionSpecimenGridModeText));
                DiversityWorkbench.Entity.setEntity(this, resources);

                this.buttonHeaderShowSelectionTree.BackColor = System.Drawing.Color.Red;// System.Drawing.SystemColors.Control;
                this.buttonHeaderShowSelectionTree_Click(null, null);

                // fuehrt bei ShowForm() zu Fehler: {"InvalidArgument=Value of '3' is not valid for 'rowIndex'."} 
                // Ursache unklar
                //this.dataGridView.AllowUserToAddRows = true;

                this._FormState = Forms.FormGridFunctions.FormState.Editing;
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Form

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
                string TableName = "IdentificationUnit";
                bool OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                if (!OK)
                {
                    this.dataGridView.ReadOnly = true;
                    this.dataGridView.AllowUserToAddRows = false;
                    this.dataGridView.AllowUserToDeleteRows = false;
                    this.tableLayoutPanelGridModeParameter.Visible = false;
                }

                this.AcceptButton = this.userControlDialogPanel.buttonOK;
                this.CancelButton = this.userControlDialogPanel.buttonCancel;
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                this.textBoxGridModeReplace.Width = 80;
                this.textBoxGridModeReplaceWith.Width = 80;
                this.comboBoxReplace.Width = 100;
                this.comboBoxReplaceWith.Width = 100;
                this.FillPicklists();
                this.setColumnsAndNodesCorrespondingToPermissions();
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility.Length > 0)
                    this.GridModeSetFieldVisibilityForNodes();
                this.GridModeSetToolTipsInTree();
                this.setImageVisibility(DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.HeaderDisplay);
                this.userControlImageSpecimenImage.ImagePath = "";
                this.setGridColumnHeaders();
                this.enableReplaceButtons(false);
                this.userControlDialogPanel.buttonOK.Click += new EventHandler(CheckForChangesAndAskForSaving);
                foreach(System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
                    this.SetToolTipsIntreeViewData(N);
                this.setColumnSequence();
                this.setColumnWidths();
                //this.setRemoveCellStyle();
                this.setOptions(null);
                this.buttonHeaderShowTree.BackColor = System.Drawing.SystemColors.Control;
                this.setVisibility();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonSearchSpecimen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FillPicklists()
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSpecimenImageType, "CollSpecimenImageType_Enum", con, true, true, true);

            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollCircumstances_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollIdentificationCategory_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollIdentificationQualifier_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollMaterialCategory_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollTaxonomicGroup_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollTypeStatus_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollUnitRelationType_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollLabelTranscriptionState_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetIdentifictionUnitGridMode.CollLabelType_Enum, false);

            string SQL = "SELECT CollectionID, CollectionName " +
                "FROM Collection ORDER BY CollectionName ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(this.dataSetIdentifictionUnitGridMode.Collection);
            }
            catch  {} 
            ad.SelectCommand.CommandText = "";
            try
            {
                ad.SelectCommand.CommandText = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                ad.Fill(this.dataSetIdentifictionUnitGridMode.ProjectList);
            }
            catch  {} 

            ad.SelectCommand.CommandText = "SELECT NameEn FROM [DiversityGazetteer].[dbo].[GeoCountries] " +
                "WHERE (PlaceID > 1) " +
                "UNION " +
                "SELECT DISTINCT CountryCache AS NameEn FROM CollectionEvent " +
                "ORDER BY NameEn";
            try
            {
                ad.Fill(this.dataSetIdentifictionUnitGridMode.GeoCountries);
            }
            catch { }

            if (this.dataSetIdentifictionUnitGridMode.GeoCountries.Rows.Count == 0)
            {
                ad.SelectCommand.CommandText = "SELECT DISTINCT CountryCache AS NameEn " +
                    "FROM CollectionEvent ORDER BY NameEn";
                try
                {
                    ad.Fill(this.dataSetIdentifictionUnitGridMode.GeoCountries);
                }
                catch { }
            }

            //this.setAnalysisSource();

            if (this.dataSetIdentifictionUnitGridMode.Analysis.Rows.Count == 0)
            {
                ad.SelectCommand.CommandText = "SELECT P.AnalysisID, P.DisplayText " +
                    "+ CASE WHEN A.DisplayText IS NULL THEN '' ELSE ' / ' + A.DisplayText END " +
                    "+ CASE WHEN P.Description IS NULL OR P.Description = '' THEN '' ELSE ': ' + P.Description END " +
                    "AS DisplayText " +
                    "FROM dbo.AnalysisProjectList(" + this._ProjectID.ToString() + ") AS P LEFT OUTER JOIN " +
                    "Analysis AS A ON A.AnalysisID = P.AnalysisParentID " +
                    "WHERE (P.OnlyHierarchy = 0)";
                try
                {
                    ad.Fill(this.dataSetIdentifictionUnitGridMode.Analysis);
                }
                catch { }
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Feedback.SendFeedback(
                    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                    "",
                    this.SpecimenID.ToString());
            }
            catch { }
        }

        private void FormIdentificationUnitGridMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.CheckForChangesAndAskForSaving(null, null);
                string ColumnWidths = "";
                string ColumnPositions = "";
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                {
                    ColumnWidths += C.Width + " ";
                    ColumnPositions += C.DisplayIndex.ToString() + " ";
                }
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitColumnWidth = ColumnWidths;
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitColumnSequence = ColumnPositions;
                DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.HeaderDisplay = this.CurrentHeaderDisplaySettings;
                DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.Save();
            }
            catch { } 

        }

        private string Message(string Resource)
        {
            string Message = "";
            try
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityCollection.Forms.FormCollectionSpecimenGridModeText));
                Message = resources.GetString(Resource);

            }
            catch { } 
            return Message;
        }

        private void FormIdentificationUnitGridMode_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.Analysis". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.analysisTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.Analysis);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.CollTypeStatus_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collTypeStatus_EnumTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.CollTypeStatus_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.CollIdentificationQualifier_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collIdentificationQualifier_EnumTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.CollIdentificationQualifier_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.CollIdentificationCategory_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collIdentificationCategory_EnumTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.CollIdentificationCategory_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.CollCircumstances_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collCircumstances_EnumTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.CollCircumstances_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.CollUnitRelationType_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collUnitRelationType_EnumTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.CollUnitRelationType_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.CollTaxonomicGroup_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collTaxonomicGroup_EnumTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.CollTaxonomicGroup_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetIdentifictionUnitGridMode.FirstLinesUnit". Sie können sie bei Bedarf verschieben oder entfernen.
            ////this.firstLinesUnitTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit);
        }

        #endregion

        #region Options

        private void buttonSetOptions_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Forms.FormIdentificationUnitGridModeSetSettings f = new DiversityCollection.Forms.FormIdentificationUnitGridModeSetSettings();
                f.ProjectID = this._ProjectID;
                f.ShowDialog();
                this.setOptions(f);
            }
            catch (System.Exception ex) { }
        }

        private void setOptions(DiversityCollection.Forms.FormIdentificationUnitGridModeSetSettings f)
        {
            this.textBoxOptions.Text = "";
            this.textBoxOptions.Height = 0;
            if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisStartDate)
            {
                this.textBoxOptions.Text += "Start date for analysis: " + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.ToShortDateString() + "\r\n";
                textBoxOptions.Height += 8;
            }
            if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisEndDate)
            {
                this.textBoxOptions.Text += "End date for analysis: " + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.ToShortDateString() + "\r\n";
                textBoxOptions.Height += 8;
            }
            if (f == null)
                f = new DiversityCollection.Forms.FormIdentificationUnitGridModeSetSettings();
            if (this.AnalysisList.Count > 0) //DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisIDs)
            {
                textBoxOptions.Height += 16;
                this.textBoxOptions.Text += "Types of analysis: ";
                foreach (System.Data.DataRow R in f.DtAnalysis.Rows)
                {
                    if (R.RowState != DataRowState.Deleted)
                        this.textBoxOptions.Text += R[0].ToString() + "; ";
                }
            }
            this._AnalysisList = null;
            //this.SetAnalysisIDs();
            this.setTitleIntreeViewData();
            this.setGridColumnHeaders();
            this.GridModeFillTable();
        }

        #endregion

        #region treeViewColumnSelector

        private void setIconsIntreeViewData()
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
                            this.setIconsIntreeViewData(N);
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
                            this.setIconsIntreeViewData(N);
                            break;
                        case "NodeSpecimen":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Specimen, false);
                            this.setIconsIntreeViewData(N);
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
                            this.setIconsIntreeViewData(N);
                            break;
                        case "NodeStorage":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Storage, false);
                            break;
                        case "NodeTransaction":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Transaction, false);
                            break;
                    }
                    N.ImageIndex = iIndex;
                    N.SelectedImageIndex = iIndex;
                }

            }
            catch { }
        }

        private void setIconsIntreeViewData(System.Windows.Forms.TreeNode TreeNode)
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

        private void setTitleIntreeViewData()
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
            {
                try
                {
                    Forms.GridModeQueryField Q = this.GridModeQueryFieldOfNode(N);
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
                        this.setTitleIntreeViewData(childN);

                }
                catch { }
            }
        }

        private void setTitleIntreeViewData(System.Windows.Forms.TreeNode TreeNode)
        {
            try
            {
                Forms.GridModeQueryField Q = this.GridModeQueryFieldOfNode(TreeNode);
                System.Collections.Generic.Dictionary<string, string> EntityInfo = new Dictionary<string, string>();
                if (Q.Entity != null && Q.Entity.Length > 0 
                    && (Q.Restriction == null || Q.Restriction.Length == 0)
                    && !Q.AliasForColumn.StartsWith("Link_to")
                    && !Q.AliasForColumn.StartsWith("Remove_link"))
                {
                    if (TreeNode.Text == "Link to Transaction")
                    {
                        switch (DiversityWorkbench.Settings.Language)
                        {
                            case "de-DE":
                                TreeNode.Text = "Verbindung zu Transaktion";
                                break;
                        }
                    }
                    else
                    {
                        if (Q.Table == "IdentificationUnitAnalysis")
                        {
                            string NodeTitle = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                            switch (Q.AliasForTable)
                            {
                                case "Analysis_0":
                                    if (this.AnalysisList.Count > 0)
                                        TreeNode.Text = this.AnalysisList[0].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_1":
                                    if (this.AnalysisList.Count > 1)
                                        TreeNode.Text = this.AnalysisList[1].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_2":
                                    if (this.AnalysisList.Count > 2)
                                        TreeNode.Text = this.AnalysisList[2].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_3":
                                    if (this.AnalysisList.Count > 3)
                                        TreeNode.Text = this.AnalysisList[3].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_4":
                                    if (this.AnalysisList.Count > 4)
                                        TreeNode.Text = this.AnalysisList[4].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_5":
                                    if (this.AnalysisList.Count > 5)
                                        TreeNode.Text = this.AnalysisList[5].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_6":
                                    if (this.AnalysisList.Count > 6)
                                        TreeNode.Text = this.AnalysisList[6].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_7":
                                    if (this.AnalysisList.Count > 7)
                                        TreeNode.Text = this.AnalysisList[7].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_8":
                                    if (this.AnalysisList.Count > 8)
                                        TreeNode.Text = this.AnalysisList[8].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                                case "Analysis_9":
                                    if (this.AnalysisList.Count > 9)
                                        TreeNode.Text = this.AnalysisList[9].AnalysisType;
                                    else TreeNode.Text = NodeTitle;
                                    //else
                                    //{
                                    //    TreeNode.Checked = false;
                                    //    TreeNode.Remove();
                                    //}
                                    break;
                            }
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
                            switch (DiversityWorkbench.Settings.Language)
                            {
                                case "de-DE":
                                    Title = Title.Replace("Link to", "Verbindung zu");
                                    break;
                            }
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
                            switch (DiversityWorkbench.Settings.Language)
                            {
                                case "de-DE":
                                    Title = Title.Replace("Remove link for", "Lösche Verbindung zu");
                                    break;
                            }
                            //if (Title.EndsWith("collector"))
                            //{
                            //    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionAgent");
                            //    Title = Title.Replace("collector", EntityInfo["DisplayText"]);
                            //}
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
                                case "Result":
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
                    this.setTitleIntreeViewData(childN);
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

        private void treeViewDataGridModeFieldSelector_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if (e.Node.Checked)
            //{
            //}
        }

        #region Field visibility, handling of the tree

        private void SetToolTipsIntreeViewData(System.Windows.Forms.TreeNode Node)
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
                                case "_Transaction":
                                    Description = "The first transaction in which a part of the specimen is involved";
                                    break;
                                case "On_loan":
                                    Description = "The first part of the specimen that is on loan";
                                    break;
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
                            this.setGridColumnToolTips(Description, AliasForColumn);
                        }
                    }
                    this.SetToolTipsIntreeViewData(N);
                }

            }
            catch { }
        }

        private void treeViewDataGridModeFieldSelector_AfterCheck(object sender, TreeViewEventArgs e)
        {
            System.Windows.Forms.TreeNode N = e.Node; // this.treeViewDataGridModeFieldSelector.SelectedNode;
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
                // The nodes for the analysis can only be checked if there is an analysis available
                if (N.Checked && N.Tag != null && N.Tag.ToString().StartsWith("IdentificationUnitAnalysis"))
                {
                    string[] TagContent = N.Tag.ToString().Split(new char[] { ';' });
                    string Analysis = TagContent[1];
                    int iAnalysis = int.Parse(Analysis.Substring(Analysis.Length - 1));
                    if (this.AnalysisList.Count < iAnalysis + 1)
                        N.Checked = false;
                }

            }
            catch { }
        }

        private void GridModeSetFieldVisibilityForNodes()
        {
            try
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility.Length > 0 
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
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility = Visibility;
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                }

            }
            catch { }
        }

        #endregion

        #endregion

        #region Permissions and related functions

        private void setColumnsAndNodesCorrespondingToPermissions()
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
                this.dataGridView.ReadOnly = !OK;

                // Check INSERT
                OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                this.buttonGridModeCopy.Enabled = OK;

                // Check DELETE
                OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                this.buttonGridModeDelete.Visible = OK;

                // setting the columns if GridView is not ReadOnly
                if (!this.dataGridView.ReadOnly)
                {
                    // CollectionSpecimenTransaction
                    TableName = "CollectionSpecimenTransaction";
                    // Check UPDATE
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.setGridColumnAccordingToPermission(TableName, OK);

                    // CollectionEventLocalisation
                    // Check UPDATE
                    //TableName = "CollectionEventLocalisation";
                    //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    //this.setGridColumnAccordingToPermission("", "Named_area", TableName, OK);

                    // CollectionEventProperty
                    // Check UPDATE
                    //TableName = "CollectionEventProperty";
                    //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    //this.setGridColumnAccordingToPermission("", "Geographic_region", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Lithostratigraphy", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Chronostratigraphy", TableName, OK);

                    // CollectionSpecimen
                    // Check UPDATE
                    //TableName = "CollectionSpecimen";
                    //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    //this.setGridColumnAccordingToPermission("", "Link_to_DiversityExsiccatae", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Depositors_link_to_DiversityAgents", TableName, OK);

                    // CollectionAgent
                    // Check UPDATE
                    //TableName = "CollectionAgent";
                    //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    //this.setGridColumnAccordingToPermission("", "Link_to_DiversityAgents", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Depositors_link_to_DiversityAgents", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Link_to_DiversityAgents_for_responsible", TableName, OK);

                    // Identification
                    // Check UPDATE
                    //TableName = "Identification";
                    //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    //this.setGridColumnAccordingToPermission("", "Link_to_DiversityTaxonNames", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Link_to_DiversityTaxonNames_of_second_organism", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Link_to_DiversityReferences", TableName, OK);
                    //this.setGridColumnAccordingToPermission("", "Link_to_DiversityAgents_for_responsible", TableName, OK);
                }


                //// CollectionEvent
                //// Check INSERT
                //TableName = "CollectionEvent";
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.groupBoxEvent, OK);
                //this.toolStripButtonOverviewHierarchyAssignEventSeries.Enabled = OK;
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonExpeditionDelete.Enabled = OK;

                //// CollectionEventSeries
                //// Check INSERT
                //TableName = "CollectionEventSeries";
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripButtonOverviewHierarchyNewEventSeries.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.groupBoxEventSeries, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonExpeditionDelete.Enabled = OK;

                //// CollectionExpeditionImage
                //// Check INSERT
                //TableName = "CollectionEventImage";
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripButtonEventImageNew.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelEventImages, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                //this.toolStripButtonEventImageDelete.Enabled = OK;

                //// CollectionGeography
                //// Check INSERT
                //TableName = "CollectionEventLocalisation";
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripDropDownButtonOverviewHierarchyNewEventLocalisation.Enabled = OK;
                ////this.toolStripButtonGeographyNew.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.groupBoxEventLocalisation, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonGeographyDelete.Enabled = OK;

                //// CollectionHabitat
                //TableName = "CollectionEventProperty";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripDropDownButtonOverviewHierarchyNewEventProperty.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.groupBoxEventProperty, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonHabitatDelete.Enabled = OK;

                //// CollectionSpecimen
                //TableName = "CollectionSpecimen";
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelExsiccata, OK);
                //this.FormFunctions.setDataControlEnabled(this.groupBoxAccession, OK);
                //this.FormFunctions.setDataControlEnabled(this.groupBoxNotes, OK);
                //this.FormFunctions.setDataControlEnabled(this.groupBoxProjects, OK);
                //this.FormFunctions.setDataControlEnabled(this.groupBoxDisplayOrder, OK);
                //if (this.scanModeToolStripMenuItem.Checked)
                //{
                //    this.buttonSave.Enabled = OK;
                //    this.toolStripButtonSave.Enabled = OK;
                //}
                //else this.buttonSave.Visible = false;
                ////this.FormFunctions.setDataControlEnabled(this.groupBoxSpecimenReference, OK);
                //this.comboBoxHeaderDataWithholdingReason.Enabled = OK;
                //this.FormFunctions.setDataControlEnabled(this.groupBoxLabel, OK);
                //this.buttonFindNextAccessionNumber.Enabled = OK;

                //TableName = "CollectionSpecimen_log";
                //// Check SELECT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "SELECT");
                //this.buttonHeaderHistory.Enabled = OK;

                //// CollectionSpecimenImage
                //TableName = "CollectionSpecimenImage";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripButtonSpecimenImageNew.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelSpecimenImage, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                //this.toolStripButtonSpecimenImageDelete.Enabled = OK;

                //// CollectionSpecimenPart
                //TableName = "CollectionSpecimenPart";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                ////this.toolStripButtonStorageNew.Enabled = OK;
                //this.PermissionStorageInsert = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.groupBoxPart, OK);
                //this.PermissionStorageUpdate = OK;
                //this.FormFunctions.setDataControlEnabled(this.groupBoxDisplayOrderPart, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonStorageDelete.Enabled = OK;
                //this.PermissionStorageDelete = OK;

                //// CollectionSpecimenProcessing
                //TableName = "CollectionSpecimenProcessing";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripDropDownButtonOverviewHierarchyNewProcessing.Enabled = OK;
                //this.PermissionProcessingInsert = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.groupBoxProcessing, OK);
                //this.PermissionProcessingUpdate = OK;
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonProcessingDelete.Enabled = OK;
                //this.PermissionStorageDelete = OK;

                //// CollectionSpecimenRelation
                //TableName = "CollectionSpecimenRelation";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripButtonOverviewHierarchyNewRelation.Enabled = OK;
                //this.toolStripButtonOverviewHierarchyNewRelationExtern.Enabled = OK;
                ////this.toolStripButtonRelationNew.Enabled = OK;
                ////this.toolStripButtonRelationNewInternal.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.groupBoxSpecimenRelation, OK);
                //this.buttonRelatedSpecimenURL.Enabled = OK;
                ////this.buttonExchangeOpen.Enabled = OK;
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonRelationDelete.Enabled = OK;

                //// CollectionAgent
                //TableName = "CollectionAgent";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripButtonOverviewHierarchyNewAgent.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.toolStripButtonOverviewHierarchyAgentDown.Enabled = OK;
                //this.toolStripButtonOverviewHierarchyAgentUp.Enabled = OK;
                //this.FormFunctions.setDataControlEnabled(this.groupBoxCollector, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonCollectorDelete.Enabled = OK;

                //// CollectionProject
                //TableName = "CollectionProject";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripButtonProjectNew.Enabled = OK;
                //this.toolStripButtonProjectOpen.Enabled = OK;
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                //this.toolStripButtonProjectDelete.Enabled = OK;

                //// IdentificationUnit
                //TableName = "IdentificationUnit";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripDropDownButtonOverviewHierarchyNewUnit.Enabled = OK;
                ////this.panelCustomPresetSubstrate.Enabled = OK;
                ////this.panelCustomPresetMainOrganism.Enabled = OK;
                ////this.panelCustomPresetMainOrganismIdentification.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelIdentificationUnit, OK);
                //this.FormFunctions.setDataControlEnabled(this.groupBoxUnit, OK);
                ////this.FormFunctions.setDataControlEnabled(this.tabPageSubstrate, OK);
                //this.toolStripDisplayOrder.Enabled = OK;
                //this.toolStripDisplay.Enabled = OK;
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonUnitDelete.Enabled = OK;
                ////this.buttonShowSynonym.Enabled = OK;

                //// IdentificationUnitAnalysis
                //TableName = "IdentificationUnitAnalysis";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripDropDownButtonOverviewHierarchyNewAnalysis.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelAnalysis, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonAnalysisDelete.Enabled = OK;
                ////this.buttonShowSynonym.Enabled = OK;

                //// Identification
                //TableName = "Identification";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripButtonOverviewHierarchyNewConfirmation.Enabled = OK;
                //this.toolStripButtonOverviewHierarchyNewIdentAtBase.Enabled = OK;
                //this.toolStripButtonOverviewHierarchyNewIdentification.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelIdentification, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonIdentificationDelete.Enabled = OK;

                //// IdentificationUnitInPart
                //TableName = "IdentificationUnitInPart";
                //// Check INSERT
                //OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                //this.toolStripDropDownButtonOverviewHierarchyNewAnalysis.Enabled = OK;
                //// Check UPDATE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                //this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelDisplayOrderPart, OK);
                //// Check DELETE
                //OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                ////this.toolStripButtonAnalysisDelete.Enabled = OK;
                ////this.buttonShowSynonym.Enabled = OK;
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
        private void setGridColumnAccordingToPermission(string TableName, bool HasPermission)
        {
            System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> FieldList = new List<Forms.GridModeQueryField>();
            try
            {
                foreach (DiversityCollection.Forms.GridModeQueryField G in this.GridModeQueryFields)
                {
                    if (G.Table == TableName && !FieldList.Contains(G))
                        FieldList.Add(G);
                }
                foreach (System.Windows.Forms.DataGridViewColumn Col in this.dataGridView.Columns)
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
        private void setGridColumnAccordingToPermission(string ColumnName, string AliasForColumn, string TableName, bool HasPermission)
        {
            bool HasAccessToModule = false;
            try
            {
                switch (AliasForColumn)
                {
                    case "Named_area":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityGazetteer");
                        //this.treeViewDataGridModeFieldSelector.Nodes["NodeNamedArea"].Checked = false;
                        if (!HasAccessToModule)
                            this.RemoveNodeFromTree(AliasForColumn, null);
                        //this.RemoveNodeFromTree("NodeNamedArea", null);
                        break;
                    case "Geographic_region":
                    case "Lithostratigraphy":
                    case "Chronostratigraphy":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms");
                        if (!HasAccessToModule)
                        {
                            this.RemoveNodeFromTree(AliasForColumn, null);
                            //this.RemoveNodeFromTree("NodeLithostratigraphy", null);
                            //this.RemoveNodeFromTree("NodeChronostratigraphy", null);
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
                    foreach (System.Windows.Forms.DataGridViewColumn Col in this.dataGridView.Columns)
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

        private bool setSpecimen(int SpecimenID, int UnitID)
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
                this.fillSpecimen(SpecimenID, UnitID);
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

        //private bool setSpecimen(string AccessionNumber)
        //{
        //    bool OK = false;
        //    string SQL = "SELECT CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
        //        "FROM CollectionSpecimen INNER JOIN " +
        //        "CollectionSpecimenID_UserAvailable ON  " +
        //        "CollectionSpecimen.CollectionSpecimenID = CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
        //        "WHERE (CollectionSpecimen.AccessionNumber = N'" + AccessionNumber + "')";
        //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //    Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //    try
        //    {
        //        con.Open();
        //        int ID;
        //        OK = int.TryParse(C.ExecuteScalar()?.ToString(), out ID);
        //        if (!OK)
        //        {
        //            System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Entity.Message("The_entry") + " " + AccessionNumber + " " + DiversityWorkbench.Entity.Message("could_not_be_found_in_the_database"));
        //            return OK;
        //        }
        //        //int IDv = int.Parse(C.ExecuteScalar()?.ToString());
        //        con.Close();
        //        OK = this.setSpecimen(ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

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
                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count == 1)
                {
                    string Title = this.labelHeaderAccessionNumber.Text;
                    if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value) ||
                        this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString().Length == 0)
                    {
                        Title = "ID " + this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString();
                        if (!Title.StartsWith("Edit organisms of "))
                            Title = "Edit organisms of " + Title;
                        this.textBoxHeaderAccessionNumber.Text = "";
                    }
                    else 
                        Title = "Edit organisms of Acc.Nr.:";
                    this.labelHeaderAccessionNumber.Text = Title;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillSpecimen(int SpecimenID, int UnitID)
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
                string WhereClauseUnit = " AND IdentificationUnitID = " + UnitID.ToString();

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

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnit, DiversityCollection.CollectionSpecimen.SqlUnit + WhereClause + WhereClauseUnit, this.dataSetCollectionSpecimen.IdentificationUnit);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, DiversityCollection.CollectionSpecimen.SqlIdentification + WhereClause + WhereClauseUnit + " ORDER BY IdentificationSequence", this.dataSetCollectionSpecimen.Identification);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAnalysis, DiversityCollection.CollectionSpecimen.SqlAnalysis + WhereClause + WhereClauseUnit, this.dataSetCollectionSpecimen.IdentificationUnitAnalysis);

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
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimen", this.sqlDataAdapterSpecimen, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this.collectionSpecimenImageBindingSource);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionAgent", this.sqlDataAdapterAgent, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionProject", this.sqlDataAdapterProject, this.BindingContext);

                // if datasets of this table were deleted, this must happen before deleting the parent tables
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenPart", this.sqlDataAdapterPart, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnit", this.sqlDataAdapterUnit, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Identification", this.sqlDataAdapterIdentification, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitAnalysis", this.sqlDataAdapterAnalysis, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEvent", this.sqlDataAdapterEvent, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventImage", this.sqlDataAdapterEventImage, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventLocalisation", this.sqlDataAdapterLocalisation, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventProperty", this.sqlDataAdapterProperty, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Collection", this.sqlDataAdapterCollection, this.BindingContext);
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

        #region Analysis

        public System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> AnalysisList
        {
            get 
            {
                if (this._AnalysisList == null)
                {
                    this._AnalysisList = new List<DiversityCollection.Forms.AnalysisEntry>();

                    try
                    {
                        string SQL = "";
                        string AnalysisIDs = "";
                        if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs != null)
                        {
                            foreach (string s in DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs)
                            {
                                if (AnalysisIDs.Length > 0) AnalysisIDs += ",";
                                AnalysisIDs += s;
                            }
                        }
                        if (AnalysisIDs.Length > 0)
                        {
                            SQL = "SELECT DISTINCT TOP 10 A.DisplayText, A.AnalysisID " +
                            "FROM dbo.Analysis AS A " +
                            "WHERE A.AnalysisID IN (" + AnalysisIDs + ") " +
                            "ORDER BY A.DisplayText";
                        }
                        if (SQL.Length > 0)
                        {
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter();
                            System.Data.DataTable dt = new DataTable();
                            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref ad, dt, SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(4000));
                            this._AnalysisList.Clear();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DiversityCollection.Forms.AnalysisEntry A = new DiversityCollection.Forms.AnalysisEntry();
                                A.AnalysisID = int.Parse(dt.Rows[i]["AnalysisID"].ToString());
                                A.AnalysisType = dt.Rows[i]["DisplayText"].ToString();
                                this._AnalysisList.Add(A);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    { }
                }
                return _AnalysisList; 
            }
        }
        
        /// <summary>
        /// filling the table Analysis in the dataset
        /// </summary>
        private void setAnalysis()
        {
            int? ID = this.AnalysisIDsforCurrentDataset();
            if (ID == null) return;
            else
            {
                try
                {
                    if (this.dataSetIdentifictionUnitGridMode.Analysis.Rows.Count == 0)
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
                            ad.Fill(this.dataSetIdentifictionUnitGridMode.Analysis);
                        }
                        catch { }
                    }
                    int AnalysisID = (int)ID;
                    string Analysis = "";
                    System.Data.DataRow[] RR = this.dataSetIdentifictionUnitGridMode.Analysis.Select("AnalysisID = " + ID.ToString());
                    if (RR.Length == 0) return;
                    Analysis = RR[0]["DisplayText"].ToString();
                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID"] = AnalysisID;
                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["Analysis"] = Analysis;
                    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["Analysis_number"].Equals(System.DBNull.Value))
                        this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["Analysis_number"] = "1";
                }
                catch { }
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

            if (!int.TryParse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["IdentificationUnitID"].ToString(), out UnitID))
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
            catch { }
            return null;
        }

        ///// <summary>
        ///// setting the analysis IDs that are used for the current columns in the grid
        ///// </summary>
        //private void SetAnalysisIDs()
        //{
        //    string SQL = "SELECT DISTINCT TOP 10 A.DisplayText, A.AnalysisID " +
        //        "FROM dbo.IdentificationUnitAnalysis AS U, dbo.Analysis AS A " +
        //        "WHERE U.CollectionSpecimenID IN (" + this.IDlist + ") " +
        //        "AND U.AnalysisID = A.AnalysisID " +
        //        "ORDER BY A.AnalysisID";
        //    string AnalysisIDs = "";
        //    foreach (string s in DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs)
        //    {
        //        if (AnalysisIDs.Length > 0) AnalysisIDs += ",";
        //        AnalysisIDs += s;
        //    }
        //    if (AnalysisIDs.Length > 0)
        //    {
        //        SQL = "SELECT DISTINCT TOP 10 A.DisplayText, A.AnalysisID " +
        //            "FROM dbo.Analysis AS A " +
        //            "WHERE A.AnalysisID IN (" + AnalysisIDs + ") " +
        //            "ORDER BY A.AnalysisID";
        //    }
        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter();
        //    System.Data.DataTable dt = new DataTable();
        //    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref ad, dt, SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(4000));
        //    this._AnalysisList.Clear();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        AnalysisEntry A = new AnalysisEntry();
        //        A.AnalysisID = int.Parse(dt.Rows[i]["AnalysisID"].ToString());
        //        A.AnalysisType = dt.Rows[i]["DisplayText"].ToString();
        //        this._AnalysisList.Add(A);
        //    }
        //}

        /// <summary>
        /// setting the analysis IDs that are used for the current columns in the grid
        /// </summary>
        //private void SetAnalysisList()
        //{
        //    string SQL = "";
        //    string AnalysisIDs = "";
        //    foreach (string s in DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs)
        //    {
        //        if (AnalysisIDs.Length > 0) AnalysisIDs += ",";
        //        AnalysisIDs += s;
        //    }
        //    if (AnalysisIDs.Length > 0)
        //    {
        //            SQL = "SELECT DISTINCT TOP 10 A.DisplayText, A.AnalysisID " +
        //            "FROM dbo.Analysis AS A " +
        //            "WHERE A.AnalysisID IN (" + AnalysisIDs + ") " +
        //            "ORDER BY A.DisplayText";
        //    }
        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter();
        //    System.Data.DataTable dt = new DataTable();
        //    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref ad, dt, SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(4000));
        //    this._AnalysisList.Clear();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        AnalysisEntry A = new AnalysisEntry();
        //        A.AnalysisID = int.Parse(dt.Rows[i]["AnalysisID"].ToString());
        //        A.AnalysisType = dt.Rows[i]["DisplayText"].ToString();
        //        this._AnalysisList.Add(A);
        //    }
        //}


        //private System.Collections.Generic.List<int> AnalysisIDs
        //{
        //    get
        //    {
        //        if (this._AnalysisIDs == null)
        //        {
        //            this._AnalysisIDs = new List<int>();
        //            try
        //            {
        //                foreach (string s in DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs)
        //                    this._AnalysisIDs.Add(int.Parse(s));
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //        return this._AnalysisIDs;
        //    }

        //}

        public System.Collections.Generic.List<string> AnalysisReadOnlyColumns
        {
            get 
            {
                if (this._AnalysisReadOnlyColumns == null)
                {
                    this._AnalysisReadOnlyColumns = new List<string>();
                    for (int i = 9; i >= this.AnalysisList.Count; i--)
                    {
                        this._AnalysisReadOnlyColumns.Add("Analysis_" + i.ToString());
                        this._AnalysisReadOnlyColumns.Add("AnalysisID_" + i.ToString());
                        this._AnalysisReadOnlyColumns.Add("Analysis_number_" + i.ToString());
                        this._AnalysisReadOnlyColumns.Add("Analysis_result_" + i.ToString());
                    }
                }
                return _AnalysisReadOnlyColumns; 
            }
        }

        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> TaxonAnalysisDict
        {
            get 
            {
                if (this._TaxonAnalysisDict == null)
                {
                    try
                    {
                        this._TaxonAnalysisDict = DiversityCollection.Analysis.AnalysisForTaxonomicGroup(this._ProjectID);
                    }
                    catch (System.Exception ex) { }
                }
                return _TaxonAnalysisDict; 
            }
        }

        #endregion   

        #region Grid

        private void GridModeFillTable()
        {
            try
            {
                //this.firstLinesUnitTableAdapter.Fill(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit);
                string SQL = this.GridModeFillCommand(); ;
                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Clear();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(900000));
                ad.Fill(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit);
                this.dataGridViewGridMode_RowEnter(null, null);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) 
            {
                string M = ex.Message;
                if (M.IndexOf("timeout") > -1) M += "\r\nPlease ask you database administrator to recreate the index for the table IdentificationUnit";
                System.Windows.Forms.MessageBox.Show(M);
            }
        }

        #region Formatting of the grid view

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.DrawRowNumber(this.dataGridView, this.dataGridView.Font, e);
        }

        private void setColumnWidths()
        {
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitColumnWidth.Length > 0)
            {
                string[] ColumnWidths = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitColumnWidth.Split(new char[] { ' ' });
                for (int i = 0; i < ColumnWidths.Length; i++)
                {
                    int Width = 0;
                    if (int.TryParse(ColumnWidths[i], out Width))
                    {
                        try
                        {
                            if (this.dataGridView.Columns.Count > i)
                                this.dataGridView.Columns[i].Width = Width;
                        }
                        catch { }
                    }
                }
            }
        }

        private void setColumnSequence()
        {
            try
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitColumnSequence.Length > 0)
                {
                    string[] Sequence = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitColumnSequence.Split(new char[] { ' ' });
                    for (int i = 0; i < Sequence.Length; i++)
                    {
                        int Display = 0;
                        if (int.TryParse(Sequence[i], out Display))
                        {
                            if (Display < this.dataGridView.Columns.Count)
                                this.dataGridView.Columns[i].DisplayIndex = Display;
                        }
                    }
                }
                else
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    {
                        C.DisplayIndex = C.Index;
                    }
                }

            }
            catch { }
        }

        private void setGridColumnHeaders()
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                {
                    string Property = C.DataPropertyName;
                    string Header = C.HeaderText;
                    string Entity = "";
                    if (!Property.EndsWith("_second_organism"))
                    {
                        foreach (Forms.GridModeQueryField Q in this.GridModeQueryFields)
                        {
                            if (Q.AliasForColumn == Property)
                            {
                                if (Q.Table == "IdentificationUnitAnalysis"
                                    && Q.Column == "AnalysisResult")
                                {
                                    switch (Q.AliasForTable)
                                    {
                                        case "Analysis_0":
                                            if (this.AnalysisList.Count > 0)
                                                C.HeaderText = this.AnalysisList[0].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_1":
                                            if (this.AnalysisList.Count > 1)
                                                C.HeaderText = this.AnalysisList[1].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_2":
                                            if (this.AnalysisList.Count > 2)
                                                C.HeaderText = this.AnalysisList[2].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_3":
                                            if (this.AnalysisList.Count > 3)
                                                C.HeaderText = this.AnalysisList[3].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_4":
                                            if (this.AnalysisList.Count > 4)
                                                C.HeaderText = this.AnalysisList[4].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_5":
                                            if (this.AnalysisList.Count > 5)
                                                C.HeaderText = this.AnalysisList[5].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_6":
                                            if (this.AnalysisList.Count > 6)
                                                C.HeaderText = this.AnalysisList[6].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_7":
                                            if (this.AnalysisList.Count > 7)
                                                C.HeaderText = this.AnalysisList[7].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_8":
                                            if (this.AnalysisList.Count > 8)
                                                C.HeaderText = this.AnalysisList[8].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                        case "Analysis_9":
                                            if (this.AnalysisList.Count > 9)
                                                C.HeaderText = this.AnalysisList[9].AnalysisType;
                                            else C.HeaderText = Q.Column + " " + Q.AliasForTable.Substring(Q.AliasForTable.Length - 1);
                                            break;
                                    }
                                    //string IdList = "";
                                    //foreach (int i in this._IDs)
                                    //    IdList += i.ToString() + ", ";
                                    //if (IdList.Length > 0) 
                                    //    IdList = IdList.Substring(0, IdList.Length - 2);
                                    //string IdColumn = "AnalysisID_" + Q.AliasForColumn.Substring(Q.AliasForColumn.Length - 1);
                                    //string SQL = "SELECT A.DisplayText " +
                                    //    "FROM dbo.FirstLinesUnit('" + IdList + "') AS F, dbo.Analysis AS A " +
                                    //    "WHERE NOT F." + IdColumn + " IS NULL AND F." + IdColumn + " = A.AnalysisID ";
                                    //C.HeaderText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                }
                                else
                                {
                                    System.Collections.Generic.Dictionary<string, string> EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.Column);
                                    if (EntityInfo["DisplayTextOK"] == "True")
                                    {
                                        Entity = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.Column)["DisplayText"];
                                    }
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

        private void setGridColumnToolTips(string Description, string DataPropertyName)
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
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
            this.dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            //if (this.buttonOptRowHeight.Tag == null)
            //{
            //    this.buttonOptRowHeight.Tag = "AllCells";
            //    this.buttonOptRowHeight.BackColor = System.Drawing.SystemColors.Highlight;
            //    this.dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            //}
            //else
            //{
            //    this.buttonOptRowHeight.Tag = null;
            //    this.buttonOptRowHeight.BackColor = System.Drawing.SystemColors.Control;
            //    //this.dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells);
            //    this.dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //}
        }

        private void buttonOptColumnWidth_Click(object sender, EventArgs e)
        {
            this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //if (this.buttonOptColumnWidth.Tag == null)
            //{
            //    this.buttonOptColumnWidth.Tag = "AllCells";
            //    this.buttonOptColumnWidth.BackColor = System.Drawing.SystemColors.Highlight;
            //}
            //else
            //{
            //    this.buttonOptColumnWidth.Tag = null;
            //    this.buttonOptColumnWidth.BackColor = System.Drawing.SystemColors.Control;
            //    this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //    //this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
            //}
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
                    if (this.dataGridView.SelectedCells.Count > 0)
                    {
                        int IdentificationUnitID = 0;
                        if (int.TryParse(this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out IdentificationUnitID))
                        {
                            for (i = 0; i < this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count; i++)
                            {
                                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].RowState == DataRowState.Deleted
                                    || this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].RowState == DataRowState.Detached)
                                    continue;
                                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i]["IdentificationUnitID"].ToString() == IdentificationUnitID.ToString())
                                    break;
                            }
                        }
                    }

                }
                catch (System.Exception ex) { }
                return i;
            }
        }

        private int DatasetIndexOfLine(int UnitID)
        {
            int i = 0;
            try
            {
                for (i = 0; i < this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count; i++)
                {
                    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i]["IdentificationUnitID"].ToString() == UnitID.ToString())
                        break;
                }

            }
            catch (System.Exception ex) { }
            return i;
        }

        private int GridIndexOfDataline(int UnitID)
        {
            int i = 0;
            try
            {
                if (this.dataGridView.Rows.Count > 0)
                {
                    for (i = 0; i < this.dataGridView.Rows.Count; i++)
                    {
                        if (this.dataGridView.Rows[i].Cells[0].Value.ToString() == UnitID.ToString())
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
                //this.setAnalysis();
                //this.SetAnalysisIDs();
                //this.setGridColumnHeaders();
                this.GridModeFillTable();
            }
            catch { }
        }

        private void buttonResetSequence_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitColumnSequence = "";
            this.setColumnSequence();
        }

        private void buttonGridModeUpdateColumnSettings_Click(object sender, EventArgs e)
        {
            this._GridModeColumnList = null;
            this._GridModeQueryFields = null;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility = "";
            this.GridModeSetColumnVisibility();
            this.enableReplaceButtons(false);
        }
        
        private void buttonGridModeFindUsedColumns_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.FindUsedDataColumns(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit, this.treeViewGridModeFieldSelector);
            this.buttonGridModeUpdateColumnSettings_Click(null, null);
        }

        #endregion

        #region Button events for Finding, Copy and Saving and related functions

        private void buttonGridModeFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count == 0 
                    || this.dataGridView.SelectedCells.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show(this.Message("Nothing_selected"));
                    return;
                }
                if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.SaveAll();
                int ID = 0;
                if (int.TryParse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine]["CollectionSpecimenID"].ToString(), out ID))
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
            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine].BeginEdit();
            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine].EndEdit();
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
                //for (int i = 0; i < this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count; i++)
                //{
                //    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].BeginEdit();
                //    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].EndEdit();
                //}
                if (this.dataSetIdentifictionUnitGridMode.HasChanges())
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
                this.progressBarSaveAll.Maximum = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count;
                this.progressBarSaveAll.Value = 0;
                for (int i = 0; i < this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count; i++)
                {
                    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].RowState == DataRowState.Deleted
                        || this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].RowState == DataRowState.Detached
                        || this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].RowState == DataRowState.Unchanged)
                        continue;
                    this.dataGridView.Rows[i].Cells[0].Selected = true;
                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].BeginEdit();
                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[i].EndEdit();
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
            if (this.dataGridView.SelectedRows.Count > 1) MoreThanOneRow = true;
            try
            {
                if (this.dataGridView.SelectedCells.Count > 1)
                {
                    System.Collections.Generic.List<int> RowIndex = new List<int>();
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
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

                if (System.Windows.Forms.MessageBox.Show("Do you want to create a copy of the current dataset", "Copy dataset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Data.DataRow Rori = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.dataGridView.SelectedCells[0].RowIndex];
                    System.Data.DataRow Rnew = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.NewFirstLinesUnitRow();
                    Rnew[0] = this.InsertNewUnit(this._SpecimenID);
                    for ( int i = 1; i < this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns.Count; i++)
                    {
                        string ColumnName = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns[i].ColumnName;
                        Rnew[ColumnName] = Rori[ColumnName];
                    }
                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Add(Rnew);


                }
            }
            catch { }
        }

        private void buttonGridModeDelete_Click(object sender, EventArgs e)
        {
            bool DeleteSelection = false;
            this.buttonGridModeDelete.Enabled = false;
            System.Collections.Generic.List<int> IDsToDelete = new List<int>();
            try
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)R.DataBoundItem;
                    int ID = 0;
                    if (int.TryParse(RV["IdentificationUnitID"].ToString(), out ID))
                    {
                        string Taxon = RV["Taxonomic_name"].ToString();
                        if (!DeleteSelection)
                        {
                            string Message = DiversityWorkbench.Entity.Message("Do_you_want_to_delete_the_dataset");
                            if (Taxon.Length > 0) Message += "\r\n" + DiversityWorkbench.Entity.EntityInformation("Identification.TaxonomicName")["DisplayText"] + ": " + Taxon;
                            else Message += " ID " + ID.ToString();
                            if (this.dataGridView.SelectedRows.Count > 1)
                                Message += " " + DiversityWorkbench.Entity.Message("and") + " " + (this.dataGridView.SelectedRows.Count - 1).ToString() + " " + this.Message("further_datasets") + "?";
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
                        //this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.AcceptChanges();
                    }
                }
                if (DeleteSelection)
                {
                    foreach (int ID in IDsToDelete)
                    {
                        this.deleteIdentificationUnit(ID);
                        //RV.Delete();
                        //this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.AcceptChanges();
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
        private void deleteIdentificationUnit(int ID)
        {
            try
            {
                string SQL = "DELETE FROM IdentificationUnit WHERE IdentificationUnitID = " + ID.ToString();
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

                if (this.dataGridView.SelectedCells.Count > 1)
                    this.enableReplaceButtons(true);
                else this.enableReplaceButtons(false);

                if (e.ColumnIndex == this.dataGridView.SelectedCells[0].ColumnIndex)
                {
                    switch (ColumnName)
                    {
                        //case "Link_to_GoogleMaps":
                        //    this.GetCoordinatesFromGoogleMaps();
                        //    break;
                        //case "NamedAreaLocation2":
                        //case "Link_to_SamplingPlots":
                        //case "Geographic_region":
                        //case "Lithostratigraphy":
                        //case "Chronostratigraphy":
                        case "Link_to_DiversityTaxonNames":
                        //case "Link_to_DiversityTaxonNames_of_second_organism":
                        //case "Link_to_DiversityAgents":
                        //case "Depositors_link_to_DiversityAgents":
                        //case "Link_to_DiversityExsiccatae":
                        case "Link_to_DiversityReferences":
                        //case "Link_to_DiversityReferences_of_second_organism":
                        case "Link_to_DiversityAgents_for_responsible":
                        //case "Link_to_DiversityAgents_for_responsible_of_second_organism":
                            this.GetRemoteValues(this.dataGridView.SelectedCells[0]);
                            break;
                        //case "On_loan":
                        //case "_TransactionID":
                        //    int TransactionID;
                        //    if (int.TryParse(this.dataGridView.SelectedCells[0].Value.ToString(), out TransactionID))
                        //    {
                        //        DiversityCollection.Forms.FormTransaction f = new FormTransaction(TransactionID);
                        //        f.ShowDialog();
                        //    }
                        //    else
                        //    {
                        //        string Message = "";
                        //        if (ColumnName == "On_loan") Message = "This dataset is not on loan";
                        //        if (ColumnName == "_TransactionID") Message = "This dataset is not involved in a transaction";
                        //        if (Message.Length > 0) System.Windows.Forms.MessageBox.Show(Message);
                        //    }
                        //    break;
                        //case "Remove_link_to_gazetteer":
                        //case "Remove_link_to_SamplingPlots":
                        //case "Remove_link_for_collector":
                        //case "Remove_link_for_Depositor":
                        //case "Remove_link_to_exsiccatae":
                        case "Remove_link_for_identification":
                        case "Remove_link_for_reference":
                        //case "Remove_link_for_reference_of_second_organism":
                        case "Remove_link_for_determiner":
                        //case "Remove_link_for_responsible_of_second_organism":
                        //case "Remove_link_for_second_organism":
                            this.RemoveLink(this.dataGridView.SelectedCells[0]);
                            break;
                        //case "Analysis":
                        //    this.setAnalysis();
                        //    break;
                        //case "Analysis_result":
                        //case "Analysis_result_0":
                        //    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID_0"].Equals(System.DBNull.Value))
                        //        this.setAnalysis();
                        //    break;
                        //case "Analysis_result_1":
                        //case "Analysis_result_2":
                        //case "Analysis_result_3":
                        //case "Analysis_result_4":
                        //case "Analysis_result_5":
                        //case "Analysis_result_6":
                        //case "Analysis_result_7":
                        //case "Analysis_result_8":
                        //case "Analysis_result_9":
                        //    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID"].Equals(System.DBNull.Value))
                        //        this.setAnalysis();
                        //    break;
                        //case "New_identification":
                        //case "New_identification_of_second_organism":
                        //    this.insertNewIdentification(this.dataGridView.SelectedCells[0]);
                        //    break;
                    }
                }
                if (this.textBoxHeaderID.Text.Length == 0)
                    this.setSpecimen(this.SpecimenID, this._UnitID);

            }
            catch (System.Exception ex) { }
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                this.labelGridViewReplaceColumnName.Text = ColumnName;
                if (this.buttonGridModeFind.Enabled == false)
                    this.buttonGridModeFind.Enabled = true;
                if ((this.dataGridView.SelectedCells.Count > 1 && ColumnName != "CollectionSpecimenID")
                    && (typeof(System.Windows.Forms.DataGridViewTextBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
                    && ColumnName != "CollectionSpecimenID")
                    //|| typeof(System.Windows.Forms.DataGridViewComboBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
                    )
                    this.enableReplaceButtons(true);
                else this.enableReplaceButtons(false);
                if (this.dataGridView.SelectedCells.Count == 1)
                    this.labelGridCounter.Text = "line " + (this.dataGridView.SelectedCells[0].RowIndex + 1).ToString() + " of " + (this.dataGridView.Rows.Count).ToString();
                else if (this.dataGridView.SelectedCells.Count > 1)
                {
                    int StartLine = this.dataGridView.SelectedCells[0].RowIndex + 1;
                    int EndLine = this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1;
                    //if (this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1 < StartLine)
                    //    StartLine = this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1;
                    this.labelGridCounter.Text = "line ";
                    if (EndLine > StartLine)
                        this.labelGridCounter.Text += StartLine.ToString() + " to " + EndLine.ToString();
                    else if (EndLine < StartLine)
                        this.labelGridCounter.Text += EndLine.ToString() + " to " + StartLine.ToString();
                    else
                        this.labelGridCounter.Text += EndLine.ToString();
                    this.labelGridCounter.Text += " of " + (this.dataGridView.Rows.Count).ToString();
                }
                else
                    this.labelGridCounter.Text = "line 1 of " + (this.dataGridView.Rows.Count).ToString();

            }
            catch { }
        }

        private void dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridView.SelectedCells.Count > 0 &&
                         this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                         this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0)
                    this.checkForMissingAndDefaultValues(this.dataGridView.SelectedCells[0], false);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void checkForMissingAndDefaultValues(System.Windows.Forms.DataGridViewCell Cell, bool Silent)
        {
            try
            {
                if (this.dataGridView.SelectedCells.Count > 0 &&
                     this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                     this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0)
                {
                    string ColumnName = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
                    string Message = "";
                    string Value = Cell.EditedFormattedValue.ToString();
                    System.Data.DataRow Rcurr = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex];

                    // Checking if a correct value was entered
                    //if (!this.ValueIsValid(Cell.ColumnIndex, Value))
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
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Taxonomic_group_of_second_organism"].Equals(System.DBNull.Value)
                                && !Silent)
                                System.Windows.Forms.MessageBox.Show("Please select a taxonomic group for this organism");
                            break;
                        case "AnalysisID":
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Analysis_number"].Equals(System.DBNull.Value))
                            {
                                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Analysis_number"] = 1;
                            }
                            goto case "Exsiccata_number";
                        case "Analysis_number":
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["AnalysisID"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnitAnalysis.AnalysisID")
                                    && this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"].Length > 0)
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["AnalysisID"] = this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"];
                                else
                                    if (!Silent) Message += "Please select an analysis type\r\n";
                            }
                            goto case "Exsiccata_number";
                        case "Analysis_result":
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Analysis_number"].Equals(System.DBNull.Value))
                            {
                                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Analysis_number"] = 1;
                            }
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["AnalysisID"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnitAnalysis.AnalysisID")
                                    && this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"].Length > 0)
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["AnalysisID"] = this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"];
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
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Taxonomic_group"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup")
                                    && this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Taxonomic_group"] = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
                                else
                                    if (!Silent) Message += "Please select a taxonomic group for this organism";
                            }
                            break;
                        case "Collection":
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Material_category"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Material_category"] = this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"];
                                else
                                    if (!Silent) Message = "Please select a material category for this part.";
                            }
                            break;
                        case "Material_category":
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Collection"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Collection"] = this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
                                else
                                    if (!Silent) Message = "Please select a collection for this part.";
                            }
                            break;
                        case "Storage_location":
                        case "Stock":
                        case "Preparation_method":
                        case "Preparation_date":
                        case "Notes_for_part":
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Material_category"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Material_category"] = this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"];
                                else
                                    if (!Silent) Message = "Please select a material category for this part.\r\n";
                            }
                            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Collection"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Cell.RowIndex]["Collection"] = this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
                                else
                                    if (!Silent) Message += "Please select a collection for this part.";
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
                        if (!Silent) System.Windows.Forms.MessageBox.Show(Message);
                }
                if (this.dataGridView.SelectedCells.Count > 0)
                    this.setCellBlockings(Cell.RowIndex);

            }
            catch (System.Exception ex) { }
        }

        private void dataGridViewGridMode_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this._FormState == Forms.FormGridFunctions.FormState.Loading)
                    return;
                int SpecimenID = 0;
                if (this._UseAsSpecimenTaxonList) 
                    SpecimenID = this._SpecimenID;
                int UnitID = 0;
                int iCurrentLine = DatasetIndexOfCurrentLine;// -1;
                if (iCurrentLine == -1) iCurrentLine = 0;
                if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex < this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count &&
                    int.TryParse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[iCurrentLine]["CollectionSpecimenID"].ToString(), out SpecimenID) &&
                    int.TryParse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[iCurrentLine]["IdentificationUnitID"].ToString(), out UnitID))
                {
                    if (SpecimenID != this._SpecimenID ||
                        UnitID != this._UnitID)
                    {
                        this.setSpecimen(SpecimenID, UnitID);
                        this._SpecimenID = SpecimenID;
                        this._UnitID = UnitID;
                    }
                }
                else if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count)
                {
                    this.insertNewDataset(this.dataGridView.SelectedCells[0].RowIndex);
                }
                else if (this.dataGridView.SelectedCells.Count == 0
                    && this.dataGridView.Rows.Count > 0
                    && this.dataGridView.Rows[0].Cells[1].Value != null)
                {
                    if (int.TryParse(this.dataGridView.Rows[0].Cells[1].Value.ToString(), out SpecimenID) &&
                        int.TryParse(this.dataGridView.Rows[0].Cells[0].Value.ToString(), out UnitID))
                    {
                        this.setSpecimen(SpecimenID, UnitID);
                        this._SpecimenID = SpecimenID;
                        this._UnitID = UnitID;
                    }
                }
                else if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count == 0)
                {
                    this.insertNewDataset(0);
                }
                this.setCellBlockings();
                this.setRemoveCellStyle();
            }
            catch { }
        }

        private string GridModeFillCommand()
        {
            string SQL = "SELECT " + this._SqlSpecimenFields + " FROM dbo." + this.SourceFunction + " ";

            try
            {
                string WhereClause = "";
                foreach (int i in this._IDs)
                {
                    WhereClause += i.ToString() + ", ";
                }
                if (WhereClause.Length == 0) WhereClause = " ('') ";
                else
                    WhereClause = " ('" + WhereClause.Substring(0, WhereClause.Length - 2) + "'";
                WhereClause += ", ";
                if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs != null &&
                    DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs.Count > 0)
                {
                    WhereClause += "'";
                    foreach (string s in DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs)
                        WhereClause += s + ",";
                    WhereClause = WhereClause.Substring(0, WhereClause.Length - 1) + "'";
                }
                else
                    WhereClause += "null";
                WhereClause += ", ";
                if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisStartDate)
                {
                    WhereClause += "'" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Year.ToString() +
                        "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Month.ToString() +
                        "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Day.ToString() + "'";
                }
                else
                    WhereClause += "null";
                WhereClause += ", ";
                if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisEndDate)
                {
                    WhereClause += "'" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Year.ToString() +
                        "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Month.ToString() +
                        "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Day.ToString() + "'";
                }
                else
                    WhereClause += "null";
                WhereClause += ")";
                SQL += WhereClause + " ORDER BY Accession_number, CollectionSpecimenID, IdentificationUnitID ";
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
                if (this.dataSetIdentifictionUnitGridMode.HasChanges())
                {
                    System.Data.DataRow RDataset = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index];
                    if (RDataset.RowState == DataRowState.Modified || RDataset.RowState == DataRowState.Added)
                    {
                        // setting the dataset
                        // the dataset is filled with the original data from the database as a basis for comparision with the data in the grid
                        int CollectionSpecimenID = int.Parse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["CollectionSpecimenID"].ToString());
                        int IdentificationUnitID = int.Parse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["IdentificationUnitID"].ToString());
                        this._SpecimenID = CollectionSpecimenID;
                        this._UnitID = IdentificationUnitID;
                        this.fillSpecimen(this._SpecimenID, this._UnitID);

                        // if no data 
                        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count == 0)
                            this.setSpecimen(this._SpecimenID, this.UnitID);

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
                                    && !Q.AliasForColumn.StartsWith("Remove"))
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
                                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns.Contains(KVc.Key))
                                {
                                    ColumnValues[KVc.Value] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][KVc.Key].ToString();
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
                                                            WhereClause += " IdentificationUnitID = " + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["IdentificationUnitID"].ToString();
                                                            break;
                                                        case "IdentificationUnitAnalysis":
                                                            break;
                                                    }
                                                    break;
                                                case "AnalysisID":
                                                    switch (KV.Key)
                                                    {
                                                        case "Analysis_0":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[0].AnalysisID;
                                                            break;
                                                        case "Analysis_1":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[1].AnalysisID;
                                                            break;
                                                        case "Analysis_2":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[2].AnalysisID;
                                                            break;
                                                        case "Analysis_3":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[3].AnalysisID;
                                                            break;
                                                        case "Analysis_4":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[4].AnalysisID;
                                                            break;
                                                        case "Analysis_5":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[5].AnalysisID;
                                                            break;
                                                        case "Analysis_6":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[6].AnalysisID;
                                                            break;
                                                        case "Analysis_7":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[7].AnalysisID;
                                                            break;
                                                        case "Analysis_8":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[8].AnalysisID;
                                                            break;
                                                        case "Analysis_9":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " AnalysisID = " + this.AnalysisList[9].AnalysisID;
                                                            break;
                                                    }
                                                    break;
                                                case "IdentificationSequence":
                                                    switch (KV.Key)
                                                    {
                                                        case "Identification":
                                                            if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"].ToString();
                                                            }
                                                            break;
                                                        case "SecondUnitIdentification":
                                                            if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"].ToString();
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
                                    if (Qrest.AliasForTable == KV.Key && Qrest.Restriction != null && Qrest.Restriction.Length > 0)
                                    {
                                        if (WhereClause.Length > 0)
                                            WhereClause += " AND " + Qrest.Restriction;
                                        else
                                            WhereClause = Qrest.Restriction;
                                        break;
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
                                            if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_CollectionEventID"].Equals(System.DBNull.Value) &&
                                                this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
                                                this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_CollectionEventID"];
                                        }
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
                                catch{}
                            }
                        }
                        this.updateSpecimen();
                        RDataset.AcceptChanges();
                    }
                }
            }
            catch { }
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
                    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].Equals(System.DBNull.Value) ||
                        this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].ToString().Length == 0)
                    {
                        string Message = "Please enter the taxonomic group for the second organism ";
                        if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value))
                            Message += this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString() + " ";
                        if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Accession_number"].Equals(System.DBNull.Value))
                            Message += this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Accession_number"].ToString();
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
                        && !Q.AliasForColumn.StartsWith("Remove"))
                    {
                        TableColumns.Add(Q.AliasForColumn, Q.Column);
                        ColumnValues.Add(Q.Column, "");
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> KVc in TableColumns)
                {
                    ColumnValues[KVc.Value] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][KVc.Key].ToString();
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
                        if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns.Contains(AliasForColumn))
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
                        ColumnValues[KVpk.Key] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][AliasForColumn].ToString();
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
                                if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["Locality_description"].Equals(System.DBNull.Value))
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["Locality_description"].ToString();
                                Locality = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["Locality_description"].ToString();
                                int EventID = this.createEvent(Index);
                                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["_CollectionEventID"] = EventID;
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
                                        TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group"].ToString();
                                        if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
                                            LastIdentification = TaxonomicGroup;
                                        else
                                            LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString();
                                        DisplayOrder = 1;
                                        //this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Last_identification"] = LastIdentification;
                                        break;
                                    case "SecondUnit":
                                        TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
                                        if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
                                            LastIdentification = TaxonomicGroup;
                                        else
                                            LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
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
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationUnitID"] = UnitID;
                                            break;
                                        case "SecondUnit":
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondUnitID"] = UnitID;
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
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"] = Sequence;
                                            break;
                                        case "IdentificationSecondUnit":
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"] = Sequence;
                                            break;
                                    }
                                }
                                else
                                {
                                    ColumnValues[KVpk.Key] = "1";
                                    switch (AliasForTable)
                                    {
                                        case "Identification":
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"] = 1;
                                            break;
                                        case "IdentificationSecondUnit":
                                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"] = 1;
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
                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SpecimenPartID"] = PartID;
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
                    !this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["_CollectionEventID"].Equals(System.DBNull.Value))
                    Rnew["CollectionEventID"] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["_CollectionEventID"];

                if (Table == "CollectionSpecimenPart" &&
                    Rnew["SpecimenPartID"].Equals(System.DBNull.Value))
                    Rnew["SpecimenPartID"] = 1;

                if (Table == "IdentificationUnitAnalysis" &&
                    Rnew["AnalysisNumber"].Equals(System.DBNull.Value))
                    Rnew["AnalysisNumber"] = "1";

                if (Table == "IdentificationUnitAnalysis" &&
                    Rnew["AnalysisID"].Equals(System.DBNull.Value))
                {
                    switch(AliasForTable)
                    {
                        case "Analysis_0":
                            Rnew["AnalysisID"] = this.AnalysisList[0].AnalysisID;
                            break;
                        case "Analysis_1":
                            Rnew["AnalysisID"] = this.AnalysisList[1].AnalysisID;
                            break;
                        case "Analysis_2":
                            Rnew["AnalysisID"] = this.AnalysisList[2].AnalysisID;
                            break;
                        case "Analysis_3":
                            Rnew["AnalysisID"] = this.AnalysisList[3].AnalysisID;
                            break;
                        case "Analysis_4":
                            Rnew["AnalysisID"] = this.AnalysisList[4].AnalysisID;
                            break;
                        case "Analysis_5":
                            Rnew["AnalysisID"] = this.AnalysisList[5].AnalysisID;
                            break;
                        case "Analysis_6":
                            Rnew["AnalysisID"] = this.AnalysisList[6].AnalysisID;
                            break;
                        case "Analysis_7":
                            Rnew["AnalysisID"] = this.AnalysisList[7].AnalysisID;
                            break;
                        case "Analysis_8":
                            Rnew["AnalysisID"] = this.AnalysisList[8].AnalysisID;
                            break;
                        case "Analysis_9":
                            Rnew["AnalysisID"] = this.AnalysisList[9].AnalysisID;
                            break;
                    }
                }

                if (Table == "IdentificationUnitAnalysis" &&
                    Rnew["AnalysisDate"].Equals(System.DBNull.Value))
                {
                    Rnew["AnalysisDate"] = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                }


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

                this.dataSetCollectionSpecimen.Tables[Table].Rows.Add(Rnew);
                if (PKcontainsIdentity)
                    Rnew.AcceptChanges();
            }
            catch { }
        }

        #region Remote services

        private void GetCoordinatesFromGoogleMaps()
        {
            string Latitude = "";
            string Longitude = "";
            try
            {
                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine]["Latitude"].Equals(System.DBNull.Value))
                {
                    Latitude = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine]["Latitude"].ToString();
                    Longitude = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine]["Longitude"].ToString();
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
                            System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
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
                        RvbCountry.BindingSource = this.firstLinesUnitBindingSource;
                        RvbCountry.Column = "Country";
                        RvbCountry.RemoteParameter = "Country";
                        RemoteValueBindings.Add(RvbCountry);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLatitude.BindingSource = this.firstLinesUnitBindingSource;
                        RvbLatitude.Column = "_NamedAverageLatitudeCache";
                        RvbLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLongitude.BindingSource = this.firstLinesUnitBindingSource;
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
                        RvbSamplingPlotLatitude.BindingSource = this.firstLinesUnitBindingSource;
                        RvbSamplingPlotLatitude.Column = "Latitude_of_sampling_plot";
                        RvbSamplingPlotLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotLongitude.BindingSource = this.firstLinesUnitBindingSource;
                        RvbSamplingPlotLongitude.Column = "Longitude_of_sampling_plot";
                        RvbSamplingPlotLongitude.RemoteParameter = "Longitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLongitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotAccuracy = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotAccuracy.BindingSource = this.firstLinesUnitBindingSource;
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
                        RvbHierarchyLitho.BindingSource = this.firstLinesUnitBindingSource;
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
                        RvbHierarchyChrono.BindingSource = this.firstLinesUnitBindingSource;
                        RvbHierarchyChrono.Column = "_ChronostratigraphyPropertyHierarchyCache";
                        RvbHierarchyChrono.RemoteParameter = "HierarchyCache";
                        RemoteValueBindings.Add(RvbHierarchyChrono);
                        break;

                    case "Link_to_DiversityTaxonNames":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames")) return;
                        ValueColumn = "Link_to_DiversityTaxonNames";
                        DisplayColumn = "Taxonomic_name";
                        DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbFamily.BindingSource = this.firstLinesUnitBindingSource;
                        RvbFamily.Column = "Family_of_taxon";
                        RvbFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbOrder.BindingSource = this.firstLinesUnitBindingSource;
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
                        RvbSecondFamily.BindingSource = this.firstLinesUnitBindingSource;
                        RvbSecondFamily.Column = "_SecondUnitFamilyCache";
                        RvbSecondFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbSecondFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSecondOrder.BindingSource = this.firstLinesUnitBindingSource;
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

                    default:
                        DiversityWorkbench.TaxonName Default = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Default;
                        break;
                }

                if (this.firstLinesUnitBindingSource != null && IWorkbenchUnit != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
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
                    f.TopMost = true;
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.DisplayText != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
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

                if (this.firstLinesUnitBindingSource != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
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
                    this._RemoveColumns.Add("Remove_link_for_identification", "");
                    this._RemoveColumns.Add("Remove_link_for_reference", "");
                    this._RemoveColumns.Add("Remove_link_for_determiner", "");
                    this._RemoveColumns.Add("Remove_link_for_second_organism", "");
                    this._RemoveColumns.Add("Remove_link_for_reference_of_second_organism", "");
                    this._RemoveColumns.Add("Remove_link_for_responsible_of_second_organism", "");
                }
                return this._RemoveColumns;
            }
        }

        private void setRemoveCellStyle()
        {
            try
            {
                for (int i = 0; i < this.dataGridView.Rows.Count; i++)
                    this.setRemoveCellStyle(i);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setRemoveCellStyle(int RowIndex)
        {
            //if (this._StyleRemove == null)
            //{
            //    this._StyleRemove = new DataGridViewCellStyle();
            //    this._StyleRemove.BackColor = System.Drawing.Color.Red;
            //    this._StyleRemove.SelectionBackColor = System.Drawing.Color.Red;
            //    this._StyleRemove.ForeColor = System.Drawing.Color.Red;
            //    this._StyleRemove.SelectionForeColor = System.Drawing.Color.Red;
            //    this._StyleRemove.Tag = "Remove";
            //}
            try
            {
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this.RemoveColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell RemoveCell in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.dataGridView.Columns[RemoveCell.ColumnIndex].DataPropertyName ==
                                this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
                            {
                                RemoveCell.Style = DiversityCollection.Forms.FormGridFunctions.StyleRemove;// this._StyleRemove;
                                break;
                            }
                        }
                    }
                }
            }
            catch { }
        }
        
        #endregion

        #endregion

        #region Columns for new identifications

        //private void insertNewIdentification(System.Windows.Forms.DataGridViewCell Cell)
        //{
        //    string DisplayColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
        //    string Table = "SecondUnitIdentification";
        //    if (this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName == "New_identification")
        //        Table = "Identification";

        //    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        //    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationRow R = this.dataSetCollectionSpecimen.Identification.NewIdentificationRow();
        //    R.CollectionSpecimenID = int.Parse(RU["CollectionSpecimenID"].ToString());
        //    if (Table == "Identification")
        //    {
        //        if (RU["_IdentificationUnitID"].Equals(System.DBNull.Value))
        //            return;
        //        R.IdentificationUnitID = int.Parse(RU["_IdentificationUnitID"].ToString());
        //    }
        //    else
        //    {
        //        if (RU["_SecondUnitID"].Equals(System.DBNull.Value))
        //            return;
        //        R.IdentificationUnitID = int.Parse(RU["_SecondUnitID"].ToString());
        //    }
        //    System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.Identification.Select("", "IdentificationSequence DESC");
        //    if (rr.Length == 0)
        //        return;
        //    short Sequence = 0;
        //    if (!short.TryParse(rr[0]["IdentificationSequence"].ToString(), out Sequence))
        //        return;
        //    Sequence++;
        //    R.IdentificationSequence = Sequence;
        //    R.TaxonomicName = "New identification";
        //    this.dataSetCollectionSpecimen.Identification.Rows.Add(R);

        //    try
        //    {
        //        foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        //        {
        //            if (Q.AliasForTable == Table)
        //            {
        //                if (this.firstLinesUnitBindingSource != null)
        //                {
        //                    foreach (System.Data.DataColumn C in R.Table.Columns)
        //                    {
        //                        foreach (DiversityCollection.Forms.GridModeQueryField QQ in this.GridModeQueryFields)
        //                        {
        //                            if (QQ.AliasForTable == R.Table.TableName
        //                                && QQ.Column == C.ColumnName
        //                                && !QQ.AliasForColumn.StartsWith("Remove"))
        //                            {
        //                                try { RU[QQ.AliasForColumn] = R[QQ.Column]; }
        //                                catch { }
        //                            }
        //                            //foreach (System.Data.DataColumn Ccurrent in RU.Row.Table.Columns)
        //                            //{
        //                            //    if (C.ColumnName == Ccurrent.ColumnName) ;
        //                            //}
        //                        }
        //                    }
        //                    //System.Data.DataRowView RUx = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        //                    //string Tee = "";
        //                    //if (this.dataGridView.Columns.Contains(Q.AliasForColumn))
        //                    //{ string y = ""; }
        //                    ////if (this.dataGridView.Columns[Q.AliasForColumn].ReadOnly == false)
        //                    ////{
        //                    //    try 
        //                    //    { 
        //                    //        RUx[Q.AliasForColumn] = System.DBNull.Value; 
        //                    //    }
        //                    //    catch { }
        //                    ////}
        //                }
        //            }
        //        }

        //        //foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        //        //{
        //        //    //if (Q.AliasForTable == Table && Q.Column == ValueColumn)
        //        //    //{
        //        //    //    LinkColumn = Q.AliasForColumn;
        //        //    //    break;
        //        //    //}
        //        //}


        //    }
        //    catch { }
        //}

        ///// <summary>
        ///// Dictionary of columns that insert new identifications
        ///// </summary>
        //private System.Collections.Generic.Dictionary<string, string> NewIdentificationColumns
        //{
        //    get
        //    {
        //        if (this._NewIdentificationColumns == null)
        //        {
        //            this._NewIdentificationColumns = new Dictionary<string, string>();
        //            this._NewIdentificationColumns.Add("New_identification", "");
        //            this._NewIdentificationColumns.Add("New_identification_of_second_organism", "");
        //        }
        //        return this._NewIdentificationColumns;
        //    }
        //}

        //private void setNewIdentificationCellStyle(int RowIndex)
        //{
        //    if (this._StyleNewIdentification == null)
        //    {
        //        this._StyleNewIdentification = new DataGridViewCellStyle();
        //        this._StyleNewIdentification.BackColor = System.Drawing.Color.Green;
        //        this._StyleNewIdentification.SelectionBackColor = System.Drawing.Color.Green;
        //        this._StyleNewIdentification.ForeColor = System.Drawing.Color.Green;
        //        this._StyleNewIdentification.SelectionForeColor = System.Drawing.Color.Green;
        //        this._StyleNewIdentification.Tag = "New identification";
        //    }
        //    try
        //    {
        //        foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
        //        {
        //            if (this.NewIdentificationColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
        //            {
        //                foreach (System.Windows.Forms.DataGridViewCell NewIdentificationCell in this.dataGridView.Rows[RowIndex].Cells)
        //                {
        //                    if (this.dataGridView.Columns[NewIdentificationCell.ColumnIndex].DataPropertyName ==
        //                        this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
        //                    {
        //                        NewIdentificationCell.Style = this._StyleNewIdentification;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

        #endregion

        #region Blocking of Cells that are linked to external services

        private void setCellBlockings()
        {
            try
            {
                for (int i = 0; i < this.dataGridView.Rows.Count; i++)
                {
                    this.setCellBlockings(i);
                    this.setCellBlockingsForAnalysis(i);
                }
            }
            catch (System.Exception ex) 
            { }
        }

        private void setCellBlockings(int RowIndex)
        {
            //if (DiversityCollection.Forms.FormGridFunctions.StyleBlocked this._StyleBlocked == null)
            //{
            //    this._StyleBlocked = new DataGridViewCellStyle();
            //    this._StyleBlocked.BackColor = System.Drawing.Color.Yellow;
            //    this._StyleBlocked.SelectionBackColor = System.Drawing.Color.Yellow;
            //    this._StyleBlocked.ForeColor = System.Drawing.Color.Blue;
            //    this._StyleBlocked.SelectionForeColor = System.Drawing.Color.Blue;
            //    this._StyleBlocked.Tag = "Blocked";
            //}
            //if (this._StyleUnblocked == null)
            //{
            //    this._StyleUnblocked = new DataGridViewCellStyle();
            //    this._StyleUnblocked.BackColor = System.Drawing.SystemColors.Window;
            //    this._StyleUnblocked.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            //    this._StyleUnblocked.ForeColor = System.Drawing.Color.Black;
            //    this._StyleUnblocked.SelectionForeColor = System.Drawing.SystemColors.Window;
            //    this._StyleUnblocked.Tag = "";
            //}
            //if (this._StyleReadOnly == null)
            //{
            //    this._StyleReadOnly = new DataGridViewCellStyle();
            //    this._StyleReadOnly.BackColor = System.Drawing.Color.LightGray;
            //    this._StyleReadOnly.SelectionBackColor = System.Drawing.Color.LightGray;
            //    this._StyleReadOnly.ForeColor = System.Drawing.Color.Black;
            //    this._StyleReadOnly.SelectionForeColor = System.Drawing.Color.Black;
            //    this._StyleReadOnly.Tag = "ReadOnly";
            //}
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
                                    CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleBlocked;// DiversityCollection.Forms.FormGridFunctions.StyleBlocked;
                                    CellToBlock.ReadOnly = true;
                                }
                                else
                                {
                                    CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleUnblocked;// this._StyleUnblocked;
                                    CellToBlock.ReadOnly = false;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.ReadOnlyColumns.Contains(this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName))
                            {
                                CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly;// this._StyleReadOnly;
                                CellToBlock.ReadOnly = true;
                            }
                        }
                    }
                }

                //System.Windows.Forms.DataGridViewCellStyle StyleReadOnly = new DataGridViewCellStyle();
                //StyleReadOnly.BackColor = System.Drawing.Color.LightGray;
                //StyleReadOnly.SelectionBackColor = System.Drawing.Color.LightGray;
                //StyleReadOnly.ForeColor = System.Drawing.Color.Gray;
                //StyleReadOnly.SelectionForeColor = System.Drawing.Color.Gray;
                //StyleReadOnly.Tag = "ReadOnly";

                //foreach (string R in this.ReadOnlyColumns)
                //{
                //    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                //    {
                //        if (C.DataPropertyName == R)
                //            C.HeaderCell.Style = StyleReadOnly;
                //    }
                //}
            }
            catch (System.Exception ex) { }
        }

        private void setCellBlockingsForAnalysis(int RowIndex)
        {
            try
            {
                string TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[RowIndex]["Taxonomic_group"].ToString();
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName == "Taxonomic_group")
                    {
                        TaxonomicGroup = Cell.Value.ToString();
                        break;
                    }
                }
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    string DataProperty = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
                    if (DataProperty.StartsWith("Analysis_result"))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.AnalysisReadOnlyColumns.Contains(this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName))
                            {
                                CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly;// this._StyleReadOnly;
                                CellToBlock.ReadOnly = true;
                            }
                            else
                            {
                                int i;
                                if (int.TryParse(DataProperty.Substring(DataProperty.Length - 1), out i))
                                {
                                    if (this.AnalysisList.Count > 0 
                                        && this.TaxonAnalysisDict[TaxonomicGroup].Contains(this.AnalysisList[i].AnalysisID))
                                    {
                                        Cell.Style = DiversityCollection.Forms.FormGridFunctions.StyleUnblocked;// this._StyleUnblocked;
                                        Cell.ReadOnly = false;
                                    }
                                    else
                                    {
                                        Cell.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly; //this._StyleReadOnly;
                                        Cell.ReadOnly = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
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
                    this._BlockedColumns.Add("Link_to_DiversityTaxonNames", "Taxonomic_name");
                    this._BlockedColumns.Add("Link_to_DiversityReferences", "Reference_title");
                    this._BlockedColumns.Add("Link_to_DiversityAgents_for_responsible", "Responsible");

                    this._BlockedColumns.Add("Link_to_DiversityTaxonNames_of_second_organism", "Taxonomic_name_of_second_organism");
                    this._BlockedColumns.Add("Link_to_DiversityReferences_of_second_organism", "Reference_title_of_second_organism");
                    this._BlockedColumns.Add("Link_to_DiversityAgents_for_responsible_of_second_organism", "Responsible_of_second_organism");

                    this._BlockedColumns.Add("NamedAreaLocation2", "Named_area");
                    this._BlockedColumns.Add("Link_to_SamplingPlots", "Sampling_plot");
                }
                //foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                //{
                //    if (Q.Table != "IdentificationUnit" &&
                //        Q.Table != "Identification" &&
                //        Q.Table != "IdentificationUnitAnalysis")
                //        if (!this._BlockedColumns.ContainsKey(Q.AliasForColumn))
                //            this._BlockedColumns.Add(Q.AliasForColumn, Q.AliasForColumn);
                //}
                return this._BlockedColumns;
            }
        }

        private System.Collections.Generic.List<string> ReadOnlyColumns
        {
            get
            {
                if (this._ReadOnlyColumns == null)
                {
                    this._ReadOnlyColumns = new List<string>();
                    foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                    {
                        if (Q.Table != "IdentificationUnit" &&
                            Q.Table != "Identification" &&
                            Q.Table != "IdentificationUnitAnalysis")
                            if (!this._ReadOnlyColumns.Contains(Q.AliasForColumn))
                                this._ReadOnlyColumns.Add(Q.AliasForColumn);
                    }
                    this._ReadOnlyColumns.Add("Related_organism");
                    this._ReadOnlyColumns.Add("Analysis");
                    this._ReadOnlyColumns.Add("AnalysisID");
                }
                return this._ReadOnlyColumns;
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
                case Forms.FormCopyDataset.EventCopyMode.NewEvent:
                    SQL += ", 1";
                    break;
                case Forms.FormCopyDataset.EventCopyMode.SameEvent:
                    SQL += ", 0";
                    break;
                case Forms.FormCopyDataset.EventCopyMode.NoEvent:
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
                // Bugfix - procedure returns several lines
                int id;
                cmd.CommandText = "SELECT MAX(CollectionSpecimenID) FROM CollectionSpecimen WHERE LogUpdatedBy = SYSTEM_USER OR LogUpdatedBy = cast([dbo].[UserID]() as varchar)";
                if (int.TryParse(cmd.ExecuteScalar().ToString(), out id))
                    ID = id;
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

        private void dataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && this._UseAsSpecimenTaxonList)
            {
                if (this.dataGridView.SelectedCells[0].RowIndex == this.dataGridView.Rows.Count - 1)
                {
                    try
                    {
                        int Position = this.dataGridView.SelectedCells[0].RowIndex;
                        int UnitID = this.InsertNewUnit(this._SpecimenID);
                        System.Data.DataTable dt = new DataTable();
                        string SQL = "SELECT " + this._SqlSpecimenFields +
                            " FROM dbo." + this.SourceFunction + " ('" + this._SpecimenID.ToString() + "', null, null, null) WHERE IdentificationUnitID = " + UnitID.ToString();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        DiversityCollection.Datasets.DataSetIdentifictionUnitGridMode.FirstLinesUnitRow Rnew = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.NewFirstLinesUnitRow();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (System.Data.DataColumn C in Rnew.Table.Columns)
                            {
                                Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
                            }
                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Add(Rnew);
                        }
                        Position++;
                        //this.dataGridView.SelectedCells[0].Selected = false;
                        System.Windows.Forms.DataGridViewCell Cell = this.dataGridView.Rows[Position].Cells[this.dataGridView.SelectedCells[0].ColumnIndex];
                        this.dataGridView.CurrentCell = Cell;
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        private void insertNewDataset(int Index)
        {
            if ((this.dataGridView.SelectedCells.Count > 0 &&
                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0 &&
                this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count)
                || this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count == 0)
            {
                string TaxonomicGroup = "";
                if (this.ProjectSettings.Count > 0)
                {
                    if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
                        this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
                    {
                        TaxonomicGroup = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
                    }

                }
                DiversityCollection.Forms.FormIdentificationUnitGridNewEntry f = new DiversityCollection.Forms.FormIdentificationUnitGridNewEntry(this.dataSetCollectionSpecimen, TaxonomicGroup);
                f.TopMost = true;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    int ID = this._SpecimenID;
                    if (f.IdentificationUnitCopyMode == DiversityCollection.Forms.FormIdentificationUnitGridNewEntry.UnitCopyMode.NewSpecimen)
                    {
                        try
                        {
                            if (f.AccessionNumber.Length > 0)
                                ID = this.InsertNewSpecimen(f.AccessionNumber);
                            else
                                ID = this.InsertNewSpecimen(Index);
                            this._IDs.Add(ID);
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            return;
                        }
                    }
                    try
                    {
                        int UnitID = this.InsertNewUnit(ID);
                        System.Data.DataTable dt = new DataTable();
                        string SQL = "SELECT " + this._SqlSpecimenFields +
                            " FROM dbo." + this.SourceFunction + " ('" + ID.ToString() + "', null, null, null) WHERE IdentificationUnitID = " + UnitID.ToString();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        DiversityCollection.Datasets.DataSetIdentifictionUnitGridMode.FirstLinesUnitRow Rnew = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.NewFirstLinesUnitRow();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (System.Data.DataColumn C in Rnew.Table.Columns)
                            {
                                Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
                            }
                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Add(Rnew);
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
                    //string TaxonomicGroup = "";
                    //if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
                    //    this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
                    //{
                    //    TaxonomicGroup = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
                    //}
                    //else
                    //    TaxonomicGroup = this.dataSetCollectionSpecimen.IdentificationUnit.Rows[0]["TaxonomicGroup"].ToString();
                    //SQL = "INSERT INTO IdentificationUnit " +
                    //    "(CollectionSpecimenID, LastIdentificationCache, TaxonomicGroup, DisplayOrder) " +
                    //    "VALUES (" + ID.ToString() + ", '" + TaxonomicGroup
                    //    + "', '" + TaxonomicGroup + "', 1)";
                    //cmd.CommandText = SQL;
                    //cmd.ExecuteNonQuery();
                    if (this.ProjectSettings.Count > 0)
                    {
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

        private int InsertNewUnit(int SpecimenID)
        {
            try
            {
                int UnitID;
                string TaxonomicGroup = "";
                if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
                    this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
                    TaxonomicGroup = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
                else
                    TaxonomicGroup = this.dataSetCollectionSpecimen.IdentificationUnit.Rows[0]["TaxonomicGroup"].ToString();
                string SQL = "INSERT INTO IdentificationUnit " +
                    "(CollectionSpecimenID, LastIdentificationCache, TaxonomicGroup, DisplayOrder) " +
                    "VALUES (" + SpecimenID.ToString() + ", '" + TaxonomicGroup
                    + "', '" + TaxonomicGroup + "', 1) (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                cmd.CommandText = SQL;
                con.Open();
                UnitID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return UnitID;
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
                SqlValues += this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["CollectionSpecimenID"].ToString() + ", ";
                switch (AliasForTable)
                {
                    case "IdentificationUnit":
                        TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group"].ToString();
                        if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
                            LastIdentification = TaxonomicGroup;
                        else
                            LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString();
                        DisplayOrder = 1;
                        break;
                    case "SecondUnit":
                        TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
                        if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
                            LastIdentification = TaxonomicGroup;
                        else
                            LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
                        DisplayOrder = 2;
                        break;
                }
                SqlColumns += " LastIdentificationCache, ";
                SqlValues += " '" + LastIdentification + "', ";
                SqlColumns += " DisplayOrder, ";
                SqlValues += " " + DisplayOrder.ToString() + ", ";
            }
            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > Index)
            {
                System.Data.DataRow R = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index];
                foreach (System.Data.DataColumn C in this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns)
                {
                    DiversityCollection.Forms.GridModeQueryField GMQF = this.GridModeGetQueryField(C.ColumnName);
                    if (GMQF.AliasForTable == AliasForTable &&
                        !this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][GMQF.AliasForColumn].Equals(System.DBNull.Value))
                    {
                        SqlColumns += GMQF.Column + ", ";
                        SqlValues += "'" + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][GMQF.AliasForColumn].ToString() + "', ";
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
      
        #endregion

        #region Handling the visibility of the columns in the grid

        /// <summary>
        /// setting the visibility of the columns in the datagrid based on the definitions in the query fields
        /// </summary>
        private void GridModeSetColumnVisibility()
        {
            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
            {
                try
                {
                    if (this.GridModeColumnList.Contains(C.DataPropertyName))
                        C.Visible = true;
                    else
                    { 
                        if (C.Visible)
                            C.Visible = false; 
                    }
                }
                catch { }
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
        /// the grid mode field as specified in the treeViewData
        /// </summary>
        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> GridModeQueryFields
        {
            get
            {
                if (this._GridModeQueryFields == null)
                {
                    this._GridModeQueryFields = new List<Forms.GridModeQueryField>();
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
                    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility.Length == 0)
                    {
                        foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                        {
                            if (!Q.IsHidden)
                            {
                                if (Q.IsVisible) Visibility += "1";
                                else Visibility += "0";
                            }
                        }
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility = Visibility;
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                    }
                    else
                    {
                        Visibility = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility;
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
        /// All fields that exist in the database view FirstLinesUnit
        /// but not in the selection tree
        /// </summary>
        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> GridModeHiddenQueryFields
        {
            get
            {
                if (this._GridModeHiddenQueryFields == null)
                {
                    this._GridModeHiddenQueryFields = new List<Forms.GridModeQueryField>();

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


                        #region Analysis

                        //DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber = new Forms.GridModeQueryField();
                        //Q_AnalysisNumber.Table = "IdentificationUnitAnalysis";
                        //Q_AnalysisNumber.Column = "AnalysisNumber";
                        //Q_AnalysisNumber.AliasForColumn = "Analysis_number";
                        //Q_AnalysisNumber.AliasForTable = "IdentificationUnitAnalysis";
                        //Q_AnalysisNumber.IsVisible = false;
                        //Q_AnalysisNumber.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_0 = new Forms.GridModeQueryField();
                        Q_AnalysisID_0.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_0.Column = "AnalysisID";
                        Q_AnalysisID_0.AliasForColumn = "AnalysisID_0";
                        Q_AnalysisID_0.AliasForTable = "Analysis_0";
                        Q_AnalysisID_0.IsVisible = false;
                        Q_AnalysisID_0.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_0);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_1 = new Forms.GridModeQueryField();
                        Q_AnalysisID_1.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_1.Column = "AnalysisID";
                        Q_AnalysisID_1.AliasForColumn = "AnalysisID_1";
                        Q_AnalysisID_1.AliasForTable = "Analysis_1";
                        Q_AnalysisID_1.IsVisible = false;
                        Q_AnalysisID_1.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_1);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_2 = new Forms.GridModeQueryField();
                        Q_AnalysisID_2.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_2.Column = "AnalysisID";
                        Q_AnalysisID_2.AliasForColumn = "AnalysisID_2";
                        Q_AnalysisID_2.AliasForTable = "Analysis_2";
                        Q_AnalysisID_2.IsVisible = false;
                        Q_AnalysisID_2.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_2);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_3 = new Forms.GridModeQueryField();
                        Q_AnalysisID_3.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_3.Column = "AnalysisID";
                        Q_AnalysisID_3.AliasForColumn = "AnalysisID_3";
                        Q_AnalysisID_3.AliasForTable = "Analysis_3";
                        Q_AnalysisID_3.IsVisible = false;
                        Q_AnalysisID_3.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_3);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_4 = new Forms.GridModeQueryField();
                        Q_AnalysisID_4.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_4.Column = "AnalysisID";
                        Q_AnalysisID_4.AliasForColumn = "AnalysisID_4";
                        Q_AnalysisID_4.AliasForTable = "Analysis_4";
                        Q_AnalysisID_4.IsVisible = false;
                        Q_AnalysisID_4.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_4);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_5 = new Forms.GridModeQueryField();
                        Q_AnalysisID_5.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_5.Column = "AnalysisID";
                        Q_AnalysisID_5.AliasForColumn = "AnalysisID_5";
                        Q_AnalysisID_5.AliasForTable = "Analysis_5";
                        Q_AnalysisID_5.IsVisible = false;
                        Q_AnalysisID_5.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_5);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_6 = new Forms.GridModeQueryField();
                        Q_AnalysisID_6.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_6.Column = "AnalysisID";
                        Q_AnalysisID_6.AliasForColumn = "AnalysisID_6";
                        Q_AnalysisID_6.AliasForTable = "Analysis_6";
                        Q_AnalysisID_6.IsVisible = false;
                        Q_AnalysisID_6.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_6);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_7 = new Forms.GridModeQueryField();
                        Q_AnalysisID_7.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_7.Column = "AnalysisID";
                        Q_AnalysisID_7.AliasForColumn = "AnalysisID_7";
                        Q_AnalysisID_7.AliasForTable = "Analysis_7";
                        Q_AnalysisID_7.IsVisible = false;
                        Q_AnalysisID_7.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_7);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_8 = new Forms.GridModeQueryField();
                        Q_AnalysisID_8.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_8.Column = "AnalysisID";
                        Q_AnalysisID_8.AliasForColumn = "AnalysisID_8";
                        Q_AnalysisID_8.AliasForTable = "Analysis_8";
                        Q_AnalysisID_8.IsVisible = false;
                        Q_AnalysisID_8.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_8);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisID_9 = new Forms.GridModeQueryField();
                        Q_AnalysisID_9.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisID_9.Column = "AnalysisID";
                        Q_AnalysisID_9.AliasForColumn = "AnalysisID_9";
                        Q_AnalysisID_9.AliasForTable = "Analysis_9";
                        Q_AnalysisID_9.IsVisible = false;
                        Q_AnalysisID_9.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisID_9);



                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_0 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_0.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_0.Column = "AnalysisNumber";
                        Q_AnalysisNumber_0.AliasForColumn = "Analysis_number_0";
                        Q_AnalysisNumber_0.AliasForTable = "Analysis_0";
                        Q_AnalysisNumber_0.IsVisible = false;
                        Q_AnalysisNumber_0.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_0);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_1 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_1.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_1.Column = "AnalysisNumber";
                        Q_AnalysisNumber_1.AliasForColumn = "Analysis_number_1";
                        Q_AnalysisNumber_1.AliasForTable = "Analysis_1";
                        Q_AnalysisNumber_1.IsVisible = false;
                        Q_AnalysisNumber_1.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_1);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_2 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_2.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_2.Column = "AnalysisNumber";
                        Q_AnalysisNumber_2.AliasForColumn = "Analysis_number_2";
                        Q_AnalysisNumber_2.AliasForTable = "Analysis_2";
                        Q_AnalysisNumber_2.IsVisible = false;
                        Q_AnalysisNumber_2.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_2);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_3 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_3.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_3.Column = "AnalysisNumber";
                        Q_AnalysisNumber_3.AliasForColumn = "Analysis_number_3";
                        Q_AnalysisNumber_3.AliasForTable = "Analysis_3";
                        Q_AnalysisNumber_3.IsVisible = false;
                        Q_AnalysisNumber_3.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_3);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_4 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_4.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_4.Column = "AnalysisNumber";
                        Q_AnalysisNumber_4.AliasForColumn = "Analysis_number_4";
                        Q_AnalysisNumber_4.AliasForTable = "Analysis_4";
                        Q_AnalysisNumber_4.IsVisible = false;
                        Q_AnalysisNumber_4.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_4);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_5 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_5.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_5.Column = "AnalysisNumber";
                        Q_AnalysisNumber_5.AliasForColumn = "Analysis_number_5";
                        Q_AnalysisNumber_5.AliasForTable = "Analysis_5";
                        Q_AnalysisNumber_5.IsVisible = false;
                        Q_AnalysisNumber_5.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_5);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_6 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_6.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_6.Column = "AnalysisNumber";
                        Q_AnalysisNumber_6.AliasForColumn = "Analysis_number_6";
                        Q_AnalysisNumber_6.AliasForTable = "Analysis_6";
                        Q_AnalysisNumber_6.IsVisible = false;
                        Q_AnalysisNumber_6.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_6);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_7 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_7.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_7.Column = "AnalysisNumber";
                        Q_AnalysisNumber_7.AliasForColumn = "Analysis_number_7";
                        Q_AnalysisNumber_7.AliasForTable = "Analysis_7";
                        Q_AnalysisNumber_7.IsVisible = false;
                        Q_AnalysisNumber_7.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_7);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_8 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_8.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_8.Column = "AnalysisNumber";
                        Q_AnalysisNumber_8.AliasForColumn = "Analysis_number_8";
                        Q_AnalysisNumber_8.AliasForTable = "Analysis_8";
                        Q_AnalysisNumber_8.IsVisible = false;
                        Q_AnalysisNumber_8.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_8);

                        DiversityCollection.Forms.GridModeQueryField Q_AnalysisNumber_9 = new Forms.GridModeQueryField();
                        Q_AnalysisNumber_9.Table = "IdentificationUnitAnalysis";
                        Q_AnalysisNumber_9.Column = "AnalysisNumber";
                        Q_AnalysisNumber_9.AliasForColumn = "Analysis_number_9";
                        Q_AnalysisNumber_9.AliasForTable = "Analysis_9";
                        Q_AnalysisNumber_9.IsVisible = false;
                        Q_AnalysisNumber_9.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_AnalysisNumber_9);

                        //DiversityCollection.Forms.GridModeQueryField Q_AnalysisID = new Forms.GridModeQueryField();
                        //Q_AnalysisID.Table = "IdentificationUnitAnalysis";
                        //Q_AnalysisID.Column = "AnalysisID";
                        //Q_AnalysisID.AliasForColumn = "AnalysisID";
                        //Q_AnalysisID.AliasForTable = "IdentificationUnitAnalysis";
                        //Q_AnalysisID.IsVisible = false;
                        //Q_AnalysisID.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_AnalysisID);
                        
                        #endregion

                        //DiversityCollection.Forms.GridModeQueryField Q_SecondUnitID = new Forms.GridModeQueryField();
                        //Q_SecondUnitID.Table = "Identification";
                        //Q_SecondUnitID.Column = "IdentificationUnitID";
                        //Q_SecondUnitID.AliasForColumn = "_SecondUnitID";
                        //Q_SecondUnitID.AliasForTable = "SecondUnit";
                        //Q_SecondUnitID.IsVisible = false;
                        //Q_SecondUnitID.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_SecondUnitID);

                        //DiversityCollection.Forms.GridModeQueryField Q_SecondSequence = new Forms.GridModeQueryField();
                        //Q_SecondSequence.Table = "Identification";
                        //Q_SecondSequence.Column = "IdentificationSequence";
                        //Q_SecondSequence.AliasForColumn = "_SecondSequence";
                        //Q_SecondSequence.AliasForTable = "SecondUnitIdentification";
                        //Q_SecondSequence.IsVisible = false;
                        //Q_SecondSequence.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_SecondSequence);

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


                        DiversityCollection.Forms.GridModeQueryField Q_CoordinatesLocationNotes = new Forms.GridModeQueryField();
                        Q_CoordinatesLocationNotes.Table = "CollectionEventLocalisation";
                        Q_CoordinatesLocationNotes.Column = "LocationNotes";
                        Q_CoordinatesLocationNotes.AliasForColumn = "_CoordinatesLocationNotes";
                        Q_CoordinatesLocationNotes.AliasForTable = "CoordinatesWGS84";
                        Q_CoordinatesLocationNotes.Restriction = "LocalisationSystemID=8";
                        Q_CoordinatesLocationNotes.IsVisible = false;
                        Q_CoordinatesLocationNotes.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_CoordinatesLocationNotes);


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

                        //DiversityCollection.Forms.GridModeQueryField Q_SecondUnitFamilyCache = new Forms.GridModeQueryField();
                        //Q_SecondUnitFamilyCache.Table = "Identification";
                        //Q_SecondUnitFamilyCache.Column = "FamilyCache";
                        //Q_SecondUnitFamilyCache.AliasForColumn = "_SecondUnitFamilyCache";
                        //Q_SecondUnitFamilyCache.AliasForTable = "SecondUnitIdentification";
                        //Q_SecondUnitFamilyCache.IsVisible = false;
                        //Q_SecondUnitFamilyCache.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_SecondUnitFamilyCache);

                        //DiversityCollection.Forms.GridModeQueryField Q_SecondUnitOrderCache = new Forms.GridModeQueryField();
                        //Q_SecondUnitOrderCache.Table = "Identification";
                        //Q_SecondUnitOrderCache.Column = "OrderCache";
                        //Q_SecondUnitOrderCache.AliasForColumn = "_SecondUnitOrderCache";
                        //Q_SecondUnitOrderCache.AliasForTable = "SecondUnitIdentification";
                        //Q_SecondUnitOrderCache.IsVisible = false;
                        //Q_SecondUnitOrderCache.IsHidden = true;
                        //this._GridModeHiddenQueryFields.Add(Q_SecondUnitOrderCache);

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

        private Forms.GridModeQueryField GridModeQueryFieldOfNode(System.Windows.Forms.TreeNode N)
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

        public int? CollectionSpecimenID
        {
            get
            {
                return this._SpecimenID;
            }
        }

        public int UnitID { get { return this._UnitID; } }

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

        public string IDlist
        {
            get
            {
                string List = "";
                foreach (int i in this._IDs)
                {
                    List += i.ToString() + ", ";
                }
                if (List.Length > 0) 
                    List = List.Substring(0, List.Length - 2);
                return List;
            }
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

        private void buttonHeaderShowTree_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonHeaderShowTree.BackColor == System.Drawing.SystemColors.Control)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.buttonHeaderShowTree.BackColor = System.Drawing.Color.Red;
                    this.userControlEventSeriesTree.initControl(this._IDs, "IdentificationUnitList", "IdentificationUnitID", this.dataGridView, this.dataSetIdentifictionUnitGridMode.FirstLinesUnit, toolStripButtonSearchSpecimen_Click);
                    //this.fillDataSetSeries();
                    //this.initTree();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                else this.buttonHeaderShowTree.BackColor = System.Drawing.SystemColors.Control;

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
                if (this.ShowColumnSelectionTree || this.ShowImagesSpecimen || this.ShowDataTree)
                {
                    this.splitContainerMain.Panel1Collapsed = false;

                    if (this.ShowImagesSpecimen) this.splitContainerTreeView.Panel2Collapsed = false;
                    else this.splitContainerTreeView.Panel2Collapsed = true;

                    if (this.ShowDataTree || this.ShowColumnSelectionTree)
                        this.splitContainerTreeView.Panel1Collapsed = false;
                    else this.splitContainerTreeView.Panel1Collapsed = true;

                    if (this.ShowColumnSelectionTree) this.splitContainerTrees.Panel1Collapsed = false;
                    else this.splitContainerTrees.Panel1Collapsed = true;

                    if (this.ShowDataTree) this.splitContainerTrees.Panel2Collapsed = false;
                    else this.splitContainerTrees.Panel2Collapsed = true;
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

        private bool ShowDataTree
        {
            get
            {
                if (this.buttonHeaderShowTree.BackColor == System.Drawing.SystemColors.Control)
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

        #region Replace, Insert, Append, Clear Function for a single Column

        private void buttonMarkWholeColumn_Click(object sender, EventArgs e)
        {
            try
            {
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                if (ColumnName == "CollectionSpecimenID") return;
                System.Windows.Forms.DataGridViewColumn C = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                int iLines = this.dataGridView.Rows.Count;
                if (this.dataGridView.AllowUserToAddRows)
                    iLines--;
                for (int i = 0; i < iLines; i++)
                {
                    this.dataGridView.Rows[i].Cells[this.dataGridView.SelectedCells[0].ColumnIndex].Selected = true;
                }
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
            catch  { }
            this.enableReplaceButtons(true);
        }

        private void contextMenuStripDataGrid_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if ((System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.CommaSeparatedValue)
                || System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.Text))
                && e.ClickedItem.Name == "toolStripMenuItemCopyFromClipboard")
            {
                // finding the top row
                int IndexTopRow = this.dataGridView.Rows.Count;
                if (this.dataGridView.SelectedCells.Count > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                        if (IndexTopRow > C.RowIndex) IndexTopRow = C.RowIndex;
                }

                // parsing the content of the clipboard
                System.Collections.Generic.List < System.Collections.Generic.List < string> > ClipBoardValues = new List<List<string>>();
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lineSeparators = new string[] { "\t" };
                string ClipBoardText = System.Windows.Forms.Clipboard.GetText();
                string[] ClipBoardList = ClipBoardText.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < ClipBoardList.Length - 1; i++)
                {
                    System.Collections.Generic.List<string> ClipBoardValueStrings = new List<string>();
                    string[] ClipBoardListStrings = ClipBoardList[i].Split(lineSeparators, StringSplitOptions.None);
                    for (int ii = 0; ii < ClipBoardListStrings.Length; ii++)
                        ClipBoardValueStrings.Add(ClipBoardListStrings[ii]);
                    ClipBoardValues.Add(ClipBoardValueStrings);
                }

                // transfering the values
                try
                {
                    // the colums for the transfer
                    System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = new List<DataGridViewColumn>();
                    int CurrentDisplayIndex = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DisplayIndex;
                    for (int i = 0; i < ClipBoardValues[0].Count; i++)
                    {
                        foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                        {
                            if (C.Visible && C.DisplayIndex == CurrentDisplayIndex + i)
                            {
                                GridColums.Add(C);
                                break;
                            }
                        }
                    }
                    for (int ii = 0; ii < GridColums.Count; ii++) // the columns
                    {
                        for (int i = 0; i < ClipBoardValues.Count; i++) // the rows
                        {
                            if (DiversityCollection.Forms.FormGridFunctions.ValueIsValid(this.dataGridView, GridColums[ii].Index, ClipBoardValues[i][ii]))
                            {
                                this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].Value = ClipBoardValues[i][ii];
                                this.checkForMissingAndDefaultValues(this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index], true);
                            }
                            else
                            {
                                string Message = ClipBoardValues[i][ii] + " is not a valid value for "
                                    + this.dataGridView.Columns[GridColums[ii].Index].DataPropertyName
                                    + "\r\n\r\nDo you want to try to insert the other values?";
                                if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo) == DialogResult.No)
                                    break;
                            }
                            if (i + IndexTopRow + 3 > this.dataGridView.Rows.Count)
                                continue;
                        }
                    }
                }
                catch { }
            }
            else if (e.ClickedItem.Name == "toolStripMenuItemCopyFromClipboard")
            {
                System.Windows.Forms.MessageBox.Show("Only text and spreadsheet values can be copied");
                return;
            }
        }

        private void enableReplaceButtons(bool IsEnabled)
        {
            System.Collections.Generic.List<int> ColList = new List<int>();
            for (int i = 0; i < this.dataGridView.SelectedCells.Count; i++)
                if (!ColList.Contains(this.dataGridView.SelectedCells[i].ColumnIndex)) ColList.Add(this.dataGridView.SelectedCells[i].ColumnIndex);
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
            if (!IsEnabled) this.labelGridViewReplaceColumnName.Text = "?";
            if (this.dataGridView.SelectedCells.Count > 0 && ColList.Count == 1)
            {
                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Combo
                        = (System.Windows.Forms.DataGridViewComboBoxColumn)this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                    if (Combo.DisplayMember == "DisplayText" && Combo.ValueMember == "Code")
                    {
                        this.comboBoxReplace.Width = 100;
                        this.comboBoxReplaceWith.Width = 100;
                        System.Object O = Combo.DataSource;
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)O;
                        DiversityCollection.Datasets.DataSetIdentifictionUnitGridMode DS = (DiversityCollection.Datasets.DataSetIdentifictionUnitGridMode)BS.DataSource;
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
            }
            this.setReplaceOptions();
        }

        private void buttonGridModeReplace_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit, 0, this.ReplaceOptionStatus);
            //this.ColumnValues_ReplaceInsertClear();
        }

        private void buttonGridModeInsert_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit, 0, this.ReplaceOptionStatus);
            //this.ColumnValues_ReplaceInsertClear();
        }

        private void buttonGridModeAppend_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit, 0, this.ReplaceOptionStatus);
            //this.ColumnValues_ReplaceInsertClear();
        }

        private void buttonGridModeRemove_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit, 0, this.ReplaceOptionStatus);
            //this.ColumnValues_ReplaceInsertClear();
        }

        private void buttonGridModeUndo_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
            {
                this.UndoChanges(R.Index);
            }
        }

        private void buttonGridModeUndoSingleLine_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedCells.Count > 0)
            {
                this.UndoChanges(this.dataGridView.SelectedCells[0].RowIndex);
            }
        }

        private void UndoChanges(int RowIndex)
        {
            try
            {
                int UnitID;
                if (int.TryParse(this.dataGridView.Rows[RowIndex].Cells[0].Value.ToString(), out UnitID))
                {
                    System.Data.DataRow Rori = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfLine(UnitID)];
                    if (Rori.RowState != DataRowState.Unchanged)
                    {
                        Rori.RejectChanges();
                    }
                }
            }
            catch { }
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
            if (this.dataGridView.SelectedCells.Count > 0)
            {
                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Combo
                        = (System.Windows.Forms.DataGridViewComboBoxColumn)this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                    if (Combo.DisplayMember == "DisplayText" && Combo.ValueMember == "Code")
                    {
                        ShowComboBoxes = true;
                    }
                }
            }
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
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                {
                    if (C.Visible)
                        sw.Write(C.DataPropertyName + "\t");
                }
                sw.WriteLine();
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
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

        #region Autocomplete for taxa

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this._UseAsSpecimenTaxonList)
            {
                if (this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].DataPropertyName.ToLower() == "taxonomic_name")
                {
                    if (AutoCompleteTaxa().Count > 1)
                    {
                        TextBox autoText = e.Control as TextBox;
                        if (autoText != null)
                        {
                            autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                            autoText.AutoCompleteCustomSource = this.AutoCompleteTaxa();
                            autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
                else if (this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].DataPropertyName.ToLower().StartsWith("analysis_result_"))
                {
                    if (AutoCompleteAnalysis().Count > 1)
                    {
                        TextBox autoText = e.Control as TextBox;
                        if (autoText != null)
                        {
                            autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                            autoText.AutoCompleteCustomSource = this.AutoCompleteAnalysis();
                            autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
            }
            else
            {
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
        }

        private AutoCompleteStringCollection AutoCompleteTaxa()
        {
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
            if (this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].DataPropertyName.ToLower() == "taxonomic_name")
            {
                if (this.dataGridView.CurrentCell.Value.ToString().Length > 2)
                {
                    string TaxonomicGroup = "";
                    System.Windows.Forms.DataGridViewRow R = this.dataGridView.Rows[this.dataGridView.CurrentCell.RowIndex];
                    foreach (System.Windows.Forms.DataGridViewCell C in R.Cells)
                    {
                        if (this.dataGridView.Columns[C.ColumnIndex].DataPropertyName.ToLower() == "taxonomic_group")
                        {
                            TaxonomicGroup = C.Value.ToString();
                            break;
                        }
                    }
                    string SQL = "SELECT I.TaxonomicName " +
                        "FROM  IdentificationUnit AS U INNER JOIN " +
                        "Identification AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND U.IdentificationUnitID = I.IdentificationUnitID " +
                        "WHERE (I.NameURI <> '') AND (U.TaxonomicGroup = N'" + TaxonomicGroup + "') " +
                        "GROUP BY I.TaxonomicName " +
                        "HAVING (I.TaxonomicName LIKE N'" + this.dataGridView.CurrentCell.Value.ToString() + "%') " +
                        "ORDER BY I.TaxonomicName";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow Rt in dt.Rows)
                    {
                        DataCollection.Add(Rt[0].ToString());
                    }
                }
            }
            return DataCollection;
        }

        private AutoCompleteStringCollection AutoCompleteAnalysis()
        {
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
            if (this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].DataPropertyName.ToLower().StartsWith("analysis_result_"))
            {
                // Getting the type of the analysis
                string AnalysisColumn = this.dataGridView.Columns[this.dataGridView.CurrentCell.ColumnIndex].DataPropertyName;
                int AnalysisColumnPos = int.Parse(AnalysisColumn.Substring("Analysis_result_".Length));
                int AnalysisID = this._AnalysisList[AnalysisColumnPos].AnalysisID;
                string SQL = "SELECT AnalysisResult " +
                    " FROM  AnalysisResult AS a " +
                    " WHERE AnalysisID =  " + AnalysisID.ToString() +
                    " ORDER BY a.AnalysisResult";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow Rt in dt.Rows)
                    {
                        DataCollection.Add(Rt[0].ToString());
                    }
                }
            }
            return DataCollection;
        }

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

        //#region Tree

        //#region Data

        //private void saveDataSeries()
        //{
        //    try
        //    {
        //        foreach (System.Data.DataRow R in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //        {
        //            R.EndEdit();
        //        }
        //        if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0
        //            && this.dataSetCollectionEventSeries.CollectionEventSeries.DataSet.HasChanges())
        //        {
        //            this._sqlDataAdapterEventSeries.Update(this.dataSetCollectionEventSeries.CollectionEventSeries);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void fillDataSetSeries()
        //{
        //    this.saveDataSeries();
        //    // init the sql adapter
        //    try
        //    {
        //        string WhereClause = "";// WHERE SeriesID = 0";
        //        //this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeries, DiversityCollection.CollectionSpecimen.SqlEventSeries + WhereClause, this.dataSetCollectionEventSeries.CollectionEventSeries);
        //        // clear the table
        //        this.dataSetCollectionEventSeries.CollectionEventSeries.Clear();
        //        // fill tables
        //        // EventSeries
        //        string SQL = "SELECT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, Description, Notes FROM CollectionEventSeries WHERE SeriesID IN(SELECT SeriesID FROM dbo.FirstLinesSeries('" + this._sIDs + "'))";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.Fill(this.dataSetCollectionEventSeries.CollectionEventSeries);
        //        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeries, SQL, this.dataSetCollectionEventSeries.CollectionEventSeries);

        //        // the collection events
        //        if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
        //        {
        //            SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesEvent + "WHERE SeriesID IN (";
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                SQL += r["SeriesID"].ToString() + ", ";
        //            }
        //        }
        //        if (SQL.EndsWith(", "))
        //            SQL = SQL.Substring(0, SQL.Length - 2) + ") ORDER BY CollectionDate";
        //        else
        //            SQL = SQL.Substring(0, SQL.Length - 1) + ") ORDER BY CollectionDate";
        //        this.dataSetCollectionEventSeries.CollectionEventList.Clear();
        //        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesEvent, SQL, this.dataSetCollectionEventSeries.CollectionEventList);

        //        // Adding the events without a series
        //        SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesEvent + "WHERE SeriesID IS NULL " +
        //            "AND CollectionEventID IN (SELECT CollectionEventID FROM CollectionSpecimen " +
        //            "WHERE NOT CollectionEventID IS NULL AND CollectionSpecimenID IN (" + this._sIDs +"))";
        //        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesEvent, SQL, this.dataSetCollectionEventSeries.CollectionEventList);

        //        if (this.dataSetCollectionEventSeries.CollectionEventList.Rows.Count > 0)
        //        {
        //            // Specimen
        //            SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesSpecimen + " WHERE CollectionEventID IN (";
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventList.Rows)
        //            {
        //                SQL += r["CollectionEventID"].ToString() + ", ";
        //            }
        //            SQL = SQL.Substring(0, SQL.Length - 2) + ") ORDER BY AccessionNumber";
        //            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesSpecimen, SQL, this.dataSetCollectionEventSeries.CollectionSpecimenList);

        //            // Adding the specimen without an event
        //            SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesSpecimen + " WHERE CollectionEventID IS NULL " +
        //                "AND CollectionSpecimenID IN (" + this._sIDs + ")";
        //            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesSpecimen, SQL, this.dataSetCollectionEventSeries.CollectionSpecimenList);

        //            // Unit
        //            if (this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows.Count > 0)
        //            {
        //                SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesUnit + " WHERE CollectionSpecimenID IN (";
        //                foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
        //                {
        //                    SQL += r["CollectionSpecimenID"].ToString() + ", ";
        //                }
        //                SQL = SQL.Substring(0, SQL.Length - 2) + ")";
        //                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesUnit, SQL, this.dataSetCollectionEventSeries.IdentificationUnitList);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    //this.fillEventSeriesImages();
        //}

        //#endregion

        //#region building the tree

        //private void initTree()
        //{
        //    this.treeViewData.ImageList = DiversityCollection.Specimen.ImageList;
        //    this.treeViewData.Visible = false;
        //    this.treeViewData.Nodes.Clear();
        //    try
        //    {
        //        try
        //        {
        //            if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
        //            {
        //                this.addEventSeries();
        //                this.setColorOfNodesToNotPresent();
        //                foreach (System.Windows.Forms.TreeNode N in this.PresentUnitNodes)
        //                    this.setColorOfParentNode(N);
        //            }
        //            else if (this.dataSetCollectionEventSeries.CollectionEventList.Rows.Count > 0)
        //            {
        //            }
        //            else if (this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows.Count > 0)
        //            {
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //        //this.treeViewData.ExpandAll();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    finally
        //    {
        //        this.treeViewData.Visible = true;
        //    }
        //}

        //private void setColorOfNodesToNotPresent()
        //{
        //    foreach (System.Windows.Forms.TreeNode N in this.treeViewData.Nodes)
        //    {
        //        N.ForeColor = this._ColorOfNotPresentNodes;
        //        //N.ImageIndex++;
        //        this.setColorOfChildNodesToNotPresent(N);
        //    }
        //}

        //private void setColorOfChildNodesToNotPresent(System.Windows.Forms.TreeNode N)
        //{
        //    foreach (System.Windows.Forms.TreeNode NChild in N.Nodes)
        //    {
        //        NChild.ForeColor = this._ColorOfNotPresentNodes;
        //        this.setColorOfChildNodesToNotPresent(NChild);
        //    }
        //}

        //private System.Collections.Generic.List<System.Windows.Forms.TreeNode> PresentUnitNodes
        //{
        //    get
        //    {
        //        System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //        foreach (System.Windows.Forms.TreeNode N in this.treeViewData.Nodes)
        //            this.addPresentUnitNodesToList(ref Nodes, N);
        //        return Nodes;
        //    }
        //}

        //private void addPresentUnitNodesToList(ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes, System.Windows.Forms.TreeNode N)
        //{
        //    if (N.Tag != null)
        //    {
        //        System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //        if (R.Table.TableName == "IdentificationUnitList")
        //        {
        //            string UnitID = R["IdentificationUnitID"].ToString();
        //            System.Data.DataRow[] rr = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Select("IdentificationUnitID = " + UnitID);
        //            if (rr.Length > 0)
        //                Nodes.Add(N);
        //        }
        //    }
        //    if (N.Nodes.Count > 0)
        //    {
        //        foreach (System.Windows.Forms.TreeNode Nchild in N.Nodes)
        //            this.addPresentUnitNodesToList(ref Nodes, Nchild);
        //    }
        //}


        //private void setColorOfParentNode(System.Windows.Forms.TreeNode N)
        //{
        //    N.ForeColor = this._ColorOfNodes;
        //    N.Expand();
        //    //N.ImageIndex--;
        //    //if (N.ForeColor.IsKnownColor && N.ForeColor.Name == "Gray")
        //    //{
        //    //    N.ImageIndex++;
        //    //}
        //    if (N.Parent != null)
        //        this.setColorOfParentNode(N.Parent);
        //}

        //private System.Windows.Forms.TreeNode addEventSeriesSuperiorList()
        //{
        //    System.Windows.Forms.TreeNode ParentNode = new TreeNode();
        //    try
        //    {
        //        if (this.NoLoopInEventSeries())
        //        {
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                if (r["SeriesParentID"].Equals(System.DBNull.Value)/* || r["SeriesParentID"].ToString() == r["SeriesID"].ToString()*/)
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(r), false);
        //                    ParentNode = N;
        //                    this.treeViewData.Nodes.Add(N);
        //                    this.getEventSeriesSuperiorChilds(N, ref ParentNode);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            string SeriesID = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0]["SeriesID"].ToString();
        //            System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + SeriesID);
        //            ParentNode = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(rr[0]), false);
        //            System.Windows.Forms.MessageBox.Show("The event series contains a loop. Please set the series for the collection event");
        //            this.treeViewData.Nodes.Add(ParentNode);
        //        }
        //    }
        //    catch { }

        //    return ParentNode;
        //}

        //private bool NoLoopInEventSeries()
        //{
        //    bool NoLoop = true;
        //    System.Data.DataRow RParent;
        //    System.Data.DataRow RChild;
        //    if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
        //    {
        //        System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID IS NULL");
        //        if (RR.Length > 0)
        //        {
        //            RParent = RR[0];
        //            return true;
        //        }
        //        else
        //        {
        //            RParent = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //        }
        //        if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 1)
        //        {
        //            if (RR.Length > 0)
        //            {
        //                System.Data.DataRow[] RRChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("NOT SeriesParentID IS NULL");
        //                if (RRChild.Length > 0)
        //                    RChild = RRChild[0];
        //                else
        //                    RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //            }
        //            else
        //                RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[1];
        //        }
        //        else
        //            RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //        if (RChild != null && RParent != null)
        //            NoLoop = this.NoLoopInEventSeries(RChild, RParent);
        //    }
        //    return NoLoop;
        //}

        ////private bool NoLoopInEventSeries(System.Data.DataRow rChild, System.Data.DataRow rParent)
        ////{
        ////    bool NoLoop = true;
        ////    try
        ////    {
        ////        int ChildID = int.Parse(rChild["SeriesID"].ToString());
        ////        int ParentID = int.Parse(rParent["SeriesID"].ToString());
        ////        if (ChildID == ParentID)
        ////            return false;
        ////        int? ParentOfParentID = null;
        ////        int iPP = 0;
        ////        if (int.TryParse(rParent["SeriesParentID"].ToString(), out iPP))
        ////            ParentOfParentID = iPP;
        ////        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentID);
        ////        if (rr.Length > 0)
        ////        {
        ////            if (!rr[0]["SeriesParentID"].Equals(System.DBNull.Value))
        ////            {
        ////                while (ParentOfParentID != null)
        ////                {
        ////                    if (ParentOfParentID == ChildID)
        ////                    {
        ////                        NoLoop = false;
        ////                        break;
        ////                    }
        ////                    System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentOfParentID);
        ////                    if (RR.Length > 0)
        ////                    {
        ////                        if (RR[0]["SeriesParentID"].Equals(System.DBNull.Value))
        ////                            break;
        ////                        else
        ////                        {
        ////                            ParentOfParentID = int.Parse(RR[0]["SeriesParentID"].ToString());
        ////                        }
        ////                    }
        ////                    else break;
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch { }
        ////    return NoLoop;
        ////}

        //private System.Data.DataRow CollectionEventSeriesDataRowFromEventDataset(System.Data.DataRow DataRowFromSpecimenDataset)
        //{
        //    System.Data.DataRow Rseries = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //    foreach (System.Data.DataRow R in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //    {
        //        if (R["SeriesID"].ToString() == DataRowFromSpecimenDataset["SeriesID"].ToString())
        //        {
        //            Rseries = R;
        //            break;
        //        }
        //    }
        //    return Rseries;
        //}

        //private void getEventSeriesSuperiorChilds(System.Windows.Forms.TreeNode Node, ref System.Windows.Forms.TreeNode ParentNode)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string SeriesParentID = rParent["SeriesID"].ToString();
        //        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID = " + rParent["SeriesID"].ToString(), "DateStart");
        //        foreach (System.Data.DataRow rO in rr)
        //        {
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                if (rO["SeriesID"].ToString() == r["SeriesID"].ToString())
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(r), false);
        //                    Node.Nodes.Add(N);
        //                    this.getEventSeriesSuperiorChilds(N, ref ParentNode);
        //                    ParentNode = N;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void addEventSeries()
        //{
        //    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //    {
        //        if (r["SeriesParentID"].Equals(System.DBNull.Value))
        //        {
        //            DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //            this.treeViewData.Nodes.Add(N);
        //            this.getEventSeriesChilds(N);
        //            this.getEventSeriesEvents(N);
        //        }
        //    }
        //    this.getEvents();
        //    this.getSpecimen();
        //    this.addHierarchyUnits();
        //    // if a mistake occurs, i.e. a loop the system will take one eventseries and add the rest of the tree
        //    //if (this.treeViewData.Nodes.Count == 0)
        //    //{
        //    //    this.addEventSeriesSuperiorList();
        //    //    System.Windows.Forms.TreeNode EventNode = this.OverviewHierarchyEventNode;
        //    //    this.treeViewData.Nodes[0].Nodes.Add(EventNode);
        //    //    System.Windows.Forms.TreeNode SpecimenNode = this.OverviewHierarchySpecimenNode;
        //    //    EventNode.Nodes.Add(SpecimenNode);
        //    //}
        //    //this.replaceEventListNode();
        //}

        //private void getEventSeriesChilds(System.Windows.Forms.TreeNode Node)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string SeriesParentID = rParent["SeriesID"].ToString();
        //        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID = " + rParent["SeriesID"].ToString(), "DateStart");
        //        foreach (System.Data.DataRow rO in rr)
        //        {
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                if (rO["SeriesID"].ToString() == r["SeriesID"].ToString())
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                    Node.Nodes.Add(N);
        //                    this.getEventSeriesChilds(N);
        //                    this.getEventSeriesEvents(N);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEvents(System.Windows.Forms.TreeNode Node)
        //{
        //    try
        //    {
        //        if (Node != null)
        //        {
        //            System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //            string SeriesID = rParent["SeriesID"].ToString();
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventList.Rows)
        //            {
        //                if (r["SeriesID"].ToString() == SeriesID)
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                    Node.Nodes.Add(N);
        //                    this.getEventSeriesEventSpecimen(N);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            System.Data.DataRow rEvent = this.dataSetCollectionEventSeries.CollectionEventList.Rows[0];
        //            DiversityCollection.HierarchyNode N = new HierarchyNode(rEvent, false);
        //            this.treeViewData.Nodes.Add(N);
        //            this.getEventSeriesEventSpecimen(N);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEvents()
        //{
        //    try
        //    {
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventList.Rows)
        //            {
        //                if (r["SeriesID"].Equals(System.DBNull.Value))
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                    this.treeViewData.Nodes.Add(N);
        //                    this.getEventSeriesEventSpecimen(N);
        //                }
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimen(System.Windows.Forms.TreeNode Node)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionEventID = rParent["CollectionEventID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
        //        {
        //            if (r["CollectionEventID"].ToString() == CollectionEventID)
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //            }
        //        }
        //        //this.addHierarchyUnits();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getSpecimen()
        //{
        //    try
        //    {
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
        //        {
        //            if (r["CollectionEventID"].Equals(System.DBNull.Value))
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                this.treeViewData.Nodes.Add(N);
        //            }
        //        }
        //        //this.addHierarchyUnits();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimen(System.Windows.Forms.TreeNode Node, int MainSpecimenID)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionEventID = rParent["CollectionEventID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
        //        {
        //            if (r["CollectionEventID"].ToString() == CollectionEventID && r["CollectionSpecimenID"].ToString() != MainSpecimenID.ToString())
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimenUnits(System.Windows.Forms.TreeNode Node)
        //{
        //    if (!this.ShowUnit) return;
        //    try
        //    {
        //        if (Node.Nodes.Count > 0)
        //        {
        //            foreach (System.Windows.Forms.TreeNode NChild in Node.Nodes)
        //            {
        //                System.Data.DataRow RChildNode = (System.Data.DataRow)NChild.Tag;
        //                if (RChildNode.Table.TableName == "IdentificationUnitList")
        //                {
        //                    this.getEventSeriesEventSpecimenUnitChilds(NChild);
        //                    return;
        //                }
        //            }
        //        }

        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionSpecimenID = rParent["CollectionSpecimenID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.IdentificationUnitList.Rows)
        //        {
        //            if (r["CollectionSpecimenID"].ToString() == CollectionSpecimenID
        //                && r["RelatedUnitID"].Equals(System.DBNull.Value))
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //                this.getEventSeriesEventSpecimenUnitChilds(N);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimenUnitChilds(System.Windows.Forms.TreeNode Node)
        //{
        //    if (!this.ShowUnit) return;
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionSpecimenID = rParent["CollectionSpecimenID"].ToString();
        //        string SubstrateID = rParent["IdentificationUnitID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.IdentificationUnitList.Rows)
        //        {
        //            if (r["CollectionSpecimenID"].ToString() == CollectionSpecimenID && r["RelatedUnitID"].ToString() == SubstrateID && SubstrateID != null)
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //                this.getEventSeriesEventSpecimenUnitChilds(N);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //#endregion

        ////private void replaceEventListNode()
        ////{
        ////    System.Collections.Generic.List<System.Windows.Forms.TreeNode> EventNodes = new List<TreeNode>();
        ////    this.getHierarchyNodes(null, "CollectionEventList", this.treeViewData, ref EventNodes);
        ////    this.treeViewData.CollapseAll();
        ////    foreach (System.Windows.Forms.TreeNode N in EventNodes)
        ////    {
        ////        System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        ////        if (R["CollectionEventID"].ToString() == this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString())
        ////        {
        ////            System.Windows.Forms.TreeNode ParentNode = N.Parent;
        ////            N.Remove();
        ////            System.Windows.Forms.TreeNode EventNode = this.OverviewHierarchyEventNode;
        ////            ParentNode.Nodes.Add(EventNode);
        ////            System.Windows.Forms.TreeNode SpecimenNode = this.OverviewHierarchySpecimenNode;
        ////            int CollectionSpecimenID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString());
        ////            this.getEventSeriesEventSpecimen(EventNode, CollectionSpecimenID);
        ////            EventNode.Nodes.Add(SpecimenNode);
        ////            SpecimenNode.Expand();
        ////            this.treeViewData.SelectedNode = SpecimenNode;
        ////            this.treeViewData.SelectedNode = null;
        ////            EventNode.ExpandAll();
        ////            //ParentNode.ExpandAll();
        ////            return;
        ////        }
        ////    }
        ////}

        ////private void addOverviewHierarchyEventSeriesHierarchy()
        ////{
        ////    if (this.ShowEventSeries)
        ////    {
        ////        //System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        ////        //this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewData, ref Nodes);
        ////        //foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventProperty.Rows)
        ////        //{
        ////        //    DiversityCollection.HierarchyNode NA = new HierarchyNode(R);
        ////        //    NA.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.ForeColor = System.Drawing.Color.Green;
        ////        //    Nodes[0].Nodes.Add(NA);
        ////        //}
        ////    }
        ////}

        ////private void addOverviewHierarchyEventSeries()
        ////{
        ////    if (this.ShowEventSeries)
        ////    {
        ////        //System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        ////        //this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewData, ref Nodes);
        ////        //foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventProperty.Rows)
        ////        //{
        ////        //    DiversityCollection.HierarchyNode NA = new HierarchyNode(R);
        ////        //    NA.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.ForeColor = System.Drawing.Color.Green;
        ////        //    Nodes[0].Nodes.Add(NA);
        ////        //}
        ////    }
        ////}

        //#region Drag & Drop

        //private void treeViewData_DragDrop(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Data.GetDataPresent("DiversityCollection.HierarchyNode", false))
        //        {
        //            Point pt = this.treeViewData.PointToClient(new Point(e.X, e.Y));
        //            TreeNode ParentNode = this.treeViewData.GetNodeAt(pt);
        //            TreeNode ChildNode = (TreeNode)e.Data.GetData("DiversityCollection.HierarchyNode");
        //            System.Data.DataRow rChild = (System.Data.DataRow)ChildNode.Tag;
        //            System.Data.DataRow rParent = (System.Data.DataRow)ParentNode.Tag;
        //            string ChildTable = rChild.Table.TableName;
        //            string ParentTable = rParent.Table.TableName;
        //            string SQL = "";
        //            Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
        //            switch (ChildTable)
        //            {
        //                case "CollectionEventSeries":
        //                    if (ParentTable == "CollectionEventSeries")
        //                    {
        //                        if (this.NoLoopInEventSeries(rChild, rParent))
        //                        {
        //                            rChild["SeriesParentID"] = rParent["SeriesID"];
        //                            this.initTree();
        //                        }
        //                        else
        //                            System.Windows.Forms.MessageBox.Show("This would create a loop in the event series", "Loop", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    }
        //                    else
        //                    {
        //                        System.Windows.Forms.MessageBox.Show("Event series can only be placed within other event series");
        //                        return;
        //                    }
        //                    break;
        //                case "CollectionSpecimenList":
        //                    if (ParentTable == "CollectionEvent")
        //                    {
        //                        SQL = "UPDATE CollectionSpecimen SET CollectionEventID  = " + rParent["CollectionEventID"].ToString() + " WHERE CollectionSpecimenID = " + rChild["CollectionSpecimenID"].ToString();
        //                        cmd.CommandText = SQL;
        //                        c.Open();
        //                        cmd.ExecuteNonQuery();
        //                        c.Close();
        //                        //this.setSpecimen(this.ID);
        //                    }
        //                    else
        //                    {
        //                        System.Windows.Forms.MessageBox.Show("Specimens can only be placed within collection events");
        //                        return;
        //                    }
        //                    break;
        //                case "CollectionEventList":
        //                    if (ParentTable == "CollectionEventSeries")
        //                    {
        //                        rChild["SeriesID"] = rParent["SeriesID"];
        //                        SQL = "UPDATE CollectionEvent SET SeriesID  = " + rParent["SeriesID"].ToString() + " WHERE CollectionEventID = " + rChild["CollectionEventID"].ToString();
        //                        cmd.CommandText = SQL;
        //                        c.Open();
        //                        cmd.ExecuteNonQuery();
        //                        c.Close();
        //                        //this.setSpecimen(this.ID);
        //                    }
        //                    else
        //                    {
        //                        System.Windows.Forms.MessageBox.Show("Collection events can only be placed within event series");
        //                        //this.buildEventSeriesHierarchy();
        //                        //this.setSpecimen(this.ID);
        //                        return;
        //                    }
        //                    break;
        //                //case "CollectionEventSeries":
        //                //    if (ParentTable == "CollectionEventSeries")
        //                //    {
        //                //        SQL = "UPDATE CollectionEventSeries SET EventSeriesParentID  = " + rParent["SeriesID"].ToString() + " WHERE EventSeriesID = " + rChild["SeriesID"].ToString();
        //                //        cmd.CommandText = SQL;
        //                //        c.Open();
        //                //        cmd.ExecuteNonQuery();
        //                //        c.Close();
        //                //        //this.buildEventSeriesHierarchy();
        //                //        this.setSpecimen(this.ID);
        //                //    }
        //                //    else
        //                //    {
        //                //        System.Windows.Forms.MessageBox.Show("EventSeriess / collection event groups can only be placed within EventSeriess / collection event groups");
        //                //        //this.buildEventSeriesHierarchy();
        //                //        return;
        //                //    }
        //                //    break;
        //                case "IdentificationUnit":
        //                    int ChildID;
        //                    int oldChildSubstrateID;
        //                    int ParentID;
        //                    //try
        //                    //{
        //                    //    if (e.Data.GetDataPresent("DiversityCollection.HierarchyNode", false))
        //                    //    {
        //                    //        pt = this.treeViewDataUnitHierarchy.PointToClient(new Point(e.X, e.Y));
        //                    //        ParentNode = this.treeViewDataUnitHierarchy.GetNodeAt(pt);
        //                    //        ChildNode = (TreeNode)e.Data.GetData("DiversityCollection.HierarchyNode");
        //                    if (ParentTable == "IdentificationUnitList")
        //                    {
        //                        if (!ParentNode.Equals(ChildNode))
        //                        {
        //                            rChild = (System.Data.DataRow)ChildNode.Tag;
        //                            ChildID = System.Int32.Parse(rChild["IdentificationUnitID"].ToString());
        //                            if (rChild["RelatedUnitID"].Equals(System.DBNull.Value)) oldChildSubstrateID = -1;
        //                            else oldChildSubstrateID = System.Int32.Parse(rChild["RelatedUnitID"].ToString());
        //                            if (ParentNode.Tag != null)
        //                            {
        //                                rParent = (System.Data.DataRow)ParentNode.Tag;
        //                                ParentID = System.Int32.Parse(rParent["IdentificationUnitID"].ToString());
        //                                rChild["RelatedUnitID"] = ParentID;
        //                            }
        //                            else rChild["RelatedUnitID"] = System.DBNull.Value;
        //                            System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.Tables["IdentificationUnit"].Select("RelatedUnitID = " + ChildID.ToString());
        //                            foreach (System.Data.DataRow r in rr)
        //                            {
        //                                if (oldChildSubstrateID > -1) r["RelatedUnitID"] = oldChildSubstrateID;
        //                                else r["RelatedUnitID"] = System.DBNull.Value;
        //                            }
        //                            //this.buildUnitHierarchy();
        //                        }
        //                    }
        //                    else if (ParentTable == "CollectionSpecimen")
        //                    {
        //                        rChild = (System.Data.DataRow)ChildNode.Tag;
        //                        rChild["RelatedUnitID"] = System.DBNull.Value;
        //                    }
        //                    //    }
        //                    //}
        //                    //catch (Exception ex)
        //                    //{
        //                    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //                    //}
        //                    break;
        //                default:
        //                    System.Windows.Forms.MessageBox.Show("Only event series, collection events, collection speciman and identification units can moved here");
        //                    //this.buildEventSeriesHierarchy();
        //                    //return;
        //                    break;
        //            }
        //            this.fillDataSetSeries();
        //            this.initTree();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private bool NoLoopInEventSeries(System.Data.DataRow rChild, System.Data.DataRow rParent)
        //{
        //    bool NoLoop = true;
        //    try
        //    {
        //        int ChildID = int.Parse(rChild["SeriesID"].ToString());
        //        int ParentID = int.Parse(rParent["SeriesID"].ToString());
        //        if (ChildID == ParentID)
        //            return false;
        //        int? ParentOfParentID = null;
        //        int iPP = 0;
        //        if (int.TryParse(rParent["SeriesParentID"].ToString(), out iPP))
        //            ParentOfParentID = iPP;
        //        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentID);
        //        if (rr.Length > 0)
        //        {
        //            if (!rr[0]["SeriesParentID"].Equals(System.DBNull.Value))
        //            {
        //                while (ParentOfParentID != null)
        //                {
        //                    if (ParentOfParentID == ChildID)
        //                    {
        //                        NoLoop = false;
        //                        break;
        //                    }
        //                    System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentOfParentID);
        //                    if (RR.Length > 0)
        //                    {
        //                        if (RR[0]["SeriesParentID"].Equals(System.DBNull.Value))
        //                            break;
        //                        else
        //                        {
        //                            ParentOfParentID = int.Parse(RR[0]["SeriesParentID"].ToString());
        //                        }
        //                    }
        //                    else break;
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //    return NoLoop;
        //}

        //private void treeViewData_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        System.Windows.Forms.TreeNode tn;
        //        e.Effect = DragDropEffects.Move;
        //        TreeView tv = sender as TreeView;
        //        Point pt = tv.PointToClient(new Point(e.X, e.Y));
        //        int delta = tv.Height - pt.Y;
        //        if ((delta < tv.Height / 2) && (delta > 0))
        //        {
        //            tn = tv.GetNodeAt(pt.X, pt.Y);
        //            if (tn != null)
        //            {
        //                if (tn.NextVisibleNode != null)
        //                    tn.NextVisibleNode.EnsureVisible();
        //            }
        //        }
        //        if ((delta > tv.Height / 2) && (delta < tv.Height))
        //        {
        //            tn = tv.GetNodeAt(pt.X, pt.Y);
        //            if (tn.PrevVisibleNode != null)
        //                tn.PrevVisibleNode.EnsureVisible();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //    //System.Windows.Forms.TreeNode N = (System.Windows.Forms.TreeNode)sender;
        //    //System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //    //string Table = R.Table.TableName;
        //    //switch (Table)
        //    //{
        //    //    case "CollectionEventSeries":
        //    //        try
        //    //        {
        //    //            e.Effect = DragDropEffects.Move;
        //    //            treeViewData tv = sender as treeViewData;
        //    //            Point pt = tv.PointToClient(new Point(e.X, e.Y));
        //    //            int delta = tv.Height - pt.Y;
        //    //            if ((delta < tv.Height / 2) && (delta > 0))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.NextVisibleNode != null)
        //    //                    tn.NextVisibleNode.EnsureVisible();
        //    //            }
        //    //            if ((delta > tv.Height / 2) && (delta < tv.Height))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.PrevVisibleNode != null)
        //    //                    tn.PrevVisibleNode.EnsureVisible();
        //    //            }

        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //        }
        //    //        break;
        //    //    case "IdentificationUnit":
        //    //        try
        //    //        {
        //    //            e.Effect = DragDropEffects.Move;
        //    //            treeViewData tv = sender as treeViewData;
        //    //            Point pt = tv.PointToClient(new Point(e.X, e.Y));
        //    //            int delta = tv.Height - pt.Y;
        //    //            if ((delta < tv.Height / 2) && (delta > 0))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.NextVisibleNode != null)
        //    //                    tn.NextVisibleNode.EnsureVisible();
        //    //            }
        //    //            if ((delta > tv.Height / 2) && (delta < tv.Height))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.PrevVisibleNode != null)
        //    //                    tn.PrevVisibleNode.EnsureVisible();
        //    //            }

        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //        }
        //    //        break;
        //    //}
        //}

        //private void treeViewData_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //    System.Windows.Forms.TreeNode N = (System.Windows.Forms.TreeNode)e.Item;
        //    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //    string Table = R.Table.TableName;
        //    switch (Table)
        //    {
        //        case "CollectionEventSeries":
        //        case "CollectionEventList":
        //        case "CollectionSpecimenList":
        //            //case "IdentificationUnit":
        //            this.treeViewData.DoDragDrop(e.Item, System.Windows.Forms.DragDropEffects.Move);
        //            break;
        //    }
        //}


        //#endregion

        //#region handling the tree

        //private bool ShowUnit
        //{
        //    get
        //    {
        //        if (this.toolStripButtonShowUnit.Tag == null) return true;
        //        else
        //        {
        //            if (this.toolStripButtonShowUnit.Tag.ToString() == "Show") return true;
        //            else return false;
        //        }
        //    }
        //}

        //private void treeViewData_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    try
        //    {
        //        foreach (System.Windows.Forms.DataGridViewRow rr in this.dataGridView.Rows)
        //        {
        //            rr.Selected = false;
        //        }

        //        if (this.treeViewData.SelectedNode != null
        //            && this.treeViewData.SelectedNode.Tag != null)
        //        {
        //            System.Data.DataRow R = (System.Data.DataRow)this.treeViewData.SelectedNode.Tag;
        //            string Table = R.Table.TableName;
        //            switch (Table)
        //            {
        //                case "IdentificationUnitList":
        //                    int UnitID;
        //                    if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
        //                    {
        //                        foreach (System.Windows.Forms.DataGridViewRow rr in this.dataGridView.Rows)
        //                        {
        //                            if (rr.Cells[0].Value != null
        //                                && rr.Cells[0].Value.ToString() == UnitID.ToString())
        //                                rr.Selected = true;
        //                        }
        //                    }
        //                    break;
        //                case "CollectionSpecimenList":
        //                    int SpecimenID;
        //                    if (int.TryParse(R["CollectionSpecimenID"].ToString(), out SpecimenID))
        //                    {
        //                        foreach (System.Windows.Forms.DataGridViewRow rr in this.dataGridView.Rows)
        //                        {
        //                            if (rr.Cells[1].Value != null
        //                                && rr.Cells[1].Value.ToString() == SpecimenID.ToString())
        //                                rr.Selected = true;
        //                        }
        //                    }
        //                    break;
        //                case "CollectionEventList":
        //                    int EventID;
        //                    if (int.TryParse(R["CollectionEventID"].ToString(), out EventID))
        //                    {
        //                        System.Data.DataRow[] rrSp = this.dataSetCollectionEventSeries.CollectionSpecimenList.Select("CollectionEventID = " + EventID.ToString());
        //                        if (rrSp.Length > 0)
        //                        {
        //                            System.Collections.Generic.List<string> ListSpecimenIDs = new List<string>();
        //                            foreach (System.Data.DataRow rSp in rrSp)
        //                                ListSpecimenIDs.Add(rSp[0].ToString());
        //                            foreach (System.Windows.Forms.DataGridViewRow rr in this.dataGridView.Rows)
        //                            {
        //                                if (rr.Cells[1].Value != null
        //                                    && ListSpecimenIDs.Contains(rr.Cells[1].Value.ToString()))
        //                                    rr.Selected = true;
        //                            }
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //        if (this.dataGridView.SelectedRows.Count == 0)
        //            System.Windows.Forms.MessageBox.Show("Selected data are not shown in table");
        //    }
        //    catch { }
        //}

        //private void toolStripButtonShowUnit_Click(object sender, EventArgs e)
        //{
        //    this.setToolStripButtonOverviewHierarchyState(
        //        this.toolStripButtonShowUnit,
        //        DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImageTaxon.Plant],
        //        DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImageTaxon.PlantGrey]);
        //    if (!this.ShowUnit) this.hideHierarchyUnits();
        //    else
        //        this.addHierarchyUnits();
        //}

        //private void addHierarchyUnits()
        //{
        //    if (this.ShowUnit)
        //    {
        //        System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //        this.getHierarchyNodes(null, "CollectionSpecimenList", this.treeViewData, ref Nodes);
        //        if (Nodes.Count > 0)
        //        {
        //            foreach (System.Windows.Forms.TreeNode N in Nodes)
        //            {
        //                this.getEventSeriesEventSpecimenUnits(N);
        //            }
        //        }
        //    }
        //}

        //private void hideHierarchyUnits()
        //{
        //    this.hideHierarchyNodes("IdentificationUnit");
        //    this.hideHierarchyNodes("IdentificationUnitList");
        //}

        //private void hideHierarchyNodes(string Table)
        //{
        //    System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //    this.getHierarchyNodes(null, Table, this.treeViewData, ref Nodes);
        //    foreach (System.Windows.Forms.TreeNode N in Nodes)
        //        N.Remove();
        //}

        //private void getHierarchyNodes(System.Windows.Forms.TreeNode Node, string Table,
        //    System.Windows.Forms.TreeView treeViewData,
        //    ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> TreeNodes)
        //{
        //    if (TreeNodes == null) TreeNodes = new List<TreeNode>();
        //    if (Node == null)
        //    {
        //        foreach (System.Windows.Forms.TreeNode N in treeViewData.Nodes)
        //        {
        //            if (N.Tag != null)
        //            {
        //                try
        //                {
        //                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //                    if (R.Table.TableName == Table)
        //                        TreeNodes.Add(N);
        //                    this.getHierarchyNodes(N, Table, treeViewData, ref TreeNodes);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
        //        {
        //            if (N.Tag != null)
        //            {
        //                try
        //                {
        //                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //                    if (R.Table.TableName == Table) TreeNodes.Add(N);
        //                    this.getHierarchyNodes(N, Table, treeViewData, ref TreeNodes);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}

        //private void setToolStripButtonOverviewHierarchyState(
        //    System.Windows.Forms.ToolStripButton Button,
        //    System.Drawing.Image ImageShow,
        //    System.Drawing.Image ImageHide)
        //{
        //    if (Button.Tag == null)
        //        Button.Tag = "Hide";
        //    else
        //    {
        //        if (Button.Tag.ToString() == "Hide")
        //            Button.Tag = "Show";
        //        else
        //            Button.Tag = "Hide";
        //    }
        //    if (Button.Tag.ToString() == "Hide")
        //    {
        //        Button.Image = ImageHide;
        //        Button.BackColor = System.Drawing.Color.Yellow;
        //    }
        //    else
        //    {
        //        Button.Image = ImageShow;
        //        Button.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //}

        //private void toolStripButtonTreeSearch_Click(object sender, EventArgs e)
        //{
        //    if (this.treeViewData.SelectedNode != null)
        //    {
        //        if (this.treeViewData.SelectedNode.Tag != null)
        //        {
        //            try
        //            {
        //                System.Data.DataRow R = (System.Data.DataRow)treeViewData.SelectedNode.Tag;
        //                switch (R.Table.TableName)
        //                {
        //                    case "IdentificationUnitList":
        //                    case "CollectionSpecimenList":
        //                        int ID;
        //                        if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
        //                        {
        //                            this._SpecimenID = ID;
        //                            this.DialogResult = DialogResult.OK;
        //                            this.Close();
        //                        }
        //                        break;
        //                    default:
        //                        System.Windows.Forms.MessageBox.Show("Please select a specimen");
        //                        break;
        //                }
        //            }
        //            catch { }
        //        }
        //    }
        //}

        //#endregion

        //#endregion


    }
}
