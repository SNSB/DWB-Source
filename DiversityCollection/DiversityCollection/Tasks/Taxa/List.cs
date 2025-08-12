using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks.Taxa
{
    public class List
    {
        private static System.Collections.Generic.List<int> _IpmListIDs;
        public static System.Collections.Generic.List<int> IpmListIDs
        {
            get
            {
                if(_IpmListIDs == null)
                {
                    _IpmListIDs = new List<int>();
                    _IpmListIDs.Add(Settings.Default.DiversityTaxonNamesPestListID);
                    _IpmListIDs.Add(Settings.Default.DiversityTaxonNamesBeneficialListID);
                    _IpmListIDs.Add(Settings.Default.DiversityTaxonNamesBycatchListID);
                }
                return _IpmListIDs;
            }
        }

        public static IPM.TaxonSource TaxonSource(int ListID)
        {
            IPM.TaxonSource taxonSource = IPM.TaxonSource.Pest;
            if (ListID == Settings.Default.DiversityTaxonNamesBeneficialListID)
                taxonSource = IPM.TaxonSource.Beneficial;
            else if (ListID == Settings.Default.DiversityTaxonNamesBycatchListID)
                taxonSource = IPM.TaxonSource.Bycatch;
            return taxonSource;
        }

        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<string, Tasks.Taxa.TaxonStage>> _Lists;

        public static System.Collections.Generic.Dictionary<string, TaxonStage> TaxonStageDict(IPM.TaxonSource taxonSource)
        {
            if (_Lists == null) 
                _Lists = new Dictionary<IPM.TaxonSource, Dictionary<string, TaxonStage>>();
            if (!_Lists.ContainsKey(taxonSource))
            {
                int listID = ListID(taxonSource);
                int ProjectID = Settings.Default.DiversityTaxonNamesProjectID;
                System.Collections.Generic.Dictionary<string, TaxonStage> stages = new Dictionary<string, TaxonStage>();
                System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> Taxa = Database.ChecklistTaxa(Settings.Default.DiversityTaxonNamesDatabase, Settings.Default.DiversityTaxonNamesProjectID, listID);
                foreach(System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Api.Taxon.Taxon> KV in Taxa)
                {
                    try
                    {
                        if (KV.Value == null) continue;

                        if (KV.Value.Checklist.Any(c => c.ChecklistID == listID))
                        {
                            DiversityWorkbench.Api.Taxon.TaxonChecklist CL = KV.Value.Checklist.Find(c => c.ChecklistID == listID);
                            if (CL.Analysis != null)
                            {
                                foreach(DiversityWorkbench.Api.Taxon.ChecklistAnalysis checklistAnalysis in CL.Analysis)
                                {
                                    string[] IPM = checklistAnalysis.Analysis.Split(new char[] { '|' },StringSplitOptions.RemoveEmptyEntries);
                                    if (IPM.Length > 2 && IPM[0].Trim().ToLower() == "ipm" && IPM[1].Trim().ToLower() == "stage")
                                    {
                                        string Identifier = KV.Value.URL + "|" + checklistAnalysis.AnalysisID.ToString();
                                        TaxonStage taxonStage = new TaxonStage(Identifier, taxonSource);
                                        stages.Add(Identifier, taxonStage);
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
                _Lists.Add(taxonSource, stages);


                // Alter Code
                if (false)
                {
                    System.Collections.Generic.Dictionary<string, Tasks.Taxa.TaxonStage> taxa = new Dictionary<string, TaxonStage>();
                    System.Collections.Generic.SortedDictionary<int, string> taxaSorted = new SortedDictionary<int, string>();
                    System.Collections.Generic.Dictionary<string, Tasks.Taxa.TaxonStage> SortedTaxa = new Dictionary<string, TaxonStage>();
                    try
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, int> KV in Database.TaxonListStages(listID))
                        {
                            Tasks.Taxa.TaxonStage taxon = new TaxonStage(KV.Key, taxonSource);
                            //int Sorting = Taxa.Sorting.SortingForTaxon(taxonSource, taxon.NameID, taxon.StageID);
                            if (!taxaSorted.ContainsKey(taxon.Sorting))
                            {
                                taxa.Add(taxon.Identifier, taxon);
                                taxaSorted.Add(taxon.Sorting, taxon.Identifier);
                            }
                        }
                        foreach (System.Collections.Generic.KeyValuePair<int, string> KV in taxaSorted)
                        {
                            if (!SortedTaxa.ContainsKey(taxa[KV.Value].Identifier))
                            {
                                SortedTaxa.Add(taxa[KV.Value].Identifier, taxa[KV.Value]);
                            }
                        }
                        _Lists.Add(taxonSource, SortedTaxa);
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }

                //if (SortedSystem.Collections.Generic.KeyValuePair<string, int> KV
                //{
                //    // getting a dict sorted according to the names
                //    System.Collections.Generic.SortedDictionary<string, Tasks.Taxa.TaxonStage> dictSortNames = new SortedDictionary<string, Tasks.Taxa.TaxonStage>();
                //    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in Database.TaxonListStages(listID))
                //    {
                //        Tasks.Taxa.TaxonStage taxon = new TaxonStage(KV.Key, taxonSource);
                //        dictSortNames.Add(taxon.DisplayText(), taxon);
                //    }
                //    int Max = dictSortNames.Count;

                //    // getting a dict with nameID and position in sorted list
                //    System.Collections.Generic.SortedDictionary<int, int> dictNameSequence = new SortedDictionary<int, int>();
                //    int i = 0;
                //    foreach (System.Collections.Generic.KeyValuePair<string, Tasks.Taxa.TaxonStage> KV in dictSortNames)
                //    {
                //        if (!dictNameSequence.ContainsKey(KV.Value.NameID))
                //        {
                //            dictNameSequence.Add(KV.Value.NameID, i);
                //            i++;
                //        }
                //    }


                //    System.Collections.Generic.SortedDictionary<int, Tasks.Taxa.TaxonStage> dictSort = new SortedDictionary<int, Tasks.Taxa.TaxonStage>();
                //    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in Database.TaxonListStages(listID))
                //    {
                //        Tasks.Taxa.TaxonStage taxon = new TaxonStage(KV.Key, taxonSource);
                //        int Sort = taxon.Sorting;
                //        while(dictSort.ContainsKey(Sort)) 
                //            Sort++;
                //        dictSort.Add(Sort, taxon);
                //    }
                //    foreach (System.Collections.Generic.KeyValuePair<int, Tasks.Taxa.TaxonStage> KV in dictSort)
                //    {
                //        taxa.Add(KV.Value.Identifier, KV.Value);
                //    }
                //}
                //else
                //{
                //    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in Database.TaxonListStages(listID))
                //    {
                //        Tasks.Taxa.TaxonStage taxon = new TaxonStage(KV.Key, taxonSource);
                //        taxa.Add(KV.Key, taxon);
                //    }
                //}
                //_Lists.Add(IPM.taxonSource(listID), taxa);

                //string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();
                //string Stages = "";
                //foreach(int ID in Taxa.Stages.StageIDs)
                //{
                //    if (Stages.Length > 0) Stages += ",";
                //    Stages += ID.ToString();
                //}
                ////Dictionary<int, int> acceptedIDs = Database.AcceptedNames;

                //string SQL = "SELECT B.BaseURL + CAST(P.NameID AS varchar) + CASE WHEN A.AnalysisID IS NULL THEN '' ELSE '|' + CAST(A.AnalysisID AS varchar) END AS TaxonIdentifier " +
                //        " FROM " + Database.TaxonDBPrefix + "TaxonName AS T " +
                //        " INNER JOIN " + Database.TaxonDBPrefix + "TaxonNameProject AS P ON T.NameID = P.NameID AND P.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() +
                //        " INNER JOIN " + Database.TaxonDBPrefix + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = " + listID.ToString() + " " +
                //        " LEFT OUTER JOIN " + Database.TaxonDBPrefix + "TaxonNameListAnalysis AS A ON A.NameID = L.NameID AND A.ProjectID = L.ProjectID AND A.AnalysisID IN( " + Stages + ") " +
                //        " CROSS JOIN " + Database.TaxonDBPrefix + "ViewBaseURL B";
                //System.Data.DataTable dt = new System.Data.DataTable();
                //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                //Dictionary<string, Taxon> taxa = new Dictionary<string, Taxon>();
                //foreach (System.Data.DataRow R in dt.Rows)
                //{
                //    Taxon taxon = new Taxon(R[0].ToString(), taxonSource);
                //    //if (acceptedIDs.ContainsKey(taxon.NameID))
                //    //    taxon.AcceptedNameID = acceptedIDs[taxon.NameID];
                //    taxa.Add(R[0].ToString(), taxon);
                //}
                //_Lists.Add(taxonSource, taxa);
            }
            if (_Lists.ContainsKey(taxonSource)) return _Lists[taxonSource];
            return new System.Collections.Generic.Dictionary<string, TaxonStage>();// null;
        }

        //private static Dictionary<int, int> AcceptedNames(IPM.TaxonSource taxonSource)
        //{
        //    Dictionary<int, int> ids = new Dictionary<int, int>();
        //    System.Data.DataTable dtAccepted = new System.Data.DataTable();
        //    string SQL = "SELECT T.NameID, CASE WHEN S.AcceptedNameID IS NULL THEN A.NameID ELSE S.AcceptedNameID END AS AcceptedNameID " +
        //        " FROM " + Database.TaxonDBPrefix + "TaxonName AS T " +
        //        " LEFT OUTER  JOIN " + Database.TaxonDBPrefix + "TaxonAcceptedName AS A ON T.NameID = A.NameID AND A.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() +
        //        " INNER JOIN " + Database.TaxonDBPrefix + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = " + ListID(taxonSource) +
        //        " LEFT OUTER JOIN " + Database.TaxonDBPrefix + "[TaxonSynonymyAcceptedNameID] AS S ON S.NameID = A.NameID AND S.ProjectID = A.ProjectID";
        //    SQL += " WHERE (A.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") ";
        //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtAccepted);
        //    foreach(System.Data.DataRow R in dtAccepted.Rows)
        //    {
        //        int nameID;
        //        int acceptedID;
        //        if (int.TryParse(R[0].ToString(), out nameID))
        //        {
        //            if (!int.TryParse(R[1].ToString(), out acceptedID)) acceptedID = nameID;
        //            ids.Add(nameID, acceptedID);
        //        }
        //    }
        //    return ids;
        //}

        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, Tasks.Taxa.Taxon>> _TaxonDicts;

        public static System.Collections.Generic.Dictionary<int, Taxon> TaxonDict(IPM.TaxonSource taxonSource)
        {
            if (_TaxonDicts == null)
                _TaxonDicts = new Dictionary<IPM.TaxonSource, Dictionary<int, Taxon>>();
            if (!_TaxonDicts.ContainsKey(taxonSource))
            {
                int listID = ListID(taxonSource);
                int ProjectID = Settings.Default.DiversityTaxonNamesProjectID;
                System.Collections.Generic.Dictionary<int, Taxon> taxonDict = new Dictionary<int, Taxon>();
                System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> Taxa = Database.ChecklistTaxa(Settings.Default.DiversityTaxonNamesDatabase, Settings.Default.DiversityTaxonNamesProjectID, listID);
                foreach(System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Api.Taxon.Taxon> KV in Taxa)
                {

                }

                //foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Api.Taxon.Taxon> KV in Taxa)
                //{
                //    if (!taxonDict.ContainsKey(KV.Key))
                //    {
                //        try
                //        {
                //            if (KV.Value == null) continue;

                //            if (KV.Value.Checklist.Any(c => c.ChecklistID == listID))
                //            {
                //                DiversityWorkbench.Api.Taxon.TaxonChecklist CL = KV.Value.Checklist.Find(c => c.ChecklistID == listID);
                //                if (CL.Analysis != null)
                //                {
                //                    foreach (DiversityWorkbench.Api.Taxon.ChecklistAnalysis checklistAnalysis in CL.Analysis)
                //                    {
                //                        string[] IPM = checklistAnalysis.Analysis.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //                        if (IPM.Length > 2 && IPM[0].Trim().ToLower() == "ipm" && IPM[1].Trim().ToLower() == "stage")
                //                        {
                //                            Taxon taxon = new Taxon(KV.Key, taxonSource);
                //                            taxonDict.Add(KV.Key, taxon);
                //                        }
                //                        else
                //                        {

                //                        }
                //                    }
                //                }
                //                else
                //                {

                //                }
                //            }
                //            else
                //            {

                //            }
                //        }
                //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                //    }
                //}
                _TaxonDicts.Add(taxonSource, taxonDict);
            }
            if (_Lists.ContainsKey(taxonSource)) return _TaxonDicts[taxonSource];
            return new System.Collections.Generic.Dictionary<int, Taxon>();// null;
        }



        public static int ListID(IPM.TaxonSource taxonSource)
        {
            int listID = 1190;
            switch (taxonSource)
            {
                case IPM.TaxonSource.Pest:
                    listID = Settings.Default.DiversityTaxonNamesPestListID;
                    break;
                case IPM.TaxonSource.Beneficial:
                    listID = Settings.Default.DiversityTaxonNamesBeneficialListID;
                    break;
                case IPM.TaxonSource.Bycatch:
                    listID = Settings.Default.DiversityTaxonNamesBycatchListID;
                    break;
            }
            return listID;
        }

        public static System.Collections.Generic.Dictionary<string, TaxonStage> SelectedTaxa(IPM.TaxonSource taxonSource)
        {
            System.Collections.Generic.Dictionary<string, TaxonStage> taxa = new Dictionary<string, TaxonStage>();
            foreach(System.Collections.Generic.KeyValuePair<string, TaxonStage> KV in TaxonStageDict(taxonSource))
            {
                bool Selected = false;
                switch (taxonSource)
                {
                    case IPM.TaxonSource.Pest:
                        if (Settings.Default.PestNameUris != null)
                            Selected = Settings.Default.PestNameUris.Contains(KV.Key);
                        else Selected = true;
                        break;
                    case IPM.TaxonSource.Beneficial:
                        if (Settings.Default.BeneficialNameUris != null)
                            Selected = Settings.Default.BeneficialNameUris.Contains(KV.Key);
                        else Selected = true;
                        break;
                    case IPM.TaxonSource.Bycatch:
                        if (Settings.Default.BycatchNameUris != null)
                            Selected = Settings.Default.BycatchNameUris.Contains(KV.Key);
                        else Selected = true;
                        break;
                }
                if (Selected) taxa.Add(KV.Key, KV.Value);
            }
            return taxa;
        }

        //public static System.Collections.Generic.Dictionary<string, Record> SelectedRecords(IPM.TaxonSource taxonSource)
        //{
        //    System.Collections.Generic.Dictionary<string, Record> taxa = new Dictionary<string, Record>();
        //    foreach (System.Collections.Generic.KeyValuePair<string, Record> KV in RecordDicts.ChecklistRecords(taxonSource))
        //    {
        //        bool Selected = false;
        //        switch (taxonSource)
        //        {
        //            case IPM.TaxonSource.Pest:
        //                if (Settings.Default.PestNameUris != null)
        //                    Selected = Settings.Default.PestNameUris.Contains(KV.Key);
        //                else Selected = true;
        //                break;
        //            case IPM.TaxonSource.Beneficial:
        //                if (Settings.Default.BeneficialNameUris != null)
        //                    Selected = Settings.Default.BeneficialNameUris.Contains(KV.Key);
        //                else Selected = true;
        //                break;
        //            case IPM.TaxonSource.Bycatch:
        //                if (Settings.Default.BycatchNameUris != null)
        //                    Selected = Settings.Default.BycatchNameUris.Contains(KV.Key);
        //                else Selected = true;
        //                break;
        //        }
        //        if (Selected) taxa.Add(KV.Key, KV.Value);
        //    }
        //    return taxa;
        //}


    }
}
