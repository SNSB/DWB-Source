using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DiversityWorkbench.CacheDB.ReplaceDB
{

    public partial class Replacement : Component
    {

        #region Parameter
        public enum Status { _None, Added, Congruent, DataCountDifferent, DataAreMissing, UpdateNeeded, Missing }
        public enum Type { Agent, Collection, Gazetteer, Project, Project_Package, Reference, Reference_Webservice, SamplingPlot, ScientificTerm, Taxon, Taxon_Webservice }

        private string _OldReplacedDatabase = "";
        private string _NewReplacementDatabase = "";
        private System.Collections.Generic.Dictionary<Replacement.Type, ReplacedPart> _Sources;
        private bool _KeepCopyOfDB = true;

        public bool KeepCopyOfDB
        {
            get
            {
                return this._KeepCopyOfDB;
            }
            set
            {
                this._KeepCopyOfDB = value;
            }
        }
        public bool RestrictToCritical = true;

        #endregion

        #region Construction
        public Replacement()
        {
            InitializeComponent();
            this._NewReplacementDatabase = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
//#if !DEBUG
//            _KeepCopyOfDB = false;
//#endif
        }

        public Replacement(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

#endregion

        public void setSources(System.Collections.Generic.Dictionary<Replacement.Type, ReplacedPart> Sources)
        {
            this._Sources = Sources;
        }

        public System.Collections.Generic.Dictionary<Replacement.Type, ReplacedPart> Sources()
        {
            return this._Sources;
        }

        public void SetOldReplacedDatabase(string OldReplacedDatabase)
        {
            this._OldReplacedDatabase = OldReplacedDatabase;
            this._OldProjects = null;
            this._ProjectReplacedParts = null;
        }

        public string OldDatabase { get { return this._OldReplacedDatabase; } }
        public string NewDatabase { get { return this._NewReplacementDatabase; } }


        private System.Data.DataTable _DtDatabases;
        public System.Data.DataTable DtDatabases()
        {
            if (this._DtDatabases == null)
            {
                this._DtDatabases = new DataTable();

                string SQL = "SELECT datname FROM pg_database " +
                    "WHERE datistemplate = false " +
                    "AND datname LIKE 'Diversity%Cache%' " +
                    "and datname <> '" + this._NewReplacementDatabase + "' " +
                    "order by datname";
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref this._DtDatabases, ref Message);
            }
            return this._DtDatabases;
        }

#region Status

        public Status StatusWorst(Status statusOld, Status statusNew)
        {
            if ((int)statusNew > (int)statusOld)
                return statusNew;
            else
                return statusOld;
        }

        public bool CanReplaceDB(Status status)
        {
            if ((int)status > (int)Status.DataCountDifferent)
                return false;
            else
                return true;
        }

        public System.Windows.Forms.ImageList ImageListStates()
        {
            return this.imageListState;
        }

        public System.Drawing.Image ImageStatus(Status status)
        {
            switch (status)
            {
                case Status.Added:
                    return this.imageListState.Images[0];
                case Status.Congruent:
                    return this.imageListState.Images[1];
                case Status.DataCountDifferent:
                    return this.imageListState.Images[2];
                case Status.DataAreMissing:
                    return this.imageListState.Images[3];
                case Status.UpdateNeeded:
                    return this.imageListState.Images[4];
                default:
                    return this.imageListState.Images[5];
            }
        }

        public int ImageIndexStatus(Status status)
        {
            switch (status)
            {
                case Status.Added:
                    return 0;
                case Status.Congruent:
                    return 1;
                case Status.DataCountDifferent:
                    return 2;
                case Status.DataAreMissing:
                    return 3;
                case Status.UpdateNeeded:
                    return 4;
                default:
                    return 5;
            }
        }

        public string StatusDescription(Status status)
        {
            string Message = "OK";
            switch (status)
            {
                case Replacement.Status.Added:
                    Message = "Will be added";
                    break;
                case Replacement.Status.Congruent:
                    Message = "OK";
                    break;
                case Replacement.Status.DataAreMissing:
                    Message = "No data";
                    break;
                case Replacement.Status.UpdateNeeded:
                    Message = "Needs update";
                    break;
                case Replacement.Status.Missing:
                    Message = "Is missing";
                    break;
                case Status.DataCountDifferent:
                    Message = "Different content numbers";
                    break;
                case Status._None:
                    Message = "";
                    break;
                default:
                    Message = "Packages differing";
                    break;
            }
            return Message;
        }

        #endregion

        #region Projects

        public int ProjectCount()
        {
            int iCount = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ProjectList())
            {
                iCount++;
            }
            return iCount;
        }


        private System.Collections.Generic.Dictionary<int, string> _OldProjects;
        private System.Collections.Generic.Dictionary<int, string> _NewProjects;

        private System.Collections.Generic.SortedList<string, string> ProjectList()
        {
            System.Collections.Generic.List<string> Databases = new List<string>();
            Databases.Add(this.OldDatabase);
            Databases.Add(this.NewDatabase);
            System.Collections.Generic.SortedList<string, string> Projects = new SortedList<string, string>();
            try
            {
                string SQL = "select substring(schema_name from 9) as \"Project\" " +
                    "from information_schema.schemata " +
                    "where schema_name like 'Project_%' " +
                    "order by schema_name";
                foreach(string Database in Databases)
                {
                    string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(Database);
                    System.Data.DataTable dt = new DataTable();
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
                    con.Open();
                    Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, con);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Project = R[0].ToString();
                        try
                        {
                            if (!Projects.ContainsKey(Project))
                                Projects.Add(Project, Project);
                        }
                        catch (System.Exception ex) { }
                    }
                    con.Close();
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return Projects;
        }


        private System.Collections.Generic.SortedList<string, ReplacedPart> _ProjectReplacedParts;

        /// <summary>
        /// the list of Projects involved in the replacement
        /// </summary>
        /// <returns>sorted list of projects</returns>
        public System.Collections.Generic.SortedList<string, ReplacedPart> ProjectReplacedParts()
        {
            if (this._ProjectReplacedParts == null)
            {
                try
                {
                    this._ProjectReplacedParts = new SortedList<string, ReplacedPart>();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> P in this.ProjectList())
                    {
                        ReplacedPart CO = new ReplacedPart(this, "Project_" + P.Value, Type.Project);
                        this._ProjectReplacedParts.Add(P.Value, CO);
                    }
                }
                catch(System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return this._ProjectReplacedParts;
        }


        #endregion

        #region Sources

        public int SourceCount()
        {
            int iCount = 0;
            foreach(System.Collections.Generic.KeyValuePair<Replacement.Type, ReplacedPart> KV in this._Sources)
            {
                if (KV.Key == Type.Project || KV.Key == Type.Project_Package)
                    continue;
                iCount++;
            }
            return iCount;
        }

        public System.Collections.Generic.SortedDictionary<string, ReplacedPart> SourceReplacedParts(Replacement.Type Type)
        {
            System.Collections.Generic.SortedDictionary<string, ReplacedPart> Dict = new SortedDictionary<string, ReplacedPart>();
            foreach (string T in this.SourceViews(Type))
            {
                ReplacedPart RP = new ReplacedPart(this, "public", Type, T);
                Dict.Add(T, RP);
            }
            return Dict;
        }

        public System.Collections.Generic.List<string> SourceViews(Replacement.Type Type)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            if (this.Sources().ContainsKey(Type))
            {
                ReplacedPart RP = this.Sources()[Type];
                string SQL = "SELECT DISTINCT \"SourceView\" FROM \"" + RP.MainTable + "\" ";
                switch (Type)
                {
                    case Replacement.Type.Taxon:
                        SQL += " WHERE \"NameID\" > -1 ";
                        break;
                    case Replacement.Type.Taxon_Webservice:
                        SQL += " WHERE \"NameID\" = -1 ";
                        break;
                }
                SQL += " ORDER BY \"SourceView\";";
                System.Data.DataTable dtOld = new System.Data.DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtOld, ref Message, this._OldReplacedDatabase);
                foreach (System.Data.DataRow R in dtOld.Rows)
                {
                    L.Add(R[0].ToString());
                }
                System.Data.DataTable dtNew = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtNew, ref Message, this._NewReplacementDatabase);
                foreach (System.Data.DataRow R in dtNew.Rows)
                {
                    if (!L.Contains(R[0].ToString()))
                        L.Add(R[0].ToString());
                }
            }
            return L;
        }

        #endregion

        #region Replace database

        public bool ReplaceDatabase(ref string Message)
        {
            string DatabaseOwner = "CacheAdmin";
            string CurrentDB = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
            string SQL = "";
            string TempDB = "";
            bool OK = false;
            try
            {
                // Ensure owner of databases is CacheAdmin
                System.Collections.Generic.List<string> CheckDBs = new List<string>();
                CheckDBs.Add(this.OldDatabase);
                CheckDBs.Add(this.NewDatabase);
                foreach(string DB in CheckDBs)
                {
                    Message = "";
                    SQL = "SELECT u.usename " +
                        "FROM pg_database d " +
                        "JOIN pg_user u ON(d.datdba = u.usesysid) " +
                        "WHERE d.datname = '" + DB + "'; ";
                    string CheckOwner = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
                    if (Message.Length > 0 && Message.StartsWith("Object "))
                    {
                        Message = "";
                        // try with roles
                        SQL = "SELECT U.rolname " +
                            "FROM pg_roles AS U JOIN pg_database AS D ON(D.datdba = U.oid) " +
                            "WHERE D.datname = '" + DB + "'; ";
                        CheckOwner = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
                    }
                    if (Message.Length > 0)
                    {
                        System.Windows.Forms.MessageBox.Show(Message);
                        return false;
                    }
                    if (CheckOwner.Length == 0)
                    {
                        Message = "Can not read owner of database. Owner of " + DB + " must be CacheAdmin";
                        System.Windows.Forms.MessageBox.Show(Message);
                        return false;
                    }
                    else if (CheckOwner != "CacheAdmin")
                    {
                        // try to set Owner to CacheAdmin
                        SQL = "ALTER DATABASE \"" + DB + "\" OWNER TO \"CacheAdmin\";";
                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))
                        {
                            Message = "Owner of " + DB + " must be CacheAdmin";
                            System.Windows.Forms.MessageBox.Show(Message);
                            return false;
                        }
                    }
                }

                if (this.KeepCopyOfDB)
                {
                    // if the current database should be kept, create a copy of it
                    TempDB = this.NewDatabase + "_Temp";
                    // remove a database with this name if it exists
                    SQL = "DROP DATABASE IF EXISTS \"" + TempDB + "\";";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false);
                    if (!OK)
                        OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false);
                    if (!OK)
                    {
                        if (Message.Length > 0)
                            System.Windows.Forms.MessageBox.Show("Removal of " + TempDB + " failed:\r\n" + Message);
                        return false;
                    }
                    // create a copy of the current DB
                    OK = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().CreateCopy(TempDB, DatabaseOwner, true, "", ref Message);
                    if (!OK)
                    {
                        if (Message.Length > 0)
                            System.Windows.Forms.MessageBox.Show(Message);
                        return false;
                    }
                    DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(CurrentDB);
                }

                // Rename the old database that should be replaced to ..._OLD
                SQL = "SELECT pg_terminate_backend( pid ) " +
                    "FROM pg_stat_activity " +
                    "WHERE pid <> pg_backend_pid( ) " +
                    "AND datname = '" + this.OldDatabase + "'; " +
                    "ALTER DATABASE \"" + this.OldDatabase + "\" RENAME TO \"" + this.OldDatabase + "_OLD\"; ";
                OK = false;
                if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                {
                    if (Message.Length == 0)
                    {
                        DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                        // Change to the renamed database
                        if (DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(this.OldDatabase + "_OLD"))
                        {
                            // Rename the new database that should be the replacement to the name of the old replaced database
                            SQL = "SELECT pg_terminate_backend( pid ) " +
                                "FROM pg_stat_activity " +
                                "WHERE pid <> pg_backend_pid( ) " +
                                "AND datname = '" + this.NewDatabase + "'; " +
                                "ALTER DATABASE \"" + this.NewDatabase + "\" RENAME TO \"" + this.OldDatabase + "\";";
                            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                            {
                                if (Message.Length == 0)
                                {
                                    // Change to the replacement database
                                    if (DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(this.OldDatabase))
                                    {
                                        DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                                        // Drop the old database
                                        SQL = "SELECT pg_terminate_backend( pid ) " +
                                            "FROM pg_stat_activity " +
                                            "WHERE pid <> pg_backend_pid( ) " +
                                            "AND datname = '" + this.OldDatabase + "_OLD'; ";
                                        if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                                        {
                                            SQL = "DROP DATABASE \"" + this.OldDatabase + "_OLD\";";
                                            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false))
                                            {
                                                OK = true;
                                                if (this.KeepCopyOfDB && TempDB.Length > 0)
                                                {
                                                    // Rename the temp database that
                                                    SQL = "SELECT pg_terminate_backend( pid ) " +
                                                        "FROM pg_stat_activity " +
                                                        "WHERE pid <> pg_backend_pid( ) " +
                                                        "AND datname = '" + TempDB + "'; " +
                                                        "ALTER DATABASE \"" + TempDB + "\" RENAME TO \"" + this.NewDatabase + "\"; ";
                                                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false);
                                                }
                                                //this.buttonReplace.Enabled = false;
                                                //this.comboBoxReplacedDatabase.Enabled = false;
                                                //_DatabaseHasBeenReplaced = true;
                                                //System.Windows.Forms.MessageBox.Show("Replacement has been successful");
                                                //this.Close();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            if (!OK)
            {
                System.Windows.Forms.MessageBox.Show("Replacement failed: " + Message);
            }
            return OK;
        }

        #endregion

    }
}
