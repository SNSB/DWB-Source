using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using DiversityWorkbench.DwbManual;

namespace DiversityCollection.Forms
{
    public partial class FormPartGrid : Form
    {

        #region Parameter

        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields;
        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeHiddenQueryFields;
        private System.Collections.Generic.List<string> _GridModeColumnList;
        private System.Collections.Generic.Dictionary<string, string> _GridModeTableList;
        private System.Collections.Generic.List<string> _ReadOnlyColumns;
        private System.Collections.Generic.Dictionary<string, string> _RemoveColumns;
        private System.Collections.Generic.Dictionary<string, string> _BlockedColumns;
        private System.Collections.Generic.List<System.Windows.Forms.DataGridViewRow> _RowsWithBlockingSet;

        private System.Collections.Generic.Dictionary<string, string> _ProjectSettings;
        private int _ProjectID = 0;

        private int _PartID = 0;
        private int _SpecimenID = 0;

        private System.Collections.Generic.List<int> _IDs;
        private string _sIDs;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;

        private DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState _ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace;

        private System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> _AnalysisList;
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _TaxonAnalysisDict;
        private System.Collections.Generic.List<string> _AnalysisReadOnlyColumns;

        private System.Collections.Generic.List<ProcessingEntry> _ProcessingList;
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> _MaterialProcessingDict;
        private System.Collections.Generic.List<string> _ProcessingReadOnlyColumns;
        private System.Data.DataTable _dtProcessingRestriction = new DataTable();

        private enum SaveModes { Single, All };
        private FormPartGrid.SaveModes _SaveMode = SaveModes.Single;
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
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesUnit;

        #endregion

        private string _SqlSpecimenFields = "SpecimenPartID, CollectionSpecimenID, Accession_number, Data_withholding_reason, Data_withholding_reason_for_collection_event, " +
            "Data_withholding_reason_for_collector, Collectors_event_number, Collection_day, Collection_month, Collection_year, Collection_date_supplement, Collection_time, " +
            "Collection_time_span, Country, Locality_description, Habitat_description, Collecting_method, Collection_event_notes, Named_area, NamedAreaLocation2, " +
            "Remove_link_to_gazetteer, Distance_to_location, Direction_to_location, Longitude, Latitude, Coordinates_accuracy, Link_to_GoogleMaps, Altitude_from, Altitude_to, " +
            "Altitude_accuracy, Notes_for_Altitude, MTB, Quadrant, Notes_for_MTB, Sampling_plot, Link_to_SamplingPlots, Remove_link_to_SamplingPlots, " +
            "Accuracy_of_sampling_plot, Latitude_of_sampling_plot, Longitude_of_sampling_plot, Geographic_region, Lithostratigraphy, Chronostratigraphy, Collectors_name, " +
            "Link_to_DiversityAgents, Remove_link_for_collector, Collectors_number, Notes_about_collector, Accession_day, Accession_month, Accession_year, " +
            "Accession_date_supplement, Depositors_name, Depositors_link_to_DiversityAgents, Remove_link_for_Depositor, Depositors_accession_number, " +
            "Exsiccata_abbreviation, Link_to_DiversityExsiccatae, Remove_link_to_exsiccatae, Exsiccata_number, Original_notes, Additional_notes, Internal_notes, Label_title, " +
            "Label_type, Label_transcription_state, Label_transcription_notes, Problems, Taxonomic_group, Relation_type, Colonised_substrate_part, Related_organism, " +
            "Life_stage, Gender, Number_of_units, Circumstances, Order_of_taxon, Family_of_taxon, Identifier_of_organism, Description_of_organism, Only_observed, " +
            "Notes_for_organism, Taxonomic_name, Link_to_DiversityTaxonNames, Remove_link_for_identification, Vernacular_term, Identification_day, Identification_month, " +
            "Identification_year, Identification_category, Identification_qualifier, Type_status, Type_notes, Notes_for_identification, " + //Reference_title, Link_to_DiversityReferences, Remove_link_for_reference, " +
            "Determiner, Link_to_DiversityAgents_for_determiner, Remove_link_for_determiner, Analysis_0, AnalysisID_0, Analysis_number_0, " +
            "Analysis_result_0, Analysis_1, AnalysisID_1, Analysis_number_1, Analysis_result_1, Analysis_2, AnalysisID_2, Analysis_number_2, Analysis_result_2, Analysis_3, " +
            "AnalysisID_3, Analysis_number_3, Analysis_result_3, Analysis_4, AnalysisID_4, Analysis_number_4, Analysis_result_4, Analysis_5, AnalysisID_5, " +
            "Analysis_number_5, Analysis_result_5, Analysis_6, AnalysisID_6, Analysis_number_6, Analysis_result_6, Analysis_7, AnalysisID_7, Analysis_number_7, " +
            "Analysis_result_7, Analysis_8, AnalysisID_8, Analysis_number_8, Analysis_result_8, Analysis_9, AnalysisID_9, Analysis_number_9, Analysis_result_9, " +
            "Preparation_method, Preparation_date, Part_accession_number, Part_sublabel, Collection, Material_category, Storage_location, Storage_container, Stock, Stock_unit, Notes_for_part, " +
            "Description_of_unit_in_part, Processing_date_1, ProcessingID_1, Processing_Protocoll_1, Processing_duration_1, Processing_notes_1, Processing_date_2, ProcessingID_2, Processing_Protocoll_2, " +
            "Processing_duration_2, Processing_notes_2, Processing_date_3, ProcessingID_3, Processing_Protocoll_3, Processing_duration_3, Processing_notes_3, " +
            "Processing_date_4, ProcessingID_4, Processing_Protocoll_4, Processing_duration_4, Processing_notes_4, Processing_date_5, ProcessingID_5, Processing_Protocoll_5, " +
            "Processing_duration_5, Processing_notes_5, _TransactionID, _Transaction, On_loan, _CollectionEventID, " +
            "_IdentificationUnitID, _IdentificationSequence, _SpecimenPartID, _CoordinatesAverageLatitudeCache, _CoordinatesAverageLongitudeCache, " +
            "_CoordinatesLocationNotes, _GeographicRegionPropertyURI, _LithostratigraphyPropertyURI, _ChronostratigraphyPropertyURI, _NamedAverageLatitudeCache, " +
            "_NamedAverageLongitudeCache, _LithostratigraphyPropertyHierarchyCache, _ChronostratigraphyPropertyHierarchyCache, _AverageAltitudeCache ";


        private string _SourceFunction;
        private string SourceFunction
        {
            get
            {
                if (this._SourceFunction == null || this._SourceFunction.Length == 0)
                {
                    this._SourceFunction = "FirstLinesPart_2";
                    string SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.ROUTINE_NAME = '" + this._SourceFunction + "'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result != "1")
                        this._SourceFunction = "FirstLinesPart";
                }
                return this._SourceFunction;
            }
        }

        #endregion

        #region Construction

