using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class Taxa
    {

        private System.Collections.Generic.Dictionary<int, Taxon> _taxons;
        private string _ConnectionString;
        private int _ProjectID;

        private static System.Collections.Generic.Dictionary<int, Taxon> _ProjectTaxa;
        public static System.Collections.Generic.Dictionary<int, Taxon> TaxonDict(int ProjectID, string Database)
        {
            if (_ProjectTaxa == null)
            {
                try
                {
                    _ProjectTaxa = new Dictionary<int, Taxon>();
                    string Restriction = "";
                    string ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                    Restriction = " T INNER JOIN " + Database + ".dbo.TaxonNameProject P ON P.NameID = T.ID AND P.ProjectID = " + ProjectID.ToString();
                    System.Data.DataTable dtJson = DiversityWorkbench.Api.JsonCache.DtJson(Database, ConnectionString, Restriction);
                    foreach (System.Data.DataRow dr in dtJson.Rows)
                    {
                        try
                        {
                            string Json = dr["Data"].ToString();
                            var taxon = System.Text.Json.JsonSerializer.Deserialize<List<DiversityWorkbench.Api.Taxon.Taxon>>(Json);
                            int ID;
                            if (int.TryParse(dr["ID"].ToString(), out ID) && taxon[0].GetType() == typeof(DiversityWorkbench.Api.Taxon.Taxon))
                            {
                                DiversityWorkbench.Api.Taxon.Taxon Taxon = (DiversityWorkbench.Api.Taxon.Taxon)taxon[0];
                                _ProjectTaxa.Add(ID, Taxon);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            return _ProjectTaxa;
        }

        public static System.Collections.Generic.Dictionary<string, Taxon> ProjectTaxa(int ProjectID, string Database)
        {
            System.Collections.Generic.Dictionary<string, Taxon> taxa = new Dictionary<string, Taxon>();
            foreach(System.Collections.Generic.KeyValuePair<int, Taxon> KV in TaxonDict(ProjectID, Database))
            {
                if (!taxa.ContainsKey(KV.Value.URL))
                    taxa.Add(KV.Value.URL, KV.Value);
            }
            return taxa;
        }



        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, Taxon>> _ChecklistTaxa;

        public static System.Collections.Generic.Dictionary<int, Taxon> ChecklistTaxa(string Database, int ChecklistID, int? ProjectID = null)
        {
            if (_ChecklistTaxa == null) _ChecklistTaxa = new Dictionary<int, Dictionary<int, Taxon>>();
            if (!_ChecklistTaxa.ContainsKey(ChecklistID))
            {
                System.Collections.Generic.Dictionary<int, Taxon> taxa = new Dictionary<int, Taxon>();
                string Restriction = " T INNER JOIN " + Database + ".dbo.TaxonNameList L ON L.NameID = T.ID AND L.ProjectID = " + ChecklistID.ToString();
                if (ProjectID != null)
                {
                    Restriction += " INNER JOIN " + Database + ".dbo.TaxonNameProject P ON P.NameID = T.ID AND P.ProjectID = " + ProjectID.ToString();
                }
                string ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                System.Data.DataTable dtJson = DiversityWorkbench.Api.JsonCache.DtJson(Database, ConnectionString, Restriction);
                foreach (System.Data.DataRow dr in dtJson.Rows)
                {
                    try
                    {
                        string Json = dr["Data"].ToString();
                        var taxon = System.Text.Json.JsonSerializer.Deserialize<List<DiversityWorkbench.Api.Taxon.Taxon>>(Json);
                        int ID;
                        if (int.TryParse(dr["ID"].ToString(), out ID) && taxon[0].GetType() == typeof(DiversityWorkbench.Api.Taxon.Taxon))
                        {
                            DiversityWorkbench.Api.Taxon.Taxon Taxon = (DiversityWorkbench.Api.Taxon.Taxon)taxon[0];
                            taxa.Add(ID, Taxon);
                        }
                    }
                    catch (Exception ex) { }
                }
                _ChecklistTaxa.Add(ChecklistID, taxa);
            }
            if (_ChecklistTaxa.ContainsKey(ChecklistID))
                return _ChecklistTaxa[ChecklistID];
            else
                return new Dictionary<int, Taxon>();
        }

        public static System.Collections.Generic.Dictionary<int, Taxon> ChecklistTaxa(int ChecklistID)
        {
            if (_ChecklistTaxa.ContainsKey(ChecklistID))
                return _ChecklistTaxa[ChecklistID];
            else
                return new Dictionary<int, Taxon>();
        }

        public static void ResetChecklistTaxa()
        {
            _ChecklistTaxa = null;
        }


    }
}
