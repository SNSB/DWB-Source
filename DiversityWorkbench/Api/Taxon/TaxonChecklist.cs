using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class TaxonChecklist
    {
        private string _Checklist;
        public string Checklist { get; set; }

        public int ChecklistID { get; set; }

        public System.Collections.Generic.List<ChecklistAnalysis> Analysis { get; set; }
        public System.Collections.Generic.List<ChecklistDistribution> Distribution { get; set; }

        public System.Collections.Generic.List<ChecklistSpecimen> Specimen { get; set; }

        public System.Collections.Generic.List<ChecklistReference> Reference { get; set; }

        public System.Collections.Generic.List<ChecklistArea> Area { get; set; }

    }
}
