using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    interface InterfaceTransferStep
    {
        bool StartTransfer();
        bool ReadyForAction();
        string Title();
    }
}
