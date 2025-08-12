using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class TaxonList
    {
        public string Name;
        public System.Collections.Generic.Dictionary<int, Taxon> Taxa;
        public Taxon Taxon(int ID) 
        {
            if (Taxa.ContainsKey(ID))
                return Taxa[ID];
            else return null;
        }

        //private static System.Collections.Generic.Dictionary<int, TaxonList> _CheckLists;
        //public static System.Collections.Generic.Dictionary<int, TaxonList> CheckLists
        //{
        //    get
        //    {
        //        if (_CheckLists == null) _CheckLists = new Dictionary<int, TaxonList>();
        //        return _CheckLists;
        //    }
        //}
    }

}
