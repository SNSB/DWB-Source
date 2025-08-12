#define AgentIdentifierIncluded

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DiversityCollection.CacheDatabase.Diagnostics;
using DiversityWorkbench.DwbManual;
using Npgsql;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormCacheDatabase : Form, InterfaceCacheDatabase
    {

        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCacheDatabase;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterProjectProxy;
        private System.Data.DataTable _DtMaterialNotTransferred;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterLocalisation;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonomicGroup;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterKingdoms;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonSynonymySource;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterMaterialCategory;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumKingdom;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumPreparationType;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumRecordBasis;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumTaxonomicGroup;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonomicGroupInProject;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterLocalisationSystem;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAnonymCollector;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterProjectPublished;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterScientificTermSource;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterGazetteerSource;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterPlotSource;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentSource;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterReferencesSource;

        private bool? _AllowAdministration;

        private bool _PostgresDatabaseNeedsUpdate = false;

        private bool _TransferFinished = false;

        private bool _FilterCollectionForUpdate = false;
        private bool _FilterTaxaForUpdate = false;
        private bool _FilterPlotsForUpdate = false;
        private bool _FilterAgentsForUpdate = false;
        private bool _FilterTermsForUpdate = false;
        private bool _FilterGazetteerForUpdate = false;
        private bool _FilterDescriptionsForUpdate = false;
        private bool _FilterReferencesForUpdate = false;

        public bool TransferFinished
        {
            get { return _TransferFinished; }
            //set { _TransferFinished = value; }
        }

        // #174
        private bool _HasAccess = false;
        public bool HasAccess
        {
            get { return _HasAccess; }
        }

        #endregion

        #region Construction

        public FormCacheDatabase()
        {
            InitializeComponent();
            this.initForm();
        }

        public FormCacheDatabase(string[] args)
        {
            InitializeComponent();
            DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = true;
            this.Hide();
            CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Started FormCacheDatabase");
            bool HasAccess = false;
            if (this.initDatabase(ref HasAccess))
            {
                this.initProjects();
                this.initPostgres();
                this.initGazetteer();
                this.initAgentSources();
                this.initGazetteerSources();
                this.initReferenceTitleSources();
                this.initTaxonSources();
                this.initTermSources();
                this.initPlotSources();

                if (this.UpdateCheckForDatabase())
                {
                    CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Transfer failed. Update cache database to current version");
                    //this.WriteToLog(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Transfer failed. Update cache database to current version");
                    this.Close();
                }
            }
            else
            {
                CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Failed to initialize cache database\r\n" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName);
                this.Close();
            }
            System.Collections.Generic.List<bool> L = new List<bool>();
            try
            {
                if (args.Length > 1 && args[0].Length > 0)
                {
                    #region Old version - not supported any longer

		            string TransferSelection = args[1];
                    if (TransferSelection.ToUpper().IndexOf("A") == -1 &&
                        TransferSelection.ToUpper().IndexOf("D") == -1 &&
                        TransferSelection.ToUpper().IndexOf("G") == -1 &&
                        TransferSelection.ToUpper().IndexOf("S") == -1 &&
                        TransferSelection.ToUpper().IndexOf("T") == -1 &&
                        TransferSelection.ToUpper().IndexOf("C") == -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Nothing to transfer. No data domains selected");
                        this.Close();
                    }
                    string Target = args[0].ToLower().Trim();
                    bool ForPostgres = false;
                    if (Target == "cachetopostgres")
                    {
                        ForPostgres = true;
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer from cache to Postgres");
                    }
                    else if (Target == "databasetocache")
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer from source to cache");
                    else
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer from source to cache and from cache to Postgres");
                    }

                    // Agents
                    if (TransferSelection.ToUpper().IndexOf("A") > -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer of agent sources");
                        if (TransferSelection.ToUpper().IndexOf("A1") > -1)
                            this._FilterAgentsForUpdate = true;
                        this.TransferAgentSources(!ForPostgres, ForPostgres);
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Agent sources transferred");
                    }

                    // References
                    if (TransferSelection.ToUpper().IndexOf("R") > -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer of reference sources");
                        if (TransferSelection.ToUpper().IndexOf("A1") > -1)
                            this._FilterReferencesForUpdate = true;
                        this.TransferReferenceTitleSources(true, !ForPostgres, ForPostgres);
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Reference sources transferred");
                    }

                    // Descriptions
                    if (TransferSelection.ToUpper().IndexOf("D") > -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer of description sources");
                        //if (TransferSelection.ToUpper().IndexOf("D1") > -1)
                        //    this._FilterGazetteerForUpdate = true;
                        //this.TransferGazetteerSources(true, !ForPostgres, ForPostgres);
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Description sources transferred");
                    }

                    // Gazetteer
                    if (TransferSelection.ToUpper().IndexOf("G") > -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer of gazetteer sources");
                        if (TransferSelection.ToUpper().IndexOf("G1") > -1)
                            this._FilterGazetteerForUpdate = true;
                        this.TransferGazetteerSources(/*true,*/ !ForPostgres, ForPostgres);
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Gazetteer sources transferred");
                    }

                    // ScientificTerms
                    if (TransferSelection.ToUpper().IndexOf("S") > -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer of scientific term sources");
                        if (TransferSelection.ToUpper().IndexOf("S1") > -1)
                            this._FilterTermsForUpdate = true;
                        this.TransferScientificTermSources(!ForPostgres, ForPostgres);
                        // Markus 28.3.25: Falsche Meldung korrigiert #49
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Scientific term sources transferred");
                    }

                    // Taxonomy
                    if (TransferSelection.ToUpper().IndexOf("T") > -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer of taxonomy sources");
                        if (TransferSelection.ToUpper().IndexOf("T1") > -1)
                            this._FilterTaxaForUpdate = true;
                        this.TransferTaxonomySources(!ForPostgres, ForPostgres);
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Taxonomy sources transferred");
                    }

                    // Collection
                    if (TransferSelection.ToUpper().IndexOf("C") > -1)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting transfer of collection projects");
                        if (TransferSelection.ToUpper().IndexOf("C1") > -1)
                            this._FilterCollectionForUpdate = true;
                        //string ReportFile = this.TransferProtocolFileName();
                        //System.IO.StreamWriter sw;
                        //if (System.IO.File.Exists(ReportFile))
                        //    sw = new System.IO.StreamWriter(ReportFile, true, System.Text.Encoding.UTF8);
                        //else
                        //    sw = new System.IO.StreamWriter(ReportFile, false, System.Text.Encoding.UTF8);
                        //this.InitTransferCollectionProjectProtocol(ref sw);
                        this.TransferCollectionProjects(!ForPostgres, ForPostgres);//, /*true,*/ ref sw);
                        //sw.Close();
                        //sw.Dispose();
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Collection projects transferred");
                    }
                    this._TransferFinished = true;
                    if (ForPostgres)
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Transfer from cache to Postgres finished");
                    else
                        CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Transfer from source to cache finished");
 
	                #endregion                
                }
                else
                {
                    CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Starting cache transfer");
                    this.StartAutoTransfer(CacheDB.IncludeCacheDB, CacheDB.IncludePostgres);// true, true);//, true);
                    this._TransferFinished = true;
                    CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "Cache transfer finished");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Form

        private void FormCacheDatabase_Load(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAnonymCollector, this.dataSetCacheDatabase.AnonymCollector, "SELECT CollectorsName, Anonymisation FROM AnonymCollector", DiversityWorkbench.Settings.ConnectionString);
                // Check existance of tables
                string SqlCheck = "select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'ProjectPublished'";
                string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCheck);
                if (Check == "1")
                {
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterProjectPublished, this.dataSetCacheDatabase.ProjectPublished, "SELECT ProjectID, Project, CoordinatePrecision, ProjectURI, LastUpdatedWhen, LastUpdatedBy FROM ProjectPublished", DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initForm()
        {
            try
            {
                bool OK = true;

                // removing pages that are not used so far
                this.tabControlModuleSources.TabPages.Remove(this.tabPageDescriptions);
                // this.tabControlModuleSources.TabPages.Remove(this.tabPagePlots);

                this.labelDatabase.Text = DiversityWorkbench.Settings.DatabaseName;
                this.labelProjectDatabase.Text = DiversityWorkbench.Settings.DatabaseName;
                DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = false;
                if (this.initDatabase(ref _HasAccess)) // #174
                {
                    this.labelCacheDB.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                    this.labelProjectCacheDB.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                    this.initProjects();
                    this.initAnonymCollector();
                    //this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                }
                else if (HasAccess)
                {
                    if (System.Windows.Forms.MessageBox.Show("Failed to initialize cache database\r\n" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "\r\nDelete this entry?", "Delete entry?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string SQL = "DELETE FROM CacheDatabase2";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    this.Close();
                    OK = false;
                }
                else
                {
                    OK = false;
                    this.Close();
                }
                if (OK)
                {
                    this.initPostgres();
                    this.initTaxonSources();
                    this.initTermSources();
                    this.initGazetteerSources();
                    this.initGazetteer();
                    this.initAgentSources();
                    this.initReferenceTitleSources();
                    this.initPlotSources();
                    this.initDiagnostics();
                    if (!this.UpdateCheckForDatabase())
                    {
                        this.CheckFunctionProjectsDB();
                    }
                    try
                    {
                        string SQL = "select user_name()";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        string User = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        con.Close();
                        this.toolStripButtonDatabaseTools.Visible = User == "dbo";
                    }
                    catch(System.Exception ex) { this.toolStripButtonDatabaseTools.Visible = false;  DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            this.initOldVersion();
        }

        private void setMessages()
        {
            string SQL = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
            {
                this.UpdateCheckForDatabase();
                try
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    con.Open();
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Close();
                    con.Dispose();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void FormCacheDB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.projectPublishedBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectPublishedBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            if (this._SqlDataAdapterProjectProxy != null)
            {
                this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDatabase.ProjectPublished);
            }
        }

        private void initGazetteer(/*bool ProcessOnly*/)
        {
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionGazetteer() == null)
                    this.buttonNoGazetteer.Visible = true;
                else
                    this.buttonNoGazetteer.Visible = false;
            }
        }

        private void buttonNoGazetteer_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormConnectionAdministration f = new DiversityWorkbench.Forms.FormConnectionAdministration(this.helpProvider.HelpNamespace);
            f.ShowDialog();
            this.initGazetteer();
        }

        private void CheckFunctionProjectsDB()
        {
            System.Data.DataTable dtCheck = new DataTable();
            string Message = "";
            int Fit = 0;
            int NoFit = 0;
            string ProjectsDB = "";
            string BaseURL = "";
            if (this.FunctionProjectsDatabaseIsOK(ref dtCheck, ref Message, ref Fit, ref NoFit, ref ProjectsDB, ref BaseURL))
            {
                if (this.FunctionProjectsStableIdentifierIsOK(ref dtCheck, ref Message, ref Fit, ref NoFit, ref ProjectsDB, ref BaseURL))
                    this.buttonCheckProjectsDatabase.Visible = false;
                else
                {
                    this.buttonCheckProjectsDatabase.Text = "Check stable identifier in Projects database";
                    this.buttonCheckProjectsDatabase.Tag = "StableIdentifier";
                    this.buttonCheckProjectsDatabase.Visible = true;
                }
            }
            else
                this.buttonCheckProjectsDatabase.Visible = true;
        }

        private bool FunctionProjectsDatabaseIsOK(ref System.Data.DataTable dtNoFit, ref string Message, ref int Fit, ref int NoFit, ref string ProjectDB, ref string BaseURL)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                string SQL = "SELECT [dbo].[ProjectsDatabase] ()";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                ProjectDB = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (ProjectDB == string.Empty)
                {
                    return false;
                }
                C.CommandText = "SELECT " + ProjectDB + ".dbo.BaseURL()";
                BaseURL = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (BaseURL == string.Empty)
                {
                    return false;
                }
                C.CommandText = "SELECT count(*) " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                    "where P.ProjectURI  COLLATE DATABASE_DEFAULT LIKE " + ProjectDB + ".dbo.BaseURL() + '%'  COLLATE DATABASE_DEFAULT ";
                string FittingProject = C.ExecuteScalar()?.ToString();
                C.CommandText = "SELECT count(*) " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                    "where P.ProjectURI  COLLATE DATABASE_DEFAULT <> '' AND P.ProjectURI  COLLATE DATABASE_DEFAULT not LIKE " + ProjectDB + ".dbo.BaseURL() + '%'  COLLATE DATABASE_DEFAULT";
                string NotFitting = C.ExecuteScalar()?.ToString();
                if (int.TryParse(FittingProject, out Fit) && int.TryParse(NotFitting, out NoFit))
                {
                    if (NoFit > 0 || Fit == 0)
                    {
                        OK = false;
                    }
                    if (NoFit > 0)
                    {
                        SQL = "SELECT P.Project, P.ProjectURI " +
                            "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                            "where P.ProjectURI  COLLATE DATABASE_DEFAULT <> '' AND P.ProjectURI  COLLATE DATABASE_DEFAULT not LIKE " + ProjectDB + ".dbo.BaseURL() + '%'  COLLATE DATABASE_DEFAULT " +
                            "ORDER BY P.Project";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                        ad.Fill(dtNoFit);
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Message = ex.Message;
            }
            return OK;
        }

        private bool FunctionProjectsStableIdentifierIsOK(ref System.Data.DataTable dtNoFit, ref string Message, ref int Fit, ref int NoFit, ref string ProjectDB, ref string BaseURL)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                string SQL = "SELECT [dbo].[ProjectsDatabase] ()";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                ProjectDB = C.ExecuteScalar()?.ToString() ?? string.Empty;
                C.CommandText = "SELECT min(projectid) from ProjectPublished";
                string ProjectID = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (ProjectDB == string.Empty || ProjectID == string.Empty)
                {
                    Message = "Project database or stable identifier is empty.";
                    return false;
                }
                C.CommandText = "select " + ProjectDB + ".dbo.StableIdentifier (" + ProjectID + ") ";
                //string StableIdenfier = C.ExecuteScalar()?.ToString();
                string FittingProject = C.ExecuteScalar()?.ToString();
                C.CommandText = "SELECT count(*) " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                    "where P.ProjectURI  COLLATE DATABASE_DEFAULT <> '' AND P.ProjectURI  COLLATE DATABASE_DEFAULT not LIKE " + ProjectDB + ".dbo.BaseURL() + '%'  COLLATE DATABASE_DEFAULT";
                string NotFitting = C.ExecuteScalar()?.ToString();
                if (int.TryParse(FittingProject, out Fit) && int.TryParse(NotFitting, out NoFit))
                {
                    if (NoFit > 0 || Fit == 0)
                    {
                        OK = false;
                    }
                    if (NoFit > 0)
                    {
                        SQL = "SELECT P.Project, P.ProjectURI " +
                            "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                            "where P.ProjectURI  COLLATE DATABASE_DEFAULT <> '' AND P.ProjectURI  COLLATE DATABASE_DEFAULT not LIKE " + ProjectDB + ".dbo.BaseURL() + '%'  COLLATE DATABASE_DEFAULT " +
                            "ORDER BY P.Project";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                        ad.Fill(dtNoFit);
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Message = ex.Message;
            }
            return OK;
        }

        private void buttonCheckProjectsDatabase_Click(object sender, EventArgs e)
        {
            if (buttonCheckProjectsDatabase.Tag.ToString() == "StableIdentifier")
            {
                System.Windows.Forms.MessageBox.Show("Please turn to the manual of DiversityProjects to set the stable identifier");
            }
            else
            {
                System.Data.DataTable dtCheck = new DataTable();
                string Message = "";
                int Fit = 0;
                int NoFit = 0;
                string ProjectDB = "";
                string BaseURL = "";
                this.FunctionProjectsDatabaseIsOK(ref dtCheck, ref Message, ref Fit, ref NoFit, ref ProjectDB, ref BaseURL);
                if (dtCheck.Rows.Count > 0)
                {
                    DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent("Projects database: " + ProjectDB, "These projects as stored in table ProjectProxy do not match the link to the projects database (" + BaseURL + ")", dtCheck);
                    f.ShowDialog();
                }
                else if (Message.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show(Message);
                }
                else if (Fit == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No project with a fitting link to the projects database were found");
                }
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
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

        private void FormCacheDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormFunctions.updateTable(this.dataSetCacheDatabase, "AnonymCollector", this._SqlDataAdapterAnonymCollector, this.BindingContext);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormSettings f = new FormSettings();
            f.ShowDialog();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private bool _InitSuccessful = true;
        public bool InitSuccessful { get { return _InitSuccessful; } }

        #endregion

        #region Properties

        private bool AllowAdministration
        {
            get
            {
                if (this._AllowAdministration == null)
                {
                    if (DiversityCollection.CacheDatabase.CacheDB.DatabaseRoles().Contains("CacheAdmin"))
                        this._AllowAdministration = true;
                    else this._AllowAdministration = false;
                }
                return (bool)this._AllowAdministration;
            }
        }

        #endregion

        #region Cache database

        #region Update

        private void buttonUpdateDatabase_Click(object sender, EventArgs e)
        {
            this.UpdateDatabase();
        }

        private void setUpdateControls()
        {
            string SQL = "select user_name()";
            string User = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    User = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                    if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
                    {
                        if (DiversityCollection.CacheDatabase.CacheDB.DatabaseRoles().Contains("CacheAdministrator") || 
                            DiversityCollection.CacheDatabase.CacheDB.DatabaseRoles().Contains("CacheAdmin") ||
                            DiversityCollection.CacheDatabase.CacheDB.DatabaseRoles().Contains("db_owner") ||
                            User == "dbo")
                        {
                            bool Update = this.UpdateCheckForDatabase();
                            this.buttonUpdateDatabase.Visible = Update;
                            this.textBoxCurrentDatabaseVersion.Visible = true;// this.buttonUpdateDatabase.Visible;
                            this.labelCurrentDatabaseVersion.Visible = true;// this.buttonUpdateDatabase.Visible;
                            this.textBoxAvailableDatabaseVersion.Visible = Update;
                            this.textBoxAvailableDatabaseVersion.Text = DiversityCollection.Properties.Settings.Default.SqlServerCacheDBVersion;
                            this.textBoxAvailableDatabaseVersion.Text = DiversityCollection.Properties.Settings.Default.SqlServerCacheDBVersion;

                            System.Data.DataTable dtCheck = new DataTable();
                            string Message = "";
                            int Fit = 0;
                            int NoFit = 0;
                            string ProjectsDB = "";
                            string BaseURL = "";
                            this.FunctionProjectsDatabaseIsOK(ref dtCheck, ref Message, ref Fit, ref NoFit, ref ProjectsDB, ref BaseURL);
                        }
                    }
                    else
                    {
                        bool IsAdmin = false;
                        if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator") || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                            IsAdmin = true;
                        if (User == "dbo" || IsAdmin)
                        {
                            this.buttonUpdateDatabase.Visible = this.UpdateCheckForDatabase();
                            this.textBoxCurrentDatabaseVersion.Visible = this.buttonUpdateDatabase.Visible;
                            this.labelCurrentDatabaseVersion.Visible = this.buttonUpdateDatabase.Visible;
                            this.textBoxAvailableDatabaseVersion.Visible = this.buttonUpdateDatabase.Visible;
                        }
                    }
                }
                catch (System.Exception ex)
                { }
            }
        }

        private bool UpdateCheckForDatabase(/*bool ProcessOnly*/)
        {
            bool Update = false;
            string SQL = "if (select count(*) from INFORMATION_SCHEMA.ROUTINES R " +
                "where R.ROUTINE_TYPE = 'FUNCTION' " +
                "and R.SPECIFIC_NAME = 'Version' " +
                "and R.SPECIFIC_SCHEMA = 'dbo') = 1 " +
                "select dbo.version() " +
                "else " +
                "select '00.00.00'";
            string Version = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Version = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
            }
            catch (System.Exception ex)
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "The connection " + DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB + " is not valid.");
                else
                    System.Windows.Forms.MessageBox.Show("The connection " + DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB + " is not valid.");
                return false;
            }
            if (Version != DiversityCollection.Properties.Settings.Default.SqlServerCacheDBVersion)
            {
                int DBversionMajor;
                int DBversionMinor;
                int DBversionRevision;
                int C_versionMajor;
                int C_versionMinor;
                int C_versionRevision;
                string[] VersionDBParts;
                if (Version.IndexOf('.') > -1) VersionDBParts = Version.Split(new Char[] { '.' });
                else if (Version.IndexOf('/') > -1) VersionDBParts = Version.Split(new Char[] { '/' });
                else VersionDBParts = Version.Split(new Char[] { ' ' });
                if (!int.TryParse(VersionDBParts[0], out DBversionMajor)) return false;
                if (!int.TryParse(VersionDBParts[1], out DBversionMinor)) return false;
                if (!int.TryParse(VersionDBParts[2], out DBversionRevision)) return false;

                string[] VersionC_Parts = DiversityCollection.Properties.Settings.Default.SqlServerCacheDBVersion.Split(new Char[] { '.' });
                if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                if (C_versionMajor > DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                    Update = true;
                if (DBversionMajor == 2)
                {
                    Version = "00.00.00";
                    Update = true;
                }
            }
            return Update;
        }

        private void UpdateDatabase()
        {
            try
            {
                // Checking permission
                bool OK = false;
                if (!DiversityWorkbench.Settings.IsTrustedConnection && DiversityWorkbench.Settings.DatabaseUser.Length > 0)
                {
                    OK = DiversityWorkbench.Database.LoginIsSysAdmin(DiversityWorkbench.Settings.DatabaseUser);
                    if (!OK)
                    {
                        // Test ist user is CacheAdmin
                        string SqlRole = "SELECT sysusers_1.name " +
                            "FROM sysmembers INNER JOIN " +
                            "sysusers ON sysmembers.memberuid = sysusers.uid INNER JOIN " +
                            "sysusers sysusers_1 ON sysmembers.groupuid = sysusers_1.uid " +
                            "WHERE (replace(sysusers.name, '\', '\\') = N'" + DiversityWorkbench.Settings.DatabaseUser + "') AND sysusers_1.name = 'CacheAdmin'" +
                            "ORDER BY sysusers_1.name";
                        string CacheAdmin = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlRole);
                        if (CacheAdmin == "CacheAdmin")
                            OK = true;
                        else
                        {
                            string User = System.Environment.UserName;
                            OK = DiversityWorkbench.Database.LoginIsSysAdmin(User);
                        }
                    }
                }
                else
                {
                    string User = System.Environment.UserName;
                    OK = DiversityWorkbench.Database.LoginIsSysAdmin(User);
                    if (!OK)
                    {
                        string Sql = "select user_name()";
                        User = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sql);
                        OK = DiversityWorkbench.Database.LoginIsSysAdmin(User);
                        if (!OK && User == "dbo")
                            OK = true;
                    }
                }
                if (!OK)
                {
                    System.Windows.Forms.MessageBox.Show("Sorry, but your permissions are not sufficient for structural changes in the database.\r\n\r\nPlease turn to your Admin", "Missing permission", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string DatabaseCurrentVersion = "";
                string SQL = "select dbo.Version()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    DatabaseCurrentVersion = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                }
                catch { }
                DatabaseCurrentVersion = DatabaseCurrentVersion.Replace(".", "").Replace("/", "");
                string DatabaseFinalVersion = DiversityCollection.Properties.Settings.Default.SqlServerCacheDBVersion;
                DatabaseFinalVersion = DatabaseFinalVersion.Replace(".", "").Replace("/", "");

                // Ignore former version
                if (DatabaseCurrentVersion.StartsWith("02"))
                {
                    SQL = "ALTER FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '00.00.00' END";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    DatabaseCurrentVersion = "000000";
                }

                // check resouces for update scripts
                System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
                System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
                System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdate_") && !Versions.ContainsKey(de.Key.ToString()))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    DiversityWorkbench.Forms.FormUpdateDatabase f = new DiversityWorkbench.Forms.FormUpdateDatabase(DiversityCollection.CacheDatabase.CacheDB.DatabaseName, DiversityCollection.Properties.Settings.Default.SqlServerCacheDBVersion, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, Versions, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                    f.ShowDialog();
                    bool HasAccess = true;
                    if (f.Reconnect) this.setDatabase(ref HasAccess);
                    this.UpdateCheckForDatabase();
                    this.initTaxonSources();
                    this.initTermSources();
                    this.initGazetteerSources();
                    this.initGazetteer();
                    this.initAgentSources();
                    this.initReferenceTitleSources();
                    this.initPlotSources();
                    this.initDiagnostics();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Database

        private bool initDatabase(/*bool ProcessOnly, */ref bool HasAccess)
        {
            bool OK = true;
            try
            {
                string SQL = "SELECT DatabaseName, Server, Port, Version FROM CacheDatabase2";
                this._SqlDataAdapterCacheDatabase = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapterCacheDatabase.Fill(this.dataSetCacheDatabase.CacheDatabase);
                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && this.dataSetCacheDatabase.CacheDatabase.Rows.Count == 0)
                {
                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && System.Windows.Forms.MessageBox.Show("So far no cache database has been defined.\r\nDo you want to create a cache database?", "Create cache database?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string ProjectsDatabase = this.ModuleDatabase(DiversityWorkbench.WorkbenchUnit.ModuleType.Projects);
                        if (ProjectsDatabase.Length == 0)
                            return false;
                        if (!this.CheckStableIdentifierInProjectsDatabase(ProjectsDatabase))
                        {
                            System.Windows.Forms.MessageBox.Show("The projects database " + ProjectsDatabase + " has no definition for the stable identifier\r\nPlease set the stable identifier in this database", "Missing stable identifier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        string AgentsDatabase = this.ModuleDatabase(DiversityWorkbench.WorkbenchUnit.ModuleType.Agents);
                        if (AgentsDatabase.Length == 0)
                            return false;
                        string DBname = DiversityWorkbench.Settings.DatabaseName;
                        if (DBname.IndexOf("_") > -1)
                            DBname = DBname.Replace("_", "Cache_");
                        else DBname += "Cache";
                        DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Cache database", "Please enter the name of the new cache database", DBname);
                        //if (System.Windows.Forms.MessageBox.Show("The name of the new cache database:\r\n" + DBname, "New cache database", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                        {
                            DBname = f.String;
                            System.Data.DataTable dtFiles = new DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("exec sp_helpfile", DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(dtFiles);
                            string DataFile = dtFiles.Rows[0]["filename"].ToString();
                            // Check if file exists
                            System.IO.FileInfo DB = new System.IO.FileInfo(DataFile);
                            if (DB.Exists)
                            {
                                ///ToDo:
                                // remove file if possible
                            }
                            if (DataFile.IndexOf("\\" + DiversityWorkbench.Settings.DatabaseName + ".") > -1)
                                DataFile = DataFile.Replace("\\" + DiversityWorkbench.Settings.DatabaseName + ".", "\\" + DBname + ".");
                            else
                            {
                                DataFile = DataFile.Substring(0, DataFile.LastIndexOf("\\")) + "\\" + DBname + ".mdf";
                            }
                            SQL = string.Format("SELECT [collation_name] FROM [master].[sys].[databases] WHERE [name]='{0}'", DiversityWorkbench.Settings.DatabaseName);
                            string collation = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, DiversityWorkbench.Settings.ConnectionString);
                            string LogFile = DataFile.Substring(0, DataFile.LastIndexOf(".")) + "_log.ldf";
                            SQL = "CREATE DATABASE [" + DBname + "] " +
                                //"CONTAINMENT = NONE " +
                                "ON  PRIMARY  " +
                                "( NAME = N'" + DBname + "', FILENAME = N'" + DataFile + "' , SIZE = 5032KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) " +
                                "LOG ON  " +
                                "( NAME = N'" + DBname + "_Log', FILENAME = N'" + LogFile + "' , SIZE = 504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)" +
                                "COLLATE " + collation;
                            try
                            {
                                string Message = "";
                                // Check if Database exists
                                bool DbExits = false;
                                string SqlCheck = "select count(*) from sys.databases d where d.name = '" + DBname + "'";
                                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlCheck);
                                if (Result == "1")
                                    DbExits = true;
                                if (DbExits && DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0 && !DiversityCollection.CacheDatabase.CacheDB.HasAccessToDatabase(ref Message))
                                {
                                    Message = "Login to CacheDatabase failed\r\nPlease contact your administator\r\n" + Message;
                                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                        DiversityCollection.CacheDatabase.CacheDB.LogEvent("initDatabase(bool ProcessOnly, ref bool HasAccess)", DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, Message);
                                    else
                                        System.Windows.Forms.MessageBox.Show(Message);
                                }
                                else
                                {
                                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                                    {
                                        SQL = "ALTER DATABASE " + DBname + " SET RECOVERY SIMPLE;";
                                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                        SQL = "INSERT INTO CacheDatabase2 (Server, DatabaseName, Port, Version) "
                                        + "VALUES ('" + DiversityWorkbench.Settings.DatabaseServer + "', '" + DBname + "', " + DiversityWorkbench.Settings.DatabasePort.ToString() + ", '00.00.00')";
                                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                        this._SqlDataAdapterCacheDatabase.Fill(this.dataSetCacheDatabase.CacheDatabase);
                                        this.setDatabase(ref HasAccess);
                                        // Markus 15.4.2021 - verlagert aus Skript DiversityCollectionCacheUpdate_010000_to_010001
                                        OK = this.DefineModuleDatabase(ProjectsDatabase, DiversityWorkbench.WorkbenchUnit.ModuleType.Projects);
                                        if (OK)
                                            // Markus 15.4.2021 - verlagert aus Skript DiversityCollectionCacheUpdate_010028_to_010029
                                            OK = this.DefineModuleDatabase(AgentsDatabase, DiversityWorkbench.WorkbenchUnit.ModuleType.Agents);
                                    }
                                    else
                                    {
                                        if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                            DiversityCollection.CacheDatabase.CacheDB.LogEvent("initDatabase(bool ProcessOnly, ref bool HasAccess)", DBname, "Creation of cache database failed: " + Message + "\r\nYour database server may be of an old version.\r\nPlease update to e.g. SQL-Server 2014");
                                        else
                                            System.Windows.Forms.MessageBox.Show("Creation of cache database failed: " + Message + "\r\nYour database server may be of an old version.\r\nPlease update to e.g. SQL-Server 2014");
                                        OK = false;
                                        _InitSuccessful = false;
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                OK = false;
                                _InitSuccessful = false;
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                        else
                            this.Close();
                    }
                    else
                    {
                        OK = false;
                        HasAccess = false;
                        _InitSuccessful = false;
                        //this.Close();
                    }
                }
                else if (this.dataSetCacheDatabase.CacheDatabase.Rows.Count == 1)
                {
                    OK = this.setDatabase(ref HasAccess);
                }
            }
            catch (System.Exception ex)
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    CacheDB.LogEvent(this.Name.ToString(), "FormCacheDatabase(string[] args)", "No cache databases available");
                else
                    System.Windows.Forms.MessageBox.Show("No cache databases available");
                OK = false;
                _InitSuccessful = false;
                this.Close();
            }
            return OK;
        }

        private string ModuleDatabase(DiversityWorkbench.WorkbenchUnit.ModuleType Type)
        {
            string DB = "";
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this.ModuleDatabasesAvailable(Type), "Diversity" + Type.ToString() + " database", "The cache database needs access to a Diversity" + Type.ToString() + " database on the same server.\r\nPlease select a " + Type.ToString() + " database from the list", true);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.SelectedString.Length > 0)
                DB = f.SelectedString;
            return DB;
        }

        private System.Collections.Generic.List<string> ModuleDatabasesAvailable(DiversityWorkbench.WorkbenchUnit.ModuleType Type)
        {
            System.Collections.Generic.List<string> DBs = new List<string>();
            string SQL = "select d.name from sys.databases d where d.name like 'Diversity" + Type.ToString() + "%' and d.state = 0 and d.is_read_only = 0 and d.user_access = 0";
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            foreach(System.Data.DataRow R in dt.Rows)
            {
                if (this.CheckModuleDatabase(R[0].ToString(), Type))
                    DBs.Add(R[0].ToString());
            }
            return DBs;
        }


        private bool CheckModuleDatabase(string Database, DiversityWorkbench.WorkbenchUnit.ModuleType Type)
        {
            bool OK = false;
            try
            {
                string SQL = "SELECT [" + Database + "].[dbo].[DiversityWorkbenchModule] ()";
                string Module = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Module.EndsWith(Type.ToString()))
                    OK = true;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private bool DefineModuleDatabase(string Database, DiversityWorkbench.WorkbenchUnit.ModuleType Type)
        {
            bool OK = false;
            string SQL = "CREATE FUNCTION [dbo].[" + Type.ToString() + "Database] () RETURNS nvarchar(255) as begin return '" + Database + "' end";
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            return OK;
        }

        private bool CheckStableIdentifierInProjectsDatabase(string Database)
        {
            bool OK = false;
            string SQL = "SELECT " + Database + ".dbo.StableIdentifierBase()";
            string SIB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);// DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (SIB.Length > 0)
                OK = true;
            return OK;
        }

        private bool setDatabase(ref bool HasAccess)
        {
            bool OK = true;
            try
            {
                if (this.dataSetCacheDatabase.CacheDatabase.Rows.Count == 1)
                {
                    System.Data.DataRow R = this.dataSetCacheDatabase.CacheDatabase.Rows[0];
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R["Server"].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R["DatabaseName"].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R["Port"].ToString());
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R["Version"].ToString();
                    string Message = "";
                    string SQL = "";
                    if (!DiversityCollection.CacheDatabase.CacheDB.HasAccessToDatabase(ref Message))
                    {
                        // Check if databse exists
                        SQL = "select count(*) from sys.databases d where d.name = '" + R["DatabaseName"].ToString() + "'";
                        string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (Result == "0")
                        {
                            Message = "The database " + R["DatabaseName"].ToString() + " does not exist. Do you want to remove this entry?";
                            if (System.Windows.Forms.MessageBox.Show(Message, "Remove wrong entry", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                            {
                                SQL = "DELETE FROM CacheDatabase2";
                                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                return false;
                            }
                        }
                        Message = "Login to CacheDatabase failed\r\nPlease contact your administator\r\n" + Message;
                        System.Windows.Forms.MessageBox.Show(Message);
                        HasAccess = false;
                        return false;
                    }
                    else { HasAccess = true; } // #174

                    SQL = "SELECT dbo.Version()";
                    this.textBoxCurrentDatabaseVersion.Text = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    if (this.textBoxCurrentDatabaseVersion.Text.Length == 0)
                    {
                        this.textBoxCurrentDatabaseVersion.Text = "00.00.00";
                        SQL = "CREATE FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '00.00.00' END";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            OK = false;
                        SQL = "SELECT dbo.BaseURL()";
                        string BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                        //Markus 9.11.2016 - transferred from skript DiversityCollectionCacheUpdate_010000_to_010001
                        SQL = "CREATE FUNCTION [dbo].[BaseURLofSource] () RETURNS  nvarchar (500) AS BEGIN RETURN '" + BaseURL + "' END";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            OK = false;
                        else
                        {
                            // Markus 4.3.2019 - Check if CacheUser exists - if not, create this role
                            SQL = "Select count(*) From sysusers Where issqlrole = 1 and name = 'CacheUser'";
                            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                            if (Result == "0")
                            {
                                SQL = "CREATE ROLE CacheUser";
                                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                            }
                            SQL = "GRANT EXEC ON [dbo].[BaseURLofSource] TO [CacheUser]";
                            if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                OK = false;
                            SQL = "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The URL of the source database as defined in the source database, e.g. http://tnt.diversityworkbench.de/collection' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'BaseURLofSource'";
                            if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                OK = false;
                        }
                    }
                    this.labelCacheDB.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;

                    SQL = "SELECT dbo.ProjectsDatabase()";
                    string ProjectsDB = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    this.textBoxProjectsDatabase.Text = ProjectsDB;
                    if (this.textBoxProjectsDatabase.Text.Length == 0)
                        this.textBoxProjectsDatabase.Text = "Not defined";
                }

                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    //this.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName + " on " + DiversityCollection.CacheDatabase.CacheDB.DatabaseServer;

                    //if (this._FormState == FormState.Administration || this._FormState == FormState.Both)
                    //    this.tabPageAdminCacheDB.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;

                    //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    //try
                    //{
                    //}
                    //catch (System.Exception ex)
                    //{
                    //}

                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            if (OK)
            {
                this.setMessages();
                this.setUpdateControls();
                this.initBioCASE();
            }
            return OK;
        }

        private void buttonLoginAdministration_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT dbo.DiversityWorkbenchModule()";
            string Module = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            C.CommandTimeout = 999;
            try
            {
                con.Open();
                Module = C.ExecuteScalar()?.ToString() ?? string.Empty;
            }
            catch (System.Exception ex) { }
            con.Close();
            if (Module != "DiversityCollectionCache")
            {
                System.Windows.Forms.MessageBox.Show("Please run updates first");
                return;
            }

            DiversityWorkbench.Forms.FormLoginAdministration f = new DiversityWorkbench.Forms.FormLoginAdministration(con);
            f.setHelpProvider(this.helpProvider.HelpNamespace, "Logins");
            f.SqlConnection = con;
            f.ShowDialog();
        }

        #endregion

        #endregion

        #region Projects

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceProject> _I_Projects;

        public bool initProjects(/*bool ProcessOnly*/)
        {
            bool OK = true;
            try
            {
                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                {
                    this.comboBoxTimerDay.SelectedIndex = 5;
                    System.DateTime DT = new DateTime(2000, 1, 1, 0, 30, 0);
                    this.dateTimePickerTimerTime.Value = DT;
                }
                this._I_Projects = new List<InterfaceProject>();
                if (DiversityCollection.CacheDatabase.CacheDB.DtProjects != null)
                {
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageProjects))
                        this.tabControlMain.TabPages.Add(this.tabPageProjects);
                    foreach(System.Windows.Forms.Control C in this.panelProjects.Controls)
                        C.Dispose();
                    this.panelProjects.Controls.Clear();
                    if (DiversityCollection.CacheDatabase.CacheDB.DtProjects.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in DiversityCollection.CacheDatabase.CacheDB.DtProjects.Rows)
                        {
                            string SQL = "SELECT COUNT(*) FROM ProjectProxy WHERE ProjectID = " + R[0].ToString();
                            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            if (Result == "1")
                            {
                                DiversityCollection.CacheDatabase.UserControlProject U = new UserControlProject(int.Parse(R["ProjectID"].ToString()), R["Project"].ToString(), this);
                                this.panelProjects.Controls.Add(U);
                                U.Dock = DockStyle.Top;
                                U.BringToFront();
                                U.setHelp("Cache database projects");
                                this._I_Projects.Add(U);
                            }
                        }
                        if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0 && DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas.Count > 1)
                        {
                            this.initPostgresProjectsMissingInCacheDB();
                        }
                    }
                }
                else
                {
                    this.tabControlMain.TabPages.Remove(this.tabPageProjects);
                    this.tabControlMain.TabPages.Remove(this.tabPageAnonymAgent);
                    this.buttonCheckProjectsDatabase.Visible = false;
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private void buttonAddProject_Click(object sender, EventArgs e)
        {
            try
            {
                string PublishedProjects = "";
                foreach(System.Windows.Forms.Control C in this.panelProjects.Controls)
                {
                    if (C.GetType() == typeof(DiversityCollection.CacheDatabase.UserControlProject))
                    {
                        DiversityCollection.CacheDatabase.UserControlProject CP = (DiversityCollection.CacheDatabase.UserControlProject)C;
                        if (PublishedProjects.Length > 0)
                            PublishedProjects += ", ";
                        PublishedProjects += CP.ProjectID().ToString();
                    }
                }
                System.Data.DataTable dtProjects = new DataTable();
                // Markus 31.3.2021: von ...Proxy auf ...List geaendert
                string SQL = "SELECT Project, ProjectID FROM ProjectList";
                if (PublishedProjects.Length > 0)
                    SQL += " WHERE ProjectID NOT IN (" + PublishedProjects + ") ";
                SQL += " ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtProjects);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtProjects, "Project", "ProjectID", "New published projects", "Please select the project");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    int ProjectID = int.Parse(f.SelectedValue.ToString());
                    DiversityCollection.CacheDatabase.UserControlProject U = new UserControlProject(ProjectID, f.SelectedString, this);
                    if (U.EstablishProject(ProjectID, f.SelectedString))
                    {
                        DiversityCollection.CacheDatabase.CacheDB.ResetDtProjects();
                        if (DiversityCollection.CacheDatabase.CacheDB.DtProjects.Rows.Count == 0)
                        {
                            foreach (System.Windows.Forms.Control C in this.panelProjects.Controls)
                                C.Dispose();
                            this.panelProjects.Controls.Clear();
                        }
                        U.initControl();
                        this.panelProjects.Controls.Add(U);
                        U.Dock = DockStyle.Top;
                        U.BringToFront();
                        SQL = "SELECT COUNT(*) FROM ProjectPublished";
                        string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                        if (Result == "1")
                            this.initAnonymCollector();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setProjectPostgresControls()
        {
            try
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceProject P in this._I_Projects)
                    P.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool initPostgresProjectsMissingInCacheDB()
        {
            bool OK = true;
            OK = this.RemovePostgresProjectsMissingInCacheDB();
            if (OK)
                OK = this.AddPostgresProjectsMissingInCacheDB();
            return OK;
        }

        private void resetProjectPostgresControls()
        {
            try
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceProject P in this._I_Projects)
                    P.resetPostgresControls();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonProjectTransferToCacheDB_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            this.StartAutoTransfer(true, false);//, false);
            System.Windows.Forms.MessageBox.Show("Transfer finished");
        }

        private void buttonProjectTransferToPostgresDB_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            this.StartAutoTransfer(false, true);//, false);
            System.Windows.Forms.MessageBox.Show("Transfer finished");
        }

        private void buttonViewPublic_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(true, "public");
            f.setHelp("Cache database transfer");
            f.ShowDialog();
        }

        private void buttonViewDbo_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(false, "dbo");
            f.setHelp("Cache database transfer");
            f.ShowDialog();
        }


        #region Filter projects

        //private bool _FilterProjectSourceForUpdate = false;
        //private bool _FilterProjectCacheForUpdate = false;

        //private void buttonProjectTransferToCacheDBFilter_Click(object sender, EventArgs e)
        //{
        //    if (this._FilterProjectSourceForUpdate)
        //    {
        //        this.toolTip.SetToolTip(this.buttonProjectTransferToCacheDBFilter, "Data not filtered for updates");
        //        this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.FilterClear;
        //    }
        //    else
        //    {
        //        this.toolTip.SetToolTip(this.buttonProjectTransferToCacheDBFilter, "Data filtered for updates later than last transfer");
        //        this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.Filter;
        //    }
        //    this._FilterProjectSourceForUpdate = !this._FilterProjectSourceForUpdate;
        //}

        //private void buttonProjectTransferToPostgresFilter_Click(object sender, EventArgs e)
        //{
        //    if (this._FilterProjectCacheForUpdate)
        //    {
        //        this.toolTip.SetToolTip(this.buttonProjectTransferToPostgresFilter, "Data not filtered for updates");
        //        this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.FilterClear;
        //    }
        //    else
        //    {
        //        this.toolTip.SetToolTip(this.buttonProjectTransferToPostgresFilter, "Data filtered for updates later than last transfer");
        //        this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.Filter;
        //    }
        //    this._FilterProjectCacheForUpdate = !this._FilterProjectCacheForUpdate;
        //}

        #endregion

        #endregion

        #region Postgres

        private void initPostgres(/*bool ProcessOnly*/)
        {
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                this.setPostgresControls();
            this.initPostgresControlsForSources();
            //this.setPostgresControlsForTaxonomy();
            this.setProjectPostgresControls();
        }

        private bool AddPostgresProjectsMissingInCacheDB()
        {
            bool OK = true;

            // Getting the schemas not missing in CacheDB
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (InterfaceProject IP in this._I_Projects)
            {
                if (!IP.MissingInCacheDB())
                    L.Add(IP.Schema());
            }

            // getting the schemas missing in the CacheDB
            string SQL = "select distinct schema_name from information_schema.schemata";
            System.Data.DataTable dtSchema = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtSchema, ref Message);
            System.Collections.Generic.Dictionary<int, string> Missing = new Dictionary<int, string>();
            foreach (System.Data.DataRow R in dtSchema.Rows)
            {
                if (!L.Contains(R[0].ToString()))
                {
                    SQL = "SELECT \"" + R[0].ToString() + "\".projectid();";
                    string ProjectID = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
                    if (ProjectID.Length == 0)
                        continue;
                    string Project = R[0].ToString().Substring("Project_".Length);
                    int iProjectID = int.Parse(ProjectID);
                    if (!Missing.ContainsKey(iProjectID))
                        Missing.Add(iProjectID, Project);
                }
            }

            // inserting the controls for the missing schemas
            foreach(System.Collections.Generic.KeyValuePair<int, string> KV in Missing)
            {
                DiversityCollection.CacheDatabase.UserControlProject U = new UserControlProject(KV.Key, KV.Value, this);
                U.setMissingInCacheDB();
                this.panelProjects.Controls.Add(U);
                U.Dock = DockStyle.Top;
                U.BringToFront();
                U.setHelp("Cache database projects");

                SQL = "SELECT COUNT(*) FROM CollectionProject C, ProjectProxy P WHERE C.ProjectID = P.ProjectID AND P.ProjectID = " + KV.Key.ToString() + " AND P.Project = '" + KV.Value + "' ";
                string ProjectInDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (ProjectInDB == "0")
                {
                    U.setMissingInDB(KV.Value);
                }
                //bool CacheDBNeedsUpdate = this.UpdateCheckForDatabase(true);
                U.setPostgesProject(KV.Key, KV.Value);
                U.initPostgresControls(true);//CacheDBNeedsUpdate);
            }
            return OK;
        }

        private bool RemovePostgresProjectsMissingInCacheDB()
        {
            bool OK = true;
            try
            {
                System.Collections.Generic.List<UserControlProject> ToRemove = new List<UserControlProject>();
                foreach (System.Windows.Forms.Control C in this.panelProjects.Controls)
                {
                    if (C.GetType() == typeof(UserControlProject))
                    {
                        UserControlProject U = (UserControlProject)C;
                        if (U.MissingInCacheDB() || U.MissingInDB())
                        {
                            ToRemove.Add(U);
                        }
                    }
                }
                foreach(UserControlProject U in ToRemove)
                {
                    this.panelProjects.Controls.Remove(U);
                    U.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void setPostgresControls()
        {
            try
            {
                string Database = "";
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                        Database = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                    else if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Contains(";Database=postgres;"))
                    {
                        Database = "postgres";
                    }
                }
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0
                    && Database.Length > 0)
                {
                    //this.buttonConnectToPostgresProject.Visible = false;
                    // Markus 3.5.23: toolStripButtonPostgresServer hinzugefügt - Darstellung der Aktiviät auf Postgres Server
                    if (Database.ToLower() == "postgres")
                    {
                        this.toolStripButtonPostgesDeleteDB.Enabled = false;
                        this.toolStripButtonPostgresAddDB.Enabled = true;
                        this.toolStripButtonPostgresRename.Enabled = false;
                        this.toolStripButtonPostgresLogins.Enabled = true;
                        this.toolStripButtonPostgesExchangeDB.Enabled = false;
                        this.toolStripButtonPostgresCopyDatabase.Enabled = false;
                        this.toolStripButtonPostgresServer.Enabled = false;
                        this.labelPostgresDB.Text = "No cache database selected or available";
                        this.labelProjectPostgresDB.Text = "No cache database selected or available";
                        this.labelPostgresDB.ForeColor = System.Drawing.Color.Red;
                        //this.buttonConnectToPostgres.BackColor = System.Drawing.Color.Transparent;
                        this.buttonConnectToPostgres.Image = DiversityCollection.Resource.Postgres;
                        this.buttonConnectToPostgresProject.Image = DiversityCollection.Resource.Postgres;
                        this.buttonConnectToPostgresProject.Text = "      No cache database selected or available";
                        this.buttonConnectToPostgresProject.ForeColor = System.Drawing.Color.Red;
                        this.buttonPostgresUpdate.Visible = false;
                        this.textBoxPostgresVersion.Text = "";
                        this.textBoxPostgresAvailableVersion.Visible = false;
                    }
                    else
                    {
                        this.labelPostgresDB.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText();
                        this.labelPostgresDB.ForeColor = System.Drawing.Color.Black;
                        this.labelProjectPostgresDB.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText();
                        //this.buttonConnectToPostgres.BackColor = System.Drawing.Color.Transparent;
                        this.buttonConnectToPostgres.Image = DiversityCollection.Resource.Postgres;
                        this.buttonConnectToPostgresProject.Image = DiversityCollection.Resource.Postgres;
                        this.buttonConnectToPostgresProject.Text = "      " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                        this.toolStripButtonPostgesDeleteDB.Enabled = true;
                        this.toolStripButtonPostgresAddDB.Enabled = true;
                        this.toolStripButtonPostgresRename.Enabled = true;
                        this.toolStripButtonPostgresLogins.Enabled = true;
                        this.toolStripButtonPostgesExchangeDB.Enabled = true;
                        this.toolStripButtonPostgresCopyDatabase.Enabled = true;
                        this.toolStripButtonPostgresServer.Enabled = true;
                        this.tableLayoutPanelPostgres.Enabled = true;
                        //this.buttonProjectTransferToPostgresDB.Enabled = true;
                        //this.buttonProjectTransferToPostgresDB.Enabled = false;
                        // Update
                        string Version = "";
                        string Message = "";
                        this._PostgresDatabaseNeedsUpdate = this.PostgresDatabaseNeedsUpdate(ref Version, ref Message);
                        if (this._PostgresDatabaseNeedsUpdate)
                        {
                            System.Windows.Forms.MessageBox.Show("The database " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + " needs an update to version " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion);
                            this.buttonPostgresUpdate.Visible = true;
                            this.textBoxPostgresAvailableVersion.Visible = true;
                            this.textBoxPostgresAvailableVersion.Text = "Update to version " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion;
                        }
                        else
                        {
                            this.buttonPostgresUpdate.Visible = false;
                            if (Message.Length == 0)
                                this.textBoxPostgresAvailableVersion.Visible = false;
                            else
                            {
                                this.textBoxPostgresAvailableVersion.Visible = true;
                                this.textBoxPostgresAvailableVersion.Text = Message;
                            }
                        }

                        this.toolStripButtonPostgresLogins.Enabled = !this._PostgresDatabaseNeedsUpdate;
                        //this.buttonProjectTransferToPostgresDB.Enabled = !this._PostgresDatabaseNeedsUpdate;
                        this.buttonProjectTransferToPostgresDB.Enabled = false; // !this._PostgresDatabaseNeedsUpdate;
                        this.buttonViewPublic.Enabled = !this._PostgresDatabaseNeedsUpdate;
                        this.checkBoxTimerIncludePostgres.Enabled = !this._PostgresDatabaseNeedsUpdate;

                        this.textBoxPostgresVersion.Text = Version;

                        // geht nicht
                        //this.textBoxPostgresLinkedServer.Text = DiversityCollection.CacheDatabase.CacheDB.getPostgresLinkedServer();
                        //if(this.textBoxPostgresLinkedServer.Text.Length == 0)
                        //    this.textBoxPostgresLinkedServer.Text = DiversityCollection.CacheDatabase.CacheDB.getLinkedServerForCurrentPostgresDatabase();
                    }
                }
                else
                {
                    this.labelPostgresDB.Text = "Not connected";
                    this.labelProjectPostgresDB.Text = "Not connected";
                    this.labelPostgresDB.ForeColor = System.Drawing.Color.Red;
                    this.buttonConnectToPostgres.Image = DiversityCollection.Resource.NoPostgres;
                    this.toolStripButtonPostgesDeleteDB.Enabled = false;
                    this.toolStripButtonPostgresAddDB.Enabled = false;
                    this.toolStripButtonPostgresRename.Enabled = false;
                    this.toolStripButtonPostgresLogins.Enabled = false;
                    this.toolStripButtonPostgesExchangeDB.Enabled = false;
                    this.toolStripButtonPostgresCopyDatabase.Enabled = false;
                    this.toolStripButtonPostgresServer.Enabled = false;
                    this.tableLayoutPanelPostgres.Enabled = false;
                    this.buttonProjectTransferToPostgresDB.Enabled = false;
                    this.buttonPostgresUpdate.Visible = false;
                    this.textBoxPostgresAvailableVersion.Visible = false;
                    //this.buttonConnectToPostgresProject.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void initPostgresDatabase()
        {
            try
            {
                string Database = "";
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                    {
                        Database = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                    }
                }
                if (Database.Length > 0 && Database != "postgres" && DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(Database))
                {
                    tableLayoutPanelPostgres.Enabled = true;
                    this.labelPostgresDB.ForeColor = System.Drawing.Color.Black;
                    string Version = "";
                    string Message = "";
                    this.buttonPostgresUpdate.Visible = this.PostgresDatabaseNeedsUpdate(ref Version, ref Message);
                    if (Message.Length > 0)
                    {
                        System.Windows.Forms.MessageBox.Show(Message + "\r\nPlease change to the current version");
                    }
                    this.initTarget();
                    DiversityCollection.CacheDatabase.Project.ResetProjects();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int? _TargetID = null;
        private void initTarget()
        {
            try
            {
                string SQL = "SELECT TargetID ";
                SQL += " FROM Target T WHERE T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
                    "AND DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                    "AND Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString();
                int iTarget;
                // Markus 28.3.25: Check existence. Issue #36
                if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, true), out iTarget))
                    this._TargetID = iTarget;
                else
                {
                    string SqlInsert = "INSERT INTO Target(Server, DatabaseName, Port) " +
                        "VALUES('" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "', " +
                        "'" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "', " +
                        DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() + ")";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SqlInsert))
                    {
                        if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL), out iTarget))
                            this._TargetID = iTarget;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #region BulkTransfer

        private void buttonPostgresTransferDirectory_Click(object sender, EventArgs e)
        {
            this.SetTransferSetting(TransferSetting.Directory);
            return;

            try
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Transfer directory", "Please enter path of the transfer directory on the postgres server", this.textBoxPostgresTransferDirectory.Text);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory = f.String;
                    this.initPostgresControlsForSourceTransfer();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonPostgresBashFile_Click(object sender, EventArgs e)
        {
            this.SetTransferSetting(TransferSetting.Bashfile);
            return;

            try
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Bash file", "Please enter path of the bash file for conversion of the exported files", this.textBoxPostgresBashFile.Text);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile = f.String;
                    this.initPostgresControlsForSourceTransfer();
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonPostgresMountPoint_Click(object sender, EventArgs e)
        {
            this.SetTransferSetting(TransferSetting.Mountpoint);
            return;

            try
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Mount point", "Please enter the mount point name of the transfer folder. Please ask the postgres server administrator for details.", this.textBoxPostgresMountPoint.Text);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint = f.String;
                    this.initPostgresControlsForSourceTransfer();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private enum TransferSetting { Directory, Bashfile, Mountpoint }
        private void SetTransferSetting(TransferSetting Setting)
        {
            //string TransferSetting = "";
            try
            {
                string SQL = "";
                string Column = "";
                string Header = "";
                string Message = "";
                string Start = "";
                System.Drawing.Image image = DiversityCollection.Resource.Open;
                switch (Setting)
                {
                    case FormCacheDatabase.TransferSetting.Bashfile:
                        Column = "BashFile";
                        Header = "Bash file";
                        Message = "Please enter path of the bash file for conversion of the exported files";
                        Start = this.textBoxPostgresBashFile.Text;
                        image = DiversityCollection.Resource.TransferToFile;
                        break;
                    case FormCacheDatabase.TransferSetting.Directory:
                        Column = "TransferDirectory";
                        Header = "Transfer directory";
                        Message = "Please enter path of the transfer directory on the postgres server";
                        Start = this.textBoxPostgresTransferDirectory.Text;
                        break;
                    case FormCacheDatabase.TransferSetting.Mountpoint:
                        Column = "MountPoint";
                        Header = "Mount point";
                        Message = "Please enter the mount point name of the transfer folder";
                        Start = this.textBoxPostgresMountPoint.Text;
                        image = DiversityCollection.Resource.MountPoint;
                        break;
                }
                Message += ". Please ask the postgres server administrator for details.";
                SQL = "SELECT '' AS Setting UNION SELECT '" + Start + "' AS Setting UNION SELECT DISTINCT " + Column + " AS Setting FROM Target WHERE " + Column + " <> '' ORDER BY Setting";
                System.Data.DataTable dt = new DataTable();
                string M = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref M);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Setting", "Setting", Header, Message, "", false, false, true, image);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    //TransferSetting = f.SelectedString;
                    switch (Setting)
                    {
                        case FormCacheDatabase.TransferSetting.Bashfile:
                            DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile = f.String;
                            break;
                        case FormCacheDatabase.TransferSetting.Directory:
                            DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory = f.String;
                            break;
                        case FormCacheDatabase.TransferSetting.Mountpoint:
                            DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint = f.String;
                            break;
                    }
                    this.initPostgresControlsForSourceTransfer();
                    if (DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile.Length > 0 &&
                        DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory.Length > 0 &&
                        DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint.Length > 0)
                    {
                        this.setProjectPostgresControls();
                    }
                }

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //return TransferSetting;
        }

        #endregion

        #region Update

        private void buttonPostgresUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.UpdatePostgresDatabase();
                this.setPostgresControls();
                this.setProjectPostgresControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool PostgresDatabaseNeedsUpdate(ref string Version, ref string Message)
        {
            string SQL = "select public.version()";
            Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// DiversityWorkbench.Postgres.PostgresExecuteSqlSkalar(SQL);
            bool NeedsUpdate = false;
            if (Version != DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion)
            {
                int DBversionMajor;
                int DBversionMinor;
                int DBversionRevision;
                int C_versionMajor;
                int C_versionMinor;
                int C_versionRevision;
                string[] VersionDBParts;
                if (Version.IndexOf('.') > -1) VersionDBParts = Version.Split(new Char[] { '.' });
                else if (Version.IndexOf('/') > -1) VersionDBParts = Version.Split(new Char[] { '/' });
                else VersionDBParts = Version.Split(new Char[] { ' ' });
                if (!int.TryParse(VersionDBParts[0], out DBversionMajor)) return false;
                if (!int.TryParse(VersionDBParts[1], out DBversionMinor)) return false;
                if (!int.TryParse(VersionDBParts[2], out DBversionRevision)) return false;

                string[] VersionC_Parts = DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion.Split(new Char[] { '.' });
                if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                if (C_versionMajor > DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                    NeedsUpdate = true;
                else if (C_versionMajor < DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor < DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision < DBversionRevision))
                    Message = "Outdated software, expecting version " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion + ".";
            }

            return NeedsUpdate;
        }

        private void UpdatePostgresDatabase()
        {
            DiversityWorkbench.PostgreSQL.Database DB = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase();
            string DatabaseCurrentVersion = DB.Version();
            string DatabaseFinalVersion = DiversityCollection.Properties.Settings.Default.SqlServerCacheDBVersion;
            DatabaseFinalVersion = DatabaseFinalVersion.Replace(".", "").Replace("/", "");

            try
            {
                // check resouces for update scripts
                System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
                System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
                System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdatePG_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    Npgsql.NpgsqlConnection con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    DiversityWorkbench.Forms.FormUpdateDatabase f =
                        new DiversityWorkbench.Forms.FormUpdateDatabase(
                            DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,
                            DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion,
                            con,
                            Versions,
                            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                    f.ForPostgres = true;
                    f.ShowInTaskbar = true;
                    f.Width = this.Width + 10;
                    f.PostgresRole = "CacheAdmin";
                    f.ShowDialog();
                    if (f.Reconnect)
                    {
                        bool HasAccess = true;
                        this.setDatabase(ref HasAccess);
                    }
                    string Version = "";
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Connection

        private void buttonConnectToPostgres_Click(object sender, EventArgs e)
        {
            if (this.ConnectToPostgresDatabaseAndInitContols())
                this.initPostgresProjectsMissingInCacheDB();
            else
            {
                if (DiversityWorkbench.PostgreSQL.Connection._CurrentServer != null &&
                    DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null &&
                    DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name.Length > 0)
                {
                    this.toolStripButtonPostgresAddDB.Enabled = true;
                }
            }
        }

        private void buttonConnectToPostgresProject_Click(object sender, EventArgs e)
        {
            if (this.ConnectToPostgresDatabaseAndInitContols())
                this.initPostgresProjectsMissingInCacheDB();
        }

        private bool ConnectToPostgresDatabaseAndInitContols()
        {
            bool OK = this.ConnectToPostgresDatabase();
            if (OK)
                OK = DiversityWorkbench.PostgreSQL.Connection.SetRoleWithMaxPermission();
            //this.setPostgresControls();
            if (OK)
            {
                this.setPostgresControls();
                //this.setProjectPostgresControls();
                this.initPostgresControlsForSources();
                this.resetProjectPostgresControls();
                this.setProjectPostgresControls();
            }
            return OK;
        }

        private void initPostgresControlsForSources()
        {
            this.initPostgresControlsForSourceTransfer();
            this.initPostgresControlsForTaxonomy();
            this.initPostgresControlsForGazetteerSources();
            this.initPostgresControlsForTerms();
            this.initPostgresControlsForAgents();
            this.initPostgresControlsForReferences();
            this.initPostgresControlsForPlots();
        }

        private void resetPostgresControlsForSources()
        {
            foreach (System.Windows.Forms.Control C in this.panelAgents.Controls)
                C.Dispose();
            this.panelAgents.Controls.Clear();

            foreach (System.Windows.Forms.Control C in this.panelDescriptions.Controls)
                C.Dispose();
            this.panelDescriptions.Controls.Clear();

            foreach (System.Windows.Forms.Control C in this.panelGazetteerSources.Controls)
                C.Dispose();
            this.panelGazetteerSources.Controls.Clear();

            foreach (System.Windows.Forms.Control C in this.panelPlots.Controls)
                C.Dispose();
            this.panelPlots.Controls.Clear();

            foreach (System.Windows.Forms.Control C in this.panelReferences.Controls)
                C.Dispose();
            this.panelReferences.Controls.Clear();

            foreach (System.Windows.Forms.Control C in this.panelTaxonSources.Controls)
                C.Dispose();
            this.panelTaxonSources.Controls.Clear();

            foreach (System.Windows.Forms.Control C in this.panelTaxonWebservice.Controls)
                C.Dispose();
            this.panelTaxonWebservice.Controls.Clear();

            foreach (System.Windows.Forms.Control C in this.panelTerms.Controls)
                C.Dispose();
            this.panelTerms.Controls.Clear();
        }

        private bool ConnectToPostgresDatabase()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            bool OK = true;
            DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(DiversityWorkbench.Forms.FormConnectToDatabase.DatabaseManagementSystem.Postgres, DiversityWorkbench.PostgreSQL.Connection.PreviousConnections());
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityCollection.CacheDatabase.CacheDB.ResetBulkTransfer();
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentServer(f.Server);
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentPort(f.Port);
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(f.Database);
                if (!f.IsTrusted)
                {
                    DiversityWorkbench.PostgreSQL.Connection.SetCurrentRole(f.User);
                    DiversityWorkbench.PostgreSQL.Connection.Password = f.Password;
                }
                this.labelPostgresDB.Text = f.Database;
                this.buttonConnectToPostgresProject.Text = "      " + f.Database;
                this.toolStripButtonPostgresAddDB.Enabled = true;
                this.buttonConnectToPostgres.Image = DiversityCollection.Resource.Postgres;
                this.buttonConnectToPostgresProject.Image = DiversityCollection.Resource.Postgres;
            }
            else
            {
                OK = false;
            }

            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                if (f.IsTrusted)
                    DiversityWorkbench.PostgreSQL.Connection.AddPreviousConnection(f.Server, f.Port);
                else
                    DiversityWorkbench.PostgreSQL.Connection.AddPreviousConnection(f.Server, f.Port, f.User);
            }

            this.initPostgresDatabase();
            this.Cursor = System.Windows.Forms.Cursors.Default;
            return OK;
        }

        #endregion

        #region Toolstrip

        private void toolStripButtonPostgresAddDB_Click(object sender, EventArgs e)
        {
            try
            {
                bool OK = false;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)// .Postgres.PostgresConnection() == null)
                    this.buttonConnectToPostgres_Click(null, null);
                string CacheDatabase = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New Database", "Please enter the name of the new database", CacheDatabase);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    char x = '"';
                    string Database = f.String.Replace("\"", "").Replace("'", "");
                    string SQL = "CREATE DATABASE " + x + f.String + x +
                        "WITH ENCODING='UTF8' CONNECTION LIMIT=-1;"; // OWNER=postgres MW 2017/03/01: removed to enable other users than postgres to create a database
                    string Message = "";
                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false))// .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show(Message);
                    else
                    {
                        OK = DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(Database);
                        // Anlegen der Rollen falls fehlend
                        if (OK)
                            OK = this.CreatePostgresRole("CacheAdmin", true, ref Message);
                        if (OK)
                            OK = this.CreatePostgresRole("CacheUser", false, ref Message);
                        if (OK)
                        {
                            SQL = "CREATE OR REPLACE FUNCTION diversityworkbenchmodule() " +
                                "RETURNS text AS " +
                                "$BODY$ " +
                                "declare " +
                                "v text; " +
                                "BEGIN " +
                                "SELECT 'DiversityCollectionCache' into v; " +
                                "RETURN v; " +
                                "END; " +
                                "$BODY$ " +
                                "LANGUAGE plpgsql STABLE " +
                                "COST 100; " +
                                "ALTER FUNCTION diversityworkbenchmodule() OWNER TO \"CacheAdmin\";" +
                                "GRANT EXECUTE ON FUNCTION diversityworkbenchmodule() TO GROUP \"CacheUser\"; ";
                            if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))// .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                            {
                                System.Windows.Forms.MessageBox.Show(Message);
                                OK = false;
                            }
                            else
                            {
                                SQL = "CREATE OR REPLACE FUNCTION public.version() " +
                                    "RETURNS text AS " +
                                    "$BODY$ " +
                                    "declare " +
                                    "v text; " +
                                    "BEGIN " +
                                    "SELECT '00.00.00' into v; " +
                                    "RETURN v; " +
                                    "END; " +
                                    "$BODY$ " +
                                    "LANGUAGE plpgsql STABLE " +
                                    "COST 100; " +
                                    "ALTER FUNCTION public.version() OWNER TO \"CacheAdmin\"; " +
                                    "GRANT EXECUTE ON FUNCTION version() TO GROUP \"CacheUser\"; ";
                                if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, true, false, true))
                                {
                                    System.Windows.Forms.MessageBox.Show(Message);
                                    OK = false;
                                }
                                else
                                {
                                    DiversityWorkbench.Forms.FormGetString fDes = new DiversityWorkbench.Forms.FormGetString("Description for database", "Please enter a short description for the new database", "");
                                    fDes.TopMost = true;
                                    fDes.ShowDialog();
                                    if (fDes.String.Length > 0 && fDes.DialogResult == System.Windows.Forms.DialogResult.OK)
                                    {
                                        string Description = fDes.String.Replace("\"", "").Replace("'", "");
                                        SQL = "SET ROLE \"CacheAdmin\"; COMMENT ON DATABASE \"" + Database + "\" IS '" + Description + "';";
                                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) // .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                                        {
                                            System.Windows.Forms.MessageBox.Show(Message);
                                            OK = false;
                                        }
                                        else
                                            OK = true;
                                    }
                                }
                            }
                        }
                        if(!OK && Message.Length > 0)
                            System.Windows.Forms.MessageBox.Show(Message);
                        DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(Database);
                        if (OK)
                        {
                            this.initPostgresDatabase();
                            this.setPostgresControls();
                            this.resetPostgresControlsForSources();
                            //this.setPostgresControlsForTaxonomy();
                            this.setProjectPostgresControls();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        /// <summary>
        /// Anlegen von Rollen auf Postgres server (von Toni übernommen)
        /// </summary>
        /// <param name="Role">Name of the role</param>
        /// <param name="IsAdmin">If the role is admin for the database</param>
        /// <param name="Message">Any error messages</param>
        /// <returns></returns>
        private bool CreatePostgresRole(string Role, bool IsAdmin, ref string Message)
        {
            bool OK = false;
            string SQL = "CREATE or replace FUNCTION MakeRole() RETURNS void AS " +
                "$BODY$  " +
                "declare i INTEGER:= 0; ";
            if (IsAdmin)
                SQL += "db character varying; ";
            SQL += "begin " +
                "SELECT count(*) FROM pg_roles R where R.rolname = '" + Role + "' into i; " +
                "if i = 0 " +
                "then " +
                "CREATE ROLE \"" + Role + "\" VALID UNTIL 'infinity'; " +
                "end if; " +
                "GRANT \"" + Role + "\" TO CURRENT_USER WITH ADMIN OPTION;  ";
            if (IsAdmin)
            {
                SQL += "db = (SELECT current_database()); " +
                "EXECUTE 'GRANT ALL PRIVILEGES ON DATABASE \"' || db || '\" TO \"CacheAdmin\";'; " +
                "EXECUTE 'ALTER DATABASE \"' || db || '\" OWNER TO \"CacheAdmin\";'; ";
            }
            SQL += "end; " +
                "$BODY$ LANGUAGE plpgsql; " +
                "SELECT MakeRole(); " +
                "DROP FUNCTION MakeRole(); ";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) 
                OK = true;
            return OK;
        }

        private void toolStripButtonPostgresCopyDatabase_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.FormCopyDatabase f = new DiversityWorkbench.PostgreSQL.FormCopyDatabase();
            f.ShowDialog();
        }

        private void toolStripButtonPostgresRename_Click(object sender, EventArgs e)
        {
            string CurrentDB = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New name of database", "Please enter the new name for the current database", CurrentDB);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.String.Length > 0)
            {
                string NewNameForDB = f.String;
                DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                if (DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase("postgres"))
                {
                    string SQL = "SELECT pg_terminate_backend( pid ) " +
                        "FROM pg_stat_activity " +
                        "WHERE pid <> pg_backend_pid( ) " +
                        "AND datname = '" + CurrentDB + "'; " +
                        "ALTER DATABASE \"" + CurrentDB + "\" RENAME TO \"" + NewNameForDB + "\"; ";
                    string Message = "";
                    if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                    {
                        if (Message.Length == 0)
                        {
                            if (DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(NewNameForDB))
                            {
                                DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                                System.Windows.Forms.MessageBox.Show("Renaming has been successful");
                                this.initPostgres();
                            }
                        }
                        else
                            System.Windows.Forms.MessageBox.Show("Renaming failed:\r\n" + Message);
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Renaming failed:\r\n" + Message);
                }
            }
        }

        private void toolStripButtonPostgesExchangeDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() == null)
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                    {
                        Npgsql.NpgsqlConnection con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                        if (con.Database != "postgres")
                            DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(con.Database);
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Please reconnect to postgres database");
                            return;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Please reconnect to postgres database");
                        return;
                    }
                }
                DiversityWorkbench.CacheDB.ReplaceDB.Replacement replacement = new DiversityWorkbench.CacheDB.ReplaceDB.Replacement();
                System.Collections.Generic.Dictionary<DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type, DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart> Sources = new Dictionary<DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type, DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart>();

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Agent = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Agent);
                Sources.Add(Agent.Type, Agent);

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Gazetteer = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Gazetteer);
                Sources.Add(Gazetteer.Type, Gazetteer);

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Plot = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.SamplingPlot);
                Sources.Add(Plot.Type, Plot);

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Reference = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Reference, "", "ReferenceTitle");
                Sources.Add(Reference.Type, Reference);

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Taxon = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Taxon, "", "TaxonSynonymy");
                Sources.Add(Taxon.Type, Taxon);

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Taxon_Webservice = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Taxon_Webservice, "", "TaxonSynonymy");
                Sources.Add(Taxon_Webservice.Type, Taxon_Webservice);

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Term = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.ScientificTerm);
                Sources.Add(Term.Type, Term);

                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart Project = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Project, "", "CacheCount", false);
                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart ABCD = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Project_Package, "ABCD_%", "ABCD_Unit", true);
                Project.ContainedParts.Add("ABCD", ABCD);
                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart ABCD_BayernFlora = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Project_Package, "ABCD__BayernFlora%", "ABCD__BayernFlora_EndangeredSpeciesBase", false);
                Project.ContainedParts.Add("ABCD_BayernFlora", ABCD_BayernFlora);
                DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart FloraRaster = new DiversityWorkbench.CacheDB.ReplaceDB.ReplacedPart(ref replacement, DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Project_Package, "FloraRaster_%", "FloraRaster_Sippendaten", false);
                Project.ContainedParts.Add("FloraRaster", FloraRaster);
                Sources.Add(DiversityWorkbench.CacheDB.ReplaceDB.Replacement.Type.Project, Project);
                replacement.setSources(Sources);

                DiversityWorkbench.CacheDB.ReplaceDB.FormReplaceDB f = new DiversityWorkbench.CacheDB.ReplaceDB.FormReplaceDB(ref replacement, "CacheAdmin");
                f.setStepsMax(this.ReplacementStepCount());
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DatabaseHasBeenReplaced())
                    this.initPostgres();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int ReplacementStepCount()
        {
            int Count = 0;
            Count += this.panelAgents.Controls.Count;
            Count += this.panelDescriptions.Controls.Count;
            Count += this.panelGazetteerSources.Controls.Count;
            Count += this.panelPlots.Controls.Count;
            Count += this.panelProjects.Controls.Count;
            Count += this.panelReferences.Controls.Count;
            Count += this.panelTaxonSources.Controls.Count;
            Count += this.panelTaxonWebservice.Controls.Count;
            Count += this.panelTerms.Controls.Count;
            return Count;
        }

        private void toolStripButtonPostgesDeleteDB_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null &&
                    DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name != null)
                {
                    string Database = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name; // this.listBoxPostgresDBs.SelectedItem.ToString();
                    string SQL = "SELECT pg_terminate_backend(pg_stat_activity.pid) " +
                        "FROM pg_stat_activity " +
                        "WHERE pg_stat_activity.datname = '" + Database + "' " +
                        "AND pid <> pg_backend_pid();";// DROP DATABASE \"" + Database + "\";";
                    if (System.Windows.Forms.MessageBox.Show("Do you really want to delete the database " + Database + "?", "Delete database", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase("postgres");
                        string Message = "";
                        if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                        {
                            SQL = "DROP DATABASE \"" + Database + "\";";
                            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false))
                            {
                                System.Windows.Forms.MessageBox.Show("Database " + Database + " deleted");
                                DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                                DiversityWorkbench.PostgreSQL.Connection.ResetDefaultConnectionString();

                                this.initPostgresDatabase();
                            }
                            else
                                System.Windows.Forms.MessageBox.Show("Deleting database " + Database + " failed:\r\n" + Message);
                        }
                        this.setPostgresControls();
                        this.resetPostgresControlsForSources();
                        this.setProjectPostgresControls();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No database selected");
                    this.setPostgresControls();
                    this.initPostgres();
                }
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripButtonPostgresLogins_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.PostgreSQL.FormRoleAdministration f = new DiversityWorkbench.PostgreSQL.FormRoleAdministration();
                f.setHelp("Cache database postgres");
                f.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Processes

        private void toolStripButtonPostgresServer_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.ShowServerActiviy(this.Width, this.Height, true);
        }

        #endregion

        #endregion

        #region Auxillary

        // converts a name into a name compatible with sql (no space etc.)
        private string ConvertToSqlName(string Name)
        {
            string Result = "";
            bool ToUpper = true;
            foreach (char c in Name)
            {
                int i = (int)c;
                if (i == 32) ToUpper = true;
                if ((i > 10 && i < 48) || (i > 58 && i < 65) || (i > 90 && i < 95) || (i > 95 && i < 97) || i > 122)
                    continue;
                if (ToUpper) Result += c.ToString().ToUpper();
                else Result += c.ToString();
                ToUpper = false;
                if (Result.Length > 100) break;
            }
            return Result;
        }

        private void SearchForSources(string SourceModule, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> PotentialSourceLinks)
        {
            try
            {
                string Message = "";
                string SQL = "select [dbo].[ServerURL]()";
                string Server = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
                SQL = "SELECT SourceView, [LinkedServerName], [DatabaseName] FROM [dbo].";
                switch (SourceModule)
                {
                    case "TaxonName":
                        SQL += "[TaxonSynonymySource]";
                        break;
                    case "ScientificTerms":
                        SQL += "[ScientificTermSource]";
                        break;
                    case "Agents":
                        SQL += "[AgentSource]";
                        break;
                    case "References":
                        SQL += "[ReferenceTitleSource]";
                        break;
                    case "Gazetteer":
                        SQL += "[GazetteerSource]";
                        break;
                }
                System.Data.DataTable dtAvailable = new DataTable();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtAvailable, ref Message);

                System.Collections.Generic.SortedDictionary<string, int> MissingSources = new SortedDictionary<string, int>();
                // getting all projects
                SQL = "select distinct SCHEMA_NAME from INFORMATION_SCHEMA.SCHEMATA " +
                "S where S.SCHEMA_NAME like 'Project_%'";
                System.Data.DataTable dtProjects = new DataTable();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtProjects, ref Message);
                foreach (System.Data.DataRow R in dtProjects.Rows)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> KVsource in PotentialSourceLinks)
                    {
                        foreach (string LinkColumn in KVsource.Value)
                        {
                            SQL = "SELECT rtrim(reverse(substring(reverse([" + LinkColumn + "]), charindex('/', reverse([" + LinkColumn + "])), 500))) AS Source, count(*) AS Quantity " +
                                "FROM [" + R[0].ToString() + "].[" + KVsource.Key + "] " +
                                "where " + LinkColumn + " like 'http%/" + SourceModule + "%/%' " +
                                "group by rtrim(reverse(substring(reverse([" + LinkColumn + "]), charindex('/', reverse([" + LinkColumn + "])), 500)))";
                            System.Data.DataTable dt = new DataTable();
                            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                            foreach (System.Data.DataRow rS in dt.Rows)
                            {
                                if (MissingSources.ContainsKey(rS["Source"].ToString()))
                                {
                                    MissingSources[rS["Source"].ToString()] += int.Parse(rS["Quantity"].ToString());
                                }
                                else
                                {
                                    MissingSources.Add(rS["Source"].ToString(), int.Parse(rS["Quantity"].ToString()));
                                }
                            }
                        }
                    }
                }
                string Result = "Sources and quantity detected within the projects:\r\n\r\n";
                if (MissingSources.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in MissingSources)
                    {
                        Result += KV.Key + ":\t" + KV.Value.ToString() + "\r\n";
                        foreach (System.Data.DataRow R in dtAvailable.Rows)
                        {
                            if (KV.Key.ToLower().Contains(R["LinkedServerName"].ToString().ToLower()) &&
                                KV.Key.ToLower().EndsWith((R["DatabaseName"].ToString().ToLower() + "/")))
                            {
                                Result += "\t" + R["SourceView"].ToString();
                                break;
                            }
                        }

                    }
                }
                else
                    Result = "No missing sources have been detected";
                System.Windows.Forms.MessageBox.Show(Result);
            }
            catch (System.Exception ex)
            { }
        }

        #endregion

        #region Sources

        #region Common: Subsets, recreation
        private string SourceSubsets(DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable Table)
        {
            string Subsets = "";
            System.Collections.Generic.List<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable> List
                = DiversityCollection.CacheDatabase.UserControlLookupSource.SourceSubsets(Table);
            foreach (DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable T in List)
            {
                if (Subsets.Length > 0) Subsets += "|";
                Subsets += T.ToString();
            }
            return Subsets;
        }

        //private void RecreateSource(string Source)
        //{
        //    if (Source == null)
        //    {
        //        //System.Collections.Generic.List<string> SourceList = new List<string>();
        //        //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnections())
        //        //{
        //        //    SourceList.Add(KV.Value.DisplayText);
        //        //}
        //        //DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
        //        //f.ShowDialog();
        //        //if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //        //{
        //        //    Source = f.String;
        //        //    DiversityWorkbench.ServerConnection S = null;
        //        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnections())
        //        //    {
        //        //        if (KV.Value.DisplayText == Source)
        //        //        {
        //        //            S = KV.Value;
        //        //            break;
        //        //        }
        //        //    }
        //        //}
        //    }
        //    System.Collections.Generic.List<string> SourceList = new List<string>();
        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnections())
        //    {
        //        SourceList.Add(KV.Value.DisplayText);
        //    }
        //    if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
        //    {
        //    }
        //    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
        //    f.ShowDialog();
        //    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //    {
        //        Source = f.String;
        //        DiversityWorkbench.ServerConnection S = null;
        //        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnections())
        //        {
        //            if (KV.Value.DisplayText == Source)
        //            {
        //                S = KV.Value;
        //                break;
        //            }
        //        }
        //        if (S != null)
        //        {
        //            string PrefixDB = "";
        //            if (S.LinkedServer.Length > 0)
        //                PrefixDB = "[" + S.LinkedServer + "].";
        //            PrefixDB += S.DatabaseName + ".dbo.";
        //            System.Data.DataTable dtProject = new DataTable();
        //            // Markus 11.3.2021: von ...List auf ...Proxy geaendert
        //            string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectProxy ORDER BY Project";
        //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
        //            ad.Fill(dtProject);
        //            DiversityWorkbench.Forms.FormGetStringFromList fProject = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Please select the project");
        //            fProject.ShowDialog();
        //            if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK)
        //            {
        //                int ProjectID;
        //                if (int.TryParse(fProject.SelectedValue, out ProjectID))
        //                {
        //                    string Project = fProject.SelectedString;
        //                    string View = this.CreateReferencesSource(S, fProject.SelectedString, ProjectID);
        //                    if (View.Length > 0)
        //                    {
        //                        SQL = "INSERT INTO ReferenceTitleSource (SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets) " +
        //                            "VALUES ('" + View + "', '" + Project + "', " + ProjectID.ToString() + ", '" + S.LinkedServer + "', '" + S.DatabaseName + "' " +
        //                            ", '" + this.SourceSubsets(UserControlLookupSource.SubsetTable.ReferenceTitle) + "')";
        //                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
        //                            this.initReferenceTitleSources();
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}

        #endregion

        #region Agents

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> _I_Agents;

        public bool initAgentSources(/*bool ProcessOnly*/)
        {
            this._I_Agents = new List<InterfaceLookupSource>();
            bool OK = this.initSources(ref this._I_Agents, this.panelAgents, this.toolStripAgents, this.dataSetCacheDatabase.AgentSource, this._SqlDataAdapterAgentSource, UserControlLookupSource.TypeOfSource.Agents);
            return OK;


            #region old
            //string SQL = "SELECT COUNT(*) " +
            //        "FROM AgentSource ";
            //if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            //{
            //    this.toolStripAgents.Enabled = true;
            //    try
            //    {
            //        this.dataSetCacheDatabase.AgentSource.Clear();
            //        SQL = "SELECT SourceView, Source, SourceID, LinkedServerName, DatabaseName " +
            //            "FROM AgentSource ";
            //        if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
            //            SQL += " WHERE IncludeInTransfer = 1 ";
            //        SQL += "ORDER BY SourceView ";
            //        if (this._SqlDataAdapterAgentSource == null)
            //            this._SqlDataAdapterAgentSource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            //        else
            //            this._SqlDataAdapterAgentSource.SelectCommand.CommandText = SQL;
            //        this._SqlDataAdapterAgentSource.Fill(this.dataSetCacheDatabase.AgentSource);
            //        System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
            //        foreach (System.Windows.Forms.Control C in this.panelAgents.Controls)
            //        {
            //            CCtoDelete.Add(C);
            //        }
            //        this.panelAgents.Controls.Clear();
            //        foreach (System.Windows.Forms.Control C in CCtoDelete)
            //        {
            //            C.Dispose();
            //        }
            //        foreach (System.Data.DataRow R in this.dataSetCacheDatabase.AgentSource.Rows)
            //        {
            //            DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, UserControlLookupSource.TypeOfSource.Agents);
            //            U.Dock = DockStyle.Top;
            //            this.panelAgents.Controls.Add(U);
            //            U.BringToFront();
            //            U.setHelp("Cache agents");
            //            this._I_Agents.Add(U);
            //        }
            //        //this.setp();
            //    }
            //    catch (System.Exception ex)
            //    {
            //        OK = false;
            //    }
            //}
            //else
            //{
            //    OK = false;
            //    this.toolStripAgents.Enabled = false;
            //}

            #endregion
        }

        private void toolStripButtonAgentsAddSource_Click(object sender, EventArgs e)
        {
            if (this.AddSource(DiversityWorkbench.WorkbenchUnit.ModuleType.Agents, UserControlLookupSource.SubsetTable.Agent, AgentDataTables(), UserControlLookupSource.TypeOfSource.Agents))
                this.initAgentSources();
            return;

            #region old
            //System.Collections.Generic.List<string> SourceList = new List<string>();
            //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnections())
            //{
            //    SourceList.Add(KV.Value.DisplayText);
            //}
            //if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
            //{
            //}
            //DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            //f.ShowDialog();
            //if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            //{
            //    string Source = f.String;
            //    DiversityWorkbench.ServerConnection S = null;
            //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnections())
            //    {
            //        if (KV.Value.DisplayText == Source)
            //        {
            //            S = KV.Value;
            //            break;
            //        }
            //    }
            //    if (S != null)
            //    {
            //        string PrefixDB = "";
            //        if (S.LinkedServer.Length > 0)
            //            PrefixDB = "[" + S.LinkedServer + "].";
            //        PrefixDB += S.DatabaseName + ".dbo.";
            //        System.Data.DataTable dtProject = new DataTable();
            //        string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectList ORDER BY Project";
            //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
            //        ad.Fill(dtProject);
            //        DiversityWorkbench.Forms.FormGetStringFromList fProject = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Please select the project");
            //        fProject.ShowDialog();
            //        if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK)
            //        {
            //            int ProjectID;
            //            if (int.TryParse(fProject.SelectedValue, out ProjectID))
            //            {
            //                string Project = fProject.SelectedString;
            //                string View = this.CreateAgentSource(S, fProject.SelectedString, ProjectID);
            //                if (View.Length > 0)
            //                {
            //                    SQL = "INSERT INTO AgentSource (SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets, Version) " +
            //                        "VALUES ('" + View + "', '" + Project + "', " + ProjectID.ToString() + ", '" + S.LinkedServer + "', '" + S.DatabaseName + "' " +
            //                        ", '" + this.SourceSubsets(UserControlLookupSource.SubsetTable.Agent) + "' " +
            //                        ", " + DiversityCollection.CacheDatabase.UserControlLookupSource.LookupSourceVersion[UserControlLookupSource.TypeOfSource.Agents].ToString() + ")";
            //                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            //                        this.initAgentSources();
            //                }
            //            }
            //        }
            //    }
            //}

            #endregion
        }

        private System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> AgentDataTables()
        {
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables = new Dictionary<UserControlLookupSource.SubsetTable, string>();
            DataTables.Add(UserControlLookupSource.SubsetTable.Agent, "");
            DataTables.Add(UserControlLookupSource.SubsetTable.AgentContactInformation, "_C");
            DataTables.Add(UserControlLookupSource.SubsetTable.AgentImage, "_I");
#if AgentIdentifierIncluded
            DataTables.Add(UserControlLookupSource.SubsetTable.AgentIdentifier, "_ID");
#endif
            return DataTables;
        }

        private void toolStripButtonAgentsTransferSource_Click(object sender, EventArgs e)
        {
            this.TransferAgentSources(true, false);
        }

        private void toolStripButtonAgentsTransferToPostgres_Click(object sender, EventArgs e)
        {
            this.TransferAgentSources(false, true);
        }

        private string TransferAgentSources(bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres)
        {
            string Report = "";
            if (this._I_Agents != null && this._I_Agents.Count > 0)
                Report = this.TransferSources(/*ProcessOnly,*/ IncludeTransferToCacheDB, IncludeTransferToPostgres, this._I_Agents, "Agents");//, this._FilterAgentsForUpdate);// "";
            return Report;
        }

        private void initPostgresControlsForAgents()
        {
            if (this._I_Agents != null)
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource A in this._I_Agents)
                    A.initPostgresControls();
            }
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                this.initMissingSourceViews("AgentSource", this.panelAgents, false);
            }
        }

        private void toolStripButtonAgentsFilter_Click(object sender, EventArgs e)
        {
            if (this._FilterAgentsForUpdate)
            {
                this.toolStripButtonAgentsFilter.Text = "Data not filtered for updates";
                this.toolStripButtonAgentsFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                this.toolStripButtonAgentsFilter.Text = "Data filtered for updates later than last transfer";
                this.toolStripButtonAgentsFilter.Image = DiversityCollection.Resource.Filter;
            }
            this._FilterAgentsForUpdate = !this._FilterAgentsForUpdate;
        }

        private void toolStripButtonAgentSearchSources_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> PotentialSourceLinks = new Dictionary<string, List<string>>();

            // Collector
            System.Collections.Generic.List<string> CollectorLinkColumns = new List<string>();
            CollectorLinkColumns.Add("CollectorsAgentURI");
            PotentialSourceLinks.Add("CacheCollectionAgent", CollectorLinkColumns);

            // Identifier
            System.Collections.Generic.List<string> IdentificationLinkColumns = new List<string>();
            IdentificationLinkColumns.Add("ResponsibleAgentURI");
            PotentialSourceLinks.Add("CacheIdentification", IdentificationLinkColumns);

            // CacheCollection - AdministrativeContactAgentURI
            System.Collections.Generic.List<string> CollectionLinkColumns = new List<string>();
            CollectionLinkColumns.Add("AdministrativeContactAgentURI");
            PotentialSourceLinks.Add("CacheCollection", CollectionLinkColumns);

            //CacheCollectionEventLocalisation - ResponsibleAgentURI
            System.Collections.Generic.List<string> EventLocalisationLinkColumns = new List<string>();
            EventLocalisationLinkColumns.Add("ResponsibleAgentURI");
            PotentialSourceLinks.Add("CacheCollectionEventLocalisation", EventLocalisationLinkColumns);

            //CacheCollectionEventProperty - ResponsibleAgentURI
            System.Collections.Generic.List<string> EventPropertyLinkColumns = new List<string>();
            EventPropertyLinkColumns.Add("ResponsibleAgentURI");
            PotentialSourceLinks.Add("CacheCollectionEventProperty", EventPropertyLinkColumns);

            //CacheCollectionSpecimenImage - CreatorAgentURI
            System.Collections.Generic.List<string> SpecimenImageLinkColumns = new List<string>();
            SpecimenImageLinkColumns.Add("CreatorAgentURI");
            PotentialSourceLinks.Add("CacheCollectionSpecimenImage", SpecimenImageLinkColumns);

            //CacheCollectionSpecimenProcessing - ResponsibleAgentURI
            System.Collections.Generic.List<string> SpecimenProcessingLinkColumns = new List<string>();
            SpecimenProcessingLinkColumns.Add("ResponsibleAgentURI");
            PotentialSourceLinks.Add("CacheCollectionSpecimenProcessing", SpecimenProcessingLinkColumns);

            //CacheCollectionSpecimenReference - ResponsibleAgentURI
            System.Collections.Generic.List<string> SpecimenReferenceLinkColumns = new List<string>();
            SpecimenReferenceLinkColumns.Add("ResponsibleAgentURI");
            PotentialSourceLinks.Add("CacheCollectionSpecimenReference", SpecimenReferenceLinkColumns);

            //CacheIdentificationUnitAnalysis - ResponsibleAgentURI
            System.Collections.Generic.List<string> UnitAnalysisLinkColumns = new List<string>();
            UnitAnalysisLinkColumns.Add("ResponsibleAgentURI");
            PotentialSourceLinks.Add("CacheIdentificationUnitAnalysis", UnitAnalysisLinkColumns);

            //CacheIdentificationUnitGeoAnalysis - ResponsibleAgentURI
            System.Collections.Generic.List<string> UnitGeoAnalysisLinkColumns = new List<string>();
            UnitGeoAnalysisLinkColumns.Add("ResponsibleAgentURI");
            PotentialSourceLinks.Add("CacheIdentificationUnitGeoAnalysis", UnitGeoAnalysisLinkColumns);

            this.SearchForSources("Agents", PotentialSourceLinks);
        }

