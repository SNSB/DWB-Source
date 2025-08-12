using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.CacheDatabase
{
    public class LookupSourceReferences : LookupSource
    {
        public LookupSourceReferences()
        {
            this._Subsets.Add(SubsetTable.ReferenceRelator, "_R");
            this._Module = "DiversityReferences";
            this._Module = "ReferenceTitle";
        }

        private void initClass()
        {

        }

        public void TransferData()
        {
            //DiversityCollection.CacheDatabase.TransferStep TRef = new TransferStep("Reference", null, "ReferenceTitle", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
            //TRef.I_Transfer = this;
            //DiversityCollection.CacheDatabase.TransferStep TRefRel = new TransferStep("ReferenceRelator", null, "ReferenceRelator", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
            //TRefRel.I_Transfer = this;
            //if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer Reference");
            //OK = TRef.TransferData();
            //_Error = TRef.Errors();
            //if (_Error.Length > 0)
            //{
            //    this.WriteTransferErrorsPostgres(_Error);
            //    this._TransferHistory.Add(TRef.TableName(), _Error);
            //}
            //else
            //    this._TransferHistory.Add(TRef.TableName(), TRef.TotalCount);
            //_Report = TRef.Report();
            //if (OK)
            //{
            //    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer ReferenceRelator");
            //    OK = TRefRel.TransferData();
            //    _Error = TRefRel.Errors();
            //    if (_Error.Length > 0)
            //    {
            //        this.WriteTransferErrorsPostgres(_Error);
            //        this._TransferHistory.Add(TRefRel.TableName(), _Error);
            //    }
            //    else
            //        this._TransferHistory.Add(TRefRel.TableName(), TRefRel.TotalCount);
            //    if (OK)
            //        _Report += TRefRel.Report();
            //}

        }
    }
}
