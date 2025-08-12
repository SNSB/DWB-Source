#define Version202

using DiversityWorkbench;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace DiversityCollection.CacheDatabase
{
    public partial class UserControlProject : UserControl, InterfaceProject
    {

        #region Parameter

        private DiversityCollection.CacheDatabase.InterfaceCacheDatabase _I_CacheDatabase;
        private int _ProjectID;
        private string _Project;

        private System.Collections.Generic.Dictionary<string, object> _TransferHistory = new Dictionary<string, object>();

        private enum StateOfData { SourceUpdated, NoDataInTarget, TargetUpToDate, PartialUpToDate, MissingInSource, NoAccess, Unknown }

        public string Project
        {
            get { return _Project; }
            set { _Project = value.Replace(" ", "_"); }
        }

        private bool _CacheProjectNeedsUpdatee = false;
        private bool _PostgresDatabaseNeedsUpdate = false;
        private bool _PostgresProjectNeedsUpdate = false;
        private bool _PostgresPackageNeedsUpdate = false;

        //private string _TransferDirectory = "";

        public bool CacheProjectNeedsUpdate() { return this._CacheProjectNeedsUpdatee; }
        public bool PostgresDatabaseNeedsUpdate() { return this._PostgresDatabaseNeedsUpdate; }
        public bool PostgresProjectNeedsUpdate() { return this._PostgresProjectNeedsUpdate; }
        public bool PostgresPackageNeedsUpdate() { return this._PostgresPackageNeedsUpdate; }
       
        #endregion

        #region Construction

        public UserControlProject(int ProjectID, string Project, DiversityCollection.CacheDatabase.InterfaceCacheDatabase InterfaceCacheDB)
        {
            InitializeComponent();
            this._ProjectID = ProjectID;
            this.Project = Project;
            this._I_CacheDatabase = InterfaceCacheDB;
            this.initControl();
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

        public string DisplayText() { return this.Project; }

        public int ProjectID() { return this._ProjectID; }
        #endregion

        #region Control

        private readonly int _ControlHeight = 94;

        public void initControl()
        {
            try
            {
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(this._ControlHeight);

                this.textBoxProjectID.Text = this._ProjectID.ToString();

                // Checking Access
                string Message = "";
                string SQL = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString() +
                    " AND ProjectID NOT IN (SELECT ProjectID FROM ProjectList)";
                string NoAccess = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                if (NoAccess.Length > 0)
                    this._StateOfCacheData = StateOfData.NoAccess;

                string Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString(), ref Message);
                if (Project.Length > 0)
                    this.Project = Project; // DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString());
                this.labelProject.Text = this.Project;

                //Checking Project name
                SQL = "SELECT P.Project FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
                // Markus 28.3.25: Check count. Issue #36
                string DifferingProjectName = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, true);
                if (DifferingProjectName != Project)
                {
                    this.buttonProjectDifferingName.Visible = true;
                    this.toolTip.SetToolTip(this.buttonProjectDifferingName, "The name of the project in the cache database differs from that in the database: " + DifferingProjectName);
                    this.buttonProjectDifferingName.Tag = DifferingProjectName;
                }

                // Inclusion in schedule based transfer
                try
                {
                    System.Data.DataTable dtProPub = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT * FROM ProjectPublished WHERE ProjectID = " + this._ProjectID.ToString(), DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    ad.Fill(dtProPub);
                    if (!dtProPub.Rows[0]["IncludeInTransfer"].Equals(System.DBNull.Value) && dtProPub.Rows[0]["IncludeInTransfer"].ToString() == "True")
                        this.checkBoxIncludeInTransfer.Checked = true;
                    else this.checkBoxIncludeInTransfer.Checked = false;
                    if (!dtProPub.Rows[0]["TransferProtocol"].Equals(System.DBNull.Value) && dtProPub.Rows[0]["TransferProtocol"].ToString().Length > 0)
                        this.buttonTransferProtocol.Enabled = true;
                    else this.buttonTransferProtocol.Enabled = false;
                    if (!dtProPub.Rows[0]["TransferErrors"].Equals(System.DBNull.Value) && dtProPub.Rows[0]["TransferErrors"].ToString().Length > 0)
                        this.buttonTransferErrors.Enabled = true;
                    else this.buttonTransferErrors.Enabled = false;
                }
                catch (System.Exception ex) { }

                // Number of datasets
                if (this._StateOfCacheData == StateOfData.NoAccess)
                {
                    this.labelNumberInDatabase.Text = "No access";
                    this.labelNumberInDatabase.ForeColor = System.Drawing.Color.Red;
                    System.Drawing.Font F = new Font(this.labelNumberInDatabase.Font, FontStyle.Bold);
                    this.labelNumberInDatabase.Font = F;
                    this.labelLastUpdateInDatabase.Text = "";
                    this.labelDatawithholding.Text = "";
                    this.labelProjectID.ForeColor = System.Drawing.Color.Tomato;
                    this.textBoxProjectID.ForeColor = System.Drawing.Color.Tomato;
                }
                else
                {
                    this.labelNumberInDatabase.Text = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT COUNT(*) AS Datasets FROM CollectionProject WHERE ProjectID = " + this._ProjectID.ToString());
                    if (this.labelNumberInDatabase.Text == "0")
                        this.labelNumberInDatabase.Text = "No data";
                    else if (this.labelNumberInDatabase.Text == "1")
                        this.labelNumberInDatabase.Text += " dataset";
                    else
                        this.labelNumberInDatabase.Text += " datasets";
                    // Last update
                    string LastUpdate = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT MAX(CONVERT(nvarchar(50), S.LogUpdatedWhen, 120)) AS LastUpdate " +
                        "FROM CollectionProject AS P INNER JOIN CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                        "WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND (S.DataWithholdingReason is null or S.DataWithholdingReason = '') ");
                    if (LastUpdate.Length > 0)
                        this.labelLastUpdateInDatabase.Text = "Last update:\r\n" + LastUpdate;
                    else
                        this.labelLastUpdateInDatabase.Text = "";
                    // Datawithholding
                    this.labelDatawithholding.Text = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT COUNT(*) AS Datasets FROM CollectionProject P, CollectionSpecimen S " +
                        "WHERE S.CollectionSpecimenID = P.CollectionSpecimenID AND S.DataWithholdingReason <> '' AND P.ProjectID = " + this._ProjectID.ToString()) +
                        " datasets withheld";
                }
                this.initCacheDB();
                this.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
                this.initArchive();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public string Schema()
        {
            if (this.Project.Length > 0) 
                return "Project_" + this.Project;
            else 
                return "";
        }

        private void buttonProjectDifferingName_Click(object sender, EventArgs e)
        {
            string Message = "The name of the project in the cache database:\r\n" + buttonProjectDifferingName.Tag.ToString() +
                "\r\ndiffers from that in the database:\r\n" + this.Project;
            System.Windows.Forms.MessageBox.Show(Message);
        }

        #endregion  
      
        #region Database

        private void buttonDatawithholding_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to see and edit the datawithholding reasons. It may need some time to retrieve it", "Edit withholding?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityCollection.Forms.FormDatawithholding f = new Forms.FormDatawithholding(this._ProjectID);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();
        }

        #endregion

        #region Cache database

        public bool CacheProjectTableDoesExist(string tableName)
        {
            try
            {
                // Query to check if the table exists in the database
                string sql = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{this.Schema()}' AND TABLE_NAME = '{tableName}'";
                // Execute the query
                var result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(sql);
                // Return true if the table exists
                return !string.IsNullOrEmpty(result);
            }
            catch (Exception ex)
            {
                // Log the error
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile($"Error in CacheProjectTablesDoExist: {ex.Message}");
                return false;
            }
        }

        public bool CacheProjectDoesExist()
        {
            try
            {
                string Message = "";
                string sql = $"SELECT [Project_{this.Project}].Version()";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(sql, ref Message))
                    return true;
                else
                {
                    ExceptionHandling.WriteToErrorLogFile(
                        $"Error - UserControlProject.CacheProjectDoesExist(): Message: {Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(
                    $"Error in CacheProjectDoesExist: {ex.Message}");
                return false;
            }
        }

        private void initCacheDB()
        {
            if (this.CacheProjectDoesExist())
            {
                this.initCacheDBControls();
                this.UpdateCheckCacheDBProject();
                this.initScheduleControls();
            }
        }

        public void initCacheDBControls()
        {
            this.labelNumberInCacheDB.Text = "No data";
            this.labelLastUpdateInCacheDatabase.Text = "";
            if (this.CacheProjectDoesExist() && CacheProjectTableDoesExist("CacheCollectionSpecimen"))
            {
              
                this.labelNumberInCacheDB.Text =
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB("SELECT COUNT(*) FROM [" +
                        this.Schema() + "].CacheCollectionSpecimen AS Datasets");
                
                if (this.labelNumberInCacheDB.Text.Length == 0 || this.labelNumberInCacheDB.Text == "0")
                {
                    this.labelNumberInCacheDB.Text = "No data";
                    this.labelLastUpdateInCacheDatabase.Text = "";
                }
                else
                {
                    if (this.labelNumberInCacheDB.Text == "0")
                        this.labelNumberInCacheDB.Text = "No data";
                    else if (this.labelNumberInCacheDB.Text == "1")
                        this.labelNumberInCacheDB.Text += " dataset";
                    else
                        this.labelNumberInCacheDB.Text += " datasets";
                    string LastUpdate = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB("SELECT CONVERT(nvarchar(50), P.LastUpdatedWhen, 120) AS LastUpdate " +
                        "FROM dbo.ProjectPublished AS P WHERE P.ProjectID = " + this._ProjectID.ToString());
                    if (LastUpdate.Length == 0)
                        LastUpdate = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB("SELECT CONVERT(nvarchar(50), MAX(S.LogUpdatedWhen), 120) AS LastUpdate " +
                        "FROM " + this.Schema() + ".CacheCollectionSpecimen AS S");
                    this.labelLastUpdateInCacheDatabase.Text = "Last transfer:\r\n" + LastUpdate;
                }

                // EmbargoDate
                if (this.EmbargoDate() != null)
                {
                    this.labelDatawithholding.BackColor = System.Drawing.Color.Red;
                    this.labelDatawithholding.ForeColor = System.Drawing.Color.White;
                    System.DateTime Embargo = (System.DateTime)this.EmbargoDate();
                    this.labelDatawithholding.Text = "Embargo until " + Embargo.ToString("yyyy-MM-dd"); // + Embargo.Year.ToString() + "-" + Embargo.Month.ToString() + "-" + Embargo.Day.ToString();
                }
                string CurrentTransfer = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB("SELECT TransferIsExecutedBy " +
                    "FROM dbo.ProjectPublished AS P WHERE P.ProjectID = " + this._ProjectID.ToString());
                if (CurrentTransfer.Length > 0)
                {
                    this.buttonTransferDatabaseToCache.Enabled = false;
                    this.buttonTransferDatabaseToCache.BackColor = System.Drawing.Color.Red;
                    this.labelLastUpdateInCacheDatabase.Text = "Active transfer: " + CurrentTransfer;
                    this.labelLastUpdateInCacheDatabase.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    if (!this.buttonUpdateCache.Enabled)// && this.EmbargoDate() == null)
                        this.buttonTransferDatabaseToCache.Enabled = true;

                    // setting the color of the transfer button according to the state of the data
                    switch (this.StateOfCacheData())
                    {
                        case UserControlProject.StateOfData.SourceUpdated:
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderSize = 4;
                            //this.buttonTransferDatabaseToCache.Image = DiversityCollection.Resource.ArrowNextNext;
                            //this.labelLastUpdateInCacheDatabase.ForeColor = System.Drawing.Color.Red;
                            this.toolTip.SetToolTip(this.buttonTransferDatabaseToCache, "Data in source have been updated. Transfer data to cache database");
                            break;
                        case UserControlProject.StateOfData.PartialUpToDate:
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderSize = 1;
                            //this.buttonTransferDatabaseToCache.Image = DiversityCollection.Resource.ArrowNext1;
                            //this.labelLastUpdateInCacheDatabase.ForeColor = System.Drawing.Color.Red;
                            this.toolTip.SetToolTip(this.buttonTransferDatabaseToCache, "Data in source have been updated. A part of the data have been transferred. Transfer data to cache database");
                            break;
                        case UserControlProject.StateOfData.TargetUpToDate:
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderSize = 0;
                            //this.buttonTransferDatabaseToCache.Image = DiversityCollection.Resource.ArrowNext1;
                            //this.labelLastUpdateInCacheDatabase.ForeColor = System.Drawing.SystemColors.WindowText;
                            this.toolTip.SetToolTip(this.buttonTransferDatabaseToCache, "Data are up to date. Transfer of data to cache database not necessary");
                            break;
                        case StateOfData.NoDataInTarget:
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderSize = 4;
                            this.toolTip.SetToolTip(this.buttonTransferDatabaseToCache, "Data missing in target. Transfer data to cache database");
                            break;
                        default:
                            this.buttonTransferDatabaseToCache.FlatAppearance.BorderSize = 0;
                            //this.buttonTransferDatabaseToCache.Image = DiversityCollection.Resource.ArrowNext1;
                            //this.labelLastUpdateInCacheDatabase.ForeColor = System.Drawing.SystemColors.WindowText;
                            this.toolTip.SetToolTip(this.buttonTransferDatabaseToCache, "Transfer data to cache database");
                            break;
                    }
                }
                if (this.CompareLogDateForCacheDB) this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.Filter;
                else this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.FilterClear;
            }
        }

        private bool _EmbargoChecked = false;
        private System.DateTime? _EmbargoDate;
        private System.DateTime? EmbargoDate()
        {
            if (!_EmbargoChecked)
            {
                string SQL = "SELECT [dbo].[ProjectsDatabase]()";
                string ProjectsDB = CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                SQL = "SELECT COUNT(*) FROM " + ProjectsDB + ".dbo.Project WHERE (ProjectID = " + this._ProjectID.ToString() + ") AND (EmbargoDate > GETDATE())";
                string Result = CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "0") { return null; }
                SQL = "SELECT EmbargoDate FROM " + ProjectsDB + ".dbo.Project WHERE (ProjectID = " + this._ProjectID.ToString() + ") AND (EmbargoDate > GETDATE())";
                System.DateTime Embargo;
                if (Result.Length > 0 && System.DateTime.TryParse(Result, out Embargo))
                {
                    _EmbargoDate = Embargo;
                    _EmbargoChecked = true;
                    return Embargo;
                }
                else
                    return null;
            }
            else
                return _EmbargoDate;
        }

        private StateOfData _StateOfCacheData = StateOfData.Unknown;
        private StateOfData StateOfCacheData()
        {
            if (this._StateOfCacheData == StateOfData.Unknown)
            {
                try
                {
                    string SQL = "SELECT CASE WHEN P.[LastChanges] IS NULL THEN 0 ELSE CASE WHEN P.[LastChanges] > PP.LastUpdatedWhen THEN 1 ELSE " +
                        "case when PP.LastUpdatedWhen is null then 3 else 2 end END END AS StateOfDate " +
                        "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P, " +
                        "[ProjectPublished] PP " +
                        "WHERE P.ProjectID = PP.ProjectID AND P.ProjectID = " + this._ProjectID.ToString();
                    int State = 0;
                    if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL), out State))
                    {
                        switch (State)
                        {
                            case 0:
                            case 2:
                                _StateOfCacheData = StateOfData.TargetUpToDate;
                                break;
                            case 1:
                                SQL = "SELECT COUNT(*) FROM " + this.Schema() + ".CacheCollectionSpecimen";
                                if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL), out State) && State > 0)
                                    _StateOfCacheData = StateOfData.SourceUpdated;
                                else
                                    _StateOfCacheData = StateOfData.NoDataInTarget;
                                break;
                            case 3:
                                _StateOfCacheData = StateOfData.NoDataInTarget;
                                break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    _StateOfCacheData = StateOfData.Unknown;
                }
            }
            return this._StateOfCacheData;
        }

        public bool EstablishProject(int ProjectID, string Project)
        {
            this._Project = Project.Replace(" ", "_");
            this._ProjectID = ProjectID;
            bool Establihed = false;
            try
            {
                string SQL = "CREATE SCHEMA [" + this.Schema() + "]";
                string Message = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                {
                    SQL = "ALTER AUTHORIZATION ON SCHEMA::[" + this.Schema() + "] TO [CacheAdmin]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    {
                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        {
                            SQL = "GRANT SELECT ON SCHEMA::[" + this.Schema() + "] TO [CacheUser]";
                            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            {
                                SQL = "CREATE FUNCTION [" + this.Schema() + "].[Version] () RETURNS int AS BEGIN RETURN 0 END";
                                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                {
                                    SQL = "INSERT INTO ProjectPublished (ProjectID, Project, LastUpdatedWhen, LastUpdatedBy) VALUES (" + ProjectID.ToString() + ", '" + Project + "', NULL, NULL)";
                                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                    {
                                        SQL = "CREATE FUNCTION [" + this.Schema() + "].[ProjectID] () RETURNS int AS BEGIN RETURN " + ProjectID.ToString() + " END";
                                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                            Establihed = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Message.Length > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("An error greating the schema occured:\r\n" + Message + "\r\n\r\nDo you want to remove this schema?", "Remove schema?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Message = "";
                        bool OK = this.RemoveSchema(ref Message);
                        //SQL = "DROP SCHEMA [" + this.Schema() + "]";
                        if (!OK)
                        {
                            System.Windows.Forms.MessageBox.Show(Message, "Removing failed");
                        }
                        else
                            System.Windows.Forms.MessageBox.Show("Schema has been removed. \r\nPlease retry creating the project");
                        //else
                        //{
                        //    // Removing entries in table ProjectPublished
                        //    SQL = "DELETE P FROM ProjectPublished P WHERE ProjectID = " + this._ProjectID.ToString();
                        //    if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        //        OK = false;
                        //}
                        //if (OK)
                        //{
                        //    System.Windows.Forms.MessageBox.Show("Schema " + this.Schema() + " removed");
                        //    SQL = "DELETE P FROM ProjectPublished P WHERE ProjectID = " + this._ProjectID.ToString();
                        //    if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                        //        OK = false;
                        //    DiversityCollection.CacheDatabase.CacheDB.ResetDtProjects();
                        //    this._I_CacheDatabase.initProjects(false);
                        //}
                    }
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (Establihed)
                this.initControl();
            return Establihed;
        }

        #region Update

        private void buttonUpdateCache_Click(object sender, EventArgs e)
        {
            try
            {
                // check resouces for update scripts
                string sVersion = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB("SELECT [Project_" + this.Project + "].Version()");
                int CurrentVersion;
                if (!int.TryParse(sVersion, out CurrentVersion))
                    return;
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
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdateSchema_"))
                            {
                                string Descriminator = de.Key.ToString().Substring(de.Key.ToString().LastIndexOf("_")+1);
                                int iVersion;
                                if (int.TryParse(Descriminator, out iVersion))
                                {
                                    if (iVersion > CurrentVersion)
                                        Versions.Add(de.Key.ToString(), de.Value.ToString());
                                }
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> ReplaceStrings = new Dictionary<string, string>();
                    ReplaceStrings.Add("#project#", this.Schema());
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    DiversityWorkbench.Forms.FormUpdateDatabase f =
                        new DiversityWorkbench.Forms.FormUpdateDatabase(
                            DiversityCollection.CacheDatabase.CacheDB.DatabaseName, // .Postgres.PostgresConnection().Database,
                            DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB,
                            DiversityCollection.Properties.Settings.Default.SqlServerCacheDBProjectVersion,
                            Versions,
                            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(),
                            this.Schema(),
                            ReplaceStrings);
                    f.ShowInTaskbar = true;
                    f.ShowDialog();
                    this.UpdateCheckCacheDBProject();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }
            }
            catch (System.Exception ex)
            { }
        }

        public void UpdateCheckCacheDBProject()
        {
            string SQL = "select [" + this.Schema() + "].version()";
            try
            {
                int version = int.Parse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL));// .Postgres.PostgresExecuteSqlSkalar(SQL));

#if xDEBUG
                // for testing the value of CacheDBProjectVersion - obviously a wrong value has been taken
                int versionCurrent = DiversityCollection.Properties.Settings.Default.SqlServerCacheDBProjectVersion;
                string vC = versionCurrent.ToString();
                System.Windows.Forms.MessageBox.Show(vC);
#endif 

                if (version < DiversityCollection.Properties.Settings.Default.SqlServerCacheDBProjectVersion)//.CacheDBProjectVersion)
                {
                    this.buttonUpdateCache.Enabled = true;
                    this.buttonUpdateCache.Text = "Update (V. " + DiversityCollection.Properties.Settings.Default.SqlServerCacheDBProjectVersion.ToString() + ")";
                    this.toolTip.SetToolTip(this.buttonUpdateCache, "Update to version " + DiversityCollection.Properties.Settings.Default.SqlServerCacheDBProjectVersion.ToString());
                    this.buttonTransferDatabaseToCache.Enabled = false;
                    this.buttonSetTransferFilter.Enabled = false;
                    this._CacheProjectNeedsUpdatee = true;
                }
                else
                {
                    this.buttonUpdateCache.Enabled = false;
                    this.buttonUpdateCache.Text = "Version " + version.ToString();
                    string CurrentTransfer = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB("SELECT TransferIsExecutedBy " +
                        "FROM dbo.ProjectPublished AS P WHERE P.ProjectID = " + this._ProjectID.ToString());
                    if (CurrentTransfer.Length > 0)
                    {
                        this.buttonTransferDatabaseToCache.Enabled = false;
                        this.buttonSetTransferFilter.Enabled = false;
                    }
                    else
                    {
                        if (this._StateOfCacheData != StateOfData.NoAccess)
                        {
                            this.buttonTransferDatabaseToCache.Enabled = true;
                            this.buttonSetTransferFilter.Enabled = true;
                        }
                        else
                        {
                            this.buttonTransferDatabaseToCache.Enabled = false;
                            this.buttonSetTransferFilter.Enabled = false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Transfer

        private void buttonTransferDatabaseToCache_Click(object sender, EventArgs e)
        {
            if (this.TransferFilterIsOK())
            {
                if (this.EmbargoDate() != null)
                {
                    System.DateTime Embargo = (System.DateTime)this.EmbargoDate();
                    string Message = "Embargo until " + Embargo.ToString("yyyy-MM-dd") + "\r\nAre you sure that you want to publish these data";
                    if (System.Windows.Forms.MessageBox.Show(Message, "Embargo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }
                this.TransferDataToCacheDB(null);
            }
            else
                this.buttonSetTransferFilter_Click(null, null);
        }

        public string TransferDataToCacheDB(InterfaceCacheDatabase iCacheDB)
        {
            if (this.EmbargoDate() != null && DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
            {
                System.DateTime Embargo = (System.DateTime)this.EmbargoDate();
                string EmbargoMessage = "Embargo until " + Embargo.Year.ToString() + "-" + Embargo.Month.ToString() + "-" + Embargo.Day.ToString();
                return EmbargoMessage;
            }

            // only for test
            //System.Collections.Generic.Dictionary<string, string> D = this.MetaData();

            string Error = "";
            string Message = "";
            this._TransferHistory.Clear();
            bool DataHaveBeenTransferred = false;
            if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly 
                && !DiversityCollection.CacheDatabase.CacheDB.SchedulerPlanedTimeReached("ProjectPublished", "", this._ProjectID, null /*this._TargetID*/, ref Error, ref Message))
            {
                CacheDB.LogEvent(this.Name.ToString(), "TransferDataToCacheDB(InterfaceCacheDatabase iCacheDB)", "Scheduler planed time not reached; " + Error);
                return Error;
            }

            string Report = "";
            string Errors = "";
            try
            {
                string TransferActiv = DiversityCollection.CacheDatabase.CacheDB.SetTransferActive(
                    "ProjectPublished",
                    "",
                    this._TargetID,
                    this._ProjectID,
                    "",
                    false);
                if (TransferActiv.Length > 0)
                {
                    CacheDB.LogEvent(this.Name.ToString(), "TransferDataToCacheDB(InterfaceCacheDatabase iCacheDB)", "Competing transfer active: " + TransferActiv);
                    return "";
                }

                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    CacheDB.LogEvent(this.Name.ToString(), "TransferDataToCacheDB(InterfaceCacheDatabase iCacheDB)", "Getting transfer steps");

                System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps = this.TransferSteps(false);// new List<TransferStep>();

                if (this.CompareLogDateForCacheDB)// this._FilterProjectSourceForUpdate)
                {
                    if (this.StateOfCacheData() != StateOfData.SourceUpdated) // this.DataAreUpdatedInSource(TransferSteps))
                    {
                        if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        {
                            System.Windows.Forms.MessageBox.Show("Data are up to date. No transfer needed");
                        }
                        Report = "Data are up to date. No data transferred";
                        if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        {
                            CacheDB.LogEvent(this.Name.ToString(), "TransferDataToCacheDB(bool AutoStart)", Report);
                            System.Collections.Generic.Dictionary<string, object> DataTransfer = new Dictionary<string, object>();
                            DataTransfer.Add(Report, "0");
                            DiversityCollection.CacheDatabase.CacheDB.WriteProjectTransferHistory(CacheDB.HistoryTarget.DataToCache, this._ProjectID, this.Schema(), DataTransfer);
                        }
                        return Report;
                    }
                }
                DiversityCollection.CacheDatabase.FormCacheDatabaseTransfer f = new FormCacheDatabaseTransfer(TransferSteps);//, DiversityCollection.CacheDatabase.CacheDB.ProcessOnly);
                f.setProjectID(this._ProjectID);
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                {
                    Report = f.Report();
                    CacheDB.LogEvent(this.Name.ToString(), "TransferDataToCacheDB(bool AutoStart)", "Report: " + Report);
                    Errors = f.Errors();
                    if (Errors.Length > 0)
                        CacheDB.LogEvent(this.Name.ToString(), "TransferDataToCacheDB(bool AutoStart)", "Errors: " + Errors);
                    else
                        DataHaveBeenTransferred = true;
                    if (f.DataHaveBeenTransferred)
                    {
                        DiversityCollection.CacheDatabase.CacheDB.WriteProjectTransferHistory(CacheDB.HistoryTarget.DataToCache, this._ProjectID, this.Schema(), f.TransferHistory);
                    }
                    else
                    {
                        DiversityCollection.CacheDatabase.CacheDB.WriteProjectTransferHistory(CacheDB.HistoryTarget.DataToCache, this._ProjectID, this.Schema(), f.TransferHistory);
                    }
                    f.Dispose();
                }
                else
                {
                    try
                    {
                        f.setHelp("Cache database transfer");
                        f.ShowDialog();
                        DiversityCollection.CacheDatabase.CacheDB.SetTransferFinished(
                            "ProjectPublished",
                            "",
                            null,
                            this._ProjectID,
                            "",
                            Report,
                            Errors,
                            f.DataHaveBeenTransferred);
                        if (f.DataHaveBeenTransferred)
                        {
                            //this._TransferHistory = f.TransferHistory;
                            //this.WriteHistory(HistoryTarget.DataToCache);
                            DiversityCollection.CacheDatabase.CacheDB.WriteProjectTransferHistory(CacheDB.HistoryTarget.DataToCache, this._ProjectID, this.Schema(), f.TransferHistory);
                            if (f.AllDataHaveBeenTransferred)
                                this._StateOfCacheData = StateOfData.TargetUpToDate;
                            else
                                this._StateOfCacheData = StateOfData.PartialUpToDate;
                            this._StateOfPostgresData = StateOfData.SourceUpdated;
                            this.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
                        }
                        this.initCacheDB();
                        Errors = f.Errors();
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                DiversityCollection.CacheDatabase.CacheDB.SetTransferFinished(
                    "ProjectPublished",
                    "",
                    null,
                    this._ProjectID,
                    "",
                    Report,
                    Errors,
                    DataHaveBeenTransferred);
            }
            return Report;
        }

        private bool TransferFilterIsOK()
        {
            bool OK = true;
            string Result = "";

            string SQL = "SELECT COUNT(*) FROM [" + this.Schema() + "].ProjectTaxonomicGroup";
            Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result == "0" && System.Windows.Forms.MessageBox.Show("No taxonomic groups have been selected for the transfer.\r\nDo you want to add taxonomic groups to be transferred?", "Add taxa?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return false;

            SQL = "SELECT COUNT(*)  FROM [" + this.Schema() + "].ProjectMaterialCategory";
            if (Result == "0" && System.Windows.Forms.MessageBox.Show("No material categories have been selected for the transfer.\r\nDo you want to add material categories to be transferred?", "Add material?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return false;

            SQL = "SELECT COUNT(*)  FROM [" + this.Schema() + "].CacheLocalisationSystem";
            if (Result == "0" && System.Windows.Forms.MessageBox.Show("No localisation systems have been selected for the transfer.\r\nDo you want to add localisation systems to be transferred?", "Add localisation systems?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return false;

            return OK;
        }

        private void buttonSetTransferFilter_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityCollection.CacheDatabase.FormTransferFilter f = new FormTransferFilter(this.Schema(), this._ProjectID);
            f.setHelp("Cache database restrictions");
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();
        }
        
        #endregion

        private void buttonViewCacheContent_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(false, this.Schema());
            f.setHelp("Cache database projects");
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();
        }

        private void buttonRemoveProject_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to remove the project " + this._Project + "?", "Remove project " + this._Project + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            string Message = "";
            bool OK = true;
            OK = this.RemoveSchema(ref Message);
            string SQL = "";
            if (OK && Message.Length == 0)
            {
                SQL = "DELETE P FROM ProjectPublished P WHERE ProjectID = " + this._ProjectID.ToString();
                if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    OK = false;
            }
            if (Message.Length > 0 || !OK)
            {
                System.Windows.Forms.MessageBox.Show("Deleting schema " + this.Schema() + " failed:\r\n" + Message);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Schema " + this.Schema() + " deleted");
                SQL = "SELECT COUNT(*) FROM ProjectPublished";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "0")
                    this._I_CacheDatabase.initAnonymCollector();
            }
            DiversityCollection.CacheDatabase.CacheDB.ResetDtProjects();
            this._I_CacheDatabase.initProjects();
        }

        private bool RemoveSchema(ref string Message)
        {
            bool OK = true;

            string SQL = "select o.name, case when o.type = 'U' then 'Z' else o.type end as type from sys.objects O, sys.schemas S " +
                "where O.schema_id = s.schema_id " +
                "and S.name = '" + this.Schema() + "' " +
                "order by case when o.type = 'U' then 'Z' else o.type end";
            System.Data.DataTable dt = new DataTable();
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message) && Message.Length == 0)
            {
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Type = R["type"].ToString().ToUpper().Trim();
                    SQL = "";
                    switch (Type)
                    {
                        // nach Info von Toni nicht notwendig
                        //case "C":
                        //case "D":
                        case "F":
                            //case "PK":
                            SQL = "DROP CONSTRAINT ";
                            break;
                        case "FN":
                            SQL = "DROP FUNCTION ";
                            break;
                        case "P":
                            SQL = "DROP PROCEDURE  ";
                            break;
                        case "R":
                            SQL = "DROP RULE ";
                            break;
                        case "TR":
                            SQL = "DROP TRIGGER ";
                            break;
                        case "V":
                            SQL = "DROP VIEW ";
                            break;
                        case "Z":
                            SQL = "DROP TABLE ";
                            break;
                    }
                    if (SQL.Length > 0)
                    {
                        SQL += "[" + this.Schema() + "].[" + R["name"].ToString() + "]";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                            OK = false;
                    }
                }
            }
            if (OK && Message.Length == 0)
            {
                SQL = "DROP SCHEMA [" + this.Schema() + "]";
                if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                {
                    OK = false;
                }
            }
            return OK;
        }

        #region Compare the dates

        private bool DataAreUpdatedInSource(System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps)
        {
            bool DataAreUpdated = true;
            System.DateTime LastTransferToCache = System.DateTime.Now;
            string SQL = "SELECT LastUpdatedWhen FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
            if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastTransferToCache))
            {
                DataAreUpdated = false;
                foreach (DiversityCollection.CacheDatabase.TransferStep TS in TransferSteps)
                {
                    if (TS.TableName() == "CacheProjectReference")
                        continue;
                    System.DateTime LastUpdateOfStep = TS.LastUpdateInSource();
                    if (LastTransferToCache.CompareTo(LastUpdateOfStep) < 0)
                    {
                        DataAreUpdated = true;
                        break;
                    }
                }
            }
            return DataAreUpdated;
        }

        private void buttonProjectTransferToCacheDBFilter_Click(object sender, EventArgs e)
        {
            bool DoCompare = this.CompareLogDateForCacheDB;
            if (DoCompare)//this._FilterProjectSourceForUpdate)
            {
                this.toolTip.SetToolTip(this.buttonProjectTransferToCacheDBFilter, "Data not filtered for updates");
                this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                this.toolTip.SetToolTip(this.buttonProjectTransferToCacheDBFilter, "Data filtered for updates later than last transfer");
                this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.Filter;
            }
            this.CompareLogDateForCacheDB = !DoCompare;// this._FilterProjectSourceForUpdate = !this._FilterProjectSourceForUpdate;
        }

        private void viewDifferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.StateOfCacheData() == StateOfData.SourceUpdated)
            {
                string SQL = "SELECT CASE WHEN S.[AccessionNumber] IS NULL THEN 'ID: ' + cast(S.CollectionSpecimenID as varchar) ELSE S.[AccessionNumber] END AS [AccessionNumber or ID], " +
                    "convert(varchar(20), S.LogUpdatedWhen, 120) AS [Last changes], " +
                    "convert(varchar(20), PP.LastUpdatedWhen, 120) AS [Last transfer] " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P, " +
                     "[" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[CollectionSpecimen] S, " +
                     "[" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[CollectionProject] CP, " +
                    "[ProjectPublished] PP " +
                    "WHERE P.ProjectID = PP.ProjectID " +
                    "AND CP.ProjectID = P.ProjectID " +
                    "AND S.CollectionSpecimenID = CP.CollectionSpecimenID " +
                    "AND S.LogUpdatedWhen > PP.LastUpdatedWhen " +
                    "AND P.ProjectID = " + this._ProjectID.ToString() +
                    " ORDER BY AccessionNumber";
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                if (dt.Rows.Count > 0)
                {
                    DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent("Changed data", "Datasets with changes after the last transfer", dt);
                    f.Width = 380;
                    f.setDataGridViewAutoSizeColumnMode(DataGridViewAutoSizeColumnMode.AllCells);
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog();
                }
                else
                    System.Windows.Forms.MessageBox.Show("No changes after the last transfer found");
            }
            else
                System.Windows.Forms.MessageBox.Show("State of the cache data:\r\n" + this.StateOfCacheData().ToString());


            return;
            // old version comparing the dates of the single steps
            System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps = this.TransferSteps(false);
            if (!this.DataAreUpdatedInSource(TransferSteps))
            {
                System.Windows.Forms.MessageBox.Show("No differences depending on dates found");
            }
            else
            {
                System.DateTime LastTransferToCache = System.DateTime.Now;
                string SQL = "SELECT LastUpdatedWhen FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
                string Message = "";
                if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastTransferToCache))
                {
                    foreach (DiversityCollection.CacheDatabase.TransferStep TS in TransferSteps)
                    {
                        System.DateTime LastUpdateOfStep = TS.LastUpdateInSource();
                        if (LastTransferToCache.CompareTo(LastUpdateOfStep) < 0)
                        {
                            if (Message.Length > 0) Message += "\r\n";
                            string Table = TS.TableName();
                            Message += LastUpdateOfStep.ToShortDateString() + "  <->  ";
                            Message += LastTransferToCache.ToShortDateString()+ ":\t";
                            Message += Table;
                        }
                    }
                }
                if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show("Differences (dates) found:\r\n\r\nsource DB\tcache DB\tTable:\r\n\r\n" + Message);
                else
                    System.Windows.Forms.MessageBox.Show("Failed to find the differences");
            }

        }

        private bool CompareLogDateForCacheDB
        {
            get
            {
                string SQL = "SELECT CompareLogDate FROM ProjectPublished WHERE ProjectID = " + this._ProjectID.ToString();
                bool DoCompare = false;
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                bool.TryParse(Result, out DoCompare);
                return DoCompare;
            }
            set
            {
                string SQL = "UPDATE P SET P.CompareLogDate = ";
                if (value)
                    SQL += "1";
                else
                    SQL += "0";
                SQL += " FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                if (value) this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.Filter;
                else this.buttonProjectTransferToCacheDBFilter.Image = DiversityCollection.Resource.FilterClear;
            }
        }
        
        #endregion

        #region Schedule based transfer and protocol

        private void buttonIncludeInTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormScheduleTransferSettings f = new FormScheduleTransferSettings("ProjectPublished", this._Project, this._ProjectID);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.initScheduleControls();
                    this.initCacheDBControls();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void initScheduleControls()
        {
            try
            {
                DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(
                    "ProjectPublished",
                    "",
                    this._ProjectID,
                    this.checkBoxIncludeInTransfer,
                    this.buttonProjectTransferToCacheDBFilter,
                    this.buttonIncludeInTransfer,
                    this.buttonTransferProtocol,
                    this.buttonTransferErrors,
                    this.buttonTransferDatabaseToCache,
                    this.labelLastUpdateInCacheDatabase,
                    this.toolTip);
            }
            catch (Exception ex)
            {
            }
        }

        public bool IncludeInTransfer() { return this.checkBoxIncludeInTransfer.Checked; }

        public bool IncludePostgresInTransfer() { return this.checkBoxPostgresIncludeInTransfer.Checked; }

        private void checkBoxIncludeInTransfer_Click(object sender, EventArgs e)
        {
            string SQL = "UPDATE P SET P.IncludeInTransfer = ";
            if (this.checkBoxIncludeInTransfer.Checked)
                SQL += "1";
            else SQL += "0";
            SQL += " FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            this.buttonIncludeInTransfer.Enabled = this.checkBoxIncludeInTransfer.Checked;
        }

        private void buttonTransferProtocol_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.TransferProtocol FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer protocol for project " + this.labelProject.Text, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No protocol found");
        }

        private void buttonTransferErrors_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.TransferErrors FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer errors for project " + this.labelProject.Text, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No errors found");
        }

        #endregion

        private bool DataAreUpdatedInCache()
        {
            bool DataAreUpdated = true;
            System.DateTime LastTransferToCache = System.DateTime.Now;
            string SQL = "SELECT LastUpdatedWhen FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
            if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastTransferToCache))
            {
                System.DateTime LastTransferToPostgres = System.DateTime.Now;
                SQL = "SELECT to_char(MAX(\"LogLastTransfer\"), 'YYYY-MM-DD HH24:MI:SS') AS LogLastTransfer FROM \"" + this.Schema() + "\".\"CacheMetadata\"";
                if (System.DateTime.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out LastTransferToPostgres))
                {
                    if (LastTransferToCache.CompareTo(LastTransferToPostgres) < 0)
                        DataAreUpdated = false;
                }
            }
            else
            {

            }
            return DataAreUpdated;
        }

        private void buttonProjectTargets_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormPostgresTargets f = new FormPostgresTargets(this._ProjectID, this._Project);
                f.Width = this.Width;
                if (!f.IsDisposed)
                    f.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }

        public System.Collections.Generic.Dictionary<string, string> MetaData()
        {
            System.Collections.Generic.Dictionary<string, string> Data = new Dictionary<string, string>();
            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
            Data = P.Metadata((int)this._ProjectID);
            return Data;
        }

        #endregion

        #region Postgres

        #region Parameter and properties

        private DiversityCollection.CacheDatabase.Project _PostgresProject;

        public DiversityCollection.CacheDatabase.Project PostgresProject
        {
            get
            {
                if (this._PostgresProject == null && DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                    this._PostgresProject = new Project(this.Project, DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase());
                return _PostgresProject;
            }
            //set { _PostgresProject = value; }
        }
        
        #endregion

        #region init the controls

        public void resetPostgresControls()
        {
            try
            {
                if(this._PostgresProject != null)
                    this._PostgresProject.ProjectID = null;
            }
            catch (System.Exception ex)
            {
            }
        }

        public void setPostgesProject(int ProjectID, string Project)
        {
            if (this._PostgresProject.ProjectID == null)
            {
                this._PostgresProject.ProjectID = ProjectID;
                this._PostgresProject.SetSchemaName(Project);
                //this.Project = Project;
            }
        }

        public void initPostgresControls(bool DBneedsUpdate)
        {
            try
            {
                //this.initPostgresForFileTransfer();
                //this.checkBoxPostgresUseBulkTransfer.Checked = this.UseBulkTransfer;
                if (this.UseBulkTransfer)
                {
                    this.buttonTransferCacheToPostgres.Visible = false;
                    this.buttonTransferCacheToPostgres.Dock = DockStyle.Left;
                    this.buttonTransferCacheToPostgresFile.Visible = true;
                    this.buttonTransferCacheToPostgresFile.Dock = DockStyle.Fill;
                    this.toolTip.SetToolTip(this.buttonTransferCacheToPostgresFile, "Transfer data via file transfer in directory " + DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory);
                }
                else
                {
                    this.buttonTransferCacheToPostgresFile.Visible = false;
                    this.buttonTransferCacheToPostgresFile.Dock = DockStyle.Right;
                    this.buttonTransferCacheToPostgres.Visible = true;
                    this.buttonTransferCacheToPostgres.Dock = DockStyle.Fill;
                }

                this.checkBoxPostgresUseBulkTransfer.Enabled = DiversityCollection.CacheDatabase.CacheDB.CanUseBulkTransfer;
                if (DiversityCollection.CacheDatabase.CacheDB.CanUseBulkTransfer)
                    this.checkBoxPostgresUseBulkTransfer.Checked = this.UseBulkTransfer;
                else
                    this.checkBoxPostgresUseBulkTransfer.Checked = false;

                this._PostgresDatabaseNeedsUpdate = DBneedsUpdate;
                this.labelNumberInPostgresDB.Text = "";
                this.labelLastUpdateInPostgresDatabase.Text = "";
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    if (this.PostgresProject.ProjectID != null)
                    {
                        bool IsMissingInSource = false;
                        if (MissingInCacheDB() || MissingInDB())
                        {
                            IsMissingInSource = true;
                            this.checkBoxPostgresIncludeInTransfer.Enabled = false;
                        }
                        this.buttonPostgresEstablishProject.Enabled = true;// IsMissingInSource;
                        this.buttonPostgresEstablishProject.Image = this.imageListButtonIcons.Images[1];
                        this.buttonPostgresEstablishProject.Tag = "Drop";
                        this.toolTip.SetToolTip(this.buttonPostgresEstablishProject, "Delete " + this.Schema() + " from " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name);
                        this.buttonAdministratePackages.Enabled = true;
                        this.listBoxPackages.Enabled = true;
                        this.buttonTransferCacheToPostgres.Enabled = false;// true;
                        this.buttonTransferCacheToPostgresFile.Enabled = false;
                        //this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                        //this.checkBoxPostgresUseBulkTransfer.Checked = false;

                        this.buttonViewPostgresContent.Enabled = true;
                        this.buttonHistoryInPostgres.Enabled = true;
                        string Schema = this.Schema();
                        if (Schema.Length == 0)
                            Schema = this._PostgresProject.SchemaName;
                        this.UpdateCheckPostgresProject(Schema);
                        bool NotEstablished = false;
                        if (this.buttonUpdatePostgres.Text == "Not established")
                            NotEstablished = true;
                        if (NotEstablished)
                        {

                        }
                        else
                        {

                            string SQL = "SELECT COUNT(*) FROM \"" + Schema + "\".\"CacheCollectionSpecimen\"";
                            string Message = "";
                            string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
                            if (Result != null && Result != "0" && Result.Length > 0)
                            {
                                this.labelNumberInPostgresDB.Text = Result + " datasets";
                                SQL = "SELECT to_char(MAX(\"LogLastTransfer\"), 'YYYY-MM-DD HH24:MI:SS') AS LogLastTransfer FROM \"" + Schema + "\".\"CacheMetadata\"; ";
                                this.labelLastUpdateInPostgresDatabase.Text = "Last transfer:\r\n" + DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                            }
                            else
                            {
                                this.labelNumberInPostgresDB.Text = "No data";
                            }
                            // init Packages if getting previous infos did not return an error
                            if (Message.Length == 0)
                            this.initPackages(Schema);
                            this.initOtherTargets();
                            // Markus 28.3.25: Check count. Issue #36
                            string CurrentTransfer = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(
                                "SELECT TransferIsExecutedBy " +
                                "FROM dbo.ProjectTarget AS P, [Target] T " +
                                "WHERE P.TargetID = T.TargetID " +
                                "AND P.ProjectID = " + this._ProjectID.ToString() +
                                " AND T.TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString(), true);
                            if (CurrentTransfer != null && CurrentTransfer.Length > 0)
                            {
                                this.buttonTransferCacheToPostgres.Enabled = false;
                                this.buttonTransferCacheToPostgres.BackColor = System.Drawing.Color.Red;
                                this.buttonTransferCacheToPostgresFile.Enabled = false;
                                this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                                this.checkBoxPostgresUseBulkTransfer.Checked = false;

                                this.buttonTransferCacheToPostgresFile.BackColor = System.Drawing.Color.Red;
                                this.labelLastUpdateInPostgresDatabase.Text = "Competing transfer active: " + CurrentTransfer;
                                this.labelLastUpdateInPostgresDatabase.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                switch (this.StateOfPostgresData())
                                {
                                    case StateOfData.SourceUpdated:
                                        this.buttonTransferCacheToPostgres.FlatAppearance.BorderSize = 4;
                                        this.buttonTransferCacheToPostgres.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                                        this.buttonTransferCacheToPostgresFile.FlatAppearance.BorderSize = 4;
                                        this.buttonTransferCacheToPostgresFile.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                                        break;
                                    case StateOfData.PartialUpToDate:
                                        this.buttonTransferCacheToPostgres.FlatAppearance.BorderSize = 1;
                                        this.buttonTransferCacheToPostgres.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                                        this.buttonTransferCacheToPostgresFile.FlatAppearance.BorderSize = 1;
                                        this.buttonTransferCacheToPostgresFile.FlatAppearance.BorderColor = System.Drawing.Color.Red;
                                        break;
                                    case StateOfData.TargetUpToDate:
                                    default:
                                        this.buttonTransferCacheToPostgres.FlatAppearance.BorderSize = 0;
                                        this.buttonTransferCacheToPostgresFile.FlatAppearance.BorderSize = 0;
                                        break;
                                }
                                this.buttonTransferCacheToPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
                                this.buttonTransferCacheToPostgresFile.BackColor = System.Drawing.Color.LightSteelBlue;
                                this.labelLastUpdateInPostgresDatabase.ForeColor = System.Drawing.Color.Black;
                            }
                            if (this.CompareLogDateForPostgresDB)
                                this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.Filter;
                            else
                                this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.FilterClear;
                        }

                    }
                    else if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString() != null &&
                        DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().IndexOf(";Database=postgres") > -1)
                    {
                        this.labelNumberInPostgresDB.Text = "";
                        this.buttonPostgresEstablishProject.Image = this.imageListButtonIcons.Images[0];
                        this.buttonPostgresEstablishProject.Tag = "";
                        this.toolTip.SetToolTip(this.buttonPostgresEstablishProject, "");
                        this.buttonPostgresEstablishProject.Enabled = false;
                        this.buttonAdministratePackages.Enabled = false;
                        this.listBoxPackages.Enabled = false;
                        this.buttonTransferCacheToPostgres.Enabled = false;
                        this.buttonTransferCacheToPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
                        this.buttonTransferCacheToPostgresFile.Enabled = false;
                        this.buttonTransferCacheToPostgresFile.BackColor = System.Drawing.Color.LightSteelBlue;
                        this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                        this.checkBoxPostgresUseBulkTransfer.Checked = false;

                        this.buttonUpdatePostgres.Enabled = false;
                        this.buttonUpdatePostgres.Text = "";
                        this.buttonViewPostgresContent.Enabled = false;
                        this.buttonHistoryInPostgres.Enabled = false;
                        this.listBoxPackages.DataSource = null;
                    }
                    else
                    {
                        this.labelNumberInPostgresDB.Text = "Not established";
                        this.buttonPostgresEstablishProject.Image = this.imageListButtonIcons.Images[0];
                        this.buttonPostgresEstablishProject.Tag = "Establish";
                        this.toolTip.SetToolTip(this.buttonPostgresEstablishProject, "Establish " + this.Schema() + " from " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name);
                        this.buttonAdministratePackages.Enabled = false;
                        this.listBoxPackages.Enabled = false;
                        this.buttonTransferCacheToPostgres.Enabled = false;
                        this.buttonTransferCacheToPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
                        this.buttonTransferCacheToPostgresFile.Enabled = false;
                        this.buttonTransferCacheToPostgresFile.BackColor = System.Drawing.Color.LightSteelBlue;
                        this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                        this.checkBoxPostgresUseBulkTransfer.Checked = false;

                        this.buttonUpdatePostgres.Enabled = false;
                        this.buttonUpdatePostgres.Text = "";
                        this.buttonViewPostgresContent.Enabled = false;
                        this.buttonHistoryInPostgres.Enabled = false;
                        this.listBoxPackages.DataSource = null;
                        bool IsCacheDatabase = true;
                        try
                        {
                            string sql = "SELECT public.diversityworkbenchmodule();";
                            string Message = "";
                            string result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(sql, ref Message);
                            if (result != null && result != "DiversityCollectionCache")
                                IsCacheDatabase = false;
                        }
                        catch (System.Exception ex)
                        {
                            IsCacheDatabase = false;
                        }
                        if (IsCacheDatabase)
                        {
                            this.buttonPostgresEstablishProject.Enabled = true;
                            this.initOtherTargets(); // Nach Toni 05.02.2021, 10:07
                        }
                        else
                            this.buttonPostgresEstablishProject.Enabled = false;
                    }
                    if (DBneedsUpdate)
                    {
                        if (MissingInCacheDB() || MissingInDB())
                        {
                            this.buttonPostgresEstablishProject.Enabled = true;
                            this.buttonViewPostgresContent.Enabled = true;
                            this.buttonHistoryInPostgres.Enabled = true;
                        }
                        else
                        {
                            this.buttonPostgresEstablishProject.Enabled = false;
                            this.buttonViewPostgresContent.Enabled = false;
                            this.buttonHistoryInPostgres.Enabled = false;
                        }
                        this.listBoxPackages.Enabled = false;
                        this.buttonTransferCacheToPostgres.Enabled = false;
                        this.buttonTransferCacheToPostgresFile.Enabled = false;
                        this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                        this.checkBoxPostgresUseBulkTransfer.Checked = false;

                        this.buttonUpdatePostgres.Enabled = false;
                        this.buttonUpdatePostgres.Text = "";
                    }
                }
                else
                {
                    this.buttonPostgresEstablishProject.Enabled = false;
                    this.buttonAdministratePackages.Enabled = false;
                    this.listBoxPackages.Enabled = false;
                    this.buttonTransferCacheToPostgres.Enabled = false;
                    this.buttonTransferCacheToPostgresFile.Enabled = false;
                    this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                    this.checkBoxPostgresUseBulkTransfer.Checked = false;

                    this.buttonUpdatePostgres.Enabled = false;
                    this.buttonUpdatePostgres.Text = "";
                    this.buttonViewPostgresContent.Enabled = false;
                    this.buttonHistoryInPostgres.Enabled = false;
                    this.buttonProjectTransferToPostgresFilter.Enabled = false;
                    this.labelNumberInPostgresDB.Text = "Not connected";
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.initPostgresScheduleControls();
        }

        private StateOfData _StateOfPostgresData = StateOfData.Unknown;
        private StateOfData StateOfPostgresData()
        {
            if (this._StateOfPostgresData == StateOfData.Unknown)
            {
                try
                {
                    System.DateTime LastTransferToCache = System.DateTime.Now;
                    string SQL = "SELECT LastUpdatedWhen FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
                    // Markus 28.3.25: Check existence. Issue #36
                    if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, true).ToString(), out LastTransferToCache))
                    {
                        System.DateTime LastTransferToPostgres = System.DateTime.Now;
                        SQL = "SELECT to_char(MAX(\"LogLastTransfer\"), 'YYYY-MM-DD HH24:MI:SS') AS LogLastTransfer FROM \"" + this.Schema() + "\".\"CacheMetadata\"";
                        string Message = "";
                        if (System.DateTime.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message), out LastTransferToPostgres))
                        {
                            if (LastTransferToCache.CompareTo(LastTransferToPostgres) < 0)
                            {
                                this._StateOfPostgresData = StateOfData.TargetUpToDate;
                            }
                            else
                                this._StateOfPostgresData = StateOfData.SourceUpdated;
                        }
                        else
                            this._StateOfPostgresData = StateOfData.NoDataInTarget;
                    }
                    else
                    {
                        this._StateOfPostgresData = StateOfData.MissingInSource;
                    }
                }
                catch (System.Exception ex)
                {
                    _StateOfPostgresData = StateOfData.Unknown;
                }
            }
            return this._StateOfPostgresData;
        }
        #endregion

        #region Missing

        private bool _MissingInCacheDB = false;
        public bool MissingInCacheDB()
        {
            return this._MissingInCacheDB;
        }

        public void setMissingInCacheDB()
        {
            this._MissingInCacheDB = true;
            if (this._MissingInCacheDB)
            {
                this.buttonRemoveProject.Visible = false;

                this.buttonIncludeInTransfer.Enabled = false;
                //this.buttonPostgresEstablishProject.Enabled = false;
                this.buttonPostgresTransferSettings.Enabled = false;
                this.buttonProjectTransferToCacheDBFilter.Enabled = false;
                this.buttonSetTransferFilter.Enabled = false;
                this.buttonTransferDatabaseToCache.Enabled = false;
                this.buttonTransferCacheToPostgres.Enabled = false;
                this.buttonTransferCacheToPostgresFile.Enabled = false;
                this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                this.checkBoxPostgresUseBulkTransfer.Checked = false;

                this.buttonUpdateCache.Enabled = false;
                this.buttonUpdateCache.Text = "";
                this.buttonViewCacheContent.Enabled = false;
                this.buttonViewCacheContent.Text = "";
                this.checkBoxIncludeInTransfer.Enabled = false;
                this.buttonTransferProtocol.Enabled = false;
                this.buttonTransferErrors.Enabled = false;

                this.labelLastUpdateInCacheDatabase.Text = "Missing";
                this.labelNumberInCacheDB.Text = "";
                this.checkBoxPostgresIncludeInTransfer.Enabled = false;

                this.buttonViewPostgresContent.Enabled = true;
                this.buttonHistoryInPostgres.Enabled = true;

                this.buttonPostgresEstablishProject.Enabled = true;
            }
        }

        private bool _MissingInDB = false;
        
        public bool MissingInDB()
        {
            return this._MissingInDB;
        }

        public void setMissingInDB(string Project)
        {
            this._MissingInDB = true;
            if (this._MissingInDB)
            {
                this.labelNumberInDatabase.Text = "";
                this.labelLastUpdateInDatabase.Text = "Missing";
                this.labelDatawithholding.Text = "";
                this.buttonDatawithholding.Enabled = false;
                this.labelProject.Text = Project;
            }
        }

        #endregion

        #region Establish and remove project

        public bool PostgresEstablishProject()
        {
            bool Establihed = true;
            try
            {
                string Message = "";
                string SQL = "SET ROLE \"CacheAdmin\"; CREATE SCHEMA  \"" + this.Schema() + "\"" +
                    "AUTHORIZATION \"CacheAdmin\";";
                // Create the Schema
                if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) // .Postgres.PostgresExecuteSqlNonQuery(SQL))
                {
                    if (Message.Contains("42P06"))
                    //    if (Message.IndexOf("42P06: ") > -1 && Message.ToLower().IndexOf(" schema") > -1)
                    //    Establihed = true;
                    //else if ((Message.StartsWith("42P06: schema") || Message.IndexOf("42P06: Schema") > -1) && (Message.EndsWith("already exists") || Message.EndsWith("existiert bereits")))
                        Establihed = true;
                    else
                    {
                        Establihed = false;
                        System.Windows.Forms.MessageBox.Show("Could not establish project:\r\n" + Message);
                    }
                }
                if (Establihed)
                {
                    SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + this.Schema() + "\"" +
                        "GRANT EXECUTE ON FUNCTIONS " +
                        "TO \"CacheUser\";";
                    Message = "";
                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) // .Postgres.PostgresExecuteSqlNonQuery(SQL))
                    {
                        Establihed = false;
                    }
                    else
                    {
                        SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + this.Schema() + "\"" +
                            "GRANT SELECT ON TABLES " +
                            "TO \"CacheUser\";";
                        Message = "";
                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) //.Postgres.PostgresExecuteSqlNonQuery(SQL))
                            Establihed = false;
                        else
                        {
                            SQL = "CREATE OR REPLACE FUNCTION \"" + this.Schema() + "\".version() " +
                                "RETURNS integer AS " +
                                "$BODY$ " +
                                "declare " +
                                "v integer; " +
                                "BEGIN " +
                                "SELECT 0 into v; " +
                                "RETURN v; " +
                                "END; " +
                                "$BODY$ " +
                                "LANGUAGE plpgsql STABLE " +
                                "COST 100; " +
                                "ALTER FUNCTION \"" + this.Schema() + "\".version() " +
                                "OWNER TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this.Schema() + "\".version() TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this.Schema() + "\".version() TO \"CacheUser\"";
                            if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL)) //.Postgres.PostgresExecuteSqlNonQuery(SQL))
                                Establihed = false;

                            SQL = "CREATE OR REPLACE FUNCTION \"" + this.Schema() + "\".projectid() " +
                                "RETURNS integer AS " +
                                "$BODY$ " +
                                "declare " +
                                "v integer; " +
                                "BEGIN " +
                                "SELECT " + this._ProjectID.ToString() + " into v; " +
                                "RETURN v; " +
                                "END; " +
                                "$BODY$ " +
                                "LANGUAGE plpgsql STABLE " +
                                "COST 100; " +
                                "ALTER FUNCTION \"" + this.Schema() + "\".projectid() " +
                                "OWNER TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this.Schema() + "\".projectid() TO \"CacheAdmin\"; " +
                                "GRANT EXECUTE ON FUNCTION \"" + this.Schema() + "\".projectid() TO \"CacheUser\"";
                            if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL)) //.Postgres.PostgresExecuteSqlNonQuery(SQL))
                                Establihed = false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            { }
            return Establihed;
        }

        private void buttonPostgresEstablishProject_Click(object sender, EventArgs e)
        {
            if (this.buttonPostgresEstablishProject.Tag.ToString() == "Drop")
            {
                this.PostgresDropProject();
            }
            else
            {
                this.PostgresEstablishProject();
            }
            this.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
        }

        public bool PostgresDropProject()
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to remove the project " + this._Project + "?", "Remove project " + this._Project + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return false;

            bool OK = true;
            // Remove all views and tables except the package tables
            string SQL = "select T.Table_name, T.table_type from information_schema.Tables T " +
                "where T.table_schema = '" + this.Schema() + "' and T.table_name not like 'Package%'" +
                "order by T.table_type desc";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message) && Message.Length == 0)
            {
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Type = R["table_type"].ToString().ToUpper().Trim();
                    SQL = "";
                    switch (Type)
                    {
                        case "BASE TABLE":
                            SQL = "DROP TABLE ";
                            break;
                        case "VIEW":
                            SQL = "DROP VIEW ";
                            break;
                    }
                    if (SQL.Length > 0)
                    {
                        SQL += "\"" + this.Schema() + "\".\"" + R["Table_name"].ToString() + "\"";
                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                            OK = false;
                    }
                }
            }
            else
            {
                return false;
            }
            // removing the package tables
            SQL = "select T.Table_name, T.table_type from information_schema.Tables T " +
                "where T.table_schema = '" + this.Schema() + "' " +
                "order by T.table_type desc";
            dt.Rows.Clear();
            if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message) && Message.Length == 0)
            {
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Type = R["table_type"].ToString().ToUpper().Trim();
                    SQL = "";
                    switch (Type)
                    {
                        case "BASE TABLE":
                            SQL = "DROP TABLE ";
                            break;
                        case "VIEW":
                            SQL = "DROP VIEW ";
                            break;
                    }
                    if (SQL.Length > 0)
                    {
                        SQL += "\"" + this.Schema() + "\".\"" + R["Table_name"].ToString() + "\"";
                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                            OK = false;
                    }
                }
            }
            else
            {
                return false;
            }

            SQL = "select R.Routine_name from information_schema.routines R " +
                "where R.Routine_schema = '" + this.Schema() + "'";
            System.Data.DataTable dtRoutines = new DataTable();
            if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRoutines, ref Message) && Message.Length == 0)
            {
                foreach (System.Data.DataRow R in dtRoutines.Rows)
                {
                    SQL = "DROP FUNCTION " + "\"" + this.Schema() + "\"." + R["Routine_name"].ToString() + "()";
                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                        OK = false;
                }
            }
            if (OK && Message.Length == 0)
            {
                SQL = "DROP SCHEMA \"" + this.Schema() + "\"";
                if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                {
                    // Revoke grants for all objects in schema public

                // nicht mehr durchgeführt da Rollen aufgelöst

                    //System.Data.DataTable dtTables = new DataTable();
                    //SQL = "SELECT table_name from information_schema.Tables t " +
                    //    "where t.table_schema = 'public'";
                    //DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtTables, ref Message);
                    //foreach (System.Data.DataRow R in dtTables.Rows)
                    //{
                    //    SQL = "Revoke all on TABLE \"public\"." + R[0].ToString() + " from  \"CacheUser\"; "; //_" + this.Schema() + "\"";
                    //    DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                    //    SQL = "Revoke all on TABLE \"public\"." + R[0].ToString() + " from  \"CacheAdmin\"; "; //_" + this.Schema() + "\"";
                    //    DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                    //}
                    //System.Data.DataTable dtFunctions = new DataTable();
                    //SQL = "SELECT routine_name from information_schema.routines t " +
                    //    "where t.routine_schema = 'public'";
                    //foreach (System.Data.DataRow R in dtFunctions.Rows)
                    //{
                    //    SQL = "Revoke EXECUTE on FUNCTION \"public\"." + R[0].ToString() + " from  \"CacheUser\"; "; //_" + this.Schema() + "\"";
                    //    DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                    //    SQL = "Revoke EXECUTE on FUNCTION \"public\"." + R[0].ToString() + " from  \"CacheAdmin\"; "; //_" + this.Schema() + "\"";
                    //    DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                    //}

                    //SQL = "DROP ROLE \"CacheUser_" + this.Schema() + "\"";
                    //string x = "";
                    //DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref x);
                    //SQL = "DROP ROLE \"CacheAdmin\"; "; //_" + this.Schema() + "\""; ;
                    //DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref x);
                }
            }
            if (OK && Message.Length == 0)
            {
                SQL = "DELETE FROM ProjectTarget WHERE ProjectID = " + this._ProjectID.ToString() + " AND TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString(); //'" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            }
            this.PostgresProject.ProjectID = null;
            if (Message.Length > 0 || !OK)
            {
                System.Windows.Forms.MessageBox.Show("Deleting schema " + this.Schema() + " failed:\r\n" + Message);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Schema " + this.Schema() + " deleted");
            }
            return OK;
        }
        
        private void PostgresRemoveProjectData()
        {
            try
            {
                if (DiversityCollection.CacheDatabase.Project.Projects.ContainsKey(this._ProjectID))
                    DiversityCollection.CacheDatabase.Project.Projects[this._ProjectID].ClearProject();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Update

        private void buttonUpdatePostgres_Click(object sender, EventArgs e)
        {
            try
            {
                // Check packages
                if (this.listBoxPackages.Items.Count > 0)
                {
                    string Message = "";
                    foreach (System.Object o in this.listBoxPackages.Items)
                    {
                        if (o.GetType() == typeof(System.Data.DataRowView))
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)o;
                            Message += "\t" + R[0].ToString() + "\r\n";
                        }
                    }
                    Message = this.listBoxPackages.Items.Count.ToString() + " package(s) are installed:\r\n\r\n" +
                        Message +
                        "\r\nThese may interfere with the update of the project.\r\n" +
                        "It is recommended to remove the packages in advance of the update and reinstall them afterwards.\r\n\r\n" +
                        "Do you want to remove the packages first?";
                    if (System.Windows.Forms.MessageBox.Show(Message, "Remove packages", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        return;
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
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdatePGSchema_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> ReplaceStrings = new Dictionary<string, string>();
                    ReplaceStrings.Add("#project#", this.Schema());
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    DiversityWorkbench.Forms.FormUpdateDatabase f =
                        new DiversityWorkbench.Forms.FormUpdateDatabase(
                            DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, // .Postgres.PostgresConnection().Database,
                            DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion.ToString(),
                            con,
                            Versions,
                            DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(),
                            this.Schema(),
                            ReplaceStrings);
                    f.ForPostgres = true;
                    f.ShowInTaskbar = true;
                    f.ShowDialog();
                    //this._Interface.initPostgresAdminProjectLists();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }
                this.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void UpdateCheckPostgresProject(string Schema = "")
        {
            string _Schema = this.Schema();
            if (_Schema.Length == 0)
            {
                _Schema = Schema;
            }
            if (_Schema.Length == 0)
            {
                CacheDB.LogEvent("UserControlPostgres", "UpdateCheckPostgresProject", "Schema is missing");
                return;
            }
            string SQL = "select \"" + _Schema + "\".version()";
            try
            {
                string Message = "";
                int version;
                if (int.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message), out version))
                {
                    if (Message.Length == 0 && version < DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion)
                    {
                        this.buttonUpdatePostgres.Enabled = true;
                        this.buttonTransferCacheToPostgres.Enabled = false;
                        this.buttonTransferCacheToPostgresFile.Enabled = false;
                        this.checkBoxPostgresUseBulkTransfer.Enabled = false;
                        this.checkBoxPostgresUseBulkTransfer.Checked = false;
                        this.buttonUpdatePostgres.Text = "Update v. " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion.ToString();
                        this.buttonUpdatePostgres.Text = "Update v. " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion.ToString();
                        this.toolTip.SetToolTip(this.buttonUpdatePostgres, "Update to version " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBProjectVersion.ToString());
                        this._PostgresProjectNeedsUpdate = true;
                    }
                    else
                    {
                        this.buttonUpdatePostgres.Enabled = false;
                        this.buttonUpdatePostgres.Text = "Version " + version.ToString();
                        this.buttonTransferCacheToPostgres.Enabled = true;
                        this.buttonTransferCacheToPostgresFile.Enabled = true;
                        this.checkBoxPostgresUseBulkTransfer.Enabled = CacheDB.CanUseBulkTransfer;// !CacheDB.IsSqlServerExpress;
                    }
                }
                else
                {
                    this.buttonUpdatePostgres.Enabled = false;
                    this.buttonUpdatePostgres.Text = "Not established";
                    this.buttonTransferCacheToPostgres.Enabled = false;
                    this.buttonTransferCacheToPostgresFile.Enabled = false;
                    this.checkBoxPostgresUseBulkTransfer.Enabled = false;// !CacheDB.IsSqlServerExpress;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }        
        
        #endregion

        #region View data

        private void buttonViewPostgresContent_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(true, this.Schema());
            f.setHelp("Cache database postgres");
            f.ShowDialog();
        }

        #endregion

        #region Transfer data

        private void buttonTransferCacheToPostgres_Click(object sender, EventArgs e)
        {
            this.TransferCacheToPostgres();
        }


        private void buttonTransferCacheToPostgresFile_Click(object sender, EventArgs e)
        {
            this.TransferCacheToPostgres();
        }

        private void TransferCacheToPostgres()
        {
            if (this.EmbargoDate() != null)
            {
                System.DateTime Embargo = (System.DateTime)this.EmbargoDate();
                string Message = "Embargo until " + Embargo.ToString("yyyy-MM-dd") + "\r\nAre you sure that you want to publish these data";
                if (System.Windows.Forms.MessageBox.Show(Message, "Embargo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            string Report = this.TransferDataToPostgres(null);

            if (Report.Length > 0 && System.Windows.Forms.MessageBox.Show("Save report?", "Report", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    System.IO.StreamWriter sw;
                    string ReportFile = DiversityCollection.CacheDatabase.CacheDB.ReportsDirectory() + "\\Datatransfer_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss"); //.Year.ToString();
                    ReportFile += ".txt";
                    if (System.IO.File.Exists(ReportFile))
                        sw = new System.IO.StreamWriter(ReportFile, true, System.Text.Encoding.UTF8);
                    else
                        sw = new System.IO.StreamWriter(ReportFile, false, System.Text.Encoding.UTF8);
                    try
                    {
                        sw.WriteLine("Data transfer to Cache database");
                        sw.WriteLine();
                        sw.WriteLine("Started by:\t" + System.Environment.UserName);
                        sw.Write("Started at:\t");
                        sw.WriteLine(DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString());
                        sw.WriteLine();
                        sw.Write(Report);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    finally
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
        }

        public string TransferDataToPostgres(InterfaceCacheDatabase iCacheDB)
        {
            if (this.EmbargoDate() != null && DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
            {
                System.DateTime Embargo = (System.DateTime)this.EmbargoDate();
                string EmbargoMessage = "Embargo until " + Embargo.Year.ToString() + "-" + Embargo.Month.ToString() + "-" + Embargo.Day.ToString();
                return EmbargoMessage;
            }
            string Report = "";
            this._TransferHistory.Clear();
            if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransfer)
            {
                if (true) // nicht automatisch alle Ziele ansteuern, nur noch das angegebene. Wird sonst schwer nachvollziehbar
                {
                    string StartingTarget = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                    string Message = "";
                    string SQL = "SELECT T.DatabaseName, P.TransferDirectory " +
                        "FROM ProjectTarget AS P, [Target] T " +
                        "WHERE P.ProjectID = " + this._ProjectID.ToString() +
                        " AND (P.IncludeInTransfer = 1) " +
                        " AND P.TargetID = T.TargetID " +
                        " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
                        " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString();
                    System.Data.DataTable dtProjects = new DataTable();
                    DiversityCollection.CacheDatabase.CacheDB.LogEvent("TransferDataToPostgres", DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, "Getting targets: " + SQL);
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtProjects, ref Message);
                    if (dtProjects.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dtProjects.Rows)
                        {
                            DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(R["DatabaseName"].ToString());
                            DiversityCollection.CacheDatabase.CacheDB.LogEvent("TransferDataToPostgres", DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, "Transfer to target: " + R["DatabaseName"].ToString());
                            Report += this.TransferDataToPostgresDatabase(iCacheDB, R["TransferDirectory"].ToString());
                        }
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(StartingTarget);
                    }
                }
                else
                {
                    DiversityCollection.CacheDatabase.CacheDB.LogEvent("TransferDataToPostgres", DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, "Transfer to target: " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name);
                    Report += this.TransferDataToPostgresDatabase(iCacheDB);
                }
            }
            else // manual transfer
            {
                Report = this.TransferDataToPostgresDatabase(iCacheDB);
            }
            return Report;
        }

        public string TransferDataToPostgresDatabase(InterfaceCacheDatabase iCacheDB, string TransferDirectory = "")
        {
            string Message = "";
            // Check for updatas
            if (this._PostgresDatabaseNeedsUpdate)
            {
                Message = "Please update postgres database to current version";
                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransfer)
                    System.Windows.Forms.MessageBox.Show(Message);
                if (iCacheDB != null)
                    iCacheDB.ShowTransferState(Message);
                return Message;
            }

            // Check connection to database and if project has been established
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)
            {
                Message = "Not connected to postgres database";
                if (iCacheDB != null)
                    iCacheDB.ShowTransferState(Message);
                return Message;
            }
            else if (this.PostgresProject.ProjectID == null)
            {
                Message = "Project " + this._Project + " not established for " + DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText();
                if (iCacheDB != null)
                    iCacheDB.ShowTransferState(Message);
                return Message;
            }

            // If the date of the last changes should be checked
            if (this.CompareLogDateForPostgresDB)//.FilterProjectCacheForUpdate)
            {
                if (!this.DataAreUpdatedInCache())
                {
                    Message = "Data are up to date. No transfer needed";
                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransfer)
                    {
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                    if (iCacheDB != null)
                        iCacheDB.ShowTransferState(Message);
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransfer)
                        return Message;
                    else return "";
                }
            }

            // Check if the scheduled point in time is reached
            string Error = "";
            Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly /*AutoTransfer*/ && !DiversityCollection.CacheDatabase.CacheDB.SchedulerPlanedTimeReached(
                "ProjectTarget",
                "",
                this._ProjectID,
                this._TargetID,// DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,
                ref Error,
                ref Message))
            {
                Message = "Scheduled time not reached";
                if (iCacheDB != null)
                    iCacheDB.ShowTransferState(Message);
                DiversityCollection.CacheDatabase.CacheDB.LogEvent("UserControlProject", "TransferDataToPostgresDatabase(InterfaceCacheDatabase iCacheDB)", Message);
                return Message;
            }

            // Check if competing transfer is active
            string TransferActiv = DiversityCollection.CacheDatabase.CacheDB.SetTransferActive(
                "ProjectTarget",
                DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,
                this._TargetID,
                this._ProjectID,
                "",
                true);
            if (TransferActiv.Length > 0)
            {
                if (iCacheDB != null)
                    iCacheDB.ShowTransferState(TransferActiv);
                DiversityCollection.CacheDatabase.CacheDB.LogEvent("UserControlProject", "TransferDataToPostgresDatabase(InterfaceCacheDatabase iCacheDB)", "Competing transfer active: " + TransferActiv);
                return "";
            }

            string Report = "Transfer to " + DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText();
            Report += "\r\nStarted at: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString() + "\r\n";
            Report += "\r\nProject " + this._Project + "\r\n";

            this.PostgresRemoveProjectData();

            string Errors = "";

            System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps = this.TransferSteps(true);// new List<TransferStep>();
            if (this.UseBulkTransfer)
            {
                foreach (DiversityCollection.CacheDatabase.TransferStep T in TransferSteps)
                    T.PostgresUseBulkTransfer = true;
            }

            DiversityCollection.CacheDatabase.FormCacheDatabaseTransfer f = new FormCacheDatabaseTransfer(TransferSteps, DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory);//, AutoTransfer);
            f.setProjectID(this._ProjectID);
            if (TransferSteps[0].PostgresUseBulkTransfer)
                f.SetBulkTransferIcon();
            try
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && this.EmbargoDate() == null)
                {
                    Report += f.Report();
                    Errors += f.Errors();
                    this.TransferDataToPackagesInDatabase(ref Report, ref Error);
                }
                else
                {
                    f.setHelp("Cache database transfer");
                    f.ShowDialog();
                    if (f.Report().Length > 0)
                        Report += f.Report();
                    else Report = "";
                    Errors += f.Errors();
                }
                this._TransferHistory = f.TransferHistory;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                DiversityCollection.CacheDatabase.CacheDB.SetTransferFinished(
                    "ProjectTarget",
                    DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,
                    this._TargetID,
                    this._ProjectID,
                    "",
                    Report,
                    Errors);
            }
            if (f.DataHaveBeenTransferred)
            {
                //this.WriteHistory(HistoryTarget.CacheToPostgres, this._TargetID);
                DiversityCollection.CacheDatabase.CacheDB.WriteProjectTransferHistory(CacheDB.HistoryTarget.CacheToPostgres, this._ProjectID, this.Schema(), f.TransferHistory, this._TargetID);
                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransfer)
                {
                    if (f.AllDataHaveBeenTransferred)
                        this._StateOfPostgresData = StateOfData.TargetUpToDate;
                    else
                        this._StateOfPostgresData = StateOfData.PartialUpToDate;
                }
            }
            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)//AutoTransfer)
            {
                this.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
            }

            return Report;
        }
        
        #endregion

        #region Postgres Schedule

        private bool _FilterProjectCacheForUpdate = false;

        private void buttonProjectTransferToPostgresFilter_Click(object sender, EventArgs e)
        {
            bool DoCompare = this.CompareLogDateForPostgresDB;
            if (DoCompare)
            {
                this.toolTip.SetToolTip(this.buttonProjectTransferToPostgresFilter, "Data not filtered for updates");
                this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.FilterClear;
            }
            else
            {
                this.toolTip.SetToolTip(this.buttonProjectTransferToPostgresFilter, "Data filtered for updates later than last transfer");
                this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.Filter;
            }
            this.CompareLogDateForPostgresDB = !DoCompare;
        }

        private bool CompareLogDateForPostgresDB
        {
            get
            {
                string SQL = "SELECT CompareLogDate FROM ProjectTarget P, [Target] T " +
                    " WHERE T.TargetID = P.TargetID " +
                    " AND P.ProjectID = " + this._ProjectID.ToString() + 
                    " AND T.TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
                bool DoCompare = false;
                // Markus 28.3.25: Check count. Issue #36
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, true);
                bool.TryParse(Result, out DoCompare);
                return DoCompare;
            }
            set
            {
                string SQL = "UPDATE P SET CompareLogDate = ";
                if (value)
                    SQL += "1";
                else
                    SQL += "0";
                SQL += " FROM ProjectTarget P, [Target] T " +
                    "WHERE T.TargetID = P.TargetID " +
                    " AND P.ProjectID = " + this._ProjectID.ToString() +
                    " AND T.TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                if (value) this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.Filter;
                else this.buttonProjectTransferToPostgresFilter.Image = DiversityCollection.Resource.FilterClear;
            }
        }

        private void initPostgresScheduleControls()
        {
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0 &&
                    this.PostgresProject.ProjectID != null)
                {
                    this.initTarget();
                    this.initPostgresProjectTarget();
                    DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(
                        "ProjectTarget",
                        "",
                        this._ProjectID,
                        this.checkBoxPostgresIncludeInTransfer,
                        this.buttonProjectTransferToPostgresFilter,
                        this.buttonPostgresTransferSettings,
                        this.buttonPostgresTransferProtocol,
                        this.buttonPostgresTransferErrors,
                        this.buttonTransferCacheToPostgres,
                        this.labelLastUpdateInPostgresDatabase,
                        this.toolTip);
                    if (this.MissingInCacheDB() || this.MissingInDB())
                    {
                        this.checkBoxPostgresIncludeInTransfer.Enabled = false;
                        this.buttonPostgresTransferSettings.Enabled = false;
                        this.buttonProjectTransferToPostgresFilter.Enabled = false;
                    }
                }
                else
                {
                    this.checkBoxPostgresIncludeInTransfer.Enabled = false;
                    this.buttonPostgresTransferSettings.Enabled = false;
                    this.buttonPostgresTransferProtocol.Enabled = false;
                    this.buttonPostgresTransferErrors.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void checkBoxPostgresIncludeInTransfer_Click(object sender, EventArgs e)
        {
            string SQL = "UPDATE P SET P.IncludeInTransfer = ";
            if (this.checkBoxPostgresIncludeInTransfer.Checked)
                SQL += "1";
            else SQL += "0";
            SQL += " FROM ProjectTarget P, [Target] T " +
                "WHERE T.TargetID = P.TargetID " +
                " AND P.ProjectID = " + this._ProjectID.ToString() +
                " AND T.TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
            //SQL += " FROM ProjectTarget P WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            this.buttonPostgresTransferSettings.Enabled = this.checkBoxPostgresIncludeInTransfer.Checked;
        }

        private void buttonPostgresTransferSettings_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormScheduleTransferSettings f = new FormScheduleTransferSettings("ProjectTarget", DiversityCollection.CacheDatabase.CacheDB.TargetID(),/* DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,*/ this._Project, this._ProjectID);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.initPostgresControls(this._PostgresDatabaseNeedsUpdate);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void initPostgresForFileTransfer()
        //{
        //    string SQL = "";
        //    try
        //    {
        //        if (CacheDB.IsSqlServerExpress)
        //            this._TransferDirectory = "";
        //        else if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
        //        {
        //            SQL = "SELECT P.[TransferDirectory] " +
        //                " FROM [dbo].[ProjectTarget] P, [dbo].[Target] T " +
        //                " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
        //                " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
        //                " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
        //                " AND T.TargetID = P.TargetID " +
        //                " AND P.ProjectID = " + this.ProjectID().ToString();
        //            _TransferDirectory = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
        //        }
        //    }
        //    catch(System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private void checkBoxPostgresUseBulkTransfer_Click(object sender, EventArgs e)
        {
            this.UseBulkTransfer = this.checkBoxPostgresUseBulkTransfer.Checked;
            this.initPostgresControls(false);
            //DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Transfer directory", "Please enter path of the transfer directory", this._TransferDirectory);
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    //this.textBoxPostgresTransferDirectory.Text = f.String;
            //    string SQL = "UPDATE P SET P.[TransferDirectory] = '" + f.String + "' " +
            //        " FROM [dbo].[ProjectTarget] P, [Target] T " +
            //        " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
            //        " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
            //        " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
            //        " AND T.TargetID = P.TargetID " +
            //        " AND P.ProjectID = " + this.ProjectID().ToString();
            //    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            //    //this.initPostgresForFileTransfer();
            //    this.initPostgresControls(false);
            //}

        }

        private bool? _UseBulkTransfer;
        private bool UseBulkTransfer
        {
            get
            {
                if (this._UseBulkTransfer == null)
                {
                    if (DiversityCollection.CacheDatabase.CacheDB.CanUseBulkTransfer &&
                        DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                    {
                        string SQL = "SELECT UseBulkTransfer " +
                            " FROM [dbo].[ProjectTarget] P, [Target] T " +
                            " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                            " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                            " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
                            " AND T.TargetID = P.TargetID " +
                            " AND P.ProjectID = " + this.ProjectID().ToString();
                        string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                        bool UseBulk;
                        if (bool.TryParse(Result, out UseBulk))
                            this._UseBulkTransfer = UseBulk;
                        else this._UseBulkTransfer = false;
                    }
                    else
                    {
                        this._UseBulkTransfer = false;
                    }
                }
                return (bool)this._UseBulkTransfer;
            }
            set
            {
                this._UseBulkTransfer = value;
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                {
                    string SQL = "UPDATE P SET P.[UseBulkTransfer] = ";
                    if (value && DiversityCollection.CacheDatabase.CacheDB.CanUseBulkTransfer)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += " FROM [dbo].[ProjectTarget] P, [Target] T " +
                        " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                        " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                        " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
                        " AND T.TargetID = P.TargetID " +
                        " AND P.ProjectID = " + this.ProjectID().ToString();
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
            }
        }

        //private bool? _CanUseBulkTransfer;
        ////private string _TransferDirectory = "";
        ////private string _BashFile = "";
        //private bool CanUseBulkTransfer
        //{
        //    get
        //    {
        //        if (this._CanUseBulkTransfer == null)
        //        {
        //            if (DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile.Length > 0 &&
        //                DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory.Length > 0)
        //            {
        //                this._CanUseBulkTransfer = true;
        //            }
        //            else
        //                this._CanUseBulkTransfer = false;
        //            //string SQL = "SELECT UseBulkTransfer " +
        //            //    " FROM [dbo].[ProjectTarget] P, [Target] T " +
        //            //    " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
        //            //    " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
        //            //    " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
        //            //    " AND T.TargetID = P.TargetID " +
        //            //    " AND P.ProjectID = " + this.ProjectID().ToString();
        //            //string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
        //            //bool UseBulk;
        //            //if (bool.TryParse(Result, out UseBulk))
        //            //    this._UseBulkTransfer = UseBulk;
        //            //else this._UseBulkTransfer = false;
        //        }
        //        return (bool)this._CanUseBulkTransfer;
        //    }
        //}



        private int? _TargetID = null;
        private void initTarget()
        {
            try
            {
                this._TargetID = DiversityCollection.CacheDatabase.CacheDB.TargetID(); // int.Parse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL));
            }
            catch (System.Exception ex)
            { 
            }
        }

        private void initPostgresProjectTarget()
        {
            try
            {
                if (this._TargetID != null)
                {
                    string SQL = "SELECT COUNT(*) FROM ProjectTarget P WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND TargetID = " + _TargetID.ToString();
                    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    if (Result != "1")
                    {
                        SQL = "SELECT [Project_" + this._Project + "].[ProjectID] ()";
                        string ProjectID = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                        int ID;
                        if (int.TryParse(ProjectID, out ID))
                        {
                            if (ID == this._ProjectID)
                            {
                                SQL = "INSERT INTO ProjectTarget (ProjectID, TargetID, LastUpdatedWhen, IncludeInTransfer) VALUES (" + this._ProjectID.ToString() + ", " + _TargetID.ToString() + ", NULL, 0)";
                                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonPostgresTransferProtocol_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.TransferProtocol FROM ProjectTarget P WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND TargetID = " + this._TargetID.ToString();
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer protocol for project " + this.labelProject.Text, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No protocol found");
        }

        private void buttonPostgresTransferErrors_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.TransferErrors FROM ProjectTarget P WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND TargetID = " + this._TargetID.ToString();
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer errors for project " + this.labelProject.Text, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No errors found");
        }

        #endregion   

        #region Packages

        private void buttonAdministratePackages_Click(object sender, EventArgs e)
        {
            this.AdministratePackages();
        }

        private void listBoxPackages_MouseClick(object sender, MouseEventArgs e)
        {
            this.AdministratePackages();
        }

        private void AdministratePackages()
        {
            DiversityCollection.CacheDatabase.FormPackages f = new FormPackages(this._Project, (int)this._TargetID);
            f.ShowDialog();
            f.setHelp("Cache database packages");
            this.initPackages();
        }

        private System.Data.DataTable _dtPackages;

        private void initPackages(string Schema = "")
        {
            try
            {
                string Message = "";
                if (this.Schema().Length > 0)
                    Schema = this.Schema();
                string SQL = "SELECT \"Package\"  FROM \"" + Schema + "\".\"Package\" ORDER BY \"Package\"";
                if (Schema.Length > 0)
                {
                    this._dtPackages = new DataTable();
                    if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref this._dtPackages, ref Message))
                    {
                        this.listBoxPackages.DataSource = this._dtPackages;
                        this.listBoxPackages.DisplayMember = "Package";
                        this.listBoxPackages.ValueMember = "Package";
                    }
                    string SqlPackages = "";
                    foreach (System.Data.DataRow R in this._dtPackages.Rows)
                    {
                        SQL = "IF (SELECT COUNT(*) FROM [dbo].[ProjectTargetPackage] P " +
                            "WHERE P.Package = '" + R[0].ToString() + "' " +
                            "AND P.ProjectID = " + this._ProjectID.ToString() + " " +
                            "AND P.TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString() + ") = 0 " +
                            "AND (SELECT COUNT(*) FROM [dbo].[ProjectTarget] T " +
                            "WHERE T.ProjectID = " + this._ProjectID.ToString() + " AND T.TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString() + ") > 0 " +
                            "BEGIN " +
                            "INSERT INTO [ProjectTargetPackage](Package, ProjectID, TargetID) " +
                            "VALUES ('" + R[0].ToString() + "', " + this._ProjectID.ToString() + ", " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString() + ") " +
                            "END";
                        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                        if (SqlPackages.Length > 0) SqlPackages += ", ";
                        SqlPackages += "'" + R[0].ToString() + "'";
                    }
                    // Aenderung ala Toni - Email 05.02.2021, 10:07
                    //if (SqlPackages.Length > 0)
                    //{
                    //    SQL = "DELETE FROM ProjectTargetPackage WHERE ProjectID = " + this._ProjectID.ToString() + " " +
                    //        "AND TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString() + " " +
                    //        "AND Package NOT IN (" + SqlPackages + ")";
                    //    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    //}
                    // Delete packages not present in target
                    SQL = "DELETE FROM ProjectTargetPackage WHERE ProjectID = " + this._ProjectID.ToString() + " " +
                          "AND TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
                    if (SqlPackages.Length > 0)
                        SQL += " AND Package NOT IN (" + SqlPackages + ")";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);

                    this.buttonTransferPackageData.Visible = false;
                    if (this._dtPackages.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in this._dtPackages.Rows)
                        {
                            DiversityCollection.CacheDatabase.Package P = new Package(R[0].ToString(), this._PostgresProject);
                            if (DiversityCollection.CacheDatabase.Package.TransferSteps(P.PackagePack, this._PostgresProject.SchemaName).Count > 0)
                            {
                                this.buttonTransferPackageData.Visible = true;
                                break;
                            }
                            if (P.NeedsUpdate())
                                this._PostgresPackageNeedsUpdate = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void buttonTransferPackageData_Click(object sender, EventArgs e)
        {
            if (this.EmbargoDate() != null)
            {
                System.DateTime Embargo = (System.DateTime)this.EmbargoDate();
                string Message = "Embargo until " + Embargo.ToString("yyyy-MM-dd") + "\r\nAre you sure that you want to publish these data";
                if (System.Windows.Forms.MessageBox.Show(Message, "Embargo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Report = "";
            string Error = "";
            try
            {
                this.TransferDataToPackagesInDatabase(ref Report, ref Error);
                if (Report.Length > 0 || Error.Length > 0)
                {
                    string Message = Report + "\r\n";
                    if (Error.Length > 0) Message += "Errors during transfer:\r\n" + Error;
                    System.Windows.Forms.MessageBox.Show(Message);
                }
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public void TransferDataToPackagesInDatabase(ref string Report, ref string Error)
        {
            if (this.EmbargoDate() != null && DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
            {
                System.DateTime Embargo = (System.DateTime)this.EmbargoDate();
                Report = "Embargo until " + Embargo.Year.ToString() + "-" + Embargo.Month.ToString() + "-" + Embargo.Day.ToString();
                return;
            }
            string Message = "";
            if (this._PostgresDatabaseNeedsUpdate)
            {
                Error = "Please update postgres database to current version";
                CacheDB.LogEvent(this.Name.ToString(), "TransferDataToPackagesInDatabase", Error);
                return;
            }
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)
            {
                Error = "Not connected to postgres database";
                CacheDB.LogEvent(this.Name.ToString(), "TransferDataToPackagesInDatabase", Error);
                return;
            }
            else if (this.PostgresProject.ProjectID == null)
            {
                Error = "Project " + this._Project + " not established for " + DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText();
                CacheDB.LogEvent(this.Name.ToString(), "TransferDataToPackagesInDatabase", Error);
                return;
            }

            this.initPackages();
            if (this._dtPackages.Rows.Count > 0)
            {
                try
                {
                    CacheDB.LogEvent(this.Name.ToString(), "TransferDataToPackagesInDatabase", "Transfer packages");
                    foreach (System.Data.DataRow R in this._dtPackages.Rows)
                    {
                        DiversityCollection.CacheDatabase.Package P = new Package(R[0].ToString(), this._PostgresProject);
                        if (P.NeedsUpdate())
                        {
                            Error = "Please update package " + P.Name + " to current version";
                            CacheDB.LogEvent(this.Name.ToString(), "TransferDataToPackagesInDatabase", Error);
                        }
                        else if (DiversityCollection.CacheDatabase.Package.TransferSteps(P.PackagePack, this._PostgresProject.SchemaName).Count > 0)
                        {
                            // Dictionary for all infos
                            //System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<object>> D_History = new Dictionary<string, System.Collections.Generic.List<object>>();
                            CacheDB.LogEvent("", "", "Transfer " + P.PackagePack);

                            System.Collections.Generic.Dictionary<string, object> TransferHistory = new Dictionary<string, object>();
                            P.TransferData(ref Error, null, ref TransferHistory);
                            Message += P.Name + "\r\n";
                            //System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
                            //string Settings = JSS.Serialize(TransferHistory);
                            //this.WriteHistory(HistoryTarget.CacheToPostgres, this._TargetID, Settings, P.Name);
                            DiversityCollection.CacheDatabase.CacheDB.WriteProjectTransferHistory(CacheDB.HistoryTarget.CacheToPostgres, this._ProjectID, this.Schema(), TransferHistory, this._TargetID, P.Name);
                            CacheDB.LogEvent("", "", "Transfer " + P.PackagePack + " finished");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
            Report += "Packages\r\n" + Message + "transferred";

            return;
        }

        #endregion

        #region Other targets

        //TODO Ariane TEST
        private void ClearPanelPostgresOtherTargetsControls()
        {
            foreach (Control control in this.panelPostgresOtherTargets.Controls)
            {
                control.Dispose();
            }
            this.panelPostgresOtherTargets.Controls.Clear();
        }

        public void initOtherTargets()
        {
            string SQL = "";
            try
            {
                //TODO: hier noch anpassen - s. Mail Toni 11.02.2021, 11:13 und 11.02.2021, 13:57
                SQL = "SELECT T.TargetID " +
                    "FROM ProjectTarget P, [Target] T " +
                    "WHERE p.ProjectID = " + this._ProjectID +
                    " AND P.TargetID = T.TargetID " +
                    " AND p.TargetID <> " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
                string Message = "";
                //this.panelPostgresOtherTargets.Controls.Clear();
                this.ClearPanelPostgresOtherTargetsControls();
                this.panelPostgresOtherTargets.Height = 0;
                this.Height = (int)(this._ControlHeight * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                //string SQL = "SELECT Target FROM ProjectTarget AS p WHERE p.ProjectID = " + this._ProjectID + " AND p.Target <> '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                System.Data.DataTable dt = new DataTable();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        using (var U = new UserControlProjectPostgresTargets(this._ProjectID, int.Parse(R[0].ToString()), this.Schema(), this))
                        {
                            this.panelPostgresOtherTargets.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                            U.BringToFront();
                            this.panelPostgresOtherTargets.Height += DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(U.HeightForDisplay());
                        }
                        //DiversityCollection.CacheDatabase.UserControlProjectPostgresTargets U = new UserControlProjectPostgresTargets(this._ProjectID, int.Parse(R[0].ToString()), this.Schema(), this);
                        //this.panelPostgresOtherTargets.Controls.Add(U);
                        //U.Dock = DockStyle.Top;
                        //U.BringToFront();
                        //this.panelPostgresOtherTargets.Height += DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(U.HeightForDisplay());
                    }
                    this.Height += this.panelPostgresOtherTargets.Height;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                MessageBox.Show("An error occurred while initializing targets. Please check the error log for details.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        #endregion

        #endregion

        #region Archive

        private void initArchive()
        {
            this.labelDoiInCacheDB.Image = null;
            this.labelDoiInPackage.Image = null;
            this.labelDoiInPostgres.Image = null;
            this.buttonCreateArchive.Enabled = false;
        }

        private string _DoiForArchive = "";
        private bool _GetDoi = false;

        private void buttonGetDoi_Click(object sender, EventArgs e)
        {
            this._GetDoi = !this._GetDoi;
            if (this._GetDoi)
            {
                this.buttonGetDoi.Image = DiversityCollection.Resource.DOI;
                this.toolTip.SetToolTip(this.buttonGetDoi, "DOI will be retrieved for storage with the archive");
            }
            else
            {
                this.buttonGetDoi.Image = DiversityCollection.Resource.DOIGrey;
                this.toolTip.SetToolTip(this.buttonGetDoi, "No DOI for the archive. Click here to retrieve a DOI together with the transfer");
            }

            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            ////return;
            //string SQL = "SELECT [dbo].[ProjectsDatabase] ()";
            //string ProjectDB = CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            //string ConnectionString = DiversityWorkbench.Settings.ServerConnection.ConnectionStringForDB(ProjectDB, "DiversityProjects");
            //this._DoiForArchive = DiversityWorkbench.Project.getDOI(ConnectionString, this._ProjectID);
            //this.buttonGetDoi.Enabled = false;
        }

        private void buttonCreateArchive_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region TransferSteps
        
        public System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps(bool ForPostgres)
        {
            System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps = new List<TransferStep>();

            DiversityCollection.CacheDatabase.TransferStep TCollectionSpecimen = new TransferStep("Specimen", this.imageListTransferSteps.Images[4], "CollectionSpecimen", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionSpecimen);

            DiversityCollection.CacheDatabase.TransferStep TCollectionProject = new TransferStep("Project", this.imageListTransferSteps.Images[22], "CollectionProject", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionProject);

            DiversityCollection.CacheDatabase.TransferStep TCollectionEvent = new TransferStep("Event", this.imageListTransferSteps.Images[1], "CollectionEvent", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionEvent);

            DiversityCollection.CacheDatabase.TransferStep TCollectionEventLocalisation = new TransferStep("Localisation", this.imageListTransferSteps.Images[2], "CollectionEventLocalisation", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionEventLocalisation);

            if (ForPostgres)
            {
                DiversityCollection.CacheDatabase.TransferStep TLocalisationSystem = new TransferStep("Loc. system", this.imageListTransferSteps.Images[2], "LocalisationSystem", this.Schema(), ForPostgres);
                TransferSteps.Add(TLocalisationSystem);
            }

            DiversityCollection.CacheDatabase.TransferStep TCollectionEventProperty = new TransferStep("Property", this.imageListTransferSteps.Images[3], "CollectionEventProperty", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionEventProperty);

//#if DEBUG

            DiversityCollection.CacheDatabase.TransferStep TMethod = new TransferStep("Method", this.imageListTransferSteps.Images[19], "Method", this.Schema(), ForPostgres);
            TransferSteps.Add(TMethod);

            DiversityCollection.CacheDatabase.TransferStep TCollectionEventMethod = new TransferStep("CollectionEventMethod", this.imageListTransferSteps.Images[19], "CollectionEventMethod", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionEventMethod);

            DiversityCollection.CacheDatabase.TransferStep TCollectionEventParameterValue = new TransferStep("CollectionEventParameterValue", this.imageListTransferSteps.Images[20], "CollectionEventParameterValue", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionEventParameterValue);

//#endif

            DiversityCollection.CacheDatabase.TransferStep TCollectionAgent = new TransferStep("Agent", this.imageListTransferSteps.Images[5], "CollectionAgent", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionAgent);

            DiversityCollection.CacheDatabase.TransferStep TCollectionReference = new TransferStep("Reference", this.imageListTransferSteps.Images[16], "CollectionSpecimenReference", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionReference);

            DiversityCollection.CacheDatabase.TransferStep TCollectionRelation = new TransferStep("Relation", this.imageListTransferSteps.Images[6], "CollectionSpecimenRelation", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionRelation);

            DiversityCollection.CacheDatabase.TransferStep TCollectionSpecimenImages = new TransferStep("Image", this.imageListTransferSteps.Images[7], "CollectionSpecimenImage", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionSpecimenImages);

            DiversityCollection.CacheDatabase.TransferStep TCollectionSpecimenParts = new TransferStep("Part", this.imageListTransferSteps.Images[8], "CollectionSpecimenPart", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionSpecimenParts);

#if Version202
            DiversityCollection.CacheDatabase.TransferStep TCollectionSpecimenPartDescription = new TransferStep("Part description", this.imageListTransferSteps.Images[23], "CollectionSpecimenPartDescription", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionSpecimenPartDescription);
#endif
            DiversityCollection.CacheDatabase.TransferStep TProcessing = new TransferStep("Processing", this.imageListTransferSteps.Images[9], "Processing", this.Schema(), ForPostgres);
            TransferSteps.Add(TProcessing);

            DiversityCollection.CacheDatabase.TransferStep TCollectionSpecimenProcessing = new TransferStep("Processing of part", this.imageListTransferSteps.Images[9], "CollectionSpecimenProcessing", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionSpecimenProcessing);

#if Version202
            DiversityCollection.CacheDatabase.TransferStep TCollectionSpecimenProcessingMethod = new TransferStep("Processing method for part", this.imageListTransferSteps.Images[19], "CollectionSpecimenProcessingMethod", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionSpecimenProcessingMethod);

            DiversityCollection.CacheDatabase.TransferStep TCollectionSpecimenProcessingMethodParameter = new TransferStep("Processing parameter for part", this.imageListTransferSteps.Images[20], "CollectionSpecimenProcessingMethodParameter", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollectionSpecimenProcessingMethodParameter);

#endif

            DiversityCollection.CacheDatabase.TransferStep TIdentificationUnit = new TransferStep("Organism", this.imageListTransferSteps.Images[10], "IdentificationUnit", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentificationUnit);

            DiversityCollection.CacheDatabase.TransferStep TIdentificationUnitInPart = new TransferStep("Organism in part", this.imageListTransferSteps.Images[11], "IdentificationUnitInPart", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentificationUnitInPart);

            DiversityCollection.CacheDatabase.TransferStep TIdentification = new TransferStep("Identification", this.imageListTransferSteps.Images[12], "Identification", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentification);

            DiversityCollection.CacheDatabase.TransferStep TIdentificationUnitGeoAnalysis = new TransferStep("Geography of organism", this.imageListTransferSteps.Images[2], "IdentificationUnitGeoAnalysis", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentificationUnitGeoAnalysis);

            DiversityCollection.CacheDatabase.TransferStep TIdentificationUnitAnalysis = new TransferStep("Analysis of organism", this.imageListTransferSteps.Images[13], "IdentificationUnitAnalysis", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentificationUnitAnalysis);
#if Version202
            DiversityCollection.CacheDatabase.TransferStep TIdentificationUnitAnalysisMethod = new TransferStep("Analysis method for organism", this.imageListTransferSteps.Images[19], "IdentificationUnitAnalysisMethod", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentificationUnitAnalysisMethod);

            DiversityCollection.CacheDatabase.TransferStep TIdentificationUnitAnalysisMethodParameter = new TransferStep("Analysis parameter for organism", this.imageListTransferSteps.Images[20], "IdentificationUnitAnalysisMethodParameter", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentificationUnitAnalysisMethodParameter);

#endif
            DiversityCollection.CacheDatabase.TransferStep TAnalysis = new TransferStep("Analysis", this.imageListTransferSteps.Images[13], "Analysis", this.Schema(), ForPostgres);
            TransferSteps.Add(TAnalysis);

            DiversityCollection.CacheDatabase.TransferStep TCollection = new TransferStep("Collection", this.imageListTransferSteps.Images[14], "Collection", this.Schema(), ForPostgres);
            TransferSteps.Add(TCollection);

            DiversityCollection.CacheDatabase.TransferStep TIdentifier = new TransferStep("Identifier", this.imageListTransferSteps.Images[17], "ExternalIdentifier", this.Schema(), ForPostgres);
            TransferSteps.Add(TIdentifier);

            DiversityCollection.CacheDatabase.TransferStep TAnnotation = new TransferStep("Annotation", this.imageListTransferSteps.Images[15], "Annotation", this.Schema(), ForPostgres);
            TransferSteps.Add(TAnnotation);

            DiversityCollection.CacheDatabase.TransferStep TDatasource = new TransferStep("Datasource", this.imageListTransferSteps.Images[18], "CollectionExternalDatasource", this.Schema(), ForPostgres);
            TransferSteps.Add(TDatasource);

            DiversityCollection.CacheDatabase.TransferStep TMetadata = new TransferStep("Metadata", this.imageListTransferSteps.Images[0], "Metadata", this.Schema(), ForPostgres);
            TransferSteps.Add(TMetadata);

            DiversityCollection.CacheDatabase.TransferStep TProjectReference = new TransferStep("ProjectReference", this.imageListTransferSteps.Images[16], "ProjectReference", this.Schema(), ForPostgres);
            TransferSteps.Add(TProjectReference);

            DiversityCollection.CacheDatabase.TransferStep TProjectAgent = new TransferStep("ProjectAgent", this.imageListTransferSteps.Images[5], "ProjectAgent", this.Schema(), ForPostgres);
            TransferSteps.Add(TProjectAgent);

            DiversityCollection.CacheDatabase.TransferStep TProjectAgentRole = new TransferStep("ProjectAgentRole", this.imageListTransferSteps.Images[5], "ProjectAgentRole", this.Schema(), ForPostgres);
            TransferSteps.Add(TProjectAgentRole);

            //DiversityCollection.CacheDatabase.TransferStep TProjectLicense = new TransferStep("ProjectLicense", this.imageListTransferSteps.Images[5], "ProjectLicense", this.Schema(), ForPostgres);
            //TransferSteps.Add(TProjectLicense);

            DiversityCollection.CacheDatabase.TransferStep TProjectDescriptor = new TransferStep("ProjectDescriptor", this.imageListTransferSteps.Images[24], "ProjectDescriptor", this.Schema(), ForPostgres);
            TransferSteps.Add(TProjectDescriptor);
#if DEBUG
#endif

            DiversityCollection.CacheDatabase.TransferStep TCount = new TransferStep("Count", this.imageListTransferSteps.Images[21], "Count", this.Schema(), ForPostgres);
            TransferSteps.Add(TCount);

#if DEBUG
#endif
            return TransferSteps;
        }
        
        #endregion

        #region History

        private enum HistoryTarget { DataToCache, CacheToPostgres }

        private string SettingsForHistory(HistoryTarget Target, int? TargetID = null)
        {
            string History = "";
            try
            {
                string SQL = "";
                // Dictionary for all infos
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<object>> D_History = new Dictionary<string, System.Collections.Generic.List<object>>();
                
                // Versions
                System.Collections.Generic.List<object> OVers = new List<object>();
                OVers.Add(this.HistoryVersions(Target));
                D_History.Add("Versions", OVers);

                if (Target == HistoryTarget.DataToCache)
                {
                    // Settings
                    System.Collections.Generic.Dictionary<string, int> D_Pre = new Dictionary<string, int>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Loc = new Dictionary<string, System.Collections.Generic.List<string>>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Tax = new Dictionary<string, System.Collections.Generic.List<string>>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Mat = new Dictionary<string, System.Collections.Generic.List<string>>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Ana = new Dictionary<string, System.Collections.Generic.List<string>>();

                    // CoordinatePrecision
                    SQL = "SELECT CoordinatePrecision FROM ProjectPublished WHERE ProjectID = " + this._ProjectID.ToString();
                    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    int Pre;
                    if (int.TryParse(Result, out Pre))
                    {
                        D_Pre.Add("CoordinatePrecision", Pre);
                    }

                    // LocalisationSystemID
                    SQL = "SELECT S.DisplayText + ' [ID: ' + CAST (S.LocalisationSystemID AS varchar) + ']' FROM " + this.Schema() + ".CacheLocalisationSystem AS L, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.LocalisationSystem S " +
                        "WHERE L.LocalisationSystemID = S.LocalisationSystemID ORDER BY Sequence";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> Loc = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            Loc.Add(R[0].ToString());
                        D_Loc.Add("LocalisationSystems", Loc);
                    }

                    // TaxonomicGroups
                    SQL = "SELECT TaxonomicGroup FROM " + this.Schema() + ".ProjectTaxonomicGroup ORDER BY TaxonomicGroup";
                    dt = new DataTable();
                    Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> L = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            L.Add(R[0].ToString());
                        D_Tax.Add("TaxonomicGroups", L);
                    }

                    // ProjectMaterialCategories
                    SQL = "SELECT MaterialCategory FROM " + this.Schema() + ".ProjectMaterialCategory ORDER BY MaterialCategory";
                    dt = new DataTable();
                    Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> L = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            L.Add(R[0].ToString());
                        D_Mat.Add("MaterialCategories", L);
                    }

                    // Analysis
                    SQL = "SELECT A.DisplayText + ' [ID: ' + CAST(A.AnalysisID as varchar) + ']' FROM " + this.Schema() + ".ProjectAnalysis P, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.Analysis A " +
                        "WHERE A.AnalysisID = P.AnalysisID ORDER BY A.DisplayText";
                    dt = new System.Data.DataTable();
                    Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> L = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            L.Add(R[0].ToString());
                        D_Ana.Add("AnalysisIDs", L);
                    }

                    System.Collections.Generic.List<object> OO_settings = new List<object>();
                    if (D_Pre.Count > 0)
                        OO_settings.Add(D_Pre);
                    if (D_Ana.Count > 0)
                        OO_settings.Add(D_Ana);
                    if (D_Loc.Count > 0)
                        OO_settings.Add(D_Loc);
                    if (D_Mat.Count > 0)
                        OO_settings.Add(D_Mat);
                    if (D_Tax.Count > 0)
                        OO_settings.Add(D_Tax);
                    D_History.Add("Settings", OO_settings);

                }
                else
                {
                    System.Collections.Generic.List<object> OO_Packs = new List<object>();

                    // the dictionary for the packages including the add ons
                    System.Collections.Generic.Dictionary<string, string> D_Package = new Dictionary<string, string>();

                    //System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Pack = new Dictionary<string, System.Collections.Generic.List<string>>();

                    System.Data.DataTable dt = new System.Data.DataTable();
                    string Message = "";
                    SQL = "SELECT \"Package\", \"Version\" " +
                        "FROM \"" + this.Schema() + "\".\"Package\" ORDER BY \"Package\";";
                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            D_Package.Add(R[0].ToString(), "Vers.: " + R[1].ToString());
                            SQL = "SELECT \"AddOn\", \"Version\" " +
                                " FROM \"" + this.Schema() + "\".\"PackageAddOn\" " +
                                " WHERE \"Package\" = '" + R[0].ToString() + "';";
                            dt = new System.Data.DataTable();
                            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (System.Data.DataRow Ra in dt.Rows)
                                {
                                    //System.Collections.Generic.Dictionary<string, string> D_AddOn = new Dictionary<string, string>();
                                    D_Package.Add(Ra[0].ToString(), "Vers.: " + Ra[1].ToString());
                                }
                            }
                        }
                        OO_Packs.Add(D_Package);
                    }
                    if (OO_Packs.Count > 0)
                    {
                        D_History.Add("Packages", OO_Packs);
                    }

                }
                // Data
                System.Collections.Generic.List<object> L_Data = new List<object>();
                L_Data.Add(this._TransferHistory);
                D_History.Add("Data", L_Data);
                
                History = System.Text.Json.JsonSerializer.Serialize(D_History);
            }
            catch (System.Exception ex)
            {
            }
            return History;
        }

        private System.Collections.Generic.Dictionary<string, string> HistoryVersions(HistoryTarget Target)
        {
            System.Collections.Generic.Dictionary<string, string> D_Ver = new Dictionary<string, string>();
            string SQL = "SELECT dbo.Version()";
            string V = "";
            if (Target == HistoryTarget.DataToCache)
            {
                // DB
                V = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                D_Ver.Add("Database", V);
            }
            // CacheDB
            V = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            D_Ver.Add("CacheDB", V);
            // Project
            SQL = "select [" + this.Schema() + "].version()";
            V = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            D_Ver.Add("Project", V);
            if (Target != HistoryTarget.DataToCache)
            {
                // Postgres DB
                SQL = "SELECT public.version()";
                V = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                D_Ver.Add("PostgresDB", V);
                // Postgres Project
                SQL = "SELECT \"" + this.Schema() + "\".version()";
                V = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                D_Ver.Add("Postgres Schema", V);
            }
            return D_Ver;
        }

        private void buttonHistoryInCacheDB_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormTransferHistory f = new FormTransferHistory(this._ProjectID);
                f.ShowDialog();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonHistoryInPostgres_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormTransferHistory f = new FormTransferHistory(this._ProjectID, (int)this._TargetID);
                f.ShowDialog();
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

    }
}
