using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class TaxonExternalID
    {
        public string URI { get; set; }

        public string ExternalDatabase { get; set; }

        public string DatabaseVersion { get; set; }

    }
}
