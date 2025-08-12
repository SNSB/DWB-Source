using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class ChecklistAnalysis
    {
        private string _Analysis;
        public string Analysis { get; set; }

        public int AnalysisID { get; set; }

        public string Value { get; set; }

        public string PreviewImage { get; set; }

        public System.Collections.Generic.List<ChecklistAnalysisTitle> AnalysisTitle;

    }
}