#endregion

#region Gazetteer Sources

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> _I_Gazetteer;

        public bool initGazetteerSources()
        {
            this._I_Gazetteer = new List<InterfaceLookupSource>();
            bool OK = this.initSources(ref this._I_Gazetteer, this.panelGazetteerSources, this.toolStripGazetteerSources, this.dataSetCacheDatabase.GazetteerSource, this._SqlDataAdapterGazetteerSource, UserControlLookupSource.TypeOfSource.Gazetteer);
            return OK;




            //bool OK = true;
            string SQL = "SELECT COUNT(*) " +
                    "FROM GazetteerSource ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                this.toolStripGazetteerSources.Enabled = true;//.toolStripTermSources.Enabled = true;
                try
                {
                    this.dataSetCacheDatabase.GazetteerSource.Clear();//.ScientificTermSource.Clear();
                    SQL = "SELECT SourceView, Source, SourceID, LinkedServerName, DatabaseName " +
                        "FROM GazetteerSource ";
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        SQL += " WHERE IncludeInTransfer = 1 ";
                    SQL += "ORDER BY SourceView ";
                    if (this._SqlDataAdapterGazetteerSource == null)//._SqlDataAdapterScientificTermSource == null)
                        this._SqlDataAdapterGazetteerSource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    else
                        this._SqlDataAdapterGazetteerSource.SelectCommand.CommandText = SQL;
                    this._SqlDataAdapterGazetteerSource.Fill(this.dataSetCacheDatabase.GazetteerSource);
                    System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                    foreach (System.Windows.Forms.Control C in this.panelGazetteerSources.Controls)
                    {
                        CCtoDelete.Add(C);
                    }
                    this.panelGazetteerSources.Controls.Clear();
                    foreach (System.Windows.Forms.Control C in CCtoDelete)
                    {
                        C.Dispose();
                    }
                    foreach (System.Data.DataRow R in this.dataSetCacheDatabase.GazetteerSource.Rows)
                    {
                        DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, UserControlLookupSource.TypeOfSource.Gazetteer);
                        U.Dock = DockStyle.Top;
                        this.panelGazetteerSources.Controls.Add(U);
                        U.BringToFront();
                        U.setHelp("Cache database gazetteer");
                        this._I_Gazetteer.Add(U);
                    }
                    //this.setp();
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                this.toolStripGazetteerSources.Enabled = false;
            }
            return OK;
        }

        private void toolStripButtonAddGazetteerSource_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables = new Dictionary<UserControlLookupSource.SubsetTable, string>();
            DataTables.Add(UserControlLookupSource.SubsetTable.Gazetteer, "");
            DataTables.Add(UserControlLookupSource.SubsetTable.GazetteerExternalDatabase, "_E");
            if (this.AddSource(DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer, UserControlLookupSource.SubsetTable.Gazetteer, DataTables, UserControlLookupSource.TypeOfSource.Gazetteer))
                this.initGazetteerSources();
            return;





            System.Collections.Generic.List<string> SourceList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnections())
            {
                SourceList.Add(KV.Value.DisplayText);
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string Source = f.String;
                DiversityWorkbench.ServerConnection S = null;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnections())
                {
                    if (KV.Value.DisplayText == Source)
                    {
                        S = KV.Value;
                        break;
                    }
                }
                if (S != null)
                {
                    System.Data.DataTable dtGazetteer = new DataTable();
                    string PrefixDB = "";
                    if (S.LinkedServer.Length > 0)
                        PrefixDB = "[" + S.LinkedServer + "].";
                    PrefixDB += S.DatabaseName + ".dbo.";
                    string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectProxy ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
                    ad.Fill(dtGazetteer);
                    f = new DiversityWorkbench.Forms.FormGetStringFromList(dtGazetteer, "Project", "ProjectID", "Project", "Please select the project");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        int ProjectID;
                        if (int.TryParse(f.SelectedValue, out ProjectID))
                        {
                            string Project = f.SelectedString;
                            this.CreateGazetteerSource(S, f.SelectedString, ProjectID);
                            this.initGazetteerSources();
                        }
                    }
                }
            }
        }

        private void initPostgresControlsForGazetteerSources()
        {
            if (this._I_Gazetteer != null)
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource T in this._I_Gazetteer)
                    T.initPostgresControls();
            }
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                this.initMissingSourceViews("GazetteerSource", this.panelGazetteerSources, false);
            }
        }

        private void toolStripButtonGazetteerTransferSourceToCache_Click(object sender, EventArgs e)
        {
            this.TransferGazetteerSources(/*false,*/ true, false);
        }

        private void toolStripButtonGazetteerTransferCacheToPostgres_Click(object sender, EventArgs e)
        {
            this.TransferGazetteerSources(/*false,*/ false, true);
        }

        private string TransferGazetteerSources(/*bool ProcessOnly,*/ bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres)
        {
            string Report = this.TransferSources(/*ProcessOnly, */IncludeTransferToCacheDB, IncludeTransferToPostgres, this._I_Gazetteer, "Gazetteer");//, this._FilterGazetteerForUpdate);// "";
            return Report;
        }

        private void toolStripButtonGazetteerFilter_Click(object sender, EventArgs e)
        {
            if (this._FilterGazetteerForUpdate)
            {
                this.toolStripButtonGazetteerFilter.Text = "Data not filtered for updates";
                this.toolStripButtonGazetteerFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                this.toolStripButtonGazetteerFilter.Text = "Data filtered for updates later than last transfer";
                this.toolStripButtonGazetteerFilter.Image = DiversityCollection.Resource.Filter;
            }
            this._FilterGazetteerForUpdate = !this._FilterGazetteerForUpdate;
        }

        private void toolStripButtonGazetteerSearchSources_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> PotentialSourceLinks = new Dictionary<string, List<string>>();

            // CacheCollectionEventLocalisation - Location2
            System.Collections.Generic.List<string> CacheCollectionEventLocalisationLinkColumns = new List<string>();
            CacheCollectionEventLocalisationLinkColumns.Add("Location2");
            PotentialSourceLinks.Add("CacheCollectionSpecimen", CacheCollectionEventLocalisationLinkColumns);

            this.SearchForSources("Gazetteer", PotentialSourceLinks);
        }

