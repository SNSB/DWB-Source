using DiversityWorkbench.Utilities;
using DiversityWorkbench.DwbManual;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfSamplingPlotPage;


namespace DiversitySamplingPlots.Forms
{
    public partial class FormDiversitySamplingPlots : Form, DiversityWorkbench.Spreadsheet.iMainForm, DiversityWorkbench.WorkbenchUnitUserInterface, DiversityWorkbench.Maintenance.iMaintenanceCheckSingle
    {
        #region Parameter

        private bool? _LocalDatabaseNotMissing = null;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotDataTable _dtSamplingPlot;
        private System.Data.DataTable _DtReplicationPublisher;

        // GEOGRAPHY
        private enum MeasurementUnit { m, km, feet, mile, degree, percent, numeric, deg_min_sec, orientation };
        private string[] NamedOrientation = new string[16] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
        private readonly string GeoOriValStart = "[Ori.val.: ";
        private readonly string GeoOriValEnd = "]";

        private System.Collections.Generic.Dictionary<string, int> _SpatialReferenceIDs;

        private System.Data.DataTable _dtGeo = new DataTable();
        private System.Data.DataTable _dtGeo1 = new DataTable();
        private System.Data.DataTable _dtLocalisationList = new DataTable();

        private string _SqlProjectProxy = "SELECT ProjectID, Project, ProjectURI FROM ProjectProxy P WHERE P.ProjectID IN (SELECT ProjectID FROM ProjectList)";
        private Microsoft.Data.SqlClient.SqlDataAdapter _adapterProjectProxy;
        private System.Data.DataTable _dtProjectProxy;

        private System.Collections.Generic.List<string> _DatabaseRoles;
        private bool m_ConnectionOK = false;
        private int m_LastID = 0;

        private char[] SplitChars = { ' ', '(', ')' };

        // Parameter for command line processing (von Toni übernommen)
        private enum ProcessingMode { RegularInteractive, InteractiveQuery, SetDatabase, UpdateDatabase, HiddenCache, HiddenArchive, IrregularAbort };
        private ProcessingMode _Processing = ProcessingMode.RegularInteractive;
        private int _ProcessingResult = 0;

        private CancellationTokenSource _userCts;
        #endregion

        #region SQL data adapters

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSamplingPlot;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSamplingProject;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSamplingPlotProperty;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSamplingPlotResource;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSamplingPlotLocalisation;

        #endregion

        #region Properties

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        public System.Data.DataTable DtProjectProxy
        {
            get
            {
                if (this._dtProjectProxy == null)
                {
                    this._dtProjectProxy = new DataTable();
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._adapterProjectProxy, this._dtProjectProxy, this._SqlProjectProxy, DiversityWorkbench.Settings.ConnectionString);
                }
                return _dtProjectProxy;
            }
            set { _dtProjectProxy = value; }
        }

        public System.Collections.Generic.List<string> DatabaseRoles
        {
            get
            {
                if (this._DatabaseRoles == null)
                    this._DatabaseRoles = DiversityWorkbench.Database.DatabaseRoles();
                return _DatabaseRoles;
            }
            set { _DatabaseRoles = value; }
        }

        internal DiversitySamplingPlots.SamplingPlot SamplingPlot
        {
            get
            {
                DiversitySamplingPlots.SamplingPlot S = new SamplingPlot(this.ServerConnection);
                return S;
            }
        }

