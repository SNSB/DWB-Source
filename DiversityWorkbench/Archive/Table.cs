using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Archive
{
    public class Table
    {

        #region Properties and parameter

        private iTableGUI _iTableGUI;
        public void SetGUI(iTableGUI GUI) { this._iTableGUI = GUI; }
        
        private string _TableName;
        public string TableName() { return this._TableName; }
        private string LogTableName { get { return _TableName + "_log"; } }
        private Archive.DataArchive _DataArchive;
        //private string _ParentTable;
        private System.Collections.Generic.Dictionary<string, Archive.Column> _Columns;
        private System.Collections.Generic.Dictionary<string, Archive.Column> _LogColumns;

        public System.Collections.Generic.Dictionary<string, Archive.Column> Columns
        {
            get
            {
                if (this._Columns == null)
                {
                    this._Columns = new Dictionary<string, Column>();
                    string SQL = "SELECT C.COLUMN_NAME, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH, CASE WHEN C.IS_NULLABLE = 'YES' THEN 'true' ELSE 'false' END AS IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this._TableName + "' AND C.COLUMN_NAME NOT LIKE 'xx_%'";
                    SQL += " ORDER BY C.ORDINAL_POSITION";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Archive.Column TC = new Column(R[0].ToString(), this);
                        TC.DataType = R[1].ToString();
                        if (!R[2].Equals(System.DBNull.Value))
                            TC.DataTypeLength = int.Parse(R[2].ToString());
                        bool IsNullable = true;
                        bool.TryParse(R[3].ToString(), out IsNullable);
                        TC.IsNullable = IsNullable;
                        this._Columns.Add(R[0].ToString(), TC);
                    }
                }
                return _Columns;
            }
            //set { _Columns = value; }
        }

        public System.Collections.Generic.Dictionary<string, Archive.Column> LogColumns
        {
            get
            {
                if (this._LogColumns == null)
                {
                    this._LogColumns = new Dictionary<string, Column>();
                    string SQL = "SELECT C.COLUMN_NAME, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH, CASE WHEN C.IS_NULLABLE = 'YES' THEN 'true' ELSE 'false' END AS IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this._TableName + "_log' AND C.COLUMN_NAME NOT LIKE 'xx_%'";
                    SQL += " ORDER BY C.ORDINAL_POSITION";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Archive.Column TC = new Column(R[0].ToString(), this);
                        TC.DataType = R[1].ToString();
                        if (!R[2].Equals(System.DBNull.Value))
                            TC.DataTypeLength = int.Parse(R[2].ToString());
                        bool IsNullable = true;
                        bool.TryParse(R[3].ToString(), out IsNullable);
                        TC.IsNullable = IsNullable;
                        this._LogColumns.Add(R[0].ToString(), TC);
                    }
                }
                return _LogColumns;
            }
            //set { _Columns = value; }
        }

        private bool _IsReferencingTable = false;
        private System.Collections.Generic.Dictionary<string, string> _ReferencedTables;
        private string _ColumnForReferencedTable = "ReferencedTable";
        private string _ColumnForReferencedColumn = "ReferencedID";

        private bool? _HasInternalRelations;

        private Microsoft.Data.SqlClient.SqlDataAdapter _Adapter;

        public bool HasInternalRelations
        {
            get
            {
                if (this._HasInternalRelations == null)
                {
                    this.FindChildParentColumns();
                    foreach (System.Collections.Generic.KeyValuePair<string, Column> C in this.Columns)
                    {
                        if (C.Value.ParentColumn != null && C.Value.ParentColumn.Length > 0)
                        {
                            this._HasInternalRelations = true;
                            break;
                        }
                    }
                    if (this._HasInternalRelations == null)
                        this._HasInternalRelations = false;
                }
                return (bool)_HasInternalRelations;
            }
        }

        private System.Collections.Generic.List<string> _internalRelationColums;
        public System.Collections.Generic.List<string> InternalRelationColums
        {
            get
            {
                if (_internalRelationColums == null)
                {
                    _internalRelationColums = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, Column> C in this.Columns)
                    {
                        if (C.Value.ParentColumn != null && C.Value.ParentColumn.Length > 0 && this.Columns.ContainsKey(C.Value.ParentColumn))
                        {
                            _internalRelationColums.Add(C.Key);
                        }
                    }
                }
                return _internalRelationColums;
            }
        }

        private System.Data.DataTable _DT;
        private System.Data.DataSet _DS;

        private System.Data.DataTable _DT_log;
        private bool _IncludeLog = false;
        public bool IncludeLog { get { return _IncludeLog; } set { _IncludeLog = value; } }

        private bool? _HasLog = null;
        public bool HasLog
        {
            get
            {
                if (_HasLog == null)
                {
                    _HasLog = (Exists(this._TableName + "_log"));
                }
                return (bool)_HasLog;
            }
        }
        //private Table _LogTable;
        //private bool _IsLogTable;

        private System.IO.FileInfo _File;
        private System.IO.FileInfo _LogFile;

        private System.Collections.Generic.List<string> _ColumnsExcludedFromRestriction;

        public void AddColumnExcludedFromRestriction(string Column)
        {
            if (this._ColumnsExcludedFromRestriction == null)
                this._ColumnsExcludedFromRestriction = new List<string>();
            if (!this._ColumnsExcludedFromRestriction.Contains(Column))
                this._ColumnsExcludedFromRestriction.Add(Column);
        }
        public System.Collections.Generic.List<string> ColumnsExcludedFromRestriction
        {
            get
            {
                if (this._ColumnsExcludedFromRestriction == null)
                    this._ColumnsExcludedFromRestriction = new List<string>();
                return this._ColumnsExcludedFromRestriction;
            }
        }

        public System.Data.DataTable DataTable()
        {
            if (this._DataArchive == null && this._File == null) // neither Create nor Restore means Reset
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                try
                {
                    string SQL = "SELECT * FROM [" + this._TableName + "]";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                }
                catch (System.Exception ex)
                {
                }
                return dt;
            }
            return _DT; 
        }

        
        #endregion

        #region Construction

        public Table(string TableName, Archive.DataArchive DataArchive, bool IncludeLog = false) //, bool IsLogTable = false)//, string ParentTable)
        {
            this._TableName = TableName;
            this._DataArchive = DataArchive;
            this._IncludeLog = IncludeLog;
            this.FindColumnsWithForeignRelations();
            this.FindChildParentColumns();
            //if (IncludeLog)
            //{
            //    if (Exists(TableName + "_log"))
            //    {
            //        this._LogTable = new Table(TableName + "_log", DataArchive, true, true);
            //    }
            //}
            //this._IsLogTable = IsLogTable;
            //if (!IsLogTable)
            //{
            //}
        }

        //public Table(string TableName, Archive.DataArchive DataArchive, bool ForLog)//, string ParentTable)
        //{
        //    this._TableName = TableName;
        //    if (Exists())
        //    {
        //        this._DataArchive = DataArchive;
        //        this.FindColumnsWithForeignRelations();
        //        this.FindChildParentColumns();
        //    }
        //}

        public Table(string TableName, System.IO.FileInfo File)//, string ParentTable)
        {
            this._TableName = TableName;
            this._File = File;
            if(IncludeLog)
            {

            }
            //this._DT.ReadXml(this._File.FullName);
        }

        public Table(string TableName)
        {
            this._TableName = TableName;
        }

        public Table(string TableName, string ColumnForReferencedTable, string ColumnForReferencedColumn, Archive.DataArchive DataArchive)
        {
            this._TableName = TableName;
            this._ColumnForReferencedTable = ColumnForReferencedTable;
            this._ColumnForReferencedColumn = ColumnForReferencedColumn;
            this._IsReferencingTable = true;
            this.FindReferencedTables();
            this.FindChildParentColumns();
        }

        #endregion

        #region Datahandling

        #region Reset

        public string RemoveForeignKeys()
        {
            string Message = "";
            if (this.Count() == "0")
                return "";
            // Clear any column that relates to another column
            try
            {
                System.Collections.Generic.List<string> LinkColumns = new List<string>();
                this.FindColumnsWithForeignRelations();
                this.FindChildParentColumns();
                foreach (System.Collections.Generic.KeyValuePair<string, Archive.Column> KV in this.Columns)
                {
                    if (KV.Value.ForeignRelationColumn != null &&
                        KV.Value.ForeignRelationColumn.Length > 0 &&
                        KV.Value.ForeignRelationTable != null &&
                        KV.Value.ForeignRelationTable.Length > 0 &&
                        !PrimaryKeyColumnList.Contains(KV.Key))
                    {
                        LinkColumns.Add(KV.Key);
                    }
                }
                string SQL = "";
                foreach (string C in LinkColumns)
                {
                    if (this.Columns[C].IsNullable) //.DataTable().Columns[C].AllowDBNull)
                    {
                        if (SQL.Length > 0) SQL += ", ";
                        SQL += C + " = NULL";
                    }
                    else { }
                }
                if (SQL.Length > 0)
                {
                    SQL = "UPDATE [" + this._TableName + "] SET " + SQL;
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
            }
            catch (System.Exception ex)
            {
                Message = "Error removing foreign key from table " + this._TableName + ":\r\n" + ex.Message;
            }
            return Message;
        }

        public string ClearTable()
        {
            string SQL = "";
            string Message = "";
            try
            {
                SQL = "TRUNCATE TABLE [" + this._TableName + "]";
                bool? Ignore = true;
                if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, Ignore))
                {
                    SQL = "DELETE FROM [" + this._TableName + "]";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message);
                }
                if (HasLog)
                {
                    SQL = "TRUNCATE TABLE [" + this.LogTableName + "]";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, Ignore) ;
                }
                if (this._TableName == "ProjectProxy")
                {
                    // ensure existence of default project
                    SQL = "INSERT INTO ProjectProxy (ProjectID, Project) SELECT 0, 'DiversityWorkbench' WHERE NOT EXISTS (SELECT * FROM ProjectProxy P WHERE P.ProjectID = 0)";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message + "\r\n";
            }
            return Message;
        }

        public string Count()
        {
            string SQL = "SELECT COUNT(*) FROM [" + this.TableName() + "]";
            return DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        }

        #endregion

        #region Create

        public string FindData(System.Collections.Generic.List<Archive.Table> ParentTables)
        {
            string Message = "";
            // Check if table exists in the database
            string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.Tables T WHERE T.TABLE_NAME = '" + this._TableName + "'";
            int Result = 0;
            if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString(), out Result))
            {
                return "";
            }

            if (Result == 0)
                return "";

           // Special treatment for table Annotation
            if (ParentTables.Count == 0 && (this._TableName == "Annotation" || this._TableName == "ExternalIdentifier"))
            {
                Message = this.FindReferencedData(this._TableName);
                return Message;
            }
            try
            {
                string Alias = this._DataArchive.TableAlias(this._TableName);

                string SqlFromWhere = " FROM " + this._DataArchive.SqlFromWhereClause(this._TableName);
                SQL = "SELECT DISTINCT " + this.SqlColumns + SqlFromWhere;// FROM [" + this._TableName + "] T1 ";//, [" + this._ParentTable + "] P WHERE 1 = 1 ";
                this._DT = new System.Data.DataTable(this._TableName);

#if DEBUG
                string sql = "select count(*) " + SqlFromWhere;
                string count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql, this._DataArchive.ConnectionTempIDs());
                sql = "select count(*) from #" + this._DataArchive.RootTable;
                //count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql, this._DataArchive.ConnectionTempIDs());