        public FormPartGrid(System.Collections.Generic.List<int> IDs, string FormTitle, int ProjectID)
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please connect to a database");
                this.Close();
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this._FormState = FormGridFunctions.FormState.Loading;
            InitializeComponent();
            this._ProjectID = ProjectID;
            this.textBoxHeaderProject.Text = DiversityCollection.LookupTable.ProjectName(this._ProjectID);
            this._IDs = IDs;
            for (int i = 0; i < _IDs.Count; i++)
            {
                if (i > 0) _sIDs += ", ";
                this._sIDs += _IDs[i].ToString();
            }
            if (FormTitle.Length > 0) this.Text = FormTitle;
            this.initForm();
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            if (IDs.Count > 0)
                this._SpecimenID = IDs[0];
            this.userControlImageSpecimenImage.MaxSizeOfImageVisible = true;
            this.GridModeFillTable();
            this.GridModeSetColumnVisibility();
            this.setRemoveCellStyle();
            this.setIconsIntreeViewData();
            this.setTitleIntreeViewData();
            this.dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this._FormState = FormGridFunctions.FormState.Editing;
            DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityCollection.Forms.FormCollectionSpecimenGridModeText));
            DiversityWorkbench.Entity.setEntity(this, resources);
        }
        
        #endregion

        #region Form

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
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
                string TableName = "CollectionSpecimenPart";
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
                this.comboBoxReplace.DropDownWidth = 400;
                this.comboBoxReplaceWith.Width = 100;
                this.comboBoxReplaceWith.DropDownWidth = 400;
                this.FillPicklists();
                if (DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate != null &&
                    DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate.Year != 1)
                    this.dateTimePickerProcessingFrom.Value = DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate;
                if (DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate != null &&
                    DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate.Year != 1)
                    this.dateTimePickerProcessingTo.Value = DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate;
                this.setColumnsAndNodesCorrespondingToPermissions();
                if (DiversityCollection.Forms.FormPartGridSettings.Default.Visibility.Length > 0)
                    this.GridModeSetFieldVisibilityForNodes();
                this.GridModeSetToolTipsInTree();
                this.setImageVisibility(DiversityCollection.Forms.FormPartGridSettings.Default.HeaderDisplay);
                this.userControlImageSpecimenImage.ImagePath = "";
                this.setGridColumnHeaders();
                this.enableReplaceButtons(false);
                this.userControlDialogPanel.buttonOK.Click += new EventHandler(CheckForChangesAndAskForSaving);
                foreach (System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
                    this.SetToolTipsIntreeViewData(N);
                this.setColumnSequence();
                this.setColumnWidths();
                this.setOptions();
                this.buttonHeaderShowTree.BackColor = System.Drawing.SystemColors.Control;
                this.setVisibilityTopControls();
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

            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollCircumstances_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollIdentificationCategory_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollIdentificationQualifier_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollMaterialCategory_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollTaxonomicGroup_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollTypeStatus_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollUnitRelationType_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollLabelTranscriptionState_Enum, false);
            DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetPartGrid.CollLabelType_Enum, false);

            string SQL = "SELECT CollectionID, DisplayText AS CollectionName " +
                "FROM dbo.CollectionHierarchyAll() ORDER BY DisplayText ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(this.dataSetPartGrid.Collection);
            }
            catch (System.Exception ex) { }
            ad.SelectCommand.CommandText = "";
            try
            {
                ad.SelectCommand.CommandText = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                ad.Fill(this.dataSetPartGrid.ProjectList);
            }
            catch { }

            ad.SelectCommand.CommandText = "SELECT NameEn FROM [DiversityGazetteer].[dbo].[GeoCountries] " +
                "WHERE (PlaceID > 1) " +
                "UNION " +
                "SELECT DISTINCT CountryCache AS NameEn FROM CollectionEvent " +
                "ORDER BY NameEn";
            try
            {
                ad.Fill(this.dataSetPartGrid.GeoCountries);
            }
            catch { }

            if (this.dataSetPartGrid.GeoCountries.Rows.Count == 0)
            {
                ad.SelectCommand.CommandText = "SELECT DISTINCT CountryCache AS NameEn " +
                    "FROM CollectionEvent ORDER BY NameEn";
                try
                {
                    ad.Fill(this.dataSetPartGrid.GeoCountries);
                }
                catch { }
            }

            if (this.dataSetPartGrid.Analysis.Rows.Count == 0)
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
                    ad.Fill(this.dataSetPartGrid.Analysis);
                }
                catch { }
            }

            if (this.dataSetPartGrid.Processing.Rows.Count == 0)
            {
                ad.SelectCommand.CommandText = "SELECT P.ProcessingID, P.DisplayText " +
                    "+ CASE WHEN Pr.DisplayText IS NULL THEN '' ELSE ' / ' + Pr.DisplayText END " +
                    "+ CASE WHEN P.Description IS NULL OR P.Description = '' THEN '' ELSE ': ' + P.Description END " +
                    "AS DisplayText " +
                    "FROM dbo.ProcessingProjectList( " + this._ProjectID.ToString() + " ) AS P LEFT OUTER JOIN " +
                    "Processing AS Pr ON Pr.ProcessingID = P.ProcessingParentID " +
                    "WHERE (P.OnlyHierarchy = 0)";
                try
                {
                    ad.Fill(this.dataSetPartGrid.Processing);
                }
                catch { }
            }

            if (this._dtProcessingRestriction.Rows.Count == 0)
            {
                ad.SelectCommand.CommandText = "SELECT 0 AS ProcessingID, '' AS DisplayText " +
                    "UNION " +
                    "SELECT P.ProcessingID, P.DisplayText " +
                    "+ CASE WHEN Pr.DisplayText IS NULL THEN '' ELSE ' / ' + Pr.DisplayText END " +
                    "+ CASE WHEN P.Description IS NULL OR P.Description = '' THEN '' ELSE ': ' + P.Description END " +
                    "AS DisplayText " +
                    "FROM dbo.ProcessingProjectList( " + this._ProjectID.ToString() + " ) AS P LEFT OUTER JOIN " +
                    "Processing AS Pr ON Pr.ProcessingID = P.ProcessingParentID " +
                    "WHERE (P.OnlyHierarchy = 0)";
                try
                {
                    ad.Fill(this._dtProcessingRestriction);
                    this.comboBoxProcessing.DataSource = this._dtProcessingRestriction;
                    this.comboBoxProcessing.DisplayMember = "DisplayText";
                    this.comboBoxProcessing.ValueMember = "ProcessingID";
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

        private void FormPartGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bool OK = this.FormFunctions.getObjectPermissions("CollectionSpecimenpart", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                if (OK)
                {
                    this.CheckForChangesAndAskForSaving(null, null);
                }
                string ColumnWidths = "";
                string ColumnPositions = "";
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                {
                    ColumnWidths += C.Width + " ";
                    ColumnPositions += C.DisplayIndex.ToString() + " ";
                }
                DiversityCollection.Forms.FormPartGridSettings.Default.ColumnWidth = ColumnWidths;
                DiversityCollection.Forms.FormPartGridSettings.Default.ColumnSequence = ColumnPositions;
                DiversityCollection.Forms.FormPartGridSettings.Default.HeaderDisplay = this.CurrentHeaderDisplaySettings;
                DiversityCollection.Forms.FormPartGridSettings.Default.Save();
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

        private void FormPartGrid_Load(object sender, EventArgs e)
        {
            //// TODO: This line of code loads data into the 'dataSetPartGrid.Collection' table. You can move, or remove it, as needed.
            //this.collectionTableAdapter.Fill(this.dataSetPartGrid.Collection);
            //// TODO: This line of code loads data into the 'dataSetPartGrid.CollMaterialCategory_Enum' table. You can move, or remove it, as needed.
            //this.collMaterialCategory_EnumTableAdapter.Fill(this.dataSetPartGrid.CollMaterialCategory_Enum);
            //// TODO: This line of code loads data into the 'dataSetPartGrid.Processing' table. You can move, or remove it, as needed.
            //this.processingTableAdapter.Fill(this.dataSetPartGrid.Processing);
            //// TODO: This line of code loads data into the 'dataSetPartGrid.Analysis' table. You can move, or remove it, as needed.
            //this.analysisTableAdapter.Fill(this.dataSetPartGrid.Analysis);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimen.CollectionSpecimenImage". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionSpecimenImageTableAdapter.Fill(this.dataSetCollectionSpecimen.CollectionSpecimenImage);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetPartGrid.FirstLinesPart". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.firstLinesPartTableAdapter.Fill(this.dataSetPartGrid.FirstLinesPart);

        }

        #endregion

        #region Options

        private void buttonSetOptions_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Forms.FormGridOptions f = new DiversityCollection.Forms.FormGridOptions(
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate,
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate,
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs,
                    this._ProjectID);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (f.AnalysisStartDate != null)
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate = (System.DateTime)f.AnalysisStartDate;
                    if (f.AnalysisEndDate != null)
                        DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate = (System.DateTime)f.AnalysisEndDate;
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs = f.AnalysisIDs;
                    DiversityCollection.Forms.FormPartGridSettings.Default.Save();
                    System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> AnalysisList = f.AnalysisList;
                    this.setOptions(AnalysisList);
                    this.buttonGridModeUpdateColumnSettings_Click(null, null);
                }
            }
            catch (System.Exception ex) { }
        }

        private void setOptions(System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> AnalysisList)
        {
            this.textBoxOptions.Text = "";
            this.textBoxOptions.Height = 0;
            if (DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate.Year != 1)
            {
                this.textBoxOptions.Text += "Start date for analysis: " + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate.ToShortDateString() + "\r\n";
                textBoxOptions.Height += 8;
            }
            if (DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate.Year != 1)
            {
                this.textBoxOptions.Text += "End date for analysis: " + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate.ToShortDateString() + "\r\n";
                textBoxOptions.Height += 8;
            }
            this.AnalysisList = AnalysisList;
            if (this.AnalysisList.Count > 0) //DiversityCollection.Forms.FormPartGridSettings.Default.UseAnalysisIDs)
            {
                textBoxOptions.Height += 16;
                this.textBoxOptions.Text += "Types of analysis: ";
                foreach (DiversityCollection.Forms.AnalysisEntry A in AnalysisList)
                {
                    this.textBoxOptions.Text += A.AnalysisType + "; ";
                }
            }
            //this._AnalysisList = null;
            this.setTitleIntreeViewData();
            this.setGridColumnHeaders();
            this.GridModeFillTable();
        }

        private void setOptions()
        {
            try
            {
                System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> List = new List<DiversityCollection.Forms.AnalysisEntry>();
                if (DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs != null)
                {
                    foreach (string s in DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs)
                    {
                        DiversityCollection.Forms.AnalysisEntry A = new DiversityCollection.Forms.AnalysisEntry();
                        A.AnalysisType = DiversityCollection.LookupTable.AnalysisTitle(int.Parse(s));
                        A.AnalysisID = int.Parse(s);
                        List.Add(A);
                    }
                }
                this.setOptions(List);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //this.setTitleIntreeViewData();
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
                        case "NodeProcessing":
                            iIndex = DiversityCollection.Specimen.ImageTableOrFieldIndex(DiversityCollection.Specimen.ImageTableOrField.Processing, false);
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
                try
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
                catch { }
            }
        }

        private void setTitleIntreeViewData()
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
                        this.setTitleIntreeViewData(childN);
                }
                catch { }
            }
        }

        private void setTitleIntreeViewData(System.Windows.Forms.TreeNode TreeNode)
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
                                    this.setAnalysisNode(TreeNode, 0, NodeTitle);
                                    //if (this.AnalysisList.Count > 0)
                                    //    TreeNode.Text = this.AnalysisList[0].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_1":
                                    this.setAnalysisNode(TreeNode, 1, NodeTitle);
                                    //if (this.AnalysisList.Count > 1)
                                    //    TreeNode.Text = this.AnalysisList[1].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_2":
                                    this.setAnalysisNode(TreeNode, 2, NodeTitle);
                                    //if (this.AnalysisList.Count > 2)
                                    //    TreeNode.Text = this.AnalysisList[2].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_3":
                                    this.setAnalysisNode(TreeNode, 3, NodeTitle);
                                    //if (this.AnalysisList.Count > 3)
                                    //    TreeNode.Text = this.AnalysisList[3].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_4":
                                    this.setAnalysisNode(TreeNode, 4, NodeTitle);
                                    //if (this.AnalysisList.Count > 4)
                                    //    TreeNode.Text = this.AnalysisList[4].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_5":
                                    this.setAnalysisNode(TreeNode, 5, NodeTitle);
                                    //if (this.AnalysisList.Count > 5)
                                    //    TreeNode.Text = this.AnalysisList[5].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_6":
                                    this.setAnalysisNode(TreeNode, 6, NodeTitle);
                                    //if (this.AnalysisList.Count > 6)
                                    //    TreeNode.Text = this.AnalysisList[6].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_7":
                                    this.setAnalysisNode(TreeNode, 7, NodeTitle);
                                    //if (this.AnalysisList.Count > 7)
                                    //    TreeNode.Text = this.AnalysisList[7].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_8":
                                    this.setAnalysisNode(TreeNode, 8, NodeTitle);
                                    //if (this.AnalysisList.Count > 8)
                                    //    TreeNode.Text = this.AnalysisList[8].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
                                    break;
                                case "Analysis_9":
                                    this.setAnalysisNode(TreeNode, 9, NodeTitle);
                                    //if (this.AnalysisList.Count > 9)
                                    //    TreeNode.Text = this.AnalysisList[9].AnalysisType;
                                    //else TreeNode.Text = NodeTitle;
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
                    if (Q.Entity != null && Q.Entity.Length > 0 && Q.Restriction != null
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

        private void setAnalysisNode(System.Windows.Forms.TreeNode N, int AnalysisIndex, string NodeTitle)
        {
            if (this.AnalysisList.Count > AnalysisIndex)
            {
                N.Text = this.AnalysisList[AnalysisIndex].AnalysisType;
                N.Checked = true;
                N.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                N.Text = NodeTitle;
                N.Checked = false;
                N.ForeColor = System.Drawing.Color.Gray;
            }
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
                        string[] Parameter = DiversityCollection.Forms.FormGridFunctions.GridModeFieldTagArray(N.Tag.ToString());
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
                if (DiversityCollection.Forms.FormPartGridSettings.Default.Visibility.Length > 0
                    && this.GridModeQueryFields.Count > 0)
                {
                    foreach (System.Windows.Forms.TreeNode Node in this.treeViewGridModeFieldSelector.Nodes)
                    {
                        this.GridModeSetFieldVisibilityForChildNodes(Node);
                    }
                }

            }
            catch { }
        }

        private void GridModeSetFieldVisibilityForChildNodes(System.Windows.Forms.TreeNode Node)
        {
            try
            {
                foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
                {
                    if (N.Tag != null)
                    {
                        string[] Parameter = DiversityCollection.Forms.FormGridFunctions.GridModeFieldTagArray(N.Tag.ToString());
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
            catch { }
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
                    string[] Parameter = DiversityCollection.Forms.FormGridFunctions.GridModeFieldTagArray(Node.Tag.ToString());
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
                    DiversityCollection.Forms.FormPartGridSettings.Default.Visibility = Visibility;
                    DiversityCollection.Forms.FormPartGridSettings.Default.Save();
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
                // CollectionSpecimenPart
                TableName = "CollectionSpecimenPart";
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

                if (!DiversityCollection.Forms.FormCollectionSpecimen.HasTransactionAccess)
                {
                    this.RemoveNodeFromTree("On_loan", null);
                    this.RemoveNodeFromTree("_Transaction", null);
                    this.RemoveNodeFromTree("_TransactionID", null);
                }
                else
                {
                    // setting the columns if GridView is not ReadOnly
                    if (!this.dataGridView.ReadOnly)
                    {
                        // CollectionSpecimenTransaction
                        TableName = "CollectionSpecimenTransaction";
                        // Check UPDATE
                        OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                        this.setGridColumnAccordingToPermission(TableName, OK);
                    }
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
        private void setGridColumnAccordingToPermission(string TableName, bool HasPermission)
        {
            System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> FieldList = new List<GridModeQueryField>();
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
            catch { }
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
                        if (!HasAccessToModule)
                            this.RemoveNodeFromTree(AliasForColumn, null);
                        break;
                    case "Geographic_region":
                    case "Lithostratigraphy":
                    case "Chronostratigraphy":
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
                        }
                        break;
                    case "Link_to_DiversityAgents":
                    case "Depositors_link_to_DiversityAgents":
                    case "Link_to_DiversityAgents_for_responsible":
                        HasAccessToModule = DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents");
                        if (!HasAccessToModule)
                        {
                            this.RemoveNodeFromTree(AliasForColumn, null);
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

        private bool setSpecimen(int SpecimenID, int PartID)
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
                this.fillSpecimen(SpecimenID, PartID);
                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    //this.setHeader();
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

        private void setHeader()
        {
            try
            {
                string StorageLocation = "";
                if (this.firstLinesPartBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
                    if (!R["Storage_location"].Equals(System.DBNull.Value))
                        StorageLocation = R["Storage_location"].ToString();
                    if (!R["Accession_number"].Equals(System.DBNull.Value))
                        this.textBoxHeaderAccessionNumber.Text = R["Accession_number"].ToString();
                }
                else
                {
                    this.textBoxHeaderStorageLocation.Text = "";
                    this.textBoxHeaderStorageLocation.BackColor = System.Drawing.SystemColors.Control;
                }

                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                {
                    if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count > 0)
                    {
                        System.Data.DataRow[] rrType = this.dataSetCollectionSpecimen.Identification.Select("TypeStatus <> '' AND TypeStatus NOT LIKE 'not %'", "IdentificationSequence DESC");
                        if (rrType.Length > 0)
                        {
                            this.textBoxHeaderStorageLocation.BackColor = System.Drawing.Color.Red;
                            System.Data.DataRow rType = rrType[0];
                            if (!rType["TaxonomicName"].Equals(System.DBNull.Value))
                                StorageLocation += "["+ rType["TaxonomicName"].ToString() + "\r\n" + rType["TypeStatus"].ToString() + "]";
                            else
                                StorageLocation += "[" + rType["TypeStatus"].ToString() + "]";
                        }
                        else
                        {
                            this.textBoxHeaderStorageLocation.BackColor = System.Drawing.SystemColors.Control;
                            if (StorageLocation.Length == 0)
                            {
                                System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder");
                                System.Data.DataRow r = rr[0];
                                if (!r["StorageLocation"].Equals(System.DBNull.Value))
                                    StorageLocation = r["StorageLocation"].ToString();
                                else
                                    StorageLocation = "";
                            }
                        }
                    }
                }
                this.textBoxHeaderStorageLocation.Text = StorageLocation;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillSpecimen(int SpecimenID, int PartID)
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

                // Specimen and related data
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

                // Part and related data
                string WhereClausePart = " AND SpecimenPartID = " + PartID.ToString();

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause + WhereClausePart, this.dataSetCollectionSpecimen.CollectionSpecimenPart);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProcessing, DiversityCollection.CollectionSpecimen.SqlProcessing + WhereClause + WhereClausePart, this.dataSetCollectionSpecimen.CollectionSpecimenProcessing);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterTransaction, DiversityCollection.CollectionSpecimen.SqlTransaction + WhereClause + WhereClausePart, this.dataSetCollectionSpecimen.CollectionSpecimenTransaction);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAnalysis, DiversityCollection.CollectionSpecimen.SqlAnalysis + WhereClause + WhereClausePart, this.dataSetCollectionSpecimen.IdentificationUnitAnalysis);

                // Unit and related data
                string WhereClauseUnit = " AND IdentificationUnitID = (SELECT MIN(U.IdentificationUnitID) AS UnitID " +
                    "FROM IdentificationUnit AS U INNER JOIN " +
                    "IdentificationUnitInPart AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID AND U.IdentificationUnitID = P.IdentificationUnitID " +
                    "WHERE P.SpecimenPartID = " + PartID.ToString() + ")";
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnit, DiversityCollection.CollectionSpecimen.SqlUnit + WhereClause + WhereClauseUnit, this.dataSetCollectionSpecimen.IdentificationUnit);
                string WhereClauseIdentification = " AND IdentificationSequence = ( " +
                    "SELECT MAX(I.IdentificationSequence) AS Sequence " +
                    "FROM Identification AS I INNER JOIN " +
                    "IdentificationUnitInPart AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID AND I.IdentificationUnitID = P.IdentificationUnitID " +
                    "WHERE P.SpecimenPartID = " + PartID.ToString() + ")";
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, DiversityCollection.CollectionSpecimen.SqlIdentification + WhereClause + WhereClauseUnit + WhereClauseIdentification + " ORDER BY IdentificationSequence", this.dataSetCollectionSpecimen.Identification);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnitInPart, DiversityCollection.CollectionSpecimen.SqlUnitInPart + WhereClause + WhereClausePart + WhereClauseUnit, this.dataSetCollectionSpecimen.IdentificationUnitInPart);

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
                // if datasets of this table were deleted, this must happen before deleting the parent tables
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenPart", this.sqlDataAdapterPart, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenProcessing", this.sqlDataAdapterProcessing, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitAnalysis", this.sqlDataAdapterAnalysis, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenTransaction", this.sqlDataAdapterTransaction, this.BindingContext);

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
                        if (DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs != null)
                        {
                            foreach (string s in DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs)
                            {
                                if (AnalysisIDs.Length > 0) AnalysisIDs += ",";
                                AnalysisIDs += s;
                            }
                            if (AnalysisIDs.Length > 0)
                            {
                                SQL = "SELECT DISTINCT TOP 10 A.DisplayText, A.AnalysisID " +
                                "FROM dbo.Analysis AS A " +
                                "WHERE A.AnalysisID IN (" + AnalysisIDs + ") " +
                                "ORDER BY A.DisplayText";
                            }
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
                    { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
                return _AnalysisList;
            }
            set { this._AnalysisList = value; }
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
                    if (this.dataSetPartGrid.Analysis.Rows.Count == 0)
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
                            ad.Fill(this.dataSetPartGrid.Analysis);
                        }
                        catch { }
                    }
                    int AnalysisID = (int)ID;
                    string Analysis = "";
                    System.Data.DataRow[] RR = this.dataSetPartGrid.Analysis.Select("AnalysisID = " + ID.ToString());
                    if (RR.Length == 0) return;
                    Analysis = RR[0]["DisplayText"].ToString();
                    this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID"] = AnalysisID;
                    this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["Analysis"] = Analysis;
                    if (this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["Analysis_number"].Equals(System.DBNull.Value))
                        this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["Analysis_number"] = "1";
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

            if (!int.TryParse(this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["IdentificationUnitID"].ToString(), out UnitID))
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
                    System.Windows.Forms.MessageBox.Show("No analysis types are available for this organism");
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
                    this._TaxonAnalysisDict = DiversityCollection.Analysis.AnalysisForTaxonomicGroup(this._ProjectID);
                return _TaxonAnalysisDict;
            }
        }

        private void setGridColumnsAndNodesForAnalysis()
        {

        }

        #endregion   

        #region Processing

        //public System.Collections.Generic.List<ProcessingEntry> ProcessingList
        //{
        //    get
        //    {
        //        if (this._ProcessingList == null)
        //        {
        //            this._ProcessingList = new List<ProcessingEntry>();

        //            try
        //            {
        //                string SQL = "";
        //                string ProcessingIDs = "";
        //                foreach (string s in DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingIDs)
        //                {
        //                    if (ProcessingIDs.Length > 0) ProcessingIDs += ",";
        //                    ProcessingIDs += s;
        //                }
        //                if (ProcessingIDs.Length > 0)
        //                {
        //                    SQL = "SELECT DISTINCT TOP 10 A.DisplayText, A.ProcessingID " +
        //                    "FROM dbo.Processing AS A " +
        //                    "WHERE A.ProcessingID IN (" + ProcessingIDs + ") " +
        //                    "ORDER BY A.DisplayText";
        //                }
        //                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter();
        //                System.Data.DataTable dt = new DataTable();
        //                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref ad, dt, SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(4000));
        //                this._ProcessingList.Clear();
        //                for (int i = 0; i < dt.Rows.Count; i++)
        //                {
        //                    ProcessingEntry A = new ProcessingEntry();
        //                    A.ProcessingID = int.Parse(dt.Rows[i]["ProcessingID"].ToString());
        //                    A.ProcessingType = dt.Rows[i]["DisplayText"].ToString();
        //                    this._ProcessingList.Add(A);
        //                }

        //            }
        //            catch
        //            { }
        //        }
        //        return _ProcessingList;
        //    }
        //}

        /// <summary>
        /// filling the table Analysis in the dataset
        /// </summary>
        private void setProcessing()
        {
            int? ID = this.ProcessingIDsforCurrentDataset();
            if (ID == null) return;
            else
            {
                try
                {
                    if (this.dataSetPartGrid.Processing.Rows.Count == 0)
                    {
                        string SQL = "SELECT A.ProcessingID, CASE WHEN PP.DisplayText IS NULL THEN '' ELSE PP.DisplayText + ' - ' END + CASE WHEN P.DisplayText IS NULL " +
                            "THEN '' ELSE P.DisplayText + ' - ' END + A.DisplayText + CASE WHEN A.[Description] IS NULL OR " +
                            "A.[Description] = A.[DisplayText] THEN '' ELSE ': ' + A.[Description] END AS DisplayText " +
                            "FROM Processing AS P RIGHT OUTER JOIN " +
                            "Processing AS A ON P.ProcessingID = A.ProcessingParentID LEFT OUTER JOIN " +
                            "Processing AS PP ON P.ProcessingParentID = PP.ProcessingID " +
                            "ORDER BY DisplayText";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        try
                        {
                            ad.Fill(this.dataSetPartGrid.Processing);
                        }
                        catch { }
                    }
                    int ProcessingID = (int)ID;
                    string Processing = "";
                    System.Data.DataRow[] RR = this.dataSetPartGrid.Processing.Select("ProcessingID = " + ID.ToString());
                    if (RR.Length == 0) return;
                    Processing = RR[0]["DisplayText"].ToString();
                    this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["ProcessingID"] = ProcessingID;
                    this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["Processing"] = Processing;
                    if (this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["Processing_number"].Equals(System.DBNull.Value))
                        this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["Processing_number"] = "1";
                }
                catch { }
            }
        }

        private int? ProcessingIDsforCurrentDataset()
        {
            int ID;
            int PartID;
            if (this.ProjectSettings.ContainsKey("CollectionSpecimenProcessing.ProcessingID")
                && this.ProjectSettings["CollectionSpecimenProcessing.ProcessingID"].Length > 0)
                if (int.TryParse(this.ProjectSettings["CollectionSpecimenProcessing.ProcessingID"], out ID))
                    return ID;

            if (!int.TryParse(this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfCurrentLine]["SpecimenPartID"].ToString(), out PartID))
                return null;
            System.Data.DataTable dtProcessing = new DataTable();
            string SQL = "SELECT ProcessingID, DisplayTextHierarchy AS DisplayText " +
                "FROM dbo.ProcessingListForPart (" + PartID.ToString() + ") " +
                "ORDER BY DisplayTextHierarchy";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dtProcessing);
                if (dtProcessing.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No processing types are available for this part");
                    return null;
                }
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtProcessing, "DisplayText", "ProcessingID", "Processing", "Please select the processing you want to insert");
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

        //public System.Collections.Generic.List<string> ProcessingReadOnlyColumns
        //{
        //    get
        //    {
        //        if (this._ProcessingReadOnlyColumns == null)
        //        {
        //            this._ProcessingReadOnlyColumns = new List<string>();
        //            for (int i = 9; i >= this.ProcessingList.Count; i--)
        //            {
        //                this._ProcessingReadOnlyColumns.Add("Processing_" + i.ToString());
        //                this._ProcessingReadOnlyColumns.Add("ProcessingID_" + i.ToString());
        //                this._ProcessingReadOnlyColumns.Add("Processing_number_" + i.ToString());
        //                this._ProcessingReadOnlyColumns.Add("Processing_result_" + i.ToString());
        //            }
        //        }
        //        return _ProcessingReadOnlyColumns;
        //    }
        //}

        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> MaterialProcessingDict
        {
            get
            {
                if (this._MaterialProcessingDict == null)
                    this._MaterialProcessingDict = DiversityCollection.Processing.ProcessingForMaterialCategory(this._ProjectID);
                return _MaterialProcessingDict;
            }
        }

        private void dateTimePickerProcessingFrom_ValueChanged(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate = this.dateTimePickerProcessingFrom.Value;
            DiversityCollection.Forms.FormPartGridSettings.Default.Save();
        }

        private void dateTimePickerProcessingTo_ValueChanged(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate = this.dateTimePickerProcessingTo.Value;
            DiversityCollection.Forms.FormPartGridSettings.Default.Save();
        }

        private void comboBoxProcessing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxProcessing.SelectedIndex > -1 && this.comboBoxProcessing.DataSource != null)
            {
                int i = 0;
                if (this.comboBoxProcessing.SelectedValue.GetType() == typeof(Int32))
                {
                    i = int.Parse(this.comboBoxProcessing.SelectedValue.ToString());
                }
                else if (this.comboBoxProcessing.SelectedValue.GetType() == typeof(System.Data.DataRowView))
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxProcessing.SelectedValue;
                    i = int.Parse(R.Row["ProcessingID"].ToString());
                }
                DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingID = i;
                DiversityCollection.Forms.FormPartGridSettings.Default.Save();
            }
        }

        #endregion   

        #region Grid

        private void GridModeFillTable()
        {
            try
            {
                string SQL = this.GridModeFillCommand(); ;
                this.dataSetPartGrid.FirstLinesPart.Clear();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(900000));
                ad.Fill(this.dataSetPartGrid.FirstLinesPart);
                this.dataGridView_RowEnter(null, null);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string M = ex.Message;
                if (M.IndexOf("timeout") > -1) M += "\r\nPlease ask your database administrator to recreate the index for the table IdentificationUnit";
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
            if (DiversityCollection.Forms.FormPartGridSettings.Default.ColumnWidth.Length > 0)
            {
                string[] ColumnWidths = DiversityCollection.Forms.FormPartGridSettings.Default.ColumnWidth.Split(new char[] { ' ' });
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
                if (DiversityCollection.Forms.FormPartGridSettings.Default.ColumnSequence.Length > 0)
                {
                    string[] Sequence = DiversityCollection.Forms.FormPartGridSettings.Default.ColumnSequence.Split(new char[] { ' ' });
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
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
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
                        foreach (GridModeQueryField Q in this.GridModeQueryFields)
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
                        string[] HeaderParts = C.HeaderText.Split(new char[] { '_' });
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
        }

        private void buttonOptColumnWidth_Click(object sender, EventArgs e)
        {
            this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void buttonFilterGrid_Click(object sender, EventArgs e)
        {
            //this.CheckForChangesAndAskForSaving(null, null);
            if (this.dataSetPartGrid.HasChanges())
            {
                if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.textBoxHeaderAccessionNumber.Focus();
                    this.SaveAll();
                    System.Windows.Forms.MessageBox.Show("Please try to filter again");
                    this.dataGridView.CurrentCell = null;
                    return;
                }
            }
            if (this.dataGridView.SelectedCells != null)
            {
                if (this.dataGridView.SelectedCells[0].Value != null)
                {
                    string Filter = this.dataGridView.SelectedCells[0].Value.ToString();
                    System.Collections.Generic.List<DataRow> RowsToRemove = new List<DataRow>();
                    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                    {
                        if (R.Cells[this.dataGridView.SelectedCells[0].ColumnIndex].Value.ToString() != Filter)
                        {
                            System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
                            RowsToRemove.Add(Rdata.Row);
                        }
                    }
                    foreach (System.Data.DataRow R in RowsToRemove)
                        R.Delete();
                    //foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                    //{
                    //    try
                    //    {
                    //        if (R.Cells[this.dataGridView.SelectedCells[0].ColumnIndex].Value.ToString() != Filter)
                    //        {
                    //            System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
                    //            Rdata.Delete();
                    //            Rdata.Row.AcceptChanges();
                    //        }
                    //    }
                    //    catch (System.Exception ex) { }
                    //}
                    this.dataSetPartGrid.AcceptChanges();
                }
            }
        }

        private void buttonRemoveRowFromGrid_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count > 0)
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
                {
                    try
                    {
                        System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
                        Rdata.Delete();
                        Rdata.Row.AcceptChanges();
                    }
                    catch (System.Exception ex) { }
                }
                this.dataSetPartGrid.AcceptChanges();
            }
            else
                System.Windows.Forms.MessageBox.Show("No rows selected");
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
                        int SpecimenPartID = 0;
                        if (int.TryParse(this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out SpecimenPartID))
                        {
                            for (i = 0; i < this.dataSetPartGrid.FirstLinesPart.Rows.Count; i++)
                            {
                                if (this.dataSetPartGrid.FirstLinesPart.Rows[i].RowState == DataRowState.Deleted
                                    || this.dataSetPartGrid.FirstLinesPart.Rows[i].RowState == DataRowState.Detached)
                                    continue;
                                if (this.dataSetPartGrid.FirstLinesPart.Rows[i]["SpecimenPartID"].ToString() == SpecimenPartID.ToString())
                                    break;
                            }
                        }
                    }
                }
                catch { }
                return i;
            }
        }

        private int DatasetIndexOfLine(int SpecimenPartID)
        {
            int i = 0;
            try
            {
                for (i = 0; i < this.dataSetPartGrid.FirstLinesPart.Rows.Count; i++)
                {
                    if (this.dataSetPartGrid.FirstLinesPart.Rows[i]["SpecimenPartID"].ToString() == SpecimenPartID.ToString())
                        break;
                }

            }
            catch { }
            return i;
        }

        private int GridIndexOfDataline(int SpecimenPartID)
        {
            int i = 0;
            try
            {
                if (this.dataGridView.Rows.Count > 0)
                {
                    for (i = 0; i < this.dataGridView.Rows.Count; i++)
                    {
                        if (this.dataGridView.Rows[i].Cells[0].Value.ToString() == SpecimenPartID.ToString())
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
                this.GridModeFillTable();
            }
            catch { }
        }

        private void buttonResetSequence_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormPartGridSettings.Default.ColumnSequence = "";
            this.setColumnSequence();
        }

        private void buttonGridModeUpdateColumnSettings_Click(object sender, EventArgs e)
        {
            this._GridModeColumnList = null;
            this._GridModeQueryFields = null;
            DiversityCollection.Forms.FormPartGridSettings.Default.Visibility = "";
            this.GridModeSetColumnVisibility();
            this.enableReplaceButtons(false);
        }

        private void buttonGridModeFindUsedColumns_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.FindUsedDataColumns(this.dataSetPartGrid.FirstLinesPart, this.treeViewGridModeFieldSelector);
            this.buttonGridModeUpdateColumnSettings_Click(null, null);
        }

        private void buttonFindEditedColumns_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.FindEditedColumns(this.dataSetPartGrid.FirstLinesPart, this.treeViewGridModeFieldSelector, this.ReadOnlyColumns);
            this.buttonGridModeUpdateColumnSettings_Click(null, null);
        }

        #endregion

        #region Button events for Finding, Copy and Saving and related functions

        private void buttonGridModeFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataSetPartGrid.FirstLinesPart.Rows.Count == 0
                    || this.dataGridView.SelectedCells.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show(this.Message("Nothing_selected"));
                    return;
                }
                if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.SaveAll();
                int ID = 0;
                if (int.TryParse(this.dataSetPartGrid.FirstLinesPart.Rows[DatasetIndexOfCurrentLine]["CollectionSpecimenID"].ToString(), out ID))
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
            this.dataSetPartGrid.FirstLinesPart.Rows[DatasetIndexOfCurrentLine].BeginEdit();
            this.dataSetPartGrid.FirstLinesPart.Rows[DatasetIndexOfCurrentLine].EndEdit();
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
                if (this.dataSetPartGrid.HasChanges())
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
                this.progressBarSaveAll.Maximum = this.dataSetPartGrid.FirstLinesPart.Rows.Count;
                this.progressBarSaveAll.Value = 0;
                for (int i = 0; i < this.dataSetPartGrid.FirstLinesPart.Rows.Count; i++)
                {
                    if (this.dataSetPartGrid.FirstLinesPart.Rows[i].RowState == DataRowState.Deleted
                        || this.dataSetPartGrid.FirstLinesPart.Rows[i].RowState == DataRowState.Detached
                        || this.dataSetPartGrid.FirstLinesPart.Rows[i].RowState == DataRowState.Unchanged)
                        continue;
                    this.dataGridView.Rows[i].Cells[0].Selected = true;
                    this.dataSetPartGrid.FirstLinesPart.Rows[i].BeginEdit();
                    this.dataSetPartGrid.FirstLinesPart.Rows[i].EndEdit();
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
                    System.Data.DataRow Rori = this.dataSetPartGrid.FirstLinesPart.Rows[this.dataGridView.SelectedCells[0].RowIndex];
                    System.Data.DataRow Rnew = this.dataSetPartGrid.FirstLinesPart.NewFirstLinesPartRow();
                    Rnew[0] = this.InsertNewPart(this._SpecimenID);
                    for (int i = 1; i < this.dataSetPartGrid.FirstLinesPart.Columns.Count; i++)
                    {
                        string ColumnName = this.dataSetPartGrid.FirstLinesPart.Columns[i].ColumnName;
                        Rnew[ColumnName] = Rori[ColumnName];
                    }
                    this.dataSetPartGrid.FirstLinesPart.Rows.Add(Rnew);
                }
            }
            catch { }
        }

        private void buttonGridModeDelete_Click(object sender, EventArgs e)
        {
            bool DeleteSelection = false;
            this.buttonGridModeDelete.Enabled = false;
            System.Collections.Generic.List<int> PartIDsToDelete = new List<int>();
            try
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)R.DataBoundItem;
                    int PartID = 0;
                    if (int.TryParse(RV["SpecimenPartID"].ToString(), out PartID))
                    {
                        string AccessionNumber = RV["Part_accession_number"].ToString();
                        string StorageLocation = RV["Storage_location"].ToString();
                        if (!DeleteSelection)
                        {
                            string Message = DiversityWorkbench.Entity.Message("Do_you_want_to_delete_the_dataset");
                            if (AccessionNumber.Length > 0) Message += "\r\n" + DiversityWorkbench.Entity.EntityInformation("CollectionSpecimenPart.AccessionNumber")["DisplayText"] + ": " + AccessionNumber;
                            if (StorageLocation.Length > 0) Message += "\r\n" + DiversityWorkbench.Entity.EntityInformation("CollectionSpecimenPart.StorageLocation")["DisplayText"] + ": " + StorageLocation;
                            Message += "\r\nID: " + PartID.ToString();
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
                        PartIDsToDelete.Add(PartID);
                    }
                }
                if (DeleteSelection)
                {
                    foreach (int ID in PartIDsToDelete)
                    {
                        this.deleteSpecimenPart(ID);
                    }
                    this.GridModeFillTable();
                }
            }
            catch { }


            //bool DeleteSelection = false;
            //this.buttonGridModeDelete.Enabled = false;
            //System.Collections.Generic.List<int> IDsToDelete = new List<int>();
            //try
            //{
            //    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
            //    {
            //        System.Data.DataRowView RV = (System.Data.DataRowView)R.DataBoundItem;
            //        int ID = 0;
            //        if (int.TryParse(RV["CollectionSpecimenID"].ToString(), out ID))
            //        {
            //            string AccessionNumber = RV["Accession_Number"].ToString();
            //            if (!DeleteSelection)
            //            {
            //                string Message = DiversityWorkbench.Entity.Message("Do_you_want_to_delete_the_dataset");
            //                if (AccessionNumber.Length > 0) Message += "\r\n" + DiversityWorkbench.Entity.EntityInformation("CollectionSpecimen.AccessionNumber")["DisplayText"] + ": " + AccessionNumber;
            //                else Message += " ID " + ID.ToString();
            //                if (this.dataGridView.SelectedRows.Count > 1)
            //                    Message += " " + DiversityWorkbench.Entity.Message("and") + " " + (this.dataGridView.SelectedRows.Count - 1).ToString() + " " + this.Message("further_datasets") + "?";
            //                if (System.Windows.Forms.MessageBox.Show(Message, DiversityWorkbench.Entity.Message("delete_entry"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //                    return;
            //                else
            //                    DeleteSelection = true;
            //            }
            //        }
            //        if (DeleteSelection)
            //        {
            //            IDsToDelete.Add(ID);
            //        }
            //    }
            //    if (DeleteSelection)
            //    {
            //        foreach (int ID in IDsToDelete)
            //        {
            //            this.deleteSpecimen(ID);
            //        }
            //        this.GridModeFillTable();
            //    }
            //}
            //catch { }
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

        /// <summary>
        /// delete a specimen part from the database
        /// </summary>
        /// <param name="ID">the Primary key of table CollectionSpecimen corresponding to the item that should be deleted</param>
        private void deleteSpecimenPart(int SpecimenPartID)
        {
            try
            {
                string SQL = "DELETE FROM CollectionSpecimenPart WHERE SpecimenPartID = " + SpecimenPartID.ToString();
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

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bool OK = this.FormFunctions.getObjectPermissions("CollectionSpecimenPart", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
                if (!OK) 
                    return;

                this.buttonGridModeDelete.Enabled = false;
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                System.Data.DataTable dtProcessing = new DataTable();
                if (ColumnName.IndexOf("Processing") > -1)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
                    string SQL = "SELECT ProcessingID, DisplayText, ProcessingParentID " +
                        "FROM dbo.ProcessingListForPart(" + R["CollectionSpecimenID"].ToString() + ", " + R["SpecimenPartID"].ToString() + ") AS P ";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dtProcessing);
                    if (dtProcessing.Rows.Count == 0)
                    {
                        foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells)
                        {
                            if (C.DataGridView.Columns[C.ColumnIndex].DataPropertyName.IndexOf("Processing") > -1)
                            {
                                C.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly;
                                C.ReadOnly = true;
                            }
                        }
                    }
                    else
                    {
                        string ProcessingEntry = ColumnName.Substring(ColumnName.Length - 1);
                        int i;
                        if (int.TryParse(ProcessingEntry, out i))
                        {
                            if (R["ProcessingID_" + ProcessingEntry].Equals(System.DBNull.Value))
                            {
                                if (System.Windows.Forms.MessageBox.Show("Do you want to insert a new processing?", "New processing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    DiversityWorkbench.Forms.FormGetItemFromHierarchy f = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(dtProcessing, "ProcessingID", "ProcessingParentID", "DisplayText", "ProcessingID", "Please select a processing", "Processing");
                                    f.StartPosition = FormStartPosition.CenterParent;
                                    f.ShowDialog();
                                    if (f.DialogResult == DialogResult.OK)
                                    {
                                        R["ProcessingID_" + ProcessingEntry] = f.SelectedValue;
                                        if (R["Processing_date_" + ProcessingEntry].Equals(System.DBNull.Value))
                                            R["Processing_date_" + ProcessingEntry] = System.DateTime.Now;
                                    }
                                }
                            }
                        }
                    }
                }
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
                        case "On_loan":
                        case "_TransactionID":
                            int TransactionID;
                            System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
                            if (R["_TransactionID"].Equals(System.DBNull.Value))
                            {
                                DiversityCollection.Forms.FormTransaction f = new FormTransaction(true);
                                f.ShowDialog();
                                if (f.DialogResult == DialogResult.OK)
                                {
                                    R.BeginEdit();
                                    R["_TransactionID"] = f.ID;
                                    R["_Transaction"] = f.TransactionTitle;
                                    R["On_loan"] = 1;
                                    R.EndEdit();
                                }
                            }
                            else
                            {
                                if (ColumnName == "_TransactionID")
                                {
                                    if (int.TryParse(R["_TransactionID"].ToString(), out TransactionID))
                                    {
                                        DiversityCollection.Forms.FormTransaction f = new FormTransaction(TransactionID);
                                        f.ShowDialog();
                                    }
                                    //else
                                    //{
                                    //    if (ColumnName == "_TransactionID" || ColumnName == "On_loan")
                                    //    {
                                    //    }
                                    //}
                                }
                                else if (ColumnName == "On_loan")
                                {
                                    R.BeginEdit();
                                    if (R["On_loan"].ToString() == "1")
                                        R["On_loan"] = 0;
                                    else R["On_loan"] = 1;
                                    R.EndEdit();
                                }
                            }
                            break;
                        //case "Link_to_DiversityTaxonNames":
                        //case "Link_to_DiversityReferences":
                        //case "Link_to_DiversityAgents_for_responsible":
                        //    this.GetRemoteValues(this.dataGridView.SelectedCells[0]);
                        //    break;
                        //case "Remove_link_for_identification":
                        //case "Remove_link_for_reference":
                        //case "Remove_link_for_determiner":
                        //    this.RemoveLink(this.dataGridView.SelectedCells[0]);
                        //    break;

                        //case "ProcessingID_1":
                        //case "ProcessingID_2":
                        //case "ProcessingID_3":
                        //case "ProcessingID_4":
                        //case "ProcessingID_5":
                        //    System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
                        //    string SQL = "SELECT     ProcessingID, DisplayText, ProcessingParentID " +
                        //        "FROM dbo.ProcessingListForPart(" + R["CollectionSpecimenID"].ToString() + ", " + R["SpecimenPartID"].ToString() + ") AS P ";
                        //    System.Data.DataTable dt = new DataTable();
                        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        //    ad.Fill(dt);
                        //    if (dt.Rows.Count == 0)
                        //    {
                        //        foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells)
                        //        {
                        //            if (C.DataGridView.Columns[C.ColumnIndex].DataPropertyName.IndexOf("Processing") > -1)
                        //            {
                        //                C.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly;// this._StyleReadOnly;
                        //                C.ReadOnly = true;
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        DiversityWorkbench.Forms.FormGetItemFromHierarchy f = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(dt, "ProcessingID", "ProcessingParentID", "DisplayText", "ProcessingID", "Please select a processing", "Processing");
                        //        f.StartPosition = FormStartPosition.CenterParent;
                        //        f.ShowDialog();
                        //        if (f.DialogResult == DialogResult.OK)
                        //        {
                        //            R[ColumnName] = f.SelectedValue;
                        //        }
                        //    }
                        //    //string MaterialCategory = R["Material_category"].ToString();
                        //    break;
                    }
                }
                if (this.textBoxHeaderID.Text.Length == 0)
                    this.setSpecimen(this.SpecimenID, this._PartID);

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
                    )
                    this.enableReplaceButtons(true);
                else this.enableReplaceButtons(false);

                this.labelGridCounter.Text = DiversityCollection.Forms.FormGridFunctions.GridCounter(this.dataGridView);
                //if (this.dataGridView.SelectedCells.Count == 1)
                //    this.labelGridCounter.Text = "line " + (this.dataGridView.SelectedCells[0].RowIndex + 1).ToString() + " of " + (this.dataGridView.Rows.Count - 1).ToString();
                //else if (this.dataGridView.SelectedCells.Count > 1)
                //{
                //    int StartLine = this.dataGridView.SelectedCells[0].RowIndex + 1;
                //    if (this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1 < StartLine)
                //        StartLine = this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1;
                //    this.labelGridCounter.Text = "line " + StartLine.ToString() + " to " +
                //        (this.dataGridView.SelectedCells.Count + StartLine - 1).ToString() + " of " +
                //        (this.dataGridView.Rows.Count - 1).ToString();
                //}
                //else
                //    this.labelGridCounter.Text = "line 1 of " + (this.dataGridView.Rows.Count - 1).ToString();

            }
            catch { }
        }

        private void dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView.SelectedCells.Count > 0 &&
                     this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                     this.dataSetPartGrid.FirstLinesPart.Rows.Count > 0)
                this.checkForMissingAndDefaultValues(this.dataGridView.SelectedCells[0], false);

        }

        private void checkForMissingAndDefaultValues(System.Windows.Forms.DataGridViewCell Cell, bool Silent)
        {
            try
            {
                if (this.dataGridView.SelectedCells.Count > 0 &&
                     this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                     this.dataSetPartGrid.FirstLinesPart.Rows.Count > 0)
                {
                    string ColumnName = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
                    string Message = "";
                    string Value = Cell.EditedFormattedValue.ToString();
                    System.Data.DataRow Rcurr = this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex];

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
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Taxonomic_group_of_second_organism"].Equals(System.DBNull.Value)
                                && !Silent)
                                System.Windows.Forms.MessageBox.Show("Please select a taxonomic group for this organism");
                            break;
                        case "AnalysisID":
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Analysis_number"].Equals(System.DBNull.Value))
                            {
                                this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Analysis_number"] = 1;
                            }
                            goto case "Exsiccata_number";
                        case "Analysis_number":
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["AnalysisID"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnitAnalysis.AnalysisID")
                                    && this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"].Length > 0)
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["AnalysisID"] = this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"];
                                else
                                    if (!Silent) Message += "Please select an analysis type\r\n";
                            }
                            goto case "Exsiccata_number";
                        case "Analysis_result":
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Analysis_number"].Equals(System.DBNull.Value))
                            {
                                this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Analysis_number"] = 1;
                            }
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["AnalysisID"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnitAnalysis.AnalysisID")
                                    && this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"].Length > 0)
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["AnalysisID"] = this.ProjectSettings["IdentificationUnitAnalysis.AnalysisID"];
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
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Taxonomic_group"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup")
                                    && this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Taxonomic_group"] = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
                                else
                                    if (!Silent) Message += "Please select a taxonomic group for this organism";
                            }
                            break;
                        case "Collection":
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Material_category"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Material_category"] = this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"];
                                else
                                    if (!Silent) Message = "Please select a material category for this part.";
                            }
                            break;
                        case "Material_category":
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Collection"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Collection"] = this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
                                else
                                    if (!Silent) Message = "Please select a collection for this part.";
                            }
                            break;
                        case "Storage_location":
                        case "Stock":
                        case "Preparation_method":
                        case "Preparation_date":
                        case "Notes_for_part":
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Material_category"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
                                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Material_category"] = this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"];
                                else
                                    if (!Silent) Message = "Please select a material category for this part.\r\n";
                            }
                            if (this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Collection"].Equals(System.DBNull.Value))
                            {
                                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
                                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Cell.RowIndex]["Collection"] = this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
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
            catch { }
        }

        private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this._FormState == FormGridFunctions.FormState.Loading)
                    return;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                int SpecimenID = 0;
                int PartID = 0;
                int iCurrentLine = DatasetIndexOfCurrentLine - 1;
                if (iCurrentLine == -1) iCurrentLine = 0;
                if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetPartGrid.FirstLinesPart.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex < this.dataSetPartGrid.FirstLinesPart.Rows.Count &&
                    int.TryParse(this.dataSetPartGrid.FirstLinesPart.Rows[iCurrentLine]["CollectionSpecimenID"].ToString(), out SpecimenID) &&
                    int.TryParse(this.dataSetPartGrid.FirstLinesPart.Rows[iCurrentLine]["SpecimenPartID"].ToString(), out PartID))
                {
                    if (SpecimenID != this._SpecimenID ||
                        PartID != this._PartID)
                    {
                        this.setSpecimen(SpecimenID, PartID);
                        this._SpecimenID = SpecimenID;
                        this._PartID = PartID;
                    }
                }
                else if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetPartGrid.FirstLinesPart.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetPartGrid.FirstLinesPart.Rows.Count)
                {
                    this.insertNewDataset(this.dataGridView.SelectedCells[0].RowIndex);
                }
                else if (this.dataGridView.SelectedCells.Count == 0
                    && this.dataGridView.Rows.Count > 0
                    && this.dataGridView.Rows[0].Cells[1].Value != null)
                {
                    if (int.TryParse(this.dataGridView.Rows[0].Cells[1].Value.ToString(), out SpecimenID) &&
                        int.TryParse(this.dataGridView.Rows[0].Cells[0].Value.ToString(), out PartID))
                    {
                        this.setSpecimen(SpecimenID, PartID);
                        this._SpecimenID = SpecimenID;
                        this._PartID = PartID;
                    }
                }
                else if (this.dataSetPartGrid.FirstLinesPart.Rows.Count == 0)
                {
                    this.insertNewDataset(0);
                }
                this.setCellBlockings();
                this.setRemoveCellStyle();

            }
            catch { }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private string GridModeFillCommand()
        {
            string SQL = "SELECT " + this._SqlSpecimenFields + " FROM dbo." + this.SourceFunction + " ";

            try
            {
                string WhereClause = "";
                int iCount = 0;
                foreach (int i in this._IDs)
                {
                    iCount++;
                    WhereClause += i.ToString() + ", ";
                    if (WhereClause.Length > 8000)
                    {
                        System.Windows.Forms.MessageBox.Show("Only the first " + iCount.ToString() + " datasets can be displayed");
                        break;
                    }
                }
                if (WhereClause.Length == 0) WhereClause = " ('') ";
                else
                    WhereClause = " ('" + WhereClause.Substring(0, WhereClause.Length - 2) + "'";
                WhereClause += ", ";
                if (DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs != null &&
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs.Count > 0)
                {
                    WhereClause += "'";
                    foreach (string s in DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisIDs)
                        WhereClause += s + ",";
                    WhereClause = WhereClause.Substring(0, WhereClause.Length - 1) + "'";
                }
                else
                    WhereClause += "null";
                WhereClause += ", ";
                /*CONVERT(DATETIME, '2000-03-31 00:00:00', 102))*/
                if (DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate != null &&
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate.Year > 1)
                {
                    WhereClause += " CONVERT(DATETIME, '" + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate.Year.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate.Month.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisStartDate.Day.ToString() + " 00:00:00', 102)";
                }
                else
                    WhereClause += "null";
                WhereClause += ", ";
                if (DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate != null &&
                    DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate.Year > 1)
                {
                    WhereClause += " CONVERT(DATETIME,'" + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate.Year.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate.Month.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.AnalysisEndDate.Day.ToString() + " 00:00:00', 102)";
                }
                else
                    WhereClause += "null";
                WhereClause += " , ";
                if (DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingID == 0)
                    WhereClause += "null";
                else 
                    WhereClause += DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingID.ToString();
                WhereClause += " , ";
                if (DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate != null &&
                    DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate.Year > 1 &&
                    this.checkBoxProcessingRange.Checked)
                {
                    WhereClause += " CONVERT(DATETIME,'" + DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate.Year.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate.Month.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingStartDate.Day.ToString() + " 00:00:00', 102)";
                }
                else
                    WhereClause += "null";
                WhereClause += ", ";
                if (DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate != null &&
                    DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate.Year > 1 &&
                    this.checkBoxProcessingRange.Checked)
                {
                    WhereClause += " CONVERT(DATETIME, '" + DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate.Year.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate.Month.ToString() +
                        "-" + DiversityCollection.Forms.FormPartGridSettings.Default.ProcessingEndDate.Day.ToString() + " 00:00:00', 102)";
                }
                else
                    WhereClause += "null";
                WhereClause += ")";
                SQL += WhereClause + " ORDER BY Accession_number, CollectionSpecimenID, SpecimenPartID ";
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
                if (this.dataSetPartGrid.HasChanges())
                {
                    System.Data.DataRow RDataset = this.dataSetPartGrid.FirstLinesPart.Rows[Index];
                    if (RDataset.RowState == DataRowState.Modified || RDataset.RowState == DataRowState.Added)
                    {
                        // setting the dataset
                        // the dataset is filled with the original data from the database as a basis for comparision with the data in the grid
                        int CollectionSpecimenID = int.Parse(this.dataSetPartGrid.FirstLinesPart.Rows[Index]["CollectionSpecimenID"].ToString());
                        int SpecimenPartID = int.Parse(this.dataSetPartGrid.FirstLinesPart.Rows[Index]["SpecimenPartID"].ToString());
                        this._SpecimenID = CollectionSpecimenID;
                        this._PartID = SpecimenPartID;
                        this.fillSpecimen(this._SpecimenID, this._PartID);

                        // if no data 
                        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count == 0)
                            this.setSpecimen(this._SpecimenID, this._PartID);

                        // getting a list of all tables (Alias + TableName) from the grid
                        System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();
                        int iii = 0;
                        foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
                        {
                            try
                            {
                                if (Q.AliasForTable != null)
                                {
                                    if (!Tables.ContainsKey(Q.AliasForTable))
                                    {
                                        try
                                        {
                                            Tables.Add(Q.AliasForTable, Q.Table);
                                        }
                                        catch (System.Exception ex) { }
                                    }
                                }
                                else
                                {
                                    if (!Tables.ContainsKey(Q.Table))
                                    {
                                        try
                                        {
                                            Tables.Add(Q.Table, Q.Table);
                                        }
                                        catch (System.Exception ex) { }
                                    }
                                }
                            }
                            catch (System.Exception ex) { }
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
                                    try
                                    {
                                        TableColumns.Add(Q.AliasForColumn, Q.Column);
                                        ColumnValues.Add(Q.Column, "");
                                    }
                                    catch (System.Exception ex) { }
                                }
                                iii++;
                            }

                            // no rows in the table - add new entry
                            // check if there are any values
                            bool AnyValuePresent = false;
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVc in TableColumns)
                            {
                                if (this.dataSetPartGrid.FirstLinesPart.Columns.Contains(KVc.Key))
                                {
                                    ColumnValues[KVc.Value] = this.dataSetPartGrid.FirstLinesPart.Rows[Index][KVc.Key].ToString();
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
                                {
                                    try
                                    {
                                        PKColumns.Add(PK[i].ColumnName);
                                    }
                                    catch (System.Exception ex) { }
                                }

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
                                            try
                                            {
                                                ColumnAlias = KvPK.Key;
                                                PKvalues.Add(KvPK.Key, "");
                                            }
                                            catch (System.Exception ex) { }
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
                                            string[] WW = Value.Split(new char[] { '\'' });
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
                                                            WhereClause += " IdentificationUnitID = " + this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationUnitID"].ToString();
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
                                                case "ProcessingID":
                                                    switch (KV.Key)
                                                    {
                                                        case "ProcessingID_1":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " ProcessingID = " + this.AnalysisList[1].AnalysisID;
                                                            break;
                                                        case "ProcessingID_2":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " ProcessingID = " + this.AnalysisList[2].AnalysisID;
                                                            break;
                                                        case "ProcessingID_3":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " ProcessingID = " + this.AnalysisList[3].AnalysisID;
                                                            break;
                                                        case "ProcessingID_4":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " ProcessingID = " + this.AnalysisList[4].AnalysisID;
                                                            break;
                                                        case "ProcessingID_5":
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += " ProcessingID = " + this.AnalysisList[5].AnalysisID;
                                                            break;
                                                    }
                                                    break;
                                                case "IdentificationSequence":
                                                    switch (KV.Key)
                                                    {
                                                        case "Identification":
                                                            if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationSequence"].ToString();
                                                            }
                                                            break;
                                                        case "SecondUnitIdentification":
                                                            if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SecondSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SecondSequence"].ToString();
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case "TransactionID":
                                                    switch (KV.Key)
                                                    {
                                                        case "Identification":
                                                            if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationSequence"].ToString();
                                                            }
                                                            break;
                                                        case "SecondUnitIdentification":
                                                            if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SecondSequence"].Equals(System.DBNull.Value))
                                                            {
                                                                if (WhereClause.Length > 0) WhereClause += " AND ";
                                                                WhereClause += " IdentificationSequence = " + this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SecondSequence"].ToString();
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

                                    if (RR.Length == 1)
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
                                            if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_CollectionEventID"].Equals(System.DBNull.Value) &&
                                                this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
                                                this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"] = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_CollectionEventID"];
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
                                catch (System.Exception ex) { }
                            }
                        }
                        this.updateSpecimen();
                        RDataset.AcceptChanges();
                    }
                }
            }
            catch (System.Exception ex) { }
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
                    if (this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_group_of_second_organism"].Equals(System.DBNull.Value) ||
                        this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_group_of_second_organism"].ToString().Length == 0)
                    {
                        string Message = "Please enter the taxonomic group for the second organism ";
                        if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value))
                            Message += this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].ToString() + " ";
                        if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Accession_number"].Equals(System.DBNull.Value))
                            Message += this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Accession_number"].ToString();
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
                        && Q.Column.Length > 0)
                    {
                        TableColumns.Add(Q.AliasForColumn, Q.Column);
                        ColumnValues.Add(Q.Column, "");
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> KVc in TableColumns)
                {
                    ColumnValues[KVc.Value] = this.dataSetPartGrid.FirstLinesPart.Rows[Index][KVc.Key].ToString();
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
                        if (!this.dataSetPartGrid.FirstLinesPart.Columns.Contains(AliasForColumn))
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
                        ColumnValues[KVpk.Key] = this.dataSetPartGrid.FirstLinesPart.Rows[Index][AliasForColumn].ToString();
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
                                if (!this.dataSetPartGrid.FirstLinesPart[Index]["Locality_description"].Equals(System.DBNull.Value))
                                    this.dataSetPartGrid.FirstLinesPart[Index]["Locality_description"].ToString();
                                Locality = this.dataSetPartGrid.FirstLinesPart[Index]["Locality_description"].ToString();
                                int EventID = this.createEvent(Index);
                                this.dataSetPartGrid.FirstLinesPart[Index]["_CollectionEventID"] = EventID;
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
                                        TaxonomicGroup = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_group"].ToString();
                                        if (this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
                                            LastIdentification = TaxonomicGroup;
                                        else
                                            LastIdentification = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name"].ToString();
                                        DisplayOrder = 1;
                                        //this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Last_identification"] = LastIdentification;
                                        break;
                                    case "SecondUnit":
                                        TaxonomicGroup = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
                                        if (this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
                                            LastIdentification = TaxonomicGroup;
                                        else
                                            LastIdentification = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
                                        DisplayOrder = 2;
                                        break;
                                }
                                if (TaxonomicGroup.Length > 0 && LastIdentification.Length > 0)
                                {
                                    int UnitID = this.createSpecimenPart(Index, AliasForTable);
                                    ColumnValues[KVpk.Key] = UnitID.ToString();
                                    switch (AliasForTable)
                                    {
                                        case "IdentificationUnit":
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationUnitID"] = UnitID;
                                            break;
                                        case "SecondUnit":
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SecondUnitID"] = UnitID;
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
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationSequence"] = Sequence;
                                            break;
                                        case "IdentificationSecondUnit":
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SecondSequence"] = Sequence;
                                            break;
                                    }
                                }
                                else
                                {
                                    ColumnValues[KVpk.Key] = "1";
                                    switch (AliasForTable)
                                    {
                                        case "Identification":
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_IdentificationSequence"] = 1;
                                            break;
                                        case "IdentificationSecondUnit":
                                            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SecondSequence"] = 1;
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
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_SpecimenPartID"] = PartID;
                                }
                                break;
                            case "TransactionID":
                                System.Data.DataRow[] RRTrans = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("", "SpecimenPartID DESC");
                                if (RRTrans.Length > 0)
                                {
                                    int TransID = int.Parse(RRTrans[0]["TransactionID"].ToString()) + 1;
                                    ColumnValues[KVpk.Key] = TransID.ToString();
                                    this.dataSetPartGrid.FirstLinesPart.Rows[Index]["_TransactionID"] = TransID;
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
                        {
                            if (Rnew.Table.Columns[KV.Key].DataType == typeof(System.Boolean))
                            {
                                if (KV.Value == "1")
                                    Rnew[KV.Key] = true;
                                else
                                    Rnew[KV.Key] = false;
                            }
                            else 
                                Rnew[KV.Key] = KV.Value;
                        }
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
                    !this.dataSetPartGrid.FirstLinesPart[Index]["_CollectionEventID"].Equals(System.DBNull.Value))
                    Rnew["CollectionEventID"] = this.dataSetPartGrid.FirstLinesPart[Index]["_CollectionEventID"];

                //if (Table == "CollectionSpecimenPart" &&
                //    Rnew["SpecimenPartID"].Equals(System.DBNull.Value))
                //    Rnew["SpecimenPartID"] = 1;

                if (Table == "IdentificationUnitAnalysis" &&
                    Rnew["AnalysisNumber"].Equals(System.DBNull.Value))
                    Rnew["AnalysisNumber"] = "1";

                if (Table == "IdentificationUnitAnalysis" &&
                    Rnew["AnalysisID"].Equals(System.DBNull.Value))
                {
                    switch (AliasForTable)
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

                if (Table == "IdentificationUnitAnalysis")
                    Rnew["SpecimenPartID"] = this.PartID;

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

                if (Table == "CollectionSpecimenProcessing")
                {
                    //switch (AliasForTable)
                    //{
                    //    case "Processing_1":
                    //        break;
                    //}
                    Rnew["SpecimenPartID"] = this.PartID;
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
            catch (System.Exception ex) { }
        }

        #region Remote services

        private void GetCoordinatesFromGoogleMaps()
        {
            string Latitude = "";
            string Longitude = "";
            try
            {
                if (this.dataSetPartGrid.FirstLinesPart.Rows[DatasetIndexOfCurrentLine]["Latitude"].Equals(System.DBNull.Value))
                {
                    Latitude = this.dataSetPartGrid.FirstLinesPart.Rows[DatasetIndexOfCurrentLine]["Latitude"].ToString();
                    Longitude = this.dataSetPartGrid.FirstLinesPart.Rows[DatasetIndexOfCurrentLine]["Longitude"].ToString();
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
                            System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
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
                        RvbCountry.BindingSource = this.firstLinesPartBindingSource;
                        RvbCountry.Column = "Country";
                        RvbCountry.RemoteParameter = "Country";
                        RemoteValueBindings.Add(RvbCountry);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLatitude.BindingSource = this.firstLinesPartBindingSource;
                        RvbLatitude.Column = "_NamedAverageLatitudeCache";
                        RvbLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLongitude.BindingSource = this.firstLinesPartBindingSource;
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
                        RvbSamplingPlotLatitude.BindingSource = this.firstLinesPartBindingSource;
                        RvbSamplingPlotLatitude.Column = "Latitude_of_sampling_plot";
                        RvbSamplingPlotLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotLongitude.BindingSource = this.firstLinesPartBindingSource;
                        RvbSamplingPlotLongitude.Column = "Longitude_of_sampling_plot";
                        RvbSamplingPlotLongitude.RemoteParameter = "Longitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLongitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotAccuracy = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotAccuracy.BindingSource = this.firstLinesPartBindingSource;
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
                        RvbHierarchyLitho.BindingSource = this.firstLinesPartBindingSource;
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
                        RvbHierarchyChrono.BindingSource = this.firstLinesPartBindingSource;
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
                        RvbFamily.BindingSource = this.firstLinesPartBindingSource;
                        RvbFamily.Column = "Family_of_taxon";
                        RvbFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbOrder.BindingSource = this.firstLinesPartBindingSource;
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
                        RvbSecondFamily.BindingSource = this.firstLinesPartBindingSource;
                        RvbSecondFamily.Column = "_SecondUnitFamilyCache";
                        RvbSecondFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbSecondFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSecondOrder.BindingSource = this.firstLinesPartBindingSource;
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

                if (this.firstLinesPartBindingSource != null && IWorkbenchUnit != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
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
                        System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
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

                if (this.firstLinesPartBindingSource != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesPartBindingSource.Current;
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
            for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
                this.setRemoveCellStyle(i);
        }

        private void setRemoveCellStyle(int RowIndex)
        {
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

        #region Blocking of Cells that are linked to external services

        private void setCellBlockings()
        {
            for (int i = 0; i < this.dataGridView.Rows.Count; i++)
            {
                this.setCellBlockings(i);
                this.setCellBlockingsForAnalysis(i);
            }
        }

        private void setCellBlockings(int RowIndex)
        {
            try
            {
                if (this._RowsWithBlockingSet == null) this._RowsWithBlockingSet = new List<DataGridViewRow>();
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this._RowsWithBlockingSet.Contains(this.dataGridView.Rows[RowIndex]))
                        continue;
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
                    this._RowsWithBlockingSet.Add(this.dataGridView.Rows[RowIndex]);
                }
            }
            catch (System.Exception ex) { }
        }

        private void setCellBlockingsForAnalysis(int RowIndex)
        {
            try
            {
                string TaxonomicGroup = this.dataSetPartGrid.FirstLinesPart.Rows[RowIndex]["Taxonomic_group"].ToString();
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
                                    if (this.AnalysisList.Count > 0 && (this.TaxonAnalysisDict[TaxonomicGroup].Contains(this.AnalysisList[i].AnalysisID)))
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
                        if (Q.Table != "CollectionSpecimenPart" &&
                            Q.Table != "CollectionSpecimenProcessing" &&
                            Q.Table != "CollectionSpecimenTransaction" &&
                            Q.Table != "IdentificationUnitInPart" &&
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

        private int? NewSpecimenPartID()
        {
            int? PartID = null;
            try
            {
                if (this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows.Count > 0)
                {
                    int ID = 0;
                    if (int.TryParse(this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows[0]["CollectionSpecimenPartID"].ToString(), out ID))
                        PartID = ID;
                }
                //if (this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows.Count > 0)
                //{
                //    System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("DisplayOrder > 0", "DisplayOrder ASC");
                //    if (RR.Length > 0)
                //    {
                //        int ID = 0;
                //        if (int.TryParse(RR[0]["IdentificationUnitID"].ToString(), out ID))
                //            PartID = ID;
                //    }
                //}

            }
            catch { }
            return PartID;
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
            //if ((this.dataGridView.SelectedCells.Count > 0 &&
            //    this.dataSetPartGrid.FirstLinesPart.Rows.Count > 0 &&
            //    this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetPartGrid.FirstLinesPart.Rows.Count)
            //    || this.dataSetPartGrid.FirstLinesPart.Rows.Count == 0)
            //{
            //    string TaxonomicGroup = "";
            //    if (this.ProjectSettings.Count > 0)
            //    {
            //        if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
            //            this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
            //        {
            //            TaxonomicGroup = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
            //        }

            //    }
            //    DiversityCollection.Forms.FormIdentificationUnitGridNewEntry f = new DiversityCollection.Forms.FormIdentificationUnitGridNewEntry(this.dataSetCollectionSpecimen, TaxonomicGroup);
            //    f.TopMost = true;
            //    f.ShowDialog();
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        int ID = this._SpecimenID;
            //        if (f.IdentificationUnitCopyMode == DiversityCollection.Forms.FormIdentificationUnitGridNewEntry.UnitCopyMode.NewSpecimen)
            //        {
            //            try
            //            {
            //                if (f.AccessionNumber.Length > 0)
            //                    ID = this.InsertNewSpecimen(f.AccessionNumber);
            //                else
            //                    ID = this.InsertNewSpecimen(Index);
            //                this._IDs.Add(ID);
            //            }
            //            catch (Exception ex)
            //            {
            //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //                return;
            //            }
            //        }
            //        try
            //        {
            //            int UnitID = this.InsertNewUnit(ID);
            //            System.Data.DataTable dt = new DataTable();
            //            string SQL = "SELECT " + this._SqlSpecimenFields +
            //                " FROM dbo.FirstLinesPart_2 ('" + ID.ToString() + "', null, null, null) WHERE IdentificationUnitID = " + UnitID.ToString();
            //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //            ad.Fill(dt);
            //            DiversityCollection.Datasets.DataSetIdentifictionUnitGridMode.FirstLinesPartRow Rnew = this.dataSetPartGrid.FirstLinesPart.NewFirstLinesPartRow();
            //            if (dt.Rows.Count > 0)
            //            {
            //                foreach (System.Data.DataColumn C in Rnew.Table.Columns)
            //                {
            //                    Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
            //                }
            //                this.dataSetPartGrid.FirstLinesPart.Rows.Add(Rnew);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //        }
            //    }
            //}
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

        private int InsertNewPart(int SpecimenID)
        {
            try
            {
                int PartID;
                string MaterialCategory = "";
                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory") &&
                    this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
                    MaterialCategory = this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"];
                else
                    MaterialCategory = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows[0]["MaterialCategory"].ToString();
                int CollectionID;
                if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID") &&
                    this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
                    CollectionID = int.Parse(this.ProjectSettings["CollectionSpecimenPart.CollectionID"]);
                else
                    CollectionID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows[0]["CollectionID"].ToString());
                string SQL = "INSERT INTO CollectionSpecimenPart " +
                    "(CollectionSpecimenID, StorageLocation, MaterialCategory, CollectionID) " +
                    "VALUES (" + SpecimenID.ToString() + ", '" + MaterialCategory
                    + "', '" + MaterialCategory + "'," + CollectionID.ToString() + ") (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                cmd.CommandText = SQL;
                con.Open();
                PartID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return PartID;
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

        private int createSpecimenPart(int Index, string AliasForTable)
        {
            int PartID = -1;
            try
            {
                string SQL = this.GridModeInsertCommandForNewData(Index, AliasForTable);
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                PartID = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
                con.Dispose();
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
            if (this.GridModeTableName(AliasForTable) == "CollectionSpecimenPart")
            {
                string MaterialCategory = "";
                int CollectionID = 0;
                if (!this.dataSetPartGrid.FirstLinesPart.Rows[Index]["CollectionID"].Equals(System.DBNull.Value))
                    CollectionID = int.Parse(this.dataSetPartGrid.FirstLinesPart.Rows[Index]["CollectionID"].ToString());
                string StorageLocation = "";
                SqlColumns += " CollectionSpecimenID, ";
                SqlValues += this.dataSetPartGrid.FirstLinesPart.Rows[Index]["CollectionSpecimenID"].ToString() + ", ";
                MaterialCategory = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Material_category"].ToString();
                if (this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Material_category"].Equals(System.DBNull.Value) ||
                    this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Material_category"].ToString().Length == 0)
                    StorageLocation = MaterialCategory;
                else
                    StorageLocation = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Storage_location"].ToString();
                SqlColumns += " StorageLocation, ";
                SqlValues += " '" + StorageLocation + "', ";
                SqlColumns += " MaterialCategory, ";
                SqlValues += " " + MaterialCategory.ToString() + ", ";
                SqlColumns += " CollectionID, ";
                SqlValues += " " + CollectionID.ToString() + ", ";

                //switch (AliasForTable)
                //{
                //    case "IdentificationUnit":
                //        TaxonomicGroup = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_group"].ToString();
                //        if (this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
                //            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
                //            LastIdentification = TaxonomicGroup;
                //        else
                //            LastIdentification = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name"].ToString();
                //        DisplayOrder = 1;
                //        break;
                //    case "SecondUnit":
                //        TaxonomicGroup = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
                //        if (this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
                //            this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
                //            LastIdentification = TaxonomicGroup;
                //        else
                //            LastIdentification = this.dataSetPartGrid.FirstLinesPart.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
                //        DisplayOrder = 2;
                //        break;
                //}
            }
            if (this.dataSetPartGrid.FirstLinesPart.Rows.Count > Index)
            {
                System.Data.DataRow R = this.dataSetPartGrid.FirstLinesPart.Rows[Index];
                foreach (System.Data.DataColumn C in this.dataSetPartGrid.FirstLinesPart.Columns)
                {
                    DiversityCollection.Forms.GridModeQueryField GMQF = this.GridModeGetQueryField(C.ColumnName);
                    if (GMQF.AliasForTable == AliasForTable &&
                        !this.dataSetPartGrid.FirstLinesPart.Rows[Index][GMQF.AliasForColumn].Equals(System.DBNull.Value))
                    {
                        SqlColumns += GMQF.Column + ", ";
                        SqlValues += "'" + this.dataSetPartGrid.FirstLinesPart.Rows[Index][GMQF.AliasForColumn].ToString() + "', ";
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
                catch { }
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
                    try
                    {
                        string Visibility = DiversityCollection.Forms.FormPartGridSettings.Default.Visibility;
                        DiversityCollection.Forms.FormGridFunctions.GridModeQueryFields(ref this._GridModeQueryFields,
                            this.treeViewGridModeFieldSelector,
                            this.GridModeHiddenQueryFields,
                            ref Visibility);
                        DiversityCollection.Forms.FormPartGridSettings.Default.Visibility = Visibility;
                        DiversityCollection.Forms.FormPartGridSettings.Default.Save();
                    }
                    catch (System.Exception ex) { }
                }
                return this._GridModeQueryFields;
            }
        }

        /// <summary>
        /// All fields that exist in the database view FirstLinesPart
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


                        #region Analysis

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


                        #endregion

                        DiversityCollection.Forms.GridModeQueryField Q_SpecimenPartID = new Forms.GridModeQueryField();
                        Q_SpecimenPartID.Table = "CollectionSpecimenPart";
                        Q_SpecimenPartID.Column = "SpecimenPartID";
                        Q_SpecimenPartID.AliasForColumn = "_SpecimenPartID";
                        Q_SpecimenPartID.AliasForTable = "CollectionSpecimenPart";
                        Q_SpecimenPartID.IsVisible = false;
                        Q_SpecimenPartID.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_SpecimenPartID);

                        #region Processing

                        DiversityCollection.Forms.GridModeQueryField Q_ProcessingDate_1 = new Forms.GridModeQueryField();
                        Q_ProcessingDate_1.Table = "CollectionSpecimenProcessing";
                        Q_ProcessingDate_1.Column = "ProcessingDate";
                        Q_ProcessingDate_1.AliasForColumn = "Processing_date_1";
                        Q_ProcessingDate_1.AliasForTable = "Processing_1";
                        Q_ProcessingDate_1.IsVisible = false;
                        Q_ProcessingDate_1.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_ProcessingDate_1);

                        DiversityCollection.Forms.GridModeQueryField Q_ProcessingDate_2 = new Forms.GridModeQueryField();
                        Q_ProcessingDate_2.Table = "CollectionSpecimenProcessing";
                        Q_ProcessingDate_2.Column = "ProcessingDate";
                        Q_ProcessingDate_2.AliasForColumn = "Processing_date_2";
                        Q_ProcessingDate_2.AliasForTable = "Processing_2";
                        Q_ProcessingDate_2.IsVisible = false;
                        Q_ProcessingDate_2.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_ProcessingDate_2);

                        DiversityCollection.Forms.GridModeQueryField Q_ProcessingDate_3 = new Forms.GridModeQueryField();
                        Q_ProcessingDate_3.Table = "CollectionSpecimenProcessing";
                        Q_ProcessingDate_3.Column = "ProcessingDate";
                        Q_ProcessingDate_3.AliasForColumn = "Processing_date_3";
                        Q_ProcessingDate_3.AliasForTable = "Processing_3";
                        Q_ProcessingDate_3.IsVisible = false;
                        Q_ProcessingDate_3.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_ProcessingDate_3);

                        DiversityCollection.Forms.GridModeQueryField Q_ProcessingDate_4 = new Forms.GridModeQueryField();
                        Q_ProcessingDate_4.Table = "CollectionSpecimenProcessing";
                        Q_ProcessingDate_4.Column = "ProcessingDate";
                        Q_ProcessingDate_4.AliasForColumn = "Processing_date_4";
                        Q_ProcessingDate_4.AliasForTable = "Processing_4";
                        Q_ProcessingDate_4.IsVisible = false;
                        Q_ProcessingDate_4.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_ProcessingDate_4);

                        DiversityCollection.Forms.GridModeQueryField Q_ProcessingDate_5 = new Forms.GridModeQueryField();
                        Q_ProcessingDate_5.Table = "CollectionSpecimenProcessing";
                        Q_ProcessingDate_5.Column = "ProcessingDate";
                        Q_ProcessingDate_5.AliasForColumn = "Processing_date_5";
                        Q_ProcessingDate_5.AliasForTable = "Processing_5";
                        Q_ProcessingDate_5.IsVisible = false;
                        Q_ProcessingDate_5.IsHidden = true;
                        this._GridModeHiddenQueryFields.Add(Q_ProcessingDate_5);

                        #endregion

                        #region Localisation

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
                        
                        #endregion

                        #region Event property

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

                        #endregion

                    }
                    catch { }
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
            catch { }
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
                catch { }
            }
        }

        private GridModeQueryField GridModeQueryFieldOfNode(System.Windows.Forms.TreeNode N)
        {
            DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
            try
            {
                if (N.Tag != null)
                {
                    string[] Parameter = DiversityCollection.Forms.FormGridFunctions.GridModeFieldTagArray(N.Tag.ToString());
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
            catch { }
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

        public int PartID { get { return this._PartID; } }

        public System.Collections.Generic.Dictionary<string, string> ProjectSettings
        {
            get
            {
                if (this._ProjectSettings == null
                    && this._ProjectID != null)
                {
                    this._ProjectSettings = new Dictionary<string, string>();
                    try
                    {
                        string SQL = "use master; SELECT TOP 1 name as DatabaseName FROM sys.databases where name like 'DiversityProject%'";
                        string Database = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (Database.Length == 0)
                            Database = "DiversityProjects";
                        SQL = "SELECT ProjectSetting, Value FROM [" + Database + "].[dbo].[ProjectSettings_Defaults] (" + this._ProjectID.ToString() + ")";
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

            this.setVisibilityTopControls();
        }

        private void buttonHeaderShowTree_Click(object sender, EventArgs e)
        {
            try
            {
                // nicht anwendbar fuer Parts

                //if (this.buttonHeaderShowTree.BackColor == System.Drawing.SystemColors.Control)
                //{
                //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //    this.buttonHeaderShowTree.BackColor = System.Drawing.Color.Red;
                //    this.userControlEventSeriesTree.initControl(this._IDs, "IdentificationUnitList", "SpecimenPartID", this.dataGridView, this.dataSetPartGrid.FirstLinesPart, toolStripButtonSearchSpecimen_Click);
                //    this.Cursor = System.Windows.Forms.Cursors.Default;
                //}
                //else this.buttonHeaderShowTree.BackColor = System.Drawing.SystemColors.Control;

            }
            catch { }
            this.setVisibilityTopControls();
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
            this.setVisibilityTopControls();
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
            this.setVisibilityTopControls();
        }

        private void setVisibilityTopControls()
        {
            try
            {

                if (this.ShowColumnSelectionTree || this.ShowImagesSpecimen)
                {
                    this.splitContainerMain.Panel1Collapsed = false;

                    if (this.ShowImagesSpecimen) this.splitContainerTreeView.Panel2Collapsed = false;
                    else this.splitContainerTreeView.Panel2Collapsed = true;

                    if (this.ShowColumnSelectionTree)
                        this.splitContainerTreeView.Panel1Collapsed = false;
                    else this.splitContainerTreeView.Panel1Collapsed = true;

                    if (this.ShowColumnSelectionTree) this.splitContainerTrees.Panel1Collapsed = false;
                    else this.splitContainerTrees.Panel1Collapsed = true;

                }
                else
                    this.splitContainerMain.Panel1Collapsed = true;


                //if (this.ShowColumnSelectionTree || this.ShowImagesSpecimen || this.ShowDataTree)
                //{
                //    this.splitContainerMain.Panel1Collapsed = false;

                //    if (this.ShowImagesSpecimen) this.splitContainerTreeView.Panel2Collapsed = false;
                //    else this.splitContainerTreeView.Panel2Collapsed = true;

                //    if (this.ShowDataTree || this.ShowColumnSelectionTree)
                //        this.splitContainerTreeView.Panel1Collapsed = false;
                //    else this.splitContainerTreeView.Panel1Collapsed = true;

                //    if (this.ShowColumnSelectionTree) this.splitContainerTrees.Panel1Collapsed = false;
                //    else this.splitContainerTrees.Panel1Collapsed = true;

                //    if (this.ShowDataTree) this.splitContainerTrees.Panel2Collapsed = false;
                //    else this.splitContainerTrees.Panel2Collapsed = true;
                //}
                //else
                //    this.splitContainerMain.Panel1Collapsed = true;

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
                return false;
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
            catch { }
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
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = new List<List<string>>();
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

            //this.comboBoxReplaceWith.Visible = false;
            //this.comboBoxReplace.Visible = false;
            //this.textBoxGridModeReplace.Visible = true;
            //this.textBoxGridModeReplaceWith.Visible = true;
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
                    //if (Combo.DisplayMember == "DisplayText" && Combo.ValueMember == "Code")
                    //{
                    //    this.comboBoxReplace.Width = 100;
                    //    this.comboBoxReplaceWith.Width = 100;
                    //    this.comboBoxReplace.Visible = true;
                    //    this.comboBoxReplaceWith.Visible = true;
                    //    System.Object O = Combo.DataSource;
                    //    System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)O;
                    //    DiversityCollection.Datasets.DataSetPartGrid DS = (DiversityCollection.Datasets.DataSetPartGrid)BS.DataSource;
                    //    System.Data.DataTable dtReplace = DS.Tables[BS.DataMember.ToString()].Copy();
                    //    System.Data.DataTable dtReplaceWith = DS.Tables[BS.DataMember.ToString()].Copy();

                    //    this.comboBoxReplace.DataSource = dtReplace;
                    //    this.comboBoxReplace.DisplayMember = Combo.DisplayMember;
                    //    this.comboBoxReplace.ValueMember = Combo.ValueMember;
                    //    DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplace, AutoCompleteMode.Suggest);

                    //    this.comboBoxReplaceWith.DataSource = dtReplaceWith;
                    //    this.comboBoxReplaceWith.DisplayMember = Combo.DisplayMember;
                    //    this.comboBoxReplaceWith.ValueMember = Combo.ValueMember;
                    //    DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplaceWith, AutoCompleteMode.Suggest);
                    //}
                    //else
                    //{
                        this.comboBoxReplace.Width = 100;
                        this.comboBoxReplaceWith.Width = 100;
                        //this.comboBoxReplace.Visible = true;
                        //this.comboBoxReplaceWith.Visible = true;
                        //this.textBoxGridModeReplace.Visible = false;
                        //this.textBoxGridModeReplaceWith.Visible = false;
                        System.Object O = Combo.DataSource;
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)O;
                        DiversityCollection.Datasets.DataSetPartGrid DS = (DiversityCollection.Datasets.DataSetPartGrid)BS.DataSource;
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
                    //}
                }
            }
            this.setReplaceOptions();
        }

        private void buttonGridModeReplace_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetPartGrid.FirstLinesPart, 0, this.ReplaceOptionStatus);
        }

        private void buttonGridModeInsert_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetPartGrid.FirstLinesPart, 0, this.ReplaceOptionStatus);
        }

        private void buttonGridModeAppend_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetPartGrid.FirstLinesPart, 0, this.ReplaceOptionStatus);
        }

        private void buttonGridModeRemove_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetPartGrid.FirstLinesPart, 0, this.ReplaceOptionStatus);
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
                int PartID;
                if (int.TryParse(this.dataGridView.Rows[RowIndex].Cells[0].Value.ToString(), out PartID))
                {
                    System.Data.DataRow Rori = this.dataSetPartGrid.FirstLinesPart.Rows[this.DatasetIndexOfLine(PartID)];
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
                    //if (Combo.DisplayMember == "DisplayText" && Combo.ValueMember == "Code")
                    //{
                        ShowComboBoxes = true;
                    //}
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

        #region Autocomplete

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
