using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Archive
{
    public class DataArchive
    {

        #region Parameter and properties

        private string _RootTable;
        /// <summary>
        /// The table where the restriction is set, e.g. ProjectProxy where the ProjectID is filtered for a certain entry
        /// </summary>
        public string RootTable
        {
            get { return _RootTable; }
            set { _RootTable = value; }
        }

        public static string PlaceHolderQueryStringTempID { get { return "#ID#"; } }

        private string _ProtocolColumn;
 
        private string _DisplayColumn;

        public string DisplayColumn
        {
            get { return _DisplayColumn; }
            //set { _DisplayColumn = value; }
        }

        private string _ErrorMessage;
        public void ErrorMessageAdd(string Error)
        {
            if (this._ErrorMessage == null)
                this._ErrorMessage = "";
            this._ErrorMessage += Error + "\r\n";
        }

        private string _LastChangesInArchiveDataRetrievalFunction;
        public void setLastChangesInArchiveDataRetrievalFunction(string RetrievalFunction)
        {
            this._LastChangesInArchiveDataRetrievalFunction = RetrievalFunction;
        }
        public string LastChangesInArchiveDataRetrievalFunction() { return this._LastChangesInArchiveDataRetrievalFunction; }

        private string _ValueColumn;
        /// <summary>
        /// The value column in the root table
        /// </summary>
        public string ValueColumn
        {
            get { return _ValueColumn; }
            set { _ValueColumn = value; }
        }
        private string _Restriction;

        private string _TempIDTableName;
        private string _TempIDColumnName;

        private string _RestrictionValue;
        /// <summary>
        /// The value to which the root table is restricted, e.g. a ProjectID in table ProjectProxy
        /// </summary>
        public string RestrictionValue
        {
            get { return _RestrictionValue; }
            set { _RestrictionValue = value; }
        }
        private System.Collections.Generic.Dictionary<string, string> _DataTables;

        public System.Collections.Generic.Dictionary<string, string> DataTables
        {
            get 
            {
                if (this._DataTables == null)
                    this._DataTables = new Dictionary<string, string>();
                return _DataTables; 
            }
            //set { _DataTables = value; }
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _MultiParentDataTables;
        /// <summary>
        /// The tables with more than 1 Parent, e.g. Method in DiversityCollection getting data from 3 parents
        /// </summary>
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> MultiParentDataTables
        {
            get
            {
                if (this._MultiParentDataTables == null)
                    this._MultiParentDataTables = new Dictionary<string, System.Collections.Generic.List<string>>();
                return _MultiParentDataTables;
            }
            //set { _DataTables = value; }
        }

        private System.Collections.Generic.Dictionary<string, Archive.Table> _Tables;
        private System.Collections.Generic.Dictionary<string, Archive.Table> _LogTables;

        // For creating incremental archives
        private System.DateTime _IncrementalArchiveStartDate;
        private bool _IsIncrementalArchive = false;

        private bool _includeLog = false;
        public bool IncludeLog { get { return _includeLog; } set { _includeLog = value; } }

        private static Microsoft.Data.SqlClient.SqlConnection _SqlConnection;
        public static Microsoft.Data.SqlClient.SqlConnection SqlConnection(bool Close = false)
        {
            if (Close)
            {
                if (_SqlConnection != null)
                {
                    if (_SqlConnection.State == System.Data.ConnectionState.Open)
                        _SqlConnection.Close();
                    _SqlConnection.Dispose();
                    _SqlConnection = null;
                }
                return null;
            }
            if (_SqlConnection == null)
            {
                _SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                _SqlConnection.Open();
            }
            return _SqlConnection;
        }


        #endregion

        #region Construction

        /// <summary>
        /// Creating a data archive
        /// </summary>
        /// <param name="RootTable">The table where the retrieval of the data starts, e.g. ProjectProxy for a project archive in DiversityCollection</param>
        /// <param name="DisplayColumn">The display column of table where the retrieval of the data starts, e.g. Project for a project archive in DiversityCollection</param>
        /// <param name="ValueColumn">The value column of table where the retrieval of the data starts, e.g. ProjectID for a project archive in DiversityCollection</param>
        /// <param name="Restriction">The restriction of the table where the retrieval of the data starts, e.g. restrictio to ProjectList for a project archive in DiversityCollection</param>
        /// <param name="TempIDTable">The name of the table replaced by the temporary table, e.g. CollectionProject</param>
        /// <param name="TempIDColumn">The name of the column table used replaced the temporary table, e.g. CollectionSpecimenID</param>
        /// <param name="ProtocolColumn">The name of the column in the root table containing the protocol, e.g. ArchiveProtocol in table ProjectProxy</param>
        /// <param name="DataTables">The Dictionary of tables that should be archived 
        /// where the Key is the name of the table and the Value the name of the parent table.
        /// For MultiParentTables the tables must be separated with a |
        /// If any columns should be excluded from the restriction these must be included with [-...] and separated with |
        /// </param>
        public DataArchive(
            string RootTable, 
            string DisplayColumn, 
            string ValueColumn, 
            string Restriction, 
            string TempIDTable,
            string TempIDColumn,
            string ProtocolColumn,
            System.Collections.Generic.Dictionary<string, string> DataTables)
        {
            this._RootTable = RootTable;
            this._DisplayColumn = DisplayColumn;
            this._ValueColumn = ValueColumn;
            this._Restriction = Restriction;
            this._TempIDTableName = TempIDTable;
            this._TempIDColumnName = TempIDColumn;
            this._ProtocolColumn = ProtocolColumn;
            this._DataTables = DataTables;
            System.Collections.Generic.Dictionary<string, string> ExcludedColumns = new Dictionary<string, string>();
            foreach(System.Collections.Generic.KeyValuePair<string, string> KV in this._DataTables)
            {
                if (KV.Value.IndexOf("[-") > -1)
                {
                    ExcludedColumns.Add(KV.Key, KV.Value);//.Substring(KV.Value.IndexOf("[-") + 2).Replace("]", ""));
                    //this._DataTables[KV.Key] = KV.Value.Substring(0, KV.Value.IndexOf("[-"));

                }
                // MultiParentDataTables
                if (KV.Value.IndexOf("|") > -1)
                {
                    System.Collections.Generic.List<string> PP = new List<string>();
                    string Parents = KV.Value;
                    while (Parents.Length > 0)
                    {
                        if (Parents.IndexOf("|") > -1)
                            PP.Add(Parents.Substring(0, Parents.IndexOf("|")));
                        else PP.Add(Parents);
                        if (Parents.IndexOf("|") > -1)
                            Parents = Parents.Substring(Parents.IndexOf("|") + 1);
                        else Parents = "";
                    }
                    this.MultiParentDataTables.Add(KV.Key, PP);
                }
            }
            // Excluded columns
            if (ExcludedColumns.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ExcludedColumns)
                {
                    this._DataTables[KV.Key] = KV.Value.Substring(0, KV.Value.IndexOf("[-"));
                    string ExColumns = KV.Value.Substring(KV.Value.IndexOf("[-") + 2).Replace("]", "");
                    string[] CC = ExColumns.Split(new char[] { '|' });
                    for (int i = 0; i < CC.Length; i++)
                    {
                        this.getTables()[KV.Key].AddColumnExcludedFromRestriction(CC[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Creating a data archive
        /// </summary>
        /// <param name="DataTables">The Dictionary of tables of which a schema should be created
        /// </param>
        public DataArchive(
            System.Collections.Generic.Dictionary<string, string> DataTables)
        {
            this._DataTables = DataTables;
            System.Collections.Generic.Dictionary<string, string> ExcludedColumns = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._DataTables)
            {
                if (KV.Value.IndexOf("[-") > -1)
                {
                    ExcludedColumns.Add(KV.Key, KV.Value);//.Substring(KV.Value.IndexOf("[-") + 2).Replace("]", ""));
                    //this._DataTables[KV.Key] = KV.Value.Substring(0, KV.Value.IndexOf("[-"));

                }
                // MultiParentDataTables
                if (KV.Value.IndexOf("|") > -1)
                {
                    System.Collections.Generic.List<string> PP = new List<string>();
                    string Parents = KV.Value;
                    while (Parents.Length > 0)
                    {
                        if (Parents.IndexOf("|") > -1)
                            PP.Add(Parents.Substring(0, Parents.IndexOf("|")));
                        else PP.Add(Parents);
                        if (Parents.IndexOf("|") > -1)
                            Parents = Parents.Substring(Parents.IndexOf("|") + 1);
                        else Parents = "";
                    }
                    this.MultiParentDataTables.Add(KV.Key, PP);
                }
            }
            // Excluded columns
            if (ExcludedColumns.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ExcludedColumns)
                {
                    this._DataTables[KV.Key] = KV.Value.Substring(0, KV.Value.IndexOf("[-"));
                    string ExColumns = KV.Value.Substring(KV.Value.IndexOf("[-") + 2).Replace("]", "");
                    string[] CC = ExColumns.Split(new char[] { '|' });
                    for (int i = 0; i < CC.Length; i++)
                    {
                        this.getTables()[KV.Key].AddColumnExcludedFromRestriction(CC[i]);
                    }
                }
            }
        }


        #endregion

        #region Data handling

        public System.Collections.Generic.Dictionary<string, Archive.Table> getTables()
        {
            if (this._Tables == null)
            {
                this._Tables = new Dictionary<string, Table>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._DataTables)
                {
                    Archive.Table T = new Table(KV.Key, this, IncludeLog);
                    this._Tables.Add(KV.Key, T);
                }
                if(this._RootTable != null && !this._Tables.ContainsKey(this._RootTable))
                {
                    Archive.Table T = new Table(this._RootTable, this);
                    this._Tables.Add(this._RootTable, T);
                }
            }
            return this._Tables;
        }

        public void SetInrementialArchiveStartDate(System.DateTime DateTime)
        {
            this._IsIncrementalArchive = true;
            this._IncrementalArchiveStartDate = DateTime;
        }

        public System.Data.DataTable DtRootTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable(this._RootTable);
            string SQL = "SELECT " + this._DisplayColumn + " AS Display, " + this._ValueColumn + " AS Value FROM " + this._RootTable + this._Restriction + " ORDER BY " + this._DisplayColumn;
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            return dt;
        }

        /// <summary>
        /// For the creation of SQL statements
        /// </summary>
        /// <param name="TableName">Name of the table</param>
        /// <returns>The SQL clause after FROM including the WHERE clause. Aliases for the tables according to function TableAlias()</returns>
        public string SqlFromWhereClause(string TableName, bool ForLog = false)
        {
            string SQL = "";
            try
            {
                string FromClause = " [" + TableName + "] AS " + this.TableAlias(TableName);
                if (ForLog)
                {
                    FromClause += " INNER JOIN [" + TableName + "_log] AS " + this.TableAlias(TableName) + "_log ON ";
                    DiversityWorkbench.Data.Table table = new Data.Table(TableName);
                    string JoinClause = "";
                    foreach(string PK in table.PrimaryKeyColumnList)
                    {
                        if (JoinClause.Length > 0) JoinClause += " AND ";
                        JoinClause += this.TableAlias(TableName) + "." + PK + " = " + this.TableAlias(TableName) + "_log." + PK;
                    }
                    FromClause += JoinClause + " ";
                }
                string WhereClause = "";
                try
                {
                    string TableAlias = this.TableAlias(TableName);
                    string ParentAlias = "";
                    if (this.DataTables.ContainsKey(TableName))
                        ParentAlias = this.TableAlias(this.DataTables[TableName]);
                    else
                        this.DataTables.Add(TableName, "");

                    if (!this.MultiParentDataTables.ContainsKey(this.DataTables[TableName])) //&&                         !this._ReferencingDataTables.ContainsKey(this.DataTables[TableName]))
                    {
                        WhereClause = " " + this.JoinClause(TableName, TableAlias, this.DataTables[TableName], ParentAlias);
                        if (FromClause.IndexOf(ParentAlias) == -1)
                        {

                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
                System.Collections.Generic.List<Archive.Table> ParentTables = this.ParentTables(TableName);
                if (TableName == this.RootTable)
                {
                    WhereClause += this.TableAlias(TableName) + "." + this.ValueColumn + " = '" + this.RestrictionValue + "'";
                }
                if (ParentTables.Count == 0 && this.MultiParentDataTables.ContainsKey(this.DataTables[TableName]))
                {
                    string Parent = this.DataTables[TableName];
                    string ParentAlias = this.TableAlias(Parent);
                    if (WhereClause.Trim().Length > 0)
                        WhereClause += " AND ";
                    WhereClause += this.JoinClause(TableName, this.TableAlias(TableName), Parent, ParentAlias);
                    if (WhereClause.Trim().Length > 0)
                        WhereClause += " AND ";
                    WhereClause += this.ExistsClause(this.DataTables[TableName]);// "(" + ExistsClause + ")";
                    FromClause += ", [" + this.DataTables[TableName] + "] AS " + this.TableAlias(this.DataTables[TableName]);
                }
                foreach (Archive.Table P in ParentTables)
                {
                    if (P != null && this.DataTables.ContainsKey(P.TableName()))
                    {
                        string Table = P.TableName();
                        string Alias = this.TableAlias(Table);
                        string ParentTable = this._DataTables[Table];
                        string ParentAlias = this.TableAlias(ParentTable);
                        if (this.MultiParentDataTables.ContainsKey(ParentTable))
                        {
                            FromClause += ", [" + Table + "] AS " + Alias;
                            if (FromClause.IndexOf(" AS " + this.TableAlias(ParentTable)) == -1)
                            {
                                FromClause += ", [" + ParentTable + "] AS " + this.TableAlias(ParentTable);
                            }
                            if (WhereClause.IndexOf(" " + this.TableAlias(ParentTable) + ".") == -1)
                            {
                                string Join = this.JoinClause(Table, Alias, ParentTable, ParentAlias);
                                if (Join.Length > 0)
                                {
                                    if (WhereClause.Trim().Length > 0)
                                        WhereClause += " AND ";
                                    WhereClause += Join;
                                }
                            }
                            if (WhereClause.Trim().Length > 0)
                                WhereClause += " AND ";
                            WhereClause += this.ExistsClause(ParentTable);// "(" + ExistsClause + ")";
                        }
                        else
                        {
                            FromClause += ", [" + P.TableName() + "] AS " + this.TableAlias(P.TableName());// AliasParent;
                            string Join = this.JoinClause(Table, Alias, ParentTable, ParentAlias);
                            if (Join.Length > 0)
                            {
                                if (WhereClause.Trim().Length > 0)
                                    WhereClause += " AND ";
                                WhereClause += Join;
                            }
                        }
                    }
                    else if (this.MultiParentDataTables.ContainsKey(TableName))
                    {
                        if (WhereClause.Trim().Length > 0)
                            WhereClause += " AND ";
                        WhereClause += this.ExistsClause(TableName);// "(" + ExistsClause + ")";
                    }
                    else if (P.TableName() == "#" + this._TempIDTableName)
                    {
                        FromClause += ", [" + P.TableName() + "] AS " + this.TableAlias(P.TableName()); ;
                    }
                    else if (P.TableName() == this._RootTable)
                    {
                        FromClause += ", [" + P.TableName() + "] AS " + this.TableAlias(P.TableName()); ;
                    }
                    if (P != null && P.TableName() == this.RootTable && TableName != this.RootTable)
                    {
                        if (WhereClause.Trim().Length > 0)
                            WhereClause += " AND ";
                        WhereClause += this.TableAlias(P.TableName()) + "." + this.ValueColumn + " = '" + this.RestrictionValue + "'";
                    }
                }
                if (WhereClause.Trim().Length > 0) 
                    WhereClause = " WHERE " + WhereClause;

                SQL = FromClause + WhereClause;
                //if (IncludeLog && SQL.IndexOf("#") > -1) SQL = SQL.Replace("#", "");
            }
            catch (System.Exception ex)
            {
            }
            return SQL;
        }

        /// <summary>
        /// Exits clause for tables with more than 1 parent, e.g. Method
        /// </summary>
        /// <param name="MultiParentTable">Name of the table</param>
        /// <returns>The Exists clause - one for every parenttable, combined with OR</returns>
        private string ExistsClause(string MultiParentTable)
        {
            string WhereClause = "";
            if (this.MultiParentDataTables.ContainsKey(MultiParentTable))
            {
                string ExistsClause = "";
                string ParentAlias = this.TableAlias(MultiParentTable);
                foreach (string PT in this.MultiParentDataTables[MultiParentTable])
                {
                    string SqlFromWhere = this.SqlFromWhereClause(PT);
                    string SqlPrefix = "EXISTS (SELECT * FROM ";// [" + MultiParentTable + "] AS " + ParentAlias + ", ";
                    string TabAli = this.TableAlias(PT);
                    string Join = this.JoinClause(MultiParentTable, ParentAlias, PT, TabAli);
                    string SqlPostfix = "";
                    // Markus 24.5.23: Bugfix if where is missing in SqlFromWhere
                    if (Join.Length > 0)
                    {
                        if (SqlFromWhere.ToLower().IndexOf(" where ") > -1)
                            SqlPostfix += " AND ";
                        else
                            SqlPostfix += " WHERE ";
                        SqlPostfix += Join;
                    }
                    string PAlias = this.TableAlias(PT);
                    Archive.Table E = new Table(PT, this);
                    if (ExistsClause.Length > 0)
                        ExistsClause += " OR ";
                    ExistsClause += SqlPrefix + SqlFromWhere + SqlPostfix + ")";
                }
                if (WhereClause.Length > 0)
                    WhereClause += " AND ";
                WhereClause += "(" + ExistsClause + ")";
            }
            return WhereClause;
        }

        public Archive.Table getTable(string TableName)
        {
            if (this._Tables.ContainsKey(TableName))
                return this._Tables[TableName];
            else return null;
        }

        private System.Collections.Generic.Dictionary<string, string> _Alias;
        public string TableAlias(string TableName)
        {
            string A = "";
            try
            {
                int i = 0;
                if (this._Alias == null)
                {
                    this._Alias = new Dictionary<string, string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._DataTables)
                    {
                        this._Alias.Add(KV.Key, "T" + i.ToString());
                        i++;
                    }
                }
                if (TableName.Length > 0 && TableName == "#" + this._TempIDTableName && this._Alias.ContainsKey(this._TempIDTableName))
                {
                    return this._Alias[this._TempIDTableName];
                }
                else if (!this._Alias.ContainsKey(TableName) && TableName.Length > 0)
                    this._Alias.Add(TableName, "T" + this._Alias.Count.ToString());
                if (this._Alias.ContainsKey(TableName))
                    return this._Alias[TableName];
            }
            catch (System.Exception ex)
            {
            }
            return A;
        }

        public System.Collections.Generic.List<Archive.Table> ParentTables(string TableName)
        {
            System.Collections.Generic.List<Archive.Table> List = new List<Table>();
            try
            {
                if (this._DataTables.ContainsKey(TableName))
                {
                    string Parent = this._DataTables[TableName];
                    while (Parent.Length > 0)
                    {
                        if (this.MultiParentDataTables.ContainsKey(Parent))
                        {
                            Parent = "";
                        }
                        //else if (this._ReferencingDataTables.ContainsKey(Parent))
                        //    Parent = "";
                        else
                        {
                            if (Parent == this._TempIDTableName)
                            {
                                List.Add(this.TempIDTable());
                                Parent = "";
                            }
                            else
                            {
                                Archive.Table t = this.getTable(Parent); // Markus 18.8.23: keine null objekte hinzufuegen
                                if(t != null)
                                    List.Add(t);
                                if (this._DataTables.ContainsKey(Parent))
                                    Parent = this._DataTables[Parent];
                                else Parent = "";
                            }
                        }
                    }
                }
                if (TableName == this._TempIDTableName)
                {
                }
            }
            catch (System.Exception ex)
            {
            }
            return List;
        }

        public string JoinClause(string Table, string Alias, string ParentTable, string ParentAlias)
        {
            if (Table.Length == 0 || Alias.Length == 0 || ParentTable.Length == 0 || ParentAlias.Length == 0)
                return "";

            string Join = "";
            try
            {
                string SQL = "";
                Archive.Table T = this.getTable(Table);
                foreach (System.Collections.Generic.KeyValuePair<string, Archive.Column> C in T.Columns)
                {
                    if (T.ColumnsExcludedFromRestriction.Contains(C.Key))
                        continue;
                    if (_TempIDTableName != null && this.TableAlias(_TempIDTableName) == ParentAlias)
                    {
                        if (_TempIDColumnName != C.Key)
                            continue;
                    }
                    if (C.Value.ForeignRelationTable == ParentTable)
                    {
                        if (SQL.Length > 0) SQL += " AND ";
                        SQL += " " + Alias + "." + C.Key + " = " + ParentAlias + "." + C.Value.ForeignRelationColumn;
                    }
                    if (SQL.Length == 0 && C.Value.ForeignRelations.Count > 1)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in C.Value.ForeignRelations)
                        {
                            if (KV.Key == ParentTable)
                            {
                                if (SQL.Length > 0)
                                    SQL += " AND ";
                                SQL += " " + Alias + "." + C.Key + " = " + ParentAlias + "." + KV.Value;
                            }
                        }
                    }
                }
                if (SQL.Length == 0)
                {
                    if (ParentTable.IndexOf("|") > -1)
                    {
                        /// ToDo - hier passt noch was nicht - vermutlich wegen fehlender interner Beziehung der Tabelle - trat in DTN auf
                        string[] pp = ParentTable.Split(new char[] { '|' });
                        foreach(string p in pp)
                        {
                            Archive.Table P = this.getTable(p);
                            if (P != null) // Markus 18.8.23: Bugfix falls Parent Table nicht ermittelt werden kann
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, Archive.Column> C in P.Columns)
                                {
                                    if (C.Value.ForeignRelationTable == Table)
                                    {
                                        // vorerst aufkommentiert bis fehler gefunden
                                        //if (SQL.Length > 0) SQL += " OR "; // " AND "; // Markus 10.8.23: Bugfix - internal restriction with or
                                        //SQL += " " + ParentAlias + "." + C.Key + " = " + Alias + "." + C.Value.ForeignRelationColumn;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Archive.Table P = this.getTable(ParentTable);
                        if (P != null) // Markus 18.8.23: Bugfix falls Parent Table nicht ermittelt werden kann
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, Archive.Column> C in P.Columns)
                            {
                                if (C.Value.ForeignRelationTable == Table)
                                {
                                    if (SQL.Length > 0) SQL += " OR "; // " AND "; // Markus 10.8.23: Bugfix - internal restriction with or
                                    SQL += " " + ParentAlias + "." + C.Key + " = " + Alias + "." + C.Value.ForeignRelationColumn;
                                }
                            }
                        }
                    }
                    if (SQL.Length > 0)
                        SQL =  " ( " + SQL + " ) ";
                }
                Join = SQL;
            }
            catch (System.Exception ex)
            {
            }
            return Join;
        }

        #endregion

        #region Protocol

        private System.Collections.Generic.List<string> _ArchiveProtocol;

        public System.Collections.Generic.List<string> ArchiveProtocol
        {
            get 
            {
                if (this._ArchiveProtocol == null)
                    this._ArchiveProtocol = new List<string>();
                return _ArchiveProtocol; 
            }
            //set { _Protocol = value; }
        }

        public void ArchiveProtocolAdd(string ProtocolText)
        {
            this.ArchiveProtocol.Add(ProtocolText);
        }

        public void DatabaseProtocolWrite(bool KeepOldProtocols)
        {
            try
            {
                string Protocol = "";
                if (KeepOldProtocols)
                    Protocol = "''\r\n\r\n################################################################\r\n\r\n''";
                foreach (string s in this._ArchiveProtocol)
                    Protocol += s.Replace("'", "''") + "\r\n";
                if (this._ErrorMessage != null && this._ErrorMessage.Length > 0)
                    Protocol += "Error: " + this._ErrorMessage.Replace("'", "''") + "\r\n";
                string SQL = "UPDATE R SET R." + this._ProtocolColumn + " = ";
                if (KeepOldProtocols)
                    SQL += " CASE WHEN R." + this._ProtocolColumn + " IS NULL THEN '' ELSE R." + this._ProtocolColumn + " END + ";
                SQL += "'" + Protocol + "' FROM [" + this._RootTable + "] AS R WHERE R." + this._ValueColumn + " = " + this._RestrictionValue;
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (System.Exception ex)
            {
            }
        }

        public void TextProtocolWrite(string ArchiveFolder)
        {
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(ArchiveFolder + "\\Protocol.txt"))
                {
                    sw.WriteLine("Archive for: " + DiversityWorkbench.Settings.ModuleName);
                    //sw.WriteLine("Created at: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLocalTime());
                    //sw.WriteLine("Created by: " + System.Environment.UserName);
                    sw.WriteLine("Server: " + DiversityWorkbench.Settings.DatabaseServer);
                    sw.WriteLine("Database: " + DiversityWorkbench.Settings.DatabaseName);
                    string SQL = "SELECT dbo.Version()";
                    string Version = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Version.Length > 0)
                        sw.WriteLine("Database version: " + Version);
                    sw.WriteLine("SERVERNAME: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select @@SERVERNAME"));
                    sw.WriteLine("BaseURL: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo.BaseURL()"));
                    //sw.WriteLine("LastChanges: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo." + this.LastChangesInArchiveDataRetrievalFunction() + "(" + this.RestrictionValue + ")"));
                    if (this._ErrorMessage != null && this._ErrorMessage.Length > 0)
                        sw.WriteLine("Error: " + this._ErrorMessage);
                    foreach (string s in this._ArchiveProtocol)
                        sw.WriteLine(s);
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "ArchiveData()", ex.Message, DiversityWorkbench.Settings.ModuleName);
            }
        }

        public void DatabaseSchema(string ArchiveFolder)
        {
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(ArchiveFolder + "\\Protocol.txt"))
                {
                    sw.WriteLine("Archive for: " + DiversityWorkbench.Settings.ModuleName);
                    //sw.WriteLine("Created at: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLocalTime());
                    //sw.WriteLine("Created by: " + System.Environment.UserName);
                    sw.WriteLine("Server: " + DiversityWorkbench.Settings.DatabaseServer);
                    sw.WriteLine("Database: " + DiversityWorkbench.Settings.DatabaseName);
                    string SQL = "SELECT dbo.Version()";
                    string Version = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Version.Length > 0)
                        sw.WriteLine("Database version: " + Version);
                    sw.WriteLine("SERVERNAME: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select @@SERVERNAME"));
                    sw.WriteLine("BaseURL: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo.BaseURL()"));
                    //sw.WriteLine("LastChanges: " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("select dbo." + this.LastChangesInArchiveDataRetrievalFunction() + "(" + this.RestrictionValue + ")"));
                    if (this._ErrorMessage != null && this._ErrorMessage.Length > 0)
                        sw.WriteLine("Error: " + this._ErrorMessage);
                    foreach (string s in this._ArchiveProtocol)
                        sw.WriteLine(s);
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormCreateArchive", "ArchiveData()", ex.Message, DiversityWorkbench.Settings.ModuleName);
            }
        }

        #endregion

        #region Temporary table for IDs to speed up query

        #region Connection for temporary IDs - the temporary table exists only within this connection

        private Microsoft.Data.SqlClient.SqlConnection _ConnectionTempIDs;

        public Microsoft.Data.SqlClient.SqlConnection ConnectionTempIDs()
        {
            if (this._ConnectionTempIDs == null)
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    this._ConnectionTempIDs = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                else this._ConnectionTempIDs = null;
            }
            if (this._ConnectionTempIDs != null && this._ConnectionTempIDs.State == System.Data.ConnectionState.Closed)
                this._ConnectionTempIDs.Open();
            return _ConnectionTempIDs;
        }

        public void CloseConnectionTempIDs()
        {
            if (this._ConnectionTempIDs != null)
            {
                if (this._ConnectionTempIDs.State == System.Data.ConnectionState.Open)
                    this._ConnectionTempIDs.Close();
                this._ConnectionTempIDs.Dispose();
            }
        }

        #endregion

        private Archive.Table _TempIDTable;
        private Archive.Table TempIDTable()
        {
            if (this._TempIDTable == null)
            {
                this._TempIDTable = new Table("#" + this._TempIDTableName, this);
                this.InitTempIDs();
            }
            return this._TempIDTable;
        }

        private string _QueryStringTempIDs;
        public void SetQueryStringTempIDs(string QueryStringTempIDs) { this._QueryStringTempIDs = QueryStringTempIDs; }

        private string QueryStringTempIDs
        {
            get
            {
                if (this._QueryStringTempIDs != null && this._QueryStringTempIDs.Length > 0 && this._RestrictionValue != null && this._RestrictionValue.Length > 0)
                {
                    return _QueryStringTempIDs.Replace(DiversityWorkbench.Archive.DataArchive.PlaceHolderQueryStringTempID, this._RestrictionValue);
                }

                string SQL = "";
                SQL = "SELECT DISTINCT " + this.TableAlias(this._TempIDTableName) + "." + this._TempIDColumnName  + " FROM " + this.SqlFromWhereClause(this._TempIDTableName);
                return SQL;
            }
            //set
            //{
            //    this._QueryStringTempIDs = value;
            //}
        }

        public Microsoft.Data.SqlClient.SqlDataAdapter DataAdapterForTempIDs(string SQL)
        {
            if (this.InitTempIDs())
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionTempIDs());
                return ad;
            }
            else return null;
        }

        public bool InitTempIDs(bool Reset = false)
        {
            bool OK = true;
            string Test = this.SqlExecuteScalarForTempIDs("SELECT COUNT(*) FROM #" + this._TempIDTableName);
            if (Test.Length == 0 || Test == "0" || Reset)
            {
                OK = this.SetTempIDs();
            }
            return OK;
        }

        private bool SetTempIDs()
        {
            bool OK = true;
            try
            {
                if (this.ResetTempIdTable())
                {
                    string SQL = "INSERT INTO #" + this._TempIDTableName + " " + this.QueryStringTempIDs;
                    OK = this.SqlExecuteForTempIDs(SQL);
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private bool ResetTempIdTable()
        {
            bool OK = true;
            string SQL = "create table #" + this._TempIDTableName + " (" + this._TempIDColumnName + " int primary key)";
            this.SqlExecuteForTempIDs(SQL);

            // Clear the table
            SQL = "truncate table #" + this._TempIDTableName;
            OK = this.SqlExecuteForTempIDs(SQL);
            return OK;
        }

        private bool SqlExecuteForTempIDs(string SQL)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionTempIDs());
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private string SqlExecuteScalarForTempIDs(string SQL)
        {
            string Result = "";
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionTempIDs());
                Result = C.ExecuteScalar()?.ToString();
            }
            catch (System.Exception ex)
            {
            }
            return Result;
        }

        #endregion

    }
}
