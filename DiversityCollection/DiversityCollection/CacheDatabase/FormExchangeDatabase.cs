using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{

    public partial class FormExchangeDatabase : Form
    {
        #region Parameter
 
        private string _OldReplacedDatabase = "";
        private string _NewReplacementDatabase = "";
        private string _IncongruenceIssues = "";
        private bool _DatabaseHasBeenReplaced = false;
        private ReplaceDatabase _replaceDatabase;
        //private bool _IsCongruent = false;

        public string IncongruenceIssues
        {
            get => _IncongruenceIssues;
            set
            {
                if (value.Length > 0)
                {
                    //_IsCongruent = false;
                    if (_IncongruenceIssues.Length > 0)
                        _IncongruenceIssues += "\r\n";
                    _IncongruenceIssues += value;
                }
            }
        }

        public bool IsCongruent { get => _IncongruenceIssues.Length == 0; }

        #endregion

        #region Construction

        public FormExchangeDatabase()
        {
            InitializeComponent();
            this.initForm();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                this._NewReplacementDatabase = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                this.labelHeader.Text = "Replace the selected database with the current database " + this._NewReplacementDatabase;
                string SQL = "SELECT datname FROM pg_database " +
                    "WHERE datistemplate = false " +
                    "AND datname LIKE 'DiversityCollectionCache%' " +
                    "and datname <> '" + this._NewReplacementDatabase + "' " +
                    "order by datname";
                System.Data.DataTable dtDB = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtDB, ref Message);
                this.comboBoxExchangedDatabase.DataSource = dtDB;
                this.comboBoxExchangedDatabase.DisplayMember = "datname";
                this.comboBoxExchangedDatabase.ValueMember = "datname";
                this._replaceDatabase = new ReplaceDatabase();
                this.tabControlProjects.ImageList = this._replaceDatabase.imageList;
                this.tabControlCongruence.ImageList = this._replaceDatabase.imageList;
                this.tabControlSources.ImageList = this._replaceDatabase.imageList;
                this.tabControlWebservices.ImageList = this._replaceDatabase.imageList;
            }
            catch (System.Exception ex)
            {
            }
        }
        
        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Congruence

        private void ResetCongruence()
        {
            this._IncongruenceIssues = "";
            //this._IsCongruent = true;
        }

        //private bool StatusIsCongruent(UserControlExchangeCongruence.CongruenceStatus Status)
        //{
        //    bool Congruent = false;
        //    switch (Status)
        //    {
        //        case UserControlExchangeCongruence.CongruenceStatus.Added:
        //        case UserControlExchangeCongruence.CongruenceStatus.Congruent:
        //        case UserControlExchangeCongruence.CongruenceStatus.DataCountDifferent:
        //            Congruent = true;
        //            break;
        //    }
        //    return Congruent;
        //}


        //private bool DatabaseIsCongruent()
        //{
        //    bool ProjectOK = this.ProjectsAreCongruent();
        //    bool SourcesOK = this.SourcesAreCongruent();
        //    //if (ProjectOK && SourcesOK)
        //    //    IsCongruent = true;
        //    return IsCongruent;
        //}

        #region Projects

        private void CongruenceOfProjects()
        {
            // getting rid of old controls
            this.tabControlProjects.TabPages.Clear();
            //this.tabPageProjects.Controls.Clear();

            //getting the projects of the databases
            System.Collections.Generic.Dictionary<int, string> ProjectsOldReplaced = this.ProjectDictionary(this._OldReplacedDatabase);
            System.Collections.Generic.Dictionary<int, string> ProjectsNewReplacement = this.ProjectDictionary(this._NewReplacementDatabase);

            System.Collections.Generic.Dictionary<int, CongruenceObject> Projects = new Dictionary<int, CongruenceObject>();
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in ProjectsOldReplaced)
                {
                    CongruenceObject CO = new CongruenceObject(KV.Value, this.ProjectCongruenceCounts("Project_" + KV.Value));
                    CO.ContainedObjects = this.CongruenceOfPackages("Project_" + KV.Value);
                    //System.Collections.Generic.List<CongruenceCount> Counts = new List<CongruenceCount>();
                    //CongruenceCount CS = new CongruenceCount("CacheCollectionSpecimen");
                    //Counts.Add(CS);
                    //CO.CongruenceCounts = this.ProjectCongruenceCounts("Project_" + KV.Value);// Counts;
                    //if (!ProjectsNewReplacement.ContainsKey(KV.Key))
                    //{
                    //    CO.Status = ReplaceDatabase.CongruenceStatus.Missing;
                    //}
                    //else if (this.ProjectNeedsUpdate("Project_" + KV.Value))
                    //{
                    //    CO.Status = ReplaceDatabase.CongruenceStatus.UpdateNeeded;
                    //}
                    //else if (!this.ProjectIsEmpty("Project_" + KV.Value, this._OldReplacedDatabase) && this.ProjectIsEmpty("Project_" + KV.Value, this._NewReplacementDatabase))
                    //{
                    //    CO.Status = ReplaceDatabase.CongruenceStatus.DataAreMissing;
                    //}
                    
                    DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(CO);
                    System.Windows.Forms.TabPage TP = new TabPage(CO.ObjectName);
                    TP.ImageIndex = this._replaceDatabase.ImageIndexCongruenceStatus(CO.Status);
                    TP.Controls.Add(U);
                    this.tabControlProjects.TabPages.Add(TP);
                    //this.tabPageProjects.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in ProjectsNewReplacement)
                {
                    if (!ProjectsOldReplaced.ContainsKey(KV.Key))
                    {
                        CongruenceObject CO = new CongruenceObject(KV.Value, this.ProjectCongruenceCounts("Project_" + KV.Value));
                        DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(CO);
                        System.Windows.Forms.TabPage TP = new TabPage(CO.ObjectName);
                        TP.ImageIndex = this._replaceDatabase.ImageIndexCongruenceStatus(CO.Status);
                        TP.Controls.Add(U);
                        this.tabControlProjects.TabPages.Add(TP);
                        //bool IsCongruent = true;
                        //int CountOld = 0;
                        //int CountNew = 0;
                        //DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(KV.Key, KV.Value, ReplaceDatabase.CongruenceStatus.Added, this.PackageCongruence("Project_" + KV.Value, ref IsCongruent, ref CountOld, ref CountNew), this._OldReplacedDatabase, this._NewReplacementDatabase);
                        //IncongruenceIssues = U.IncongruenceIssue();
                        //this.tabPageProjects.Controls.Add(U);
                        U.Dock = DockStyle.Top;
                        U.BringToFront();
                    }
                }
            }
            catch (System.Exception ex)
            {
                IncongruenceIssues = ex.Message;
            }


            //bool Congruent = true;
            //try
            //{
            //    foreach (System.Collections.Generic.KeyValuePair<int, string> KV in ProjectsOldReplaced)
            //    {
            //        CongruenceObject CO = new CongruenceObject(KV.Value, ReplaceDatabase.CongruenceStatus.Congruent);
            //        System.Collections.Generic.List<CongruenceCount> Counts = new List<CongruenceCount>();
            //        CongruenceCount CS = new CongruenceCount("CacheCollectionSpecimen");
            //        Counts.Add(CS);
            //        CO.CongruenceCounts = Counts;
            //        //this.GetContentCount(ref CO.CountOld, ref CO.CountNew, "Project_" + KV.Value, "CacheCollectionSpecimen");
            //        //UserControlExchangeCongruence.CongruenceStatus Status = UserControlExchangeCongruence.CongruenceStatus.Congruent;
            //        if (!ProjectsNewReplacement.ContainsKey(KV.Key))
            //        {
            //            CO.Status = ReplaceDatabase.CongruenceStatus.Missing;
            //            //IncongruenceIssues = KV.Value + " " + Status.ToString();
            //            Congruent = false;
            //        }
            //        else if (this.ProjectNeedsUpdate("Project_" + KV.Value))
            //        {
            //            CO.Status = ReplaceDatabase.CongruenceStatus.UpdateNeeded;
            //            //IncongruenceIssues = KV.Value + " " + Status.ToString();
            //            Congruent = false;
            //        }
            //        else if (!this.ProjectIsEmpty("Project_" + KV.Value, this._OldReplacedDatabase) && this.ProjectIsEmpty("Project_" + KV.Value, this._NewReplacementDatabase))
            //        {
            //            CO.Status = ReplaceDatabase.CongruenceStatus.DataAreMissing;
            //            //IncongruenceIssues = KV.Value + " " + Status.ToString();
            //            Congruent = false;
            //        }
            //        bool ProjectCongruent = true;// Congruent;
            //        int CountOld = 0;
            //        int CountNew = 0;
            //        DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(KV.Key, KV.Value, CO.Status, this.PackageCongruence("Project_" + KV.Value, ref ProjectCongruent, ref CountOld, ref CountNew), this._OldReplacedDatabase, this._NewReplacementDatabase);
            //        if (!ProjectCongruent)
            //            Congruent = ProjectCongruent;
            //        //IncongruenceIssues = U.IncongruenceIssue();
            //        this.tabPageProjects.Controls.Add(U);
            //        U.Dock = DockStyle.Top;
            //        U.BringToFront();
            //    }
            //    foreach (System.Collections.Generic.KeyValuePair<int, string> KV in ProjectsNewReplacement)
            //    {
            //        if (!ProjectsOldReplaced.ContainsKey(KV.Key))
            //        {
            //            bool IsCongruent = true;
            //            int CountOld = 0;
            //            int CountNew = 0;
            //            DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(KV.Key, KV.Value, ReplaceDatabase.CongruenceStatus.Added, this.PackageCongruence("Project_" + KV.Value, ref IsCongruent, ref CountOld, ref CountNew), this._OldReplacedDatabase, this._NewReplacementDatabase);
            //            IncongruenceIssues = U.IncongruenceIssue();
            //            this.tabPageProjects.Controls.Add(U);
            //            U.Dock = DockStyle.Top;
            //            U.BringToFront();
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    IncongruenceIssues = ex.Message;
            //    Congruent = false;
            //}
            //if (!Congruent)
            //    this.tabPageProjects. = System.Drawing.Color.Red;
            //return Congruent;
        }

        private System.Collections.Generic.Dictionary<string, CongruenceObject> CongruenceOfPackages(string Schema)
        {
            System.Collections.Generic.Dictionary<string, CongruenceObject> COs = new Dictionary<string, CongruenceObject>();
            string SQL = "SELECT \"Package\", \"Version\" " +
                "FROM \"" + Schema + "\".\"Package\" " +
                "UNION " +
                "SELECT \"AddOn\", \"Version\" " +
                //"UNION " +
                //"SELECT concat(\"Package\", ' - ', \"AddOn\"), \"Version\" " +
                "FROM \"" + Schema + "\".\"PackageAddOn\";";
            System.Data.DataTable dtOld = this.SqlGetPostgresTable(SQL, this._OldReplacedDatabase);
            System.Data.DataTable dtNew = this.SqlGetPostgresTable(SQL, this._NewReplacementDatabase);
            System.Collections.Generic.Dictionary<string, CongruenceObject> COTempList = new Dictionary<string, CongruenceObject>();
            foreach (System.Data.DataRow R in dtOld.Rows)
            {
                try
                {
                    CongruenceObject CO = new CongruenceObject(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Congruent);
                    CO.VersionOld = int.Parse(R[1].ToString());
                    COTempList.Add(CO.ObjectName, CO);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            foreach (System.Data.DataRow R in dtNew.Rows)
            {
                try
                {
                    if (!COTempList.ContainsKey(R[0].ToString()))
                    {
                        CongruenceObject CO = new CongruenceObject(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Congruent);
                        CO.VersionNew = int.Parse(R[1].ToString());
                        COTempList.Add(CO.ObjectName, CO);
                    }
                    else
                    {
                        CongruenceObject CO = COTempList[R[0].ToString()];
                        CO.VersionNew = int.Parse(R[1].ToString());
                        COTempList[R[0].ToString()] = CO;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            foreach(System.Collections.Generic.KeyValuePair<string, CongruenceObject> CoTemp in COTempList)
            {
                System.Collections.Generic.List<CongruenceCount> Tables = this.ContentCountTables(CoTemp.Key, Schema);
                System.Collections.Generic.List<CongruenceCount> Counts = new List<CongruenceCount>();
                foreach (CongruenceCount CC in Tables)
                {
                    CongruenceCount cc = this.ContentCount(CC.SourceName, Schema);
                    Counts.Add(cc);
                }
                CongruenceObject CO = new CongruenceObject(CoTemp.Key, Counts);
                COs.Add(CO.ObjectName, CO);
            }


            //foreach (System.Data.DataRow R in dtOld.Rows)
            //{
            //    try
            //    {
            //        System.Collections.Generic.List < CongruenceCount > List = this.ContentCountTables("Package");
            //        foreach(CongruenceCount CC in List)
            //        {

            //        }
            //        //System.Collections.Generic.List<CongruenceCount> Counts = this.GetContentCount(R[0].ToString(), Schema,;


            //        if (dtNew.Rows.Count > 0) // if there are any rows in the new version
            //        {
            //            System.Data.DataRow[] rr = dtNew.Select("Package = '" + R[0].ToString() + "'", "");
            //            if (rr.Length == 0)
            //            {
            //                Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Missing);
            //                IncongruenceIssues = R[0].ToString() + " is missing";
            //                AllPackagesCongruent = false; // a package missing in the new database that was present in the old one
            //            }
            //            else
            //            {
            //                int VersOld = int.Parse(R[1].ToString());
            //                int VersNew = int.Parse(rr[0][1].ToString());
            //                if (VersOld > VersNew)
            //                {
            //                    Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.UpdateNeeded);
            //                    IncongruenceIssues = R[0].ToString() + " needs update";
            //                    AllPackagesCongruent = false;
            //                }
            //                else
            //                {
            //                    if (!this.CompareContentCount(ref CountOld, ref CountNew, this._OldReplacedDatabase, this._NewReplacementDatabase, Schema, R[0].ToString()))
            //                    {
            //                        if (CountNew == 0 && CountOld > 0)
            //                        {
            //                            IncongruenceIssues = R[0].ToString() + " contains no data";
            //                            AllPackagesCongruent = false;
            //                            Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.DataAreMissing);
            //                            //IsCongruent = true;
            //                        }
            //                        else if (CountNew < CountOld)
            //                        {
            //                            IncongruenceIssues = R[0].ToString() + " has less data";
            //                            Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.DataCountDifferent);
            //                        }
            //                        else
            //                        {
            //                            Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.DataCountDifferent);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Congruent);
            //                        IsCongruent = true;
            //                    }
            //                }
            //            }
            //        }
            //        else // no rows in the new version
            //        {
            //            Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Missing);
            //            IncongruenceIssues = R[0].ToString() + " is missing";
            //            AllPackagesCongruent = false;
            //        }
            //    }
            //    catch (System.Exception ex)
            //    {
            //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //    }
            //}
            //dtNew.AcceptChanges();
            //foreach (System.Data.DataRow R in dtNew.Rows)
            //{
            //    if (!Packs.ContainsKey(R[0].ToString()))
            //    {
            //        Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Added);
            //        //IsCongruent = true;
            //    }
            //    else
            //    {
            //        //AllPackagesCongruent = false;
            //        //IsCongruent = false;
            //    }
            //}
            //if (!AllPackagesCongruent)
            //    IsCongruent = AllPackagesCongruent;
            return COs;
        }

        private System.Collections.Generic.List<CongruenceCount> ProjectCongruenceCounts(string Schema)
        {
            System.Collections.Generic.List<CongruenceCount> Counts = new List<CongruenceCount>();
            try
            {
                string SQL = "SELECT \"Tablename\", \"TotalCount\" FROM \"" + Schema + "\".\"CacheCount\"";
                string MessageOld = "";
                string MessageNew = "";
                System.Data.DataTable dtOld = new DataTable();
                System.Data.DataTable dtNew = new DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtOld, ref MessageOld, this._OldReplacedDatabase);
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtNew, ref MessageNew, this._NewReplacementDatabase);
                foreach (System.Data.DataRow R in dtOld.Rows)
                {
                    int? Old = null;
                    int? New = null;
                    int iOld = 0;
                    int iNew = 0;
                    if (int.TryParse(R[1].ToString(), out iOld))
                        Old = iOld;
                    if(dtNew != null && dtNew.Rows.Count > 0)
                    {
                        System.Data.DataRow[] rr = dtNew.Select("Tablename = '" + R[0].ToString() + "'");
                        if (rr.Length == 1)
                        {
                            if (int.TryParse(rr[0][1].ToString(), out iNew))
                                New = iNew;
                        }
                    }
                    CongruenceCount CO = new CongruenceCount(R[0].ToString(), Old, New);
                    Counts.Add(CO);
                }
                foreach (System.Data.DataRow R in dtNew.Rows)
                {
                    int? New = null;
                    int iNew = 0;
                    System.Data.DataRow[] rr = dtOld.Select("Tablename = '" + R[0].ToString() + "'");
                    if (rr.Length == 0)
                    {
                        if (int.TryParse(R[1].ToString(), out iNew))
                            New = iNew;
                    }
                    CongruenceCount CO = new CongruenceCount(R[0].ToString(), null, New);
                    Counts.Add(CO);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Counts;
        }

        private bool ProjectsAreCongruent()
        {
            this.tabControlProjects.TabPages.Clear();
            //this.tabPageProjects.Controls.Clear();
            System.Collections.Generic.Dictionary<int, string> ProjectsOldReplaced = this.ProjectsOldReplaced();
            System.Collections.Generic.Dictionary<int, string> ProjectsNewReplacement = this.ProjectsNewReplacement();
            bool Congruent = true;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in ProjectsOldReplaced)
                {
                    ReplaceDatabase.CongruenceStatus Status = ReplaceDatabase.CongruenceStatus.Congruent;
                    if (!ProjectsNewReplacement.ContainsKey(KV.Key))
                    {
                        Status = ReplaceDatabase.CongruenceStatus.Missing;
                        IncongruenceIssues = KV.Value + " " + Status.ToString();
                        Congruent = false;
                    }
                    else if (this.ProjectNeedsUpdate("Project_" + KV.Value))
                    {
                        Status = ReplaceDatabase.CongruenceStatus.UpdateNeeded;
                        IncongruenceIssues = KV.Value + " " + Status.ToString();
                        Congruent = false;
                    }
                    else if (!this.ProjectIsEmpty("Project_" + KV.Value, this._OldReplacedDatabase) && this.ProjectIsEmpty("Project_" + KV.Value, this._NewReplacementDatabase))
                    {
                        Status = ReplaceDatabase.CongruenceStatus.DataAreMissing;
                        IncongruenceIssues = KV.Value + " " + Status.ToString();
                        Congruent = false;
                    }
                    bool ProjectCongruent = true;// Congruent;
                    int CountOld = 0;
                    int CountNew = 0;
                    DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(KV.Key, KV.Value, Status, this.PackageCongruence("Project_" + KV.Value, ref ProjectCongruent, ref CountOld, ref CountNew), this._OldReplacedDatabase, this._NewReplacementDatabase);
                    if (!ProjectCongruent)
                        Congruent = ProjectCongruent;
                    //IncongruenceIssues = U.IncongruenceIssue();

                    System.Windows.Forms.TabPage TP = new TabPage(KV.Value);
                    TP.Controls.Add(U);
                    this.tabControlProjects.TabPages.Add(TP);
                    //this.tabPageProjects.Controls.Add(U);

                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in ProjectsNewReplacement)
                {
                    if (!ProjectsOldReplaced.ContainsKey(KV.Key))
                    {
                        bool IsCongruent = true;
                        int CountOld = 0;
                        int CountNew = 0;
                        DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(KV.Key, KV.Value, ReplaceDatabase.CongruenceStatus.Added, this.PackageCongruence("Project_" + KV.Value, ref IsCongruent, ref CountOld, ref CountNew), this._OldReplacedDatabase, this._NewReplacementDatabase);
                        IncongruenceIssues = U.IncongruenceIssue();

                        System.Windows.Forms.TabPage TP = new TabPage(KV.Value);
                        TP.Controls.Add(U);
                        this.tabControlProjects.TabPages.Add(TP);
                        //this.tabPageProjects.Controls.Add(U);

                        U.Dock = DockStyle.Top;
                        U.BringToFront();
                    }
                }
            }
            catch (System.Exception ex)
            {
                IncongruenceIssues = ex.Message;
                Congruent = false;
            }
            //if (!Congruent)
            //    this.tabPageProjects. = System.Drawing.Color.Red;
            return Congruent;
        }

        private System.Collections.Generic.Dictionary<int, string> ProjectsOldReplaced()
        {
            System.Collections.Generic.Dictionary<int, string> ProjectsDict = this.ProjectDictionary(this._OldReplacedDatabase);
            return ProjectsDict;
        }

        private System.Collections.Generic.Dictionary<int, string> ProjectsNewReplacement()
        {
            System.Collections.Generic.Dictionary<int, string> ProjectsDict = this.ProjectDictionary(this._NewReplacementDatabase);
            return ProjectsDict;
        }

        private System.Collections.Generic.Dictionary<int, string> ProjectDictionary(string DatabaseName)
        {
            System.Collections.Generic.Dictionary<int, string> ProjectsDict = new Dictionary<int, string>();

            try
            {
                string SQL = "select schema_name, substring(schema_name from 9) as \"Project\" " +
                    "from information_schema.schemata " +
                    "where schema_name like 'Project_%' " +
                    "order by schema_name";
                string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DatabaseName);
                System.Data.DataTable dt = new DataTable();
                Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
                con.Open();
                Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, con);
                Npgsql.NpgsqlCommand C = new Npgsql.NpgsqlCommand("", con);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Project = R[1].ToString();
                    C.CommandText = "SELECT \"" + R[0].ToString() + "\".projectid();";
                    int ProjectID;
                    try
                    {
                        if (int.TryParse(C.ExecuteScalar()?.ToString(), out ProjectID))
                            ProjectsDict.Add(ProjectID, Project);
                    }
                    catch (System.Exception ex) { }
                }
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ProjectsDict;
        }

        private bool ProjectIsEmpty(string Schema, string DatabaseName)
        {
            bool IsEmpty = true;
            try
            {
                string SQL = "select count(*) from \"" + Schema + "\".\"CacheCollectionSpecimen\"";
                string Result = this.SqlResultFromPostgresCacheDB(SQL, DatabaseName);
                if (Result.Length == 0 || Result == "0")
                    IsEmpty = true;
                else
                    IsEmpty = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return IsEmpty;
        }

        private bool ProjectNeedsUpdate(string Schema)
        {
            bool NeedsUpdate = false;
            int ExVersion;
            int ReVersion;
            string SQL = "SELECT \"" + Schema + "\".version();";
            if (int.TryParse(this.SqlResultFromPostgresCacheDB(SQL, this._OldReplacedDatabase), out ExVersion) &&
                int.TryParse(this.SqlResultFromPostgresCacheDB(SQL, this._NewReplacementDatabase), out ReVersion))
            {
                if (ExVersion > ReVersion)
                    NeedsUpdate = true;
            }
            return NeedsUpdate;
        }

        private string SqlResultFromPostgresCacheDB(string SQL, string DatabaseName, bool IgnoreException = false)
        {
            string Result = "";
            try
            {
                string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DatabaseName);
                Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
                con.Open();
                Npgsql.NpgsqlCommand C = new Npgsql.NpgsqlCommand(SQL, con);
                Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                if (!IgnoreException)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Result;
        }

        private System.Data.DataTable SqlGetPostgresTable(string SQL, string DatabaseName)
        {
            System.Data.DataTable dt = new DataTable();
            try
            {
                string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DatabaseName);
                if (ConnectionString.Length > 0)
                {
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
                    con.Open();
                    Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, con);
                    ad.Fill(dt);
                    con.Close();
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        //private bool PackagesAreCongruent(string Schema)
        //{
        //    bool Congruent = true;

        //    return Congruent;
        //}

        private System.Collections.Generic.Dictionary<string, ReplaceDatabase.CongruenceStatus> PackageCongruence(string Schema, ref bool IsCongruent, ref int CountOld, ref int CountNew)
        {
            bool AllPackagesCongruent = true;
            //IsCongruent = false;
            System.Collections.Generic.Dictionary<string, ReplaceDatabase.CongruenceStatus> Packs = new Dictionary<string, ReplaceDatabase.CongruenceStatus>();
            string SQL = "SELECT \"Package\", \"Version\" " +
                "FROM \"" + Schema + "\".\"Package\" " +
                "UNION " +
                "SELECT concat(\"Package\", ' - ', \"AddOn\"), \"Version\" " +
                "FROM \"" + Schema + "\".\"PackageAddOn\";";
            System.Data.DataTable dtOld = this.SqlGetPostgresTable(SQL, this._OldReplacedDatabase);
            System.Data.DataTable dtNew = this.SqlGetPostgresTable(SQL, this._NewReplacementDatabase);
            foreach (System.Data.DataRow R in dtOld.Rows)
            {
                try
                {
                    if (dtNew.Rows.Count > 0) // if there are any rows in the new version
                    {
                        System.Data.DataRow[] rr = dtNew.Select("Package = '" + R[0].ToString() + "'", "");
                        if (rr.Length == 0)
                        {
                            Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Missing);
                            IncongruenceIssues = R[0].ToString() + " is missing";
                            AllPackagesCongruent = false; // a package missing in the new database that was present in the old one
                        }
                        else
                        {
                            int VersOld = int.Parse(R[1].ToString());
                            int VersNew = int.Parse(rr[0][1].ToString());
                            if (VersOld > VersNew)
                            {
                                Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.UpdateNeeded);
                                IncongruenceIssues = R[0].ToString() + " needs update";
                                AllPackagesCongruent = false;
                            }
                            else
                            {
                                if (!this.CompareContentCount(ref CountOld, ref CountNew, this._OldReplacedDatabase, this._NewReplacementDatabase, Schema, R[0].ToString()))
                                {
                                    if (CountNew == 0 && CountOld > 0)
                                    {
                                        IncongruenceIssues = R[0].ToString() + " contains no data";
                                        AllPackagesCongruent = false;
                                        Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.DataAreMissing);
                                        //IsCongruent = true;
                                    }
                                    else if (CountNew < CountOld)
                                    {
                                        IncongruenceIssues = R[0].ToString() + " has less data";
                                        Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.DataCountDifferent);
                                    }
                                    else
                                    {
                                        Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.DataCountDifferent);
                                    }
                                }
                                else
                                {
                                    Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Congruent);
                                    IsCongruent = true;
                                }
                            }
                        }
                    }
                    else // no rows in the new version
                    {
                        Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Missing);
                        IncongruenceIssues = R[0].ToString() + " is missing";
                        AllPackagesCongruent = false;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            dtNew.AcceptChanges();
            foreach (System.Data.DataRow R in dtNew.Rows)
            {
                if (!Packs.ContainsKey(R[0].ToString()))
                {
                    Packs.Add(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Added);
                    //IsCongruent = true;
                }
                else
                {
                    //AllPackagesCongruent = false;
                    //IsCongruent = false;
                }
            }
            if (!AllPackagesCongruent)
                IsCongruent = AllPackagesCongruent;
            return Packs;
        }


        private System.Collections.Generic.Dictionary<string, CongruenceObject> PackageCongruence(string Schema, ref bool IsCongruent)
        {
            bool AllPackagesCongruent = true;
            int CountOld = 0;
            int CountNew = 0;
            //IsCongruent = false;
            //int VersionOld = 0;
            //int VersionNew = 0;
            System.Collections.Generic.Dictionary<string, CongruenceObject> Packs = new Dictionary<string, CongruenceObject>();
            string SQL = "SELECT \"Package\", \"Version\" " +
                "FROM \"" + Schema + "\".\"Package\" " +
                "UNION " +
                "SELECT concat(\"Package\", ' - ', \"AddOn\"), \"Version\" " +
                "FROM \"" + Schema + "\".\"PackageAddOn\";";
            System.Data.DataTable dtOld = this.SqlGetPostgresTable(SQL, this._OldReplacedDatabase);
            System.Data.DataTable dtNew = this.SqlGetPostgresTable(SQL, this._NewReplacementDatabase);
            foreach (System.Data.DataRow R in dtOld.Rows)
            {
                CongruenceObject CO = new CongruenceObject(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Congruent);
                try
                {
                    if (dtNew.Rows.Count > 0) // if there are any rows in the new version
                    {
                        System.Data.DataRow[] rr = dtNew.Select("Package = '" + R[0].ToString() + "'", "");
                        if (rr.Length == 0)
                        {
                            CO.Status = ReplaceDatabase.CongruenceStatus.Missing;
                            //Packs.Add(R[0].ToString(), UserControlExchangeCongruence.CongruenceStatus.Missing);
                            IncongruenceIssues = R[0].ToString() + " is missing";
                            AllPackagesCongruent = false; // a package missing in the new database that was present in the old one
                        }
                        else
                        {
                            int VersOld = int.Parse(R[1].ToString());
                            int VersNew = int.Parse(rr[0][1].ToString());
                            if (VersOld > VersNew)
                            {
                                CO.Status = ReplaceDatabase.CongruenceStatus.UpdateNeeded;
                                CO.VersionNew = VersNew;
                                CO.VersionOld = VersOld;
                                //Packs.Add(R[0].ToString(), UserControlExchangeCongruence.CongruenceStatus.UpdateNeeded);
                                IncongruenceIssues = R[0].ToString() + " needs update";
                                AllPackagesCongruent = false;
                            }
                            else
                            {
                                if (!this.CompareContentCount(ref CountOld, ref CountNew, this._OldReplacedDatabase, this._NewReplacementDatabase, Schema, R[0].ToString()))
                                {
                                    //CO.CountNew = CountNew;
                                    //CO.CountOld = CountOld;
                                    if (CountNew == 0 && CountOld > 0)
                                    {
                                        IncongruenceIssues = R[0].ToString() + " contains no data";
                                        AllPackagesCongruent = false;
                                        CO.Status = ReplaceDatabase.CongruenceStatus.DataAreMissing;
                                        //Packs.Add(R[0].ToString(), UserControlExchangeCongruence.CongruenceStatus.DataAreMissing);
                                        //IsCongruent = true;
                                    }
                                    else if (CountNew < CountOld)
                                    {
                                        IncongruenceIssues = R[0].ToString() + " has less data";
                                        CO.Status = ReplaceDatabase.CongruenceStatus.DataCountDifferent;
                                        //Packs.Add(R[0].ToString(), UserControlExchangeCongruence.CongruenceStatus.DataAreLess);
                                    }
                                    else
                                    {
                                        CO.Status = ReplaceDatabase.CongruenceStatus.DataCountDifferent;
                                        //Packs.Add(R[0].ToString(), UserControlExchangeCongruence.CongruenceStatus.DataProblem);
                                    }
                                }
                                else
                                {
                                    //Packs.Add(R[0].ToString(), UserControlExchangeCongruence.CongruenceStatus.Congruent);
                                    IsCongruent = true;
                                }
                            }
                        }
                    }
                    else // no rows in the new version
                    {
                        CO.Status = ReplaceDatabase.CongruenceStatus.Missing;
                        //Packs.Add(R[0].ToString(), UserControlExchangeCongruence.CongruenceStatus.Missing);
                        IncongruenceIssues = R[0].ToString() + " is missing";
                        AllPackagesCongruent = false;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                Packs.Add(R[0].ToString(), CO);
            }
            dtNew.AcceptChanges();
            foreach (System.Data.DataRow R in dtNew.Rows)
            {
                if (!Packs.ContainsKey(R[0].ToString()))
                {
                    CongruenceObject CO = new CongruenceObject(R[0].ToString(), ReplaceDatabase.CongruenceStatus.Added);
                    Packs.Add(R[0].ToString(), CO);
                    //IsCongruent = true;
                }
                else
                {
                    //AllPackagesCongruent = false;
                    //IsCongruent = false;
                }
            }
            if (!AllPackagesCongruent)
                IsCongruent = AllPackagesCongruent;
            return Packs;
        }

        #endregion

        #region Sources

        private void CongruenceOfSources()
        {
            System.Collections.Generic.Dictionary<UserControlLookupSource.TypeOfSource, System.Windows.Forms.TabPage> SourceIssues = new Dictionary<UserControlLookupSource.TypeOfSource, TabPage>();
            SourceIssues.Add(UserControlLookupSource.TypeOfSource.Agents, this.tabPageAgents);
            SourceIssues.Add(UserControlLookupSource.TypeOfSource.Gazetteer, this.tabPageGazetteer);
            SourceIssues.Add(UserControlLookupSource.TypeOfSource.Plots, this.tabPagePlots);
            SourceIssues.Add(UserControlLookupSource.TypeOfSource.References, this.tabPageReferences);
            SourceIssues.Add(UserControlLookupSource.TypeOfSource.ScientificTerms, this.tabPageTerms);
            SourceIssues.Add(UserControlLookupSource.TypeOfSource.Taxa, this.tabPageTaxa);

            ReplaceDatabase.CongruenceStatus status = this.CongruenceOfSources(SourceIssues, false);
            //tabPageTaxaWebservice.ImageIndex = this._replaceDatabase.ImageIndexCongruenceStatus(status);
            this.tabPageSources.ImageIndex = this._replaceDatabase.ImageIndexCongruenceStatus(status);


            //foreach (System.Collections.Generic.KeyValuePair<UserControlLookupSource.TypeOfSource, System.Windows.Forms.TabPage> SI in SourceIssues)
            //{
            //    // Remove the old controls
            //    SI.Value.Controls.Clear();
            //    // get the source views
            //    System.Collections.Generic.List<string> SourceViewList = this.SourceViews(SI.Key);
            //    // for every source view get the content counts for all listed tables, extrat the worst difference and insert a user control
            //    foreach(string SV in SourceViewList)
            //    {

            //    }
            //    //System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, CongruenceCount>> SourceViews = this.SourceContentCounts(SI.Key);


            //    //System.Collections.Generic.Dictionary<string, CongruenceObject> Objects = this.CongruenceOfSource(SI.Key);
            //    //foreach(System.Collections.Generic.KeyValuePair<string, CongruenceObject> CO in Objects)
            //    //{
            //    //    try
            //    //    {
            //    //        //SourceTabPage.Controls.Clear();
            //    //        //System.Collections.Generic.List<string> SourceOldReplaced = this.SourceListOldReplaced(SQL);
            //    //        //System.Collections.Generic.List<string> SourceNewReplacement = this.SourceListNewReplacement(SQL);
            //    //        //UserControlExchangeCongruence.CongruenceStatus Status = UserControlExchangeCongruence.CongruenceStatus.Congruent;
            //    //        //foreach (string S in SourceOldReplaced)
            //    //        //{
            //    //        //    if (!SourceNewReplacement.Contains(S))
            //    //        //    {
            //    //        //        Status = UserControlExchangeCongruence.CongruenceStatus.Missing;
            //    //        //        IncongruenceIssues = S + " " + Status.ToString();
            //    //        //        //Congruent = false;
            //    //        //    }
            //    //        //    DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(SourceType, S, Status);
            //    //        //    SourceTabPage.Controls.Add(U);
            //    //        //    U.Dock = DockStyle.Top;
            //    //        //    U.BringToFront();
            //    //        //}
            //    //        //foreach (string S in SourceNewReplacement)
            //    //        //{
            //    //        //    if (!SourceOldReplaced.Contains(S))
            //    //        //    {
            //    //        //        Status = UserControlExchangeCongruence.CongruenceStatus.Added;
            //    //        //        DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(SourceType, S, Status);
            //    //        //        SourceTabPage.Controls.Add(U);
            //    //        //        U.Dock = DockStyle.Top;
            //    //        //        U.BringToFront();
            //    //        //    }
            //    //        //}
            //    //    }
            //    //    catch (System.Exception ex)
            //    //    {
            //    //        IncongruenceIssues = ex.Message;
            //    //        //Congruent = false;
            //    //    }

            //    //}
            //}
        }

        //private System.Collections.Generic.Dictionary<string, CongruenceObject> CongruenceOfSource(UserControlLookupSource.TypeOfSource SourceType)
        //{
        //    System.Collections.Generic.Dictionary<string, CongruenceObject> Objects = new Dictionary<string, CongruenceObject>();
        //    try
        //    {

        //        System.Collections.Generic.Dictionary<string, int> SourceOldReplaced = this.SourceContentCount(SourceType, true);
        //        System.Collections.Generic.Dictionary<string, int> SourceNewReplacement = this.SourceContentCount(SourceType); 
        //        foreach (System.Collections.Generic.KeyValuePair<string, int> KV in SourceOldReplaced)
        //        {
        //            CongruenceObject CO = new CongruenceObject(KV.Key, UserControlExchangeCongruence.CongruenceStatus.Congruent);
        //            foreach(CongruenceCount CC in CO.CongruenceCounts)
        //            {

        //            }
        //            //CO.CountOld = KV.Value;
        //            //if (!SourceNewReplacement.ContainsKey(KV.Key))
        //            //{
        //            //    CO.Status = UserControlExchangeCongruence.CongruenceStatus.Missing;
        //            //}
        //            //else
        //            //{
        //            //    CO.CountNew = SourceNewReplacement[KV.Key];
        //            //    if (CO.CountNew == 0 && CO.CountOld > 0)
        //            //        CO.Status = UserControlExchangeCongruence.CongruenceStatus.DataAreMissing;
        //            //    else if (CO.CountNew < CO.CountOld)
        //            //        CO.Status = UserControlExchangeCongruence.CongruenceStatus.DataAreLess;
        //            //    else if (CO.CountOld == 0 && CO.CountNew == 0)
        //            //        CO.Status = UserControlExchangeCongruence.CongruenceStatus.DataProblem;
        //            //}
        //            Objects.Add(CO.ObjectName, CO);
        //        }
        //        foreach (System.Collections.Generic.KeyValuePair<string, int> KV in SourceNewReplacement)
        //        {
        //            if (!SourceOldReplaced.ContainsKey(KV.Key))
        //            {
        //                CongruenceObject CO = new CongruenceObject(KV.Key, UserControlExchangeCongruence.CongruenceStatus.Added);
        //                //CO.CountNew = KV.Value;
        //                Objects.Add(CO.ObjectName, CO);
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //    return Objects;
        //}

        //private System.Collections.Generic.Dictionary<string, int> SourceContentCount(UserControlLookupSource.TypeOfSource SourceType, bool IsForOldDB = false)
        //{
        //    System.Data.DataTable dt = new DataTable();
        //    try
        //    {
        //        string DatabaseName = this._NewReplacementDatabase;
        //        if (IsForOldDB)
        //            DatabaseName = this._OldReplacedDatabase;
        //        string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DatabaseName);
        //        CongruenceObject CO = new CongruenceObject(SourceType.ToString(), UserControlExchangeCongruence.CongruenceStatus.Congruent);
        //        string CountObject = this.ContentCountObject(SourceType.ToString());
        //        string SQL = "SELECT \"SourceView\", Count(*) FROM public.\"" + CountObject + "\" GROUP BY \"SourceView\";";
        //        Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
        //        con.Open();
        //        Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, con);
        //        Npgsql.NpgsqlCommand C = new Npgsql.NpgsqlCommand("", con);
        //        ad.Fill(dt);
        //        con.Close();
        //        con.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    System.Collections.Generic.Dictionary<string, int> Dict = new Dictionary<string, int>();
        //    foreach (System.Data.DataRow R in dt.Rows)
        //        Dict.Add(R[0].ToString(), int.Parse(R[1].ToString()));
        //    return Dict;
        //}


        /// <summary>
        /// List all source views within a source type
        /// </summary>
        /// <param name="SourceType"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<string> SourceViews(UserControlLookupSource.TypeOfSource SourceType, bool ForWebservice = false)
        {
            System.Collections.Generic.List<string> SV = new List<string>();
            string Source = this.ContentCountTables(SourceType.ToString(), "public")[0].SourceName;
            string SQL = "SELECT DISTINCT \"SourceView\" FROM \"" + Source + "\"";
            switch(SourceType)
            {
                case UserControlLookupSource.TypeOfSource.Taxa:
                    SQL += " WHERE \"NameID\"";
                    if (ForWebservice)
                        SQL += " = -1";
                    else
                        SQL += " > -1";
                    break;
            }
            SQL += ";";

            //New
            System.Data.DataTable dtNew = new DataTable();
            string ConnectionStringNew = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._NewReplacementDatabase);
            Npgsql.NpgsqlConnection conNew = new Npgsql.NpgsqlConnection(ConnectionStringNew);
            conNew.Open();
            Npgsql.NpgsqlDataAdapter adNew = new Npgsql.NpgsqlDataAdapter(SQL, conNew);
            adNew.Fill(dtNew);
            foreach (System.Data.DataRow R in dtNew.Rows)
            {
                string SourceView = R[0].ToString();
                if (!SV.Contains(SourceView))
                    SV.Add(SourceView);
            }

            //Old
            System.Data.DataTable dtOld = new DataTable();
            string ConnectionStringOld = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._OldReplacedDatabase);
            Npgsql.NpgsqlConnection conOld = new Npgsql.NpgsqlConnection(ConnectionStringOld);
            conOld.Open();
            Npgsql.NpgsqlDataAdapter adOld = new Npgsql.NpgsqlDataAdapter(SQL, conOld);
            adOld.Fill(dtOld);
            foreach (System.Data.DataRow R in dtOld.Rows)
            {
                string SourceView = R[0].ToString();
                if (!SV.Contains(SourceView))
                    SV.Add(SourceView);
            }

            return SV;
        }

        private ReplaceDatabase.CongruenceStatus CongruenceOfSources(System.Collections.Generic.Dictionary<UserControlLookupSource.TypeOfSource, System.Windows.Forms.TabPage> SourceIssues, bool ForWebservice = false)
        {
            ReplaceDatabase.CongruenceStatus congruenceStatus = ReplaceDatabase.CongruenceStatus.Congruent;
            foreach (System.Collections.Generic.KeyValuePair<UserControlLookupSource.TypeOfSource, System.Windows.Forms.TabPage> SI in SourceIssues)
            {
                // Remove the old controls
                SI.Value.Controls.Clear();

                // get the source views
                System.Collections.Generic.List<string> SourceList = this.SourceViews(SI.Key, ForWebservice);

                // for every service get the content counts for all listed tables, extrat the worst difference and insert a user control
                foreach (string SV in SourceList)
                {
                    // Congruence object for the whole SourceView
                    CongruenceObject SourceView = new CongruenceObject(SV, ReplaceDatabase.CongruenceStatus.Congruent);

                    // get the listed tables
                    System.Collections.Generic.List<CongruenceCount> CC = this.ContentCountTables(SI.Key.ToString(), "public");

                    // get the content counts for every table
                    foreach (CongruenceCount C in CC)
                    {
                        CongruenceCount Co = new CongruenceCount(C.SourceName);
                        this.GetContentCount(ref Co, "public", "\"SourceView\" = '" + SV + "'");
                        SourceView.CongruenceCounts.Add(Co);

                        // getting the worst status for the source view
                        if (SourceView.Status < Co.Status)
                        {
                            SourceView.Status = Co.Status;
                        }
                        if (congruenceStatus < Co.Status)
                            congruenceStatus = Co.Status;
                    }

                    // create a user control and insert it in the tabpage
                    DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(SourceView);
                    SI.Value.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                    SI.Value.ImageIndex = this._replaceDatabase.ImageIndexCongruenceStatus(congruenceStatus);
                }
            }
            return congruenceStatus;
        }


        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, CongruenceCount>> SourceContentCounts(UserControlLookupSource.TypeOfSource SourceType)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, CongruenceCount>> DictSourceViews = new Dictionary<string, System.Collections.Generic.Dictionary<string, CongruenceCount>>();
            System.Data.DataTable dtNew = new DataTable();
            System.Data.DataTable dtOld = new DataTable();
            string ConnectionStringNew = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._NewReplacementDatabase);
            string ConnectionStringOld = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._OldReplacedDatabase);
            //Npgsql.NpgsqlConnection conNew = new Npgsql.NpgsqlConnection(ConnectionStringNew);
            //Npgsql.NpgsqlConnection conOld = new Npgsql.NpgsqlConnection(ConnectionStringOld);
            try
            {
                //conNew.Open();
                //conOld.Open();
                foreach(CongruenceCount CC in this.ContentCountTables(SourceType.ToString(), "public"))
                {
                    //string SQL = "SELECT \"SourceView\", Count(*) FROM public.\"" + CC.SourceName + "\" GROUP BY \"SourceView\";";
                    //Npgsql.NpgsqlDataAdapter adNew = new Npgsql.NpgsqlDataAdapter(SQL, conNew);
                    //adNew.Fill(dtNew);
                    dtNew = this.GetSourceCounts(CC.SourceName, ConnectionStringNew, "", "SourceView");
                    foreach(System.Data.DataRow R in dtNew.Rows)
                    {
                        string SourceView = R[0].ToString();
                        if (!DictSourceViews.ContainsKey(SourceView))
                        {
                            System.Collections.Generic.Dictionary<string, CongruenceCount> dd = new Dictionary<string, CongruenceCount>();
                            CongruenceCount cc = new CongruenceCount(CC.SourceName);
                            cc.CountNew = int.Parse(R[1].ToString());
                            dd.Add(CC.SourceName, cc);
                            DictSourceViews.Add(SourceView, dd);
                        }
                        else
                        {
                            if (!DictSourceViews[SourceView].ContainsKey(CC.SourceName))
                            {
                                CongruenceCount cc = new CongruenceCount(CC.SourceName);
                                cc.CountNew = int.Parse(R[1].ToString());
                                DictSourceViews[SourceView].Add(CC.SourceName, cc);
                            }
                            else
                            {
                                CongruenceCount cc = DictSourceViews[SourceView][CC.SourceName];
                                cc.CountNew = int.Parse(R[1].ToString());
                                DictSourceViews[SourceView][CC.SourceName] = cc;
                            }
                        }
                    }
                    //Npgsql.NpgsqlDataAdapter adOld = new Npgsql.NpgsqlDataAdapter(SQL, conOld);
                    //adOld.Fill(dtOld);
                    dtOld = this.GetSourceCounts(CC.SourceName, ConnectionStringOld, "", "SourceView");
                    foreach (System.Data.DataRow R in dtOld.Rows)
                    {
                        string SourceView = R[0].ToString();
                        if (!DictSourceViews.ContainsKey(SourceView))
                        {
                            System.Collections.Generic.Dictionary<string, CongruenceCount> dd = new Dictionary<string, CongruenceCount>();
                            CongruenceCount cc = new CongruenceCount(CC.SourceName);
                            cc.CountOld = int.Parse(R[1].ToString());
                            dd.Add(CC.SourceName, cc);
                            DictSourceViews.Add(SourceView, dd);
                        }
                        else
                        {
                            if (!DictSourceViews[SourceView].ContainsKey(CC.SourceName))
                            {
                                CongruenceCount cc = new CongruenceCount(CC.SourceName);
                                cc.CountOld = int.Parse(R[1].ToString());
                                DictSourceViews[SourceView].Add(CC.SourceName, cc);
                            }
                            else
                            {
                                CongruenceCount cc = DictSourceViews[SourceView][CC.SourceName];
                                cc.CountOld = int.Parse(R[1].ToString());
                                DictSourceViews[SourceView][CC.SourceName] = cc;
                            }
                        }
                    }
                }
                string CountObject = this.ContentCountObject(SourceType.ToString());
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                //conNew.Close();
                //conNew.Dispose();
                //conOld.Close();
                //conOld.Dispose();
            }
            return DictSourceViews;
        }


        #region Old version

        private bool SourcesAreCongruent()
        {
            bool Congruent = false;
            try
            {
                bool AgentOK = this.AgentsAreCongruent();
                bool GazetteerOK = this.GazetteerAreCongruent();
                bool ReferencesOK = this.ReferencesAreCongruent();
                bool TaxaOK = this.TaxaAreCongruent();
                bool TermsOK = this.TermsAreCongruent();
                if (AgentOK && GazetteerOK && ReferencesOK && TaxaOK && TermsOK)
                    Congruent = true;
            }
            catch (System.Exception ex)
            {
                Congruent = false;
            }
            return Congruent;
        }

        private bool AgentsAreCongruent()
        {
            string SQL = "SELECT  distinct \"SourceView\" FROM public.\"Agent\";";
            bool Congruent = this.SourcesAreCongruent(SQL, UserControlLookupSource.TypeOfSource.Agents, this.tabPageAgents); ;
            return Congruent;
        }

        private bool GazetteerAreCongruent()
        {
            string SQL = "SELECT  distinct \"SourceView\" FROM public.\"Gazetteer\";";
            bool Congruent = this.SourcesAreCongruent(SQL, UserControlLookupSource.TypeOfSource.Gazetteer, this.tabPageGazetteer); ;
            return Congruent;
        }

        private bool ReferencesAreCongruent()
        {
            string SQL = "SELECT  distinct \"SourceView\" FROM public.\"ReferenceTitle\";";
            bool Congruent = this.SourcesAreCongruent(SQL, UserControlLookupSource.TypeOfSource.References, this.tabPageReferences); ;
            return Congruent;
        }

        private bool TaxaAreCongruent()
        {
            string SQL = "SELECT  distinct \"SourceView\" FROM public.\"TaxonSynonymy\";";
            bool Congruent = this.SourcesAreCongruent(SQL, UserControlLookupSource.TypeOfSource.Taxa, this.tabPageTaxa); ;
            return Congruent;
        }

        private bool TermsAreCongruent()
        {
            string SQL = "SELECT  distinct \"SourceView\" FROM public.\"ScientificTerm\";";
            bool Congruent = this.SourcesAreCongruent(SQL, UserControlLookupSource.TypeOfSource.ScientificTerms, this.tabPageTerms); ;
            return Congruent;
        }

        #endregion

        #endregion

        #region Webservices

        private void CongruenceOfWebservices()
        {
            System.Collections.Generic.Dictionary<UserControlLookupSource.TypeOfSource, System.Windows.Forms.TabPage> SourceIssues = new Dictionary<UserControlLookupSource.TypeOfSource, TabPage>();
            SourceIssues.Add(UserControlLookupSource.TypeOfSource.Taxa, this.tabPageTaxaWebservice);

            ReplaceDatabase.CongruenceStatus status = this.CongruenceOfSources(SourceIssues, true);
            //tabPageTaxaWebservice.ImageIndex = this._replaceDatabase.ImageIndexCongruenceStatus(status);
            this.tabPageWebservices.ImageIndex = this._replaceDatabase.ImageIndexCongruenceStatus(status);
        }


        #endregion

        #endregion

        #region Replacement

        private void comboBoxExchangedDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this.ResetCongruence();

                this._OldReplacedDatabase = this.comboBoxExchangedDatabase.SelectedValue.ToString();
                this.buttonExchange.Text = "Rename current database\r\n   \t" + this._NewReplacementDatabase +
                    "\t   to   \t" + _OldReplacedDatabase +
                    "\r\nand delete old version of\r\n   \t" + _OldReplacedDatabase;

                this.CongruenceOfProjects();
                //this.ProjectsAreCongruent();

                // Sources
                //this.SourcesAreCongruent();
                this.CongruenceOfSources();

                this.CongruenceOfWebservices();

                if (this.IncongruenceIssues.Length > 0)
                    System.Windows.Forms.MessageBox.Show(this.IncongruenceIssues, "Databases incongruent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.buttonExchange.Enabled = this.IsCongruent;// .DatabaseIsCongruent();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonEnableIncongruentExchange_Click(object sender, EventArgs e)
        {
            if (this._OldReplacedDatabase == null || this._OldReplacedDatabase.Length == 0)
                this.comboBoxExchangedDatabase_SelectionChangeCommitted(null, null);
            this.buttonExchange.Enabled = true;
        }

        private void buttonExchange_Click(object sender, EventArgs e)
        {
            string Message = "";
            bool OK = false;
            try
            {
                // Rename the old database that should be replaced to ..._OLD
                string SQL = "SELECT pg_terminate_backend( pid ) " +
                    "FROM pg_stat_activity " +
                    "WHERE pid <> pg_backend_pid( ) " +
                    "AND datname = '" + _OldReplacedDatabase + "'; " +
                    "ALTER DATABASE \"" + _OldReplacedDatabase + "\" RENAME TO \"" + _OldReplacedDatabase + "_OLD\"; ";
                if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                {
                    if (Message.Length == 0)
                    {
                        DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                        // Change to the renamed database
                        if (DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(_OldReplacedDatabase + "_OLD"))
                        {
                            // Rename the new database that should be the replacement to the name of the old replaced database
                            SQL = "SELECT pg_terminate_backend( pid ) " +
                                "FROM pg_stat_activity " +
                                "WHERE pid <> pg_backend_pid( ) " +
                                "AND datname = '" + this._NewReplacementDatabase + "'; " +
                                "ALTER DATABASE \"" + this._NewReplacementDatabase + "\" RENAME TO \"" + _OldReplacedDatabase + "\";";
                            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                            {
                                if (Message.Length == 0)
                                {
                                    // Change to the replacement database
                                    if (DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(_OldReplacedDatabase))
                                    {
                                        DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                                        // Drop the old database
                                        SQL = "SELECT pg_terminate_backend( pid ) " +
                                            "FROM pg_stat_activity " +
                                            "WHERE pid <> pg_backend_pid( ) " +
                                            "AND datname = '" + this._OldReplacedDatabase + "_OLD'; ";
                                        if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                                        {
                                            SQL = "DROP DATABASE \"" + _OldReplacedDatabase + "_OLD\";";
                                            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false))
                                            {
                                                this.buttonExchange.Enabled = false;
                                                this.comboBoxExchangedDatabase.Enabled = false;
                                                _DatabaseHasBeenReplaced = true;
                                                System.Windows.Forms.MessageBox.Show("Replacement has been successful");
                                                OK = true;
                                                this.Close();
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
                OK = false;
            }
            if (!OK)
            {
                System.Windows.Forms.MessageBox.Show("Replacement failed: " + Message);
            }
        }

        public bool DatabaseHasBeenReplaced() { return _DatabaseHasBeenReplaced; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        #endregion

        #region Auxillary

        private void setSourceCounts(string SourceName, System.Data.DataTable dtSouce, 
            ref System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, CongruenceCount>> DictSources, 
            string GroupColumn = "", bool ForOld = false)
        {
            foreach (System.Data.DataRow R in dtSouce.Rows)
            {
                string SourceView = R[0].ToString();
                if (!DictSources.ContainsKey(SourceView))
                {
                    System.Collections.Generic.Dictionary<string, CongruenceCount> dd = new Dictionary<string, CongruenceCount>();
                    CongruenceCount cc = new CongruenceCount(SourceName);
                    if (ForOld)
                        cc.CountOld = int.Parse(R[1].ToString());
                    else
                        cc.CountNew = int.Parse(R[1].ToString());
                    dd.Add(SourceName, cc);
                    DictSources.Add(SourceView, dd);
                }
                else
                {
                    if (!DictSources[SourceView].ContainsKey(SourceName))
                    {
                        CongruenceCount cc = new CongruenceCount(SourceName);
                        if (ForOld)
                            cc.CountOld = int.Parse(R[1].ToString());
                        else
                            cc.CountNew = int.Parse(R[1].ToString());
                        DictSources[SourceView].Add(SourceName, cc);
                    }
                    else
                    {
                        CongruenceCount cc = DictSources[SourceView][SourceName];
                        if (ForOld)
                            cc.CountOld = int.Parse(R[1].ToString());
                        else
                            cc.CountNew = int.Parse(R[1].ToString());
                        DictSources[SourceView][SourceName] = cc;
                    }
                }
            }
        }

        private System.Data.DataTable GetSourceCounts(string Source, string ConnectionString, string Schema = "", string GroupByColumn = "")
        {
            System.Data.DataTable dt = new DataTable();
            if (Schema.Length > 0)
                Schema = "\"" + Schema + "\"";
            else
                Schema = "public";
            string SQL = "SELECT Count(*) FROM " + Schema + ".\"" + Source + "\";";
            if (GroupByColumn.Length > 0)
                SQL = "SELECT Count(*), \"" + GroupByColumn + "\" FROM " + Schema + ".\"" + Source + "\" GROUP BY \"" + GroupByColumn + "\";";

            try
            {
                Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
                Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, con);
                con.Open();
                ad.Fill(dt);
                con.Close();
            }
            catch (Exception e)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(e);
            }
            return dt;
        }

        private bool SourcesAreCongruent(string SQL, UserControlLookupSource.TypeOfSource SourceType, System.Windows.Forms.TabPage SourceTabPage)
        {
            try
            {
                SourceTabPage.Controls.Clear();
                System.Collections.Generic.List<string> SourceOldReplaced = this.SourceListOldReplaced(SQL);
                System.Collections.Generic.List<string> SourceNewReplacement = this.SourceListNewReplacement(SQL);
                ReplaceDatabase.CongruenceStatus Status = ReplaceDatabase.CongruenceStatus.Congruent;
                foreach (string S in SourceOldReplaced)
                {
                    if (!SourceNewReplacement.Contains(S))
                    {
                        Status = ReplaceDatabase.CongruenceStatus.Missing;
                        IncongruenceIssues = S + " " + Status.ToString();
                        //Congruent = false;
                    }
                    DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(SourceType, S, Status);
                    SourceTabPage.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
                foreach (string S in SourceNewReplacement)
                {
                    if (!SourceOldReplaced.Contains(S))
                    {
                        Status = ReplaceDatabase.CongruenceStatus.Added;
                        DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(SourceType, S, Status);
                        SourceTabPage.Controls.Add(U);
                        U.Dock = DockStyle.Top;
                        U.BringToFront();
                    }
                }
            }
            catch (System.Exception ex)
            {
                IncongruenceIssues = ex.Message;
                //Congruent = false;
            }

            bool Congruent = true;
            return Congruent;
        }

        private System.Collections.Generic.List<string> SourceListOldReplaced(string SQL)
        {
            return this.SourceList(SQL, this._OldReplacedDatabase);
        }

        private System.Collections.Generic.List<string> SourceListNewReplacement(string SQL)
        {
            return this.SourceList(SQL, this._NewReplacementDatabase);
        }

        private System.Collections.Generic.List<string> SourceList(string SQL, string DatabaseName)
        {
            System.Data.DataTable dt = new DataTable();
            try
            {
                string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DatabaseName);
                Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
                con.Open();
                Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, con);
                Npgsql.NpgsqlCommand C = new Npgsql.NpgsqlCommand("", con);
                ad.Fill(dt);
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            System.Collections.Generic.List<string> L = new List<string>();
            foreach(System.Data.DataRow R in dt.Rows)
                L.Add(R[0].ToString());
            return L;
        }

        private void GetContentCount(ref int CountOld, ref int CountNew, string Schema, string Source)
        {
            string ContentSource = this.ContentCountObject(Source);
            if (ContentSource.Length > 0)
            {
                string SQL = "select count(*) from \"" + Schema + "\".\"" + ContentSource + "\"";
                string ResultOld = this.SqlResultFromPostgresCacheDB(SQL, this._OldReplacedDatabase);
                string ResultNew = this.SqlResultFromPostgresCacheDB(SQL, this._NewReplacementDatabase);
                int.TryParse(ResultOld, out CountOld);
                int.TryParse(ResultNew, out CountNew);
            }
            else
            {
                CountNew = 0;
                CountOld = 0;
            }
        }


        private void GetContentCount(ref CongruenceCount CC, string Schema, string Restriction = "")
        {
            string SQL = "select count(*) from \"" + Schema + "\".\"" + CC.SourceName + "\" ";
            if (Restriction.Length > 0)
                SQL += " WHERE " + Restriction;
            SQL += ";";
            string ResultOld = this.SqlResultFromPostgresCacheDB(SQL, this._OldReplacedDatabase);
            string ResultNew = this.SqlResultFromPostgresCacheDB(SQL, this._NewReplacementDatabase);
            int Old = 0;
            int New = 0;
            if (int.TryParse(ResultOld, out Old))
                CC.CountOld = Old;
            if (int.TryParse(ResultNew, out New))
                CC.CountNew = New;
            if (CC.CountOld == null && CC.CountNew != null)
                CC.Status = ReplaceDatabase.CongruenceStatus.Added;
            else if (CC.CountOld != null && CC.CountNew == null)
                CC.Status = ReplaceDatabase.CongruenceStatus.Missing;
            else if (CC.CountOld > 0 && CC.CountNew == 0)
                CC.Status = ReplaceDatabase.CongruenceStatus.DataAreMissing;
            else if (CC.CountOld != CC.CountNew)
                CC.Status = ReplaceDatabase.CongruenceStatus.DataCountDifferent;
        }

        private int GetContentCount(string Source, string ConnectionString, string Schema = "", string Restriction = "")
        {
            int Count = 0;
            if (Schema.Length > 0)
                Schema = "\"" + Schema + "\"";
            else
                Schema = "public";
            string SQL = "SELECT Count(*) FROM " + Schema + ".\"" + Source + "\";";
            if (Restriction.Length > 0)
                SQL += " WHERE " + Restriction + ";";

            try
            {
                Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
                Npgsql.NpgsqlCommand C = new Npgsql.NpgsqlCommand(SQL, con);
                con.Open();
                string Result = C.ExecuteScalar()?.ToString();
                int.TryParse(Result, out Count);
                con.Close();
            }
            catch (Exception e)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(e);
            }
            return Count;
        }


        private CongruenceCount ContentCount(string SourceName, string Schema, string Restriction = "")
        {
            string SQL = "select count(*) from \"" + Schema + "\".\"" + SourceName + "\" ";
            if (Restriction.Length > 0)
                SQL += " WHERE " + Restriction;
            SQL += ";";
            string ResultOld = this.SqlResultFromPostgresCacheDB(SQL, this._OldReplacedDatabase, true);
            string ResultNew = this.SqlResultFromPostgresCacheDB(SQL, this._NewReplacementDatabase, true);
            int? Old = null;
            int? New = null;
            int iOld = 0;
            int iNew = 0;
            if (int.TryParse(ResultOld, out iOld))
                Old = iOld;
            if (int.TryParse(ResultNew, out iNew))
                New = iNew;
            CongruenceCount CC = new CongruenceCount(SourceName, Old, New);
            return CC;
        }





        private bool CompareContentCount(ref int CountOld, ref int CountNew, string DatabaseOld, string DatabaseNew, string Schema, string Source)
        {
            bool OK = false;
            string ContentSource = this.ContentCountObject(Source);
            if (ContentSource.Length > 0)
            {
                string SQL = "select count(*) from \"" + Schema + "\".\"" + ContentSource + "\"";
                string ResultOld = this.SqlResultFromPostgresCacheDB(SQL, DatabaseOld);
                string ResultNew = this.SqlResultFromPostgresCacheDB(SQL, DatabaseNew);
                if (int.TryParse(ResultOld, out CountOld) &&
                    int.TryParse(ResultNew, out CountNew) &&
                    CountNew == CountOld)
                    OK = true;
            }
            else
            {
                OK = false;
                CountNew = 0;
                CountOld = 0;
            }
            return OK;
        }

        private System.Collections.Generic.Dictionary<string, string> _ContentCountObjects;
        private string ContentCountObject(string Source)
        {
            if (this._ContentCountObjects == null)
            {
                this._ContentCountObjects = new Dictionary<string, string>();
                this._ContentCountObjects.Add("ABCD", "ABCD_Unit");
                this._ContentCountObjects.Add("ABCD - ABCD_BayernFlora", "ABCD__BayernFlora_EndangeredSpeciesBase");
                this._ContentCountObjects.Add("FloraRaster", "FloraRaster_Sippendaten");
                this._ContentCountObjects.Add("Project", "CacheCollectionSpecimen");
                this._ContentCountObjects.Add(UserControlLookupSource.TypeOfSource.Agents.ToString(), "Agent");
                this._ContentCountObjects.Add(UserControlLookupSource.TypeOfSource.Gazetteer.ToString(), "Gazetteer");
                this._ContentCountObjects.Add(UserControlLookupSource.TypeOfSource.Plots.ToString(), "SamplingPlot");
                this._ContentCountObjects.Add(UserControlLookupSource.TypeOfSource.References.ToString(), "ReferenceTitle");
                this._ContentCountObjects.Add(UserControlLookupSource.TypeOfSource.ScientificTerms.ToString(), "ScientificTerm");
                this._ContentCountObjects.Add(UserControlLookupSource.TypeOfSource.Taxa.ToString(), "TaxonSynonymy");
            }
            if (this._ContentCountObjects.ContainsKey(Source))
                return this._ContentCountObjects[Source];
            else return "";
        }

        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<CongruenceCount>> _ContentCountTables;
        private System.Collections.Generic.List<CongruenceCount> ContentCountTables(string Source, string Schema)
        {
            if (this._ContentCountTables == null)
            {
                this._ContentCountTables = new Dictionary<string, List<CongruenceCount>>();

                // ABCD
                List<CongruenceCount> ABCD = this.ContentCountTableList("public", "ABCD_%"); 
                this._ContentCountTables.Add("ABCD", ABCD);

                //ABCD_BayernFlora
                List<CongruenceCount> ABCD_BayernFlora = this.ContentCountTableList(Schema, "ABCD__BayernFlora_%");
                this._ContentCountTables.Add("ABCD_BayernFlora", ABCD_BayernFlora);

                //FloraRaster
                List<CongruenceCount> FloraRaster = this.ContentCountTableList(Schema, "FloraRaster_%");
                this._ContentCountTables.Add("FloraRaster", FloraRaster);

                //Agent
                List<CongruenceCount> Agents = new List<CongruenceCount>();
                CongruenceCount Agent = new CongruenceCount("Agent");
                Agents.Add(Agent);
                CongruenceCount AgentContactInformation = new CongruenceCount("AgentContactInformation");
                Agents.Add(AgentContactInformation);
                this._ContentCountTables.Add(UserControlLookupSource.TypeOfSource.Agents.ToString(), Agents);

                //Gazetteer
                List<CongruenceCount> Gazetteers = new List<CongruenceCount>();
                CongruenceCount Gazetteer = new CongruenceCount("Gazetteer");
                Gazetteers.Add(Gazetteer);
                this._ContentCountTables.Add(UserControlLookupSource.TypeOfSource.Gazetteer.ToString(), Gazetteers);

                //SamplingPlot
                List<CongruenceCount> Plots = new List<CongruenceCount>();
                CongruenceCount SamplingPlot = new CongruenceCount("SamplingPlot");
                Plots.Add(SamplingPlot);
                CongruenceCount SamplingPlotLocalisation = new CongruenceCount("SamplingPlotLocalisation");
                Plots.Add(SamplingPlotLocalisation);
                CongruenceCount SamplingPlotProperty = new CongruenceCount("SamplingPlotProperty");
                Plots.Add(SamplingPlotProperty);
                this._ContentCountTables.Add(UserControlLookupSource.TypeOfSource.Plots.ToString(), Plots);

                //References
                List<CongruenceCount> References = new List<CongruenceCount>();
                CongruenceCount ReferenceTitle = new CongruenceCount("ReferenceTitle");
                References.Add(ReferenceTitle);
                this._ContentCountTables.Add(UserControlLookupSource.TypeOfSource.References.ToString(), References);

                //ScientificTerm
                List<CongruenceCount> ScientificTerms = new List<CongruenceCount>();
                CongruenceCount ScientificTerm = new CongruenceCount("ScientificTerm");
                ScientificTerms.Add(ScientificTerm);
                this._ContentCountTables.Add(UserControlLookupSource.TypeOfSource.ScientificTerms.ToString(), ScientificTerms);

                //Taxa
                List<CongruenceCount> Taxa = new List<CongruenceCount>();
                CongruenceCount TaxonSynonymy = new CongruenceCount("TaxonSynonymy");
                Taxa.Add(TaxonSynonymy);
                CongruenceCount TaxonAnalysis = new CongruenceCount("TaxonAnalysis");
                Taxa.Add(TaxonAnalysis);
                CongruenceCount TaxonAnalysisCategory = new CongruenceCount("TaxonAnalysisCategory");
                Taxa.Add(TaxonAnalysisCategory);
                CongruenceCount TaxonAnalysisCategoryValue = new CongruenceCount("TaxonAnalysisCategoryValue");
                Taxa.Add(TaxonAnalysisCategoryValue);
                CongruenceCount TaxonList = new CongruenceCount("TaxonList");
                Taxa.Add(TaxonList);
                CongruenceCount TaxonCommonName = new CongruenceCount("TaxonCommonName");
                Taxa.Add(TaxonCommonName);
                CongruenceCount TaxonNameExternalID = new CongruenceCount("TaxonNameExternalID");
                Taxa.Add(TaxonNameExternalID);
                this._ContentCountTables.Add(UserControlLookupSource.TypeOfSource.Taxa.ToString(), Taxa);
            }
            if (!_ContentCountTables.ContainsKey(Source))
            {

            }
            else if (_ContentCountTables[Source].Count == 0)
            {
                List<CongruenceCount> List = this.ContentCountTableList(Schema, Source.Replace("_", "%") + "%");
                if (List.Count > 0)
                    this._ContentCountTables[Source] = List;
            }
            return _ContentCountTables[Source];
        }

        private System.Collections.Generic.List<CongruenceCount> ContentCountTableList(string Schema, string Restriction)
        {
            System.Collections.Generic.List<CongruenceCount> List = new List<CongruenceCount>();
            string SQL = "select t.table_name from information_schema.Tables T " +
                "where t.table_schema = '" + Schema + "' " +
                "and t.table_name like '" + Restriction + "'  " +
                "order by t.table_name";
            System.Data.DataTable dtOld = this.SqlGetPostgresTable(SQL, this._OldReplacedDatabase);
            foreach(System.Data.DataRow R in dtOld.Rows)
            {
                CongruenceCount CO = new CongruenceCount(R[0].ToString());
                List.Add(CO);
            }
            return List;
        }


        //private System.Data.DataTable ReplacementTable(string SQL)
        //{
        //    return this.DataTable(SQL, this._ReplacementDatabase);
        //}

        //private System.Data.DataTable DataTable(string SQL, string DatabaseName)
        //{
        //    System.Data.DataTable dt = new DataTable();
        //    try
        //    {
        //        string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DatabaseName);
        //        Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
        //        con.Open();
        //        Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, con);
        //        Npgsql.NpgsqlCommand C = new Npgsql.NpgsqlCommand("", con);
        //        ad.Fill(dt);
        //        con.Close();
        //        con.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return dt;
        //}

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