#region Creation of source

        /// <summary>
        /// Creating a view for the gazetteer from a DiversityGazetteer source either local or from a linked server
        /// </summary>
        /// <param name="Database">Name of the database, may include linked server as first part</param>
        /// <returns></returns>
        private string CreateGazetteerSource(DiversityWorkbench.ServerConnection ServerConn, string Project, int ProjectID)
        {
            string View = "";
            try
            {
                View = ServerConn.DatabaseName.Replace("Diversity", "") + "_"; ;
                if (ServerConn.LinkedServer.Length > 0)
                {
                    string LinkedServerMarker = ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                    if (!View.EndsWith("_" + LinkedServerMarker))
                        View += ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                }
                bool ToUpper = true;
                foreach (char c in Project)
                {
                    int i = (int)c;
                    if (i == 32) ToUpper = true;
                    if (i < 48 || (i > 58 && i < 65) || (i > 90 && i < 97) || i > 122)
                        continue;
                    if (ToUpper) View += c.ToString().ToUpper();
                    else View += c.ToString();
                    ToUpper = false;
                    if (View.Length > 100) break;
                }
                string SQL = "SELECT COUNT(*) FROM GazetteerSource S WHERE S.SourceView LIKE '" + View + "%'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result != "0" && Result != "")
                {
                    int i = int.Parse(Result);
                    View += "_" + (i + 1).ToString();
                }
                string PrefixDB = "";
                if (ServerConn.LinkedServer.Length > 0)
                    PrefixDB = "[" + ServerConn.LinkedServer + "].";
                PrefixDB += ServerConn.DatabaseName + ".dbo.";

                SQL = "CREATE VIEW [dbo].[" + View + "] " +
                    "AS SELECT U.BaseURL, N.NameID, N.Name, N.LanguageCode, N.PlaceID, P.PlaceType, " +
                    "N.Name AS PreferredName, N.NameID AS PreferredNameID, N.LanguageCode AS PreferredNameLanguageCode, N.ExternalNameID, N.ExternalDatabaseID, PN.ProjectID, N.LogUpdatedWhen " +
                    "FROM " + PrefixDB + "GeoName AS N INNER JOIN " +
                    "" + PrefixDB + "GeoProject AS PN ON N.NameID = PN.NameID INNER JOIN " +
                    "" + PrefixDB + "ViewGeoPlace AS P ON N.PlaceID = P.PlaceID /* AND N.NameID = P.PreferredNameID */ CROSS JOIN " +
                    "" + PrefixDB + "ViewBaseURL AS U " +
                    "WHERE PN.ProjectID = " + ProjectID.ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    {
                        SQL = "INSERT INTO GazetteerSource " +
                            "(SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets, Version) " +
                            "VALUES ('" + View + "', '" + Project + "', " + ProjectID.ToString() +
                            " , '" + ServerConn.LinkedServer + "', '" + ServerConn.DatabaseName + "' " +
                            ", '" + this.SourceSubsets(UserControlLookupSource.SubsetTable.Gazetteer) + "' " +
                            ", " + DiversityCollection.CacheDatabase.UserControlLookupSource.LookupSourceVersion[UserControlLookupSource.TypeOfSource.Gazetteer].ToString() + ")";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        {
                            View = "";
                        }
                    }
                    else View = "";
                }
                else View = "";
            }
            catch (Exception ex)
            {
                View = "";
            }
            return View;
        }

