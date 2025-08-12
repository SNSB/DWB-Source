using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormCacheDBAdministration : Form
    {

        #region Parameter

        //private string _ProjectDB;
        //private string _CacheDB;
        //private System.Collections.Generic.Dictionary<int, string> _SqlCommands;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCacheDatabase;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDiversityTaxonNames;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterDiversityTaxonNamesProjectSequence;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterProjectProxy;
        private string _DiversityTaxonNamesConnectionString;
        private string _DiversityTaxonNamesDataSource;
        private string _DiversityTaxonNamesDataBase;


        private string _VersionOfSelectedDatabase;

        #endregion

        #region Construction
        
        public FormCacheDBAdministration()
        {
            InitializeComponent();
            this.initForm();
        }
        
        #endregion

        #region Form
        
        public void initForm()
        {
            if (!this.CacheDBavailable)
            {
                System.Windows.Forms.MessageBox.Show("The cache database functionality is not available. Please contact your system administrator");
                return;
            }
            this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm";
            if (DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
            {
                this.toolStripButtonDatabaseDelete.Enabled = true;
                this.toolStripButtonDatabaseNew.Enabled = true;
                this.buttonLoginAdministration.Enabled = true;
                this.buttonUpdateDatabase.Enabled = true;
            }
            else
            {
                this.toolStripButtonDatabaseDelete.Enabled = false;
                this.toolStripButtonDatabaseNew.Enabled = false;
                this.buttonLoginAdministration.Enabled = false;
                this.buttonUpdateDatabase.Enabled = false;
            }
            this.setDatabaseList();
        }

        private bool CacheDBavailable
        {
            get
            {
                bool OK = true;
                try
                {
                    string SQL = "select COUNT(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'CacheDatabase'";
                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    int i = 0;
                    if (int.TryParse(C.ExecuteScalar().ToString(), out i) && i > 0)
                        OK = true;
                    else OK = false;
                }
                catch (System.Exception ex) { OK = false; }
                return OK;
            }
        }

        private void ShowTabpage(System.Windows.Forms.TabPage T, bool ClearTabControl)
        {
            if (ClearTabControl)
                this.tabControlMain.TabPages.Clear();
            this.tabControlMain.TabPages.Add(T);
        }
        
        private void FormCacheDBAdministration_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB.CacheDatabase". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.cacheDatabaseTableAdapter.Fill(this.dataSetCacheDB.CacheDatabase);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB.DiversityTaxonNamesProjectSequence". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.diversityTaxonNamesProjectSequenceTableAdapter.Fill(this.dataSetCacheDB.DiversityTaxonNamesProjectSequence);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB.ProjectProxy". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectProxyTableAdapter.Fill(this.dataSetCacheDB.ProjectProxy);
        }

        private void FormCacheDBAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.projectProxyBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectProxyBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
                this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
            }
        }

        #endregion

        #region Properties

        public string CacheDBServer
        {
            get
            {
                return DiversityCollection.CacheDatabase.CacheDB.DatabaseServer;
            }
        }

        public string CacheDB
        {
            get
            {
                return DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
            }
        }

        public string ProjectDB
        {
            get 
            {
                return DiversityCollection.CacheDatabase.CacheDB.ProjectsDatabase; 
            }
        }

        public string VersionOfSelectedDatabase
        {
            get { return _VersionOfSelectedDatabase; }
            set 
            { 
                _VersionOfSelectedDatabase = value; 
            }
        }

        #endregion

        #region Database

        private void setDatabaseList()
        {
            try
            {
                string SQL = "SELECT DatabaseName, Server, Port, Version FROM CacheDatabase"; //, ProjectsDatabaseName
                this._SqlDataAdapterCacheDatabase = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapterCacheDatabase.Fill(this.dataSetCacheDB.CacheDatabase);
                if (this.listBoxDatabase.DataSource == null)
                {
                    this.listBoxDatabase.DataSource = this.dataSetCacheDB.CacheDatabase;
                    this.listBoxDatabase.DisplayMember = "DatabaseName";
                    this.listBoxDatabase.ValueMember = "Server";
                }
                if (this.dataSetCacheDB.CacheDatabase.Rows.Count == 0)
                {
                    this.tabControlMain.TabPages.Remove(this.tabPageProjects);
                    this.tabControlMain.TabPages.Remove(this.tabPageTaxonomy);
                    this.tableLayoutPanelDatabase.Enabled = false;
                }
                else
                {
                }
            }
            catch (System.Exception ex) { }
        }
        
        private void toolStripButtonDatabaseNew_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("available in upcoming version");
            return;

            string SQL = "";
            string Database = "";
            string ConnectionStringCacheDB = DiversityWorkbench.Settings.ConnectionString;
            string Directory = "C:\\Program Files\\Microsoft SQL Server\\MSSQL10.MSSQLSERVER\\MSSQL\\DATA";

            DiversityWorkbench.ServerConnection SC = new DiversityWorkbench.ServerConnection();
            SC.DatabaseName = "master";
            SC.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
            SC.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
            SC.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;

            string Message = "Server of the current database:\r\n" + DiversityWorkbench.Settings.DatabaseServer + "\r\n\r\nShould the cache database be installed on a different server";

            if (System.Windows.Forms.MessageBox.Show(Message, "Different server?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                SQL = "select physical_name from sys.database_files where data_space_id = 1";
                System.IO.FileInfo F = new System.IO.FileInfo(DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL));
                Directory = F.DirectoryName;
            }
            else
            {
                DiversityWorkbench.FormDatabaseConnection fconn = new DiversityWorkbench.FormDatabaseConnection(SC);
                fconn.ShowDialog();
                if (fconn.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else
                    ConnectionStringCacheDB = SC.ConnectionString;
            }
            DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("Files", "Please enter the path for the files of the cache database", Directory);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Directory = f.String;
                f = new DiversityWorkbench.FormGetString("Database", "Please enter the name of the database", DiversityWorkbench.Settings.DatabaseName + "_Cache");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Database = f.String;
                }
                else
                    return;

                string ProjectDB = "";
                System.Collections.Generic.Dictionary<int, string> ProjectDBs = new Dictionary<int, string>();
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                    {
                        if (KVconn.Value.ModuleName == "DiversityProjects")
                        {
                            ProjectDBs.Add(i, KVconn.Value.DatabaseName);
                            ProjectDB = KVconn.Value.DatabaseName;
                            i++;
                        }
                    }
                }
                if (ProjectDBs.Count == 0)
                    return;
                if (ProjectDBs.Count > 1)
                {
                    DiversityWorkbench.FormGetStringFromList fProject = new DiversityWorkbench.FormGetStringFromList(ProjectDBs, "Projects database", "Please select the database containing the project definitions");
                    if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK && fProject.String.Length > 0)
                        ProjectDB = fProject.String;
                    else
                        return;
                }
                SQL = "USE [master]; " +
                    "CREATE DATABASE [" + Database + "] ON  PRIMARY " +
                    "( NAME = N'" + Database + "_Data', FILENAME = N'" + Directory + "\\" + Database + ".mdf' , SIZE = 50000 , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) " +
                    "LOG ON  " +
                    "( NAME = N'" + Database + "_Log', FILENAME = N'" + Directory + "\\" + Database + "_log.ldf' , SIZE = 504 , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) ";
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionStringCacheDB);
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = 999999;
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    C.CommandText = "USE " + Database + ";";
                    C.ExecuteNonQuery();
                    C.CommandText = "CREATE FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '03.00.00' END;";
                    C.ExecuteNonQuery();
                    C.CommandText = "GRANT EXEC ON [dbo].[Version] TO public";
                    C.ExecuteNonQuery();
                    SQL = "SELECT dbo.BaseURL()";
                    string BaseURL = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                    C.CommandText = "CREATE FUNCTION [dbo].[BaseUrlSource] () RETURNS varchar(255) AS BEGIN RETURN '" + BaseURL + "' END;";
                    C.ExecuteNonQuery();
                    C.CommandText = "GRANT EXEC ON [dbo].[BaseUrlSource] TO public";
                    C.ExecuteNonQuery();
                    DiversityCollection.CacheDatabase.DataSetCacheDB.CacheDatabaseRow R = this.dataSetCacheDB.CacheDatabase.NewCacheDatabaseRow();
                    R.DatabaseName = Database;
                    R.Server = SC.DatabaseServer;
                    R.Port = (short)SC.DatabaseServerPort;
                    R.ProjectsDatabaseName = ProjectDB;
                    this.dataSetCacheDB.CacheDatabase.Rows.Add(R);
                    this._SqlDataAdapterCacheDatabase.Update(this.dataSetCacheDB.CacheDatabase);
                }
                catch (System.Exception ex) { }
                finally
                {
                    con.Close();
                }
                this.setUpdateControls();
            }
        }

        private void toolStripButtonDatabaseDelete_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("available in upcoming version");
            return;


            string SQL = "USE [master] " +
                "GO " +
                "IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "') " +
                "DROP DATABASE [" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "] " +
                "GO ";
        }

        private void listBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setDatabase();
        }

        private void setDatabase()
        {
            if (this.listBoxDatabase.SelectedIndex > -1)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxDatabase.SelectedItem;
                DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R["DatabaseName"].ToString();
                DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R["Server"].ToString();
                DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R["Port"].ToString());
                DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R["Version"].ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
                {
                    this.labelProjectsDatabase.Visible = false;
                    this.textBoxProjectsDatabase.Visible = false;
                    this.toolStripDatabase.Visible = false;
                    this.toolStripButtonDatabaseNew.Visible = false;
                    this.toolStripButtonDatabaseDelete.Visible = false;
                }
                this.initProjects();
                this.InitTaxonList();
                this.setUpdateControls();
            }
            else
            {
                this.tableLayoutPanelDatabase.Enabled = false;
                if (this.tabControlMain.TabPages.Contains(this.tabPageProjects))
                    this.tabControlMain.TabPages.Remove(this.tabPageProjects);
                if (this.tabControlMain.TabPages.Contains(this.tabPageTaxonomy))
                    this.tabControlMain.TabPages.Remove(this.tabPageTaxonomy);
            }
        }
       
        #endregion

        #region Updates

        private void buttonUpdateDatabase_Click(object sender, EventArgs e)
        {
            this.UpdateDatabase();
        }

        private void setUpdateControls()
        {
            string SQL = "select user_name()";
            string User = "";
            //bool UpdateDatabase = false;
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    User = C.ExecuteScalar().ToString();
                    con.Close();
                    if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
                    {
                        if (DiversityWorkbench.Database.DatabaseRoles().Contains("CacheAdministrator") || User == "dbo")
                        {
                            bool Update  = this.UpdateCheckForDatabase();
                            this.buttonUpdateDatabase.Visible = Update;
                            this.textBoxCurrentDatabaseVersion.Visible = true;// this.buttonUpdateDatabase.Visible;
                            this.labelCurrentDatabaseVersion.Visible = true;// this.buttonUpdateDatabase.Visible;
                            this.textBoxAvailableDatabaseVersion.Visible = Update;
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
                catch { }
            }
        }

        private bool UpdateCheckForDatabase()
        {

            bool Update = false;
            string SQL = "select dbo.Version()";
            string Version = "";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);// DiversityWorkbench.Settings.ConnectionString);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Version = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch (System.Exception ex) { }
            if (Version.StartsWith("01") || Version.StartsWith("00"))
            {
                if (Version != DiversityCollection.Properties.Settings.Default.CacheDBV1)
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

                    string[] VersionC_Parts = DiversityCollection.Properties.Settings.Default.CacheDBV1.Split(new Char[] { '.' });
                    if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                    if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                    if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                    if (C_versionMajor > DBversionMajor ||
                        (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                        (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                        Update = true;
                    //if (DBversionMajor == 2)
                    //{
                    //    Version = "00.00.00";
                    //    Update = true;
                    //}
                }
                this.textBoxCurrentDatabaseVersion.Text = Version;
                this.textBoxAvailableDatabaseVersion.Visible = true;
                this.textBoxAvailableDatabaseVersion.Text = DiversityCollection.Properties.Settings.Default.CacheDBV1;
                //if (Update) this.labelV1Update.Text = "Your version of the database is " + Version + ". Please update the database to version " + DiversityCollection.Properties.Settings.Default.CacheDBV1;
                //else this.labelV1Update.Text = "No updates for database";
                //this.panelV1Update.Visible = Update;
                return Update;
            }
            else if (Version != DiversityCollection.Properties.Settings.Default.DatabaseVersion)
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

                string[] VersionC_Parts = DiversityCollection.Properties.Settings.Default.DatabaseVersion.Split(new Char[] { '.' });
                if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                if (C_versionMajor > DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                    Update = true;
            }
            this.textBoxCurrentDatabaseVersion.Text = Version;
            this.textBoxAvailableDatabaseVersion.Text =  DiversityCollection.Properties.Settings.Default.CacheDatabaseVersion;
            return Update;
        }

        private void UpdateDatabase()
        {
            string DatabaseCurrentVersion = "";
            string SQL = "select dbo.Version()";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);// DiversityWorkbench.Settings.ConnectionString);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                DatabaseCurrentVersion = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch { }
            DatabaseCurrentVersion = DatabaseCurrentVersion.Replace(".", "").Replace("/", "");
            string DatabaseFinalVersion = DiversityCollection.Properties.Settings.Default.CacheDatabaseVersion;
            DatabaseFinalVersion = DatabaseFinalVersion.Replace(".", "").Replace("/", "");

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
                        if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
                        {
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdateV1_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                        else
                        {
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdate_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }
            }

            if (Versions.Count > 0)
            {
                if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
                {
                    this.UpdateForV1();
                    DiversityWorkbench.FormUpdateDatabase f = new DiversityWorkbench.FormUpdateDatabase(DiversityCollection.CacheDatabase.CacheDB.DatabaseName, DiversityCollection.Properties.Settings.Default.CacheDBV1, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, Versions, System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm");
                    f.ShowDialog();
                }
                else
                {
                    DiversityWorkbench.FormUpdateDatabase f = new DiversityWorkbench.FormUpdateDatabase(DiversityCollection.CacheDatabase.CacheDB.DatabaseName, DiversityCollection.Properties.Settings.Default.CacheDatabaseVersion, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, Versions, System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm");
                    f.ShowDialog();
                }
                //if (f.Reconnect) this.setDatabase();
                this.setUpdateControls();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
            }
        }

        /// <summary>
        /// this function creates all objects that need the name of the current source database
        /// </summary>
        private void UpdateForV1()
        {

            if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.Replace(".", ":") == "01:00:00" ||
                DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.Replace(".", ":") == "01:00:01")
            {
                // the view on the table CollectionEventLocalisation
                string SQL = "CREATE VIEW CollectionEventLocalisation " +
                    "AS SELECT L.* " +
                    "FROM " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionEventLocalisation AS L INNER JOIN " + 
                    DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID " +
                    "WHERE (E.DataWithholdingReason = '') OR " +
                    "(E.DataWithholdingReason IS NULL)";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);

                // a view providing the CollectionEventID in the table CollectionSpecimen (was missing in original view, that could not be changed via script due to local variations)
                SQL = "CREATE VIEW [dbo].[CollectionSpecimenEventID] " +
                    "AS " +
                    "SELECT S.CollectionSpecimenID, S.CollectionEventID " +
                    "FROM " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionSpecimen AS S WHERE NOT S.CollectionEventID IS NULL ";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                SQL = "GRANT SELECT ON [CollectionSpecimenEventID] TO [CollectionCacheUser]";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);

                return;
                ///TODO: Freigeben sobald klar ist ob alles passt

                // a view providing the kingdom according to the taxonomic group of the IdentificationUnit
                SQL = "CREATE VIEW [dbo].[Kingdom] " + 
                    "AS " +
                    "SELECT DISTINCT CASE WHEN Code = 'mollusc' OR code = 'animal' OR code = 'arthropod' OR code = 'insect' OR code = 'echinoderm' OR " +
                    "code = 'vertebrate' OR code = 'fish' OR code = 'amphibian' OR code = 'reptile' OR code = 'mammal' OR code = 'cnidaria' OR " +
                    "code = 'evertebrate' OR code = 'bird' THEN 'Animalia' " +
                    "ELSE CASE WHEN code = 'bryophyte' OR code = 'plant' THEN 'Plantae' " +
                    "ELSE CASE WHEN code = 'bacterium' THEN 'Bacteria' " +
                    "ELSE CASE WHEN code = 'virus' THEN 'Viruses' " +
                    "ELSE CASE WHEN code = 'fungus' OR code = 'lichen' THEN 'Fungi' " +
                    "ELSE CASE WHEN code = 'myxomycete' OR code = 'protozoa' THEN 'Protozoa' " +
                    "ELSE CASE WHEN code = 'chromista' THEN 'Chromista' " +
                    "ELSE CASE WHEN code = 'archaea' THEN 'Archaea' " +
                    "ELSE 'incertae sedis' END END END END END END END END AS Kingdom, Code AS TaxonomicGroup " +
                    "FROM  " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollTaxonomicGroup_Enum ";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                SQL = "GRANT SELECT ON [Kingdom] TO [CollectionCacheUser]";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);

                // adding the column for the kingdom to the view
                SQL = "ALTER VIEW [dbo].[IdentificationUnitPart] " +
                    "AS " +
                    "SELECT U.CollectionSpecimenID, U.IdentificationUnitID, P.SpecimenPartID, U.LastIdentificationCache, U.TaxonomicGroup, U.RelatedUnitID, U.RelationType, " +
                    "U.ExsiccataNumber, U.DisplayOrder, U.ColonisedSubstratePart, I.IdentificationQualifier, I.VernacularTerm, I.TaxonomicName, I.Notes, I.TypeStatus, I.SynonymName,  " +
                    "I.AcceptedName, I.AcceptedNameURI, I.TaxonomicRank, I.GenusOrSupragenericName, I.TaxonNameSinAuthor, F.TaxonomicNameFirstEntry, I.LastValidTaxonName,  " +
                    "I.LastValidTaxonWithQualifier, I.LastValidTaxonNameSinAuthor, I.LastValidTaxonIndex, I.LastValidTaxonListOrder, U.FamilyCache, U.OrderCache, U.LifeStage,  " +
                    "U.Gender, P.MaterialCategory, CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber + ' / ' ELSE '' END + CAST(U.IdentificationUnitID AS varchar(90))  " +
                    "AS AccNrUnitID, K.Kingdom AS HigherClassification " +
                    "FROM " + DiversityWorkbench.Settings.DatabaseName + ".dbo.IdentificationUnit AS U INNER JOIN " +
                    "Identification AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND U.IdentificationUnitID = I.IdentificationUnitID INNER JOIN " +
                    "IdentificationFirstEntry AS F ON U.CollectionSpecimenID = F.CollectionSpecimenID AND U.IdentificationUnitID = F.IdentificationUnitID INNER JOIN " +
                    "CollectionSpecimenCache AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                    "IdentificationUnitInPart AS UP ON U.CollectionSpecimenID = UP.CollectionSpecimenID AND U.IdentificationUnitID = UP.IdentificationUnitID INNER JOIN " +
                    "CollectionSpecimenPart AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID AND UP.CollectionSpecimenID = P.CollectionSpecimenID AND " +
                    "UP.SpecimenPartID = P.SpecimenPartID INNER JOIN " +
                    "Kingdom K ON U.TaxonomicGroup = K.TaxonomicGroup";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);

            }
        }

        #endregion

        #region Setting the Cache Server

        private void buttonSetDatabase_Click(object sender, EventArgs e)
        {
            //DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(true);
            //f.ShowDialog();
            //if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            //{
            //    DiversityCollection.CacheDB.SetCacheServer(f.ServerConnection.DatabaseServer + "," + f.ServerConnection.DatabaseServerPort.ToString());
            //    DiversityCollection.CacheDB.SetCacheDatabase(f.ServerConnection.DatabaseName);
            //}
        }

        private void comboBoxSelectCacheServer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (this.comboBoxSelectCacheServer.SelectedValue != null)
            //{
            //    string Server = this.comboBoxSelectCacheServer.SelectedValue.ToString();
            //    if (Server.Length > 0)
            //    {
            //        if (this.SetCacheServer(Server))
            //            this.setInterface();
            //    }
            //}
        }

        private void comboBoxSelectCacheServer_DropDown(object sender, EventArgs e)
        {
            //this.comboBoxSelectCacheServer.DataSource = this.DatabaseList("", "%Collection%Cache%");
            //this.comboBoxSelectCacheServer.DisplayMember = "DatabaseName";
            //this.comboBoxSelectCacheServer.ValueMember = "DatabaseName";
        }

        private bool SetCacheServer(string Server)
        {
            bool OK = false;
            //if (System.Windows.Forms.MessageBox.Show("Should the server " + Server + "\r\nbe established as server for the cache database for the database " + DiversityWorkbench.Settings.DatabaseName, "CacheDatabase", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    DiversityCollection.CacheDB.SetCacheServer(Server);
            //    if (DiversityCollection.CacheDB.DatabaseServer.Length > 0)
            //        OK = true;
            //}
            return OK;
        }

        #endregion

        #region Setting the Project DB

        //private void ShowControlsForProjectDB(bool visible)
        //{
        //    this.labelSelectProjectsDB.Visible = visible;
        //    this.comboBoxSelectProjectsDB.Visible = visible;
        //}
        
        private void comboBoxSelectProjectsDB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (this.comboBoxSelectProjectsDB.SelectedValue != null)
            //{
            //    string DB = this.comboBoxSelectProjectsDB.SelectedValue.ToString();
            //    if (DB.Length > 0)
            //    {
            //        if(this.setProjectDatabase(DB))
            //            this.setInterface();
            //    }   
            //}
        }

        //private void comboBoxSelectProjectsDB_DropDown(object sender, EventArgs e)
        //{
        //    this.comboBoxSelectProjectsDB.DataSource = this.DatabaseList("DiversityProjects", "");
        //    this.comboBoxSelectProjectsDB.DisplayMember = "DatabaseName";
        //    this.comboBoxSelectProjectsDB.ValueMember = "DatabaseName";
        //}
        
        public bool setProjectDatabase(string Database)
        {
            bool OK = false;
            //if (System.Windows.Forms.MessageBox.Show("Should the database " + Database + " be established as source for the project definitions for the ", "Project source", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    if (DiversityCollection.CacheDB.DatabaseName.Length > 0)
            //    {
            //        DiversityCollection.CacheDB.ProjectsDatabase = Database;
            //        //string SQL = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityProjectsDatabaseName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
            //        //    "DROP FUNCTION [dbo].[DiversityProjectsDatabaseName]";
            //        //try
            //        //{
            //        //    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.ConnectionStringCacheDB);
            //        //    System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            //        //    con.Open();
            //        //    C.ExecuteNonQuery();
            //        //    C.CommandText = "CREATE FUNCTION [dbo].[DiversityProjectsDatabaseName] () RETURNS nvarchar(50) " +
            //        //    "AS BEGIN RETURN '" + Database + "' END";
            //        //    C.ExecuteNonQuery();
            //        //    con.Close();
            //        //    con.Dispose();
            //        //    OK = true;
            //        //}
            //        //catch (System.Exception ex) { }
            //        this.setInterface();
            //    }
            //}
            return OK;
        }

        #endregion

        #region Taxonomy
        
        private void toolStripButtonTaxonomyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(T);

                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    DiversityCollection.CacheDatabase.DataSetCacheDB.DiversityTaxonNamesRow R = this.dataSetCacheDB.DiversityTaxonNames.NewDiversityTaxonNamesRow();
                    R.DataSource = f.ServerConnection.DatabaseServer + "," + f.ServerConnection.DatabaseServerPort.ToString();
                    R.DatabaseName = f.ServerConnection.DatabaseName;
                    this.dataSetCacheDB.DiversityTaxonNames.Rows.Add(R);
                    if (this._SqlDataAdapterDiversityTaxonNames.UpdateCommand == null)
                    {
                        string SQL = "INSERT INTO DiversityTaxonNamesSources " +
                            "(DataSource, DatabaseName) " +
                        "VALUES     ('" + R.DataSource + "', '" + R.DatabaseName + "')";
                        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    }
                    else
                        this._SqlDataAdapterDiversityTaxonNames.Update(this.dataSetCacheDB.DiversityTaxonNames);

                    //DiversityCollection.CacheDB.AddTaxonDatabaseConnection(f.ServerConnection.ConnectionString);
                    //this._TaxonControls = null;
                    this.RequeryTaxonList();
                    //this.initReset();
                }
            }
            catch (System.Exception ex) { }
        }

        private void InitTaxonList()
        {
            if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
            {
                if (this.tabControlMain.TabPages.Contains(this.tabPageTaxonomy))
                {
                    this.tabControlMain.TabPages.Remove(this.tabPageTaxonomy);
                }
                return;
            }
            bool OK = false;
            try
            {
                this.dataSetCacheDB.DiversityTaxonNames.Clear();
                string SQL = "SELECT DataSource, DatabaseName, LastUpdate " +
                    "FROM DiversityTaxonNamesSources ORDER BY DataSource, DatabaseName";
                this._SqlDataAdapterDiversityTaxonNames = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                this._SqlDataAdapterDiversityTaxonNames.Fill(this.dataSetCacheDB.DiversityTaxonNames);
                System.Data.SqlClient.SqlCommandBuilder cb = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterDiversityTaxonNames);
                this.RequeryTaxonList();
                OK = true;
            }
            catch (System.Exception ex) { }
            if (OK)
            {
                if (!this.tabControlMain.TabPages.Contains(this.tabPageTaxonomy))
                {
                    this.tabControlMain.TabPages.Add(this.tabPageTaxonomy);
                }
            }
            else
            {
                if (this.tabControlMain.TabPages.Contains(this.tabPageTaxonomy))
                {
                    this.tabControlMain.TabPages.Remove(this.tabPageTaxonomy);
                }
            }
        }

        private void RequeryTaxonList()
        {
            this.panelTaxonomy.Controls.Clear();
            this._TaxonControls = null;
            //if (this.TaxonControls != null)
            //{
                foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.UserControl> KV in this.TaxonControls)
                {
                    this.panelTaxonomy.Controls.Add(KV.Value);
                    KV.Value.Dock = DockStyle.Top;
                }
            //}
        }

        private System.Collections.Generic.Dictionary<string, System.Windows.Forms.UserControl> _TaxonControls;
        private System.Collections.Generic.Dictionary<string, System.Windows.Forms.UserControl> TaxonControls
        {
            get
            {
                if (this._TaxonControls == null)
                {
                    this._TaxonControls = new Dictionary<string, UserControl>();
                    foreach (System.Data.DataRow R in this.dataSetCacheDB.DiversityTaxonNames.Rows)
                    {
                        System.Windows.Forms.UserControl U = new UserControl();
                        U.Height = 30;

                        System.Windows.Forms.Button B = new Button();
                        B.Image = global::DiversityCollection.Resource.Delete;
                        this.toolTip.SetToolTip(B, "Remove the selected datasource from the list");
                        B.Click += this.taxonControlDeleteButton_Click;
                        B.Dock = DockStyle.Right;
                        B.Width = 23;
                        B.Tag = R;
                        U.Controls.Add(B);

                        System.Windows.Forms.Label L = new Label();
                        L.Text = "Server: " + R["DataSource"].ToString() + " Database: " + R["DatabaseName"].ToString();
                        L.Dock = DockStyle.Fill;
                        L.AutoSize = false;
                        L.Width = 500;
                        L.TextAlign = ContentAlignment.MiddleLeft;
                        L.Tag = R;
                        L.Click += this.taxonControl_Click;
                        U.Controls.Add(L);

                        U.Tag = this.ConnectionStringForTaxonSource(R);

                        this._TaxonControls.Add(R["DataSource"].ToString() + "|" + R["DatabaseName"].ToString(), U);
                    }
                }
                return this._TaxonControls;
            }
        }

        private string ConnectionStringForTaxonSource(System.Data.DataRow R)
        {
            string CS = "";
            DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
            S.DatabaseName = R["DatabaseName"].ToString();
            S.DatabaseServer = R["DataSource"].ToString();
            S.IsTrustedConnection = DiversityWorkbench.Settings.ServerConnection.IsTrustedConnection;
            if (!S.IsTrustedConnection)
            {
                S.DatabaseUser = DiversityWorkbench.Settings.ServerConnection.DatabaseUser;
                S.DatabasePassword = DiversityWorkbench.Settings.Password;
            }
            CS = S.ConnectionString;
            return CS;
        }

        private void taxonControl_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Label L = (System.Windows.Forms.Label)sender;
            this.labelTaxonomyProjects.Text = "Projects and their sequence used for the datasource\r\n" + L.Text;
            System.Data.DataRow R = (System.Data.DataRow)L.Tag;
            this.SetTaxonSourceProjects(R["DataSource"].ToString(), R["DatabaseName"].ToString());
            this._DiversityTaxonNamesConnectionString = this.ConnectionStringForTaxonSource(R);
            this._DiversityTaxonNamesDataBase = R["DatabaseName"].ToString();
            this._DiversityTaxonNamesDataSource = R["DataSource"].ToString();
        }

        private void SetTaxonSourceProjects(string DataSource, string DatabaseName)
        {
            this.dataSetCacheDB.DiversityTaxonNamesProjectSequence.Clear();
            string SQL = "SELECT DataSource, DatabaseName, ProjectID, Sequence, Project " +
                "FROM         DiversityTaxonNamesProjectSequence " +
                "WHERE     (DataSource = '" + DataSource + "') AND (DatabaseName = '" + DatabaseName + "')";
            this._SqlDataAdapterDiversityTaxonNamesProjectSequence = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            this._SqlDataAdapterDiversityTaxonNamesProjectSequence.Fill(this.dataSetCacheDB.DiversityTaxonNamesProjectSequence);
            System.Data.SqlClient.SqlCommandBuilder cb = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterDiversityTaxonNamesProjectSequence);
        }

        private void taxonControlDeleteButton_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to remove this datasource?", "Remove?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                if (B.Tag != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)B.Tag;
                    if (this._SqlDataAdapterDiversityTaxonNames.UpdateCommand != null)
                    {
                        R.Delete();
                        this._SqlDataAdapterDiversityTaxonNames.Update(this.dataSetCacheDB.DiversityTaxonNames);
                    }
                    else
                    {
                        string SQL = "DELETE FROM DiversityTaxonNamesSources WHERE DataSource = '" + R["DataSource"].ToString() + "' AND DatabaseName = '" + R["DatabaseName"].ToString() + "'";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            System.Windows.Forms.MessageBox.Show("Deleting failed");
                    }
                    this.labelTaxonomyProjects.Text = "Projects and their sequence used for a datasource";
                    this.dataSetCacheDB.DiversityTaxonNamesProjectSequence.Clear();
                    this._DiversityTaxonNamesConnectionString = "";
                    this._DiversityTaxonNamesDataBase = "";
                    this._DiversityTaxonNamesDataSource = "";
                    this.InitTaxonList();
                }
            }
            //this._SelectedTaxonSource = null;
        }

        private void toolStripButtonTaxonomyRemove_Click(object sender, EventArgs e)
        {
            //System.Data.DataRow R = 
            this.RequeryTaxonList();
        }

        private void toolStripButtonTaxonProjectAdd_Click(object sender, EventArgs e)
        {
            if (this._DiversityTaxonNamesConnectionString == null || this._DiversityTaxonNamesConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a datasource");
                return;
            }
            string SQL = "";
            foreach (System.Data.DataRow R in this.dataSetCacheDB.DiversityTaxonNamesProjectSequence.Rows)
            {
                if (SQL.Length > 0) SQL += ", ";
                SQL += R["ProjectID"];
            }
            if (SQL.Length > 0)
                SQL = " WHERE P.ProjectID NOT IN (" + SQL + ")";
            SQL = "SELECT DISTINCT P.ProjectID, P.Project " +
                "FROM ProjectProxy AS P INNER JOIN " +
                "TaxonAcceptedName AS A ON P.ProjectID = A.ProjectID " + SQL +
                "ORDER BY P.Project";
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, this._DiversityTaxonNamesConnectionString);
            System.Data.DataTable dt = new DataTable();
            ad.Fill(dt);
            DiversityWorkbench.FormGetStringFromList f = new DiversityWorkbench.FormGetStringFromList(dt, "Project", "ProjectID", "Project", "Please select a project");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityCollection.CacheDatabase.DataSetCacheDB.DiversityTaxonNamesProjectSequenceRow Rnew = this.dataSetCacheDB.DiversityTaxonNamesProjectSequence.NewDiversityTaxonNamesProjectSequenceRow();
                Rnew.Project = f.SelectedString;
                Rnew.ProjectID = int.Parse(f.SelectedValue);
                Rnew.DatabaseName = this._DiversityTaxonNamesDataBase;
                Rnew.DataSource = this._DiversityTaxonNamesDataSource;
                int Sequence = 1;
                System.Data.DataRow[] rr = this.dataSetCacheDB.DiversityTaxonNamesProjectSequence.Select("", "Sequence DESC");
                if (rr.Length > 0)
                    Sequence = int.Parse(rr[0]["Sequence"].ToString()) + 1;
                Rnew.Sequence = Sequence;
                this.dataSetCacheDB.DiversityTaxonNamesProjectSequence.Rows.Add(Rnew);
            }
            this._SqlDataAdapterDiversityTaxonNamesProjectSequence.Update(this.dataSetCacheDB.DiversityTaxonNamesProjectSequence);
        }

        private void toolStripButtonTaxonProjectRemove_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to remove this project?", "Remove?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
            }
        }

        private void dataGridViewTaxonProject_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        #endregion

        #region User
        
        private void buttonLoginAdministration_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT dbo.DiversityWorkbenchModule()";
            string Module = "";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            C.CommandTimeout = 999;
            try
            {
                con.Open();
                Module = C.ExecuteScalar().ToString();
            }
            catch(System.Exception ex){}
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
            //DiversityCollection.LookupTable.ResetUser();
        }

        #endregion

        #region Projects

        private void initProjects()
        {
            if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
            {
                try
                {
                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    if (this._SqlDataAdapterProjectProxy == null)
                        this._SqlDataAdapterProjectProxy = new System.Data.SqlClient.SqlDataAdapter();
                    if (this._SqlDataAdapterProjectProxy.SelectCommand == null)
                    {
                        string SQL = "SELECT ProjectID, Project, ProjectURI, CoordinatePrecision FROM ProjectPublished ORDER BY Project";
                        this._SqlDataAdapterProjectProxy.SelectCommand = new System.Data.SqlClient.SqlCommand(SQL, con);
                    }
                    try
                    {
                        this._SqlDataAdapterProjectProxy.Fill(this.dataSetCacheDB.ProjectProxy);
                        if (this._SqlDataAdapterProjectProxy.UpdateCommand == null)
                        {
                            System.Data.SqlClient.SqlCommandBuilder cb = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterProjectProxy);
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    if (!this.tabControlMain.TabPages.Contains(this.tabPageProjects))
                    {
                        this.tabControlMain.TabPages.Add(this.tabPageProjects);
                    }
                    this.checkBoxRestrictImages.Visible = false;
                    this.checkBoxRestrictLocalisations.Visible = false;
                    this.checkBoxRestrictMaterialCategories.Visible = false;
                    this.checkBoxRestrictTaxonomicGroups.Visible = false;
                    if (this.tabControlProject.TabPages.Contains(this.tabPageTaxonomicGroups))
                        this.tabControlProject.TabPages.Remove(this.tabPageTaxonomicGroups);
                    if (this.tabControlProject.TabPages.Contains(this.tabPageMaterialCategories))
                        this.tabControlProject.TabPages.Remove(this.tabPageMaterialCategories);
                    if (this.tabControlProject.TabPages.Contains(this.tabPageLocalisations))
                        this.tabControlProject.TabPages.Remove(this.tabPageLocalisations);
                    if (this.tabControlProject.TabPages.Contains(this.tabPageImages))
                        this.tabControlProject.TabPages.Remove(this.tabPageImages);
                    //this.toolStripButtonProjectAdd.Enabled = false;
                    //this.toolStripButtonProjectRemove.Enabled = false;
                }
                catch (System.Exception ex)
                {
                }
                //if (this.tabControlMain.TabPages.Contains(this.tabPageProjects))
                //{
                //    this.tabControlMain.TabPages.Remove(this.tabPageProjects);
                //}
                return;
            }
            bool OK = false;
            //int ProjectID;
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                if (this._SqlDataAdapterProjectProxy == null)
                    this._SqlDataAdapterProjectProxy = new System.Data.SqlClient.SqlDataAdapter();
                if (this._SqlDataAdapterProjectProxy.SelectCommand == null)
                {
                    string SQL = "SELECT ProjectID, Project, ProjectURI /*, ProjectSettings, DataType, CoordinatePrecision*/ FROM ProjectProxy ORDER BY Project";
                    this._SqlDataAdapterProjectProxy.SelectCommand = new System.Data.SqlClient.SqlCommand(SQL, con);
                }
                this._SqlDataAdapterProjectProxy.Fill(this.dataSetCacheDB.ProjectProxy);
                if (this._SqlDataAdapterProjectProxy.UpdateCommand == null)
                {
                    System.Data.SqlClient.SqlCommandBuilder cb = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterProjectProxy);
                }
                string SqlEnum = "SELECT Code, DisplayText FROM  ProjectDataType_Enum";
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SqlEnum, con);
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                this.comboBoxProjectDataType.DataSource = dt;
                this.comboBoxProjectDataType.DisplayMember = "DisplayText";
                this.comboBoxProjectDataType.ValueMember = "Code";
                OK = true;
            }
            catch (System.Exception ex) { }
            if (OK)
            {
                if (!this.tabControlMain.TabPages.Contains(this.tabPageProjects))
                {
                    this.tabControlMain.TabPages.Add(this.tabPageProjects);
                    if (this.tabControlProject.TabPages.Contains(this.tabPageTaxonomicGroups))
                        this.tabControlProject.TabPages.Remove(this.tabPageTaxonomicGroups);
                    if (this.tabControlProject.TabPages.Contains(this.tabPageMaterialCategories))
                        this.tabControlProject.TabPages.Remove(this.tabPageMaterialCategories);
                    if (this.tabControlProject.TabPages.Contains(this.tabPageLocalisations))
                        this.tabControlProject.TabPages.Remove(this.tabPageLocalisations);
                    if (this.tabControlProject.TabPages.Contains(this.tabPageImages))
                        this.tabControlProject.TabPages.Remove(this.tabPageImages);
                }
            }
            else
            {
                if (this.tabControlMain.TabPages.Contains(this.tabPageProjects))
                {
                    this.tabControlMain.TabPages.Remove(this.tabPageProjects);
                }
            }
        }

        private void listBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxProjects.SelectedItem == null)
                return;
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            if (!RV.Row["ProjectID"].Equals(System.DBNull.Value))
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());

                if (RV.Row["CoordinatePrecision"].Equals(System.DBNull.Value))
                {
                    this.checkBoxCoordinatePrecision.Checked = false;
                    this.numericUpDownCoordinatePrecision.Visible = false;
                }
                else
                {
                    this.checkBoxCoordinatePrecision.Checked = true;
                    this.numericUpDownCoordinatePrecision.Visible = true;
                    this.numericUpDownCoordinatePrecision.Value = int.Parse(RV["CoordinatePrecision"].ToString());
                }

                if (!DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
                {
                    this.initTaxonomicGroups(ProjectID);

                    this.initMaterialCategory(ProjectID);

                    this.initLocalisations(ProjectID);

                    this.initImages(ProjectID);

                    this.toolStripButtonProjectSave.Visible = true;
                }
                else
                {
                    this.toolStripButtonProjectSave.Visible = false;
                }
            }
        }

        private void toolStripButtonProjectAdd_Click(object sender, EventArgs e)
        {
            if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
            {
                try
                {
                    System.Data.DataTable dtProjectPublished = new DataTable("ProjectProxy");
                    string SQL = "SELECT ProjectID, Project, ProjectURI, CoordinatePrecision " +
                        "FROM ProjectPublished";
                    System.Data.SqlClient.SqlDataAdapter adProjectPublished = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    adProjectPublished.Fill(dtProjectPublished);
                    SQL = "";
                    foreach(System.Data.DataRow R in dtProjectPublished.Rows)
                    {
                        if (SQL.Length > 0) SQL += ", ";
                        SQL += R["ProjectID"].ToString();
                    }
                    foreach (System.Data.DataRow R in this.dataSetCacheDB.ProjectProxy.Rows)
                    {
                        if (SQL.Length > 0) SQL += ", ";
                        SQL += R["ProjectID"].ToString();
                    }
                    if (SQL.Length > 0)
                    {
                        SQL = "SELECT ProjectID, Project, ProjectURI " +
                            "FROM ProjectProxy " +
                            "WHERE (NOT (ProjectID IN (" + SQL + "))) " +
                            "ORDER BY Project";
                    }
                    else
                        SQL = "SELECT ProjectID, Project, ProjectURI " +
                            "FROM ProjectProxy " +
                            "ORDER BY Project";
                    System.Data.DataTable dtProjectProxy = new DataTable();
                    System.Data.SqlClient.SqlDataAdapter adProjectProxy = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    adProjectProxy.Fill(dtProjectProxy);

                    DiversityWorkbench.FormGetStringFromList f = new DiversityWorkbench.FormGetStringFromList(dtProjectProxy, "Project", "ProjectID", "New project", "Please select a project from the list", "", false);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        System.Data.DataRowView RN = (System.Data.DataRowView)this.projectProxyBindingSource.AddNew();
                        RN.BeginEdit();
                        RN["ProjectID"] = int.Parse(f.SelectedValue);
                        RN["Project"] = f.SelectedString;
                        string ProjectURI = "";
                        System.Data.DataRow[] rr = dtProjectProxy.Select("ProjectID = " + f.SelectedValue.ToString());
                        if (rr.Length > 0)
                            ProjectURI = rr[0]["ProjectURI"].ToString();
                        RN["ProjectURI"] = ProjectURI;
                        RN.EndEdit();
                        //this.initProjects();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                try
                {
                    System.Data.DataTable dtProjectProxy = new DataTable("ProjectProxy");
                    string SQL = "SELECT ProjectID, Project, ProjectURI, ProjectSettings, DataType, CoordinatePrecision " +
                        "FROM ProjectProxy";
                    System.Data.DataRowView RP = (System.Data.DataRowView)this.cacheDatabaseBindingSource.Current;
                    string ProjectsDatabase = RP["ProjectsDatabaseName"].ToString();

                    System.Data.SqlClient.SqlDataAdapter adProjectProxy = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    adProjectProxy.Fill(dtProjectProxy);
                    DataSet DsProjectProxy = new DataSet();
                    DsProjectProxy.Tables.Add(dtProjectProxy);
                    System.Data.SqlClient.SqlCommandBuilder cb = new System.Data.SqlClient.SqlCommandBuilder(adProjectProxy);
                    System.Windows.Forms.BindingSource ProjectBindingSource = new System.Windows.Forms.BindingSource(DsProjectProxy, "ProjectProxy");
                    ((System.ComponentModel.ISupportInitialize)(ProjectBindingSource)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(ProjectBindingSource)).EndInit();
                    DiversityWorkbench.Forms.FormLoginAdminGetProjects f = new DiversityWorkbench.Forms.FormLoginAdminGetProjects(dtProjectProxy, DiversityWorkbench.Settings.ServerConnection, ProjectsDatabase);
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (f.AddedProjectRows.Count > 0)
                        {
                            foreach (System.Data.DataRow R in f.AddedProjectRows)
                            {
                                System.Data.DataRow[] rr = dtProjectProxy.Select("ProjectID = " + R["ProjectID"].ToString());
                                if (rr.Length == 0)
                                {
                                    System.Data.DataRowView RN = (System.Data.DataRowView)this.projectProxyBindingSource.AddNew();
                                    RN.BeginEdit();
                                    RN["ProjectID"] = R["ProjectID"];
                                    RN["Project"] = R["Project"];
                                    RN["ProjectURI"] = R["ProjectURI"];
                                    RN["ProjectSettings"] = R["ProjectSettings"];
                                    RN.EndEdit();
                                }
                                else
                                {
                                    if (R["ProjectID"].ToString() == rr[0]["Project"].ToString())
                                    {
                                        rr[0]["ProjectURI"] = R["ProjectURI"];
                                    }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("The ProjectID " + R["ProjectID"].ToString() + " for the project " + R["Project"].ToString() + " is used for the project " + rr[0]["Project"].ToString());
                                    }
                                }
                            }

                            //ProjectBindingSource.EndEdit();
                            //adProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
                            this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
                            //this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
                            this.projectProxyBindingSource.ResetBindings(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void comboBoxProjectDataType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
        }

        private void toolStripButtonProjectRemove_Click(object sender, EventArgs e)
        {
            if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
            {
                this.projectProxyBindingSource.RemoveCurrent();
                this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
            }
            else
            {
                this.projectProxyBindingSource.RemoveCurrent();
                this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
            }
        }

        private void toolStripButtonProjectSave_Click(object sender, EventArgs e)
        {
            if (this.projectProxyBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectProxyBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
                this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB.ProjectProxy);
            }
        }

        private void buttonShowProjectDataTypeDescription_Click(object sender, EventArgs e)
        {

        }

        #region Transfersettings
        
        private void SetProjectTransferSetting_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox C = (System.Windows.Forms.CheckBox)sender;
            System.Data.DataRow R = (System.Data.DataRow)C.Tag;
            int TransferToCache = 0;
            if (C.Checked)
                TransferToCache = 1;
            string SQL = "UPDATE T SET T.TransferToCache = " + TransferToCache + " FROM ProjectTransferSetting T WHERE T.ProjectID = " + R["ProjectID"] + " AND T.TransferSetting = '" + R["TransferSetting"].ToString() + "' AND T.Value = '" + R["Value"].ToString() + "'";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        }

        private void initTransferSettings(int ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting, System.Windows.Forms.Panel Panel)
        {
            Panel.Controls.Clear();
            foreach (System.Data.DataRow R in DiversityCollection.CacheDatabase.CacheDB.DtProjectTransferSetting(ProjectID, TransferSetting).Rows)
            {
                bool IsUsed = bool.Parse(R["TransferToCache"].ToString());
                System.Windows.Forms.CheckBox C = new CheckBox();
                C.Text = R["DisplayText"].ToString();
                C.Checked = IsUsed;
                C.Tag = R;
                C.Click += SetProjectTransferSetting_Click;
                C.Dock = DockStyle.Top;
                C.Margin = new Padding(0);
                C.Height = 18;
                Panel.Controls.Add(C);
                C.BringToFront();
            }
        }

        private void RefreshTransferSetting(int ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting, System.Windows.Forms.Panel Panel)
        {
            Panel.Controls.Clear();
            foreach (System.Data.DataRow R in DiversityCollection.CacheDatabase.CacheDB.DtProjectTransferSetting(ProjectID, TransferSetting).Rows)
            {
                bool IsUsed = bool.Parse(R["TransferToCache"].ToString());
                System.Windows.Forms.CheckBox C = new CheckBox();
                C.Text = R["DisplayText"].ToString();
                C.Checked = IsUsed;
                C.Tag = R;
                C.Click += SetProjectTransferSetting_Click;
                C.Dock = DockStyle.Top;
                C.Margin = new Padding(0);
                C.Height = 18;
                Panel.Controls.Add(C);
                C.BringToFront();
            }
        }
        
        #endregion

        private void SetProjectRestriction(System.Windows.Forms.CheckBox CheckBox, string DisplayText, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting)
        {
            int ProjectID;
            bool Requery = false;
            System.Data.DataRowView R = (System.Data.DataRowView)this.projectProxyBindingSource.Current;
            if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
            {
                if (CheckBox.Checked)
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to add a restriction to the " + DisplayText + "?", "Add restriction", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (TransferSetting == CacheDatabase.CacheDB.TransferSetting.Images)
                        {
                            bool OK = false;
                            if (DiversityCollection.CacheDatabase.CacheDB.AddProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesSpecimen)) OK = true;
                            if (DiversityCollection.CacheDatabase.CacheDB.AddProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesEvent)) OK = true;
                            if (DiversityCollection.CacheDatabase.CacheDB.AddProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesSeries)) OK = true;
                            if (OK)
                                Requery = true;
                        }
                        else
                        {
                            if (DiversityCollection.CacheDatabase.CacheDB.AddProjectTransferSetting(ProjectID, TransferSetting))
                                Requery = true;
                        }
                    }
                    else
                        CheckBox.Checked = false;
                }
                else
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to remove the restriction on " + DisplayText + "?", "Remove restriction", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (DiversityCollection.CacheDatabase.CacheDB.RemoveProjectTransferSetting(ProjectID, TransferSetting))
                            Requery = true;
                    }
                    else
                        CheckBox.Checked = true;
                }
                if (Requery)
                {
                    switch (TransferSetting)
                    {
                        case DiversityCollection.CacheDatabase.CacheDB.TransferSetting.TaxonomicGroup:
                            this.initTaxonomicGroups(ProjectID);
                            break;
                        case DiversityCollection.CacheDatabase.CacheDB.TransferSetting.MaterialCategory:
                            this.initMaterialCategory(ProjectID);
                            break;
                        case DiversityCollection.CacheDatabase.CacheDB.TransferSetting.Localisation:
                            this.initLocalisations(ProjectID);
                            break;
                        case CacheDatabase.CacheDB.TransferSetting.Images:
                            this.initImages(ProjectID);
                            break;
                    }
                }
            }
        }

        #region Taxonomic groups

        private void initTaxonomicGroups(int ProjectID)
        {
            this.initTransferSettings(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.TaxonomicGroup, this.panelTaxonomicGroups);
            if (this.panelTaxonomicGroups.Controls.Count > 0)
            {
                this.checkBoxRestrictTaxonomicGroups.Checked = true;
                if (!this.tabControlProject.TabPages.Contains(this.tabPageTaxonomicGroups))
                    this.tabControlProject.TabPages.Add(this.tabPageTaxonomicGroups);
            }
            else if (this.panelTaxonomicGroups.Controls.Count == 0)
            {
                this.checkBoxRestrictTaxonomicGroups.Checked = false;
                if (this.tabControlProject.TabPages.Contains(this.tabPageTaxonomicGroups))
                    this.tabControlProject.TabPages.Remove(this.tabPageTaxonomicGroups);
            }
        }

        private void checkBoxRestrictTaxonomicGroups_Click(object sender, EventArgs e)
        {
            this.SetProjectRestriction(this.checkBoxRestrictTaxonomicGroups, "taxonomic groups", DiversityCollection.CacheDatabase.CacheDB.TransferSetting.TaxonomicGroup);
        }

        private void buttonRequeryTaxonomicGroups_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            if (!RV.Row["ProjectID"].Equals(System.DBNull.Value))
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                DiversityCollection.CacheDatabase.CacheDB.RefereshProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.TaxonomicGroup);
            }
        }
        
        #endregion

        #region MaterialCategory
        
        private void initMaterialCategory(int ProjectID)
        {
            this.initTransferSettings(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.MaterialCategory, this.panelMaterialCategories);
            if (this.panelMaterialCategories.Controls.Count > 0)
            {
                this.checkBoxRestrictMaterialCategories.Checked = true;
                if (!this.tabControlProject.TabPages.Contains(this.tabPageMaterialCategories))
                    this.tabControlProject.TabPages.Add(this.tabPageMaterialCategories);
            }
            else if (this.panelMaterialCategories.Controls.Count == 0)
            {
                this.checkBoxRestrictMaterialCategories.Checked = false;
                if (this.tabControlProject.TabPages.Contains(this.tabPageMaterialCategories))
                    this.tabControlProject.TabPages.Remove(this.tabPageMaterialCategories);
            }
        }

        private void checkBoxRestrictMaterialCategories_Click(object sender, EventArgs e)
        {
            this.SetProjectRestriction(this.checkBoxRestrictMaterialCategories, "material categories", DiversityCollection.CacheDatabase.CacheDB.TransferSetting.MaterialCategory);
        }
        
        private void buttonRequeryMaterialCategories_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            if (!RV.Row["ProjectID"].Equals(System.DBNull.Value))
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                DiversityCollection.CacheDatabase.CacheDB.RefereshProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.MaterialCategory);
            }
        }

        #endregion

        #region Localisations
        
        private void initLocalisations(int ProjectID)
        {
            this.initTransferSettings(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.Localisation, this.panelLocalisations);
            if (this.panelLocalisations.Controls.Count > 0)
            {
                this.checkBoxRestrictLocalisations.Checked = true;
                if (!this.tabControlProject.TabPages.Contains(this.tabPageLocalisations))
                    this.tabControlProject.TabPages.Add(this.tabPageLocalisations);
            }
            else if (this.panelLocalisations.Controls.Count == 0)
            {
                this.checkBoxRestrictLocalisations.Checked = false;
                if (this.tabControlProject.TabPages.Contains(this.tabPageLocalisations))
                    this.tabControlProject.TabPages.Remove(this.tabPageLocalisations);
            }
        }

        private void checkBoxRestrictLocalisations_Click(object sender, EventArgs e)
        {
            this.SetProjectRestriction(this.checkBoxRestrictLocalisations, "localisation systems", DiversityCollection.CacheDatabase.CacheDB.TransferSetting.Localisation);
        }
        
        private void buttonRequeryLocalisations_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            if (!RV.Row["ProjectID"].Equals(System.DBNull.Value))
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                DiversityCollection.CacheDatabase.CacheDB.RefereshProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.Localisation);
            }
        }

        #endregion

        #region Coordinate precision

        private void checkBoxCoordinatePrecision_Click(object sender, EventArgs e)
        {
            if (this.projectProxyBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectProxyBindingSource.Current;
                if (this.checkBoxCoordinatePrecision.Checked)
                {
                    this.numericUpDownCoordinatePrecision.Visible = true;
                    this.numericUpDownCoordinatePrecision.Value = 0;
                    R["CoordinatePrecision"] = 0;
                }
                else
                {
                    this.numericUpDownCoordinatePrecision.Visible = false;
                    R["CoordinatePrecision"] = System.DBNull.Value;
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("No project selected");
        }

        private void numericUpDownCoordinatePrecision_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.projectProxyBindingSource.Current;
            R["CoordinatePrecision"] = this.numericUpDownCoordinatePrecision.Value;
        }
        
        #endregion

        #endregion

        #region Images

        private void initImages(int ProjectID)
        {
            this.initTransferSettings(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesSpecimen, this.panelImagesSpecimen);
            this.initTransferSettings(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesEvent, this.panelImagesEvent);
            this.initTransferSettings(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesSeries, this.panelImagesSeries);
            if (this.panelImagesSpecimen.Controls.Count > 0 || this.panelImagesEvent.Controls.Count > 0 || this.panelImagesSeries.Controls.Count > 0)
            {
                this.checkBoxRestrictImages.Checked = true;
                if (!this.tabControlProject.TabPages.Contains(this.tabPageImages))
                    this.tabControlProject.TabPages.Add(this.tabPageImages);
            }
            else
            {
                this.checkBoxRestrictImages.Checked = false;
                if (this.tabControlProject.TabPages.Contains(this.tabPageImages))
                    this.tabControlProject.TabPages.Remove(this.tabPageImages);
            }
        }

        private void checkBoxRestrictImages_Click(object sender, EventArgs e)
        {
            bool TryToFindImages = false;
            if (this.checkBoxRestrictImages.Checked) TryToFindImages = true;
            this.SetProjectRestriction(this.checkBoxRestrictImages, "images", DiversityCollection.CacheDatabase.CacheDB.TransferSetting.Images);
            if (!this.checkBoxRestrictImages.Checked && TryToFindImages)
                System.Windows.Forms.MessageBox.Show("This project contains no images");
        }
        
        private void buttonRequeryImagesSpecimen_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            if (!RV.Row["ProjectID"].Equals(System.DBNull.Value))
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                DiversityCollection.CacheDatabase.CacheDB.RefereshProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesSpecimen);
            }
        }

        private void buttonRequeryImagesEvent_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            if (!RV.Row["ProjectID"].Equals(System.DBNull.Value))
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                DiversityCollection.CacheDatabase.CacheDB.RefereshProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesEvent);
            }
        }

        private void buttonRequeryImagesSeries_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            if (!RV.Row["ProjectID"].Equals(System.DBNull.Value))
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                DiversityCollection.CacheDatabase.CacheDB.RefereshProjectTransferSetting(ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting.ImagesSeries);
            }
        }

        #endregion

        #region Auxillary

        private System.Data.DataTable ServerList(string Module, string Restriction)
        {
            System.Data.DataTable dt = new DataTable();
            if (this.ConnectionStringMaster.Length > 0)
            {
                string SQL = "SELECT name as DatabaseName FROM sys.databases where name not in ( 'master', 'model', 'tempdb', 'msdb')";
                if (Restriction.Length > 0)
                    SQL += " AND name LIKE '" + Restriction + "'";
                SQL += " ORDER BY name";
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringMaster);
                try
                {
                    ad.Fill(dt);
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    return dt;
                }
            }
            if (dt.Rows.Count > 0)
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.ConnectionStringMaster);
                con.Open();
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand("", con);
                if (Module.Length > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string SqlModule = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND " +
                            "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                            "BEGIN SELECT dbo.DiversityWorkbenchModule() END " +
                            "ELSE BEGIN SELECT NULL END";
                        try
                        {
                            C.CommandText = SqlModule;
                            if (Module != C.ExecuteScalar().ToString())
                            {
                                R.Delete();
                            }
                        }
                        catch { R.Delete(); }
                    }
                }
                con.Close();
            }
            return dt;
        }

        private System.Data.DataTable DatabaseList(string Module, string Restriction)
        {
            System.Data.DataTable dt = new DataTable();
            if (this.ConnectionStringMaster.Length > 0)
            {
                string SQL = "SELECT name as DatabaseName FROM sys.databases where name not in ( 'master', 'model', 'tempdb', 'msdb')";
                if (Restriction.Length > 0)
                    SQL += " AND name LIKE '" + Restriction + "'";
                SQL += " ORDER BY name";
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringMaster);
                try
                {
                    ad.Fill(dt);
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    return dt;
                }
            }
            if (dt.Rows.Count > 0)
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.ConnectionStringMaster);
                con.Open();
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand("", con);
                if (Module.Length > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string SqlModule = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND " +
                            "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                            "BEGIN SELECT dbo.DiversityWorkbenchModule() END " +
                            "ELSE BEGIN SELECT NULL END";
                        try
                        {
                            C.CommandText = SqlModule;
                            if (Module != C.ExecuteScalar().ToString())
                            {
                                R.Delete();
                            }
                        }
                        catch { R.Delete(); }
                    }
                }
                con.Close();
            }
            return dt;
        }

        private string ConnectionStringMaster
        {
            get
            {
                string conStr = "";
                if (DiversityWorkbench.Settings.DatabaseServer.Length > 0)
                {
                    conStr = "Data Source=" + DiversityWorkbench.Settings.DatabaseServer;
                    conStr += "," + DiversityWorkbench.Settings.DatabasePort;
                    conStr += ";initial catalog=master;";
                    if (DiversityWorkbench.Settings.IsTrustedConnection)
                        conStr += "Integrated Security=True";
                    else
                    {
                        if (DiversityWorkbench.Settings.DatabaseUser.Length > 0 && DiversityWorkbench.Settings.Password.Length > 0)
                            conStr += "user id=" + DiversityWorkbench.Settings.DatabaseUser + ";password=" + DiversityWorkbench.Settings.Password;
                        else conStr = "";
                    }
                }
                return conStr;
            }
        }

        #endregion

        #region Feedback

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        #endregion

    }
}
