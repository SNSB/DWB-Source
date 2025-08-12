using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class TaxonHierarchy
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Rank { get; set; }

        public int Sequence { get; set; }

    }
}
