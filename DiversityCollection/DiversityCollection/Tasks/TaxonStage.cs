using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public class TaxonStage
    {
        private string _Stage;
        private string _PreviewImage;
        public TaxonStage(int NameID, int AnalysisID, IPM.IpmGroup group)
        {

        }

        public string DisplayText
        {
            get
            {
                string stage = _Stage;
                string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();
                switch (Language)
                {
                    case "de":
                        switch (_Stage)
                        {
                            case "larva":
                                stage = "Larve";
                                break;
                            case "egg":
                                stage = "Eier";
                                break;
                            case "cocoon":
                                stage = "Gespinst";
                                break;
                            case "exuviae":
                                stage = "Exuvie";
                                break;
                        }
                        break;
                }
                return stage;
            }
        }

    }
}
