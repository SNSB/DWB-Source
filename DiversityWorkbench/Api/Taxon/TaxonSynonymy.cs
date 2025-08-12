using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class TaxonSynonymy
    {
        public int ID { get; set; }
        public string FullName { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int SynNameID { get; set; }

        public int Ord { get; set; }
    
    }
}