#endregion

#endregion

#region References

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> _I_References;

        public bool initReferenceTitleSources(/*bool ProcessOnly*/)
        {
            this._I_References = new List<InterfaceLookupSource>();
            bool OK = this.initSources(ref this._I_References, this.panelReferences, this.toolStripReferences, this.dataSetCacheDatabase.ReferenceTitleSource, this._SqlDataAdapterReferencesSource, UserControlLookupSource.TypeOfSource.References);
            return OK;


            string SQL = "SELECT COUNT(*) " +
                    "FROM ReferenceTitleSource ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                this.toolStripReferences.Enabled = true;
                try
                {
                    this.dataSetCacheDatabase.ReferenceTitleSource.Clear();
                    SQL = "SELECT SourceView, Source, SourceID, LinkedServerName, DatabaseName " +
                        "FROM ReferenceTitleSource ";
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        SQL += " WHERE IncludeInTransfer = 1 ";
                    SQL += "ORDER BY SourceView ";
                    if (this._SqlDataAdapterReferencesSource == null)
                        this._SqlDataAdapterReferencesSource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    else
                        this._SqlDataAdapterReferencesSource.SelectCommand.CommandText = SQL;
                    this._SqlDataAdapterReferencesSource.Fill(this.dataSetCacheDatabase.ReferenceTitleSource);
                    System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                    foreach (System.Windows.Forms.Control C in this.panelReferences.Controls)
                    {
                        CCtoDelete.Add(C);
                    }
                    this.panelReferences.Controls.Clear();
                    foreach (System.Windows.Forms.Control C in CCtoDelete)
                    {
                        C.Dispose();
                    }
                    foreach (System.Data.DataRow R in this.dataSetCacheDatabase.ReferenceTitleSource.Rows)
                    {
                        DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, UserControlLookupSource.TypeOfSource.References);
                        U.Dock = DockStyle.Top;
                        this.panelReferences.Controls.Add(U);
                        U.BringToFront();
                        U.setHelp("Cache references");
                        this._I_References.Add(U);
                    }
                    //this.setp();
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                this.toolStripReferences.Enabled = false;
            }
            return OK;
        }

        private void toolStripButtonReferencesAdd_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables = new Dictionary<UserControlLookupSource.SubsetTable, string>();
            DataTables.Add(UserControlLookupSource.SubsetTable.ReferenceTitle, "");
            DataTables.Add(UserControlLookupSource.SubsetTable.ReferenceRelator, "_R");
            if (this.AddSource(DiversityWorkbench.WorkbenchUnit.ModuleType.References, UserControlLookupSource.SubsetTable.ReferenceTitle, DataTables, UserControlLookupSource.TypeOfSource.References))
                this.initReferenceTitleSources();
            return;


            System.Collections.Generic.List<string> SourceList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnections())
            {
                SourceList.Add(KV.Value.DisplayText);
            }
            if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
            {
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string Source = f.String;
                DiversityWorkbench.ServerConnection S = null;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnections())
                {
                    if (KV.Value.DisplayText == Source)
                    {
                        S = KV.Value;
                        break;
                    }
                }
                if (S != null)
                {
                    string PrefixDB = "";
                    if (S.LinkedServer.Length > 0)
                        PrefixDB = "[" + S.LinkedServer + "].";
                    PrefixDB += S.DatabaseName + ".dbo.";
                    System.Data.DataTable dtProject = new DataTable();
                    string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectList ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
                    ad.Fill(dtProject);
                    DiversityWorkbench.Forms.FormGetStringFromList fProject = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Please select the project");
                    fProject.ShowDialog();
                    if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        int ProjectID;
                        if (int.TryParse(fProject.SelectedValue, out ProjectID))
                        {
                            string Project = fProject.SelectedString;
                            string View = this.CreateReferencesSource(S, fProject.SelectedString, ProjectID);
                            if (View.Length > 0)
                            {
                                SQL = "INSERT INTO ReferenceTitleSource (SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets, Version) " +
                                    "VALUES ('" + View + "', '" + Project + "', " + ProjectID.ToString() + ", '" + S.LinkedServer + "', '" + S.DatabaseName + "' " +
                                    ", '" + this.SourceSubsets(UserControlLookupSource.SubsetTable.ReferenceTitle) + "' " +
                                    ", " + DiversityCollection.CacheDatabase.UserControlLookupSource.LookupSourceVersion[UserControlLookupSource.TypeOfSource.References].ToString() + ")";
                                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                    this.initReferenceTitleSources();
                            }
                        }
                    }
                }
            }
        }

        private void toolStripButtonReferencesFilter_Click(object sender, EventArgs e)
        {
            if (this._FilterReferencesForUpdate)
            {
                this.toolStripButtonReferencesFilter.Text = "Data not filtered for updates";
                this.toolStripButtonReferencesFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                this.toolStripButtonReferencesFilter.Text = "Data filtered for updates later than last transfer";
                this.toolStripButtonReferencesFilter.Image = DiversityCollection.Resource.Filter;
            }
            this._FilterReferencesForUpdate = !this._FilterReferencesForUpdate;
        }

        private void toolStripButtonReferencesTransferSource_Click(object sender, EventArgs e)
        {
            this.TransferReferenceTitleSources(false, true, false);
        }

        private string TransferReferenceTitleSources(bool ProcessOnly, bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres)
        {
            string Report = this.TransferSources(/*ProcessOnly,*/ IncludeTransferToCacheDB, IncludeTransferToPostgres, this._I_References, "References");//, this._FilterReferencesForUpdate);// "";
            return Report;
        }

        private void toolStripButtonReferencesTransferToPostgres_Click(object sender, EventArgs e)
        {
            this.TransferReferenceTitleSources(false, false, true);
        }

        private void initPostgresControlsForReferences()
        {
            if (this._I_References != null)
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource R in this._I_References)
                    R.initPostgresControls();
            }
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                this.initMissingSourceViews("ReferenceTitleSource", this.panelReferences, false);
            }
        }

#region Creation of source

        private string CreateReferencesSource(DiversityWorkbench.ServerConnection ServerConn, string Project, int ProjectID)
        {
            string View = ServerConn.DatabaseName.Replace("Diversity", "") + "_";
            if (ServerConn.LinkedServer.Length > 0)
            {
                string LinkedServerMarker = ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                if (!View.EndsWith("_" + LinkedServerMarker))
                    View += ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
            }
            View += Project;
            View = this.ConvertToSqlName(View);
            // Check if there are sources in table ReferenceTitleSource with the same name and change name if needed
            string SQL = "SELECT COUNT(*) FROM ReferenceTitleSource S WHERE S.SourceName LIKE '" + View + "%'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result != "0" && Result != "")
            {
                int i = int.Parse(Result);
                View += "_" + (i + 1).ToString();
            }
            string PrefixDB = "";
            if (ServerConn.LinkedServer.Length > 0)
                PrefixDB = "[" + ServerConn.LinkedServer + "].";
            PrefixDB += ServerConn.DatabaseName + ".dbo.";
            string Database = ServerConn.DatabaseName;
            SQL = "SELECT U.BaseURL FROM " + PrefixDB + "ViewBaseURL AS U";
            string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (BaseURL.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("This database is not prepared for usage by cache databases. Turn to your administrator for an update");
                return "";
            }

            // Check if the view allready exists and remove it after OK
            SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return "";
                    }
                }
                else
                    return "";
            }
            SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT B.BaseURL, R.RefType, R.RefID, R.RefDescription_Cache, R.Title, R.DateYear, R.DateMonth, R.DateDay, R.DateSuppl, R.SourceTitle, R.SeriesTitle, R.Periodical, R.Volume, R.Issue, R.Pages, R.Publisher, " +
                "R.PublPlace, R.Edition, R.DateYear2, R.DateMonth2, R.DateDay2, R.DateSuppl2, R.ISSN_ISBN, R.Miscellaneous1, R.Miscellaneous2, R.Miscellaneous3, R.UserDef1, R.UserDef2, R.UserDef3, R.UserDef4,  " +
                "R.UserDef5, R.WebLinks, R.LinkToPDF, R.LinkToFullText, R.RelatedLinks, R.LinkToImages, R.SourceRefID, R.Language, R.CitationText, R.CitationFrom, R.LogInsertedWhen AS LogUpdatedWhen, P.ProjectID " +
                "FROM " + PrefixDB + "ReferenceTitle AS R, " +
                PrefixDB + "ReferenceProject AS P, " +
                PrefixDB + "ViewBaseURL AS B " +
                "WHERE R.RefID = P.RefID AND P.ProjectID = " + ProjectID.ToString();

            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    if (this.CreateReferenceRelatorSource(PrefixDB, View, ProjectID))
                        return View;
                    else
                        return "";
                }
                else
                    return "";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Message);
                return "";
            }
        }

        /// <summary>
        /// Auxillary view for references
        /// </summary>
        /// <param name="PrefixDB">Name of the database, may include linked server as first part</param>
        /// <param name="View">name of the main view</param>
        /// <param name="ProjectID">ID of the project</param>
        /// <returns>If creation of view was successful</returns>
        private bool CreateReferenceRelatorSource(string PrefixDB, string View, int ProjectID)
        {
            bool OK = true;
            View += "_R";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT B.BaseURL, R.RefID, R.Role, R.Sequence, R.Name, R.AgentURI, R.SortLabel, R.Address, T.LogInsertedWhen AS LogUpdatedWhen " +
            "FROM " + PrefixDB + "ReferenceTitle AS T, " + PrefixDB + "ReferenceRelator AS R , " +
            PrefixDB + "ReferenceProject AS P, " +
            PrefixDB + "ViewBaseURL AS B " +
            "WHERE T.RefID = R.RefID AND R.RefID = P.RefID AND P.ProjectID = " + ProjectID.ToString();

            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

#endregion

        private void toolStripButtonReferencesSearchSources_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> PotentialSourceLinks = new Dictionary<string, List<string>>();

            // CacheCollectionSpecimen - ReferenceURI
            System.Collections.Generic.List<string> CollectionSpecimenLinkColumns = new List<string>();
            CollectionSpecimenLinkColumns.Add("ReferenceURI");
            PotentialSourceLinks.Add("CacheCollectionSpecimen", CollectionSpecimenLinkColumns);

            //CacheCollectionSpecimenReference - ResponsibleAgentURI
            System.Collections.Generic.List<string> SpecimenReferenceLinkColumns = new List<string>();
            SpecimenReferenceLinkColumns.Add("ReferenceURI");
            PotentialSourceLinks.Add("CacheCollectionSpecimenReference", SpecimenReferenceLinkColumns);

            // CacheIdentification
            System.Collections.Generic.List<string> IdentificationLinkColumns = new List<string>();
            IdentificationLinkColumns.Add("ReferenceURI");
            PotentialSourceLinks.Add("CacheIdentification", IdentificationLinkColumns);

            //CacheProjectReference
            System.Collections.Generic.List<string> ProjectReferenceLinkColumns = new List<string>();
            ProjectReferenceLinkColumns.Add("ReferenceURI");
            PotentialSourceLinks.Add("CacheProjectReference", ProjectReferenceLinkColumns);

            this.SearchForSources("References", PotentialSourceLinks);
        }

#endregion

#region SamplingPlots

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> _I_Plots;

        public bool initPlotSources()
        {
            bool OK = true;
            OK = this.initSources(
                ref this._I_Plots,
                this.panelPlots,
                this.toolStripPlots,
                this.dataSetCacheDatabase.SamplingPlotSource,
                this._SqlDataAdapterPlotSource,
                UserControlLookupSource.TypeOfSource.Plots,
                "SamplingPlotSource");
            if (this._I_Plots == null)
                this._I_Plots = new List<InterfaceLookupSource>();
            return OK;

            /*
            this._I_Plots = new List<InterfaceLookupSource>();
            bool OK = true;
            string SQL = "SELECT COUNT(*) " +
                    "FROM SamplingPlotSource ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                this.toolStripPlots.Enabled = true;
                try
                {
                    this.dataSetCacheDatabase.SamplingPlotSource.Clear();
                    SQL = "SELECT SourceView, Source, SourceID, LinkedServerName, DatabaseName " +
                        "FROM SamplingPlotSource ";
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        SQL += " WHERE IncludeInTransfer = 1 ";
                    SQL += "ORDER BY SourceView ";
                    if (this._SqlDataAdapterPlotSource == null)
                        this._SqlDataAdapterPlotSource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    else
                        this._SqlDataAdapterPlotSource.SelectCommand.CommandText = SQL;
                    this._SqlDataAdapterPlotSource.Fill(this.dataSetCacheDatabase.SamplingPlotSource);
                    System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                    foreach (System.Windows.Forms.Control C in this.panelPlots.Controls)
                    {
                        CCtoDelete.Add(C);
                    }
                    this.panelPlots.Controls.Clear();
                    foreach (System.Windows.Forms.Control C in CCtoDelete)
                    {
                        C.Dispose();
                    }
                    foreach (System.Data.DataRow R in this.dataSetCacheDatabase.SamplingPlotSource.Rows)
                    {
                        DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, UserControlLookupSource.TypeOfSource.Plots);
                        U.Dock = DockStyle.Top;
                        this.panelPlots.Controls.Add(U);
                        U.BringToFront();
                        U.setHelp("Cache plots");
                        this._I_Plots.Add(U);
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                this.toolStripPlots.Enabled = false;
            }
            return OK;
             * */
        }

        private void toolStripButtonPlotsAddSource_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables = new Dictionary<UserControlLookupSource.SubsetTable, string>();
            DataTables.Add(UserControlLookupSource.SubsetTable.SamplingPlot, "");
            DataTables.Add(UserControlLookupSource.SubsetTable.SamplingPlotLocalisation, "_L");
            DataTables.Add(UserControlLookupSource.SubsetTable.SamplingPlotProperty, "_P");
            if (this.AddSource(DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots, UserControlLookupSource.SubsetTable.SamplingPlot, DataTables, UserControlLookupSource.TypeOfSource.Plots))
                this.initPlotSources();
            return;




            System.Collections.Generic.List<string> SourceList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversitySamplingPlots"].ServerConnections())
            {
                SourceList.Add(KV.Value.DisplayText);
            }
            if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
            {
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string Source = f.String;
                DiversityWorkbench.ServerConnection S = null;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversitySamplingPlots"].ServerConnections())
                {
                    if (KV.Value.DisplayText == Source)
                    {
                        S = KV.Value;
                        break;
                    }
                }
                if (S != null)
                {
                    string PrefixDB = "";
                    if (S.LinkedServer.Length > 0)
                        PrefixDB = "[" + S.LinkedServer + "].";
                    PrefixDB += S.DatabaseName + ".dbo.";
                    System.Data.DataTable dtProject = new DataTable();
                    string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectList ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
                    ad.Fill(dtProject);
                    DiversityWorkbench.Forms.FormGetStringFromList fProject = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Please select the project");
                    fProject.ShowDialog();
                    if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        int ProjectID;
                        if (int.TryParse(fProject.SelectedValue, out ProjectID))
                        {
                            string Project = fProject.SelectedString;
                            string View = this.CreatePlotSource(S, fProject.SelectedString, ProjectID);
                            if (View.Length > 0)
                            {
                                SQL = "INSERT INTO SamplingPlotSource (SourceView, Source, SourceID, LinkedServerName, DatabaseName) " +
                                    "VALUES ('" + View + "', '" + Project + "', " + ProjectID.ToString() + ", '" + S.LinkedServer + "', '" + S.DatabaseName + "')";
                                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                    this.initPlotSources();
                            }
                        }
                    }
                }
            }
        }

        private void toolStripButtonPlotsTransferSource_Click(object sender, EventArgs e)
        {
            this.TransferPlotSources(true, false);
        }

        private void toolStripButtonPlotsTransferToPostgres_Click(object sender, EventArgs e)
        {
            this.TransferPlotSources(false, true);
        }

        private string TransferPlotSources(bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres)
        {
            string Report = "";
            if (this._I_Plots != null && this._I_Plots.Count > 0)
                Report = this.TransferSources(/*ProcessOnly,*/ IncludeTransferToCacheDB, IncludeTransferToPostgres, this._I_Plots, "Plots");//, this._FilterPlotsForUpdate);// "";
            return Report;
        }

        private void initPostgresControlsForPlots()
        {
            if (this._I_Plots != null)
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource A in this._I_Plots)
                    A.initPostgresControls();
            }
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                this.initMissingSourceViews("SamplingPlotSource", this.panelPlots, false);
            }

        }

