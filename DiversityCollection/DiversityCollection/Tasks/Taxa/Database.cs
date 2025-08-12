#define JSON

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DiversityCollection.Tasks.IPM;

namespace DiversityCollection.Tasks.Taxa
{

    public struct TaxonProjectSource
    {
        public string DatabaseName;
        public int ProjectID;
    }

    public struct TaxonChecklistSource
    {
        public string DatabaseName;
        public int ProjectID;
        public int ChecklistID;
    }

    public class Database
    {
        public static string TaxonDBPrefix { get { return Settings.Default.DiversityTaxonNamesDatabase + ".dbo."; } }

        #region BaseURL
        public static string TaxonDBBaseURL
        {
            get
            {
                if (_TaxonDBBaseURL == null)
                {
                    string SQL = "SELECT BaseURL FROM " + TaxonDBPrefix + "ViewBaseURL";
                    _TaxonDBBaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                return _TaxonDBBaseURL;
            }
        }
        private static string _TaxonDBBaseURL;

        #endregion

        #region Resources

        private static Dictionary<ResourceType, System.Data.DataTable> _ResourceTables;

        public static System.Data.DataTable ResourceTable(ResourceType resourceType)
        {
            if (_ResourceTables == null)
            {
                _ResourceTables = new Dictionary<ResourceType, System.Data.DataTable>();
            }    
            if (!_ResourceTables.ContainsKey(resourceType))
            {
                string SQL = "";
                System.Data.DataTable dt =new System.Data.DataTable();
                switch (resourceType)
                {
                    case ResourceType.Image:
                    case ResourceType.Info:
                        string type = resourceType.ToString();
                        if (type.ToLower() == "info") type = "information";
                        SQL = "SELECT Im.NameID, Im.Title, Im.URI, Im.Notes, Im.CopyrightStatement, Im.DisplayOrder, Creator " +
                            "FROM " + Database.TaxonDBPrefix + "ViewTaxonNameResource AS Im " +
                            "WHERE Im.ResourceType = '" + type + "'  AND (Im.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ")" +
                            "ORDER BY Im.DisplayOrder";
                        break;
                    //SQL = "SELECT Im.NameID, Im.URI AS Image, Im.Notes, Im.CopyrightStatement, Im.DisplayOrder " +
                    //    "FROM " + Database.TaxonDBPrefix + "ViewTaxonNameResource AS Im " +
                    //    "WHERE Im.ResourceType = 'Image'  AND (R.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ")" +
                    //    "ORDER BY Im.DisplayOrder";
                    //break;
                    //SQL = "SELECT R.NameID, URI AS Info, Creator " +
                    //    "FROM " + Database.TaxonDBPrefix + "ViewTaxonNameResource AS R " +
                    //    "WHERE ResourceType = 'Information' AND (R.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ")" +
                    //    "ORDER BY DisplayOrder";
                    //break;
                    case ResourceType.Preview:
                        SQL = "SELECT  R.NameID, CASE WHEN C.AnalysisID IS NULL THEN 0 ELSE C.AnalysisID END AS StageID, R.URI " +
                            "FROM " + TaxonDBPrefix + "TaxonNameListAnalysis AS A " +
                            "LEFT OUTER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID " +
                            "RIGHT OUTER JOIN " + TaxonDBPrefix + "ViewTaxonNameResource AS R ON A.NameID = R.NameID AND C.DisplayText = R.Title " +
                            "WHERE(R.ResourceType = N'preview') AND(R.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ")";
                        break;
                    //case ResourceType.Preview:
                    //    SQL = "SELECT  B.BaseURL + CAST(R.NameID AS varchar) + CASE WHEN C.AnalysisID IS NULL THEN '' ELSE '|' + CAST(C.AnalysisID AS varchar) END AS TaxonIdentifier, R.URI" +
                    //        "FROM " + TaxonDBPrefix + "TaxonNameListAnalysis AS A " +
                    //        "LEFT OUTER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID " +
                    //        "RIGHT OUTER JOIN " + TaxonDBPrefix + "ViewTaxonNameResource AS R ON A.NameID = R.NameID AND C.DisplayText = R.Title " +
                    //        "CROSS JOIN " + TaxonDBPrefix + "ViewBaseURL AS B" +
                    //        "WHERE (R.ResourceType = N'preview') AND (R.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ")";
                    //    break;
                }
                dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                _ResourceTables.Add(resourceType, dt);
            }
            return _ResourceTables[resourceType];
        }

        //#region Preview images

