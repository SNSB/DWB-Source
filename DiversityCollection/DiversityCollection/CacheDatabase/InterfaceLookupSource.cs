using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public interface InterfaceLookupSource
    {
        void initPostgresControls();
        void initCacheDBControls();
        bool TransferData(ref string Report, bool TransferFromSourceToCacheDB, bool TransferFromCacheToPostgres, /*bool ProcessOnly, */InterfaceCacheDatabase iCacheDB);//, bool FilterForUpdate);
        string DisplayText();
        void setState(UserControlLookupSource.Stati State, string Message = "");

        UserControlLookupSource.TypeOfSource SourceType();

        System.Collections.Generic.Dictionary<UserControlLookupSource.SubsetTable, string> SubsetTables();
    }
}
