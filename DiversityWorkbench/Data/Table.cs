using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Data
{
    public class Table
    {

        #region Parameter and Properties

        protected string _Name;

        public string Name
        {
            get { return _Name; }
        }

        protected System.Collections.Generic.List<string> _ViewTables;

        public System.Collections.Generic.List<string> ViewTables
        {
            get { if (_ViewTables == null) _ViewTables = new List<string>(); return _ViewTables; }
        }

        protected string _Schema;

        public string Schema
        {
            get
            {
                if (this._Schema == null)
                    this._Schema = "dbo";
                return _Schema;
            }
        }

        private string _Module;

        protected string _ConnectionString;

        protected System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Column> _Columns;

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Column> Columns
        {
            get
            {
                if (this._Columns == null || this._Columns.Count == 0)
                {
                    this._Columns = new Dictionary<string, Column>();
                    string SQL = "SELECT C.COLUMN_NAME, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH, C.COLUMN_DEFAULT, C.IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this.Name.Replace("[", "").Replace("]", "") + "'";// AND C.COLUMN_NAME NOT LIKE 'xx_%'";
                    // Markus 5.5.23: Bugfix if there are more then 1 schemata e.g. in CacheDB include Schema
                    // Markus 23.5.23: Bugfix for missing _schema
                    if (_Schema != null && _Schema.Length > 0)
                        SQL += " AND C.TABLE_SCHEMA = '" + _Schema + "' ";
                    SQL += " ORDER BY C.ORDINAL_POSITION";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);// DiversityWorkbench.Settings.ConnectionString);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Data.Column TC = new Column(R[0].ToString(), this);
                        TC.DataType = R[1].ToString();
                        if (!R[2].Equals(System.DBNull.Value))
                            TC.DataTypeLength = int.Parse(R[2].ToString());
                        if (!R["COLUMN_DEFAULT"].Equals(System.DBNull.Value))
                            TC.ColumnDefault = R["COLUMN_DEFAULT"].ToString();
                        switch (R["IS_NULLABLE"].ToString().ToUpper())
                        {
                            case "NO":
                                TC.IsNullable = false;
                                break;
                            case "YES":
                                TC.IsNullable = true;
                                break;
                        }
                        if (!this._Columns.ContainsKey(R[0].ToString()))
                            this._Columns.Add(R[0].ToString(), TC);
                        else { }
                    }
                }
                return _Columns;
            }
        }

        public System.Collections.Generic.List<string> ColumnList
        {
            get
            {
                System.Collections.Generic.List<string> List = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, Column> KV in this.Columns)
                    List.Add(KV.Key);
                return List;
            }
        }

        #endregion

        #region Construction

        protected Table()
        {
        }

        public Table(string TableName, string ConnectionString = "")
        {
            this._Name = TableName;
            if (ConnectionString.Length == 0)
                this._ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            else
                this._ConnectionString = ConnectionString;
        }

        public Table(string TableName, string Schema, string ConnectionString)
        {
            this._Name = TableName;
            this._Schema = Schema;
            this._ConnectionString = ConnectionString;
        }

        public Table(string TableName, string Schema, string ConnectionString, string Module)
        {
            this._Name = TableName;
            this._Schema = Schema;
            this._ConnectionString = ConnectionString;
            this._Module = Module;
        }

        #endregion

        #region Database informations

        public System.Collections.Generic.Dictionary<string, string> ForeignRelationColumns(string RelatedTable)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, Column> C in this.Columns)
            {
                if (C.Value.ForeignRelationTable == RelatedTable)
                    Dict.Add(C.Value.Name, C.Value.ForeignRelationColumn);
                else if (C.Value.ForeignRelations.ContainsKey(RelatedTable))
                {
                    Dict.Add(C.Value.Name, C.Value.ForeignRelations[RelatedTable]);
                }
            }
            return Dict;
        }

        public enum TableRelation { Parent, Child, Self }

        System.Collections.Generic.Dictionary<string, TableRelation> _RelatedTables;

        public System.Collections.Generic.Dictionary<string, TableRelation> RelatedTables()
        {
            if (this._RelatedTables == null)
            {
                this._RelatedTables = new Dictionary<string, TableRelation>();
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, Column> C in this.Columns)
                    {
                        if (C.Value.ForeignRelationTable != null && !this._RelatedTables.ContainsKey(C.Value.ForeignRelationTable))
                        {
                            if (C.Value.ForeignRelationTable == this.Name)
                                _RelatedTables.Add(this.Name, TableRelation.Self);
                            else
                                _RelatedTables.Add(C.Value.ForeignRelationTable, TableRelation.Parent);
                        }
                    }
                    // Markus 24.5.23: Ensure check if table is a view via getting the PK
                    if (this.PrimaryKeyColumnList.Count > 0)
                    {
                        string SQL = "SELECT DISTINCT TF.TABLE_NAME " +
                            "FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP  " +
                            "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF  " +
                            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME  " +
                            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK  " +
                            "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON FK.COLUMN_NAME = PK.COLUMN_NAME  " +
                            "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME  " +
                            "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION  " +
                            "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY')  " +
                            "AND TF.TABLE_NAME = TPK.TABLE_NAME " +
                            "AND TF.TABLE_NAME ";
                        // Markus 24.5.23: Getting relations of basic table if the object is a view
                        if (ViewTables.Count > 0)
                        {
                            SQL += " IN (";
                            for (int i = 0; i < ViewTables.Count; i++)
                            {
                                if (i > 0) SQL += ", ";
                                SQL += "'" + ViewTables[i] + "'";
                            }
                            SQL += ")";
                        }
                        else SQL += " = '" + this._Name + "'";
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!this._RelatedTables.ContainsKey(R[0].ToString()))
                                this._RelatedTables.Add(R[0].ToString(), TableRelation.Child);
                        }
                    }
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            return this._RelatedTables;
        }

        System.Collections.Generic.Dictionary<string, TableRelation> _ParentTables;

        public System.Collections.Generic.Dictionary<string, TableRelation> ParentTables()
        {
            if (this._ParentTables == null)
            {
                this._ParentTables = new Dictionary<string, TableRelation>();
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, Column> C in this.Columns)
                    {
                        if (C.Value.ForeignRelationTable != null && !this._ParentTables.ContainsKey(C.Value.ForeignRelationTable))
                        {
                            if (C.Value.ForeignRelationTable == this.Name)
                                _ParentTables.Add(this.Name, TableRelation.Self);
                            else
                                _ParentTables.Add(C.Value.ForeignRelationTable, TableRelation.Parent);
                        }
                    }
                    // Markus 24.5.23: Ensure check if table is a view via getting the PK
                    if (this.PrimaryKeyColumnList.Count > 0)
                    {
                        string SQL = "SELECT DISTINCT P.TABLE_NAME " +
                            "FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP  " +
                            "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF  " +
                            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME  " +
                            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK  " +
                            "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON FK.COLUMN_NAME = PK.COLUMN_NAME  " +
                            "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME  " +
                            "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION  " +
                            "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY')  " +
                            "AND TF.TABLE_NAME = TPK.TABLE_NAME " +
                            "AND TF.TABLE_NAME ";
                        // Markus 24.5.23: Getting relations of basic table if the object is a view
                        if (ViewTables.Count > 0)
                        {
                            SQL += " IN (";
                            for (int i = 0; i < ViewTables.Count; i++)
                            {
                                if (i > 0) SQL += ", ";
                                SQL += "'" + ViewTables[i] + "'";
                            }
                            SQL += ")";
                        }
                        else SQL += " = '" + this._Name + "'";
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!this._ParentTables.ContainsKey(R[0].ToString()))
                                this._ParentTables.Add(R[0].ToString(), TableRelation.Child);
                        }
                    }
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            return this._ParentTables;
        }

        public void setForeignRelationsToParent(string ParentTable, System.Collections.Generic.Dictionary<string, string> Columns)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Columns)
            {
                this._Columns[KV.Key].ForeignRelationColumn = KV.Value;
                this._Columns[KV.Key].ForeignRelationTable = ParentTable;
                this._Columns[KV.Key].ForeignRelations.Clear();
                this._Columns[KV.Key].ForeignRelations.Add(ParentTable, KV.Value);
            }
        }

        /// <summary>
        /// Finding all colums that depend on another table via a foreign relation
        /// </summary>
        public void FindColumnsWithForeignRelations(bool Local = false)
        {
            try
            {
                System.Data.DataRow[] RR;
                if (Local)
                {
                    if (this._Module == null)
                    {
                        string[] findModule = this._ConnectionString.Split(new char[] { ';' });
                        foreach (string m in findModule)
                        {
                            if (m.ToLower().StartsWith("initial ca"))
                            {
                                this._Module = m;
                                break;
                            }
                        }
                        if (this._Module.IndexOf("Divers") > -1)
                            _Module = _Module.Substring(_Module.IndexOf("Divers"));
                    }
                    RR = DtForeignRelationsForModule(this._Module, this._ConnectionString).Select("TABLE_NAME = '" + this._Name + "'");
                }
                else RR = Table.DtForeignRelations().Select("TABLE_NAME = '" + this._Name + "'");
                if (RR.Length > 0)
                {
                    for (int i = 0; i < RR.Length; i++)
                    {
                        // Markus 23.5.23: Bugfix for missing content
                        string Column = RR[i][0].ToString();
                        if (this.Columns.ContainsKey(Column) && !this.Columns[Column].ForeignRelations.ContainsKey(RR[i]["ForeignTable"].ToString()))
                            this.Columns[Column].ForeignRelations.Add(RR[i]["ForeignTable"].ToString(), RR[i]["ForeignColumn"].ToString());
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, Data.Column> KV in this.Columns)
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
                else
                {
                    //string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
                    //    "FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP INNER JOIN " +
                    //    "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF INNER JOIN " +
                    //    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN " +
                    //    "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK INNER JOIN " +
                    //    "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON " +
                    //    "FK.COLUMN_NAME = PK.COLUMN_NAME INNER JOIN " +
                    //    "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                    //    "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  " +
                    //    "PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                    //    "WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') AND " +
                    //    "(TF.TABLE_NAME = '" + this._Name + "') AND " +
                    //    "(TPK.TABLE_NAME = '" + this._Name + "')";
                    string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
                       "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK " +
                       "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                       "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                       "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME " +
                       "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                       "WHERE TF.CONSTRAINT_TYPE = 'FOREIGN KEY' AND (TF.TABLE_NAME = '" + this._Name + "')";
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!this.Columns[R[0].ToString()].ForeignRelations.ContainsKey(R["ForeignTable"].ToString()))
                                this.Columns[R[0].ToString()].ForeignRelations.Add(R["ForeignTable"].ToString(), R["ForeignColumn"].ToString());
                        }
                        foreach (System.Collections.Generic.KeyValuePair<string, Data.Column> KV in this.Columns)
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
                        this._RelatedTables = null;
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
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
                "WHERE (Kc.TABLE_NAME = '" + this._Name + "') AND (Kp.TABLE_NAME = '" + this._Name + "')" +
                "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    this.Columns[R[0].ToString()].ParentColumn = R["ParentColumn"].ToString();
                    //this._HasInternalRelations = true;
                }
            }
            catch (System.Exception ex) { }
        }

        protected System.Collections.Generic.List<string> _PrimaryKeyColumns;
        /// <summary>
        /// the list of the primary key columns
        /// </summary>
        public System.Collections.Generic.List<string> PrimaryKeyColumnList
        {
            get
            {
                if (this._PrimaryKeyColumns == null || this._PrimaryKeyColumns.Count == 0)
                {
                    this._PrimaryKeyColumns = new List<string>();
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "SELECT COLUMN_NAME " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE (TABLE_NAME = '" + this._Name + "') " +
                            "AND TABLE_SCHEMA = '" + this.Schema + "' " +
                            "AND (EXISTS " +
                            "(SELECT * " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                            "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString); // DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {
                            // Check if it is a view and number of base tables
                            SQL = "select count(*) from INFORMATION_SCHEMA.VIEW_TABLE_USAGE V inner join INFORMATION_SCHEMA.TABLES T ON V.TABLE_NAME = T.TABLE_NAME " +
                                "AND T.TABLE_TYPE = 'BASE TABLE' " +
                                "AND (V.VIEW_NAME = '" + this._Name + "') " +
                                "AND V.TABLE_SCHEMA = '" + this.Schema + "' ";
                            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            int i = 0;
                            if (int.TryParse(Result, out i) && i > 0)
                            {
                                SQL = "select V.TABLE_NAME from INFORMATION_SCHEMA.VIEW_TABLE_USAGE V inner join INFORMATION_SCHEMA.TABLES T ON V.TABLE_NAME = T.TABLE_NAME " +
                                    "AND T.TABLE_TYPE = 'BASE TABLE' " +
                                    "AND (V.VIEW_NAME = '" + this._Name + "') " +
                                    "AND V.TABLE_SCHEMA = '" + this.Schema + "' ";
                                // if there are several base tables try to correlate view with base table
                                if (this._Name.IndexOf("_") > -1 && i > 1)
                                {
                                    SQL += "AND T.TABLE_NAME LIKE '%" + this._Name.Substring(0, this._Name.IndexOf("_")) + "%' ";
                                }
                                System.Data.DataTable dtBase = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                                foreach (System.Data.DataRow R in dtBase.Rows)
                                    ViewTables.Add(R[0].ToString());
                                if (ViewTables.Count > 0 && ViewTables[0].Length > 0)
                                {
                                    ad.SelectCommand.CommandText = "SELECT COLUMN_NAME " +
                                        "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                                        "WHERE (TABLE_NAME = '" + ViewTables[0] + "') " +
                                        "AND TABLE_SCHEMA = '" + this.Schema + "' " +
                                        "AND (EXISTS " +
                                        "(SELECT * " +
                                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                                        "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                                        "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                                    ad.Fill(dt);
                                }
                            }
                        }
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!this._PrimaryKeyColumns.Contains(R[0].ToString()))
                                this._PrimaryKeyColumns.Add(R[0].ToString());
                            else { }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._PrimaryKeyColumns;
            }
        }

        public void setPK(System.Collections.Generic.List<string> PK) { this._PrimaryKeyColumns = PK; }

        protected string _IdentityColumn;
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
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> KV in this.Columns)
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
        }

        public void setIdentity(string Identity) { this._IdentityColumn = Identity; }

        private string _Description;
        public string Description
        {
            get
            {
                if (this._Description == null)
                {
                    string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                        " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + this._Name + "', ";
                    SQL += " default, NULL";
                    SQL += ") WHERE name =  'MS_Description'";
                    this._Description = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                return this._Description;
            }
        }

        public string JoinClause(string TableAlias, string TableAliasJoin, string JoinTable)
        {
            string SQL = "";
            if (this._ConnectionString != DiversityWorkbench.Settings.ConnectionString)
            {
                this.FindColumnsWithForeignRelations(true);
            }
            if (this.ForeignRelationColumns(JoinTable).Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ForeignRelationColumns(JoinTable))
                {
                    if (SQL.Length > 0) SQL += " AND ";
                    SQL += TableAlias + "." + KV.Key + " = " + TableAliasJoin + "." + KV.Value;
                }
            }
            else
            {
                Table join = new Table(JoinTable, this._ConnectionString);
                join.FindColumnsWithForeignRelations(true);
                if (join.ForeignRelationColumns(this.Name).Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in join.ForeignRelationColumns(this.Name))
                    {
                        if (SQL.Length > 0) SQL += " AND ";
                        SQL += TableAlias + "." + KV.Key + " = " + TableAliasJoin + "." + KV.Value;
                    }
                }
            }
            //else
            //{
            //    if (this.ForeignRelationColumns(JoinTable).Count > 0)
            //    {
            //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ForeignRelationColumns(JoinTable))
            //        {

            //        }
            //    }
            //    else
            //    {
            //        Table join = new Table(JoinTable, this._ConnectionString);
            //        if (join.ForeignRelationColumns(this.Name).Count > 0)
            //        {
            //            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in join.ForeignRelationColumns(this.Name))
            //            {

            //            }
            //        }
            //    }
            //}
            return SQL;
        }

        private System.Data.DataTable _DtForeignRelationsLocal;

        public System.Data.DataTable DtForeignRelationsLocal()
        {
            if (_DtForeignRelationsLocal == null)
            {
                //string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn, TF.TABLE_NAME " +
                //"FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP INNER JOIN " +
                //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF INNER JOIN " +
                //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN " +
                //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK INNER JOIN " +
                //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON " +
                //"FK.COLUMN_NAME = PK.COLUMN_NAME INNER JOIN " +
                //"INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                //"INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  " +
                //"PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                //"WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') AND (TF.TABLE_NAME = TPK.TABLE_NAME)";

                // Markus 31.5.23: Optimiert mit Bing
                string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn, TF.TABLE_NAME " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK " +
                    "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                    "WHERE TF.CONSTRAINT_TYPE = 'FOREIGN KEY'";

                try
                {
                    string message = "";
                    _DtForeignRelationsLocal = new System.Data.DataTable();
                    // Markus 10.6.22: ensure result by setting timeout to 0
                    if (!DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtForeignRelationsLocal, this._ConnectionString))
                        _DtForeignRelationsLocal = null;
                    //if (false)//message != "")
                    //{
                    //}
                }
                catch (System.Exception ex)
                {
                    _DtForeignRelationsLocal = null;
                }
            }
            return _DtForeignRelationsLocal;
        }


        private static System.Collections.Generic.Dictionary<string, System.Data.DataTable> _DtForeignRelationsForModules;

        public static System.Data.DataTable DtForeignRelationsForModule(string Module, string ConnectionString)
        {
            if (_DtForeignRelationsForModules == null)
            {
                _DtForeignRelationsForModules = new Dictionary<string, System.Data.DataTable>();// new System.Data.DataTable();
                System.Data.DataTable dataTable = DtForeignRelationsForModule(ConnectionString);
                _DtForeignRelationsForModules.Add(Module, dataTable);
            }
            else if (!_DtForeignRelationsForModules.ContainsKey(Module))
            {
                System.Data.DataTable dataTable = DtForeignRelationsForModule(ConnectionString);
                _DtForeignRelationsForModules.Add(Module, dataTable);
            }
            return _DtForeignRelationsForModules[Module];
        }

        private static System.Data.DataTable DtForeignRelationsForModule(string ConnectionString)
        {
            System.Data.DataTable _Dt = new System.Data.DataTable();// new System.Data.DataTable();
            //string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn, TF.TABLE_NAME " +
            //"FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP INNER JOIN " +
            //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF INNER JOIN " +
            //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN " +
            //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK INNER JOIN " +
            //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON " +
            //"FK.COLUMN_NAME = PK.COLUMN_NAME INNER JOIN " +
            //"INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
            //"INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  " +
            //"PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
            //"WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') AND (TF.TABLE_NAME = TPK.TABLE_NAME)";

            // Markus 31.5.23: Optimiert mit Bing
            string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn, TF.TABLE_NAME " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK " +
                "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                "WHERE TF.CONSTRAINT_TYPE = 'FOREIGN KEY'";
            try
            {
                if (!DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _Dt, ConnectionString))
                    _Dt = null;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return _Dt;
        }


        #endregion

        #region Static

        private static System.Data.DataTable _DtForeignRelations;

        public static System.Data.DataTable DtForeignRelations()
        {
            if (_DtForeignRelations == null)
            {
                //string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn, TF.TABLE_NAME " +
                //"FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP INNER JOIN " +
                //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF INNER JOIN " +
                //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN " +
                //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK INNER JOIN " +
                //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON " +
                //"FK.COLUMN_NAME = PK.COLUMN_NAME INNER JOIN " +
                //"INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                //"INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  " +
                //"PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                //"WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') AND (TF.TABLE_NAME = TPK.TABLE_NAME)";

                // Markus 31.5.23: Optimiert nach ChatGPT
                string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn, TF.TABLE_NAME " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK " +
                    "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME " +
                    "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                    "WHERE TF.CONSTRAINT_TYPE = 'FOREIGN KEY'";
                try
                {
                    string message = "";
                    _DtForeignRelations = new System.Data.DataTable();
                    // Markus 10.6.22: ensure result by setting timeout to 0
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtForeignRelations, ref message, 0);
                    if (message != "")
                    {
                        _DtForeignRelations = null;
                    }
                }
                catch (System.Exception ex)
                {
                    _DtForeignRelations = null;
                }
            }
            return _DtForeignRelations;
        }

        #endregion

    }
}
