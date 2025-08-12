using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiversityCollection.Tasks.IPM;
using System.Windows.Documents;

namespace DiversityCollection.Tasks.Taxa
{
    public class Sorting
    {

        public static int SortingForTaxon(IPM.TaxonSource taxonSource, int NameID, int StageID)
        {
            int Sorting = 0;
            try
            {
                System.Collections.Generic.Dictionary<int, int> dict;
                if (SortingInList.TryGetValue(taxonSource, out dict))
                {
                    if (dict.ContainsKey(NameID))
                    {
                        //int StageSort = StageSorting(StageID);
                        Sorting = SortingForTaxon(dict[NameID], NameID, StageID);
                    }
                    //if (dict.TryGetValue(NameID, out Sorting))
                    //{
                    //    int StageSort = StageSorting(StageID);
                    //    Sorting = SortingForTaxon(StageSort, NameID, StageID);
                    //}
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //if (SortingInList.ContainsKey(taxonSource))
            //{
            //    dict = SortingInList[taxonSource];
            //    if (dict.TryGetValue(NameID, out Sorting))
            //    {
            //        int StageSort = StageSorting(StageID);
            //        Sorting *= 1000;
            //        Sorting += StageSort;
            //    }
            //}
            return Sorting;
        }

        /// <summary>
        /// Returns an int that corresponds to the sorting of a taxon stage based on the given values
        /// </summary>
        /// <param name="Sorting">Sorting of a parent name as stored in the analysis in DTN</param>
        /// <param name="NameID">ID of the name to retrieve the sorting according to the alphabetic order of the names</param>
        /// <param name="StageID">ID of the stage to retrieve the sorting of the stage according to the analysis category in DTN</param>
        /// <returns></returns>
        private static int SortingForTaxon(int Sorting, int NameID, int StageID)
        {
            // this is the main Sort and therefore multiplied with a high number to ensure its superiority
            int Sort = Sorting * 1000000000;
            if (RankOrderForTaxa.ContainsKey(NameID))
            {
                int RankOrder = RankOrderForTaxa[NameID];
                Sort += RankOrder * 1000000;
            }
            if (SortingForTaxa.ContainsKey(NameID))
            {
                // this corresponds to the alphabetic order of the names - second level of sorting and therefore multiplied with a lower number to ensure intermediate level
                int SortTaxon = SortingForTaxa[NameID];
                Sort += SortTaxon * 100;
            }
            Sort += StageSorting(StageID); // this corresponds to the sorting of the stages
            return Sort;
        }


        private static System.Collections.Generic.Dictionary<int, int> _SortingForTaxa;
        private static System.Collections.Generic.Dictionary<int, int> SortingForTaxa
        {
            get
            {
                if (_SortingForTaxa == null)
                {
                    _SortingForTaxa = new Dictionary<int, int>();
                    try
                    {
                        foreach (System.Data.DataRow R in Tasks.Taxa.Database.DtTaxa.Rows)
                        {
                            int NameID;
                            int TaxonNameSorting;
                            if (int.TryParse(R["NameID"].ToString(), out NameID) && int.TryParse(R["TaxonNameSorting"].ToString(), out TaxonNameSorting))
                            {
                                if (!_SortingForTaxa.ContainsKey((int)NameID))
                                    _SortingForTaxa.Add(NameID, TaxonNameSorting);
                                else { 
                                }
                            }
                        }
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

                }
                return _SortingForTaxa;
            }
        }

        private static System.Collections.Generic.Dictionary<int, int> _RankOrderForTaxa;
        private static System.Collections.Generic.Dictionary<int, int> RankOrderForTaxa
        {
            get
            {
                if (_RankOrderForTaxa == null)
                {
                    _RankOrderForTaxa = new Dictionary<int, int>();
                    try
                    {
                        foreach (System.Data.DataRow R in Tasks.Taxa.Database.DtTaxa.Rows)
                        {
                            int NameID;
                            int RankOrder;
                            if (int.TryParse(R["NameID"].ToString(), out NameID) && int.TryParse(R["RankOrder"].ToString(), out RankOrder))
                            {
                                if (!SortingForTaxa.ContainsKey((int)NameID))
                                    _RankOrderForTaxa.Add(NameID, RankOrder);
                                else
                                {
                                }
                            }
                        }
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

                }
                return _RankOrderForTaxa;
            }
        }



        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, int>> _SortingInList;
        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, int>> SortingInList
        {
            get
            {
                if (_SortingInList == null)
                {
                    //// getting the dicts for the lists sorted according to the names
                    //System.Collections.Generic.SortedDictionary<IPM.TaxonSource, Dictionary<int, int>> dictSortNames = new SortedDictionary<IPM.TaxonSource, Dictionary<int, int>>();
                    //foreach (System.Collections.Generic.KeyValuePair<IPM.TaxonSource, int> KV in IPM.TaxonSources)
                    //{
                    //    SortedDictionary<string, int> NamesSorted = new SortedDictionary<string, int>();
                    //    Dictionary<string, int> NameIDsForNamesSorted = new Dictionary<string, int>();
                    //    int ListID = List.ListID(KV.Key);
                    //    int i = 0;
                    //    foreach (System.Collections.Generic.KeyValuePair<string, int> kv in Database.TaxonListStages(ListID))
                    //    {
                    //        Tasks.Taxa.TaxonStage taxon = new TaxonStage(kv.Key, KV.Key);
                    //        if (!NamesSorted.ContainsKey(taxon.DisplayText(false)))
                    //        {
                    //            NamesSorted.Add(taxon.DisplayText(false), i);
                    //            NameIDsForNamesSorted.Add(taxon.DisplayText(false), taxon.NameID);
                    //            i++;
                    //        }
                    //    }
                    //}

                    _SortingInList = new System.Collections.Generic.Dictionary<IPM.TaxonSource, Dictionary<int, int>>();
                    foreach (System.Collections.Generic.KeyValuePair<IPM.TaxonSource, int> KV in IPM.TaxonSources)
                    {
                        // the dictionary for the current
                        Dictionary<int, int> ListSorting = new Dictionary<int, int>();
                        int ListID = List.ListID(KV.Key);
                        System.Collections.Generic.Dictionary<int, int> sorting = Database.SortingInList(ListID);
                        int NameID = 0;
                        System.Collections.Generic.List<int> NameIDs = new List<int>();
                        foreach(int ID in Database.HierarchyEndPoints)
                        {
                            NameID = ID;
                            // getting the parents until a sorting is detected
                            while(!sorting.ContainsKey(NameID))
                            {
                                if(!NameIDs.Contains(NameID))
                                {
                                    NameIDs.Add(NameID);
                                }
                                if (Database.NameParentID(NameID) == null)
                                    break;
                                NameID = (int)Database.NameParentID(NameID);
                            }
                            if (sorting != null && sorting.ContainsKey(NameID))
                            {
                                if (!NameIDs.Contains(NameID))
                                    NameIDs.Add(NameID);
                                int Sort = sorting[NameID];
                                foreach(int nameID in NameIDs)
                                {
                                    if (!ListSorting.ContainsKey(nameID))
                                    {
                                        ListSorting.Add(nameID, Sort);
                                    }
                                }
                            }
                            NameIDs.Clear();
                        }
                        _SortingInList.Add(KV.Key, ListSorting);
                    }
                }
                return _SortingInList;
            }
        }

        private static int StageSorting(int StageID)
        {
            int Sort = 0;
            if (Database.StageSorting.ContainsKey(StageID))
                Sort = Database.StageSorting[StageID];
            return Sort;
        }


        //private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> _SortingTaxaInList;
        //private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, string>> SortingTaxaInList
        //{
        //    get
        //    {
        //        if (_SortingTaxaInList == null)
        //        {
        //            _SortingTaxaInList = new System.Collections.Generic.Dictionary<IPM.TaxonSource, Dictionary<int, string>>();
        //            foreach (System.Collections.Generic.KeyValuePair<IPM.TaxonSource, int> KV in IPM.TaxonSources)
        //            {
        //                // the dictionary for the current
        //                Dictionary<int, string> ListSorting = new Dictionary<int, string>();
        //                int ListID = List.ListID(KV.Key);
        //                System.Collections.Generic.Dictionary<int, string> sorting = Database.SortingInList(ListID);
        //                int NameID = 0;
        //                System.Collections.Generic.List<int> NameIDs = new List<int>();
        //                foreach (int ID in Database.HierarchyEndPoints)
        //                {
        //                    NameID = ID;
        //                    // getting the parents until a sorting is detected
        //                    while (!sorting.ContainsKey(NameID))
        //                    {
        //                        if (!NameIDs.Contains(NameID))
        //                        {
        //                            NameIDs.Add(NameID);
        //                        }
        //                        if (Database.NameParentID(NameID) == null)
        //                            break;
        //                        NameID = (int)Database.NameParentID(NameID);
        //                    }
        //                    if (sorting != null && sorting.ContainsKey(NameID))
        //                    {
        //                        if (!NameIDs.Contains(NameID))
        //                            NameIDs.Add(NameID);
        //                        int Sort = sorting[NameID];
        //                        foreach (int nameID in NameIDs)
        //                        {
        //                            if (!ListSorting.ContainsKey(nameID))
        //                            {
        //                                ListSorting.Add(nameID, Sort);
        //                            }
        //                        }
        //                    }
        //                    NameIDs.Clear();
        //                }
        //                _SortingInList.Add(KV.Key, ListSorting);
        //            }
        //        }
        //        return _SortingInList;
        //    }
        //}




    }
}