        //private static System.Data.DataTable _DtPreviewImages;
        //private static System.Data.DataTable DtPreviewImages()
        //{ 
        //    if (_DtPreviewImages == null)
        //    {
        //        string SQL = "SELECT  B.BaseURL + CAST(R.NameID AS varchar) + CASE WHEN C.AnalysisID IS NULL THEN '' ELSE '|' + CAST(C.AnalysisID AS varchar) END AS TaxonIdentifier, R.URI" +
        //            "FROM " + TaxonDBPrefix + "TaxonNameListAnalysis AS A " +
        //            "LEFT OUTER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID " +
        //            "RIGHT OUTER JOIN " + TaxonDBPrefix + "ViewTaxonNameResource AS R ON A.NameID = R.NameID AND C.DisplayText = R.Title " +
        //            "CROSS JOIN " + TaxonDBPrefix + "ViewBaseURL AS B" +
        //            "WHERE (R.ResourceType = N'preview') AND (R.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ")";
        //        _DtPreviewImages = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //    }
        //    return _DtPreviewImages; 
        //}

        //private System.Collections.Generic.Dictionary<string, string> _PreviewImages;
        //private System.Collections.Generic.Dictionary<string, string> PreviewImages
        //{
        //    get 
        //    {
        //        if ( _PreviewImages == null)
        //        {
        //            _PreviewImages = new System.Collections.Generic.Dictionary<string, string>();
        //            foreach(System.Data.DataRow R in DtPreviewImages().Rows)
        //            {
        //                _PreviewImages.Add(R[0].ToString(), R[1].ToString());
        //            }
        //        }
        //        return _PreviewImages; 
        //    }
        //}

        //public string PreviewImage(string TaxonIdentifier)
        //{
        //    string Preview = "";
        //    if (PreviewImages.ContainsKey(TaxonIdentifier))
        //    {
        //        Preview = PreviewImages[TaxonIdentifier];
        //    }
        //    return Preview;
        //}

        //#endregion

        #endregion

        #region AcceptedNames

        //private static System.Data.DataTable _DtAcceptedNames;
        //private static System.Data.DataTable DtAcceptedNames()
        //{
        //    if (_DtAcceptedNames == null)
        //    {
        //        string SQL = "SELECT T.NameID, CASE WHEN A.NameID IS NULL THEN S.AcceptedNameID ELSE A.NameID END AS AcceptedNameID " +
        //            " FROM " + TaxonDBPrefix + "TaxonName AS T " +
        //            " LEFT OUTER  JOIN " + TaxonDBPrefix + "TaxonAcceptedName AS A ON T.NameID = A.NameID " +
        //            " LEFT OUTER JOIN " + TaxonDBPrefix + "[TaxonSynonymyAcceptedNameID] AS S ON S.NameID = A.NameID AND S.ProjectID = A.ProjectID";
        //        SQL += " WHERE (A.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") ";
        //        _DtAcceptedNames = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //    }
        //    return _DtAcceptedNames;
        //}

        //private static System.Collections.Generic.Dictionary<int, int> _AcceptedNames;
        //public static System.Collections.Generic.Dictionary<int, int> AcceptedNames
        //{
        //    get
        //    {
        //        if (_AcceptedNames == null)
        //        {
        //            _AcceptedNames = new Dictionary<int, int>();
        //            foreach (System.Data.DataRow row in DtAcceptedNames().Rows)
        //            {
        //                _AcceptedNames.Add(int.Parse(row[0].ToString()), int.Parse(row[1].ToString()));
        //            }
        //        }
        //        return _AcceptedNames;
        //    }
        //}

        #endregion

        #region CommonNames

        //private static System.Data.DataTable _DtCommonNames;
        //private static System.Data.DataTable DtCommonNames()
        //{
        //    if (_DtCommonNames == null)
        //    {
        //        string SQL = "SELECT N.NameID, CASE WHEN C.CommonName IS NULL THEN " +
        //            " N.GenusOrSupragenericName + CASE WHEN N.SpeciesEpithet <> '' THEN ' ' + N.SpeciesEpithet ELSE '' END ELSE C.CommonName END" +
        //            " FROM " + TaxonDBPrefix + "TaxonCommonName AS C" +
        //            " RIGHT OUTER JOIN " + TaxonDBPrefix + "TaxonName AS N ON C.NameID = N.NameID " +
        //            " AND (C.LanguageCode = '" + DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower() + "')  " +
        //            " INNER JOIN " + TaxonDBPrefix + "TaxonNameProject AS P ON N.NameID = P.NameID" +
        //            " WHERE (P.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") ";
        //        _DtCommonNames = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //    }
        //    return _DtCommonNames;
        //}

