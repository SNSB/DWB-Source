using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public interface InterfaceTransfer
    {
        void SetMessage(string Message);
        void SetInfo(string Info);
        void SetTransferState(DiversityCollection.CacheDatabase.TransferStep.TransferState State);
        void SetTransferStart();
        void SetDoTransfer(bool DoTransfer);
        void SetTransferProgress(int PercentReached);
    }
}
