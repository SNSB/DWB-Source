using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DiversityWorkbench.Api
{
    public class Taxon
    {
        public Taxon(string JSON)
        {
            Taxon T = JsonSerializer.Deserialize<Taxon>(JSON);
        }

        public string Server { get; set; }
        public string Database { get; set; }

        public string URL { get; set; }

        public string Type { get; set; }

        public string ID { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Rank { get; set; }

        public string GenusOrSupragenericName { get; set; }

        public string SpeciesEpithet { get; set; }

    }
}
