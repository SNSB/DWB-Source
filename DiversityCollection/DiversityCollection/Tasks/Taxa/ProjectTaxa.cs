using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks.Taxa
{

    internal class ProjectTaxa
    {
        #region Interface

        public System.Collections.Generic.Dictionary<int, ProjectTaxonStage> ProjectTaxonStages(IPM.TaxonSource taxonSource, string SelectedUris = "") 
        {
            System.Collections.Generic.Dictionary<int, ProjectTaxonStage> Stages = new Dictionary<int, ProjectTaxonStage>();
            return Stages;
        }

        #endregion

        private System.Collections.Generic.Dictionary<int, DiversityWorkbench.TaxonName.Taxon> _projectTaxa;
        private System.Collections.Generic.Dictionary<int, DiversityWorkbench.TaxonName.Taxon> projectTaxa
        {
            get 
            { 
                if (_projectTaxa == null)
                {
                    DiversityWorkbench.ServerConnection serverConnection = new DiversityWorkbench.ServerConnection();
                    serverConnection.DatabaseName = Settings.Default.DiversityTaxonNamesDatabase;
                    DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                    _projectTaxa = T.ProjectTaxa(Settings.Default.DiversityTaxonNamesProjectID);

                    //DiversityWorkbench.ServerConnection serverConnection = new DiversityWorkbench.ServerConnection();
                    //DiversityWorkbench.TaxonName taxonName = new DiversityWorkbench.TaxonName()
                }
                return _projectTaxa; 
            }
        }

        private System.Collections.Generic.Dictionary<int, DiversityWorkbench.TaxonName.Taxon> projectTaxonList(IPM.TaxonSource taxonSource)
        {
            if (_projectTaxonSources == null) _projectTaxonSources = new Dictionary<IPM.TaxonSource, Dictionary<int, DiversityWorkbench.TaxonName.Taxon>>();
            if (!_projectTaxonSources.ContainsKey(taxonSource))
            {
                Dictionary<int, DiversityWorkbench.TaxonName.Taxon> list = new Dictionary<int, DiversityWorkbench.TaxonName.Taxon>();
                _projectTaxonSources.Add(taxonSource, list);
            }
            return _projectTaxonSources[taxonSource];
        }

        private System.Collections.Generic.Dictionary<IPM.TaxonSource, System.Collections.Generic.Dictionary<int, DiversityWorkbench.TaxonName.Taxon>> _projectTaxonSources;

        #region List

        private static System.Collections.Generic.List<int> _IpmListIDs;
        public static System.Collections.Generic.List<int> IpmListIDs
        {
            get
            {
                if (_IpmListIDs == null)
                {
                    _IpmListIDs = new List<int>();
                    _IpmListIDs.Add(Settings.Default.DiversityTaxonNamesPestListID);
                    _IpmListIDs.Add(Settings.Default.DiversityTaxonNamesBeneficialListID);
                    _IpmListIDs.Add(Settings.Default.DiversityTaxonNamesBycatchListID);
                }
                return _IpmListIDs;
            }
        }

        public static IPM.TaxonSource TaxonSource(int ListID)
        {
            IPM.TaxonSource taxonSource = IPM.TaxonSource.Pest;
            if (ListID == Settings.Default.DiversityTaxonNamesBeneficialListID)
                taxonSource = IPM.TaxonSource.Beneficial;
            else if (ListID == Settings.Default.DiversityTaxonNamesBycatchListID)
                taxonSource = IPM.TaxonSource.Bycatch;
            return taxonSource;
        }

        #endregion

        private static string TaxonDBPrefix { get { return Settings.Default.DiversityTaxonNamesDatabase + ".dbo."; } }

        public void Reset() { _projectTaxa = null; }

    }

    public struct ProjectTaxonStage
    {
        public int Sorting;
        public string Stage;
        public DiversityWorkbench.TaxonName.Taxon Taxon;
    }
}
