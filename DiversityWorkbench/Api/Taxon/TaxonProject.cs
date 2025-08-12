using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api.Taxon
{
    public class TaxonProject
    {

        private string _Project;
        public string Project { get => _Project; set => _Project = value; }


        private int _ProjectID;
        public int ProjectID { get => _ProjectID; set => _ProjectID = value; }

    }
}