#region Creation of source

        private string CreatePlotSource(DiversityWorkbench.ServerConnection ServerConn, string Project, int ProjectID)
        {
            string View = ServerConn.DatabaseName.Replace("Diversity", "") + "_";
            if (ServerConn.LinkedServer.Length > 0)
            {
                string LinkedServerMarker = ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                if (!View.EndsWith("_" + LinkedServerMarker))
                    View += ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
            }
            View += Project;
            View = this.ConvertToSqlName(View);
            // Check if there are sources in table PlotSource with the same name and change name if needed
            string SQL = "SELECT COUNT(*) FROM SamplingPlotSource S WHERE S.SourceName LIKE '" + View + "%'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result != "0" && Result != "")
            {
                int i = int.Parse(Result);
                View += "_" + (i + 1).ToString();
            }
            string PrefixDB = "";
            if (ServerConn.LinkedServer.Length > 0)
                PrefixDB = "[" + ServerConn.LinkedServer + "].";
            PrefixDB += ServerConn.DatabaseName + ".dbo.";
            string Database = ServerConn.DatabaseName;
            SQL = "SELECT U.BaseURL FROM " + PrefixDB + "ViewBaseURL AS U";
            string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (BaseURL.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("This database is not prepared for usage by cache databases. Turn to your administrator for an update");
                return "";
            }

            // Check if the view allready exists and remove it after OK
            SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return "";
                    }
                }
                else
                    return "";
            }
            SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT B.BaseURL,  S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache.ToString() AS PlotGeography_Cache, S.PlotDescription, " +
                "S.PlotType, S.CountryCache, P.ProjectID, S.LogUpdatedWhen " +
                "FROM " + PrefixDB + "SamplingPlot AS S, " +
                PrefixDB + "SamplingProject AS P, " +
                PrefixDB + "ViewBaseURL AS B " +
                "WHERE S.PlotID = P.PlotID AND P.ProjectID = " + ProjectID.ToString();

            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    if (this.CreatePlotLocalisationSource(PrefixDB, View, ProjectID)
                        && this.CreatePlotPropertySource(PrefixDB, View, ProjectID))
                        return View;
                    else
                        return "";
                }
                else
                    return "";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Message);
                return "";
            }
        }

        /// <summary>
        /// Auxillary view for sampling plots
        /// </summary>
        /// <param name="PrefixDB">Name of the database, may include linked server as first part</param>
        /// <param name="View">name of the main view</param>
        /// <returns></returns>
        private bool CreatePlotLocalisationSource(string PrefixDB, string View, int ProjectID)
        {
            bool OK = true;
            View += "_L";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT B.BaseURL, S.PlotID, S.LocalisationSystemID, S.Location1, S.Location2, S.LocationAccuracy, S.LocationNotes, S.DeterminationDate, S.DistanceToLocation, " +
            "S.DirectionToLocation, S.ResponsibleName, S.ResponsibleAgentURI, S.Geography.ToString() AS Geography, " +
            "S.AverageAltitudeCache, S.AverageLatitudeCache, S.AverageLongitudeCache, S.LogUpdatedWhen " +
            "FROM " + PrefixDB + "SamplingPlotLocalisation AS S, " +
            PrefixDB + "SamplingProject AS P, " +
            PrefixDB + "ViewBaseURL AS B " +
            "WHERE S.PlotID = P.PlotID AND P.ProjectID = " + ProjectID.ToString();
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

#endregion

        /// <summary>
        /// Auxillary view for sampling plots
        /// </summary>
        /// <param name="PrefixDB">Name of the database, may include linked server as first part</param>
        /// <param name="View">name of the main view</param>
        /// <returns></returns>
        private bool CreatePlotPropertySource(string PrefixDB, string View, int ProjectID)
        {
            bool OK = true;
            View += "_P";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT B.BaseURL, S.PlotID, S.PropertyID, S.PropertyURI, S.DisplayText, S.PropertyHierarchyCache, S.PropertyValue, " +
            "S.ResponsibleName, S.ResponsibleAgentURI, S.Notes, S.AverageValueCache, S.LogUpdatedWhen " +
            "FROM " + PrefixDB + "SamplingPlotProperty AS S, " +
            PrefixDB + "SamplingProject AS P, " +
            PrefixDB + "ViewBaseURL AS B " +
            "WHERE S.PlotID = P.PlotID AND P.ProjectID = " + ProjectID.ToString();
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private void toolStripButtonPlotsFilter_Click(object sender, EventArgs e)
        {
            if (this._FilterPlotsForUpdate)
            {
                //this.toolStripButtonPlotsFilter.Text = "Data not filtered for updates";
                //this.toolStripButtonPlotsFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                //this.toolStripButtonPlotsFilter.Text = "Data filtered for updates later than last transfer";
                //this.toolStripButtonPlotsFilter.Image = DiversityCollection.Resource.Filter;
            }
            this._FilterPlotsForUpdate = !this._FilterPlotsForUpdate;
        }

        private void toolStripButtonPlotsSearchSources_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> PotentialSourceLinks = new Dictionary<string, List<string>>();

            //// Collector
            //System.Collections.Generic.List<string> CollectorLinkColumns = new List<string>();
            //CollectorLinkColumns.Add("CollectorsAgentURI");
            //PotentialSourceLinks.Add("CacheCollectionAgent", CollectorLinkColumns);

            //// Identifier
            //System.Collections.Generic.List<string> IdentificationLinkColumns = new List<string>();
            //IdentificationLinkColumns.Add("ResponsibleAgentURI");
            //PotentialSourceLinks.Add("CacheIdentification", IdentificationLinkColumns);

            //// CacheCollection - AdministrativeContactAgentURI
            //System.Collections.Generic.List<string> CollectionLinkColumns = new List<string>();
            //CollectionLinkColumns.Add("AdministrativeContactAgentURI");
            //PotentialSourceLinks.Add("CacheCollection", CollectionLinkColumns);

            ////CacheCollectionEventLocalisation - ResponsibleAgentURI
            //System.Collections.Generic.List<string> EventLocalisationLinkColumns = new List<string>();
            //EventLocalisationLinkColumns.Add("ResponsibleAgentURI");
            //PotentialSourceLinks.Add("CacheCollectionEventLocalisation", EventLocalisationLinkColumns);

            ////CacheCollectionEventProperty - ResponsibleAgentURI
            //System.Collections.Generic.List<string> EventPropertyLinkColumns = new List<string>();
            //EventPropertyLinkColumns.Add("ResponsibleAgentURI");
            //PotentialSourceLinks.Add("CacheCollectionEventProperty", EventPropertyLinkColumns);

            ////CacheCollectionSpecimenImage - CreatorAgentURI
            //System.Collections.Generic.List<string> SpecimenImageLinkColumns = new List<string>();
            //SpecimenImageLinkColumns.Add("CreatorAgentURI");
            //PotentialSourceLinks.Add("CacheCollectionSpecimenImage", SpecimenImageLinkColumns);

            ////CacheCollectionSpecimenProcessing - ResponsibleAgentURI
            //System.Collections.Generic.List<string> SpecimenProcessingLinkColumns = new List<string>();
            //SpecimenProcessingLinkColumns.Add("ResponsibleAgentURI");
            //PotentialSourceLinks.Add("CacheCollectionSpecimenProcessing", SpecimenProcessingLinkColumns);

            ////CacheCollectionSpecimenReference - ResponsibleAgentURI
            //System.Collections.Generic.List<string> SpecimenReferenceLinkColumns = new List<string>();
            //SpecimenReferenceLinkColumns.Add("ResponsibleAgentURI");
            //PotentialSourceLinks.Add("CacheCollectionSpecimenReference", SpecimenReferenceLinkColumns);

            ////CacheIdentificationUnitAnalysis - ResponsibleAgentURI
            //System.Collections.Generic.List<string> UnitAnalysisLinkColumns = new List<string>();
            //UnitAnalysisLinkColumns.Add("ResponsibleAgentURI");
            //PotentialSourceLinks.Add("CacheIdentificationUnitAnalysis", UnitAnalysisLinkColumns);

            ////CacheIdentificationUnitGeoAnalysis - ResponsibleAgentURI
            //System.Collections.Generic.List<string> UnitGeoAnalysisLinkColumns = new List<string>();
            //UnitGeoAnalysisLinkColumns.Add("ResponsibleAgentURI");
            //PotentialSourceLinks.Add("CacheIdentificationUnitGeoAnalysis", UnitGeoAnalysisLinkColumns);

            this.SearchForSources("Plots", PotentialSourceLinks);
        }

#endregion

#region Scientific Terms

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> _I_Terms;

        public bool initTermSources(/*bool ProcessOnly*/)
        {
            return this.initSources(
                ref this._I_Terms,
                this.panelTerms, 
                this.toolStripTermSources, 
                this.dataSetCacheDatabase.ScientificTermSource, 
                this._SqlDataAdapterScientificTermSource, 
                UserControlLookupSource.TypeOfSource.ScientificTerms,
                "ScientificTermSource", 
                "Cache database terms");




            this._I_Terms = new List<InterfaceLookupSource>();
            bool OK = true;
            string SQL = "SELECT COUNT(*) " +
                    "FROM ScientificTermSource ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                this.toolStripTermSources.Enabled = true;
                try
                {
                    this.dataSetCacheDatabase.ScientificTermSource.Clear();
                    SQL = "SELECT SourceView, Source, SourceID, LinkedServerName, DatabaseName " +
                        "FROM ScientificTermSource ";
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        SQL += " WHERE IncludeInTransfer = 1 ";
                    SQL += "ORDER BY SourceView ";
                    if (this._SqlDataAdapterScientificTermSource == null)
                        this._SqlDataAdapterScientificTermSource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    else
                        this._SqlDataAdapterScientificTermSource.SelectCommand.CommandText = SQL;
                    this._SqlDataAdapterScientificTermSource.Fill(this.dataSetCacheDatabase.ScientificTermSource);
                    System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                    foreach (System.Windows.Forms.Control C in this.panelTerms.Controls)
                    {
                        CCtoDelete.Add(C);
                    }
                    this.panelTerms.Controls.Clear();
                    foreach (System.Windows.Forms.Control C in CCtoDelete)
                    {
                        C.Dispose();
                    }
                    foreach (System.Data.DataRow R in this.dataSetCacheDatabase.ScientificTermSource.Rows)
                    {
                        DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, UserControlLookupSource.TypeOfSource.ScientificTerms);
                        U.Dock = DockStyle.Top;
                        this.panelTerms.Controls.Add(U);
                        U.BringToFront();
                        U.setHelp("Cache database terms");
                        this._I_Terms.Add(U);
                    }
                    //this.setp();
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                this.toolStripTaxonSources.Enabled = false;
            }
            return OK;
        }

        private void toolStripButtonAddScientificTermSource_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables = new Dictionary<UserControlLookupSource.SubsetTable, string>();
            DataTables.Add(UserControlLookupSource.SubsetTable.ScientificTerm, "");
            if (this.AddSource(DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms, UserControlLookupSource.SubsetTable.ScientificTerm, DataTables, UserControlLookupSource.TypeOfSource.ScientificTerms))
                this.initTermSources();
            return;



            System.Collections.Generic.List<string> SourceList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnections())
            {
                SourceList.Add(KV.Value.DisplayText);
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string Source = f.String;
                DiversityWorkbench.ServerConnection S = null;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnections())
                {
                    if (KV.Value.DisplayText == Source)
                    {
                        S = KV.Value;
                        break;
                    }
                }
                if (S != null)
                {
                    System.Data.DataTable dtTerminology = new DataTable();
                    string PrefixDB = "";
                    if (S.LinkedServer.Length > 0)
                        PrefixDB = "[" + S.LinkedServer + "].";
                    PrefixDB += S.DatabaseName + ".dbo.";
                    string SQL = "SELECT TerminologyID, DisplayText FROM " + PrefixDB + "Terminology ORDER BY DisplayText";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
                    ad.Fill(dtTerminology);
                    f = new DiversityWorkbench.Forms.FormGetStringFromList(dtTerminology, "DisplayText", "TerminologyID", "Terminology", "Please select the terminology");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        int TerminologyID;
                        if (int.TryParse(f.SelectedValue, out TerminologyID))
                        {
                            string Terminology = f.SelectedString;
                            this.CreateScientificTermsSource(S, f.SelectedString, TerminologyID);
                            this.initTermSources();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Creating a view for the scientific terms from a DiversityScientificTerms source either local or from a linked server
        /// </summary>
        /// <param name="Database">Name of the database, may include linked server as first part</param>
        /// <returns></returns>
        private string CreateScientificTermsSource(DiversityWorkbench.ServerConnection ServerConn, string Terminology, int TerminologyID)
        {
            string View = "";
            try
            {
                View = ServerConn.DatabaseName.Replace("DiversityScientific", "") + "_";
                if (ServerConn.LinkedServer.Length > 0)
                {
                    string LinkedServerMarker = ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                    if (!View.EndsWith("_" + LinkedServerMarker))
                        View += ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                }
                bool ToUpper = true;
                foreach (char c in Terminology)
                {
                    int i = (int)c;
                    if (i == 32) ToUpper = true;
                    if (i < 48 || (i > 58 && i < 65) || (i > 90 && i < 97) || i > 122)
                        continue;
                    if (ToUpper) View += c.ToString().ToUpper();
                    else View += c.ToString();
                    ToUpper = false;
                    if (View.Length > 100) break;
                }
                string SQL = "SELECT COUNT(*) FROM ScientificTermSource S WHERE S.SourceView LIKE '" + View + "%'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result != "0" && Result != "")
                {
                    int i = int.Parse(Result);
                    View += "_" + (i + 1).ToString();
                }
                string PrefixDB = "";
                if (ServerConn.LinkedServer.Length > 0)
                    PrefixDB = "[" + ServerConn.LinkedServer + "].";
                PrefixDB += ServerConn.DatabaseName + ".dbo.";
                SQL = "CREATE VIEW [dbo].[" + View + "] " +
                    "AS SELECT U.BaseURL + CAST(R.RepresentationID AS nvarchar(50)) AS RepresentationURI, R.TerminologyID, " +
                    "R.TermID, R.DisplayText, R.Description, R.HierarchyCache, " +
                    "R.HierarchyCacheDown, R.ExternalID, R.Notes, R.LanguageCode, MIN(AR.DisplayText) AS RankingTerm, R.LogUpdatedWhen " +
                    "FROM " + PrefixDB + "TermRepresentation AS AR INNER JOIN " +
                    "" + PrefixDB + "Term AS A ON AR.TermID = A.TermID RIGHT OUTER JOIN " +
                    "" + PrefixDB + "TermRepresentation AS R INNER JOIN " +
                    "" + PrefixDB + "Term AS T ON R.TermID = T.TermID ON A.TermID = T.RankingTermID CROSS JOIN " +
                    "" + PrefixDB + "ViewBaseURL AS U " +
                    "WHERE R.TerminologyID = " + TerminologyID.ToString() + " AND T.IsRankingTerm = 0 " +
                    " GROUP BY U.BaseURL + CAST(R.RepresentationID AS nvarchar(50)), R.TerminologyID, R.TermID, R.DisplayText, " +
                    "R.Description, R.HierarchyCache, R.HierarchyCacheDown,  " +
                    "R.ExternalID, R.Notes, R.LanguageCode, R.LogUpdatedWhen";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    {
                        SQL = "INSERT INTO ScientificTermSource " +
                            "(SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets, Version) " +
                            "VALUES ('" + View + "', '" + Terminology + "', " + TerminologyID.ToString() +
                            " , '" + ServerConn.LinkedServer + "', '" + ServerConn.DatabaseName + "' " +
                            " '" + this.SourceSubsets(UserControlLookupSource.SubsetTable.ScientificTerm) + "' " +
                            ", " + DiversityCollection.CacheDatabase.UserControlLookupSource.LookupSourceVersion[UserControlLookupSource.TypeOfSource.ScientificTerms].ToString() + ")";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        {
                            View = "";
                        }
                    }
                    else View = "";
                }
                else View = "";
            }
            catch (Exception ex)
            {
                View = "";
            }
            return View;
        }

        private void initPostgresControlsForTerms()
        {
            if (this._I_Terms != null)
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource T in this._I_Terms)
                    T.initPostgresControls();
            }
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                this.initMissingSourceViews("ScientificTermSource", this.panelTerms, false);
            }
        }

        private void toolStripButtonScientificTermSourceTransfer_Click(object sender, EventArgs e)
        {
            this.TransferScientificTermSources(true, false);
        }

        private void toolStripButtonScientificTermTransferToPostgres_Click(object sender, EventArgs e)
        {
            this.TransferScientificTermSources(false, true);
        }

        private string TransferScientificTermSources(bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres)
        {
            string Report = this.TransferSources(/*ProcessOnly,*/ IncludeTransferToCacheDB, IncludeTransferToPostgres, this._I_Terms, "Scientific terms");//, this._FilterTermsForUpdate);// "";
            return Report;
        }


        private void toolStripButtonScientificTermFilter_Click(object sender, EventArgs e)
        {
            if (this._FilterTermsForUpdate)
            {
                this.toolStripButtonScientificTermFilter.Text = "Data not filtered for updates";
                this.toolStripButtonScientificTermFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                this.toolStripButtonScientificTermFilter.Text = "Data filtered for updates later than last transfer";
                this.toolStripButtonScientificTermFilter.Image = DiversityCollection.Resource.Filter;
            }
            this._FilterTermsForUpdate = !this._FilterTermsForUpdate;
        }

        private void toolStripButtonScientificTermSearchSource_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> PotentialSourceLinks = new Dictionary<string, List<string>>();
            System.Collections.Generic.List<string> EventPropertyLinkColumns = new List<string>();
            EventPropertyLinkColumns.Add("PropertyURI");
            PotentialSourceLinks.Add("CacheCollectionEventProperty", EventPropertyLinkColumns);
            System.Collections.Generic.List<string> IdentificationLinkColumns = new List<string>();
            IdentificationLinkColumns.Add("TermURI");
            PotentialSourceLinks.Add("CacheIdentification", IdentificationLinkColumns);
            this.SearchForSources("ScientificTerms", PotentialSourceLinks);
        }

#endregion

#region Taxonomy

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> _I_Taxonomies;
        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> _I_TaxonomyWebservices;

        public bool initTaxonSources(/*bool ProcessOnly*/)
        {
            bool OK = false;
            try
            {
                this._I_Taxonomies = new List<InterfaceLookupSource>();
                OK = this.initSources(ref this._I_Taxonomies, this.panelTaxonSources, this.toolStripTaxonSources, this.dataSetCacheDatabase.TaxonSynonymySource, this._SqlDataAdapterTaxonSynonymySource, UserControlLookupSource.TypeOfSource.Taxa, "", "Cache database taxonomy");
                this.initTaxonWebservices();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;




            string SQL = "SELECT COUNT(*) " +
                    "FROM TaxonSynonymySource ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                this.toolStripTaxonSources.Enabled = true;
                try
                {
                    this.dataSetCacheDatabase.TaxonSynonymySource.Clear();
                    SQL = "SELECT SourceView, Source, SourceID, LinkedServerName, DatabaseName " + //DatabaseName, CASE WHEN SourceName IS NULL THEN '' ELSE SourceName END AS SourceName " +
                        "FROM TaxonSynonymySource WHERE NOT SourceID IS NULL ";
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        SQL += " AND IncludeInTransfer = 1 ";
                    SQL += "ORDER BY SourceView ";
                    if (this._SqlDataAdapterTaxonSynonymySource == null)
                        this._SqlDataAdapterTaxonSynonymySource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    else
                        this._SqlDataAdapterTaxonSynonymySource.SelectCommand.CommandText = SQL;
                    this._SqlDataAdapterTaxonSynonymySource.Fill(this.dataSetCacheDatabase.TaxonSynonymySource);
                    // remove previous controls
                    System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                    foreach (System.Windows.Forms.Control C in this.panelTaxonSources.Controls)
                    {
                        CCtoDelete.Add(C);
                    }
                    this.panelTaxonSources.Controls.Clear();
                    foreach (System.Windows.Forms.Control C in CCtoDelete)
                    {
                        C.Dispose();
                    }
                    // insert current controls
                    foreach (System.Data.DataRow R in this.dataSetCacheDatabase.TaxonSynonymySource.Rows)
                    {
                        DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, UserControlLookupSource.TypeOfSource.Taxa);
                        U.Dock = DockStyle.Top;
                        this.panelTaxonSources.Controls.Add(U);
                        U.BringToFront();
                        U.setHelp("Cache database taxonomy");
                        this._I_Taxonomies.Add(U);
                    }
                    //this.initPostgresControlsForTaxonomy();
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                this.toolStripTaxonSources.Enabled = false;
            }

            this.initTaxonWebservices();

            return OK;
        }

        private void toolStripButtonTaxonSourceAdd_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables = new Dictionary<UserControlLookupSource.SubsetTable, string>();
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonSynonymy, "");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonAnalysis, "_LA");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonAnalysisCategory, "_LAC");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonAnalysisCategoryValue, "_LACV");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonCommonName, "_C");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonHierarchy, "_H");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonList, "_L");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonNameExternalDatabase, "_E");
            DataTables.Add(UserControlLookupSource.SubsetTable.TaxonNameExternalID, "_EID");
            if (this.AddSource(DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames, UserControlLookupSource.SubsetTable.TaxonSynonymy, DataTables, UserControlLookupSource.TypeOfSource.Taxa))
                this.initTaxonSources();
            this.Cursor = System.Windows.Forms.Cursors.Default;
            return;


            System.Collections.Generic.List<string> SourceList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnections())
            {
                SourceList.Add(KV.Value.DisplayText);
            }
            if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
            {
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string Source = f.String;
                DiversityWorkbench.ServerConnection S = null;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnections())
                {
                    if (KV.Value.DisplayText == Source)
                    {
                        S = KV.Value;
                        break;
                    }
                }
                if (S != null)
                {
                    string PrefixDB = "";
                    if (S.LinkedServer.Length > 0)
                        PrefixDB = "[" + S.LinkedServer + "].";
                    PrefixDB += S.DatabaseName + ".dbo.";
                    System.Data.DataTable dtProject = new DataTable();
                    string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectList ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
                    ad.Fill(dtProject);
                    DiversityWorkbench.Forms.FormGetStringFromList fProject = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Please select the project");
                    fProject.ShowDialog();
                    if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        int ProjectID;
                        if (int.TryParse(fProject.SelectedValue, out ProjectID))
                        {
                            string Project = fProject.SelectedString;
                            string View = this.CreateTaxonSource(S, fProject.SelectedString, ProjectID);
                            if (View.Length > 0)
                            {
                                SQL = "INSERT INTO TaxonSynonymySource (SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets, Version) " +
                                    "VALUES ('" + View + "', '" + Project + "', " + ProjectID.ToString() + ", '" + S.LinkedServer + "', '" + S.DatabaseName + "' " +
                                    ", '" + this.SourceSubsets(UserControlLookupSource.SubsetTable.TaxonSynonymy) + "' " +
                                    ", " + DiversityCollection.CacheDatabase.UserControlLookupSource.LookupSourceVersion[UserControlLookupSource.TypeOfSource.Taxa].ToString() + ")";
                                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                    this.initTaxonSources();
                            }
                        }
                    }
                }
            }

        }

#region Creation of source

        private string CreateTaxonSource(DiversityWorkbench.ServerConnection ServerConn, string Project, int ProjectID)
        {
            string View = ServerConn.DatabaseName.Replace("DiversityTaxon", "") + "_";
            if (ServerConn.LinkedServer.Length > 0)
            {
                string LinkedServerMarker = ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                if (!View.EndsWith("_" + LinkedServerMarker))
                    View += ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
            }
            View += Project;
            View = this.ConvertToSqlName(View);
            // Check if there are sources in table TaxonSynonymySource with the same name and change name if needed
            string SQL = "SELECT COUNT(*) FROM TaxonSynonymySource S WHERE S.SourceName LIKE '" + View + "%'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result != "0" && Result != "")
            {
                int i = int.Parse(Result);
                View += "_" + (i + 1).ToString();
            }
            string PrefixDB = "";
            if (ServerConn.LinkedServer.Length > 0)
                PrefixDB = "[" + ServerConn.LinkedServer + "].";
            PrefixDB += ServerConn.DatabaseName + ".dbo.";
            string Database = ServerConn.DatabaseName;
            SQL = "SELECT U.BaseURL FROM " + PrefixDB + "ViewBaseURL AS U";
            string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (BaseURL.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("This database is not prepared for usage by cache databases. Turn to your administrator for an update");
                return "";
            }

            // Check if the view allready exists and remove it after OK
            SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return "";
                    }
                }
                else
                    return "";
            }

#region SQL for view

            SQL = this.SqlViewTaxonNames(View, BaseURL, PrefixDB, ProjectID);
            //"CREATE VIEW [dbo].[" + View + "] " +
            //    "AS " +
            //    "SELECT TOP 100 PERCENT " +
            //    "T.NameID, " +
            //    "'" + BaseURL + "' AS BaseURL,  " +
            //    "T.TaxonNameCache AS TaxonName,  " +
            //    "T.NameID AS AcceptedNameID,  " +
            //    "T.TaxonNameCache AS AcceptedName,  " +
            //    "T.TaxonomicRank,  " +
            //    "T.GenusOrSupragenericName,  " +
            //    "T.SpeciesGenusNameID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor,  " +
            //    "A.ProjectID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T.LogUpdatedWhen " +
            //    "FROM            " + PrefixDB + "TaxonName T INNER JOIN " +
            //    "" + PrefixDB + "TaxonAcceptedName A ON T.NameID = A.NameID " +
            //    "WHERE        (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " " +

            //    "UNION " +

            //    "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
            //    "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
            //    "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
            //    "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
            //    "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
            //    "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
            //    "FROM            " + PrefixDB + "TaxonSynonymy AS S INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T ON S.SynNameID = T.NameID INNER JOIN " +
            //    "" + PrefixDB + "TaxonAcceptedName AS A ON T.NameID = A.NameID AND  " +
            //    "S.ProjectID = A.ProjectID INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID " +
            //    "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) " +
            //    "AND A.ProjectID = " + ProjectID.ToString() + " " +

            //    "UNION " +

            //    "SELECT        TOP 100 PERCENT T.NameID, '" + BaseURL + "' AS BaseURL, T.TaxonNameCache AS TaxonName,  " +
            //    "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T.TaxonomicRank, T.GenusOrSupragenericName, T.SpeciesGenusNameID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, NULL,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T.LogUpdatedWhen " +
            //    "FROM            " + PrefixDB + "TaxonName T " +
            //    ",  " + PrefixDB + "TaxonNameProject P " +
            //    "WHERE        T.IgnoreButKeepForReference = 0 AND T.NameID NOT IN " +
            //    "(SELECT        NameID " +
            //    "FROM            " + PrefixDB + "TaxonSynonymy) AND T.NameID NOT IN " +
            //    "(SELECT        NameID " +
            //    "FROM            " + PrefixDB + "TaxonAcceptedName) AND T.NameID NOT IN " +
            //    "(SELECT        SynNameID " +
            //    "FROM            " + PrefixDB + "TaxonSynonymy) " +
            //    "AND P.NameID = T.NameID AND P.ProjectID = " + ProjectID.ToString() + " " +

            //    "UNION " +

            //    "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
            //    "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
            //    "FROM            " + PrefixDB + "TaxonAcceptedName AS A INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T ON A.NameID = T.NameID INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S1 ON T.NameID = S1.SynNameID AND  " +
            //    "A.ProjectID = S1.ProjectID INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID " +
            //    "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
            //    "A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString() + " AND S1.ProjectID = " + ProjectID.ToString() + " " +

            //    "UNION " +

            //    "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
            //    "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
            //    "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
            //    "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
            //    "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
            //    "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
            //    "FROM            " + PrefixDB + "TaxonSynonymy AS S2 INNER JOIN " +
            //    "" + PrefixDB + "TaxonAcceptedName AS A INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T ON A.NameID = T.NameID ON S2.SynNameID = T.NameID AND  " +
            //    "S2.ProjectID = A.ProjectID INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND  " +
            //    "S.ProjectID = S1.ProjectID INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND  " +
            //    "S2.ProjectID = S1.ProjectID " +
            //    "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
            //    "(S2.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString() + " AND S1.ProjectID = " + ProjectID.ToString() + " AND S2.ProjectID = " + ProjectID.ToString() + " " +

            //    "UNION " +

            //    "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
            //    "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
            //    "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
            //    "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
            //    "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
            //    "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
            //    "FROM            " + PrefixDB + "TaxonSynonymy AS S3 INNER JOIN " +
            //    "" + PrefixDB + "TaxonAcceptedName AS A INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T ON A.NameID = T.NameID ON S3.SynNameID = T.NameID AND  " +
            //    "S3.ProjectID = A.ProjectID INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S2 INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND  " +
            //    "S.ProjectID = S1.ProjectID INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND  " +
            //    "S2.ProjectID = S1.ProjectID ON S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
            //    "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
            //    "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " AND S1.ProjectID = " + ProjectID.ToString() + " AND S2.ProjectID = " + ProjectID.ToString() + " AND  " +
            //    "S3.ProjectID = " + ProjectID.ToString() + " " +

            //    "UNION " +

            //    "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
            //    "T1.NameID AS AcceptedNameID, T1.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
            //    "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
            //    "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
            //    "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
            //    "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
            //    "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
            //    "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
            //    "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            //    "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            //    "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            //    "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
            //    "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            //    "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
            //    "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
            //    "FROM            " + PrefixDB + "TaxonSynonymy AS S1 INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T1 ON S1.NameID = T1.NameID INNER JOIN " +
            //    "" + PrefixDB + "TaxonSynonymy AS S INNER JOIN " +
            //    "" + PrefixDB + "TaxonName AS T ON S.SynNameID = T.NameID INNER JOIN " +
            //    "" + PrefixDB + "TaxonAcceptedName AS A ON T.NameID = A.NameID AND S.ProjectID = A.ProjectID ON  " +
            //    "S1.SynNameID = S.NameID AND S1.ProjectID = S.ProjectID " +
            //    "WHERE        (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'duplicate') OR " +
            //    "(T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym') AND  " +
            //    "A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString() + " AND S1.ProjectID = " + ProjectID.ToString() +
            //    " ORDER BY TaxonName, AcceptedName";

#endregion

            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    if (this.CreateTaxonListAnalysisSource(PrefixDB, View, BaseURL, ref Message) &&
                        this.CreateTaxonListAnalysisCategorySource(PrefixDB, View, BaseURL, ref Message) &&
                        this.CreateTaxonListAnalysisCategoryValueSource(PrefixDB, View, BaseURL, ref Message) &&
                        this.CreateListSource(PrefixDB, View, BaseURL, ref Message) &&
                        this.CreateTaxonCommonNameSource(PrefixDB, View, BaseURL, ref Message) &&
                        this.CreateTaxonHierarchySource(PrefixDB, View, ProjectID, BaseURL, ref Message) &&
                        this.CreateTaxonNameExternalDatabaseSource(PrefixDB, View, BaseURL, ref Message) &&
                        this.CreateTaxonNameExternalIDSource(PrefixDB, View, BaseURL, ref Message))
                        return View;
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Creation of source failed:\r\n" + Message + "\r\nPlease check source database for updates");
                        return "";
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Message);
                    return "";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Message);
                return "";
            }
        }

        /// <summary>
        /// Auxillary view for taxononmy
        /// </summary>
        /// <param name="PrefixDB">Name of the database, may include linked server as first part</param>
        /// <param name="View">name of the main view</param>
        /// <param name="ProjectID">ID of the project</param>
        /// <returns></returns>
        private bool CreateTaxonHierarchySource(string PrefixDB, string View, int ProjectID, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_H";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, T.NameID, T.NameParentID, T.LogUpdatedWhen " +
            "FROM " + PrefixDB + "TaxonHierarchy T " +
            "WHERE (T.IgnoreButKeepForReference = 0) " +
            "AND T.ProjectID = " + ProjectID.ToString() + " AND NOT T.NameParentID IS NULL";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool CreateTaxonCommonNameSource(string PrefixDB, string View, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_C";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            // Markus 27.3.2025: inclusion of LogUpdatedWhen, ReferenceTitle
            //"SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, NameID, CommonName, LanguageCode, CountryCode, MAX(LogUpdatedWhen) AS LogUpdatedWhen " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, NameID, CommonName, LanguageCode, CountryCode, LogUpdatedWhen, ReferenceTitle " +
            "FROM " + PrefixDB + "TaxonCommonName T ";// +
            //"GROUP BY NameID, LanguageCode, CountryCode, CommonName";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool CreateListSource(string PrefixDB, string View, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_L";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, T.ProjectID, T.Project, T.DisplayText " +
            "FROM " + PrefixDB + "TaxonNameListProjectProxy T ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool CreateTaxonListAnalysisSource(string PrefixDB, string View, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_LA";
            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, T.NameID, T.ProjectID, T.AnalysisID, MIN(T.AnalysisValue) AS AnalysisValue, MIN(T.Notes) AS Notes, MAX(T.LogUpdatedWhen) AS LogUpdatedWhen " +
            "FROM " + PrefixDB + "TaxonNameListAnalysis T, " + PrefixDB + "TaxonNameListAnalysisCategory C WHERE T.AnalysisID = C.AnalysisID AND (C.DataWithholdingReason = '' OR C.DataWithholdingReason IS NULL) " +
            "GROUP BY T.NameID, T.ProjectID, T.AnalysisID";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool CreateTaxonListAnalysisCategorySource(string PrefixDB, string View, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_LAC";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, T.AnalysisID, T.AnalysisParentID, T.DisplayText, T.Description, AnalysisURI, ReferenceTitle, ReferenceURI, SortingID, T.LogUpdatedWhen " +
            "FROM " + PrefixDB + "TaxonNameListAnalysisCategory T ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool CreateTaxonListAnalysisCategoryValueSource(string PrefixDB, string View, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_LACV";

            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }

            SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, T.AnalysisID, T.AnalysisValue, T.Description, T.DisplayText, T.DisplayOrder, T.Notes, T.LogUpdatedWhen " +
            "FROM " + PrefixDB + "TaxonNameListAnalysisCategoryValue T ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool CreateTaxonNameExternalDatabaseSource(string PrefixDB, string View, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_E";
            // Check if previous version exists
            if (!this.RemovePreviousViewVersion(View))
                return false;

            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, " +
            "ExternalDatabaseURI, ExternalDatabaseInstitution, ExternalAttribute_NameID, LogUpdatedWhen " +
            "FROM " + PrefixDB + "TaxonNameExternalDatabase T " +
            "WHERE T.Disabled IS NULL";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool CreateTaxonNameExternalIDSource(string PrefixDB, string View, string BaseURL, ref string Message)
        {
            bool OK = true;
            View += "_EID";

            // Check if previous version exists
            if (!this.RemovePreviousViewVersion(View))
                return false;

            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT '" + BaseURL + "' AS BaseURL, NameID, ExternalDatabaseID, ExternalNameURI, LogUpdatedWhen " +
            "FROM " + PrefixDB + "TaxonNameExternalID T ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = true;
                else OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private bool RemovePreviousViewVersion(string View)
        {
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    SQL = "DROP VIEW [dbo].[" + View + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        System.Windows.Forms.MessageBox.Show("Old view removed");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                        return false;
                    }
                }
                else
                    return false;
            }
            return true;
        }

