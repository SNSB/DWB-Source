using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.BayernFlora
{
    public struct Taxon
    {
        #region Old version

        //public string Uri;
        //public string UriAcceptedName;
        //public System.Collections.Generic.List<string> UrisSubTaxa;
        
        #endregion

        public int NameID;
        public int AcceptedNameID;
        public System.Collections.Generic.List<int> SubNameIDs;
    }

    public class Taxa
    {
        private static System.Data.DataTable _DtTaxa;
        private static System.Collections.Generic.Dictionary<int, Taxon> _TaxaDict;
        private static System.Collections.Generic.List<int> _TaxaList;

        public static void InitCacheDB()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            string SQL = "SELECT DatabaseName, Server, Port, Version FROM CacheDatabase2";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            System.Data.DataRow R = dt.Rows[0];
            DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R["Server"].ToString();
            DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R["DatabaseName"].ToString();
            DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R["Port"].ToString());
            DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R["Version"].ToString();

        }

        public static int InitTaxa(InterfaceExport Interface, int? AnalysisID = null, System.Collections.Generic.List<string> AnalysisResults = null)
        {
            string SQL = "SELECT DISTINCT T.NameID, T.AcceptedNameID, T.NameParentID " +
                "FROM TaxonSynonymy T ";
            if (AnalysisID != null && AnalysisResults.Count > 0)
            {
                string Result = "";
                foreach(string R in AnalysisResults)
                {
                    if (Result.Length > 0)
                        Result += ", ";
                    Result += "N'" + R + "'";
                }
                SQL += " INNER JOIN TaxonAnalysis A ON T.NameID = A.NameID AND (A.AnalysisValue IN (" + Result + ")) AND (A.AnalysisID = " + AnalysisID.ToString() + ")";
            }
            SQL += " WHERE T.TaxonName <> 'New Taxon' and T.TaxonomicRank not in (" +
                "'fam.', 'superfam.', 'infraord.', 'subord.', 'ord.', 'superord.', 'infracl.', 'subcl.', 'cl.', 'supercl.', 'infraphyl./div.', " +
                "'subphyl./div.', 'phyl./div.', 'superphyl./div.', 'infrareg.', 'subreg.', 'reg.', 'superreg.', 'dom.', 'tax. supragen.')";
            _DtTaxa = new System.Data.DataTable();
            string Message = "";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref _DtTaxa, ref Message);
            Interface.setMax(_DtTaxa.Rows.Count);
            if (_TaxaDict == null)
                _TaxaDict = new Dictionary<int, Taxon>();
            _TaxaList = new List<int>();
            foreach (System.Data.DataRow R in _DtTaxa.Rows)
            {
                int NameID = int.Parse(R[0].ToString());
                _TaxaList.Add(NameID);
                if (!_TaxaDict.ContainsKey(NameID))
                {
                    Taxon T = new Taxon();
                    T.NameID = int.Parse(R[0].ToString());
                    T.AcceptedNameID = int.Parse(R[1].ToString());
                    //T.SubNameIDs = SubNameIDFromCacheDB(T.NameID);
                    _TaxaDict.Add(T.NameID, T);
                }
                Interface.setProgress(_TaxaList.Count);
            }
            return _TaxaList.Count;
        }

        public static int Count()
        {
            if (_TaxaDict == null)
                return 0;
            else
                return _TaxaDict.Count;
        }

        public static bool TaxonInList(int NameID)
        {
            return _TaxaList.Contains(NameID);
        }

        private static System.Collections.Generic.List<int> SubNameIDFromCacheDB(int NameID)
        {
            System.Collections.Generic.List<int> SubTaxa = new List<int>();
            string SQL = "declare @SubID TABLE(NameID int); " +
                "insert into @SubID(NameID)  " +
                "select NameID from TaxonSynonymy T " +
                "where T.NameParentID = " + NameID + "; " +
                "declare @i int; " +
                "set @i = (select count(*) from @SubID S, TaxonSynonymy T where S.NameID = T.NameParentID and T.NameID not in (select NameID from @SubID)); " +
                "while @i > 0 " +
                "begin " +
                "insert into @SubID(NameID)  " +
                "select T.NameID from @SubID S, TaxonSynonymy T where S.NameID = T.NameParentID and T.NameID not in (select NameID from @SubID); " +
                "set @i = (select count(*) from @SubID S, TaxonSynonymy T where S.NameID = T.NameParentID and T.NameID not in (select NameID from @SubID)); " +
                "end " +
                "select NameID from @SubID;";
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (!R[0].Equals(System.DBNull.Value) && !SubTaxa.Contains(int.Parse(R[0].ToString())) && R[0].ToString().Length > 0)
                    SubTaxa.Add(int.Parse(R[0].ToString()));
            }
            return SubTaxa;
        }

        public static System.Collections.Generic.List<int> SubNameID(int NameID)
        {
            if (_TaxaDict != null)
            {
                if (_TaxaDict[NameID].SubNameIDs == null)
                {
                    Taxon T = new Taxon();
                    T.NameID = NameID;
                    T.AcceptedNameID = _TaxaDict[NameID].AcceptedNameID;
                    T.SubNameIDs = SubNameIDFromCacheDB(NameID);
                    _TaxaDict[NameID] = T;
                }
                return _TaxaDict[NameID].SubNameIDs;
            }
            else
                return null;
        }

        public static System.Data.DataTable DtAnalysisFromCacheDB()
        {
            string SQL = "SELECT AnalysisID, DisplayText " +
                "FROM TaxonAnalysisCategory " +
                "ORDER BY DisplayText";
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            return dt;
        }

        #region Old version

        //private static System.Collections.Generic.Dictionary<string, Taxon> _TaxaDict;

        //private static void GetTaxon(string URI)
        //{
        //    GetTaxonFromCacheDB(URI);
        //    return;

        //    string UrlAccepted = "";
        //    string Name = DiversityWorkbench.TaxonName.AcceptedName(URI, ref UrlAccepted);
        //    System.Collections.Generic.Dictionary<string, string> Hierarchy = DiversityWorkbench.TaxonName.SubTaxa(UrlAccepted);
        //    System.Collections.Generic.List<string> SubTaxa = new List<string>();
        //    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Hierarchy)
        //    {
        //        if (KV.Key != UrlAccepted)
        //            SubTaxa.Add(KV.Key);
        //    }
        //    if (_TaxaDict == null)
        //        _TaxaDict = new Dictionary<string, Taxon>();
        //    if (!_TaxaDict.ContainsKey(URI))
        //    {
        //        Taxon T = new Taxon();
        //        T.Uri = URI;
        //        T.UriAcceptedName = UrlAccepted;
        //        T.UrisSubTaxa = SubTaxa;
        //        _TaxaDict.Add(URI, T);
        //    }
        //}

        //private static void GetTaxonFromCacheDB(string URI)
        //{
        //    if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length == 0)
        //    {
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        string Message = "";
        //        string SQL = "SELECT DatabaseName, Server, Port, Version FROM CacheDatabase2";
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
        //        System.Data.DataRow R = dt.Rows[0];
        //        DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R["Server"].ToString();
        //        DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R["DatabaseName"].ToString();
        //        DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R["Port"].ToString());
        //        DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R["Version"].ToString();
        //    }
        //    string UrlAccepted = UrlAcceptedFromCacheDB(URI);
        //    System.Collections.Generic.List<string> SubTaxa = UrlSubTaxonFromCacheDB(URI);
        //    if (_TaxaDict == null)
        //        _TaxaDict = new Dictionary<string, Taxon>();
        //    if (!_TaxaDict.ContainsKey(URI))
        //    {
        //        Taxon T = new Taxon();
        //        T.Uri = URI;
        //        T.UriAcceptedName = UrlAccepted;
        //        T.UrisSubTaxa = SubTaxa;
        //        _TaxaDict.Add(URI, T);
        //    }
        //}

        //private static string UrlAcceptedFromCacheDB(string URI)
        //{
        //    string URL = "";
        //    string SQL = "SELECT case when [AcceptedNameID] is null or [AcceptedNameID] = [NameID] " +
        //        "then [NameURI]  " +
        //        "else [BaseURL] + cast([AcceptedNameID] as varchar) end " +
        //        "FROM [dbo].[TaxonSynonymy] " +
        //        "T where T.NameURI = '" + URI + "'";
        //    URL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
        //    return URL;
        //}

        //private static System.Collections.Generic.List<string> UrlSubTaxonFromCacheDB(string URI)
        //{
        //    System.Collections.Generic.List<string> SubTaxa = new List<string>();
        //    string NameID = URI.Substring(URI.LastIndexOf("/")+1);
        //    string BaseURL = URI.Substring(0, URI.LastIndexOf("/")+1);
        //    string SQL = "declare @SubID TABLE(NameID int); " +
        //        "insert into @SubID(NameID)  " +
        //        "select NameID from TaxonSynonymy T " +
        //        "where T.NameParentID = " + NameID + "; " +
        //        "declare @i int; " +
        //        "set @i = (select count(*) from @SubID S, TaxonSynonymy T where S.NameID = T.NameParentID and T.NameID not in (select NameID from @SubID)); " +
        //        "while @i > 0 " +
        //        "begin " +
        //        "insert into @SubID(NameID)  " +
        //        "select T.NameID from @SubID S, TaxonSynonymy T where S.NameID = T.NameParentID and T.NameID not in (select NameID from @SubID); " +
        //        "set @i = (select count(*) from @SubID S, TaxonSynonymy T where S.NameID = T.NameParentID and T.NameID not in (select NameID from @SubID)); " +
        //        "end " +
        //        "select NameID from @SubID;";
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    string Message = "";
        //    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
        //    foreach (System.Data.DataRow R in dt.Rows)
        //    {
        //        if (!R[0].Equals(System.DBNull.Value) && !SubTaxa.Contains(BaseURL + R[0].ToString()) && R[0].ToString().Length >  0)
        //            SubTaxa.Add(BaseURL + R[0].ToString());
        //    }
        //    return SubTaxa;
        //}

        //public static string UrlAcceptedName(string URL)
        //{
        //    if (_TaxaDict == null)
        //        _TaxaDict = new Dictionary<string, Taxon>();
        //    if (!_TaxaDict.ContainsKey(URL))
        //        GetTaxon(URL);
        //    return _TaxaDict[URL].UriAcceptedName;
        //}

        //public static System.Collections.Generic.List<string> UrlsSubTaxa(string URL)
        //{
        //    if (_TaxaDict == null)
        //        _TaxaDict = new Dictionary<string, Taxon>();
        //    if (URL.Length > 0 && !_TaxaDict.ContainsKey(URL))
        //        GetTaxon(URL);
        //    return _TaxaDict[URL].UrisSubTaxa;
        //}
        
        #endregion

    }
}
