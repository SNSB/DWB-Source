using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Data
{
    public class Column
    {

        #region Name & Table

        private string _Name;

        public string Name
        {
            get { return _Name; }
        }

        //private string _DisplayText;

        //public string DisplayText
        //{
        //    get { return _DisplayText; }
        //    set { _DisplayText = value; }
        //}

        private DiversityWorkbench.Data.Table _Table;
        public DiversityWorkbench.Data.Table Table
        {
            get { return _Table; }
            //set { _Table = value; }
        }

        private string _DataType;
        /// <summary>
        /// the data type of the column as defined in the database
        /// </summary>
        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }

        public enum DataTypeBase { text, numeric, date }
        public DataTypeBase DataTypeBasicType
        {
            get 
            {
                switch (this.DataType.ToLower())
                {
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                        return DataTypeBase.date;
                    case "int":
                    case "smallint":
                    case "tinyint":
                    case "bit":
                    case "float":
                        return DataTypeBase.numeric;
                }
                return DataTypeBase.text; 
            }
        }

        private int _DataTypeLength;
        /// <summary>
        /// The length in the definition of the datatype for e.g. character columns as defined in the database
        /// </summary>
        public int DataTypeLength
        {
            get { return _DataTypeLength; }
            set { _DataTypeLength = value; }
        }

        private bool _IsNullable;

        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }

        private string _ColumnDefault;

        public string ColumnDefault
        {
            get { return _ColumnDefault; }
            set { _ColumnDefault = value; }
        }

        #endregion

        #region Relations

        private System.Collections.Generic.Dictionary<string, string> _ForeignRelations;
        /// <summary>
        /// The foreign relations for the column as defined in the database: Key = ForeignTable, Value = ForeignColumn
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ForeignRelations
        {
            get
            {
                if (this._ForeignRelations == null)
                {
                    this._ForeignRelations = new Dictionary<string, string>();
                }
                return this._ForeignRelations;
            }
        }

        private string _ForeignRelationTable;
        /// <summary>
        /// The name of the parent table of a foreign relation as defined in the database and choosed by the user
        /// </summary>
        public string ForeignRelationTable
        {
            get
            {
                if (this._ForeignRelationTable == null)
                {
                    if (this.ForeignRelations.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ForeignRelations)
                            this._ForeignRelationTable = KV.Key;
                    }
                }
                return _ForeignRelationTable;
            }
            set { _ForeignRelationTable = value; }
        }

        private string _ForeignRelationColumn;
        /// <summary>
        /// The name of the column in the foreign relation table as defined in the database and choosed by the user
        /// </summary>
        public string ForeignRelationColumn
        {
            get
            {
                if (this._ForeignRelationColumn == null)
                {
                    if (this.ForeignRelations.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ForeignRelations)
                            this._ForeignRelationColumn = KV.Value;
                    }
                    else
                    {
                        this._ForeignRelationColumn = "";
                    }
                }
                return _ForeignRelationColumn;
            }
            set { _ForeignRelationColumn = value; }
        }

        private string _ParentColumn;
        /// <summary>
        /// Column with a relation to another column within the same table
        /// </summary>
        public string ParentColumn
        {
            get { return _ParentColumn; }
            set { _ParentColumn = value; }
        }

        private System.Data.DataTable _DtForeignRelationTableColumns;
        public System.Data.DataTable getForeignRelationTableColumns()
        {
            if (this._DtForeignRelationTableColumns == null)
            {
                this._DtForeignRelationTableColumns = new System.Data.DataTable();
                string SQL = "SELECT COLUMN_NAME " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                    "WHERE (TABLE_NAME = '" + this.ForeignRelationTable + "') " +
                    "ORDER BY ORDINAL_POSITION";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._DtForeignRelationTableColumns);
            }
            return this._DtForeignRelationTableColumns;
        }

        #endregion

        #region Informations derived from database

        public string SqlForDataColumn(string TableAlias)
        {
            string SQL = this.Name;
            if (TableAlias.Length > 0)
                SQL = TableAlias + ".[" + SQL + "]";
            switch (this.DataType)
            {
                case "datetime":
                    SQL = "convert(varchar(20)," + SQL + ", 120) AS " + this.Name;
                    break;
                case "datetime2":
                    SQL = "convert(varchar(30)," + SQL + ", 121) AS " + this.Name;
                    break;
                case "geometry":
                case "geography":
                    SQL += ".ToString() AS " + this.Name;
                    break;
            }
            return SQL;
        }

        //public bool AllowOrderBy
        //{
        //    get
        //    {
        //        if (this.DataType == "geography") return false;
        //        return true;
        //    }
        //}

        private bool? _IsIdentity;
        /// <summary>
        /// If the column is an identity column where the value will be automatically generated by the database
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                if (this._IsIdentity == null)
                {
                    if (this._DataType != "int")
                        this._IsIdentity = false;
                    else
                    {
                        try
                        {
                            string SQL = "select case when min(c.name) is null then '' else min(c.name) end from sys.columns c, sys.tables t where c.is_identity = 1 " +
                            "and c.object_id = t.object_id and t.name = '" + this._Table.Name + "'";
                            string IdentityColumn = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            if (IdentityColumn.Length > 0 && IdentityColumn == this._Name)
                                this.IsIdentity = true;
                            else
                                this._IsIdentity = false;
                        }
                        catch (System.Exception ex)
                        {
                            this._IsIdentity = false;
                        }
                    }
                }
                return (bool)_IsIdentity;
            }
            set { _IsIdentity = value; }
        }

        //private System.Data.DataTable _DtSource;
        //public System.Data.DataTable DtSource()
        //{
        //    if (this.ForeignRelations.Count == 1)
        //    {
        //        if (this._DtSource == null)
        //        {
        //            this._DtSource = new System.Data.DataTable();
        //            string SQL = "SELECT DISTINCT [" + this.ForeignRelationColumn + "] FROM [" + this.ForeignRelationTable + "] ORDER BY [" + this.ForeignRelationColumn + "]";
        //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(this._DtSource);
        //        }
        //        return this._DtSource;
        //    }
        //    else return null;
        //}
        
        #endregion

        #region Construction

        protected Column()
        {
        }
        
        public Column(/*string DisplayText, */string ColumnName, DiversityWorkbench.Data.Table Table)
        {
            this._Name = ColumnName;
            //if (DisplayText.Length > 0)
            //    this._DisplayText = DisplayText;
            //else this._DisplayText = ColumnName;
            this._Table = Table;
        }
        
        #endregion


    }
}
