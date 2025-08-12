using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public class TaxonGroup
    {
        private System.Collections.Generic.Dictionary<string, Taxon> _Taxa;
        private string _DisplayText;

        public TaxonGroup(int NameID, string DisplayText)
        {
            this._DisplayText = DisplayText;
            this.GetTaxa(NameID);
        }

        /*
         * Pilze
         * Spinnen
         * Asseln
         * Schaben
         * Käfer
         * Motten
         * Fliegen
         * Ameisen
         * Fischchen
         * Milben
         * Nager
         * Vögel
         */



        private void GetTaxa(int NameID)
        {

        }
    }
}