#endregion

        //private string _SourceTransferDirectory;
        //private string _BashFile;

        private void initPostgresControlsForSourceTransfer()
        {
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                {
                    this.textBoxPostgresTransferDirectory.Text = DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory; // _SourceTransferDirectory;
                    this.textBoxPostgresBashFile.Text = DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile;// _BashFile;
                    this.textBoxPostgresMountPoint.Text = DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint;// _BashFile;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initPostgresControlsForTaxonomy()
        {
            if (this._I_Taxonomies != null)
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource T in this._I_Taxonomies)
                    T.initPostgresControls();
            }
            if (this._I_TaxonomyWebservices != null)
            {
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource T in this._I_TaxonomyWebservices)
                    T.initPostgresControls();
            }
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                this.initMissingSourceViews("TaxonSynonymySource", this.panelTaxonSources, false);
                this.initMissingSourceViews("TaxonSynonymySource", this.panelTaxonWebservice, true);
            }

        }

        private void toolStripButtonTaxonSourcesTransfer_Click(object sender, EventArgs e)
        {
            this.TransferTaxonomySources(true, false);
        }

        private void toolStripButtonTaxonSourcesTransferToPostgres_Click(object sender, EventArgs e)
        {
            this.TransferTaxonomySources(false, true);
        }

        private string TransferTaxonomySources(bool TransferFromSourceToCache, bool TransferFromCacheToPostgres)
        {
            string Report = this.TransferSources(/*ProcessOnly,*/ TransferFromSourceToCache, TransferFromCacheToPostgres, this._I_Taxonomies, "Taxa");//, this._FilterTaxaForUpdate);// "";
            return Report;
        }

        private void toolStripButtonTaxonSourceFilter_Click(object sender, EventArgs e)
        {
            if (this._FilterTaxaForUpdate)
            {
                this.toolStripButtonTaxonSourceFilter.Text = "Data not filtered for updates";
                this.toolStripButtonTaxonSourceFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                this.toolStripButtonTaxonSourceFilter.Text = "Data filtered for updates later than last transfer";
                this.toolStripButtonTaxonSourceFilter.Image = DiversityCollection.Resource.Filter;
            }
            this._FilterTaxaForUpdate = !this._FilterTaxaForUpdate;
        }

        private void toolStripButtonTaxonSearchSources_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> PotentialSourceLinks = new Dictionary<string, List<string>>();
            System.Collections.Generic.List<string> LinkColumns = new List<string>();
            LinkColumns.Add("NameURI");
            PotentialSourceLinks.Add("CacheIdentification", LinkColumns);
            this.SearchForSources("TaxonName", PotentialSourceLinks);
        }

#endregion

#region Taxa - webservices

        public bool initTaxonWebservices()
        {
            this._I_TaxonomyWebservices = new List<InterfaceLookupSource>();
            bool OK = this.initSources(ref this._I_TaxonomyWebservices, this.panelTaxonWebservice, this.toolStripTaxonWebservices, this.dataSetCacheDatabase.TaxonSynonymySource_Webservice, this._SqlDataAdapterTaxonSynonymySource, UserControlLookupSource.TypeOfSource.Taxa, "TaxonSynonymySource", "Cache database taxonomy", true);
            return OK;

            string SQL = "SELECT COUNT(*) " +
                    "FROM TaxonSynonymySource ";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                this.toolStripTaxonWebservices.Enabled = true;
                try
                {
                    // remove previous controls
                    System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                    foreach (System.Windows.Forms.Control C in this.panelTaxonWebservice.Controls)
                    {
                        CCtoDelete.Add(C);
                    }
                    this.panelTaxonWebservice.Controls.Clear();
                    foreach (System.Windows.Forms.Control C in CCtoDelete)
                    {
                        C.Dispose();
                    }

                    // get data for current controls
                    this.dataSetCacheDatabase.TaxonSynonymySource_Webservice.Clear();
                    SQL = "SELECT SourceView, Source " + 
                        "FROM TaxonSynonymySource WHERE SourceID IS NULL ";
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        SQL += " AND IncludeInTransfer = 1 ";
                    SQL += "ORDER BY SourceView ";
                    if (this._SqlDataAdapterTaxonSynonymySource == null)
                        this._SqlDataAdapterTaxonSynonymySource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    else
                        this._SqlDataAdapterTaxonSynonymySource.SelectCommand.CommandText = SQL;
                    this._SqlDataAdapterTaxonSynonymySource.Fill(this.dataSetCacheDatabase.TaxonSynonymySource_Webservice);

                    // insert current controls
                    foreach (System.Data.DataRow R in this.dataSetCacheDatabase.TaxonSynonymySource_Webservice.Rows)
                    {
                        DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, UserControlLookupSource.TypeOfSource.Taxa);
                        U.Dock = DockStyle.Top;
                        this.panelTaxonWebservice.Controls.Add(U);
                        U.BringToFront();
                        U.setHelp("Cache database taxonomy");
                        this._I_TaxonomyWebservices.Add(U);
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
            }
            else
            {
                OK = false;
                this.toolStripTaxonWebservices.Enabled = false;
            }
            return OK;
        }


        private void toolStripButtonTaxonWebservicesAdd_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> SourceList = DiversityCollection.CacheDatabase.UserControlLookupSource.TaxaWebserviceSourceList();
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string View = f.SelectedString;
                string Source = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].AdditionalServicesOfModule()[f.SelectedString];
                string SQL = "INSERT INTO TaxonSynonymySource (SourceView, Source) " +
                "VALUES ('" + View + "', '" + Source + "')";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.initTaxonWebservices();
            }
        }

#endregion

