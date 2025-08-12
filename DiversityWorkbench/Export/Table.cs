using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Export
{
    public class Table
    {

        #region Properties and parameter

        #region Template

        private string _Template;
        /// <summary>
        /// The alias of the template used to create this table
        /// </summary>
        public string Template
        {
            get 
            {
                if (this._Template == null)
                {
                    if (Export.Exporter.TemplateTableDictionary.ContainsKey(this.TableAlias))
                        this._Template = this.TableAlias;
                    else if (Export.Exporter.TemplateTableDictionary.ContainsKey(this.TableName))
                        this._Template = this.TableName;
                }
                return _Template; 
            }
            set { _Template = value; }
        }
        
        #endregion

        #region Start point

        private bool _IsStartPoint;
        public bool IsStartPoint
        {
            get { return _IsStartPoint; }
            set { _IsStartPoint = value; }
        }
        
        #endregion

        #region Orientation of results

        public enum OrientationOfParallel { InColumns, InLines }
        private OrientationOfParallel _ParallelOrientation;
        /// <summary>
        /// The orientation of parallel the results in the exported data
        /// e.g. in DiversityCollection where several collectors can be added as several columns or additional lines
        /// </summary>
        public OrientationOfParallel ParallelOrientation
        {
            get { return _ParallelOrientation; }
            set { _ParallelOrientation = value; }
        }
        
        #endregion

        #region Position in Hierarchy: Indet in interface

        private int? _Indent;
        // for the interface, how far the controls should be indented to demostrate the hierarchical position of the table
        public int Indent
        {
            get
            {
                if (_Indent == null)
                {
                    if (this.ParentTable == null)
                        this._Indent = 0;
                    else
                        this._Indent = this.ParentTable.Indent + 1;
                }
                return (int)_Indent;
            }
            //set { _HierarchyPosition = value; }
        }

        //private int? _HierarchyPosition;

        //public int HierarchyPosition
        //{
        //    get 
        //    {
        //        if (_HierarchyPosition == null)
        //        {
        //            if (this.ParentTable == null)
        //                this._HierarchyPosition = 0;
        //            else
        //                this._HierarchyPosition = this.ParentTable.HierarchyPosition + 1;
        //        }
        //        return (int)_HierarchyPosition; 
        //    }
        //    //set { _HierarchyPosition = value; }
        //}

        //private string _Position;

        //public string Position
        //{
        //    get
        //    {
        //        if (this.ParentTable == null)
        //            _Position = this._HierarchyPosition.ToString();
        //        else
        //            _Position = this.ParentTable.Position + "." + this._HierarchyPosition.ToString();
        //        return _Position;
        //    }
        //    //set { _Position = value; }
        //}

        //private string _PositionKey;
        //// The key of the table within the whole list of tables, including the type
        //public string PositionKey
        //{
        //    get
        //    {
        //        if (this._PositionKey == null)
        //        {
        //            if (this.ParentTable != null)
        //                _PositionKey = this.ParentTable.PositionKey + "_";
        //            if (this.HierarchyPosition < 10)
        //                _PositionKey += "0";
        //            _PositionKey += this.HierarchyPosition.ToString() + ":";
        //            if (this.ParallelPosition < 10)
        //                _PositionKey += "0";
        //            _PositionKey += this.ParallelPosition.ToString();
        //        }
        //        //string Pos = "";
        //        //if (this.ParentTable != null)
        //        //    Pos = this.ParentTable.PositionKey + ".";
        //        //if (this.HierarchyPosition < 10)
        //        //    Pos += "0";
        //        //Pos += this.HierarchyPosition.ToString() + "-";
        //        //if (this.ParallelPosition < 10)
        //        //    Pos += "0";
        //        //Pos += this.ParallelPosition.ToString();
        //        //if (this.TableAlias.IndexOf('_') > -1 && this._PositionKey != null && !this.TableAlias.EndsWith(this._ParallelPosition.ToString()))
        //        //{
        //        //    ///TODO - geflickt - Ursache noch zu ermitteln
        //        //    string[] PP = this.TableAlias.Split(new char[] { '_' });
        //        //    string P = PP.Last();
        //        //    this._ParallelPosition = int.Parse(P);
        //        //    this._PositionKey = null;
        //        //    this._TableAliasKey = null;
        //        //}
        //        //if (this._PositionKey == null)
        //        //{
        //        //    if (this.TypeOfParallelity != Parallelity.unique)
        //        //        this._PositionKey = this.ParentPositionKey;
        //        //    else
        //        //        this._PositionKey = "";
        //        //    if (this._PositionKey.Length == 0)
        //        //    {
        //        //        this._PositionKey = "_";
        //        //        if (((int)DiversityWorkbench.Import.Step.StepType.Table) < 10)
        //        //            this._PositionKey += "0";
        //        //        this._PositionKey += ((int)DiversityWorkbench.Import.Step.StepType.Table).ToString() + "_";
        //        //    }
        //        //    else
        //        //        this._PositionKey += "_";
        //        //    if (this.SequencePosition < 10)
        //        //        this._PositionKey += "0";
        //        //    this._PositionKey += this.SequencePosition.ToString();
        //        //    this._PositionKey += ":";
        //        //    if (this.ParallelPosition < 10)
        //        //        this._PositionKey += "0";
        //        //    this._PositionKey += this.ParallelPosition.ToString();
        //        //}
        //        //this._PositionKey = Pos;
        //        return _PositionKey;
        //    }
        //    //set { _PositionKey = value; }
        //}

        #endregion

        #region Parallelity

        /// <summary>
        /// unique = only one dataset in this table is exported, e.g. Table CollectionSpecimen in module DC if this table is the starting point
        /// restricted = Query restricts results to one dataset in a table, but there are several parallels, e.g. Table CollectionEventLocalisation with WGS84, Altitude etc.
        /// parallel = several dataset are retrieved from this table. Sorting needed
        /// referenced = several datasets. Sorting needed. Reference restricts datasets from the table, e.g. table Annotation in DC
        /// </summary>
        public enum Parallelity { unique, restricted, parallel, referencing };
        private Parallelity _Parallelity;
        public Parallelity TypeOfParallelity
        { get { return this._Parallelity; } }

        private int? _ParallelPosition;
        /// <summary>
        /// Null if no parallelity possible, otherwise the number of the parallel
        /// </summary>
        public int ParallelPosition
        {
            get
            {
                if (this._ParallelPosition == null)
                    return 1;
                else
                    return (int)_ParallelPosition;
            }
            //set { _ParallelPosition = value; }
        }

        private int? _ReferencedParallelPosition;

        public int? ReferencedParallelPosition
        {
            get 
            {
                if (_ReferencedParallelPosition == null && this._Parallelity == Parallelity.referencing)
                {
                    _ReferencedParallelPosition = 1;
                    foreach (DiversityWorkbench.Export.Table T in DiversityWorkbench.Export.Exporter.SourceTableList())
                    {
                        if (T.TableName == this.TableName && T.ParentTable.TableAlias == this.ParentTable.TableAlias)
                            _ReferencedParallelPosition++;
                    }
                }
                return _ReferencedParallelPosition; 
            }
            //set { _ReferencedParallelPosition = value; }
        }

        public void setParallelPosition(int P)
        {
            if (this._ParallelPosition == null)
            {
                this._ParallelPosition = P;
                if (this._Parallelity == Parallelity.parallel || (this.HasInternalRelations && P > 1))
                {
                    this._DisplayText += this.ParallelityLabel().Replace("_", " ");
                    this._TableAlias = null;
                }
                else if (this._Parallelity == Parallelity.referencing)
                {
                    int i;
                    while (int.TryParse(this._DisplayText[this._DisplayText.Length - 1].ToString(), out i))
                        this._DisplayText = this._DisplayText.Substring(0, this._DisplayText.Length - 1).Trim();
                    if (this.ParentTable.TypeOfParallelity == Parallelity.parallel)
                        this._DisplayText += this.ParentTable.ParallelityLabel().Replace("_", " ");
                    this._DisplayText += " " + this.ReferencedParallelPosition.ToString();
                    this._TableAlias = null;
                }
                else if ((this._Parallelity == Parallelity.restricted || this._Parallelity == Parallelity.referencing) 
                    && this.ParentTable != null && this.ParentTable.TypeOfParallelity == Parallelity.parallel)
                {
                    this._DisplayText += this.ParallelityLabel().Replace("_", " ");
                    this._TableAlias = null;
                }
            }
        }

        public string ParallelityLabel()
        {
            string Label = "";
            if (this.TypeOfParallelity != Parallelity.unique)
            {
                if (this.TypeOfParallelity == Parallelity.referencing)
                {
                    Label = this.ParentTable.ParallelityLabel();
                }
                else if (this.ParentTable != null && this.ParentTable.TypeOfParallelity != Parallelity.unique)
                    Label = this.ParentTable.ParallelityLabel();
                Label += "_" + this.ParallelPosition.ToString();
            }
            else if (this.TypeOfParallelity == Parallelity.unique && this.HasInternalRelations && this.ParallelPosition > 1)
            {
                if (this.ParentTable != null && this.TypeOfParallelity == Parallelity.unique && this.HasInternalRelations)
                    Label = this.ParentTable.ParallelityLabel();
                Label += "_" + this.ParallelPosition.ToString();
            }
            return Label;
        }

        public int NumberOfParallelSourceTables()
        {
            int Nr = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> ST in Export.Exporter.SourceTableDictionary())
            {
                if (ST.Value.ParentTable != null
                    && this.ParentTable != null
                    && ST.Value.ParentTable.TableAlias == this.ParentTable.TableAlias
                    && ST.Value.TableName == this.TableName)
                    Nr++;
            }
            return Nr;
        }

        #endregion

        #region Grouping

        private bool _IsGrouped;

        public bool IsGrouped
        {
            get { return _IsGrouped; }
            set { _IsGrouped = value; }
        }
        
        #endregion

        #region Restriction

        private string _SqlRestriction;
        /// <summary>
        /// If the content of the source table is restricted
        /// e.g. in DiversityCollection where the table CollectionEventLocalisation is restricted to a certain type of localisation system
        /// </summary>
        public string SqlRestriction
        {
            get { return _SqlRestriction; }
            set { _SqlRestriction = value; }
        }

        #endregion

        #region Join

        public enum JoinType { Inner, Left, Outer, Right }
        private JoinType _TypeOfJoin;

        public JoinType TypeOfJoin
        {
            get { return _TypeOfJoin; }
            set { _TypeOfJoin = value; }
        }
        
        private DiversityWorkbench.Export.Table _ParentTable;

        public DiversityWorkbench.Export.Table ParentTable
        {
            get { return _ParentTable; }
            set { _ParentTable = value; }
        }

        public string InternalRelationJoinColumn;

        //private System.Collections.Generic.Dictionary<string, string> _JoinColumn;
        ///// <summary>
        ///// The columns via which the join to the parent table should be performed
        ///// </summary>
        //public System.Collections.Generic.Dictionary<string, string> JoinColumn
        //{
        //    get { return _JoinColumn; }
        //    set { _JoinColumn = value; }
        //}

        private System.Collections.Generic.List<string> _PotentialJoinTableNames;

        public System.Collections.Generic.List<string> PotentialJoinTableNames
        {
            get 
            {
                if (this._PotentialJoinTableNames == null)
                {
                    this._PotentialJoinTableNames = new List<string>();
                    foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.TableColumn> KV in this.TableColumns)
                    {
                        if (KV.Value.ForeignRelations.Count > 0)
                        {
                            foreach(System.Collections.Generic.KeyValuePair<string, string> KVforeign in KV.Value.ForeignRelations)
                            {
                                if (!this._PotentialJoinTableNames.Contains(KVforeign.Key))
                                    this._PotentialJoinTableNames.Add(KVforeign.Key);
                            }
                        }

                        //if (KV.Value.ForeignRelationTable != null
                        //    && KV.Value.ForeignRelationTable.Length > 0
                        //    && !this._PotentialJoinTableNames.Contains(KV.Value.ForeignRelationTable))
                        //    this._PotentialJoinTableNames.Add(KV.Value.ForeignRelationTable);
                    }
                }
                return _PotentialJoinTableNames; 
            }
            //set { _PotentialJoinTableNames = value; }
        }


        private System.Collections.Generic.List<string> _PotentialDependentTableNames;

        public System.Collections.Generic.List<string> PotentialDependentTableNames
        {
            get
            {
                if (this._PotentialDependentTableNames == null)
                {
                    this._PotentialDependentTableNames = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.TableColumn> KV in this.TableColumns)
                    {
                        if (KV.Value.ForeignRelations.Count > 0)
                        {
                            foreach(System.Collections.Generic.KeyValuePair<string, string> KvForeign in KV.Value.ForeignRelations)
                            {
                                if (!this._PotentialDependentTableNames.Contains(KvForeign.Key))
                                    this._PotentialDependentTableNames.Add(KvForeign.Key);
                            }
                        }

                        //if (KV.Value.ForeignRelationTable != null
                        //    && KV.Value.ForeignRelationTable.Length > 0
                        //    && !this._PotentialDependentTableNames.Contains(KV.Value.ForeignRelationTable))
                        //    this._PotentialDependentTableNames.Add(KV.Value.ForeignRelationTable);
                    }
                }
                return _PotentialDependentTableNames;
            }
            //set { _PotentialJoinTableNames = value; }
        }

        private bool? _HasInternalRelations;

        public bool HasInternalRelations
        {
            get 
            {
                if (this._HasInternalRelations == null)
                {
                    this.FindChildParentColumns();
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> C in this.TableColumns)
                    {
                        if (C.Value.ParentColumn != null && C.Value.ParentColumn.Length > 0)
                        {
                            this._HasInternalRelations = true;
                            break;
                        }
                    }
                    if(this._HasInternalRelations == null)
                        this._HasInternalRelations = false;
                }
                return (bool)_HasInternalRelations; 
            }
            //set { _HasInternalRelations = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _JoinColumns;

        /// <summary>
        /// the columns for joining the values to the parent table
        /// these may differ from those retrieved by the default procedure and can be set here
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> JoinColumns
        {
            get 
            {
                if (this._JoinColumns == null)
                    this._JoinColumns = new Dictionary<string, string>();
                return _JoinColumns; 
            }
            set { _JoinColumns = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _ColumnForeignRelationTables;

        /// <summary>
        /// the lookup tables for the columns if there is more than 1 table a column has a relation to
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ColumnForeignRelationTables
        {
            get
            {
                if (this._ColumnForeignRelationTables == null)
                    this._ColumnForeignRelationTables = new Dictionary<string, string>();
                return _ColumnForeignRelationTables;
            }
            set { _ColumnForeignRelationTables = value; }
        }

        #endregion

        #region Database informations

        /// <summary>
        /// Finding all colums that depend on another table via a foreign relation
        /// </summary>
        public void FindColumnsWithForeignRelations()
        {
            System.Data.DataRow[] RR = Data.Table.DtForeignRelations().Select("TABLE_NAME = '" + this._TableName + "'");
            if (RR.Length > 0)
            {
                for (int i = 0; i < RR.Length; i++)
                {
                    if (!this.TableColumns[RR[i][0].ToString()].ForeignRelations.ContainsKey(RR[i]["ForeignTable"].ToString()))
                        this.TableColumns[RR[i][0].ToString()].ForeignRelations.Add(RR[i]["ForeignTable"].ToString(), RR[i]["ForeignColumn"].ToString());
                }
                foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> KV in this._TableColumns)
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
                        this.TableColumns[R[0].ToString()].ForeignRelations.Add(R["ForeignTable"].ToString(), R["ForeignColumn"].ToString());
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> KV in this.TableColumns)
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
                    this.TableColumns[R[0].ToString()].ParentColumn = R["ParentColumn"].ToString();
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
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.TableColumn> KV in this.TableColumns)
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

        #endregion

        #region Image

        private System.Drawing.Image _Image;
        /// <summary>
        /// The image as shown in the user interface
        /// </summary>
        public System.Drawing.Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }
        
        #endregion

        #region Names

        private string _DisplayText;

        public string DisplayText
        {
            get 
            {
                if (this._DisplayText == null)
                    this._DisplayText = this.TableAlias.Replace("_", " ");
                return _DisplayText; 
            }
            //set { _DisplayText = value; }
        }

        private string _TableName;

        public string TableName
        {
            get { return _TableName; }
            //set { _TableName = value; }
        }

        private string _TableAlias;
        public string TableAlias
        {
            get
            {
                if (this._TableAlias == null)
                {
                    if (this._TableName != null)
                        this._TableAlias = this.TableName;
                    else
                        this._TableAlias = ""; // this should never happen
                    if (this._ParallelPosition != null && this.TypeOfParallelity != Parallelity.unique)
                        this._TableAlias += "_" + this.ParallelityLabel();// this.ParallelPosition.ToString();
                    else if (this._ParallelPosition != null && this.TypeOfParallelity == Parallelity.unique && this.HasInternalRelations && this.ParallelPosition > 1)
                        this._TableAlias += "_" + this.ParallelityLabel();// this.ParallelPosition.ToString();
                    //if (this._ParentTable != null && DiversityWorkbench.Export.Exporter.)
                    //{
                    //    this._TableAlias += DiversityWorkbench.Import.Import.Tables[this._ParentTableAlias].TableAliasKey;
                    //}
                    //else if (this._ParentTableAlias != null && this._ParentTableAlias.Length > 0 && DiversityWorkbench.Import.Import.TemplateTables.ContainsKey(this._ParentTableAlias))
                    //    this._TableAlias += DiversityWorkbench.Import.Import.TemplateTables[this._ParentTableAlias].TableAliasKey;
                    //if (this.TypeOfParallelity != Parallelity.unique)
                    //    this._TableAlias += "_" + this._ParallelPosition.ToString();
                }
                return this._TableAlias;
            }
        }
        
        #endregion

        #region Columns
        
        private System.Collections.Generic.Dictionary<string, Export.TableColumn> _TableColumns;

        public System.Collections.Generic.Dictionary<string, Export.TableColumn> TableColumns
        {
            get
            {
                if (this._TableColumns == null || this._TableColumns.Count == 0)
                {
                    this._TableColumns = new Dictionary<string, TableColumn>();
                    string SQL = "SELECT C.COLUMN_NAME, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this._TableName + "' AND C.COLUMN_NAME NOT LIKE 'xx_%'";
                    if (this._SuppressedColumns != null && this._SuppressedColumns.Count > 0)
                    {
                        foreach (string s in this._SuppressedColumns)
                            SQL += " AND C.COLUMN_NAME <> '" + s + "'";
                    }
                    SQL += " ORDER BY C.ORDINAL_POSITION";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Export.TableColumn TC = new TableColumn(R[0].ToString(), R[0].ToString(), this);
                        TC.DataType = R[1].ToString();
                        if (!R[2].Equals(System.DBNull.Value))
                            TC.DataTypeLength = int.Parse(R[2].ToString());
                        this._TableColumns.Add(R[0].ToString(), TC);
                    }
                }
                if (this._AddedColumns != null && this._AddedColumns.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._AddedColumns)
                    {
                        if (!this._TableColumns.ContainsKey(KV.Key))
                        {
                            Export.TableColumn TC = new TableColumn(KV.Key, KV.Key, this, KV.Value);
                            this._TableColumns.Add(KV.Key, TC);
                        }
                    }
                }
                return _TableColumns;
            }
            set { _TableColumns = value; }
        }

        public System.Collections.Generic.Dictionary<string, string> ModuleConnections()
        {
            System.Collections.Generic.Dictionary<string, string> MC = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.TableColumn> KV in this.TableColumns)
            {
                if (KV.Value.DiversityWorkbenchModule != null && KV.Value.DiversityWorkbenchModule.Length > 0)
                    MC.Add(KV.Key, KV.Value.DiversityWorkbenchModule);
            }
            return MC;
        }

        public void SetSorting(System.Collections.Generic.List<string> SortingColumns)
        {
            int i = 1;
            foreach (string s in SortingColumns)
            {
                if (this.TableColumns.ContainsKey(s))
                {
                    this.TableColumns[s].SortingSequence = i;
                    this.TableColumns[s].SortingType = TableColumn.Sorting.ascending;
                }
                i++;
            }
        }

        public System.Collections.Generic.List<string> SortingColumnList()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.TableColumn> KV in this.TableColumns)
            {
                if (KV.Value.SortingType != TableColumn.Sorting.notsorted)
                    L.Add(KV.Key);
            }
            return L;
        }

        private System.Collections.Generic.List<string> _SuppressedColumns;
        public void setSuppressedColumns(System.Collections.Generic.List<string> ColumnNames)
        {
            this._SuppressedColumns = new List<string>();
            foreach (string s in ColumnNames)
            {
                this._SuppressedColumns.Add(s);
                if (this._TableColumns.ContainsKey(s))
                    this._TableColumns.Remove(s);
            }
        }
        public System.Collections.Generic.List<string> SuppressedColumns()
        { return this._SuppressedColumns; }

        private System.Collections.Generic.Dictionary<string, string> _AddedColumns;
        //public void AddColumn(string DisplayText, string SQL)
        //{
        //    if (this._AddedColumns == null)
        //        this._AddedColumns = new Dictionary<string, string>();
        //    if (!_AddedColumns.ContainsKey(DisplayText))
        //        this._AddedColumns.Add(DisplayText, SQL);
        //}

        public System.Collections.Generic.Dictionary<string, string> AddedColumns()
        { return this._AddedColumns; }
        public void setAddedColumns(System.Collections.Generic.Dictionary<string, string> AddedColumns)
        {
            if (AddedColumns != null)
            {
                this._AddedColumns = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in AddedColumns)
                {
                    this._AddedColumns.Add(KV.Key, KV.Value);
                    if (!this._TableColumns.ContainsKey(KV.Key))
                    {
                        DiversityWorkbench.Export.TableColumn TCol = new DiversityWorkbench.Export.TableColumn(KV.Key, KV.Key, this, KV.Value);
                        this._TableColumns.Add(KV.Key, TCol);
                    }
                }
            }
        }

        #endregion

        #region User control

        private DiversityWorkbench.Export.UserControlTable _UserControlTable;

        public DiversityWorkbench.Export.UserControlTable UserControlTable
        {
            get { return _UserControlTable; }
            set { _UserControlTable = value; }
        }
        
        #endregion

        #region Exporter

        private DiversityWorkbench.Export.Exporter _Exporter;

        public DiversityWorkbench.Export.Exporter Exporter
        {
            get { return _Exporter; }
            set { _Exporter = value; }
        }
        
        #endregion

        #endregion

        #region Construction

        //public Table() { }
        
        public Table(string TableName, string DisplayText, Parallelity TypeOfParallelity, System.Collections.Generic.List<string> SortingColumns, System.Collections.Generic.Dictionary<string, string> ModuleConnections, string Restriction)
        {
            this._TableName = TableName;
            while (DisplayText.EndsWith(" 1"))
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 2);
            this._DisplayText = DisplayText;
            //this._HierarchyPosition = HierarchyPosition;
            this._Parallelity = TypeOfParallelity;
            if (Restriction != null && Restriction.Length > 0)
                this.SqlRestriction = Restriction;
            if (SortingColumns != null)
                this.SetSorting(SortingColumns);
            if (ModuleConnections != null && this.TableColumns.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ModuleConnections)
                {
                    this.TableColumns[KV.Key].DiversityWorkbenchModule = KV.Value;
                }
            }
            this.FindChildParentColumns();
            this.FindColumnsWithForeignRelations();
        }

        public Table(Export.Table TemplateTable, Export.Table ParentTable)
        {
            this._TableName = TemplateTable.TableName;
            this._Parallelity = TemplateTable.TypeOfParallelity;
            this._Image = TemplateTable.Image;
            this.ParentTable = ParentTable;
            if (this._Parallelity == Parallelity.referencing)
            {
                this._DisplayText = this._TableName + " for " + this._ParentTable.DisplayText.ToLower();

                if (this._ParentTable.TypeOfParallelity == Parallelity.parallel)
                {
                    this._DisplayText += " " + this.ParentTable.ParallelityLabel().Replace("_", " ");// this._ParentTable.ParallelityLabel();
                }

                foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in DiversityWorkbench.Export.Exporter.SourceTableDictionary())
                {
                    try
                    {
                        if (KV.Value.TableName == this.TableName && KV.Value.ParentTable.TableAlias == this.ParentTable.TableAlias)
                        {
                            this._ReferencedParallelPosition = KV.Value.ReferencedParallelPosition + 1;
                            this._SqlRestriction = KV.Value._SqlRestriction;
                            if (this.JoinColumns.Count == 0)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVJoin in KV.Value.JoinColumns)
                                    this.JoinColumns.Add(KVJoin.Key, KVJoin.Value);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                //this._DisplayText += " " + this.ReferencedParallelPosition;
            }
            else
                this._DisplayText = TemplateTable._DisplayText.Substring(0, TemplateTable._DisplayText.IndexOf(" ")).Trim();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in TemplateTable.ModuleConnections())
            {
                this.TableColumns[KV.Key].DiversityWorkbenchModule = KV.Value;
            }
            this.FindChildParentColumns();
            this.FindColumnsWithForeignRelations();
        }

        #endregion

        #region Getting the data

        System.Collections.Generic.Dictionary<string, string> _Data;

        /// <summary>
        /// getting the data according to the ID and the parallel position: only 1 dataline retrieved
        /// </summary>
        /// <param name="ID">The ID as derived from the Starttable</param>
        /// <returns></returns>
        public System.Collections.Generic.Dictionary<string, string> GetData(int ID)
        {
            // test

            string SQL = "";
            if (this._Data == null)
            {
                this._Data = new Dictionary<string, string>();

                try
                {
                    if (this.TypeOfParallelity == Parallelity.referencing)
                        SQL = "SELECT TOP " + this.ReferencedParallelPosition.ToString() + " " + this.SqlTableColumns() +
                            " FROM [" + this.TableName + "] T " + this.SqlWhereClause(ID);
                    else
                        SQL = "SELECT TOP " + this.ParallelPosition.ToString() + " " + this.SqlTableColumns() +
                            " FROM [" + this.TableName + "] T " + this.SqlWhereClause(ID);
                    if (this.JoinColumns.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.JoinColumns)
                            SQL += " AND T." + KV.Key + " = T2." + KV.Value;
                    }
                    else if (this.TypeOfParallelity == Parallelity.referencing)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in DiversityWorkbench.Export.Exporter.SourceTableDictionary())
                        {
                            if (KV.Value.TableName == this.TableName && KV.Value.ParentTable != null && KV.Value.ParentTable.TableAlias == this.ParentTable.TableAlias)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVJoin in KV.Value.JoinColumns)
                                    this.JoinColumns.Add(KVJoin.Key, KVJoin.Value);
                            }
                        }
                    }
                    if (this.OrderClause().Length > 0)
                        SQL += " ORDER BY " + this.OrderClause();

                    System.Data.DataTable dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (Exporter.SqlDocumentationActive)
                    {
                        Exporter.addSQL(this.TableAlias, SQL);
                    }

                    // getting the row filter
                    System.Collections.Generic.Dictionary<string, string> RowFilter = new Dictionary<string, string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.TableColumn> KV in this.TableColumns)
                    {
                        if (KV.Value.RowFilter.Length > 0)
                            RowFilter.Add(KV.Key, KV.Value.RowFilter);
                    }

                    if (RowFilter.Count > 0 && dt.Rows.Count > 0)
                    {
                        string Restriction = "";
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in RowFilter)
                        {
                            if (Restriction.Length > 0) Restriction += " AND ";
                            Restriction += KV.Key + " = '" + KV.Value + "'";
                        }
                        System.Data.DataRow[] rr = dt.Select(Restriction, "");
                        if (rr.Length > 0)
                        {
                            foreach (System.Data.DataColumn C in rr[0].Table.Columns)
                            {
                                this._Data.Add(C.ColumnName, rr[0][C.ColumnName].ToString());
                            }
                            foreach (Export.FileColumn FC in this.SelectedFileColumns())
                            {
                                if (this._Data.ContainsKey(FC.TableColumn.ColumnName))
                                {
                                    FC.CurrentValue = this._Data[FC.TableColumn.ColumnName];
                                }
                            }
                        }
                        else
                        {
                            foreach (Export.FileColumn FC in this.SelectedFileColumns())
                            {
                                FC.CurrentValue = "";
                            }
                        }
                    }
                    else if (this.TypeOfParallelity == Parallelity.referencing && dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataColumn C in dt.Columns)
                        {
                            if (dt.Rows.Count >= (int)this.ReferencedParallelPosition)
                                this._Data.Add(C.ColumnName, dt.Rows[(int)this.ReferencedParallelPosition-1][C.ColumnName].ToString());
                        }
                        foreach (Export.FileColumn FC in this.SelectedFileColumns())
                        {
                            if (this._Data.ContainsKey(FC.TableColumn.ColumnName))
                            {
                                FC.CurrentValue = this._Data[FC.TableColumn.ColumnName];
                            }
                            else FC.CurrentValue = "";
                        }
                    }
                    else if ((this.TypeOfParallelity != Parallelity.parallel && dt.Rows.Count > 0) || dt.Rows.Count > this.ParallelPosition - 1)
                    {
                        foreach (System.Data.DataColumn C in dt.Columns)
                        {
                            int Position = 0;
                            if (this.TypeOfParallelity == Parallelity.parallel)
                                Position = this.ParallelPosition - 1;
                            this._Data.Add(C.ColumnName, dt.Rows[Position][C.ColumnName].ToString());
                        }
                        foreach (Export.FileColumn FC in this.SelectedFileColumns())
                        {
                            if (this._Data.ContainsKey(FC.TableColumn.ColumnName))
                            {
                                FC.CurrentValue = this._Data[FC.TableColumn.ColumnName];
                                // because of possible transformation get value
                                //this._Data[FC.TableColumn.ColumnName] = FC.TransformedValue(FC.CurrentValue);
                            }
                        }
                    }
                    else
                    {
                        if (this._Data.Count == 0)
                        {
                            foreach (Export.FileColumn FC in this.SelectedFileColumns())
                            {
                                FC.CurrentValue = "";
                            }
                        }
                        else
                        {
                            foreach (Export.FileColumn FC in this.SelectedFileColumns())
                            {
                                if (this._Data.ContainsKey(FC.TableColumn.ColumnName))
                                {
                                    FC.CurrentValue = "";
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }


            return this._Data;
        }

        private string GetDataSQL(int ID)
        {
            string SQL = "";
            try
            {
                if (this.TypeOfParallelity == Parallelity.referencing)
                    SQL = "SELECT TOP " + this.ReferencedParallelPosition.ToString() + " " + this.SqlTableColumns() +
                        " FROM [" + this.TableName + "] T " + this.SqlWhereClause(ID);
                else
                    SQL = "SELECT TOP " + this.ParallelPosition.ToString() + " " + this.SqlTableColumns() +
                        " FROM [" + this.TableName + "] T " + this.SqlWhereClause(ID);
                if (this.JoinColumns.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.JoinColumns)
                        SQL += " AND T." + KV.Key + " = T2." + KV.Value;
                }
                else if (this.TypeOfParallelity == Parallelity.referencing)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in DiversityWorkbench.Export.Exporter.SourceTableDictionary())
                    {
                        if (KV.Value.TableName == this.TableName && KV.Value.ParentTable != null && KV.Value.ParentTable.TableAlias == this.ParentTable.TableAlias)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVJoin in KV.Value.JoinColumns)
                                this.JoinColumns.Add(KVJoin.Key, KVJoin.Value);
                        }
                    }
                }
                if (this.OrderClause().Length > 0)
                    SQL += " ORDER BY " + this.OrderClause();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return SQL;
        }

        public void ResetData()
        { this._Data = null; }

        /// <summary>
        /// A string that contains all user defined ordering columns and the PK
        /// </summary>
        /// <returns></returns>
        public string OrderClause()
        {
            string OrderBy = "";
            try
            {
                System.Collections.Generic.SortedDictionary<int, string> OrderColumns = new SortedDictionary<int, string>();
                int iOrderSequence = 0;
                if (this._ParallelPosition == 1 || this._ParentTable == null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> KV in this.TableColumns)
                    {
                        if (KV.Value.SortingType != TableColumn.Sorting.notsorted)
                        {
                            if (!OrderColumns.ContainsValue(KV.Value.ColumnName + " ASC") &&
                                !OrderColumns.ContainsValue(KV.Value.ColumnName + " DESC"))
                            {
                                if (OrderColumns.ContainsKey(KV.Value.SortingSequence))
                                {
                                    System.Windows.Forms.MessageBox.Show("In the table " + this.DisplayText + " the column " + KV.Key + " has the already used sorting sequence " + KV.Value.SortingSequence);
                                }
                                else if (!OrderColumns.ContainsValue("T." + KV.Value.ColumnName + " " + KV.Value.SqlSortingTyp))
                                    OrderColumns.Add(KV.Value.SortingSequence, "T." + KV.Value.ColumnName + " " + KV.Value.SqlSortingTyp);
                                else
                                {
                                }
                            }
                            if (KV.Value.SortingSequence > iOrderSequence)
                                iOrderSequence = KV.Value.SortingSequence;
                        }
                    }
                }
                else
                {
                    if (this._ParentTable != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> KV in Export.Exporter.SourceTableDictionary())
                        {
                            if (KV.Value.ParentTable != null &&
                                KV.Value.ParentTable.TableAlias == this._ParentTable.TableAlias && 
                                KV.Value.TableName == this.TableName &&
                                KV.Value.ParallelPosition == 1)
                            {
                                OrderBy = KV.Value.OrderClause();
                                break;
                            }
                        }
                    }
                }
                iOrderSequence++;
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if (!OrderColumns.ContainsValue("T." + s + " ASC")
                        && !OrderColumns.ContainsValue("T." + s + " DESC")
                        && OrderBy.IndexOf("T." + s + " ASC") == -1
                        && OrderBy.IndexOf("T." + s + " DESC") == -1
                        && !OrderColumns.ContainsKey(iOrderSequence))
                        OrderColumns.Add(iOrderSequence, "T." + s + " ASC");
                    iOrderSequence++;
                }
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in OrderColumns)
                {
                    if (OrderBy.Length > 0) OrderBy += ", ";
                    OrderBy += KV.Value;
                }
            }
            catch (System.Exception ex)
            {
            }
            return OrderBy;
        }

        private string SqlTableColumns()
        {
            // For testing
            if (this.TableName == "IdentificationUnit" || this.TableName == "TaxonSynonymyList" || this.TableName == "TaxonOrder" || this.TableName == "TaxonNameListArea" || this.TableName == "TaxonGeography")
            { }

            string SQL = "";
            try
            {
                foreach (string PK in this.PrimaryKeyColumnList)
                {
                    bool PKinColumns = false;
                    foreach (Export.FileColumn FC in this.SelectedFileColumns())
                    {
                        if (FC.TableColumn.ColumnName == PK)
                        {
                            PKinColumns = true;
                            break;
                        }
                    }
                    if (!PKinColumns)
                    {
                        if (SQL.Length > 0) SQL += ", ";
                        SQL += this.TableColumns[PK].SqlForDataColumn("T");
                    }
                }
                foreach (Export.FileColumn FC in this.SelectedFileColumns())
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += FC.TableColumn.SqlForDataColumn("T");
                }
            }
            catch (System.Exception ex)
            {
            }
            // For testing
            if (this.TableName == "IdentificationUnit" || this.TableName == "TaxonSynonymyList" || this.TableName == "TaxonOrder" || this.TableName == "TaxonNameListArea" || this.TableName == "TaxonGeography")
            { }
            return SQL;
        }

        private string SqlWhereClause(int ID)
        {
            System.Collections.Generic.List<int> L = new List<int>();
            L.Add(ID);
            return this.SqlWhereClause(L);
        }

        public string SqlWhereClause(System.Collections.Generic.List<int> IDs)
        {
            System.Collections.Generic.Dictionary<string, string> TableAliasDict = new Dictionary<string, string>();

            string SQL = "";// this.SqlRestriction;
            // seeking for the join if necessary
            string IdList = "";
            foreach (int ID in IDs)
            {
                if (IdList.Length > 0) IdList += ", ";
                IdList += ID.ToString();
            }
            string TableClause = "";
            string ColumnClause = "";
            int i = 1;
            try
            {
                if (this.TableAlias == Export.Exporter.StartTable.TableAlias)
                {
                    ColumnClause = " WHERE T." + Export.Exporter.StartTableIdColumn() + " IN (" + IdList + ")";
                }
                else
                {
                    Export.Table PreviousTable = new Table("", "", Parallelity.parallel, null, null, "");
                    Export.Table.TableRelation PreviousRelation = TableRelation.Start;
                    foreach (System.Collections.Generic.KeyValuePair<Export.Table, TableRelation> KV in this.RelationToStartTable())
                    {
                        string TableAlias = "T" + i.ToString();
                        if (KV.Key.TableAlias == this.TableAlias)
                        {
                            TableAlias = "T";
                        }
                        else
                        {
                            TableClause += ", [" + KV.Key.TableName + "] AS " + TableAlias;
                        }
                        TableAliasDict.Add(KV.Key.TableName, TableAlias);

                        string PreviousTableAlias = "T" + (i - 1).ToString();
                        if (PreviousTable.TableAlias == this.TableAlias)
                            PreviousTableAlias = "T";

                        if (PreviousRelation == TableRelation.Parent)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in KV.Key.TableColumns)
                            {
                                if (TC.Value.ForeignRelations.ContainsKey(PreviousTable.TableName))
                                {
                                    if (ColumnClause.Length == 0)
                                        ColumnClause += " WHERE ";
                                    else
                                        ColumnClause += " AND ";
                                    ColumnClause += PreviousTableAlias + "." + TC.Value.ForeignRelations[PreviousTable.TableName] + " = " + TableAlias + "." + TC.Key;
                                }
                            }
                            if (KV.Value == TableRelation.Start)
                            {
                                if (ColumnClause.Length == 0)
                                    ColumnClause += " WHERE ";
                                else
                                    ColumnClause += " AND ";
                                if (IDs.Count == 1)
                                {
                                    ColumnClause += TableAlias + "." + Export.Exporter.StartTableIdColumn() + " = " + IDs[0].ToString();
                                }
                                else
                                {
                                    ColumnClause += TableAlias + "." + Export.Exporter.StartTableIdColumn() + " IN (" + IdList + ")";
                                }
                            }
                        }
                        else if (PreviousRelation == TableRelation.Child)
                        {
                            System.Collections.Generic.List<string> ParentTableLinkedColumns = new List<string>();
                            foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in PreviousTable.TableColumns)
                            {
                                if (TC.Value.ForeignRelations.ContainsKey(KV.Key.TableName))
                                {
                                    if (TC.Value.Table.HasInternalRelations
                                        && TC.Value.Table.TableName == KV.Key.TableName
                                        && TC.Value.ParentColumn != null
                                        && TC.Value.Table.InternalRelationJoinColumn != TC.Key)
                                        continue;

                                    if (this.HasInternalRelations 
                                        && PreviousTable.TableName == this.TableName 
                                        && TC.Value.ParentColumn != null
                                        && this.InternalRelationJoinColumn != TC.Key)
                                        continue;

                                    if (ParentTableLinkedColumns.Contains(TC.Value.ForeignRelations[KV.Key.TableName]))
                                    {
                                        // do not add a link to the same colum in the parent table from more than 1 column
                                        continue;
                                    }

                                    if (ColumnClause.Length == 0)
                                        ColumnClause += " WHERE ";
                                    else
                                        ColumnClause += " AND ";
                                    ColumnClause += TableAlias + "." + TC.Value.ForeignRelations[KV.Key.TableName] + " = " + PreviousTableAlias + "." + TC.Key;
                                    ParentTableLinkedColumns.Add(TC.Value.ForeignRelations[KV.Key.TableName]);
                                    string Column = TC.Key;
                                    if (TC.Value.ParentColumn != null) Column = TC.Value.ParentColumn;
                                    if (TC.Value.Table.ParentTable._Data != null
                                        && TC.Value.Table.ParentTable._Data.ContainsKey(Column)
                                        && TC.Value.Table.ParentTable._Data[Column].Length > 0)
                                    {
                                        ColumnClause += " AND " + TableAlias + "." + Column + " = '" + TC.Value.Table.ParentTable._Data[Column] + "'";
                                        //ParentTableLinkedColumns.Add(Column);
                                    }
                                    else
                                    {
                                        // Markus 18.01.2024: If no parent data do exist
                                        if (TC.Value.Table.ParentTable._Data.Count == 0 && TC.Value.Table.ParentTable.TableColumns.ContainsKey(Column)) // && TC.Value.Table.ParentTable._Data.ContainsKey(Column))
                                        {
                                            ColumnClause += " AND " + TableAlias + "." + Column + " IS NULL ";
                                        }
                                    }
                                }
                                else
                                {
                                    if (PreviousTable.TypeOfParallelity == Parallelity.referencing && PreviousTable.TableColumns.ContainsKey(TC.Key))
                                    {
                                        if (this.ParentTable.TableColumns.ContainsKey(TC.Key) && this.ParentTable.TableColumns[TC.Key].IsIdentity)
                                        {
                                            if (ColumnClause.Length == 0)
                                                ColumnClause += " WHERE ";
                                            else
                                                ColumnClause += " AND ";
                                            ColumnClause += TableAlias + "." + TC.Key + " = " + PreviousTableAlias + "." + TC.Key;
                                            //ParentTableLinkedColumns.Add(TC.Key);
                                        }
                                    }
                                }
                            }
                            if (KV.Value == TableRelation.Start)
                            {
                                if (ColumnClause.Length == 0)
                                    ColumnClause += " WHERE ";
                                else
                                    ColumnClause += " AND ";
                                if (IDs.Count == 1)
                                {
                                    ColumnClause += TableAlias + "." + Export.Exporter.StartTableIdColumn() + " = " + IDs[0].ToString();
                                }
                                else
                                {
                                    ColumnClause += TableAlias + "." + Export.Exporter.StartTableIdColumn() + " IN (" + IdList + ")";
                                }
                            }
                        }
                        else
                        {
                            if (KV.Value == TableRelation.Child)
                            {
                                if (TableAliasDict.ContainsKey(this.ParentTable.TableName))
                                {
                                    foreach (string PK in this.ParentTable.PrimaryKeyColumnList)
                                    {
                                        if (this.PrimaryKeyColumnList.Contains(PK))
                                        {
                                            if (ColumnClause.Length == 0)
                                                ColumnClause += " WHERE ";
                                            else
                                                ColumnClause += " AND ";
                                            ColumnClause += TableAlias + "." + PK + " = " + this.ParentTable.TableAlias + "." + PK;
                                        }
                                    }
                                }
                            }
                            else if (KV.Value == TableRelation.Parent)
                            {
                                if (PreviousTable.TableName.Length > 0)
                                {
                                }
                            }
                            else
                            {
                                if (ColumnClause.Length == 0)
                                    ColumnClause += " WHERE ";
                                else
                                    ColumnClause += " AND ";
                                if (IDs.Count == 1)
                                {
                                    ColumnClause += TableAlias + "." + Export.Exporter.StartTableIdColumn() + " = " + IDs[0].ToString();
                                }
                                else
                                {
                                    ColumnClause += TableAlias + "." + Export.Exporter.StartTableIdColumn() + " IN (" + IdList + ")";
                                }
                            }
                        }
                        PreviousTable = KV.Key;
                        PreviousRelation = KV.Value;
                        i++;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            // ADD potential missing join for main table
            if (TableAliasDict.ContainsValue("T") && ColumnClause.IndexOf(" T.") == -1)
            {
                if (TableAliasDict.ContainsKey(this.ParentTable.TableName))
                {
                    foreach (string PK in this.ParentTable.PrimaryKeyColumnList)
                    {
                        if (this.PrimaryKeyColumnList.Contains(PK))
                        {
                            if (ColumnClause.Length == 0)
                                ColumnClause += " WHERE ";
                            else
                                ColumnClause += " AND ";
                            ColumnClause += TableAliasDict[this.TableName] + "." + PK + " = " + TableAliasDict[this.ParentTable.TableName] + "." + PK;
                        }
                    }
                }
            }
            SQL = TableClause + ColumnClause;
            if (this.SqlRestriction != null && this.SqlRestriction.Length > 0 && ColumnClause.Length > 0)
                SQL += " AND T." + this.SqlRestriction;
            foreach(System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in this.TableColumns)
            {
                if (TC.Value.Filter != null && TC.Value.Filter.Length > 0)
                    SQL += " AND T." + TC.Key + " = '" + TC.Value.Filter + "'";
            }


            // for test
            if (this.TableName == "Identification")
            {
            }
            // end for test


            return SQL;
        }

        public int NumberOfParallelData()
        {
            int Parallels = 0;
            string SQL = "DECLARE @T table (Number int); " +
                "INSERT INTO @T SELECT COUNT(*) FROM [" + this.TableName + "] T " 
                + this.SqlWhereClause(Export.Exporter.ListOfIDs) 
                + " GROUP BY ";
            int i = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in this.TableColumns)
            {
                if (TC.Value.ForeignRelationTable == Export.Exporter.StartTable.TableAlias
                    && Export.Exporter.StartTable.PrimaryKeyColumnList.Contains(TC.Value.ForeignRelationColumn))
                {
                    if (i > 0) SQL += ", ";
                    SQL += "T." + TC.Key;
                    i++;
                }
                else if (TC.Value.ForeignRelations.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in TC.Value.ForeignRelations)
                    {
                        if (KV.Key == Export.Exporter.StartTable.TableName
                            && Export.Exporter.StartTable.PrimaryKeyColumnList.Contains(KV.Value))
                        {
                            if (i > 0) SQL += ", ";
                            SQL += "T." + TC.Key;
                            i++;
                        }
                    }
                }
            }
            SQL += "; SELECT MAX(Number) FROM @T;";
            int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out Parallels);
            return Parallels;
        }

        public bool CanDetectParallelData()
        {
            bool CanDetect = false;
            if (this.ParentTable != null && this.ParentTable.TableAlias == Export.Exporter.StartTable.TableAlias)
                CanDetect = true;
            return CanDetect;
        }

        #region Relation to the start table
        
        private enum TableRelation { Child, Parent, Start };
        private System.Collections.Generic.Dictionary<DiversityWorkbench.Export.Table, TableRelation> _RelationToStartTable;
        /// <summary>
        /// The relation to the start table
        /// </summary>
        /// <returns>A dictionary containing all tables from the current table to the start table with the relation to the next table</returns>
        private System.Collections.Generic.Dictionary<DiversityWorkbench.Export.Table, TableRelation> RelationToStartTable()
        {
            if (this._RelationToStartTable == null || this._RelationToStartTable.Count == 0)
            {
                this._RelationToStartTable = new Dictionary<Table, TableRelation>();
                if (this == Exporter.StartTable)
                {
                    this._RelationToStartTable.Add(this, TableRelation.Start);
                }
                else
                {
                    try
                    {
                        if (this._ParentTable == Exporter.StartTable) // the current table is a direct child of the start table
                        {
                            this._RelationToStartTable.Add(this, TableRelation.Child);
                            this._RelationToStartTable.Add(this._ParentTable, TableRelation.Start);
                        }
                        else
                        {
                            if (Exporter.StartTable.ParentTable != null && Exporter.StartTable.ParentTable.TableAlias == this.TableAlias) // the current table is the direct parent of the start table
                            {
                                this._RelationToStartTable.Add(this, TableRelation.Parent);
                                this._RelationToStartTable.Add(Exporter.StartTable, TableRelation.Start);
                            }
                            else
                            {
                                bool RelationFound = false;
                                // Try upwards
                                if (this._ParentTable != null)
                                {
                                    DiversityWorkbench.Export.Table T = this._ParentTable;
                                    int i = Export.Exporter.SourceTableList().Count;
                                    while (!RelationFound && i > 0)
                                    {
                                        if (T == Exporter.StartTable)
                                        {
                                            this._RelationToStartTable.Add(T, TableRelation.Start);
                                            RelationFound = true;
                                            break;
                                        }
                                        else
                                            this._RelationToStartTable.Add(T, TableRelation.Parent);

                                        // if the start table could not be found
                                        if (T.ParentTable == null)
                                        {
                                            this._RelationToStartTable.Clear();
                                            break;
                                        }
                                        T = T.ParentTable;
                                        i--;
                                    }
                                }
                                // try down resp. up and down
                                if (!RelationFound)
                                {
                                    // getting the list from the start table to the top
                                    System.Collections.Generic.List<string> StartUpList = new List<string>();
                                    System.Collections.Generic.Dictionary<string, Export.Table> StartUpDict = new Dictionary<string, Table>();

                                    StartUpDict.Add(Exporter.StartTable.TableAlias, Exporter.StartTable);
                                    StartUpList.Add(Exporter.StartTable.TableAlias);

                                    if (Exporter.StartTable.ParentTable != null)
                                    {
                                        StartUpDict.Add(Exporter.StartTable.ParentTable.TableAlias, Exporter.StartTable.ParentTable);
                                        StartUpList.Add(Exporter.StartTable.ParentTable.TableAlias);

                                        DiversityWorkbench.Export.Table T = Exporter.StartTable.ParentTable;
                                        while (T.ParentTable != null)
                                        {
                                            StartUpDict.Add(T.ParentTable.TableAlias, T.ParentTable);
                                            StartUpList.Add(T.ParentTable.TableAlias);

                                            T = T.ParentTable;
                                            if (T == this || T.TableAlias == this.TableAlias)
                                            {
                                                RelationFound = true;
                                                for (int i = StartUpList.Count; i > 0; i--)
                                                {
                                                    if (i > 1 && StartUpList[i - 1] != Exporter.StartTable.TableAlias)
                                                        this._RelationToStartTable.Add(StartUpDict[StartUpList[i - 1]], TableRelation.Parent);
                                                    else
                                                        this._RelationToStartTable.Add(StartUpDict[StartUpList[i - 1]], TableRelation.Start);
                                                }
                                                break;
                                            }
                                        }
                                    }

                                    if (!RelationFound)
                                    {
                                        // getting the list from the table to the top and test if one of this tables is contained in the start table list
                                        System.Collections.Generic.List<Export.Table> TableUpList = new List<Table>();
                                        if (this.ParentTable != null)
                                        {
                                            TableUpList.Add(this);
                                            DiversityWorkbench.Export.Table T = this.ParentTable;
                                            while (!RelationFound)
                                            {
                                                if (StartUpList.Contains(T.TableAlias))
                                                {
                                                    RelationFound = true;
                                                    foreach (Export.Table TP in TableUpList)
                                                    {
                                                        this._RelationToStartTable.Add(TP, TableRelation.Child);
                                                    }
                                                    bool LinkFound = false;
                                                    for (int i = StartUpList.Count; i > 0; i--)
                                                    {
                                                        if (StartUpList[i - 1] == T.TableAlias)
                                                            LinkFound = true;
                                                        if (LinkFound)
                                                        {
                                                            if (StartUpDict[StartUpList[i - 1]].TableAlias == Exporter.StartTable.TableAlias)
                                                                this._RelationToStartTable.Add(StartUpDict[StartUpList[i - 1]], TableRelation.Start);
                                                            else
                                                                this._RelationToStartTable.Add(StartUpDict[StartUpList[i - 1]], TableRelation.Parent);
                                                        }
                                                    }
                                                }
                                                TableUpList.Add(T);
                                                if (T.ParentTable != null)
                                                    T = T.ParentTable;
                                                else
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }

            // for test
            if (this.TableName == "Identification")
            {
            }
            // end for test


            return this._RelationToStartTable;
        }
        
        #endregion

        private System.Collections.Generic.List<Export.FileColumn> SelectedFileColumns()
        {
            System.Collections.Generic.List<Export.FileColumn> FileColums = new List<FileColumn>();
            // getting the relevant columns
            foreach (Export.FileColumn FC in Exporter.FileColumns)
            {
                if (FC.TableColumn.Table == this)
                {
                    FileColums.Add(FC);
                }
            }
            return FileColums;
        }
        
        public System.Data.DataTable DtResult()
        {
            System.Collections.Generic.List<Export.FileColumn> FileColums = new List<FileColumn>();
            // getting the relevant columns
            foreach (Export.FileColumn FC in Exporter.FileColumns)
            {
                if (FC.TableColumn.Table == this)
                {
                    FileColums.Add(FC);
                }
            }
            System.Data.DataTable dt = new System.Data.DataTable();

            return dt;
        }
        
        #endregion

    }
}
