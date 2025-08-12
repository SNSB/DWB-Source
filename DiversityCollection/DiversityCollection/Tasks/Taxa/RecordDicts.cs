using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks.Taxa
{
    public class RecordDicts
    {
        public static void Reset()
        {
            _ChecklistRecords = null;
        }

        #region Old design
        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<string, Tasks.Taxa.TaxonRecord>> _ChecklistRecords;

        /// <summary>
        /// The Taxa including their stages and remains as defined in DTN for IPM
        /// </summary>
        /// <param name="taxonSource">Pest or Bycatch</param>
        /// <returns>Dictionary containing the Uris and the records as defined in DTN</returns>
        //public static System.Collections.Generic.Dictionary<string, Record> ChecklistRecords(IPM.TaxonSource taxonSource)
        //{
        //    if (_ChecklistRecords == null)
        //        _ChecklistRecords = new Dictionary<IPM.TaxonSource, Dictionary<string, Record>>();
        //    if (!_ChecklistRecords.ContainsKey(taxonSource))
        //    {
        //        int listID = IPM.ListID(taxonSource);
        //        int ProjectID = Settings.Default.DiversityTaxonNamesProjectID;
        //        System.Collections.Generic.Dictionary<string, Record> records = new Dictionary<string, Record>();
        //        // records where something is missing, e.g. the group that should be set in a second trial
        //        System.Collections.Generic.List<string> RecordsToFix = new List<string>();
        //        System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> Taxa = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(Settings.Default.DiversityTaxonNamesDatabase, listID, Settings.Default.DiversityTaxonNamesProjectID);
        //        foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Api.Taxon.Taxon> KV in Taxa)
        //        {
        //            try
        //            {
        //                if (KV.Value == null) continue;

        //                System.Collections.Generic.List<DiversityWorkbench.Api.Taxon.TaxonChecklist> cc = KV.Value.Checklist;
        //                foreach (DiversityWorkbench.Api.Taxon.TaxonChecklist CL in cc)
        //                {
        //                    if (CL.Analysis != null)
        //                    {
        //                        foreach (DiversityWorkbench.Api.Taxon.ChecklistAnalysis checklistAnalysis in CL.Analysis)
        //                        {
        //                            string[] IPM = checklistAnalysis.Analysis.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //                            if (IPM.Length > 2 && IPM[0].Trim().ToLower() == "ipm" && (IPM[1].Trim().ToLower() == "stage" || IPM[1].Trim().ToLower() == "remains"))
        //                            {
        //                                string Identifier = KV.Value.URL + "|" + checklistAnalysis.AnalysisID.ToString();
        //                                if (!records.ContainsKey(Identifier))
        //                                {
        //                                    Record record = new Record(Identifier, KV.Value, taxonSource);
        //                                    if (IPM[1].Trim().ToLower() == "stage" && record.States.Count == 0 && record.Stage != null)
        //                                    {

        //                                    }
        //                                    records.Add(Identifier, record);
        //                                    if (record.Group == null || !record.PreviewExists)
        //                                    {
        //                                        RecordsToFix.Add(Identifier);
        //                                    }
        //                                }
        //                                else { }
        //                            }
        //                            else { }
        //                        }
        //                    }
        //                    else { }
        //                }
        //            }
        //            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //        }

        //        if (RecordsToFix.Count > 0)
        //        {
        //            foreach (string R in RecordsToFix)
        //                records[R].SetParamters();
        //        }
        //        _ChecklistRecords.Add(taxonSource, records);
        //    }
        //    if (_ChecklistRecords.ContainsKey(taxonSource)) return _ChecklistRecords[taxonSource];
        //    return new System.Collections.Generic.Dictionary<string, Record>();// null;
        //}


        private static System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<string, TaxonRecord>> _SelectedRecords;
        //public static System.Collections.Generic.Dictionary<string, Record> ChecklistSelectedRecords(IPM.TaxonSource taxonSource, bool Reset = false)
        //{
        //    if (Reset) _SelectedRecords = null;
        //    if (_SelectedRecords == null) _SelectedRecords = new Dictionary<IPM.TaxonSource, Dictionary<string, Record>>();
        //    if (!_SelectedRecords.ContainsKey(taxonSource))
        //    {
        //        System.Collections.Generic.Dictionary<string, Record> selectedRecords = new Dictionary<string, Record>();
        //        System.Collections.Specialized.StringCollection uris = IPM.NameUris(taxonSource);
        //        //switch (taxonSource)
        //        //{
        //        //    case IPM.TaxonSource.Pest: uris = Settings.Default.PestNameUris; break;
        //        //    case IPM.TaxonSource.Bycatch: uris = Settings.Default.BycatchNameUris; break;
        //        //}
        //        foreach (System.Collections.Generic.KeyValuePair<string, Record> KV in ChecklistRecords(taxonSource))
        //        {
        //            if (uris.Contains(KV.Key))
        //                selectedRecords.Add(KV.Key, KV.Value);
        //        }
        //        _SelectedRecords.Add(taxonSource, selectedRecords);
        //    }
        //    return _SelectedRecords[taxonSource];
        //}

        #endregion

        private static System.Collections.Generic.Dictionary<IPM.RecordingTarget, System.Collections.Generic.Dictionary<string, Tasks.Taxa.TaxonRecord>> _ChecklistRecordings;
        public static System.Collections.Generic.Dictionary<string, TaxonRecord> ChecklistRecordings(IPM.RecordingTarget recordingTarget)
        {
            if (_ChecklistRecordings == null)
                _ChecklistRecordings = new Dictionary<IPM.RecordingTarget, Dictionary<string, TaxonRecord>>();
            if (!_ChecklistRecordings.ContainsKey(recordingTarget))
            {
                int listID = IPM.ListID(recordingTarget);
                int ProjectID = Settings.Default.DiversityTaxonNamesProjectID;
                System.Collections.Generic.Dictionary<string, TaxonRecord> records = new Dictionary<string, TaxonRecord>();
                System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> Taxa = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(Settings.Default.DiversityTaxonNamesDatabase, listID, Settings.Default.DiversityTaxonNamesProjectID);
                foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Api.Taxon.Taxon> KV in Taxa)
                {
                    try
                    {
                        if (KV.Value == null) continue;

                        System.Collections.Generic.List<DiversityWorkbench.Api.Taxon.TaxonChecklist> cc = KV.Value.Checklist;
                        foreach (DiversityWorkbench.Api.Taxon.TaxonChecklist CL in cc)
                        {
                            if (CL.Analysis != null)
                            {
                                foreach (DiversityWorkbench.Api.Taxon.ChecklistAnalysis checklistAnalysis in CL.Analysis)
                                {
                                    string[] IPM = checklistAnalysis.Analysis.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (IPM.Length > 2 && IPM[0].Trim().ToLower() == "ipm" && (IPM[1].Trim().ToLower() == "stage" || IPM[1].Trim().ToLower() == "remains"))
                                    {
                                        string Identifier = KV.Value.URL + "|" + checklistAnalysis.AnalysisID.ToString();
                                        if (!records.ContainsKey(Identifier))
                                        {
                                            TaxonRecord record = new TaxonRecord(Identifier, KV.Value, recordingTarget);
                                            records.Add(Identifier, record);
                                        }
                                        else { }
                                    }
                                    else { }
                                }
                            }
                            else { }
                        }
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
                _ChecklistRecordings.Add(recordingTarget, records);
            }
            if (_ChecklistRecordings.ContainsKey(recordingTarget)) return _ChecklistRecordings[recordingTarget];
            return new System.Collections.Generic.Dictionary<string, TaxonRecord>();// null;
        }

        private static System.Collections.Generic.Dictionary<IPM.RecordingTarget, System.Collections.Generic.Dictionary<string, TaxonRecord>> _SelectedRecordings;

        public static System.Collections.Generic.Dictionary<string, TaxonRecord> ChecklistSelectedRecordings(IPM.RecordingTarget recordingTarget, bool Reset = false)
        {
            if (Reset) _SelectedRecordings = null;
            if (_SelectedRecordings == null) _SelectedRecordings = new Dictionary<IPM.RecordingTarget, Dictionary<string, TaxonRecord>>();
            if (!_SelectedRecordings.ContainsKey(recordingTarget))
            {
                System.Collections.Generic.Dictionary<string, TaxonRecord> selectedRecords = new Dictionary<string, TaxonRecord>();
                System.Collections.Specialized.StringCollection uris = IPM.NameUris(recordingTarget);
                foreach (System.Collections.Generic.KeyValuePair<string, TaxonRecord> KV in ChecklistRecordings(recordingTarget))
                {
                    if (uris.Contains(KV.Key))
                        selectedRecords.Add(KV.Key, KV.Value);
                }
                _SelectedRecordings.Add(recordingTarget, selectedRecords);
            }
            return _SelectedRecordings[recordingTarget];
        }


    }
}