#region Common

        private bool initSources(ref List<InterfaceLookupSource> InterfaceList,
            System.Windows.Forms.Panel Panel,
            System.Windows.Forms.ToolStrip Toolstrip,
            System.Data.DataTable Table,
            Microsoft.Data.SqlClient.SqlDataAdapter Adapter,
            UserControlLookupSource.TypeOfSource Type,
            string TableName = "",
            string HelpIndexEntry = "",
            bool Webservices = false)
        {
            bool OK = true;
            try
            {
                if (TableName.Length == 0)
                    TableName = Table.TableName;
                if (HelpIndexEntry.Length == 0)
                {
                    HelpIndexEntry = "Cache database sources";
                }
                HelpIndexEntry = "Cache " + TableName.Replace("Source", "").ToLower() + "s";
                InterfaceList = new List<InterfaceLookupSource>();
                // Check existance of tables
                string SqlCheck = "select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = '" + TableName + "'";
                string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCheck);
                if (Check == "0")
                    return false;
                string SQL = "SELECT COUNT(*) " +
                        "FROM " + TableName;
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    Toolstrip.Enabled = true;
                    try
                    {
                        Table.Clear();
                        System.Collections.Generic.List<string> PresentSourceViews = new List<string>();
                        SQL = "SELECT SourceView, Source, SourceID, LinkedServerName, DatabaseName " +
                            "FROM " + TableName + " WHERE 1 = 1 ";
                        if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                            SQL += " AND IncludeInTransfer = 1 ";
                        if (Webservices)
                            SQL += " AND SourceID IS NULL ";
                        else
                            SQL += " AND NOT SourceID IS NULL ";
                        SQL += " ORDER BY SourceView ";
                        if (Adapter == null)
                            Adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                        else
                            Adapter.SelectCommand.CommandText = SQL;
                        Adapter.Fill(Table);
                        System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                        foreach (System.Windows.Forms.Control C in Panel.Controls)
                        {
                            CCtoDelete.Add(C);
                        }
                        foreach (System.Windows.Forms.Control C in Panel.Controls)
                            C.Dispose();
                        Panel.Controls.Clear();
                        foreach (System.Windows.Forms.Control C in CCtoDelete)
                        {
                            C.Dispose();
                        }
                        foreach (System.Data.DataRow R in Table.Rows)
                        {
                            DiversityCollection.CacheDatabase.UserControlLookupSource U = new UserControlLookupSource(R, this, Type);
                            U.Dock = DockStyle.Top;
                            Panel.Controls.Add(U);
                            U.BringToFront();
                            U.setHelp(HelpIndexEntry);
                            InterfaceList.Add(U);
                            PresentSourceViews.Add(R["SourceView"].ToString());
                        }
                        if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && PresentSourceViews.Count > 0 && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                        {
                            this.AddMissingSourceViews(PresentSourceViews, TableName, Panel, Webservices);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        OK = false;
                    }
                }
                else
                {
                    OK = false;
                    Toolstrip.Enabled = false;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void initMissingSourceViews(string TableName, System.Windows.Forms.Panel Panel, bool Webservices)
        {
            try
            {
                //if (this.buttonUpdateDatabase)
                System.Collections.Generic.List<System.Windows.Forms.Control> ToRemove = new List<System.Windows.Forms.Control>();
                foreach (System.Windows.Forms.Control c in Panel.Controls)
                {
                    if (c.GetType() == typeof(UserControlLookupSourceMissing))
                    {
                        ToRemove.Add(c);
                    }
                }
                foreach (System.Windows.Forms.Control c in ToRemove)
                {
                    Panel.Controls.Remove(c);
                    c.Dispose();
                }
                System.Collections.Generic.List<string> PresentSourceViews = new List<string>();
                string SQL = "SELECT SourceView " +
                    "FROM " + TableName + " WHERE ";
                if (Webservices)
                    SQL += " SourceID IS NULL ";
                else
                    SQL += " NOT SourceID IS NULL ";
                SQL += " ORDER BY SourceView ";
                System.Data.DataTable Table = new DataTable();
                string Message = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref Table, ref Message);
                foreach (System.Data.DataRow R in Table.Rows)
                {
                    PresentSourceViews.Add(R["SourceView"].ToString());
                }
                this.AddMissingSourceViews(PresentSourceViews, TableName, Panel, Webservices);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void AddMissingSourceViews(System.Collections.Generic.List<string> PresentSourceViews, string TableName, System.Windows.Forms.Panel Panel, bool Webservices)
        {
            TableName = TableName.Replace("Source", "");
            if (!DiversityCollection.CacheDatabase.CacheDB.ObjectExistsInDatabase(TableName))
            {
                return;
            }
            string WebDecisionColumn = "";
            switch(TableName)
            {
                case "TaxonSynonymy":
                    WebDecisionColumn = "NameID";
                    break;
            }
            string SQL = "SELECT \"SourceView\", COUNT(*) " +
                "FROM \"" + TableName + "\" WHERE 1 = 1 ";
            if (PresentSourceViews.Count > 0)
            {
                SQL += " AND \"SourceView\" NOT IN (";
                foreach(string S in PresentSourceViews)
                {
                    if (!SQL.EndsWith("("))
                        SQL += ", ";
                    SQL += "'" + S + "'";
                }
                SQL += ") ";
            }
            if (WebDecisionColumn.Length > 0)
            {
                if (Webservices)
                    SQL += " AND \"" + WebDecisionColumn + "\" = -1 ";
                else
                    SQL += " AND \"" + WebDecisionColumn + "\" > -1 ";
            }
            SQL +=   "GROUP BY \"SourceView\" ORDER BY \"SourceView\"; ";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
            int c = 0;
            foreach(System.Data.DataRow R in dt.Rows)
            {
                int i;
                if (int.TryParse(R[1].ToString(), out i) && i > 0)
                {
                    DiversityCollection.CacheDatabase.UserControlLookupSourceMissing U = new UserControlLookupSourceMissing(TableName, R[0].ToString());
                    U.Dock = DockStyle.Top;
                    Panel.Controls.Add(U);
                    if (c == 0)
                        U.setHeader("Sourceviews not present in the SQL-Server database");
                    U.BringToFront();
                    c++;
                }
            }
        }

        public string AddSource(InterfaceLookupSource LookupSource, System.Collections.Generic.Dictionary<string, string> SourceParameters)
        {
            // Parameter
            string TableSource = this.RecreationTable(LookupSource.SourceType()) + "Source";
            string SourceView = "";
            string Project = "";
            string Database = "";
            string LinkedServer = "";
            int ProjectID = 0;

            // Getting the parameters
            bool ParametersComplete = false;
            if (SourceParameters != null)
            {
                try
                {
                    if (SourceParameters.ContainsKey("Source"))
                    {
                        SourceView = SourceParameters["Source"];
                        if (SourceParameters.ContainsKey("Database"))
                        {
                            Database = SourceParameters["Database"];
                            if (SourceParameters.ContainsKey("LinkedServer"))
                                LinkedServer = SourceParameters["LinkedServer"];
                            if (SourceParameters.ContainsKey("Project"))
                            {
                                Project = SourceParameters["Project"];
                                if (SourceParameters.ContainsKey("ProjectID"))
                                {
                                    if (int.TryParse(SourceParameters["ProjectID"], out ProjectID))
                                    {
                                        ParametersComplete = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch(System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            if (!ParametersComplete)
            {
                LookupSource.setState(UserControlLookupSource.Stati.SearchingSource, "Getting parameters failed");
                return "";
            }

            // Getting the server connection
            DiversityWorkbench.ServerConnection S = null;

            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["Diversity" + this.RecreationModule(LookupSource.SourceType()).ToString()].ServerConnections())
            {
                if (KV.Value.DatabaseName == Database &&
                    KV.Value.LinkedServer == LinkedServer)
                {
                    S = KV.Value;
                    if (LookupSource != null)
                        LookupSource.setState(UserControlLookupSource.Stati.SearchingSource, S.DatabaseName);
                    break;
                }
            }

            if (S == null)
            {
                LookupSource.setState(UserControlLookupSource.Stati.SearchingSource, "Could not detect source server");
                return "";
            }
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables = new Dictionary<UserControlLookupSource.SubsetTable, string>();
            DataTables.Add(this.RecreationTable(LookupSource.SourceType()), "");
            foreach (System.Collections.Generic.KeyValuePair<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> KV in LookupSource.SubsetTables())
                DataTables.Add(KV.Key, KV.Value);
            SourceView = this.CreateSourceViews(S, this.RecreationTable(LookupSource.SourceType()), DataTables, Project, ProjectID, true);

            if (SourceView.Length > 0)
            {
                if (!this.AddSourceToSourceTable(this.RecreationTable(LookupSource.SourceType()), SourceView, Project, ProjectID, S, LookupSource.SourceType()))
                    LookupSource.setState(UserControlLookupSource.Stati.RecreationFailed, "Writing to " + TableSource + " failed");
            }
            else
                LookupSource.setState(UserControlLookupSource.Stati.RecreationFailed, "Creation of view failed");

            //OK = this.AddSource(this.RecreationModule(LookupSource.SourceType()), this.RecreationTable(LookupSource.SourceType()), LookupSource.SubsetTables(), LookupSource.SourceType(), LookupSource, SourceParameters);
            return SourceView;
        }

        private DiversityWorkbench.WorkbenchUnit.ModuleType RecreationModule(UserControlLookupSource.TypeOfSource SourceType)
        {
            DiversityWorkbench.WorkbenchUnit.ModuleType M = DiversityWorkbench.WorkbenchUnit.ModuleType.Agents;
            switch (SourceType)
            {
                case UserControlLookupSource.TypeOfSource.Descriptions:
                    M = DiversityWorkbench.WorkbenchUnit.ModuleType.Descriptions;
                    break;
                case UserControlLookupSource.TypeOfSource.Gazetteer:
                    M = DiversityWorkbench.WorkbenchUnit.ModuleType.Gazetteer;
                    break;
                case UserControlLookupSource.TypeOfSource.Plots:
                    M = DiversityWorkbench.WorkbenchUnit.ModuleType.SamplingPlots;
                    break;
                case UserControlLookupSource.TypeOfSource.References:
                    M = DiversityWorkbench.WorkbenchUnit.ModuleType.References;
                    break;
                case UserControlLookupSource.TypeOfSource.ScientificTerms:
                    M = DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms;
                    break;
                case UserControlLookupSource.TypeOfSource.Taxa:
                    M = DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames;
                    break;
            }
            return M;
        }

        private DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable RecreationTable(UserControlLookupSource.TypeOfSource SourceType)
        {
            DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable T = UserControlLookupSource.SubsetTable.Agent;
            switch (SourceType)
            {
                case UserControlLookupSource.TypeOfSource.Descriptions:
                    T = UserControlLookupSource.SubsetTable.Description;
                    break;
                case UserControlLookupSource.TypeOfSource.Gazetteer:
                    T = UserControlLookupSource.SubsetTable.Gazetteer;
                    break;
                case UserControlLookupSource.TypeOfSource.Plots:
                    T = UserControlLookupSource.SubsetTable.SamplingPlot;
                    break;
                case UserControlLookupSource.TypeOfSource.References:
                    T = UserControlLookupSource.SubsetTable.ReferenceTitle;
                    break;
                case UserControlLookupSource.TypeOfSource.ScientificTerms:
                    T = UserControlLookupSource.SubsetTable.ScientificTerm;
                    break;
                case UserControlLookupSource.TypeOfSource.Taxa:
                    T = UserControlLookupSource.SubsetTable.TaxonSynonymy;
                    break;
            }
            return T;
        }

        private bool AddSource(DiversityWorkbench.WorkbenchUnit.ModuleType Module, 
            DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable Table, 
            System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables, 
            UserControlLookupSource.TypeOfSource SourceType
            )
        {
            bool OK = false;
            string SQL = "";
            // Parameter
            string TableSource = Table.ToString() + "Source";
            string Source = "";
            string Project = "";
            int ProjectID = 0;

            DiversityWorkbench.ServerConnection S = null;

            // getting the source list
            System.Collections.Generic.List<string> SourceList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["Diversity" + Module.ToString()].ServerConnections())
            {
                if (KV.Value.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer)
                    continue; // these are sources that had been linked in manually and so far are not prepared for use as cache db sources
                SourceList.Add(KV.Value.DisplayText);
            }

            // Check existence of table
            if (!CacheDB.ObjectExistsInDatabase(TableSource))
            {
                System.Windows.Forms.MessageBox.Show("Table " + TableSource + " is missing.\r\nMissing update?", "Missing table", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return OK;
            }

            if (SourceList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("So far no sources are available. Turn to your administrator to include sources e.g. via linked server");
                return false;
            }

            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(SourceList, "Source", "Please select a source from the list", true);
            f.ShowDialog();
            if (f.DialogResult != System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                return OK;
            else
            {
                Source = f.String;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["Diversity" + Module.ToString()].ServerConnections())
                {
                    if (KV.Value.DisplayText == Source)
                    {
                        S = KV.Value;
                        break;
                    }
                }
                if (S != null)
                {
                    string PrefixDB = "";
                    if (S.LinkedServer.Length > 0)
                        PrefixDB = "[" + S.LinkedServer + "].";
                    PrefixDB += S.DatabaseName + ".dbo.";
                    // Check existence of Project source
                    // Markus 11.3.2021: von ...List auf ...Proxy geaendert
                    string ProjectSource = "ProjectProxy";
                    System.Collections.Generic.List<string> potentialSoures = new List<string>();
                    potentialSoures.Add("ProjectList");
                    potentialSoures.Add("ProjectProxy");
                    potentialSoures.Add("Project");
                    foreach (string P in potentialSoures)
                    {
                        SQL = "select count(*) from " + PrefixDB.Replace(".dbo", "") + "INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = '" + P + "'";
                        string pp = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (pp != "0")
                        {
                            ProjectSource = P;
                            break;
                        }
                    }
                    System.Data.DataTable dtProject = new DataTable();
                    SQL = "SELECT ProjectID, Project FROM " + PrefixDB + ProjectSource + " ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
                    ad.Fill(dtProject);
                    DiversityWorkbench.Forms.FormGetStringFromList fProject = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Please select the project");
                    fProject.ShowDialog();
                    if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (int.TryParse(fProject.SelectedValue, out ProjectID))
                        {
                            Project = fProject.SelectedString;
                        }
                        else return OK;
                    }
                    else return OK;
                }
                else return OK;
            }
            string View = this.CreateSourceViews(S, Table, DataTables, Project, ProjectID, false);
            if (View.Length > 0)
            {
                OK = this.AddSourceToSourceTable(Table, View, Project, ProjectID, S, SourceType);
            }
            return OK;
        }

        private bool AddSourceToSourceTable(DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable Table, string View, string Project, int ProjectID, DiversityWorkbench.ServerConnection S, UserControlLookupSource.TypeOfSource SourceType)
        {
            bool OK = false;
            string Version = DiversityCollection.CacheDatabase.UserControlLookupSource.LookupSourceVersion[SourceType].ToString();
            if (Version.Length == 0)
                Version = "NULL";
            string TableSource = Table.ToString() + "Source";
            string SQL = "INSERT INTO " + TableSource + " (SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets, Version) " +
                "SELECT '" + View + "', '" + Project + "', " + ProjectID.ToString() + ", '" + S.LinkedServer + "', '" + S.DatabaseName + "' " +
                ", '" + this.SourceSubsets(Table) + "' " +
                ", " + Version +
                " WHERE NOT EXISTS (SELECT * FROM " + TableSource + " WHERE SourceView = '" + View + "')";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                OK = true;
            }
            return OK;
        }

        private string CreateSourceViews(DiversityWorkbench.ServerConnection ServerConn, DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable SourceTable, System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> DataTables, string Project, int ProjectID, bool ForRecreation)
        {
            string View = ServerConn.DatabaseName.Replace("Diversity", "") + "_";
            if (ServerConn.LinkedServer.Length > 0)
            {
                string LinkedServerMarker = ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
                if (!View.EndsWith("_" + LinkedServerMarker))
                    View += ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
            }
            View += Project;
            View = this.ConvertToSqlName(View);
            // Check if there are sources in table ...Source with the same name and change name if needed
            string SQL = "SELECT COUNT(*) FROM " + SourceTable.ToString() + " S WHERE S.SourceName LIKE '" + View + "%'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result != "0" && Result != "")
            {
                int i = int.Parse(Result);
                View += "_" + (i + 1).ToString();
            }
            string PrefixDB = "";
            if (ServerConn.LinkedServer.Length > 0)
                PrefixDB = "[" + ServerConn.LinkedServer + "].";
            PrefixDB += ServerConn.DatabaseName + ".dbo.";
            string Database = ServerConn.DatabaseName;
            SQL = "SELECT U.BaseURL FROM " + PrefixDB + "ViewBaseURL AS U";
            string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (BaseURL.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("This database is not prepared for usage by cache databases. Turn to your administrator for an update");
                return "";
            }

            bool OK = true;
            System.Collections.Generic.List<string> Views = new List<string>();
            foreach(System.Collections.Generic.KeyValuePair<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> KV in DataTables)
            {
                if(this.CreateSourceView(View + KV.Value, KV.Key, PrefixDB, ProjectID, ForRecreation))
                {
                    if (KV.Key.ToString().StartsWith("proc"))
                        Views.Add(KV.Key.ToString());
                    else
                    {
                        SQL = "SELECT COUNT(*) FROM " + View + KV.Value;
                        // Markus 11.10.2021: Checking exsistance - if Source database is out of date the creation may fail
                        bool Exists = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                        if (!Exists)
                        {
                            continue;
                        }
                        Views.Add(View + KV.Value);
                    }
                }
                else
                {
                    OK = false;
                    foreach(string V in Views)
                    {
                        SQL = "DROP VIEW " + V;
                        if (V.StartsWith("proc"))
                            SQL = "DROP PROCEDURE " + V;
                        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    }
                    View = "";
                    break;
                }
            }
            return View;




            //// Check if the view allready exists and remove it after OK
            //SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            //string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            //if (Check != "0")
            //{
            //    if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            //    {
            //        SQL = "DROP VIEW [dbo].[" + View + "]";
            //        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            //            System.Windows.Forms.MessageBox.Show("Old view removed");
            //        else
            //        {
            //            System.Windows.Forms.MessageBox.Show("Removal failed");
            //            return "";
            //        }
            //    }
            //    else
            //        return "";
            //}
            //SQL = "CREATE VIEW [dbo].[" + View + "] " +
            //    "AS " +
            //    "SELECT B.BaseURL, A.AgentID, A.AgentParentID, A.AgentName, A.AgentTitle, A.GivenName, A.GivenNamePostfix, " +
            //    "A.InheritedNamePrefix, A.InheritedName, A.InheritedNamePostfix, A.Abbreviation, A.AgentType, A.AgentRole, A.AgentGender, " +
            //    "A.Description, A.OriginalSpelling, A.Notes, A.ValidFromDate, A.ValidUntilDate, A.SynonymToAgentID, P.ProjectID, A.LogUpdatedWhen " +
            //    "FROM " + PrefixDB + "PublicAgent AS A, " +
            //    PrefixDB + "AgentProject AS P, " +
            //    PrefixDB + "ViewBaseURL AS B " +
            //    "WHERE A.AgentID = P.AgentID AND P.ProjectID = " + ProjectID.ToString();

            //string Message = "";
            //if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            //{
            //    SQL = "GRANT SELECT ON " + View + " TO CacheUser";
            //    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            //    {
            //        if (this.CreateAgentContactSource(PrefixDB, View, ProjectID))
            //        {
            //            if (this.CreateAgentImageSource(PrefixDB, View, ProjectID))
            //            {
            //                return View;
            //            }
            //            else
            //                return "";
            //        }
            //        else
            //            return "";
            //    }
            //    else
            //        return "";
            //}
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show(Message);
            //    return "";
            //}
        }

        private bool CreateSourceView(string View, DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable SourceTable, string PrefixDB, int ProjectID, bool ForRecreation)
        {
            bool OK = true;

            bool IsProcedure = false;
            if (SourceTable.ToString().StartsWith("proc"))
                IsProcedure = true;


            // Check if previous version exists
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            if (IsProcedure)
                SQL = "select count(*) from INFORMATION_SCHEMA.ROUTINES R where R.SPECIFIC_NAME = '" + SourceTable + "'";
            string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Check != "0")
            {
                if (!ForRecreation)
                {
                    string Message = "The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?";
                    if (IsProcedure)
                        Message = "The procedure\r\n" + SourceTable + "\r\nis allready present. Should it be replaced by the new version?";
                    if (System.Windows.Forms.MessageBox.Show(Message, "Remove previous object", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                        return false;
                }
                SQL = "DROP VIEW [dbo].[" + View + "]";
                if (IsProcedure)
                    SQL = "DROP PROCEDURE [dbo].[" + SourceTable + "]";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    if (ForRecreation)
                    { }
                    else
                        System.Windows.Forms.MessageBox.Show("Old object removed");

                }
                else
                {
                    if (ForRecreation)
                    { }
                    else
                        System.Windows.Forms.MessageBox.Show("Removal failed");
                    return false;
                }
            }

            SQL = this.SqlView(SourceTable, PrefixDB, View, ProjectID);
            // Markus 11.10.2021: Checking exsistance - if Source database is out of date the creation may fail
            string SqlCheck = SQL.Substring(SQL.IndexOf(" AS ") + 4);
            // Markus 8.4.25: Change check for proc #49
            bool SourceExists = true;
            if (!SourceTable.ToString().StartsWith("proc"))
            {
                SourceExists = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SqlCheck);
            }
            if (SourceExists)
            {
                if (SourceExists && DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    if (IsProcedure)
                        SQL = "GRANT EXEC ON " + SourceTable + " TO CacheUser";
                    else
                        SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    {
                        OK = true;
                    }
                    else OK = false;
                }
                else
                    OK = false;
            }
            else
            {
                string Message = "The source for " + SourceTable + " could not be created.\r\n";
                if (!IsProcedure)
                    Message += "The view " + View + " is missing\r\n";
                System.Windows.Forms.MessageBox.Show(Message + "Please check if the database containing the source needs to be updated");
            }
            return OK;
        }


        private string SqlView(DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable Table, string PrefixDB, string View, int ProjectID)
        {
            string SQL = "CREATE VIEW [dbo].[" + View + "] AS ";
            switch(Table)
            {
                case UserControlLookupSource.SubsetTable.Agent:
                    SQL += "SELECT B.BaseURL, A.AgentID, A.AgentParentID, A.AgentName, A.AgentTitle, A.GivenName, A.GivenNamePostfix, " +
                        "A.InheritedNamePrefix, A.InheritedName, A.InheritedNamePostfix, A.Abbreviation, A.AgentType, A.AgentRole, A.AgentGender, " +
                        "A.Description, A.OriginalSpelling, A.Notes, A.ValidFromDate, A.ValidUntilDate, A.SynonymToAgentID, P.ProjectID, A.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "PublicAgent AS A, " +
                        PrefixDB + "AgentProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE A.AgentID = P.AgentID AND P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.AgentContactInformation:
                    SQL += "SELECT B.BaseURL, C.AgentID, C.DisplayOrder, C.AddressType, C.Country, C.City, C.PostalCode, C.Streetaddress, C.Address, C.Telephone, " +
                       "C.CellularPhone, C.Telefax, C.Email, C.URI, C.Notes, C.ValidFrom, C.ValidUntil, C.LogUpdatedWhen " +
                       "FROM " + PrefixDB + "PublicContactInformation AS C, " +
                       PrefixDB + "AgentProject AS P, " +
                       PrefixDB + "ViewBaseURL AS B " +
                       "WHERE C.AgentID = P.AgentID AND P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.AgentImage:
                    SQL += "SELECT B.BaseURL, C.AgentID, C.URI, C.Type, C.Sequence, C.Description, C.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "PublicImage AS C , " +
                        PrefixDB + "AgentProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE C.AgentID = P.AgentID AND P.ProjectID = " + ProjectID.ToString();
                    break;
#if AgentIdentifierIncluded

                case UserControlLookupSource.SubsetTable.AgentIdentifier:
                    SQL += "SELECT B.BaseURL, C.AgentID, SUBSTRING(C.Identifier, 1, 190) AS Identifier, C.IdentifierURI, C.Type, C.Notes, C.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "PublicIdentifier AS C , " +
                        PrefixDB + "AgentProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE C.AgentID = P.AgentID AND P.ProjectID = " + ProjectID.ToString();
                    break;
#endif
                case UserControlLookupSource.SubsetTable.Gazetteer:
                    SQL += "SELECT U.BaseURL, N.NameID, N.Name, N.LanguageCode, N.PlaceID, P.PlaceType, " +
                        "N.Name AS PreferredName, N.NameID AS PreferredNameID, N.LanguageCode AS PreferredNameLanguageCode, N.ExternalNameID, N.ExternalDatabaseID, PN.ProjectID, N.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "GeoName AS N INNER JOIN " +
                        "" + PrefixDB + "GeoProject AS PN ON N.NameID = PN.NameID INNER JOIN " +
                        "" + PrefixDB + "ViewGeoPlace AS P ON N.PlaceID = P.PlaceID /* AND N.NameID = P.PreferredNameID */ CROSS JOIN " +
                        "" + PrefixDB + "ViewBaseURL AS U " +
                        "WHERE PN.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.GazetteerExternalDatabase:
                    SQL += "SELECT ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, ExternalAttribute_NameID, ExternalAttribute_PlaceID, ExternalCoordinatePrecision, InternalNotes " +
                        "FROM " + PrefixDB + "ExternalDatabase";
                    break;
                case UserControlLookupSource.SubsetTable.ReferenceRelator:
                    SQL += "SELECT B.BaseURL, R.RefID, R.Role, R.Sequence, R.Name, R.AgentURI, R.SortLabel, R.Address, T.LogInsertedWhen AS LogUpdatedWhen " +
                        "FROM " + PrefixDB + "ReferenceTitle AS T, " + PrefixDB + "ReferenceRelator AS R , " +
                        PrefixDB + "ReferenceProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE T.RefID = R.RefID AND R.RefID = P.RefID AND P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.ReferenceTitle:
                    SQL += "SELECT B.BaseURL, R.RefType, R.RefID, R.RefDescription_Cache, R.Title, R.DateYear, R.DateMonth, R.DateDay, R.DateSuppl, R.SourceTitle, R.SeriesTitle, R.Periodical, R.Volume, R.Issue, R.Pages, R.Publisher, " +
                        "R.PublPlace, R.Edition, R.DateYear2, R.DateMonth2, R.DateDay2, R.DateSuppl2, R.ISSN_ISBN, R.Miscellaneous1, R.Miscellaneous2, R.Miscellaneous3, R.UserDef1, R.UserDef2, R.UserDef3, R.UserDef4,  " +
                        "R.UserDef5, R.WebLinks, R.LinkToPDF, R.LinkToFullText, R.RelatedLinks, R.LinkToImages, R.SourceRefID, R.Language, R.CitationText, R.CitationFrom, R.LogInsertedWhen AS LogUpdatedWhen, P.ProjectID " +
                        "FROM " + PrefixDB + "ReferenceTitle AS R, " +
                        PrefixDB + "ReferenceProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE R.RefID = P.RefID AND P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.SamplingPlot:
                    SQL += "SELECT B.BaseURL,  S.PlotID, S.PartOfPlotID, S.PlotIdentifier, S.PlotGeography_Cache.ToString() AS PlotGeography_Cache, S.PlotDescription, " +
                        "S.PlotType, S.CountryCache, P.ProjectID, S.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "SamplingPlot AS S, " +
                        PrefixDB + "SamplingProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE S.PlotID = P.PlotID AND P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.SamplingPlotLocalisation:
                    SQL += "SELECT B.BaseURL, S.PlotID, S.LocalisationSystemID, S.Location1, S.Location2, S.LocationAccuracy, S.LocationNotes, S.DeterminationDate, S.DistanceToLocation, " +
                        "S.DirectionToLocation, S.ResponsibleName, S.ResponsibleAgentURI, S.Geography.ToString() AS Geography, " +
                        "S.AverageAltitudeCache, S.AverageLatitudeCache, S.AverageLongitudeCache, S.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "SamplingPlotLocalisation AS S, " +
                        PrefixDB + "SamplingProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE S.PlotID = P.PlotID AND P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.SamplingPlotProperty:
                    SQL += "SELECT B.BaseURL, S.PlotID, S.PropertyID, S.PropertyURI, S.DisplayText, S.PropertyHierarchyCache, S.PropertyValue, " +
                        "S.ResponsibleName, S.ResponsibleAgentURI, S.Notes, S.AverageValueCache, S.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "SamplingPlotProperty AS S, " +
                        PrefixDB + "SamplingProject AS P, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE S.PlotID = P.PlotID AND P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.procSamplingPlotLocalisationHierarchy:
                    SQL = "CREATE PROCEDURE[dbo].[procSamplingPlotLocalisationHierarchy] " +
                        " @View nvarchar(50)" +
                        " AS" +
                        " BEGIN" +
                        " /*" +
                        " Adds localisations to plots as defined by the hierarchy" +
                        " exec dbo.procSamplingPlotLocalisationHierarchy 'SamplingPlots_Test_SMNKspiderplots'" +
                        " */" +
                        " SET NOCOUNT ON;" +
                        " declare @i int;" +
                        " set @i = (select count(*)" +
                        " from SamplingPlot S, SamplingPlotLocalisation L" +
                        " where S.PartOfPlotID = L.PlotID and S.SourceView = L.SourceView " +
                        " and S.SourceView = @View" +
                        " and NOT EXISTS(select * from SamplingPlotLocalisation LL" +
                        " where S.PlotID = LL.PlotID and L.LocalisationSystemID = LL.LocalisationSystemID and L.SourceView = LL.SourceView)); " +
                        " while @i > 0" +
                        " begin" +
                        " insert into SamplingPlotLocalisation(PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache, SourceView)" +
                        " select distinct S.PlotID, L.LocalisationSystemID, L.Location1, L.Location2, L.LocationAccuracy, L.LocationNotes, L.Geography, L.AverageAltitudeCache, L.AverageLatitudeCache, L.AverageLongitudeCache, L.SourceView" +
                        "  from SamplingPlot S, SamplingPlotLocalisation L" +
                        "  where L.PlotID = S.PartOfPlotID and S.SourceView = L.SourceView and S.SourceView = @View" +
                        " and NOT EXISTS(select * from SamplingPlotLocalisation LL" +
                        " where S.PlotID = LL.PlotID and L.LocalisationSystemID = LL.LocalisationSystemID and L.SourceView = LL.SourceView)" +
                        " set @i = (select count(*)" +
                        " from SamplingPlot S, SamplingPlotLocalisation L" +
                        " where S.PartOfPlotID = L.PlotID and S.SourceView = L.SourceView " +
                        " and S.SourceView = @View" +
                        " and NOT EXISTS(select * from SamplingPlotLocalisation LL" +
                        " where S.PlotID = LL.PlotID and L.LocalisationSystemID = LL.LocalisationSystemID and L.SourceView = LL.SourceView)) " +
                        " end" +
                        " END";
                    break;
                case UserControlLookupSource.SubsetTable.procSamplingPlotPropertyHierarchy:
                    SQL = "CREATE PROCEDURE [dbo].[procSamplingPlotPropertyHierarchy] " +
                        " @View nvarchar(50)" +
                        " AS" +
                        " BEGIN" +
                        " /*" +
                        " Adds properties to plots as defined by the hierarchy" +
                        " exec dbo.procSamplingPlotPropertyHierarchy 'SamplingPlots_Test_SMNKspiderplots'" +
                        " */" +
                        " SET NOCOUNT ON;" +
                        " declare @i int;" +
                        " set @i = (select count(*)" +
                        " from SamplingPlot S, SamplingPlotProperty P" +
                        " where S.PartOfPlotID = P.PlotID and S.SourceView = P.SourceView" +
                        " and S.SourceView = @View" +
                        " and NOT EXISTS(select * from SamplingPlotProperty PP" +
                        " where S.PlotID = PP.PlotID and P.PropertyID = PP.PropertyID and P.SourceView = PP.SourceView)); " +
                        " while @i > 0" +
                        " begin" +
                        " insert into SamplingPlotProperty(PlotID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, Notes, AverageValueCache, SourceView)" +
                        " select distinct S.PlotID, P.PropertyID, P.DisplayText, P.PropertyURI, P.PropertyHierarchyCache, P.PropertyValue, P.Notes, P.AverageValueCache, P.SourceView" +
                        " from SamplingPlot S, SamplingPlotProperty P" +
                        " where P.PlotID = S.PartOfPlotID and S.SourceView = P.SourceView and S.SourceView = @View" +
                        " and NOT EXISTS(select * from SamplingPlotProperty PP" +
                        " where S.PlotID = PP.PlotID and P.PropertyID = PP.PropertyID and P.SourceView = PP.SourceView)" +
                        " set @i = (select count(*)" +
                        " from SamplingPlot S, SamplingPlotProperty P" +
                        " where S.PartOfPlotID = P.PlotID and S.SourceView = P.SourceView" +
                        " and S.SourceView = @View" +
                        " and NOT EXISTS(select * from SamplingPlotProperty PP" +
                        " where S.PlotID = PP.PlotID and P.PropertyID = PP.PropertyID and P.SourceView = PP.SourceView)) " +
                        " end" +
                        " END";
                    break;
                case UserControlLookupSource.SubsetTable.ScientificTerm:
                    SQL += "SELECT U.BaseURL, R.RepresentationID, U.BaseURL + CAST(R.RepresentationID AS nvarchar(50)) AS RepresentationURI, R.TerminologyID, " +
                        "R.TermID, R.DisplayText, R.Description, R.HierarchyCache, " +
                        "R.HierarchyCacheDown, R.ExternalID, R.Notes, R.LanguageCode, MIN(AR.DisplayText) AS RankingTerm, R.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "TermRepresentation AS AR INNER JOIN " +
                        PrefixDB + "Term AS A ON AR.TermID = A.TermID RIGHT OUTER JOIN " +
                        PrefixDB + "TermRepresentation AS R INNER JOIN " +
                        PrefixDB + "Term AS T ON R.TermID = T.TermID ON A.TermID = T.RankingTermID CROSS JOIN " +
                        PrefixDB + "ViewBaseURL AS U " +
                        "WHERE R.TerminologyID = " + ProjectID.ToString() + " AND T.IsRankingTerm = 0 " +
                        " GROUP BY U.BaseURL, R.RepresentationID, U.BaseURL + CAST(R.RepresentationID AS nvarchar(50)), R.TerminologyID, R.TermID, R.DisplayText, " +
                        "R.Description, R.HierarchyCache, R.HierarchyCacheDown,  " +
                        "R.ExternalID, R.Notes, R.LanguageCode, R.LogUpdatedWhen";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonAnalysis:
                    SQL += "SELECT B.BaseURL, T.NameID, T.ProjectID, T.AnalysisID, MIN(T.AnalysisValue) AS AnalysisValue, MIN(T.Notes) AS Notes, MAX(T.LogUpdatedWhen) AS LogUpdatedWhen " +
                        "FROM " + PrefixDB + "TaxonNameListAnalysis AS T INNER JOIN " +
                        PrefixDB + "TaxonNameListAnalysisCategory AS C ON T.AnalysisID = C.AnalysisID INNER JOIN " +
                        PrefixDB + "TaxonNameProject AS P ON T.NameID = P.NameID CROSS JOIN " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE (P.ProjectID = " + ProjectID.ToString() + ") AND (C.DataWithholdingReason = N'' OR C.DataWithholdingReason IS NULL) " +
                        "GROUP BY T.NameID, T.ProjectID, T.AnalysisID, B.BaseURL";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonAnalysisCategory:
                    SQL += "SELECT B.BaseURL,  T.AnalysisID, T.AnalysisParentID, T.DisplayText, T.Description, AnalysisURI, ReferenceTitle, ReferenceURI, SortingID, T.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "TaxonNameListAnalysisCategory T CROSS JOIN " +
                        PrefixDB + "ViewBaseURL AS B ";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonAnalysisCategoryValue:
                    SQL += "SELECT B.BaseURL,T.AnalysisID, T.AnalysisValue, T.Description, T.DisplayText, T.DisplayOrder, T.Notes, T.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "TaxonNameListAnalysisCategoryValue T CROSS JOIN " +
                        PrefixDB + "ViewBaseURL AS B ";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonCommonName:
                    // Markus 27.3.25: inclusion of ReferenceTitle, LogUpdatedWhen
                    //SQL += "SELECT TOP 100 PERCENT B.BaseURL, T.NameID, T.CommonName, T.LanguageCode, T.CountryCode, MAX(T.LogUpdatedWhen) AS LogUpdatedWhen  " +
                    SQL += "SELECT TOP 100 PERCENT B.BaseURL, T.NameID, T.CommonName, T.LanguageCode, T.CountryCode, T.LogUpdatedWhen, T.ReferenceTitle  " +
                           "FROM " + PrefixDB + "TaxonCommonName T INNER JOIN " +
                           PrefixDB + "TaxonNameProject AS P ON T.NameID = P.NameID, " +
                           PrefixDB + "ViewBaseURL AS B " +
                           //"WHERE P.ProjectID = " + ProjectID.ToString() + " GROUP BY B.BaseURL, T.NameID, T.CommonName, T.LanguageCode, T.CountryCode";
                           "WHERE P.ProjectID = " + ProjectID.ToString() + " GROUP BY B.BaseURL, T.NameID, T.CommonName, T.LanguageCode, T.CountryCode, T.LogUpdatedWhen, T.ReferenceTitle";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonHierarchy:
                    SQL += "SELECT TOP 100 PERCENT B.BaseURL, T.NameID, T.NameParentID, T.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "TaxonHierarchy T, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE (T.IgnoreButKeepForReference = 0) " +
                        "AND T.ProjectID = " + ProjectID.ToString() + " AND NOT T.NameParentID IS NULL";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonList:
                    SQL += "SELECT TOP 100 PERCENT B.BaseURL, T.ProjectID, T.Project, T.DisplayText " +
                        "FROM " + PrefixDB + "TaxonNameListProjectProxy T, " +
                        PrefixDB + "ViewBaseURL AS B ";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonNameExternalDatabase:
                    SQL += "SELECT TOP 100 PERCENT B.BaseURL, ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, " +
                        "ExternalDatabaseURI, ExternalDatabaseInstitution, ExternalAttribute_NameID, LogUpdatedWhen " +
                        "FROM " + PrefixDB + "TaxonNameExternalDatabase T, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE T.Disabled IS NULL";
                    break;
                case UserControlLookupSource.SubsetTable.TaxonNameExternalID:
                    SQL += "SELECT TOP 100 PERCENT B.BaseURL, T.NameID, T.ExternalDatabaseID, T.ExternalNameURI, T.LogUpdatedWhen " +
                        "FROM " + PrefixDB + "TaxonNameExternalID T INNER JOIN " +
                        PrefixDB + "TaxonNameProject AS P ON T.NameID = P.NameID, " +
                        PrefixDB + "ViewBaseURL AS B " +
                        "WHERE P.ProjectID = " + ProjectID.ToString();
                    break;
                case UserControlLookupSource.SubsetTable.TaxonSynonymy:
                    SQL = this.SqlViewTaxonNames(View, "", PrefixDB, ProjectID);
                    break;
                case UserControlLookupSource.SubsetTable.procTaxonNameHierarchy:
                    // Markus 8.4.25: #49 bugfix
                    SQL = "CREATE PROCEDURE [dbo].[procTaxonNameHierarchy] " +
                          "@View nvarchar(50) " +
                          "AS " +
                          "BEGIN " +
                          "/* " +
                          "Sets the NameParentID as defined by the hierarchy " +
                          "exec dbo.procTaxonNameHierarchy '" + View + "' " +
                          "*/ " +
                          "SET NOCOUNT ON; " +
                          "declare @SQL nvarchar(max) " +
                          "set @SQL = (select 'update S set S.NameParentID = H.NameParentID " +
                          "from [dbo].[TaxonSynonymy] S inner join [dbo].[' + @View + '_H] H on H.BaseURL = S.BaseURL and H.NameID = S.NameID " +
                          "and S.SourceView = ''' + @View + '''') " +
                          "begin try " +
                          "exec sp_executesql @SQL " +
                          "end try " +
                          "begin catch " +
                          "end catch " +
                          "END";
                    break;
            }
            return SQL;
        }

        private string SqlViewTaxonNames(string View, string BaseURL, string PrefixDB, int ProjectID)
        {
#region SQL for view
            if (BaseURL.Length == 0)
            {
                string S = "SELECT BaseURL FROM " + PrefixDB + "ViewBaseURL";
                BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(S);
            }

            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT TOP 100 PERCENT " +
                "T.NameID, " +
                "'" + BaseURL + "' AS BaseURL,  " +
                "T.TaxonNameCache AS TaxonName,  " +
                "T.NameID AS AcceptedNameID,  " +
                "T.TaxonNameCache AS AcceptedName,  " +
                "T.TaxonomicRank,  " +
                "T.GenusOrSupragenericName,  " +
                "T.SpeciesGenusNameID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor,  " +
                "A.ProjectID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T.LogUpdatedWhen " +
                "FROM            " + PrefixDB + "TaxonName T INNER JOIN " +
                "" + PrefixDB + "TaxonAcceptedName A ON T.NameID = A.NameID " +
                "WHERE        (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " " +

                "UNION " +

                "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
                "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
                "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
                "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
                "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
                "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
                "FROM            " + PrefixDB + "TaxonSynonymy AS S INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T ON S.SynNameID = T.NameID INNER JOIN " +
                "" + PrefixDB + "TaxonAcceptedName AS A ON T.NameID = A.NameID AND  " +
                "S.ProjectID = A.ProjectID INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) " +
                "AND A.ProjectID = " + ProjectID.ToString() + " " +

                "UNION " +

                "SELECT        TOP 100 PERCENT T.NameID, '" + BaseURL + "' AS BaseURL, T.TaxonNameCache AS TaxonName,  " +
                "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T.TaxonomicRank, T.GenusOrSupragenericName, T.SpeciesGenusNameID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, P.ProjectID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T.LogUpdatedWhen " +
                "FROM            " + PrefixDB + "TaxonName T " +
                ",  " + PrefixDB + "TaxonNameProject P " +
                "WHERE        T.IgnoreButKeepForReference = 0 AND T.NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            " + PrefixDB + "TaxonSynonymy) AND T.NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            " + PrefixDB + "TaxonAcceptedName) AND T.NameID NOT IN " +
                "(SELECT        SynNameID " +
                "FROM            " + PrefixDB + "TaxonSynonymy) " +
                "AND P.NameID = T.NameID AND P.ProjectID = " + ProjectID.ToString() + " " +

                "UNION " +

                "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
                "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
                "FROM            " + PrefixDB + "TaxonAcceptedName AS A INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T ON A.NameID = T.NameID INNER JOIN " +
                "" + PrefixDB + "TaxonSynonymy AS S1 ON T.NameID = S1.SynNameID AND  " +
                "A.ProjectID = S1.ProjectID INNER JOIN " +
                "" + PrefixDB + "TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString() + " AND S1.ProjectID = " + ProjectID.ToString() + " " +

                "UNION " +

                "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
                "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
                "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
                "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
                "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
                "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
                "FROM            " + PrefixDB + "TaxonSynonymy AS S2 INNER JOIN " +
                "" + PrefixDB + "TaxonAcceptedName AS A INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T ON A.NameID = T.NameID ON S2.SynNameID = T.NameID AND  " +
                "S2.ProjectID = A.ProjectID INNER JOIN " +
                "" + PrefixDB + "TaxonSynonymy AS S INNER JOIN " +
                "" + PrefixDB + "TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND  " +
                "S.ProjectID = S1.ProjectID INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND  " +
                "S2.ProjectID = S1.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString() + " AND S1.ProjectID = " + ProjectID.ToString() + " AND S2.ProjectID = " + ProjectID.ToString() + " " +

                "UNION " +

                "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
                "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
                "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
                "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
                "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
                "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
                "FROM            " + PrefixDB + "TaxonSynonymy AS S3 INNER JOIN " +
                "" + PrefixDB + "TaxonAcceptedName AS A INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T ON A.NameID = T.NameID ON S3.SynNameID = T.NameID AND  " +
                "S3.ProjectID = A.ProjectID INNER JOIN " +
                "" + PrefixDB + "TaxonSynonymy AS S2 INNER JOIN " +
                "" + PrefixDB + "TaxonSynonymy AS S INNER JOIN " +
                "" + PrefixDB + "TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND  " +
                "S.ProjectID = S1.ProjectID INNER JOIN " +
                "" + PrefixDB + "TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND  " +
                "S2.ProjectID = S1.ProjectID ON S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " AND S1.ProjectID = " + ProjectID.ToString() + " AND S2.ProjectID = " + ProjectID.ToString() + " AND  " +
                "S3.ProjectID = " + ProjectID.ToString() + " " +

                "UNION " +

                "SELECT        TOP (100) PERCENT T1.NameID, '" + BaseURL + "' AS BaseURL, T1.TaxonNameCache AS TaxonName,  " +
                "T1.NameID AS AcceptedNameID, T1.TaxonNameCache AS AcceptedName, T1.TaxonomicRank, T1.GenusOrSupragenericName, T1.SpeciesGenusNameID,  " +
                "T1.GenusOrSupragenericName + CASE WHEN T1.InfragenericEpithet IS NULL OR " +
                "T1.InfragenericEpithet = '' THEN '' ELSE ' ' + T1.TaxonomicRank + ' ' + T1.InfragenericEpithet END + CASE WHEN T1.SpeciesEpithet IS NULL OR " +
                "T1.SpeciesEpithet = '' THEN '' ELSE ' ' + T1.SpeciesEpithet END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet <> T1.InfraspecificEpithet THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.InfraspecificEpithet IS NULL OR " +
                "T1.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T1.SpeciesEpithet = T1.InfraspecificEpithet AND NOT T1.InfraspecificEpithet IS NULL AND  " +
                "T1.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T1.TaxonomicRank IS NULL OR " +
                "T1.TaxonomicRank = '' OR T1.NomenclaturalCode = 3 THEN '' ELSE T1.TaxonomicRank + ' ' END + T1.InfraspecificEpithet ELSE '' END END + CASE WHEN T1.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T1.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID,  " +
                "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                "T.TaxonomicRank = '' OR T.NomenclaturalCode = 3 THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS AcceptedNameSinAuthor, T1.LogUpdatedWhen " +
                "FROM " + PrefixDB + "TaxonSynonymy AS S1 " +
                " INNER JOIN " + PrefixDB + "TaxonName AS T1 ON S1.NameID = T1.NameID AND ((S1.SynType = N'duplicate') OR (S1.SynType = N'isonym')) AND (S1.IgnoreButKeepForReference = 0) AND S1.ProjectID = " + ProjectID.ToString() +
                " INNER JOIN " + PrefixDB + "TaxonSynonymy AS S ON S1.SynNameID = S.NameID AND (S.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND S1.ProjectID = S.ProjectID AND S.ProjectID = " + ProjectID.ToString() +
                " INNER JOIN " + PrefixDB + "TaxonName AS T ON (T.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND S.SynNameID = T.NameID " +
                " INNER JOIN " + PrefixDB + "TaxonAcceptedName AS A ON T.NameID = A.NameID AND (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND S.ProjectID = A.ProjectID AND A.ProjectID = " + ProjectID.ToString() + 
                " ORDER BY TaxonName, AcceptedName";

#endregion

            return SQL;
        }

#endregion

#endregion

#region Anonym collector

        public bool initAnonymCollector()
        {
            bool OK = true;
            try
            {
                string SQL = "select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'AnonymCollector'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "0")
                {
                    if (this.tabControlMain.TabPages.Contains(this.tabPageAnonymAgent))
                        this.tabControlMain.TabPages.Remove(this.tabPageAnonymAgent);
                    OK = false;
                }
                else
                {
                    SQL = "SELECT Project, ProjectID FROM ProjectPublished ORDER BY Project";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        this.comboBoxAnonymCollectorProject.DataSource = dt;
                        this.comboBoxAnonymCollectorProject.DisplayMember = "Project";
                        this.comboBoxAnonymCollectorProject.ValueMember = "ProjectID";
                        this.comboBoxAnonymCollectorProject.SelectedIndex = 0;
                        this.dataGridViewAnonymAgent.Columns[0].ReadOnly = true;
                        if (!this.tabControlMain.TabPages.Contains(this.tabPageAnonymAgent))
                            this.tabControlMain.TabPages.Add(this.tabPageAnonymAgent);
                    }
                    else
                    {
                        if (this.tabControlMain.TabPages.Contains(this.tabPageAnonymAgent))
                            this.tabControlMain.TabPages.Remove(this.tabPageAnonymAgent);
                        OK = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private void initAnonymColletorSource()
        {
            try
            {
                if (this.comboBoxAnonymCollectorProject.SelectedValue != null)
                {
                    string Value = "";
                    try
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxAnonymCollectorProject.SelectedValue;
                        Value = R["ProjectID"].ToString();
                    }
                    catch (System.Exception ex)
                    {
                        Value = this.comboBoxAnonymCollectorProject.SelectedValue.ToString();
                    }
                    string SQL = "SELECT DISTINCT A.CollectorsName " +
                        " FROM CollectionAgent AS A INNER JOIN " +
                        " CollectionProject AS P ON A.CollectionSpecimenID = P.CollectionSpecimenID " +
                        " WHERE P.ProjectID =  " + Value + // this.comboBoxAnonymCollectorProject.SelectedValue.ToString() +
                        " AND A.CollectorsName NOT IN (SELECT CollectorsName FROM AnonymCollector) " +
                        " ORDER BY A.CollectorsName";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    this.listBoxNotAnonymCollector.DataSource = dt;
                    this.listBoxNotAnonymCollector.DisplayMember = "CollectorsName";
                    this.listBoxNotAnonymCollector.ValueMember = "CollectorsName";
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonCollectorIsAnonym_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dtAn = new DataTable();
                string SQL = "SELECT DISTINCT Anonymisation FROM AnonymCollector WHERE Anonymisation <> '' ORDER BY Anonymisation";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtAn);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtAn, "Anonymisation", "Anonymisation", "Anonymisation", "Please enter or select an anonymisation from the list", "", true);
                f.ShowDialog();
                if (f.DialogResult != System.Windows.Forms.DialogResult.OK)
                    return;
                foreach (System.Object o in this.listBoxNotAnonymCollector.SelectedItems)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)o;
                    System.Data.DataRow R = this.dataSetCacheDatabase.Tables["AnonymCollector"].NewRow();
                    R[0] = RV[0].ToString();
                    if (f.String.Length > 0)
                        R[1] = f.String;
                    this.dataSetCacheDatabase.Tables["AnonymCollector"].Rows.Add(R);
                    R.BeginEdit();
                    R.EndEdit();
                }
                this.FormFunctions.updateTable(this.dataSetCacheDatabase, "AnonymCollector", this._SqlDataAdapterAnonymCollector, this.BindingContext);
                this.initAnonymColletorSource();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonCollectorNotAnonym_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    System.Data.DataRow R = this.dataSetCacheDatabase.Tables["AnonymCollector"].Rows[this.dataGridViewAnonymAgent.SelectedCells[0].RowIndex];
            //    //this.dataSetCacheDatabase.Tables["AnonymCollector"].Rows.Remove(R);
            //    R.Delete();
            //    R.BeginEdit();
            //    R.EndEdit();
            //    this.FormFunctions.updateTable(this.dataSetCacheDatabase, "AnonymCollector", this.anonymCollectorTableAdapter.Adapter, this.BindingContext);
            //    this.initAnonymColletorSource();
            //}
            //catch (System.Exception ex)
            //{
            //}
        }

        private void buttonAnonymCollectorRequery_Click(object sender, EventArgs e)
        {
            this.initAnonymColletorSource();
        }

        private void dataGridViewAnonymAgent_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.FormFunctions.updateTable(this.dataSetCacheDatabase, "AnonymCollector", this._SqlDataAdapterAnonymCollector, this.BindingContext);
            this.initAnonymColletorSource();
        }

        private void dataGridViewAnonymAgent_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void comboBoxAnonymCollectorProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.initAnonymColletorSource();
        }

#endregion

#region BioCASE

        private System.Collections.Generic.Stack<System.Uri> _BioCASEsites;

        private void initBioCASE()
        {
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources.Length > 0)
                this.textBoxBioCASEsources.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources;
        }

        private void buttonBioCASErefresh_Click(object sender, EventArgs e)
        {
            if (this.textBoxBioCASEsources.Text.Length > 0)
            {
                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources != this.textBoxBioCASEsources.Text)
                {
                    DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources = this.textBoxBioCASEsources.Text;
                    DiversityCollection.CacheDatabase.CacheDBsettings.Default.Save();
                }
                try
                {
                    System.Uri U = new Uri(this.textBoxBioCASEsources.Text);
                    this.userControlWebViewBioCASE.Url = null;
                    this.userControlWebViewBioCASE.Navigate(U);
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private void buttonBioCASEback_Click(object sender, EventArgs e)
        {
            if (this._BioCASEsites != null && this._BioCASEsites.Count > 1)
            {
                System.Uri U = this._BioCASEsites.Pop();
                this.userControlWebViewBioCASE.Url = null;
                this.userControlWebViewBioCASE.Navigate(U);
                //this.wexbBrowserBioCASE.Url = this._BioCASEsites.Pop();
            }
        }

        private void webBrowserBioCASE_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (this._BioCASEsites == null)
                this._BioCASEsites = new Stack<Uri>();
            this._BioCASEsites.Push(this.userControlWebViewBioCASE.Url);
            if (this.userControlWebViewBioCASE.Url.ToString() != DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources)
                this.buttonBioCASEback.Enabled = true;
        }

#endregion

#region Transfer, timer and reports

        /// <summary>
        /// Starting the transfer of the data as scheduled for the server based transfer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTransferAll_Click(object sender, EventArgs e)
        {
            //this._IsSimulation = true;
            DiversityCollection.CacheDatabase.CacheDB.IsSimulation = true;
            DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = true;
            this.TransferScheduledData();
            //this.ShowTransferState("Start Transfer of Source in CacheDB");
            //this.StartAutoTransfer(true, false, true);
            //this.ShowTransferState("Start Transfer of CacheDB in Postgres");
            //this.StartAutoTransfer(false, true, true);
            DiversityCollection.CacheDatabase.CacheDB.IsSimulation = false;
            DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = false;
            //this._IsSimulation = false;
        }

        private void TransferScheduledData()
        {
            this.ShowTransferState("Start Transfer of Source in CacheDB");
            this.StartAutoTransfer(true, false);//, true);
            this.ShowTransferState("Start Transfer of CacheDB in Postgres");
            this.StartAutoTransfer(false, true);//, true);
        }

        //bool _IsSimulation = false;

        public void ShowTransferState(string Message)
        {
            if (DiversityCollection.CacheDatabase.CacheDB.IsSimulation)//this._IsSimulation)
            {
                this.toolStripLabelTransferAll.Text = Message;
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private DayOfWeek _SelectedTimerDay;

        private void buttonReports_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialogReport.InitialDirectory = DiversityCollection.CacheDatabase.CacheDB.ReportsDirectory();
                this.openFileDialogReport.Filter = "Textfiles|*.txt";
                this.openFileDialogReport.ShowDialog();
                if (this.openFileDialogReport.FileName.Length > 0)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = this.openFileDialogReport.FileName,
                            UseShellExecute = true
                        });
                        
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void comboBoxTimerDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBoxTimerDay.SelectedIndex)
            {
                case 0:
                    this._SelectedTimerDay = DayOfWeek.Monday;
                    break;
                case 1:
                    this._SelectedTimerDay = DayOfWeek.Tuesday;
                    break;
                case 2:
                    this._SelectedTimerDay = DayOfWeek.Wednesday;
                    break;
                case 3:
                    this._SelectedTimerDay = DayOfWeek.Thursday;
                    break;
                case 4:
                    this._SelectedTimerDay = DayOfWeek.Friday;
                    break;
                case 5:
                    this._SelectedTimerDay = DayOfWeek.Saturday;
                    break;
                case 6:
                    this._SelectedTimerDay = DayOfWeek.Sunday;
                    break;
            }
        }

        private void timerTransfer_Tick(object sender, EventArgs e)
        {

            if (System.DateTime.Now.Hour == this.dateTimePickerTimerTime.Value.Hour &&
                System.DateTime.Now.Minute == this.dateTimePickerTimerTime.Value.Minute)// && System.DateTime.Now.Second == this.dateTimePickerTimerTime.Value.Second)
            {
                if (this.radioButtonTimerWeekly.Checked && System.DateTime.Now.DayOfWeek == this._SelectedTimerDay)
                    this.StartAutoTransfer(this.checkBoxTimerIncludeCacheDB.Checked, this.checkBoxTimerIncludePostgres.Checked);//, false);
                else
                    this.StartAutoTransfer(this.checkBoxTimerIncludeCacheDB.Checked, this.checkBoxTimerIncludePostgres.Checked);//, false);
            }
        }

        //private void StartAutoTransfer()//, bool ProcessOnly)
        //{
        //    this.StartAutoTransfer(CacheDB.IncludeCacheDB, CacheDB.IncludePostgres);
        //}

        private void StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres)//, bool ProcessOnly)
        {
            string Message =  "";
            
            //Check connection to CacheDB
            if (IncludeCacheDB && DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length == 0)
            {
                Message =  "ConnectionStringCacheDB.Length = 0, Connection is not valid";
                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres)", Message);
                this.ShowTransferState(Message);
                return;
            }

            //Check connection to Postgres
            if (IncludePostgres 
                && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0
                && DiversityWorkbench.PostgreSQL.Connection.Password != null
                && DiversityWorkbench.PostgreSQL.Connection.Password.Length > 0)
            {
                Message = "Connection to Postgres is not valid";
                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres)", Message);
                this.ShowTransferState(Message);
                return;
            }

            // Check for updates of cache DB
            if (this.UpdateCheckForDatabase() && IncludeCacheDB)
            {
                Message = "Please update cache database to current version";
                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    System.Windows.Forms.MessageBox.Show(Message);
                else
                {
                    CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", Message);
                    this.ShowTransferState(Message);
                }
                return;
            }

            // Check for updates of Postgres DB
            if (this._PostgresDatabaseNeedsUpdate && IncludePostgres)
            {
                Message = "Please update postgres database to current version";
                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    System.Windows.Forms.MessageBox.Show(Message);
                else
                {
                    CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", Message);
                    this.ShowTransferState(Message);
                }
                return;
            }

            System.IO.StreamWriter sw = null;
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
            {
                string ReportFile = this.TransferProtocolFileName();
                if (System.IO.File.Exists(ReportFile))
                    sw = new System.IO.StreamWriter(ReportFile, true, System.Text.Encoding.UTF8);
                else
                    sw = new System.IO.StreamWriter(ReportFile, false, System.Text.Encoding.UTF8);
            }
            try
            {
                string Report = "";
                CacheDB.LogEvent("", "", "___Transfer_of_Agents___________________________");
                Report = this.TransferAgentSources(IncludeCacheDB, IncludePostgres);
                if (Report.Length > 0 && DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null) 
                    sw.Write(Report);
                CacheDB.LogEvent("", "", "___Transfer_of_Agents_finished__________________");

                CacheDB.LogEvent("", "", "___Transfer_of_Gazetteer___________________________");
                Report = this.TransferGazetteerSources(IncludeCacheDB, IncludePostgres);
                if (Report.Length > 0 && DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null) 
                    sw.Write(Report);
                CacheDB.LogEvent("", "", "___Transfer_of_Gazetteer_finished__________________");

                CacheDB.LogEvent("", "", "___Transfer_of_Terms___________________________");
                Report = this.TransferScientificTermSources(IncludeCacheDB, IncludePostgres);
                if (Report.Length > 0 && DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null) 
                    sw.Write(Report);
                CacheDB.LogEvent("", "", "___Transfer_of_Terms_finished__________________");

                CacheDB.LogEvent("", "", "___Transfer_of_Taxa___________________________");
                Report = this.TransferTaxonomySources(IncludeCacheDB, IncludePostgres);
                if (Report.Length > 0 && DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null) 
                    sw.Write(Report);
                CacheDB.LogEvent("", "", "___Transfer_of_Taxa_finished__________________");

                CacheDB.LogEvent("", "", "___Transfer_of_Collection___________________________");
                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                {
                    this.InitTransferCollectionProjectProtocol(ref sw);
                }
                foreach (InterfaceProject IP in this._I_Projects)
                {
                    if (IP.IncludeInTransfer())
                    {
                        if (IncludeCacheDB)
                        {
                            if (IP.CacheProjectNeedsUpdate())
                            {
                                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                                {
                                    sw.WriteLine();
                                    sw.WriteLine(IP.DisplayText() + " needs update");
                                }
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", IP.DisplayText() + " needs update");
                                continue;
                            }

                            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                            {
                                sw.WriteLine();
                                sw.WriteLine(IP.DisplayText());
                            }
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                IP.initCacheDBControls();
                            else
                            {
                                Message = "Starting transfer to cache DB of " + IP.DisplayText();
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", Message);
                                this.ShowTransferState(Message);
                            }
                            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                            {
                                sw.Write(IP.TransferDataToCacheDB(/*true, */this));
                            }
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                IP.initCacheDBControls();
                            else
                            {
                                Message = IP.DisplayText() + " transferred";
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", Message);
                                this.ShowTransferState(Message);
                            }
                        }
                    }
                    if (IP.IncludePostgresInTransfer())
                    {
                        if (IncludePostgres)
                        {
                            if (IP.PostgresDatabaseNeedsUpdate())
                            {
                                string UpdateMessage = "Postgres database " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + " needs update";
                                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                                {
                                    sw.WriteLine();
                                    sw.WriteLine(UpdateMessage + " needs update");
                                }
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", UpdateMessage);
                                continue;
                            }
                            if (IP.PostgresProjectNeedsUpdate())
                            {
                                string UpdateMessage = "Postgres project " + IP.DisplayText() + " needs update";
                                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                                {
                                    sw.WriteLine();
                                    sw.WriteLine(IP.DisplayText() + " needs update");
                                }
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", UpdateMessage);
                                continue;
                            }
                            if (IP.PostgresPackageNeedsUpdate())
                            {
                                string UpdateMessage = "Packages for " + IP.DisplayText() + " need update";
                                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                                {
                                    sw.WriteLine();
                                    sw.WriteLine(IP.DisplayText() + " needs update");
                                }
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", UpdateMessage);
                                continue;
                            }
                            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                            {

                                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                                    sw.WriteLine();
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                    IP.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
                                else
                                {
                                    Message = "Starting transfer to Postgres of " + IP.DisplayText();
                                    CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", Message);
                                    this.ShowTransferState(Message);
                                }
                                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                                {
                                    sw.Write(IP.TransferDataToPostgres(/*true,*/ this));
                                }
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                    IP.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
                                else
                                {
                                    Message = IP.DisplayText() + " transferred";
                                    CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", Message);
                                    this.ShowTransferState(Message);
                                }
                            }
                        }
                    }
                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        System.Windows.Forms.Application.DoEvents();
                }
                CacheDB.LogEvent("", "", "___Transfer_of_Collection_finished__________________");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        private void TransferCollectionProjects(bool IncludeCacheDB, bool IncludePostgres)
        {
            try
            {
                System.IO.StreamWriter sw = null;
                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                {
                    string ReportFile = this.TransferProtocolFileName();
                    if (System.IO.File.Exists(ReportFile))
                        sw = new System.IO.StreamWriter(ReportFile, true, System.Text.Encoding.UTF8);
                    else
                        sw = new System.IO.StreamWriter(ReportFile, false, System.Text.Encoding.UTF8);
                    this.InitTransferCollectionProjectProtocol(ref sw);
                }

                foreach (InterfaceProject IP in this._I_Projects)
                {
                    if (IP.IncludeInTransfer())
                    {
                        if (IncludeCacheDB)
                        {
                            string Report = IP.TransferDataToCacheDB(/*true,*/ this);
                            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                            {
                                sw.WriteLine();
                                sw.WriteLine(IP.DisplayText());
                                sw.Write(Report);
                            }
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)// ProcessOnly)
                                IP.initCacheDBControls();
                            else
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", IP.DisplayText() + " transferred");
                        }
                        if (IncludePostgres)
                        {
                            string Report = IP.TransferDataToPostgres(/*true, */this);
                            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                            {
                                sw.WriteLine();
                                sw.Write(Report);
                            }
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//ProcessOnly)
                                IP.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
                            else
                                CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", IP.DisplayText() + " transferred");
                        }
                        if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//ProcessOnly)
                            System.Windows.Forms.Application.DoEvents();
                    }
                }
                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents && sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void TransferCollectionProjects(bool IncludeCacheDB, bool IncludePostgres, /*bool ProcessOnly,*/ ref System.IO.StreamWriter sw)
        //{
        //    try
        //    {
        //        this.InitTransferCollectionProjectProtocol(ref sw);

        //        foreach (InterfaceProject IP in this._I_Projects)
        //        {
        //            if (IP.IncludeInTransfer())
        //            {
        //                if (IncludeCacheDB)
        //                {
        //                    sw.WriteLine();
        //                    sw.WriteLine(IP.DisplayText());
        //                    sw.Write(IP.TransferDataToCacheDB(/*true,*/ this));
        //                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)// ProcessOnly)
        //                        IP.initCacheDBControls();
        //                    else
        //                        CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", IP.DisplayText() + " transferred");

        //                }
        //                if (IncludePostgres)
        //                {
        //                    sw.WriteLine();
        //                    sw.Write(IP.TransferDataToPostgres(/*true, */this));
        //                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//ProcessOnly)
        //                        IP.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
        //                    else
        //                        CacheDB.LogEvent(this.Name.ToString(), "StartAutoTransfer(bool IncludeCacheDB, bool IncludePostgres, bool ProcessOnly)", IP.DisplayText() + " transferred");
        //                }
        //                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//ProcessOnly)
        //                    System.Windows.Forms.Application.DoEvents();
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private string TransferProtocolFileName()
        {
            string ReportFile = DiversityCollection.CacheDatabase.CacheDB.ReportsDirectory() + "\\Datatransfer_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"); //.Year.ToString();
            //if (System.DateTime.Now.Month < 10) ReportFile += "0";
            //ReportFile += System.DateTime.Now.Month.ToString();
            //if (System.DateTime.Now.Day < 10) ReportFile += "0";
            //ReportFile += System.DateTime.Now.Day.ToString() + "_";
            //if (System.DateTime.Now.Hour < 10) ReportFile += "0";
            //ReportFile += System.DateTime.Now.Hour.ToString();
            //if (System.DateTime.Now.Minute < 10) ReportFile += "0";
            //ReportFile += System.DateTime.Now.Minute.ToString();
            //if (System.DateTime.Now.Second < 10) ReportFile += "0";
            //ReportFile += System.DateTime.Now.Second.ToString();
            ReportFile += ".txt";
            return ReportFile;
        }

        private void InitTransferCollectionProjectProtocol(ref System.IO.StreamWriter sw)
        {
            sw.WriteLine("Data transfer collection projects to Cache database");
            sw.WriteLine();
            sw.WriteLine("Started by:\t" + System.Environment.UserName);
            sw.Write("Started at:\t");
            sw.WriteLine(DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString());
            sw.WriteLine();
        }

        private void ProtocolWrite(string Message, int ProjectID)
        {
            string SQL = "Update P SET TransferProtocol = TransferProtocol + '" + Message.Replace("'", "''") + "' FROM ProjectPublished P WHERE P.ProjectID = " + ProjectID.ToString();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        }

        private void ProtocolInit(int ProjectID)
        {
            string SQL = "Update P SET TransferProtocol = '' FROM ProjectPublished P WHERE P.ProjectID = " + ProjectID.ToString();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        }

        private void buttonTimerStart_Click(object sender, EventArgs e)
        {
            System.Timers.Timer TransferTimer = new System.Timers.Timer();
            int Interval = 1000 * 60;
            this.timerTransfer.Interval = Interval;
            DayOfWeek DoWnow = System.DateTime.Now.DayOfWeek;
            int DaySetOff = DoWnow - this._SelectedTimerDay;
            this.timerTransfer.Start();
            string Timing = "Every ";
            if (this.radioButtonTimerDaily.Checked) Timing += " day";
            else Timing += "every " + this._SelectedTimerDay.ToString();
            Timing += " at ";
            if (this.dateTimePickerTimerTime.Value.Hour < 10)
                Timing += "0";
            Timing += this.dateTimePickerTimerTime.Value.Hour.ToString() + ":";
            if (this.dateTimePickerTimerTime.Value.Minute < 10)
                Timing += "0";
            Timing += this.dateTimePickerTimerTime.Value.Minute.ToString();
            DiversityCollection.CacheDatabase.FormTransferTimer f =
                new FormTransferTimer(
                    this.checkBoxTimerIncludeCacheDB.Checked,
                    this.checkBoxTimerIncludePostgres.Checked,
                    Timing);
            f.setHelp("Cache database transfer");
            f.ShowDialog();
            this.timerTransfer.Stop();
        }

        private void radioButtonTimerWeekly_Click(object sender, EventArgs e)
        {
            this.setTimerControls();
        }

        private void radioButtonTimerDaily_Click(object sender, EventArgs e)
        {
            this.setTimerControls();
        }

        private void checkBoxTimerIncludeCacheDB_Click(object sender, EventArgs e)
        {
            this.setTimerControls();
        }

        private void checkBoxTimerIncludePostgres_Click(object sender, EventArgs e)
        {
            this.setTimerControls();
        }

        private void setTimerControls()
        {
            if ((!this.radioButtonTimerDaily.Checked &&
                !this.radioButtonTimerWeekly.Checked)
                ||
                (!this.checkBoxTimerIncludeCacheDB.Checked &&
                !this.checkBoxTimerIncludePostgres.Checked))
                this.buttonTimerStart.Enabled = false;
            else
                this.buttonTimerStart.Enabled = true;
        }

        private void buttonDbToCache_Click(object sender, EventArgs e)
        {
            this.StartAutoTransfer(true, false);//, false);
        }

        private void buttonCacheToPostgres_Click(object sender, EventArgs e)
        {
            this.StartAutoTransfer(false, true);//, false);
        }

        private string TransferSources(/*bool ProcessOnly,*/ bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres, System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> iList, string Sources)//, bool FilterForUpdate)
        {
            string Report = "";
            string Message = "";
            try
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                {
                    Message = "Starting " + Sources + " transfer";
                    CacheDB.LogEvent(this.Name.ToString(), "TransferSources(bool ProcessOnly, bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres, System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> iList, string Sources)", Message);
                    this.ShowTransferState(Message);
                }
                foreach (DiversityCollection.CacheDatabase.InterfaceLookupSource T in iList)
                {
                    string ReportSource = "";
                    if (T.TransferData(ref ReportSource, IncludeTransferToCacheDB, IncludeTransferToPostgres, this))
                    {
                        if (ReportSource.Length > 0) // otherwise nothing has been done
                        {
                            Report += ReportSource;
                            if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                            {
                                Message = T.DisplayText() + Report;
                                CacheDB.LogEvent(this.Name.ToString(), "TransferSources(ProcessOnly, IncludeTransferToCacheDB = " + IncludeTransferToCacheDB.ToString() + ", IncludeTransferToPostgres = " + IncludeTransferToPostgres.ToString() + ",iList, Sources)", Message);
                                this.ShowTransferState(Message);
                            }
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                            {
                                T.initCacheDBControls();
                                T.initPostgresControls();
                            }
                        }
                    }
                    else
                    {
                        Message = "Transfer failed for: " + this.Name.ToString();
                        CacheDB.LogEvent(Message, "TransferSources(ProcessOnly, IncludeTransferToCacheDB = " + IncludeTransferToCacheDB.ToString() + ", IncludeTransferToPostgres = " + IncludeTransferToPostgres.ToString() + ",iList, Sources)", T.DisplayText() + " transferred");
                        this.ShowTransferState(Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Sources: " + Sources);
            }
            return Report;
        }

        private bool SourcesAreUpdated(bool IncludeTransferToCacheDB, bool IncludeTransferToPostgres, System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceLookupSource> iList, string Sources)
        {
            bool Updated = false;
            return Updated;
        }

        private void buttonTransferScheduled_Click(object sender, EventArgs e)
        {
            //this.SuspendLayout();
            //this.panelProjects.SuspendLayout();
            try
            {
                this.buttonTransferScheduled.BackColor = System.Drawing.Color.Red;
                System.Windows.Forms.Application.DoEvents();
                this.StartAutoTransfer(true, true);//, true);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.buttonTransferScheduled.BackColor = System.Drawing.Color.Transparent;
            System.Windows.Forms.Application.DoEvents();
            //this.initForm();
            //this.panelProjects.ResumeLayout();
            //this.ResumeLayout();

            System.Windows.Forms.MessageBox.Show("Transfer finished. To see the results, please close and reopen the window");
        }

        private void buttonOpenReports_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialogReport.InitialDirectory = DiversityCollection.CacheDatabase.CacheDB.ReportsDirectory();
                this.openFileDialogReport.Filter = "Textfiles|*.txt";
                this.openFileDialogReport.ShowDialog();
                if (this.openFileDialogReport.FileName.Length > 0 && this.openFileDialogReport.FileName != "openFileDialogReport")
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = this.openFileDialogReport.FileName,
                            UseShellExecute = true
                        });
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

#endregion

#region Old version

        private string _ConnectionStringOld;
        private string _ConnectionStringOld_2;

        private void initOldVersion()
        {
            try
            {
                string SQL = "select min(name) from sys.databases db " +
                    "where db.name like 'DiversityCollectionCache%_2' " +
                    "and replace(replace(name, 'Cache', ''), '_2', '') = '" + DiversityWorkbench.Settings.DatabaseName + "'";
                string CacheDB_2 = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (CacheDB_2.Length > 0)
                {
                    DiversityWorkbench.ServerConnection SC_2 = new DiversityWorkbench.ServerConnection(DiversityWorkbench.Settings.ConnectionString);
                    SC_2.DatabaseName = CacheDB_2;
                    this.labelOldVersionDatabaseName.Text = CacheDB_2;
                    SC_2.ConnectionIsValid = true;
                    this._ConnectionStringOld_2 = SC_2.ConnectionString;
                    string CacheDB = CacheDB_2.Replace("_2", "");
                    DiversityWorkbench.ServerConnection SC = new DiversityWorkbench.ServerConnection(DiversityWorkbench.Settings.ConnectionString);
                    SC.DatabaseName = CacheDB;
                    SC.ConnectionIsValid = true;
                    this._ConnectionStringOld = SC.ConnectionString;
                    this.OldVersionFillViewList();
                    this.initOldVersionProjects();
                }
                else
                {
                    this.tabControlMain.TabPages.Remove(this.tabPageOldVersion);
                }
            }
            catch (System.Exception ex)
            {
                this.tabControlMain.TabPages.Remove(this.tabPageOldVersion);
            }
        }

        private void OldVersionFillViewList()
        {
            try
            {
                string SQL = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS V ORDER BY V.TABLE_NAME";
                System.Data.DataTable dtViews = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionStringOld_2);
                ad.Fill(dtViews);
                this.listBoxOldVersionViews.DataSource = dtViews;
                this.listBoxOldVersionViews.DisplayMember = "TABLE_NAME";
                this.listBoxOldVersionViews.ValueMember = "TABLE_NAME";

            }
            catch (Exception ex)
            {
            }
        }
        
        private void listBoxOldVersionViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT TOP " + this.numericUpDownOldVersion.Value.ToString() + " * FROM " + this.listBoxOldVersionViews.SelectedValue.ToString();
                System.Data.DataTable dtView = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionStringOld_2);
                ad.Fill(dtView);
                this.dataGridViewOldVersionView.DataSource = dtView;

            }
            catch (Exception ex)
            {
            }
        }

        private void buttonTransferOldVersion_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string SQL = "EXECUTE [dbo].[procRefresh] ";
                string Message = "";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionStringOld);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = 0;
                con.Open();
                Message = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
                con.Dispose();
                this.Cursor = System.Windows.Forms.Cursors.Default;
                Message = "Data transferred.\r\n" + Message;
                System.Windows.Forms.MessageBox.Show(Message);
            }
            catch (Exception ex)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void initOldVersionProjects()
        {
            try
            {
                string SQL = "SELECT ProjectID, Project FROM dbo.ProjectPublished";
                System.Data.DataTable dtPublished = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionStringOld);
                ad.Fill(dtPublished);
                this.listBoxOldVersionProjectsPublished.DataSource = dtPublished;
                this.listBoxOldVersionProjectsPublished.DisplayMember = "Project";
                this.listBoxOldVersionProjectsPublished.ValueMember = "ProjectID";
                string PPid = "";
                foreach (System.Data.DataRow R in dtPublished.Rows)
                {
                    if (PPid.Length > 0)
                        PPid += ", ";
                    PPid += R[0].ToString();
                }
                // Markus - 11.3.2021 auf von ...List auf ...Proxy geaendert
                SQL = "SELECT ProjectID, Project FROM ProjectProxy";
                if (PPid.Length > 0)
                    SQL += " WHERE ProjectID NOT IN (" + PPid + ")";
                System.Data.DataTable dtUnpublished = new DataTable();
                string Error = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtUnpublished, ref Error);
                this.listBoxOldVersionProjectsUnpublished.DataSource = dtUnpublished;
                this.listBoxOldVersionProjectsUnpublished.DisplayMember = "Project";
                this.listBoxOldVersionProjectsUnpublished.ValueMember = "ProjectID";
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonOldVersionProjectPublish_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxOldVersionProjectsUnpublished.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxOldVersionProjectsUnpublished.SelectedItem;
                    if (System.Windows.Forms.MessageBox.Show("Publish project " + R["Project"].ToString(), "Publish project?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string SQL = "INSERT INTO ProjectPublished (ProjectID, Project) VALUES (" + R["ProjectID"].ToString() + ", '" + R["Project"].ToString() + "')";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionStringOld);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        this.initOldVersionProjects();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void buttonOldVersionProjectUnpublish_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxOldVersionProjectsPublished.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxOldVersionProjectsPublished.SelectedItem;
                    if (System.Windows.Forms.MessageBox.Show("Remove project " + R["Project"].ToString() + " from published list", "Unpublish project?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string SQL = "DELETE FROM ProjectPublished WHERE ProjectID = " + R["ProjectID"].ToString();
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionStringOld);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        this.initOldVersionProjects();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

#endregion

#region Toolstrip Header

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        private void toolStripButtonTransferAll_Click(object sender, EventArgs e)
        {
            //this._IsSimulation = true;
            DiversityCollection.CacheDatabase.CacheDB.IsSimulation = true;
            DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = true;
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                DiversityWorkbench.ExceptionHandling.InitErrorLogFile("___Simulation_of_Cachetransfer______________");

            this.ShowTransferState("Start Transfer of Source in CacheDB");
            CacheDB.LogEvent("", "", "___Start_Transfer_of_Source_to_CacheDB_______________");
            this.StartAutoTransfer(true, false);//, true);
            CacheDB.LogEvent("", "", "___Transfer_of_Source_to_CacheDB_finished____________");

            this.ShowTransferState("Start Transfer of CacheDB in Postgres");
            CacheDB.LogEvent("", "", "___Start_Transfer_of_CacheDB_to_Postgres_______________");
            this.StartAutoTransfer(false, true);//, true);
            CacheDB.LogEvent("", "", "___Transfer_of_CacheDB_to_Postgres_finished____________");

            CacheDB.LogEvent("", "", "___Transfer_finished___________________________________");

            this.ShowTransferState("Transfer of all data finished");
            DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = false;
            DiversityCollection.CacheDatabase.CacheDB.IsSimulation = false;
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormSettings f = new FormSettings();
            f.ShowDialog();
        }
        
        private void cacheDBAndPostgresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CacheDB.IncludePostgres = true;
            CacheDB.IncludeCacheDB = true;
            TransferTest();
        }

        private void cacheDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CacheDB.IncludePostgres = false;
            CacheDB.IncludeCacheDB = true;
            TransferTest();
        }

        private void postgresOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CacheDB.IncludePostgres = true;
            CacheDB.IncludeCacheDB = false;
            TransferTest();
        }

        private void TransferTest()
        {
            //this._IsSimulation = true;
            DiversityCollection.CacheDatabase.CacheDB.IsSimulation = true;
            DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = true;
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                DiversityWorkbench.ExceptionHandling.InitErrorLogFile("___Simulation_of_Cachetransfer______________");

            if (CacheDB.IncludeCacheDB)
            {
                this.ShowTransferState("Start Transfer of Source in CacheDB");
                CacheDB.LogEvent("", "", "___Start_Transfer_of_Source_to_CacheDB_______________");
                this.StartAutoTransfer(true, false);//, true);
                CacheDB.LogEvent("", "", "___Transfer_of_Source_to_CacheDB_finished____________");
            }
            if (CacheDB.IncludePostgres)
            {
                this.ShowTransferState("Start Transfer of CacheDB in Postgres");
                CacheDB.LogEvent("", "", "___Start_Transfer_of_CacheDB_to_Postgres_______________");
                this.StartAutoTransfer(false, true);//, true);
                CacheDB.LogEvent("", "", "___Transfer_of_CacheDB_to_Postgres_finished____________");
            }

            CacheDB.LogEvent("", "", "___Transfer_finished___________________________________");

            this.ShowTransferState("Transfer of all data finished");
            DiversityCollection.CacheDatabase.CacheDB.ProcessOnly = false;
            DiversityCollection.CacheDatabase.CacheDB.IsSimulation = false;
        }

        private void toolStripButtonDocumentation_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormDocumentation f = new DiversityWorkbench.Forms.FormDocumentation(this.helpProvider.HelpNamespace, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDBWithTimeOut(DiversityWorkbench.Settings.TimeoutDatabase));
            f.Width = this.Width - 10;
            f.Height = this.Height - 10;
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void toolStripButtonDatabaseTools_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormDatabaseTool f = new DiversityWorkbench.Forms.FormDatabaseTool(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, true);
            f.setHelpProvider(this.helpProvider.HelpNamespace, "Database tools");
            f.setTitle("- " + DiversityCollection.CacheDatabase.CacheDB.DatabaseName);
            f.ShowDialog();
        }

        #endregion

        #region Errorlog
        private void buttonErrorlogRequery_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo E = new System.IO.FileInfo(DiversityWorkbench.ExceptionHandling.ErrorLogFilePath);
            if (E.Exists)
            {
                try
                {
                    System.IO.FileInfo fileObj = new System.IO.FileInfo(E.FullName);
                    fileObj.Attributes = System.IO.FileAttributes.ReadOnly;
                    System.Text.Encoding Encode = System.Text.Encoding.UTF8;
                    System.IO.StreamReader sr = new System.IO.StreamReader(E.FullName, Encode);
                    using (sr)
                    {
                        this.textBoxErrorlog.Text = "";
                        string line = "";
                        int iline = 0;
                        bool ErrorFound = false;
                        while ((line = sr.ReadLine()) != null)
                        {
                            iline++;
                            if (this.checkBoxErrorlogRestrictToFailure.Checked)
                            {
                                if (line.Trim().Length == 0)
                                    ErrorFound = false;
                                if (line.ToLower().IndexOf("failure") == -1 &&
                                   line.IndexOf("ERROR:") == -1 &&
                                   line.IndexOf("FEHLER:") == -1 &&
                                   line.IndexOf("ERREUR:") == -1 &&
                                   line.IndexOf("ERRO:") == -1 &&
                                   line.IndexOf("ERRORE:") == -1 &&
                                   !ErrorFound)
                                {
                                    continue;
                                }
                                else if (!ErrorFound)
                                {
                                    this.textBoxErrorlog.Text += "...\r\n";
                                    ErrorFound = true;
                                }
                            }
                            if (this.checkBoxErrorlogShowLines.Checked)
                                this.textBoxErrorlog.Text += iline.ToString() + "\t";
                            this.textBoxErrorlog.Text += line + "\r\n";
                            if (iline > this.numericUpDownErrorlog.Value)
                            {
                                this.textBoxErrorlog.Text += "...";
                                break;
                            }
                        }
                        sr.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No ErrorLog present");
            }
        }

        private void buttonErrorlogClear_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ExceptionHandling.InitErrorLogFile("");
        }

        private void buttonErrorlogOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.ExceptionHandling.ShowErrorLog();
        }

