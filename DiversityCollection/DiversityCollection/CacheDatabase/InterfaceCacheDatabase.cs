using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public interface InterfaceCacheDatabase
    {
        bool initAgentSources();
        bool initGazetteerSources();
        bool initTaxonSources();
        bool initTermSources();
        bool initReferenceTitleSources();
        bool initPlotSources();
        bool initProjects();
        bool initAnonymCollector();
        void ShowTransferState(string Message);
        string AddSource(InterfaceLookupSource LookupSource, System.Collections.Generic.Dictionary<string, string> SourceParameters);
    }
}
