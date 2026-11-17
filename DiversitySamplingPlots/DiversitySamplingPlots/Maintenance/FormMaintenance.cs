using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversitySamplingPlots.Maintenance
{
    public partial class FormMaintenance : Form, DiversityWorkbench.Maintenance.iMaintenanceForm
    {
        #region Construction
        public FormMaintenance()
        {
            InitializeComponent();

            // Manual
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form_KeyDown);

            this.initForm();
            this.setDatabases();
            try
            {
                //if (DiversitySamplingPlots.Maintenance.FormMaintenanceSettings.Default.Project.Length > 0)
                //{
                //    this.comboBoxSynColTaxProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynColRefProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynColGazProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynColExsProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynColPlotProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynColTaxFamOrdProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynFamOrdProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynColTaxTextProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxAccNrDuplicatesProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxBulkProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSetPlaceProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    //this.comboBoxUnrelatedEventSeriesProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxAccNrDuplicatesProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxSynColAgeSplitCollectorsProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxGazetteerForProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxNagoyaProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxAnalysisProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxCheckRetrievalProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //    this.comboBoxGeographyInAreaProject.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Project;
                //}

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.InitUnrelatedEvents();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }

        #endregion

        #region Interface

        public int FormWidth() { return this.Width; }
        public int FormHeight() { return this.Height; }

        #endregion

        #region Parameter

        private System.Data.DataTable _dtProjects;
        private System.Data.DataTable _dtProjectsWithNull;

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                DiversitySamplingPlots.Forms.FormDiversitySamplingPlots f = new Forms.FormDiversitySamplingPlots();
                DiversityWorkbench.Maintenance.Synchronize.SynchronizeTerm synchronizeTermLinked = new DiversityWorkbench.Maintenance.Synchronize.SynchronizeTerm(true,
                    DiversitySamplingPlots.Properties.Resources.Habitat,
                    "Site property",
                    "Property",
                    f,
                    "PlotID",
                    "SamplingPlot",
                    "PlotIdentifier",
                    "SamplingPlotProperty",
                    "DisplayText",
                    "PropertyID",
                    "PropertyHierarchyCache",
                    "PropertyURI",
                    "SamplingProject");

                this.tabPageSynchroTermsUri.Controls.Clear();
                DiversityWorkbench.Maintenance.Synchronize.UserControlSynchronizeTerm userControlSynchronizeTermLinked = new DiversityWorkbench.Maintenance.Synchronize.UserControlSynchronizeTerm(synchronizeTermLinked, this);
                synchronizeTermLinked.setUserControl(userControlSynchronizeTermLinked);
                this.tabPageSynchroTermsUri.Controls.Add(userControlSynchronizeTermLinked);
                userControlSynchronizeTermLinked.Dock = DockStyle.Fill;


                DiversityWorkbench.Maintenance.Synchronize.SynchronizeTerm synchronizeTermText = new DiversityWorkbench.Maintenance.Synchronize.SynchronizeTerm(false,
                    DiversitySamplingPlots.Properties.Resources.Habitat,
                    "Site property",
                    "Property",
                    f,
                    "PlotID",
                    "SamplingPlot",
                    "PlotIdentifier",
                    "SamplingPlotProperty",
                    "DisplayText",
                    "PropertyID",
                    "PropertyHierarchyCache",
                    "PropertyURI",
                    "SamplingProject");

                this.tabPageSynchroTermsText.Controls.Clear();
                DiversityWorkbench.Maintenance.Synchronize.UserControlSynchronizeTerm userControlSynchronizeTermText = new DiversityWorkbench.Maintenance.Synchronize.UserControlSynchronizeTerm(synchronizeTermText, this);
                synchronizeTermText.setUserControl(userControlSynchronizeTermText);
                this.tabPageSynchroTermsText.Controls.Add(userControlSynchronizeTermText);
                userControlSynchronizeTermText.Dock = DockStyle.Fill;

                //this.tabControlCoordinateCalculation.TabPages.Remove(this.tabPageCoordinateParser);
                //this.tabControlPlaceAndCountry.TabPages.Remove(this.tabPagePlaceAndCountryGazetteer);

                //this.comboBoxSynColGazTarget.Items.Add(_SynColGazType.Place.ToString());
                //this.comboBoxSynColGazTarget.Items.Add(_SynColGazType.Coordinates.ToString());

                //this.radioButtonSynColGazCountryToPlace.Checked = !DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry;
                //this.radioButtonSynColGazPlaceToCountry.Checked = DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry;
                //this.radioButtonGazetteerCountryToPlace.Checked = !DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry;
                //this.radioButtonGazetteerPlaceToCountry.Checked = DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry;


                //this.comboBoxGazetteerLocalisationSystem.DataSource = DiversityCollection.LookupTable.DtLocationNameAeras();
                //this.comboBoxGazetteerLocalisationSystem.DisplayMember = "DisplayText";
                //this.comboBoxGazetteerLocalisationSystem.ValueMember = "LocalisationSystemID";

                //this.comboBoxSynColGazTarget.SelectedIndex = 0;

                //DiversityWorkbench.Agent Agent = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                //try
                //{
                //    System.Data.DataTable dtTypes = Agent.AgentNameDisplayTypes(true);
                //    if (dtTypes.Columns.Count > 1)
                //    {
                //        this.comboBoxSynColAgeDisplaytype.DataSource = dtTypes;
                //        this.comboBoxSynColAgeDisplaytype.DisplayMember = dtTypes.Columns[1].ColumnName;
                //        this.comboBoxSynColAgeDisplaytype.ValueMember = dtTypes.Columns[0].ColumnName;

                //        this.comboBoxSynColAgeTextDisplaytype.DataSource = dtTypes;
                //        this.comboBoxSynColAgeTextDisplaytype.DisplayMember = dtTypes.Columns[1].ColumnName;
                //        this.comboBoxSynColAgeTextDisplaytype.ValueMember = dtTypes.Columns[0].ColumnName;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //}

                //try
                //{
                //    System.Uri U = new Uri("http://www.geonames.org/img/globe.gif");
                //    this.userControlWebViewGeoNames.Url = null;
                //    this.userControlWebViewGeoNames.Navigate(U);
                //    this.labelSetPlaceAndCountry.Text = DiversityCollection.Forms.FormMaintenanceText.SetPlaceAndCountryInfo;
                //}
                //catch (System.Exception ex) { }

                this.initOrphanedData();

                this.initSamplingPlots();

                //this.initAcceptedNames();
                //this.initAnalysis();


                //initSynchronization_ScientificTerms();

                //this.initSynColAgeText();

                //this.initSplitCollectors();

                //this.initGeographyInArea();

                // wird nicht mehr angezeigt, da eingegebene Daten inkonsistent sind
                //this.tabControlSynColTaxFamOrd.TabPages.Remove(this.tabPageAvailableData);
                //this.tabControlSynColGaz.TabPages.Remove(this.tabPageSynColGazTK25);
                //this.buttonUnrelatedEventSeriesList.Height = (int)(this.buttonUnrelatedEventSeriesList.Height * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                //this.buttonUnrelatedEventSeriesDelete.Height = (int)(this.buttonUnrelatedEventSeriesDelete.Height * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

                // Event & Series
                //this.initEvent();
                //this.initEventSeries();

                //this.initCheckGeography();
                //this.initTaxonBrokenLinks();

                //this.toolTip.SetToolTip(this.buttonSetTimeout, "Set the timeout for maintenance queries: " + DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout.ToString() + " sec.");
                //this.buttonSetTimeout.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout.ToString() + " s";
                //if (DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout == 0)
                //    this.buttonSetTimeout.Text = "inf.";

                //this.initNagoya();

                //this.initTK25forCoordinates();

                //this.initImageExtenionCheck();
                //this.initRetrievalType();

                //this.initReferences();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initOrphanedData()
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    //// Series
                    //if (this.tabControlMain.TabPages.Contains(this.tabPageUnrelatedEventSeries))
                    //    this.tabControlMain.TabPages.Remove(this.tabPageUnrelatedEventSeries);

                    //bool OK = this.FormFunctions.getObjectPermissions("CollectionEventSeries", "DELETE");
                    //if (!OK && this.tabControlOrphanedData.TabPages.Contains(this.tabPageOrphanedSeries))
                    //    this.tabControlOrphanedData.TabPages.Remove(this.tabPageOrphanedSeries);

                    //// Events
                    //OK = this.FormFunctions.getObjectPermissions("CollectionEvent", "DELETE");
                    //if (!OK && this.tabControlOrphanedData.TabPages.Contains(this.tabPageOrphanedEvent))
                    //    this.tabControlOrphanedData.TabPages.Remove(this.tabPageOrphanedEvent);

                    //// Annotations
                    //if (this.tabControlMain.TabPages.Contains(this.tabPageAnnotations))
                    //    this.tabControlMain.TabPages.Remove(this.tabPageAnnotations);

                    //OK = this.FormFunctions.getObjectPermissions("Annotation", "DELETE");
                    //if (!OK && this.tabControlOrphanedData.TabPages.Contains(this.tabPageOrphanedAnnotations))
                    //    this.tabControlOrphanedData.TabPages.Remove(this.tabPageOrphanedAnnotations);

                    //// Identifier
                    //if (this.tabControlMain.TabPages.Contains(this.tabPageIdentifier))
                    //    this.tabControlMain.TabPages.Remove(this.tabPageIdentifier);

                    //OK = this.FormFunctions.getObjectPermissions("ExternalIdentifier", "DELETE");
                    //if (!OK && this.tabControlOrphanedData.TabPages.Contains(this.tabPageOrphanedIdentifier))
                    //    this.tabControlOrphanedData.TabPages.Remove(this.tabPageOrphanedIdentifier);
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initSamplingPlots()
        {
            if (DiversityWorkbench.Settings.ConnectionString != "")
            {
                try
                {
                    //this.comboBoxSynColPlotDatabase.Items.Clear();
                    //this.comboBoxSynColPlotDatabase.Items.Add("");
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversitySamplingPlots"].ServerConnectionList())
                    //{
                    //    this.comboBoxSynColPlotDatabase.Items.Add(KV.Key);
                    //}
                    //if (comboBoxSynColPlotDatabase.Items.Count > 0)
                    //    comboBoxSynColPlotDatabase.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void FormMaintenance_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.comboBoxSynColTaxProject.Text.Length > 0)
            //    DiversityCollection.Forms.FormMaintenanceSettings.Default.Project = this.comboBoxSynColTaxProject.Text;
            //else
            //{
            //    if (this.comboBoxSynColExsProject.Text.Length > 0)
            //        DiversityCollection.Forms.FormMaintenanceSettings.Default.Project = this.comboBoxSynColExsProject.Text;
            //    else
            //    {
            //    }
            //}
            //if (this.comboBoxSynColTaxDatabase.Text.Length > 0)
            //    DiversityCollection.Forms.FormMaintenanceSettings.Default.SynColTaxDatabase = this.comboBoxSynColTaxDatabase.Text;
            //DiversityCollection.Forms.FormMaintenanceSettings.Default.Save();
            //if (this._ConnectionForSamplingPlotSynchronisation != null)
            //{
            //    if (this._ConnectionForSamplingPlotSynchronisation.State == ConnectionState.Open)
            //        this._ConnectionForSamplingPlotSynchronisation.Close();
            //    this._ConnectionForSamplingPlotSynchronisation.Dispose();
            //}
            //if (_sqlConnectionSynColAgeText != null)
            //{
            //    if (_sqlConnectionSynColAgeText.State == ConnectionState.Open)
            //        _sqlConnectionSynColAgeText.Close();
            //    _sqlConnectionSynColAgeText.Dispose();
            //}
            //if (_sqlConnectionCollectorsSequence != null)
            //{
            //    if (_sqlConnectionCollectorsSequence.State == ConnectionState.Open)
            //    {
            //        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(CollectorsSequenceRemoveTempTable());
            //        _sqlConnectionCollectorsSequence.Close();
            //    }
            //    _sqlConnectionCollectorsSequence.Dispose();
            //}
        }

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void InitUnrelatedEvents()
        {
            try
            {
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                //{
                //    bool OK = this.FormFunctions.getObjectPermissions("CollectionEvent", "DELETE");
                //    if (!OK && this.tabControlCollectionEvent.TabPages.Contains(this.tabPageRemoveEvents))
                //        this.tabControlCollectionEvent.TabPages.Remove(this.tabPageRemoveEvents);

                //    OK = this.FormFunctions.getObjectPermissions("CollectionEventSeries", "DELETE");
                //    if (!OK && this.tabControlMain.TabPages.Contains(this.tabPageUnrelatedEventSeries))
                //        this.tabControlMain.TabPages.Remove(this.tabPageUnrelatedEventSeries);
                //}

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void buttonSetTimeout_Click(object sender, EventArgs e)
        {
            //int Timeout = DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout;// /1000;
            //DiversityWorkbench.FormGetInteger f = new DiversityWorkbench.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for the maintenance queries in seconds");
            //f.ShowDialog();
            //if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            //{
            //    DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout = (int)f.Integer;// *1000;
            //    DiversityCollection.Forms.FormMaintenanceSettings.Default.Save();
            //    this.toolTip.SetToolTip(this.buttonSetTimeout, "Set the timeout for maintenance queries: " + DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout.ToString() + " sec.");
            //    this.buttonSetTimeout.Text = DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout.ToString() + " s";
            //    if (DiversityCollection.Forms.FormMaintenanceSettings.Default.Timeout == 0)
            //        this.buttonSetTimeout.Text = "inf.";
            //}
        }

        #endregion

        #region Database

        private void setDatabases()
        {
            if (DiversityWorkbench.Settings.ConnectionString != "")
            {
                try
                {
                    // Taxonomy
                    //this.comboBoxSynColTaxDatabase.Items.Clear();
                    //this.comboBoxSynColTaxFamOrdDatabase.Items.Clear();
                    //this.comboBoxSynColTaxTextDatabase.Items.Clear();
                    //this.comboBoxAcceptedNameTaxonomySource.Items.Clear();
                    //this.comboBoxRemoveNameURITaxonDB.Items.Clear();
                    //foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].DatabaseServices())
                    //{
                    //    this.comboBoxSynColTaxDatabase.Items.Add(DS.DisplayText);
                    //    this.comboBoxSynColTaxFamOrdDatabase.Items.Add(DS.DisplayText);
                    //    this.comboBoxSynColTaxTextDatabase.Items.Add(DS.DisplayText);
                    //    this.comboBoxAcceptedNameTaxonomySource.Items.Add(DS.DisplayText);
                    //    if (!DS.IsWebservice && !DS.IsListInDatabase && !DS.IsCacheDB && !DS.IsForeignSource)
                    //        this.comboBoxRemoveNameURITaxonDB.Items.Add(DS.DisplayText);
                    //}

                    // Agents
                    //this.comboBoxSynColAgeDatabase.Items.Clear();
                    //this.comboBoxSynColAgeTextDatabase.Items.Clear();
                    //foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].DatabaseServices())
                    //{
                    //    this.comboBoxSynColAgeDatabase.Items.Add(DS.DisplayText);
                    //    this.comboBoxSynColAgeTextDatabase.Items.Add(DS.DisplayText);
                    //}
                    //if (this.comboBoxSynColAgeTextDatabase.Items.Count > 0)
                    //    this.comboBoxSynColAgeTextDatabase.SelectedIndex = 0;
                    //if (this.comboBoxSynColAgeDatabase.Items.Count > 0)
                    //    this.comboBoxSynColAgeDatabase.SelectedIndex = 0;

                    // Exsiccatae
                    //this.comboBoxSynColExsDB.Items.Clear();
                    //if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityExsiccatae"))
                    //{
                    //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityExsiccatae"].ServerConnections())
                    //    {
                    //        this.comboBoxSynColExsDB.Items.Add(KV.Value.DisplayText);
                    //    }
                    //    if (this.comboBoxSynColExsDB.Items.Count > 0)
                    //        this.comboBoxSynColExsDB.SelectedIndex = 0;
                    //}

                    // References
                    //this.comboBoxSynColRefDatabase.Items.Clear();
                    //foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].DatabaseServices())
                    //{
                    //    if (!DS.IsWebservice)
                    //        this.comboBoxSynColRefDatabase.Items.Add(DS.DisplayText);
                    //}
                    //if (this.comboBoxSynColRefDatabase.Items.Count > 0)
                    //    this.comboBoxSynColRefDatabase.SelectedIndex = 0;

                    // TK25
                    //this.comboBoxTK25Source.Items.Clear();
                    //foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._TK25Module].DatabaseServices())
                    //{
                    //    this.comboBoxTK25Source.Items.Add(DS.DisplayText);
                    //}
                    //if (comboBoxTK25Source.Items.Count > 0)
                    //    comboBoxTK25Source.SelectedIndex = 0;

                    //// Gazetteer
                    //this.comboBoxSynColGazDatabase.Items.Clear();
                    //this.comboBoxSynColGazTK25Database.Items.Clear();
                    //this.comboBoxGazetteerDatabase.Items.Clear();
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList())
                    //{
                    //    this.comboBoxSynColGazDatabase.Items.Add(KV.Value.DisplayText);
                    //    this.comboBoxSynColGazTK25Database.Items.Add(KV.Value.DisplayText);
                    //    this.comboBoxGazetteerDatabase.Items.Add(KV.Value.DisplayText);
                    //}

                    // Terms
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList())
                    {
                        this.comboBoxSynColTermDatabase.Items.Add(KVconn.Key);
                        this.comboBoxSynTermsTextDatabase.Items.Add(KVconn.Key);

                        //this.comboBoxSynUnitTermsDatabase.Items.Add(KVconn.Key);
                        //this.comboBoxSynUnitTermsTextDatabase.Items.Add(KVconn.Key);

                        //this.comboBoxPartDescDatabase.Items.Add(KVconn.Key);
                        //this.comboBoxSynPartDescriptionTextDatabase.Items.Add(KVconn.Key);
                    }

                    //Check
                    //this.comboBoxGeographyInAreaSource.Items.Clear();
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList())
                    //{
                    //    this.comboBoxGeographyInAreaSource.Items.Add(KV.Value.DisplayText);
                    //}
                    //if (comboBoxGeographyInAreaSource.Items.Count > 0)
                    //    comboBoxGeographyInAreaSource.SelectedIndex = 0;


                    this.setLookupTables();
                    //this.setModuleConnectionsForBulkControls();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.FormFunctions.setTitle();
        }

        private void setLookupTables()
        {
            try
            {
                if (this._dtProjects == null) this._dtProjects = new DataTable();
                else this._dtProjects.Clear();
                string SQL = "SELECT [ProjectID], [Project] " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectList] ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._dtProjects);

                if (this._dtProjectsWithNull == null) this._dtProjectsWithNull = new DataTable();
                else this._dtProjectsWithNull.Clear();
                ad.SelectCommand.CommandText = "SELECT NULL AS [ProjectID] , NULL AS [Project] " +
                    "UNION " +
                    "SELECT [ProjectID], [Project] " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectList] ORDER BY Project";
                ad.Fill(this._dtProjectsWithNull);

                //this.comboBoxSynColTaxProject.DataSource = this._dtProjects;
                //this.comboBoxSynColTaxProject.DisplayMember = "Project";
                //this.comboBoxSynColTaxProject.ValueMember = "ProjectID";

                //ad.SelectCommand.CommandText = "SELECT TerminologyID AS ProjectID, DisplayText AS Project " +
                //    "FROM Terminology ORDER BY Project";
                //System.Data.DataTable _dtTerminology = new DataTable();
                //ad.Fill(_dtTerminology);
                //this.comboBoxSynUnitTermsTermProject.DataSource = _dtTerminology;
                //this.comboBoxSynUnitTermsTermProject.DisplayMember = "Project";
                //this.comboBoxSynUnitTermsTermProject.ValueMember = "ProjectID";

                //this.comboBoxSynUnitTermsTextTermProject.DataSource = _dtTerminology;
                //this.comboBoxSynUnitTermsTextTermProject.DisplayMember = "Project";
                //this.comboBoxSynUnitTermsTextTermProject.ValueMember = "ProjectID";

                //this.comboBoxSynUnitTermsProject.DataSource = this._dtProjects;
                //this.comboBoxSynUnitTermsProject.DisplayMember = "Project";
                //this.comboBoxSynUnitTermsProject.ValueMember = "ProjectID";

                //this.comboBoxSynUnitTermsTextProject.DataSource = this._dtProjects;
                //this.comboBoxSynUnitTermsTextProject.DisplayMember = "Project";
                //this.comboBoxSynUnitTermsTextProject.ValueMember = "ProjectID";

                //this.comboBoxPartDescProject.DataSource = this._dtProjects;
                //this.comboBoxPartDescProject.DisplayMember = "Project";
                //this.comboBoxPartDescProject.ValueMember = "ProjectID";

                //this.comboBoxSynColExsProject.DataSource = this._dtProjects;
                //this.comboBoxSynColExsProject.DisplayMember = "Project";
                //this.comboBoxSynColExsProject.ValueMember = "ProjectID";

                //this.comboBoxSynColPlotProject.DataSource = this._dtProjects;
                //this.comboBoxSynColPlotProject.DisplayMember = "Project";
                //this.comboBoxSynColPlotProject.ValueMember = "ProjectID";

                //this.comboBoxSynColGazProject.DataSource = this._dtProjectsWithNull;
                //this.comboBoxSynColGazProject.DisplayMember = "Project";
                //this.comboBoxSynColGazProject.ValueMember = "ProjectID";
                ////this.comboBoxSynColGazProject.Sorted = true;

                //this.comboBoxSynColGazTK25Project.DataSource = this._dtProjectsWithNull;
                //this.comboBoxSynColGazTK25Project.DisplayMember = "Project";
                //this.comboBoxSynColGazTK25Project.ValueMember = "ProjectID";

                //this.comboBoxSynColRefProject.DataSource = this._dtProjects;
                //this.comboBoxSynColRefProject.DisplayMember = "Project";
                //this.comboBoxSynColRefProject.ValueMember = "ProjectID";

                //this.comboBoxSynColAgeProject.DataSource = this._dtProjects;
                //this.comboBoxSynColAgeProject.DisplayMember = "Project";
                //this.comboBoxSynColAgeProject.ValueMember = "ProjectID";

                //this.comboBoxSynColAgeTextProject.DataSource = this._dtProjects;
                //this.comboBoxSynColAgeTextProject.DisplayMember = "Project";
                //this.comboBoxSynColAgeTextProject.ValueMember = "ProjectID";

                //this.comboBoxSynFamOrdProject.DataSource = this._dtProjects;
                //this.comboBoxSynFamOrdProject.DisplayMember = "Project";
                //this.comboBoxSynFamOrdProject.ValueMember = "ProjectID";

                //this.comboBoxSynColTaxFamOrdProject.DataSource = this._dtProjects;
                //this.comboBoxSynColTaxFamOrdProject.DisplayMember = "Project";
                //this.comboBoxSynColTaxFamOrdProject.ValueMember = "ProjectID";

                //this.comboBoxSynColTaxTextProject.DataSource = this._dtProjects;
                //this.comboBoxSynColTaxTextProject.DisplayMember = "Project";
                //this.comboBoxSynColTaxTextProject.ValueMember = "ProjectID";

                this.comboBoxSynColTermProject.DataSource = this._dtProjects;
                this.comboBoxSynColTermProject.DisplayMember = "Project";
                this.comboBoxSynColTermProject.ValueMember = "ProjectID";

                this.comboBoxSynTermsTextProject.DataSource = this._dtProjects;
                this.comboBoxSynTermsTextProject.DisplayMember = "Project";
                this.comboBoxSynTermsTextProject.ValueMember = "ProjectID";

                //this.comboBoxBulkProject.DataSource = this._dtProjects;
                //this.comboBoxBulkProject.DisplayMember = "Project";
                //this.comboBoxBulkProject.ValueMember = "ProjectID";

                //this.comboBoxSetPlaceProject.DataSource = this._dtProjects;
                //this.comboBoxSetPlaceProject.DisplayMember = "Project";
                //this.comboBoxSetPlaceProject.ValueMember = "ProjectID";

                //this.comboBoxMergeEventsProject.DataSource = this._dtProjectsWithNull;
                //this.comboBoxMergeEventsProject.DisplayMember = "Project";
                //this.comboBoxMergeEventsProject.ValueMember = "ProjectID";

                //this.comboBoxAddCoordinatesProject.DataSource = this._dtProjects;
                //this.comboBoxAddCoordinatesProject.DisplayMember = "Project";
                //this.comboBoxAddCoordinatesProject.ValueMember = "ProjectID";

                //this.comboBoxCalculateCoordinatesProject.DataSource = this._dtProjects;
                //this.comboBoxCalculateCoordinatesProject.DisplayMember = "Project";
                //this.comboBoxCalculateCoordinatesProject.ValueMember = "ProjectID";

                //this.comboBoxParseCoordinatesProject.DataSource = this._dtProjects;
                //this.comboBoxParseCoordinatesProject.DisplayMember = "Project";
                //this.comboBoxParseCoordinatesProject.ValueMember = "ProjectID";

                ////this.comboBoxUnrelatedEventSeriesProject.DataSource = this._dtCollectionProjects;
                ////this.comboBoxUnrelatedEventSeriesProject.DisplayMember = "Project";
                ////this.comboBoxUnrelatedEventSeriesProject.ValueMember = "ProjectID";

                //this.comboBoxPrintProject.DataSource = this._dtProjects;
                //this.comboBoxPrintProject.DisplayMember = "Project";
                //this.comboBoxPrintProject.ValueMember = "ProjectID";

                //this.comboBoxSynColAgeSplitCollectorsProject.DataSource = this._dtProjects;
                //this.comboBoxSynColAgeSplitCollectorsProject.DisplayMember = "Project";
                //this.comboBoxSynColAgeSplitCollectorsProject.ValueMember = "ProjectID";

                //SQL = "select USER_NAME()";
                //string User = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToLower();
                //if (User == "dbo")
                //    this.comboBoxCheckGeographyProject.DataSource = this._dtProjectsWithNull;
                //else
                //    this.comboBoxCheckGeographyProject.DataSource = this._dtProjects;
                //this.comboBoxCheckGeographyProject.DisplayMember = "Project";
                //this.comboBoxCheckGeographyProject.ValueMember = "ProjectID";

                //this.comboBoxGazetteerForProject.DataSource = this._dtProjects;
                //this.comboBoxGazetteerForProject.DisplayMember = "Project";
                //this.comboBoxGazetteerForProject.ValueMember = "ProjectID";

                //this.comboBoxImagesProject.DataSource = this._dtProjects;
                //this.comboBoxImagesProject.DisplayMember = "Project";
                //this.comboBoxImagesProject.ValueMember = "ProjectID";

                //this.comboBoxSetStorageLocationProject.DataSource = this._dtProjects;
                //this.comboBoxSetStorageLocationProject.DisplayMember = "Project";
                //this.comboBoxSetStorageLocationProject.ValueMember = "ProjectID";

                //this.comboBoxSynColTaxBrokenProject.DataSource = this._dtProjects;
                //this.comboBoxSynColTaxBrokenProject.DisplayMember = "Project";
                //this.comboBoxSynColTaxBrokenProject.ValueMember = "ProjectID";

                //this.comboBoxEventMethodParametersProject.DataSource = this._dtProjectsWithNull;
                //this.comboBoxEventMethodParametersProject.DisplayMember = "Project";
                //this.comboBoxEventMethodParametersProject.ValueMember = "ProjectID";

                //this.comboBoxAcceptedNameProject.DataSource = this._dtProjects;
                //this.comboBoxAcceptedNameProject.DisplayMember = "Project";
                //this.comboBoxAcceptedNameProject.ValueMember = "ProjectID";

                //this.comboBoxNagoyaProject.DataSource = this._dtProjects;
                //this.comboBoxNagoyaProject.DisplayMember = "Project";
                //this.comboBoxNagoyaProject.ValueMember = "ProjectID";

                //this.comboBoxTK25forCoordinatesProject.DataSource = this._dtProjects;
                //this.comboBoxTK25forCoordinatesProject.DisplayMember = "Project";
                //this.comboBoxTK25forCoordinatesProject.ValueMember = "ProjectID";

                //this.comboBoxAnalysisProject.DataSource = this._dtProjects;
                //this.comboBoxAnalysisProject.DisplayMember = "Project";
                //this.comboBoxAnalysisProject.ValueMember = "ProjectID";

                //this.comboBoxCheckRetrievalProject.DataSource = this._dtProjects;
                //this.comboBoxCheckRetrievalProject.DisplayMember = "Project";
                //this.comboBoxCheckRetrievalProject.ValueMember = "ProjectID";

                //this.comboBoxGeographyInAreaProject.DataSource = this._dtProjects;
                //this.comboBoxGeographyInAreaProject.DisplayMember = "Project";
                //this.comboBoxGeographyInAreaProject.ValueMember = "ProjectID";

                //this.comboBoxOutsideTK25Project.DataSource = this._dtProjects;
                //this.comboBoxOutsideTK25Project.DisplayMember = "Project";
                //this.comboBoxOutsideTK25Project.ValueMember = "ProjectID";

                //this.comboBoxPlaceAndCountryGazetteerProject.DataSource = this._dtProjects;
                //this.comboBoxPlaceAndCountryGazetteerProject.DisplayMember = "Project";
                //this.comboBoxPlaceAndCountryGazetteerProject.ValueMember = "ProjectID";

                //this.comboBoxRemoveNameURIProject.DataSource = this._dtProjects;
                //this.comboBoxRemoveNameURIProject.DisplayMember = "Project";
                //this.comboBoxRemoveNameURIProject.ValueMember = "ProjectID";

                System.Data.DataTable _dtProject = new DataTable();
                SQL = "SELECT NULL AS ProjectID , NULL AS Project " +
                    "UNION " +
                    "SELECT ProjectID, Project " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectList] ORDER BY Project";
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(_dtProject);
                //this.comboBoxAccNrDuplicatesProject.DataSource = _dtProject;
                //this.comboBoxAccNrDuplicatesProject.DisplayMember = "Project";
                //this.comboBoxAccNrDuplicatesProject.ValueMember = "ProjectID";

                SQL = "SELECT ProjectID, Project " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectList] ORDER BY Project";
                ad.SelectCommand.CommandText = SQL;
                //this._dtSamplingPlotToLocalityProjects = new DataTable();
                //ad.Fill(this._dtSamplingPlotToLocalityProjects);
                //this.comboBoxSamplingPlotToLocalityProject.DataSource = _dtSamplingPlotToLocalityProjects;
                //this.comboBoxSamplingPlotToLocalityProject.DisplayMember = "Project";
                //this.comboBoxSamplingPlotToLocalityProject.ValueMember = "ProjectID";

                SQL = "SELECT ProjectID, Project " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectList] ORDER BY Project";
                ad.SelectCommand.CommandText = SQL;
                //this._dtTK25Projects = new DataTable();
                //ad.Fill(this._dtTK25Projects);
                //this.comboBoxTK25Project.DataSource = _dtTK25Projects;
                //this.comboBoxTK25Project.DisplayMember = "Project";
                //this.comboBoxTK25Project.ValueMember = "ProjectID";

                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                //string Restriction = "";
                //foreach (string C in DiversityWorkbench.CollectionSpecimen.TermRelatedTaxonomicGroups)
                //{
                //    if (Restriction.Length > 0)
                //        Restriction += ", ";
                //    Restriction += "'" + C + "'";
                //}
                //Restriction = "Code not in (" + Restriction + ")";
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynColTaxFamOrdTaxonomicGroup, "CollTaxonomicGroup_Enum", con, false, true, false, Restriction);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynColTaxTextTaxonomicGroup, "CollTaxonomicGroup_Enum", con, false, true, false, Restriction);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynColTaxTaxonomicGroup, "CollTaxonomicGroup_Enum", con, false, true, false, Restriction);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynFamOrdTaxonomicGroup, "CollTaxonomicGroup_Enum", con);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynColTaxBrokenTaxonomicGroup, "CollTaxonomicGroup_Enum", con, true, true, false, Restriction);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAcceptedNameTaxonomicGroup, "CollTaxonomicGroup_Enum", con, true, true, false, Restriction);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxAnalysisTaxonomicGroup, "CollTaxonomicGroup_Enum", con, false, true, false, Restriction);

                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxBulkTaxonomicGroup, "CollTaxonomicGroup_Enum", con, true, true);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxBulkIdentCategory, "CollIdentificationCategory_Enum", con);

                //Restriction = Restriction.Replace(" not ", " ");
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynUnitTermsTaxonomicGroup, "CollTaxonomicGroup_Enum", con, false, true, false, Restriction);
                //DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxSynUnitTermsTextTaxonomicGroup, "CollTaxonomicGroup_Enum", con, false, true, false, Restriction);


                //this.comboBoxAddCoordinatesSource.Items.Add(LookupTable.CoordinateSystem.GaussKrüger);
                //this.comboBoxAddCoordinatesSource.Items.Add(LookupTable.CoordinateSystem.PotsdamDatum);
                //this.comboBoxAddCoordinatesSource.Items.Add(LookupTable.CoordinateSystem.UTM);
                //this.comboBoxAddCoordinatesSource.Items.Add(LookupTable.CoordinateSystem.WGS84);

                //this.comboBoxCalculateCoordinatesType.Items.Add(LookupTable.CoordinateSystem.WGS84);

                //foreach (string P in this.CoordiateParser)
                //{
                //    this.comboBoxParseCoordinatesType.Items.Add(P);
                //}
                //this.comboBoxParseCoordinatesType.SelectedIndex = 0;

                //System.Data.DataTable dtMethod = new DataTable();
                //SQL = "SELECT NULL AS MethodID , NULL AS Method " +
                //    "UNION " +
                //    "SELECT MethodID, DisplayText AS Method " +
                //    "FROM Method ORDER BY Method";
                //ad.SelectCommand.CommandText = SQL;
                //ad.Fill(dtMethod);
                //this.comboBoxEventMethodParametersMethod.DataSource = dtMethod;
                //this.comboBoxEventMethodParametersMethod.DisplayMember = "Method";
                //this.comboBoxEventMethodParametersMethod.ValueMember = "MethodID";
                //// fixing unclear visibilty changes - TODO

                //this.comboBoxEventMethodParametersMethod.Visible = true;
                //this.labelEventMethodParametersMethod.Visible = true;
                //this.buttonEventMethodParametersCheckDataset.Visible = false;

            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Site properties

        private System.Data.DataTable _dtSynColTerm;
        private int _PlotID;


        public enum SynTermTargetTable { SamplingPlotProperty, CollectionSpecimenPartDescription, Identification, NULL }


        #region Obsolete Common



        private void InitSynTermsHideForRelease()
        {
        }

        private void SynTermSetCellStyle(System.Windows.Forms.DataGridView D, int From, int? To = null)
        {
            try
            {
                System.Windows.Forms.DataGridViewCellStyle S = new DataGridViewCellStyle(D.DefaultCellStyle);
                S.ForeColor = System.Drawing.Color.Blue;
                int Max = D.ColumnCount;
                if (To != null)
                    Max = (int)To;
                for (int i = From; i < Max; i++)
                {
                    D.Columns[i].DefaultCellStyle = S;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Linked

        //private void InitSynchronizeTerm_EventProperty_Linked()
        //{
        //    tabPageSynchroTermsUri.Controls.Clear();
        //    DiversitySamplingPlots.Maintenance.SynchronizeTerm synchronizeTerm = new DiversitySamplingPlots.Maintenance.SynchronizeTerm(DiversitySamplingPlots.Maintenance.SynchronizeTerm.TargetTable.SamplingPlotProperty, true);
        //    DiversitySamplingPlots.Maintenance.UserControlSynchronizeTerm userControlSynchronizeTerm = new DiversitySamplingPlots.Maintenance.UserControlSynchronizeTerm(synchronizeTerm);
        //    userControlSynchronizeTerm.Dock = DockStyle.Fill;
        //    tabPageSynchroTermsUri.Controls.Add(userControlSynchronizeTerm);
        //}

        #region Obsolete

        //private DiversitySamplingPlots.Maintenance.SynchronizeTerm _SynchronizeTerm_EventProperty_Linked;

        //private DiversitySamplingPlots.Maintenance.SynchronizeTerm SynchronizeTerm_EventProperty_Linked
        //{
        //    get
        //    {
        //        if (_SynchronizeTerm_EventProperty_Linked == null)
        //        {
        //            _SynchronizeTerm_EventProperty_Linked = new DiversitySamplingPlots.Maintenance.SynchronizeTerm(DiversitySamplingPlots.Maintenance.SynchronizeTerm.TargetTable.SamplingPlotProperty, true,
        //                this.dataGridViewSynColTerm, this._dtSynColTerm, this.labelSynColTermResult,
        //                this.buttonSynColTermUpdate);
        //        }
        //        return _SynchronizeTerm_EventProperty_Linked;
        //    }
        //}



        private DiversityWorkbench.ServerConnection _SynTermsLinkServerConnection;

        private SynTermTargetTable SynTermLinkTargetSelectedTable
        {
            get
            {

                SynTermTargetTable T = SynTermTargetTable.NULL;
                return T;
            }
        }

        private bool SynTermsLinkPrechecksPassed(ref int ProjectID)
        {
            bool OK = true;
            try
            {
                if (this.comboBoxSynColTermDatabase.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a database");
                    return false;
                }
                if (this.comboBoxSynColTermProject.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a project");
                    return false;
                }

                if (this.SynTermLinkTargetSelectedTable == SynTermTargetTable.NULL)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the table that should be checked");
                    return false;
                }

                if (this.comboBoxSynColTermProject.SelectedItem != null)
                {
                    System.Data.DataRowView RP = (System.Data.DataRowView)this.comboBoxSynColTermProject.SelectedItem;
                    if (!int.TryParse(RP["ProjectID"].ToString(), out ProjectID))
                        return false;
                }
                else return false;


            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void comboBoxSynColTermDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelSynColTermDatabase.Text = "      Database:";
            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList()[this.comboBoxSynColTermDatabase.SelectedItem.ToString()];
            this._SynTermsLinkServerConnection = SC;
            if (SC.LinkedServer.Length > 0)
                this.labelSynColTermDatabase.Text = "      " + SC.DatabaseName;
        }

        private void buttonSynColTermCheckDataset_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewSynColTerm.SelectedRows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a dataset");
                return;
            }
            System.Windows.Forms.DataGridViewRow R = this.dataGridViewSynColTerm.SelectedRows[0];
            bool OK = true;
            try { string test = R.Cells["CollectionSpecimenID"].Value.ToString(); }
            catch { OK = false; }
            if (OK)
            {
                if (int.TryParse(R.Cells["CollectionSpecimenID"].Value.ToString(), out this._PlotID))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Dataset in database can only be checked if you include the accession number before checking for differences");
            }
        }

        private void buttonSynColTermSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                bool setColumnWidth = true;



                int ProjectID = 0;
                if (!SynTermsLinkPrechecksPassed(ref ProjectID))
                    return;

                string SQL = "SELECT DISTINCT ";
                if (this.dataGridViewSynColTerm.ColumnCount > 1) setColumnWidth = false;
                switch (this.SynTermLinkTargetSelectedTable)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += " T.PropertyURI AS URL, cast(1 as bit) as OK, T.DisplayText AS [Property in Collection] ";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += " T.DescriptionTermURI AS URL, cast(1 as bit) as OK, T.Description AS [Property in Collection] ";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += " T.TermURI AS URL, cast(1 as bit) as OK, T.VernacularTerm AS [Property in Collection] ";
                        break;
                }

                switch ("")//this.comboBoxSynColTermTarget.Text)
                {
                    case "Term":
                        SQL += ", '' AS [Property in ScientificTerms] ";
                        break;
                    case "Hierarchy":
                        switch (this.SynTermLinkTargetSelectedTable)
                        {
                            case SynTermTargetTable.SamplingPlotProperty:
                                SQL += ", T.PropertyHierarchyCache ";
                                break;
                            case SynTermTargetTable.CollectionSpecimenPartDescription:
                                SQL += ", T.DescriptionHierarchyCache ";
                                break;
                            case SynTermTargetTable.Identification:
                                SQL += ", U.HierarchyCache ";
                                break;
                        }
                        SQL += " AS [Hierarchy in Collection], '' AS [Hierarchy in ScientificTerms]";
                        break;
                }
                if (this.checkBoxSynLinkedTermIncludePlot.Checked)
                    SQL += ", S.AccessionNumber, S.CollectionSpecimenID ";



                SQL += SynTermsLinkFromClause();
                SQL += SynTermsLinkGroupAndOrderClause();


                this._dtSynColTerm = new DataTable();
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversitySamplingPlots.Maintenance.FormMaintenanceSettings.Default.Timeout));
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = DiversitySamplingPlots.Maintenance.FormMaintenanceSettings.Default.Timeout;
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(C);
                this._dtSynColTerm.Clear();
                ad.Fill(this._dtSynColTerm);

                this.progressBarSynColTerm.Value = 0;
                this.progressBarSynColTerm.Maximum = this._dtSynColTerm.Rows.Count;
                Microsoft.Data.SqlClient.SqlConnection conTerm = new Microsoft.Data.SqlClient.SqlConnection(this._SynTermsLinkServerConnection.ConnectionString);
                conTerm.Open();
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand("", conTerm);
                System.Collections.Generic.List<System.Data.DataRow> RowsToRemove = new List<DataRow>();
                string Prefix = this._SynTermsLinkServerConnection.Prefix();// SC.DatabaseName + ".dbo.";
                foreach (System.Data.DataRow R in this._dtSynColTerm.Rows)
                {
                    string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(R["URL"].ToString());
                    int i;
                    if (!int.TryParse(ID, out i))
                    {
                        RowsToRemove.Add(R);
                        continue;
                    }
                    SQL = "SELECT ";
                    switch ("")//this.comboBoxSynColTermTarget.Text)
                    {
                        case "Term":
                            SQL += " DisplayText ";
                            break;
                        case "Hierarchy":
                            SQL += " HierarchyCacheDown ";
                            break;
                    }

                    SQL += " FROM " + Prefix + "TermRepresentation AS R " +
                        "WHERE RepresentationID = " + ID;
                    Com.CommandText = SQL;
                    string Result = Com.ExecuteScalar().ToString();

                    switch ("")//this.comboBoxSynColTermTarget.Text)
                    {
                        case "Term":
                            if (Result.Length > 0)
                            {
                                if (R["Property in Collection"].ToString() == Result)
                                    RowsToRemove.Add(R);
                                else
                                {
                                    R["OK"] = true;
                                    R["Property in ScientificTerms"] = Result;
                                }
                            }
                            else
                            {
                                if (R["Property in Collection"].ToString().Length == 0)
                                    RowsToRemove.Add(R);
                                else
                                    R["OK"] = false;
                            }
                            break;
                        case "Hierarchy":
                            if (Result.Length > 0)
                            {
                                if (R["Hierarchy in Collection"].ToString() == Result)
                                    RowsToRemove.Add(R);
                                else
                                {
                                    R["OK"] = true;
                                    R["Hierarchy in ScientificTerms"] = Result;
                                }
                            }
                            else
                            {
                                if (R["Hierarchy in Collection"].ToString().Length == 0)
                                    RowsToRemove.Add(R);
                                else
                                    R["OK"] = false;
                            }
                            break;
                    }

                    if (this.progressBarSynColTerm.Value < this.progressBarSynColTerm.Maximum)
                        this.progressBarSynColTerm.Value++;
                }
                conTerm.Close();
                conTerm.Dispose();
                int iFits = RowsToRemove.Count;
                foreach (System.Data.DataRow R in RowsToRemove)
                    R.Delete();
                this._dtSynColTerm.AcceptChanges();

                this.dataGridViewSynColTerm.Columns.Clear();
                this.dataGridViewSynColTerm.DataSource = this._dtSynColTerm;
                this.dataGridViewSynColTerm.Columns[0].Width = 30;
                this.dataGridViewSynColTerm.ReadOnly = false;
                this.dataGridViewSynColTerm.Columns[0].Visible = false;
                this.dataGridViewSynColTerm.Columns[1].ReadOnly = false;
                this.dataGridViewSynColTerm.Columns[2].ReadOnly = true;
                this.dataGridViewSynColTerm.Columns[3].ReadOnly = true;
                if (this._dtSynColTerm.Rows.Count > 0)
                {
                    this.labelSynColTermResult.Text = this._dtSynColTerm.Rows.Count.ToString() + " differences found";
                    if (iFits > 0)
                        this.labelSynColTermResult.Text += "\r\n" + iFits.ToString() + " where not different";
                    this.buttonSynColTermUpdate.Enabled = true;
                }
                else
                {
                    this.labelSynColTermResult.Text = "No differences found";
                    this.buttonSynColTermUpdate.Enabled = false;
                }
                this.dataGridViewSynColTerm.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);

                int Start = 3;
                this.SynTermSetCellStyle(this.dataGridViewSynColTerm, Start, Start + 1);
            }
            catch (System.Exception ex) { }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonSynColTermCheckAll_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewSynColTerm.Rows)
            {
                R.Cells[1].Value = true;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonSynColTermCheckNone_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewSynColTerm.Rows)
            {
                R.Cells[1].Value = false;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonSynColTermUpdate_Click(object sender, EventArgs e)
        {
            bool OK = true;

            int ProjectID = 0;
            if (!SynTermsLinkPrechecksPassed(ref ProjectID))
                return;

            string SQL = "";
            string SelectedType = "";// this.comboBoxSynColTermTarget.SelectedItem.ToString();
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            try
            {
                con.Open();
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.progressBarSynColTerm.Value = 0;
                this.progressBarSynColTerm.Maximum = this._dtSynColTerm.Rows.Count;
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                if (this.comboBoxSynColTermProject.SelectedItem != null)
                {
                    System.Data.DataRowView RP = (System.Data.DataRowView)this.comboBoxSynColTermProject.SelectedItem;
                    if (!int.TryParse(RP["ProjectID"].ToString(), out ProjectID))
                        return;
                }
                else return;

                SynTermTargetTable Table = SynTermLinkTargetSelectedTable;

                int i = 0;
                foreach (System.Data.DataRow R in this._dtSynColTerm.Rows)// System.Windows.Forms.DataGridViewRow R in this.dataGridViewSynColTerm.Rows)
                {
                    if (R[1].ToString().ToLower() == "true") //R.Cells[1].Value.ToString().ToLower() == "true")
                    {

                        SQL = "UPDATE T SET "; //";

                        switch ("")//this.comboBoxSynColTermTarget.Text)
                        {
                            case "Term":
                                switch (Table)
                                {
                                    case SynTermTargetTable.SamplingPlotProperty:
                                        SQL += "T.DisplayText ";
                                        break;
                                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                                        SQL += "T.Description ";
                                        break;
                                    case SynTermTargetTable.Identification:
                                        SQL += "T.VernacularTerm ";
                                        break;
                                }
                                SQL += " = '" + R[3].ToString() + "' ";// + R.Cells[3].Value.ToString() + "' ";
                                break;
                            case "Hierarchy":
                                switch (Table)
                                {
                                    case SynTermTargetTable.SamplingPlotProperty:
                                        SQL += "T.PropertyHierarchyCache ";
                                        break;
                                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                                        SQL += "T.DescriptionHierarchyCache ";
                                        break;
                                    case SynTermTargetTable.Identification:
                                        SQL = "UPDATE U SET U.HierarchyCache ";
                                        break;
                                }
                                SQL += " = '" + R[4].ToString() + "' ";// + R.Cells[4].Value.ToString() + "' ";
                                break;
                        }
                        SQL += SynTermsLinkFromClause();


                        SQL += " WHERE ";
                        switch (Table)
                        {
                            case SynTermTargetTable.SamplingPlotProperty:
                                SQL += " T.PropertyURI ";
                                break;
                            case SynTermTargetTable.CollectionSpecimenPartDescription:
                                SQL += " T.DescriptionTermURI ";
                                break;
                            case SynTermTargetTable.Identification:
                                SQL += " T.TermURI ";
                                break;
                        }
                        SQL += "  = '" + R[0].ToString() + "'";// + R.Cells[0].Value.ToString() + "'";
                        if (R.Table.Columns.Contains("CollectionSpecimenID"))
                            SQL += " AND S.CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString();


                        try
                        {
                            C.CommandText = SQL;
                            C.CommandTimeout = DiversitySamplingPlots.Maintenance.FormMaintenanceSettings.Default.Timeout;
                            C.ExecuteNonQuery();
                            foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridViewSynColTerm.Rows[i].Cells)
                                Cell.Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridViewSynColTerm.Rows[i].Cells)
                                Cell.Style.BackColor = System.Drawing.Color.Pink;
                        }
                    }

                    i++;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                System.Windows.Forms.MessageBox.Show("Update finished");
                this.dataGridViewSynColTerm.DataSource = null;
                con.Close();
            }
        }

        private void comboBoxSynColTermTarget_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
            }
            catch (System.Exception ex) { }
        }

        private string SqlTermUpdate(System.Windows.Forms.DataGridViewRow R)
        {
            string SQL = "UPDATE ";
            if (this.comboBoxSynColTermProject.SelectedIndex > 0)
            {
                int ProjectID = 0;
                if (this.comboBoxSynColTermProject.SelectedItem != null)
                {
                    System.Data.DataRowView RP = (System.Data.DataRowView)this.comboBoxSynColTermProject.SelectedItem;
                    if (int.TryParse(RP[0].ToString(), out ProjectID))
                        SQL += " AND P.ProjectID = " + ProjectID.ToString();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please select a project");
                    return "";
                }
            }
            else
                SQL += " AND P.ProjectID IN (SELECT ProjectID FROM ProjectList) ";
            return SQL;
        }

        private string SynTermsLinkFromClause(System.Data.DataRow Row = null, bool ForUpdate = false)
        {
            // Tests
            string SQL = " FROM ";
            try
            {
                SynTermTargetTable Table = SynTermLinkTargetSelectedTable;
                SQL += Table.ToString() + " AS T INNER JOIN CollectionSpecimen S ON T";
                int ProjectID;
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += ".CollectionEventID = S.CollectionEventID AND T.PropertyURI";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += ".CollectionSpecimenID = S.CollectionSpecimenID AND T.DescriptionTermURI";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += ".CollectionSpecimenID = S.CollectionSpecimenID AND T.TermURI";
                        break;
                }
                SQL += " LIKE '" + this._SynTermsLinkServerConnection.BaseURL + "%' ";

                if (Row != null && Row.Table.Columns.Contains("CollectionSpecimenID"))
                {
                    SQL += " AND S.CollectionSpecimenID = " + Row["CollectionSpecimenID"].ToString();
                }

                // getting the project
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxSynColTermProject.SelectedItem;
                if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
                    SQL += " INNER JOIN CollectionProject P ON S.CollectionSpecimenID = P.CollectionSpecimenID AND P.ProjectID = " + ProjectID.ToString();

                SQL += " CROSS JOIN " + _SynTermsLinkServerConnection.Prefix() + "ViewBaseURL AS B";
                SQL += " INNER JOIN " + _SynTermsLinkServerConnection.Prefix() + "TermRepresentation R ON T.";
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += "PropertyURI";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += "DescriptionTermURI";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += "TermURI";
                        break;
                }
                SQL += " = B.BaseURL + cast(R.RepresentationID as varchar) ";

                return SQL;
            }

            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                SQL = "";
            }
            return SQL;
        }

        private string SynTermsLinkGroupAndOrderClause() //System.Data.DataRow Row = null, bool ForUpdate = false)
        {
            // Tests
            string SQL = " ";
            try
            {
                SynTermTargetTable Table = SynTermLinkTargetSelectedTable;
                SQL += " GROUP BY T.";
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += "DisplayText, T.PropertyURI ";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += "Description, T.DescriptionTermURI ";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += "VernacularTerm, T.TermURI ";
                        break;
                }

                if (this.checkBoxSynLinkedTermIncludePlot.Checked)
                    SQL += ", S.AccessionNumber, S.CollectionSpecimenID ";
                SQL += " ORDER BY T.";
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += "DisplayText";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += "Description";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += "VernacularTerm";
                        break;
                }

            }

            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                SQL = "";
            }
            return SQL;
        }

        #endregion

        #endregion

        #region Via Text

        //private void InitSynchronizeTerm_EventProperty_Text()
        //{
        //    try
        //    {

        //        tabPageSynchroTermsText.Controls.Clear();
        //        DiversitySamplingPlots.Maintenance.SynchronizeTerm synchronizeTerm = new DiversitySamplingPlots.Maintenance.SynchronizeTerm(DiversitySamplingPlots.Maintenance.SynchronizeTerm.TargetTable.SamplingPlotProperty, false);
        //        DiversitySamplingPlots.Maintenance.UserControlSynchronizeTerm userControlSynchronizeTerm = new DiversitySamplingPlots.Maintenance.UserControlSynchronizeTerm(synchronizeTerm);
        //        userControlSynchronizeTerm.Dock = DockStyle.Fill;
        //        tabPageSynchroTermsText.Controls.Add(userControlSynchronizeTerm);
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //}

        #region Obsolete

        //private DiversitySamplingPlots.Maintenance.SynchronizeTerm _SynchronizeTerm_EventProperty_Text;

        //private DiversitySamplingPlots.Maintenance.SynchronizeTerm SynchronizeTerm_EventProperty_Text
        //{
        //    get
        //    {
        //        if (_SynchronizeTerm_EventProperty_Text == null)
        //        {
        //            _SynchronizeTerm_EventProperty_Text = new DiversitySamplingPlots.Maintenance.SynchronizeTerm(DiversitySamplingPlots.Maintenance.SynchronizeTerm.TargetTable.SamplingPlotProperty, false,
        //                this.dataGridViewSynTermsText, this._dtSynTermsText, this.labelSynTermsTextResult,
        //                this.buttonSynTermsTextUpdate);
        //        }
        //        return _SynchronizeTerm_EventProperty_Text;
        //    }
        //}

        private DiversityWorkbench.ServerConnection _SynTermsTextServerConnection;

        private System.Data.DataTable _dtSynTermsText;

        #region Hierarchy
        //private void checkBoxSynPartDescriptionTextIncludeHierarchy_Click(object sender, EventArgs e)
        //{
        //    this.SynPartDescriptionTextSetHierarchy();
        //}

        //private void radioButtonSynPartDescriptionTextIncludeHierarchyDetailTop_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.SynPartDescriptionTextSetHierarchy();
        //}

        //private void radioButtonSynPartDescriptionTextIncludeHierarchyTopDetail_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.SynPartDescriptionTextSetHierarchy();
        //}

        //private void SynPartDescriptionTextSetHierarchy()
        //{
        //    if (this.checkBoxSynPartDescriptionTextIncludeHierarchy.Checked)
        //    {
        //        if (this.radioButtonSynPartDescriptionTextIncludeHierarchyDetailTop.Checked)
        //            SynchronizeTerm_PartDescription_Text.setHierarchy(DiversitySamplingPlots.Maintenance.SynchronizeTerm.Hierarchy.DetailTop);
        //        else
        //            SynchronizeTerm_PartDescription_Text.setHierarchy(DiversitySamplingPlots.Maintenance.SynchronizeTerm.Hierarchy.TopDetail);
        //    }
        //    else SynchronizeTerm_PartDescription_Text.setHierarchy(DiversitySamplingPlots.Maintenance.SynchronizeTerm.Hierarchy.None);
        //}

        #endregion


        private void buttonSynTermsTextCheckSingle_Click(object sender, EventArgs e)
        {
            //SynchronizeTerm_PartDescription_Text.ShowDetails(this.Width, this.Height, this.Location);

        }

        private SynTermTargetTable SynTermTextTargetSelectedTable
        {
            get
            {
                SynTermTargetTable T = SynTermTargetTable.SamplingPlotProperty;
                return T;
            }
        }

        private bool SynTermsTextPrechecksPassed()
        {
            bool OK = true;
            try
            {
                //if (this.SynTermTextTargetSelectedTable == SynTermTargetTable.NULL)
                //{
                //    System.Windows.Forms.MessageBox.Show("Please select the table that should be checked");
                //    return false;
                //}
                if (this.comboBoxSynTermsTextDatabaseProject.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the terminology that should be checked");
                    return false;
                }
                if (this.comboBoxSynTermsTextProject.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the project that should be checked");
                    return false;
                }
                if (this._SynTermsTextServerConnection == null || this._SynTermsTextServerConnection.ConnectionString.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the database that should be checked");
                    return false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void buttonSynTermsTextSearch_Click(object sender, EventArgs e)
        {
            if (!SynTermsTextPrechecksPassed())
                return;

            try
            {
                SynTermTargetTable Table = SynTermTextTargetSelectedTable;
                string SQL = "SELECT ";
                SQL += " CASE WHEN COUNT(DISTINCT R.RepresentationID) = 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS OK, T.";
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += "DisplayText";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += "Description";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += "VernacularTerm";
                        break;
                }

                SQL += " AS [Term in " + Table.ToString() + "] ";
                SQL += ",  MIN(R.DisplayText) AS [Term in DiversityScientificTerms], ";
                SQL += "MIN(R.HierarchyCacheDown) AS Hierarchy, ";
                SQL += "COUNT(DISTINCT R.RepresentationID) AS Count, MIN(B.[BaseURL]) + CAST(MIN(R.RepresentationID) AS varchar) AS URL ";
                if (this.checkBoxSynTermsTextIncludePlot.Visible && this.checkBoxSynTermsTextIncludePlot.Checked)
                    SQL += ", S.PlotIdentifier, S.PlotID ";
                SQL += " " + this.SynTermsTextFromClause();

                this._dtSynTermsText = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversitySamplingPlots.Maintenance.FormMaintenanceSettings.Default.Timeout));
                ad.SelectCommand.CommandTimeout = DiversitySamplingPlots.Maintenance.FormMaintenanceSettings.Default.Timeout;
                try
                {
                    ad.Fill(this._dtSynTermsText);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                this.dataGridViewSynTermsText.DataSource = this._dtSynTermsText;
                for (int i = 1; i < this.dataGridViewSynTermsText.Columns.Count; i++)
                {
                    this.dataGridViewSynTermsText.Columns[i].ReadOnly = true;
                }

                this.SynTermSetCellStyle(this.dataGridViewSynTermsText, 2, 6);

                this.dataGridViewSynTermsText.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                if (this._dtSynTermsText.Rows.Count > 0)
                {
                    this.labelSynTermsTextResult.Text = this._dtSynTermsText.Rows.Count.ToString() + " matches found";
                    this.buttonSynTermsTextUpdate.Enabled = true;
                }
                else
                {
                    this.labelSynTermsTextResult.Text = "No match found";
                    //this.buttonSynColAgeTextUpdate.Enabled = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSynTermsTextSelectAll_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewSynTermsText.Rows)
            {
                R.Cells[0].Value = true;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewSynTermsText.Rows)
            {
                R.Cells[0].Value = false;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonSynTermsTextUpdate_Click(object sender, EventArgs e)
        {
            //SynchronizeTerm_PartDescription_Text.Update();

        }


        private void comboBoxSynTermsTextTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // nicht notwendig  -hier gibt es nur eine Tabelle

            //System.Data.DataTable dtRestriction = new DataTable();
            //string SQL = "";
            //switch (SynTermTextTargetSelectedTable)
            //{
            //    case SynTermTargetTable.NULL:
            //        this.comboBoxSynTermsTextProperty.DataSource = null;
            //        this.labelSynTermsTextProperty.Text = "nothing selected";
            //        this.labelSynTermsTextProperty.Image = null;
            //        break;
            //    case SynTermTargetTable.SamplingPlotProperty:
            //        this.labelSynTermsTextProperty.Text = "      Property";
            //        this.labelSynTermsTextProperty.Image = DiversitySamplingPlots.Properties.Resources.Habitat;
            //        SQL = "SELECT PropertyID AS Value, DisplayText AS Display FROM Property ORDER BY Display";
            //        goto default;
            //    case SynTermTargetTable.CollectionSpecimenPartDescription:
            //        this.labelSynTermsTextProperty.Text = "      Material category";
            //        this.labelSynTermsTextProperty.Image = DiversitySamplingPlots.Resource.Specimen;
            //        SQL = "SELECT Code AS Value, DisplayText AS Display FROM CollMaterialCategory_Enum ORDER BY Display";
            //        goto default;
            //    case SynTermTargetTable.Identification:
            //        this.labelSynTermsTextProperty.Text = "      Taxonomic group";
            //        this.labelSynTermsTextProperty.Image = DiversitySamplingPlots.Resource.Plant;
            //        SQL = "SELECT Code AS Value, DisplayText AS Display FROM CollTaxonomicGroup_Enum ORDER BY Display";
            //        goto default;
            //    default:
            //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtRestriction);
            //        this.comboBoxSynTermsTextProperty.DataSource = dtRestriction;
            //        this.comboBoxSynTermsTextProperty.DisplayMember = "Display";
            //        this.comboBoxSynTermsTextProperty.ValueMember = "Value";
            //        break;
            //}
        }

        private void comboBoxSynTermsTextDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelSynTermsTextDatabase.Text = "      Database:";
            this._SynTermsTextServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList()[this.comboBoxSynTermsTextDatabase.SelectedItem.ToString()];
            if (this._SynTermsTextServerConnection.LinkedServer.Length > 0)
                this.labelSynTermsTextDatabase.Text = "      " + this._SynTermsTextServerConnection.DatabaseName;
            this.setSynTermsTextTerminologies();
        }

        private string SynTermsTextFromClause(System.Data.DataRow Row = null, bool ForUpdate = false)
        {
            // Tests
            string SQL = " FROM ";
            try
            {
                SynTermTargetTable Table = SynTermTextTargetSelectedTable;
                SQL += Table.ToString() + " AS T INNER JOIN SamplingPlot S ON T";
                int ProjectID;
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += ".PlotID = S.PlotID AND (T.PropertyURI IS NULL OR T.PropertyURI = '') ";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += ".CollectionSpecimenID = S.CollectionSpecimenID AND (T.DescriptionTermURI IS NULL OR T.DescriptionTermURI = '') ";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += ".CollectionSpecimenID = S.CollectionSpecimenID AND (T.TermURI IS NULL OR T.TermURI = '') ";
                        break;
                }
                if (Row != null && Row.Table.Columns.Contains("PlotID"))
                {
                    SQL += " AND S.PlotID = " + Row["PlotID"].ToString();
                }

                // getting the project
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxSynTermsTextProject.SelectedItem;
                if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
                    SQL += " INNER JOIN SamplingProject P ON S.PlotID = P.PlotID AND P.ProjectID = " + ProjectID.ToString();

                SQL += " INNER JOIN " + _SynTermsTextServerConnection.Prefix() + "TermRepresentation R ON T.";
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += "DisplayText";
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += "Description";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += "VernacularTerm";
                        break;
                }
                SQL += " collate database_default = R.DisplayText collate database_default AND R.LanguageCode = '" + this.comboBoxSynTermsTextDatabaseLanguage.SelectedValue.ToString() + "' ";
                // Terminology
                SQL += " AND R.TerminologyID = " + this.comboBoxSynTermsTextDatabaseProject.SelectedValue.ToString();
                // No Rankging terms
                SQL += " INNER JOIN " + _SynTermsTextServerConnection.Prefix() + "Term DST ON R.TermID = DST.TermID AND DST.IsRankingTerm = 0 ";
                // BaseURL
                SQL += " CROSS JOIN " + _SynTermsTextServerConnection.Prefix() + "ViewBaseURL AS B";
                switch (Table)
                {
                    case SynTermTargetTable.SamplingPlotProperty:
                        SQL += " WHERE T.PropertyID = " + this.comboBoxSynTermsTextProperty.SelectedValue.ToString();
                        break;
                    case SynTermTargetTable.CollectionSpecimenPartDescription:
                        SQL += " INNER JOIN CollectionSpecimenPart CSP ON CSP.CollectionSpecimenID = T.CollectionSpecimenID AND CSP.SpecimenPartID = T.SpecimenPartID AND CSP.MaterialCategory = '" + this.comboBoxSynTermsTextProperty.SelectedValue.ToString() + "'";
                        break;
                    case SynTermTargetTable.Identification:
                        SQL += " INNER JOIN IdentificationUnit U ON U.CollectionSpecimenID = T.CollectionSpecimenID AND U.IdentificationUnitID = T.IdentificationUnitID AND U.TaxonomicGroup = '" + this.comboBoxSynTermsTextProperty.SelectedValue.ToString() + "'";
                        break;
                }
                if (!ForUpdate)
                {
                    SQL += " GROUP BY T.";
                    switch (Table)
                    {
                        case SynTermTargetTable.SamplingPlotProperty:
                            SQL += "DisplayText";
                            break;
                        case SynTermTargetTable.CollectionSpecimenPartDescription:
                            SQL += "Description";
                            break;
                        case SynTermTargetTable.Identification:
                            SQL += "VernacularTerm";
                            break;
                    }
                    if (this.checkBoxSynTermsTextIncludePlot.Checked)
                        SQL += ", S.AccessionNumber, S.PlotID ";
                }

                return SQL;
            }

            catch (System.Exception ex)
            {
                SQL = "";
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }

        private void setSynTermsTextTerminologies()
        {
            string Prefix = "dbo.";
            if (this._SynTermsTextServerConnection.LinkedServer != null && this._SynTermsTextServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._SynTermsTextServerConnection.LinkedServer + "].[" + this._SynTermsTextServerConnection.DatabaseName + "].dbo.";
            string SQL = "SELECT TerminologyID, DisplayText FROM " + Prefix + "Terminology ORDER BY DisplayText";
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, this._SynTermsTextServerConnection.ConnectionString);
            this.comboBoxSynTermsTextDatabaseProject.DataSource = dt;
            this.comboBoxSynTermsTextDatabaseProject.DisplayMember = "DisplayText";
            this.comboBoxSynTermsTextDatabaseProject.ValueMember = "TerminologyID";
            if (dt.Rows.Count > 0)
            {
                this.comboBoxSynTermsTextDatabaseProject.SelectedIndex = 0;
            }
            SQL = "SELECT Code, DisplayText FROM " + Prefix + "LanguageCode_Enum ORDER BY DisplayText";
            System.Data.DataTable dtLang = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtLang, this._SynTermsTextServerConnection.ConnectionString);
            this.comboBoxSynTermsTextDatabaseLanguage.DataSource = dtLang;
            this.comboBoxSynTermsTextDatabaseLanguage.DisplayMember = "DisplayText";
            this.comboBoxSynTermsTextDatabaseLanguage.ValueMember = "Code";
            if (dtLang.Rows.Count > 0)
            {
                this.comboBoxSynTermsTextDatabaseLanguage.SelectedIndex = 0;
            }
        }

        private int? SynTermsTextTerminologyID()
        {
            try
            {
                if (this.comboBoxSynTermsTextDatabaseProject.SelectedIndex > -1)
                {
                    int i;
                    if (int.TryParse(this.comboBoxSynTermsTextDatabaseProject.SelectedValue.ToString(), out i))
                        return i;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return null;
        }

        private void dataGridViewSynTermsText_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.SynTermsTextSetCheckSingleButtonVisibile();
        }

        private void checkBoxSynTermsTextIncludeAccNr_CheckedChanged(object sender, EventArgs e)
        {
            this.SynTermsTextSetCheckSingleButtonVisibile();
        }

        private void SynTermsTextSetCheckSingleButtonVisibile()
        {
            if (this.checkBoxSynTermsTextIncludePlot.Visible &&
                this.checkBoxSynTermsTextIncludePlot.Checked &&
                this.dataGridViewSynTermsText.SelectedRows.Count == 1)
            {
                if (this.dataGridViewSynTermsText.SelectedCells.Count > 0)
                {
                    if (this.dataGridViewSynTermsText.SelectedCells[0].RowIndex == this.dataGridViewSynTermsText.SelectedRows[0].Index)
                        this.buttonSynTermsTextCheckSingle.Visible = true;
                    else
                        this.buttonSynTermsTextCheckSingle.Visible = false;
                }
                else
                    this.buttonSynTermsTextCheckSingle.Visible = true;
            }
            else
                this.buttonSynTermsTextCheckSingle.Visible = false;
        }

        private void dataGridViewSynTermsText_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.SynTermsTextSetCheckSingleButtonVisibile();
        }

        #endregion

        #endregion

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async Task InitManual()
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
        /// Set help keywords for controls where it does not work automatically
        /// </summary>
        private void InitManualControls()
        {
            try
            {
                helpProvider.SetHelpKeyword(this, "maintenance_dsp");
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
                    this.InitManualControls();
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