#endregion

#region Diagnostics

        private System.Collections.Generic.SortedDictionary<string, int> _PublishedProjects;

        private void initDiagnostics()
        {
            try
            {
                int? CurrentProjectID = null;
                if (_PublishedProjects == null)
                    _PublishedProjects = new SortedDictionary<string, int>();
                string SQL = "SELECT ProjectID, Project FROM ProjectPublished";
                System.Data.DataTable dtProjects = new DataTable();
                string Message = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtProjects, ref Message))
                {
                    foreach (System.Data.DataRow R in dtProjects.Rows)
                    {
                        int ProjectID;
                        if (int.TryParse(R[0].ToString(), out ProjectID) && !_PublishedProjects.ContainsKey(R[1].ToString())) // Markus 2.4.25: Test if present added
                        {
                            _PublishedProjects.Add(R[1].ToString(), ProjectID);
                        }
                        else { }
                    }
                }
                foreach (System.Windows.Forms.Control C in this.panelProjects.Controls)
                {
                    if (C.GetType() == typeof(DiversityCollection.CacheDatabase.UserControlProject))
                    {
                        DiversityCollection.CacheDatabase.UserControlProject CP = (DiversityCollection.CacheDatabase.UserControlProject)C;
                        if (CP.PostgresProject != null &&
                            _PublishedProjects.ContainsKey(CP.Project) &&
                            CP.ProjectID() == _PublishedProjects[CP.Project])
                        {
                            CurrentProjectID = CP.PostgresProject.ProjectID;
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this._PublishedProjects)
                    this.comboBoxDiagnosticsProject.Items.Add(KV.Key);
                if (CurrentProjectID != null)
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in _PublishedProjects)
                    {
                        if ((int)CurrentProjectID == KV.Value)
                        {
                            this.comboBoxDiagnosticsProject.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                }

                this.comboBoxDiagnosticsTarget.Items.Add(Diagnostics.Target.TargetType.ABCD.ToString());
                this.comboBoxDiagnosticsTarget.SelectedIndex = 0;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); } // Markus 2.4.25: Handling exception
        }

        private void buttonDiagnosticsStart_Click(object sender, EventArgs e)
        {
#if !DEBUG
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;
#endif
            if (this.comboBoxDiagnosticsProject.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }

            if (this.comboBoxDiagnosticsTarget.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a target");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                DiversityWorkbench.Forms.FormStarting f = new DiversityWorkbench.Forms.FormStarting(DiversityWorkbench.Forms.FormStarting.Unit.Details, "Diagnostics", "Analysing data ...", 9);
                f.Show();
                switch (this.comboBoxDiagnosticsTarget.SelectedItem.ToString())
                {
                    case "ABCD":
                        ABCD TargetABCD = new ABCD(this.comboBoxDiagnosticsProject.SelectedItem.ToString(), this._PublishedProjects[this.comboBoxDiagnosticsProject.SelectedItem.ToString()]);
                        TargetABCD.StartDiagnostics(f);
                        Uri U = new Uri(TargetABCD.ResultFile());
                        this.userControlWebViewDiagnostics.Url = null;
                        this.userControlWebViewDiagnostics.Navigate(U);
                        break;
                }
                f.setEnd();
                f.Close();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
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
