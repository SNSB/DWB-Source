using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.CacheDB.ReplaceDB
{

    public struct ReplaceCount
    {
        public string SourceName;
        public int? CountOld;
        public int? CountNew;
        public Replacement.Status Status;
        public ReplaceCount(string Source)
        {
            SourceName = Source;
            CountNew = null;
            CountOld = null;
            Status = Replacement.Status.Congruent;
        }

        public ReplaceCount(string Source, int? Old, int? New)
        {
            SourceName = Source;
            CountNew = New;
            CountOld = Old;
            if (Old == null && New == null)
            {
                Status = Replacement.Status._None;
            }
            else if (Old != null && New == null)
            {
                Status = Replacement.Status.Missing;
            }
            else if (Old == null && New != null)
            {
                Status = Replacement.Status.Added;
            }
            else
            {
                if (Old == New)
                    Status = Replacement.Status.Congruent;
                else if (Old == 0 && New > 0)
                    Status = Replacement.Status.Added;
                else if (Old > 0 && New == 0)
                    Status = Replacement.Status.DataAreMissing;
                else
                    Status = Replacement.Status.DataCountDifferent;
            }
        }

    }


    public class ReplacedPart
    {
        #region Parameter

        private string _Name;
        public string Name
        {
            get
            {
                if (this._Name != null && this._Name.Length > 0)
                    return this._Name;
                else
                {
                    switch(this._Type)
                    {
                        case Replacement.Type.Project:
                            this._Name = this._Schema.Substring(("Project_").Length);
                            break;
                        default:
                            this._Name = "";
                            break;
                    }
                    return this._Name;
                }
            }
        }

        private Replacement.Status _Status = Replacement.Status._None;

        public Replacement.Status Status()
        {
            if (this._Status == Replacement.Status._None)
            {
                try
                {
                    switch (this._Type)
                    {
                        case Replacement.Type.Project:
                            System.Collections.Generic.SortedDictionary<string, ReplaceCount> Projects = this.ProjectReplacementCounts();
                            foreach (System.Collections.Generic.KeyValuePair<string, ReplaceCount> KV in Projects)// this.ProjectReplacementCounts())
                            {
                                this._Status = this._Replacement.StatusWorst(this._Status, KV.Value.Status);
                            }
                            if (this.ContainedParts != null && this.ContainedParts.Count > 0)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, ReplacedPart> KV in this.ContainedParts)
                                {
                                    this._Status = this._Replacement.StatusWorst(this._Status, KV.Value.Status());
                                }
                            }
                            break;
                        default:
                            foreach (System.Collections.Generic.KeyValuePair<string, ReplaceCount> KV in this.ReplacementCounts())
                            {
                                this._Status = this._Replacement.StatusWorst(this._Status, KV.Value.Status);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return this._Status;
        }

        private Replacement.Type _Type;

        public Replacement.Type Type { get { return this._Type; } }

        public string Message;

        private Replacement _Replacement;
        private bool _InPublic;
        private string _Schema;
        private string _Restriction;
        private string _MainTable;

        public string MainTable
        {
            get
            {
                if (this._MainTable == null || this._MainTable.Length == 0)
                    return this._Type.ToString();
                else
                    return _MainTable;
            }
        }

        #endregion

        #region Construction

        public ReplacedPart(ref Replacement replacement, Replacement.Type T, string Restriction = "", string MainTable = "", bool InPublic = true)
        {
            this._Replacement = replacement;
            _Type = T;
            _Status = Replacement.Status._None;
            _VersionOld = 0;
            _VersionNew = 0;
            if (Restriction.Length > 0)
                _Restriction = Restriction;
            else
            {
                if (this._Type == Replacement.Type.Taxon_Webservice)
                    _Restriction = "Taxon%";
                else if (this._Type == Replacement.Type.Reference_Webservice)
                    _Restriction = "Reference%";
                else
                    _Restriction = _Type.ToString() + "%";

            }
            if (MainTable.Length > 0)
                _MainTable = MainTable;
            else
                _MainTable = T.ToString();
            this._InPublic = InPublic;
        }


        //public ReplacedPart(Replacement replacement, string Restriction, Replacement.Type T, bool InPublic = true)
        //{
        //    this._Replacement = replacement;
        //    Type = T;
        //    Status = Replacement.Status._None;
        //    VersionOld = 0;
        //    VersionNew = 0;
        //    _Restriction = Restriction;
        //    this._InPublic = InPublic;
        //}


        //public ReplacedPart(Replacement replacement, string Schema, string Restriction, Replacement.Status S, Replacement.Type T)
        //{
        //    this._Replacement = replacement;
        //    Type = T;
        //    Status = S;
        //    VersionOld = 0;
        //    VersionNew = 0;
        //    _Schema = Schema;
        //    _Restriction = Restriction;
        //    //ReplaceCounts = new List<ReplaceCount>();
        //    Message = "";
        //    ContainedParts = new Dictionary<string, ReplacedPart>();
        //}

        public ReplacedPart(Replacement replacement, string Schema, Replacement.Type T, string Name = "")
        {
            this._Replacement = replacement;
            _Type = T;
            _Schema = Schema;
            if (Name.Length > 0)
                this._Name = Name;

            //_VersionOld = 0;
            //_VersionNew = 0;
            //Status = Replacement.Status.Added;
            //ReplaceCounts = Counts;
            //foreach (ReplaceCount CC in ReplaceCounts)
            //{
            //    if (CC.Status > Status)
            //        Status = CC.Status;
            //}
            //Message = "";
            //ContainedParts = new Dictionary<string, ReplacedPart>();
        }



        //public ReplacedPart(Replacement replacement, string Name, string Schema, string Restriction, Replacement.Type T, System.Collections.Generic.List<ReplaceCount> Counts)
        //{
        //    this._Replacement = replacement;
        //    _Name = Name;
        //    _Type = T;
        //    _VersionOld = 0;
        //    _VersionNew = 0;
        //    Status = Replacement.Status.Added;
        //    _Schema = Schema;
        //    _Restriction = Restriction;
        //    //ReplaceCounts = Counts;
        //    //foreach (ReplaceCount CC in ReplaceCounts)
        //    //{
        //    //    if (CC.Status > Status)
        //    //        Status = CC.Status;
        //    //}
        //    Message = "";
        //    ContainedParts = new Dictionary<string, ReplacedPart>();
        //}

        #endregion

        #region Version

        private int? _VersionOld;
        public int? VersionNew { get { return this._VersionNew; } set { this._VersionNew = value; } }
        private int? _VersionNew;
        public int? VersionOld
        {
            get
            {
                if (this._VersionOld == null)
                {
                    switch (this._Type)
                    {
                        case Replacement.Type.Project:
                            string SQL = "";
                            break;
                    }
                }
                return this._VersionOld;
            }
            set
            {
                if (this._VersionOld == null)
                    this._VersionOld = value;
            }
        }

        private void initVersions()
        {
            switch (this._Type)
            {
                case Replacement.Type.Project:
                    this.setVersionsForProject();
                    break;

            }
        }

        private void setVersionsForProject()
        {
            //int VersionOld
        }

        private int? VersionOfProject(string Database)
        {
            int? Version = null;
            return Version;
        }

        #endregion

        //public System.Collections.Generic.Dictionary

        #region Counts

        private System.Collections.Generic.List<string> _CountTables;
        private System.Collections.Generic.SortedDictionary<string, ReplaceCount> _Counts;

        public System.Collections.Generic.SortedDictionary<string, ReplaceCount> Counts()
        {
            if (this._Counts == null )
            {
                switch(this._Type)
                {
                    case Replacement.Type.Project:
                        this._Counts = this.ProjectReplacementCounts();
                        break;
                    //case Replacement.Type.Project_Package:
                    //    break;
                    default:
                        this._Counts = this.ReplacementCounts();
                        //_Counts = new SortedDictionary<string, ReplaceCount>();
                        break;
                }
            }
            return this._Counts;
        }

        //public System.Collections.Generic.List<string> SourceViews()
        //{
        //    System.Collections.Generic.List<string> L = new List<string>();
        //    string SQL = "SELECT DISTINCT \"SourceView\" FROM \"" + this.MainTable;
        //    switch (this.Type)
        //    {
        //        case Replacement.Type.Taxon:
        //            SQL += " WHERE \"NameID\" > -1 ";
        //            break;
        //        case Replacement.Type.Taxon_Webservice:
        //            SQL += " WHERE \"NameID\" = -1 ";
        //            break;
        //    }
        //    SQL += "\" ORDER BY \"SourceView\";";
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    string Message = "";
        //    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
        //    foreach(System.Data.DataRow R in dt.Rows)
        //    {
        //        L.Add(R[0].ToString());
        //    }
        //    SQL += ";";
        //    return L; 
        //}

        private System.Collections.Generic.List<string> CountTables()
        {
            if (this._CountTables == null)
            {
                this._CountTables = new List<string>();
                if (this._Restriction == null)
                {
                    switch(this._Type)
                    {
                        case Replacement.Type.Project_Package:
                            if (this._Replacement.Sources().ContainsKey(Replacement.Type.Project))
                            {
                                if (this._Replacement.Sources()[Replacement.Type.Project].ContainedParts.ContainsKey(this.Name))
                                {
                                    this._Restriction = this._Replacement.Sources()[Replacement.Type.Project].ContainedParts[this.Name]._Restriction;
                                }
                            }
                            break;
                        default:
                            if (this._Replacement.Sources().ContainsKey(this.Type))
                            {
                                this._Restriction = this._Replacement.Sources()[this.Type]._Restriction;
                            }
                            else
                                this._Restriction = "%";
                            break;
                    }
                }
                string SQL = "select t.table_name from information_schema.Tables T " +
                    "where t.table_schema = '" + _Schema + "' " +
                    "and t.table_name like '" + _Restriction + "'  " +
                    "order by t.table_name";
                System.Data.DataTable dtOld = this.SqlGetPostgresTable(SQL, this._Replacement.OldDatabase);
                foreach (System.Data.DataRow R in dtOld.Rows)
                {
                    this._CountTables.Add(R[0].ToString());
                }
            }
            return this._CountTables;
        }

        private System.Collections.Generic.SortedDictionary<string, ReplaceCount> ProjectReplacementCounts()
        {
            System.Collections.Generic.SortedDictionary<string, ReplaceCount> Counts = new SortedDictionary<string, ReplaceCount>();
            try
            {
                string SQL = "SELECT \"Tablename\", \"TotalCount\" FROM \"" + _Schema + "\".\"CacheCount\"";
                string MessageOld = "";
                string MessageNew = "";
                System.Data.DataTable dtOld = new System.Data.DataTable();
                System.Data.DataTable dtNew = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtOld, ref MessageOld, this._Replacement.OldDatabase);
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtNew, ref MessageNew, this._Replacement.NewDatabase);
                foreach (System.Data.DataRow R in dtOld.Rows)
                {
                    int? Old = null;
                    int? New = null;
                    int iOld = 0;
                    int iNew = 0;
                    if (int.TryParse(R[1].ToString(), out iOld))
                        Old = iOld;
                    if (dtNew != null && dtNew.Rows.Count > 0)
                    {
                        System.Data.DataRow[] rr = dtNew.Select("Tablename = '" + R[0].ToString() + "'");
                        if (rr.Length == 1)
                        {
                            if (int.TryParse(rr[0][1].ToString(), out iNew))
                                New = iNew;
                        }
                    }
                    ReplaceCount CO = new ReplaceCount(R[0].ToString(), Old, New);
                    Counts.Add(R[0].ToString(), CO);
                }
                foreach (System.Data.DataRow R in dtNew.Rows)
                {
                    int? New = null;
                    int iNew = 0;
                    if (int.TryParse(R[1].ToString(), out iNew))
                        New = iNew;
                    System.Data.DataRow[] rr = dtOld.Select("Tablename = '" + R[0].ToString() + "'");
                    if (rr.Length == 0)
                    {
                        ReplaceCount CO = new ReplaceCount(R[0].ToString(), null, New);
                        if (Counts.ContainsKey(R[0].ToString()))
                        {
                            Counts[R[0].ToString()] = CO;
                        }
                        else
                        {
                            Counts.Add(R[0].ToString(), CO);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Counts;
        }

        private System.Collections.Generic.SortedDictionary<string, ReplaceCount> ReplacementCounts(string Restriction = "")
        {
            System.Collections.Generic.SortedDictionary<string, ReplaceCount> Counts = new SortedDictionary<string, ReplaceCount>();
            try
            {
                System.Collections.Generic.List<string> Tables = this.CountTables();
                foreach (string T in Tables)
                {
                    string Message = "";
                    string SQL = "SELECT COUNT(*) FROM \"" + _Schema + "\".\"" + T + "\" WHERE 1 = 1 ";
                    if (Restriction.Length > 0)
                        SQL += Restriction;
                    else if (this._Name.Length > 0)
                    {
                        switch(this._Type)
                        {
                            case Replacement.Type.Project:
                            case Replacement.Type.Project_Package:
                                break;
                            case Replacement.Type.Taxon:
                                SQL += " AND \"NameID\" > -1 ";
                                goto default;
                            case Replacement.Type.Taxon_Webservice:
                                SQL += " AND \"NameID\" = -1 ";
                                goto default;
                            default:
                                SQL += " AND \"SourceView\" = '" + this._Name + "'; ";
                                break;
                        }
                    }
                    string ResultOld = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.DatabaseConnectionString(this._Replacement.OldDatabase), ref Message);
                    string ResultNew = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.DatabaseConnectionString(this._Replacement.NewDatabase), ref Message);
                    int? iOld = null;
                    int? iNew = null;
                    int i;
                    if (int.TryParse(ResultOld, out i)) iOld = i;
                    if (int.TryParse(ResultNew, out i)) iNew = i;
                    ReplaceCount RC = new ReplaceCount(T, iOld, iNew);
                    Counts.Add(T, RC);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Counts;
        }

        #endregion


        #region ContainedParts

        private Dictionary<string, ReplacedPart> _ContainedParts;

        public Dictionary<string, ReplacedPart> ContainedParts
        {
            get
            {
                if (_ContainedParts == null)
                {
                    switch(this._Type)
                    {
                        case Replacement.Type.Project:
                            this._ContainedParts = this.ProjectPackages();
                            break;
                        default:
                            this._ContainedParts = new Dictionary<string, ReplacedPart>();
                            break;
                    }
                }
                return this._ContainedParts;
            }
            set => _ContainedParts = value;
        }


        private System.Collections.Generic.Dictionary<string, ReplacedPart> ProjectPackages()
        {
            System.Collections.Generic.Dictionary<string, ReplacedPart> COs = new Dictionary<string, ReplacedPart>();
            string SQL = "SELECT \"Package\", \"Version\" " +
                "FROM \"" + _Schema + "\".\"Package\" " +
                "UNION " +
                "SELECT \"AddOn\", \"Version\" " +
                "FROM \"" + _Schema + "\".\"PackageAddOn\";";
            System.Data.DataTable dtOld = this.SqlGetPostgresTable(SQL, this._Replacement.OldDatabase);
            System.Data.DataTable dtNew = this.SqlGetPostgresTable(SQL, this._Replacement.NewDatabase);

            try
            {
                // getting all packages and add ons as found in the 2 databases
                foreach (System.Data.DataRow R in dtOld.Rows)
                {
                    try
                    {
                        ReplacedPart CO = new ReplacedPart(this._Replacement, _Schema, Replacement.Type.Project_Package, R[0].ToString());
                        CO.VersionOld = int.Parse(R[1].ToString());
                        COs.Add(CO.Name, CO);
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
                        if (!COs.ContainsKey(R[0].ToString()))
                        {
                            ReplacedPart CO = new ReplacedPart(this._Replacement, _Schema, Replacement.Type.Project_Package, R[0].ToString());
                            CO.VersionNew = int.Parse(R[1].ToString());
                            COs.Add(CO.Name, CO);
                        }
                        else
                        {
                            ReplacedPart CO = COs[R[0].ToString()];
                            CO.VersionNew = int.Parse(R[1].ToString());
                            COs[R[0].ToString()] = CO;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                foreach(System.Collections.Generic.KeyValuePair<string, ReplacedPart> KV in COs)
                {
                    this._Status = this._Replacement.StatusWorst(this._Status, KV.Value.Status());
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return COs;
        }

        #endregion


        #region Aux
        private System.Data.DataTable SqlGetPostgresTable(string SQL, string DatabaseName, bool IgnoreException = true)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
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
                if (!IgnoreException)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        #endregion


    }


}