#endif
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._DataArchive.ConnectionTempIDs());// DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DT);
                if(IncludeLog && HasLog)
                {
                    SqlFromWhere = " FROM " + this._DataArchive.SqlFromWhereClause(this._TableName, true);
                    SQL = "SELECT DISTINCT " + this.SqlLogColumns + SqlFromWhere;
                    this._DT_log = new System.Data.DataTable(this.LogTableName);
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(this._DT_log);
                }
                try
                {
                    SQL = "select dbo.Version()";
                    string Version = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    this._DS = new System.Data.DataSet(DiversityWorkbench.Settings.ModuleName + "_" + Version);
                    //if (!this._DS.Tables.Contains(this._DT.TableName))
                        this._DS.Tables.Add(this._DT);
                    DiversityWorkbench.Data.Table T = new Data.Table(this._TableName);
                    foreach (System.Data.DataColumn DC in this._DT.Columns)
                    {
                        if (T.Columns.ContainsKey(DC.ColumnName))
                        {
                            string Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(this._TableName, DC.ColumnName);
                            DC.Caption = Description;
                            if (!T.Columns[DC.ColumnName].IsNullable ||
                                T.Columns[DC.ColumnName].IsIdentity) // Markus 6.12.22: Logik falsch - korrigiert - hier stand ! vor dem Ausdruck
                            {
                                try
                                {
                                    DC.AllowDBNull = false;
                                }
                                catch(System.Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
                catch(System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                if (this.HasInternalRelations)
                {
                    System.Collections.Generic.Dictionary<string, string> InternalRelationColumns = new Dictionary<string, string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, Archive.Column> KV in this.Columns)
                    {
                        if (KV.Value.ParentColumn != null)
                        {
                            InternalRelationColumns.Add(KV.Key, KV.Value.ParentColumn);
                        }
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in InternalRelationColumns)
                    {
                        System.Collections.Generic.List<string> KeysToCheck = new List<string>();
                        System.Data.DataRow[] RR = this._DT.Select("NOT " + KV.Key + " IS NULL", "");
                        if (RR.Length > 0)
                        {
                            foreach (System.Data.DataRow R in RR)
                            {
                                string KeyValue = R[KV.Key].ToString();
                                int i;
                                if (!int.TryParse(KeyValue, out i))
                                    KeyValue = "'" + KeyValue + "'";
                                System.Data.DataRow[] rr = this._DT.Select(KV.Value + " = " + KeyValue);
                                if (rr.Length == 0)
                                {
                                    this.FillTable(this._TableName, " WHERE " + KV.Value + " = " + KeyValue, ad);
                                    System.Data.DataRow[] cc = this._DT.Select(KV.Value + " = " + KeyValue);
                                    if (!cc[0][KV.Key].Equals(System.DBNull.Value))
                                    {
                                        if (!KeysToCheck.Contains(cc[0][KV.Key].ToString()))
                                            KeysToCheck.Add(cc[0][KV.Key].ToString());
                                    }
                                }
                            }
                            while (KeysToCheck.Count > 0)
                            {
                                System.Collections.Generic.List<string> KeysChecked = new List<string>();
                                System.Collections.Generic.List<string> KeysToAdd = new List<string>();
                                foreach (string Key in KeysToCheck)
                                {
                                    string KeyValue = Key;
                                    int i;
                                    if (!int.TryParse(KeyValue, out i))
                                        KeyValue = "'" + KeyValue + "'";

                                    System.Data.DataRow[] rr = this._DT.Select(KV.Value + " = " + KeyValue);
                                    if (rr.Length == 0)
                                    {
                                        this.FillTable(this._TableName, " WHERE " + KV.Value + " = " + KeyValue, ad);
                                        System.Data.DataRow[] cc = this._DT.Select(KV.Value + " = " + KeyValue);
                                        if (!cc[0][KV.Key].Equals(System.DBNull.Value))
                                        {
                                            if (!KeysToCheck.Contains(cc[0][KV.Key].ToString()))
                                                KeysToAdd.Add(cc[0][KV.Key].ToString());
                                        }
                                    }
                                    else KeysChecked.Add(Key);
                                }
                                foreach (string K in KeysChecked)
                                    KeysToCheck.Remove(K);
                                foreach (string K in KeysToAdd)
                                    KeysToCheck.Add(K);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message + "\r\nin: " + SQL + "\r\n";
                this._DataArchive.ErrorMessageAdd(Message);
            }
            return Message;
        }

        private string _Alias = "";
        private string Alias
        {
            get
            {
                if (_Alias.Length == 0)
                _Alias = this._DataArchive.TableAlias(this._TableName);
                return _Alias;
            }
        }

        private string _SqlColumns = "";
        private string SqlColumns
        {
            get
            {
                if (_SqlColumns.Length == 0)
                    _SqlColumns = this.SQL_Columns(this.Columns, Alias);
                return _SqlColumns;
            }
        }

        private string _SqlLogColumns = "";
        private string SqlLogColumns
        {
            get
            {
                if (_SqlLogColumns.Length == 0)
                    _SqlLogColumns = this.SQL_Columns(this.LogColumns, Alias + "_log");
                return _SqlLogColumns;
            }
        }

        private string SQL_Columns(System.Collections.Generic.Dictionary<string, Archive.Column> Columns, string Alias)
        {
            string SqlColumns = "";

            foreach (System.Collections.Generic.KeyValuePair<string, Archive.Column> KV in Columns)
            {
                if (SqlColumns.Length > 0)
                    SqlColumns += ", ";
                if (KV.Value.DataType.ToLower() == "xml")
                {
                    SqlColumns += "CAST(" + Alias + "." + KV.Key + " AS nvarchar(MAX)) AS " + KV.Key;
                }
                else if (KV.Value.DataType.ToLower() == "image")
                {
                    SqlColumns += "CAST(" + Alias + "." + KV.Key + " AS varbinary(MAX)) AS " + KV.Key;
                }
                else if (KV.Value.DataType.ToLower() == "geography" || KV.Value.DataType.ToLower() == "geometry")
                    SqlColumns += Alias + "." + KV.Key + ".ToString() AS " + KV.Key;
                else
                    SqlColumns += Alias + "." + KV.Key;

            }
            return SqlColumns;
        }

        private void FillTable(string TableName, string WhereClause, Microsoft.Data.SqlClient.SqlDataAdapter ad)
        {
            try
            {
                string SQL = "SELECT DISTINCT " + this.SqlColumns + " FROM [" + TableName + "] AS " + Alias + WhereClause;
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(this._DT);
                if (IncludeLog && HasLog)
                {
                    SQL = "SELECT DISTINCT " + SqlLogColumns + " FROM [" + TableName + "_log] AS " + Alias + "_log " + WhereClause;
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(this._DT_log);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public bool Exists(string TableName)
        {
            string SQL = "select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = '" + TableName + "'";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString();
            return Result == "1";
        }

        private string FindReferencedData(string ReferencingTable)
        {
            string Message = "";
            string SQL = "SELECT DISTINCT ReferencedTable FROM " + ReferencingTable;
            System.Data.DataTable dtReferencedTables = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._DataArchive.ConnectionTempIDs());// DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtReferencedTables);
            this._DT = new System.Data.DataTable(this._TableName);
            foreach (System.Data.DataRow R in dtReferencedTables.Rows)
            {
                string Alias = this._DataArchive.TableAlias(this._TableName);
                string ReferencedTable = R[0].ToString();
                if (!this._DataArchive.DataTables.ContainsKey(ReferencedTable))
                    continue;
                string AliasReferencedTable = this._DataArchive.TableAlias(ReferencedTable);
                string SqlFrom = "SELECT " + Alias + ".* FROM " + ReferencingTable + " AS " + Alias + ", ";// +", " + ReferencedTable + " AS " + this._DataArchive.TableAlias(ReferencedTable) + ", ";
                string SqlWhere = " AND " + Alias + ".ReferencedID = " + AliasReferencedTable + "." + this._DataArchive.getTable(ReferencedTable).IdentityColumn +
                    " AND " + Alias + ".ReferencedTable = '" + ReferencedTable + "'";
                SQL = this._DataArchive.SqlFromWhereClause(ReferencedTable);
                SQL = SqlFrom + SQL + SqlWhere;

                ad.SelectCommand.CommandText = SQL;
                try
                {
                    ad.Fill(this._DT);
                }
                catch (System.Exception ex)
                {
                    Message = ex.Message + "\r\n";
                    this._DataArchive.ErrorMessageAdd(ex.Message);
                }
            }
            return Message;
        }

        public string ArchiveData(string Folder)
        {
            string Message = "";
            string FileName = Folder + "\\" + this._TableName + ".xml";
            try
            {
                if (this._DT != null && this._DT.Rows.Count > 0)
                {
                    this._DT.WriteXml(FileName, System.Data.XmlWriteMode.WriteSchema);
                    this._DataArchive.ArchiveProtocolAdd(this._TableName + ":\t\t" + this._DT.Rows.Count.ToString() + " Rows");
                }
                if(this.IncludeLog && HasLog)
                {
                    FileName = Folder + "\\" + this.LogTableName + ".xml";
                    if (this._DT_log != null)
                    {
                        this._DT_log.WriteXml(FileName, System.Data.XmlWriteMode.WriteSchema);
                        this._DataArchive.ArchiveProtocolAdd(this.LogTableName + ":\t\t" + this._DT_log.Rows.Count.ToString() + " Rows");
                    }
                    else if (this._DT != null && this._DT.Rows.Count > 0 && this.HasLog)
                    {
                        Message += "\r\nPlease read the log data to store the log for table\r\n\t" + this.LogTableName;
                    }
                }
                //this._DataArchive.ProtocolWrite(this._TableName + ":\t\t" + this._DT.Rows.Count.ToString() + " Rows\r\n");
            }
            catch (System.Exception ex)
            {
                Message = ex.Message + "\r\n";
                this._DataArchive.ErrorMessageAdd(ex.Message);
                this._DataArchive.ArchiveProtocolAdd("Failure for " + this._TableName + ": " + Message);
                //this._DataArchive.ProtocolWrite("Failure for " + this._TableName + ": " + Message + "\r\n");
            }
            return Message;
        }

        #endregion

        #region Schema

        public string WriteSchema(string Folder, string Version)
        {
            string Message = "";
            string SQL = "SELECT * FROM [" + this._TableName + "] WHERE 1 = 0";
            this._DT = new System.Data.DataTable(this._TableName);
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DT);

            try
            {
                this._DS = new System.Data.DataSet(DiversityWorkbench.Settings.ModuleName + "_" + Version);
                this._DS.Tables.Add(this._DT);
                DiversityWorkbench.Data.Table T = new Data.Table(this._TableName);
                System.Data.DataColumn[] PK = new System.Data.DataColumn[T.PrimaryKeyColumnList.Count];
                foreach (System.Data.DataColumn DC in this._DT.Columns)
                {
                    if (T.Columns.ContainsKey(DC.ColumnName))
                    {
                        string Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(this._TableName, DC.ColumnName);
                        DC.Caption = Description;
                        if (!T.Columns[DC.ColumnName].IsNullable ||
                            !T.Columns[DC.ColumnName].IsIdentity)
                        {
                            try
                            {
                                DC.AllowDBNull = false;
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        if (T.PrimaryKeyColumnList.Contains(DC.ColumnName))
                        {
                            int i = 0;
                            PK[i] = DC;
                            i++;
                        }
                    }
                }
                this._DT.PrimaryKey = PK;
                //this._DT.DisplayExpression = T.Description;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            string FileName = Folder + "\\" + this._TableName + ".xml";
            try
            {
                    this._DT.WriteXml(FileName, System.Data.XmlWriteMode.WriteSchema);
            }
            catch (System.Exception ex)
            {
                Message = ex.Message + "\r\n";
            }
            return Message;
        }

        #endregion

        #region Restore

        public void setLogFile(System.IO.FileInfo fileInfo) { this._LogFile = fileInfo; }

        public string ReadData()
        {
            string Message = "";
            try
            {
                if (this._File == null)
                    return "";
                this._DS = new System.Data.DataSet();
                this._DS.ReadXml(this._File.FullName, System.Data.XmlReadMode.ReadSchema);
                this._DT = this._DS.Tables[this._TableName];
            }
            catch (System.Exception ex)
            {
                Message = ex.Message + "\r\n";
            }
            return Message;
        }

        public string ReadData(ref int Count, ref int CountLog)
        {
            string Message = "";
            try
            {
                if (this._File == null && this._LogFile == null)
                    return "";
                if (this._File != null)
                {
                    this._DS = new System.Data.DataSet();
                    this._DS.ReadXml(this._File.FullName, System.Data.XmlReadMode.ReadSchema);
                    this._DT = this._DS.Tables[this._TableName];
                    Count = this._DT.Rows.Count;
                    _XmlColumnList = new List<string>();
                    foreach (System.Data.DataColumn c in this._DT.Columns)
                        _XmlColumnList.Add(c.ColumnName);
                }
                if (IncludeLog && HasLog && this._LogFile != null)
                {
                    if (this._DS == null) this._DS = new System.Data.DataSet();
                    this._DS.ReadXml(_LogFile.FullName, System.Data.XmlReadMode.ReadSchema);
                    this._DT_log = this._DS.Tables[this.LogTableName];
                    CountLog = this._DT_log.Rows.Count;
                    _XmlColumnListLog = new List<string>();
                    foreach (System.Data.DataColumn c in this._DT_log.Columns)
                        _XmlColumnListLog.Add(c.ColumnName);
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message + "\r\n";
            }
            return Message;
        }

        private System.Collections.Generic.List<string> _XmlColumnList;
        private System.Collections.Generic.List<string> _XmlColumnListLog;

        private System.Collections.Generic.List<string> _FailedInserts;
        private System.Collections.Generic.List<string> _Inserts;

        public string RestoreArchive(ref int NumberOfFailedLines, ref int NumberOfPresentLines, ref int LogFailedLines, ref int LogPresentLines)
        {
            string Message = "";
            Message = this.RestoreArchive(this._DT, this.TableName(), this.Columns, this._XmlColumnList, ref NumberOfFailedLines, ref NumberOfPresentLines);
            if (IncludeLog && HasLog)
            {
                Message += "\r\n" + this.RestoreArchive(this._DT_log, this.TableName() + "_log", this.LogColumns, this._XmlColumnListLog, ref LogFailedLines, ref LogPresentLines);
            }
            return Message;
        }

        public string RestoreArchive(System.Data.DataTable DataTable, string TableName,
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Archive.Column> ArchiveColumns,
            System.Collections.Generic.List<string> XmlColumns,
            ref int NumberOfFailedLines, ref int NumberOfPresentLines)
        {
            bool IsLogTable = TableName.EndsWith("_log");
            string Message = "";
            if (DataTable == null)
                return "";
            try
            {
                string SqlColumns = this.GetImportColumns(TableName, ArchiveColumns, XmlColumns, ref Message); // "";
                bool HasIdentity = this.TableHasIdentity(ArchiveColumns); 
                if (HasIdentity)
                {
                    this.SetIdentityInsert(TableName, true, this.SqlConnectionForImport());
                }
                string SqlValues = "";
                string SQL = "";

                this._Inserts = new List<string>();
                this._FailedInserts = new List<string>();
                if (this._iTableGUI != null) this._iTableGUI.setMaxRows(DataTable.Rows.Count);

                foreach (System.Data.DataRow R in DataTable.Rows)
                {
                    SqlValues = this.RestoreArchiveValues(DataTable, ArchiveColumns, XmlColumns, R, ref Message);
                    SQL = "";
                    SQL += "INSERT INTO [" + TableName + "] (" + SqlColumns + ") VALUES (" + SqlValues + ")";
                    this._Inserts.Add(SQL);
                }

                
                this._iTableGUI.setCurrentRow(0);
                foreach (string S in this._Inserts)
                {
                    if (this._iTableGUI != null) this._iTableGUI.setCurrentRow();
                    int ErrorCode = 0;
                    string ErrorMessage = "";
                    if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(S, this.SqlConnectionForImport(), ref ErrorMessage, ref ErrorCode))
                    {
                        if (ErrorCode == 2627)
                        {
                            NumberOfPresentLines++;
                            Message += "\r\n" + ErrorMessage;
                        }
                        else if (!IsLogTable)
                            _FailedInserts.Add(S);
                    }
                }

                if (this._FailedInserts.Count > 0)
                    Message += this.RestoreArchiveTryToInsertFailed(ref NumberOfFailedLines);

                //    if (this._FailedInserts.Count > 0)
                //{
                //    int Start = this._FailedInserts.Count;
                //    int End = 0;
                //    // As long as there are failed commands and the number is getting smaller
                //    while (this._FailedInserts.Count > 0 && Start > End)
                //    {
                //        Start = this._FailedInserts.Count;

                //        // Move Failed commands to Insert commands
                //        this._Inserts.Clear();
                //        foreach (string S in this._FailedInserts)
                //            this._Inserts.Add(S);

                //        this._FailedInserts.Clear();

                //        // Retry Insert commands
                //        string TestMessage = "";
                //        foreach (string S in this._Inserts)
                //        {
                //            if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(S, this.SqlConnectionForImport(), ref TestMessage))
                //                _FailedInserts.Add(S);
                //        }
                //        End = this._FailedInserts.Count();

                //        // Retry from the other end
                //        if (Start == End && Start > 0)
                //        {
                //            this._FailedInserts.Clear();
                //            string Test = "";
                //            for (int ii = this._Inserts.Count - 1; ii >= 0; ii--)
                //            {
                //                if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(this._Inserts[ii], this.SqlConnectionForImport(), ref Test))
                //                    _FailedInserts.Add(this._Inserts[ii]);
                //            }
                //            End = this._FailedInserts.Count();
                //        }
                //    }
                //}

                //// Failed command that were left unsolved
                //if (this._FailedInserts.Count > 0)
                //{
                //    foreach (string S in this._FailedInserts)
                //    {
                //        if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(S, this.SqlConnectionForImport(), ref Message))
                //        {
                //            NumberOfFailedLines++;
                //            Message += "\r\n" + S;
                //        }
                //    }
                //}


                if (IsLogTable && this._iTableGUI != null)
                {
                    int i = this._Inserts.Count();
                    SQL = "SELECT COUNT(*) FROM [" + TableName + "]";
                    int c = 0;
                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this.SqlConnectionForImport()),out c))
                    {
                        this._iTableGUI.setLogCount(i, c);
                    }
                }

                if (HasIdentity)
                {
                    this.SetIdentityInsert(TableName, false, this.SqlConnectionForImport());
                }
                this.SqlConnectionForImport(true);
            }
            catch (System.Exception ex)
            {
                Message = ex.Message + "\r\n";
            }
            return Message;
        }

        private string RestoreArchiveTryToInsertFailed(ref int NumberOfFailedLines)
        {
            string Message = "";
            if (this._FailedInserts.Count > 0)
            {
                int Start = this._FailedInserts.Count;
                int End = 0;
                // As long as there are failed commands and the number is getting smaller
                while (this._FailedInserts.Count > 0 && Start > End)
                {
                    Start = this._FailedInserts.Count;

                    // Move Failed commands to Insert commands
                    this._Inserts.Clear();
                    foreach (string S in this._FailedInserts)
                        this._Inserts.Add(S);

                    this._FailedInserts.Clear();

                    // Retry Insert commands
                    string TestMessage = "";
                    foreach (string S in this._Inserts)
                    {
                        if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(S, this.SqlConnectionForImport(), ref TestMessage))
                            _FailedInserts.Add(S);
                    }
                    End = this._FailedInserts.Count();

                    // Retry from the other end
                    if (Start == End && Start > 0)
                    {
                        this._FailedInserts.Clear();
                        string Test = "";
                        for (int ii = this._Inserts.Count - 1; ii >= 0; ii--)
                        {
                            if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(this._Inserts[ii], this.SqlConnectionForImport(), ref Test))
                                _FailedInserts.Add(this._Inserts[ii]);
                        }
                        End = this._FailedInserts.Count();
                    }
                }
            }

            // Failed command that were left unsolved
            if (this._FailedInserts.Count > 0)
            {
                foreach (string S in this._FailedInserts)
                {
                    if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(S, this.SqlConnectionForImport(), ref Message))
                    {
                        NumberOfFailedLines++;
                        Message += "\r\n" + S;
                    }
                }
            }
            return Message;

        }


        private Microsoft.Data.SqlClient.SqlConnection SqlConnectionForImport(bool Close = false)
        {
            return DataArchive.SqlConnection(Close);
        }

        private void SetIdentityInsert(string TableName, bool Insert, Microsoft.Data.SqlClient.SqlConnection con)
        {
            string SQL = "SET IDENTITY_INSERT [" + TableName + "] "; 
            if (Insert) SQL += " ON; ";
            else SQL += " OFF; ";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, con);
        }

        private bool TableHasIdentity(System.Collections.Generic.Dictionary<string, DiversityWorkbench.Archive.Column> ArchiveColumns)
        {
            bool HasIdentity = false;
            foreach (System.Collections.Generic.KeyValuePair<string, Archive.Column> KV in ArchiveColumns)
            {
                if (KV.Value.IsIdentity)
                {
                    HasIdentity = true;
                    break;
                }
            }
            return HasIdentity;
        }


        private int? _ProjectID = null;
        public int? ProjectID()
        {
            if (this.PrimaryKeyColumnList.Count == 1 && this.PrimaryKeyColumnList[0] == "ProjectID")
            {
                return this._ProjectID;
            }
            else
                return null;
        }

        private string GetImportColumns(string TableName, 
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Archive.Column> ArchiveColumns,
            System.Collections.Generic.List<string> XmlColumns, 
            ref string Message)
        {
            string SqlColumns = "";
            System.Data.DataTable DtColumns = new System.Data.DataTable(TableName);
            string SQL = "SELECT TOP 0 * FROM [" + TableName + "]";
            string ConStr = DiversityWorkbench.Settings.ConnectionString;
            if (!ConStr.EndsWith(";"))
                ConStr += ";";
            ConStr += "Type System Version=SQL Server 2012;";
            this._Adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConStr);
            this._Adapter.Fill(DtColumns);
            foreach (string C in XmlColumns)
            {
                if (ArchiveColumns.ContainsKey(C))
                {
                    if (ArchiveColumns[C].DataType == "timestamp")
                    {
                        continue;
                    }
                    if (SqlColumns.Length > 0)
                        SqlColumns += ", ";
                    SqlColumns += "[" + C + "]";
                }
                else
                {
                    Message += "\r\nColumn " + C + " is missing in table";
                }
            }
            return SqlColumns;
        }

        private string RestoreArchiveValues(System.Data.DataTable DataTable, 
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Archive.Column> ArchiveColumns, 
            System.Collections.Generic.List<string> XmlColumns, 
            System.Data.DataRow R, 
            ref string Message)
        {
            string SqlValues = "";
            foreach(string xC in XmlColumns)
            {
                if (!ArchiveColumns.ContainsKey(xC))
                {
                    if (!R[xC].Equals(System.DBNull.Value) && R[xC].ToString().Length > 0)
                    {
                        Message += "Content of column " + xC + ": " + R[xC].ToString() + " in xml file could not be inserted due do missing column in target table";
                    }
                    continue;
                }
                if (!DataTable.Columns.Contains(xC))
                {
                    continue;
                }
                System.Data.DataColumn C = DataTable.Columns[xC];
                try
                {
                    if (DataTable.Columns[C.ColumnName].DataType.ToString() == "timestamp")
                    {
                        continue;
                    }
                    if (SqlValues.Length > 0)
                        SqlValues += ", ";
                    if (!DataTable.Columns.Contains(C.ColumnName))
                        SqlValues += "NULL";
                    else
                    {
                        if (R[C.ColumnName].Equals(System.DBNull.Value))
                            SqlValues += "NULL";
                        else if (R[C.ColumnName].ToString().Length == 0 && C.AllowDBNull && ArchiveColumns[C.ColumnName].IsNullable)
                            SqlValues += "NULL";
                        else
                        {
                            if (ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("char") > -1 ||
                                ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("text") > -1 ||
                                ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("xml") > -1 ||
                                ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("uniqueidentifier") > -1)
                                SqlValues += "'";

                            if (ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("date") > -1 ||
                                (ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("time") > -1 &&
                                ArchiveColumns[C.ColumnName].DataType.ToLower() != "timestamp"))
                            {
                                string Value = R[C.ColumnName].ToString();
                                System.DateTime Date;
                                if (System.DateTime.TryParse(Value, out Date))
                                {
                                    Value = "CONVERT(datetime, '" + Date.Year.ToString() + "-" + Date.Month.ToString() + "-" + Date.Day.ToString();
                                    if (Date.Hour > 0) Value += " " + Date.Hour.ToString() + ":" + Date.Minute.ToString() + ":" + Date.Second.ToString();
                                    if (Date.Millisecond > 0) Value += "." + Date.Millisecond.ToString();
                                    Value += "', 121)";
                                    SqlValues += Value;
                                }
                                else
                                    SqlValues += "'" + Value + "'";
                            }
                            else if (ArchiveColumns[C.ColumnName].DataType.ToLower() == "geography")// ColumnDict[C.ColumnName].ToLower() == "geography")// || C.DataType.Name == "SqlGeography")
                            {
                                SqlValues += "geography::STGeomFromText('" + R[C.ColumnName].ToString() + "', 4326) ";
                            }
                            else if (ArchiveColumns[C.ColumnName].DataType.ToLower() == "geometry")//C.DataType.Name == "SqlGeometry")
                            {
                                SqlValues += "geometry::STGeomFromText('" + R[C.ColumnName].ToString() + "', 0) ";
                            }
                            else if (ArchiveColumns[C.ColumnName].DataType.ToLower() == "bit")// C.DataType.Name == "Boolean")
                            {
                                SqlValues += "'" + R[C.ColumnName].ToString() + "' ";
                            }
                            else if (ArchiveColumns[C.ColumnName].DataType.ToLower() == "image" || ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("binary") > -1) //C.DataType.Name == "Byte[]")
                            {
                                try
                                {
                                    byte[] bb = (byte[])R[C.ColumnName];
                                    SqlValues += "0x" + BitConverter.ToString(bb).Replace("-", "");
                                }
                                catch (System.Exception ex)
                                {
                                    SqlValues += "'' ";
                                }
                            }
                            else if (R[C.ColumnName].ToString().Length == 0)
                            {
                                SqlValues += "'' ";
                            }
                            else
                                SqlValues += R[C.ColumnName].ToString().Replace("'", "''");


                            if (ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("char") > -1 ||
                                ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("text") > -1 ||
                                ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("xml") > -1 ||
                                ArchiveColumns[C.ColumnName].DataType.ToLower().IndexOf("uniqueidentifier") > -1)
                                SqlValues += "'";
                        }
                    }
                    if (C.ColumnName == "ProjectID" && this.PrimaryKeyColumnList.Count == 1 && this.PrimaryKeyColumnList[0] == "ProjectID")
                    {
                        int i = 0;
                        if (int.TryParse(R[C.ColumnName].ToString(), out i))
                            this._ProjectID = i;
                    }
                }
                catch (Exception ex)
                {
                    Message = "Failed to get column " + C.ColumnName + ": " + ex.Message + "\r\n";
                }
            }
            return SqlValues;
        }

        
        #endregion

        #region Auxillary

        public int DataCount()
        {
            int i = 0;
            try
            {
                if (this._DT != null)
                    i = this._DT.Rows.Count;
            }
            catch (System.Exception ex)
            {
            }
            return i;
        }

        public int LogCount()
        {
            int i = 0;
            try
            {
                i = this._DT_log.Rows.Count;
            }
            catch (System.Exception ex)
            {
            }
            return i;
        }


        /// <summary>
        /// Reset all data holding objects
        /// </summary>
        public void ResetDatatable(bool IncludeDispose = false)
        {
            try
            {
                if (this._DT != null)
                {
                    this._DT.Clear();
                    if (IncludeDispose)
                        this._DT.Dispose();
                    this._DT = null;
                }
                if ( this._DT_log != null)
                {
                    if (IncludeDispose) this._DT_log.Dispose();
                    this._DT_log = null;
                }
                if (this._DS != null)
                {
                    this._DS.Clear();
                    if (IncludeDispose)
                    {
                        try { this._DS.Dispose(); }
                        catch { }
                    }
                    this._DS = null;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #endregion

        #region Database informations

        /// <summary>
        /// Finding all colums that depend on another table via a foreign relation
        /// </summary>
        public void FindColumnsWithForeignRelations()
        {
            string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
            "FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP INNER JOIN " +
            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF INNER JOIN " +
            "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN " +
            "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK INNER JOIN " +
            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON " +
            "FK.COLUMN_NAME = PK.COLUMN_NAME INNER JOIN " +
            "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
            "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  " +
            "PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
            "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') AND " +
            "(TF.TABLE_NAME = '" + this._TableName + "') AND " +
            "(TPK.TABLE_NAME = '" + this._TableName + "')";
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    this.Columns[R[0].ToString()].ForeignRelations.Add(R["ForeignTable"].ToString(), R["ForeignColumn"].ToString());
                }
                foreach (System.Collections.Generic.KeyValuePair<string, Column> KV in this.Columns)
                {
                    if (KV.Value.ForeignRelations.Count == 1)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> kv in KV.Value.ForeignRelations)
                        {
                            KV.Value.ForeignRelationColumn = kv.Value;
                            KV.Value.ForeignRelationTable = kv.Key;
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        /// <summary>
        /// Finding all columns that have a table internal parent child relation
        /// </summary>
        public void FindChildParentColumns()
        {
            string SQL = "SELECT Kc.COLUMN_NAME AS ChildColumn, Kp.COLUMN_NAME AS ParentColumn " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')" +
                "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    this.Columns[R[0].ToString()].ParentColumn = R["ParentColumn"].ToString();
                    this._HasInternalRelations = true;
                }
            }
            catch (System.Exception ex) { }
        }

        private System.Collections.Generic.List<string> _PrimaryKeyColumns;
        /// <summary>
        /// the list of the primary key columns
        /// </summary>
        public System.Collections.Generic.List<string> PrimaryKeyColumnList
        {
            get
            {
                if (this._PrimaryKeyColumns == null)
                {
                    this._PrimaryKeyColumns = new List<string>();
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "SELECT COLUMN_NAME " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
                            "(SELECT * " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                            "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                            this._PrimaryKeyColumns.Add(R[0].ToString());
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._PrimaryKeyColumns;
            }
        }

        private string _IdentityColumn;
        /// <summary>
        /// the name of the identity column if present
        /// </summary>
        public string IdentityColumn
        {
            get
            {
                if (this._IdentityColumn == null)
                {
                    this._IdentityColumn = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Archive.Column> KV in this.Columns)
                    {
                        if (KV.Value.IsIdentity)
                        {
                            this._IdentityColumn = KV.Key;
                            break;
                        }
                    }
                }
                return _IdentityColumn;
            }
            //set { _IdentityColumn = value; }
        }

        private void FindReferencedTables()
        {
            string SQL = "SELECT DISTINCT " + this._ColumnForReferencedTable + " FROM " + this.TableName();
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                SQL = "select case when min(c.name) is null then '' else min(c.name) end from sys.columns c, sys.tables t where c.is_identity = 1 " +
                    "and c.object_id = t.object_id and t.name = '" + this.TableName() + "'";
                string ReferencedColumn = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this._ReferencedTables.Add(R[0].ToString(), ReferencedColumn);
            }
        }
        
        #endregion

    }
}
