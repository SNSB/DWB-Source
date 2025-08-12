using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public class CacheDatabase : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction

        public CacheDatabase(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityCollectionCache"; }

        public override System.Collections.Generic.List<DiversityWorkbench.DatabaseService> DatabaseServices()
        {
            System.Collections.Generic.List<DiversityWorkbench.DatabaseService> ds = new List<DatabaseService>();
            //string Test = "";
            try
            {
                string SQLDS = "SELECT DISTINCT SourceView " +
                    "FROM SourceTransfer WHERE Source = '";
                switch (this._CacheDomain)
                {
                    case CacheDomain.Agent:
                        SQLDS += "Agents";
                        break;
                    case CacheDomain.Gazet:
                        SQLDS += "Gazetteer";
                        break;
                    case CacheDomain.Names:
                        SQLDS += "Taxa";
                        break;
                    case CacheDomain.Terms:
                        SQLDS += "ScientificTerms";
                        break;
                    default:
                        break;
                }
                SQLDS += "' ORDER BY SourceView";
                Microsoft.Data.SqlClient.SqlDataAdapter adDS = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLDS, this.ServerConnection.ConnectionString);// KV.Value.ConnectionString);
                System.Data.DataTable dtDS = new System.Data.DataTable();
                adDS.Fill(dtDS);
                foreach (System.Data.DataRow R in dtDS.Rows)
                {
                    DiversityWorkbench.DatabaseService DS;
                    DS = new DatabaseService(this.ServerConnection.DatabaseName);// KV.Value.DatabaseName);
                    DS.IsWebservice = false;
                    DS.IsListInDatabase = true;
                    DS.ListName = R["SourceView"].ToString();
                    DS.RestrictionForListInDatabase = "SourceView = '" + R["SourceView"].ToString() + "'";
                    ds.Add(DS);
                }
            }
            catch (System.Exception ex)
            {
            }
            return ds;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues()
        {
            switch (this._CacheDomain)
            {
                case CacheDomain.Names:
                    return this.TaxonValues(-1);
                    break;
                case CacheDomain.Terms:
                    break;
            }

            if (this._UnitValues == null)
                this._UnitValues = new Dictionary<string, string>();
            return this._UnitValues;
        }

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            //string Prefix = "";
            //if (this._ServerConnection.LinkedServer.Length > 0)
            //    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            //else Prefix = "dbo.";

            switch (this._CacheDomain)
            {
                case CacheDomain.Names:
                    return this.TaxonValues(ID);
                    break;
                case CacheDomain.Terms:
                    break;
            }

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();

            return Values;
        }

        public string MainTable() { return "Sources"; }
        
        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
            switch (this._CacheDomain)
            {
                case CacheDomain.Names:
                    QueryDisplayColumns[0].DisplayText = "TaxonName";
                    QueryDisplayColumns[0].DisplayColumn = "TaxonName";
                    QueryDisplayColumns[0].OrderColumn = "TaxonName";
                    QueryDisplayColumns[0].IdentityColumn = "NameID";
                    QueryDisplayColumns[0].TableName = "TaxonSynonymy";
                    QueryDisplayColumns[0].Module = "CacheDB|DiversityTaxonNames";
                    break;
                case CacheDomain.Agent:
                    QueryDisplayColumns[0].Module = "CacheDB|DiversityAgents";
                    goto default;
                case CacheDomain.Descr:
                    QueryDisplayColumns[0].Module = "CacheDB|DiversityDescriptions";
                    goto default;
                case CacheDomain.Gazet:
                    QueryDisplayColumns[0].Module = "CacheDB|DiversityGazetteer";
                    goto default;
                case CacheDomain.Refer:
                    QueryDisplayColumns[0].Module = "CacheDB|DiversityReferences";
                    goto default;
                case CacheDomain.Terms:
                    QueryDisplayColumns[0].Module = "CacheDB|DiversityScientificTerms";
                    goto default;
                default:
                    QueryDisplayColumns[0].DisplayText = "DisplayText";
                    QueryDisplayColumns[0].DisplayColumn = "DisplayText";
                    QueryDisplayColumns[0].OrderColumn = "DisplayText";
                    QueryDisplayColumns[0].IdentityColumn = "ID";
                    QueryDisplayColumns[0].TableName = "Sources";
                    break;
            }

            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Database = "DiversityCollectionCache";
            try
            {
                Database = this._ServerConnection.DatabaseName;// DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            return QueryConditions;
        } 

        public static string CollectionCacheDB
        {
            get
            {
                string SQL = "SELECT TOP (1) [DatabaseName] FROM [CacheDatabase2]";
                string CollectionCacheDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                return CollectionCacheDB;
            }
        }

        #region Domains

        public enum CacheDomain { Agent, Gazet, Terms, Names, Refer, Descr, Missing }
        private CacheDomain _CacheDomain = CacheDomain.Missing;
        public void SetCacheDomain(CacheDomain Domain)
        {
            this._CacheDomain = Domain;
        }
        public string DomainInCacheDB() { return this._CacheDomain.ToString(); }

        private string _SourceViewDisplayText = "";

        private string _SourceView;
        public string SourceView
        {
            get { return this._SourceView; }
        }

        public string SourceViewDisplayText
        {
            get { return _SourceViewDisplayText; }
            set 
            { 
                _SourceViewDisplayText = value;
                switch (Domain)
                {
                    case "Agent":
                        SetCacheDomain(CacheDomain.Agent);
                        break;
                    case "Gazet":
                        SetCacheDomain(CacheDomain.Gazet);
                        break;
                    case "Terms":
                        SetCacheDomain(CacheDomain.Terms);
                        break;
                    case "Names":
                        SetCacheDomain(CacheDomain.Names);
                        break;
                    case "Refer":
                        SetCacheDomain(CacheDomain.Refer);
                        break;
                    case "Descr":
                        SetCacheDomain(CacheDomain.Descr);
                        break;
                    default:
                        SetCacheDomain(CacheDomain.Missing);
                        break;
                }
                foreach (DiversityWorkbench.DatabaseService S in this.DatabaseServices())
                {
                    if (S.DisplayText == value)
                    {
                        this._SourceView = S.ListName;
                        break;
                    }
                }

            }
        }
        
        private string Domain
        {
            get
            {
                return this._CacheDomain.ToString();
            }
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string SourceView, int ID)
        {
            this.SourceViewDisplayText = SourceView;
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            switch (this._CacheDomain)
            {
                case CacheDomain.Agent:
                    break;
                case CacheDomain.Terms:
                    Values = this.TermValues(ID);
                    break;
                case CacheDomain.Names:
                    Values = this.TaxonValues(ID);
                    break;
            }
            return Values;
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns(string SourceView)
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns;
            this.SourceViewDisplayText = SourceView;
            switch (Domain)
            {
                case "Terms":
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                    QueryDisplayColumns[0].DisplayText = "DisplayText";
                    QueryDisplayColumns[0].DisplayColumn = "DisplayText";
                    QueryDisplayColumns[0].OrderColumn = "DisplayText";
                    QueryDisplayColumns[0].IdentityColumn = "TerminologyID";
                    QueryDisplayColumns[0].TableName = "Terminology";
                    break;
                default:
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[0];
                    break;
            }
            return QueryDisplayColumns;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string SourceView)
        {
            this._SourceViewDisplayText = SourceView;
            string Database = "DiversityCollectionCache";
            try
            {
                Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityCollectionCache"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
            switch (this._CacheDomain)
            {
                case CacheDomain.Terms:
                    #region Representation
            
                    string Description = "Scientific term";
                    DiversityWorkbench.QueryCondition qTerm = new DiversityWorkbench.QueryCondition(true, "ScientificTerm", "RepresentationURI", "DisplayText", "Term", "Term", "Scientific term", Description);
                    QueryConditions.Add(qTerm);

                    //Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "ExternalDatabase");
                    //DiversityWorkbench.QueryCondition qExternalDatabase = new DiversityWorkbench.QueryCondition(false, "Terminology", "TerminologyID", "ExternalDatabase", "Terminology", "Ext. database", "External database", Description);
                    //QueryConditions.Add(qExternalDatabase);

                    //Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "ExternalDatabaseAuthors");
                    //DiversityWorkbench.QueryCondition qExternalDatabaseAuthors = new DiversityWorkbench.QueryCondition(false, "Terminology", "TerminologyID", "ExternalDatabaseAuthors", "Terminology", "Ext. DB aut.", "External database authors", Description);
                    //QueryConditions.Add(qExternalDatabaseAuthors);
            
                    #endregion

                    break;
                case CacheDomain.Names:
                    DiversityWorkbench.QueryCondition qTaxon = new DiversityWorkbench.QueryCondition(true, "TaxonSynonymy", "NameID", "TaxonName", "Taxon", "Taxon", "Taxon", "");
                    QueryConditions.Add(qTaxon);

                    break;
                default:
                    break;
            }
            return QueryConditions;
        }

        public System.Data.DataTable DtSources()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            return dt;
        }

        private System.Collections.Generic.Dictionary<string, string> TermValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT U.BaseURL + 'Terminology/' + CAST(TerminologyID AS varchar) AS  _URI, T.DisplayText AS _DisplayText, " +
                    "DisplayText, Description, TerminologyID AS ID, ExternalDatabase, ExternalDatabaseVersion, ExternalDatabaseAuthors, ExternalDatabaseURI, " +
                    "ExternalDatabaseInstitution, ExternalAttribute_NameID, Rights, DefaultLanguageCode " +
                    "FROM " + Prefix + "Terminology AS T, " + Prefix + "ViewBaseURL U " +
                    "WHERE T. TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT DisplayText AS Property/*Property,  Description, Datatype ,*/ " +
                    "FROM " + Prefix + "TerminologyProperty AS P " +
                    "WHERE        TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Reference, ReferenceDetails " +
                    "FROM " + Prefix + "TerminologyReference AS R " +
                    "WHERE TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        private System.Collections.Generic.Dictionary<string, string> TaxonValues(int ID)
        {
            string Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT T.NameURI AS _URI, T.TaxonName AS _DisplayText, " +
                    "T.NameURI AS Link, T.TaxonName AS Taxon, T.TaxonNameSinAuthor AS [Taxon name sin author], T.NameID AS ID, " +
                    "T.AcceptedName AS [Accepted name], T.AcceptedNameSinAuthor AS [Accepted name sin author], " +
                    "T.TaxonomicRank AS Rank, T.GenusOrSupragenericName AS Genus, P.TaxonName AS [Higher taxon] " +
                    "FROM TaxonSynonymy AS T LEFT OUTER JOIN TaxonSynonymy AS P ON T.NameParentID = P.NameID " +
                    "WHERE T.NameID = " + ID.ToString() + " AND T.SourceView = N'" + this._SourceView + "'";
                this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT DisplayText AS Property/*Property,  Description, Datatype ,*/ " +
                //    "FROM " + Prefix + "TerminologyProperty AS P " +
                //    "WHERE        TerminologyID = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT Reference, ReferenceDetails " +
                //    "FROM " + Prefix + "TerminologyReference AS R " +
                //    "WHERE TerminologyID = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        #endregion

        #endregion

        //#region static functions

        ///// <summary>
        ///// Getting the terms underneath a given term including the given term
        ///// </summary>
        ///// <param name="URL">The URL of the term (= BaseURL + RepresentationID)</param>
        ///// <returns>A dictionary containing the URLs and the terms</returns>
        //public static System.Collections.Generic.Dictionary<string, string> SubPlots(string URL)
        //{
        //    System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
        //    System.Data.DataTable dt = getStartTerm(URL);
        //    getSubTerms(ref dt);
        //    getTerms(ref DD, dt);
        //    return DD;
        //}

        ///// <summary>
        ///// Getting the synonyms of a given term
        ///// </summary>
        ///// <param name="URL">The URL of the term (= BaseURL + RepresentationID)</param>
        ///// <returns>A dictionary containing the URLs and the terms</returns>
        //public static System.Collections.Generic.Dictionary<string, string> Synonyms(string URL)
        //{
        //    System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
        //    System.Data.DataTable dt = getStartTerm(URL);
        //    getSynonyms(ref dt);
        //    getTerms(ref DD, dt);
        //    return DD;
        //}

        ///// <summary>
        ///// Getting the terms underneath a given term including the given term and all synonyms
        ///// </summary>
        ///// <param name="URL">The URL of the taxon (= BaseURL + NameID)</param>
        ///// <returns>A dictionary containing the URLs and the names</returns>
        //public static System.Collections.Generic.Dictionary<string, string> SubTermSynonyms(string URL)
        //{
        //    System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
        //    System.Data.DataTable dt = getStartTerm(URL);
        //    getSubTerms(ref dt);
        //    getSynonyms(ref dt);
        //    getTerms(ref DD, dt);
        //    return DD;
        //}

        //#region Auxillary

        //private static int? _TerminologyID;
        ////private static int TerminologyID()
        ////{
        ////    if (_TerminologyID == null)
        ////    {
        ////        string SQL = "SELECT TerminologyID, DisplayText FROM " + _SC.Prefix() + "Terminology ORDER BY DisplayText";
        ////        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
        ////        System.Data.DataTable dt = new System.Data.DataTable();
        ////        ad.Fill(dt);
        ////        if (dt.Rows.Count > 1)
        ////        {
        ////            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "TerminologyID", "Terminology", "Please select a terminology");
        ////            f.ShowDialog();
        ////            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        ////            {
        ////                int i;
        ////                if (int.TryParse(f.SelectedValue, out i))
        ////                    _TerminologyID = i;
        ////            }
        ////        }
        ////        else
        ////            _TerminologyID = int.Parse(dt.Rows[0][0].ToString());
        ////    }
        ////    return (int)_TerminologyID;
        ////}

        //private static int TerminologyID(string IDs)
        //{
        //    if (_TerminologyID == null)
        //    {
        //        string SQL = "SELECT DISTINCT TerminologyID FROM TermRepresentation R WHERE RepresentationID IN (" + IDs + ")";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        ad.Fill(dt);
        //        if (dt.Rows.Count == 1)
        //            _TerminologyID = int.Parse(dt.Rows[0][0].ToString());
        //        else
        //        {
        //            dt = new System.Data.DataTable();
        //            SQL = "SELECT TerminologyID, DisplayText FROM " + _SC.Prefix() + "Terminology ORDER BY DisplayText";
        //            ad.SelectCommand.CommandText = SQL;
        //            ad.Fill(dt);
        //            if (dt.Rows.Count > 1)
        //            {
        //                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Terminology", "TerminologyID", "Terminology", "Please select a terminology");
        //                f.ShowDialog();
        //                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //                {
        //                    int i;
        //                    if (int.TryParse(f.SelectedValue, out i))
        //                        _TerminologyID = i;
        //                }
        //            }
        //            else
        //                _TerminologyID = int.Parse(dt.Rows[0][0].ToString());
        //        }
        //    }
        //    return (int)_TerminologyID;
        //}

        //private static DiversityWorkbench.ServerConnection _SC;

        //private static System.Data.DataTable getStartTerm(string URL)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    try
        //    {
        //        // resetting the project
        //        _TerminologyID = null;
        //        // getting the server connection for the URL
        //        setServerConnection(URL);
        //        // Inserting the ID to start the query
        //        string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URL);
        //        string SQL = "SELECT RepresentationID FROM " + _SC.Prefix() + "TermRepresentation N WHERE RepresentationID = " + ID;
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
        //        ad.Fill(dt);
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //    return dt;
        //}

        //public static void setServerConnection(string URL)
        //{
        //    // getting the server connection for the URL
        //    _SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URL);
        //}

        //public static void getTerms(ref System.Collections.Generic.Dictionary<string, string> DD, System.Data.DataTable DT)
        //{
        //    string SQL = "";
        //    try
        //    {
        //        foreach (System.Data.DataRow R in DT.Rows)
        //        {
        //            if (SQL.Length > 0) SQL += ", ";
        //            SQL += R[0].ToString();
        //        }
        //        SQL = "SELECT U.BaseURL + cast(T.RepresentationID as varchar) AS URL, T.DisplayText AS Term FROM " + _SC.Prefix() + "TermRepresentation T, " + _SC.Prefix() + "ViewBaseURL U WHERE T.RepresentationID IN (" + SQL + ")";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        ad.Fill(dt);
        //        foreach (System.Data.DataRow R in dt.Rows)
        //        {
        //            if (!DD.ContainsKey(R[0].ToString()))
        //                DD.Add(R[0].ToString(), R[1].ToString());
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //}

        //private static void getSubTerms(ref System.Data.DataTable DT)
        //{
        //    string IDs = "";
        //    foreach (System.Data.DataRow R in DT.Rows)
        //    {
        //        if (IDs.Length > 0) IDs += ", ";
        //        IDs += R[0].ToString();
        //    }
        //    string SQL = "SELECT R.RepresentationID " +
        //        " FROM " + _SC.Prefix() + "Term T, " +
        //        "" + _SC.Prefix() + "Term TP, " +
        //        "" + _SC.Prefix() + "TermRepresentation R, " +
        //        "" + _SC.Prefix() + "TermRepresentation RP " +
        //        " WHERE R.TermID = T.TermID " +
        //        " AND TP.TermID = RP.TermID " +
        //        " AND T.BroaderTermID = TP.TermID " +
        //        " AND T.TerminologyID = R.TerminologyID " +
        //        " AND T.TerminologyID = TP.TerminologyID " +
        //        " AND TP.TerminologyID = RP.TerminologyID " +
        //        " AND T.TerminologyID = " + TerminologyID(IDs) + " " +
        //        " AND RP.RepresentationID IN ( " + IDs + ") " +
        //        " AND R.RepresentationID NOT IN (" + IDs + ")";
        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    ad.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        ad.Fill(DT);
        //        getSubTerms(ref DT);
        //    }
        //}

        //private static void getSynonyms(ref System.Data.DataTable DT, bool RestricTerminology = false)
        //{
        //    try
        //    {
        //        if (_SC == null)
        //            return;

        //        string IDs = "";
        //        foreach (System.Data.DataRow R in DT.Rows)
        //        {
        //            if (IDs.Length > 0) IDs += ", ";
        //            IDs += R[0].ToString();
        //        }
        //        string sTerminologyID = "";
        //        sTerminologyID = TerminologyID(IDs).ToString();
        //        // getting the terms that are synonmys to the terms in the table
        //        string SQL = "SELECT R.RepresentationID FROM " + _SC.Prefix() + "Term T, " +
        //            _SC.Prefix() + "TermRepresentation R, " +
        //            _SC.Prefix() + "TermRepresentation S " +
        //            "WHERE R.TermID = T.TermID  AND S.TermID = T.TermID " +
        //            " AND S.RepresentationID IN( " + IDs + ") " +
        //            " AND T.TerminologyID = " + sTerminologyID + " " +
        //            " AND R.RepresentationID NOT IN (" + IDs + ")";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        ad.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            ad.Fill(DT);
        //            getSynonyms(ref DT);
        //        }

        //        //// getting the synonyms of the names in the table
        //        //dt.Clear();
        //        //ad.SelectCommand.CommandText = "SELECT S.SynNameID AS NameID FROM " + _SC.Prefix() + "TaxonSynonymy S, " + _SC.Prefix() + "TaxonName N WHERE S.NameID = N.NameID " +
        //        //    " AND S.NameID IN ( " + IDs + ") " +
        //        //    " AND S.ProjectID = " + sTerminologyID + " AND S.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
        //        //    " AND S.SynNameID NOT IN (" + IDs + ")";
        //        //ad.Fill(dt);
        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    ad.Fill(DT);
        //        //    getSynonyms(ref DT, RestricTerminology);
        //        //}
        //        //// getting the names that are based on the names in the table
        //        //dt.Clear();
        //        //ad.SelectCommand.CommandText = "SELECT N.NameID FROM " + _SC.Prefix() + "TaxonName N " +
        //        //    " WHERE N.BasedOnNameID IN ( " + IDs + ") " +
        //        //    " AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
        //        //    " AND N.NameID NOT IN (" + IDs + ")";
        //        //ad.Fill(dt);
        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    ad.Fill(DT);
        //        //    getSynonyms(ref DT, RestricTerminology);
        //        //}
        //        //// getting the Basionyms for the names in the table
        //        //dt.Clear();
        //        //ad.SelectCommand.CommandText = "SELECT N.BasedOnNameID AS NameID FROM " + _SC.Prefix() + "TaxonName N " +
        //        //    " WHERE N.NameID IN ( " + IDs + ") " +
        //        //    " AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
        //        //    " AND N.BasedOnNameID NOT IN (" + IDs + ")";
        //        //ad.Fill(dt);
        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    ad.Fill(DT);
        //        //    getSynonyms(ref DT, RestricTerminology);
        //        //}
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //}

        //#endregion

        //#endregion

    }
}
