using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public class TransferChunk
    {
        #region Parameter

        private string _Position;
        private int _iPosition;
        public int iPosition() { return this._iPosition; }
        private int _Start;
        private int _End;
        private int _iTop;
        public int iTop() { return this._iTop; }
        private int _iBase;
        public int iBase() { return this._iBase; }
        private bool? _Failed;

        public bool Failed
        {
            get 
            {
                if (_Failed == null)
                    _Failed = true;
                return (bool)_Failed; 
            }
        }
        private TransferStep _TransferStep;
        private string _Error;
        private System.Collections.Generic.SortedList<int, TransferChunk> _FailedChunks;
        private string _TableName;

        public string TableName() { return this._TableName; }

        #endregion

        #region Construction

        public TransferChunk(string TableName, string Position, int Start, int End, TransferStep Step)
        {
            this._Position = Position;
            this._End = End;
            this._Start = Start;
            this._TableName = TableName;
            this._TransferStep = Step;
        }

        public TransferChunk()
        {
            this._iPosition = __TransferChunks.Count + 1;
            if (this._iPosition > 1)
            {
                this._iTop = __TransferChunks[this._iPosition - 1]._iBase - 1;
            }
            else
            {
                this._iTop = __iTopList;
            }
            string Error = "";
            string SQL = "select min([" + __Step.CountColumn() + "]) from [" + __Step.Schema + "].[" + __Step.Target + "] " +
                "where [" + __Step.CountColumn() + "] in (" +
                "select top " + DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizePostgres.ToString() + " [" + __Step.CountColumn() + "] from [" + __Step.Schema + "].[" + __Step.Target + "] where [" + __Step.CountColumn() + "] < " + this._iTop.ToString();
            if (__Step.SourceView != null && __Step.SourceView.Length > 0)
                SQL += " AND SourceView = '" + __Step.SourceView + "' ";
            SQL += " order by " + __Step.CountColumn() + " desc) ";
            if (__Step.SourceView != null && __Step.SourceView.Length > 0)
                SQL += " AND SourceView = '" + __Step.SourceView + "' ";
            this._iBase = int.Parse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Error).ToString());
            if (this._iBase >= this._iTop)
                this._iBase = this._iTop - 1;
        }

        private TransferChunk(string TableName, string Position, int Start, int End, TransferStep Step, ref System.Collections.Generic.SortedList<int, TransferChunk> FailedChunks)
        {
            this._Position = Position;
            this._End = End;
            this._Start = Start;
            this._TableName = TableName;
            this._TransferStep = Step;
            this._FailedChunks = FailedChunks;
        }

        private TransferChunk(string TableName, int Start, int End, TransferStep Step, ref System.Collections.Generic.SortedList<int, TransferChunk> FailedChunks)
        {
            this._Position = Start.ToString();
            this._End = End;
            this._Start = Start;
            this._TableName = TableName;
            this._TransferStep = Step;
            this._FailedChunks = FailedChunks;
        }
        
        #endregion

        public bool TransferData()
        {
            bool OK = false;
            string TableName = this._TableName + "_" + this._Start.ToString() + "-" + this._End.ToString();
            this._TransferStep.PostgresCreateTransferTable(TableName);// this._TableName);
            string Error = "";
            if (this._TransferStep.PostgresTransferData(this._Start, this._End, TableName, ref Error, false)) //, this._TableName, ref Error, false))
            {
                OK = true;
                this._Failed = false;
                //this._TransferStep.PostgresDropTransferTable(this._TableName);
            }
            else
            {
                this._Error = Error;
                return OK;




                this._FailedChunks = new SortedList<int, TransferChunk>();
                if (this._Start != this._End)
                {
                    int A_Start = this._Start;
                    int A_End = A_Start + (int)((this._End - this._Start) / 2) - 1;
                    if (A_End < A_Start)
                        A_End = A_Start;
                    int B_Start = A_End + 1;
                    int B_End = this._End;
                    if (A_End >= A_Start)
                    {
                        //DiversityCollection.CacheDatabase.TransferChunk TA = new TransferChunk(this._TableName + "_" + A_Start.ToString() + "-" + A_End.ToString(), A_Start.ToString(), A_Start, A_End, this._TransferStep, ref this._FailedChunks);
                        DiversityCollection.CacheDatabase.TransferChunk TA = new TransferChunk(this._TableName, A_Start, A_End, this._TransferStep, ref this._FailedChunks);
                        OK = TA.TransferData();
                        if (!OK)
                            _FailedChunks.Add(_FailedChunks.Count, TA);
                    }
                    if (B_Start <= B_End && B_Start > this._Start)
                    {
                        bool B_OK;
                        //DiversityCollection.CacheDatabase.TransferChunk TB = new TransferChunk(this._TableName + "_2", this._Position + "_2", B_Start, B_End, this._TransferStep, ref this._FailedChunks);
                        DiversityCollection.CacheDatabase.TransferChunk TB = new TransferChunk(this._TableName, B_Start, B_End, this._TransferStep, ref this._FailedChunks);
                        B_OK = TB.TransferData();
                        if (!B_OK)
                            _FailedChunks.Add(_FailedChunks.Count, TB);
                        if (OK && !B_OK)
                            OK = B_OK;
                    }
                }
            }
            this._Failed = !OK;
            if (OK)
                this._TransferStep.PostgresTransferDataDropChunkTable(TableName);
            return OK;
        }

        public bool TransferFailedData()
        {
            bool OK = true;
            string Error = "";
            if (this._FailedChunks != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<int, TransferChunk> KV in this._FailedChunks)
                {
                    OK = KV.Value.TransferFailedData(ref Error);
                    if (!OK)
                        break;
                }
            }
            else
                OK = false;
            if (Error.Length > 0)
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("TransferFailedData", this._TableName, Error);
            return OK;
        }

        private bool TransferFailedData(ref string Error)
        {
            bool OK = this._TransferStep.PostgresTransferData(this._Start, this._End, this._TableName, ref Error, true);
            return OK;
        }

        public bool ExchangeData(ref string Error)
        {
            bool OK = false;
            OK = this._TransferStep.PostgresExchangeData(this._Start, this._End, this._TableName, ref Error); //, this._TableName, ref Error, false))
            return OK;
        }

        public int StartID() { return this._Start; }
        public int EndID() { return this._End; }

        #region Dictionary for chunks

        private static TransferStep __Step;
        private static string __TableName;
        private static int __iTopList;
        public static int iTopList() { return __iTopList; }
        private static int __iBaseList;
        public static int iBaseList() { return __iBaseList; }
        private static System.Collections.Generic.Dictionary<int, TransferChunk> __TransferChunks;
        // getting the chunks
        public static System.Collections.Generic.Dictionary<int, TransferChunk> TransferChunks(
            string TableName, 
            TransferStep Step)
        {
            __Step = Step;
            __TableName = TableName;
            if (TransferChunk.__TransferChunks == null)
                TransferChunk.__TransferChunks = new Dictionary<int, TransferChunk>();
            if (TransferChunk.FindTop())
            {
                if (TransferChunk.FindBase())
                {
                    TransferChunk TC = new TransferChunk();
                    __TransferChunks.Add(TC._iPosition, TC);
                    while (__TransferChunks.Last().Value._iBase > __iBaseList)
                    {
                        TransferChunk TCC = new TransferChunk();
                        __TransferChunks.Add(TCC._iPosition, TCC);
                    }
                }
            }
            return TransferChunk.__TransferChunks;
        }

        private static void RemovePreviousChunkTables()
        {

        }

        // finding the top most value for the count column
        private static bool FindTop()
        {
            bool OK = true;
            try
            {
                string SqlCount = "SELECT MAX(" + __Step.CountColumn() + ") FROM  [" + __Step.Schema + "].[" + __Step.Target + "]";
                if (__Step.SourceView != null && __Step.SourceView.Length > 0)
                    SqlCount += " WHERE SourceView = '" + __Step.SourceView + "'";
                if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out __iTopList))
                {
                    OK = false;
                    //if (__I_Transfer != null)
                    //    __I_Transfer.SetMessage("Search for start failed");
                }
            }
            catch (Exception ex)
            {
                OK = false;
                {
                    //if (__I_Transfer != null)
                    //    __I_Transfer.SetMessage(ex.Message);
                }
            }
            return OK;
        }

        // finding the base most value for the count column
        private static bool FindBase()
        {
            bool OK = true;
            try
            {
                string SqlCount = "SELECT MIN(" + __Step.CountColumn() + ") FROM  [" + __Step.Schema + "].[" + __Step.Target + "]";
                if (__Step.SourceView != null && __Step.SourceView.Length > 0)
                    SqlCount += " WHERE SourceView = '" + __Step.SourceView + "'";
                if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out __iBaseList))
                {
                    OK = false;
                    //if (__I_Transfer != null)
                    //    __I_Transfer.SetMessage("Search for start failed");
                }
            }
            catch (Exception ex)
            {
                OK = false;
                {
                    //if (__I_Transfer != null)
                    //    __I_Transfer.SetMessage(ex.Message);
                }
            }
            return OK;
        }

        public static void ResetTransferChunks() 
        { 
            TransferChunk.__TransferChunks = null; 
        }
        
        #endregion
    }
}
