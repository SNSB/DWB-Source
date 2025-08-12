using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public interface InterfaceProject
    {
        // Updates needed
        //bool CacheDatabaseNeedsUpdate();
        bool CacheProjectNeedsUpdate();
        bool PostgresDatabaseNeedsUpdate();
        bool PostgresProjectNeedsUpdate();
        bool PostgresPackageNeedsUpdate();
        // setting controls
        void initPostgresControls(bool DBneedsUpdate);
        void resetPostgresControls();
        void initCacheDBControls();
        // Transfer
        string TransferDataToCacheDB(/*bool AutoTransfer, */InterfaceCacheDatabase iCacheDB);
        string TransferDataToPostgres(/*bool AutoTransfer, */InterfaceCacheDatabase iCacheDB);
        bool IncludeInTransfer();
        bool IncludePostgresInTransfer();
        // misc
        bool MissingInCacheDB();
        string DisplayText();
        string Schema();
        // Targets
        void initOtherTargets();
    }
}
