using DiversityWorkbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DiversityCollection.Tasks.Taxa
{

    public struct TaxonGroup
    {
        public int NameID;
        public string GroupName;
        public System.Collections.Generic.Dictionary<int, int> InferiorNameIDs;
    }

    public class Groups
    {

        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> _GroupsInList;
        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> GroupsInList
        {
            get
            {
                if (_GroupsInList == null)
                {
                    _GroupsInList = new System.Collections.Generic.Dictionary<IPM.TaxonSource, Dictionary<int, string>>();
                }
                foreach (System.Collections.Generic.KeyValuePair<IPM.TaxonSource, int> KV in IPM.TaxonSources)
                {
                    if (!_GroupsInList.ContainsKey(KV.Key))
                    {
                        try
                        {
                            // the dictionary for the current
                            Dictionary<int, string> ListGroups = new Dictionary<int, string>();
                            int ListID = List.ListID(KV.Key);
                            System.Collections.Generic.Dictionary<string, int> stages = Database.TaxonListStages(ListID);
                            foreach (System.Collections.Generic.KeyValuePair<string, int> kv in stages)
                            {
                                if (Database.HierarchyEndPoints.Contains(kv.Value))
                                {
                                    string Group = "";
                                    System.Collections.Generic.List<int> IDs = new List<int>();
                                    int CurrentNameID = kv.Value;
                                    IDs.Add(CurrentNameID);
                                    if (Database.GroupsInList(ListID).ContainsKey(CurrentNameID))
                                        Group = Database.GroupsInList(ListID)[CurrentNameID];
                                    while (Group.Length == 0)
                                    {
                                        if (Database.NameParentID(CurrentNameID) != null)
                                        {
                                            CurrentNameID = (int)Database.NameParentID(CurrentNameID);
                                            IDs.Add(CurrentNameID);
                                            if (Database.GroupsInList(ListID).ContainsKey(CurrentNameID))
                                                Group = Database.GroupsInList(ListID)[CurrentNameID];
                                        }
                                        else
                                            break;
                                    }
                                    foreach (int ID in IDs)
                                    {
                                        if (!ListGroups.ContainsKey(ID))
                                            ListGroups.Add(ID, Group);
                                    }
                                }
                            }
                            if (ListGroups.Count > 0)
                                _GroupsInList.Add(KV.Key, ListGroups);
                        }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
                }
                return _GroupsInList;
            }
        }

        public static string GroupForTaxon(IPM.TaxonSource taxonSource, int NameID)
        {
            try
            {
                if (GroupsInList != null && GroupsInList.Count > 0 && GroupsInList.ContainsKey(taxonSource) && GroupsInList[taxonSource].ContainsKey(NameID))
                    return GroupsInList[taxonSource][NameID];
            }catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return "";
        }


        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> _TaxonGroups;
        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> TaxonGroups
        {
            get
            {
                if (_TaxonGroups == null) 
                {
                    _TaxonGroups = new System.Collections.Generic.Dictionary<IPM.TaxonSource, Dictionary<int, string>>();
                    foreach(System.Collections.Generic.KeyValuePair<IPM.TaxonSource, int> KV in IPM.TaxonSources)
                    {
                        initTaxonGroups(KV.Key);
                    }
                }
                return _TaxonGroups;
            }
        }

        private static void initTaxonGroups(IPM.TaxonSource taxonSource)
        {
            try
            {
                if (!_TaxonGroups.ContainsKey(taxonSource))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    _TaxonGroups.Add(taxonSource, dict);
                }
                // starting at the top of the hierarchy
                foreach (int RootNameID in Tasks.Taxa.Hierarchy.HierarchyRootNameIDs)
                {
                    // if the Groups of the lists contains the RootNameID
                    if (GroupNameIDs(taxonSource).Contains(RootNameID))
                    {
                        if (!_TaxonGroups[taxonSource].ContainsKey(RootNameID))
                        {
                            _TaxonGroups[taxonSource].Add(RootNameID, GroupsInList[taxonSource][RootNameID]);
                        }
                    }
                    else // Search in the child nodes
                    {
                        initTaxonGroups(taxonSource, RootNameID);
                    }
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private static void initTaxonGroups(IPM.TaxonSource taxonSource, int ParentNameID)
        {
            try
            {
                foreach (int NameID in Hierarchy.Children(ParentNameID))
                {
                    int ListID = List.ListID(taxonSource);
                    if (Database.ListGroups.ContainsKey(ListID))
                    {
                        System.Collections.Generic.List<int> groupIDs = Database.ListGroups[ListID];
                        if (groupIDs.Contains(NameID))
                        {
                            if  (_TaxonGroups.ContainsKey(taxonSource))
                            {
                                if (!_TaxonGroups[taxonSource].ContainsKey(ParentNameID))
                                {
                                    if (GroupsInList.ContainsKey(taxonSource))
                                    {

                                    }
                                }
                            }
                            if (!_TaxonGroups[taxonSource].ContainsKey(ParentNameID) &&
                                GroupsInList.ContainsKey(taxonSource) &&
                                GroupsInList[taxonSource].ContainsKey(ParentNameID))
                            {
                                _TaxonGroups[taxonSource].Add(ParentNameID, GroupsInList[taxonSource][ParentNameID]);
                            }
                            break;
                        }
                        else initTaxonGroups(taxonSource, NameID);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        //private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> _GroupsInList;
        //private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> GroupsInList
        //{
        //    get
        //    {
        //        if (_GroupsInList == null)
        //        {
        //            _GroupsInList = new Dictionary<IPM.TaxonSource, Dictionary<int, string>>();
        //            try
        //            {
        //                foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in Tasks.Taxa.Database.ListGroups)
        //                {
        //                    System.Collections.Generic.Dictionary<int, string> dict = new Dictionary<int, string>();
        //                    foreach (int NameID in KV.Value)
        //                    {
        //                        string Group = getGroup(IPM.taxonSource(KV.Key), NameID);
        //                        dict.Add(NameID, Group);
        //                    }
        //                    _GroupsInList.Add(IPM.taxonSource(KV.Key), dict);
        //                }
        //            }
        //            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //        }
        //        return _GroupsInList;
        //    }
        //}

        //private static void initTaxonGroups(IPM.TaxonSource taxonSource, int ParentNameID)
        //{

        //}

        public static string GetTaxonGroup(IPM.TaxonSource taxonSource, int NameID)
        {
            if (TaxonGroups.ContainsKey(taxonSource) && TaxonGroups[taxonSource].ContainsKey(NameID))
                return TaxonGroups[taxonSource][NameID];
            else
                return "";
        }

        /// <summary>
        /// Getting the NameIDs of groups
        /// </summary>
        /// <param name="taxonSource">The IPM List in the database, e.g. Pests</param>
        /// <returns>The NameIDs marked as group in the database</returns>
        private static System.Collections.Generic.List<int> GroupNameIDs(IPM.TaxonSource taxonSource)
        {
            if (Database.ListGroups.ContainsKey(List.ListID(taxonSource)))
                return Database.ListGroups[List.ListID(taxonSource)];
            return new System.Collections.Generic.List<int>();
        }


        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int,TaxonGroup>> _ListGroups;

        /// <summary>
        /// Gruppen fuer die Listen Pests und Bycatch wobei der Schlüssel der ID der Liste entspricht
        /// </summary>
        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, TaxonGroup>> ListGroups
        {
            get
            {
                if (_ListGroups == null)
                {
                    _ListGroups = new Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, TaxonGroup>>();
                    foreach(System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in Database.ListGroups)
                    {

                    }
                }
                return _ListGroups;
            }
        }

        /// <summary>
        /// Taxongroups fuer die Listen von Pests und Bycatch
        /// </summary>
        /// <param name="taxonSource">The IPM List in the database, e.g. Pests</param>
        /// <returns></returns>
        private static System.Collections.Generic.Dictionary<int, TaxonGroup> Taxongroups(IPM.TaxonSource taxonSource)
        {
            if (!ListGroups.ContainsKey(taxonSource))
            {
                System.Collections.Generic.Dictionary<int, TaxonGroup> taxonGroup = new System.Collections.Generic.Dictionary<int, TaxonGroup>();
                ListGroups.Add(taxonSource, taxonGroup);
            }
            return ListGroups[taxonSource];
        }

        private static void AddGroup(IPM.TaxonSource taxonSource, int ParentID, string Group, System.Collections.Generic.List<int> Inferiors)
        {
            if(!ListGroups.ContainsKey(taxonSource))
            {
                InitGroups(taxonSource);
            }
            foreach(int ID in Inferiors)
            {
                if (!ListGroups[taxonSource][ParentID].InferiorNameIDs.ContainsKey(ID))
                    ListGroups[taxonSource][ParentID].InferiorNameIDs.Add(ID, ParentID);
            }
        }

        public static void InitGroups(IPM.TaxonSource taxonSource)
        {
            try
            {
                int ListID = 1190;
                switch (taxonSource)
                {
                    case IPM.TaxonSource.Pest:
                        ListID = Settings.Default.DiversityTaxonNamesPestListID;
                        break;
                    case IPM.TaxonSource.Beneficial:
                        ListID = Settings.Default.DiversityTaxonNamesBeneficialListID;
                        break;
                    case IPM.TaxonSource.Bycatch:
                        ListID = Settings.Default.DiversityTaxonNamesBycatchListID;
                        break;
                }
                string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();

                string SQL = "SELECT CASE WHEN GC.CommonName IS NULL THEN G.GenusOrSupragenericName ELSE GC.CommonName END AS Gruppe, H.NameID " +
                        "FROM " + Database.TaxonDBPrefix + "TaxonHierarchy AS H " +
                        "INNER JOIN " + Database.TaxonDBPrefix + "TaxonName AS G ON H.NameID = G.NameID AND H.ProjectID = 380 " +
                        "LEFT OUTER JOIN " + Database.TaxonDBPrefix + "TaxonCommonName AS GC ON G.NameID = GC.NameID AND GC.LanguageCode = '" + Language + "' " +
                        "INNER JOIN " + Database.TaxonDBPrefix + "TaxonNameList AS L ON H.NameID = L.NameID AND L.ProjectID = " + ListID.ToString() + " " +
                        "INNER JOIN " + Database.TaxonDBPrefix + "TaxonNameListAnalysis AS A ON A.NameID = L.NameID AND A.ProjectID = L.ProjectID AND A.AnalysisID = 1";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        int ParentID = 0;
                        int.TryParse(R["NameID"].ToString(), out ParentID);
                        SQL = "SELECT H.NameID " +
                        "FROM " + Database.TaxonDBPrefix + "TaxonHierarchy AS H " +
                        "WHERE H.NameParentID = " + ParentID.ToString() + " AND H.ProjectID = 380";
                        System.Data.DataTable dtInferior = new System.Data.DataTable();
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtInferior);
                        System.Collections.Generic.Dictionary<int, int> Inferiors = new Dictionary<int, int>();
                        foreach (System.Data.DataRow r in dtInferior.Rows)
                        {
                            int i;
                            if (int.TryParse(r[0].ToString(), out i) && !Inferiors.ContainsKey(i))
                            Inferiors.Add(i, ParentID);
                        }
                        if (!ListGroups.ContainsKey(taxonSource))
                        {
                            System.Collections.Generic.Dictionary<int, TaxonGroup> taxa = new Dictionary<int, TaxonGroup>();
                            ListGroups.Add(taxonSource, taxa);
                        }
                        if (!ListGroups[taxonSource].ContainsKey(ParentID))
                        {
                            TaxonGroup group = new TaxonGroup();
                            group.GroupName = R["Gruppe"].ToString();
                            group.InferiorNameIDs = Inferiors;
                            group.NameID = ParentID;
                            ListGroups[taxonSource].Add(ParentID, group);
                        }
                    }
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public static string getGroup(IPM.TaxonSource taxonSource, int NameID, System.Collections.Generic.Dictionary<int, int> Inferiors = null)
        {
            string Group = "";


            try
            {
                int ParentID = 0;
                string SQL = "SELECT H.NameParentID " +
                    "FROM " + Database.TaxonDBPrefix + "TaxonHierarchy AS H WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") " +
                    " AND H.NameID = " + NameID.ToString();
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ParentID))
                {
                    if (ParentID == -1) 
                    {
                        SQL = "SELECT TOP 1 CASE WHEN GC.CommonName IS NULL THEN G.GenusOrSupragenericName ELSE GC.CommonName END AS Gruppe " +
                            "FROM " + Database.TaxonDBPrefix + "TaxonName AS G " +
                            "LEFT OUTER JOIN " + Database.TaxonDBPrefix + "TaxonCommonName AS GC ON G.NameID = GC.NameID AND GC.LanguageCode = '" + DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower() + "' " +
                            "WHERE G.NameID = " + NameID.ToString();
                        Group = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                    }
                    if (!ListGroups.ContainsKey(taxonSource))
                    {
                        InitGroups(taxonSource);
                    }
                    if (ListGroups[taxonSource].ContainsKey(ParentID))
                    {
                        if (!ListGroups[taxonSource][ParentID].InferiorNameIDs.ContainsKey(NameID))
                            ListGroups[taxonSource][ParentID].InferiorNameIDs.Add(NameID, ParentID);
                        Group = ListGroups[taxonSource][ParentID].GroupName;
                    }
                    else
                    {
                        foreach(System.Collections.Generic.KeyValuePair<int, TaxonGroup> KV in ListGroups[taxonSource])
                        {
                            if (KV.Value.InferiorNameIDs.ContainsKey(NameID) || KV.Value.InferiorNameIDs.ContainsKey(ParentID))
                            {
                                Group = KV.Value.GroupName;
                                break;
                            }
                        }
                        if (Group.Length == 0)
                        {
                            if (Inferiors == null) Inferiors = new Dictionary<int, int>();
                            if (!Inferiors.ContainsKey(NameID))
                                Inferiors.Add(NameID, ParentID);
                            Group = getGroup(taxonSource, ParentID, Inferiors);
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Group;
        }


    }
}
