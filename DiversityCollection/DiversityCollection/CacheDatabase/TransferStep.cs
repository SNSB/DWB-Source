using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DiversityCollection.CacheDatabase
{

    public class TransferStep
    {

        #region Properties

		public enum TransferState { Error, NotStarted, Transfer, Successfull, Failed }

        public DiversityCollection.CacheDatabase.InterfaceTransfer I_Transfer;

        private bool _ForPostgres;
        public bool ForPostgres
        {
            get { return _ForPostgres; }
            set { _ForPostgres = value; }
        }

        private bool _ForPackage = false;
        public bool ForPackage
        {
            get { return _ForPackage; }
            set { _ForPackage = value; }
        }

        private string _PostgresRole = "";
        public string PostgresRole
        {
            get { return _PostgresRole; }
            set { _PostgresRole = value; }
        }

        private bool _PostgresUseBulkTransfer = false;

        private string _Errors = "";
        public string Errors() { return this._Errors; }

        private string _Report;
        public string Report() { return this._Report; }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private System.Drawing.Image _Image;
        public System.Drawing.Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        private bool _DoTransferData = true;
        public bool DoTransferData
        {
            get { return _DoTransferData; }
            set 
            { 
                _DoTransferData = value;
                this.I_Transfer.SetDoTransfer(value);
            }
        }

        private string _ViewForChunks;
        public string ViewForChunks
        {
            get 
            {
                if (this._ViewForChunks == null)
                {
                    this._ViewForChunks = "View" + this.Target.Replace("Cache", "");
                    string SQL = "SELECT TOP 1 * FROM " + this._ViewForChunks;
                    string Message = "";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (Message.Length > 0)
                        this._ViewForChunks = "";
                }
                return this._ViewForChunks;// _ViewForChunks; 
            }
            //set { _ViewForChunks = value; }
        }

        private string _SourceView;
        public string SourceView
        {
            get { return _SourceView; }
            set { _SourceView = value; }
        }

        private string _TransferProcedure;
        public string TransferProcedure
        {
            get { return _TransferProcedure; }
            set { _TransferProcedure = value; }
        }

        private string _Schema;
        public string Schema
        {
            get { return _Schema; }
            set { _Schema = value; }
        }

        private string _SchemaPostgres;
        public string SchemaPostgres
        {
            get { if (this._SchemaPostgres == null) return this._Schema; else return _SchemaPostgres; }
            set { _SchemaPostgres = value; }
        }

        private string _Target;
        public string Target
        {
            get { return _Target; }
            //set { _Target = value; }
        }
        public string TargetTemp
        {
            get { return this.Target + "_Temp"; }
        }

        private System.Collections.Generic.List<string> _SuppressedColumns;

        public System.Collections.Generic.List<string> SuppressedColumns
        {
            get { if (this._SuppressedColumns == null) this._SuppressedColumns = new List<string>(); return _SuppressedColumns; }
            //set { _SuppressedColumns = value; }
        }

        //private bool _LoggingIsOn = true;

        public bool LoggingIsOn
        {
            get { return DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents;}// _LoggingIsOn; }
            //set { _LoggingIsOn = value; }
        }

        private int _TotalCount = 0;

        public int TotalCount
        {
            get { return _TotalCount; }
            //set { _TotalCount = value; }
        }

        private string _SourceTable = "";

 
	    #endregion

        #region Construction
		        
        public TransferStep(string Title, System.Drawing.Image Image, string Table, string Schema, bool ForPostgres, bool UseBulkTransfer = false)//, string SqlViewResults)
        {
            this._Image = Image;
            this._Target = "Cache" + Table;
            this._Schema = Schema;
            this._Title = Title;
            this._ForPostgres = ForPostgres;
            if (ForPostgres)
            {
                this._TransferProcedure = "";
                //this._SourceView = "";
            }
            else
            {
                //this._SourceView = "View" + Table;
                this._TransferProcedure = "procPublish" + Table;
            }
        }

        public TransferStep(string Title, System.Drawing.Image Image, string Table, string SchemaSql, string SchemaPostgres, bool ForPostgres, bool KeepTablename, System.Collections.Generic.List<string> SuppressedColumns, string SourceView)//, string SqlViewResults)
        {
            this._Image = Image;
            this._SourceView = SourceView;
            if (KeepTablename)
                this._Target = Table;
            else
                this._Target = "Cache" + Table;
            this._Schema = SchemaSql;
            this._SchemaPostgres = SchemaPostgres;
            this._Title = Title;
            this._ForPostgres = ForPostgres;
            this._TransferProcedure = "";
            this._SuppressedColumns = SuppressedColumns;
        }

        // For transfer of packages
        public TransferStep(string Title, string SchemaPostgres, string PackageFunction, string Target, System.Drawing.Image Image)//, string SqlViewResults)
        {
            this._Title = Title;
            this._Image = Image;
            this._ForPostgres = true;
            this.ForPackage = true;
            this._SchemaPostgres = SchemaPostgres;
            this._Schema = SchemaPostgres;
            this._TransferProcedure = PackageFunction;
            this._Target = Target;
        }

        // Transfer webservice content to postgres
        public TransferStep(string SourceTable, string PostgresTable, string SourceView)
        {
            this._SourceView = SourceView;
            this._SourceTable = SourceTable;
            this._Target = PostgresTable;
            this._Schema = "dbo";
            this._SchemaPostgres = "postgres";
            this._ForPostgres = true;
        }

        #endregion

        #region Checking update in source

        private System.DateTime _LastTransfer;

        public System.DateTime LastTransfer()
        {
            if (this._LastTransfer == null || this._LastTransfer.Year == 1)
            {
                string SQL = "SELECT LastUpdatedWhen FROM ProjectPublished WHERE ProjectID = " + this._Schema + ".ProjectID()";
                if (!System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out this._LastTransfer))
                {
                    this._LastTransfer = System.DateTime.Now;
                }
            }
            return this._LastTransfer;
        }

        private System.DateTime _LastUpdateInSource;

        public System.DateTime LastUpdateInSource()
        {
            if (this._LastUpdateInSource == null || this._LastUpdateInSource.Year == 1)
            {
                string WhereClause = "";
                foreach (string s in this.PKcolumns())
                {
                    if (WhereClause.Length > 0) WhereClause += " AND ";
                    WhereClause += " S." + s + " = T." + s + " ";
                }
                string SQL = "";
                SQL = "SELECT case when MAX(S.LogUpdatedWhen) is null then cast('1800/01/01' as datetime) else  MAX(S.LogUpdatedWhen) end FROM ";
                if (this.SourceView == null)
                {
                    if (this.ViewForChunks != null && this.ViewForChunks.Length > 0)
                        SQL += this.ViewForChunks;
                    else
                    {
                        string Table = this.TableName();
                        switch (Table)
                        {
                            case "CacheProjectReference":
                                break;
                        }
                        SQL += this.Schema + "." + Table;
                    }
                }
                else
                {
                    SQL += this.SourceView;
                }
                SQL += " S, " + this._Schema + "." + this._Target + " T WHERE " + WhereClause;
                if (!System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out this._LastUpdateInSource))
                {
                    this._LastUpdateInSource = System.DateTime.Now;
                }
            }
            return this._LastUpdateInSource;
        }

        private System.Collections.Generic.List<string> PKcolumns()
        {
            System.Collections.Generic.List<string> PK = new List<string>();
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            string SQL = "SELECT COLUMN_NAME " +
                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                "WHERE (TABLE_NAME = '" + this._Target + "') " +
                "AND (TABLE_SCHEMA = '" + this._Schema + "') " +
                "AND (EXISTS " +
                "(SELECT * " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
                PK.Add(R[0].ToString());
            return PK;
        }
        
        #endregion

        #region Transfer

        public bool TransferData(string TransferDirectory = "")
        {
            if (!this.DoTransferData)
                return true;
            if (this.I_Transfer != null)
                this.I_Transfer.SetTransferStart();
            bool OK = true;
            this._Report = "";
            if (this.ForPackage)
            {
                //CacheDB.LogEvent("", "", "___Transfer_package" + this.n___________________________");

                OK = this.TransferDataToPackage(ref this._TotalCount);
            }
            else if (this._ForPostgres)
            {
                 OK = this.TransferDataToPostgres(TransferDirectory);
            }
            else
            {
                // Check amout of the data and if a view for the chunks has been defined (count column included to check if this column does exist)
                string Message = "";
                if (this.I_Transfer != null)
                    this.I_Transfer.SetTransferState(TransferState.Transfer);
                if (this.I_Transfer != null)
                    this.I_Transfer.SetMessage("Calculation of amount");

                bool TransferInChunks = false;
                string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_SCHEMA = '" + this.Schema + "' and v.TABLE_NAME = '" + this.ViewForChunks + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
                int iAmout = -1;
                if (Result == "1")
                {
                    SQL = "SELECT COUNT(*) FROM [" + this.Schema + "]." + this.ViewForChunks + " V WHERE NOT V." + this.CountColumn() + " IS NULL";
                    if (int.TryParse(Result, out iAmout))
                    {
                        if (iAmout > DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitCacheDB)//00000)
                            TransferInChunks = true;
                    }
                }
                if (TransferInChunks)
                {
                    if (this.I_Transfer != null)
                        this.I_Transfer.SetMessage(iAmout.ToString() + "Datasets. Transfer in chunks");
                    this.TransferDataInChunksToCacheDB(iAmout);
                }
                else
                {
                    SQL = "exec [" + this.Schema + "]." + this._TransferProcedure;
                    if (this.I_Transfer != null)
                        this.I_Transfer.SetTransferState(TransferState.Transfer);
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                    {
                        if (this.I_Transfer != null)
                            this.I_Transfer.SetTransferState(TransferState.Successfull);

                        SQL = "SELECT COUNT(*) FROM [" + this.Schema + "].[" + this.Target + "]";
                        if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL), out this._TotalCount))
                            this._Report += this._TotalCount;
                    }
                    else
                    {
                        this._Errors += Message;
                        if (this.I_Transfer != null)
                        {
                            this.I_Transfer.SetTransferState(TransferState.Failed);
                            OK = false;
                        }
                    }
                    if (Message.Length > 0)
                    {
                        if (this.I_Transfer != null)
                            this.I_Transfer.SetMessage(Message);
                        this._Report += Message;
                    }
                }
            }
            this._Report += "\t" + this.Target;
            this._Report += "\r\n";
            return OK;
        }

        #region Transfer to CacheDB
        
        private bool TransferDataInChunksToCacheDB(int TotalCount)
        {
            bool OK = this.PrepareTransferChunksToCacheDB();
            if (OK)
            {
                //System.Collections.Generic.SortedList<int, int> ChunkList = new SortedList<int, int>();

                // getting the chunk size
                //string SqlCount = "SELECT COUNT(*) FROM [" + this.Schema + "].[" + this.ViewForChunks + "]";
                //if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out TotalCount))
                //    return false;

                int ChunkSize = TotalCount / 100;
                if (ChunkSize > DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizeCacheDB) 
                    ChunkSize = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizeCacheDB;

                //int ChunkColumnMin;
                //SqlCount = "SELECT MIN(" + this.CountColumn() + ") FROM  [" + this.Schema + "].[" + this.ViewForChunks + "]";
                //if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out ChunkColumnMin))
                //    return false;
                //int ChunkColumnMax;
                //SqlCount = "SELECT MAX(" + this.CountColumn() + ") + 1 FROM  [" + this.Schema + "].[" + this.ViewForChunks + "]";
                //if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out ChunkColumnMax))
                //    return false;
                //ChunkList.Add(ChunkColumnMin, ChunkColumnMin);
                //ChunkList.Add(ChunkColumnMax, ChunkColumnMax);
                //this.FindChunks(ref ChunkList, 0, ChunkSize);//, false);

                if (!this.FindChunks(0, ChunkSize))
                    return false;

                //System.Collections.Generic.List<int> Chunks = new List<int>();
                //foreach (System.Collections.Generic.KeyValuePair<int, int> KV in this._ChunkList)
                //{
                //    Chunks.Add(KV.Key);
                //}

                if (this.I_Transfer != null)
                    this.I_Transfer.SetInfo(Chunks.Count.ToString() + "Chunks defined");

                // ONLY FOR CHECKING THE CHUNK SIZE FOR DEBUGGING
                //string CheckChunks = "";
                //for (int i = 0; i < Chunks.Count - 1; i++)
                //{
                //    CheckChunks += "SELECT COUNT(*) FROM [" + this.Schema + "]." + this._Target + " WHERE " + CountColumn + " BETWEEN " + Chunks[i].ToString() + " AND " + (Chunks[i + 1] - 1).ToString() + "; ";
                //}
                // ONLY FOR CHECKING THE CHUNK SIZE

                if (this.I_Transfer != null)
                    this.I_Transfer.SetTransferState(TransferState.Transfer);

                // init the faild chunks
                this._FailedChunkErrors = new SortedDictionary<int, string>();
                this._FailedChunks = new List<int>();

                // Transfer data
                for (int i = 0; i < Chunks.Count - 1; i++)
                {
                    this.TransferCacheDBChunk(i);//, this._Chunks);
                }

                // transfer failed chunks - try only once
                string Error = "";
                if (_FailedChunks.Count > 0)
                {
                    OK = true;
                    int[] ChunksToDo = new int[_FailedChunks.Count];
                    _FailedChunks.CopyTo(ChunksToDo);
                    _FailedChunks.Clear();
                    for (int c = 0; c < ChunksToDo.Length; c++)
                        this.TransferCacheDBChunk(ChunksToDo[c]);//, this._Chunks);
                    if (_FailedChunks.Count > 0)
                    {
                        OK = false;
                        foreach (int i in _FailedChunks)
                        {
                            if (_FailedChunkErrors.ContainsKey(i))
                                Error += "Failed chunk " + i.ToString() + ": " + _FailedChunkErrors[i];
                        }
                    }
                }

                // switch tables
                string SQL = "BEGIN TRANSACTION " +
                    " begin try DROP TABLE [" + this.Schema + "].[" + this.Target + "]; EXEC sp_rename '" + this.Schema + "." + this.TargetTemp + "', '" + this.Target + "';   COMMIT TRANSACTION end try begin catch ROLLBACK TRANSACTION end catch ";
                OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Error);

                if (this.I_Transfer != null)
                {
                    if (_FailedChunks.Count == 0)
                        this.I_Transfer.SetTransferState(TransferState.Successfull);
                    else
                        this.I_Transfer.SetTransferState(TransferState.Error);
                }
            }
            return OK;
        }

        private bool PrepareTransferChunksToCacheDB()
        {
            bool OK = true;
            string Error = "";
            try
            {
                string SQL = "exec [dbo].[spCloneTableStructure] '" + this.Schema + "', '" + this.Target + "', '" + this.Schema + "', '" + this.TargetTemp + "', 1";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Error);
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message;
            }
            if (Error.Length > 0)
                this._Report += "\r\n" + Error;
            if (this.I_Transfer != null)
                this.I_Transfer.SetInfo(this.TargetTemp + " created");
            return OK;
        }

        private bool TransferCacheDBChunk(int ChunkPosition)
        {
            bool OK = true;
            string ChunkFailure = "";

            if (this.I_Transfer != null)
                this.I_Transfer.SetInfo("Chunk " + ChunkPosition.ToString() + " of " + this._Chunks.Count.ToString());

            string SQL = "INSERT INTO [" + this.Schema + "].[" + this.TargetTemp + "] " +
                " SELECT * FROM [" + this.Schema + "].[" + this.ViewForChunks + "] " +
                " WHERE " + this.CountColumn() + " BETWEEN " + this._Chunks[ChunkPosition].ToString() + " AND " + (this._Chunks[ChunkPosition + 1] - 1).ToString(); ;
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref ChunkFailure);
            if (ChunkFailure.Length > 0)
            {
                CacheDB.LogEvent("TransferCacheDBChunk", "Chunk " + ChunkPosition.ToString() + "; SQL: " + SQL, ChunkFailure);

                if (_FailedChunkErrors.ContainsKey(ChunkPosition))
                    _FailedChunkErrors[ChunkPosition] += ChunkFailure;
                else
                    _FailedChunkErrors.Add(ChunkPosition, ChunkFailure);
                _FailedChunks.Add(ChunkPosition);
            }

            double Percent = ((double)ChunkPosition + 1) / (double)this._Chunks.Count;
            if (this.I_Transfer != null)
                this.I_Transfer.SetTransferProgress((int)(Percent * 100));
            return OK;
        }

        #endregion

        #region Database infos

        private string _CountColumn;
        public string CountColumn()
        {
            if (this._CountColumn == null)
            {
                try
                {
                    if (this.TargetTable.PrimaryKeyColumnList.Contains("IdentificationUnitID"))
                        _CountColumn = "IdentificationUnitID";
                    else if (this.TargetTable.PrimaryKeyColumnList.Contains("SpecimenPartID"))
                        _CountColumn = "SpecimenPartID";
                    else if (this.TargetTable.IdentityColumn.Length > 0)
                        _CountColumn = this.TargetTable.IdentityColumn;
                    else if (this.TargetTable.PrimaryKeyColumnList.Contains("NameID"))
                        _CountColumn = "NameID";
                    else
                        _CountColumn = this.TargetTable.PrimaryKeyColumnList[0];
                }
                catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            return _CountColumn;
        }

        private DiversityWorkbench.Data.Table _TargetTable;

        private DiversityWorkbench.Data.Table TargetTable
        {
            get
            {
                if (this._TargetTable == null)
                {
                    this._TargetTable = new DiversityWorkbench.Data.Table(this.Target, this.Schema, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                }
                return _TargetTable;
            }
        }

        #endregion

        #region Chunks

        private System.Collections.Generic.SortedList<int, int> _ChunkSortedList;
        private System.Collections.Generic.List<int> _Chunks;

        public System.Collections.Generic.List<int> Chunks
        {
            get
            {
                if (this._Chunks == null && this._ChunkSortedList != null)
                {
                    this._Chunks = new List<int>();
                    foreach (System.Collections.Generic.KeyValuePair<int, int> KV in this._ChunkSortedList)
                    {
                        Chunks.Add(KV.Key);
                    }
                }
                return _Chunks;
            }
            //set { _Chunks = value; }
        }

        /// <summary>
        /// find the range of values containing a max. number of results
        /// </summary>
        /// <param name="Start">the value where the chunk starts</param>
        /// <param name="ChunkSize">the maximal size of the chunk</param>
        /// <returns>if it worked</returns>
        private bool FindChunks(int Start, int ChunkSize)
        {
            bool OK = true;
            try
            {
                if (this.I_Transfer != null)
                    this.I_Transfer.SetInfo("Calculating the chunks sizes");

                if (this._ChunkSortedList == null)
                {
                    this._ChunkSortedList = new SortedList<int, int>();
                    int ChunkColumnMin;
                    string SqlCount = "SELECT MIN(" + this.CountColumn() + ") FROM  [" + this.Schema + "].[";
                    if (this.ForPostgres)
                        SqlCount += this._Target;
                    else
                        SqlCount += this.ViewForChunks;
                    SqlCount += "]";
                    if (this.SourceView != null && this.SourceView.Length > 0)
                        SqlCount += " WHERE SourceView = '" + this.SourceView + "'";
                    if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out ChunkColumnMin))
                        return false;
                    int ChunkColumnMax;
                    SqlCount = "SELECT MAX(" + this.CountColumn() + ") + 1 FROM  [" + this.Schema + "].[";// +this._Target + "]";
                    if (this.ForPostgres)
                        SqlCount += this._Target;
                    else
                        SqlCount += this.ViewForChunks;
                    SqlCount += "]";
                    if (this.SourceView != null && this.SourceView.Length > 0)
                        SqlCount += " WHERE SourceView = '" + this.SourceView + "'";
                    if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out ChunkColumnMax))
                        return false;
                    this._ChunkSortedList.Add(ChunkColumnMin, ChunkColumnMin);
                    Start = ChunkColumnMin;
                    this._ChunkSortedList.Add(ChunkColumnMax, ChunkColumnMax);
                }

                string SQL = "";
                bool StartFound = false;
                int UpperBorder = 0;
                foreach (System.Collections.Generic.KeyValuePair<int, int> KV in this._ChunkSortedList)
                {
                    if (StartFound)
                    {
                        UpperBorder = KV.Key;
                        break;
                    }
                    if (KV.Key == Start)
                    {
                        StartFound = true;
                    }
                }
                if (UpperBorder > 0)
                {
                    int NewChunk = Start + (UpperBorder - Start) / 2;
                    this._ChunkSortedList.Add(NewChunk, UpperBorder);
                    SQL = "SELECT COUNT(*) FROM [" + this.Schema + "].[";
                    if (ForPostgres)
                        SQL += this.Target;
                    else
                        SQL += this.ViewForChunks;
                    SQL += "] WHERE " + this.CountColumn() + " BETWEEN " + Start.ToString() + " AND " + (NewChunk - 1).ToString();
                    int CheckCount = 0;
                    if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL), out CheckCount) && (CheckCount > ChunkSize))
                    {
                        this.FindChunks(Start, ChunkSize);//, ForPostgres);
                    }
                    SQL = "SELECT COUNT(*) FROM [" + this.Schema + "].[";
                    if (ForPostgres)
                        SQL += this.Target;
                    else
                        SQL += this.ViewForChunks;
                    SQL += "] WHERE " + this.CountColumn() + " BETWEEN " + NewChunk.ToString() + " AND " + (UpperBorder - 1).ToString();
                    if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL), out CheckCount) && (CheckCount > ChunkSize))
                    {
                        OK = this.FindChunks(NewChunk, ChunkSize);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return OK;
        }
        
        #endregion

        #region Transfer to postgres
        
        private System.Collections.Generic.List<int> _FailedChunks;
        private System.Collections.Generic.SortedDictionary<int, string> _FailedChunkErrors;
        private System.Data.DataTable _dtSource;
        private Microsoft.Data.SqlClient.SqlDataAdapter _adSource;
        private Npgsql.NpgsqlDataAdapter _adTargetPG;
        private System.Data.DataTable _dtTargetPG;

        private Npgsql.NpgsqlDataAdapter _adTargetChunkPG;
        private System.Data.DataTable _dtTargetChunkPG;

        public bool PostgresUseBulkTransfer
        {
            get
            {
                return this._PostgresUseBulkTransfer;
            }
            set
            {
                this._PostgresUseBulkTransfer = value;
            }
        }

        private bool TransferDataToPostgres(string TransferDirectory = "")
        {
            bool OK = true;
            string Error = "";
            if (PostgresUseBulkTransfer)
            {
                OK = this.PostgresTransferViaFile(ref Error, TransferDirectory);
            }
            else
            {
                if (!this.PrepareTransferToPostgres())
                    return false;

                try
                {
                    if (!this.initTransferDataToPostgresColumns())
                        return false;

                    // get the total number of data in the source
                    string SqlCount = "SELECT COUNT(*) FROM [" + this.Schema + "].[" + this._Target + "]";
                    if (this._SourceTable.Length > 0)
                        SqlCount = "SELECT COUNT(*) FROM [" + this.Schema + "].[" + this._SourceTable + "]";
                    if (this.SourceView != null && this.SourceView.Length > 0)
                        SqlCount += " WHERE SourceView = '" + this.SourceView + "'";
                    if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out this._TotalCount))
                        return false;

                    // prepare the source and target table
                    string SQL = "";
                    if (!this.PostgresPrepareSourceTable(ref SQL))
                        return false;
                    if (!this.PostgresPrepareTargetTable())
                        return false;
                    bool IncludeChunks = false;
#if DEBUG
                    //IncludeChunks = true;
#endif

                    if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.UseChunksForPostgres &&
                        TotalCount > DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitPostgres
                        && IncludeChunks) // Markus 18.6.2020: Abgeschaltet wegen unklarem Fehler in Zeile 2997: ERROR: 42703: column d.adsrc does not exist
                    {
                        OK = this.PostgresTransferInChunks(TotalCount, SQL, ref Error);
                    }
                    else
                    {
                        OK = this.PostgresTransferWithoutChunks(ref Error);
                        //OK = this.PostgresTransferSingle(ref Error);
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                    if (Error.Length > 0)
                        Error += "\r\n\r\n";
                    Error += ex.Message;
                }
            }
            if (Error.Length > 0)
            {
                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents &&
                    PostgresUseBulkTransfer &&
                    OK)
                {
                    this._Report += Error;
                    this.I_Transfer.SetTransferState(TransferState.Successfull);
                    this.I_Transfer.SetInfo("");
                }
                else
                {
                    OK = false;
                    if (this.I_Transfer != null)
                    {
                        this.I_Transfer.SetMessage(Error);
                        this.I_Transfer.SetTransferState(TransferState.Failed);
                    }
                    if (this._Report.IndexOf(Error) == -1)
                    {
                        if (this._Report.Length > 0)
                            this._Report += "\r\n";
                        this._Report += Error;
                    }
                }
            }
            else if (this.I_Transfer != null)
            {
                this.I_Transfer.SetTransferState(TransferState.Successfull);
                this.I_Transfer.SetInfo("");
            }
            return OK;
        }

        #region Bulk transfer via bcp

        private enum bcpStep
        {
            Start, Init, CreateView, Export, ExportFileExists, CreateTempTable, BashImport, CompareCounts, CreateBackup, ClearTable, GetPK, RemovePK, 
            InsertData, ConfirmTransfer, EnsureLogged, RestoreFromBackup, RestorePK, RemoveFile, RemoveTempTable, RemoveBackup
        }

        private System.Collections.Generic.Dictionary<bcpStep, string> _bcpStepMessages;
        private System.Collections.Generic.Dictionary<bcpStep, string> bcpStepMessages
        {
            get
            {
                if (this._bcpStepMessages == null)
                {
                    this._bcpStepMessages = new Dictionary<bcpStep, string>();
                    this._bcpStepMessages.Add(bcpStep.Init, "1 of 18 - Initialize bulk transfer");
                    this._bcpStepMessages.Add(bcpStep.CreateView, "2 of 18 - Create view for bulk transfer");
                    this._bcpStepMessages.Add(bcpStep.Export, "3 of 18 - Data export via bcp in csv file in transfer directory");
                    this._bcpStepMessages.Add(bcpStep.ExportFileExists, "4 of 18 - Exported csv file exists in transfer directory");
                    this._bcpStepMessages.Add(bcpStep.CreateTempTable, "5 of 18 - Creation of temporary table");
                    this._bcpStepMessages.Add(bcpStep.BashImport, "6 of 18 - Bash conversion and import of data in temporary table");
                    this._bcpStepMessages.Add(bcpStep.CompareCounts, "7 of 18 - Comparing counts of view and imported data");
                    this._bcpStepMessages.Add(bcpStep.CreateBackup, "8 of 18 - Creation of backup");
                    this._bcpStepMessages.Add(bcpStep.ClearTable, "9 of 18 - Clearing table");
                    this._bcpStepMessages.Add(bcpStep.GetPK, "10 of 18 - Getting PK of table");
                    this._bcpStepMessages.Add(bcpStep.RemovePK, "11 of 18 - Removal of PK from table");
                    this._bcpStepMessages.Add(bcpStep.InsertData, "12 of 18 - Insert of data from temporary table in table");
                    this._bcpStepMessages.Add(bcpStep.ConfirmTransfer, "13 of 18 - Confirmation of insert of data from temporary table in table");
                    this._bcpStepMessages.Add(bcpStep.RestoreFromBackup, "14 of 18 - Restoring data from backup");
                    this._bcpStepMessages.Add(bcpStep.RestorePK, "15 of 18 - Restoring of PK");
                    this._bcpStepMessages.Add(bcpStep.RemoveFile, "16 of 18 - Removal of csv file from transfer directory");
                    this._bcpStepMessages.Add(bcpStep.RemoveTempTable, "17 of 18 - Removal of temporary table");
                    this._bcpStepMessages.Add(bcpStep.RemoveBackup, "18 of 18 - Removal of backup table");
                }
                return this._bcpStepMessages;
            }
        }

        private bool PostgresTransferViaFile(ref string Error, string TransferDirectory)
        {
            bool OK = true;
            bcpStep Step = bcpStep.Start;
            string SQL = "";

            // Init Export  - already done with start
            Step = bcpStep.Init;

            // Create view
            if (Step == bcpStep.Init)
            {
                OK = CacheDB.bcpCreateView(this.Schema, this.TableName(), ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.CreateView;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Export Data
            if (Step == bcpStep.CreateView)
            {
                OK = this.PostgresTransferViaFile_ExportData(TransferDirectory, ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.Export;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Export File exists
            if (Step == bcpStep.Export)
            {
                OK = this.PostgresTransferViaFile_ExportFileExists(ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.ExportFileExists;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Create temp table if not existing
            if (Step == bcpStep.ExportFileExists)
            {
                OK = this.PostgresTransferViaFile_CreateTempTable(ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.CreateTempTable;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Bash conversion and import of the data into Postgres
            if (Step == bcpStep.CreateTempTable)
            {
                OK = this.PostgresTransferViaFile_BashImport(ref Error, ref SQL);
                if (OK)
                {
                    // Test numbers
                    Step = bcpStep.BashImport;
                }
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Compare data count from view and temp table
            if (Step == bcpStep.BashImport)
            {
                OK = this.PostgresTransferViaFile_CompareCounts(ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.CompareCounts;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // stop in case of error if demanded
            if (!OK && DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError)
                return OK;

            // Copy data from productive table in backup
            if (Step == bcpStep.CompareCounts)
            {
                OK = this.PostgresTransferViaFile_CreateBackup(ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.CreateBackup;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Clear productive table
            if (Step == bcpStep.CreateBackup)
            {
                OK = this.PostgresTransferViaFile_ClearTable(this.Schema, this.TableName(), TransferDirectory, ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.ClearTable;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Get PK from productive table
            System.Data.DataTable dtPK = new System.Data.DataTable();
            if (Step == bcpStep.ClearTable)
            {
                OK = this.PostgresTransferViaFile_GetPK(ref dtPK, ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.GetPK;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Remove PK from productive table
            if (Step == bcpStep.GetPK)
            {
                OK = this.PostgresTransferViaFile_RemovePK(ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.RemovePK;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Transfer data to productive table
            if (Step == bcpStep.RemovePK || Step == bcpStep.GetPK)
            {
                OK = this.PostgresTransferViaFile_InsertData(ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.InsertData;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // Confirmation of transfer into productive table
            if (Step == bcpStep.InsertData)
            {
                OK = this.PostgresTransferViaFile_ConfirmTransfer(ref Error, ref SQL);
                if (OK)
                    Step = bcpStep.ConfirmTransfer;
                this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
            }

            // ensure logged persistence
            if (Step == bcpStep.ConfirmTransfer)
            {
                OK = this.PostgresTransferViaFile_EnsureLogged(ref Error, ref SQL);
            }

            // Restore data in case of failure
            switch (Step)
            {
                case bcpStep.ClearTable:
                case bcpStep.RemovePK:
                case bcpStep.InsertData:
                    OK = this.PostgresTransferViaFile_RestoreFromBackup(ref Error, ref SQL);
                    if (OK)
                        Step = bcpStep.RestoreFromBackup;
                    this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
                    break;
            }

            // Add PK to productive table
            switch (Step)
            {
                case bcpStep.RestoreFromBackup:
                case bcpStep.InsertData:
                case bcpStep.ConfirmTransfer:
                case bcpStep.RemovePK:
                    OK = this.PostgresTransferViaFile_RestorePK(dtPK, ref Error, ref SQL);
                    if (OK)
                        Step = bcpStep.RestorePK;
                    this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
                    break;
            }

            // stop in case of error if demanded
            if (!OK && DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError)
                return OK;

            // remove file from directory
            bool StateBeforeRemoval = OK;
            switch (Step)
            {
                case bcpStep.BashImport:
                    if(this.PostgresTransferViaFile_RemoveFile(TransferDirectory, ref Error, ref SQL))
                         this.PostgresTransferViaFile_SetMessage(bcpStep.RemoveFile, true, SQL);
                    break;
                default:
                    OK = this.PostgresTransferViaFile_RemoveFile(TransferDirectory, ref Error, ref SQL);
                    if (OK)
                        Step = bcpStep.RemoveFile;
                    this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
                    break;
            }

            // Drop temp table if existing
            switch (Step)
            {
                case bcpStep.BashImport:
                    if (this.PostgresTransferViaFile_RemoveTempTable(ref Error, ref SQL))
                        this.PostgresTransferViaFile_SetMessage(bcpStep.RemoveTempTable, true, SQL);
                    break;

                default:
                    OK = this.PostgresTransferViaFile_RemoveTempTable(ref Error, ref SQL);
                    if (OK)
                        Step = bcpStep.RemoveTempTable;
                    this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
                    break;
            }

            // Drop backup table if existing
            switch (Step)
            {
                case bcpStep.BashImport:
                    if (this.PostgresTransferViaFile_RemoveBackup(ref Error, ref SQL))
                        this.PostgresTransferViaFile_SetMessage(bcpStep.RemoveBackup, true, SQL);
                    break;
                case bcpStep.ClearTable:
                case bcpStep.RemovePK:
                    break;
                default:
                    OK = this.PostgresTransferViaFile_RemoveBackup(ref Error, ref SQL);
                    if (OK)
                        Step = bcpStep.RemoveBackup;
                    this.PostgresTransferViaFile_SetMessage(Step, OK, SQL);
                    break;
            }
            if (!StateBeforeRemoval)
                OK = StateBeforeRemoval;
            return OK;
        }

        private bool PostgresTransferViaFile_ExportData(string TransferDirectory, ref string Error, ref string SQL)
        {
            SQL = "DECLARE @RC int " +
                "DECLARE @TableName varchar(200) " +
                "DECLARE @Schema varchar(200) " +
                "DECLARE @TargetPath varchar(200) " +
                "DECLARE @ProtocolFileName varchar(200) " +
                "SET @TableName = '" + this.TableName() + "' " +
                "SET @Schema = '" + this.Schema + "' " +
                "SET @TargetPath = '" + TransferDirectory + "' " +
                "SET @ProtocolFileName = 'Outfile.txt' " +
                "EXECUTE @RC = [dbo].[procBcpExport] " +
                "@TableName, @Schema, @TargetPath, @ProtocolFileName";
            string Message = "";
            //Message = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);

            bool OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_ExportFileExists(ref string Error, ref string SQL)
        {
            string Message = "";
            SQL = "SET ROLE \"CacheAdmin\"; " +
                "DROP TABLE IF EXISTS files; " +
                "CREATE TABLE files(filename text); " +
                "ALTER TABLE files OWNER to \"CacheAdmin\"; " +
                "COPY files FROM PROGRAM " +
                "'bash " + CacheDatabase.CacheDB.BulkTransferBashFile + " " +
                CacheDatabase.CacheDB.BulkTransferMountPoint + " " +
                "list " +
                DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "/" + this.Schema +
                "'; " + //-maxdepth 1 -type f -printf \"% f\\n\"'; " +
                "SELECT filename FROM files WHERE filename = '" + this.Target + ".csv' ; ";
            //SQL = "DROP TABLE IF EXISTS files; " +
            //    "CREATE TABLE files(filename text); " +
            //    "COPY files FROM PROGRAM 'find " + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "/" + this.Schema + " -maxdepth 1 -type f -printf \"% f\\n\"'; " +
            //    "SELECT filename FROM files WHERE filename = '" + this.Target + ".csv' ; ";
            bool OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, true, true, false);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            else
            {
                SQL = "SELECT filename FROM files WHERE filename = '" + this.Target + ".csv' ; ";
                Message = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                if (Message != this.Target + ".csv")
                {
                    OK = false;
                    Error += "\r\nExported file" + this.Target + ".csv not found";
                }
            }
            return OK;
        }

        private bool PostgresTransferViaFile_CreateTempTable(ref string Error, ref string SQL)
        {
            string Message = "";
            SQL = "CREATE TABLE IF NOT EXISTS \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" " +
                "(LIKE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\");";// INCLUDING INDEXES);  ";
            string SqlForLog = SQL;
            bool OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            if (!OK)
            {
                Error += Message;
                return OK;
            }
            // set the ownership
            SQL = "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" OWNER TO \"CacheAdmin\"";//_" + this.SchemaPostgres + "\"; ";
            //if (this.SchemaPostgres == "public")
            //    SQL += "\"";
            //else
            //    SQL += "_" + this.SchemaPostgres + "\"; ";
            SqlForLog += "\r\n\r\n" + SQL;
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            if (!OK)
            {
                Error += Message;
                return OK;
            }
            // clear table
            SQL = "DELETE FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\"";
            SqlForLog += "\r\n\r\n" + SQL;
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }

            SQL = SqlForLog;
            return OK;
        }

        private bool PostgresTransferViaFile_BashImport(ref string Error, ref string SQL)
        {
            string Message = "";
            SQL = "COPY \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" FROM PROGRAM " +
                "'bash " + CacheDatabase.CacheDB.BulkTransferBashFile + " " +
                CacheDatabase.CacheDB.BulkTransferMountPoint + " " +
                "conv " +
                DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "/" + this.Schema + "/" + this.Target + ".csv' " +
                "with delimiter E'\t' csv; ";
            bool OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_CompareCounts(ref string Error, ref string SQL)
        {
            bool OK = true;
            string Message = "";
            int NumberInView = 0;
            int NumberInTempTable = 0;
            SQL = "select count(*) from " + this.Schema + ".TransferView_" + this.Target;
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (int.TryParse(Result, out NumberInView))
            {
                SQL = "select count(*) from \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" ; ";
                Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message, true);
                if (int.TryParse(Result, out NumberInTempTable))
                {
                    if (NumberInTempTable != NumberInView)
                    {
                        OK = false;
                        Message = "Number in view (" + NumberInView.ToString() + 
                            ") does not match number of imported data (" +
                            NumberInTempTable.ToString() + ")";
                    }
                }
                else
                {
                    OK = false;
                    Message = "Failed to count data in " + this.SchemaPostgres + "." + this.TargetTemp + "; " + Message;
                }
            }
            else
            {
                OK = false;
                Message = "Failed to count data in TransferView_" + this.TargetTable + "; " + Message;
            }
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }



        private bool PostgresTransferViaFile_CreateBackup(ref string Error, ref string SQL)
        {
            string Message = "";
            SQL = "DROP TABLE IF EXISTS  \"" + this.SchemaPostgres + "\".\"" + this.Target + "_Backup\"; ";
            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

            SQL = "CREATE TABLE IF NOT EXISTS  \"" + this.SchemaPostgres + "\".\"" + this.Target + "_Backup\" " +
                " AS SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + this.Target + "\"";
            bool OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_ClearTable(string Schema, string Table, string TransferDirectory, ref string Error, ref string SQL)
        {
            string Message = "";
            bool CanTruncate = true;
            bool OK = false;
            SQL = "BEGIN; " +
                "TRUNCATE TABLE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\"; " +
                "END; ";
            CanTruncate = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
            if (!CanTruncate)
            {
                SQL = "BEGIN; " +
                "DELETE FROM \"" + this.SchemaPostgres + "\".\"" + this.Target + "\"; " +
                "END; ";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message, true, false, true);
            }
            else
                OK = CanTruncate;
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_GetPK(ref System.Data.DataTable dtPK, ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            SQL = "SELECT c.column_name " +
                "FROM information_schema.table_constraints tc " +
                "JOIN information_schema.constraint_column_usage AS ccu USING(constraint_schema, constraint_name) " +
                "JOIN information_schema.columns AS c ON c.table_schema = tc.constraint_schema " +
                "AND tc.table_name = c.table_name AND ccu.column_name = c.column_name " +
                "WHERE constraint_type = 'PRIMARY KEY' and tc.table_name = '" + this.Target + "' " +
                " AND c.table_schema = '" + this.SchemaPostgres + "'; ";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtPK, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_RemovePK(ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            SQL = "SELECT tc.constraint_name " +
                "FROM information_schema.table_constraints tc " +
                "WHERE tc.constraint_type = 'PRIMARY KEY' and tc.table_name = '" + this.Target + "' " +
                "and tc.table_schema = '" + this.SchemaPostgres + "'; ";
            string PK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
            if (PK.Length > 0)
            {
                SQL = "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" DROP CONSTRAINT IF EXISTS \"" + PK + "\"";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            }
            else
                OK = true;
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_EmptyDefautsPresent(ref string Error, ref string SQL, ref System.Collections.Generic.List<string> EmptyDefaultColumns)
        {
            bool TableContainsEmptyDefaults = false;
            // get defaults
            SQL = "SELECT column_name, column_default " +
                "FROM information_schema.columns " +
                "WHERE(table_schema, table_name) = ('" + this.SchemaPostgres + "', '" + this.Target + "') " +
                "AND NOT column_default IS NULL; ";
            System.Data.DataTable dtDefaults = new System.Data.DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtDefaults, ref Message);
            if (dtDefaults.Rows.Count > 0)
            { 
                SQL = "SELECT COUNT(*) FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\";";
                string All = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Error);
                int iAll;
                if (int.TryParse(All, out iAll) && iAll > 0)
                {
                    foreach (System.Data.DataRow R in dtDefaults.Rows)
                    {
                        SQL = "SELECT COUNT(*) FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\"" +
                            " WHERE \"" + R[0].ToString() + "\" IS NULL; ";
                        string NULL = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Error);
                        int iNULL;
                        if (int.TryParse(NULL, out iNULL) && iNULL == iAll)
                        {
                            EmptyDefaultColumns.Add(R[0].ToString());
                            TableContainsEmptyDefaults = true;
                        }
                    }
                }
            }
            return TableContainsEmptyDefaults;
        }

        private bool PostgresTransferViaFile_InsertData(ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            System.Collections.Generic.List<string> EmptyDefaultColumns = new List<string>();
            if (PostgresTransferViaFile_EmptyDefautsPresent(ref Error, ref SQL, ref EmptyDefaultColumns))
            {
                SQL = "SELECT column_name " +
                    "FROM information_schema.columns " +
                    "WHERE(table_schema, table_name) = ('" + this.SchemaPostgres + "', '" + this.Target + "') ; ";
                System.Data.DataTable dtColumns = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtColumns, ref Message);
                string Columns = "";
                foreach(System.Data.DataRow R in dtColumns.Rows)
                {
                    if (EmptyDefaultColumns.Contains(R[0].ToString()))
                        continue;
                    if (Columns.Length > 0)
                        Columns += ", ";
                    Columns += "\"" + R[0].ToString() + "\"";
                }
                SQL = "INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" " +
                    " (" + Columns + ") " +
                    "SELECT " + Columns + " FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\"; ";
            }
            else
            {
                SQL = "INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" " +
                        "SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\"; ";
            }
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_ConfirmTransfer(ref string Error, ref string SQL)
        {
            bool OK = false;
            try
            {
                string Message = "";
                int iTemp = 0;
                int iTab = 0;
                SQL = "SELECT COUNT(*) FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\"; ";
                string ResultTemp = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                SQL = "SELECT COUNT(*) FROM \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" ";
                string ResultTab = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                if (int.TryParse(ResultTemp, out iTemp) &&
                    int.TryParse(ResultTab, out iTab) &&
                    iTab == iTemp)
                    OK = true;
                else
                {
                    OK = false;
                    Message = "The number in " + this.TargetTemp + " (" + iTemp.ToString() + ") " +
                        "do not correspond to the number in " + this.Target + " (" + iTab.ToString() + ")";
                }
                if (!OK)
                {
                    Error += Message + ": " + SQL;
                }
            }
            catch(System.Exception ex)
            {

            }
            return OK;
        }

        // ensure that the table is logged - change to logged if persistence is unlogged
        private bool PostgresTransferViaFile_EnsureLogged(ref string Error, ref string SQL)
        {
            bool OK = true;
            try
            {
                string Message = "";
                SQL = "select relpersistence from pg_class c inner join pg_catalog.pg_namespace n on n.oid = c.relnamespace " +
                    "where relname = '" + this.Target + "' and n.nspname = '" + this.SchemaPostgres + "'; ";
                string Persistence = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
                if (Persistence.ToLower() == "u")
                {
                    // Change persistence to logged
                    SQL = "alter table \"" + this.SchemaPostgres + "\".\"" + this.Target + "\"  set logged; ";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_RestoreFromBackup(ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            SQL = "INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" " +
                    "SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + this.Target + "_Backup\"; ";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_RestorePK(System.Data.DataTable dtPK, ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            SQL = "";
            foreach (System.Data.DataRow R in dtPK.Rows)
            {
                if (SQL.Length > 0)
                    SQL += ", ";
                SQL += "\"" + R[0].ToString() + "\"";
            }
            SQL = "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" " +
                "ADD CONSTRAINT \"" + this.Target + "_pkey\" PRIMARY KEY(" + SQL + "); ";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_RemoveFile(string TransferDirectory, ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            SQL = "DECLARE @RC int; " +
                "DECLARE @TableName varchar(200); " +
                "DECLARE @Schema varchar(200); " +
                "DECLARE @TargetPath varchar(200); " +
                "SET @TableName = '" + this.Target + "'; " +
                "SET @Schema = '" + this.Schema + "'; " +
                "SET @TargetPath = '" + TransferDirectory + "'; " +
                "EXECUTE @RC = [dbo].[procBcpRemoveFile] " +
                "@TableName " +
                ",@Schema " +
                ",@TargetPath";
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_RemoveTempTable(ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            SQL = "DROP TABLE IF EXISTS \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\";  ";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }

        private bool PostgresTransferViaFile_RemoveBackup(ref string Error, ref string SQL)
        {
            string Message = "";
            bool OK = false;
            SQL = "DROP TABLE IF EXISTS \"" + this.SchemaPostgres + "\".\"" + this.Target + "_Backup\";  ";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
            {
                Error += Message + ": " + SQL;
            }
            return OK;
        }


        private void PostgresTransferViaFile_SetMessage(bcpStep Step, bool OK, string SQL = "")
        {
            try
            {
                string Message = "";
                if (OK)
                    Message = this.bcpStepMessages[Step] + ": Done";
                else
                    Message = "Failure at: " + this.bcpStepMessages[Step];

                if (this.I_Transfer != null)
                {
                    this.I_Transfer.SetInfo(Message);
                }
                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                {
                    if (!OK)
                        Message += "\r\nRole: " + DiversityWorkbench.PostgreSQL.Connection.CurrentRole();
                    if (SQL.Length > 0)
                        Message += "\r\nSQL:\r\n" + SQL;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("TransferStep", Step.ToString(), Message);
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        //private bool PostgresTransferSingle(ref string Error)
        //{
        //    bool OK = true;
        //    System.Collections.Generic.List<string> PK = new List<string>();
        //    DiversityWorkbench.Data.Table t = new DiversityWorkbench.Data.Table(this.TableName(), this.Schema, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
        //    PK = t.PrimaryKeyColumnList;
        //    string SQL = this._SqlTransfer;
        //    int Min;
        //    int Max;
        //    SQL = "SELECT MIN(" + PK[0] + ") FROM [" + this.Schema + "].[" + this._Target + "]";
        //    if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out Min))
        //    {
        //        SQL = "SELECT MAX(" + PK[0] + ") FROM [" + this.Schema + "].[" + this._Target + "]";
        //        if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out Max))
        //        {
        //            int i = Min;
        //            int diff = Max - Min;
        //            int PercentReached = 0;
        //            while (i <= Max)
        //            {
        //                bool Transferred = this.PostgresTransferSingle(i.ToString(), PK, ref Error);
        //                if (!Transferred)
        //                    OK = false;
        //                int Percent = (int)(100 * (((float)i - Min) / (float)diff));
        //                if (Percent > PercentReached)
        //                {
        //                    PercentReached = Percent;
        //                    if (this.I_Transfer != null)
        //                    {
        //                        this.I_Transfer.SetTransferProgress(PercentReached);
        //                        this.I_Transfer.SetInfo("Transfer: " + PK[0] + " = " + i.ToString());
        //                    }
        //                }
        //                i++;
        //            }
        //            if (OK && Error.Length == 0)
        //                this.PostgresTransferExchangeTempTable();
        //        }
        //    }

        //    return OK;
        //}

        //private bool PostgresTransferSingle(string ID, System.Collections.Generic.List<string> PK, ref string Error)
        //{
        //    bool OK = true;
        //    try
        //    {
        //        this._dtSource.Clear();
        //        this._adSource.SelectCommand.CommandText = this._SqlTransfer;
        //        if (this._SqlTransfer.ToLower().IndexOf(" where ") > -1)
        //            this._adSource.SelectCommand.CommandText += " AND ";
        //        else
        //            this._adSource.SelectCommand.CommandText += " WHERE ";
        //        this._adSource.SelectCommand.CommandText += " [" + PK[0] + "] = '" + ID + "'";
        //        this._adSource.Fill(this._dtSource);

        //        //this._Report += this._dtSource.Rows.Count.ToString();
        //        string SqlRow = "";
        //        string SqlRowValues = "";

        //        // transfer the data
        //        //if (this.I_Transfer != null)
        //        //    this.I_Transfer.SetInfo("Transfer: " + PK[0] + " = " + ID);

        //        int iRow = 0;
        //        foreach (System.Data.DataRow R in this._dtSource.Rows)
        //        {
        //            SqlRow = "";
        //            SqlRowValues = "";
        //            iRow++;
        //            System.Data.DataRow Rnew = this._dtTargetPG.NewRow();
        //            foreach (string s in this._TransferDataToPostgresColumns)
        //            {
        //                Rnew[s] = R[s];
        //                if (!R[s].Equals(System.DBNull.Value) && R[s].ToString().Length > 0)
        //                {
        //                    if (SqlRow.Length > 0)
        //                        SqlRow += ", ";
        //                    SqlRow += "\"" + s + "\"";
        //                    if (SqlRowValues.Length > 0)
        //                        SqlRowValues += ", ";
        //                    switch(R.Table.Columns[s].DataType.Name)
        //                    {
        //                        case "DateTime":
        //                            System.DateTime DT;
        //                            if (System.DateTime.TryParse(R[s].ToString(), out DT))
        //                            {
        //                                string DateValue = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString() + " T" + DT.Hour.ToString() + ":" + DT.Minute.ToString() + ":" + DT.Second.ToString();
        //                                SqlRowValues += "'" + DateValue + "'";
        //                            }
        //                            else
        //                                SqlRowValues += "'" + R[s].ToString().Replace("'", "''") + "'";
        //                            break;
        //                        case "Float":
        //                            SqlRowValues += "'" + ((float)R[s]).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace("'", "''") + "'";
        //                            break;
        //                        case "Double":
        //                            SqlRowValues += "'" + ((double)R[s]).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace("'", "''") + "'";
        //                            break;
        //                        case "Decimal":
        //                            SqlRowValues += "'" + ((decimal)R[s]).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace("'", "''") + "'";
        //                            break;
        //                        default:
        //                            SqlRowValues += "'" + R[s].ToString().Replace("'", "''") + "'";
        //                            break;
        //                    }
        //                    //if (R.Table.Columns[s].DataType.Name == "DateTime")
        //                    //{
        //                    //    System.DateTime DT;
        //                    //    if (System.DateTime.TryParse(R[s].ToString(), out DT))
        //                    //    {
        //                    //        string DateValue = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString() + " T" + DT.Hour.ToString() + ":" + DT.Minute.ToString() + ":" + DT.Second.ToString();
        //                    //        SqlRowValues += "'" + DateValue + "'";
        //                    //    }
        //                    //    else
        //                    //        SqlRowValues += "'" + R[s].ToString().Replace("'", "''") + "'";
        //                    //}
        //                    //else
        //                    //{
        //                    //    SqlRowValues += "'" + R[s].ToString().Replace("'", "''") + "'";
        //                    //}
        //                }
        //                else if (PK.Contains(s)) ///*!R[s].Equals(System.DBNull.Value) && R[s].ToString().Length > 0 &&*/ this._dtSource.PrimaryKey.Contains(this._dtSource.Columns[s]))
        //                {
        //                    if (SqlRow.Length > 0)
        //                        SqlRow += ", ";
        //                    SqlRow += "\"" + s + "\"";
        //                    if (SqlRowValues.Length > 0)
        //                        SqlRowValues += ", ";
        //                    SqlRowValues += "''";
        //                }
        //            }
        //            string Schema = this._SchemaPostgres;
        //            if (Schema == null)
        //                Schema = this._Schema;
        //            string SQL = "INSERT INTO \"" + Schema + "\".\"" + this.TargetTemp + "\" (" + SqlRow + ") VALUES (" + SqlRowValues + ");";
        //            string Message = "";
        //            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQueryWithSameConnection(SQL, ref Message);
        //            if (!OK && Message.Length > 0)
        //            {
        //                Error += Message + ": " + SQL + "\r\n";
        //                this._Errors += Error;
        //            }
        //            //int PercentReached = (int)(100 * (float)iRow / (float)this._dtSource.Rows.Count);
        //            //if (this.I_Transfer != null)
        //            //{
        //            //    this.I_Transfer.SetTransferProgress(PercentReached);
        //            //}
        //        }
        //        //bool TransferSuccessful = true;
        //        //try
        //        //{
        //        //        if (this.I_Transfer != null)
        //        //            this.I_Transfer.SetInfo("Updating postgres");
        //        //        this._adTargetPG.Update(this._dtTargetPG);
        //        //}
        //        //catch (System.Exception ex)
        //        //{
        //        //    OK = false;
        //        //    if (Error.Length > 0)
        //        //        Error += "\r\n\r\n";
        //        //    Error += ex.Message;
        //        //    TransferSuccessful = false;
        //        //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        //}
        //        //string SqlInsert = "";
        //        //if (!TransferSuccessful)
        //        //{
        //        //    SqlInsert = "INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" (" + SqlRow + ") VALUES (" + SqlRowValues + ")";
        //        //    TransferSuccessful = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SqlInsert, ref Error);
        //        //}
        //        //if (Error.Length > 0)
        //        //{
        //        //    OK = false;
        //        //    if (this.I_Transfer != null)
        //        //        this.I_Transfer.SetMessage(Error);
        //        //    this._Report += Error;
        //        //}
        //        //else
        //        //{
        //        //    this.PostgresTransferExchangeTempTable();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        OK = false;
        //        if (Error.Length > 0)
        //            Error += "\r\n\r\n";
        //        Error += ex.Message;
        //        if (this.I_Transfer != null)
        //            this.I_Transfer.SetTransferState(TransferState.Failed);
        //    } 
        //    return OK;
        //}


        #region Preparations
        private bool PostgresPrepareTargetTable()
        {
            bool OK = true;
            try
            {
                string SqlPG = "";
                foreach (string C in this._TransferDataToPostgresColumns)// TransferColumns)
                {
                    if (SqlPG.Length > 0) SqlPG += ", ";
                    SqlPG += "\"" + C + "\"";
                }
                SqlPG = "SELECT " + SqlPG;
                SqlPG += " FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" WHERE 1=0";

                // creating the target table
                this._adTargetPG = new Npgsql.NpgsqlDataAdapter(SqlPG, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                this._dtTargetPG = new System.Data.DataTable();
                _adTargetPG.Fill(_dtTargetPG);
                _adTargetPG.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                Npgsql.NpgsqlCommandBuilder cb = new Npgsql.NpgsqlCommandBuilder(_adTargetPG);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        /// <summary>
        /// Check if the temporary table for the transfer is available, resp. create and clear it
        /// </summary>
        /// <returns>If preparation was successful</returns>
        private bool PrepareTransferToPostgres()
        {
            bool OK = true;
            string Error = "";
            try
            {
                string SQL = "";

                // Remove temporary tables from previous transfer if deleting these tables failed - unless logging is on
                if (!DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                {
                    System.Data.DataTable OldChunks = new System.Data.DataTable();
                    SQL = "select T.table_name from information_schema.tables T " +
                        "where T.table_name like '" + this.Target + "_Chunk_%' " +
                        "and T.table_schema = '" + this.SchemaPostgres + "'";
                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref OldChunks, ref Error);
                    foreach (System.Data.DataRow R in OldChunks.Rows)
                    {
                        this.PostgresTransferDataDropChunkTable(R[0].ToString());
                    }
                }

                // Include test for existence in SQL statement
                SQL = "CREATE TABLE IF NOT EXISTS \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" " +
                    "(LIKE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" INCLUDING DEFAULTS INCLUDING INDEXES);  ";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

                if (OK)
                {
                    // Check if column LogInsertedWhen does exist
                    SQL = "select count(*) from information_schema.columns c where c.table_name = '" + this.TargetTemp + "'  and c.table_schema = '" + this.SchemaPostgres + "' and c.column_name = 'LogInsertedWhen'";
                    string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                    if (Result == "1")
                    {
                        SQL = "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" ALTER COLUMN \"LogInsertedWhen\" SET DEFAULT (now())::timestamp without time zone;";
                        OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                    }
                }

                // clear the table
                SQL = "DELETE FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\"";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

                // set the ownership
                SQL = "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" OWNER TO \"CacheAdmin\"";//_" + this.SchemaPostgres + "\"; ";
                //if (this.SchemaPostgres == "public")
                //    SQL += "\"";
                //else
                //    SQL += "_" + this.SchemaPostgres + "\"; ";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                if (!OK)
                {
                    SQL = "select tableowner from pg_tables where tablename = '" + this.TargetTemp + "' and schemaname = '" + this.SchemaPostgres + "'";
                    string Owner = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                    if (Owner == "CacheAdmin" || Owner == "" || Owner.StartsWith("CacheAdmin"))
                        OK = true;
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message;
            }
            if (Error.Length > 0)
                this._Report += "\r\n" + Error;
            if (this.I_Transfer != null)
                this.I_Transfer.SetInfo(this.TargetTemp + " created");
            return OK;
        }

        #endregion

        #region Transfer without chunks
        private bool PostgresTransferWithoutChunks(ref string Error)
        {
            bool OK = true;
            bool NewVersionWithInsertInsteadOfUpdate = true;
            System.Collections.Generic.List<string> PK = new List<string>();
            if (NewVersionWithInsertInsteadOfUpdate)
            {
                DiversityWorkbench.Data.Table t = new DiversityWorkbench.Data.Table(this.TableName(), this.Schema, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                PK = t.PrimaryKeyColumnList;
            }

            try
            {
                this._adSource.Fill(this._dtSource);


                this._Report += this._dtSource.Rows.Count.ToString();
                string SqlRow = "";
                string SqlRowValues = "";
                string CheckPK = "";
                System.Collections.Generic.Dictionary<string, string> _NotNullColumns = new Dictionary<string, string>();
                System.Data.DataTable _DtNotNullColumns = new System.Data.DataTable();
                string SqlNotNull = "SELECT column_name, data_type " +
                    "FROM information_schema.columns " +
                    "WHERE table_name = '" + this.TableName() + "' " +
                    "and table_schema = '" + this._SchemaPostgres + "' and column_default is null and is_nullable = 'NO'; "; // von Toni übernommen: this._Schema statt public, E-mail 25.05.2021, 12:48
                string Ex = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SqlNotNull, ref _DtNotNullColumns, ref Ex);
                foreach(System.Data.DataRow R in _DtNotNullColumns.Rows)
                {
                    _NotNullColumns.Add(R[0].ToString(), R[1].ToString());
                }

                // transfer the data
                if (this.I_Transfer != null)
                    this.I_Transfer.SetInfo("Transfer of data");

                int iRow = 0;
                foreach (System.Data.DataRow R in this._dtSource.Rows)
                {
                    SqlRow = "";
                    SqlRowValues = "";
                    CheckPK = "";
                    iRow++;
                    System.Data.DataRow Rnew = this._dtTargetPG.NewRow();
                    foreach (string s in this._TransferDataToPostgresColumns)//._TransferColumns)
                    {
                        Rnew[s] = R[s];
                        if (!R[s].Equals(System.DBNull.Value) && R[s].ToString().Length > 0)
                        {
                            if (SqlRow.Length > 0)
                                SqlRow += ", ";
                            SqlRow += "\"" + s + "\"";
                            if (PK.Contains(s))
                            {
                                if (CheckPK.Length > 0)
                                    CheckPK += " AND ";
                                CheckPK += "\"" + s + "\" = ";
                            }
                            if (SqlRowValues.Length > 0)
                                SqlRowValues += ", ";
                            if (R.Table.Columns[s].DataType.Name == "DateTime")
                            {
                                System.DateTime DT;
                                if (System.DateTime.TryParse(R[s].ToString(), out DT))
                                {
                                    string DateValue = DT.Year.ToString() + "-" + DT.Month.ToString() + "-" + DT.Day.ToString() + " T" + DT.Hour.ToString() + ":" + DT.Minute.ToString() + ":" + DT.Second.ToString();
                                    SqlRowValues += "'" + DateValue + "'";
                                    if (PK.Contains(s))
                                        CheckPK += "'" + DateValue + "'";
                                }
                                else
                                {
                                    SqlRowValues += "'" + R[s].ToString().Replace("'", "''") + "'";
                                    if (PK.Contains(s))
                                        CheckPK += "'" + R[s].ToString().Replace("'", "''") + "'";
                                }
                            }
                            else if (R.Table.Columns[s].DataType.Name == "Double")
                            {
                                double D;
                                if (double.TryParse(R[s].ToString(), out D))
                                {
                                    SqlRowValues += D.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);// double.Parse(R[s].ToString(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                    if (PK.Contains(s))
                                        CheckPK += D.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                }
                                else
                                {
                                    SqlRowValues += "''";
                                    if (PK.Contains(s))
                                        CheckPK += "''";
                                }
                            }
                            else
                            {
                                SqlRowValues += "'" + R[s].ToString().Replace("'", "''") + "'";
                                if (PK.Contains(s))
                                    CheckPK += "'" + R[s].ToString().Replace("'", "''") + "'";
                            }
                        }
                        else if (PK.Contains(s)) ///*!R[s].Equals(System.DBNull.Value) && R[s].ToString().Length > 0 &&*/ this._dtSource.PrimaryKey.Contains(this._dtSource.Columns[s]))
                        {
                            if (SqlRow.Length > 0)
                                SqlRow += ", ";
                            SqlRow += "\"" + s + "\"";
                            if (SqlRowValues.Length > 0)
                                SqlRowValues += ", ";
                            SqlRowValues += "''";
                            if (CheckPK.Length > 0)
                                CheckPK += " AND ";
                            CheckPK += "\"" + s + "\"" + " = ''";
                        }
                        else
                        {
                            if (_NotNullColumns.ContainsKey(s))
                            {
                                switch(_NotNullColumns[s])
                                {
                                    case "character varying":
                                        if (SqlRow.Length > 0)
                                            SqlRow += ", ";
                                        SqlRow += "\"" + s + "\"";
                                        if (SqlRowValues.Length > 0)
                                            SqlRowValues += ", ";
                                        SqlRowValues += "''";
                                        break;
                                    default:
                                        if (SqlRow.Length > 0)
                                            SqlRow += ", ";
                                        SqlRow += "\"" + s + "\"";
                                        if (SqlRowValues.Length > 0)
                                            SqlRowValues += ", ";
                                        SqlRowValues += "''";
                                        break;
                                }
                            }
                        }
                    }
                    if (NewVersionWithInsertInsteadOfUpdate)
                    {
                        //string Schema = this._SchemaPostgres;
                        //if (Schema == null)
                        //    Schema = this._Schema;
                        //string SQL = "INSERT INTO \"" + Schema + "\".\"" + this.TargetTemp + "\" (" + SqlRow + ") VALUES (" + SqlRowValues + ");";
                        // Markus 6.2.2020 - no insert if there are data with the same key
                        string SQL = "INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" (" + SqlRow + ") " +
                            " SELECT " + SqlRowValues + " WHERE NOT EXISTS(SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" WHERE " + CheckPK + ");";
                        string Message = "";
                        OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQueryWithSameConnection(SQL, ref Message);
                        if (!OK && Message.Length > 0)
                        {
                            Error += Message + ": " + SQL + "\r\n";
                            this._Errors += Error;
                        }
                    }
                    else
                    {
                        this._dtTargetPG.Rows.Add(Rnew);
                    }
                    int PercentReached = (int)(100 * (float)iRow / (float)this._dtSource.Rows.Count);
                    if (this.I_Transfer != null)
                    {
                        this.I_Transfer.SetTransferProgress(PercentReached);
                    }
                }
                bool TransferSuccessful = true;
                try
                {
                    if (!NewVersionWithInsertInsteadOfUpdate)
                    {
                        if (this.I_Transfer != null)
                            this.I_Transfer.SetInfo("Updating postgres");
                        this._adTargetPG.Update(this._dtTargetPG);
                    }
                    else
                    {
                        if (this.I_Transfer != null)
                            this.I_Transfer.SetInfo("Transfer finished");
                        DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQueryCloseConnection();
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                    if (Error.Length > 0)
                        Error += "\r\n\r\n";
                    Error += ex.Message;
                    TransferSuccessful = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                string SqlInsert = "";
                if (!TransferSuccessful)
                {
                    SqlInsert = "INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" (" + SqlRow + ") VALUES (" + SqlRowValues + ")";
                    TransferSuccessful = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SqlInsert, ref Error);
                }
                if (Error.Length > 0)
                {
                    OK = false;
                    if (this.I_Transfer != null)
                        this.I_Transfer.SetMessage(Error);
                    this._Report += Error;
                }
                else
                {
                    if (!this.PostgresTransferExchangeTempTable(ref Error))
                        OK = false;
                }
                //OK = this.PostgresTransferExchangeTempTable();
            }
            catch (Exception ex)
            {
                OK = false;
                if (Error.Length > 0)
                    Error += "\r\n\r\n";
                Error += ex.Message;
                if (this.I_Transfer != null)
                    this.I_Transfer.SetTransferState(TransferState.Failed);
            }
            return OK;
        }

        #endregion

        private string _SqlTransfer;
        public string SqlTransfer() { return this._SqlTransfer; }

        private bool PostgresPrepareSourceTable(ref string SQL)
        {
            bool OK = true;

            try
            {
                //string SQL = "";
                foreach (string C in this._TransferDataToPostgresColumns)// TransferColumns)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += "T.[" + C + "]";
                }
                SQL = "SELECT DISTINCT " + SQL;
                SQL += " FROM [" + this.Schema + "].[" + this._Target + "] AS T ";
                if (this.SourceView != null && this.SourceView.Length > 0)
                    SQL += " WHERE SourceView = '" + this.SourceView + "'";
                this._SqlTransfer = SQL;
                // Source table
                this._adSource = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                this._dtSource = new System.Data.DataTable();
            }
            catch (Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public string TableName() { return this._Target; }

        private bool PostgresPrepareTargetChunkTable(string ChunkTable)
        {
            bool OK = true;
            Npgsql.NpgsqlCommandBuilder cb = null;
            try
            {
                string SqlPG = "";
                foreach (string C in this._TransferDataToPostgresColumns)// TransferColumns)
                {
                    if (SqlPG.Length > 0) SqlPG += ", ";
                    SqlPG += "\"" + C + "\"";
                }
                SqlPG = "SELECT " + SqlPG;
                SqlPG += " FROM \"" + this.SchemaPostgres + "\".\"" + ChunkTable + "\" WHERE 1=0";

                // creating the target table
                this._adTargetChunkPG = new Npgsql.NpgsqlDataAdapter(SqlPG, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                this._dtTargetChunkPG = new System.Data.DataTable();
                if (this._adTargetChunkPG.SelectCommand.Connection.State == System.Data.ConnectionState.Closed)
                    this._adTargetChunkPG.SelectCommand.Connection.Open();
                this._adTargetChunkPG.Fill(this._dtTargetChunkPG);
                this._adTargetChunkPG.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                cb = new Npgsql.NpgsqlCommandBuilder(this._adTargetChunkPG);
            }
            catch (Exception ex)
            {
                OK = false;
            }
            finally
            {
                //cb.Dispose();
                //if (this._adTargetChunkPG != null
                //    && this._adTargetChunkPG.SelectCommand != null
                //    && this._adTargetChunkPG.SelectCommand.Connection != null
                //    && this._adTargetChunkPG.SelectCommand.Connection.State == System.Data.ConnectionState.Open)
                //    this._adTargetChunkPG.SelectCommand.Connection.Close();
            }
            return OK;
        }
        
        private System.Collections.Generic.List<string> _TransferDataToPostgresColumns;

        private bool initTransferDataToPostgresColumns()
        {
            bool OK = true;
            try
            {
                this._TransferDataToPostgresColumns = new List<string>();
                // Getting all columns that exist in the Postgres and the SqlServer Cache database
                System.Data.DataTable DtColPG = new System.Data.DataTable();
                string SQL = "SELECT column_name FROM information_schema.columns " +
                    "WHERE table_schema = '" + this.SchemaPostgres + "' AND table_name = '" + this._Target + "'  " +
                    "ORDER BY column_name";
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref DtColPG, ref Message);

                // Getting the columns from SQL Server
                SQL = "select C.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS C " +
                    "where C.TABLE_NAME = '" + this._Target + "' " +
                    "and C.TABLE_SCHEMA = '" + this.Schema + "' " +
                    "order by C.COLUMN_NAME";
                System.Data.DataTable DtColCache = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(DtColCache);

                // Comparing the two tables and getting the intersection
                foreach (System.Data.DataRow Rcol in DtColPG.Rows)
                {
                    System.Data.DataRow[] rr = DtColCache.Select("COLUMN_NAME = '" + Rcol[0].ToString() + "'");
                    if (rr.Length > 0 && !this.SuppressedColumns.Contains(Rcol[0].ToString()))
                        _TransferDataToPostgresColumns.Add(Rcol[0].ToString());
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool TransferDataToPostgresInChunks(
            string SQL, 
            int NumberOfFailedChunks)
        {
            bool OK = true;
            string Error = "";

            if (this._FailedChunks.Count == 0)
            {
                for (int i = 0; i < Chunks.Count - 1; i++)
                {
                    this.PostgresTransferTableChunk(i, SQL);
                    //this.PostgresTransferChunk(i, SQL);//, /*Chunks, ad,*/ dtSource, adPG, dtTarget, adTrans, TransferColumns);
                }
                if (this._FailedChunks.Count > 0)
                    OK = this.TransferDataToPostgresInChunks(SQL, /*dtSource, dtTarget, adPG, adTrans, TransferColumns, Chunks,*/ this._FailedChunks.Count);
            }
            else
            {
                OK = true;
                int[] ChunksToDo = new int[_FailedChunks.Count];
                _FailedChunks.CopyTo(ChunksToDo);
                _FailedChunks.Clear();
                for (int c = 0; c < ChunksToDo.Length; c++)
                {
                    this.PostgresTransferTableChunk(ChunksToDo[c], SQL);
                    //this.PostgresTransferChunk(ChunksToDo[c], SQL);//, /*Chunks, ad,*/ dtSource, adPG, dtTarget, adTrans, TransferColumns);
                }
                if (_FailedChunks.Count > 0)
                {
                    if (_FailedChunks.Count < NumberOfFailedChunks)
                        OK = this.TransferDataToPostgresInChunks(SQL, /*dtSource, dtTarget, adPG, adTrans, TransferColumns, Chunks,*/ _FailedChunks.Count);
                    else
                    {
                        OK = false;
                        foreach (int i in _FailedChunks)
                        {
                            if (_FailedChunkErrors.ContainsKey(i))
                                Error += "Failed chunk " + i.ToString() + ": " + _FailedChunkErrors[i];
                        }
                    }
                }
            }
            if (this.I_Transfer != null)
            {
                if (_FailedChunks.Count == 0)
                    this.I_Transfer.SetTransferState(TransferState.Successfull);
                else
                    this.I_Transfer.SetTransferState(TransferState.Error);
            }
            return OK;
        }

        /// <summary>
        /// Transfers the data into a temporary table for the chunk and then as final step into the temp table
        /// therefore creating and deleting the temporary chunk table
        /// </summary>
        /// <param name="i">the number of the chunk</param>
        /// <param name="SQL">The SQL command for the data transfer</param>
        /// <returns></returns>
        private bool PostgresTransferTableChunk(int i, string SQL)
        {
            bool OK = true;
            string ChunkFailure = "";

            if (this.I_Transfer != null)
                this.I_Transfer.SetInfo("Chunk " + i.ToString() + " of " + this.Chunks.Count.ToString());

            // Preparation of the chunk table
            string ChunkTable = "";
            if (!this.CreateTransferToPostgresChunkTable(i, ref ChunkTable))
                return false;

            // clearing the source table
            this._dtSource.Rows.Clear();
            // prepare the target table
            if (!this.PostgresPrepareTargetChunkTable(ChunkTable))
                return false;

            // Inclusion of Chunk borders in SQL
            if (SQL.IndexOf(" WHERE ") > -1)
                SQL += " AND ";
            else SQL += " WHERE ";
            SQL += this.CountColumn() + " BETWEEN " + this.Chunks[i].ToString() + " AND " + (this.Chunks[i + 1] - 1).ToString();

            // filling the target table
            this._adSource.SelectCommand.CommandText = SQL;
            this._adSource.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
            //if (SQL.IndexOf(" WHERE ") > -1)
            //    this._adSource.SelectCommand.CommandText += " AND ";
            //else this._adSource.SelectCommand.CommandText += " WHERE ";
            //this._adSource.SelectCommand.CommandText += this.CountColumn() + " BETWEEN " + this.Chunks[i].ToString() + " AND " + (this.Chunks[i + 1] - 1).ToString();
            try
            {
                this._adSource.Fill(this._dtSource);
                // transfer the data
                foreach (System.Data.DataRow R in this._dtSource.Rows)
                {
                    System.Data.DataRow Rnew = this._dtTargetChunkPG.NewRow();
                    foreach (string s in this._TransferDataToPostgresColumns)
                        Rnew[s] = R[s];
                    this._dtTargetChunkPG.Rows.Add(Rnew);
                }
                this._adTargetChunkPG.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres;
                if (this._adTargetChunkPG.SelectCommand.Connection.State == System.Data.ConnectionState.Closed)
                    this._adTargetChunkPG.SelectCommand.Connection.Open();
                try
                {
                    this._adTargetChunkPG.Update(this._dtTargetChunkPG);
                }
                catch (System.Exception ex)
                {
                }

                this._adTargetPG.SelectCommand.Connection.Close();
                double Percent = ((double)i + 1) / (double)this.Chunks.Count;
                if (this.I_Transfer != null)
                    this.I_Transfer.SetTransferProgress((int)(Percent * 100));
            }
            catch (Npgsql.NpgsqlException ex)
            {
                OK = false;
                ChunkFailure = ex.Message + this._adSource.SelectCommand.CommandText + "\r\n\r\n";
                if (_FailedChunkErrors.ContainsKey(i))
                    _FailedChunkErrors[i] += ChunkFailure;
                else
                    _FailedChunkErrors.Add(i, ChunkFailure);
                _FailedChunks.Add(i);
            }
            catch (System.Exception ex)
            {
                OK = false;
                ChunkFailure = ex.Message + this._adSource.SelectCommand.CommandText + "\r\n\r\n";
                if (_FailedChunkErrors.ContainsKey(i))
                    _FailedChunkErrors[i] += ChunkFailure;
                else
                    _FailedChunkErrors.Add(i, ChunkFailure);
                _FailedChunks.Add(i);
            }
            finally
            {
                if (ChunkFailure.Length > 0)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("PostgresTransferChunk", "Chunk " + i.ToString() + "; SQL: " + SQL, ChunkFailure);
            }

            if (OK)
            {
                OK = PostgresTransferDataFromChunkToTemp(ChunkTable);
            }

            return OK;
        }

        private bool PostgresTransferDataFromChunkToTemp(string ChunkTable)
        {
            bool OK = false;
            string Message = "";
            string SQL = "INSERT INTO  \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + ChunkTable + "\"";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
            {
                SQL = "DROP TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTable + "\"";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            }
            else
                CacheDB.LogEvent("PostgresTransferDataFromChunkToTemp", this.TargetTemp, "TransferStep failed: " + SQL);
            return OK;
        }

        public bool PostgresTransferDataDropChunkTable(string ChunkTable)
        {
            bool OK = false;
            string Message = "";
            string SQL = "DO LANGUAGE plpgsql " +
                "$$ " +
                "BEGIN " +
                "if (select count(*) from information_schema.Tables T where T.table_schema = '" + this.SchemaPostgres + "' and T.table_name = '" + ChunkTable + "') > 0 " +
                "then " +
                "DROP TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTable + "\"; " + 
                "end if;  " +
                "END; " +
                "$$;";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            if (!OK)
                CacheDB.LogEvent("PostgresTransferDataDropChunkTable", ChunkTable, "Droping " + ChunkTable + " failed: " + SQL);
            return OK;
        }

        private bool CreateTransferToPostgresChunkTable(int iChunk, ref string ChunkTableName)
        {
            bool OK = true;
            string Error = "";
            ChunkTableName = this.Target + "_Chunk_" + iChunk.ToString();
            try
            {
                // Test if the table exists - otherwise create it
                string SQL = "select count(*) " +
                    "from information_schema.tables T " +
                    "where T.table_schema = '" + this.SchemaPostgres + "' and T.table_name = '" + ChunkTableName + "'";
                string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                if (Result == "0")
                {
                    SQL = "CREATE TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\" " +
                        "(LIKE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" INCLUDING INDEXES);  ";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                }

                // clear the table
                SQL = "DELETE FROM \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\"";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

                // set the ownership
                SQL = "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\" OWNER TO \"CacheAdmin\"; "; //_" + this.SchemaPostgres + "\"; ";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message;
            }
            if (Error.Length > 0)
                this._Report += "\r\n" + Error;
            if (this.I_Transfer != null)
                this.I_Transfer.SetInfo(this.TargetTemp + " created");
            return OK;
        }

        #endregion
       
        #region Postges - on basis of Transfer chunks

        #region Finding the chunks

        private System.Data.DataTable GetChunksViaSql(ref string Error, string Schema, string TableName, string CountColumn, int ChunkSize)
        {
            string SQL = "declare @IDs table(ID int PRIMARY KEY NOT NULL) " +
                "insert into @IDs select distinct [" + CountColumn + "] from [" + Schema + "].[" + TableName + "]; " +
                "declare @Chunks table(ID int PRIMARY KEY NOT NULL) " +
                "declare @Min int; " +
                "set @Min = (select min([ID]) from @IDs); " +
                "declare @ID int; " +
                "set @ID = (select max([ID]) from @IDs); " +
                "while (@ID > @Min and (select count(*) from @IDs) > 0 and not @ID is null) " +
                "begin " +
                "insert into @Chunks(ID) values(@ID); " +
                "delete from @IDs where ID > @ID; " +
                "set @ID = (select min([ID]) from @IDs WHERE ID IN (select top " + ChunkSize.ToString() + " ID from @IDs order by [ID] desc)); " +
                "end " +
                "insert into @Chunks(ID) values(@Min); " +
                "select ID from @Chunks order by ID;";
            System.Data.DataTable DtChunks = new System.Data.DataTable();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref DtChunks, ref Error);
            return DtChunks;
        }

        private int PostgresFindUpperStartForChunks(ref string ErrorMessage)
        {
            int iFrom = 0;

            try
            {
                string SqlCount = "SELECT MAX(" + this.CountColumn() + ") FROM  [" + this.Schema + "].[" + this._Target + "]";
                if (this.SourceView != null && this.SourceView.Length > 0)
                    SqlCount += " WHERE SourceView = '" + this.SourceView + "'";
                if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out iFrom))
                    ErrorMessage = "Search for start failed";
            }
            catch (Exception ex)
            {
                ErrorMessage += ex.Message;
            }
            return iFrom;
        }

        private int PostgresFindLowerEndForChunk(int UpperStartID, ref string ErrorMessage)
        {
            int LowerEndID = 0;
            try
            {
                string SQL = "select min([" + this.CountColumn() + "]) from [" + this.Schema + "].[" + this._Target + "] " +
                    "where [" + this.CountColumn() + "] in (" +
                    "select top " + DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizePostgres.ToString() + " [" + this.CountColumn() + "] from [" + this.Schema + "].[" + this._Target + "] where [" + this.CountColumn() + "] < " + UpperStartID.ToString();
                if (this.SourceView != null && this.SourceView.Length > 0)
                    SQL += " AND SourceView = '" + this.SourceView + "' ";
                SQL += " order by " + this.CountColumn() + " desc) ";
                if (this.SourceView != null && this.SourceView.Length > 0)
                    SQL += " AND SourceView = '" + this.SourceView + "' ";
                LowerEndID = int.Parse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref ErrorMessage).ToString());
            }
            catch (System.Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return LowerEndID;
        }

        private int PostgresFindLowerRestForChunks(int LowestValueOfID, ref string ErrorMessage)
        {
            int iRest = 0;
            try
            {
                string SqlCount = "SELECT COUNT(*) FROM  [" + this.Schema + "].[" + this._Target + "] WHERE " + this.CountColumn() + " < " + LowestValueOfID.ToString();
                if (this.SourceView != null && this.SourceView.Length > 0)
                    SqlCount += " AND SourceView = '" + this.SourceView + "'";
                if (!int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlCount), out iRest))
                    ErrorMessage = "Search for rest failed";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return iRest;
        }
        
        private System.Collections.Generic.SortedList<int, TransferChunk> _TransferChunks;

        public System.Collections.Generic.SortedList<int, TransferChunk> TransferChunks
        {
            get 
            {
                if (this._TransferChunks == null)
                    this._TransferChunks = new SortedList<int, TransferChunk>();
                return _TransferChunks; 
            }
            set { _TransferChunks = value; }
        }

        private bool PostgresFindTransferChunks(ref string Error)
        {
            bool OK = true;
            if (this.I_Transfer != null)
                this.I_Transfer.SetInfo("Determination of the chunks");

            System.Data.DataTable dtChunks = this.GetChunksViaSql(ref Error, this.Schema, this.TableName(), this.CountColumn(), DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizePostgres);
            try
            {
                System.Data.DataRow[] RR = dtChunks.Select("", "ID");
                int Min;
                int.TryParse(RR[0]["ID"].ToString(), out Min);
                int Max = Min;
                this.TransferChunks.Clear();
                for (int i = 1; i < RR.Length; i++ )
                {
                    int.TryParse(RR[i]["ID"].ToString(), out Max);
                    TransferChunk T = new TransferChunk(this.TableName() + "_Chunk_" + i.ToString(), "_1", Min, Max, this);
                    this.TransferChunks.Add(i, T);
                    Min = Max + 1;
                    if (this.LoggingIsOn)
                        CacheDB.LogEvent("PostgresFindTransferChunks", "Chunk " + i.ToString() + " determined", "Range: " + T.StartID().ToString() + " - " + T.EndID().ToString());
                }

                if (this.I_Transfer != null)
                    this.I_Transfer.SetInfo(this.TransferChunks.Count.ToString() + " chunks determined");
            }
            catch (System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;






            try
            {
                if (this.I_Transfer != null)
                    this.I_Transfer.SetInfo("Determination of the chunks");

                // getting the starting point
                int iTop = this.PostgresFindUpperStartForChunks(ref Error);// this.PostgresFindStartForChunks(ref Error);
                if (Error.Length > 0)
                {
                    this._Report += "\r\n" + Error;
                    return false;
                }

                // getting the rest
                int iRest = this.PostgresFindLowerRestForChunks(iTop, ref Error);// this.PostgresFindRestForChunks(iFrom, ref Error);
                if (Error.Length > 0)
                {
                    this._Report += "\r\n" + Error;
                    return false;
                }

                int iPosition = 1;
                // getting the chunks
                while (iRest > 0)
                {
                    int iBase = this.PostgresFindLowerEndForChunk(iTop, ref Error);// this.PostgresFindEndForChunk(iFrom, ref Error);
                    if (Error.Length > 0)
                    {
                        this._Report += "\r\n" + Error;
                        return false;
                    }
                    TransferChunk T = new TransferChunk(this.TableName() + "_Chunk_" + iPosition.ToString(), "_1", iBase, iTop, this);
                    this.TransferChunks.Add(iPosition, T);
                    // Check for any overlap
                    if (iPosition > 1)
                    {
                        if (this.TransferChunks[iPosition - 1].StartID() <= this.TransferChunks[iPosition].EndID())
                        {
                            CacheDB.LogEvent("PostgresFindTransferChunks", "Chunk " + iPosition.ToString() + " with overlap", (iPosition - 1).ToString() + " Start = " + this.TransferChunks[iPosition - 1].StartID().ToString() + ". " + (iPosition).ToString() + " End = " + this.TransferChunks[iPosition].EndID().ToString());
                        }
                    }

                    iTop = iBase - 1;
                    iRest = this.PostgresFindLowerRestForChunks(iTop, ref Error);// this.PostgresFindRestForChunks(iFrom, ref Error);
                    if (Error.Length > 0)
                    {
                        this._Report += "\r\n" + Error;
                        return false;
                    }
                    if (this.I_Transfer != null)
                        this.I_Transfer.SetInfo("Range determined for chunk " + this.TransferChunks.Count.ToString() + ": " + T.StartID().ToString() + " - " + T.EndID().ToString());
                    if (this.LoggingIsOn)
                        CacheDB.LogEvent("PostgresFindTransferChunks", "Chunk " + iPosition.ToString() + " determined", "Range: " + T.StartID().ToString() + " - " + T.EndID().ToString());
                    iPosition++;
                }
                if (this.I_Transfer != null)
                    this.I_Transfer.SetInfo(this.TransferChunks.Count.ToString() + " chunks determined");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //OK = false;
            }
            return OK;
        }

        #endregion

        private bool PostgresTransferInChunks(int TotalCount, string SQL, ref string Error)
        {
            bool OK = true;

            try
            {
                string Message = "";
                OK = this.PostgresRemoveOldChunkTables(ref Message);
                if (!OK)
                {
                    System.Windows.Forms.MessageBox.Show(Message);
                    return OK;
                }

                // darf hier nicht gelöscht werden
                //OK = this.PostgresRemoveOldTempTables(ref Message);
                //if (!OK)
                //{
                //    System.Windows.Forms.MessageBox.Show(Message);
                //    return OK;
                //}

                if (!this.PostgresFindTransferChunks(ref Error))
                    return false;
                if (this.LoggingIsOn)
                {
                    CacheDB.LogEvent("PostgresTransferInChunks", "Chunks defined:", this.TransferChunks.Count.ToString() + " chunks");
                }
                // transfer the chunks
                foreach (System.Collections.Generic.KeyValuePair<int, TransferChunk> KV in this.TransferChunks)
                {
                    if (this.I_Transfer != null)
                    {
                        this.I_Transfer.SetInfo("Transfer chunk: " + KV.Key.ToString() + " of " + this.TransferChunks.Count.ToString() + " for table " + KV.Value.TableName());
                        this.I_Transfer.SetTransferProgress((int)(100 * KV.Key / this.TransferChunks.Count));
                    }
                    if (!KV.Value.TransferData())
                    {
                        OK = false;
                        if (this.I_Transfer != null)
                            this.I_Transfer.SetInfo("Failed chunk: " + KV.Key.ToString() + " of " + this.TransferChunks.Count.ToString());
                    }
                    else if (this.LoggingIsOn)
                        CacheDB.LogEvent("PostgresTransferInChunks", "Chunk " + KV.Key.ToString() + " for table " + KV.Value.TableName(), "Range of transferred IDs: " + KV.Value.StartID().ToString() + " - " + KV.Value.EndID().ToString());
                }

                if (OK)
                {
                    OK = this.PostgresTransferExchangeTempTable(ref Error);
                }

                return OK;
            }
            catch (Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private bool PostgresRemoveOldChunkTables(ref string Message)
        {
            bool OK = true;
            string SQL = "select concat('\"', table_schema,'\"','.','\"',table_name,'\"') from information_schema.Tables " +
                "where table_type = 'BASE TABLE' " +
                "and table_name like 'Cache%\\_Chunk\\_%'";
            System.Data.DataTable dtRemove = new System.Data.DataTable();
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRemove, ref Message);
            if (OK)
            {
                foreach (System.Data.DataRow R in dtRemove.Rows)
                {
                    SQL = "DROP TABLE " + R[0].ToString() + ";";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQueryWithSameConnection(SQL, ref Message);
                    if (!OK)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("PostgresRemoveOldChunks()", R[0].ToString(), Message + " - SQL: " + SQL, true, true);
                        break;
                    }
                }
            }
             return OK;
        }

        private bool PostgresRemoveOldTempTables(ref string Message)
        {
            bool OK = true;
            string SQL = "select concat('\"', table_schema,'\"','.','\"',table_name,'\"') from information_schema.Tables " +
                "where table_type = 'BASE TABLE' " +
                "and table_name like 'Cache%\\_Temp'";
            System.Data.DataTable dtRemove = new System.Data.DataTable();
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRemove, ref Message);
            if (OK)
            {
                foreach (System.Data.DataRow R in dtRemove.Rows)
                {
                    SQL = "DROP TABLE " + R[0].ToString() + ";";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQueryWithSameConnection(SQL, ref Message);
                    if (!OK)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("PostgresRemoveOldTempTables()", R[0].ToString(), Message + " - SQL: " + SQL, true, true);
                        break;
                    }
                }
            }
            return OK;
        }

        private bool PostgresTransferExchangeTempTable(ref string ErrorMessage)
        {
            bool OK = true;
            try
            {
                if (this.LoggingIsOn)
                    CacheDB.LogEvent("PostgresTransferExchangeTempTable", "Exchanging table:", this.TargetTemp + " -> " + this._Target);
                bool HasDependedObjects = false;
                string Message = "";
                string LoggingMessage = "Data transferred: ";
                string SQL = "SELECT dependent_ns.nspname as dependent_schema " +
                    ", dependent_view.relname as dependent_view  " +
                    ", source_ns.nspname as source_schema " +
                    ", source_table.relname as source_table " +
                    ", pg_attribute.attname as column_name " +
                    "FROM pg_depend  " +
                    "JOIN pg_rewrite ON pg_depend.objid = pg_rewrite.oid  " +
                    "JOIN pg_class as dependent_view ON pg_rewrite.ev_class = dependent_view.oid  " +
                    "JOIN pg_class as source_table ON pg_depend.refobjid = source_table.oid  " +
                    "JOIN pg_attribute ON pg_depend.refobjid = pg_attribute.attrelid  " +
                    "AND pg_depend.refobjsubid = pg_attribute.attnum " +
                    "JOIN pg_namespace dependent_ns ON dependent_ns.oid = dependent_view.relnamespace " +
                    "JOIN pg_namespace source_ns ON source_ns.oid = source_table.relnamespace " +
                    "WHERE  " +
                    "source_ns.nspname = '" + this.SchemaPostgres + "' " +
                    "AND source_table.relname = '" + this._Target + "' " +
                    "AND pg_attribute.attnum > 0 ;";
                System.Data.DataTable DtTestDependencies = new System.Data.DataTable();
                if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref DtTestDependencies, ref Message))
                {
                    if (DtTestDependencies.Rows.Count > 0)
                        HasDependedObjects = true;
                    if (this.LoggingIsOn && HasDependedObjects)
                        CacheDB.LogEvent("PostgresTransferExchangeTempTable", "HasDependedObjects: ", DtTestDependencies.Rows.Count.ToString() + " objects");
                }
                string Error = "";
                SQL = "BEGIN; ";
                if ((this.SourceView != null && this.SourceView.Length > 0) || HasDependedObjects)
                {
                    string ExchangeError = "";
                    OK = this.PostgresExchangeRemoveMissingData(ref ExchangeError);
                    if (OK)
                        OK = this.PostgresExchangeInChunks(ref ExchangeError);

                    if (OK)
                        LoggingMessage += "Data exchanged";
                    else
                        LoggingMessage += "Data exchange failed: " + ExchangeError;
                }
                else
                {
                    SQL = "BEGIN; ";
                    SQL += "SET ROLE \"CacheAdmin\"; "; // _" + this.SchemaPostgres + "\"; ";
                    SQL += "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" RENAME TO \"" + this._Target + "_OLD\";" +
                        "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" RENAME TO \"" + this._Target + "\";" +
                        "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" SET LOGGED;" +
                        "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" OWNER TO \"CacheAdmin\"; " + //_" + this.SchemaPostgres + "\";" +
                        "GRANT ALL ON TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" TO \"CacheAdmin\"; " + //_" + this.SchemaPostgres + "\";" +
                        "GRANT SELECT ON TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" TO \"CacheUser\"; " +
                        //"GRANT SELECT ON TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" TO \"CacheUser_" + this.SchemaPostgres + "\";" +
                        "DROP TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "_OLD\"; ";
                    SQL += "COMMIT;";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                    if (!OK && Error.IndexOf(" owner ") > -1)
                    {
                        SQL = "BEGIN; ";
                        SQL += "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" RENAME TO \"" + this._Target + "_OLD\";" +
                            "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" RENAME TO \"" + this._Target + "\";" +
                            "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" SET LOGGED;" +
                            "ALTER TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" OWNER TO \"CacheAdmin\"; " + //_" + this.SchemaPostgres + "\";" +
                            "GRANT ALL ON TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" TO \"CacheAdmin\"; " + //_" + this.SchemaPostgres + "\";" +
                            "GRANT SELECT ON TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" TO \"CacheUser\"; " +
                            //"GRANT SELECT ON TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" TO \"CacheUser_" + this.SchemaPostgres + "\";" +
                            "DROP TABLE \"" + this.SchemaPostgres + "\".\"" + this._Target + "_OLD\"; ";
                        SQL += "COMMIT;";
                        Error = "";
                        OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                    }
                    if (OK)
                        LoggingMessage += "Tables exchanged";
                    else
                        LoggingMessage += "Failed to exchange temp table";
                    if (OK && LoggingIsOn)
                        CacheDB.LogEvent("PostgresTransferExchangeTempTable", this._Target, LoggingMessage);
                    else if (LoggingIsOn)
                        CacheDB.LogEvent("PostgresTransferExchangeTempTable", "Transfer for " + this._Target + " failed", Error);
                    if (Error.Length > 0)
                        this._Report += "\r\n" + Error;
                }
                if (this._Target == "CacheMetadata")
                {
                    SQL = "BEGIN; " +
                        "UPDATE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" " +
                        "SET \"LogLastTransfer\" = now(); " +
                        "COMMIT;";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                    if (this.LoggingIsOn)
                    {
                        if (Error.Length == 0)
                            CacheDB.LogEvent("PostgresTransferExchangeTempTable", "LogLastTransfer updated", SQL);
                        else
                            CacheDB.LogEvent("PostgresTransferExchangeTempTable", "Update failed", SQL + "\r\nError: " + Error);
                    }
                }
                if (this.I_Transfer != null)
                {
                    if (Error.Length == 0)
                        this.I_Transfer.SetTransferState(TransferState.Successfull);
                    else
                        this.I_Transfer.SetTransferState(TransferState.Error);
                }
                if (Error.Length > 0)
                {
                    if (ErrorMessage.Length > 0)
                        ErrorMessage += "\r\n";
                    ErrorMessage += Error;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                if (ErrorMessage.Length > 0)
                    ErrorMessage += "\r\n";
                ErrorMessage += ex.Message;
                OK = false;
            }
            return OK;
        }

        /// <summary>
        /// Transfer of the data from SQL-Server table into a Postgres table
        /// </summary>
        /// <param name="iFrom">the start value of the ID column</param>
        /// <param name="iTo">the end value of the ID column</param>
        /// <param name="TableName">The name of the </param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public bool PostgresTransferData(int iFrom, int iTo, string TableName, ref string Error, bool RemoveExistingData)
        {
            bool OK = true;

            // Creation of the chunk table in the database
            if (!this.PostgresCreateTransferTable(TableName))
                return false;

            // clearing the source table
            this._dtSource.Rows.Clear();
            // prepare the target table
            if (!this.PostgresPrepareTargetChunkTable(TableName))
                return false;

            // filling the target table
            string SQL =  this.SqlTransfer();
            this._adSource.SelectCommand.CommandText = SQL;
            this._adSource.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
            if (SQL.IndexOf(" WHERE ") > -1)
                this._adSource.SelectCommand.CommandText += " AND ";
            else this._adSource.SelectCommand.CommandText += " WHERE ";
            this._adSource.SelectCommand.CommandText += this.CountColumn() + " BETWEEN " + iFrom + " AND " + iTo.ToString();
            try
            {
                this._adSource.Fill(this._dtSource);
                // transfer the data
                foreach (System.Data.DataRow R in this._dtSource.Rows)
                {
                    System.Data.DataRow Rnew = this._dtTargetChunkPG.NewRow();
                    foreach (string s in this._TransferDataToPostgresColumns)
                        Rnew[s] = R[s];
                    this._dtTargetChunkPG.Rows.Add(Rnew);
                }
                this._adTargetChunkPG.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres;
                if (this._adTargetChunkPG.SelectCommand.Connection.State == System.Data.ConnectionState.Closed)
                    this._adTargetChunkPG.SelectCommand.Connection.Open();
                try
                {
                    this._adTargetChunkPG.Update(this._dtTargetChunkPG);
                }
                catch (System.Exception ex)
                {
                    OK = false;
                    Error = ex.Message + "; " + this._adSource.SelectCommand.CommandText + "\r\n\r\n";
                }
                this._adTargetPG.SelectCommand.Connection.Close();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                OK = false;
                Error = ex.Message + this._adSource.SelectCommand.CommandText + "\r\n\r\n";
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error = ex.Message + this._adSource.SelectCommand.CommandText + "\r\n\r\n";
            }
            finally
            {
            }

            if (OK)
            {
                OK = PostgresTransferDataFromChunkToTemp(TableName, RemoveExistingData);
            }

            return OK;
        }

        public bool PostgresTransferFailedData(int iFrom, int iTo, string TableName, ref string Error)
        {
            bool OK = true;

            // Creation of the chunk table in the database
            if (!this.PostgresCreateTransferTable(TableName))
                return false;

            // clearing the source table
            this._dtSource.Rows.Clear();
            // prepare the target table
            if (!this.PostgresPrepareTargetChunkTable(TableName))
                return false;

            // filling the target table
            string SQL = this.SqlTransfer();
            this._adSource.SelectCommand.CommandText = SQL;
            this._adSource.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
            if (SQL.IndexOf(" WHERE ") > -1)
                this._adSource.SelectCommand.CommandText += " AND ";
            else this._adSource.SelectCommand.CommandText += " WHERE ";
            this._adSource.SelectCommand.CommandText += this.CountColumn() + " BETWEEN " + iFrom + " AND " + iTo.ToString();
            try
            {
                this._adSource.Fill(this._dtSource);
                // transfer the data
                foreach (System.Data.DataRow R in this._dtSource.Rows)
                {
                    System.Data.DataRow Rnew = this._dtTargetChunkPG.NewRow();
                    foreach (string s in this._TransferDataToPostgresColumns)
                        Rnew[s] = R[s];
                    this._dtTargetChunkPG.Rows.Add(Rnew);
                }
                this._adTargetChunkPG.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres;
                if (this._adTargetChunkPG.SelectCommand.Connection.State == System.Data.ConnectionState.Closed)
                    this._adTargetChunkPG.SelectCommand.Connection.Open();
                try
                {
                    this._adTargetChunkPG.Update(this._dtTargetChunkPG);
                }
                catch (System.Exception ex)
                {
                }
                this._adTargetPG.SelectCommand.Connection.Close();
                //double Percent = ((double)i + 1) / (double)this.Chunks.Count;
                //if (this.I_Transfer != null)
                //    this.I_Transfer.SetTransferProgress((int)(Percent * 100));
            }
            catch (Npgsql.NpgsqlException ex)
            {
                OK = false;
                Error = ex.Message + this._adSource.SelectCommand.CommandText + "\r\n\r\n";
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error = ex.Message + this._adSource.SelectCommand.CommandText + "\r\n\r\n";
            }
            finally
            {
            }

            if (OK)
            {
                OK = PostgresTransferDataFromChunkToTemp(TableName);
            }

            return OK;
        }

        public bool PostgresCreateTransferTable(string ChunkTableName)
        {
            bool OK = true;
            string Error = "";
            try
            {
                // Test for existence included in SQL statement
                string SQL = "CREATE TABLE IF NOT EXISTS \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\" " +
                    "(LIKE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" INCLUDING DEFAULTS INCLUDING INDEXES);  ";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

                // Adding grants
                SQL = "GRANT ALL ON TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\" TO \"CacheAdmin\"; "; //_" + this.SchemaPostgres + "\";";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

                // Test if the table exists - otherwise create it
                //string SQL = "select count(*) " +
                //    "from information_schema.tables T " +
                //    "where T.table_schema = '" + this.SchemaPostgres + "' and T.table_name = '" + ChunkTableName + "'";
                //string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                //if (Result == "0")
                //{
                //    SQL = "CREATE TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\" " +
                //        "(LIKE \"" + this.SchemaPostgres + "\".\"" + this.Target + "\" INCLUDING INDEXES);  ";
                //    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                //}

                // clear the table
                SQL = "DELETE FROM \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\"";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message;
            }
            if (Error.Length > 0)
                this._Report += "\r\n" + Error;
            //if (this.I_Transfer != null)
            //    this.I_Transfer.SetInfo(this.TargetTemp + " created");
            return OK;
        }

        public bool PostgresDropTransferTable(string ChunkTableName)
        {
            bool OK = true;
            string Error = "";
            try
            {
                // Test if the table exists and remove it
                string SQL = "select count(*) " +
                    "from information_schema.tables T " +
                    "where T.table_schema = '" + this.SchemaPostgres + "' and T.table_name = '" + ChunkTableName + "'";
                string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                if (Result == "1")
                {
                    SQL = "DROP TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTableName + "\"";
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message;
            }
            if (Error.Length > 0)
                this._Report += "\r\n" + Error;
            if (this.I_Transfer != null)
                this.I_Transfer.SetInfo(this.TargetTemp + " removed");
            return OK;
        }

        private bool PostgresTransferDataFromChunkToTemp(string ChunkTable, bool RemoveExistingData)
        {
            bool OK = false;
            string Message = "";
            string SQL = "";
            if (RemoveExistingData)
            {
                SQL = "DELETE T FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" AS T, " +
                    "\"" + this.SchemaPostgres + "\".\"" + ChunkTable + "\" AS C " +
                    "WHERE T.\"" + this.CountColumn() + "\" = C.\"" + "\"" + this.CountColumn() + "\"";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
                if (!OK)
                {
                    CacheDB.LogEvent("PostgresTransferDataFromChunkToTemp", this.TargetTemp, "Remove existing data failed: " + SQL);
                    return OK;
                }
            }
            SQL = "INSERT INTO  \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + ChunkTable + "\"";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
            {
                SQL = "DROP TABLE \"" + this.SchemaPostgres + "\".\"" + ChunkTable + "\"";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            }
            else
                CacheDB.LogEvent("PostgresTransferDataFromChunkToTemp", this.TargetTemp, "TransferStep failed: " + SQL);
            return OK;
        }

        private bool PostgresExchangeData(ref string Error)
        {
            bool OK = false;
            CacheDB.LogEvent("PostgresExchangeData(ref string Error)", "Getting PrimaryKey", "Target: " + this._Target + "; Schema: " + this.SchemaPostgres);
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Column> PK = DiversityWorkbench.PostgreSQL.Table.PrimaryKey(this._Target, this.SchemaPostgres);
            CacheDB.LogEvent("PostgresExchangeData(ref string Error)", "Getting PrimaryKey", "Target: " + this._Target + "; Schema: " + this.SchemaPostgres);
            string SQL = "BEGIN; ";
            SQL += "DELETE FROM \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" AS T ";
            if (this.SourceView != null && this.SourceView.Length > 0)
                SQL += " WHERE T.\"SourceView\" = '" + this.SourceView + "'";
            SQL += "; ";
            SQL += "INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" " +
                "SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" TT";
            if (this.SourceView != null && this.SourceView.Length > 0)
                SQL += " WHERE TT.\"SourceView\" = '" + this.SourceView + "'";
            else
                SQL += " WHERE 1 = 1 ";
            if (PK.Count > 0)
            {
                SQL += " AND NOT EXISTS (SELECT * FROM  \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" T WHERE ";
                string pk = "";
                foreach(string P in PK.Keys)
                {
                    if (pk.Length > 0) pk += " AND ";
                    pk += " T.\"" + P + "\" = TT.\"" + P + "\" ";
                }
                SQL += pk + ")";
            }
            SQL += "; COMMIT;";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

            return OK;
        }

        private bool PostgresExchangeInChunks(/*int TotalCount, string SQL, */ref string Error)
        {
            bool OK = true;

            try
            {
                if (this.LoggingIsOn)
                {
                    CacheDB.LogEvent("PostgresExchangeInChunks", "Chunks defined:", this.TransferChunks.Count.ToString() + " chunks");
                }
                if (this.TransferChunks.Count > 0)
                {
                    // transfer the chunks
                    foreach (System.Collections.Generic.KeyValuePair<int, TransferChunk> KV in this.TransferChunks)
                    {
                        if (this.I_Transfer != null)
                        {
                            this.I_Transfer.SetInfo("Exchange chunk: " + KV.Key.ToString() + " of " + this.TransferChunks.Count.ToString());
                            this.I_Transfer.SetTransferProgress((int)(100 * KV.Key / this.TransferChunks.Count));
                        }
                        if (!KV.Value.ExchangeData(ref Error))
                        {
                            OK = false;
                            if (this.I_Transfer != null)
                                this.I_Transfer.SetInfo("Failed chunk: " + KV.Key.ToString() + " of " + this.TransferChunks.Count.ToString());
                        }
                        else if (this.LoggingIsOn)
                            CacheDB.LogEvent("PostgresExchangeInChunks", "Chunk " + KV.Key.ToString(), "Range of transferred IDs: " + KV.Value.StartID().ToString() + " - " + KV.Value.EndID().ToString());
                    }
                }
                else
                {
                    CacheDB.LogEvent("PostgresExchangeInChunks", "Starting PostgresExchangeData(ref Error)", "");
                    OK = this.PostgresExchangeData(ref Error);
                    CacheDB.LogEvent("PostgresExchangeInChunks", "PostgresExchangeData(ref Error) finished", "Error: " + Error);
                }

                if (OK)
                {
                    OK = this.PostgresDropTransferTable(this.TargetTemp);
                }

                return OK;
            }
            catch (Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public bool PostgresExchangeData(int iFrom, int iTo, string TableName, ref string Error)
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Column> PK = DiversityWorkbench.PostgreSQL.Table.PrimaryKey(this._Target, this.SchemaPostgres);
            bool OK = false;
            string SQL = "BEGIN; ";
            SQL += "DELETE FROM \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" AS T " +
                "WHERE T.\"" + this.CountColumn() + "\" BETWEEN " + iFrom.ToString() + " AND " + iTo.ToString();
            if (this.SourceView != null && this.SourceView.Length > 0)
                SQL += " AND T.\"SourceView\" = '" + this.SourceView + "'";
            SQL += "; INSERT INTO \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" " +
                "SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" TT WHERE TT.\"" + this.CountColumn() + "\" BETWEEN " + iFrom.ToString() + " AND " + iTo.ToString() + " ";
            if (this.SourceView != null && this.SourceView.Length > 0)
                SQL += " AND TT.\"SourceView\" = '" + this.SourceView + "'";
            if (PK.Count > 0)
            {
                SQL += "AND NOT EXISTS (SELECT * FROM \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" AS E WHERE ";
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Column> KV in PK)
                {
                    if (i > 0)
                        SQL += " AND ";
                    SQL += "TT.\"" + KV.Key + "\" = E.\"" + KV.Key + "\"";
                    i++;
                }
                SQL += ")";
            }
            SQL += "; COMMIT;";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);

            return OK;
        }

        /// <summary>
        /// Removing those data from target table that do not exist any longer in the new data
        /// </summary>
        /// <param name="Error">a possible error</param>
        /// <returns></returns>
        private bool PostgresExchangeRemoveMissingData(ref string Error)
        {
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Column> PK = DiversityWorkbench.PostgreSQL.Table.PrimaryKey(this._Target, this.Schema);
            bool OK = false;
            try
            {
                string Owner = DiversityWorkbench.PostgreSQL.Connection.ObjectOwner(this._Target, this.SchemaPostgres, DiversityWorkbench.PostgreSQL.Connection.ObjectType.Table);
                string SQL = "BEGIN; ";
                string CountColumn = this.CountColumn();
                if (Owner.Length > 0)
                    SQL += "SET ROLE '" + Owner + "'; ";
                SQL += "DELETE FROM  \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" " +
                    "WHERE \"" + this.CountColumn() + "\" IN (SELECT T.\"" + this.CountColumn() + "\" " +
                    "FROM \"" + this.SchemaPostgres + "\".\"" + this._Target + "\" AS T " +
                    " LEFT JOIN \"" + this.SchemaPostgres + "\".\"" + this.TargetTemp + "\" AS TT " +
                    " ON T.\"" + this.CountColumn() + "\" = TT.\"" + this.CountColumn() + "\" ";
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Column> KV in PK)
                    SQL += " AND T.\"" + KV.Key + "\" = TT.\"" + KV.Key + "\" ";
                SQL += " WHERE TT.\"" + this.CountColumn() + "\" IS NULL ";
                if (this.SourceView != null && this.SourceView.Length > 0)
                    SQL += " AND T.\"SourceView\" = '" + this.SourceView + "' AND TT.\"SourceView\" = '" + this.SourceView + "' ";
                SQL += "); COMMIT;";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                if (!OK)
                { }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK;
        }

        #endregion
        
        #region Package

        private bool TransferDataToPackage(ref int Count)
        {
            bool OK = true;
            try
            {
                CacheDB.LogEvent("TransferStep", "TransferDataToPackage(ref int Count)", "TransferProcedure: " + this._TransferProcedure);
                string Message = "";
                if (this.I_Transfer != null)
                    this.I_Transfer.SetTransferState(TransferState.Transfer);
                string SQL = "SELECT \"" + this.Schema + "\"." + this._TransferProcedure;
                if (!SQL.EndsWith("()"))
                    SQL += "()";
                if (this.I_Transfer != null)
                    this.I_Transfer.SetTransferState(TransferState.Transfer);
                if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                {
                    if (this.I_Transfer != null)
                        this.I_Transfer.SetTransferState(TransferState.Successfull);

                    SQL = "SELECT COUNT(*) FROM \"" + this.Schema + "\".\"" + this.Target + "\"";
                    string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                    int.TryParse(Result, out Count);
                    this._Report += Result;
                }
                else
                {
                    this._Errors += Message;
                    if (this.I_Transfer != null)
                    {
                        this.I_Transfer.SetTransferState(TransferState.Failed);
                        OK = false;
                    }
                }
                if (Message.Length > 0)
                {
                    if (this.I_Transfer != null)
                        this.I_Transfer.SetMessage(Message);
                    this._Report += Message;
                }
                CacheDB.LogEvent("", "", this._TransferProcedure + " finished. " + Message);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                this._Report += ex.Message;
            }
            return OK;
        }
        
        #endregion

        #endregion

    }
}
