using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public static class Checklists
    {
        private static System.Collections.Generic.Dictionary<int, TaxonList> _Taxa;
        public static System.Collections.Generic.Dictionary<int, TaxonList> Taxa
        {
            get
            {
                if (_Taxa == null) _Taxa = new Dictionary<int, TaxonList>();
                return _Taxa;
            }
        }

        //public static TaxonList List(int ChecklistID)
        //{
        //    if (!Taxa.ContainsKey(ChecklistID))
        //    {
        //        TaxonList taxonList = new TaxonList();
        //        Taxa.Add(ChecklistID, taxonList);
        //    }
        //    return Taxa[ChecklistID];
        //}

        public static Taxon Taxon(int ChecklistID, int ID)
        {
            if (Taxa.ContainsKey(ChecklistID))
            {
                if (Taxa[ChecklistID].Taxa.ContainsKey(ID))
                    return Taxa[ChecklistID].Taxa[ID];
            }
            return null;
        }

        public static void AddTaxon(string JSON)
        {
            var taxa = System.Text.Json.JsonSerializer.Deserialize<List<DiversityWorkbench.Api.Taxon.Taxon>>(JSON);
            if (taxa != null && taxa.GetType() == typeof(List<DiversityWorkbench.Api.Taxon.Taxon>) && taxa.Count == 1)
            {
                DiversityWorkbench.Api.Taxon.Taxon taxon = taxa[0];
                foreach (DiversityWorkbench.Api.Taxon.TaxonChecklist checklist in taxon.Checklist)
                {
                    DiversityWorkbench.Api.Taxon.Checklists.AddTaxon(checklist.ChecklistID, taxon.ID, taxon);
                    if (DiversityWorkbench.Api.Taxon.Checklists.Taxa.ContainsKey(checklist.ChecklistID))
                        taxon.SetTaxonList(DiversityWorkbench.Api.Taxon.Checklists.Taxa[checklist.ChecklistID]);
                }
            }

        }

        private static void AddTaxon(int ChecklistID, int ID, Taxon taxon)
        {
            if (!Taxa.ContainsKey(ChecklistID))
            {
                TaxonList taxonList = new TaxonList();
                taxonList.Taxa = new Dictionary<int, Taxon>();
                taxonList.Taxa.Add(ID, taxon);
                Taxa.Add(ChecklistID, taxonList);
            }
            else if (Taxa.ContainsKey(ChecklistID) && !Taxa[ChecklistID].Taxa.ContainsKey(ID))
            {
                Taxa[ChecklistID].Taxa.Add(ID, taxon);
            }
            //if (List(ChecklistID).Taxa == null)
            //{
            //    TaxonList taxonList = new TaxonList();
            //    taxonList.Taxa.Add(ID, taxon);
            //    List(ChecklistID).Taxa = taxonList;
            //}
            //if (List(ChecklistID).Taxa != null && !List(ChecklistID).Taxa.ContainsKey(ID))
            //{
            //    Taxa[ChecklistID].Taxa.Add(ID, taxon);
            //}
        }

    }
}