        private static System.Collections.Generic.Dictionary<int, string> _CommonNames;
        private static System.Collections.Generic.Dictionary<int, string> CommonNames
        {
            get
            {
                if (_CommonNames == null)
                {
                    _CommonNames = new Dictionary<int, string>();
                    //foreach (System.Data.DataRow row in DtCommonNames().Rows)
                    //{
                    //    if (!_CommonNames.ContainsKey(int.Parse(row[0].ToString())))
                    //    _CommonNames.Add(int.Parse(row[0].ToString()), row[1].ToString());
                    //}
                    foreach (System.Data.DataRow row in DtTaxa.Rows)
                    {
                        int NameID; 
                        if (int.TryParse(row["NameID"].ToString(), out NameID))
                        {
                            string CommonName = row["TaxonName"].ToString();
                            if (!_CommonNames.ContainsKey(NameID))
                                _CommonNames.Add(NameID, CommonName);
                        }
                    }
                }
                return _CommonNames;
            }
        }

        public static string CommonName(int NameID)
        {
            if (CommonNames.ContainsKey(NameID))
                return CommonNames[NameID];
            return "";
        }

        #endregion

        #region Sorting

        private static int _SortingAnalysisID = 0;
        private static int SortingAnalysisID
        {
            get
            {
                if (_SortingAnalysisID == 0)
                {
                    string SQL = "SELECT C.AnalysisID " +
                        "FROM " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C " +
                        "INNER JOIN  " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS I ON C.AnalysisParentID = I.AnalysisID " +
                        "WHERE (I.DisplayText = N'IPM') AND C.DisplayText IN ('Sorting')";
                    int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out _SortingAnalysisID);
                }
                return _SortingAnalysisID;
            }
        }

        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, int>> _SortingInListDict;

        public static System.Collections.Generic.Dictionary<int, int> SortingInList(int ListID)
        {
            if (_SortingInListDict == null) _SortingInListDict = new Dictionary<int, Dictionary<int, int>>();
            if (!_SortingInListDict.ContainsKey(ListID))
            {
                string SQL = "SELECT T.NameID, MIN(L.AnalysisValue) AS Sorting " +
                    " FROM  " + TaxonDBPrefix + "TaxonName AS T " +
                    " INNER JOIN  " + TaxonDBPrefix + "TaxonNameListAnalysis AS L ON T.NameID = L.NameID AND L.AnalysisID = " + SortingAnalysisID.ToString() + " AND L.ProjectID = " + ListID.ToString() +
                    " INNER JOIN  " + TaxonDBPrefix + "TaxonNameProject P ON T.NameID = P.NameID AND P.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + " " +
                    " WHERE (T.IgnoreButKeepForReference = 0) " +
                    " AND (T.DataWithholdingReason IS NULL OR T.DataWithholdingReason = '') " +
                    "GROUP BY T.NameID";
                Dictionary<int, int> dict = new Dictionary<int, int>();
                System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    int Sort;
                    if (int.TryParse(R[1].ToString(), out Sort))
                        dict.Add(int.Parse(R[0].ToString()), Sort);
                }
                _SortingInListDict.Add(ListID, dict);
            }
            return _SortingInListDict[ListID];
        }

        private static System.Collections.Generic.Dictionary<int, System.Data.DataTable> _DtSorting;
        public static System.Data.DataTable DtSorting(int ListID) 
        { 
            if (_DtSorting == null) 
            { 
                _DtSorting = new Dictionary<int, System.Data.DataTable>(); 
            }
            if (!_DtSorting.ContainsKey(ListID))
            {
                string SQL = "SELECT A.NameID, T.TaxonNameCache, A.AnalysisValue AS Sorting " +
                    " FROM " +
                    TaxonDBPrefix + "TaxonNameListAnalysis AS A INNER JOIN" +
                    TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = " + SortingAnalysisID.ToString() + " AND A.AnalysisID = C.AnalysisID and isnumeric(A.AnalysisValue) = 1 INNER JOIN" +
                    TaxonDBPrefix + "TaxonName AS T ON A.NameID = T.NameID INNER JOIN  " + 
                    TaxonDBPrefix + "TaxonNameProject P ON T.NameID = P.NameID AND P.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + " " +
                    " WHERE A.ProjectID = " + ListID.ToString() +
                    " ORDER BY A.ProjectID, cast(A.AnalysisValue as int)";
                System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                _DtSorting.Add(ListID, dt);
            }
            return _DtSorting[ListID];
        }

        public static void DtSortingReset()
        { _DtSorting = null; }

      
        #endregion

        #region Groups

        private static int _GroupAnalysisID = 0;
        private static int GroupAnalysisID
        {
            get
            {
                if (_GroupAnalysisID == 0)
                {
                    string SQL = "SELECT C.AnalysisID " +
                        "FROM " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C " +
                        "INNER JOIN  " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS I ON C.AnalysisParentID = I.AnalysisID " +
                        "WHERE (I.DisplayText = N'IPM') AND C.DisplayText IN ('Group')";
                    int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out _GroupAnalysisID);
                }
                return _GroupAnalysisID;
            }
        }

        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, string>> _ListGroupDict;

        public static System.Collections.Generic.Dictionary<int, string> GroupsInList(int ListID)
        {
            if (_ListGroupDict == null) _ListGroupDict = new Dictionary<int, Dictionary<int, string>>();
            if (!_ListGroupDict.ContainsKey(ListID))
            {
                string SQL = "SELECT T.NameID, MIN(CASE WHEN C.CommonName IS NULL THEN T.GenusOrSupragenericName  ELSE C.CommonName END) AS Gruppe " +
                    " FROM  " + TaxonDBPrefix + "TaxonName AS T " +
                    " INNER JOIN  " + TaxonDBPrefix + "TaxonAcceptedName AS A ON T.NameID = A.NameID AND A.IgnoreButKeepForReference = 0 " +
                    " INNER JOIN  " + TaxonDBPrefix + "TaxonNameListAnalysis AS L ON T.NameID = L.NameID AND L.AnalysisID = " + GroupAnalysisID.ToString() + " AND L.ProjectID = " + ListID.ToString() +
                    " INNER JOIN  " + TaxonDBPrefix + "TaxonNameProject P ON T.NameID = P.NameID AND A.NameID = P.NameID AND A.ProjectID = P.ProjectID AND P.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + " " +
                    " LEFT OUTER JOIN  " + TaxonDBPrefix + "TaxonCommonName AS C ON T.NameID = C.NameID AND C.LanguageCode = '" + DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower() + "' " +
                    " WHERE (T.IgnoreButKeepForReference = 0) " +
                    " AND (T.DataWithholdingReason IS NULL OR T.DataWithholdingReason = '') " +
                    "GROUP BY T.NameID";
                Dictionary<int, string> dict = new Dictionary<int, string>();
                System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                foreach (System.Data.DataRow R in dt.Rows)
                    dict.Add(int.Parse(R[0].ToString()), R[1].ToString());
                _ListGroupDict.Add(ListID, dict);
            }
            return _ListGroupDict[ListID];
        }


        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> _ListGroups;
        public static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> ListGroups
        {
            get
            {
                if (_ListGroups == null)
                {
                    _ListGroups = new Dictionary<int, List<int>>();
                    foreach(int ListID in List.IpmListIDs)
                    {
                        string SQL = "SELECT [NameID] " +
                            "FROM " + TaxonDBPrefix + "[TaxonNameListAnalysis] " +
                            "A WHERE A.ProjectID = " + ListID.ToString() + " AND A.AnalysisID = " + GroupAnalysisID.ToString();
                        System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                        System.Collections.Generic.List<int> IDs = new List<int>();
                        foreach(System.Data.DataRow r in dt.Rows)
                        {
                            int ID;
                            if (int.TryParse(r[0].ToString(), out ID))
                                IDs.Add(ID);
                        }
                        _ListGroups.Add(ListID, IDs);
                    }
                }
                return _ListGroups;
            }
        }


        //private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, string>> _GroupsInList;
        //public static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> GroupsInList
        //{
        //    get
        //    {
        //        if (_ListGroups == null)
        //        {
        //            _GroupsInList = new Dictionary<int, Dictionary<int, string>>();
        //            foreach (int ListID in List.IpmListIDs)
        //            {
        //                string SQL = "SELECT [NameID] " +
        //                    "FROM[dbo].[TaxonNameListAnalysis] " +
        //                    "A WHERE A.ProjectID = " + ListID.ToString() + " AND A.AnalysisID = " + GroupAnalysisID.ToString();
        //                System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //                System.Collections.Generic.List<int> IDs = new List<int>();
        //                foreach (System.Data.DataRow r in dt.Rows)
        //                {
        //                    int ID;
        //                    if (int.TryParse(r[0].ToString(), out ID))
        //                        IDs.Add(ID);
        //                }
        //                _GroupsInList.Add(ListID, IDs);
        //            }
        //        }
        //        return _GroupsInList;
        //    }
        //}


        //public static System.Collections.Generic.List<int> Children(int ParentNameID, System.Collections.Generic.List<int> List = null)
        //{
        //    if (List == null)
        //    {
        //        List = new System.Collections.Generic.List<int>();

        //    }
        //    return List;
        //}


        //private static System.Data.DataTable _DtHierarchy;
        //private static System.Data.DataTable DtHierarchy()
        //{
        //    if (_DtHierarchy == null)
        //    {
        //        string SQL = "SELECT NameID, NameParentID FROM " + TaxonDBPrefix + "TaxonHierarchy " +
        //            "WHERE  (ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") AND (IgnoreButKeepForReference = 0)";
        //        _DtHierarchy = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL) ;
        //    }
        //    return _DtHierarchy; 
        //}

        //private static System.Collections.Generic.Dictionary<int, int> _Hierarchy;
        //public static System.Collections.Generic.Dictionary<int, int> Hierarchy
        //{
        //    get
        //    {
        //        if (_Hierarchy == null)
        //        {
        //            _Hierarchy = new Dictionary<int, int>();
        //            foreach (System.Data.DataRow R in DtHierarchy().Rows)
        //            {
        //                _Hierarchy.Add(int.Parse(R[0].ToString()), int.Parse(R[1].ToString()));
        //            }
        //        }
        //        return _Hierarchy;
        //    }
        //}

        private static System.Collections.Generic.Dictionary<int, System.Data.DataTable> _DtGroups;
        public static System.Data.DataTable DtGroups(int ListID)
        {
            if (_DtGroups == null)
            {
                _DtGroups = new Dictionary<int, System.Data.DataTable>();
            }
            if (!_DtGroups.ContainsKey(ListID))
            {
                string SQL = "SELECT A.NameID, T.TaxonNameCache " +
                    " FROM " +
                    TaxonDBPrefix + "TaxonNameListAnalysis AS A INNER JOIN" +
                    TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = " + GroupAnalysisID.ToString() + " AND A.AnalysisID = C.AnalysisID INNER JOIN" +
                    TaxonDBPrefix + "TaxonName AS T ON A.NameID = T.NameID INNER JOIN  " +
                    TaxonDBPrefix + "TaxonNameProject P ON T.NameID = P.NameID AND P.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + " " +
                    " WHERE A.ProjectID = " + ListID.ToString() +
                    " ORDER BY A.ProjectID, cast(A.AnalysisValue as int)";
                System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                _DtGroups.Add(ListID, dt);
            }
            return _DtGroups[ListID];
        }

        public static void DtGroupsReset() { _DtGroups = null; }

        #endregion

        #region Hierarchy

        private static System.Data.DataTable _DtHierarchyRoot;
        public static System.Data.DataTable DtHierarchyRoot()
        {
            if (_DtHierarchyRoot == null)
            {
                string SQL = "SELECT H.NameID " +
                    "FROM " + TaxonDBPrefix + "TaxonHierarchy H " +
                    "LEFT OUTER JOIN " + TaxonDBPrefix + "TaxonHierarchy T ON T.NameID = H.NameParentID AND T.ProjectID = H.ProjectID " +
                    "WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") AND (H.IgnoreButKeepForReference = 0) " +
                    "AND T.NameID IS NULL";
                _DtHierarchyRoot = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            }
            return _DtHierarchyRoot;
        }


        //private static System.Collections.Generic.List<int> _HierarchyTrunk;
        //public static System.Collections.Generic.List<int> HierarchyTrunk
        //{
        //    get
        //    {
        //        if ( _HierarchyTrunk == null)
        //        {
        //            _HierarchyTrunk = new List<int>();
        //            foreach(System.Data.DataRow R in DtHierarchyTrunk().Rows)
        //            {
        //                _HierarchyTrunk.Add(int.Parse(R[0].ToString()));
        //            }
        //        }
        //        return _HierarchyTrunk;
        //    }
        //}


        private static System.Data.DataTable _DtHierarchyLeaf;
        private static System.Data.DataTable DtHierarchyLeaf()
        {
            if (_DtHierarchyLeaf == null)
            {
                string SQL = "SELECT DISTINCT H.NameID, H.NameParentID " +
                    "FROM " + TaxonDBPrefix + "TaxonHierarchy H " +
                    "WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") AND (H.IgnoreButKeepForReference = 0) " +
                    "AND H.NameID NOT IN " +
                    "( " +
                    "SELECT NameParentID " +
                    "FROM " + TaxonDBPrefix + "TaxonHierarchy " +
                    "WHERE (ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") AND (IgnoreButKeepForReference = 0) AND NameParentID > 0 " +
                    ")";
                _DtHierarchyLeaf = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            }
            return _DtHierarchyLeaf;
        }

        private static System.Collections.Generic.List<int> _HierarchyEndPoints;
        public static System.Collections.Generic.List<int> HierarchyEndPoints
        {
            get
            {
                if (_HierarchyEndPoints == null)
                {
                    _HierarchyEndPoints = new List<int>();
                    foreach (System.Data.DataRow R in DtHierarchyLeaf().Rows)
                        _HierarchyEndPoints.Add(int.Parse(R[0].ToString()));
                }
                return _HierarchyEndPoints;
            }
        }

        private static System.Collections.Generic.Dictionary<int, int> _NameParents;
        public static int? NameParentID(int NameID)
        {
            if (_NameParents == null)
            {
                _NameParents = new Dictionary<int, int>();
                foreach (System.Data.DataRow R in DtTaxa.Rows)
                {
                    int ID;
                    int ParentID;
                    if (int.TryParse(R["NameID"].ToString(), out ID) && int.TryParse(R["NameParentID"].ToString(), out ParentID) && !_NameParents.ContainsKey(ID))
                        _NameParents.Add(ID, ParentID);
                }
            }
            if (_NameParents.ContainsKey(NameID))
                return _NameParents[NameID];
            return null;
        }

        //private static System.Collections.Generic.Dictionary<int, string> _HierarchyLeaf;
        //public static System.Collections.Generic.Dictionary<int, string> HierarchyLeaf
        //{
        //    get
        //    {
        //        if (_HierarchyLeaf == null)
        //        {
        //            _HierarchyLeaf = new Dictionary<int, string>();
        //            foreach (System.Data.DataRow R in DtHierarchyTrunk().Rows)
        //            {
        //                string Group = "";
        //                _HierarchyLeaf.Add(int.Parse(R[0].ToString()), Group);
        //            }
        //        }
        //        return _HierarchyLeaf;
        //    }
        //}

        //private static string GetGroup(int NameID)
        //{
        //    string Group = "";
        //    if (Hierarchy.ContainsKey(NameID))
        //    {

        //    }
        //    return Group;
        //}

        #endregion

        #region IPM

        private static System.Data.DataTable _DtIPM;
        private static System.Data.DataTable DtIPM()
        {
            if (_DtIPM == null)
            {
                string SQL = "SELECT A.NameID, A.ProjectID AS ListID " +
                    "FROM " + TaxonDBPrefix + "TaxonNameListAnalysis AS A " +
                    "INNER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID " +
                    "WHERE (C.DisplayText = N'IPM') ";
                _DtIPM = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            }
            return _DtIPM;
        }

        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> _IPM;
        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> IPM
        {
            get
            {
                if (_IPM == null)
                {
                    _IPM = new Dictionary<int, List<int>>();
                    foreach (System.Data.DataRow R in DtIPM().Rows)
                    {
                        int NameID; 
                        int ListID; 
                        if (int.TryParse(R[1].ToString(), out ListID) &&
                            int.TryParse(R[0].ToString(), out NameID))
                        { 
                            if (!_IPM.ContainsKey(ListID))
                            {
                                System.Collections.Generic.List<int> ints = new List<int>();
                                ints.Add(NameID);
                                _IPM.Add(ListID, ints);
                            }
                            if (!_IPM[ListID].Contains(NameID))
                                _IPM[ListID].Add(NameID);
                        }
                    }
                }
                return _IPM;
            }
        }

        public static System.Collections.Generic.List<int> IpmGroups(int ListID)
        { 
            if (IPM.ContainsKey(ListID))
                return IPM[ListID];
            return null;
        }

        #endregion

        #region Stages

        private static System.Collections.Generic.Dictionary<int, string> _StageList;
        public static System.Collections.Generic.Dictionary<int, string> StageList
        {
            get
            {
                if (_StageList == null)
                {
                    _StageList = new Dictionary<int, string>();
                    string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();
                    string SQL = "SELECT S.[AnalysisID] , case when V.DisplayText is null then S.[DisplayText] else V.DisplayText end AS DisplayText " +
                        "FROM " + TaxonDBPrefix + "[TaxonNameListAnalysisCategory] S " +
                        "INNER JOIN " + TaxonDBPrefix + "[TaxonNameListAnalysisCategory] P ON S.AnalysisParentID = P.AnalysisID AND (P.DisplayText = 'Stage' OR P.DisplayText = 'Remains') " +
                        "INNER JOIN " + TaxonDBPrefix + "[TaxonNameListAnalysisCategory] I ON P.AnalysisParentID = I.AnalysisID AND I.DisplayText = 'IPM' " +
                        "LEFT OUTER JOIN " + TaxonDBPrefix + "[TaxonNameListAnalysisCategoryValue] V ON S.AnalysisID = V.AnalysisID AND V.AnalysisValue = '" + Language + "'";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                        _StageList.Add(int.Parse(R[0].ToString()), R[1].ToString());
                }
                return _StageList;
            }
        }

        private static System.Collections.Generic.Dictionary<int, string> _Stages;

        private static System.Collections.Generic.Dictionary<int, string> Stages
        {
            get
            {
                if (_Stages == null)
                {
                    _Stages = new Dictionary<int, string>();
                    string SQL = "SELECT   S.AnalysisID, S.DisplayText " +
                        "FROM " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS S " +
                        "INNER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C on S.AnalysisParentID = C.AnalysisID " +
                        "INNER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS I ON C.AnalysisParentID = I.AnalysisID " +
                        "WHERE  (I.DisplayText = N'IPM') AND C.DisplayText IN ('Stage', 'Remains')";
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    foreach(System.Data.DataRow R in dt.Rows)
                    {
                        int ID;
                        if (int.TryParse(R[0].ToString(), out ID))
                            _Stages.Add(ID, R[1].ToString());
                    }
                }
                return _Stages;
            }
        }

        private static System.Collections.Generic.Dictionary<int, int> _StageSorting;

        public static System.Collections.Generic.Dictionary<int, int> StageSorting
        {
            get
            {
                if (_StageSorting == null)
                {
                    _StageSorting = new Dictionary<int, int>();
                    string SQL = "SELECT   S.AnalysisID, S.SortingID " +
                        "FROM " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS S " +
                        "INNER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS C on S.AnalysisParentID = C.AnalysisID " +
                        "INNER JOIN " + TaxonDBPrefix + "TaxonNameListAnalysisCategory AS I ON C.AnalysisParentID = I.AnalysisID " +
                        "WHERE  (I.DisplayText = N'IPM') AND C.DisplayText IN ('Stage', 'Remains')";
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        int ID;
                        int Sort;
                        if (int.TryParse(R[0].ToString(), out ID) && int.TryParse(R[1].ToString(), out Sort))
                            _StageSorting.Add(ID, Sort);
                    }
                }
                return _StageSorting;
            }
        }

        #endregion

        #region Taxa

        private static System.Collections.Generic.Dictionary<TaxonProjectSource, System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon>> _ProjectTaxa;

        private static System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> ProjectTaxa(string DatabaseName, int ProjectID)
        {
            if (_ProjectTaxa == null) { _ProjectTaxa = new Dictionary<TaxonProjectSource, Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon>>(); }
            TaxonProjectSource taxonSource = new TaxonProjectSource();
            taxonSource.DatabaseName = DatabaseName;
            taxonSource.ProjectID = ProjectID;
            if (!_ProjectTaxa.ContainsKey(taxonSource))
            {
                Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> taxa = DiversityWorkbench.Api.Taxon.Taxa.TaxonDict(ProjectID, Settings.Default.DiversityTaxonNamesDatabase);
                if (taxa != null)
                {
                    _ProjectTaxa.Add(taxonSource, taxa);
                }
            }
            if (_ProjectTaxa.ContainsKey(taxonSource))
            {
                if(_ProjectTaxa[taxonSource].Count == 0)
                {
                    Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> taxa = DiversityWorkbench.Api.Taxon.Taxa.TaxonDict(ProjectID, Settings.Default.DiversityTaxonNamesDatabase);
                    if (taxa != null)
                    {
                        _ProjectTaxa.Add(taxonSource, taxa);
                    }
                }
                return _ProjectTaxa[taxonSource];
            }
            else
                return null;
        }

        private static System.Collections.Generic.Dictionary<TaxonChecklistSource, System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon>> _ChecklistTaxa;

        public static System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> ChecklistTaxa(string DatabaseName, int ProjectID, int ChecklistID)
        {
            if (_ChecklistTaxa == null) 
            { _ChecklistTaxa = new Dictionary<TaxonChecklistSource, Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon>>(); }
            TaxonChecklistSource taxonSource = new TaxonChecklistSource();
            taxonSource.DatabaseName = DatabaseName;
            taxonSource.ProjectID = ProjectID;
            taxonSource.ChecklistID = ChecklistID;
            if (!_ChecklistTaxa.ContainsKey(taxonSource))
            {
                Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> taxa = new Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon>();
                _ChecklistTaxa.Add(taxonSource, taxa);

            }
            if (_ChecklistTaxa.ContainsKey(taxonSource))
            {
                if (_ChecklistTaxa[taxonSource].Count == 0)
                {
                    System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> projectTaxa = ProjectTaxa(DatabaseName, ProjectID);
                    if (projectTaxa != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Api.Taxon.Taxon> KV in projectTaxa)
                        {
                            if (KV.Value != null)
                            {
                                if (KV.Value.Checklist.Count > 0)
                                {
                                    foreach(DiversityWorkbench.Api.Taxon.TaxonChecklist c in KV.Value.Checklist)
                                    {
                                        if (c.ChecklistID == ChecklistID)
                                        {
                                            if (c.Analysis != null)
                                            {
                                                foreach(DiversityWorkbench.Api.Taxon.ChecklistAnalysis analysis in c.Analysis)
                                                {
                                                    string[] IPM = analysis.Analysis.Split(new char[] { '|' });
                                                    if (IPM.Length > 2 &&
                                                        IPM[0].Trim().ToLower() == "ipm" &&
                                                        IPM[1].Trim().ToLower() == "stage")
                                                    {
                                                        _ChecklistTaxa[taxonSource].Add(KV.Key, KV.Value);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return _ChecklistTaxa[taxonSource];
            }
            else
                return null;
        }



        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, int>> _TaxonListStages;
        public static System.Collections.Generic.Dictionary<string, int> TaxonListStages(int ListID)
        {
            if (_TaxonListStages == null)
            {
                _TaxonListStages = new Dictionary<int, Dictionary<string, int>>();
            }

            if (_TaxonListStages != null && _TaxonListStages.ContainsKey(ListID))
                return _TaxonListStages[ListID];
            else return new Dictionary<string, int>();
        }

        private static System.Data.DataTable _dtTaxa;

        public static System.Data.DataTable DtTaxa
        {
            get
            {
                if (_dtTaxa == null)
                {
                    string SQL = "SELECT N.NameID, N.TaxonNameCache, N.TaxonomicRank, N.GenusOrSupragenericName, N.SpeciesEpithet, H.NameParentID, R.DisplayOrder AS RankOrder, " +
                        " CASE WHEN C.CommonName IS NULL THEN N.GenusOrSupragenericName + CASE WHEN N.SpeciesEpithet <> '' THEN ' ' + N.SpeciesEpithet ELSE '' END ELSE C.CommonName END AS TaxonName, " +
                        "ROW_NUMBER() OVER (ORDER BY (CASE WHEN C.CommonName IS NULL THEN N.GenusOrSupragenericName + CASE WHEN N.SpeciesEpithet <> '' THEN ' ' + N.SpeciesEpithet ELSE '' END ELSE C.CommonName END) ) AS TaxonNameSorting " +
                        //", N.BasionymAuthors, N.BasionymAuthorsYear, N.CombiningAuthors, N.YearOfPubl, N.NomenclaturalCode, CAST(CASE WHEN A.NameID IS NULL THEN 0 ELSE 1 END AS bit) AS IsAccepted, S.SynNameID" +
                        "FROM " + TaxonDBPrefix + "TaxonName AS N INNER JOIN " +
                        TaxonDBPrefix + "TaxonNameProject AS P ON N.NameID = P.NameID INNER JOIN " +
                        TaxonDBPrefix + "TaxonNameTaxonomicRank_Enum AS R ON N.TaxonomicRank = R.Code INNER JOIN " +
                        TaxonDBPrefix + "TaxonAcceptedName AS A ON P.NameID = A.NameID AND P.ProjectID = A.ProjectID  AND (A.IgnoreButKeepForReference = 0) LEFT OUTER JOIN " +
                        TaxonDBPrefix + "TaxonHierarchy AS H ON P.NameID = H.NameID AND P.ProjectID = H.ProjectID AND (H.IgnoreButKeepForReference = 0) LEFT OUTER JOIN" +
                        TaxonDBPrefix + "TaxonCommonName AS C ON C.NameID = N.NameID AND (C.LanguageCode = '" + DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower() + "')  " +
                        "WHERE(P.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") " +
                        "AND (N.DataWithholdingReason = N'' OR N.DataWithholdingReason IS NULL) AND (N.IgnoreButKeepForReference = 0)";
                    _dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                }
                return _dtTaxa;
            }
        }

        #endregion

        #region JSON

        private static System.Data.DataTable _DtJsonCache;
        private static System.Data.DataTable DtJsonCache
        {
            get
            {
                if(_DtJsonCache == null)
                {
                    string SQL = "SELECT [ID], [Data]" +
                        "FROM " + TaxonDBPrefix + "[JsonCache] J" +
                        "  WHERE ISJSON(J.Data) > 0" +
                        "  AND JSON_VALUE(J.Data, '$[0].Project[0].Project') = 'SNSBnames'";// +
                        //"  AND JSON_VALUE(J.Data, '$[0].Checklist[2].ChecklistID') = '1190'";
                    _DtJsonCache = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL, "", "JsonCache");
                }
                return _DtJsonCache;
            }
        }

        #endregion

    }
}
