using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace DiversityWorkbench.Forms
{
    public partial class FormUpdateDatabase : Form
    {
        #region Parameter

        private Microsoft.Data.SqlClient.SqlConnection _Connection;
        private System.Collections.Generic.Dictionary<string, string> _Versions;
        private System.IO.FileInfo _Protocol;
        private string _Script;
        private string _UpdatesPath;
        private string _Database;
        private string _VersionFinal;
        private string _VersionDisplay;
        private string _VersionCurrent;
        private bool _Reconnect = true;
        private bool _SilentMode = false;
        private bool _SilentUpdateSuccess = false;
        private bool _IsBusy = false;
        private bool _Abort = false;

        /// <summary>
        /// when not the default schema dbo should be used, but a specified schema
        /// the version in these schemata is an integer
        /// </summary>
        private string _Schema = "";

        // Postgres
        private Npgsql.NpgsqlConnection _PostgresConnection;
        private bool _ForPostgres = false;
        private System.Collections.Generic.Dictionary<string, string> _ReplaceStrings;
        private string _PostgresSchema = "";
        private string _PostgresPackage = "";
        private string _PostgresRole = "";

        #endregion

        #region Construction

        public FormUpdateDatabase(string PathHelpProvider, string Database = "")
        {
            InitializeComponent();
            _Database = Database;
            this.helpProvider.HelpNamespace = PathHelpProvider;
            this.splitContainerOutput.Panel1Collapsed = true;
            this.splitContainerOutput.Panel2Collapsed = false;
            _UpdatesPath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Updates) + (_Database == null || _Database == "" ? "" : _Database);
            _UpdatesPath = _UpdatesPath.TrimEnd('\\');

            System.IO.DirectoryInfo path = new System.IO.DirectoryInfo(_UpdatesPath);
            if (!path.Exists)
            {
                path.Create();
            }

            // Init for expert mode...
            checkBoxExpert_CheckedChanged(null, null);

            // Markus 17.11.2017 - needed for certain clean ups
            // this.buttonSetTimeout.Visible = false; 
            this.setTimeOutButtonText();

        }

        public FormUpdateDatabase(string Database, string Version, string ConnectionString, System.Collections.Generic.Dictionary<string, string> Versions, string PathHelpProvider)
            : this(PathHelpProvider, Database)
        {
            _VersionFinal = Version;
            _VersionDisplay = Version;
            this.setHeaderText();
            _VersionFinal = Version.Replace(".", "");
            _Versions = Versions;

            if (ConnectionString.Length > 0)
            {
                _Connection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                //string SqlUser = "select user_name()";
                this.checkBoxExpert.Visible = true;// ("dbo" == this.SqlExecuteScalar(SqlUser));

                // build work list on result panel
                buildWorkList();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Connection to database failed");
                this.tableLayoutPanelMain.Enabled = false;
            }
            // Markus 17.11.2017 - needed for certain clean ups
            // this.buttonSetTimeout.Visible = false;
            this.setTimeOutButtonText();

        }

        public FormUpdateDatabase(string Database,
            string Version,
            string ConnectionString,
            System.Collections.Generic.Dictionary<string, string> Versions,
            string PathHelpProvider,
            string Schema,
            System.Collections.Generic.Dictionary<string, string> ReplaceStrings)
            : this(PathHelpProvider, Database)
        {
            _VersionFinal = Version;
            _VersionDisplay = Version;
            this.setHeaderText();
            _VersionFinal = Version.Replace(".", "");
            _Versions = Versions;

            if (Schema != null && Schema != "")
            {
                _UpdatesPath += (_UpdatesPath.EndsWith("\\") ? "" : "\\") + Schema;
            }

            if (ConnectionString.Length > 0)
            {
                _Connection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);

                // Init for expert mode...
                checkBoxExpert_CheckedChanged(null, null);
                this.checkBoxExpert.Visible = true;

                this._ReplaceStrings = ReplaceStrings;
                this._Schema = Schema;

                this.setHeaderText();

                // build work list on result panel
                buildWorkList();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Connection to database failed");
                this.tableLayoutPanelMain.Enabled = false;
            }
            this.buttonSetTimeout.Visible = false;
        }

        /// <summary>
        /// For schemas in Cache database
        /// </summary>
        /// <param name="Database">Name of the database</param>
        /// <param name="Version">Version of the Schema</param>
        /// <param name="Versions">The update scripts</param>
        /// <param name="PathHelpProvider">The help provider</param>
        /// <param name="Schema">The schema in which the package should be updated</param>
        /// <param name="ReplaceStrings">The strings that must be replaced in the scripts</param>
        public FormUpdateDatabase(
            string Database,
            string ConnectionString,
            int Version,
            System.Collections.Generic.Dictionary<string, string> Versions,
            string PathHelpProvider,
            string Schema,
            System.Collections.Generic.Dictionary<string, string> ReplaceStrings)
        {
            InitializeComponent();

            this._Connection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            this.helpProvider.HelpNamespace = PathHelpProvider;
            this.splitContainerOutput.Panel1Collapsed = true;
            this.splitContainerOutput.Panel2Collapsed = false;
            _UpdatesPath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Updates) + Database + "\\" + Schema; // ...Windows.Forms.Application.StartupPath + "\\Updates";

            System.IO.DirectoryInfo path = new System.IO.DirectoryInfo(_UpdatesPath);
            if (!path.Exists)
            {
                path.Create();
            }

            // Init for expert mode...
            checkBoxExpert_CheckedChanged(null, null);

            this._ReplaceStrings = ReplaceStrings;
            this._Schema = Schema;
            _Database = Database;
            _VersionFinal = Version.ToString();
            _VersionDisplay = Version.ToString();
            ForPostgres = false;
            this.setHeaderText();
            _VersionFinal = "000000".Substring(Version.ToString().Length) + Version.ToString();
            _Versions = Versions;

            buildWorkList();
            this.checkBoxExpert.Visible = true;// ("dbo" == this.SqlExecuteScalar(SqlUser));

            this.buttonSetTimeout.Visible = false;
        }

        #region Postgres

        public FormUpdateDatabase(string Database, string Version, Npgsql.NpgsqlConnection PostgresConnection, System.Collections.Generic.Dictionary<string, string> Versions, string PathHelpProvider)
            : this(PathHelpProvider, Database)
        {
            ForPostgres = true;
            _VersionFinal = Version;
            _VersionDisplay = Version;
            this.setHeaderText();
            _VersionFinal = Version.Replace(".", "");
            _Versions = Versions;

            if (PostgresConnection.ConnectionString.Length > 0)
            {
                _PostgresConnection = new Npgsql.NpgsqlConnection(PostgresConnection.ConnectionString);
                //string SqlUser = "select user_name()";
                this.checkBoxExpert.Visible = true;// ("dbo" == this.SqlExecuteScalar(SqlUser));

                // build work list on result panel
                buildWorkList();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Connection to database failed");
                this.tableLayoutPanelMain.Enabled = false;
            }
            this.setTimeOutButtonText();
        }

        public FormUpdateDatabase(string Database, string Version, Npgsql.NpgsqlConnection PostgresConnection, System.Collections.Generic.Dictionary<string, string> Versions, string PathHelpProvider, string Schema, System.Collections.Generic.Dictionary<string, string> ReplaceStrings)
        {
            InitializeComponent();

            this.helpProvider.HelpNamespace = PathHelpProvider;
            this.splitContainerOutput.Panel1Collapsed = true;
            this.splitContainerOutput.Panel2Collapsed = false;
            _UpdatesPath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Updates) + Database + "\\" + Schema;// ...Windows.Forms.Application.StartupPath + "\\Updates";

            System.IO.DirectoryInfo path = new System.IO.DirectoryInfo(_UpdatesPath);
            if (!path.Exists)
            {
                path.Create();
            }

            // Init for expert mode...
            checkBoxExpert_CheckedChanged(null, null);

            this._ReplaceStrings = ReplaceStrings;
            this._PostgresSchema = Schema;
            ForPostgres = true;
            _Database = Database;
            _VersionFinal = Version;
            _VersionDisplay = Version;
            this.labelHeader.Text = "Update the schema " + Schema + " to version " + _VersionFinal;
            this.setHeaderText();
            _VersionFinal = "000000".Substring(Version.Length) + Version;
            _Versions = Versions;

            if (PostgresConnection.ConnectionString.Length > 0)
            {
                _PostgresConnection = new Npgsql.NpgsqlConnection(PostgresConnection.ConnectionString);
                //string SqlUser = "select user_name()";
                this.checkBoxExpert.Visible = true;// ("dbo" == this.SqlExecuteScalar(SqlUser));

                // build work list on result panel
                buildWorkList();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Connection to database failed");
                this.tableLayoutPanelMain.Enabled = false;
            }
            this.setTimeOutButtonText();

        }

        /// <summary>
        /// For packages in postgres databases
        /// </summary>
        /// <param name="Database">Name of the database</param>
        /// <param name="Package">Name of the package</param>
        /// <param name="Version">Version of the package</param>
        /// <param name="PostgresConnection">The connection to the database</param>
        /// <param name="Versions">The update scripts</param>
        /// <param name="PathHelpProvider">The help provider</param>
        /// <param name="Schema">The schema in which the package should be updated</param>
        /// <param name="ReplaceStrings">The strings that must be replaced in the scripts</param>
        public FormUpdateDatabase(string Database, string Package, int Version, Npgsql.NpgsqlConnection PostgresConnection, System.Collections.Generic.Dictionary<string, string> Versions, string PathHelpProvider, string Schema, System.Collections.Generic.Dictionary<string, string> ReplaceStrings)
        {
            InitializeComponent();

            this.helpProvider.HelpNamespace = PathHelpProvider;
            this.splitContainerOutput.Panel1Collapsed = true;
            this.splitContainerOutput.Panel2Collapsed = false;
            _UpdatesPath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Updates) + Database + "\\" + Schema;// ...Windows.Forms.Application.StartupPath + "\\Updates";

            System.IO.DirectoryInfo path = new System.IO.DirectoryInfo(_UpdatesPath);
            if (!path.Exists)
            {
                path.Create();
            }

            // Init for expert mode...
            checkBoxExpert_CheckedChanged(null, null);

            this._ReplaceStrings = ReplaceStrings;
            this._PostgresSchema = Schema;
            this._PostgresPackage = Package;
            _Database = Database;
            _VersionFinal = Version.ToString();
            _VersionDisplay = Version.ToString();
            //this.labelHeader.Text = "Update the package " + Package + " in schema " + Schema + " to version " + _VersionFinal;
            ForPostgres = true;
            this.setHeaderText();
            _VersionFinal = "000000".Substring(Version.ToString().Length) + Version.ToString();
            _Versions = Versions;

            if (PostgresConnection.ConnectionString.Length > 0)
            {
                _PostgresConnection = new Npgsql.NpgsqlConnection(PostgresConnection.ConnectionString);
                //string SqlUser = "select user_name()";
                this.checkBoxExpert.Visible = true;// ("dbo" == this.SqlExecuteScalar(SqlUser));

                // build work list on result panel
                buildWorkList();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Connection to database failed");
                this.tableLayoutPanelMain.Enabled = false;
            }
            this.setTimeOutButtonText();
        }

        #endregion

        #endregion

        #region Public properties

        public bool Reconnect { get { return _Reconnect; } }

        /// <summary>
        /// MW 7.7.2015 - if the update should be performed on a postgres database
        /// </summary>
        public bool ForPostgres { get { return this._ForPostgres; } set { this._ForPostgres = value; } }

        public string PostgresRole { set { this._PostgresRole = value; } }

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

        public bool UpdateSilent()
        {
            if (splitContainerOutput.Panel2.Controls.Count > 0)
            {
                _SilentMode = true;
                buttonStartUpdate_Click(null, null);
                clearControls();
                this.buttonStartUpdate.Enabled = false;
            }
            return _SilentUpdateSuccess;
        }
        #endregion

        #region Private functions

        private void textBoxFile_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxFile.Text.Length > 0)
            {
                System.IO.FileInfo F = new System.IO.FileInfo(this.textBoxFile.Text);
                if (F.Exists)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(F.FullName);
                    using (sr)
                    {
                        this.textBoxScript.Text = sr.ReadToEnd();
                    }
                }
                else this.textBoxScript.Text = "";
            }
            else
                this.textBoxScript.Text = "";
        }

        private void buttonStartUpdate_Click(object sender, EventArgs e)
        {
            if (_IsBusy)
            {
                // In principle you could abort update process after each single update file by setting _Abort=true;
                // Currently no such exit point is set in the upgrade processing (else-branch)....
                // So this branch is to prevent re-entry in regular processing by subsequenc clicking the start button.
            }
            else
            {
                // Mark running update
                _IsBusy = true;
                _Abort = false;

                // mark running upgrade and set SQL File text box to browse mode
                bool localDB = false;
                if (!ForPostgres)
                    localDB = _Connection.DataSource.Trim().StartsWith("127.0.0.1");
                bool upgradeSuccess = true;
                DateTime backupTime = DateTime.Now;

                _VersionCurrent = this.CurrentVersionOfDatabase.Replace(".", "").Replace("/", "");
                if (_VersionCurrent == "000000")
                    localDB = false;
                string backupFile = string.Format("{0}_{1}_{2}{3}{4}.bak",
                    _Database, _VersionCurrent, backupTime.Year.ToString("0000"), backupTime.Month.ToString("00"), backupTime.Day.ToString("00"));

                // create database backup
                if (localDB && !_SilentMode)
                {
                    // insert local backup path
                    string backupPath = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Backup);// ...Windows.Forms.Application.StartupPath + "\\Backup";
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(backupPath);
                    if (!di.Exists) di.Create();
                    backupFile = string.Format("{0}\\{1}", backupPath, backupFile);

                    if (System.Windows.Forms.MessageBox.Show("Create " + (localDB ? "local " : "") + "database backup " + backupFile + "? (recommended)", "Database backup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string except = string.Empty;
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        upgradeSuccess = DiversityWorkbench.Database.BackupCopy(_Database, ref backupFile, out except);
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        if (!upgradeSuccess)
                        {
                            // local backup error
                            backupFile = string.Empty;
                            if (System.Windows.Forms.MessageBox.Show(except + "\r\nStop upgrade?", "Database backup error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                            {
                                _IsBusy = false;
                                return;
                            }
                            upgradeSuccess = true;
                        }
                    }
                    else
                    {
                        // Neutralize backup file name
                        backupFile = string.Empty;
                    }
                }
                else
                {
                    // Neutralize backup file name
                    backupFile = string.Empty;
                }

                try
                {
                    string resultFile;
                    string SQL;
                    bool verPresent;

                    //for (int i = 0; i < workList.Count; i++)
                    for (int i = this.splitContainerOutput.Panel2.Controls.Count - 1; i >= 0; i--)
                    {
                        DiversityWorkbench.UserControls.UserControlUpdateResult currentControl = (this.splitContainerOutput.Panel2.Controls[i] as DiversityWorkbench.UserControls.UserControlUpdateResult);
                        _Script = currentControl.ScriptTitle;

                        // check database version
                        SQL = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Version]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))" +
                              "BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
                        if (ForPostgres)
                        {
                            if (this._PostgresSchema.Length == 0)
                            {
                                SQL = "select count(*) from information_Schema.routines where routine_name = 'version' and routine_schema = 'public'";
                                if (verPresent = ("1" == DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL)))// .Postgres.PostgresExecuteSqlSkalar(SQL)))
                                {
                                    string DatabaseVersion = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT \"public\".VERSION()");// .Postgres.PostgresExecuteSqlSkalar("SELECT public.VERSION()");
                                    if (DatabaseVersion.Replace("/", "").Replace(".", "") != VersionFrom(_Script))
                                    {
                                        if (!_SilentMode)
                                            System.Windows.Forms.MessageBox.Show("Database version " + DatabaseVersion + " does not fit to script " + _Script + "!\r\nUpgrade failed", "Upgrade error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        upgradeSuccess = false;
                                        _Reconnect = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (this._PostgresPackage.Length == 0)
                                {
                                    SQL = "select count(*) from information_Schema.routines where routine_name = 'version' and routine_schema = '" + this._PostgresSchema + "'";
                                    if (verPresent = ("1" == DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL)))// .Postgres.PostgresExecuteSqlSkalar(SQL)))
                                    {
                                        SQL = "SELECT \"" + this._PostgresSchema + "\".VERSION()";
                                        string SchemaVersion = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar("SELECT \"" + this._PostgresSchema + "\".VERSION()");
                                        int iSchemaVersion = int.Parse(SchemaVersion);
                                        int iSkriptVersion = int.Parse(VersionFrom(_Script));
                                        if (iSchemaVersion != iSkriptVersion) //SchemaVersion.Replace("/", "").Replace(".", "") != VersionFrom(_Script))
                                        {
                                            if (!_SilentMode)
                                                System.Windows.Forms.MessageBox.Show("Schema version " + SchemaVersion + " does not fit to script " + _Script + "!\r\nUpgrade failed", "Upgrade error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            upgradeSuccess = false;
                                            _Reconnect = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (this._PostgresPackage.IndexOf("_") == -1)
                                    {
                                        SQL = "select count(*) from information_Schema.tables where Table_name = 'Package' and table_schema = '" + this._PostgresSchema + "'";
                                        if (verPresent = ("1" == DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL)))// .Postgres.PostgresExecuteSqlSkalar(SQL)))
                                        {
                                            string PackageVersion = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT \"Version\" FROM \"" + this._PostgresSchema + "\".\"Package\" WHERE \"Package\" = '" + this._PostgresPackage + "';");// .Postgres.PostgresExecuteSqlSkalar("SELECT \"" + this._PostgresSchema + "\".VERSION()");
                                            int iSchemaVersion = int.Parse(PackageVersion);
                                            int iSkriptVersion = int.Parse(VersionFrom(_Script));
                                            if (iSchemaVersion != iSkriptVersion) //SchemaVersion.Replace("/", "").Replace(".", "") != VersionFrom(_Script))
                                            {
                                                if (!_SilentMode)
                                                    System.Windows.Forms.MessageBox.Show("Package version " + PackageVersion + " does not fit to script " + _Script + "!\r\nUpgrade failed", "Upgrade error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                upgradeSuccess = false;
                                                _Reconnect = false;
                                                break;
                                            }
                                        }
                                    }
                                    else // Add on
                                    {
                                        SQL = "select count(*) from information_Schema.tables where Table_name = 'PackageAddOn' and table_schema = '" + this._PostgresSchema + "'";
                                        if (verPresent = ("1" == DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL)))// .Postgres.PostgresExecuteSqlSkalar(SQL)))
                                        {
                                            string PackageAddOnVersion = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT \"Version\" FROM \"" + this._PostgresSchema + "\".\"PackageAddOn\" WHERE \"Package\" = '" + this._PostgresPackage.Substring(0, this._PostgresPackage.IndexOf("_")) + "' AND \"AddOn\" = '" + this._PostgresPackage + "';");// .Postgres.PostgresExecuteSqlSkalar("SELECT \"" + this._PostgresSchema + "\".VERSION()");
                                            int iSchemaVersion = int.Parse(PackageAddOnVersion);
                                            int iSkriptVersion = int.Parse(VersionFrom(_Script));
                                            if (iSchemaVersion != iSkriptVersion) //SchemaVersion.Replace("/", "").Replace(".", "") != VersionFrom(_Script))
                                            {
                                                if (!_SilentMode)
                                                    System.Windows.Forms.MessageBox.Show("Package version " + PackageAddOnVersion + " does not fit to script " + _Script + "!\r\nUpgrade failed", "Upgrade error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                upgradeSuccess = false;
                                                _Reconnect = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (this._Schema != null && this._Schema.Length > 0)
                            {
                                SQL = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + this._Schema + "].[Version]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))" +
                                      "BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
                            }
                            if (verPresent = ("1" == this.SqlExecuteScalar(SQL)))
                            {
                                if (this._Schema != null && this._Schema.Length > 0)
                                {
                                    string SchemaVersion = this.SqlExecuteScalar("SELECT [" + this._Schema + "].VERSION()");
                                    int iSchemaVersion = int.Parse(SchemaVersion);
                                    int iSkriptVersion = int.Parse(VersionFrom(_Script));
                                    if (iSchemaVersion != iSkriptVersion) //SchemaVersion.Replace("/", "").Replace(".", "") != VersionFrom(_Script))
                                    {
                                        if (!_SilentMode)
                                            System.Windows.Forms.MessageBox.Show("Project version " + SchemaVersion + " does not fit to script " + _Script + "!\r\nUpgrade failed", "Upgrade error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        upgradeSuccess = false;
                                        _Reconnect = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    string DatabaseVersion = this.SqlExecuteScalar("SELECT dbo.VERSION()");
                                    if (DatabaseVersion.Replace("/", "").Replace(".", "") != VersionFrom(_Script))
                                    {
                                        if (!_SilentMode)
                                            System.Windows.Forms.MessageBox.Show("Database version " + DatabaseVersion + " does not fit to script " + _Script + "!\r\nUpgrade failed", "Upgrade error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        upgradeSuccess = false;
                                        _Reconnect = false;
                                        break;
                                    }
                                }
                            }
                        }

                        // mark current script as running
                        currentControl.SetRunning();
                        this.splitContainerOutput.Panel2.Update();
                        Application.DoEvents();

                        // perform upgrade
                        if (currentControl.Skip)
                        {
                            upgradeSuccess = true;
                            resultFile = "";
                        }
                        else
                            upgradeSuccess = this.UpdateDatabase(out resultFile);

                        // set database version
                        if (upgradeSuccess)
                        {
                            if (ForPostgres)
                            {
                                if (this._PostgresSchema.Length == 0)
                                {
                                    SQL = string.Format("CREATE OR REPLACE FUNCTION version() RETURNS text AS $BODY$ declare v text; BEGIN SELECT '{0}' into v; RETURN v; END; $BODY$ LANGUAGE plpgsql STABLE COST 100;", VersionTo(_Script).Insert(2, ".").Insert(5, "."));
                                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))
                                    {
                                        SQL = "";
                                        if (this._PostgresRole.Length > 0)
                                            SQL = "SET ROLE \"" + this._PostgresRole + "\"; ";
                                        SQL += string.Format("CREATE OR REPLACE FUNCTION version() RETURNS text AS $BODY$ declare v text; BEGIN SELECT '{0}' into v; RETURN v; END; $BODY$ LANGUAGE plpgsql STABLE COST 100;", VersionTo(_Script).Insert(2, ".").Insert(5, "."));
                                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))// Postgres .PostgresExecuteSqlNonQuery(SQL))
                                        {
                                            if (!_SilentMode)
                                                System.Windows.Forms.MessageBox.Show("Database version " + VersionTo(_Script).Insert(2, ".").Insert(5, ".") + " could not be set", "Version problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                                else if (this._PostgresPackage.Length == 0)
                                {
                                    int PgSchemaVersion;
                                    if (int.TryParse(_Script.Substring(_Script.LastIndexOf("_") + 1), out PgSchemaVersion))
                                    {
                                        SQL = string.Format("CREATE OR REPLACE FUNCTION \"" + _PostgresSchema + "\".version() RETURNS integer AS $BODY$ declare v integer; BEGIN SELECT {0} into v; RETURN v; END; $BODY$ LANGUAGE plpgsql STABLE COST 100;", PgSchemaVersion.ToString());
                                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))// Postgres .PostgresExecuteSqlNonQuery(SQL))
                                        {
                                            if (!_SilentMode)
                                                System.Windows.Forms.MessageBox.Show("Schema version " + PgSchemaVersion.ToString() + " could not be set", "Version problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                                // Kollidiert mit Update der Packages - wird direkt in Skript geloest
                                //if (this._PostgresSchema.Length > 0)
                                //    SQL = string.Format("CREATE OR REPLACE FUNCTION \"" + _PostgresSchema + "\".version() RETURNS integer AS $BODY$ declare v integer; BEGIN SELECT {0} into v; RETURN v; END; $BODY$ LANGUAGE plpgsql STABLE COST 100;", int.Parse(VersionTo(_Script)).ToString());
                                //else
                                //    SQL = string.Format("CREATE OR REPLACE FUNCTION version() RETURNS text AS $BODY$ declare v text; BEGIN SELECT '{0}' into v; RETURN v; END; $BODY$ LANGUAGE plpgsql STABLE COST 100;", VersionTo(_Script).Insert(2, ".").Insert(5, "."));
                                //if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))// Postgres .PostgresExecuteSqlNonQuery(SQL))
                                //{
                                //    if (this._PostgresSchema.Length > 0)
                                //        System.Windows.Forms.MessageBox.Show("Schema version " + int.Parse(VersionTo(_Script)).ToString() + " could not be set", "Version problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    else
                                //        System.Windows.Forms.MessageBox.Show("Database version " + VersionTo(_Script).Insert(2, ".").Insert(5, ".") + " could not be set", "Version problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //}
                            }
                            else
                            {
                                if (this._Schema.Length > 0)
                                {
                                    if (verPresent)
                                    {
                                        SQL = string.Format("ALTER FUNCTION [" + this._Schema + "].[Version] () RETURNS int AS BEGIN RETURN {0} END", int.Parse(VersionTo(_Script)).ToString());
                                    }
                                    else
                                    {
                                        SQL = string.Format("CREATE FUNCTION [dbo].[" + this._Schema + "].[Version] () RETURNS int AS BEGIN RETURN {0} END", int.Parse(VersionTo(_Script)).ToString());
                                    }
                                    if (!this.SqlExecuteNonQuery(SQL))
                                    {
                                        if (!_SilentMode)
                                            System.Windows.Forms.MessageBox.Show("Schema version" + VersionTo(_Script).Insert(2, ".").Insert(5, ".") + " could not be set", "Version problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    if (verPresent)
                                    {
                                        SQL = string.Format("ALTER FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '{0}' END", VersionTo(_Script).Insert(2, ".").Insert(5, "."));
                                    }
                                    else
                                    {
                                        SQL = string.Format("CREATE FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '{0}' END", VersionTo(_Script).Insert(2, ".").Insert(5, "."));
                                    }
                                    if (!this.SqlExecuteNonQuery(SQL))
                                    {
                                        if (!_SilentMode)
                                            System.Windows.Forms.MessageBox.Show("Database version" + VersionTo(_Script).Insert(2, ".").Insert(5, ".") + " could not be set", "Version problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }

                        }

                        // supply missing components in result panel
                        currentControl.SetResultFile(resultFile);
                        currentControl.SetResult(upgradeSuccess);
                        this.splitContainerOutput.Panel2.Update();
                        Application.DoEvents();

                        if (!upgradeSuccess)
                        {
                            if (this._ForPostgres && this._PostgresSchema.Length > 0)
                            {
                                if (!_SilentMode)
                                    System.Windows.Forms.MessageBox.Show("Update of schema " + _PostgresSchema + " to version " + _VersionFinal + " failed");
                            }
                            else if (this._Schema != null && this._Schema.Length > 0)
                            {
                                if (!_SilentMode)
                                    System.Windows.Forms.MessageBox.Show("Update of schema " + _Schema + " to version " + _VersionFinal + " failed");
                            }
                            else
                            {
                                if (!_SilentMode)
                                    System.Windows.Forms.MessageBox.Show("Update of database " + _Database + " to version " + _VersionFinal + " failed");
                            }
                            _Reconnect = false;
                            break;
                        }
                    }
                    if (upgradeSuccess)
                    {
                        string Message = "Database " + _Database + " updated to version ";
                        string DBversion = this.CurrentVersionOfDatabase;
                        if (this._ForPostgres && this._PostgresSchema.Length > 0)
                        {
                            Message = "Schema " + _PostgresSchema + " updated to version ";
                        }
                        if (_ForPostgres && _PostgresSchema.Length > 0)
                        {
                            if (DBversion != _VersionFinal)
                                Message += " " + DBversion + ". Scripts for update to version " + _VersionDisplay + " missing or invalid";
                            else
                                Message += " " + DBversion;
                        }
                        else if (this._Schema != null && this._Schema.Length > 0)
                        {
                            Message = "Schema " + _Schema + " updated to version ";
                            if (DBversion.Replace(".", "").Replace("/", "") != _VersionFinal)
                                Message += " " + DBversion + ". Scripts for update to version " + _VersionDisplay + " missing or invalid";
                            else
                                Message += " " + _VersionDisplay;
                        }
                        else
                        {
                            if (DBversion.Replace(".", "").Replace("/", "") != _VersionFinal)
                                Message += " " + DBversion + ". Scripts for update to version " + _VersionDisplay + " missing or invalid";
                            else
                                Message += " " + _VersionDisplay;
                        }
                        if (!_SilentMode)
                            System.Windows.Forms.MessageBox.Show(Message);
                        else
                            _SilentUpdateSuccess = upgradeSuccess;
                        this.labelHeader.Text = Message;
                        this.buttonStartUpdate.Enabled = false;
                    }
                }
                catch (System.Exception ex)
                {
                    upgradeSuccess = false;
                    _Reconnect = false;
                    string Message = "Update of database " + _Database + " to version " + _VersionFinal + " failed";
                    if (this._ForPostgres && this._PostgresSchema.Length > 0)
                        Message = "Update of schema " + _PostgresSchema + " to version " + _VersionFinal + " failed";
                    if (!_SilentMode)
                        System.Windows.Forms.MessageBox.Show(Message);
                    else
                        _SilentUpdateSuccess = upgradeSuccess;
                }

                string SqlUser = "select user_name()";
                if (!upgradeSuccess && (backupFile != string.Empty) && ("dbo" == this.SqlExecuteScalar(SqlUser)))
                {
                    if (!_SilentMode)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Restore " + (localDB ? "local " : "") + "database from backup " + backupFile + "? (recommended)", "Database backup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // restore database from backup
                            string except = string.Empty;
                            if (DiversityWorkbench.Database.RestoreCopy(_Database, backupFile, out except))
                                System.Windows.Forms.MessageBox.Show("Database successfully restored from " + backupFile, "Restore success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                System.Windows.Forms.MessageBox.Show(except + "\r\nYou may try to restore the database backup from file " + backupFile + " ", "Restore failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                // Mark no more task is runing
                _IsBusy = false;
            }
        }

        private bool UpdateDatabase(out string resultFile)
        {
            resultFile = string.Empty;
            bool OK = true;

            if (_Connection != null || (ForPostgres && _PostgresConnection != null))
            {
                string xmlFile = _UpdatesPath + "\\" + _Script + ".xml";
                _Protocol = new System.IO.FileInfo(xmlFile);

                string ConnectionString;
                if (!ForPostgres)
                {
                    ConnectionString = _Connection.ConnectionString;
                }
                else
                    ConnectionString = _PostgresConnection.ConnectionString;
                ScriptProcessor sp = new ScriptProcessor(ConnectionString);
                if (ForPostgres)
                    sp.ForPostgres = true;
                sp.Open(_Versions[_Script], _Script, _Protocol);
                textBoxFile.Text = _Script;
                textBoxFile.Update();
                textBoxScript.Text = _Versions[_Script];
                textBoxScript.Update();
                Application.DoEvents();

                string Bugreport = "";
                string SQL = "";
                int procLen;

                using (sp)
                {
                    if (this.checkBoxExpert.Checked && !_SilentMode) // trace processing
                    {
                        while ((SQL = sp.NextStep(out procLen)) != string.Empty)
                        {
                            //Locate script text
                            this.textBoxScript.SelectionStart = textBoxScript.SelectionStart + textBoxScript.SelectionLength;
                            this.textBoxScript.SelectionLength = procLen;
                            this.textBoxScript.ScrollToCaret();
                            Application.DoEvents();

                            if (this.checkBoxSingle.Checked)
                            {
                                if (ForPostgres)
                                {
                                    if (SQL.StartsWith("--"))
                                    {
                                        // only comment
                                    }
                                    else
                                    {
                                        string Message = SQL;
                                        if (this._ReplaceStrings != null && this._ReplaceStrings.Count > 0)
                                        {
                                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ReplaceStrings)
                                                Message = Message.Replace(KV.Key, KV.Value);
                                        }
                                        System.Windows.Forms.DialogResult dr = MessageBox.Show(Message, "Execute next step?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                                        if (dr != System.Windows.Forms.DialogResult.Yes)
                                        {
                                            if (dr == System.Windows.Forms.DialogResult.No)
                                                continue;
                                            else if (MessageBox.Show("Switch off single step mode and continue processing?", "Switch off single step?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                                            {
                                                this.checkBoxSingle.Checked = false;
                                            }
                                            else
                                            {
                                                // do not accept errors to provide correct script results
                                                sp.AcceptedError = 0;
                                                OK = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    System.Windows.Forms.DialogResult dr = MessageBox.Show(SQL, "Execute next step?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                                    if (dr != System.Windows.Forms.DialogResult.Yes)
                                    {
                                        if (dr == System.Windows.Forms.DialogResult.No)
                                            continue;
                                        else if (MessageBox.Show("Switch off single step mode and continue processing?", "Switch off single step?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                                        {
                                            this.checkBoxSingle.Checked = false;
                                        }
                                        else
                                        {
                                            // do not accept errors to provide correct script results
                                            sp.AcceptedError = 0;
                                            OK = false;
                                            break;
                                        }

                                    }
                                }
                            }
                            if (ForPostgres)
                            {
                                if (this._PostgresSchema.Length > 0)
                                {
                                    if (SQL.StartsWith("CREATE OR REPLACE FUNCTION " + this._PostgresSchema + ".version()") &&
                                        Bugreport.Length > 0)
                                    {
                                        if (System.Windows.Forms.MessageBox.Show("Should version of schema be updated inspite of errors while executing the script?", "Update version?", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            // do not accept errors to provide correct script results
                                            sp.AcceptedError = 0;
                                            OK = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (SQL.StartsWith("CREATE OR REPLACE FUNCTION version()") &&
                                        Bugreport.Length > 0)
                                    {
                                        if (System.Windows.Forms.MessageBox.Show("Should version of database be updated inspite of errors while executing the script?", "Update version?", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            // do not accept errors to provide correct script results
                                            sp.AcceptedError = 0;
                                            OK = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (SQL.StartsWith("ALTER FUNCTION [dbo].[Version] ()") &&
                                    Bugreport.Length > 0)
                                {
                                    if (System.Windows.Forms.MessageBox.Show("Should version of database be updated inspite of errors while executing the script?", "Update version?", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        // do not accept errors to provide correct script results
                                        sp.AcceptedError = 0;
                                        OK = false;
                                        break;
                                    }
                                }
                            }

                            string Exception = "";
                            int err = 0;
                            if (ForPostgres)
                            {
                                if (this._ReplaceStrings != null && this._ReplaceStrings.Count > 0)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ReplaceStrings)
                                        SQL = SQL.Replace(KV.Key, KV.Value);
                                }
                                // new trial with version 5.0.3
                                err = 0;
                                Exception = "";
                                if (false)
                                {
                                    if (false)
                                    {
                                        System.Collections.Generic.List<string> ReplaceList = new List<string>();
                                        ReplaceList.Add("\t");
                                        ReplaceList.Add("\r\n");
                                        foreach (string s in ReplaceList)
                                        {
                                            SQL = SQL.Replace(s, " ");
                                        }

                                    }
                                    string[] SqlCommands = SQL.Split(';');
                                    foreach (string s in SqlCommands)
                                    {
                                        if (s.Trim().Length > 0)
                                            err = sp.ExecStepPostgres(s, out Exception);
                                        if (Exception.Length > 0)
                                        { }
                                    }

                                }
                                else
                                {
                                    err = sp.ExecStepPostgres(SQL, out Exception);
                                    if (Exception.Length > 0)
                                    { }
                                }
                            }
                            else
                            {
                                if (this._Schema != null && this._Schema.Length > 0 && this._ReplaceStrings != null && this._ReplaceStrings.Count > 0)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ReplaceStrings)
                                        SQL = SQL.Replace(KV.Key, KV.Value);
                                }
                                err = sp.ExecStep(SQL, out Exception);
                            }

                            if (err > 0)
                            {
                                Exception = Exception + "\r\nSQL Request:\r\n" + SQL;
                                Bugreport += Exception + "\r\n";
                                if (DiversityWorkbench.Forms.FormMessageDialog.Show(Exception + "\r\n\r\nStop upgrade?", "SQL error", "Stop update", "Continue update", (err > sp.AcceptedError) ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2, (err > sp.AcceptedError) ? MessageBoxIcon.Stop : MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    // do not accept errors to provide correct script results
                                    sp.AcceptedError = 0;
                                    OK = false;
                                    break;
                                }
                                else
                                {
                                    // accept at least actual error level to provide correct script results
                                    if (err > sp.AcceptedError)
                                    {
                                        sp.AcceptedError = err;
                                    }
                                }
                            }
                        }
                        // close script to save protocol
                        sp.Close();
                    }
                    else // automatic processing
                    {
                        OK = (sp.AcceptedError >= sp.Run());
                        sp.Close();
                    }
                    // convert XML result file to HTML format
                    resultFile = sp.ConvertResult();
                    if (resultFile == string.Empty)
                    {
                        resultFile = xmlFile;
                    }
                }

                if (Bugreport.Length > 0)
                {
                    DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Bugreport", Bugreport);
                    f.ShowDialog();
                    string Logfile = _UpdatesPath + "\\Bugreport.log";
                    System.IO.StreamWriter sw;
                    if (System.IO.File.Exists(Logfile))
                        sw = new System.IO.StreamWriter(Logfile, true);
                    else
                        sw = new System.IO.StreamWriter(Logfile);
                    try
                    {
                        sw.WriteLine("User:\t" + System.Environment.UserName);
                        sw.Write("Date:\t");
                        sw.WriteLine(DateTime.Now);
                        sw.WriteLine();
                        sw.Write(Bugreport);
                        sw.WriteLine();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
            return OK;
        }

        private void checkBoxExpert_CheckedChanged(object sender, EventArgs e)
        {
            this.splitContainerOutput.Panel1Collapsed = !(this.checkBoxExpert.Checked);
            this.checkBoxSingle.Visible = this.checkBoxExpert.Checked;
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            //this.openFileDialog = new OpenFileDialog();
            //this.openFileDialog.RestoreDirectory = true;
            //this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = _UpdatesPath;
            //this.openFileDialog.Filter = "SQL scripts|*.sql";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.textBoxFile.Tag = this.openFileDialog.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxFile.Text = "";
                    this.textBoxFile.Text = f.FullName;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FormUpdateDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_IsBusy)
            {
                e.Cancel = true;
                return;
            }
            clearControls();
        }

        private void buildWorkList()
        {
            // build up work list
            string actualVersion = this.CurrentVersionOfDatabase.Replace(".", "").Replace("/", "");
            System.Collections.Generic.List<string> workList = new System.Collections.Generic.List<string>();

            while (actualVersion != _VersionFinal)
            {
                bool match = false;
                foreach (string script in _Versions.Keys)
                {
                    if (actualVersion == VersionFrom(script))
                    {
                        workList.Add(script);
                        actualVersion = VersionTo(script);
                        match = true;
                        break;
                    }
                }
                if (!match)
                {
                    // script file is missing to reach final version: terminate loop, error message will be displayed after update
                    actualVersion = _VersionFinal;
                }
            }

            // prepare result panel
            clearControls();
            for (int i = workList.Count; i > 0; i--)
            {
                DiversityWorkbench.UserControls.UserControlUpdateResult result = new DiversityWorkbench.UserControls.UserControlUpdateResult(workList[i - 1], _Versions[workList[i - 1]]);
                result.textBoxVersionOld.Text = VersionFrom(workList[i - 1]).Insert(2, ".").Insert(5, ".");
                result.textBoxVersionNew.Text = VersionTo(workList[i - 1]).Insert(2, ".").Insert(5, ".");
                result.Dock = DockStyle.Top;
                splitContainerOutput.Panel2.Controls.Add(result);
                splitContainerOutput.Panel2.Update();
            }
        }

        private void clearControls()
        {
            for (int i = splitContainerOutput.Panel2.Controls.Count - 1; i >= 0; i--)
            {
                Control uc = splitContainerOutput.Panel2.Controls[i];
                splitContainerOutput.Panel2.Controls.Remove(uc);
                uc.Dispose();
            }
        }

        private void setHeaderText()
        {
            if (this._ForPostgres && this._PostgresSchema.Length > 0)
            {
                if (this._PostgresPackage.Length > 0)
                    this.labelHeader.Text = "Update the package " + _PostgresPackage + " in schema " + _PostgresSchema + " to version " + _VersionFinal;
                else
                    this.labelHeader.Text = "Update the schema " + _PostgresSchema + " to version " + _VersionFinal;
            }
            else if (this._Schema != null && this._Schema.Length > 0)
                this.labelHeader.Text = "Update the schema " + _Schema + " to version " + _VersionFinal;
            else
                this.labelHeader.Text = "Update the database " + _Database + " to version " + _VersionFinal;
        }

        private string CurrentVersionOfDatabase
        {
            get
            {
                string Version = "";
                if (ForPostgres)
                {
                    if (this._PostgresSchema.Length > 0)
                    {
                        if (this._PostgresPackage.Length > 0)
                        {
                            if (this._PostgresPackage.IndexOf("_") > -1)
                            {
                                string Package = this._PostgresPackage.Substring(0, this._PostgresPackage.IndexOf("_"));
                                Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT \"Version\" FROM \"" + this._PostgresSchema + "\".\"PackageAddOn\" WHERE \"Package\" = '" + Package + "' AND \"AddOn\" = '" + this._PostgresPackage + "';");
                                Version = "000000".Substring(Version.Length) + Version;
                            }
                            else
                            {
                                Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT \"Version\" FROM \"" + this._PostgresSchema + "\".\"Package\" WHERE \"Package\" = '" + this._PostgresPackage + "';");
                                Version = "000000".Substring(Version.Length) + Version;
                            }
                        }
                        else
                        {
                            Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT \"" + this._PostgresSchema + "\".VERSION()");// Postgres.PostgresExecuteSqlSkalar("SELECT \"" + this._PostgresSchema + "\".VERSION()");
                            Version = "000000".Substring(Version.Length) + Version;
                        }
                    }
                    else
                        Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT \"public\".VERSION()");// Postgres.PostgresExecuteSqlSkalar("SELECT public.VERSION()");
                }
                else
                {
                    if (this._Schema != null && this._Schema.Length > 0)
                    {
                        string SQL = "SELECT [" + this._Schema + "].VERSION()";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_Connection.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        try
                        {
                            con.Open();
                            Version = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            con.Close();
                        }
                        catch { }
                        Version = "000000".Substring(Version.Length) + Version;
                    }
                    else
                    {
                        string SQL = "SELECT DBO.VERSION()";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_Connection.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        try
                        {
                            con.Open();
                            Version = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            con.Close();
                        }
                        catch { }
                    }
                }
                return Version;
            }
        }

        private string VersionFrom(string ScriptName)
        {
            string Version = ScriptName.Substring(ScriptName.IndexOf("_") + 1);
            if (_PostgresPackage != null && _PostgresPackage.IndexOf("_") > -1 && Version.Substring(0, 1) != "0")
                Version = Version.Substring(Version.IndexOf("_") + 1);
            Version = Version.Substring(0, Version.IndexOf("_"));
            if (_ForPostgres && _PostgresSchema.Length > 0)
            {
                Version = int.Parse(Version).ToString();
                Version = "000000".Substring(Version.Length) + Version;
            }
            else if (this._Schema != null && _Schema.Length > 0)
            {
                Version = int.Parse(Version).ToString();
                Version = "000000".Substring(Version.Length) + Version;
            }
            return Version;
        }

        private string VersionTo(string ScriptName)
        {
            string Version = ScriptName.ToUpper();
            while (Version.Contains("_"))
            {
                Version = Version.Substring(Version.IndexOf("_") + 1);
            }
            if (ScriptName.Contains("."))
            {
                Version = Version.Substring(0, Version.IndexOf("."));
            }
            if (_ForPostgres && _PostgresSchema.Length > 0)
            {
                Version = int.Parse(Version).ToString();
                Version = "000000".Substring(Version.Length) + Version;
            }
            return Version;
        }

        private string SqlExecuteScalar(string SQL)
        {
            string Result = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
                return Result;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_Connection.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
            }
            catch { }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Result;
        }

        private bool SqlExecuteNonQuery(string SQL)
        {
            bool OK = true;

            // MW 2015-03-11: Do not use default connection
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_Connection.ConnectionString);
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);

            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                // MW 2017-11-17: Use timeout
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex) { OK = false; }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return OK;
        }

        #endregion

        #region Postgres

        #endregion

        #region Button events for feedback and timeout

        private void buttonSetTimeout_Click(object sender, EventArgs e)
        {
            int Timeout = DiversityWorkbench.Settings.TimeoutDatabase;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database queries in seconds");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
                this.setTimeOutButtonText();
            }
        }

        private void setTimeOutButtonText()
        {
            this.buttonSetTimeout.Text = "     ";
            if (DiversityWorkbench.Settings.TimeoutDatabase == 0)
                this.buttonSetTimeout.Text += "inf.";
            else
                this.buttonSetTimeout.Text += DiversityWorkbench.Settings.TimeoutDatabase.ToString() + " s.";
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            //DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name, System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

    }
}
