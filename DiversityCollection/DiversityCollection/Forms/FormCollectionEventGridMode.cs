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
    public partial class FormCollectionEventGridMode : Form
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
        private string _sIDs;

        private int _EventID = 0;
        private int _ProjectID = 0;
        private int _SpecimenID = 0;


        private string _TaxonomicGroup = "plant";
        private DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState _ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace;
        private bool _StopReplacing = false;
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
        private FormCollectionEventGridMode.SaveModes _SaveMode = SaveModes.Single;
        private DiversityCollection.Forms.FormGridFunctions.FormState _FormState = Forms.FormGridFunctions.FormState.Loading;

        #region Data

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimen;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimenImage;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAgent;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProject;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelation;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelationInvers;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterPart;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProcessing;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterTransaction;

        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnit;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnitInPart;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterIdentification;
        //private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAnalysis;

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

        private string _SqlEventFields = "CollectionEventID, " +
            "Data_withholding_reason_for_collection_event, " +
            "Collectors_event_number, Collection_day, Collection_month, Collection_year, Collection_date_supplement, " +
            "Collection_time, Collection_time_span,  " +
            "Country, Locality_description, Habitat_description, Collecting_method, Collection_event_notes, " +
            "Named_area,  NamedAreaLocation2, Remove_link_to_gazetteer, Distance_to_location, Direction_to_location, " +
            "Longitude, Latitude, Coordinates_accuracy, Link_to_GoogleMaps,  " +
            "Altitude_from, Altitude_to, Altitude_accuracy, " +
            "MTB, Quadrant, Notes_for_MTB, " +
            "Sampling_plot, Link_to_SamplingPlots, Remove_link_to_SamplingPlots,  Accuracy_of_sampling_plot, Latitude_of_sampling_plot, Longitude_of_sampling_plot, " +
            "Geographic_region, Lithostratigraphy, Chronostratigraphy,  " +
            "_CollectionEventID,  " +
            "_CoordinatesAverageLatitudeCache, _CoordinatesAverageLongitudeCache, _CoordinatesLocationNotes, " +
            "_GeographicRegionPropertyURI, _LithostratigraphyPropertyURI, _ChronostratigraphyPropertyURI, " +
            "_NamedAverageLatitudeCache, _NamedAverageLongitudeCache, _LithostratigraphyPropertyHierarchyCache, " +
            "_ChronostratigraphyPropertyHierarchyCache, _AverageAltitudeCache ";

        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeries;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesEvent;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesSpecimen;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesUnit;

        //private System.Drawing.Color _ColorOfNotPresentNodes = System.Drawing.Color.LightGray;
        //private System.Drawing.Color _ColorOfNodes = System.Drawing.Color.Black;

        //private System.Data.DataTable _dtGridData;
        //private System.Data.DataTable _dtTreeData;
        //private string _KeyColumn;
        
        #endregion

        #endregion

        #region Construction

        public FormCollectionEventGridMode(System.Collections.Generic.List<int> IDs, string FormTitle, int ProjectID)
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
                this._EventID = IDs[0];
            this.userControlImage.MaxSizeOfImageVisible = true;
            this.GridModeFillTable();
            this.GridModeSetColumnVisibility();
            this.setRemoveCellStyle();
            this.setLinkCellStyle();
            this.setIconsInTreeView();
            this.setTitleInTreeView();
            this.dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this._FormState = FormGridFunctions.FormState.Editing;
            DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms.FormCollectionSpecimenGridModeText));
            DiversityWorkbench.Entity.setEntity(this, resources);
            this.dataGridViewEventSeries.Columns[0].Visible = false;
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
                string TableName = "CollectionEvent";
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
                this.buttonGridModeFind.Visible = false;
                this.userControlEventSeriesTree.toolStripButtonSearchSpecimen.Visible = false;
                this.userControlEventSeriesTree.toolStripButtonShowUnit.Visible = false;
                this.textBoxGridModeReplace.Width = 80;
                this.textBoxGridModeReplaceWith.Width = 80;
                this.comboBoxReplace.Width = 100;
                this.comboBoxReplaceWith.Width = 100;
                this.FillPicklists();
                this.setColumnsAndNodesCorrespondingToPermissions();
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility.Length > 0)
                    this.GridModeSetFieldVisibilityForNodes();
                this.GridModeSetToolTipsInTree();
                this.setImageVisibility(DiversityCollection.Forms.FormCollectionSpecimenGridModeSettings.Default.HeaderDisplay);
                this.userControlImage.ImagePath = "";
                this.setGridColumnHeaders();
                this.enableReplaceButtons(false);
                this.userControlDialogPanel.buttonOK.Click += new EventHandler(CheckForChangesAndAskForSaving);
                foreach (System.Windows.Forms.TreeNode N in this.treeViewGridModeFieldSelector.Nodes)
                    this.SetToolTipsInTreeView(N);
                this.setColumnSequence();
                this.setColumnWidths();
                this.dataGridViewEventSeries.DataSource = this.userControlEventSeriesTree.DtEventSeries;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FillPicklists()
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxImageType, "CollSpecimenImageType_Enum", con, true, true, true);

            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollCircumstances_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollIdentificationCategory_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollIdentificationQualifier_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollMaterialCategory_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollTaxonomicGroup_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollTypeStatus_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollUnitRelationType_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollLabelTranscriptionState_Enum, false);
            //DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetCollectionEventGridMode.CollLabelType_Enum, false);

            //string SQL = "SELECT CollectionID, CollectionName " +
            //    "FROM Collection ORDER BY CollectionName ";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //try
            //{
            //    ad.Fill(this.dataSetCollectionEventGridMode.Collection);
            //}
            //catch { }
            //ad.SelectCommand.CommandText = "";
            //try
            //{
            //    ad.SelectCommand.CommandText = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
            //    ad.Fill(this.dataSetCollectionEventGridMode.ProjectList);
            //}
            //catch { }

            string SQL = "SELECT CollectionID, CollectionName " +
                "FROM Collection ORDER BY CollectionName ";

            System.Data.DataTable dtCountry = DiversityWorkbench.Gazetteer.Countries();
            if (dtCountry.Rows.Count > 0)
            {
                System.Data.DataRow Rnull = this.dataSetCollectionEventGridMode.GeoCountries.NewGeoCountriesRow();
                Rnull["NameEn"] = "";
                this.dataSetCollectionEventGridMode.GeoCountries.Rows.Add(Rnull);
                foreach (System.Data.DataRow RC in dtCountry.Rows)
                {
                    System.Data.DataRow RCountry = this.dataSetCollectionEventGridMode.GeoCountries.NewGeoCountriesRow();
                    RCountry["NameEn"] = RC[0].ToString();
                    this.dataSetCollectionEventGridMode.GeoCountries.Rows.Add(RCountry);
                }
            }
            else
            {

                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.SelectCommand.CommandText = "SELECT NameEn FROM [DiversityGazetteer].[dbo].[GeoCountries] " +
                    "WHERE (PlaceID > 1) " +
                    "UNION " +
                    "SELECT DISTINCT CountryCache AS NameEn FROM CollectionEvent " +
                    "ORDER BY NameEn";
                try
                {
                    ad.Fill(this.dataSetCollectionEventGridMode.GeoCountries);
                }
                catch { }
                if (this.dataSetCollectionEventGridMode.GeoCountries.Rows.Count == 0)
                {
                    ad.SelectCommand.CommandText = "SELECT DISTINCT CountryCache AS NameEn " +
                        "FROM CollectionEvent ORDER BY NameEn";
                    try
                    {
                        ad.Fill(this.dataSetCollectionEventGridMode.GeoCountries);
                    }
                    catch { }
                }
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
                    this.EventID.ToString());
            }
            catch { }
        }

        private void FormCollectionEventGridMode_FormClosing(object sender, FormClosingEventArgs e)
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
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnWidth = ColumnWidths;
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence = ColumnPositions;
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
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms.FormCollectionSpecimenGridModeText));
                Message = resources.GetString(Resource);

            }
            catch { }
            return Message;
        }

        private void FormCollectionEventGridMode_Load(object sender, EventArgs e)
        {
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
                        switch (DiversityWorkbench.Settings.Language)
                        {
                            case "de-DE":
                                TreeNode.Text = "Verbindung zu Transaktion";
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
                // TODO: Funktion muss noch getestet werden
                OK = false;
                this.buttonGridModeCopy.Visible = false;
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
                        //this.treeViewGridModeFieldSelector.Nodes["NodeNamedArea"].Checked = false;
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

        #region Event

        private bool setEvent(int EventID)
        {
            bool OK = true;
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return false;
            try
            {
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    this.updateEvent();
                }
                this.userControlImage.ImagePath = "";
                this.fillEvent(EventID);
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    this.setHeader();
                    this.tableLayoutPanelHeader.Visible = true;
                }
                else
                {
                    if (EventID != 0)
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
        //        OK = this.setEvent(ID);
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
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    string Date = "";
                    string Title = "";
                    if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0 &&
                        !this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["LocalityDescription"].Equals(System.DBNull.Value))
                    {
                        Title = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["LocalityDescription"].ToString();
                        if (Title.Length > 50)
                            Title = Title.Substring(0, 50) + " ...";
                    }
                    if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].Equals(System.DBNull.Value) ||
                       !this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionMonth"].Equals(System.DBNull.Value) ||
                       !this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDay"].Equals(System.DBNull.Value))
                    {
                        if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].Equals(System.DBNull.Value))
                            Date = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].ToString();
                        else Date = "-";
                        if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionMonth"].Equals(System.DBNull.Value))
                            Date += "/" + this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionMonth"].ToString();
                        else Date += "/-";
                        if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDay"].Equals(System.DBNull.Value))
                            Date += "/" + this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDay"].ToString();
                        else Date += "/-";
                    }
                    if (Date.Length > 0) Title = Date + " " + Title;
                    if (Title.Length == 0) Title = "[ID: " + this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionEventID"].ToString() + "]";

                    this.textBoxHeaderTitle.Text = Title;
                }
                else
                {
                    this.textBoxHeaderNumber.Text = "";
                    this.textBoxHeaderTitle.Text = "";
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillEvent(int EventID)
        {
            try
            {
                if (this._EventID != EventID)
                    this.dataSetCollectionSpecimen.Clear();
                else
                {
                    //this.dataSetCollectionSpecimen.CollectionAgent.Clear();
                    this.dataSetCollectionSpecimen.CollectionEvent.Clear();
                    this.dataSetCollectionSpecimen.CollectionEventImage.Clear();
                    this.dataSetCollectionSpecimen.CollectionEventLocalisation.Clear();
                    this.dataSetCollectionSpecimen.CollectionEventProperty.Clear();
                    //this.dataSetCollectionSpecimen.CollectionEventSeries.Clear();
                    //this.dataSetCollectionSpecimen.CollectionProject.Clear();
                    //this.dataSetCollectionSpecimen.CollectionProjectList.Clear();
                    //this.dataSetCollectionSpecimen.CollectionSpecimen.Clear();
                    //this.dataSetCollectionSpecimen.CollectionSpecimenPart.Clear();
                    //this.dataSetCollectionSpecimen.CollectionSpecimenPrinting.Clear();
                    //this.dataSetCollectionSpecimen.CollectionSpecimenProcessing.Clear();
                    //this.dataSetCollectionSpecimen.CollectionSpecimenRelation.Clear();
                    //this.dataSetCollectionSpecimen.CollectionSpecimenRelationInvers.Clear();
                    //this.dataSetCollectionSpecimen.CollectionSpecimenTransaction.Clear();
                    //this.dataSetCollectionSpecimen.Identification.Clear();
                    //this.dataSetCollectionSpecimen.IdentificationUnit.Clear();
                    //this.dataSetCollectionSpecimen.IdentificationUnitAnalysis.Clear();
                    //this.dataSetCollectionSpecimen.IdentificationUnitInPart.Clear();
                    //this.dataSetCollectionSpecimen.IdentificationUnitInPartDisplayList.Clear();
                    //this.dataSetCollectionSpecimen.IdentificationUnitInPartHideList.Clear();
                    //this.dataSetCollectionSpecimen.IdentificationUnitNotInPartList.Clear();
                }

                //string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
                //    " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable)";

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimen);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAgent, DiversityCollection.CollectionSpecimen.SqlAgent + WhereClause + " ORDER BY CollectorsSequence", this.dataSetCollectionSpecimen.CollectionAgent);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProject, DiversityCollection.CollectionSpecimen.SqlProject + WhereClause + " ORDER BY ProjectID", this.dataSetCollectionSpecimen.CollectionProject);
                //if (this._SpecimenID != SpecimenID)
                //    this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this.dataSetCollectionSpecimen.CollectionSpecimenImage);

                //string SQL = "SELECT R.CollectionSpecimenID, R.RelatedSpecimenURI, S.AccessionNumber AS RelatedSpecimenDisplayText, R.RelationType, R.RelatedSpecimenCollectionID, " +
                //    "R.RelatedSpecimenDescription, R.Notes, R.IsInternalRelationCache  " +
                //    "FROM CollectionSpecimenRelation R, CollectionSpecimen S  " +
                //    "WHERE (R.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                //    "AND (S.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                //    "AND (R.IsInternalRelationCache = 1)  " +
                //    "AND rtrim(substring(R.RelatedSpecimenURI, len(dbo.BaseURL()) + 1, 255)) = '" + SpecimenID.ToString() + "'  " +
                //    "AND S.CollectionSpecimenID = R.CollectionSpecimenID " +
                //    "ORDER BY RelatedSpecimenDisplayText ";

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenPart);

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnit, DiversityCollection.CollectionSpecimen.SqlUnit + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnit);

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, DiversityCollection.CollectionSpecimen.SqlIdentification + WhereClause + " ORDER BY IdentificationSequence", this.dataSetCollectionSpecimen.Identification);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAnalysis, DiversityCollection.CollectionSpecimen.SqlAnalysis + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnitAnalysis);

                //if (this._SaveMode == SaveModes.Single)
                //{
                //    this.listBoxImage.Items.Clear();
                //    if (this.ShowImagesSpecimen)
                //    {
                //        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListSpecimenImages,
                //            this.dataSetCollectionSpecimen.CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                //    }

                //    if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count == 0) this.tableLayoutPanelSpecimenImage.Enabled = false;
                //    if (!this.ShowImagesSpecimen && this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > 0)
                //        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.Color.Yellow;
                //    else if (!this.ShowImagesSpecimen && this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count == 0)
                //        this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.SystemColors.Control;
                //}

                // Event
                //if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                //{
                //    if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
                //    {
                        //int EventID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString());
                string WhereClause = " WHERE CollectionEventID = " + EventID.ToString();

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEvent, DiversityCollection.CollectionSpecimen.SqlEvent + WhereClause, this.dataSetCollectionSpecimen.CollectionEvent);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterLocalisation, DiversityCollection.CollectionSpecimen.SqlEventLocalisation + WhereClause, this.dataSetCollectionSpecimen.CollectionEventLocalisation);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProperty, DiversityCollection.CollectionSpecimen.SqlEventProperty + WhereClause, this.dataSetCollectionSpecimen.CollectionEventProperty);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEventImage, DiversityCollection.CollectionSpecimen.SqlEventImage + WhereClause, this.dataSetCollectionSpecimen.CollectionEventImage);
                //    }
                //}

                if (this._SaveMode == SaveModes.Single)
                {
                    this.listBoxImage.Items.Clear();
                    if (this.ShowImages)
                    {
                        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListEventImages,
                            this.dataSetCollectionSpecimen.CollectionEventImage, "URI", this.userControlImage);
                    }

                    if (this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Count == 0) this.tableLayoutPanelSpecimenImage.Enabled = false;
                    if (!this.ShowImages && this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Count > 0)
                        this.buttonHeaderShowImage.BackColor = System.Drawing.Color.Yellow;
                    else if (!this.ShowImages && this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Count == 0)
                        this.buttonHeaderShowImage.BackColor = System.Drawing.SystemColors.Control;
                }

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the collection event", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void updateEvent()
        {
            if (this.sqlDataAdapterEvent != null)
            {
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimen", this.sqlDataAdapterSpecimen, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this.collectionSpecimenImageBindingSource);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionAgent", this.sqlDataAdapterAgent, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionProject", this.sqlDataAdapterProject, this.BindingContext);

                // if datasets of this table were deleted, this must happen before deleting the parent tables
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenPart", this.sqlDataAdapterPart, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnit", this.sqlDataAdapterUnit, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Identification", this.sqlDataAdapterIdentification, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitAnalysis", this.sqlDataAdapterAnalysis, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEvent", this.sqlDataAdapterEvent, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventImage", this.sqlDataAdapterEventImage, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventLocalisation", this.sqlDataAdapterLocalisation, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventProperty", this.sqlDataAdapterProperty, this.BindingContext);

                //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Collection", this.sqlDataAdapterCollection, this.BindingContext);
            }
            this.userControlEventSeriesTree.saveDataEventSeries();
        }

        #endregion

        #region Event images

        private void listBoxImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListEventImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void listBoxImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListEventImages.ImageSize.Height;
            e.ItemWidth = this.imageListEventImages.ImageSize.Width;
        }

        private void listBoxImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxImage.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this.dataSetCollectionSpecimen.CollectionEventImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Count > i)
                {
                    this.tableLayoutPanelSpecimenImage.Enabled = true;
                    System.Data.DataRow r = this.dataSetCollectionSpecimen.CollectionEventImage.Rows[i];
                    this.userControlImage.ImagePath = r["URI"].ToString();
                    this.collectionEventImageBindingSource.Position = i;
                    //this.collectionSpecimenImageBindingSource.Position = i;
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
                        DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventImageRow R = this.dataSetCollectionSpecimen.CollectionEventImage.NewCollectionEventImageRow();
                        R.CollectionEventID = this.EventID;
                        R.URI = f.URIImage;
                        this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Add(R);
                        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListEventImages, this.imageListForm, this.dataSetCollectionSpecimen.CollectionEventImage, "URI", this.userControlImage);
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
                string URL = this.userControlImage.ImagePath;
                if (URL.Length > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventImage.Select("URI = '" + URL + "'");
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            r.Delete();
                            this.FormFunctions.FillImageList(this.listBoxImage, this.imageListEventImages, this.imageListForm, this.dataSetCollectionSpecimen.CollectionEventImage, "URI", this.userControlImage);
                            if (this.listBoxImage.Items.Count > 0) this.listBoxImage.SelectedIndex = 0;
                            else
                            {
                                this.listBoxImage.SelectedIndex = -1;
                                this.userControlImage.ImagePath = "";
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
                string SQL = this.GridModeFillCommand(); ;
                this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Clear();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this.dataSetCollectionEventGridMode.FirstLinesEvent_2);
                this.dataGridView_RowEnter(null, null);
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
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence.Length > 0)
                {
                    string[] Sequence = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence.Split(new char[] { ' ' });
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
                    //if (!Property.EndsWith("_second_organism"))
                    //{
                    //    foreach (GridModeQueryField Q in this.GridModeQueryFields)
                    //    {
                    //        if (Q.AliasForColumn == Property)
                    //        {
                    //            System.Collections.Generic.Dictionary<string, string> EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.Column);
                    //            if (EntityInfo["DisplayTextOK"] == "True")
                    //            {
                    //                Entity = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.Column)["DisplayText"];
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}

                    if (C.HeaderText == "CollectionEventID")
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
                        int CollectionEventID = 0;
                        if (int.TryParse(this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out CollectionEventID))
                        {
                            for (i = 0; i < this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count; i++)
                            {
                                if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i].RowState == DataRowState.Deleted
                                    || this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i].RowState == DataRowState.Detached)
                                    continue;
                                if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i]["CollectionEventID"].ToString() == CollectionEventID.ToString())
                                    break;
                            }
                        }
                    }

                }
                catch { }
                return i;
            }
        }

        private int DatasetIndexOfLine(int EventID)
        {
            int i = 0;
            try
            {
                for (i = 0; i < this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count; i++)
                {
                    if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i]["CollectionEventID"].ToString() == EventID.ToString())
                        break;
                }

            }
            catch { }
            return i;
        }

        private int GridIndexOfDataline(int EventID)
        {
            int i = 0;
            try
            {
                if (this.dataGridView.Rows.Count > 0)
                {
                    for (i = 0; i < this.dataGridView.Rows.Count; i++)
                    {
                        if (this.dataGridView.Rows[i].Cells[0].Value.ToString() == EventID.ToString())
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
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeColumnSequence = "";
            this.setColumnSequence();
        }

        private void buttonGridModeUpdateColumnSettings_Click(object sender, EventArgs e)
        {
            this._GridModeColumnList = null;
            this._GridModeQueryFields = null;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeVisibility = "";
            this.GridModeSetColumnVisibility();
            this.enableReplaceButtons(false);
        }

        #endregion

        #region Button events for Finding, Copy and Saving and related functions

        private void buttonGridModeFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count == 0
                    || this.dataGridView.SelectedCells.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show(this.Message("Nothing_selected"));
                    return;
                }
                if (this.dataSetCollectionEventGridMode.HasChanges())
                {
                    if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        this.SaveAll();
                }
                int ID = 0;
                if (int.TryParse(this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[DatasetIndexOfCurrentLine]["CollectionEventID"].ToString(), out ID))
                {
                    this.DialogResult = DialogResult.OK;
                    this._EventID = ID;
                    this.Close();
                }

            }
            catch { }
        }

        private void buttonGridModeSave_Click(object sender, EventArgs e)
        {
            this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[DatasetIndexOfCurrentLine].BeginEdit();
            this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[DatasetIndexOfCurrentLine].EndEdit();
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
                this.textBoxHeaderNumber.Focus();
                if (this.dataSetCollectionEventGridMode.HasChanges())
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
                this.progressBarSaveAll.Maximum = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count;
                this.progressBarSaveAll.Value = 0;
                for (int i = 0; i < this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count; i++)
                {
                    if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i].RowState == DataRowState.Deleted
                        || this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i].RowState == DataRowState.Detached)
                        continue;
                    this.dataGridView.Rows[i].Cells[0].Selected = true;
                    this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i].BeginEdit();
                    this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[i].EndEdit();
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

                if (System.Windows.Forms.MessageBox.Show("Do you want to create a copy of this dataset?", "Copy?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        int OriginalID = (int)this.EventID;
                        int EventID = this.CopyEvent(OriginalID);
                        System.Data.DataTable dt = new DataTable();
                        string SQL = "SELECT " + this._SqlEventFields + " FROM FirstLinesEvent_2 ('" + EventID.ToString() + "')";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        //for (int i = 1; i < NumberOfCopies; i++)
                        //{
                        DiversityCollection.Datasets.DataSetCollectionEventGridMode.FirstLinesEvent_2Row Rnew = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.NewFirstLinesEvent_2Row();

                            foreach (System.Data.DataColumn C in Rnew.Table.Columns)
                            {
                                Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
                            }
                            this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Add(Rnew);
                            System.Windows.Forms.DataGridViewCell Cell = this.dataGridView.Rows[this.dataGridView.Rows.Count - 2].Cells[0];
                            if (Cell.Visible)
                                this.dataGridView.CurrentCell = Cell;
                        //}
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
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
                    if (int.TryParse(RV["CollectionEventID"].ToString(), out ID))
                    {
                        //string AccessionNumber = RV["Accession_Number"].ToString();
                        if (!DeleteSelection)
                        {
                            string Message = DiversityWorkbench.Entity.Message("Do_you_want_to_delete_the_dataset");
                            //if (AccessionNumber.Length > 0) Message += "\r\n" + DiversityWorkbench.Entity.EntityInformation("CollectionSpecimen.AccessionNumber")["DisplayText"] + ": " + AccessionNumber;
                            //else Message += " ID " + ID.ToString();
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
                        //this.dataSetCollectionEventGridMode.FirstLinesEvent.AcceptChanges();
                    }
                }
                if (DeleteSelection)
                {
                    foreach (int ID in IDsToDelete)
                    {
                        this.deleteEvent(ID);
                        //RV.Delete();
                        //this.dataSetCollectionEventGridMode.FirstLinesEvent.AcceptChanges();
                    }
                    this.GridModeFillTable();
                }
            }
            catch { }
        }

        /// <summary>
        /// delete a event from the database
        /// </summary>
        /// <param name="ID">the Primary key of table CollectionEvent corresponding to the item that should be deleted</param>
        private void deleteEvent(int ID)
        {
            try
            {
                string SQL = "DELETE FROM CollectionEvent WHERE CollectionEventID = " + ID.ToString();
                Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
                com.CommandType = System.Data.CommandType.Text;
                Conn.Open();
                com.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.buttonGridModeDelete.Enabled = false;
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                this.labelGridViewReplaceColumnName.Text = ColumnName;
                if (this.buttonGridModeFind.Enabled == false)
                    this.buttonGridModeFind.Enabled = true;

                if (this.dataGridView.SelectedCells.Count > 0)
                    this.enableReplaceButtons(true);
                else this.enableReplaceButtons(false);

                if (e.ColumnIndex == this.dataGridView.SelectedCells[0].ColumnIndex)
                {
                    switch (ColumnName)
                    {
                        case "Link_to_GoogleMaps":
                            this.GetCoordinatesFromGoogleMaps();
                            break;
                        case "NamedAreaLocation2":
                        case "Link_to_SamplingPlots":
                        case "Geographic_region":
                        case "Lithostratigraphy":
                        case "Chronostratigraphy":
                            this.GetRemoteValues(this.dataGridView.SelectedCells[0]);
                            break;
                        case "Remove_link_to_gazetteer":
                        case "Remove_link_to_SamplingPlots":
                            this.RemoveLink(this.dataGridView.SelectedCells[0]);
                            break;
                    }
                }
                if (this.textBoxHeaderEventID.Text.Length == 0)
                    this.setEvent(this.EventID);

            }
            catch { }
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                this.labelGridViewReplaceColumnName.Text = ColumnName;
                if (this.buttonGridModeFind.Enabled == false)
                    this.buttonGridModeFind.Enabled = true;
                if ((this.dataGridView.SelectedCells.Count > 0 && ColumnName != "CollectionEventID")
                    && (typeof(System.Windows.Forms.DataGridViewTextBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
                    && ColumnName != "CollectionEventID")
                    //|| typeof(System.Windows.Forms.DataGridViewComboBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
                    )
                    this.enableReplaceButtons(true);
                else this.enableReplaceButtons(false);
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
            catch { }
        }

        private void dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView.SelectedCells.Count > 0 &&
                     this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                     this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count > 0)
                this.checkForMissingAndDefaultValues(this.dataGridView.SelectedCells[0], false);
        }

        private void checkForMissingAndDefaultValues(System.Windows.Forms.DataGridViewCell Cell, bool Silent)
        {
            try
            {
                if (this.dataGridView.SelectedCells.Count > 0 &&
                     this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
                     this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count > 0)
                {
                    string ColumnName = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
                    string Message = "";
                    string Value = Cell.EditedFormattedValue.ToString();
                    System.Data.DataRow Rcurr = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Cell.RowIndex];

                    // Checking if a correct value was entered
                    if (!this.ValueIsValid(Cell.ColumnIndex, Value))
                    {
                        System.Windows.Forms.MessageBox.Show(Value + " is not a valid value for " + ColumnName + ".");
                    }

                    switch (ColumnName)
                    {
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
                int EventID = 0;
                if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex < this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count &&
                    int.TryParse(this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[DatasetIndexOfCurrentLine]["CollectionEventID"].ToString(), out EventID))
                {
                    if (EventID != this._EventID)
                    {
                        this.setEvent(EventID);
                        this._EventID = EventID;
                    }
                }
                else if (this.dataGridView.SelectedCells.Count > 0 &&
                    this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count > 0 &&
                    this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count)
                {
                    this.insertNewDataset(this.dataGridView.SelectedCells[0].RowIndex);
                }
                else if (this.dataGridView.SelectedCells.Count == 0
                    && this.dataGridView.Rows.Count > 0
                    && this.dataGridView.Rows[0].Cells[0].Value != null)
                {
                    if (int.TryParse(this.dataGridView.Rows[0].Cells[0].Value.ToString(), out EventID))
                    {
                        this.setEvent(EventID);
                        this._EventID = EventID;
                    }
                }
                else if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count == 0)
                {
                    this.insertNewDataset(0);
                }
                this.setCellBlockings();
                this.setRemoveCellStyle();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            // Sicherung nur bei Schliessen des Formulars oder expliziter Anforderung
            // ansonsten wird bei Laden bereits gesichert
            //if (this.dataGridView.SelectedCells != null && this.dataGridView.SelectedCells.Count > 0)
            //{
            //    this.dataSetCollectionEventGridMode.FirstLinesEvent.Rows[this.dataGridView.SelectedCells[0].RowIndex].BeginEdit();
            //    this.dataSetCollectionEventGridMode.FirstLinesEvent.Rows[this.dataGridView.SelectedCells[0].RowIndex].EndEdit();
            //    this.GridModeUpdate(this.dataGridView.SelectedCells[0].RowIndex);
            //}
        }

        private string GridModeFillCommand()
        {
            string SQL = "SELECT " + this._SqlEventFields + " FROM dbo.FirstLinesEvent_2 ";

            try
            {
                string WhereClause = "";
                foreach (int i in this._IDs)
                {
                    WhereClause += i.ToString() + ", ";
                }
                if (WhereClause.Length == 0) WhereClause = " ('') ";
                else
                    WhereClause = " ('" + WhereClause.Substring(0, WhereClause.Length - 2) + "')";

                SQL += WhereClause;// +" ORDER BY Accession_number ";
            }
            catch { }
            return SQL;
        }

        private void updateEventFromGrid(System.Windows.Forms.DataGridViewRow Row)
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
                if (this.dataSetCollectionEventGridMode.HasChanges())
                {
                    System.Data.DataRow RDataset = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index];
                    if (RDataset.RowState == DataRowState.Modified || RDataset.RowState == DataRowState.Added)
                    {
                        // setting the dataset
                        // the dataset is filled with the original data from the database as a basis for comparision with the data in the grid
                        int CollectionEventID = int.Parse(this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index]["CollectionEventID"].ToString());
                        this._EventID = CollectionEventID;
                        this.fillEvent(this._EventID);

                        // if no data 
                        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count == 0)
                            this.setEvent(this._EventID);

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
                                if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Columns.Contains(KVc.Key))
                                {
                                    ColumnValues[KVc.Value] = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index][KVc.Key].ToString();
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
                                catch { }
                            }
                        }
                        this.updateEvent();
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
                    ColumnValues[KVc.Value] = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index][KVc.Key].ToString();
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
                        if (!this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Columns.Contains(AliasForColumn))
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
                        ColumnValues[KVpk.Key] = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index][AliasForColumn].ToString();
                    }
                }

                // test if all PrimaryKey values are set
                //string TaxonomicGroup = "";
                //int DisplayOrder = 1;
                //string LastIdentification = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KVpk in PKColumns)
                {
                    if (ColumnValues[KVpk.Key].Length == 0)
                    {
                        // the value for a primary key column is missing
                        switch (KVpk.Key)
                        {
                            case "CollectionEventID":
                                // if a previously empty part related to the event is filled
                                string Locality = "";
                                if (!this.dataSetCollectionEventGridMode.FirstLinesEvent_2[Index]["Locality_description"].Equals(System.DBNull.Value))
                                    this.dataSetCollectionEventGridMode.FirstLinesEvent_2[Index]["Locality_description"].ToString();
                                Locality = this.dataSetCollectionEventGridMode.FirstLinesEvent_2[Index]["Locality_description"].ToString();
                                int EventID = this.createEvent();
                                this.dataSetCollectionEventGridMode.FirstLinesEvent_2[Index]["_CollectionEventID"] = EventID;
                                ColumnValues["CollectionEventID"] = EventID.ToString();
                                if (Locality.Length > 0
                                    && ColumnValues.ContainsKey("LocalityDescription"))
                                    ColumnValues["LocalityDescription"] = Locality;
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
                if (Table == "CollectionEvent" &&
                    Rnew["Version"].Equals(System.DBNull.Value))
                    Rnew["Version"] = 1;


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


                this.dataSetCollectionSpecimen.Tables[Table].Rows.Add(Rnew);
                if (PKcontainsIdentity)
                    Rnew.AcceptChanges();
            }
            catch { }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count == 1)
                this.dataGridView_RowEnter(null, null);
            else { }
        }

        #region Remote services

        private void GetCoordinatesFromGoogleMaps()
        {
            string Latitude = "";
            string Longitude = "";
            try
            {
                if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[DatasetIndexOfCurrentLine]["Latitude"].Equals(System.DBNull.Value))
                {
                    Latitude = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[DatasetIndexOfCurrentLine]["Latitude"].ToString();
                    Longitude = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[DatasetIndexOfCurrentLine]["Longitude"].ToString();
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
                            System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesEventBindingSource.Current;
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
                        RvbCountry.BindingSource = this.firstLinesEventBindingSource;
                        RvbCountry.Column = "Country";
                        RvbCountry.RemoteParameter = "Country";
                        RemoteValueBindings.Add(RvbCountry);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLatitude.BindingSource = this.firstLinesEventBindingSource;
                        RvbLatitude.Column = "_NamedAverageLatitudeCache";
                        RvbLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbLongitude.BindingSource = this.firstLinesEventBindingSource;
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
                        RvbSamplingPlotLatitude.BindingSource = this.firstLinesEventBindingSource;
                        RvbSamplingPlotLatitude.Column = "Latitude_of_sampling_plot";
                        RvbSamplingPlotLatitude.RemoteParameter = "Latitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLatitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotLongitude.BindingSource = this.firstLinesEventBindingSource;
                        RvbSamplingPlotLongitude.Column = "Longitude_of_sampling_plot";
                        RvbSamplingPlotLongitude.RemoteParameter = "Longitude";
                        RemoteValueBindings.Add(RvbSamplingPlotLongitude);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotAccuracy = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSamplingPlotAccuracy.BindingSource = this.firstLinesEventBindingSource;
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
                        RvbHierarchyLitho.BindingSource = this.firstLinesEventBindingSource;
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
                        RvbHierarchyChrono.BindingSource = this.firstLinesEventBindingSource;
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
                        RvbFamily.BindingSource = this.firstLinesEventBindingSource;
                        RvbFamily.Column = "Family_of_taxon";
                        RvbFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbOrder.BindingSource = this.firstLinesEventBindingSource;
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
                        RvbSecondFamily.BindingSource = this.firstLinesEventBindingSource;
                        RvbSecondFamily.Column = "_SecondUnitFamilyCache";
                        RvbSecondFamily.RemoteParameter = "Family";
                        RemoteValueBindings.Add(RvbSecondFamily);

                        DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                        RvbSecondOrder.BindingSource = this.firstLinesEventBindingSource;
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

                if (this.firstLinesEventBindingSource != null && IWorkbenchUnit != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesEventBindingSource.Current;
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
                        System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesEventBindingSource.Current;
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

        private System.Collections.Generic.Dictionary<string, string> LinkColumns
        {
            get
            {
                if (this._LinkColumns == null)
                {
                    this._LinkColumns = new Dictionary<string, string>();
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
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

        private void setLinkCellStyle()
        {
            for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
                this.setLinkCellStyle(i);
        }

        private void setLinkCellStyle(int RowIndex)
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
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this.LinkColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell LinkCell in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.dataGridView.Columns[LinkCell.ColumnIndex].DataPropertyName ==
                                this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
                            {
                                LinkCell.Style = this._StyleLink;
                                break;
                            }
                        }
                    }
                }
            }
            catch { }
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

                if (this.firstLinesEventBindingSource != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesEventBindingSource.Current;
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
                    this._RemoveColumns.Add("Remove_link_for_determiner_of_second_organism", "");
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
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this.RemoveColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell RemoveCell in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.dataGridView.Columns[RemoveCell.ColumnIndex].DataPropertyName ==
                                this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
                            {
                                RemoveCell.Style = this._StyleRemove;
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

        //    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesEventBindingSource.Current;
        //    DiversityCollection.DataSetCollectionSpecimen.IdentificationRow R = this.dataSetCollectionSpecimen.Identification.NewIdentificationRow();
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
        //                if (this.firstLinesEventBindingSource != null)
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
        //                    //System.Data.DataRowView RUx = (System.Data.DataRowView)this.firstLinesEventBindingSource.Current;
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

        //private void setNewIdentificationCellStyle()
        //{
        //    for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
        //        this.setNewIdentificationCellStyle(i);
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
            for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
                this.setCellBlockings(i);
        }

        private void setCellBlockings(int RowIndex)
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
                                }
                                else
                                {
                                    CellToBlock.Style = this._StyleUnblocked;
                                    CellToBlock.ReadOnly = false;
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

        #endregion

        #region Copy

        /// <summary>
        /// create a copy of a collection event
        /// </summary>
        /// <param name="OriginalID">The CollectionEventID of the original dataset</param>
        /// <returns></returns>
        private int CopyEvent(int OriginalID)
        {
            string SQL = "execute dbo.procInsertCollectionEventCopy NULL , " + OriginalID.ToString();
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            int ID = 0;
            try
            {
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
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
                this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count > 0 &&
                this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count)
                || this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count == 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to create a new dataset?", "New dataset?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        int ID = this.createEvent();
                        this._IDs.Add(ID);
                        System.Data.DataTable dt = new DataTable();
                        string SQL = "SELECT " + this._SqlEventFields +
                            " FROM dbo.FirstLinesEvent_2 ('" + ID.ToString() + "')";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        DiversityCollection.Datasets.DataSetCollectionEventGridMode.FirstLinesEvent_2Row Rnew = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.NewFirstLinesEvent_2Row();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (System.Data.DataColumn C in Rnew.Table.Columns)
                            {
                                Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
                            }
                            this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Add(Rnew);
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
        /// inserting a new event into the table CollectionEvent
        /// </summary>
        /// <param name="Index">Index of the row in the grid view</param>
        /// <returns>the CollectionEventID of the new specimen</returns>
        private int InsertNewEvent(int Index)
        {
            try
            {
                int ID = -1;
                ID = this.createEvent();
                return ID;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
        }

        private int createEvent()
        {
            int EventID = -1;
            try
            {
                string SQL = this.GridModeInsertCommandForNewData("CollectionEvent");//"INSERT INTO CollectionEvent (LocalityDescription)VALUES ('" + LocalityDescription + "') (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
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
            if (this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count > Index)
            {
                System.Data.DataRow R = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index];
                foreach (System.Data.DataColumn C in this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Columns)
                {
                    DiversityCollection.Forms.GridModeQueryField GMQF = this.GridModeGetQueryField(C.ColumnName);
                    if (GMQF.AliasForTable == AliasForTable &&
                        !this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index][GMQF.AliasForColumn].Equals(System.DBNull.Value))
                    {
                        SqlColumns += GMQF.Column + ", ";
                        SqlValues += "'" + this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[Index][GMQF.AliasForColumn].ToString() + "', ";
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

        private string GridModeInsertCommandForNewData(string AliasForTable)
        {
            string SQL = "INSERT INTO " + AliasForTable + " (Version, LocalityDescription";
            string SqlValues = "VALUES ( 1, '')";
            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (f.Date != null)
                {
                    SQL += ", CollectionDay, CollectionMonth, CollectionYear";
                    SqlValues += ", " + f.Date.Day.ToString() + ", " + f.Date.Month.ToString() + ", " + f.Date.Year.ToString();
                }
            }
            SQL += ") " + SqlValues + ") (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
            return SQL;
        }

        #endregion

        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                if (this.dataGridView.Rows.Count == 0) return;
                foreach (System.Data.DataRow R in this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows)
                {
                    if (R.RowState == DataRowState.Deleted)
                    {
                    }
                }

            }
            catch { }
        }

        #endregion

        #region Handling the visibility of the columns in the grid

        /// <summary>
        /// setting the visibility of the columns in the datagrid based on the definitions in the query fields
        /// </summary>
        private void GridModeSetColumnVisibility()
        {
            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
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
                catch { }
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
                catch { }
                return this._GridModeQueryFields;
            }
        }

        /// <summary>
        /// All fields that exist in the database view FirstLinesEvent
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

        public int EventID { get { return this._EventID; } }

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

        public int SpecimenID { get { return this._SpecimenID; } }

        public int? CollectionSpecimenID
        {
            get
            {
                return this.userControlEventSeriesTree.CollectionSpecimenID;
            }
        }
        
        #endregion

        #region Visibility of images and column selection tree

        private void setImageVisibility(string Visibility)
        {
            try
            {

                if (this.ShowColumnSelectionTree || this.ShowImages || this.ShowDataTree)
                {
                    this.splitContainerMain.Panel1Collapsed = false;

                    if (this.ShowImages) this.splitContainerTreeView.Panel2Collapsed = false;
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


                //if (Visibility.Length != 2) return;

                //if (Visibility.Substring(0, 1) == "1")
                //    this.buttonHeaderShowSelectionTree.BackColor = System.Drawing.Color.Red;
                //else
                //    this.buttonHeaderShowSelectionTree.BackColor = System.Drawing.SystemColors.Control;


                //if (Visibility.Substring(1, 1) == "1")
                //    this.buttonHeaderShowImage.BackColor = System.Drawing.Color.Red;
                //else
                //{
                //    if (this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Count > 0)
                //        this.buttonHeaderShowImage.BackColor = System.Drawing.Color.Yellow;
                //    else
                //        this.buttonHeaderShowImage.BackColor = System.Drawing.SystemColors.Control;
                //}

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
                if (this.buttonHeaderShowImage.BackColor == System.Drawing.Color.Red)
                {
                    if (this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Count > 0)
                        this.buttonHeaderShowImage.BackColor = System.Drawing.Color.Yellow;
                    else
                        this.buttonHeaderShowImage.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.buttonHeaderShowImage.BackColor = System.Drawing.Color.Red;
                    if (this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Count > 0
                        && this.listBoxImage.Items.Count == 0)
                        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListEventImages,
                            this.dataSetCollectionSpecimen.CollectionEventImage, "URI", this.userControlImage);
                }

            }
            catch { }
            this.setVisibility();
        }

        private void setVisibility()
        {
            try
            {

                if (this.ShowColumnSelectionTree || this.ShowImages || this.ShowDataTree)
                {
                    this.splitContainerMain.Panel1Collapsed = false;

                    if (this.ShowImages) this.splitContainerTreeView.Panel2Collapsed = false;
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


                //if (this.ShowColumnSelectionTree || this.ShowImages)
                //{
                //    this.splitContainerMain.Panel1Collapsed = false;
                //    if (this.ShowImages) this.splitContainerTreeView.Panel2Collapsed = false;
                //    else this.splitContainerTreeView.Panel2Collapsed = true;
                //    if (this.ShowColumnSelectionTree) this.splitContainerTreeView.Panel1Collapsed = false;
                //    else this.splitContainerTreeView.Panel1Collapsed = true;
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

        private bool ShowImages
        {
            get
            {
                if (this.buttonHeaderShowImage.BackColor == System.Drawing.Color.Red)
                    return true;
                else
                    return false;
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

        private string CurrentHeaderDisplaySettings
        {
            get
            {
                string Setting = "11";

                try
                {
                    if (this.ShowColumnSelectionTree) Setting = "1";
                    else Setting = "0";
                    if (this.ShowImages) Setting += "1";
                    else Setting += "0";

                }
                catch
                {
                    Setting = "11";
                }
                return Setting;
            }
        }

        private void buttonHeaderShowTree_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonHeaderShowTree.BackColor == System.Drawing.SystemColors.Control)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.buttonHeaderShowTree.BackColor = System.Drawing.Color.Red;
                    this.userControlEventSeriesTree.initControl(this._IDs, "CollectionEvent", "CollectionEventID", this.dataGridView, this.dataSetCollectionEventGridMode.FirstLinesEvent_2, toolStripButtonSearchSpecimen_Click);
                    this.setColumnEventSeriesWidth();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                else this.buttonHeaderShowTree.BackColor = System.Drawing.SystemColors.Control;

            }
            catch { }
            this.setVisibility();
        }

        private void toolStripButtonSearchSpecimen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Copy & Paste from Excel & Co.

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
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = this.ClipBoardValues;
                System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = this.GridColums;
                //string[] stringSeparators = new string[] { "\r\n" };
                //string[] lineSeparators = new string[] { "\t" };
                //string ClipBoardText = System.Windows.Forms.Clipboard.GetText();
                //string[] ClipBoardList = ClipBoardText.Split(stringSeparators, StringSplitOptions.None);
                //for (int i = 0; i < ClipBoardList.Length - 1; i++)
                //{
                //    System.Collections.Generic.List<string> ClipBoardValueStrings = new List<string>();
                //    string[] ClipBoardListStrings = ClipBoardList[i].Split(lineSeparators, StringSplitOptions.None);
                //    for (int ii = 0; ii < ClipBoardListStrings.Length; ii++)
                //        ClipBoardValueStrings.Add(ClipBoardListStrings[ii]);
                //    ClipBoardValues.Add(ClipBoardValueStrings);
                //}

                if (!this.CanCopyClipboardInDataGrid(IndexTopRow, ClipBoardValues, GridColums))
                    return;

                try
                {
                    // transfering the values
                    //System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = new List<DataGridViewColumn>();
                    //int CurrentDisplayIndex = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DisplayIndex;

                    //for (int i = 0; i < this.dataGridView.Columns.Count; i++)
                    //{
                    //    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    //    {
                    //        if (C.Visible && C.DisplayIndex == CurrentDisplayIndex + i)
                    //        {
                    //            GridColums.Add(C);
                    //            break;
                    //        }
                    //    }
                    //    if (GridColums.Count >= ClipBoardValues[0].Count) break;
                    //}


                    for (int ii = 0; ii < GridColums.Count; ii++) // the columns
                    {
                        for (int i = 0; i < ClipBoardValues.Count; i++) // the rows
                        {
                            if (this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].ReadOnly)
                                continue;
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

        private bool CanCopyClipboardInDataGrid(int IndexTopRow, System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues, System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums)
        {
            bool OK = true;
            try
            {
                string Message = this.CheckNumberOfRowsToCopyClipboard(IndexTopRow, ClipBoardValues);
                Message += this.CheckNumberOfColumnsToCopyClipboard(ClipBoardValues, GridColums);
                Message += this.CheckTypeOfColumnsToCopyClipboard(GridColums);
                Message = Message.Trim();
                if (Message.Length > 0)
                {
                    OK = false;
                    System.Windows.Forms.MessageBox.Show(Message);
                }
            }
            catch { OK = false; }
            return OK;
        }

        private string CheckNumberOfRowsToCopyClipboard(int IndexTopRow, System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues)
        {
            string Message = "";
            if (this.dataGridView.Rows.Count <= IndexTopRow + ClipBoardValues.Count)
                Message = "You try to copy " + ClipBoardValues.Count.ToString() + " rows into " + (this.dataGridView.Rows.Count - IndexTopRow - 1).ToString() + " available row(s).\r\n" +
                    "Please reduce you selection.\r\n\r\n";
            return Message;
        }

        private string CheckNumberOfColumnsToCopyClipboard(System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues, System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums)
        {
            string Message = "";
            if (GridColums.Count < ClipBoardValues[0].Count)
                Message = "You try to copy " + ClipBoardValues[0].Count.ToString() + " columns into " + (GridColums.Count).ToString() + " available column(s).\r\n" +
                    "Please reduce your selection.\r\n\r\n";
            return Message;
        }

        private string CheckTypeOfColumnsToCopyClipboard(System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums)
        {
            string Message = "";
            foreach (System.Windows.Forms.DataGridViewColumn GridColum in GridColums)
            {
                if (GridColum.CellType == typeof(System.Windows.Forms.DataGridViewButtonCell)
                    && (GridColum.HeaderText.StartsWith("Link to ")
                    || GridColum.HeaderText.StartsWith("Remove link to ")))
                    Message += GridColum.HeaderText + "\r\n";
            }
            if (Message.Length > 0)
                Message = "The following columns can not be changed via the clipboard:\r\n" + Message + "Please hide the columns from the grid or reduce your selection\r\n\r\n";
            return Message;
        }

        private System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues
        {
            get
            {
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
                return ClipBoardValues;
            }
        }

        private System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums
        {
            get
            {
                System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = new List<DataGridViewColumn>();
                int CurrentDisplayIndex = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DisplayIndex;
                if (this.ClipBoardValues.Count > 0)
                {

                    for (int i = 0; i < this.dataGridView.Columns.Count; i++)
                    {
                        foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                        {
                            if (C.Visible && C.DisplayIndex == CurrentDisplayIndex + i)
                            {
                                GridColums.Add(C);
                                break;
                            }
                        }
                        if (GridColums.Count >= ClipBoardValues[0].Count) break;
                    }
                }
                return GridColums;
            }
        }



        #endregion

        #region Replace, Insert, Append, Clear Function for a single Column

        private void buttonMarkWholeColumn_Click(object sender, EventArgs e)
        {
            try
            {
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                if (ColumnName == "CollectionEventID") return;
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
                        DiversityCollection.Datasets.DataSetCollectionEventGridMode DS = (DiversityCollection.Datasets.DataSetCollectionEventGridMode)BS.DataSource;
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
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewButtonColumn Button
                        = (System.Windows.Forms.DataGridViewButtonColumn)this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                    if (Button.Text == null
                        && Button.DataPropertyName.StartsWith("Link_to_"))// != "X")
                    {
                        this.buttonGridModeReplace.Enabled = false;
                        this.buttonGridModeInsert.Enabled = false;
                        this.buttonGridModeAppend.Enabled = false;

                        this.radioButtonGridModeReplace.Enabled = false;
                        this.radioButtonGridModeInsert.Enabled = false;
                        this.radioButtonGridModeAppend.Enabled = false;

                        this.radioButtonGridModeRemove.Enabled = true;
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
            }
            this.setReplaceOptions();
        }

        private void buttonGridModeReplace_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear();
        }

        private void buttonGridModeInsert_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear();
        }

        private void buttonGridModeAppend_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear();
        }

        private void buttonGridModeRemove_Click(object sender, EventArgs e)
        {
            this.ColumnValues_ReplaceInsertClear();
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
                int EventID;
                if (int.TryParse(this.dataGridView.Rows[RowIndex].Cells[0].Value.ToString(), out EventID))
                {
                    System.Data.DataRow Rori = this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows[this.DatasetIndexOfLine(EventID)];
                    if (Rori.RowState != DataRowState.Unchanged)
                    {
                        Rori.RejectChanges();
                    }
                }
            }
            catch { }
        }

        private void ColumnValues_ReplaceInsertClear()
        {
            if (this.dataGridView.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing has been selected");
                return;
            }
            if (this.dataGridView.SelectedCells.Count > 0)
            {
                this._StopReplacing = false;
                int Index = this.dataGridView.SelectedCells[0].ColumnIndex;
                System.Collections.Generic.List<int> PK = new List<int>();
                if (this.dataGridView.SelectedCells.Count > 1)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                    {
                        int iPK = 0;
                        if (int.TryParse(this.dataGridView.Rows[C.RowIndex].Cells[0].Value.ToString(), out iPK))
                            PK.Add(iPK);
                    }
                }
                else if (this.dataGridView.SelectedCells.Count == 1)
                {
                    if (System.Windows.Forms.MessageBox.Show("Only one field has been selected.\r\n Do you want to apply the changes to the whole column?",
                        "Apply to whole column?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                        {
                            int iPK = 0;
                            if (this.dataGridView.Rows[R.Index].Cells[0].Value != null)
                            {
                                if (int.TryParse(R.Cells[0].Value.ToString(), out iPK))
                                    PK.Add(iPK);
                            }
                        }
                    }
                    else
                    {
                        int iPK = 0;
                        if (int.TryParse(this.dataGridView.SelectedCells[0].Value.ToString(), out iPK))
                            PK.Add(iPK);
                    }
                }

                foreach (int ID in PK)
                {
                    try
                    {
                        System.Windows.Forms.DataGridViewRow Row = this.dataGridView.Rows[0];
                        foreach (System.Windows.Forms.DataGridViewRow RR in this.dataGridView.Rows)
                        {
                            if (RR.Cells[0].Value.ToString() == ID.ToString())
                            {
                                Row = RR;
                                break;
                            }
                        }

                        string ColumnName = this.dataGridView.Columns[Index].DataPropertyName;
                        if (Row.Cells[Index].Style.Tag != null && Row.Cells[Index].Style.Tag.ToString() == "Blocked")
                            continue;
                        if (Row.Index > this.dataSetCollectionEventGridMode.FirstLinesEvent_2.Rows.Count - 1)
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
                        string CorrectedValue = DiversityCollection.Forms.FormGridFunctions.CheckReplaceValue(
                            this.dataGridView,
                            this.dataSetCollectionEventGridMode.FirstLinesEvent_2,
                            Index,
                            Value,
                            OriginalText,
                            NewText,
                            ref IsValid,
                            this._ReplaceOptionState);
                        if (this._StopReplacing) return;
                        if (!IsValid) continue;
                        if (Type == typeof(string))
                        {
                            Row.Cells[Index].Value = CorrectedValue;
                            if (CorrectedValue.Length == 0 && this.dataGridView.Columns[Index].CellType.Name == "DataGridViewComboBoxCell") // && this.comboBoxReplaceWith.Visible && this.comboBoxReplaceWith.SelectedIndex == 0)
                            {
                                Row.Cells[Index].Value = null;
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

        //public static void ColumnValues_ReplaceInsertClear(
        //    System.Windows.Forms.DataGridView dataGridView,
        //    System.Windows.Forms.ComboBox comboBoxReplace,
        //    System.Windows.Forms.ComboBox comboBoxReplaceWith,
        //    System.Windows.Forms.TextBox textBoxGridModeReplace,
        //    System.Windows.Forms.TextBox textBoxGridModeReplaceWith,
        //    System.Data.DataTable dtSource,
        //    int ColumnIndexPK,
        //    ReplaceOptionState ReplaceOptionStatus)
        //{
        //    System.Collections.Generic.List<int> PK = new List<int>();
        //    if (dataGridView.SelectedCells.Count == 0)
        //    {
        //        System.Windows.Forms.MessageBox.Show("Nothing has been selected");
        //        return;
        //    }
        //    if (dataGridView.SelectedCells.Count > 0)
        //    {
        //        DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
        //        DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
        //        int Index = dataGridView.SelectedCells[0].ColumnIndex;
        //        if (dataGridView.SelectedCells.Count > 1)
        //        {
        //            foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
        //            {
        //                int iPK = 0;
        //                if (dataGridView.Rows[C.RowIndex].Cells[0].Value != null)
        //                {
        //                    if (int.TryParse(dataGridView.Rows[C.RowIndex].Cells[0].Value.ToString(), out iPK))
        //                        PK.Add(iPK);
        //                }
        //            }
        //        }
        //        else if (dataGridView.SelectedCells.Count == 1)
        //        {
        //            if (System.Windows.Forms.MessageBox.Show("Only one field has been selected.\r\n Do you want to apply the changes to the whole column?",
        //                "Apply to whole column?",
        //                MessageBoxButtons.YesNo,
        //                MessageBoxIcon.Question) == DialogResult.Yes)
        //            {
        //                foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
        //                {
        //                    int iPK = 0;
        //                    if (dataGridView.Rows[R.Index].Cells[0].Value != null)
        //                    {
        //                        if (int.TryParse(R.Cells[0].Value.ToString(), out iPK))
        //                            PK.Add(iPK);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                int iPK = 0;
        //                if (dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0].Value != null)
        //                {
        //                    if (int.TryParse(dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out iPK))
        //                        PK.Add(iPK);
        //                }
        //            }
        //        }

        //        foreach (int ID in PK)
        //        {
        //            try
        //            {
        //                System.Windows.Forms.DataGridViewRow Row = dataGridView.Rows[0];
        //                foreach (System.Windows.Forms.DataGridViewRow RR in dataGridView.Rows)
        //                {
        //                    if (RR.Cells[0].Value.ToString() == ID.ToString())
        //                    {
        //                        Row = RR;
        //                        break;
        //                    }
        //                }
        //                if (Row.Cells[Index].Style.Tag != null && Row.Cells[Index].Style.Tag.ToString() == "Blocked")
        //                    continue;
        //                if (Row.Index > dtSource.Rows.Count - 1)
        //                    break;
        //                System.Type Type = Row.Cells[Index].ValueType;
        //                string Value = "";
        //                if (Row.Cells[Index].Value != null && !Row.Cells[Index].Value.Equals(System.DBNull.Value))
        //                    Value = Row.Cells[Index].Value.ToString();
        //                string OriginalText = textBoxGridModeReplace.Text;
        //                if (comboBoxReplace.Visible)
        //                {
        //                    if (comboBoxReplace.SelectedIndex > -1)
        //                        OriginalText = comboBoxReplace.SelectedValue.ToString();
        //                    else OriginalText = "";
        //                }
        //                string NewText = textBoxGridModeReplaceWith.Text;
        //                if (comboBoxReplaceWith.Visible)
        //                {
        //                    if (comboBoxReplaceWith.SelectedIndex > -1)
        //                        NewText = comboBoxReplaceWith.SelectedValue.ToString();
        //                    else NewText = "";
        //                }
        //                bool IsValid = true;
        //                string CorrectedValue = DiversityCollection.Forms.FormCollectionSpecimenGridMode.CheckReplaceValue(
        //                    dataGridView,
        //                    dtSource,
        //                    Index,
        //                    Value,
        //                    OriginalText,
        //                    NewText,
        //                    ref IsValid,
        //                    ReplaceOptionStatus);
        //                if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing) return;
        //                if (!IsValid) continue;
        //                if (Type == typeof(string))
        //                {
        //                    Row.Cells[Index].Value = CorrectedValue;
        //                    if (CorrectedValue.Length == 0 && dataGridView.Columns[Index].CellType.Name == "DataGridViewComboBoxCell") // && comboBoxReplaceWith.Visible && comboBoxReplaceWith.SelectedIndex == 0)
        //                    {
        //                        Row.Cells[Index].Value = null;
        //                    }
        //                }
        //                else if (Type == typeof(int)
        //                    || Type == typeof(System.Byte)
        //                    || Type == typeof(System.DateTime)
        //                    || Type == typeof(System.Int16))
        //                {
        //                    if (CorrectedValue.Length == 0)
        //                        Row.Cells[Index].Value = System.DBNull.Value;
        //                    else
        //                    {
        //                        if (Type == typeof(int))
        //                            Row.Cells[Index].Value = int.Parse(CorrectedValue);
        //                        else if (Type == typeof(System.Byte))
        //                            Row.Cells[Index].Value = System.Byte.Parse(CorrectedValue);
        //                        else if (Type == typeof(System.Int16))
        //                            Row.Cells[Index].Value = System.Int16.Parse(CorrectedValue);
        //                        else if (Type == typeof(System.DateTime))
        //                            Row.Cells[Index].Value = System.DateTime.Parse(CorrectedValue);
        //                    }
        //                }
        //            }
        //            catch { }
        //            if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing)
        //                break;
        //        }
        //    }
        //}

        //public static string CheckReplaceValue(
        //    System.Windows.Forms.DataGridView dataGridView,
        //    System.Data.DataTable dtSource,
        //    int ColumnIndex,
        //    string OriginalValue,
        //    string ReplacedValue,
        //    string Replacement,
        //    ref bool IsValid,
        //    ReplaceOptionState ReplaceOptionStatus)
        //{
        //    string TypeOfColumn = dataGridView.Columns[ColumnIndex].ValueType.ToString();
        //    System.Type Type = dataGridView.Columns[ColumnIndex].ValueType;
        //    string ColumnName = dtSource.Columns[ColumnIndex].ColumnName;
        //    string Value = "";
        //    IsValid = true;
        //    try
        //    {
        //        // constructing the new value
        //        if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
        //            == dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].CellType)
        //        {
        //            switch (ReplaceOptionStatus)
        //            {
        //                case ReplaceOptionState.Insert:
        //                    Value = Replacement;
        //                    break;
        //                case ReplaceOptionState.Append:
        //                    Value = Replacement;
        //                    break;
        //                case ReplaceOptionState.Replace:
        //                    if (OriginalValue == ReplacedValue)
        //                        Value = Replacement;
        //                    else Value = OriginalValue;
        //                    break;
        //            }
        //            //Value = Replacement;
        //        }
        //        else
        //        {
        //            switch (ReplaceOptionStatus)
        //            {
        //                case ReplaceOptionState.Insert:
        //                    Value = Replacement + OriginalValue;
        //                    break;
        //                case ReplaceOptionState.Append:
        //                    Value = OriginalValue + Replacement;
        //                    break;
        //                case ReplaceOptionState.Replace:
        //                    if (Type == typeof(int)
        //                    || Type == typeof(System.Byte)
        //                    || Type == typeof(System.DateTime)
        //                    || Type == typeof(System.Int16))
        //                    {
        //                        if (OriginalValue == ReplacedValue)
        //                            Value = Replacement;
        //                        else
        //                            Value = OriginalValue;
        //                    }
        //                    else
        //                    {
        //                        if (Replacement.Length < OriginalValue.Length && ReplacedValue.Length > 0)
        //                            Value = OriginalValue.Replace(ReplacedValue, Replacement);
        //                        else if (OriginalValue.Length == 0 && ReplacedValue.Length == 0)
        //                            Value = Replacement;
        //                        else if (OriginalValue == ReplacedValue && ReplacedValue.Length > 0)
        //                            Value = Replacement;
        //                        else
        //                            IsValid = false;
        //                    }
        //                    break;
        //            }
        //        }

        //        if (!DiversityCollection.Forms.FormCollectionSpecimenGridMode.ValueIsValid(dataGridView, ColumnIndex, Value))
        //        {
        //            IsValid = false;
        //            DiversityCollection.Forms.FormCollectionSpecimenGridMode.AskIfReplacementShouldBeStopped(ColumnName, Value, ReplaceOptionStatus);
        //        }
        //    }
        //    catch { }
        //    return Value;
        //}


        private string CheckReplaceValue(int ColumnIndex, string OriginalValue, string ReplacedValue, string Replacement, ref bool IsValid)
        {
            string TypeOfColumn = this.dataGridView.Columns[ColumnIndex].ValueType.ToString();
            System.Type Type = this.dataGridView.Columns[ColumnIndex].ValueType;
            //string x = this.dataGridView.Columns[ColumnIndex].DataPropertyName;
            string ColumnName = this.dataGridView.Columns[ColumnIndex].DataPropertyName; //this.dataSetCollectionEventGridMode.FirstLinesEvent.Columns[ColumnIndex].ColumnName;
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
                                    Value = OriginalValue.Replace(ReplacedValue, Replacement);
                                else
                                    IsValid = false;
                            }
                            break;
                    }
                }

                if (!this.ValueIsValid(ColumnIndex, Value))
                {
                    IsValid = false;
                    this.AskIfReplacementShouldBeStopped(ColumnName, Value);
                }
            }
            catch { }
            return Value;
        }

        //private string CheckReplaceValue(string ColumnName, string TypeOfColumn, string OriginalValue, string ReplacedValue, string Replacement)
        //{
        //    string Value = "";
        //    System.DateTime Date;
        //    System.Byte Byte;
        //    System.Int16 Int16;
        //    int Int;
        //    try
        //    {
        //        // constructing the new value
        //        switch (this.ReplaceOptionStatus)
        //        {
        //            case ReplaceOptionState.Insert:
        //                Value = Replacement + OriginalValue;
        //                break;
        //            case ReplaceOptionState.Append:
        //                Value = OriginalValue + Replacement;
        //                break;
        //            case ReplaceOptionState.Replace:
        //                Value = OriginalValue.Replace(ReplacedValue, Replacement);
        //                break;
        //        }

        //        // check if the values are valible
        //        switch (TypeOfColumn)
        //        {
        //            case "System.DateTime":
        //                if (!System.DateTime.TryParse(Value, out Date))
        //                    Value = OriginalValue;
        //                break;
        //            case "int":
        //                if (!int.TryParse(Value, out Int))
        //                    Value = OriginalValue;
        //                break;
        //            case "System.Byte":
        //                if (!System.Byte.TryParse(Value, out Byte))
        //                    Value = OriginalValue;
        //                break;
        //            case "System.Int16":
        //                if (!System.Int16.TryParse(Value, out Int16))
        //                    Value = OriginalValue;
        //                break;
        //        }

        //        // check if the values fit the column definition
        //        if (Value.Length > 0)
        //        {
        //            switch (ColumnName)
        //            {
        //                case "Collection_day":
        //                case "Accession_day":
        //                case "Identification_day":
        //                    int Day = int.Parse(Value);
        //                    if (Day < 1 || Day > 31)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid day");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "Collection_month":
        //                case "Accession_month":
        //                case "Identification_month":
        //                    int Month = int.Parse(Value);
        //                    if (Month < 1 || Month > 12)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid month");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "Collection_year":
        //                case "Accession_year":
        //                case "Identification_year":
        //                    int Year = int.Parse(Value);
        //                    if (Year < 1000 || Year > System.DateTime.Now.Year + 1)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid year");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "Preparation_date":
        //                    break;
        //                case "Longitude":
        //                    bool IsValid = true;
        //                    float Lon;
        //                    if (!float.TryParse(Value, out Lon)) IsValid = false;
        //                    if (!IsValid || Lon > 180 || Lon < -180)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid longitude");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "Latitude":
        //                    IsValid = true;
        //                    float Lat;
        //                    if (!float.TryParse(Value, out Lat)) IsValid = false;
        //                    if (!IsValid || Lat > 90 || Lat < -90)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid latitude");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "Altitude_from":
        //                case "Altitude_to":
        //                    IsValid = true;
        //                    float Alt;
        //                    if (!float.TryParse(Value, out Alt)) IsValid = false;
        //                    if (!IsValid || Alt > 9000 || Alt < -11000)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid altitude");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "MTB":
        //                    bool OK = true;
        //                    if (Value.Length != 4) OK = false;
        //                    if (!int.TryParse(Value, out Int)) OK = false;
        //                    else
        //                    {

        //                    }
        //                    if (!OK)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid TK25 resp. MTB");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "Quadrant":
        //                    OK = true;
        //                    if (!int.TryParse(Value, out Int)) OK = false;
        //                    else
        //                    {
        //                        for (int i = 0; i < Value.Length; i++)
        //                        {
        //                            if (Value.Substring(i, 1) != "1" &&
        //                                Value.Substring(i, 1) != "2" &&
        //                                Value.Substring(i, 1) != "3" &&
        //                                Value.Substring(i, 1) != "4")
        //                                OK = false;
        //                        }
        //                    }
        //                    if (!OK)
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        //System.Windows.Forms.MessageBox.Show(Value + " is not a valid quadrant");
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //                case "Number_of_units":
        //                case "Stock":
        //                    if (!int.TryParse(Value, out Int))
        //                    {
        //                        this.AskIfReplacementShouldBeStopped(ColumnName, Value);
        //                        Value = OriginalValue;
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    catch { }            
        //    return Value;
        //}

        private bool ValueIsValid(int ColumnIndex, string Value)
        {
            CultureInfo InvC = new CultureInfo("");

            if (Value.Length == 0) return true;
            System.DateTime Date;
            System.Byte Byte;
            System.Int16 Int16;
            string TypeOfColumn = this.dataGridView.Columns[ColumnIndex].ValueType.ToString();
            string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
            //string ColumnName = this.dataSetCollectionEventGridMode.FirstLinesEvent.Columns[ColumnIndex].ColumnName;
            int Int;
            bool ValidValue = true;
            try
            {
                // check if the values are valible
                switch (TypeOfColumn)
                {
                    case "System.DateTime":
                        if (!System.DateTime.TryParse(Value, out Date))
                        {
                            if (!System.DateTime.TryParse(Value, InvC, DateTimeStyles.None, out Date))
                                ValidValue = false;
                        }
                        break;
                    case "int":
                        if (!int.TryParse(Value, out Int))
                            ValidValue = false;

                        break;
                    case "System.Byte":
                        if (!System.Byte.TryParse(Value, out Byte))
                            ValidValue = false;
                        break;
                    case "System.Int16":
                        if (!System.Int16.TryParse(Value, out Int16))
                            ValidValue = false;
                        break;
                }

                // check if the values fit the column definition
                if (Value.Length > 0 && ValidValue)
                {
                    switch (ColumnName)
                    {
                        case "Collection_day":
                        case "Accession_day":
                        case "Identification_day":
                            int Day = int.Parse(Value);
                            if (Day < 1 || Day > 31)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_month":
                        case "Accession_month":
                        case "Identification_month":
                            int Month = int.Parse(Value);
                            if (Month < 1 || Month > 12)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_year":
                        case "Accession_year":
                        case "Identification_year":
                            int Year = int.Parse(Value);
                            if (Year < 1000 || Year > System.DateTime.Now.Year + 1)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Preparation_date":
                            break;
                        case "Longitude":
                            bool IsValid = true;
                            float Lon;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Lon)) IsValid = false;
                            if (!IsValid || Lon > 180 || Lon < -180)
                            {
                                int x = (int)Lon;
                                float f = Lon * 2;
                                ValidValue = false;
                            }
                            break;
                        case "Latitude":
                            IsValid = true;
                            float Lat;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Lat)) IsValid = false;
                            if (!IsValid || Lat > 90 || Lat < -90)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Altitude_from":
                        case "Altitude_to":
                            IsValid = true;
                            float Alt;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Alt)) IsValid = false;
                            if (!IsValid || Alt > 9000 || Alt < -11000)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "MTB":
                            bool OK = true;
                            if (Value.Length != 4) OK = false;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Quadrant":
                            OK = true;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            else
                            {
                                for (int i = 0; i < Value.Length; i++)
                                {
                                    if (Value.Substring(i, 1) != "1" &&
                                        Value.Substring(i, 1) != "2" &&
                                        Value.Substring(i, 1) != "3" &&
                                        Value.Substring(i, 1) != "4")
                                        OK = false;
                                }
                            }
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Number_of_units":
                        case "Stock":
                            if (!int.TryParse(Value, out Int))
                            {
                                ValidValue = false;
                            }
                            break;
                    }
                }
            }
            catch { }
            return ValidValue;
        }

        public static bool ValueIsValid(
            System.Windows.Forms.DataGridView dataGridView,
            int ColumnIndex,
            string Value)
        {
            CultureInfo InvC = new CultureInfo("");

            if (Value.Length == 0) return true;
            System.DateTime Date;
            System.Byte Byte;
            System.Int16 Int16;
            string TypeOfColumn = dataGridView.Columns[ColumnIndex].ValueType.ToString();
            string ColumnName = dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
            //string ColumnName = this.dataSetCollectionEventGridMode.FirstLinesEvent.Columns[ColumnIndex].ColumnName;
            int Int;
            bool ValidValue = true;
            try
            {
                // check if the values are valible
                switch (TypeOfColumn)
                {
                    case "System.DateTime":
                        if (!System.DateTime.TryParse(Value, out Date))
                        {
                            if (!System.DateTime.TryParse(Value, InvC, DateTimeStyles.None, out Date))
                                ValidValue = false;
                        }
                        break;
                    case "int":
                        if (!int.TryParse(Value, out Int))
                            ValidValue = false;

                        break;
                    case "System.Byte":
                        if (!System.Byte.TryParse(Value, out Byte))
                            ValidValue = false;
                        break;
                    case "System.Int16":
                        if (!System.Int16.TryParse(Value, out Int16))
                            ValidValue = false;
                        break;
                }

                // check if the values fit the column definition
                if (Value.Length > 0 && ValidValue)
                {
                    switch (ColumnName)
                    {
                        case "Collection_day":
                        case "Accession_day":
                        case "Identification_day":
                            int Day = int.Parse(Value);
                            if (Day < 1 || Day > 31)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_month":
                        case "Accession_month":
                        case "Identification_month":
                            int Month = int.Parse(Value);
                            if (Month < 1 || Month > 12)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_year":
                        case "Accession_year":
                        case "Identification_year":
                            int Year = int.Parse(Value);
                            if (Year < 1000 || Year > System.DateTime.Now.Year + 1)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Preparation_date":
                            break;
                        case "Longitude":
                            bool IsValid = true;
                            float Lon;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Lon)) IsValid = false;
                            if (!IsValid || Lon > 180 || Lon < -180)
                            {
                                int x = (int)Lon;
                                float f = Lon * 2;
                                ValidValue = false;
                            }
                            break;
                        case "Latitude":
                            IsValid = true;
                            float Lat;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Lat)) IsValid = false;
                            if (!IsValid || Lat > 90 || Lat < -90)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Altitude_from":
                        case "Altitude_to":
                            IsValid = true;
                            float Alt;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Alt)) IsValid = false;
                            if (!IsValid || Alt > 9000 || Alt < -11000)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "MTB":
                            bool OK = true;
                            if (Value.Length != 4) OK = false;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Quadrant":
                            OK = true;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            else
                            {
                                for (int i = 0; i < Value.Length; i++)
                                {
                                    if (Value.Substring(i, 1) != "1" &&
                                        Value.Substring(i, 1) != "2" &&
                                        Value.Substring(i, 1) != "3" &&
                                        Value.Substring(i, 1) != "4")
                                        OK = false;
                                }
                            }
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Number_of_units":
                        case "Stock":
                            if (!int.TryParse(Value, out Int))
                            {
                                ValidValue = false;
                            }
                            break;
                    }
                }
            }
            catch { }
            return ValidValue;
        }

        private void AskIfReplacementShouldBeStopped(string ColumnName, string Value)
        {
            string Message;
            if (Value.Length > 0)
                Message = Value + " is not a valid value for " + ColumnName.Replace("_", " ") + "\r\n\r\nDo you want to " + this.ReplaceOptionStatus.ToString() + " further values?";
            else
                Message = "You insert an empty value for " + ColumnName.Replace("_", " ") + "\r\n\r\nDo you want to " + this.ReplaceOptionStatus.ToString() + " further values?";
            if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                this._StopReplacing = true;
            else this._StopReplacing = false;
        }

        public static void AskIfReplacementShouldBeStopped(
            string ColumnName,
            string Value,
            DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState ReplaceOptionStatus)
        {
            string Message;
            if (Value.Length > 0)
                Message = Value + " is not a valid value for " + ColumnName.Replace("_", " ") + "\r\n\r\nDo you want to " + ReplaceOptionStatus.ToString() + " further values?";
            else
                Message = "You insert an empty value for " + ColumnName.Replace("_", " ") + "\r\n\r\nDo you want to " + ReplaceOptionStatus.ToString() + " further values?";
            if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = true;
            else DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
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

        #region Event Series

        private void setColumnEventSeriesWidth()
        {
            this.dataGridViewEventSeries.Columns[0].Visible = false;
            int i = this.dataGridViewEventSeries.Columns.Count;
            int W = this.dataGridViewEventSeries.Width - this.dataGridViewEventSeries.RowHeadersWidth - this.dataGridViewEventSeries.Columns[i - 1].Width - this.dataGridViewEventSeries.Columns[i - 2].Width - this.dataGridViewEventSeries.Columns[i - 4].Width;
            this.dataGridViewEventSeries.Columns[i - 3].Width = (W / 2) + 20;
            this.dataGridViewEventSeries.Columns[i - 5].Width = (W / 2);
        }


        private void dataGridViewEventSeries_SizeChanged(object sender, EventArgs e)
        {
            this.setColumnEventSeriesWidth();
        }

        private void dataGridViewEventSeries_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewEventSeries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string ColumnName = this.dataGridViewEventSeries.Columns[this.dataGridViewEventSeries.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                if (e.ColumnIndex == this.dataGridViewEventSeries.SelectedCells[0].ColumnIndex)
                {
                    switch (ColumnName)
                    {
                        case "DateStart":
                        case "DateEnd":
                            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false);
                            f.ShowDialog();
                            if (f.DialogResult == DialogResult.OK)
                                this.dataGridViewEventSeries.SelectedCells[0].Value = f.Date;
                            break;
                    }
                }
            }
            catch { }
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
