using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DiversityWorkbench.Api.Taxon
{
    public class Taxon
    {

        #region Properties

        private string _Server;
        public string Server { get => _Server; set => _Server = value; }


        private string _Database;
        public string Database { get => _Database; set => _Database = value; }

        public string _Type;
        public string Type { get => _Type; set => _Type = value; }


        private string _URL;
        public string URL { get => _URL; set => _URL = value; }


        private int _ID;
        public int ID { get => _ID; set => _ID = value; }


        private string _FullName;
        public string FullName { get => _FullName; set => _FullName = value; }


        private string _Name;
        public string Name { get => _Name; set => _Name = value; }


        private string _Rank;
        public string Rank { get => _Rank; set => _Rank = value; }

        private string _GenusOrSupragenericName;
        public string GenusOrSupragenericName { get => _GenusOrSupragenericName; set => _GenusOrSupragenericName = value; }


        private string _InfragenericEpithet;
        public string InfragenericEpithet { get => _InfragenericEpithet; set => _InfragenericEpithet = value; }


        private string _SpeciesEpithet;
        public string SpeciesEpithet { get => _SpeciesEpithet; set => _SpeciesEpithet = value; }


        private string _InfraspecificEpithet;
        public string InfraspecificEpithet { get => _InfraspecificEpithet; set => _InfraspecificEpithet = value; }


        private string _Authors;
        public string Authors { get => _Authors; set => _Authors = value; }


        private string _NomenclaturalStatus;
        public string NomenclaturalStatus { get => _NomenclaturalStatus; set => _NomenclaturalStatus = value; }


        private string _Publication;
        public string Publication { get => _Publication; set => _Publication = value; }


        private System.Collections.Generic.List<TaxonProject> _Project;
        public System.Collections.Generic.List<TaxonProject> Project { get => _Project; set => _Project = value; }


        public System.Collections.Generic.List<TaxonChecklist> Checklist { get; set; }


        public System.Collections.Generic.List<TaxonSynonymy> Synonymy { get; set; }


        public System.Collections.Generic.List<TaxonHierarchy> Hierarchy { get; set; }


        public System.Collections.Generic.List<TaxonCommonNames> CommonNames { get; set; }


        public System.Collections.Generic.List<TaxonExternalID> ExternalID { get; set; }

        public System.Collections.Generic.List<TaxonGeography> Geography { get; set; }

        public System.Collections.Generic.List<TaxonReference> Reference { get; set; }

        public System.Collections.Generic.List<TaxonResource> Resource { get; set; }

        public System.Collections.Generic.List<TaxonTypification> Typification { get; set; }

        private System.Collections.Generic.Dictionary<(int, int), (string, string)> _ChecklistAnalysis;

        #endregion

        private System.Collections.Generic.Dictionary<int, (string, int)> _IpmGroups;// ("", 0);

        #region Construction
        // Klasse darf keinen Konstruktor enthalten wenn dieser nicht alle Felder befüllt. Sonst scheitert Deserialisierung
        #endregion

        private TaxonList _TaxonList;
        public void SetTaxonList(TaxonList taxonList)
        {
            this._TaxonList = taxonList;
        }

    }
}