        public int PlotID
        {
            get
            {
                int ID = -1;
                if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0)
                    ID = System.Int32.Parse(this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotID"].ToString());
                return ID;
            }
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = DiversityWorkbench.Settings.ServerConnection;
                return S;
            }
        }

        public bool LocalDatabaseNotMissing
        {
            get
            {
                if (this._LocalDatabaseNotMissing == null)
                {
                    this._LocalDatabaseNotMissing = true;
                    if (DiversityWorkbench.Settings.ConnectionString.StartsWith("127.0.0.1"))
                    {
                        if (!this.FormFunctions.DatabaseAccessible)
                            this._LocalDatabaseNotMissing = false;
                    }
                }
                return (bool)_LocalDatabaseNotMissing;
            }
        }

        private bool ShowMap
        {
            get
            {
                if (this.buttonHeaderShowMap.BackColor == System.Drawing.Color.Red)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormDiversitySamplingPlots()
        {
            try
            {
                WebView2UserDataFolderCleanup.CleanupStaleSessions();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("CleanUp old NetWebView2 Session folders failed: " + ex);
            }
            InitializeComponent();
            try
            {
                // Set module name
                DiversityWorkbench.Settings.ModuleName = "DiversitySamplingPlots";

                this.KeyPreview = true;

                // Init database
                if (DiversityWorkbench.Settings.DatabaseServer != "127.0.0.1")
                    this.setDatabase();
                else
                    this.splitContainerData.Visible = false;

                // Init form
                this.initForm();

                // Init query
                this.initQuery();

                // Init menu
                this.initMenu();

                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                DiversityWorkbench.Forms.FormFeedback.HelpProviderHelpNamespace = this.helpProvider.HelpNamespace;

                this.splitContainerData.Visible = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormDiversitySamplingPlots(string[] args)
            : this()
        {
            try
            {
                System.Collections.Generic.List<string> Arguments = new List<string>();
                string ArgumentsString = "";
                foreach (var arg in args)
                {
                    if (ArgumentsString.Length > 0)
                        ArgumentsString += "; ";
                    ArgumentsString += arg;
                    Arguments.Add(arg.ToString());
                }
                string Target = Arguments[0].ToLower().Trim();
                switch (Target)
                {
                    case "updatedatabase": //  (von Toni übernommen)
                        // UpdateDatabase <server> <port> 
                        #region UpdateDatabase
                        // Read server address and update available databases
                        try
                        {
                            // Save data
                            _Processing = ProcessingMode.UpdateDatabase;
                            string Server = Arguments[1];
                            int Port = int.Parse(Arguments[2]);

                            // Update database parameter
                            if (DiversityWorkbench.Database.SilentDatabaseUpdate(Server,
                                                                                 Port,
                                                                                 DiversitySamplingPlots.Properties.Settings.Default.DatabaseVersion,
                                                                                 "DiversitySamplingPlotsUpdate_",
                                                                                 this.helpProvider.HelpNamespace,
                                                                                 Properties.Resources.ResourceManager))
                            {
                                // Mark success
                                _ProcessingResult = 1;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            string message = ex.Message + "\r\n\r\n" + ArgumentsString;
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(this.Name, "FormDiversitySamplingPlots(string[] args)", message);
                            _Processing = ProcessingMode.IrregularAbort;
                        }
                        finally
                        {
                            this.Close();
                        }
                        #endregion
                        break;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormDiversitySamplingPlots(int PlotID) : this()
        {
            try
            {
                this.setSamplingPlot(PlotID);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion // Construction

        #region Initialization

        #region Init database

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool setDatabase()
        {
            bool OK = true;
            this.dataSetSamplingPlot.Clear();

            if (!CheckConnection(DiversityWorkbench.Settings.ConnectionString + ";Connect Timeout=10"))
            {
                OK = false;
            }
            else if (DiversityWorkbench.Settings.ConnectionString != "" && this.FormFunctions.DatabaseAccessible && LocalDatabaseNotMissing)
            {
                try
                {
                    this.resetSqlAdapters();
                    DiversityWorkbench.WorkbenchUnit.ResetWorkbenchUnitConnections();
                    DiversityWorkbench.Forms.FormFunctions.ResetPermissionsCache();
                    DiversityWorkbench.Database.DatabaseRolesReset();

                    this.setToolbarPermissions();
                    //this.setSearchOptions(); // dauert
                    this.initRemoteModules();
                    this.userControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionString, "SamplingPlot");
                    this.FormFunctions.setDescriptions();

                    DiversityWorkbench.WorkbenchUnit.LoadAddedRemoteConnections();
                    this.initMenu();  // wird das hier überhaupt ausgeführt ?? abhängig von m_ConnectionOK == true !!
                    // this.initQuery(); // ruft setSearchOptions() nochmal auf ??
                }
                catch (Exception ex)
                {
                    OK = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
                OK = false;

            //DiversityWorkbench.Entity.Reset();
            //DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            this.splitContainerData.Visible = false;

            if (OK)
            {
                this.FormFunctions.setTitle();
                this.userControlQueryList.toolStripButtonConnection.ToolTipText = "Change database. Current connection: " + this.FormFunctions.ConnectionInfo();
            }
            this.setHeader();
            m_ConnectionOK = OK;
            //this.setMenu();
            return OK;
        }


        /// <summary>
        /// Check Connection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool CheckConnection(string connectionString)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                connection.Close();
            }
            catch
            {
                // MessageBox.Show("DatabaseQuery failed: " + ex.Message);
                return false;
            }
            // Return obsolet
            return true;
        }


        /// <summary>
        /// Reset SQL data adapters.
        /// </summary>
        private void resetSqlAdapters()
        {
            this.sqlDataAdapterSamplingPlot = null;
            this.sqlDataAdapterSamplingProject = null;
        }


        /// <summary>
        /// DiversityWorkbench dictionary and permission stuff.
        /// </summary>
        private void setToolbarPermissions()
        {
            try
            {
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>>> DataControlDictionary = new Dictionary<string, Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, List<Control>>>();

                // SamplingPlot
                System.Collections.Generic.List<System.Windows.Forms.Control> SamplingPlotControls = new List<Control>();
                SamplingPlotControls.Add(this.tableLayoutPanelHeader);
                SamplingPlotControls.Add(this.groupBoxHierarchy);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>> SamplingPlotDict = new Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, List<System.Windows.Forms.Control>>();
                SamplingPlotDict.Add(DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update, SamplingPlotControls);
                DataControlDictionary.Add("SamplingPlot", SamplingPlotDict);

                // SamplingPlotLocalisation
                System.Collections.Generic.List<System.Windows.Forms.Control> SamplingPlotLocalisationControls = new List<Control>();
                SamplingPlotLocalisationControls.Add(this.groupBoxPlotLocalisation);
                SamplingPlotLocalisationControls.Add(this.toolStripLocalisation);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>> SamplingPlotLocalisationDict = new Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, List<System.Windows.Forms.Control>>();
                SamplingPlotLocalisationDict.Add(DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update, SamplingPlotLocalisationControls);
                DataControlDictionary.Add("SamplingPlotLocalisation", SamplingPlotLocalisationDict);

                // SamplingProject
                System.Collections.Generic.List<System.Windows.Forms.Control> SamplingProjectControls = new List<Control>();
                SamplingProjectControls.Add(this.groupBoxProjects);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>> SamplingProjectDict = new Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, List<System.Windows.Forms.Control>>();
                SamplingProjectDict.Add(DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update, SamplingProjectControls);
                DataControlDictionary.Add("SamplingProject", SamplingProjectDict);

                // SamplingPlotProperty
                System.Collections.Generic.List<System.Windows.Forms.Control> SamplingPlotPropertyControls = new List<Control>();
                SamplingPlotPropertyControls.Add(this.groupBoxProperties);
                SamplingPlotPropertyControls.Add(this.toolStripProperties);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>> SamplingPlotPropertyDict = new Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, List<System.Windows.Forms.Control>>();
                SamplingPlotPropertyDict.Add(DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update, SamplingPlotPropertyControls);
                DataControlDictionary.Add("SamplingPlotProperty", SamplingPlotPropertyDict);

                // SamplingPlotResource
                System.Collections.Generic.List<System.Windows.Forms.Control> SamplingPlotResourceControls = new List<Control>();
                SamplingPlotResourceControls.Add(this.groupBoxFile);
                SamplingPlotResourceControls.Add(this.toolStripFile);
                System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>> SamplingPlotResourceDict = new Dictionary<DiversityWorkbench.Forms.FormFunctions.DatabaseGrant, List<System.Windows.Forms.Control>>();
                SamplingPlotResourceDict.Add(DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update, SamplingPlotResourceControls);
                DataControlDictionary.Add("SamplingPlotResource", SamplingPlotResourceDict);


                this.FormFunctions.setControlsAccordingToPermissions(DataControlDictionary);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        /// <summary>
        /// Remote module connections for Agents, Gazetteers and ScientificTerms
        /// </summary>
        private void initRemoteModules()
        {
            try
            {
                // Collection
                //DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                //this.userControlModuleRelatedEntryCollectionSpecimen.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)C;

                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryFileResponsible.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryPropertyResponsible.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;

                this.userControlModuleRelatedEntryFileResponsible.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
                this.userControlModuleRelatedEntryPropertyResponsible.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

                this.userControlModuleRelatedEntryFileResponsible.bindToData("SamplingPlotResource", "ResponsibleName", "ResponsibleAgentURI", this.samplingPlotResourceBindingSource);
                this.userControlModuleRelatedEntryPropertyResponsible.bindToData("SamplingPlotProperty", "ResponsibleName", "ResponsibleAgentURI", this.samplingPlotPropertyBindingSource);

                // Gazetteers
                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryGazetteer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;

                this.userControlModuleRelatedEntryGazetteer.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

                this.userControlModuleRelatedEntryGazetteer.bindToData("SamplingPlotLocalisation", "Location1", "Location2", "LocalisationSystemID = 7", this.samplingPlotLocalisationBindingSource);

                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbGazetteer = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();

                DiversityWorkbench.UserControls.RemoteValueBinding RvbCountry = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbCountry.BindingSource = this.samplingPlotLocalisationBindingSource;
                RvbCountry.Column = "CountryCache";
                RvbCountry.RemoteParameter = "Country";
                RvbGazetteer.Add(RvbCountry);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbLatitude.BindingSource = this.samplingPlotLocalisationBindingSource;
                RvbLatitude.Column = "AverageLatitudeCache";
                RvbLatitude.RemoteParameter = "Latitude";
                RvbGazetteer.Add(RvbLatitude);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbLongitude.BindingSource = this.samplingPlotLocalisationBindingSource;
                RvbLongitude.Column = "AverageLongitudeCache";
                RvbLongitude.RemoteParameter = "Longitude";
                RvbGazetteer.Add(RvbLongitude);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbAltitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbAltitude.BindingSource = this.samplingPlotLocalisationBindingSource;
                RvbAltitude.Column = "AverageAltitudeCache";
                RvbAltitude.RemoteParameter = "Altitude";
                RvbGazetteer.Add(RvbAltitude);

                //DiversityWorkbench.UserControls.RemoteValueBinding RvbGazGeo = new DiversityWorkbench.UserControls.RemoteValueBinding();
                //RvbGazGeo.BindingSource = this.CollectionEventLocalisationGeoBindingSource;
                //RvbGazGeo.Column = "GeographyAsString";
                //RvbGazGeo.RemoteParameter = "Geography";
                //RvbGazetteer.Add(RvbGazGeo);

                this.userControlModuleRelatedEntryGazetteer.setRemoteValueBindings(RvbGazetteer);

                // ScientificTerms
                DiversityWorkbench.ScientificTerm S = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryProperty.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S;
                this.userControlModuleRelatedEntryProperty.IsListInDatabase = true;

                this.userControlModuleRelatedEntryProperty.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

                this.userControlModuleRelatedEntryProperty.bindToData("SamplingPlotProperty", "DisplayText", "PropertyURI", this.samplingPlotPropertyBindingSource);
                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbScientificTerm = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();

                DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchy = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbHierarchy.BindingSource = this.samplingPlotPropertyBindingSource;
                RvbHierarchy.Column = "PropertyHierarchyCache";
                RvbHierarchy.RemoteParameter = "HierarchyCache";
                RvbScientificTerm.Add(RvbHierarchy);

                this.userControlModuleRelatedEntryProperty.setRemoteValueBindings(RvbScientificTerm);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void setHeader()
        {
            try
            {
                if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0)
                {
                    this.textBoxHeaderID.Text = this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotID"].ToString();
                    if (!this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].Equals(System.DBNull.Value))
                        this.textBoxHeaderPlotIdentifier.Text = this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].ToString();
                    else
                        this.textBoxHeaderPlotIdentifier.Text = "";
                }
                else
                {
                    this.textBoxHeaderID.Text = "";
                    this.textBoxHeaderPlotIdentifier.Text = "";
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion // Init database

        #region Init form

        /// <summary>
        /// init Form
        /// </summary>
        private void initForm()
        {
            try
            {
                this.AcceptButton = this.userControlDialogPanel.buttonOK;
                this.CancelButton = this.userControlDialogPanel.buttonCancel;
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                if (m_ConnectionOK)
                {
                    this.setDatabindings();
                    this.initRemoteModules();
                }
                this.initSRID();
                this.setQueryControlEvents();
                this.userControlGeography.InitControl(this);
                //this.initGeometryTypes();
                this.askOnExitToolStripMenuItem.Checked = DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.AskOnExit;
                this.setImageVisibilityMarker(DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.ImageVisibility);
                this.splitContainerData.SplitterDistance = DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.SplitContainerData_SplitterDistance;

                //Markus 23.2.22 adaption for ManyOrderByColumns
                this.userControlQueryList.ManyOrderByColumns_Allow(true);
                this.userControlQueryList.OptimizingAvailable(true);
                this.userControlQueryList.ManyOrderByColumns_InitControls();

                this.initCollectionImages();

                this.initBackLinks();


            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        /// <summary>
        /// set Databindings
        /// </summary>
        private void setDatabindings()
        {
            try
            {
                this.setUserControlDatabindings();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        /// <summary>
        /// set UserControl Databindings
        /// </summary>
        private void setUserControlDatabindings()
        {
            try
            {
                // Gazetteer

                this.userControlModuleRelatedEntryGazetteer.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryGazetteer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;

                this.userControlModuleRelatedEntryGazetteer.bindToData("SamplingPlotLocalisation", "Location1", "Location2", "LocalisationSystemID = 7", this.samplingPlotLocalisationBindingSource);

                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbGazetteer = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();

                DiversityWorkbench.UserControls.RemoteValueBinding RvbGazGeo = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbGazGeo.BindingSource = this.samplingPlotLocalisationBindingSource;
                RvbGazGeo.Column = "GeographyAsString";
                RvbGazGeo.RemoteParameter = "Geography";
                RvbGazetteer.Add(RvbGazGeo);

                this.userControlModuleRelatedEntryGazetteer.setRemoteValueBindings(RvbGazetteer);

                // responsible
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryLocalisationResponsible.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;

                this.userControlModuleRelatedEntryLocalisationResponsible.bindToData("SamplingPlotLocalisation", "ResponsibleName", "ResponsibleAgentURI", this.samplingPlotLocalisationBindingSource);

                this.userControlModuleRelatedEntryLocalisationResponsible.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void initSRID()
        {
            this._SpatialReferenceIDs = new Dictionary<string, int>();
            this._SpatialReferenceIDs.Add("WGS 84", 4326);
            this._SpatialReferenceIDs.Add("Potsdam Datum", 4746);
        }


        private void setQueryControlEvents()
        {
            try
            {
                // QueryList
                this.userControlQueryList.toolStripButtonConnection.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
                this.userControlQueryList.toolStripButtonCopy.Click += new System.EventHandler(this.copySamplingPlot);
                this.userControlQueryList.toolStripButtonDelete.Click += new System.EventHandler(this.deletePlot);
                this.userControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.createNewPlot);
                this.userControlQueryList.toolStripButtonSave.Click += new System.EventHandler(this.savePlot);
                this.userControlQueryList.toolStripButtonUndo.Click += new System.EventHandler(this.undoChangesInPlot);
                this.userControlQueryList.toolStripButtonSwitchOrientation.Click += new System.EventHandler(this.switchQueryOrientation);
                this.userControlQueryList.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        /// <summary>
        /// set Image Visibility Marker
        /// </summary>
        /// <param name="Visibility"></param>
        private void setImageVisibilityMarker(string Visibility)
        {
            if (Visibility.Length > 0 && Visibility.Substring(0, 1) == "1")
                this.buttonHeaderShowMap.BackColor = System.Drawing.Color.Red;
            else
            {
                if (this.dataSetSamplingPlot.SamplingPlotGeography.Rows.Count > 0)
                {
                    if (this.ShowMap) this.buttonHeaderShowMap.BackColor = System.Drawing.Color.Red;
                    else this.buttonHeaderShowMap.BackColor = System.Drawing.Color.Yellow;
                }
                else
                    this.buttonHeaderShowMap.BackColor = System.Drawing.SystemColors.Control;
            }

            //if (Visibility.Length > 0 && Visibility.Substring(1, 1) == "1")
            //    this.buttonHeaderShowGeometry.BackColor = System.Drawing.Color.Red;
            //else
            //{
            //    if (this.dataSetSamplingPlot.SamplingPlotGeometry.Rows.Count > 0)
            //    {
            //        if (this.ShowGeometry) this.buttonHeaderShowGeometry.BackColor = System.Drawing.Color.Red;
            //        else this.buttonHeaderShowGeometry.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else
            //        this.buttonHeaderShowGeometry.BackColor = System.Drawing.SystemColors.Control;
            //}

            //if (Visibility.Length > 0 && Visibility.Substring(1, 1) == "1")
            //    this.buttonHeaderShowImages.BackColor = System.Drawing.Color.Red;
            //else
            //{
            //    if (this.dataSetSamplingPlot.SamplingPlotImage.Rows.Count > 0)
            //    {
            //        if (this.ShowImages) 
            //            this.buttonHeaderShowImages.BackColor = System.Drawing.Color.Red;
            //        else 
            //            this.buttonHeaderShowImages.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else
            //        this.buttonHeaderShowImages.BackColor = System.Drawing.SystemColors.Control;
            //}

            this.setImageVisibility();
        }


        /// <summary>
        /// set Image Visibility
        /// </summary>
        private void setImageVisibility()
        {
            if (this.ShowMap) //  || this.ShowImages || this.ShowGeometry
            {
                this.splitContainerData.Panel1Collapsed = false;

                //if (this.ShowGeometry || this.ShowMap)
                //{
                //    this.splitContainerMapsAndImages.Panel1Collapsed = false;
                //    if (this.ShowMap)
                //        this.splitContainerGeoImages.Panel1Collapsed = false;
                //    else this.splitContainerGeoImages.Panel1Collapsed = true;
                //    if (this.ShowGeometry)
                //        this.splitContainerGeoImages.Panel2Collapsed = false;
                //    else this.splitContainerGeoImages.Panel2Collapsed = true;
                //}
                //else this.splitContainerMapsAndImages.Panel1Collapsed = true;

                //if (this.ShowMap)
                //    this.splitContainerMapsAndImages.Panel1Collapsed = false;
                //else
                //    this.splitContainerMapsAndImages.Panel1Collapsed = true;

                //if (this.ShowImages)
                //{
                //    this.splitContainerMapsAndImages.Panel2Collapsed = false;
                //    this.splitContainerPoints.Panel2Collapsed = false;
                //}
                //else
                //{
                //    this.splitContainerMapsAndImages.Panel2Collapsed = true;
                //    this.splitContainerPoints.Panel2Collapsed = true;
                //}
            }
            else
                this.splitContainerData.Panel1Collapsed = true;
            string Visibility = "";
            if (this.ShowMap)
                Visibility += "1";
            else
                Visibility += "0";
            //if (this.ShowGeometry) Visibility += "1";
            //else Visibility += "0";
            //if (this.ShowImages) 
            //    Visibility += "1";
            //else 
            //    Visibility += "0";
            DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.ImageVisibility = Visibility;
            DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.Save();
        }

        #endregion // Init form

        #region Init query

        /// <summary>
        /// init Query
        /// </summary>
        private void initQuery()
        {
            try
            {
                this.userControlQueryList.FormatDateToYearMonthDay(false);
                if (DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.QueryConditionVisibility.Length > 0)
                    this.userControlQueryList.QueryConditionVisiblity = DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.QueryConditionVisibility;
                if (m_ConnectionOK)
                {
                    this.setSearchOptions();
                    this.setQueryDisplayColumns();
                    this.initQueryOptimizing();
                    //Markus 23.2.22 adaption for ManyOrderByColumns
                    this.userControlQueryList.ManyOrderByColumns_Allow(true, DiversityWorkbench.Settings.ModuleName, "FormDiversitySamplingPlots", true);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void initQueryOptimizing()
        {
            DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "ViewSamplingPlotListForCurrentProject";
            this.userControlQueryList.QueryMainTableLocal = "ViewSamplingPlotListForCurrentProject";
        }


        private void setSearchOptions()
        {
            this.userControlQueryList.setQueryConditions(this.SamplingPlot.QueryConditions(), DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.QueryConditionVisibility.ToString());
        }

        private void setQueryDisplayColumns()
        {
            this.userControlQueryList.QueryDisplayColumns = this.SamplingPlot.QueryDisplayColumns();
        }

        #endregion // Init query

        #endregion // Initialization

        #region Menu

        #region Init menu

        /// <summary>
        /// Initialize tool strip menu.
        /// </summary>
        private void initMenu()
        {
            this.setConnectionMenu();
            this.setDataMenu();
            this.setAdminMenu();
            this.setHelpMenu();
            this.setUpdateMenu();

            //if (m_ConnectionOK)
            //{
            //}
#if DEBUG
            this.testToolStripMenuItem.Visible = true;
#endif
        }

        #endregion // Init menu

        #region Connection

        private void setConnectionMenu()
        {
            this.databaseToolStripMenuItem.ToolTipText = "Change database. Current connection: " + this.FormFunctions.ConnectionInfo();

            bool OK = DiversityWorkbench.Settings.ConnectionString.Length > 0 && DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner");
            this.createDatabaseToolStripMenuItem.Enabled = OK;
            this.currentServerActivityToolStripMenuItem.Enabled = OK;
            OK = OK || (DiversityWorkbench.Settings.ConnectionString.Length > 0 && DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchEditor"));
            this.moduleConnectionsToolStripMenuItem.Enabled = OK;
        }

        /// <summary>
        /// Connect to database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase("DiversitySamplingPlots", this.createDatabase_Versions());
                f.setHelpProviderNameSpace(System.Windows.Forms.Application.StartupPath + "\\DiversitySamplingPlots.chm", "Login");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.setDatabase();
                    this.initQuery();
                    this.setUpdateMenu();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Quit application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Transfer previous settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transferSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Settings.TransferPreviousSettings();

                DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.Upgrade();
                DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.Reload();
                DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.Save();

                this.resetSqlAdapters();
                this.setDatabase();

                this.askOnExitToolStripMenuItem.Checked = DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.AskOnExit;
            }
            catch { }
        }

        /// <summary>
        /// Module connections.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moduleConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormConnectionAdministration f = new DiversityWorkbench.Forms.FormConnectionAdministration(System.Windows.Forms.Application.StartupPath + "\\DiversitySamplingPlots.chm");
            f.ShowDialog();
        }


        /// <summary>
        /// Timeout for database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemTimeout_Click(object sender, EventArgs e)
        {
            int Timeout = DiversityWorkbench.Settings.TimeoutDatabase;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database queries in seconds");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
                this.toolStripMenuItemTimeout.ToolTipText = DiversityWorkbench.Settings.TimeoutDatabase.ToString() + " sec.";
            }
        }

        private void currentServerActivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.ShowServerActiviy(null, null, false, "server_activity_dsp");
        }
        private void createDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Settings.ModuleName.Length == 0)
            {
                string Name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                Name = Name.Substring(0, Name.IndexOf(","));
                DiversityWorkbench.Settings.ModuleName = Name;
            }
            System.Collections.Generic.Dictionary<string, string> Versions = this.createDatabase_Versions();
            new Dictionary<string, string>();
            string Message = "";
            if (Versions.Count > 0)
            {
                if (DiversityWorkbench.Database.CreateDatabase(Versions, ref Message))
                {
                    this.setDatabase();
                    this.setUpdateMenu();
                }
                else
                {
                    if (Message.Length > 0)
                        MessageBox.Show(Message, "Failed to create database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("No database was created");
                }
            }
            else
                MessageBox.Show(Message, "Update script is missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private System.Collections.Generic.Dictionary<string, string> createDatabase_Versions()
        {
            System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
            try
            {
                System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
                System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith("DiversitySamplingPlotsUpdate_000000_to_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Versions;
        }


        #endregion // Connection

        #region Query

        private void showQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowQuery(this.showQueryToolStripMenuItem.Checked);
        }

        private void ShowQuery(bool Show)
        {
            this.splitContainerMain.Panel1Collapsed = !Show;
            this.showQueryToolStripMenuItem.Checked = Show;
        }

        #endregion // Query

        #region Data
        /// <summary>
        /// Handle visibilty of Data menu items.
        /// </summary>
        private void setDataMenu()
        {
            bool IsAdmin = false;
            try
            {
                bool Any = false;
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    bool OK = DiversityWorkbench.Settings.ConnectionString.Length > 0 && DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner");
                    this.backupDatabaseToolStripMenuItem.Enabled = OK;
                    this.replicationToolStripMenuItem.Enabled = OK;
                    IsAdmin = OK;
                    if (IsAdmin)
                        this.initReplicationMenu(IsAdmin);
                    Any = OK;

                    OK = OK || DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchEditor");
                    this.importToolStripMenuItem.Enabled = OK;
                    this.exportToolStripMenuItem.Enabled = OK;
                    this.transferToolStripMenuItem.Enabled = OK;
                    this.tableEditorToolStripMenuItem.Enabled = OK;
                    this.spreadsheetToolStripMenuItem.Enabled = OK;
                    this.scanModulesToolStripMenuItem.Enabled = OK;
                    if (OK)
                        this.setTableEditorMenu();
                    if (!Any)
                        Any = OK;
                    this.dataToolStripMenuItem.Enabled = Any;
                }
            }
            catch (Exception ex)
            {
                this.dataToolStripMenuItem.Enabled = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            //this.initReplicationMenu(IsAdmin);
        }

        #region Data events

        /// <summary>
        /// Backup database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to create a backup of the whole database?", "Backup", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string ErrorMessage = "";
                string Filename = "";
                DiversityWorkbench.Database.BackupCopy(DiversityWorkbench.Settings.DatabaseName, ref Filename, out ErrorMessage);
                if (ErrorMessage == string.Empty)
                    ErrorMessage = "Backup has been successful!";
                System.Windows.Forms.MessageBox.Show(ErrorMessage);
            }
        }

        #region Export
        private void exportPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.FormStartWizard fs = null;
            DiversityWorkbench.Export.FormExport f = null;
            try
            {
                if (this.userControlQueryList.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    return;
                }

                this.Enabled = false;

                // Show start window
                fs = new DiversityWorkbench.Import.FormStartWizard("Starting export wizard...", DiversityWorkbench.Import.FormStartWizard.Direction.Export);
                fs.Show();
                Application.DoEvents();

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityWorkbench.Export.Exporter.ResetAll();

                // getting the IDs
                System.Collections.Generic.List<int> L = new List<int>();
                foreach (int ID in this.userControlQueryList.ListOfIDs)
                {
                    L.Add(ID);
                }
                if (L.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No data found");
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }

                // getting the template tables
                DiversityWorkbench.Export.Table TSamplingPlot = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlot", "Plot", DiversityWorkbench.Export.Table.Parallelity.unique, DiversitySamplingPlots.Properties.Resources.SamplingPlot, null, null, null, "");

                // Localisation
                this.ExportWizardPlotLocalisationTables(TSamplingPlot);
                // Site properties
                this.ExportWizardPlotPropertyTables(TSamplingPlot);


                // Specimen Image
                System.Collections.Generic.Dictionary<string, string> TSamplingPlotResource = new Dictionary<string, string>();
                TSamplingPlotResource.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table TSpecimenImage = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotResource", "Resource", DiversityWorkbench.Export.Table.Parallelity.parallel, DiversitySamplingPlots.Properties.Resources.Icones, TSamplingPlot, null, TSamplingPlotResource, "");

                // Table list
                System.Collections.Generic.List<DiversityWorkbench.Export.Table> TT = new List<DiversityWorkbench.Export.Table>();
                TT.Add(TSamplingPlot);

                f = new DiversityWorkbench.Export.FormExport(TSamplingPlot, L, TT, this.helpProvider.HelpNamespace);
                f.setKeyword("exportwizard_dsp");
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                f.StartPosition = FormStartPosition.CenterParent;

                fs.Close();
                f.ShowDialog();

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.Enabled = true;
                if (fs != null)
                    fs.Close();
                if (f != null)
                    f.Close();
            }
        }

        private void ExportWizardPlotLocalisationTables(DiversityWorkbench.Export.Table Parent)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, string> TWGS84M = new Dictionary<string, string>();
                TWGS84M.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table TWGS84 = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", "WGS84", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Localisation, Parent, null, TWGS84M, "LocalisationSystemID = 8");

                System.Collections.Generic.Dictionary<string, string> TaltM = new Dictionary<string, string>();
                TaltM.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table TAltitude = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", "Altitude", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Altitude, Parent, null, TaltM, "LocalisationSystemID = 4");

                string Caption = "Gazetteer";
                //string Entity = "LocalisationSystem.LocalisationSystemID.7";
                //System.Collections.Generic.Dictionary<string, string> E = DiversityWorkbench.Entity.EntityInformation(Entity, "General", DiversityWorkbench.Settings.Language);
                //if (E[DiversityWorkbench.Entity.EntityInformationField.DisplayTextOK.ToString()] == "True")
                //    Caption = E["DisplayText"];

                System.Collections.Generic.Dictionary<string, string> TGazetteerM = new Dictionary<string, string>();
                TGazetteerM.Add("ResponsibleAgentURI", "DiversityAgents");
                TGazetteerM.Add("Location2", "DiversityGazetteer");
                DiversityWorkbench.Export.Table TGazetteer = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", Caption, DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Place, Parent, null, TGazetteerM, "LocalisationSystemID = 7");

                //Entity = "LocalisationSystem.LocalisationSystemID.18";
                //System.Collections.Generic.Dictionary<string, string> E18 = DiversityWorkbench.Entity.EntityInformation(Entity, "General", DiversityWorkbench.Settings.Language);
                //if (E18[DiversityWorkbench.Entity.EntityInformationField.DisplayTextOK.ToString()] == "True")
                //    Caption = E18["DisplayText"];

                Caption = "Gazetteer 2";
                System.Collections.Generic.Dictionary<string, string> TGazetteerM2 = new Dictionary<string, string>();
                TGazetteerM2.Add("ResponsibleAgentURI", "DiversityAgents");
                TGazetteerM2.Add("Location2", "DiversityGazetteer");
                DiversityWorkbench.Export.Table TGazetteer2 = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", Caption, DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Place, Parent, null, TGazetteerM2, "LocalisationSystemID = 18");

                //Entity = "LocalisationSystem.LocalisationSystemID.19";
                //System.Collections.Generic.Dictionary<string, string> E19 = DiversityWorkbench.Entity.EntityInformation(Entity, "General", DiversityWorkbench.Settings.Language);
                //if (E19[DiversityWorkbench.Entity.EntityInformationField.DisplayTextOK.ToString()] == "True")
                //    Caption = E19["DisplayText"];

                Caption = "Gazetteer 3";
                System.Collections.Generic.Dictionary<string, string> TGazetteerM3 = new Dictionary<string, string>();
                TGazetteerM3.Add("ResponsibleAgentURI", "DiversityAgents");
                TGazetteerM3.Add("Location2", "DiversityGazetteer");
                DiversityWorkbench.Export.Table TGazetteer3 = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", Caption, DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Place, Parent, null, TGazetteerM3, "LocalisationSystemID = 19");

                //Entity = "LocalisationSystem.LocalisationSystemID.20";
                //System.Collections.Generic.Dictionary<string, string> E20 = DiversityWorkbench.Entity.EntityInformation(Entity, "General", DiversityWorkbench.Settings.Language);
                //if (E20[DiversityWorkbench.Entity.EntityInformationField.DisplayTextOK.ToString()] == "True")
                //    Caption = E20["DisplayText"];

                Caption = "Gazetteer 4";
                System.Collections.Generic.Dictionary<string, string> TGazetteerM4 = new Dictionary<string, string>();
                TGazetteerM4.Add("ResponsibleAgentURI", "DiversityAgents");
                TGazetteerM4.Add("Location2", "DiversityGazetteer");
                DiversityWorkbench.Export.Table TGazetteer4 = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", Caption, DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Place, Parent, null, TGazetteerM4, "LocalisationSystemID = 20");

                //Entity = "LocalisationSystem.LocalisationSystemID.21";
                //System.Collections.Generic.Dictionary<string, string> E21 = DiversityWorkbench.Entity.EntityInformation(Entity, "General", DiversityWorkbench.Settings.Language);
                //if (E21[DiversityWorkbench.Entity.EntityInformationField.DisplayTextOK.ToString()] == "True")
                //    Caption = E21["DisplayText"];

                Caption = "Gazetteer 5";
                System.Collections.Generic.Dictionary<string, string> TGazetteerM5 = new Dictionary<string, string>();
                TGazetteerM5.Add("ResponsibleAgentURI", "DiversityAgents");
                TGazetteerM5.Add("Location2", "DiversityGazetteer");
                DiversityWorkbench.Export.Table TGazetteer5 = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", Caption, DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Place, Parent, null, TGazetteerM5, "LocalisationSystemID = 21");

                System.Collections.Generic.Dictionary<string, string> TMTBM = new Dictionary<string, string>();
                TMTBM.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table TMTB = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotLocalisation", "MTB", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.MTB, Parent, null, TMTBM, "LocalisationSystemID = 3");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /*
PropertyID	PropertyParentID	PropertyName
1	NULL	European Nature Information System (EUNIS)
2	NULL	EUNIS 2012 (European Nature Information System)
10	NULL	Geographic regions
20	NULL	Chronostratigraphy
30	NULL	Lithostratigraphy
40	NULL	Lebensraumtypen (LfU)
41	NULL	Biotoptypen (BFN)
50	NULL	Pflanzengesellschaften
*/
        private void ExportWizardPlotPropertyTables(DiversityWorkbench.Export.Table Parent)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, string> T_Chronostratigraphy_M = new Dictionary<string, string>();
                T_Chronostratigraphy_M.Add("PropertyURI", "DiversityScientificTerms");
                T_Chronostratigraphy_M.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table T_Chronostratigraphy = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotProperty", "Chronostratigraphy", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Geology, Parent, null, T_Chronostratigraphy_M, "PropertyID = 20");

                System.Collections.Generic.Dictionary<string, string> T_Lithostratigraphy_M = new Dictionary<string, string>();
                T_Lithostratigraphy_M.Add("PropertyURI", "DiversityScientificTerms");
                T_Lithostratigraphy_M.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table T_Lithostratigraphy = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotProperty", "Lithostratigraphy", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Geology, Parent, null, T_Lithostratigraphy_M, "PropertyID = 30");

                //System.Collections.Generic.Dictionary<string, string> T_Biostratigraphy_M = new Dictionary<string, string>();
                //T_Biostratigraphy_M.Add("PropertyURI", "DiversityScientificTerms");
                //T_Biostratigraphy_M.Add("ResponsibleAgentURI", "DiversityAgents");
                //DiversityWorkbench.Export.Table T_Biostratigraphy = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotProperty", "Biostratigraphy", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Habitat, Parent, null, T_Biostratigraphy_M, "PropertyID = 60");

                System.Collections.Generic.Dictionary<string, string> T_Lebensraum_M = new Dictionary<string, string>();
                T_Lebensraum_M.Add("PropertyURI", "DiversityScientificTerms");
                T_Lebensraum_M.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table T_Lebensraum = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotProperty", "Lebensraum", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Habitat, Parent, null, T_Lebensraum_M, "PropertyID = 40");

                System.Collections.Generic.Dictionary<string, string> T_BiotoptypenBfN_M = new Dictionary<string, string>();
                T_BiotoptypenBfN_M.Add("PropertyURI", "DiversityScientificTerms");
                T_BiotoptypenBfN_M.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table T_BiotoptypenBfN = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotProperty", "Biotoptypen", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Habitat, Parent, null, T_BiotoptypenBfN_M, "PropertyID = 41");

                System.Collections.Generic.Dictionary<string, string> T_Pflanzengesellschaft_M = new Dictionary<string, string>();
                T_Pflanzengesellschaft_M.Add("PropertyURI", "DiversityScientificTerms");
                T_Pflanzengesellschaft_M.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table T_Pflanzengesellschaft = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotProperty", "Pflanzengesellschaft", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Habitat, Parent, null, T_Lebensraum_M, "PropertyID = 50");

                System.Collections.Generic.Dictionary<string, string> T_Eunis_M = new Dictionary<string, string>();
                T_Eunis_M.Add("PropertyURI", "DiversityScientificTerms");
                T_Eunis_M.Add("ResponsibleAgentURI", "DiversityAgents");
                DiversityWorkbench.Export.Table T_Eunis = DiversityWorkbench.Export.Exporter.AddTemplateTable("SamplingPlotProperty", "EUNIS", DiversityWorkbench.Export.Table.Parallelity.restricted, DiversitySamplingPlots.Properties.Resources.Habitat, Parent, null, T_Eunis_M, "PropertyID = 1");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region Transfer

        private void toProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");

            if (this.userControlQueryList.ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            string Message = "";
            int i = 0;
            int n = 0;
            System.Data.DataTable dtProjects = DiversitySamplingPlots.LookupTable.DtProject;
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtProjects, "Project", "ProjectID", "Project", "Please select the additional project for the selected plots");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int CurrentProjectID = this.userControlQueryList.ProjectID;
                string CurrentProject = DiversitySamplingPlots.LookupTable.ProjectName(CurrentProjectID);
                Message = "Do you want to remove the plots from the current project " + CurrentProject;
                bool RemoveFromCurrentProject = (System.Windows.Forms.MessageBox.Show(Message, "Remove from " + CurrentProject, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                int ProjectID = int.Parse(f.SelectedValue);
                string Project = f.SelectedString;
                try
                {
                    Message = "";
                    foreach (int ID in this.userControlQueryList.ListOfIDs)
                    {
                        string SQL = "";
                        if (RemoveFromCurrentProject)
                            SQL = "UPDATE S SET ProjectID = " + ProjectID.ToString() +
                            " FROM SamplingProject S " +
                            "WHERE S.PlotID = " + ID.ToString() + " AND S.ProjectID = " + CurrentProjectID.ToString();
                        else
                            SQL = "INSERT INTO SamplingProject " +
                            "(PlotID, ProjectID) " +
                            "SELECT    PlotID, " + ProjectID.ToString() + " AS ProjectID FROM SamplingPlot " +
                            "WHERE  PlotID = " + ID.ToString() + " AND   (NOT EXISTS " +
                            "(SELECT     PlotID " +
                            "FROM          SamplingProject AS SamplingProject_1 " +
                            "WHERE      (PlotID = " + ID.ToString() + ") AND (ProjectID = " + ProjectID.ToString() + ")))";
                        string Error = "";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                        if (RemoveFromCurrentProject && Error.Length > 0)
                        {
                            SQL = "BEGIN TRANSACTION " +
                                "BEGIN TRY " +
                                "DELETE S " +
                                " FROM SamplingProject S " +
                                "WHERE S.PlotID = " + ID.ToString() + " AND S.ProjectID = " + CurrentProjectID.ToString() +
                                " INSERT INTO SamplingProject " +
                                "(PlotID, ProjectID) " +
                                "SELECT    PlotID, " + ProjectID.ToString() + " AS ProjectID FROM SamplingPlot " +
                                "WHERE  PlotID = " + ID.ToString() + " AND   (NOT EXISTS " +
                                "(SELECT     PlotID " +
                                "FROM          SamplingProject AS SamplingProject_1 " +
                                "WHERE      (PlotID = " + ID.ToString() + ") AND (ProjectID = " + ProjectID.ToString() + "))); " +
                                "COMMIT TRANSACTION " +
                                "END TRY " +
                                "BEGIN CATCH " +
                                "ROLLBACK TRANSACTION " +
                                "END CATCH ";
                            Error = "";
                            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                        }
                        if (Error.Length > 0)
                        {
                            Message += Error + "\r\n";
                            n++;
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
                string finalMessage = i.ToString() + " plots are now placed project\r\n" + Project;
                if (Message.Length > 0 && n > 0)
                    finalMessage += "\r\n\r\nThe position of " + n.ToString() + " plots has not been changed:\r\n" + Message;
                System.Windows.Forms.MessageBox.Show(finalMessage);
            }
        }

        private void toSamplingPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;

            if (this.userControlQueryList.ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            System.Data.DataTable dtPlot = new DataTable();
            string SQL = "SELECT H.PlotID, H.DisplayText " +
                "FROM ViewSamplingPlotHierarchy AS H INNER JOIN " +
                "SamplingProject AS P ON H.PlotID = P.PlotID INNER JOIN " +
                "ProjectList ON P.ProjectID = ProjectList.ProjectID " +
                "ORDER BY H.DisplayText";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtPlot);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtPlot, "DisplayText", "PlotID", "Superior plot", "Please select the sampling plot that should contain the selected plots");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                int PlotID = int.Parse(f.SelectedValue);
                string Plot = f.SelectedString;
                int i = 0;
                try
                {
                    foreach (int ID in this.userControlQueryList.ListOfIDs)
                    {
                        SQL = "UPDATE S SET PartOfPlotID = " + PlotID.ToString() +
                            " FROM SamplingPlot S " +
                            "WHERE  PlotID = " + ID.ToString() + " AND   (NOT EXISTS " +
                            "(SELECT     PlotID " +
                            "FROM          SamplingProject AS SamplingProject_1 " +
                            "WHERE      (PlotID = " + ID.ToString() + ") AND (ProjectID = " + PlotID.ToString() + ")))";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
                System.Windows.Forms.MessageBox.Show(i.ToString() + " plots are now placed the sampling plot\r\n" + Plot);
            }

        }

        #endregion

        #region Scan modules

        private void diversityCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, this.diversityCollectionToolStripMenuItem.Checked);
            this.initBackLinksCollection();
        }

        private bool _ShowSpecimenImages = false;
        private void specimenImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ShowSpecimenImages = !_ShowSpecimenImages;
            this.initBackLinksCollection();
        }

        #endregion

        #region Table editors

        private void setTableEditorMenu()
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    bool Any = false;
                    bool OK;
                    string TableName = "";

                    // SamplingPlotLocalisation
                    TableName = "SamplingPlotLocalisation";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.localisationTableToolStripMenuItem.Enabled = OK;
                    if (OK) Any = true;

                    // SamplingPlotProperty
                    TableName = "SamplingPlotProperty";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.siteTableToolStripMenuItem.Enabled = OK;
                    if (OK) Any = true;

                    // SamplingPlot
                    TableName = "SamplingPlot";
                    OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
                    this.plotSheetToolStripMenuItem.Enabled = OK;
                    if (OK) Any = true;


                    this.tableEditorToolStripMenuItem.Enabled = Any;
                }
                else
                {
                    this.tableEditorToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                this.tableEditorToolStripMenuItem.Enabled = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void localisationTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("SamplingPlotLocalisation", DiversitySamplingPlots.Properties.Resources.Localisation);
        }

        private void siteTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("SamplingPlotProperty", DiversitySamplingPlots.Properties.Resources.Habitat);
        }

        private void plotTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TableEditing("SamplingPlot", DiversitySamplingPlots.Properties.Resources.SamplingPlot);
        }

        private void TableEditing(string Table, System.Drawing.Image Icon, System.Collections.Generic.List<string> BlockedColumns = null)
        {
            try
            {
                if (this.userControlQueryList.OptimizingIsUsed())
                {
                    this.TableEditingForOptimizing(Table, Icon, BlockedColumns);
                }
                else
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    if (this.userControlQueryList.ListOfIDs.Count == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Nothing selected");
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                    System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
                    DiversityWorkbench.Data.Table T = new DiversityWorkbench.Data.Table(Table);
                    foreach (string PK in T.PrimaryKeyColumnList)
                        ReadOnlyColumns.Add(PK);
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> DC in T.Columns)
                    {
                        if (DC.Key.StartsWith("Log") && !ReadOnlyColumns.Contains(DC.Key))
                            ReadOnlyColumns.Add(DC.Key);
                        else if (DC.Value.IsIdentity && !ReadOnlyColumns.Contains(DC.Key))
                            ReadOnlyColumns.Add(DC.Key);
                        else if (DC.Key.EndsWith("ID") && !ReadOnlyColumns.Contains(DC.Key))
                            ReadOnlyColumns.Add(DC.Key);
                    }
                    if (BlockedColumns != null)
                    {
                        foreach (string C in BlockedColumns)
                        {
                            if (!ReadOnlyColumns.Contains(C))
                                ReadOnlyColumns.Add(C);
                        }
                    }

                    string IDs = "";
                    foreach (int i in this.userControlQueryList.ListOfIDs)
                    {
                        if (IDs.Length > 0) IDs += ",";
                        IDs += i.ToString();
                    }
                    string SQL = "SELECT * FROM " + Table + " T WHERE T.PlotID IN (" + IDs + ")";

                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, ReadOnlyColumns, DiversityWorkbench.Settings.TimeoutDatabase, null, Table);
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.Width = this.Width - 10;
                    f.Height = this.Height - 10;
                    f.setHelpProvider(this.helpProvider.HelpNamespace, "tableeditors_dsp");
                    bool SetTimeout = false;
                    try
                    {
                        f.ShowDialog();
                        this.setSamplingPlot(this.PlotID);
                    }
                    catch (System.Exception ex)
                    {
                        SetTimeout = true;
                    }
                    //if (SetTimeout)
                    //{
                    //    int? Timeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout;
                    //    DiversityWorkbench.FormGetInteger ftimeout = new DiversityWorkbench.FormGetInteger(Timeout, "Set timeout", "A timeout occured. Please set the seconds you are prepared to wait");
                    //    ftimeout.ShowDialog();
                    //    if (ftimeout.DialogResult == System.Windows.Forms.DialogResult.OK && ftimeout.Integer != null)
                    //    {
                    //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout = (int)ftimeout.Integer;
                    //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                    //    }
                    //}
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "TableEditing(string Table, System.Drawing.Image Icon)");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void TableEditingForOptimizing(string Table, System.Drawing.Image Icon, System.Collections.Generic.List<string> BlockedColumns = null)
        {
            if (this.userControlQueryList.OptimizingIsUsed())
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this.userControlQueryList.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }
                System.Collections.Generic.List<string> ReadOnlyColumns = this.TableEditingReadOnlyColumns(Table);
                if (BlockedColumns != null)
                {
                    foreach (string C in BlockedColumns)
                        ReadOnlyColumns.Add(C);
                }

                string SQL = "SELECT T.* FROM [" + Table + "] T WHERE T.PlotID IN (SELECT ID FROM #ID)";
                if (this.userControlQueryList.ListOfBlockedIDs.Count > 0)
                {
                    string IDs = "";
                    foreach (int i in this.userControlQueryList.ListOfBlockedIDs)
                    {
                        if (IDs.Length > 0)
                            IDs += ", ";
                        IDs += i.ToString();
                    }
                    SQL += " AND T.PlotID NOT IN (" + IDs + ")";
                }
                // Check if Table #ID is filled
                bool TempTableIDfilled = true;
                string SqlCheck = "SELECT COUNT(*) FROM #ID ";
                Microsoft.Data.SqlClient.SqlDataAdapter adCheck = this.userControlQueryList.DataAdapterForTempIDs(SqlCheck);
                System.Data.DataTable dtCheck = new DataTable();
                adCheck.Fill(dtCheck);
                if (dtCheck.Rows.Count == 0)
                {

                }
                else if (dtCheck.Rows.Count == 1)
                {
                    string Result = dtCheck.Rows[0][0].ToString();
                    if (Result == "0")
                    {
                        if (this.userControlQueryList.ListOfIDs.Count > 0)
                        {

                        }
                    }
                }

                Microsoft.Data.SqlClient.SqlDataAdapter ad = this.userControlQueryList.DataAdapterForTempIDs(SQL);
                DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, Table, "T", ReadOnlyColumns, this.TableEditingLookupValues(Table));
                f.StartPosition = FormStartPosition.CenterParent;
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                f.setHelpProvider(this.helpProvider.HelpNamespace, "tableeditors_dsp");
                bool SetTimeout = false;
                try
                {
                    f.ShowDialog();
                    this.setSamplingPlot(this.PlotID);
                }
                catch (System.Exception ex)
                {
                    SetTimeout = true;
                }
                //if (SetTimeout)
                //{
                //    int? Timeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout;
                //    DiversityWorkbench.FormGetInteger ftimeout = new DiversityWorkbench.FormGetInteger(Timeout, "Set timeout", "A timeout occured. Please set the seconds you are prepared to wait");
                //    ftimeout.ShowDialog();
                //    if (ftimeout.DialogResult == System.Windows.Forms.DialogResult.OK && ftimeout.Integer != null)
                //    {
                //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout = (int)ftimeout.Integer;
                //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                //    }
                //}
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private System.Collections.Generic.List<string> TableEditingReadOnlyColumns(string Table)
        {
            System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
            string SqlCol = "SELECT C.COLUMN_NAME, C.DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + Table + "'";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter adCol = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCol, DiversityWorkbench.Settings.ConnectionString);
            adCol.Fill(dt);

            System.Collections.Generic.List<string> LookUpColumns = new List<string>();
            LookUpColumns.Add("PlotID");

            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (LookUpColumns.Contains(R[0].ToString()))
                    continue;
                if (R[0].ToString().EndsWith("ID")
                    || R[0].ToString().StartsWith("Log")
                    || R[1].ToString() == "uniqueidentifier"
                    || R[1].ToString() == "geography"
                    || R[1].ToString() == "geometry")
                    ReadOnlyColumns.Add(R[0].ToString());
            }
            return ReadOnlyColumns;
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues> TableEditingLookupValues(string TableName)
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues> DD = new Dictionary<string, DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues>();
            DiversityWorkbench.Data.Table T = new DiversityWorkbench.Data.Table(TableName, DiversityWorkbench.Settings.ConnectionString);
            T.FindColumnsWithForeignRelations();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> C in T.Columns)
            {
                if (C.Value.ForeignRelations.Count == 1
                    && C.Value.ForeignRelationColumn.Length > 0
                    && C.Value.ForeignRelationTable.Length > 0
                    && C.Value.ForeignRelationTable.ToLower().EndsWith("_enum"))
                {
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT NULL AS Code, NULL AS DisplayText UNION SELECT Code, DisplayText FROM " + C.Value.ForeignRelationTable + " WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues CBV = new DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues(dt, "DisplayText", "Code");
                    DD.Add(C.Key, CBV);
                }
                else if (C.Value.ForeignRelations.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in C.Value.ForeignRelations)
                    {
                        if (!DD.ContainsKey(C.Key))
                        {
                            System.Data.DataTable dt = new DataTable();
                            string SQL = "";
                            string DisplayColumn = "DisplayText";
                            string ColumnHeader = "";
                            string Union = "SELECT NULL AS " + KV.Value + ", NULL AS DisplayText UNION ";
                            switch (KV.Value)
                            {
                                case "ProjectID":
                                    SQL = Union + "SELECT ProjectID, Project FROM ProjectProxy ORDER BY DisplayText";
                                    ColumnHeader = "Project";
                                    break;
                                case "PropertyID":
                                    SQL = Union + "SELECT PropertyID, DisplayText FROM Property ORDER BY DisplayText";
                                    ColumnHeader = "Property";
                                    break;
                                case "LocalisationSystemID":
                                    SQL = Union + "SELECT LocalisationSystemID, DisplayText FROM LocalisationSystem ORDER BY DisplayText";
                                    ColumnHeader = "LocalisationSystem";
                                    break;
                            }
                            if (SQL.Length > 0)
                            {
                                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                ad.Fill(dt);
                                DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues CBV = new DiversityWorkbench.Forms.FormTableEditor.ComboBoxValues(dt, DisplayColumn, KV.Value, ColumnHeader);
                                DD.Add(C.Key, CBV);
                            }
                        }
                    }
                }
            }
            return DD;
        }

        #endregion

        #region Spreadsheets

        private void setSpreadsheetMenu()
        {
#if !DEBUG
#endif
        }

        private void plotSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartSpreadSheet(Spreadsheet.Target.SheetTarget.Plot);
        }


        private void StartSpreadSheet(Spreadsheet.Target.SheetTarget Target)
        {
            DiversityWorkbench.Import.FormStartWizard fs = new DiversityWorkbench.Import.FormStartWizard("Starting spreadsheet...", "", DiversitySamplingPlots.Properties.Resources.Spreadsheet);
            try
            {
                this.Enabled = false;
                string CurrentConnection = DiversityWorkbench.Settings.ConnectionString;
                int X = this.Location.X + (int)(this.Width / 2) - (int)(fs.Width / 2);
                int Y = this.Location.Y + (int)(this.Height / 2) - (int)(fs.Height / 2);
                System.Drawing.Point P = new Point(X, Y);
                fs.Location = P;
                fs.StartPosition = FormStartPosition.Manual;//.CenterScreen;

                fs.Show();
                fs.ShowCurrentStep("Getting definitions of the tables");
                Application.DoEvents();

                DiversityWorkbench.Spreadsheet.Sheet Sheet = null;
                switch (Target)
                {
                    case Spreadsheet.Target.SheetTarget.Plot:
                        Sheet = Spreadsheet.Target.TargetSheet(Spreadsheet.Target.SheetTarget.Plot);
                        break;
                }
                if (Sheet != null)
                {
                    DiversitySamplingPlots.Forms.FormDiversitySamplingPlots fMain = new FormDiversitySamplingPlots();
                    Sheet.setInterfaceMainForm(fMain);

                    fs.ShowCurrentStep("Reading settings");
                    Application.DoEvents();
                    DiversityWorkbench.Spreadsheet.FormSpreadsheet f = new DiversityWorkbench.Spreadsheet.FormSpreadsheet(Sheet, this.helpProvider.HelpNamespace);
                    f.setKeyword("spreadsheet_dsp");

                    // setting the image column if present
                    switch (Sheet.Target())
                    {
                        case "Plots":
                            if (Sheet.DataTables().ContainsKey("E6_Im"))
                                f.setImageColumn(Sheet.DataTables()["E6_Im"].DataColumns()["URI"]);
                            break;
                    }
                    if (Spreadsheet.Target.StarterTarget() != null)
                        f.IsStarter = true;
                    else
                    {
                        f.IsStarter = false;
                        f.setSize(this.Width, this.Height);
                        f.setLocation(this.Location.X, this.Location.Y);
                        f.StartPosition = FormStartPosition.Manual;
                    }
                    fs.Close();
                    if (!f.IsDisposed)
                    {
                        f.ShowDialog();
                        if (f.IsStarter)
                        {
                            Spreadsheet.Target.SetStart(Target);
                        }
                        else Spreadsheet.Target.SetStart(null);
                        if (DiversityWorkbench.Settings.ConnectionString != CurrentConnection)
                        {
                            System.Windows.Forms.MessageBox.Show("Connection changed to new database");
                            this.setDatabase();
                        }
                        fMain.Close();
                        fMain.Dispose();
                    }
                }
                else
                {
                    fs.Close();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                fs.Close();
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void CreateSpreadSheetMainForm()
        {
            DiversitySamplingPlots.Forms.FormDiversitySamplingPlots fMain = new FormDiversitySamplingPlots();
            DiversityWorkbench.Spreadsheet.Sheet.setStaticInterfaceMainForm(fMain);
        }

        public void ShowInMainForm(int ID, bool ShowDialogPanel, bool ShowMenu)
        {
            try
            {
                try { DiversityWorkbench.Settings.ModuleName = "DiversityCollection"; }
                catch { }
                bool OK = true;
                if (DiversityWorkbench.Settings.DatabaseServer != "127.0.0.1")
                    OK = this.setDatabase();
                else
                    this.splitContainerData.Visible = false;
                this.initForm();
                if (ShowMenu)
                    this.initQuery();
                this.setQueryControlEvents();
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                //if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection > -1)
                //    this.UserControl_Collection.DefaultCollection = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection;
                //this.setLanguage();
                //this.timerScanning.Interval = DiversityCollection.Forms.FormTransactionSettings.Default.TimerIntervall;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            try
            {
                if (this.Parent != null)
                {
                    this.Width = this.Parent.Width - 20;
                    this.Height = this.Parent.Height - 20;
                }
                this.StartPosition = FormStartPosition.CenterParent;
                this.splitContainerMain.Panel1Collapsed = true;
                this.setSamplingPlot(ID);
                if (this.textBoxHeaderPlotIdentifier.Text.Length == 0)
                    this.Text = "[ID: " + this.PlotID.ToString() + "]";
                else
                    this.Text = this.textBoxHeaderPlotIdentifier.Text;
                this.Text = this.Text.Replace("\r\n", " - ");
                this.userControlDialogPanel.Visible = ShowDialogPanel;
                this.menuStripMain.Visible = ShowMenu;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //this._ViewMode = ViewMode.SpreadsheetMode;
            this.Show();
        }

        #endregion

        #region Replication events

        /// <summary>
        /// Replication: Add publisher.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPublisherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ServerConnection SC = new DiversityWorkbench.ServerConnection();
            SC.ModuleName = "DiversitySamplingPlots";
            SC.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
            SC.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
            SC.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
            SC.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
            SC.DatabasePassword = DiversityWorkbench.Settings.Password;
            DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(SC);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityWorkbench.ServerConnection ScPulisher = f.ServerConnection;
                if (ScPulisher.DatabaseName == DiversityWorkbench.Settings.DatabaseName
                    && ScPulisher.DatabaseServer == DiversityWorkbench.Settings.DatabaseServer)
                {
                    System.Windows.Forms.MessageBox.Show("You can not add the current database as a publisher to itself");
                    return;
                }
                string SQL = "INSERT INTO ReplicationPublisher " +
                    "(DatabaseName, Server, Port) " +
                    "VALUES ('" + ScPulisher.DatabaseName + "', '" + ScPulisher.DatabaseServer + "', " + ScPulisher.DatabaseServerPort.ToString() + ")";
                try
                {
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        this.initReplicationMenu(true);
                    else
                        System.Windows.Forms.MessageBox.Show("Insert failed");
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        /// <summary>
        /// Replication: Download.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;
            System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
            System.Data.DataRow R = (System.Data.DataRow)T.Tag;
            try
            {
                DiversitySamplingPlots.Forms.FormReplication f = new DiversitySamplingPlots.Forms.FormReplication(
                    DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Download,
                    null, this.ReplicationPublisherConnectionString(R));
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Replication: Upload.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.userControlQueryList.ProjectIsSelected)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }
            System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
            System.Data.DataRow R = (System.Data.DataRow)T.Tag;
            if (!ProjectForUploadIsAvailableInPublisher(this.ReplicationPublisherConnectionString(R)))
            {
                System.Windows.Forms.MessageBox.Show("Either you have no access to the publishing database or the current project does not exist there");
                return;
            }
            try
            {
                DiversitySamplingPlots.Forms.FormReplication f = new DiversitySamplingPlots.Forms.FormReplication(
                    DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Upload,
                    this.userControlQueryList.ProjectID, this.ReplicationPublisherConnectionString(R));
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Replication: Clean database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cleanDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiversitySamplingPlots.Forms.FormReplication f = new DiversitySamplingPlots.Forms.FormReplication(
                    DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Clean, null, "");
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Replication: Merge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.userControlQueryList.ProjectIsSelected)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }
            System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
            System.Data.DataRow R = (System.Data.DataRow)T.Tag;
            try
            {
                DiversitySamplingPlots.Forms.FormReplication f = new DiversitySamplingPlots.Forms.FormReplication(
                    DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Merge,
                    this.userControlQueryList.ProjectID, this.ReplicationPublisherConnectionString(R));
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Replication: Delete publisher.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteReplicationPublisherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ToolStripMenuItem T = (System.Windows.Forms.ToolStripMenuItem)sender;
                System.Data.DataRow R = (System.Data.DataRow)T.Tag;
                string SQL = "DELETE R " +
                    "FROM  ReplicationPublisher AS R " +
                    "WHERE (R.DatabaseName = '" + R["DatabaseName"].ToString() + "') AND (R.Server = '" + R["Server"].ToString() + "')";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this.initReplicationMenu(true);
                else
                    System.Windows.Forms.MessageBox.Show("Removal of replication publisher failed");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        // Replication methods and properties

        /// <summary>
        /// For upload the project must be availble in the publishing database
        /// </summary>
        private bool ProjectForUploadIsAvailableInPublisher(string ReplicationPublisherConnectionString)
        {
            bool OK = false;
            try
            {
                string SqlProject = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + this.userControlQueryList.ProjectID.ToString();
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ReplicationPublisherConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlProject, con);
                con.Open();
                string Project = C.ExecuteScalar().ToString();
                string SQL = "SELECT COUNT(*) " +
                    "FROM ProjectProxy AS P INNER JOIN " +
                    "ProjectUser AS U ON P.ProjectID = U.ProjectID " +
                    "WHERE U.LoginName = USER_NAME() AND P.Project = N'" + Project + "' AND P.ProjectID = " + this.userControlQueryList.ProjectID.ToString();
                C.CommandText = SQL;
                string Result = C.ExecuteScalar().ToString();
                if (Result != "1")
                {
                    OK = false;
                    string Message = "The project " + Project + " (ID = " + this.userControlQueryList.ProjectID.ToString()
                        + ") is either missing in the publisher database, has a different name or you have no access.";
                    System.Windows.Forms.MessageBox.Show(Message);
                }
                else OK = true;
                con.Close();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private System.Collections.Generic.Dictionary<string, string> _ReplicationPublisherConnections;

        /// <summary>
        /// Replication: Retuns the connections.
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ReplicationPublisherConnections
        {
            get
            {
                if (this._ReplicationPublisherConnections == null)
                {
                    this._ReplicationPublisherConnections = new Dictionary<string, string>();
                    string SQL = "SELECT   'Data Source=' + Server  + ',' + CAST( Port as varchar) + ';Initial Catalog=' + DatabaseName " +
                        "FROM         ReplicationPublisher";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                        this._ReplicationPublisherConnections.Add(R[0].ToString(), "");
                }
                return _ReplicationPublisherConnections;
            }
            //set { _ReplicationPublisherConnections = value; }
        }

        /// <summary>
        /// Replication: Returns the connection string.
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>
        public string ReplicationPublisherConnectionString(System.Data.DataRow R)
        {
            if (R["DatabaseName"].Equals(System.DBNull.Value)
                || R["DatabaseName"].ToString().Length == 0
                || R["Server"].Equals(System.DBNull.Value)
                || R["Server"].ToString().Length == 0
                || R["Port"].Equals(System.DBNull.Value)
                || R["Port"].ToString().Length == 0)
                return "";

            string ReplicationPublisherConnection = "Data Source=" + R["Server"].ToString() + "," + R["Port"].ToString()
                + ";Initial Catalog=" + R["DatabaseName"].ToString();
            if (!this.ReplicationPublisherConnections.ContainsKey(ReplicationPublisherConnection))
                this.ReplicationPublisherConnections.Add(ReplicationPublisherConnection, "");
            if (this.ReplicationPublisherConnections[ReplicationPublisherConnection].Length > 0)
                return this.ReplicationPublisherConnections[ReplicationPublisherConnection];
            // try the current login data to connect to the database
            string ConnectionLogin = "";
            if (DiversityWorkbench.Settings.IsTrustedConnection)
                ConnectionLogin = ";Integrated Security=True";
            else
                ConnectionLogin = ";User ID=" + DiversityWorkbench.Settings.DatabaseUser + ";Password=" + DiversityWorkbench.Settings.Password;
            try
            {
                string SQL = "SELECT dbo.BaseURL()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ReplicationPublisherConnection + ConnectionLogin);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string BaseURL = C.ExecuteScalar().ToString();
                con.Close();
                con.Dispose();
                if (BaseURL.Length > 0)
                    this.ReplicationPublisherConnections[ReplicationPublisherConnection] = ReplicationPublisherConnection + ConnectionLogin;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            if (this.ReplicationPublisherConnections[ReplicationPublisherConnection].Length == 0)
            {
                DiversityWorkbench.ServerConnection SC = new DiversityWorkbench.ServerConnection();
                SC.DatabaseName = R["DatabaseName"].ToString();
                SC.DatabaseServer = R["Server"].ToString();
                SC.DatabaseServerPort = int.Parse(R["Port"].ToString());
                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(true, SC);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ServerConnection.ConnectionString.Length > 0)
                        this.ReplicationPublisherConnections[ReplicationPublisherConnection] = f.ServerConnection.ConnectionString;
                }
            }
            return this.ReplicationPublisherConnections[ReplicationPublisherConnection];
        }

        #endregion // Replication events

        #region Import wizard events

        // Importwizard Tables
        private enum ImportWizardMainTables { SamplingPlot };
        private enum ImportWizardPlotTables { SamplingProject, SamplingPlotLocalisation, SamplingPlotProperty, SamplingPlotResource };

        /// <summary>
        /// Import wizard start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void samplingPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.ResetTemplate();
            StartWizard("SamplingPlot", ImportStepsSamplingPlot());
        }

        // Import wizard methods

        private void StartWizard(string Target, System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportSteps)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityWorkbench.Import.Import.ResetTemplate();
                DiversityWorkbench.Import.FormWizard f = new DiversityWorkbench.Import.FormWizard(Target, DiversitySamplingPlots.Properties.Settings.Default.DatabaseVersion, ImportSteps, this.helpProvider.HelpNamespace);
                f.setKeyword("importwizard_dsp");
                f.Height = this.Height - 10;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("ImportWizard - ERROR: " + ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> ImportStepsSamplingPlot()
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> IS = new Dictionary<string, DiversityWorkbench.Import.Step>();

            DiversityWorkbench.Import.DataTable DT = this.ImportWizardDataTable(
                "SamplingPlot",
                "",
                "Plot",
                DiversityWorkbench.Import.DataTable.Parallelity.unique,
                (int)DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardMainTables.SamplingPlot,
                "PlotGeometry_Cache",
                "PlotIdentifier",
                DiversitySamplingPlots.Properties.Resources.SamplingPlot);

            DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStepTemplate(DT, DiversitySamplingPlots.Properties.Resources.SamplingPlot, 0, true, null);
            S.MustSelect = true;
            IS.Add(DT.PositionKey, S);

            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, DiversityWorkbench.Import.StepColumnGroup>();

            DiversityWorkbench.Import.DataTable DTProjects = this.ImportWizardDataTable("SamplingProject", "SamplingPlot", "Project", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotTables.SamplingProject, "", "", DiversitySamplingPlots.Properties.Resources.Project);
            DTProjects.AddDatasource("ProjectID", "SELECT ProjectID AS Value, Project AS DisplayText " +
                "FROM ProjectProxy " +
                "ORDER BY DisplayText ");
            DiversityWorkbench.Import.Step SProjects = DiversityWorkbench.Import.Step.GetStepTemplate(DTProjects, DiversitySamplingPlots.Properties.Resources.Project, 1, false, null);
            SProjects.MustSelect = true;
            IS.Add(DTProjects.PositionKey, SProjects);

            this.ImportStepsEventLocalisation(ref IS);

            this.ImportStepsEventProperties(ref IS);

            DiversityWorkbench.Import.DataTable DTResource = this.ImportWizardDataTable("SamplingPlotResource", DT.TableAlias, "Resource", DiversityWorkbench.Import.DataTable.Parallelity.parallel, (int)DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotTables.SamplingPlotResource, "", "", DiversitySamplingPlots.Properties.Resources.Icones);
            DiversityWorkbench.Import.Step SResource = DiversityWorkbench.Import.Step.GetStepTemplate(DTResource, DiversitySamplingPlots.Properties.Resources.Settings, 1, false, ColumnGroups);
            IS.Add(DTResource.PositionKey, SResource);


            return IS;
        }

        private DiversityWorkbench.Import.StepColumnGroup ImportWizardStepColumnGroup(string DisplayText, System.Drawing.Image Image, string Columns)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            Columns = Columns.Replace(" ", "");
            string[] CC = Columns.Split(new char[] { ',' });
            for (int i = 0; i < CC.Length; i++)
                L.Add(CC[i]);
            DiversityWorkbench.Import.StepColumnGroup G = new DiversityWorkbench.Import.StepColumnGroup(Image, DisplayText, L);
            return G;
        }

        private DiversityWorkbench.Import.DataTable ImportWizardDataTable(string TableName, string ParentTableAlias, string DisplayText, DiversityWorkbench.Import.DataTable.Parallelity P, int SequencePosition, string IgnoredColumns, string AttachmentColumns, System.Drawing.Image Image)
        { //1
            System.Collections.Generic.List<string> lAC = new List<string>();
            AttachmentColumns = AttachmentColumns.Replace(" ", "");
            string[] ACC = AttachmentColumns.Split(new char[] { ',' });
            for (int i = 0; i < ACC.Length; i++)
                lAC.Add(ACC[i]);
            System.Collections.Generic.List<string> IC = new List<string>();
            IgnoredColumns = IgnoredColumns.Replace(" ", "");
            string[] CC = IgnoredColumns.Split(new char[] { ',' });
            for (int i = 0; i < CC.Length; i++)
                IC.Add(CC[i]);
            DiversityWorkbench.Import.DataTable DT = DiversityWorkbench.Import.DataTable.GetTableTemplate(TableName, ParentTableAlias, DisplayText, Image, P, SequencePosition, IC, lAC);
            return DT;
        }

        /*
        LocalisationSystemID	LocalisationSystemName
        3	MTB (A, CH, D)
        4	Altitude (mNN)
        7	Named area (DiversityGazetteer)
        8	Coordinates WGS84
        10	Exposition
        11	Slope
        14	Depth
        15	Height
         * */
        private enum ImportWizardPlotLocalisation { MTB = 3, Altitude = 4, WGS84 = 8, NamedArea = 7, Exposition = 10, Slope = 11, Depth = 14, Height = 15, NamedArea2 = 18, NamedArea3 = 19, NamedArea4 = 20, NamedArea5 = 21 };
        //private enum ImportWizardPlotLocalisation { Top50 = 1, GaussKrueger, MTB, Altitude, mNN, GreenwichCoordinates, NamedArea, WGS84, Coordinates, Exposition, Slope, PostsdamDatum, SamplingPlot, Depth, Height, DutchCoordinates, OesterreichischeKarte, NamedArea2, NamedArea3, NamedArea4, NamedArea5 };
        private enum ImportWizardPlotProperties { EUNIS = 1, EUNIS2012 = 2, GeographicRegions = 10, Chronostratigraphy = 20, Lithostratigraphy = 30, Lebensraumtypen = 40, Biotoptypen = 41, Pflanzengesellschaften = 50 };

        private void ImportStepsEventLocalisation(ref System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> IS)
        {
            try
            {
                DiversityWorkbench.Import.Step S_MTB = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.MTB);
                IS.Add(S_MTB.DataTableTemplate.PositionKey, S_MTB);

                DiversityWorkbench.Import.Step S_Altitude = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.Altitude);
                IS.Add(S_Altitude.DataTableTemplate.PositionKey, S_Altitude);

                DiversityWorkbench.Import.Step S_NamedArea = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.NamedArea);
                IS.Add(S_NamedArea.DataTableTemplate.PositionKey, S_NamedArea);

                DiversityWorkbench.Import.Step SWGS84 = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.WGS84);
                IS.Add(SWGS84.DataTableTemplate.PositionKey, SWGS84);

                DiversityWorkbench.Import.Step S_Exposition = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.Exposition);
                IS.Add(S_Exposition.DataTableTemplate.PositionKey, S_Exposition);

                DiversityWorkbench.Import.Step S_Slope = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.Slope);
                IS.Add(S_Slope.DataTableTemplate.PositionKey, S_Slope);

                DiversityWorkbench.Import.Step S_Depth = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.Depth);
                IS.Add(S_Depth.DataTableTemplate.PositionKey, S_Depth);

                DiversityWorkbench.Import.Step S_Height = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.Height);
                IS.Add(S_Height.DataTableTemplate.PositionKey, S_Height);

                /*
                DiversityWorkbench.Import.Step S_Coo = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.Coordinates);
                IS.Add(S_Coo.DataTableTemplate.PositionKey, S_Coo);

                DiversityWorkbench.Import.Step S_GaussKrueger = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.GaussKrueger);
                IS.Add(S_GaussKrueger.DataTableTemplate.PositionKey, S_GaussKrueger);

                DiversityWorkbench.Import.Step S_PostsdamDatum = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.PostsdamDatum);
                IS.Add(S_PostsdamDatum.DataTableTemplate.PositionKey, S_PostsdamDatum);
                 */
                DiversityWorkbench.Import.Step S_NamedArea2 = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.NamedArea2);
                IS.Add(S_NamedArea2.DataTableTemplate.PositionKey, S_NamedArea2);

                DiversityWorkbench.Import.Step S_NamedArea3 = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.NamedArea3);
                IS.Add(S_NamedArea3.DataTableTemplate.PositionKey, S_NamedArea3);

                DiversityWorkbench.Import.Step S_NamedArea4 = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.NamedArea4);
                IS.Add(S_NamedArea4.DataTableTemplate.PositionKey, S_NamedArea4);

                DiversityWorkbench.Import.Step S_NamedArea5 = this.ImportWizardLocalisation(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImportWizardPlotLocalisation.NamedArea5);
                IS.Add(S_NamedArea5.DataTableTemplate.PositionKey, S_NamedArea5);

            }
            catch (System.Exception ex)
            {
            }
        }

        private void ImportStepsEventProperties(ref System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> IS)
        {
            DiversityWorkbench.Import.Step S_Chrono = this.ImportWizardEventProperty(ImportWizardPlotProperties.Chronostratigraphy);
            IS.Add(S_Chrono.DataTableTemplate.PositionKey, S_Chrono);

            DiversityWorkbench.Import.Step S_Lithostratigraphy = this.ImportWizardEventProperty(ImportWizardPlotProperties.Lithostratigraphy);
            IS.Add(S_Lithostratigraphy.DataTableTemplate.PositionKey, S_Lithostratigraphy);

            DiversityWorkbench.Import.Step S_GeographicRegions = this.ImportWizardEventProperty(ImportWizardPlotProperties.GeographicRegions);
            IS.Add(S_GeographicRegions.DataTableTemplate.PositionKey, S_GeographicRegions);

            DiversityWorkbench.Import.Step S_Lebensraumtypen = this.ImportWizardEventProperty(ImportWizardPlotProperties.Lebensraumtypen);
            IS.Add(S_Lebensraumtypen.DataTableTemplate.PositionKey, S_Lebensraumtypen);

            DiversityWorkbench.Import.Step S_Biotoptypen = this.ImportWizardEventProperty(ImportWizardPlotProperties.Biotoptypen);
            IS.Add(S_Biotoptypen.DataTableTemplate.PositionKey, S_Biotoptypen);

            DiversityWorkbench.Import.Step S_Pflanzengesellschaften = this.ImportWizardEventProperty(ImportWizardPlotProperties.Pflanzengesellschaften);
            IS.Add(S_Pflanzengesellschaften.DataTableTemplate.PositionKey, S_Pflanzengesellschaften);

            DiversityWorkbench.Import.Step S_EUNIS = this.ImportWizardEventProperty(ImportWizardPlotProperties.EUNIS);
            IS.Add(S_EUNIS.DataTableTemplate.PositionKey, S_EUNIS);

            DiversityWorkbench.Import.Step S_EUNIS2012 = this.ImportWizardEventProperty(ImportWizardPlotProperties.EUNIS2012);
            IS.Add(S_EUNIS2012.DataTableTemplate.PositionKey, S_EUNIS2012);
        }

        private DiversityWorkbench.Import.Step ImportWizardLocalisation(ImportWizardPlotLocalisation Localisation)
        {
            /*SELECT     CollectionEventID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, 
            ResponsibleName, ResponsibleAgentURI, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, LogCreatedWhen, LogCreatedBy, 
            LogUpdatedWhen, LogUpdatedBy, RowGUID, RecordingMethod
            FROM         CollectionEventLocalisation*/

            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, DiversityWorkbench.Import.StepColumnGroup>();

            DiversityWorkbench.Import.StepColumnGroup Gres = this.ImportWizardStepColumnGroup("Responsible",
                DiversitySamplingPlots.Properties.Resources.Agent,
                "ResponsibleName, ResponsibleAgentURI, DeterminationDate");
            ColumnGroups.Add(Gres.DisplayText, Gres);
            string IgnoredColumns = "";
            DiversityWorkbench.Import.StepColumnGroup Ggeo;
            switch (Localisation)
            {
                //case ImportWizardPlotLocalisation.Coordinates:
                //case ImportWizardPlotLocalisation.PostsdamDatum:
                //case ImportWizardPlotLocalisation.GreenwichCoordinates:
                //case ImportWizardPlotLocalisation.SamplingPlot:
                case ImportWizardPlotLocalisation.WGS84:
                case ImportWizardPlotLocalisation.MTB:
                    Ggeo = this.ImportWizardStepColumnGroup("Geography",
                        DiversitySamplingPlots.Properties.Resources.Localisation,
                        "Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache");
                    ColumnGroups.Add(Ggeo.DisplayText, Ggeo);
                    IgnoredColumns = "DistanceToLocation, DirectionToLocation";
                    break;
                case ImportWizardPlotLocalisation.NamedArea:
                case ImportWizardPlotLocalisation.NamedArea2:
                case ImportWizardPlotLocalisation.NamedArea3:
                case ImportWizardPlotLocalisation.NamedArea4:
                case ImportWizardPlotLocalisation.NamedArea5:
                    Ggeo = this.ImportWizardStepColumnGroup("Geography",
                        DiversitySamplingPlots.Properties.Resources.Localisation,
                        "Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, DistanceToLocation, DirectionToLocation");
                    ColumnGroups.Add(Ggeo.DisplayText, Ggeo);
                    IgnoredColumns = "";
                    break;
                //case ImportWizardPlotLocalisation.OesterreichischeKarte:
                //case ImportWizardPlotLocalisation.GaussKrueger:
                //    Ggeo = this.ImportWizardStepColumnGroup("Geography",
                //        DiversitySamplingPlots.Properties.Resources.Localisation,
                //        "AverageLatitudeCache, AverageLongitudeCache");
                //    ColumnGroups.Add(Ggeo.DisplayText, Ggeo);
                //    IgnoredColumns = "Geography, AverageAltitudeCache, DistanceToLocation, DirectionToLocation";
                //    break;
                //case ImportWizardPlotLocalisation.Top50:
                case ImportWizardPlotLocalisation.Altitude:
                default:
                    IgnoredColumns = "Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, " +
                        "DistanceToLocation, DirectionToLocation";
                    break;
            }

            string Title = Localisation.ToString();
            System.Collections.Generic.Dictionary<string, string> DD = DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.LocalisationSystemInfo(((int)Localisation).ToString());
            string DisplayTextLocation1 = "";
            string DisplayTextLocation2 = "";
            if (DD.Count > 0)
            {
                DisplayTextLocation1 = DD["DisplayTextLocation1"];
                DisplayTextLocation2 = DD["DisplayTextLocation2"];
            }
            else
            {
                DisplayTextLocation1 = "Location1";
                DisplayTextLocation2 = "Location2";
            }
            System.Drawing.Image Image = DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImageForLocalisationSystem(((int)Localisation));
            DiversityWorkbench.Import.DataTable DT = this.ImportWizardDataTable("SamplingPlotLocalisation", "SamplingPlot", Title, DiversityWorkbench.Import.DataTable.Parallelity.restricted, ((int)ImportWizardPlotTables.SamplingPlotLocalisation), IgnoredColumns, "", Image);
            DT.AddPresetValue("LocalisationSystemID", ((int)Localisation).ToString());
            DT.AddColumnDisplayText("Location1", DisplayTextLocation1);
            DT.AddColumnDisplayText("Location2", DisplayTextLocation2);
            DT.ParentTableAlias = "SamplingPlot";
            DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStepTemplate(DT, Image, 1, false, ColumnGroups);
            //DT.Step = S;
            return S;
        }

        private static System.Data.DataTable _DtLocalisationSystem;

        public static System.Data.DataTable DtLocalisationSystem
        {
            get
            {
                if (DiversitySamplingPlots.Forms.FormDiversitySamplingPlots._DtLocalisationSystem == null)
                {
                    string SQL = "SELECT LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, DefaultMeasurementUnit, " +
                        "DefaultAccuracyOfLocalisation, ParsingMethodName, DisplayText, DisplayEnable, DisplayOrder, " +
                        "Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2, " +
                        "DescriptionLocation2 " +
                        "FROM LocalisationSystem WHERE (DisplayEnable = 1) ORDER BY LocalisationSystemName";
                    DiversitySamplingPlots.Forms.FormDiversitySamplingPlots._DtLocalisationSystem = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(DiversitySamplingPlots.Forms.FormDiversitySamplingPlots._DtLocalisationSystem);
                }
                return DiversitySamplingPlots.Forms.FormDiversitySamplingPlots._DtLocalisationSystem;
            }
            //set { LookupTable._Localisation = value; }
        }

        public static System.Collections.Generic.Dictionary<string, string> LocalisationSystemInfo(string Code)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            System.Data.DataRow[] RR = DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.DtLocalisationSystem.Select("LocalisationSystemID = " + Code);
            if (RR.Length > 0)
            {
                foreach (System.Data.DataColumn C in RR[0].Table.Columns)
                    Dict.Add(C.ColumnName, RR[0][C.ColumnName].ToString());
            }
            return Dict;
        }

        private DiversityWorkbench.Import.Step ImportWizardEventProperty(ImportWizardPlotProperties Property)
        {
            /*SELECT     CollectionEventID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, 
            AverageValueCache, LogCreatedWhen, LogCreatedBy, LogUpdatedWhen, LogUpdatedBy, RowGUID
            FROM         CollectionEventProperty*/

            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, DiversityWorkbench.Import.StepColumnGroup>();

            DiversityWorkbench.Import.StepColumnGroup Gres = this.ImportWizardStepColumnGroup("Responsible",
                DiversitySamplingPlots.Properties.Resources.Agent,
                "ResponsibleName, ResponsibleAgentURI");
            ColumnGroups.Add(Gres.DisplayText, Gres);

            string IgnoredColumns = "PropertyValue, AverageValueCache";

            switch (Property)
            {
                case ImportWizardPlotProperties.Chronostratigraphy:
                case ImportWizardPlotProperties.EUNIS:
                case ImportWizardPlotProperties.EUNIS2012:
                case ImportWizardPlotProperties.GeographicRegions:
                case ImportWizardPlotProperties.Lebensraumtypen:
                case ImportWizardPlotProperties.Biotoptypen:
                case ImportWizardPlotProperties.Lithostratigraphy:
                case ImportWizardPlotProperties.Pflanzengesellschaften:
                    IgnoredColumns = "";
                    break;
                default:
                    break;
            }

            string Title = Property.ToString();
            System.Drawing.Image Image = DiversitySamplingPlots.Forms.FormDiversitySamplingPlots.ImageForCollectionEventProperty(((int)Property));
            DiversityWorkbench.Import.DataTable DT = this.ImportWizardDataTable("SamplingPlotProperty", "SamplingPlot", Title, DiversityWorkbench.Import.DataTable.Parallelity.restricted, ((int)ImportWizardPlotTables.SamplingPlotProperty), IgnoredColumns, "", Image);
            DT.AddPresetValue("PropertyID", ((int)Property).ToString());
            DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStepTemplate(DT, Image, 1, false, ColumnGroups);

            return S;
        }

        public static System.Drawing.Image ImageForLocalisationSystem(int LocalisationSystemID)
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
            //DiversityCollection.Specimen.OverviewImageLocalisationSystem L = OverviewImageLocalisationSystem.Localisation;
            switch (LocalisationSystemID)
            {
                case 2:
                    return DiversitySamplingPlots.Properties.Resources.GaussKrueger;
                case 3:
                    return DiversitySamplingPlots.Properties.Resources.MTB;
                case 4:
                    return DiversitySamplingPlots.Properties.Resources.Altitude;
                case 7:
                    return DiversitySamplingPlots.Properties.Resources.Place;
                case 10:
                    return DiversitySamplingPlots.Properties.Resources.Exposition;
                case 11:
                    return DiversitySamplingPlots.Properties.Resources.Slope;
                case 13:
                    return DiversitySamplingPlots.Properties.Resources.SamplingPlot;
                case 14:
                    return DiversitySamplingPlots.Properties.Resources.Depth;
                case 15:
                    return DiversitySamplingPlots.Properties.Resources.Height;
                case 16:
                    return DiversitySamplingPlots.Properties.Resources.GaussKrueger;
                case 17:
                    return DiversitySamplingPlots.Properties.Resources.MTB;
                case 18:
                case 19:
                case 20:
                case 21:
                    return DiversitySamplingPlots.Properties.Resources.Place;
            }
            return DiversitySamplingPlots.Properties.Resources.Localisation;
            //return DiversityCollection.Specimen.ImageListLocalisationSystem.Images[(int)L];
        }

        public static System.Drawing.Image ImageForCollectionEventProperty(int PropertyID)
        {
            /*
            Stratigraphy
            Vegetation             
             * * */
            switch (PropertyID)
            {
                case 20:
                case 30:
                    return DiversitySamplingPlots.Properties.Resources.Geology;
            }
            return DiversitySamplingPlots.Properties.Resources.Habitat;
        }

        #endregion // Import wizard events

        #endregion

        #endregion

        #region Administration

        /// <summary>
        /// Handle visibilty of Administration menu items.
        /// </summary>
        private void setAdminMenu()
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    bool Any = false;
                    bool IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                    bool IsDboOrSysAdmin = false;
                    bool IsEditor = DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchEditor");
                    if (!DiversityWorkbench.Settings.IsTrustedConnection && DiversityWorkbench.Settings.DatabaseUser.Length > 0)
                    {
                        IsDboOrSysAdmin = DiversityWorkbench.Database.LoginIsSysAdmin(DiversityWorkbench.Settings.DatabaseUser);
                    }
                    else
                    {
                        string User = System.Environment.UserName;
                        IsDboOrSysAdmin = DiversityWorkbench.Database.LoginIsSysAdmin(User);
                        if (!IsDboOrSysAdmin)
                        {
                            string Sql = "select user_name()";
                            User = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sql);
                            IsDboOrSysAdmin = DiversityWorkbench.Database.LoginIsSysAdmin(User);
                            if (!IsDboOrSysAdmin && User == "dbo")
                            {
                                IsDboOrSysAdmin = true;
                            }
                        }
                    }
                    bool IsAdmin = DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchAdministrator");
                    if (!IsAdmin && IsDboOrSysAdmin) IsAdmin = IsDboOrSysAdmin;

                    //bool OK;
                    string TableName = "";

                    // User
                    this.userToolStripMenuItem.Visible = false;


                    // PW
                    if (DiversityWorkbench.Database.DatabaseRoles().Contains("DiversityWorkbenchUser")
                        || IsTrustedConnection)
                    {
                        this.changePasswordToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        this.changePasswordToolStripMenuItem.Visible = true;
                        //this.administrationToolStripMenuItem.Visible = true;
                        Any = true;
                    }

                    // Renaming the database and the BaseURL
                    this.renameDatabaseToolStripMenuItem.Enabled = IsDboOrSysAdmin;
                    this.setBaseURLToolStripMenuItem.Enabled = IsDboOrSysAdmin;
                    this.databaseToolsToolStripMenuItem.Enabled = IsAdmin;
                    this.loginsToolStripMenuItem.Enabled = IsDboOrSysAdmin;
                    if (!Any && (IsDboOrSysAdmin || IsAdmin)) Any = true;

                    this.documentationToolStripMenuItem.Enabled = IsDboOrSysAdmin || IsAdmin;
                    this.sitePropertiesToolStripMenuItem.Enabled = IsDboOrSysAdmin || IsAdmin;
                    if (!Any && IsAdmin)
                        Any = true;

                    // DatabaseTools
                    //TableName = "SamplingPlot_log";
                    //bool OK = this.FormFunctions.getObjectPermissions(TableName, "DELETE");
                    this.databaseToolsToolStripMenuItem.Enabled = IsDboOrSysAdmin;
                    if (!Any && IsDboOrSysAdmin) Any = true;

                    // Projects
                    TableName = "ProjectProxy";
                    bool OK = this.FormFunctions.getObjectPermissions(TableName, "INSERT");
                    this.projectsToolStripMenuItem.Enabled = IsAdmin || IsDboOrSysAdmin || OK;
                    if (!Any && IsAdmin)
                        Any = true;

                    this.maintenanceToolStripMenuItem.Enabled = IsAdmin || IsEditor || IsDboOrSysAdmin;
                    if (!Any && IsEditor)
                        Any = true;


                    if (Any)
                        this.administrationToolStripMenuItem.Enabled = true;
                    else
                        this.administrationToolStripMenuItem.Enabled = false;
                }
                else
                {
                    this.administrationToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                this.administrationToolStripMenuItem.Enabled = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        /// <summary>
        /// Initialize replication menu.
        /// </summary>
        /// <param name="IsAdmin"></param>
        private void initReplicationMenu(bool IsAdmin)
        {
            this.addPublisherToolStripMenuItem.Enabled = true;
            //this.setServerToolStripMenuItem.Visible = false;
            //this.downloadToolStripMenuItem.Visible = false;
            //this.uploadToolStripMenuItem.Visible = false;
            //this.mergeToolStripMenuItem.Visible = false;
            this.cleanDatabaseToolStripMenuItem.Enabled = true;

            this.replicationToolStripMenuItem.DropDownItems.Clear();
            if (IsAdmin)
            {
                this.replicationToolStripMenuItem.DropDownItems.Add(this.addPublisherToolStripMenuItem);
                this.replicationToolStripMenuItem.DropDownItems.Add(this.cleanDatabaseToolStripMenuItem);
            }

            string SQL = "";
            if (this._DtReplicationPublisher == null)
                this._DtReplicationPublisher = new DataTable();
            else
                this._DtReplicationPublisher.Clear();

            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    // Import
                    SQL = "SELECT DatabaseName, Server, Port FROM ReplicationPublisher";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtReplicationPublisher);
                    if (this._DtReplicationPublisher.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in this._DtReplicationPublisher.Rows)
                        {
                            string DisplayText = R["DatabaseName"].ToString() + " on " + R["Server"].ToString();
                            System.Windows.Forms.ToolStripMenuItem MI = new ToolStripMenuItem(DisplayText, DiversitySamplingPlots.Properties.Resources.Synchronize);
                            this.replicationToolStripMenuItem.DropDownItems.Add(MI);
                            System.Windows.Forms.ToolStripMenuItem MiDownload = new ToolStripMenuItem("Download ...", DiversitySamplingPlots.Properties.Resources.Download, this.downloadToolStripMenuItem_Click);
                            MiDownload.Tag = R;
                            MiDownload.ToolTipText = "Download data from the replication publisher";
                            MI.DropDownItems.Add(MiDownload);
                            System.Windows.Forms.ToolStripMenuItem MiMerge = new ToolStripMenuItem("Merge ...", DiversitySamplingPlots.Properties.Resources.Merge, this.mergeToolStripMenuItem_Click);
                            MiMerge.Tag = R;
                            MiMerge.ToolTipText = "Merge data for the current project between the replication publisher and your local database";
                            MI.DropDownItems.Add(MiMerge);
                            System.Windows.Forms.ToolStripMenuItem MiUpload = new ToolStripMenuItem("Upload ...", DiversitySamplingPlots.Properties.Resources.Upload, this.uploadToolStripMenuItem_Click);
                            MiUpload.Tag = R;
                            MiUpload.ToolTipText = "Upload the data of the current project to the replication publisher";
                            MI.DropDownItems.Add(MiUpload);
                            if (IsAdmin)
                            {
                                System.Windows.Forms.ToolStripMenuItem MiDelete = new ToolStripMenuItem("Remove", DiversitySamplingPlots.Properties.Resources.NoDatabase, this.deleteReplicationPublisherToolStripMenuItem_Click);
                                MiDelete.Tag = R;
                                MiDelete.ToolTipText = "Remove the replication publisher from the list";
                                MI.DropDownItems.Add(MiDelete);
                            }
                        }
                        this.replicationToolStripMenuItem.Enabled = true;
                        this.addPublisherToolStripMenuItem.Enabled = true;
                        //this.setServerToolStripMenuItem.Visible = false;
                        //this.downloadToolStripMenuItem.Visible = false;
                        //this.uploadToolStripMenuItem.Visible = false;
                        //this.mergeToolStripMenuItem.Visible = false;
                        this.cleanDatabaseToolStripMenuItem.Enabled = true;
                    }
                    this.replicationToolStripMenuItem.DropDownItems.Add(this.cleanDatabaseToolStripMenuItem);
                }
            }
            catch (Exception ex)
            {
                this.replicationToolStripMenuItem.Enabled = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Projects.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormLoginAdminGetProjects f = new DiversityWorkbench.Forms.FormLoginAdminGetProjects(this.DtProjectProxy, DiversityWorkbench.Settings.ServerConnection);
            f.setKeyword("projectadministration_dsp");
            f.ShowDialog();
            if (f.AddedProjectRows.Count > 0)
            {
                foreach (System.Data.DataRow R in f.AddedProjectRows)
                {
                    System.Data.DataRow[] rr = this.DtProjectProxy.Select("ProjectID = " + R["ProjectID"].ToString());
                    if (rr.Length == 0)
                    {
                        System.Data.DataRow RN = this.DtProjectProxy.NewRow();
                        RN["ProjectID"] = R["ProjectID"];
                        RN["Project"] = R["Project"];
                        RN["ProjectURI"] = R["ProjectURI"];
                        this.DtProjectProxy.Rows.Add(RN);
                    }
                    else
                    {
                        if (R["Project"].ToString().ToLower().Trim() == rr[0]["Project"].ToString().ToLower().Trim())
                        {
                            rr[0]["ProjectURI"] = R["ProjectURI"];
                            rr[0]["Project"] = R["Project"];
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("The ProjectID " + R["ProjectID"].ToString() + " for the project " + R["Project"].ToString() + " is used for the project " + rr[0]["Project"].ToString());
                        }
                    }
                }
                this._adapterProjectProxy.Update(this.DtProjectProxy);
            }
        }

        /// <summary>
        /// Fill Plot Properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Properties fill up.....", "WARNING", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                // FillPlotProperties();
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormPassword f = new DiversityWorkbench.Forms.FormPassword();
                f.ShowDialog();
            }
            catch (System.Exception ex)
            { }
        }

        private void resourcesDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.setWorkbenchDirectory();// .Settings.setResourcesDirectory();
        }

        private void sitePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversitySamplingPlots.Forms.FormProperty f = new Forms.FormProperty();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        #region Database

        /// <summary>
        /// Logins.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(DiversityWorkbench.Settings.DatabaseName))
            {

                DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                DiversityWorkbench.UserControls.UserControlModuleRelatedEntry u = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            }
            DiversityWorkbench.Forms.FormLoginAdministration f = new DiversityWorkbench.Forms.FormLoginAdministration();
            f.setKeyword("loginadministration_dsp");
            f.ShowDialog();
        }

        /// <summary>
        /// Documentation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormDocumentation f = new DiversityWorkbench.Forms.FormDocumentation();
            f.setKeyword("documentation_of_database_dsp");
            f.Width = this.Width - 20;
            f.Height = this.Height - 20;
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        /// <summary>
        /// Set base URL.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void setPublishedAddressToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DiversityWorkbench.Database.SetBaseURL();
        }


        /// <summary>
        /// Rename database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void renameDatabaseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (DiversityWorkbench.Database.RenameDatabase())
                this.setDatabase();
        }


        /// <summary>
        /// Tools.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void databaseToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormDatabaseTool f = new DiversityWorkbench.Forms.FormDatabaseTool();
            f.ShowDialog();
        }

        private void maintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversitySamplingPlots.Maintenance.FormMaintenance form = new Maintenance.FormMaintenance();
            form.Width = this.Width - 10;
            form.Height = this.Height - 10;
            form.StartPosition = FormStartPosition.CenterParent;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            form.ShowDialog();
        }


        #endregion // Database


        #endregion // Administration

        #region Help

        private void setHelpMenu()
        {
            try
            {
                bool OK = DiversityWorkbench.Settings.ConnectionString.Length > 0 && this.DatabaseRoles.Contains("db_owner");
                this.windowSizeToolStripMenuItem.Visible = OK;
                this.setErrorLogMenu();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Errorlog

        private void setErrorLogMenu()
        {
            try
            {
                this.clearErrorlogAtProgramStartToolStripMenuItem.Visible = !DiversityWorkbench.Settings.ErrorLogResetAtStartup;
                this.keepErrorlogToolStripMenuItem.Visible = DiversityWorkbench.Settings.ErrorLogResetAtStartup;
                this.clearErrorlogToolStripMenuItem.Visible = !DiversityWorkbench.Settings.ErrorLogResetAtStartup;
                this.setInfoMenu();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setInfoMenu()
        {
            try
            {
                if (!this.helpToolStripMenuItem.DropDownItems.ContainsKey("infoToolStripMenuItem"))
                {
                    System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                    infoToolStripMenuItem.Image = global::DiversitySamplingPlots.Properties.Resources.info;
                    infoToolStripMenuItem.Name = "infoToolStripMenuItem";
                    infoToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
                    infoToolStripMenuItem.Text = "Info ...";
                    infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
                    this.helpToolStripMenuItem.DropDownItems.Add(infoToolStripMenuItem);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                DiversitySamplingPlots.Forms.FormAbout f = new DiversitySamplingPlots.Forms.FormAbout();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>


        private void clearErrorlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ExceptionHandling.InitErrorLogFile("");
        }

        private void errorLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ExceptionHandling.ShowErrorLog();
        }

        private void keepErrorlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ErrorLogResetAtStartup = false;
            this.setErrorLogMenu();
        }

        private void clearErrorlogAtProgramStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.ErrorLogResetAtStartup = true;
            this.setErrorLogMenu();
        }

        #endregion

        private void xGA1024X768ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Height = 768;
            this.Width = 1024;
        }

        #region Help events

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string urlManual = global::DiversitySamplingPlots.Properties.Settings.Default.ManualURL;
                if (!string.IsNullOrEmpty(urlManual))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = urlManual,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to open manual: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);

            }
        }

        // activate when we have the information model page in the manual
        //private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string urlManual = global::DiversitySamplingPlots.Properties.Settings.Default.InformationModelManualURL;
        //        if (!string.IsNullOrEmpty(urlManual))
        //        {
        //            Process.Start(new ProcessStartInfo
        //            {
        //                FileName = urlManual,
        //                UseShellExecute = true
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            $"Unable to open information model manual page: {ex.Message}",
        //            "Error",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);

        //    }
        //}

        /// <summary>
        /// Feedback dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                // System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,   --- old version!!
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.PlotID.ToString());
        }

        #endregion

        #endregion // Help

        #region Update

        /// <summary>
        /// Check database for DB and client versions and show Update menu entries, if applicable.
        /// </summary>
        private void setUpdateMenu()
        {
            string SQL = "select user_name()";
            string User = "";
            bool UpdateDatabase = false;
            bool UpdateClient = false;
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    User = C.ExecuteScalar().ToString();
                    con.Close();
                    bool IsAdmin = false;
                    if (this.DatabaseRoles.Contains("Administrator"))
                        IsAdmin = true;
                    if (User == "dbo" || IsAdmin)
                    {
                        string VersionOfDatabase = "";
                        UpdateDatabase = DiversityWorkbench.Database.CheckIfDatabaseNeedsAnUpdate(DiversitySamplingPlots.Properties.Settings.Default.DatabaseVersion, ref VersionOfDatabase);
                        this.updateDatabaseToolStripMenuItem.Visible = UpdateDatabase;
                    }
                    string ClientVersionInDB = "";
                    UpdateClient = DiversityWorkbench.Database.CheckIfClientNeedsAnUpdate(
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Major,
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Minor,
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Build,
                        System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Revision,
                        ref ClientVersionInDB);
                    this.updateClientToolStripMenuItem.Visible = UpdateClient;
                    if ((UpdateClient || UpdateDatabase) && (User == "dbo" || IsAdmin))
                        this.updateToolStripMenuItem.Visible = true;
                    else
                        this.updateToolStripMenuItem.Visible = false;
                }
                catch { }
            }
        }

        /// <summary>
        /// Update database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Database.UpdateDatabase(DiversitySamplingPlots.Properties.Settings.Default.DatabaseVersion,
                "DiversitySamplingPlotsUpdate_",
                System.Windows.Forms.Application.StartupPath + "\\DiversitySamplingPlots.chm",
                Properties.Resources.ResourceManager))
                this.setDatabase();
            this.setUpdateMenu();
        }

        /// <summary>
        /// Update client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
            

            /* old version -----
            string DownloadPathForClient = "http://diversityworkbench.net/Portal/DiversitySamplingPlots";
            DiversityWorkbench.Forms.FormUpdateClient f = new DiversityWorkbench.Forms.FormUpdateClient(DownloadPathForClient, this.ClientVersionUsed(), DiversityWorkbench.Database.ClientVersionAvailable());
            f.StartPosition = FormStartPosition.CenterParent;
            f.Width = this.Width - 10;
            f.Height = this.Height - 10;
            f.ShowDialog();
            */
        }
        #endregion // Update

        #region Test
        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.userControlQueryList.ProjectID == -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }

            int ProjectID = this.userControlQueryList.ProjectID;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            // BaseURL
            string SQL = "SELECT dbo.BaseURL()";
            string BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            string URL = BaseURL + ProjectID.ToString();

            //DiversityWorkbench.SamplingPlot.ResetChart(URL);

            // Section if present
            int? SectionID = null;
            //System.Collections.Generic.Dictionary<int, string> Sections = DiversityWorkbench.Terminology.Sections(this.TerminologyID, DiversityWorkbench.Settings.ServerConnection);
            //if (Sections != null && Sections.Count > 0)
            //{
            //    DiversityWorkbench.Forms.FormGetStringFromList fSect = new DiversityWorkbench.Forms.FormGetStringFromList(Sections, "Section", "Do you want to restrict to a section?");
            //    fSect.ShowDialog();
            //    if (fSect.DialogResult == System.Windows.Forms.DialogResult.OK && fSect.SelectedValue != null)
            //    {
            //        SectionID = int.Parse(fSect.SelectedValue);
            //    }
            //    else
            //        return;
            //}


            // Da hat der Wolfgang was verbastelt - muss ich erst reparieren ...

            DiversityWorkbench.SamplingPlot.SubPlots("");

            //DiversityWorkbench.Chart C = DiversityWorkbench.SamplingPlot.GetChart(URL, SectionID, this.Height, this.Width);
            //DiversityWorkbench.Forms.FormRemoteQueryChart f = new DiversityWorkbench.Forms.FormRemoteQueryChart(C, BaseURL);
            //this.Cursor = System.Windows.Forms.Cursors.Default;
            //f.StartPosition = FormStartPosition.CenterParent;
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK && f.SelectedID() != null)
            //{
            //    string SelectedID = f.SelectedID().ToString();
            //    int i;
            //    if (int.TryParse(SelectedID, out i))
            //        this.setSamplingPlot(i);
            //    else
            //    {

            //    }
            //}
            //else
            //{

            //}

        }

        #endregion

        #endregion

        #region Interface
        public void ShowInMainForm(int ID, int? width, int? height)
        {
            this.setSamplingPlot(ID);
            if (width != null) this.Width = (int)width;
            if (height != null) this.Height = (int)height;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowQuery(false);
            this.ShowDialog();
        }


        #endregion

        #region Methods

        private bool setSamplingPlot(int PlotID)
        {
            bool OK = true;
            try
            {
                this.splitContainerMain.Panel2.Enabled = true;

                if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0)
                {
                    this.updatePlot();
                }
                // this.userControlImagePlotImage.ImagePath = "";
                // this.web BrowserMap.Url = new Uri("about:blank ");
                this.fillSamplingPlot(PlotID);
                if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0)
                {
                    this.setHeader();
                    this.splitContainerData.Visible = true;
                    this.tableLayoutPanelHeader.Visible = true;
                    this.setImageVisibilityMarker(DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.ImageVisibility);
                    if (this.ShowMap)
                    {
                        //if (this.userControlGIS.MapDisplayMode == DiversitySamplingPlots.UserControls.UserControlGIS.MapMode.Browse)
                        //    this.showGoogleMap();
                        this.initGisMap();
                    }
                    this.fillLocalisationList();
                    this.linkLabelUri.Text = DiversityWorkbench.Settings.ServerConnection.BaseURL + PlotID.ToString();
                    this.scanModules();
                }
                else
                {
                    this.splitContainerData.Visible = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void fillSamplingPlot(int PlotID)
        {
            try
            {
                this.dataSetSamplingPlot.Clear();

                // -obs-  if (this.dataSetSamplingPlot.SamplingPlotPointType_Enum.Rows.Count == 0)
                // -obs-    DiversityWorkbench.EnumTable.FillEnumTable(this.dataSetSamplingPlot.SamplingPlotPointType_Enum, false, false);

                string WhereClause = " WHERE PlotID = " + PlotID.ToString() +
                    " AND PlotID IN (SELECT PlotID FROM PlotID_UserAvailable)";

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSamplingPlot, DiversitySamplingPlots.SamplingPlot.SqlSamplingPlot + WhereClause, this.dataSetSamplingPlot.SamplingPlot);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSamplingProject, DiversitySamplingPlots.SamplingPlot.SqlSamplingProject + WhereClause + " ORDER BY ProjectID", this.dataSetSamplingPlot.SamplingProject);
                // this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSamplingPlotPointInImage, DiversitySamplingPlots.SamplingPlot.SqlSamplingPlotPointInImage + WhereClause, this.dataSetSamplingPlot.SamplingPlotPointInImage);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSamplingPlotProperty, DiversitySamplingPlots.SamplingPlot.SqlSamplingPlotProperty + WhereClause, this.dataSetSamplingPlot.SamplingPlotProperty);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSamplingPlotResource, DiversitySamplingPlots.SamplingPlot.SqlSamplingPlotResource + WhereClause, this.dataSetSamplingPlot.SamplingPlotResource);

                if (!this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSamplingPlotLocalisation, DiversitySamplingPlots.SamplingPlot.SqlSamplingPlotLocalisation + WhereClause, this.dataSetSamplingPlot.SamplingPlotLocalisation))
                {
                    if (!DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSamplingPlotLocalisation, this.dataSetSamplingPlot.SamplingPlotLocalisation, DiversitySamplingPlots.SamplingPlot.SqlSamplingPlotLocalisation + WhereClause, DiversityWorkbench.Settings.ConnectionString))
                    {
                        try
                        {
                            if (this.sqlDataAdapterSamplingPlotLocalisation == null)
                                this.sqlDataAdapterSamplingPlotLocalisation = new Microsoft.Data.SqlClient.SqlDataAdapter(DiversitySamplingPlots.SamplingPlot.SqlSamplingPlotProperty + WhereClause, DiversityWorkbench.Settings.ConnectionString);
                            Microsoft.Data.SqlClient.SqlCommandBuilder cbA = new Microsoft.Data.SqlClient.SqlCommandBuilder(this.sqlDataAdapterSamplingPlotLocalisation);
                            this.sqlDataAdapterSamplingPlotLocalisation.Fill(this.dataSetSamplingPlot.SamplingPlotLocalisation);
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Init sqlDataAdapterSamplingPlotLocalisation in fillSamplingPlot()");
                        }
                    }

                }

                // string SQL = "SELECT PlotID, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotGeometry_Cache.ToString() AS PlotGeometry_Cache " + "FROM SamplingPlot " + WhereClause;
                string SQL = "SELECT PlotID, Geography, null " + "FROM SamplingPlotGeoInfo " + WhereClause;

                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._dtGeo.Clear();
                ad.Fill(this._dtGeo);

                if (_dtGeo.Rows.Count > 0)
                {
                    string geoStr = _dtGeo.Rows[0][1].ToString();
                    if (geoStr.Length > 50)
                        geoStr = _dtGeo.Rows[0][1].ToString().Substring(0, 50) + "...";
                    this.labelGeography.Text = "Geography: " + geoStr;
#if DEBUG
                    this.userControlGeography.DataRowSamplingPlot_GeoCache = _dtGeo.Rows[0];
#endif
                }
                else
                {
                    this.labelGeography.Text = "Geography: ";
                }

                // filling the geographical values
                if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0)
                {
                    //this.fillGeography(PlotID);
                    //this.fillGeometry(PlotID);
                    this.fillProjectList(PlotID);
                    this.fillProjectListNoAccess(PlotID);
                }

                this.FormFunctions.FillImageList(this.listBoxImages, this.imageList, this.imageListForm, this.DataTableImages(PlotID), "ResourceURI", this.pictureBoxImage);


                this.buildHierarchy(PlotID);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void fillProjectList(int PlotID)
        {
            this.dataSetSamplingPlot.SamplingProjectList.Clear();
            string SQL = DiversitySamplingPlots.SamplingPlot.SqlSamplingProjectList + " WHERE PlotID = " + PlotID.ToString();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this.dataSetSamplingPlot.SamplingProjectList);
            this.listBoxProjects.SelectedIndex = -1;
        }

        private void fillProjectListNoAccess(int PlotID)
        {
            this.dataSetSamplingPlot.SamplingProjectListNoAccess.Clear();
            string SQL = DiversitySamplingPlots.SamplingPlot.SqlSamplingProjectListNoAccess + " AND PlotID = " + PlotID.ToString();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this.dataSetSamplingPlot.SamplingProjectListNoAccess);
            this.listBoxProjectsNoAccess.SelectedIndex = -1;
        }


        private void updatePlot()
        {
            if (this.sqlDataAdapterSamplingPlot != null)
            {

                this.FormFunctions.updateTable(this.dataSetSamplingPlot, "SamplingPlot", this.sqlDataAdapterSamplingPlot, this.BindingContext);
                //this.FormFunctions.updateTable(this.dataSetSamplingPlot, "SamplingPlotImage", this.sqlDataAdapterSamplingPlotImage, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetSamplingPlot, "SamplingProject", this.sqlDataAdapterSamplingProject, this.BindingContext);

                // special treatment for Property
                System.Data.DataRowView RProp = (System.Data.DataRowView)this.samplingPlotPropertyBindingSource.Current;
                if (RProp != null)
                {
                    try
                    {
                        RProp.BeginEdit();
                        RProp.EndEdit();
                    }
                    catch (System.Data.ConstraintException cex)
                    {
                        System.Windows.Forms.MessageBox.Show(cex.Message);
                    }
                }
                this.FormFunctions.updateTable(this.dataSetSamplingPlot, "SamplingPlotProperty", this.sqlDataAdapterSamplingPlotProperty, this.BindingContext);

                // special treatment for Resource
                System.Data.DataRowView RRes = (System.Data.DataRowView)this.samplingPlotResourceBindingSource.Current;
                if (RRes != null)
                {
                    try
                    {
                        RRes.BeginEdit();
                        RRes.EndEdit();
                    }
                    catch (System.Data.ConstraintException cex)
                    {
                        System.Windows.Forms.MessageBox.Show(cex.Message);
                    }
                }
                this.FormFunctions.updateTable(this.dataSetSamplingPlot, "SamplingPlotResource", this.sqlDataAdapterSamplingPlotResource, this.BindingContext);

                // special treatment for Localisation
                System.Data.DataRowView RLoc = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
                if (RLoc != null)
                {
                    try
                    {
                        RLoc.BeginEdit();
                        RLoc.EndEdit();
                    }
                    catch (System.Data.ConstraintException cex)
                    {
                        System.Windows.Forms.MessageBox.Show(cex.Message);
                    }
                }
                this.FormFunctions.updateTable(this.dataSetSamplingPlot, "SamplingPlotLocalisation", this.sqlDataAdapterSamplingPlotLocalisation, this.BindingContext);
                // this.updateGeography(this.PlotID);
                // this.updateGeometry(this.PlotID);
            }
        }


        private void buildHierarchy(int PlotID)
        {
            this.treeViewHierarchy.Nodes.Clear();
            string SQL = "SELECT PlotID, PartOfPlotID, PlotIdentifier " +
                "FROM dbo.SamplingPlotHierarchy(" + PlotID.ToString() + ")";
            this._dtSamplingPlot = new DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotDataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(_dtSamplingPlot);
            System.Data.DataRow[] RR = this._dtSamplingPlot.Select("PartOfPlotID IS NULL");
            if (RR.Length > 0)
            {
                System.Windows.Forms.TreeNode N = new TreeNode(RR[0]["PlotIdentifier"].ToString());
                N.Tag = RR[0];
                if (RR[0]["PlotID"].ToString() == PlotID.ToString())
                {
                    N.BackColor = System.Drawing.Color.Yellow;
                }
                this.treeViewHierarchy.Nodes.Add(N);
                this.addHierarchyChilds(N);
            }
            this.treeViewHierarchy.ExpandAll();
            treeViewHierarchy.Sort();


            // Bring current hierarchy node into view
            bool found = false;
            var nodesToCheck = new Queue<TreeNode>(this.treeViewHierarchy.Nodes.Cast<TreeNode>());
            while (nodesToCheck.Count > 0 && !found)
            {
                TreeNode N = nodesToCheck.Dequeue();
                if (N.BackColor == System.Drawing.Color.Yellow)
                {
                    N.EnsureVisible();
                    found = true;
                }
                else
                {
                    foreach (TreeNode child in N.Nodes)
                    {
                        nodesToCheck.Enqueue(child);
                    }
                }
            }            

            // userControlQueryList.RefreshHierarchy();

        }

        private void addHierarchyChilds(System.Windows.Forms.TreeNode Node)
        {
            DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotRow R = (DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotRow)Node.Tag;
            int PartOfPlotID = R.PlotID;
            foreach (DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotRow rr in this._dtSamplingPlot.Select("NOT PartOfPlotID IS NULL"))
            {
                try
                {
                    if (rr.PartOfPlotID.Equals(System.DBNull.Value)) continue;
                    if (rr.PartOfPlotID == PartOfPlotID)
                    {
                        System.Windows.Forms.TreeNode NN = new TreeNode(rr.PlotIdentifier.ToString());
                        NN.Tag = rr;
                        if (rr.PlotID.ToString() == this.PlotID.ToString()) NN.BackColor = System.Drawing.Color.Yellow;
                        Node.Nodes.Add(NN);
                        this.addHierarchyChilds(NN);
                    }
                }
                catch { }
            }
        }


        private void startSamplingPlotEditor()
        {
            // Set up list of default brushes
            List<System.Windows.Media.Brush> m_Brushes = new List<System.Windows.Media.Brush>();
            m_Brushes.Add(System.Windows.Media.Brushes.Red);
            m_Brushes.Add(System.Windows.Media.Brushes.Yellow);
            m_Brushes.Add(System.Windows.Media.Brushes.Blue);
            m_Brushes.Add(System.Windows.Media.Brushes.Magenta);
            m_Brushes.Add(System.Windows.Media.Brushes.Green);
            m_Brushes.Add(System.Windows.Media.Brushes.White);
            m_Brushes.Add(System.Windows.Media.Brushes.Cyan);

            List<GeoObject> GeoObjects = new List<GeoObject>();
            DiversityWorkbench.Forms.FormGeography editor = null;

            int count = _dtGeo.Rows.Count;
            int index = 0;
            foreach (DataRow row in _dtGeo.Rows)
            {
                if (index >= m_Brushes.Count)
                    index = 0;
                string geoData = row[1].ToString();
                if (geoData.Length > 0)
                    GeoObjects.Add(setupGeoObject(geoData, m_Brushes[index++]));
            }

            if (GeoObjects.Count > 0)
                editor = new DiversityWorkbench.Forms.FormGeography(GeoObjects);
            else
                editor = new DiversityWorkbench.Forms.FormGeography();

            editor.ShowDialog();
            if (editor.DialogResult == DialogResult.OK)
            {
                // Missing: Updating database with changed or new GeoObjects!
                if (editor.ReceiveGeoObjects(out GeoObjects))
                    updateDatabase(GeoObjects);
            }
            editor = null;
            //this.userControlGoogleMaps.web Browser.Refresh();
            //this.userControlGIS.

        }

        // ===WR===
        private void initGisMap()
        {
            if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0
                && this._dtGeo.Rows.Count > 0)
            {
                this.userControlGeography.DataRowSamplingPlot = this.dataSetSamplingPlot.SamplingPlot.Rows[0];
                this.userControlGeography.DataRowSamplingPlot_GeoCache = this._dtGeo.Rows[0];
            }
            else
            {
                this.userControlGeography.DataRowSamplingPlot_GeoCache = null;
                this.userControlGeography.DataRowSamplingPlot = null;
            }

            this.userControlGeography.ShowPlots();


            //if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0
            //    && this._dtGeo.Rows.Count > 0)
            //{
            //    this.userControlGIS.DataRowSamplingPlot = this.dataSetSamplingPlot.SamplingPlot.Rows[0];
            //    this.userControlGIS.DataRowSamplingPlot_GeoCache = this._dtGeo.Rows[0];
            //}
            //else
            //{
            //    this.userControlGIS.DataRowSamplingPlot_GeoCache = null;
            //    this.userControlGIS.DataRowSamplingPlot = null;
            //}

            //this.userControlGIS.ShowPlots();

        }

        // Dummy so far... ??
        private void updateDatabase(List<GeoObject> GeoObjects)
        {
            if (GeoObjects.Count > 0)
            {
                string SQL = "UPDATE SamplingPlot SET PlotGeography_Cache = geography::STGeomFromText('" + GeoObjects[0].GeometryData + "', 4326) " +
                    "WHERE PlotID = " + PlotID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
        }

        // Fill up GeoObject structure...
        // Where to save colors, transparency and so on?
        private GeoObject setupGeoObject(string data, System.Windows.Media.Brush brush)
        {
            GeoObject geoObject = new GeoObject();
            geoObject.Identifier = this.textBoxHeaderID.Text;  // this.textBoxHeaderPlotIdentifier.Text
            geoObject.DisplayText = this.textBoxPlotDescription.Text;
            geoObject.StrokeBrush = brush;
            geoObject.FillBrush = brush;
            geoObject.StrokeTransparency = 255;
            geoObject.FillTransparency = 64;
            geoObject.StrokeThickness = 1.0;
            geoObject.PointType = WpfSamplingPlotPage.PointSymbol.None;
            geoObject.PointSymbolSize = 0;
            geoObject.GeometryData = data;
            return geoObject;
        }

        #endregion // Methods

        #region Localization

        private void fillLocalisationList()
        {
            this._dtLocalisationList.Clear();
            string SQL = "SELECT S.LocalisationSystemID, L.DisplayText " +
                "FROM SamplingPlotLocalisation AS S INNER JOIN " +
                "LocalisationSystem AS L ON S.LocalisationSystemID = L.LocalisationSystemID " +
                "WHERE S.PlotID =  " + this.PlotID.ToString() +
                " ORDER BY L.DisplayText";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this._dtLocalisationList);
            if (this.listBoxLocalisation.DataSource == null)
            {
                this.listBoxLocalisation.DataSource = this._dtLocalisationList;
                this.listBoxLocalisation.DisplayMember = "DisplayText";
                this.listBoxLocalisation.ValueMember = "LocalisationSystemID";
            }
        }

        private void setLocalisationControls(System.Data.DataRow DataRow)
        {
            string ParsingMethodName;
            string LocalisationSystem;
            int LocalisationSystemID = 0;
            if (int.TryParse(DataRow["LocalisationSystemID"].ToString(), out LocalisationSystemID))
            {
                ParsingMethodName = DiversitySamplingPlots.LookupTable.ParsingMethodName(LocalisationSystemID);
                LocalisationSystem = DiversitySamplingPlots.LookupTable.LocalisationSystemName(LocalisationSystemID);
                try
                {
                    System.Data.DataRow[] rr = DiversitySamplingPlots.LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + LocalisationSystemID.ToString());
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        this.labelLocation1.Text = r["DisplayTextLocation1"].ToString();
                        if (this.labelLocation1.Text.Length > 11)
                            this.labelLocation1.Text = this.labelLocation1.Text.Substring(0, 10) + ".";
                        this.labelLocation2.Text = r["DisplayTextLocation2"].ToString();
                        if (this.labelLocation2.Text.Length > 11)
                            this.labelLocation2.Text = this.labelLocation2.Text.Substring(0, 10) + ".";
                        this.toolTip.SetToolTip(this.textBoxLocation1, r["DescriptionLocation1"].ToString());
                        this.toolTip.SetToolTip(this.textBoxLocation2, r["DescriptionLocation2"].ToString());

                        this.labelLocation1Custom.Text = r["DisplayTextLocation1"].ToString();
                        this.labelLocation2Custom.Text = r["DisplayTextLocation2"].ToString();
                        this.toolTip.SetToolTip(this.textBoxLocation1a, r["DescriptionLocation1"].ToString());
                        this.toolTip.SetToolTip(this.textBoxLocation2a, r["DescriptionLocation2"].ToString());

                        this.setGeographyCustomUnitList(ParsingMethodName);

                        if (this.comboBoxGeographyLocationUnits.Items.Count > 0) this.comboBoxGeographyLocationUnits.Visible = true;
                        else this.comboBoxGeographyLocationUnits.Visible = false;
                        this.buttonGetCoordinatesFromGoogleMaps.Visible = false;
                        this.buttonCoordinatesConvert.Visible = false;
                        this.labelGeography.Visible = false;

                        switch (ParsingMethodName)
                        {
                            case "Altitude":
                                this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                this.tableLayoutPanelGeographyLocation.Visible = true;

                                this.labelLocationDirection.Visible = false;
                                this.textBoxLocationDirection.Visible = false;
                                this.labelLocationDistance.Visible = false;
                                this.textBoxLocationDistance.Visible = false;

                                break;
                            case "Coordinates":
                                this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                this.tableLayoutPanelGeographyLocation.Visible = true;
                                this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                if (LocalisationSystem == "Coordinates WGS84")
                                {
                                    this.buttonGetCoordinatesFromGoogleMaps.Visible = true;
                                    this.labelGeography.Visible = true;
                                }
                                this.buttonCoordinatesConvert.Visible = true;
                                goto default;
                            case "Gazetteer":
                                this.tableLayoutPanelGeographyLocation.Visible = false;
                                this.tableLayoutPanelGeographyLocationStandard.Visible = false;
                                this.userControlModuleRelatedEntryGazetteer.Visible = true;
                                goto default;
                            case "RD":
                            case "GK":
                                this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                this.tableLayoutPanelGeographyLocation.Visible = true;
                                this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                goto default;
                            case "MTB":
                                this.buttonGetCoordinatesFromGoogleMaps.Visible = true;
                                this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                this.tableLayoutPanelGeographyLocation.Visible = true;
                                this.tableLayoutPanelGeographyLocationStandard.Visible = true;
                                goto default;
                            case "Height":
                            case "Depth":
                            case "Exposition":
                            case "Slope":
                                this.userControlModuleRelatedEntryGazetteer.Visible = false;
                                this.tableLayoutPanelGeographyLocation.Visible = true;
                                this.tableLayoutPanelGeographyLocationStandard.Visible = true;

                                this.labelLocationDirection.Visible = false;
                                this.textBoxLocationDirection.Visible = false;
                                this.labelLocationDistance.Visible = false;
                                this.textBoxLocationDistance.Visible = false;

                                break;
                            case "Top50":
                                goto default;
                            default:
                                this.labelLocationDirection.Visible = true;
                                this.textBoxLocationDirection.Visible = true;
                                this.labelLocationDistance.Visible = true;
                                this.textBoxLocationDistance.Visible = true;
                                break;
                        }

                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void TryToSaveAndCalculateAverageForLocalisation(bool FromDataRow)
        {
            CultureInfo InvC = new CultureInfo("");

            string Loc1 = this.textBoxLocation1.Text;
            string Loc2 = this.textBoxLocation2.Text;
            System.Data.DataRowView RV = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
            if (FromDataRow)
            {
                if (RV["Location1"].Equals(System.DBNull.Value)) Loc1 = "";
                else Loc1 = RV["Location1"].ToString();
                if (RV["Location2"].Equals(System.DBNull.Value)) Loc2 = "";
                else Loc2 = RV["Location2"].ToString();
            }
            int LocSysID;
            if (!int.TryParse(RV["LocalisationSystemID"].ToString(), out LocSysID)) return;
            string LocalisationSystem = DiversitySamplingPlots.LookupTable.LocalisationSystemName(LocSysID);
            switch (LocalisationSystem)
            {
                case "mNN (barometric)":
                case "Altitude (mNN)":
                    double? _AltFrom = null;
                    double? _AltTo = null;
                    double? _AltAvg = null;

                    double AltFrom;
                    double AltTo;
                    if (Loc1.Length == 0) _AltFrom = null;
                    else
                    {
                        if (!double.TryParse(Loc1, NumberStyles.Float, InvC, out AltFrom))
                        {
                            if (!double.TryParse(Loc1.Replace(",", "."), NumberStyles.Float, InvC, out AltFrom))
                                return;
                        }
                        _AltFrom = AltFrom;
                    }
                    if (Loc2.Length == 0) _AltTo = null;
                    else
                    {
                        if (!double.TryParse(Loc2, NumberStyles.Float, InvC, out AltTo))
                        {
                            if (!double.TryParse(Loc2.Replace(",", "."), NumberStyles.Float, InvC, out AltTo))
                                return;
                        }
                        _AltTo = AltTo;
                    }

                    if (_AltTo != null && _AltFrom != null) _AltAvg = (_AltFrom + _AltTo) / 2;
                    else if (_AltTo != null && _AltFrom == null) _AltAvg = _AltTo;
                    else if (_AltTo == null && _AltFrom != null) _AltAvg = _AltFrom;
                    //if (_AltAvg != null)
                    //{
                    //    RV["AverageAltitudeCache"] = _AltAvg;
                    //    if (this.labelAltitudeCache.Text != RV["AverageAltitudeCache"].ToString())
                    //        this.textBoxAltitudeCache.Text = RV["AverageAltitudeCache"].ToString();
                    //}
                    break;
                case "Coordinates":
                    break;
                default:
                    break;
            }
        }

        // Custom units

        private void setGeographyCustomUnitList(string ParsingMethodName)
        {
            this.comboBoxGeographyLocationUnits.Items.Clear();
            this.comboBoxGeographyLocationUnits.Text = "";
            string[] List = this.GeographyCustomUnitList(ParsingMethodName);
            foreach (string s in List)
            {
                this.comboBoxGeographyLocationUnits.Items.Add(s);
            }
            if (this.comboBoxGeographyLocationUnits.Items.Count > 0)
                this.comboBoxGeographyLocationUnits.SelectedIndex = 0;
        }

        private string[] GeographyCustomUnitList(string ParsingMethodName)
        {
            string[] List;

            switch (ParsingMethodName)
            {
                case "Altitude":
                case "Height":
                case "Depth":
                    List = new string[] { "text", MeasurementUnit.m.ToString(), MeasurementUnit.feet.ToString() };
                    break;
                case "Coordinates":
                    List = new string[] { "text", MeasurementUnit.numeric.ToString(), MeasurementUnit.deg_min_sec.ToString() };
                    break;
                case "Exposition":
                    List = new string[] { "text", MeasurementUnit.degree.ToString(), MeasurementUnit.orientation.ToString() };
                    break;
                case "Slope":
                    List = new string[] { "text", MeasurementUnit.degree.ToString(), MeasurementUnit.percent.ToString() }; // };
                    break;
                default:
                    List = new string[] { };
                    break;
            }
            return List;
        }

        private void GeographyLocationSetNotesDateResponsible(System.Data.DataRow R, string OriginalValueNotes)
        {
            string LocationNotes = R["LocationNotes"].ToString();
            if (OriginalValueNotes.Length > 0)
                OriginalValueNotes = this.GeoOriValStart + OriginalValueNotes + this.GeoOriValEnd;
            if (LocationNotes.IndexOf(this.GeoOriValStart) > -1
                && LocationNotes.IndexOf(this.GeoOriValEnd) > LocationNotes.IndexOf(this.GeoOriValStart))
            {
                OriginalValueNotes = LocationNotes.Substring(0, LocationNotes.IndexOf(this.GeoOriValStart)) +
                    OriginalValueNotes +
                    LocationNotes.Substring(LocationNotes.IndexOf(this.GeoOriValEnd) + 1);
            }
            else
            {
                if (LocationNotes.Length > 0)
                    OriginalValueNotes += " " + LocationNotes.Trim();
            }
            R["LocationNotes"] = OriginalValueNotes.Trim();
            R["DeterminationDate"] = System.DateTime.Now.ToShortDateString();
        }

        private void setGeographyCustomUnitControls()
        {
            CultureInfo InvC = new CultureInfo("");

            double Value1 = 0;
            double Value2 = 0;
            bool OK1 = false;
            bool OK2 = false;
            string StringValue1 = "";
            string StringValue2 = "";
            if (this.samplingPlotLocalisationBindingSource.Current != null)
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
                if (!r["Location1"].Equals(System.DBNull.Value))
                {
                    OK1 = double.TryParse(r["Location1"].ToString(), NumberStyles.Float, InvC, out Value1);
                    if (!OK1)
                        OK1 = double.TryParse(r["Location1"].ToString().Replace(".", ","), NumberStyles.Float, InvC, out Value1);
                    if (!OK1)
                        OK1 = double.TryParse(r["Location1"].ToString().Replace(",", "."), NumberStyles.Float, InvC, out Value1);
                    StringValue1 = r["Location1"].ToString();
                }
                if (!r["Location2"].Equals(System.DBNull.Value))
                {
                    OK2 = double.TryParse(r["Location2"].ToString(), NumberStyles.Float, InvC, out Value2);
                    if (!OK2)
                        OK2 = double.TryParse(r["Location2"].ToString().Replace(".", ","), NumberStyles.Float, InvC, out Value2);
                    if (!OK2)
                        OK2 = double.TryParse(r["Location2"].ToString().Replace(",", "."), NumberStyles.Float, InvC, out Value2);
                    StringValue2 = r["Location2"].ToString();
                }
            }
            string GeographicalUnit = this.comboBoxGeographyLocationUnits.Text;

            this.textBoxLocation1a.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation2a.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation1b.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation2b.TextAlign = HorizontalAlignment.Center;
            this.textBoxLocation1a.Visible = true;
            this.textBoxLocation2a.Visible = true;
            this.comboBoxLocation2a.Visible = false;
            this.comboBoxLocation1a.Visible = false;

            switch (GeographicalUnit)
            {
                case "numeric":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    if (OK1) this.textBoxLocation1a.Text = Value1.ToString(); else this.textBoxLocation1a.Text = "?";
                    if (OK2) this.textBoxLocation2a.Text = Value2.ToString(); else this.textBoxLocation2a.Text = "?";
                    break;
                case "deg_min_sec":
                    int W = this.tableLayoutPanelGeographyLocationCustom.Width - this.labelLocation1Custom.Width - this.labelLocation2Custom.Width - 40;
                    int Wc = (int)((double)W * 0.2);
                    this.textBoxLocation1c.Width = Wc;
                    this.textBoxLocation2c.Width = Wc;
                    this.toolTip.SetToolTip(this.textBoxLocation1a, "Degree EW");
                    this.toolTip.SetToolTip(this.textBoxLocation2a, "Degree NS");
                    this.toolTip.SetToolTip(this.textBoxLocation1b, "Minutes EW");
                    this.toolTip.SetToolTip(this.textBoxLocation1c, "Seconds EW");
                    this.toolTip.SetToolTip(this.textBoxLocation2b, "Minutes NS");
                    this.toolTip.SetToolTip(this.textBoxLocation2c, "Seconds NS");
                    this.textBoxLocation1a.TextAlign = HorizontalAlignment.Right;
                    this.textBoxLocation2a.TextAlign = HorizontalAlignment.Right;
                    this.textBoxLocation1b.TextAlign = HorizontalAlignment.Center;
                    this.textBoxLocation2b.TextAlign = HorizontalAlignment.Center;
                    this.textBoxLocation1b.Visible = true;
                    this.textBoxLocation1c.Visible = true;
                    this.textBoxLocation2b.Visible = true;
                    this.textBoxLocation2c.Visible = true;
                    this.buttonGeographyCustomSave.Visible = true;
                    if (OK1)
                    {
                        int Prefix = 1;
                        int Deg = 0;
                        int Min = 0;
                        double Sek = 0;
                        this.convertFromNumericToDegMinSec(Value1, ref Prefix, ref Deg, ref Min, ref Sek);
                        string sDeg = Deg.ToString();
                        if (Prefix < 0) sDeg = "- " + sDeg;
                        this.textBoxLocation1a.Text = sDeg;
                        if (Min > 0) this.textBoxLocation1b.Text = Min.ToString();
                        else
                        {
                            if (Sek > 0) this.textBoxLocation1b.Text = "0";
                            else this.textBoxLocation1b.Text = "";
                        }
                        if (Sek > 0) this.textBoxLocation1c.Text = Sek.ToString();
                        else this.textBoxLocation1c.Text = "";
                    }
                    else
                    {
                        this.textBoxLocation1a.Text = "";
                        this.textBoxLocation1b.Text = "";
                        this.textBoxLocation1c.Text = "";
                    }
                    if (OK2)
                    {
                        int Prefix = 1;
                        int Deg = 0;
                        int Min = 0;
                        double Sek = 0;
                        this.convertFromNumericToDegMinSec(Value2, ref Prefix, ref Deg, ref Min, ref Sek);
                        string sDeg = Deg.ToString();
                        if (Prefix < 0) sDeg = "- " + sDeg;
                        this.textBoxLocation2a.Text = sDeg;
                        if (Min > 0) this.textBoxLocation2b.Text = Min.ToString();
                        else
                        {
                            if (Sek > 0) this.textBoxLocation2b.Text = "0";
                            else this.textBoxLocation2b.Text = "";
                        }
                        if (Sek > 0) this.textBoxLocation2c.Text = Sek.ToString();
                        else this.textBoxLocation2c.Text = "";
                    }
                    else
                    {
                        this.textBoxLocation2a.Text = "";
                        this.textBoxLocation2b.Text = "";
                        this.textBoxLocation2c.Text = "";
                    }
                    break;
                case "degree":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    if (OK1) this.textBoxLocation1a.Text = Value1.ToString();
                    else
                    {
                        if (StringValue1.Length < 4)
                        {
                            double Deg = 0;
                            string Ori = StringValue1;
                            if (this.convertFromOrientationToDegree(ref Ori, ref Deg))
                                this.textBoxLocation1a.Text = Deg.ToString();
                        }
                    }
                    if (OK2) this.textBoxLocation2a.Text = Value2.ToString();
                    else
                    {
                        if (StringValue2.Length < 4)
                        {
                            double Deg = 0;
                            string Ori = StringValue2;
                            if (this.convertFromOrientationToDegree(ref Ori, ref Deg))
                                this.textBoxLocation2a.Text = Deg.ToString();
                        }
                    }
                    break;
                case "orientation":
                    this.textBoxLocation1a.Visible = false;
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2a.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.comboBoxLocation1a.Width = 80;
                    this.comboBoxLocation2a.Width = 80;
                    this.comboBoxLocation1a.Visible = true;
                    this.comboBoxLocation2a.Visible = true;
                    this.comboBoxLocation1a.Items.Clear();
                    this.comboBoxLocation2a.Items.Clear();
                    this.comboBoxLocation1a.Items.Add("");
                    this.comboBoxLocation2a.Items.Add("");
                    for (int i = 0; i < this.NamedOrientation.Length; i++)
                    {
                        this.comboBoxLocation1a.Items.Add(this.NamedOrientation[i]);
                        this.comboBoxLocation2a.Items.Add(this.NamedOrientation[i]);
                    }
                    this.buttonGeographyCustomSave.Visible = true;
                    string OrientFrom = this.convertFromDegreeToOrientation(StringValue1);
                    string OrientTo = this.convertFromDegreeToOrientation(StringValue2);
                    for (int i = 0; i < this.NamedOrientation.Length; i++)
                    {
                        if (this.NamedOrientation[i] == OrientFrom)
                        {
                            this.comboBoxLocation1a.SelectedIndex = i + 1;
                            break;
                        }
                    }
                    for (int i = 0; i < this.NamedOrientation.Length; i++)
                    {
                        if (this.NamedOrientation[i] == OrientTo)
                        {
                            this.comboBoxLocation2a.SelectedIndex = i + 1;
                            break;
                        }
                    }
                    break;
                case "percent":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    this.textBoxLocation1a.Text = this.convertUnit(StringValue1, MeasurementUnit.degree, MeasurementUnit.percent);
                    this.textBoxLocation2a.Text = this.convertUnit(StringValue2, MeasurementUnit.degree, MeasurementUnit.percent);
                    break;
                case "feet":
                case "foot":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    this.textBoxLocation1a.Text = this.convertUnit(StringValue1, MeasurementUnit.m, MeasurementUnit.feet);
                    this.textBoxLocation2a.Text = this.convertUnit(StringValue2, MeasurementUnit.m, MeasurementUnit.feet);
                    break;
                case "m":
                    this.textBoxLocation1b.Visible = false;
                    this.textBoxLocation1c.Visible = false;
                    this.textBoxLocation2b.Visible = false;
                    this.textBoxLocation2c.Visible = false;
                    this.buttonGeographyCustomSave.Visible = true;
                    this.textBoxLocation1a.Text = this.convertUnit(StringValue1, MeasurementUnit.m, MeasurementUnit.m);
                    this.textBoxLocation2a.Text = this.convertUnit(StringValue2, MeasurementUnit.m, MeasurementUnit.m);
                    break;
                default:
                    this.textBoxLocation1a.Visible = true;
                    this.textBoxLocation2a.Visible = true;
                    this.buttonGeographyCustomSave.Visible = false;
                    this.comboBoxGeographyLocationUnits.Visible = true;
                    break;
            }
        }

        // Conversions

        /// <summary>
        /// converts all values to or from SI unit like m, sec
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="UnitSource"></param>
        /// <param name="UnitTarget"></param>
        /// <returns></returns>
        private string convertUnit(string Value, MeasurementUnit UnitSource, MeasurementUnit UnitTarget)
        {
            string Result = "?";
            double Source = 0;
            decimal dResult = 0;
            double Target = 0;
            bool OK = false;
            switch (UnitSource)
            {
                case MeasurementUnit.feet:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Target = Source / 3.2808;
                        Result = Target.ToString();
                    }
                    break;
                case MeasurementUnit.mile:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Target = Source * 1609;
                        Result = Target.ToString();
                    }
                    break;
                case MeasurementUnit.km:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Target = Source * 1000.0;
                        Result = Target.ToString();
                    }
                    break;
                case MeasurementUnit.m:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.feet:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = Source * 3.2808;
                                goto default;
                            }
                            break;
                        case MeasurementUnit.mile:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = Source / 1609;
                                goto default;
                            }
                            break;
                        case MeasurementUnit.km:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = Source / 1000.0;
                                goto default;
                            }
                            break;
                        default:
                            if (Target == 0)
                            {
                                OK = double.TryParse(Value, out Source);
                                if (OK)
                                {
                                    Target = Source;
                                }
                            }
                            dResult = (decimal)Target;
                            dResult = System.Math.Round(dResult, 9);
                            Target = (double)dResult;
                            Result = Target.ToString();
                            break;
                    }
                    break;
                case MeasurementUnit.percent:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.degree:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                double tan = Source / 100.0;
                                double result = System.Math.Atan(tan);
                                double angle = result * 180 / Math.PI;
                                Result = angle.ToString();
                            }
                            break;
                    }
                    break;
                case MeasurementUnit.degree:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.percent:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                Target = System.Math.Tan(Source * (Math.PI / 180.0)) * 100.0;
                                Result = Target.ToString();
                            }
                            break;
                        case MeasurementUnit.orientation:
                            OK = double.TryParse(Value, out Source);
                            if (OK)
                            {
                                int ori = (int)((Source + 11.25) / 22.5);
                                Result = this.NamedOrientation[ori];
                            }
                            break;
                    }
                    break;
                default:
                    OK = double.TryParse(Value, out Source);
                    if (OK)
                    {
                        Result = Source.ToString();
                    }
                    break;
            }
            return Result;
        }

        /// <summary>
        /// converts all values to or from SI unit like m, sec
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="UnitSource"></param>
        /// <param name="UnitTarget"></param>
        /// <returns></returns>
        private double convertUnit(double Value, MeasurementUnit UnitSource, MeasurementUnit UnitTarget)
        {
            switch (UnitSource)
            {
                case MeasurementUnit.feet:
                    return Value / 3.2808;

                case MeasurementUnit.mile:
                    return Value * 1609;

                case MeasurementUnit.km:
                    return Value * 1000.0;

                case MeasurementUnit.m:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.feet:
                            return Value * 3.2808;

                        case MeasurementUnit.mile:
                            return Value / 1609;

                        case MeasurementUnit.km:
                            return Value / 1000.0;

                        default:
                            break;
                    }
                    return Value;

                case MeasurementUnit.percent:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.degree:
                            double tan = Value / 100.0;
                            double result = System.Math.Atan(tan);
                            double angle = result * 180 / Math.PI;
                            return angle;
                    }
                    break;

                case MeasurementUnit.degree:
                    switch (UnitTarget)
                    {
                        case MeasurementUnit.percent:
                            return System.Math.Tan(Value * (Math.PI / 180.0)) * 100.0;
                    }
                    break;

                default:
                    return Value;
            }
            return Value;
        }

        private bool convertFromNumericToDegMinSec(double Numeric, ref int Prefix, ref int Deg, ref int Min, ref double Sec)
        {
            bool OK = true;
            try
            {
                //Prefix
                if (Numeric < 0) Prefix = -1;
                else Prefix = 1;
                // Deg
                Deg = (int)Numeric * Math.Sign(Numeric);
                // Min
                double MinD = Numeric * Math.Sign(Numeric) - Deg;
                MinD = MinD * 0.6;
                MinD = ((MinD * 100));
                Min = (int)MinD;
                // Sec
                double SecD = MinD - Min;
                SecD *= 60;
                decimal dSec = (decimal)SecD;
                dSec = Math.Round(dSec, 9);
                SecD = (double)dSec;
                if (SecD >= 60)
                {
                    SecD -= 60;
                    Min += 1;
                }
                decimal dSecD = (decimal)SecD;
                dSecD = System.Math.Round(dSecD, 9);
                SecD = (double)dSecD;
                Sec = SecD;
                Deg = System.Math.Abs(Deg);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool convertFromDegMinSecToNumeric(ref double Numeric, int Deg, int Min, double Sec)
        {
            try
            {
                if (Deg * Math.Sign(Deg) > 180 || Min >= 60 || Min < 0 || Sec < 0 || Sec >= 60)
                {
                    System.Windows.Forms.MessageBox.Show("Wrong entry");
                    return false;
                }
                double dDeg = (double)Deg * Math.Sign(Deg);
                double dMin = (double)Min / (double)60 * Math.Sign(Min);
                double dSec = Sec / 3600;
                Numeric = dDeg + dMin + dSec;
                if (Deg != 0)
                    Numeric = Numeric * Math.Sign(Deg);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private bool convertFromOrientationToDegree(ref string Orientation, ref double Degree)
        {
            bool OK = true;
            int i;
            try
            {
                if (Orientation.Length > 0 && Orientation.Length < 4)
                {
                    Degree = 0F;
                    Orientation = Orientation.ToUpper();
                    for (i = 0; i < Orientation.Length; i++)
                    {
                        if (Orientation[i].ToString() == "N" || Orientation[i].ToString() == "W" || Orientation[i].ToString() == "E" || Orientation[i].ToString() == "S" || Orientation[i].ToString() == "O")
                        {
                            switch (Orientation[i].ToString())
                            {
                                case "N":
                                    if (Orientation.IndexOf("W") > 0)
                                        Degree += 360.0F;
                                    else
                                        Degree += 0F;
                                    break;
                                case "E":
                                case "O":
                                    Degree += 90.0F;
                                    break;
                                case "S":
                                    Degree += 180.0F;
                                    break;
                                case "W":
                                    Degree += 270.0F;
                                    break;
                                default:
                                    OK = false;
                                    break;
                            }
                        }
                        else
                        {
                            Orientation = "?";
                            break;
                        }
                    }
                    if (Orientation != "?")
                        Degree = Degree / i;
                }
            }
            catch
            {
                OK = false;
            }
            return OK;
        }

        private string convertFromDegreeToOrientation(string Degree)
        {
            string O = "?";
            try
            {
                float D;
                bool OK = float.TryParse(Degree, out D);
                if (OK)
                {
                    int ori = (int)((D + 11.25F) / 22.5F);
                    O = this.NamedOrientation[ori];
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return O;
        }

        #endregion // Localization

        #region Event handlers

        #region Form events

        /// <summary>
        /// Load --- Obsolete ---
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiversitySamplingPlots_Load(object sender, EventArgs e)
        {
            //#if !DEBUG
            //            this.tabControlDetails.TabPages.Remove(this.tabPageImages);
            //#endif

        }

        /// <summary>
        /// Close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDiversitySamplingPlots_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.askOnExitToolStripMenuItem.Checked)
            {
                if (MessageBox.Show("Do you really want to exit DiversitySamplingPlots?", "Exit application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Save last data
            this.savePlot(sender, e);

            // Save settings
            if (this.userControlQueryList.QueryConditionVisiblity.Length > 0)
                DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.QueryConditionVisibility = this.userControlQueryList.QueryConditionVisiblity;
            DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.SplitContainerData_SplitterDistance = this.splitContainerData.SplitterDistance;
            DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.AskOnExit = this.askOnExitToolStripMenuItem.Checked;
            DiversitySamplingPlots.Forms.FormDiversitySamplingPlotsSettings.Default.Save();
            //Markus 23.2.22 adaption for ManyOrderByColumns
            if (this.userControlQueryList.ManyOrderByColumns_Allow())
            {
                DiversityWorkbench.Settings.ManyOrderColumnSave(DiversityWorkbench.Settings.ModuleName, "FormDiversitySamplingPlots", this.userControlQueryList.ManyOrderByColumns_Widths());
            }

            try
            {
                DiversityWorkbench.Utilities.WebView2UserDataFolderCleanup.CleanupCurrentSessionFolder();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Update events


        // Update methods
        //private string _ClientVersionUsed;
        //private string ClientVersionUsed()
        //{
        //    if (this._ClientVersionUsed != null)
        //        return this._ClientVersionUsed;

        //    int VersionMajor = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Major;
        //    int VersionMinor = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Minor;
        //    int VersionBuild = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Build;
        //    int VersionRevision = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.Revision;
        //    if (VersionMajor < 10) _ClientVersionUsed += "0";
        //    _ClientVersionUsed += VersionMajor.ToString() + ".";
        //    if (VersionMinor < 10) _ClientVersionUsed += "0";
        //    _ClientVersionUsed += VersionMinor.ToString() + ".";
        //    if (VersionBuild < 10) _ClientVersionUsed += "0";
        //    _ClientVersionUsed += VersionBuild.ToString() + ".";
        //    if (VersionRevision < 10) _ClientVersionUsed += "0";
        //    _ClientVersionUsed += VersionRevision.ToString();
        //    return _ClientVersionUsed;
        //}

        #endregion // Update events

        #region Query events

        private void switchQueryOrientation(object sender, System.EventArgs e)
        {
            try
            {
                if (this.userControlQueryList.splitContainerMain.Orientation == Orientation.Vertical)
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance / 2;
                else
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance * 2;
                this.userControlQueryList.switchOrientation();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        // ===WR===

        private void listBoxQueryResult_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (this.userControlQueryList.ListOfSelectedIDs.Count > 1)
                {
                    // enable multiple plot selection selection 
                    if (userControlGeography != null)
                    {
                        userControlGeography._QueryListOfSelectedIDs = this.userControlQueryList.ListOfSelectedIDs;
                        this.userControlGeography.ShowPlots();
                    }
                    
                }
                else
                {
                    // delete old list of selected IDs in userControlGeography to avoid wrong selection
                    if (userControlGeography != null)
                    {
                        userControlGeography._QueryListOfSelectedIDs.Clear();
                    }
                    // Avoid GIS-Editor call for multiple IndexChanged events with same Plot
                    if (m_LastID != this.userControlQueryList.ID)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        this.SuspendLayout();
                        this.setSamplingPlot(this.userControlQueryList.ID);
                        this.ResumeLayout();
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        m_LastID = this.userControlQueryList.ID;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion // Query events

        #region Toolstrip events

        #region New plot events

        private void createNewPlot(object sender, System.EventArgs e)
        {
            try
            {
                if (this.userControlQueryList.ProjectID == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a project");
                    return;
                }
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New plot?", "Please enter the identifier for the new sampling plot", "");
                f.AcceptReturns(false);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.String.Length > 0)
                {
                    string PlotIdentifier = f.String;
                    int ID = this.InsertNewPlot(PlotIdentifier);
                    if (ID > -1)
                    {
                        this.setSamplingPlot(ID);

                        if (PlotIdentifier.Length == 0)
                        {
                            PlotIdentifier = "ID: " + ID.ToString();
                        }
                        //Markus 23.2.22 adaption for ManyOrderByColumns
                        if (this.userControlQueryList.ManyOrderByColumns())
                        {
                            string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(ID);
                            this.userControlQueryList.AddListItem(ID, Display);
                        }
                        else
                        {
                            this.userControlQueryList.AddListItem(ID, PlotIdentifier);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region obsolete
        //private void createNewPlot(int? ParentID)
        //{
        //    try
        //    {
        //        if (this.userControlQueryList.ProjectID == -1)
        //        {
        //            System.Windows.Forms.MessageBox.Show("Please select a project");
        //            return;
        //        }
        //        DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New plot?", "Please enter the identifier for the new sampling plot", "");
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK && f.String.Length > 0)
        //        {
        //            string PlotIdentifier = f.String;
        //            int ID = this.InsertNewPlot(PlotIdentifier);
        //            if (ID > -1)
        //            {
        //                this.setSamplingPlot(ID);
        //                if (PlotIdentifier.Length > 0)
        //                    this.userControlQueryList.AddListItem(ID, PlotIdentifier);
        //                else
        //                    this.userControlQueryList.AddListItem(ID, "ID: " + ID.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        #endregion // obsolete

        /// <summary>
        /// inserting a new specimen into the table SamplingPlot
        /// </summary>
        /// <returns>the PlotID of the new plot</returns>
        private int InsertNewPlot(string PlotIdentifier)
        {
            try
            {
                int ID = -1;
                string SQL = "INSERT INTO SamplingPlot (PlotIdentifier) VALUES ('" + PlotIdentifier + "') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                SQL = "INSERT INTO SamplingProject (PlotID, ProjectID) VALUES (" + ID.ToString() + ", " + this.userControlQueryList.ProjectID.ToString() + ")";
                cmd.CommandText = SQL;
                cmd.ExecuteNonQuery();
                con.Close();
                return ID;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
        }

        #endregion //  New plot events

        #region Save plot events

        private void savePlot(object sender, System.EventArgs e)
        {
            // move to a neutral control to ensure that the last changes are stored
            this.textBoxHeaderID.Focus();

            this.updatePlot();
        }

        #endregion // Save plot events

        #region Undo plot changes

        private void undoChangesInPlot(object sender, System.EventArgs e)
        {
            try
            {
                int i = System.Int32.Parse(this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotID"].ToString());
                this.dataSetSamplingPlot.Clear();
                this.setSamplingPlot(i);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion // Undo plot changes

        #region Copy plot events

        private void copySamplingPlot(object sender, System.EventArgs e)
        {
            string DisplayText = "copy of ";
            if (this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"] == System.DBNull.Value)
                DisplayText += this.dataSetSamplingPlot.SamplingPlot.Rows[0][0].ToString();
            else
                DisplayText += this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].ToString();
            int OriginalID = (int)this.PlotID;
            try
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to create a " + DisplayText, "Copy sampling plot?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    string PlotIdentifier = DisplayText;
                    int ID = this.CopyPlot(OriginalID, ref PlotIdentifier);
                    //this.userControlQueryList.AddListItem(ID, PlotIdentifier);
                    //Markus 23.2.22 adaption for ManyOrderByColumns
                    if (this.userControlQueryList.ManyOrderByColumns())
                    {
                        string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(ID);
                        this.userControlQueryList.AddListItem(ID, Display);
                    }
                    else
                        this.userControlQueryList.AddListItem(ID, PlotIdentifier);

                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        // Copy plot methods

        /// <summary>
        /// Copy the data of a specimen into a new dataset
        /// </summary>
        /// <param name="OriginalID">The ID of the dataset that should be copied</param>
        /// <param name="PlotIdentifier">the PlotIdentifier of the new specimen</param>
        /// <returns>the ID of the new specimen</returns>
        private int CopyPlot(int OriginalID, ref string PlotIdentifier)
        {
            string SQL = "execute dbo.procInsertSamplingPlotCopy NULL , " + OriginalID.ToString() + ", '" + PlotIdentifier + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            int ID = 0;
            try
            {
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                this.setSamplingPlot(ID);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            con.Close();
            return ID;
        }

        #region obsolete: 

        /// <summary>
        /// create a copy of a sampling plot
        /// </summary>
        /// <param name="OriginalID">The PlotID of the original dataset</param>
        /// <returns>The ID of the new plot</returns>
        private int CopyPlot(int OriginalID)
        {
            string SQL = "";
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

        #endregion // obsolete

        #endregion // Copy plot events

        #region Delete plot event

        private void deletePlot(object sender, System.EventArgs e)
        {
            if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0)
            {
                int ID = (int)this.PlotID;
                string SQL = "SELECT COUNT(*) FROM SamplingPlot WHERE PartOfPlotID = " + ID.ToString();
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString() != "0")
                {
                    System.Windows.Forms.MessageBox.Show("You can not delete this plot as long as there are inferior plots depending on the current plot");
                    return;
                }
                SQL = "PlotID = " + ID.ToString();
                //if (System.Windows.Forms.MessageBox.Show("Do you want to check for datasets in other modules that refer to this plot?", "Check module relations", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //{
                //    DiversityWorkbench.FormDatabaseConnection f = new DiversityWorkbench.FormDatabaseConnection();
                //    f.ShowDialog();
                //    if (f.DialogResult == DialogResult.OK)
                //    {

                //    }
                //}
                string DisplayTextDel = this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].ToString();
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to delete the plot\r\n\r\n" + DisplayTextDel, "Delete plot?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.userControlQueryList.RemoveSelectedListItem();
                        this.deletePlot(ID);
                        this.dataSetSamplingPlot.Clear();
                        // this.splitContainerData.Visible = false;
                        userControlQueryList.RefreshHierarchy();
                        this.userControlQueryList.listBoxQueryResult.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                if (this.userControlQueryList.listBoxQueryResult.Items.Count > 0)
                {
                    if (this.userControlQueryList.listBoxQueryResult.SelectedIndex > -1)
                        System.Windows.Forms.MessageBox.Show("Please select a Plot first!");
                }
            }
        }

        // Delete plot methods

        /// <summary>
        /// delete a name from the database
        /// </summary>
        /// <param name="NameID">the Primary key of table TaxonName corresponding to the name that should be deleted</param>
        private void deletePlot(int ID)
        {
            try
            {
                string SQL = "DELETE FROM SamplingProject WHERE PlotID = " + ID.ToString();
                Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
                com.CommandType = System.Data.CommandType.Text;
                Conn.Open();
                com.ExecuteNonQuery();
                SQL = "DELETE FROM SamplingPlotProperty WHERE PlotID = " + ID.ToString();
                com.CommandText = SQL;
                com.ExecuteNonQuery();
                SQL = "DELETE FROM SamplingPlotLocalisation WHERE PlotID = " + ID.ToString();
                com.CommandText = SQL;
                com.ExecuteNonQuery();
                SQL = "DELETE FROM SamplingPlot WHERE PlotID = " + ID.ToString();
                com.CommandText = SQL;
                com.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion // Delete plot event

        /// <summary>
        /// Fill up plot properties with properties from upper plots in hierachy
        /// </summary>
        /// <returns>0 if successful.</returns>
        private void FillPlotProperties()
        {
            //
            string SQL = "execute dbo.procSamplingPlotInheritAllProperties";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                // result = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: Properties fill up failed!", "ERROR");
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            con.Close();
            //
        }

        #endregion // Toolstrip events

        #region Hierarchy events

        /// <summary>
        /// (Not used)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (this.ShowMap)
            //    this.showGeographyMap();
        }

        /// <summary>
        /// Hierarchy new plot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPlotNew_Click(object sender, EventArgs e)
        {
            if (this.treeViewHierarchy.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a plot in which the new plot is located");
                return;
            }
            if (treeViewHierarchy.SelectedNode.Tag == null) return;
            System.Data.DataRow R = (System.Data.DataRow)treeViewHierarchy.SelectedNode.Tag;
            string PlotIdentifier = R["PlotIdentifier"].ToString();
            int ParentID = 0;
            if (int.TryParse(R["PlotID"].ToString(), out ParentID))
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New plot", "Please enter the identifier for the new plot", PlotIdentifier);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.String.Length > 0)
                {
                    if (f.String == PlotIdentifier)
                    {
                        System.Windows.Forms.MessageBox.Show("Plot identifiers should be unique!");
                        return;
                    }
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("Select count(*) from SamplingPlot where PlotIdentifier = '" + f.String + "'", con);
                    int i = 0;
                    con.Open();
                    if (!int.TryParse(C.ExecuteScalar().ToString(), out i))
                    {
                        if (i > 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Plot identifiers should be unique!");
                            return;
                        }
                    }
                    con.Close();
                    int ID = this.InsertNewPlot(f.String, ParentID);
                    this.setSamplingPlot(ID);
                    //DiversitySamplingPlots.DataSetSamplingPlot.SamplingPlotRow RChild = this.dataSetSamplingPlot.SamplingPlot.NewSamplingPlotRow();
                    //RChild.PartOfPlotID = ParentID;
                    //RChild.PlotIdentifier = f.String;
                    //this.dataSetSamplingPlot.SamplingPlot.Rows.Add(RChild);
                }
            }
        }

        /// <summary>
        /// Hierarchy change plot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonHierarchyFind_Click(object sender, EventArgs e)
        {
            if (this.treeViewHierarchy.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a plot");
                return;
            }
            int PlotIDofNode = 0;
            if (treeViewHierarchy.SelectedNode.Tag == null) return;

            System.Data.DataRow R = (System.Data.DataRow)treeViewHierarchy.SelectedNode.Tag;
            if (int.TryParse(R["PlotID"].ToString(), out PlotIDofNode))
            {
                this.setSamplingPlot(PlotIDofNode);
            }
        }

        /// <summary>
        /// Hierarchy set superior plot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonHierarchySetParent_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.userControlQueryList.ProjectIsSelected)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a project");
                    return;
                }
                System.Data.DataTable dtChilds = new DataTable();
                string SQL = "SELECT PlotID " +
                    "FROM dbo.SamplingPlotChildNodes(" + this.PlotID.ToString() + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtChilds);
                SQL = "";
                foreach (System.Data.DataRow R in dtChilds.Rows)
                {
                    if (SQL.Length == 0) SQL = " AND S.PlotID NOT IN (";
                    else SQL += ", ";
                    SQL += R[0].ToString();
                }
                if (SQL.Length > 0) SQL += ") ";

                System.Data.DataTable dt = new DataTable();
                ad.SelectCommand.CommandTimeout = 60000;
                ad.SelectCommand.CommandText = "SELECT S.PlotID, S.DisplayText " +
                    " FROM dbo.SamplingPlotList() AS S INNER JOIN " +
                    " SamplingProject AS P ON S.PlotID = P.PlotID " +
                    " WHERE P.ProjectID =  " + this.userControlQueryList.ProjectID +
                    " AND P.PlotID <> " + this.PlotID.ToString() +
                    SQL +
                    " ORDER BY S.DisplayText";
                ad.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No sampling plots found");
                    return;
                }
                string Header = "Please select the plot of which the";
                if (!this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].Equals(System.DBNull.Value)
                    && this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].ToString().Length > 0)
                    Header += " plot " + this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].ToString();
                else Header += " current plot ";
                Header += " is a part of";
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "PlotID", "Superior Plot", Header);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    int PartOfPlotID = int.Parse(f.SelectedValue);
                    this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PartOfPlotID"] = PartOfPlotID;
                    this.updatePlot();
                    this.buildHierarchy(this.PlotID);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Hierarchy remove superior plot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonHierarchyRemoveParent_Click(object sender, EventArgs e)
        {
            this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PartOfPlotID"] = System.DBNull.Value;
            this.updatePlot();
            this.buildHierarchy(this.PlotID);
        }

        // Hierarchy methods

        /// <summary>
        /// inserting a new specimen into the table SamplingPlot
        /// </summary>
        /// <returns>the PlotID of the new plot</returns>
        private int InsertNewPlot(string PlotIdentifier, int ParentID)
        {
            try
            {
                int ID = -1;
                string SQL = "INSERT INTO SamplingPlot (PartOfPlotID, PlotIdentifier) VALUES (" + ParentID.ToString() + ", '" + PlotIdentifier + "') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                SQL = "INSERT INTO SamplingProject (PlotID, ProjectID) VALUES (" + ID.ToString() + ", " + this.userControlQueryList.ProjectID.ToString() + ")";
                cmd.CommandText = SQL;
                cmd.ExecuteNonQuery();
                con.Close();
                return ID;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return -1;
            }
        }

        #endregion

        #region Sampling plot events

        private void comboBoxPlotType_DropDown(object sender, EventArgs e)
        {
            // save current combobox entry
            string currentItem = comboBoxPlotType.Text;

            string SQL = "SELECT DISTINCT PlotType " +
                "FROM         SamplingPlot " +
                "WHERE     (PlotType <> N'') " +
                "ORDER BY PlotType";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxPlotType.DataSource = dt;
            this.comboBoxPlotType.DisplayMember = "PlotType";
            this.comboBoxPlotType.ValueMember = "PlotType";
            // preselect combobox entry in drop down list
            comboBoxPlotType.Text = currentItem;
        }

        #endregion

        #region Localisation events

        private string GetParsingMethodName()
        {
            string ParsingMethodName = "";
            // string LocalisationSystem = "";
            int LocalisationSystemID = 0;
            System.Data.DataRowView RLoc = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;

            // get the current Localisation system
            if (int.TryParse(RLoc["LocalisationSystemID"].ToString(), out LocalisationSystemID))
            {
                ParsingMethodName = DiversitySamplingPlots.LookupTable.ParsingMethodName(LocalisationSystemID);
                // LocalisationSystem = DiversitySamplingPlots.LookupTable.LocalisationSystemName(LocalisationSystemID);
            }
            return ParsingMethodName;
        }


        private bool GetCurrentCoordinates(string ParsingMethodName, out double la, out double lo)
        {
            // try to find existing Coordinates
            bool OK = false;
            la = 0.0;
            lo = 0.0;
            string Lat = this.textBoxLocation2.Text.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
            string Long = this.textBoxLocation1.Text.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
            if (Lat.Length > 0 && Long.Length > 0 && ParsingMethodName != "MTB")
            {
                OK = true;
                if (!double.TryParse(Lat, NumberStyles.Float, CultureInfo.InvariantCulture, out la))
                    OK = false;
                if (!double.TryParse(Long, NumberStyles.Float, CultureInfo.InvariantCulture, out lo))
                    OK = false;
            }

            if (!OK)
            {
                // try to find existing coordinates
                System.Data.DataRow[] rrC = this.dataSetSamplingPlot.SamplingPlotLocalisation.Select("AverageLatitudeCache <> 0 AND AverageLongitudeCache <> 0");
                if (rrC.Length > 0)
                {
                    OK = true;
                    if (!double.TryParse(rrC[0]["AverageLatitudeCache"].ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out la))
                        OK = false;
                    if (!OK)
                        OK = double.TryParse(rrC[0]["AverageLatitudeCache"].ToString().Replace(".", ","), NumberStyles.Float, CultureInfo.InvariantCulture, out la);
                    if (!OK)
                        OK = double.TryParse(rrC[0]["AverageLatitudeCache"].ToString().Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out la);
                    if (la > 180)
                        la = 0.0;

                    if (!double.TryParse(rrC[0]["AverageLongitudeCache"].ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out lo))
                        OK = false;
                    if (!OK)
                        OK = double.TryParse(rrC[0]["AverageLongitudeCache"].ToString().Replace(".", ","), NumberStyles.Float, CultureInfo.InvariantCulture, out lo);
                    if (!OK)
                        OK = double.TryParse(rrC[0]["AverageLongitudeCache"].ToString().Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out lo);
                    if (lo > 90)
                        lo = 0.0;
                }
            }

            return OK;
        }

        private async void buttonGetCoordinatesFromGoogleMaps_Click(object sender, EventArgs e)
        {
            double la = 0.0;
            double lo = 0.0;

            System.Data.DataRowView RLoc = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;

            string ParsingMethodName = GetParsingMethodName();

            bool OK = GetCurrentCoordinates(ParsingMethodName, out la, out lo);


            // get the correct form
            if (ParsingMethodName == "Coordinates")
            {
                DiversityWorkbench.Forms.FormGoogleMapsCoordinates f;

                if (OK) f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(la, lo);
                else f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(0.0, 0.0);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    try
                    {
                        if (f.Longitude <= 180 && f.Longitude >= -180 && f.Latitude <= 90 && f.Latitude >= -90 && f.LatitudeAccuracy != 0.0 && f.LongitudeAccuracy != 0.0)
                        {
                            _userCts = new CancellationTokenSource();
                            System.Collections.Generic.Dictionary<DiversityWorkbench.GeoFunctions.GeoInfo, string> GeoInfos = await DiversityWorkbench.GeoFunctions.getGeoInfosAsync(f.Latitude, f.Longitude, _userCts.Token);

                            string geoinfosource = "";
                            if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Source))
                            {
                                geoinfosource = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Source];
                            }
                            System.Data.DataRowView R = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
                            R["Location1"] = f.Longitude.ToString("F09", CultureInfo.InvariantCulture);
                            this.textBoxLocation1.Text = f.Longitude.ToString("F09", CultureInfo.InvariantCulture);
                            R["Location2"] = f.Latitude.ToString("F09", CultureInfo.InvariantCulture);
                            this.textBoxLocation2.Text = f.Latitude.ToString("F09", CultureInfo.InvariantCulture);
                            R["LocationAccuracy"] = f.Accuracy.ToString("F00", CultureInfo.InvariantCulture) + " m";
                            this.textBoxLocationAccuracy.Text = f.Accuracy.ToString("F00", CultureInfo.InvariantCulture) + " m";
                            if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Altitude))
                                R["AverageAltitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude]);
                            string Notes = "";
                            if (!R["LocationNotes"].Equals(System.DBNull.Value)) Notes = R["LocationNotes"].ToString();
                            if (Notes.Length > 0)
                            {
                                if (Notes.IndexOf("Derived from Google Maps") == -1)
                                    Notes += ". Derived from Google Maps";
                            }
                            else
                                Notes = "Derived from Google Maps";
                            R["LocationNotes"] = Notes;
                            this.textBoxLocationNotes.Text = Notes;

                            // insert Gazetteer entry if missing
                            System.Data.DataRow[] rrGaz = this.dataSetSamplingPlot.SamplingPlotLocalisation.Select("LocalisationSystemID = 7");
                            if (GeoInfos.Count > 0
                                && rrGaz.Length == 0
                                && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName))
                            {
                                DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow RGazetteer = this.dataSetSamplingPlot.SamplingPlotLocalisation.NewSamplingPlotLocalisationRow();
                                RGazetteer.PlotID = int.Parse(this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows[0]["PlotID"].ToString());
                                RGazetteer.LocalisationSystemID = 7;
                                string Place = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.City)
                                    && GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City] != GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName])
                                    Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision))
                                    Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Country))
                                    Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country];
                                RGazetteer.Location1 = Place;
                                RGazetteer.LocationNotes = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Source];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Distance))
                                {
                                    RGazetteer.LocationAccuracy = f.Accuracy.ToString("F00", CultureInfo.InvariantCulture) + " m";
                                    RGazetteer.DistanceToLocation = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Distance];
                                }
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Direction))
                                    RGazetteer.DirectionToLocation = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Direction];
                                this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows.Add(RGazetteer);
                            }
                            else if (GeoInfos.Count > 0
                                && rrGaz.Length == 1
                                && rrGaz[0]["LocationNotes"].ToString().Contains(geoinfosource) //Ariane alt: Contains(DiversityWorkbench.GeoFunctions.GeoInfoSource)
                                && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName))
                            {
                                string Place = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.City)
                                    && GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City] != GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.PlaceName])
                                    Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.City];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision))
                                    Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.CountrySubdivision];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Country))
                                    Place += ", " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Country];
                                rrGaz[0]["Location1"] = Place;
                                rrGaz[0]["LocationNotes"] = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Source];
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Latitude))
                                    rrGaz[0]["AverageLatitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Latitude]);
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Longitude))
                                    rrGaz[0]["AverageLongitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Longitude]);
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Distance))
                                {
                                    rrGaz[0]["LocationAccuracy"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Distance];
                                    rrGaz[0]["DistanceToLocation"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Distance];
                                }
                                if (GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Direction))
                                    rrGaz[0]["DirectionToLocation"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Direction];
                            }

                            // insert altitude entry if missing
                            System.Data.DataRow[] rrAlt = this.dataSetSamplingPlot.SamplingPlotLocalisation.Select("LocalisationSystemID = 4");
                            if (GeoInfos.Count > 0
                                && rrAlt.Length == 0
                                && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Altitude))
                            {
                                DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow RAltitude = this.dataSetSamplingPlot.SamplingPlotLocalisation.NewSamplingPlotLocalisationRow();
                                RAltitude.PlotID = int.Parse(this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows[0]["PlotID"].ToString());
                                RAltitude.LocalisationSystemID = 4;
                                RAltitude.Location1 = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude];
                                RAltitude.LocationNotes = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.AltitudeSource];
                                this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows.Add(RAltitude);
                            }
                            else if (GeoInfos.Count > 0
                                && rrAlt.Length == 1
                                && rrAlt[0]["LocationNotes"].ToString().Contains(geoinfosource) //Ariane alt: Contains(DiversityWorkbench.GeoFunctions.GeoInfoSource)
                                && GeoInfos.ContainsKey(DiversityWorkbench.GeoFunctions.GeoInfo.Altitude))
                            {
                                rrAlt[0]["Location1"] = GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude];
                                rrAlt[0]["AverageAltitudeCache"] = float.Parse(GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.Altitude]);
                                rrAlt[0]["LocationNotes"] = "Source: " + GeoInfos[DiversityWorkbench.GeoFunctions.GeoInfo.AltitudeSource];
                            }

                            this.savePlot(null, null);
                            string Geography = string.Format("POINT({0} {1})", f.Longitude.ToString("F10", CultureInfo.InvariantCulture), f.Latitude.ToString("F10", CultureInfo.InvariantCulture));
                            setGeography(Geography);
                            
                            // Reload the entire plot to sync in-memory data with database
                            this.setSamplingPlot(this.PlotID);

                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            else if (ParsingMethodName == "MTB")//LocalisationSystem == "MTB (A, CH, D)")
            {
                System.Data.DataRow[] RR = this.dataSetSamplingPlot.SamplingPlotLocalisation.Select("");
                if (RR.Length > 0)
                {
                    string ConnectionString = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversitySamplingPlots"].ServerConnection.ConnectionString;
                    DiversityWorkbench.Forms.FormTK25 f = new DiversityWorkbench.Forms.FormTK25((float)lo, (float)la, ConnectionString);
                    try
                    {
                        f.ShowDialog();
                        if (f.DialogResult == DialogResult.OK)
                        {
                            RLoc["Location1"] = f.TK25;
                            this.textBoxLocation1.Text = f.TK25;

                            RLoc["Location2"] = f.TK25_Quadrant;
                            this.textBoxLocation2.Text = f.TK25_Quadrant;

                            RLoc["LocationNotes"] = f.TK25_Name;
                            this.textBoxLocationNotes.Text = f.TK25_Name;
                        }
                    }
                    catch { }
                }
            }
        }

        public void setGeography(string Geography)
        {
            // Add Localisation WGS84
            // string Geography = string.Format("POINT({0} {1})", lon.ToString("F09", CultureInfo.InvariantCulture), lat.ToString("F09", CultureInfo.InvariantCulture));

            // Check if Localisation WGS84 exists
            System.Data.DataRow[] rr = this.dataSetSamplingPlot.SamplingPlotLocalisation.Select("LocalisationSystemID = 8");
            // If not, create an entry in the Localisation list
            if (rr.Length == 0)
            {
                // MessageBox.Show("WGS84 not available");
                // this.savePlot(null, null);

                DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow R = this.dataSetSamplingPlot.SamplingPlotLocalisation.NewSamplingPlotLocalisationRow();
                R.PlotID = this.PlotID;
                R.LocalisationSystemID = 8;
                this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows.Add(R);

                System.Data.DataRow Rlist = this._dtLocalisationList.NewRow();
                Rlist["LocalisationSystemID"] = R.LocalisationSystemID;
                Rlist["DisplayText"] = "Coord. WGS84";
                this._dtLocalisationList.Rows.Add(Rlist);
            }

            // Set Localisation Geography
            // Check if LoalisationSystem 8 entry is available
            string SQL1 = "select count(*) from  SamplingPlotLocalisation where PlotID = " + this.PlotID.ToString() + " and LocalisationSystemID = 8";
            string result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL1);
            // If not, create it
            if (result == "0")
            {
                // Create Localisation
                SQL1 = "INSERT INTO SamplingPlotLocalisation (PlotID, LocalisationSystemID, Location1, Location2, Geography, AverageLongitudeCache, AverageLatitudeCache ) " +
                       "VALUES  (" + this.PlotID.ToString() +
                       ", 8 " +
                       ", geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Long " +
                       ", geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Lat " +
                       ", geography::STGeomFromText('" + Geography + "', 4326) " +
                       ", geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Long " +
                       ", geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Lat)";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL1);
            }
            // Else update it
            else
            {
                // Set Localisation Geography
                SQL1 = "UPDATE SamplingPlotLocalisation SET Geography =  geography::STGeomFromText('" + Geography + "', 4326) " +
                    ", Location1 = geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Long " +
                    ", Location2 = geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Lat " +
                    ", AverageLongitudeCache = geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Long " +
                    ", AverageLatitudeCache = geography::STGeomFromText('" + Geography + "', 4326).EnvelopeCenter().Lat " +
                    "WHERE PlotID = " + this.PlotID.ToString() + " and LocalisationSystemID = 8";                // Set Localisation Geography
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL1);
            }
            // Truncate geography string and show ist in label
            string geoStr = (Geography.Length > 100 ? Geography.Substring(0, 100) : Geography);
            this.labelGeography.Text = "Geography: " + geoStr + "...";

            // Set Plot PlotGeography_Cache
            SQL1 = "UPDATE SamplingPlot SET PlotGeography_Cache =  geography::STGeomFromText('" + Geography + "', 4326) WHERE PlotID = " + this.PlotID.ToString();
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL1);
        }

        private void dateTimePickerLocalisationDate_CloseUp(object sender, EventArgs e)
        {
            if (this.samplingPlotLocalisationBindingSource.Current != null)
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
                    R["DeterminationDate"] = this.dateTimePickerLocalisationDate.Value.Date.ToShortDateString();
                    this.textBoxGeographyDeterminationDate.Text = this.dateTimePickerLocalisationDate.Value.Date.ToShortDateString();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void textBoxLocation1_TextChanged(object sender, EventArgs e)
        {
            if (this.samplingPlotLocalisationBindingSource.Current != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
                if (!RV["Location1"].Equals(System.DBNull.Value)
                    && RV["Location1"].ToString() != this.textBoxLocation1.Text//                    && RV["Location1"].ToString() != "New Altitude (mNN)"
                    )
                {
                    if (RV["Location1"].ToString() == "New Altitude (mNN)")
                        RV["Location1"] = this.textBoxLocation1.Text;
                    this.TryToSaveAndCalculateAverageForLocalisation(false);
                }
            }
        }

        private void textBoxLocation2_TextChanged(object sender, EventArgs e)
        {
            if (this.samplingPlotLocalisationBindingSource.Current != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
                if (!RV["Location2"].Equals(System.DBNull.Value)
                    && RV["Location2"].ToString() != this.textBoxLocation2.Text)
                {
                    this.TryToSaveAndCalculateAverageForLocalisation(false);
                }
            }
        }
        private void buttonCoordinatesConvertToWGS84_Click(object sender, EventArgs e)
        {
            if (this.samplingPlotLocalisationBindingSource.Current != null)
            {
                DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow R = (DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow)this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows[this.samplingPlotLocalisationBindingSource.Position];
                DiversitySamplingPlots.Forms.FormGeoConversion f = new DiversitySamplingPlots.Forms.FormGeoConversion(R, ref this.dataSetSamplingPlot);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
        }

        private void listBoxLocalisation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.samplingPlotLocalisationBindingSource.Current != null)
            {
                this.groupBoxPlotLocalisation.Enabled = true;
                if (this.listBoxLocalisation.SelectedItem != null)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxLocalisation.SelectedItem;
                    this.setLocalisationControls(RV.Row);
                    int LocalisationSystemID = int.Parse(RV["LocalisationSystemID"].ToString());
                    for (int i = 0; i < this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows.Count; i++)
                    {
                        if (this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows[i]["LocalisationSystemID"].ToString() == LocalisationSystemID.ToString())
                        {
                            this.samplingPlotLocalisationBindingSource.Position = i;
                            break;
                        }
                    }
                }
                else
                    this.groupBoxPlotLocalisation.Enabled = false;
            }
            else
                this.groupBoxPlotLocalisation.Enabled = false;
        }

        private void toolStripButtonLocalisationNew_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt = DiversitySamplingPlots.LookupTable.DtLocalisationSystem.Copy();
                System.Collections.Generic.List<System.Data.DataRow> RR = new List<DataRow>();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    System.Data.DataRow[] rr = this.dataSetSamplingPlot.SamplingPlotLocalisation.Select("LocalisationSystemID = " + R["LocalisationSystemID"].ToString());
                    if (rr.Length > 0)
                        RR.Add(R);
                }
                foreach (System.Data.DataRow Rdel in RR)
                    Rdel.Delete();
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "LocalisationSystemID", "Localisation system", "Select a localisation system");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotLocalisationRow R = this.dataSetSamplingPlot.SamplingPlotLocalisation.NewSamplingPlotLocalisationRow();
                    R.PlotID = this.PlotID;
                    R.LocalisationSystemID = int.Parse(f.SelectedValue);
                    this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows.Add(R);

                    System.Data.DataRow Rlist = this._dtLocalisationList.NewRow();
                    Rlist["LocalisationSystemID"] = int.Parse(f.SelectedValue);
                    Rlist["DisplayText"] = f.SelectedString;
                    this._dtLocalisationList.Rows.Add(Rlist);
                }
            }
            catch
            {
            }
        }

        private void toolStripButtonLocalisationDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;

            //  MessageBox.Show("LocalisationSystemID = " + R.Row.ItemArray[1].ToString());

            if (R.Row.ItemArray[1].ToString() == "8")
            {
                GeographyCacheDelete(R.Row.ItemArray[0].ToString());
            }

            R.Delete();
            System.Data.DataRowView Rlist = (System.Data.DataRowView)this.listBoxLocalisation.SelectedItem;
            Rlist.Delete();
            this.updatePlot();

        }

        private void GeographyCacheDelete(string StrPlotID)
        {
            string SQL = "UPDATE SamplingPlot SET PlotGeography_Cache = null WHERE PlotID = " + StrPlotID;
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        }


        // Custom units

        private void comboBoxGeographyLocationUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Custom = false;
            this.comboBoxGeographyLocationUnits.BackColor = System.Drawing.SystemColors.Window;
            if (this.comboBoxGeographyLocationUnits.Visible)
            {
                if (this.comboBoxGeographyLocationUnits.Items.Count > 0)
                {
                    if (this.comboBoxGeographyLocationUnits.SelectedIndex > 0)
                    {
                        Custom = true;
                        this.comboBoxGeographyLocationUnits.BackColor = System.Drawing.Color.Aquamarine;
                    }
                }
            }
            this.tableLayoutPanelGeographyLocationCustom.Visible = Custom;
            this.tableLayoutPanelGeographyLocationStandard.Visible = !Custom;
            if (Custom)
            {
                this.setGeographyCustomUnitControls();
            }
        }

        private void buttonGeographyCustomSave_Click(object sender, EventArgs e)
        {
            double Value1 = 0;
            double Value2 = 0;
            bool OK1 = false;
            bool OK2 = false;
            string StringValue1 = "";
            string StringValue2 = "";
            string Ori = "";
            if (this.samplingPlotLocalisationBindingSource.Current != null)
            {
                try
                {
                    string GeographicalUnit = this.comboBoxGeographyLocationUnits.Text;
                    System.Data.DataRowView r = (System.Data.DataRowView)this.samplingPlotLocalisationBindingSource.Current;
                    System.Data.DataRow R = this.dataSetSamplingPlot.SamplingPlotLocalisation.Rows[this.samplingPlotLocalisationBindingSource.Position];
                    R.BeginEdit();
                    r.BeginEdit();
                    switch (GeographicalUnit)
                    {
                        case "numeric":
                            StringValue1 = this.textBoxLocation1a.Text;
                            OK1 = double.TryParse(StringValue1, out Value1);
                            if (OK1)
                            {
                                r["Location1"] = Value1.ToString();
                                if (r["AverageLongitudeCache"].Equals(System.DBNull.Value) || Value1 != 0)
                                    r["AverageLongitudeCache"] = Value1;
                            }

                            StringValue2 = this.textBoxLocation2a.Text;
                            OK2 = double.TryParse(StringValue2, out Value2);
                            if (OK2)
                            {
                                r["Location2"] = Value2.ToString();
                                if (r["AverageLatitudeCache"].Equals(System.DBNull.Value) || Value2 != 0)
                                    r["AverageLatitudeCache"] = Value2;
                            }
                            break;

                        case "deg_min_sec":
                            int DegNS = 0;
                            int DegEW = 0;
                            int MinNS = 0;
                            int MinEW = 0;
                            int PrefixNS = 1;
                            int PrefixEW = 1;
                            double SecNS = 0;
                            double SecEW = 0;

                            bool OK = true;
                            string Deg = this.textBoxLocation1a.Text.ToString();
                            Deg = Deg.Replace(" ", "");
                            if (Deg.Trim().StartsWith("-")) PrefixEW = -1;
                            if (Deg.Length == 0) Deg = "0";
                            if (!int.TryParse(Deg, out DegEW)) OK = false;
                            if (OK) DegEW = System.Math.Abs(DegEW);
                            string Min = this.textBoxLocation1b.Text.ToString();
                            if (Min.Length == 0) Min = "0";
                            if (!int.TryParse(Min, out MinEW)) OK = false;
                            string Sec = this.textBoxLocation1c.Text.ToString();
                            Sec = Sec.Replace(".", ",");
                            if (Sec.Length == 0) Sec = "0";
                            if (!double.TryParse(Sec, out SecEW)) OK = false;
                            if (OK)
                            {
                                if (!this.convertFromDegMinSecToNumeric(ref Value1, DegEW, MinEW, SecEW))
                                    OK = false;
                            }

                            Deg = this.textBoxLocation2a.Text.ToString();
                            Deg = Deg.Replace(" ", "");
                            if (Deg.Trim().StartsWith("-")) PrefixNS = -1;
                            if (Deg.Length == 0) Deg = "0";
                            if (!int.TryParse(Deg, out DegNS)) OK = false;
                            if (OK) DegNS = System.Math.Abs(DegNS);
                            Min = this.textBoxLocation2b.Text.ToString();
                            if (Min.Length == 0) Min = "0";
                            if (!int.TryParse(Min, out MinNS)) OK = false;
                            Sec = this.textBoxLocation2c.Text.ToString();
                            Sec = Sec.Replace(".", ",");
                            if (Sec.Length == 0) Sec = "0";
                            if (!double.TryParse(Sec, out SecNS)) OK = false;
                            if (OK)
                            {
                                if (!this.convertFromDegMinSecToNumeric(ref Value2, DegNS, MinNS, SecNS))
                                    OK = false;
                            }
                            if (OK)
                            {
                                R["Location1"] = (Value1 * PrefixEW).ToString();
                                R["Location2"] = (Value2 * PrefixNS).ToString();
                                R["AverageLatitudeCache"] = Value2 * PrefixNS;
                                R["AverageLongitudeCache"] = Value1 * PrefixEW;
                                Ori = "Lat.: " + this.textBoxLocation2a.Text + "° ";
                                if (this.textBoxLocation2b.Text.Length > 0) Ori += this.textBoxLocation2b.Text + "' ";
                                if (this.textBoxLocation2c.Text.Length > 0) Ori += this.textBoxLocation2c.Text + "'' ";
                                Ori += " Long.: " + this.textBoxLocation1a.Text + "° ";
                                if (this.textBoxLocation1b.Text.Length > 0) Ori += this.textBoxLocation1b.Text + "' ";
                                if (this.textBoxLocation1c.Text.Length > 0) Ori += this.textBoxLocation1c.Text + "'' ";
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromDegMinSec(DegNS, MinNS, SecNS, DegEW, MinEW, SecEW);
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                            }
                            break;

                        case "degree":
                            int degFrom = 0;
                            int degTo = 0;
                            // int degAv;
                            // int degAcc = 0;
                            bool isDegFrom = false;
                            bool isDegTo = false;
                            if (int.TryParse(this.textBoxLocation1a.Text, out degFrom))
                            {
                                R["Location1"] = degFrom.ToString();
                                isDegFrom = true;
                            }
                            if (int.TryParse(this.textBoxLocation2a.Text, out degTo))
                            {
                                R["Location2"] = degTo.ToString();
                                isDegTo = true;
                            }
                            if (isDegFrom || isDegTo)
                            {
                                //if (isDegFrom && isDegTo)
                                //{
                                //    degAv = (degFrom + degTo) / 2;
                                //    R["AverageAltitudeCache"] = degAv;
                                //}
                                //else
                                //{
                                //    degAv = degFrom + degTo;
                                //    R["AverageAltitudeCache"] = degAv;
                                //}
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromDegree(degFrom, degTo).ToString() + " °";
                            }
                            this.GeographyLocationSetNotesDateResponsible(R, "");
                            break;
                        case "orientation":
                            double dValFrom = 0;
                            double dValTo = 0;
                            bool isFromOri = false;
                            bool isToOri = false;
                            string FromOri = this.comboBoxLocation1a.Text;
                            string ToOri = this.comboBoxLocation2a.Text;
                            if (this.convertFromOrientationToDegree(ref FromOri, ref dValFrom))
                            {
                                R["Location1"] = dValFrom;
                                isFromOri = true;
                            }
                            if (this.convertFromOrientationToDegree(ref ToOri, ref dValTo))
                            {
                                R["Location2"] = dValTo;
                                isToOri = true;
                            }
                            if (isFromOri || isToOri)
                            {
                                if (isFromOri) Ori = FromOri;
                                if (isFromOri && isToOri) Ori += " - ";
                                if (isToOri) Ori += ToOri;
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromOrientation(this.comboBoxLocation1a.Text, this.comboBoxLocation2a.Text);
                            }
                            break;
                        case "percent":
                            double pFrom = 0;
                            double pTo = 0;
                            double pmValFrom;
                            double pmValTo;
                            double pfAcc;
                            double pmAcc;
                            bool pisFrom = false;
                            bool pisTo = false;
                            if (double.TryParse(this.textBoxLocation1a.Text, out pFrom))
                            {
                                pmValFrom = this.convertUnit(pFrom, MeasurementUnit.percent, MeasurementUnit.degree);
                                R["Location1"] = pmValFrom;
                                pisFrom = true;
                            }
                            if (double.TryParse(this.textBoxLocation2a.Text, out pTo))
                            {
                                pmValTo = this.convertUnit(pTo, MeasurementUnit.percent, MeasurementUnit.degree);
                                R["Location2"] = pmValTo;
                                pisTo = true;
                            }
                            if (pisFrom || pisTo)
                            {
                                if (pisFrom) Ori = pFrom.ToString();
                                if (pisFrom && pisTo) Ori += " - ";
                                if (pisTo) Ori += pTo.ToString();
                                Ori += " percent";
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                            }
                            if (pFrom != 0 || pTo != 0)
                            {
                                pfAcc = DiversityWorkbench.GeoFunctions.AccuracyFromValues(pFrom, pTo);
                                pmAcc = this.convertUnit((double)pfAcc, MeasurementUnit.percent, MeasurementUnit.degree);
                                R["LocationAccuracy"] = pmAcc + " °";
                            }
                            break;
                        case "feet":
                        case "foot":
                            double fFrom = 0;
                            double fTo = 0;
                            double mValFrom;
                            double mValTo;
                            double fAcc;
                            double mAcc;
                            bool isFrom = false;
                            bool isTo = false;
                            if (double.TryParse(this.textBoxLocation1a.Text, out fFrom))
                            {
                                mValFrom = this.convertUnit(fFrom, MeasurementUnit.feet, MeasurementUnit.m);
                                R["Location1"] = mValFrom;
                                isFrom = true;
                            }
                            if (double.TryParse(this.textBoxLocation2a.Text, out fTo))
                            {
                                mValTo = this.convertUnit(fTo, MeasurementUnit.feet, MeasurementUnit.m);
                                R["Location2"] = mValTo;
                                isTo = true;
                            }
                            if (isFrom || isTo)
                            {
                                if (isFrom) Ori = fFrom.ToString();
                                if (isFrom && isTo) Ori += " - ";
                                if (isTo) Ori += fTo.ToString();
                                Ori += " feet";
                                this.GeographyLocationSetNotesDateResponsible(R, Ori);
                            }
                            if (fFrom != 0 || fTo != 0)
                            {
                                fAcc = DiversityWorkbench.GeoFunctions.AccuracyFromValues(fFrom, fTo);
                                mAcc = this.convertUnit((double)fAcc, MeasurementUnit.feet, MeasurementUnit.m);
                                R["LocationAccuracy"] = mAcc + " m";
                            }
                            goto case "AverageAltitude";
                        // break;
                        case "m":
                            double From = 0;
                            double To = 0;
                            double Av;
                            decimal ValFrom;
                            decimal ValTo;
                            // double Acc = 0;
                            if (double.TryParse(this.textBoxLocation1a.Text, out From))
                            {
                                ValFrom = (decimal)From;
                                ValFrom = Math.Round(ValFrom, 0);
                                R["Location1"] = ValFrom.ToString();
                            }
                            if (double.TryParse(this.textBoxLocation2a.Text, out To))
                            {
                                ValTo = (decimal)To;
                                ValTo = Math.Round(ValTo, 0);
                                R["Location2"] = ValTo.ToString();
                            }
                            if (From != 0 || To != 0)
                            {
                                if (From != 0 && To != 0)
                                {
                                    Av = (From + To) / 2;
                                    R["AverageAltitudeCache"] = Av;
                                }
                                else
                                {
                                    Av = From + To;
                                    R["AverageAltitudeCache"] = Av;
                                }
                                R["LocationAccuracy"] = DiversityWorkbench.GeoFunctions.AccuracyFromValues(From, To) + " m";
                            }
                            this.GeographyLocationSetNotesDateResponsible(R, "");
                            goto case "AverageAltitude";
                        // break;
                        case "AverageAltitude":
                            int LocSysID;
                            if (int.TryParse(R["LocalisationSystemID"].ToString(), out LocSysID))
                            {
                                string LocalisationSystem = DiversitySamplingPlots.LookupTable.LocalisationSystemName(LocSysID);
                                if (LocalisationSystem == "Altitude (mNN)")
                                {
                                    this.TryToSaveAndCalculateAverageForLocalisation(true);
                                }
                            }
                            break;
                        default:
                            this.comboBoxGeographyLocationUnits.Visible = true;
                            break;
                    }
                    R.EndEdit();
                }
                catch { }
            }
        }

        #endregion

        #region Projects events

        private void toolStripButtonProjectNew_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtProject = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT Project, ProjectID FROM ProjectList", DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtProject);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Select a project from the list");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingProjectRow R = this.dataSetSamplingPlot.SamplingProject.NewSamplingProjectRow();
                R.PlotID = this.PlotID;
                R.ProjectID = int.Parse(f.SelectedValue);
                this.dataSetSamplingPlot.SamplingProject.Rows.Add(R);
                DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingProjectListRow Rl = this.dataSetSamplingPlot.SamplingProjectList.NewSamplingProjectListRow();
                Rl.ProjectID = R.ProjectID;
                Rl.Project = f.SelectedString;
                this.dataSetSamplingPlot.SamplingProjectList.Rows.Add(Rl);
                //this.fillProjectList(this.ID);
            }
        }

        private void toolStripButtonProjectRemove_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            try
            {
                string Project = rv["Project"].ToString();
                if (System.Windows.Forms.MessageBox.Show("Do you want to remove this specimen from the project \n\r\n\r" + Project, "Delete project?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int ProjectID = int.Parse(this.dataSetSamplingPlot.SamplingProjectList.Rows[this.samplingProjectListBindingSource.Position]["ProjectID"].ToString());
                    //int ProjectID = int.Parse(this.dataSetCollectionSpecimen.CollectionProjectList.Rows[collectionProjectListBindingSource.Position]["ProjectID"].ToString());
                    this.samplingProjectListBindingSource.RemoveCurrent();
                    //this.collectionProjectListBindingSource.RemoveCurrent();
                    foreach (System.Data.DataRow R in this.dataSetSamplingPlot.SamplingProject.Rows)
                    {
                        if (R["ProjectID"].ToString() == ProjectID.ToString())
                        {
                            R.Delete();
                            break;
                        }
                    }
                    this.FormFunctions.updateTable(this.dataSetSamplingPlot, "SamplingProject", this.sqlDataAdapterSamplingProject, this.BindingContext);
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Maps and images events

        private void buttonHeaderShowMap_Click(object sender, EventArgs e)
        {
            if (this.buttonHeaderShowMap.BackColor == System.Drawing.Color.Red)
            {
                if (this.dataSetSamplingPlot.SamplingPlotGeography.Rows.Count > 0)
                    this.buttonHeaderShowMap.BackColor = System.Drawing.Color.Yellow;
                else
                    this.buttonHeaderShowMap.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                this.buttonHeaderShowMap.BackColor = System.Drawing.Color.Red;
            }
            this.setImageVisibility();
        }

        #endregion // Maps and images events

        #region Sampling plot editor events

        //void userControlGIS_SizeChanged(object sender, System.EventArgs e)
        //{
        //    this.userControlGIS.SizeOfControlChanged();
        //}

        private void userControlGeography_SizeChanged(object sender, EventArgs e)
        {
            this.userControlGeography.SizeOfControlChanged();
        }

        #endregion

        #region Files events
        private void toolStripButtonFileNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.PlotID == -1)
                {
                    MessageBox.Show("No plot has been selected!", "Error");
                    return;
                }

                if (this.openFileDialog == null)
                {
                    this.openFileDialog = new OpenFileDialog();
                    //string Filter = "|";
                    //for (int i = 0; i < this._MediaServiceSupportedImageFormats.Length; i++)
                    //{
                    //    if (i > 0) Filter = ", " + Filter;
                    //    Filter = this._MediaServiceSupportedImageFormats[i].Replace(".", "") + Filter;
                    //    if (i > 0) Filter = Filter + ", ";
                    //    Filter = Filter + this._MediaServiceSupportedImageFormats[i];
                    //}
                    //this.openFileDialog.Filter = Filter;
                }
                if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (this.openFileDialog.FileName.Length > 0)
                    {
                        DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotResourceRow R = this.dataSetSamplingPlot.SamplingPlotResource.NewSamplingPlotResourceRow();
                        R.PlotID = this.PlotID;
                        R.ResourceURI = this.openFileDialog.FileName;
                        this.dataSetSamplingPlot.SamplingPlotResource.Rows.Add(R);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO
                if (this.PlotID == -1)
                {
                    MessageBox.Show("No plot has been selected!", "Error");
                    return;
                }
                // EnsureBrowserEmulationEnabled(false);
                // Start Form with Wiki site
                DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser("https://startpage.com");

                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.URL.Length > 0)
                    {
                        DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotResourceRow R = this.dataSetSamplingPlot.SamplingPlotResource.NewSamplingPlotResourceRow();
                        R.PlotID = this.PlotID;
                        R.ResourceURI = f.URL;
                        this.dataSetSamplingPlot.SamplingPlotResource.Rows.Add(R);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonFileOpen_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxFiles.SelectedItem;
                if (R == null)
                {
                    MessageBox.Show("No resource available!", "Error");
                }
                else
                {
                    string File = R["ResourceURI"].ToString();
                    if (File.StartsWith("http"))
                    {
                        DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(File);
                        f.ShowDialog();
                    }
                    else
                    {
                        System.IO.FileInfo F = new System.IO.FileInfo(File);
                        if (F.Exists)
                        {
                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(F.FullName);
                            info.UseShellExecute = true;
                            System.Diagnostics.Process.Start(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void toolStripButtonFileRemove_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView r = (System.Data.DataRowView)this.listBoxFiles.SelectedItem;
            if (r == null)
            {
                MessageBox.Show("No resource available!", "Error");
            }
            else
            {
                r.Row.Delete();
            }
        }

        private void listBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool EnableControls = true;
            if (DiversityWorkbench.Forms.FormFunctions.getObjectPermissionCache("SamplingPlotResource", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update) != null)
                EnableControls = (bool)DiversityWorkbench.Forms.FormFunctions.getObjectPermissionCache("SamplingPlotResource", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
            if (this.listBoxFiles.SelectedIndex == -1)
            {
                EnableControls = false;
                this.labelFileName.Text = "Resources";
                this.groupBoxFile.Text = "Resources";
                this.pictureBoxResource.Image = null;
                this.pictureBoxResource.Width = 1;
                this.pictureBoxResource.Visible = false;
            }
            else
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxFiles.SelectedItem;
                    string File = R["ResourceURI"].ToString();
                    System.IO.FileInfo F = new System.IO.FileInfo(File);
                    this.labelFileName.Text = F.Name;
                    this.groupBoxFile.Text = F.Name;
                    switch (F.Extension.ToLower())
                    {
                        case ".jpg":
                        case ".png":
                        case ".gif":
                            System.Drawing.Bitmap B = new Bitmap(F.FullName);
                            this.pictureBoxResource.Image = B;
                            this.pictureBoxResource.Visible = true;
                            this.pictureBoxResource.Width = (int)(this.tableLayoutPanelFiles.Width / 2);
                            break;
                        default:
                            this.pictureBoxResource.Width = 1;
                            this.pictureBoxResource.Visible = false;
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    this.pictureBoxResource.Width = 1;
                    this.pictureBoxResource.Visible = false;
                }
            }
            this.userControlModuleRelatedEntryFileResponsible.Enabled = EnableControls;
            this.textBoxFileNotes.Enabled = EnableControls;
        }

        private void pictureBoxResource_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxFiles.SelectedItem;
                string File = R["ResourceURI"].ToString();
                DiversityWorkbench.Forms.FormImage f = new DiversityWorkbench.Forms.FormImage(File);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Properties events

        private void toolStripButtonPropertyDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this.listBoxProperties.SelectedItem;
                r.Row.Delete();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonPropertyNew_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT PropertyID, DisplayText FROM Property WHERE DisplayEnabled = 1 ";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "PropertyID", "Property", "Select an entry from the list");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversitySamplingPlots.Datasets.DataSetSamplingPlot.SamplingPlotPropertyRow R = this.dataSetSamplingPlot.SamplingPlotProperty.NewSamplingPlotPropertyRow();
                    R.PlotID = this.PlotID;
                    R.PropertyID = int.Parse(f.SelectedValue);
                    R.PropertyURI = "";
                    R.DisplayText = "New " + f.SelectedString;
                    this.dataSetSamplingPlot.SamplingPlotProperty.Rows.Add(R);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool EnableControls = true;
            if (DiversityWorkbench.Forms.FormFunctions.getObjectPermissionCache("SamplingPlotProperty", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update) != null)
                EnableControls = (bool)DiversityWorkbench.Forms.FormFunctions.getObjectPermissionCache("SamplingPlotProperty", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Update);
            if (this.listBoxProperties.SelectedIndex == -1)
                EnableControls = false;
            this.userControlModuleRelatedEntryProperty.Enabled = EnableControls;
            this.userControlModuleRelatedEntryPropertyResponsible.Enabled = EnableControls;
            this.textBoxPropertyNotes.Enabled = EnableControls;
            this.setLocalListForScientificTerm();
        }


        // Property functions

        private void setLocalListForScientificTerm()
        {
            try
            {
                if (this.listBoxProperties.SelectedItem != null && this.listBoxProperties.SelectedIndex < this.listBoxProperties.Items.Count)
                {
                    System.Data.DataRowView DataRow = (System.Data.DataRowView)this.listBoxProperties.SelectedItem;
                    int PropertyID = int.Parse(DataRow["PropertyID"].ToString());
                    string SQL = "SELECT DisplayText FROM Property WHERE PropertyID = " + PropertyID.ToString();
                    string ListInDatabase = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    this.userControlModuleRelatedEntryProperty.ListInDatabase = ListInDatabase;
                    this.groupBoxProperties.Text = ListInDatabase;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion // Properties events

        private void linkLabelUri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.linkLabelUri.Text.Length > 0)
            {
                DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(this.PlotID, S);
                f.ShowDialog();
            }
        }

        #endregion // Event handlers

        #region Images

        private void initCollectionImages()
        {
            this.userControlXMLTreeEXIF.setToDisplayOnly();
        }

        private System.Data.DataTable _dataTableImages;
        private System.Data.DataTable DataTableImages(int PlotID)
        {
            if (_dataTableImages == null) _dataTableImages = new DataTable("Images");
            else _dataTableImages.Clear();
            this.labelImageSource.Text = "";
            this.linkLabelImage.Text = "";
            this.ImageSetIcon(ImageSouce.Plot);
            try
            {
                if (this.dataSetSamplingPlot.SamplingPlot.Rows.Count > 0)
                {
                    //    string SQL = "SELECT ResourceURI, 'Plot' AS SourceType, '" + this.dataSetSamplingPlot.SamplingPlot.Rows[0]["PlotIdentifier"].ToString() + "' AS Source case when [Description] is null then '' else cast([Description] as nvarchar(Max)) end AS EXIF " +
                    //    " FROM SamplingPlotResourcE " +
                    //    " WHERE PlotID = " + PlotID.ToString() +
                    //    " AND PlotID IN (SELECT PlotID FROM PlotID_UserAvailable) AND (ResourceURI LIKE '%.jpg' OR ResourceURI LIKE '%.png' OR ResourceURI LIKE '%.gif')";
                    //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _dataTableImages);
                    this.ImagesAddDC();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return _dataTableImages;
        }


        private void listBoxImages_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageList.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 70, 70, e.Index);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void listBoxImages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageList.ImageSize.Height;
            e.ItemWidth = this.imageList.ImageSize.Width;
        }


        private void listBoxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.pictureBoxImage.Image = null;

                if (this._dataTableImages.Rows.Count > 0)
                    this.tableLayoutPanelImages.Enabled = true;
                else
                    this.tableLayoutPanelImages.Enabled = false;

                int i = this.listBoxImages.SelectedIndex;
                if (this._dataTableImages.Rows.Count > i && i > -1)
                {
                    System.Data.DataRow r = this._dataTableImages.Rows[i];
                    if (r["ResourceURI"].ToString().Length > 0)
                    {
                        DiversityWorkbench.Forms.FormFunctions.SetPictureBoxImage(r["ResourceURI"].ToString(), this.pictureBoxImage, this.imageList);
                        this.linkLabelImage.Text = r["ResourceURI"].ToString();
                        this.ImageSetIcon(this.SourceOfImage(r[1].ToString()));
                        this.labelImageSource.Text = r["Source"].ToString();
                        this.userControlXMLTreeEXIF.XML = r["EXIF"].ToString();
                        this.buttonImageDCunitValues.Enabled = true;
                    }
                    else
                    {
                        this.buttonImageDCunitValues.Enabled = false;
                        this.pictureBoxImage.Image = null;
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ImageSetIcon(ImageSouce source)
        {
            switch (source)
            {
                case ImageSouce.Plot:
                    this.pictureBoxImageSource.Image = this.imageListImageSource.Images[0];
                    break;
                case ImageSouce.Event:
                    this.pictureBoxImageSource.Image = this.imageListImageSource.Images[1];
                    break;
                case ImageSouce.Specimen:
                    this.pictureBoxImageSource.Image = this.imageListImageSource.Images[2];
                    break;
                case ImageSouce.Unit:
                    this.pictureBoxImageSource.Image = this.imageListImageSource.Images[3];
                    break;
                case ImageSouce.Part:
                    this.pictureBoxImageSource.Image = this.imageListImageSource.Images[4];
                    break;
            }
        }


        private void buttonImageOpen_Click(object sender, EventArgs e)
        {
            if (this.listBoxImages.SelectedItem != null)
            {
                System.Data.DataRow RV = this._dataTableImages.Rows[this.listBoxImages.SelectedIndex];
                if (RV["ResourceURI"].ToString().Length == 0) return;
                else this.OpenImage(RV["ResourceURI"].ToString());
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
            }
        }

        private void linkLabelImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.OpenImage(this.linkLabelImage.Text);
        }

        private void OpenImage(string URI)
        {
            DiversityWorkbench.Forms.FormImage f = new DiversityWorkbench.Forms.FormImage(URI);
            //f.setParentForm(this);
            if (f.Parent != null)
                f.StartPosition = FormStartPosition.CenterParent;
            else
                f.StartPosition = FormStartPosition.CenterScreen;
            f.Width = this.Width - 10;
            f.Height = this.Height - 10;
            f.Show();
        }

        private enum ImageSouce { Plot, Event, Specimen, Unit, Part }


        #region DC

        private void buttonImagesDC_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ServerConnection connection = new DiversityWorkbench.ServerConnection();
            connection.ModuleName = "DiversityCollection";
            connection.DatabaseName = "";
            connection.DatabasePassword = DiversityWorkbench.Settings.Password;
            connection.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
            connection.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
            connection.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
            connection.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
            DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(connection, true);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.ServerConnection != null)
            {
                this._serverConnectionDC = f.ServerConnection;
                this.buttonImagesDC.Image = DiversitySamplingPlots.Properties.Resources.Database;
                this.toolTip.SetToolTip(this.buttonImagesDC, this._serverConnectionDC.DatabaseName);
                this.labelImages.Text = "Images in " + this._serverConnectionDC.DatabaseName;
                this.labelImageSource.Visible = true;
                this.pictureBoxImageSource.Visible = true;
                this.ImagesAddDC();
                this.FormFunctions.FillImageList(this.listBoxImages, this.imageList, this.imageListForm, this.DataTableImages(PlotID), "ResourceURI", this.pictureBoxImage);
                this.labelImages.BackColor = System.Drawing.Color.Transparent;
            }
            else
            {
                if (this._serverConnectionDC != null)
                {
                    if (System.Windows.Forms.MessageBox.Show("Disconnect to DiversityCollection Source for images?", "Disconnect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this._serverConnectionDC = null;
                        this.buttonImagesDC.Image = DiversitySamplingPlots.Properties.Resources.NoDatabase;
                        this.toolTip.SetToolTip(this.buttonImagesDC, "Connection to datbase");
                        this.labelImages.Text = "Connect to DiversityCollection database to show linked images.";
                        this.linkLabelImage.Text = "";
                        this.userControlXMLTreeEXIF.XML = "";
                        this.labelImageSource.Visible = false;
                        this.pictureBoxImageSource.Visible = false;
                        this.listBoxImages.DataSource = null;
                        this.listBoxImages.Items.Clear();
                        this.listBoxImages_SelectedIndexChanged(null, null);
                        this.labelImages.BackColor = System.Drawing.Color.Pink;
                    }
                }
            }
        }

        private DiversityWorkbench.ServerConnection _serverConnectionDC;

        private void ImagesAddDC()
        {

            if (_serverConnectionDC != null)
            {
                using (Microsoft.Data.SqlClient.SqlConnection con = new SqlConnection(_serverConnectionDC.ConnectionString))
                {
                    con.Open();
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.PlotID.ToString();
                    string SQL = "SELECT EI.URI AS ResourceURI, 'Event' AS SourceType, case when E.CollectionYear is null then '' else cast(E.CollectionYear as varchar) end +'-' + " +
                        "case when E.CollectionMonth is null then '' else cast(E.CollectionMonth as varchar) end + '-' + " +
                        "case when E.CollectionDay is null then '' else cast(E.CollectionDay as varchar) end + ': ' + case when E.LocalityDescription is null then '' else E.LocalityDescription end as Source, " +
                        "dbo.BaseURL() + cast(S.CollectionSpecimenID as varchar) AS LinkToDC, case when [Description] is null then '' else cast([Description] as nvarchar(Max)) end AS EXIF " +
                        "FROM CollectionEventImage AS EI INNER JOIN " +
                        "CollectionEventLocalisation AS L ON EI.CollectionEventID = L.CollectionEventID INNER JOIN " +
                        "CollectionEvent AS E ON EI.CollectionEventID = E.CollectionEventID AND L.CollectionEventID = E.CollectionEventID " +
                        "INNER JOIN CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID " +
                        "WHERE(L.LocalisationSystemID = 13) AND(L.Location2 = N'" + URL + "')  AND (EI.URI LIKE '%.jpg' OR EI.URI LIKE '%.png' OR EI.URI LIKE '%.gif') " +
                        "UNION " +
                        "SELECT I.URI AS ResourceURI, CASE WHEN I.SpecimenPartID IS NULL AND I.IdentificationUnitID IS NULL THEN 'Specimen' ELSE CASE WHEN I.IdentificationUnitID IS NULL THEN 'Part' ELSE 'Unit' END END AS SourceType, " +
                        "case when E.CollectionYear is null then '' else cast(E.CollectionYear as varchar) end +'-' + " +
                        "case when E.CollectionMonth is null then '' else cast(E.CollectionMonth as varchar) end + '-' + " +
                        "case when E.CollectionDay is null then '' else cast(E.CollectionDay as varchar) end + ': ' + " +
                        "case when S.AccessionNumber is null then 'ID: ' + cast(S.CollectionSpecimenID as varchar) else S.AccessionNumber end + " +
                        "CASE WHEN I.IdentificationUnitID IS NULL THEN '' ELSE '. ' + U.TaxonomicGroup + ': ' + U.LastIdentificationCache END + " +
                        "case when I.SpecimenPartID IS NULL then '' else '. ' + P.MaterialCategory + ': ' + P.StorageLocation end " +
                        "AS Source , dbo.BaseURL() + cast(I.CollectionSpecimenID as varchar) AS LinkToDC, case when [Description] is null then '' else cast([Description] as nvarchar(Max)) end AS EXIF " +
                        "FROM CollectionEventLocalisation AS L INNER JOIN " +
                        "CollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID INNER JOIN " +
                        "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID INNER JOIN " +
                        "CollectionSpecimenImage AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID LEFT OUTER JOIN " +
                        "CollectionSpecimenPart AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID AND I.SpecimenPartID = P.SpecimenPartID LEFT OUTER JOIN " +
                        "IdentificationUnit AS U ON I.CollectionSpecimenID = U.CollectionSpecimenID AND I.IdentificationUnitID = U.IdentificationUnitID " +
                        "WHERE(L.LocalisationSystemID = 13) AND (L.Location2 = N'" + URL + "')  AND (I.URI LIKE '%.jpg' OR I.URI LIKE '%.png' OR I.URI LIKE '%.gif') ";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _dataTableImages, con);
                    con.Close();
                }
            }
        }


        private ImageSouce SourceOfImage(string Source)
        {
            ImageSouce source = ImageSouce.Plot;
            switch (Source)
            {
                case "Event":
                    source = ImageSouce.Event;
                    break;
                case "Part":
                    source = ImageSouce.Part;
                    break;
                case "Unit":
                    source = ImageSouce.Unit;
                    break;
                case "Specimen":
                    source = ImageSouce.Specimen;
                    break;
            }
            return source;
        }


        private void buttonImageDCunitValues_Click(object sender, EventArgs e)
        {
            if (this.listBoxImages.SelectedIndex > -1 && this.listBoxImages.Items.Count == this._dataTableImages.Rows.Count)
            {
                System.Data.DataRow R = this._dataTableImages.Rows[this.listBoxImages.SelectedIndex];
                DiversityWorkbench.CollectionSpecimen collectionSpecimen = new DiversityWorkbench.CollectionSpecimen(_serverConnectionDC);
                DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(R["LinkToDC"].ToString(), collectionSpecimen);
                f.ShowDialog();
            }
        }


        #endregion

        #endregion

        #region Backlinks

        public void SetHeight(int Height)
        {

        }

        public DiversityWorkbench.WorkbenchUnit.ModuleType GetModuleType()
        {
            return DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots;
        }

        private void initBackLinks()
        {
            this.initBackLinksCollection();
        }


        private void initBackLinksCollection()
        {
            if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection) || this._ShowSpecimenImages)
            {
                if (!this.tabControlDetails.TabPages.Contains(this.tabPageSpecimen))
                    this.tabControlDetails.TabPages.Add(this.tabPageSpecimen);
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection))
                {
                    if (!this.tabControlSpecimen.TabPages.Contains(this.tabPageSpecimenDetails))
                    {
                        this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenDetails);
                        this.userControlModuleBackLinksCollection.SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, this, false);
                        this.scanModules();
                    }
                }
                else
                {
                    if (this.tabControlSpecimen.TabPages.Contains(this.tabPageSpecimenDetails))
                        this.tabControlSpecimen.TabPages.Remove(this.tabPageSpecimenDetails);
                }
                if (_ShowSpecimenImages)
                {
                    if (!this.tabControlSpecimen.TabPages.Contains(this.tabPageSpecimenImages))
                        this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenImages);
                }
                else
                {
                    if (this.tabControlSpecimen.TabPages.Contains(this.tabPageSpecimenImages))
                        this.tabControlSpecimen.TabPages.Remove(this.tabPageSpecimenImages);
                }
            }
            else
            {
                if (this.tabControlDetails.TabPages.Contains(this.tabPageSpecimen))
                    this.tabControlDetails.TabPages.Remove(this.tabPageSpecimen);
            }
            this.diversityCollectionToolStripMenuItem.Checked = DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection);
        }

        private void scanModules()
        {
            try
            {
                // Markus 8.5.23: Setzen den Modultyps falls falsch
                if (this.userControlModuleBackLinksCollection.BackLinkModuleType != DiversityWorkbench.WorkbenchUnit.ModuleType.Collection)
                {
                    this.userControlModuleBackLinksCollection.SetModuleParameter(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection, this, false);
                    this.scanModules();
                }
                if (DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Collection))
                {
                    string URL = DiversityWorkbench.Settings.ServerConnection.BaseURL + this.PlotID.ToString();
                    this.userControlModuleBackLinksCollection.SearchForBackLinks(URL);
                }
            }
            catch (System.Exception ex)
            {

            }
        }


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
                helpProvider.SetHelpKeyword(this.tableLayoutPanelHeader, "editplots_dsp");

                helpProvider.SetHelpKeyword(userControlQueryList, "query_dsp");

                helpProvider.SetHelpKeyword(this.tabPageLocalisation, "localisation_dsp");
                helpProvider.SetHelpKeyword(this.tabPageProperty, "properties_dsp");
                helpProvider.SetHelpKeyword(this.tabPageResources, "resources_dsp");
                helpProvider.SetHelpKeyword(this.tabPageSpecimen, "backlinks_dsp");

                helpProvider.SetHelpKeyword(this.groupBoxHierarchy, "hierarchy_dsp");
                helpProvider.SetHelpKeyword(this.groupBoxProjects, "projects_dsp");


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
                //else
                //{
                //    DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                //    if (manual != null)
                //    {
                //        await manual.addKeyDownF1ToForm();
                //    }
                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion



    }
}
