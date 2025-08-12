using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public interface InterfaceCacheDB
    {
        bool initTaxonSources();
        string TransferCountryIsoCode();
        void setPostgresProject(string Project, bool NeedsUpdate);
        void initPostgresAdminProjectLists();
        bool TransferDataToCacheDB();
        bool TransferProjectDataToPostgresDB(string Project, int? ProjectID);
        void initOverviewPostgres();
    }
}
